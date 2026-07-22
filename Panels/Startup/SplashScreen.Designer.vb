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
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PreviewFlag = New System.Windows.Forms.PictureBox()
        Me.VersionLabel = New System.Windows.Forms.Label()
        CType(Me.LogoPic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PreviewFlag, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LogoPic
        '
        Me.LogoPic.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LogoPic.BackColor = System.Drawing.Color.Transparent
        Me.LogoPic.Image = Global.DISMTools.My.Resources.Resources.dt_branding
        Me.LogoPic.Location = New System.Drawing.Point(12, 36)
        Me.LogoPic.Name = "LogoPic"
        Me.LogoPic.Size = New System.Drawing.Size(776, 119)
        Me.LogoPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.LogoPic.TabIndex = 0
        Me.LogoPic.TabStop = False
        '
        'Timer1
        '
        '
        'PreviewFlag
        '
        Me.PreviewFlag.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.PreviewFlag.BackColor = System.Drawing.Color.Transparent
        Me.PreviewFlag.Image = Global.DISMTools.My.Resources.Resources.preview_flag
        Me.PreviewFlag.Location = New System.Drawing.Point(513, 130)
        Me.PreviewFlag.Name = "PreviewFlag"
        Me.PreviewFlag.Size = New System.Drawing.Size(112, 48)
        Me.PreviewFlag.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PreviewFlag.TabIndex = 0
        Me.PreviewFlag.TabStop = False
        Me.PreviewFlag.Visible = False
        '
        'VersionLabel
        '
        Me.VersionLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.VersionLabel.AutoEllipsis = True
        Me.VersionLabel.BackColor = System.Drawing.Color.Transparent
        Me.VersionLabel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VersionLabel.Location = New System.Drawing.Point(233, 170)
        Me.VersionLabel.Name = "VersionLabel"
        Me.VersionLabel.Size = New System.Drawing.Size(560, 23)
        Me.VersionLabel.TabIndex = 1
        Me.VersionLabel.Text = LocalizationService.ForSection("Designer.SplashScreen")("VersionLabel.Label")
        Me.VersionLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.VersionLabel.Visible = False
        '
        'SplashScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackgroundImage = Global.DISMTools.My.Resources.Resources.startup_bg
        Me.ClientSize = New System.Drawing.Size(800, 200)
        Me.ControlBox = False
        Me.Controls.Add(Me.VersionLabel)
        Me.Controls.Add(Me.PreviewFlag)
        Me.Controls.Add(Me.LogoPic)
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
        Me.Text = LocalizationService.ForSection("Designer.SplashScreen")("DISM.Tools.Starting.Button")
        Me.TopMost = True
        CType(Me.LogoPic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PreviewFlag, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LogoPic As System.Windows.Forms.PictureBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents PreviewFlag As System.Windows.Forms.PictureBox
    Friend WithEvents VersionLabel As System.Windows.Forms.Label
End Class
