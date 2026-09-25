Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class AddDrivers
    Implements IImageTaskDialog

    Dim drvPkgList As New List(Of String)
    Dim drvPkgs(65535) As String
    Dim drvRecursiveList As New List(Of String)
    Dim drvRecursivePkgs(65535) As String

    Dim drvPkgCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        drvPkgList.Clear()
        drvRecursiveList.Clear()
        ProgressPanel.MountDir = MainForm.MountDir
        drvPkgCount = ListView1.Items.Count
        DynaLog.LogMessage("Detecting drivers to add...")
        If ListView1.Items.Count > 0 Then
            For x = 0 To ListView1.Items.Count - 1
                drvPkgList.Add(ListView1.Items(x).SubItems(0).Text)
            Next
            drvPkgs = drvPkgList.ToArray()
            For x = 0 To drvPkgs.Length - 1
                ProgressPanel.drvAdditionPkgs(x) = drvPkgs(x)
            Next
            For x = 0 To drvPkgCount - 1
                If (File.GetAttributes(ListView1.Items(x).SubItems(0).Text) And FileAttributes.Directory) = FileAttributes.Directory And CheckedListBox1.CheckedItems.Contains(ListView1.Items(x).SubItems(0).Text) Then
                    drvRecursiveList.Add(ListView1.Items(x).SubItems(0).Text)
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
        ProgressPanel.drvAdditionLastPkg = ListView1.Items(drvPkgCount - 1).SubItems(0).Text
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
        ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, LocalizationService.ForSection("AddDrivers.FileOk")("File.Label")}))
        Button3.Enabled = ListView1.Items.Count > 0
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
                        ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, LocalizationService.ForSection("AddDrivers")("Folder.Label")}))
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
        Button3.Enabled = ListView1.Items.Count > 0
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            Button3.Enabled = True
            Button4.Enabled = True
        Else
            Button3.Enabled = False
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListView1.FocusedItem.Text <> "" Then
            If CheckedListBox1.Items.Contains(ListView1.FocusedItem.Text) Then
                CheckedListBox1.Items.RemoveAt(CheckedListBox1.FindStringExact(ListView1.FocusedItem.Text))
            End If
            ListView1.Items.Remove(ListView1.FocusedItem)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListView1.Items.Clear()
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
        ListView1.Columns(0).Text = LocalizationService.ForSection("AddDrivers")("FileFolder.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("AddDrivers")("Type.Column")
        OpenFileDialog1.Title = LocalizationService.ForSection("AddDrivers")("DriverPackage.Title")
        FolderBrowserDialog1.Description = LocalizationService.ForSection("AddDrivers")("DriverFolder.Description")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        CheckedListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        CheckedListBox1.ForeColor = ForeColor
        CheckBox2.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ColumnHeader1.Width = WindowHelper.ScaleLogical(350)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(154)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub ListView1_DragEnter(sender As Object, e As DragEventArgs) Handles ListView1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListView1_DragDrop(sender As Object, e As DragEventArgs) Handles ListView1.DragDrop
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
                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, LocalizationService.ForSection("AddDrivers.DragDrop")("Folder.Label")}))
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
                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, LocalizationService.ForSection("AddDrivers.DragDrop")("File.Item")}))
            End If
        Next
        Button3.Enabled = ListView1.Items.Count > 0
    End Sub
End Class
