Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.ComponentModel

Public Class MainForm

    Private RestartMessage As String, ProcessExitCodeMessage As String

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Since we need Windows Server to run PXE Helper Servers, we'll block access to that page
        ' on non-Server Windows.
        Dim instTypeRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
        Dim instTypeVal As String = instTypeRk.GetValue("InstallationType", "")
        instTypeRk.Close()
        LinkLabel3.Enabled = (instTypeVal = "Server")
        PictureBox4.Image = If(instTypeVal = "Server", My.Resources.arrow_normal, My.Resources.arrow_disabled)
        PictureBox4.Enabled = (instTypeVal = "Server")

        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                RestartMessage = "This will restart your computer. Make sure you have configured your computer to boot via installation media. Do you want to restart?"
                ProcessExitCodeMessage = "Process exited with code 0x{0}:" & CrLf & CrLf & "{1}"
                Label1.Text = "What do you want to do?"
                Label3.Text = "Start a PXE Helper Server for Network Installation"
                LinkLabel1.Text = "Install an Operating System"
                LinkLabel2.Text = "Restart to Installation Media"
                LinkLabel3.Text = "Start a PXE Helper Server for Network Installation"
                LinkLabel4.Text = "Prepare System for Image Capture"
                LinkLabel5.Text = "Back"
                LinkLabel7.Text = "Start PXE Helper Server for FOG"
                LinkLabel8.Text = "Start PXE Helper Server for Windows Deployment Services"
                ExitLink.Text = "Exit"
            Case "ESN"
                RestartMessage = "Esto reiniciará su equipo. Asegúrese de haber configurado el equipo para iniciar este medio de instalación. ¿Desea reiniciar?"
                ProcessExitCodeMessage = "El proceso terminó con código 0x{0}:" & CrLf & CrLf & "{1}"
                Label1.Text = "¿Qué desea hacer?"
                Label3.Text = "Iniciar un servidor de PXE Helpers para instalación en red"
                LinkLabel1.Text = "Instalar un sistema operativo"
                LinkLabel2.Text = "Reiniciar desde el medio de instalación"
                LinkLabel3.Text = "Iniciar un servidor de PXE Helpers para instalación en red"
                LinkLabel4.Text = "Preparar el sistema para captura de imágenes"
                LinkLabel5.Text = "Atrás"
                LinkLabel7.Text = "Iniciar el servidor de PXE Helpers para FOG"
                LinkLabel8.Text = "Iniciar el servidor de PXE Helpers para WDS"
                ExitLink.Text = "Salir"
            Case "FRA"
                RestartMessage = "Votre ordinateur va redémarrer. Assurez-vous qu’il est configuré pour démarrer sur le média d’installation. Voulez-vous redémarrer ?"
                ProcessExitCodeMessage = "Processus terminé avec le code 0x{0} :" & CrLf & CrLf & "{1}"
                Label1.Text = "Que voulez-vous faire ?"
                Label3.Text = "Démarrer un serveur PXE Helper pour l’installation réseau"
                LinkLabel1.Text = "Installer un système d’exploitation"
                LinkLabel2.Text = "Redémarrer sur le média d’installation"
                LinkLabel3.Text = "Démarrer un serveur PXE Helper pour l’installation réseau"
                LinkLabel4.Text = "Préparer le système pour la capture d’image"
                LinkLabel5.Text = "Retour"
                LinkLabel7.Text = "Démarrer un serveur PXE Helper pour FOG"
                LinkLabel8.Text = "Démarrer un serveur PXE Helper pour WDS"
                ExitLink.Text = "Sortie"
            Case "PTB", "PTG"
                RestartMessage = "O computador será reiniciado. Certifique-se de que está configurado para iniciar pelo meio de instalação. Deseja reiniciar?"
                ProcessExitCodeMessage = "Processo terminou com o código 0x{0}:" & CrLf & CrLf & "{1}"
                Label1.Text = "O que deseja fazer?"
                Label3.Text = "Iniciar servidor PXE Helper para instalação em rede"
                LinkLabel1.Text = "Instalar um sistema operativo"
                LinkLabel2.Text = "Reiniciar para o meio de instalação"
                LinkLabel3.Text = "Iniciar servidor PXE Helper para instalação em rede"
                LinkLabel4.Text = "Preparar sistema para captura de imagem"
                LinkLabel5.Text = "Voltar"
                LinkLabel7.Text = "Iniciar servidor PXE Helper para FOG"
                LinkLabel8.Text = "Iniciar servidor PXE Helper para WDS"
                ExitLink.Text = "Sair"
            Case "ITA"
                RestartMessage = "Il computer verrà riavviato. Assicurati che sia configurato per avviarsi dal supporto di installazione. Vuoi riavviare?"
                ProcessExitCodeMessage = "Processo terminato con codice 0x{0}:" & CrLf & CrLf & "{1}"
                Label1.Text = "Cosa vuoi fare?"
                Label3.Text = "Avvia server PXE Helper per installazione di rete"
                LinkLabel1.Text = "Installa un sistema operativo"
                LinkLabel2.Text = "Riavvia al supporto di installazione"
                LinkLabel3.Text = "Avvia server PXE Helper per installazione di rete"
                LinkLabel4.Text = "Prepara sistema per acquisizione immagine"
                LinkLabel5.Text = "Indietro"
                LinkLabel7.Text = "Avvia server PXE Helper per FOG"
                LinkLabel8.Text = "Avvia server PXE Helper per WDS"
                ExitLink.Text = "Esci"
        End Select
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
            MsgBox(String.Format(ProcessExitCodeMessage, Hex(exitCode), New Win32Exception(exitCode).Message),
                   vbOKOnly + vbExclamation, Text)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        RunProcess(Path.Combine(Application.StartupPath, "setup.exe"), RunAsAdmin:=True)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        If MsgBox(RestartMessage, vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "shutdown.exe"), "/r /t 0")
        End If
    End Sub

    Private Sub LinkLabel8_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel8.LinkClicked
        RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                   "-Executionpolicy Bypass -File " & Quote & Path.Combine(Application.StartupPath, "pxehelpers", "wds", "wdshelper_server.ps1") & Quote,
                   True)
    End Sub

    Private Sub LinkLabel7_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked
        RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                   "-Executionpolicy Bypass -File " & Quote & Path.Combine(Application.StartupPath, "pxehelpers", "fog", "foghelper_server.ps1") & Quote,
                   True)
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        RunProcess(Path.Combine(Application.StartupPath, "Tools", "SysprepPreparator", "SysprepPreparator.exe"), RunAsAdmin:=True)
    End Sub
End Class
