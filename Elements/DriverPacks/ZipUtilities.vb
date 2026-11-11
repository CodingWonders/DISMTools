' ============================================================================
' ZipUtilities.vb
'
' A minimal, dependency-free ZIP compressor/extractor for .NET Framework 2.0.
'
' WHY THIS EXISTS
' ----------------
' .NET Framework 2.0 does NOT include System.IO.Compression.ZipFile or
' ZipArchive (those arrived in .NET 4.5). It DOES, however, include
' System.IO.Compression.DeflateStream (raw DEFLATE, no zlib/gzip header),
' which is exactly the compression algorithm the ZIP file format uses
' internally for "compression method 8". That means a fully valid, standard
' ZIP file can be produced/read on .NET 2.0 by:
'   1. Using DeflateStream or the included LZMA encoder for each file's raw bytes.
'   2. Hand-writing/parsing the surrounding PKZIP container structures
'      (Local File Header, Central Directory Header, End Of Central
'      Directory record) with BinaryWriter/BinaryReader.
'
' Stored and DEFLATE archives are compatible with Windows Explorer, 7-Zip,
' System.IO.Compression.ZipFile on newer frameworks, etc. LZMA entries use
' standard ZIP method 14 and require a reader that supports that method.
'
' STREAMING I/O AND LARGE FILE (ZIP64) SUPPORT
' -----------------------------------------------
' Both compression and extraction move data in fixed-size (80 KB) chunks
' rather than loading whole files into memory, so peak memory use stays
' constant no matter how large a file is.
'
' Because of that, entries and archives that exceed the classic ZIP format's
' 4 GB size / 65,535-entry limits are handled automatically via the ZIP64
' extension (extra fields on oversized entries, plus a ZIP64 End Of Central
' Directory record + locator when the archive itself needs it). Small
' files/archives are completely unaffected -- their bytes are written in the
' original, non-ZIP64 layout.
'
' One consequence of streaming compression: the local file header's
' compressed size and CRC-32 aren't known until the data has actually been
' compressed, so this code writes placeholder zeros, streams the compressed
' bytes, then SEEKS BACK and patches the real values in afterward. This
' requires the output to be a seekable stream (a normal on-disk FileStream
' qualifies, which is what CreateZip uses).
'
' SELF-EXTRACTING ARCHIVES (e.g. HP SoftPAQ .exe files)
' ------------------------------------------------------
' ZipExtractor can read a ZIP that is appended to an EXE. The ZIP end-of-central-
' directory record is found by scanning the tail of the file, and every offset is
' corrected by the size of the stub in front of the ZIP data (see ReadCentralDirectory).
' If no ZIP end-of-central-directory record is present, ZipExtractor can also invoke
' 7zr.exe to extract an embedded 7-Zip archive. Set ZipExtractor.SevenZipExecutablePath
' to its full path, or make 7zr.exe available on PATH. Progress percentages and the
' current file name are forwarded through the same ProgressChanged event.
' Extraction also skips folder entries, refuses entry names that would escape the
' destination folder, honours the UTF-8 name flag, and rejects encrypted entries.
'
' LIMITATIONS (documented so they don't surprise you later)
' -----------------------------------------------------------
'   - Extraction supports Stored (0), Deflate (8), LZMA (14), and the
'     method-33 LZMA2 variant documented by ZipLzmaDecoder.
'   - File names are written as ASCII (no UTF-8 flag). Stick to standard
'     characters in entry names, or extend WriteEntry/ReadCentralDirectory
'     to set the UTF-8 general-purpose bit (0x0800) and use Encoding.UTF8
'     if you need non-ASCII names.
'   - No support for encryption or spanned/multi-disk archives.
'   - ZIP creation supports Stored (0), Deflate (8), and LZMA (14). The
'     included LZMA encoder uses distance-one repetition matches for runs
'     of identical bytes, but does not perform general dictionary matching.
' ============================================================================

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Diagnostics
Imports System.Globalization

''' <summary>
''' Event data describing progress through a compression or extraction operation.
''' </summary>
Public Class ZipProgressEventArgs
    Inherits EventArgs

    Private ReadOnly _currentFile As String
    Private ReadOnly _filesProcessed As Integer
    Private ReadOnly _totalFiles As Integer
    Private ReadOnly _percentage As Integer
    Private ReadOnly _hasFileCounts As Boolean

    Public Sub New(ByVal currentFile As String, ByVal filesProcessed As Integer, ByVal totalFiles As Integer)
        _currentFile = currentFile
        _filesProcessed = filesProcessed
        _totalFiles = totalFiles
        _hasFileCounts = True

        If totalFiles > 0 Then
            _percentage = CInt((filesProcessed / CDbl(totalFiles)) * 100)
        Else
            _percentage = 0
        End If
    End Sub

    ''' <summary>
    ''' Creates progress data for a tool-driven extraction where only percentage
    ''' progress is available and completed-file counts cannot be verified.
    ''' </summary>
    Friend Sub New(ByVal currentFile As String, ByVal percentage As Integer)
        _currentFile = currentFile
        _filesProcessed = 0
        _totalFiles = 0
        _percentage = Math.Max(0, Math.Min(100, percentage))
        _hasFileCounts = False
    End Sub

    ''' <summary>Name of the entry that was just processed (empty string on the final "done" event).</summary>
    Public ReadOnly Property CurrentFile() As String
        Get
            Return _currentFile
        End Get
    End Property

    ''' <summary>Number of files completed so far.</summary>
    Public ReadOnly Property FilesProcessed() As Integer
        Get
            Return _filesProcessed
        End Get
    End Property

    ''' <summary>Total number of files in this operation.</summary>
    Public ReadOnly Property TotalFiles() As Integer
        Get
            Return _totalFiles
        End Get
    End Property

    ''' <summary>
    ''' True when FilesProcessed and TotalFiles are exact counts. External-tool
    ''' progress reports percentages but cannot reliably provide completed-file counts.
    ''' </summary>
    Public ReadOnly Property HasFileCounts() As Boolean
        Get
            Return _hasFileCounts
        End Get
    End Property

    ''' <summary>Overall completion percentage (0-100), suitable for binding directly to a ProgressBar.Value.</summary>
    Public ReadOnly Property Percentage() As Integer
        Get
            Return _percentage
        End Get
    End Property
End Class

''' <summary>Delegate for the ProgressChanged events raised by ZipCompressor and ZipExtractor.</summary>
Public Delegate Sub ZipProgressEventHandler(ByVal sender As Object, ByVal e As ZipProgressEventArgs)

''' <summary>Compression methods supported when creating ZIP archives.</summary>
Public Enum ZipCompressionMethod As UShort
    ''' <summary>Stores entry data without compression.</summary>
    Stored = 0
    ''' <summary>Compresses entry data with DEFLATE.</summary>
    Deflate = 8
    ''' <summary>
    ''' Compresses entry data with ZIP method 14 LZMA. The built-in encoder
    ''' uses adaptive literal coding and distance-one repetition matches,
    ''' but does not perform general dictionary match finding.
    ''' </summary>
    Lzma = 14
End Enum

''' <summary>
''' Signatures, tags, and threshold constants from the PKZIP / ZIP64 (APPNOTE)
''' specification, shared by the compressor and extractor so the two sides can
''' never disagree with each other.
''' </summary>
Friend NotInheritable Class ZipConstants
    Private Sub New()
    End Sub

    Public Const LocalFileHeaderSignature As UInteger = &H4034B50UI
    Public Const CentralDirectoryHeaderSignature As UInteger = &H2014B50UI
    Public Const EndOfCentralDirectorySignature As UInteger = &H6054B50UI
    Public Const Zip64EndOfCentralDirectorySignature As UInteger = &H6064B50UI
    Public Const Zip64EndOfCentralDirectoryLocatorSignature As UInteger = &H7064B50UI

    Public Const LzmaCompressionMethod As UShort = 14US
    Public Const Lzma2CompressionMethod As UShort = 33US

    ''' <summary>Extra-field "tag" identifying a ZIP64 extended information block.</summary>
    Public Const Zip64ExtraFieldTag As UShort = &H1US

    ''' <summary>Placeholder written into a 4-byte field when the real value needs 8 bytes (ZIP64).</summary>
    Public Const UInt32Max As UInteger = &HFFFFFFFFUI

    ''' <summary>Placeholder written into a 2-byte entry-count field when the real count needs more room (ZIP64).</summary>
    Public Const UInt16Max As UShort = &HFFFFUS
End Class

' ----------------------------------------------------------------------------
' Internal record describing one entry, shared between the writer (compressor)
' and reader (extractor) code paths. Sizes/offsets are Long (64-bit) so a
' single in-memory record can represent both classic and ZIP64 entries.
' ----------------------------------------------------------------------------
Friend Class ZipCentralDirectoryEntry
    Public EntryName As String
    Public Crc32 As UInteger
    Public CompressedSize As Long
    Public UncompressedSize As Long
    Public LocalHeaderOffset As Long
    Public CompressionMethod As UShort
    Public GeneralPurposeFlags As UShort
    Public DosTime As UShort
    Public DosDate As UShort
End Class

''' <summary>
''' A read-only stream wrapper that exposes only the next N bytes of an
''' underlying stream, then reports end-of-stream. Used while decompressing so
''' DeflateStream can never read past one entry's compressed data into the
''' next entry's bytes -- DeflateStream is otherwise free to read ahead
''' further than it strictly needs, which would corrupt the read position
''' for whatever comes next in the archive.
''' </summary>
Friend Class BoundedStream
    Inherits Stream

    Private ReadOnly _innerStream As Stream
    Private ReadOnly _totalLength As Long
    Private _bytesRemaining As Long

    Public Sub New(ByVal innerStream As Stream, ByVal length As Long)
        _innerStream = innerStream
        _totalLength = length
        _bytesRemaining = length
    End Sub

    Public Overrides ReadOnly Property CanRead() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property CanSeek() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property CanWrite() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Long
        Get
            Return _totalLength
        End Get
    End Property

    Public Overrides Property Position() As Long
        Get
            Return _totalLength - _bytesRemaining
        End Get
        Set(ByVal value As Long)
            Throw New NotSupportedException("BoundedStream does not support seeking.")
        End Set
    End Property

    Public Overrides Function Read(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer) As Integer
        If _bytesRemaining <= 0L Then
            Return 0
        End If

        Dim toRead As Integer = count
        If CLng(toRead) > _bytesRemaining Then
            toRead = CInt(_bytesRemaining)
        End If

        Dim bytesRead As Integer = _innerStream.Read(buffer, offset, toRead)
        _bytesRemaining -= bytesRead
        Return bytesRead
    End Function

    Public Overrides Sub Flush()
    End Sub

    Public Overrides Function Seek(ByVal offset As Long, ByVal origin As SeekOrigin) As Long
        Throw New NotSupportedException("BoundedStream does not support seeking.")
    End Function

    Public Overrides Sub SetLength(ByVal value As Long)
        Throw New NotSupportedException("BoundedStream does not support resizing.")
    End Sub

    Public Overrides Sub Write(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer)
        Throw New NotSupportedException("BoundedStream is read-only.")
    End Sub
End Class

