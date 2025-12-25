Imports System.Diagnostics
Imports System.IO

Module ProcessHelper

    Function RunProcess(FilePath As String, Optional Arguments As String = "", Optional RunAsAdmin As Boolean = False) As Integer
        Dim exitCode As Integer = 0
        Try
            Dim proc As New Process() With {
                .StartInfo = New ProcessStartInfo() With {
                    .FileName = FilePath,
                    .Arguments = Arguments,
                    .WorkingDirectory = Path.GetDirectoryName(FilePath),
                    .CreateNoWindow = True,
                    .Verb = If(RunAsAdmin, "runas", "")
                }
            }
            proc.Start()
            proc.WaitForExit()
            exitCode = proc.ExitCode
        Catch ex As Exception
            exitCode = ex.HResult
        End Try
        Return exitCode
    End Function

End Module
