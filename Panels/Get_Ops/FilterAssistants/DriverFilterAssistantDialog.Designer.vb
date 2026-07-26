<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DriverFilterAssistantDialog
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.FilterTypeContainerPanel = New System.Windows.Forms.Panel()
        Me.DateFilterPanel = New System.Windows.Forms.Panel()
        Me.DateFilterSuboperatorContainerPanel = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.YearMonthPanel = New System.Windows.Forms.Panel()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.DatePanel = New System.Windows.Forms.Panel()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.ComboBox4 = New System.Windows.Forms.ComboBox()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.SignatureStatusFilterPanel = New System.Windows.Forms.Panel()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.BootCriticalStatusFilterPanel = New System.Windows.Forms.Panel()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.InboxStatusFilterPanel = New System.Windows.Forms.Panel()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ClassNameFilterPanel = New System.Windows.Forms.Panel()
        Me.CNDetailsTLP = New System.Windows.Forms.TableLayoutPanel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.ProviderNameFilterPanel = New System.Windows.Forms.Panel()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.OriginalFileNameFilterPanel = New System.Windows.Forms.Panel()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.PublishedNameFilterPanel = New System.Windows.Forms.Panel()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.NoFilterTypeSelectedPanel = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FilterTypeContainerPanel.SuspendLayout()
        Me.DateFilterPanel.SuspendLayout()
        Me.DateFilterSuboperatorContainerPanel.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.YearMonthPanel.SuspendLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DatePanel.SuspendLayout()
        Me.SignatureStatusFilterPanel.SuspendLayout()
        Me.BootCriticalStatusFilterPanel.SuspendLayout()
        Me.InboxStatusFilterPanel.SuspendLayout()
        Me.ClassNameFilterPanel.SuspendLayout()
        Me.CNDetailsTLP.SuspendLayout()
        Me.ProviderNameFilterPanel.SuspendLayout()
        Me.OriginalFileNameFilterPanel.SuspendLayout()
        Me.PublishedNameFilterPanel.SuspendLayout()
        Me.NoFilterTypeSelectedPanel.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(466, 240)
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
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.DriverFilter")("Apply.Button")
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
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.DriverFilter")("Clear.Button")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(138, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = LocalizationService.ForSection("Designer.DriverFilter")("FilterPrompt.Label")
        '
        'ComboBox1
        '
        Me.ComboBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {LocalizationService.ForSection("Designer.DriverFilter")("PublishedName.Item"), LocalizationService.ForSection("Designer.DriverFilter")("Original.File.Name.Item"), LocalizationService.ForSection("Designer.DriverFilter")("ProviderName.Item"), LocalizationService.ForSection("Designer.DriverFilter")("ClassName.Item"), LocalizationService.ForSection("Designer.DriverFilter")("InboxStatus.Item"), LocalizationService.ForSection("Designer.DriverFilter")("Boot.Critical.Status.Item"), LocalizationService.ForSection("Designer.DriverFilter")("SignatureStatus.Item"), LocalizationService.ForSection("Designer.DriverFilter")("Date.Item")})
        Me.ComboBox1.Location = New System.Drawing.Point(15, 28)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(597, 21)
        Me.ComboBox1.TabIndex = 5
        '
        'FilterTypeContainerPanel
        '
        Me.FilterTypeContainerPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FilterTypeContainerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FilterTypeContainerPanel.Controls.Add(Me.DateFilterPanel)
        Me.FilterTypeContainerPanel.Controls.Add(Me.SignatureStatusFilterPanel)
        Me.FilterTypeContainerPanel.Controls.Add(Me.BootCriticalStatusFilterPanel)
        Me.FilterTypeContainerPanel.Controls.Add(Me.InboxStatusFilterPanel)
        Me.FilterTypeContainerPanel.Controls.Add(Me.ClassNameFilterPanel)
        Me.FilterTypeContainerPanel.Controls.Add(Me.ProviderNameFilterPanel)
        Me.FilterTypeContainerPanel.Controls.Add(Me.OriginalFileNameFilterPanel)
        Me.FilterTypeContainerPanel.Controls.Add(Me.PublishedNameFilterPanel)
        Me.FilterTypeContainerPanel.Controls.Add(Me.NoFilterTypeSelectedPanel)
        Me.FilterTypeContainerPanel.Location = New System.Drawing.Point(15, 56)
        Me.FilterTypeContainerPanel.Name = "FilterTypeContainerPanel"
        Me.FilterTypeContainerPanel.Size = New System.Drawing.Size(597, 181)
        Me.FilterTypeContainerPanel.TabIndex = 6
        '
        'DateFilterPanel
        '
        Me.DateFilterPanel.Controls.Add(Me.DateFilterSuboperatorContainerPanel)
        Me.DateFilterPanel.Controls.Add(Me.ComboBox3)
        Me.DateFilterPanel.Controls.Add(Me.Label12)
        Me.DateFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DateFilterPanel.Location = New System.Drawing.Point(0, 0)
        Me.DateFilterPanel.Name = "DateFilterPanel"
        Me.DateFilterPanel.Size = New System.Drawing.Size(595, 179)
        Me.DateFilterPanel.TabIndex = 8
        Me.DateFilterPanel.Visible = False
        '
        'DateFilterSuboperatorContainerPanel
        '
        Me.DateFilterSuboperatorContainerPanel.Controls.Add(Me.TableLayoutPanel2)
        Me.DateFilterSuboperatorContainerPanel.Controls.Add(Me.ComboBox4)
        Me.DateFilterSuboperatorContainerPanel.Location = New System.Drawing.Point(14, 60)
        Me.DateFilterSuboperatorContainerPanel.Name = "DateFilterSuboperatorContainerPanel"
        Me.DateFilterSuboperatorContainerPanel.Size = New System.Drawing.Size(572, 100)
        Me.DateFilterSuboperatorContainerPanel.TabIndex = 3
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.YearMonthPanel, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.DatePanel, 1, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(12, 40)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(546, 46)
        Me.TableLayoutPanel2.TabIndex = 3
        '
        'YearMonthPanel
        '
        Me.YearMonthPanel.Controls.Add(Me.Label13)
        Me.YearMonthPanel.Controls.Add(Me.NumericUpDown1)
        Me.YearMonthPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.YearMonthPanel.Location = New System.Drawing.Point(3, 3)
        Me.YearMonthPanel.Name = "YearMonthPanel"
        Me.YearMonthPanel.Size = New System.Drawing.Size(267, 40)
        Me.YearMonthPanel.TabIndex = 0
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(135, 13)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(67, 13)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = LocalizationService.ForSection("Designer.DriverFilter")("MonthName.Label")
        Me.Label13.Visible = False
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(13, 10)
        Me.NumericUpDown1.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(115, 21)
        Me.NumericUpDown1.TabIndex = 0
        Me.NumericUpDown1.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'DatePanel
        '
        Me.DatePanel.Controls.Add(Me.DateTimePicker1)
        Me.DatePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DatePanel.Location = New System.Drawing.Point(276, 3)
        Me.DatePanel.Name = "DatePanel"
        Me.DatePanel.Size = New System.Drawing.Size(267, 40)
        Me.DatePanel.TabIndex = 1
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(12, 10)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(241, 21)
        Me.DateTimePicker1.TabIndex = 0
        '
        'ComboBox4
        '
        Me.ComboBox4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox4.FormattingEnabled = True
        Me.ComboBox4.Items.AddRange(New Object() {LocalizationService.ForSection("Designer.DriverFilter")("Year.Item"), LocalizationService.ForSection("Designer.DriverFilter")("Month.Item"), LocalizationService.ForSection("Designer.DriverFilter")("Date.Item")})
        Me.ComboBox4.Location = New System.Drawing.Point(12, 12)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.Size = New System.Drawing.Size(546, 21)
        Me.ComboBox4.TabIndex = 2
        '
        'ComboBox3
        '
        Me.ComboBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Items.AddRange(New Object() {LocalizationService.ForSection("Designer.DriverFilter")("Released.Item"), LocalizationService.ForSection("Designer.DriverFilter")("NotReleased.Item"), LocalizationService.ForSection("Designer.DriverFilter")("ReleasedBefore.Item"), LocalizationService.ForSection("Designer.DriverFilter")("ReleasedOnBefore.Item"), LocalizationService.ForSection("Designer.DriverFilter")("ReleasedAfter.Item"), LocalizationService.ForSection("Designer.DriverFilter")("ReleasedOnAfter.Item")})
        Me.ComboBox3.Location = New System.Drawing.Point(14, 32)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(570, 21)
        Me.ComboBox3.TabIndex = 2
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(12, 12)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(34, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = LocalizationService.ForSection("Designer.DriverFilter")("Date.Label")
        '
        'SignatureStatusFilterPanel
        '
        Me.SignatureStatusFilterPanel.Controls.Add(Me.CheckBox3)
        Me.SignatureStatusFilterPanel.Controls.Add(Me.Label11)
        Me.SignatureStatusFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SignatureStatusFilterPanel.Location = New System.Drawing.Point(0, 0)
        Me.SignatureStatusFilterPanel.Name = "SignatureStatusFilterPanel"
        Me.SignatureStatusFilterPanel.Size = New System.Drawing.Size(595, 179)
        Me.SignatureStatusFilterPanel.TabIndex = 7
        Me.SignatureStatusFilterPanel.Visible = False
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Location = New System.Drawing.Point(24, 36)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(216, 17)
        Me.CheckBox3.TabIndex = 1
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.DriverFilter")("Search.Signed.CheckBox")
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(12, 12)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(91, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = LocalizationService.ForSection("Designer.DriverFilter")("SignatureStatus.Label")
        '
        'BootCriticalStatusFilterPanel
        '
        Me.BootCriticalStatusFilterPanel.Controls.Add(Me.CheckBox2)
        Me.BootCriticalStatusFilterPanel.Controls.Add(Me.Label10)
        Me.BootCriticalStatusFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BootCriticalStatusFilterPanel.Location = New System.Drawing.Point(0, 0)
        Me.BootCriticalStatusFilterPanel.Name = "BootCriticalStatusFilterPanel"
        Me.BootCriticalStatusFilterPanel.Size = New System.Drawing.Size(595, 179)
        Me.BootCriticalStatusFilterPanel.TabIndex = 6
        Me.BootCriticalStatusFilterPanel.Visible = False
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(24, 36)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(312, 17)
        Me.CheckBox2.TabIndex = 1
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.DriverFilter")("Search.BootCritical.CheckBox")
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 12)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(103, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = LocalizationService.ForSection("Designer.DriverFilter")("Boot.Critical.Status.Label")
        '
        'InboxStatusFilterPanel
        '
        Me.InboxStatusFilterPanel.Controls.Add(Me.CheckBox1)
        Me.InboxStatusFilterPanel.Controls.Add(Me.Label9)
        Me.InboxStatusFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InboxStatusFilterPanel.Location = New System.Drawing.Point(0, 0)
        Me.InboxStatusFilterPanel.Name = "InboxStatusFilterPanel"
        Me.InboxStatusFilterPanel.Size = New System.Drawing.Size(595, 179)
        Me.InboxStatusFilterPanel.TabIndex = 5
        Me.InboxStatusFilterPanel.Visible = False
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(24, 36)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(339, 17)
        Me.CheckBox1.TabIndex = 1
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.DriverFilter")("Search.Inbox.CheckBox")
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = LocalizationService.ForSection("Designer.DriverFilter")("InboxStatus.Label")
        '
        'ClassNameFilterPanel
        '
        Me.ClassNameFilterPanel.Controls.Add(Me.CNDetailsTLP)
        Me.ClassNameFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ClassNameFilterPanel.Location = New System.Drawing.Point(0, 0)
        Me.ClassNameFilterPanel.Name = "ClassNameFilterPanel"
        Me.ClassNameFilterPanel.Size = New System.Drawing.Size(595, 179)
        Me.ClassNameFilterPanel.TabIndex = 4
        Me.ClassNameFilterPanel.Visible = False
        '
        'CNDetailsTLP
        '
        Me.CNDetailsTLP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CNDetailsTLP.ColumnCount = 2
        Me.CNDetailsTLP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.9656391!))
        Me.CNDetailsTLP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.0343628!))
        Me.CNDetailsTLP.Controls.Add(Me.Label6, 0, 0)
        Me.CNDetailsTLP.Controls.Add(Me.Label7, 0, 1)
        Me.CNDetailsTLP.Controls.Add(Me.Label8, 1, 1)
        Me.CNDetailsTLP.Controls.Add(Me.ComboBox2, 1, 0)
        Me.CNDetailsTLP.Location = New System.Drawing.Point(12, 12)
        Me.CNDetailsTLP.Name = "CNDetailsTLP"
        Me.CNDetailsTLP.RowCount = 2
        Me.CNDetailsTLP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.57724!))
        Me.CNDetailsTLP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.4227676!))
        Me.CNDetailsTLP.Size = New System.Drawing.Size(568, 123)
        Me.CNDetailsTLP.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label6.Location = New System.Drawing.Point(3, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(124, 29)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = LocalizationService.ForSection("Designer.DriverFilter")("ClassName.Label")
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label7.Location = New System.Drawing.Point(3, 29)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(124, 94)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = LocalizationService.ForSection("Designer.DriverFilter")("Class.Name.Notes.Label")
        '
        'Label8
        '
        Me.Label8.AutoEllipsis = True
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label8.Location = New System.Drawing.Point(133, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(432, 94)
        Me.Label8.TabIndex = 0
        '
        'ComboBox2
        '
        Me.ComboBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(133, 3)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(432, 21)
        Me.ComboBox2.TabIndex = 1
        '
        'ProviderNameFilterPanel
        '
        Me.ProviderNameFilterPanel.Controls.Add(Me.TextBox3)
        Me.ProviderNameFilterPanel.Controls.Add(Me.Label5)
        Me.ProviderNameFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProviderNameFilterPanel.Location = New System.Drawing.Point(0, 0)
        Me.ProviderNameFilterPanel.Name = "ProviderNameFilterPanel"
        Me.ProviderNameFilterPanel.Size = New System.Drawing.Size(595, 179)
        Me.ProviderNameFilterPanel.TabIndex = 3
        Me.ProviderNameFilterPanel.Visible = False
        '
        'TextBox3
        '
        Me.TextBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox3.Location = New System.Drawing.Point(15, 29)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(562, 21)
        Me.TextBox3.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(81, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = LocalizationService.ForSection("Designer.DriverFilter")("ProviderName.Label")
        '
        'OriginalFileNameFilterPanel
        '
        Me.OriginalFileNameFilterPanel.Controls.Add(Me.TextBox2)
        Me.OriginalFileNameFilterPanel.Controls.Add(Me.Label4)
        Me.OriginalFileNameFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OriginalFileNameFilterPanel.Location = New System.Drawing.Point(0, 0)
        Me.OriginalFileNameFilterPanel.Name = "OriginalFileNameFilterPanel"
        Me.OriginalFileNameFilterPanel.Size = New System.Drawing.Size(595, 179)
        Me.OriginalFileNameFilterPanel.TabIndex = 2
        Me.OriginalFileNameFilterPanel.Visible = False
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.Location = New System.Drawing.Point(15, 29)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(562, 21)
        Me.TextBox2.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(96, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = LocalizationService.ForSection("Designer.DriverFilter")("Original.File.Name.Label")
        '
        'PublishedNameFilterPanel
        '
        Me.PublishedNameFilterPanel.Controls.Add(Me.TextBox1)
        Me.PublishedNameFilterPanel.Controls.Add(Me.Label3)
        Me.PublishedNameFilterPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PublishedNameFilterPanel.Location = New System.Drawing.Point(0, 0)
        Me.PublishedNameFilterPanel.Name = "PublishedNameFilterPanel"
        Me.PublishedNameFilterPanel.Size = New System.Drawing.Size(595, 179)
        Me.PublishedNameFilterPanel.TabIndex = 1
        Me.PublishedNameFilterPanel.Visible = False
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(15, 29)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(562, 21)
        Me.TextBox1.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = LocalizationService.ForSection("Designer.DriverFilter")("PublishedName.Label")
        '
        'NoFilterTypeSelectedPanel
        '
        Me.NoFilterTypeSelectedPanel.Controls.Add(Me.Label2)
        Me.NoFilterTypeSelectedPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NoFilterTypeSelectedPanel.Location = New System.Drawing.Point(0, 0)
        Me.NoFilterTypeSelectedPanel.Name = "NoFilterTypeSelectedPanel"
        Me.NoFilterTypeSelectedPanel.Size = New System.Drawing.Size(595, 179)
        Me.NoFilterTypeSelectedPanel.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoEllipsis = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(595, 179)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = LocalizationService.ForSection("Designer.DriverFilter")("Driver.Searches.Choose.Label")
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DriverFilterAssistantDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(624, 281)
        Me.Controls.Add(Me.FilterTypeContainerPanel)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DriverFilterAssistantDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = LocalizationService.ForSection("Designer.DriverFilter")("Title")
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.FilterTypeContainerPanel.ResumeLayout(False)
        Me.DateFilterPanel.ResumeLayout(False)
        Me.DateFilterPanel.PerformLayout()
        Me.DateFilterSuboperatorContainerPanel.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.YearMonthPanel.ResumeLayout(False)
        Me.YearMonthPanel.PerformLayout()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DatePanel.ResumeLayout(False)
        Me.SignatureStatusFilterPanel.ResumeLayout(False)
        Me.SignatureStatusFilterPanel.PerformLayout()
        Me.BootCriticalStatusFilterPanel.ResumeLayout(False)
        Me.BootCriticalStatusFilterPanel.PerformLayout()
        Me.InboxStatusFilterPanel.ResumeLayout(False)
        Me.InboxStatusFilterPanel.PerformLayout()
        Me.ClassNameFilterPanel.ResumeLayout(False)
        Me.CNDetailsTLP.ResumeLayout(False)
        Me.ProviderNameFilterPanel.ResumeLayout(False)
        Me.ProviderNameFilterPanel.PerformLayout()
        Me.OriginalFileNameFilterPanel.ResumeLayout(False)
        Me.OriginalFileNameFilterPanel.PerformLayout()
        Me.PublishedNameFilterPanel.ResumeLayout(False)
        Me.PublishedNameFilterPanel.PerformLayout()
        Me.NoFilterTypeSelectedPanel.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents FilterTypeContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents ClassNameFilterPanel As System.Windows.Forms.Panel
    Friend WithEvents ProviderNameFilterPanel As System.Windows.Forms.Panel
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents OriginalFileNameFilterPanel As System.Windows.Forms.Panel
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PublishedNameFilterPanel As System.Windows.Forms.Panel
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents NoFilterTypeSelectedPanel As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CNDetailsTLP As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents InboxStatusFilterPanel As System.Windows.Forms.Panel
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents DateFilterPanel As System.Windows.Forms.Panel
    Friend WithEvents DateFilterSuboperatorContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents ComboBox4 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents SignatureStatusFilterPanel As System.Windows.Forms.Panel
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents BootCriticalStatusFilterPanel As System.Windows.Forms.Panel
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents YearMonthPanel As System.Windows.Forms.Panel
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents DatePanel As System.Windows.Forms.Panel
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker

End Class
