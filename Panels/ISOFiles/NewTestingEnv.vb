Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars

Public Class NewTestingEnv

    Dim progressMessages() As String = New String(2) {"Status", "Creating project. This can take some time. Please wait...", "The project has been created"}
    Dim success As Boolean

    Private Sub NewTestingEnv_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        progressMessages(0) = "Status"
                        progressMessages(1) = "Creating project. This can take some time. Please wait..."
                        progressMessages(2) = "The project has been created"
                        Text = "Create a testing environment"
                        Label1.Text = Text
                        Label2.Text = "This wizard will create an environment that will help you test your applications on Windows Preinstallation Environments." & CrLf & CrLf &
                                      "The project that will be created contains a template solution compatible with all environments and resources for the creation of applications for Windows Preinstallation Environments. You can learn more about these projects in the included README file."
                        Label3.Text = "Once you're ready, click the Create button."
                        Label5.Text = "Environment architecture:"
                        Label6.Text = "Architecture:"
                        Label7.Text = "Target project location:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "You can do other things while the project is being created. Come back here anytime for an updated status."
                        Button3.Text = "Browse..."
                        OK_Button.Text = "Create"
                        Cancel_Button.Text = "Cancel"
                        GroupBox2.Text = "Progress"
                        LinkLabel1.Text = "Download the Windows ADK"
                    Case "ESN"
                        progressMessages(0) = "Estado"
                        progressMessages(1) = "Creando proyecto. Esto puede llevar algo de tiempo. Espere..."
                        progressMessages(2) = "El proyecto ha sido creado"
                        Text = "Crear un entorno de pruebas"
                        Label1.Text = Text
                        Label2.Text = "Este asistente le creará un entorno que le ayudará a probar sus aplicaciones en entornos de preinstalación de Windows." & CrLf & CrLf &
                                      "El proyecto que será creado contiene una plantilla de solución compatible con todos los entornos, y recursos para crear aplicaciones para entornos de preinstalación de Windows. Puede aprender más acerca de estos proyectos en el archivo LÉAME incluido."
                        Label3.Text = "Cuando esté listo, haga clic en Crear."
                        Label5.Text = "Arquitectura del entorno:"
                        Label6.Text = "Arquitectura:"
                        Label7.Text = "Ubicación del proyecto:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "Puede hacer otras cosas mientras se crea el proyecto. Vuelva aquí para ver un estado actualizado."
                        Button3.Text = "Examinar..."
                        OK_Button.Text = "Crear"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox2.Text = "Progreso"
                        LinkLabel1.Text = "Descargar el ADK de Windows"
                    Case "FRA"
                        progressMessages(0) = "Statut"
                        progressMessages(1) = "Création du projet. Cela peut prendre un certain temps. Veuillez patienter..."
                        progressMessages(2) = "Le projet a été créé"
                        Text = "Créer un environnement de test"
                        Label1.Text = Text
                        Label2.Text = "Cet assistant va créer un environnement qui vous aidera à tester vos applications sur les environnements de préinstallation Windows." & CrLf & CrLf &
                                      "Le projet qui sera créé contient une solution modèle compatible avec tous les environnements et ressources pour la création d'applications pour les environnements de préinstallation Windows. Vous pouvez en savoir plus sur ces projets dans le fichier LISEZMOI inclus."
                        Label3.Text = "Lorsque vous êtes prêt, cliquez sur le bouton Créer."
                        Label5.Text = "Architecture de l'environnement :"
                        Label6.Text = "Architecture :"
                        Label7.Text = "Emplacement du projet cible :"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "Vous pouvez faire d'autres choses pendant la création du projet. Revenez ici à tout moment pour connaître l'état d'avancement du projet."
                        Button3.Text = "Parcourir..."
                        OK_Button.Text = "Créer"
                        Cancel_Button.Text = "Annuler"
                        GroupBox2.Text = "Progrès"
                        LinkLabel1.Text = "Télécharger l'ADK Windows"
                    Case "PTB", "PTG"
                        progressMessages(0) = "Estado"
                        progressMessages(1) = "A criar projeto. Isto pode demorar algum tempo. Por favor, aguarde..."
                        progressMessages(2) = "O projeto foi criado"
                        Text = "Criar um ambiente de teste"
                        Label1.Text = Text
                        Label2.Text = "Este assistente irá criar um ambiente que o ajudará a testar as suas aplicações em ambientes de pré-instalação do Windows." & CrLf & CrLf &
                                      "O projeto que será criado contém uma solução modelo compatível com todos os ambientes e recursos para a criação de aplicações para Ambientes de Pré-instalação do Windows. Pode obter mais informações sobre estes projectos no ficheiro LEIAME incluído."
                        Label3.Text = "Quando estiver pronto, clique no botão Criar."
                        Label5.Text = "Arquitetura do ambiente:"
                        Label6.Text = "Arquitetura:"
                        Label7.Text = "Localização do projeto alvo:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "Pode fazer outras coisas enquanto o projeto está a ser criado. Volte aqui em qualquer altura para obter um estado atualizado."
                        Button3.Text = "Navegar..."
                        OK_Button.Text = "Criar"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox2.Text = "Progresso"
                        LinkLabel1.Text = "Baixar o Windows ADK"
                    Case "ITA"
                        progressMessages(0) = "Stato"
                        progressMessages(1) = "Creazione del progetto. L'operazione può richiedere del tempo. Attendere..."
                        progressMessages(2) = "Il progetto è stato creato"
                        Text = "Creare un ambiente di test"
                        Label1.Text = Text
                        Label2.Text = "Questa procedura guidata creerà un ambiente che vi aiuterà a testare le vostre applicazioni sugli ambienti di preinstallazione di Windows." & CrLf & CrLf &
                                      "Il progetto che verrà creato contiene una soluzione modello compatibile con tutti gli ambienti e le risorse per la creazione di applicazioni per gli ambienti di preinstallazione di Windows. Per ulteriori informazioni su questi progetti, consultare il file LEGGIMI incluso."
                        Label3.Text = "Una volta pronti, fate clic sul pulsante Crea."
                        Label5.Text = "Architettura dell'ambiente:"
                        Label6.Text = "Architettura:"
                        Label7.Text = "Posizione del progetto di destinazione:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "Potete fare altre cose mentre il progetto viene creato. Tornate qui in qualsiasi momento per avere uno stato aggiornato."
                        Button3.Text = "Sfoglia..."
                        OK_Button.Text = "Crea"
                        Cancel_Button.Text = "Annullare"
                        GroupBox2.Text = "Stato di avanzamento"
                        LinkLabel1.Text = "Scarica l'ADK di Windows"
                End Select
            Case 1
                progressMessages(0) = "Status"
                progressMessages(1) = "Creating project. This can take some time. Please wait..."
                progressMessages(2) = "The project has been created"
                Text = "Create a testing environment"
                Label1.Text = Text
                Label2.Text = "This wizard will create an environment that will help you test your applications on Windows Preinstallation Environments." & CrLf & CrLf &
                              "The project that will be created contains a template solution compatible with all environments and resources for the creation of applications for Windows Preinstallation Environments. You can learn more about these projects in the included README file."
                Label3.Text = "Once you're ready, click the Create button."
                Label5.Text = "Environment architecture:"
                Label6.Text = "Architecture:"
                Label7.Text = "Target project location:"
                Label8.Text = progressMessages(0)
                Label9.Text = "You can do other things while the project is being created. Come back here anytime for an updated status."
                Button3.Text = "Browse..."
                OK_Button.Text = "Create"
                Cancel_Button.Text = "Cancel"
                GroupBox2.Text = "Progress"
                LinkLabel1.Text = "Download the Windows ADK"
            Case 2
                progressMessages(0) = "Estado"
                progressMessages(1) = "Creando proyecto. Esto puede llevar algo de tiempo. Espere..."
                progressMessages(2) = "El proyecto ha sido creado"
                Text = "Crear un entorno de pruebas"
                Label1.Text = Text
                Label2.Text = "Este asistente le creará un entorno que le ayudará a probar sus aplicaciones en entornos de preinstalación de Windows." & CrLf & CrLf &
                              "El proyecto que será creado contiene una plantilla de solución compatible con todos los entornos, y recursos para crear aplicaciones para entornos de preinstalación de Windows. Puede aprender más acerca de estos proyectos en el archivo LÉAME incluido."
                Label3.Text = "Cuando esté listo, haga clic en Crear."
                Label5.Text = "Arquitectura del entorno:"
                Label6.Text = "Arquitectura:"
                Label7.Text = "Ubicación del proyecto:"
                Label8.Text = progressMessages(0)
                Label9.Text = "Puede hacer otras cosas mientras se crea el proyecto. Vuelva aquí para ver un estado actualizado."
                Button3.Text = "Examinar..."
                OK_Button.Text = "Crear"
                Cancel_Button.Text = "Cancelar"
                GroupBox2.Text = "Progreso"
                LinkLabel1.Text = "Descargar el ADK de Windows"
            Case 3
                progressMessages(0) = "Statut"
                progressMessages(1) = "Création du projet. Cela peut prendre un certain temps. Veuillez patienter..."
                progressMessages(2) = "Le projet a été créé"
                Text = "Créer un environnement de test"
                Label1.Text = Text
                Label2.Text = "Cet assistant va créer un environnement qui vous aidera à tester vos applications sur les environnements de préinstallation Windows." & CrLf & CrLf &
                              "Le projet qui sera créé contient une solution modèle compatible avec tous les environnements et ressources pour la création d'applications pour les environnements de préinstallation Windows. Vous pouvez en savoir plus sur ces projets dans le fichier LISEZMOI inclus."
                Label3.Text = "Lorsque vous êtes prêt, cliquez sur le bouton Créer."
                Label5.Text = "Architecture de l'environnement :"
                Label6.Text = "Architecture :"
                Label7.Text = "Emplacement du projet cible :"
                Label8.Text = progressMessages(0)
                Label9.Text = "Vous pouvez faire d'autres choses pendant la création du projet. Revenez ici à tout moment pour connaître l'état d'avancement du projet."
                Button3.Text = "Parcourir..."
                OK_Button.Text = "Créer"
                Cancel_Button.Text = "Annuler"
                GroupBox2.Text = "Progrès"
                LinkLabel1.Text = "Télécharger l'ADK Windows"
            Case 4
                progressMessages(0) = "Estado"
                progressMessages(1) = "A criar projeto. Isto pode demorar algum tempo. Por favor, aguarde..."
                progressMessages(2) = "O projeto foi criado"
                Text = "Criar um ambiente de teste"
                Label1.Text = Text
                Label2.Text = "Este assistente irá criar um ambiente que o ajudará a testar as suas aplicações em ambientes de pré-instalação do Windows." & CrLf & CrLf &
                              "O projeto que será criado contém uma solução modelo compatível com todos os ambientes e recursos para a criação de aplicações para Ambientes de Pré-instalação do Windows. Pode obter mais informações sobre estes projectos no ficheiro LEIAME incluído."
                Label3.Text = "Quando estiver pronto, clique no botão Criar."
                Label5.Text = "Arquitetura do ambiente:"
                Label6.Text = "Arquitetura:"
                Label7.Text = "Localização do projeto alvo:"
                Label8.Text = progressMessages(0)
                Label9.Text = "Pode fazer outras coisas enquanto o projeto está a ser criado. Volte aqui em qualquer altura para obter um estado atualizado."
                Button3.Text = "Navegar..."
                OK_Button.Text = "Criar"
                Cancel_Button.Text = "Cancelar"
                GroupBox2.Text = "Progresso"
                LinkLabel1.Text = "Baixar o Windows ADK"
            Case 5
                progressMessages(0) = "Stato"
                progressMessages(1) = "Creazione del progetto. L'operazione può richiedere del tempo. Attendere..."
                progressMessages(2) = "Il progetto è stato creato"
                Text = "Creare un ambiente di test"
                Label1.Text = Text
                Label2.Text = "Questa procedura guidata creerà un ambiente che vi aiuterà a testare le vostre applicazioni sugli ambienti di preinstallazione di Windows." & CrLf & CrLf &
                              "Il progetto che verrà creato contiene una soluzione modello compatibile con tutti gli ambienti e le risorse per la creazione di applicazioni per gli ambienti di preinstallazione di Windows. Per ulteriori informazioni su questi progetti, consultare il file LEGGIMI incluso."
                Label3.Text = "Una volta pronti, fate clic sul pulsante Crea."
                Label5.Text = "Architettura dell'ambiente:"
                Label6.Text = "Architettura:"
                Label7.Text = "Posizione del progetto di destinazione:"
                Label8.Text = progressMessages(0)
                Label9.Text = "Potete fare altre cose mentre il progetto viene creato. Tornate qui in qualsiasi momento per avere uno stato aggiornato."
                Button3.Text = "Sfoglia..."
                OK_Button.Text = "Crea"
                Cancel_Button.Text = "Annullare"
                GroupBox2.Text = "Stato di avanzamento"
                LinkLabel1.Text = "Scarica l'ADK di Windows"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox3.ForeColor = ForeColor
        GroupBox2.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            TextBox3.Text = FolderBrowserDialog1.SelectedPath
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

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
        OptionsPanel.Enabled = False
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        BackgroundWorker1.ReportProgress(0)
        Dim ISOCreator As New Process()
        ISOCreator.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        ISOCreator.StartInfo.WorkingDirectory = Application.StartupPath & "\bin\extps1\PE_Helper"
        ISOCreator.StartInfo.Arguments = "-executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\extps1\PE_Helper\PE_Helper.ps1" & Quote & " -cmd StartDevelopment -testArch " & ComboBox1.SelectedItem & " -targetPath " & Quote & TextBox3.Text & Quote
        ISOCreator.Start()
        ISOCreator.WaitForExit()
        success = (ISOCreator.ExitCode = 0)
        BackgroundWorker1.ReportProgress(100)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        IdlePanel.Visible = False
        ISOProgressPanel.Visible = True
        If e.ProgressPercentage < 100 Then
            Label8.Text = progressMessages(1)
            ProgressBar1.Style = ProgressBarStyle.Marquee
        Else
            If success Then Label8.Text = progressMessages(2)
            ProgressBar1.Style = ProgressBarStyle.Blocks
        End If
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Dim msg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg = If(success, "The project has been created successfully", "Failed to create the project")
                    Case "ESN"
                        msg = If(success, "El proyecto ha sido creado satisfactoriamente", "No pudimos crear el proyecto")
                    Case "FRA"
                        msg = If(success, "Le projet a été créé avec succès", "Le processus de création de le projet a échoué")
                    Case "PTB", "PTG"
                        msg = If(success, "O projeto ISO foi criado com êxito", "O processo de criação do projeto falhou")
                    Case "ITA"
                        msg = If(success, "Il progetto è stato creato con successo", "La creazione del progetto non è riuscita")
                End Select
            Case 1
                msg = If(success, "The project has been created successfully", "Failed to create the project")
            Case 2
                msg = If(success, "El proyecto ha sido creado satisfactoriamente", "No pudimos crear el proyecto")
            Case 3
                msg = If(success, "Le projet a été créé avec succès", "Le processus de création de le projet a échoué")
            Case 4
                msg = If(success, "O projeto ISO foi criado com êxito", "O processo de criação do projeto falhou")
            Case 5
                msg = If(success, "Il progetto è stato creato con successo", "La creazione del progetto non è riuscita")
        End Select
        MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
        OptionsPanel.Enabled = True
        IdlePanel.Visible = True
        ISOProgressPanel.Visible = False
    End Sub
End Class