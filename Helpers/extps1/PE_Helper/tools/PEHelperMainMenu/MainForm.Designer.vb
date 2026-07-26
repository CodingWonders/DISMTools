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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
        Me.ExitLink = New System.Windows.Forms.LinkLabel()
        Me.MainMenuPanel = New System.Windows.Forms.Panel()
        Me.LinkLabel6 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel4 = New System.Windows.Forms.LinkLabel()
        Me.PictureBox7 = New System.Windows.Forms.PictureBox()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PxeHelpersMenu = New System.Windows.Forms.Panel()
        Me.Bevel1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LinkLabel5 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel10 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel9 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel7 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel8 = New System.Windows.Forms.LinkLabel()
        Me.PictureBox11 = New System.Windows.Forms.PictureBox()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.PictureBox10 = New System.Windows.Forms.PictureBox()
        Me.PictureBox8 = New System.Windows.Forms.PictureBox()
        Me.PictureBox9 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainMenuPanel.SuspendLayout()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PxeHelpersMenu.SuspendLayout()
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = Global.PEHelperMainMenu.My.Resources.Resources.DT_PE_Helper_Logo
        Me.PictureBox1.Location = New System.Drawing.Point(8, 64)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(768, 64)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoEllipsis = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(96, 192)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(575, 48)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("WhatWant.Label")
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox2.Image = Global.PEHelperMainMenu.My.Resources.Resources.arrow_normal
        Me.PictureBox2.Location = New System.Drawing.Point(12, 9)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(62, 24)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 2
        Me.PictureBox2.TabStop = False
        '
        'LinkLabel1
        '
        Me.LinkLabel1.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel1.Font = New System.Drawing.Font("Segoe UI", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.ForeColor = System.Drawing.Color.Black
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.White
        Me.LinkLabel1.Location = New System.Drawing.Point(82, 8)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(255, 25)
        Me.LinkLabel1.TabIndex = 3
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Install.Operating.Link")
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox3.Image = Global.PEHelperMainMenu.My.Resources.Resources.arrow_normal
        Me.PictureBox3.Location = New System.Drawing.Point(12, 39)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(62, 24)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox3.TabIndex = 2
        Me.PictureBox3.TabStop = False
        '
        'LinkLabel2
        '
        Me.LinkLabel2.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel2.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel2.ForeColor = System.Drawing.Color.Black
        Me.LinkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel2.LinkColor = System.Drawing.Color.White
        Me.LinkLabel2.Location = New System.Drawing.Point(82, 38)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(242, 25)
        Me.LinkLabel2.TabIndex = 3
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Restart.Install.Media.Link")
        '
        'PictureBox4
        '
        Me.PictureBox4.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox4.Image = Global.PEHelperMainMenu.My.Resources.Resources.arrow_normal
        Me.PictureBox4.Location = New System.Drawing.Point(12, 69)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(62, 24)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox4.TabIndex = 2
        Me.PictureBox4.TabStop = False
        '
        'LinkLabel3
        '
        Me.LinkLabel3.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.LinkLabel3.AutoSize = True
        Me.LinkLabel3.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel3.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel3.ForeColor = System.Drawing.Color.Black
        Me.LinkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel3.LinkColor = System.Drawing.Color.White
        Me.LinkLabel3.Location = New System.Drawing.Point(82, 68)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(413, 25)
        Me.LinkLabel3.TabIndex = 3
        Me.LinkLabel3.TabStop = True
        Me.LinkLabel3.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("StartPXE.Link")
        '
        'ExitLink
        '
        Me.ExitLink.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.ExitLink.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitLink.AutoEllipsis = True
        Me.ExitLink.BackColor = System.Drawing.Color.Transparent
        Me.ExitLink.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExitLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.ExitLink.LinkColor = System.Drawing.Color.White
        Me.ExitLink.Location = New System.Drawing.Point(688, 508)
        Me.ExitLink.Name = "ExitLink"
        Me.ExitLink.Size = New System.Drawing.Size(71, 25)
        Me.ExitLink.TabIndex = 3
        Me.ExitLink.TabStop = True
        Me.ExitLink.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Exit.Button")
        Me.ExitLink.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'MainMenuPanel
        '
        Me.MainMenuPanel.BackColor = System.Drawing.Color.Transparent
        Me.MainMenuPanel.Controls.Add(Me.LinkLabel6)
        Me.MainMenuPanel.Controls.Add(Me.LinkLabel4)
        Me.MainMenuPanel.Controls.Add(Me.LinkLabel3)
        Me.MainMenuPanel.Controls.Add(Me.LinkLabel2)
        Me.MainMenuPanel.Controls.Add(Me.PictureBox7)
        Me.MainMenuPanel.Controls.Add(Me.LinkLabel1)
        Me.MainMenuPanel.Controls.Add(Me.PictureBox5)
        Me.MainMenuPanel.Controls.Add(Me.PictureBox4)
        Me.MainMenuPanel.Controls.Add(Me.PictureBox3)
        Me.MainMenuPanel.Controls.Add(Me.PictureBox2)
        Me.MainMenuPanel.ForeColor = System.Drawing.Color.Black
        Me.MainMenuPanel.Location = New System.Drawing.Point(90, 255)
        Me.MainMenuPanel.Name = "MainMenuPanel"
        Me.MainMenuPanel.Size = New System.Drawing.Size(592, 226)
        Me.MainMenuPanel.TabIndex = 4
        '
        'LinkLabel6
        '
        Me.LinkLabel6.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.LinkLabel6.AutoSize = True
        Me.LinkLabel6.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel6.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel6.ForeColor = System.Drawing.Color.Black
        Me.LinkLabel6.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel6.LinkColor = System.Drawing.Color.White
        Me.LinkLabel6.Location = New System.Drawing.Point(82, 128)
        Me.LinkLabel6.Name = "LinkLabel6"
        Me.LinkLabel6.Size = New System.Drawing.Size(237, 25)
        Me.LinkLabel6.TabIndex = 3
        Me.LinkLabel6.TabStop = True
        Me.LinkLabel6.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Explore.Contents.Disc.Link")
        '
        'LinkLabel4
        '
        Me.LinkLabel4.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.LinkLabel4.AutoSize = True
        Me.LinkLabel4.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel4.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel4.ForeColor = System.Drawing.Color.Black
        Me.LinkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel4.LinkColor = System.Drawing.Color.White
        Me.LinkLabel4.Location = New System.Drawing.Point(82, 98)
        Me.LinkLabel4.Name = "LinkLabel4"
        Me.LinkLabel4.Size = New System.Drawing.Size(290, 25)
        Me.LinkLabel4.TabIndex = 3
        Me.LinkLabel4.TabStop = True
        Me.LinkLabel4.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Prepare.System.Image.Link")
        '
        'PictureBox7
        '
        Me.PictureBox7.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox7.Enabled = False
        Me.PictureBox7.Image = Global.PEHelperMainMenu.My.Resources.Resources.arrow_normal
        Me.PictureBox7.Location = New System.Drawing.Point(12, 129)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(62, 24)
        Me.PictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox7.TabIndex = 2
        Me.PictureBox7.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox5.Enabled = False
        Me.PictureBox5.Image = Global.PEHelperMainMenu.My.Resources.Resources.arrow_normal
        Me.PictureBox5.Location = New System.Drawing.Point(12, 99)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(62, 24)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox5.TabIndex = 2
        Me.PictureBox5.TabStop = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoEllipsis = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(12, 507)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(670, 45)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("PE.Helper.Message")
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'PxeHelpersMenu
        '
        Me.PxeHelpersMenu.BackColor = System.Drawing.Color.Transparent
        Me.PxeHelpersMenu.Controls.Add(Me.Bevel1)
        Me.PxeHelpersMenu.Controls.Add(Me.Label3)
        Me.PxeHelpersMenu.Controls.Add(Me.LinkLabel5)
        Me.PxeHelpersMenu.Controls.Add(Me.LinkLabel10)
        Me.PxeHelpersMenu.Controls.Add(Me.LinkLabel9)
        Me.PxeHelpersMenu.Controls.Add(Me.LinkLabel7)
        Me.PxeHelpersMenu.Controls.Add(Me.LinkLabel8)
        Me.PxeHelpersMenu.Controls.Add(Me.PictureBox11)
        Me.PxeHelpersMenu.Controls.Add(Me.PictureBox6)
        Me.PxeHelpersMenu.Controls.Add(Me.PictureBox10)
        Me.PxeHelpersMenu.Controls.Add(Me.PictureBox8)
        Me.PxeHelpersMenu.Controls.Add(Me.PictureBox9)
        Me.PxeHelpersMenu.ForeColor = System.Drawing.Color.Black
        Me.PxeHelpersMenu.Location = New System.Drawing.Point(90, 255)
        Me.PxeHelpersMenu.Name = "PxeHelpersMenu"
        Me.PxeHelpersMenu.Size = New System.Drawing.Size(592, 226)
        Me.PxeHelpersMenu.TabIndex = 4
        Me.PxeHelpersMenu.Visible = False
        '
        'Bevel1
        '
        Me.Bevel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bevel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Bevel1.Location = New System.Drawing.Point(12, 105)
        Me.Bevel1.Name = "Bevel1"
        Me.Bevel1.Size = New System.Drawing.Size(569, 2)
        Me.Bevel1.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(9, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(455, 25)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("StartPXE.Label")
        '
        'LinkLabel5
        '
        Me.LinkLabel5.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.LinkLabel5.AutoSize = True
        Me.LinkLabel5.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel5.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel5.ForeColor = System.Drawing.Color.White
        Me.LinkLabel5.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel5.LinkColor = System.Drawing.Color.White
        Me.LinkLabel5.Location = New System.Drawing.Point(82, 188)
        Me.LinkLabel5.Name = "LinkLabel5"
        Me.LinkLabel5.Size = New System.Drawing.Size(50, 25)
        Me.LinkLabel5.TabIndex = 3
        Me.LinkLabel5.TabStop = True
        Me.LinkLabel5.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Back.Button")
        '
        'LinkLabel10
        '
        Me.LinkLabel10.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.LinkLabel10.AutoSize = True
        Me.LinkLabel10.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel10.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel10.ForeColor = System.Drawing.Color.White
        Me.LinkLabel10.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel10.LinkColor = System.Drawing.Color.White
        Me.LinkLabel10.Location = New System.Drawing.Point(82, 148)
        Me.LinkLabel10.Name = "LinkLabel10"
        Me.LinkLabel10.Size = New System.Drawing.Size(326, 25)
        Me.LinkLabel10.TabIndex = 3
        Me.LinkLabel10.TabStop = True
        Me.LinkLabel10.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Copy.Install.Image.Link")
        '
        'LinkLabel9
        '
        Me.LinkLabel9.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.LinkLabel9.AutoSize = True
        Me.LinkLabel9.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel9.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel9.ForeColor = System.Drawing.Color.White
        Me.LinkLabel9.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel9.LinkColor = System.Drawing.Color.White
        Me.LinkLabel9.Location = New System.Drawing.Point(82, 118)
        Me.LinkLabel9.Name = "LinkLabel9"
        Me.LinkLabel9.Size = New System.Drawing.Size(270, 25)
        Me.LinkLabel9.TabIndex = 3
        Me.LinkLabel9.TabStop = True
        Me.LinkLabel9.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Copy.Boot.Image.Link")
        '
        'LinkLabel7
        '
        Me.LinkLabel7.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.LinkLabel7.AutoSize = True
        Me.LinkLabel7.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel7.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel7.ForeColor = System.Drawing.Color.White
        Me.LinkLabel7.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel7.LinkColor = System.Drawing.Color.White
        Me.LinkLabel7.Location = New System.Drawing.Point(82, 68)
        Me.LinkLabel7.Name = "LinkLabel7"
        Me.LinkLabel7.Size = New System.Drawing.Size(268, 25)
        Me.LinkLabel7.TabIndex = 3
        Me.LinkLabel7.TabStop = True
        Me.LinkLabel7.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("StartPXE.PXEFOG.Link")
        '
        'LinkLabel8
        '
        Me.LinkLabel8.ActiveLinkColor = System.Drawing.Color.LawnGreen
        Me.LinkLabel8.AutoSize = True
        Me.LinkLabel8.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabel8.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel8.ForeColor = System.Drawing.Color.White
        Me.LinkLabel8.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel8.LinkColor = System.Drawing.Color.White
        Me.LinkLabel8.Location = New System.Drawing.Point(82, 38)
        Me.LinkLabel8.Name = "LinkLabel8"
        Me.LinkLabel8.Size = New System.Drawing.Size(481, 25)
        Me.LinkLabel8.TabIndex = 3
        Me.LinkLabel8.TabStop = True
        Me.LinkLabel8.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("StartPXE.PXE.Windows.Link")
        '
        'PictureBox11
        '
        Me.PictureBox11.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox11.Image = Global.PEHelperMainMenu.My.Resources.Resources.arrow_normal
        Me.PictureBox11.Location = New System.Drawing.Point(12, 149)
        Me.PictureBox11.Name = "PictureBox11"
        Me.PictureBox11.Size = New System.Drawing.Size(62, 24)
        Me.PictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox11.TabIndex = 2
        Me.PictureBox11.TabStop = False
        '
        'PictureBox6
        '
        Me.PictureBox6.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox6.Image = Global.PEHelperMainMenu.My.Resources.Resources.arrow_normal_left
        Me.PictureBox6.Location = New System.Drawing.Point(12, 189)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(62, 24)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox6.TabIndex = 2
        Me.PictureBox6.TabStop = False
        '
        'PictureBox10
        '
        Me.PictureBox10.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox10.Image = Global.PEHelperMainMenu.My.Resources.Resources.arrow_normal
        Me.PictureBox10.Location = New System.Drawing.Point(12, 119)
        Me.PictureBox10.Name = "PictureBox10"
        Me.PictureBox10.Size = New System.Drawing.Size(62, 24)
        Me.PictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox10.TabIndex = 2
        Me.PictureBox10.TabStop = False
        '
        'PictureBox8
        '
        Me.PictureBox8.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox8.Image = Global.PEHelperMainMenu.My.Resources.Resources.arrow_normal
        Me.PictureBox8.Location = New System.Drawing.Point(12, 69)
        Me.PictureBox8.Name = "PictureBox8"
        Me.PictureBox8.Size = New System.Drawing.Size(62, 24)
        Me.PictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox8.TabIndex = 2
        Me.PictureBox8.TabStop = False
        '
        'PictureBox9
        '
        Me.PictureBox9.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox9.Image = Global.PEHelperMainMenu.My.Resources.Resources.arrow_normal
        Me.PictureBox9.Location = New System.Drawing.Point(12, 39)
        Me.PictureBox9.Name = "PictureBox9"
        Me.PictureBox9.Size = New System.Drawing.Size(62, 24)
        Me.PictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox9.TabIndex = 2
        Me.PictureBox9.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackgroundImage = Global.PEHelperMainMenu.My.Resources.Resources.background
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ExitLink)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.PxeHelpersMenu)
        Me.Controls.Add(Me.MainMenuPanel)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("DISM.Tools.PE.Label")
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainMenuPanel.ResumeLayout(False)
        Me.MainMenuPanel.PerformLayout()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PxeHelpersMenu.ResumeLayout(False)
        Me.PxeHelpersMenu.PerformLayout()
        CType(Me.PictureBox11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents LinkLabel3 As System.Windows.Forms.LinkLabel
    Friend WithEvents ExitLink As System.Windows.Forms.LinkLabel
    Friend WithEvents MainMenuPanel As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel4 As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents PxeHelpersMenu As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel5 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel7 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel8 As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox8 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox9 As System.Windows.Forms.PictureBox
    Friend WithEvents LinkLabel6 As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
    Friend WithEvents Bevel1 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel9 As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox10 As System.Windows.Forms.PictureBox
    Friend WithEvents LinkLabel10 As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox11 As System.Windows.Forms.PictureBox

End Class
