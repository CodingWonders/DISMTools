Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports System.Threading
Imports System.Management
Imports DISMTools.Utilities

Public Class ApplicationDriveSpecifier

    Public SelectedDriveId As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Sub ListDisks()
        DynaLog.LogMessage("Preparing to list disks...")
        ListView1.Items.Clear()
        Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher("SELECT DeviceID, Model, Partitions, Size FROM Win32_DiskDrive")
        Dim dskResults As ManagementObjectCollection = searcher.Get()
        DynaLog.LogMessage("Management object searcher returned " & dskResults.Count & " result(s)")
        ListView1.Items.AddRange(dskResults.Cast(Of ManagementObject)().OrderBy(Function(result) result("DeviceID")).Select(Function(result) New ListViewItem(New String() {result("DeviceID"), result("Model"), result("Partitions"), result("Size") & LocalizationService.ForSection("AppDrive")("Bytes.Label") & Converters.BytesToReadableSize(result("Size")) & ")"})).ToArray())
    End Sub

    Private Sub ApplicationDriveSpecifier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("AppDrive")("Target.Disk.Button")
        Label2.Text = LocalizationService.ForSection("AppDrive")("Destination.Disk.Id.Label")
        Button2.Text = LocalizationService.ForSection("AppDrive")("Refresh.Button")
        OK_Button.Text = LocalizationService.ForSection("AppDrive")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("AppDrive")("Cancel.Button")
        ListView1.Columns(0).Text = LocalizationService.ForSection("AppDrive")("DeviceID.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("AppDrive")("Model.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("AppDrive")("Partitions.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("AppDrive")("Size.Column")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        RichTextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ListDisks()
        BringToFront()

        ColumnHeader1.Width = WindowHelper.ScaleLogical(246)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(347)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(127)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(179)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DynaLog.LogMessage("Refreshing lists...")
        ListDisks()
    End Sub

    Private Sub RichTextBox1_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox1.LinkClicked
        TextBox1.Text = e.LinkText
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        TextBox1.Text = ListView1.FocusedItem.SubItems(0).Text
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        SelectedDriveId = TextBox1.Text
    End Sub

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        If ListView1.SelectedItems.Count = 1 Then
            OK_Button.PerformClick()
        End If
    End Sub
End Class
