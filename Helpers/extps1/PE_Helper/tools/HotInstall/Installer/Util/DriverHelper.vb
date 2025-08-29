Imports System.IO
Imports System.Diagnostics
Imports Microsoft.VisualBasic.ControlChars

Module DriverHelper

    Sub ExportOnlineDrivers(Destination As String)
        If Not Directory.Exists(Destination) Then
            Directory.CreateDirectory(Destination)
        End If
        Dim DismProcess As New Process()
        DismProcess.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "dism.exe")
        DismProcess.StartInfo.Arguments = "/online /export-driver /destination=" & Quote & Destination & Quote
        DismProcess.StartInfo.CreateNoWindow = True
        DismProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        DismProcess.Start()
        DismProcess.WaitForExit()
        If DismProcess.ExitCode <> 0 Then
            Throw New Exception(String.Format(GetValueFromLanguageData("MainForm.DriverExporter_FailureMessage"), Hex(DismProcess.ExitCode)))
        End If
    End Sub

End Module
