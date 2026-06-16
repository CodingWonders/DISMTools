Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports System.Net
Imports Microsoft.Win32

Public Class ISOCreator

    Dim ImageInfoCollection As DismImageInfoCollection
    Dim ISOMsg As String = ""
    Dim progressMessages() As String = New String(2) {"Status", "Creating ISO file. This can take some time. Please wait...", "The ISO file has been created"}
    Dim success As Boolean
    Dim architectures() As String = New String(2) {"x86", "amd64", "arm64"}
    Dim adkDownloadLocations() As String = New String(1) {"https://download.microsoft.com/download/615540bc-be0b-433a-b91b-1f2b0642bb24/adk/adksetup.exe", "https://download.microsoft.com/download/2472e9a0-7c74-4ffd-a3e4-27ed1fa30d30/adkwinpeaddons/adkwinpesetup.exe"}
    Dim adkDownloadSuccess As Boolean

    Private Sub ISOCreator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        progressMessages(0) = "Status"
                        progressMessages(1) = "Creating ISO file. This can take some time. Please wait..."
                        progressMessages(2) = "The ISO file has been created"
                        Text = "Create an ISO file"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "The ISO file creation wizard lets you quickly create a disc image file that you can use to test the changes made to your Windows image. A custom Preinstallation Environment (PE) will be created. This environment will automatically perform disk configuration and apply the image you specify here."
                        Label3.Text = "Once you're ready, click the Create button."
                        Label4.Text = "Image file to add to ISO file:"
                        Label6.Text = "Architecture:"
                        Label7.Text = "Target ISO location:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "You can do other things while the ISO is being created. Come back here anytime for an updated status."
                        Button1.Text = "Browse..."
                        Button2.Text = "Pick..."
                        Button3.Text = "Browse..."
                        Button4.Text = "Use mounted image"
                        Button5.Text = "Browse..."
                        OK_Button.Text = "Create"
                        Cancel_Button.Text = "Cancel"
                        GroupBox1.Text = "Options"
                        GroupBox2.Text = "Progress"
                        LinkLabel1.Text = "Download the Windows ADK"
                        ColumnHeader2.Text = "Image Name"
                        ColumnHeader3.Text = "Image Description"
                        ColumnHeader4.Text = "Image Version"
                        ColumnHeader5.Text = "Image Architecture"
                        CheckBox1.Text = "Unattended answer file:"
                        CheckBox2.Text = "Copy to Ventoy drives"
                        CheckBox3.Text = "Use newly-signed boot binaries"
                    Case "ESN"
                        progressMessages(0) = "Estado"
                        progressMessages(1) = "Creando archivo ISO. Esto puede llevar algo de tiempo. Espere..."
                        progressMessages(2) = "El archivo ISO ha sido creado"
                        Text = "Crear un archivo ISO"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "El asistente de creación de archivos ISO le permite crear un archivo de imagen de disco rápidamente y que puede utilizar para probar los cambios hechos a su imagen de Windows. Un Entorno de Preinstalación (PE) personalizado será creado. Este entorno realizará configuración del disco automáticamente y aplicará la imagen que especifique aquí."
                        Label3.Text = "Cuando esté listo, haga clic en Crear."
                        Label4.Text = "Archivo de imagen a añadir al archivo ISO:"
                        Label6.Text = "Arquitectura:"
                        Label7.Text = "Ubicación del archivo ISO de destino:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "Puede hacer otras cosas mientras se crea el archivo ISO. Vuelva aquí para ver un estado actualizado."
                        Button1.Text = "Examinar..."
                        Button2.Text = "Escoger..."
                        Button3.Text = "Examinar..."
                        Button4.Text = "Usar imagen montada"
                        Button5.Text = "Examinar..."
                        OK_Button.Text = "Crear"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox1.Text = "Opciones"
                        GroupBox2.Text = "Progreso"
                        LinkLabel1.Text = "Descargar el ADK de Windows"
                        ColumnHeader2.Text = "Nombre de la imagen"
                        ColumnHeader3.Text = "Descripción de la imagen"
                        ColumnHeader4.Text = "Versión"
                        ColumnHeader5.Text = "Arquitectura"
                        CheckBox1.Text = "Archivo de respuesta:"
                        CheckBox2.Text = "Copiar a discos Ventoy"
                        CheckBox3.Text = "Utilizar archivos de arranque firmados con nuevos certificados"
                    Case "FRA"
                        progressMessages(0) = "Statut"
                        progressMessages(1) = "Création du fichier ISO en cours. Cela peut prendre un certain temps. Veuillez patienter..."
                        progressMessages(2) = "Le fichier ISO a été créé"
                        Text = "Créer un fichier ISO"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "L'assistant de création de fichier ISO vous permet de créer rapidement un fichier image de disque que vous pouvez utiliser pour tester les modifications apportées à votre image Windows. Un environnement de préinstallation (PE) personnalisé sera créé. Cet environnement effectuera automatiquement la configuration du disque et appliquera l'image que vous spécifiez ici."
                        Label3.Text = "Lorsque vous êtes prêt, cliquez sur le bouton Créer."
                        Label4.Text = "Fichier image à ajouter au fichier ISO :"
                        Label6.Text = "Architecture :"
                        Label7.Text = "Emplacement ISO cible :"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "Vous pouvez faire d'autres choses pendant la création de l'ISO. Revenez ici à tout moment pour obtenir une mise à jour de l'état."
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Choisir..."
                        Button3.Text = "Parcourir..."
                        Button4.Text = "Utiliser une image montée"
                        Button5.Text = "Parcourir..."
                        OK_Button.Text = "Créer"
                        Cancel_Button.Text = "Annuler"
                        GroupBox1.Text = "Paramètres"
                        GroupBox2.Text = "Progrès"
                        LinkLabel1.Text = "Télécharger l'ADK Windows"
                        ColumnHeader2.Text = "Nom de l'image"
                        ColumnHeader3.Text = "Description de l'image"
                        ColumnHeader4.Text = "Version"
                        ColumnHeader5.Text = "Architecture"
                        CheckBox1.Text = "Fichier de réponse :"
                        CheckBox2.Text = "Copier sur les lecteurs Ventoy"
                        CheckBox3.Text = "Utiliser des binaires de démarrage nouvellement signés"
                    Case "PTB", "PTG"
                        progressMessages(0) = "Estado"
                        progressMessages(1) = "A criar ficheiro ISO. Isto pode demorar algum tempo. Por favor, aguarde..."
                        progressMessages(2) = "O ficheiro ISO foi criado"
                        Text = "Criar um ficheiro ISO"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "O assistente de criação de ficheiros ISO permite-lhe criar rapidamente um ficheiro de imagem de disco que pode utilizar para testar as alterações efectuadas à sua imagem do Windows. Será criado um ambiente de pré-instalação (PE) personalizado. Este ambiente irá efetuar automaticamente a configuração do disco e aplicar a imagem que especificar aqui."
                        Label3.Text = "Quando estiver pronto, clique no botão Criar."
                        Label4.Text = "Ficheiro de imagem a adicionar ao ficheiro ISO:"
                        Label6.Text = "Arquitetura:"
                        Label7.Text = "Localização ISO de destino:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "Pode fazer outras coisas enquanto o ISO está a ser criado. Volte aqui em qualquer altura para obter um estado atualizado."
                        Button1.Text = "Procurar..."
                        Button2.Text = "Escolher..."
                        Button3.Text = "Procurar..."
                        Button4.Text = "Utilizar imagem montada"
                        Button5.Text = "Procurar..."
                        OK_Button.Text = "Criar"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox1.Text = "Configurações"
                        GroupBox2.Text = "Progresso"
                        LinkLabel1.Text = "Baixar o Windows ADK"
                        ColumnHeader2.Text = "Nome da imagem"
                        ColumnHeader3.Text = "Descrição da imagem"
                        ColumnHeader4.Text = "Versão"
                        ColumnHeader5.Text = "Arquitetura"
                        CheckBox1.Text = "Ficheiro de resposta:"
                        CheckBox2.Text = "Copiar para unidades Ventoy"
                        CheckBox3.Text = "Utilizar binários de arranque com assinatura recente"
                    Case "ITA"
                        progressMessages(0) = "Stato"
                        progressMessages(1) = "Creazione del file ISO. L'operazione può richiedere del tempo. Attendere..."
                        progressMessages(2) = "Il file ISO è stato creato"
                        Text = "Creare un file ISO"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "La creazione guidata del file ISO consente di creare rapidamente un file immagine del disco da utilizzare per testare le modifiche apportate all'immagine di Windows. Verrà creato un ambiente di preinstallazione (PE) personalizzato. Questo ambiente eseguirà automaticamente la configurazione del disco e applicherà l'immagine specificata qui."
                        Label3.Text = "Una volta pronti, fare clic sul pulsante Crea"
                        Label4.Text = "File immagine da aggiungere al file ISO:"
                        Label6.Text = "Architettura:"
                        Label7.Text = "Posizione ISO di destinazione:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "È possibile fare altre cose mentre la ISO viene creata. Tornare qui in qualsiasi momento per uno stato aggiornato"
                        Button1.Text = "Sfoglia..."
                        Button2.Text = "Scegli..."
                        Button3.Text = "Sfoglia..."
                        Button4.Text = "Usa immagine montata"
                        Button5.Text = "Sfoglia..."
                        OK_Button.Text = "Crea"
                        Cancel_Button.Text = "Annulla"
                        GroupBox1.Text = "Opzioni"
                        GroupBox2.Text = "Avanzamento"
                        LinkLabel1.Text = "Scarica l'ADK di Windows"
                        ColumnHeader2.Text = "Nome dell'immagine"
                        ColumnHeader3.Text = "Descrizione dell'immagine"
                        ColumnHeader4.Text = "Versione"
                        ColumnHeader5.Text = "Architettura"
                        CheckBox1.Text = "File di risposta:"
                        CheckBox2.Text = "Copia su unità Ventoy"
                        CheckBox3.Text = "Utilizzare binari di avvio con firma recente"
                End Select
            Case 1
                progressMessages(0) = "Status"
                progressMessages(1) = "Creating ISO file. This can take some time. Please wait..."
                progressMessages(2) = "The ISO file has been created"
                Text = "Create an ISO file"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "The ISO file creation wizard lets you quickly create a disc image file that you can use to test the changes made to your Windows image. A custom Preinstallation Environment (PE) will be created. This environment will automatically perform disk configuration and apply the image you specify here."
                Label3.Text = "Once you're ready, click the Create button."
                Label4.Text = "Image file to add to ISO file:"
                Label6.Text = "Architecture:"
                Label7.Text = "Target ISO location:"
                Label8.Text = progressMessages(0)
                Label9.Text = "You can do other things while the ISO is being created. Come back here anytime for an updated status."
                Button1.Text = "Browse..."
                Button2.Text = "Pick..."
                Button3.Text = "Browse..."
                Button4.Text = "Use mounted image"
                Button5.Text = "Browse..."
                OK_Button.Text = "Create"
                Cancel_Button.Text = "Cancel"
                GroupBox1.Text = "Options"
                GroupBox2.Text = "Progress"
                LinkLabel1.Text = "Download the Windows ADK"
                ColumnHeader2.Text = "Image Name"
                ColumnHeader3.Text = "Image Description"
                ColumnHeader4.Text = "Image Version"
                ColumnHeader5.Text = "Image Architecture"
                CheckBox1.Text = "Unattended answer file:"
                CheckBox2.Text = "Copy to Ventoy drives"
                CheckBox3.Text = "Use newly-signed boot binaries"
            Case 2
                progressMessages(0) = "Estado"
                progressMessages(1) = "Creando archivo ISO. Esto puede llevar algo de tiempo. Espere..."
                progressMessages(2) = "El archivo ISO ha sido creado"
                Text = "Crear un archivo ISO"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "El asistente de creación de archivos ISO le permite crear un archivo de imagen de disco rápidamente y que puede utilizar para probar los cambios hechos a su imagen de Windows. Un Entorno de Preinstalación (PE) personalizado será creado. Este entorno realizará configuración del disco automáticamente y aplicará la imagen que especifique aquí."
                Label3.Text = "Cuando esté listo, haga clic en Crear."
                Label4.Text = "Archivo de imagen a añadir al archivo ISO:"
                Label6.Text = "Arquitectura:"
                Label7.Text = "Ubicación del archivo ISO de destino:"
                Label8.Text = progressMessages(0)
                Label9.Text = "Puede hacer otras cosas mientras se crea el archivo ISO. Vuelva aquí para ver un estado actualizado."
                Button1.Text = "Examinar..."
                Button2.Text = "Escoger..."
                Button3.Text = "Examinar..."
                Button4.Text = "Usar imagen montada"
                Button5.Text = "Examinar..."
                OK_Button.Text = "Crear"
                Cancel_Button.Text = "Cancelar"
                GroupBox1.Text = "Opciones"
                GroupBox2.Text = "Progreso"
                LinkLabel1.Text = "Descargar el ADK de Windows"
                ColumnHeader2.Text = "Nombre de la imagen"
                ColumnHeader3.Text = "Descripción de la imagen"
                ColumnHeader4.Text = "Versión"
                ColumnHeader5.Text = "Arquitectura"
                CheckBox1.Text = "Archivo de respuesta:"
                CheckBox2.Text = "Copiar a discos Ventoy"
                CheckBox3.Text = "Utilizar archivos de arranque firmados con nuevos certificados"
            Case 3
                progressMessages(0) = "Statut"
                progressMessages(1) = "Création du fichier ISO en cours. Cela peut prendre un certain temps. Veuillez patienter..."
                progressMessages(2) = "Le fichier ISO a été créé"
                Text = "Créer un fichier ISO"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "L'assistant de création de fichier ISO vous permet de créer rapidement un fichier image de disque que vous pouvez utiliser pour tester les modifications apportées à votre image Windows. Un environnement de préinstallation (PE) personnalisé sera créé. Cet environnement effectuera automatiquement la configuration du disque et appliquera l'image que vous spécifiez ici."
                Label3.Text = "Lorsque vous êtes prêt, cliquez sur le bouton Créer."
                Label4.Text = "Fichier image à ajouter au fichier ISO :"
                Label6.Text = "Architecture :"
                Label7.Text = "Emplacement ISO cible :"
                Label8.Text = progressMessages(0)
                Label9.Text = "Vous pouvez faire d'autres choses pendant la création de l'ISO. Revenez ici à tout moment pour obtenir une mise à jour de l'état."
                Button1.Text = "Parcourir..."
                Button2.Text = "Choisir..."
                Button3.Text = "Parcourir..."
                Button4.Text = "Utiliser une image montée"
                Button5.Text = "Parcourir..."
                OK_Button.Text = "Créer"
                Cancel_Button.Text = "Annuler"
                GroupBox1.Text = "Paramètres"
                GroupBox2.Text = "Progrès"
                LinkLabel1.Text = "Télécharger l'ADK Windows"
                ColumnHeader2.Text = "Nom de l'image"
                ColumnHeader3.Text = "Description de l'image"
                ColumnHeader4.Text = "Version"
                ColumnHeader5.Text = "Architecture"
                CheckBox1.Text = "Fichier de réponse :"
                CheckBox2.Text = "Copier sur les lecteurs Ventoy"
                CheckBox3.Text = "Utiliser des binaires de démarrage nouvellement signés"
            Case 4
                progressMessages(0) = "Estado"
                progressMessages(1) = "A criar ficheiro ISO. Isto pode demorar algum tempo. Por favor, aguarde..."
                progressMessages(2) = "O ficheiro ISO foi criado"
                Text = "Criar um ficheiro ISO"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "O assistente de criação de ficheiros ISO permite-lhe criar rapidamente um ficheiro de imagem de disco que pode utilizar para testar as alterações efectuadas à sua imagem do Windows. Será criado um ambiente de pré-instalação (PE) personalizado. Este ambiente irá efetuar automaticamente a configuração do disco e aplicar a imagem que especificar aqui."
                Label3.Text = "Quando estiver pronto, clique no botão Criar."
                Label4.Text = "Ficheiro de imagem a adicionar ao ficheiro ISO:"
                Label6.Text = "Arquitetura:"
                Label7.Text = "Localização ISO de destino:"
                Label8.Text = progressMessages(0)
                Label9.Text = "Pode fazer outras coisas enquanto o ISO está a ser criado. Volte aqui em qualquer altura para obter um estado atualizado."
                Button1.Text = "Procurar..."
                Button2.Text = "Escolher..."
                Button3.Text = "Procurar..."
                Button4.Text = "Utilizar imagem montada"
                Button5.Text = "Procurar..."
                OK_Button.Text = "Criar"
                Cancel_Button.Text = "Cancelar"
                GroupBox1.Text = "Configurações"
                GroupBox2.Text = "Progresso"
                LinkLabel1.Text = "Baixar o Windows ADK"
                ColumnHeader2.Text = "Nome da imagem"
                ColumnHeader3.Text = "Descrição da imagem"
                ColumnHeader4.Text = "Versão"
                ColumnHeader5.Text = "Arquitetura"
                CheckBox1.Text = "Ficheiro de resposta:"
                CheckBox2.Text = "Copiar para unidades Ventoy"
                CheckBox3.Text = "Utilizar binários de arranque com assinatura recente"
            Case 5
                progressMessages(0) = "Stato"
                progressMessages(1) = "Creazione del file ISO. L'operazione può richiedere del tempo. Attendere..."
                progressMessages(2) = "Il file ISO è stato creato"
                Text = "Creare un file ISO"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "La creazione guidata del file ISO consente di creare rapidamente un file immagine del disco da utilizzare per testare le modifiche apportate all'immagine di Windows. Verrà creato un ambiente di preinstallazione (PE) personalizzato. Questo ambiente eseguirà automaticamente la configurazione del disco e applicherà l'immagine specificata qui."
                Label3.Text = "Una volta pronti, fare clic sul pulsante Crea"
                Label4.Text = "File immagine da aggiungere al file ISO:"
                Label6.Text = "Architettura:"
                Label7.Text = "Posizione ISO di destinazione:"
                Label8.Text = progressMessages(0)
                Label9.Text = "È possibile fare altre cose mentre la ISO viene creata. Tornare qui in qualsiasi momento per uno stato aggiornato"
                Button1.Text = "Sfoglia..."
                Button2.Text = "Scegli..."
                Button3.Text = "Sfoglia..."
                Button4.Text = "Usa immagine montata"
                Button5.Text = "Sfoglia..."
                OK_Button.Text = "Crea"
                Cancel_Button.Text = "Annulla"
                GroupBox1.Text = "Opzioni"
                GroupBox2.Text = "Avanzamento"
                LinkLabel1.Text = "Scarica l'ADK di Windows"
                ColumnHeader2.Text = "Nome dell'immagine"
                ColumnHeader3.Text = "Descrizione dell'immagine"
                ColumnHeader4.Text = "Versione"
                ColumnHeader5.Text = "Architettura"
                CheckBox1.Text = "File di risposta:"
                CheckBox2.Text = "Copia su unità Ventoy"
                CheckBox3.Text = "Utilizzare binari di avvio con firma recente"
        End Select
        ImageTaskHeader1.SetColors()
        ImageTaskHeader1.HideWindowTitle(Me.Handle)
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox3.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        GroupBox2.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        If MainForm.SourceImg = "N/A" Or Not File.Exists(MainForm.SourceImg) Or MainForm.OnlineManagement Or MainForm.OfflineManagement Then
            Button4.Enabled = False
        Else
            Button4.Enabled = True
        End If
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
        Dim bm As New Bitmap(ListView1.ClientSize.Width, ListView1.ClientSize.Height)
        Graphics.FromImage(bm).Clear(ListView1.BackColor)
        ListView1.BackgroundImage = bm

        ' Declare path constant for Windows ADK
        Dim ADKPath As String = Path.Combine(If(Environment.Is64BitOperatingSystem,
                                                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)), "Windows Kits", "10",
                                                "Assessment and Deployment Kit")
        ' Check ADK status
        If Not Directory.Exists(ADKPath) Then
            DynaLog.LogMessage("ADK installation directory " & Quote & ADKPath & Quote & " is not found in this system. Either it has not been installed or it has been installed somewhere else.")
            If MsgBox("The Windows ADK was not found on your system. Do you want DISMTools to download and install the latest one for you? Note that you'll need around 4 GB on your system.", vbYesNo + vbQuestion, "") = MsgBoxResult.Yes Then
                Visible = True
                ADKDownloaderBW.RunWorkerAsync()
                Do Until Not ADKDownloaderBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(100)
                Loop
                If Not adkDownloadSuccess Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Process.Start("https://learn.microsoft.com/en-us/windows-hardware/get-started/adk-install")
                                Case "ESN"
                                    Process.Start("https://learn.microsoft.com/es-es/windows-hardware/get-started/adk-install")
                                Case "FRA"
                                    Process.Start("https://learn.microsoft.com/fr-fr/windows-hardware/get-started/adk-install")
                                Case "PTB", "PTG"
                                    Process.Start("https://learn.microsoft.com/pt-pt/windows-hardware/get-started/adk-install")
                                Case "ITA"
                                    Process.Start("https://learn.microsoft.com/it-it/windows-hardware/get-started/adk-install")
                            End Select
                        Case 1
                            Process.Start("https://learn.microsoft.com/en-us/windows-hardware/get-started/adk-install")
                        Case 2
                            Process.Start("https://learn.microsoft.com/es-es/windows-hardware/get-started/adk-install")
                        Case 3
                            Process.Start("https://learn.microsoft.com/fr-fr/windows-hardware/get-started/adk-install")
                        Case 4
                            Process.Start("https://learn.microsoft.com/pt-pt/windows-hardware/get-started/adk-install")
                        Case 5
                            Process.Start("https://learn.microsoft.com/it-it/windows-hardware/get-started/adk-install")
                    End Select
                    Close()
                End If
            Else
                Close()
            End If

        End If

        ' Restore combobox architecture items
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(architectures)
        ' Remove architectures incompatible with the system ADK
        For Each architecture In architectures
            Dim WimPath As String = Path.Combine(ADKPath, "Windows Preinstallation Environment", architecture, "en-us", "winpe.wim")
            DynaLog.LogMessage("Testing if architecture " & architecture & " is supported by the ADK installed in this system...")
            If Not File.Exists(WimPath) Then
                DynaLog.LogMessage("- Windows PE WIM " & Quote & WimPath & Quote & " is not present. Removing architecture option...")
                ComboBox1.Items.Remove(architecture)
            End If
        Next
        ' If we are left with no architectures, add them back
        If ComboBox1.Items.Count = 0 Then
            DynaLog.LogMessage("For some reason we excluded all of them. This could be because of incorrect detections. Adding back...")
            ComboBox1.Items.AddRange(architectures)
        End If
        ComboBox1.SelectedIndex = 0

        ' Apply PE Helper settings
        DynaLog.LogMessage("Getting ISO creation settings...")
        DynaLog.LogMessage("- Unattended answer file (overrides existing answer files in an image): " & MainForm.PEHelper_UnattendedFile)
        DynaLog.LogMessage("- Copy to Ventoy? " & MainForm.PEHelper_CopyToVentoy)
        DynaLog.LogMessage("- Use new EFI boot binaries? " & MainForm.PEHelper_Use2023EFI)
        DynaLog.LogMessage("- Include System Drivers? " & MainForm.PEHelper_IncludeSysDrvs)

        If MainForm.PEHelper_UnattendedFile <> "" AndAlso File.Exists(MainForm.PEHelper_UnattendedFile) Then
            DynaLog.LogMessage("Unattended answer file has been specified and exists. Using it...")
            CheckBox1.Checked = True
            TextBox4.Text = MainForm.PEHelper_UnattendedFile
        Else
            DynaLog.LogMessage("Either no answer file was specified or it was specified, but doesn't exist...")
            CheckBox1.Checked = False
            TextBox4.Text = ""
        End If
        CheckBox2.Checked = MainForm.PEHelper_CopyToVentoy
        CheckBox3.Checked = MainForm.PEHelper_Use2023EFI
        CheckBox4.Checked = MainForm.PEHelper_IncludeSysDrvs

        AddHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged

        ColumnHeader1.Width = WindowHelper.ScaleLogical(29)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(265)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(343)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(103)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(130)
    End Sub

    Private Sub DownloadADK()
        Try
            ProgressReporter.SetMessage("Preparing to download Assessment and Deployment Kit...")
            ADKDownloaderBW.ReportProgress(0)
            Dim FileNames As New List(Of String)
            For Each downloadLocation In adkDownloadLocations
                FileNames.Add(Path.GetFileName(downloadLocation))
                Dim current As Integer = adkDownloadLocations.ToList().IndexOf(downloadLocation)
                Dim count As Integer = adkDownloadLocations.Count
                ProgressReporter.SetMessage(String.Format("Downloading ADK component {0} of {1}...", current + 1, count))
                ADKDownloaderBW.ReportProgress(50 * (current / count))
                Using client As New WebClient()
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                    client.DownloadFile(downloadLocation, Path.Combine(Application.StartupPath, Path.GetFileName(downloadLocation)))
                End Using
            Next
            Dim currentProgress As Integer = 50
            For Each FileName In FileNames
                Dim current As Integer = FileNames.IndexOf(FileName)
                Dim count As Integer = FileNames.Count
                ProgressReporter.SetMessage(String.Format("Installing ADK component {0} of {1}...", current + 1, count))
                ADKDownloaderBW.ReportProgress(currentProgress)
                Dim InstallerProcess As New Process()
                InstallerProcess.StartInfo.WorkingDirectory = Application.StartupPath
                If File.Exists(Path.Combine(Application.StartupPath, FileName)) Then
                    InstallerProcess.StartInfo.FileName = FileName
                    ' Guess command-line options. Source of necessary options comes from remediation script Microsoft published
                    ' during the CrowdStrike incident.
                    InstallerProcess.StartInfo.Arguments = String.Format("/features {0} /q /ceip off",
                                                                         If(FileName.Contains("winpe"),
                                                                            "OptionId.WindowsPreinstallationEnvironment",
                                                                            "OptionId.DeploymentTools")
                                                                        )
                    InstallerProcess.Start()
                    InstallerProcess.WaitForExit()
                    If Not InstallerProcess.ExitCode = 0 Then
                        Throw New Exception("One of the ADK component installers has finished with exit code " & InstallerProcess.ExitCode)
                    End If
                End If
                currentProgress += 25
            Next
            Try
                ProgressReporter.SetMessage("Deleting temporary files...")
                ADKDownloaderBW.ReportProgress(100)
                For Each FileName In FileNames
                    File.Delete(Path.Combine(Application.StartupPath, FileName))
                Next
            Catch ex As Exception

            End Try
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        DynaLog.LogMessage("Source image file to test: " & Quote & OpenFileDialog1.FileName & Quote)
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Sub GetImageInfo(ImageFile As String)
        DynaLog.LogMessage("Image file to get information about: " & Quote & ImageFile & Quote)
        DynaLog.LogMessage("Checking if mounted image detector is busy...")
        ListView1.Items.Clear()
        MainForm.StopMountedImageDetector()
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            ImageInfoCollection = DismApi.GetImageInfo(ImageFile)
            DynaLog.LogMessage("Information collection count: " & ImageInfoCollection.Count)
            If ImageInfoCollection.Count > 0 Then
                DynaLog.LogMessage("This file has images. Updating lists...")
                ListView1.Items.AddRange(ImageInfoCollection.Select(Function(ImageInfo) New ListViewItem(New String() {(ImageInfoCollection.IndexOf(ImageInfo) + 1),
                                                                                                                       imageinfo.ImageName,
                                                                                                                       imageinfo.ImageDescription,
                                                                                                                       imageinfo.ProductVersion.ToString(),
                                                                                                                       casters.CastDismArchitecture(ImageInfo.Architecture)})).ToArray())
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Could not gather information of this image file. Reason:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "ESN"
                            msg = "No pudimos obtener información de este archivo de imagen. Razón:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "FRA"
                            msg = "Impossible de recueillir des informations sur ce fichier de l'image. Raison :" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "PTB", "PTG"
                            msg = "Não foi possível recolher informações sobre este ficheiro de imagem. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "ITA"
                            msg = "Impossibile raccogliere informazioni sull'immagine. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                    End Select
                Case 1
                    msg = "Could not gather information of this image file. Reason:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 2
                    msg = "No pudimos obtener información de este archivo de imagen. Razón:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 3
                    msg = "Impossible de recueillir des informations sur ce fichier de l'image. Raison :" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 4
                    msg = "Não foi possível recolher informações sobre este ficheiro de imagem. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 5
                    msg = "Impossibile raccogliere informazioni sull'immagine. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
        Finally
            DynaLog.LogMessage("Shutting down API...")
            Try
                DismApi.Shutdown()
            Catch ex As Exception
                ' Don't do anything
            End Try
        End Try
        DynaLog.LogMessage("This process has finished.")
        MainForm.StartMountedImageDetector()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        DynaLog.LogMessage("Specified destination: " & Quote & SaveFileDialog1.FileName & Quote)
        TextBox3.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Checking provided information...")
        DynaLog.LogMessage("- Source image to add to ISO file: " & Quote & TextBox1.Text & Quote)
        DynaLog.LogMessage("- Destination ISO file: " & Quote & TextBox3.Text & Quote)
        If TextBox1.Text = "" OrElse Not File.Exists(TextBox1.Text) Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            ISOMsg = "Either the source image file does not exist or you haven't provided any image file. Please specify a valid image file and try again."
                        Case "ESN"
                            ISOMsg = "El archivo de imagen de origen no existe o no ha especificado un archivo. Especifique un archivo de imagen válido e inténtelo de nuevo."
                        Case "FRA"
                            ISOMsg = "Soit le fichier image source n'existe pas, soit vous n'avez pas fourni de fichier image. Veuillez spécifier un fichier image valide et réessayer."
                        Case "PTB", "PTG"
                            ISOMsg = "Ou o ficheiro de imagem de origem não existe ou não forneceu qualquer ficheiro de imagem. Especifique um ficheiro de imagem válido e tente novamente."
                        Case "ITA"
                            ISOMsg = "Il file immagine di origine non esiste o non è stato fornito alcun file immagine. Specificare un file immagine valido e riprovare."
                    End Select
                Case 1
                    ISOMsg = "Either the source image file does not exist or you haven't provided any image file. Please specify a valid image file and try again."
                Case 2
                    ISOMsg = "El archivo de imagen de origen no existe o no ha especificado un archivo. Especifique un archivo de imagen válido e inténtelo de nuevo."
                Case 3
                    ISOMsg = "Soit le fichier image source n'existe pas, soit vous n'avez pas fourni de fichier image. Veuillez spécifier un fichier image valide et réessayer."
                Case 4
                    ISOMsg = "Ou o ficheiro de imagem de origem não existe ou não forneceu qualquer ficheiro de imagem. Especifique um ficheiro de imagem válido e tente novamente."
                Case 5
                    ISOMsg = "Il file immagine di origine non esiste o non è stato fornito alcun file immagine. Specificare un file immagine valido e riprovare."
            End Select
            MsgBox(ISOMsg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If TextBox3.Text = "" Then
            If SaveFileDialog1.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                ISOMsg = "The target ISO hasn't been specified. Please specify a location for the ISO file and try again."
                            Case "ESN"
                                ISOMsg = "El archivo ISO de destino no se ha especificado. Especifique una ubicación para el archivo ISO e inténtelo de nuevo."
                            Case "FRA"
                                ISOMsg = "L'ISO cible n'a pas été spécifiée. Veuillez indiquer un emplacement pour le fichier ISO et réessayez."
                            Case "PTB", "PTG"
                                ISOMsg = "O ISO de destino não foi especificado. Especifique uma localização para o ficheiro ISO e tente novamente."
                            Case "ITA"
                                ISOMsg = "L'ISO di destinazione non è stata specificata. Specificare una posizione per il file ISO e riprovare."
                        End Select
                    Case 1
                        ISOMsg = "The target ISO hasn't been specified. Please specify a location for the ISO file and try again."
                    Case 2
                        ISOMsg = "El archivo ISO de destino no se ha especificado. Especifique una ubicación para el archivo ISO e inténtelo de nuevo."
                    Case 3
                        ISOMsg = "L'ISO cible n'a pas été spécifiée. Veuillez indiquer un emplacement pour le fichier ISO et réessayez."
                    Case 4
                        ISOMsg = "O ISO de destino não foi especificado. Especifique uma localização para o ficheiro ISO e tente novamente."
                    Case 5
                        ISOMsg = "L'ISO di destinazione non è stata specificata. Specificare una posizione per il file ISO e riprovare."
                End Select
                MsgBox(ISOMsg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Exit Sub
            End If
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ISOMsg = "Make sure that you have saved all your changes before continuing." & CrLf & CrLf & "If you have not done so, click No, save your image, and start the process again. You don't have to close this window." & CrLf & CrLf & "Do you want to create an ISO with this file?"
                    Case "ESN"
                        ISOMsg = "Asegúrese de que haya guardado todos sus cambios antes de continuar." & CrLf & CrLf & "Si no lo ha hecho, haga clic en No, guarde su imagen, y comience el proceso de nuevo. No tiene que cerrar esta ventana." & CrLf & CrLf & "¿Desea crear un archivo ISO con esta imagen?"
                    Case "FRA"
                        ISOMsg = "Assurez-vous d'avoir enregistré toutes vos modifications avant de continuer." & CrLf & CrLf & "Si vous ne l'avez pas fait, cliquez sur Non, enregistrez votre image et recommencez le processus. Vous n'êtes pas obligé de fermer cette fenêtre." & CrLf & CrLf & "Voulez-vous créer une ISO avec ce fichier ?"
                    Case "PTB", "PTG"
                        ISOMsg = "Certifique-se de que guardou todas as suas alterações antes de continuar." & CrLf & CrLf & "Se ainda não o fez, clique em Não, guarde a sua imagem e comece o processo novamente. Não é necessário fechar esta janela." & CrLf & CrLf & "Deseja criar uma ISO com este ficheiro?"
                    Case "ITA"
                        ISOMsg = "Assicurarsi di aver salvato tutte le modifiche prima di continuare." & CrLf & CrLf & "Se non lo si è fatto, fare clic su No, salvare l'immagine e ricominciare il processo. Non è necessario chiudere questa finestra." & CrLf & CrLf & "Si desidera creare una ISO con questo file?"
                End Select
            Case 1
                ISOMsg = "Make sure that you have saved all your changes before continuing." & CrLf & CrLf & "If you have not done so, click No, save your image, and start the process again. You don't have to close this window." & CrLf & CrLf & "Do you want to create an ISO with this file?"
            Case 2
                ISOMsg = "Asegúrese de que haya guardado todos sus cambios antes de continuar." & CrLf & CrLf & "Si no lo ha hecho, haga clic en No, guarde su imagen, y comience el proceso de nuevo. No tiene que cerrar esta ventana." & CrLf & CrLf & "¿Desea crear un archivo ISO con esta imagen?"
            Case 3
                ISOMsg = "Assurez-vous d'avoir enregistré toutes vos modifications avant de continuer." & CrLf & CrLf & "Si vous ne l'avez pas fait, cliquez sur Non, enregistrez votre image et recommencez le processus. Vous n'êtes pas obligé de fermer cette fenêtre." & CrLf & CrLf & "Voulez-vous créer une ISO avec ce fichier ?"
            Case 4
                ISOMsg = "Certifique-se de que guardou todas as suas alterações antes de continuar." & CrLf & CrLf & "Se ainda não o fez, clique em Não, guarde a sua imagem e comece o processo novamente. Não é necessário fechar esta janela." & CrLf & CrLf & "Deseja criar uma ISO com este ficheiro?"
            Case 5
                ISOMsg = "Assicurarsi di aver salvato tutte le modifiche prima di continuare." & CrLf & CrLf & "Se non lo si è fatto, fare clic su No, salvare l'immagine e ricominciare il processo. Non è necessario chiudere questa finestra." & CrLf & CrLf & "Si desidera creare una ISO con questo file?"
        End Select
        If MsgBox(ISOMsg, vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.No Then
            Exit Sub
        End If
        If File.Exists(TextBox3.Text) Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            ISOMsg = "The target ISO already exists. Do you want to replace it?"
                        Case "ESN"
                            ISOMsg = "El archivo ISO ya existe. ¿Desea reemplazarlo?"
                        Case "FRA"
                            ISOMsg = "L'ISO cible existe déjà. Voulez-vous la remplacer ?"
                        Case "PTB", "PTG"
                            ISOMsg = "O ISO de destino já existe. Deseja substituí-la?"
                        Case "ITA"
                            ISOMsg = "L'ISO di destinazione esiste già. Si desidera sostituirla?"
                    End Select
                Case 1
                    ISOMsg = "The target ISO already exists. Do you want to replace it?"
                Case 2
                    ISOMsg = "El archivo ISO ya existe. ¿Desea reemplazarlo?"
                Case 3
                    ISOMsg = "L'ISO cible existe déjà. Voulez-vous la remplacer ?"
                Case 4
                    ISOMsg = "O ISO de destino já existe. Deseja substituí-la?"
                Case 5
                    ISOMsg = "L'ISO di destinazione esiste già. Si desidera sostituirla?"
            End Select
            If MsgBox(ISOMsg, vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.Yes Then
                Try
                    File.Delete(TextBox3.Text)
                Catch ex As Exception
                    ' Could not delete ISO
                End Try
            Else
                Exit Sub
            End If
        End If
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
        GroupBox1.Enabled = False
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        BackgroundWorker1.ReportProgress(0)
        DynaLog.LogMessage("Starting PE Helper...")
        DynaLog.LogMessage("- Task: generate ISO")
        DynaLog.LogMessage("- Architecture: " & ComboBox1.SelectedItem)
        DynaLog.LogMessage("- Image file to test: " & Quote & TextBox1.Text & Quote)
        DynaLog.LogMessage("- Unattended answer file to try: " & Quote & TextBox4.Text & Quote)
        DynaLog.LogMessage("- Destination ISO file: " & Quote & TextBox3.Text & Quote)
        DynaLog.LogMessage("- Copy the ISO file to Ventoy drives afterwards? " & If(CheckBox2.Checked, "Yes", "No"))
        DynaLog.LogMessage("- Use boot binaries signed with Windows UEFI CA 2023? " & If(CheckBox3.Checked, "Yes", "No"))
        DynaLog.LogMessage("- Include system essential drivers (SCSI adapters/network controllers)? " & If(CheckBox4.Checked, "Yes", "No"))
        Dim ISOCreator As New Process()
        ISOCreator.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        ISOCreator.StartInfo.WorkingDirectory = Application.StartupPath & "\bin\extps1\PE_Helper"
        ' Disable the unattended answer file thing if not willing to use
        Dim unattFile As String = TextBox4.Text
        If Not CheckBox1.Checked Then
            unattFile = ""
        End If

        ' get build time to show on watermark
        Try
            Dim buildTime As String = BuildGetter.RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm")
            File.WriteAllText(Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "version"), buildTime)
        Catch ex As Exception

        End Try

        ISOCreator.StartInfo.Arguments = "-noprofile -nologo -executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\extps1\PE_Helper\PE_Helper.ps1" & Quote & " -cmd StartPEGen -arch " & ComboBox1.SelectedItem & " -imgFile " & Quote & TextBox1.Text & Quote & " -isoPath " & Quote & TextBox3.Text & Quote & " -unattendFile " & Quote & unattFile & Quote & If(CheckBox2.Checked, " -copyToVentoy", "") & If(CheckBox3.Checked, " -bootex", "") & If(CheckBox4.Checked, " -includeSysDrivers", "")
        ISOCreator.Start()
        ISOCreator.WaitForExit()
        DynaLog.LogMessage("The PE Helper process finished with exit code " & Hex(ISOCreator.ExitCode))
        success = (ISOCreator.ExitCode = 0)
        BackgroundWorker1.ReportProgress(100)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        IdlePanel.Visible = False
        ISOProgressPanel.Visible = True
        If e.ProgressPercentage < 100 Then
            WindowHelper.DisableCloseCapability(Handle)
            Label8.Text = progressMessages(1)
            ProgressBar1.Style = ProgressBarStyle.Marquee
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.Indeterminate, MainForm.Handle)
        Else
            WindowHelper.EnableCloseCapability(Handle)
            If success Then Label8.Text = progressMessages(2)
            ProgressBar1.Style = ProgressBarStyle.Blocks
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.None, MainForm.Handle)
        End If
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        DynaLog.LogMessage("The PE Helper has finished.")
        DynaLog.LogMessage("- Did it succeed? " & If(success, "Yes", "No"))
        Dim msg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg = If(success, "The ISO file has been created successfully", "Failed to create the ISO file")
                    Case "ESN"
                        msg = If(success, "El archivo ISO ha sido creado satisfactoriamente", "No pudimos crear el archivo ISO")
                    Case "FRA"
                        msg = If(success, "Le fichier ISO a été créé avec succès", "Le processus de création de l'ISO a échoué")
                    Case "PTB", "PTG"
                        msg = If(success, "O ficheiro ISO foi criado com êxito", "O processo de criação do ISO falhou")
                    Case "ITA"
                        msg = If(success, "Il file ISO è stato creato con successo", "La creazione del file ISO non è riuscita")
                End Select
            Case 1
                msg = If(success, "The ISO file has been created successfully", "Failed to create the ISO file")
            Case 2
                msg = If(success, "El archivo ISO ha sido creado satisfactoriamente", "No pudimos crear el archivo ISO")
            Case 3
                msg = If(success, "Le fichier ISO a été créé avec succès", "Le processus de création de l'ISO a échoué")
            Case 4
                msg = If(success, "O ficheiro ISO foi criado com êxito", "O processo de criação do ISO falhou")
            Case 5
                msg = If(success, "Il file ISO è stato creato con successo", "La creazione del file ISO non è riuscita")
        End Select
        WindowHelper.DisplayNotificationBalloon(If(success, ToolTipIcon.Info, ToolTipIcon.Warning), ImageTaskHeader1.ItemText, msg)
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
        GroupBox1.Enabled = True
        IdlePanel.Visible = True
        ISOProgressPanel.Visible = False
    End Sub

    Private Sub ISOCreator_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If BackgroundWorker1.IsBusy Then
            DynaLog.LogMessage("The PE Helper is busy. Cancelling exit...")
            e.Cancel = True
            Beep()
            Exit Sub
        End If
        DynaLog.LogMessage("Saving settings...")
        If CheckBox1.Checked Then
            MainForm.PEHelper_UnattendedFile = TextBox4.Text
        Else
            MainForm.PEHelper_UnattendedFile = ""
        End If
        MainForm.PEHelper_CopyToVentoy = CheckBox2.Checked
        MainForm.PEHelper_Use2023EFI = CheckBox3.Checked
        MainForm.PEHelper_IncludeSysDrvs = CheckBox4.Checked

        Dim customPolicyPath As String = Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "files", "CustomPolicy.reg")
        If File.Exists(customPolicyPath) Then
            Try
                File.Delete(customPolicyPath)
            Catch ex As Exception

            End Try
        End If

        RemoveHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim selectedImage As WindowsImage = PopupMountedImagePicker.PickImage(".wim")
        If selectedImage IsNot Nothing Then
            DynaLog.LogMessage("Selected image: " & selectedImage.ImageFile)
            TextBox1.Text = selectedImage.ImageFile
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("The specified file exists. Getting information...")
            GetImageInfo(TextBox1.Text)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Process.Start("https://learn.microsoft.com/en-us/windows-hardware/get-started/adk-install")
                    Case "ESN"
                        Process.Start("https://learn.microsoft.com/es-es/windows-hardware/get-started/adk-install")
                    Case "FRA"
                        Process.Start("https://learn.microsoft.com/fr-fr/windows-hardware/get-started/adk-install")
                    Case "PTB", "PTG"
                        Process.Start("https://learn.microsoft.com/pt-pt/windows-hardware/get-started/adk-install")
                    Case "ITA"
                        Process.Start("https://learn.microsoft.com/it-it/windows-hardware/get-started/adk-install")
                End Select
            Case 1
                Process.Start("https://learn.microsoft.com/en-us/windows-hardware/get-started/adk-install")
            Case 2
                Process.Start("https://learn.microsoft.com/es-es/windows-hardware/get-started/adk-install")
            Case 3
                Process.Start("https://learn.microsoft.com/fr-fr/windows-hardware/get-started/adk-install")
            Case 4
                Process.Start("https://learn.microsoft.com/pt-pt/windows-hardware/get-started/adk-install")
            Case 5
                Process.Start("https://learn.microsoft.com/it-it/windows-hardware/get-started/adk-install")
        End Select
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Panel2.Enabled = CheckBox1.Checked
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        OpenFileDialog2.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog2_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk
        DynaLog.LogMessage("Unattended answer file to test: " & Quote & OpenFileDialog2.FileName & Quote)
        TextBox4.Text = OpenFileDialog2.FileName
    End Sub

    Private Sub ISOCreator_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If Visible And WindowState <> FormWindowState.Minimized Then
            ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
            Dim bm As New Bitmap(ListView1.ClientSize.Width, ListView1.ClientSize.Height)
            Graphics.FromImage(bm).Clear(ListView1.BackColor)
            ListView1.BackgroundImage = bm
        End If
        If BackgroundWorker1.IsBusy Then
            WindowHelper.DisableCloseCapability(Handle)
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs)
        Dim uefiCA2023_Message As String = "", uefiCA2023_Title As String = "", uefiCA2023_NotSupportedOnCurrentSystemMessage As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        uefiCA2023_Message = "This option will create ISO files that contain EFI boot binaries that are signed with the " & Quote & "Windows UEFI CA 2023" & Quote & " certificate." & CrLf & CrLf &
                            "Some computers that use UEFI may not boot correctly to this ISO file with the updated boot binaries. Because of this, it is recommended that you check your test equipment for compatibility with these binaries." & CrLf & CrLf &
                            "Run the PowerShell command described in the Help documentation for the ISO creator to determine whether a device has this certificate installed." & CrLf & CrLf &
                            "If you have any doubts, we recommend that you leave this option unchecked."
                        uefiCA2023_NotSupportedOnCurrentSystemMessage = "We have detected that, currently, this system does not support Windows UEFI CA 2023 boot binaries. If you continue with ISO creation, you may not be able to boot to the resulting ISO file on this system."
                        uefiCA2023_Title = "Windows UEFI CA 2023 information"
                    Case "ESN"
                        uefiCA2023_Message = "Esta opción creará archivos ISO que contengan archivos de arranque EFI firmados con el certificado " & Quote & "Windows UEFI CA 2023" & Quote & CrLf & CrLf &
                            "Algunos equipos que utilicen UEFI podrán no iniciar correctamente este archivo ISO con los archivos de arranque actualizados. Debido a esto, es recomendable que compruebe sus dispositivos de prueba para ver si son compatibles con estos archivos." & CrLf & CrLf &
                            "Ejecute el comando de PowerShell descrito en la Ayuda para el creador de archivos ISO (en inglés) para determinar si un equipo tiene este certificado instalado." & CrLf & CrLf &
                            "Si tiene dudas, le recomendamos que deje esta opción sin marcar."
                        uefiCA2023_NotSupportedOnCurrentSystemMessage = "Hemos detectado que, actualmente, su sistema no soporta archivos de arranque Windows UEFI CA 2023. Si continúa con la creación de archivos ISO, podría no ser capaz de arrancar a archivos ISO resultantes en este sistema."
                        uefiCA2023_Title = "Información sobre Windows UEFI CA 2023"
                    Case "FRA"
                        uefiCA2023_Message = "Cette option créera des fichiers ISO contenant des binaires de démarrage EFI signés avec le certificat " & Quote & "Windows UEFI CA 2023" & Quote & "." & CrLf & CrLf &
                            "Certains ordinateurs qui utilisent l'UEFI peuvent ne pas démarrer correctement avec ce fichier ISO contenant les binaires de démarrage mis à jour. Pour cette raison, il est recommandé de vérifier la compatibilité de votre équipement de test avec ces binaires." & CrLf & CrLf &
                            "Exécutez la commande PowerShell décrite dans la documentation d'aide du créateur de l'ISO (en anglais) pour déterminer si ce certificat est installé sur un appareil." & CrLf & CrLf &
                            "Si vous avez des doutes, nous vous recommandons de ne pas cocher cette option."
                        uefiCA2023_NotSupportedOnCurrentSystemMessage = "Nous avons détecté que, actuellement, ce système ne prend pas en charge les binaires de démarrage Windows UEFI CA 2023. Si vous poursuivez la création de l'ISO, vous risquez de ne pas pouvoir démarrer à partir du fichier ISO obtenu sur ce système."
                        uefiCA2023_Title = "Informations Windows UEFI CA 2023"
                    Case "PTB", "PTG"
                        uefiCA2023_Message = "Esta opção criará ficheiros ISO que contêm binários de arranque EFI assinados com o certificado " & Quote & "Windows UEFI CA 2023" & Quote & "." & CrLf & CrLf &
                            "Alguns computadores que utilizam UEFI podem não arrancar corretamente com este ficheiro ISO com os binários de arranque actualizados. Por este motivo, recomenda-se que verifique a compatibilidade do seu equipamento de teste com estes binários." & CrLf & CrLf &
                            "Execute o comando PowerShell descrito na documentação de ajuda do criador ISO (em inglês) para determinar se um dispositivo tem este certificado instalado." & CrLf & CrLf &
                            "Se tiver dúvidas, recomendamos que deixe esta opção desmarcada."
                        uefiCA2023_NotSupportedOnCurrentSystemMessage = "Detetámos que, atualmente, este sistema não suporta binários de arranque Windows UEFI CA 2023. Se continuar com a criação da ISO, poderá não conseguir arrancar a partir do ficheiro ISO resultante neste sistema."
                        uefiCA2023_Title = "Informações sobre o Windows UEFI CA 2023"
                    Case "ITA"
                        uefiCA2023_Message = "Questa opzione creerà file ISO contenenti binari di avvio EFI firmati con il certificato " & Quote & "Windows UEFI CA 2023" & Quote & "." & CrLf & CrLf &
                            "Alcuni computer che utilizzano UEFI potrebbero non avviarsi correttamente da questo file ISO con i binari di avvio aggiornati. Per questo motivo, si consiglia di verificare la compatibilità della propria apparecchiatura di test con questi file binari." & CrLf & CrLf &
                            "Eseguire il comando PowerShell descritto nella documentazione della Guida per il creatore ISO (in inglese) per determinare se un dispositivo ha questo certificato installato." & CrLf & CrLf &
                            "In caso di dubbi, si consiglia di lasciare questa opzione deselezionata."
                        uefiCA2023_NotSupportedOnCurrentSystemMessage = "Abbiamo rilevato che, attualmente, questo sistema non supporta i file binari di avvio Windows UEFI CA 2023. Se si procede con la creazione dell'ISO, potrebbe non essere possibile avviare il file ISO risultante su questo sistema."
                        uefiCA2023_Title = "Informazioni su Windows UEFI CA 2023"
                End Select
            Case 1
                uefiCA2023_Message = "This option will create ISO files that contain EFI boot binaries that are signed with the " & Quote & "Windows UEFI CA 2023" & Quote & " certificate." & CrLf & CrLf &
                    "Some computers that use UEFI may not boot correctly to this ISO file with the updated boot binaries. Because of this, it is recommended that you check your test equipment for compatibility with these binaries." & CrLf & CrLf &
                    "Run the PowerShell command described in the Help documentation for the ISO creator to determine whether a device has this certificate installed." & CrLf & CrLf &
                    "If you have any doubts, we recommend that you leave this option unchecked."
                uefiCA2023_NotSupportedOnCurrentSystemMessage = "We have detected that, currently, this system does not support Windows UEFI CA 2023 boot binaries. If you continue with ISO creation, you may not be able to boot to the resulting ISO file on this system."
                uefiCA2023_Title = "Windows UEFI CA 2023 information"
            Case 2
                uefiCA2023_Message = "Esta opción creará archivos ISO que contengan archivos de arranque EFI firmados con el certificado " & Quote & "Windows UEFI CA 2023" & Quote & CrLf & CrLf &
                    "Algunos equipos que utilicen UEFI podrán no iniciar correctamente este archivo ISO con los archivos de arranque actualizados. Debido a esto, es recomendable que compruebe sus dispositivos de prueba para ver si son compatibles con estos archivos." & CrLf & CrLf &
                    "Ejecute el comando de PowerShell descrito en la Ayuda para el creador de archivos ISO (en inglés) para determinar si un equipo tiene este certificado instalado." & CrLf & CrLf &
                    "Si tiene dudas, le recomendamos que deje esta opción sin marcar."
                uefiCA2023_NotSupportedOnCurrentSystemMessage = "Hemos detectado que, actualmente, su sistema no soporta archivos de arranque Windows UEFI CA 2023. Si continúa con la creación de archivos ISO, podría no ser capaz de arrancar a archivos ISO resultantes en este sistema."
                uefiCA2023_Title = "Información sobre Windows UEFI CA 2023"
            Case 3
                uefiCA2023_Message = "Cette option créera des fichiers ISO contenant des binaires de démarrage EFI signés avec le certificat " & Quote & "Windows UEFI CA 2023" & Quote & "." & CrLf & CrLf &
                    "Certains ordinateurs qui utilisent l'UEFI peuvent ne pas démarrer correctement avec ce fichier ISO contenant les binaires de démarrage mis à jour. Pour cette raison, il est recommandé de vérifier la compatibilité de votre équipement de test avec ces binaires." & CrLf & CrLf &
                    "Exécutez la commande PowerShell décrite dans la documentation d'aide du créateur de l'ISO (en anglais) pour déterminer si ce certificat est installé sur un appareil." & CrLf & CrLf &
                    "Si vous avez des doutes, nous vous recommandons de ne pas cocher cette option."
                uefiCA2023_NotSupportedOnCurrentSystemMessage = "Nous avons détecté que, actuellement, ce système ne prend pas en charge les binaires de démarrage Windows UEFI CA 2023. Si vous poursuivez la création de l'ISO, vous risquez de ne pas pouvoir démarrer à partir du fichier ISO obtenu sur ce système."
                uefiCA2023_Title = "Informations Windows UEFI CA 2023"
            Case 4
                uefiCA2023_Message = "Esta opção criará ficheiros ISO que contêm binários de arranque EFI assinados com o certificado " & Quote & "Windows UEFI CA 2023" & Quote & "." & CrLf & CrLf &
                    "Alguns computadores que utilizam UEFI podem não arrancar corretamente com este ficheiro ISO com os binários de arranque actualizados. Por este motivo, recomenda-se que verifique a compatibilidade do seu equipamento de teste com estes binários." & CrLf & CrLf &
                    "Execute o comando PowerShell descrito na documentação de ajuda do criador ISO (em inglês) para determinar se um dispositivo tem este certificado instalado." & CrLf & CrLf &
                    "Se tiver dúvidas, recomendamos que deixe esta opção desmarcada."
                uefiCA2023_NotSupportedOnCurrentSystemMessage = "Detetámos que, atualmente, este sistema não suporta binários de arranque Windows UEFI CA 2023. Se continuar com a criação da ISO, poderá não conseguir arrancar a partir do ficheiro ISO resultante neste sistema."
                uefiCA2023_Title = "Informações sobre o Windows UEFI CA 2023"
            Case 5
                uefiCA2023_Message = "Questa opzione creerà file ISO contenenti binari di avvio EFI firmati con il certificato " & Quote & "Windows UEFI CA 2023" & Quote & "." & CrLf & CrLf &
                    "Alcuni computer che utilizzano UEFI potrebbero non avviarsi correttamente da questo file ISO con i binari di avvio aggiornati. Per questo motivo, si consiglia di verificare la compatibilità della propria apparecchiatura di test con questi file binari." & CrLf & CrLf &
                    "Eseguire il comando PowerShell descritto nella documentazione della Guida per il creatore ISO (in inglese) per determinare se un dispositivo ha questo certificato installato." & CrLf & CrLf &
                    "In caso di dubbi, si consiglia di lasciare questa opzione deselezionata."
                uefiCA2023_NotSupportedOnCurrentSystemMessage = "Abbiamo rilevato che, attualmente, questo sistema non supporta i file binari di avvio Windows UEFI CA 2023. Se si procede con la creazione dell'ISO, potrebbe non essere possibile avviare il file ISO risultante su questo sistema."
                uefiCA2023_Title = "Informazioni su Windows UEFI CA 2023"
        End Select
        If CheckBox3.Checked Then
            MsgBox(uefiCA2023_Message, vbOKOnly + vbInformation, uefiCA2023_Title)

            ' Detect if we support UEFI CA 2023 binaries on the current system, just to have an idea (on the current system, at least)
            ' https://techcommunity.microsoft.com/blog/Windows-ITPro-blog/secure-boot-playbook-for-certificates-expiring-in-2026/4469235
            Try
                ' we don't REALLY need to check on BIOS systems
                If Not Environment.GetEnvironmentVariable("FIRMWARE_TYPE").Equals("UEFI") Then Exit Try

                ' Before checking the system for CA 2023 certs, we'll check if Secure Boot is enabled.
                DynaLog.LogMessage("Detecting current Secure Boot status...")

                Dim sbStateRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\SecureBoot\State", False)
                Dim sbState As Integer = sbStateRk.GetValue("UEFISecureBootEnabled")
                sbStateRk.Close()

                DynaLog.LogMessage("Secure Boot Status: " & sbState)

                ' If we have 0 then we know secure boot is disabled on the system.
                If sbState = 0 Then Exit Try

                DynaLog.LogMessage("Detecting if current system is compatible with UEFI CA 2023...")

                Dim sbUefiBinStatusRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\SecureBoot\Servicing", False)
                Dim sbUefiBinStatus As String = sbUefiBinStatusRk.GetValue("UEFICA2023Status", "")
                sbUefiBinStatusRk.Close()

                DynaLog.LogMessage("UEFI CA 2023 Status: " & sbUefiBinStatus)

                ' If the status value is "Updated", it means that the system has already applied Secure Boot DBX updates
                ' to enable support for UEFI CA 2023 binaries. If it is "NotStarted" or something else though, then
                ' the system hasn't initiated any DBX updates.
                If Not sbUefiBinStatus.Equals("updated", StringComparison.InvariantCultureIgnoreCase) Then
                    DynaLog.LogMessage("UEFI CA 2023 Status is not Updated. We are not running with UEFI CA 2023-supported SecureBoot")

                    MsgBox(uefiCA2023_NotSupportedOnCurrentSystemMessage, vbOKOnly + vbExclamation, uefiCA2023_Title)
                End If

            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub ADKDownloaderBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ADKDownloaderBW.DoWork
        DownloadADK()
    End Sub

    Private Sub ADKDownloaderBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles ADKDownloaderBW.ProgressChanged
        ProgressReporter.ReportProgress(Me, e.ProgressPercentage)
    End Sub

    Private Sub ADKDownloaderBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ADKDownloaderBW.RunWorkerCompleted
        ProgressReporter.Hide()
        adkDownloadSuccess = e.Error Is Nothing
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        PECustomizerDialog.ShowDialog(Me)
    End Sub

    Private Sub CheckBox4_MouseHover(sender As Object, e As EventArgs) Handles CheckBox4.MouseHover
        WindowHelper.DisplayToolTip(sender, "When you check this option, storage controllers and network adapter drivers from this machine will be included" & CrLf &
                                            "in your ISO file. They will also be applied to the image file once deployed.")
    End Sub

    Private Sub CheckBox3_MouseHover(sender As Object, e As EventArgs) Handles CheckBox3.MouseHover
        WindowHelper.DisplayToolTip(sender, "If available in your installed Assessment and Deployment Kit, your ISO file will use boot binaries signed with Windows UEFI CA 2023." & CrLf &
                                            "This option is designed for target systems that support Secure Boot and have the latest boot certificates in the allowlist database (DB).")
    End Sub
End Class