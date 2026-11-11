<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectDriversFromDriverPack
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.comboComputerSelect = New System.Windows.Forms.ComboBox()
        Me.lvDriverFiles = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tbComputerInfo = New System.Windows.Forms.TabControl()
        Me.tpHardware = New System.Windows.Forms.TabPage()
        Me.tpSoftware = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.nudStorage = New System.Windows.Forms.NumericUpDown()
        Me.nudMemory = New System.Windows.Forms.NumericUpDown()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.tbSysProcessor = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.tbSysOSName = New System.Windows.Forms.TextBox()
        Me.tbSysOSSku = New System.Windows.Forms.TextBox()
        Me.tbSysOSVer = New System.Windows.Forms.TextBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.tbComputerInfo.SuspendLayout()
        Me.tpHardware.SuspendLayout()
        Me.tpSoftware.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.nudStorage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMemory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel3.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(546, 400)
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
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoEllipsis = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(679, 32)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "The following drivers are present in the Driver Pack that you provided. Select th" & _
    "e ones you would like to add from the list below, or use one of the computers to" & _
    " which you associated the driver files:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(69, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Computer:"
        '
        'comboComputerSelect
        '
        Me.comboComputerSelect.FormattingEnabled = True
        Me.comboComputerSelect.Items.AddRange(New Object() {"Manual Driver Selection"})
        Me.comboComputerSelect.Location = New System.Drawing.Point(184, 53)
        Me.comboComputerSelect.Name = "comboComputerSelect"
        Me.comboComputerSelect.Size = New System.Drawing.Size(452, 21)
        Me.comboComputerSelect.TabIndex = 3
        Me.comboComputerSelect.Text = "Manual Driver Selection"
        '
        'lvDriverFiles
        '
        Me.lvDriverFiles.CheckBoxes = True
        Me.lvDriverFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvDriverFiles.Location = New System.Drawing.Point(12, 215)
        Me.lvDriverFiles.Name = "lvDriverFiles"
        Me.lvDriverFiles.Size = New System.Drawing.Size(680, 179)
        Me.lvDriverFiles.TabIndex = 5
        Me.lvDriverFiles.UseCompatibleStateImageBehavior = False
        Me.lvDriverFiles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 256
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Driver File"
        Me.ColumnHeader2.Width = 256
        '
        'tbComputerInfo
        '
        Me.tbComputerInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbComputerInfo.Controls.Add(Me.tpHardware)
        Me.tbComputerInfo.Controls.Add(Me.tpSoftware)
        Me.tbComputerInfo.Location = New System.Drawing.Point(12, 80)
        Me.tbComputerInfo.Name = "tbComputerInfo"
        Me.tbComputerInfo.SelectedIndex = 0
        Me.tbComputerInfo.Size = New System.Drawing.Size(680, 129)
        Me.tbComputerInfo.TabIndex = 6
        '
        'tpHardware
        '
        Me.tpHardware.Controls.Add(Me.TableLayoutPanel2)
        Me.tpHardware.Location = New System.Drawing.Point(4, 22)
        Me.tpHardware.Name = "tpHardware"
        Me.tpHardware.Padding = New System.Windows.Forms.Padding(3)
        Me.tpHardware.Size = New System.Drawing.Size(672, 103)
        Me.tpHardware.TabIndex = 0
        Me.tpHardware.Text = "Hardware Information"
        Me.tpHardware.UseVisualStyleBackColor = True
        '
        'tpSoftware
        '
        Me.tpSoftware.Controls.Add(Me.TableLayoutPanel3)
        Me.tpSoftware.Location = New System.Drawing.Point(4, 22)
        Me.tpSoftware.Name = "tpSoftware"
        Me.tpSoftware.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSoftware.Size = New System.Drawing.Size(672, 103)
        Me.tpSoftware.TabIndex = 1
        Me.tpSoftware.Text = "Software Information"
        Me.tpSoftware.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TableLayoutPanel2.ColumnCount = 3
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.3917503!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.6082535!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 59.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.nudStorage, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.nudMemory, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label15, 2, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label14, 2, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label11, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.tbSysProcessor, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label12, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label13, 0, 2)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(80, 10)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 3
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1111097!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1111097!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1111097!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(512, 82)
        Me.TableLayoutPanel2.TabIndex = 6
        '
        'nudStorage
        '
        Me.nudStorage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nudStorage.Location = New System.Drawing.Point(122, 57)
        Me.nudStorage.Maximum = New Decimal(New Integer() {-1, -1, 0, 0})
        Me.nudStorage.Minimum = New Decimal(New Integer() {16, 0, 0, 0})
        Me.nudStorage.Name = "nudStorage"
        Me.nudStorage.ReadOnly = True
        Me.nudStorage.Size = New System.Drawing.Size(327, 21)
        Me.nudStorage.TabIndex = 6
        Me.nudStorage.Value = New Decimal(New Integer() {16, 0, 0, 0})
        '
        'nudMemory
        '
        Me.nudMemory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nudMemory.Location = New System.Drawing.Point(122, 30)
        Me.nudMemory.Maximum = New Decimal(New Integer() {-1, -1, 0, 0})
        Me.nudMemory.Minimum = New Decimal(New Integer() {16, 0, 0, 0})
        Me.nudMemory.Name = "nudMemory"
        Me.nudMemory.ReadOnly = True
        Me.nudMemory.Size = New System.Drawing.Size(327, 21)
        Me.nudMemory.TabIndex = 6
        Me.nudMemory.Value = New Decimal(New Integer() {16, 0, 0, 0})
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label15.Location = New System.Drawing.Point(455, 54)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(54, 28)
        Me.Label15.TabIndex = 7
        Me.Label15.Text = "MB"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label14.Location = New System.Drawing.Point(455, 27)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(54, 27)
        Me.Label14.TabIndex = 7
        Me.Label14.Text = "MB"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label11.Location = New System.Drawing.Point(3, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(113, 27)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Processor:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSysProcessor
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.tbSysProcessor, 2)
        Me.tbSysProcessor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysProcessor.Location = New System.Drawing.Point(122, 3)
        Me.tbSysProcessor.Name = "tbSysProcessor"
        Me.tbSysProcessor.ReadOnly = True
        Me.tbSysProcessor.Size = New System.Drawing.Size(387, 21)
        Me.tbSysProcessor.TabIndex = 5
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label12.Location = New System.Drawing.Point(3, 27)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(113, 27)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Memory:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label13.Location = New System.Drawing.Point(3, 54)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(113, 28)
        Me.Label13.TabIndex = 4
        Me.Label13.Text = "Storage:"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.3917503!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.6082535!))
        Me.TableLayoutPanel3.Controls.Add(Me.Label17, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.Label18, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.Label19, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.tbSysOSName, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.tbSysOSSku, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.tbSysOSVer, 1, 2)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(80, 10)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 3
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1111097!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1111097!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.1111097!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(512, 82)
        Me.TableLayoutPanel3.TabIndex = 6
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label17.Location = New System.Drawing.Point(3, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(129, 27)
        Me.Label17.TabIndex = 9
        Me.Label17.Text = "OS Name:"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label18.Location = New System.Drawing.Point(3, 27)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(129, 27)
        Me.Label18.TabIndex = 9
        Me.Label18.Text = "Edition:"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label19.Location = New System.Drawing.Point(3, 54)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(129, 28)
        Me.Label19.TabIndex = 9
        Me.Label19.Text = "OS Version:"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSysOSName
        '
        Me.tbSysOSName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysOSName.Location = New System.Drawing.Point(138, 3)
        Me.tbSysOSName.Name = "tbSysOSName"
        Me.tbSysOSName.ReadOnly = True
        Me.tbSysOSName.Size = New System.Drawing.Size(371, 21)
        Me.tbSysOSName.TabIndex = 5
        '
        'tbSysOSSku
        '
        Me.tbSysOSSku.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysOSSku.Location = New System.Drawing.Point(138, 30)
        Me.tbSysOSSku.Name = "tbSysOSSku"
        Me.tbSysOSSku.ReadOnly = True
        Me.tbSysOSSku.Size = New System.Drawing.Size(371, 21)
        Me.tbSysOSSku.TabIndex = 5
        '
        'tbSysOSVer
        '
        Me.tbSysOSVer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysOSVer.Location = New System.Drawing.Point(138, 57)
        Me.tbSysOSVer.Name = "tbSysOSVer"
        Me.tbSysOSVer.ReadOnly = True
        Me.tbSysOSVer.Size = New System.Drawing.Size(371, 21)
        Me.tbSysOSVer.TabIndex = 5
        '
        'SelectDriversFromDriverPack
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(704, 441)
        Me.Controls.Add(Me.tbComputerInfo)
        Me.Controls.Add(Me.lvDriverFiles)
        Me.Controls.Add(Me.comboComputerSelect)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectDriversFromDriverPack"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Drivers from the Driver Pack"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.tbComputerInfo.ResumeLayout(False)
        Me.tpHardware.ResumeLayout(False)
        Me.tpSoftware.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        CType(Me.nudStorage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMemory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents comboComputerSelect As System.Windows.Forms.ComboBox
    Friend WithEvents lvDriverFiles As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbComputerInfo As System.Windows.Forms.TabControl
    Friend WithEvents tpHardware As System.Windows.Forms.TabPage
    Friend WithEvents tpSoftware As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents nudStorage As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudMemory As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tbSysProcessor As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents tbSysOSName As System.Windows.Forms.TextBox
    Friend WithEvents tbSysOSSku As System.Windows.Forms.TextBox
    Friend WithEvents tbSysOSVer As System.Windows.Forms.TextBox

End Class
