<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Actions_MainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Actions_MainForm))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton5 = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.BackgroundProcessesButton = New System.Windows.Forms.ToolStripSplitButton()
        Me.MenuDesc = New System.Windows.Forms.ToolStripStatusLabel()
        Me.DockPanel1 = New WeifenLuo.WinFormsUI.Docking.DockPanel()
        Me.VS2012BlueTheme1 = New WeifenLuo.WinFormsUI.Docking.VS2012BlueTheme()
        Me.VS2012DarkTheme1 = New WeifenLuo.WinFormsUI.Docking.VS2012DarkTheme()
        Me.VS2012LightTheme1 = New WeifenLuo.WinFormsUI.Docking.VS2012LightTheme()
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ActionToolStripMenuItem, Me.TestingToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1264, 24)
        Me.MenuStrip1.TabIndex = 5
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'ActionToolStripMenuItem
        '
        Me.ActionToolStripMenuItem.Name = "ActionToolStripMenuItem"
        Me.ActionToolStripMenuItem.Size = New System.Drawing.Size(54, 20)
        Me.ActionToolStripMenuItem.Text = "Action"
        '
        'TestingToolStripMenuItem
        '
        Me.TestingToolStripMenuItem.Name = "TestingToolStripMenuItem"
        Me.TestingToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.TestingToolStripMenuItem.Text = "Testing"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2, Me.ToolStripButton3, Me.ToolStripSeparator2, Me.ToolStripButton4, Me.ToolStripSeparator3, Me.ToolStripButton5})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1264, 25)
        Me.ToolStrip1.TabIndex = 6
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.DISMTools.My.Resources.Resources.newfile
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "New..."
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = Global.DISMTools.My.Resources.Resources.openfile
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton2.Text = "Open..."
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton3.Image = Global.DISMTools.My.Resources.Resources.save_glyph
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton3.Text = "Save..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton4
        '
        Me.ToolStripButton4.Image = Global.DISMTools.My.Resources.Resources.action_testing_light
        Me.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton4.Name = "ToolStripButton4"
        Me.ToolStripButton4.Size = New System.Drawing.Size(102, 22)
        Me.ToolStripButton4.Text = "Test action file"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton5
        '
        Me.ToolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton5.Image = Global.DISMTools.My.Resources.Resources.edit_action_props_light
        Me.ToolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton5.Name = "ToolStripButton5"
        Me.ToolStripButton5.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton5.Text = "Edit properties..."
        '
        'StatusStrip
        '
        Me.StatusStrip.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BackgroundProcessesButton, Me.MenuDesc})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 655)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1264, 26)
        Me.StatusStrip.TabIndex = 7
        Me.StatusStrip.Text = "Status"
        '
        'BackgroundProcessesButton
        '
        Me.BackgroundProcessesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BackgroundProcessesButton.DropDownButtonWidth = 0
        Me.BackgroundProcessesButton.Image = Global.DISMTools.My.Resources.Resources.bg_ops_complete
        Me.BackgroundProcessesButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.BackgroundProcessesButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BackgroundProcessesButton.Name = "BackgroundProcessesButton"
        Me.BackgroundProcessesButton.Size = New System.Drawing.Size(25, 24)
        Me.BackgroundProcessesButton.ToolTipText = "View background processes"
        '
        'MenuDesc
        '
        Me.MenuDesc.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuDesc.Name = "MenuDesc"
        Me.MenuDesc.Size = New System.Drawing.Size(39, 21)
        Me.MenuDesc.Text = "Ready"
        '
        'DockPanel1
        '
        Me.DockPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DockPanel1.DockBackColor = System.Drawing.Color.FromArgb(CType(CType(41, Byte), Integer), CType(CType(57, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.DockPanel1.Location = New System.Drawing.Point(0, 49)
        Me.DockPanel1.Name = "DockPanel1"
        Me.DockPanel1.Padding = New System.Windows.Forms.Padding(6)
        Me.DockPanel1.ShowAutoHideContentOnHover = False
        Me.DockPanel1.Size = New System.Drawing.Size(1264, 606)
        Me.DockPanel1.TabIndex = 8
        Me.DockPanel1.Theme = Me.VS2012BlueTheme1
        '
        'Actions_MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(1264, 681)
        Me.Controls.Add(Me.DockPanel1)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(1024, 600)
        Me.Name = "Actions_MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Action editor"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ActionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton5 As System.Windows.Forms.ToolStripButton
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents BackgroundProcessesButton As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents MenuDesc As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents DockPanel1 As WeifenLuo.WinFormsUI.Docking.DockPanel
    Friend WithEvents VS2012BlueTheme1 As WeifenLuo.WinFormsUI.Docking.VS2012BlueTheme
    Friend WithEvents VS2012DarkTheme1 As WeifenLuo.WinFormsUI.Docking.VS2012DarkTheme
    Friend WithEvents VS2012LightTheme1 As WeifenLuo.WinFormsUI.Docking.VS2012LightTheme
End Class
