<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrgSetup
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
        Me.wndControlPanel = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnControlPanel = New System.Windows.Forms.Panel()
        Me.Back_Button = New System.Windows.Forms.Button()
        Me.Next_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.BodyPanelContainer = New System.Windows.Forms.Panel()
        Me.WelcomePanel = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.wndControlPanel.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.btnControlPanel.SuspendLayout()
        Me.BodyPanelContainer.SuspendLayout()
        Me.WelcomePanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'wndControlPanel
        '
        Me.wndControlPanel.BackColor = System.Drawing.Color.Transparent
        Me.wndControlPanel.BackgroundImage = Global.DISMTools.My.Resources.Resources.wndPanel_Backdrop
        Me.wndControlPanel.Controls.Add(Me.Label1)
        Me.wndControlPanel.Controls.Add(Me.PictureBox2)
        Me.wndControlPanel.Controls.Add(Me.PictureBox1)
        Me.wndControlPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.wndControlPanel.Location = New System.Drawing.Point(0, 0)
        Me.wndControlPanel.Name = "wndControlPanel"
        Me.wndControlPanel.Size = New System.Drawing.Size(960, 48)
        Me.wndControlPanel.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "DISMTools"
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.DISMTools.My.Resources.Resources.minBox
        Me.PictureBox2.Location = New System.Drawing.Point(869, 0)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(46, 32)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DISMTools.My.Resources.Resources.closebox
        Me.PictureBox1.Location = New System.Drawing.Point(914, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(46, 32)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'btnControlPanel
        '
        Me.btnControlPanel.BackColor = System.Drawing.Color.Transparent
        Me.btnControlPanel.BackgroundImage = Global.DISMTools.My.Resources.Resources.wndPanel_Backdrop
        Me.btnControlPanel.Controls.Add(Me.Label4)
        Me.btnControlPanel.Controls.Add(Me.Back_Button)
        Me.btnControlPanel.Controls.Add(Me.Next_Button)
        Me.btnControlPanel.Controls.Add(Me.Cancel_Button)
        Me.btnControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnControlPanel.Location = New System.Drawing.Point(0, 664)
        Me.btnControlPanel.Name = "btnControlPanel"
        Me.btnControlPanel.Size = New System.Drawing.Size(960, 48)
        Me.btnControlPanel.TabIndex = 1
        '
        'Back_Button
        '
        Me.Back_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Back_Button.Location = New System.Drawing.Point(711, 13)
        Me.Back_Button.Name = "Back_Button"
        Me.Back_Button.Size = New System.Drawing.Size(75, 23)
        Me.Back_Button.TabIndex = 0
        Me.Back_Button.Text = "Back"
        Me.Back_Button.UseVisualStyleBackColor = True
        '
        'Next_Button
        '
        Me.Next_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Next_Button.Location = New System.Drawing.Point(792, 13)
        Me.Next_Button.Name = "Next_Button"
        Me.Next_Button.Size = New System.Drawing.Size(75, 23)
        Me.Next_Button.TabIndex = 0
        Me.Next_Button.Text = "Next"
        Me.Next_Button.UseVisualStyleBackColor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Cancel_Button.Location = New System.Drawing.Point(873, 13)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(75, 23)
        Me.Cancel_Button.TabIndex = 0
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'BodyPanelContainer
        '
        Me.BodyPanelContainer.BackColor = System.Drawing.Color.Transparent
        Me.BodyPanelContainer.Controls.Add(Me.WelcomePanel)
        Me.BodyPanelContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BodyPanelContainer.Location = New System.Drawing.Point(0, 48)
        Me.BodyPanelContainer.Name = "BodyPanelContainer"
        Me.BodyPanelContainer.Size = New System.Drawing.Size(960, 616)
        Me.BodyPanelContainer.TabIndex = 2
        '
        'WelcomePanel
        '
        Me.WelcomePanel.Controls.Add(Me.Label3)
        Me.WelcomePanel.Controls.Add(Me.Label2)
        Me.WelcomePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WelcomePanel.Location = New System.Drawing.Point(0, 0)
        Me.WelcomePanel.Name = "WelcomePanel"
        Me.WelcomePanel.Size = New System.Drawing.Size(960, 616)
        Me.WelcomePanel.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Variable Display Semib", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(24, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(268, 32)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Welcome to DISMTools"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(56, 76)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(769, 20)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "DISMTools is a free and open-source, project-driven GUI for DISM operations. To b" & _
    "egin setting things up, click Next"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(12, 17)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(579, 15)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "This is internal content, and is in design phases. You should not call this proce" & _
    "dure since it won't do anything"
        '
        'PrgSetup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.DISMTools.My.Resources.Resources.dt_bgbranding
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(960, 712)
        Me.Controls.Add(Me.BodyPanelContainer)
        Me.Controls.Add(Me.btnControlPanel)
        Me.Controls.Add(Me.wndControlPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "PrgSetup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PrgSetup"
        Me.wndControlPanel.ResumeLayout(False)
        Me.wndControlPanel.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.btnControlPanel.ResumeLayout(False)
        Me.btnControlPanel.PerformLayout()
        Me.BodyPanelContainer.ResumeLayout(False)
        Me.WelcomePanel.ResumeLayout(False)
        Me.WelcomePanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents wndControlPanel As System.Windows.Forms.Panel
    Friend WithEvents btnControlPanel As System.Windows.Forms.Panel
    Friend WithEvents Back_Button As System.Windows.Forms.Button
    Friend WithEvents Next_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents BodyPanelContainer As System.Windows.Forms.Panel
    Friend WithEvents WelcomePanel As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
