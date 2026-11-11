Imports System.IO

Namespace Helpers

    Public Class ExplorerHelper

        Public Shared Sub SelectFileInExplorer(ByVal FilePath As String)
            If Not File.Exists(FilePath) Then Exit Sub

            Try
                Dim explorerProcess As New Process()
                With explorerProcess
                    .StartInfo = New ProcessStartInfo()
                    With .StartInfo
                        .FileName = Path.Combine(Environment.GetEnvironmentVariable("SYSTEMROOT"), "explorer.exe")
                        .Arguments = String.Format("/select,{0}{1}{0}", Quote, FilePath)
                    End With
                End With
                explorerProcess.Start()
            Catch ex As Exception

            End Try
        End Sub

    End Class

End Namespace