Imports System.IO
Imports DrvPakCreator.Classes.ProgressReporting

Namespace Helpers.FileIO

    Public Class PathUtilities

        Public Shared Function GetRecursiveFileList(ByVal SourceDirectory As String, Optional ByVal FileExtension As String = "") As List(Of String)
            If Not Directory.Exists(SourceDirectory) Then Return New List(Of String)

            Dim Files As New List(Of String), _
                SubDirectoriesInDirectory() As String

            ' Enumerate directories first, then files in the current directory
            SubDirectoriesInDirectory = Directory.GetDirectories(SourceDirectory, "*", SearchOption.TopDirectoryOnly)

            Files.AddRange(Directory.GetFiles(SourceDirectory, IIf(FileExtension <> "", FileExtension, "*.*"), SearchOption.TopDirectoryOnly))

            For Each SubDirectoryInDirectory As String In SubDirectoriesInDirectory
                Try
                    Files.AddRange(GetRecursiveFileList(SubDirectoryInDirectory, FileExtension))
                Catch ex As Exception
                    ' don't scan this directory
                End Try
            Next
            Return Files
        End Function

        Public Shared Function GetDirectoryName(ByVal SourceDirectory As String) As String
            If SourceDirectory = "" Then Return ""
            Dim pathParts() As String = SourceDirectory.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            Array.Reverse(pathParts)
            Return pathParts(0)
        End Function

        Public Shared Function CopyRecursive(ByVal SourceDirectory As String, ByVal DestinationDirectory As String, Optional ByVal OverwriteContents As Boolean = False, Optional ByVal ProgressReporter As Action(Of TaskReporter) = Nothing) As Boolean
            If Not Directory.Exists(SourceDirectory) Then Throw New DirectoryNotFoundException()

            ' If the destination folder does not exist, then we try creating it. If we couldn't,
            ' we simply give up.
            If Not Directory.Exists(DestinationDirectory) Then
                Try
                    Directory.CreateDirectory(DestinationDirectory)
                Catch ex As Exception
                    Return False
                End Try
            End If

            Try
                ' Now, we create all the directories of the source folder to the destination
                If ProgressReporter IsNot Nothing Then ProgressReporter.Invoke(New TaskReporter("Enumerating directories...", 0))
                Dim dirsInSource As String() = Directory.GetDirectories(SourceDirectory, "*", SearchOption.AllDirectories)
                For Each dirInSource As String In dirsInSource
                    Dim sourcePath As String = dirInSource.Substring(SourceDirectory.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    Dim destinationPath As String = Path.Combine(DestinationDirectory, sourcePath)

                    If ProgressReporter IsNot Nothing Then ProgressReporter.Invoke(New TaskReporter(String.Format("Creating folder {0}", GetDirectoryName(destinationPath)), 0))

                    If Not Directory.Exists(destinationPath) Then
                        Directory.CreateDirectory(destinationPath)
                    End If
                Next

                ' Next, we copy all the files in the source directory to the destination
                If ProgressReporter IsNot Nothing Then ProgressReporter.Invoke(New TaskReporter("Enumerating files...", 0))
                Dim FilesInDirectory() As String = Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories)

                Dim i As Long = 0, _
                    fileCount As Long = FilesInDirectory.Length

                For Each FileToCopy As String In FilesInDirectory
                    Dim sourcePath As String = FileToCopy.Substring(SourceDirectory.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    Dim destinationPath As String = Path.Combine(DestinationDirectory, sourcePath)

                    If ProgressReporter IsNot Nothing Then ProgressReporter.Invoke(New TaskReporter(String.Format("Copying file {0}", GetDirectoryName(destinationPath)), (i / fileCount) * 100))
                    File.Copy(FileToCopy, destinationPath, OverwriteContents)
                    i += 1
                Next

                If ProgressReporter IsNot Nothing Then ProgressReporter.Invoke(New TaskReporter("Adding Driver to Driver Pack", -1))
            Catch ex As Exception
                Return False
            End Try

            Return True
        End Function

    End Class

End Namespace