Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class OneDriveExclusionDlg

    Public ExcludedFolders As New List(Of String)
    Dim successfulExclusion As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Preparing to exclude user OneDrive/SkyDrive folders...")
        ExcludeFolders(TextBox1.Text)
        If Not successfulExclusion Then Exit Sub
        Label3.Text = LocalizationService.ForSection("OneDriveExclusion.Valid")("User.Folders.Label")
        Refresh()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Sub ExcludeFolders(ImagePath As String)
        DynaLog.LogMessage("Excluding user OneDrive/SkyDrive folders...")
        DynaLog.LogMessage("Image Path: " & ImagePath)
        If ImagePath = "" Or Not Directory.Exists(ImagePath) Then
            DynaLog.LogMessage("Image Path is nothing or doesn't exist. Exiting...")
            successfulExclusion = False
            Exit Sub
        End If
        If Directory.Exists(ImagePath & "\Users") Then
            DynaLog.LogMessage("A users folder exists in Image Path. Scanning for OneDrive/SkyDrive folder...")
            Try
                Label3.Text = LocalizationService.ForSection("OneDriveExclusion.Folders")("Excluding.User.Label")
                Refresh()
                ' Go through all User folders and exclude all OneDrive folders
                For Each UserDir In Directory.GetDirectories(ImagePath & "\Users", "*", SearchOption.TopDirectoryOnly)
                    If Directory.Exists(UserDir & "\OneDrive") Then
                        DynaLog.LogMessage("A user OneDrive folder exists. Adding to DISM Configuration List File...")
                        Dim excludedPath As String = ""
                        excludedPath = UserDir.Replace(ImagePath & "\", "\").Trim() & "\OneDrive"
                        ExcludedFolders.Add(excludedPath)
                    ElseIf Directory.Exists(UserDir & "\SkyDrive") Then
                        DynaLog.LogMessage("A user SkyDrive folder exists. Adding to DISM Configuration List File...")
                        Dim excludedPath As String = ""
                        excludedPath = UserDir.Replace(ImagePath & "\", "\").Trim() & "\SkyDrive"
                        ExcludedFolders.Add(excludedPath)
                    End If
                Next
                successfulExclusion = True
            Catch ex As Exception
                DynaLog.LogMessage("Could not exclude user OneDrive/SkyDrive folders. Error message: " & ex.Message)
                successfulExclusion = False
            End Try
        Else
            successfulExclusion = False
        End If
    End Sub

    Private Sub OneDriveExclusionDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("OneDriveExclusion")("Exclude.User.Label")
        Label1.Text = LocalizationService.ForSection("OneDriveExclusion")("Tool.Help.Exclude.Message")
        Label2.Text = LocalizationService.ForSection("OneDriveExclusion")("Path.Exclude.Label")
        Label3.Text = LocalizationService.ForSection("OneDriveExclusion")("Re.Ready.Label")
        Button1.Text = LocalizationService.ForSection("OneDriveExclusion")("Browse.Button")
        OK_Button.Text = LocalizationService.ForSection("OneDriveExclusion")("Exclude.Button")
        Cancel_Button.Text = LocalizationService.ForSection("OneDriveExclusion")("Cancel.Button")
        FolderBrowserDialog1.Description = LocalizationService.ForSection("OneDriveExclusion")("UserFolderPath.Description")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = CurrentTheme.ForegroundColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ExcludedFolders.Clear()
        successfulExclusion = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And Directory.Exists(TextBox1.Text) Then OK_Button.Enabled = True Else OK_Button.Enabled = False
    End Sub
End Class
