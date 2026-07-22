<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SampleScriptBrowser
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SampleScriptBrowser))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.ActionPanel = New System.Windows.Forms.Panel()
        Me.CreateStarterScriptBtn = New System.Windows.Forms.Button()
        Me.ScriptListPanel = New System.Windows.Forms.Panel()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ScriptStageSelectionPanel = New System.Windows.Forms.Panel()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ScriptDetailsContainerPanel = New System.Windows.Forms.Panel()
        Me.ScriptDetailsPanel = New System.Windows.Forms.Panel()
        Me.EnterFSModeBtn = New System.Windows.Forms.Button()
        Me.ExportScriptCodeBtn = New System.Windows.Forms.Button()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ScriptDetailsNoSelectedPanel = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ScriptCodeExporterSFD = New System.Windows.Forms.SaveFileDialog()
        Me.SSETimer = New System.Windows.Forms.Timer(Me.components)
        Me.ScriptCodeFSPanel = New System.Windows.Forms.Panel()
        Me.RichTextBox2 = New System.Windows.Forms.RichTextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ExitFSModeBtn = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ActionPanel.SuspendLayout()
        Me.ScriptListPanel.SuspendLayout()
        Me.ScriptStageSelectionPanel.SuspendLayout()
        Me.ScriptDetailsContainerPanel.SuspendLayout()
        Me.ScriptDetailsPanel.SuspendLayout()
        Me.ScriptDetailsNoSelectedPanel.SuspendLayout()
        Me.ScriptCodeFSPanel.SuspendLayout()
        Me.Panel1.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(851, 7)
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
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Ok.Button")
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
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Cancel.Button")
        '
        'ActionPanel
        '
        Me.ActionPanel.Controls.Add(Me.CreateStarterScriptBtn)
        Me.ActionPanel.Controls.Add(Me.TableLayoutPanel1)
        Me.ActionPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ActionPanel.Location = New System.Drawing.Point(0, 513)
        Me.ActionPanel.Name = "ActionPanel"
        Me.ActionPanel.Size = New System.Drawing.Size(1008, 48)
        Me.ActionPanel.TabIndex = 1
        '
        'CreateStarterScriptBtn
        '
        Me.CreateStarterScriptBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CreateStarterScriptBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CreateStarterScriptBtn.Location = New System.Drawing.Point(12, 10)
        Me.CreateStarterScriptBtn.Name = "CreateStarterScriptBtn"
        Me.CreateStarterScriptBtn.Size = New System.Drawing.Size(205, 23)
        Me.CreateStarterScriptBtn.TabIndex = 1
        Me.CreateStarterScriptBtn.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Create.Starter.Button")
        Me.CreateStarterScriptBtn.UseVisualStyleBackColor = True
        '
        'ScriptListPanel
        '
        Me.ScriptListPanel.Controls.Add(Me.ListView1)
        Me.ScriptListPanel.Controls.Add(Me.ScriptStageSelectionPanel)
        Me.ScriptListPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.ScriptListPanel.Location = New System.Drawing.Point(0, 0)
        Me.ScriptListPanel.Name = "ScriptListPanel"
        Me.ScriptListPanel.Size = New System.Drawing.Size(320, 513)
        Me.ScriptListPanel.TabIndex = 2
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        Me.ListView1.Location = New System.Drawing.Point(0, 72)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(320, 441)
        Me.ListView1.TabIndex = 1
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Name.Column")
        Me.ColumnHeader1.Width = 286
        '
        'ScriptStageSelectionPanel
        '
        Me.ScriptStageSelectionPanel.Controls.Add(Me.ComboBox1)
        Me.ScriptStageSelectionPanel.Controls.Add(Me.Label1)
        Me.ScriptStageSelectionPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.ScriptStageSelectionPanel.Location = New System.Drawing.Point(0, 0)
        Me.ScriptStageSelectionPanel.Name = "ScriptStageSelectionPanel"
        Me.ScriptStageSelectionPanel.Size = New System.Drawing.Size(320, 72)
        Me.ScriptStageSelectionPanel.TabIndex = 0
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {LocalizationService.ForSection("Designer.ScriptBrowser")("System.Config.Item"), LocalizationService.ForSection("Designer.ScriptBrowser")("First.User.Logs.Item"), LocalizationService.ForSection("Designer.ScriptBrowser")("Whenever.User.Logs.Item"), LocalizationService.ForSection("Designer.ScriptBrowser")("Scripts.Defined.User.Item")})
        Me.ComboBox1.Location = New System.Drawing.Point(15, 36)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(291, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(153, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Stage.Type.Choose.Label")
        '
        'ScriptDetailsContainerPanel
        '
        Me.ScriptDetailsContainerPanel.Controls.Add(Me.ScriptDetailsPanel)
        Me.ScriptDetailsContainerPanel.Controls.Add(Me.ScriptDetailsNoSelectedPanel)
        Me.ScriptDetailsContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ScriptDetailsContainerPanel.Location = New System.Drawing.Point(320, 0)
        Me.ScriptDetailsContainerPanel.Name = "ScriptDetailsContainerPanel"
        Me.ScriptDetailsContainerPanel.Size = New System.Drawing.Size(688, 513)
        Me.ScriptDetailsContainerPanel.TabIndex = 3
        '
        'ScriptDetailsPanel
        '
        Me.ScriptDetailsPanel.Controls.Add(Me.EnterFSModeBtn)
        Me.ScriptDetailsPanel.Controls.Add(Me.ExportScriptCodeBtn)
        Me.ScriptDetailsPanel.Controls.Add(Me.RichTextBox1)
        Me.ScriptDetailsPanel.Controls.Add(Me.Label7)
        Me.ScriptDetailsPanel.Controls.Add(Me.Label6)
        Me.ScriptDetailsPanel.Controls.Add(Me.Label5)
        Me.ScriptDetailsPanel.Controls.Add(Me.Label4)
        Me.ScriptDetailsPanel.Controls.Add(Me.Label3)
        Me.ScriptDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ScriptDetailsPanel.Location = New System.Drawing.Point(0, 0)
        Me.ScriptDetailsPanel.Name = "ScriptDetailsPanel"
        Me.ScriptDetailsPanel.Size = New System.Drawing.Size(688, 513)
        Me.ScriptDetailsPanel.TabIndex = 1
        Me.ScriptDetailsPanel.Visible = False
        '
        'EnterFSModeBtn
        '
        Me.EnterFSModeBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.EnterFSModeBtn.Location = New System.Drawing.Point(531, 158)
        Me.EnterFSModeBtn.Name = "EnterFSModeBtn"
        Me.EnterFSModeBtn.Size = New System.Drawing.Size(107, 23)
        Me.EnterFSModeBtn.TabIndex = 7
        Me.EnterFSModeBtn.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("EnlargePreview.Label")
        Me.EnterFSModeBtn.UseVisualStyleBackColor = True
        '
        'ExportScriptCodeBtn
        '
        Me.ExportScriptCodeBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ExportScriptCodeBtn.Location = New System.Drawing.Point(333, 158)
        Me.ExportScriptCodeBtn.Name = "ExportScriptCodeBtn"
        Me.ExportScriptCodeBtn.Size = New System.Drawing.Size(192, 23)
        Me.ExportScriptCodeBtn.TabIndex = 7
        Me.ExportScriptCodeBtn.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Export.Code.File.Button")
        Me.ExportScriptCodeBtn.UseVisualStyleBackColor = True
        '
        'RichTextBox1
        '
        Me.RichTextBox1.DetectUrls = False
        Me.RichTextBox1.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox1.Location = New System.Drawing.Point(53, 192)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ReadOnly = True
        Me.RichTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.RichTextBox1.Size = New System.Drawing.Size(585, 273)
        Me.RichTextBox1.TabIndex = 6
        Me.RichTextBox1.Text = ""
        Me.RichTextBox1.WordWrap = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(15, 482)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(399, 13)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Okinsert.Label")
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(50, 163)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("ScriptCode.Label")
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(50, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Language.Label")
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label4.AutoEllipsis = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(50, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(588, 48)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Description.Label")
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.AutoEllipsis = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(13, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(661, 44)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("ScriptName.Label")
        '
        'ScriptDetailsNoSelectedPanel
        '
        Me.ScriptDetailsNoSelectedPanel.Controls.Add(Me.Label2)
        Me.ScriptDetailsNoSelectedPanel.Controls.Add(Me.Label8)
        Me.ScriptDetailsNoSelectedPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ScriptDetailsNoSelectedPanel.Location = New System.Drawing.Point(0, 0)
        Me.ScriptDetailsNoSelectedPanel.Name = "ScriptDetailsNoSelectedPanel"
        Me.ScriptDetailsNoSelectedPanel.Size = New System.Drawing.Size(688, 513)
        Me.ScriptDetailsNoSelectedPanel.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.AutoEllipsis = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(102, 107)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(481, 96)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("View.Label")
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoEllipsis = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(16, 214)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(656, 192)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("StarterScripts.Help.Message")
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ScriptCodeExporterSFD
        '
        Me.ScriptCodeExporterSFD.Title = LocalizationService.ForSection("Designer.ScriptBrowser")("Export.Code.Title")
        '
        'SSETimer
        '
        '
        'ScriptCodeFSPanel
        '
        Me.ScriptCodeFSPanel.Controls.Add(Me.RichTextBox2)
        Me.ScriptCodeFSPanel.Controls.Add(Me.Panel1)
        Me.ScriptCodeFSPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ScriptCodeFSPanel.Location = New System.Drawing.Point(320, 0)
        Me.ScriptCodeFSPanel.Name = "ScriptCodeFSPanel"
        Me.ScriptCodeFSPanel.Size = New System.Drawing.Size(688, 513)
        Me.ScriptCodeFSPanel.TabIndex = 4
        Me.ScriptCodeFSPanel.Visible = False
        '
        'RichTextBox2
        '
        Me.RichTextBox2.DetectUrls = False
        Me.RichTextBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox2.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox2.Location = New System.Drawing.Point(0, 48)
        Me.RichTextBox2.Name = "RichTextBox2"
        Me.RichTextBox2.ReadOnly = True
        Me.RichTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.RichTextBox2.Size = New System.Drawing.Size(688, 465)
        Me.RichTextBox2.TabIndex = 7
        Me.RichTextBox2.Text = ""
        Me.RichTextBox2.WordWrap = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.ExitFSModeBtn)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(688, 48)
        Me.Panel1.TabIndex = 8
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(16, 17)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(336, 13)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Leave.Full.Screen.Label")
        '
        'ExitFSModeBtn
        '
        Me.ExitFSModeBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitFSModeBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ExitFSModeBtn.Location = New System.Drawing.Point(565, 12)
        Me.ExitFSModeBtn.Name = "ExitFSModeBtn"
        Me.ExitFSModeBtn.Size = New System.Drawing.Size(107, 23)
        Me.ExitFSModeBtn.TabIndex = 8
        Me.ExitFSModeBtn.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("GoBack.Label")
        Me.ExitFSModeBtn.UseVisualStyleBackColor = True
        '
        'SampleScriptBrowser
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(1008, 561)
        Me.Controls.Add(Me.ScriptDetailsContainerPanel)
        Me.Controls.Add(Me.ScriptCodeFSPanel)
        Me.Controls.Add(Me.ScriptListPanel)
        Me.Controls.Add(Me.ActionPanel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SampleScriptBrowser"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("LoadStarterScript.Label")
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ActionPanel.ResumeLayout(False)
        Me.ScriptListPanel.ResumeLayout(False)
        Me.ScriptStageSelectionPanel.ResumeLayout(False)
        Me.ScriptStageSelectionPanel.PerformLayout()
        Me.ScriptDetailsContainerPanel.ResumeLayout(False)
        Me.ScriptDetailsPanel.ResumeLayout(False)
        Me.ScriptDetailsPanel.PerformLayout()
        Me.ScriptDetailsNoSelectedPanel.ResumeLayout(False)
        Me.ScriptCodeFSPanel.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents ActionPanel As System.Windows.Forms.Panel
    Friend WithEvents ScriptListPanel As System.Windows.Forms.Panel
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ScriptStageSelectionPanel As System.Windows.Forms.Panel
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ScriptDetailsContainerPanel As System.Windows.Forms.Panel
    Friend WithEvents ScriptDetailsPanel As System.Windows.Forms.Panel
    Friend WithEvents ScriptDetailsNoSelectedPanel As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents CreateStarterScriptBtn As System.Windows.Forms.Button
    Friend WithEvents ExportScriptCodeBtn As System.Windows.Forms.Button
    Friend WithEvents ScriptCodeExporterSFD As System.Windows.Forms.SaveFileDialog
    Friend WithEvents SSETimer As System.Windows.Forms.Timer
    Friend WithEvents EnterFSModeBtn As System.Windows.Forms.Button
    Friend WithEvents ScriptCodeFSPanel As System.Windows.Forms.Panel
    Friend WithEvents RichTextBox2 As System.Windows.Forms.RichTextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ExitFSModeBtn As System.Windows.Forms.Button

End Class
