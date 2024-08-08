<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewUnattendWiz
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
        Me.components = New System.ComponentModel.Container()
        Dim TreeNode14 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Welcome")
        Dim TreeNode15 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Regional Configuration")
        Dim TreeNode16 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Basic System Configuration")
        Dim TreeNode17 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Time Zone")
        Dim TreeNode18 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Disk Configuration")
        Dim TreeNode19 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Product Key")
        Dim TreeNode20 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("User Accounts")
        Dim TreeNode21 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Virtual Machine Support")
        Dim TreeNode22 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Wireless Networking")
        Dim TreeNode23 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("System Telemetry")
        Dim TreeNode24 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Post-Installation Scripts")
        Dim TreeNode25 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Component Settings")
        Dim TreeNode26 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Finish")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewUnattendWiz))
        Me.SidePanel = New System.Windows.Forms.Panel()
        Me.ExpressModeSteps = New System.Windows.Forms.Panel()
        Me.StepsTreeView = New System.Windows.Forms.TreeView()
        Me.EditorPanelTrigger = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.ExpressPanelTrigger = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ExpressPanelContainer = New System.Windows.Forms.Panel()
        Me.ExperimentalPanel = New System.Windows.Forms.Panel()
        Me.StepsContainer = New System.Windows.Forms.Panel()
        Me.WelcomePanel = New System.Windows.Forms.Panel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.WelcomeHeader = New System.Windows.Forms.Label()
        Me.WelcomeDesc = New System.Windows.Forms.Label()
        Me.FinishPanel = New System.Windows.Forms.Panel()
        Me.LinkLabel4 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.FinishHeader = New System.Windows.Forms.Label()
        Me.UnattendProgressPanel = New System.Windows.Forms.Panel()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.UnattendProgressHeader = New System.Windows.Forms.Label()
        Me.FinalReviewPanel = New System.Windows.Forms.Panel()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.CheckBox17 = New System.Windows.Forms.CheckBox()
        Me.TextBox13 = New System.Windows.Forms.TextBox()
        Me.FinalReviewHeader = New System.Windows.Forms.Label()
        Me.ComponentPanel = New System.Windows.Forms.Panel()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.ComponentHeader = New System.Windows.Forms.Label()
        Me.PostInstallPanel = New System.Windows.Forms.Panel()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.PostInstallHeader = New System.Windows.Forms.Label()
        Me.SystemTelemetryPanel = New System.Windows.Forms.Panel()
        Me.TelemetryOptionsPanel = New System.Windows.Forms.Panel()
        Me.RadioButton27 = New System.Windows.Forms.RadioButton()
        Me.RadioButton26 = New System.Windows.Forms.RadioButton()
        Me.CheckBox16 = New System.Windows.Forms.CheckBox()
        Me.SystemTelemetryHeader = New System.Windows.Forms.Label()
        Me.NetworkConnectionPanel = New System.Windows.Forms.Panel()
        Me.ManualNetworkConfigPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.RadioButton25 = New System.Windows.Forms.RadioButton()
        Me.WirelessNetworkSettingsPanel = New System.Windows.Forms.Panel()
        Me.ComboBox13 = New System.Windows.Forms.ComboBox()
        Me.TextBox10 = New System.Windows.Forms.TextBox()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.CheckBox15 = New System.Windows.Forms.CheckBox()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.RadioButton30 = New System.Windows.Forms.RadioButton()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.CheckBox14 = New System.Windows.Forms.CheckBox()
        Me.NetworkConnectionHeader = New System.Windows.Forms.Label()
        Me.VirtualMachinePanel = New System.Windows.Forms.Panel()
        Me.VMProviderPanel = New System.Windows.Forms.Panel()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.ComboBox8 = New System.Windows.Forms.ComboBox()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.RadioButton24 = New System.Windows.Forms.RadioButton()
        Me.RadioButton23 = New System.Windows.Forms.RadioButton()
        Me.VirtualMachineHeader = New System.Windows.Forms.Label()
        Me.AccountLockdownPanel = New System.Windows.Forms.Panel()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.EnabledAccountLockdownPanel = New System.Windows.Forms.Panel()
        Me.AccountLockdownParametersPanel = New System.Windows.Forms.Panel()
        Me.NumericUpDown8 = New System.Windows.Forms.NumericUpDown()
        Me.NumericUpDown7 = New System.Windows.Forms.NumericUpDown()
        Me.NumericUpDown6 = New System.Windows.Forms.NumericUpDown()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.RadioButton22 = New System.Windows.Forms.RadioButton()
        Me.RadioButton21 = New System.Windows.Forms.RadioButton()
        Me.CheckBox13 = New System.Windows.Forms.CheckBox()
        Me.AccountLockdownHeader = New System.Windows.Forms.Label()
        Me.PWExpirationPanel = New System.Windows.Forms.Panel()
        Me.AutoExpirationPanel = New System.Windows.Forms.Panel()
        Me.TimedExpirationPanel = New System.Windows.Forms.Panel()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.NumericUpDown5 = New System.Windows.Forms.NumericUpDown()
        Me.RadioButton20 = New System.Windows.Forms.RadioButton()
        Me.RadioButton19 = New System.Windows.Forms.RadioButton()
        Me.RadioButton18 = New System.Windows.Forms.RadioButton()
        Me.RadioButton17 = New System.Windows.Forms.RadioButton()
        Me.PWExpirationHeader = New System.Windows.Forms.Label()
        Me.UserAccountPanel = New System.Windows.Forms.Panel()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.CheckBox6 = New System.Windows.Forms.CheckBox()
        Me.ManualAccountPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.UserAccountListing = New System.Windows.Forms.Panel()
        Me.AccountsPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.ComboBox12 = New System.Windows.Forms.ComboBox()
        Me.ComboBox11 = New System.Windows.Forms.ComboBox()
        Me.ComboBox10 = New System.Windows.Forms.ComboBox()
        Me.ComboBox9 = New System.Windows.Forms.ComboBox()
        Me.TextBox18 = New System.Windows.Forms.TextBox()
        Me.TextBox17 = New System.Windows.Forms.TextBox()
        Me.TextBox15 = New System.Windows.Forms.TextBox()
        Me.TextBox14 = New System.Windows.Forms.TextBox()
        Me.TextBox12 = New System.Windows.Forms.TextBox()
        Me.TextBox11 = New System.Windows.Forms.TextBox()
        Me.TextBox9 = New System.Windows.Forms.TextBox()
        Me.TextBox8 = New System.Windows.Forms.TextBox()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.CheckBox8 = New System.Windows.Forms.CheckBox()
        Me.CheckBox9 = New System.Windows.Forms.CheckBox()
        Me.CheckBox10 = New System.Windows.Forms.CheckBox()
        Me.CheckBox11 = New System.Windows.Forms.CheckBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.ComboBox7 = New System.Windows.Forms.ComboBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.AutoLogonSettingsPanel = New System.Windows.Forms.Panel()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.RadioButton16 = New System.Windows.Forms.RadioButton()
        Me.RadioButton15 = New System.Windows.Forms.RadioButton()
        Me.CheckBox12 = New System.Windows.Forms.CheckBox()
        Me.CheckBox7 = New System.Windows.Forms.CheckBox()
        Me.FillerLabel2 = New System.Windows.Forms.Label()
        Me.UserAccountHeader = New System.Windows.Forms.Label()
        Me.ProductKeyPanel = New System.Windows.Forms.Panel()
        Me.ManualKeyPanel = New System.Windows.Forms.Panel()
        Me.KeyVerifyWarningPanel = New System.Windows.Forms.Panel()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.GenericKeyPanel = New System.Windows.Forms.Panel()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.ComboBox6 = New System.Windows.Forms.ComboBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.RadioButton14 = New System.Windows.Forms.RadioButton()
        Me.RadioButton13 = New System.Windows.Forms.RadioButton()
        Me.ProductKeyHeader = New System.Windows.Forms.Label()
        Me.DiskConfigurationPanel = New System.Windows.Forms.Panel()
        Me.ManualPartPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.RadioButton5 = New System.Windows.Forms.RadioButton()
        Me.AutoDiskConfigPanel = New System.Windows.Forms.Panel()
        Me.WindowsREPanel = New System.Windows.Forms.Panel()
        Me.RESizePanel = New System.Windows.Forms.Panel()
        Me.NumericUpDown2 = New System.Windows.Forms.NumericUpDown()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.RadioButton10 = New System.Windows.Forms.RadioButton()
        Me.RadioButton9 = New System.Windows.Forms.RadioButton()
        Me.CheckBox5 = New System.Windows.Forms.CheckBox()
        Me.PartTablePanel = New System.Windows.Forms.Panel()
        Me.ESPPanel = New System.Windows.Forms.Panel()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.RadioButton8 = New System.Windows.Forms.RadioButton()
        Me.RadioButton7 = New System.Windows.Forms.RadioButton()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.RadioButton6 = New System.Windows.Forms.RadioButton()
        Me.DiskPartPanel = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Scintilla2 = New ScintillaNET.Scintilla()
        Me.DPActionsPanel = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ManualInstallPanel = New System.Windows.Forms.Panel()
        Me.NumericUpDown4 = New System.Windows.Forms.NumericUpDown()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.NumericUpDown3 = New System.Windows.Forms.NumericUpDown()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.RadioButton12 = New System.Windows.Forms.RadioButton()
        Me.RadioButton11 = New System.Windows.Forms.RadioButton()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.FillerLabel = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.CheckBox4 = New System.Windows.Forms.CheckBox()
        Me.DiskConfigurationHeader = New System.Windows.Forms.Label()
        Me.TimeZonePanel = New System.Windows.Forms.Panel()
        Me.TimeZoneSettings = New System.Windows.Forms.Panel()
        Me.CurrentTimeSelTZ = New System.Windows.Forms.Label()
        Me.CurrentTimeUTC = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.ComboBox5 = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.TimeZoneHeader = New System.Windows.Forms.Label()
        Me.SysConfigPanel = New System.Windows.Forms.Panel()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.ComputerNamePanel = New System.Windows.Forms.Panel()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.WinSVSettingsPanel = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.SysConfigHeader = New System.Windows.Forms.Label()
        Me.RegionalSettingsPanel = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.RegionalSettings = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ComboBox4 = New System.Windows.Forms.ComboBox()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RegionalSettingsHeader = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.EditorPanelContainer = New System.Windows.Forms.Panel()
        Me.Scintilla1 = New ScintillaNET.Scintilla()
        Me.DarkToolStrip1 = New DarkUI.Controls.DarkToolStrip()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.FontFamilyTSCB = New System.Windows.Forms.ToolStripComboBox()
        Me.FontSizeTSCB = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton5 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton6 = New System.Windows.Forms.ToolStripButton()
        Me.HeaderPanel = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.FooterContainer = New System.Windows.Forms.Panel()
        Me.ExpressPanelFooter = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Back_Button = New System.Windows.Forms.Button()
        Me.Next_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Help_Button = New System.Windows.Forms.Button()
        Me.EditorPanelFooter = New System.Windows.Forms.Panel()
        Me.TimeZonePageTimer = New System.Windows.Forms.Timer(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.UnattendGeneratorBW = New System.ComponentModel.BackgroundWorker()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.SidePanel.SuspendLayout()
        Me.ExpressModeSteps.SuspendLayout()
        Me.EditorPanelTrigger.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ExpressPanelTrigger.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ExpressPanelContainer.SuspendLayout()
        Me.ExperimentalPanel.SuspendLayout()
        Me.StepsContainer.SuspendLayout()
        Me.WelcomePanel.SuspendLayout()
        Me.FinishPanel.SuspendLayout()
        Me.UnattendProgressPanel.SuspendLayout()
        Me.FinalReviewPanel.SuspendLayout()
        Me.ComponentPanel.SuspendLayout()
        Me.PostInstallPanel.SuspendLayout()
        Me.SystemTelemetryPanel.SuspendLayout()
        Me.TelemetryOptionsPanel.SuspendLayout()
        Me.NetworkConnectionPanel.SuspendLayout()
        Me.ManualNetworkConfigPanel.SuspendLayout()
        Me.WirelessNetworkSettingsPanel.SuspendLayout()
        Me.VirtualMachinePanel.SuspendLayout()
        Me.VMProviderPanel.SuspendLayout()
        Me.AccountLockdownPanel.SuspendLayout()
        Me.EnabledAccountLockdownPanel.SuspendLayout()
        Me.AccountLockdownParametersPanel.SuspendLayout()
        CType(Me.NumericUpDown8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PWExpirationPanel.SuspendLayout()
        Me.AutoExpirationPanel.SuspendLayout()
        Me.TimedExpirationPanel.SuspendLayout()
        CType(Me.NumericUpDown5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UserAccountPanel.SuspendLayout()
        Me.ManualAccountPanel.SuspendLayout()
        Me.UserAccountListing.SuspendLayout()
        Me.AccountsPanel.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.AutoLogonSettingsPanel.SuspendLayout()
        Me.ProductKeyPanel.SuspendLayout()
        Me.ManualKeyPanel.SuspendLayout()
        Me.KeyVerifyWarningPanel.SuspendLayout()
        Me.GenericKeyPanel.SuspendLayout()
        Me.DiskConfigurationPanel.SuspendLayout()
        Me.ManualPartPanel.SuspendLayout()
        Me.AutoDiskConfigPanel.SuspendLayout()
        Me.WindowsREPanel.SuspendLayout()
        Me.RESizePanel.SuspendLayout()
        CType(Me.NumericUpDown2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PartTablePanel.SuspendLayout()
        Me.ESPPanel.SuspendLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DiskPartPanel.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.DPActionsPanel.SuspendLayout()
        Me.ManualInstallPanel.SuspendLayout()
        CType(Me.NumericUpDown4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TimeZonePanel.SuspendLayout()
        Me.TimeZoneSettings.SuspendLayout()
        Me.SysConfigPanel.SuspendLayout()
        Me.ComputerNamePanel.SuspendLayout()
        Me.WinSVSettingsPanel.SuspendLayout()
        Me.RegionalSettingsPanel.SuspendLayout()
        Me.RegionalSettings.SuspendLayout()
        Me.EditorPanelContainer.SuspendLayout()
        Me.DarkToolStrip1.SuspendLayout()
        Me.HeaderPanel.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FooterContainer.SuspendLayout()
        Me.ExpressPanelFooter.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SidePanel
        '
        Me.SidePanel.BackColor = System.Drawing.Color.White
        Me.SidePanel.Controls.Add(Me.ExpressModeSteps)
        Me.SidePanel.Controls.Add(Me.EditorPanelTrigger)
        Me.SidePanel.Controls.Add(Me.ExpressPanelTrigger)
        Me.SidePanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.SidePanel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SidePanel.Location = New System.Drawing.Point(0, 0)
        Me.SidePanel.Name = "SidePanel"
        Me.SidePanel.Size = New System.Drawing.Size(256, 561)
        Me.SidePanel.TabIndex = 0
        '
        'ExpressModeSteps
        '
        Me.ExpressModeSteps.Controls.Add(Me.StepsTreeView)
        Me.ExpressModeSteps.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExpressModeSteps.Location = New System.Drawing.Point(0, 40)
        Me.ExpressModeSteps.Name = "ExpressModeSteps"
        Me.ExpressModeSteps.Padding = New System.Windows.Forms.Padding(6)
        Me.ExpressModeSteps.Size = New System.Drawing.Size(256, 481)
        Me.ExpressModeSteps.TabIndex = 2
        '
        'StepsTreeView
        '
        Me.StepsTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.StepsTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StepsTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText
        Me.StepsTreeView.Enabled = False
        Me.StepsTreeView.HideSelection = False
        Me.StepsTreeView.ItemHeight = 24
        Me.StepsTreeView.Location = New System.Drawing.Point(6, 6)
        Me.StepsTreeView.Name = "StepsTreeView"
        TreeNode14.Name = "Nodo0"
        TreeNode14.Text = "Welcome"
        TreeNode15.Name = "Nodo1"
        TreeNode15.Text = "Regional Configuration"
        TreeNode16.Name = "Nodo2"
        TreeNode16.Text = "Basic System Configuration"
        TreeNode17.Name = "Nodo3"
        TreeNode17.Text = "Time Zone"
        TreeNode18.Name = "Nodo4"
        TreeNode18.Text = "Disk Configuration"
        TreeNode19.Name = "Nodo5"
        TreeNode19.Text = "Product Key"
        TreeNode20.Name = "Nodo6"
        TreeNode20.Text = "User Accounts"
        TreeNode21.Name = "Nodo9"
        TreeNode21.Text = "Virtual Machine Support"
        TreeNode22.Name = "Nodo10"
        TreeNode22.Text = "Wireless Networking"
        TreeNode23.Name = "Nodo11"
        TreeNode23.Text = "System Telemetry"
        TreeNode24.Name = "Nodo12"
        TreeNode24.Text = "Post-Installation Scripts"
        TreeNode25.Name = "Nodo13"
        TreeNode25.Text = "Component Settings"
        TreeNode26.Name = "Nodo14"
        TreeNode26.Text = "Finish"
        Me.StepsTreeView.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode14, TreeNode15, TreeNode16, TreeNode17, TreeNode18, TreeNode19, TreeNode20, TreeNode21, TreeNode22, TreeNode23, TreeNode24, TreeNode25, TreeNode26})
        Me.StepsTreeView.ShowLines = False
        Me.StepsTreeView.ShowPlusMinus = False
        Me.StepsTreeView.ShowRootLines = False
        Me.StepsTreeView.Size = New System.Drawing.Size(244, 469)
        Me.StepsTreeView.TabIndex = 0
        '
        'EditorPanelTrigger
        '
        Me.EditorPanelTrigger.Controls.Add(Me.Label2)
        Me.EditorPanelTrigger.Controls.Add(Me.PictureBox2)
        Me.EditorPanelTrigger.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.EditorPanelTrigger.Location = New System.Drawing.Point(0, 521)
        Me.EditorPanelTrigger.Name = "EditorPanelTrigger"
        Me.EditorPanelTrigger.Size = New System.Drawing.Size(256, 40)
        Me.EditorPanelTrigger.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(42, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Editor mode"
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.DISMTools.My.Resources.Resources.editor_mode
        Me.PictureBox2.Location = New System.Drawing.Point(12, 8)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'ExpressPanelTrigger
        '
        Me.ExpressPanelTrigger.BackColor = System.Drawing.SystemColors.Highlight
        Me.ExpressPanelTrigger.Controls.Add(Me.Label1)
        Me.ExpressPanelTrigger.Controls.Add(Me.PictureBox1)
        Me.ExpressPanelTrigger.Dock = System.Windows.Forms.DockStyle.Top
        Me.ExpressPanelTrigger.ForeColor = System.Drawing.Color.White
        Me.ExpressPanelTrigger.Location = New System.Drawing.Point(0, 0)
        Me.ExpressPanelTrigger.Name = "ExpressPanelTrigger"
        Me.ExpressPanelTrigger.Size = New System.Drawing.Size(256, 40)
        Me.ExpressPanelTrigger.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(42, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Express mode"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DISMTools.My.Resources.Resources.express_mode_select
        Me.PictureBox1.Location = New System.Drawing.Point(12, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'ExpressPanelContainer
        '
        Me.ExpressPanelContainer.Controls.Add(Me.ExperimentalPanel)
        Me.ExpressPanelContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExpressPanelContainer.Location = New System.Drawing.Point(256, 72)
        Me.ExpressPanelContainer.Name = "ExpressPanelContainer"
        Me.ExpressPanelContainer.Size = New System.Drawing.Size(752, 449)
        Me.ExpressPanelContainer.TabIndex = 1
        '
        'ExperimentalPanel
        '
        Me.ExperimentalPanel.Controls.Add(Me.StepsContainer)
        Me.ExperimentalPanel.Controls.Add(Me.Label5)
        Me.ExperimentalPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExperimentalPanel.Location = New System.Drawing.Point(0, 0)
        Me.ExperimentalPanel.Name = "ExperimentalPanel"
        Me.ExperimentalPanel.Size = New System.Drawing.Size(752, 449)
        Me.ExperimentalPanel.TabIndex = 2
        '
        'StepsContainer
        '
        Me.StepsContainer.Controls.Add(Me.FinishPanel)
        Me.StepsContainer.Controls.Add(Me.UnattendProgressPanel)
        Me.StepsContainer.Controls.Add(Me.FinalReviewPanel)
        Me.StepsContainer.Controls.Add(Me.ComponentPanel)
        Me.StepsContainer.Controls.Add(Me.PostInstallPanel)
        Me.StepsContainer.Controls.Add(Me.SystemTelemetryPanel)
        Me.StepsContainer.Controls.Add(Me.NetworkConnectionPanel)
        Me.StepsContainer.Controls.Add(Me.VirtualMachinePanel)
        Me.StepsContainer.Controls.Add(Me.AccountLockdownPanel)
        Me.StepsContainer.Controls.Add(Me.PWExpirationPanel)
        Me.StepsContainer.Controls.Add(Me.UserAccountPanel)
        Me.StepsContainer.Controls.Add(Me.ProductKeyPanel)
        Me.StepsContainer.Controls.Add(Me.DiskConfigurationPanel)
        Me.StepsContainer.Controls.Add(Me.TimeZonePanel)
        Me.StepsContainer.Controls.Add(Me.SysConfigPanel)
        Me.StepsContainer.Controls.Add(Me.RegionalSettingsPanel)
        Me.StepsContainer.Controls.Add(Me.WelcomePanel)
        Me.StepsContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StepsContainer.Location = New System.Drawing.Point(0, 0)
        Me.StepsContainer.Name = "StepsContainer"
        Me.StepsContainer.Size = New System.Drawing.Size(752, 449)
        Me.StepsContainer.TabIndex = 1
        '
        'WelcomePanel
        '
        Me.WelcomePanel.Controls.Add(Me.LinkLabel1)
        Me.WelcomePanel.Controls.Add(Me.WelcomeHeader)
        Me.WelcomePanel.Controls.Add(Me.WelcomeDesc)
        Me.WelcomePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WelcomePanel.Location = New System.Drawing.Point(0, 0)
        Me.WelcomePanel.Name = "WelcomePanel"
        Me.WelcomePanel.Size = New System.Drawing.Size(752, 449)
        Me.WelcomePanel.TabIndex = 0
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoEllipsis = True
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel1.Location = New System.Drawing.Point(77, 339)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(436, 13)
        Me.LinkLabel1.TabIndex = 2
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Answer file generator (online version)"
        '
        'WelcomeHeader
        '
        Me.WelcomeHeader.AutoEllipsis = True
        Me.WelcomeHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.WelcomeHeader.Location = New System.Drawing.Point(16, 17)
        Me.WelcomeHeader.Name = "WelcomeHeader"
        Me.WelcomeHeader.Size = New System.Drawing.Size(708, 51)
        Me.WelcomeHeader.TabIndex = 14
        Me.WelcomeHeader.Text = "Welcome to the unattended answer file creation wizard"
        '
        'WelcomeDesc
        '
        Me.WelcomeDesc.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.WelcomeDesc.AutoEllipsis = True
        Me.WelcomeDesc.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WelcomeDesc.Location = New System.Drawing.Point(78, 86)
        Me.WelcomeDesc.Name = "WelcomeDesc"
        Me.WelcomeDesc.Size = New System.Drawing.Size(596, 188)
        Me.WelcomeDesc.TabIndex = 0
        Me.WelcomeDesc.Text = resources.GetString("WelcomeDesc.Text")
        '
        'FinishPanel
        '
        Me.FinishPanel.Controls.Add(Me.LinkLabel4)
        Me.FinishPanel.Controls.Add(Me.LinkLabel3)
        Me.FinishPanel.Controls.Add(Me.LinkLabel2)
        Me.FinishPanel.Controls.Add(Me.Label58)
        Me.FinishPanel.Controls.Add(Me.FinishHeader)
        Me.FinishPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FinishPanel.Location = New System.Drawing.Point(0, 0)
        Me.FinishPanel.Name = "FinishPanel"
        Me.FinishPanel.Size = New System.Drawing.Size(752, 449)
        Me.FinishPanel.TabIndex = 16
        '
        'LinkLabel4
        '
        Me.LinkLabel4.AutoSize = True
        Me.LinkLabel4.Enabled = False
        Me.LinkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel4.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel4.Location = New System.Drawing.Point(157, 236)
        Me.LinkLabel4.Name = "LinkLabel4"
        Me.LinkLabel4.Size = New System.Drawing.Size(160, 13)
        Me.LinkLabel4.TabIndex = 15
        Me.LinkLabel4.TabStop = True
        Me.LinkLabel4.Text = "Apply unattended answer file..."
        '
        'LinkLabel3
        '
        Me.LinkLabel3.AutoSize = True
        Me.LinkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel3.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel3.Location = New System.Drawing.Point(157, 209)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(141, 13)
        Me.LinkLabel3.TabIndex = 15
        Me.LinkLabel3.TabStop = True
        Me.LinkLabel3.Text = "Open the location of the file"
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel2.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel2.Location = New System.Drawing.Point(157, 183)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(136, 13)
        Me.LinkLabel2.TabIndex = 15
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "Create another answer file"
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(103, 84)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(516, 13)
        Me.Label58.TabIndex = 14
        Me.Label58.Text = "The unattended answer file has been created at the location you specified. What d" & _
    "o you want to do now?"
        '
        'FinishHeader
        '
        Me.FinishHeader.AutoEllipsis = True
        Me.FinishHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.FinishHeader.Location = New System.Drawing.Point(16, 17)
        Me.FinishHeader.Name = "FinishHeader"
        Me.FinishHeader.Size = New System.Drawing.Size(708, 51)
        Me.FinishHeader.TabIndex = 13
        Me.FinishHeader.Text = "Congratulations! You have finished"
        '
        'UnattendProgressPanel
        '
        Me.UnattendProgressPanel.Controls.Add(Me.ProgressBar1)
        Me.UnattendProgressPanel.Controls.Add(Me.Label57)
        Me.UnattendProgressPanel.Controls.Add(Me.Label56)
        Me.UnattendProgressPanel.Controls.Add(Me.UnattendProgressHeader)
        Me.UnattendProgressPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UnattendProgressPanel.Location = New System.Drawing.Point(0, 0)
        Me.UnattendProgressPanel.Name = "UnattendProgressPanel"
        Me.UnattendProgressPanel.Size = New System.Drawing.Size(752, 449)
        Me.UnattendProgressPanel.TabIndex = 15
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(96, 86)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(563, 23)
        Me.ProgressBar1.TabIndex = 14
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(93, 118)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(183, 13)
        Me.Label57.TabIndex = 13
        Me.Label57.Text = "Please wait - this can take some time"
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(93, 66)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(53, 13)
        Me.Label56.TabIndex = 13
        Me.Label56.Text = "Progress:"
        '
        'UnattendProgressHeader
        '
        Me.UnattendProgressHeader.AutoEllipsis = True
        Me.UnattendProgressHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.UnattendProgressHeader.Location = New System.Drawing.Point(16, 17)
        Me.UnattendProgressHeader.Name = "UnattendProgressHeader"
        Me.UnattendProgressHeader.Size = New System.Drawing.Size(708, 51)
        Me.UnattendProgressHeader.TabIndex = 12
        Me.UnattendProgressHeader.Text = "Please wait while your unattended answer file is being created..."
        '
        'FinalReviewPanel
        '
        Me.FinalReviewPanel.Controls.Add(Me.Label54)
        Me.FinalReviewPanel.Controls.Add(Me.CheckBox17)
        Me.FinalReviewPanel.Controls.Add(Me.TextBox13)
        Me.FinalReviewPanel.Controls.Add(Me.FinalReviewHeader)
        Me.FinalReviewPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FinalReviewPanel.Location = New System.Drawing.Point(0, 0)
        Me.FinalReviewPanel.Name = "FinalReviewPanel"
        Me.FinalReviewPanel.Size = New System.Drawing.Size(752, 449)
        Me.FinalReviewPanel.TabIndex = 14
        '
        'Label54
        '
        Me.Label54.AutoEllipsis = True
        Me.Label54.Location = New System.Drawing.Point(50, 398)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(648, 35)
        Me.Label54.TabIndex = 15
        Me.Label54.Text = "If something is not right, you will need to go back to that page in order to chan" & _
    "ge the setting. Do not worry: other settings will be kept intact"
        '
        'CheckBox17
        '
        Me.CheckBox17.Appearance = System.Windows.Forms.Appearance.Button
        Me.CheckBox17.AutoSize = True
        Me.CheckBox17.Checked = True
        Me.CheckBox17.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox17.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox17.Location = New System.Drawing.Point(628, 372)
        Me.CheckBox17.Name = "CheckBox17"
        Me.CheckBox17.Size = New System.Drawing.Size(70, 23)
        Me.CheckBox17.TabIndex = 14
        Me.CheckBox17.Text = "Word wrap"
        Me.CheckBox17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CheckBox17.UseVisualStyleBackColor = True
        '
        'TextBox13
        '
        Me.TextBox13.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox13.Location = New System.Drawing.Point(54, 80)
        Me.TextBox13.Multiline = True
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.ReadOnly = True
        Me.TextBox13.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox13.Size = New System.Drawing.Size(644, 285)
        Me.TextBox13.TabIndex = 13
        '
        'FinalReviewHeader
        '
        Me.FinalReviewHeader.AutoEllipsis = True
        Me.FinalReviewHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.FinalReviewHeader.Location = New System.Drawing.Point(16, 17)
        Me.FinalReviewHeader.Name = "FinalReviewHeader"
        Me.FinalReviewHeader.Size = New System.Drawing.Size(708, 51)
        Me.FinalReviewHeader.TabIndex = 12
        Me.FinalReviewHeader.Text = "Review your settings for the unattended answer file"
        '
        'ComponentPanel
        '
        Me.ComponentPanel.Controls.Add(Me.Label52)
        Me.ComponentPanel.Controls.Add(Me.ComponentHeader)
        Me.ComponentPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComponentPanel.Location = New System.Drawing.Point(0, 0)
        Me.ComponentPanel.Name = "ComponentPanel"
        Me.ComponentPanel.Size = New System.Drawing.Size(752, 449)
        Me.ComponentPanel.TabIndex = 13
        '
        'Label52
        '
        Me.Label52.AutoEllipsis = True
        Me.Label52.Location = New System.Drawing.Point(107, 156)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(558, 66)
        Me.Label52.TabIndex = 12
        Me.Label52.Text = "This action will be added in the future. For now, you can use the editor mode to " & _
    "add more components. You can continue."
        '
        'ComponentHeader
        '
        Me.ComponentHeader.AutoEllipsis = True
        Me.ComponentHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.ComponentHeader.Location = New System.Drawing.Point(16, 17)
        Me.ComponentHeader.Name = "ComponentHeader"
        Me.ComponentHeader.Size = New System.Drawing.Size(708, 51)
        Me.ComponentHeader.TabIndex = 11
        Me.ComponentHeader.Text = "Configure additional components"
        '
        'PostInstallPanel
        '
        Me.PostInstallPanel.Controls.Add(Me.Label51)
        Me.PostInstallPanel.Controls.Add(Me.PostInstallHeader)
        Me.PostInstallPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PostInstallPanel.Location = New System.Drawing.Point(0, 0)
        Me.PostInstallPanel.Name = "PostInstallPanel"
        Me.PostInstallPanel.Size = New System.Drawing.Size(752, 449)
        Me.PostInstallPanel.TabIndex = 12
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Location = New System.Drawing.Point(107, 156)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(279, 13)
        Me.Label51.TabIndex = 11
        Me.Label51.Text = "This action will be added in the future. You can continue."
        '
        'PostInstallHeader
        '
        Me.PostInstallHeader.AutoEllipsis = True
        Me.PostInstallHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.PostInstallHeader.Location = New System.Drawing.Point(16, 17)
        Me.PostInstallHeader.Name = "PostInstallHeader"
        Me.PostInstallHeader.Size = New System.Drawing.Size(708, 51)
        Me.PostInstallHeader.TabIndex = 10
        Me.PostInstallHeader.Text = "What will be run after installation?"
        '
        'SystemTelemetryPanel
        '
        Me.SystemTelemetryPanel.Controls.Add(Me.TelemetryOptionsPanel)
        Me.SystemTelemetryPanel.Controls.Add(Me.CheckBox16)
        Me.SystemTelemetryPanel.Controls.Add(Me.SystemTelemetryHeader)
        Me.SystemTelemetryPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SystemTelemetryPanel.Location = New System.Drawing.Point(0, 0)
        Me.SystemTelemetryPanel.Name = "SystemTelemetryPanel"
        Me.SystemTelemetryPanel.Size = New System.Drawing.Size(752, 449)
        Me.SystemTelemetryPanel.TabIndex = 11
        '
        'TelemetryOptionsPanel
        '
        Me.TelemetryOptionsPanel.Controls.Add(Me.RadioButton27)
        Me.TelemetryOptionsPanel.Controls.Add(Me.RadioButton26)
        Me.TelemetryOptionsPanel.Location = New System.Drawing.Point(59, 80)
        Me.TelemetryOptionsPanel.Name = "TelemetryOptionsPanel"
        Me.TelemetryOptionsPanel.Size = New System.Drawing.Size(635, 93)
        Me.TelemetryOptionsPanel.TabIndex = 13
        '
        'RadioButton27
        '
        Me.RadioButton27.AutoSize = True
        Me.RadioButton27.Location = New System.Drawing.Point(29, 45)
        Me.RadioButton27.Name = "RadioButton27"
        Me.RadioButton27.Size = New System.Drawing.Size(106, 17)
        Me.RadioButton27.TabIndex = 0
        Me.RadioButton27.Text = "Enable telemetry"
        Me.RadioButton27.UseVisualStyleBackColor = True
        '
        'RadioButton26
        '
        Me.RadioButton26.AutoSize = True
        Me.RadioButton26.Checked = True
        Me.RadioButton26.Location = New System.Drawing.Point(29, 22)
        Me.RadioButton26.Name = "RadioButton26"
        Me.RadioButton26.Size = New System.Drawing.Size(108, 17)
        Me.RadioButton26.TabIndex = 0
        Me.RadioButton26.TabStop = True
        Me.RadioButton26.Text = "Disable telemetry"
        Me.RadioButton26.UseVisualStyleBackColor = True
        '
        'CheckBox16
        '
        Me.CheckBox16.AutoSize = True
        Me.CheckBox16.Location = New System.Drawing.Point(65, 392)
        Me.CheckBox16.Name = "CheckBox16"
        Me.CheckBox16.Size = New System.Drawing.Size(276, 17)
        Me.CheckBox16.TabIndex = 12
        Me.CheckBox16.Text = "I want to configure these settings during installation"
        Me.CheckBox16.UseVisualStyleBackColor = True
        '
        'SystemTelemetryHeader
        '
        Me.SystemTelemetryHeader.AutoEllipsis = True
        Me.SystemTelemetryHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.SystemTelemetryHeader.Location = New System.Drawing.Point(16, 17)
        Me.SystemTelemetryHeader.Name = "SystemTelemetryHeader"
        Me.SystemTelemetryHeader.Size = New System.Drawing.Size(708, 51)
        Me.SystemTelemetryHeader.TabIndex = 10
        Me.SystemTelemetryHeader.Text = "Control and limit how much information is sent to Microsoft and third-parties"
        '
        'NetworkConnectionPanel
        '
        Me.NetworkConnectionPanel.Controls.Add(Me.ManualNetworkConfigPanel)
        Me.NetworkConnectionPanel.Controls.Add(Me.CheckBox14)
        Me.NetworkConnectionPanel.Controls.Add(Me.NetworkConnectionHeader)
        Me.NetworkConnectionPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NetworkConnectionPanel.Location = New System.Drawing.Point(0, 0)
        Me.NetworkConnectionPanel.Name = "NetworkConnectionPanel"
        Me.NetworkConnectionPanel.Size = New System.Drawing.Size(752, 449)
        Me.NetworkConnectionPanel.TabIndex = 10
        '
        'ManualNetworkConfigPanel
        '
        Me.ManualNetworkConfigPanel.AutoScroll = True
        Me.ManualNetworkConfigPanel.Controls.Add(Me.RadioButton25)
        Me.ManualNetworkConfigPanel.Controls.Add(Me.WirelessNetworkSettingsPanel)
        Me.ManualNetworkConfigPanel.Controls.Add(Me.RadioButton30)
        Me.ManualNetworkConfigPanel.Controls.Add(Me.Label55)
        Me.ManualNetworkConfigPanel.Controls.Add(Me.Label53)
        Me.ManualNetworkConfigPanel.Enabled = False
        Me.ManualNetworkConfigPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.ManualNetworkConfigPanel.Location = New System.Drawing.Point(65, 74)
        Me.ManualNetworkConfigPanel.Name = "ManualNetworkConfigPanel"
        Me.ManualNetworkConfigPanel.Padding = New System.Windows.Forms.Padding(4, 6, 0, 0)
        Me.ManualNetworkConfigPanel.Size = New System.Drawing.Size(622, 300)
        Me.ManualNetworkConfigPanel.TabIndex = 12
        Me.ManualNetworkConfigPanel.WrapContents = False
        '
        'RadioButton25
        '
        Me.RadioButton25.AutoSize = True
        Me.RadioButton25.Checked = True
        Me.RadioButton25.Location = New System.Drawing.Point(7, 9)
        Me.RadioButton25.Name = "RadioButton25"
        Me.RadioButton25.Size = New System.Drawing.Size(259, 17)
        Me.RadioButton25.TabIndex = 4
        Me.RadioButton25.TabStop = True
        Me.RadioButton25.Text = "Configure settings for the wireless network now:"
        Me.RadioButton25.UseVisualStyleBackColor = True
        '
        'WirelessNetworkSettingsPanel
        '
        Me.WirelessNetworkSettingsPanel.Controls.Add(Me.ComboBox13)
        Me.WirelessNetworkSettingsPanel.Controls.Add(Me.TextBox10)
        Me.WirelessNetworkSettingsPanel.Controls.Add(Me.TextBox7)
        Me.WirelessNetworkSettingsPanel.Controls.Add(Me.CheckBox15)
        Me.WirelessNetworkSettingsPanel.Controls.Add(Me.Label49)
        Me.WirelessNetworkSettingsPanel.Controls.Add(Me.Label48)
        Me.WirelessNetworkSettingsPanel.Controls.Add(Me.Label50)
        Me.WirelessNetworkSettingsPanel.Controls.Add(Me.Label47)
        Me.WirelessNetworkSettingsPanel.Location = New System.Drawing.Point(24, 32)
        Me.WirelessNetworkSettingsPanel.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.WirelessNetworkSettingsPanel.Name = "WirelessNetworkSettingsPanel"
        Me.WirelessNetworkSettingsPanel.Size = New System.Drawing.Size(576, 188)
        Me.WirelessNetworkSettingsPanel.TabIndex = 5
        '
        'ComboBox13
        '
        Me.ComboBox13.FormattingEnabled = True
        Me.ComboBox13.Items.AddRange(New Object() {"Open (least secure)", "WPA2-PSK", "WPA3-SAE"})
        Me.ComboBox13.Location = New System.Drawing.Point(277, 66)
        Me.ComboBox13.Name = "ComboBox13"
        Me.ComboBox13.Size = New System.Drawing.Size(221, 21)
        Me.ComboBox13.TabIndex = 3
        '
        'TextBox10
        '
        Me.TextBox10.Location = New System.Drawing.Point(277, 155)
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox10.Size = New System.Drawing.Size(221, 21)
        Me.TextBox10.TabIndex = 2
        '
        'TextBox7
        '
        Me.TextBox7.Location = New System.Drawing.Point(277, 15)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(221, 21)
        Me.TextBox7.TabIndex = 2
        '
        'CheckBox15
        '
        Me.CheckBox15.AutoEllipsis = True
        Me.CheckBox15.Location = New System.Drawing.Point(81, 42)
        Me.CheckBox15.Name = "CheckBox15"
        Me.CheckBox15.Size = New System.Drawing.Size(417, 17)
        Me.CheckBox15.TabIndex = 1
        Me.CheckBox15.Text = "Connect even if not broadcasting"
        Me.CheckBox15.UseVisualStyleBackColor = True
        '
        'Label49
        '
        Me.Label49.AutoEllipsis = True
        Me.Label49.Location = New System.Drawing.Point(78, 158)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(193, 13)
        Me.Label49.TabIndex = 0
        Me.Label49.Text = "Password:"
        '
        'Label48
        '
        Me.Label48.AutoEllipsis = True
        Me.Label48.Location = New System.Drawing.Point(78, 69)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(193, 13)
        Me.Label48.TabIndex = 0
        Me.Label48.Text = "Authentication technology:"
        '
        'Label50
        '
        Me.Label50.AutoEllipsis = True
        Me.Label50.Location = New System.Drawing.Point(277, 93)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(221, 57)
        Me.Label50.TabIndex = 0
        Me.Label50.Text = "Please choose the technology that both the wireless router and your network adapt" & _
    "er support."
        '
        'Label47
        '
        Me.Label47.AutoEllipsis = True
        Me.Label47.Location = New System.Drawing.Point(78, 18)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(193, 13)
        Me.Label47.TabIndex = 0
        Me.Label47.Text = "SSID (Network Name):"
        '
        'RadioButton30
        '
        Me.RadioButton30.AutoSize = True
        Me.RadioButton30.Location = New System.Drawing.Point(7, 226)
        Me.RadioButton30.Name = "RadioButton30"
        Me.RadioButton30.Size = New System.Drawing.Size(110, 17)
        Me.RadioButton30.TabIndex = 4
        Me.RadioButton30.Text = "Skip configuration"
        Me.RadioButton30.UseVisualStyleBackColor = True
        '
        'Label55
        '
        Me.Label55.AutoEllipsis = True
        Me.Label55.Location = New System.Drawing.Point(24, 246)
        Me.Label55.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(576, 36)
        Me.Label55.TabIndex = 0
        Me.Label55.Text = "Choose this option if you either don't have a network adapter or plan to use Ethe" & _
    "rnet"
        '
        'Label53
        '
        Me.Label53.AutoEllipsis = True
        Me.Label53.Location = New System.Drawing.Point(7, 282)
        Me.Label53.Name = "Label53"
        Me.Label53.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label53.Size = New System.Drawing.Size(405, 6)
        Me.Label53.TabIndex = 1
        Me.Label53.UseMnemonic = False
        '
        'CheckBox14
        '
        Me.CheckBox14.AutoSize = True
        Me.CheckBox14.Checked = True
        Me.CheckBox14.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox14.Location = New System.Drawing.Point(65, 392)
        Me.CheckBox14.Name = "CheckBox14"
        Me.CheckBox14.Size = New System.Drawing.Size(276, 17)
        Me.CheckBox14.TabIndex = 11
        Me.CheckBox14.Text = "I want to configure these settings during installation"
        Me.CheckBox14.UseVisualStyleBackColor = True
        '
        'NetworkConnectionHeader
        '
        Me.NetworkConnectionHeader.AutoEllipsis = True
        Me.NetworkConnectionHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.NetworkConnectionHeader.Location = New System.Drawing.Point(16, 17)
        Me.NetworkConnectionHeader.Name = "NetworkConnectionHeader"
        Me.NetworkConnectionHeader.Size = New System.Drawing.Size(708, 51)
        Me.NetworkConnectionHeader.TabIndex = 9
        Me.NetworkConnectionHeader.Text = "Configure wireless network settings and get connected online"
        '
        'VirtualMachinePanel
        '
        Me.VirtualMachinePanel.Controls.Add(Me.VMProviderPanel)
        Me.VirtualMachinePanel.Controls.Add(Me.RadioButton24)
        Me.VirtualMachinePanel.Controls.Add(Me.RadioButton23)
        Me.VirtualMachinePanel.Controls.Add(Me.VirtualMachineHeader)
        Me.VirtualMachinePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.VirtualMachinePanel.Location = New System.Drawing.Point(0, 0)
        Me.VirtualMachinePanel.Name = "VirtualMachinePanel"
        Me.VirtualMachinePanel.Size = New System.Drawing.Size(752, 449)
        Me.VirtualMachinePanel.TabIndex = 9
        '
        'VMProviderPanel
        '
        Me.VMProviderPanel.Controls.Add(Me.Label46)
        Me.VMProviderPanel.Controls.Add(Me.ComboBox8)
        Me.VMProviderPanel.Controls.Add(Me.Label45)
        Me.VMProviderPanel.Enabled = False
        Me.VMProviderPanel.Location = New System.Drawing.Point(82, 93)
        Me.VMProviderPanel.Name = "VMProviderPanel"
        Me.VMProviderPanel.Size = New System.Drawing.Size(624, 114)
        Me.VMProviderPanel.TabIndex = 11
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Location = New System.Drawing.Point(159, 52)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(269, 39)
        Me.Label46.TabIndex = 2
        Me.Label46.Text = "- Use Guest Additions with Oracle VM VirtualBox" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "- Use VMware Tools with VMware h" & _
    "ypervisors" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "- Use VirtIO Guest Tools with QEMU-based hypervisors"
        '
        'ComboBox8
        '
        Me.ComboBox8.FormattingEnabled = True
        Me.ComboBox8.Items.AddRange(New Object() {"VirtualBox Guest Additions", "VMware Tools", "VirtIO Guest Tools"})
        Me.ComboBox8.Location = New System.Drawing.Point(162, 19)
        Me.ComboBox8.Name = "ComboBox8"
        Me.ComboBox8.Size = New System.Drawing.Size(439, 21)
        Me.ComboBox8.TabIndex = 1
        Me.ComboBox8.Text = "VirtIO Guest Tools"
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Location = New System.Drawing.Point(21, 22)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(124, 13)
        Me.Label45.TabIndex = 0
        Me.Label45.Text = "Virtual Machine Support:"
        '
        'RadioButton24
        '
        Me.RadioButton24.AutoSize = True
        Me.RadioButton24.Checked = True
        Me.RadioButton24.Location = New System.Drawing.Point(65, 217)
        Me.RadioButton24.Name = "RadioButton24"
        Me.RadioButton24.Size = New System.Drawing.Size(303, 17)
        Me.RadioButton24.TabIndex = 10
        Me.RadioButton24.TabStop = True
        Me.RadioButton24.Text = "No, I plan on using the target installation on a real system"
        Me.RadioButton24.UseVisualStyleBackColor = True
        '
        'RadioButton23
        '
        Me.RadioButton23.AutoSize = True
        Me.RadioButton23.Location = New System.Drawing.Point(65, 71)
        Me.RadioButton23.Name = "RadioButton23"
        Me.RadioButton23.Size = New System.Drawing.Size(318, 17)
        Me.RadioButton23.TabIndex = 10
        Me.RadioButton23.Text = "Yes, I want to use the target installation on a virtual machine"
        Me.RadioButton23.UseVisualStyleBackColor = True
        '
        'VirtualMachineHeader
        '
        Me.VirtualMachineHeader.AutoEllipsis = True
        Me.VirtualMachineHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.VirtualMachineHeader.Location = New System.Drawing.Point(16, 17)
        Me.VirtualMachineHeader.Name = "VirtualMachineHeader"
        Me.VirtualMachineHeader.Size = New System.Drawing.Size(708, 51)
        Me.VirtualMachineHeader.TabIndex = 9
        Me.VirtualMachineHeader.Text = "Do you want to add enhanced support from your virtual machine solution?"
        '
        'AccountLockdownPanel
        '
        Me.AccountLockdownPanel.Controls.Add(Me.Label44)
        Me.AccountLockdownPanel.Controls.Add(Me.EnabledAccountLockdownPanel)
        Me.AccountLockdownPanel.Controls.Add(Me.CheckBox13)
        Me.AccountLockdownPanel.Controls.Add(Me.AccountLockdownHeader)
        Me.AccountLockdownPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccountLockdownPanel.Location = New System.Drawing.Point(0, 0)
        Me.AccountLockdownPanel.Name = "AccountLockdownPanel"
        Me.AccountLockdownPanel.Size = New System.Drawing.Size(752, 449)
        Me.AccountLockdownPanel.TabIndex = 8
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Location = New System.Drawing.Point(81, 392)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(443, 13)
        Me.Label44.TabIndex = 13
        Me.Label44.Text = "Checking this option will make the target installation more vulnerable to brute-f" & _
    "orce attacks"
        '
        'EnabledAccountLockdownPanel
        '
        Me.EnabledAccountLockdownPanel.Controls.Add(Me.AccountLockdownParametersPanel)
        Me.EnabledAccountLockdownPanel.Controls.Add(Me.RadioButton22)
        Me.EnabledAccountLockdownPanel.Controls.Add(Me.RadioButton21)
        Me.EnabledAccountLockdownPanel.Location = New System.Drawing.Point(57, 65)
        Me.EnabledAccountLockdownPanel.Name = "EnabledAccountLockdownPanel"
        Me.EnabledAccountLockdownPanel.Size = New System.Drawing.Size(650, 198)
        Me.EnabledAccountLockdownPanel.TabIndex = 12
        '
        'AccountLockdownParametersPanel
        '
        Me.AccountLockdownParametersPanel.Controls.Add(Me.NumericUpDown8)
        Me.AccountLockdownParametersPanel.Controls.Add(Me.NumericUpDown7)
        Me.AccountLockdownParametersPanel.Controls.Add(Me.NumericUpDown6)
        Me.AccountLockdownParametersPanel.Controls.Add(Me.Label43)
        Me.AccountLockdownParametersPanel.Controls.Add(Me.Label42)
        Me.AccountLockdownParametersPanel.Controls.Add(Me.Label41)
        Me.AccountLockdownParametersPanel.Controls.Add(Me.Label40)
        Me.AccountLockdownParametersPanel.Enabled = False
        Me.AccountLockdownParametersPanel.Location = New System.Drawing.Point(27, 54)
        Me.AccountLockdownParametersPanel.Name = "AccountLockdownParametersPanel"
        Me.AccountLockdownParametersPanel.Size = New System.Drawing.Size(602, 139)
        Me.AccountLockdownParametersPanel.TabIndex = 10
        '
        'NumericUpDown8
        '
        Me.NumericUpDown8.Location = New System.Drawing.Point(351, 91)
        Me.NumericUpDown8.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown8.Name = "NumericUpDown8"
        Me.NumericUpDown8.Size = New System.Drawing.Size(120, 21)
        Me.NumericUpDown8.TabIndex = 1
        Me.NumericUpDown8.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'NumericUpDown7
        '
        Me.NumericUpDown7.Location = New System.Drawing.Point(351, 64)
        Me.NumericUpDown7.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown7.Name = "NumericUpDown7"
        Me.NumericUpDown7.Size = New System.Drawing.Size(120, 21)
        Me.NumericUpDown7.TabIndex = 1
        Me.NumericUpDown7.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'NumericUpDown6
        '
        Me.NumericUpDown6.Location = New System.Drawing.Point(351, 37)
        Me.NumericUpDown6.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown6.Name = "NumericUpDown6"
        Me.NumericUpDown6.Size = New System.Drawing.Size(120, 21)
        Me.NumericUpDown6.TabIndex = 1
        Me.NumericUpDown6.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Label43
        '
        Me.Label43.AutoEllipsis = True
        Me.Label43.Location = New System.Drawing.Point(30, 93)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(314, 35)
        Me.Label43.TabIndex = 0
        Me.Label43.Text = "After the following amount of minutes, unlock the account:"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label42
        '
        Me.Label42.AutoEllipsis = True
        Me.Label42.Location = New System.Drawing.Point(30, 67)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(314, 13)
        Me.Label42.TabIndex = 0
        Me.Label42.Text = "Within the following timeframe in minutes:"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label41
        '
        Me.Label41.AutoEllipsis = True
        Me.Label41.Location = New System.Drawing.Point(30, 39)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(314, 13)
        Me.Label41.TabIndex = 0
        Me.Label41.Text = "After the following amount of failed attempts:"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(12, 14)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(115, 13)
        Me.Label40.TabIndex = 0
        Me.Label40.Text = "Lock out an account..."
        '
        'RadioButton22
        '
        Me.RadioButton22.AutoSize = True
        Me.RadioButton22.Location = New System.Drawing.Point(8, 29)
        Me.RadioButton22.Name = "RadioButton22"
        Me.RadioButton22.Size = New System.Drawing.Size(257, 17)
        Me.RadioButton22.TabIndex = 9
        Me.RadioButton22.Text = "Continue with custom Account Lockdown policies"
        Me.RadioButton22.UseVisualStyleBackColor = True
        '
        'RadioButton21
        '
        Me.RadioButton21.AutoSize = True
        Me.RadioButton21.Checked = True
        Me.RadioButton21.Location = New System.Drawing.Point(8, 6)
        Me.RadioButton21.Name = "RadioButton21"
        Me.RadioButton21.Size = New System.Drawing.Size(257, 17)
        Me.RadioButton21.TabIndex = 9
        Me.RadioButton21.TabStop = True
        Me.RadioButton21.Text = "Continue with default Account Lockdown policies"
        Me.RadioButton21.UseVisualStyleBackColor = True
        '
        'CheckBox13
        '
        Me.CheckBox13.AutoSize = True
        Me.CheckBox13.Location = New System.Drawing.Point(65, 369)
        Me.CheckBox13.Name = "CheckBox13"
        Me.CheckBox13.Size = New System.Drawing.Size(90, 17)
        Me.CheckBox13.TabIndex = 11
        Me.CheckBox13.Text = "Disable policy"
        Me.CheckBox13.UseVisualStyleBackColor = True
        '
        'AccountLockdownHeader
        '
        Me.AccountLockdownHeader.AutoEllipsis = True
        Me.AccountLockdownHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.AccountLockdownHeader.Location = New System.Drawing.Point(16, 17)
        Me.AccountLockdownHeader.Name = "AccountLockdownHeader"
        Me.AccountLockdownHeader.Size = New System.Drawing.Size(708, 51)
        Me.AccountLockdownHeader.TabIndex = 8
        Me.AccountLockdownHeader.Text = "Configure Account Lockdown policies for the target system"
        '
        'PWExpirationPanel
        '
        Me.PWExpirationPanel.Controls.Add(Me.AutoExpirationPanel)
        Me.PWExpirationPanel.Controls.Add(Me.RadioButton18)
        Me.PWExpirationPanel.Controls.Add(Me.RadioButton17)
        Me.PWExpirationPanel.Controls.Add(Me.PWExpirationHeader)
        Me.PWExpirationPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PWExpirationPanel.Location = New System.Drawing.Point(0, 0)
        Me.PWExpirationPanel.Name = "PWExpirationPanel"
        Me.PWExpirationPanel.Size = New System.Drawing.Size(752, 449)
        Me.PWExpirationPanel.TabIndex = 7
        '
        'AutoExpirationPanel
        '
        Me.AutoExpirationPanel.Controls.Add(Me.TimedExpirationPanel)
        Me.AutoExpirationPanel.Controls.Add(Me.RadioButton20)
        Me.AutoExpirationPanel.Controls.Add(Me.RadioButton19)
        Me.AutoExpirationPanel.Enabled = False
        Me.AutoExpirationPanel.Location = New System.Drawing.Point(80, 122)
        Me.AutoExpirationPanel.Name = "AutoExpirationPanel"
        Me.AutoExpirationPanel.Size = New System.Drawing.Size(606, 107)
        Me.AutoExpirationPanel.TabIndex = 9
        '
        'TimedExpirationPanel
        '
        Me.TimedExpirationPanel.Controls.Add(Me.Label39)
        Me.TimedExpirationPanel.Controls.Add(Me.NumericUpDown5)
        Me.TimedExpirationPanel.Enabled = False
        Me.TimedExpirationPanel.Location = New System.Drawing.Point(31, 59)
        Me.TimedExpirationPanel.Name = "TimedExpirationPanel"
        Me.TimedExpirationPanel.Size = New System.Drawing.Size(562, 35)
        Me.TimedExpirationPanel.TabIndex = 1
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(291, 11)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(30, 13)
        Me.Label39.TabIndex = 1
        Me.Label39.Text = "days"
        '
        'NumericUpDown5
        '
        Me.NumericUpDown5.Location = New System.Drawing.Point(241, 7)
        Me.NumericUpDown5.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown5.Name = "NumericUpDown5"
        Me.NumericUpDown5.Size = New System.Drawing.Size(44, 21)
        Me.NumericUpDown5.TabIndex = 0
        Me.NumericUpDown5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NumericUpDown5.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'RadioButton20
        '
        Me.RadioButton20.AutoSize = True
        Me.RadioButton20.Location = New System.Drawing.Point(13, 34)
        Me.RadioButton20.Name = "RadioButton20"
        Me.RadioButton20.Size = New System.Drawing.Size(316, 17)
        Me.RadioButton20.TabIndex = 0
        Me.RadioButton20.Text = "Passwords should expire after the following number of days:"
        Me.RadioButton20.UseVisualStyleBackColor = True
        '
        'RadioButton19
        '
        Me.RadioButton19.AutoSize = True
        Me.RadioButton19.Checked = True
        Me.RadioButton19.Location = New System.Drawing.Point(13, 12)
        Me.RadioButton19.Name = "RadioButton19"
        Me.RadioButton19.Size = New System.Drawing.Size(211, 17)
        Me.RadioButton19.TabIndex = 0
        Me.RadioButton19.TabStop = True
        Me.RadioButton19.Text = "Passwords should expire after 42 days"
        Me.RadioButton19.UseVisualStyleBackColor = True
        '
        'RadioButton18
        '
        Me.RadioButton18.AutoSize = True
        Me.RadioButton18.Location = New System.Drawing.Point(65, 94)
        Me.RadioButton18.Name = "RadioButton18"
        Me.RadioButton18.Size = New System.Drawing.Size(431, 17)
        Me.RadioButton18.TabIndex = 8
        Me.RadioButton18.Text = "Passwords should expire after a certain amount of days (not recommended by NIST)"
        Me.RadioButton18.UseVisualStyleBackColor = True
        '
        'RadioButton17
        '
        Me.RadioButton17.AutoSize = True
        Me.RadioButton17.Checked = True
        Me.RadioButton17.Location = New System.Drawing.Point(65, 71)
        Me.RadioButton17.Name = "RadioButton17"
        Me.RadioButton17.Size = New System.Drawing.Size(174, 17)
        Me.RadioButton17.TabIndex = 8
        Me.RadioButton17.TabStop = True
        Me.RadioButton17.Text = "Passwords should never expire"
        Me.RadioButton17.UseVisualStyleBackColor = True
        '
        'PWExpirationHeader
        '
        Me.PWExpirationHeader.AutoEllipsis = True
        Me.PWExpirationHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.PWExpirationHeader.Location = New System.Drawing.Point(16, 17)
        Me.PWExpirationHeader.Name = "PWExpirationHeader"
        Me.PWExpirationHeader.Size = New System.Drawing.Size(708, 51)
        Me.PWExpirationHeader.TabIndex = 7
        Me.PWExpirationHeader.Text = "Should passwords expire?"
        '
        'UserAccountPanel
        '
        Me.UserAccountPanel.Controls.Add(Me.Label34)
        Me.UserAccountPanel.Controls.Add(Me.CheckBox6)
        Me.UserAccountPanel.Controls.Add(Me.ManualAccountPanel)
        Me.UserAccountPanel.Controls.Add(Me.UserAccountHeader)
        Me.UserAccountPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserAccountPanel.Location = New System.Drawing.Point(0, 0)
        Me.UserAccountPanel.Name = "UserAccountPanel"
        Me.UserAccountPanel.Size = New System.Drawing.Size(752, 449)
        Me.UserAccountPanel.TabIndex = 6
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(81, 414)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(286, 13)
        Me.Label34.TabIndex = 11
        Me.Label34.Text = "Uncheck this only if you want to set up local accounts now"
        '
        'CheckBox6
        '
        Me.CheckBox6.AutoSize = True
        Me.CheckBox6.Checked = True
        Me.CheckBox6.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox6.Location = New System.Drawing.Point(65, 392)
        Me.CheckBox6.Name = "CheckBox6"
        Me.CheckBox6.Size = New System.Drawing.Size(276, 17)
        Me.CheckBox6.TabIndex = 10
        Me.CheckBox6.Text = "I want to configure these settings during installation"
        Me.CheckBox6.UseVisualStyleBackColor = True
        '
        'ManualAccountPanel
        '
        Me.ManualAccountPanel.AutoScroll = True
        Me.ManualAccountPanel.Controls.Add(Me.UserAccountListing)
        Me.ManualAccountPanel.Controls.Add(Me.GroupBox1)
        Me.ManualAccountPanel.Controls.Add(Me.CheckBox7)
        Me.ManualAccountPanel.Controls.Add(Me.FillerLabel2)
        Me.ManualAccountPanel.Enabled = False
        Me.ManualAccountPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.ManualAccountPanel.Location = New System.Drawing.Point(65, 71)
        Me.ManualAccountPanel.Name = "ManualAccountPanel"
        Me.ManualAccountPanel.Padding = New System.Windows.Forms.Padding(4, 6, 0, 0)
        Me.ManualAccountPanel.Size = New System.Drawing.Size(622, 305)
        Me.ManualAccountPanel.TabIndex = 9
        Me.ManualAccountPanel.WrapContents = False
        '
        'UserAccountListing
        '
        Me.UserAccountListing.Controls.Add(Me.AccountsPanel)
        Me.UserAccountListing.Location = New System.Drawing.Point(7, 9)
        Me.UserAccountListing.Name = "UserAccountListing"
        Me.UserAccountListing.Size = New System.Drawing.Size(593, 153)
        Me.UserAccountListing.TabIndex = 2
        '
        'AccountsPanel
        '
        Me.AccountsPanel.ColumnCount = 4
        Me.AccountsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.AccountsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.AccountsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.AccountsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.AccountsPanel.Controls.Add(Me.ComboBox12, 3, 5)
        Me.AccountsPanel.Controls.Add(Me.ComboBox11, 3, 4)
        Me.AccountsPanel.Controls.Add(Me.ComboBox10, 3, 3)
        Me.AccountsPanel.Controls.Add(Me.ComboBox9, 3, 2)
        Me.AccountsPanel.Controls.Add(Me.TextBox18, 2, 5)
        Me.AccountsPanel.Controls.Add(Me.TextBox17, 1, 5)
        Me.AccountsPanel.Controls.Add(Me.TextBox15, 2, 4)
        Me.AccountsPanel.Controls.Add(Me.TextBox14, 1, 4)
        Me.AccountsPanel.Controls.Add(Me.TextBox12, 2, 3)
        Me.AccountsPanel.Controls.Add(Me.TextBox11, 1, 3)
        Me.AccountsPanel.Controls.Add(Me.TextBox9, 2, 2)
        Me.AccountsPanel.Controls.Add(Me.TextBox8, 1, 2)
        Me.AccountsPanel.Controls.Add(Me.TextBox6, 2, 1)
        Me.AccountsPanel.Controls.Add(Me.Label35, 1, 0)
        Me.AccountsPanel.Controls.Add(Me.Label36, 2, 0)
        Me.AccountsPanel.Controls.Add(Me.Label37, 3, 0)
        Me.AccountsPanel.Controls.Add(Me.Label38, 0, 1)
        Me.AccountsPanel.Controls.Add(Me.CheckBox8, 0, 2)
        Me.AccountsPanel.Controls.Add(Me.CheckBox9, 0, 3)
        Me.AccountsPanel.Controls.Add(Me.CheckBox10, 0, 4)
        Me.AccountsPanel.Controls.Add(Me.CheckBox11, 0, 5)
        Me.AccountsPanel.Controls.Add(Me.TextBox4, 1, 1)
        Me.AccountsPanel.Controls.Add(Me.ComboBox7, 3, 1)
        Me.AccountsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccountsPanel.Location = New System.Drawing.Point(0, 0)
        Me.AccountsPanel.Name = "AccountsPanel"
        Me.AccountsPanel.RowCount = 6
        Me.AccountsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.AccountsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.AccountsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.AccountsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.AccountsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.AccountsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.AccountsPanel.Size = New System.Drawing.Size(593, 153)
        Me.AccountsPanel.TabIndex = 0
        '
        'ComboBox12
        '
        Me.ComboBox12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox12.Enabled = False
        Me.ComboBox12.FormattingEnabled = True
        Me.ComboBox12.Items.AddRange(New Object() {"Administrators", "Users"})
        Me.ComboBox12.Location = New System.Drawing.Point(447, 128)
        Me.ComboBox12.Name = "ComboBox12"
        Me.ComboBox12.Size = New System.Drawing.Size(143, 21)
        Me.ComboBox12.TabIndex = 24
        Me.ComboBox12.Text = "Users"
        '
        'ComboBox11
        '
        Me.ComboBox11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox11.Enabled = False
        Me.ComboBox11.FormattingEnabled = True
        Me.ComboBox11.Items.AddRange(New Object() {"Administrators", "Users"})
        Me.ComboBox11.Location = New System.Drawing.Point(447, 103)
        Me.ComboBox11.Name = "ComboBox11"
        Me.ComboBox11.Size = New System.Drawing.Size(143, 21)
        Me.ComboBox11.TabIndex = 23
        Me.ComboBox11.Text = "Users"
        '
        'ComboBox10
        '
        Me.ComboBox10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox10.Enabled = False
        Me.ComboBox10.FormattingEnabled = True
        Me.ComboBox10.Items.AddRange(New Object() {"Administrators", "Users"})
        Me.ComboBox10.Location = New System.Drawing.Point(447, 78)
        Me.ComboBox10.Name = "ComboBox10"
        Me.ComboBox10.Size = New System.Drawing.Size(143, 21)
        Me.ComboBox10.TabIndex = 22
        Me.ComboBox10.Text = "Users"
        '
        'ComboBox9
        '
        Me.ComboBox9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox9.Enabled = False
        Me.ComboBox9.FormattingEnabled = True
        Me.ComboBox9.Items.AddRange(New Object() {"Administrators", "Users"})
        Me.ComboBox9.Location = New System.Drawing.Point(447, 53)
        Me.ComboBox9.Name = "ComboBox9"
        Me.ComboBox9.Size = New System.Drawing.Size(143, 21)
        Me.ComboBox9.TabIndex = 21
        Me.ComboBox9.Text = "Users"
        '
        'TextBox18
        '
        Me.TextBox18.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox18.Enabled = False
        Me.TextBox18.Location = New System.Drawing.Point(299, 128)
        Me.TextBox18.Name = "TextBox18"
        Me.TextBox18.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox18.Size = New System.Drawing.Size(142, 21)
        Me.TextBox18.TabIndex = 18
        '
        'TextBox17
        '
        Me.TextBox17.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox17.Enabled = False
        Me.TextBox17.Location = New System.Drawing.Point(151, 128)
        Me.TextBox17.Name = "TextBox17"
        Me.TextBox17.Size = New System.Drawing.Size(142, 21)
        Me.TextBox17.TabIndex = 17
        '
        'TextBox15
        '
        Me.TextBox15.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox15.Enabled = False
        Me.TextBox15.Location = New System.Drawing.Point(299, 103)
        Me.TextBox15.Name = "TextBox15"
        Me.TextBox15.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox15.Size = New System.Drawing.Size(142, 21)
        Me.TextBox15.TabIndex = 15
        '
        'TextBox14
        '
        Me.TextBox14.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox14.Enabled = False
        Me.TextBox14.Location = New System.Drawing.Point(151, 103)
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.Size = New System.Drawing.Size(142, 21)
        Me.TextBox14.TabIndex = 14
        '
        'TextBox12
        '
        Me.TextBox12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox12.Enabled = False
        Me.TextBox12.Location = New System.Drawing.Point(299, 78)
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox12.Size = New System.Drawing.Size(142, 21)
        Me.TextBox12.TabIndex = 12
        '
        'TextBox11
        '
        Me.TextBox11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox11.Enabled = False
        Me.TextBox11.Location = New System.Drawing.Point(151, 78)
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.Size = New System.Drawing.Size(142, 21)
        Me.TextBox11.TabIndex = 11
        '
        'TextBox9
        '
        Me.TextBox9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox9.Enabled = False
        Me.TextBox9.Location = New System.Drawing.Point(299, 53)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox9.Size = New System.Drawing.Size(142, 21)
        Me.TextBox9.TabIndex = 9
        '
        'TextBox8
        '
        Me.TextBox8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox8.Enabled = False
        Me.TextBox8.Location = New System.Drawing.Point(151, 53)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Size = New System.Drawing.Size(142, 21)
        Me.TextBox8.TabIndex = 8
        '
        'TextBox6
        '
        Me.TextBox6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox6.Location = New System.Drawing.Point(299, 28)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox6.Size = New System.Drawing.Size(142, 21)
        Me.TextBox6.TabIndex = 6
        '
        'Label35
        '
        Me.Label35.AutoEllipsis = True
        Me.Label35.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label35.Location = New System.Drawing.Point(151, 0)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(142, 25)
        Me.Label35.TabIndex = 0
        Me.Label35.Text = "Account name:"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label36
        '
        Me.Label36.AutoEllipsis = True
        Me.Label36.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label36.Location = New System.Drawing.Point(299, 0)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(142, 25)
        Me.Label36.TabIndex = 1
        Me.Label36.Text = "Account password:"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label37
        '
        Me.Label37.AutoEllipsis = True
        Me.Label37.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label37.Location = New System.Drawing.Point(447, 0)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(143, 25)
        Me.Label37.TabIndex = 1
        Me.Label37.Text = "Account group:"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label38
        '
        Me.Label38.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label38.Location = New System.Drawing.Point(3, 25)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(142, 25)
        Me.Label38.TabIndex = 2
        Me.Label38.Text = "Account 1:"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CheckBox8
        '
        Me.CheckBox8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CheckBox8.Location = New System.Drawing.Point(3, 53)
        Me.CheckBox8.Name = "CheckBox8"
        Me.CheckBox8.Size = New System.Drawing.Size(142, 19)
        Me.CheckBox8.TabIndex = 3
        Me.CheckBox8.Text = "Account 2:"
        Me.CheckBox8.UseVisualStyleBackColor = True
        '
        'CheckBox9
        '
        Me.CheckBox9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CheckBox9.Location = New System.Drawing.Point(3, 78)
        Me.CheckBox9.Name = "CheckBox9"
        Me.CheckBox9.Size = New System.Drawing.Size(142, 19)
        Me.CheckBox9.TabIndex = 3
        Me.CheckBox9.Text = "Account 3:"
        Me.CheckBox9.UseVisualStyleBackColor = True
        '
        'CheckBox10
        '
        Me.CheckBox10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CheckBox10.Location = New System.Drawing.Point(3, 103)
        Me.CheckBox10.Name = "CheckBox10"
        Me.CheckBox10.Size = New System.Drawing.Size(142, 19)
        Me.CheckBox10.TabIndex = 3
        Me.CheckBox10.Text = "Account 4:"
        Me.CheckBox10.UseVisualStyleBackColor = True
        '
        'CheckBox11
        '
        Me.CheckBox11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CheckBox11.Location = New System.Drawing.Point(3, 128)
        Me.CheckBox11.Name = "CheckBox11"
        Me.CheckBox11.Size = New System.Drawing.Size(142, 22)
        Me.CheckBox11.TabIndex = 3
        Me.CheckBox11.Text = "Account 5:"
        Me.CheckBox11.UseVisualStyleBackColor = True
        '
        'TextBox4
        '
        Me.TextBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox4.Location = New System.Drawing.Point(151, 28)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(142, 21)
        Me.TextBox4.TabIndex = 4
        Me.TextBox4.Text = "Admin"
        '
        'ComboBox7
        '
        Me.ComboBox7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox7.FormattingEnabled = True
        Me.ComboBox7.Items.AddRange(New Object() {"Administrators", "Users"})
        Me.ComboBox7.Location = New System.Drawing.Point(447, 28)
        Me.ComboBox7.Name = "ComboBox7"
        Me.ComboBox7.Size = New System.Drawing.Size(143, 21)
        Me.ComboBox7.TabIndex = 19
        Me.ComboBox7.Text = "Administrators"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.AutoLogonSettingsPanel)
        Me.GroupBox1.Controls.Add(Me.CheckBox12)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 168)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(593, 109)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "First log on"
        '
        'AutoLogonSettingsPanel
        '
        Me.AutoLogonSettingsPanel.Controls.Add(Me.TextBox5)
        Me.AutoLogonSettingsPanel.Controls.Add(Me.RadioButton16)
        Me.AutoLogonSettingsPanel.Controls.Add(Me.RadioButton15)
        Me.AutoLogonSettingsPanel.Enabled = False
        Me.AutoLogonSettingsPanel.Location = New System.Drawing.Point(34, 43)
        Me.AutoLogonSettingsPanel.Name = "AutoLogonSettingsPanel"
        Me.AutoLogonSettingsPanel.Size = New System.Drawing.Size(546, 58)
        Me.AutoLogonSettingsPanel.TabIndex = 1
        '
        'TextBox5
        '
        Me.TextBox5.Enabled = False
        Me.TextBox5.Location = New System.Drawing.Point(327, 31)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox5.Size = New System.Drawing.Size(204, 21)
        Me.TextBox5.TabIndex = 7
        '
        'RadioButton16
        '
        Me.RadioButton16.AutoSize = True
        Me.RadioButton16.Location = New System.Drawing.Point(10, 32)
        Me.RadioButton16.Name = "RadioButton16"
        Me.RadioButton16.Size = New System.Drawing.Size(311, 17)
        Me.RadioButton16.TabIndex = 0
        Me.RadioButton16.Text = "Log on to the built-in administrator account, with password:"
        Me.RadioButton16.UseVisualStyleBackColor = True
        '
        'RadioButton15
        '
        Me.RadioButton15.AutoSize = True
        Me.RadioButton15.Checked = True
        Me.RadioButton15.Location = New System.Drawing.Point(10, 9)
        Me.RadioButton15.Name = "RadioButton15"
        Me.RadioButton15.Size = New System.Drawing.Size(258, 17)
        Me.RadioButton15.TabIndex = 0
        Me.RadioButton15.TabStop = True
        Me.RadioButton15.Text = "Log on to the first administrator account created"
        Me.RadioButton15.UseVisualStyleBackColor = True
        '
        'CheckBox12
        '
        Me.CheckBox12.AutoSize = True
        Me.CheckBox12.Location = New System.Drawing.Point(17, 24)
        Me.CheckBox12.Name = "CheckBox12"
        Me.CheckBox12.Size = New System.Drawing.Size(260, 17)
        Me.CheckBox12.TabIndex = 0
        Me.CheckBox12.Text = "Log on automatically to an Administrator account"
        Me.CheckBox12.UseVisualStyleBackColor = True
        '
        'CheckBox7
        '
        Me.CheckBox7.AutoSize = True
        Me.CheckBox7.Location = New System.Drawing.Point(7, 283)
        Me.CheckBox7.Name = "CheckBox7"
        Me.CheckBox7.Size = New System.Drawing.Size(181, 17)
        Me.CheckBox7.TabIndex = 3
        Me.CheckBox7.Text = "Obscure passwords with Base64"
        Me.CheckBox7.UseVisualStyleBackColor = True
        '
        'FillerLabel2
        '
        Me.FillerLabel2.AutoEllipsis = True
        Me.FillerLabel2.Location = New System.Drawing.Point(7, 303)
        Me.FillerLabel2.Name = "FillerLabel2"
        Me.FillerLabel2.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.FillerLabel2.Size = New System.Drawing.Size(405, 6)
        Me.FillerLabel2.TabIndex = 1
        Me.FillerLabel2.UseMnemonic = False
        '
        'UserAccountHeader
        '
        Me.UserAccountHeader.AutoEllipsis = True
        Me.UserAccountHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.UserAccountHeader.Location = New System.Drawing.Point(16, 17)
        Me.UserAccountHeader.Name = "UserAccountHeader"
        Me.UserAccountHeader.Size = New System.Drawing.Size(708, 51)
        Me.UserAccountHeader.TabIndex = 6
        Me.UserAccountHeader.Text = "Who will use the target installation?"
        '
        'ProductKeyPanel
        '
        Me.ProductKeyPanel.Controls.Add(Me.ManualKeyPanel)
        Me.ProductKeyPanel.Controls.Add(Me.GenericKeyPanel)
        Me.ProductKeyPanel.Controls.Add(Me.RadioButton14)
        Me.ProductKeyPanel.Controls.Add(Me.RadioButton13)
        Me.ProductKeyPanel.Controls.Add(Me.ProductKeyHeader)
        Me.ProductKeyPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProductKeyPanel.Location = New System.Drawing.Point(0, 0)
        Me.ProductKeyPanel.Name = "ProductKeyPanel"
        Me.ProductKeyPanel.Size = New System.Drawing.Size(752, 449)
        Me.ProductKeyPanel.TabIndex = 5
        '
        'ManualKeyPanel
        '
        Me.ManualKeyPanel.Controls.Add(Me.KeyVerifyWarningPanel)
        Me.ManualKeyPanel.Controls.Add(Me.Label33)
        Me.ManualKeyPanel.Controls.Add(Me.Label30)
        Me.ManualKeyPanel.Controls.Add(Me.TextBox3)
        Me.ManualKeyPanel.Enabled = False
        Me.ManualKeyPanel.Location = New System.Drawing.Point(85, 225)
        Me.ManualKeyPanel.Name = "ManualKeyPanel"
        Me.ManualKeyPanel.Size = New System.Drawing.Size(602, 140)
        Me.ManualKeyPanel.TabIndex = 8
        '
        'KeyVerifyWarningPanel
        '
        Me.KeyVerifyWarningPanel.Controls.Add(Me.Label32)
        Me.KeyVerifyWarningPanel.Controls.Add(Me.Label31)
        Me.KeyVerifyWarningPanel.Location = New System.Drawing.Point(22, 57)
        Me.KeyVerifyWarningPanel.Name = "KeyVerifyWarningPanel"
        Me.KeyVerifyWarningPanel.Size = New System.Drawing.Size(565, 69)
        Me.KeyVerifyWarningPanel.TabIndex = 4
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.Label32.Location = New System.Drawing.Point(25, 34)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(277, 13)
        Me.Label32.TabIndex = 3
        Me.Label32.Text = "Please make sure that the product key you enter is valid"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(7, 11)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(421, 13)
        Me.Label31.TabIndex = 3
        Me.Label31.Text = "DISMTools cannot verify whether product keys can be valid for activation"
        '
        'Label33
        '
        Me.Label33.AutoEllipsis = True
        Me.Label33.Location = New System.Drawing.Point(19, 38)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(569, 13)
        Me.Label33.TabIndex = 0
        Me.Label33.Text = "(Type each character of the product key, including the dashes)"
        '
        'Label30
        '
        Me.Label30.AutoEllipsis = True
        Me.Label30.Location = New System.Drawing.Point(19, 15)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(228, 13)
        Me.Label30.TabIndex = 0
        Me.Label30.Text = "Product Key:"
        '
        'TextBox3
        '
        Me.TextBox3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox3.Location = New System.Drawing.Point(253, 11)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(336, 21)
        Me.TextBox3.TabIndex = 2
        '
        'GenericKeyPanel
        '
        Me.GenericKeyPanel.Controls.Add(Me.Label29)
        Me.GenericKeyPanel.Controls.Add(Me.TextBox2)
        Me.GenericKeyPanel.Controls.Add(Me.ComboBox6)
        Me.GenericKeyPanel.Controls.Add(Me.Label28)
        Me.GenericKeyPanel.Controls.Add(Me.Label27)
        Me.GenericKeyPanel.Location = New System.Drawing.Point(85, 96)
        Me.GenericKeyPanel.Name = "GenericKeyPanel"
        Me.GenericKeyPanel.Size = New System.Drawing.Size(601, 100)
        Me.GenericKeyPanel.TabIndex = 7
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(8, 64)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(353, 13)
        Me.Label29.TabIndex = 3
        Me.Label29.Text = "You should only use this generic key with the edition you want to deploy"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(262, 35)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(327, 21)
        Me.TextBox2.TabIndex = 2
        '
        'ComboBox6
        '
        Me.ComboBox6.FormattingEnabled = True
        Me.ComboBox6.Items.AddRange(New Object() {"Education", "Education N", "Home", "Home N", "Home Single Language", "Pro", "Pro Education", "Pro Education N", "Pro for Workstations", "Pro N", "Pro N for Workstations"})
        Me.ComboBox6.Location = New System.Drawing.Point(262, 8)
        Me.ComboBox6.Name = "ComboBox6"
        Me.ComboBox6.Size = New System.Drawing.Size(327, 21)
        Me.ComboBox6.TabIndex = 1
        '
        'Label28
        '
        Me.Label28.AutoEllipsis = True
        Me.Label28.Location = New System.Drawing.Point(28, 39)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(228, 13)
        Me.Label28.TabIndex = 0
        Me.Label28.Text = "Product Key:"
        '
        'Label27
        '
        Me.Label27.AutoEllipsis = True
        Me.Label27.Location = New System.Drawing.Point(8, 11)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(248, 13)
        Me.Label27.TabIndex = 0
        Me.Label27.Text = "Use the product key for this edition:"
        '
        'RadioButton14
        '
        Me.RadioButton14.AutoSize = True
        Me.RadioButton14.Location = New System.Drawing.Point(65, 202)
        Me.RadioButton14.Name = "RadioButton14"
        Me.RadioButton14.Size = New System.Drawing.Size(149, 17)
        Me.RadioButton14.TabIndex = 6
        Me.RadioButton14.Text = "Use a custom product key"
        Me.RadioButton14.UseVisualStyleBackColor = True
        '
        'RadioButton13
        '
        Me.RadioButton13.AutoSize = True
        Me.RadioButton13.Checked = True
        Me.RadioButton13.Location = New System.Drawing.Point(65, 71)
        Me.RadioButton13.Name = "RadioButton13"
        Me.RadioButton13.Size = New System.Drawing.Size(278, 17)
        Me.RadioButton13.TabIndex = 6
        Me.RadioButton13.TabStop = True
        Me.RadioButton13.Text = "Use a generic product key (no activation capabilities)"
        Me.RadioButton13.UseVisualStyleBackColor = True
        '
        'ProductKeyHeader
        '
        Me.ProductKeyHeader.AutoEllipsis = True
        Me.ProductKeyHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.ProductKeyHeader.Location = New System.Drawing.Point(16, 17)
        Me.ProductKeyHeader.Name = "ProductKeyHeader"
        Me.ProductKeyHeader.Size = New System.Drawing.Size(708, 51)
        Me.ProductKeyHeader.TabIndex = 5
        Me.ProductKeyHeader.Text = "Type your product key for operating system installation"
        '
        'DiskConfigurationPanel
        '
        Me.DiskConfigurationPanel.Controls.Add(Me.ManualPartPanel)
        Me.DiskConfigurationPanel.Controls.Add(Me.Label20)
        Me.DiskConfigurationPanel.Controls.Add(Me.CheckBox4)
        Me.DiskConfigurationPanel.Controls.Add(Me.DiskConfigurationHeader)
        Me.DiskConfigurationPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DiskConfigurationPanel.Location = New System.Drawing.Point(0, 0)
        Me.DiskConfigurationPanel.Name = "DiskConfigurationPanel"
        Me.DiskConfigurationPanel.Size = New System.Drawing.Size(752, 449)
        Me.DiskConfigurationPanel.TabIndex = 4
        '
        'ManualPartPanel
        '
        Me.ManualPartPanel.AutoScroll = True
        Me.ManualPartPanel.Controls.Add(Me.RadioButton5)
        Me.ManualPartPanel.Controls.Add(Me.AutoDiskConfigPanel)
        Me.ManualPartPanel.Controls.Add(Me.RadioButton6)
        Me.ManualPartPanel.Controls.Add(Me.DiskPartPanel)
        Me.ManualPartPanel.Controls.Add(Me.FillerLabel)
        Me.ManualPartPanel.Enabled = False
        Me.ManualPartPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.ManualPartPanel.Location = New System.Drawing.Point(65, 71)
        Me.ManualPartPanel.Name = "ManualPartPanel"
        Me.ManualPartPanel.Padding = New System.Windows.Forms.Padding(4, 6, 0, 0)
        Me.ManualPartPanel.Size = New System.Drawing.Size(622, 300)
        Me.ManualPartPanel.TabIndex = 8
        Me.ManualPartPanel.WrapContents = False
        '
        'RadioButton5
        '
        Me.RadioButton5.AutoSize = True
        Me.RadioButton5.Checked = True
        Me.RadioButton5.Location = New System.Drawing.Point(7, 9)
        Me.RadioButton5.Name = "RadioButton5"
        Me.RadioButton5.Size = New System.Drawing.Size(160, 17)
        Me.RadioButton5.TabIndex = 4
        Me.RadioButton5.TabStop = True
        Me.RadioButton5.Text = "Configure settings for disk 0"
        Me.RadioButton5.UseVisualStyleBackColor = True
        '
        'AutoDiskConfigPanel
        '
        Me.AutoDiskConfigPanel.Controls.Add(Me.WindowsREPanel)
        Me.AutoDiskConfigPanel.Controls.Add(Me.CheckBox5)
        Me.AutoDiskConfigPanel.Controls.Add(Me.PartTablePanel)
        Me.AutoDiskConfigPanel.Controls.Add(Me.Label21)
        Me.AutoDiskConfigPanel.Location = New System.Drawing.Point(24, 32)
        Me.AutoDiskConfigPanel.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.AutoDiskConfigPanel.Name = "AutoDiskConfigPanel"
        Me.AutoDiskConfigPanel.Size = New System.Drawing.Size(576, 240)
        Me.AutoDiskConfigPanel.TabIndex = 5
        '
        'WindowsREPanel
        '
        Me.WindowsREPanel.Controls.Add(Me.RESizePanel)
        Me.WindowsREPanel.Controls.Add(Me.RadioButton10)
        Me.WindowsREPanel.Controls.Add(Me.RadioButton9)
        Me.WindowsREPanel.Location = New System.Drawing.Point(37, 127)
        Me.WindowsREPanel.Name = "WindowsREPanel"
        Me.WindowsREPanel.Size = New System.Drawing.Size(528, 97)
        Me.WindowsREPanel.TabIndex = 3
        '
        'RESizePanel
        '
        Me.RESizePanel.Controls.Add(Me.NumericUpDown2)
        Me.RESizePanel.Controls.Add(Me.Label23)
        Me.RESizePanel.Location = New System.Drawing.Point(30, 32)
        Me.RESizePanel.Name = "RESizePanel"
        Me.RESizePanel.Size = New System.Drawing.Size(481, 32)
        Me.RESizePanel.TabIndex = 1
        '
        'NumericUpDown2
        '
        Me.NumericUpDown2.Location = New System.Drawing.Point(338, 7)
        Me.NumericUpDown2.Maximum = New Decimal(New Integer() {10240, 0, 0, 0})
        Me.NumericUpDown2.Minimum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.NumericUpDown2.Name = "NumericUpDown2"
        Me.NumericUpDown2.Size = New System.Drawing.Size(133, 21)
        Me.NumericUpDown2.TabIndex = 1
        Me.NumericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NumericUpDown2.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'Label23
        '
        Me.Label23.AutoEllipsis = True
        Me.Label23.Location = New System.Drawing.Point(8, 9)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(323, 13)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = "Windows Recovery Environment partition size (in MB):"
        '
        'RadioButton10
        '
        Me.RadioButton10.AutoSize = True
        Me.RadioButton10.Location = New System.Drawing.Point(9, 70)
        Me.RadioButton10.Name = "RadioButton10"
        Me.RadioButton10.Size = New System.Drawing.Size(298, 17)
        Me.RadioButton10.TabIndex = 0
        Me.RadioButton10.Text = "Install a Recovery Environment on the Windows partition"
        Me.RadioButton10.UseVisualStyleBackColor = True
        '
        'RadioButton9
        '
        Me.RadioButton9.AutoSize = True
        Me.RadioButton9.Checked = True
        Me.RadioButton9.Location = New System.Drawing.Point(9, 9)
        Me.RadioButton9.Name = "RadioButton9"
        Me.RadioButton9.Size = New System.Drawing.Size(301, 17)
        Me.RadioButton9.TabIndex = 0
        Me.RadioButton9.TabStop = True
        Me.RadioButton9.Text = "Install a Recovery Environment on the Recovery partition"
        Me.RadioButton9.UseVisualStyleBackColor = True
        '
        'CheckBox5
        '
        Me.CheckBox5.AutoSize = True
        Me.CheckBox5.Checked = True
        Me.CheckBox5.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox5.Location = New System.Drawing.Point(18, 104)
        Me.CheckBox5.Name = "CheckBox5"
        Me.CheckBox5.Size = New System.Drawing.Size(176, 17)
        Me.CheckBox5.TabIndex = 2
        Me.CheckBox5.Text = "Install a Recovery Environment"
        Me.CheckBox5.UseVisualStyleBackColor = True
        '
        'PartTablePanel
        '
        Me.PartTablePanel.Controls.Add(Me.ESPPanel)
        Me.PartTablePanel.Controls.Add(Me.RadioButton8)
        Me.PartTablePanel.Controls.Add(Me.RadioButton7)
        Me.PartTablePanel.Location = New System.Drawing.Point(109, 3)
        Me.PartTablePanel.Name = "PartTablePanel"
        Me.PartTablePanel.Size = New System.Drawing.Size(457, 95)
        Me.PartTablePanel.TabIndex = 1
        '
        'ESPPanel
        '
        Me.ESPPanel.Controls.Add(Me.NumericUpDown1)
        Me.ESPPanel.Controls.Add(Me.Label22)
        Me.ESPPanel.Location = New System.Drawing.Point(35, 29)
        Me.ESPPanel.Name = "ESPPanel"
        Me.ESPPanel.Size = New System.Drawing.Size(408, 32)
        Me.ESPPanel.TabIndex = 1
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(261, 6)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {10240, 0, 0, 0})
        Me.NumericUpDown1.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(133, 21)
        Me.NumericUpDown1.TabIndex = 1
        Me.NumericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NumericUpDown1.Value = New Decimal(New Integer() {300, 0, 0, 0})
        '
        'Label22
        '
        Me.Label22.AutoEllipsis = True
        Me.Label22.Location = New System.Drawing.Point(8, 9)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(247, 13)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = "EFI System Partition (ESP) size (in MB):"
        '
        'RadioButton8
        '
        Me.RadioButton8.AutoSize = True
        Me.RadioButton8.Location = New System.Drawing.Point(15, 63)
        Me.RadioButton8.Name = "RadioButton8"
        Me.RadioButton8.Size = New System.Drawing.Size(46, 17)
        Me.RadioButton8.TabIndex = 0
        Me.RadioButton8.Text = "MBR"
        Me.RadioButton8.UseVisualStyleBackColor = True
        '
        'RadioButton7
        '
        Me.RadioButton7.AutoSize = True
        Me.RadioButton7.Checked = True
        Me.RadioButton7.Location = New System.Drawing.Point(15, 10)
        Me.RadioButton7.Name = "RadioButton7"
        Me.RadioButton7.Size = New System.Drawing.Size(44, 17)
        Me.RadioButton7.TabIndex = 0
        Me.RadioButton7.TabStop = True
        Me.RadioButton7.Text = "GPT"
        Me.RadioButton7.UseVisualStyleBackColor = True
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(18, 15)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(78, 13)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Partition table:"
        '
        'RadioButton6
        '
        Me.RadioButton6.AutoSize = True
        Me.RadioButton6.Location = New System.Drawing.Point(7, 278)
        Me.RadioButton6.Name = "RadioButton6"
        Me.RadioButton6.Size = New System.Drawing.Size(216, 17)
        Me.RadioButton6.TabIndex = 4
        Me.RadioButton6.Text = "Configure settings with a DiskPart script"
        Me.RadioButton6.UseVisualStyleBackColor = True
        '
        'DiskPartPanel
        '
        Me.DiskPartPanel.Controls.Add(Me.SplitContainer1)
        Me.DiskPartPanel.Enabled = False
        Me.DiskPartPanel.Location = New System.Drawing.Point(24, 301)
        Me.DiskPartPanel.Margin = New System.Windows.Forms.Padding(20, 3, 3, 3)
        Me.DiskPartPanel.Name = "DiskPartPanel"
        Me.DiskPartPanel.Size = New System.Drawing.Size(576, 267)
        Me.DiskPartPanel.TabIndex = 5
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Scintilla2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.DPActionsPanel)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ManualInstallPanel)
        Me.SplitContainer1.Panel2.Controls.Add(Me.RadioButton12)
        Me.SplitContainer1.Panel2.Controls.Add(Me.RadioButton11)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label24)
        Me.SplitContainer1.Size = New System.Drawing.Size(576, 267)
        Me.SplitContainer1.SplitterDistance = 167
        Me.SplitContainer1.TabIndex = 0
        '
        'Scintilla2
        '
        Me.Scintilla2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Scintilla2.IndentationGuides = ScintillaNET.IndentView.LookBoth
        Me.Scintilla2.LexerName = Nothing
        Me.Scintilla2.Location = New System.Drawing.Point(0, 0)
        Me.Scintilla2.Name = "Scintilla2"
        Me.Scintilla2.Size = New System.Drawing.Size(576, 139)
        Me.Scintilla2.TabIndex = 3
        Me.Scintilla2.Text = resources.GetString("Scintilla2.Text")
        '
        'DPActionsPanel
        '
        Me.DPActionsPanel.Controls.Add(Me.Button2)
        Me.DPActionsPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DPActionsPanel.Location = New System.Drawing.Point(0, 139)
        Me.DPActionsPanel.Name = "DPActionsPanel"
        Me.DPActionsPanel.Size = New System.Drawing.Size(576, 28)
        Me.DPActionsPanel.TabIndex = 4
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button2.Location = New System.Drawing.Point(369, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(204, 23)
        Me.Button2.TabIndex = 0
        Me.Button2.Text = "Open from DiskPart script file..."
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ManualInstallPanel
        '
        Me.ManualInstallPanel.Controls.Add(Me.NumericUpDown4)
        Me.ManualInstallPanel.Controls.Add(Me.Label26)
        Me.ManualInstallPanel.Controls.Add(Me.NumericUpDown3)
        Me.ManualInstallPanel.Controls.Add(Me.Label25)
        Me.ManualInstallPanel.Enabled = False
        Me.ManualInstallPanel.Location = New System.Drawing.Point(210, 56)
        Me.ManualInstallPanel.Name = "ManualInstallPanel"
        Me.ManualInstallPanel.Size = New System.Drawing.Size(354, 27)
        Me.ManualInstallPanel.TabIndex = 2
        '
        'NumericUpDown4
        '
        Me.NumericUpDown4.Location = New System.Drawing.Point(218, 4)
        Me.NumericUpDown4.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.NumericUpDown4.Name = "NumericUpDown4"
        Me.NumericUpDown4.Size = New System.Drawing.Size(120, 21)
        Me.NumericUpDown4.TabIndex = 1
        Me.NumericUpDown4.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(164, 7)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(51, 13)
        Me.Label26.TabIndex = 0
        Me.Label26.Text = "Partition:"
        '
        'NumericUpDown3
        '
        Me.NumericUpDown3.Location = New System.Drawing.Point(39, 3)
        Me.NumericUpDown3.Maximum = New Decimal(New Integer() {7, 0, 0, 0})
        Me.NumericUpDown3.Name = "NumericUpDown3"
        Me.NumericUpDown3.Size = New System.Drawing.Size(120, 21)
        Me.NumericUpDown3.TabIndex = 1
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(3, 6)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(30, 13)
        Me.Label25.TabIndex = 0
        Me.Label25.Text = "Disk:"
        '
        'RadioButton12
        '
        Me.RadioButton12.AutoSize = True
        Me.RadioButton12.Location = New System.Drawing.Point(28, 60)
        Me.RadioButton12.Name = "RadioButton12"
        Me.RadioButton12.Size = New System.Drawing.Size(155, 17)
        Me.RadioButton12.TabIndex = 1
        Me.RadioButton12.Text = "Install to another partition:"
        Me.RadioButton12.UseVisualStyleBackColor = True
        '
        'RadioButton11
        '
        Me.RadioButton11.AutoSize = True
        Me.RadioButton11.Checked = True
        Me.RadioButton11.Location = New System.Drawing.Point(28, 37)
        Me.RadioButton11.Name = "RadioButton11"
        Me.RadioButton11.Size = New System.Drawing.Size(453, 17)
        Me.RadioButton11.TabIndex = 1
        Me.RadioButton11.TabStop = True
        Me.RadioButton11.Text = "Install Windows to the first available partition with enough space and with no in" & _
    "stallations"
        Me.RadioButton11.UseVisualStyleBackColor = True
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(11, 14)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(186, 13)
        Me.Label24.TabIndex = 0
        Me.Label24.Text = "After configuration has been applied:"
        '
        'FillerLabel
        '
        Me.FillerLabel.AutoEllipsis = True
        Me.FillerLabel.Location = New System.Drawing.Point(7, 571)
        Me.FillerLabel.Name = "FillerLabel"
        Me.FillerLabel.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.FillerLabel.Size = New System.Drawing.Size(405, 6)
        Me.FillerLabel.TabIndex = 1
        Me.FillerLabel.UseMnemonic = False
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(81, 399)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(524, 13)
        Me.Label20.TabIndex = 7
        Me.Label20.Text = "Uncheck this only if you want to set up disk configuration now, either with an au" & _
    "tomation script, or manually"
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.Checked = True
        Me.CheckBox4.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox4.Location = New System.Drawing.Point(65, 377)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(276, 17)
        Me.CheckBox4.TabIndex = 5
        Me.CheckBox4.Text = "I want to configure these settings during installation"
        Me.CheckBox4.UseVisualStyleBackColor = True
        '
        'DiskConfigurationHeader
        '
        Me.DiskConfigurationHeader.AutoEllipsis = True
        Me.DiskConfigurationHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.DiskConfigurationHeader.Location = New System.Drawing.Point(16, 17)
        Me.DiskConfigurationHeader.Name = "DiskConfigurationHeader"
        Me.DiskConfigurationHeader.Size = New System.Drawing.Size(708, 51)
        Me.DiskConfigurationHeader.TabIndex = 4
        Me.DiskConfigurationHeader.Text = "Configure the disk and partition layout of the target system"
        '
        'TimeZonePanel
        '
        Me.TimeZonePanel.Controls.Add(Me.TimeZoneSettings)
        Me.TimeZonePanel.Controls.Add(Me.RadioButton4)
        Me.TimeZonePanel.Controls.Add(Me.RadioButton3)
        Me.TimeZonePanel.Controls.Add(Me.TimeZoneHeader)
        Me.TimeZonePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TimeZonePanel.Location = New System.Drawing.Point(0, 0)
        Me.TimeZonePanel.Name = "TimeZonePanel"
        Me.TimeZonePanel.Size = New System.Drawing.Size(752, 449)
        Me.TimeZonePanel.TabIndex = 3
        '
        'TimeZoneSettings
        '
        Me.TimeZoneSettings.Controls.Add(Me.CurrentTimeSelTZ)
        Me.TimeZoneSettings.Controls.Add(Me.CurrentTimeUTC)
        Me.TimeZoneSettings.Controls.Add(Me.Label19)
        Me.TimeZoneSettings.Controls.Add(Me.Label18)
        Me.TimeZoneSettings.Controls.Add(Me.ComboBox5)
        Me.TimeZoneSettings.Controls.Add(Me.Label17)
        Me.TimeZoneSettings.Enabled = False
        Me.TimeZoneSettings.Location = New System.Drawing.Point(86, 123)
        Me.TimeZoneSettings.Name = "TimeZoneSettings"
        Me.TimeZoneSettings.Size = New System.Drawing.Size(621, 136)
        Me.TimeZoneSettings.TabIndex = 5
        '
        'CurrentTimeSelTZ
        '
        Me.CurrentTimeSelTZ.AutoSize = True
        Me.CurrentTimeSelTZ.Location = New System.Drawing.Point(231, 78)
        Me.CurrentTimeSelTZ.Name = "CurrentTimeSelTZ"
        Me.CurrentTimeSelTZ.Size = New System.Drawing.Size(29, 13)
        Me.CurrentTimeSelTZ.TabIndex = 3
        Me.CurrentTimeSelTZ.Text = "Time"
        '
        'CurrentTimeUTC
        '
        Me.CurrentTimeUTC.AutoSize = True
        Me.CurrentTimeUTC.Location = New System.Drawing.Point(231, 57)
        Me.CurrentTimeUTC.Name = "CurrentTimeUTC"
        Me.CurrentTimeUTC.Size = New System.Drawing.Size(29, 13)
        Me.CurrentTimeUTC.TabIndex = 3
        Me.CurrentTimeUTC.Text = "Time"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(46, 78)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(171, 13)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "Current time (selected time zone):"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(46, 57)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(102, 13)
        Me.Label18.TabIndex = 2
        Me.Label18.Text = "Current time (UTC):"
        '
        'ComboBox5
        '
        Me.ComboBox5.FormattingEnabled = True
        Me.ComboBox5.Location = New System.Drawing.Point(99, 16)
        Me.ComboBox5.Name = "ComboBox5"
        Me.ComboBox5.Size = New System.Drawing.Size(502, 21)
        Me.ComboBox5.TabIndex = 1
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(21, 19)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(59, 13)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Time zone:"
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(65, 94)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(148, 17)
        Me.RadioButton4.TabIndex = 4
        Me.RadioButton4.Text = "Set a time zone manually:"
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Checked = True
        Me.RadioButton3.Location = New System.Drawing.Point(65, 71)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(422, 17)
        Me.RadioButton3.TabIndex = 4
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "Let Windows decide my time zone based on the regional configurations I set earlie" & _
    "r"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'TimeZoneHeader
        '
        Me.TimeZoneHeader.AutoEllipsis = True
        Me.TimeZoneHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.TimeZoneHeader.Location = New System.Drawing.Point(16, 17)
        Me.TimeZoneHeader.Name = "TimeZoneHeader"
        Me.TimeZoneHeader.Size = New System.Drawing.Size(708, 51)
        Me.TimeZoneHeader.TabIndex = 3
        Me.TimeZoneHeader.Text = "Configure time zone settings"
        '
        'SysConfigPanel
        '
        Me.SysConfigPanel.Controls.Add(Me.CheckBox3)
        Me.SysConfigPanel.Controls.Add(Me.Label15)
        Me.SysConfigPanel.Controls.Add(Me.ComputerNamePanel)
        Me.SysConfigPanel.Controls.Add(Me.WinSVSettingsPanel)
        Me.SysConfigPanel.Controls.Add(Me.Label13)
        Me.SysConfigPanel.Controls.Add(Me.Label12)
        Me.SysConfigPanel.Controls.Add(Me.ListBox1)
        Me.SysConfigPanel.Controls.Add(Me.Label11)
        Me.SysConfigPanel.Controls.Add(Me.SysConfigHeader)
        Me.SysConfigPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SysConfigPanel.Location = New System.Drawing.Point(0, 0)
        Me.SysConfigPanel.Name = "SysConfigPanel"
        Me.SysConfigPanel.Size = New System.Drawing.Size(752, 449)
        Me.SysConfigPanel.TabIndex = 2
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Checked = True
        Me.CheckBox3.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox3.Location = New System.Drawing.Point(55, 339)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(230, 17)
        Me.CheckBox3.TabIndex = 8
        Me.CheckBox3.Text = "Let Windows set a random computer name"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoEllipsis = True
        Me.Label15.Location = New System.Drawing.Point(71, 410)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(616, 33)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "You can set a different computer name at any time in the system settings"
        '
        'ComputerNamePanel
        '
        Me.ComputerNamePanel.Controls.Add(Me.TextBox1)
        Me.ComputerNamePanel.Controls.Add(Me.Label16)
        Me.ComputerNamePanel.Enabled = False
        Me.ComputerNamePanel.Location = New System.Drawing.Point(61, 362)
        Me.ComputerNamePanel.Name = "ComputerNamePanel"
        Me.ComputerNamePanel.Size = New System.Drawing.Size(626, 39)
        Me.ComputerNamePanel.TabIndex = 7
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(124, 9)
        Me.TextBox1.MaxLength = 15
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(489, 21)
        Me.TextBox1.TabIndex = 2
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(10, 12)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(87, 13)
        Me.Label16.TabIndex = 1
        Me.Label16.Text = "Computer name:"
        '
        'WinSVSettingsPanel
        '
        Me.WinSVSettingsPanel.Controls.Add(Me.Label14)
        Me.WinSVSettingsPanel.Controls.Add(Me.CheckBox2)
        Me.WinSVSettingsPanel.Controls.Add(Me.CheckBox1)
        Me.WinSVSettingsPanel.Location = New System.Drawing.Point(182, 196)
        Me.WinSVSettingsPanel.Name = "WinSVSettingsPanel"
        Me.WinSVSettingsPanel.Size = New System.Drawing.Size(505, 131)
        Me.WinSVSettingsPanel.TabIndex = 7
        '
        'Label14
        '
        Me.Label14.AutoEllipsis = True
        Me.Label14.Location = New System.Drawing.Point(28, 54)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(464, 70)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "Check this option only if the target system does not have any network capabilitie" & _
    "s. You can configure local users in the Users and Passwords section"
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(12, 34)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(215, 17)
        Me.CheckBox2.TabIndex = 0
        Me.CheckBox2.Text = "Bypass Mandatory Network Connection"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(12, 11)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(167, 17)
        Me.CheckBox1.TabIndex = 0
        Me.CheckBox1.Text = "Bypass System Requirements"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(52, 208)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(110, 13)
        Me.Label13.TabIndex = 6
        Me.Label13.Text = "Windows 11 settings:"
        '
        'Label12
        '
        Me.Label12.AutoEllipsis = True
        Me.Label12.Location = New System.Drawing.Point(182, 158)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(505, 32)
        Me.Label12.TabIndex = 5
        Me.Label12.Text = "Please select the system architecture that is supported by the target Windows ima" & _
    "ge to apply"
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Items.AddRange(New Object() {"x86 (Desktop 32-Bit)", "x64 (Desktop 64-Bit)", "ARM64 (Windows on ARM)"})
        Me.ListBox1.Location = New System.Drawing.Point(182, 82)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(505, 69)
        Me.ListBox1.TabIndex = 4
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(52, 84)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(119, 13)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "Processor architecture:"
        '
        'SysConfigHeader
        '
        Me.SysConfigHeader.AutoEllipsis = True
        Me.SysConfigHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.SysConfigHeader.Location = New System.Drawing.Point(16, 17)
        Me.SysConfigHeader.Name = "SysConfigHeader"
        Me.SysConfigHeader.Size = New System.Drawing.Size(708, 51)
        Me.SysConfigHeader.TabIndex = 2
        Me.SysConfigHeader.Text = "Configure basic system settings"
        '
        'RegionalSettingsPanel
        '
        Me.RegionalSettingsPanel.Controls.Add(Me.Label10)
        Me.RegionalSettingsPanel.Controls.Add(Me.RegionalSettings)
        Me.RegionalSettingsPanel.Controls.Add(Me.RadioButton2)
        Me.RegionalSettingsPanel.Controls.Add(Me.RadioButton1)
        Me.RegionalSettingsPanel.Controls.Add(Me.RegionalSettingsHeader)
        Me.RegionalSettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RegionalSettingsPanel.Location = New System.Drawing.Point(0, 0)
        Me.RegionalSettingsPanel.Name = "RegionalSettingsPanel"
        Me.RegionalSettingsPanel.Size = New System.Drawing.Size(752, 449)
        Me.RegionalSettingsPanel.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.AutoEllipsis = True
        Me.Label10.Enabled = False
        Me.Label10.Location = New System.Drawing.Point(84, 289)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(636, 68)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "You will need to configure these settings during the setup process"
        '
        'RegionalSettings
        '
        Me.RegionalSettings.Controls.Add(Me.Button1)
        Me.RegionalSettings.Controls.Add(Me.Label9)
        Me.RegionalSettings.Controls.Add(Me.Label8)
        Me.RegionalSettings.Controls.Add(Me.Label7)
        Me.RegionalSettings.Controls.Add(Me.Label6)
        Me.RegionalSettings.Controls.Add(Me.ComboBox4)
        Me.RegionalSettings.Controls.Add(Me.ComboBox3)
        Me.RegionalSettings.Controls.Add(Me.ComboBox2)
        Me.RegionalSettings.Controls.Add(Me.ComboBox1)
        Me.RegionalSettings.Location = New System.Drawing.Point(85, 100)
        Me.RegionalSettings.Name = "RegionalSettings"
        Me.RegionalSettings.Size = New System.Drawing.Size(635, 159)
        Me.RegionalSettings.TabIndex = 3
        '
        'Button1
        '
        Me.Button1.Enabled = False
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(485, 124)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(137, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Additional layouts"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoEllipsis = True
        Me.Label9.Location = New System.Drawing.Point(19, 99)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(253, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Home location:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label8
        '
        Me.Label8.AutoEllipsis = True
        Me.Label8.Location = New System.Drawing.Point(19, 73)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(253, 13)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Keyboard layout/IME:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label7
        '
        Me.Label7.AutoEllipsis = True
        Me.Label7.Location = New System.Drawing.Point(19, 45)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(253, 13)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "System locale:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label6
        '
        Me.Label6.AutoEllipsis = True
        Me.Label6.Location = New System.Drawing.Point(19, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(253, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "System language:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ComboBox4
        '
        Me.ComboBox4.FormattingEnabled = True
        Me.ComboBox4.Location = New System.Drawing.Point(278, 96)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.Size = New System.Drawing.Size(345, 21)
        Me.ComboBox4.TabIndex = 0
        Me.ComboBox4.Text = "United States"
        '
        'ComboBox3
        '
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Location = New System.Drawing.Point(278, 69)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(345, 21)
        Me.ComboBox3.TabIndex = 0
        Me.ComboBox3.Text = "US"
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(278, 42)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(345, 21)
        Me.ComboBox2.TabIndex = 0
        Me.ComboBox2.Text = "English (United States)"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(278, 15)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(345, 21)
        Me.ComboBox1.TabIndex = 0
        Me.ComboBox1.Text = "English"
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(65, 265)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(168, 17)
        Me.RadioButton2.TabIndex = 2
        Me.RadioButton2.Text = "Configure these settings later"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(65, 71)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(170, 17)
        Me.RadioButton1.TabIndex = 2
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Configure these settings now:"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RegionalSettingsHeader
        '
        Me.RegionalSettingsHeader.AutoEllipsis = True
        Me.RegionalSettingsHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RegionalSettingsHeader.Location = New System.Drawing.Point(16, 17)
        Me.RegionalSettingsHeader.Name = "RegionalSettingsHeader"
        Me.RegionalSettingsHeader.Size = New System.Drawing.Size(708, 51)
        Me.RegionalSettingsHeader.TabIndex = 1
        Me.RegionalSettingsHeader.Text = "Configure your language, keyboard layout, and other regional settings"
        '
        'Label5
        '
        Me.Label5.AutoEllipsis = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(141, 167)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(471, 115)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Not available for now!"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label5.Visible = False
        '
        'EditorPanelContainer
        '
        Me.EditorPanelContainer.Controls.Add(Me.Scintilla1)
        Me.EditorPanelContainer.Controls.Add(Me.DarkToolStrip1)
        Me.EditorPanelContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EditorPanelContainer.Location = New System.Drawing.Point(256, 72)
        Me.EditorPanelContainer.Name = "EditorPanelContainer"
        Me.EditorPanelContainer.Size = New System.Drawing.Size(752, 449)
        Me.EditorPanelContainer.TabIndex = 1
        Me.EditorPanelContainer.Visible = False
        '
        'Scintilla1
        '
        Me.Scintilla1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Scintilla1.IndentationGuides = ScintillaNET.IndentView.LookBoth
        Me.Scintilla1.LexerName = Nothing
        Me.Scintilla1.Location = New System.Drawing.Point(0, 28)
        Me.Scintilla1.Name = "Scintilla1"
        Me.Scintilla1.Size = New System.Drawing.Size(752, 421)
        Me.Scintilla1.TabIndex = 2
        Me.Scintilla1.Text = "<?xml version=""1.0"" encoding=""utf-8""?>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<unattend xmlns=""urn:schemas-microsoft-co" & _
    "m:unattend"">" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "</unattend>"
        '
        'DarkToolStrip1
        '
        Me.DarkToolStrip1.AutoSize = False
        Me.DarkToolStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
        Me.DarkToolStrip1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DarkToolStrip1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.DarkToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.DarkToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton2, Me.ToolStripButton3, Me.ToolStripButton4, Me.ToolStripSeparator4, Me.FontFamilyTSCB, Me.FontSizeTSCB, Me.ToolStripSeparator5, Me.ToolStripButton5, Me.ToolStripSeparator6, Me.ToolStripButton6})
        Me.DarkToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.DarkToolStrip1.Name = "DarkToolStrip1"
        Me.DarkToolStrip1.Padding = New System.Windows.Forms.Padding(5, 0, 1, 0)
        Me.DarkToolStrip1.Size = New System.Drawing.Size(752, 28)
        Me.DarkToolStrip1.TabIndex = 3
        Me.DarkToolStrip1.Text = "DarkToolStrip1"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.ToolStripButton2.Image = Global.DISMTools.My.Resources.Resources.newfile_dark
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(23, 25)
        Me.ToolStripButton2.Text = "New"
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.ToolStripButton3.Image = Global.DISMTools.My.Resources.Resources.openfile_dark
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(23, 25)
        Me.ToolStripButton3.Text = "Open..."
        '
        'ToolStripButton4
        '
        Me.ToolStripButton4.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.ToolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.ToolStripButton4.Image = Global.DISMTools.My.Resources.Resources.save_glyph_dark
        Me.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton4.Name = "ToolStripButton4"
        Me.ToolStripButton4.Size = New System.Drawing.Size(23, 25)
        Me.ToolStripButton4.Text = "Save as..."
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.ToolStripSeparator4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.ToolStripSeparator4.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 28)
        '
        'FontFamilyTSCB
        '
        Me.FontFamilyTSCB.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.FontFamilyTSCB.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FontFamilyTSCB.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.FontFamilyTSCB.Name = "FontFamilyTSCB"
        Me.FontFamilyTSCB.Size = New System.Drawing.Size(121, 28)
        '
        'FontSizeTSCB
        '
        Me.FontSizeTSCB.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.FontSizeTSCB.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FontSizeTSCB.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.FontSizeTSCB.Items.AddRange(New Object() {"8", "9", "10", "11", "12", "14", "16", "18", "20", "24", "28", "36", "48", "72", "96"})
        Me.FontSizeTSCB.Name = "FontSizeTSCB"
        Me.FontSizeTSCB.Size = New System.Drawing.Size(75, 28)
        Me.FontSizeTSCB.Text = "10"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.ToolStripSeparator5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.ToolStripSeparator5.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 28)
        '
        'ToolStripButton5
        '
        Me.ToolStripButton5.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.ToolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.ToolStripButton5.Image = Global.DISMTools.My.Resources.Resources.wordwrap_dark
        Me.ToolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton5.Name = "ToolStripButton5"
        Me.ToolStripButton5.Size = New System.Drawing.Size(23, 25)
        Me.ToolStripButton5.Text = "Word wrap"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.ToolStripSeparator6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.ToolStripSeparator6.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 28)
        '
        'ToolStripButton6
        '
        Me.ToolStripButton6.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton6.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(63, Byte), Integer), CType(CType(65, Byte), Integer))
        Me.ToolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.ToolStripButton6.Image = Global.DISMTools.My.Resources.Resources.help_glyph_dark
        Me.ToolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton6.Name = "ToolStripButton6"
        Me.ToolStripButton6.Size = New System.Drawing.Size(23, 25)
        Me.ToolStripButton6.Text = "Help"
        '
        'HeaderPanel
        '
        Me.HeaderPanel.Controls.Add(Me.Label4)
        Me.HeaderPanel.Controls.Add(Me.Label3)
        Me.HeaderPanel.Controls.Add(Me.PictureBox3)
        Me.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.HeaderPanel.Location = New System.Drawing.Point(256, 0)
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.Size = New System.Drawing.Size(752, 72)
        Me.HeaderPanel.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoEllipsis = True
        Me.Label4.Location = New System.Drawing.Point(52, 41)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(672, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "If you haven't created unattended answer files before, use this wizard to create " & _
    "one."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.Label3.Location = New System.Drawing.Point(48, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(141, 30)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Express mode"
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.DISMTools.My.Resources.Resources.express_mode_fc
        Me.PictureBox3.Location = New System.Drawing.Point(10, 8)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox3.TabIndex = 0
        Me.PictureBox3.TabStop = False
        '
        'FooterContainer
        '
        Me.FooterContainer.Controls.Add(Me.ExpressPanelFooter)
        Me.FooterContainer.Controls.Add(Me.EditorPanelFooter)
        Me.FooterContainer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FooterContainer.Location = New System.Drawing.Point(256, 521)
        Me.FooterContainer.Name = "FooterContainer"
        Me.FooterContainer.Size = New System.Drawing.Size(752, 40)
        Me.FooterContainer.TabIndex = 3
        '
        'ExpressPanelFooter
        '
        Me.ExpressPanelFooter.Controls.Add(Me.TableLayoutPanel1)
        Me.ExpressPanelFooter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExpressPanelFooter.Location = New System.Drawing.Point(0, 0)
        Me.ExpressPanelFooter.Name = "ExpressPanelFooter"
        Me.ExpressPanelFooter.Size = New System.Drawing.Size(752, 40)
        Me.ExpressPanelFooter.TabIndex = 0
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(458, 6)
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
        Me.Next_Button.Enabled = False
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
        'EditorPanelFooter
        '
        Me.EditorPanelFooter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EditorPanelFooter.Location = New System.Drawing.Point(0, 0)
        Me.EditorPanelFooter.Name = "EditorPanelFooter"
        Me.EditorPanelFooter.Size = New System.Drawing.Size(752, 40)
        Me.EditorPanelFooter.TabIndex = 0
        '
        'TimeZonePageTimer
        '
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "All Files|*.*"
        '
        'UnattendGeneratorBW
        '
        Me.UnattendGeneratorBW.WorkerReportsProgress = True
        Me.UnattendGeneratorBW.WorkerSupportsCancellation = True
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "Answer files|*.xml"
        '
        'NewUnattendWiz
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 561)
        Me.Controls.Add(Me.ExpressPanelContainer)
        Me.Controls.Add(Me.EditorPanelContainer)
        Me.Controls.Add(Me.HeaderPanel)
        Me.Controls.Add(Me.FooterContainer)
        Me.Controls.Add(Me.SidePanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1024, 600)
        Me.Name = "NewUnattendWiz"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Unattended answer file creation wizard"
        Me.SidePanel.ResumeLayout(False)
        Me.ExpressModeSteps.ResumeLayout(False)
        Me.EditorPanelTrigger.ResumeLayout(False)
        Me.EditorPanelTrigger.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ExpressPanelTrigger.ResumeLayout(False)
        Me.ExpressPanelTrigger.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ExpressPanelContainer.ResumeLayout(False)
        Me.ExperimentalPanel.ResumeLayout(False)
        Me.StepsContainer.ResumeLayout(False)
        Me.WelcomePanel.ResumeLayout(False)
        Me.FinishPanel.ResumeLayout(False)
        Me.FinishPanel.PerformLayout()
        Me.UnattendProgressPanel.ResumeLayout(False)
        Me.UnattendProgressPanel.PerformLayout()
        Me.FinalReviewPanel.ResumeLayout(False)
        Me.FinalReviewPanel.PerformLayout()
        Me.ComponentPanel.ResumeLayout(False)
        Me.PostInstallPanel.ResumeLayout(False)
        Me.PostInstallPanel.PerformLayout()
        Me.SystemTelemetryPanel.ResumeLayout(False)
        Me.SystemTelemetryPanel.PerformLayout()
        Me.TelemetryOptionsPanel.ResumeLayout(False)
        Me.TelemetryOptionsPanel.PerformLayout()
        Me.NetworkConnectionPanel.ResumeLayout(False)
        Me.NetworkConnectionPanel.PerformLayout()
        Me.ManualNetworkConfigPanel.ResumeLayout(False)
        Me.ManualNetworkConfigPanel.PerformLayout()
        Me.WirelessNetworkSettingsPanel.ResumeLayout(False)
        Me.WirelessNetworkSettingsPanel.PerformLayout()
        Me.VirtualMachinePanel.ResumeLayout(False)
        Me.VirtualMachinePanel.PerformLayout()
        Me.VMProviderPanel.ResumeLayout(False)
        Me.VMProviderPanel.PerformLayout()
        Me.AccountLockdownPanel.ResumeLayout(False)
        Me.AccountLockdownPanel.PerformLayout()
        Me.EnabledAccountLockdownPanel.ResumeLayout(False)
        Me.EnabledAccountLockdownPanel.PerformLayout()
        Me.AccountLockdownParametersPanel.ResumeLayout(False)
        Me.AccountLockdownParametersPanel.PerformLayout()
        CType(Me.NumericUpDown8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PWExpirationPanel.ResumeLayout(False)
        Me.PWExpirationPanel.PerformLayout()
        Me.AutoExpirationPanel.ResumeLayout(False)
        Me.AutoExpirationPanel.PerformLayout()
        Me.TimedExpirationPanel.ResumeLayout(False)
        Me.TimedExpirationPanel.PerformLayout()
        CType(Me.NumericUpDown5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UserAccountPanel.ResumeLayout(False)
        Me.UserAccountPanel.PerformLayout()
        Me.ManualAccountPanel.ResumeLayout(False)
        Me.ManualAccountPanel.PerformLayout()
        Me.UserAccountListing.ResumeLayout(False)
        Me.AccountsPanel.ResumeLayout(False)
        Me.AccountsPanel.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.AutoLogonSettingsPanel.ResumeLayout(False)
        Me.AutoLogonSettingsPanel.PerformLayout()
        Me.ProductKeyPanel.ResumeLayout(False)
        Me.ProductKeyPanel.PerformLayout()
        Me.ManualKeyPanel.ResumeLayout(False)
        Me.ManualKeyPanel.PerformLayout()
        Me.KeyVerifyWarningPanel.ResumeLayout(False)
        Me.KeyVerifyWarningPanel.PerformLayout()
        Me.GenericKeyPanel.ResumeLayout(False)
        Me.GenericKeyPanel.PerformLayout()
        Me.DiskConfigurationPanel.ResumeLayout(False)
        Me.DiskConfigurationPanel.PerformLayout()
        Me.ManualPartPanel.ResumeLayout(False)
        Me.ManualPartPanel.PerformLayout()
        Me.AutoDiskConfigPanel.ResumeLayout(False)
        Me.AutoDiskConfigPanel.PerformLayout()
        Me.WindowsREPanel.ResumeLayout(False)
        Me.WindowsREPanel.PerformLayout()
        Me.RESizePanel.ResumeLayout(False)
        CType(Me.NumericUpDown2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PartTablePanel.ResumeLayout(False)
        Me.PartTablePanel.PerformLayout()
        Me.ESPPanel.ResumeLayout(False)
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DiskPartPanel.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.DPActionsPanel.ResumeLayout(False)
        Me.ManualInstallPanel.ResumeLayout(False)
        Me.ManualInstallPanel.PerformLayout()
        CType(Me.NumericUpDown4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TimeZonePanel.ResumeLayout(False)
        Me.TimeZonePanel.PerformLayout()
        Me.TimeZoneSettings.ResumeLayout(False)
        Me.TimeZoneSettings.PerformLayout()
        Me.SysConfigPanel.ResumeLayout(False)
        Me.SysConfigPanel.PerformLayout()
        Me.ComputerNamePanel.ResumeLayout(False)
        Me.ComputerNamePanel.PerformLayout()
        Me.WinSVSettingsPanel.ResumeLayout(False)
        Me.WinSVSettingsPanel.PerformLayout()
        Me.RegionalSettingsPanel.ResumeLayout(False)
        Me.RegionalSettingsPanel.PerformLayout()
        Me.RegionalSettings.ResumeLayout(False)
        Me.EditorPanelContainer.ResumeLayout(False)
        Me.DarkToolStrip1.ResumeLayout(False)
        Me.DarkToolStrip1.PerformLayout()
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FooterContainer.ResumeLayout(False)
        Me.ExpressPanelFooter.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SidePanel As System.Windows.Forms.Panel
    Friend WithEvents EditorPanelTrigger As System.Windows.Forms.Panel
    Friend WithEvents ExpressPanelTrigger As System.Windows.Forms.Panel
    Friend WithEvents ExpressPanelContainer As System.Windows.Forms.Panel
    Friend WithEvents EditorPanelContainer As System.Windows.Forms.Panel
    Friend WithEvents ExpressModeSteps As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents HeaderPanel As System.Windows.Forms.Panel
    Friend WithEvents FooterContainer As System.Windows.Forms.Panel
    Friend WithEvents ExpressPanelFooter As System.Windows.Forms.Panel
    Friend WithEvents EditorPanelFooter As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents StepsTreeView As System.Windows.Forms.TreeView
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Back_Button As System.Windows.Forms.Button
    Friend WithEvents Next_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Help_Button As System.Windows.Forms.Button
    Friend WithEvents Scintilla1 As ScintillaNET.Scintilla
    Friend WithEvents DarkToolStrip1 As DarkUI.Controls.DarkToolStrip
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents FontFamilyTSCB As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents FontSizeTSCB As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton5 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton6 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ExperimentalPanel As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents StepsContainer As System.Windows.Forms.Panel
    Friend WithEvents RegionalSettingsPanel As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents RegionalSettings As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboBox4 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RegionalSettingsHeader As System.Windows.Forms.Label
    Friend WithEvents WelcomePanel As System.Windows.Forms.Panel
    Friend WithEvents WelcomeDesc As System.Windows.Forms.Label
    Friend WithEvents SysConfigPanel As System.Windows.Forms.Panel
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ComputerNamePanel As System.Windows.Forms.Panel
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents WinSVSettingsPanel As System.Windows.Forms.Panel
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents SysConfigHeader As System.Windows.Forms.Label
    Friend WithEvents TimeZonePanel As System.Windows.Forms.Panel
    Friend WithEvents TimeZoneSettings As System.Windows.Forms.Panel
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents TimeZoneHeader As System.Windows.Forms.Label
    Friend WithEvents CurrentTimeSelTZ As System.Windows.Forms.Label
    Friend WithEvents CurrentTimeUTC As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents ComboBox5 As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents TimeZonePageTimer As System.Windows.Forms.Timer
    Friend WithEvents DiskConfigurationPanel As System.Windows.Forms.Panel
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents DiskConfigurationHeader As System.Windows.Forms.Label
    Friend WithEvents ManualPartPanel As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
    Friend WithEvents AutoDiskConfigPanel As System.Windows.Forms.Panel
    Friend WithEvents WindowsREPanel As System.Windows.Forms.Panel
    Friend WithEvents RESizePanel As System.Windows.Forms.Panel
    Friend WithEvents NumericUpDown2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents RadioButton10 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton9 As System.Windows.Forms.RadioButton
    Friend WithEvents CheckBox5 As System.Windows.Forms.CheckBox
    Friend WithEvents PartTablePanel As System.Windows.Forms.Panel
    Friend WithEvents ESPPanel As System.Windows.Forms.Panel
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents RadioButton8 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton7 As System.Windows.Forms.RadioButton
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents RadioButton6 As System.Windows.Forms.RadioButton
    Friend WithEvents DiskPartPanel As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Scintilla2 As ScintillaNET.Scintilla
    Friend WithEvents DPActionsPanel As System.Windows.Forms.Panel
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ManualInstallPanel As System.Windows.Forms.Panel
    Friend WithEvents NumericUpDown4 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown3 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents RadioButton12 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton11 As System.Windows.Forms.RadioButton
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents FillerLabel As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ProductKeyPanel As System.Windows.Forms.Panel
    Friend WithEvents RadioButton13 As System.Windows.Forms.RadioButton
    Friend WithEvents ProductKeyHeader As System.Windows.Forms.Label
    Friend WithEvents ManualKeyPanel As System.Windows.Forms.Panel
    Friend WithEvents KeyVerifyWarningPanel As System.Windows.Forms.Panel
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents GenericKeyPanel As System.Windows.Forms.Panel
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents ComboBox6 As System.Windows.Forms.ComboBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents RadioButton14 As System.Windows.Forms.RadioButton
    Friend WithEvents UserAccountPanel As System.Windows.Forms.Panel
    Friend WithEvents UserAccountHeader As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents CheckBox6 As System.Windows.Forms.CheckBox
    Friend WithEvents ManualAccountPanel As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents UserAccountListing As System.Windows.Forms.Panel
    Friend WithEvents AccountsPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents CheckBox7 As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBox12 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox11 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox10 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox9 As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox18 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox17 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox15 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox14 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox12 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents CheckBox8 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox9 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox10 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox11 As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents ComboBox7 As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents AutoLogonSettingsPanel As System.Windows.Forms.Panel
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents RadioButton16 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton15 As System.Windows.Forms.RadioButton
    Friend WithEvents CheckBox12 As System.Windows.Forms.CheckBox
    Friend WithEvents PWExpirationPanel As System.Windows.Forms.Panel
    Friend WithEvents FillerLabel2 As System.Windows.Forms.Label
    Friend WithEvents AutoExpirationPanel As System.Windows.Forms.Panel
    Friend WithEvents TimedExpirationPanel As System.Windows.Forms.Panel
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown5 As System.Windows.Forms.NumericUpDown
    Friend WithEvents RadioButton20 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton19 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton18 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton17 As System.Windows.Forms.RadioButton
    Friend WithEvents PWExpirationHeader As System.Windows.Forms.Label
    Friend WithEvents AccountLockdownPanel As System.Windows.Forms.Panel
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents EnabledAccountLockdownPanel As System.Windows.Forms.Panel
    Friend WithEvents AccountLockdownParametersPanel As System.Windows.Forms.Panel
    Friend WithEvents NumericUpDown8 As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDown7 As System.Windows.Forms.NumericUpDown
    Friend WithEvents NumericUpDown6 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents RadioButton22 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton21 As System.Windows.Forms.RadioButton
    Friend WithEvents CheckBox13 As System.Windows.Forms.CheckBox
    Friend WithEvents AccountLockdownHeader As System.Windows.Forms.Label
    Friend WithEvents VirtualMachinePanel As System.Windows.Forms.Panel
    Friend WithEvents VMProviderPanel As System.Windows.Forms.Panel
    Friend WithEvents ComboBox8 As System.Windows.Forms.ComboBox
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents RadioButton24 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton23 As System.Windows.Forms.RadioButton
    Friend WithEvents VirtualMachineHeader As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents NetworkConnectionPanel As System.Windows.Forms.Panel
    Friend WithEvents NetworkConnectionHeader As System.Windows.Forms.Label
    Friend WithEvents CheckBox14 As System.Windows.Forms.CheckBox
    Friend WithEvents ManualNetworkConfigPanel As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents RadioButton25 As System.Windows.Forms.RadioButton
    Friend WithEvents WirelessNetworkSettingsPanel As System.Windows.Forms.Panel
    Friend WithEvents ComboBox13 As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents CheckBox15 As System.Windows.Forms.CheckBox
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents RadioButton30 As System.Windows.Forms.RadioButton
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents SystemTelemetryPanel As System.Windows.Forms.Panel
    Friend WithEvents SystemTelemetryHeader As System.Windows.Forms.Label
    Friend WithEvents TelemetryOptionsPanel As System.Windows.Forms.Panel
    Friend WithEvents CheckBox16 As System.Windows.Forms.CheckBox
    Friend WithEvents RadioButton27 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton26 As System.Windows.Forms.RadioButton
    Friend WithEvents FinalReviewPanel As System.Windows.Forms.Panel
    Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
    Friend WithEvents FinalReviewHeader As System.Windows.Forms.Label
    Friend WithEvents ComponentPanel As System.Windows.Forms.Panel
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents ComponentHeader As System.Windows.Forms.Label
    Friend WithEvents PostInstallPanel As System.Windows.Forms.Panel
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents PostInstallHeader As System.Windows.Forms.Label
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents CheckBox17 As System.Windows.Forms.CheckBox
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents UnattendProgressPanel As System.Windows.Forms.Panel
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents UnattendProgressHeader As System.Windows.Forms.Label
    Friend WithEvents UnattendGeneratorBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents FinishPanel As System.Windows.Forms.Panel
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents FinishHeader As System.Windows.Forms.Label
    Friend WithEvents LinkLabel4 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel3 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents WelcomeHeader As System.Windows.Forms.Label
End Class
