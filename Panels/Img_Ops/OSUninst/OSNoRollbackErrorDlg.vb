Imports System.Windows.Forms

Public Class OSNoRollbackErrorDlg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OSNoRollbackErrorDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = LocalizationService.ForSection("OS.No")("Troll.Back.Older.Label")
        Label2.Text = LocalizationService.ForSection("OS.No")("Old.Versions.None.Message")
        OK_Button.Text = LocalizationService.ForSection("OS.No")("Ok.Button")
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
