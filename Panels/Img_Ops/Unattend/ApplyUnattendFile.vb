Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class ApplyUnattendFile

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Checking answer file...")
        If TextBox1.Text <> "" AndAlso File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("An answer file has been specified and it exists in the file system.")
            ProgressPanel.UnattendedFile = TextBox1.Text
        Else
            DynaLog.LogMessage("Either no unattended answer file has been specified or it does not exist in the file system.")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("ApplyUnattend.Validation")("AnswerFile.Choose.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.OperationNum = 79
        ProgressPanel.UnattendedCopyToSysprep = CheckBox1.Checked
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        DynaLog.LogMessage("Specified unattended answer file: " & Quote & OpenFileDialog1.FileName & Quote)
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub ApplyUnattendFile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ApplyUnattend")("UnattendAnswer.File.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("ApplyUnattend").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("ApplyUnattend")("AnswerFile.Label")
        Button1.Text = LocalizationService.ForSection("ApplyUnattend")("Browse.Button")
        OK_Button.Text = LocalizationService.ForSection("ApplyUnattend")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ApplyUnattend")("Cancel.Button")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub
End Class
