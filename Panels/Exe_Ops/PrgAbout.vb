Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class PrgAbout

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub PrgAbout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RichTextBox1.Text = My.Resources.LicenseOverview
        If MainForm.dtBranch.Contains("preview") Then
            PreviewPanel.Visible = True
        Else
            PreviewPanel.Visible = False
        End If
        Label1.Text = "DISMTools - version " & My.Application.Info.Version.ToString()
        Label15.Text = "Built on " & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe") & " by msbuild"
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TabPage1.BackColor = Color.FromArgb(31, 31, 31)
            TabPage2.BackColor = Color.FromArgb(31, 31, 31)
            RichTextBox1.BackColor = Color.FromArgb(31, 31, 31)
            PictureBox1.Image = My.Resources.logo_aboutdlg_dark
            PictureBox2.Image = My.Resources.preview_dark
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            RichTextBox1.BackColor = Color.FromArgb(238, 238, 242)
            PictureBox1.Image = My.Resources.logo_aboutdlg_light
            PictureBox2.Image = My.Resources.preview_light
        End If
        RichTextBox1.ForeColor = ForeColor
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Process.Start("https://icons8.com")
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        Process.Start("https://7-zip.org")
    End Sub

    Private Sub LinkLabel6_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel6.LinkClicked
        Process.Start("https://wimlib.net")
    End Sub

    Private Sub LinkLabel7_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked
        Process.Start("https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/deployment-image-servicing-and-management--dism--command-line-options")
    End Sub

    Private Sub LinkLabel8_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel8.LinkClicked
        Process.Start("https://www.windowsafg.com/")
    End Sub

    Private Sub LinkLabel9_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel9.LinkClicked
        Process.Start("https://github.com/jacobslusser/ScintillaNET")
    End Sub

    Private Sub LinkLabel10_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel10.LinkClicked
        Process.Start("https://github.com/jeffkl/ManagedDism")
    End Sub

    Private Sub RichTextBox1_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox1.LinkClicked
        Process.Start(e.LinkText)
    End Sub

    Function RetrieveLinkerTimestamp(ByVal filePath As String) As DateTime
        Const PeHeaderOffset As Integer = 60
        Const LinkerTimestampOffset As Integer = 8
        Dim b(2047) As Byte
        Dim s As Stream
        Try
            s = New FileStream(filePath, FileMode.Open, FileAccess.Read)
            s.Read(b, 0, 2048)
        Finally
            If Not s Is Nothing Then s.Close()
        End Try
        Dim i As Integer = BitConverter.ToInt32(b, PeHeaderOffset)
        Dim SecondsSince1970 As Integer = BitConverter.ToInt32(b, i + LinkerTimestampOffset)
        Dim dt As New DateTime(1970, 1, 1, 0, 0, 0)
        dt = dt.AddSeconds(SecondsSince1970)
        dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours)
        Return dt
    End Function
End Class
