Imports System.Windows.Forms
Imports System.IO
Imports System.Management
Imports DISMTools.Utilities

Public Class FfuApply

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If TextBox1.Text = "" Or Not File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("Either no image file has been specified or it does not exist in the file system.")
            MsgBox(LocalizationService.ForSection("FfuApply.Validation")("ImageFile.Message"), vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        ProgressPanel.FFUApplicationSourceImg = TextBox1.Text
        ProgressPanel.FFUApplicationDestDrive = TextBox2.Text
        If CheckBox4.Checked Then
            ProgressPanel.FFUApplicationSFUPattern = Path.GetDirectoryName(TextBox1.Text) & "\" & TextBox4.Text & "*.sfu"
        Else
            ProgressPanel.FFUApplicationSFUPattern = ""
        End If

        ' TODO until we find a way to grab the manifest of the unmounted FFU, simply ask.
        MsgBox(LocalizationService.ForSection("FFU.Apply.Messages")("Destination.Disk.Message"), vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 2
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
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub UseMountedImgBtn_Click(sender As Object, e As EventArgs) Handles UseMountedImgBtn.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ApplicationDriveSpecifier.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            TextBox2.Text = ApplicationDriveSpecifier.SelectedDriveId
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ScanSFUPattern(TextBox4.Text)
    End Sub

    Sub ScanSFUPattern(PatternName As String)
        DynaLog.LogMessage("Preparing to scan files with the specified pattern...")
        DynaLog.LogMessage("- Scan pattern: " & PatternName)
        ListBox1.Items.Clear()
        If TextBox1.Text = "" Or PatternName = "" Then
            DynaLog.LogMessage("Either no source image file has been specified or no pattern has been specified.")
            MsgBox(LocalizationService.ForSection("FfuApply.ScanSFUPattern")("Source.File.Required.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("FfuApply.ScanSFUPattern")("ApplyImage.Message"))
            ToolStripStatusLabel1.Text = LocalizationService.ForSection("FfuApply.ScanSFUPattern").Format("Naming.Returns.Item", ListBox1.Items.Count)
            Beep()
            Exit Sub
        End If
        DynaLog.LogMessage("Scanning SFU files with given pattern...")
        For Each sfuFile In My.Computer.FileSystem.GetFiles(Path.GetDirectoryName(TextBox1.Text), FileIO.SearchOption.SearchTopLevelOnly, "*.sfu")
            If Path.GetFileNameWithoutExtension(sfuFile).StartsWith(PatternName) Then
                ListBox1.Items.Add(Path.GetFileName(sfuFile))
            End If
        Next
        DynaLog.LogMessage("Pattern search results: " & ListBox1.Items.Count)
        ToolStripStatusLabel1.Text = LocalizationService.ForSection("FfuApply.ScanSFUPattern").Format("Naming.Returns.Label", ListBox1.Items.Count)
        If ListBox1.Items.Count <= 0 Then Beep()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox4.Text = Path.GetFileNameWithoutExtension(TextBox1.Text)
        ScanSFUPattern(TextBox4.Text)
    End Sub

    Private Sub FfuApply_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        RichTextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        GroupBox4.ForeColor = CurrentTheme.ForegroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        StatusStrip1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        ToolStripStatusLabel1.Text = LocalizationService.ForSection("FfuApply")("NamingPattern.Required.Label")
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

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        RichTextBox1.Clear()
        Try
            Dim SelectedDriveMO As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT Description, Manufacturer, Model, PNPDeviceID, Size, Status, Partitions FROM Win32_DiskDrive WHERE DeviceID LIKE {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(TextBox2.Text)))
            If SelectedDriveMO IsNot Nothing Then
                Dim SelectedDriveDetails As Dictionary(Of String, Object) = WMIHelper.GetObjectValues(SelectedDriveMO(0), "Model", "Manufacturer",
                                                                                                      "Description", "PNPDeviceId", "Partitions",
                                                                                                      "Size", "Status")
                RichTextBox1.Text = String.Format("  - Model: {1}{0}" &
                                                  "  - Manufacturer: {2}{0}" &
                                                  "  - Description: {3}{0}" &
                                                  "  - Device ID (Plug-and-Play): {4}{0}" &
                                                  "  - Partitions: {5}{0}" &
                                                  "  - Size: {6} bytes (~{7}){0}" &
                                                  "  - Status: {8}",
                                                  Environment.NewLine, SelectedDriveDetails("Model"),
                                                                       SelectedDriveDetails("Manufacturer"),
                                                                       SelectedDriveDetails("Description"),
                                                                       SelectedDriveDetails("PNPDeviceId"),
                                                                       SelectedDriveDetails("Partitions"),
                                                                       SelectedDriveDetails("Size"),
                                                                       Converters.BytesToReadableSize(SelectedDriveDetails("Size")),
                                                                       SelectedDriveDetails("Status"))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        SFUFilePanel.Enabled = CheckBox4.Checked = True
    End Sub
End Class
