Imports System.Drawing.Drawing2D
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Net

Public Class PrgSetup

    Dim ColorModes() As String = New String(2) {"Use system setting", "Light mode", "Dark mode"}
    Dim Languages() As String = New String(5) {"Use system language", "English", "Spanish", "French", "Portuguese", "Italian"}
    Dim SupportedLangCodes() As String = New String(6) {"ENU", "ENG", "ESN", "FRA", "PTB", "PTG", "ITA"}

    Dim btnToolTip As New ToolTip()
    Private isMouseDown As Boolean = False
    Private mouseOffset As Point
    Dim pageInt As Integer = 0

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
        Dim msg As String = ""
        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                msg = "Minimize"
            Case "ESN"
                msg = "Minimizar"
            Case "FRA"
                msg = "Minimiser"
            Case "PTB", "PTG"
                msg = "Minimizar"
            Case "ITA"
                msg = "Minimizza"
        End Select
        btnToolTip.SetToolTip(sender, msg)
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
        Dim msg As String = ""
        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                msg = "Close"
            Case "ESN"
                msg = "Cerrar"
            Case "FRA"
                msg = "Fermer"
            Case "PTB", "PTG"
                msg = "Fechar"
            Case "ITA"
                msg = "Chiudi"
        End Select
        btnToolTip.SetToolTip(sender, msg)
    End Sub

    Private Sub closeBox_Click(sender As Object, e As EventArgs) Handles closeBox.Click
        IncompleteSetupDlg.ShowDialog()
        If IncompleteSetupDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Close()
        End If
    End Sub

    Private Sub backBox_MouseEnter(sender As Object, e As EventArgs) Handles backBox.MouseEnter
        backBox.Image = My.Resources.backbox_focus
    End Sub

    Private Sub backBox_MouseLeave(sender As Object, e As EventArgs) Handles backBox.MouseLeave
        backBox.Image = My.Resources.backbox
    End Sub

    Private Sub backBox_MouseDown(sender As Object, e As MouseEventArgs) Handles backBox.MouseDown
        backBox.Image = My.Resources.backbox_down
    End Sub

    Private Sub backBox_MouseUp(sender As Object, e As MouseEventArgs) Handles backBox.MouseUp
        backBox.Image = My.Resources.backbox_focus
    End Sub

    Private Sub backBox_MouseHover(sender As Object, e As EventArgs) Handles backBox.MouseHover
        Dim msg As String = ""
        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                msg = "Go back"
            Case "ESN"
                msg = "Atrás"
            Case "FRA"
                msg = "Retourner"
            Case "PTB", "PTG"
                msg = "Voltar atrás"
            Case "ITA"
                msg = "Indietro"
        End Select
        btnToolTip.SetToolTip(sender, msg)
    End Sub

    'Private Sub backBox_Click(sender As Object, e As EventArgs) Handles backBox.Click
    '    Back_Button.PerformClick()
    'End Sub

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

    Private Sub Next_Button_Click(sender As Object, e As EventArgs) Handles Next_Button.Click
        If pageInt = 4 Then
            ' Set program to English if the system language is not supported
            If Not SupportedLangCodes.Contains(My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName) Then MainForm.Language = 1
            MainForm.SaveDTSettings()
            Close()
        End If
        pageInt += 1
        Select Case pageInt
            Case 0
                WelcomePanel.Visible = True
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = True
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 2
                MainForm.ColorMode = ComboBox1.SelectedIndex
                MainForm.Language = ComboBox2.SelectedIndex
                MainForm.LogFont = ComboBox3.SelectedItem
                MainForm.LogFontSize = NumericUpDown1.Value
                MainForm.LogFontIsBold = Toggle1.Checked
                MainForm.ProgressPanelStyle = If(RadioButton1.Checked, 1, 0)
                MainForm.ProjectView.Visible = True
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = True
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 3
                MainForm.AutoLogs = CheckBox1.Checked
                If Not CheckBox1.Checked And Not Directory.Exists(Path.GetDirectoryName(TextBox2.Text)) Then
                    Dim msg As String = ""
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "The folder the log file will be stored on doesn't exist. Make sure it exists and try again."
                        Case "ESN"
                            msg = "La carpeta donde se almacenará el archivo de registro no existe. Asegúrese de que exista e inténtelo de nuevo."
                        Case "FRA"
                            msg = "Le dossier dans lequel le fichier journal sera stocké n'existe pas. Assurez-vous qu'il existe et réessayez."
                        Case "PTB", "PTG"
                            msg = "A pasta onde o ficheiro de registo será guardado não existe. Certifique-se de que existe e tente novamente."
                        Case "ITA"
                            msg = "La cartella in cui verrà memorizzato il file di registro non esiste. Assicurarsi che esista e riprovare."
                    End Select
                    MsgBox(msg, vbOKOnly + vbCritical, Text)
                    Exit Sub
                End If
                MainForm.LogFile = TextBox2.Text
                MainForm.LogLevel = TrackBar1.Value + 1
                pageInt += 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = True
            Case 4
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = True
        End Select
        If pageInt = 4 Then
            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                Case "ENU", "ENG"
                    Next_Button.Text = "Finish"
                Case "ESN"
                    Next_Button.Text = "Finalizar"
                Case "FRA"
                    Next_Button.Text = "Finir"
                Case "PTB", "PTG"
                    Next_Button.Text = "Terminar"
                Case "ITA"
                    Next_Button.Text = "Fine"
            End Select
            Cancel_Button.Enabled = False
            closeBox.Enabled = False
        Else
            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                Case "ENU", "ENG"
                    Next_Button.Text = "Next"
                Case "ESN"
                    Next_Button.Text = "Siguiente"
                Case "FRA"
                    Next_Button.Text = "Suivant"
                Case "PTB", "PTG"
                    Next_Button.Text = "Seguinte"
                Case "ITA"
                    Next_Button.Text = "Avanti"
            End Select
            Cancel_Button.Enabled = True
            closeBox.Enabled = True
        End If
        If pageInt = 0 Then
            Back_Button.Enabled = False
            backBox.Visible = False
            Label1.Left = 8
        Else
            Back_Button.Enabled = True
            backBox.Visible = True
            Label1.Left = 54
        End If
    End Sub

    Private Sub Back_Button_Click(sender As Object, e As EventArgs) Handles Back_Button.Click, backBox.Click
        pageInt -= 1
        Select Case pageInt
            Case 0
                WelcomePanel.Visible = True
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = True
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 2
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = True
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 3
                ' Same
                'WelcomePanel.Visible = False
                'CustomizationPanel.Visible = False
                'LogsPanel.Visible = False
                'ModulesPanel.Visible = True
                'FinishPanel.Visible = False
                pageInt -= 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = True
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 4
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = True
        End Select
        If pageInt = 4 Then
            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                Case "ENU", "ENG"
                    Next_Button.Text = "Finish"
                Case "ESN"
                    Next_Button.Text = "Finalizar"
                Case "FRA"
                    Next_Button.Text = "Finir"
                Case "PTB", "PTG"
                    Next_Button.Text = "Terminar"
                Case "ITA"
                    Next_Button.Text = "Fine"
            End Select
            Cancel_Button.Enabled = False
            closeBox.Enabled = False
        Else
            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                Case "ENU", "ENG"
                    Next_Button.Text = "Next"
                Case "ESN"
                    Next_Button.Text = "Siguiente"
                Case "FRA"
                    Next_Button.Text = "Suivant"
                Case "PTB", "PTG"
                    Next_Button.Text = "Seguinte"
                Case "ITA"
                    Next_Button.Text = "Avanti"
            End Select
            Cancel_Button.Enabled = True
            closeBox.Enabled = True
        End If
        If pageInt = 0 Then
            Back_Button.Enabled = False
            backBox.Visible = False
            Label1.Left = 8
        Else
            Back_Button.Enabled = True
            backBox.Visible = True
            Label1.Left = 54
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ' MainForm.SaveDTSettings()
        Options.PrefReset.Enabled = False
        Options.ShowDialog(Me)
    End Sub

    Private Sub PrgSetup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Generate new settings file and load it
        MainForm.GenerateDTSettings()
        MainForm.LoadDTSettings(1)
        GetSystemFonts()
        TextBox2.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Logs\DISM\DISM.log"
        MainForm.LogFile = TextBox2.Text

        Next_Button.Left = 998

        ' Set color modes
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BodyPanelContainer.BackColor = Color.FromArgb(48, 48, 48)
            BodyPanelContainer.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BodyPanelContainer.BackColor = Color.FromArgb(239, 239, 242)
            BodyPanelContainer.ForeColor = Color.Black
        End If
        ComboBox1.BackColor = BodyPanelContainer.BackColor
        ComboBox2.BackColor = BodyPanelContainer.BackColor
        ComboBox3.BackColor = BodyPanelContainer.BackColor
        ComboBox1.ForeColor = BodyPanelContainer.ForeColor
        ComboBox2.ForeColor = BodyPanelContainer.ForeColor
        ComboBox3.ForeColor = BodyPanelContainer.ForeColor
        NumericUpDown1.BackColor = BodyPanelContainer.BackColor
        NumericUpDown1.ForeColor = BodyPanelContainer.ForeColor
        TextBox1.BackColor = BodyPanelContainer.BackColor
        TextBox1.ForeColor = BodyPanelContainer.ForeColor
        TextBox2.BackColor = BodyPanelContainer.BackColor
        TextBox2.ForeColor = BodyPanelContainer.ForeColor
        TrackBar1.BackColor = BodyPanelContainer.BackColor

        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        ComboBox1.SelectedText = ""
        ComboBox2.SelectedText = ""

        ' Set translations (follow system language)
        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                Text = "Set up DISMTools"
                Label1.Text = Text
                Label2.Text = "Welcome to DISMTools"
                Label3.Text = "DISMTools is a free and open-source, project-driven GUI for DISM operations. To begin setting things up, click Next."
                Label5.Text = "Make it yours. Customize this program to your liking and click Next. These settings can be configured at any time in the " & Quote & "Personalization" & Quote & " section in the Options window"
                Label6.Text = "Customize this program"
                Label7.Text = "Color mode:"
                Label8.Text = "Language:"
                Label9.Text = "Log window font:"
                Label10.Text = "Log file:"
                ' Since we start with log level 3, manually show that option
                Label11.Text = "Errors, warnings and information messages (Log level 3)"
                Label13.Text = "Specify the log settings and click Next. Depending on the content level you specify, we will log more or less information. This setting can be configured at any time in the " & Quote & "Logs" & Quote & " section in the Options window"
                Label14.Text = "What should we log when you perform an operation?"
                ' Same here
                Label16.Text = "The log file should display errors, warnings and information messages after performing an image operation."
                Label20.Text = "Is there anything else you would like to configure?"
                Label21.Text = "The settings available to you are more than what you've just configured. If you wish to change more of these, click the button below. We'll also make those settings persistent."
                Label22.Text = "You can perform these steps at any time."
                Label23.Text = "You have finished setting up the basics to use DISMTools the way you wanted. Click " & Quote & "Finish" & Quote & ", and we'll make your settings persistent."
                Label24.Text = "Setup is complete"
                Label25.Text = "Now that you've set things up, we recommend you do the following things:"
                Label26.Text = "Stay up to date to receive new features and an improved experience"
                Label27.Text = "Get started with DISMTools and image servicing, so you can get around quicker"
                Label28.Text = "Secondary progress panel style:"
                Label29.Text = "This font may not be readable on log windows. While you can still use it, we recommend monospaced fonts for increased readability."
                Back_Button.Text = "Back"
                Next_Button.Text = "Next"
                Cancel_Button.Text = "Cancel"
                Button1.Text = "Browse..."
                Button2.Text = "Use default log file"
                Button5.Text = "Configure more settings"
                Button6.Text = "Get started"
                Button7.Text = "Check for updates"
                CheckBox1.Text = "Automatically create logs in the program's log directory"
                RadioButton1.Text = "Modern"
                RadioButton2.Text = "Classic"
                SaveFileDialog1.Title = "Specify the log file"

                ' Configure string arrays to put them in the comboboxes
                ColorModes(0) = "Use system setting"
                ColorModes(1) = "Light mode"
                ColorModes(2) = "Dark mode"
                Languages(0) = "Use system language"
                Languages(1) = "English"
                Languages(2) = "Spanish"
                Languages(3) = "French"
                Languages(4) = "Portuguese"
                Languages(5) = "Italian"
            Case "ESN"
                Text = "Configurar DISMTools"
                Label1.Text = Text
                Label2.Text = "Bienvenido a DISMTools"
                Label3.Text = "DISMTools es una interfaz gráfica basada en proyectos, gratuita y de código abierto. Para comenzar a configurar el programa, haga clic en Siguiente."
                Label5.Text = "Hágalo suyo. Personalice este programa a su gusto y haga clic en Siguiente. Estas opciones pueden ser configuradas en cualquier momento en la sección " & Quote & "Personalización" & Quote & " de la ventana Opciones"
                Label6.Text = "Personalice este programa"
                Label7.Text = "Modo de color:"
                Label8.Text = "Idioma:"
                Label9.Text = "Fuente de la ventana de registro:"
                Label10.Text = "Archivo de registro:"
                ' Since we start with log level 3, manually show that option
                Label11.Text = "Errores, advertencias y mensajes de información (Nivel 3)"
                Label13.Text = "Especifique las opciones del registro y haga clic en Siguiente. Dependiendo del nivel de contenido que especifique, registraremos más o menos información. Esta opción puede ser configurada en cualquier momento en la sección " & Quote & "Registro" & Quote & " de la ventana Opciones"
                Label14.Text = "¿Qué deberíamos registrar cuando realice una operación?"
                ' Same here
                Label16.Text = "El archivo de registro debe mostrar errores, advertencias y mensajes de información tras realizar una operación."
                Label20.Text = "¿Hay algo más que quiera configurar?"
                Label21.Text = "Las opciones disponibles son más de las que acaba de configurar. Si desea cambiarlas, haga clic en el botón de abajo. También guardaremos esas preferencias."
                Label22.Text = "Puede realizar estos pasos en cualquier momento."
                Label23.Text = "Ha terminado de configurar las opciones básicas para utilizar DISMTools como quiso. Haga clic en " & Quote & "Finalizar" & Quote & ", y guardaremos sus preferencias."
                Label24.Text = "Configuración completa"
                Label25.Text = "Ahora que ha configurado el programa, le recomendamos que haga lo siguiente:"
                Label26.Text = "Manténgase al día para recibir nuevas características y una experiencia mejorada"
                Label27.Text = "Aprenda DISMTools y el servicio de imágenes para poder manejarse mejor"
                Label28.Text = "Estilo del panel de progreso secundario:"
                Label29.Text = "Esta fuente podría no ser legible en ventanas de registro. Aunque todavía pueda utilizarla, le recomendamos fuentes monoespaciadas para una legibilidad aumentada."
                Back_Button.Text = "Atrás"
                Next_Button.Text = "Siguiente"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Examinar..."
                Button2.Text = "Utilizar archivo de registro predeterminado"
                Button5.Text = "Configurar más opciones"
                Button6.Text = "Comenzar"
                Button7.Text = "Comprobar actualizaciones"
                CheckBox1.Text = "Crear archivos de registro automáticamente en la carpeta de registros del programa"
                RadioButton1.Text = "Moderno"
                RadioButton2.Text = "Clásico"
                SaveFileDialog1.Title = "Especifique el archivo de registro"

                ' Configure string arrays to put them in the comboboxes
                ColorModes(0) = "Usar configuración del sistema"
                ColorModes(1) = "Modo claro"
                ColorModes(2) = "Modo oscuro"
                Languages(0) = "Usar idioma del sistema"
                Languages(1) = "Inglés"
                Languages(2) = "Español"
                Languages(3) = "Francés"
                Languages(4) = "Portugués"
                Languages(5) = "Italiano"
            Case "FRA"
                Text = "Configurer DISMTools"
                Label1.Text = Text
                Label2.Text = "Bienvenue à DISMTools"
                Label3.Text = "DISMTools est une interface graphique libre et gratuite pour les opérations DISM. Pour commencer à configurer les choses, cliquez sur Suivant."
                Label5.Text = "Faites-le vôtre. Personnalisez ce programme à votre guise et cliquez sur Suivant. Ces paramètres peuvent être configurés à tout moment dans la section " & Quote & "Personnalisation" & Quote & " de la fenêtre des paramètres."
                Label6.Text = "Personnaliser ce programme"
                Label7.Text = "Mode couleur :"
                Label8.Text = "Langue :"
                Label9.Text = "Fonte de la fenêtre du journal :"
                Label10.Text = "Fichier journal :"
                ' Since we start with log level 3, manually show that option
                Label11.Text = "Erreurs, avertissements et messages d'information (niveau du journal 3)"
                Label13.Text = "Spécifiez les paramètres du journal et cliquez sur Suivant. En fonction du niveau de contenu spécifié, nous enregistrerons plus ou moins d'informations. Ce paramètre peut être configuré à tout moment dans la section " & Quote & "Journaux" & Quote & " de la fenêtre des paramètres."
                Label14.Text = "Que devons-nous enregistrer lorsque vous effectuez une opération ?"
                ' Same here
                Label16.Text = "Le fichier journal doit afficher les erreurs, les avertissements et les messages d'information après l'exécution d'une opération d'image."
                Label20.Text = "Souhaitez-vous configurer autre chose ?"
                Label21.Text = "Les paramètres disponibles sont plus nombreux que ceux que vous venez de configurer. Si vous souhaitez en modifier d'autres, cliquez sur le bouton ci-dessous. Nous rendrons également ces paramètres persistants."
                Label22.Text = "Vous pouvez effectuer ces démarches à tout moment."
                Label23.Text = "Vous avez fini de configurer les bases pour utiliser DISMTools comme vous le souhaitiez. Cliquez sur " & Quote & "Finir" & Quote & ", et nous rendrons vos paramètres persistants."
                Label24.Text = "La configuration est terminée"
                Label25.Text = "Maintenant que vous avez tout configuré, nous vous recommandons de procéder aux opérations suivantes :"
                Label26.Text = "Restez à jour pour recevoir de nouvelles caractéristiques et une expérience améliorée."
                Label27.Text = "Commencez à utiliser DISMTools et le service d'images, afin de vous déplacer plus rapidement."
                Label28.Text = "Style du panneau de progression secondaire :"
                Label29.Text = "Cette police peut ne pas être lisible sur les fenêtres logiques. Bien que vous puissiez encore l'utiliser, nous recommandons les polices monospaces pour une meilleure lisibilité."
                Back_Button.Text = "Retour"
                Next_Button.Text = "Suivant"
                Cancel_Button.Text = "Annuler"
                Button1.Text = "Parcourir..."
                Button2.Text = "Utiliser le fichier journal par défaut"
                Button5.Text = "Configurer d'autres paramètres"
                Button6.Text = "Commencer"
                Button7.Text = "Mettre à jour les données"
                CheckBox1.Text = "Créer automatiquement des journaux dans le répertoire des journaux du programme"
                RadioButton1.Text = "Moderne"
                RadioButton2.Text = "Classique"
                SaveFileDialog1.Title = "Spécifier le fichier journal"

                ' Configure string arrays to put them in the comboboxes
                ColorModes(0) = "Utiliser les paramètres du système"
                ColorModes(1) = "Mode lumineux"
                ColorModes(2) = "Mode sombre"
                Languages(0) = "Utiliser la langue du système"
                Languages(1) = "Anglais"
                Languages(2) = "Espagnol"
                Languages(3) = "Français"
                Languages(4) = "Portugais"
                Languages(5) = "Italien"
            Case "PTB", "PTG"
                Text = "Configurar DISMTools"
                Label1.Text = Text
                Label2.Text = "Bem-vindo ao DISMTools"
                Label3.Text = "DISMTools é uma GUI gratuita e de código aberto, orientada para projectos, para operações DISM. Para iniciar a configuração, clique em Seguinte."
                Label5.Text = "Torne-o seu. Personalize este programa a seu gosto e clique em Next. Estas configurações podem ser feitas a qualquer momento na secção " & Quote & "Personalização" & Quote & " da janela Opções"
                Label6.Text = "Personalizar este programa"
                Label7.Text = "Modo de cor:"
                Label8.Text = "Idioma:"
                Label9.Text = "Tipo de letra da janela de registo:"
                Label10.Text = "Ficheiro de registo:"
                ' Since we start with log level 3, manually show that option
                Label11.Text = "Erros, avisos e mensagens de informação (nível de registo 3)"
                Label13.Text = "Especifique as configurações de registo e clique em Seguinte. Dependendo do nível de conteúdo que especificar, registaremos mais ou menos informações. Esta configuração pode ser feita a qualquer momento na secção " & Quote & "Logs" & Quote & " da janela Opções"
                Label14.Text = "O que devemos registar quando executa uma operação?"
                ' Same here
                Label16.Text = "O ficheiro de registo deve apresentar erros, avisos e mensagens de informação após a execução de uma operação de imagem."
                Label20.Text = "Há mais alguma coisa que gostaria de configurar?"
                Label21.Text = "As configurações disponíveis são mais do que as que acabou de configurar. Se pretender alterar mais definições, clique no botão abaixo. Também vamos tornar essas configurações persistentes."
                Label22.Text = "Pode executar estes passos em qualquer altura."
                Label23.Text = "Terminou a configuração básica para usar o DISMTools da forma desejada. Clique em " & Quote & "Finish" & Quote & ", e as configurações serão mantidas."
                Label24.Text = "A configuração está concluída"
                Label25.Text = "Agora que já configurou tudo, recomendamos que efectue as seguintes acções:"
                Label26.Text = "Mantenha-se atualizado para receber novas funcionalidades e uma experiência melhorada"
                Label27.Text = "Começar a utilizar o DISMTools e o serviço de manutenção de imagens, para obter mais rapidamente"
                Label28.Text = "Estilo do painel de progresso secundário:"
                Label29.Text = "Esta fonte pode não ser legível em janelas de registo. Embora possa continuar a utilizá-lo, recomendamos tipos de letra monoespaçados para maior legibilidade."
                Back_Button.Text = "Voltar"
                Next_Button.Text = "Seguinte"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Navegar..."
                Button2.Text = "Utilizar ficheiro de registo predefinido"
                Button5.Text = "Configurar mais definições"
                Button6.Text = "Obter"
                Button7.Text = "Verificar se há actualizações"
                CheckBox1.Text = "Criar automaticamente registos no diretório de registos do programa"
                RadioButton1.Text = "Moderno"
                RadioButton2.Text = "Clássico"
                SaveFileDialog1.Title = "Especificar o ficheiro de registo"

                ' Configure string arrays to put them in the comboboxes
                ColorModes(0) = "Utilizar a configuração do sistema"
                ColorModes(1) = "Modo de luz"
                ColorModes(2) = "Modo escuro"
                Languages(0) = "Utilizar o idioma do sistema"
                Languages(1) = "Inglês"
                Languages(2) = "Espanhol"
                Languages(3) = "Francês"
                Languages(4) = "Português"
                Languages(5) = "Italiano"
            Case "ITA"
                Text = "Impostare DISMTools"
                Label1.Text = Text
                Label2.Text = "Ti diamo il benvenuto a DISMTools"
                Label3.Text = "DISMTools è un'interfaccia grafica gratuita e open-source, basata su progetti, per le operazioni DISM. Per iniziare a configurare le operazioni, fare clic su Avanti"
                Label5.Text = "Rendilo tuo. Personalizzate questo programma a vostro piacimento e fate clic su Avanti. Queste impostazioni possono essere configurate in qualsiasi momento nella sezione " & Quote & "Personalizzazione" & Quote & " della finestra Opzioni"
                Label6.Text = "Personalizzazione di questo programma"
                Label7.Text = "Modalità colore:"
                Label8.Text = "Lingua:"
                Label9.Text = "Carattere della finestra di registro:"
                Label10.Text = "File di registro:"
                ' Since we start with log level 3, manually show that option
                Label11.Text = "Errori, avvisi e messaggi informativi (livello di registro 3)"
                Label13.Text = "Specificare le impostazioni del registro e fare clic su Avanti. A seconda del livello di contenuto specificato, verranno registrate più o meno informazioni. Questa impostazione può essere configurata in qualsiasi momento nella sezione " & Quote & "Logs" & Quote & " della finestra Opzioni"
                Label14.Text = "Cosa registrare quando si esegue un'operazione?"
                ' Same here
                Label16.Text = "Il file di registro deve visualizzare gli errori, le avvertenze e i messaggi informativi dopo l'esecuzione di un'operazione sull'immagine."
                Label20.Text = "C'è qualcos'altro che si desidera configurare?"
                Label21.Text = "Le impostazioni disponibili sono più di quelle appena configurate. Se si desidera modificarne altre, fare clic sul pulsante sottostante. Inoltre, queste impostazioni diventeranno persistenti"
                Label22.Text = "È possibile eseguire questi passaggi in qualsiasi momento"
                Label23.Text = "Avete finito di impostare gli elementi di base per utilizzare DISMTools nel modo desiderato. Fate clic su " & Quote & "Finish" & Quote & " e le vostre impostazioni diventeranno permanenti."
                Label24.Text = "L'impostazione è completa"
                Label25.Text = "Ora che avete configurato il tutto, vi consigliamo di fare le seguenti cose:"
                Label26.Text = "Rimanere aggiornati per ricevere nuove funzioni e un'esperienza migliorata"
                Label27.Text = "Iniziare a utilizzare DISMTools e il servizio di assistenza alle immagini, in modo da muoversi più rapidamente"
                Label28.Text = "Stile del pannello di avanzamento secondario:"
                Label29.Text = "Questo carattere potrebbe non essere leggibile sulle finestre di registro. Anche se è possibile utilizzarlo, si consiglia di utilizzare caratteri monospaziati per una maggiore leggibilità."
                Back_Button.Text = "Indietro"
                Next_Button.Text = "Avanti"
                Cancel_Button.Text = "Annulla"
                Button1.Text = "Sfoglia..."
                Button2.Text = "Usa il file di registro predefinito"
                Button5.Text = "Configura altre impostazioni"
                Button6.Text = "Iniziare"
                Button7.Text = "Controlla gli aggiornamenti"
                CheckBox1.Text = "Crea automaticamente i log nella directory dei log del programma"
                RadioButton1.Text = "Moderno"
                RadioButton2.Text = "Classica"
                SaveFileDialog1.Title = "Specificare il file di registro"

                ' Configure string arrays to put them in the comboboxes
                ColorModes(0) = "Utilizza le impostazioni di sistema"
                ColorModes(1) = "Modalità luce"
                ColorModes(2) = "Modalità scura"
                Languages(0) = "Utilizza la lingua di sistema"
                Languages(1) = "Inglese"
                Languages(2) = "Spagnolo"
                Languages(3) = "Francese"
                Languages(4) = "Portoghese"
                Languages(5) = "Italiano"
        End Select
        ' Add new items to the comboboxes
        ComboBox1.Items.AddRange(ColorModes)
        ComboBox2.Items.AddRange(Languages)

        ' Since we default to the system deciding the aforementioned settings, choose the first items
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0

        If Not Environment.OSVersion.Version.Major >= 10 Or Not (DetectFont("Segoe UI Variable Display Semib") Or DetectFont("Segoe UI Variable Semib")) Then
            Label2.Font = New Font("Segoe UI", Label2.Font.Size, FontStyle.Regular)
            Label6.Font = New Font("Segoe UI", Label6.Font.Size, FontStyle.Regular)
            Label14.Font = New Font("Segoe UI", Label14.Font.Size, FontStyle.Regular)
            Label24.Font = New Font("Segoe UI", Label24.Font.Size, FontStyle.Regular)
        End If
    End Sub

    Function DetectFont(FontName As String) As Boolean
        For Each fntFamily As FontFamily In FontFamily.Families
            If fntFamily.Name = FontName Then
                Return True
            End If
        Next
        Return False
    End Function

    Sub GetSystemFonts()
        ComboBox3.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            ComboBox3.Items.Add(fntFamily.Name)
        Next
        ComboBox3.SelectedItem = "Consolas"
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        TextBox1.Font = New Font(ComboBox3.Text, NumericUpDown1.Value, If(Toggle1.Checked, FontStyle.Bold, FontStyle.Regular))
        MainForm.LogFont = ComboBox3.SelectedItem
        MainForm.LogFontSize = NumericUpDown1.Value
        MainForm.LogFontIsBold = Toggle1.Checked
        Panel9.Visible = Not IsMonospacedFont(ComboBox3.Text)
    End Sub

    Function IsMonospacedFont(ftName As String) As Boolean
        Using testFont As Font = New Font(ftName, 10)
            Dim widthI As Decimal = MeasureCharacterWidth(testFont, "i")
            Dim widthW As Decimal = MeasureCharacterWidth(testFont, "w")
            Return widthI = widthW
        End Using
        Return False
    End Function

    Function MeasureCharacterWidth(ft As Font, character As Char) As Decimal
        Using bmp As Bitmap = New Bitmap(1, 1)
            Using g As Graphics = Graphics.FromImage(bmp)
                Dim size As SizeF = g.MeasureString(character.ToString(), ft)
                Return size.Width
            End Using
        End Using
        Return 0
    End Function

    Private Sub Toggle1_CheckedChanged(sender As Object, e As EventArgs) Handles Toggle1.CheckedChanged
        TextBox1.Font = New Font(ComboBox3.Text, NumericUpDown1.Value, If(Toggle1.Checked, FontStyle.Bold, FontStyle.Regular))
        MainForm.LogFont = ComboBox3.SelectedItem
        MainForm.LogFontSize = NumericUpDown1.Value
        MainForm.LogFontIsBold = Toggle1.Checked
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        TextBox1.Font = New Font(ComboBox3.Text, NumericUpDown1.Value, If(Toggle1.Checked, FontStyle.Bold, FontStyle.Regular))
        MainForm.LogFont = ComboBox3.SelectedItem
        MainForm.LogFontSize = NumericUpDown1.Value
        MainForm.LogFontIsBold = Toggle1.Checked
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        MainForm.ColorMode = ComboBox1.SelectedIndex
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        MainForm.Language = ComboBox2.SelectedIndex
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                Select Case TrackBar1.Value
                    Case 0
                        Label11.Text = "Errors (Log level 1)"
                        Label16.Text = "The log file should only display errors after performing an image operation."
                    Case 1
                        Label11.Text = "Errors and warnings (Log level 2)"
                        Label16.Text = "The log file should display errors and warnings after performing an image operation."
                    Case 2
                        Label11.Text = "Errors, warnings and information messages (Log level 3)"
                        Label16.Text = "The log file should display errors, warnings and information messages after performing an image operation."
                    Case 3
                        Label11.Text = "Errors, warnings, information and debug messages (Log level 4)"
                        Label16.Text = "The log file should display errors, warnings, information and debug messages after performing an image operation."
                End Select
            Case "ESN"
                Select Case TrackBar1.Value
                    Case 0
                        Label11.Text = "Errores (Nivel 1)"
                        Label16.Text = "El archivo de registro solo debe mostrar errores tras realizar una operación."
                    Case 1
                        Label11.Text = "Errores y advertencias (Nivel 2)"
                        Label16.Text = "El archivo de registro debe mostrar errores y advertencias tras realizar una operación."
                    Case 2
                        Label11.Text = "Errores, advertencias y mensajes de información (Nivel 3)"
                        Label16.Text = "El archivo de registro debe mostrar errores, advertencias y mensajes de información tras realizar una operación."
                    Case 3
                        Label11.Text = "Errores, advertencias, mensajes de información y de depuración (Nivel 4)"
                        Label16.Text = "El archivo de registro debe mostrar errores, advertencias, mensajes de información y de depuración tras realizar una operación."
                End Select
            Case "FRA"
                Select Case TrackBar1.Value
                    Case 0
                        Label11.Text = "Erreurs (niveau du journal 1)"
                        Label16.Text = "Le fichier journal ne doit afficher les erreurs qu'après l'exécution d'une opération d'image."
                    Case 1
                        Label11.Text = "Erreurs et avertissements (niveau de journal 2)"
                        Label16.Text = "Le fichier journal doit afficher les erreurs et les avertissements après l'exécution d'une opération d'image."
                    Case 2
                        Label11.Text = "Erreurs, avertissements et messages d'information (niveau du journal 3)"
                        Label16.Text = "Le fichier journal doit afficher les erreurs, les avertissements et les messages d'information après l'exécution d'une opération d'image."
                    Case 3
                        Label11.Text = "Erreurs, avertissements, informations et messages de débogage (niveau du journal 4)"
                        Label16.Text = "Le fichier journal doit afficher les erreurs, les avertissements, les informations et les messages de débogage après l'exécution d'une opération d'image."
                End Select
            Case "PTB", "PTG"
                Select Case TrackBar1.Value
                    Case 0
                        Label11.Text = "Erros (nível de registo 1)"
                        Label16.Text = "O ficheiro de registo só deve apresentar erros depois de executar uma operação de imagem."
                    Case 1
                        Label11.Text = "Erros e avisos (nível de registo 2)"
                        Label16.Text = "O ficheiro de registo deve apresentar erros e avisos após a realização de uma operação de imagem."
                    Case 2
                        Label11.Text = "Erros, avisos e mensagens de informação (nível de registo 3)"
                        Label16.Text = "O ficheiro de registo deve apresentar erros, avisos e mensagens de informação após a realização de uma operação de imagem."
                    Case 3
                        Label11.Text = "Erros, avisos, informações e mensagens de depuração (nível de registo 4)"
                        Label16.Text = "O ficheiro de registo deve apresentar erros, avisos, informações e mensagens de depuração após a realização de uma operação de imagem."
                End Select
            Case "ITA"
                Select Case TrackBar1.Value
                    Case 0
                        Label11.Text = "Errori (livello di log 1)"
                        Label16.Text = "Il file di registro dovrebbe visualizzare gli errori solo dopo l'esecuzione di un'operazione di immagine"
                    Case 1
                        Label11.Text = "Errori e avvisi (livello di registro 2)"
                        Label16.Text = "Il file di log deve visualizzare errori e avvisi dopo l'esecuzione di un'operazione di immagine"
                    Case 2
                        Label11.Text = "Errori, avvisi e messaggi informativi (livello di registro 3)"
                        Label16.Text = "Il file di log deve visualizzare errori, avvisi e messaggi informativi dopo l'esecuzione di un'operazione di immagine."
                    Case 3
                        Label11.Text = "Errori, avvisi, informazioni e messaggi di debug (livello di registro 4)"
                        Label16.Text = "Il file di log deve visualizzare errori, avvisi, informazioni e messaggi di debug dopo l'esecuzione di un'operazione di immagine."
                End Select
        End Select
        MainForm.LogLevel = TrackBar1.Value + 1
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        IncompleteSetupDlg.ShowDialog()
        If IncompleteSetupDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Close()
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            SecProgressStylePreview.Image = My.Resources.secprogress_modern
        Else
            SecProgressStylePreview.Image = My.Resources.secprogress_classic
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Panel8.Enabled = Not CheckBox1.Checked
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox2.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Logs\DISM\DISM.log"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If File.Exists(Application.StartupPath & "\update.exe") Then File.Delete(Application.StartupPath & "\update.exe")
        Try
            Using client As New WebClient()
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                client.DownloadFile("https://github.com/CodingWonders/DISMTools/raw/" & MainForm.dtBranch & "/Updater/DISMTools-UCS/update-bin/update.exe", Application.StartupPath & "\update.exe")
            End Using
        Catch ex As WebException
            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                Case "ENU", "ENG"
                    MsgBox("We couldn't download the update checker. Reason:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Check for updates")
                Case "ESN"
                    MsgBox("No pudimos descargar el comprobador de actualizaciones. Razón:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Comprobar actualizaciones")
                Case "FRA"
                    MsgBox("Nous n'avons pas pu télécharger le vérificateur de mise à jour. Raison :" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Mettre à jour les données")
                Case "PTB", "PTG"
                    MsgBox("Não foi possível descarregar o verificador de actualizações. Motivo:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Verificar actualizações")
                Case "ITA"
                    MsgBox("Non è stato possibile scaricare il programma di controllo degli aggiornamenti. Motivo:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Verifica aggiornamenti")
            End Select
            Exit Sub
        End Try
        If File.Exists(Application.StartupPath & "\update.exe") Then
            Process.Start(Application.StartupPath & "\update.exe", "/" & MainForm.dtBranch)
            Next_Button.PerformClick()
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        HelpBrowserForm.WebBrowser1.Navigate(Application.StartupPath & "\docs\getting_started\start\index.html")
        HelpBrowserForm.MinimizeBox = True
        HelpBrowserForm.MaximizeBox = True
        HelpBrowserForm.Show()
    End Sub
End Class