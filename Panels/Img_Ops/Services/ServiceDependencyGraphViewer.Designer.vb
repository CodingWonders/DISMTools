<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ServiceDependencyGraphViewer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ServiceDependencyGraphViewer))
        Me.btnZoomIn = New System.Windows.Forms.Button()
        Me.btnZoomOut = New System.Windows.Forms.Button()
        Me.btnResetZoom = New System.Windows.Forms.Button()
        Me.btnSaveImage = New System.Windows.Forms.Button()
        Me.depDiagram = New DISMTools.ServiceDependencyDiagram()
        Me.sfdDiagram = New System.Windows.Forms.SaveFileDialog()
        Me.SuspendLayout()
        '
        'btnZoomIn
        '
        Me.btnZoomIn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnZoomIn.Image = Global.DISMTools.My.Resources.Resources.diagram_zoom_in_light
        Me.btnZoomIn.Location = New System.Drawing.Point(1216, 12)
        Me.btnZoomIn.Name = "btnZoomIn"
        Me.btnZoomIn.Size = New System.Drawing.Size(36, 36)
        Me.btnZoomIn.TabIndex = 1
        Me.btnZoomIn.UseVisualStyleBackColor = True
        '
        'btnZoomOut
        '
        Me.btnZoomOut.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnZoomOut.Image = Global.DISMTools.My.Resources.Resources.diagram_zoom_out_light
        Me.btnZoomOut.Location = New System.Drawing.Point(1216, 54)
        Me.btnZoomOut.Name = "btnZoomOut"
        Me.btnZoomOut.Size = New System.Drawing.Size(36, 36)
        Me.btnZoomOut.TabIndex = 1
        Me.btnZoomOut.UseVisualStyleBackColor = True
        '
        'btnResetZoom
        '
        Me.btnResetZoom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnResetZoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnResetZoom.Image = Global.DISMTools.My.Resources.Resources.diagram_zoom_reset_light
        Me.btnResetZoom.Location = New System.Drawing.Point(1216, 96)
        Me.btnResetZoom.Name = "btnResetZoom"
        Me.btnResetZoom.Size = New System.Drawing.Size(36, 36)
        Me.btnResetZoom.TabIndex = 1
        Me.btnResetZoom.UseVisualStyleBackColor = True
        '
        'btnSaveImage
        '
        Me.btnSaveImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSaveImage.Image = Global.DISMTools.My.Resources.Resources.diagram_save_image_light
        Me.btnSaveImage.Location = New System.Drawing.Point(1216, 138)
        Me.btnSaveImage.Name = "btnSaveImage"
        Me.btnSaveImage.Size = New System.Drawing.Size(36, 36)
        Me.btnSaveImage.TabIndex = 1
        Me.btnSaveImage.UseVisualStyleBackColor = True
        '
        'depDiagram
        '
        Me.depDiagram.ArrowColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.depDiagram.BackColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.depDiagram.DescriptionTextColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.depDiagram.DiagramBackColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.depDiagram.Dock = System.Windows.Forms.DockStyle.Fill
        Me.depDiagram.ForeColor = System.Drawing.Color.White
        Me.depDiagram.Location = New System.Drawing.Point(0, 0)
        Me.depDiagram.MainNodeBackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(86, Byte), Integer), CType(CType(120, Byte), Integer))
        Me.depDiagram.MainNodeBorderColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(170, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.depDiagram.Name = "depDiagram"
        Me.depDiagram.NodeBackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(48, Byte), Integer), CType(CType(48, Byte), Integer))
        Me.depDiagram.NodeBorderColor = System.Drawing.Color.FromArgb(CType(CType(120, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(120, Byte), Integer))
        Me.depDiagram.NodeCornerRadius = 0
        Me.depDiagram.NodeTextColor = System.Drawing.Color.White
        Me.depDiagram.Size = New System.Drawing.Size(1264, 681)
        Me.depDiagram.TabIndex = 0
        Me.depDiagram.Text = "ServiceDependencyDiagram1"
        Me.depDiagram.Zoom = 1.0!
        '
        'sfdDiagram
        '
        Me.sfdDiagram.Filter = "PNG files|*.png"
        '
        'ServiceDependencyGraphViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1264, 681)
        Me.Controls.Add(Me.btnSaveImage)
        Me.Controls.Add(Me.btnResetZoom)
        Me.Controls.Add(Me.btnZoomOut)
        Me.Controls.Add(Me.btnZoomIn)
        Me.Controls.Add(Me.depDiagram)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1024, 600)
        Me.Name = "ServiceDependencyGraphViewer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Service Dependency Viewer"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents depDiagram As DISMTools.ServiceDependencyDiagram
    Friend WithEvents btnZoomIn As System.Windows.Forms.Button
    Friend WithEvents btnZoomOut As System.Windows.Forms.Button
    Friend WithEvents btnResetZoom As System.Windows.Forms.Button
    Friend WithEvents btnSaveImage As System.Windows.Forms.Button
    Friend WithEvents sfdDiagram As System.Windows.Forms.SaveFileDialog
End Class
