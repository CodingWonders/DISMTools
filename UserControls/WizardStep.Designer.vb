<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WizardStep
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
        Me.lblWizardStepName = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblWizardStepName
        '
        Me.lblWizardStepName.AutoEllipsis = True
        Me.lblWizardStepName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblWizardStepName.ForeColor = System.Drawing.Color.DodgerBlue
        Me.lblWizardStepName.Location = New System.Drawing.Point(0, 0)
        Me.lblWizardStepName.Name = "lblWizardStepName"
        Me.lblWizardStepName.Padding = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.lblWizardStepName.Size = New System.Drawing.Size(244, 28)
        Me.lblWizardStepName.TabIndex = 0
        Me.lblWizardStepName.Text = "Label1"
        Me.lblWizardStepName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'WizardStep
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.lblWizardStepName)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "WizardStep"
        Me.Size = New System.Drawing.Size(244, 28)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblWizardStepName As System.Windows.Forms.Label

End Class
