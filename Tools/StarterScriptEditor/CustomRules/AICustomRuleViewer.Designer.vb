<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AICustomRuleViewer
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.AddCustomRuleButton = New System.Windows.Forms.Button
        Me.ModifyCustomRuleButton = New System.Windows.Forms.Button
        Me.DeleteCustomRuleButton = New System.Windows.Forms.Button
        Me.SaveCustomRulesButton = New System.Windows.Forms.Button
        Me.RefreshRulesButton = New System.Windows.Forms.Button
        Me.CustomRuleLV = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(917, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "View custom rules for automated inspection. Use the buttons at the bottom of the " & _
            "window to perform tasks with custom rules. When you've finished setting up custo" & _
            "m rules, click Save Rules."
        '
        'AddCustomRuleButton
        '
        Me.AddCustomRuleButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddCustomRuleButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.AddCustomRuleButton.Location = New System.Drawing.Point(12, 538)
        Me.AddCustomRuleButton.Name = "AddCustomRuleButton"
        Me.AddCustomRuleButton.Size = New System.Drawing.Size(96, 23)
        Me.AddCustomRuleButton.TabIndex = 1
        Me.AddCustomRuleButton.Text = "Add Rule..."
        Me.AddCustomRuleButton.UseVisualStyleBackColor = True
        '
        'ModifyCustomRuleButton
        '
        Me.ModifyCustomRuleButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ModifyCustomRuleButton.Enabled = False
        Me.ModifyCustomRuleButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ModifyCustomRuleButton.Location = New System.Drawing.Point(114, 538)
        Me.ModifyCustomRuleButton.Name = "ModifyCustomRuleButton"
        Me.ModifyCustomRuleButton.Size = New System.Drawing.Size(96, 23)
        Me.ModifyCustomRuleButton.TabIndex = 1
        Me.ModifyCustomRuleButton.Text = "Modify Rule..."
        Me.ModifyCustomRuleButton.UseVisualStyleBackColor = True
        '
        'DeleteCustomRuleButton
        '
        Me.DeleteCustomRuleButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DeleteCustomRuleButton.Enabled = False
        Me.DeleteCustomRuleButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.DeleteCustomRuleButton.Location = New System.Drawing.Point(216, 538)
        Me.DeleteCustomRuleButton.Name = "DeleteCustomRuleButton"
        Me.DeleteCustomRuleButton.Size = New System.Drawing.Size(96, 23)
        Me.DeleteCustomRuleButton.TabIndex = 1
        Me.DeleteCustomRuleButton.Text = "Delete Rule"
        Me.DeleteCustomRuleButton.UseVisualStyleBackColor = True
        '
        'SaveCustomRulesButton
        '
        Me.SaveCustomRulesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveCustomRulesButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SaveCustomRulesButton.Location = New System.Drawing.Point(806, 538)
        Me.SaveCustomRulesButton.Name = "SaveCustomRulesButton"
        Me.SaveCustomRulesButton.Size = New System.Drawing.Size(96, 23)
        Me.SaveCustomRulesButton.TabIndex = 1
        Me.SaveCustomRulesButton.Text = "Save Rules"
        Me.SaveCustomRulesButton.UseVisualStyleBackColor = True
        '
        'RefreshRulesButton
        '
        Me.RefreshRulesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RefreshRulesButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RefreshRulesButton.Location = New System.Drawing.Point(908, 538)
        Me.RefreshRulesButton.Name = "RefreshRulesButton"
        Me.RefreshRulesButton.Size = New System.Drawing.Size(96, 23)
        Me.RefreshRulesButton.TabIndex = 1
        Me.RefreshRulesButton.Text = "Refresh Rules"
        Me.RefreshRulesButton.UseVisualStyleBackColor = True
        '
        'CustomRuleLV
        '
        Me.CustomRuleLV.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CustomRuleLV.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.CustomRuleLV.FullRowSelect = True
        Me.CustomRuleLV.Location = New System.Drawing.Point(12, 39)
        Me.CustomRuleLV.Name = "CustomRuleLV"
        Me.CustomRuleLV.Size = New System.Drawing.Size(992, 493)
        Me.CustomRuleLV.TabIndex = 2
        Me.CustomRuleLV.UseCompatibleStateImageBehavior = False
        Me.CustomRuleLV.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 192
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Description"
        Me.ColumnHeader2.Width = 344
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Expression"
        Me.ColumnHeader3.Width = 344
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Severity"
        Me.ColumnHeader4.Width = 72
        '
        'AICustomRuleViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1016, 573)
        Me.Controls.Add(Me.CustomRuleLV)
        Me.Controls.Add(Me.RefreshRulesButton)
        Me.Controls.Add(Me.SaveCustomRulesButton)
        Me.Controls.Add(Me.DeleteCustomRuleButton)
        Me.Controls.Add(Me.ModifyCustomRuleButton)
        Me.Controls.Add(Me.AddCustomRuleButton)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "AICustomRuleViewer"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Custom Rules"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AddCustomRuleButton As System.Windows.Forms.Button
    Friend WithEvents ModifyCustomRuleButton As System.Windows.Forms.Button
    Friend WithEvents DeleteCustomRuleButton As System.Windows.Forms.Button
    Friend WithEvents SaveCustomRulesButton As System.Windows.Forms.Button
    Friend WithEvents RefreshRulesButton As System.Windows.Forms.Button
    Friend WithEvents CustomRuleLV As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
End Class
