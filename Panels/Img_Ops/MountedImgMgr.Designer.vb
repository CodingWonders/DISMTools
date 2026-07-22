<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MountedImgMgr
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MountedImgMgr))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ActionsTLP = New System.Windows.Forms.TableLayoutPanel()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ActionsTLP.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(1239, 29)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Overview.Images.Message")
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.6734695!))
        Me.TableLayoutPanel1.Controls.Add(Me.ListView1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ActionsTLP, 0, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(16, 52)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1236, 617)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(3, 3)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(1230, 576)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("ImageFile.Column")
        Me.ColumnHeader1.Width = 480
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Index.Column")
        Me.ColumnHeader2.Width = 72
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("MountDirectory.Column")
        Me.ColumnHeader3.Width = 420
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Status.Column")
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Read.Write.Column")
        Me.ColumnHeader5.Width = 147
        '
        'ActionsTLP
        '
        Me.ActionsTLP.ColumnCount = 7
        Me.ActionsTLP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.ActionsTLP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.ActionsTLP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.ActionsTLP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.ActionsTLP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.ActionsTLP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.6666698!))
        Me.ActionsTLP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42.0!))
        Me.ActionsTLP.Controls.Add(Me.Button6, 5, 0)
        Me.ActionsTLP.Controls.Add(Me.Button7, 6, 0)
        Me.ActionsTLP.Controls.Add(Me.Button4, 3, 0)
        Me.ActionsTLP.Controls.Add(Me.Button3, 2, 0)
        Me.ActionsTLP.Controls.Add(Me.Button2, 1, 0)
        Me.ActionsTLP.Controls.Add(Me.Button5, 4, 0)
        Me.ActionsTLP.Controls.Add(Me.Button1, 0, 0)
        Me.ActionsTLP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ActionsTLP.Location = New System.Drawing.Point(3, 585)
        Me.ActionsTLP.Name = "ActionsTLP"
        Me.ActionsTLP.RowCount = 1
        Me.ActionsTLP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.ActionsTLP.Size = New System.Drawing.Size(1230, 29)
        Me.ActionsTLP.TabIndex = 1
        '
        'Button6
        '
        Me.Button6.AutoEllipsis = True
        Me.Button6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button6.Enabled = False
        Me.Button6.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button6.Location = New System.Drawing.Point(988, 3)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(191, 23)
        Me.Button6.TabIndex = 7
        Me.Button6.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("LoadProject.Button")
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.AutoEllipsis = True
        Me.Button7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button7.Enabled = False
        Me.Button7.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button7.Location = New System.Drawing.Point(1185, 3)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(42, 23)
        Me.Button7.TabIndex = 6
        Me.Button7.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Value.Button")
        Me.Button7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button4.Enabled = False
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button4.Location = New System.Drawing.Point(594, 3)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(191, 23)
        Me.Button4.TabIndex = 5
        Me.Button4.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Open.Mount.Dir.Button")
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.AutoEllipsis = True
        Me.Button3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button3.Enabled = False
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button3.Location = New System.Drawing.Point(397, 3)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(191, 23)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Enable.Write.Button")
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.AutoEllipsis = True
        Me.Button2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button2.Enabled = False
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button2.Location = New System.Drawing.Point(200, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(191, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("ReloadServicing.Button")
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.AutoEllipsis = True
        Me.Button5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button5.Enabled = False
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button5.Location = New System.Drawing.Point(791, 3)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(191, 23)
        Me.Button5.TabIndex = 1
        Me.Button5.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Remove.VolumeImages.Button")
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.AutoEllipsis = True
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button1.Enabled = False
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(3, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(191, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("UnmountImage.Button")
        Me.Button1.UseVisualStyleBackColor = True
        '
        'MountedImgMgr
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1264, 681)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(4096, 4096)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "MountedImgMgr"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Image.Manager.Label")
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ActionsTLP.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ActionsTLP As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
End Class
