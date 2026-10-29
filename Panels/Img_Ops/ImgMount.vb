Imports System.IO
Imports System.Windows.Forms
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars
Imports System.Threading
Imports Microsoft.Dism

Public Class ImgMount

    Dim WimInfo As Process
    Dim WimStr As String
    Dim IsReqField1Valid As Boolean
    Dim IsReqField2Valid As Boolean
    Dim IsReqField3Valid As Boolean
    Dim DismVerChecker As FileVersionInfo

    Dim projPath As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Checking if the mount directory exists...")
        If Not Directory.Exists(TextBox2.Text) Then
            DynaLog.LogMessage("The mount directory does not exist. Asking the user whether or not to create it...")
            MountOpDirCreationDialog.ShowDialog(Me)
            If MountOpDirCreationDialog.DialogResult = Windows.Forms.DialogResult.Yes Then
                Try
                    DynaLog.LogMessage("The user wants the mount directory to be created. Attempting to create it...")
                    Directory.CreateDirectory(TextBox2.Text)
                Catch ex As Exception
                    DynaLog.LogMessage("Could not create the mount directory. Error message: " & ex.Message)
                    MsgBox(LocalizationService.ForSection("ImgMount.Validation").Format("Create.Dir.Reason.Message", ex.ToString(), ex.Message), MsgBoxStyle.OkOnly + vbCritical, LocalizationService.ForSection("ImgMount.Validation")("MountImage.Title"))
                    Exit Sub
                End Try
            ElseIf MountOpDirCreationDialog.DialogResult = Windows.Forms.DialogResult.No Then
                DynaLog.LogMessage("The user does not want the mount directory to be created.")
                Exit Sub
            End If
        End If
        'TextBox1.Text = ProgressPanel.SourceImg
        'NumericUpDown1.Value = ImgIndex
        'TextBox2.Text = ProgressPanel.MountDir
        ProgressPanel.SourceImg = TextBox1.Text
        ProgressPanel.ImgIndex = NumericUpDown1.Value
        ProgressPanel.MountDir = TextBox2.Text
        If CheckBox1.Checked Then
            ProgressPanel.isReadOnly = True
        Else
            ProgressPanel.isReadOnly = False
        End If
        If CheckBox3.Checked Then
            ProgressPanel.isOptimized = True
        Else
            ProgressPanel.isOptimized = False
        End If
        If CheckBox4.Checked Then
            ProgressPanel.isIntegrityTested = True
        Else
            ProgressPanel.isIntegrityTested = False
        End If
        'ProgressPanel.SourceImg = SourceImg
        'ProgressPanel.ImgIndex = ImgIndex
        'ProgressPanel.MountDir = MountDir
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 15
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgMount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ImgMount")("MountImage.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("ImgMount")("Options.Required.Label")
        Label3.Text = LocalizationService.ForSection("ImgMount")("ImageFile.Label")
        If Path.GetExtension(TextBox1.Text).EndsWith(LocalizationService.ForSection("ImgMount")("ESD.Label"), StringComparison.OrdinalIgnoreCase) Then
            Label4.Text = LocalizationService.ForSection("ImgMount")("Convert.File.WIM.Label")
            Button3.Text = LocalizationService.ForSection("ImgMount")("Convert.Button")
        ElseIf Path.GetExtension(TextBox1.Text).EndsWith(LocalizationService.ForSection("ImgMount")("SWM.Label"), StringComparison.OrdinalIgnoreCase) Then
            Label4.Text = LocalizationService.ForSection("ImgMount")("Merge.Swmfiles.Item")
            Button3.Text = LocalizationService.ForSection("ImgMount")("Merge.Item")
        End If
        Label6.Text = LocalizationService.ForSection("ImgMount")("MountDirectory.Label")
        Label7.Text = LocalizationService.ForSection("ImgMount")("Index.Label")
        Label11.Text = LocalizationService.ForSection("ImgMount")("Fields.End.Required.Label")
        GroupBox1.Text = LocalizationService.ForSection("ImgMount")("Source.Group")
        GroupBox2.Text = LocalizationService.ForSection("ImgMount")("Destination.Group")
        GroupBox3.Text = LocalizationService.ForSection("ImgMount")("Options.Group")
        Button1.Text = LocalizationService.ForSection("ImgMount")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("ImgMount")("Browse.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImgMount")("Cancel.Button")
        OK_Button.Text = LocalizationService.ForSection("ImgMount")("Ok.Button")
        ListView1.Columns(0).Text = LocalizationService.ForSection("ImgMount")("Index.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("ImgMount")("ImageName.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("ImgMount")("ImageDescription.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("ImgMount")("ImageVersion.Column")
        CheckBox1.Text = LocalizationService.ForSection("ImgMount")("Mount.Read.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("ImgMount")("Optimize.Times.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("ImgMount")("Integrity.CheckBox")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        DismVerChecker = FileVersionInfo.GetVersionInfo(MainForm.DismExe)
        If DismVerChecker.ProductMajorPart = 6 And DismVerChecker.ProductMinorPart = 1 Then
            FileSpecDialog.Filter = LocalizationService.ForSection("Panels.ImageOps.MountImage")("WIM.Files.Filter")
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        If TextBox1.Text <> "" AndAlso File.Exists(TextBox1.Text) AndAlso MainForm.MountedImageList.FirstOrDefault(Function(image) image.ImageFile = TextBox1.Text) IsNot Nothing Then
            IsReqField1Valid = False
            OK_Button.Enabled = False
        Else
            IsReqField1Valid = True
            OK_Button.Enabled = True
        End If
        Try
            DynaLog.LogMessage("Setting mount directory to be the one provided by the project...")
            If ProgressPanel.OperationNum = 0 Then
                If ProgressPanel.projPath = "" Then
                    TextBox2.Text = MainForm.projPath & "\mount"
                Else
                    TextBox2.Text = ProgressPanel.projPath & "\" & ProgressPanel.projName & "\mount"
                End If
            Else
                TextBox2.Text = MainForm.projPath & "\mount"
            End If
        Catch ex As Exception

        End Try

        ColumnHeader1.Width = WindowHelper.ScaleLogical(44)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(256)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(256)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(128)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FileSpecDialog.ShowDialog(Me)
        If TextBox1.Text <> "" Then
            If Path.GetExtension(TextBox1.Text).EndsWith("esd", StringComparison.OrdinalIgnoreCase) Then
                Button3.Visible = True
                Label4.Visible = True
                Label4.Text = LocalizationService.ForSection("ImgMount")("Convert.File.WIM.Label")
                Button3.Text = LocalizationService.ForSection("ImgMount.Actions")("Convert.Button")
                IsReqField1Valid = False
                ImgWim2Esd.TextBox1.Text = TextBox1.Text
                ImgWim2Esd.TextBox2.Text = TextBox1.Text.Replace(Path.GetExtension(TextBox1.Text), ".wim").Trim()
                Hide()
                ImgWim2Esd.ShowDialog(MainForm)
                Show()
                If ImgWim2Esd.DialogResult = Windows.Forms.DialogResult.OK And File.Exists(ImgWim2Esd.TextBox2.Text) Then
                    TextBox1.Text = ImgWim2Esd.TextBox2.Text
                    Button3.Visible = False
                    Label4.Visible = False
                ElseIf ImgWim2Esd.DialogResult = Windows.Forms.DialogResult.Cancel Then
                    MsgBox(LocalizationService.ForSection("ImgMount.Actions")("Convert.Image.WIM.Message"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
                End If
            ElseIf Path.GetExtension(TextBox1.Text).EndsWith("swm", StringComparison.OrdinalIgnoreCase) Then
                Button3.Visible = True
                Label4.Visible = True
                Label4.Text = LocalizationService.ForSection("ImgMount")("Merge.Swmfiles.Item")
                Button3.Text = LocalizationService.ForSection("ImgMount")("Merge.Item")
                IsReqField1Valid = False
                ImgSwmToWim.TextBox1.Text = TextBox1.Text
                ImgSwmToWim.TextBox2.Text = TextBox1.Text.Replace(Path.GetExtension(TextBox1.Text), ".wim").Trim()
                Hide()
                ImgSwmToWim.ShowDialog(MainForm)
                Show()
                If ImgSwmToWim.DialogResult = Windows.Forms.DialogResult.OK And File.Exists(ImgSwmToWim.TextBox2.Text) Then
                    TextBox1.Text = ImgSwmToWim.TextBox2.Text
                    Button3.Visible = False
                    Label4.Visible = False
                ElseIf ImgSwmToWim.DialogResult = Windows.Forms.DialogResult.Cancel Then
                    MsgBox(LocalizationService.ForSection("ImgMount.Actions")("Merge.Swmfiles.Message"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
                End If
            ElseIf Path.GetExtension(TextBox1.Text).EndsWith(".iso", StringComparison.OrdinalIgnoreCase) Then
                DynaLog.LogMessage("Performing extraction of this ISO file...")
                projPath = MainForm.projPath
                IsoExtractorBW.RunWorkerAsync()
            End If
        Else
            Button3.Visible = False
            Label4.Visible = False
        End If
    End Sub

    Private Sub FileSpecDialog_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles FileSpecDialog.FileOk
        TextBox1.Text = FileSpecDialog.FileName
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs)
        ListView1.Items.Clear()
        Width = 800
        CenterToParent()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FolderBrowserDialog1.ShowDialog(Me)
        If DialogResult.OK Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox2.Text = ""
        End If
        GetFields()
    End Sub

    Sub GetIndexes(ImgFile As String)
        DynaLog.LogMessage("Image file to get information about: " & Quote & ImgFile & Quote)
        DynaLog.LogMessage("Checking if mounted image detector is busy...")
        MainForm.StopMountedImageDetector()
        ListView1.Items.Clear()
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Dim imgInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(ImgFile)
            DynaLog.LogMessage("Information collection count: " & imgInfoCollection.Count)
            NumericUpDown1.Maximum = imgInfoCollection.Count
            If imgInfoCollection.Count > 0 Then
                DynaLog.LogMessage("This file has images. Updating list...")
                ListView1.Items.AddRange(imgInfoCollection.Select(Function(imgInfo) New ListViewItem(New String() {imgInfo.ImageIndex, imgInfo.ImageName, imgInfo.ImageDescription, imgInfo.ProductVersion.ToString()})).ToArray())
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            Dim msg As String = ""
            msg = LocalizationService.ForSection("ImgMount.GetIndexes").Format("Gather.ImageFile.Message", ex.ToString(), ex.Message, Hex(ex.HResult))
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
        Finally
            Try
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
    End Sub

    Sub GetFields()
        DynaLog.LogMessage("Checking fields...")
        IsReqField3Valid = True
        If TextBox1.Text = "" Then
            If ProgressPanel.OperationNum = 15 Then
                TextBox1.Text = ProgressPanel.SourceImg
            Else
                IsReqField1Valid = False
            End If
        Else
            If File.Exists(TextBox1.Text) Then
                IsReqField1Valid = True
                ProgressPanel.SourceImg = TextBox1.Text
                If Path.GetExtension(TextBox1.Text).EndsWith("esd", StringComparison.OrdinalIgnoreCase) Or
                    Path.GetExtension(TextBox1.Text).EndsWith("swm", StringComparison.OrdinalIgnoreCase) Or
                    Path.GetExtension(TextBox1.Text).EndsWith("iso", StringComparison.OrdinalIgnoreCase) Then
                    IsReqField1Valid = False
                ElseIf MainForm.MountedImageList.Select(Function(image) image.ImageFile).Contains(TextBox1.Text) Then
                    IsReqField1Valid = False
                End If
                If IsReqField1Valid Then GetIndexes(TextBox1.Text)
            Else
                IsReqField1Valid = False
            End If
        End If
        If TextBox2.Text = "" Then
            If ProgressPanel.OperationNum = 15 Then
                TextBox2.Text = ProgressPanel.MountDir
            Else
                IsReqField1Valid = False
            End If
            IsReqField2Valid = False
        Else
            If Directory.Exists(TextBox2.Text) Then
                IsReqField2Valid = True
                ProgressPanel.MountDir = TextBox2.Text
            End If
        End If
        If IsReqField1Valid And IsReqField2Valid And IsReqField3Valid Then
            DynaLog.LogMessage("All fields are valid.")
            OK_Button.Enabled = True
        Else
            DynaLog.LogMessage("None or not all fields are valid.")
            OK_Button.Enabled = False
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        GetFields()
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) And MainForm.MountedImageList.Select(Function(image) image.ImageFile).Contains(TextBox1.Text) Then
            DynaLog.LogMessage("The Windows image is already mounted.")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("ImgMount")("Image.Already.Message")
            MsgBox(msg, vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        GetFields()
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        ProgressPanel.ImgIndex = NumericUpDown1.Value
    End Sub

    Private Sub ImgMount_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        MainForm.StartMountedImageDetector()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Path.GetExtension(TextBox1.Text).EndsWith("esd", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("Beginning conversion from ESD to WIM...")
            IsReqField1Valid = False
            ImgWim2Esd.TextBox1.Text = TextBox1.Text
            ImgWim2Esd.TextBox2.Text = TextBox1.Text.Replace(Path.GetExtension(TextBox1.Text), ".wim").Trim()
            Hide()
            ImgWim2Esd.ShowDialog(MainForm)
            Show()
            If ImgWim2Esd.DialogResult = Windows.Forms.DialogResult.OK And File.Exists(ImgWim2Esd.TextBox2.Text) Then
                DynaLog.LogMessage("Conversion has been carried over successfully. Using newly created WIM file...")
                TextBox1.Text = ImgWim2Esd.TextBox2.Text
                Button3.Visible = False
                Label4.Visible = False
            ElseIf ImgWim2Esd.DialogResult = Windows.Forms.DialogResult.Cancel Then
                DynaLog.LogMessage("No conversion has been made.")
                MsgBox(LocalizationService.ForSection("ImgMount.Actions")("Convert.Image.WIM.Message"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            End If
        ElseIf Path.GetExtension(TextBox1.Text).EndsWith("swm", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("Beginning merger of SWM files...")
            IsReqField1Valid = False
            ImgSwmToWim.TextBox1.Text = TextBox1.Text
            ImgSwmToWim.TextBox2.Text = TextBox1.Text.Replace(Path.GetExtension(TextBox1.Text), ".wim").Trim()
            Hide()
            ImgSwmToWim.ShowDialog(MainForm)
            Show()
            If ImgSwmToWim.DialogResult = Windows.Forms.DialogResult.OK And File.Exists(ImgSwmToWim.TextBox2.Text) Then
                DynaLog.LogMessage("Merger has been carried over successfully. Using newly created WIM file...")
                TextBox1.Text = ImgSwmToWim.TextBox2.Text
                Button3.Visible = False
                Label4.Visible = False
            ElseIf ImgSwmToWim.DialogResult = Windows.Forms.DialogResult.Cancel Then
                DynaLog.LogMessage("No merger has been made.")
                MsgBox(LocalizationService.ForSection("ImgMount.Actions")("Swm.Merge.Required.Message"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            End If
        Else
            Button3.Visible = False
            Label4.Visible = False
        End If
    End Sub

    Private Sub ExtractIsoFileContents(ProjectPath As String, IsoFile As String)
        Try
            ProgressReporter.SetMessage("Preparing to mount ISO file...")
            IsoExtractorBW.ReportProgress(0)
            Dim extractedImagePath As String = Path.Combine(ProjectPath, "IsoFileContents")
            If Not Directory.Exists(extractedImagePath) Then
                Directory.CreateDirectory(extractedImagePath)
            End If
            ProgressReporter.SetMessage("Mounting ISO file...")
            IsoExtractorBW.ReportProgress(10)
            Dim mountLetter As Char = IsoHelper.MountIso(IsoFile)
            If mountLetter = Chr(0) Then Exit Sub
            ProgressReporter.SetMessage("Scanning mounted ISO file for Windows images...")
            IsoExtractorBW.ReportProgress(25)
            Dim WindowsImageFiles As String() = Directory.EnumerateFiles(String.Format("{0}:\", mountLetter), "*.*", SearchOption.AllDirectories).Where(Function(fileInDisc) {".wim", ".esd"}.Contains(Path.GetExtension(fileInDisc).ToLowerInvariant())).ToArray()
            For Each WindowsImageFile In WindowsImageFiles
                ProgressReporter.SetMessage(String.Format("Copying file {0} to your project...", Quote & Path.GetFileName(WindowsImageFile) & Quote))
                IsoExtractorBW.ReportProgress(50)
                File.Copy(WindowsImageFile, Path.Combine(ProjectPath, "IsoFileContents", Path.GetFileName(WindowsImageFile)), True)
            Next
            ProgressReporter.SetMessage("Unmounting ISO file...")
            IsoExtractorBW.ReportProgress(95)
            IsoHelper.DismountIso(IsoFile)
        Catch ex As Exception
            DynaLog.LogMessage("Could not extract relevant contents. Error message: " & ex.Message)
            Throw
        End Try
        ProgressReporter.SetMessage("Extraction complete.")
        IsoExtractorBW.ReportProgress(100)
    End Sub

    Private Sub IsoExtractorBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles IsoExtractorBW.DoWork
        ExtractIsoFileContents(projPath, TextBox1.Text)
    End Sub

    Private Sub IsoExtractorBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles IsoExtractorBW.ProgressChanged
        ProgressReporter.ReportProgress(Me, e.ProgressPercentage)
    End Sub

    Private Sub IsoExtractorBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles IsoExtractorBW.RunWorkerCompleted
        ProgressReporter.Hide()
        If e.Error Is Nothing Then
            ' Try to find the install image
            ImageFilePickerDialog.SourceImageFileRepoPath = Path.Combine(projPath, "IsoFileContents")
            If ImageFilePickerDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                TextBox1.Text = ImageFilePickerDialog.SelectedImageFilePath
            Else
                MessageBox.Show(LocalizationService.ForSection("ImageOps.Mount.Messages")("Copied.Image.Message"), LocalizationService.ForSection("ImageOps.Mount.Messages")("Extraction.Succeeded.Label"), MessageBoxButtons.OK, MessageBoxIcon.Information)
                Dim installImagePath_WIM As String = Path.Combine(projPath, "IsoFileContents", "install.wim")
                Dim installImagePath_ESD As String = Path.Combine(projPath, "IsoFileContents", "install.esd")

                If File.Exists(installImagePath_WIM) Then TextBox1.Text = installImagePath_WIM
                If File.Exists(installImagePath_ESD) Then TextBox1.Text = installImagePath_ESD
            End If
        Else
            ' Then we've failed
            MessageBox.Show(LocalizationService.ForSection("ImageOps.Mount.Messages")("Windows.Message"), LocalizationService.ForSection("ImageOps.Mount.Messages")("Extraction.Succeeded.Message"), MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            If ListView1.SelectedItems.Count = 1 Then
                NumericUpDown1.Value = ListView1.FocusedItem.Index + 1
            End If
        Catch ex As Exception
            NumericUpDown1.Value = 1
            NumericUpDown1.Maximum = 1
        End Try
    End Sub
End Class
