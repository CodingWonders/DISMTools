<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AIResults
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
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Label1 = New System.Windows.Forms.Label
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.Label2 = New System.Windows.Forms.Label
        Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.lblViolationCount = New System.Windows.Forms.Label
        Me.cbHighViolations = New System.Windows.Forms.CheckBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.cbLowViolations = New System.Windows.Forms.CheckBox
        Me.cbMediumViolations = New System.Windows.Forms.CheckBox
        Me.CustomRulesButton = New System.Windows.Forms.Button
        Me.ScannedRuleSeverityColumn = New System.Windows.Forms.DataGridViewImageColumn
        Me.ScannedRuleDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.AutoInspectionResultBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.AutoInspectionResultBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(324, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "The following results were returned during the inspection process:"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridView1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ScannedRuleSeverityColumn, Me.ScannedRuleDataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.AutoInspectionResultBindingSource
        Me.DataGridView1.Location = New System.Drawing.Point(12, 72)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridView1.RowsDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(992, 462)
        Me.DataGridView1.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoEllipsis = True
        Me.Label2.Location = New System.Drawing.Point(12, 547)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(992, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "The inspections performed on the script code are automated and may not be complet" & _
            "ely accurate. Revise script code manually."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DataGridViewImageColumn1
        '
        Me.DataGridViewImageColumn1.DataPropertyName = "ScannedRule"
        Me.DataGridViewImageColumn1.HeaderText = "Severity"
        Me.DataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
        Me.DataGridViewImageColumn1.ReadOnly = True
        Me.DataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewImageColumn1.Width = 64
        '
        'CheckBox1
        '
        Me.CheckBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBox1.Appearance = System.Windows.Forms.Appearance.Button
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox1.Location = New System.Drawing.Point(941, 8)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(63, 23)
        Me.CheckBox1.TabIndex = 3
        Me.CheckBox1.Text = "Pin to top"
        Me.CheckBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'lblViolationCount
        '
        Me.lblViolationCount.AutoSize = True
        Me.lblViolationCount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblViolationCount.Location = New System.Drawing.Point(3, 0)
        Me.lblViolationCount.Name = "lblViolationCount"
        Me.lblViolationCount.Size = New System.Drawing.Size(122, 28)
        Me.lblViolationCount.TabIndex = 4
        Me.lblViolationCount.Text = "Violation count:"
        Me.lblViolationCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbHighViolations
        '
        Me.cbHighViolations.Appearance = System.Windows.Forms.Appearance.Button
        Me.cbHighViolations.AutoSize = True
        Me.cbHighViolations.Checked = True
        Me.cbHighViolations.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbHighViolations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbHighViolations.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbHighViolations.Location = New System.Drawing.Point(131, 3)
        Me.cbHighViolations.Name = "cbHighViolations"
        Me.cbHighViolations.Size = New System.Drawing.Size(164, 22)
        Me.cbHighViolations.TabIndex = 5
        Me.cbHighViolations.Text = "High-severity violations"
        Me.cbHighViolations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cbHighViolations.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.Controls.Add(Me.cbLowViolations, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.cbMediumViolations, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblViolationCount, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.cbHighViolations, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 38)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(640, 28)
        Me.TableLayoutPanel1.TabIndex = 6
        '
        'cbLowViolations
        '
        Me.cbLowViolations.Appearance = System.Windows.Forms.Appearance.Button
        Me.cbLowViolations.AutoSize = True
        Me.cbLowViolations.Checked = True
        Me.cbLowViolations.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbLowViolations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbLowViolations.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbLowViolations.Location = New System.Drawing.Point(471, 3)
        Me.cbLowViolations.Name = "cbLowViolations"
        Me.cbLowViolations.Size = New System.Drawing.Size(166, 22)
        Me.cbLowViolations.TabIndex = 7
        Me.cbLowViolations.Text = "Low-severity violations"
        Me.cbLowViolations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cbLowViolations.UseVisualStyleBackColor = True
        '
        'cbMediumViolations
        '
        Me.cbMediumViolations.Appearance = System.Windows.Forms.Appearance.Button
        Me.cbMediumViolations.AutoSize = True
        Me.cbMediumViolations.Checked = True
        Me.cbMediumViolations.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbMediumViolations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cbMediumViolations.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbMediumViolations.Location = New System.Drawing.Point(301, 3)
        Me.cbMediumViolations.Name = "cbMediumViolations"
        Me.cbMediumViolations.Size = New System.Drawing.Size(164, 22)
        Me.cbMediumViolations.TabIndex = 6
        Me.cbMediumViolations.Text = "Medium-severity violations"
        Me.cbMediumViolations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cbMediumViolations.UseVisualStyleBackColor = True
        '
        'CustomRulesButton
        '
        Me.CustomRulesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CustomRulesButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CustomRulesButton.Location = New System.Drawing.Point(839, 8)
        Me.CustomRulesButton.Name = "CustomRulesButton"
        Me.CustomRulesButton.Size = New System.Drawing.Size(96, 23)
        Me.CustomRulesButton.TabIndex = 7
        Me.CustomRulesButton.Text = "Custom Rules..."
        Me.CustomRulesButton.UseVisualStyleBackColor = True
        '
        'ScannedRuleSeverityColumn
        '
        Me.ScannedRuleSeverityColumn.DataPropertyName = "ScannedRule"
        Me.ScannedRuleSeverityColumn.HeaderText = "Severity"
        Me.ScannedRuleSeverityColumn.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.ScannedRuleSeverityColumn.Name = "ScannedRuleSeverityColumn"
        Me.ScannedRuleSeverityColumn.ReadOnly = True
        Me.ScannedRuleSeverityColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ScannedRuleSeverityColumn.Width = 64
        '
        'ScannedRuleDataGridViewTextBoxColumn
        '
        Me.ScannedRuleDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ScannedRuleDataGridViewTextBoxColumn.DataPropertyName = "ScannedRule"
        Me.ScannedRuleDataGridViewTextBoxColumn.HeaderText = "Scanned Rule"
        Me.ScannedRuleDataGridViewTextBoxColumn.Name = "ScannedRuleDataGridViewTextBoxColumn"
        Me.ScannedRuleDataGridViewTextBoxColumn.ReadOnly = True
        '
        'AutoInspectionResultBindingSource
        '
        Me.AutoInspectionResultBindingSource.DataSource = GetType(StarterScriptEditor.Classes.AutoInspection.AutoInspectionResult)
        '
        'AIResults
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1016, 573)
        Me.Controls.Add(Me.CustomRulesButton)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimumSize = New System.Drawing.Size(720, 320)
        Me.Name = "AIResults"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Automated Inspection Results"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.AutoInspectionResultBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents AutoInspectionResultBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ScannedRuleSeverityColumn As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents ScannedRuleDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewImageColumn1 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents lblViolationCount As System.Windows.Forms.Label
    Friend WithEvents cbHighViolations As System.Windows.Forms.CheckBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cbLowViolations As System.Windows.Forms.CheckBox
    Friend WithEvents cbMediumViolations As System.Windows.Forms.CheckBox
    Friend WithEvents CustomRulesButton As System.Windows.Forms.Button
End Class
