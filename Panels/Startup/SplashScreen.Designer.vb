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
        Me.VersionPic = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.LogoPic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VersionPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'LogoPic
        '
        Me.LogoPic.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LogoPic.BackColor = System.Drawing.Color.Transparent
        Me.LogoPic.Image = Global.DISMTools.My.Resources.Resources.logo_aboutdlg_dark
        Me.LogoPic.Location = New System.Drawing.Point(220, 64)
        Me.LogoPic.Name = "LogoPic"
        Me.LogoPic.Size = New System.Drawing.Size(360, 72)
        Me.LogoPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.LogoPic.TabIndex = 0
        Me.LogoPic.TabStop = False
        '
        'VersionPic
        '
        Me.VersionPic.BackColor = System.Drawing.Color.Transparent
        Me.VersionPic.Dock = System.Windows.Forms.DockStyle.Fill
        Me.VersionPic.Image = Global.DISMTools.My.Resources.Resources.version
        Me.VersionPic.Location = New System.Drawing.Point(0, 0)
        Me.VersionPic.Name = "VersionPic"
        Me.VersionPic.Size = New System.Drawing.Size(164, 32)
        Me.VersionPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.VersionPic.TabIndex = 0
        Me.VersionPic.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.VersionPic)
        Me.Panel1.Location = New System.Drawing.Point(636, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(164, 32)
        Me.Panel1.TabIndex = 1
        '
        'Timer1
        '
        '
        'SplashScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.DISMTools.My.Resources.Resources.startup_bg
        Me.ClientSize = New System.Drawing.Size(800, 200)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.LogoPic)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SplashScreen"
        Me.Opacity = 0.0R
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DISMTools - Starting up..."
        CType(Me.LogoPic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VersionPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LogoPic As System.Windows.Forms.PictureBox
    Friend WithEvents VersionPic As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
