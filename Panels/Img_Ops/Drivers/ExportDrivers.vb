Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Public Class ExportDrivers

    Private SelectedClass As String

    Private DriverClassInfoDictionary As New Dictionary(Of String, String) From {
        {"AudioProcessingObject", LocalizationService.ForSection("DriverFilter.Classes")("AudioProcessing.Message")},
        {"Battery", LocalizationService.ForSection("DriverFilter.Classes")("Battery.Devices.UPS.Label")},
        {"Biometric", LocalizationService.ForSection("DriverFilter.Classes")("Windows.Message")},
        {"Bluetooth", LocalizationService.ForSection("DriverFilter.Classes")("Windows.Label")},
        {"Camera", LocalizationService.ForSection("DriverFilter.Classes")("Camera.Message")},
        {"CDROM", LocalizationService.ForSection("DriverFilter.Classes")("Cd.Rom.Drives.Message")},
        {"DiskDrive", LocalizationService.ForSection("DriverFilter.Classes")("Hard.Disk.Drives.Label")},
        {"Display", LocalizationService.ForSection("DriverFilter.Classes")("VideoAdapters.Message")},
        {"Extension", LocalizationService.ForSection("DriverFilter.Classes")("Extension.Message")},
        {"FDC", LocalizationService.ForSection("DriverFilter.Classes")("Floppy.Disk.Drive.Label")},
        {"FloppyDisk", LocalizationService.ForSection("DriverFilter.Classes")("Floppy.Disk.Drives.Label")},
        {"HDC", LocalizationService.ForSection("DriverFilter.Classes")("Includes.Hard.Message")},
        {"HIDClass", LocalizationService.ForSection("DriverFilter.Classes")("InputDevices.Message")},
        {"Dot4", LocalizationService.ForSection("DriverFilter.Classes")("ControlDevices.Message")},
        {"Dot4Print", LocalizationService.ForSection("DriverFilter.Classes")("Dot.Print.Functions.Message")},
        {"61883", LocalizationService.ForSection("DriverFilter.Classes")("Ieeedevices.Support.Message")},
        {"AVC", LocalizationService.ForSection("DriverFilter.Classes")("Ieeedevices.Support.Label")},
        {"SBP2", LocalizationService.ForSection("DriverFilter.Classes")("SBP2.Message")},
        {"1394", LocalizationService.ForSection("DriverFilter.Classes")("HostControllers.Message")},
        {"Image", LocalizationService.ForSection("DriverFilter.Classes")("Still.Image.Capture.Label")},
        {"Infrared", LocalizationService.ForSection("DriverFilter.Classes")("InfraredDevices.Message")},
        {"Keyboard", LocalizationService.ForSection("DriverFilter.Classes")("Keyboards.Message")},
        {"MediumChanger", LocalizationService.ForSection("DriverFilter.Classes")("ScsimediaChanger.Label")},
        {"MTD", LocalizationService.ForSection("DriverFilter.Classes")("Memory.Devices.Such.Label")},
        {"Modem", LocalizationService.ForSection("DriverFilter.Classes")("Modem.Devices.INF.Message")},
        {"Monitor", LocalizationService.ForSection("DriverFilter.Classes")("Display.Monitors.INF.Message")},
        {"Mouse", LocalizationService.ForSection("DriverFilter.Classes")("Mouse.Devices.Message")},
        {"MultiFunction", LocalizationService.ForSection("DriverFilter.Classes")("Combo.Cards.Such.Message")},
        {"Media", LocalizationService.ForSection("DriverFilter.Classes")("Audio.Dvdmultimedia.Message")},
        {"MultiPortSerial", LocalizationService.ForSection("DriverFilter.Classes")("MultiportSerial.Message")},
        {"Net", LocalizationService.ForSection("DriverFilter.Classes")("NetworkAdapter.Message")},
        {"NetClient", LocalizationService.ForSection("DriverFilter.Classes")("Includes.Network.Message")},
        {"NetService", LocalizationService.ForSection("DriverFilter.Classes")("Network.Services.Such.Label")},
        {"NetTrans", LocalizationService.ForSection("DriverFilter.Classes")("NdisprotocolsCo.Message")},
        {"SecurityAccelerator", LocalizationService.ForSection("DriverFilter.Classes")("SecureDevices.Message")},
        {"PCMCIA", LocalizationService.ForSection("DriverFilter.Classes")("PcmciacardBus.Message")},
        {"Ports", LocalizationService.ForSection("DriverFilter.Classes")("Serial.Parallel.Port.Message")},
        {"Printer", LocalizationService.ForSection("DriverFilter.Classes")("Printers.Admin.Hit.Label")},
        {"PnpPrinters", LocalizationService.ForSection("DriverFilter.Classes")("Includes.SCSI.Message")},
        {"Processor", LocalizationService.ForSection("DriverFilter.Classes")("ProcessorTypes.Label")},
        {"SCSIAdapter", LocalizationService.ForSection("DriverFilter.Classes")("ScsihostBus.Message")},
        {"SecurityDevices", LocalizationService.ForSection("DriverFilter.Classes")("Includes.Trusted.Message")},
        {"Sensor", LocalizationService.ForSection("DriverFilter.Classes")("Includes.Sensor.Label")},
        {"SmartCardReader", LocalizationService.ForSection("DriverFilter.Classes")("Smart.Card.Readers.Label")},
        {"SoftwareComponent", LocalizationService.ForSection("DriverFilter.Classes")("Virtual.Child.Device.Message")},
        {"Storage", LocalizationService.ForSection("DriverFilter.Classes")("Storage.Disks.Label")},
        {"Volume", LocalizationService.ForSection("DriverFilter.Classes")("Includes.Storage.Message")},
        {"System", LocalizationService.ForSection("DriverFilter.Classes")("HalsSystem.Message")},
        {"TapeDrive", LocalizationService.ForSection("DriverFilter.Classes")("Tape.Drives.Including.Label")},
        {"USBDevice", LocalizationService.ForSection("DriverFilter.Classes")("Usbdevice.Includes.Message")},
        {"WCEUSBS", LocalizationService.ForSection("DriverFilter.Classes")("WindowsCeactive.Message")},
        {"WPD", LocalizationService.ForSection("DriverFilter.Classes")("Wpddevices.Label")}
    }

    Private ProvidedImageClassNames As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Specified driver export target: " & Quote & TextBox1.Text & Quote)
        DynaLog.LogMessage("Checking if directory exists...")
        If TextBox1.Text <> "" And Directory.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("Export target exists.")
            ProgressPanel.drvExportTarget = TextBox1.Text
        Else
            DynaLog.LogMessage("Export target does not exist.")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("ExportDrivers.Validation")("Target.Required.Message")
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If RadioButton2.Checked AndAlso ComboBox1.SelectedItem = "-----------------" Then
            MessageBox.Show(LocalizationService.ForSection("ImageOps.Drivers.Export")("Class.Name.Message"), ImageTaskHeader1.ItemText, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        ProgressPanel.drvExportAllDrvs = RadioButton1.Checked
        ProgressPanel.drvExportSpecificClassName = SelectedClass
        ProgressPanel.OperationNum = 77
        ' Windows 7 behaves differently from Windows 8 and later when getting drivers.
        ProgressPanel.drvExportWin7Mode = MainForm.CurrentImage.ImageVersion.Major = 6 AndAlso MainForm.CurrentImage.ImageVersion.Minor = 1
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ExportDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ExportDrivers")("Title.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("ExportDrivers").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("ExportDrivers")("ExportTarget.Label")
        Button1.Text = LocalizationService.ForSection("ExportDrivers")("Browse.Button")
        OK_Button.Text = LocalizationService.ForSection("ExportDrivers")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ExportDrivers")("Cancel.Button")
        FolderBrowserDialog1.Description = LocalizationService.ForSection("ExportDrivers")("DriversPath.Description")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = ForeColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(DriverClassInfoDictionary.Keys.ToArray())
        ComboBox1.SelectedItem = SelectedClass
        ImageTaskHeader1.HideWindowTitle(handle)

        Try
            Dim ObtainedDrivers As Object = If(MainForm.CurrentImage.ImageDrivers_Backup.Count > MainForm.CurrentImage.ImageDrivers.Count, MainForm.CurrentImage.ImageDrivers_Backup, MainForm.CurrentImage.ImageDrivers)

            If TypeOf ObtainedDrivers Is DismDriverPackageCollection Then
                ProvidedImageClassNames = CType(ObtainedDrivers, DismDriverPackageCollection).Select(Function(driver) driver.ClassName).Distinct().ToList()
            ElseIf TypeOf ObtainedDrivers Is List(Of ImageDriver) Then
                ProvidedImageClassNames = CType(ObtainedDrivers, List(Of ImageDriver)).Select(Function(driver) driver.DriverClassName).Distinct().ToList()
            End If

            If ProvidedImageClassNames.Any() Then
                Dim UniqueImageClassNames As IEnumerable(Of String) = ProvidedImageClassNames.Where(Function(cn) Not DriverClassInfoDictionary.ContainsKey(cn))
                If UniqueImageClassNames.Any() Then
                    ComboBox1.Items.Add("-----------------")
                    ComboBox1.Items.AddRange(UniqueImageClassNames.Select(Function(cn) cn).ToArray())
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Selected path: " & Quote & FolderBrowserDialog1.SelectedPath & Quote)
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        TableLayoutPanel2.Enabled = Not RadioButton1.Checked
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If DriverClassInfoDictionary.ContainsKey(ComboBox1.SelectedItem) Then
            Dim SelectedClassInfo As KeyValuePair(Of String, String) = DriverClassInfoDictionary.ElementAtOrDefault(ComboBox1.SelectedIndex)
            If SelectedClassInfo.Value IsNot Nothing Then
                Label5.Text = SelectedClassInfo.Value
                SelectedClass = SelectedClassInfo.Key
            End If
        Else
            ' We are using a class name that is not in the default set; accept it anyway,
            ' but don't show any notes because we don't know where these are, or whether
            ' they are localized.
            Label5.Text = ""
            SelectedClass = ComboBox1.SelectedItem
        End If
    End Sub
End Class
