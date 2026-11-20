<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ModelCatalogDownloadDialog
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.pbHP = New System.Windows.Forms.PictureBox
        Me.pbDell = New System.Windows.Forms.PictureBox
        Me.pbLenovo = New System.Windows.Forms.PictureBox
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.pbHP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbDell, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbLenovo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(471, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Please wait while device model catalogs from the following manufacturers are bein" & _
            "g downloaded:"
        '
        'Label2
        '
        Me.Label2.AutoEllipsis = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(187, 30)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "HP SoftPAQ Lists"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.Controls.Add(Me.pbHP, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.pbDell, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.pbLenovo, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 2, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(66, 56)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.58823!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.41176!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(582, 100)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoEllipsis = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(196, 70)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(187, 30)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Dell Command | Deploy Driver Pack Catalog"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label4
        '
        Me.Label4.AutoEllipsis = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(389, 70)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(190, 30)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Lenovo CDRT Model Catalog"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'pbHP
        '
        Me.pbHP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbHP.Image = Global.DrvPakCreator.My.Resources.Resources.man_hp
        Me.pbHP.Location = New System.Drawing.Point(3, 3)
        Me.pbHP.Name = "pbHP"
        Me.pbHP.Size = New System.Drawing.Size(187, 64)
        Me.pbHP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbHP.TabIndex = 1
        Me.pbHP.TabStop = False
        '
        'pbDell
        '
        Me.pbDell.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbDell.Image = Global.DrvPakCreator.My.Resources.Resources.man_dell
        Me.pbDell.Location = New System.Drawing.Point(196, 3)
        Me.pbDell.Name = "pbDell"
        Me.pbDell.Size = New System.Drawing.Size(187, 64)
        Me.pbDell.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbDell.TabIndex = 1
        Me.pbDell.TabStop = False
        '
        'pbLenovo
        '
        Me.pbLenovo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbLenovo.Image = Global.DrvPakCreator.My.Resources.Resources.man_lenovo
        Me.pbLenovo.Location = New System.Drawing.Point(389, 3)
        Me.pbLenovo.Name = "pbLenovo"
        Me.pbLenovo.Size = New System.Drawing.Size(190, 64)
        Me.pbLenovo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbLenovo.TabIndex = 1
        Me.pbLenovo.TabStop = False
        '
        'ModelCatalogDownloadDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(714, 231)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ModelCatalogDownloadDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Downloading Model Catalogs"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.pbHP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbDell, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbLenovo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pbHP As System.Windows.Forms.PictureBox
    Friend WithEvents pbDell As System.Windows.Forms.PictureBox
    Friend WithEvents pbLenovo As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label

End Class
