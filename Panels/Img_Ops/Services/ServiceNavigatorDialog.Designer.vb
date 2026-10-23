<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ServiceNavigatorDialog
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
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.lblNavigatorHeader = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lvServiceDependencies = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lvServiceDependents = New System.Windows.Forms.ListView()
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SuspendLayout()
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Cancel_Button.Location = New System.Drawing.Point(625, 346)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Close"
        '
        'lblNavigatorHeader
        '
        Me.lblNavigatorHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNavigatorHeader.AutoEllipsis = True
        Me.lblNavigatorHeader.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNavigatorHeader.Location = New System.Drawing.Point(13, 13)
        Me.lblNavigatorHeader.Name = "lblNavigatorHeader"
        Me.lblNavigatorHeader.Size = New System.Drawing.Size(679, 13)
        Me.lblNavigatorHeader.TabIndex = 2
        Me.lblNavigatorHeader.Text = "Navigate through the hierarchy of service <service> by double-clicking an entry:"
        Me.lblNavigatorHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Dependencies:"
        '
        'lvServiceDependencies
        '
        Me.lvServiceDependencies.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lvServiceDependencies.FullRowSelect = True
        Me.lvServiceDependencies.Location = New System.Drawing.Point(13, 56)
        Me.lvServiceDependencies.MultiSelect = False
        Me.lvServiceDependencies.Name = "lvServiceDependencies"
        Me.lvServiceDependencies.Size = New System.Drawing.Size(679, 128)
        Me.lvServiceDependencies.TabIndex = 4
        Me.lvServiceDependencies.UseCompatibleStateImageBehavior = False
        Me.lvServiceDependencies.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Service Name"
        Me.ColumnHeader1.Width = 512
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 190)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Dependents:"
        '
        'lvServiceDependents
        '
        Me.lvServiceDependents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2})
        Me.lvServiceDependents.FullRowSelect = True
        Me.lvServiceDependents.Location = New System.Drawing.Point(13, 210)
        Me.lvServiceDependents.MultiSelect = False
        Me.lvServiceDependents.Name = "lvServiceDependents"
        Me.lvServiceDependents.Size = New System.Drawing.Size(679, 128)
        Me.lvServiceDependents.TabIndex = 4
        Me.lvServiceDependents.UseCompatibleStateImageBehavior = False
        Me.lvServiceDependents.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Service Name"
        Me.ColumnHeader2.Width = 512
        '
        'ServiceNavigatorDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(704, 381)
        Me.Controls.Add(Me.lvServiceDependents)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lvServiceDependencies)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblNavigatorHeader)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ServiceNavigatorDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Navigate to Service in Hierarchy"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents lblNavigatorHeader As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lvServiceDependencies As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lvServiceDependents As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader

End Class
