Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32
Imports System.IO

Public Class SetPETargetPath
    Implements IImageTaskDialog

    Sub GetTargetPath()
        DynaLog.LogMessage("Preparing to get Windows PE settings...")
        DynaLog.LogMessage("Loading SOFTWARE hive of WinPE image...")
        Dim regExitCode As Integer = RegistryHelper.LoadRegistryHive(Path.Combine(MainForm.MountDir, "Windows", "system32", "config", "SOFTWARE"), "HKLM\PE_SOFT")
        DynaLog.LogMessage("REG hive exit code: " & Hex(regExitCode))
        Try
            DynaLog.LogMessage("Getting target path...")
            Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("PE_SOFT\Microsoft\Windows NT\CurrentVersion\WinPE", False)
            DynaLog.LogMessage("Target path: " & Quote & regKey.GetValue("InstRoot", "") & Quote)
            TextBox1.Text = regKey.GetValue("InstRoot", "").ToString()
            regKey.Close()
        Catch ex As Exception

        End Try
        DynaLog.LogMessage("Unloading hives...")
        ' Unload registry hives
        RegistryHelper.UnloadRegistryHive("HKLM\PE_SOFT")
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ' Detect the following requirements before proceeding:
        ' - Right length (3-32 chars)
        ' - Starts with any drive letter other than A or B
        ' - Is absolute
        ' - Drive letter is followed by :
        ' - Doesn't contain spaces or quotation marks
        Dim msg As String = ""
        DynaLog.LogMessage("Checking specified target path...")
        If TextBox1.TextLength < 3 Or TextBox1.TextLength > 32 Then
            DynaLog.LogMessage("Target path is not long enough or is too long.")
            msg = LocalizationService.ForSection("PETargetPath.Validation")("Target.Least.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If TextBox1.Text.StartsWith("A", StringComparison.OrdinalIgnoreCase) Or TextBox1.Text.StartsWith("B", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("Target path points to a drive commonly used by floppy disks.")
            msg = LocalizationService.ForSection("PETargetPath.Validation")("Target.Start.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If Not TextBox1.Text.Chars(1).Equals(":"c) Then
            DynaLog.LogMessage("The drive letter is ill-formed.")
            msg = LocalizationService.ForSection("PETargetPath.Validation")("DriveLetterFormat.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If TextBox1.Text.Contains(".\") Or TextBox1.Text.Contains("..\") Then
            DynaLog.LogMessage("A relative path is being used.")
            msg = LocalizationService.ForSection("PETargetPath.Validation")("AbsolutePath.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If TextBox1.Text.Contains(" ") Or TextBox1.Text.Contains(Quote) Then
            DynaLog.LogMessage("The target path contains spaces or quotes.")
            msg = LocalizationService.ForSection("PETargetPath.Validation")("Target.Contain.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.peNewTargetPath = TextBox1.Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 84
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Opening target path configuration dialog...")
        If MainForm.ImgBW.IsBusy Then
            DynaLog.LogMessage("Background processes are still busy.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        Return True
    End Function

    Private Sub SetTargetPath_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("PETargetPath.Target")("Set.Windows.Petarget.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("PETargetPath.Target").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("PETargetPath.Target")("Target.Dir.Message")
        Label3.Text = LocalizationService.ForSection("PETargetPath.Target")("TargetPath.Label")
        OK_Button.Text = LocalizationService.ForSection("PETargetPath.Target")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("PETargetPath.Target")("Cancel.Button")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor

        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        GetTargetPath()
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub
End Class
