Imports System.Windows.Forms
Imports DISMTools.Elements

Public Class SetImageEdition

    Public TargetEditions As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.imgEditionNewEdition = ComboBox1.SelectedItem
        If MainForm.imgInstType.Equals("Server", StringComparison.OrdinalIgnoreCase) AndAlso MainForm.OnlineManagement Then
            ProgressPanel.imgEditionCopyEula = RadioButton1.Checked
            ProgressPanel.imgEditionAcceptEula = RadioButton2.Checked
            ' TODO: Improve error handling
            ProgressPanel.imgEditionEulaDestination = TextBox1.Text
            Dim productKey As ProductKey = ProductKeyValidator.ValidateProductKey(TextBox2.Text)
            If Not productKey.Valid Then
                MsgBox("The product key has been typed incorrectly", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
                Exit Sub
            End If
            ProgressPanel.imgEditionEditionKey = productKey.Key
        Else
            ProgressPanel.imgEditionCopyEula = False
            ProgressPanel.imgEditionAcceptEula = False
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 71
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SetImageEdition_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If TargetEditions.Count < 1 Then
            Close()
        Else
            ComboBox1.Items.Clear()
            ComboBox1.Items.AddRange(TargetEditions.ToArray())
            If ComboBox1.Items.Count > 0 Then
                ComboBox1.SelectedIndex = 0
            End If
        End If

        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            ImageTaskHeader1.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            ImageTaskHeader1.ItemColor = ImageTaskHeader.ColorMode.Dark
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            ImageTaskHeader1.ItemColor = ImageTaskHeader.ColorMode.Light
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        ComboBox1.BackColor = BackColor
        TextBox1.BackColor = BackColor
        TextBox2.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(Handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        DynaLog.LogMessage("Determining EULA option compatibility...")
        DynaLog.LogMessage("- Image Installation Type: " & MainForm.imgPType)
        DynaLog.LogMessage("- Managing Active Installation? " & If(MainForm.OnlineManagement, "Yes", "No"))
        ' Disable group box if not managing an active server installation
        If MainForm.imgInstType.Equals("Server", StringComparison.OrdinalIgnoreCase) AndAlso MainForm.OnlineManagement Then
            DynaLog.LogMessage("All requirements are met. We are managing a Windows Server installation")
            GroupBox1.Enabled = True
        Else
            DynaLog.LogMessage("Either one or none of the two requirements described above is met. The image we are managing is not an active installation, or a Windows Server installation")
            GroupBox1.Enabled = False
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        EulaPanel.Enabled = RadioButton1.Checked
        TextBox2.Enabled = RadioButton2.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Selected path: " & FolderBrowserDialog1.SelectedPath)
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
End Class
