<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AIResultWindowPinDialog
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PMDetailLabel = New System.Windows.Forms.Label()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.OffsetDetailLabel = New System.Windows.Forms.Label()
        Me.BottomRightBtn = New System.Windows.Forms.Button()
        Me.BottomLeftBtn = New System.Windows.Forms.Button()
        Me.TopRightBtn = New System.Windows.Forms.Button()
        Me.TopLeftBtn = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoEllipsis = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(353, 36)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.PinDialog")("ChoosePosition.Message")
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Panel1
        '
        Me.Panel1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Panel1.BackColor = System.Drawing.Color.Black
        Me.Panel1.Controls.Add(Me.PMDetailLabel)
        Me.Panel1.Location = New System.Drawing.Point(93, 83)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(192, 128)
        Me.Panel1.TabIndex = 1
        '
        'PMDetailLabel
        '
        Me.PMDetailLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PMDetailLabel.AutoEllipsis = True
        Me.PMDetailLabel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PMDetailLabel.ForeColor = System.Drawing.Color.White
        Me.PMDetailLabel.Location = New System.Drawing.Point(30, 37)
        Me.PMDetailLabel.Name = "PMDetailLabel"
        Me.PMDetailLabel.Size = New System.Drawing.Size(133, 54)
        Me.PMDetailLabel.TabIndex = 4
        Me.PMDetailLabel.Text = LocalizationService.ForSection("StarterScript.Designer.PinDialog")("Monitor.Label")
        Me.PMDetailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Cancel_Button.Location = New System.Drawing.Point(102, 260)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(175, 23)
        Me.Cancel_Button.TabIndex = 2
        Me.Cancel_Button.Text = LocalizationService.ForSection("StarterScript.Designer.PinDialog")("ManualPosition.Button")
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'OffsetDetailLabel
        '
        Me.OffsetDetailLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OffsetDetailLabel.AutoEllipsis = True
        Me.OffsetDetailLabel.Location = New System.Drawing.Point(93, 218)
        Me.OffsetDetailLabel.Name = "OffsetDetailLabel"
        Me.OffsetDetailLabel.Size = New System.Drawing.Size(192, 32)
        Me.OffsetDetailLabel.TabIndex = 4
        Me.OffsetDetailLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'BottomRightBtn
        '
        Me.BottomRightBtn.BackColor = System.Drawing.SystemColors.Control
        Me.BottomRightBtn.BackgroundImage = Global.StarterScriptEditor.My.Resources.Resources.bottomright
        Me.BottomRightBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BottomRightBtn.Location = New System.Drawing.Point(290, 206)
        Me.BottomRightBtn.Name = "BottomRightBtn"
        Me.BottomRightBtn.Size = New System.Drawing.Size(36, 36)
        Me.BottomRightBtn.TabIndex = 3
        Me.BottomRightBtn.UseVisualStyleBackColor = False
        '
        'BottomLeftBtn
        '
        Me.BottomLeftBtn.BackColor = System.Drawing.SystemColors.Control
        Me.BottomLeftBtn.BackgroundImage = Global.StarterScriptEditor.My.Resources.Resources.bottomleft
        Me.BottomLeftBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BottomLeftBtn.Location = New System.Drawing.Point(52, 206)
        Me.BottomLeftBtn.Name = "BottomLeftBtn"
        Me.BottomLeftBtn.Size = New System.Drawing.Size(36, 36)
        Me.BottomLeftBtn.TabIndex = 3
        Me.BottomLeftBtn.UseVisualStyleBackColor = False
        '
        'TopRightBtn
        '
        Me.TopRightBtn.BackColor = System.Drawing.SystemColors.Control
        Me.TopRightBtn.BackgroundImage = Global.StarterScriptEditor.My.Resources.Resources.topright
        Me.TopRightBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.TopRightBtn.Location = New System.Drawing.Point(290, 52)
        Me.TopRightBtn.Name = "TopRightBtn"
        Me.TopRightBtn.Size = New System.Drawing.Size(36, 36)
        Me.TopRightBtn.TabIndex = 3
        Me.TopRightBtn.UseVisualStyleBackColor = False
        '
        'TopLeftBtn
        '
        Me.TopLeftBtn.BackColor = System.Drawing.SystemColors.Control
        Me.TopLeftBtn.BackgroundImage = Global.StarterScriptEditor.My.Resources.Resources.topleft
        Me.TopLeftBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.TopLeftBtn.Location = New System.Drawing.Point(52, 52)
        Me.TopLeftBtn.Name = "TopLeftBtn"
        Me.TopLeftBtn.Size = New System.Drawing.Size(36, 36)
        Me.TopLeftBtn.TabIndex = 3
        Me.TopLeftBtn.UseVisualStyleBackColor = False
        '
        'AIResultWindowPinDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(378, 295)
        Me.Controls.Add(Me.OffsetDetailLabel)
        Me.Controls.Add(Me.BottomRightBtn)
        Me.Controls.Add(Me.BottomLeftBtn)
        Me.Controls.Add(Me.TopRightBtn)
        Me.Controls.Add(Me.TopLeftBtn)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AIResultWindowPinDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.PinDialog")("Title")
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TopLeftBtn As System.Windows.Forms.Button
    Friend WithEvents TopRightBtn As System.Windows.Forms.Button
    Friend WithEvents BottomLeftBtn As System.Windows.Forms.Button
    Friend WithEvents BottomRightBtn As System.Windows.Forms.Button
    Friend WithEvents OffsetDetailLabel As System.Windows.Forms.Label
    Friend WithEvents PMDetailLabel As System.Windows.Forms.Label

End Class
