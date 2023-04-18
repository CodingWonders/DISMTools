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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.WindowControls = New System.Windows.Forms.Panel()
        Me.minBox = New System.Windows.Forms.PictureBox()
        Me.closeBox = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PanelButtons = New System.Windows.Forms.Panel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.BodyContainer = New System.Windows.Forms.Panel()
        Me.InstMethod = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.WelcomePanel = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.EulaPanel = New System.Windows.Forms.Panel()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.PleaseWait = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
        Me.InstSettings = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.WindowControls.SuspendLayout()
        CType(Me.minBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.closeBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelButtons.SuspendLayout()
        Me.BodyContainer.SuspendLayout()
        Me.InstMethod.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WelcomePanel.SuspendLayout()
        Me.EulaPanel.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PleaseWait.SuspendLayout()
        Me.InstSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'WindowControls
        '
        Me.WindowControls.BackColor = System.Drawing.Color.Transparent
        Me.WindowControls.Controls.Add(Me.minBox)
        Me.WindowControls.Controls.Add(Me.closeBox)
        Me.WindowControls.Controls.Add(Me.Label1)
        Me.WindowControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.WindowControls.Location = New System.Drawing.Point(0, 0)
        Me.WindowControls.Name = "WindowControls"
        Me.WindowControls.Size = New System.Drawing.Size(1280, 96)
        Me.WindowControls.TabIndex = 0
        '
        'minBox
        '
        Me.minBox.Image = Global.DISMTools_Setup.My.Resources.Resources.minBox
        Me.minBox.Location = New System.Drawing.Point(1189, 0)
        Me.minBox.Name = "minBox"
        Me.minBox.Size = New System.Drawing.Size(46, 32)
        Me.minBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.minBox.TabIndex = 1
        Me.minBox.TabStop = False
        '
        'closeBox
        '
        Me.closeBox.Image = Global.DISMTools_Setup.My.Resources.Resources.closebox
        Me.closeBox.Location = New System.Drawing.Point(1234, 0)
        Me.closeBox.Name = "closeBox"
        Me.closeBox.Size = New System.Drawing.Size(46, 32)
        Me.closeBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.closeBox.TabIndex = 2
        Me.closeBox.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(32, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(262, 45)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "DISMTools Setup"
        '
        'PanelButtons
        '
        Me.PanelButtons.BackColor = System.Drawing.Color.Transparent
        Me.PanelButtons.Controls.Add(Me.LinkLabel1)
        Me.PanelButtons.Controls.Add(Me.Button3)
        Me.PanelButtons.Controls.Add(Me.Button2)
        Me.PanelButtons.Controls.Add(Me.Button1)
        Me.PanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelButtons.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PanelButtons.ForeColor = System.Drawing.Color.Black
        Me.PanelButtons.Location = New System.Drawing.Point(0, 664)
        Me.PanelButtons.Name = "PanelButtons"
        Me.PanelButtons.Size = New System.Drawing.Size(1280, 56)
        Me.PanelButtons.TabIndex = 1
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.MidnightBlue
        Me.LinkLabel1.Location = New System.Drawing.Point(20, 21)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(167, 15)
        Me.LinkLabel1.TabIndex = 1
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Manage a portable installation"
        '
        'Button3
        '
        Me.Button3.Enabled = False
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button3.Location = New System.Drawing.Point(1031, 17)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 0
        Me.Button3.Text = "Back"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button2.Location = New System.Drawing.Point(1112, 17)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 0
        Me.Button2.Text = "Next"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(1193, 17)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Cancel"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'BodyContainer
        '
        Me.BodyContainer.BackColor = System.Drawing.Color.Transparent
        Me.BodyContainer.Controls.Add(Me.WelcomePanel)
        Me.BodyContainer.Controls.Add(Me.EulaPanel)
        Me.BodyContainer.Controls.Add(Me.InstMethod)
        Me.BodyContainer.Controls.Add(Me.InstSettings)
        Me.BodyContainer.Controls.Add(Me.PleaseWait)
        Me.BodyContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BodyContainer.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BodyContainer.Location = New System.Drawing.Point(0, 96)
        Me.BodyContainer.Name = "BodyContainer"
        Me.BodyContainer.Size = New System.Drawing.Size(1280, 568)
        Me.BodyContainer.TabIndex = 2
        '
        'InstMethod
        '
        Me.InstMethod.Controls.Add(Me.LinkLabel3)
        Me.InstMethod.Controls.Add(Me.Label9)
        Me.InstMethod.Controls.Add(Me.Label7)
        Me.InstMethod.Controls.Add(Me.Label8)
        Me.InstMethod.Controls.Add(Me.Label6)
        Me.InstMethod.Controls.Add(Me.PictureBox3)
        Me.InstMethod.Controls.Add(Me.PictureBox2)
        Me.InstMethod.Controls.Add(Me.Label5)
        Me.InstMethod.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InstMethod.Location = New System.Drawing.Point(0, 0)
        Me.InstMethod.Name = "InstMethod"
        Me.InstMethod.Size = New System.Drawing.Size(1280, 568)
        Me.InstMethod.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(375, 126)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(603, 62)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "This method copies the program files to your local disk and lets you configure mo" & _
    "re stuff. Missing dependencies will be installed automatically."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(373, 94)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(324, 30)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Install DISMTools to the local disk"
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.DISMTools_Setup.My.Resources.Resources.portable_inst
        Me.PictureBox3.Location = New System.Drawing.Point(302, 215)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox3.TabIndex = 3
        Me.PictureBox3.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.DISMTools_Setup.My.Resources.Resources.local_inst
        Me.PictureBox2.Location = New System.Drawing.Point(302, 94)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 3
        Me.PictureBox2.TabStop = False
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(40, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(1200, 51)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "How do you want to install DISMTools?"
        '
        'WelcomePanel
        '
        Me.WelcomePanel.Controls.Add(Me.Label3)
        Me.WelcomePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WelcomePanel.Location = New System.Drawing.Point(0, 0)
        Me.WelcomePanel.Name = "WelcomePanel"
        Me.WelcomePanel.Size = New System.Drawing.Size(1280, 568)
        Me.WelcomePanel.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(40, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(1200, 104)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "This wizard will guide you in the steps of configuring a local or a portable DISM" & _
    "Tools installation. To begin setting things up, click Next."
        '
        'EulaPanel
        '
        Me.EulaPanel.Controls.Add(Me.LinkLabel2)
        Me.EulaPanel.Controls.Add(Me.PictureBox1)
        Me.EulaPanel.Controls.Add(Me.RichTextBox1)
        Me.EulaPanel.Controls.Add(Me.Label4)
        Me.EulaPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EulaPanel.Location = New System.Drawing.Point(0, 0)
        Me.EulaPanel.Name = "EulaPanel"
        Me.EulaPanel.Size = New System.Drawing.Size(1280, 568)
        Me.EulaPanel.TabIndex = 1
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel2.LinkColor = System.Drawing.Color.MidnightBlue
        Me.LinkLabel2.Location = New System.Drawing.Point(82, 492)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(171, 21)
        Me.LinkLabel2.TabIndex = 4
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "Printer-friendly version"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DISMTools_Setup.My.Resources.Resources.print
        Me.PictureBox1.Location = New System.Drawing.Point(44, 487)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'RichTextBox1
        '
        Me.RichTextBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(10, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.RichTextBox1.ForeColor = System.Drawing.Color.White
        Me.RichTextBox1.Location = New System.Drawing.Point(44, 96)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ReadOnly = True
        Me.RichTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.RichTextBox1.Size = New System.Drawing.Size(1196, 384)
        Me.RichTextBox1.TabIndex = 2
        Me.RichTextBox1.Text = ""
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(40, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(1200, 71)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Please read the end-user license agreement and click Next when you've finished:"
        '
        'PleaseWait
        '
        Me.PleaseWait.Controls.Add(Me.Label2)
        Me.PleaseWait.Controls.Add(Me.ProgressBar1)
        Me.PleaseWait.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PleaseWait.Location = New System.Drawing.Point(0, 0)
        Me.PleaseWait.Name = "PleaseWait"
        Me.PleaseWait.Size = New System.Drawing.Size(1280, 568)
        Me.PleaseWait.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 500)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 21)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Label2"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 529)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(1256, 27)
        Me.ProgressBar1.TabIndex = 0
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(373, 215)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(280, 30)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Create a portable installation"
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(375, 247)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(603, 62)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = resources.GetString("Label9.Text")
        '
        'LinkLabel3
        '
        Me.LinkLabel3.AutoSize = True
        Me.LinkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel3.LinkColor = System.Drawing.Color.MidnightBlue
        Me.LinkLabel3.Location = New System.Drawing.Point(40, 513)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(336, 21)
        Me.LinkLabel3.TabIndex = 5
        Me.LinkLabel3.TabStop = True
        Me.LinkLabel3.Text = "Which method should I use for my installation?"
        '
        'InstSettings
        '
        Me.InstSettings.Controls.Add(Me.CheckBox3)
        Me.InstSettings.Controls.Add(Me.CheckBox2)
        Me.InstSettings.Controls.Add(Me.CheckBox1)
        Me.InstSettings.Controls.Add(Me.Label11)
        Me.InstSettings.Controls.Add(Me.Label10)
        Me.InstSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InstSettings.Location = New System.Drawing.Point(0, 0)
        Me.InstSettings.Name = "InstSettings"
        Me.InstSettings.Size = New System.Drawing.Size(1280, 568)
        Me.InstSettings.TabIndex = 6
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(40, 20)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(1200, 51)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "Please configure the settings of this installation:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(115, 91)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(239, 21)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Create program shortcuts on the:"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(152, 140)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(105, 25)
        Me.CheckBox1.TabIndex = 5
        Me.CheckBox1.Text = "Start menu"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Checked = True
        Me.CheckBox2.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox2.Location = New System.Drawing.Point(152, 171)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(86, 25)
        Me.CheckBox2.TabIndex = 5
        Me.CheckBox2.Text = "Desktop"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Location = New System.Drawing.Point(119, 227)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(283, 25)
        Me.CheckBox3.TabIndex = 6
        Me.CheckBox3.Text = "Download the ""boot.wim"" test image"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.DISMTools_Setup.My.Resources.Resources.setupbg
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(1280, 720)
        Me.Controls.Add(Me.BodyContainer)
        Me.Controls.Add(Me.PanelButtons)
        Me.Controls.Add(Me.WindowControls)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MainForm"
        Me.Text = "DISMTools Setup"
        Me.WindowControls.ResumeLayout(False)
        Me.WindowControls.PerformLayout()
        CType(Me.minBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.closeBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelButtons.ResumeLayout(False)
        Me.PanelButtons.PerformLayout()
        Me.BodyContainer.ResumeLayout(False)
        Me.InstMethod.ResumeLayout(False)
        Me.InstMethod.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WelcomePanel.ResumeLayout(False)
        Me.EulaPanel.ResumeLayout(False)
        Me.EulaPanel.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PleaseWait.ResumeLayout(False)
        Me.PleaseWait.PerformLayout()
        Me.InstSettings.ResumeLayout(False)
        Me.InstSettings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents WindowControls As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents minBox As System.Windows.Forms.PictureBox
    Friend WithEvents closeBox As System.Windows.Forms.PictureBox
    Friend WithEvents PanelButtons As System.Windows.Forms.Panel
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents BodyContainer As System.Windows.Forms.Panel
    Friend WithEvents WelcomePanel As System.Windows.Forms.Panel
    Friend WithEvents PleaseWait As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents EulaPanel As System.Windows.Forms.Panel
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents InstMethod As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel3 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents InstSettings As System.Windows.Forms.Panel
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label

End Class
