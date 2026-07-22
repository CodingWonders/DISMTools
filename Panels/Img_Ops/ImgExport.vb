Imports System.Windows.Forms
Imports Microsoft.Dism
Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgExport

    Dim CompressionTypeStrings() As String = New String(3) {"", "", "", ""}
    Dim originalFileFilters As String = "WIM files|*.wim|ESD files|*.esd"

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Checking source image...")
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("A source image has been specified and exists in the file system.")
            DynaLog.LogMessage("Extension of source image file: " & Path.GetExtension(TextBox1.Text))
            If Path.GetExtension(TextBox1.Text).EndsWith("swm", StringComparison.OrdinalIgnoreCase) Then
                DynaLog.LogMessage("A split WIM (SWM) file has been detected. Tweaking export source...")
                ProgressPanel.imgExportSourceImage = TextBox1.Text.Replace(Path.GetFileNameWithoutExtension(TextBox1.Text), _
                                                                           Path.GetFileNameWithoutExtension(TextBox1.Text) & "*")
            Else
                ProgressPanel.imgExportSourceImage = TextBox1.Text
            End If
        Else
            DynaLog.LogMessage("Either no source image has been specified or it does not exist in the file system.")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("ImgExport.Validation")("SourceImageFile.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If TextBox2.Text <> "" Then
            DynaLog.LogMessage("A destination image has been specified.")
            ProgressPanel.imgExportDestinationImage = TextBox2.Text
        Else
            DynaLog.LogMessage("A destination image has not been specified.")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("ImgExport.Validation")("ImageFile.Required.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.imgExportSourceIndex = NumericUpDown1.Value
        ProgressPanel.imgExportDestinationUseCustomName = CheckBox2.Checked
        If CheckBox2.Checked Then
            DynaLog.LogMessage("A custom name is expected to be used for the destination image. Checking name...")
            If TextBox3.Text <> "" Then
                ProgressPanel.imgExportDestinationName = TextBox3.Text
            Else
                DynaLog.LogMessage("No name has been specified. Falling back to using the default name of the source Windows image to export...")
                ProgressPanel.imgExportDestinationName = ""
                ProgressPanel.imgExportDestinationUseCustomName = False
            End If
        End If
        ProgressPanel.imgExportCompressType = ComboBox1.SelectedIndex
        ProgressPanel.imgExportMarkBootable = CheckBox3.Checked
        ProgressPanel.imgExportUseWimBoot = CheckBox4.Checked
        ProgressPanel.imgExportCheckIntegrity = CheckBox5.Checked
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 10
        Visible = False
        ProgressPanel.ShowDialog(Me)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        DynaLog.LogMessage("Export target: " & Quote & SaveFileDialog1.FileName & Quote)
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        DynaLog.LogMessage("Source image: " & Quote & OpenFileDialog1.FileName & Quote)
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub ImgExport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ImgExport")("ExportImage.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("ImgExport")("Destination.ImageFile.Label")
        Label3.Text = LocalizationService.ForSection("ImgExport")("SourceImageFile.Label")
        Label4.Text = LocalizationService.ForSection("ImgExport")("NamingPattern.Label")
        Label5.Text = LocalizationService.ForSection("ImgExport")("CompressionType.Label")
        Label7.Text = LocalizationService.ForSection("ImgExport")("Source.Image.Index.Label")
        CheckBox1.Text = LocalizationService.ForSection("ImgExport")("Reference.Swmfiles.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("ImgExport")("CustomName.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("ImgExport")("Image.Bootable.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("ImgExport")("Append.Image.WIM.CheckBox")
        CheckBox5.Text = LocalizationService.ForSection("ImgExport")("CheckIntegrity.CheckBox")
        OK_Button.Text = LocalizationService.ForSection("ImgExport")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImgExport")("Cancel.Button")
        Button1.Text = LocalizationService.ForSection("ImgExport")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("ImgExport")("Browse.Button")
        Button4.Text = LocalizationService.ForSection("ImgExport")("Name.Image.Button")
        Button5.Text = LocalizationService.ForSection("ImgExport")("ScanPattern.Button")
        GroupBox1.Text = LocalizationService.ForSection("ImgExport")("Sources.Destinations.Group")
        GroupBox2.Text = LocalizationService.ForSection("ImgExport")("Options.Group")
        OpenFileDialog1.Title = LocalizationService.ForSection("ImgExport")("Source.ImageFile.Title")
        ListView1.Columns(0).Text = LocalizationService.ForSection("ImgExport")("Index.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("ImgExport")("ImageName.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("ImgExport")("ImageDescription.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("ImgExport")("ImageVersion.Column")
        CompressionTypeStrings(0) = LocalizationService.ForSection("ImgExport")("No.Compression.None.Item")
        CompressionTypeStrings(1) = LocalizationService.ForSection("ImgExport")("Fast.Compression.Item")
        CompressionTypeStrings(2) = LocalizationService.ForSection("ImgExport")("MaxCompression.Message")
        CompressionTypeStrings(3) = LocalizationService.ForSection("ImgExport")("Compression.Level.Message")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox3.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        StatusStrip1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        ToolStripStatusLabel1.Text = LocalizationService.ForSection("ImgExport")("NamingPattern.Required.Label")
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Try
            ' WIMBoot is only compatible with Windows 8.1
            DynaLog.LogMessage("Detecting if the Windows image that is being serviced supports WIMBoot...")
            If MainForm.CurrentImage.ImageVersion IsNot Nothing And MainForm.CurrentImage.ImageVersion.Build = 9600 Then
                ' We are dealing with Windows 8.1
                DynaLog.LogMessage("The image that is being serviced contains Windows 8.1. It supports WIMBoot.")
                CheckBox4.Enabled = True
            Else
                DynaLog.LogMessage("The image that is being serviced does not contain Windows 8.1. It does not support WIMBoot.")
                CheckBox4.Enabled = False
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not detect WIMBoot compatibility. Error Message: " & ex.Message)
            CheckBox4.Enabled = False
        End Try

        ColumnHeader1.Width = WindowHelper.ScaleLogical(44)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(256)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(256)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(128)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("An image file has been specified and it exists in the file system. Getting information...")
            DynaLog.LogMessage("Checking if mounted image detector is busy...")
            MainForm.StopMountedImageDetector()
            Try
                ListView1.Items.Clear()
                DismApi.Initialize(DismLogLevel.LogErrors)
                Dim imgInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(TextBox1.Text)
                NumericUpDown1.Maximum = imgInfoCollection.Count
                For Each imgInfo As DismImageInfo In imgInfoCollection
                    ListView1.Items.Add(New ListViewItem(New String() {imgInfo.ImageIndex, imgInfo.ImageName, imgInfo.ImageDescription, imgInfo.ProductVersion.ToString()}))
                Next
            Catch ex As Exception
                DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
                MsgBox(LocalizationService.ForSection("ImageOps.Export.Messages")("Get.Index.Image.Label"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Finally
                Try
                    DynaLog.LogMessage("Shutting down API...")
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
            If File.Exists(TextBox1.Text) And Path.GetExtension(TextBox1.Text).EndsWith("swm", StringComparison.OrdinalIgnoreCase) Then
                CheckBox1.Checked = True
                Button4.PerformClick()
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        SWMFilePanel.Enabled = CheckBox1.Checked = True
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        TextBox3.Enabled = CheckBox2.Checked = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        SaveFileDialog1.Filter = originalFileFilters
        If ComboBox1.SelectedIndex = 0 Then
            Label8.Text = CompressionTypeStrings(0)
        ElseIf ComboBox1.SelectedIndex = 1 Then
            Label8.Text = CompressionTypeStrings(1)
        ElseIf ComboBox1.SelectedIndex = 2 Then
            Label8.Text = CompressionTypeStrings(2)
        ElseIf ComboBox1.SelectedIndex = 3 Then
            Label8.Text = CompressionTypeStrings(3)
            ' If recovery is specified, the target image must be an ESD file
            SaveFileDialog1.Filter = LocalizationService.ForSection("Panels.ImageOps.ExportImage")("Esdfiles.Filter")
            If TextBox2.Text <> "" Then
                ' Switch the extension of the target image file
                TextBox2.Text = TextBox2.Text.Replace(Path.GetExtension(TextBox2.Text), ".esd").Trim()
            End If
        End If
    End Sub

    Private Sub ImgExport_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SaveFileDialog1.Filter = originalFileFilters
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
            MsgBox(LocalizationService.ForSection("ImgExport.ScanSwmPattern")("Source.WIM.Required.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            ToolStripStatusLabel1.Text = LocalizationService.ForSection("ImgExport.ScanSwmPattern").Format("Naming.Returns.Item", ListBox1.Items.Count)
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
        ToolStripStatusLabel1.Text = LocalizationService.ForSection("ImgExport.ScanSwmPattern").Format("Naming.Returns.Label", ListBox1.Items.Count)
        If ListBox1.Items.Count <= 0 Then Beep()
    End Sub
End Class
