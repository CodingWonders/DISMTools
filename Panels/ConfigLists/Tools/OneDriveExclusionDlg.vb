Imports System.Windows.Forms
Imports System.IO

Public Class OneDriveExclusionDlg

    Public ExcludedFolders As New List(Of String)
    Dim successfulExclusion As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ExcludeFolders(TextBox1.Text)
        If Not successfulExclusion Then Exit Sub
        Label3.Text = "User OneDrive folders have been excluded and will be added to the configuration list."
        Refresh()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Sub ExcludeFolders(ImagePath As String)
        If Directory.Exists(ImagePath & "\Users") Then
            Try
                Label3.Text = "Excluding user OneDrive folders..."
                Refresh()
                ' Go through all User folders and exclude all OneDrive folders
                For Each UserDir In Directory.GetDirectories(ImagePath & "\Users", "*", SearchOption.TopDirectoryOnly)
                    If Directory.Exists(UserDir & "\OneDrive") Then
                        Dim excludedPath As String = ""
                        excludedPath = UserDir.Replace(ImagePath & "\", "\").Trim() & "\OneDrive"
                        ExcludedFolders.Add(excludedPath)
                    End If
                Next
                successfulExclusion = True
            Catch ex As Exception
                successfulExclusion = False
            End Try
        Else
            successfulExclusion = False
        End If
    End Sub

    Private Sub OneDriveExclusionDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        ExcludedFolders.Clear()
        successfulExclusion = False
        Label3.Text = "When you're ready, click Exclude."
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And Directory.Exists(TextBox1.Text) Then OK_Button.Enabled = True Else OK_Button.Enabled = False
    End Sub
End Class
