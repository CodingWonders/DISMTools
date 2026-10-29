Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgAppend

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim msg As String = ""
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Checking source image directory...")
        If TextBox1.Text = "" Or Not Directory.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("Either no source directory has been specified or it does not exist in the file system.")
            msg = LocalizationService.ForSection("ImgAppend.Validation")("SourceImage.Required.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        Else
            ProgressPanel.AppendixSourceDir = TextBox1.Text
        End If
        DynaLog.LogMessage("Checking destination image...")
        If TextBox2.Text = "" Or Not File.Exists(TextBox2.Text) Then
            DynaLog.LogMessage("Either no destination image has been specified or it does not exist in the file system.")
            msg = LocalizationService.ForSection("ImgAppend.Validation")("ImageFile.Required.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        Else
            ProgressPanel.AppendixDestinationImage = TextBox2.Text
        End If
        DynaLog.LogMessage("Checking name of image to append to destination...")
        If TextBox3.Text = "" Then
            DynaLog.LogMessage("No name has been specified.")
            msg = LocalizationService.ForSection("ImgAppend.Validation")("NameDestination.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        Else
            ProgressPanel.AppendixName = TextBox3.Text
        End If
        ProgressPanel.AppendixDescription = TextBox4.Text
        If CheckBox1.Checked Then
            DynaLog.LogMessage("A configuration list file is expected to be used. Checking specified file...")
            If TextBox5.Text <> "" And File.Exists(TextBox5.Text) Then
                DynaLog.LogMessage("A configuration list file has been specified and it exists in the file system.")
                ProgressPanel.AppendixWimScriptConfig = TextBox5.Text
            Else
                DynaLog.LogMessage("Either no configuration list file has been specified or it does not exist in the file system.")
                DynaLog.LogMessage("We can continue without it, but that may not be what the user wants. Asking...")
                msg = LocalizationService.ForSection("ImgAppend.Validation")("Either.Config.List.Message")
                If MsgBox(msg, vbYesNo + vbCritical, ImageTaskHeader1.ItemText) = MsgBoxResult.Ok Then
                    DynaLog.LogMessage("The user does not mind if we continue without the configuration list file.")
                    ProgressPanel.AppendixWimScriptConfig = ""
                Else
                    DynaLog.LogMessage("The user wants the configuration list file.")
                    Exit Sub
                End If
            End If
        Else
            DynaLog.LogMessage("A configuration list file is not expected to be used.")
            ProgressPanel.AppendixWimScriptConfig = ""
        End If
        ProgressPanel.AppendixUseWimBoot = CheckBox2.Checked
        ProgressPanel.AppendixBootable = CheckBox3.Checked
        ProgressPanel.AppendixCheckIntegrity = CheckBox4.Checked
        ProgressPanel.AppendixVerify = CheckBox5.Checked
        ProgressPanel.AppendixReparsePt = CheckBox6.Checked
        ProgressPanel.AppendixCaptureExtendedAttribs = CheckBox7.Checked
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 1
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgAppend_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ImgAppend")("AppendImage.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("ImgAppend").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("ImgAppend")("Path.Config.File.Label")
        Label3.Text = LocalizationService.ForSection("ImgAppend")("Source.Image.Dir.Label")
        Label5.Text = LocalizationService.ForSection("ImgAppend")("Dest.Image.Description.Label")
        Label6.Text = LocalizationService.ForSection("ImgAppend")("Destination.ImageFile.Label")
        Label7.Text = LocalizationService.ForSection("ImgAppend")("Destination.Image.Name.Label")
        OK_Button.Text = LocalizationService.ForSection("ImgAppend")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImgAppend")("Cancel.Button")
        Button1.Text = LocalizationService.ForSection("ImgAppend")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("ImgAppend")("Browse.Button")
        Button3.Text = LocalizationService.ForSection("ImgAppend")("Browse.Button")
        Button4.Text = LocalizationService.ForSection("ImgAppend")("Grab.Last.Image.Button")
        Button5.Text = LocalizationService.ForSection("ImgAppend")("Create.Button")
        CheckBox1.Text = LocalizationService.ForSection("ImgAppend")("Exclude.Files.Dirs.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("ImgAppend")("WIM.Boot.Config.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("ImgAppend")("Image.Bootable.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("ImgAppend")("Verify.Image.CheckBox")
        CheckBox5.Text = LocalizationService.ForSection("ImgAppend")("Check.File.Errors.CheckBox")
        CheckBox6.Text = LocalizationService.ForSection("ImgAppend")("Reparse.Point.Tag.CheckBox")
        CheckBox7.Text = LocalizationService.ForSection("ImgAppend")("ExtendedAttributes.CheckBox")
        GroupBox1.Text = LocalizationService.ForSection("ImgAppend")("Sources.Destinations.Group")
        GroupBox2.Text = LocalizationService.ForSection("ImgAppend")("Options.Group")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox3.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox5.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        TextBox5.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Try
            ' WIMBoot is only compatible with Windows 8.1
            DynaLog.LogMessage("Detecting if the Windows image that is being serviced supports WIMBoot...")
            If MainForm.CurrentImage.ImageVersion IsNot Nothing And MainForm.CurrentImage.ImageVersion.Build = 9600 Then
                ' We are dealing with Windows 8.1
                DynaLog.LogMessage("The image that is being serviced contains Windows 8.1. It supports WIMBoot.")
                CheckBox2.Enabled = True
            Else
                DynaLog.LogMessage("The image that is being serviced does not contain Windows 8.1. It does not support WIMBoot.")
                CheckBox2.Enabled = False
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not detect WIMBoot compatibility. Error Message: " & ex.Message)
            CheckBox2.Enabled = False
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
            Label2.Enabled = True
            TextBox5.Enabled = True
            Button3.Enabled = True
            Button5.Enabled = True
        Else
            Label2.Enabled = False
            TextBox5.Enabled = False
            Button3.Enabled = False
            Button5.Enabled = False
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox5.Text = OpenFileDialog1.FileName
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

    Function GetLastImageName() As String
        DynaLog.LogMessage("Image file to get information about: " & Quote & TextBox2.Text & Quote)
        Dim imageName As String = ""
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Dim ImageInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(TextBox2.Text)
            DynaLog.LogMessage("Information collection count: " & ImageInfoCollection.Count)
            DynaLog.LogMessage("Getting name of last index...")
            imageName = ImageInfoCollection.Last.ImageName
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            MsgBox(LocalizationService.ForSection("ImageOps.Append.Messages").Format("Grab.Last.Image.Label", ex.ToString()), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
        Finally
            Try
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As Exception
                ' Don't do anything
            End Try
        End Try
        DynaLog.LogMessage("Name of last image: " & imageName)
        Return imageName
    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        DynaLog.LogMessage("Checking if the destination file has been specified and exists...")
        If TextBox2.Text = "" OrElse Not File.Exists(TextBox2.Text) Then Exit Sub
        MainForm.StopMountedImageDetector()
        TextBox3.Text = GetLastImageName()
        MainForm.StartMountedImageDetector()
    End Sub

    Private Sub Button4_MouseHover(sender As Object, e As EventArgs) Handles Button4.MouseHover
        Dim msg As String = ""
        msg = LocalizationService.ForSection("ImgAppend.Tooltip")("Grab.Name.Last.Message")
        WindowHelper.DisplayToolTip(sender, msg)
    End Sub
End Class
