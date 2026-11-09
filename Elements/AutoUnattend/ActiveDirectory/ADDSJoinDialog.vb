Imports System.Threading
Imports System.Net.NetworkInformation
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.RegularExpressions
Imports System.DirectoryServices.AccountManagement
Imports Tulpep.ActiveDirectoryObjectPicker
Imports System.DirectoryServices
Imports System.Net
Imports System.Net.Sockets

Public Class ADDSJoinDialog

    Private dsIsInDomain As Boolean
    Private dsDomainName As String = ""
    Private dsDefaultDomainName As String = ""
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
        Public Property DnsAddresses As New List(Of String)

        Public Sub New(suffix As String, interfaceAlias As String, addresses As List(Of String))
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

    ''' <summary>
    ''' Gets the current domain role of the system.
    ''' </summary>
    ''' <returns>The current domain role of the system</returns>
    Private Function GetSystemDomainRole() As DomainRole
        Dim domainRoleCollection As ManagementObjectCollection = GetResultsFromManagementQuery("SELECT DomainRole FROM Win32_ComputerSystem")
        If domainRoleCollection IsNot Nothing Then
            Return GetObjectValue(domainRoleCollection(0), "DomainRole")
        End If
        Return DomainRole.Unknown
    End Function

    Private dnsInfo As DnsInformation
    Private dsInfo As DomainInformation

    Private DomainsInForest As New List(Of ForestEntry)

    Private dnsAddresses As String()

    Private NtLogonPathStart As String = ""
    Private NtDefaultLogonPathStart As String = ""

    Private userMappings As Dictionary(Of String, List(Of Principal))
    Private initialUserName As String = ""

    Private IsDomainController As Boolean

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Private Sub ADDSJoinDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GroupBox1.Text = LocalizationService.ForSection("Designer.DomainJoin")("Nicsettings.Group")
        DnsSyntaxCheckerBtn.Text = LocalizationService.ForSection("Designer.DomainJoin")("Verify.DNS.Label")
        Label5.Text = LocalizationService.ForSection("Designer.DomainJoin")("Default.Adapter.Same.Message")
        RadioButton2.Text = LocalizationService.ForSection("Designer.DomainJoin")("ManualAdapter.RadioButton")
        Label7.Text = LocalizationService.ForSection("Designer.DomainJoin")("Address.First.Line.Message")
        Label6.Text = LocalizationService.ForSection("Designer.DomainJoin")("DNSServer.Addresses.Label")
        Label4.Text = LocalizationService.ForSection("Designer.DomainJoin")("PrimarySuffix.Label")
        RadioButton1.Text = LocalizationService.ForSection("Designer.DomainJoin")("PickAdapter.RadioButton")
        Label3.Text = LocalizationService.ForSection("Designer.DomainJoin")("InterfaceAlias.Label")
        Label1.Text = LocalizationService.ForSection("Designer.DomainJoin")("PrimarySuffix.Label")
        DNSConfigHeader.Text = LocalizationService.ForSection("Designer.DomainJoin")("DNSSettings.Label")
        Label17.Text = LocalizationService.ForSection("Designer.DomainJoin")("Type.Security.Account.Label")
        Label15.Text = LocalizationService.ForSection("Designer.DomainJoin")("Organizational.Unit.Label")
        Label16.Text = LocalizationService.ForSection("Designer.DomainJoin")("User.Label")
        Label19.Text = LocalizationService.ForSection("Designer.DomainJoin")("SAM.Account.Label")
        Label18.Text = LocalizationService.ForSection("Designer.DomainJoin")("Org.Unit.Account.Message")
        DsAccountObjectPickerBtn.Text = LocalizationService.ForSection("Designer.DomainJoin")("Pick.Account.Object.Button")
        RemoveHandler comboAccountMode.SelectedIndexChanged, AddressOf comboAccountMode_SelectedIndexChanged
        comboAccountMode.Items.Clear()
        comboAccountMode.Items.AddRange({LocalizationService.ForSection("Designer.DomainJoin")("User.Manually.Item"),
                                  LocalizationService.ForSection("Designer.DomainJoin")("Pick.User.Org.Item"),
                                  LocalizationService.ForSection("Designer.DomainJoin")("Pick.User.Object.Item")})
        AddHandler comboAccountMode.SelectedIndexChanged, AddressOf comboAccountMode_SelectedIndexChanged
        Label13.Text = LocalizationService.ForSection("Designer.DomainJoin")("User.Principal.Name.Label")
        Label14.Text = LocalizationService.ForSection("Designer.DomainJoin")("Logon.Path.Pre.Label")
        Label11.Text = LocalizationService.ForSection("Designer.DomainJoin")("Domain.Auto.Detected.Message")
        Label12.Text = LocalizationService.ForSection("Designer.DomainJoin")("Ask.Admin.Provide.Message")
        Label10.Text = LocalizationService.ForSection("Designer.DomainJoin")("Password.Label")
        Label9.Text = LocalizationService.ForSection("Designer.DomainJoin")("UserAccount.Label")
        Label8.Text = LocalizationService.ForSection("Designer.DomainJoin")("DomainName.Label")
        rbAutoDomainsInForest.Text = LocalizationService.ForSection("Designer.DomainJoin")("PDS_AutoDomainForest")
        rbManualDomain.Text = LocalizationService.ForSection("Designer.DomainJoin")("PDS_ManualDomain")
        DSDomainConfigHeader.Text = LocalizationService.ForSection("Designer.DomainJoin")("Domain.Auth.Label")
        DS7_Description.Text = LocalizationService.ForSection("Designer.DomainJoin")("Wizard.Helps.Set.Description")
        DS7_Header.Text = LocalizationService.ForSection("Designer.DomainJoin")("Join.Active.Dir.Label")
        DNS_Explanation_Link.Text = LocalizationService.ForSection("Designer.DomainJoin")("WhatDNS.Link")
        Back_Button.Text = LocalizationService.ForSection("Designer.DomainJoin")("Back.Button")
        Next_Button.Text = LocalizationService.ForSection("Designer.DomainJoin")("Next.Button")
        Cancel_Button.Text = LocalizationService.ForSection("Designer.DomainJoin")("Cancel.Button")
        Help_Button.Text = LocalizationService.ForSection("Designer.DomainJoin")("Help.Button")
        DnsResolutionTSMI.Text = LocalizationService.ForSection("Designer.DomainJoin")("Test.Dnsresolution.Label")
        DnsZoneTSMI.Text = LocalizationService.ForSection("Designer.DomainJoin")("DNSZone.Domain.Choose.Label")
        Text = LocalizationService.ForSection("Designer.DomainJoin")("Domain.Services.Wizard.Label")

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        cbNicsInSystem.BackColor = BackColor
        comboOU.BackColor = BackColor
        comboUserInOU.BackColor = BackColor
        comboAccountMode.BackColor = BackColor
        comboDomainsInForest.BackColor = BackColor
        tbManualDomainName.BackColor = BackColor
        tbManualNicName.BackColor = BackColor
        tbNicPDS.BackColor = BackColor
        tbDomainName.BackColor = BackColor
        tbManualSamName.BackColor = BackColor
        tbAccountPassword.BackColor = BackColor
        tbSamNameInDomain.BackColor = BackColor
        GroupBox1.BackColor = BackColor
        rtbDNSAddresses.BackColor = BackColor
        cbNicsInSystem.ForeColor = ForeColor
        comboOU.ForeColor = ForeColor
        comboUserInOU.ForeColor = ForeColor
        comboAccountMode.ForeColor = ForeColor
        comboDomainsInForest.ForeColor = ForeColor
        tbManualDomainName.ForeColor = ForeColor
        tbManualNicName.ForeColor = ForeColor
        tbNicPDS.ForeColor = ForeColor
        tbDomainName.ForeColor = ForeColor
        tbManualSamName.ForeColor = ForeColor
        tbAccountPassword.ForeColor = ForeColor
        tbSamNameInDomain.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        rtbDNSAddresses.ForeColor = ForeColor
        DsJoinCMS.Renderer = GetProfessionalRenderer()
        DsJoinCMS.ForeColor = ForeColor
        DnsResolutionTSMI.Image = GetGlyphResource("search")
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        VerifyInPages.AddRange(New WizardPage() {WizardPage.DnsConfigPage, WizardPage.DsConfigPage})
        ChangePage(WizardPage.DnsConfigPage)
        IsDomainController = GetSystemDomainRole() >= DomainRole.PrimaryDomainController
        dnsMappings.Clear()
        comboDomainsInForest.Items.Clear()
        cbNicsInSystem.Items.Clear()
        comboOU.Items.Clear()
        comboUserInOU.Items.Clear()
        EnsureHandlesCreated(Me)
        Visible = True
        ADDSInitBW.RunWorkerAsync()
        Do Until Not ADDSInitBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        Loop
    End Sub

    Private Sub EnsureHandlesCreated(parentControl As Control)
        Dim forcedHandle As IntPtr = parentControl.Handle
        For Each childControl As Control In parentControl.Controls
            EnsureHandlesCreated(childControl)
        Next
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        cbNicsInSystem.Enabled = RadioButton1.Checked
        tbManualNicName.Enabled = RadioButton2.Checked
    End Sub

    Private Sub rbAutoDomainsInForest_CheckedChanged(sender As Object, e As EventArgs) Handles rbAutoDomainsInForest.CheckedChanged
        comboDomainsInForest.Enabled = rbAutoDomainsInForest.Checked
        tbManualDomainName.Enabled = Not rbAutoDomainsInForest.Checked
        tbDomainName.ReadOnly = rbAutoDomainsInForest.Checked

        dsDomainName = If(rbAutoDomainsInForest.Checked, comboDomainsInForest.SelectedItem, tbManualDomainName.Text)
        If Not rbAutoDomainsInForest.Checked Then NtLogonPathStart = NtDefaultLogonPathStart ' default to the default NT NETBIOS name
        tbDomainName.Text = dsDomainName

        ' Let's leave the domain controller-exclusive stuff, well, exclusive to domain controllers ;)
        DnsZoneTSMI.Enabled = IsDomainController AndAlso rbManualDomain.Checked

        tbNicPDS.Text = dsDomainName
    End Sub

    Private Sub InitializeWizard()
        ProgressReporter.SetMessage("Checking if device is part of a domain...")
        ADDSInitBW.ReportProgress(5)
        dsIsInDomain = DomainServicesModule.DSIsInDomain()
        If dsIsInDomain Then
            rbAutoDomainsInForest.Enabled = True
            rbAutoDomainsInForest.Checked = True
            rbManualDomain.Checked = False
            ProgressReporter.SetMessage("Getting name of assigned domain...")
            ADDSInitBW.ReportProgress(20)
            dsDomainName = DomainServicesModule.DSGetDomainName()
            dsDefaultDomainName = dsDomainName
            ProgressReporter.SetMessage("Getting domains in current forest...")
            ADDSInitBW.ReportProgress(30)
            DomainsInForest = DomainServicesModule.DSGetDomainsInForest()
            comboDomainsInForest.Items.AddRange(DomainsInForest.Select(Function(domain) domain.DnsPath).ToArray())
            comboDomainsInForest.SelectedItem = dsDomainName
        Else
            rbAutoDomainsInForest.Checked = False
            rbManualDomain.Checked = True
            rbAutoDomainsInForest.Enabled = False
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
        NtDefaultLogonPathStart = NtLogonPathStart
        If dsIsInDomain Then
            comboAccountMode.SelectedIndex = 1
            ProgressReporter.SetMessage("Mapping organizational units and users...")
            ADDSInitBW.ReportProgress(90)
            userMappings = DomainServicesModule.DSMapOrganizationalUnitsAndUsers(dsDomainName, NtLogonPathStart)
            If userMappings IsNot Nothing Then
                comboOU.Items.AddRange(userMappings.Keys.OrderBy(Function(ou) ou).ToArray())
                comboOU.SelectedIndex = 0
            End If
        Else
            comboAccountMode.SelectedIndex = 0
            If comboAccountMode.Items.Count > 1 Then
                ' We do this twice to get rid of both items
                comboAccountMode.Items.RemoveAt(1)
                comboAccountMode.Items.RemoveAt(1)
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
            cbNicsInSystem.Items.Add(mapping.Key)
        Next
        ProgressReporter.Hide()
        DSNoDomainPanel.Visible = Not dsIsInDomain
        If dsIsInDomain Then
            RemoveHandler tbDomainName.TextChanged, AddressOf TextBox4_TextChanged
            tbDomainName.Text = dsDomainName
            AddHandler tbDomainName.TextChanged, AddressOf TextBox4_TextChanged
            If NtLogonPathStart = "" Then
                NtLogonPathStart = "Primary DC NetBIOS"
            End If
        Else
            NtLogonPathStart = "Primary DC NetBIOS"
        End If
        If comboUserInOU.SelectedIndex < 0 Then AddsNtLogonPathText.Text = String.Format("{0}\<user>", NtLogonPathStart)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbNicsInSystem.SelectedIndexChanged
        Try
            Dim dnsConfig As DnsProperties = dnsMappings(cbNicsInSystem.SelectedItem)
            tbManualDomainName.Text = dnsConfig.Suffix
            rtbDNSAddresses.Text = ""
            If dnsConfig.Addresses.Count > 0 Then
                For Each Address In dnsConfig.Addresses
                    rtbDNSAddresses.AppendText(Address.ToString() & CrLf)
                Next
            End If
            dnsAliasName = cbNicsInSystem.SelectedItem

            If rbAutoDomainsInForest.Checked Then tbNicPDS.Text = dsDomainName
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tbManualDomainName_TextChanged(sender As Object, e As EventArgs) Handles tbManualDomainName.TextChanged
        tbNicPDS.Text = tbManualDomainName.Text
        dsDomainName = tbManualDomainName.Text
    End Sub

    Private Sub ChangePage(NewPage As WizardPage)
        DynaLog.LogMessage("Changing current page of the wizard...")
        DynaLog.LogMessage("New page to load: " & NewPage.ToString())
        If NewPage > CurrentWizardPage AndAlso VerifyInPages.Contains(CurrentWizardPage) Then
            If Not VerifyOptionsInPage(CurrentWizardPage) Then Exit Sub
        End If

        tbDomainName.Text = dsDomainName

        ' Apply settings in the DNS class if in DNS page
        If CurrentWizardPage = WizardPage.DnsConfigPage Then
            dnsInfo = New DnsInformation(dsDomainName, dnsAliasName, rtbDNSAddresses.Lines.ToList())
        End If

        DNSConfigPanel.Visible = (NewPage = WizardPage.DnsConfigPage)
        DSDomainConfigPanel.Visible = (NewPage = WizardPage.DsConfigPage)

        CurrentWizardPage = NewPage
        Back_Button.Enabled = Not (NewPage = WizardPage.DnsConfigPage)

        Next_Button.Text = If(NewPage = WizardPage.DsConfigPage, LocalizationService.ForSection("ADDSJoinDialog.ChangePage")("Finish.Label"), LocalizationService.ForSection("ADDSJoinDialog.ChangePage")("Next.Button"))

        DNS_Explanation_Link.Visible = (NewPage = WizardPage.DnsConfigPage)
    End Sub

    Private Function VerifyOptionsInPage(page As WizardPage) As Boolean
        DynaLog.LogMessage("Verifying user options before moving on to next page...")
        DynaLog.LogMessage("Page in which we need to verify user settings: " & page.ToString())
        Select Case page
            Case WizardPage.DnsConfigPage
                DynaLog.LogMessage("Verifying DNS configuration options...")

                If rbAutoDomainsInForest.Checked Then
                    DynaLog.LogMessage("Automatic domain selection is enabled.")
                    DynaLog.LogMessage("Selected domain: " & If(comboDomainsInForest.SelectedItem Is Nothing, "<none>", comboDomainsInForest.SelectedItem.ToString()))

                    ' If it isn't the current domain, we test connectivity and get the users for that.
                    Dim connectionDetails As New Dictionary(Of String, Object),
                        errorMessage As String = ""
                    Dim connectionSucceeded As Boolean = False
                    Do Until connectionSucceeded
                        DynaLog.LogMessage("Testing connectivity to the selected domain controller...")
                        Cursor = Cursors.WaitCursor
                        ADDSDomainControllerConnectionTestProgressForm.DomainName = dsDomainName
                        ADDSDomainControllerConnectionTestProgressForm.Show(Me)
                        ADDSDomainControllerConnectionTestProgressForm.BringToFront()
                        Application.DoEvents()
                        connectionDetails = DomainServicesModule.DSTestDomainControllerConnection(comboDomainsInForest.SelectedItem)
                        ADDSDomainControllerConnectionTestProgressForm.Close()
                        Cursor = Cursors.Arrow

                        connectionSucceeded = CBool(connectionDetails("connected"))
                        DynaLog.LogMessage("Domain controller connectivity test result: " & connectionSucceeded.ToString())

                        If Not connectionSucceeded Then
                            DynaLog.LogMessage("Domain controller connectivity test failed.")

                            If TypeOf (connectionDetails("connectionFailureDetails")) Is DirectoryServicesCOMException Then
                                Dim finalEx As DirectoryServicesCOMException = CType(connectionDetails("connectionFailureDetails"), DirectoryServicesCOMException)

                                DynaLog.LogMessage("Directory Services connection error: " & finalEx.Message)
                                errorMessage = LocalizationService.ForSection("DomainJoin.Messages").Format("ConnectionToDomainFailed", comboDomainsInForest.SelectedItem, String.Format("{0} - {1}", finalEx.Message.TrimEnd(Environment.NewLine), finalEx.ExtendedErrorMessage))

                                If MessageBox.Show(Me, errorMessage, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                                    DynaLog.LogMessage("User chose not to retry the domain controller connectivity test.")
                                    Return False
                                End If
                            Else
                                Dim connectionException As Exception = CType(connectionDetails("connectionFailureDetails"), Exception)

                                DynaLog.LogMessage("Domain connection error: " & connectionException.Message)
                                errorMessage = connectionException.Message
                                MessageBox.Show(Me, errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return False
                            End If

                            DynaLog.LogMessage("User chose to retry the domain controller connectivity test.")
                        End If
                    Loop

                    Cursor = Cursors.WaitCursor
                    DynaLog.LogMessage("Loading organizational units and users for the selected domain...")

                    comboUserInOU.Items.Clear()
                    comboOU.Items.Clear()
                    userMappings = DomainServicesModule.DSMapOrganizationalUnitsAndUsers(dsDomainName, NtLogonPathStart)

                    If userMappings IsNot Nothing Then
                        DynaLog.LogMessage("Organizational unit mappings loaded: " & userMappings.Count.ToString())
                        comboOU.Items.AddRange(userMappings.Keys.OrderBy(Function(ou) ou).ToArray())
                        comboOU.SelectedIndex = 0
                    Else
                        DynaLog.LogMessage("No organizational unit mappings were returned.")
                    End If

                    Cursor = Cursors.Arrow
                Else
                    DynaLog.LogMessage("Manual domain selection is enabled.")

                    ' Check if we have put anything in the manual domain field.
                    If dsDomainName = "" Then
                        DynaLog.LogMessage("Manual primary DNS suffix validation failed: no value was provided.")
                        MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("PrimarySuffix.Required"), vbOKOnly + vbCritical)
                        Return False
                    End If
                End If

                If dnsAliasName = "" Then
                    DynaLog.LogMessage("DNS interface alias validation failed: no interface alias was provided.")
                    MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("InterfaceAlias.Message"), vbOKOnly + vbCritical)
                    Return False
                End If

                If rtbDNSAddresses.Text = "" Then
                    DynaLog.LogMessage("DNS server validation failed: no DNS server addresses were provided.")
                    MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("No.DNSServer.None.Label"), vbOKOnly + vbCritical)
                    Return False
                End If

                DynaLog.LogMessage("DNS configuration options validated successfully.")
            Case WizardPage.DsConfigPage
                If tbDomainName.Text = "" Then
                    MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("DomainName.Label"), vbOKOnly + vbCritical)
                    Return False
                End If
                If initialUserName = "" Then
                    MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("User.Name.Label"), vbOKOnly + vbCritical)
                    Return False
                End If
                If tbAccountPassword.Text = "" Then
                    Try
                        If DomainServicesModule.DSAccountRequiresPassword(dsDomainName, initialUserName) Then
                            MsgBox(LocalizationService.ForSection("DomainJoin.Messages").Format("Password.User.Message", initialUserName), vbOKOnly + vbCritical)
                            Return False
                        End If
                    Catch ex As Exception
                        MsgBox(LocalizationService.ForSection("DomainJoin.Messages").Format("Password.User.Message", initialUserName), vbOKOnly + vbCritical)
                        Return False
                    End Try
                End If
                If dsIsInDomain AndAlso Not DomainServicesModule.DSAccountExists(dsDomainName, initialUserName) Then
                    If MsgBox(LocalizationService.ForSection("DomainJoin.Messages").Format("User.Appear.Exist.Message", initialUserName), vbYesNo + vbExclamation, Text) = MsgBoxResult.No Then
                        Return False
                    End If
                End If
                Return MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("Verify.Typed.Message"), vbYesNo + vbQuestion, LocalizationService.ForSection("DomainJoin.Messages")("VerifySettings.Title")) = MsgBoxResult.Yes
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
                dsInfo = New DomainInformation(tbDomainName.Text, initialUserName, tbAccountPassword.Text)
            End If
            If ApplyDsSettings() Then
                MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("Domain.Settings.Message"))
                SetDefaultSettings()
                Close()
            Else
                MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("Add.Domain.Settings.Label"))
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
            If rbAutoDomainsInForest.Checked Then
                Try
                    ' Try to get the address of the DNS server hosting the domain. If not present, a client
                    ' may not be able to join the domain. So, try to help.
                    Dim serverDnsAddresses As IPAddress() = Dns.GetHostAddresses(dsDomainName)
                    Dim RouteTableMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT InterfaceIndex FROM Win32_IP4RouteTable WHERE Destination = '0.0.0.0'")
                    If RouteTableMOC Is Nothing Then Throw New Exception()

                    Dim interfaceIndex As Integer = WMIHelper.GetObjectValue(RouteTableMOC(0), "InterfaceIndex")

                    Dim DefaultNICAddressMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT IPAddress FROM Win32_NetworkAdapterConfiguration WHERE InterfaceIndex = {0}", interfaceIndex))
                    Dim DefaultNICAddresses As String() = WMIHelper.GetObjectValue(DefaultNICAddressMOC(0), "IPAddress")

                    For Each serverDnsAddress In serverDnsAddresses
                        Dim finalAddress As String = DefaultNICAddresses.FirstOrDefault(Function(address) address = serverDnsAddress.ToString())

                        If finalAddress <> "" AndAlso Not dnsInfo.DnsAddresses.Contains(finalAddress) Then
                            dnsInfo.DnsAddresses.Insert(0, finalAddress)
                        End If
                    Next
                Catch notConnectedEx As SocketException

                Catch ex As Exception

                End Try
            End If
            Dim dnsServerSearchOrder As String = "        <DNSServerSearchOrder>" & CrLf
            For Each Address In dnsInfo.DnsAddresses
                If String.IsNullOrEmpty(Address) Then Continue For
                dnsServerSearchOrder &= String.Format("            <IpAddress wcm:action=" & Quote & "add" & Quote & " wcm:keyValue=" & Quote & "{0}" & Quote & ">{1}</IpAddress>" & CrLf,
                                                      dnsInfo.DnsAddresses.IndexOf(Address) + 1,
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
        tbManualDomainName.Text = ""
        tbManualNicName.Text = ""
        rtbDNSAddresses.Clear()
        RadioButton1.Checked = True
        DynaLog.LogMessage("Setting default domain settings...")
        tbDomainName.Text = ""
        tbManualSamName.Text = ""
        tbAccountPassword.Text = ""
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles tbManualNicName.TextChanged
        dnsAliasName = tbManualNicName.Text
    End Sub

    Private Sub DNS_Explanation_Link_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles DNS_Explanation_Link.LinkClicked
        MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("Dnsshort.DomainName.Message"),
               vbOKOnly + vbInformation)
    End Sub

    Private Sub DnsSyntaxCheckerBtn_Click(sender As Object, e As EventArgs) Handles DnsSyntaxCheckerBtn.Click
        If DnsValidatorBW.IsBusy Then
            Exit Sub
        End If
        DnsSyntaxCheckerBtn.Enabled = False
        dnsAddresses = rtbDNSAddresses.Lines.Where(Function(address) Not String.IsNullOrEmpty(address)).ToArray()
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
                    InvalidAddressList.Add(LocalizationService.ForSection("DomainJoin.DNS").Format("Site.Local.Address.Label", dnsAddress))
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
                InvalidAddressList.Add(LocalizationService.ForSection("DomainJoin.DNS").Format("MalformedAddress.Label", dnsAddress))
            End If
            current += 1
        Next

        ValidToInvalidAddressRatio = Math.Round(((IPv4Addresses + GlobalIPv6Addresses + LinkLocalIPv6Addresses + UniqueLocalIPv6Addresses) / total) * 100, 2)

        ' Now let's report our info to the user
        dnsAddressValidationInfo = LocalizationService.ForSection("DomainJoin.DNS").Format("AddressSyntax.Message", InvalidAddresses, IPv4Addresses, GlobalIPv6Addresses, LinkLocalIPv6Addresses, UniqueLocalIPv6Addresses, ValidToInvalidAddressRatio, If(InvalidAddresses > 0,
                                                    LocalizationService.ForSection("DomainJoin.DNS").Format("InvalidAddresses.Label", String.Join(CrLf, InvalidAddressList)),
                                                    ""))

        Throw New Exception(LocalizationService.ForSection("DomainJoin.DNS").Format("Verification.Done.Message", dnsAddressValidationInfo))
    End Sub

    Private Sub DnsValidatorBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles DnsValidatorBW.ProgressChanged
        ProgressReporter.ReportProgress(Me, e.ProgressPercentage)
    End Sub

    Private Sub DnsValidatorBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles DnsValidatorBW.RunWorkerCompleted
        ProgressReporter.Hide()
        If e.Error IsNot Nothing Then
            MessageBox.Show(e.Error.Message,
                            LocalizationService.ForSection("DomainJoin.DNS")("AddressValidation.Title"),
                            MessageBoxButtons.OK,
                            If(e.Error.Message.StartsWith(LocalizationService.ForSection("DomainJoin.DNS")("Verification.Done.Label")), MessageBoxIcon.Information, MessageBoxIcon.Error))
        End If
        DnsSyntaxCheckerBtn.Enabled = True
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles rtbDNSAddresses.TextChanged
        DnsSyntaxCheckerBtn.Enabled = Not String.IsNullOrEmpty(rtbDNSAddresses.Text)
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles tbDomainName.TextChanged
        AddsUpnPathText.Text = String.Format("{0}@{1}", If(String.IsNullOrEmpty(tbManualSamName.Text), "<user>", tbManualSamName.Text), If(String.IsNullOrEmpty(tbDomainName.Text), "domain", tbDomainName.Text))
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles tbManualSamName.TextChanged
        AddsUpnPathText.Text = String.Format("{0}@{1}", If(String.IsNullOrEmpty(tbManualSamName.Text), "<user>", tbManualSamName.Text), If(String.IsNullOrEmpty(tbDomainName.Text), "domain", tbDomainName.Text))
        AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, If(String.IsNullOrEmpty(tbManualSamName.Text), "<user>", tbManualSamName.Text))
        initialUserName = tbManualSamName.Text
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles comboOU.SelectedIndexChanged
        Try
            comboUserInOU.Items.Clear()
            comboUserInOU.Items.AddRange(userMappings(comboOU.SelectedItem).OrderBy(Function(adUser) adUser.DisplayName).Select(Function(adUser) adUser.DisplayName).ToArray())
            comboUserInOU.SelectedIndex = 0
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles comboUserInOU.SelectedIndexChanged
        Dim referenceUserDispName As String = comboUserInOU.SelectedItem
        Dim referenceUser As Principal = userMappings(comboOU.SelectedItem).FirstOrDefault(Function(adUser) adUser.DisplayName.Equals(referenceUserDispName, StringComparison.OrdinalIgnoreCase))
        If referenceUser Is Nothing Then Exit Sub

        AddsUpnPathText.Text = referenceUser.UserPrincipalName
        AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, referenceUser.SamAccountName)
        initialUserName = referenceUser.SamAccountName

        If Not DomainServicesModule.DSAccountIsEnabled(dsDomainName, referenceUser.SamAccountName) Then
            MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("UserDisabled.Message"), vbOKOnly + vbExclamation, LocalizationService.ForSection("DomainJoin.Messages")("AccountDisabled.Title"))
        End If
    End Sub

    Private Sub DnsToolsBtn_Click(sender As Object, e As EventArgs) Handles DnsToolsBtn.Click
        DsJoinCMS.Show(sender, New Point(8, 8))
    End Sub

    Private Sub DsAccountObjectPickerBtn_Click(sender As Object, e As EventArgs) Handles DsAccountObjectPickerBtn.Click
        If Not dsIsInDomain Then
            MessageBox.Show(LocalizationService.ForSection("DomainJoin.Messages")("Computer.Belong.Domain.Label"), Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
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
                tbSamNameInDomain.Text = DomainServicesModule.DSGetSamNameFromUserLdapPath(dsaPicker.SelectedObject.Path)
            End If
        End Using
    End Sub

    Private Sub comboAccountMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles comboAccountMode.SelectedIndexChanged
        If {1, 2}.Contains(comboAccountMode.SelectedIndex) AndAlso Not rbAutoDomainsInForest.Checked Then
            MessageBox.Show(Me, LocalizationService.ForSection("Designer.DomainJoin")("AccountMode_ManualDomainNotice"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            comboAccountMode.SelectedIndex = 0
            Exit Sub
        End If

        ManualUserPickerPanel.Visible = comboAccountMode.SelectedIndex = 0
        UserInDomainOuPickerPanel.Visible = comboAccountMode.SelectedIndex = 1
        UserInDomainPickerPanel.Visible = comboAccountMode.SelectedIndex = 2

        ' Reconfigure initial user
        Try
            Select Case comboAccountMode.SelectedIndex
                Case 0
                    AddsUpnPathText.Text = String.Format("{0}@{1}", If(String.IsNullOrEmpty(tbManualSamName.Text), "<user>", tbManualSamName.Text), If(String.IsNullOrEmpty(tbDomainName.Text), "domain", tbDomainName.Text))
                    AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, If(String.IsNullOrEmpty(tbManualSamName.Text), "<user>", tbManualSamName.Text))
                    initialUserName = tbManualSamName.Text
                Case 1
                    Dim referenceUserDispName As String = comboUserInOU.SelectedItem
                    Dim referenceUser As Principal = userMappings(comboOU.SelectedItem).FirstOrDefault(Function(adUser) adUser.DisplayName.Equals(referenceUserDispName, StringComparison.OrdinalIgnoreCase))
                    If referenceUser Is Nothing Then Exit Sub

                    AddsUpnPathText.Text = referenceUser.UserPrincipalName
                    AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, referenceUser.SamAccountName)
                    initialUserName = referenceUser.SamAccountName
                Case 2
                    initialUserName = tbSamNameInDomain.Text
                    AddsUpnPathText.Text = DomainServicesModule.DSGetUserPrincipalNameFromSamAccountName(dsDomainName, initialUserName)
                    AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, tbSamNameInDomain.Text)
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles tbSamNameInDomain.TextChanged
        initialUserName = tbSamNameInDomain.Text
        AddsUpnPathText.Text = DomainServicesModule.DSGetUserPrincipalNameFromSamAccountName(dsDomainName, initialUserName)
        AddsNtLogonPathText.Text = String.Format("{0}\{1}", NtLogonPathStart, tbSamNameInDomain.Text)
    End Sub

    Private Sub Help_Button_Click(sender As Object, e As EventArgs) Handles Help_Button.Click
        HelpDocsModule.DisplayHelpDocumentation("docs\img_tasks\unattend\unatt_create.html", "active-directory-domain-services-domain-join")
        DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub DnsResolutionTSMI_Click(sender As Object, e As EventArgs) Handles DnsResolutionTSMI.Click
        If String.IsNullOrEmpty(dsDomainName) OrElse String.IsNullOrWhiteSpace(dsDomainName) Then
            MsgBox(LocalizationService.ForSection("DomainJoin.Messages")("Provide.Domain.Label"), vbOKOnly + vbExclamation, Text)
            Exit Sub
        End If

        Cursor = Cursors.WaitCursor
        Dim nslookupProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "nslookup.exe"),
                .Arguments = dsDomainName,
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
        MsgBox(LocalizationService.ForSection("DomainJoin.Messages").Format("Nslookupoutput.Label", nslookupOut), vbOKOnly + vbInformation, LocalizationService.ForSection("DomainJoin.Messages")("DomainResolution.Title"))
    End Sub

    Private Sub DnsZoneTSMI_Click(sender As Object, e As EventArgs) Handles DnsZoneTSMI.Click
        If DnsZoneChooserDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            tbManualDomainName.Text = DnsZoneChooserDialog.SelectedDnsZone
        End If
    End Sub

    Private Sub comboDomainsInForest_SelectedIndexChanged(sender As Object, e As EventArgs) Handles comboDomainsInForest.SelectedIndexChanged
        If comboDomainsInForest.SelectedIndex > -1 Then
            dsDomainName = comboDomainsInForest.SelectedItem
            NtLogonPathStart = If(dsDomainName = dsDefaultDomainName, NtDefaultLogonPathStart, DomainsInForest(comboDomainsInForest.SelectedIndex).NetBIOSName)
            tbDomainName.Text = dsDomainName
            tbNicPDS.Text = dsDomainName
        End If
    End Sub
End Class