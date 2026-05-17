Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Public Class ImgUMount

    Dim UMountOperations() As String = New String(1) {"Save changes and unmount", "Discard changes and unmount"}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If RadioButton1.Checked = True Then
            DynaLog.LogMessage("The image mounted in this project will be unmounted.")
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
            ProgressPanel.MountDir = MainForm.MountDir
        Else
            DynaLog.LogMessage("An image mounted in a different folder will be unmounted.")
            DynaLog.LogMessage("- Provided mount directory: " & Quote & TextBox1.Text & Quote)
            ProgressPanel.UMountLocalDir = False
            ' Determine if given mount dir exists
            DynaLog.LogMessage("Checking if the provided mount directory exists...")
            If Directory.Exists(TextBox1.Text) Then
                DynaLog.LogMessage("The provided mount directory exists. Checking if an image is mounted there...")
                ' Detect whether the mount dir has an image mounted (I don't believe on what users claim, just to be sure)
                If MainForm.MountedImageList.Select(Function(image) image.ImageMountDirectory).Contains(TextBox1.Text) Then
                    DynaLog.LogMessage("An image is mounted there. This is a valid mount directory.")
                    ProgressPanel.RandomMountDir = TextBox1.Text
                Else
                    DynaLog.LogMessage("No image is mounted there. This is not a valid mount directory.")
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    MsgBox("The specified directory isn't a valid mount directory.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                                Case "ESN"
                                    MsgBox("El directorio especificado no es un directorio de montaje válido.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                                Case "FRA"
                                    MsgBox("Le répertoire spécifié n'est pas un répertoire de montage valide.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                                Case "PTB", "PTG"
                                    MsgBox("O diretório especificado não é um diretório de montagem válido.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                                Case "ITA"
                                    MsgBox("La directory specificata non è una directory di montaggio valida.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            End Select
                        Case 1
                            MsgBox("The specified directory isn't a valid mount directory.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case 2
                            MsgBox("El directorio especificado no es un directorio de montaje válido.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case 3
                            MsgBox("Le répertoire spécifié n'est pas un répertoire de montage valide.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case 4
                            MsgBox("O diretório especificado não é um diretório de montagem válido.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case 5
                            MsgBox("La directory specificata non è una directory di montaggio valida.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    End Select
                    Exit Sub
                End If
            Else
                DynaLog.LogMessage("The provided mount directory does not exist.")
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MsgBox("The mount directory doesn't exist.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Case "ESN"
                                MsgBox("El directorio de montaje no existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Case "FRA"
                                MsgBox("Le répertoire de montage n'existe pas.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Case "PTB", "PTG"
                                MsgBox("O diretório de montagem não existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Case "ITA"
                                MsgBox("La directory di montaggio non esiste", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        End Select
                    Case 1
                        MsgBox("The mount directory doesn't exist.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Case 2
                        MsgBox("El directorio de montaje no existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Case 3
                        MsgBox("Le répertoire de montage n'existe pas.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Case 4
                        MsgBox("O diretório de montagem não existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Case 5
                        MsgBox("La directory di montaggio non esiste", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                End Select
                Exit Sub
            End If
        End If
        If ComboBox1.SelectedIndex = 0 Then
            ProgressPanel.UMountOp = 0
        ElseIf ComboBox1.SelectedIndex = 1 Then
            ProgressPanel.UMountOp = 1
        End If
        If CheckBox1.Checked Then
            ProgressPanel.CheckImgIntegrity = True
        Else
            ProgressPanel.CheckImgIntegrity = False
        End If
        If CheckBox2.Checked Then
            ProgressPanel.SaveToNewIndex = True
        Else
            ProgressPanel.SaveToNewIndex = False
        End If
        If MainForm.isProjectLoaded Then
            ProgressPanel.UMountImgIndex = MainForm.ImgIndex
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 21
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgUMount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedText = ""
        ComboBox1.Items.Clear()
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Unmount an image"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Please specify the options to unmount this image:"
                        Label3.Text = "The mount directory:"
                        Label4.Text = "Mount directory:"
                        Label7.Text = "Unmount operation:"
                        CheckBox1.Text = "Check image integrity"
                        CheckBox2.Text = "Append changes to another index"
                        Button1.Text = "Pick..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        FolderBrowserDialog1.Description = "Please specify a mount directory:"
                        RadioButton1.Text = "is loaded in the project"
                        RadioButton2.Text = "is located somewhere else"
                        UMountOperations(0) = "Save changes and unmount"
                        UMountOperations(1) = "Discard changes and unmount"
                        GroupBox1.Text = "Mount directory"
                        GroupBox2.Text = "Additional options"
                    Case "ESN"
                        Text = "Desmontar una imagen"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Especifique las opciones para desmontar esta imagen:"
                        Label3.Text = "El directorio de montaje:"
                        Label4.Text = "Directorio de montaje:"
                        Label7.Text = "Operación de desmontaje:"
                        CheckBox1.Text = "Comprobar integridad de la imagen"
                        CheckBox2.Text = "Anexar los cambios en otro índice"
                        Button1.Text = "Escoger..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        FolderBrowserDialog1.Description = "Especifique un directorio de montaje:"
                        RadioButton1.Text = "está cargado en el proyecto"
                        RadioButton2.Text = "se ubica en otro lugar"
                        UMountOperations(0) = "Guardar cambios y desmontar"
                        UMountOperations(1) = "Descartar cambios y desmontar"
                        GroupBox1.Text = "Directorio de montaje"
                        GroupBox2.Text = "Opciones adicionales"
                    Case "FRA"
                        Text = "Démonter une image"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Veuillez spécifier les options pour démonter cette image :"
                        Label3.Text = "Le répertoire de montage :"
                        Label4.Text = "Répertoire de montage :"
                        Label7.Text = "Opération de démontage :"
                        CheckBox1.Text = "Vérifier l'intégrité de l'image"
                        CheckBox2.Text = "Ajouter des modifications à un autre index"
                        Button1.Text = "Choisir..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        FolderBrowserDialog1.Description = "Veuillez indiquer un répertoire de montage :"
                        RadioButton1.Text = "est chargé dans le projet"
                        RadioButton2.Text = "est situé ailleurs"
                        UMountOperations(0) = "Sauvegarder les modifications et démonter"
                        UMountOperations(1) = "Annuler les modifications et démonter"
                        GroupBox1.Text = "Répertoire de montage"
                        GroupBox2.Text = "Paramètres supplémentaires"
                    Case "PTB", "PTG"
                        Text = "Desmontar uma imagem"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Por favor, especifique as opções para desmontar esta imagem:"
                        Label3.Text = "O diretório de montagem:"
                        Label4.Text = "Diretório de montagem:"
                        Label7.Text = "Operação de desmontagem:"
                        CheckBox1.Text = "Verificar a integridade da imagem"
                        CheckBox2.Text = "Anexar alterações a outro índice"
                        Button1.Text = "Escolher..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        FolderBrowserDialog1.Description = "Por favor, especifique um diretório de montagem:"
                        RadioButton1.Text = "está carregado no projeto"
                        RadioButton2.Text = "está localizado noutro local"
                        UMountOperations(0) = "Guardar alterações e desmontar"
                        UMountOperations(1) = "Descartar alterações e desmontar"
                        GroupBox1.Text = "Diretório de montagem"
                        GroupBox2.Text = "Opções adicionais"
                    Case "ITA"
                        Text = "Smontare un'immagine"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Specificare le opzioni per smontare questa immagine:"
                        Label3.Text = "La directory di montaggio:"
                        Label4.Text = "Directory di montaggio:"
                        Label7.Text = "Operazione di smontaggio:"
                        CheckBox1.Text = "Controlla l'integrità dell'immagine"
                        CheckBox2.Text = "Applica le modifiche a un altro indice"
                        Button1.Text = "Scegli..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        FolderBrowserDialog1.Description = "Specificare una directory di montaggio:"
                        RadioButton1.Text = "è caricata nel progetto"
                        RadioButton2.Text = "si trova da qualche altra parte"
                        UMountOperations(0) = "Salvare le modifiche e smontare"
                        UMountOperations(1) = "Scartare le modifiche e smontare"
                        GroupBox1.Text = "Montare la directory"
                        GroupBox2.Text = "Opzioni aggiuntive"
                End Select
            Case 1
                Text = "Unmount an image"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Please specify the options to unmount this image:"
                Label3.Text = "The mount directory:"
                Label4.Text = "Mount directory:"
                Label7.Text = "Unmount operation:"
                CheckBox1.Text = "Check image integrity"
                CheckBox2.Text = "Append changes to another index"
                Button1.Text = "Pick..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                FolderBrowserDialog1.Description = "Please specify a mount directory:"
                RadioButton1.Text = "is loaded in the project"
                RadioButton2.Text = "is located somewhere else"
                UMountOperations(0) = "Save changes and unmount"
                UMountOperations(1) = "Discard changes and unmount"
                GroupBox1.Text = "Mount directory"
                GroupBox2.Text = "Additional options"
            Case 2
                Text = "Desmontar una imagen"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Especifique las opciones para desmontar esta imagen:"
                Label3.Text = "El directorio de montaje:"
                Label4.Text = "Directorio de montaje:"
                Label7.Text = "Operación de desmontaje:"
                CheckBox1.Text = "Comprobar integridad de la imagen"
                CheckBox2.Text = "Anexar los cambios en otro índice"
                Button1.Text = "Escoger..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                FolderBrowserDialog1.Description = "Especifique un directorio de montaje:"
                RadioButton1.Text = "está cargado en el proyecto"
                RadioButton2.Text = "se ubica en otro lugar"
                UMountOperations(0) = "Guardar cambios y desmontar"
                UMountOperations(1) = "Descartar cambios y desmontar"
                GroupBox1.Text = "Directorio de montaje"
                GroupBox2.Text = "Opciones adicionales"
            Case 3
                Text = "Démonter une image"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Veuillez spécifier les options pour démonter cette image :"
                Label3.Text = "Le répertoire de montage :"
                Label4.Text = "Répertoire de montage :"
                Label7.Text = "Opération de démontage :"
                CheckBox1.Text = "Vérifier l'intégrité de l'image"
                CheckBox2.Text = "Ajouter des modifications à un autre index"
                Button1.Text = "Choisir..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                FolderBrowserDialog1.Description = "Veuillez indiquer un répertoire de montage :"
                RadioButton1.Text = "est chargé dans le projet"
                RadioButton2.Text = "est situé ailleurs"
                UMountOperations(0) = "Sauvegarder les modifications et démonter"
                UMountOperations(1) = "Annuler les modifications et démonter"
                GroupBox1.Text = "Répertoire de montage"
                GroupBox2.Text = "Paramètres supplémentaires"
            Case 4
                Text = "Desmontar uma imagem"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Por favor, especifique as opções para desmontar esta imagem:"
                Label3.Text = "O diretório de montagem:"
                Label4.Text = "Diretório de montagem:"
                Label7.Text = "Operação de desmontagem:"
                CheckBox1.Text = "Verificar a integridade da imagem"
                CheckBox2.Text = "Anexar alterações a outro índice"
                Button1.Text = "Escolher..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                FolderBrowserDialog1.Description = "Por favor, especifique um diretório de montagem:"
                RadioButton1.Text = "está carregado no projeto"
                RadioButton2.Text = "está localizado noutro local"
                UMountOperations(0) = "Guardar alterações e desmontar"
                UMountOperations(1) = "Descartar alterações e desmontar"
                GroupBox1.Text = "Diretório de montagem"
                GroupBox2.Text = "Opções adicionais"
            Case 5
                Text = "Smontare un'immagine"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Specificare le opzioni per smontare questa immagine:"
                Label3.Text = "La directory di montaggio:"
                Label4.Text = "Directory di montaggio:"
                Label7.Text = "Operazione di smontaggio:"
                CheckBox1.Text = "Controlla l'integrità dell'immagine"
                CheckBox2.Text = "Applica le modifiche a un altro indice"
                Button1.Text = "Scegli..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                FolderBrowserDialog1.Description = "Specificare una directory di montaggio:"
                RadioButton1.Text = "è caricata nel progetto"
                RadioButton2.Text = "si trova da qualche altra parte"
                UMountOperations(0) = "Salvare le modifiche e smontare"
                UMountOperations(1) = "Scartare le modifiche e smontare"
                GroupBox1.Text = "Montare la directory"
                GroupBox2.Text = "Opzioni aggiuntive"
        End Select
        ComboBox1.Items.AddRange(UMountOperations)
        ComboBox1.SelectedIndex = 0
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        If RadioButton1.Checked Then
            Label4.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
        Else
            Label4.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label4.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
        Else
            Label4.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim selectedImage As WindowsImage = PopupMountedImagePicker.PickImage()
        If selectedImage IsNot Nothing Then
            TextBox1.Text = selectedImage.ImageMountDirectory
            DynaLog.LogMessage("Checking if selected item is the mount directory of the project...")
            If TextBox1.Text = MainForm.MountDir Then
                DynaLog.LogMessage("The selected item is the mount directory of the project.")
                TextBox1.Text = ""
                RadioButton1.Checked = True
                RadioButton2.Checked = False
            End If
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Select Case ComboBox1.SelectedIndex
            Case 0
                CheckBox1.Enabled = True
                CheckBox2.Enabled = True
            Case 1
                CheckBox1.Enabled = False
                CheckBox2.Enabled = False
        End Select
    End Sub
End Class
