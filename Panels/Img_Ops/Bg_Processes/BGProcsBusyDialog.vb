Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class BGProcsBusyDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub BGProcsBusyDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = LocalizationService.ForSection("BG.Procs")("Re.Still.Gathering.Label")
        Label2.Text = LocalizationService.ForSection("BG.Procs")("Finish.Process.Begin.Message")
        OK_Button.Text = LocalizationService.ForSection("BG.Procs")("Ok.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Panel1.BackColor = CurrentTheme.SectionBackgroundColor
        Label1.ForeColor = Color.FromArgb(0, 122, 204)
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Beep()
    End Sub
End Class
