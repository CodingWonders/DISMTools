Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Net

Public Class PrgAbout

    Private resized As Boolean = False

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub PrgAbout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not resized Then ResizeImage()
        Text = LocalizationService.ForSection("PrgAbout")("AboutProgram.Label")
        Label1.Text = LocalizationService.ForSection("PrgAbout").Format("DISM.Tools.Version.Label", My.Application.Info.Version.ToString(), If(MainForm.dtBranch.Contains("pre"), "." & MainForm.dtBranch & "." & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm"), ""))
        Label2.Text = LocalizationService.ForSection("PrgAbout")("DISM.Tools.Lets.Label")
        Label3.Text = LocalizationService.ForSection("PrgAbout")("ResourcesUsed.Label")
        Label4.Text = LocalizationService.ForSection("PrgAbout")("Resources.Label")
        Label5.Text = LocalizationService.ForSection("PrgAbout")("Fluency.Label")
        Label6.Text = LocalizationService.ForSection("PrgAbout")("Sqlserver.Icon.Color.Label")
        Label7.Text = LocalizationService.ForSection("PrgAbout")("Utilities.Label")
        Label8.Text = LocalizationService.ForSection("PrgAbout")("Zip.Label")
        Label10.Text = LocalizationService.ForSection("PrgAbout")("Help.Documentation.Label")
        Label11.Text = LocalizationService.ForSection("PrgAbout")("Command.Help.Source.Label")
        Label13.Text = LocalizationService.ForSection("PrgAbout")("Scintilla.Netnu.Get.Label")
        If Not MainForm.dtBranch.Contains(LocalizationService.ForSection("PrgAbout")("Pre.Label")) Then
            Label15.Text = LocalizationService.ForSection("PrgAbout").Format("BuiltMsbuild.Label", RetrieveLinkerTimestamp())
            Label15.Visible = True
        End If
        Label16.Text = LocalizationService.ForSection("PrgAbout")("Managed.Dismnu.Get.Label")
        Label17.Text = LocalizationService.ForSection("PrgAbout")("BrandingAssets.Label")
        Label18.Text = LocalizationService.ForSection("PrgAbout")("Windows.Label")
        LinkLabel1.Text = LocalizationService.ForSection("PrgAbout")("Credits.Link")
        LinkLabel2.Text = LocalizationService.ForSection("PrgAbout")("Licenses.Link")
        LinkLabel3.Text = LocalizationService.ForSection("PrgAbout")("Whatsnew.Link")
        LinkLabel4.Text = LocalizationService.ForSection("PrgAbout")("Icons.Link")
        LinkLabel5.Text = LocalizationService.ForSection("PrgAbout")("VisitWebsite.Link")
        LinkLabel7.Text = LocalizationService.ForSection("PrgAbout")("Microsoft.Link")
        LinkLabel9.Text = LocalizationService.ForSection("PrgAbout")("VisitWebsite.Link")
        LinkLabel10.Text = LocalizationService.ForSection("PrgAbout")("VisitWebsite.Link")
        LinkLabel11.Text = LocalizationService.ForSection("PrgAbout")("Microsoft.Link")
        LinkLabel12.Text = LocalizationService.ForSection("PrgAbout")("VisitWebsite.Link")
        OK_Button.Text = LocalizationService.ForSection("PrgAbout")("Ok.Button")
        UpdCheckBtn.Text = LocalizationService.ForSection("PrgAbout")("CheckUpdates.Label")
        RichTextBox1.Text = LocalizationService.ForSection("PrgAbout.Resources")("DISM.Tools.Free.Message")
        RichTextBox2.Text = LocalizationService.ForSection("PrgAbout.Resources")("PreviewChanges.Message")
        ForeColor = Color.White
        Label15.ForeColor = Color.Black
        PictureBox1.Image = If(MainForm.dtBranch.Contains("pre"), My.Resources.logo_preview, My.Resources.logo_aboutdlg_dark)
        If CreditsPanel.Visible Then
            LinkLabel1.LinkColor = Color.FromArgb(241, 241, 241)
            LinkLabel2.LinkColor = Color.FromArgb(153, 153, 153)
            LinkLabel3.LinkColor = Color.FromArgb(153, 153, 153)
        ElseIf LicensesPanel.Visible Then
            LinkLabel1.LinkColor = Color.FromArgb(153, 153, 153)
            LinkLabel2.LinkColor = Color.FromArgb(241, 241, 241)
            LinkLabel3.LinkColor = Color.FromArgb(153, 153, 153)
        ElseIf WhatsNewPanel.Visible Then
            LinkLabel1.LinkColor = Color.FromArgb(153, 153, 153)
            LinkLabel2.LinkColor = Color.FromArgb(153, 153, 153)
            LinkLabel3.LinkColor = Color.FromArgb(241, 241, 241)
        End If
        CreditsPanel.ForeColor = Color.White
        RichTextBox1.ForeColor = ForeColor
        RichTextBox2.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        UpdCheckBtn.Enabled = Not MainForm.SkipUpdates
    End Sub

    Private Sub ResizeImage()
        If BackgroundImage Is Nothing Then Exit Sub

        Dim scale As Double = WindowHelper.ScaleLogical(100) / 96.0F

        ' we don't need to resize with 100% display scaling
        If scale = 96.0F Then Exit Sub

        Dim newWidth As Integer = BackgroundImage.Width * scale,
            newHeight As Integer = BackgroundImage.Height * scale

        Dim scaled As New Bitmap(BackgroundImage, New Size(newWidth, newHeight))

        BackgroundImage = scaled

        resized = True
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Process.Start("https://icons8.com")
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        Process.Start("https://7-zip.org")
    End Sub

    Private Sub LinkLabel7_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked
        Process.Start("https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/deployment-image-servicing-and-management--dism--command-line-options")
    End Sub

    Private Sub LinkLabel8_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Process.Start("https://www.windowsafg.com/")
    End Sub

    Private Sub LinkLabel9_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel9.LinkClicked
        Process.Start("https://github.com/jacobslusser/ScintillaNET")
    End Sub

    Private Sub LinkLabel10_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel10.LinkClicked
        Process.Start("https://github.com/jeffkl/ManagedDism")
    End Sub

    Private Sub LinkLabel11_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel11.LinkClicked
        Process.Start("https://web.archive.org/web/20210907191944/https://twitter.com/prsymatic/status/1435317646346522628")
    End Sub

    Private Sub LinkLabel12_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel12.LinkClicked
        Process.Start("https://github.com/RobinPerris/DarkUI")
    End Sub

    Private Sub LinkLabel13_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Process.Start("https://github.com/DockPanelSuite/DockPanelSuite")
    End Sub

    Private Sub RichTextBox1_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox1.LinkClicked
        Process.Start(e.LinkText)
    End Sub

#Region "LinkLabel.MouseEnter events"

    Private Sub LinkLabel1_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel1.MouseEnter
        If LinkLabel1.LinkColor = Color.FromArgb(241, 241, 241) Then
            Cursor = Cursors.Arrow
            Exit Sub
        Else
            LinkLabel1.LinkColor = Color.Lime
        End If
    End Sub

    Private Sub LinkLabel2_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel2.MouseEnter
        If LinkLabel2.LinkColor = Color.FromArgb(241, 241, 241) Then
            Cursor = Cursors.Arrow
            Exit Sub
        Else
            LinkLabel2.LinkColor = Color.Lime
        End If
    End Sub

    Private Sub LinkLabel3_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel3.MouseEnter
        If LinkLabel3.LinkColor = Color.FromArgb(241, 241, 241) Then
            Cursor = Cursors.Arrow
            Exit Sub
        Else
            LinkLabel3.LinkColor = Color.Lime
        End If
    End Sub
#End Region

#Region "LinkLabel.MouseLeave events"

    Private Sub LinkLabel1_MouseLeave(sender As Object, e As EventArgs) Handles LinkLabel1.MouseLeave
        If CreditsPanel.Visible Then
            LinkLabel1.LinkColor = Color.FromArgb(241, 241, 241)
        Else
            LinkLabel1.LinkColor = Color.FromArgb(153, 153, 153)
        End If
    End Sub

    Private Sub LinkLabel2_MouseLeave(sender As Object, e As EventArgs) Handles LinkLabel2.MouseLeave
        If LicensesPanel.Visible Then
            LinkLabel2.LinkColor = Color.FromArgb(241, 241, 241)
        Else
            LinkLabel2.LinkColor = Color.FromArgb(153, 153, 153)
        End If
    End Sub

    Private Sub LinkLabel3_MouseLeave(sender As Object, e As EventArgs) Handles LinkLabel3.MouseLeave
        If WhatsNewPanel.Visible Then
            LinkLabel3.LinkColor = Color.FromArgb(241, 241, 241)
        Else
            LinkLabel3.LinkColor = Color.FromArgb(153, 153, 153)
        End If
    End Sub
#End Region

#Region "LinkLabel.LinkClicked events"

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        CreditsPanel.Visible = True
        LicensesPanel.Visible = False
        WhatsNewPanel.Visible = False
        LinkLabel1.LinkColor = Color.FromArgb(241, 241, 241)
        LinkLabel2.LinkColor = Color.FromArgb(153, 153, 153)
        LinkLabel3.LinkColor = Color.FromArgb(153, 153, 153)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        CreditsPanel.Visible = False
        LicensesPanel.Visible = True
        WhatsNewPanel.Visible = False
        LinkLabel1.LinkColor = Color.FromArgb(153, 153, 153)
        LinkLabel2.LinkColor = Color.FromArgb(241, 241, 241)
        LinkLabel3.LinkColor = Color.FromArgb(153, 153, 153)
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        CreditsPanel.Visible = False
        LicensesPanel.Visible = False
        WhatsNewPanel.Visible = True
        LinkLabel1.LinkColor = Color.FromArgb(153, 153, 153)
        LinkLabel2.LinkColor = Color.FromArgb(153, 153, 153)
        LinkLabel3.LinkColor = Color.FromArgb(241, 241, 241)
    End Sub
#End Region

    Private Sub Picture_MouseEnter(sender As Object, e As EventArgs) Handles PictureBox3.MouseEnter, PictureBox2.MouseEnter, PictureBox4.MouseEnter, PictureBox5.MouseEnter
        Cursor = Cursors.Hand
    End Sub

    Private Sub Picture_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox3.MouseLeave, PictureBox2.MouseLeave, PictureBox4.MouseLeave, PictureBox5.MouseLeave
        Cursor = Cursors.Arrow
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Process.Start("https://github.com/CodingWonders/DISMTools")
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Process.Start("https://reddit.com/r/DISMTools")
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        Process.Start("https://forums.mydigitallife.net/threads/discussion-dismtools.87263/")
    End Sub

    Private Sub PictureBox2_MouseHover(sender As Object, e As EventArgs) Handles PictureBox2.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("PrgAbout.Tooltip")("Project.GitHub.Label"))
    End Sub

    Private Sub PictureBox3_MouseHover(sender As Object, e As EventArgs) Handles PictureBox3.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("PrgAbout.Tooltip")("Text1.Label"))
    End Sub

    Private Sub PictureBox4_MouseHover(sender As Object, e As EventArgs) Handles PictureBox4.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("PrgAbout.Tooltip")("Project.MDL.Label"))
    End Sub

    Private Sub UpdCheckBtn_Click(sender As Object, e As EventArgs) Handles UpdCheckBtn.Click
        Try
            If File.Exists(Application.StartupPath & "\update.exe") Then File.Delete(Application.StartupPath & "\update.exe")
        Catch ex As Exception
            DynaLog.LogMessage("Could not delete existing update downloader...")
            Exit Sub
        End Try
        Try
            Using client As New WebClient()
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                client.DownloadFile("https://github.com/CodingWonders/DISMTools/raw/stable/Updater/DISMTools-UCS/update-bin/update.exe", Application.StartupPath & "\update.exe")
            End Using
        Catch ex As WebException
            MsgBox(LocalizationService.ForSection("PrgAbout.UpdateCheck").Format("Couldn.Tdownload.Message", ex.Status.ToString()), vbOKOnly + vbCritical, UpdCheckBtn.Text)
            Exit Sub
        End Try
        If File.Exists(Application.StartupPath & "\update.exe") Then Process.Start(Application.StartupPath & "\update.exe", "/" & MainForm.dtBranch & " /pid=" & Process.GetCurrentProcess().Id & " " & LocalizationService.GetLanguageCommandLineArgument())
    End Sub

    Private Sub PictureBox5_MouseHover(sender As Object, e As EventArgs) Handles PictureBox5.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("PrgAbout.Tooltip")("Join.Coding.Wonders.Label"))
    End Sub

    Private Sub PictureBox5_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click
        Process.Start("https://discord.gg/5TxEmKXNwu")
    End Sub
End Class
