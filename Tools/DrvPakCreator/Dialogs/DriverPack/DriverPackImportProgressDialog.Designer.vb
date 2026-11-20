<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DriverPackImportProgressDialog
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
        Me.lblBuildStatus = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.bwPakUnpack = New System.ComponentModel.BackgroundWorker
        Me.tmrTimeTaken = New System.Windows.Forms.Timer(Me.components)
        Me.lblTimeTaken = New System.Windows.Forms.Label
        Me.ProgressTask1 = New DrvPakCreator.ProgressTask
        Me.SuspendLayout()
        '
        'lblBuildStatus
        '
        Me.lblBuildStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBuildStatus.AutoEllipsis = True
        Me.lblBuildStatus.Location = New System.Drawing.Point(16, 99)
        Me.lblBuildStatus.Name = "lblBuildStatus"
        Me.lblBuildStatus.Size = New System.Drawing.Size(508, 13)
        Me.lblBuildStatus.TabIndex = 4
        Me.lblBuildStatus.Text = "Progress"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(369, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Please wait while the driver pack is being imported. This will take some time."
        '
        'bwPakUnpack
        '
        Me.bwPakUnpack.WorkerReportsProgress = True
        '
        'tmrTimeTaken
        '
        '
        'lblTimeTaken
        '
        Me.lblTimeTaken.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTimeTaken.AutoEllipsis = True
        Me.lblTimeTaken.Location = New System.Drawing.Point(530, 99)
        Me.lblTimeTaken.Name = "lblTimeTaken"
        Me.lblTimeTaken.Size = New System.Drawing.Size(87, 13)
        Me.lblTimeTaken.TabIndex = 6
        Me.lblTimeTaken.Text = "00:00:00"
        Me.lblTimeTaken.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ProgressTask1
        '
        Me.ProgressTask1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProgressTask1.Location = New System.Drawing.Point(50, 50)
        Me.ProgressTask1.Name = "ProgressTask1"
        Me.ProgressTask1.Size = New System.Drawing.Size(401, 24)
        Me.ProgressTask1.TabIndex = 2
        Me.ProgressTask1.TaskName = "Unpack Driver Pack"
        Me.ProgressTask1.TaskPercentage = -1
        Me.ProgressTask1.TaskStatus = DrvPakCreator.ProgressTaskStatus.NotStarted
        '
        'DriverPackImportProgressDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(634, 135)
        Me.Controls.Add(Me.lblTimeTaken)
        Me.Controls.Add(Me.lblBuildStatus)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressTask1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DriverPackImportProgressDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create Driver Pack Project from Existing Driver Pack"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblBuildStatus As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ProgressTask1 As DrvPakCreator.ProgressTask
    Friend WithEvents bwPakUnpack As System.ComponentModel.BackgroundWorker
    Friend WithEvents tmrTimeTaken As System.Windows.Forms.Timer
    Friend WithEvents lblTimeTaken As System.Windows.Forms.Label

End Class
