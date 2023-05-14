<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnControlPanel = New System.Windows.Forms.Panel()
        Me.wndControlPanel = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.minBox = New System.Windows.Forms.PictureBox()
        Me.closeBox = New System.Windows.Forms.PictureBox()
        Me.WelcomePanel = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.UpdatePanel = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.FinishPanel = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.ReleaseFetcherBW = New System.ComponentModel.BackgroundWorker()
        Me.UpdaterBW = New System.ComponentModel.BackgroundWorker()
        Me.btnControlPanel.SuspendLayout()
        Me.wndControlPanel.SuspendLayout()
        CType(Me.minBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.closeBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WelcomePanel.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.UpdatePanel.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FinishPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(12, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(264, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "DISMTools Update Check System - Version <ver>"
        '
        'btnControlPanel
        '
        Me.btnControlPanel.BackColor = System.Drawing.Color.Transparent
        Me.btnControlPanel.BackgroundImage = Global.DISMTools_UCS.My.Resources.Resources.wndPanel_Backdrop
        Me.btnControlPanel.Controls.Add(Me.Label1)
        Me.btnControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnControlPanel.Location = New System.Drawing.Point(0, 664)
        Me.btnControlPanel.Name = "btnControlPanel"
        Me.btnControlPanel.Size = New System.Drawing.Size(960, 48)
        Me.btnControlPanel.TabIndex = 3
        '
        'wndControlPanel
        '
        Me.wndControlPanel.BackColor = System.Drawing.Color.Transparent
        Me.wndControlPanel.BackgroundImage = Global.DISMTools_UCS.My.Resources.Resources.wndPanel_Backdrop
        Me.wndControlPanel.Controls.Add(Me.Label2)
        Me.wndControlPanel.Controls.Add(Me.minBox)
        Me.wndControlPanel.Controls.Add(Me.closeBox)
        Me.wndControlPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.wndControlPanel.Location = New System.Drawing.Point(0, 0)
        Me.wndControlPanel.Name = "wndControlPanel"
        Me.wndControlPanel.Size = New System.Drawing.Size(960, 48)
        Me.wndControlPanel.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(8, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Product updates"
        '
        'minBox
        '
        Me.minBox.Image = Global.DISMTools_UCS.My.Resources.Resources.minBox
        Me.minBox.Location = New System.Drawing.Point(869, 0)
        Me.minBox.Name = "minBox"
        Me.minBox.Size = New System.Drawing.Size(46, 32)
        Me.minBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.minBox.TabIndex = 0
        Me.minBox.TabStop = False
        '
        'closeBox
        '
        Me.closeBox.Image = Global.DISMTools_UCS.My.Resources.Resources.closebox
        Me.closeBox.Location = New System.Drawing.Point(914, 0)
        Me.closeBox.Name = "closeBox"
        Me.closeBox.Size = New System.Drawing.Size(46, 32)
        Me.closeBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.closeBox.TabIndex = 0
        Me.closeBox.TabStop = False
        '
        'WelcomePanel
        '
        Me.WelcomePanel.BackColor = System.Drawing.Color.Transparent
        Me.WelcomePanel.Controls.Add(Me.Panel1)
        Me.WelcomePanel.Controls.Add(Me.Label4)
        Me.WelcomePanel.Controls.Add(Me.ProgressBar1)
        Me.WelcomePanel.Controls.Add(Me.Label3)
        Me.WelcomePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WelcomePanel.Location = New System.Drawing.Point(0, 48)
        Me.WelcomePanel.Name = "WelcomePanel"
        Me.WelcomePanel.Size = New System.Drawing.Size(960, 616)
        Me.WelcomePanel.TabIndex = 4
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.LinkLabel1)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Location = New System.Drawing.Point(30, 68)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(900, 475)
        Me.Panel1.TabIndex = 4
        Me.Panel1.Visible = False
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(810, 439)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Update"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel1.Location = New System.Drawing.Point(19, 439)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(103, 15)
        Me.LinkLabel1.TabIndex = 1
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "View release notes"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(0, 42)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(900, 42)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Version information"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(16, 96)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(538, 15)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Please close any open DISMTools windows, while saving any projects loaded, and th" & _
    "en click ""Update"""
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(16, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(328, 15)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "There is a new version available to download and installation:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(27, 546)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 15)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Progress"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(30, 568)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(900, 23)
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.ProgressBar1.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI Variable Display Semib", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(24, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(267, 32)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Checking for updates..."
        '
        'UpdatePanel
        '
        Me.UpdatePanel.BackColor = System.Drawing.Color.Transparent
        Me.UpdatePanel.Controls.Add(Me.PictureBox1)
        Me.UpdatePanel.Controls.Add(Me.PictureBox2)
        Me.UpdatePanel.Controls.Add(Me.PictureBox3)
        Me.UpdatePanel.Controls.Add(Me.PictureBox4)
        Me.UpdatePanel.Controls.Add(Me.CheckBox1)
        Me.UpdatePanel.Controls.Add(Me.Label13)
        Me.UpdatePanel.Controls.Add(Me.Label12)
        Me.UpdatePanel.Controls.Add(Me.Label11)
        Me.UpdatePanel.Controls.Add(Me.Label10)
        Me.UpdatePanel.Controls.Add(Me.Label14)
        Me.UpdatePanel.Controls.Add(Me.Label9)
        Me.UpdatePanel.Controls.Add(Me.Label8)
        Me.UpdatePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UpdatePanel.Location = New System.Drawing.Point(0, 48)
        Me.UpdatePanel.Name = "UpdatePanel"
        Me.UpdatePanel.Size = New System.Drawing.Size(960, 616)
        Me.UpdatePanel.TabIndex = 4
        Me.UpdatePanel.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DISMTools_UCS.My.Resources.Resources.check
        Me.PictureBox1.Location = New System.Drawing.Point(58, 123)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.DISMTools_UCS.My.Resources.Resources.check
        Me.PictureBox2.Location = New System.Drawing.Point(58, 144)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 5
        Me.PictureBox2.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.DISMTools_UCS.My.Resources.Resources.check
        Me.PictureBox3.Location = New System.Drawing.Point(58, 165)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox3.TabIndex = 5
        Me.PictureBox3.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.DISMTools_UCS.My.Resources.Resources.check
        Me.PictureBox4.Location = New System.Drawing.Point(58, 186)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox4.TabIndex = 5
        Me.PictureBox4.TabStop = False
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(49, 568)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(129, 19)
        Me.CheckBox1.TabIndex = 4
        Me.CheckBox1.Text = "Launch when ready"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.Color.Gray
        Me.Label13.Location = New System.Drawing.Point(88, 191)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(156, 15)
        Me.Label13.TabIndex = 3
        Me.Label13.Text = "Finishing update installation"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Gray
        Me.Label12.Location = New System.Drawing.Point(88, 170)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(115, 15)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "Installing the update"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.Gray
        Me.Label11.Location = New System.Drawing.Point(88, 149)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(159, 15)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "Preparing update installation"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Gray
        Me.Label10.Location = New System.Drawing.Point(88, 128)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(138, 15)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "Downloading the update"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(46, 511)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(227, 15)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = "The update may take some time to install."
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(46, 84)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(422, 15)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Please wait while we update your copy of DISMTools. This may take some time."
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI Variable Display Semib", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(24, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(255, 32)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Updating DISMTools..."
        '
        'FinishPanel
        '
        Me.FinishPanel.BackColor = System.Drawing.Color.Transparent
        Me.FinishPanel.Controls.Add(Me.Button2)
        Me.FinishPanel.Controls.Add(Me.Label17)
        Me.FinishPanel.Controls.Add(Me.Label16)
        Me.FinishPanel.Controls.Add(Me.Label15)
        Me.FinishPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FinishPanel.Location = New System.Drawing.Point(0, 48)
        Me.FinishPanel.Name = "FinishPanel"
        Me.FinishPanel.Size = New System.Drawing.Size(960, 616)
        Me.FinishPanel.TabIndex = 4
        Me.FinishPanel.Visible = False
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button2.Location = New System.Drawing.Point(840, 507)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Launch"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(46, 478)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(677, 15)
        Me.Label17.TabIndex = 4
        Me.Label17.Text = "This version may come with new settings you may not have set previously. Your old" & _
    " settings file will be migrated to this version."
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(46, 84)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(499, 15)
        Me.Label16.TabIndex = 4
        Me.Label16.Text = "DISMTools has been updated successfully. You can now enjoy the new features of th" & _
    "is release."
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Segoe UI Variable Display Semib", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(24, 24)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(203, 32)
        Me.Label15.TabIndex = 3
        Me.Label15.Text = "Update complete"
        '
        'ReleaseFetcherBW
        '
        Me.ReleaseFetcherBW.WorkerReportsProgress = True
        '
        'UpdaterBW
        '
        Me.UpdaterBW.WorkerReportsProgress = True
        Me.UpdaterBW.WorkerSupportsCancellation = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.DISMTools_UCS.My.Resources.Resources.dt_bgbranding
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(960, 712)
        Me.Controls.Add(Me.WelcomePanel)
        Me.Controls.Add(Me.UpdatePanel)
        Me.Controls.Add(Me.FinishPanel)
        Me.Controls.Add(Me.btnControlPanel)
        Me.Controls.Add(Me.wndControlPanel)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Product updates"
        Me.btnControlPanel.ResumeLayout(False)
        Me.btnControlPanel.PerformLayout()
        Me.wndControlPanel.ResumeLayout(False)
        Me.wndControlPanel.PerformLayout()
        CType(Me.minBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.closeBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WelcomePanel.ResumeLayout(False)
        Me.WelcomePanel.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.UpdatePanel.ResumeLayout(False)
        Me.UpdatePanel.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FinishPanel.ResumeLayout(False)
        Me.FinishPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnControlPanel As System.Windows.Forms.Panel
    Friend WithEvents wndControlPanel As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents minBox As System.Windows.Forms.PictureBox
    Friend WithEvents closeBox As System.Windows.Forms.PictureBox
    Friend WithEvents WelcomePanel As System.Windows.Forms.Panel
    Friend WithEvents UpdatePanel As System.Windows.Forms.Panel
    Friend WithEvents FinishPanel As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents ReleaseFetcherBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents UpdaterBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label

End Class
