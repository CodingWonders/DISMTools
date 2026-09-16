<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DisplayNameFormatterDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DisplayNameFormatterDialog))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tlpDefaultOptions = New System.Windows.Forms.TableLayoutPanel()
        Me.btnOneWord = New System.Windows.Forms.Button()
        Me.btnTwoWords = New System.Windows.Forms.Button()
        Me.btnThreeWords = New System.Windows.Forms.Button()
        Me.btnFourWords = New System.Windows.Forms.Button()
        Me.btnFiveWords = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbCustomUserFormat = New System.Windows.Forms.TextBox()
        Me.btnCustomFormat = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblSourceDispName = New System.Windows.Forms.Label()
        Me.lblTargetFormattedName = New System.Windows.Forms.Label()
        Me.tlpDefaultOptions.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoEllipsis = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(471, 48)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = resources.GetString("Label1.Text")
        '
        'tlpDefaultOptions
        '
        Me.tlpDefaultOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpDefaultOptions.ColumnCount = 1
        Me.tlpDefaultOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpDefaultOptions.Controls.Add(Me.btnOneWord, 0, 0)
        Me.tlpDefaultOptions.Controls.Add(Me.btnTwoWords, 0, 1)
        Me.tlpDefaultOptions.Controls.Add(Me.btnThreeWords, 0, 2)
        Me.tlpDefaultOptions.Controls.Add(Me.btnFourWords, 0, 3)
        Me.tlpDefaultOptions.Controls.Add(Me.btnFiveWords, 0, 4)
        Me.tlpDefaultOptions.Location = New System.Drawing.Point(16, 65)
        Me.tlpDefaultOptions.Name = "tlpDefaultOptions"
        Me.tlpDefaultOptions.RowCount = 5
        Me.tlpDefaultOptions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tlpDefaultOptions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tlpDefaultOptions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tlpDefaultOptions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tlpDefaultOptions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tlpDefaultOptions.Size = New System.Drawing.Size(468, 160)
        Me.tlpDefaultOptions.TabIndex = 1
        '
        'btnOneWord
        '
        Me.btnOneWord.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnOneWord.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnOneWord.Location = New System.Drawing.Point(3, 3)
        Me.btnOneWord.Name = "btnOneWord"
        Me.btnOneWord.Size = New System.Drawing.Size(462, 26)
        Me.btnOneWord.TabIndex = 0
        Me.btnOneWord.Text = "Continue with"
        Me.btnOneWord.UseVisualStyleBackColor = True
        '
        'btnTwoWords
        '
        Me.btnTwoWords.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnTwoWords.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnTwoWords.Location = New System.Drawing.Point(3, 35)
        Me.btnTwoWords.Name = "btnTwoWords"
        Me.btnTwoWords.Size = New System.Drawing.Size(462, 26)
        Me.btnTwoWords.TabIndex = 0
        Me.btnTwoWords.Text = "Continue with"
        Me.btnTwoWords.UseVisualStyleBackColor = True
        '
        'btnThreeWords
        '
        Me.btnThreeWords.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnThreeWords.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnThreeWords.Location = New System.Drawing.Point(3, 67)
        Me.btnThreeWords.Name = "btnThreeWords"
        Me.btnThreeWords.Size = New System.Drawing.Size(462, 26)
        Me.btnThreeWords.TabIndex = 0
        Me.btnThreeWords.Text = "Continue with"
        Me.btnThreeWords.UseVisualStyleBackColor = True
        '
        'btnFourWords
        '
        Me.btnFourWords.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnFourWords.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnFourWords.Location = New System.Drawing.Point(3, 99)
        Me.btnFourWords.Name = "btnFourWords"
        Me.btnFourWords.Size = New System.Drawing.Size(462, 26)
        Me.btnFourWords.TabIndex = 0
        Me.btnFourWords.Text = "Continue with"
        Me.btnFourWords.UseVisualStyleBackColor = True
        '
        'btnFiveWords
        '
        Me.btnFiveWords.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnFiveWords.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnFiveWords.Location = New System.Drawing.Point(3, 131)
        Me.btnFiveWords.Name = "btnFiveWords"
        Me.btnFiveWords.Size = New System.Drawing.Size(462, 26)
        Me.btnFiveWords.TabIndex = 0
        Me.btnFiveWords.Text = "Continue with"
        Me.btnFiveWords.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoEllipsis = True
        Me.Label2.Location = New System.Drawing.Point(13, 234)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(471, 16)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "...or you can continue with the following custom format:"
        '
        'tbCustomUserFormat
        '
        Me.tbCustomUserFormat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbCustomUserFormat.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbCustomUserFormat.Location = New System.Drawing.Point(16, 259)
        Me.tbCustomUserFormat.Name = "tbCustomUserFormat"
        Me.tbCustomUserFormat.Size = New System.Drawing.Size(468, 25)
        Me.tbCustomUserFormat.TabIndex = 2
        '
        'btnCustomFormat
        '
        Me.btnCustomFormat.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnCustomFormat.Location = New System.Drawing.Point(16, 345)
        Me.btnCustomFormat.Name = "btnCustomFormat"
        Me.btnCustomFormat.Size = New System.Drawing.Size(468, 23)
        Me.btnCustomFormat.TabIndex = 3
        Me.btnCustomFormat.Text = "Continue with this format"
        Me.btnCustomFormat.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 374)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(8)
        Me.GroupBox1.Size = New System.Drawing.Size(468, 175)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Format Help"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoEllipsis = True
        Me.Label3.Location = New System.Drawing.Point(8, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(452, 145)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = resources.GetString("Label3.Text")
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.2649612!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.7350464!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblSourceDispName, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblTargetFormattedName, 1, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(16, 291)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(468, 48)
        Me.TableLayoutPanel1.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoEllipsis = True
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label5.Location = New System.Drawing.Point(3, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(145, 24)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Formatted Account Name:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoEllipsis = True
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label4.Location = New System.Drawing.Point(3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(145, 24)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Account Display Name:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSourceDispName
        '
        Me.lblSourceDispName.AutoEllipsis = True
        Me.lblSourceDispName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblSourceDispName.Location = New System.Drawing.Point(154, 0)
        Me.lblSourceDispName.Name = "lblSourceDispName"
        Me.lblSourceDispName.Size = New System.Drawing.Size(311, 24)
        Me.lblSourceDispName.TabIndex = 0
        Me.lblSourceDispName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTargetFormattedName
        '
        Me.lblTargetFormattedName.AutoEllipsis = True
        Me.lblTargetFormattedName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTargetFormattedName.Location = New System.Drawing.Point(154, 24)
        Me.lblTargetFormattedName.Name = "lblTargetFormattedName"
        Me.lblTargetFormattedName.Size = New System.Drawing.Size(311, 24)
        Me.lblTargetFormattedName.TabIndex = 0
        Me.lblTargetFormattedName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DisplayNameFormatterDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(496, 561)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCustomFormat)
        Me.Controls.Add(Me.tbCustomUserFormat)
        Me.Controls.Add(Me.tlpDefaultOptions)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DisplayNameFormatterDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "User Account Name Required"
        Me.tlpDefaultOptions.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tlpDefaultOptions As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnOneWord As System.Windows.Forms.Button
    Friend WithEvents btnTwoWords As System.Windows.Forms.Button
    Friend WithEvents btnThreeWords As System.Windows.Forms.Button
    Friend WithEvents btnFourWords As System.Windows.Forms.Button
    Friend WithEvents btnFiveWords As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbCustomUserFormat As System.Windows.Forms.TextBox
    Friend WithEvents btnCustomFormat As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblSourceDispName As System.Windows.Forms.Label
    Friend WithEvents lblTargetFormattedName As System.Windows.Forms.Label

End Class
