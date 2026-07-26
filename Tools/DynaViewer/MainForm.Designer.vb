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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button3 = New System.Windows.Forms.Button
        Me.ColorModeCMS = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LightCM_TSMI = New System.Windows.Forms.ToolStripMenuItem
        Me.DarkCM_TSMI = New System.Windows.Forms.ToolStripMenuItem
        Me.SystemCM_TSMI = New System.Windows.Forms.ToolStripMenuItem
        Me.Button4 = New System.Windows.Forms.Button
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.RegexCB = New System.Windows.Forms.CheckBox
        Me.RegexFailureBtn = New System.Windows.Forms.Button
        Me.CaseSensitiveCB = New System.Windows.Forms.CheckBox
        Me.FilterBW = New System.ComponentModel.BackgroundWorker
        Me.DebounceTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.ColorModeCMS.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Dyna.Log.File.Label")
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(111, 10)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(987, 21)
        Me.TextBox1.TabIndex = 1
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("DynaViewer.Designer.Main")("LogFiles.Filter")
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.ListView1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 101)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1248, 551)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Dyna.Log.Event")
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader4, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(3, 17)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(1242, 531)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("EventTimestamp.Column")
        Me.ColumnHeader1.Width = 192
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("ProcessID.Column")
        Me.ColumnHeader4.Width = 94
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("EventCaller.Column")
        Me.ColumnHeader2.Width = 256
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Message.Column")
        Me.ColumnHeader3.Width = 640
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(1104, 9)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Browse.Button")
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Enabled = False
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button2.Location = New System.Drawing.Point(1185, 9)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Refresh.Button")
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoEllipsis = True
        Me.Label2.Location = New System.Drawing.Point(12, 663)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(687, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Processed.Entries.Label")
        Me.Label2.Visible = False
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button3.Location = New System.Drawing.Point(1185, 658)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 6
        Me.Button3.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("About.ActionButton")
        Me.Button3.UseVisualStyleBackColor = True
        '
        'ColorModeCMS
        '
        Me.ColorModeCMS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LightCM_TSMI, Me.DarkCM_TSMI, Me.SystemCM_TSMI})
        Me.ColorModeCMS.Name = "ColorModeCMS"
        Me.ColorModeCMS.Size = New System.Drawing.Size(121, 70)
        '
        'LightCM_TSMI
        '
        Me.LightCM_TSMI.Name = "LightCM_TSMI"
        Me.LightCM_TSMI.Size = New System.Drawing.Size(120, 22)
        Me.LightCM_TSMI.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("LightCM.Label")
        '
        'DarkCM_TSMI
        '
        Me.DarkCM_TSMI.Name = "DarkCM_TSMI"
        Me.DarkCM_TSMI.Size = New System.Drawing.Size(120, 22)
        Me.DarkCM_TSMI.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("DarkCM.Label")
        '
        'SystemCM_TSMI
        '
        Me.SystemCM_TSMI.Name = "SystemCM_TSMI"
        Me.SystemCM_TSMI.Size = New System.Drawing.Size(120, 22)
        Me.SystemCM_TSMI.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("SystemCM.Label")
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button4.Location = New System.Drawing.Point(1073, 658)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(106, 23)
        Me.Button4.TabIndex = 8
        Me.Button4.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("ColorMode.Button")
        Me.Button4.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 6
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.921619!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.97709!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.10129!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBox2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label6, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBox1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBox2, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.RegexCB, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.RegexFailureBtn, 5, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.CaseSensitiveCB, 4, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 38)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1248, 57)
        Me.TableLayoutPanel1.TabIndex = 9
        '
        'ComboBox2
        '
        Me.ComboBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(94, 31)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(212, 21)
        Me.ComboBox2.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(85, 28)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("PID.Label")
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(94, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(212, 28)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("EventCaller.Label")
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label6, 2)
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(1152, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 28)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Options.Heading.Label")
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox1
        '
        Me.ComboBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(3, 31)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(85, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'TextBox2
        '
        Me.TextBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox2.Location = New System.Drawing.Point(312, 31)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(834, 21)
        Me.TextBox2.TabIndex = 3
        '
        'RegexCB
        '
        Me.RegexCB.Appearance = System.Windows.Forms.Appearance.Button
        Me.RegexCB.AutoSize = True
        Me.RegexCB.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RegexCB.Location = New System.Drawing.Point(1152, 31)
        Me.RegexCB.Name = "RegexCB"
        Me.RegexCB.Size = New System.Drawing.Size(26, 23)
        Me.RegexCB.TabIndex = 4
        Me.RegexCB.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("RegexCB.Label")
        Me.RegexCB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.RegexCB.UseVisualStyleBackColor = True
        '
        'RegexFailureBtn
        '
        Me.RegexFailureBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RegexFailureBtn.Location = New System.Drawing.Point(1216, 31)
        Me.RegexFailureBtn.Name = "RegexFailureBtn"
        Me.RegexFailureBtn.Size = New System.Drawing.Size(27, 23)
        Me.RegexFailureBtn.TabIndex = 5
        Me.RegexFailureBtn.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Regex.Failure.Btn.Label")
        Me.RegexFailureBtn.UseVisualStyleBackColor = True
        Me.RegexFailureBtn.Visible = False
        '
        'CaseSensitiveCB
        '
        Me.CaseSensitiveCB.Appearance = System.Windows.Forms.Appearance.Button
        Me.CaseSensitiveCB.AutoSize = True
        Me.CaseSensitiveCB.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CaseSensitiveCB.Location = New System.Drawing.Point(1184, 31)
        Me.CaseSensitiveCB.Name = "CaseSensitiveCB"
        Me.CaseSensitiveCB.Size = New System.Drawing.Size(26, 23)
        Me.CaseSensitiveCB.TabIndex = 4
        Me.CaseSensitiveCB.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Aa.Label")
        Me.CaseSensitiveCB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CaseSensitiveCB.UseVisualStyleBackColor = True
        '
        'FilterBW
        '
        Me.FilterBW.WorkerReportsProgress = True
        Me.FilterBW.WorkerSupportsCancellation = True
        '
        'DebounceTimer
        '
        Me.DebounceTimer.Interval = 350
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(875, 658)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(192, 23)
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar1.TabIndex = 10
        Me.ProgressBar1.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(312, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(834, 28)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Message.Label")
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1272, 693)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1280, 720)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Dyna.Log.Viewer.Label")
        Me.GroupBox1.ResumeLayout(False)
        Me.ColorModeCMS.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColorModeCMS As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents LightCM_TSMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DarkCM_TSMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SystemCM_TSMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents RegexCB As System.Windows.Forms.CheckBox
    Friend WithEvents FilterBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents RegexFailureBtn As System.Windows.Forms.Button
    Friend WithEvents DebounceTimer As System.Windows.Forms.Timer
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents CaseSensitiveCB As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label

End Class
