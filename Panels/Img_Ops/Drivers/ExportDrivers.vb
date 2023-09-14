Imports System.Windows.Forms
Imports System.IO

Public Class ExportDrivers

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextBox1.Text <> "" And Directory.Exists(TextBox1.Text) Then
            ProgressPanel.drvExportTarget = TextBox1.Text
        Else
            ' Add error message later
        End If
        ProgressPanel.OperationNum = 77
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ExportDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
End Class
