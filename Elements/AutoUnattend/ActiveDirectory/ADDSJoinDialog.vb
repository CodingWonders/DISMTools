Imports System.Threading
Imports System.Net.NetworkInformation
Imports Microsoft.VisualBasic.ControlChars

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

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Private Sub ADDSJoinDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = BackColor
        TextBox1.BackColor = BackColor
        TextBox2.BackColor = BackColor
        TextBox3.BackColor = BackColor
        TextBox4.BackColor = BackColor
        TextBox5.BackColor = BackColor
        TextBox6.BackColor = BackColor
        GroupBox1.BackColor = BackColor
        RichTextBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        TextBox5.ForeColor = ForeColor
        TextBox6.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, CurrentTheme.IsDark)
        VerifyInPages.AddRange(New WizardPage() {WizardPage.DnsConfigPage, WizardPage.DsConfigPage})
        ChangePage(WizardPage.DnsConfigPage)
        ComboBox1.Items.Clear()
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
            ADDSInitBW.ReportProgress(25)
            dsDomainName = DomainServicesModule.DSGetDomainName()
        End If
        ProgressReporter.SetMessage("Getting network adapters...")
        ADDSInitBW.ReportProgress(50)
        GetNetworkAdapters()
        ProgressReporter.SetMessage("Getting DNS addresses for each network adapter...")
        ADDSInitBW.ReportProgress(75)
        GetDnsAddresses()
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
        ProgressReporter.ReportProgress(Me, "", e.ProgressPercentage)
    End Sub

    Private Sub ADDSInitBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ADDSInitBW.RunWorkerCompleted
        For Each mapping In dnsMappings
            ComboBox1.Items.Add(mapping.Key)
        Next
        ProgressReporter.Hide()
        DSNoDomainPanel.Visible = Not dsIsInDomain
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
                If TextBox5.Text = "" Then
                    MsgBox("A user name must be specified", vbOKOnly + vbCritical)
                    Return False
                End If
                If TextBox6.Text = "" Then
                    MsgBox("A password must be specified as per security policies imposed by the domain controller", vbOKOnly + vbCritical)
                    Return False
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
                dsInfo = New DomainInformation(TextBox4.Text, TextBox5.Text, TextBox6.Text)
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
        DynaLog.LogMessage("Applying services for Domain Services...")
        Try
            DynaLog.LogMessage("Specified Domain Name System (DNS) information:" & CrLf &
                               "- Primary DNS Suffix: " & dnsInfo.Suffix & CrLf &
                               "- NIC (Network Adapter) Alias: " & dnsInfo.NicAlias & CrLf &
                               "- Addresses: " & String.Join(", ", dnsInfo.DnsAddresses))
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
End Class