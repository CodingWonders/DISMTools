Imports System.Windows.Forms

Public Class FfuOptimize

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()

        If TextBox1.Text = "" OrElse Not File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("The source image file has not been specified or it does not exist in the file system.")
            MsgBox(LocalizationService.ForSection("FFU.Optimize.Messages")("Path.Image.Required.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If

        ProgressPanel.FFUOptimizationSource = TextBox1.Text
        If CheckBox1.Checked Then
            ProgressPanel.FFUOptimizationCustomPartitionNum = NumericUpDown1.Value
        Else
            ProgressPanel.FFUOptimizationCustomPartitionNum = 0
        End If

        ProgressPanel.OperationNum = 16
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub FfuOptimize_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Panel1.Enabled = CheckBox1.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub
End Class
