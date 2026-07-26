Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports System.Threading

Public Class SingleImageIndexError

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SingleImageIndexError_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = LocalizationService.ForSection("Single.Image")("Image.Seems.Only.Item")
        Label2.Text = LocalizationService.ForSection("Single.Image")("Cannot.Switch.Index.Message")
        LinkLabel1.Text = LocalizationService.ForSection("Single.Image")("Know.Indexes.Message")
        LinkLabel1.LinkArea = LocalizationService.GetLinkArea(LinkLabel1.Text, LocalizationService.ForSection("Single.Image")("Here.LinkText"))
        OK_Button.Text = LocalizationService.ForSection("Single.Image")("Ok.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Panel1.BackColor = CurrentTheme.SectionBackgroundColor
        Panel2.BackColor = CurrentTheme.SectionBackgroundColor
        Label1.ForeColor = Color.FromArgb(0, 122, 204)
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Beep()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Visible = False
        GetImgInfoDlg.RadioButton1.Checked = True
        GetImgInfoDlg.RadioButton2.Checked = False
        GetImgInfoDlg.ShowDialog(MainForm)
    End Sub
End Class
