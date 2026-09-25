Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Public Class RemPackage
    Implements IImageTaskDialog

    Public pkgRemovalCount As Integer
    Public pkgRemovalNames(65535) As String
    Public pkgRemovalFiles(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        ProgressPanel.pkgRemovalSource = TextBox1.Text
        DynaLog.LogMessage("Detecting packages to remove...")
        If RadioButton1.Checked Then
            DynaLog.LogMessage("A selective removal of package files will be done.")
            DynaLog.LogMessage("The selected items may not be even installed, but this is the least of our concerns.")
            pkgRemovalCount = CheckedListBox1.CheckedItems.Count
            ProgressPanel.pkgRemovalOp = 0
            ProgressPanel.pkgRemovalCount = pkgRemovalCount
            If CheckedListBox1.CheckedItems.Count <= 0 Then
                DynaLog.LogMessage("No items have been added to the queue.")
                MessageBox.Show(MainForm, LocalizationService.ForSection("RemPackage.Validation")("No.Packages.Selected.Message"), LocalizationService.ForSection("RemPackage.Validation")("Packages.Selected.None.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Else
                If pkgRemovalCount > 65535 Then
                    MessageBox.Show(MainForm, LocalizationService.ForSection("Panels.Packages.Remove")("CurrentLimit.Message"), LocalizationService.ForSection("Panels.Packages.Remove")("CurrentLimit.Detail"), MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                Else
                    Try
                        For x As Integer = 0 To pkgRemovalCount - 1
                            pkgRemovalNames(x) = CheckedListBox1.CheckedItems(x).ToString()
                        Next
                        For x = 0 To pkgRemovalNames.Length
                            ProgressPanel.pkgRemovalNames(x) = pkgRemovalNames(x)
                        Next
                    Catch ex As Exception

                    End Try
                End If
                ProgressPanel.pkgRemovalLastName = CheckedListBox1.CheckedItems(pkgRemovalCount - 1).ToString()
            End If
        Else
            DynaLog.LogMessage("A selective removal of installed packages will be done.")
            pkgRemovalCount = CheckedListBox2.CheckedItems.Count
            ProgressPanel.pkgRemovalOp = 1
            ProgressPanel.pkgRemovalCount = pkgRemovalCount
            If CheckedListBox2.CheckedItems.Count <= 0 Then
                DynaLog.LogMessage("No items have been added to the queue.")
                MessageBox.Show(MainForm, LocalizationService.ForSection("RemPackage.Validation")("No.Packages.Selected.Message"), LocalizationService.ForSection("RemPackage.Validation")("Packages.Selected.None.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Else
                If pkgRemovalCount > 65535 Then
                    MessageBox.Show(MainForm, LocalizationService.ForSection("Panels.Packages.Remove")("CurrentLimit.SecondMessage"), LocalizationService.ForSection("Panels.Packages.Remove")("CurrentLimit.SecondDetail"), MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                Else
                    Try
                        For x As Integer = 0 To pkgRemovalCount - 1
                            pkgRemovalFiles(x) = CheckedListBox2.CheckedItems(x).ToString()
                        Next
                        For x = 0 To pkgRemovalFiles.Length
                            ProgressPanel.pkgRemovalFiles(x) = pkgRemovalFiles(x)
                        Next
                    Catch ex As Exception

                    End Try
                End If
                ProgressPanel.pkgRemovalLastFile = CheckedListBox2.CheckedItems(pkgRemovalCount - 1).ToString()
            End If
        End If
        ProgressPanel.OperationNum = 27
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Opening package removal dialog...")
        CheckedListBox1.Items.Clear()
        If Not MainForm.CompletedTasks(0) Then
            DynaLog.LogMessage("Package background processes haven't completed.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        DynaLog.LogMessage("Adding packages to arrays...")
        If MainForm.CurrentImage.ImagePackages IsNot Nothing AndAlso MainForm.CurrentImage.ImagePackages.Count > 0 Then
            CheckedListBox1.Items.AddRange(MainForm.CurrentImage.ImagePackages.Where(Function(package) Not New DismPackageFeatureState() {DismPackageFeatureState.NotPresent, DismPackageFeatureState.Removed, DismPackageFeatureState.UninstallPending}.Contains(package.PackageState)).Select(Function(package) package.PackageName).ToArray())
        Else
            CheckedListBox1.Items.AddRange(MainForm.CurrentImage.ImagePackages_Backup.Where(Function(package) Not New DismPackageFeatureState() {DismPackageFeatureState.NotPresent, DismPackageFeatureState.Removed, DismPackageFeatureState.UninstallPending}.Contains(package.PackageState)).Select(Function(package) package.PackageName).ToArray())
        End If
        Return True
    End Function

    Private Sub RemPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("RemPackage")("RemovePackages.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("RemPackage").Format("Image.Task.Header.Label", Text)
        Label3.Text = LocalizationService.ForSection("RemPackage")("PackageSource.Label")
        Label4.Text = LocalizationService.ForSection("RemPackage")("Note.May.Message")
        GroupBox1.Text = LocalizationService.ForSection("RemPackage")("PackageRemoval.Group")
        RadioButton1.Text = LocalizationService.ForSection("RemPackage")("Package.Names.RadioButton")
        RadioButton2.Text = LocalizationService.ForSection("RemPackage")("Package.Files.RadioButton")
        Button1.Text = LocalizationService.ForSection("RemPackage")("Browse.Button")
        Cancel_Button.Text = LocalizationService.ForSection("RemPackage")("Cancel.Button")
        OK_Button.Text = LocalizationService.ForSection("RemPackage")("Ok.Button")
        FolderBrowserDialog1.Description = LocalizationService.ForSection("RemPackage")("PackageSource.Description")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        CheckedListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        CheckedListBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        CheckedListBox1.ForeColor = ForeColor
        CheckedListBox2.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            CheckedListBox1.Enabled = True
            Label3.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
            CheckedListBox2.Enabled = False
            Label4.Enabled = False
        Else
            CheckedListBox1.Enabled = False
            Label3.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
            CheckedListBox2.Enabled = True
            Label4.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog(Me)
        If DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        DynaLog.LogMessage("Scanning folder for possible packages...")
        DynaLog.LogMessage("- Folder to scan: " & Quote & TextBox1.Text & Quote)
        Try
            DynaLog.LogMessage("Getting CAB files in directory (recursive operation)...")
            For Each cabFile In My.Computer.FileSystem.GetFiles(TextBox1.Text, FileIO.SearchOption.SearchAllSubDirectories, "*.cab")
                CheckedListBox2.Items.Add(cabFile)
            Next
        Catch ex As Exception
            Try
                DynaLog.LogMessage("Getting CAB files in directory...")
                For Each cabFile In My.Computer.FileSystem.GetFiles(TextBox1.Text, FileIO.SearchOption.SearchTopLevelOnly, "*.cab")
                    CheckedListBox2.Items.Add(cabFile)
                Next
            Catch ex2 As Exception
                Exit Try    ' Give up
            End Try
        End Try
        If CheckedListBox2.Items.Count <= 0 Then
            DynaLog.LogMessage("There are no items in the selected folder.")
            MsgBox(LocalizationService.ForSection("RemPackage")("Couldn.Tscan.Message"), MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, LocalizationService.ForSection("RemPackage")("DISMTools.Title"))
        End If
    End Sub
End Class
