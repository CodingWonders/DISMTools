Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgCapture

    Dim CompressionTypeStrings() As String = New String(2) {"", "", ""}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()

        If TextBox1.Text = "" OrElse Not Directory.Exists(TextBox1.Text) Then
            MsgBox(LocalizationService.ForSection("ImageOps.Capture.Messages")("Provide.Source.Dir.Label"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If

        Dim sysprepTag As String = String.Format("{0}\Windows\system32\sysprep\sysprep_succeeded.tag", TextBox1.Text)
        If Not File.Exists(sysprepTag) Then
            If MsgBox(LocalizationService.ForSection("ImageOps.Capture.Messages").Format("SourcePrepWarning.Message", Environment.NewLine),
                                vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.No Then Exit Sub
        End If

        ProgressPanel.CaptureSourceDir = TextBox1.Text
        ProgressPanel.CaptureDestinationImage = TextBox2.Text
        ProgressPanel.CaptureName = TextBox3.Text
        ProgressPanel.CaptureDescription = TextBox4.Text
        If CheckBox1.Checked Then
            ProgressPanel.CaptureWimScriptConfig = TextBox5.Text
        Else
            ProgressPanel.CaptureWimScriptConfig = ""
        End If
        Select Case ComboBox1.SelectedIndex
            Case 0
                ProgressPanel.CaptureCompressType = 0
            Case 1
                ProgressPanel.CaptureCompressType = 1
            Case 2
                ProgressPanel.CaptureCompressType = 2
        End Select
        If CheckBox2.Checked Then
            ProgressPanel.CaptureBootable = True
        Else
            ProgressPanel.CaptureBootable = False
        End If
        If CheckBox3.Checked Then
            ProgressPanel.CaptureCheckIntegrity = True
        Else
            ProgressPanel.CaptureCheckIntegrity = False
        End If
        If CheckBox4.Checked Then
            ProgressPanel.CaptureVerify = True
        Else
            ProgressPanel.CaptureVerify = False
        End If
        If CheckBox5.Checked Then
            ProgressPanel.CaptureReparsePt = True
        Else
            ProgressPanel.CaptureReparsePt = False
        End If
        If CheckBox6.Checked Then
            ProgressPanel.CaptureUseWimBoot = True
        Else
            ProgressPanel.CaptureUseWimBoot = False
        End If
        If CheckBox7.Checked Then
            ProgressPanel.CaptureExtendedAttributes = True
        Else
            ProgressPanel.CaptureExtendedAttributes = False
        End If
        If CheckBox8.Checked Then
            DynaLog.LogMessage("The target image will be mounted after capture is complete.")
            ProgressPanel.CaptureMountDestImg = True
            ' Since ProgressPanel doesn't consider what other form variables contain, set them to ProgressPanel variables
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""
            ProgressPanel.MountDir = MainForm.MountDir
            ProgressPanel.UMountOp = 1
            ProgressPanel.CheckImgIntegrity = False
            ProgressPanel.SaveToNewIndex = False
            ProgressPanel.SourceImg = ProgressPanel.CaptureDestinationImage
            ProgressPanel.isOptimized = False
            ProgressPanel.isIntegrityTested = False
            ProgressPanel.TaskList.AddRange({6, 21, 15})
        Else
            ProgressPanel.CaptureMountDestImg = False
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 6
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgCapture_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ImgCapture")("CaptureImage.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("ImgCapture")("Destination.ImageFile.Label")
        Label3.Text = LocalizationService.ForSection("ImgCapture")("Source.Image.Dir.Label")
        Label4.Text = LocalizationService.ForSection("ImgCapture")("Dest.Image.Description.Label")
        Label5.Text = LocalizationService.ForSection("ImgCapture")("Destination.Image.Name.Label")
        Label6.Text = LocalizationService.ForSection("ImgCapture")("Path.Config.File.Label")
        Label7.Text = LocalizationService.ForSection("ImgCapture")("CompressionType.Label")
        GroupBox1.Text = LocalizationService.ForSection("ImgCapture")("Sources.Destinations.Group")
        GroupBox2.Text = LocalizationService.ForSection("ImgCapture")("Options.Group")
        Button1.Text = LocalizationService.ForSection("ImgCapture")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("ImgCapture")("Browse.Button")
        Button3.Text = LocalizationService.ForSection("ImgCapture")("Browse.Button")
        Button5.Text = LocalizationService.ForSection("ImgCapture")("Create.Button")
        OK_Button.Text = LocalizationService.ForSection("ImgCapture")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImgCapture")("Cancel.Button")
        CheckBox1.Text = LocalizationService.ForSection("ImgCapture")("Exclude.Files.Dirs.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("ImgCapture")("Image.Bootable.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("ImgCapture")("Verify.Image.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("ImgCapture")("Check.File.Errors.CheckBox")
        CheckBox5.Text = LocalizationService.ForSection("ImgCapture")("Reparse.Point.Tag.CheckBox")
        CheckBox6.Text = LocalizationService.ForSection("ImgCapture")("Append.WIM.Boot.CheckBox")
        CheckBox7.Text = LocalizationService.ForSection("ImgCapture")("Extended.Attributes.CheckBox")
        CheckBox8.Text = LocalizationService.ForSection("ImgCapture")("Mount.Dest.Image.CheckBox")
        CompressionTypeStrings(0) = LocalizationService.ForSection("ImgCapture")("No.Compression.None.Item")
        CompressionTypeStrings(1) = LocalizationService.ForSection("ImgCapture")("Fast.Compression.Item")
        CompressionTypeStrings(2) = LocalizationService.ForSection("ImgCapture")("MaxCompression.Message")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox3.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox5.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        TextBox5.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        If MainForm.OnlineManagement Or MainForm.OfflineManagement Then
            CheckBox8.Enabled = False
        Else
            CheckBox8.Enabled = True
        End If
        Try
            ' WIMBoot is only compatible with Windows 8.1
            DynaLog.LogMessage("Detecting if the Windows image that is being serviced supports WIMBoot...")
            If MainForm.CurrentImage.ImageVersion IsNot Nothing And MainForm.CurrentImage.ImageVersion.Build = 9600 Then
                ' We are dealing with Windows 8.1
                DynaLog.LogMessage("The image that is being serviced contains Windows 8.1. It supports WIMBoot.")
                CheckBox6.Enabled = True
            Else
                DynaLog.LogMessage("The image that is being serviced does not contain Windows 8.1. It does not support WIMBoot.")
                CheckBox6.Enabled = False
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not detect WIMBoot compatibility. Error Message: " & ex.Message)
            CheckBox6.Enabled = False
        End Try
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Selected source directory: " & Quote & FolderBrowserDialog1.SelectedPath & Quote)
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        DynaLog.LogMessage("Selected destination image file: " & Quote & SaveFileDialog1.FileName & Quote)
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label6.Enabled = True
            TextBox5.Enabled = True
            Button3.Enabled = True
            Button5.Enabled = True
        Else
            Label6.Enabled = False
            TextBox5.Enabled = False
            Button3.Enabled = False
            Button5.Enabled = False
        End If
        GatherValidFields()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        DynaLog.LogMessage("Selected configuration list file: " & Quote & OpenFileDialog1.FileName & Quote)
        TextBox5.Text = OpenFileDialog1.FileName
    End Sub

    Sub GatherValidFields()
        DynaLog.LogMessage("Checking fields...")
        If CheckBox1.Checked Then
            DynaLog.LogMessage("A configuration list file is expected to be used.")
            If Directory.Exists(TextBox1.Text) And TextBox2.Text IsNot "" And TextBox3.Text IsNot "" And TextBox5.Text IsNot "" Then
                DynaLog.LogMessage("All fields are valid.")
                OK_Button.Enabled = True
            Else
                DynaLog.LogMessage("None or not all fields are valid.")
                OK_Button.Enabled = False
            End If
        Else
            DynaLog.LogMessage("A configuration list file is not expected to be used.")
            If Directory.Exists(TextBox1.Text) And TextBox2.Text IsNot "" And TextBox3.Text IsNot "" Then
                DynaLog.LogMessage("All fields are valid.")
                OK_Button.Enabled = True
            Else
                DynaLog.LogMessage("None or not all fields are valid.")
                OK_Button.Enabled = False
            End If
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        GatherValidFields()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        GatherValidFields()
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        GatherValidFields()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 0 Then
            Label8.Text = CompressionTypeStrings(0)
        ElseIf ComboBox1.SelectedIndex = 1 Then
            Label8.Text = CompressionTypeStrings(1)
        ElseIf ComboBox1.SelectedIndex = 2 Then
            Label8.Text = CompressionTypeStrings(2)
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        GatherValidFields()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Visible = False
        ' Make it so that it can only close
        WimScriptEditor.MinimizeBox = False
        WimScriptEditor.MaximizeBox = False
        WimScriptEditor.ShowDialog(MainForm)
        If File.Exists(WimScriptEditor.ConfigListFile) Then
            TextBox5.Text = WimScriptEditor.ConfigListFile
        End If
        Visible = True
    End Sub
End Class
