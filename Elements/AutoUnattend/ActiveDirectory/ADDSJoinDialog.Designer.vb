<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ADDSJoinDialog
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ADDSJoinDialog))
        Me.ExpressPanelContainer = New System.Windows.Forms.Panel()
        Me.ExperimentalPanel = New System.Windows.Forms.Panel()
        Me.StepsContainer = New System.Windows.Forms.Panel()
        Me.DNSConfigPanel = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.DnsSyntaxCheckerBtn = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DNSConfigHeader = New System.Windows.Forms.Label()
        Me.DSDomainConfigPanel = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.AddsUpnPathText = New System.Windows.Forms.Label()
        Me.AddsNtLogonPathText = New System.Windows.Forms.Label()
        Me.DomainAutoUserPanel = New System.Windows.Forms.Panel()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.DSNoDomainPanel = New System.Windows.Forms.Panel()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.DSDomainConfigHeader = New System.Windows.Forms.Label()
        Me.HeaderPanel = New System.Windows.Forms.Panel()
        Me.DS7_Description = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.DS7_Header = New System.Windows.Forms.Label()
        Me.FooterContainer = New System.Windows.Forms.Panel()
        Me.ExpressPanelFooter = New System.Windows.Forms.Panel()
        Me.DNS_Explanation_Link = New System.Windows.Forms.LinkLabel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Back_Button = New System.Windows.Forms.Button()
        Me.Next_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Help_Button = New System.Windows.Forms.Button()
        Me.ADDSInitBW = New System.ComponentModel.BackgroundWorker()
        Me.DnsValidatorBW = New System.ComponentModel.BackgroundWorker()
        Me.DnsNsLookupBtn = New System.Windows.Forms.Button()
        Me.ExpressPanelContainer.SuspendLayout()
        Me.ExperimentalPanel.SuspendLayout()
        Me.StepsContainer.SuspendLayout()
        Me.DNSConfigPanel.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.DSDomainConfigPanel.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.DomainAutoUserPanel.SuspendLayout()
        Me.DSNoDomainPanel.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.HeaderPanel.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FooterContainer.SuspendLayout()
        Me.ExpressPanelFooter.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ExpressPanelContainer
        '
        Me.ExpressPanelContainer.Controls.Add(Me.ExperimentalPanel)
        Me.ExpressPanelContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExpressPanelContainer.Location = New System.Drawing.Point(0, 72)
        Me.ExpressPanelContainer.Name = "ExpressPanelContainer"
        Me.ExpressPanelContainer.Size = New System.Drawing.Size(784, 449)
        Me.ExpressPanelContainer.TabIndex = 2
        '
        'ExperimentalPanel
        '
        Me.ExperimentalPanel.Controls.Add(Me.StepsContainer)
        Me.ExperimentalPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExperimentalPanel.Location = New System.Drawing.Point(0, 0)
        Me.ExperimentalPanel.Name = "ExperimentalPanel"
        Me.ExperimentalPanel.Size = New System.Drawing.Size(784, 449)
        Me.ExperimentalPanel.TabIndex = 2
        '
        'StepsContainer
        '
        Me.StepsContainer.Controls.Add(Me.DNSConfigPanel)
        Me.StepsContainer.Controls.Add(Me.DSDomainConfigPanel)
        Me.StepsContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StepsContainer.Location = New System.Drawing.Point(0, 0)
        Me.StepsContainer.Name = "StepsContainer"
        Me.StepsContainer.Size = New System.Drawing.Size(784, 449)
        Me.StepsContainer.TabIndex = 1
        '
        'DNSConfigPanel
        '
        Me.DNSConfigPanel.Controls.Add(Me.DnsNsLookupBtn)
        Me.DNSConfigPanel.Controls.Add(Me.GroupBox1)
        Me.DNSConfigPanel.Controls.Add(Me.TextBox1)
        Me.DNSConfigPanel.Controls.Add(Me.Label2)
        Me.DNSConfigPanel.Controls.Add(Me.Label1)
        Me.DNSConfigPanel.Controls.Add(Me.DNSConfigHeader)
        Me.DNSConfigPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DNSConfigPanel.Location = New System.Drawing.Point(0, 0)
        Me.DNSConfigPanel.Name = "DNSConfigPanel"
        Me.DNSConfigPanel.Size = New System.Drawing.Size(784, 449)
        Me.DNSConfigPanel.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.DnsSyntaxCheckerBtn)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.TextBox3)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.ComboBox1)
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(28, 116)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(716, 316)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "NIC Settings"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RichTextBox1)
        Me.Panel1.Location = New System.Drawing.Point(27, 208)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(657, 66)
        Me.Panel1.TabIndex = 8
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox1.Location = New System.Drawing.Point(0, 0)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.RichTextBox1.Size = New System.Drawing.Size(657, 66)
        Me.RichTextBox1.TabIndex = 6
        Me.RichTextBox1.Text = ""
        '
        'DnsSyntaxCheckerBtn
        '
        Me.DnsSyntaxCheckerBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DnsSyntaxCheckerBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.DnsSyntaxCheckerBtn.Location = New System.Drawing.Point(492, 280)
        Me.DnsSyntaxCheckerBtn.Name = "DnsSyntaxCheckerBtn"
        Me.DnsSyntaxCheckerBtn.Size = New System.Drawing.Size(192, 23)
        Me.DnsSyntaxCheckerBtn.TabIndex = 7
        Me.DnsSyntaxCheckerBtn.Text = "Verify DNS Address Syntax"
        Me.DnsSyntaxCheckerBtn.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Enabled = False
        Me.TextBox2.Location = New System.Drawing.Point(302, 91)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(382, 21)
        Me.TextBox2.TabIndex = 3
        '
        'TextBox3
        '
        Me.TextBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox3.Enabled = False
        Me.TextBox3.Location = New System.Drawing.Point(146, 129)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(538, 21)
        Me.TextBox3.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoEllipsis = True
        Me.Label5.Location = New System.Drawing.Point(146, 156)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(538, 31)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "By default, this adapter will use the same domain suffix you specified above. You" & _
    " can change it later"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(302, 63)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(382, 21)
        Me.ComboBox1.TabIndex = 2
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(64, 92)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(201, 17)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.Text = "Specify a network adapter manually:"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoEllipsis = True
        Me.Label7.Location = New System.Drawing.Point(24, 280)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(461, 33)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "The address in the first line will be the primary DNS server address, and any oth" & _
    "er addresses will become alternative server addresses. You can put both IPv4 and" & _
    " IPv6 addresses."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(24, 192)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(280, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "DNS Server Addresses (put each address in its own line):"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(24, 132)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(116, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Primary Domain Suffix:"
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(64, 64)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(232, 17)
        Me.RadioButton1.TabIndex = 1
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Pick from a network adapter in this system:"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(24, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(81, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Interface Alias:"
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(150, 54)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(445, 21)
        Me.TextBox1.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoEllipsis = True
        Me.Label2.Location = New System.Drawing.Point(150, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(591, 31)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "This domain suffix will be added to the list of DNS suffixes. You can add more to" & _
    " the list of suffixes later."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Primary Domain Suffix:"
        '
        'DNSConfigHeader
        '
        Me.DNSConfigHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DNSConfigHeader.AutoEllipsis = True
        Me.DNSConfigHeader.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DNSConfigHeader.Location = New System.Drawing.Point(24, 16)
        Me.DNSConfigHeader.Name = "DNSConfigHeader"
        Me.DNSConfigHeader.Size = New System.Drawing.Size(658, 23)
        Me.DNSConfigHeader.TabIndex = 3
        Me.DNSConfigHeader.Text = "Configure DNS settings for network adapters"
        '
        'DSDomainConfigPanel
        '
        Me.DSDomainConfigPanel.Controls.Add(Me.TableLayoutPanel2)
        Me.DSDomainConfigPanel.Controls.Add(Me.DomainAutoUserPanel)
        Me.DSDomainConfigPanel.Controls.Add(Me.RadioButton4)
        Me.DSDomainConfigPanel.Controls.Add(Me.RadioButton3)
        Me.DSDomainConfigPanel.Controls.Add(Me.DSNoDomainPanel)
        Me.DSDomainConfigPanel.Controls.Add(Me.TextBox6)
        Me.DSDomainConfigPanel.Controls.Add(Me.Label12)
        Me.DSDomainConfigPanel.Controls.Add(Me.Label10)
        Me.DSDomainConfigPanel.Controls.Add(Me.TextBox5)
        Me.DSDomainConfigPanel.Controls.Add(Me.Label9)
        Me.DSDomainConfigPanel.Controls.Add(Me.TextBox4)
        Me.DSDomainConfigPanel.Controls.Add(Me.Label8)
        Me.DSDomainConfigPanel.Controls.Add(Me.DSDomainConfigHeader)
        Me.DSDomainConfigPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DSDomainConfigPanel.Location = New System.Drawing.Point(0, 0)
        Me.DSDomainConfigPanel.Name = "DSDomainConfigPanel"
        Me.DSDomainConfigPanel.Size = New System.Drawing.Size(784, 449)
        Me.DSDomainConfigPanel.TabIndex = 1
        Me.DSDomainConfigPanel.Visible = False
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Label13, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label14, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.AddsUpnPathText, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.AddsNtLogonPathText, 1, 1)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(91, 203)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(591, 42)
        Me.TableLayoutPanel2.TabIndex = 12
        '
        'Label13
        '
        Me.Label13.AutoEllipsis = True
        Me.Label13.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label13.Location = New System.Drawing.Point(3, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(289, 21)
        Me.Label13.TabIndex = 8
        Me.Label13.Text = "User Principal Name (Windows 2000):"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label14
        '
        Me.Label14.AutoEllipsis = True
        Me.Label14.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label14.Location = New System.Drawing.Point(3, 21)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(289, 21)
        Me.Label14.TabIndex = 8
        Me.Label14.Text = "Logon Path (pre-Windows 2000):"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AddsUpnPathText
        '
        Me.AddsUpnPathText.AutoEllipsis = True
        Me.AddsUpnPathText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AddsUpnPathText.Location = New System.Drawing.Point(298, 0)
        Me.AddsUpnPathText.Name = "AddsUpnPathText"
        Me.AddsUpnPathText.Size = New System.Drawing.Size(290, 21)
        Me.AddsUpnPathText.TabIndex = 8
        Me.AddsUpnPathText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'AddsNtLogonPathText
        '
        Me.AddsNtLogonPathText.AutoEllipsis = True
        Me.AddsNtLogonPathText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AddsNtLogonPathText.Location = New System.Drawing.Point(298, 21)
        Me.AddsNtLogonPathText.Name = "AddsNtLogonPathText"
        Me.AddsNtLogonPathText.Size = New System.Drawing.Size(290, 21)
        Me.AddsNtLogonPathText.TabIndex = 8
        Me.AddsNtLogonPathText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DomainAutoUserPanel
        '
        Me.DomainAutoUserPanel.Controls.Add(Me.ComboBox3)
        Me.DomainAutoUserPanel.Controls.Add(Me.ComboBox2)
        Me.DomainAutoUserPanel.Controls.Add(Me.Label16)
        Me.DomainAutoUserPanel.Controls.Add(Me.Label15)
        Me.DomainAutoUserPanel.Location = New System.Drawing.Point(117, 132)
        Me.DomainAutoUserPanel.Name = "DomainAutoUserPanel"
        Me.DomainAutoUserPanel.Size = New System.Drawing.Size(565, 32)
        Me.DomainAutoUserPanel.TabIndex = 11
        '
        'ComboBox3
        '
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Location = New System.Drawing.Point(348, 5)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(209, 21)
        Me.ComboBox3.TabIndex = 10
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(40, 5)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(263, 21)
        Me.ComboBox2.TabIndex = 10
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(309, 8)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(33, 13)
        Me.Label16.TabIndex = 5
        Me.Label16.Text = "User:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(7, 8)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(26, 13)
        Me.Label15.TabIndex = 5
        Me.Label15.Text = "OU:"
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(103, 171)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(142, 17)
        Me.RadioButton4.TabIndex = 9
        Me.RadioButton4.Text = "Specify a user manually:"
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Checked = True
        Me.RadioButton3.Location = New System.Drawing.Point(103, 109)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(216, 17)
        Me.RadioButton3.TabIndex = 9
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "Pick the following user from the domain:"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'DSNoDomainPanel
        '
        Me.DSNoDomainPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DSNoDomainPanel.Controls.Add(Me.PictureBox2)
        Me.DSNoDomainPanel.Controls.Add(Me.Label11)
        Me.DSNoDomainPanel.Location = New System.Drawing.Point(91, 295)
        Me.DSNoDomainPanel.Name = "DSNoDomainPanel"
        Me.DSNoDomainPanel.Size = New System.Drawing.Size(579, 61)
        Me.DSNoDomainPanel.TabIndex = 7
        Me.DSNoDomainPanel.Visible = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.DISMTools.My.Resources.Resources.caution
        Me.PictureBox2.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'Label11
        '
        Me.Label11.AutoEllipsis = True
        Me.Label11.Location = New System.Drawing.Point(50, 12)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(517, 32)
        Me.Label11.TabIndex = 5
        Me.Label11.Text = "A domain name could not be obtained automatically because this device does not be" & _
    "long to a domain."
        '
        'TextBox6
        '
        Me.TextBox6.Location = New System.Drawing.Point(211, 257)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.TextBox6.Size = New System.Drawing.Size(459, 21)
        Me.TextBox6.TabIndex = 6
        '
        'Label12
        '
        Me.Label12.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoEllipsis = True
        Me.Label12.Location = New System.Drawing.Point(88, 367)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(582, 65)
        Me.Label12.TabIndex = 5
        Me.Label12.Text = resources.GetString("Label12.Text")
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(88, 260)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(57, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Password:"
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(251, 170)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(419, 21)
        Me.TextBox5.TabIndex = 6
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(88, 84)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(75, 13)
        Me.Label9.TabIndex = 5
        Me.Label9.Text = "User Account:"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(211, 54)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(459, 21)
        Me.TextBox4.TabIndex = 6
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(88, 57)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(76, 13)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "Domain Name:"
        '
        'DSDomainConfigHeader
        '
        Me.DSDomainConfigHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DSDomainConfigHeader.AutoEllipsis = True
        Me.DSDomainConfigHeader.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DSDomainConfigHeader.Location = New System.Drawing.Point(24, 16)
        Me.DSDomainConfigHeader.Name = "DSDomainConfigHeader"
        Me.DSDomainConfigHeader.Size = New System.Drawing.Size(658, 23)
        Me.DSDomainConfigHeader.TabIndex = 4
        Me.DSDomainConfigHeader.Text = "Provide domain and authentication information"
        '
        'HeaderPanel
        '
        Me.HeaderPanel.Controls.Add(Me.DS7_Description)
        Me.HeaderPanel.Controls.Add(Me.PictureBox1)
        Me.HeaderPanel.Controls.Add(Me.DS7_Header)
        Me.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.HeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.Size = New System.Drawing.Size(784, 72)
        Me.HeaderPanel.TabIndex = 3
        '
        'DS7_Description
        '
        Me.DS7_Description.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DS7_Description.AutoEllipsis = True
        Me.DS7_Description.Location = New System.Drawing.Point(27, 34)
        Me.DS7_Description.Name = "DS7_Description"
        Me.DS7_Description.Size = New System.Drawing.Size(643, 28)
        Me.DS7_Description.TabIndex = 5
        Me.DS7_Description.Text = "This wizard helps you set up your unattended answer file to make a device join a " & _
    "domain powered by Active Directory Domain Services (AD DS)."
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = Global.DISMTools.My.Resources.Resources.ad_ds
        Me.PictureBox1.Location = New System.Drawing.Point(724, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'DS7_Header
        '
        Me.DS7_Header.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DS7_Header.AutoEllipsis = True
        Me.DS7_Header.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DS7_Header.Location = New System.Drawing.Point(12, 9)
        Me.DS7_Header.Name = "DS7_Header"
        Me.DS7_Header.Size = New System.Drawing.Size(658, 23)
        Me.DS7_Header.TabIndex = 3
        Me.DS7_Header.Text = "Join an Active Directory domain"
        '
        'FooterContainer
        '
        Me.FooterContainer.Controls.Add(Me.ExpressPanelFooter)
        Me.FooterContainer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FooterContainer.Location = New System.Drawing.Point(0, 521)
        Me.FooterContainer.Name = "FooterContainer"
        Me.FooterContainer.Size = New System.Drawing.Size(784, 40)
        Me.FooterContainer.TabIndex = 4
        '
        'ExpressPanelFooter
        '
        Me.ExpressPanelFooter.Controls.Add(Me.DNS_Explanation_Link)
        Me.ExpressPanelFooter.Controls.Add(Me.TableLayoutPanel1)
        Me.ExpressPanelFooter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExpressPanelFooter.Location = New System.Drawing.Point(0, 0)
        Me.ExpressPanelFooter.Name = "ExpressPanelFooter"
        Me.ExpressPanelFooter.Size = New System.Drawing.Size(784, 40)
        Me.ExpressPanelFooter.TabIndex = 0
        '
        'DNS_Explanation_Link
        '
        Me.DNS_Explanation_Link.AutoSize = True
        Me.DNS_Explanation_Link.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.DNS_Explanation_Link.LinkColor = System.Drawing.Color.DodgerBlue
        Me.DNS_Explanation_Link.Location = New System.Drawing.Point(12, 14)
        Me.DNS_Explanation_Link.Name = "DNS_Explanation_Link"
        Me.DNS_Explanation_Link.Size = New System.Drawing.Size(71, 13)
        Me.DNS_Explanation_Link.TabIndex = 2
        Me.DNS_Explanation_Link.TabStop = True
        Me.DNS_Explanation_Link.Text = "What is DNS?"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Back_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Next_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Help_Button, 3, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(490, 6)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(282, 29)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'Back_Button
        '
        Me.Back_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Back_Button.Enabled = False
        Me.Back_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Back_Button.Location = New System.Drawing.Point(3, 3)
        Me.Back_Button.Name = "Back_Button"
        Me.Back_Button.Size = New System.Drawing.Size(64, 23)
        Me.Back_Button.TabIndex = 0
        Me.Back_Button.Text = "Back"
        '
        'Next_Button
        '
        Me.Next_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Next_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Next_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Next_Button.Location = New System.Drawing.Point(73, 3)
        Me.Next_Button.Name = "Next_Button"
        Me.Next_Button.Size = New System.Drawing.Size(64, 23)
        Me.Next_Button.TabIndex = 1
        Me.Next_Button.Text = "Next"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Cancel_Button.Location = New System.Drawing.Point(143, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(64, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'Help_Button
        '
        Me.Help_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Help_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Help_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Help_Button.Location = New System.Drawing.Point(214, 3)
        Me.Help_Button.Name = "Help_Button"
        Me.Help_Button.Size = New System.Drawing.Size(64, 23)
        Me.Help_Button.TabIndex = 1
        Me.Help_Button.Text = "Help"
        '
        'ADDSInitBW
        '
        Me.ADDSInitBW.WorkerReportsProgress = True
        '
        'DnsValidatorBW
        '
        Me.DnsValidatorBW.WorkerReportsProgress = True
        '
        'DnsNsLookupBtn
        '
        Me.DnsNsLookupBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.DnsNsLookupBtn.Location = New System.Drawing.Point(601, 53)
        Me.DnsNsLookupBtn.Name = "DnsNsLookupBtn"
        Me.DnsNsLookupBtn.Size = New System.Drawing.Size(140, 23)
        Me.DnsNsLookupBtn.TabIndex = 7
        Me.DnsNsLookupBtn.Text = "Test DNS resolution..."
        Me.DnsNsLookupBtn.UseVisualStyleBackColor = True
        '
        'ADDSJoinDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.ExpressPanelContainer)
        Me.Controls.Add(Me.FooterContainer)
        Me.Controls.Add(Me.HeaderPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ADDSJoinDialog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Domain Services Wizard"
        Me.ExpressPanelContainer.ResumeLayout(False)
        Me.ExperimentalPanel.ResumeLayout(False)
        Me.StepsContainer.ResumeLayout(False)
        Me.DNSConfigPanel.ResumeLayout(False)
        Me.DNSConfigPanel.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.DSDomainConfigPanel.ResumeLayout(False)
        Me.DSDomainConfigPanel.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.DomainAutoUserPanel.ResumeLayout(False)
        Me.DomainAutoUserPanel.PerformLayout()
        Me.DSNoDomainPanel.ResumeLayout(False)
        Me.DSNoDomainPanel.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FooterContainer.ResumeLayout(False)
        Me.ExpressPanelFooter.ResumeLayout(False)
        Me.ExpressPanelFooter.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ExpressPanelContainer As System.Windows.Forms.Panel
    Friend WithEvents ExperimentalPanel As System.Windows.Forms.Panel
    Friend WithEvents StepsContainer As System.Windows.Forms.Panel
    Friend WithEvents HeaderPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents DS7_Header As System.Windows.Forms.Label
    Friend WithEvents DS7_Description As System.Windows.Forms.Label
    Friend WithEvents FooterContainer As System.Windows.Forms.Panel
    Friend WithEvents ExpressPanelFooter As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Back_Button As System.Windows.Forms.Button
    Friend WithEvents Next_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Help_Button As System.Windows.Forms.Button
    Friend WithEvents DNSConfigPanel As System.Windows.Forms.Panel
    Friend WithEvents DSDomainConfigPanel As System.Windows.Forms.Panel
    Friend WithEvents DNSConfigHeader As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ADDSInitBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents DSDomainConfigHeader As System.Windows.Forms.Label
    Friend WithEvents DSNoDomainPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents DNS_Explanation_Link As System.Windows.Forms.LinkLabel
    Friend WithEvents DnsSyntaxCheckerBtn As System.Windows.Forms.Button
    Friend WithEvents DnsValidatorBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents AddsNtLogonPathText As System.Windows.Forms.Label
    Friend WithEvents AddsUpnPathText As System.Windows.Forms.Label
    Friend WithEvents DomainAutoUserPanel As System.Windows.Forms.Panel
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DnsNsLookupBtn As System.Windows.Forms.Button
End Class
