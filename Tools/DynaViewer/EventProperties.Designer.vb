<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EventProperties
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtEventTimestamp = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtEventParentCaller = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtEventCaller = New System.Windows.Forms.TextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.txtEventMessage = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.btnNextEvent = New System.Windows.Forms.Button
        Me.btnPreviousEvent = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoEllipsis = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(413, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("Num.Events.Label")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("EventTimestamp.Label")
        '
        'txtEventTimestamp
        '
        Me.txtEventTimestamp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEventTimestamp.Location = New System.Drawing.Point(106, 6)
        Me.txtEventTimestamp.Name = "txtEventTimestamp"
        Me.txtEventTimestamp.ReadOnly = True
        Me.txtEventTimestamp.Size = New System.Drawing.Size(300, 21)
        Me.txtEventTimestamp.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.LinkLabel1)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.txtEventParentCaller)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtEventCaller)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 33)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(396, 136)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("MethodCallers.Group")
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(15, 112)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(177, 13)
        Me.LinkLabel1.TabIndex = 4
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("Field.Empty.Link")
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 68)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(342, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("Method.Function.Label")
        '
        'txtEventParentCaller
        '
        Me.txtEventParentCaller.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEventParentCaller.Location = New System.Drawing.Point(15, 84)
        Me.txtEventParentCaller.Name = "txtEventParentCaller"
        Me.txtEventParentCaller.ReadOnly = True
        Me.txtEventParentCaller.Size = New System.Drawing.Size(364, 21)
        Me.txtEventParentCaller.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(214, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("Logged.Method.Function.Label")
        '
        'Label6
        '
        Me.Label6.AutoEllipsis = True
        Me.Label6.Location = New System.Drawing.Point(283, 43)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("PID.Label")
        '
        'txtEventCaller
        '
        Me.txtEventCaller.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEventCaller.Location = New System.Drawing.Point(15, 40)
        Me.txtEventCaller.Name = "txtEventCaller"
        Me.txtEventCaller.ReadOnly = True
        Me.txtEventCaller.Size = New System.Drawing.Size(262, 21)
        Me.txtEventCaller.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.txtEventTimestamp)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Location = New System.Drawing.Point(12, 41)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(414, 270)
        Me.Panel1.TabIndex = 5
        '
        'txtEventMessage
        '
        Me.txtEventMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtEventMessage.Location = New System.Drawing.Point(0, 0)
        Me.txtEventMessage.Multiline = True
        Me.txtEventMessage.Name = "txtEventMessage"
        Me.txtEventMessage.ReadOnly = True
        Me.txtEventMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtEventMessage.Size = New System.Drawing.Size(397, 75)
        Me.txtEventMessage.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 175)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(84, 13)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("EventMessage.Label")
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnNextEvent, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnPreviousEvent, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 317)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(414, 29)
        Me.TableLayoutPanel1.TabIndex = 6
        '
        'btnNextEvent
        '
        Me.btnNextEvent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnNextEvent.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnNextEvent.Location = New System.Drawing.Point(210, 3)
        Me.btnNextEvent.Name = "btnNextEvent"
        Me.btnNextEvent.Size = New System.Drawing.Size(201, 23)
        Me.btnNextEvent.TabIndex = 1
        Me.btnNextEvent.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("NextEvent.Label")
        Me.btnNextEvent.UseVisualStyleBackColor = True
        '
        'btnPreviousEvent
        '
        Me.btnPreviousEvent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnPreviousEvent.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnPreviousEvent.Location = New System.Drawing.Point(3, 3)
        Me.btnPreviousEvent.Name = "btnPreviousEvent"
        Me.btnPreviousEvent.Size = New System.Drawing.Size(201, 23)
        Me.btnPreviousEvent.TabIndex = 0
        Me.btnPreviousEvent.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("PreviousEvent.Label")
        Me.btnPreviousEvent.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.txtEventMessage)
        Me.Panel2.Location = New System.Drawing.Point(9, 192)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(397, 75)
        Me.Panel2.TabIndex = 7
        '
        'EventProperties
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(438, 352)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EventProperties"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("EventProps.Label")
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtEventTimestamp As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtEventParentCaller As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtEventCaller As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtEventMessage As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnPreviousEvent As System.Windows.Forms.Button
    Friend WithEvents btnNextEvent As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel

End Class
