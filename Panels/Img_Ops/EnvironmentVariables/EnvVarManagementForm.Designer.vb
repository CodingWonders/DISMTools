<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EnvVarManagementForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EnvVarManagementForm))
        Me.ButtonContainerPanel = New System.Windows.Forms.Panel()
        Me.SaveAllChangesBtn = New System.Windows.Forms.Button()
        Me.HeaderContainerPanel = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.EnvVarContainerSplitPanel = New System.Windows.Forms.SplitContainer()
        Me.EnvVarListPanel = New System.Windows.Forms.Panel()
        Me.SysEnvVarGB = New System.Windows.Forms.GroupBox()
        Me.SysEnvVarPanel = New System.Windows.Forms.Panel()
        Me.SysEnvVarLV = New System.Windows.Forms.ListView()
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SysEnvVarActionPanel = New System.Windows.Forms.Panel()
        Me.RemoveMachineVarButton = New System.Windows.Forms.Button()
        Me.AddMachineVarButton = New System.Windows.Forms.Button()
        Me.UserEnvVarGB = New System.Windows.Forms.GroupBox()
        Me.UserEnvVarPanel = New System.Windows.Forms.Panel()
        Me.UserEnvVarLV = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.UserEnvVarActionPanel = New System.Windows.Forms.Panel()
        Me.RemoveUserVarBtn = New System.Windows.Forms.Button()
        Me.AddUserVarButton = New System.Windows.Forms.Button()
        Me.EnvVarDetailsPanel = New System.Windows.Forms.Panel()
        Me.SaveVarBtn = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.CopyToUserScopeBtn = New System.Windows.Forms.Button()
        Me.CopyToMachineScopeBtn = New System.Windows.Forms.Button()
        Me.MoveToMachineScopeBtn = New System.Windows.Forms.Button()
        Me.MoveToUserScopeBtn = New System.Windows.Forms.Button()
        Me.ButtonContainerPanel.SuspendLayout()
        Me.HeaderContainerPanel.SuspendLayout()
        CType(Me.EnvVarContainerSplitPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EnvVarContainerSplitPanel.Panel1.SuspendLayout()
        Me.EnvVarContainerSplitPanel.Panel2.SuspendLayout()
        Me.EnvVarContainerSplitPanel.SuspendLayout()
        Me.EnvVarListPanel.SuspendLayout()
        Me.SysEnvVarGB.SuspendLayout()
        Me.SysEnvVarPanel.SuspendLayout()
        Me.SysEnvVarActionPanel.SuspendLayout()
        Me.UserEnvVarGB.SuspendLayout()
        Me.UserEnvVarPanel.SuspendLayout()
        Me.UserEnvVarActionPanel.SuspendLayout()
        Me.EnvVarDetailsPanel.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonContainerPanel
        '
        Me.ButtonContainerPanel.Controls.Add(Me.SaveAllChangesBtn)
        Me.ButtonContainerPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ButtonContainerPanel.Location = New System.Drawing.Point(0, 633)
        Me.ButtonContainerPanel.Name = "ButtonContainerPanel"
        Me.ButtonContainerPanel.Size = New System.Drawing.Size(1264, 48)
        Me.ButtonContainerPanel.TabIndex = 0
        '
        'SaveAllChangesBtn
        '
        Me.SaveAllChangesBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SaveAllChangesBtn.Location = New System.Drawing.Point(1115, 13)
        Me.SaveAllChangesBtn.Name = "SaveAllChangesBtn"
        Me.SaveAllChangesBtn.Size = New System.Drawing.Size(137, 23)
        Me.SaveAllChangesBtn.TabIndex = 0
        Me.SaveAllChangesBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Save.Changes.Label")
        Me.SaveAllChangesBtn.UseVisualStyleBackColor = True
        '
        'HeaderContainerPanel
        '
        Me.HeaderContainerPanel.Controls.Add(Me.Label1)
        Me.HeaderContainerPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.HeaderContainerPanel.Location = New System.Drawing.Point(0, 0)
        Me.HeaderContainerPanel.Name = "HeaderContainerPanel"
        Me.HeaderContainerPanel.Size = New System.Drawing.Size(1264, 72)
        Me.HeaderContainerPanel.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoEllipsis = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(1240, 42)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = LocalizationService.ForSection("Designer.EnvVars")("Intro.Message")
        '
        'EnvVarContainerSplitPanel
        '
        Me.EnvVarContainerSplitPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.EnvVarContainerSplitPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EnvVarContainerSplitPanel.IsSplitterFixed = True
        Me.EnvVarContainerSplitPanel.Location = New System.Drawing.Point(0, 72)
        Me.EnvVarContainerSplitPanel.Name = "EnvVarContainerSplitPanel"
        '
        'EnvVarContainerSplitPanel.Panel1
        '
        Me.EnvVarContainerSplitPanel.Panel1.Controls.Add(Me.EnvVarListPanel)
        '
        'EnvVarContainerSplitPanel.Panel2
        '
        Me.EnvVarContainerSplitPanel.Panel2.Controls.Add(Me.EnvVarDetailsPanel)
        Me.EnvVarContainerSplitPanel.Size = New System.Drawing.Size(1264, 561)
        Me.EnvVarContainerSplitPanel.SplitterDistance = 768
        Me.EnvVarContainerSplitPanel.TabIndex = 2
        '
        'EnvVarListPanel
        '
        Me.EnvVarListPanel.Controls.Add(Me.SysEnvVarGB)
        Me.EnvVarListPanel.Controls.Add(Me.UserEnvVarGB)
        Me.EnvVarListPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EnvVarListPanel.Location = New System.Drawing.Point(0, 0)
        Me.EnvVarListPanel.Name = "EnvVarListPanel"
        Me.EnvVarListPanel.Size = New System.Drawing.Size(766, 559)
        Me.EnvVarListPanel.TabIndex = 0
        '
        'SysEnvVarGB
        '
        Me.SysEnvVarGB.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SysEnvVarGB.Controls.Add(Me.SysEnvVarPanel)
        Me.SysEnvVarGB.Location = New System.Drawing.Point(16, 283)
        Me.SysEnvVarGB.Name = "SysEnvVarGB"
        Me.SysEnvVarGB.Size = New System.Drawing.Size(734, 260)
        Me.SysEnvVarGB.TabIndex = 0
        Me.SysEnvVarGB.TabStop = False
        Me.SysEnvVarGB.Text = LocalizationService.ForSection("Designer.EnvVars")("TargetSystem.Label")
        '
        'SysEnvVarPanel
        '
        Me.SysEnvVarPanel.Controls.Add(Me.SysEnvVarLV)
        Me.SysEnvVarPanel.Controls.Add(Me.SysEnvVarActionPanel)
        Me.SysEnvVarPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SysEnvVarPanel.Location = New System.Drawing.Point(3, 17)
        Me.SysEnvVarPanel.Name = "SysEnvVarPanel"
        Me.SysEnvVarPanel.Size = New System.Drawing.Size(728, 240)
        Me.SysEnvVarPanel.TabIndex = 1
        '
        'SysEnvVarLV
        '
        Me.SysEnvVarLV.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4})
        Me.SysEnvVarLV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SysEnvVarLV.FullRowSelect = True
        Me.SysEnvVarLV.Location = New System.Drawing.Point(0, 0)
        Me.SysEnvVarLV.MultiSelect = False
        Me.SysEnvVarLV.Name = "SysEnvVarLV"
        Me.SysEnvVarLV.Size = New System.Drawing.Size(728, 208)
        Me.SysEnvVarLV.TabIndex = 2
        Me.SysEnvVarLV.UseCompatibleStateImageBehavior = False
        Me.SysEnvVarLV.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.EnvVars")("Name.Column")
        Me.ColumnHeader3.Width = 221
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.EnvVars")("Value.Column")
        Me.ColumnHeader4.Width = 476
        '
        'SysEnvVarActionPanel
        '
        Me.SysEnvVarActionPanel.Controls.Add(Me.RemoveMachineVarButton)
        Me.SysEnvVarActionPanel.Controls.Add(Me.AddMachineVarButton)
        Me.SysEnvVarActionPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SysEnvVarActionPanel.Location = New System.Drawing.Point(0, 208)
        Me.SysEnvVarActionPanel.Name = "SysEnvVarActionPanel"
        Me.SysEnvVarActionPanel.Size = New System.Drawing.Size(728, 32)
        Me.SysEnvVarActionPanel.TabIndex = 1
        '
        'RemoveMachineVarButton
        '
        Me.RemoveMachineVarButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RemoveMachineVarButton.Enabled = False
        Me.RemoveMachineVarButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RemoveMachineVarButton.Location = New System.Drawing.Point(560, 6)
        Me.RemoveMachineVarButton.Name = "RemoveMachineVarButton"
        Me.RemoveMachineVarButton.Size = New System.Drawing.Size(161, 23)
        Me.RemoveMachineVarButton.TabIndex = 0
        Me.RemoveMachineVarButton.Text = LocalizationService.ForSection("Designer.EnvVars")("Remove.Machine.Label")
        Me.RemoveMachineVarButton.UseVisualStyleBackColor = True
        '
        'AddMachineVarButton
        '
        Me.AddMachineVarButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddMachineVarButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.AddMachineVarButton.Location = New System.Drawing.Point(393, 6)
        Me.AddMachineVarButton.Name = "AddMachineVarButton"
        Me.AddMachineVarButton.Size = New System.Drawing.Size(161, 23)
        Me.AddMachineVarButton.TabIndex = 0
        Me.AddMachineVarButton.Text = LocalizationService.ForSection("Designer.EnvVars")("Add.Machine.Variable.Button")
        Me.AddMachineVarButton.UseVisualStyleBackColor = True
        '
        'UserEnvVarGB
        '
        Me.UserEnvVarGB.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UserEnvVarGB.Controls.Add(Me.UserEnvVarPanel)
        Me.UserEnvVarGB.Location = New System.Drawing.Point(16, 17)
        Me.UserEnvVarGB.Name = "UserEnvVarGB"
        Me.UserEnvVarGB.Size = New System.Drawing.Size(734, 260)
        Me.UserEnvVarGB.TabIndex = 0
        Me.UserEnvVarGB.TabStop = False
        Me.UserEnvVarGB.Text = LocalizationService.ForSection("Designer.EnvVars")("DefaultUser.Label")
        '
        'UserEnvVarPanel
        '
        Me.UserEnvVarPanel.Controls.Add(Me.UserEnvVarLV)
        Me.UserEnvVarPanel.Controls.Add(Me.UserEnvVarActionPanel)
        Me.UserEnvVarPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserEnvVarPanel.Location = New System.Drawing.Point(3, 17)
        Me.UserEnvVarPanel.Name = "UserEnvVarPanel"
        Me.UserEnvVarPanel.Size = New System.Drawing.Size(728, 240)
        Me.UserEnvVarPanel.TabIndex = 0
        '
        'UserEnvVarLV
        '
        Me.UserEnvVarLV.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.UserEnvVarLV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserEnvVarLV.FullRowSelect = True
        Me.UserEnvVarLV.Location = New System.Drawing.Point(0, 0)
        Me.UserEnvVarLV.MultiSelect = False
        Me.UserEnvVarLV.Name = "UserEnvVarLV"
        Me.UserEnvVarLV.Size = New System.Drawing.Size(728, 208)
        Me.UserEnvVarLV.TabIndex = 1
        Me.UserEnvVarLV.UseCompatibleStateImageBehavior = False
        Me.UserEnvVarLV.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.EnvVars")("Name.Column")
        Me.ColumnHeader1.Width = 221
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.EnvVars")("Value.Column")
        Me.ColumnHeader2.Width = 476
        '
        'UserEnvVarActionPanel
        '
        Me.UserEnvVarActionPanel.Controls.Add(Me.RemoveUserVarBtn)
        Me.UserEnvVarActionPanel.Controls.Add(Me.AddUserVarButton)
        Me.UserEnvVarActionPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.UserEnvVarActionPanel.Location = New System.Drawing.Point(0, 208)
        Me.UserEnvVarActionPanel.Name = "UserEnvVarActionPanel"
        Me.UserEnvVarActionPanel.Size = New System.Drawing.Size(728, 32)
        Me.UserEnvVarActionPanel.TabIndex = 0
        '
        'RemoveUserVarBtn
        '
        Me.RemoveUserVarBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RemoveUserVarBtn.Enabled = False
        Me.RemoveUserVarBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RemoveUserVarBtn.Location = New System.Drawing.Point(560, 6)
        Me.RemoveUserVarBtn.Name = "RemoveUserVarBtn"
        Me.RemoveUserVarBtn.Size = New System.Drawing.Size(161, 23)
        Me.RemoveUserVarBtn.TabIndex = 0
        Me.RemoveUserVarBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Remove.User.Variable.Label")
        Me.RemoveUserVarBtn.UseVisualStyleBackColor = True
        '
        'AddUserVarButton
        '
        Me.AddUserVarButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddUserVarButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.AddUserVarButton.Location = New System.Drawing.Point(393, 6)
        Me.AddUserVarButton.Name = "AddUserVarButton"
        Me.AddUserVarButton.Size = New System.Drawing.Size(161, 23)
        Me.AddUserVarButton.TabIndex = 0
        Me.AddUserVarButton.Text = LocalizationService.ForSection("Designer.EnvVars")("Add.User.Variable.Button")
        Me.AddUserVarButton.UseVisualStyleBackColor = True
        '
        'EnvVarDetailsPanel
        '
        Me.EnvVarDetailsPanel.Controls.Add(Me.SaveVarBtn)
        Me.EnvVarDetailsPanel.Controls.Add(Me.TextBox2)
        Me.EnvVarDetailsPanel.Controls.Add(Me.TextBox3)
        Me.EnvVarDetailsPanel.Controls.Add(Me.TextBox1)
        Me.EnvVarDetailsPanel.Controls.Add(Me.Label4)
        Me.EnvVarDetailsPanel.Controls.Add(Me.Label7)
        Me.EnvVarDetailsPanel.Controls.Add(Me.Label6)
        Me.EnvVarDetailsPanel.Controls.Add(Me.Label5)
        Me.EnvVarDetailsPanel.Controls.Add(Me.Label3)
        Me.EnvVarDetailsPanel.Controls.Add(Me.Label2)
        Me.EnvVarDetailsPanel.Controls.Add(Me.PictureBox1)
        Me.EnvVarDetailsPanel.Controls.Add(Me.TableLayoutPanel1)
        Me.EnvVarDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EnvVarDetailsPanel.Enabled = False
        Me.EnvVarDetailsPanel.Location = New System.Drawing.Point(0, 0)
        Me.EnvVarDetailsPanel.Name = "EnvVarDetailsPanel"
        Me.EnvVarDetailsPanel.Size = New System.Drawing.Size(490, 559)
        Me.EnvVarDetailsPanel.TabIndex = 1
        '
        'SaveVarBtn
        '
        Me.SaveVarBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveVarBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SaveVarBtn.Location = New System.Drawing.Point(369, 516)
        Me.SaveVarBtn.Name = "SaveVarBtn"
        Me.SaveVarBtn.Size = New System.Drawing.Size(99, 23)
        Me.SaveVarBtn.TabIndex = 5
        Me.SaveVarBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("SaveVariable.Label")
        Me.SaveVarBtn.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox2.Location = New System.Drawing.Point(22, 142)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(449, 21)
        Me.TextBox2.TabIndex = 3
        '
        'TextBox3
        '
        Me.TextBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox3.Location = New System.Drawing.Point(22, 254)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox3.Size = New System.Drawing.Size(449, 191)
        Me.TextBox3.TabIndex = 3
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(22, 89)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(449, 21)
        Me.TextBox1.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 126)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = LocalizationService.ForSection("Designer.EnvVars")("Scope.Label")
        '
        'Label7
        '
        Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoEllipsis = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(19, 472)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(452, 36)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = LocalizationService.ForSection("Designer.EnvVars")("Hierarchical.Values.Message")
        Me.Label7.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(19, 448)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(345, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = LocalizationService.ForSection("Designer.EnvVars")("Variables.Location.Label")
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(19, 238)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = LocalizationService.ForSection("Designer.EnvVars")("Value.Label")
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(19, 73)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = LocalizationService.ForSection("Designer.EnvVars")("Name.Label")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(54, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(256, 19)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = LocalizationService.ForSection("Designer.EnvVars")("VariableInfo.Label")
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DISMTools.My.Resources.Resources.envvar_pic_32px
        Me.PictureBox1.Location = New System.Drawing.Point(16, 16)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.CopyToUserScopeBtn, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.CopyToMachineScopeBtn, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.MoveToMachineScopeBtn, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.MoveToUserScopeBtn, 1, 0)
        Me.TableLayoutPanel1.Enabled = False
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(22, 169)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(449, 58)
        Me.TableLayoutPanel1.TabIndex = 6
        '
        'CopyToUserScopeBtn
        '
        Me.CopyToUserScopeBtn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CopyToUserScopeBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CopyToUserScopeBtn.Location = New System.Drawing.Point(227, 32)
        Me.CopyToUserScopeBtn.Name = "CopyToUserScopeBtn"
        Me.CopyToUserScopeBtn.Size = New System.Drawing.Size(219, 23)
        Me.CopyToUserScopeBtn.TabIndex = 7
        Me.CopyToUserScopeBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Copy.Default.User.Label")
        Me.CopyToUserScopeBtn.UseVisualStyleBackColor = True
        '
        'CopyToMachineScopeBtn
        '
        Me.CopyToMachineScopeBtn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CopyToMachineScopeBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CopyToMachineScopeBtn.Location = New System.Drawing.Point(3, 32)
        Me.CopyToMachineScopeBtn.Name = "CopyToMachineScopeBtn"
        Me.CopyToMachineScopeBtn.Size = New System.Drawing.Size(218, 23)
        Me.CopyToMachineScopeBtn.TabIndex = 5
        Me.CopyToMachineScopeBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Copy.Machine.Scope.Label")
        Me.CopyToMachineScopeBtn.UseVisualStyleBackColor = True
        '
        'MoveToMachineScopeBtn
        '
        Me.MoveToMachineScopeBtn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MoveToMachineScopeBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.MoveToMachineScopeBtn.Location = New System.Drawing.Point(3, 3)
        Me.MoveToMachineScopeBtn.Name = "MoveToMachineScopeBtn"
        Me.MoveToMachineScopeBtn.Size = New System.Drawing.Size(218, 23)
        Me.MoveToMachineScopeBtn.TabIndex = 4
        Me.MoveToMachineScopeBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Move.Machine.Scope.Label")
        Me.MoveToMachineScopeBtn.UseVisualStyleBackColor = True
        '
        'MoveToUserScopeBtn
        '
        Me.MoveToUserScopeBtn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MoveToUserScopeBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.MoveToUserScopeBtn.Location = New System.Drawing.Point(227, 3)
        Me.MoveToUserScopeBtn.Name = "MoveToUserScopeBtn"
        Me.MoveToUserScopeBtn.Size = New System.Drawing.Size(219, 23)
        Me.MoveToUserScopeBtn.TabIndex = 4
        Me.MoveToUserScopeBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Move.Default.User.Label")
        Me.MoveToUserScopeBtn.UseVisualStyleBackColor = True
        '
        'EnvVarManagementForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1264, 681)
        Me.Controls.Add(Me.EnvVarContainerSplitPanel)
        Me.Controls.Add(Me.HeaderContainerPanel)
        Me.Controls.Add(Me.ButtonContainerPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1280, 720)
        Me.Name = "EnvVarManagementForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = LocalizationService.ForSection("Designer.EnvVars")("SystemVariables.Label")
        Me.ButtonContainerPanel.ResumeLayout(False)
        Me.HeaderContainerPanel.ResumeLayout(False)
        Me.EnvVarContainerSplitPanel.Panel1.ResumeLayout(False)
        Me.EnvVarContainerSplitPanel.Panel2.ResumeLayout(False)
        CType(Me.EnvVarContainerSplitPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EnvVarContainerSplitPanel.ResumeLayout(False)
        Me.EnvVarListPanel.ResumeLayout(False)
        Me.SysEnvVarGB.ResumeLayout(False)
        Me.SysEnvVarPanel.ResumeLayout(False)
        Me.SysEnvVarActionPanel.ResumeLayout(False)
        Me.UserEnvVarGB.ResumeLayout(False)
        Me.UserEnvVarPanel.ResumeLayout(False)
        Me.UserEnvVarActionPanel.ResumeLayout(False)
        Me.EnvVarDetailsPanel.ResumeLayout(False)
        Me.EnvVarDetailsPanel.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents HeaderContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents EnvVarContainerSplitPanel As System.Windows.Forms.SplitContainer
    Friend WithEvents EnvVarListPanel As System.Windows.Forms.Panel
    Friend WithEvents SysEnvVarGB As System.Windows.Forms.GroupBox
    Friend WithEvents SysEnvVarPanel As System.Windows.Forms.Panel
    Friend WithEvents SysEnvVarLV As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents SysEnvVarActionPanel As System.Windows.Forms.Panel
    Friend WithEvents UserEnvVarGB As System.Windows.Forms.GroupBox
    Friend WithEvents UserEnvVarPanel As System.Windows.Forms.Panel
    Friend WithEvents UserEnvVarLV As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents UserEnvVarActionPanel As System.Windows.Forms.Panel
    Friend WithEvents EnvVarDetailsPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents SaveVarBtn As System.Windows.Forms.Button
    Friend WithEvents MoveToUserScopeBtn As System.Windows.Forms.Button
    Friend WithEvents MoveToMachineScopeBtn As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents CopyToUserScopeBtn As System.Windows.Forms.Button
    Friend WithEvents CopyToMachineScopeBtn As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents SaveAllChangesBtn As System.Windows.Forms.Button
    Friend WithEvents RemoveUserVarBtn As System.Windows.Forms.Button
    Friend WithEvents AddUserVarButton As System.Windows.Forms.Button
    Friend WithEvents RemoveMachineVarButton As System.Windows.Forms.Button
    Friend WithEvents AddMachineVarButton As System.Windows.Forms.Button
End Class
