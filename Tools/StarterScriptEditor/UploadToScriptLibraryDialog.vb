#If VBC_VER >= 10.0 Then
Imports System.Threading.Tasks
#End If
Imports System.Windows.Forms
Imports StarterScriptEditor.Classes.ColorUtilities
Imports StarterScriptEditor.Classes
Imports System.Security.Cryptography
Imports System.IO

Public Class UploadToScriptLibraryDialog

    Private CurrentColorMode As ColorThemeMode

    Private Const RepoOwner As String = "CodingWonders", _
                  RepoName As String = "StarterScriptLibrary"

    ' Security Variables
    Private Key_PWD As String = ""
    Private ReadOnly ApiKeyPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "APIKEY")

    Private GhApiKey As String = ""

    Private cryptoHelper As New CryptographyHelper()

    Private IsUploading As Boolean

    Public StarterScriptToUpload As StarterScript
#If VBC_VER >= 10.0 Then        ' VS2010 introduced async/await
    Private Async Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ValidateGitHubApiKey(TextBox1.Text) Then
            MessageBox.Show(LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("InvalidApiKey.Message"), LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("ApiKey.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If IsUploading Then
            MessageBox.Show(LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("UploadInProgress.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        IsUploading = True
        Enabled = False
        Cursor = Cursors.WaitCursor
        Dim GHClient As New GitHubClient(TextBox1.Text)
        Dim GHUserName As String = Await GHClient.GetAuthenticatedUserNameAsync()
        Dim IsCWS As Boolean = GHUserName = RepoOwner
        If Not IsCWS Then
            Await GHClient.ForkRepositoryAsync(RepoOwner, RepoName)
            Await Task.Delay(5000)
        End If

        Dim Sha As String = Await GHClient.GetBranchShaAsync(GHUserName, RepoName, "main")
        Dim TargetBranch As String = String.Format("sse-scriptlib-{0}-{1}-{2}", GHUserName, Date.Now.ToString("yyyy_MM_dd"), New Random().Next(Integer.MaxValue))

        Await GHClient.CreateBranchAsync(GHUserName, RepoName, TargetBranch, Sha)
        ' If we upload the starter script, it's going to be an Infinity script no matter what.
        Await GHClient.CreateOrUpdateFileAsync(GHUserName, RepoName, String.Format("sse_dtss_{0}.dtss", Guid.NewGuid().ToString()), ParseStarterScript(), String.Format("[DTSS] {0}", StarterScriptToUpload.Name), TargetBranch)
        Dim PullRequestUrl As String = Await GHClient.CreatePullRequestAsync(RepoOwner, RepoName, String.Format("Starter Script Addition: {0}{1}{0}", ControlChars.Quote, StarterScriptToUpload.Name),
                                                                             String.Format("{0}{1}", If(Not IsCWS, String.Format("{0}:", GHUserName), ""), TargetBranch),
                                                                             "main", "This is my contribution to the library.")
        IsUploading = False
        Enabled = True
        Cursor = Cursors.Arrow
        If PullRequestUrl <> "" Then
            Process.Start(PullRequestUrl)
            MessageBox.Show(PullRequestUrl, Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        If CheckBox3.Checked Then EncryptApiKey()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
#Else
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ValidateGitHubApiKey(TextBox1.Text) Then
            MessageBox.Show(LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("InvalidApiKey.Message"), LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("ApiKey.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        MessageBox.Show(LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("UnsupportedVersion.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        If CheckBox3.Checked Then EncryptApiKey()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
#End If

    Private Sub EncryptApiKey()
        If Key_PWD = "" Then Key_PWD = InputBox(LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("EnterPassword.Message"),
                                                LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("EnterPassword.Title"))
        If Key_PWD = "" Then
            MessageBox.Show(LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("PasswordRequired.Message"), LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("PasswordRequired.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        EncryptionBW.RunWorkerAsync()
        ShowCryptoProgressMessage(True)
    End Sub

    Private Sub DecryptApiKey()
        If Key_PWD = "" Then Key_PWD = InputBox(LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("DecryptPassword.Message"), LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("DecryptPassword.Title"))
        If Key_PWD <> "" Then
            DecryptionBW.RunWorkerAsync()
            ShowCryptoProgressMessage(False)
        End If
    End Sub

    Private Sub ShowCryptoProgressMessage(ByVal Encrypted As Boolean)
        If Encrypted Then
            CryptographicProgressDialog.ProgressLabel.Text = LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("SavingApiKey.Message")
        Else
            CryptographicProgressDialog.ProgressLabel.Text = LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("LoadingApiKey.Message")
        End If
        CryptographicProgressDialog.ProgressLabel.Text &= LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("CryptoDuration.Suffix")
        CryptographicProgressDialog.ShowDialog(Me)
    End Sub

    Private Function ValidateGitHubApiKey(ByVal ApiKey As String) As Boolean
        If String.IsNullOrEmpty(ApiKey) Then Return False
        Return ApiKey.StartsWith("ghp_") OrElse ApiKey.StartsWith("github_pat_")
    End Function

    Private Function ParseStarterScript() As String
        Return String.Format("Language: {0}{1}" & _
            "Name: {2}{1}" & _
            "Description: {3}{1}" & _
            "Customizable: {4}{1}" & _
            "{5}", StarterScriptToUpload.Language, Environment.NewLine, StarterScriptToUpload.Name, _
                   StarterScriptToUpload.Description, IIf(StarterScriptToUpload.OptionsCustomizable, "Yes", "No"), StarterScriptToUpload.Code)
    End Function

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub UploadToScriptLibraryDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
#If DEBUG Then

#Else
        If Environment.OSVersion.Version.Major < 10 Then
            MessageBox.Show(LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("UnsupportedOs.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Exit Sub
        End If
#End If
        CurrentColorMode = MainForm.CurrentColorMode
        SetColorMode()
        If File.Exists(ApiKeyPath) Then DecryptApiKey()
    End Sub

    Private Sub SetColorMode()
        Select Case CurrentColorMode
            Case ColorThemeMode.Light
                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                ForeColor = Color.White
        End Select

        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://github.com/signup")
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        ApiKeyGenerationStepsWizard.ShowDialog(Me)
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged, CheckBox2.CheckedChanged
        CheckBox2.Enabled = CheckBox1.Checked
        OK_Button.Enabled = CheckBox1.Checked AndAlso CheckBox2.Checked
    End Sub

    Private Sub PreventLeaks_InfoBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreventLeaks_InfoBtn.Click
        MessageBox.Show(LocalizationService.ForSection("StarterScript.UploadLibrary.Messages")("PreventLeaks.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub PreventLeaks_InspectBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreventLeaks_InspectBtn.Click
        DialogResult = DialogResult.Cancel
        Close()
        MainForm.ToolStripButton10.PerformClick()
        AIResults.CheckBox1.Checked = True
    End Sub

    Private Sub UploadToScriptLibraryDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If IsUploading Then
            e.Cancel = True
            Exit Sub
        End If
        TextBox1.Text = ""
    End Sub

    Private Sub EncryptionBW_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles EncryptionBW.DoWork
        Dim Salt(15) As Byte
        Dim rng As New RNGCryptoServiceProvider()
        rng.GetBytes(Salt)

        Dim pbkdf2 As New Rfc2898DeriveBytes(Key_PWD, Salt, 500000)
        Dim Key As Byte() = pbkdf2.GetBytes(32)

        cryptoHelper.EncryptStringToFile(GhApiKey, ApiKeyPath, Key, Salt)
    End Sub

    Private Sub DecryptionBW_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles DecryptionBW.DoWork
        GhApiKey = cryptoHelper.DecryptStringFromFile(ApiKeyPath, Key_PWD)
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        GhApiKey = TextBox1.Text
    End Sub

    Private Sub EncryptionBW_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles EncryptionBW.RunWorkerCompleted
        CryptographicProgressDialog.Close()
    End Sub

    Private Sub DecryptionBW_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles DecryptionBW.RunWorkerCompleted
        CryptographicProgressDialog.Close()
        TextBox1.Text = GhApiKey

        ' If the API key is not valid then we forget the provided password
        If Not ValidateGitHubApiKey(GhApiKey) Then Key_PWD = ""
    End Sub
End Class
