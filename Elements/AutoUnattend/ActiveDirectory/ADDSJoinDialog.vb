Imports System.Threading
Imports System.Net.NetworkInformation
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.RegularExpressions
Imports System.DirectoryServices.AccountManagement
Imports Tulpep.ActiveDirectoryObjectPicker

Public Class ADDSJoinDialog

    Private dsIsInDomain As Boolean
    Private dsDomainName As String = ""
    Private netAdapters As NetworkInterface()
    Private dnsMappings As New Dictionary(Of String, DnsProperties)
    Private dnsAliasName As String = ""

    Private Enum WizardPage
        DnsConfigPage = 0
        DsConfigPage = 1
    End Enum

    Private PageCount As Integer = 2
    Private CurrentWizardPage As WizardPage
    Dim VerifyInPages As New List(Of WizardPage)

    Private Class DnsProperties
        Public Property Suffix As String
        Public Property Addresses As IPAddressCollection

        Public Sub New(suffix As String, addresses As IPAddressCollection)
            Me.Suffix = suffix
            Me.Addresses = addresses
        End Sub
    End Class

    Private Class DnsInformation
        Public Property Suffix As String
        Public Property NicAlias As String
        Public Property DnsAddresses As String()

        Public Sub New(suffix As String, interfaceAlias As String, addresses As String())
            Me.Suffix = suffix
            Me.NicAlias = interfaceAlias
            Me.DnsAddresses = addresses
        End Sub
    End Class

    Private Class DomainInformation
        Public Property DomainName As String
        Public Property DomainUser As String
        Public Property DomainPassword As String

        Public Sub New(name As String, user As String, password As String)
            Me.DomainName = name
            Me.DomainUser = user
            Me.DomainPassword = password
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("Domain: {0}. Log on as user {1} with password {2}", DomainName, DomainUser, New String("*", DomainPassword.Length))
        End Function
    End Class

    Private dnsInfo As DnsInformation
    Private dsInfo As DomainInformation

    Private dnsAddresses As String()

    Private NtLogonPathStart As String = ""

    Private userMappings As Dictionary(Of String, List(Of Principal))
    Private initialUserName As String = ""

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Private Sub ADDSJoinDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = BackColor
        ComboBox2.BackColor = BackColor
        ComboBox3.BackColor = BackColor
        ComboBox4.BackColor = BackColor
        TextBox1.BackColor = BackColor
        TextBox2.BackColor = BackColor
        TextBox3.BackColor = BackColor
        TextBox4.BackColor = BackColor
        TextBox5.BackColor = BackColor
        TextBox6.BackColor = BackColor
        TextBox7.BackColor = BackColor
        GroupBox1.BackColor = BackColor
        RichTextBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        ComboBox2.ForeColor = ForeColor
        ComboBox3.ForeColor = ForeColor
        ComboBox4.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        TextBox5.ForeColor = ForeColor
        TextBox6.ForeColor = ForeColor
        TextBox7.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        VerifyInPages.AddRange(New WizardPage() {WizardPage.DnsConfigPage, WizardPage.DsConfigPage})
        ChangePage(WizardPage.DnsConfigPage)
        dnsMappings.Clear()
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        ComboBox3.Items.Clear()
        Visible = True
        ADDSInitBW.RunWorkerAsync()
        Do Until Not ADDSInitBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        Loop
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        ComboBox1.Enabled = RadioButton1.Checked
        TextBox2.Enabled = RadioButton2.Checked
    End Sub

    Private Sub InitializeWizard()
        ProgressReporter.SetMessage("Checking if device is part of a domain...")
        ADDSInitBW.ReportProgress(5)
        dsIsInDomain = DomainServicesModule.DSIsInDomain()
        If dsIsInDomain Then
            ProgressReporter.SetMessage("Getting name of assigned domain...")
            ADDSInitBW.ReportProgress(20)
            dsDomainName = DomainServicesModule.DSGetDomainName()
        End If
        ProgressReporter.SetMessage("Getting network adapters...")
        ADDSInitBW.ReportProgress(40)
        GetNetworkAdapters()
        ProgressReporter.SetMessage("Getting DNS addresses for each network adapter...")
        ADDSInitBW.ReportProgress(60)
        GetDnsAddresses()
        ProgressReporter.SetMessage("Getting primary domain controller NetBIOS name...")
        ADDSInitBW.ReportProgress(80)
        NtLogonPathStart = DomainServicesModule.DSGetDomainControllerNetBIOSName()
        If dsIsInDomain Then
            ComboBox4.SelectedIndex = 1
            ProgressReporter.SetMessage("Mapping organizational units and users...")
            ADDSInitBW.ReportProgress(90)
            userMappings = DomainServicesModule.DSMapOrganizationalUnitsAndUsers(dsDomainName, NtLogonPathStart)
            If userMappings IsNot Nothing Then
                ComboBox2.Items.AddRange(userMappings.Keys.OrderBy(Function(ou) ou).ToArray())
                ComboBox2.SelectedIndex = 0
            End If
        Else
            ComboBox4.SelectedIndex = 0
            If ComboBox4.Items.Count > 1 Then
                ComboBox4.Items.RemoveAt(1)
                ComboBox4.Items.RemoveAt(2)
            End If
        End If
        DsAccountObjectPickerBtn.Enabled = dsIsInDomain
        ProgressReporter.SetMessage("Initialization complete.")
        ADDSInitBW.ReportProgress(100)
    End Sub

    Private Sub GetNetworkAdapters()
        netAdapters = NetworkInterface.GetAllNetworkInterfaces()
    End Sub

    Private Sub GetDnsAddresses()
        If netAdapters.Count > 0 Then
            For Each netAdapter As NetworkInterface In netAdapters.Where(Function(adapter) adapter.GetIPProperties().IsDnsEnabled Or adapter.GetIPProperties().IsDynamicDnsEnabled)
                dnsMappings.Add(netAdapter.Name, New DnsProperties(netAdapter.GetIPProperties().DnsSuffix, netAdapter.GetIPProperties().DnsAddresses))
            Next
        End If
    End Sub

    Private Sub ADDSInitBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ADDSInitBW.DoWork
        InitializeWizard()
    End Sub

    Private Sub ADDSInitBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles ADDSInitBW.ProgressChanged
        ProgressReporter.ReportProgress(Me, e.ProgressPercentage)
    End Sub

    Private Sub ADDSInitBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ADDSInitBW.RunWorkerCompleted
        For Each mapping In dnsMappings
            ComboBox1.Items.Add(mapping.Key)
        Next
        ProgressReporter.Hide()
        DSNoDomainPanel.Visible = Not dsIsInDomain
        If dsIsInDomain Then
            RemoveHandler TextBox4.TextChanged, AddressOf TextBox4_TextChanged
            TextBox4.Text = dsDomainName
            AddHandler TextBox4.TextChanged, AddressOf TextBox4_TextChanged
            If NtLogonPathStart = "" Then
                NtLogonPathStart = "Primary DC NetBIOS"
            End If
        Else
            NtLogonPathStart = "Primary DC NetBIOS"
        End If
        If ComboBox3.SelectedIndex < 0 Then AddsNtLogonPathText.Text = String.Format("{0}\", NtLogonPathStart)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            Dim dnsConfig As DnsProperties = dnsMappings(ComboBox1.SelectedItem)
            TextBox1.Text = dnsConfig.Suffix
            RichTextBox1.Text = ""
            If dnsConfig.Addresses.Count > 0 Then
                For Each Address In dnsConfig.Addresses
                    RichTextBox1.AppendText(Address.ToString() & CrLf)
                Next
            End If
            dnsAliasName = ComboBox1.SelectedItem
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        TextBox3.Text = TextBox1.Text
    End Sub

    Private Sub ChangePage(NewPage As WizardPage)
        DynaLog.LogMessage("Changing current page of the wizard...")
        DynaLog.LogMessage("New page to load: " & NewPage.ToString())
        If NewPage > CurrentWizardPage AndAlso VerifyInPages.Contains(CurrentWizardPage) Then
            If Not VerifyOptionsInPage(CurrentWizardPage) Then Exit Sub
        End If

        ' Apply settings in the DNS class if in DNS page
        If CurrentWizardPage = WizardPage.DnsConfigPage Then
            dnsInfo = New DnsInformation(TextBox1.Text, dnsAliasName, RichTextBox1.Lines)
        End If

        DNSConfigPanel.Visible = (NewPage = WizardPage.DnsConfigPage)
        DSDomainConfigPanel.Visible = (NewPage = WizardPage.DsConfigPage)

        CurrentWizardPage = NewPage
        Back_Button.Enabled = Not (NewPage = WizardPage.DnsConfigPage)

        Next_Button.Text = If(NewPage = WizardPage.DsConfigPage, "Finish", "Next")

        DNS_Explanation_Link.Visible = (NewPage = WizardPage.DnsConfigPage)
    End Sub

    Private Function VerifyOptionsInPage(page As WizardPage) As Boolean
        DynaLog.LogMessage("Verifying user options before moving on to next page...")
        DynaLog.LogMessage("Page in which we need to verify user settings: " & page.ToString())
        Select Case page
            Case WizardPage.DnsConfigPage
                If TextBox1.Text = "" Then
                    MsgBox("A primary domain suffix must be provided for DNS", vbOKOnly + vbCritical)
                    Return False
                End If
                If dnsAliasName = "" Then
                    MsgBox("An interface alias must be provided for DNS. These are the names of the network adapters installed on your system", vbOKOnly + vbCritical)
                    Return False
                End If
                If RichTextBox1.Text = "" Then
                    MsgBox("No DNS server addresses have been provided", vbOKOnly + vbCritical)
                    Return False
                End If
            Case WizardPage.DsConfigPage
                If TextBox4.Text = "" Then
                    MsgBox("A domain name must be specified", vbOKOnly + vbCritical)
                    Return False
                End If
                If initialUserName = "" Then
                    MsgBox("A user name must be specified", vbOKOnly + vbCritical)
                    Return False
                End If
                If TextBox6.Text = "" Then
                    Try
                        If DomainServicesModule.DSAccountRequiresPassword(dsDomainName, initialUserName) Then
                            MsgBox(String.Format("A password for the specified user, {0}{1}{0}, must be specified as per security policies imposed by the domain controller.", Quote, initialUserName), vbOKOnly + vbCritical)
                            Return False
                        End If
                    Catch ex As Exception
                        MsgBox(String.Format("A password for the specified user, {0}{1}{0}, must be specified as per security policies imposed by the domain controller.", Quote, initialUserName), vbOKOnly + vbCritical)
                        Return False
                    End Try
                End If
                If dsIsInDomain AndAlso Not DomainServicesModule.DSAccountExists(dsDomainName, initialUserName) Then
                    If MsgBox(String.Format("The specified user, {1}, does not appear to exist in the provided domain. You may not be able to sign in with this user unless you create it first.{0}{0}" &
                                            "Do you want to continue?", Environment.NewLine, initialUserName), vbYesNo + vbExclamation, Text) = MsgBoxResult.No Then
                        Return False
                    End If
                End If
                Return MsgBox("Please verify the information that you typed. If you incorrectly typed a field, the client device may not join the domain." & CrLf & CrLf & "The client device will also not join the domain if it will run home editions of Windows." & CrLf & CrLf & "Are you sure that these settings are correct?", vbYesNo + vbQuestion, "Verify Settings") = MsgBoxResult.Yes
        End Select
        Return True
    End Function

    Private Sub Back_Button_Click(sender As Object, e As EventArgs) Handles Back_Button.Click
        ChangePage(CurrentWizardPage - 1)
    End Sub

    Private Sub Next_Button_Click(sender As Object, e As EventArgs) Handles Next_Button.Click
        If CurrentWizardPage = WizardPage.DsConfigPage Then
            If Not VerifyOptionsInPage(WizardPage.DsConfigPage) Then
                DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            Else
                dsInfo = New DomainInformation(TextBox4.Text, initialUserName, TextBox6.Text)
            End If
            If ApplyDsSettings() Then
                MsgBox("Domain settings were added successfully to the answer file. You can further modify these components in the System components section.")
                SetDefaultSettings()
                Close()
            Else
                MsgBox("Could not add domain settings.")
            End If
        Else
            ChangePage(CurrentWizardPage + 1)
            DialogResult = Windows.Forms.DialogResult.None
        End If
    End Sub

    Private Function ApplyDsSettings() As Boolean
        DynaLog.LogMessage("Applying settings for Domain Services...")
        Try
            DynaLog.LogMessage("Specified Domain Name System (DNS) information:" & CrLf &
                               "- Primary DNS Suffix: " & dnsInfo.Suffix & CrLf &
                               "- NIC (Network Adapter) Alias: " & dnsInfo.NicAlias & CrLf &
                               "- Addresses: " & String.Join(", ", dnsInfo.DnsAddresses.Where(Function(address) Not String.IsNullOrEmpty(address)).ToArray()))
            DynaLog.LogMessage("Specified DS information: " & dsInfo.ToString())
            Dim dnsServerSearchOrder As String = "        <DNSServerSearchOrder>" & CrLf
            For Each Address In dnsInfo.DnsAddresses
                If String.IsNullOrEmpty(Address) Then Continue For
                dnsServerSearchOrder &= String.Format("            <IpAddress wcm:action=" & Quote & "add" & Quote & " wcm:keyValue=" & Quote & "{0}" & Quote & ">{1}</IpAddress>" & CrLf,
                                                      dnsInfo.DnsAddresses.ToList().IndexOf(Address) + 1,
                                                      Address)
            Next
            dnsServerSearchOrder &= "        </DNSServerSearchOrder>"
            Dim dnsXml As String = String.Format("<DNSDomain>{0}</DNSDomain>" & CrLf &
                                                 "<DNSSuffixSearchOrder>" & CrLf &
                                                 "    <DomainName wcm:action=" & Quote & "add" & Quote & " wcm:keyValue=" & Quote & "1" & Quote & ">{0}</DomainName>" & CrLf &
                                                 "    <!-- Copy the line above this one, increase the value in wcm:keyValue by 1, and put a suffix name if you want more suffixes -->" & CrLf &
                                                 "</DNSSuffixSearchOrder>" & CrLf &
                                                 "<UseDomainNameDevolution>true</UseDomainNameDevolution>" & CrLf &
                                                 "<Interfaces>" & CrLf &
                                                 "    <Interface wcm:action=" & Quote & "add" & Quote & ">" & CrLf &
                                                 "        <Identifier>{1}</Identifier>" & CrLf &
                                                 "        <DNSDomain>{0}</DNSDomain>" & CrLf &
                                                 "{2}" & CrLf &
                                                 "        <EnableAdapterDomainNameRegistration>true</EnableAdapterDomainNameRegistration>" & CrLf &
                                                 "        <DisableDynamicUpdate>false</DisableDynamicUpdate>" & CrLf &
                                                 "    </Interface>" & CrLf &
                                                 "    <!-- Copy the whole Interface block above this line and configure it if you want to add more network adapters for which you want to set up DNS -->" & CrLf &
                                                 "</Interfaces>", dnsInfo.Suffix, dnsInfo.NicAlias, dnsServerSearchOrder)
            Dim dsXml As String = String.Format("<Identification>" & CrLf &
                                                "    <JoinDomain>{0}</JoinDomain>" & CrLf &
                                                "    <Credentials>" & CrLf &
                                                "        <Domain>{0}</Domain>" & CrLf &
                                                "        <Username>{1}</Username>" & CrLf &
                                                "        <Password>{2}</Password>" & CrLf &
                                                "    </Credentials>" & CrLf &
                                                "</Identification>", dsInfo.DomainName, dsInfo.DomainUser, dsInfo.DomainPassword)
            DynaLog.LogMessage("Adding components...")
            NewUnattendWiz.AddComponent("Microsoft-Windows-DNS-Client", "specialize", dnsXml)
            NewUnattendWiz.AddComponent("Microsoft-Windows-UnattendedJoin", "specialize", dsXml)
            Return True
        Catch ex As Exception
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
            Return False
        End Try
    End Function

    Sub SetDefaultSettings()
        DynaLog.LogMessage("Setting default configuration...")
        DynaLog.LogMessage("Setting default DNS settings...")
        TextBox1.Text = ""
        TextBox2.Text = ""
        RichTextBox1.Clear()
        RadioButton1.Checked = True
        DynaLog.LogMessage("Setting default domain settings...")
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        dnsAliasName = TextBox2.Text
    End Sub

    Private Sub DNS_Explanation_Link_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles DNS_Explanation_Link.LinkClicked
        MsgBox("DNS (short for Domain Name System) is a server role that automatically translates IP addresses to human-readable names." & CrLf & CrLf &
               "When you use this wizard, DISMTools assumes that either you or your system administrator have set up DNS on your network. If not, cancel this wizard and set it up.",
               vbOKOnly + vbInformation)
    End Sub

    Private Sub DnsSyntaxCheckerBtn_Click(sender As Object, e As EventArgs) Handles DnsSyntaxCheckerBtn.Click
        If DnsValidatorBW.IsBusy Then
            Exit Sub
        End If
        DnsSyntaxCheckerBtn.Enabled = False
        dnsAddresses = RichTextBox1.Lines.Where(Function(address) Not String.IsNullOrEmpty(address)).ToArray()
        DnsValidatorBW.RunWorkerAsync()
    End Sub

    Private Sub DnsValidatorBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles DnsValidatorBW.DoWork
        If dnsAddresses Is Nothing Then
            Throw New Exception("Please provide DNS addresses to test")
        End If
        ProgressReporter.SetMessage("Checking DNS addresses...")
        DnsValidatorBW.ReportProgress(0)
        Dim dnsAddressValidationInfo As String = ""
        Dim current As Integer = 1
        Dim total As Integer = dnsAddresses.Count

        ' Variables used for statistics
        Dim InvalidAddresses As Integer = 0,
            IPv4Addresses As Integer = 0,
            GlobalIPv6Addresses As Integer = 0,
            LinkLocalIPv6Addresses As Integer = 0,
            UniqueLocalIPv6Addresses As Integer = 0
        ' Ratio of valid to invalid. Ratio is calculated using the following formula: 
        ' ((IPv4 + IPv6 + Link-Local IPv6 + Unique Local IPv6) / Total Address Count) * 100
        ' Any site-local addresses will be treated as invalid for compatibility reasons with operating systems.
        Dim ValidToInvalidAddressRatio As Double = 0.0
        Dim InvalidAddressList As New List(Of String)       ' We'll have this to explain why the addresses are invalid to the user

        For Each dnsAddress In dnsAddresses
            ' Start testing them with regex patterns to determine if they are valid IPv4 or IPv6 addresses
            DynaLog.LogMessage(String.Format("Checking address {0} of {1}...", current, total))
            ProgressReporter.SetMessage(String.Format("Verifying syntax of DNS address {0} of {1}...", current, total))
            DnsValidatorBW.ReportProgress(((current - 1) / total) * 100)

            If dnsAddress.Contains("%") Then    ' Typical for scoped IPv6
                Try
                    Dim percentLocation As Integer = InStr(dnsAddress, "%") - 1
                    dnsAddress = dnsAddress.Remove(percentLocation, dnsAddress.Count - percentLocation)
                Catch ex As Exception
                    ' Don't do anything then
                End Try
            End If

            If Regex.IsMatch(dnsAddress, "^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$") Then    ' First, let's check IPv4
                DynaLog.LogMessage("This is an IPv4 address.")
                IPv4Addresses += 1
            ElseIf Regex.IsMatch(dnsAddress, "^((?:[0-9A-Fa-f]{1,4}:){7}[0-9A-Fa-f]{1,4}|(?:[0-9A-Fa-f]{1,4}:){1,7}:|:(?::[0-9A-Fa-f]{1,4}){1,7}|(?:[0-9A-Fa-f]{1,4}:){1,6}:[0-9A-Fa-f]{1,4}|(?:[0-9A-Fa-f]{1,4}:){1,5}(?::[0-9A-Fa-f]{1,4}){1,2}|(?:[0-9A-Fa-f]{1,4}:){1,4}(?::[0-9A-Fa-f]{1,4}){1,3}|(?:[0-9A-Fa-f]{1,4}:){1,3}(?::[0-9A-Fa-f]{1,4}){1,4}|(?:[0-9A-Fa-f]{1,4}:){1,2}(?::[0-9A-Fa-f]{1,4}){1,5}|[0-9A-Fa-f]{1,4}:(?:(?::[0-9A-Fa-f]{1,4}){1,6})|:(?:(?::[0-9A-Fa-f]{1,4}){1,6}))$") Then   ' If it's not IPv4, we'll check IPv6. The pattern is very long, but so are IPv6 addresses
                DynaLog.LogMessage("This is an IPv6 address.")
                ' Before increasing values, we'll check our 48-bit prefix to uniquely associate the IPv6 address with our categories declared above.
                ' We don't care about the whole string now, so the pattern is incredibly shorter.
                If Regex.IsMatch(dnsAddress, "^fe(8|9|a|b).*") Then     ' Link-Local pattern
                    DynaLog.LogMessage("Address Type: Link-Local")
                    LinkLocalIPv6Addresses += 1
                ElseIf Regex.IsMatch(dnsAddress, "^fec.*") Then         ' Site-Local pattern. Invalid address in the program's perspective, and as per RFC 3879: https://datatracker.ietf.org/doc/html/rfc3879
                    DynaLog.LogMessage("Address Type: Site-Local. For compatibility reasons, it will be treated as invalid")
                    InvalidAddressList.Add(String.Format("- {0} -- Site-Local Address; it is no longer in use", dnsAddress))
                    InvalidAddresses += 1
                ElseIf Regex.IsMatch(dnsAddress, "^f(c|d).*") Then      ' Unique Local address pattern. It's our Site-Local replacement as per RFC 4193: https://datatracker.ietf.org/doc/html/rfc4193
                    ' It can be either fc or fd depending on whether the prefix is locally assigned
                    DynaLog.LogMessage("Address Type: Unique Local Address")
                    UniqueLocalIPv6Addresses += 1
                Else
                    ' Outright add it
                    GlobalIPv6Addresses += 1
                End If
            Else
                DynaLog.LogMessage("This is an unrecognized address")
                InvalidAddresses += 1
                InvalidAddressList.Add(String.Format("- {0} -- Malformed Address", dnsAddress))
            End If
            current += 1
        Next

        ValidToInvalidAddressRatio = Math.Round(((IPv4Addresses + GlobalIPv6Addresses + LinkLocalIPv6Addresses + UniqueLocalIPv6Addresses) / total) * 100, 2)

        ' Now let's report our info to the user
        dnsAddressValidationInfo = String.Format("Address Syntax Validation Results:" & CrLf &
                                                 "- Invalid Addresses: {0}" & CrLf &
                                                 "- IPv4 Addresses: {1}" & CrLf &
                                                 "- Global IPv6 Addresses: {2}" & CrLf &
                                                 "- Link-Local IPv6 Addresses: {3}" & CrLf &
                                                 "- Unique Local IPv6 Addresses: {4}" & CrLf & CrLf &
                                                 "- Valid/Invalid Address Ratio: {5}%" & CrLf &
                                                 "{6}" & CrLf &
                                                 "These addresses will be configured in the unattended answer file.",
                                                 InvalidAddresses,
                                                 IPv4Addresses,
                                                 GlobalIPv6Addresses,
                                                 LinkLocalIPv6Addresses,
                                                 UniqueLocalIPv6Addresses,
                                                 ValidToInvalidAddressRatio,
                                                 If(InvalidAddresses > 0,
                                                    CrLf & "Some addresses are invalid. Here's why: " & CrLf & CrLf & String.Join(CrLf, InvalidAddressList) & CrLf,
                                                    ""))

        Throw New Exception("The verification has finished." & CrLf & CrLf & dnsAddressValidationInfo)
    End Sub

    Private Sub DnsValidatorBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles DnsValidatorBW.ProgressChanged
        ProgressReporter.ReportProgress(Me, e.ProgressPercentage)
    End Sub

    Private Sub DnsValidatorBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles DnsValidatorBW.RunWorkerCompleted
        ProgressReporter.Hide()
        If e.Error IsNot Nothing Then
            MessageBox.Show(e.Error.Message,
                            "DNS Address Validation Results",
                            MessageBoxButtons.OK,
                            If(e.Error.Message.StartsWith("The verification has finished."), MessageBoxIcon.Information, MessageBoxIcon.Error))
        End If
        DnsSyntaxCheckerBtn.Enabled = True
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        DnsSyntaxCheckerBtn.Enabled = Not String.IsNullOrEmpty(RichTextBox1.Text)
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        AddsUpnPathText.Text = String.Format("{0}@{1}", TextBox5.Text, TextBox4.Text)
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        AddsUpnPathText.Text = String.Format("{0}@{1}", TextBox5.Text, TextBox4.Text)
        AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, TextBox5.Text)
        initialUserName = TextBox5.Text
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Try
            ComboBox3.Items.Clear()
            ComboBox3.Items.AddRange(userMappings(ComboBox2.SelectedItem).OrderBy(Function(adUser) adUser.DisplayName).Select(Function(adUser) adUser.DisplayName).ToArray())
            ComboBox3.SelectedIndex = 0
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        Dim referenceUserDispName As String = ComboBox3.SelectedItem
        Dim referenceUser As Principal = userMappings(ComboBox2.SelectedItem).FirstOrDefault(Function(adUser) adUser.DisplayName.Equals(referenceUserDispName, StringComparison.OrdinalIgnoreCase))
        If referenceUser Is Nothing Then Exit Sub

        AddsUpnPathText.Text = referenceUser.UserPrincipalName
        AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, referenceUser.SamAccountName)
        initialUserName = referenceUser.SamAccountName

        If Not DomainServicesModule.DSAccountIsEnabled(dsDomainName, referenceUser.SamAccountName) Then
            MsgBox("The selected user is not enabled in the domain. The user will not be able to sign into target devices unless it's re-enabled.", vbOKOnly + vbExclamation, "Account Disabled")
        End If
    End Sub

    Private Sub DnsNsLookupBtn_Click(sender As Object, e As EventArgs) Handles DnsNsLookupBtn.Click
        If String.IsNullOrEmpty(TextBox1.Text) OrElse String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MsgBox("Please provide a domain for which to test domain name resolution.", vbOKOnly + vbExclamation, Text)
            Exit Sub
        End If

        Cursor = Cursors.WaitCursor
        Dim nslookupProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "nslookup.exe"),
                .Arguments = TextBox1.Text,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .UseShellExecute = False,
                .RedirectStandardOutput = True,
                .RedirectStandardError = True
            }
        }

        Try
            nslookupProc.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding(Globalization.CultureInfo.CurrentCulture.TextInfo.OEMCodePage)
            nslookupProc.StartInfo.StandardErrorEncoding = System.Text.Encoding.GetEncoding(Globalization.CultureInfo.CurrentCulture.TextInfo.OEMCodePage)
        Catch ex As Exception
            nslookupProc.StartInfo.StandardOutputEncoding = Nothing
            nslookupProc.StartInfo.StandardErrorEncoding = Nothing
        End Try

        Dim nslookupOut As String = ""
        nslookupProc.Start()
        nslookupOut = nslookupProc.StandardOutput.ReadToEnd() & nslookupProc.StandardError.ReadToEnd()
        nslookupProc.WaitForExit()
        Cursor = Cursors.Arrow
        MsgBox(String.Format("NSLOOKUP output:{0}{0}{1}", Environment.NewLine, nslookupOut), vbOKOnly + vbInformation, "Domain name resolution results")
    End Sub

    Private Sub DsAccountObjectPickerBtn_Click(sender As Object, e As EventArgs) Handles DsAccountObjectPickerBtn.Click
        If Not dsIsInDomain Then
            MessageBox.Show("This computer does not belong to a domain.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        Dim dsaPicker As New DirectoryObjectPickerDialog() With {
            .AllowedObjectTypes = ObjectTypes.Users,
            .DefaultObjectTypes = ObjectTypes.Users,
            .AllowedLocations = Locations.All,
            .DefaultLocations = Locations.JoinedDomain,
            .MultiSelect = False,
            .ShowAdvancedView = True
        }
        Using dsaPicker
            If dsaPicker.ShowDialog() = Windows.Forms.DialogResult.OK Then
                TextBox7.Text = DomainServicesModule.DSGetSamNameFromUserLdapPath(dsaPicker.SelectedObject.Path)
            End If
        End Using
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        ManualUserPickerPanel.Visible = ComboBox4.SelectedIndex = 0
        UserInDomainOuPickerPanel.Visible = ComboBox4.SelectedIndex = 1
        UserInDomainPickerPanel.Visible = ComboBox4.SelectedIndex = 2

        ' Reconfigure initial user
        Try
            Select Case ComboBox4.SelectedIndex
                Case 0
                    AddsUpnPathText.Text = String.Format("{0}@{1}", TextBox5.Text, TextBox4.Text)
                    AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, TextBox5.Text)
                    initialUserName = TextBox5.Text
                Case 1
                    Dim referenceUserDispName As String = ComboBox3.SelectedItem
                    Dim referenceUser As Principal = userMappings(ComboBox2.SelectedItem).FirstOrDefault(Function(adUser) adUser.DisplayName.Equals(referenceUserDispName, StringComparison.OrdinalIgnoreCase))
                    If referenceUser Is Nothing Then Exit Sub

                    AddsUpnPathText.Text = referenceUser.UserPrincipalName
                    AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, referenceUser.SamAccountName)
                    initialUserName = referenceUser.SamAccountName
                Case 2
                    initialUserName = TextBox7.Text
                    AddsUpnPathText.Text = DomainServicesModule.DSGetUserPrincipalNameFromSamAccountName(dsDomainName, initialUserName)
                    AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, TextBox7.Text)
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged
        initialUserName = TextBox7.Text
        AddsUpnPathText.Text = DomainServicesModule.DSGetUserPrincipalNameFromSamAccountName(dsDomainName, initialUserName)
        AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, TextBox7.Text)
    End Sub
End Class