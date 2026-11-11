<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DriverAdditionProgressDialog
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
        Me.ProgressTask1 = New DrvPakCreator.ProgressTask
        Me.ProgressTask2 = New DrvPakCreator.ProgressTask
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblAdditionStatus = New System.Windows.Forms.Label
        Me.bwFileCopy = New System.ComponentModel.BackgroundWorker
        Me.SuspendLayout()
        '
        'ProgressTask1
        '
        Me.ProgressTask1.Location = New System.Drawing.Point(48, 48)
        Me.ProgressTask1.Name = "ProgressTask1"
        Me.ProgressTask1.Size = New System.Drawing.Size(401, 24)
        Me.ProgressTask1.TabIndex = 0
        Me.ProgressTask1.TaskName = "Create Driver Folder"
        Me.ProgressTask1.TaskPercentage = -1
        Me.ProgressTask1.TaskStatus = DrvPakCreator.ProgressTaskStatus.NotStarted
        '
        'ProgressTask2
        '
        Me.ProgressTask2.Location = New System.Drawing.Point(48, 78)
        Me.ProgressTask2.Name = "ProgressTask2"
        Me.ProgressTask2.Size = New System.Drawing.Size(401, 24)
        Me.ProgressTask2.TabIndex = 0
        Me.ProgressTask2.TaskName = "Copy Driver Files From Source"
        Me.ProgressTask2.TaskPercentage = -1
        Me.ProgressTask2.TaskStatus = DrvPakCreator.ProgressTaskStatus.NotStarted
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(332, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Please wait while the driver is being added. This will take some time."
        '
        'lblAdditionStatus
        '
        Me.lblAdditionStatus.AutoSize = True
        Me.lblAdditionStatus.Location = New System.Drawing.Point(13, 128)
        Me.lblAdditionStatus.Name = "lblAdditionStatus"
        Me.lblAdditionStatus.Size = New System.Drawing.Size(49, 13)
        Me.lblAdditionStatus.TabIndex = 1
        Me.lblAdditionStatus.Text = "Progress"
        '
        'bwFileCopy
        '
        Me.bwFileCopy.WorkerReportsProgress = True
        '
        'DriverAdditionProgressDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(656, 157)
        Me.Controls.Add(Me.lblAdditionStatus)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressTask2)
        Me.Controls.Add(Me.ProgressTask1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DriverAdditionProgressDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Adding Device Driver..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ProgressTask1 As DrvPakCreator.ProgressTask
    Friend WithEvents ProgressTask2 As DrvPakCreator.ProgressTask
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblAdditionStatus As System.Windows.Forms.Label
    Friend WithEvents bwFileCopy As System.ComponentModel.BackgroundWorker

End Class
