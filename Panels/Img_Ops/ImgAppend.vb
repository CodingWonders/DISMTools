Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgAppend

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim msg As String = ""
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If TextBox1.Text = "" Or Not Directory.Exists(TextBox1.Text) Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Please specify a source image directory and try again."
                        Case "ESN"
                            msg = "Por favor, especifique un directorio de imagen de origen e inténtelo de nuevo."
                        Case "FRA"
                            msg = "Veuillez indiquer un répertoire d'images source et réessayer."
                        Case "PTB", "PTG"
                            msg = "Especifique um diretório de imagens de origem e tente novamente."
                        Case "ITA"
                            msg = "Specificare una directory di origine dell'immagine e riprovare."
                    End Select
                Case 1
                    msg = "Please specify a source image directory and try again."
                Case 2
                    msg = "Por favor, especifique un directorio de imagen de origen e inténtelo de nuevo."
                Case 3
                    msg = "Veuillez indiquer un répertoire d'images source et réessayer."
                Case 4
                    msg = "Especifique um diretório de imagens de origem e tente novamente."
                Case 5
                    msg = "Specificare una directory di origine dell'immagine e riprovare."
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        Else
            ProgressPanel.AppendixSourceDir = TextBox1.Text
        End If
        If TextBox2.Text = "" Or Not File.Exists(TextBox2.Text) Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Please specify a destination image file and try again."
                        Case "ESN"
                            msg = "Por favor, especifique un archivo de imagen de destino e inténtelo de nuevo."
                        Case "FRA"
                            msg = "Veuillez indiquer un fichier image de destination et réessayer."
                        Case "PTB", "PTG"
                            msg = "Especifique um ficheiro de imagem de destino e tente novamente."
                        Case "ITA"
                            msg = "Specificare un file immagine di destinazione e riprovare."
                    End Select
                Case 1
                    msg = "Please specify a destination image file and try again."
                Case 2
                    msg = "Por favor, especifique un archivo de imagen de destino e inténtelo de nuevo."
                Case 3
                    msg = "Veuillez indiquer un fichier image de destination et réessayer."
                Case 4
                    msg = "Especifique um ficheiro de imagem de destino e tente novamente."
                Case 5
                    msg = "Specificare un file immagine di destinazione e riprovare."
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        Else
            ProgressPanel.AppendixDestinationImage = TextBox2.Text
        End If
        If TextBox3.Text = "" Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Please specify a name for the destination image file and try again."
                        Case "ESN"
                            msg = "Por favor, especifique un nombre para el archivo de imagen de destino e inténtelo de nuevo."
                        Case "FRA"
                            msg = "Veuillez indiquer un nom pour le fichier image de destination et réessayer."
                        Case "PTB", "PTG"
                            msg = "Especifique um nome para o ficheiro de imagem de destino e tente novamente."
                        Case "ITA"
                            msg = "Specificare un nome per il file immagine di destinazione e riprovare."
                    End Select
                Case 1
                    msg = "Please specify a name for the destination image file and try again."
                Case 2
                    msg = "Por favor, especifique un nombre para el archivo de imagen de destino e inténtelo de nuevo."
                Case 3
                    msg = "Veuillez indiquer un nom pour le fichier image de destination et réessayer."
                Case 4
                    msg = "Especifique um nome para o ficheiro de imagem de destino e tente novamente."
                Case 5
                    msg = "Specificare un nome per il file immagine di destinazione e riprovare."
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        Else
            ProgressPanel.AppendixName = TextBox3.Text
        End If
        ProgressPanel.AppendixDescription = TextBox4.Text
        If CheckBox1.Checked Then
            If TextBox5.Text <> "" And File.Exists(TextBox5.Text) Then
                ProgressPanel.AppendixWimScriptConfig = TextBox5.Text
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg = "Either no configuration list file has been specified or the configuration list file could not be detected in your file system. Would you like to continue without any configuration list file?"
                            Case "ESN"
                                msg = "Ningún archivo de lista de configuración fue especificado, o no se pudo detectar en su sistema de archivos. ¿Desea continuar sin un archivo de lista de configuración?"
                            Case "FRA"
                                msg = "Soit aucun fichier de liste de configuration n'a été spécifié, soit le fichier de liste de configuration n'a pas pu être détecté dans votre système de fichiers. Souhaitez-vous continuer sans fichier de liste de configuration ?"
                            Case "PTB", "PTG"
                                msg = "Ou não foi especificado nenhum ficheiro de lista de configuração ou o ficheiro de lista de configuração não foi detectado no seu sistema de ficheiros. Deseja continuar sem qualquer ficheiro de lista de configuração?"
                            Case "ITA"
                                msg = "Non è stato specificato alcun file dell'elenco di configurazione oppure non è stato possibile rilevare il file dell'elenco di configurazione nel file system. Si desidera continuare senza alcun file dell'elenco di configurazione?"
                        End Select
                    Case 1
                        msg = "Either no configuration list file has been specified or the configuration list file could not be detected in your file system. Would you like to continue without any configuration list file?"
                    Case 2
                        msg = "Ningún archivo de lista de configuración fue especificado, o no se pudo detectar en su sistema de archivos. ¿Desea continuar sin un archivo de lista de configuración?"
                    Case 3
                        msg = "Soit aucun fichier de liste de configuration n'a été spécifié, soit le fichier de liste de configuration n'a pas pu être détecté dans votre système de fichiers. Souhaitez-vous continuer sans fichier de liste de configuration ?"
                    Case 4
                        msg = "Ou não foi especificado nenhum ficheiro de lista de configuração ou o ficheiro de lista de configuração não foi detectado no seu sistema de ficheiros. Deseja continuar sem qualquer ficheiro de lista de configuração?"
                    Case 5
                        msg = "Non è stato specificato alcun file dell'elenco di configurazione oppure non è stato possibile rilevare il file dell'elenco di configurazione nel file system. Si desidera continuare senza alcun file dell'elenco di configurazione?"
                End Select
                If MsgBox(msg, vbYesNo + vbCritical, Label1.Text) = MsgBoxResult.Ok Then
                    ProgressPanel.AppendixWimScriptConfig = ""
                Else
                    Exit Sub
                End If
            End If
        Else
            ProgressPanel.AppendixWimScriptConfig = ""
        End If
        ProgressPanel.AppendixUseWimBoot = CheckBox2.Checked
        ProgressPanel.AppendixBootable = CheckBox3.Checked
        ProgressPanel.AppendixCheckIntegrity = CheckBox4.Checked
        ProgressPanel.AppendixVerify = CheckBox5.Checked
        ProgressPanel.AppendixReparsePt = CheckBox6.Checked
        ProgressPanel.AppendixCaptureExtendedAttribs = CheckBox7.Checked
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 1
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgAppend_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Append to an image"
                        Label1.Text = Text
                        Label2.Text = "Path of configuration file:"
                        Label3.Text = "Source image directory:"
                        Label5.Text = "Destination image description:"
                        Label6.Text = "Destination image file:"
                        Label7.Text = "Destination image name:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        Button1.Text = "Browse..."
                        Button2.Text = "Browse..."
                        Button3.Text = "Browse..."
                        Button5.Text = "Create..."
                        CheckBox1.Text = "Exclude certain files and directories for destination image"
                        CheckBox2.Text = "Append with WIMBoot configuration"
                        CheckBox3.Text = "Make image bootable (Windows PE only)"
                        CheckBox4.Text = "Verify image integrity"
                        CheckBox5.Text = "Check for file errors"
                        CheckBox6.Text = "Use the reparse point tag fix"
                        CheckBox7.Text = "Capture extended attributes"
                        GroupBox1.Text = "Sources and destinations"
                        GroupBox2.Text = "Options"
                    Case "ESN"
                        Text = "Anexar a una imagen"
                        Label1.Text = Text
                        Label2.Text = "Ruta del archivo de configuración:"
                        Label3.Text = "Directorio de la imagen de origen:"
                        Label5.Text = "Descripción de la imagen de destino:"
                        Label6.Text = "Archivo de imagen de destino:"
                        Label7.Text = "Nombre de la imagen de destino:"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Examinar..."
                        Button3.Text = "Examinar..."
                        Button5.Text = "Crear..."
                        CheckBox1.Text = "Excluir algunos archivos y directorios para la imagen de destino"
                        CheckBox2.Text = "Anexar con configuración WIMBoot"
                        CheckBox3.Text = "Hacer imagen arrancable (solo Windows PE)"
                        CheckBox4.Text = "Verificar integridad de imagen"
                        CheckBox5.Text = "Comprobar errores de archivos"
                        CheckBox6.Text = "Utilizar corrección de etiquetas de puntos de repetición de análisis"
                        CheckBox7.Text = "Capturar atributos extendidos"
                        GroupBox1.Text = "Orígenes y destinos"
                        GroupBox2.Text = "Opciones"
                    Case "FRA"
                        Text = "Ajouter à une image"
                        Label1.Text = Text
                        Label2.Text = "Chemin du fichier de configuration :"
                        Label3.Text = "Répertoire de l'image source :"
                        Label5.Text = "Description de l'image de destination :"
                        Label6.Text = "Fichier de l'image de destination :"
                        Label7.Text = "Nom de l'image de destination :"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Parcourir..."
                        Button3.Text = "Parcourir..."
                        Button5.Text = "Créer..."
                        CheckBox1.Text = "Exclure certains fichiers et répertoires pour l'image de destination"
                        CheckBox2.Text = "Ajouter la configuration WIMBoot"
                        CheckBox3.Text = "Rendre l'image amorçable (Windows PE uniquement)"
                        CheckBox4.Text = "Vérifier l'intégrité de l'image"
                        CheckBox5.Text = "Rechercher les erreurs de fichiers"
                        CheckBox6.Text = "Utiliser la correction de la balise reparse"
                        CheckBox7.Text = "Capturer les attributs étendus"
                        GroupBox1.Text = "Sources et destinations"
                        GroupBox2.Text = "Paramètres"
                    Case "PTB", "PTG"
                        Text = "Anexar a uma imagem"
                        Label1.Text = Text
                        Label2.Text = "Localização do ficheiro de configuração:"
                        Label3.Text = "Diretório da imagem de origem:"
                        Label5.Text = "Descrição da imagem de destino:"
                        Label6.Text = "Ficheiro de imagem de destino:"
                        Label7.Text = "Nome da imagem de destino:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Navegar..."
                        Button2.Text = "Procurar..."
                        Button3.Text = "Procurar..."
                        Button5.Text = "Criar..."
                        CheckBox1.Text = "Excluir determinados ficheiros e directórios para a imagem de destino"
                        CheckBox2.Text = "Anexar com a configuração WIMBoot"
                        CheckBox3.Text = "Tornar a imagem de arranque (apenas Windows PE)"
                        CheckBox4.Text = "Verificar a integridade da imagem"
                        CheckBox5.Text = "Verificar se existem erros nos ficheiros"
                        CheckBox6.Text = "Utilizar a correção da etiqueta de ponto de reparação"
                        CheckBox7.Text = "Capturar atributos alargados"
                        GroupBox1.Text = "Origens e destinos"
                        GroupBox2.Text = "Opções"
                    Case "ITA"
                        Text = "Aggiungi a un'immagine"
                        Label1.Text = Text
                        Label2.Text = "Percorso del file di configurazione:"
                        Label3.Text = " Cartella dell'immagine di origine:"
                        Label5.Text = "Descrizione immagine di destinazione:"
                        Label6.Text = "File immagine di destinazione:"
                        Label7.Text = "Nome immagine di destinazione:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        Button1.Text = "Sfoglia..."
                        Button2.Text = "Sfoglia..."
                        Button3.Text = "Sfogliare..."
                        Button5.Text = "Crea..."
                        CheckBox1.Text = "Escludi determinati file e cartelle per l'immagine di destinazione"
                        CheckBox2.Text = "Aggiungi con la configurazione WIMBoot"
                        CheckBox3.Text = "Rendi l'immagine avviabile (solo Windows PE)"
                        CheckBox4.Text = "Verifica l'integrità dell'immagine"
                        CheckBox5.Text = "Controlla gli errori dei file"
                        CheckBox6.Text = "Utilizza la correzione del tag del punto di reparse"
                        CheckBox7.Text = "Cattura attributi estesi"
                        GroupBox1.Text = "Sorgenti e destinazioni"
                        GroupBox2.Text = "Opzioni"
                End Select
            Case 1
                Text = "Append to an image"
                Label1.Text = Text
                Label2.Text = "Path of configuration file:"
                Label3.Text = "Source image directory:"
                Label5.Text = "Destination image description:"
                Label6.Text = "Destination image file:"
                Label7.Text = "Destination image name:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                Button1.Text = "Browse..."
                Button2.Text = "Browse..."
                Button3.Text = "Browse..."
                Button5.Text = "Create..."
                CheckBox1.Text = "Exclude certain files and directories for destination image"
                CheckBox2.Text = "Append with WIMBoot configuration"
                CheckBox3.Text = "Make image bootable (Windows PE only)"
                CheckBox4.Text = "Verify image integrity"
                CheckBox5.Text = "Check for file errors"
                CheckBox6.Text = "Use the reparse point tag fix"
                CheckBox7.Text = "Capture extended attributes"
                GroupBox1.Text = "Sources and destinations"
                GroupBox2.Text = "Options"
            Case 2
                Text = "Anexar a una imagen"
                Label1.Text = Text
                Label2.Text = "Ruta del archivo de configuración:"
                Label3.Text = "Directorio de la imagen de origen:"
                Label5.Text = "Descripción de la imagen de destino:"
                Label6.Text = "Archivo de imagen de destino:"
                Label7.Text = "Nombre de la imagen de destino:"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Examinar..."
                Button2.Text = "Examinar..."
                Button3.Text = "Examinar..."
                Button5.Text = "Crear..."
                CheckBox1.Text = "Excluir algunos archivos y directorios para la imagen de destino"
                CheckBox2.Text = "Anexar con configuración WIMBoot"
                CheckBox3.Text = "Hacer imagen arrancable (solo Windows PE)"
                CheckBox4.Text = "Verificar integridad de imagen"
                CheckBox5.Text = "Comprobar errores de archivos"
                CheckBox6.Text = "Utilizar corrección de etiquetas de puntos de repetición de análisis"
                CheckBox7.Text = "Capturar atributos extendidos"
                GroupBox1.Text = "Orígenes y destinos"
                GroupBox2.Text = "Opciones"
            Case 3
                Text = "Ajouter à une image"
                Label1.Text = Text
                Label2.Text = "Chemin du fichier de configuration :"
                Label3.Text = "Répertoire de l'image source :"
                Label5.Text = "Description de l'image de destination :"
                Label6.Text = "Fichier de l'image de destination :"
                Label7.Text = "Nom de l'image de destination :"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                Button1.Text = "Parcourir..."
                Button2.Text = "Parcourir..."
                Button3.Text = "Parcourir..."
                Button5.Text = "Créer..."
                CheckBox1.Text = "Exclure certains fichiers et répertoires pour l'image de destination"
                CheckBox2.Text = "Ajouter la configuration WIMBoot"
                CheckBox3.Text = "Rendre l'image amorçable (Windows PE uniquement)"
                CheckBox4.Text = "Vérifier l'intégrité de l'image"
                CheckBox5.Text = "Rechercher les erreurs de fichiers"
                CheckBox6.Text = "Utiliser la correction de la balise reparse"
                CheckBox7.Text = "Capturer les attributs étendus"
                GroupBox1.Text = "Sources et destinations"
                GroupBox2.Text = "Paramètres"
            Case 4
                Text = "Anexar a uma imagem"
                Label1.Text = Text
                Label2.Text = "Localização do ficheiro de configuração:"
                Label3.Text = "Diretório da imagem de origem:"
                Label5.Text = "Descrição da imagem de destino:"
                Label6.Text = "Ficheiro de imagem de destino:"
                Label7.Text = "Nome da imagem de destino:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Navegar..."
                Button2.Text = "Procurar..."
                Button3.Text = "Procurar..."
                Button5.Text = "Criar..."
                CheckBox1.Text = "Excluir determinados ficheiros e directórios para a imagem de destino"
                CheckBox2.Text = "Anexar com a configuração WIMBoot"
                CheckBox3.Text = "Tornar a imagem de arranque (apenas Windows PE)"
                CheckBox4.Text = "Verificar a integridade da imagem"
                CheckBox5.Text = "Verificar se existem erros nos ficheiros"
                CheckBox6.Text = "Utilizar a correção da etiqueta de ponto de reparação"
                CheckBox7.Text = "Capturar atributos alargados"
                GroupBox1.Text = "Origens e destinos"
                GroupBox2.Text = "Opções"
            Case 5
                Text = "Aggiungi a un'immagine"
                Label1.Text = Text
                Label2.Text = "Percorso del file di configurazione:"
                Label3.Text = " Cartella dell'immagine di origine:"
                Label5.Text = "Descrizione immagine di destinazione:"
                Label6.Text = "File immagine di destinazione:"
                Label7.Text = "Nome immagine di destinazione:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                Button1.Text = "Sfoglia..."
                Button2.Text = "Sfoglia..."
                Button3.Text = "Sfogliare..."
                Button5.Text = "Crea..."
                CheckBox1.Text = "Escludi determinati file e cartelle per l'immagine di destinazione"
                CheckBox2.Text = "Aggiungi con la configurazione WIMBoot"
                CheckBox3.Text = "Rendi l'immagine avviabile (solo Windows PE)"
                CheckBox4.Text = "Verifica l'integrità dell'immagine"
                CheckBox5.Text = "Controlla gli errori dei file"
                CheckBox6.Text = "Utilizza la correzione del tag del punto di reparse"
                CheckBox7.Text = "Cattura attributi estesi"
                GroupBox1.Text = "Sorgenti e destinazioni"
                GroupBox2.Text = "Opzioni"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
            TextBox4.BackColor = Color.FromArgb(31, 31, 31)
            TextBox5.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
            TextBox4.BackColor = Color.FromArgb(238, 238, 242)
            TextBox5.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
        End If
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        TextBox5.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label2.Enabled = True
            TextBox5.Enabled = True
            Button3.Enabled = True
            Button5.Enabled = True
        Else
            Label2.Enabled = False
            TextBox5.Enabled = False
            Button3.Enabled = False
            Button5.Enabled = False
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox5.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Visible = False
        ' Make it so that it can only close
        WimScriptEditor.MinimizeBox = False
        WimScriptEditor.MaximizeBox = False
        WimScriptEditor.ShowDialog(MainForm)
        If File.Exists(WimScriptEditor.ConfigListFile) Then
            TextBox5.Text = WimScriptEditor.ConfigListFile
        End If
        Visible = True
    End Sub

    Function GetLastImageName() As String
        Dim imageName As String = ""
        Try
            DismApi.Initialize(DismLogLevel.LogErrors)
            Dim ImageInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(TextBox2.Text)
            imageName = ImageInfoCollection.Last.ImageName
        Catch ex As Exception
            MsgBox("Could not grab last image name. Error information:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, Label1.Text)
        Finally
            Try
                DismApi.Shutdown()
            Catch ex As Exception
                ' Don't do anything
            End Try
        End Try
        Return imageName
    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If TextBox2.Text = "" OrElse Not File.Exists(TextBox2.Text) Then Exit Sub
        If MainForm.MountedImageDetectorBW.IsBusy Then
            MainForm.MountedImageDetectorBW.CancelAsync()
            While MainForm.MountedImageDetectorBW.IsBusy
                Application.DoEvents()
                Threading.Thread.Sleep(500)
            End While
        End If
        MainForm.WatcherTimer.Enabled = False
        If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
        While MainForm.WatcherBW.IsBusy
            Application.DoEvents()
            Threading.Thread.Sleep(100)
        End While
        TextBox3.Text = GetLastImageName()
        Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub

    Private Sub Button4_MouseHover(sender As Object, e As EventArgs) Handles Button4.MouseHover
        ToolTip1.SetToolTip(sender, "Grab the name of the last index of the target image")
    End Sub
End Class
