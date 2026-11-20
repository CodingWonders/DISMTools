<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DriverReindexProgressDialog
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
        Me.ProgressTask1 = New DrvPakCreator.ProgressTask
        Me.ProgressTask2 = New DrvPakCreator.ProgressTask
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(345, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Please wait while drivers are being reindexed. This will take some time."
        '
        'ProgressTask1
        '
        Me.ProgressTask1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProgressTask1.Location = New System.Drawing.Point(48, 48)
        Me.ProgressTask1.Name = "ProgressTask1"
        Me.ProgressTask1.Size = New System.Drawing.Size(401, 24)
        Me.ProgressTask1.TabIndex = 3
        Me.ProgressTask1.TaskName = "Reindex drivers in metadata"
        Me.ProgressTask1.TaskPercentage = -1
        Me.ProgressTask1.TaskStatus = DrvPakCreator.ProgressTaskStatus.NotStarted
        '
        'ProgressTask2
        '
        Me.ProgressTask2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProgressTask2.Location = New System.Drawing.Point(48, 78)
        Me.ProgressTask2.Name = "ProgressTask2"
        Me.ProgressTask2.Size = New System.Drawing.Size(401, 24)
        Me.ProgressTask2.TabIndex = 3
        Me.ProgressTask2.TaskName = "Reorder driver files in project location"
        Me.ProgressTask2.TaskPercentage = -1
        Me.ProgressTask2.TaskStatus = DrvPakCreator.ProgressTaskStatus.NotStarted
        '
        'DriverReindexProgressDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(634, 135)
        Me.Controls.Add(Me.ProgressTask2)
        Me.Controls.Add(Me.ProgressTask1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DriverReindexProgressDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Reindexing Device Drivers"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ProgressTask1 As DrvPakCreator.ProgressTask
    Friend WithEvents ProgressTask2 As DrvPakCreator.ProgressTask

End Class
