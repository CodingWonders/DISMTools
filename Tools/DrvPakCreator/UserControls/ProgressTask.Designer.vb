<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProgressTask
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.lblTaskName = New System.Windows.Forms.Label
        Me.pbTaskStatus = New System.Windows.Forms.PictureBox
        CType(Me.pbTaskStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTaskName
        '
        Me.lblTaskName.AutoEllipsis = True
        Me.lblTaskName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTaskName.Location = New System.Drawing.Point(24, 0)
        Me.lblTaskName.Name = "lblTaskName"
        Me.lblTaskName.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.lblTaskName.Size = New System.Drawing.Size(377, 24)
        Me.lblTaskName.TabIndex = 0
        Me.lblTaskName.Text = "Task Name"
        Me.lblTaskName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pbTaskStatus
        '
        Me.pbTaskStatus.Dock = System.Windows.Forms.DockStyle.Left
        Me.pbTaskStatus.Location = New System.Drawing.Point(0, 0)
        Me.pbTaskStatus.Name = "pbTaskStatus"
        Me.pbTaskStatus.Size = New System.Drawing.Size(24, 24)
        Me.pbTaskStatus.TabIndex = 1
        Me.pbTaskStatus.TabStop = False
        '
        'ProgressTask
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.lblTaskName)
        Me.Controls.Add(Me.pbTaskStatus)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "ProgressTask"
        Me.Size = New System.Drawing.Size(401, 24)
        CType(Me.pbTaskStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents lblTaskName As System.Windows.Forms.Label
    Private WithEvents pbTaskStatus As System.Windows.Forms.PictureBox

End Class
