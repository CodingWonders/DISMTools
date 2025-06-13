<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.ButtonContainerPanel = New System.Windows.Forms.Panel()
        Me.GetImgInfoBtn = New System.Windows.Forms.Button()
        Me.ExportDrvsBtn = New System.Windows.Forms.Button()
        Me.BackButton = New System.Windows.Forms.Button()
        Me.NextButton = New System.Windows.Forms.Button()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.PageContainerPanel = New System.Windows.Forms.Panel()
        Me.ErrorPanel = New System.Windows.Forms.Panel()
        Me.ErrorTextBox = New System.Windows.Forms.TextBox()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.FinishPanel = New System.Windows.Forms.Panel()
        Me.ProgressBar2 = New System.Windows.Forms.ProgressBar()
        Me.RestartButton = New System.Windows.Forms.Button()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.InstallationPanel = New System.Windows.Forms.Panel()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.StepPanelContainer = New System.Windows.Forms.Panel()
        Me.BootMgrStep = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.DTPEStep = New System.Windows.Forms.Panel()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.WindowsStep = New System.Windows.Forms.Panel()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.ExplanationPanel = New System.Windows.Forms.Panel()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.ImageInfoPanel = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.DisclaimerPanel = New System.Windows.Forms.Panel()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SlideshowTimer = New System.Windows.Forms.Timer(Me.components)
        Me.InstallerBW = New System.ComponentModel.BackgroundWorker()
        Me.BCDEditProcess = New System.Diagnostics.Process()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ExportDrvsFBD = New System.Windows.Forms.FolderBrowserDialog()
        Me.ExportDrvsBW = New System.ComponentModel.BackgroundWorker()
        Me.ButtonContainerPanel.SuspendLayout()
        Me.PageContainerPanel.SuspendLayout()
        Me.ErrorPanel.SuspendLayout()
        Me.FinishPanel.SuspendLayout()
        Me.InstallationPanel.SuspendLayout()
        Me.StepPanelContainer.SuspendLayout()
        Me.BootMgrStep.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DTPEStep.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WindowsStep.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ExplanationPanel.SuspendLayout()
        Me.ImageInfoPanel.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.DisclaimerPanel.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonContainerPanel
        '
        Me.ButtonContainerPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(239, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.ButtonContainerPanel.Controls.Add(Me.GetImgInfoBtn)
        Me.ButtonContainerPanel.Controls.Add(Me.ExportDrvsBtn)
        Me.ButtonContainerPanel.Controls.Add(Me.BackButton)
        Me.ButtonContainerPanel.Controls.Add(Me.NextButton)
        Me.ButtonContainerPanel.Controls.Add(Me.ExitButton)
        Me.ButtonContainerPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ButtonContainerPanel.Location = New System.Drawing.Point(0, 525)
        Me.ButtonContainerPanel.Name = "ButtonContainerPanel"
        Me.ButtonContainerPanel.Size = New System.Drawing.Size(784, 36)
        Me.ButtonContainerPanel.TabIndex = 0
        '
        'GetImgInfoBtn
        '
        Me.GetImgInfoBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GetImgInfoBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GetImgInfoBtn.Location = New System.Drawing.Point(244, 6)
        Me.GetImgInfoBtn.Name = "GetImgInfoBtn"
        Me.GetImgInfoBtn.Size = New System.Drawing.Size(230, 23)
        Me.GetImgInfoBtn.TabIndex = 1
        Me.GetImgInfoBtn.Text = "Get image information"
        Me.GetImgInfoBtn.UseVisualStyleBackColor = True
        Me.GetImgInfoBtn.Visible = False
        '
        'ExportDrvsBtn
        '
        Me.ExportDrvsBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExportDrvsBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ExportDrvsBtn.Location = New System.Drawing.Point(8, 6)
        Me.ExportDrvsBtn.Name = "ExportDrvsBtn"
        Me.ExportDrvsBtn.Size = New System.Drawing.Size(230, 23)
        Me.ExportDrvsBtn.TabIndex = 1
        Me.ExportDrvsBtn.Text = "Export system drivers..."
        Me.ExportDrvsBtn.UseVisualStyleBackColor = True
        Me.ExportDrvsBtn.Visible = False
        '
        'BackButton
        '
        Me.BackButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BackButton.Enabled = False
        Me.BackButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BackButton.Location = New System.Drawing.Point(538, 6)
        Me.BackButton.Name = "BackButton"
        Me.BackButton.Size = New System.Drawing.Size(75, 23)
        Me.BackButton.TabIndex = 0
        Me.BackButton.Text = "Back"
        Me.BackButton.UseVisualStyleBackColor = True
        '
        'NextButton
        '
        Me.NextButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NextButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.NextButton.Location = New System.Drawing.Point(619, 6)
        Me.NextButton.Name = "NextButton"
        Me.NextButton.Size = New System.Drawing.Size(75, 23)
        Me.NextButton.TabIndex = 0
        Me.NextButton.Text = "Next"
        Me.NextButton.UseVisualStyleBackColor = True
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ExitButton.Location = New System.Drawing.Point(700, 6)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 0
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'PageContainerPanel
        '
        Me.PageContainerPanel.Controls.Add(Me.ErrorPanel)
        Me.PageContainerPanel.Controls.Add(Me.FinishPanel)
        Me.PageContainerPanel.Controls.Add(Me.InstallationPanel)
        Me.PageContainerPanel.Controls.Add(Me.ExplanationPanel)
        Me.PageContainerPanel.Controls.Add(Me.ImageInfoPanel)
        Me.PageContainerPanel.Controls.Add(Me.DisclaimerPanel)
        Me.PageContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PageContainerPanel.Location = New System.Drawing.Point(0, 0)
        Me.PageContainerPanel.Name = "PageContainerPanel"
        Me.PageContainerPanel.Size = New System.Drawing.Size(784, 525)
        Me.PageContainerPanel.TabIndex = 1
        '
        'ErrorPanel
        '
        Me.ErrorPanel.Controls.Add(Me.ErrorTextBox)
        Me.ErrorPanel.Controls.Add(Me.Label38)
        Me.ErrorPanel.Controls.Add(Me.Label37)
        Me.ErrorPanel.Controls.Add(Me.Label36)
        Me.ErrorPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ErrorPanel.Location = New System.Drawing.Point(0, 0)
        Me.ErrorPanel.Name = "ErrorPanel"
        Me.ErrorPanel.Size = New System.Drawing.Size(784, 525)
        Me.ErrorPanel.TabIndex = 5
        Me.ErrorPanel.Visible = False
        '
        'ErrorTextBox
        '
        Me.ErrorTextBox.BackColor = System.Drawing.Color.White
        Me.ErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ErrorTextBox.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ErrorTextBox.Location = New System.Drawing.Point(68, 126)
        Me.ErrorTextBox.Multiline = True
        Me.ErrorTextBox.Name = "ErrorTextBox"
        Me.ErrorTextBox.ReadOnly = True
        Me.ErrorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ErrorTextBox.Size = New System.Drawing.Size(649, 302)
        Me.ErrorTextBox.TabIndex = 9
        '
        'Label38
        '
        Me.Label38.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label38.Location = New System.Drawing.Point(14, 439)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(758, 72)
        Me.Label38.TabIndex = 8
        Me.Label38.Text = resources.GetString("Label38.Text")
        '
        'Label37
        '
        Me.Label37.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label37.Location = New System.Drawing.Point(14, 43)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(758, 72)
        Me.Label37.TabIndex = 8
        Me.Label37.Text = "Your computer could not be prepared to boot to the next stage of operating system" & _
    " installation. Any changes made to your computer will be undone." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "See below fo" & _
    "r reasons why this process has failed:"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.Location = New System.Drawing.Point(13, 13)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(165, 21)
        Me.Label36.TabIndex = 7
        Me.Label36.Text = "We ran into a problem"
        '
        'FinishPanel
        '
        Me.FinishPanel.Controls.Add(Me.ProgressBar2)
        Me.FinishPanel.Controls.Add(Me.RestartButton)
        Me.FinishPanel.Controls.Add(Me.Label32)
        Me.FinishPanel.Controls.Add(Me.Label35)
        Me.FinishPanel.Controls.Add(Me.Label33)
        Me.FinishPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FinishPanel.Location = New System.Drawing.Point(0, 0)
        Me.FinishPanel.Name = "FinishPanel"
        Me.FinishPanel.Size = New System.Drawing.Size(784, 525)
        Me.FinishPanel.TabIndex = 4
        Me.FinishPanel.Visible = False
        '
        'ProgressBar2
        '
        Me.ProgressBar2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar2.Location = New System.Drawing.Point(17, 441)
        Me.ProgressBar2.Name = "ProgressBar2"
        Me.ProgressBar2.Size = New System.Drawing.Size(751, 23)
        Me.ProgressBar2.TabIndex = 9
        '
        'RestartButton
        '
        Me.RestartButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.RestartButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RestartButton.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RestartButton.Location = New System.Drawing.Point(328, 470)
        Me.RestartButton.Name = "RestartButton"
        Me.RestartButton.Size = New System.Drawing.Size(128, 34)
        Me.RestartButton.TabIndex = 8
        Me.RestartButton.Text = "Restart Now"
        Me.RestartButton.UseVisualStyleBackColor = True
        '
        'Label32
        '
        Me.Label32.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label32.Location = New System.Drawing.Point(14, 43)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(758, 120)
        Me.Label32.TabIndex = 7
        Me.Label32.Text = resources.GetString("Label32.Text")
        '
        'Label35
        '
        Me.Label35.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label35.AutoEllipsis = True
        Me.Label35.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(17, 410)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(751, 21)
        Me.Label35.TabIndex = 6
        Me.Label35.Text = "Your computer will restart in 10 seconds..."
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(13, 13)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(225, 21)
        Me.Label33.TabIndex = 6
        Me.Label33.Text = "Your computer needs to restart"
        '
        'InstallationPanel
        '
        Me.InstallationPanel.Controls.Add(Me.ProgressBar1)
        Me.InstallationPanel.Controls.Add(Me.StepPanelContainer)
        Me.InstallationPanel.Controls.Add(Me.Label34)
        Me.InstallationPanel.Controls.Add(Me.Label19)
        Me.InstallationPanel.Controls.Add(Me.Label17)
        Me.InstallationPanel.Controls.Add(Me.Label18)
        Me.InstallationPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InstallationPanel.Location = New System.Drawing.Point(0, 0)
        Me.InstallationPanel.Name = "InstallationPanel"
        Me.InstallationPanel.Size = New System.Drawing.Size(784, 525)
        Me.InstallationPanel.TabIndex = 3
        Me.InstallationPanel.Visible = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 486)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(760, 23)
        Me.ProgressBar1.TabIndex = 7
        '
        'StepPanelContainer
        '
        Me.StepPanelContainer.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.StepPanelContainer.Controls.Add(Me.BootMgrStep)
        Me.StepPanelContainer.Controls.Add(Me.DTPEStep)
        Me.StepPanelContainer.Controls.Add(Me.WindowsStep)
        Me.StepPanelContainer.Location = New System.Drawing.Point(50, 120)
        Me.StepPanelContainer.Name = "StepPanelContainer"
        Me.StepPanelContainer.Size = New System.Drawing.Size(685, 327)
        Me.StepPanelContainer.TabIndex = 6
        '
        'BootMgrStep
        '
        Me.BootMgrStep.Controls.Add(Me.PictureBox1)
        Me.BootMgrStep.Controls.Add(Me.Label23)
        Me.BootMgrStep.Controls.Add(Me.Label22)
        Me.BootMgrStep.Controls.Add(Me.Label21)
        Me.BootMgrStep.Controls.Add(Me.Label20)
        Me.BootMgrStep.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BootMgrStep.Location = New System.Drawing.Point(0, 0)
        Me.BootMgrStep.Name = "BootMgrStep"
        Me.BootMgrStep.Size = New System.Drawing.Size(685, 327)
        Me.BootMgrStep.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Installer.My.Resources.Resources.hotinstall_step1
        Me.PictureBox1.Location = New System.Drawing.Point(236, -2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(833, 621)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Segoe UI Light", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Gray
        Me.Label23.Location = New System.Drawing.Point(123, 254)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(53, 65)
        Me.Label23.TabIndex = 4
        Me.Label23.Text = "3"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Segoe UI Light", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.Gray
        Me.Label22.Location = New System.Drawing.Point(64, 254)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(53, 65)
        Me.Label22.TabIndex = 4
        Me.Label22.Text = "2"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Segoe UI Light", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(13, 254)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(45, 65)
        Me.Label21.TabIndex = 4
        Me.Label21.Text = "1"
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(18, 22)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(201, 235)
        Me.Label20.TabIndex = 4
        Me.Label20.Text = "In your system boot manager, select ""<entry>"" and press ENTER"
        '
        'DTPEStep
        '
        Me.DTPEStep.Controls.Add(Me.PictureBox2)
        Me.DTPEStep.Controls.Add(Me.Label24)
        Me.DTPEStep.Controls.Add(Me.Label25)
        Me.DTPEStep.Controls.Add(Me.Label26)
        Me.DTPEStep.Controls.Add(Me.Label27)
        Me.DTPEStep.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DTPEStep.Location = New System.Drawing.Point(0, 0)
        Me.DTPEStep.Name = "DTPEStep"
        Me.DTPEStep.Size = New System.Drawing.Size(685, 327)
        Me.DTPEStep.TabIndex = 1
        Me.DTPEStep.Visible = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.Installer.My.Resources.Resources.hotinstall_step2
        Me.PictureBox2.Location = New System.Drawing.Point(236, -173)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(833, 621)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 5
        Me.PictureBox2.TabStop = False
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Segoe UI Light", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.ForeColor = System.Drawing.Color.Gray
        Me.Label24.Location = New System.Drawing.Point(123, 254)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(53, 65)
        Me.Label24.TabIndex = 6
        Me.Label24.Text = "3"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Segoe UI Light", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(64, 254)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(53, 65)
        Me.Label25.TabIndex = 7
        Me.Label25.Text = "2"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Segoe UI Light", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.ForeColor = System.Drawing.Color.Gray
        Me.Label26.Location = New System.Drawing.Point(13, 254)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(45, 65)
        Me.Label26.TabIndex = 8
        Me.Label26.Text = "1"
        '
        'Label27
        '
        Me.Label27.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(18, 22)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(201, 235)
        Me.Label27.TabIndex = 9
        Me.Label27.Text = "Specify the target disk and partition, and the index to apply." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "We will take it" & _
    " from there"
        '
        'WindowsStep
        '
        Me.WindowsStep.Controls.Add(Me.PictureBox3)
        Me.WindowsStep.Controls.Add(Me.Label28)
        Me.WindowsStep.Controls.Add(Me.Label29)
        Me.WindowsStep.Controls.Add(Me.Label30)
        Me.WindowsStep.Controls.Add(Me.Label31)
        Me.WindowsStep.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WindowsStep.Location = New System.Drawing.Point(0, 0)
        Me.WindowsStep.Name = "WindowsStep"
        Me.WindowsStep.Size = New System.Drawing.Size(685, 327)
        Me.WindowsStep.TabIndex = 2
        Me.WindowsStep.Visible = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.Installer.My.Resources.Resources.hotinstall_step3
        Me.PictureBox3.Location = New System.Drawing.Point(236, -2)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(449, 329)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox3.TabIndex = 5
        Me.PictureBox3.TabStop = False
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Segoe UI Light", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(123, 254)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(53, 65)
        Me.Label28.TabIndex = 6
        Me.Label28.Text = "3"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Segoe UI Light", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.Gray
        Me.Label29.Location = New System.Drawing.Point(64, 254)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(53, 65)
        Me.Label29.TabIndex = 7
        Me.Label29.Text = "2"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Segoe UI Light", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.ForeColor = System.Drawing.Color.Gray
        Me.Label30.Location = New System.Drawing.Point(13, 254)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(45, 65)
        Me.Label30.TabIndex = 8
        Me.Label30.Text = "1"
        '
        'Label31
        '
        Me.Label31.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(18, 23)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(201, 235)
        Me.Label31.TabIndex = 9
        Me.Label31.Text = "Verify that the target operating system works the way you want." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If the customi" & _
    "zations were successful, go ahead and perform a bigger-scale deployment."
        '
        'Label34
        '
        Me.Label34.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label34.AutoEllipsis = True
        Me.Label34.Location = New System.Drawing.Point(493, 464)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(279, 15)
        Me.Label34.TabIndex = 5
        Me.Label34.Text = "API Progress: 0"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.Label34.Visible = False
        '
        'Label19
        '
        Me.Label19.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(14, 464)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(55, 15)
        Me.Label19.TabIndex = 5
        Me.Label19.Text = "Progress:"
        '
        'Label17
        '
        Me.Label17.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label17.Location = New System.Drawing.Point(14, 43)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(758, 72)
        Me.Label17.TabIndex = 5
        Me.Label17.Text = resources.GetString("Label17.Text")
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(13, 13)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(298, 21)
        Me.Label18.TabIndex = 4
        Me.Label18.Text = "Preparing your computer for installation..."
        '
        'ExplanationPanel
        '
        Me.ExplanationPanel.Controls.Add(Me.Label15)
        Me.ExplanationPanel.Controls.Add(Me.Label16)
        Me.ExplanationPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExplanationPanel.Location = New System.Drawing.Point(0, 0)
        Me.ExplanationPanel.Name = "ExplanationPanel"
        Me.ExplanationPanel.Size = New System.Drawing.Size(784, 525)
        Me.ExplanationPanel.TabIndex = 2
        Me.ExplanationPanel.Visible = False
        '
        'Label15
        '
        Me.Label15.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label15.Location = New System.Drawing.Point(14, 43)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(758, 206)
        Me.Label15.TabIndex = 5
        Me.Label15.Text = resources.GetString("Label15.Text")
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(13, 13)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(215, 21)
        Me.Label16.TabIndex = 4
        Me.Label16.Text = "About the installation process"
        '
        'ImageInfoPanel
        '
        Me.ImageInfoPanel.Controls.Add(Me.GroupBox2)
        Me.ImageInfoPanel.Controls.Add(Me.GroupBox1)
        Me.ImageInfoPanel.Controls.Add(Me.Label6)
        Me.ImageInfoPanel.Controls.Add(Me.Label7)
        Me.ImageInfoPanel.Controls.Add(Me.Label5)
        Me.ImageInfoPanel.Controls.Add(Me.Label14)
        Me.ImageInfoPanel.Controls.Add(Me.Label3)
        Me.ImageInfoPanel.Controls.Add(Me.Label4)
        Me.ImageInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ImageInfoPanel.Location = New System.Drawing.Point(0, 0)
        Me.ImageInfoPanel.Name = "ImageInfoPanel"
        Me.ImageInfoPanel.Size = New System.Drawing.Size(784, 525)
        Me.ImageInfoPanel.TabIndex = 1
        Me.ImageInfoPanel.Visible = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ListView1)
        Me.GroupBox2.Location = New System.Drawing.Point(17, 224)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(755, 176)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Information of installation image"
        '
        'ListView1
        '
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(3, 19)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(749, 154)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "#"
        Me.ColumnHeader1.Width = 26
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Image name"
        Me.ColumnHeader2.Width = 188
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Image description"
        Me.ColumnHeader3.Width = 211
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Image version"
        Me.ColumnHeader4.Width = 152
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Image architecture"
        Me.ColumnHeader5.Width = 141
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Location = New System.Drawing.Point(17, 118)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(755, 100)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Information of boot image"
        '
        'Label10
        '
        Me.Label10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(386, 52)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(109, 15)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "Image architecture:"
        '
        'Label9
        '
        Me.Label9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(33, 52)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(84, 15)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Image version:"
        '
        'Label13
        '
        Me.Label13.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(501, 52)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(46, 15)
        Me.Label13.TabIndex = 3
        Me.Label13.Text = "<arch>"
        '
        'Label12
        '
        Me.Label12.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(123, 52)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(61, 15)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "<version>"
        '
        'Label11
        '
        Me.Label11.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(123, 30)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(53, 15)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "<name>"
        '
        'Label8
        '
        Me.Label8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(33, 30)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(76, 15)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Image name:"
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(184, 413)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(46, 15)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "<arch>"
        '
        'Label7
        '
        Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.ForeColor = System.Drawing.Color.OrangeRed
        Me.Label7.Location = New System.Drawing.Point(297, 413)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(471, 15)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "The architectures of the boot image and your computer are incompatible"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.Label7.Visible = False
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 413)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(130, 15)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Computer architecture:"
        '
        'Label14
        '
        Me.Label14.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.Location = New System.Drawing.Point(14, 439)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(758, 83)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = resources.GetString("Label14.Text")
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.Location = New System.Drawing.Point(14, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(758, 47)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Check if this disc image contains the Windows image you want to test and click Ne" & _
    "xt"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(13, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(218, 21)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Review image file information"
        '
        'DisclaimerPanel
        '
        Me.DisclaimerPanel.Controls.Add(Me.CheckBox1)
        Me.DisclaimerPanel.Controls.Add(Me.TabControl1)
        Me.DisclaimerPanel.Controls.Add(Me.Label2)
        Me.DisclaimerPanel.Controls.Add(Me.Label1)
        Me.DisclaimerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DisclaimerPanel.Location = New System.Drawing.Point(0, 0)
        Me.DisclaimerPanel.Name = "DisclaimerPanel"
        Me.DisclaimerPanel.Size = New System.Drawing.Size(784, 525)
        Me.DisclaimerPanel.TabIndex = 0
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(17, 494)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(210, 19)
        Me.CheckBox1.TabIndex = 3
        Me.CheckBox1.Text = "I agree to the disclaimers in all tabs"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(17, 94)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(755, 397)
        Me.TabControl1.TabIndex = 2
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TextBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(747, 369)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Warranty disclaimers"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(3, 3)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(741, 363)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = resources.GetString("TextBox1.Text")
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.TextBox2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(747, 369)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Use of this disc image"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox2.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(3, 3)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox2.Size = New System.Drawing.Size(741, 363)
        Me.TextBox2.TabIndex = 1
        Me.TextBox2.Text = resources.GetString("TextBox2.Text")
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.TextBox3)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(747, 369)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Licenses"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TextBox3
        '
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox3.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(0, 0)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox3.Size = New System.Drawing.Size(747, 369)
        Me.TextBox3.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.Location = New System.Drawing.Point(14, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(758, 47)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Please read these notes and agree to them before continuing with OS installation." & _
    " Switch tabs to read specific information and click Next"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(246, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Disclaimers and important notices"
        '
        'SlideshowTimer
        '
        Me.SlideshowTimer.Interval = 10000
        '
        'InstallerBW
        '
        Me.InstallerBW.WorkerReportsProgress = True
        Me.InstallerBW.WorkerSupportsCancellation = True
        '
        'BCDEditProcess
        '
        Me.BCDEditProcess.StartInfo.CreateNoWindow = True
        Me.BCDEditProcess.StartInfo.Domain = ""
        Me.BCDEditProcess.StartInfo.LoadUserProfile = False
        Me.BCDEditProcess.StartInfo.Password = Nothing
        Me.BCDEditProcess.StartInfo.RedirectStandardOutput = True
        Me.BCDEditProcess.StartInfo.StandardErrorEncoding = Nothing
        Me.BCDEditProcess.StartInfo.StandardOutputEncoding = Nothing
        Me.BCDEditProcess.StartInfo.UserName = ""
        Me.BCDEditProcess.StartInfo.UseShellExecute = False
        Me.BCDEditProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
        Me.BCDEditProcess.SynchronizingObject = Me
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'ExportDrvsFBD
        '
        Me.ExportDrvsFBD.Description = "Specify the path to export drivers to:"
        Me.ExportDrvsFBD.RootFolder = System.Environment.SpecialFolder.MyComputer
        '
        'ExportDrvsBW
        '
        Me.ExportDrvsBW.WorkerReportsProgress = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.PageContainerPanel)
        Me.Controls.Add(Me.ButtonContainerPanel)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "MainForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ButtonContainerPanel.ResumeLayout(False)
        Me.PageContainerPanel.ResumeLayout(False)
        Me.ErrorPanel.ResumeLayout(False)
        Me.ErrorPanel.PerformLayout()
        Me.FinishPanel.ResumeLayout(False)
        Me.FinishPanel.PerformLayout()
        Me.InstallationPanel.ResumeLayout(False)
        Me.InstallationPanel.PerformLayout()
        Me.StepPanelContainer.ResumeLayout(False)
        Me.BootMgrStep.ResumeLayout(False)
        Me.BootMgrStep.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DTPEStep.ResumeLayout(False)
        Me.DTPEStep.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WindowsStep.ResumeLayout(False)
        Me.WindowsStep.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ExplanationPanel.ResumeLayout(False)
        Me.ExplanationPanel.PerformLayout()
        Me.ImageInfoPanel.ResumeLayout(False)
        Me.ImageInfoPanel.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.DisclaimerPanel.ResumeLayout(False)
        Me.DisclaimerPanel.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents BackButton As System.Windows.Forms.Button
    Friend WithEvents NextButton As System.Windows.Forms.Button
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents PageContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents FinishPanel As System.Windows.Forms.Panel
    Friend WithEvents InstallationPanel As System.Windows.Forms.Panel
    Friend WithEvents ExplanationPanel As System.Windows.Forms.Panel
    Friend WithEvents ImageInfoPanel As System.Windows.Forms.Panel
    Friend WithEvents DisclaimerPanel As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents StepPanelContainer As System.Windows.Forms.Panel
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents BootMgrStep As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents DTPEStep As System.Windows.Forms.Panel
    Friend WithEvents WindowsStep As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents SlideshowTimer As System.Windows.Forms.Timer
    Friend WithEvents InstallerBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents RestartButton As System.Windows.Forms.Button
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents BCDEditProcess As System.Diagnostics.Process
    Friend WithEvents Label34 As Label
    Friend WithEvents ProgressBar2 As ProgressBar
    Friend WithEvents Label35 As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents ErrorPanel As System.Windows.Forms.Panel
    Friend WithEvents ErrorTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents ExportDrvsBtn As System.Windows.Forms.Button
    Friend WithEvents ExportDrvsFBD As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ExportDrvsBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents GetImgInfoBtn As System.Windows.Forms.Button
End Class
