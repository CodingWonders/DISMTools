Imports System.IO
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

Public Class MainForm

    Dim btnToolTip As New ToolTip()
    Private isMouseDown As Boolean = False
    Private mouseOffset As Point
    Dim pageInt As Integer = 0

    ' Argument variables
    Dim branch As String

    ' Progress variables
    Dim msg As String

    ' Necessary variables
    Dim latestVer As String
    Dim relTag As String

    Private Sub minBox_MouseEnter(sender As Object, e As EventArgs) Handles minBox.MouseEnter
        minBox.Image = My.Resources.minBox_focus
    End Sub

    Private Sub minBox_MouseLeave(sender As Object, e As EventArgs) Handles minBox.MouseLeave
        minBox.Image = My.Resources.minBox
    End Sub

    Private Sub minBox_MouseDown(sender As Object, e As MouseEventArgs) Handles minBox.MouseDown
        minBox.Image = My.Resources.minBox_down
    End Sub

    Private Sub minBox_MouseUp(sender As Object, e As MouseEventArgs) Handles minBox.MouseUp
        minBox.Image = My.Resources.minBox_focus
    End Sub

    Private Sub minBox_MouseHover(sender As Object, e As EventArgs) Handles minBox.MouseHover
        btnToolTip.SetToolTip(sender, "Minimize")
    End Sub

    Private Sub minBox_Click(sender As Object, e As EventArgs) Handles minBox.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub closeBox_MouseEnter(sender As Object, e As EventArgs) Handles closeBox.MouseEnter
        closeBox.Image = My.Resources.closebox_focus
    End Sub

    Private Sub closeBox_MouseLeave(sender As Object, e As EventArgs) Handles closeBox.MouseLeave
        closeBox.Image = My.Resources.closebox
    End Sub

    Private Sub closeBox_MouseDown(sender As Object, e As MouseEventArgs) Handles closeBox.MouseDown
        closeBox.Image = My.Resources.closebox_down
    End Sub

    Private Sub closeBox_MouseUp(sender As Object, e As MouseEventArgs) Handles closeBox.MouseUp
        closeBox.Image = My.Resources.closebox_focus
    End Sub

    Private Sub closeBox_MouseHover(sender As Object, e As EventArgs) Handles closeBox.MouseHover
        btnToolTip.SetToolTip(sender, "Close")
    End Sub

    Private Sub closeBox_Click(sender As Object, e As EventArgs) Handles closeBox.Click
        Close()
    End Sub

    Private Sub wndControlPanel_MouseDown(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Get the new position
            mouseOffset = New Point(-e.X, -e.Y)
            ' Set that left button is pressed
            isMouseDown = True
        End If
    End Sub

    Private Sub wndControlPanel_MouseMove(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            ' Get the new form position
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Location = mousePos
        End If
    End Sub

    Private Sub wndControlPanel_MouseUp(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "DISMTools Update Check System - Version " & Application.ProductVersion
        GetArguments()
        ReleaseFetcherBW.RunWorkerAsync()
    End Sub

    Sub GetArguments()
        If Environment.GetCommandLineArgs.Length = 1 Then
            MsgBox("The branch parameter is necessary to be able to check for updates", vbOKOnly + vbCritical, Text)
            Environment.Exit(1)
        Else
            Dim args() As String = Environment.GetCommandLineArgs()
            branch = args(1).Replace("/", "").Trim()
        End If
    End Sub

    Private Sub ReleaseFetcherBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ReleaseFetcherBW.DoWork
        msg = "Fetching update server..."
        Threading.Thread.Sleep(3000)
        ReleaseFetcherBW.ReportProgress(0)
        FetchUpdates()
        msg = "Comparing versions..."
        ReleaseFetcherBW.ReportProgress(50)
        CompareVersions()
    End Sub

    Private Sub ReleaseFetcherBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles ReleaseFetcherBW.ProgressChanged
        ProgressBar1.Style = ProgressBarStyle.Blocks
        ProgressBar1.Value = e.ProgressPercentage
        Label4.Text = msg
        If e.ProgressPercentage = 100 Then
            Label3.Text = "Update information"
            Label6.Text = FileVersionInfo.GetVersionInfo(Application.StartupPath & "\DISMTools.exe").FileVersion.ToString() & " → " & latestVer
            Panel1.Visible = True
            Label4.Visible = False
            ProgressBar1.Visible = False
        End If
    End Sub

    Sub FetchUpdates()
        Using client As New WebClient()
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Try
                client.DownloadFile("https://raw.githubusercontent.com/CodingWonders/DISMTools/" & branch & "/Updater/DISMTools-UCS/update-bin/" & If(branch.Contains("preview"), "preview.ini", "stable.ini"), Application.StartupPath & "\info.ini")
            Catch ex As WebException
                MsgBox("We couldn't fetch the necessary update information. Reason:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, Text)
                Environment.Exit(1)
            End Try
            If File.Exists(Application.StartupPath & "\info.ini") Then
                Dim infoRTB As New RichTextBox With {
                    .Text = File.ReadAllText(Application.StartupPath & "\info.ini")
                }
                For Each Line In infoRTB.Lines
                    If Line.StartsWith("LatestVer") Then
                        latestVer = Line.Replace("LatestVer = ", "").Trim()
                    ElseIf Line.StartsWith("ReleaseTag") Then
                        relTag = Line.Replace("ReleaseTag = ", "").Trim()
                    End If
                Next
                File.Delete(Application.StartupPath & "\info.ini")
            End If
        End Using
    End Sub

    Sub CompareVersions()
        If File.Exists(Application.StartupPath & "\DISMTools.exe") Then
            Dim fv As String = FileVersionInfo.GetVersionInfo(Application.StartupPath & "\DISMTools.exe").ProductVersion.ToString()
            If fv = latestVer Then
                MsgBox("There aren't any updates available", vbOKOnly + vbInformation, Text)
            Else
                ReleaseFetcherBW.ReportProgress(100)
            End If
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://github.com/CodingWonders/DISMTools/releases/tag/" & relTag)
    End Sub
End Class
