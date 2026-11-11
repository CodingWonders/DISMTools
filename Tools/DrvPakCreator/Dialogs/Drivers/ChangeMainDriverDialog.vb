Imports System.Windows.Forms
Imports System.IO
Imports DrvPakCreator.Classes.SetupAPI
Imports DrvPakCreator.Helpers.FileIO

Public Class ChangeMainDriverDialog

    Public DriverRootDirectory As String

    Public NewDriverMainFile As String
    Public NewDriverName As String
    Public NewDriverManufacturer As String

    Private InfFiles As New List(Of String)
    Private hwEntries As List(Of InfDriverEntry)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ChangeMainDriverDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.BackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        lbDriverFiles.BackColor = BackColor
        lbDriverFiles.ForeColor = ForeColor
        lvHwTargets.BackColor = BackColor
        lvHwTargets.ForeColor = ForeColor
        tbDriverName.BackColor = BackColor
        tbDriverName.ForeColor = ForeColor
        tbDriverManufacturer.BackColor = BackColor
        tbDriverManufacturer.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        lbDriverFiles.Items.Clear()
        bwDirScanner.RunWorkerAsync()
    End Sub

    Private Sub bwDirScanner_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDirScanner.DoWork
        InfFiles = PathUtilities.GetRecursiveFileList(DriverRootDirectory, "*.inf")
    End Sub

    Private Sub ChangeMainDriverDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If bwDirScanner.IsBusy Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub bwDirScanner_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDirScanner.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            ' TODO handle this case
        End If

        WindowHelper.EnableCloseCapability(Handle)
        Cursor = Cursors.Arrow

        Dim InfFileNames As New List(Of String)
        For Each InfFile As String In InfFiles
            InfFileNames.Add(Path.GetFileName(InfFile))
        Next

        lbDriverFiles.Items.AddRange(InfFileNames.ToArray())
    End Sub

    Private Sub lbDriverFiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbDriverFiles.SelectedIndexChanged
        Try
            If lbDriverFiles.SelectedItems.Count = 1 Then
                Dim drvDirectory As String = Path.GetDirectoryName(InfFiles(lbDriverFiles.SelectedIndex)), _
                    mainFileSubPath As String = ""

                ' The bits that remain in the directory need to be used for the main driver file
                mainFileSubPath = drvDirectory.Replace(DriverRootDirectory, "").TrimStart("\").TrimEnd("\")

                NewDriverMainFile = String.Format("{0}{1}", IIf(mainFileSubPath <> "", String.Format("{0}\", mainFileSubPath), ""), lbDriverFiles.SelectedItem)

                ' begin scanning the INF file for HW targets
                GetHardwareTargets(InfFiles(lbDriverFiles.SelectedIndex))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub GetHardwareTargets(ByVal InfFile As String)
        Dim className As String = "", _
            classGuid As Guid = Guid.Empty

        MessageBox.Show("The selected INF file will be scanned for hardware targets you may be able to choose from. This process will take some time, depending on the size of the INF file and the speed of your computer.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Cursor = Cursors.WaitCursor

        If Not InfParser.GetInfClassInfo(InfFile, className, classGuid) Then
            MessageBox.Show("This INF file appears to be invalid.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        hwEntries = InfParser.ParseInfFile(InfFile)
        If hwEntries.Count = 0 Then
            MessageBox.Show("This INF file appears to be invalid.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        Dim hwTargets As New List(Of ListViewItem)

        For Each entry As InfDriverEntry In hwEntries
            Dim position As UInteger = CUInt(hwEntries.IndexOf(entry) + 1)
            Dim DriverEntry As String = ""
            If entry.HardwareIDs.Count > 0 Then
                DriverEntry = String.Format("{0}. {1} - {2}", position, entry.Description, entry.HardwareIDs(0))
            Else
                DriverEntry = String.Format("{0}. {1}", position, entry.Description)
            End If

            hwTargets.Add(New ListViewItem(New String() {DriverEntry}))
        Next

        Cursor = Cursors.Arrow

        lvHwTargets.Items.Clear()
        lvHwTargets.Items.AddRange(hwTargets.ToArray())
    End Sub

    Private Sub lvHwTargets_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvHwTargets.SelectedIndexChanged
        Try
            If lvHwTargets.SelectedItems.Count = 1 Then
                tbDriverName.Text = hwEntries(lvHwTargets.FocusedItem.Index).Description
                tbDriverManufacturer.Text = hwEntries(lvHwTargets.FocusedItem.Index).Manufacturer
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tbDriverManufacturer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDriverManufacturer.TextChanged
        NewDriverManufacturer = tbDriverManufacturer.Text
    End Sub

    Private Sub tbDriverName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDriverName.TextChanged
        NewDriverName = tbDriverName.Text
    End Sub

    Private Sub rbNoIdChange_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbNoIdChange.CheckedChanged
        pnlDriverReidentification.Enabled = Not rbNoIdChange.Checked

        If rbNoIdChange.Checked Then
            NewDriverName = ""
            NewDriverManufacturer = ""
        Else
            NewDriverName = tbDriverName.Text
            NewDriverManufacturer = tbDriverManufacturer.Text
        End If
    End Sub
End Class
