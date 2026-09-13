Imports System.Windows.Forms

Public Class ImgOptimize

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()

        If TextBox1.Text = "" OrElse Not Directory.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("The source mount directory has not been specified or it does not exist in the file system.")
            MsgBox(LocalizationService.ForSection("ImageOps.Optimize.Messages")("Mount.Dir.Required.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If

        ProgressPanel.OptimizationSource = TextBox1.Text
        ProgressPanel.OptimizationMode = If(RadioButton1.Checked, 0, 1)

        ProgressPanel.OperationNum = 17
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
        Dim selectedImage As WindowsImage = PopupMountedImagePicker.PickImage()
        If selectedImage IsNot Nothing Then
            DynaLog.LogMessage("Selected image: " & selectedImage.ImageFile)
            TextBox1.Text = selectedImage.ImageMountDirectory
        End If
    End Sub

    Private Sub ImgOptimize_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        GroupBox1.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Text = MainForm.MountDir
    End Sub
End Class
