<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserCard
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
        Me.tbUserDisplayName = New DISMTools.CuedTextBox()
        Me.tlpCardBody = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlAccountDisplayName = New System.Windows.Forms.Panel()
        Me.pnlAccountPassword = New System.Windows.Forms.Panel()
        Me.cbShowPassword = New System.Windows.Forms.CheckBox()
        Me.tbUserPassword = New DISMTools.CuedTextBox()
        Me.pnlSystemAccountName = New System.Windows.Forms.Panel()
        Me.btnGenerateUserName = New System.Windows.Forms.Button()
        Me.tbUserAccountName = New DISMTools.CuedTextBox()
        Me.pnlAccountGroup = New System.Windows.Forms.Panel()
        Me.comboUserGroup = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnRemoveUser = New System.Windows.Forms.Button()
        Me.tlpCardBody.SuspendLayout()
        Me.pnlAccountDisplayName.SuspendLayout()
        Me.pnlAccountPassword.SuspendLayout()
        Me.pnlSystemAccountName.SuspendLayout()
        Me.pnlAccountGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbUserDisplayName
        '
        Me.tbUserDisplayName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbUserDisplayName.cueBanner = "User Display Name"
        Me.tbUserDisplayName.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbUserDisplayName.Location = New System.Drawing.Point(3, 1)
        Me.tbUserDisplayName.MaxLength = 256
        Me.tbUserDisplayName.Name = "tbUserDisplayName"
        Me.tbUserDisplayName.Size = New System.Drawing.Size(361, 26)
        Me.tbUserDisplayName.TabIndex = 0
        '
        'tlpCardBody
        '
        Me.tlpCardBody.ColumnCount = 3
        Me.tlpCardBody.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCardBody.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCardBody.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97.0!))
        Me.tlpCardBody.Controls.Add(Me.pnlAccountDisplayName, 0, 0)
        Me.tlpCardBody.Controls.Add(Me.pnlAccountPassword, 1, 0)
        Me.tlpCardBody.Controls.Add(Me.pnlSystemAccountName, 0, 1)
        Me.tlpCardBody.Controls.Add(Me.pnlAccountGroup, 1, 1)
        Me.tlpCardBody.Controls.Add(Me.btnRemoveUser, 2, 0)
        Me.tlpCardBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpCardBody.Location = New System.Drawing.Point(0, 0)
        Me.tlpCardBody.Name = "tlpCardBody"
        Me.tlpCardBody.RowCount = 2
        Me.tlpCardBody.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCardBody.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCardBody.Size = New System.Drawing.Size(844, 71)
        Me.tlpCardBody.TabIndex = 0
        '
        'pnlAccountDisplayName
        '
        Me.pnlAccountDisplayName.Controls.Add(Me.tbUserDisplayName)
        Me.pnlAccountDisplayName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlAccountDisplayName.Location = New System.Drawing.Point(3, 3)
        Me.pnlAccountDisplayName.Name = "pnlAccountDisplayName"
        Me.pnlAccountDisplayName.Size = New System.Drawing.Size(367, 29)
        Me.pnlAccountDisplayName.TabIndex = 0
        '
        'pnlAccountPassword
        '
        Me.pnlAccountPassword.Controls.Add(Me.cbShowPassword)
        Me.pnlAccountPassword.Controls.Add(Me.tbUserPassword)
        Me.pnlAccountPassword.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlAccountPassword.Location = New System.Drawing.Point(376, 3)
        Me.pnlAccountPassword.Name = "pnlAccountPassword"
        Me.pnlAccountPassword.Size = New System.Drawing.Size(367, 29)
        Me.pnlAccountPassword.TabIndex = 0
        '
        'cbShowPassword
        '
        Me.cbShowPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbShowPassword.Appearance = System.Windows.Forms.Appearance.Button
        Me.cbShowPassword.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cbShowPassword.Location = New System.Drawing.Point(282, 3)
        Me.cbShowPassword.Name = "cbShowPassword"
        Me.cbShowPassword.Size = New System.Drawing.Size(82, 23)
        Me.cbShowPassword.TabIndex = 1
        Me.cbShowPassword.Text = "Show"
        Me.cbShowPassword.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.cbShowPassword.UseVisualStyleBackColor = True
        '
        'tbUserPassword
        '
        Me.tbUserPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbUserPassword.cueBanner = "Account Password"
        Me.tbUserPassword.Location = New System.Drawing.Point(4, 4)
        Me.tbUserPassword.Name = "tbUserPassword"
        Me.tbUserPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.tbUserPassword.Size = New System.Drawing.Size(272, 21)
        Me.tbUserPassword.TabIndex = 0
        '
        'pnlSystemAccountName
        '
        Me.pnlSystemAccountName.Controls.Add(Me.btnGenerateUserName)
        Me.pnlSystemAccountName.Controls.Add(Me.tbUserAccountName)
        Me.pnlSystemAccountName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSystemAccountName.Location = New System.Drawing.Point(3, 38)
        Me.pnlSystemAccountName.Name = "pnlSystemAccountName"
        Me.pnlSystemAccountName.Size = New System.Drawing.Size(367, 30)
        Me.pnlSystemAccountName.TabIndex = 2
        '
        'btnGenerateUserName
        '
        Me.btnGenerateUserName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerateUserName.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnGenerateUserName.Location = New System.Drawing.Point(266, 3)
        Me.btnGenerateUserName.Name = "btnGenerateUserName"
        Me.btnGenerateUserName.Size = New System.Drawing.Size(99, 23)
        Me.btnGenerateUserName.TabIndex = 1
        Me.btnGenerateUserName.Text = "Generate..."
        Me.btnGenerateUserName.UseVisualStyleBackColor = True
        '
        'tbUserAccountName
        '
        Me.tbUserAccountName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbUserAccountName.cueBanner = "User name used by the system"
        Me.tbUserAccountName.Location = New System.Drawing.Point(3, 4)
        Me.tbUserAccountName.MaxLength = 20
        Me.tbUserAccountName.Name = "tbUserAccountName"
        Me.tbUserAccountName.Size = New System.Drawing.Size(258, 21)
        Me.tbUserAccountName.TabIndex = 0
        '
        'pnlAccountGroup
        '
        Me.pnlAccountGroup.Controls.Add(Me.comboUserGroup)
        Me.pnlAccountGroup.Controls.Add(Me.Label2)
        Me.pnlAccountGroup.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlAccountGroup.Location = New System.Drawing.Point(376, 38)
        Me.pnlAccountGroup.Name = "pnlAccountGroup"
        Me.pnlAccountGroup.Size = New System.Drawing.Size(367, 30)
        Me.pnlAccountGroup.TabIndex = 3
        '
        'comboUserGroup
        '
        Me.comboUserGroup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboUserGroup.FormattingEnabled = True
        Me.comboUserGroup.Items.AddRange(New Object() {"Administrators", "Users"})
        Me.comboUserGroup.Location = New System.Drawing.Point(111, 4)
        Me.comboUserGroup.Name = "comboUserGroup"
        Me.comboUserGroup.Size = New System.Drawing.Size(254, 21)
        Me.comboUserGroup.TabIndex = 1
        Me.comboUserGroup.Text = "Administrators"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Account Group:"
        '
        'btnRemoveUser
        '
        Me.btnRemoveUser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnRemoveUser.ForeColor = System.Drawing.Color.Black
        Me.btnRemoveUser.Image = Global.DISMTools.My.Resources.Resources.delete_imgindex
        Me.btnRemoveUser.Location = New System.Drawing.Point(749, 3)
        Me.btnRemoveUser.Name = "btnRemoveUser"
        Me.tlpCardBody.SetRowSpan(Me.btnRemoveUser, 2)
        Me.btnRemoveUser.Size = New System.Drawing.Size(92, 65)
        Me.btnRemoveUser.TabIndex = 4
        Me.btnRemoveUser.Text = "Remove User"
        Me.btnRemoveUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnRemoveUser.UseVisualStyleBackColor = True
        '
        'UserCard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.tlpCardBody)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UserCard"
        Me.Size = New System.Drawing.Size(844, 71)
        Me.tlpCardBody.ResumeLayout(False)
        Me.pnlAccountDisplayName.ResumeLayout(False)
        Me.pnlAccountDisplayName.PerformLayout()
        Me.pnlAccountPassword.ResumeLayout(False)
        Me.pnlAccountPassword.PerformLayout()
        Me.pnlSystemAccountName.ResumeLayout(False)
        Me.pnlSystemAccountName.PerformLayout()
        Me.pnlAccountGroup.ResumeLayout(False)
        Me.pnlAccountGroup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tlpCardBody As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents pnlAccountPassword As System.Windows.Forms.Panel
    Friend WithEvents pnlSystemAccountName As System.Windows.Forms.Panel
    Friend WithEvents pnlAccountGroup As System.Windows.Forms.Panel
    Friend WithEvents pnlAccountDisplayName As System.Windows.Forms.Panel
    Private WithEvents tbUserDisplayName As DISMTools.CuedTextBox
    Private WithEvents tbUserPassword As DISMTools.CuedTextBox
    Private WithEvents btnGenerateUserName As System.Windows.Forms.Button
    Private WithEvents tbUserAccountName As DISMTools.CuedTextBox
    Private WithEvents cbShowPassword As System.Windows.Forms.CheckBox
    Private WithEvents comboUserGroup As System.Windows.Forms.ComboBox
    Private WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents btnRemoveUser As System.Windows.Forms.Button

End Class
