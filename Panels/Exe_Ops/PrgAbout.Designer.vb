<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrgAbout
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.LinkLabel8 = New System.Windows.Forms.LinkLabel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LinkLabel4 = New System.Windows.Forms.LinkLabel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.LinkLabel5 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel6 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel9 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel10 = New System.Windows.Forms.LinkLabel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.LinkLabel7 = New System.Windows.Forms.LinkLabel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.PreviewPanel = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.PreviewPanel.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(699, 340)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(73, 29)
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
        Me.OK_Button.Text = "OK"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DISMTools.My.Resources.Resources.logo_aboutdlg_light
        Me.PictureBox1.Location = New System.Drawing.Point(26, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(377, 72)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 101)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(121, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "DISMTools - version {0}"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(23, 144)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(377, 41)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "DISMTools lets you deploy, manage, and service Windows images with ease, thanks t" & _
    "o a GUI."
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.Location = New System.Drawing.Point(410, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(362, 322)
        Me.TabControl1.TabIndex = 6
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TableLayoutPanel2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(354, 294)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Credits"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.AutoScroll = True
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel2.Controls.Add(Me.LinkLabel8, 0, 12)
        Me.TableLayoutPanel2.Controls.Add(Me.Label9, 0, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.Label3, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label4, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label5, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.LinkLabel4, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label6, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.Label7, 0, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.Label8, 0, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.LinkLabel5, 1, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.LinkLabel6, 1, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.LinkLabel9, 1, 7)
        Me.TableLayoutPanel2.Controls.Add(Me.LinkLabel10, 1, 8)
        Me.TableLayoutPanel2.Controls.Add(Me.Label10, 0, 9)
        Me.TableLayoutPanel2.Controls.Add(Me.Label13, 0, 7)
        Me.TableLayoutPanel2.Controls.Add(Me.Label16, 0, 8)
        Me.TableLayoutPanel2.Controls.Add(Me.Label11, 0, 10)
        Me.TableLayoutPanel2.Controls.Add(Me.LinkLabel7, 1, 9)
        Me.TableLayoutPanel2.Controls.Add(Me.Label12, 0, 11)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 13
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(348, 288)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'LinkLabel8
        '
        Me.LinkLabel8.AutoSize = True
        Me.TableLayoutPanel2.SetColumnSpan(Me.LinkLabel8, 2)
        Me.LinkLabel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LinkLabel8.LinkArea = New System.Windows.Forms.LinkArea(59, 29)
        Me.LinkLabel8.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel8.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel8.Location = New System.Drawing.Point(3, 231)
        Me.LinkLabel8.Name = "LinkLabel8"
        Me.LinkLabel8.Size = New System.Drawing.Size(342, 57)
        Me.LinkLabel8.TabIndex = 7
        Me.LinkLabel8.TabStop = True
        Me.LinkLabel8.Text = "The Unattended answer file creation wizard is based on the Windows Answer File Ge" & _
    "nerator website"
        Me.LinkLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.LinkLabel8.UseCompatibleTextRendering = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 129)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(191, 15)
        Me.Label9.TabIndex = 5
        Me.Label9.Text = "wimlib-imagex (used in the future)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.TableLayoutPanel2.SetColumnSpan(Me.Label3, 2)
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(337, 42)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "These resources and components were used in the creation of this program:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.TableLayoutPanel2.SetColumnSpan(Me.Label4, 2)
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(3, 42)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 21)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Resources"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 63)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 15)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Fluency"
        '
        'LinkLabel4
        '
        Me.LinkLabel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LinkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel4.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel4.Location = New System.Drawing.Point(273, 63)
        Me.LinkLabel4.Name = "LinkLabel4"
        Me.TableLayoutPanel2.SetRowSpan(Me.LinkLabel4, 2)
        Me.LinkLabel4.Size = New System.Drawing.Size(72, 30)
        Me.LinkLabel4.TabIndex = 3
        Me.LinkLabel4.TabStop = True
        Me.LinkLabel4.Text = "Icons8"
        Me.LinkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 78)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(129, 15)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "SQL Server icon (Color)"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.TableLayoutPanel2.SetColumnSpan(Me.Label7, 2)
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(3, 93)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(70, 21)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Utilities"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(3, 114)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 15)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "7-Zip"
        '
        'LinkLabel5
        '
        Me.LinkLabel5.AutoSize = True
        Me.LinkLabel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LinkLabel5.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel5.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel5.Location = New System.Drawing.Point(273, 114)
        Me.LinkLabel5.Name = "LinkLabel5"
        Me.LinkLabel5.Size = New System.Drawing.Size(72, 15)
        Me.LinkLabel5.TabIndex = 4
        Me.LinkLabel5.TabStop = True
        Me.LinkLabel5.Text = "Visit website"
        Me.LinkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LinkLabel6
        '
        Me.LinkLabel6.AutoSize = True
        Me.LinkLabel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LinkLabel6.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel6.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel6.Location = New System.Drawing.Point(273, 129)
        Me.LinkLabel6.Name = "LinkLabel6"
        Me.LinkLabel6.Size = New System.Drawing.Size(72, 15)
        Me.LinkLabel6.TabIndex = 4
        Me.LinkLabel6.TabStop = True
        Me.LinkLabel6.Text = "Visit website"
        Me.LinkLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LinkLabel9
        '
        Me.LinkLabel9.AutoSize = True
        Me.LinkLabel9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LinkLabel9.LinkArea = New System.Windows.Forms.LinkArea(0, 13)
        Me.LinkLabel9.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel9.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel9.Location = New System.Drawing.Point(273, 144)
        Me.LinkLabel9.Name = "LinkLabel9"
        Me.LinkLabel9.Size = New System.Drawing.Size(72, 15)
        Me.LinkLabel9.TabIndex = 8
        Me.LinkLabel9.TabStop = True
        Me.LinkLabel9.Text = "Visit website"
        Me.LinkLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LinkLabel10
        '
        Me.LinkLabel10.AutoSize = True
        Me.LinkLabel10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LinkLabel10.LinkArea = New System.Windows.Forms.LinkArea(0, 13)
        Me.LinkLabel10.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel10.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel10.Location = New System.Drawing.Point(273, 159)
        Me.LinkLabel10.Name = "LinkLabel10"
        Me.LinkLabel10.Size = New System.Drawing.Size(72, 15)
        Me.LinkLabel10.TabIndex = 8
        Me.LinkLabel10.TabStop = True
        Me.LinkLabel10.Text = "Visit website"
        Me.LinkLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(3, 174)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(168, 21)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Help documentation"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(3, 144)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(161, 15)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "Scintila.NET (NuGet package)"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(3, 159)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(176, 15)
        Me.Label16.TabIndex = 5
        Me.Label16.Text = "ManagedDism (NuGet package)"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(3, 195)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(130, 15)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Command Help source"
        '
        'LinkLabel7
        '
        Me.LinkLabel7.AutoSize = True
        Me.LinkLabel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LinkLabel7.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel7.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel7.Location = New System.Drawing.Point(273, 174)
        Me.LinkLabel7.Name = "LinkLabel7"
        Me.LinkLabel7.Size = New System.Drawing.Size(72, 21)
        Me.LinkLabel7.TabIndex = 4
        Me.LinkLabel7.TabStop = True
        Me.LinkLabel7.Text = "Microsoft"
        Me.LinkLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.TableLayoutPanel2.SetColumnSpan(Me.Label12, 2)
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(3, 210)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(310, 21)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Unattended answer file creation wizard"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.RichTextBox1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(354, 294)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "License"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'RichTextBox1
        '
        Me.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox1.Location = New System.Drawing.Point(3, 3)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ReadOnly = True
        Me.RichTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.RichTextBox1.Size = New System.Drawing.Size(348, 288)
        Me.RichTextBox1.TabIndex = 0
        Me.RichTextBox1.Text = ""
        '
        'PreviewPanel
        '
        Me.PreviewPanel.Controls.Add(Me.Label14)
        Me.PreviewPanel.Controls.Add(Me.PictureBox2)
        Me.PreviewPanel.Location = New System.Drawing.Point(12, 255)
        Me.PreviewPanel.Name = "PreviewPanel"
        Me.PreviewPanel.Size = New System.Drawing.Size(388, 72)
        Me.PreviewPanel.TabIndex = 7
        Me.PreviewPanel.Visible = False
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(39, 8)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(332, 64)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "You are currently using a preview build, which may not be ready for production ye" & _
    "t. You may encounter more bugs along the way." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Use at your own risk."
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.DISMTools.My.Resources.Resources.preview_light
        Me.PictureBox2.Location = New System.Drawing.Point(8, 8)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(23, 348)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(121, 13)
        Me.Label15.TabIndex = 4
        Me.Label15.Text = "DISMTools - version {0}"
        '
        'PrgAbout
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 381)
        Me.Controls.Add(Me.PreviewPanel)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PrgAbout"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About this program"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.PreviewPanel.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel4 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents LinkLabel8 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel5 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel6 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel7 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel9 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel10 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents PreviewPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label

End Class
