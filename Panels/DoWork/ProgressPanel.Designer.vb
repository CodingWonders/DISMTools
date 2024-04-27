<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProgressPanel
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProgressPanel))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.CurrentPB = New System.Windows.Forms.ProgressBar()
        Me.AllPB = New System.Windows.Forms.ProgressBar()
        Me.currentTask = New System.Windows.Forms.Label()
        Me.allTasks = New System.Windows.Forms.Label()
        Me.taskCountLbl = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LogView = New System.Windows.Forms.TextBox()
        Me.LogButton = New System.Windows.Forms.Button()
        Me.ProgressBW = New System.ComponentModel.BackgroundWorker()
        Me.BodyPanel = New System.Windows.Forms.Panel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.DISMProc = New System.Diagnostics.Process()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.BodyPanel.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoEllipsis = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.Label1.Location = New System.Drawing.Point(51, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(703, 30)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Image operations in progress..."
        '
        'Label2
        '
        Me.Label2.AutoEllipsis = True
        Me.Label2.Location = New System.Drawing.Point(55, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(699, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Please wait while the following tasks are done. This may take some time."
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Cancel_Button.Location = New System.Drawing.Point(677, 205)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(75, 23)
        Me.Cancel_Button.TabIndex = 3
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'CurrentPB
        '
        Me.CurrentPB.Location = New System.Drawing.Point(58, 108)
        Me.CurrentPB.Name = "CurrentPB"
        Me.CurrentPB.Size = New System.Drawing.Size(696, 23)
        Me.CurrentPB.TabIndex = 4
        '
        'AllPB
        '
        Me.AllPB.Location = New System.Drawing.Point(58, 166)
        Me.AllPB.Name = "AllPB"
        Me.AllPB.Size = New System.Drawing.Size(696, 23)
        Me.AllPB.TabIndex = 4
        '
        'currentTask
        '
        Me.currentTask.AutoEllipsis = True
        Me.currentTask.Location = New System.Drawing.Point(55, 84)
        Me.currentTask.Name = "currentTask"
        Me.currentTask.Size = New System.Drawing.Size(699, 13)
        Me.currentTask.TabIndex = 5
        Me.currentTask.Text = "currentTask"
        '
        'allTasks
        '
        Me.allTasks.AutoEllipsis = True
        Me.allTasks.Location = New System.Drawing.Point(55, 144)
        Me.allTasks.Name = "allTasks"
        Me.allTasks.Size = New System.Drawing.Size(581, 13)
        Me.allTasks.TabIndex = 5
        Me.allTasks.Text = "allTasks"
        '
        'taskCountLbl
        '
        Me.taskCountLbl.AutoEllipsis = True
        Me.taskCountLbl.Location = New System.Drawing.Point(642, 144)
        Me.taskCountLbl.Name = "taskCountLbl"
        Me.taskCountLbl.Size = New System.Drawing.Size(112, 13)
        Me.taskCountLbl.TabIndex = 5
        Me.taskCountLbl.Text = "Tasks: {currentTCont} of {taskCount}"
        Me.taskCountLbl.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Location = New System.Drawing.Point(58, 242)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(696, 153)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Log"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.LogView)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 17)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(690, 133)
        Me.Panel1.TabIndex = 0
        '
        'LogView
        '
        Me.LogView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.LogView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LogView.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogView.Location = New System.Drawing.Point(0, 0)
        Me.LogView.Multiline = True
        Me.LogView.Name = "LogView"
        Me.LogView.ReadOnly = True
        Me.LogView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.LogView.Size = New System.Drawing.Size(690, 133)
        Me.LogView.TabIndex = 2
        '
        'LogButton
        '
        Me.LogButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LogButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.LogButton.Location = New System.Drawing.Point(56, 205)
        Me.LogButton.Name = "LogButton"
        Me.LogButton.Size = New System.Drawing.Size(116, 23)
        Me.LogButton.TabIndex = 3
        Me.LogButton.Text = "Show log"
        Me.LogButton.UseVisualStyleBackColor = True
        '
        'ProgressBW
        '
        Me.ProgressBW.WorkerReportsProgress = True
        Me.ProgressBW.WorkerSupportsCancellation = True
        '
        'BodyPanel
        '
        Me.BodyPanel.BackColor = System.Drawing.Color.White
        Me.BodyPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.BodyPanel.Controls.Add(Me.LinkLabel1)
        Me.BodyPanel.Controls.Add(Me.PictureBox1)
        Me.BodyPanel.Controls.Add(Me.Label1)
        Me.BodyPanel.Controls.Add(Me.Label2)
        Me.BodyPanel.Controls.Add(Me.Cancel_Button)
        Me.BodyPanel.Controls.Add(Me.LogButton)
        Me.BodyPanel.Controls.Add(Me.CurrentPB)
        Me.BodyPanel.Controls.Add(Me.AllPB)
        Me.BodyPanel.Controls.Add(Me.currentTask)
        Me.BodyPanel.Controls.Add(Me.allTasks)
        Me.BodyPanel.Controls.Add(Me.taskCountLbl)
        Me.BodyPanel.Controls.Add(Me.GroupBox1)
        Me.BodyPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BodyPanel.Location = New System.Drawing.Point(0, 0)
        Me.BodyPanel.Name = "BodyPanel"
        Me.BodyPanel.Size = New System.Drawing.Size(800, 240)
        Me.BodyPanel.TabIndex = 7
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel1.Location = New System.Drawing.Point(178, 210)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(153, 13)
        Me.LinkLabel1.TabIndex = 7
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Show DISM log file (advanced)"
        Me.LinkLabel1.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DISMTools.My.Resources.Resources.imgoperation
        Me.PictureBox1.Location = New System.Drawing.Point(13, 13)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'DISMProc
        '
        Me.DISMProc.EnableRaisingEvents = True
        Me.DISMProc.StartInfo.Domain = ""
        Me.DISMProc.StartInfo.LoadUserProfile = False
        Me.DISMProc.StartInfo.Password = Nothing
        Me.DISMProc.StartInfo.StandardErrorEncoding = Nothing
        Me.DISMProc.StartInfo.StandardOutputEncoding = Nothing
        Me.DISMProc.StartInfo.UserName = ""
        Me.DISMProc.SynchronizingObject = Me
        '
        'ProgressPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 240)
        Me.ControlBox = False
        Me.Controls.Add(Me.BodyPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ProgressPanel"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Progress"
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.BodyPanel.ResumeLayout(False)
        Me.BodyPanel.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents CurrentPB As System.Windows.Forms.ProgressBar
    Friend WithEvents AllPB As System.Windows.Forms.ProgressBar
    Friend WithEvents currentTask As System.Windows.Forms.Label
    Friend WithEvents allTasks As System.Windows.Forms.Label
    Friend WithEvents taskCountLbl As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents LogButton As System.Windows.Forms.Button
    Friend WithEvents ProgressBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents LogView As System.Windows.Forms.TextBox
    Friend WithEvents BodyPanel As System.Windows.Forms.Panel
    Friend WithEvents DISMProc As System.Diagnostics.Process
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
End Class