''' <summary>
''' Standalone CRC-32 (IEEE 802.3 / ZIP variant) implementation, since .NET 2.0
''' has no built-in CRC-32 class. Every ZIP entry stores a CRC-32 of its
''' *uncompressed* data. InitialValue/UpdateCrc32/FinalizeCrc32 let the CRC be
''' computed incrementally, chunk by chunk, alongside streaming compression or
''' decompression instead of requiring the whole buffer up front.
''' </summary>
Public NotInheritable Class Crc32Calculator
    Private Sub New()
    End Sub

    Private Shared ReadOnly Table As UInteger() = BuildTable()

    Private Shared Function BuildTable() As UInteger()
        Dim table As UInteger() = New UInteger(255) {}

        For i As Integer = 0 To 255
            Dim c As UInteger = CUInt(i)
            For j As Integer = 0 To 7
                If (c And 1UI) <> 0UI Then
                    c = &HEDB88320UI Xor (c >> 1)
                Else
                    c = c >> 1
                End If
            Next
            table(i) = c
        Next

        Return table
    End Function

    ''' <summary>Starting value for a new running CRC-32 computation.</summary>
    Public Shared Function InitialValue() As UInteger
        Return &HFFFFFFFFUI
    End Function

    ''' <summary>Folds one more chunk of data into a running CRC-32 value.</summary>
    Public Shared Function UpdateCrc32(ByVal runningCrc As UInteger, ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer) As UInteger
        Dim crc As UInteger = runningCrc

        For i As Integer = offset To offset + count - 1
            Dim tableIndex As Byte = CByte((crc Xor CUInt(buffer(i))) And &HFFUI)
            crc = Table(tableIndex) Xor (crc >> 8)
        Next

        Return crc
    End Function

    ''' <summary>Converts a running CRC-32 value into the final stored/compared value.</summary>
    Public Shared Function FinalizeCrc32(ByVal runningCrc As UInteger) As UInteger
        Return runningCrc Xor &HFFFFFFFFUI
    End Function

    ''' <summary>Convenience one-shot CRC-32 over a complete byte array.</summary>
    Public Shared Function ComputeCrc32(ByVal data As Byte()) As UInteger
        Dim crc As UInteger = InitialValue()
        crc = UpdateCrc32(crc, data, 0, data.Length)
        Return FinalizeCrc32(crc)
    End Function
End Class

''' <summary>
''' Creates standard ZIP archives from a set of files, streaming and compressing
''' each entry with a selected method (Stored, DEFLATE, or LZMA), with automatic
''' ZIP64 support for large files/archives and progress reporting.
''' </summary>
Public Class ZipCompressor

    ''' <summary>Raised after each file has been written to the archive, and once more at 100% when finished.</summary>
    Public Event ProgressChanged As ZipProgressEventHandler
    Private _compressionMethod As ZipCompressionMethod = ZipCompressionMethod.Deflate

    ''' <summary>
    ''' Compression method used by CreateZip overloads that do not specify one.
    ''' Defaults to Deflate for compatibility with existing callers.
    ''' </summary>
    Public Property CompressionMethod() As ZipCompressionMethod
        Get
            Return _compressionMethod
        End Get
        Set(ByVal value As ZipCompressionMethod)
            ValidateCompressionMethod(value)
            _compressionMethod = value
        End Set
    End Property

    ''' <summary>
    ''' Compresses the given files into a new ZIP archive. Entries are named
    ''' using just the file name (no folder structure preserved).
    ''' </summary>
    Public Sub CreateZip(ByVal sourceFiles As String(), ByVal zipFilePath As String)
        CreateZip(sourceFiles, Nothing, zipFilePath)
    End Sub

    ''' <summary>Creates an archive using the requested ZIP compression method.</summary>
    Public Sub CreateZip(ByVal sourceFiles As String(), ByVal zipFilePath As String, ByVal compressionMethod As ZipCompressionMethod)
        CreateZip(sourceFiles, Nothing, zipFilePath, compressionMethod)
    End Sub

    ''' <summary>
    ''' Compresses the given files into a new ZIP archive. If baseFolder is
    ''' supplied, each entry name is the file's path relative to baseFolder
    ''' (with forward slashes, as ZIP requires), preserving folder structure.
    ''' Pass Nothing/empty for baseFolder to store entries as flat file names.
    ''' The instance CompressionMethod property selects the compression method.
    ''' </summary>
    Public Sub CreateZip(ByVal sourceFiles As String(), ByVal baseFolder As String, ByVal zipFilePath As String)
        CreateZip(sourceFiles, baseFolder, zipFilePath, _compressionMethod)
    End Sub

    ''' <summary>
    ''' Compresses the given files using the requested method. LZMA entries use
    ''' the standard ZIP method 14 header and raw LZMA payload.
    ''' </summary>
    Public Sub CreateZip(ByVal sourceFiles As String(), ByVal baseFolder As String, ByVal zipFilePath As String, ByVal compressionMethod As ZipCompressionMethod)
        If sourceFiles Is Nothing OrElse sourceFiles.Length = 0 Then
            Throw New ArgumentException("No source files supplied.", "sourceFiles")
        End If
        ValidateCompressionMethod(compressionMethod)

        Dim entries As New List(Of ZipCentralDirectoryEntry)()

        Using outputStream As New FileStream(zipFilePath, FileMode.Create, FileAccess.Write)
            Using writer As New BinaryWriter(outputStream)

                For fileIndex As Integer = 0 To sourceFiles.Length - 1
                    Dim sourcePath As String = sourceFiles(fileIndex)
                    Dim entryName As String = GetEntryName(sourcePath, baseFolder)

                    If String.IsNullOrEmpty(entryName) Then
                        Dim baseFolderText As String = baseFolder
                        If baseFolderText Is Nothing Then
                            baseFolderText = "<Nothing>"
                        End If
                        Throw New InvalidOperationException("Computed an empty entry name for source file '" & sourcePath & "' (baseFolder='" & baseFolderText & "'). This usually means baseFolder is the file's own path rather than its containing folder, or the file list includes the base folder itself. Writing an archive with a nameless entry was refused.")
                    End If

                    Dim record As ZipCentralDirectoryEntry = WriteEntry(writer, sourcePath, entryName, compressionMethod)
                    entries.Add(record)

                    RaiseEvent ProgressChanged(Me, New ZipProgressEventArgs(entryName, fileIndex + 1, sourceFiles.Length))
                Next

                ' --- Central directory: one header per entry, pointing back at its local header offset ---
                Dim centralDirectoryOffset As Long = outputStream.Position

                For Each record As ZipCentralDirectoryEntry In entries
                    WriteCentralDirectoryHeader(writer, record)
                Next

                Dim centralDirectorySize As Long = outputStream.Position - centralDirectoryOffset

                WriteEndOfCentralDirectory(writer, entries.Count, centralDirectorySize, centralDirectoryOffset)
            End Using
        End Using
    End Sub

    Private Shared Sub ValidateCompressionMethod(ByVal compressionMethod As ZipCompressionMethod)
        If compressionMethod <> ZipCompressionMethod.Stored AndAlso _
           compressionMethod <> ZipCompressionMethod.Deflate AndAlso _
           compressionMethod <> ZipCompressionMethod.Lzma Then
            Throw New ArgumentOutOfRangeException("compressionMethod", "Only Stored, Deflate, and LZMA compression are supported.")
        End If
    End Sub

    Private Function GetEntryName(ByVal sourcePath As String, ByVal baseFolder As String) As String
        If String.IsNullOrEmpty(baseFolder) Then
            Return Path.GetFileName(sourcePath)
        End If

        Dim fullBase As String = Path.GetFullPath(baseFolder)
        Dim fullSource As String = Path.GetFullPath(sourcePath)

        If Not fullBase.EndsWith(Path.DirectorySeparatorChar.ToString()) Then
            fullBase &= Path.DirectorySeparatorChar
        End If

        Dim relative As String = fullSource
        If fullSource.StartsWith(fullBase, StringComparison.OrdinalIgnoreCase) Then
            relative = fullSource.Substring(fullBase.Length)
        End If

        Return relative.Replace(Path.DirectorySeparatorChar, "/"c)
    End Function

    ''' <summary>
    ''' Writes one entry: local header, then the file's contents streamed
    ''' through the selected codec in fixed-size chunks, then seeks back to
    ''' patch in the CRC-32 and compressed size once they're actually known.
    ''' </summary>
    Private Function WriteEntry(ByVal writer As BinaryWriter, ByVal sourcePath As String, ByVal entryName As String, ByVal selectedMethod As ZipCompressionMethod) As ZipCentralDirectoryEntry
        Dim outputStream As Stream = writer.BaseStream
        Dim sourceInfo As New FileInfo(sourcePath)
        Dim uncompressedSize As Long = sourceInfo.Length
        Dim localHeaderOffset As Long = outputStream.Position

        Dim compressionMethod As UShort = CUShort(selectedMethod)
        ' LZMA can expand incompressible data, so reserve a larger safety margin
        ' before deciding whether the local header needs ZIP64 size fields.
        Dim zip64SizeThreshold As Long = CLng(ZipConstants.UInt32Max) - 1024L
        If selectedMethod = ZipCompressionMethod.Lzma Then
            zip64SizeThreshold = CLng(ZipConstants.UInt32Max) - 67108864L
        End If
        Dim needsZip64 As Boolean = (uncompressedSize >= zip64SizeThreshold) OrElse (localHeaderOffset >= CLng(ZipConstants.UInt32Max))

        Dim dosTime As UShort = 0US
        Dim dosDate As UShort = 0US
        GetDosDateTime(File.GetLastWriteTime(sourcePath), dosTime, dosDate)

        Dim nameBytes As Byte() = Encoding.ASCII.GetBytes(entryName)

        writer.Write(ZipConstants.LocalFileHeaderSignature)
        Dim localVersionNeeded As UShort
        If needsZip64 Then
            localVersionNeeded = CUShort(45)
        Else
            localVersionNeeded = CUShort(20)
        End If
        writer.Write(localVersionNeeded)                        ' Version needed to extract
        writer.Write(CUShort(0))                                ' General purpose bit flag
        writer.Write(compressionMethod)
        writer.Write(dosTime)
        writer.Write(dosDate)

        Dim crcFieldPosition As Long = outputStream.Position
        writer.Write(CUInt(0))                                  ' CRC-32 placeholder, patched after compression

        Dim compressedSizeFieldPosition As Long = -1L
        Dim zip64CompressedSizeFieldPosition As Long = -1L

        If needsZip64 Then
            writer.Write(ZipConstants.UInt32Max)                ' Compressed size placeholder (real value lives in extra field)
            writer.Write(ZipConstants.UInt32Max)                ' Uncompressed size placeholder (real value lives in extra field)
        Else
            compressedSizeFieldPosition = outputStream.Position
            writer.Write(CUInt(0))                              ' Compressed size placeholder, patched after compression
            writer.Write(CUInt(uncompressedSize))               ' Uncompressed size is already known up front
        End If

        writer.Write(CUShort(nameBytes.Length))

        If needsZip64 Then
            writer.Write(CUShort(20))                           ' Extra field length: 4-byte header + 16 bytes of data
            writer.Write(nameBytes)
            writer.Write(ZipConstants.Zip64ExtraFieldTag)
            writer.Write(CUShort(16))                           ' Data size: two 8-byte fields
            writer.Write(uncompressedSize)                      ' Known already
            zip64CompressedSizeFieldPosition = outputStream.Position
            writer.Write(CLng(0))                               ' Compressed size placeholder, patched after compression
        Else
            writer.Write(CUShort(0))                            ' Extra field length
            writer.Write(nameBytes)
        End If

        writer.Flush()

        ' --- Stream the file through the selected codec and into the archive. ---
        Dim dataStartPosition As Long = outputStream.Position
        Dim runningCrc As UInteger = Crc32Calculator.InitialValue()

        Using sourceFileStream As New FileStream(sourcePath, FileMode.Open, FileAccess.Read)
            If selectedMethod = ZipCompressionMethod.Deflate Then
                Using deflateStream As New DeflateStream(outputStream, CompressionMode.Compress, True)
                    Dim buffer As Byte() = New Byte(81919) {}
                    Dim bytesRead As Integer = sourceFileStream.Read(buffer, 0, buffer.Length)
                    Do While bytesRead > 0
                        runningCrc = Crc32Calculator.UpdateCrc32(runningCrc, buffer, 0, bytesRead)
                        deflateStream.Write(buffer, 0, bytesRead)
                        bytesRead = sourceFileStream.Read(buffer, 0, buffer.Length)
                    Loop
                End Using
            ElseIf selectedMethod = ZipCompressionMethod.Stored Then
                Dim buffer As Byte() = New Byte(81919) {}
                Dim bytesRead As Integer = sourceFileStream.Read(buffer, 0, buffer.Length)
                Do While bytesRead > 0
                    runningCrc = Crc32Calculator.UpdateCrc32(runningCrc, buffer, 0, bytesRead)
                    outputStream.Write(buffer, 0, bytesRead)
                    bytesRead = sourceFileStream.Read(buffer, 0, buffer.Length)
                Loop
            Else
                ZipLzmaEncoder.Compress(sourceFileStream, outputStream, uncompressedSize, runningCrc)
            End If
        End Using

        writer.Flush()
        Dim compressedSize As Long = outputStream.Position - dataStartPosition
        Dim crc As UInteger = Crc32Calculator.FinalizeCrc32(runningCrc)
        Dim endOfEntryPosition As Long = outputStream.Position

        ' --- Go back and patch in the CRC-32 and compressed size we now know. ---
        outputStream.Seek(crcFieldPosition, SeekOrigin.Begin)
        writer.Write(crc)

        If needsZip64 Then
            outputStream.Seek(zip64CompressedSizeFieldPosition, SeekOrigin.Begin)
            writer.Write(compressedSize)
        Else
            outputStream.Seek(compressedSizeFieldPosition, SeekOrigin.Begin)
            writer.Write(CUInt(compressedSize))
        End If

        writer.Flush()
        outputStream.Seek(endOfEntryPosition, SeekOrigin.Begin)

        Dim record As New ZipCentralDirectoryEntry()
        record.EntryName = entryName
        record.Crc32 = crc
        record.CompressedSize = compressedSize
        record.UncompressedSize = uncompressedSize
        record.LocalHeaderOffset = localHeaderOffset
        record.CompressionMethod = compressionMethod
        record.DosTime = dosTime
        record.DosDate = dosDate

        Return record
    End Function

    Private Sub WriteCentralDirectoryHeader(ByVal writer As BinaryWriter, ByVal record As ZipCentralDirectoryEntry)
        Dim nameBytes As Byte() = Encoding.ASCII.GetBytes(record.EntryName)

        Dim uncompressedOverflow As Boolean = record.UncompressedSize >= CLng(ZipConstants.UInt32Max)
        Dim compressedOverflow As Boolean = record.CompressedSize >= CLng(ZipConstants.UInt32Max)
        Dim offsetOverflow As Boolean = record.LocalHeaderOffset >= CLng(ZipConstants.UInt32Max)
        Dim needsZip64 As Boolean = uncompressedOverflow OrElse compressedOverflow OrElse offsetOverflow

        ' NOTE: every "which value do I write" decision below is spelled out as an
        ' explicit Dim + If/Else rather than a ternary. When the *Overflow flag is
        ' True, the corresponding CUInt/CUShort cast on the real 64-bit value would
        ' itself throw OverflowException -- an If/Else block guarantees that branch
        ' is never even evaluated, unlike a function call such as IIf (which
        ' evaluates both branches unconditionally and would crash exactly when the
        ' overflow guard is needed most).

        Dim versionField As UShort
        If needsZip64 Then
            versionField = CUShort(45)
        Else
            versionField = CUShort(20)
        End If

        writer.Write(ZipConstants.CentralDirectoryHeaderSignature)
        writer.Write(versionField)     ' Version made by
        writer.Write(versionField)     ' Version needed to extract
        writer.Write(CUShort(0))       ' General purpose bit flag
        writer.Write(record.CompressionMethod)
        writer.Write(record.DosTime)
        writer.Write(record.DosDate)
        writer.Write(record.Crc32)

        Dim compressedSizeField As UInteger
        If compressedOverflow Then
            compressedSizeField = ZipConstants.UInt32Max
        Else
            compressedSizeField = CUInt(record.CompressedSize)
        End If
        writer.Write(compressedSizeField)

        Dim uncompressedSizeField As UInteger
        If uncompressedOverflow Then
            uncompressedSizeField = ZipConstants.UInt32Max
        Else
            uncompressedSizeField = CUInt(record.UncompressedSize)
        End If
        writer.Write(uncompressedSizeField)

        writer.Write(CUShort(nameBytes.Length))

        Dim overflowFieldCount As Integer = 0
        If uncompressedOverflow Then overflowFieldCount += 1
        If compressedOverflow Then overflowFieldCount += 1
        If offsetOverflow Then overflowFieldCount += 1

        Dim extraFieldLength As UShort
        If needsZip64 Then
            extraFieldLength = CUShort(4 + (overflowFieldCount * 8))
        Else
            extraFieldLength = CUShort(0)
        End If
        writer.Write(extraFieldLength)

        writer.Write(CUShort(0))       ' File comment length
        writer.Write(CUShort(0))       ' Disk number start
        writer.Write(CUShort(0))       ' Internal file attributes
        writer.Write(CUInt(0))         ' External file attributes

        Dim offsetField As UInteger
        If offsetOverflow Then
            offsetField = ZipConstants.UInt32Max
        Else
            offsetField = CUInt(record.LocalHeaderOffset)
        End If
        writer.Write(offsetField)

        writer.Write(nameBytes)

        If needsZip64 Then
            ' Order matters here and must match the read side: uncompressed, compressed, offset.
            writer.Write(ZipConstants.Zip64ExtraFieldTag)
            writer.Write(CUShort(overflowFieldCount * 8))
            If uncompressedOverflow Then writer.Write(record.UncompressedSize)
            If compressedOverflow Then writer.Write(record.CompressedSize)
            If offsetOverflow Then writer.Write(record.LocalHeaderOffset)
        End If
    End Sub

    Private Sub WriteEndOfCentralDirectory(ByVal writer As BinaryWriter, ByVal entryCount As Integer, ByVal centralDirectorySize As Long, ByVal centralDirectoryOffset As Long)
        Dim needsZip64ForArchive As Boolean = (entryCount >= CInt(ZipConstants.UInt16Max)) OrElse (centralDirectorySize >= CLng(ZipConstants.UInt32Max)) OrElse (centralDirectoryOffset >= CLng(ZipConstants.UInt32Max))

        If needsZip64ForArchive Then
            Dim zip64EocdOffset As Long = writer.BaseStream.Position

            writer.Write(ZipConstants.Zip64EndOfCentralDirectorySignature)
            writer.Write(CLng(44))                    ' Size of remaining record (fixed -- no extensible data sector)
            writer.Write(CUShort(45))                 ' Version made by
            writer.Write(CUShort(45))                 ' Version needed to extract
            writer.Write(CUInt(0))                    ' Number of this disk
            writer.Write(CUInt(0))                    ' Disk with start of central directory
            writer.Write(CLng(entryCount))            ' Total entries on this disk
            writer.Write(CLng(entryCount))            ' Total entries overall
            writer.Write(centralDirectorySize)
            writer.Write(centralDirectoryOffset)

            writer.Write(ZipConstants.Zip64EndOfCentralDirectoryLocatorSignature)
            writer.Write(CUInt(0))                    ' Disk with start of ZIP64 EOCD
            writer.Write(zip64EocdOffset)
            writer.Write(CUInt(1))                    ' Total number of disks
        End If

        Dim entryCountField As UShort
        If entryCount >= CInt(ZipConstants.UInt16Max) Then
            entryCountField = ZipConstants.UInt16Max
        Else
            entryCountField = CUShort(entryCount)
        End If

        Dim centralDirectorySizeField As UInteger
        If centralDirectorySize >= CLng(ZipConstants.UInt32Max) Then
            centralDirectorySizeField = ZipConstants.UInt32Max
        Else
            centralDirectorySizeField = CUInt(centralDirectorySize)
        End If

        Dim centralDirectoryOffsetField As UInteger
        If centralDirectoryOffset >= CLng(ZipConstants.UInt32Max) Then
            centralDirectoryOffsetField = ZipConstants.UInt32Max
        Else
            centralDirectoryOffsetField = CUInt(centralDirectoryOffset)
        End If

        writer.Write(ZipConstants.EndOfCentralDirectorySignature)
        writer.Write(CUShort(0))                     ' Number of this disk
        writer.Write(CUShort(0))                     ' Disk where central directory starts
        writer.Write(entryCountField)                ' Records on this disk
        writer.Write(entryCountField)                ' Total records
        writer.Write(centralDirectorySizeField)
        writer.Write(centralDirectoryOffsetField)
        writer.Write(CUShort(0))                     ' Comment length
    End Sub

    Private Shared Sub GetDosDateTime(ByVal dt As DateTime, ByRef dosTime As UShort, ByRef dosDate As UShort)
        ' MS-DOS date/time packing used by the ZIP format:
        '   time: bits 15-11 hour, 10-5 minute, 4-0 seconds/2
        '   date: bits 15-9 (year-1980), 8-5 month, 4-0 day
        dosTime = CUShort((dt.Hour << 11) Or (dt.Minute << 5) Or (dt.Second \ 2))
        dosDate = CUShort(((dt.Year - 1980) << 9) Or (dt.Month << 5) Or dt.Day)
    End Sub
