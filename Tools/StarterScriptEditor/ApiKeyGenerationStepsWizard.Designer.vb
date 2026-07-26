<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ApiKeyGenerationStepsWizard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ApiKeyGenerationStepsWizard))
        Me.Close_Button = New System.Windows.Forms.Button
        Me.ButtonContainerPanel = New System.Windows.Forms.Panel
        Me.StepsContainerPanel = New System.Windows.Forms.Panel
        Me.PageContainerPanel = New System.Windows.Forms.Panel
        Me.PatContainerPanel = New System.Windows.Forms.Panel
        Me.PatCreationNextStepBtn = New System.Windows.Forms.Button
        Me.PatCreationPrevStepBtn = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.ClassicPatStepsContainerPanel = New System.Windows.Forms.Panel
        Me.ClassicPatDetailsPanel = New System.Windows.Forms.Panel
        Me.Label10 = New System.Windows.Forms.Label
        Me.PictureBox5 = New System.Windows.Forms.PictureBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.ClassicPatKeyPanel = New System.Windows.Forms.Panel
        Me.Label11 = New System.Windows.Forms.Label
        Me.PictureBox6 = New System.Windows.Forms.PictureBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.FineGrainedPatStepsContainerPanel = New System.Windows.Forms.Panel
        Me.FineGrainedPatDetailsPanel = New System.Windows.Forms.Panel
        Me.Label16 = New System.Windows.Forms.Label
        Me.PictureBox8 = New System.Windows.Forms.PictureBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.FineGrainedPatContDetailsPanel = New System.Windows.Forms.Panel
        Me.Label18 = New System.Windows.Forms.Label
        Me.PictureBox9 = New System.Windows.Forms.PictureBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.FineGrainedPatKeyPanel = New System.Windows.Forms.Panel
        Me.Label13 = New System.Windows.Forms.Label
        Me.PictureBox7 = New System.Windows.Forms.PictureBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.TokenMgmtPanel = New System.Windows.Forms.Panel
        Me.TokenMgmtNextStepBtn = New System.Windows.Forms.Button
        Me.TokenMgmtPrevStepBtn = New System.Windows.Forms.Button
        Me.TMStepsContainerPanel = New System.Windows.Forms.Panel
        Me.AccountMenuPanel = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.AccountDevSettingsPanel = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.NewPATPanel = New System.Windows.Forms.Panel
        Me.Label6 = New System.Windows.Forms.Label
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.StepsSidePanel = New System.Windows.Forms.Panel
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonContainerPanel.SuspendLayout()
        Me.StepsContainerPanel.SuspendLayout()
        Me.PageContainerPanel.SuspendLayout()
        Me.PatContainerPanel.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.ClassicPatStepsContainerPanel.SuspendLayout()
        Me.ClassicPatDetailsPanel.SuspendLayout()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ClassicPatKeyPanel.SuspendLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.FineGrainedPatStepsContainerPanel.SuspendLayout()
        Me.FineGrainedPatDetailsPanel.SuspendLayout()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FineGrainedPatContDetailsPanel.SuspendLayout()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FineGrainedPatKeyPanel.SuspendLayout()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TokenMgmtPanel.SuspendLayout()
        Me.TMStepsContainerPanel.SuspendLayout()
        Me.AccountMenuPanel.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.AccountDevSettingsPanel.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.NewPATPanel.SuspendLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StepsSidePanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Close_Button
        '
        Me.Close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Close_Button.Location = New System.Drawing.Point(931, 13)
        Me.Close_Button.Name = "Close_Button"
        Me.Close_Button.Size = New System.Drawing.Size(75, 23)
        Me.Close_Button.TabIndex = 0
        Me.Close_Button.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("Close.Button")
        Me.Close_Button.UseVisualStyleBackColor = True
        '
        'ButtonContainerPanel
        '
        Me.ButtonContainerPanel.Controls.Add(Me.Close_Button)
        Me.ButtonContainerPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ButtonContainerPanel.Location = New System.Drawing.Point(0, 527)
        Me.ButtonContainerPanel.Name = "ButtonContainerPanel"
        Me.ButtonContainerPanel.Size = New System.Drawing.Size(1018, 48)
        Me.ButtonContainerPanel.TabIndex = 1
        '
        'StepsContainerPanel
        '
        Me.StepsContainerPanel.Controls.Add(Me.PageContainerPanel)
        Me.StepsContainerPanel.Controls.Add(Me.StepsSidePanel)
        Me.StepsContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StepsContainerPanel.Location = New System.Drawing.Point(0, 0)
        Me.StepsContainerPanel.Name = "StepsContainerPanel"
        Me.StepsContainerPanel.Size = New System.Drawing.Size(1018, 527)
        Me.StepsContainerPanel.TabIndex = 2
        '
        'PageContainerPanel
        '
        Me.PageContainerPanel.Controls.Add(Me.PatContainerPanel)
        Me.PageContainerPanel.Controls.Add(Me.TokenMgmtPanel)
        Me.PageContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PageContainerPanel.Location = New System.Drawing.Point(256, 0)
        Me.PageContainerPanel.Name = "PageContainerPanel"
        Me.PageContainerPanel.Size = New System.Drawing.Size(762, 527)
        Me.PageContainerPanel.TabIndex = 3
        '
        'PatContainerPanel
        '
        Me.PatContainerPanel.Controls.Add(Me.PatCreationNextStepBtn)
        Me.PatContainerPanel.Controls.Add(Me.PatCreationPrevStepBtn)
        Me.PatContainerPanel.Controls.Add(Me.TabControl1)
        Me.PatContainerPanel.Controls.Add(Me.Label9)
        Me.PatContainerPanel.Controls.Add(Me.Label8)
        Me.PatContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PatContainerPanel.Location = New System.Drawing.Point(0, 0)
        Me.PatContainerPanel.Name = "PatContainerPanel"
        Me.PatContainerPanel.Size = New System.Drawing.Size(762, 527)
        Me.PatContainerPanel.TabIndex = 1
        Me.PatContainerPanel.Visible = False
        '
        'PatCreationNextStepBtn
        '
        Me.PatCreationNextStepBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PatCreationNextStepBtn.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PatCreationNextStepBtn.Location = New System.Drawing.Point(669, 464)
        Me.PatCreationNextStepBtn.Name = "PatCreationNextStepBtn"
        Me.PatCreationNextStepBtn.Size = New System.Drawing.Size(32, 32)
        Me.PatCreationNextStepBtn.TabIndex = 5
        Me.PatCreationNextStepBtn.Text = ">"
        Me.PatCreationNextStepBtn.UseVisualStyleBackColor = True
        '
        'PatCreationPrevStepBtn
        '
        Me.PatCreationPrevStepBtn.Enabled = False
        Me.PatCreationPrevStepBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PatCreationPrevStepBtn.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PatCreationPrevStepBtn.Location = New System.Drawing.Point(61, 465)
        Me.PatCreationPrevStepBtn.Name = "PatCreationPrevStepBtn"
        Me.PatCreationPrevStepBtn.Size = New System.Drawing.Size(32, 32)
        Me.PatCreationPrevStepBtn.TabIndex = 4
        Me.PatCreationPrevStepBtn.Text = "<"
        Me.PatCreationPrevStepBtn.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(64, 112)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(637, 344)
        Me.TabControl1.TabIndex = 3
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ClassicPatStepsContainerPanel)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(629, 318)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ClassicPat.Tab")
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'ClassicPatStepsContainerPanel
        '
        Me.ClassicPatStepsContainerPanel.Controls.Add(Me.ClassicPatDetailsPanel)
        Me.ClassicPatStepsContainerPanel.Controls.Add(Me.ClassicPatKeyPanel)
        Me.ClassicPatStepsContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ClassicPatStepsContainerPanel.Location = New System.Drawing.Point(3, 3)
        Me.ClassicPatStepsContainerPanel.Name = "ClassicPatStepsContainerPanel"
        Me.ClassicPatStepsContainerPanel.Size = New System.Drawing.Size(623, 312)
        Me.ClassicPatStepsContainerPanel.TabIndex = 3
        '
        'ClassicPatDetailsPanel
        '
        Me.ClassicPatDetailsPanel.Controls.Add(Me.Label10)
        Me.ClassicPatDetailsPanel.Controls.Add(Me.PictureBox5)
        Me.ClassicPatDetailsPanel.Controls.Add(Me.Label14)
        Me.ClassicPatDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ClassicPatDetailsPanel.Location = New System.Drawing.Point(0, 0)
        Me.ClassicPatDetailsPanel.Name = "ClassicPatDetailsPanel"
        Me.ClassicPatDetailsPanel.Size = New System.Drawing.Size(623, 312)
        Me.ClassicPatDetailsPanel.TabIndex = 0
        '
        'Label10
        '
        Me.Label10.AutoEllipsis = True
        Me.Label10.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(16, 16)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(212, 50)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ProvideTokenInfo.Label")
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = Global.StarterScriptEditor.My.Resources.Resources.classic_pat_params
        Me.PictureBox5.Location = New System.Drawing.Point(234, 3)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(386, 314)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox5.TabIndex = 0
        Me.PictureBox5.TabStop = False
        '
        'Label14
        '
        Me.Label14.AutoEllipsis = True
        Me.Label14.Location = New System.Drawing.Point(17, 66)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(207, 227)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ClassicPatDetails.Message")
        '
        'ClassicPatKeyPanel
        '
        Me.ClassicPatKeyPanel.Controls.Add(Me.Label11)
        Me.ClassicPatKeyPanel.Controls.Add(Me.PictureBox6)
        Me.ClassicPatKeyPanel.Controls.Add(Me.Label12)
        Me.ClassicPatKeyPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ClassicPatKeyPanel.Location = New System.Drawing.Point(0, 0)
        Me.ClassicPatKeyPanel.Name = "ClassicPatKeyPanel"
        Me.ClassicPatKeyPanel.Size = New System.Drawing.Size(623, 312)
        Me.ClassicPatKeyPanel.TabIndex = 1
        Me.ClassicPatKeyPanel.Visible = False
        '
        'Label11
        '
        Me.Label11.AutoEllipsis = True
        Me.Label11.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(16, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(593, 22)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("WorkWithKey.Label")
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = Global.StarterScriptEditor.My.Resources.Resources.example_classic_pat_key
        Me.PictureBox6.Location = New System.Drawing.Point(11, 123)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(598, 170)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox6.TabIndex = 0
        Me.PictureBox6.TabStop = False
        '
        'Label12
        '
        Me.Label12.AutoEllipsis = True
        Me.Label12.Location = New System.Drawing.Point(16, 46)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(588, 58)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("UseGeneratedKey.Message")
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.FineGrainedPatStepsContainerPanel)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(629, 318)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("FineGrainedPat.Tab")
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'FineGrainedPatStepsContainerPanel
        '
        Me.FineGrainedPatStepsContainerPanel.Controls.Add(Me.FineGrainedPatDetailsPanel)
        Me.FineGrainedPatStepsContainerPanel.Controls.Add(Me.FineGrainedPatContDetailsPanel)
        Me.FineGrainedPatStepsContainerPanel.Controls.Add(Me.FineGrainedPatKeyPanel)
        Me.FineGrainedPatStepsContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FineGrainedPatStepsContainerPanel.Location = New System.Drawing.Point(3, 3)
        Me.FineGrainedPatStepsContainerPanel.Name = "FineGrainedPatStepsContainerPanel"
        Me.FineGrainedPatStepsContainerPanel.Size = New System.Drawing.Size(623, 312)
        Me.FineGrainedPatStepsContainerPanel.TabIndex = 4
        '
        'FineGrainedPatDetailsPanel
        '
        Me.FineGrainedPatDetailsPanel.Controls.Add(Me.Label16)
        Me.FineGrainedPatDetailsPanel.Controls.Add(Me.PictureBox8)
        Me.FineGrainedPatDetailsPanel.Controls.Add(Me.Label17)
        Me.FineGrainedPatDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FineGrainedPatDetailsPanel.Location = New System.Drawing.Point(0, 0)
        Me.FineGrainedPatDetailsPanel.Name = "FineGrainedPatDetailsPanel"
        Me.FineGrainedPatDetailsPanel.Size = New System.Drawing.Size(623, 312)
        Me.FineGrainedPatDetailsPanel.TabIndex = 0
        '
        'Label16
        '
        Me.Label16.AutoEllipsis = True
        Me.Label16.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(16, 16)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(212, 50)
        Me.Label16.TabIndex = 1
        Me.Label16.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ProvideTokenInfo.Label")
        '
        'PictureBox8
        '
        Me.PictureBox8.Image = Global.StarterScriptEditor.My.Resources.Resources.finegrained_pat_params_1
        Me.PictureBox8.Location = New System.Drawing.Point(234, 3)
        Me.PictureBox8.Name = "PictureBox8"
        Me.PictureBox8.Size = New System.Drawing.Size(386, 314)
        Me.PictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox8.TabIndex = 0
        Me.PictureBox8.TabStop = False
        '
        'Label17
        '
        Me.Label17.AutoEllipsis = True
        Me.Label17.Location = New System.Drawing.Point(17, 66)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(207, 227)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("FineGrainedDetails.Message")
        '
        'FineGrainedPatContDetailsPanel
        '
        Me.FineGrainedPatContDetailsPanel.Controls.Add(Me.Label18)
        Me.FineGrainedPatContDetailsPanel.Controls.Add(Me.PictureBox9)
        Me.FineGrainedPatContDetailsPanel.Controls.Add(Me.Label19)
        Me.FineGrainedPatContDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FineGrainedPatContDetailsPanel.Location = New System.Drawing.Point(0, 0)
        Me.FineGrainedPatContDetailsPanel.Name = "FineGrainedPatContDetailsPanel"
        Me.FineGrainedPatContDetailsPanel.Size = New System.Drawing.Size(623, 312)
        Me.FineGrainedPatContDetailsPanel.TabIndex = 2
        Me.FineGrainedPatContDetailsPanel.Visible = False
        '
        'Label18
        '
        Me.Label18.AutoEllipsis = True
        Me.Label18.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(16, 16)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(199, 50)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ProvideTokenInfo.Label")
        '
        'PictureBox9
        '
        Me.PictureBox9.Image = Global.StarterScriptEditor.My.Resources.Resources.finegrained_pat_params_2
        Me.PictureBox9.Location = New System.Drawing.Point(230, -83)
        Me.PictureBox9.Name = "PictureBox9"
        Me.PictureBox9.Size = New System.Drawing.Size(390, 413)
        Me.PictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox9.TabIndex = 0
        Me.PictureBox9.TabStop = False
        '
        'Label19
        '
        Me.Label19.AutoEllipsis = True
        Me.Label19.Location = New System.Drawing.Point(17, 66)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(194, 227)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("FineGrainedPermissions.Message")
        '
        'FineGrainedPatKeyPanel
        '
        Me.FineGrainedPatKeyPanel.Controls.Add(Me.Label13)
        Me.FineGrainedPatKeyPanel.Controls.Add(Me.PictureBox7)
        Me.FineGrainedPatKeyPanel.Controls.Add(Me.Label15)
        Me.FineGrainedPatKeyPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FineGrainedPatKeyPanel.Location = New System.Drawing.Point(0, 0)
        Me.FineGrainedPatKeyPanel.Name = "FineGrainedPatKeyPanel"
        Me.FineGrainedPatKeyPanel.Size = New System.Drawing.Size(623, 312)
        Me.FineGrainedPatKeyPanel.TabIndex = 1
        Me.FineGrainedPatKeyPanel.Visible = False
        '
        'Label13
        '
        Me.Label13.AutoEllipsis = True
        Me.Label13.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(16, 16)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(593, 22)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("WorkWithKey.Label")
        '
        'PictureBox7
        '
        Me.PictureBox7.Image = Global.StarterScriptEditor.My.Resources.Resources.example_finegrained_pat_key
        Me.PictureBox7.Location = New System.Drawing.Point(11, 123)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(598, 170)
        Me.PictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox7.TabIndex = 0
        Me.PictureBox7.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoEllipsis = True
        Me.Label15.Location = New System.Drawing.Point(16, 46)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(588, 58)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("UseGeneratedKey.Message")
        '
        'Label9
        '
        Me.Label9.AutoEllipsis = True
        Me.Label9.Location = New System.Drawing.Point(64, 64)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(636, 32)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ChooseTokenType.Message")
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(16, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(270, 19)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("CreatePat.Label")
        '
        'TokenMgmtPanel
        '
        Me.TokenMgmtPanel.Controls.Add(Me.TokenMgmtNextStepBtn)
        Me.TokenMgmtPanel.Controls.Add(Me.TokenMgmtPrevStepBtn)
        Me.TokenMgmtPanel.Controls.Add(Me.TMStepsContainerPanel)
        Me.TokenMgmtPanel.Controls.Add(Me.Label3)
        Me.TokenMgmtPanel.Controls.Add(Me.Label2)
        Me.TokenMgmtPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TokenMgmtPanel.Location = New System.Drawing.Point(0, 0)
        Me.TokenMgmtPanel.Name = "TokenMgmtPanel"
        Me.TokenMgmtPanel.Size = New System.Drawing.Size(762, 527)
        Me.TokenMgmtPanel.TabIndex = 1
        '
        'TokenMgmtNextStepBtn
        '
        Me.TokenMgmtNextStepBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.TokenMgmtNextStepBtn.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TokenMgmtNextStepBtn.Location = New System.Drawing.Point(669, 464)
        Me.TokenMgmtNextStepBtn.Name = "TokenMgmtNextStepBtn"
        Me.TokenMgmtNextStepBtn.Size = New System.Drawing.Size(32, 32)
        Me.TokenMgmtNextStepBtn.TabIndex = 3
        Me.TokenMgmtNextStepBtn.Text = ">"
        Me.TokenMgmtNextStepBtn.UseVisualStyleBackColor = True
        '
        'TokenMgmtPrevStepBtn
        '
        Me.TokenMgmtPrevStepBtn.Enabled = False
        Me.TokenMgmtPrevStepBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.TokenMgmtPrevStepBtn.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TokenMgmtPrevStepBtn.Location = New System.Drawing.Point(61, 465)
        Me.TokenMgmtPrevStepBtn.Name = "TokenMgmtPrevStepBtn"
        Me.TokenMgmtPrevStepBtn.Size = New System.Drawing.Size(32, 32)
        Me.TokenMgmtPrevStepBtn.TabIndex = 3
        Me.TokenMgmtPrevStepBtn.Text = "<"
        Me.TokenMgmtPrevStepBtn.UseVisualStyleBackColor = True
        '
        'TMStepsContainerPanel
        '
        Me.TMStepsContainerPanel.Controls.Add(Me.AccountMenuPanel)
        Me.TMStepsContainerPanel.Controls.Add(Me.AccountDevSettingsPanel)
        Me.TMStepsContainerPanel.Controls.Add(Me.NewPATPanel)
        Me.TMStepsContainerPanel.Location = New System.Drawing.Point(61, 102)
        Me.TMStepsContainerPanel.Name = "TMStepsContainerPanel"
        Me.TMStepsContainerPanel.Size = New System.Drawing.Size(640, 357)
        Me.TMStepsContainerPanel.TabIndex = 2
        '
        'AccountMenuPanel
        '
        Me.AccountMenuPanel.Controls.Add(Me.Label4)
        Me.AccountMenuPanel.Controls.Add(Me.PictureBox1)
        Me.AccountMenuPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccountMenuPanel.Location = New System.Drawing.Point(0, 0)
        Me.AccountMenuPanel.Name = "AccountMenuPanel"
        Me.AccountMenuPanel.Size = New System.Drawing.Size(640, 357)
        Me.AccountMenuPanel.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(89, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(463, 19)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("OpenSettings.Message")
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.StarterScriptEditor.My.Resources.Resources.github_account_settings
        Me.PictureBox1.Location = New System.Drawing.Point(180, 90)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(281, 176)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'AccountDevSettingsPanel
        '
        Me.AccountDevSettingsPanel.Controls.Add(Me.Label5)
        Me.AccountDevSettingsPanel.Controls.Add(Me.PictureBox3)
        Me.AccountDevSettingsPanel.Controls.Add(Me.PictureBox2)
        Me.AccountDevSettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccountDevSettingsPanel.Location = New System.Drawing.Point(0, 0)
        Me.AccountDevSettingsPanel.Name = "AccountDevSettingsPanel"
        Me.AccountDevSettingsPanel.Size = New System.Drawing.Size(640, 357)
        Me.AccountDevSettingsPanel.TabIndex = 0
        Me.AccountDevSettingsPanel.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(11, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(619, 19)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("OpenDeveloperSettings.Message")
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.StarterScriptEditor.My.Resources.Resources.github_devapps_tokens
        Me.PictureBox3.Location = New System.Drawing.Point(323, 56)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(296, 298)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox3.TabIndex = 2
        Me.PictureBox3.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.StarterScriptEditor.My.Resources.Resources.account_dev_settings
        Me.PictureBox2.Location = New System.Drawing.Point(21, 56)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(296, 298)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 2
        Me.PictureBox2.TabStop = False
        '
        'NewPATPanel
        '
        Me.NewPATPanel.Controls.Add(Me.Label6)
        Me.NewPATPanel.Controls.Add(Me.PictureBox4)
        Me.NewPATPanel.Controls.Add(Me.Label7)
        Me.NewPATPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NewPATPanel.Location = New System.Drawing.Point(0, 0)
        Me.NewPATPanel.Name = "NewPATPanel"
        Me.NewPATPanel.Size = New System.Drawing.Size(640, 357)
        Me.NewPATPanel.TabIndex = 0
        Me.NewPATPanel.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(112, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(417, 19)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("GenerateToken.Message")
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.StarterScriptEditor.My.Resources.Resources.generate_pat
        Me.PictureBox4.Location = New System.Drawing.Point(157, 116)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(326, 166)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox4.TabIndex = 2
        Me.PictureBox4.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(156, 300)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(329, 13)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ClassicTokenNote.Message")
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(64, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(247, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("TokenManagementSteps.Message")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(291, 19)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("AccessTokenManagement.Label")
        '
        'StepsSidePanel
        '
        Me.StepsSidePanel.Controls.Add(Me.LinkLabel2)
        Me.StepsSidePanel.Controls.Add(Me.LinkLabel1)
        Me.StepsSidePanel.Controls.Add(Me.Label1)
        Me.StepsSidePanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.StepsSidePanel.Location = New System.Drawing.Point(0, 0)
        Me.StepsSidePanel.Name = "StepsSidePanel"
        Me.StepsSidePanel.Size = New System.Drawing.Size(256, 527)
        Me.StepsSidePanel.TabIndex = 0
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel2.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel2.Location = New System.Drawing.Point(28, 64)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(174, 13)
        Me.LinkLabel2.TabIndex = 1
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("CreatePat.Link")
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel1.Location = New System.Drawing.Point(28, 40)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(216, 13)
        Me.LinkLabel1.TabIndex = 1
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("AccessTokenManagement.Link")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(216, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("SelectStep.Message")
        '
        'ApiKeyGenerationStepsWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Close_Button
        Me.ClientSize = New System.Drawing.Size(1018, 575)
        Me.Controls.Add(Me.StepsContainerPanel)
        Me.Controls.Add(Me.ButtonContainerPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ApiKeyGenerationStepsWizard"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("Title")
        Me.ButtonContainerPanel.ResumeLayout(False)
        Me.StepsContainerPanel.ResumeLayout(False)
        Me.PageContainerPanel.ResumeLayout(False)
        Me.PatContainerPanel.ResumeLayout(False)
        Me.PatContainerPanel.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.ClassicPatStepsContainerPanel.ResumeLayout(False)
        Me.ClassicPatDetailsPanel.ResumeLayout(False)
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ClassicPatKeyPanel.ResumeLayout(False)
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.FineGrainedPatStepsContainerPanel.ResumeLayout(False)
        Me.FineGrainedPatDetailsPanel.ResumeLayout(False)
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FineGrainedPatContDetailsPanel.ResumeLayout(False)
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FineGrainedPatKeyPanel.ResumeLayout(False)
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TokenMgmtPanel.ResumeLayout(False)
        Me.TokenMgmtPanel.PerformLayout()
        Me.TMStepsContainerPanel.ResumeLayout(False)
        Me.AccountMenuPanel.ResumeLayout(False)
        Me.AccountMenuPanel.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.AccountDevSettingsPanel.ResumeLayout(False)
        Me.AccountDevSettingsPanel.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.NewPATPanel.ResumeLayout(False)
        Me.NewPATPanel.PerformLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StepsSidePanel.ResumeLayout(False)
        Me.StepsSidePanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Close_Button As System.Windows.Forms.Button
    Friend WithEvents ButtonContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents StepsContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents StepsSidePanel As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PatContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents TokenMgmtPanel As System.Windows.Forms.Panel
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PageContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents TMStepsContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents AccountMenuPanel As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TokenMgmtNextStepBtn As System.Windows.Forms.Button
    Friend WithEvents TokenMgmtPrevStepBtn As System.Windows.Forms.Button
    Friend WithEvents NewPATPanel As System.Windows.Forms.Panel
    Friend WithEvents AccountDevSettingsPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents PatCreationNextStepBtn As System.Windows.Forms.Button
    Friend WithEvents PatCreationPrevStepBtn As System.Windows.Forms.Button
    Friend WithEvents ClassicPatStepsContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents ClassicPatDetailsPanel As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents ClassicPatKeyPanel As System.Windows.Forms.Panel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents FineGrainedPatStepsContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents FineGrainedPatContDetailsPanel As System.Windows.Forms.Panel
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents PictureBox9 As System.Windows.Forms.PictureBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents FineGrainedPatDetailsPanel As System.Windows.Forms.Panel
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents PictureBox8 As System.Windows.Forms.PictureBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents FineGrainedPatKeyPanel As System.Windows.Forms.Panel
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
    Friend WithEvents Label15 As System.Windows.Forms.Label

End Class
