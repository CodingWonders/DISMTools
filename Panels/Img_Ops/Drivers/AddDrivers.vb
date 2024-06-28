Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class AddDrivers

    Dim drvPkgList As New List(Of String)
    Dim drvPkgs(65535) As String
    Dim drvRecursiveList As New List(Of String)
    Dim drvRecursivePkgs(65535) As String

    Dim drvPkgCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        drvPkgList.Clear()
        drvRecursiveList.Clear()
        ProgressPanel.MountDir = MainForm.MountDir
        drvPkgCount = ListView1.Items.Count
        If ListView1.Items.Count > 0 Then
            For x = 0 To ListView1.Items.Count - 1
                drvPkgList.Add(ListView1.Items(x).SubItems(0).Text)
            Next
            drvPkgs = drvPkgList.ToArray()
            For x = 0 To drvPkgs.Length - 1
                ProgressPanel.drvAdditionPkgs(x) = drvPkgs(x)
            Next
            For x = 0 To drvPkgCount - 1
                If File.GetAttributes(ListView1.Items(x).SubItems(0).Text) = FileAttributes.Directory And CheckedListBox1.CheckedItems.Contains(ListView1.Items(x).SubItems(0).Text) Then
                    drvRecursiveList.Add(ListView1.Items(x).SubItems(0).Text)
                End If
            Next
            If drvRecursiveList.Count > 0 Then
                drvRecursivePkgs = drvRecursiveList.ToArray()
                For x = 0 To drvRecursivePkgs.Length - 1
                    ProgressPanel.drvAdditionFolderRecursiveScan(x) = drvRecursivePkgs(x)
                Next
            End If
            If CheckBox1.Checked Then
                ProgressPanel.drvAdditionForceUnsigned = True
            Else
                ProgressPanel.drvAdditionForceUnsigned = False
            End If
            If CheckBox2.Checked And Not MainForm.OnlineManagement Then
                ProgressPanel.drvAdditionCommit = True
            Else
                ProgressPanel.drvAdditionCommit = False
            End If
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("There are no selected driver packages to install. Please specify the driver packages you'd like to install and try again.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ESN"
                            MsgBox("No hay paquetes de controladores seleccionados para instalar. Especifique los paquetes de controladores que le gustaría instalar e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                        Case "FRA"
                            MsgBox("Il n'y a pas de pilotes sélectionnés à installer. Veuillez spécifier les paquets de pilotes que vous souhaitez installer et réessayez.", vbOKOnly + vbCritical, Label1.Text)
                        Case "PTB", "PTG"
                            MsgBox("Não existem pacotes de controladores seleccionados para instalar. Especifique os pacotes de controladores que gostaria de instalar e tente novamente.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ITA"
                            MsgBox("Non sono stati selezionati pacchetti driver da installare. Specificare i pacchetti di driver che si desidera installare e riprovare", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                Case 1
                    MsgBox("There are no selected driver packages to install. Please specify the driver packages you'd like to install and try again.", vbOKOnly + vbCritical, Label1.Text)
                Case 2
                    MsgBox("No hay paquetes de controladores seleccionados para instalar. Especifique los paquetes de controladores que le gustaría instalar e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                Case 3
                    MsgBox("Il n'y a pas de pilotes sélectionnés à installer. Veuillez spécifier les paquets de pilotes que vous souhaitez installer et réessayez.", vbOKOnly + vbCritical, Label1.Text)
                Case 4
                    MsgBox("Não existem pacotes de controladores seleccionados para instalar. Especifique os pacotes de controladores que gostaria de instalar e tente novamente.", vbOKOnly + vbCritical, Label1.Text)
                Case 5
                    MsgBox("Non sono stati selezionati pacchetti driver da installare. Specificare i pacchetti di driver che si desidera installare e riprovare", vbOKOnly + vbCritical, Label1.Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.drvAdditionLastPkg = ListView1.Items(drvPkgCount - 1).SubItems(0).Text
        ProgressPanel.drvAdditionCount = drvPkgCount
        ProgressPanel.OperationNum = 75
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

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "File"}))
                    Case "ESN"
                        ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "Archivo"}))
                    Case "FRA"
                        ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "Fichier"}))
                    Case "PTB", "PTG"
                        ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "Ficheiro"}))
                    Case "ITA"
                        ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "File"}))
                End Select
            Case 1
                ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "File"}))
            Case 2
                ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "Archivo"}))
            Case 3
                ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "Fichier"}))
            Case 4
                ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "Ficheiro"}))
            Case 5
                ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "File"}))
        End Select
        Button3.Enabled = ListView1.Items.Count > 0
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Cursor = Cursors.WaitCursor
            If My.Computer.FileSystem.GetFiles(FolderBrowserDialog1.SelectedPath, FileIO.SearchOption.SearchAllSubDirectories, "*.inf").Count > 0 Then
                Dim msg As String = ""
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg = "The package specified is a folder. You can let DISM scan it recursively to add all drivers in it, or you can specify the drivers to add manually." & CrLf & CrLf & _
                                      "- To let DISM scan this folder recursively, click Yes" & CrLf & _
                                      "- To pick the drivers in this folder manually, click No" & CrLf & _
                                      "- To skip adding this folder, click Cancel"
                            Case "ESN"
                                msg = "El paquete especificado es una carpeta. Puede dejar que DISM la escanee de forma recursiva para añadir todos los controladores en ella, o puede especificar los controladores a añadir manualmente." & CrLf & CrLf & _
                                      "- Para dejar que DISM escanee esta carpeta de forma recursiva, haga clic en Sí" & CrLf & _
                                      "- Para escoger los controladores en esta carpeta manualmente, haga clic en No" & CrLf & _
                                      "- Para omitir la adición de esta carpeta, haga clic en Cancelar"
                            Case "FRA"
                                msg = "Le paquet de pilotes spécifié est un dossier. Vous pouvez laisser DISM l'analyser de manière récursive pour ajouter tous les pilotes qu'il contient, ou vous pouvez spécifier les pilotes à ajouter manuellement." & CrLf & CrLf & _
                                      "- Pour laisser DISM analyser ce dossier de manière récursive, cliquez sur Oui" & CrLf & _
                                      "- Pour sélectionner manuellement les pilotes de ce dossier, cliquez sur Non" & CrLf & _
                                      "- Pour ne pas ajouter ce dossier, cliquez sur Annuler"
                            Case "PTB", "PTG"
                                msg = "O pacote especificado é uma pasta. Pode deixar que o DISM a analise recursivamente para adicionar todos os controladores nela contidos, ou pode especificar os controladores a adicionar manualmente." & CrLf & CrLf & _
                                      "- Para permitir que o DISM verifique esta pasta recursivamente, clique em Sim" & CrLf & _
                                      "- Para escolher manualmente os controladores desta pasta, clique em Não" & CrLf & _
                                      "- Para não adicionar esta pasta, clique em Cancelar"
                            Case "ITA"
                                msg = "Il pacchetto specificato è una cartella. Si può lasciare che DISM la scansioni ricorsivamente per aggiungere tutti i driver in essa contenuti, oppure si possono specificare i driver da aggiungere manualmente." & CrLf & CrLf & _
                                      "- Per consentire a DISM di eseguire la scansione ricorsiva di questa cartella, fare clic su Sì" & CrLf & _
                                      "- Per scegliere manualmente i driver in questa cartella, fare clic su No" & CrLf & _
                                      "- Per non aggiungere questa cartella, fare clic su Annulla"
                        End Select
                    Case 1
                        msg = "The package specified is a folder. You can let DISM scan it recursively to add all drivers in it, or you can specify the drivers to add manually." & CrLf & CrLf & _
                              "- To let DISM scan this folder recursively, click Yes" & CrLf & _
                              "- To pick the drivers in this folder manually, click No" & CrLf & _
                              "- To skip adding this folder, click Cancel"
                    Case 2
                        msg = "El paquete especificado es una carpeta. Puede dejar que DISM la escanee de forma recursiva para añadir todos los controladores en ella, o puede especificar los controladores a añadir manualmente." & CrLf & CrLf & _
                              "- Para dejar que DISM escanee esta carpeta de forma recursiva, haga clic en Sí" & CrLf & _
                              "- Para escoger los controladores en esta carpeta manualmente, haga clic en No" & CrLf & _
                              "- Para omitir la adición de esta carpeta, haga clic en Cancelar"
                    Case 3
                        msg = "Le paquet de pilotes spécifié est un dossier. Vous pouvez laisser DISM l'analyser de manière récursive pour ajouter tous les pilotes qu'il contient, ou vous pouvez spécifier les pilotes à ajouter manuellement." & CrLf & CrLf & _
                              "- Pour laisser DISM analyser ce dossier de manière récursive, cliquez sur Oui" & CrLf & _
                              "- Pour sélectionner manuellement les pilotes de ce dossier, cliquez sur Non" & CrLf & _
                              "- Pour ne pas ajouter ce dossier, cliquez sur Annuler"
                    Case 4
                        msg = "O pacote especificado é uma pasta. Pode deixar que o DISM a analise recursivamente para adicionar todos os controladores nela contidos, ou pode especificar os controladores a adicionar manualmente." & CrLf & CrLf & _
                              "- Para permitir que o DISM verifique esta pasta recursivamente, clique em Sim" & CrLf & _
                              "- Para escolher manualmente os controladores desta pasta, clique em Não" & CrLf & _
                              "- Para não adicionar esta pasta, clique em Cancelar"
                    Case 5
                        msg = "Il pacchetto specificato è una cartella. Si può lasciare che DISM la scansioni ricorsivamente per aggiungere tutti i driver in essa contenuti, oppure si possono specificare i driver da aggiungere manualmente." & CrLf & CrLf & _
                              "- Per consentire a DISM di eseguire la scansione ricorsiva di questa cartella, fare clic su Sì" & CrLf & _
                              "- Per scegliere manualmente i driver in questa cartella, fare clic su No" & CrLf & _
                              "- Per non aggiungere questa cartella, fare clic su Annulla"
                End Select
                Select Case MsgBox(msg, vbYesNoCancel + vbInformation, Label1.Text)
                    Case MsgBoxResult.Yes
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Folder"}))
                                    Case "ESN"
                                        ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Carpeta"}))
                                    Case "FRA"
                                        ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Répertoire"}))
                                    Case "PTB", "PTG"
                                        ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Pasta"}))
                                    Case "ITA"
                                        ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Cartella"}))
                                End Select
                            Case 1
                                ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Folder"}))
                            Case 2
                                ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Carpeta"}))
                            Case 3
                                ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Répertoire"}))
                            Case 4
                                ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Pasta"}))
                            Case 5
                                ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Cartella"}))
                        End Select
                        CheckedListBox1.Items.Add(FolderBrowserDialog1.SelectedPath)
                        CheckedListBox1.SetItemChecked(CheckedListBox1.Items.IndexOf(FolderBrowserDialog1.SelectedPath), True)
                    Case MsgBoxResult.No
                        DriverManualFilePicker.DriverDir = FolderBrowserDialog1.SelectedPath
                        DriverManualFilePicker.ShowDialog(Me)
                    Case MsgBoxResult.Cancel
                        Exit Sub
                End Select
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MsgBox("There are no driver packages in the specified folder", vbOKOnly + vbCritical, Label1.Text)
                            Case "ESN"
                                MsgBox("No hay paquetes de controladores en la carpeta espcificada", vbOKOnly + vbCritical, Label1.Text)
                            Case "FRA"
                                MsgBox("Il n'y a pas de pilotes dans le répertoire spécifié.", vbOKOnly + vbCritical, Label1.Text)
                            Case "PTB", "PTG"
                                MsgBox("Não existem pacotes de controladores na pasta especificada", vbOKOnly + vbCritical, Label1.Text)
                            Case "ITA"
                                MsgBox("Non ci sono pacchetti driver nella cartella specificata", vbOKOnly + vbCritical, Label1.Text)
                        End Select
                    Case 1
                        MsgBox("There are no driver packages in the specified folder", vbOKOnly + vbCritical, Label1.Text)
                    Case 2
                        MsgBox("No hay paquetes de controladores en la carpeta espcificada", vbOKOnly + vbCritical, Label1.Text)
                    Case 3
                        MsgBox("Il n'y a pas de pilotes dans le répertoire spécifié.", vbOKOnly + vbCritical, Label1.Text)
                    Case 4
                        MsgBox("Não existem pacotes de controladores na pasta especificada", vbOKOnly + vbCritical, Label1.Text)
                    Case 5
                        MsgBox("Non ci sono pacchetti driver nella cartella specificata", vbOKOnly + vbCritical, Label1.Text)
                End Select
            End If
            Cursor = Cursors.Arrow
        End If
        Button3.Enabled = ListView1.Items.Count > 0
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            Button3.Enabled = True
            Button4.Enabled = True
        Else
            Button3.Enabled = False
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListView1.FocusedItem.Text <> "" Then
            If CheckedListBox1.Items.Contains(ListView1.FocusedItem.Text) Then
                CheckedListBox1.Items.RemoveAt(CheckedListBox1.FindStringExact(ListView1.FocusedItem.Text))
            End If
            ListView1.Items.Remove(ListView1.FocusedItem)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListView1.Items.Clear()
        CheckedListBox1.Items.Clear()
        Button3.Enabled = False
        Button4.Enabled = False
    End Sub

    Private Sub AddDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Add drivers"
                        Label1.Text = Text
                        Label2.Text = "Please specify the drivers to add by using the buttons below or by dropping them to the list below:"
                        Label3.Text = "You can let the program scan the driver folders present on the list below recursively and add them as well. To do so, tick the entries you'd like to be scanned:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        Button1.Text = "Add file..."
                        Button2.Text = "Add folder..."
                        Button3.Text = "Remove all entries"
                        Button4.Text = "Remove selected entry"
                        CheckBox1.Text = "Force installation of unsigned drivers"
                        CheckBox2.Text = "Commit image after adding drivers"
                        GroupBox1.Text = "Driver files"
                        GroupBox2.Text = "Driver folders"
                        GroupBox3.Text = "Options"
                        ListView1.Columns(0).Text = "File/Folder"
                        ListView1.Columns(1).Text = "Type"
                        OpenFileDialog1.Title = "Specify the driver package to add"
                        FolderBrowserDialog1.Description = "Specify the folder containing driver packages. You will then be able to specify if it needs to be scanned recursively:"
                    Case "ESN"
                        Text = "Añadir controladores"
                        Label1.Text = Text
                        Label2.Text = "Especifique los controladores a añadir usando los botones de abajo o colocándolos en la lista de abajo:"
                        Label3.Text = "Puede dejar que el programa escanee las carpetas de controladores presentes en la lista de abajo de forma recursiva y añadirlos también. Para hacerlo, marque las entradas que le gustaría que fuesen escaneadas:"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Añadir archivo..."
                        Button2.Text = "Añadir carpeta..."
                        Button3.Text = "Eliminar todas las entradas"
                        Button4.Text = "Eliminar entrada seleccionada"
                        CheckBox1.Text = "Forzar instalación de controladores no firmados"
                        CheckBox2.Text = "Guardar imagen tras añadir controladores"
                        GroupBox1.Text = "Archivos de controladores"
                        GroupBox2.Text = "Carpetas de controladores"
                        GroupBox3.Text = "Opciones"
                        ListView1.Columns(0).Text = "Archivo/Carpeta"
                        ListView1.Columns(1).Text = "Tipo"
                        OpenFileDialog1.Title = "Especifique el paquete de controlador a añadir"
                        FolderBrowserDialog1.Description = "Especifique la carpeta que contiene paquetes de controladores. Luego podrá especificar si necesita ser escaneada de forma recursiva:"
                    Case "FRA"
                        Text = "Ajouter des pilotes"
                        Label1.Text = Text
                        Label2.Text = "Veuillez spécifier les pilotes à ajouter en utilisant les boutons ci-dessous ou en les déposant dans la liste ci-dessous :"
                        Label3.Text = "Vous pouvez laisser le programme analyser les répertoires de pilotes présents dans la liste ci-dessous de manière récursive et les ajouter également. Pour ce faire, cochez les entrées que vous souhaitez voir analysées :"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        Button1.Text = "Ajouter un fichier..."
                        Button2.Text = "Ajouter un répertoire..."
                        Button3.Text = "Supprimer toutes les entrées"
                        Button4.Text = "Supprimer l'entrée sélectionnée"
                        CheckBox1.Text = "Forcer l'installation des pilotes non signés"
                        CheckBox2.Text = "Sauvegarder l'image après l'ajout des pilotes"
                        GroupBox1.Text = "Fichiers des pilotes"
                        GroupBox2.Text = "Répertoires des pilotes"
                        GroupBox3.Text = "Paramètres"
                        ListView1.Columns(0).Text = "Fichier/Répertoire"
                        ListView1.Columns(1).Text = "Type"
                        OpenFileDialog1.Title = "Spécifier le paquet de pilotes à ajouter"
                        FolderBrowserDialog1.Description = "Indiquez le répertoire contenant les pilotes. Vous pourrez ensuite préciser s'il doit être analysé de manière récursive :"
                    Case "PTB", "PTG"
                        Text = "Adicionar controladores"
                        Label1.Text = Text
                        Label2.Text = "Especifique os controladores a adicionar utilizando os botões abaixo ou colocando-os na lista abaixo:"
                        Label3.Text = "Pode deixar o programa procurar recursivamente as pastas de controladores presentes na lista abaixo e adicioná-las também. Para o fazer, assinale as entradas que pretende que sejam verificadas:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Adicionar ficheiro..."
                        Button2.Text = "Adicionar pasta..."
                        Button3.Text = "Remover todos os registos"
                        Button4.Text = "Remover entrada selecionada"
                        CheckBox1.Text = "Forçar a instalação de controladores não assinados"
                        CheckBox2.Text = "Confirmar imagem após adicionar controladores"
                        GroupBox1.Text = "Ficheiros de controladores"
                        GroupBox2.Text = "Pastas de controladores"
                        GroupBox3.Text = "Opções"
                        ListView1.Columns(0).Text = "Ficheiro/Pasta"
                        ListView1.Columns(1).Text = "Tipo"
                        OpenFileDialog1.Title = "Especificar o pacote de controladores a adicionar"
                        FolderBrowserDialog1.Description = "Especificar a pasta que contém os pacotes de controladores. Poderá então especificar se é necessário efetuar uma verificação recursiva:"
                    Case "ITA"
                        Text = "Aggiungi driver"
                        Label1.Text = Text
                        Label2.Text = "Specificare i driver da aggiungere utilizzando i pulsanti sottostanti o rilasciandoli nell'elenco sottostante:"
                        Label3.Text = "È possibile lasciare che il programma esegua una scansione ricorsiva delle cartelle dei driver presenti nell'elenco sottostante e aggiungerli. A tale scopo, selezionare le voci che si desidera sottoporre a scansione:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        Button1.Text = "Aggiungi file..."
                        Button2.Text = "Aggiungi cartella..."
                        Button3.Text = "Rimuovi tutte le voci"
                        Button4.Text = "Rimuovi la voce selezionata"
                        CheckBox1.Text = "Forza l'installazione dei driver non firmati"
                        CheckBox2.Text = "Impegna l'immagine dopo l'aggiunta dei driver"
                        GroupBox1.Text = "File di driver"
                        GroupBox2.Text = "Cartelle dei driver"
                        GroupBox3.Text = "Opzioni"
                        ListView1.Columns(0).Text = "File/Cartella"
                        ListView1.Columns(1).Text = "Tipo"
                        OpenFileDialog1.Title = "Specificare il pacchetto driver da aggiungere"
                        FolderBrowserDialog1.Description = "Specificare la cartella contenente i pacchetti di driver. Sarà quindi possibile specificare se è necessario eseguire una scansione ricorsiva:"
                End Select
            Case 1
                Text = "Add drivers"
                Label1.Text = Text
                Label2.Text = "Please specify the drivers to add by using the buttons below or by dropping them to the list below:"
                Label3.Text = "You can let the program scan the driver folders present on the list below recursively and add them as well. To do so, tick the entries you'd like to be scanned:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                Button1.Text = "Add file..."
                Button2.Text = "Add folder..."
                Button3.Text = "Remove all entries"
                Button4.Text = "Remove selected entry"
                CheckBox1.Text = "Force installation of unsigned drivers"
                CheckBox2.Text = "Commit image after adding drivers"
                GroupBox1.Text = "Driver files"
                GroupBox2.Text = "Driver folders"
                GroupBox3.Text = "Options"
                ListView1.Columns(0).Text = "File/Folder"
                ListView1.Columns(1).Text = "Type"
                OpenFileDialog1.Title = "Specify the driver package to add"
                FolderBrowserDialog1.Description = "Specify the folder containing driver packages. You will then be able to specify if it needs to be scanned recursively:"
            Case 2
                Text = "Añadir controladores"
                Label1.Text = Text
                Label2.Text = "Especifique los controladores a añadir usando los botones de abajo o colocándolos en la lista de abajo:"
                Label3.Text = "Puede dejar que el programa escanee las carpetas de controladores presentes en la lista de abajo de forma recursiva y añadirlos también. Para hacerlo, marque las entradas que le gustaría que fuesen escaneadas:"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Añadir archivo..."
                Button2.Text = "Añadir carpeta..."
                Button3.Text = "Eliminar todas las entradas"
                Button4.Text = "Eliminar entrada seleccionada"
                CheckBox1.Text = "Forzar instalación de controladores no firmados"
                CheckBox2.Text = "Guardar imagen tras añadir controladores"
                GroupBox1.Text = "Archivos de controladores"
                GroupBox2.Text = "Carpetas de controladores"
                GroupBox3.Text = "Opciones"
                ListView1.Columns(0).Text = "Archivo/Carpeta"
                ListView1.Columns(1).Text = "Tipo"
                OpenFileDialog1.Title = "Especifique el paquete de controlador a añadir"
                FolderBrowserDialog1.Description = "Especifique la carpeta que contiene paquetes de controladores. Luego podrá especificar si necesita ser escaneada de forma recursiva:"
            Case 3
                Text = "Ajouter des pilotes"
                Label1.Text = Text
                Label2.Text = "Veuillez spécifier les pilotes à ajouter en utilisant les boutons ci-dessous ou en les déposant dans la liste ci-dessous :"
                Label3.Text = "Vous pouvez laisser le programme analyser les répertoires de pilotes présents dans la liste ci-dessous de manière récursive et les ajouter également. Pour ce faire, cochez les entrées que vous souhaitez voir analysées :"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                Button1.Text = "Ajouter un fichier..."
                Button2.Text = "Ajouter un répertoire..."
                Button3.Text = "Supprimer toutes les entrées"
                Button4.Text = "Supprimer l'entrée sélectionnée"
                CheckBox1.Text = "Forcer l'installation des pilotes non signés"
                CheckBox2.Text = "Sauvegarder l'image après l'ajout des pilotes"
                GroupBox1.Text = "Fichiers des pilotes"
                GroupBox2.Text = "Répertoires des pilotes"
                GroupBox3.Text = "Paramètres"
                ListView1.Columns(0).Text = "Fichier/Répertoire"
                ListView1.Columns(1).Text = "Type"
                OpenFileDialog1.Title = "Spécifier le paquet de pilotes à ajouter"
                FolderBrowserDialog1.Description = "Indiquez le répertoire contenant les pilotes. Vous pourrez ensuite préciser s'il doit être analysé de manière récursive :"
            Case 4
                Text = "Adicionar controladores"
                Label1.Text = Text
                Label2.Text = "Especifique os controladores a adicionar utilizando os botões abaixo ou colocando-os na lista abaixo:"
                Label3.Text = "Pode deixar o programa procurar recursivamente as pastas de controladores presentes na lista abaixo e adicioná-las também. Para o fazer, assinale as entradas que pretende que sejam verificadas:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Adicionar ficheiro..."
                Button2.Text = "Adicionar pasta..."
                Button3.Text = "Remover todos os registos"
                Button4.Text = "Remover entrada selecionada"
                CheckBox1.Text = "Forçar a instalação de controladores não assinados"
                CheckBox2.Text = "Confirmar imagem após adicionar controladores"
                GroupBox1.Text = "Ficheiros de controladores"
                GroupBox2.Text = "Pastas de controladores"
                GroupBox3.Text = "Opções"
                ListView1.Columns(0).Text = "Ficheiro/Pasta"
                ListView1.Columns(1).Text = "Tipo"
                OpenFileDialog1.Title = "Especificar o pacote de controladores a adicionar"
                FolderBrowserDialog1.Description = "Especificar a pasta que contém os pacotes de controladores. Poderá então especificar se é necessário efetuar uma verificação recursiva:"
            Case 5
                Text = "Aggiungi driver"
                Label1.Text = Text
                Label2.Text = "Specificare i driver da aggiungere utilizzando i pulsanti sottostanti o rilasciandoli nell'elenco sottostante:"
                Label3.Text = "È possibile lasciare che il programma esegua una scansione ricorsiva delle cartelle dei driver presenti nell'elenco sottostante e aggiungerli. A tale scopo, selezionare le voci che si desidera sottoporre a scansione:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                Button1.Text = "Aggiungi file..."
                Button2.Text = "Aggiungi cartella..."
                Button3.Text = "Rimuovi tutte le voci"
                Button4.Text = "Rimuovi la voce selezionata"
                CheckBox1.Text = "Forza l'installazione dei driver non firmati"
                CheckBox2.Text = "Impegna l'immagine dopo l'aggiunta dei driver"
                GroupBox1.Text = "File di driver"
                GroupBox2.Text = "Cartelle dei driver"
                GroupBox3.Text = "Opzioni"
                ListView1.Columns(0).Text = "File/Cartella"
                ListView1.Columns(1).Text = "Tipo"
                OpenFileDialog1.Title = "Specificare il pacchetto driver da aggiungere"
                FolderBrowserDialog1.Description = "Specificare la cartella contenente i pacchetti di driver. Sarà quindi possibile specificare se è necessario eseguire una scansione ricorsiva:"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            GroupBox3.ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            CheckedListBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            GroupBox3.ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            CheckedListBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        CheckedListBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        CheckBox2.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub ListView1_DragEnter(sender As Object, e As DragEventArgs) Handles ListView1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListView1_DragDrop(sender As Object, e As DragEventArgs) Handles ListView1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each PkgFile In PackageFiles
            If Not File.GetAttributes(PkgFile) = FileAttributes.Directory And Not Path.GetExtension(PkgFile).EndsWith("inf", StringComparison.OrdinalIgnoreCase) Then Continue For
            If File.GetAttributes(PkgFile) = FileAttributes.Directory Then
                Dim msg As String = ""
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg = "The package specified is a folder. You can let DISM scan it recursively to add all drivers in it, or you can specify the drivers to add manually." & CrLf & CrLf & _
                                      "- To let DISM scan this folder recursively, click Yes" & CrLf & _
                                      "- To pick the drivers in this folder manually, click No" & CrLf & _
                                      "- To skip adding this folder, click Cancel"
                            Case "ESN"
                                msg = "El paquete especificado es una carpeta. Puede dejar que DISM la escanee de forma recursiva para añadir todos los controladores en ella, o puede especificar los controladores a añadir manualmente." & CrLf & CrLf & _
                                      "- Para dejar que DISM escanee esta carpeta de forma recursiva, haga clic en Sí" & CrLf & _
                                      "- Para escoger los controladores en esta carpeta manualmente, haga clic en No" & CrLf & _
                                      "- Para omitir la adición de esta carpeta, haga clic en Cancelar"
                            Case "FRA"
                                msg = "Le paquet de pilotes spécifié est un dossier. Vous pouvez laisser DISM l'analyser de manière récursive pour ajouter tous les pilotes qu'il contient, ou vous pouvez spécifier les pilotes à ajouter manuellement." & CrLf & CrLf & _
                                      "- Pour laisser DISM analyser ce dossier de manière récursive, cliquez sur Oui" & CrLf & _
                                      "- Pour sélectionner manuellement les pilotes de ce dossier, cliquez sur Non" & CrLf & _
                                      "- Pour ne pas ajouter ce dossier, cliquez sur Annuler"
                            Case "PTB", "PTG"
                                msg = "O pacote especificado é uma pasta. Pode deixar que o DISM a analise recursivamente para adicionar todos os controladores nela contidos, ou pode especificar os controladores a adicionar manualmente." & CrLf & CrLf & _
                                      "- Para permitir que o DISM verifique esta pasta recursivamente, clique em Sim" & CrLf & _
                                      "- Para escolher manualmente os controladores desta pasta, clique em Não" & CrLf & _
                                      "- Para não adicionar esta pasta, clique em Cancelar"
                            Case "ITA"
                                msg = "Il pacchetto specificato è una cartella. Si può lasciare che DISM la scansioni ricorsivamente per aggiungere tutti i driver in essa contenuti, oppure si possono specificare i driver da aggiungere manualmente." & CrLf & CrLf & _
                                      "- Per consentire a DISM di eseguire la scansione ricorsiva di questa cartella, fare clic su Sì" & CrLf & _
                                      "- Per scegliere manualmente i driver in questa cartella, fare clic su No" & CrLf & _
                                      "- Per non aggiungere questa cartella, fare clic su Annulla"
                        End Select
                    Case 1
                        msg = "The package specified is a folder. You can let DISM scan it recursively to add all drivers in it, or you can specify the drivers to add manually." & CrLf & CrLf & _
                              "- To let DISM scan this folder recursively, click Yes" & CrLf & _
                              "- To pick the drivers in this folder manually, click No" & CrLf & _
                              "- To skip adding this folder, click Cancel"
                    Case 2
                        msg = "El paquete especificado es una carpeta. Puede dejar que DISM la escanee de forma recursiva para añadir todos los controladores en ella, o puede especificar los controladores a añadir manualmente." & CrLf & CrLf & _
                              "- Para dejar que DISM escanee esta carpeta de forma recursiva, haga clic en Sí" & CrLf & _
                              "- Para escoger los controladores en esta carpeta manualmente, haga clic en No" & CrLf & _
                              "- Para omitir la adición de esta carpeta, haga clic en Cancelar"
                    Case 3
                        msg = "Le paquet de pilotes spécifié est un dossier. Vous pouvez laisser DISM l'analyser de manière récursive pour ajouter tous les pilotes qu'il contient, ou vous pouvez spécifier les pilotes à ajouter manuellement." & CrLf & CrLf & _
                              "- Pour laisser DISM analyser ce dossier de manière récursive, cliquez sur Oui" & CrLf & _
                              "- Pour sélectionner manuellement les pilotes de ce dossier, cliquez sur Non" & CrLf & _
                              "- Pour ne pas ajouter ce dossier, cliquez sur Annuler"
                    Case 4
                        msg = "O pacote especificado é uma pasta. Pode deixar que o DISM a analise recursivamente para adicionar todos os controladores nela contidos, ou pode especificar os controladores a adicionar manualmente." & CrLf & CrLf & _
                              "- Para permitir que o DISM verifique esta pasta recursivamente, clique em Sim" & CrLf & _
                              "- Para escolher manualmente os controladores desta pasta, clique em Não" & CrLf & _
                              "- Para não adicionar esta pasta, clique em Cancelar"
                    Case 5
                        msg = "Il pacchetto specificato è una cartella. Si può lasciare che DISM la scansioni ricorsivamente per aggiungere tutti i driver in essa contenuti, oppure si possono specificare i driver da aggiungere manualmente." & CrLf & CrLf & _
                              "- Per consentire a DISM di eseguire la scansione ricorsiva di questa cartella, fare clic su Sì" & CrLf & _
                              "- Per scegliere manualmente i driver in questa cartella, fare clic su No" & CrLf & _
                              "- Per non aggiungere questa cartella, fare clic su Annulla"
                End Select
                Select Case MsgBox(msg, vbYesNoCancel + vbInformation, Label1.Text)
                    Case MsgBoxResult.Yes
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Folder"}))
                                    Case "ESN"
                                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Carpeta"}))
                                    Case "FRA"
                                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Répertoire"}))
                                    Case "PTB", "PTG"
                                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Pasta"}))
                                    Case "ITA"
                                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Cartella"}))
                                End Select
                            Case 1
                                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Folder"}))
                            Case 2
                                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Carpeta"}))
                            Case 3
                                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Répertoire"}))
                            Case 4
                                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Pasta"}))
                            Case 5
                                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Cartella"}))
                        End Select
                        CheckedListBox1.Items.Add(PkgFile)
                        CheckedListBox1.SetItemChecked(CheckedListBox1.Items.IndexOf(PkgFile), True)
                    Case MsgBoxResult.No
                        DriverManualFilePicker.DriverDir = PkgFile
                        DriverManualFilePicker.ShowDialog(Me)
                    Case MsgBoxResult.Cancel
                        Continue For
                End Select
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "File"}))
                            Case "ESN"
                                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Archivo"}))
                            Case "FRA"
                                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Fichier"}))
                            Case "PTB", "PTG"
                                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Ficheiro"}))
                            Case "ITA"
                                ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "File"}))
                        End Select
                    Case 1
                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "File"}))
                    Case 2
                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Archivo"}))
                    Case 3
                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Fichier"}))
                    Case 4
                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "Ficheiro"}))
                    Case 5
                        ListView1.Items.Add(New ListViewItem(New String() {PkgFile, "File"}))
                End Select
            End If
        Next
        Button3.Enabled = ListView1.Items.Count > 0
    End Sub
End Class
