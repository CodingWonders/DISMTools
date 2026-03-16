Imports System.Windows.Forms
Imports DISMTools.Elements
Imports System.IO
Imports Microsoft.Dism

Public Class SetImageEdition
    Implements IImageTaskDialog

    Public TargetEditions As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.imgEditionNewEdition = ComboBox1.SelectedItem
        If MainForm.CurrentImage.ImageInstallationType.ToLower().Contains("server") AndAlso MainForm.OnlineManagement Then
            ProgressPanel.imgEditionCopyEula = RadioButton1.Checked
            ProgressPanel.imgEditionAcceptEula = RadioButton2.Checked
            If RadioButton1.Checked Then
                If (TextBox1.Text = "" Or Not Directory.Exists(TextBox1.Text)) Then
                    MsgBox("Either no directory has been specified or it does not exist.", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
                    Exit Sub
                End If
                ProgressPanel.imgEditionEulaDestination = TextBox1.Text
            Else
                Dim productKey As ProductKey = ProductKeyValidator.ValidateProductKey(TextBox2.Text)
                If Not productKey.Valid Then
                    MsgBox("The product key has been typed incorrectly", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
                    Exit Sub
                End If
                ProgressPanel.imgEditionEditionKey = productKey.Key
            End If
        Else
            ProgressPanel.imgEditionCopyEula = False
            ProgressPanel.imgEditionAcceptEula = False
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 71
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Preparing to get target editions...")
        TargetEditions.Clear()
        DynaLog.LogMessage("Getting target editions...")
        Dim msg As String = ""
        Try
            DynaLog.LogMessage("Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            DynaLog.LogMessage("Creating session...")
            Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                Dim imageTargetEditions As DismEditionCollection = DismApi.GetTargetEditions(imgSession)
                DynaLog.LogMessage("Amount of target editions: " & imageTargetEditions.Count)
                If imageTargetEditions.Count > 0 Then
                    ' This image hasn't been upgraded to its highest edition
                    DynaLog.LogMessage("There are target editions. This image can give a little more")
                    For Each targetEdition In imageTargetEditions
                        TargetEditions.Add(targetEdition)
                    Next
                Else
                    ' This image has been upgraded to its highest edition
                    DynaLog.LogMessage("There are no target editions. This image is already rocking the best edition")
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "This image cannot be upgraded to higher editions because it is in its highest edition"
                                Case "ESN"
                                    msg = "Esta imagen no puede ser actualizada a ediciones superiores porque ya tiene la edición más avanzada"
                                Case "FRA"
                                    msg = "Cette image ne peut pas être mise à niveau vers des éditions supérieures car elle se trouve dans son édition la plus élevée"
                                Case "PTB", "PTG"
                                    msg = "Esta imagem não pode ser actualizada para edições superiores porque está na sua edição mais elevada"
                                Case "ITA"
                                    msg = "Questa immagine non può essere aggiornata a edizioni superiori perché si trova nell'edizione più alta"
                            End Select
                        Case 1
                            msg = "This image cannot be upgraded to higher editions because it is in its highest edition"
                        Case 2
                            msg = "Esta imagen no puede ser actualizada a ediciones superiores porque ya tiene la edición más avanzada"
                        Case 3
                            msg = "Cette image ne peut pas être mise à niveau vers des éditions supérieures car elle se trouve dans son édition la plus élevée"
                        Case 4
                            msg = "Esta imagem não pode ser actualizada para edições superiores porque está na sua edição mais elevada"
                        Case 5
                            msg = "Questa immagine non può essere aggiornata a edizioni superiori perché si trova nell'edizione più alta"
                    End Select
                    MsgBox(msg, vbOKOnly + vbInformation, Text)
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not grab edition targets. Error message: " & ex.Message)
            If MainForm.CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Then
                DynaLog.LogMessage("Image edition is WindowsPE. This is a Windows PE image.")
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg = "Windows PE images cannot be upgraded to higher editions."
                            Case "ESN"
                                msg = "Las imágenes de Windows PE no pueden ser actualizadas a ediciones superiores."
                            Case "FRA"
                                msg = "Les images Windows PE ne peuvent pas être mises à niveau vers des éditions supérieures."
                            Case "PTB", "PTG"
                                msg = "As imagens do Windows PE não podem ser actualizadas para edições superiores."
                            Case "ITA"
                                msg = "Le immagini di Windows PE non possono essere aggiornate a edizioni superiori."
                        End Select
                    Case 1
                        msg = "Windows PE images cannot be upgraded to higher editions."
                    Case 2
                        msg = "Las imágenes de Windows PE no pueden ser actualizadas a ediciones superiores."
                    Case 3
                        msg = "Les images Windows PE ne peuvent pas être mises à niveau vers des éditions supérieures."
                    Case 4
                        msg = "As imagens do Windows PE não podem ser actualizadas para edições superiores."
                    Case 5
                        msg = "Le immagini di Windows PE non possono essere aggiornate a edizioni superiori."
                End Select
            Else
                msg = ex.ToString()
            End If
            MsgBox(msg, vbOKOnly + vbExclamation, Text)
        Finally
            Try
                DismApi.Shutdown()
            Catch ex As Exception
                ' Don't do anything
            End Try
        End Try
        If TargetEditions.Count < 1 Then
            Return False
        Else
            ComboBox1.Items.Clear()
            ComboBox1.Items.AddRange(TargetEditions.ToArray())
            If ComboBox1.Items.Count > 0 Then
                ComboBox1.SelectedIndex = 0
            End If
        End If
        Return True
    End Function

    Private Sub SetImageEdition_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Set image edition"
                        ImageTaskHeader1.ItemText = Text
                        Label1.Text = "Target edition to upgrade to:"
                        GroupBox1.Text = "Active server installation options"
                        RadioButton1.Text = "Copy the End-User License Agreement (EULA) to the following location:"
                        RadioButton2.Text = "Accept the End-User License Agreement (EULA) and use the following product key:"
                        Button1.Text = "Browse..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Establecer edición de la imagen"
                        ImageTaskHeader1.ItemText = Text
                        Label1.Text = "Edición a la que actualizar:"
                        GroupBox1.Text = "Opciones para instalaciones de servidores"
                        RadioButton1.Text = "Copiar el Contrato de Licencia de Usuario Final (CLUF) a la siguiente ubicación:"
                        RadioButton2.Text = "Aceptar el Contrato de Licencia de Usuario Final (CLUF) y utilizar la siguiente clave de producto:"
                        Button1.Text = "Examinar..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Définir l'édition de l'image"
                        ImageTaskHeader1.ItemText = Text
                        Label1.Text = "Édition cible pour la mise à niveau :"
                        GroupBox1.Text = "Options d'installation active du serveur"
                        RadioButton1.Text = "Copier le Contrat de Licence Utilisateur Final (CLUF) à l'emplacement suivant :"
                        RadioButton2.Text = "Accepter le Contrat de Licence Utilisateur Final (CLUF) et utiliser la clé de produit suivante :"
                        Button1.Text = "Parcourir..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Text = "Definir edição da imagem"
                        ImageTaskHeader1.ItemText = Text
                        Label1.Text = "Edição de destino para atualizar:"
                        GroupBox1.Text = "Opções de instalação ativa do servidor"
                        RadioButton1.Text = "Copiar o Contrato de Licença de Usuário Final (EULA) para o seguinte local:"
                        RadioButton2.Text = "Aceitar o Contrato de Licença de Usuário Final (EULA) e usar a seguinte chave de produto:"
                        Button1.Text = "Procurar..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Imposta edizione immagine"
                        ImageTaskHeader1.ItemText = Text
                        Label1.Text = "Edizione di destinazione per l'aggiornamento:"
                        GroupBox1.Text = "Opzioni di installazione attiva del server"
                        RadioButton1.Text = "Copia il Contratto di Licenza con l'Utente Finale (CLUF) nella seguente posizione:"
                        RadioButton2.Text = "Accetta il Contratto di Licenza con l'Utente Finale (CLUF) e utilizza la seguente chiave prodotto:"
                        Button1.Text = "Sfoglia..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annulla"
                End Select
            Case 1
                Text = "Set image edition"
                ImageTaskHeader1.ItemText = Text
                Label1.Text = "Target edition to upgrade to:"
                GroupBox1.Text = "Active server installation options"
                RadioButton1.Text = "Copy the End-User License Agreement (EULA) to the following location:"
                RadioButton2.Text = "Accept the End-User License Agreement (EULA) and use the following product key:"
                Button1.Text = "Browse..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Establecer edición de la imagen"
                ImageTaskHeader1.ItemText = Text
                Label1.Text = "Edición a la que actualizar:"
                GroupBox1.Text = "Opciones para instalaciones de servidores"
                RadioButton1.Text = "Copiar el Contrato de Licencia de Usuario Final (CLUF) a la siguiente ubicación:"
                RadioButton2.Text = "Aceptar el Contrato de Licencia de Usuario Final (CLUF) y utilizar la siguiente clave de producto:"
                Button1.Text = "Examinar..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Définir l'édition de l'image"
                ImageTaskHeader1.ItemText = Text
                Label1.Text = "Édition cible pour la mise à niveau :"
                GroupBox1.Text = "Options d'installation active du serveur"
                RadioButton1.Text = "Copier le Contrat de Licence Utilisateur Final (CLUF) à l'emplacement suivant :"
                RadioButton2.Text = "Accepter le Contrat de Licence Utilisateur Final (CLUF) et utiliser la clé de produit suivante :"
                Button1.Text = "Parcourir..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case 4
                Text = "Definir edição da imagem"
                ImageTaskHeader1.ItemText = Text
                Label1.Text = "Edição de destino para atualizar:"
                GroupBox1.Text = "Opções de instalação ativa do servidor"
                RadioButton1.Text = "Copiar o Contrato de Licença de Usuário Final (EULA) para o seguinte local:"
                RadioButton2.Text = "Aceitar o Contrato de Licença de Usuário Final (EULA) e usar a seguinte chave de produto:"
                Button1.Text = "Procurar..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
            Case 5
                Text = "Imposta edizione immagine"
                ImageTaskHeader1.ItemText = Text
                Label1.Text = "Edizione di destinazione per l'aggiornamento:"
                GroupBox1.Text = "Opzioni di installazione attiva del server"
                RadioButton1.Text = "Copia il Contratto di Licenza con l'Utente Finale (CLUF) nella seguente posizione:"
                RadioButton2.Text = "Accetta il Contratto di Licenza con l'Utente Finale (CLUF) e utilizza la seguente chiave prodotto:"
                Button1.Text = "Sfoglia..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annulla"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = BackColor
        TextBox1.BackColor = BackColor
        TextBox2.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)
        DynaLog.LogMessage("Determining EULA option compatibility...")
        DynaLog.LogMessage("- Image Installation Type: " & MainForm.CurrentImage.ImageProductType)
        DynaLog.LogMessage("- Managing Active Installation? " & If(MainForm.OnlineManagement, "Yes", "No"))
        ' Disable group box if not managing an active server installation
        If MainForm.CurrentImage.ImageInstallationType.ToLower().Contains("server") AndAlso MainForm.OnlineManagement Then
            DynaLog.LogMessage("All requirements are met. We are managing a Windows Server installation")
            GroupBox1.Enabled = True
        Else
            DynaLog.LogMessage("Either one or none of the two requirements described above is met. The image we are managing is not an active installation, or a Windows Server installation")
            GroupBox1.Enabled = False
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        EulaPanel.Enabled = RadioButton1.Checked
        TextBox2.Enabled = RadioButton2.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Selected path: " & FolderBrowserDialog1.SelectedPath)
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
End Class
