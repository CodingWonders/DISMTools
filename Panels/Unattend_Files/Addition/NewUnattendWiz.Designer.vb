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
        Me.ExperimentalPanel = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SidePanel.SuspendLayout()
        Me.ExpressModeSteps.SuspendLayout()
        Me.EditorPanelTrigger.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ExpressPanelTrigger.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ExpressPanelContainer.SuspendLayout()
        Me.EditorPanelContainer.SuspendLayout()
        Me.DarkToolStrip1.SuspendLayout()
        Me.HeaderPanel.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FooterContainer.SuspendLayout()
        Me.ExpressPanelFooter.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ExperimentalPanel.SuspendLayout()
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
        Me.StepsTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.StepsTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StepsTreeView.HideSelection = False
        Me.StepsTreeView.ItemHeight = 24
        Me.StepsTreeView.Location = New System.Drawing.Point(0, 0)
        Me.StepsTreeView.Name = "StepsTreeView"
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
        Me.ExpressPanelContainer.Controls.Add(Me.ExperimentalPanel)
        Me.ExpressPanelContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExpressPanelContainer.Location = New System.Drawing.Point(256, 72)
        Me.ExpressPanelContainer.Name = "ExpressPanelContainer"
        Me.ExpressPanelContainer.Size = New System.Drawing.Size(752, 449)
        Me.ExpressPanelContainer.TabIndex = 1
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
        'ExperimentalPanel
        '
        Me.ExperimentalPanel.Controls.Add(Me.Label5)
        Me.ExperimentalPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ExperimentalPanel.Location = New System.Drawing.Point(0, 0)
        Me.ExperimentalPanel.Name = "ExperimentalPanel"
        Me.ExperimentalPanel.Size = New System.Drawing.Size(752, 449)
        Me.ExperimentalPanel.TabIndex = 2
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
        Me.EditorPanelContainer.ResumeLayout(False)
        Me.DarkToolStrip1.ResumeLayout(False)
        Me.DarkToolStrip1.PerformLayout()
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FooterContainer.ResumeLayout(False)
        Me.ExpressPanelFooter.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ExperimentalPanel.ResumeLayout(False)
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
End Class
