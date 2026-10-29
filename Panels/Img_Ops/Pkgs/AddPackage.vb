Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class AddPackageDlg

    Public CheckedCount As Integer
    Public pkgCount As Integer
    Public pkgs(65535) As String        ' This is hard-coded. If you have more than 65535 selected packages, the program will throw an exception
    Dim Addition_MUMFile As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        ProgressPanel.pkgSource = TextBox1.Text
        pkgCount = CheckedListBox1.CheckedItems.Count
        If RadioButton1.Checked AndAlso (Addition_MUMFile Is Nothing OrElse Addition_MUMFile = "") Then
            DynaLog.LogMessage("Packages will be added recursively.")
            ProgressPanel.pkgAdditionOp = 0
        Else
            DynaLog.LogMessage("Packages will not be added recursively.")
            DynaLog.LogMessage("Getting whether or not a MUM (Microsoft Update Manifest) file has been specified...")
            If Addition_MUMFile <> "" Then
                DynaLog.LogMessage("An update manifest has been specified for addition.")
                ProgressPanel.pkgAdditionOp = 2
                ProgressPanel.pkgCount = 1
            Else
                DynaLog.LogMessage("An update manifest has not been specified for addition. Proceeding with selective operation...")
                ProgressPanel.pkgAdditionOp = 1
                ProgressPanel.pkgCount = pkgCount
            End If
        End If
        If CheckBox1.Checked Then
            ProgressPanel.pkgIgnoreApplicabilityChecks = True
        Else
            ProgressPanel.pkgIgnoreApplicabilityChecks = False
        End If
        If CheckBox2.Checked Then
            ProgressPanel.pkgPreventIfPendingOnline = True
        Else
            ProgressPanel.pkgPreventIfPendingOnline = False
        End If
        If CheckBox3.Checked And Not MainForm.OnlineManagement Then
            ProgressPanel.pkgAdditionCommit = True
        Else
            ProgressPanel.pkgAdditionCommit = False
        End If
        If ProgressPanel.pkgAdditionOp = 1 Then
            DynaLog.LogMessage("A selective package addition operation will be started. Checking packages to add...")
            If CheckedListBox1.CheckedItems.Count <= 0 Then
                DynaLog.LogMessage("No items have been added to the queue.")
                MessageBox.Show(MainForm, LocalizationService.ForSection("AddPackage.Validation")("Packages.Message"), LocalizationService.ForSection("AddPackage.Validation")("PackagesSelected.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                DynaLog.LogMessage("OS packages to add to the queue: " & pkgCount)
                If pkgCount > 65535 Then
                    MessageBox.Show(MainForm, LocalizationService.ForSection("Panels.Packages.Add")("CurrentLimit.Message"), LocalizationService.ForSection("Panels.Packages.Add")("CurrentLimit.Detail"), MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    DynaLog.LogMessage("Adding AppX packages to queue...")
                    Try
                        For x As Integer = 0 To pkgCount - 1
                            pkgs(x) = CheckedListBox1.CheckedItems(x).ToString()
                        Next
                        For x = 0 To pkgs.Length
                            ProgressPanel.pkgs(x) = pkgs(x)
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If ProgressPanel.pkgAdditionOp = 1 Then
                    ProgressPanel.pkgLastCheckedPackageName = CheckedListBox1.CheckedItems(pkgCount - 1).ToString()
                End If
                ProgressPanel.OperationNum = 26
                Visible = False
                ProgressPanel.ShowDialog(MainForm)
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        ElseIf ProgressPanel.pkgAdditionOp = 0 Then
            ProgressPanel.OperationNum = 26
            Visible = False
            ProgressPanel.ShowDialog(MainForm)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        ElseIf ProgressPanel.pkgAdditionOp = 2 Then
            pkgs(0) = Addition_MUMFile
            ProgressPanel.pkgs(0) = pkgs(0)
            ProgressPanel.OperationNum = 26
            Visible = False
            ProgressPanel.ShowDialog(MainForm)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog(Me)
        If DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
            ScanBW.RunWorkerAsync()
        End If
    End Sub

    Sub GatherPackages(FolderToScan As String)
        DynaLog.LogMessage("Scanning folder for possible packages...")
        DynaLog.LogMessage("- Folder to scan: " & Quote & FolderToScan & Quote)
        DynaLog.LogMessage("Clearing existing items...")
        CheckedListBox1.Items.Clear()
        Cursor = Cursors.WaitCursor
                Label4.Text = LocalizationService.ForSection("AddPackage.GatherPackages")("Scanning.Dir.Label")
        Refresh()
        ' TODO: show CheckedListBox items without full path
        Try
            DynaLog.LogMessage("Getting CAB files in directory (recursive operation)...")
            For Each CabPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchAllSubDirectories, "*.cab")
                If CabPkg.Contains("MsuExtract") Then
                    ' CAB files stored in MsuExtract are skipped, as they come from MSU files. Skip these items and continue loop
                    DynaLog.LogMessage("CAB file " & Quote & Path.GetFileName(CabPkg) & Quote & " is in a directory that is a result of MSU content extraction. Skipping...")
                    Continue For
                End If
                CheckedListBox1.Items.Add(CabPkg)
            Next
            DynaLog.LogMessage("Getting MSU files in directory (recursive operation)...")
            For Each MsuPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchAllSubDirectories, "*.msu")
                CheckedListBox1.Items.Add(MsuPkg)
            Next
        Catch ex As Exception
            DynaLog.LogMessage("Getting CAB files in directory...")
            For Each CabPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchTopLevelOnly, "*.cab")
                If CabPkg.Contains("MsuExtract") Then
                    ' CAB files stored in MsuExtract are skipped, as they come from MSU files. Skip these items and continue loop
                    DynaLog.LogMessage("CAB file " & Quote & Path.GetFileName(CabPkg) & Quote & " is in a directory that is a result of MSU content extraction. Skipping...")
                    Continue For
                End If
                CheckedListBox1.Items.Add(CabPkg)
            Next
            DynaLog.LogMessage("Getting MSU files in directory...")
            For Each MsuPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchTopLevelOnly, "*.msu")
                CheckedListBox1.Items.Add(MsuPkg)
            Next
        End Try
        CountItems()
        Cursor = Cursors.Arrow
    End Sub

    Sub CountItems()
        DynaLog.LogMessage("Counting items in list...")
        DynaLog.LogMessage("Balancing count of checked items if necessary...")
        If CheckedCount > CheckedListBox1.CheckedItems.Count Then
            Do Until CheckedCount = CheckedListBox1.CheckedItems.Count
                CheckedCount -= 1
            Loop
        ElseIf CheckedCount < 0 Then
            Do Until CheckedCount = 0
                CheckedCount += 1
            Loop
        End If
        DynaLog.LogMessage("Items in list: " & CheckedListBox1.Items.Count)
        If CheckedListBox1.Items.Count = 0 Then
            DynaLog.LogMessage("This folder does not have any items")
            Label4.Text = LocalizationService.ForSection("AddPackage.CountItems")("Folder.Contain.Label")
            Beep()
        Else
            If CheckedListBox1.Items.Count = 1 Then
                Label4.Text = LocalizationService.ForSection("AddPackage.CountItems").Format("Folder.Contains.Label", CheckedListBox1.Items.Count)
            Else
                Label4.Text = LocalizationService.ForSection("AddPackage.CountItems").Format("Folder.Packages.Label", CheckedListBox1.Items.Count)
            End If
        End If
    End Sub

    Private Sub AddPackageDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("AddPackage")("AddPackages.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("AddPackage")("PackageSource.Label")
        Label3.Text = LocalizationService.ForSection("AddPackage")("PackageOperation.Label")
        Button1.Text = LocalizationService.ForSection("AddPackage")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("AddPackage")("SelectAll.Button")
        Button3.Text = LocalizationService.ForSection("AddPackage")("SelectNone.Button")
        Button4.Text = LocalizationService.ForSection("AddPackage")("Update.Manifest.Button")
        Cancel_Button.Text = LocalizationService.ForSection("AddPackage")("Cancel.Button")
        OK_Button.Text = LocalizationService.ForSection("AddPackage")("Ok.Button")
        RadioButton1.Text = LocalizationService.ForSection("AddPkg")("ScanRecursive.RadioButton")
        RadioButton2.Text = LocalizationService.ForSection("AddPackage")("Packages.Choose.RadioButton")
        CheckBox1.Text = LocalizationService.ForSection("AddPackage")("Ignore.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("AddPkg")("Skip.Online.Install.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("AddPackage")("CommitImage.CheckBox")
        FolderBrowserDialog1.Description = LocalizationService.ForSection("AddPackage")("CabFolder.Description")
        GroupBox1.Text = LocalizationService.ForSection("AddPackage")("Packages.Group")
        GroupBox2.Text = LocalizationService.ForSection("AddPackage")("Options.Group")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        CheckedListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        CheckedListBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Control.CheckForIllegalCrossThreadCalls = False
        If CheckedListBox1.Items.Count = 0 Then
            Label4.Text = LocalizationService.ForSection("AddPackage")("Dir.CAB.Required.Label")
        End If
        CheckBox3.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Addition_MUMFile = ""
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label4.Enabled = False
            CheckedListBox1.Enabled = False
            TableLayoutPanel2.Enabled = False
        Else
            Label4.Enabled = True
            CheckedListBox1.Enabled = True
            TableLayoutPanel2.Enabled = True
        End If
        If ProgressPanel.OperationNum = 26 Then
            pkgCount = CheckedCount
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DynaLog.LogMessage("Checking all packages in list to add to the addition queue...")
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, True)
            CheckedCount += 1
            CountItems()
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DynaLog.LogMessage("Unchecking all packages in list to add to the addition queue...")
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, False)
            CheckedCount -= 1
            CountItems()
        Next
        DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub ScanBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ScanBW.DoWork
        DynaLog.LogMessage("Getting items in specified folder")
        GatherPackages(TextBox1.Text)
    End Sub

    Private Sub CheckedListBox1_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck
        CheckedCount = CheckedListBox1.CheckedItems.Count
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If MUMAdditionDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("A user may have specified an update manifest file.")
            Addition_MUMFile = MUMAdditionDialog.MUMFile
            If File.Exists(Addition_MUMFile) Then
                DynaLog.LogMessage("The specified update manifest exists in the file system. Proceeding to add it...")
                OK_Button.PerformClick()
            End If
        End If
    End Sub
End Class
