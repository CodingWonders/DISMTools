Imports System.Windows.Forms
Imports System.IO
Imports DISMTools.Utilities

Public Class OfflineInstDriveLister

    Dim DIList As New List(Of DriveInfo)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        MainForm.MountDir = ListView1.FocusedItem.SubItems(0).Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OfflineInstDriveLister_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().ToList()
        For Each DI As DriveInfo In DIList
            If DI.IsReady Then
                ListView1.Items.Add(New ListViewItem(New String() {DI.Name, DI.VolumeLabel, Casters.CastDriveType(DI.DriveType, True), Converters.BytesToReadableSize(DI.TotalSize), Converters.BytesToReadableSize(DI.AvailableFreeSpace), DI.DriveFormat, If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"), If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")}))
            End If
        Next
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().ToList()
        For Each DI As DriveInfo In DIList
            If DI.IsReady Then
                ListView1.Items.Add(New ListViewItem(New String() {DI.Name, DI.VolumeLabel, Casters.CastDriveType(DI.DriveType, True), Converters.BytesToReadableSize(DI.TotalSize), Converters.BytesToReadableSize(DI.AvailableFreeSpace), DI.DriveFormat, If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"), If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")}))
            End If
        Next
    End Sub
End Class
