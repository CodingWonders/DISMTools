Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.ComponentModel

Public Class MainForm

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Since we need Windows Server to run PXE Helper Servers, we'll block access to that page
        ' on non-Server Windows.
        Dim instTypeRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
        Dim instTypeVal As String = instTypeRk.GetValue("InstallationType", "")
        instTypeRk.Close()
        LinkLabel3.Enabled = (instTypeVal = "Server")
        PictureBox4.Image = If(instTypeVal = "Server", My.Resources.arrow_normal, My.Resources.arrow_disabled)
        PictureBox4.Enabled = (instTypeVal = "Server")
    End Sub

    Private Sub ExitLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles ExitLink.LinkClicked
        Close()
    End Sub

    Private Sub ArrowPic_MouseHover(sender As Object, e As EventArgs) Handles PictureBox4.MouseEnter, PictureBox3.MouseEnter, PictureBox2.MouseEnter, PictureBox5.MouseEnter, PictureBox9.MouseEnter, PictureBox8.MouseEnter
        CType(sender, PictureBox).Image = My.Resources.arrow_hovered
    End Sub

    Private Sub ArrowPic_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox4.MouseLeave, PictureBox3.MouseLeave, PictureBox2.MouseLeave, PictureBox5.MouseLeave, PictureBox9.MouseLeave, PictureBox8.MouseLeave
        CType(sender, PictureBox).Image = My.Resources.arrow_normal
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        MainMenuPanel.Visible = False
        PxeHelpersMenu.Visible = True
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        MainMenuPanel.Visible = True
        PxeHelpersMenu.Visible = False
    End Sub

    Private Sub BackArrowPic_MouseHover(sender As Object, e As EventArgs) Handles PictureBox6.MouseEnter
        CType(sender, PictureBox).Image = My.Resources.arrow_hovered_left
    End Sub

    Private Sub BackArrowPic_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox6.MouseLeave
        CType(sender, PictureBox).Image = My.Resources.arrow_normal_left
    End Sub

    Sub RunProcess(FilePath As String, Optional Arguments As String = "", Optional RunAsAdmin As Boolean = False)
        Visible = False
        Dim exitCode As Integer = ProcessHelper.RunProcess(FilePath, Arguments, RunAsAdmin)
        Visible = True
        If exitCode <> 0 Then
            MsgBox(String.Format("Process exited with code 0x{0}:" & CrLf & CrLf & "{1}", Hex(exitCode), New Win32Exception(exitCode).Message),
                   vbOKOnly + vbExclamation, Text)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        RunProcess(Path.Combine(Application.StartupPath, "setup.exe"), RunAsAdmin:=True)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        If MsgBox("This will restart your computer. Make sure you have configured your computer to boot via installation media. Do you want to restart?", vbYesNo + vbQuestion, "Computer Restart") = MsgBoxResult.Yes Then
            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "shutdown.exe"), "/r /t 0")
        End If
    End Sub

    Private Sub LinkLabel8_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel8.LinkClicked
        RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                   "-Executionpolicy Bypass -Command iex " & Quote & Path.Combine(Application.StartupPath, "pxehelpers", "wds", "wdshelper_server.ps1") & Quote,
                   True)
    End Sub

    Private Sub LinkLabel7_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked
        RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                   "-Executionpolicy Bypass -Command iex " & Quote & Path.Combine(Application.StartupPath, "pxehelpers", "fog", "foghelper_server.ps1") & Quote,
                   True)
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        RunProcess(Path.Combine(Application.StartupPath, "Tools", "SysprepPreparator", "SysprepPreparator.exe"), RunAsAdmin:=True)
    End Sub
End Class
