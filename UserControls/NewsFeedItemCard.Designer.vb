<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewsFeedItemCard
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
        Me.FeedItemLinkLabel = New System.Windows.Forms.LinkLabel()
        Me.FeedItemDateLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'FeedItemLinkLabel
        '
        Me.FeedItemLinkLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FeedItemLinkLabel.AutoEllipsis = True
        Me.FeedItemLinkLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FeedItemLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.FeedItemLinkLabel.LinkColor = System.Drawing.Color.DodgerBlue
        Me.FeedItemLinkLabel.Location = New System.Drawing.Point(15, 15)
        Me.FeedItemLinkLabel.Name = "FeedItemLinkLabel"
        Me.FeedItemLinkLabel.Size = New System.Drawing.Size(293, 34)
        Me.FeedItemLinkLabel.TabIndex = 0
        Me.FeedItemLinkLabel.TabStop = True
        Me.FeedItemLinkLabel.Text = LocalizationService.ForSection("Designer.NewsFeedCard")("Item.Title")
        '
        'FeedItemDateLabel
        '
        Me.FeedItemDateLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FeedItemDateLabel.AutoEllipsis = True
        Me.FeedItemDateLabel.Location = New System.Drawing.Point(317, 15)
        Me.FeedItemDateLabel.Name = "FeedItemDateLabel"
        Me.FeedItemDateLabel.Size = New System.Drawing.Size(217, 34)
        Me.FeedItemDateLabel.TabIndex = 1
        Me.FeedItemDateLabel.Text = LocalizationService.ForSection("Designer.NewsFeedCard")("ItemDate.Label")
        Me.FeedItemDateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'NewsFeedItemCard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.FeedItemDateLabel)
        Me.Controls.Add(Me.FeedItemLinkLabel)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "NewsFeedItemCard"
        Me.Size = New System.Drawing.Size(548, 64)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FeedItemLinkLabel As System.Windows.Forms.LinkLabel
    Friend WithEvents FeedItemDateLabel As System.Windows.Forms.Label

End Class
