Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Namespace Elements.DriverPacks

    Public Class DriverPackHelper

        Public Shared Function DeserializeDriverPackMetadata(ByVal MetadataXml As String) As DriverPack
            If Not File.Exists(MetadataXml) Then Throw New FileNotFoundException(MetadataXml)

            Dim obtainedPackData As DriverPack = Nothing

            Try
                Dim deserializer As New XmlSerializer(GetType(DriverPack))

                Using fs As New FileStream(MetadataXml, FileMode.Open)
                    obtainedPackData = CType(deserializer.Deserialize(fs), DriverPack)
                End Using
            Catch ex As Exception

            End Try

            Return obtainedPackData
        End Function

        Public Shared Function SerializeDriverPackMetadata(ByVal MetadataXml As String, ByVal DriverMetadata As DriverPack) As Boolean
            If DriverMetadata Is Nothing Then Throw New ArgumentNullException("DriverMetadata")

            Try
                Dim serializer As New XmlSerializer(GetType(DriverPack))
                Using fs As New FileStream(MetadataXml, FileMode.Create)
                    serializer.Serialize(fs, DriverMetadata)
                End Using
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Const PackVersion As String = "0.9"
        Private Const PackCodename As String = "Discovery"

        Public Shared Function IsPackValid(ByVal PackPath As String) As Boolean
            If PackPath = "" Then Throw New ArgumentNullException(PackPath)
            If Not File.Exists(PackPath) Then Throw New FileNotFoundException(PackPath)

            Dim PakDatHeader As String = String.Format("DISMTools {0}{1}{0} Driver Pack File -- Driver Pack File Format {2}", Quote, PackCodename, PackVersion), _
                PackValid As Boolean = False

            Try
                Dim PackTesterExtractor As New ZipExtractor()
                Dim TempExtractedPackPath As String = Path.Combine(Path.GetTempPath(), String.Format("DRVPAK_{0}", New Random().Next()))
                Directory.CreateDirectory(TempExtractedPackPath)
                PackTesterExtractor.ExtractFiles(PackPath, TempExtractedPackPath, New String() {"PAKDAT"})
                If File.Exists(Path.Combine(TempExtractedPackPath, "PAKDAT")) Then
                    PackValid = File.ReadAllText(Path.Combine(TempExtractedPackPath, "PAKDAT")) = PakDatHeader
                End If
                Directory.Delete(TempExtractedPackPath, True)
                Return PackValid
            Catch ex As Exception
                Return False
            End Try
        End Function

    End Class

End Namespace