<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExceptionForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExceptionForm))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ErrorText = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Issue_Btn = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Continue_Btn = New System.Windows.Forms.Button()
        Me.Exit_Btn = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoEllipsis = True
        Me.Label1.Location = New System.Drawing.Point(51, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(561, 57)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "We are sorry for the inconvenience, but DISMTools has run into an error that it c" & _
    "ouldn't handle and we need your help in order to continue." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Here is the error " & _
    "information if you need it:"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DISMTools.My.Resources.Resources.error_32px
        Me.PictureBox1.Location = New System.Drawing.Point(13, 13)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'ErrorText
        '
        Me.ErrorText.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ErrorText.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ErrorText.Location = New System.Drawing.Point(54, 73)
        Me.ErrorText.Multiline = True
        Me.ErrorText.Name = "ErrorText"
        Me.ErrorText.ReadOnly = True
        Me.ErrorText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ErrorText.Size = New System.Drawing.Size(558, 128)
        Me.ErrorText.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoEllipsis = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(51, 204)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(561, 17)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Please help us fix this issue"
        '
        'Label3
        '
        Me.Label3.AutoEllipsis = True
        Me.Label3.Location = New System.Drawing.Point(51, 221)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(561, 42)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "In order to prevent this problem from happening again, we would like to know more" & _
    " about it by reporting an issue on the GitHub repository. You will need a GitHub" & _
    " account to report feedback."
        '
        'Issue_Btn
        '
        Me.Issue_Btn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Issue_Btn.Location = New System.Drawing.Point(184, 266)
        Me.Issue_Btn.Name = "Issue_Btn"
        Me.Issue_Btn.Size = New System.Drawing.Size(256, 23)
        Me.Issue_Btn.TabIndex = 3
        Me.Issue_Btn.Text = "Report this issue"
        Me.Issue_Btn.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoEllipsis = True
        Me.Label4.Location = New System.Drawing.Point(51, 308)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(561, 65)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = resources.GetString("Label4.Text")
        '
        'Continue_Btn
        '
        Me.Continue_Btn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Continue_Btn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Continue_Btn.Location = New System.Drawing.Point(350, 406)
        Me.Continue_Btn.Name = "Continue_Btn"
        Me.Continue_Btn.Size = New System.Drawing.Size(128, 23)
        Me.Continue_Btn.TabIndex = 4
        Me.Continue_Btn.Text = "Continue"
        Me.Continue_Btn.UseVisualStyleBackColor = True
        '
        'Exit_Btn
        '
        Me.Exit_Btn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Exit_Btn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Exit_Btn.Location = New System.Drawing.Point(484, 406)
        Me.Exit_Btn.Name = "Exit_Btn"
        Me.Exit_Btn.Size = New System.Drawing.Size(128, 23)
        Me.Exit_Btn.TabIndex = 4
        Me.Exit_Btn.Text = "Exit"
        Me.Exit_Btn.UseVisualStyleBackColor = True
        '
        'ExceptionForm
        '
        Me.AcceptButton = Me.Continue_Btn
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 441)
        Me.Controls.Add(Me.Exit_Btn)
        Me.Controls.Add(Me.Continue_Btn)
        Me.Controls.Add(Me.Issue_Btn)
        Me.Controls.Add(Me.ErrorText)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExceptionForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DISMTools - Internal Error"
        Me.TopMost = True
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ErrorText As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Issue_Btn As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Continue_Btn As System.Windows.Forms.Button
    Friend WithEvents Exit_Btn As System.Windows.Forms.Button
End Class
