<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomRuleDetailsDialog
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.RuleNameTextBox = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.RuleDescriptionTextBox = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.RuleSeverityComboBox = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.RegexNextMatchButton = New System.Windows.Forms.Button
        Me.RegexPrevMatchButton = New System.Windows.Forms.Button
        Me.MatchCountLabel = New System.Windows.Forms.Label
        Me.RegexTesterButton = New System.Windows.Forms.Button
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.RuleExpressionTesterTextBox = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.RegexCheatSheetButton = New System.Windows.Forms.Button
        Me.RuleExpressionTextBox = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(476, 414)
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
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Rule Name:"
        '
        'RuleNameTextBox
        '
        Me.RuleNameTextBox.Location = New System.Drawing.Point(107, 10)
        Me.RuleNameTextBox.Name = "RuleNameTextBox"
        Me.RuleNameTextBox.Size = New System.Drawing.Size(515, 21)
        Me.RuleNameTextBox.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Rule Description:"
        '
        'RuleDescriptionTextBox
        '
        Me.RuleDescriptionTextBox.Location = New System.Drawing.Point(107, 37)
        Me.RuleDescriptionTextBox.Name = "RuleDescriptionTextBox"
        Me.RuleDescriptionTextBox.Size = New System.Drawing.Size(515, 21)
        Me.RuleDescriptionTextBox.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Rule Severity:"
        '
        'RuleSeverityComboBox
        '
        Me.RuleSeverityComboBox.FormattingEnabled = True
        Me.RuleSeverityComboBox.Items.AddRange(New Object() {"Low", "Medium", "High"})
        Me.RuleSeverityComboBox.Location = New System.Drawing.Point(107, 64)
        Me.RuleSeverityComboBox.Name = "RuleSeverityComboBox"
        Me.RuleSeverityComboBox.Size = New System.Drawing.Size(515, 21)
        Me.RuleSeverityComboBox.TabIndex = 3
        Me.RuleSeverityComboBox.Text = "Medium"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RegexNextMatchButton)
        Me.GroupBox1.Controls.Add(Me.RegexPrevMatchButton)
        Me.GroupBox1.Controls.Add(Me.MatchCountLabel)
        Me.GroupBox1.Controls.Add(Me.RegexTesterButton)
        Me.GroupBox1.Controls.Add(Me.ComboBox2)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.RuleExpressionTesterTextBox)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.RegexCheatSheetButton)
        Me.GroupBox1.Controls.Add(Me.RuleExpressionTextBox)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 91)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(606, 317)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Expression Parameters"
        '
        'RegexNextMatchButton
        '
        Me.RegexNextMatchButton.Enabled = False
        Me.RegexNextMatchButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RegexNextMatchButton.Location = New System.Drawing.Point(461, 275)
        Me.RegexNextMatchButton.Name = "RegexNextMatchButton"
        Me.RegexNextMatchButton.Size = New System.Drawing.Size(128, 23)
        Me.RegexNextMatchButton.TabIndex = 9
        Me.RegexNextMatchButton.Text = "Next Match"
        Me.RegexNextMatchButton.UseVisualStyleBackColor = True
        '
        'RegexPrevMatchButton
        '
        Me.RegexPrevMatchButton.Enabled = False
        Me.RegexPrevMatchButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RegexPrevMatchButton.Location = New System.Drawing.Point(327, 275)
        Me.RegexPrevMatchButton.Name = "RegexPrevMatchButton"
        Me.RegexPrevMatchButton.Size = New System.Drawing.Size(128, 23)
        Me.RegexPrevMatchButton.TabIndex = 9
        Me.RegexPrevMatchButton.Text = "Previous Match"
        Me.RegexPrevMatchButton.UseVisualStyleBackColor = True
        '
        'MatchCountLabel
        '
        Me.MatchCountLabel.AutoSize = True
        Me.MatchCountLabel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MatchCountLabel.Location = New System.Drawing.Point(20, 285)
        Me.MatchCountLabel.Name = "MatchCountLabel"
        Me.MatchCountLabel.Size = New System.Drawing.Size(65, 13)
        Me.MatchCountLabel.TabIndex = 8
        Me.MatchCountLabel.Text = "0 Matches"
        '
        'RegexTesterButton
        '
        Me.RegexTesterButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RegexTesterButton.Location = New System.Drawing.Point(23, 247)
        Me.RegexTesterButton.Name = "RegexTesterButton"
        Me.RegexTesterButton.Size = New System.Drawing.Size(128, 23)
        Me.RegexTesterButton.TabIndex = 7
        Me.RegexTesterButton.Text = "Test Matches"
        Me.RegexTesterButton.UseVisualStyleBackColor = True
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Items.AddRange(New Object() {"Custom", "API key leaks"})
        Me.ComboBox2.Location = New System.Drawing.Point(203, 218)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(387, 21)
        Me.ComboBox2.TabIndex = 6
        Me.ComboBox2.Text = "Custom"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(20, 221)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(177, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Security violation pattern template:"
        '
        'RuleExpressionTesterTextBox
        '
        Me.RuleExpressionTesterTextBox.Font = New System.Drawing.Font("Lucida Console", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RuleExpressionTesterTextBox.Location = New System.Drawing.Point(19, 84)
        Me.RuleExpressionTesterTextBox.Multiline = True
        Me.RuleExpressionTesterTextBox.Name = "RuleExpressionTesterTextBox"
        Me.RuleExpressionTesterTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.RuleExpressionTesterTextBox.Size = New System.Drawing.Size(570, 128)
        Me.RuleExpressionTesterTextBox.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoEllipsis = True
        Me.Label5.Location = New System.Drawing.Point(19, 49)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(372, 32)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Use the area below to test your rule against common security violation patterns, " & _
            "or use a custom template:"
        '
        'RegexCheatSheetButton
        '
        Me.RegexCheatSheetButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RegexCheatSheetButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RegexCheatSheetButton.Location = New System.Drawing.Point(397, 49)
        Me.RegexCheatSheetButton.Name = "RegexCheatSheetButton"
        Me.RegexCheatSheetButton.Size = New System.Drawing.Size(192, 23)
        Me.RegexCheatSheetButton.TabIndex = 2
        Me.RegexCheatSheetButton.Text = "Regular Expression Cheatsheet"
        Me.RegexCheatSheetButton.UseVisualStyleBackColor = True
        '
        'RuleExpressionTextBox
        '
        Me.RuleExpressionTextBox.Location = New System.Drawing.Point(109, 21)
        Me.RuleExpressionTextBox.Name = "RuleExpressionTextBox"
        Me.RuleExpressionTextBox.Size = New System.Drawing.Size(481, 21)
        Me.RuleExpressionTextBox.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(87, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Rule Expression:"
        '
        'CustomRuleDetailsDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(634, 455)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.RuleSeverityComboBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.RuleDescriptionTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.RuleNameTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CustomRuleDetailsDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add/Modify Custom Inspection Rule"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RuleNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents RuleDescriptionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents RuleSeverityComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents MatchCountLabel As System.Windows.Forms.Label
    Friend WithEvents RegexTesterButton As System.Windows.Forms.Button
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents RuleExpressionTesterTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents RegexCheatSheetButton As System.Windows.Forms.Button
    Friend WithEvents RuleExpressionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents RegexNextMatchButton As System.Windows.Forms.Button
    Friend WithEvents RegexPrevMatchButton As System.Windows.Forms.Button

End Class
