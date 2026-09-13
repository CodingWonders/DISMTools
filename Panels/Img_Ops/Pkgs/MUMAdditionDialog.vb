Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class MUMAdditionDialog

    Public MUMFile As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub MUMAdditionDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("MUMAdditionDialog")("Add.Update.Manifest.Label")
        Label1.Text = LocalizationService.ForSection("MUMAdditionDialog")("DialogHelp.Message") & LocalizationService.ForSection("MUMAdditionDialog")("Note.Advanced.Only.Message")
        Label2.Text = LocalizationService.ForSection("MUMAdditionDialog")("Path.Manifest.File.Label")
        Button1.Text = LocalizationService.ForSection("MUMAdditionDialog")("Browse.Button")
        Cancel_Button.Text = LocalizationService.ForSection("MUMAdditionDialog")("Cancel.Button")
        OK_Button.Text = LocalizationService.ForSection("MUMAdditionDialog")("Ok.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        DynaLog.LogMessage("Selected Microsoft Update Manifest file: " & Quote & OpenFileDialog1.FileName & Quote)
        MUMFile = OpenFileDialog1.FileName
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub
End Class
