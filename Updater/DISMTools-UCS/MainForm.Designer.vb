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
        Me.FinishPanel = New System.Windows.Forms.Panel()
        Me.ReleaseFetcherBW = New System.ComponentModel.BackgroundWorker()
        Me.btnControlPanel.SuspendLayout()
        Me.wndControlPanel.SuspendLayout()
        CType(Me.minBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.closeBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WelcomePanel.SuspendLayout()
        Me.Panel1.SuspendLayout()
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
        Me.Label5.Size = New System.Drawing.Size(301, 15)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "There is a new version available to download and install:"
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
        Me.UpdatePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UpdatePanel.Location = New System.Drawing.Point(0, 48)
        Me.UpdatePanel.Name = "UpdatePanel"
        Me.UpdatePanel.Size = New System.Drawing.Size(960, 616)
        Me.UpdatePanel.TabIndex = 4
        '
        'FinishPanel
        '
        Me.FinishPanel.BackColor = System.Drawing.Color.Transparent
        Me.FinishPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FinishPanel.Location = New System.Drawing.Point(0, 48)
        Me.FinishPanel.Name = "FinishPanel"
        Me.FinishPanel.Size = New System.Drawing.Size(960, 616)
        Me.FinishPanel.TabIndex = 4
        '
        'ReleaseFetcherBW
        '
        Me.ReleaseFetcherBW.WorkerReportsProgress = True
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

End Class
