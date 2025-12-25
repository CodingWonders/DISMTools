<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImageTaskHeader
    Inherits System.Windows.Forms.UserControl

    'UserControl reemplaza a Dispose para limpiar la lista de componentes.
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
        Me.ItemPictureBox = New System.Windows.Forms.PictureBox()
        Me.ItemTitle = New System.Windows.Forms.Label()
        CType(Me.ItemPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ItemPictureBox
        '
        Me.ItemPictureBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ItemPictureBox.Location = New System.Drawing.Point(358, 8)
        Me.ItemPictureBox.Name = "ItemPictureBox"
        Me.ItemPictureBox.Size = New System.Drawing.Size(32, 32)
        Me.ItemPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.ItemPictureBox.TabIndex = 2
        Me.ItemPictureBox.TabStop = False
        '
        'ItemTitle
        '
        Me.ItemTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ItemTitle.AutoEllipsis = True
        Me.ItemTitle.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ItemTitle.Location = New System.Drawing.Point(12, 8)
        Me.ItemTitle.Name = "ItemTitle"
        Me.ItemTitle.Size = New System.Drawing.Size(340, 30)
        Me.ItemTitle.TabIndex = 3
        Me.ItemTitle.Text = "Item Text"
        '
        'ImageTaskHeader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.ItemTitle)
        Me.Controls.Add(Me.ItemPictureBox)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximumSize = New System.Drawing.Size(19200, 48)
        Me.MinimumSize = New System.Drawing.Size(400, 48)
        Me.Name = "ImageTaskHeader"
        Me.Size = New System.Drawing.Size(400, 48)
        CType(Me.ItemPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents ItemPictureBox As System.Windows.Forms.PictureBox
    Private WithEvents ItemTitle As System.Windows.Forms.Label

End Class
