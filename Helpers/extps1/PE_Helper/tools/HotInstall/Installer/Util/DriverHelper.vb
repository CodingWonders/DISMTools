Imports System.IO
Imports System.Diagnostics
Imports Microsoft.VisualBasic.ControlChars

Module DriverHelper

    Sub ExportOnlineDrivers(Destination As String)
        DynaLog.LogMessage("Preparing to export third-party drivers...")
        DynaLog.LogMessage("- Driver Export Destination: " & Destination)
        If Not Directory.Exists(Destination) Then
            DynaLog.LogMessage("Destination Directory does not exist. Creating it...")
            Directory.CreateDirectory(Destination)
        End If
        DynaLog.LogMessage("Exporting drivers...")
        Dim DismProcess As New Process()
        DismProcess.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "dism.exe")
        DismProcess.StartInfo.Arguments = "/online /export-driver /destination=" & Quote & Destination & Quote
        DismProcess.StartInfo.CreateNoWindow = True
        DismProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        DismProcess.Start()
        DismProcess.WaitForExit()
        DynaLog.LogMessage("Process exited with code " & Hex(DismProcess.ExitCode))
        If DismProcess.ExitCode <> 0 Then
            Throw New Exception(String.Format(GetValueFromLanguageData("MainForm.DriverExporter_FailureMessage"), Hex(DismProcess.ExitCode)))
        End If
    End Sub

End Module
