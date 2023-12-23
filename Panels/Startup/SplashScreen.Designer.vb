<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SplashScreen
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
        Me.components = New System.ComponentModel.Container()
        Me.LogoPic = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.VersionPic = New System.Windows.Forms.PictureBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PreviewFlag = New System.Windows.Forms.PictureBox()
        CType(Me.LogoPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.VersionPic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PreviewFlag, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LogoPic
        '
        Me.LogoPic.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LogoPic.BackColor = System.Drawing.Color.Transparent
        Me.LogoPic.Image = Global.DISMTools.My.Resources.Resources.dt_branding
        Me.LogoPic.Location = New System.Drawing.Point(222, 75)
        Me.LogoPic.Name = "LogoPic"
        Me.LogoPic.Size = New System.Drawing.Size(357, 51)
        Me.LogoPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.LogoPic.TabIndex = 0
        Me.LogoPic.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.VersionPic)
        Me.Panel1.Location = New System.Drawing.Point(636, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(164, 32)
        Me.Panel1.TabIndex = 1
        Me.Panel1.Visible = False
        '
        'VersionPic
        '
        Me.VersionPic.Dock = System.Windows.Forms.DockStyle.Fill
        Me.VersionPic.Image = Global.DISMTools.My.Resources.Resources.version
        Me.VersionPic.Location = New System.Drawing.Point(0, 0)
        Me.VersionPic.Name = "VersionPic"
        Me.VersionPic.Size = New System.Drawing.Size(164, 32)
        Me.VersionPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.VersionPic.TabIndex = 0
        Me.VersionPic.TabStop = False
        '
        'Timer1
        '
        '
        'PreviewFlag
        '
        Me.PreviewFlag.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.PreviewFlag.BackColor = System.Drawing.Color.Transparent
        Me.PreviewFlag.Image = Global.DISMTools.My.Resources.Resources.preview_flag
        Me.PreviewFlag.Location = New System.Drawing.Point(512, 121)
        Me.PreviewFlag.Name = "PreviewFlag"
        Me.PreviewFlag.Size = New System.Drawing.Size(112, 48)
        Me.PreviewFlag.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PreviewFlag.TabIndex = 0
        Me.PreviewFlag.TabStop = False
        Me.PreviewFlag.Visible = False
        '
        'SplashScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.DISMTools.My.Resources.Resources.startup_bg
        Me.ClientSize = New System.Drawing.Size(800, 200)
        Me.ControlBox = False
        Me.Controls.Add(Me.PreviewFlag)
        Me.Controls.Add(Me.LogoPic)
        Me.Controls.Add(Me.Panel1)
        Me.DoubleBuffered = True
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SplashScreen"
        Me.Opacity = 0.0R
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DISMTools - Starting up..."
        Me.TopMost = True
        CType(Me.LogoPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.VersionPic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PreviewFlag, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LogoPic As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents PreviewFlag As System.Windows.Forms.PictureBox
    Friend WithEvents VersionPic As System.Windows.Forms.PictureBox
End Class
