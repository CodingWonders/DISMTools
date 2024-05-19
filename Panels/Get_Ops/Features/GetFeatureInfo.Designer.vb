<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GetFeatureInfoDlg
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
        Me.Win10Title = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FeatureInfoPanel = New System.Windows.Forms.Panel()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SearchPanel = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
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
        Me.CPropViewer = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.cPropPathView = New System.Windows.Forms.TreeView()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.cPropValue = New System.Windows.Forms.TextBox()
        Me.cPropName = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SearchBox1 = New DISMTools.SearchBox()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Win10Title.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.CPropViewer.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.SuspendLayout()
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
        Me.Win10Title.TabIndex = 4
        Me.Win10Title.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = Global.DISMTools.My.Resources.Resources.get_feat_info
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
        Me.Label1.Size = New System.Drawing.Size(231, 30)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Get feature information"
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
        Me.FeatureInfoPanel.TabIndex = 5
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
        Me.Panel2.Controls.Add(Me.ListView1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(440, 372)
        Me.Panel2.TabIndex = 1
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        Me.ListView1.Location = New System.Drawing.Point(0, 0)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(440, 372)
        Me.ListView1.TabIndex = 5
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Feature name"
        Me.ColumnHeader1.Width = 298
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Feature state"
        Me.ColumnHeader2.Width = 118
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
        Me.SearchPanel.TabIndex = 6
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
        Me.FlowLayoutPanel3.Controls.Add(Me.CPropViewer)
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
        Me.Label22.Size = New System.Drawing.Size(78, 13)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = "Feature name:"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(7, 19)
        Me.Label23.Name = "Label23"
        Me.Label23.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label23.Size = New System.Drawing.Size(38, 15)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = "Label8"
        Me.Label23.UseMnemonic = False
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(7, 34)
        Me.Label24.Name = "Label24"
        Me.Label24.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label24.Size = New System.Drawing.Size(74, 17)
        Me.Label24.TabIndex = 0
        Me.Label24.Text = "Display name:"
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
        Me.Label25.Text = "Label8"
        Me.Label25.UseMnemonic = False
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(7, 66)
        Me.Label26.Name = "Label26"
        Me.Label26.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label26.Size = New System.Drawing.Size(104, 17)
        Me.Label26.TabIndex = 0
        Me.Label26.Text = "Feature description:"
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
        Me.Label35.Text = "Label8"
        Me.Label35.UseMnemonic = False
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(7, 98)
        Me.Label31.Name = "Label31"
        Me.Label31.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label31.Size = New System.Drawing.Size(109, 17)
        Me.Label31.TabIndex = 0
        Me.Label31.Text = "Is a restart required?"
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
        Me.Label32.Text = "Label8"
        Me.Label32.UseMnemonic = False
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(7, 130)
        Me.Label41.Name = "Label41"
        Me.Label41.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label41.Size = New System.Drawing.Size(77, 17)
        Me.Label41.TabIndex = 0
        Me.Label41.Text = "Feature state:"
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
        Me.Label40.Text = "Label8"
        Me.Label40.UseMnemonic = False
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(7, 162)
        Me.Label43.Name = "Label43"
        Me.Label43.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.Label43.Size = New System.Drawing.Size(99, 17)
        Me.Label43.TabIndex = 0
        Me.Label43.Text = "Custom properties:"
        '
        'CPropViewer
        '
        Me.CPropViewer.Controls.Add(Me.TableLayoutPanel1)
        Me.CPropViewer.Location = New System.Drawing.Point(7, 182)
        Me.CPropViewer.Name = "CPropViewer"
        Me.CPropViewer.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.CPropViewer.Size = New System.Drawing.Size(405, 337)
        Me.CPropViewer.TabIndex = 2
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.cPropPathView, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel6, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 2)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(405, 335)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'cPropPathView
        '
        Me.cPropPathView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.cPropPathView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cPropPathView.Location = New System.Drawing.Point(3, 3)
        Me.cPropPathView.Name = "cPropPathView"
        Me.cPropPathView.Size = New System.Drawing.Size(399, 161)
        Me.cPropPathView.TabIndex = 0
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.cPropValue)
        Me.Panel6.Controls.Add(Me.cPropName)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel6.Location = New System.Drawing.Point(3, 170)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(399, 162)
        Me.Panel6.TabIndex = 1
        '
        'cPropValue
        '
        Me.cPropValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cPropValue.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.cPropValue.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cPropValue.Location = New System.Drawing.Point(1, 24)
        Me.cPropValue.Multiline = True
        Me.cPropValue.Name = "cPropValue"
        Me.cPropValue.ReadOnly = True
        Me.cPropValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.cPropValue.Size = New System.Drawing.Size(395, 135)
        Me.cPropValue.TabIndex = 1
        '
        'cPropName
        '
        Me.cPropName.AutoEllipsis = True
        Me.cPropName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cPropName.Location = New System.Drawing.Point(4, 4)
        Me.cPropName.Name = "cPropName"
        Me.cPropName.Size = New System.Drawing.Size(392, 15)
        Me.cPropName.TabIndex = 0
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
        Me.Label36.Text = "Feature information"
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
        Me.Label37.Text = "Select an installed feature on the left to view its information here"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FlowLayoutPanel4
        '
        Me.FlowLayoutPanel4.Location = New System.Drawing.Point(106, 163)
        Me.FlowLayoutPanel4.Name = "FlowLayoutPanel4"
        Me.FlowLayoutPanel4.Size = New System.Drawing.Size(200, 100)
        Me.FlowLayoutPanel4.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 512)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Ready"
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button2.Location = New System.Drawing.Point(848, 499)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(96, 23)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "Save..."
        Me.Button2.UseVisualStyleBackColor = True
        '
        'SearchBox1
        '
        Me.SearchBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SearchBox1.cueBanner = "Type here to search for a feature..."
        Me.SearchBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SearchBox1.Location = New System.Drawing.Point(8, 3)
        Me.SearchBox1.Name = "SearchBox1"
        Me.SearchBox1.Size = New System.Drawing.Size(405, 18)
        Me.SearchBox1.TabIndex = 1
        '
        'Label55
        '
        Me.Label55.AutoEllipsis = True
        Me.Label55.Location = New System.Drawing.Point(7, 522)
        Me.Label55.Name = "Label55"
        Me.Label55.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.Label55.Size = New System.Drawing.Size(405, 6)
        Me.Label55.TabIndex = 1
        Me.Label55.UseMnemonic = False
        '
        'GetFeatureInfoDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 561)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.FeatureInfoPanel)
        Me.Controls.Add(Me.Win10Title)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "GetFeatureInfoDlg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Get feature information"
        Me.Win10Title.ResumeLayout(False)
        Me.Win10Title.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
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
        Me.CPropViewer.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Win10Title As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FeatureInfoPanel As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
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
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel4 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents SearchPanel As System.Windows.Forms.Panel
    Friend WithEvents SearchPic As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents SearchBox1 As DISMTools.SearchBox
    Friend WithEvents CPropViewer As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cPropPathView As System.Windows.Forms.TreeView
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents cPropValue As System.Windows.Forms.TextBox
    Friend WithEvents cPropName As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label

End Class
