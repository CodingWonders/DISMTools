<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GetAppxPkgInfoDlg
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
        Me.FeatureInfoPanel = New System.Windows.Forms.Panel()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.SearchPanel = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SearchBox1 = New DISMTools.SearchBox()
        Me.SearchPic = New System.Windows.Forms.PictureBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ImageTaskHeader1 = New DISMTools.ImageTaskHeader()
        Me.FeatureInfoPanel.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SearchPanel.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.SearchPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.FlowLayoutPanel3.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.SuspendLayout()
        '
        'FeatureInfoPanel
        '
        Me.FeatureInfoPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FeatureInfoPanel.Controls.Add(Me.SplitContainer2)
        Me.FeatureInfoPanel.Location = New System.Drawing.Point(64, 97)
        Me.FeatureInfoPanel.Name = "FeatureInfoPanel"
        Me.FeatureInfoPanel.Size = New System.Drawing.Size(880, 396)
        Me.FeatureInfoPanel.TabIndex = 11
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.IsSplitterFixed = True
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.Panel2)
        Me.SplitContainer2.Panel1.Controls.Add(Me.SearchPanel)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.Panel3)
        Me.SplitContainer2.Panel2.Controls.Add(Me.FlowLayoutPanel4)
        Me.SplitContainer2.Size = New System.Drawing.Size(880, 396)
        Me.SplitContainer2.SplitterDistance = 440
        Me.SplitContainer2.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ListBox1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(440, 372)
        Me.Panel2.TabIndex = 1
        '
        'ListBox1
        '
        Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(0, 0)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(440, 372)
        Me.ListBox1.TabIndex = 0
        '
        'SearchPanel
        '
        Me.SearchPanel.Controls.Add(Me.Panel1)
        Me.SearchPanel.Controls.Add(Me.SearchPic)
        Me.SearchPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SearchPanel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SearchPanel.Location = New System.Drawing.Point(0, 372)
        Me.SearchPanel.Name = "SearchPanel"
        Me.SearchPanel.Size = New System.Drawing.Size(440, 24)
        Me.SearchPanel.TabIndex = 7
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.SearchBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(24, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(416, 24)
        Me.Panel1.TabIndex = 3
        '
        'SearchBox1
        '
        Me.SearchBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SearchBox1.cueBanner = "Type here to search for an application..."
        Me.SearchBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SearchBox1.Location = New System.Drawing.Point(8, 3)
        Me.SearchBox1.Name = "SearchBox1"
        Me.SearchBox1.Size = New System.Drawing.Size(405, 18)
        Me.SearchBox1.TabIndex = 1
        '
        'SearchPic
        '
        Me.SearchPic.Dock = System.Windows.Forms.DockStyle.Left
        Me.SearchPic.Image = Global.DISMTools.My.Resources.Resources.search_light
        Me.SearchPic.Location = New System.Drawing.Point(0, 0)
        Me.SearchPic.Name = "SearchPic"
        Me.SearchPic.Size = New System.Drawing.Size(24, 24)
        Me.SearchPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.SearchPic.TabIndex = 2
        Me.SearchPic.TabStop = False
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Panel4)
        Me.Panel3.Controls.Add(Me.Panel7)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(436, 396)
        Me.Panel3.TabIndex = 1
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.FlowLayoutPanel3)
        Me.Panel4.Controls.Add(Me.Panel5)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(436, 396)
        Me.Panel4.TabIndex = 2
        Me.Panel4.Visible = False
        '
        'FlowLayoutPanel3
        '
        Me.FlowLayoutPanel3.AutoScroll = True
        Me.FlowLayoutPanel3.Controls.Add(Me.Label22)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label23)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label24)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label25)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label26)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label35)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label31)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label32)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label41)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label40)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label43)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label42)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label4)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label3)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label6)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label5)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label8)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label7)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label9)
        Me.FlowLayoutPanel3.Controls.Add(Me.PictureBox2)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label10)
        Me.FlowLayoutPanel3.Controls.Add(Me.LinkLabel1)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label55)
        Me.FlowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel3.Location = New System.Drawing.Point(0, 36)
        Me.FlowLayoutPanel3.Name = "FlowLayoutPanel3"
        Me.FlowLayoutPanel3.Padding = New System.Windows.Forms.Padding(4, 6, 0, 0)
        Me.FlowLayoutPanel3.Size = New System.Drawing.Size(436, 360)
        Me.FlowLayoutPanel3.TabIndex = 1
        Me.FlowLayoutPanel3.WrapContents = False
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(7, 6)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(80, 13)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = LocalizationService.ForSection("Designer.Get.AppX")("PackageName.Label")
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(7, 19)
        Me.Label23.Name = "Label23"
        Me.Label23.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label23.Size = New System.Drawing.Size(38, 15)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label23.UseMnemonic = False
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(7, 34)
        Me.Label24.Name = "Label24"
        Me.Label24.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label24.Size = New System.Drawing.Size(128, 17)
        Me.Label24.TabIndex = 0
        Me.Label24.Text = LocalizationService.ForSection("Designer.Get.AppX")("Display.Name.Label")
        '
        'Label25
        '
        Me.Label25.AutoEllipsis = True
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(7, 51)
        Me.Label25.Name = "Label25"
        Me.Label25.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label25.Size = New System.Drawing.Size(38, 15)
        Me.Label25.TabIndex = 0
        Me.Label25.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label25.UseMnemonic = False
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(7, 66)
        Me.Label26.Name = "Label26"
        Me.Label26.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label26.Size = New System.Drawing.Size(70, 17)
        Me.Label26.TabIndex = 0
        Me.Label26.Text = LocalizationService.ForSection("Designer.Get.AppX")("Architecture.Label")
        '
        'Label35
        '
        Me.Label35.AutoEllipsis = True
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(7, 83)
        Me.Label35.Name = "Label35"
        Me.Label35.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label35.Size = New System.Drawing.Size(38, 15)
        Me.Label35.TabIndex = 0
        Me.Label35.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label35.UseMnemonic = False
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(7, 98)
        Me.Label31.Name = "Label31"
        Me.Label31.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label31.Size = New System.Drawing.Size(70, 17)
        Me.Label31.TabIndex = 0
        Me.Label31.Text = LocalizationService.ForSection("Designer.Get.AppX")("ResourceID.Label")
        '
        'Label32
        '
        Me.Label32.AutoEllipsis = True
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(7, 115)
        Me.Label32.Name = "Label32"
        Me.Label32.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label32.Size = New System.Drawing.Size(38, 15)
        Me.Label32.TabIndex = 0
        Me.Label32.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label32.UseMnemonic = False
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(7, 130)
        Me.Label41.Name = "Label41"
        Me.Label41.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label41.Size = New System.Drawing.Size(46, 17)
        Me.Label41.TabIndex = 0
        Me.Label41.Text = LocalizationService.ForSection("Designer.Get.AppX")("Version.Label")
        '
        'Label40
        '
        Me.Label40.AutoEllipsis = True
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(7, 147)
        Me.Label40.Name = "Label40"
        Me.Label40.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label40.Size = New System.Drawing.Size(38, 15)
        Me.Label40.TabIndex = 0
        Me.Label40.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label40.UseMnemonic = False
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(7, 162)
        Me.Label43.Name = "Label43"
        Me.Label43.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label43.Size = New System.Drawing.Size(131, 17)
        Me.Label43.TabIndex = 0
        Me.Label43.Text = LocalizationService.ForSection("Designer.Get.AppX")("Registered.User.Label")
        '
        'Label42
        '
        Me.Label42.AutoEllipsis = True
        Me.Label42.AutoSize = True
        Me.Label42.Location = New System.Drawing.Point(7, 179)
        Me.Label42.Name = "Label42"
        Me.Label42.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label42.Size = New System.Drawing.Size(38, 15)
        Me.Label42.TabIndex = 0
        Me.Label42.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label42.UseMnemonic = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 194)
        Me.Label4.Name = "Label4"
        Me.Label4.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label4.Size = New System.Drawing.Size(110, 17)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = LocalizationService.ForSection("Designer.Get.AppX")("Install.Dir.Label")
        '
        'Label3
        '
        Me.Label3.AutoEllipsis = True
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 211)
        Me.Label3.Name = "Label3"
        Me.Label3.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label3.Size = New System.Drawing.Size(38, 15)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label3.UseMnemonic = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 226)
        Me.Label6.Name = "Label6"
        Me.Label6.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label6.Size = New System.Drawing.Size(135, 17)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = LocalizationService.ForSection("Designer.Get.AppX")("Package.Manifest.Label")
        '
        'Label5
        '
        Me.Label5.AutoEllipsis = True
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 243)
        Me.Label5.Name = "Label5"
        Me.Label5.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label5.Size = New System.Drawing.Size(38, 15)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label5.UseMnemonic = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 258)
        Me.Label8.Name = "Label8"
        Me.Label8.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label8.Size = New System.Drawing.Size(135, 17)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = LocalizationService.ForSection("Designer.Get.AppX")("StoreLogo.Asset.Dir.Label")
        '
        'Label7
        '
        Me.Label7.AutoEllipsis = True
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 275)
        Me.Label7.Name = "Label7"
        Me.Label7.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label7.Size = New System.Drawing.Size(38, 15)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label7.UseMnemonic = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 290)
        Me.Label9.Name = "Label9"
        Me.Label9.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label9.Size = New System.Drawing.Size(113, 17)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = LocalizationService.ForSection("Designer.Get.AppX")("Main.StoreLogo.Asset.Label")
        '
        'PictureBox2
        '
        Me.PictureBox2.Location = New System.Drawing.Point(7, 310)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(405, 150)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox2.TabIndex = 2
        Me.PictureBox2.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(7, 463)
        Me.Label10.Name = "Label10"
        Me.Label10.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label10.Size = New System.Drawing.Size(403, 30)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = LocalizationService.ForSection("Designer.Get.AppX")("Asset.Guessed.DISM.Message")
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel1.Location = New System.Drawing.Point(7, 493)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.LinkLabel1.Size = New System.Drawing.Size(194, 15)
        Me.LinkLabel1.TabIndex = 3
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.Get.AppX")("Asset.One.IM.Link")
        '
        'Label55
        '
        Me.Label55.AutoEllipsis = True
        Me.Label55.Location = New System.Drawing.Point(7, 508)
        Me.Label55.Name = "Label55"
        Me.Label55.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label55.Size = New System.Drawing.Size(405, 16)
        Me.Label55.TabIndex = 1
        Me.Label55.UseMnemonic = False
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.Label36)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(436, 36)
        Me.Panel5.TabIndex = 0
        '
        'Label36
        '
        Me.Label36.AutoEllipsis = True
        Me.Label36.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label36.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.Location = New System.Drawing.Point(0, 0)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(436, 36)
        Me.Label36.TabIndex = 0
        Me.Label36.Text = LocalizationService.ForSection("Designer.Get.AppX")("AppX.Package.Label")
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.Label37)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel7.Location = New System.Drawing.Point(0, 0)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(436, 396)
        Me.Panel7.TabIndex = 1
        '
        'Label37
        '
        Me.Label37.AutoEllipsis = True
        Me.Label37.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label37.Location = New System.Drawing.Point(0, 0)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(436, 396)
        Me.Label37.TabIndex = 0
        Me.Label37.Text = LocalizationService.ForSection("Designer.Get.AppX")("Installed.AppX.Label")
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FlowLayoutPanel4
        '
        Me.FlowLayoutPanel4.Location = New System.Drawing.Point(106, 163)
        Me.FlowLayoutPanel4.Name = "FlowLayoutPanel4"
        Me.FlowLayoutPanel4.Size = New System.Drawing.Size(200, 100)
        Me.FlowLayoutPanel4.TabIndex = 0
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button2.Location = New System.Drawing.Point(848, 499)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(96, 23)
        Me.Button2.TabIndex = 12
        Me.Button2.Text = LocalizationService.ForSection("Designer.Get.AppX")("Save.Button")
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ImageTaskHeader1
        '
        Me.ImageTaskHeader1.BackColor = System.Drawing.Color.White
        Me.ImageTaskHeader1.Dock = System.Windows.Forms.DockStyle.Top
        Me.ImageTaskHeader1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ImageTaskHeader1.ItemPicture = Global.DISMTools.My.Resources.Resources.get_appxpkg_info
        Me.ImageTaskHeader1.ItemText = "Get AppX package information"
        Me.ImageTaskHeader1.Location = New System.Drawing.Point(0, 0)
        Me.ImageTaskHeader1.MaximumSize = New System.Drawing.Size(19200, 48)
        Me.ImageTaskHeader1.MinimumSize = New System.Drawing.Size(400, 48)
        Me.ImageTaskHeader1.Name = "ImageTaskHeader1"
        Me.ImageTaskHeader1.Size = New System.Drawing.Size(1008, 48)
        Me.ImageTaskHeader1.TabIndex = 13
        '
        'GetAppxPkgInfoDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1008, 561)
        Me.Controls.Add(Me.ImageTaskHeader1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.FeatureInfoPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "GetAppxPkgInfoDlg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = LocalizationService.ForSection("Designer.Get.AppX")("AppX.Package.Get.Label")
        Me.FeatureInfoPanel.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.SearchPanel.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.SearchPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.FlowLayoutPanel3.ResumeLayout(False)
        Me.FlowLayoutPanel3.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FeatureInfoPanel As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents FlowLayoutPanel3 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel4 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents SearchPanel As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents SearchBox1 As DISMTools.SearchBox
    Friend WithEvents SearchPic As System.Windows.Forms.PictureBox
    Friend WithEvents ImageTaskHeader1 As DISMTools.ImageTaskHeader

End Class
