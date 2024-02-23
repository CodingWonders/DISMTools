Imports System.Windows.Forms

Public Class BGProcsAdvSettings

    Public NeedsDriverChecks As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        MainForm.ExtAppxGetter = CheckBox1.Checked
        MainForm.SkipNonRemovable = CheckBox2.Checked
        If Not MainForm.AllDrivers = CheckBox3.Checked Then NeedsDriverChecks = True Else NeedsDriverChecks = False
        MainForm.AllDrivers = CheckBox3.Checked
        MainForm.SkipFrameworks = CheckBox4.Checked
        MainForm.RunAllProcs = CheckBox5.Checked
        If CheckBox5.Checked Then
            MainForm.bwBackgroundProcessAction = 0
            MainForm.bwGetImageInfo = True
            MainForm.bwGetAdvImgInfo = True
        End If
        If (NeedsDriverChecks And MainForm.isProjectLoaded And (MainForm.IsImageMounted Or MainForm.OnlineManagement)) And Not MainForm.ImgBW.IsBusy Then
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "The program will now detect the drivers of the image according to the options you've specified. This may take some time."
                        Case "ESN"
                            msg = "El programa va a detectar los controladores de la imagen atendiendo a las opciones que ha especificado. Esto puede llevar un tiempo."
                        Case "FRA"
                            msg = "Le programme va maintenant détecter les pilotes de l'image en fonction des options que vous avez spécifiées. Cela peut prendre un certain temps."
                        Case "PTB", "PTG"
                            msg = "O programa irá agora detetar os controladores da imagem de acordo com as opções que especificou. Isto pode demorar algum tempo."
                    End Select
                Case 1
                    msg = "The program will now detect the drivers of the image according to the options you've specified. This may take some time."
                Case 2
                    msg = "El programa va a detectar los controladores de la imagen atendiendo a las opciones que ha especificado. Esto puede llevar un tiempo."
                Case 3
                    msg = "Le programme va maintenant détecter les pilotes de l'image en fonction des options que vous avez spécifiées. Cela peut prendre un certain temps."
                Case 4
                    msg = "O programa irá agora detetar os controladores da imagem de acordo com as opções que especificou. Isto pode demorar algum tempo."
            End Select
            MsgBox(msg, vbOKOnly + vbInformation, Text)
            MainForm.bwGetImageInfo = False
            MainForm.bwGetAdvImgInfo = False
            MainForm.bwBackgroundProcessAction = 5
            MainForm.UpdateProjProperties(True, False)
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BGProcsAdvSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Advanced background process settings"
                        Label1.Text = "Configure additional settings for background processes:"
                        CheckBox1.Text = "Enhance detection of all installed AppX packages of an active installation with PowerShell helpers"
                        CheckBox2.Text = "Skip packages with non-removable policies set"
                        CheckBox3.Text = "Detect all image drivers"
                        CheckBox4.Text = "Skip framework packages, and remove them from the listings if they were detected"
                        CheckBox5.Text = "Run all background processes after performing a task"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Configuraciones avanzadas de procesos en segundo plano"
                        Label1.Text = "Configure opciones adicionales para los procesos en segundo plano:"
                        CheckBox1.Text = "Mejorar la detección de todos los paquetes AppX instalados en una instalación activa con ayudantes de PowerShell"
                        CheckBox2.Text = "Omitir paquetes no removibles"
                        CheckBox3.Text = "Detectar todos los controladores de la imagen"
                        CheckBox4.Text = "Omitir paquetes de marcos de trabajo, y eliminarlos de los listados si fueron detectados"
                        CheckBox5.Text = "Ejecutar todos los procesos en segundo plano tras realizar una operación"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Paramètres avancés des processus en arrière plan"
                        Label1.Text = "Configurer des paramètres supplémentaires pour les processus en arrière plan :"
                        CheckBox1.Text = "Améliorer la détection de tous les paquets AppX installés dans une installation active grâce aux aides PowerShell"
                        CheckBox2.Text = "Sauter les paquets dont les politiques ne sont pas supprimées"
                        CheckBox3.Text = "Détecter tous les pilotes de l'image"
                        CheckBox4.Text = "Ignorer les paquets cadres et les supprimer de la liste s'ils ont été détectés."
                        CheckBox5.Text = "Exécuter tous les processus en arrière plan après l'exécution d'une tâche"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Text = "Configurações avançadas de processos em segundo plano"
                        Label1.Text = "Configurar definições adicionais para processos em segundo plano:"
                        CheckBox1.Text = "Melhorar a deteção de todos os pacotes AppX instalados de uma instalação ativa com ajudantes do PowerShell"
                        CheckBox2.Text = "Ignorar pacotes com políticas não removíveis definidas"
                        CheckBox3.Text = "Detetar todos os controladores de imagem"
                        CheckBox4.Text = "Ignorar pacotes de estrutura e removê-los das listagens se tiverem sido detectados"
                        CheckBox5.Text = "Executar todos os processos em segundo plano depois de executar uma tarefa"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                End Select
            Case 1
                Text = "Advanced background process settings"
                Label1.Text = "Configure additional settings for background processes:"
                CheckBox1.Text = "Enhance detection of all installed AppX packages of an active installation with PowerShell helpers"
                CheckBox2.Text = "Skip packages with non-removable policies set"
                CheckBox3.Text = "Detect all image drivers"
                CheckBox4.Text = "Skip framework packages, and remove them from the listings if they were detected"
                CheckBox5.Text = "Run all background processes after performing a task"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Configuraciones avanzadas de procesos en segundo plano"
                Label1.Text = "Configure opciones adicionales para los procesos en segundo plano:"
                CheckBox1.Text = "Mejorar la detección de todos los paquetes AppX instalados en una instalación activa con ayudantes de PowerShell"
                CheckBox2.Text = "Omitir paquetes no removibles"
                CheckBox3.Text = "Detectar todos los controladores de la imagen"
                CheckBox4.Text = "Omitir paquetes de marcos de trabajo, y eliminarlos de los listados si fueron detectados"
                CheckBox5.Text = "Ejecutar todos los procesos en segundo plano tras realizar una operación"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Paramètres avancés des processus en arrière plan"
                Label1.Text = "Configurer des paramètres supplémentaires pour les processus en arrière plan :"
                CheckBox1.Text = "Améliorer la détection de tous les paquets AppX installés dans une installation active grâce aux aides PowerShell"
                CheckBox2.Text = "Sauter les paquets dont les politiques ne sont pas supprimées"
                CheckBox3.Text = "Détecter tous les pilotes de l'image"
                CheckBox4.Text = "Ignorer les paquets cadres et les supprimer de la liste s'ils ont été détectés."
                CheckBox5.Text = "Exécuter tous les processus en arrière plan après l'exécution d'une tâche"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case 4
                Text = "Configurações avançadas de processos em segundo plano"
                Label1.Text = "Configurar definições adicionais para processos em segundo plano:"
                CheckBox1.Text = "Melhorar a deteção de todos os pacotes AppX instalados de uma instalação ativa com ajudantes do PowerShell"
                CheckBox2.Text = "Ignorar pacotes com políticas não removíveis definidas"
                CheckBox3.Text = "Detetar todos os controladores de imagem"
                CheckBox4.Text = "Ignorar pacotes de estrutura e removê-los das listagens se tiverem sido detectados"
                CheckBox5.Text = "Executar todos os processos em segundo plano depois de executar uma tarefa"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        CheckBox1.Checked = MainForm.ExtAppxGetter
        CheckBox2.Checked = MainForm.SkipNonRemovable
        CheckBox3.Checked = MainForm.AllDrivers
        CheckBox4.Checked = MainForm.SkipFrameworks
        CheckBox5.Checked = MainForm.RunAllProcs
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        CheckBox2.Enabled = CheckBox1.Checked
        CheckBox4.Enabled = CheckBox1.Checked
    End Sub
End Class
