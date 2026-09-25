Imports System.Windows.Forms

Public Class SaveProjectQuestionDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Yes_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub No_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles No_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.No
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SaveProjectQuestionDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = LocalizationService.ForSection("SaveProject")("SaveChanges.Label")
        Label2.Text = LocalizationService.ForSection("SaveProject")("ShutdownRestart.Message")
        Yes_Button.Text = LocalizationService.ForSection("SaveProject")("Yes.Button")
        No_Button.Text = LocalizationService.ForSection("SaveProject")("No.Button")
        Cancel_Button.Text = LocalizationService.ForSection("SaveProject")("Cancel.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Panel1.BackColor = CurrentTheme.SectionBackgroundColor
        Label1.ForeColor = Color.FromArgb(0, 122, 204)
        Label2.Visible = MainForm.IsImageMounted
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
    End Sub
End Class
