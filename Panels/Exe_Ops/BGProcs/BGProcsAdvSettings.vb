Imports System.Windows.Forms

Public Class BGProcsAdvSettings

    Public NeedsDriverChecks As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        MainForm.ExtAppxGetter = CheckBox1.Checked
        MainForm.SkipNonRemovable = CheckBox2.Checked
        If Not MainForm.AllDrivers = CheckBox3.Checked Then NeedsDriverChecks = True Else NeedsDriverChecks = False
        MainForm.AllDrivers = CheckBox3.Checked
        MainForm.SkipFrameworks = CheckBox4.Checked
        MainForm.RunAllProcs = CheckBox5.Checked
        If CheckBox5.Checked Then
            MainForm.bwBackgroundProcessAction = 0
            MainForm.bwGetImageInfo = True
            MainForm.bwGetAdvImgInfo = True
        End If
        If (NeedsDriverChecks And MainForm.isProjectLoaded And (MainForm.IsImageMounted Or MainForm.OnlineManagement)) And Not MainForm.ImgBW.IsBusy Then
            Dim msg As String = ""
            msg = LocalizationService.ForSection("BgProcesses.Validation")("DetectDrivers.Message")
            MsgBox(msg, vbOKOnly + vbInformation, Text)
            MainForm.bwGetImageInfo = False
            MainForm.bwGetAdvImgInfo = False
            MainForm.bwBackgroundProcessAction = 5
            MainForm.UpdateProjProperties(True, False)
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BGProcsAdvSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("BgProcs.Settings")("Advanced.Process.Label")
        Label1.Text = LocalizationService.ForSection("BgProcs.Settings")("Additional.Label")
        CheckBox1.Text = LocalizationService.ForSection("BgProcesses")("Enhance.App.Detect.Message")
        CheckBox2.Text = LocalizationService.ForSection("BgProcesses")("SkipNonRemovable.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("BgProcesses")("DetectAllDrivers.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("BgProcesses")("Skip.Framework.CheckBox")
        CheckBox5.Text = LocalizationService.ForSection("BgProcesses")("Run.CheckBox")
        OK_Button.Text = LocalizationService.ForSection("BgProcesses")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("BgProcesses")("Cancel.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        CheckBox1.Checked = MainForm.ExtAppxGetter
        CheckBox2.Checked = MainForm.SkipNonRemovable
        CheckBox3.Checked = MainForm.AllDrivers
        CheckBox4.Checked = MainForm.SkipFrameworks
        CheckBox5.Checked = MainForm.RunAllProcs
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        CheckBox2.Enabled = CheckBox1.Checked
        CheckBox4.Enabled = CheckBox1.Checked
    End Sub
End Class
