Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Net

Public Class PrgAbout

    Dim PictureToolTip As New ToolTip()

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub PrgAbout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "About this program"
                        Label1.Text = "DISMTools - version " & My.Application.Info.Version.ToString() & If(MainForm.dtBranch.Contains("preview"), "." & MainForm.dtBranch & "_" & MainForm.dt_codeName.ToLower() & "." & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe").ToString("yyMMdd-HHmm"), "")
                        Label2.Text = "DISMTools lets you deploy, manage, and service Windows images with ease, thanks to a GUI"
                        Label3.Text = "These resources and components were used in the creation of this program:"
                        Label4.Text = "Resources"
                        Label5.Text = "Fluency"
                        Label6.Text = "SQL Server icon (Color)"
                        Label7.Text = "Utilities"
                        Label8.Text = "7-Zip"
                        Label9.Text = "wimlib-imagex (used in the future)"
                        Label10.Text = "Help documentation"
                        Label11.Text = "Command Help source"
                        Label12.Text = "Unattended answer file creation wizard"
                        Label13.Text = "Scintilla.NET (NuGet package)"
                        If Not MainForm.dtBranch.Contains("preview") Then
                            Label15.Text = "Built on " & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe") & " by msbuild"
                            Label15.Visible = True
                        End If
                        Label16.Text = "ManagedDism (NuGet package)"
                        Label17.Text = "Branding assets"
                        Label18.Text = "Windows Home Server wallpaper"
                        LinkLabel1.Text = "CREDITS"
                        LinkLabel2.Text = "LICENSES"
                        LinkLabel3.Text = "WHAT'S NEW"
                        LinkLabel4.Text = "Icons8"
                        LinkLabel5.Text = "Visit website"
                        LinkLabel6.Text = "Visit website"
                        LinkLabel7.Text = "Microsoft"
                        LinkLabel8.Text = "The Unattended answer file creation wizard is based on the Windows Answer File Generator website"
                        LinkLabel8.LinkArea = New LinkArea(59, 29)
                        LinkLabel9.Text = "Visit website"
                        LinkLabel10.Text = "Visit website"
                        LinkLabel11.Text = "Microsoft, Kýnan Antos"
                        LinkLabel12.Text = "Visit website"
                        LinkLabel13.Text = "Visit website"
                        OK_Button.Text = "OK"
                    Case "ESN"
                        Text = "Acerca de este programa"
                        Label1.Text = "DISMTools - versión " & My.Application.Info.Version.ToString() & If(MainForm.dtBranch.Contains("preview"), "." & MainForm.dtBranch & "_" & MainForm.dt_codeName.ToLower() & "." & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe").ToString("yyMMdd-HHmm"), "")
                        Label2.Text = "DISMTools le permite implementar, administrar, y ofrecer servicio a imágenes de Windows con facilidad, gracias a una GUI"
                        Label3.Text = "Estos recursos y componentes fueron utilizados en la creación de este programa:"
                        Label4.Text = "Recursos"
                        Label5.Text = "Fluency"
                        Label6.Text = "Icono de SQL Server (Color)"
                        Label7.Text = "Utilidades"
                        Label8.Text = "7-Zip"
                        Label9.Text = "wimlib-imagex (usado en el futuro)"
                        Label10.Text = "Documentación de ayuda"
                        Label11.Text = "Fuente de ayuda de comandos"
                        Label12.Text = "Creación de archivos de respuesta desatendida"
                        Label13.Text = "Scintilla.NET (paquete NuGet)"
                        If Not MainForm.dtBranch.Contains("preview") Then
                            Label15.Text = "Compilado el " & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe") & " por msbuild"
                            Label15.Visible = True
                        End If
                        Label16.Text = "ManagedDism (paquete NuGet)"
                        Label17.Text = "Recursos publicitarios"
                        Label18.Text = "Fondo de pantalla de Windows Home Server"
                        LinkLabel1.Text = "CRÉDITOS"
                        LinkLabel2.Text = "LICENCIAS"
                        LinkLabel3.Text = "NOVEDADES"
                        LinkLabel4.Text = "Icons8"
                        LinkLabel5.Text = "Visitar sitio"
                        LinkLabel6.Text = "Visitar sitio"
                        LinkLabel7.Text = "Microsoft"
                        LinkLabel8.Text = "El Asistente de creación de archivos de respuesta desatendida está basado en el sitio Windows Answer File Generator"
                        LinkLabel8.LinkArea = New LinkArea(86, 29)
                        LinkLabel9.Text = "Visitar sitio"
                        LinkLabel10.Text = "Visitar sitio"
                        LinkLabel11.Text = "Microsoft, Kýnan Antos"
                        LinkLabel12.Text = "Visitar sitio"
                        LinkLabel13.Text = "Visitar sitio"
                        OK_Button.Text = "Aceptar"
                        UpdCheckBtn.Text = "Comprobar actualizaciones"
                    Case "FRA"
                        Text = "À propos de ce programme"
                        Label1.Text = "DISMTools - version " & My.Application.Info.Version.ToString() & If(MainForm.dtBranch.Contains("preview"), "." & MainForm.dtBranch & "_" & MainForm.dt_codeName.ToLower() & "." & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe").ToString("yyMMdd-HHmm"), "")
                        Label2.Text = "DISMTools vous permet de déployer, de gérer et d'entretenir des images Windows en toute simplicité, grâce à une interface graphique."
                        Label3.Text = "Ces ressources et éléments ont été utilisés pour la création de ce programme :"
                        Label4.Text = "Ressources"
                        Label5.Text = "Fluency"
                        Label6.Text = "Icône SQL Server (Color)"
                        Label7.Text = "Outils"
                        Label8.Text = "7-Zip"
                        Label9.Text = "wimlib-imagex (utilisé à l'avenir)"
                        Label10.Text = "Documentation d'aide"
                        Label11.Text = "Source d'aide à la commande"
                        Label12.Text = "Création de fichiers de réponse sans surveillance"
                        Label13.Text = "Scintilla.NET (paquet NuGet)"
                        If Not MainForm.dtBranch.Contains("preview") Then
                            Label15.Text = "Construit le " & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe") & " par msbuild"
                            Label15.Visible = True
                        End If
                        Label16.Text = "ManagedDism (paquet NuGet)"
                        Label17.Text = "Les atouts de la marque"
                        Label18.Text = "Fond d'écran Windows Home Server"
                        LinkLabel1.Text = "CRÉDITS"
                        LinkLabel2.Text = "LICENCES"
                        LinkLabel3.Text = "QUOI DE NEUF"
                        LinkLabel4.Text = "Icons8"
                        LinkLabel5.Text = "Site web"
                        LinkLabel6.Text = "Site web"
                        LinkLabel7.Text = "Microsoft"
                        LinkLabel8.Text = "L'assistant de création de fichiers de réponses sans surveillance est basé sur le site web Windows Answer File Generator"
                        LinkLabel8.LinkArea = New LinkArea(79, 41)
                        LinkLabel9.Text = "Site web"
                        LinkLabel10.Text = "Site web"
                        LinkLabel11.Text = "Microsoft, Kýnan Antos"
                        LinkLabel12.Text = "Site web"
                        LinkLabel13.Text = "Site web"
                        OK_Button.Text = "OK"
                        UpdCheckBtn.Text = "Vérifier les mises à jour"
                    Case "PTB", "PTG"
                        Text = "Acerca deste programa"
                        Label1.Text = "DISMTools - versão " & My.Application.Info.Version.ToString() & If(MainForm.dtBranch.Contains("preview"), "." & MainForm.dtBranch & "_" & MainForm.dt_codeName.ToLower() & "." & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe").ToString("yyMMdd-HHmm"), "")
                        Label2.Text = "DISMTools permite-lhe implementar, gerir e efetuar a manutenção de imagens do Windows com facilidade, graças a uma GUI"
                        Label3.Text = "Estes recursos e componentes foram utilizados na criação deste programa:"
                        Label4.Text = "Recursos"
                        Label5.Text = "Fluency"
                        Label6.Text = "Ícone do SQL Server (Cor)"
                        Label7.Text = "Utilitários"
                        Label8.Text = "7-Zip"
                        Label9.Text = "wimlib-imagex (usado no futuro)"
                        Label10.Text = "Documentação de ajuda"
                        Label11.Text = "Fonte da Ajuda do Comando"
                        Label12.Text = "Criação de ficheiros de resposta não assistida"
                        Label13.Text = "Scintilla.NET (pacote NuGet)"
                        If Not MainForm.dtBranch.Contains("preview") Then
                            Label15.Text = "Construído em " & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe") & " por msbuild"
                            Label15.Visible = True
                        End If
                        Label16.Text = "ManagedDism (pacote NuGet)"
                        Label17.Text = "Activos de marca"
                        Label18.Text = "Papel de parede do Windows Home Server"
                        LinkLabel1.Text = "CRÉDITOS"
                        LinkLabel2.Text = "LICENÇAS"
                        LinkLabel3.Text = "O QUE HÁ DE NOVO"
                        LinkLabel4.Text = "Ícones8"
                        LinkLabel5.Text = "Sítio Web"
                        LinkLabel6.Text = "Sítio Web"
                        LinkLabel7.Text = "Microsoft"
                        LinkLabel8.Text = "O assistente de criação de ficheiros de resposta não assistida baseia-se no sítio Web do Gerador de Ficheiros de Resposta do Windows"
                        LinkLabel8.LinkArea = New LinkArea(76, 56)
                        LinkLabel9.Text = "Sítio Web"
                        LinkLabel10.Text = "Sítio Web"
                        LinkLabel11.Text = "Microsoft, Kýnan Antos"
                        LinkLabel12.Text = "Sítio Web"
                        LinkLabel13.Text = "Sítio Web"
                        OK_Button.Text = "OK"
                        UpdCheckBtn.Text = "Verificar actualizações"
                End Select
            Case 1
                Text = "About this program"
                Label1.Text = "DISMTools - version " & My.Application.Info.Version.ToString() & If(MainForm.dtBranch.Contains("preview"), "." & MainForm.dtBranch & "_" & MainForm.dt_codeName.ToLower() & "." & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe").ToString("yyMMdd-HHmm"), "")
                Label2.Text = "DISMTools lets you deploy, manage, and service Windows images with ease, thanks to a GUI"
                Label3.Text = "These resources and components were used in the creation of this program:"
                Label4.Text = "Resources"
                Label5.Text = "Fluency"
                Label6.Text = "SQL Server icon (Color)"
                Label7.Text = "Utilities"
                Label8.Text = "7-Zip"
                Label9.Text = "wimlib-imagex (used in the future)"
                Label10.Text = "Help documentation"
                Label11.Text = "Command Help source"
                Label12.Text = "Unattended answer file creation wizard"
                Label13.Text = "Scintilla.NET (NuGet package)"
                If Not MainForm.dtBranch.Contains("preview") Then
                    Label15.Text = "Built on " & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe") & " by msbuild"
                    Label15.Visible = True
                End If
                Label16.Text = "ManagedDism (NuGet package)"
                Label17.Text = "Branding assets"
                Label18.Text = "Windows Home Server wallpaper"
                LinkLabel1.Text = "CREDITS"
                LinkLabel2.Text = "LICENSES"
                LinkLabel3.Text = "WHAT'S NEW"
                LinkLabel4.Text = "Icons8"
                LinkLabel5.Text = "Visit website"
                LinkLabel6.Text = "Visit website"
                LinkLabel7.Text = "Microsoft"
                LinkLabel8.Text = "The Unattended answer file creation wizard is based on the Windows Answer File Generator website"
                LinkLabel8.LinkArea = New LinkArea(59, 29)
                LinkLabel9.Text = "Visit website"
                LinkLabel10.Text = "Visit website"
                LinkLabel11.Text = "Microsoft, Kýnan Antos"
                LinkLabel12.Text = "Visit website"
                LinkLabel13.Text = "Visit website"
                OK_Button.Text = "OK"
                UpdCheckBtn.Text = "Check for updates"
            Case 2
                Text = "Acerca de este programa"
                Label1.Text = "DISMTools - versión " & My.Application.Info.Version.ToString() & If(MainForm.dtBranch.Contains("preview"), "." & MainForm.dtBranch & "_" & MainForm.dt_codeName.ToLower() & "." & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe").ToString("yyMMdd-HHmm"), "")
                Label2.Text = "DISMTools le permite implementar, administrar, y ofrecer servicio a imágenes de Windows con facilidad, gracias a una GUI"
                Label3.Text = "Estos recursos y componentes fueron utilizados en la creación de este programa:"
                Label4.Text = "Recursos"
                Label5.Text = "Fluency"
                Label6.Text = "Icono de SQL Server (Color)"
                Label7.Text = "Utilidades"
                Label8.Text = "7-Zip"
                Label9.Text = "wimlib-imagex (usado en el futuro)"
                Label10.Text = "Documentación de ayuda"
                Label11.Text = "Fuente de ayuda de comandos"
                Label12.Text = "Creación de archivos de respuesta desatendida"
                Label13.Text = "Scintilla.NET (paquete NuGet)"
                If Not MainForm.dtBranch.Contains("preview") Then
                    Label15.Text = "Compilado el " & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe") & " por msbuild"
                    Label15.Visible = True
                End If
                Label16.Text = "ManagedDism (paquete NuGet)"
                Label17.Text = "Recursos publicitarios"
                Label18.Text = "Fondo de pantalla de Windows Home Server"
                LinkLabel1.Text = "CRÉDITOS"
                LinkLabel2.Text = "LICENCIAS"
                LinkLabel3.Text = "NOVEDADES"
                LinkLabel4.Text = "Icons8"
                LinkLabel5.Text = "Visitar sitio"
                LinkLabel6.Text = "Visitar sitio"
                LinkLabel7.Text = "Microsoft"
                LinkLabel8.Text = "El Asistente de creación de archivos de respuesta desatendida está basado en el sitio Windows Answer File Generator"
                LinkLabel8.LinkArea = New LinkArea(86, 29)
                LinkLabel9.Text = "Visitar sitio"
                LinkLabel10.Text = "Visitar sitio"
                LinkLabel11.Text = "Microsoft, Kýnan Antos"
                LinkLabel12.Text = "Visitar sitio"
                LinkLabel13.Text = "Visitar sitio"
                OK_Button.Text = "Aceptar"
                UpdCheckBtn.Text = "Comprobar actualizaciones"
            Case 3
                Text = "À propos de ce programme"
                Label1.Text = "DISMTools - version " & My.Application.Info.Version.ToString() & If(MainForm.dtBranch.Contains("preview"), "." & MainForm.dtBranch & "_" & MainForm.dt_codeName.ToLower() & "." & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe").ToString("yyMMdd-HHmm"), "")
                Label2.Text = "DISMTools vous permet de déployer, de gérer et d'entretenir des images Windows en toute simplicité, grâce à une interface graphique."
                Label3.Text = "Ces ressources et éléments ont été utilisés pour la création de ce programme :"
                Label4.Text = "Ressources"
                Label5.Text = "Fluency"
                Label6.Text = "Icône SQL Server (Color)"
                Label7.Text = "Outils"
                Label8.Text = "7-Zip"
                Label9.Text = "wimlib-imagex (utilisé à l'avenir)"
                Label10.Text = "Documentation d'aide"
                Label11.Text = "Source d'aide à la commande"
                Label12.Text = "Création de fichiers de réponse sans surveillance"
                Label13.Text = "Scintilla.NET (paquet NuGet)"
                If Not MainForm.dtBranch.Contains("preview") Then
                    Label15.Text = "Construit le " & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe") & " par msbuild"
                    Label15.Visible = True
                End If
                Label16.Text = "ManagedDism (paquet NuGet)"
                Label17.Text = "Les atouts de la marque"
                Label18.Text = "Fond d'écran Windows Home Server"
                LinkLabel1.Text = "CRÉDITS"
                LinkLabel2.Text = "LICENCES"
                LinkLabel3.Text = "QUOI DE NEUF"
                LinkLabel4.Text = "Icons8"
                LinkLabel5.Text = "Site web"
                LinkLabel6.Text = "Site web"
                LinkLabel7.Text = "Microsoft"
                LinkLabel8.Text = "L'assistant de création de fichiers de réponses sans surveillance est basé sur le site web Windows Answer File Generator"
                LinkLabel8.LinkArea = New LinkArea(79, 41)
                LinkLabel9.Text = "Site web"
                LinkLabel10.Text = "Site web"
                LinkLabel11.Text = "Microsoft, Kýnan Antos"
                LinkLabel12.Text = "Site web"
                LinkLabel13.Text = "Site web"
                OK_Button.Text = "OK"
                UpdCheckBtn.Text = "Vérifier les mises à jour"
            Case 4
                Text = "Acerca deste programa"
                Label1.Text = "DISMTools - versão " & My.Application.Info.Version.ToString() & If(MainForm.dtBranch.Contains("preview"), "." & MainForm.dtBranch & "_" & MainForm.dt_codeName.ToLower() & "." & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe").ToString("yyMMdd-HHmm"), "")
                Label2.Text = "DISMTools permite-lhe implementar, gerir e efetuar a manutenção de imagens do Windows com facilidade, graças a uma GUI"
                Label3.Text = "Estes recursos e componentes foram utilizados na criação deste programa:"
                Label4.Text = "Recursos"
                Label5.Text = "Fluency"
                Label6.Text = "Ícone do SQL Server (Cor)"
                Label7.Text = "Utilitários"
                Label8.Text = "7-Zip"
                Label9.Text = "wimlib-imagex (usado no futuro)"
                Label10.Text = "Documentação de ajuda"
                Label11.Text = "Fonte da Ajuda do Comando"
                Label12.Text = "Criação de ficheiros de resposta não assistida"
                Label13.Text = "Scintilla.NET (pacote NuGet)"
                If Not MainForm.dtBranch.Contains("preview") Then
                    Label15.Text = "Construído em " & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe") & " por msbuild"
                    Label15.Visible = True
                End If
                Label16.Text = "ManagedDism (pacote NuGet)"
                Label17.Text = "Activos de marca"
                Label18.Text = "Papel de parede do Windows Home Server"
                LinkLabel1.Text = "CRÉDITOS"
                LinkLabel2.Text = "LICENÇAS"
                LinkLabel3.Text = "O QUE HÁ DE NOVO"
                LinkLabel4.Text = "Ícones8"
                LinkLabel5.Text = "Sítio Web"
                LinkLabel6.Text = "Sítio Web"
                LinkLabel7.Text = "Microsoft"
                LinkLabel8.Text = "O assistente de criação de ficheiros de resposta não assistida baseia-se no sítio Web do Gerador de Ficheiros de Resposta do Windows"
                LinkLabel8.LinkArea = New LinkArea(76, 56)
                LinkLabel9.Text = "Sítio Web"
                LinkLabel10.Text = "Sítio Web"
                LinkLabel11.Text = "Microsoft, Kýnan Antos"
                LinkLabel12.Text = "Sítio Web"
                LinkLabel13.Text = "Sítio Web"
                OK_Button.Text = "OK"
                UpdCheckBtn.Text = "Verificar actualizações"
        End Select
        RichTextBox1.Text = My.Resources.LicenseOverview
        RichTextBox2.Text = My.Resources.WhatsNew
        ForeColor = Color.White
        PictureBox1.Image = If(MainForm.dtBranch.Contains("preview"), My.Resources.logo_preview, My.Resources.logo_aboutdlg_dark)
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
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        UpdCheckBtn.Enabled = Not MainForm.SkipUpdates
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

    Private Sub LinkLabel11_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel11.LinkClicked
        Process.Start("https://web.archive.org/web/20210907191944/https://twitter.com/prsymatic/status/1435317646346522628")
    End Sub

    Private Sub LinkLabel12_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel12.LinkClicked
        Process.Start("https://github.com/RobinPerris/DarkUI")
    End Sub

    Private Sub LinkLabel13_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel13.LinkClicked
        Process.Start("https://github.com/DockPanelSuite/DockPanelSuite")
    End Sub

    Private Sub RichTextBox1_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox1.LinkClicked
        Process.Start(e.LinkText)
    End Sub

    Function RetrieveLinkerTimestamp(ByVal filePath As String) As DateTime
        Const PeHeaderOffset As Integer = 60
        Const LinkerTimestampOffset As Integer = 8
        Dim b(2047) As Byte
        Dim s As Stream = Nothing
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
        Dim tz As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")
        dt = TimeZoneInfo.ConvertTimeFromUtc(dt, tz)
        Return dt
    End Function

#Region "LinkLabel.MouseEnter events"

    Private Sub LinkLabel1_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel1.MouseEnter
        If LinkLabel1.LinkColor = Color.FromArgb(241, 241, 241) Then
            Cursor = Cursors.Arrow
            Exit Sub
        Else
            LinkLabel1.LinkColor = Color.FromArgb(0, 151, 251)
        End If
    End Sub

    Private Sub LinkLabel2_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel2.MouseEnter
        If LinkLabel2.LinkColor = Color.FromArgb(241, 241, 241) Then
            Cursor = Cursors.Arrow
            Exit Sub
        Else
            LinkLabel2.LinkColor = Color.FromArgb(0, 151, 251)
        End If
    End Sub

    Private Sub LinkLabel3_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel3.MouseEnter
        If LinkLabel3.LinkColor = Color.FromArgb(241, 241, 241) Then
            Cursor = Cursors.Arrow
            Exit Sub
        Else
            LinkLabel3.LinkColor = Color.FromArgb(0, 151, 251)
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

    Private Sub PictureBox5_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click
        Process.Start("https://matrix.to/#/#dismtools:gitter.im")
    End Sub

    Private Sub PictureBox2_MouseHover(sender As Object, e As EventArgs) Handles PictureBox2.MouseHover
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PictureToolTip.SetToolTip(sender, "Check out the project's repository on GitHub")
                    Case "ESN"
                        PictureToolTip.SetToolTip(sender, "Consulte el repositorio del proyecto en GitHub")
                    Case "FRA"
                        PictureToolTip.SetToolTip(sender, "Consultez le dépôt du projet sur GitHub")
                    Case "PTB", "PTG"
                        PictureToolTip.SetToolTip(sender, "Consulte o repositório do projeto no GitHub")
                End Select
            Case 1
                PictureToolTip.SetToolTip(sender, "Check out the project's repository on GitHub")
            Case 2
                PictureToolTip.SetToolTip(sender, "Consulte el repositorio del proyecto en GitHub")
            Case 3
                PictureToolTip.SetToolTip(sender, "Consultez le dépôt du projet sur GitHub")
            Case 4
                PictureToolTip.SetToolTip(sender, "Consulte o repositório do projeto no GitHub")
        End Select
    End Sub

    Private Sub PictureBox3_MouseHover(sender As Object, e As EventArgs) Handles PictureBox3.MouseHover
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PictureToolTip.SetToolTip(sender, "Check out the project's official subreddit")
                    Case "ESN"
                        PictureToolTip.SetToolTip(sender, "Consulte el subreddit oficial del proyecto")
                    Case "FRA"
                        PictureToolTip.SetToolTip(sender, "Consultez le subreddit officiel du projet")
                    Case "PTB", "PTG"
                        PictureToolTip.SetToolTip(sender, "Consulte o subreddit oficial do projeto")
                End Select
            Case 1
                PictureToolTip.SetToolTip(sender, "Check out the project's official subreddit")
            Case 2
                PictureToolTip.SetToolTip(sender, "Consulte el subreddit oficial del proyecto")
            Case 3
                PictureToolTip.SetToolTip(sender, "Consultez le subreddit officiel du projet")
            Case 4
                PictureToolTip.SetToolTip(sender, "Consulte o subreddit oficial do projeto")
        End Select
    End Sub

    Private Sub PictureBox4_MouseHover(sender As Object, e As EventArgs) Handles PictureBox4.MouseHover
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PictureToolTip.SetToolTip(sender, "Check out the project's discussion on the My Digital Life forums")
                    Case "ESN"
                        PictureToolTip.SetToolTip(sender, "Consulte la discusión del proyecto en los foros de My Digital Life")
                    Case "FRA"
                        PictureToolTip.SetToolTip(sender, "Consultez les discussions sur le projet sur les forums de My Digital Life")
                    Case "PTB", "PTG"
                        PictureToolTip.SetToolTip(sender, "Consulte o debate sobre o projeto nos fóruns do My Digital Life")
                End Select
            Case 1
                PictureToolTip.SetToolTip(sender, "Check out the project's discussion on the My Digital Life forums")
            Case 2
                PictureToolTip.SetToolTip(sender, "Consulte la discusión del proyecto en los foros de My Digital Life")
            Case 3
                PictureToolTip.SetToolTip(sender, "Consultez les discussions sur le projet sur les forums de My Digital Life")
            Case 4
                PictureToolTip.SetToolTip(sender, "Consulte o debate sobre o projeto nos fóruns do My Digital Life")
        End Select
    End Sub

    Private Sub PictureBox5_MouseHover(sender As Object, e As EventArgs) Handles PictureBox5.MouseHover
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PictureToolTip.SetToolTip(sender, "Check out the Gitter chat (powered by Matrix)")
                    Case "ESN"
                        PictureToolTip.SetToolTip(sender, "Consulte el chat de Gitter (potenciado por Matrix)")
                    Case "FRA"
                        PictureToolTip.SetToolTip(sender, "Consultez le chat sur Gitter (powered by Matrix)")
                    Case "PTB", "PTG"
                        PictureToolTip.SetToolTip(sender, "Consulte o chat do Gitter (com tecnologia Matrix)")
                End Select
            Case 1
                PictureToolTip.SetToolTip(sender, "Check out the Gitter chat (powered by Matrix)")
            Case 2
                PictureToolTip.SetToolTip(sender, "Consulte el chat de Gitter (potenciado por Matrix)")
            Case 3
                PictureToolTip.SetToolTip(sender, "Consultez le chat sur Gitter (powered by Matrix)")
            Case 4
                PictureToolTip.SetToolTip(sender, "Consulte o chat do Gitter (com tecnologia Matrix)")
        End Select
    End Sub

    Private Sub UpdCheckBtn_Click(sender As Object, e As EventArgs) Handles UpdCheckBtn.Click
        If File.Exists(Application.StartupPath & "\update.exe") Then File.Delete(Application.StartupPath & "\update.exe")
        Try
            Using client As New WebClient()
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                client.DownloadFile("https://github.com/CodingWonders/DISMTools/raw/" & MainForm.dtBranch & "/Updater/DISMTools-UCS/update-bin/update.exe", Application.StartupPath & "\update.exe")
            End Using
        Catch ex As WebException
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("We couldn't download the update checker. Reason:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, UpdCheckBtn.Text)
                        Case "ESN"
                            MsgBox("No pudimos descargar el comprobador de actualizaciones. Razón:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, UpdCheckBtn.Text)
                        Case "FRA"
                            MsgBox("Nous n'avons pas pu télécharger le vérificateur de mise à jour. Raison :" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, UpdCheckBtn.Text)
                        Case "PTB", "PTG"
                            MsgBox("Não foi possível descarregar o verificador de actualizações. Motivo:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, UpdCheckBtn.Text)
                    End Select
                Case 1
                    MsgBox("We couldn't download the update checker. Reason:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, UpdCheckBtn.Text)
                Case 2
                    MsgBox("No pudimos descargar el comprobador de actualizaciones. Razón:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, UpdCheckBtn.Text)
                Case 3
                    MsgBox("Nous n'avons pas pu télécharger le vérificateur de mise à jour. Raison :" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, UpdCheckBtn.Text)
                Case 4
                    MsgBox("Não foi possível descarregar o verificador de actualizações. Motivo:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, UpdCheckBtn.Text)
            End Select
            Exit Sub
        End Try
        If File.Exists(Application.StartupPath & "\update.exe") Then Process.Start(Application.StartupPath & "\update.exe", "/" & MainForm.dtBranch & " /pid=" & Process.GetCurrentProcess().Id)
    End Sub
End Class
