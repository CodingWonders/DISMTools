Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class IncompleteSetupDlg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub IncompleteSetupDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = LocalizationService.ForSection("IncompleteSetup")("SetupIncomplete.Message")
        OK_Button.Text = LocalizationService.ForSection("IncompleteSetup")("Yes.Button")
        Cancel_Button.Text = LocalizationService.ForSection("IncompleteSetup")("No.Button")
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Beep()
    End Sub
End Class
