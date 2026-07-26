Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars

Public Class NewTestingEnv

    Dim progressMessages() As String = New String(2) {"", "", ""}
    Dim success As Boolean
    Dim architectures() As String = New String(2) {"x86", "amd64", "arm64"}

    Private Sub NewTestingEnv_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        progressMessages(0) = LocalizationService.ForSection("NewTestingEnv")("Status.Message")
        progressMessages(1) = LocalizationService.ForSection("NewTestingEnv")("Creating.Project.Message")
        progressMessages(2) = LocalizationService.ForSection("NewTestingEnv")("ProjectCreated.Message")
        Text = LocalizationService.ForSection("NewTestingEnv")("Create.Environment.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("NewTestingEnv")("WizardHelp.Message") & LocalizationService.ForSection("NewTestingEnv")("ProjectTemplate.Message")
        Label3.Text = LocalizationService.ForSection("NewTestingEnv")("Re.Ready.Create.Label")
        Label5.Text = LocalizationService.ForSection("NewTestingEnv")("Env.Architecture.Label")
        Label6.Text = LocalizationService.ForSection("NewTestingEnv")("Architecture.Label")
        Label7.Text = LocalizationService.ForSection("NewTestingEnv")("Target.Project.Label")
        Label8.Text = progressMessages(0)
        Label9.Text = LocalizationService.ForSection("NewTestingEnv")("Other.Things.Project.Message")
        Button3.Text = LocalizationService.ForSection("NewTestingEnv")("Browse.Button")
        OK_Button.Text = LocalizationService.ForSection("NewTestingEnv")("Create.Button")
        Cancel_Button.Text = LocalizationService.ForSection("NewTestingEnv")("Cancel.Button")
        GroupBox2.Text = LocalizationService.ForSection("NewTestingEnv")("Progress.Group")
        LinkLabel1.Text = LocalizationService.ForSection("NewTestingEnv")("Download.Windows.ADK.Link")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox3.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox3.ForeColor = ForeColor
        GroupBox2.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Declare path constant for Windows ADK
        Dim ADKPath As String = Path.Combine(If(Environment.Is64BitOperatingSystem,
                                                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)), "Windows Kits", "10",
                                                "Assessment and Deployment Kit")
        ' Check ADK status
        If Not Directory.Exists(ADKPath) Then
            DynaLog.LogMessage("ADK installation directory " & Quote & ADKPath & Quote & " is not found in this system. Either it has not been installed or it has been installed somewhere else.")
            Process.Start(LocalizationService.ForSection("NewTestingEnv")("Https.Learn.Message"))
            Close()
        End If

        ' Restore combobox architecture items
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(architectures)
        ' Remove architectures incompatible with the system ADK
        For Each architecture In architectures
            Dim WimPath As String = Path.Combine(ADKPath, "Windows Preinstallation Environment", architecture, "en-us", "winpe.wim")
            DynaLog.LogMessage("Testing if architecture " & architecture & " is supported by the ADK installed in this system...")
            If Not File.Exists(WimPath) Then
                DynaLog.LogMessage("- Windows PE WIM " & Quote & WimPath & Quote & " is not present. Removing architecture option...")
                ComboBox1.Items.Remove(architecture)
            End If
        Next
        ' If we are left with no architectures, add them back
        If ComboBox1.Items.Count = 0 Then
            DynaLog.LogMessage("For some reason we excluded all of them. This could be because of incorrect detections. Adding back...")
            ComboBox1.Items.AddRange(architectures)
        End If
        ComboBox1.SelectedIndex = 0
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Target location of testing environment files: " & Quote & FolderBrowserDialog1.SelectedPath & Quote)
            TextBox3.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start(LocalizationService.ForSection("NewTestingEnv.Links")("Https.Learn.Message"))
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
        OptionsPanel.Enabled = False
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        BackgroundWorker1.ReportProgress(0)
        DynaLog.LogMessage("Starting PE Helper...")
        DynaLog.LogMessage("- Task: generate testing environment")
        DynaLog.LogMessage("- Architecture: " & ComboBox1.SelectedItem)
        DynaLog.LogMessage("- Destination folder for testing environment: " & Quote & TextBox3.Text & Quote)
        Dim ISOCreator As New Process()
        ISOCreator.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        ISOCreator.StartInfo.WorkingDirectory = Application.StartupPath & "\bin\extps1\PE_Helper"
        ISOCreator.StartInfo.Arguments = "-noprofile -nologo -executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\extps1\PE_Helper\PE_Helper.ps1" & Quote & " -cmd StartDevelopment -testArch " & ComboBox1.SelectedItem & " -targetPath " & Quote & TextBox3.Text & Quote
        ISOCreator.Start()
        ISOCreator.WaitForExit()
        DynaLog.LogMessage("The PE Helper process finished with exit code " & Hex(ISOCreator.ExitCode))
        success = (ISOCreator.ExitCode = 0)
        BackgroundWorker1.ReportProgress(100)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        IdlePanel.Visible = False
        ISOProgressPanel.Visible = True
        If e.ProgressPercentage < 100 Then
            WindowHelper.DisableCloseCapability(Handle)
            Label8.Text = progressMessages(1)
            ProgressBar1.Style = ProgressBarStyle.Marquee
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.Indeterminate, MainForm.Handle)
        Else
            WindowHelper.EnableCloseCapability(Handle)
            If success Then Label8.Text = progressMessages(2)
            ProgressBar1.Style = ProgressBarStyle.Blocks
            TaskbarHelper.SetIndicatorState(100, Windows.Shell.TaskbarItemProgressState.None, MainForm.Handle)
        End If
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        DynaLog.LogMessage("The PE Helper has finished.")
        DynaLog.LogMessage("- Did it succeed? " & If(success, "Yes", "No"))
        Dim msg As String = ""
        msg = If(success, LocalizationService.ForSection("NewTestingEnv.Background")("Project.Created.Done.Message"), LocalizationService.ForSection("NewTestingEnv.Background")("Failed.Create.Message"))
        MsgBox(msg, vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
        OptionsPanel.Enabled = True
        IdlePanel.Visible = True
        ISOProgressPanel.Visible = False
    End Sub

    Private Sub NewTestingEnv_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If BackgroundWorker1.IsBusy Then
            e.Cancel = True
            Beep()
        End If
    End Sub
End Class
