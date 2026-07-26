<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AppxFilterAssistantDialog
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
        Me.NameFilterRadioButton = New System.Windows.Forms.RadioButton()
        Me.RegStatusRadioButton = New System.Windows.Forms.RadioButton()
        Me.RegStatusPanel = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RegStatusComboBox = New System.Windows.Forms.ComboBox()
        Me.UserAccountLV = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PackageNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SelectedUserDetailsTextBox = New System.Windows.Forms.TextBox()
        Me.SystemUserFilterPanel = New System.Windows.Forms.Panel()
        Me.UserDetailsPanel = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.RegStatusPanel.SuspendLayout()
        Me.SystemUserFilterPanel.SuspendLayout()
        Me.UserDetailsPanel.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(546, 320)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("Apply.Button")
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
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("Clear.Button")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(178, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("FilterBy.Label")
        '
        'NameFilterRadioButton
        '
        Me.NameFilterRadioButton.AutoSize = True
        Me.NameFilterRadioButton.Checked = True
        Me.NameFilterRadioButton.Location = New System.Drawing.Point(24, 36)
        Me.NameFilterRadioButton.Name = "NameFilterRadioButton"
        Me.NameFilterRadioButton.Size = New System.Drawing.Size(56, 17)
        Me.NameFilterRadioButton.TabIndex = 4
        Me.NameFilterRadioButton.TabStop = True
        Me.NameFilterRadioButton.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("Name.RadioButton")
        Me.NameFilterRadioButton.UseVisualStyleBackColor = True
        '
        'RegStatusRadioButton
        '
        Me.RegStatusRadioButton.AutoSize = True
        Me.RegStatusRadioButton.Location = New System.Drawing.Point(24, 62)
        Me.RegStatusRadioButton.Name = "RegStatusRadioButton"
        Me.RegStatusRadioButton.Size = New System.Drawing.Size(116, 17)
        Me.RegStatusRadioButton.TabIndex = 4
        Me.RegStatusRadioButton.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegistrationStatus.RadioButton")
        Me.RegStatusRadioButton.UseVisualStyleBackColor = True
        '
        'RegStatusPanel
        '
        Me.RegStatusPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RegStatusPanel.Controls.Add(Me.SystemUserFilterPanel)
        Me.RegStatusPanel.Controls.Add(Me.RegStatusComboBox)
        Me.RegStatusPanel.Controls.Add(Me.Label2)
        Me.RegStatusPanel.Enabled = False
        Me.RegStatusPanel.Location = New System.Drawing.Point(41, 85)
        Me.RegStatusPanel.Name = "RegStatusPanel"
        Me.RegStatusPanel.Size = New System.Drawing.Size(651, 229)
        Me.RegStatusPanel.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(196, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredTo.Label")
        '
        'RegStatusComboBox
        '
        Me.RegStatusComboBox.FormattingEnabled = True
        Me.RegStatusComboBox.Items.AddRange(New Object() {LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredToNoOne.Item"), LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredToAnyone.Item"), LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredToMe.Item"), LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredToUser.Item")})
        Me.RegStatusComboBox.Location = New System.Drawing.Point(232, 13)
        Me.RegStatusComboBox.Name = "RegStatusComboBox"
        Me.RegStatusComboBox.Size = New System.Drawing.Size(406, 21)
        Me.RegStatusComboBox.TabIndex = 1
        Me.RegStatusComboBox.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredToMe.Item")
        '
        'UserAccountLV
        '
        Me.UserAccountLV.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.UserAccountLV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserAccountLV.FullRowSelect = True
        Me.UserAccountLV.Location = New System.Drawing.Point(0, 0)
        Me.UserAccountLV.MultiSelect = False
        Me.UserAccountLV.Name = "UserAccountLV"
        Me.UserAccountLV.Size = New System.Drawing.Size(622, 119)
        Me.UserAccountLV.TabIndex = 2
        Me.UserAccountLV.UseCompatibleStateImageBehavior = False
        Me.UserAccountLV.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("AccountName.Column")
        Me.ColumnHeader1.Width = 128
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("DisplayName.Column")
        Me.ColumnHeader2.Width = 192
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("Sid.Column")
        Me.ColumnHeader3.Width = 272
        '
        'PackageNameTextBox
        '
        Me.PackageNameTextBox.Location = New System.Drawing.Point(86, 35)
        Me.PackageNameTextBox.Name = "PackageNameTextBox"
        Me.PackageNameTextBox.Size = New System.Drawing.Size(606, 21)
        Me.PackageNameTextBox.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(373, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("SelectUser.Message")
        '
        'SelectedUserDetailsTextBox
        '
        Me.SelectedUserDetailsTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SelectedUserDetailsTextBox.Location = New System.Drawing.Point(10, 32)
        Me.SelectedUserDetailsTextBox.Name = "SelectedUserDetailsTextBox"
        Me.SelectedUserDetailsTextBox.ReadOnly = True
        Me.SelectedUserDetailsTextBox.Size = New System.Drawing.Size(602, 21)
        Me.SelectedUserDetailsTextBox.TabIndex = 4
        '
        'SystemUserFilterPanel
        '
        Me.SystemUserFilterPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SystemUserFilterPanel.Controls.Add(Me.UserAccountLV)
        Me.SystemUserFilterPanel.Controls.Add(Me.UserDetailsPanel)
        Me.SystemUserFilterPanel.Location = New System.Drawing.Point(16, 40)
        Me.SystemUserFilterPanel.Name = "SystemUserFilterPanel"
        Me.SystemUserFilterPanel.Size = New System.Drawing.Size(622, 181)
        Me.SystemUserFilterPanel.TabIndex = 5
        '
        'UserDetailsPanel
        '
        Me.UserDetailsPanel.Controls.Add(Me.Label3)
        Me.UserDetailsPanel.Controls.Add(Me.SelectedUserDetailsTextBox)
        Me.UserDetailsPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.UserDetailsPanel.Location = New System.Drawing.Point(0, 119)
        Me.UserDetailsPanel.Name = "UserDetailsPanel"
        Me.UserDetailsPanel.Size = New System.Drawing.Size(622, 62)
        Me.UserDetailsPanel.TabIndex = 5
        '
        'AppxFilterAssistantDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(704, 361)
        Me.Controls.Add(Me.PackageNameTextBox)
        Me.Controls.Add(Me.RegStatusPanel)
        Me.Controls.Add(Me.RegStatusRadioButton)
        Me.Controls.Add(Me.NameFilterRadioButton)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AppxFilterAssistantDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("Title")
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.RegStatusPanel.ResumeLayout(False)
        Me.RegStatusPanel.PerformLayout()
        Me.SystemUserFilterPanel.ResumeLayout(False)
        Me.UserDetailsPanel.ResumeLayout(False)
        Me.UserDetailsPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents NameFilterRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents RegStatusRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents RegStatusPanel As System.Windows.Forms.Panel
    Friend WithEvents RegStatusComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents UserAccountLV As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents PackageNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents SelectedUserDetailsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SystemUserFilterPanel As System.Windows.Forms.Panel
    Friend WithEvents UserDetailsPanel As System.Windows.Forms.Panel

End Class
