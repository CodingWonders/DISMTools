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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.logActions = New System.Windows.Forms.Label()
        Me.LogButton = New System.Windows.Forms.Button()
        Me.ProgressBW = New System.ComponentModel.BackgroundWorker()
        Me.BodyPanel = New System.Windows.Forms.Panel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.DISMProc = New System.Diagnostics.Process()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.BodyPanel.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.Label1.Location = New System.Drawing.Point(51, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(298, 30)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Image operations in progress..."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(55, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(354, 13)
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
        Me.GroupBox1.Location = New System.Drawing.Point(58, 238)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(696, 153)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Log"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.LogView)
        Me.Panel1.Controls.Add(Me.Panel2)
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
        Me.LogView.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogView.Location = New System.Drawing.Point(0, 0)
        Me.LogView.Multiline = True
        Me.LogView.Name = "LogView"
        Me.LogView.ReadOnly = True
        Me.LogView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.LogView.Size = New System.Drawing.Size(690, 110)
        Me.LogView.TabIndex = 2
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ToolStrip1)
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.logActions)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 110)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(690, 23)
        Me.Panel2.TabIndex = 1
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStrip1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1})
        Me.ToolStrip1.Location = New System.Drawing.Point(65, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStrip1.Size = New System.Drawing.Size(425, 23)
        Me.ToolStrip1.TabIndex = 4
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.DISMTools.My.Resources.Resources.save_glyph
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 20)
        Me.ToolStripButton1.Text = "Save"
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.ToolStrip2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel3.Location = New System.Drawing.Point(490, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(200, 23)
        Me.Panel3.TabIndex = 3
        '
        'ToolStrip2
        '
        Me.ToolStrip2.CanOverflow = False
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStrip2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton2, Me.ToolStripLabel1, Me.ToolStripLabel2})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStrip2.Size = New System.Drawing.Size(198, 21)
        Me.ToolStrip2.TabIndex = 5
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = Global.DISMTools.My.Resources.Resources.hide_glyph
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(23, 18)
        Me.ToolStripButton2.Text = "ToolStripButton2"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.BackColor = System.Drawing.SystemColors.Control
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(83, 18)
        Me.ToolStripLabel1.Text = "Warnings: warn"
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.BackColor = System.Drawing.SystemColors.Control
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(57, 18)
        Me.ToolStripLabel2.Text = "Errors: err"
        '
        'logActions
        '
        Me.logActions.BackColor = System.Drawing.SystemColors.Control
        Me.logActions.Dock = System.Windows.Forms.DockStyle.Left
        Me.logActions.Location = New System.Drawing.Point(0, 0)
        Me.logActions.Name = "logActions"
        Me.logActions.Size = New System.Drawing.Size(65, 23)
        Me.logActions.TabIndex = 2
        Me.logActions.Text = "Log actions:"
        Me.logActions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LogButton
        '
        Me.LogButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LogButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.LogButton.Location = New System.Drawing.Point(56, 205)
        Me.LogButton.Name = "LogButton"
        Me.LogButton.Size = New System.Drawing.Size(75, 23)
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
        Me.LinkLabel1.Location = New System.Drawing.Point(137, 210)
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
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
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
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents logActions As System.Windows.Forms.Label
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents LogButton As System.Windows.Forms.Button
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ProgressBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents LogView As System.Windows.Forms.TextBox
    Friend WithEvents BodyPanel As System.Windows.Forms.Panel
    Friend WithEvents DISMProc As System.Diagnostics.Process
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
End Class
