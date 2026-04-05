Namespace Elements.Contemporaneus

    Public Class FullFlashUtilityImage

        Public Property IniManifest As String

        Public Property VhdPath As String
        Public Property VhdId As String
        Public Property VhdStorageDeviceId As Integer
        Public Property MountDiskPath As String

        Public Property FullFlashVersionInfo As New Version(0, 0, 0, 0)
        Public Property VersionInfo As New Version(0, 0, 0, 0)
        Public Property MountVersion As Integer

        Public Property Compression As FFUCompressionType = FFUCompressionType.None

        Public Property OptimizedPartitionNumber As Integer

        Public Function CompressionToString() As String
            Select Case Compression
                Case FFUCompressionType.None
                    Return "No compression"
                Case FFUCompressionType.XPRESSHuffman
                    Return "XPRESS-Huffman"
            End Select
            Return ""
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("- VHD Path                  : {1}{0}" &
                                 "- VHD ID                    : {2}{0}" &
                                 "- VHD Storage Device ID     : {3}{0}" &
                                 "- Mount Disk Path           : {4}{0}" &
                                 "- FFU Version Info          : {5}{0}" &
                                 "- Misc. Version Info        : {6}{0}" &
                                 "- Mount Version             : {7}{0}" &
                                 "- Compression Type          : {8}{0}" &
                                 "- Optimized Partition Number: {9}",
                                 Environment.NewLine,
                                 VhdPath.Replace(Environment.GetEnvironmentVariable("USERPROFILE"), "<User Folder>"),
                                 VhdId,
                                 VhdStorageDeviceId,
                                 MountDiskPath,
                                 FullFlashVersionInfo.ToString(),
                                 VersionInfo.ToString(),
                                 MountVersion,
                                 Compression,
                                 OptimizedPartitionNumber)
        End Function

    End Class

    ''' <summary>
    ''' The compression type of a FFU file
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum FFUCompressionType As Integer
        ''' <summary>
        ''' No compression
        ''' </summary>
        ''' <remarks></remarks>
        None = 0
        ''' <summary>
        ''' XPRESS-Huffman compression
        ''' </summary>
        ''' <remarks></remarks>
        XPRESSHuffman = 3
    End Enum

End Namespace
