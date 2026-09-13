Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class SettingsResetDlg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SettingsResetDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Text = LocalizationService.ForSection("SettingsReset")("ResetPreferences.Label")
        Label1.Text = LocalizationService.ForSection("SettingsReset")("ProceedReset.Message")
        OK_Button.Text = LocalizationService.ForSection("SettingsReset")("Yes.Button")
        Cancel_Button.Text = LocalizationService.ForSection("SettingsReset")("No.Button")
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Beep()
    End Sub
End Class
