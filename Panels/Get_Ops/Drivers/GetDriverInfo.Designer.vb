<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GetDriverInfo
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Win10Title = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonControlPanel = New System.Windows.Forms.Panel()
        Me.DriverInfoContainerPanel = New System.Windows.Forms.Panel()
        Me.DriverInfoPanel = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.DriverContainerPanel = New System.Windows.Forms.Panel()
        Me.InfoFromDrvPackagesPanel = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.DrvPackagesPanel = New System.Windows.Forms.Panel()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DrvPackageContainerPanel = New System.Windows.Forms.Panel()
        Me.DrvPackageInfoPanel = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.NoDrvPanel = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.InfoFromInstalledDrvsPanel = New System.Windows.Forms.Panel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.MenuPanel = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.InstalledDriverLink = New System.Windows.Forms.LinkLabel()
        Me.DriverFileLink = New System.Windows.Forms.LinkLabel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Win10Title.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ButtonControlPanel.SuspendLayout()
        Me.DriverInfoContainerPanel.SuspendLayout()
        Me.DriverInfoPanel.SuspendLayout()
        Me.DriverContainerPanel.SuspendLayout()
        Me.InfoFromDrvPackagesPanel.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.DrvPackagesPanel.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.DrvPackageContainerPanel.SuspendLayout()
        Me.DrvPackageInfoPanel.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.NoDrvPanel.SuspendLayout()
        Me.MenuPanel.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(850, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'Win10Title
        '
        Me.Win10Title.BackColor = System.Drawing.Color.White
        Me.Win10Title.Controls.Add(Me.PictureBox1)
        Me.Win10Title.Controls.Add(Me.Label1)
        Me.Win10Title.Dock = System.Windows.Forms.DockStyle.Top
        Me.Win10Title.Location = New System.Drawing.Point(0, 0)
        Me.Win10Title.Name = "Win10Title"
        Me.Win10Title.Size = New System.Drawing.Size(1008, 48)
        Me.Win10Title.TabIndex = 7
        Me.Win10Title.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = Global.DISMTools.My.Resources.Resources.get_drv_info
        Me.PictureBox1.Location = New System.Drawing.Point(964, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(217, 30)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Get driver information"
        '
        'ButtonControlPanel
        '
        Me.ButtonControlPanel.Controls.Add(Me.TableLayoutPanel1)
        Me.ButtonControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ButtonControlPanel.Location = New System.Drawing.Point(0, 509)
        Me.ButtonControlPanel.Name = "ButtonControlPanel"
        Me.ButtonControlPanel.Size = New System.Drawing.Size(1008, 52)
        Me.ButtonControlPanel.TabIndex = 8
        Me.ButtonControlPanel.Visible = False
        '
        'DriverInfoContainerPanel
        '
        Me.DriverInfoContainerPanel.Controls.Add(Me.DriverInfoPanel)
        Me.DriverInfoContainerPanel.Controls.Add(Me.MenuPanel)
        Me.DriverInfoContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DriverInfoContainerPanel.Location = New System.Drawing.Point(0, 48)
        Me.DriverInfoContainerPanel.Name = "DriverInfoContainerPanel"
        Me.DriverInfoContainerPanel.Size = New System.Drawing.Size(1008, 461)
        Me.DriverInfoContainerPanel.TabIndex = 9
        '
        'DriverInfoPanel
        '
        Me.DriverInfoPanel.Controls.Add(Me.Label5)
        Me.DriverInfoPanel.Controls.Add(Me.DriverContainerPanel)
        Me.DriverInfoPanel.Controls.Add(Me.LinkLabel1)
        Me.DriverInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DriverInfoPanel.Location = New System.Drawing.Point(0, 0)
        Me.DriverInfoPanel.Name = "DriverInfoPanel"
        Me.DriverInfoPanel.Size = New System.Drawing.Size(1008, 461)
        Me.DriverInfoPanel.TabIndex = 3
        Me.DriverInfoPanel.Visible = False
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(20, 408)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(38, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Status"
        '
        'DriverContainerPanel
        '
        Me.DriverContainerPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DriverContainerPanel.Controls.Add(Me.InfoFromDrvPackagesPanel)
        Me.DriverContainerPanel.Controls.Add(Me.InfoFromInstalledDrvsPanel)
        Me.DriverContainerPanel.Location = New System.Drawing.Point(64, 68)
        Me.DriverContainerPanel.Name = "DriverContainerPanel"
        Me.DriverContainerPanel.Size = New System.Drawing.Size(880, 324)
        Me.DriverContainerPanel.TabIndex = 3
        '
        'InfoFromDrvPackagesPanel
        '
        Me.InfoFromDrvPackagesPanel.Controls.Add(Me.SplitContainer1)
        Me.InfoFromDrvPackagesPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InfoFromDrvPackagesPanel.Location = New System.Drawing.Point(0, 0)
        Me.InfoFromDrvPackagesPanel.Name = "InfoFromDrvPackagesPanel"
        Me.InfoFromDrvPackagesPanel.Size = New System.Drawing.Size(880, 324)
        Me.InfoFromDrvPackagesPanel.TabIndex = 1
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.DrvPackagesPanel)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.DrvPackageContainerPanel)
        Me.SplitContainer1.Panel2.Controls.Add(Me.FlowLayoutPanel1)
        Me.SplitContainer1.Size = New System.Drawing.Size(880, 324)
        Me.SplitContainer1.SplitterDistance = 440
        Me.SplitContainer1.TabIndex = 0
        '
        'DrvPackagesPanel
        '
        Me.DrvPackagesPanel.Controls.Add(Me.ListBox1)
        Me.DrvPackagesPanel.Controls.Add(Me.TableLayoutPanel2)
        Me.DrvPackagesPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DrvPackagesPanel.Location = New System.Drawing.Point(0, 0)
        Me.DrvPackagesPanel.Name = "DrvPackagesPanel"
        Me.DrvPackagesPanel.Size = New System.Drawing.Size(440, 324)
        Me.DrvPackagesPanel.TabIndex = 1
        '
        'ListBox1
        '
        Me.ListBox1.AllowDrop = True
        Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(0, 0)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(440, 296)
        Me.ListBox1.TabIndex = 0
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 3
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel2.Controls.Add(Me.Button3, 2, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Button2, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Button1, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 296)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(440, 28)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'Button3
        '
        Me.Button3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button3.Location = New System.Drawing.Point(295, 3)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(142, 22)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Remove all"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button2.Location = New System.Drawing.Point(149, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(140, 22)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Remove selected"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(3, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(140, 22)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Add driver..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DrvPackageContainerPanel
        '
        Me.DrvPackageContainerPanel.Controls.Add(Me.DrvPackageInfoPanel)
        Me.DrvPackageContainerPanel.Controls.Add(Me.NoDrvPanel)
        Me.DrvPackageContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DrvPackageContainerPanel.Location = New System.Drawing.Point(0, 0)
        Me.DrvPackageContainerPanel.Name = "DrvPackageContainerPanel"
        Me.DrvPackageContainerPanel.Size = New System.Drawing.Size(436, 324)
        Me.DrvPackageContainerPanel.TabIndex = 1
        '
        'DrvPackageInfoPanel
        '
        Me.DrvPackageInfoPanel.Controls.Add(Me.FlowLayoutPanel2)
        Me.DrvPackageInfoPanel.Controls.Add(Me.Panel1)
        Me.DrvPackageInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DrvPackageInfoPanel.Location = New System.Drawing.Point(0, 0)
        Me.DrvPackageInfoPanel.Name = "DrvPackageInfoPanel"
        Me.DrvPackageInfoPanel.Size = New System.Drawing.Size(436, 324)
        Me.DrvPackageInfoPanel.TabIndex = 2
        Me.DrvPackageInfoPanel.Visible = False
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.AutoScroll = True
        Me.FlowLayoutPanel2.Controls.Add(Me.Label8)
        Me.FlowLayoutPanel2.Controls.Add(Me.Label9)
        Me.FlowLayoutPanel2.Controls.Add(Me.Label10)
        Me.FlowLayoutPanel2.Controls.Add(Me.Label11)
        Me.FlowLayoutPanel2.Controls.Add(Me.Label12)
        Me.FlowLayoutPanel2.Controls.Add(Me.Label13)
        Me.FlowLayoutPanel2.Controls.Add(Me.Label14)
        Me.FlowLayoutPanel2.Controls.Add(Me.Label16)
        Me.FlowLayoutPanel2.Controls.Add(Me.Label15)
        Me.FlowLayoutPanel2.Controls.Add(Me.Label17)
        Me.FlowLayoutPanel2.Controls.Add(Me.Label18)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(0, 36)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Padding = New System.Windows.Forms.Padding(4, 6, 0, 0)
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(436, 288)
        Me.FlowLayoutPanel2.TabIndex = 1
        Me.FlowLayoutPanel2.WrapContents = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 6)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(113, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Hardware description:"
        '
        'Label9
        '
        Me.Label9.AutoEllipsis = True
        Me.Label9.Location = New System.Drawing.Point(7, 19)
        Me.Label9.Name = "Label9"
        Me.Label9.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label9.Size = New System.Drawing.Size(390, 83)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Label8"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(7, 102)
        Me.Label10.Name = "Label10"
        Me.Label10.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label10.Size = New System.Drawing.Size(72, 17)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Hardware ID:"
        '
        'Label11
        '
        Me.Label11.AutoEllipsis = True
        Me.Label11.Location = New System.Drawing.Point(7, 119)
        Me.Label11.Name = "Label11"
        Me.Label11.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label11.Size = New System.Drawing.Size(390, 83)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Label8"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(7, 202)
        Me.Label12.Name = "Label12"
        Me.Label12.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label12.Size = New System.Drawing.Size(77, 17)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Additional IDs:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(7, 219)
        Me.Label13.Name = "Label13"
        Me.Label13.Padding = New System.Windows.Forms.Padding(12, 4, 0, 0)
        Me.Label13.Size = New System.Drawing.Size(95, 17)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Compatible IDs:"
        '
        'Label14
        '
        Me.Label14.AutoEllipsis = True
        Me.Label14.Location = New System.Drawing.Point(7, 236)
        Me.Label14.Name = "Label14"
        Me.Label14.Padding = New System.Windows.Forms.Padding(12, 2, 0, 0)
        Me.Label14.Size = New System.Drawing.Size(390, 83)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Label8"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(7, 319)
        Me.Label16.Name = "Label16"
        Me.Label16.Padding = New System.Windows.Forms.Padding(12, 4, 0, 0)
        Me.Label16.Size = New System.Drawing.Size(79, 17)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "Exclude IDs:"
        '
        'Label15
        '
        Me.Label15.AutoEllipsis = True
        Me.Label15.Location = New System.Drawing.Point(7, 336)
        Me.Label15.Name = "Label15"
        Me.Label15.Padding = New System.Windows.Forms.Padding(12, 2, 0, 0)
        Me.Label15.Size = New System.Drawing.Size(390, 83)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "Label8"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(7, 419)
        Me.Label17.Name = "Label17"
        Me.Label17.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label17.Size = New System.Drawing.Size(126, 17)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Hardware manufacturer:"
        '
        'Label18
        '
        Me.Label18.AutoEllipsis = True
        Me.Label18.Location = New System.Drawing.Point(7, 436)
        Me.Label18.Name = "Label18"
        Me.Label18.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label18.Size = New System.Drawing.Size(390, 52)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "Label8"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button6)
        Me.Panel1.Controls.Add(Me.Button5)
        Me.Panel1.Controls.Add(Me.Button4)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(436, 36)
        Me.Panel1.TabIndex = 0
        '
        'Button5
        '
        Me.Button5.Image = Global.DISMTools.My.Resources.Resources.next_element
        Me.Button5.Location = New System.Drawing.Point(404, 4)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(28, 28)
        Me.Button5.TabIndex = 1
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Image = Global.DISMTools.My.Resources.Resources.prev_element
        Me.Button4.Location = New System.Drawing.Point(4, 4)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(28, 28)
        Me.Button4.TabIndex = 1
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoEllipsis = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(90, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(257, 36)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Hardware targets"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'NoDrvPanel
        '
        Me.NoDrvPanel.Controls.Add(Me.Label6)
        Me.NoDrvPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NoDrvPanel.Location = New System.Drawing.Point(0, 0)
        Me.NoDrvPanel.Name = "NoDrvPanel"
        Me.NoDrvPanel.Size = New System.Drawing.Size(436, 324)
        Me.NoDrvPanel.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoEllipsis = True
        Me.Label6.Location = New System.Drawing.Point(0, 80)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(436, 164)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Add or select a driver package to view its information here"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(106, 163)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(200, 100)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'InfoFromInstalledDrvsPanel
        '
        Me.InfoFromInstalledDrvsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InfoFromInstalledDrvsPanel.Location = New System.Drawing.Point(0, 0)
        Me.InfoFromInstalledDrvsPanel.Name = "InfoFromInstalledDrvsPanel"
        Me.InfoFromInstalledDrvsPanel.Size = New System.Drawing.Size(880, 324)
        Me.InfoFromInstalledDrvsPanel.TabIndex = 0
        Me.InfoFromInstalledDrvsPanel.Visible = False
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel1.Location = New System.Drawing.Point(17, 12)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(60, 13)
        Me.LinkLabel1.TabIndex = 2
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "<- Go back"
        '
        'MenuPanel
        '
        Me.MenuPanel.Controls.Add(Me.Label4)
        Me.MenuPanel.Controls.Add(Me.Label3)
        Me.MenuPanel.Controls.Add(Me.PictureBox3)
        Me.MenuPanel.Controls.Add(Me.PictureBox2)
        Me.MenuPanel.Controls.Add(Me.InstalledDriverLink)
        Me.MenuPanel.Controls.Add(Me.DriverFileLink)
        Me.MenuPanel.Controls.Add(Me.Label2)
        Me.MenuPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MenuPanel.Location = New System.Drawing.Point(0, 0)
        Me.MenuPanel.Name = "MenuPanel"
        Me.MenuPanel.Size = New System.Drawing.Size(1008, 461)
        Me.MenuPanel.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoEllipsis = True
        Me.Label4.Location = New System.Drawing.Point(129, 231)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(791, 83)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Click here to get information about drivers that you want to add to the Windows i" & _
    "mage you're servicing before proceeding with the driver addition process"
        '
        'Label3
        '
        Me.Label3.AutoEllipsis = True
        Me.Label3.Location = New System.Drawing.Point(129, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(791, 83)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Click here to get information about drivers that you've installed or that came wi" & _
    "th the Windows image you're servicing"
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.DISMTools.My.Resources.Resources.info_from_drv_file
        Me.PictureBox3.Location = New System.Drawing.Point(74, 214)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox3.TabIndex = 2
        Me.PictureBox3.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.DISMTools.My.Resources.Resources.drv_info_from_image
        Me.PictureBox2.Location = New System.Drawing.Point(74, 76)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 2
        Me.PictureBox2.TabStop = False
        '
        'InstalledDriverLink
        '
        Me.InstalledDriverLink.AutoSize = True
        Me.InstalledDriverLink.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InstalledDriverLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.InstalledDriverLink.LinkColor = System.Drawing.Color.DodgerBlue
        Me.InstalledDriverLink.Location = New System.Drawing.Point(128, 76)
        Me.InstalledDriverLink.Name = "InstalledDriverLink"
        Me.InstalledDriverLink.Size = New System.Drawing.Size(352, 13)
        Me.InstalledDriverLink.TabIndex = 1
        Me.InstalledDriverLink.TabStop = True
        Me.InstalledDriverLink.Text = "I want to get information about installed drivers in the image"
        '
        'DriverFileLink
        '
        Me.DriverFileLink.AutoSize = True
        Me.DriverFileLink.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DriverFileLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.DriverFileLink.LinkColor = System.Drawing.Color.DodgerBlue
        Me.DriverFileLink.Location = New System.Drawing.Point(128, 214)
        Me.DriverFileLink.Name = "DriverFileLink"
        Me.DriverFileLink.Size = New System.Drawing.Size(248, 13)
        Me.DriverFileLink.TabIndex = 1
        Me.DriverFileLink.TabStop = True
        Me.DriverFileLink.Text = "I want to get information about driver files"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(17, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(221, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "What do you want to get information about?"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "Driver files|*.inf"
        Me.OpenFileDialog1.SupportMultiDottedExtensions = True
        Me.OpenFileDialog1.Title = "Locate driver files"
        '
        'Button6
        '
        Me.Button6.Image = Global.DISMTools.My.Resources.Resources.jumpto
        Me.Button6.Location = New System.Drawing.Point(370, 4)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(28, 28)
        Me.Button6.TabIndex = 1
        Me.Button6.UseVisualStyleBackColor = True
        '
        'GetDriverInfo
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(1008, 561)
        Me.Controls.Add(Me.DriverInfoContainerPanel)
        Me.Controls.Add(Me.Win10Title)
        Me.Controls.Add(Me.ButtonControlPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "GetDriverInfo"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Get driver information"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Win10Title.ResumeLayout(False)
        Me.Win10Title.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ButtonControlPanel.ResumeLayout(False)
        Me.DriverInfoContainerPanel.ResumeLayout(False)
        Me.DriverInfoPanel.ResumeLayout(False)
        Me.DriverInfoPanel.PerformLayout()
        Me.DriverContainerPanel.ResumeLayout(False)
        Me.InfoFromDrvPackagesPanel.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.DrvPackagesPanel.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.DrvPackageContainerPanel.ResumeLayout(False)
        Me.DrvPackageInfoPanel.ResumeLayout(False)
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.FlowLayoutPanel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.NoDrvPanel.ResumeLayout(False)
        Me.MenuPanel.ResumeLayout(False)
        Me.MenuPanel.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Win10Title As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonControlPanel As System.Windows.Forms.Panel
    Friend WithEvents DriverInfoContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents MenuPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents DriverFileLink As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DriverInfoPanel As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents DriverContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents InfoFromDrvPackagesPanel As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents InfoFromInstalledDrvsPanel As System.Windows.Forms.Panel
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents DrvPackageContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents NoDrvPanel As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents DrvPackagesPanel As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents DrvPackageInfoPanel As System.Windows.Forms.Panel
    Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents InstalledDriverLink As System.Windows.Forms.LinkLabel
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button

End Class
