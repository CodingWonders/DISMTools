Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Public Class ISOCreator

    Dim ImageInfoCollection As DismImageInfoCollection
    Dim ISOMsg As String = ""
    Dim progressMessages() As String = New String(2) {"Status", "Creating ISO file. This can take some time. Please wait...", "The ISO file has been created"}
    Dim success As Boolean

    Private Sub ISOCreator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        progressMessages(0) = "Status"
                        progressMessages(1) = "Creating ISO file. This can take some time. Please wait..."
                        progressMessages(2) = "The ISO file has been created"
                        Text = "Create an ISO file"
                        Label1.Text = Text
                        Label2.Text = "The ISO file creation wizard lets you quickly create a disc image file that you can use to test the changes made to your Windows image. ISO files created with this utility should be used only on Virtual Machines (VMs) and on computers with standard disk controllers." & CrLf & CrLf &
                                      "A custom Preinstallation Environment (PE) will be created. This environment will automatically perform disk configuration and apply the image you specify here."
                        Label3.Text = "Once you're ready, click the Create button."
                        Label4.Text = "Image file to add to ISO file:"
                        Label5.Text = "Environment architecture:"
                        Label6.Text = "Architecture:"
                        Label7.Text = "Target ISO location:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "You can do other things while the ISO is being created. Come back here anytime for an updated status."
                        Button1.Text = "Browse..."
                        Button2.Text = "Pick..."
                        Button3.Text = "Browse..."
                        OK_Button.Text = "Create"
                        Cancel_Button.Text = "Cancel"
                        GroupBox1.Text = "Options"
                        GroupBox2.Text = "Progress"
                    Case "ESN"
                        progressMessages(0) = "Estado"
                        progressMessages(1) = "Creando archivo ISO. Esto puede llevar algo de tiempo. Espere..."
                        progressMessages(2) = "El archivo ISO ha sido creado"
                        Text = "Crear un archivo ISO"
                        Label1.Text = Text
                        Label2.Text = "El asistente de creación de archivos ISO le permite crear un archivo de imagen de disco rápidamente y que puede utilizar para probar los cambios hechos a su imagen de Windows. Los archivos ISO creados solo deberían ser utilizados en máquinas virtuales y en ordenadores con controladores de disco estándares." & CrLf & CrLf &
                                      "Un Entorno de Preinstalación (PE) personalizado será creado. Este entorno realizará configuración del disco automáticamente y aplicará la imagen que especifique aquí."
                        Label3.Text = "Cuando esté listo, haga clic en Crear."
                        Label4.Text = "Archivo de imagen a añadir al archivo ISO:"
                        Label5.Text = "Arquitectura del entorno:"
                        Label6.Text = "Arquitectura:"
                        Label7.Text = "Ubicación del archivo ISO de destino:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "Puede hacer otras cosas mientras se crea el archivo ISO. Vuelva aquí para ver un estado actualizado."
                        Button1.Text = "Examinar..."
                        Button2.Text = "Escoger..."
                        Button3.Text = "Examinar..."
                        OK_Button.Text = "Crear"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox1.Text = "Opciones"
                        GroupBox2.Text = "Progreso"
                    Case "FRA"
                        progressMessages(0) = "Statut"
                        progressMessages(1) = "Création du fichier ISO en cours. Cela peut prendre un certain temps. Veuillez patienter..."
                        progressMessages(2) = "Le fichier ISO a été créé"
                        Text = "Créer un fichier ISO"
                        Label1.Text = Text
                        Label2.Text = "L'assistant de création de fichier ISO vous permet de créer rapidement un fichier image de disque que vous pouvez utiliser pour tester les modifications apportées à votre image Windows. Les fichiers ISO créés à l'aide de cet utilitaire ne doivent être utilisés que sur des machines virtuelles (VM) et des ordinateurs dotés de contrôleurs de disque standard." & CrLf & CrLf &
                                      "Un environnement de préinstallation (PE) personnalisé sera créé. Cet environnement effectuera automatiquement la configuration du disque et appliquera l'image que vous spécifiez ici."
                        Label3.Text = "Lorsque vous êtes prêt, cliquez sur le bouton Créer."
                        Label4.Text = "Fichier image à ajouter au fichier ISO :"
                        Label5.Text = "Architecture de l'environnement :"
                        Label6.Text = "Architecture :"
                        Label7.Text = "Emplacement ISO cible :"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "Vous pouvez faire d'autres choses pendant la création de l'ISO. Revenez ici à tout moment pour obtenir une mise à jour de l'état."
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Choisir..."
                        Button3.Text = "Parcourir..."
                        OK_Button.Text = "Créer"
                        Cancel_Button.Text = "Annuler"
                        GroupBox1.Text = "Paramètres"
                        GroupBox2.Text = "Progrès"
                    Case "PTB", "PTG"
                        progressMessages(0) = "Estado"
                        progressMessages(1) = "A criar ficheiro ISO. Isto pode demorar algum tempo. Por favor, aguarde..."
                        progressMessages(2) = "O ficheiro ISO foi criado"
                        Text = "Criar um ficheiro ISO"
                        Label1.Text = Text
                        Label2.Text = "O assistente de criação de ficheiros ISO permite-lhe criar rapidamente um ficheiro de imagem de disco que pode utilizar para testar as alterações efectuadas à sua imagem do Windows. Os ficheiros ISO criados com este utilitário só devem ser utilizados em Máquinas Virtuais (VMs) e computadores com controladores de disco padrão." & CrLf & CrLf &
                                      "Será criado um ambiente de pré-instalação (PE) personalizado. Este ambiente irá efetuar automaticamente a configuração do disco e aplicar a imagem que especificar aqui."
                        Label3.Text = "Quando estiver pronto, clique no botão Criar."
                        Label4.Text = "Ficheiro de imagem a adicionar ao ficheiro ISO:"
                        Label5.Text = "Arquitetura do entorno:"
                        Label6.Text = "Arquitetura:"
                        Label7.Text = "Localização ISO de destino:"
                        Label8.Text = progressMessages(0)
                        Label9.Text = "Pode fazer outras coisas enquanto o ISO está a ser criado. Volte aqui em qualquer altura para obter um estado atualizado."
                        Button1.Text = "Navegar..."
                        Button2.Text = "Escolher..."
                        Button3.Text = "Procurar..."
                        OK_Button.Text = "Criar"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox1.Text = "Configurações"
                        GroupBox2.Text = "Progresso"
                End Select
            Case 1
                progressMessages(0) = "Status"
                progressMessages(1) = "Creating ISO file. This can take some time. Please wait..."
                progressMessages(2) = "The ISO file has been created"
                Text = "Create an ISO file"
                Label1.Text = Text
                Label2.Text = "The ISO file creation wizard lets you quickly create a disc image file that you can use to test the changes made to your Windows image. ISO files created with this utility should be used only on Virtual Machines (VMs) and computers with standard disk controllers." & CrLf & CrLf &
                              "A custom Preinstallation Environment (PE) will be created. This environment will automatically perform disk configuration and apply the image you specify here."
                Label3.Text = "Once you're ready, click the Create button."
                Label4.Text = "Image file to add to ISO file:"
                Label5.Text = "Environment architecture:"
                Label6.Text = "Architecture:"
                Label7.Text = "Target ISO location:"
                Label8.Text = progressMessages(0)
                Label9.Text = "You can do other things while the ISO is being created. Come back here anytime for an updated status."
                Button1.Text = "Browse..."
                Button2.Text = "Pick..."
                Button3.Text = "Browse..."
                OK_Button.Text = "Create"
                Cancel_Button.Text = "Cancel"
                GroupBox1.Text = "Options"
                GroupBox2.Text = "Progress"
            Case 2
                progressMessages(0) = "Estado"
                progressMessages(1) = "Creando archivo ISO. Esto puede llevar algo de tiempo. Espere..."
                progressMessages(2) = "El archivo ISO ha sido creado"
                Text = "Crear un archivo ISO"
                Label1.Text = Text
                Label2.Text = "El asistente de creación de archivos ISO le permite crear un archivo de imagen de disco rápidamente y que puede utilizar para probar los cambios hechos a su imagen de Windows. Los archivos ISO creados solo deberían ser utilizados en máquinas virtuales y en ordenadores con controladores de disco estándares." & CrLf & CrLf &
                              "Un Entorno de Preinstalación (PE) personalizado será creado. Este entorno realizará configuración del disco automáticamente y aplicará la imagen que especifique aquí."
                Label3.Text = "Cuando esté listo, haga clic en Crear."
                Label4.Text = "Archivo de imagen a añadir al archivo ISO:"
                Label5.Text = "Arquitectura del entorno:"
                Label6.Text = "Arquitectura:"
                Label7.Text = "Ubicación del archivo ISO de destino:"
                Label8.Text = progressMessages(0)
                Label9.Text = "Puede hacer otras cosas mientras se crea el archivo ISO. Vuelva aquí para ver un estado actualizado."
                Button1.Text = "Examinar..."
                Button2.Text = "Escoger..."
                Button3.Text = "Examinar..."
                OK_Button.Text = "Crear"
                Cancel_Button.Text = "Cancelar"
                GroupBox1.Text = "Opciones"
                GroupBox2.Text = "Progreso"
            Case 3
                progressMessages(0) = "Statut"
                progressMessages(1) = "Création du fichier ISO en cours. Cela peut prendre un certain temps. Veuillez patienter..."
                progressMessages(2) = "Le fichier ISO a été créé"
                Text = "Créer un fichier ISO"
                Label1.Text = Text
                Label2.Text = "L'assistant de création de fichier ISO vous permet de créer rapidement un fichier image de disque que vous pouvez utiliser pour tester les modifications apportées à votre image Windows. Les fichiers ISO créés à l'aide de cet utilitaire ne doivent être utilisés que sur des machines virtuelles (VM) et des ordinateurs dotés de contrôleurs de disque standard." & CrLf & CrLf &
                              "Un environnement de préinstallation (PE) personnalisé sera créé. Cet environnement effectuera automatiquement la configuration du disque et appliquera l'image que vous spécifiez ici."
                Label3.Text = "Lorsque vous êtes prêt, cliquez sur le bouton Créer."
                Label4.Text = "Fichier image à ajouter au fichier ISO :"
                Label5.Text = "Architecture de l'environnement :"
                Label6.Text = "Architecture :"
                Label7.Text = "Emplacement ISO cible :"
                Label8.Text = progressMessages(0)
                Label9.Text = "Vous pouvez faire d'autres choses pendant la création de l'ISO. Revenez ici à tout moment pour obtenir une mise à jour de l'état."
                Button1.Text = "Parcourir..."
                Button2.Text = "Choisir..."
                Button3.Text = "Parcourir..."
                OK_Button.Text = "Créer"
                Cancel_Button.Text = "Annuler"
                GroupBox1.Text = "Paramètres"
                GroupBox2.Text = "Progrès"
            Case 4
                progressMessages(0) = "Estado"
                progressMessages(1) = "A criar ficheiro ISO. Isto pode demorar algum tempo. Por favor, aguarde..."
                progressMessages(2) = "O ficheiro ISO foi criado"
                Text = "Criar um ficheiro ISO"
                Label1.Text = Text
                Label2.Text = "O assistente de criação de ficheiros ISO permite-lhe criar rapidamente um ficheiro de imagem de disco que pode utilizar para testar as alterações efectuadas à sua imagem do Windows. Os ficheiros ISO criados com este utilitário só devem ser utilizados em Máquinas Virtuais (VMs) e computadores com controladores de disco padrão." & CrLf & CrLf &
                              "Será criado um ambiente de pré-instalação (PE) personalizado. Este ambiente irá efetuar automaticamente a configuração do disco e aplicar a imagem que especificar aqui."
                Label3.Text = "Quando estiver pronto, clique no botão Criar."
                Label4.Text = "Ficheiro de imagem a adicionar ao ficheiro ISO:"
                Label5.Text = "Arquitetura do entorno:"
                Label6.Text = "Arquitetura:"
                Label7.Text = "Localização ISO de destino:"
                Label8.Text = progressMessages(0)
                Label9.Text = "Pode fazer outras coisas enquanto o ISO está a ser criado. Volte aqui em qualquer altura para obter um estado atualizado."
                Button1.Text = "Navegar..."
                Button2.Text = "Escolher..."
                Button3.Text = "Procurar..."
                OK_Button.Text = "Criar"
                Cancel_Button.Text = "Cancelar"
                GroupBox1.Text = "Configurações"
                GroupBox2.Text = "Progresso"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        GroupBox2.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Sub GetImageInfo(ImageFile As String)
        TextBox2.Text = ""
        If MainForm.MountedImageDetectorBW.IsBusy Then
            MainForm.MountedImageDetectorBW.CancelAsync()
            While MainForm.MountedImageDetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(500)
            End While
        End If
        MainForm.WatcherTimer.Enabled = False
        If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
        While MainForm.WatcherBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        Try
            DismApi.Initialize(DismLogLevel.LogErrors)
            ImageInfoCollection = DismApi.GetImageInfo(ImageFile)
            TextBox2.Text = "Images in selected file: " & ImageInfoCollection.Count & CrLf & CrLf
            For Each ImageInfo As DismImageInfo In ImageInfoCollection
                TextBox2.AppendText(" - Image " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " of " & ImageInfoCollection.Count & CrLf &
                                    "   - Image name: " & ImageInfo.ImageName & CrLf &
                                    "   - Image description: " & ImageInfo.ImageDescription & CrLf &
                                    "   - Image version: " & ImageInfo.ProductVersion.ToString() & CrLf & CrLf)
            Next
        Catch ex As Exception
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
                    End Select
                Case 1
                    msg = "Could not gather information of this image file. Reason:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 2
                    msg = "No pudimos obtener información de este archivo de imagen. Razón:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 3
                    msg = "Impossible de recueillir des informations sur ce fichier de l'image. Raison :" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 4
                    msg = "Não foi possível recolher informações sobre este ficheiro de imagem. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
        Finally
            DismApi.Shutdown()
        End Try
        Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox3.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
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
                    End Select
                Case 1
                    ISOMsg = "Either the source image file does not exist or you haven't provided any image file. Please specify a valid image file and try again."
                Case 2
                    ISOMsg = "El archivo de imagen de origen no existe o no ha especificado un archivo. Especifique un archivo de imagen válido e inténtelo de nuevo."
                Case 3
                    ISOMsg = "Soit le fichier image source n'existe pas, soit vous n'avez pas fourni de fichier image. Veuillez spécifier un fichier image valide et réessayer."
                Case 4
                    ISOMsg = "Ou o ficheiro de imagem de origem não existe ou não forneceu qualquer ficheiro de imagem. Especifique um ficheiro de imagem válido e tente novamente."
            End Select
            MsgBox(ISOMsg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        If TextBox3.Text = "" Then
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
                    End Select
                Case 1
                    ISOMsg = "The target ISO hasn't been specified. Please specify a location for the ISO file and try again."
                Case 2
                    ISOMsg = "El archivo ISO de destino no se ha especificado. Especifique una ubicación para el archivo ISO e inténtelo de nuevo."
                Case 3
                    ISOMsg = "L'ISO cible n'a pas été spécifiée. Veuillez indiquer un emplacement pour le fichier ISO et réessayez."
                Case 4
                    ISOMsg = "O ISO de destino não foi especificado. Especifique uma localização para o ficheiro ISO e tente novamente."
            End Select
            MsgBox(ISOMsg, vbOKOnly + vbCritical, Label1.Text)
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
                    End Select
                Case 1
                    ISOMsg = "The target ISO already exists. Do you want to replace it?"
                Case 2
                    ISOMsg = "El archivo ISO ya existe. ¿Desea reemplazarlo?"
                Case 3
                    ISOMsg = "L'ISO cible existe déjà. Voulez-vous la remplacer ?"
                Case 4
                    ISOMsg = "O ISO de destino já existe. Deseja substituí-la?"
            End Select
            If MsgBox(ISOMsg, vbYesNo + vbQuestion, Label1.Text) = MsgBoxResult.Yes Then
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
        Dim ISOCreator As New Process()
        ISOCreator.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        ISOCreator.StartInfo.WorkingDirectory = Application.StartupPath & "\bin\extps1\PE_Helper"
        ISOCreator.StartInfo.Arguments = "-executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\extps1\PE_Helper\PE_Helper.ps1" & Quote & " -cmd StartPEGen -arch " & ComboBox1.SelectedItem & " -imgFile " & Quote & TextBox1.Text & Quote & " -isoPath " & Quote & TextBox3.Text & Quote
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
                        msg = If(success, "The ISO file has been created successfully", "Failed to create the ISO file")
                    Case "ESN"
                        msg = If(success, "El archivo ISO ha sido creado satisfactoriamente", "No pudimos crear el archivo ISO")
                    Case "FRA"
                        msg = If(success, "Le fichier ISO a été créé avec succès", "Le processus de création de l'ISO a échoué")
                    Case "PTB", "PTG"
                        msg = If(success, "O ficheiro ISO foi criado com êxito", "O processo de criação do ISO falhou")
                End Select
            Case 1
                msg = If(success, "The ISO file has been created successfully", "Failed to create the ISO file")
            Case 2
                msg = If(success, "El archivo ISO ha sido creado satisfactoriamente", "No pudimos crear el archivo ISO")
            Case 3
                msg = If(success, "Le fichier ISO a été créé avec succès", "Le processus de création de l'ISO a échoué")
            Case 4
                msg = If(success, "O ficheiro ISO foi criado com êxito", "O processo de criação do ISO falhou")
        End Select
        MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
        GroupBox1.Enabled = True
        IdlePanel.Visible = True
        ISOProgressPanel.Visible = False
    End Sub

    Private Sub ISOCreator_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If BackgroundWorker1.IsBusy Then
            e.Cancel = True
            Beep()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PopupImageManager.Location = Button2.PointToScreen(Point.Empty)
        If PopupImageManager.ShowDialog() = DialogResult.OK Then
            TextBox1.Text = PopupImageManager.selectedImgFile
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            GetImageInfo(TextBox1.Text)
        End If
    End Sub
End Class