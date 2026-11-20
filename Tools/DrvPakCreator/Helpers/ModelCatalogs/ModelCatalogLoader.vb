Imports DrvPakCreator.Classes.ModelCatalogs
Imports System.IO
Imports System.Net

Namespace Helpers.ModelCatalogs

    Public MustInherit Class ModelCatalogLoader

        Private Const S_OK As Integer = 0

        Protected MustOverride ReadOnly Property CatalogDownloadPath() As String

        Public MustOverride Function LoadManufacturerCatalog() As ModelCatalog

        Protected Function DownloadFile(ByVal SourceDownloadUri As String, ByVal DestinationDownloadedFile As String) As Boolean
            ' Download capabilities are disabled for .NET 2.0 version because it does not support
            ' TLS 1.2.
            Using client As New WebClient()
#If VBC_VER < 10.0 Then
                Return False
#Else
                Try
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                    client.DownloadFile(SourceDownloadUri, DestinationDownloadedFile)
                    Return True
                Catch webEx as WebException
                    Return False
                End Try
#End If
            End Using
        End Function

        Protected Function ExpandCabArchive(ByVal SourceCabLocation As String, ByVal DestinationExpandedCabLocation As String) As Boolean
            If Not File.Exists(SourceCabLocation) Then Throw New FileNotFoundException(SourceCabLocation)
            If Not Directory.Exists(DestinationExpandedCabLocation) Then
                Try
                    Directory.CreateDirectory(DestinationExpandedCabLocation)
                Catch ex As Exception
                    Throw New DirectoryNotFoundException(DestinationExpandedCabLocation)
                End Try
            End If

            Dim expandProcess As New Process()
            With expandProcess
                .StartInfo = New ProcessStartInfo()
                With .StartInfo
                    .FileName = Path.Combine(Environment.GetEnvironmentVariable("SYSTEMROOT"), "system32\expand.exe")
                    .Arguments = String.Format("-r {0}{1}{0} {0}{2}{0}", Quote, SourceCabLocation, DestinationExpandedCabLocation)
                    .CreateNoWindow = Not Debugger.IsAttached
                    .WindowStyle = IIf(Debugger.IsAttached, ProcessWindowStyle.Normal, ProcessWindowStyle.Hidden)
                End With
            End With
            expandProcess.Start()
            expandProcess.WaitForExit()
            Return expandProcess.ExitCode = S_OK
        End Function

        Protected Function LoadCatalogFile(ByVal CatalogPath As String, ByVal ReferenceType As Type) As Object
            If Not File.Exists(CatalogPath) Then Return Nothing

            Dim obtainedCatalog As Object = Nothing

            Try
                Dim deserializer As New XmlSerializer(ReferenceType)
                Using fs As New FileStream(CatalogPath, FileMode.Open)
                    obtainedCatalog = deserializer.Deserialize(fs)
                End Using
            Catch ex As Exception

            End Try

            Return obtainedCatalog
        End Function

    End Class

End Namespace