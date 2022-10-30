Imports System.Windows.Forms
Imports System.IO

Public Class ImgUMount

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If RadioButton1.Checked = True Then
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
            ProgressPanel.MountDir = MainForm.MountDir
        Else
            ProgressPanel.UMountLocalDir = False
            ' Determine if given mount dir exists
            If Directory.Exists(TextBox1.Text) Then
                ' Detect whether the mount dir has an image mounted (I don't believe on what users claim, just to be sure)
                If Directory.Exists(TextBox1.Text & "\Windows") Then
                    ProgressPanel.RandomMountDir = TextBox1.Text    ' Assume it's valid
                Else
                    Do Until Directory.Exists(TextBox1.Text) And Directory.Exists(TextBox1.Text & "\Windows")
                        If Not Directory.Exists(TextBox1.Text) Or Not Directory.Exists(TextBox1.Text & "\Windows") Then
                            Button1.PerformClick()
                        End If
                    Loop
                    ProgressPanel.RandomMountDir = TextBox1.Text    ' Assume it's valid
                End If
            Else
                Do Until Directory.Exists(TextBox1.Text) And Directory.Exists(TextBox1.Text & "\Windows")
                    If Not Directory.Exists(TextBox1.Text) Or Not Directory.Exists(TextBox1.Text & "\Windows") Then
                        Button1.PerformClick()
                    End If
                Loop
                ProgressPanel.RandomMountDir = TextBox1.Text
            End If
        End If
        If ComboBox1.SelectedIndex = 0 Then
            ProgressPanel.UMountOp = 0
        ElseIf ComboBox1.SelectedIndex = 1 Then
            ProgressPanel.UMountOp = 1
        End If
        If CheckBox1.Checked Then
            ProgressPanel.CheckImgIntegrity = True
        Else
            ProgressPanel.CheckImgIntegrity = False
        End If
        If CheckBox2.Checked Then
            ProgressPanel.SaveToNewIndex = True
        Else
            ProgressPanel.SaveToNewIndex = False
        End If
        ProgressPanel.UMountImgIndex = MainForm.ImgIndex
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 21
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgUMount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label4.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
        Else
            Label4.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox1.Text = ""
        End If
    End Sub
End Class
