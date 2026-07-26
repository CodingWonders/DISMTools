<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LockVolumeDialog
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DrLetterLabel = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.PersistentVolumeIdLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(352, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Please wait while we lock this volume. This will take a couple of seconds."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(42, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Drive Letter:"
        '
        'DrLetterLabel
        '
        Me.DrLetterLabel.AutoSize = True
        Me.DrLetterLabel.Location = New System.Drawing.Point(116, 42)
        Me.DrLetterLabel.Name = "DrLetterLabel"
        Me.DrLetterLabel.Size = New System.Drawing.Size(0, 13)
        Me.DrLetterLabel.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(42, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(110, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Persistent Volume ID:"
        '
        'PersistentVolumeIdLabel
        '
        Me.PersistentVolumeIdLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PersistentVolumeIdLabel.AutoEllipsis = True
        Me.PersistentVolumeIdLabel.Location = New System.Drawing.Point(42, 84)
        Me.PersistentVolumeIdLabel.Name = "PersistentVolumeIdLabel"
        Me.PersistentVolumeIdLabel.Size = New System.Drawing.Size(381, 43)
        Me.PersistentVolumeIdLabel.TabIndex = 2
        Me.PersistentVolumeIdLabel.Text = "      "
        Me.PersistentVolumeIdLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LockVolumeDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(464, 153)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.PersistentVolumeIdLabel)
        Me.Controls.Add(Me.DrLetterLabel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LockVolumeDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Locking volume..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DrLetterLabel As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PersistentVolumeIdLabel As System.Windows.Forms.Label

End Class
