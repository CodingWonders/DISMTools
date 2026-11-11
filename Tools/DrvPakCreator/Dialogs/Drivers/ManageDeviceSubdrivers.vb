Imports System.Windows.Forms
Imports DrvPakCreator.Helpers.FileIO

Public Class ManageDeviceSubdrivers

    Public SelectedSubdrivers As List(Of String)
    Public DriverRootDirectory As String
    Public DriverMainFile As String

    Private InfFiles As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        SelectedSubdrivers.Clear()
        For Each SelectedSubdriverItem As ListViewItem In lvDriverFiles.CheckedItems
            SelectedSubdrivers.Add(SelectedSubdriverItem.Text)
        Next
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ManageDeviceSubdrivers_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If bwDirScanner.IsBusy Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub ManageDeviceSubdrivers_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.BackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        lvDriverFiles.BackColor = BackColor
        lvDriverFiles.ForeColor = ForeColor
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        lvDriverFiles.Items.Clear()
        bwDirScanner.RunWorkerAsync()
    End Sub

    Private Sub bwDirScanner_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDirScanner.DoWork
        InfFiles = PathUtilities.GetRecursiveFileList(DriverRootDirectory, "*.inf")
    End Sub

    Private Sub bwDirScanner_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDirScanner.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            ' TODO handle this case
        End If

        WindowHelper.EnableCloseCapability(Handle)
        Cursor = Cursors.Arrow


        Dim InfFileEntries As New List(Of ListViewItem)
        For Each InfFile As String In InfFiles
            InfFile = InfFile.Replace(DriverRootDirectory, "").TrimStart("\")

            ' if it is the main file then don't show it
            If InfFile = DriverMainFile Then Continue For

            Dim lvi As New ListViewItem(New String() {InfFile})
            lvi.Checked = SelectedSubdrivers.Contains(InfFile)
            InfFileEntries.Add(lvi)
        Next

        lvDriverFiles.Items.AddRange(InfFileEntries.ToArray())
    End Sub
End Class
