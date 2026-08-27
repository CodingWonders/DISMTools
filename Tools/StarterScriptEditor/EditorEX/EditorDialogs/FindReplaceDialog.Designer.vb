<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FindReplaceDialog
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
        Me.cbPin = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbFindContents = New System.Windows.Forms.TextBox
        Me.btnFindNext = New System.Windows.Forms.Button
        Me.btnFindPrevious = New System.Windows.Forms.Button
        Me.btnFindAll = New System.Windows.Forms.Button
        Me.pnlReplace = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbReplaceContents = New System.Windows.Forms.TextBox
        Me.cbRegex = New System.Windows.Forms.CheckBox
        Me.cbMatchCase = New System.Windows.Forms.CheckBox
        Me.btnReplace = New System.Windows.Forms.Button
        Me.btnReplaceAll = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lvResults = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.btnExpandCollapse = New System.Windows.Forms.Button
        Me.pnlControls = New System.Windows.Forms.Panel
        Me.cbReplaceMode = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.pnlReplace.SuspendLayout()
        Me.pnlControls.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbPin
        '
        Me.cbPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbPin.Appearance = System.Windows.Forms.Appearance.Button
        Me.cbPin.Checked = True
        Me.cbPin.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbPin.Image = Global.StarterScriptEditor.My.Resources.Resources.pin
        Me.cbPin.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.cbPin.Location = New System.Drawing.Point(538, 156)
        Me.cbPin.Name = "cbPin"
        Me.cbPin.Size = New System.Drawing.Size(24, 24)
        Me.cbPin.TabIndex = 0
        Me.cbPin.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Find what:"
        '
        'tbFindContents
        '
        Me.tbFindContents.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbFindContents.Location = New System.Drawing.Point(93, 12)
        Me.tbFindContents.Name = "tbFindContents"
        Me.tbFindContents.Size = New System.Drawing.Size(374, 21)
        Me.tbFindContents.TabIndex = 2
        '
        'btnFindNext
        '
        Me.btnFindNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFindNext.Enabled = False
        Me.btnFindNext.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnFindNext.Location = New System.Drawing.Point(476, 11)
        Me.btnFindNext.Name = "btnFindNext"
        Me.btnFindNext.Size = New System.Drawing.Size(86, 23)
        Me.btnFindNext.TabIndex = 3
        Me.btnFindNext.Text = "Find Next"
        Me.btnFindNext.UseVisualStyleBackColor = True
        '
        'btnFindPrevious
        '
        Me.btnFindPrevious.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFindPrevious.Enabled = False
        Me.btnFindPrevious.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnFindPrevious.Location = New System.Drawing.Point(476, 40)
        Me.btnFindPrevious.Name = "btnFindPrevious"
        Me.btnFindPrevious.Size = New System.Drawing.Size(86, 23)
        Me.btnFindPrevious.TabIndex = 3
        Me.btnFindPrevious.Text = "Find Previous"
        Me.btnFindPrevious.UseVisualStyleBackColor = True
        '
        'btnFindAll
        '
        Me.btnFindAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFindAll.Enabled = False
        Me.btnFindAll.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnFindAll.Location = New System.Drawing.Point(476, 69)
        Me.btnFindAll.Name = "btnFindAll"
        Me.btnFindAll.Size = New System.Drawing.Size(86, 23)
        Me.btnFindAll.TabIndex = 3
        Me.btnFindAll.Text = "Find All"
        Me.btnFindAll.UseVisualStyleBackColor = True
        '
        'pnlReplace
        '
        Me.pnlReplace.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlReplace.Controls.Add(Me.Label2)
        Me.pnlReplace.Controls.Add(Me.tbReplaceContents)
        Me.pnlReplace.Enabled = False
        Me.pnlReplace.Location = New System.Drawing.Point(7, 67)
        Me.pnlReplace.Name = "pnlReplace"
        Me.pnlReplace.Size = New System.Drawing.Size(463, 28)
        Me.pnlReplace.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Replace with:"
        '
        'tbReplaceContents
        '
        Me.tbReplaceContents.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbReplaceContents.Location = New System.Drawing.Point(86, 3)
        Me.tbReplaceContents.Name = "tbReplaceContents"
        Me.tbReplaceContents.Size = New System.Drawing.Size(374, 21)
        Me.tbReplaceContents.TabIndex = 2
        '
        'cbRegex
        '
        Me.cbRegex.Appearance = System.Windows.Forms.Appearance.Button
        Me.cbRegex.AutoSize = True
        Me.cbRegex.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbRegex.Location = New System.Drawing.Point(93, 40)
        Me.cbRegex.Name = "cbRegex"
        Me.cbRegex.Size = New System.Drawing.Size(27, 23)
        Me.cbRegex.TabIndex = 5
        Me.cbRegex.Text = ".*"
        Me.cbRegex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cbRegex.UseVisualStyleBackColor = True
        '
        'cbMatchCase
        '
        Me.cbMatchCase.Appearance = System.Windows.Forms.Appearance.Button
        Me.cbMatchCase.AutoSize = True
        Me.cbMatchCase.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbMatchCase.Location = New System.Drawing.Point(126, 40)
        Me.cbMatchCase.Name = "cbMatchCase"
        Me.cbMatchCase.Size = New System.Drawing.Size(30, 23)
        Me.cbMatchCase.TabIndex = 5
        Me.cbMatchCase.Text = "Aa"
        Me.cbMatchCase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cbMatchCase.UseVisualStyleBackColor = True
        '
        'btnReplace
        '
        Me.btnReplace.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReplace.Enabled = False
        Me.btnReplace.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnReplace.Location = New System.Drawing.Point(476, 98)
        Me.btnReplace.Name = "btnReplace"
        Me.btnReplace.Size = New System.Drawing.Size(86, 23)
        Me.btnReplace.TabIndex = 3
        Me.btnReplace.Text = "Replace"
        Me.btnReplace.UseVisualStyleBackColor = True
        '
        'btnReplaceAll
        '
        Me.btnReplaceAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReplaceAll.Enabled = False
        Me.btnReplaceAll.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnReplaceAll.Location = New System.Drawing.Point(476, 127)
        Me.btnReplaceAll.Name = "btnReplaceAll"
        Me.btnReplaceAll.Size = New System.Drawing.Size(86, 23)
        Me.btnReplaceAll.TabIndex = 3
        Me.btnReplaceAll.Text = "Replace All"
        Me.btnReplaceAll.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(15, 103)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(38, 13)
        Me.lblStatus.TabIndex = 6
        Me.lblStatus.Text = "Ready"
        '
        'lvResults
        '
        Me.lvResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lvResults.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvResults.FullRowSelect = True
        Me.lvResults.Location = New System.Drawing.Point(0, 192)
        Me.lvResults.MultiSelect = False
        Me.lvResults.Name = "lvResults"
        Me.lvResults.Size = New System.Drawing.Size(568, 165)
        Me.lvResults.TabIndex = 7
        Me.lvResults.UseCompatibleStateImageBehavior = False
        Me.lvResults.View = System.Windows.Forms.View.Details
        Me.lvResults.Visible = False
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Line"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Column"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Length"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Line Preview"
        Me.ColumnHeader4.Width = 362
        '
        'btnExpandCollapse
        '
        Me.btnExpandCollapse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExpandCollapse.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnExpandCollapse.Location = New System.Drawing.Point(457, 156)
        Me.btnExpandCollapse.Name = "btnExpandCollapse"
        Me.btnExpandCollapse.Size = New System.Drawing.Size(75, 23)
        Me.btnExpandCollapse.TabIndex = 8
        Me.btnExpandCollapse.Text = "Expand"
        Me.btnExpandCollapse.UseVisualStyleBackColor = True
        '
        'pnlControls
        '
        Me.pnlControls.Controls.Add(Me.cbReplaceMode)
        Me.pnlControls.Controls.Add(Me.Label3)
        Me.pnlControls.Controls.Add(Me.btnExpandCollapse)
        Me.pnlControls.Controls.Add(Me.lblStatus)
        Me.pnlControls.Controls.Add(Me.cbMatchCase)
        Me.pnlControls.Controls.Add(Me.cbRegex)
        Me.pnlControls.Controls.Add(Me.pnlReplace)
        Me.pnlControls.Controls.Add(Me.btnReplaceAll)
        Me.pnlControls.Controls.Add(Me.btnReplace)
        Me.pnlControls.Controls.Add(Me.btnFindAll)
        Me.pnlControls.Controls.Add(Me.btnFindPrevious)
        Me.pnlControls.Controls.Add(Me.btnFindNext)
        Me.pnlControls.Controls.Add(Me.tbFindContents)
        Me.pnlControls.Controls.Add(Me.Label1)
        Me.pnlControls.Controls.Add(Me.cbPin)
        Me.pnlControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlControls.Location = New System.Drawing.Point(0, 0)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(568, 192)
        Me.pnlControls.TabIndex = 9
        '
        'cbReplaceMode
        '
        Me.cbReplaceMode.Appearance = System.Windows.Forms.Appearance.Button
        Me.cbReplaceMode.AutoSize = True
        Me.cbReplaceMode.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbReplaceMode.Location = New System.Drawing.Point(12, 156)
        Me.cbReplaceMode.Name = "cbReplaceMode"
        Me.cbReplaceMode.Size = New System.Drawing.Size(84, 23)
        Me.cbReplaceMode.TabIndex = 10
        Me.cbReplaceMode.Text = "Replace Mode"
        Me.cbReplaceMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cbReplaceMode.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(352, 103)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(118, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Replace this entry? -->"
        Me.Label3.Visible = False
        '
        'FindReplaceDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(568, 357)
        Me.Controls.Add(Me.lvResults)
        Me.Controls.Add(Me.pnlControls)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(576, 212)
        Me.Name = "FindReplaceDialog"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Find in Text"
        Me.pnlReplace.ResumeLayout(False)
        Me.pnlReplace.PerformLayout()
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cbPin As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbFindContents As System.Windows.Forms.TextBox
    Friend WithEvents btnFindNext As System.Windows.Forms.Button
    Friend WithEvents btnFindPrevious As System.Windows.Forms.Button
    Friend WithEvents btnFindAll As System.Windows.Forms.Button
    Friend WithEvents pnlReplace As System.Windows.Forms.Panel
    Friend WithEvents cbRegex As System.Windows.Forms.CheckBox
    Friend WithEvents cbMatchCase As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbReplaceContents As System.Windows.Forms.TextBox
    Friend WithEvents btnReplace As System.Windows.Forms.Button
    Friend WithEvents btnReplaceAll As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lvResults As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnExpandCollapse As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbReplaceMode As System.Windows.Forms.CheckBox
End Class
