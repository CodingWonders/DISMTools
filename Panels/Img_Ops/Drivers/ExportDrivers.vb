Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Public Class ExportDrivers

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
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Please specify a target to export the drivers to and make sure that the specified target exists."
                        Case "ESN"
                            msg = "Especifique un destino al que exportar los controladores y asegúrese de que el destino especificado existe."
                        Case "FRA"
                            msg = "Veuillez spécifier une cible vers laquelle exporter les pilotes et assurez-vous que la cible spécifiée existe."
                        Case "PTB", "PTG"
                            msg = "Especifique um destino para o qual exportar os controladores e certifique-se de que o destino especificado existe."
                        Case "ITA"
                            msg = "Specificare una destinazione in cui esportare i driver e assicurarsi che la destinazione specificata esista"
                    End Select
                Case 1
                    msg = "Please specify a target to export the drivers to and make sure that the specified target exists."
                Case 2
                    msg = "Especifique un destino al que exportar los controladores y asegúrese de que el destino especificado existe."
                Case 3
                    msg = "Veuillez spécifier une cible vers laquelle exporter les pilotes et assurez-vous que la cible spécifiée existe."
                Case 4
                    msg = "Especifique um destino para o qual exportar os controladores e certifique-se de que o destino especificado existe."
                Case 5
                    msg = "Specificare una destinazione in cui esportare i driver e assicurarsi che la destinazione specificata esista"
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If RadioButton2.Checked AndAlso SelectedClassNamesLB.Items.Count < 1 Then
            MessageBox.Show("Please specify class names to export and try again.", ImageTaskHeader1.ItemText, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        If RadioButton2.Checked AndAlso SelectedClassNamesLB.Items.Contains("-----------------") Then
            MessageBox.Show("One or more class names are not valid.", ImageTaskHeader1.ItemText, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        ProgressPanel.drvExportAllDrvs = RadioButton1.Checked
        ProgressPanel.drvExportSpecificClassNames = SelectedClassNamesLB.Items.Cast(Of String)().ToArray()
        ProgressPanel.drvExportOrganizeClassNameExports = CheckBox1.Checked
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
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Export drivers"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Export target:"
                        Button1.Text = "Browse..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        FolderBrowserDialog1.Description = "Please specify the path where the drivers will be exported to:"
                    Case "ESN"
                        Text = "Exportar controladores"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Destino de exportación:"
                        Button1.Text = "Examinar..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        FolderBrowserDialog1.Description = "Especifique la ruta a la que los controladores serán exportados:"
                    Case "FRA"
                        Text = "Exporter les pilotes"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Cible d'exportation :"
                        Button1.Text = "Parcourir..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        FolderBrowserDialog1.Description = "Veuillez indiquer le chemin vers lequel les pilotes seront exportés :"
                    Case "PTB", "PTG"
                        Text = "Controladores de exportação"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Exportar destino:"
                        Button1.Text = "Navegar..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        FolderBrowserDialog1.Description = "Especifique o caminho para onde os controladores serão exportados:"
                    Case "ITA"
                        Text = "Esportazione di driver"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Destinazione di esportazione:"
                        Button1.Text = "Sfoglia..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        FolderBrowserDialog1.Description = "Specificare il percorso in cui verranno esportati i driver:"
                End Select
            Case 1
                Text = "Export drivers"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Export target:"
                Button1.Text = "Browse..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                FolderBrowserDialog1.Description = "Please specify the path where the drivers will be exported to:"
            Case 2
                Text = "Exportar controladores"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Destino de exportación:"
                Button1.Text = "Examinar..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                FolderBrowserDialog1.Description = "Especifique la ruta a la que los controladores serán exportados:"
            Case 3
                Text = "Exporter les pilotes"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Cible d'exportation :"
                Button1.Text = "Parcourir..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                FolderBrowserDialog1.Description = "Veuillez indiquer le chemin vers lequel les pilotes seront exportés :"
            Case 4
                Text = "Controladores de exportação"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Exportar destino:"
                Button1.Text = "Navegar..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                FolderBrowserDialog1.Description = "Especifique o caminho para onde os controladores serão exportados:"
            Case 5
                Text = "Esportazione di driver"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Destinazione di esportazione:"
                Button1.Text = "Sfoglia..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                FolderBrowserDialog1.Description = "Specificare il percorso in cui verranno esportati i driver:"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = ForeColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = ForeColor
        SelectedClassNamesLB.BackColor = CurrentTheme.SectionBackgroundColor
        SelectedClassNamesLB.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(DriverClassInfoDictionary.Keys.ToArray())
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
        SelectedClassNamesLB.Enabled = Not RadioButton1.Checked
        TableLayoutPanel2.Enabled = Not RadioButton1.Checked
        Button2.Enabled = Not RadioButton1.Checked
        Button3.Enabled = Not RadioButton1.Checked
        CheckBox1.Enabled = Not RadioButton1.Checked
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If DriverClassInfoDictionary.ContainsKey(ComboBox1.SelectedItem) Then
            Dim SelectedClassInfo As KeyValuePair(Of String, String) = DriverClassInfoDictionary.ElementAtOrDefault(ComboBox1.SelectedIndex)
            If SelectedClassInfo.Value IsNot Nothing Then Label5.Text = SelectedClassInfo.Value
        Else
            ' We are using a class name that is not in the default set; accept it anyway,
            ' but don't show any notes because we don't know where these are, or whether
            ' they are localized.
            Label5.Text = ""
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DriverClassInfoDictionary.ContainsKey(ComboBox1.SelectedItem) Then
            Dim SelectedClassInfo As KeyValuePair(Of String, String) = DriverClassInfoDictionary.ElementAtOrDefault(ComboBox1.SelectedIndex)
            If SelectedClassInfo.Value IsNot Nothing Then SelectedClassNamesLB.Items.Add(SelectedClassInfo.Key)
        Else
            ' We are using a class name that is not in the default set; accept it anyway,
            ' but don't show any notes because we don't know where these are, or whether
            ' they are localized.
            SelectedClassNamesLB.Items.Add(ComboBox1.SelectedItem)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            SelectedClassNamesLB.Items.Remove(SelectedClassNamesLB.SelectedItem)
        Catch ex As Exception

        End Try
        Button3.Enabled = False
    End Sub

    Private Sub SelectedClassNamesLB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SelectedClassNamesLB.SelectedIndexChanged
        Button3.Enabled = SelectedClassNamesLB.SelectedItems.Count = 1
    End Sub
End Class
