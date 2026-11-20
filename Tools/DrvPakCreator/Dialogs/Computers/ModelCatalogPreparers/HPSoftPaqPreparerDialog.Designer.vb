<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HPSoftPaqPreparerDialog
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
        Me.components = New System.ComponentModel.Container()
        Me.StatusLbl = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tmrDownloadStatus = New System.Windows.Forms.Timer(Me.components)
        Me.bwPakUnpack = New System.ComponentModel.BackgroundWorker()
        Me.ProgressTask2 = New DrvPakCreator.ProgressTask()
        Me.ProgressTask1 = New DrvPakCreator.ProgressTask()
        Me.SuspendLayout()
        '
        'StatusLbl
        '
        Me.StatusLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatusLbl.AutoEllipsis = True
        Me.StatusLbl.Location = New System.Drawing.Point(16, 131)
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(606, 13)
        Me.StatusLbl.TabIndex = 9
        Me.StatusLbl.Text = "Progress"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(565, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Please wait while we download the SoftPAQ. This will take some time, depending on" & _
    " your network connection speed."
        '
        'tmrDownloadStatus
        '
        Me.tmrDownloadStatus.Enabled = True
        Me.tmrDownloadStatus.Interval = 500
        '
        'bwPakUnpack
        '
        Me.bwPakUnpack.WorkerReportsProgress = True
        Me.bwPakUnpack.WorkerSupportsCancellation = True
        '
        'ProgressTask2
        '
        Me.ProgressTask2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProgressTask2.Location = New System.Drawing.Point(51, 81)
        Me.ProgressTask2.Name = "ProgressTask2"
        Me.ProgressTask2.Size = New System.Drawing.Size(401, 24)
        Me.ProgressTask2.TabIndex = 7
        Me.ProgressTask2.TaskName = "Extract SoftPAQ"
        Me.ProgressTask2.TaskPercentage = -1.0R
        Me.ProgressTask2.TaskStatus = DrvPakCreator.ProgressTaskStatus.NotStarted
        '
        'ProgressTask1
        '
        Me.ProgressTask1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ProgressTask1.Location = New System.Drawing.Point(51, 51)
        Me.ProgressTask1.Name = "ProgressTask1"
        Me.ProgressTask1.Size = New System.Drawing.Size(401, 24)
        Me.ProgressTask1.TabIndex = 8
        Me.ProgressTask1.TaskName = "Download SoftPAQ"
        Me.ProgressTask1.TaskPercentage = -1.0R
        Me.ProgressTask1.TaskStatus = DrvPakCreator.ProgressTaskStatus.NotStarted
        '
        'HPSoftPaqPreparerDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(634, 167)
        Me.Controls.Add(Me.StatusLbl)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressTask2)
        Me.Controls.Add(Me.ProgressTask1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "HPSoftPaqPreparerDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Scanning SoftPAQ..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusLbl As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ProgressTask2 As DrvPakCreator.ProgressTask
    Friend WithEvents ProgressTask1 As DrvPakCreator.ProgressTask
    Friend WithEvents tmrDownloadStatus As System.Windows.Forms.Timer
    Friend WithEvents bwPakUnpack As System.ComponentModel.BackgroundWorker

End Class
