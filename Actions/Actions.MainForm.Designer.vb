<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Actions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Actions))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpTopicsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ActionsReferenceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutDISMToolsActionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Scintilla1 = New ScintillaNET.Scintilla()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.ActionNotSelected = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ActionProperties = New System.Windows.Forms.Panel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton5 = New System.Windows.Forms.ToolStripButton()
        Me.VisualModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BeginTestingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditPropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.PackActionFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.ActionNotSelected.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ActionToolStripMenuItem, Me.TestingToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1264, 24)
        Me.MenuStrip1.TabIndex = 0
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
        Me.ActionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditPropertiesToolStripMenuItem, Me.ToolStripSeparator5, Me.PackActionFileToolStripMenuItem})
        Me.ActionToolStripMenuItem.Name = "ActionToolStripMenuItem"
        Me.ActionToolStripMenuItem.Size = New System.Drawing.Size(54, 20)
        Me.ActionToolStripMenuItem.Text = "Action"
        '
        'TestingToolStripMenuItem
        '
        Me.TestingToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BeginTestingToolStripMenuItem})
        Me.TestingToolStripMenuItem.Name = "TestingToolStripMenuItem"
        Me.TestingToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.TestingToolStripMenuItem.Text = "Testing"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VisualModeToolStripMenuItem, Me.ToolStripSeparator4, Me.OptionsToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpTopicsToolStripMenuItem, Me.ActionsReferenceToolStripMenuItem, Me.ToolStripSeparator1, Me.AboutDISMToolsActionsToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'HelpTopicsToolStripMenuItem
        '
        Me.HelpTopicsToolStripMenuItem.Name = "HelpTopicsToolStripMenuItem"
        Me.HelpTopicsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.HelpTopicsToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.HelpTopicsToolStripMenuItem.Text = "Help Topics"
        '
        'ActionsReferenceToolStripMenuItem
        '
        Me.ActionsReferenceToolStripMenuItem.Name = "ActionsReferenceToolStripMenuItem"
        Me.ActionsReferenceToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.ActionsReferenceToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.ActionsReferenceToolStripMenuItem.Text = "Actions Reference"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(212, 6)
        '
        'AboutDISMToolsActionsToolStripMenuItem
        '
        Me.AboutDISMToolsActionsToolStripMenuItem.Name = "AboutDISMToolsActionsToolStripMenuItem"
        Me.AboutDISMToolsActionsToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.AboutDISMToolsActionsToolStripMenuItem.Text = "About DISMTools Actions"
        '
        'Scintilla1
        '
        Me.Scintilla1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Scintilla1.IndentationGuides = ScintillaNET.IndentView.LookBoth
        Me.Scintilla1.Lexer = ScintillaNET.Lexer.Vb
        Me.Scintilla1.Location = New System.Drawing.Point(0, 0)
        Me.Scintilla1.Name = "Scintilla1"
        Me.Scintilla1.Size = New System.Drawing.Size(1004, 632)
        Me.Scintilla1.TabIndex = 3
        Me.Scintilla1.UseTabs = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 49)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Scintilla1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1264, 632)
        Me.SplitContainer1.SplitterDistance = 256
        Me.SplitContainer1.TabIndex = 5
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.TreeView1)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.ActionNotSelected)
        Me.SplitContainer2.Panel2.Controls.Add(Me.ActionProperties)
        Me.SplitContainer2.Size = New System.Drawing.Size(256, 632)
        Me.SplitContainer2.SplitterDistance = 315
        Me.SplitContainer2.TabIndex = 0
        '
        'TreeView1
        '
        Me.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView1.Location = New System.Drawing.Point(0, 0)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(256, 315)
        Me.TreeView1.TabIndex = 0
        '
        'ActionNotSelected
        '
        Me.ActionNotSelected.Controls.Add(Me.Label1)
        Me.ActionNotSelected.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ActionNotSelected.Location = New System.Drawing.Point(0, 0)
        Me.ActionNotSelected.Name = "ActionNotSelected"
        Me.ActionNotSelected.Size = New System.Drawing.Size(256, 313)
        Me.ActionNotSelected.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoEllipsis = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(256, 313)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select an action file to show its details."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ActionProperties
        '
        Me.ActionProperties.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ActionProperties.Location = New System.Drawing.Point(0, 0)
        Me.ActionProperties.Name = "ActionProperties"
        Me.ActionProperties.Size = New System.Drawing.Size(256, 313)
        Me.ActionProperties.TabIndex = 0
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2, Me.ToolStripButton3, Me.ToolStripSeparator2, Me.ToolStripButton4, Me.ToolStripSeparator3, Me.ToolStripButton5})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1264, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.DISMTools.My.Resources.Resources.newfile
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = Global.DISMTools.My.Resources.Resources.openfile
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton2.Text = "ToolStripButton2"
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton3.Image = Global.DISMTools.My.Resources.Resources.save_glyph
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton3.Text = "ToolStripButton3"
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
        Me.ToolStripButton4.ToolTipText = "Begins action file testing against the mounted image"
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
        Me.ToolStripButton5.Text = "ToolStripButton5"
        '
        'VisualModeToolStripMenuItem
        '
        Me.VisualModeToolStripMenuItem.Enabled = False
        Me.VisualModeToolStripMenuItem.Name = "VisualModeToolStripMenuItem"
        Me.VisualModeToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.VisualModeToolStripMenuItem.Text = "Visual mode"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(149, 6)
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'BeginTestingToolStripMenuItem
        '
        Me.BeginTestingToolStripMenuItem.Name = "BeginTestingToolStripMenuItem"
        Me.BeginTestingToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.BeginTestingToolStripMenuItem.Text = "Begin testing"
        '
        'EditPropertiesToolStripMenuItem
        '
        Me.EditPropertiesToolStripMenuItem.Name = "EditPropertiesToolStripMenuItem"
        Me.EditPropertiesToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.EditPropertiesToolStripMenuItem.Text = "Edit properties..."
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(160, 6)
        '
        'PackActionFileToolStripMenuItem
        '
        Me.PackActionFileToolStripMenuItem.Name = "PackActionFileToolStripMenuItem"
        Me.PackActionFileToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.PackActionFileToolStripMenuItem.Text = "Pack action file..."
        '
        'Actions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 681)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(1024, 600)
        Me.Name = "Actions"
        Me.Text = "Action editor"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.ActionNotSelected.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
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
    Friend WithEvents HelpTopicsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ActionsReferenceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutDISMToolsActionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Scintilla1 As ScintillaNET.Scintilla
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents ActionNotSelected As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ActionProperties As System.Windows.Forms.Panel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents EditPropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PackActionFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BeginTestingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VisualModeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton5 As System.Windows.Forms.ToolStripButton
End Class
