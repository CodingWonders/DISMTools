Imports System.Windows.Forms

Public Class DriverFilterAssistantDialog

    Public AppliedQuery As String

    Private DriverClassInfoDictionary As New Dictionary(Of String, String) From {
        {"AudioProcessingObject", "Includes Audio processing objects (APOs). For more info, see Windows Audio Processing Objects."},
        {"Battery", "Includes battery devices and UPS devices."},
        {"Biometric", "(Windows Server 2003 and later versions) Includes all biometric-based personal identification devices."},
        {"Bluetooth", "(Windows XP SP1 and later versions) Includes all Bluetooth devices."},
        {"Camera", "(Windows 10 version 1709 and later versions) Includes universal camera drivers."},
        {"CDROM", "Includes CD-ROM drives, including SCSI CD-ROM drives. By default, the system's CD-ROM class installer also installs a system-supplied CD audio driver and CD-ROM changer driver as Plug and Play filters."},
        {"DiskDrive", "Includes hard disk drives. See also the HDC and SCSIAdapter classes."},
        {"Display", "Includes video adapters. Drivers for this class include display drivers and video miniport drivers."},
        {"Extension", "(Windows 10 and later versions) Includes all devices requiring customizations. For more information, see Using an Extension INF File."},
        {"FDC", "Includes floppy disk drive controllers."},
        {"FloppyDisk", "Includes floppy disk drives."},
        {"HDC", "Includes hard disk controllers, including ATA/ATAPI controllers but not SCSI and RAID disk controllers."},
        {"HIDClass", "Includes interactive input devices that are operated by the system-supplied HID class driver. Includes USB devices that comply with the USB HID Standard and non-USB devices that use a HID minidriver. For more information, see HIDClass Device Setup Class. See also the Keyboard or Mouse classes."},
        {"Dot4", "Includes devices that control the operation of multifunction IEEE 1284.4 peripheral devices."},
        {"Dot4Print", "Includes Dot4 print functions. A Dot4 print function is a function on a Dot4 device and has a single child device, which is a member of the Printer device setup class."},
        {"61883", "Includes IEEE 1394 devices that support the IEC-61883 protocol device class. The 61883 component includes the 61883.sys protocol driver that transmits various audio and video data streams over the 1394 bus. These currently include standard/high/low quality DV, MPEG2, DSS, and Audio. The IEC-61883 specifications define these data streams."},
        {"AVC", "Includes IEEE 1394 devices that support the AVC protocol device class."},
        {"SBP2", "Includes IEEE 1394 devices that support the SBP2 protocol device class."},
        {"1394", "Includes 1394 host controllers connected on a PCI bus, but not 1394 peripherals. Drivers for this class are system-supplied."},
        {"Image", "Includes still-image capture devices, digital cameras, and scanners."},
        {"Infrared", "Includes infrared devices. Drivers for this class include Serial-IR and Fast-IR NDIS miniports, but see also the Network Adapter class for other NDIS network adapter miniports."},
        {"Keyboard", "Includes all keyboards. That is, it must also be specified in the (secondary) INF for an enumerated child HID keyboard device."},
        {"MediumChanger", "Includes SCSI media changer devices."},
        {"MTD", "Includes memory devices, such as flash memory cards."},
        {"Modem", "Includes modem devices. An INF file for a device of this class specifies the features and configuration of the device and stores this information in the registry. An INF file for a device of this class can also be used to install device drivers for a controllerless modem or a software modem. These devices split the functionality between the modem device and the device driver. For more information about modem INF files and Microsoft Windows Driver Model (WDM) modem devices, see Overview of Modem INF Files and Adding WDM Modem Support."},
        {"Monitor", "Includes display monitors. An INF for a device of this class installs no device drivers, but instead specifies the features of a particular monitor to be stored in the registry for use by drivers of video adapters. (Monitors are enumerated as the child devices of display adapters.)"},
        {"Mouse", "Includes all mouse devices and other kinds of pointing devices, such as trackballs. That is, this class must also be specified in the (secondary) INF for an enumerated child HID mouse device."},
        {"MultiFunction", "Includes combo cards, such as a PCMCIA modem and network card adapter. The driver for such a Plug and Play multifunction device is installed under this class and enumerates the modem and network card separately as its child devices."},
        {"Media", "Includes Audio and DVD multimedia devices, joystick ports, and full-motion video capture devices."},
        {"MultiPortSerial", "Includes intelligent multiport serial cards, but not peripheral devices that connect to its ports. It doesn't include unintelligent (16550-type) multiport serial controllers or single-port serial controllers (see the Ports class)."},
        {"Net", "Consists of network adapter drivers. These drivers must either call NdisMRegisterMiniportDriver or NetAdapterCreate. Drivers that don't use NDIS or NetAdapter should use a different setup class."},
        {"NetClient", "Includes network and/or print providers. NetClient components are deprecated in Windows 8.1, Windows Server 2012 R2, and later."},
        {"NetService", "Includes network services, such as redirectors and servers."},
        {"NetTrans", "Includes NDIS protocols CoNDIS stand-alone call managers, and CoNDIS clients, in addition to higher level drivers in transport stacks."},
        {"SecurityAccelerator", "Includes devices that accelerate secure socket layer (SSL) cryptographic processing."},
        {"PCMCIA", "Includes PCMCIA and CardBus host controllers, but not PCMCIA or CardBus peripherals. Drivers for this class are system-supplied."},
        {"Ports", "Includes serial and parallel port devices. See also the MultiportSerial class."},
        {"Printer", "Includes printers. As an IT admin, hit them with a baseball bat."},
        {"PnpPrinters", "Includes SCSI/1394-enumerated printers. Drivers for this class provide printer communication for a specific bus."},
        {"Processor", "Includes processor types."},
        {"SCSIAdapter", "Includes SCSI Host Bus Adapters (HBAs), disk-array, and NVMe controllers."},
        {"SecurityDevices", "Includes Trusted Platform Module chips. A TPM is a secure cryptoprocessor that helps you with actions such as generating, storing, and limiting the use of cryptographic keys. Any new manufactured device must implement and enable TPM 2.0 by default. For more information, see TPM Recommendations."},
        {"Sensor", "Includes sensor and location devices, such as GPS devices."},
        {"SmartCardReader", "Includes smart card readers."},
        {"SoftwareComponent", "Includes virtual child device to encapsulate software components. For more information, see Adding Software Components with an INF file."},
        {"Storage", "Storage disks utilizing a multi-queue storage stack."},
        {"Volume", "Includes storage volumes as defined by the system-supplied logical volume manager and class drivers that create device objects to represent storage volumes, such as the system disk class driver."},
        {"System", "Includes HALs, system buses, system bridges, the system ACPI driver, and the system volume manager driver."},
        {"TapeDrive", "Includes tape drives, including all tape miniclass drivers."},
        {"USBDevice", "USBDevice includes all USB devices that don't belong to another class. This class isn't used for USB host controllers and hubs; drivers for these devices are provided by the operating system and should use the USB class described in System-Defined Device Setup Classes Reserved for System Use."},
        {"WCEUSBS", "Includes Windows CE ActiveSync devices. The WCEUSBS setup class supports communication between a personal computer and a device that is compatible with the Windows CE ActiveSync driver (generally, PocketPC devices) over USB."},
        {"WPD", "Includes WPD devices."}
    }

    Private MonthNumberNameDictionary As New Dictionary(Of Integer, String) From {
        {1, "January"},
        {2, "February"},
        {3, "March"},
        {4, "April"},
        {5, "May"},
        {6, "June"},
        {7, "July"},
        {8, "August"},
        {9, "September"},
        {10, "October"},
        {11, "November"},
        {12, "December"}
    }

    Public ProvidedImageClassNames As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If ComboBox1.SelectedIndex < 0 Then Exit Sub

        Select Case ComboBox1.SelectedIndex
            Case 0
                ' Published Name
                AppliedQuery = TextBox1.Text
            Case 1
                ' Original File Name
                AppliedQuery = String.Format("og:{0}", TextBox2.Text)
            Case 2
                ' Provider Name
                AppliedQuery = String.Format("prov:{0}", TextBox3.Text)
            Case 3
                ' Class Name
                If ComboBox2.SelectedItem = "-----------------" Then
                    MessageBox.Show("This class name is not valid.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    Exit Sub
                End If
                AppliedQuery = String.Format("cn:{0}", ComboBox2.SelectedItem)
            Case 4
                ' Inbox Status
                AppliedQuery = If(CheckBox1.Checked, "inbox:", "noinbox:")
            Case 5
                ' Boot-Critical Status
                AppliedQuery = If(CheckBox2.Checked, "bc:", "nobc:")
            Case 6
                ' Signature Status
                AppliedQuery = If(CheckBox3.Checked, "sig:", "nosig:")
            Case 7
                ' Date
                Dim subQuery As String = ""
                Select Case ComboBox3.SelectedIndex
                    Case 0 : subQuery = "eq"
                    Case 1 : subQuery = "ne"
                    Case 2 : subQuery = "lt"
                    Case 3 : subQuery = "le"
                    Case 4 : subQuery = "gt"
                    Case 5 : subQuery = "ge"
                End Select
                If ComboBox3.SelectedIndex < 6 Then
                    Select Case ComboBox4.SelectedIndex
                        Case 0 : subQuery &= String.Format("y-{0}", NumericUpDown1.Value)
                        Case 1 : subQuery &= String.Format("m-{0}", NumericUpDown1.Value)
                        Case 2 : subQuery &= String.Format("-{0}", DateTimePicker1.Value.ToString("dd/MM/yyyy"))
                    End Select
                End If
                AppliedQuery = String.Format("date:{0}", subQuery)
        End Select

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        AppliedQuery = ""
        ' This one does the same thing as the OK button, but after clearing the query.
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub DriverFilterAssistantDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.BackColor = BackColor
        TextBox2.ForeColor = ForeColor
        TextBox3.BackColor = BackColor
        TextBox3.ForeColor = ForeColor
        ComboBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        ComboBox2.BackColor = BackColor
        ComboBox2.ForeColor = ForeColor
        ComboBox3.BackColor = BackColor
        ComboBox3.ForeColor = ForeColor
        ComboBox4.BackColor = BackColor
        ComboBox4.ForeColor = ForeColor
        NumericUpDown1.BackColor = BackColor
        NumericUpDown1.ForeColor = ForeColor
        DateTimePicker1.BackColor = BackColor
        DateTimePicker1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(1))

        ComboBox2.Items.Clear()
        ComboBox2.Items.AddRange(DriverClassInfoDictionary.Keys.ToArray())

        If ComboBox3.SelectedIndex < 0 Then ComboBox3.SelectedIndex = 0
        If ComboBox4.SelectedIndex < 0 Then ComboBox4.SelectedIndex = 0

        If ProvidedImageClassNames.Any() Then
            Dim UniqueImageClassNames As IEnumerable(Of String) = ProvidedImageClassNames.Where(Function(cn) Not DriverClassInfoDictionary.ContainsKey(cn))
            If UniqueImageClassNames.Any() Then
                ComboBox2.Items.Add("-----------------")
                ComboBox2.Items.AddRange(UniqueImageClassNames.Select(Function(cn) cn).ToArray())
            End If
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        PublishedNameFilterPanel.Visible = ComboBox1.SelectedIndex = 0
        OriginalFileNameFilterPanel.Visible = ComboBox1.SelectedIndex = 1
        ProviderNameFilterPanel.Visible = ComboBox1.SelectedIndex = 2
        ClassNameFilterPanel.Visible = ComboBox1.SelectedIndex = 3
        InboxStatusFilterPanel.Visible = ComboBox1.SelectedIndex = 4
        BootCriticalStatusFilterPanel.Visible = ComboBox1.SelectedIndex = 5
        SignatureStatusFilterPanel.Visible = ComboBox1.SelectedIndex = 6
        DateFilterPanel.Visible = ComboBox1.SelectedIndex = 7
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        YearMonthPanel.Enabled = ComboBox4.SelectedIndex < 2
        DatePanel.Enabled = ComboBox4.SelectedIndex = 2
        Label13.Visible = ComboBox4.SelectedIndex = 1

        ' Set limits on the numeric up down thing
        If ComboBox4.SelectedIndex = 0 Then
            NumericUpDown1.Minimum = 1601
            NumericUpDown1.Maximum = Date.Now.Year
            NumericUpDown1.Value = NumericUpDown1.Maximum
        ElseIf ComboBox4.SelectedIndex = 1 Then
            NumericUpDown1.Minimum = 1
            NumericUpDown1.Maximum = 12
            NumericUpDown1.Value = Date.Now.Month
        End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        If ComboBox4.SelectedIndex = 1 Then
            Label13.Text = MonthNumberNameDictionary(NumericUpDown1.Value)
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Label8.Text = DriverClassInfoDictionary.ElementAtOrDefault(ComboBox2.SelectedIndex).Value
    End Sub
End Class
