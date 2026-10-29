Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Dism
Imports System.Threading

Public Class ImgApply

    Dim ImageVersions As New List(Of Version)
    Dim ImageEditions As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If TextBox1.Text = "" Or Not File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("Either no image file has been specified or it does not exist in the file system.")
            MsgBox(LocalizationService.ForSection("ImgApply.Validation")("ImageFile.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.ApplicationSourceImg = TextBox1.Text
        ProgressPanel.ApplicationIndex = ComboBox1.SelectedIndex + 1
        ProgressPanel.ApplicationDestDir = TextBox2.Text
        If CheckBox1.Checked Then
            ProgressPanel.ApplicationCheckInt = True
        Else
            ProgressPanel.ApplicationCheckInt = False
        End If
        If CheckBox2.Checked Then
            ProgressPanel.ApplicationVerify = True
        Else
            ProgressPanel.ApplicationVerify = False
        End If
        If CheckBox3.Checked Then
            ProgressPanel.ApplicationReparsePt = True
        Else
            ProgressPanel.ApplicationReparsePt = False
        End If
        If CheckBox4.Checked Then
            ProgressPanel.ApplicationSWMPattern = Path.GetDirectoryName(TextBox1.Text) & "\" & TextBox4.Text & "*.swm"
        Else
            ProgressPanel.ApplicationSWMPattern = ""
        End If
        If CheckBox5.Checked Then
            ProgressPanel.ApplicationValidateForTD = True
        Else
            ProgressPanel.ApplicationValidateForTD = False
        End If
        If CheckBox6.Checked Then
            ProgressPanel.ApplicationUseWimBoot = True
        Else
            ProgressPanel.ApplicationUseWimBoot = False
        End If
        If CheckBox7.Checked Then
            ProgressPanel.ApplicationCompactMode = True
        Else
            ProgressPanel.ApplicationCompactMode = False
        End If
        If CheckBox8.Checked Then
            ProgressPanel.ApplicationUseExtAttr = True
        Else
            ProgressPanel.ApplicationUseExtAttr = False
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 3
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgApply_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ImgApply")("ApplyImage.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("ImgApply").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("ImgApply")("SourceImageFile.Label")
        Label3.Text = LocalizationService.ForSection("ImgApply")("ImageIndex.Label")
        Label4.Text = LocalizationService.ForSection("ImgApply")("NamingPattern.Label")
        CheckBox1.Text = LocalizationService.ForSection("ImgApply")("Integrity.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("ImgApply")("Verify.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("ImgApply")("Reparse.Point.Tag.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("ImgApply")("Reference.Swmfiles.CheckBox")
        CheckBox5.Text = LocalizationService.ForSection("ImgApply")("Validate.Image.CheckBox")
        CheckBox6.Text = LocalizationService.ForSection("ImgApply")("Append.Image.WIM.CheckBox")
        CheckBox7.Text = LocalizationService.ForSection("ImgApply")("Image.Compact.Mode.CheckBox")
        CheckBox8.Text = LocalizationService.ForSection("ImgApply")("Extended.Attributes.CheckBox")
        Button1.Text = LocalizationService.ForSection("ImgApply")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("ImgApply")("Browse.Button")
        Button4.Text = LocalizationService.ForSection("ImgApply")("Name.Image.Button")
        Button5.Text = LocalizationService.ForSection("ImgApply")("ScanPattern.Button")
        UseMountedImgBtn.Text = LocalizationService.ForSection("ImgApply")("Mounted.Image.Label")
        OK_Button.Text = LocalizationService.ForSection("ImgApply")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImgApply")("Cancel.Button")
        Label5.Text = LocalizationService.ForSection("ImgApply")("Destination.Dir.Label")
        GroupBox1.Text = LocalizationService.ForSection("ImgApply")("Source.Group")
        GroupBox2.Text = LocalizationService.ForSection("ImgApply")("Options.Group")
        GroupBox3.Text = LocalizationService.ForSection("ImgApply")("Destination.Group")
        GroupBox4.Text = LocalizationService.ForSection("ImgApply")("SwmfilePattern.Group")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        GroupBox4.ForeColor = CurrentTheme.ForegroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        StatusStrip1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        ToolStripStatusLabel1.Text = LocalizationService.ForSection("ImgApply")("NamingPattern.Required.Label")
        If MainForm.SourceImg = "N/A" Or Not File.Exists(MainForm.SourceImg) Or MainForm.OnlineManagement Or MainForm.OfflineManagement Then
            UseMountedImgBtn.Enabled = False
        Else
            UseMountedImgBtn.Enabled = True
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then GetIndexes(TextBox1.Text) Else Exit Sub
        If TextBox1.Text.EndsWith(".swm") Then
            CheckBox4.Checked = True
            Button4.PerformClick()
        End If
    End Sub

    Sub GetIndexes(ImgFile As String)
        DynaLog.LogMessage("Mounted image detector might be busy. Stopping it if it is...")
        MainForm.MountedImageDetectorBWRestarterTimer.Enabled = False
        MainForm.StopMountedImageDetector()
        Dim imgInfo As DismImageInfoCollection = Nothing
        ComboBox1.Items.Clear()
        ImageVersions.Clear()
        ImageEditions.Clear()
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            imgInfo = DismApi.GetImageInfo(TextBox1.Text)
            DynaLog.LogMessage("Information collection count: " & imgInfo.Count)
            If imgInfo.Count > 0 Then
                DynaLog.LogMessage("Getting indexes and names...")
                For Each imageInfo In imgInfo
                    ComboBox1.Items.Add(imageInfo.ImageIndex & " (" & imageInfo.ImageName & ")")
                    ImageVersions.Add(imageInfo.ProductVersion)
                    ImageEditions.Add(imageInfo.EditionId)
                Next
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            Dim msg As String = ""
            msg = LocalizationService.ForSection("ImgApply.GetIndexes").Format("Gather.ImageFile.Message", ex.ToString(), ex.Message, Hex(ex.HResult))
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
        Finally
            Try
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As DismException
                ' Don't do anything
            End Try
        End Try
        MainForm.StartMountedImageDetector()
        If ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FolderBrowserDialog1.ShowDialog(Me)
        If DialogResult.OK Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox2.Text = ""
        End If
    End Sub

    Private Sub UseMountedImgBtn_Click(sender As Object, e As EventArgs) Handles UseMountedImgBtn.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox4.Text = Path.GetFileNameWithoutExtension(TextBox1.Text)
        ScanSwmPattern(TextBox4.Text)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ScanSwmPattern(TextBox4.Text)
    End Sub

    Sub ScanSwmPattern(PatternName As String)
        DynaLog.LogMessage("Preparing to scan files with the specified pattern...")
        DynaLog.LogMessage("- Scan pattern: " & PatternName)
        ListBox1.Items.Clear()
        If TextBox1.Text = "" Or PatternName = "" Then
            DynaLog.LogMessage("Either no source image file has been specified or no pattern has been specified.")
            MsgBox(LocalizationService.ForSection("ImgApply.ScanSwmPattern")("Source.WIM.Required.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("ImgApply.ScanSwmPattern")("ApplyImage.Message"))
            ToolStripStatusLabel1.Text = LocalizationService.ForSection("ImgApply.ScanSwmPattern").Format("Naming.Returns.Item", ListBox1.Items.Count)
            Beep()
            Exit Sub
        End If
        DynaLog.LogMessage("Scanning SWM files with given pattern...")
        For Each swmFile In My.Computer.FileSystem.GetFiles(Path.GetDirectoryName(TextBox1.Text), FileIO.SearchOption.SearchTopLevelOnly, "*.swm")
            If Path.GetFileNameWithoutExtension(swmFile).StartsWith(PatternName) Then
                ListBox1.Items.Add(Path.GetFileName(swmFile))
            End If
        Next
        DynaLog.LogMessage("Pattern search results: " & ListBox1.Items.Count)
        ToolStripStatusLabel1.Text = LocalizationService.ForSection("ImgApply.ScanSwmPattern").Format("Naming.Returns.Label", ListBox1.Items.Count)
        If ListBox1.Items.Count <= 0 Then Beep()
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        SWMFilePanel.Enabled = CheckBox4.Checked = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            If (ImageVersions.Count > 0) AndAlso (ImageEditions.Count > 0) Then
                DynaLog.LogMessage("Comparing Edition ID and version of selected image...")
                ' Windows PE 4.0 (based on Windows 8 - NT 6.2.9200)
                If ImageEditions(ComboBox1.SelectedIndex).Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) AndAlso ImageVersions(ComboBox1.SelectedIndex) >= New Version(6, 2, 9200, 0) Then
                    DynaLog.LogMessage("This is a Windows PE 4+ image. Trusted Desktop validation can be carried out.")
                    CheckBox5.Enabled = True
                Else
                    DynaLog.LogMessage("This is not a Windows PE 4+ image. Trusted Desktop validation cannot be carried out.")
                    CheckBox5.Enabled = False
                End If
                If ImageVersions(ComboBox1.SelectedIndex).Build = 9600 Then
                    DynaLog.LogMessage("The image that is being serviced contains Windows 8.1. It supports WIMBoot.")
                    CheckBox6.Enabled = True
                Else
                    DynaLog.LogMessage("The image that is being serviced does not contain Windows 8.1. It does not support WIMBoot.")
                    CheckBox6.Enabled = False
                End If
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            CheckBox5.Enabled = False
            CheckBox6.Enabled = False
        End Try
    End Sub
End Class
