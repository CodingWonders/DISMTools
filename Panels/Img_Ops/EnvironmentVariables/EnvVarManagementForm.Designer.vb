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
        Me.ButtonContainerPanel = New System.Windows.Forms.Panel()
        Me.HeaderContainerPanel = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.EnvVarContainerSplitPanel = New System.Windows.Forms.SplitContainer()
        Me.EnvVarListPanel = New System.Windows.Forms.Panel()
        Me.EnvVarDetailsPanel = New System.Windows.Forms.Panel()
        Me.UserEnvVarGB = New System.Windows.Forms.GroupBox()
        Me.SysEnvVarGB = New System.Windows.Forms.GroupBox()
        Me.UserEnvVarPanel = New System.Windows.Forms.Panel()
        Me.SysEnvVarPanel = New System.Windows.Forms.Panel()
        Me.UserEnvVarActionPanel = New System.Windows.Forms.Panel()
        Me.SysEnvVarActionPanel = New System.Windows.Forms.Panel()
        Me.UserEnvVarLV = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SysEnvVarLV = New System.Windows.Forms.ListView()
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.HeaderContainerPanel.SuspendLayout()
        CType(Me.EnvVarContainerSplitPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EnvVarContainerSplitPanel.Panel1.SuspendLayout()
        Me.EnvVarContainerSplitPanel.Panel2.SuspendLayout()
        Me.EnvVarContainerSplitPanel.SuspendLayout()
        Me.EnvVarListPanel.SuspendLayout()
        Me.UserEnvVarGB.SuspendLayout()
        Me.SysEnvVarGB.SuspendLayout()
        Me.UserEnvVarPanel.SuspendLayout()
        Me.SysEnvVarPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonContainerPanel
        '
        Me.ButtonContainerPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ButtonContainerPanel.Location = New System.Drawing.Point(0, 633)
        Me.ButtonContainerPanel.Name = "ButtonContainerPanel"
        Me.ButtonContainerPanel.Size = New System.Drawing.Size(1264, 48)
        Me.ButtonContainerPanel.TabIndex = 0
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
        Me.Label1.Text = "This tool lets you view and manage the environment variables of this target image" & _
    ". Click the Save button to save any changes made to the environment variables."
        '
        'EnvVarContainerSplitPanel
        '
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
        Me.EnvVarListPanel.Size = New System.Drawing.Size(768, 561)
        Me.EnvVarListPanel.TabIndex = 0
        '
        'EnvVarDetailsPanel
        '
        Me.EnvVarDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EnvVarDetailsPanel.Location = New System.Drawing.Point(0, 0)
        Me.EnvVarDetailsPanel.Name = "EnvVarDetailsPanel"
        Me.EnvVarDetailsPanel.Size = New System.Drawing.Size(492, 561)
        Me.EnvVarDetailsPanel.TabIndex = 1
        '
        'UserEnvVarGB
        '
        Me.UserEnvVarGB.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UserEnvVarGB.Controls.Add(Me.UserEnvVarPanel)
        Me.UserEnvVarGB.Location = New System.Drawing.Point(16, 17)
        Me.UserEnvVarGB.Name = "UserEnvVarGB"
        Me.UserEnvVarGB.Size = New System.Drawing.Size(736, 260)
        Me.UserEnvVarGB.TabIndex = 0
        Me.UserEnvVarGB.TabStop = False
        Me.UserEnvVarGB.Text = "Environment variables for default user profiles"
        '
        'SysEnvVarGB
        '
        Me.SysEnvVarGB.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SysEnvVarGB.Controls.Add(Me.SysEnvVarPanel)
        Me.SysEnvVarGB.Location = New System.Drawing.Point(16, 283)
        Me.SysEnvVarGB.Name = "SysEnvVarGB"
        Me.SysEnvVarGB.Size = New System.Drawing.Size(736, 260)
        Me.SysEnvVarGB.TabIndex = 0
        Me.SysEnvVarGB.TabStop = False
        Me.SysEnvVarGB.Text = "Environment variables for the target system"
        '
        'UserEnvVarPanel
        '
        Me.UserEnvVarPanel.Controls.Add(Me.UserEnvVarLV)
        Me.UserEnvVarPanel.Controls.Add(Me.UserEnvVarActionPanel)
        Me.UserEnvVarPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserEnvVarPanel.Location = New System.Drawing.Point(3, 17)
        Me.UserEnvVarPanel.Name = "UserEnvVarPanel"
        Me.UserEnvVarPanel.Size = New System.Drawing.Size(730, 240)
        Me.UserEnvVarPanel.TabIndex = 0
        '
        'SysEnvVarPanel
        '
        Me.SysEnvVarPanel.Controls.Add(Me.SysEnvVarLV)
        Me.SysEnvVarPanel.Controls.Add(Me.SysEnvVarActionPanel)
        Me.SysEnvVarPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SysEnvVarPanel.Location = New System.Drawing.Point(3, 17)
        Me.SysEnvVarPanel.Name = "SysEnvVarPanel"
        Me.SysEnvVarPanel.Size = New System.Drawing.Size(730, 240)
        Me.SysEnvVarPanel.TabIndex = 1
        '
        'UserEnvVarActionPanel
        '
        Me.UserEnvVarActionPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.UserEnvVarActionPanel.Location = New System.Drawing.Point(0, 208)
        Me.UserEnvVarActionPanel.Name = "UserEnvVarActionPanel"
        Me.UserEnvVarActionPanel.Size = New System.Drawing.Size(730, 32)
        Me.UserEnvVarActionPanel.TabIndex = 0
        '
        'SysEnvVarActionPanel
        '
        Me.SysEnvVarActionPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SysEnvVarActionPanel.Location = New System.Drawing.Point(0, 208)
        Me.SysEnvVarActionPanel.Name = "SysEnvVarActionPanel"
        Me.SysEnvVarActionPanel.Size = New System.Drawing.Size(730, 32)
        Me.SysEnvVarActionPanel.TabIndex = 1
        '
        'UserEnvVarLV
        '
        Me.UserEnvVarLV.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.UserEnvVarLV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UserEnvVarLV.Location = New System.Drawing.Point(0, 0)
        Me.UserEnvVarLV.Name = "UserEnvVarLV"
        Me.UserEnvVarLV.Size = New System.Drawing.Size(730, 208)
        Me.UserEnvVarLV.TabIndex = 1
        Me.UserEnvVarLV.UseCompatibleStateImageBehavior = False
        Me.UserEnvVarLV.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 221
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Value"
        Me.ColumnHeader2.Width = 476
        '
        'SysEnvVarLV
        '
        Me.SysEnvVarLV.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4})
        Me.SysEnvVarLV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SysEnvVarLV.Location = New System.Drawing.Point(0, 0)
        Me.SysEnvVarLV.Name = "SysEnvVarLV"
        Me.SysEnvVarLV.Size = New System.Drawing.Size(730, 208)
        Me.SysEnvVarLV.TabIndex = 2
        Me.SysEnvVarLV.UseCompatibleStateImageBehavior = False
        Me.SysEnvVarLV.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Name"
        Me.ColumnHeader3.Width = 221
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Value"
        Me.ColumnHeader4.Width = 476
        '
        'EnvVarManagementForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 681)
        Me.Controls.Add(Me.EnvVarContainerSplitPanel)
        Me.Controls.Add(Me.HeaderContainerPanel)
        Me.Controls.Add(Me.ButtonContainerPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimumSize = New System.Drawing.Size(1280, 720)
        Me.Name = "EnvVarManagementForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "System Environment Variable Management"
        Me.HeaderContainerPanel.ResumeLayout(False)
        Me.EnvVarContainerSplitPanel.Panel1.ResumeLayout(False)
        Me.EnvVarContainerSplitPanel.Panel2.ResumeLayout(False)
        CType(Me.EnvVarContainerSplitPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EnvVarContainerSplitPanel.ResumeLayout(False)
        Me.EnvVarListPanel.ResumeLayout(False)
        Me.UserEnvVarGB.ResumeLayout(False)
        Me.SysEnvVarGB.ResumeLayout(False)
        Me.UserEnvVarPanel.ResumeLayout(False)
        Me.SysEnvVarPanel.ResumeLayout(False)
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
End Class
