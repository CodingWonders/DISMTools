Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class ActiveInstAccessWarn

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ActiveInstAccessWarn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ActiveInstallWarn")("Active.Install.Label")
        Label1.Text = LocalizationService.ForSection("ActiveInstall")("Enter.Online.Message")
        Label2.Text = LocalizationService.ForSection("ActiveInstall")("ProjectUnloaded.Label")
        OK_Button.Text = LocalizationService.ForSection("ActiveInstall")("Continue.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ActiveInstall")("Cancel.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Beep()
    End Sub
End Class
