Imports System.Windows.Forms
Imports System.IO

Public Class FfuSplit

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("The source FFU file has been specified and exists in the file system.")
            ProgressPanel.SFUSplitSourceFile = TextBox1.Text
            ProgressPanel.SFUSplitFileSize = NumericUpDown1.Value
            If TextBox2.Text <> "" And Directory.Exists(Path.GetDirectoryName(TextBox2.Text)) Then
                DynaLog.LogMessage("A target file has been specified and its directory exists in the file system.")
                ProgressPanel.SFUSplitTargetFile = TextBox2.Text
            Else
                DynaLog.LogMessage("Either no target file has been specified or its directory does not exist in the file system.")
                MsgBox(LocalizationService.ForSection("FfuSplit.Validation")("Name.Required.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Exit Sub
            End If
            ProgressPanel.SFUSplitCheckIntegrity = CheckBox1.Checked
        Else
            DynaLog.LogMessage("Either no source FFU file has been specified or it does not exist in the file system.")
            MsgBox(LocalizationService.ForSection("FfuSplit.Validation")("Source.File.Required.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.OperationNum = 19
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub FfuSplit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("FfuSplit")("SplitFfuimages.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("FfuSplit").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("FfuSplit")("Source.Image.Label")
        Label3.Text = LocalizationService.ForSection("FfuSplit")("Name.Path.Destination.Label")
        Label4.Text = LocalizationService.ForSection("FfuSplit")("Maximum.Size.Images.Label")
        Label5.Text = LocalizationService.ForSection("FfuSplit")("LargeFile.Note.Message")
        Button1.Text = LocalizationService.ForSection("FfuSplit")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("FfuSplit")("Browse.Button")
        OK_Button.Text = LocalizationService.ForSection("FfuSplit")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("FfuSplit")("Cancel.Button")
        CheckBox1.Text = LocalizationService.ForSection("FfuSplit")("Integrity.CheckBox")
        OpenFileDialog1.Title = LocalizationService.ForSection("FfuSplit")("Source.File.Title")
        SaveFileDialog1.Title = LocalizationService.ForSection("FfuSplit")("Target.Location.Title")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(TextBox1.Text) & "_"
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        DynaLog.LogMessage("Source FFU file specified: " & Quote & OpenFileDialog1.FileName & Quote)
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        DynaLog.LogMessage("Target SFU file specified: " & Quote & SaveFileDialog1.FileName & Quote)
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub
End Class
