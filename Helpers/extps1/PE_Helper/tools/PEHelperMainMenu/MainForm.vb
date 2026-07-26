Imports Microsoft.Win32
Imports System.IO
Imports System.ComponentModel

Public Class MainForm

    Private RestartMessage As String

    Friend AutoCapture As Boolean = False
    Friend CopyProfile As Boolean = False

    Private serverPort As Integer = 8080

    Private pxeServerPortSwitcherMessage As String = ""

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Since we need Windows Server to run PXE Helper Servers, we'll block access to that page
        ' on non-Server Windows.
        Dim instTypeRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
        Dim instTypeVal As String = instTypeRk.GetValue("InstallationType", "")
        instTypeRk.Close()
        LinkLabel3.Enabled = (instTypeVal.ToLower().Contains("server"))
        PictureBox4.Image = If(instTypeVal.ToLower().Contains("server"), My.Resources.arrow_normal, My.Resources.arrow_disabled)
        PictureBox4.Enabled = (instTypeVal.ToLower().Contains("server"))

        RestartMessage = LocalizationService.ForSection("PEHelper.Restart")("Warning.Message")
        Label1.Text = LocalizationService.ForSection("PEHelper.Main")("WhatWant.Label")
        Label3.Text = LocalizationService.ForSection("PEHelper.Main")("StartServer.Label")
        LinkLabel1.Text = LocalizationService.ForSection("PEHelper.Main")("Install.Operating.Link")
        LinkLabel2.Text = LocalizationService.ForSection("PEHelper.Main")("Restart.Install.Media.Link")
        LinkLabel3.Text = LocalizationService.ForSection("PEHelper.Main")("StartServer.Network.Link")
        LinkLabel4.Text = LocalizationService.ForSection("PEHelper.Main")("Prepare.System.Image.Link")
        LinkLabel5.Text = LocalizationService.ForSection("PEHelper.Main")("Back.Button")
        LinkLabel6.Text = LocalizationService.ForSection("PEHelper.Main")("Explore.Contents.Disc.Link")
        LinkLabel7.Text = LocalizationService.ForSection("PEHelper.Main")("StartServer.Fog.Link")
        LinkLabel8.Text = LocalizationService.ForSection("PEHelper.Main")("StartServer.Wds.Link")
        LinkLabel9.Text = LocalizationService.ForSection("PEHelper.Main")("Copy.Boot.Image.Link")
        ExitLink.Text = LocalizationService.ForSection("PEHelper.Main")("Exit.Button")
        pxeServerPortSwitcherMessage = LocalizationService.ForSection("PEHelper.PXE")("ChangePort.Tooltip")
    End Sub

    Private Sub ExitLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles ExitLink.LinkClicked
        Close()
    End Sub

    Private Sub ArrowPic_MouseHover(sender As Object, e As EventArgs) Handles PictureBox4.MouseEnter, PictureBox3.MouseEnter, PictureBox2.MouseEnter, PictureBox5.MouseEnter, PictureBox9.MouseEnter, PictureBox8.MouseEnter, PictureBox7.MouseEnter, PictureBox10.MouseEnter, PictureBox11.MouseEnter
        CType(sender, PictureBox).Image = My.Resources.arrow_hovered
    End Sub

    Private Sub ArrowPic_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox4.MouseLeave, PictureBox3.MouseLeave, PictureBox2.MouseLeave, PictureBox5.MouseLeave, PictureBox9.MouseLeave, PictureBox8.MouseLeave, PictureBox7.MouseLeave, PictureBox10.MouseLeave, PictureBox11.MouseLeave
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

    Sub RunProcess(FilePath As String, Optional Arguments As String = "", Optional WorkingDirectory As String = "", Optional RunAsAdmin As Boolean = False)
        Visible = False
        Dim exitCode As Integer = ProcessHelper.RunProcess(FilePath, Arguments, WorkingDirectory, RunAsAdmin)
        Visible = True
        If exitCode <> 0 Then
            MsgBox(LocalizationService.ForSection("PEHelper.Process").Format("ExitCode.Message", Hex(exitCode), New Win32Exception(exitCode).Message),
                   vbOKOnly + vbExclamation, Text)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        RunProcess(Path.Combine(Application.StartupPath, "setup.exe"),
                   "--language=" & Quote & LocalizationService.CurrentCultureCode & Quote,
                   RunAsAdmin:=True)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        If MsgBox(RestartMessage, vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "shutdown.exe"), "/r /t 0")
        End If
    End Sub

    Private Sub LinkLabel8_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel8.LinkClicked
        If ModifierKeys.HasFlag(Keys.Shift) AndAlso ServerPortSpecifier.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            ' Use a different port then
            serverPort = ServerPortSpecifier.ServerPort
        End If
        RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                   "-Executionpolicy Bypass -File " & Quote & Path.Combine(Application.StartupPath, "pxehelpers", "wds", "wdshelper_server.ps1") & Quote & " -sPort " & serverPort,
                   RunAsAdmin:=True)
    End Sub

    Private Sub LinkLabel7_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked
        If ModifierKeys.HasFlag(Keys.Shift) AndAlso ServerPortSpecifier.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            ' Use a different port then
            serverPort = ServerPortSpecifier.ServerPort
        End If
        RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                   "-Executionpolicy Bypass -File " & Quote & Path.Combine(Application.StartupPath, "pxehelpers", "fog", "foghelper_server.ps1") & Quote & " -sPort " & serverPort,
                   RunAsAdmin:=True)
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Dim SPDlgResult As DialogResult = SysprepPreparatorModeDialog.ShowDialog(Me)
        If SPDlgResult = Windows.Forms.DialogResult.Cancel Then Exit Sub

        Dim args As String = ""

        If SPDlgResult = Windows.Forms.DialogResult.Yes Then args &= "/auto"
        If AutoCapture Then args &= " /dt_capture"
        If CopyProfile Then args &= " /copyprofile"

        RunProcess(Path.Combine(Application.StartupPath, "Tools", "SysprepPreparator", "SysprepPreparator.exe"), args, True)
    End Sub

    Private Sub LinkLabel6_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel6.LinkClicked
        Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"), Application.StartupPath)
    End Sub

    Private Sub LinkLabel9_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel9.LinkClicked
        If WDSBootImageArchitectureSelector.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "cmd.exe"),
                       "/c " & Quote & Path.Combine(Application.StartupPath, "boot_image_to_wds.bat") & Quote & " /arch=" & WDSBootImageArchitectureSelector.ComboBox1.SelectedItem, Application.StartupPath, RunAsAdmin:=True)
        End If
    End Sub

    Private Sub LinkLabel10_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel10.LinkClicked
        Dim selectedImageGroup As String = ""
        If WDSImageGroupSpecifier.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            selectedImageGroup = WDSImageGroupSpecifier.SpecifiedImageGroup
            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                       "-Executionpolicy Bypass -File " & Quote & Path.Combine(Application.StartupPath, "install_image_to_wds.ps1") & Quote &
                       " -imageGroup " & Quote & selectedImageGroup & Quote & " -installImagePath " & Quote & Path.Combine(Application.StartupPath, "sources", "install.wim") & Quote,
                       RunAsAdmin:=True)
        End If
    End Sub

    Private Sub LinkLabel8_MouseHover(sender As Object, e As EventArgs) Handles LinkLabel8.MouseHover, LinkLabel7.MouseHover
        Dim displayedToolTip As New ToolTip()
        displayedToolTip.SetToolTip(sender, pxeServerPortSwitcherMessage)
    End Sub
End Class
