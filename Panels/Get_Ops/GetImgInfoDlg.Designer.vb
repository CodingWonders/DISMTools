<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GetImgInfoDlg
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
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.ImageInfoPanel = New System.Windows.Forms.Panel()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
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
        Me.Label47 = New System.Windows.Forms.Label()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.LanguageList = New System.Windows.Forms.ListBox()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ImageTaskHeader1 = New DISMTools.ImageTaskHeader()
        Me.ImageInfoPanel.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.FlowLayoutPanel3.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.SuspendLayout()
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.Get.Img")("WIM.Files.Wimvirtual.Filter")
        Me.OpenFileDialog1.SupportMultiDottedExtensions = True
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.Get.Img")("Image.Title")
        '
        'ImageInfoPanel
        '
        Me.ImageInfoPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ImageInfoPanel.Controls.Add(Me.SplitContainer2)
        Me.ImageInfoPanel.Location = New System.Drawing.Point(64, 97)
        Me.ImageInfoPanel.Name = "ImageInfoPanel"
        Me.ImageInfoPanel.Size = New System.Drawing.Size(880, 396)
        Me.ImageInfoPanel.TabIndex = 4
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
        Me.Panel2.Controls.Add(Me.ListView1)
        Me.Panel2.Controls.Add(Me.Panel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(440, 396)
        Me.Panel2.TabIndex = 1
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        Me.ListView1.Location = New System.Drawing.Point(0, 148)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(440, 248)
        Me.ListView1.TabIndex = 5
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.Get.Img")("Index.Column")
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageName.Column")
        Me.ColumnHeader2.Width = 344
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button3)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Controls.Add(Me.RadioButton2)
        Me.Panel1.Controls.Add(Me.RadioButton1)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(440, 148)
        Me.Panel1.TabIndex = 4
        '
        'Button3
        '
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button3.Location = New System.Drawing.Point(348, 93)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = LocalizationService.ForSection("Designer.Get.Img")("Pick.Button")
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(267, 93)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = LocalizationService.ForSection("Designer.Get.Img")("Browse.Button")
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(49, 94)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(212, 21)
        Me.TextBox1.TabIndex = 2
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(32, 71)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(95, 17)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.Get.Img")("AnotherImage.RadioButton")
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(32, 48)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(146, 17)
        Me.RadioButton1.TabIndex = 1
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.Get.Img")("CurrentlyMounted.RadioButton")
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 125)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(141, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = LocalizationService.ForSection("Designer.Get.Img")("List.Indexes.ImageFile.Label")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(172, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageFile.Label")
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
        Me.FlowLayoutPanel3.Controls.Add(Me.Label47)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label46)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label33)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label34)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label28)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label27)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label30)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label29)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label39)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label38)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label45)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label4)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label5)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label44)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label7)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label8)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label9)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label6)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label11)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label10)
        Me.FlowLayoutPanel3.Controls.Add(Me.Label13)
        Me.FlowLayoutPanel3.Controls.Add(Me.LanguageList)
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
        Me.Label22.Size = New System.Drawing.Size(79, 13)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageVersion.Label")
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(7, 19)
        Me.Label23.Name = "Label23"
        Me.Label23.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label23.Size = New System.Drawing.Size(38, 15)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label23.UseMnemonic = False
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(7, 34)
        Me.Label24.Name = "Label24"
        Me.Label24.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label24.Size = New System.Drawing.Size(70, 17)
        Me.Label24.TabIndex = 0
        Me.Label24.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageName.Label")
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
        Me.Label25.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label25.UseMnemonic = False
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(7, 66)
        Me.Label26.Name = "Label26"
        Me.Label26.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label26.Size = New System.Drawing.Size(96, 17)
        Me.Label26.TabIndex = 0
        Me.Label26.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageDescription.Label")
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
        Me.Label35.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label35.UseMnemonic = False
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(7, 98)
        Me.Label31.Name = "Label31"
        Me.Label31.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label31.Size = New System.Drawing.Size(62, 17)
        Me.Label31.TabIndex = 0
        Me.Label31.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageSize.Label")
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
        Me.Label32.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label32.UseMnemonic = False
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(7, 130)
        Me.Label41.Name = "Label41"
        Me.Label41.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label41.Size = New System.Drawing.Size(102, 17)
        Me.Label41.TabIndex = 0
        Me.Label41.Text = LocalizationService.ForSection("Designer.Get.Img")("Supports.WIM.Boot.Label")
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
        Me.Label40.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label40.UseMnemonic = False
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(7, 162)
        Me.Label43.Name = "Label43"
        Me.Label43.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label43.Size = New System.Drawing.Size(70, 17)
        Me.Label43.TabIndex = 0
        Me.Label43.Text = LocalizationService.ForSection("Designer.Get.Img")("Architecture.Label")
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
        Me.Label42.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label42.UseMnemonic = False
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Location = New System.Drawing.Point(7, 194)
        Me.Label47.Name = "Label47"
        Me.Label47.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label47.Size = New System.Drawing.Size(30, 17)
        Me.Label47.TabIndex = 0
        Me.Label47.Text = LocalizationService.ForSection("Designer.Get.Img")("HAL.Label")
        '
        'Label46
        '
        Me.Label46.AutoEllipsis = True
        Me.Label46.AutoSize = True
        Me.Label46.Location = New System.Drawing.Point(7, 211)
        Me.Label46.Name = "Label46"
        Me.Label46.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label46.Size = New System.Drawing.Size(38, 15)
        Me.Label46.TabIndex = 0
        Me.Label46.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label46.UseMnemonic = False
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(7, 226)
        Me.Label33.Name = "Label33"
        Me.Label33.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label33.Size = New System.Drawing.Size(96, 17)
        Me.Label33.TabIndex = 0
        Me.Label33.Text = LocalizationService.ForSection("Designer.Get.Img")("ServicePackBuild.Label")
        '
        'Label34
        '
        Me.Label34.AutoEllipsis = True
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(7, 243)
        Me.Label34.Name = "Label34"
        Me.Label34.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label34.Size = New System.Drawing.Size(38, 15)
        Me.Label34.TabIndex = 0
        Me.Label34.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label34.UseMnemonic = False
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(7, 258)
        Me.Label28.Name = "Label28"
        Me.Label28.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label28.Size = New System.Drawing.Size(96, 17)
        Me.Label28.TabIndex = 0
        Me.Label28.Text = LocalizationService.ForSection("Designer.Get.Img")("ServicePackLevel.Label")
        '
        'Label27
        '
        Me.Label27.AutoEllipsis = True
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(7, 275)
        Me.Label27.Name = "Label27"
        Me.Label27.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label27.Size = New System.Drawing.Size(38, 15)
        Me.Label27.TabIndex = 0
        Me.Label27.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label27.UseMnemonic = False
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(7, 290)
        Me.Label30.Name = "Label30"
        Me.Label30.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label30.Size = New System.Drawing.Size(89, 17)
        Me.Label30.TabIndex = 0
        Me.Label30.Text = LocalizationService.ForSection("Designer.Get.Img")("InstallationType.Label")
        '
        'Label29
        '
        Me.Label29.AutoEllipsis = True
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(7, 307)
        Me.Label29.Name = "Label29"
        Me.Label29.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label29.Size = New System.Drawing.Size(38, 15)
        Me.Label29.TabIndex = 0
        Me.Label29.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label29.UseMnemonic = False
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(7, 322)
        Me.Label39.Name = "Label39"
        Me.Label39.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label39.Size = New System.Drawing.Size(43, 17)
        Me.Label39.TabIndex = 0
        Me.Label39.Text = LocalizationService.ForSection("Designer.Get.Img")("Edition.Label")
        '
        'Label38
        '
        Me.Label38.AutoEllipsis = True
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(7, 339)
        Me.Label38.Name = "Label38"
        Me.Label38.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label38.Size = New System.Drawing.Size(38, 15)
        Me.Label38.TabIndex = 0
        Me.Label38.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label38.UseMnemonic = False
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Location = New System.Drawing.Point(7, 354)
        Me.Label45.Name = "Label45"
        Me.Label45.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label45.Size = New System.Drawing.Size(73, 17)
        Me.Label45.TabIndex = 0
        Me.Label45.Text = LocalizationService.ForSection("Designer.Get.Img")("ProductType.Label")
        '
        'Label4
        '
        Me.Label4.AutoEllipsis = True
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 371)
        Me.Label4.Name = "Label4"
        Me.Label4.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label4.Size = New System.Drawing.Size(38, 15)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label4.UseMnemonic = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 386)
        Me.Label5.Name = "Label5"
        Me.Label5.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label5.Size = New System.Drawing.Size(74, 17)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = LocalizationService.ForSection("Designer.Get.Img")("ProductSuite.Label")
        '
        'Label44
        '
        Me.Label44.AutoEllipsis = True
        Me.Label44.AutoSize = True
        Me.Label44.Location = New System.Drawing.Point(7, 403)
        Me.Label44.Name = "Label44"
        Me.Label44.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label44.Size = New System.Drawing.Size(38, 15)
        Me.Label44.TabIndex = 0
        Me.Label44.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label44.UseMnemonic = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 418)
        Me.Label7.Name = "Label7"
        Me.Label7.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label7.Size = New System.Drawing.Size(115, 17)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = LocalizationService.ForSection("Designer.Get.Img")("System.Root.Dir.Label")
        '
        'Label8
        '
        Me.Label8.AutoEllipsis = True
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 435)
        Me.Label8.Name = "Label8"
        Me.Label8.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label8.Size = New System.Drawing.Size(38, 15)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label8.UseMnemonic = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 450)
        Me.Label9.Name = "Label9"
        Me.Label9.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label9.Size = New System.Drawing.Size(57, 17)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = LocalizationService.ForSection("Designer.Get.Img")("FileCount.Label")
        '
        'Label6
        '
        Me.Label6.AutoEllipsis = True
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 467)
        Me.Label6.Name = "Label6"
        Me.Label6.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label6.Size = New System.Drawing.Size(38, 15)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label6.UseMnemonic = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(7, 482)
        Me.Label11.Name = "Label11"
        Me.Label11.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label11.Size = New System.Drawing.Size(39, 17)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = LocalizationService.ForSection("Designer.Get.Img")("Dates.Label")
        '
        'Label10
        '
        Me.Label10.AutoEllipsis = True
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(7, 499)
        Me.Label10.Name = "Label10"
        Me.Label10.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label10.Size = New System.Drawing.Size(38, 15)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label10.UseMnemonic = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(7, 514)
        Me.Label13.Name = "Label13"
        Me.Label13.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label13.Size = New System.Drawing.Size(104, 17)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = LocalizationService.ForSection("Designer.Get.Img")("Installed.Languages.Label")
        '
        'LanguageList
        '
        Me.LanguageList.FormattingEnabled = True
        Me.LanguageList.Location = New System.Drawing.Point(7, 534)
        Me.LanguageList.Name = "LanguageList"
        Me.LanguageList.ScrollAlwaysVisible = True
        Me.LanguageList.Size = New System.Drawing.Size(410, 95)
        Me.LanguageList.TabIndex = 21
        '
        'Label55
        '
        Me.Label55.AutoEllipsis = True
        Me.Label55.Location = New System.Drawing.Point(7, 632)
        Me.Label55.Name = "Label55"
        Me.Label55.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label55.Size = New System.Drawing.Size(405, 16)
        Me.Label55.TabIndex = 22
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
        Me.Label36.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageInfo.Label")
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
        Me.Label37.Text = LocalizationService.ForSection("Designer.Get.Img")("Index.List.View.Label")
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
        Me.Button2.Location = New System.Drawing.Point(829, 499)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(96, 23)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = LocalizationService.ForSection("Designer.Get.Img")("Save.Button")
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ImageTaskHeader1
        '
        Me.ImageTaskHeader1.BackColor = System.Drawing.Color.White
        Me.ImageTaskHeader1.Dock = System.Windows.Forms.DockStyle.Top
        Me.ImageTaskHeader1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ImageTaskHeader1.ItemPicture = Global.DISMTools.My.Resources.Resources.img_info
        Me.ImageTaskHeader1.ItemText = "Get image information"
        Me.ImageTaskHeader1.Location = New System.Drawing.Point(0, 0)
        Me.ImageTaskHeader1.MaximumSize = New System.Drawing.Size(19200, 48)
        Me.ImageTaskHeader1.MinimumSize = New System.Drawing.Size(400, 48)
        Me.ImageTaskHeader1.Name = "ImageTaskHeader1"
        Me.ImageTaskHeader1.Size = New System.Drawing.Size(1008, 48)
        Me.ImageTaskHeader1.TabIndex = 6
        '
        'GetImgInfoDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1008, 561)
        Me.Controls.Add(Me.ImageTaskHeader1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ImageInfoPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "GetImgInfoDlg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = LocalizationService.ForSection("Designer.Get.Img")("Image.Label")
        Me.ImageInfoPanel.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.FlowLayoutPanel3.ResumeLayout(False)
        Me.FlowLayoutPanel3.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ImageInfoPanel As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
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
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel4 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents LanguageList As System.Windows.Forms.ListBox
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents ImageTaskHeader1 As DISMTools.ImageTaskHeader

End Class
