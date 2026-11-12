Imports System.Windows.Forms
Imports DISMTools.Elements.DriverPacks

Public Class SelectDriversFromDriverPack

    Public SourceDriverPackFolder As String
    Public SelectedDriverIDs As New List(Of Integer)

    Private DriverMetadataFile As String
    Private DriverPackMetadata As DriverPack

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If lvDriverFiles.CheckedItems.Count = 0 Then
            MessageBox.Show(Me, LocalizationService.ForSection("SelectDriversFromDriverPack")("NoSelectedDriversError"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            For i = 0 To lvDriverFiles.Items.Count - 1
                If lvDriverFiles.Items(i).Checked Then SelectedDriverIDs.Add(i + 1)
            Next
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SelectDriversFromDriverPack_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor

        Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("WndTitle")
        Label1.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("WndHeader")
        Label2.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("ComputerLabel")

        tpHardware.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("TP_Hardware")
        tpSoftware.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("TP_Software")

        ' Hardware Tab
        Label11.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("TP_HW_Processor")
        Label12.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("TP_HW_Memory")
        Label13.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("TP_HW_Storage")
        ' Software Tab
        Label17.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("TP_SW_OSName")
        Label18.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("TP_SW_OSSku")
        Label19.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("TP_SW_OSVer")

        ColumnHeader1.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("LV_CH_DriverName")
        ColumnHeader2.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("LV_CH_DriverMainFile")
        ColumnHeader1.Width = WindowHelper.ScaleLogical(256)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(256)

        OK_Button.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("OK_Button")
        Cancel_Button.Text = LocalizationService.ForSection("SelectDriversFromDriverPack")("Cancel_Button")

        tpHardware.BackColor = BackColor
        tpHardware.ForeColor = ForeColor
        tpSoftware.BackColor = BackColor
        tpSoftware.ForeColor = ForeColor

        tbSysProcessor.BackColor = BackColor
        tbSysProcessor.ForeColor = ForeColor
        nudMemory.BackColor = BackColor
        nudMemory.ForeColor = ForeColor
        nudStorage.BackColor = BackColor
        nudStorage.ForeColor = ForeColor
        tbSysOSName.BackColor = BackColor
        tbSysOSName.ForeColor = ForeColor
        tbSysOSSku.BackColor = BackColor
        tbSysOSSku.ForeColor = ForeColor
        tbSysOSVer.BackColor = BackColor
        tbSysOSVer.ForeColor = ForeColor

        lvDriverFiles.BackColor = BackColor
        lvDriverFiles.ForeColor = ForeColor
        comboComputerSelect.BackColor = BackColor
        comboComputerSelect.ForeColor = ForeColor

        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        Try
            DriverMetadataFile = Path.Combine(SourceDriverPackFolder, "DRV_METADATA.XML")
            DriverPackMetadata = DriverPackHelper.DeserializeDriverPackMetadata(DriverMetadataFile)

            If DriverPackMetadata Is Nothing Then Exit Sub

            SelectedDriverIDs.Clear()
            lvDriverFiles.Items.Clear()
            lvDriverFiles.Items.AddRange(DriverPackMetadata.DriverFiles.Drivers.Select(Function(drv) New ListViewItem(New String() {drv.Name, Path.GetFileName(drv.MainFile)})).ToArray())
            comboComputerSelect.Items.Clear()
            comboComputerSelect.Items.Add(LocalizationService.ForSection("SelectDriversFromDriverPack")("LV_ManualDriverSelect"))
            comboComputerSelect.Items.AddRange(DriverPackMetadata.Computers.Computers.Select(Function(comp) String.Format("{0} {1}", comp.ComputerInformation.Manufacturer, comp.ComputerInformation.Model)).ToArray())
        Catch ex As Exception

        End Try
    End Sub

    Private Sub comboComputerSelect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles comboComputerSelect.SelectedIndexChanged
        If comboComputerSelect.SelectedIndex > 0 Then
            ' grab info from metadata
            Dim ComputerInformation As ComputerInfo = DriverPackMetadata.Computers.Computers(comboComputerSelect.SelectedIndex - 1).ComputerInformation,
                AssociatedDriverIds As List(Of Integer) = DriverPackMetadata.Computers.Computers(comboComputerSelect.SelectedIndex - 1).AssociatedDriverIds

            tbSysProcessor.Text = ComputerInformation.Processor
            nudMemory.Value = ComputerInformation.MemoryCapacity
            nudStorage.Value = ComputerInformation.StorageCapacity
            tbSysOSName.Text = ComputerInformation.OperatingSystem.Name
            tbSysOSSku.Text = ComputerInformation.OperatingSystem.SKU
            tbSysOSVer.Text = ComputerInformation.OperatingSystem.VersionString

            ' begin selecting driver files
            WindowHelper.UncheckAllItems(lvDriverFiles)
            If DriverPackMetadata.DriverFiles.Drivers.Any() Then
                For i = 0 To lvDriverFiles.Items.Count - 1
                    If AssociatedDriverIds.Contains(i + 1) Then lvDriverFiles.Items(i).Checked = True
                Next
            End If
        Else
            tbSysProcessor.Text = ""
            nudMemory.Value = nudMemory.Minimum
            nudStorage.Value = nudStorage.Minimum
            tbSysOSName.Text = ""
            tbSysOSSku.Text = ""
            tbSysOSVer.Text = ""
            WindowHelper.UncheckAllItems(lvDriverFiles)
        End If
    End Sub
End Class
