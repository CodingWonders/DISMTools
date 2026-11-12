Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports DISMTools.Elements.DriverPacks

Public Class AddDrivers
    Implements IImageTaskDialog

    Dim drvPkgList As New List(Of String)
    Dim drvPkgs(65535) As String
    Dim drvRecursiveList As New List(Of String)
    Dim drvRecursivePkgs(65535) As String

    Dim drvPkgCount As Integer

    Private IsDriverPack As Boolean
    Private SourceDriverPackLocations As New List(Of String)
    Private DriverPackExtractedFolders As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        drvPkgList.Clear()
        drvRecursiveList.Clear()
        ProgressPanel.MountDir = MainForm.MountDir
        drvPkgCount = lvDriverFiles.Items.Count
        DynaLog.LogMessage("Detecting drivers to add...")
        If lvDriverFiles.Items.Count > 0 Then
            For x = 0 To lvDriverFiles.Items.Count - 1
                drvPkgList.Add(lvDriverFiles.Items(x).SubItems(0).Text)
            Next
            ' Verify if items exist; remove entries that don't
            Dim nonExistentEntries As List(Of String) = drvPkgList.Where(Function(drvPkg)
                                                                             Dim exists As Boolean = True

                                                                             exists = Directory.Exists(drvPkg)
                                                                             If Not exists Then exists = File.Exists(drvPkg)

                                                                             Return Not exists
                                                                         End Function).ToList()
            If nonExistentEntries.Any() Then
                DynaLog.LogMessage("Some provided items do not exist.")
                For i = nonExistentEntries.Count - 1 To 0 Step -1
                    lvDriverFiles.Items.RemoveAt(drvPkgList.IndexOf(nonExistentEntries(i)))
                    drvPkgList.Remove(nonExistentEntries(i))
                    If CheckedListBox1.Items.Contains(nonExistentEntries(i)) Then CheckedListBox1.Items.RemoveAt(CheckedListBox1.FindStringExact(nonExistentEntries(i)))
                Next

                If drvPkgList.Count = 0 Then Exit Sub
            End If

            drvPkgCount = lvDriverFiles.Items.Count

            drvPkgs = drvPkgList.ToArray()
            For x = 0 To drvPkgs.Length - 1
                ProgressPanel.drvAdditionPkgs(x) = drvPkgs(x)
            Next
            For x = 0 To drvPkgCount - 1
                If lvDriverFiles.Items(x).SubItems(0).Text = "" Then Continue For

                If (File.GetAttributes(lvDriverFiles.Items(x).SubItems(0).Text) And FileAttributes.Directory) = FileAttributes.Directory And CheckedListBox1.CheckedItems.Contains(lvDriverFiles.Items(x).SubItems(0).Text) Then
                    drvRecursiveList.Add(lvDriverFiles.Items(x).SubItems(0).Text)
                End If
            Next
            If drvRecursiveList.Count > 0 Then
                drvRecursivePkgs = drvRecursiveList.ToArray()
                For x = 0 To drvRecursivePkgs.Length - 1
                    ProgressPanel.drvAdditionFolderRecursiveScan(x) = drvRecursivePkgs(x)
                Next
            End If
            If CheckBox1.Checked Then
                ProgressPanel.drvAdditionForceUnsigned = True
            Else
                ProgressPanel.drvAdditionForceUnsigned = False
            End If
            If CheckBox2.Checked And Not MainForm.OnlineManagement Then
                ProgressPanel.drvAdditionCommit = True
            Else
                ProgressPanel.drvAdditionCommit = False
            End If
        Else
            DynaLog.LogMessage("No items have been added to the queue.")
            MsgBox(LocalizationService.ForSection("AddDrivers.Validation")("DriverPackages.None.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.drvAdditionLastPkg = lvDriverFiles.Items(drvPkgCount - 1).SubItems(0).Text
        ProgressPanel.drvAdditionCount = drvPkgCount
        ProgressPanel.OperationNum = 75
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
        DynaLog.LogMessage("Driver file specified: " & Quote & OpenFileDialog1.FileName & Quote)
        IsDriverPack = Path.GetExtension(OpenFileDialog1.FileName).EndsWith("dtdrvpak", StringComparison.OrdinalIgnoreCase)
        If IsDriverPack Then
            If DriverPackHelper.IsPackValid(OpenFileDialog1.FileName) Then
                If MessageBox.Show(Me, "The provided driver pack will be extracted to a temporary folder on the system volume. Make sure you have enough space on it. Do you want to begin?", ImageTaskHeader1.ItemText, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
                SourceDriverPackLocations.Add(OpenFileDialog1.FileName)
                DriverPackExtractedFolders.Add(Path.Combine(Path.GetTempPath(), String.Format("DrvPakExtract_{0}", New Random().Next())))
                Try
                    If Not Directory.Exists(DriverPackExtractedFolders.Last()) Then Directory.CreateDirectory(DriverPackExtractedFolders.Last())
                Catch ex As Exception
                    Exit Sub
                End Try
                bwDriverPackExtractor.RunWorkerAsync()
            Else
                MessageBox.Show(Me, "The provided driver pack is not valid.", ImageTaskHeader1.ItemText, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            lvDriverFiles.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, LocalizationService.ForSection("AddDrivers.FileOk")("File.Label")}))
            Button3.Enabled = lvDriverFiles.Items.Count > 0
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Folder specified: " & Quote & FolderBrowserDialog1.SelectedPath & Quote)
            Cursor = Cursors.WaitCursor
            If My.Computer.FileSystem.GetFiles(FolderBrowserDialog1.SelectedPath, FileIO.SearchOption.SearchAllSubDirectories, "*.inf").Count > 0 Then
                DynaLog.LogMessage("This folder has driver files. Asking user...")
                Dim msg As String = ""
                msg = LocalizationService.ForSection("AddDrivers.Actions")("Package.Folder.Message")
                Select Case MsgBox(msg, vbYesNoCancel + vbInformation, ImageTaskHeader1.ItemText)
                    Case MsgBoxResult.Yes
                        DynaLog.LogMessage("Adding folder to queue...")
                        lvDriverFiles.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, LocalizationService.ForSection("AddDrivers")("Folder.Label")}))
                        CheckedListBox1.Items.Add(FolderBrowserDialog1.SelectedPath)
                        CheckedListBox1.SetItemChecked(CheckedListBox1.Items.IndexOf(FolderBrowserDialog1.SelectedPath), True)
                    Case MsgBoxResult.No
                        DynaLog.LogMessage("Opening driver picker...")
                        DriverManualFilePicker.DriverDir = FolderBrowserDialog1.SelectedPath
                        DriverManualFilePicker.ShowDialog(Me)
                    Case MsgBoxResult.Cancel
                        DynaLog.LogMessage("Cancelling addition of folder to the queue...")
                        Exit Sub
                End Select
            Else
                DynaLog.LogMessage("This folder does not have driver files.")
                MsgBox(LocalizationService.ForSection("AddDrivers.Actions")("Packages.None.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            End If
            Cursor = Cursors.Arrow
        End If
        Button3.Enabled = lvDriverFiles.Items.Count > 0
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvDriverFiles.SelectedIndexChanged
        If lvDriverFiles.SelectedItems.Count = 1 Then
            Button3.Enabled = True
            Button4.Enabled = True
        Else
            Button3.Enabled = False
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If lvDriverFiles.FocusedItem.Text <> "" Then
            If CheckedListBox1.Items.Contains(lvDriverFiles.FocusedItem.Text) Then
                CheckedListBox1.Items.RemoveAt(CheckedListBox1.FindStringExact(lvDriverFiles.FocusedItem.Text))
            End If
            lvDriverFiles.Items.Remove(lvDriverFiles.FocusedItem)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        lvDriverFiles.Items.Clear()
        CheckedListBox1.Items.Clear()
        Button3.Enabled = False
        Button4.Enabled = False
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        ' We'll outright return true, unless other things prevent this from opening
        Return True
    End Function

    Private Sub AddDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("AddDrivers")("Title.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("AddDrivers").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("AddDrivers")("Drivers.Required.Message")
        Label3.Text = LocalizationService.ForSection("AddDrivers")("Scan.Driver.Message")
        OK_Button.Text = LocalizationService.ForSection("AddDrivers")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("AddDrivers")("Cancel.Button")
        Button1.Text = LocalizationService.ForSection("AddDrivers")("AddFile.Button")
        Button2.Text = LocalizationService.ForSection("AddDrivers")("AddFolder.Button")
        Button3.Text = LocalizationService.ForSection("AddDrivers")("Remove.Entries.Button")
        Button4.Text = LocalizationService.ForSection("AddDrivers")("Remove.Selected.Entry.Button")
        CheckBox1.Text = LocalizationService.ForSection("AddDrivers")("Force.Install.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("AddDrivers")("CommitImage.CheckBox")
        GroupBox1.Text = LocalizationService.ForSection("AddDrivers")("DriverFiles.Group")
        GroupBox2.Text = LocalizationService.ForSection("AddDrivers")("DriverFolders.Group")
        GroupBox3.Text = LocalizationService.ForSection("AddDrivers")("Options.Group")
        lvDriverFiles.Columns(0).Text = LocalizationService.ForSection("AddDrivers")("FileFolder.Column")
        lvDriverFiles.Columns(1).Text = LocalizationService.ForSection("AddDrivers")("Type.Column")
        OpenFileDialog1.Title = LocalizationService.ForSection("AddDrivers")("DriverPackage.Title")
        FolderBrowserDialog1.Description = LocalizationService.ForSection("AddDrivers")("DriverFolder.Description")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        lvDriverFiles.BackColor = CurrentTheme.SectionBackgroundColor
        CheckedListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        lvDriverFiles.ForeColor = ForeColor
        CheckedListBox1.ForeColor = ForeColor
        CheckBox2.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ColumnHeader1.Width = WindowHelper.ScaleLogical(350)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(154)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub ListView1_DragEnter(sender As Object, e As DragEventArgs) Handles lvDriverFiles.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListView1_DragDrop(sender As Object, e As DragEventArgs) Handles lvDriverFiles.DragDrop
        DynaLog.LogMessage("Getting dropped items...")
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each PkgFile In PackageFiles
            DynaLog.LogMessage("Item: " & Quote & PkgFile & Quote)
            If Not (File.GetAttributes(PkgFile) And FileAttributes.Directory) = FileAttributes.Directory And Not Path.GetExtension(PkgFile).EndsWith("inf", StringComparison.OrdinalIgnoreCase) Then Continue For
            If (File.GetAttributes(PkgFile) And FileAttributes.Directory) = FileAttributes.Directory Then
                DynaLog.LogMessage("The specified item is a folder. Asking user...")
                Dim msg As String = ""
                msg = LocalizationService.ForSection("AddDrivers.DragDrop")("Package.Folder.Message")
                Select Case MsgBox(msg, vbYesNoCancel + vbInformation, ImageTaskHeader1.ItemText)
                    Case MsgBoxResult.Yes
                        DynaLog.LogMessage("Adding folder to queue...")
                        lvDriverFiles.Items.Add(New ListViewItem(New String() {PkgFile, LocalizationService.ForSection("AddDrivers.DragDrop")("Folder.Label")}))
                        CheckedListBox1.Items.Add(PkgFile)
                        CheckedListBox1.SetItemChecked(CheckedListBox1.Items.IndexOf(PkgFile), True)
                    Case MsgBoxResult.No
                        DynaLog.LogMessage("Opening driver picker...")
                        DriverManualFilePicker.DriverDir = PkgFile
                        DriverManualFilePicker.ShowDialog(Me)
                    Case MsgBoxResult.Cancel
                        DynaLog.LogMessage("Cancelling addition of folder to the queue...")
                        Continue For
                End Select
            Else
                DynaLog.LogMessage("The specified item is a file.")
                lvDriverFiles.Items.Add(New ListViewItem(New String() {PkgFile, LocalizationService.ForSection("AddDrivers.DragDrop")("File.Item")}))
            End If
        Next
        Button3.Enabled = lvDriverFiles.Items.Count > 0
    End Sub

    Private Sub bwDriverPackExtractor_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwDriverPackExtractor.DoWork
        Try
            ProgressReporter.SetMessage("Extracting Driver Pack contents...")
            Dim unzipper As New ZipExtractor()
            ' set as fallback
            unzipper.SevenZipExecutablePath = Path.Combine(Application.StartupPath, "bin", "utils", If(Environment.Is64BitOperatingSystem, "x64", "x86"), "7z.exe")
            AddHandler unzipper.ProgressChanged, AddressOf ReportUnzipperProgress
            unzipper.ExtractAll(SourceDriverPackLocations.Last(), DriverPackExtractedFolders.Last())
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ReportUnzipperProgress(sender As Object, e As ZipProgressEventArgs)
        bwDriverPackExtractor.ReportProgress(e.Percentage)
    End Sub

    Private Sub bwDriverPackExtractor_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bwDriverPackExtractor.ProgressChanged
        ProgressReporter.ReportProgress(Me, e.ProgressPercentage)
    End Sub

    Private Sub DeleteDriverPackExtractedFiles()
        If DriverPackExtractedFolders.Any() Then
            For i = DriverPackExtractedFolders.Count - 1 To 0 Step -1
                Try
                    Directory.Delete(DriverPackExtractedFolders(i), True)
                    DriverPackExtractedFolders.RemoveAt(i)
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Private Sub DeleteDriverPackExtractedFiles(PackLocation As String)
        If PackLocation = "" Then Exit Sub
        If Not Directory.Exists(PackLocation) Then Exit Sub
        Try
            Directory.Delete(PackLocation, True)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub bwDriverPackExtractor_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDriverPackExtractor.RunWorkerCompleted
        ProgressReporter.Hide()
        If e.Error IsNot Nothing Then
            DynaLog.LogMessage(e.Error.Message)
        Else
            SelectDriversFromDriverPack.SourceDriverPackFolder = DriverPackExtractedFolders.Last()
            If SelectDriversFromDriverPack.ShowDialog(Me) = Windows.Forms.DialogResult.Cancel Then
                DeleteDriverPackExtractedFiles(DriverPackExtractedFolders.Last())
            End If

            Dim driverFilesPath As String = Path.Combine(DriverPackExtractedFolders.Last(), "DRIVERS")
            For Each SelectedDriverId In SelectDriversFromDriverPack.SelectedDriverIDs
                Dim folderName As String = String.Format("DRV_{0}", SelectedDriverId.ToString().PadLeft(10, "0")),
                    finalFolderPath As String = Path.Combine(driverFilesPath, folderName)

                lvDriverFiles.Items.Add(New ListViewItem(New String() {finalFolderPath, LocalizationService.ForSection("AddDrivers")("Folder.Label")}))
                CheckedListBox1.Items.Add(finalFolderPath)
                CheckedListBox1.SetItemChecked(CheckedListBox1.Items.IndexOf(finalFolderPath), True)
            Next
        End If
    End Sub

    Private Sub AddDrivers_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            DeleteDriverPackExtractedFiles()
        Catch ex As Exception
            ' ignore
        End Try
    End Sub
End Class
