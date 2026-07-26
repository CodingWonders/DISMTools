<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnlockVolumeDialog
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.KeyProtectorIdLabel = New System.Windows.Forms.Label()
        Me.RPS1 = New System.Windows.Forms.TextBox()
        Me.RPS2 = New System.Windows.Forms.TextBox()
        Me.RPS3 = New System.Windows.Forms.TextBox()
        Me.RPS4 = New System.Windows.Forms.TextBox()
        Me.RPS5 = New System.Windows.Forms.TextBox()
        Me.RPS6 = New System.Windows.Forms.TextBox()
        Me.RPS7 = New System.Windows.Forms.TextBox()
        Me.RPS8 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(626, 160)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Enabled = False
        Me.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.BDE.UnlockVolume")("Ok.Button")
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.BDE.UnlockVolume")("Cancel.Button")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(539, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = LocalizationService.ForSection("Designer.BDE.UnlockVolume")("RecoveryKey.Message")
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoEllipsis = True
        Me.Label2.Location = New System.Drawing.Point(128, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(192, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = LocalizationService.ForSection("Designer.BDE.UnlockVolume")("KeyProtectorId.Label")
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'KeyProtectorIdLabel
        '
        Me.KeyProtectorIdLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KeyProtectorIdLabel.AutoEllipsis = True
        Me.KeyProtectorIdLabel.Location = New System.Drawing.Point(326, 48)
        Me.KeyProtectorIdLabel.Name = "KeyProtectorIdLabel"
        Me.KeyProtectorIdLabel.Size = New System.Drawing.Size(331, 16)
        Me.KeyProtectorIdLabel.TabIndex = 2
        Me.KeyProtectorIdLabel.Text = "ID"
        '
        'RPS1
        '
        Me.RPS1.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RPS1.Location = New System.Drawing.Point(24, 93)
        Me.RPS1.MaxLength = 6
        Me.RPS1.Name = "RPS1"
        Me.RPS1.Size = New System.Drawing.Size(72, 25)
        Me.RPS1.TabIndex = 3
        '
        'RPS2
        '
        Me.RPS2.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RPS2.Location = New System.Drawing.Point(119, 93)
        Me.RPS2.MaxLength = 6
        Me.RPS2.Name = "RPS2"
        Me.RPS2.Size = New System.Drawing.Size(72, 25)
        Me.RPS2.TabIndex = 4
        '
        'RPS3
        '
        Me.RPS3.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RPS3.Location = New System.Drawing.Point(214, 93)
        Me.RPS3.MaxLength = 6
        Me.RPS3.Name = "RPS3"
        Me.RPS3.Size = New System.Drawing.Size(72, 25)
        Me.RPS3.TabIndex = 5
        '
        'RPS4
        '
        Me.RPS4.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RPS4.Location = New System.Drawing.Point(309, 93)
        Me.RPS4.MaxLength = 6
        Me.RPS4.Name = "RPS4"
        Me.RPS4.Size = New System.Drawing.Size(72, 25)
        Me.RPS4.TabIndex = 6
        '
        'RPS5
        '
        Me.RPS5.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RPS5.Location = New System.Drawing.Point(404, 93)
        Me.RPS5.MaxLength = 6
        Me.RPS5.Name = "RPS5"
        Me.RPS5.Size = New System.Drawing.Size(72, 25)
        Me.RPS5.TabIndex = 7
        '
        'RPS6
        '
        Me.RPS6.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RPS6.Location = New System.Drawing.Point(499, 93)
        Me.RPS6.MaxLength = 6
        Me.RPS6.Name = "RPS6"
        Me.RPS6.Size = New System.Drawing.Size(72, 25)
        Me.RPS6.TabIndex = 8
        '
        'RPS7
        '
        Me.RPS7.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RPS7.Location = New System.Drawing.Point(594, 93)
        Me.RPS7.MaxLength = 6
        Me.RPS7.Name = "RPS7"
        Me.RPS7.Size = New System.Drawing.Size(72, 25)
        Me.RPS7.TabIndex = 9
        '
        'RPS8
        '
        Me.RPS8.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RPS8.Location = New System.Drawing.Point(689, 93)
        Me.RPS8.MaxLength = 6
        Me.RPS8.Name = "RPS8"
        Me.RPS8.Size = New System.Drawing.Size(72, 25)
        Me.RPS8.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(102, 99)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(11, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "-"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(197, 99)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(11, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "-"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(292, 99)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(11, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "-"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(387, 99)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(11, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "-"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(482, 99)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(11, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "-"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(577, 99)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(11, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "-"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(672, 99)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(11, 13)
        Me.Label10.TabIndex = 17
        Me.Label10.Text = "-"
        '
        'UnlockVolumeDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(784, 201)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.RPS8)
        Me.Controls.Add(Me.RPS7)
        Me.Controls.Add(Me.RPS6)
        Me.Controls.Add(Me.RPS5)
        Me.Controls.Add(Me.RPS4)
        Me.Controls.Add(Me.RPS3)
        Me.Controls.Add(Me.RPS2)
        Me.Controls.Add(Me.RPS1)
        Me.Controls.Add(Me.KeyProtectorIdLabel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UnlockVolumeDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = LocalizationService.ForSection("Designer.BDE.UnlockVolume")("Title")
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents KeyProtectorIdLabel As System.Windows.Forms.Label
    Friend WithEvents RPS1 As System.Windows.Forms.TextBox
    Friend WithEvents RPS2 As System.Windows.Forms.TextBox
    Friend WithEvents RPS3 As System.Windows.Forms.TextBox
    Friend WithEvents RPS4 As System.Windows.Forms.TextBox
    Friend WithEvents RPS5 As System.Windows.Forms.TextBox
    Friend WithEvents RPS6 As System.Windows.Forms.TextBox
    Friend WithEvents RPS7 As System.Windows.Forms.TextBox
    Friend WithEvents RPS8 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label

End Class
