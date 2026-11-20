<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ModelCatalogViewerForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.lvHpProducts = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.btnLoadHpModels = New System.Windows.Forms.Button
        Me.ofdHpModelCatalog = New System.Windows.Forms.OpenFileDialog
        Me.btnLoadDellModels = New System.Windows.Forms.Button
        Me.btnLenobodyCares = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(13, 42)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(991, 519)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lvHpProducts)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(983, 493)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "HP"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'lvHpProducts
        '
        Me.lvHpProducts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvHpProducts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvHpProducts.Location = New System.Drawing.Point(3, 3)
        Me.lvHpProducts.Name = "lvHpProducts"
        Me.lvHpProducts.Size = New System.Drawing.Size(977, 487)
        Me.lvHpProducts.TabIndex = 0
        Me.lvHpProducts.UseCompatibleStateImageBehavior = False
        Me.lvHpProducts.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Model"
        Me.ColumnHeader1.Width = 423
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Operating System"
        Me.ColumnHeader2.Width = 246
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(983, 493)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Dell"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(983, 493)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Lenovo"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'btnLoadHpModels
        '
        Me.btnLoadHpModels.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnLoadHpModels.Location = New System.Drawing.Point(13, 13)
        Me.btnLoadHpModels.Name = "btnLoadHpModels"
        Me.btnLoadHpModels.Size = New System.Drawing.Size(231, 23)
        Me.btnLoadHpModels.TabIndex = 1
        Me.btnLoadHpModels.Text = "Load HP Catalog"
        Me.btnLoadHpModels.UseVisualStyleBackColor = True
        '
        'btnLoadDellModels
        '
        Me.btnLoadDellModels.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnLoadDellModels.Location = New System.Drawing.Point(250, 13)
        Me.btnLoadDellModels.Name = "btnLoadDellModels"
        Me.btnLoadDellModels.Size = New System.Drawing.Size(231, 23)
        Me.btnLoadDellModels.TabIndex = 1
        Me.btnLoadDellModels.Text = "Load Dell Catalog"
        Me.btnLoadDellModels.UseVisualStyleBackColor = True
        '
        'btnLenobodyCares
        '
        Me.btnLenobodyCares.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnLenobodyCares.Location = New System.Drawing.Point(487, 13)
        Me.btnLenobodyCares.Name = "btnLenobodyCares"
        Me.btnLenobodyCares.Size = New System.Drawing.Size(231, 23)
        Me.btnLenobodyCares.TabIndex = 1
        Me.btnLenobodyCares.Text = "Load Lenovo Catalog"
        Me.btnLenobodyCares.UseVisualStyleBackColor = True
        '
        'ModelCatalogViewerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1016, 573)
        Me.Controls.Add(Me.btnLenobodyCares)
        Me.Controls.Add(Me.btnLoadDellModels)
        Me.Controls.Add(Me.btnLoadHpModels)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimumSize = New System.Drawing.Size(1024, 600)
        Me.Name = "ModelCatalogViewerForm"
        Me.Text = "View Manufacturer Model Catalogs"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents btnLoadHpModels As System.Windows.Forms.Button
    Friend WithEvents ofdHpModelCatalog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lvHpProducts As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnLoadDellModels As System.Windows.Forms.Button
    Friend WithEvents btnLenobodyCares As System.Windows.Forms.Button
End Class
