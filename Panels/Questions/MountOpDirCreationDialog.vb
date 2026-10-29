Imports System.Windows.Forms

Public Class MountOpDirCreationDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.No
        Me.Close()
    End Sub

    Private Sub MountOpDirCreationDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = ImgMount.ImageTaskHeader1.ItemText
        Label1.Text = LocalizationService.ForSection("MountDirCreation")("Create.Label")
        OK_Button.Text = LocalizationService.ForSection("MountDirCreation")("Yes.Button")
        Cancel_Button.Text = LocalizationService.ForSection("MountDirCreation")("No.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Panel1.BackColor = CurrentTheme.SectionBackgroundColor
        Label1.ForeColor = Color.FromArgb(0, 122, 204)
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
    End Sub
End Class
