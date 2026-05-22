Imports System.Windows.Forms

Public Class FfuInfoDialog

    Public MountedFfuInformation As FullFlashUtilityImage

    Private FfuPartitions As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub FfuInfoDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TabPage1.BackColor = BackColor
        TabPage1.ForeColor = ForeColor
        TabPage2.BackColor = BackColor
        TabPage2.ForeColor = ForeColor
        TabPage3.BackColor = BackColor
        TabPage3.ForeColor = ForeColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.BackColor = BackColor
        TextBox2.ForeColor = ForeColor
        RichTextBox1.BackColor = BackColor
        RichTextBox1.ForeColor = ForeColor
        RichTextBox2.BackColor = BackColor
        RichTextBox2.ForeColor = ForeColor
        ListBox1.BackColor = BackColor
        ListBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Show the information
        TextBox1.Text = MountedFfuInformation.VhdPath
        TextBox2.Text = MountedFfuInformation.VhdId
        Label4.Text = MountedFfuInformation.VhdStorageDeviceId
        Label6.Text = MountedFfuInformation.MountDiskPath
        Label8.Text = MountedFfuInformation.FullFlashVersionInfo.ToString()
        Label10.Text = MountedFfuInformation.CompressionToString()

        ListBox1.Items.Clear()
        FfuPartitions.Clear()
        Dim FfuPartitionsMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("ASSOCIATORS OF {{Win32_DiskDrive.DeviceID={0}{1}{0}}} WHERE RESULTCLASS = Win32_DiskPartition", Quote, WMIHelper.GetEscapedValue(MountedFfuInformation.MountDiskPath))),
            FfuDriveMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT * FROM Win32_DiskDrive WHERE DeviceID = {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(MountedFfuInformation.MountDiskPath)))

        ' We don't really want to flood logs here...
        DynaLog.DisableLogging()
        If FfuPartitionsMOC IsNot Nothing Then
            For Each FfuPartitionsMO As ManagementObject In FfuPartitionsMOC
                FfuPartitions.Add(WMIHelper.GetObjectValue(FfuPartitionsMO, "DeviceID"))
            Next
        End If
        ListBox1.Items.AddRange(FfuPartitions.ToArray())
        Label13.Text = String.Format("Disk size: {1}{0}" &
                                     "Number of partitions: {2}", Environment.NewLine,
                                                                  If(FfuDriveMOC IsNot Nothing, Converters.BytesToReadableSize(WMIHelper.GetObjectValue(FfuDriveMOC(0), "Size")), "Unknown"),
                                                                  FfuPartitions.Count)

        RichTextBox2.Text = MountedFfuInformation.IniManifest
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim selectedPartition As String = FfuPartitions.ElementAtOrDefault(ListBox1.SelectedIndex)

        If selectedPartition <> "" Then
            Dim partDetailsMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT * FROM Win32_DiskPartition WHERE DeviceID = {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(selectedPartition)))

            If partDetailsMOC IsNot Nothing AndAlso partDetailsMOC.Count > 0 Then
                Dim partDetailsMO As ManagementObject = partDetailsMOC(0)

                Dim PartitionDetailProperties As Dictionary(Of String, Object) = WMIHelper.GetObjectValues(partDetailsMO,
                                                                                                           "DiskIndex", "Index", "PrimaryPartition",
                                                                                                           "BootPartition", "Bootable", "Size",
                                                                                                           "StartingOffset", "NumberOfBlocks", "BlockSize")

                RichTextBox1.Text = String.Format("Disk Index: {1}{0}" &
                                                  "Partition Index: {2}{0}" &
                                                  "Primary Partition? {3}{0}" &
                                                  "Boot Partition? {4}{0}" &
                                                  "Bootable? {5}{0}" &
                                                  "Size: {6} bytes (~{7}){0}" &
                                                  "Offset: {8}{0}" &
                                                  "Number of Blocks: {9}{0}" &
                                                  "Block Size: {10} bytes", Environment.NewLine,
                                                                            PartitionDetailProperties("DiskIndex"),
                                                                            PartitionDetailProperties("Index"),
                                                                            PartitionDetailProperties("PrimaryPartition"),
                                                                            PartitionDetailProperties("BootPartition"),
                                                                            PartitionDetailProperties("Bootable"),
                                                                            PartitionDetailProperties("Size"),
                                                                            Converters.BytesToReadableSize(PartitionDetailProperties("Size")),
                                                                            PartitionDetailProperties("StartingOffset"),
                                                                            PartitionDetailProperties("NumberOfBlocks"),
                                                                            PartitionDetailProperties("BlockSize"))
            End If
        End If
    End Sub

    Private Sub FfuInfoDialog_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        DynaLog.EnableLogging()
    End Sub
End Class
