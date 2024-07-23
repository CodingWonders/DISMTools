<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnattendMgr
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnattendMgr))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label1.AutoEllipsis = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(60, 165)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(824, 230)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Unattended answer file tasks are going through a major reconstruction."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UnattendMgr
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(944, 561)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(960, 600)
        Me.Name = "UnattendMgr"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Unattended answer file manager"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
