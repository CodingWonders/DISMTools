<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChangeMainDriverDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ChangeMainDriverDialog))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.lbDriverFiles = New System.Windows.Forms.ListBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.pnlDriverReidentification = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.tbDriverName = New System.Windows.Forms.TextBox
        Me.tbDriverManufacturer = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lvHwTargets = New System.Windows.Forms.ListView
        Me.chHwTargets = New System.Windows.Forms.ColumnHeader
        Me.rbIdChange = New System.Windows.Forms.RadioButton
        Me.rbNoIdChange = New System.Windows.Forms.RadioButton
        Me.bwDirScanner = New System.ComponentModel.BackgroundWorker
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.pnlDriverReidentification.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(860, 446)
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
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(983, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = resources.GetString("Label1.Text")
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.lbDriverFiles, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.GroupBox1, 1, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(12, 38)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(994, 402)
        Me.TableLayoutPanel2.TabIndex = 2
        '
        'lbDriverFiles
        '
        Me.lbDriverFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbDriverFiles.FormattingEnabled = True
        Me.lbDriverFiles.Location = New System.Drawing.Point(3, 3)
        Me.lbDriverFiles.Name = "lbDriverFiles"
        Me.lbDriverFiles.ScrollAlwaysVisible = True
        Me.lbDriverFiles.Size = New System.Drawing.Size(491, 394)
        Me.lbDriverFiles.TabIndex = 7
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pnlDriverReidentification)
        Me.GroupBox1.Controls.Add(Me.rbIdChange)
        Me.GroupBox1.Controls.Add(Me.rbNoIdChange)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(500, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(491, 396)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Identification in Driver Pack"
        '
        'pnlDriverReidentification
        '
        Me.pnlDriverReidentification.Controls.Add(Me.Label5)
        Me.pnlDriverReidentification.Controls.Add(Me.Label4)
        Me.pnlDriverReidentification.Controls.Add(Me.tbDriverName)
        Me.pnlDriverReidentification.Controls.Add(Me.tbDriverManufacturer)
        Me.pnlDriverReidentification.Controls.Add(Me.Label3)
        Me.pnlDriverReidentification.Controls.Add(Me.Label2)
        Me.pnlDriverReidentification.Controls.Add(Me.lvHwTargets)
        Me.pnlDriverReidentification.Enabled = False
        Me.pnlDriverReidentification.Location = New System.Drawing.Point(48, 79)
        Me.pnlDriverReidentification.Name = "pnlDriverReidentification"
        Me.pnlDriverReidentification.Size = New System.Drawing.Size(424, 298)
        Me.pnlDriverReidentification.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 250)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(114, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Manufacturer/Vendor:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 204)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Name:"
        '
        'tbDriverName
        '
        Me.tbDriverName.Location = New System.Drawing.Point(11, 220)
        Me.tbDriverName.Name = "tbDriverName"
        Me.tbDriverName.Size = New System.Drawing.Size(402, 21)
        Me.tbDriverName.TabIndex = 8
        '
        'tbDriverManufacturer
        '
        Me.tbDriverManufacturer.Location = New System.Drawing.Point(11, 266)
        Me.tbDriverManufacturer.Name = "tbDriverManufacturer"
        Me.tbDriverManufacturer.Size = New System.Drawing.Size(402, 21)
        Me.tbDriverManufacturer.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 180)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(192, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "...or specify this information manually:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(258, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Select from one of the following hardware targets..."
        '
        'lvHwTargets
        '
        Me.lvHwTargets.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chHwTargets})
        Me.lvHwTargets.FullRowSelect = True
        Me.lvHwTargets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvHwTargets.Location = New System.Drawing.Point(4, 30)
        Me.lvHwTargets.MultiSelect = False
        Me.lvHwTargets.Name = "lvHwTargets"
        Me.lvHwTargets.Size = New System.Drawing.Size(416, 144)
        Me.lvHwTargets.TabIndex = 5
        Me.lvHwTargets.UseCompatibleStateImageBehavior = False
        Me.lvHwTargets.View = System.Windows.Forms.View.Details
        '
        'chHwTargets
        '
        Me.chHwTargets.Text = "Driver"
        Me.chHwTargets.Width = 1024
        '
        'rbIdChange
        '
        Me.rbIdChange.AutoSize = True
        Me.rbIdChange.Location = New System.Drawing.Point(28, 55)
        Me.rbIdChange.Name = "rbIdChange"
        Me.rbIdChange.Size = New System.Drawing.Size(163, 17)
        Me.rbIdChange.TabIndex = 0
        Me.rbIdChange.Text = "Change this driver's identity:"
        Me.rbIdChange.UseVisualStyleBackColor = True
        '
        'rbNoIdChange
        '
        Me.rbNoIdChange.AutoSize = True
        Me.rbNoIdChange.Checked = True
        Me.rbNoIdChange.Location = New System.Drawing.Point(28, 32)
        Me.rbNoIdChange.Name = "rbNoIdChange"
        Me.rbNoIdChange.Size = New System.Drawing.Size(192, 17)
        Me.rbNoIdChange.TabIndex = 0
        Me.rbNoIdChange.TabStop = True
        Me.rbNoIdChange.Text = "Do not change this driver's identity"
        Me.rbNoIdChange.UseVisualStyleBackColor = True
        '
        'bwDirScanner
        '
        '
        'ChangeMainDriverDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(1018, 487)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ChangeMainDriverDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Change Main Driver File"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pnlDriverReidentification.ResumeLayout(False)
        Me.pnlDriverReidentification.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbNoIdChange As System.Windows.Forms.RadioButton
    Friend WithEvents rbIdChange As System.Windows.Forms.RadioButton
    Friend WithEvents pnlDriverReidentification As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lvHwTargets As System.Windows.Forms.ListView
    Friend WithEvents chHwTargets As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbDriverManufacturer As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbDriverName As System.Windows.Forms.TextBox
    Friend WithEvents lbDriverFiles As System.Windows.Forms.ListBox
    Friend WithEvents bwDirScanner As System.ComponentModel.BackgroundWorker

End Class
