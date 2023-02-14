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
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Before beginning")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Target operating system selection")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Product activation")
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("License agreement")
        Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Computer personalization")
        Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Partitioning settings")
        Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Operating system setup", New System.Windows.Forms.TreeNode() {TreeNode3, TreeNode4, TreeNode5, TreeNode6})
        Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Regional settings")
        Dim TreeNode9 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("End-user License Agreement")
        Dim TreeNode10 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("OOBE skips")
        Dim TreeNode11 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Network security")
        Dim TreeNode12 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Wireless configuration")
        Dim TreeNode13 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Computer protection")
        Dim TreeNode14 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Control Panel")
        Dim TreeNode15 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Out-of-box experience", New System.Windows.Forms.TreeNode() {TreeNode9, TreeNode10, TreeNode11, TreeNode12, TreeNode13, TreeNode14})
        Dim TreeNode16 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Account personalization")
        Dim TreeNode17 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Account security")
        Dim TreeNode18 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Customer Experience Improvement Program")
        Dim TreeNode19 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("User account settings", New System.Windows.Forms.TreeNode() {TreeNode16, TreeNode17, TreeNode18})
        Dim TreeNode20 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Ready to begin")
        Dim TreeNode21 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Creation in progress")
        Dim TreeNode22 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Finish")
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
        Me.ActivationPanel = New System.Windows.Forms.Panel()
        Me.KeyCopyButton = New System.Windows.Forms.Button()
        Me.CheckBox5 = New System.Windows.Forms.CheckBox()
        Me.CheckBox4 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.KeyInputBox5 = New System.Windows.Forms.TextBox()
        Me.GenericBox5 = New System.Windows.Forms.TextBox()
        Me.KeyInputBox4 = New System.Windows.Forms.TextBox()
        Me.GenericBox4 = New System.Windows.Forms.TextBox()
        Me.KeyInputBox3 = New System.Windows.Forms.TextBox()
        Me.GenericBox3 = New System.Windows.Forms.TextBox()
        Me.KeyInputBox2 = New System.Windows.Forms.TextBox()
        Me.GenericBox2 = New System.Windows.Forms.TextBox()
        Me.KeyInputBox1 = New System.Windows.Forms.TextBox()
        Me.KeyDash4 = New System.Windows.Forms.Label()
        Me.GenericBox1 = New System.Windows.Forms.TextBox()
        Me.KeyDash3 = New System.Windows.Forms.Label()
        Me.GenericDash4 = New System.Windows.Forms.Label()
        Me.KeyDash2 = New System.Windows.Forms.Label()
        Me.GenericDash3 = New System.Windows.Forms.Label()
        Me.KeyDash1 = New System.Windows.Forms.Label()
        Me.GenericDash2 = New System.Windows.Forms.Label()
        Me.GenericDash1 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.IncompleteWarningPanel = New System.Windows.Forms.Panel()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TargetOSSelectionPanel = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.EndUserLicenseAgreementPanel = New System.Windows.Forms.Panel()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.CheckBox6 = New System.Windows.Forms.CheckBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.CompPersonalizationPanel = New System.Windows.Forms.Panel()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.PictureBox7 = New System.Windows.Forms.PictureBox()
        Me.DiskPartPanel = New System.Windows.Forms.Panel()
        Me.PartStylePanel = New System.Windows.Forms.Panel()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.RadioButton7 = New System.Windows.Forms.RadioButton()
        Me.RadioButton8 = New System.Windows.Forms.RadioButton()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.ComboBox4 = New System.Windows.Forms.ComboBox()
        Me.RadioButton6 = New System.Windows.Forms.RadioButton()
        Me.RadioButton5 = New System.Windows.Forms.RadioButton()
        Me.NumericUpDown2 = New System.Windows.Forms.NumericUpDown()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.CheckBox8 = New System.Windows.Forms.CheckBox()
        Me.CheckBox7 = New System.Windows.Forms.CheckBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.PictureBox8 = New System.Windows.Forms.PictureBox()
        Me.RegionalSettingsPanel = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.ComboBox8 = New System.Windows.Forms.ComboBox()
        Me.ComboBox6 = New System.Windows.Forms.ComboBox()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.ComboBox7 = New System.Windows.Forms.ComboBox()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.ComboBox5 = New System.Windows.Forms.ComboBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.PictureBox9 = New System.Windows.Forms.PictureBox()
        Me.UserEulaPanel = New System.Windows.Forms.Panel()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.EulaScreen = New System.Windows.Forms.PictureBox()
        Me.CheckBox9 = New System.Windows.Forms.CheckBox()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.PictureBox10 = New System.Windows.Forms.PictureBox()
        Me.OobeSkipsPanel = New System.Windows.Forms.Panel()
        Me.Label64 = New System.Windows.Forms.Label()
        Me.Label65 = New System.Windows.Forms.Label()
        Me.Label63 = New System.Windows.Forms.Label()
        Me.CheckBox11 = New System.Windows.Forms.CheckBox()
        Me.CheckBox10 = New System.Windows.Forms.CheckBox()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.PictureBox11 = New System.Windows.Forms.PictureBox()
        Me.NetSecurityPanel = New System.Windows.Forms.Panel()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.PictureBox13 = New System.Windows.Forms.PictureBox()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.ComboBox9 = New System.Windows.Forms.ComboBox()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.Label66 = New System.Windows.Forms.Label()
        Me.PictureBox12 = New System.Windows.Forms.PictureBox()
        Me.WirelessPanel = New System.Windows.Forms.Panel()
        Me.CheckBox12 = New System.Windows.Forms.CheckBox()
        Me.Label70 = New System.Windows.Forms.Label()
        Me.Label69 = New System.Windows.Forms.Label()
        Me.PictureBox14 = New System.Windows.Forms.PictureBox()
        Me.CompProtectPanel = New System.Windows.Forms.Panel()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.WindowsUpdateDescPanel = New System.Windows.Forms.Panel()
        Me.TrackBarSelValueMsg2 = New System.Windows.Forms.Label()
        Me.TrackBarSelValueDesc2 = New System.Windows.Forms.Label()
        Me.PictureBox16 = New System.Windows.Forms.PictureBox()
        Me.TrackBar2 = New System.Windows.Forms.TrackBar()
        Me.Label73 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GeneralProtectionDescPanel = New System.Windows.Forms.Panel()
        Me.TrackBarSelValueMsg1 = New System.Windows.Forms.Label()
        Me.TrackBarSelValueDesc1 = New System.Windows.Forms.Label()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.Label72 = New System.Windows.Forms.Label()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.PictureBox15 = New System.Windows.Forms.PictureBox()
        Me.ControlPanel = New System.Windows.Forms.Panel()
        Me.ControlPreview = New System.Windows.Forms.PictureBox()
        Me.ComboBox11 = New System.Windows.Forms.ComboBox()
        Me.Label76 = New System.Windows.Forms.Label()
        Me.ComboBox10 = New System.Windows.Forms.ComboBox()
        Me.Label75 = New System.Windows.Forms.Label()
        Me.Label74 = New System.Windows.Forms.Label()
        Me.PictureBox17 = New System.Windows.Forms.PictureBox()
        Me.UsrPersonalizationPanel = New System.Windows.Forms.Panel()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.StartButton = New System.Windows.Forms.PictureBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.Win8Color1 = New System.Windows.Forms.PictureBox()
        Me.Win8Color2 = New System.Windows.Forms.PictureBox()
        Me.Win8Color3 = New System.Windows.Forms.PictureBox()
        Me.Win8Color4 = New System.Windows.Forms.PictureBox()
        Me.Win8Color5 = New System.Windows.Forms.PictureBox()
        Me.Win8Color6 = New System.Windows.Forms.PictureBox()
        Me.Win8Color7 = New System.Windows.Forms.PictureBox()
        Me.Win8Color8 = New System.Windows.Forms.PictureBox()
        Me.Win8Color9 = New System.Windows.Forms.PictureBox()
        Me.Win8Color10 = New System.Windows.Forms.PictureBox()
        Me.Win8Color11 = New System.Windows.Forms.PictureBox()
        Me.Win8Color12 = New System.Windows.Forms.PictureBox()
        Me.Win8Color13 = New System.Windows.Forms.PictureBox()
        Me.Win8Color14 = New System.Windows.Forms.PictureBox()
        Me.Win8Color15 = New System.Windows.Forms.PictureBox()
        Me.Win8Color16 = New System.Windows.Forms.PictureBox()
        Me.Win8Color17 = New System.Windows.Forms.PictureBox()
        Me.Win8Color18 = New System.Windows.Forms.PictureBox()
        Me.Win8Color19 = New System.Windows.Forms.PictureBox()
        Me.Win8Color20 = New System.Windows.Forms.PictureBox()
        Me.Win8Color21 = New System.Windows.Forms.PictureBox()
        Me.Win8Color22 = New System.Windows.Forms.PictureBox()
        Me.Win8Color23 = New System.Windows.Forms.PictureBox()
        Me.Win8Color24 = New System.Windows.Forms.PictureBox()
        Me.Win8Color25 = New System.Windows.Forms.PictureBox()
        Me.Label81 = New System.Windows.Forms.Label()
        Me.StartScreen = New System.Windows.Forms.Panel()
        Me.StartScreenLabel = New System.Windows.Forms.Label()
        Me.Label80 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.PictureBox19 = New System.Windows.Forms.PictureBox()
        Me.Label79 = New System.Windows.Forms.Label()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.PictureBox18 = New System.Windows.Forms.PictureBox()
        Me.UsrSecurityPanel = New System.Windows.Forms.Panel()
        Me.ComboBox12 = New System.Windows.Forms.ComboBox()
        Me.CheckBox17 = New System.Windows.Forms.CheckBox()
        Me.CheckBox16 = New System.Windows.Forms.CheckBox()
        Me.CheckBox14 = New System.Windows.Forms.CheckBox()
        Me.CheckBox15 = New System.Windows.Forms.CheckBox()
        Me.CheckBox13 = New System.Windows.Forms.CheckBox()
        Me.PasswordRepeatBox = New System.Windows.Forms.MaskedTextBox()
        Me.PasswordBox = New System.Windows.Forms.MaskedTextBox()
        Me.UserNameLabel = New System.Windows.Forms.Label()
        Me.Label85 = New System.Windows.Forms.Label()
        Me.Label84 = New System.Windows.Forms.Label()
        Me.PictureBox21 = New System.Windows.Forms.PictureBox()
        Me.Label83 = New System.Windows.Forms.Label()
        Me.Label82 = New System.Windows.Forms.Label()
        Me.PictureBox20 = New System.Windows.Forms.PictureBox()
        Me.UsrCeipPanel = New System.Windows.Forms.Panel()
        Me.CheckBox18 = New System.Windows.Forms.CheckBox()
        Me.Label87 = New System.Windows.Forms.Label()
        Me.Label86 = New System.Windows.Forms.Label()
        Me.PictureBox22 = New System.Windows.Forms.PictureBox()
        Me.SettingRecapPanel = New System.Windows.Forms.Panel()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.Label88 = New System.Windows.Forms.Label()
        Me.PictureBox23 = New System.Windows.Forms.PictureBox()
        Me.EditorPanelContainer = New System.Windows.Forms.Panel()
        Me.Scintilla1 = New ScintillaNET.Scintilla()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.NewTSB = New System.Windows.Forms.ToolStripButton()
        Me.OpenTSB = New System.Windows.Forms.ToolStripButton()
        Me.SaveTSB = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripComboBox1 = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripComboBox2 = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.HelpTSB = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.HeaderPanel = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.FooterContainer = New System.Windows.Forms.Panel()
        Me.ExpressPanelFooter = New System.Windows.Forms.Panel()
        Me.ExpressStatusLbl = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Back_Button = New System.Windows.Forms.Button()
        Me.Next_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Help_Button = New System.Windows.Forms.Button()
        Me.EditorPanelFooter = New System.Windows.Forms.Panel()
        Me.LocationPanel = New System.Windows.Forms.Panel()
        Me.LocationLbl = New System.Windows.Forms.Label()
        Me.SidePanel.SuspendLayout()
        Me.ExpressModeSteps.SuspendLayout()
        Me.EditorPanelTrigger.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ExpressPanelTrigger.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ExpressPanelContainer.SuspendLayout()
        Me.ActivationPanel.SuspendLayout()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.IncompleteWarningPanel.SuspendLayout()
        Me.TargetOSSelectionPanel.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EndUserLicenseAgreementPanel.SuspendLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CompPersonalizationPanel.SuspendLayout()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DiskPartPanel.SuspendLayout()
        Me.PartStylePanel.SuspendLayout()
        CType(Me.NumericUpDown2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RegionalSettingsPanel.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UserEulaPanel.SuspendLayout()
        CType(Me.EulaScreen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox10, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.OobeSkipsPanel.SuspendLayout()
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.NetSecurityPanel.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.PictureBox13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox12, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WirelessPanel.SuspendLayout()
        CType(Me.PictureBox14, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CompProtectPanel.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.WindowsUpdateDescPanel.SuspendLayout()
        CType(Me.PictureBox16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBar2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.GeneralProtectionDescPanel.SuspendLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox15, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ControlPanel.SuspendLayout()
        CType(Me.ControlPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox17, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UsrPersonalizationPanel.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        CType(Me.StartButton, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.Win8Color1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color16, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color17, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color18, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color19, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color20, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color22, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color23, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color24, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Win8Color25, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StartScreen.SuspendLayout()
        CType(Me.PictureBox19, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox18, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UsrSecurityPanel.SuspendLayout()
        CType(Me.PictureBox21, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox20, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UsrCeipPanel.SuspendLayout()
        CType(Me.PictureBox22, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SettingRecapPanel.SuspendLayout()
        CType(Me.PictureBox23, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EditorPanelContainer.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.HeaderPanel.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FooterContainer.SuspendLayout()
        Me.ExpressPanelFooter.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.LocationPanel.SuspendLayout()
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
        Me.ExpressModeSteps.Size = New System.Drawing.Size(256, 481)
        Me.ExpressModeSteps.TabIndex = 2
        '
        'StepsTreeView
        '
        Me.StepsTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StepsTreeView.HideSelection = False
        Me.StepsTreeView.ItemHeight = 24
        Me.StepsTreeView.Location = New System.Drawing.Point(0, 0)
        Me.StepsTreeView.Name = "StepsTreeView"
        TreeNode1.Name = "StartNode"
        TreeNode1.Text = "Before beginning"
        TreeNode2.Name = "TargetOSNode"
        TreeNode2.Text = "Target operating system selection"
        TreeNode3.Name = "Activation"
        TreeNode3.Text = "Product activation"
        TreeNode4.Name = "License"
        TreeNode4.Text = "License agreement"
        TreeNode5.Name = "CompPersonalize"
        TreeNode5.Text = "Computer personalization"
        TreeNode6.Name = "Partitioning"
        TreeNode6.Text = "Partitioning settings"
        TreeNode7.Name = "OSSetup"
        TreeNode7.Text = "Operating system setup"
        TreeNode8.Name = "Region"
        TreeNode8.Text = "Regional settings"
        TreeNode9.Name = "EULA"
        TreeNode9.Text = "End-user License Agreement"
        TreeNode10.Name = "OOBESkip"
        TreeNode10.Text = "OOBE skips"
        TreeNode11.Name = "OOBENet"
        TreeNode11.Text = "Network security"
        TreeNode12.Name = "Wireless"
        TreeNode12.Text = "Wireless configuration"
        TreeNode13.Name = "CompProtect"
        TreeNode13.Text = "Computer protection"
        TreeNode14.Name = "CPL"
        TreeNode14.Text = "Control Panel"
        TreeNode15.Name = "OOBE"
        TreeNode15.Text = "Out-of-box experience"
        TreeNode16.Name = "User"
        TreeNode16.Text = "Account personalization"
        TreeNode17.Name = "UserSec"
        TreeNode17.Text = "Account security"
        TreeNode18.Name = "CEIP"
        TreeNode18.Text = "Customer Experience Improvement Program"
        TreeNode19.Name = "UserAccount"
        TreeNode19.Text = "User account settings"
        TreeNode20.Name = "Prepare"
        TreeNode20.Text = "Ready to begin"
        TreeNode21.Name = "Progress"
        TreeNode21.Text = "Creation in progress"
        TreeNode22.Name = "Finish"
        TreeNode22.Text = "Finish"
        Me.StepsTreeView.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2, TreeNode7, TreeNode8, TreeNode15, TreeNode19, TreeNode20, TreeNode21, TreeNode22})
        Me.StepsTreeView.ShowLines = False
        Me.StepsTreeView.ShowPlusMinus = False
        Me.StepsTreeView.ShowRootLines = False
        Me.StepsTreeView.Size = New System.Drawing.Size(256, 481)
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
        Me.ExpressPanelContainer.Controls.Add(Me.IncompleteWarningPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.TargetOSSelectionPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.ActivationPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.EndUserLicenseAgreementPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.CompPersonalizationPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.DiskPartPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.RegionalSettingsPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.UserEulaPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.OobeSkipsPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.NetSecurityPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.WirelessPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.CompProtectPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.ControlPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.UsrPersonalizationPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.UsrSecurityPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.UsrCeipPanel)
        Me.ExpressPanelContainer.Controls.Add(Me.SettingRecapPanel)
        Me.ExpressPanelContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExpressPanelContainer.Location = New System.Drawing.Point(256, 104)
        Me.ExpressPanelContainer.Name = "ExpressPanelContainer"
        Me.ExpressPanelContainer.Size = New System.Drawing.Size(752, 417)
        Me.ExpressPanelContainer.TabIndex = 1
        '
        'ActivationPanel
        '
        Me.ActivationPanel.Controls.Add(Me.KeyCopyButton)
        Me.ActivationPanel.Controls.Add(Me.CheckBox5)
        Me.ActivationPanel.Controls.Add(Me.CheckBox4)
        Me.ActivationPanel.Controls.Add(Me.CheckBox3)
        Me.ActivationPanel.Controls.Add(Me.ComboBox2)
        Me.ActivationPanel.Controls.Add(Me.KeyInputBox5)
        Me.ActivationPanel.Controls.Add(Me.GenericBox5)
        Me.ActivationPanel.Controls.Add(Me.KeyInputBox4)
        Me.ActivationPanel.Controls.Add(Me.GenericBox4)
        Me.ActivationPanel.Controls.Add(Me.KeyInputBox3)
        Me.ActivationPanel.Controls.Add(Me.GenericBox3)
        Me.ActivationPanel.Controls.Add(Me.KeyInputBox2)
        Me.ActivationPanel.Controls.Add(Me.GenericBox2)
        Me.ActivationPanel.Controls.Add(Me.KeyInputBox1)
        Me.ActivationPanel.Controls.Add(Me.KeyDash4)
        Me.ActivationPanel.Controls.Add(Me.GenericBox1)
        Me.ActivationPanel.Controls.Add(Me.KeyDash3)
        Me.ActivationPanel.Controls.Add(Me.GenericDash4)
        Me.ActivationPanel.Controls.Add(Me.KeyDash2)
        Me.ActivationPanel.Controls.Add(Me.GenericDash3)
        Me.ActivationPanel.Controls.Add(Me.KeyDash1)
        Me.ActivationPanel.Controls.Add(Me.GenericDash2)
        Me.ActivationPanel.Controls.Add(Me.GenericDash1)
        Me.ActivationPanel.Controls.Add(Me.Label21)
        Me.ActivationPanel.Controls.Add(Me.Label20)
        Me.ActivationPanel.Controls.Add(Me.Label19)
        Me.ActivationPanel.Controls.Add(Me.Label17)
        Me.ActivationPanel.Controls.Add(Me.Label18)
        Me.ActivationPanel.Controls.Add(Me.RadioButton4)
        Me.ActivationPanel.Controls.Add(Me.RadioButton3)
        Me.ActivationPanel.Controls.Add(Me.Label16)
        Me.ActivationPanel.Controls.Add(Me.PictureBox5)
        Me.ActivationPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ActivationPanel.Location = New System.Drawing.Point(0, 0)
        Me.ActivationPanel.Name = "ActivationPanel"
        Me.ActivationPanel.Size = New System.Drawing.Size(752, 417)
        Me.ActivationPanel.TabIndex = 10
        '
        'KeyCopyButton
        '
        Me.KeyCopyButton.Image = Global.DISMTools.My.Resources.Resources.copytoclip
        Me.KeyCopyButton.Location = New System.Drawing.Point(613, 169)
        Me.KeyCopyButton.Name = "KeyCopyButton"
        Me.KeyCopyButton.Size = New System.Drawing.Size(24, 23)
        Me.KeyCopyButton.TabIndex = 12
        Me.KeyCopyButton.UseVisualStyleBackColor = True
        '
        'CheckBox5
        '
        Me.CheckBox5.AutoSize = True
        Me.CheckBox5.Checked = True
        Me.CheckBox5.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox5.Location = New System.Drawing.Point(483, 382)
        Me.CheckBox5.Name = "CheckBox5"
        Me.CheckBox5.Size = New System.Drawing.Size(111, 17)
        Me.CheckBox5.TabIndex = 9
        Me.CheckBox5.Text = "Skip license rearm"
        Me.CheckBox5.UseVisualStyleBackColor = True
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.Checked = True
        Me.CheckBox4.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox4.Location = New System.Drawing.Point(291, 382)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(145, 17)
        Me.CheckBox4.TabIndex = 9
        Me.CheckBox4.Text = "Skip automatic activation"
        Me.CheckBox4.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Checked = True
        Me.CheckBox3.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox3.Location = New System.Drawing.Point(84, 382)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(167, 17)
        Me.CheckBox3.TabIndex = 9
        Me.CheckBox3.Text = "Skip product key input screen"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'ComboBox2
        '
        Me.ComboBox2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(218, 123)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(447, 21)
        Me.ComboBox2.TabIndex = 8
        '
        'KeyInputBox5
        '
        Me.KeyInputBox5.Enabled = False
        Me.KeyInputBox5.Location = New System.Drawing.Point(540, 258)
        Me.KeyInputBox5.MaxLength = 5
        Me.KeyInputBox5.Name = "KeyInputBox5"
        Me.KeyInputBox5.Size = New System.Drawing.Size(66, 21)
        Me.KeyInputBox5.TabIndex = 11
        Me.KeyInputBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GenericBox5
        '
        Me.GenericBox5.Location = New System.Drawing.Point(540, 170)
        Me.GenericBox5.MaxLength = 5
        Me.GenericBox5.Name = "GenericBox5"
        Me.GenericBox5.ReadOnly = True
        Me.GenericBox5.Size = New System.Drawing.Size(66, 21)
        Me.GenericBox5.TabIndex = 7
        Me.GenericBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'KeyInputBox4
        '
        Me.KeyInputBox4.Enabled = False
        Me.KeyInputBox4.Location = New System.Drawing.Point(451, 258)
        Me.KeyInputBox4.MaxLength = 5
        Me.KeyInputBox4.Name = "KeyInputBox4"
        Me.KeyInputBox4.Size = New System.Drawing.Size(66, 21)
        Me.KeyInputBox4.TabIndex = 10
        Me.KeyInputBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GenericBox4
        '
        Me.GenericBox4.Location = New System.Drawing.Point(451, 170)
        Me.GenericBox4.MaxLength = 5
        Me.GenericBox4.Name = "GenericBox4"
        Me.GenericBox4.ReadOnly = True
        Me.GenericBox4.Size = New System.Drawing.Size(66, 21)
        Me.GenericBox4.TabIndex = 7
        Me.GenericBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'KeyInputBox3
        '
        Me.KeyInputBox3.Enabled = False
        Me.KeyInputBox3.Location = New System.Drawing.Point(362, 258)
        Me.KeyInputBox3.MaxLength = 5
        Me.KeyInputBox3.Name = "KeyInputBox3"
        Me.KeyInputBox3.Size = New System.Drawing.Size(66, 21)
        Me.KeyInputBox3.TabIndex = 9
        Me.KeyInputBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GenericBox3
        '
        Me.GenericBox3.Location = New System.Drawing.Point(362, 170)
        Me.GenericBox3.MaxLength = 5
        Me.GenericBox3.Name = "GenericBox3"
        Me.GenericBox3.ReadOnly = True
        Me.GenericBox3.Size = New System.Drawing.Size(66, 21)
        Me.GenericBox3.TabIndex = 7
        Me.GenericBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'KeyInputBox2
        '
        Me.KeyInputBox2.Enabled = False
        Me.KeyInputBox2.Location = New System.Drawing.Point(273, 258)
        Me.KeyInputBox2.MaxLength = 5
        Me.KeyInputBox2.Name = "KeyInputBox2"
        Me.KeyInputBox2.Size = New System.Drawing.Size(66, 21)
        Me.KeyInputBox2.TabIndex = 8
        Me.KeyInputBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GenericBox2
        '
        Me.GenericBox2.Location = New System.Drawing.Point(273, 170)
        Me.GenericBox2.MaxLength = 5
        Me.GenericBox2.Name = "GenericBox2"
        Me.GenericBox2.ReadOnly = True
        Me.GenericBox2.Size = New System.Drawing.Size(66, 21)
        Me.GenericBox2.TabIndex = 7
        Me.GenericBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'KeyInputBox1
        '
        Me.KeyInputBox1.Enabled = False
        Me.KeyInputBox1.Location = New System.Drawing.Point(184, 258)
        Me.KeyInputBox1.MaxLength = 5
        Me.KeyInputBox1.Name = "KeyInputBox1"
        Me.KeyInputBox1.Size = New System.Drawing.Size(66, 21)
        Me.KeyInputBox1.TabIndex = 7
        Me.KeyInputBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'KeyDash4
        '
        Me.KeyDash4.AutoSize = True
        Me.KeyDash4.Enabled = False
        Me.KeyDash4.Location = New System.Drawing.Point(523, 261)
        Me.KeyDash4.Name = "KeyDash4"
        Me.KeyDash4.Size = New System.Drawing.Size(11, 13)
        Me.KeyDash4.TabIndex = 6
        Me.KeyDash4.Text = "-"
        '
        'GenericBox1
        '
        Me.GenericBox1.Location = New System.Drawing.Point(184, 170)
        Me.GenericBox1.MaxLength = 5
        Me.GenericBox1.Name = "GenericBox1"
        Me.GenericBox1.ReadOnly = True
        Me.GenericBox1.Size = New System.Drawing.Size(66, 21)
        Me.GenericBox1.TabIndex = 7
        Me.GenericBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'KeyDash3
        '
        Me.KeyDash3.AutoSize = True
        Me.KeyDash3.Enabled = False
        Me.KeyDash3.Location = New System.Drawing.Point(434, 261)
        Me.KeyDash3.Name = "KeyDash3"
        Me.KeyDash3.Size = New System.Drawing.Size(11, 13)
        Me.KeyDash3.TabIndex = 6
        Me.KeyDash3.Text = "-"
        '
        'GenericDash4
        '
        Me.GenericDash4.AutoSize = True
        Me.GenericDash4.Location = New System.Drawing.Point(523, 173)
        Me.GenericDash4.Name = "GenericDash4"
        Me.GenericDash4.Size = New System.Drawing.Size(11, 13)
        Me.GenericDash4.TabIndex = 6
        Me.GenericDash4.Text = "-"
        '
        'KeyDash2
        '
        Me.KeyDash2.AutoSize = True
        Me.KeyDash2.Enabled = False
        Me.KeyDash2.Location = New System.Drawing.Point(345, 261)
        Me.KeyDash2.Name = "KeyDash2"
        Me.KeyDash2.Size = New System.Drawing.Size(11, 13)
        Me.KeyDash2.TabIndex = 6
        Me.KeyDash2.Text = "-"
        '
        'GenericDash3
        '
        Me.GenericDash3.AutoSize = True
        Me.GenericDash3.Location = New System.Drawing.Point(434, 173)
        Me.GenericDash3.Name = "GenericDash3"
        Me.GenericDash3.Size = New System.Drawing.Size(11, 13)
        Me.GenericDash3.TabIndex = 6
        Me.GenericDash3.Text = "-"
        '
        'KeyDash1
        '
        Me.KeyDash1.AutoSize = True
        Me.KeyDash1.Enabled = False
        Me.KeyDash1.Location = New System.Drawing.Point(256, 261)
        Me.KeyDash1.Name = "KeyDash1"
        Me.KeyDash1.Size = New System.Drawing.Size(11, 13)
        Me.KeyDash1.TabIndex = 6
        Me.KeyDash1.Text = "-"
        '
        'GenericDash2
        '
        Me.GenericDash2.AutoSize = True
        Me.GenericDash2.Location = New System.Drawing.Point(345, 173)
        Me.GenericDash2.Name = "GenericDash2"
        Me.GenericDash2.Size = New System.Drawing.Size(11, 13)
        Me.GenericDash2.TabIndex = 6
        Me.GenericDash2.Text = "-"
        '
        'GenericDash1
        '
        Me.GenericDash1.AutoSize = True
        Me.GenericDash1.Location = New System.Drawing.Point(256, 173)
        Me.GenericDash1.Name = "GenericDash1"
        Me.GenericDash1.Size = New System.Drawing.Size(11, 13)
        Me.GenericDash1.TabIndex = 6
        Me.GenericDash1.Text = "-"
        '
        'Label21
        '
        Me.Label21.AutoEllipsis = True
        Me.Label21.Enabled = False
        Me.Label21.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(102, 333)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(586, 48)
        Me.Label21.TabIndex = 6
        Me.Label21.Text = "IMPORTANT: DISMTools cannot verify whether or not the product key specified is va" & _
    "lid. You can continue, assuming you have checked the validity of the product key" & _
    " beforehand."
        '
        'Label20
        '
        Me.Label20.AutoEllipsis = True
        Me.Label20.Enabled = False
        Me.Label20.Location = New System.Drawing.Point(102, 291)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(586, 31)
        Me.Label20.TabIndex = 6
        Me.Label20.Text = "To move on to the next text field, press the TAB key."
        '
        'Label19
        '
        Me.Label19.AutoEllipsis = True
        Me.Label19.Enabled = False
        Me.Label19.Location = New System.Drawing.Point(102, 220)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(586, 31)
        Me.Label19.TabIndex = 6
        Me.Label19.Text = "Use a product key you have purchased to install/activate the Windows installation" & _
    ". The product key consists of 25 characters, separated into 5 groups."
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(102, 126)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(107, 13)
        Me.Label17.TabIndex = 6
        Me.Label17.Text = "Choose your edition:"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(102, 150)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(139, 13)
        Me.Label18.TabIndex = 6
        Me.Label18.Text = "This is your installation key:"
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(84, 200)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(157, 17)
        Me.RadioButton4.TabIndex = 5
        Me.RadioButton4.Text = "Use a different product key"
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Checked = True
        Me.RadioButton3.Location = New System.Drawing.Point(84, 98)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(242, 17)
        Me.RadioButton3.TabIndex = 5
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "Use a generic installation key (recommended)"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label16.AutoEllipsis = True
        Me.Label16.Location = New System.Drawing.Point(54, 16)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(682, 55)
        Me.Label16.TabIndex = 4
        Me.Label16.Text = resources.GetString("Label16.Text")
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = Global.DISMTools.My.Resources.Resources.activation
        Me.PictureBox5.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox5.TabIndex = 1
        Me.PictureBox5.TabStop = False
        '
        'IncompleteWarningPanel
        '
        Me.IncompleteWarningPanel.Controls.Add(Me.CheckBox1)
        Me.IncompleteWarningPanel.Controls.Add(Me.Label5)
        Me.IncompleteWarningPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.IncompleteWarningPanel.Location = New System.Drawing.Point(0, 0)
        Me.IncompleteWarningPanel.Name = "IncompleteWarningPanel"
        Me.IncompleteWarningPanel.Size = New System.Drawing.Size(752, 417)
        Me.IncompleteWarningPanel.TabIndex = 0
        '
        'CheckBox1
        '
        Me.CheckBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(95, 240)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(222, 17)
        Me.CheckBox1.TabIndex = 1
        Me.CheckBox1.Text = "I have read and understood this warning"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label5.Location = New System.Drawing.Point(92, 121)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(568, 115)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = resources.GetString("Label5.Text")
        '
        'TargetOSSelectionPanel
        '
        Me.TargetOSSelectionPanel.Controls.Add(Me.GroupBox1)
        Me.TargetOSSelectionPanel.Controls.Add(Me.CheckBox2)
        Me.TargetOSSelectionPanel.Controls.Add(Me.ComboBox1)
        Me.TargetOSSelectionPanel.Controls.Add(Me.Label11)
        Me.TargetOSSelectionPanel.Controls.Add(Me.Label10)
        Me.TargetOSSelectionPanel.Controls.Add(Me.Label8)
        Me.TargetOSSelectionPanel.Controls.Add(Me.Label9)
        Me.TargetOSSelectionPanel.Controls.Add(Me.Label7)
        Me.TargetOSSelectionPanel.Controls.Add(Me.RadioButton2)
        Me.TargetOSSelectionPanel.Controls.Add(Me.RadioButton1)
        Me.TargetOSSelectionPanel.Controls.Add(Me.Label6)
        Me.TargetOSSelectionPanel.Controls.Add(Me.PictureBox4)
        Me.TargetOSSelectionPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TargetOSSelectionPanel.Location = New System.Drawing.Point(0, 0)
        Me.TargetOSSelectionPanel.Name = "TargetOSSelectionPanel"
        Me.TargetOSSelectionPanel.Size = New System.Drawing.Size(752, 417)
        Me.TargetOSSelectionPanel.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Location = New System.Drawing.Point(238, 271)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(276, 129)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Target OS details"
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(27, 90)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(222, 23)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "Get more information about this OS"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(96, 52)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(69, 13)
        Me.Label15.TabIndex = 6
        Me.Label15.Text = "<keVersion>"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(108, 32)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(57, 13)
        Me.Label13.TabIndex = 6
        Me.Label13.Text = "<reldate>"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(28, 52)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(62, 13)
        Me.Label14.TabIndex = 6
        Me.Label14.Text = "NT version:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(28, 32)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(74, 13)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "Release date:"
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(272, 248)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(298, 17)
        Me.CheckBox2.TabIndex = 8
        Me.CheckBox2.Text = "I want to make an installation of an older OS unattended"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"Windows 7", "Windows Server 2008 R2", "Windows 8", "Windows Server 2012", "Windows 8.1", "Windows Server 2012 R2", "Windows 10", "Windows 11", "Windows Server 2016", "Windows Server 2019"})
        Me.ComboBox1.Location = New System.Drawing.Point(272, 220)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(343, 21)
        Me.ComboBox1.TabIndex = 7
        Me.ComboBox1.Text = "Windows 7"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(137, 224)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(129, 13)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Target operating system:"
        '
        'Label10
        '
        Me.Label10.AutoEllipsis = True
        Me.Label10.Location = New System.Drawing.Point(357, 166)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(281, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "<TargetOS>"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(115, 166)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(236, 13)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "Recommended target OS:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label9
        '
        Me.Label9.AutoEllipsis = True
        Me.Label9.Location = New System.Drawing.Point(357, 146)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(281, 13)
        Me.Label9.TabIndex = 5
        Me.Label9.Text = "<imgversion>"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(115, 146)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(236, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Mounted image version:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(84, 190)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(184, 17)
        Me.RadioButton2.TabIndex = 4
        Me.RadioButton2.Text = "Let me choose the target version"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(84, 118)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(374, 17)
        Me.RadioButton1.TabIndex = 4
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Let the program detect the version automatically for me (Recommended)"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoEllipsis = True
        Me.Label6.Location = New System.Drawing.Point(54, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(682, 94)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = resources.GetString("Label6.Text")
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.DISMTools.My.Resources.Resources.targetos
        Me.PictureBox4.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox4.TabIndex = 0
        Me.PictureBox4.TabStop = False
        '
        'EndUserLicenseAgreementPanel
        '
        Me.EndUserLicenseAgreementPanel.Controls.Add(Me.Label24)
        Me.EndUserLicenseAgreementPanel.Controls.Add(Me.Label23)
        Me.EndUserLicenseAgreementPanel.Controls.Add(Me.CheckBox6)
        Me.EndUserLicenseAgreementPanel.Controls.Add(Me.Label22)
        Me.EndUserLicenseAgreementPanel.Controls.Add(Me.PictureBox6)
        Me.EndUserLicenseAgreementPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EndUserLicenseAgreementPanel.Location = New System.Drawing.Point(0, 0)
        Me.EndUserLicenseAgreementPanel.Name = "EndUserLicenseAgreementPanel"
        Me.EndUserLicenseAgreementPanel.Size = New System.Drawing.Size(752, 417)
        Me.EndUserLicenseAgreementPanel.TabIndex = 11
        '
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(54, 323)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(682, 67)
        Me.Label24.TabIndex = 6
        Me.Label24.Text = "On setup, and if checked, the End-User License Agreement will automatically be ac" & _
    "cepted. However, if not set up later, you may need to agree to it on the Out-of-" & _
    "box Experience (OOBE)"
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(93, 140)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(595, 67)
        Me.Label23.TabIndex = 6
        Me.Label23.Text = "You can check this option to skip this question; and make the system take less ti" & _
    "me to install using these settings." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'CheckBox6
        '
        Me.CheckBox6.AutoSize = True
        Me.CheckBox6.Location = New System.Drawing.Point(77, 120)
        Me.CheckBox6.Name = "CheckBox6"
        Me.CheckBox6.Size = New System.Drawing.Size(189, 17)
        Me.CheckBox6.TabIndex = 5
        Me.CheckBox6.Text = "Skip the license agreement screen"
        Me.CheckBox6.UseVisualStyleBackColor = True
        '
        'Label22
        '
        Me.Label22.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label22.AutoEllipsis = True
        Me.Label22.Location = New System.Drawing.Point(54, 16)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(682, 55)
        Me.Label22.TabIndex = 4
        Me.Label22.Text = resources.GetString("Label22.Text")
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = Global.DISMTools.My.Resources.Resources.eula
        Me.PictureBox6.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox6.TabIndex = 1
        Me.PictureBox6.TabStop = False
        '
        'CompPersonalizationPanel
        '
        Me.CompPersonalizationPanel.Controls.Add(Me.ComboBox3)
        Me.CompPersonalizationPanel.Controls.Add(Me.Label30)
        Me.CompPersonalizationPanel.Controls.Add(Me.TextBox2)
        Me.CompPersonalizationPanel.Controls.Add(Me.Label29)
        Me.CompPersonalizationPanel.Controls.Add(Me.TextBox1)
        Me.CompPersonalizationPanel.Controls.Add(Me.Label28)
        Me.CompPersonalizationPanel.Controls.Add(Me.Label26)
        Me.CompPersonalizationPanel.Controls.Add(Me.Label27)
        Me.CompPersonalizationPanel.Controls.Add(Me.Label25)
        Me.CompPersonalizationPanel.Controls.Add(Me.PictureBox7)
        Me.CompPersonalizationPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CompPersonalizationPanel.Location = New System.Drawing.Point(0, 0)
        Me.CompPersonalizationPanel.Name = "CompPersonalizationPanel"
        Me.CompPersonalizationPanel.Size = New System.Drawing.Size(752, 417)
        Me.CompPersonalizationPanel.TabIndex = 12
        '
        'ComboBox3
        '
        Me.ComboBox3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Items.AddRange(New Object() {"English", "French", "German", "Spanish", "Dutch", "Italian", "Russian", "Japanese", "Arabic", "Czech", "Danish", "Greek", "Swedish", "Portuguese (Brazil)", "Portuguese (Portugal)"})
        Me.ComboBox3.Location = New System.Drawing.Point(367, 271)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(201, 21)
        Me.ComboBox3.TabIndex = 6
        Me.ComboBox3.Text = "English"
        '
        'Label30
        '
        Me.Label30.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label30.AutoEllipsis = True
        Me.Label30.Location = New System.Drawing.Point(185, 274)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(176, 13)
        Me.Label30.TabIndex = 4
        Me.Label30.Text = "Windows Setup language:"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(367, 191)
        Me.TextBox2.MaxLength = 63
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(201, 21)
        Me.TextBox2.TabIndex = 5
        '
        'Label29
        '
        Me.Label29.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label29.AutoEllipsis = True
        Me.Label29.Location = New System.Drawing.Point(185, 194)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(176, 13)
        Me.Label29.TabIndex = 4
        Me.Label29.Text = "Computer organization name:"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(367, 125)
        Me.TextBox1.MaxLength = 15
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(201, 21)
        Me.TextBox1.TabIndex = 5
        '
        'Label28
        '
        Me.Label28.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label28.AutoEllipsis = True
        Me.Label28.Location = New System.Drawing.Point(364, 220)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(204, 44)
        Me.Label28.TabIndex = 4
        Me.Label28.Text = "Assigning an organization name makes the computer apply settings and group polici" & _
    "es from the organization"
        '
        'Label26
        '
        Me.Label26.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label26.AutoEllipsis = True
        Me.Label26.Location = New System.Drawing.Point(185, 128)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(176, 13)
        Me.Label26.TabIndex = 4
        Me.Label26.Text = "Computer name:"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label27
        '
        Me.Label27.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label27.AutoEllipsis = True
        Me.Label27.Location = New System.Drawing.Point(364, 154)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(204, 33)
        Me.Label27.TabIndex = 4
        Me.Label27.Text = "Assigning a name makes the computer visible on the network."
        '
        'Label25
        '
        Me.Label25.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label25.AutoEllipsis = True
        Me.Label25.Location = New System.Drawing.Point(54, 16)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(682, 55)
        Me.Label25.TabIndex = 4
        Me.Label25.Text = resources.GetString("Label25.Text")
        '
        'PictureBox7
        '
        Me.PictureBox7.Image = Global.DISMTools.My.Resources.Resources.comp_personalization
        Me.PictureBox7.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox7.TabIndex = 1
        Me.PictureBox7.TabStop = False
        '
        'DiskPartPanel
        '
        Me.DiskPartPanel.Controls.Add(Me.PartStylePanel)
        Me.DiskPartPanel.Controls.Add(Me.TextBox3)
        Me.DiskPartPanel.Controls.Add(Me.ComboBox4)
        Me.DiskPartPanel.Controls.Add(Me.RadioButton6)
        Me.DiskPartPanel.Controls.Add(Me.RadioButton5)
        Me.DiskPartPanel.Controls.Add(Me.NumericUpDown2)
        Me.DiskPartPanel.Controls.Add(Me.NumericUpDown1)
        Me.DiskPartPanel.Controls.Add(Me.Label38)
        Me.DiskPartPanel.Controls.Add(Me.Label37)
        Me.DiskPartPanel.Controls.Add(Me.Label36)
        Me.DiskPartPanel.Controls.Add(Me.Label34)
        Me.DiskPartPanel.Controls.Add(Me.Label56)
        Me.DiskPartPanel.Controls.Add(Me.Label40)
        Me.DiskPartPanel.Controls.Add(Me.Label39)
        Me.DiskPartPanel.Controls.Add(Me.Label57)
        Me.DiskPartPanel.Controls.Add(Me.Label35)
        Me.DiskPartPanel.Controls.Add(Me.Label33)
        Me.DiskPartPanel.Controls.Add(Me.Label32)
        Me.DiskPartPanel.Controls.Add(Me.CheckBox8)
        Me.DiskPartPanel.Controls.Add(Me.CheckBox7)
        Me.DiskPartPanel.Controls.Add(Me.Label31)
        Me.DiskPartPanel.Controls.Add(Me.PictureBox8)
        Me.DiskPartPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DiskPartPanel.Location = New System.Drawing.Point(0, 0)
        Me.DiskPartPanel.Name = "DiskPartPanel"
        Me.DiskPartPanel.Size = New System.Drawing.Size(752, 417)
        Me.DiskPartPanel.TabIndex = 13
        '
        'PartStylePanel
        '
        Me.PartStylePanel.Controls.Add(Me.Label59)
        Me.PartStylePanel.Controls.Add(Me.Label58)
        Me.PartStylePanel.Controls.Add(Me.RadioButton7)
        Me.PartStylePanel.Controls.Add(Me.RadioButton8)
        Me.PartStylePanel.Location = New System.Drawing.Point(498, 222)
        Me.PartStylePanel.Name = "PartStylePanel"
        Me.PartStylePanel.Size = New System.Drawing.Size(238, 94)
        Me.PartStylePanel.TabIndex = 14
        '
        'Label59
        '
        Me.Label59.AutoSize = True
        Me.Label59.Location = New System.Drawing.Point(27, 77)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(194, 13)
        Me.Label59.TabIndex = 9
        Me.Label59.Text = "Choose this option for modern systems"
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(27, 35)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(189, 13)
        Me.Label58.TabIndex = 9
        Me.Label58.Text = "Choose this option for legacy systems"
        '
        'RadioButton7
        '
        Me.RadioButton7.AutoSize = True
        Me.RadioButton7.Checked = True
        Me.RadioButton7.Location = New System.Drawing.Point(12, 12)
        Me.RadioButton7.Name = "RadioButton7"
        Me.RadioButton7.Size = New System.Drawing.Size(46, 17)
        Me.RadioButton7.TabIndex = 11
        Me.RadioButton7.TabStop = True
        Me.RadioButton7.Text = "MBR"
        Me.RadioButton7.UseVisualStyleBackColor = True
        '
        'RadioButton8
        '
        Me.RadioButton8.AutoSize = True
        Me.RadioButton8.Location = New System.Drawing.Point(12, 55)
        Me.RadioButton8.Name = "RadioButton8"
        Me.RadioButton8.Size = New System.Drawing.Size(48, 17)
        Me.RadioButton8.TabIndex = 11
        Me.RadioButton8.Text = "UEFI"
        Me.RadioButton8.UseVisualStyleBackColor = True
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(596, 170)
        Me.TextBox3.MaxLength = 32
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(128, 21)
        Me.TextBox3.TabIndex = 13
        '
        'ComboBox4
        '
        Me.ComboBox4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox4.FormattingEnabled = True
        Me.ComboBox4.Items.AddRange(New Object() {"C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"})
        Me.ComboBox4.Location = New System.Drawing.Point(603, 143)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox4.TabIndex = 12
        Me.ComboBox4.Text = "C"
        '
        'RadioButton6
        '
        Me.RadioButton6.AutoSize = True
        Me.RadioButton6.Location = New System.Drawing.Point(97, 277)
        Me.RadioButton6.Name = "RadioButton6"
        Me.RadioButton6.Size = New System.Drawing.Size(56, 17)
        Me.RadioButton6.TabIndex = 11
        Me.RadioButton6.Text = "FAT32"
        Me.RadioButton6.UseVisualStyleBackColor = True
        '
        'RadioButton5
        '
        Me.RadioButton5.AutoSize = True
        Me.RadioButton5.Checked = True
        Me.RadioButton5.Location = New System.Drawing.Point(97, 234)
        Me.RadioButton5.Name = "RadioButton5"
        Me.RadioButton5.Size = New System.Drawing.Size(131, 17)
        Me.RadioButton5.TabIndex = 11
        Me.RadioButton5.TabStop = True
        Me.RadioButton5.Text = "NTFS (Recommended)"
        Me.RadioButton5.UseVisualStyleBackColor = True
        '
        'NumericUpDown2
        '
        Me.NumericUpDown2.Location = New System.Drawing.Point(581, 321)
        Me.NumericUpDown2.Maximum = New Decimal(New Integer() {7, 0, 0, 0})
        Me.NumericUpDown2.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.NumericUpDown2.Name = "NumericUpDown2"
        Me.NumericUpDown2.Size = New System.Drawing.Size(143, 21)
        Me.NumericUpDown2.TabIndex = 10
        Me.NumericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NumericUpDown2.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(323, 144)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(64, 21)
        Me.NumericUpDown1.TabIndex = 10
        Me.NumericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label38
        '
        Me.Label38.AutoEllipsis = True
        Me.Label38.Location = New System.Drawing.Point(93, 374)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(385, 27)
        Me.Label38.TabIndex = 9
        Me.Label38.Text = "Make the target partition bootable. Do note that, if the target system has multip" & _
    "le operating systems installed, this may make them unbootable."
        '
        'Label37
        '
        Me.Label37.AutoEllipsis = True
        Me.Label37.Location = New System.Drawing.Point(113, 299)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(365, 40)
        Me.Label37.TabIndex = 9
        Me.Label37.Text = "This was the default file system used by old versions of Windows. This is not rec" & _
    "ommended for newer versions of Windows, as the target installation will not work" & _
    " correctly"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(113, 257)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(333, 13)
        Me.Label36.TabIndex = 9
        Me.Label36.Text = "This is the default file system chosen by Setup to install Windows on"
        '
        'Label34
        '
        Me.Label34.AutoEllipsis = True
        Me.Label34.Location = New System.Drawing.Point(94, 172)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(364, 34)
        Me.Label34.TabIndex = 9
        Me.Label34.Text = "NOTE: Disk 0 refers to the first disk installed on the target system; as counting" & _
    " begins at 0 for disks"
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(495, 323)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(80, 13)
        Me.Label56.TabIndex = 9
        Me.Label56.Text = "Partition order:"
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(495, 173)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(95, 13)
        Me.Label40.TabIndex = 9
        Me.Label40.Text = "Target drive label:"
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(495, 146)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(99, 13)
        Me.Label39.TabIndex = 9
        Me.Label39.Text = "Target drive letter:"
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(495, 206)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(77, 13)
        Me.Label57.TabIndex = 9
        Me.Label57.Text = "Partition style:"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(74, 206)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(268, 13)
        Me.Label35.TabIndex = 9
        Me.Label35.Text = "Which file system should the target OS partition have?"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(75, 146)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(242, 13)
        Me.Label33.TabIndex = 9
        Me.Label33.Text = "The operating system should be installed on disk:"
        '
        'Label32
        '
        Me.Label32.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label32.Location = New System.Drawing.Point(94, 104)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(584, 36)
        Me.Label32.TabIndex = 8
        Me.Label32.Text = "CAUTION: The same principle applies here. If you have sensitive data stored on th" & _
    "e destination disk, back it up before proceeding."
        '
        'CheckBox8
        '
        Me.CheckBox8.AutoSize = True
        Me.CheckBox8.Checked = True
        Me.CheckBox8.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox8.Location = New System.Drawing.Point(77, 354)
        Me.CheckBox8.Name = "CheckBox8"
        Me.CheckBox8.Size = New System.Drawing.Size(138, 17)
        Me.CheckBox8.TabIndex = 7
        Me.CheckBox8.Text = "Mark partition as active"
        Me.CheckBox8.UseVisualStyleBackColor = True
        '
        'CheckBox7
        '
        Me.CheckBox7.AutoSize = True
        Me.CheckBox7.Checked = True
        Me.CheckBox7.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox7.Location = New System.Drawing.Point(77, 84)
        Me.CheckBox7.Name = "CheckBox7"
        Me.CheckBox7.Size = New System.Drawing.Size(104, 17)
        Me.CheckBox7.TabIndex = 7
        Me.CheckBox7.Text = "Wipe target disk"
        Me.CheckBox7.UseVisualStyleBackColor = True
        '
        'Label31
        '
        Me.Label31.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label31.AutoEllipsis = True
        Me.Label31.Location = New System.Drawing.Point(54, 16)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(682, 55)
        Me.Label31.TabIndex = 6
        Me.Label31.Text = resources.GetString("Label31.Text")
        '
        'PictureBox8
        '
        Me.PictureBox8.Image = Global.DISMTools.My.Resources.Resources.partitioning
        Me.PictureBox8.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox8.Name = "PictureBox8"
        Me.PictureBox8.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox8.TabIndex = 5
        Me.PictureBox8.TabStop = False
        '
        'RegionalSettingsPanel
        '
        Me.RegionalSettingsPanel.Controls.Add(Me.GroupBox2)
        Me.RegionalSettingsPanel.Controls.Add(Me.ComboBox8)
        Me.RegionalSettingsPanel.Controls.Add(Me.ComboBox6)
        Me.RegionalSettingsPanel.Controls.Add(Me.Label45)
        Me.RegionalSettingsPanel.Controls.Add(Me.Label43)
        Me.RegionalSettingsPanel.Controls.Add(Me.ComboBox7)
        Me.RegionalSettingsPanel.Controls.Add(Me.Label44)
        Me.RegionalSettingsPanel.Controls.Add(Me.ComboBox5)
        Me.RegionalSettingsPanel.Controls.Add(Me.Label42)
        Me.RegionalSettingsPanel.Controls.Add(Me.Label41)
        Me.RegionalSettingsPanel.Controls.Add(Me.PictureBox9)
        Me.RegionalSettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RegionalSettingsPanel.Location = New System.Drawing.Point(0, 0)
        Me.RegionalSettingsPanel.Name = "RegionalSettingsPanel"
        Me.RegionalSettingsPanel.Size = New System.Drawing.Size(752, 417)
        Me.RegionalSettingsPanel.TabIndex = 14
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label46)
        Me.GroupBox2.Controls.Add(Me.Label54)
        Me.GroupBox2.Controls.Add(Me.Label53)
        Me.GroupBox2.Controls.Add(Me.Label47)
        Me.GroupBox2.Controls.Add(Me.Label52)
        Me.GroupBox2.Controls.Add(Me.Label48)
        Me.GroupBox2.Controls.Add(Me.Label51)
        Me.GroupBox2.Controls.Add(Me.Label50)
        Me.GroupBox2.Controls.Add(Me.Label49)
        Me.GroupBox2.Location = New System.Drawing.Point(84, 200)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(582, 180)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "What will be configured"
        '
        'Label46
        '
        Me.Label46.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label46.AutoEllipsis = True
        Me.Label46.Location = New System.Drawing.Point(29, 32)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(525, 33)
        Me.Label46.TabIndex = 6
        Me.Label46.Text = "The unattended answer file will contain the following regional information (not a" & _
    "vailable at this time):"
        '
        'Label54
        '
        Me.Label54.AutoEllipsis = True
        Me.Label54.Location = New System.Drawing.Point(253, 72)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(258, 13)
        Me.Label54.TabIndex = 7
        Me.Label54.Text = "<Keyboard>"
        '
        'Label53
        '
        Me.Label53.AutoEllipsis = True
        Me.Label53.Location = New System.Drawing.Point(253, 126)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(258, 13)
        Me.Label53.TabIndex = 7
        Me.Label53.Text = "<TimeZone>"
        '
        'Label47
        '
        Me.Label47.AutoEllipsis = True
        Me.Label47.Location = New System.Drawing.Point(71, 72)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(176, 13)
        Me.Label47.TabIndex = 7
        Me.Label47.Text = "Keyboard or input method:"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label52
        '
        Me.Label52.AutoEllipsis = True
        Me.Label52.Location = New System.Drawing.Point(253, 153)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(258, 13)
        Me.Label52.TabIndex = 7
        Me.Label52.Text = "<UILanguage>"
        '
        'Label48
        '
        Me.Label48.AutoEllipsis = True
        Me.Label48.Location = New System.Drawing.Point(71, 126)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(176, 13)
        Me.Label48.TabIndex = 7
        Me.Label48.Text = "Time zone:"
        Me.Label48.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label51
        '
        Me.Label51.AutoEllipsis = True
        Me.Label51.Location = New System.Drawing.Point(253, 99)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(258, 13)
        Me.Label51.TabIndex = 7
        Me.Label51.Text = "<CurrencyDate>"
        '
        'Label50
        '
        Me.Label50.AutoEllipsis = True
        Me.Label50.Location = New System.Drawing.Point(71, 153)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(176, 13)
        Me.Label50.TabIndex = 7
        Me.Label50.Text = "User Interface (UI) language:"
        Me.Label50.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label49
        '
        Me.Label49.AutoEllipsis = True
        Me.Label49.Location = New System.Drawing.Point(71, 99)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(176, 13)
        Me.Label49.TabIndex = 7
        Me.Label49.Text = "Currency and date format:"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ComboBox8
        '
        Me.ComboBox8.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox8.FormattingEnabled = True
        Me.ComboBox8.Items.AddRange(New Object() {"Afrikaans - South Africa", "Albanian - Albania", "Alsatian - France", "Amharic - Ethiopia", "Arabic - Algeria", "Arabic - Bahrain", "Arabic - Egypt", "Arabic - Iraq", "Arabic - Jordan", "Arabic - Kuwait", "Arabic - Lebanon", "Arabic - Libya", "Arabic - Morocco", "Arabic - Oman", "Arabic - Qatar", "Arabic - Saudi Arabia", "Arabic - Syria", "Arabic - Tunisia", "Arabic - U.A.E.", "Arabic - Yemen", "Armenian - Armenia", "Assamese - India", "Azerbaijani - Azerbaijan (Cyrillic)", "Azerbaijani - Azerbaijan (Latin)", "Bangla - Bangladesh", "Bangla - India (Bengali Script)", "Bashkir - Russia", "Basque - Basque", "Belarusian - Belarus", "Bosnian - Bosnia and Herzegovina (Cyrillic)", "Bosnian - Bosnia and Herzegovina (Latin)", "Breton - France", "Bulgarian - Bulgaria", "Burmese - Myanmar", "Catalan - Catalan", "Central Atlas Tamazight (Latin) - Algeria", "Central Atlas Tamazight (Latin) - Algeria", "Central Atlas Tamazight (Tifinagh) - Morocco", "Central Kurdish (Iraq)", "Cherokee (Cherokee, United States)", "Chinese - Hong Kong", "Chinese - Macao", "Chinese - PRC", "Chinese - Singapore", "Corsican - France", "Croatian - Bosnia and Herzegovina", "Croatian - Croatia", "Czech - Czech Republic", "Danish - Denmark", "Dari - Afghanistan", "Divehi - Maldives", "Dutch - Belgium", "Dutch - Netherlands", "English - Australia", "English - Belize", "English - Canada", "English - Caribbean", "English - India", "English - Ireland", "English - Jamaica", "English - Malaysia", "English - New Zealand", "English - Philippines", "English - Singapore", "English - South Africa", "English - Trinidad", "English - United Kingdom", "English - United States", "English - Zimbabwe", "Estonian - Estonia", "Faroese - Faroe Islands", "Filipino - Philippines", "Finnish - Finland", "French - Belgium", "French - Canada", "French - France", "French - Luxembourg", "French - Monaco", "French - Switzerland", "Frisian - Netherlands", "Fulah (Latin, Senegal)", "Galician - Galician", "Georgian - Georgia", "German - Austria", "German - Germany", "German - Liechtenstein", "German - Luxembourg", "German - Switzerland", "Greek - Greece", "Greenlandic - Greenland", "Guarani - Paraguay", "Gujarati - India (Gujarati Script)", "Hausa (Latin) - Nigeria", "Hawaiian - United States", "Hebrew - Israel", "Hindi - India", "Hungarian - Hungary", "Icelandic - Iceland", "Igbo - Nigeria", "Inari Sami - Finland", "Indonesian - Indonesia", "Inuktitut (Latin) - Canada", "Inuktitut (Syllabics) - Canada", "Irish - Ireland", "isiXhosa / Xhosa - South Africa", "isiZulu / Zulu - South Africa", "Italian - Italy", "Italian - Switzerland", "Japanese - Japan", "Javanese (Latin) - Indonesia", "Kannada - India (Kannada Script)", "Kazakh - Kazakhstan", "Khmer - Cambodia", "K'iche - Guatemala", "Kinyarwanda - Rwanda", "Konkani - India", "Korean(Extended Wansung) - Korea", "Kyrgyz - Kyrgyzstan", "Lao - Lao PDR", "Latvian - Legacy", "Latvian - Standard", "Lithuanian - Lithuania", "Lower Sorbian - Germany", "Lule Sami - Norway", "Lule Sami - Sweden", "Luxembourgish - Luxembourg", "Macedonian - F.Y.R.O.M", "Malay - Brunei", "Malay - Malaysia", "Malayalam - India (Malayalam Script)", "Maltese - Malta", "Maori - New Zealand", "Mapudungun - Chile", "Marathi - India", "Mohawk - Mohawk", "Mongolian (Cyrillic) - Mongolia", "Mongolian (Mongolian) - Mongolia", "Mongolian (Mongolian - PRC - Legacy)", "Mongolian (Mongolian - PRC - Standard)", "N'ko - Guinea", "Nepali - Nepal", "Northern Sami - Finland", "Northern Sami - Norway", "Northern Sami - Sweden", "Norwegian - Norway (Bokmal)", "Norwegian - Norway (Nynorsk)", "Occitan - France", "Odia - India (Odia Script)", "Pashto - Afghanistan", "Persian", "Polish - Poland", "Portuguese - Brazil", "Portuguese - Portugal", "Punjabi - India (Gurmukhi Script)", "Punjabi (Islamic Republic of Pakistan)", "Quechua - Bolivia", "Quechua - Ecuador", "Quechua - Peru", "Romanian - Romania", "Romansh - Switzerland", "Russian - Russia", "Sakha - Russia", "Sanskrit - India", "Scottish Gaelic - United Kingdom", "Serbian - Bosnia and Herzegovina (Cyrillic)", "Serbian - Bosnia and Herzegovina (Latin)", "Serbian - Montenegro (Cyrillic)", "Serbian - Montenegro (Latin)", "Serbian - Serbia (Cyrillic)", "Serbian - Serbia (Latin)", "Serbian - Serbia and Montenegro (Former) (Cyrillic)", "Serbian - Serbia and Montenegro (Former) (Latin)", "Sesotho sa Leboa / Northern Sotho - South Africa", "Setswana / Tswana - Botswana", "Setswana / Tswana - South Africa", "Shona - Zimbabwe", "Sindhi (Islamic Republic of Pakistan)", "Sinhala - Sri Lanka", "Skolt Sami - Finland", "Slovak - Slovakia", "Slovenian - Slovenia", "Southern Sami - Norway", "Southern Sami - Sweden", "Spanish - Argentina", "Spanish - Bolivarian Republic of Venezuela", "Spanish - Bolivia", "Spanish - Chile", "Spanish - Colombia", "Spanish - Costa Rica", "Spanish - Dominican Republic", "Spanish - Ecuador", "Spanish - El Salvador", "Spanish - Guatemala", "Spanish - Honduras", "Spanish - Mexico", "Spanish - Nicaragua", "Spanish - Panama", "Spanish - Paraguay", "Spanish - Peru", "Spanish - Puerto Rico", "Spanish - Spain (International Sort)", "Spanish - Spain (Traditional Sort)", "Spanish - United States", "Spanish - Uruguay", "Standard Moroccan Tamazight - Morocco", "Swahili - Kenya", "Swedish - Finland", "Swedish - Sweden", "Syriac - Syria", "Tajik - Tajikistan", "Tamil - India", "Tamil - Sri Lanka", "Tatar - Russia (Legacy)", "Tatar - Russia (Standard)", "Telugu - India (Telugu Script)", "Thai - Thailand", "Tibetan - PRC", "Tigrinya (Eritrea)", "Tigrinya (Ethiopia)", "Turkish - Turkey", "Turkmen - Turkmenistan", "Ukrainian - Ukraine", "Upper Sorbian - Germany", "Urdu - India", "Urdu (Islamic Republic of Pakistan)", "Uyghur - PRC", "Uzbek - Uzbekistan (Cyrillic)", "Uzbek - Uzbekistan (Latin)", "Valencian - Valencia", "Vietnamese - Vietnam", "Welsh - United Kingdom", "Wolof - Senegal", "Yi - PRC", "Yoruba - Nigeria"})
        Me.ComboBox8.Location = New System.Drawing.Point(270, 161)
        Me.ComboBox8.Name = "ComboBox8"
        Me.ComboBox8.Size = New System.Drawing.Size(396, 21)
        Me.ComboBox8.TabIndex = 8
        Me.ComboBox8.Text = "(please choose)"
        '
        'ComboBox6
        '
        Me.ComboBox6.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox6.FormattingEnabled = True
        Me.ComboBox6.Items.AddRange(New Object() {"Afrikaans - South Africa", "Albanian - Albania", "Alsatian - France", "Amharic - Ethiopia", "Arabic - Algeria", "Arabic - Bahrain", "Arabic - Egypt", "Arabic - Iraq", "Arabic - Jordan", "Arabic - Kuwait", "Arabic - Lebanon", "Arabic - Libya", "Arabic - Morocco", "Arabic - Oman", "Arabic - Qatar", "Arabic - Saudi Arabia", "Arabic - Syria", "Arabic - Tunisia", "Arabic - U.A.E.", "Arabic - Yemen", "Armenian - Armenia", "Assamese - India", "Azerbaijani - Azerbaijan (Cyrillic)", "Azerbaijani - Azerbaijan (Latin)", "Bangla - Bangladesh", "Bangla - India (Bengali Script)", "Bashkir - Russia", "Basque - Basque", "Belarusian - Belarus", "Bosnian - Bosnia and Herzegovina (Cyrillic)", "Bosnian - Bosnia and Herzegovina (Latin)", "Breton - France", "Bulgarian - Bulgaria", "Burmese - Myanmar", "Catalan - Catalan", "Central Atlas Tamazight (Latin) - Algeria", "Central Atlas Tamazight (Latin) - Algeria", "Central Atlas Tamazight (Tifinagh) - Morocco", "Central Kurdish (Iraq)", "Cherokee (Cherokee, United States)", "Chinese - Hong Kong", "Chinese - Macao", "Chinese - PRC", "Chinese - Singapore", "Corsican - France", "Croatian - Bosnia and Herzegovina", "Croatian - Croatia", "Czech - Czech Republic", "Danish - Denmark", "Dari - Afghanistan", "Divehi - Maldives", "Dutch - Belgium", "Dutch - Netherlands", "English - Australia", "English - Belize", "English - Canada", "English - Caribbean", "English - India", "English - Ireland", "English - Jamaica", "English - Malaysia", "English - New Zealand", "English - Philippines", "English - Singapore", "English - South Africa", "English - Trinidad", "English - United Kingdom", "English - United States", "English - Zimbabwe", "Estonian - Estonia", "Faroese - Faroe Islands", "Filipino - Philippines", "Finnish - Finland", "French - Belgium", "French - Canada", "French - France", "French - Luxembourg", "French - Monaco", "French - Switzerland", "Frisian - Netherlands", "Fulah (Latin, Senegal)", "Galician - Galician", "Georgian - Georgia", "German - Austria", "German - Germany", "German - Liechtenstein", "German - Luxembourg", "German - Switzerland", "Greek - Greece", "Greenlandic - Greenland", "Guarani - Paraguay", "Gujarati - India (Gujarati Script)", "Hausa (Latin) - Nigeria", "Hawaiian - United States", "Hebrew - Israel", "Hindi - India", "Hungarian - Hungary", "Icelandic - Iceland", "Igbo - Nigeria", "Inari Sami - Finland", "Indonesian - Indonesia", "Inuktitut (Latin) - Canada", "Inuktitut (Syllabics) - Canada", "Irish - Ireland", "isiXhosa / Xhosa - South Africa", "isiZulu / Zulu - South Africa", "Italian - Italy", "Italian - Switzerland", "Japanese - Japan", "Javanese (Latin) - Indonesia", "Kannada - India (Kannada Script)", "Kazakh - Kazakhstan", "Khmer - Cambodia", "K'iche - Guatemala", "Kinyarwanda - Rwanda", "Konkani - India", "Korean(Extended Wansung) - Korea", "Kyrgyz - Kyrgyzstan", "Lao - Lao PDR", "Latvian - Legacy", "Latvian - Standard", "Lithuanian - Lithuania", "Lower Sorbian - Germany", "Lule Sami - Norway", "Lule Sami - Sweden", "Luxembourgish - Luxembourg", "Macedonian - F.Y.R.O.M", "Malay - Brunei", "Malay - Malaysia", "Malayalam - India (Malayalam Script)", "Maltese - Malta", "Maori - New Zealand", "Mapudungun - Chile", "Marathi - India", "Mohawk - Mohawk", "Mongolian (Cyrillic) - Mongolia", "Mongolian (Mongolian) - Mongolia", "Mongolian (Mongolian - PRC - Legacy)", "Mongolian (Mongolian - PRC - Standard)", "N'ko - Guinea", "Nepali - Nepal", "Northern Sami - Finland", "Northern Sami - Norway", "Northern Sami - Sweden", "Norwegian - Norway (Bokmal)", "Norwegian - Norway (Nynorsk)", "Occitan - France", "Odia - India (Odia Script)", "Pashto - Afghanistan", "Persian", "Polish - Poland", "Portuguese - Brazil", "Portuguese - Portugal", "Punjabi - India (Gurmukhi Script)", "Punjabi (Islamic Republic of Pakistan)", "Quechua - Bolivia", "Quechua - Ecuador", "Quechua - Peru", "Romanian - Romania", "Romansh - Switzerland", "Russian - Russia", "Sakha - Russia", "Sanskrit - India", "Scottish Gaelic - United Kingdom", "Serbian - Bosnia and Herzegovina (Cyrillic)", "Serbian - Bosnia and Herzegovina (Latin)", "Serbian - Montenegro (Cyrillic)", "Serbian - Montenegro (Latin)", "Serbian - Serbia (Cyrillic)", "Serbian - Serbia (Latin)", "Serbian - Serbia and Montenegro (Former) (Cyrillic)", "Serbian - Serbia and Montenegro (Former) (Latin)", "Sesotho sa Leboa / Northern Sotho - South Africa", "Setswana / Tswana - Botswana", "Setswana / Tswana - South Africa", "Shona - Zimbabwe", "Sindhi (Islamic Republic of Pakistan)", "Sinhala - Sri Lanka", "Skolt Sami - Finland", "Slovak - Slovakia", "Slovenian - Slovenia", "Southern Sami - Norway", "Southern Sami - Sweden", "Spanish - Argentina", "Spanish - Bolivarian Republic of Venezuela", "Spanish - Bolivia", "Spanish - Chile", "Spanish - Colombia", "Spanish - Costa Rica", "Spanish - Dominican Republic", "Spanish - Ecuador", "Spanish - El Salvador", "Spanish - Guatemala", "Spanish - Honduras", "Spanish - Mexico", "Spanish - Nicaragua", "Spanish - Panama", "Spanish - Paraguay", "Spanish - Peru", "Spanish - Puerto Rico", "Spanish - Spain (International Sort)", "Spanish - Spain (Traditional Sort)", "Spanish - United States", "Spanish - Uruguay", "Standard Moroccan Tamazight - Morocco", "Swahili - Kenya", "Swedish - Finland", "Swedish - Sweden", "Syriac - Syria", "Tajik - Tajikistan", "Tamil - India", "Tamil - Sri Lanka", "Tatar - Russia (Legacy)", "Tatar - Russia (Standard)", "Telugu - India (Telugu Script)", "Thai - Thailand", "Tibetan - PRC", "Tigrinya (Eritrea)", "Tigrinya (Ethiopia)", "Turkish - Turkey", "Turkmen - Turkmenistan", "Ukrainian - Ukraine", "Upper Sorbian - Germany", "Urdu - India", "Urdu (Islamic Republic of Pakistan)", "Uyghur - PRC", "Uzbek - Uzbekistan (Cyrillic)", "Uzbek - Uzbekistan (Latin)", "Valencian - Valencia", "Vietnamese - Vietnam", "Welsh - United Kingdom", "Wolof - Senegal", "Yi - PRC", "Yoruba - Nigeria"})
        Me.ComboBox6.Location = New System.Drawing.Point(270, 107)
        Me.ComboBox6.Name = "ComboBox6"
        Me.ComboBox6.Size = New System.Drawing.Size(396, 21)
        Me.ComboBox6.TabIndex = 8
        Me.ComboBox6.Text = "(please choose)"
        '
        'Label45
        '
        Me.Label45.AutoEllipsis = True
        Me.Label45.Location = New System.Drawing.Point(86, 164)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(176, 13)
        Me.Label45.TabIndex = 7
        Me.Label45.Text = "User Interface (UI) language:"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label43
        '
        Me.Label43.AutoEllipsis = True
        Me.Label43.Location = New System.Drawing.Point(86, 110)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(176, 13)
        Me.Label43.TabIndex = 7
        Me.Label43.Text = "Currency and date format:"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ComboBox7
        '
        Me.ComboBox7.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox7.FormattingEnabled = True
        Me.ComboBox7.Items.AddRange(New Object() {"(UTC-12:00) International Date Line West", "(UTC-11:00) Midway Island, Samoa", "(UTC-10:00) Hawaii", "(UTC-09:00) Alaska", "(UTC-08:00) Pacific Time (US &amp; Canada)", "(UTC-08:00) Tijuana, Baja California", "(UTC-07:00) Arizona", "(UTC-07:00) Chihuahua, La Paz, Mazatlan", "(UTC-07:00) Mountain Time (US &amp; Canada)", "(UTC-06:00) Central America", "(UTC-06:00) Central Time (US &amp; Canada)", "(UTC-06:00) Guadalajara, Mexico City, Monterrey", "(UTC-06:00) Saskatchewan", "(UTC-05:00) Bogota, Lima, Quito", "(UTC-05:00) Eastern Time (US &amp; Canada)", "(UTC-05:00) Indiana (East)", "(UTC-04:30) Caracas", "(UTC-04:00) Asuncion", "(UTC-04:00) Atlantic Time (Canada)", "(UTC-04:00) Georgetown, La Paz, San Juan", "(UTC-04:00) Santiago", "(UTC-03:30) Newfoundland", "(UTC-03:00) Brasilia", "(UTC-03:00) Buenos Aires", "(UTC-03:00) Cayenne", "(UTC-03:00) Greenland", "(UTC-03:00) Montevideo", "(UTC-02:00) Mid-Atlantic", "(UTC-01:00) Azores", "(UTC-01:00) Cape Verde Is.", "(UTC) Casablanca", "(UTC) Coordinated Universal Time", "(UTC) Dublin, Edinburgh, Lisbon, London", "(UTC) Monrovia, Reykjavik", "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna", "(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague", "(UTC+01:00) Brussels, Copenhagen, Madrid, Paris", "(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb", "(UTC+01:00) West Central Africa", "(UTC+02:00) Amman", "(UTC+02:00) Athens, Bucharest, Istanbul", "(UTC+02:00) Beirut", "(UTC+02:00) Cairo", "(UTC+02:00) Harare, Pretoria", "(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius", "(UTC+02:00) Jerusalem", "(UTC+02:00) Minsk", "(UTC+02:00) Windhoek", "(UTC+03:00) Baghdad", "(UTC+03:00) Kuwait, Riyadh", "(UTC+03:00) Moscow, St. Petersburg, Volgograd", "(UTC+03:00) Nairobi", "(UTC+03:00) Tbilisi", "(UTC+03:30) Tehran", "(UTC+04:00) Abu Dhabi, Muscat", "(UTC+04:00) Baku", "(UTC+04:00) Port Louis", "(UTC+04:00) Yerevan", "(UTC+04:30) Kabul", "(UTC+05:00) Ekaterinburg", "(UTC+05:00) Islamabad, Karachi", "(UTC+05:00) Tashkent", "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi", "(UTC+05:45) Kathmandu", "(UTC+06:00) Almaty, Novosibirsk", "(UTC+06:00) Astana, Dhaka", "(UTC+06:30) Yangon (Rangoon)", "(UTC+07:00) Bangkok, Hanoi, Jakarta", "(UTC+07:00) Krasnoyarsk", "(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi", "(UTC+08:00) Irkutsk, Ulaan Bataar", "(UTC+08:00) Kuala Lumpur, Singapore", "(UTC+08:00) Perth", "(UTC+08:00) Taipei", "(UTC+09:00) Osaka, Sapporo, Tokyo", "(UTC+09:00) Seoul", "(UTC+09:00) Yakutsk", "(UTC+09:30) Adelaide", "(UTC+09:30) Darwin", "(UTC+10:00) Brisbane", "(UTC+10:00) Canberra, Melbourne, Sydney", "(UTC+10:00) Guam, Port Moresby", "(UTC+10:00) Hobart", "(UTC+10:00) Vladivostok", "(UTC+11:00) Magadan, Solomon Is., New Caledonia", "(UTC+12:00) Auckland, Wellington", "(UTC+12:00) Fiji, Marshall Is.", "(UTC+12:00) Petropavlovsk-Kamchatsky", "(UTC+13:00) Nuku'alofa"})
        Me.ComboBox7.Location = New System.Drawing.Point(270, 134)
        Me.ComboBox7.Name = "ComboBox7"
        Me.ComboBox7.Size = New System.Drawing.Size(396, 21)
        Me.ComboBox7.TabIndex = 8
        Me.ComboBox7.Text = "(please choose)"
        '
        'Label44
        '
        Me.Label44.AutoEllipsis = True
        Me.Label44.Location = New System.Drawing.Point(86, 137)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(176, 13)
        Me.Label44.TabIndex = 7
        Me.Label44.Text = "Time zone:"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ComboBox5
        '
        Me.ComboBox5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox5.FormattingEnabled = True
        Me.ComboBox5.Items.AddRange(New Object() {"Albanian", "Arabic (101)", "Arabic (102)", "Arabic (102) AZERTY", "Armenian Eastern", "Armenian Western", "Assamese - Inscript", "Azeri Cyrillic", "Azeri Latin", "Bashkir", "Belarusian", "Belgian (Comma)", "Belgian (Period)", "Belgian French", "Bengali", "Bengali - Inscript", "Bengali - Inscript (Legacy)", "Bosnian (Cyrillic)", "Bulgarian", "Bulgarian (Latin)", "Bulgarian (phonetic layout)", "Bulgarian (phonetic traditional)", "Canadian French", "Canadian French (Legacy)", "Canadian Multilingual Standard", "Chinese (Simplified) - US Keyboard", "Chinese (Traditional) - US Keyboard", "Chinese (Traditional Macao S.A.R.) US Keyboard", "Chinese (Simplified, Singapore) - US keyboard", "Croatian", "Czech", "Czech (QWERTY)", "Czech Programmers", "Danish", "Devanagari - Inscript", "Divehi Phonetic", "Divehi Typewriter", "Dutch", "Estonian", "Faeroese", "Finnish", "Finnish with Sami", "French", "Gaelic", "Georgian", "Georgian (Ergonomic)", "Georgian (QWERTY)", "German", "German (IBM)", "Greek", "Greek (220)", "Greek (220) Latin", "Greek (319)", "Greek (319) Latin", "Greek Latin", "Greek Polytonic", "Greenlandic", "Gujarati", "Hausa", "Hebrew", "Hindi Traditional", "Hungarian", "ungarian 101-key", "Icelandic", "Igbo", "Inuktitut - Latin", "Inuktitut - Naqittaut", "Irish", "Italian", "Italian (142)", "Japanese", "Kannada", "Kazakh", "Khmer", "Korean", "Kyrgyz Cyrillic", "Lao", "Latin American", "Latvian", "Latvian (QWERTY)", "Lithuanian", "Lithuanian IBM", "Lithuanian New", "Luxembourgish", "Macedonian (FYROM)", "Macedonian (FYROM) - Standard", "Malayalam", "Maltese 47-Key", "Maltese 48-Key", "Maori", "Marathi", "Mongolian (Mongolian Script)", "Mongolian Cyrillic", "Nepali", "Norwegian", "Norwegian with Sami", "Oriya", "Pashto (Afghanistan)", "Persian", "Polish (214)", "Polish (Programmers)", "Portuguese", "Portuguese (Brazilian ABNT)", "Portuguese (Brazilian ABNT2)", "Punjabi", "Romanian (Legacy)", "Romanian (Programmers)", "Romanian (Standard)", "Russian", "Russian (Typewriter)", "Sami Extended Finland-Sweden", "Sami Extended Norway", "Serbian (Cyrillic)", "Serbian (Latin)", "Sesotho sa Leboa", "Setswana", "Sinhala", "Sinhala - wij 9", "Slovak", "Slovak (QWERTY)", "Slovenian", "Sorbian Extended", "Sorbian Standard", "Sorbian Standard (Legacy)", "Spanish", "Spanish Variation", "Swedish", "Swedish with Sami", "Swiss French", "Swiss German", "Syriac", "Syriac Phonetic", "Tajik", "Tamil", "Tatar", "Telugu", "Thai Kedmanee", "Thai Kedmanee (non-ShiftLock)", "Thai Pattachote", "Thai Pattachote (non-ShiftLock)", "Tibetan (PRC)", "Turkish F", "Turkish Q", "Turkmen", "Uyghur (Legacy)", "Ukrainian", "Ukrainian (Enhanced)", "United Kingdom", "United Kingdom Extended", "United States - Dvorak", "United States - International", "United States - Devorak for left hand", "United States - Dvorak for right hand", "United States - English", "Urdu", "Uyghur", "Uzbek Cyrillic", "Vietnamese", "Wolof", "Yakut", "Yoruba"})
        Me.ComboBox5.Location = New System.Drawing.Point(270, 80)
        Me.ComboBox5.Name = "ComboBox5"
        Me.ComboBox5.Size = New System.Drawing.Size(396, 21)
        Me.ComboBox5.TabIndex = 8
        Me.ComboBox5.Text = "(please choose)"
        '
        'Label42
        '
        Me.Label42.AutoEllipsis = True
        Me.Label42.Location = New System.Drawing.Point(86, 83)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(176, 13)
        Me.Label42.TabIndex = 7
        Me.Label42.Text = "Keyboard or input method:"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label41
        '
        Me.Label41.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label41.AutoEllipsis = True
        Me.Label41.Location = New System.Drawing.Point(54, 16)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(682, 55)
        Me.Label41.TabIndex = 6
        Me.Label41.Text = resources.GetString("Label41.Text")
        '
        'PictureBox9
        '
        Me.PictureBox9.Image = Global.DISMTools.My.Resources.Resources.regional_settings
        Me.PictureBox9.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox9.Name = "PictureBox9"
        Me.PictureBox9.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox9.TabIndex = 5
        Me.PictureBox9.TabStop = False
        '
        'UserEulaPanel
        '
        Me.UserEulaPanel.Controls.Add(Me.Label61)
        Me.UserEulaPanel.Controls.Add(Me.EulaScreen)
        Me.UserEulaPanel.Controls.Add(Me.CheckBox9)
        Me.UserEulaPanel.Controls.Add(Me.Label60)
        Me.UserEulaPanel.Controls.Add(Me.Label55)
        Me.UserEulaPanel.Controls.Add(Me.PictureBox10)
        Me.UserEulaPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserEulaPanel.Location = New System.Drawing.Point(0, 0)
        Me.UserEulaPanel.Name = "UserEulaPanel"
        Me.UserEulaPanel.Size = New System.Drawing.Size(752, 417)
        Me.UserEulaPanel.TabIndex = 15
        '
        'Label61
        '
        Me.Label61.Location = New System.Drawing.Point(74, 104)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(595, 55)
        Me.Label61.TabIndex = 8
        Me.Label61.Text = resources.GetString("Label61.Text")
        '
        'EulaScreen
        '
        Me.EulaScreen.Image = Global.DISMTools.My.Resources.Resources.eula_scr
        Me.EulaScreen.Location = New System.Drawing.Point(58, 161)
        Me.EulaScreen.Name = "EulaScreen"
        Me.EulaScreen.Size = New System.Drawing.Size(637, 215)
        Me.EulaScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.EulaScreen.TabIndex = 7
        Me.EulaScreen.TabStop = False
        '
        'CheckBox9
        '
        Me.CheckBox9.AutoSize = True
        Me.CheckBox9.Location = New System.Drawing.Point(58, 84)
        Me.CheckBox9.Name = "CheckBox9"
        Me.CheckBox9.Size = New System.Drawing.Size(223, 17)
        Me.CheckBox9.TabIndex = 6
        Me.CheckBox9.Text = "Hide End-User License Agreement screen"
        Me.CheckBox9.UseVisualStyleBackColor = True
        '
        'Label60
        '
        Me.Label60.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label60.AutoEllipsis = True
        Me.Label60.AutoSize = True
        Me.Label60.Location = New System.Drawing.Point(54, 383)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(271, 13)
        Me.Label60.TabIndex = 5
        Me.Label60.Text = "The End-User License Agreement screen will be shown."
        '
        'Label55
        '
        Me.Label55.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label55.AutoEllipsis = True
        Me.Label55.Location = New System.Drawing.Point(54, 16)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(682, 55)
        Me.Label55.TabIndex = 5
        Me.Label55.Text = "You can configure the Out-of-box Experience to skip the End-User License Agreemen" & _
    "t screen when setting up an unattended installation." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "What do you want to do?"
        '
        'PictureBox10
        '
        Me.PictureBox10.Image = Global.DISMTools.My.Resources.Resources.eula
        Me.PictureBox10.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox10.Name = "PictureBox10"
        Me.PictureBox10.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox10.TabIndex = 4
        Me.PictureBox10.TabStop = False
        '
        'OobeSkipsPanel
        '
        Me.OobeSkipsPanel.Controls.Add(Me.Label64)
        Me.OobeSkipsPanel.Controls.Add(Me.Label65)
        Me.OobeSkipsPanel.Controls.Add(Me.Label63)
        Me.OobeSkipsPanel.Controls.Add(Me.CheckBox11)
        Me.OobeSkipsPanel.Controls.Add(Me.CheckBox10)
        Me.OobeSkipsPanel.Controls.Add(Me.Label62)
        Me.OobeSkipsPanel.Controls.Add(Me.PictureBox11)
        Me.OobeSkipsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OobeSkipsPanel.Location = New System.Drawing.Point(0, 0)
        Me.OobeSkipsPanel.Name = "OobeSkipsPanel"
        Me.OobeSkipsPanel.Size = New System.Drawing.Size(752, 417)
        Me.OobeSkipsPanel.TabIndex = 16
        '
        'Label64
        '
        Me.Label64.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label64.Location = New System.Drawing.Point(82, 140)
        Me.Label64.Name = "Label64"
        Me.Label64.Size = New System.Drawing.Size(605, 136)
        Me.Label64.TabIndex = 7
        Me.Label64.Text = resources.GetString("Label64.Text")
        '
        'Label65
        '
        Me.Label65.AutoSize = True
        Me.Label65.Location = New System.Drawing.Point(82, 308)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(489, 13)
        Me.Label65.TabIndex = 7
        Me.Label65.Text = "Prevents the Welcome Center/Getting started window from showing after setting up " & _
    "the installation."
        '
        'Label63
        '
        Me.Label63.AutoSize = True
        Me.Label63.Location = New System.Drawing.Point(82, 119)
        Me.Label63.Name = "Label63"
        Me.Label63.Size = New System.Drawing.Size(199, 13)
        Me.Label63.TabIndex = 7
        Me.Label63.Text = "Performs a fully unattended installation."
        '
        'CheckBox11
        '
        Me.CheckBox11.AutoSize = True
        Me.CheckBox11.Checked = True
        Me.CheckBox11.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox11.Location = New System.Drawing.Point(65, 284)
        Me.CheckBox11.Name = "CheckBox11"
        Me.CheckBox11.Size = New System.Drawing.Size(216, 17)
        Me.CheckBox11.TabIndex = 6
        Me.CheckBox11.Text = "Skip user OOBE/Getting started window"
        Me.CheckBox11.UseVisualStyleBackColor = True
        '
        'CheckBox10
        '
        Me.CheckBox10.AutoSize = True
        Me.CheckBox10.Location = New System.Drawing.Point(65, 96)
        Me.CheckBox10.Name = "CheckBox10"
        Me.CheckBox10.Size = New System.Drawing.Size(211, 17)
        Me.CheckBox10.TabIndex = 6
        Me.CheckBox10.Text = "Skip machine OOBE/Windows Welcome"
        Me.CheckBox10.UseVisualStyleBackColor = True
        '
        'Label62
        '
        Me.Label62.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label62.AutoEllipsis = True
        Me.Label62.Location = New System.Drawing.Point(54, 16)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(682, 94)
        Me.Label62.TabIndex = 5
        Me.Label62.Text = "The unattended answer file can determine whether or not the OOBE screen and/or th" & _
    "e Welcome Center/Getting started window can be shown. Please configure these set" & _
    "tings."
        '
        'PictureBox11
        '
        Me.PictureBox11.Image = Global.DISMTools.My.Resources.Resources.oobe_skip
        Me.PictureBox11.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox11.Name = "PictureBox11"
        Me.PictureBox11.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox11.TabIndex = 4
        Me.PictureBox11.TabStop = False
        '
        'NetSecurityPanel
        '
        Me.NetSecurityPanel.Controls.Add(Me.GroupBox3)
        Me.NetSecurityPanel.Controls.Add(Me.ComboBox9)
        Me.NetSecurityPanel.Controls.Add(Me.Label67)
        Me.NetSecurityPanel.Controls.Add(Me.Label66)
        Me.NetSecurityPanel.Controls.Add(Me.PictureBox12)
        Me.NetSecurityPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NetSecurityPanel.Location = New System.Drawing.Point(0, 0)
        Me.NetSecurityPanel.Name = "NetSecurityPanel"
        Me.NetSecurityPanel.Size = New System.Drawing.Size(752, 417)
        Me.NetSecurityPanel.TabIndex = 17
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.PictureBox13)
        Me.GroupBox3.Controls.Add(Me.Label68)
        Me.GroupBox3.Location = New System.Drawing.Point(109, 142)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(535, 173)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Network location details"
        '
        'PictureBox13
        '
        Me.PictureBox13.Image = Global.DISMTools.My.Resources.Resources.home_net
        Me.PictureBox13.Location = New System.Drawing.Point(24, 32)
        Me.PictureBox13.Name = "PictureBox13"
        Me.PictureBox13.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox13.TabIndex = 0
        Me.PictureBox13.TabStop = False
        '
        'Label68
        '
        Me.Label68.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label68.AutoEllipsis = True
        Me.Label68.Location = New System.Drawing.Point(78, 32)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(436, 118)
        Me.Label68.TabIndex = 5
        Me.Label68.Text = "If all the computers on this network are at your home, and you recognize them, ch" & _
    "oose this option. However, if you are on a public place such as airports or coff" & _
    "ee shops, don't choose this option."
        '
        'ComboBox9
        '
        Me.ComboBox9.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox9.FormattingEnabled = True
        Me.ComboBox9.Items.AddRange(New Object() {"Home", "Work", "Public"})
        Me.ComboBox9.Location = New System.Drawing.Point(287, 107)
        Me.ComboBox9.Name = "ComboBox9"
        Me.ComboBox9.Size = New System.Drawing.Size(336, 21)
        Me.ComboBox9.TabIndex = 6
        Me.ComboBox9.Text = "Home"
        '
        'Label67
        '
        Me.Label67.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label67.AutoEllipsis = True
        Me.Label67.Location = New System.Drawing.Point(129, 110)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(152, 16)
        Me.Label67.TabIndex = 5
        Me.Label67.Text = "Network location:"
        Me.Label67.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label66
        '
        Me.Label66.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label66.AutoEllipsis = True
        Me.Label66.Location = New System.Drawing.Point(54, 16)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(682, 55)
        Me.Label66.TabIndex = 5
        Me.Label66.Text = "The unattended answer file can set your network location accordingly. This automa" & _
    "tically sets the network settings for the unattended installation." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please spe" & _
    "cify a network location."
        '
        'PictureBox12
        '
        Me.PictureBox12.Image = Global.DISMTools.My.Resources.Resources.net_security
        Me.PictureBox12.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox12.Name = "PictureBox12"
        Me.PictureBox12.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox12.TabIndex = 4
        Me.PictureBox12.TabStop = False
        '
        'WirelessPanel
        '
        Me.WirelessPanel.Controls.Add(Me.CheckBox12)
        Me.WirelessPanel.Controls.Add(Me.Label70)
        Me.WirelessPanel.Controls.Add(Me.Label69)
        Me.WirelessPanel.Controls.Add(Me.PictureBox14)
        Me.WirelessPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WirelessPanel.Location = New System.Drawing.Point(0, 0)
        Me.WirelessPanel.Name = "WirelessPanel"
        Me.WirelessPanel.Size = New System.Drawing.Size(752, 417)
        Me.WirelessPanel.TabIndex = 18
        '
        'CheckBox12
        '
        Me.CheckBox12.AutoSize = True
        Me.CheckBox12.Location = New System.Drawing.Point(124, 113)
        Me.CheckBox12.Name = "CheckBox12"
        Me.CheckBox12.Size = New System.Drawing.Size(217, 17)
        Me.CheckBox12.TabIndex = 6
        Me.CheckBox12.Text = "Hide the ""Join Wireless Network"" screen"
        Me.CheckBox12.UseVisualStyleBackColor = True
        '
        'Label70
        '
        Me.Label70.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label70.AutoEllipsis = True
        Me.Label70.Location = New System.Drawing.Point(54, 312)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(682, 67)
        Me.Label70.TabIndex = 5
        Me.Label70.Text = resources.GetString("Label70.Text")
        '
        'Label69
        '
        Me.Label69.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label69.AutoEllipsis = True
        Me.Label69.Location = New System.Drawing.Point(54, 16)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(682, 55)
        Me.Label69.TabIndex = 5
        Me.Label69.Text = "If your system has a wireless network device, the unattended answer file can skip" & _
    " the ""Join Wireless Network"" screen and continue with installation." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "What do y" & _
    "ou want to do?"
        '
        'PictureBox14
        '
        Me.PictureBox14.Image = Global.DISMTools.My.Resources.Resources.wireless
        Me.PictureBox14.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox14.Name = "PictureBox14"
        Me.PictureBox14.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox14.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox14.TabIndex = 4
        Me.PictureBox14.TabStop = False
        '
        'CompProtectPanel
        '
        Me.CompProtectPanel.Controls.Add(Me.GroupBox5)
        Me.CompProtectPanel.Controls.Add(Me.GroupBox4)
        Me.CompProtectPanel.Controls.Add(Me.Label71)
        Me.CompProtectPanel.Controls.Add(Me.PictureBox15)
        Me.CompProtectPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CompProtectPanel.Location = New System.Drawing.Point(0, 0)
        Me.CompProtectPanel.Name = "CompProtectPanel"
        Me.CompProtectPanel.Size = New System.Drawing.Size(752, 417)
        Me.CompProtectPanel.TabIndex = 19
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.WindowsUpdateDescPanel)
        Me.GroupBox5.Controls.Add(Me.PictureBox16)
        Me.GroupBox5.Controls.Add(Me.TrackBar2)
        Me.GroupBox5.Controls.Add(Me.Label73)
        Me.GroupBox5.Location = New System.Drawing.Point(57, 227)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(667, 158)
        Me.GroupBox5.TabIndex = 6
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Windows Update settings"
        '
        'WindowsUpdateDescPanel
        '
        Me.WindowsUpdateDescPanel.Controls.Add(Me.TrackBarSelValueMsg2)
        Me.WindowsUpdateDescPanel.Controls.Add(Me.TrackBarSelValueDesc2)
        Me.WindowsUpdateDescPanel.Location = New System.Drawing.Point(82, 74)
        Me.WindowsUpdateDescPanel.Name = "WindowsUpdateDescPanel"
        Me.WindowsUpdateDescPanel.Size = New System.Drawing.Size(556, 66)
        Me.WindowsUpdateDescPanel.TabIndex = 7
        '
        'TrackBarSelValueMsg2
        '
        Me.TrackBarSelValueMsg2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackBarSelValueMsg2.Location = New System.Drawing.Point(16, 22)
        Me.TrackBarSelValueMsg2.Name = "TrackBarSelValueMsg2"
        Me.TrackBarSelValueMsg2.Size = New System.Drawing.Size(532, 39)
        Me.TrackBarSelValueMsg2.TabIndex = 0
        Me.TrackBarSelValueMsg2.Text = "This is the recommended option. When Windows downloads updates, it will inform yo" & _
    "u about available updates and will also let you choose them."
        '
        'TrackBarSelValueDesc2
        '
        Me.TrackBarSelValueDesc2.AutoSize = True
        Me.TrackBarSelValueDesc2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackBarSelValueDesc2.Location = New System.Drawing.Point(4, 4)
        Me.TrackBarSelValueDesc2.Name = "TrackBarSelValueDesc2"
        Me.TrackBarSelValueDesc2.Size = New System.Drawing.Size(308, 13)
        Me.TrackBarSelValueDesc2.TabIndex = 0
        Me.TrackBarSelValueDesc2.Text = "Download updates, but let you choose which to install"
        '
        'PictureBox16
        '
        Me.PictureBox16.Image = Global.DISMTools.My.Resources.Resources.windows_update
        Me.PictureBox16.Location = New System.Drawing.Point(21, 30)
        Me.PictureBox16.Name = "PictureBox16"
        Me.PictureBox16.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox16.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox16.TabIndex = 4
        Me.PictureBox16.TabStop = False
        '
        'TrackBar2
        '
        Me.TrackBar2.LargeChange = 0
        Me.TrackBar2.Location = New System.Drawing.Point(23, 68)
        Me.TrackBar2.Maximum = 3
        Me.TrackBar2.Name = "TrackBar2"
        Me.TrackBar2.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.TrackBar2.Size = New System.Drawing.Size(45, 79)
        Me.TrackBar2.TabIndex = 6
        Me.TrackBar2.TickStyle = System.Windows.Forms.TickStyle.Both
        Me.TrackBar2.Value = 2
        '
        'Label73
        '
        Me.Label73.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label73.AutoEllipsis = True
        Me.Label73.Location = New System.Drawing.Point(59, 31)
        Me.Label73.Name = "Label73"
        Me.Label73.Size = New System.Drawing.Size(579, 28)
        Me.Label73.TabIndex = 5
        Me.Label73.Text = "Please specify the settings for Windows Update. We recommend the default value, a" & _
    "s it gives you control over the available updates."
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.GeneralProtectionDescPanel)
        Me.GroupBox4.Controls.Add(Me.TrackBar1)
        Me.GroupBox4.Controls.Add(Me.Label72)
        Me.GroupBox4.Location = New System.Drawing.Point(57, 63)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(667, 158)
        Me.GroupBox4.TabIndex = 6
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Protection settings"
        '
        'GeneralProtectionDescPanel
        '
        Me.GeneralProtectionDescPanel.Controls.Add(Me.TrackBarSelValueMsg1)
        Me.GeneralProtectionDescPanel.Controls.Add(Me.TrackBarSelValueDesc1)
        Me.GeneralProtectionDescPanel.Location = New System.Drawing.Point(82, 74)
        Me.GeneralProtectionDescPanel.Name = "GeneralProtectionDescPanel"
        Me.GeneralProtectionDescPanel.Size = New System.Drawing.Size(556, 66)
        Me.GeneralProtectionDescPanel.TabIndex = 7
        '
        'TrackBarSelValueMsg1
        '
        Me.TrackBarSelValueMsg1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackBarSelValueMsg1.Location = New System.Drawing.Point(16, 21)
        Me.TrackBarSelValueMsg1.Name = "TrackBarSelValueMsg1"
        Me.TrackBarSelValueMsg1.Size = New System.Drawing.Size(532, 39)
        Me.TrackBarSelValueMsg1.TabIndex = 0
        Me.TrackBarSelValueMsg1.Text = "Security updates and other important updates for Windows will be installed"
        '
        'TrackBarSelValueDesc1
        '
        Me.TrackBarSelValueDesc1.AutoSize = True
        Me.TrackBarSelValueDesc1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackBarSelValueDesc1.Location = New System.Drawing.Point(4, 4)
        Me.TrackBarSelValueDesc1.Name = "TrackBarSelValueDesc1"
        Me.TrackBarSelValueDesc1.Size = New System.Drawing.Size(142, 13)
        Me.TrackBarSelValueDesc1.TabIndex = 0
        Me.TrackBarSelValueDesc1.Text = "Important updates only"
        '
        'TrackBar1
        '
        Me.TrackBar1.LargeChange = 0
        Me.TrackBar1.Location = New System.Drawing.Point(23, 65)
        Me.TrackBar1.Maximum = 2
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.TrackBar1.Size = New System.Drawing.Size(45, 79)
        Me.TrackBar1.TabIndex = 6
        Me.TrackBar1.TickStyle = System.Windows.Forms.TickStyle.Both
        Me.TrackBar1.Value = 1
        '
        'Label72
        '
        Me.Label72.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label72.AutoEllipsis = True
        Me.Label72.Location = New System.Drawing.Point(20, 30)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(618, 28)
        Me.Label72.TabIndex = 5
        Me.Label72.Text = "Please specify the general protection settings. A lower value means that the targ" & _
    "et installation may be less secure and easier to compromise."
        '
        'Label71
        '
        Me.Label71.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label71.AutoEllipsis = True
        Me.Label71.Location = New System.Drawing.Point(54, 16)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(682, 44)
        Me.Label71.TabIndex = 5
        Me.Label71.Text = "The unattended answer file can set protection settings to make the unattended ins" & _
    "tallation more or less safe. Please specify the protection settings the unattend" & _
    "ed answer file must set."
        '
        'PictureBox15
        '
        Me.PictureBox15.Image = Global.DISMTools.My.Resources.Resources.comp_protection
        Me.PictureBox15.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox15.Name = "PictureBox15"
        Me.PictureBox15.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox15.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox15.TabIndex = 4
        Me.PictureBox15.TabStop = False
        '
        'ControlPanel
        '
        Me.ControlPanel.Controls.Add(Me.ControlPreview)
        Me.ControlPanel.Controls.Add(Me.ComboBox11)
        Me.ControlPanel.Controls.Add(Me.Label76)
        Me.ControlPanel.Controls.Add(Me.ComboBox10)
        Me.ControlPanel.Controls.Add(Me.Label75)
        Me.ControlPanel.Controls.Add(Me.Label74)
        Me.ControlPanel.Controls.Add(Me.PictureBox17)
        Me.ControlPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ControlPanel.Location = New System.Drawing.Point(0, 0)
        Me.ControlPanel.Name = "ControlPanel"
        Me.ControlPanel.Size = New System.Drawing.Size(752, 417)
        Me.ControlPanel.TabIndex = 20
        '
        'ControlPreview
        '
        Me.ControlPreview.Image = Global.DISMTools.My.Resources.Resources.cpl_catview
        Me.ControlPreview.Location = New System.Drawing.Point(25, 182)
        Me.ControlPreview.Name = "ControlPreview"
        Me.ControlPreview.Size = New System.Drawing.Size(702, 212)
        Me.ControlPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ControlPreview.TabIndex = 7
        Me.ControlPreview.TabStop = False
        '
        'ComboBox11
        '
        Me.ComboBox11.Enabled = False
        Me.ComboBox11.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox11.FormattingEnabled = True
        Me.ComboBox11.Items.AddRange(New Object() {"Large icons", "Small icons"})
        Me.ComboBox11.Location = New System.Drawing.Point(315, 121)
        Me.ComboBox11.Name = "ComboBox11"
        Me.ComboBox11.Size = New System.Drawing.Size(291, 21)
        Me.ComboBox11.TabIndex = 6
        Me.ComboBox11.Text = "Large icons"
        '
        'Label76
        '
        Me.Label76.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label76.AutoEllipsis = True
        Me.Label76.Enabled = False
        Me.Label76.Location = New System.Drawing.Point(147, 124)
        Me.Label76.Name = "Label76"
        Me.Label76.Size = New System.Drawing.Size(162, 13)
        Me.Label76.TabIndex = 5
        Me.Label76.Text = "Icon size:"
        Me.Label76.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ComboBox10
        '
        Me.ComboBox10.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox10.FormattingEnabled = True
        Me.ComboBox10.Items.AddRange(New Object() {"Category View", "Classic View"})
        Me.ComboBox10.Location = New System.Drawing.Point(315, 94)
        Me.ComboBox10.Name = "ComboBox10"
        Me.ComboBox10.Size = New System.Drawing.Size(291, 21)
        Me.ComboBox10.TabIndex = 6
        Me.ComboBox10.Text = "Category View"
        '
        'Label75
        '
        Me.Label75.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label75.AutoEllipsis = True
        Me.Label75.Location = New System.Drawing.Point(147, 97)
        Me.Label75.Name = "Label75"
        Me.Label75.Size = New System.Drawing.Size(162, 13)
        Me.Label75.TabIndex = 5
        Me.Label75.Text = "Control Panel view:"
        Me.Label75.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label74
        '
        Me.Label74.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label74.AutoEllipsis = True
        Me.Label74.Location = New System.Drawing.Point(54, 16)
        Me.Label74.Name = "Label74"
        Me.Label74.Size = New System.Drawing.Size(682, 55)
        Me.Label74.TabIndex = 5
        Me.Label74.Text = "The unattended answer file can configure settings for the Control Panel. Do note " & _
    "that these settings do not apply to the modern Windows ""Settings"" app." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Choose" & _
    " the layout you like, and click Next:"
        '
        'PictureBox17
        '
        Me.PictureBox17.Image = Global.DISMTools.My.Resources.Resources.cpl
        Me.PictureBox17.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox17.Name = "PictureBox17"
        Me.PictureBox17.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox17.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox17.TabIndex = 4
        Me.PictureBox17.TabStop = False
        '
        'UsrPersonalizationPanel
        '
        Me.UsrPersonalizationPanel.Controls.Add(Me.GroupBox6)
        Me.UsrPersonalizationPanel.Controls.Add(Me.Label80)
        Me.UsrPersonalizationPanel.Controls.Add(Me.TextBox5)
        Me.UsrPersonalizationPanel.Controls.Add(Me.TextBox4)
        Me.UsrPersonalizationPanel.Controls.Add(Me.PictureBox19)
        Me.UsrPersonalizationPanel.Controls.Add(Me.Label79)
        Me.UsrPersonalizationPanel.Controls.Add(Me.Label78)
        Me.UsrPersonalizationPanel.Controls.Add(Me.Label77)
        Me.UsrPersonalizationPanel.Controls.Add(Me.PictureBox18)
        Me.UsrPersonalizationPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UsrPersonalizationPanel.Location = New System.Drawing.Point(0, 0)
        Me.UsrPersonalizationPanel.Name = "UsrPersonalizationPanel"
        Me.UsrPersonalizationPanel.Size = New System.Drawing.Size(752, 417)
        Me.UsrPersonalizationPanel.TabIndex = 21
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.StartButton)
        Me.GroupBox6.Controls.Add(Me.FlowLayoutPanel1)
        Me.GroupBox6.Controls.Add(Me.Label81)
        Me.GroupBox6.Controls.Add(Me.StartScreen)
        Me.GroupBox6.Location = New System.Drawing.Point(357, 84)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(362, 317)
        Me.GroupBox6.TabIndex = 8
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Windows 8 screen colors"
        '
        'StartButton
        '
        Me.StartButton.Image = Global.DISMTools.My.Resources.Resources.win8_8
        Me.StartButton.Location = New System.Drawing.Point(18, 266)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(50, 40)
        Me.StartButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.StartButton.TabIndex = 7
        Me.StartButton.TabStop = False
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color1)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color2)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color3)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color4)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color5)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color6)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color7)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color8)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color9)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color10)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color11)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color12)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color13)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color14)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color15)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color16)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color17)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color18)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color19)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color20)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color21)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color22)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color23)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color24)
        Me.FlowLayoutPanel1.Controls.Add(Me.Win8Color25)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(18, 64)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(326, 110)
        Me.FlowLayoutPanel1.TabIndex = 6
        '
        'Win8Color1
        '
        Me.Win8Color1.Image = Global.DISMTools.My.Resources.Resources.win8_0_previews
        Me.Win8Color1.Location = New System.Drawing.Point(3, 3)
        Me.Win8Color1.Name = "Win8Color1"
        Me.Win8Color1.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color1.TabIndex = 0
        Me.Win8Color1.TabStop = False
        '
        'Win8Color2
        '
        Me.Win8Color2.Image = Global.DISMTools.My.Resources.Resources.win8_1_previews
        Me.Win8Color2.Location = New System.Drawing.Point(39, 3)
        Me.Win8Color2.Name = "Win8Color2"
        Me.Win8Color2.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color2.TabIndex = 1
        Me.Win8Color2.TabStop = False
        '
        'Win8Color3
        '
        Me.Win8Color3.Image = Global.DISMTools.My.Resources.Resources.win8_2_previews
        Me.Win8Color3.Location = New System.Drawing.Point(75, 3)
        Me.Win8Color3.Name = "Win8Color3"
        Me.Win8Color3.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color3.TabIndex = 2
        Me.Win8Color3.TabStop = False
        '
        'Win8Color4
        '
        Me.Win8Color4.Image = Global.DISMTools.My.Resources.Resources.win8_3_previews
        Me.Win8Color4.Location = New System.Drawing.Point(111, 3)
        Me.Win8Color4.Name = "Win8Color4"
        Me.Win8Color4.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color4.TabIndex = 3
        Me.Win8Color4.TabStop = False
        '
        'Win8Color5
        '
        Me.Win8Color5.Image = Global.DISMTools.My.Resources.Resources.win8_4_previews
        Me.Win8Color5.Location = New System.Drawing.Point(147, 3)
        Me.Win8Color5.Name = "Win8Color5"
        Me.Win8Color5.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color5.TabIndex = 4
        Me.Win8Color5.TabStop = False
        '
        'Win8Color6
        '
        Me.Win8Color6.Image = Global.DISMTools.My.Resources.Resources.win8_5_previews
        Me.Win8Color6.Location = New System.Drawing.Point(183, 3)
        Me.Win8Color6.Name = "Win8Color6"
        Me.Win8Color6.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color6.TabIndex = 5
        Me.Win8Color6.TabStop = False
        '
        'Win8Color7
        '
        Me.Win8Color7.Image = Global.DISMTools.My.Resources.Resources.win8_6_previews
        Me.Win8Color7.Location = New System.Drawing.Point(219, 3)
        Me.Win8Color7.Name = "Win8Color7"
        Me.Win8Color7.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color7.TabIndex = 6
        Me.Win8Color7.TabStop = False
        '
        'Win8Color8
        '
        Me.Win8Color8.Image = Global.DISMTools.My.Resources.Resources.win8_7_previews
        Me.Win8Color8.Location = New System.Drawing.Point(255, 3)
        Me.Win8Color8.Name = "Win8Color8"
        Me.Win8Color8.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color8.TabIndex = 7
        Me.Win8Color8.TabStop = False
        '
        'Win8Color9
        '
        Me.Win8Color9.Image = Global.DISMTools.My.Resources.Resources.win8_8_previews
        Me.Win8Color9.Location = New System.Drawing.Point(291, 3)
        Me.Win8Color9.Name = "Win8Color9"
        Me.Win8Color9.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color9.TabIndex = 8
        Me.Win8Color9.TabStop = False
        '
        'Win8Color10
        '
        Me.Win8Color10.Image = Global.DISMTools.My.Resources.Resources.win8_9_previews
        Me.Win8Color10.Location = New System.Drawing.Point(3, 39)
        Me.Win8Color10.Name = "Win8Color10"
        Me.Win8Color10.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color10.TabIndex = 9
        Me.Win8Color10.TabStop = False
        '
        'Win8Color11
        '
        Me.Win8Color11.Image = Global.DISMTools.My.Resources.Resources.win8_10_previews
        Me.Win8Color11.Location = New System.Drawing.Point(39, 39)
        Me.Win8Color11.Name = "Win8Color11"
        Me.Win8Color11.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color11.TabIndex = 10
        Me.Win8Color11.TabStop = False
        '
        'Win8Color12
        '
        Me.Win8Color12.Image = Global.DISMTools.My.Resources.Resources.win8_11_previews
        Me.Win8Color12.Location = New System.Drawing.Point(75, 39)
        Me.Win8Color12.Name = "Win8Color12"
        Me.Win8Color12.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color12.TabIndex = 11
        Me.Win8Color12.TabStop = False
        '
        'Win8Color13
        '
        Me.Win8Color13.Image = Global.DISMTools.My.Resources.Resources.win8_12_previews
        Me.Win8Color13.Location = New System.Drawing.Point(111, 39)
        Me.Win8Color13.Name = "Win8Color13"
        Me.Win8Color13.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color13.TabIndex = 12
        Me.Win8Color13.TabStop = False
        '
        'Win8Color14
        '
        Me.Win8Color14.Image = Global.DISMTools.My.Resources.Resources.win8_13_previews
        Me.Win8Color14.Location = New System.Drawing.Point(147, 39)
        Me.Win8Color14.Name = "Win8Color14"
        Me.Win8Color14.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color14.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color14.TabIndex = 13
        Me.Win8Color14.TabStop = False
        '
        'Win8Color15
        '
        Me.Win8Color15.Image = Global.DISMTools.My.Resources.Resources.win8_14_previews
        Me.Win8Color15.Location = New System.Drawing.Point(183, 39)
        Me.Win8Color15.Name = "Win8Color15"
        Me.Win8Color15.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color15.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color15.TabIndex = 14
        Me.Win8Color15.TabStop = False
        '
        'Win8Color16
        '
        Me.Win8Color16.Image = Global.DISMTools.My.Resources.Resources.win8_15_previews
        Me.Win8Color16.Location = New System.Drawing.Point(219, 39)
        Me.Win8Color16.Name = "Win8Color16"
        Me.Win8Color16.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color16.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color16.TabIndex = 15
        Me.Win8Color16.TabStop = False
        '
        'Win8Color17
        '
        Me.Win8Color17.Image = Global.DISMTools.My.Resources.Resources.win8_16_previews
        Me.Win8Color17.Location = New System.Drawing.Point(255, 39)
        Me.Win8Color17.Name = "Win8Color17"
        Me.Win8Color17.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color17.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color17.TabIndex = 16
        Me.Win8Color17.TabStop = False
        '
        'Win8Color18
        '
        Me.Win8Color18.Image = Global.DISMTools.My.Resources.Resources.win8_17_previews
        Me.Win8Color18.Location = New System.Drawing.Point(291, 39)
        Me.Win8Color18.Name = "Win8Color18"
        Me.Win8Color18.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color18.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color18.TabIndex = 17
        Me.Win8Color18.TabStop = False
        '
        'Win8Color19
        '
        Me.Win8Color19.Image = Global.DISMTools.My.Resources.Resources.win8_18_previews
        Me.Win8Color19.Location = New System.Drawing.Point(3, 75)
        Me.Win8Color19.Name = "Win8Color19"
        Me.Win8Color19.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color19.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color19.TabIndex = 18
        Me.Win8Color19.TabStop = False
        '
        'Win8Color20
        '
        Me.Win8Color20.Image = Global.DISMTools.My.Resources.Resources.win8_19_previews
        Me.Win8Color20.Location = New System.Drawing.Point(39, 75)
        Me.Win8Color20.Name = "Win8Color20"
        Me.Win8Color20.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color20.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color20.TabIndex = 19
        Me.Win8Color20.TabStop = False
        '
        'Win8Color21
        '
        Me.Win8Color21.Image = Global.DISMTools.My.Resources.Resources.win8_20_previews
        Me.Win8Color21.Location = New System.Drawing.Point(75, 75)
        Me.Win8Color21.Name = "Win8Color21"
        Me.Win8Color21.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color21.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color21.TabIndex = 20
        Me.Win8Color21.TabStop = False
        '
        'Win8Color22
        '
        Me.Win8Color22.Image = Global.DISMTools.My.Resources.Resources.win8_21_previews
        Me.Win8Color22.Location = New System.Drawing.Point(111, 75)
        Me.Win8Color22.Name = "Win8Color22"
        Me.Win8Color22.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color22.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color22.TabIndex = 21
        Me.Win8Color22.TabStop = False
        '
        'Win8Color23
        '
        Me.Win8Color23.Image = Global.DISMTools.My.Resources.Resources.win8_22_previews
        Me.Win8Color23.Location = New System.Drawing.Point(147, 75)
        Me.Win8Color23.Name = "Win8Color23"
        Me.Win8Color23.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color23.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color23.TabIndex = 22
        Me.Win8Color23.TabStop = False
        '
        'Win8Color24
        '
        Me.Win8Color24.Image = Global.DISMTools.My.Resources.Resources.win8_23_previews
        Me.Win8Color24.Location = New System.Drawing.Point(183, 75)
        Me.Win8Color24.Name = "Win8Color24"
        Me.Win8Color24.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color24.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color24.TabIndex = 23
        Me.Win8Color24.TabStop = False
        '
        'Win8Color25
        '
        Me.Win8Color25.Image = Global.DISMTools.My.Resources.Resources.win8_24_previews
        Me.Win8Color25.Location = New System.Drawing.Point(219, 75)
        Me.Win8Color25.Name = "Win8Color25"
        Me.Win8Color25.Size = New System.Drawing.Size(30, 30)
        Me.Win8Color25.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.Win8Color25.TabIndex = 24
        Me.Win8Color25.TabStop = False
        '
        'Label81
        '
        Me.Label81.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label81.AutoEllipsis = True
        Me.Label81.Location = New System.Drawing.Point(17, 26)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(328, 31)
        Me.Label81.TabIndex = 5
        Me.Label81.Text = "Set the background and accent colors for the Start screen and other surfaces, or " & _
    "leave them at default colors:"
        '
        'StartScreen
        '
        Me.StartScreen.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(104, Byte), Integer))
        Me.StartScreen.Controls.Add(Me.StartScreenLabel)
        Me.StartScreen.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StartScreen.ForeColor = System.Drawing.Color.White
        Me.StartScreen.Location = New System.Drawing.Point(18, 177)
        Me.StartScreen.Name = "StartScreen"
        Me.StartScreen.Size = New System.Drawing.Size(327, 129)
        Me.StartScreen.TabIndex = 8
        '
        'StartScreenLabel
        '
        Me.StartScreenLabel.AutoSize = True
        Me.StartScreenLabel.Font = New System.Drawing.Font("Segoe UI Light", 48.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StartScreenLabel.Location = New System.Drawing.Point(55, 45)
        Me.StartScreenLabel.Name = "StartScreenLabel"
        Me.StartScreenLabel.Size = New System.Drawing.Size(163, 86)
        Me.StartScreenLabel.TabIndex = 0
        Me.StartScreenLabel.Text = "Start"
        '
        'Label80
        '
        Me.Label80.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label80.AutoEllipsis = True
        Me.Label80.Location = New System.Drawing.Point(33, 281)
        Me.Label80.Name = "Label80"
        Me.Label80.Size = New System.Drawing.Size(297, 61)
        Me.Label80.TabIndex = 5
        Me.Label80.Text = "The user account description is only shown on the user account management screen." & _
    ""
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(34, 253)
        Me.TextBox5.MaxLength = 256
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(296, 21)
        Me.TextBox5.TabIndex = 7
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(34, 208)
        Me.TextBox4.MaxLength = 256
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(296, 21)
        Me.TextBox4.TabIndex = 7
        Me.TextBox4.Text = "Owner"
        '
        'PictureBox19
        '
        Me.PictureBox19.Image = Global.DISMTools.My.Resources.Resources.usr_pic
        Me.PictureBox19.Location = New System.Drawing.Point(133, 84)
        Me.PictureBox19.Name = "PictureBox19"
        Me.PictureBox19.Size = New System.Drawing.Size(96, 96)
        Me.PictureBox19.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox19.TabIndex = 6
        Me.PictureBox19.TabStop = False
        '
        'Label79
        '
        Me.Label79.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label79.AutoEllipsis = True
        Me.Label79.AutoSize = True
        Me.Label79.Location = New System.Drawing.Point(33, 234)
        Me.Label79.Name = "Label79"
        Me.Label79.Size = New System.Drawing.Size(113, 13)
        Me.Label79.TabIndex = 5
        Me.Label79.Text = "Description (optional):"
        '
        'Label78
        '
        Me.Label78.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label78.AutoEllipsis = True
        Me.Label78.AutoSize = True
        Me.Label78.Location = New System.Drawing.Point(33, 189)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(62, 13)
        Me.Label78.TabIndex = 5
        Me.Label78.Text = "User name:"
        '
        'Label77
        '
        Me.Label77.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label77.AutoEllipsis = True
        Me.Label77.Location = New System.Drawing.Point(54, 16)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(682, 55)
        Me.Label77.TabIndex = 5
        Me.Label77.Text = "To begin using the unattended installation, you need to create a user account. If" & _
    " not set up later, it will behave as an administrator." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please specify the use" & _
    "r account settings, and click Next:"
        '
        'PictureBox18
        '
        Me.PictureBox18.Image = Global.DISMTools.My.Resources.Resources.usr_personalization
        Me.PictureBox18.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox18.Name = "PictureBox18"
        Me.PictureBox18.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox18.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox18.TabIndex = 4
        Me.PictureBox18.TabStop = False
        '
        'UsrSecurityPanel
        '
        Me.UsrSecurityPanel.Controls.Add(Me.ComboBox12)
        Me.UsrSecurityPanel.Controls.Add(Me.CheckBox17)
        Me.UsrSecurityPanel.Controls.Add(Me.CheckBox16)
        Me.UsrSecurityPanel.Controls.Add(Me.CheckBox14)
        Me.UsrSecurityPanel.Controls.Add(Me.CheckBox15)
        Me.UsrSecurityPanel.Controls.Add(Me.CheckBox13)
        Me.UsrSecurityPanel.Controls.Add(Me.PasswordRepeatBox)
        Me.UsrSecurityPanel.Controls.Add(Me.PasswordBox)
        Me.UsrSecurityPanel.Controls.Add(Me.UserNameLabel)
        Me.UsrSecurityPanel.Controls.Add(Me.Label85)
        Me.UsrSecurityPanel.Controls.Add(Me.Label84)
        Me.UsrSecurityPanel.Controls.Add(Me.PictureBox21)
        Me.UsrSecurityPanel.Controls.Add(Me.Label83)
        Me.UsrSecurityPanel.Controls.Add(Me.Label82)
        Me.UsrSecurityPanel.Controls.Add(Me.PictureBox20)
        Me.UsrSecurityPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UsrSecurityPanel.Location = New System.Drawing.Point(0, 0)
        Me.UsrSecurityPanel.Name = "UsrSecurityPanel"
        Me.UsrSecurityPanel.Size = New System.Drawing.Size(752, 417)
        Me.UsrSecurityPanel.TabIndex = 22
        '
        'ComboBox12
        '
        Me.ComboBox12.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox12.FormattingEnabled = True
        Me.ComboBox12.Items.AddRange(New Object() {"Administrators", "Users"})
        Me.ComboBox12.Location = New System.Drawing.Point(322, 298)
        Me.ComboBox12.Name = "ComboBox12"
        Me.ComboBox12.Size = New System.Drawing.Size(172, 21)
        Me.ComboBox12.TabIndex = 11
        Me.ComboBox12.Text = "Administrators"
        '
        'CheckBox17
        '
        Me.CheckBox17.AutoSize = True
        Me.CheckBox17.Location = New System.Drawing.Point(500, 300)
        Me.CheckBox17.Name = "CheckBox17"
        Me.CheckBox17.Size = New System.Drawing.Size(123, 17)
        Me.CheckBox17.TabIndex = 10
        Me.CheckBox17.Text = "Show hidden groups"
        Me.CheckBox17.UseVisualStyleBackColor = True
        '
        'CheckBox16
        '
        Me.CheckBox16.AutoSize = True
        Me.CheckBox16.Location = New System.Drawing.Point(460, 361)
        Me.CheckBox16.Name = "CheckBox16"
        Me.CheckBox16.Size = New System.Drawing.Size(110, 17)
        Me.CheckBox16.TabIndex = 10
        Me.CheckBox16.Text = "Password expires"
        Me.CheckBox16.UseVisualStyleBackColor = True
        '
        'CheckBox14
        '
        Me.CheckBox14.AutoSize = True
        Me.CheckBox14.Checked = True
        Me.CheckBox14.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox14.Location = New System.Drawing.Point(151, 361)
        Me.CheckBox14.Name = "CheckBox14"
        Me.CheckBox14.Size = New System.Drawing.Size(97, 17)
        Me.CheckBox14.TabIndex = 10
        Me.CheckBox14.Text = "Enable Firewall"
        Me.CheckBox14.UseVisualStyleBackColor = True
        '
        'CheckBox15
        '
        Me.CheckBox15.AutoSize = True
        Me.CheckBox15.Location = New System.Drawing.Point(460, 338)
        Me.CheckBox15.Name = "CheckBox15"
        Me.CheckBox15.Size = New System.Drawing.Size(142, 17)
        Me.CheckBox15.TabIndex = 10
        Me.CheckBox15.Text = "Automatically log user in"
        Me.CheckBox15.UseVisualStyleBackColor = True
        '
        'CheckBox13
        '
        Me.CheckBox13.AutoSize = True
        Me.CheckBox13.Checked = True
        Me.CheckBox13.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox13.Location = New System.Drawing.Point(151, 338)
        Me.CheckBox13.Name = "CheckBox13"
        Me.CheckBox13.Size = New System.Drawing.Size(195, 17)
        Me.CheckBox13.TabIndex = 10
        Me.CheckBox13.Text = "Enable User Account Control (UAC)"
        Me.CheckBox13.UseVisualStyleBackColor = True
        '
        'PasswordRepeatBox
        '
        Me.PasswordRepeatBox.Location = New System.Drawing.Point(322, 271)
        Me.PasswordRepeatBox.Name = "PasswordRepeatBox"
        Me.PasswordRepeatBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PasswordRepeatBox.Size = New System.Drawing.Size(172, 21)
        Me.PasswordRepeatBox.TabIndex = 9
        '
        'PasswordBox
        '
        Me.PasswordBox.Location = New System.Drawing.Point(322, 245)
        Me.PasswordBox.Name = "PasswordBox"
        Me.PasswordBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PasswordBox.Size = New System.Drawing.Size(172, 21)
        Me.PasswordBox.TabIndex = 9
        '
        'UserNameLabel
        '
        Me.UserNameLabel.AutoEllipsis = True
        Me.UserNameLabel.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UserNameLabel.Location = New System.Drawing.Point(184, 197)
        Me.UserNameLabel.Name = "UserNameLabel"
        Me.UserNameLabel.Size = New System.Drawing.Size(384, 35)
        Me.UserNameLabel.TabIndex = 8
        Me.UserNameLabel.Text = "Owner"
        Me.UserNameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label85
        '
        Me.Label85.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label85.Location = New System.Drawing.Point(105, 301)
        Me.Label85.Name = "Label85"
        Me.Label85.Size = New System.Drawing.Size(211, 13)
        Me.Label85.TabIndex = 5
        Me.Label85.Text = "User group (advanced):"
        Me.Label85.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label84
        '
        Me.Label84.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label84.Location = New System.Drawing.Point(105, 274)
        Me.Label84.Name = "Label84"
        Me.Label84.Size = New System.Drawing.Size(211, 13)
        Me.Label84.TabIndex = 5
        Me.Label84.Text = "Repeat password:"
        Me.Label84.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'PictureBox21
        '
        Me.PictureBox21.Image = Global.DISMTools.My.Resources.Resources.usr_pic
        Me.PictureBox21.Location = New System.Drawing.Point(328, 95)
        Me.PictureBox21.Name = "PictureBox21"
        Me.PictureBox21.Size = New System.Drawing.Size(96, 96)
        Me.PictureBox21.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox21.TabIndex = 7
        Me.PictureBox21.TabStop = False
        '
        'Label83
        '
        Me.Label83.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label83.Location = New System.Drawing.Point(105, 248)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(211, 13)
        Me.Label83.TabIndex = 5
        Me.Label83.Text = "Password:"
        Me.Label83.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label82
        '
        Me.Label82.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label82.AutoEllipsis = True
        Me.Label82.Location = New System.Drawing.Point(54, 16)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(682, 55)
        Me.Label82.TabIndex = 5
        Me.Label82.Text = resources.GetString("Label82.Text")
        '
        'PictureBox20
        '
        Me.PictureBox20.Image = Global.DISMTools.My.Resources.Resources.usr_security
        Me.PictureBox20.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox20.Name = "PictureBox20"
        Me.PictureBox20.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox20.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox20.TabIndex = 4
        Me.PictureBox20.TabStop = False
        '
        'UsrCeipPanel
        '
        Me.UsrCeipPanel.Controls.Add(Me.CheckBox18)
        Me.UsrCeipPanel.Controls.Add(Me.Label87)
        Me.UsrCeipPanel.Controls.Add(Me.Label86)
        Me.UsrCeipPanel.Controls.Add(Me.PictureBox22)
        Me.UsrCeipPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UsrCeipPanel.Location = New System.Drawing.Point(0, 0)
        Me.UsrCeipPanel.Name = "UsrCeipPanel"
        Me.UsrCeipPanel.Size = New System.Drawing.Size(752, 417)
        Me.UsrCeipPanel.TabIndex = 23
        '
        'CheckBox18
        '
        Me.CheckBox18.AutoSize = True
        Me.CheckBox18.Location = New System.Drawing.Point(177, 130)
        Me.CheckBox18.Name = "CheckBox18"
        Me.CheckBox18.Size = New System.Drawing.Size(279, 17)
        Me.CheckBox18.TabIndex = 6
        Me.CheckBox18.Text = "Join the Customer Experience Improvement Program"
        Me.CheckBox18.UseVisualStyleBackColor = True
        '
        'Label87
        '
        Me.Label87.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label87.AutoEllipsis = True
        Me.Label87.Location = New System.Drawing.Point(54, 339)
        Me.Label87.Name = "Label87"
        Me.Label87.Size = New System.Drawing.Size(682, 55)
        Me.Label87.TabIndex = 5
        Me.Label87.Text = "For those privacy conscious, you might want to leave this option unchecked; becau" & _
    "se of telemetry (data collection) added by Microsoft that might be excessive."
        '
        'Label86
        '
        Me.Label86.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label86.AutoEllipsis = True
        Me.Label86.Location = New System.Drawing.Point(54, 16)
        Me.Label86.Name = "Label86"
        Me.Label86.Size = New System.Drawing.Size(682, 55)
        Me.Label86.TabIndex = 5
        Me.Label86.Text = resources.GetString("Label86.Text")
        '
        'PictureBox22
        '
        Me.PictureBox22.Image = Global.DISMTools.My.Resources.Resources.usr_ceip
        Me.PictureBox22.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox22.Name = "PictureBox22"
        Me.PictureBox22.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox22.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox22.TabIndex = 4
        Me.PictureBox22.TabStop = False
        '
        'SettingRecapPanel
        '
        Me.SettingRecapPanel.Controls.Add(Me.RichTextBox1)
        Me.SettingRecapPanel.Controls.Add(Me.Label89)
        Me.SettingRecapPanel.Controls.Add(Me.Label88)
        Me.SettingRecapPanel.Controls.Add(Me.PictureBox23)
        Me.SettingRecapPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SettingRecapPanel.Location = New System.Drawing.Point(0, 0)
        Me.SettingRecapPanel.Name = "SettingRecapPanel"
        Me.SettingRecapPanel.Size = New System.Drawing.Size(752, 417)
        Me.SettingRecapPanel.TabIndex = 24
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Location = New System.Drawing.Point(58, 82)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ReadOnly = True
        Me.RichTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.RichTextBox1.Size = New System.Drawing.Size(655, 257)
        Me.RichTextBox1.TabIndex = 6
        Me.RichTextBox1.Text = ""
        '
        'Label89
        '
        Me.Label89.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label89.AutoEllipsis = True
        Me.Label89.Location = New System.Drawing.Point(52, 350)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(682, 44)
        Me.Label89.TabIndex = 5
        Me.Label89.Text = "If these settings are OK to you, click Next. If something's not right, go back to" & _
    " that section and change that setting."
        '
        'Label88
        '
        Me.Label88.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label88.AutoEllipsis = True
        Me.Label88.Location = New System.Drawing.Point(54, 16)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(682, 44)
        Me.Label88.TabIndex = 5
        Me.Label88.Text = "Before creating the unattended answer file, you may want to review what you've ch" & _
    "osen; and go back (if necessary)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "To recap, here's what you have chosen to do:" & _
    ""
        '
        'PictureBox23
        '
        Me.PictureBox23.Image = Global.DISMTools.My.Resources.Resources.unattend_prep
        Me.PictureBox23.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox23.Name = "PictureBox23"
        Me.PictureBox23.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox23.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox23.TabIndex = 4
        Me.PictureBox23.TabStop = False
        '
        'EditorPanelContainer
        '
        Me.EditorPanelContainer.Controls.Add(Me.Scintilla1)
        Me.EditorPanelContainer.Controls.Add(Me.ToolStrip1)
        Me.EditorPanelContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EditorPanelContainer.Location = New System.Drawing.Point(256, 104)
        Me.EditorPanelContainer.Name = "EditorPanelContainer"
        Me.EditorPanelContainer.Size = New System.Drawing.Size(752, 417)
        Me.EditorPanelContainer.TabIndex = 1
        Me.EditorPanelContainer.Visible = False
        '
        'Scintilla1
        '
        Me.Scintilla1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Scintilla1.IndentationGuides = ScintillaNET.IndentView.LookBoth
        Me.Scintilla1.Location = New System.Drawing.Point(0, 25)
        Me.Scintilla1.Name = "Scintilla1"
        Me.Scintilla1.Size = New System.Drawing.Size(752, 392)
        Me.Scintilla1.TabIndex = 2
        Me.Scintilla1.Text = "<?xml version=""1.0"" encoding=""utf-8""?>" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "<unattend xmlns=""urn:schemas-microsoft-co" & _
    "m:unattend"">" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "</unattend>"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewTSB, Me.OpenTSB, Me.SaveTSB, Me.ToolStripSeparator1, Me.ToolStripComboBox1, Me.ToolStripComboBox2, Me.ToolStripSeparator2, Me.ToolStripButton1, Me.HelpTSB, Me.ToolStripSeparator3})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(752, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'NewTSB
        '
        Me.NewTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.NewTSB.Image = Global.DISMTools.My.Resources.Resources.newfile
        Me.NewTSB.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewTSB.Name = "NewTSB"
        Me.NewTSB.Size = New System.Drawing.Size(23, 22)
        Me.NewTSB.Text = "&Nuevo"
        '
        'OpenTSB
        '
        Me.OpenTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.OpenTSB.Image = Global.DISMTools.My.Resources.Resources.openfile
        Me.OpenTSB.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OpenTSB.Name = "OpenTSB"
        Me.OpenTSB.Size = New System.Drawing.Size(23, 22)
        Me.OpenTSB.Text = "&Abrir"
        '
        'SaveTSB
        '
        Me.SaveTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.SaveTSB.Image = Global.DISMTools.My.Resources.Resources.save_glyph
        Me.SaveTSB.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveTSB.Name = "SaveTSB"
        Me.SaveTSB.Size = New System.Drawing.Size(23, 22)
        Me.SaveTSB.Text = "&Guardar"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripComboBox1
        '
        Me.ToolStripComboBox1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripComboBox1.Name = "ToolStripComboBox1"
        Me.ToolStripComboBox1.Size = New System.Drawing.Size(121, 25)
        Me.ToolStripComboBox1.Text = "Courier New"
        '
        'ToolStripComboBox2
        '
        Me.ToolStripComboBox2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripComboBox2.Items.AddRange(New Object() {"8", "9", "10", "11", "12", "14", "16", "18", "20", "24", "28", "36", "48", "72", "96"})
        Me.ToolStripComboBox2.Name = "ToolStripComboBox2"
        Me.ToolStripComboBox2.Size = New System.Drawing.Size(75, 25)
        Me.ToolStripComboBox2.Text = "10"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.DISMTools.My.Resources.Resources.wordwrap
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'HelpTSB
        '
        Me.HelpTSB.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.HelpTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.HelpTSB.Image = Global.DISMTools.My.Resources.Resources.help_glyph
        Me.HelpTSB.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.HelpTSB.Name = "HelpTSB"
        Me.HelpTSB.Size = New System.Drawing.Size(23, 22)
        Me.HelpTSB.Text = "Ay&uda"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
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
        Me.ExpressPanelFooter.Controls.Add(Me.ExpressStatusLbl)
        Me.ExpressPanelFooter.Controls.Add(Me.TableLayoutPanel1)
        Me.ExpressPanelFooter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExpressPanelFooter.Location = New System.Drawing.Point(0, 0)
        Me.ExpressPanelFooter.Name = "ExpressPanelFooter"
        Me.ExpressPanelFooter.Size = New System.Drawing.Size(752, 40)
        Me.ExpressPanelFooter.TabIndex = 0
        '
        'ExpressStatusLbl
        '
        Me.ExpressStatusLbl.AutoSize = True
        Me.ExpressStatusLbl.Location = New System.Drawing.Point(10, 15)
        Me.ExpressStatusLbl.Name = "ExpressStatusLbl"
        Me.ExpressStatusLbl.Size = New System.Drawing.Size(38, 13)
        Me.ExpressStatusLbl.TabIndex = 2
        Me.ExpressStatusLbl.Text = "Status"
        Me.ExpressStatusLbl.Visible = False
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
        'LocationPanel
        '
        Me.LocationPanel.Controls.Add(Me.LocationLbl)
        Me.LocationPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LocationPanel.Location = New System.Drawing.Point(256, 72)
        Me.LocationPanel.Name = "LocationPanel"
        Me.LocationPanel.Size = New System.Drawing.Size(752, 32)
        Me.LocationPanel.TabIndex = 1
        '
        'LocationLbl
        '
        Me.LocationLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LocationLbl.AutoEllipsis = True
        Me.LocationLbl.Location = New System.Drawing.Point(6, 10)
        Me.LocationLbl.Name = "LocationLbl"
        Me.LocationLbl.Size = New System.Drawing.Size(734, 13)
        Me.LocationLbl.TabIndex = 3
        Me.LocationLbl.Text = "Current location"
        '
        'NewUnattendWiz
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 561)
        Me.Controls.Add(Me.ExpressPanelContainer)
        Me.Controls.Add(Me.EditorPanelContainer)
        Me.Controls.Add(Me.LocationPanel)
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
        Me.Text = "Unattended file creation wizard"
        Me.SidePanel.ResumeLayout(False)
        Me.ExpressModeSteps.ResumeLayout(False)
        Me.EditorPanelTrigger.ResumeLayout(False)
        Me.EditorPanelTrigger.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ExpressPanelTrigger.ResumeLayout(False)
        Me.ExpressPanelTrigger.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ExpressPanelContainer.ResumeLayout(False)
        Me.ActivationPanel.ResumeLayout(False)
        Me.ActivationPanel.PerformLayout()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.IncompleteWarningPanel.ResumeLayout(False)
        Me.IncompleteWarningPanel.PerformLayout()
        Me.TargetOSSelectionPanel.ResumeLayout(False)
        Me.TargetOSSelectionPanel.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EndUserLicenseAgreementPanel.ResumeLayout(False)
        Me.EndUserLicenseAgreementPanel.PerformLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CompPersonalizationPanel.ResumeLayout(False)
        Me.CompPersonalizationPanel.PerformLayout()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DiskPartPanel.ResumeLayout(False)
        Me.DiskPartPanel.PerformLayout()
        Me.PartStylePanel.ResumeLayout(False)
        Me.PartStylePanel.PerformLayout()
        CType(Me.NumericUpDown2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RegionalSettingsPanel.ResumeLayout(False)
        Me.RegionalSettingsPanel.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UserEulaPanel.ResumeLayout(False)
        Me.UserEulaPanel.PerformLayout()
        CType(Me.EulaScreen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox10, System.ComponentModel.ISupportInitialize).EndInit()
        Me.OobeSkipsPanel.ResumeLayout(False)
        Me.OobeSkipsPanel.PerformLayout()
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).EndInit()
        Me.NetSecurityPanel.ResumeLayout(False)
        Me.NetSecurityPanel.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.PictureBox13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox12, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WirelessPanel.ResumeLayout(False)
        Me.WirelessPanel.PerformLayout()
        CType(Me.PictureBox14, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CompProtectPanel.ResumeLayout(False)
        Me.CompProtectPanel.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.WindowsUpdateDescPanel.ResumeLayout(False)
        Me.WindowsUpdateDescPanel.PerformLayout()
        CType(Me.PictureBox16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBar2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GeneralProtectionDescPanel.ResumeLayout(False)
        Me.GeneralProtectionDescPanel.PerformLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox15, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ControlPanel.ResumeLayout(False)
        Me.ControlPanel.PerformLayout()
        CType(Me.ControlPreview, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox17, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UsrPersonalizationPanel.ResumeLayout(False)
        Me.UsrPersonalizationPanel.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.StartButton, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        CType(Me.Win8Color1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color16, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color17, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color18, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color19, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color20, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color22, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color23, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color24, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Win8Color25, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StartScreen.ResumeLayout(False)
        Me.StartScreen.PerformLayout()
        CType(Me.PictureBox19, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox18, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UsrSecurityPanel.ResumeLayout(False)
        Me.UsrSecurityPanel.PerformLayout()
        CType(Me.PictureBox21, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox20, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UsrCeipPanel.ResumeLayout(False)
        Me.UsrCeipPanel.PerformLayout()
        CType(Me.PictureBox22, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SettingRecapPanel.ResumeLayout(False)
        Me.SettingRecapPanel.PerformLayout()
        CType(Me.PictureBox23, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EditorPanelContainer.ResumeLayout(False)
        Me.EditorPanelContainer.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FooterContainer.ResumeLayout(False)
        Me.ExpressPanelFooter.ResumeLayout(False)
        Me.ExpressPanelFooter.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.LocationPanel.ResumeLayout(False)
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
    Friend WithEvents IncompleteWarningPanel As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Back_Button As System.Windows.Forms.Button
    Friend WithEvents Next_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Help_Button As System.Windows.Forms.Button
    Friend WithEvents LocationPanel As System.Windows.Forms.Panel
    Friend WithEvents LocationLbl As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TargetOSSelectionPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ExpressStatusLbl As System.Windows.Forms.Label
    Friend WithEvents ActivationPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents EndUserLicenseAgreementPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents GenericBox5 As System.Windows.Forms.TextBox
    Friend WithEvents GenericBox4 As System.Windows.Forms.TextBox
    Friend WithEvents GenericBox3 As System.Windows.Forms.TextBox
    Friend WithEvents GenericBox2 As System.Windows.Forms.TextBox
    Friend WithEvents KeyDash4 As System.Windows.Forms.Label
    Friend WithEvents GenericBox1 As System.Windows.Forms.TextBox
    Friend WithEvents KeyDash3 As System.Windows.Forms.Label
    Friend WithEvents GenericDash4 As System.Windows.Forms.Label
    Friend WithEvents KeyDash2 As System.Windows.Forms.Label
    Friend WithEvents GenericDash3 As System.Windows.Forms.Label
    Friend WithEvents KeyDash1 As System.Windows.Forms.Label
    Friend WithEvents GenericDash2 As System.Windows.Forms.Label
    Friend WithEvents GenericDash1 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents CompPersonalizationPanel As System.Windows.Forms.Panel
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
    Friend WithEvents CheckBox5 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents CheckBox6 As System.Windows.Forms.CheckBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents DiskPartPanel As System.Windows.Forms.Panel
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents ComboBox4 As System.Windows.Forms.ComboBox
    Friend WithEvents RadioButton6 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents CheckBox8 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox7 As System.Windows.Forms.CheckBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents PictureBox8 As System.Windows.Forms.PictureBox
    Friend WithEvents RegionalSettingsPanel As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox8 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox6 As System.Windows.Forms.ComboBox
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents ComboBox7 As System.Windows.Forms.ComboBox
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents ComboBox5 As System.Windows.Forms.ComboBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents PictureBox9 As System.Windows.Forms.PictureBox
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown2 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents UserEulaPanel As System.Windows.Forms.Panel
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents PictureBox10 As System.Windows.Forms.PictureBox
    Friend WithEvents RadioButton8 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton7 As System.Windows.Forms.RadioButton
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents PartStylePanel As System.Windows.Forms.Panel
    Friend WithEvents EulaScreen As System.Windows.Forms.PictureBox
    Friend WithEvents CheckBox9 As System.Windows.Forms.CheckBox
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents OobeSkipsPanel As System.Windows.Forms.Panel
    Friend WithEvents Label64 As System.Windows.Forms.Label
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents Label63 As System.Windows.Forms.Label
    Friend WithEvents CheckBox11 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox10 As System.Windows.Forms.CheckBox
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents PictureBox11 As System.Windows.Forms.PictureBox
    Friend WithEvents NetSecurityPanel As System.Windows.Forms.Panel
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents PictureBox12 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents PictureBox13 As System.Windows.Forms.PictureBox
    Friend WithEvents ComboBox9 As System.Windows.Forms.ComboBox
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents WirelessPanel As System.Windows.Forms.Panel
    Friend WithEvents CheckBox12 As System.Windows.Forms.CheckBox
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents Label69 As System.Windows.Forms.Label
    Friend WithEvents PictureBox14 As System.Windows.Forms.PictureBox
    Friend WithEvents CompProtectPanel As System.Windows.Forms.Panel
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents PictureBox16 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents GeneralProtectionDescPanel As System.Windows.Forms.Panel
    Friend WithEvents TrackBar1 As System.Windows.Forms.TrackBar
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents PictureBox15 As System.Windows.Forms.PictureBox
    Friend WithEvents WindowsUpdateDescPanel As System.Windows.Forms.Panel
    Friend WithEvents TrackBarSelValueMsg2 As System.Windows.Forms.Label
    Friend WithEvents TrackBarSelValueDesc2 As System.Windows.Forms.Label
    Friend WithEvents TrackBar2 As System.Windows.Forms.TrackBar
    Friend WithEvents Label73 As System.Windows.Forms.Label
    Friend WithEvents TrackBarSelValueMsg1 As System.Windows.Forms.Label
    Friend WithEvents TrackBarSelValueDesc1 As System.Windows.Forms.Label
    Friend WithEvents ControlPanel As System.Windows.Forms.Panel
    Friend WithEvents ControlPreview As System.Windows.Forms.PictureBox
    Friend WithEvents ComboBox11 As System.Windows.Forms.ComboBox
    Friend WithEvents Label76 As System.Windows.Forms.Label
    Friend WithEvents ComboBox10 As System.Windows.Forms.ComboBox
    Friend WithEvents Label75 As System.Windows.Forms.Label
    Friend WithEvents Label74 As System.Windows.Forms.Label
    Friend WithEvents PictureBox17 As System.Windows.Forms.PictureBox
    Friend WithEvents UsrPersonalizationPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox19 As System.Windows.Forms.PictureBox
    Friend WithEvents Label77 As System.Windows.Forms.Label
    Friend WithEvents PictureBox18 As System.Windows.Forms.PictureBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label78 As System.Windows.Forms.Label
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents StartButton As System.Windows.Forms.PictureBox
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Win8Color1 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color2 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color3 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color4 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color5 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color6 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color7 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color8 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color9 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color10 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color11 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color12 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color13 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color14 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color15 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color16 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color17 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color18 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color19 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color20 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color21 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color22 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color23 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color24 As System.Windows.Forms.PictureBox
    Friend WithEvents Win8Color25 As System.Windows.Forms.PictureBox
    Friend WithEvents Label81 As System.Windows.Forms.Label
    Friend WithEvents Label80 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label79 As System.Windows.Forms.Label
    Friend WithEvents StartScreen As System.Windows.Forms.Panel
    Friend WithEvents StartScreenLabel As System.Windows.Forms.Label
    Friend WithEvents UsrSecurityPanel As System.Windows.Forms.Panel
    Friend WithEvents ComboBox12 As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBox17 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox16 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox14 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox15 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox13 As System.Windows.Forms.CheckBox
    Friend WithEvents PasswordRepeatBox As System.Windows.Forms.MaskedTextBox
    Friend WithEvents PasswordBox As System.Windows.Forms.MaskedTextBox
    Friend WithEvents UserNameLabel As System.Windows.Forms.Label
    Friend WithEvents Label85 As System.Windows.Forms.Label
    Friend WithEvents Label84 As System.Windows.Forms.Label
    Friend WithEvents PictureBox21 As System.Windows.Forms.PictureBox
    Friend WithEvents Label83 As System.Windows.Forms.Label
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Friend WithEvents PictureBox20 As System.Windows.Forms.PictureBox
    Friend WithEvents UsrCeipPanel As System.Windows.Forms.Panel
    Friend WithEvents CheckBox18 As System.Windows.Forms.CheckBox
    Friend WithEvents Label87 As System.Windows.Forms.Label
    Friend WithEvents Label86 As System.Windows.Forms.Label
    Friend WithEvents PictureBox22 As System.Windows.Forms.PictureBox
    Friend WithEvents SettingRecapPanel As System.Windows.Forms.Panel
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents Label89 As System.Windows.Forms.Label
    Friend WithEvents Label88 As System.Windows.Forms.Label
    Friend WithEvents PictureBox23 As System.Windows.Forms.PictureBox
    Friend WithEvents KeyInputBox5 As System.Windows.Forms.TextBox
    Friend WithEvents KeyInputBox4 As System.Windows.Forms.TextBox
    Friend WithEvents KeyInputBox3 As System.Windows.Forms.TextBox
    Friend WithEvents KeyInputBox2 As System.Windows.Forms.TextBox
    Friend WithEvents KeyInputBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents NewTSB As System.Windows.Forms.ToolStripButton
    Friend WithEvents OpenTSB As System.Windows.Forms.ToolStripButton
    Friend WithEvents SaveTSB As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HelpTSB As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripComboBox1 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Scintilla1 As ScintillaNET.Scintilla
    Friend WithEvents ToolStripComboBox2 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents KeyCopyButton As System.Windows.Forms.Button
End Class