End Class

''' <summary>
''' Decodes ZIP LZMA (method 14) and method-33 LZMA2 entries. The LZMA2
''' method-33 layout used here begins with a one-byte LZMA2 dictionary property,
''' followed by the raw LZMA2 chunk stream. This layout is an explicit extension;
''' LZMA2 is not defined as a standard ZIP compression method by the 7-Zip SDK.
''' The range coder, LZMA probability models, and LZMA2 chunk state machine
''' follow the public-domain 7-Zip LZMA SDK decoder.
''' </summary>
Public NotInheritable Class ZipLzmaDecoder
    Private Const OutputBufferSize As Integer = 81920
    ' LZMA dictionaries are attacker-controlled archive metadata; keep their
    ' memory use bounded while allowing common 7-Zip dictionary sizes.
    Private Const MaximumDictionarySize As Long = 268435456L
    Private Const MinimumDictionarySize As Long = 4096L

    Private Sub New()
    End Sub

    ''' <summary>Decodes the ZIP method-14 header and its raw LZMA payload.</summary>
    Public Shared Sub DecompressLzma(ByVal source As Stream, ByVal destination As Stream, ByVal uncompressedSize As Long, ByRef runningCrc As UInteger, ByRef totalWritten As Long)
        If source Is Nothing Then Throw New ArgumentNullException("source")
        If destination Is Nothing Then Throw New ArgumentNullException("destination")
        If uncompressedSize < 0L Then Throw New ArgumentOutOfRangeException("uncompressedSize")

        ' APPNOTE method 14 stores version bytes and a 16-bit property length
        ' before the five LZMA property bytes and the range-coded payload.
        ReadRequiredByte(source, "LZMA header")
        ReadRequiredByte(source, "LZMA header")
        Dim propertySizeLow As Integer = ReadRequiredByte(source, "LZMA property size")
        Dim propertySizeHigh As Integer = ReadRequiredByte(source, "LZMA property size")
        Dim propertySize As Integer = propertySizeLow Or (propertySizeHigh << 8)
        If propertySize <> 5 Then
            Throw New InvalidDataException("ZIP LZMA entries must contain exactly five LZMA property bytes.")
        End If

        Dim properties As Byte() = ReadRequiredBytes(source, propertySize, "LZMA properties")
        Dim dictionarySize As Long = ReadUInt32(properties, 1)
        Dim decoder As New LzmaState(uncompressedSize, dictionarySize)
        decoder.SetProperties(properties(0))
        decoder.DecodeRaw(source, destination, uncompressedSize, runningCrc, totalWritten)
    End Sub

    ''' <summary>
    ''' Decodes the selected method-33 variant: one LZMA2 dictionary-property
    ''' byte followed by an LZMA2 stream with its normal chunk headers.
    ''' </summary>
    Public Shared Sub DecompressLzma2(ByVal source As Stream, ByVal destination As Stream, ByVal uncompressedSize As Long, ByRef runningCrc As UInteger, ByRef totalWritten As Long)
        If source Is Nothing Then Throw New ArgumentNullException("source")
        If destination Is Nothing Then Throw New ArgumentNullException("destination")
        If uncompressedSize < 0L Then Throw New ArgumentOutOfRangeException("uncompressedSize")

        Dim dictionaryProperty As Integer = ReadRequiredByte(source, "LZMA2 dictionary property")
        If dictionaryProperty > 40 Then
            Throw New InvalidDataException("The LZMA2 dictionary property is outside the supported range.")
        End If

        Dim dictionarySize As Long = GetLzma2DictionarySize(dictionaryProperty)
        Dim decoder As New LzmaState(uncompressedSize, dictionarySize)
        decoder.SetLzma2Properties(0)
        decoder.DecodeLzma2(source, destination, uncompressedSize, runningCrc, totalWritten)
    End Sub

    Private Shared Function ReadRequiredByte(ByVal source As Stream, ByVal fieldName As String) As Integer
        Dim value As Integer = source.ReadByte()
        If value < 0 Then
            Throw New InvalidDataException("Unexpected end of data while reading " & fieldName & ".")
        End If
        Return value
    End Function

    Private Shared Function ReadRequiredBytes(ByVal source As Stream, ByVal count As Integer, ByVal fieldName As String) As Byte()
        Dim data As Byte() = New Byte(count - 1) {}
        Dim position As Integer = 0
        Do While position < count
            Dim bytesRead As Integer = source.Read(data, position, count - position)
            If bytesRead <= 0 Then
                Throw New InvalidDataException("Unexpected end of data while reading " & fieldName & ".")
            End If
            position += bytesRead
        Loop
        Return data
    End Function

    Private Shared Function ReadUInt32(ByVal data As Byte(), ByVal offset As Integer) As Long
        Return CLng(data(offset)) Or (CLng(data(offset + 1)) << 8) Or (CLng(data(offset + 2)) << 16) Or (CLng(data(offset + 3)) << 24)
    End Function

    Private Shared Function GetLzma2DictionarySize(ByVal dictionaryProperty As Integer) As Long
        If dictionaryProperty = 40 Then
            Return &HFFFFFFFFL
        End If
        Return (CLng(2 Or (dictionaryProperty And 1)) << ((dictionaryProperty \ 2) + 11))
    End Function

    Private NotInheritable Class LzmaState
        Private Const ProbabilityTotal As Integer = 2048
        Private Const ProbabilityMoveBits As Integer = 5
        Private Const RangeTop As Long = &H1000000L
        Private Const MaximumPositionStateBits As Integer = 4
        Private Const NumberStates As Integer = 12
        Private Const NumberPositionStates As Integer = 1 << MaximumPositionStateBits
        Private Const LiteralStateSize As Integer = &H300
        Private Const MatchMinimumLength As Integer = 2
        Private Const NumberLowLengthBits As Integer = 3
        Private Const NumberMidLengthBits As Integer = 3
        Private Const NumberHighLengthBits As Integer = 8
        Private Const NumberLowLengthSymbols As Integer = 1 << NumberLowLengthBits
        Private Const NumberMidLengthSymbols As Integer = 1 << NumberMidLengthBits
        Private Const NumberStartPositionModel As Integer = 4
        Private Const NumberEndPositionModel As Integer = 14
        Private Const NumberFullDistances As Integer = 1 << (NumberEndPositionModel \ 2)
        Private Const NumberAlignBits As Integer = 4

        Private ReadOnly _dictionary As Byte()
        Private ReadOnly _dictionarySize As Long
        Private ReadOnly _isMatch As Integer() = New Integer(NumberStates * NumberPositionStates - 1) {}
        Private ReadOnly _isRep As Integer() = New Integer(NumberStates - 1) {}
        Private ReadOnly _isRepG0 As Integer() = New Integer(NumberStates - 1) {}
        Private ReadOnly _isRepG1 As Integer() = New Integer(NumberStates - 1) {}
        Private ReadOnly _isRepG2 As Integer() = New Integer(NumberStates - 1) {}
        Private ReadOnly _isRep0Long As Integer() = New Integer(NumberStates * NumberPositionStates - 1) {}
        Private ReadOnly _positionSlots As Integer() = New Integer(4 * 64 - 1) {}
        Private ReadOnly _positionDecoders As Integer() = New Integer(NumberFullDistances - NumberEndPositionModel + 1) {}
        Private ReadOnly _positionAlign As Integer() = New Integer((1 << NumberAlignBits) - 1) {}
        Private ReadOnly _lengthChoice As Integer() = New Integer(1) {}
        Private ReadOnly _lengthChoice2 As Integer() = New Integer(1) {}
        Private ReadOnly _lengthLow As Integer() = New Integer(NumberPositionStates * NumberLowLengthSymbols - 1) {}
        Private ReadOnly _lengthMid As Integer() = New Integer(NumberPositionStates * NumberMidLengthSymbols - 1) {}
        Private ReadOnly _lengthHigh As Integer() = New Integer((1 << NumberHighLengthBits) - 1) {}
        Private ReadOnly _repLengthChoice As Integer() = New Integer(1) {}
        Private ReadOnly _repLengthChoice2 As Integer() = New Integer(1) {}
        Private ReadOnly _repLengthLow As Integer() = New Integer(NumberPositionStates * NumberLowLengthSymbols - 1) {}
        Private ReadOnly _repLengthMid As Integer() = New Integer(NumberPositionStates * NumberMidLengthSymbols - 1) {}
        Private ReadOnly _repLengthHigh As Integer() = New Integer((1 << NumberHighLengthBits) - 1) {}

        Private _literalModels As Integer()
        Private _literalContextBits As Integer
        Private _literalPositionBits As Integer
        Private _positionStateBits As Integer
        Private _positionStateMask As Long
        Private _state As Integer
        Private _rep0 As Long
        Private _rep1 As Long
        Private _rep2 As Long
        Private _rep3 As Long
        Private _position As Long
        Private _historyCount As Long
        Private _dictionaryPosition As Integer
        Private _range As Long
        Private _code As Long

        Public Sub New(ByVal outputSize As Long, ByVal dictionarySize As Long)
            If dictionarySize < 1L Then dictionarySize = 1L
            _dictionarySize = dictionarySize

            Dim requiredDictionary As Long = Math.Min(dictionarySize, Math.Max(MinimumDictionarySize, outputSize))
            If requiredDictionary > MaximumDictionarySize Then
                Throw New NotSupportedException("The LZMA dictionary required by this entry exceeds the 256 MB safety limit.")
            End If

            Dim actualCapacity As Integer = CInt(Math.Max(MinimumDictionarySize, requiredDictionary))
            _dictionary = New Byte(actualCapacity - 1) {}
            ResetModels()
        End Sub

        Public Sub SetProperties(ByVal propertyByte As Integer)
            If propertyByte < 0 OrElse propertyByte >= 9 * 5 * 5 Then
                Throw New InvalidDataException("The LZMA property byte is invalid.")
            End If

            _literalContextBits = propertyByte Mod 9
            Dim remainder As Integer = propertyByte \ 9
            _literalPositionBits = remainder Mod 5
            _positionStateBits = remainder \ 5
            If _positionStateBits > MaximumPositionStateBits Then
                Throw New InvalidDataException("The LZMA position-state property is invalid.")
            End If

            Dim modelCount As Integer = (1 << (_literalContextBits + _literalPositionBits)) * LiteralStateSize
            If _literalModels Is Nothing OrElse _literalModels.Length <> modelCount Then
                _literalModels = New Integer(modelCount - 1) {}
            End If
            _positionStateMask = (1L << _positionStateBits) - 1L
            ResetModels()
        End Sub

        Public Sub SetLzma2Properties(ByVal propertyByte As Integer)
            Dim literalContextBits As Integer = propertyByte Mod 9
            Dim remainder As Integer = propertyByte \ 9
            Dim literalPositionBits As Integer = remainder Mod 5
            Dim positionStateBits As Integer = remainder \ 5
            If propertyByte < 0 OrElse propertyByte >= 9 * 5 * 5 OrElse literalContextBits + literalPositionBits > 4 OrElse positionStateBits > MaximumPositionStateBits Then
                Throw New InvalidDataException("The LZMA2 LZMA properties are invalid.")
            End If
            SetProperties(propertyByte)
        End Sub

        Public Sub DecodeRaw(ByVal source As Stream, ByVal destination As Stream, ByVal outputSize As Long, ByRef runningCrc As UInteger, ByRef totalWritten As Long)
            If outputSize = 0L Then Return
            InitializeRangeDecoder(source)
            DecodeSymbols(destination, outputSize, runningCrc, totalWritten)
        End Sub

        Public Sub DecodeLzma2(ByVal source As Stream, ByVal destination As Stream, ByVal expectedOutputSize As Long, ByRef runningCrc As UInteger, ByRef totalWritten As Long)
            Dim needInitialization As Integer = &HE0
            Dim haveProperties As Boolean = False
            Dim outputBuffer As Byte() = New Byte(OutputBufferSize - 1) {}

            Do
                Dim control As Integer = ReadRequiredByte(source, "LZMA2 chunk control")
                If control = 0 Then Exit Do

                ' LZMA2 control values 1/2 carry stored chunks. Values with
                ' bit 7 set carry packed LZMA chunks and encode which decoder
                ' state (dictionary, probabilities, and properties) to reset.
                If control < &H80 Then
                    If control = 1 Then
                        ResetDictionary()
                        needInitialization = &HC0
                    ElseIf control <> 2 OrElse needInitialization = &HE0 Then
                        Throw New InvalidDataException("Invalid LZMA2 uncompressed chunk sequence.")
                    End If

                    Dim storedUnpackSize As Integer = ReadBigEndianUInt16(source, "LZMA2 uncompressed size") + 1
                    EnsureOutputWithinLimit(totalWritten, storedUnpackSize, expectedOutputSize)
                    Dim remaining As Integer = storedUnpackSize
                    Do While remaining > 0
                        Dim count As Integer = Math.Min(remaining, outputBuffer.Length)
                        ReadRequiredBytesInto(source, outputBuffer, count, "LZMA2 uncompressed chunk")
                        Emit(outputBuffer, count, destination, runningCrc, totalWritten)
                        AddToDictionary(outputBuffer, 0, count)
                        remaining -= count
                    Loop
                    Continue Do
                End If

                If control < needInitialization Then
                    Throw New InvalidDataException("LZMA2 chunk is missing a required dictionary or state reset.")
                End If
                needInitialization = 0

                Dim compressedUnpackSize As Integer = ((control And &H1F) << 16) Or ReadBigEndianUInt16(source, "LZMA2 unpack size")
                compressedUnpackSize += 1
                Dim packedSize As Integer = ReadBigEndianUInt16(source, "LZMA2 packed size") + 1
                EnsureOutputWithinLimit(totalWritten, compressedUnpackSize, expectedOutputSize)

                If control >= &HC0 Then
                    SetLzma2Properties(ReadRequiredByte(source, "LZMA2 properties"))
                    haveProperties = True
                ElseIf Not haveProperties Then
                    Throw New InvalidDataException("The first LZMA2 compressed chunk must provide LZMA properties.")
                End If

                If control >= &HE0 Then
                    ResetDictionary()
                End If
                If control >= &HA0 Then
                    ResetModels()
                End If

                Dim packedData As Byte() = ReadRequiredBytes(source, packedSize, "LZMA2 compressed chunk")
                Using packedStream As New MemoryStream(packedData, False)
                    InitializeRangeDecoder(packedStream)
                    DecodeSymbols(destination, compressedUnpackSize, runningCrc, totalWritten)
                End Using
            Loop

            If totalWritten <> expectedOutputSize Then
                Throw New InvalidDataException("LZMA2 output size does not match the ZIP entry's declared size.")
            End If
            If source.ReadByte() <> -1 Then
                Throw New InvalidDataException("LZMA2 data remains after its end marker.")
            End If
        End Sub

        Private Shared Function ReadBigEndianUInt16(ByVal source As Stream, ByVal fieldName As String) As Integer
            Dim high As Integer = ReadRequiredByte(source, fieldName)
            Dim low As Integer = ReadRequiredByte(source, fieldName)
            Return (high << 8) Or low
        End Function

        Private Shared Sub ReadRequiredBytesInto(ByVal source As Stream, ByVal buffer As Byte(), ByVal count As Integer, ByVal fieldName As String)
            Dim offset As Integer = 0
            Do While offset < count
                Dim bytesRead As Integer = source.Read(buffer, offset, count - offset)
                If bytesRead <= 0 Then
                    Throw New InvalidDataException("Unexpected end of data while reading " & fieldName & ".")
                End If
                offset += bytesRead
            Loop
        End Sub

        Private Shared Sub EnsureOutputWithinLimit(ByVal totalWritten As Long, ByVal chunkSize As Integer, ByVal expectedSize As Long)
            If chunkSize < 0 OrElse totalWritten > expectedSize - CLng(chunkSize) Then
                Throw New InvalidDataException("LZMA2 output exceeds the ZIP entry's declared uncompressed size.")
            End If
        End Sub

        Private Sub DecodeSymbols(ByVal destination As Stream, ByVal outputSize As Long, ByRef runningCrc As UInteger, ByRef totalWritten As Long)
            Dim outputBuffer As Byte() = New Byte(OutputBufferSize - 1) {}
            Dim buffered As Integer = 0
            Dim produced As Long = 0L

            Do While produced < outputSize
                Dim positionState As Integer = CInt(_position And _positionStateMask)
                Dim modelIndex As Integer = (_state << MaximumPositionStateBits) + positionState
                Dim value As Integer

                If DecodeBit(_isMatch, modelIndex) = 0 Then
                    Dim previousByte As Integer = GetPreviousByte()
                    If _state >= 7 Then
                        value = DecodeLiteralWithMatch(_position, previousByte, GetByte(_rep0))
                    Else
                        value = DecodeLiteral(_position, previousByte)
                    End If
                    _state = UpdateCharState(_state)
                    PutByte(CByte(value), outputBuffer, buffered, destination, runningCrc, totalWritten)
                    produced += 1L
                Else
                    Dim matchLength As Integer
                    Dim distance As Long

                    If DecodeBit(_isRep, _state) <> 0 Then
                        If DecodeBit(_isRepG0, _state) = 0 Then
                            If DecodeBit(_isRep0Long, modelIndex) = 0 Then
                                If _state < 7 Then
                                    _state = 9
                                Else
                                    _state = 11
                                End If
                                value = GetByte(_rep0)
                                PutByte(CByte(value), outputBuffer, buffered, destination, runningCrc, totalWritten)
                                produced += 1L
                                Continue Do
                            End If
                            distance = _rep0
                        ElseIf DecodeBit(_isRepG1, _state) = 0 Then
                            distance = _rep1
                        ElseIf DecodeBit(_isRepG2, _state) = 0 Then
                            distance = _rep2
                            _rep2 = _rep1
                        Else
                            distance = _rep3
                            _rep3 = _rep2
                            _rep2 = _rep1
                        End If

                        _rep1 = _rep0
                        _rep0 = distance
                        matchLength = DecodeLength(_repLengthChoice, _repLengthChoice2, _repLengthLow, _repLengthMid, _repLengthHigh, positionState) + MatchMinimumLength
                        If _state < 7 Then
                            _state = 8
                        Else
                            _state = 11
                        End If
                    Else
                        _rep3 = _rep2
                        _rep2 = _rep1
                        _rep1 = _rep0
                        matchLength = DecodeLength(_lengthChoice, _lengthChoice2, _lengthLow, _lengthMid, _lengthHigh, positionState) + MatchMinimumLength
                        If _state < 7 Then
                            _state = 7
                        Else
                            _state = 10
                        End If
                        Dim lengthToPositionState As Integer = Math.Min(matchLength - MatchMinimumLength, 3)
                        Dim positionSlot As Integer = DecodeTree(_positionSlots, lengthToPositionState * 64, 6)
                        If positionSlot < NumberStartPositionModel Then
                            _rep0 = positionSlot
                        Else
                            Dim directBitCount As Integer = (positionSlot >> 1) - 1
                            _rep0 = CLng(2 Or (positionSlot And 1)) << directBitCount
                            If positionSlot < NumberEndPositionModel Then
                                Dim startIndex As Integer = CInt(_rep0) - positionSlot - 1
                                _rep0 += DecodeReverseTree(_positionDecoders, startIndex, directBitCount)
                            Else
                                _rep0 += DecodeDirectBits(directBitCount - NumberAlignBits) << NumberAlignBits
                                _rep0 += DecodeReverseTree(_positionAlign, 0, NumberAlignBits)
                            End If
                        End If
                    End If

                    If _rep0 >= _dictionarySize OrElse _rep0 >= _historyCount OrElse matchLength > outputSize - produced Then
                        Throw New InvalidDataException("The LZMA stream contains an invalid match distance or length.")
                    End If

                    For matchIndex As Integer = 0 To matchLength - 1
                        value = GetByte(_rep0)
                        PutByte(CByte(value), outputBuffer, buffered, destination, runningCrc, totalWritten)
                    Next
                    produced += CLng(matchLength)
                End If
            Loop

            If buffered > 0 Then
                Emit(outputBuffer, buffered, destination, runningCrc, totalWritten)
            End If
        End Sub

        Private Function DecodeLength(ByVal choice As Integer(), ByVal choice2 As Integer(), ByVal lowModels As Integer(), ByVal midModels As Integer(), ByVal highModels As Integer(), ByVal positionState As Integer) As Integer
            If DecodeBit(choice, 0) = 0 Then
                Return DecodeTree(lowModels, positionState * NumberLowLengthSymbols, NumberLowLengthBits)
            End If

            If DecodeBit(choice2, 0) = 0 Then
                Return NumberLowLengthSymbols + DecodeTree(midModels, positionState * NumberMidLengthSymbols, NumberMidLengthBits)
            End If

            Return NumberLowLengthSymbols + NumberMidLengthSymbols + DecodeTree(highModels, 0, NumberHighLengthBits)
        End Function

        Private Function DecodeLiteral(ByVal position As Long, ByVal previousByte As Integer) As Integer
            Dim context As Integer = LiteralContext(position, previousByte)
            Dim modelOffset As Integer = context * LiteralStateSize
            Dim symbol As Integer = 1
            Do While symbol < &H100
                symbol = (symbol << 1) Or DecodeBit(_literalModels, modelOffset + symbol)
            Loop
            Return symbol And &HFF
        End Function

        Private Function DecodeLiteralWithMatch(ByVal position As Long, ByVal previousByte As Integer, ByVal matchByteValue As Integer) As Integer
            Dim context As Integer = LiteralContext(position, previousByte)
            Dim modelOffset As Integer = context * LiteralStateSize
            Dim symbol As Integer = 1
            Dim matchByte As Integer = matchByteValue

            Do While symbol < &H100
                Dim matchBit As Integer = (matchByte >> 7) And 1
                matchByte = (matchByte << 1) And &HFF
                Dim bit As Integer = DecodeBit(_literalModels, modelOffset + ((1 + matchBit) << 8) + symbol)
                symbol = (symbol << 1) Or bit
                If matchBit <> bit Then
                    Do While symbol < &H100
                        symbol = (symbol << 1) Or DecodeBit(_literalModels, modelOffset + symbol)
                    Loop
                    Exit Do
                End If
            Loop

            Return symbol And &HFF
        End Function

        Private Function LiteralContext(ByVal position As Long, ByVal previousByte As Integer) As Integer
            Dim positionMask As Integer = (1 << _literalPositionBits) - 1
            Return (CInt(position And CLng(positionMask)) << _literalContextBits) + (previousByte >> (8 - _literalContextBits))
        End Function

        Private Function DecodeTree(ByVal models As Integer(), ByVal startIndex As Integer, ByVal bitCount As Integer) As Integer
            Dim node As Integer = 1
            For bitIndex As Integer = 0 To bitCount - 1
                node = (node << 1) Or DecodeBit(models, startIndex + node)
            Next
            Return node - (1 << bitCount)
        End Function

        Private Function DecodeReverseTree(ByVal models As Integer(), ByVal startIndex As Integer, ByVal bitCount As Integer) As Long
            Dim node As Integer = 1
            Dim result As Long = 0L
            For bitIndex As Integer = 0 To bitCount - 1
                Dim bit As Integer = DecodeBit(models, startIndex + node)
                node = (node << 1) Or bit
                result = result Or (CLng(bit) << bitIndex)
            Next
            Return result
        End Function

        Private Function DecodeDirectBits(ByVal bitCount As Integer) As Long
            Dim result As Long = 0L
            For i As Integer = bitCount - 1 To 0 Step -1
                _range >>= 1
                Dim bit As Integer = 0
                If _code >= _range Then
                    _code -= _range
                    bit = 1
                End If
                result = (result << 1) Or bit
                NormalizeRange()
            Next
            Return result
        End Function

        Private Function DecodeBit(ByVal models As Integer(), ByVal index As Integer) As Integer
            Dim probability As Integer = models(index)
            Dim bound As Long = (_range >> 11) * CLng(probability)
            Dim bit As Integer

            If _code < bound Then
                _range = bound
                models(index) = probability + ((ProbabilityTotal - probability) >> ProbabilityMoveBits)
                bit = 0
            Else
                _range -= bound
                _code -= bound
                models(index) = probability - (probability >> ProbabilityMoveBits)
                bit = 1
            End If

            NormalizeRange()
            Return bit
        End Function

        Private Sub NormalizeRange()
            If _range < RangeTop Then
                _range <<= 8
                _code = ((_code << 8) Or CLng(ReadRequiredByteFromRange())) And &HFFFFFFFFL
            End If
        End Sub

        Private Function ReadRequiredByteFromRange() As Integer
            If _rangeInput Is Nothing Then
                Throw New InvalidOperationException("The LZMA range decoder has no input stream.")
            End If
            Return ReadRequiredByte(_rangeInput, "LZMA compressed data")
        End Function

        Private _rangeInput As Stream

        Private Sub InitializeRangeDecoder(ByVal source As Stream)
            _rangeInput = source
            Dim firstByte As Integer = ReadRequiredByte(source, "LZMA range coder")
            If firstByte <> 0 Then
                Throw New InvalidDataException("The LZMA range-coder initialization byte is invalid.")
            End If

            _code = 0L
            For i As Integer = 0 To 3
                _code = (_code << 8) Or CLng(ReadRequiredByte(source, "LZMA range coder"))
            Next
            _range = &HFFFFFFFFL
        End Sub

        Private Function GetPreviousByte() As Integer
            If _historyCount = 0L Then Return 0
            Dim index As Integer = _dictionaryPosition - 1
            If index < 0 Then index += _dictionary.Length
            Return CInt(_dictionary(index))
        End Function

        Private Function GetByte(ByVal distance As Long) As Integer
            If distance < 0L OrElse distance >= _historyCount OrElse distance >= _dictionarySize Then
                Throw New InvalidDataException("The LZMA stream refers beyond the available dictionary.")
            End If

            Dim index As Long = CLng(_dictionaryPosition) - distance - 1L
            If index < 0L Then index += _dictionary.Length
            Return CInt(_dictionary(CInt(index)))
        End Function

        Private Sub PutByte(ByVal value As Byte, ByVal outputBuffer As Byte(), ByRef buffered As Integer, ByVal destination As Stream, ByRef runningCrc As UInteger, ByRef totalWritten As Long)
            _dictionary(_dictionaryPosition) = value
            _dictionaryPosition += 1
            If _dictionaryPosition = _dictionary.Length Then _dictionaryPosition = 0
            If _historyCount < _dictionary.Length Then _historyCount += 1L
            _position += 1L

            outputBuffer(buffered) = value
            buffered += 1
            If buffered = outputBuffer.Length Then
                Emit(outputBuffer, buffered, destination, runningCrc, totalWritten)
                buffered = 0
            End If
        End Sub

        Private Sub AddToDictionary(ByVal data As Byte(), ByVal offset As Integer, ByVal count As Integer)
            For i As Integer = 0 To count - 1
                _dictionary(_dictionaryPosition) = data(offset + i)
                _dictionaryPosition += 1
                If _dictionaryPosition = _dictionary.Length Then _dictionaryPosition = 0
                If _historyCount < _dictionary.Length Then _historyCount += 1L
                _position += 1L
            Next
        End Sub

        Private Shared Sub Emit(ByVal buffer As Byte(), ByVal count As Integer, ByVal destination As Stream, ByRef runningCrc As UInteger, ByRef totalWritten As Long)
            destination.Write(buffer, 0, count)
            runningCrc = Crc32Calculator.UpdateCrc32(runningCrc, buffer, 0, count)
            totalWritten += count
        End Sub

        Private Sub ResetDictionary()
            _dictionaryPosition = 0
            _historyCount = 0L
            _position = 0L
        End Sub

        Private Sub ResetModels()
            ResetProbabilityArray(_isMatch)
            ResetProbabilityArray(_isRep)
            ResetProbabilityArray(_isRepG0)
            ResetProbabilityArray(_isRepG1)
            ResetProbabilityArray(_isRepG2)
            ResetProbabilityArray(_isRep0Long)
            ResetProbabilityArray(_positionSlots)
            ResetProbabilityArray(_positionDecoders)
            ResetProbabilityArray(_positionAlign)
            ResetProbabilityArray(_lengthChoice)
            ResetProbabilityArray(_lengthChoice2)
            ResetProbabilityArray(_lengthLow)
            ResetProbabilityArray(_lengthMid)
            ResetProbabilityArray(_lengthHigh)
            ResetProbabilityArray(_repLengthChoice)
            ResetProbabilityArray(_repLengthChoice2)
            ResetProbabilityArray(_repLengthLow)
            ResetProbabilityArray(_repLengthMid)
            ResetProbabilityArray(_repLengthHigh)
            If _literalModels IsNot Nothing Then ResetProbabilityArray(_literalModels)

            _state = 0
            _rep0 = 0L
            _rep1 = 0L
            _rep2 = 0L
            _rep3 = 0L
        End Sub

        Private Shared Sub ResetProbabilityArray(ByVal models As Integer())
            For i As Integer = 0 To models.Length - 1
                models(i) = ProbabilityTotal >> 1
            Next
        End Sub

        Private Shared Function UpdateCharState(ByVal state As Integer) As Integer
            If state < 4 Then Return 0
            If state < 10 Then Return state - 3
            Return state - 6
        End Function
    End Class
End Class

''' <summary>
''' Writes ZIP method-14 LZMA streams using the standard range coder and
''' adaptive literal/repetition models. It uses distance-one repetition matches
''' for runs of identical bytes without a separate match-finder allocation.
''' </summary>
Public NotInheritable Class ZipLzmaEncoder
    Private Const ProbabilityTotal As Integer = 2048
    Private Const ProbabilityMoveBits As Integer = 5
    Private Const LiteralContextBits As Integer = 3
    Private Const LiteralPositionBits As Integer = 0
    Private Const PositionStateBits As Integer = 2
    Private Const LiteralStateSize As Integer = &H300

    Private Sub New()
    End Sub

    ''' <summary>Encodes a raw method-14 payload and updates the ZIP CRC.</summary>
    Public Shared Sub Compress(ByVal source As Stream, ByVal destination As Stream, ByVal uncompressedSize As Long, ByRef runningCrc As UInteger)
        If source Is Nothing Then Throw New ArgumentNullException("source")
        If destination Is Nothing Then Throw New ArgumentNullException("destination")
        If uncompressedSize < 0L Then Throw New ArgumentOutOfRangeException("uncompressedSize")
        If Not source.CanRead Then Throw New ArgumentException("The source stream must be readable.", "source")
        If Not destination.CanWrite Then Throw New ArgumentException("The destination stream must be writable.", "destination")

        ' Version 9.20, five property bytes, lc=3/lp=0/pb=2, one-megabyte dictionary.
        destination.WriteByte(9)
        destination.WriteByte(20)
        destination.WriteByte(5)
        destination.WriteByte(0)
        destination.WriteByte(CByte((PositionStateBits * 5 + LiteralPositionBits) * 9 + LiteralContextBits))
        destination.WriteByte(0)
        destination.WriteByte(0)
        destination.WriteByte(16)
        destination.WriteByte(0)

        Dim literalModels As Integer() = New Integer((1 << LiteralContextBits) * LiteralStateSize - 1) {}
        For i As Integer = 0 To literalModels.Length - 1
            literalModels(i) = ProbabilityTotal >> 1
        Next
        Dim isMatch As Integer() = New Integer(12 * (1 << PositionStateBits) - 1) {}
        For i As Integer = 0 To isMatch.Length - 1
            isMatch(i) = ProbabilityTotal >> 1
        Next
        Dim isRep As Integer() = New Integer(11) {}
        Dim isRepG0 As Integer() = New Integer(11) {}
        Dim isRep0Long As Integer() = New Integer(12 * (1 << PositionStateBits) - 1) {}
        Dim repLengthChoice As Integer() = New Integer(1) {}
        Dim repLengthChoice2 As Integer() = New Integer(1) {}
        Dim repLengthLow As Integer() = New Integer((1 << PositionStateBits) * 8 - 1) {}
        Dim repLengthMid As Integer() = New Integer((1 << PositionStateBits) * 8 - 1) {}
        Dim repLengthHigh As Integer() = New Integer(255) {}
        ResetProbabilities(isRep)
        ResetProbabilities(isRepG0)
        ResetProbabilities(isRep0Long)
        ResetProbabilities(repLengthChoice)
        ResetProbabilities(repLengthChoice2)
        ResetProbabilities(repLengthLow)
        ResetProbabilities(repLengthMid)
        ResetProbabilities(repLengthHigh)

        Dim rangeEncoder As New LzmaRangeEncoder(destination)
        Dim buffer As Byte() = New Byte(81919) {}
        Dim bytesRead As Integer = source.Read(buffer, 0, buffer.Length)
        Dim position As Long = 0L
        Dim previousByte As Integer = 0
        Dim state As Integer = 0
        Dim positionStateMask As Long = (1L << PositionStateBits) - 1L

        Do While bytesRead > 0
            runningCrc = Crc32Calculator.UpdateCrc32(runningCrc, buffer, 0, bytesRead)

            For i As Integer = 0 To bytesRead - 1
                Dim positionState As Integer = CInt(position And positionStateMask)
                Dim value As Integer = CInt(buffer(i))
                Dim matchLength As Integer = 0

                If position > 0L AndAlso value = previousByte Then
                    matchLength = 1
                    Do While matchLength < 273 AndAlso i + matchLength < bytesRead AndAlso CInt(buffer(i + matchLength)) = previousByte
                        matchLength += 1
                    Loop
                End If

                Dim modelIndex As Integer = (state << PositionStateBits) + positionState
                If matchLength >= 3 Then
                    rangeEncoder.EncodeBit(isMatch, modelIndex, 1)
                    rangeEncoder.EncodeBit(isRep, state, 1)
                    rangeEncoder.EncodeBit(isRepG0, state, 0)
                    rangeEncoder.EncodeBit(isRep0Long, modelIndex, 1)
                    EncodeLength(rangeEncoder, repLengthChoice, repLengthChoice2, repLengthLow, repLengthMid, repLengthHigh, positionState, matchLength - 2)
                    If state < 7 Then
                        state = 8
                    Else
                        state = 11
                    End If
                    position += CLng(matchLength)
                    i += matchLength - 1
                Else
                    rangeEncoder.EncodeBit(isMatch, modelIndex, 0)
                    EncodeLiteral(rangeEncoder, literalModels, previousByte, value, state)
                    previousByte = value
                    position += 1L
                End If
            Next

            bytesRead = source.Read(buffer, 0, buffer.Length)
        Loop

        If position <> uncompressedSize Then
            Throw New IOException("The source file size changed while it was being compressed.")
        End If
        rangeEncoder.Finish()
    End Sub

    Private Shared Sub EncodeLiteral(ByVal rangeEncoder As LzmaRangeEncoder, ByVal models As Integer(), ByVal previousByte As Integer, ByVal value As Integer, ByRef state As Integer)
        Dim context As Integer = previousByte >> (8 - LiteralContextBits)
        Dim modelOffset As Integer = context * LiteralStateSize
        Dim symbol As Integer = 1

        If state >= 7 Then
            Dim matchByte As Integer = previousByte
            Dim bitIndex As Integer = 7
            Do While symbol < &H100
                Dim matchBit As Integer = (matchByte >> 7) And 1
                matchByte = (matchByte << 1) And &HFF
                Dim bit As Integer = (value >> bitIndex) And 1
                rangeEncoder.EncodeBit(models, modelOffset + ((1 + matchBit) << 8) + symbol, bit)
                symbol = (symbol << 1) Or bit
                bitIndex -= 1

                If matchBit <> bit Then
                    Do While symbol < &H100
                        Dim nextBit As Integer = (value >> bitIndex) And 1
                        rangeEncoder.EncodeBit(models, modelOffset + symbol, nextBit)
                        symbol = (symbol << 1) Or nextBit
                        bitIndex -= 1
                    Loop
                    Exit Do
                End If
            Loop
        Else
            For bitIndex As Integer = 7 To 0 Step -1
                Dim bit As Integer = (value >> bitIndex) And 1
                rangeEncoder.EncodeBit(models, modelOffset + symbol, bit)
                symbol = (symbol << 1) Or bit
            Next
        End If

        state = UpdateCharState(state)
    End Sub

    Private Shared Sub EncodeLength(ByVal rangeEncoder As LzmaRangeEncoder, ByVal choice As Integer(), ByVal choice2 As Integer(), ByVal low As Integer(), ByVal mid As Integer(), ByVal high As Integer(), ByVal positionState As Integer, ByVal lengthSymbol As Integer)
        If lengthSymbol < 8 Then
            rangeEncoder.EncodeBit(choice, 0, 0)
            EncodeTree(rangeEncoder, low, positionState * 8, 3, lengthSymbol)
        ElseIf lengthSymbol < 16 Then
            rangeEncoder.EncodeBit(choice, 0, 1)
            rangeEncoder.EncodeBit(choice2, 0, 0)
            EncodeTree(rangeEncoder, mid, positionState * 8, 3, lengthSymbol - 8)
        Else
            rangeEncoder.EncodeBit(choice, 0, 1)
            rangeEncoder.EncodeBit(choice2, 0, 1)
            EncodeTree(rangeEncoder, high, 0, 8, lengthSymbol - 16)
        End If
    End Sub

    Private Shared Sub EncodeTree(ByVal rangeEncoder As LzmaRangeEncoder, ByVal models As Integer(), ByVal startIndex As Integer, ByVal bitCount As Integer, ByVal value As Integer)
        Dim node As Integer = 1
        For bitIndex As Integer = bitCount - 1 To 0 Step -1
            Dim bit As Integer = (value >> bitIndex) And 1
            rangeEncoder.EncodeBit(models, startIndex + node, bit)
            node = (node << 1) Or bit
        Next
    End Sub

    Private Shared Sub ResetProbabilities(ByVal models As Integer())
        For i As Integer = 0 To models.Length - 1
            models(i) = ProbabilityTotal >> 1
        Next
    End Sub

    Private Shared Function UpdateCharState(ByVal state As Integer) As Integer
        If state < 4 Then Return 0
        If state < 10 Then Return state - 3
        Return state - 6
    End Function

    Private NotInheritable Class LzmaRangeEncoder
        Private Const RangeTop As Long = &H1000000L
        Private ReadOnly _destination As Stream
        Private _range As Long = &HFFFFFFFFL
        Private _low As Long
        Private _cache As Integer
        Private _cacheSize As Long = 1L

        Public Sub New(ByVal destination As Stream)
            _destination = destination
        End Sub

        Public Sub EncodeBit(ByVal models As Integer(), ByVal index As Integer, ByVal bit As Integer)
            Dim probability As Integer = models(index)
            Dim bound As Long = (_range >> 11) * CLng(probability)

            If bit = 0 Then
                _range = bound
                models(index) = probability + ((ProbabilityTotal - probability) >> ProbabilityMoveBits)
            Else
                _low += bound
                _range -= bound
                models(index) = probability - (probability >> ProbabilityMoveBits)
            End If

            If _range < RangeTop Then
                _range <<= 8
                ShiftLow()
            End If
        End Sub

        Public Sub Finish()
            For i As Integer = 0 To 4
                ShiftLow()
            Next
        End Sub

        Private Sub ShiftLow()
            Dim low32 As Long = _low And &HFFFFFFFFL
            Dim high As Integer = CInt(_low >> 32)

            If low32 < &HFF000000L OrElse high <> 0 Then
                Dim value As Integer = _cache
                Do
                    _destination.WriteByte(CByte((value + high) And &HFF))
                    value = &HFF
                    _cacheSize -= 1L
                Loop While _cacheSize > 0L
                _cache = CInt((low32 >> 24) And &HFF)
            End If

            _cacheSize += 1L
            _low = (low32 And &HFFFFFFL) << 8
        End Sub
    End Class
End Class

''' <summary>
''' Reads standard (and ZIP64) ZIP archives, extracting either every entry or a
''' caller-supplied subset. When there is no ZIP end-of-central-directory record,
''' a configured 7-Zip command-line tool is used as a fallback for embedded 7z
''' archives. ZIP entries are decompressed in fixed-size chunks straight to their
''' output file (constant memory use regardless of entry size), with CRC-32
''' verification and progress reported through ProgressChanged.
''' </summary>
Public Class ZipExtractor

    ''' <summary>
    ''' Raised during extraction. ZIP operations report exact entry counts; the
    ''' 7-Zip fallback reports parsed percentages and current names instead.
    ''' </summary>
    Public Event ProgressChanged As ZipProgressEventHandler

    Private _sevenZipExecutablePath As String = "7zr.exe"

    ''' <summary>
    ''' Path to the 7-Zip command-line extractor used when the input has no ZIP
    ''' end-of-central-directory record. Defaults to 7zr.exe, resolved using PATH.
    ''' </summary>
    Public Property SevenZipExecutablePath() As String
        Get
            Return _sevenZipExecutablePath
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) Then
                Throw New ArgumentException("A 7-Zip executable path must be supplied.", "value")
            End If
            _sevenZipExecutablePath = value
        End Set
    End Property

    ''' <summary>Extracts every entry in the archive into destinationFolder, recreating any subfolders.</summary>
    Public Sub ExtractAll(ByVal zipFilePath As String, ByVal destinationFolder As String)
        ExtractEntries(zipFilePath, destinationFolder, Nothing)
    End Sub

    ''' <summary>
    ''' Extracts only the named entries. Names are matched case-insensitively against
    ''' the path stored in the archive (forward slashes); backslashes in the names you
    ''' pass in are normalized automatically, so "sub\file.txt" and "sub/file.txt" both work.
    ''' </summary>
    Public Sub ExtractFiles(ByVal zipFilePath As String, ByVal destinationFolder As String, ByVal fileNamesToExtract As String())
        If fileNamesToExtract Is Nothing OrElse fileNamesToExtract.Length = 0 Then
            Throw New ArgumentException("At least one file name must be supplied.", "fileNamesToExtract")
        End If

        ExtractEntries(zipFilePath, destinationFolder, fileNamesToExtract)
    End Sub

    Private Sub ExtractEntries(ByVal zipFilePath As String, ByVal destinationFolder As String, ByVal fileNamesFilter As String())
        Using probeStream As New FileStream(zipFilePath, FileMode.Open, FileAccess.Read)
            If FindEndOfCentralDirectory(probeStream) < 0L Then
                ExtractWithSevenZip(zipFilePath, destinationFolder, fileNamesFilter)
                Return
            End If
        End Using

        Dim allEntries As List(Of ZipCentralDirectoryEntry) = ReadCentralDirectory(zipFilePath)
        Dim entriesToExtract As New List(Of ZipCentralDirectoryEntry)()

        If fileNamesFilter Is Nothing Then
            entriesToExtract.AddRange(allEntries)
        Else
            For Each entry As ZipCentralDirectoryEntry In allEntries
                If IsNameInFilter(entry.EntryName, fileNamesFilter) Then
                    entriesToExtract.Add(entry)
                End If
            Next
        End If

        If Not Directory.Exists(destinationFolder) Then
            Directory.CreateDirectory(destinationFolder)
        End If

        Using inputStream As New FileStream(zipFilePath, FileMode.Open, FileAccess.Read)
            Using reader As New BinaryReader(inputStream)
                For entryIndex As Integer = 0 To entriesToExtract.Count - 1
                    Dim entry As ZipCentralDirectoryEntry = entriesToExtract(entryIndex)

                    ExtractSingleEntry(reader, entry, destinationFolder)

                    RaiseEvent ProgressChanged(Me, New ZipProgressEventArgs(entry.EntryName, entryIndex + 1, entriesToExtract.Count))
                Next
            End Using
        End Using
    End Sub

    Private Sub ExtractWithSevenZip(ByVal archivePath As String, ByVal destinationFolder As String, ByVal fileNamesFilter As String())
        Dim fullArchivePath As String = Path.GetFullPath(archivePath)
        Dim fullDestinationFolder As String = Path.GetFullPath(destinationFolder)

        If Not Directory.Exists(fullDestinationFolder) Then
            Directory.CreateDirectory(fullDestinationFolder)
        End If

        Dim arguments As New StringBuilder()
        arguments.Append("x -y -aoa -bsp1 -bb1 ")
        arguments.Append(QuoteCommandLineArgument("-o" & fullDestinationFolder))
        arguments.Append(" ")
        arguments.Append(QuoteCommandLineArgument(fullArchivePath))

        If fileNamesFilter IsNot Nothing Then
            For Each fileName As String In fileNamesFilter
                If String.IsNullOrEmpty(fileName) Then
                    Throw New ArgumentException("File-name filters cannot be empty.", "fileNamesFilter")
                End If
                arguments.Append(" ")
                arguments.Append(QuoteCommandLineArgument(fileName))
            Next
        End If

        Dim progressParser As New SevenZipProgressParser()
        AddHandler progressParser.ProgressChanged, AddressOf OnSevenZipProgressChanged

        Dim startInfo As New ProcessStartInfo()
        startInfo.FileName = _sevenZipExecutablePath
        startInfo.Arguments = arguments.ToString()
        startInfo.UseShellExecute = False
        startInfo.CreateNoWindow = True
        startInfo.RedirectStandardOutput = True
        startInfo.RedirectStandardError = True
        startInfo.WorkingDirectory = Path.GetDirectoryName(fullArchivePath)

        Using sevenZipProcess As New Process()
            sevenZipProcess.StartInfo = startInfo
            AddHandler sevenZipProcess.OutputDataReceived, AddressOf progressParser.OnOutputDataReceived
            AddHandler sevenZipProcess.ErrorDataReceived, AddressOf progressParser.OnErrorDataReceived

            Try
                If Not sevenZipProcess.Start() Then
                    Throw New InvalidOperationException("The 7-Zip extraction process could not be started.")
                End If
            Catch ex As System.ComponentModel.Win32Exception
                Throw New InvalidOperationException("Could not start the 7-Zip executable '" & _sevenZipExecutablePath & "'. Set SevenZipExecutablePath to a valid 7zr.exe path or add 7zr.exe to PATH.", ex)
            Catch ex As InvalidOperationException
                Throw New InvalidOperationException("Could not start the 7-Zip executable '" & _sevenZipExecutablePath & "'. Set SevenZipExecutablePath to a valid 7zr.exe path or add 7zr.exe to PATH.", ex)
            End Try

            sevenZipProcess.BeginOutputReadLine()
            sevenZipProcess.BeginErrorReadLine()
            sevenZipProcess.WaitForExit()
            sevenZipProcess.WaitForExit()

            If sevenZipProcess.ExitCode <> 0 Then
                Dim errorDetails As String = progressParser.GetOutputTail()
                If String.IsNullOrEmpty(errorDetails) Then
                    errorDetails = "No diagnostic output was reported."
                End If
                Throw New InvalidOperationException("7-Zip failed to extract '" & fullArchivePath & "' (exit code " & sevenZipProcess.ExitCode.ToString(CultureInfo.InvariantCulture) & "). " & errorDetails)
            End If
        End Using

        RaiseEvent ProgressChanged(Me, New ZipProgressEventArgs(String.Empty, 100))
    End Sub

    Private Sub OnSevenZipProgressChanged(ByVal sender As Object, ByVal e As ZipProgressEventArgs)
        RaiseEvent ProgressChanged(Me, e)
    End Sub

    Private Shared Function QuoteCommandLineArgument(ByVal argument As String) As String
        Dim result As New StringBuilder()
        result.Append(""""c)

        Dim backslashCount As Integer = 0
        For Each character As Char In argument
            If character = "\"c Then
                backslashCount += 1
            ElseIf character = """"c Then
                result.Append("\"c, backslashCount * 2 + 1)
                result.Append(""""c)
                backslashCount = 0
            Else
                result.Append("\"c, backslashCount)
                result.Append(character)
                backslashCount = 0
            End If
        Next

        result.Append("\"c, backslashCount * 2)
        result.Append(""""c)
        Return result.ToString()
    End Function

    ''' <summary>
    ''' Parses 7-Zip's percentage and file-name output. File counts are deliberately
    ''' not synthesized: console output does not reliably signal completed files.
    ''' </summary>
    Private NotInheritable Class SevenZipProgressParser
        Private ReadOnly _outputTail As New StringBuilder()
        Private _currentFile As String = String.Empty
        Private _percentage As Integer
        Private ReadOnly _syncRoot As New Object()

        Public Event ProgressChanged As ZipProgressEventHandler

        Public Sub OnOutputDataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
            ProcessLine(e.Data)
        End Sub

        Public Sub OnErrorDataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
            ProcessLine(e.Data)
        End Sub

        Public Function GetOutputTail() As String
            SyncLock _syncRoot
                Return _outputTail.ToString().Trim()
            End SyncLock
        End Function

        Private Sub ProcessLine(ByVal line As String)
            If line Is Nothing Then Return

            Dim progressEvent As ZipProgressEventArgs = Nothing
            SyncLock _syncRoot
                _outputTail.AppendLine(line)
                If _outputTail.Length > 8192 Then
                    _outputTail.Remove(0, _outputTail.Length - 8192)
                End If

                Dim trimmedLine As String = line.Trim()
                If trimmedLine.StartsWith("- ", StringComparison.Ordinal) Then
                    Dim fileName As String = trimmedLine.Substring(2).Trim()
                    If Not String.Equals(fileName, _currentFile, StringComparison.Ordinal) Then
                        _currentFile = fileName
                        progressEvent = New ZipProgressEventArgs(_currentFile, _percentage)
                    End If
                Else
                    Dim percentPosition As Integer = trimmedLine.IndexOf("%"c)
                    If percentPosition > 0 Then
                        Dim parsedPercentage As Integer
                        If Integer.TryParse(trimmedLine.Substring(0, percentPosition).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, parsedPercentage) AndAlso parsedPercentage >= _percentage AndAlso parsedPercentage <= 100 Then
                            If parsedPercentage <> _percentage Then
                                _percentage = parsedPercentage
                                progressEvent = New ZipProgressEventArgs(_currentFile, _percentage)
                            End If
                        End If
                    End If
                End If
            End SyncLock

            If progressEvent IsNot Nothing Then
                RaiseEvent ProgressChanged(Me, progressEvent)
            End If
        End Sub
    End Class

    Private Function IsNameInFilter(ByVal entryName As String, ByVal filter As String()) As Boolean
        For Each candidate As String In filter
            Dim normalizedCandidate As String = candidate.Replace("\"c, "/"c)
            If String.Equals(normalizedCandidate, entryName, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub ExtractSingleEntry(ByVal reader As BinaryReader, ByVal entry As ZipCentralDirectoryEntry, ByVal destinationFolder As String)
        ' Bit 0 of the general purpose flags marks an encrypted entry.
        If (entry.GeneralPurposeFlags And 1US) <> 0US Then
            Throw New NotSupportedException("Entry '" & entry.EntryName & "' is encrypted; encrypted entries are not supported.")
        End If

        ' Archives from other tools (SoftPAQs included) are untrusted input: refuse any entry
        ' name ("..\..\x", "C:\x", "/x") that would land outside the destination folder.
        Dim destinationPath As String = Path.GetFullPath(Path.Combine(destinationFolder, entry.EntryName.Replace("/"c, Path.DirectorySeparatorChar)))
        Dim destinationRoot As String = Path.GetFullPath(destinationFolder)
        If Not destinationRoot.EndsWith(Path.DirectorySeparatorChar.ToString()) Then
            destinationRoot &= Path.DirectorySeparatorChar
        End If
        If Not destinationPath.StartsWith(destinationRoot, StringComparison.OrdinalIgnoreCase) Then
            Throw New InvalidDataException("Entry '" & entry.EntryName & "' would be extracted outside the destination folder and was refused.")
        End If

        ' Third-party archives usually contain explicit folder entries (names ending in "/").
        ' They have no data, so just make sure the folder exists.
        If entry.EntryName.EndsWith("/") Then
            If Not Directory.Exists(destinationPath) Then
                Directory.CreateDirectory(destinationPath)
            End If
            Return
        End If

        reader.BaseStream.Seek(entry.LocalHeaderOffset, SeekOrigin.Begin)

        Dim signature As UInteger = reader.ReadUInt32()
        If signature <> ZipConstants.LocalFileHeaderSignature Then
            Throw New InvalidDataException("Local file header signature mismatch for entry '" & entry.EntryName & "'.")
        End If

        reader.ReadUInt16()                      ' Version needed
        reader.ReadUInt16()                      ' General purpose bit flag
        reader.ReadUInt16()                      ' Compression method (already known from central directory)
        reader.ReadUInt16()                      ' Last mod time
        reader.ReadUInt16()                      ' Last mod date
        reader.ReadUInt32()                      ' CRC-32
        reader.ReadUInt32()                      ' Compressed size
        reader.ReadUInt32()                      ' Uncompressed size
        Dim nameLength As UShort = reader.ReadUInt16()
        Dim extraLength As UShort = reader.ReadUInt16()

        reader.BaseStream.Seek(CLng(nameLength) + CLng(extraLength), SeekOrigin.Current)

        Dim destinationDirectory As String = Path.GetDirectoryName(destinationPath)

        If Not String.IsNullOrEmpty(destinationDirectory) AndAlso Not Directory.Exists(destinationDirectory) Then
            Directory.CreateDirectory(destinationDirectory)
        End If

        Dim runningCrc As UInteger = Crc32Calculator.InitialValue()
        Dim totalWritten As Long = 0L
        Dim buffer As Byte() = New Byte(81919) {} ' 80 KB chunks; memory use stays constant regardless of entry size

        Using destinationStream As New FileStream(destinationPath, FileMode.Create, FileAccess.Write)
            If entry.CompressionMethod = 8US Then
                ' BoundedStream stops DeflateStream from ever reading past this entry's
                ' compressed bytes into whatever follows it in the archive.
                Using boundedSource As New BoundedStream(reader.BaseStream, entry.CompressedSize)
                    Using deflateStream As New DeflateStream(boundedSource, CompressionMode.Decompress, True)
                        Dim bytesRead As Integer = deflateStream.Read(buffer, 0, buffer.Length)
                        Do While bytesRead > 0
                            runningCrc = Crc32Calculator.UpdateCrc32(runningCrc, buffer, 0, bytesRead)
                            destinationStream.Write(buffer, 0, bytesRead)
                            totalWritten += bytesRead
                            bytesRead = deflateStream.Read(buffer, 0, buffer.Length)
                        Loop
                    End Using
                End Using
            ElseIf entry.CompressionMethod = ZipConstants.LzmaCompressionMethod Then
                Using boundedSource As New BoundedStream(reader.BaseStream, entry.CompressedSize)
                    ZipLzmaDecoder.DecompressLzma(boundedSource, destinationStream, entry.UncompressedSize, runningCrc, totalWritten)
                End Using
            ElseIf entry.CompressionMethod = ZipConstants.Lzma2CompressionMethod Then
                Using boundedSource As New BoundedStream(reader.BaseStream, entry.CompressedSize)
                    ZipLzmaDecoder.DecompressLzma2(boundedSource, destinationStream, entry.UncompressedSize, runningCrc, totalWritten)
                End Using
            ElseIf entry.CompressionMethod = 0US Then
                Dim remaining As Long = entry.CompressedSize
                Do While remaining > 0L
                    Dim toRead As Integer = CInt(Math.Min(CLng(buffer.Length), remaining))
                    Dim bytesRead As Integer = reader.BaseStream.Read(buffer, 0, toRead)
                    If bytesRead = 0 Then
                        Exit Do
                    End If
                    runningCrc = Crc32Calculator.UpdateCrc32(runningCrc, buffer, 0, bytesRead)
                    destinationStream.Write(buffer, 0, bytesRead)
                    totalWritten += bytesRead
                    remaining -= bytesRead
                Loop
            Else
                Throw New NotSupportedException("Compression method " & entry.CompressionMethod.ToString() & " is not supported.")
            End If
        End Using

        Dim finalCrc As UInteger = Crc32Calculator.FinalizeCrc32(runningCrc)
        If finalCrc <> entry.Crc32 OrElse totalWritten <> entry.UncompressedSize Then
            Try
                File.Delete(destinationPath)
            Catch
            End Try
            Throw New InvalidDataException("CRC-32 or size mismatch for entry '" & entry.EntryName & "'. The archive may be corrupt.")
        End If
    End Sub

    ''' <summary>
    ''' Reads every central directory entry. Also handles archives that are appended
    ''' to another file (self-extracting EXEs such as HP SoftPAQs): in that case the
    ''' offsets stored in the ZIP structures are relative to where the ZIP data starts,
    ''' not to the start of the file, so every offset is corrected by the size of the
    ''' stub in front of the ZIP data. Archives that start at byte 0 get a stub size of 0
    ''' and behave exactly as before. Offsets that were already absolute (some SFX tools
    ''' rewrite them) are detected too and left alone.
    ''' </summary>
    Private Function ReadCentralDirectory(ByVal zipFilePath As String) As List(Of ZipCentralDirectoryEntry)
        Using inputStream As New FileStream(zipFilePath, FileMode.Open, FileAccess.Read)
            Dim eocdOffset As Long = FindEndOfCentralDirectory(inputStream)
            If eocdOffset < 0 Then
                Throw New InvalidDataException("This does not appear to be a valid ZIP file (end of central directory record not found).")
            End If

            Using reader As New BinaryReader(inputStream)
                inputStream.Seek(eocdOffset + 4, SeekOrigin.Begin) ' skip the 4-byte signature

                reader.ReadUInt16()                          ' Number of this disk
                reader.ReadUInt16()                          ' Disk where central directory starts
                reader.ReadUInt16()                          ' Records on this disk
                Dim totalEntriesField As UShort = reader.ReadUInt16()
                Dim centralDirectorySizeField As UInteger = reader.ReadUInt32()
                Dim centralDirectoryOffsetField As UInteger = reader.ReadUInt32()

                Dim actualTotalEntries As Long = CLng(totalEntriesField)
                Dim actualCentralDirectorySize As Long = CLng(centralDirectorySizeField)
                Dim actualCentralDirectoryOffset As Long = CLng(centralDirectoryOffsetField)

                ' Where the central directory ends in the file (= where the next record begins).
                ' Used below to work out how many bytes of EXE stub precede the ZIP data.
                Dim endOfCentralDirectoryData As Long = eocdOffset

                Dim usesZip64 As Boolean = (totalEntriesField = ZipConstants.UInt16Max) OrElse (centralDirectorySizeField = ZipConstants.UInt32Max) OrElse (centralDirectoryOffsetField = ZipConstants.UInt32Max)

                If usesZip64 Then
                    ' The ZIP64 locator is a fixed 20-byte structure immediately
                    ' preceding the standard end-of-central-directory record.
                    Dim locatorPosition As Long = eocdOffset - 20L
                    If locatorPosition < 0L Then
                        Throw New InvalidDataException("Expected a ZIP64 end of central directory locator but there is no room for one.")
                    End If

                    inputStream.Seek(locatorPosition, SeekOrigin.Begin)

                    Dim locatorSignature As UInteger = reader.ReadUInt32()
                    If locatorSignature <> ZipConstants.Zip64EndOfCentralDirectoryLocatorSignature Then
                        Throw New InvalidDataException("Expected a ZIP64 end of central directory locator but did not find one.")
                    End If

                    reader.ReadUInt32()                      ' Disk with start of ZIP64 EOCD
                    Dim zip64EocdOffset As Long = reader.ReadInt64()
                    reader.ReadUInt32()                       ' Total number of disks

                    ' The offset in the locator may be relative to the start of the ZIP data
                    ' (embedded archive). If the signature isn't where it says, fall back to the
                    ' standard position: the 56-byte record sits directly before the locator.
                    Dim zip64RecordPosition As Long = zip64EocdOffset
                    If Not HasSignatureAt(reader, zip64RecordPosition, ZipConstants.Zip64EndOfCentralDirectorySignature) Then
                        zip64RecordPosition = locatorPosition - 56L
                        If Not HasSignatureAt(reader, zip64RecordPosition, ZipConstants.Zip64EndOfCentralDirectorySignature) Then
                            Throw New InvalidDataException("Expected a ZIP64 end of central directory record but did not find one.")
                        End If
                    End If

                    inputStream.Seek(zip64RecordPosition + 4L, SeekOrigin.Begin) ' skip the signature

                    reader.ReadInt64()                        ' Size of remaining ZIP64 EOCD record
                    reader.ReadUInt16()                        ' Version made by
                    reader.ReadUInt16()                        ' Version needed to extract
                    reader.ReadUInt32()                        ' Number of this disk
                    reader.ReadUInt32()                        ' Disk with start of central directory
                    reader.ReadInt64()                         ' Total entries on this disk
                    actualTotalEntries = reader.ReadInt64()    ' Total entries overall
                    actualCentralDirectorySize = reader.ReadInt64()
                    actualCentralDirectoryOffset = reader.ReadInt64()

                    endOfCentralDirectoryData = zip64RecordPosition
                End If

                ' --- Embedded-archive (EXE stub) correction ---
                ' For a plain .zip, offset + size lands exactly on the end-of-central-directory
                ' data, so the stub size is 0. For a ZIP appended to an EXE with relative offsets,
                ' the difference is exactly the number of bytes in front of the ZIP data. The
                ' result is only trusted if a central directory header really is at the corrected
                ' position, so unusual layouts fall back to the stored offsets unchanged.
                Dim stubSize As Long = 0L
                Dim candidateStubSize As Long = endOfCentralDirectoryData - actualCentralDirectorySize - actualCentralDirectoryOffset
                If candidateStubSize > 0L Then
                    If HasSignatureAt(reader, actualCentralDirectoryOffset + candidateStubSize, ZipConstants.CentralDirectoryHeaderSignature) Then
                        stubSize = candidateStubSize
                    End If
                End If

                inputStream.Seek(actualCentralDirectoryOffset + stubSize, SeekOrigin.Begin)

                Dim entries As New List(Of ZipCentralDirectoryEntry)()

                For i As Long = 0L To actualTotalEntries - 1L
                    Dim signature As UInteger = reader.ReadUInt32()
                    If signature <> ZipConstants.CentralDirectoryHeaderSignature Then
                        Throw New InvalidDataException("Central directory header signature mismatch at entry " & i.ToString() & ".")
                    End If

                    reader.ReadUInt16()                       ' Version made by
                    reader.ReadUInt16()                        ' Version needed
                    Dim generalPurposeFlags As UShort = reader.ReadUInt16()
                    Dim compressionMethod As UShort = reader.ReadUInt16()
                    Dim dosTime As UShort = reader.ReadUInt16()
                    Dim dosDate As UShort = reader.ReadUInt16()
                    Dim crc As UInteger = reader.ReadUInt32()
                    Dim compressedSizeField As UInteger = reader.ReadUInt32()
                    Dim uncompressedSizeField As UInteger = reader.ReadUInt32()
                    Dim nameLength As UShort = reader.ReadUInt16()
                    Dim extraLength As UShort = reader.ReadUInt16()
                    Dim commentLength As UShort = reader.ReadUInt16()
                    reader.ReadUInt16()                        ' Disk number start
                    reader.ReadUInt16()                        ' Internal file attributes
                    reader.ReadUInt32()                         ' External file attributes
                    Dim localHeaderOffsetField As UInteger = reader.ReadUInt32()

                    Dim nameBytes As Byte() = reader.ReadBytes(CInt(nameLength))
                    Dim extraBytes As Byte() = reader.ReadBytes(CInt(extraLength))
                    reader.BaseStream.Seek(CLng(commentLength), SeekOrigin.Current)

                    Dim compressedSize As Long = CLng(compressedSizeField)
                    Dim uncompressedSize As Long = CLng(uncompressedSizeField)
                    Dim localHeaderOffset As Long = CLng(localHeaderOffsetField)

                    If compressedSizeField = ZipConstants.UInt32Max OrElse uncompressedSizeField = ZipConstants.UInt32Max OrElse localHeaderOffsetField = ZipConstants.UInt32Max Then
                        ParseZip64ExtraField(extraBytes, uncompressedSizeField = ZipConstants.UInt32Max, compressedSizeField = ZipConstants.UInt32Max, localHeaderOffsetField = ZipConstants.UInt32Max, uncompressedSize, compressedSize, localHeaderOffset)
                    End If

                    Dim entry As New ZipCentralDirectoryEntry()
                    entry.EntryName = DecodeEntryName(nameBytes, generalPurposeFlags)
                    entry.GeneralPurposeFlags = generalPurposeFlags
                    entry.Crc32 = crc
                    entry.CompressedSize = compressedSize
                    entry.UncompressedSize = uncompressedSize
                    entry.LocalHeaderOffset = localHeaderOffset + stubSize
                    entry.CompressionMethod = compressionMethod
                    entry.DosTime = dosTime
                    entry.DosDate = dosDate

                    entries.Add(entry)
                Next

                Return entries
            End Using
        End Using
    End Function

    ''' <summary>True if the 4 bytes at the given file position equal the expected record signature.</summary>
    Private Function HasSignatureAt(ByVal reader As BinaryReader, ByVal position As Long, ByVal expectedSignature As UInteger) As Boolean
        If position < 0L OrElse position + 4L > reader.BaseStream.Length Then
            Return False
        End If

        reader.BaseStream.Seek(position, SeekOrigin.Begin)
        Return (reader.ReadUInt32() = expectedSignature)
    End Function

    ''' <summary>
    ''' Decodes an entry name. Third-party tools set general purpose bit 11 (0x0800) when the
    ''' name is UTF-8; otherwise the ZIP specification says the name is IBM code page 437.
    ''' Names are normalized to forward slashes, since a few older tools store backslashes.
    ''' </summary>
    Private Function DecodeEntryName(ByVal nameBytes As Byte(), ByVal generalPurposeFlags As UShort) As String
        Dim decoded As String

        If (generalPurposeFlags And &H800US) <> 0US Then
            decoded = Encoding.UTF8.GetString(nameBytes)
        Else
            Try
                decoded = Encoding.GetEncoding(437).GetString(nameBytes)
            Catch ex As ArgumentException
                decoded = Encoding.ASCII.GetString(nameBytes)
            End Try
        End If

        Return decoded.Replace("\"c, "/"c)
    End Function

    ''' <summary>
    ''' Reads the true 64-bit size/offset values out of a central directory
    ''' entry's ZIP64 extra field. Fields are only present -- and only read --
    ''' for whichever of uncompressed size / compressed size / local header
    ''' offset were flagged as overflowed (0xFFFFFFFF) in the fixed header,
    ''' always in that same order, matching how WriteCentralDirectoryHeader wrote them.
    ''' </summary>
    Private Sub ParseZip64ExtraField(ByVal extraBytes As Byte(), ByVal needUncompressed As Boolean, ByVal needCompressed As Boolean, ByVal needOffset As Boolean, ByRef uncompressedSize As Long, ByRef compressedSize As Long, ByRef localHeaderOffset As Long)
        Dim position As Integer = 0

        Do While position + 4 <= extraBytes.Length
            Dim tag As UShort = BitConverter.ToUInt16(extraBytes, position)
            Dim dataSize As UShort = BitConverter.ToUInt16(extraBytes, position + 2)
            Dim dataStart As Integer = position + 4

            If tag = ZipConstants.Zip64ExtraFieldTag Then
                Dim cursor As Integer = dataStart
                If needUncompressed Then
                    uncompressedSize = BitConverter.ToInt64(extraBytes, cursor)
                    cursor += 8
                End If
                If needCompressed Then
                    compressedSize = BitConverter.ToInt64(extraBytes, cursor)
                    cursor += 8
                End If
                If needOffset Then
                    localHeaderOffset = BitConverter.ToInt64(extraBytes, cursor)
                    cursor += 8
                End If
                Exit Do
            End If

            position = dataStart + CInt(dataSize)
        Loop
    End Sub

    ''' <summary>
    ''' Locates the End Of Central Directory record by scanning backward from the
    ''' end of the file for its signature (PK\x05\x06). A backward scan is required
    ''' because the record can be preceded by an optional comment of unknown length.
    ''' </summary>
    Private Function FindEndOfCentralDirectory(ByVal stream As FileStream) As Long
        Const maxCommentSize As Integer = 65536
        Const eocdMinSize As Integer = 22

        Dim searchLength As Integer = CInt(Math.Min(stream.Length, CLng(maxCommentSize + eocdMinSize)))
        Dim buffer As Byte() = New Byte(searchLength - 1) {}

        stream.Seek(-searchLength, SeekOrigin.End)
        stream.Read(buffer, 0, searchLength)

        For i As Integer = buffer.Length - eocdMinSize To 0 Step -1
            If buffer(i) = &H50 AndAlso buffer(i + 1) = &H4B AndAlso buffer(i + 2) = &H5 AndAlso buffer(i + 3) = &H6 Then
                Return (stream.Length - searchLength) + i
            End If
        Next

        Return -1
    End Function
End Class

''' <summary>
''' Lightweight post-creation sanity check: confirms the archive's end-of-central-
''' directory record is reachable and that every central directory entry's recorded
''' name/extra lengths consume exactly as many bytes as are actually present, without
''' extracting anything. Call this right after CreateZip finishes to catch a truncated
''' or corrupt archive (e.g. a write that got cut short by a file-system size limit)
''' immediately, instead of discovering it later via ExtractAll/ExtractFiles.
''' Returns a human-readable report; an empty string means the archive looks sound.
''' NOTE: this checks plain .zip files that start at byte 0, such as the ones CreateZip
''' produces. It does not apply the EXE-stub correction that ZipExtractor uses for
''' self-extracting archives like SoftPAQs.
''' </summary>
Public NotInheritable Class ZipDiagnostics
    Private Sub New()
    End Sub

    Public Shared Function ValidateArchive(ByVal zipFilePath As String) As String
        Dim report As New System.Text.StringBuilder()
        Dim fileInfo As New FileInfo(zipFilePath)
        report.AppendLine("File size on disk: " & fileInfo.Length.ToString() & " bytes")

        Try
            Using inputStream As New FileStream(zipFilePath, FileMode.Open, FileAccess.Read)
                Using reader As New BinaryReader(inputStream)
                    Dim eocdOffset As Long = FindEndOfCentralDirectory(inputStream)
                    If eocdOffset < 0 Then
                        report.AppendLine("FAIL: End of central directory record not found. The file is likely truncated.")
                        Return report.ToString()
                    End If
                    report.AppendLine("End of central directory found at offset " & eocdOffset.ToString())

                    inputStream.Seek(eocdOffset + 4, SeekOrigin.Begin)
                    reader.ReadUInt16()
                    reader.ReadUInt16()
                    reader.ReadUInt16()
                    Dim totalEntriesField As UShort = reader.ReadUInt16()
                    Dim centralDirectorySizeField As UInteger = reader.ReadUInt32()
                    Dim centralDirectoryOffsetField As UInteger = reader.ReadUInt32()

                    Dim actualTotalEntries As Long = CLng(totalEntriesField)
                    Dim actualCentralDirectoryOffset As Long = CLng(centralDirectoryOffsetField)
                    Dim usesZip64 As Boolean = (totalEntriesField = ZipConstants.UInt16Max) OrElse (centralDirectorySizeField = ZipConstants.UInt32Max) OrElse (centralDirectoryOffsetField = ZipConstants.UInt32Max)

                    If usesZip64 Then
                        report.AppendLine("Archive uses ZIP64 (entry count, central directory size, or offset exceeds classic limits).")
                        Dim locatorPosition As Long = eocdOffset - 20L
                        If locatorPosition < 0L Then
                            report.AppendLine("FAIL: Not enough room before the EOCD record for a ZIP64 locator.")
                            Return report.ToString()
                        End If

                        inputStream.Seek(locatorPosition, SeekOrigin.Begin)
                        Dim locatorSignature As UInteger = reader.ReadUInt32()
                        If locatorSignature <> ZipConstants.Zip64EndOfCentralDirectoryLocatorSignature Then
                            report.AppendLine("FAIL: ZIP64 locator signature mismatch at offset " & locatorPosition.ToString() & ".")
                            Return report.ToString()
                        End If

                        reader.ReadUInt32()
                        Dim zip64EocdOffset As Long = reader.ReadInt64()
                        reader.ReadUInt32()

                        inputStream.Seek(zip64EocdOffset, SeekOrigin.Begin)
                        Dim zip64Signature As UInteger = reader.ReadUInt32()
                        If zip64Signature <> ZipConstants.Zip64EndOfCentralDirectorySignature Then
                            report.AppendLine("FAIL: ZIP64 end of central directory record signature mismatch at offset " & zip64EocdOffset.ToString() & ".")
                            Return report.ToString()
                        End If

                        reader.ReadInt64()
                        reader.ReadUInt16()
                        reader.ReadUInt16()
                        reader.ReadUInt32()
                        reader.ReadUInt32()
                        reader.ReadInt64()
                        actualTotalEntries = reader.ReadInt64()
                        reader.ReadInt64()
                        actualCentralDirectoryOffset = reader.ReadInt64()
                        report.AppendLine("ZIP64 record reports " & actualTotalEntries.ToString() & " entries, central directory at offset " & actualCentralDirectoryOffset.ToString() & ".")
                    End If

                    If actualCentralDirectoryOffset < 0L OrElse actualCentralDirectoryOffset > fileInfo.Length Then
                        report.AppendLine("FAIL: Central directory offset (" & actualCentralDirectoryOffset.ToString() & ") is outside the file (length " & fileInfo.Length.ToString() & "). The file is almost certainly truncated.")
                        Return report.ToString()
                    End If

                    inputStream.Seek(actualCentralDirectoryOffset, SeekOrigin.Begin)

                    For i As Long = 0L To actualTotalEntries - 1L
                        Dim entryStartOffset As Long = inputStream.Position

                        If entryStartOffset + 46L > fileInfo.Length Then
                            report.AppendLine("FAIL: Entry " & i.ToString() & " header starts at " & entryStartOffset.ToString() & " but there isn't room for a full 46-byte header before end of file (length " & fileInfo.Length.ToString() & "). The file is truncated here.")
                            Return report.ToString()
                        End If

                        Dim signature As UInteger = reader.ReadUInt32()
                        If signature <> ZipConstants.CentralDirectoryHeaderSignature Then
                            report.AppendLine("FAIL: Entry " & i.ToString() & " signature mismatch at offset " & entryStartOffset.ToString() & " (read 0x" & signature.ToString("X8") & ", expected 0x" & ZipConstants.CentralDirectoryHeaderSignature.ToString("X8") & "). Previous entry's recorded lengths likely don't match its actual size.")
                            Return report.ToString()
                        End If

                        reader.ReadUInt16()
                        reader.ReadUInt16()
                        reader.ReadUInt16()
                        reader.ReadUInt16()
                        reader.ReadUInt16()
                        reader.ReadUInt16()
                        reader.ReadUInt32()
                        reader.ReadUInt32()
                        reader.ReadUInt32()
                        Dim nameLength As UShort = reader.ReadUInt16()
                        Dim extraLength As UShort = reader.ReadUInt16()
                        Dim commentLength As UShort = reader.ReadUInt16()
                        reader.ReadUInt16()
                        reader.ReadUInt16()
                        reader.ReadUInt32()
                        reader.ReadUInt32()

                        Dim remainingVariableBytes As Long = CLng(nameLength) + CLng(extraLength) + CLng(commentLength)
                        If inputStream.Position + remainingVariableBytes > fileInfo.Length Then
                            report.AppendLine("FAIL: Entry " & i.ToString() & "'s name/extra/comment fields (name=" & nameLength.ToString() & ", extra=" & extraLength.ToString() & ", comment=" & commentLength.ToString() & ") would run past end of file. The file is truncated.")
                            Return report.ToString()
                        End If

                        Dim nameBytes As Byte() = reader.ReadBytes(CInt(nameLength))
                        reader.BaseStream.Seek(CLng(extraLength) + CLng(commentLength), SeekOrigin.Current)

                        report.AppendLine("Entry " & i.ToString() & ": '" & Encoding.ASCII.GetString(nameBytes) & "' OK (header at " & entryStartOffset.ToString() & ", name=" & nameLength.ToString() & " extra=" & extraLength.ToString() & " bytes)")
                    Next

                    report.AppendLine("All " & actualTotalEntries.ToString() & " central directory entries parsed cleanly.")
                End Using
            End Using
        Catch ex As Exception
            report.AppendLine("FAIL: Exception during validation: " & ex.GetType().Name & " - " & ex.Message)
        End Try

        Return report.ToString()
    End Function

    Private Shared Function FindEndOfCentralDirectory(ByVal stream As FileStream) As Long
        Const maxCommentSize As Integer = 65536
        Const eocdMinSize As Integer = 22

        Dim searchLength As Integer = CInt(Math.Min(stream.Length, CLng(maxCommentSize + eocdMinSize)))
        Dim buffer As Byte() = New Byte(searchLength - 1) {}

        stream.Seek(-searchLength, SeekOrigin.End)
        stream.Read(buffer, 0, searchLength)

        For i As Integer = buffer.Length - eocdMinSize To 0 Step -1
            If buffer(i) = &H50 AndAlso buffer(i + 1) = &H4B AndAlso buffer(i + 2) = &H5 AndAlso buffer(i + 3) = &H6 Then
                Return (stream.Length - searchLength) + i
            End If
        Next

        Return -1
    End Function
End Class