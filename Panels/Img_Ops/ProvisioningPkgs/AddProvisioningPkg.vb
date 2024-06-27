Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class AddProvisioningPkg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If TextBox1.Text <> "" Then
            If File.Exists(TextBox1.Text) Then
                ProgressPanel.ppkgAdditionPackagePath = TextBox1.Text
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MsgBox("The specified provisioning package does not exist. Make sure it exists in the file system and try again.", vbOKOnly + vbCritical, Label1.Text)
                            Case "ESN"
                                MsgBox("El paquete de aprovisionamiento especificado no existe. Asegúrese de que exista en el sistema de archivos e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                            Case "FRA"
                                MsgBox("Le paquet de provisionnement spécifié n'existe pas. Assurez-vous qu'il existe dans le système de fichiers et réessayez.", vbOKOnly + vbCritical, Label1.Text)
                            Case "PTB", "PTG"
                                MsgBox("O pacote de provisionamento especificado não existe. Certifique-se de que existe no sistema de ficheiros e tente novamente.", vbOKOnly + vbCritical, Label1.Text)
                            Case "ITA"
                                MsgBox("Il pacchetto di provisioning specificato non esiste. Assicuratevi che esista nel file system e riprovate.", vbOKOnly + vbCritical, Label1.Text)
                        End Select
                    Case 1
                        MsgBox("The specified provisioning package does not exist. Make sure it exists in the file system and try again.", vbOKOnly + vbCritical, Label1.Text)
                    Case 2
                        MsgBox("El paquete de aprovisionamiento especificado no existe. Asegúrese de que exista en el sistema de archivos e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                    Case 3
                        MsgBox("Le paquet de provisionnement spécifié n'existe pas. Assurez-vous qu'il existe dans le système de fichiers et réessayez.", vbOKOnly + vbCritical, Label1.Text)
                    Case 4
                        MsgBox("O pacote de provisionamento especificado não existe. Certifique-se de que existe no sistema de ficheiros e tente novamente.", vbOKOnly + vbCritical, Label1.Text)
                    Case 5
                        MsgBox("Il pacchetto di provisioning specificato non esiste. Assicuratevi che esista nel file system e riprovate.", vbOKOnly + vbCritical, Label1.Text)
                End Select
                Exit Sub
            End If
            If TextBox2.Text <> "" And File.Exists(TextBox2.Text) Then
                ProgressPanel.ppkgAdditionCatalogPath = TextBox2.Text
            ElseIf TextBox2.Text <> "" And Not File.Exists(TextBox2.Text) Then
                Dim msg As String = ""
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg = "The catalog file specified doesn't exist. We won't use this file if you proceed." & CrLf & CrLf & "Do you want to continue?"
                            Case "ESN"
                                msg = "El archivo de catálogo especificado no existe. No usaremos este archivo si continúa." & CrLf & CrLf & "¿Desea continuar?"
                            Case "FRA"
                                msg = "Le fichier de catalogue spécifié n'existe pas. Nous n'utiliserons pas ce fichier si vous continuez." & CrLf & CrLf & "Voulez-vous continuer ?"
                            Case "PTB", "PTG"
                                msg = "O ficheiro de catálogo especificado não existe. Não utilizaremos este ficheiro se prosseguir." & CrLf & CrLf & "Deseja continuar?"
                            Case "ITA"
                                msg = "Il file di catalogo specificato non esiste. Non utilizzeremo questo file se si procede." & CrLf & CrLf & "Si desidera continuare?"
                        End Select
                    Case 1
                        msg = "The catalog file specified doesn't exist. We won't use this file if you proceed." & CrLf & CrLf & "Do you want to continue?"
                    Case 2
                        msg = "El archivo de catálogo especificado no existe. No usaremos este archivo si continúa." & CrLf & CrLf & "¿Desea continuar?"
                    Case 3
                        msg = "Le fichier de catalogue spécifié n'existe pas. Nous n'utiliserons pas ce fichier si vous continuez." & CrLf & CrLf & "Voulez-vous continuer ?"
                    Case 4
                        msg = "O ficheiro de catálogo especificado não existe. Não utilizaremos este ficheiro se prosseguir." & CrLf & CrLf & "Deseja continuar?"
                    Case 5
                        msg = "Il file di catalogo specificato non esiste. Non utilizzeremo questo file se si procede." & CrLf & CrLf & "Si desidera continuare?"
                End Select
                If MsgBox(msg, vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                    Exit Sub
                End If
            Else
                ProgressPanel.ppkgAdditionCatalogPath = ""
            End If
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("No provisioning package has been specified. Please specify a provisioning package to add and try again.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ESN"
                            MsgBox("No se ha especificado un paquete de aprovisionamiento. Especifique un paquete de aprovisionamiento a añadir e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                        Case "FRA"
                            MsgBox("Aucun paquet de provisionnement n'a été spécifié. Veuillez spécifier un paquet de provisionnement à ajouter et réessayer.", vbOKOnly + vbCritical, Label1.Text)
                        Case "PTB", "PTG"
                            MsgBox("Não foi especificado nenhum pacote de aprovisionamento. Especifique um pacote de provisionamento para adicionar e tente novamente.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ITA"
                            MsgBox("Non è stato specificato alcun pacchetto di provisioning. Specificare un pacchetto di provisioning da aggiungere e riprovare.", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                Case 1
                    MsgBox("No provisioning package has been specified. Please specify a provisioning package to add and try again.", vbOKOnly + vbCritical, Label1.Text)
                Case 2
                    MsgBox("No se ha especificado un paquete de aprovisionamiento. Especifique un paquete de aprovisionamiento a añadir e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                Case 3
                    MsgBox("Aucun paquet de provisionnement n'a été spécifié. Veuillez spécifier un paquet de provisionnement à ajouter et réessayer.", vbOKOnly + vbCritical, Label1.Text)
                Case 4
                    MsgBox("Não foi especificado nenhum pacote de aprovisionamento. Especifique um pacote de provisionamento para adicionar e tente novamente.", vbOKOnly + vbCritical, Label1.Text)
                Case 5
                    MsgBox("Non è stato specificato alcun pacchetto di provisioning. Specificare un pacchetto di provisioning da aggiungere e riprovare.", vbOKOnly + vbCritical, Label1.Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.ppkgAdditionCommit = If(CheckBox1.Checked, True, False)
        ProgressPanel.OperationNum = 33
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        OpenFileDialog2.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub OpenFileDialog2_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk
        TextBox2.Text = OpenFileDialog2.FileName
    End Sub

    Private Sub TextBox1_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub TextBox1_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox1.DragDrop
        TextBox1.Text = e.Data.GetData(DataFormats.FileDrop)
    End Sub

    Private Sub TextBox2_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox2.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub TextBox2_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox2.DragDrop
        TextBox2.Text = e.Data.GetData(DataFormats.FileDrop)
    End Sub

    Private Sub AddProvisioningPkg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Add provisioning packages"
                        Label1.Text = Text
                        Label2.Text = "Package path:"
                        Label3.Text = "This action can't be reverted. Once you add a provisioning package, you won't be able to remove it from your Windows image."
                        Label4.Text = "Catalog path:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        Button1.Text = "Browse..."
                        Button2.Text = "Browse..."
                        CheckBox1.Text = "Commit image after adding this provisioning package"
                    Case "ESN"
                        Text = "Añadir paquete de aprovisionamiento"
                        Label1.Text = Text
                        Label2.Text = "Ruta de paquete:"
                        Label3.Text = "Esta acción no puede ser revertida. Cuando añada un paquete de aprovisionamiento, no lo podrá eliminar de la imagen de Windows."
                        Label4.Text = "Ruta de catálogo:"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Examinar..."
                        CheckBox1.Text = "Guardar imagen tras añadir este paquete de aprovisionamiento"
                    Case "FRA"
                        Text = "Ajouter des paquets de provisionnement"
                        Label1.Text = Text
                        Label2.Text = "Chemin du paquet :"
                        Label3.Text = "Cette action ne peut pas être annulée. Une fois que vous avez ajouté un package de provisionnement, vous ne pourrez plus le supprimer de votre image Windows."
                        Label4.Text = "Chemin du catalogue :"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Parcourir..."
                        CheckBox1.Text = "Enregistrer l'image après l'ajout de ce paquet de provisionnement"
                    Case "PTB", "PTG"
                        Text = "Adicionar pacotes de aprovisionamento"
                        Label1.Text = Text
                        Label2.Text = "Localização do pacote:"
                        Label3.Text = "Esta ação não pode ser revertida. Depois de adicionar um pacote de aprovisionamento, não o poderá remover da sua imagem do Windows."
                        Label4.Text = "Localização do catálogo:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Navegar..."
                        Button2.Text = "Navegar..."
                        CheckBox1.Text = "Confirmar imagem após adicionar este pacote de provisionamento"
                    Case "ITA"
                        Text = "Aggiungi pacchetti di approvvigionamento"
                        Label1.Text = Text
                        Label2.Text = "Percorso del pacchetto:"
                        Label3.Text = "Questa azione non può essere annullata. Una volta aggiunto un pacchetto di approvvigionamento, non sarà più possibile rimuoverlo dall'immagine di Windows."
                        Label4.Text = "Percorso catalogo:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        Button1.Text = "Sfoglia..."
                        Button2.Text = "Sfoglia..."
                        CheckBox1.Text = "Impegna l'immagine dopo aver aggiunto questo pacchetto di approvvigionamento"
                End Select
            Case 1
                Text = "Add provisioning packages"
                Label1.Text = Text
                Label2.Text = "Package path:"
                Label3.Text = "This action can't be reverted. Once you add a provisioning package, you won't be able to remove it from your Windows image."
                Label4.Text = "Catalog path:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                Button1.Text = "Browse..."
                Button2.Text = "Browse..."
                CheckBox1.Text = "Commit image after adding this provisioning package"
            Case 2
                Text = "Añadir paquete de aprovisionamiento"
                Label1.Text = Text
                Label2.Text = "Ruta de paquete:"
                Label3.Text = "Esta acción no puede ser revertida. Cuando añada un paquete de aprovisionamiento, no lo podrá eliminar de la imagen de Windows."
                Label4.Text = "Ruta de catálogo:"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Examinar..."
                Button2.Text = "Examinar..."
                CheckBox1.Text = "Guardar imagen tras añadir este paquete de aprovisionamiento"
            Case 3
                Text = "Ajouter des paquets de provisionnement"
                Label1.Text = Text
                Label2.Text = "Chemin du paquet :"
                Label3.Text = "Cette action ne peut pas être annulée. Une fois que vous avez ajouté un package de provisionnement, vous ne pourrez plus le supprimer de votre image Windows."
                Label4.Text = "Chemin du catalogue :"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                Button1.Text = "Parcourir..."
                Button2.Text = "Parcourir..."
                CheckBox1.Text = "Enregistrer l'image après l'ajout de ce paquet de provisionnement"
            Case 4
                Text = "Adicionar pacotes de aprovisionamento"
                Label1.Text = Text
                Label2.Text = "Localização do pacote:"
                Label3.Text = "Esta ação não pode ser revertida. Depois de adicionar um pacote de aprovisionamento, não o poderá remover da sua imagem do Windows."
                Label4.Text = "Localização do catálogo:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Navegar..."
                Button2.Text = "Navegar..."
                CheckBox1.Text = "Confirmar imagem após adicionar este pacote de provisionamento"
            Case 5
                Text = "Aggiungi pacchetti di approvvigionamento"
                Label1.Text = Text
                Label2.Text = "Percorso del pacchetto:"
                Label3.Text = "Questa azione non può essere annullata. Una volta aggiunto un pacchetto di approvvigionamento, non sarà più possibile rimuoverlo dall'immagine di Windows."
                Label4.Text = "Percorso catalogo:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                Button1.Text = "Sfoglia..."
                Button2.Text = "Sfoglia..."
                CheckBox1.Text = "Impegna l'immagine dopo aver aggiunto questo pacchetto di approvvigionamento"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
