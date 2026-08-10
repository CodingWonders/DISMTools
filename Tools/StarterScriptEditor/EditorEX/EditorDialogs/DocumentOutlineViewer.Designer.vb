<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DocumentOutlineViewer
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.comboLangMode = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.comboFunctionList = New System.Windows.Forms.ComboBox
        Me.btnNavigate = New System.Windows.Forms.Button
        Me.gbSignatureDetails = New System.Windows.Forms.GroupBox
        Me.lvFunctionParameters = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.lblLine = New System.Windows.Forms.Label
        Me.cbPin = New System.Windows.Forms.CheckBox
        Me.gbSignatureDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Language Mode:"
        '
        'comboLangMode
        '
        Me.comboLangMode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboLangMode.FormattingEnabled = True
        Me.comboLangMode.Items.AddRange(New Object() {"Batch", "PowerShell", "VBScript", "JScript"})
        Me.comboLangMode.Location = New System.Drawing.Point(153, 9)
        Me.comboLangMode.Name = "comboLangMode"
        Me.comboLangMode.Size = New System.Drawing.Size(331, 21)
        Me.comboLangMode.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(135, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Navigate to function/label:"
        '
        'comboFunctionList
        '
        Me.comboFunctionList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboFunctionList.FormattingEnabled = True
        Me.comboFunctionList.Location = New System.Drawing.Point(153, 37)
        Me.comboFunctionList.Name = "comboFunctionList"
        Me.comboFunctionList.Size = New System.Drawing.Size(331, 21)
        Me.comboFunctionList.TabIndex = 3
        '
        'btnNavigate
        '
        Me.btnNavigate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNavigate.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnNavigate.Location = New System.Drawing.Point(409, 65)
        Me.btnNavigate.Name = "btnNavigate"
        Me.btnNavigate.Size = New System.Drawing.Size(75, 23)
        Me.btnNavigate.TabIndex = 4
        Me.btnNavigate.Text = "Navigate"
        Me.btnNavigate.UseVisualStyleBackColor = True
        '
        'gbSignatureDetails
        '
        Me.gbSignatureDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSignatureDetails.Controls.Add(Me.lvFunctionParameters)
        Me.gbSignatureDetails.Location = New System.Drawing.Point(13, 94)
        Me.gbSignatureDetails.Name = "gbSignatureDetails"
        Me.gbSignatureDetails.Size = New System.Drawing.Size(471, 158)
        Me.gbSignatureDetails.TabIndex = 5
        Me.gbSignatureDetails.TabStop = False
        Me.gbSignatureDetails.Text = "Function Signature (VBScript only)"
        '
        'lvFunctionParameters
        '
        Me.lvFunctionParameters.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.lvFunctionParameters.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvFunctionParameters.FullRowSelect = True
        Me.lvFunctionParameters.Location = New System.Drawing.Point(3, 17)
        Me.lvFunctionParameters.Name = "lvFunctionParameters"
        Me.lvFunctionParameters.Size = New System.Drawing.Size(465, 138)
        Me.lvFunctionParameters.TabIndex = 0
        Me.lvFunctionParameters.UseCompatibleStateImageBehavior = False
        Me.lvFunctionParameters.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Parameter Name"
        Me.ColumnHeader1.Width = 160
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Parameter Passed"
        Me.ColumnHeader2.Width = 128
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Requirement"
        Me.ColumnHeader3.Width = 96
        '
        'lblLine
        '
        Me.lblLine.AutoSize = True
        Me.lblLine.Location = New System.Drawing.Point(13, 70)
        Me.lblLine.Name = "lblLine"
        Me.lblLine.Size = New System.Drawing.Size(61, 13)
        Me.lblLine.TabIndex = 6
        Me.lblLine.Text = "Line <line>"
        Me.lblLine.Visible = False
        '
        'cbPin
        '
        Me.cbPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbPin.Appearance = System.Windows.Forms.Appearance.Button
        Me.cbPin.Checked = True
        Me.cbPin.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbPin.Image = Global.StarterScriptEditor.My.Resources.Resources.pin
        Me.cbPin.ImageAlign = System.Drawing.ContentAlignment.BottomRight
        Me.cbPin.Location = New System.Drawing.Point(379, 64)
        Me.cbPin.Name = "cbPin"
        Me.cbPin.Size = New System.Drawing.Size(24, 24)
        Me.cbPin.TabIndex = 7
        Me.cbPin.UseVisualStyleBackColor = True
        '
        'DocumentOutlineViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(496, 264)
        Me.Controls.Add(Me.cbPin)
        Me.Controls.Add(Me.lblLine)
        Me.Controls.Add(Me.gbSignatureDetails)
        Me.Controls.Add(Me.btnNavigate)
        Me.Controls.Add(Me.comboFunctionList)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.comboLangMode)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(512, 300)
        Me.Name = "DocumentOutlineViewer"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Navigate Document Outline"
        Me.gbSignatureDetails.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents comboLangMode As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents comboFunctionList As System.Windows.Forms.ComboBox
    Friend WithEvents btnNavigate As System.Windows.Forms.Button
    Friend WithEvents gbSignatureDetails As System.Windows.Forms.GroupBox
    Friend WithEvents lvFunctionParameters As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblLine As System.Windows.Forms.Label
    Friend WithEvents cbPin As System.Windows.Forms.CheckBox
End Class
