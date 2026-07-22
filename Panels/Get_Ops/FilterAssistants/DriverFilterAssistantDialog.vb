Imports System.Windows.Forms

Public Class DriverFilterAssistantDialog

    Public AppliedQuery As String

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

    Private MonthNumberNameDictionary As New Dictionary(Of Integer, String) From {
        {1, LocalizationService.ForSection("DriverFilter.Month")("January.Label")},
        {2, LocalizationService.ForSection("DriverFilter.Month")("February.Label")},
        {3, LocalizationService.ForSection("DriverFilter.Month")("March.Label")},
        {4, LocalizationService.ForSection("DriverFilter.Month")("April.Label")},
        {5, LocalizationService.ForSection("DriverFilter.Month")("Value.Label")},
        {6, LocalizationService.ForSection("DriverFilter.Month")("June.Label")},
        {7, LocalizationService.ForSection("DriverFilter.Month")("July.Label")},
        {8, LocalizationService.ForSection("DriverFilter.Month")("August.Label")},
        {9, LocalizationService.ForSection("DriverFilter.Month")("September.Label")},
        {10, LocalizationService.ForSection("DriverFilter.Month")("October.Label")},
        {11, LocalizationService.ForSection("DriverFilter.Month")("November.Label")},
        {12, LocalizationService.ForSection("DriverFilter.Month")("December.Label")}
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
                    MessageBox.Show(LocalizationService.ForSection("DriverFilter.Messages")("Class.Name.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
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
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

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
