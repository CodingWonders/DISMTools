<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WDSInstallImageCopy
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WDSInstallImageCopy))
        Me.ImageTaskHeader1 = New DISMTools.ImageTaskHeader()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ProgressContainer = New System.Windows.Forms.Panel()
        Me.IdlePanel = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ISOProgressPanel = New System.Windows.Forms.Panel()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.OptionsPanel = New System.Windows.Forms.Panel()
        Me.GroupBox1.SuspendLayout()
        Me.ProgressContainer.SuspendLayout()
        Me.IdlePanel.SuspendLayout()
        Me.ISOProgressPanel.SuspendLayout()
        Me.OptionsPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageTaskHeader1
        '
        Me.ImageTaskHeader1.BackColor = System.Drawing.Color.White
        Me.ImageTaskHeader1.Dock = System.Windows.Forms.DockStyle.Top
        Me.ImageTaskHeader1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ImageTaskHeader1.ItemPicture = Global.DISMTools.My.Resources.Resources.copy_img_to_wds
        Me.ImageTaskHeader1.ItemText = "Copy an image to Windows Deployment Services"
        Me.ImageTaskHeader1.Location = New System.Drawing.Point(0, 0)
        Me.ImageTaskHeader1.MaximumSize = New System.Drawing.Size(19200, 48)
        Me.ImageTaskHeader1.MinimumSize = New System.Drawing.Size(400, 48)
        Me.ImageTaskHeader1.Name = "ImageTaskHeader1"
        Me.ImageTaskHeader1.Size = New System.Drawing.Size(1008, 48)
        Me.ImageTaskHeader1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.OK_Button.Location = New System.Drawing.Point(840, 526)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(75, 23)
        Me.OK_Button.TabIndex = 10
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Ok.Button")
        Me.OK_Button.UseVisualStyleBackColor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Cancel_Button.Location = New System.Drawing.Point(921, 526)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(75, 23)
        Me.Cancel_Button.TabIndex = 11
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Cancel.Button")
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button2.Location = New System.Drawing.Point(751, 20)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 14
        Me.Button2.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Pick.Button")
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(670, 20)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 15
        Me.Button1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Browse.Button")
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(4, 21)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(660, 21)
        Me.TextBox1.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 4)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(144, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageFile.Server.Label")
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button3.Location = New System.Drawing.Point(832, 20)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(156, 23)
        Me.Button3.TabIndex = 16
        Me.Button3.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Mounted.Image.Button")
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(238, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Images.Added.Group.Label")
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.Location = New System.Drawing.Point(4, 64)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(822, 21)
        Me.TextBox2.TabIndex = 13
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.CheckBoxes = True
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.ListView1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.Location = New System.Drawing.Point(4, 92)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(984, 242)
        Me.ListView1.TabIndex = 17
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Value.Column")
        Me.ColumnHeader1.Width = 44
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageName.Column")
        Me.ColumnHeader2.Width = 265
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageDescription.Column")
        Me.ColumnHeader3.Width = 343
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageVersion.Column")
        Me.ColumnHeader4.Width = 103
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Image.Architecture.Column")
        Me.ColumnHeader5.Width = 130
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button4.Location = New System.Drawing.Point(832, 63)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(156, 23)
        Me.Button4.TabIndex = 14
        Me.Button4.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Pick.Server.Groups.Button")
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button5.Location = New System.Drawing.Point(790, 340)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(96, 23)
        Me.Button5.TabIndex = 14
        Me.Button5.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("SelectAll.Button")
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button6.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button6.Location = New System.Drawing.Point(892, 340)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(96, 23)
        Me.Button6.TabIndex = 14
        Me.Button6.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ClearSelection.Button")
        Me.Button6.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.ProgressContainer)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 422)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(984, 98)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Progress.Group")
        '
        'ProgressContainer
        '
        Me.ProgressContainer.Controls.Add(Me.IdlePanel)
        Me.ProgressContainer.Controls.Add(Me.ISOProgressPanel)
        Me.ProgressContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProgressContainer.Location = New System.Drawing.Point(3, 17)
        Me.ProgressContainer.Name = "ProgressContainer"
        Me.ProgressContainer.Size = New System.Drawing.Size(978, 78)
        Me.ProgressContainer.TabIndex = 0
        '
        'IdlePanel
        '
        Me.IdlePanel.Controls.Add(Me.Label3)
        Me.IdlePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.IdlePanel.Location = New System.Drawing.Point(0, 0)
        Me.IdlePanel.Name = "IdlePanel"
        Me.IdlePanel.Size = New System.Drawing.Size(978, 78)
        Me.IdlePanel.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoEllipsis = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(978, 78)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Re.Ready.OK.Label")
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ISOProgressPanel
        '
        Me.ISOProgressPanel.Controls.Add(Me.ProgressBar1)
        Me.ISOProgressPanel.Controls.Add(Me.Label8)
        Me.ISOProgressPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ISOProgressPanel.Location = New System.Drawing.Point(0, 0)
        Me.ISOProgressPanel.Name = "ISOProgressPanel"
        Me.ISOProgressPanel.Size = New System.Drawing.Size(978, 78)
        Me.ISOProgressPanel.TabIndex = 0
        Me.ISOProgressPanel.Visible = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(15, 29)
        Me.ProgressBar1.MarqueeAnimationSpeed = 25
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(947, 23)
        Me.ProgressBar1.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoEllipsis = True
        Me.Label8.Location = New System.Drawing.Point(12, 12)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(950, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Status.Label")
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.WDSImageCopy")("WIM.Files.Filter")
        '
        'BackgroundWorker1
        '
        Me.BackgroundWorker1.WorkerReportsProgress = True
        Me.BackgroundWorker1.WorkerSupportsCancellation = True
        '
        'OptionsPanel
        '
        Me.OptionsPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OptionsPanel.Controls.Add(Me.ListView1)
        Me.OptionsPanel.Controls.Add(Me.Button6)
        Me.OptionsPanel.Controls.Add(Me.Button5)
        Me.OptionsPanel.Controls.Add(Me.Button4)
        Me.OptionsPanel.Controls.Add(Me.Button2)
        Me.OptionsPanel.Controls.Add(Me.Button1)
        Me.OptionsPanel.Controls.Add(Me.TextBox2)
        Me.OptionsPanel.Controls.Add(Me.TextBox1)
        Me.OptionsPanel.Controls.Add(Me.Label1)
        Me.OptionsPanel.Controls.Add(Me.Label4)
        Me.OptionsPanel.Controls.Add(Me.Button3)
        Me.OptionsPanel.Location = New System.Drawing.Point(8, 53)
        Me.OptionsPanel.Name = "OptionsPanel"
        Me.OptionsPanel.Size = New System.Drawing.Size(999, 369)
        Me.OptionsPanel.TabIndex = 19
        '
        'WDSInstallImageCopy
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(1008, 561)
        Me.Controls.Add(Me.OptionsPanel)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.ImageTaskHeader1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1024, 600)
        Me.Name = "WDSInstallImageCopy"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Image.Win.Deploy.Label")
        Me.GroupBox1.ResumeLayout(False)
        Me.ProgressContainer.ResumeLayout(False)
        Me.IdlePanel.ResumeLayout(False)
        Me.ISOProgressPanel.ResumeLayout(False)
        Me.OptionsPanel.ResumeLayout(False)
        Me.OptionsPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ImageTaskHeader1 As DISMTools.ImageTaskHeader
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ProgressContainer As System.Windows.Forms.Panel
    Friend WithEvents IdlePanel As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ISOProgressPanel As System.Windows.Forms.Panel
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents OptionsPanel As System.Windows.Forms.Panel
End Class
