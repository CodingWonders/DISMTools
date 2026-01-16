Imports System.IO
Imports System.Threading
Imports Microsoft.Dism

Public Class MountedImgMgr

    Public ignoreRepeats As Boolean = False

    Private Sub MountedImgMgr_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Mounted image manager"
                        Label1.Text = "Here is an overview of the images that have been mounted on this system. You can look up information about them, and perform some basic tasks. To fully perform image actions with this program though, you need to load the mount directory into a project:"
                        ListView1.Columns(0).Text = "Image file"
                        ListView1.Columns(1).Text = "Index"
                        ListView1.Columns(2).Text = "Mount directory"
                        ListView1.Columns(3).Text = "Status"
                        ListView1.Columns(4).Text = "Read/write permissions?"
                        Button1.Text = "Unmount image"
                        Button2.Text = "Reload servicing"
                        Button3.Text = "Enable write permissions"
                        Button4.Text = "Open mount directory"
                        Button5.Text = "Remove volume images..."
                        Button6.Text = "Load into project"
                    Case "ESN"
                        Text = "Administrador de imágenes montadas"
                        Label1.Text = "Este es un resumen de las imágenes que se han montado en este sistema. Puede consultar información sobre ellas, y realizar algunas tareas básicas. En cambio, si desea realizar todas las operaciones posibles con este programa, necesita cargar el directorio de montaje en un proyecto:"
                        ListView1.Columns(0).Text = "Archivo de imagen"
                        ListView1.Columns(1).Text = "Índice"
                        ListView1.Columns(2).Text = "Directorio de montaje"
                        ListView1.Columns(3).Text = "Estado"
                        ListView1.Columns(4).Text = "¿Permisos de lectura y escritura?"
                        Button1.Text = "Desmontar imagen"
                        Button2.Text = "Recargar servicio"
                        Button3.Text = "Habilitar escritura"
                        Button4.Text = "Abrir directorio de montaje"
                        Button5.Text = "Eliminar imágenes de volumen..."
                        Button6.Text = "Cargar en proyecto"
                    Case "FRA"
                        Text = "Gestionnaire des images montées"
                        Label1.Text = "Voici une vue d'ensemble des images qui ont été montées sur ce système. Vous pouvez rechercher des informations à leur sujet et effectuer quelques tâches de base. Cependant, pour effectuer des actions sur les images avec ce programme, vous devez charger le répertoire de montage dans un projet :"
                        ListView1.Columns(0).Text = "Fichier image"
                        ListView1.Columns(1).Text = "Index"
                        ListView1.Columns(2).Text = "Répertoire de montage"
                        ListView1.Columns(3).Text = "État"
                        ListView1.Columns(4).Text = "Droits de lecture/écriture ?"
                        Button1.Text = "Démonter l'image"
                        Button2.Text = "Recharger le service"
                        Button3.Text = "Activer les droits d'écriture"
                        Button4.Text = "Ouvrir le répertoire de montage"
                        Button5.Text = "Supprimer les images de volume..."
                        Button6.Text = "Charger dans le projet"
                    Case "PTB", "PTG"
                        Text = "Gestor de imagens montadas"
                        Label1.Text = "Aqui está uma visão geral das imagens que foram montadas neste sistema. Pode procurar informação sobre elas e executar algumas tarefas básicas. No entanto, para executar totalmente as acções de imagem com este programa, é necessário carregar o diretório de montagem para um projeto:"
                        ListView1.Columns(0).Text = "Ficheiro de imagem"
                        ListView1.Columns(1).Text = "Índice"
                        ListView1.Columns(2).Text = "Diretório de montagem"
                        ListView1.Columns(3).Text = "Estado"
                        ListView1.Columns(4).Text = "Permissões de leitura/escrita?"
                        Button1.Text = "Desmontar imagem"
                        Button2.Text = "Recarregar a manutenção"
                        Button3.Text = "Ativar permissões de escrita"
                        Button4.Text = "Abrir diretório de montagem"
                        Button5.Text = "Remover imagens de volume..."
                        Button6.Text = "Carregar no projeto"
                    Case "ITA"
                        Text = "Gestione di immagini montate"
                        Label1.Text = "Questa è una panoramica delle immagini che sono state montate su questo sistema. È possibile cercare informazioni su di esse ed eseguire alcune operazioni elementari. Per eseguire completamente le azioni sulle immagini con questo programma, tuttavia, è necessario caricare la directory di montaggio in un progetto:"
                        ListView1.Columns(0).Text = "File immagine"
                        ListView1.Columns(1).Text = "Indice"
                        ListView1.Columns(2).Text = "Directory di montaggio"
                        ListView1.Columns(3).Text = "Stato"
                        ListView1.Columns(4).Text = "Permessi di lettura/scrittura?"
                        Button1.Text = "Smontare l'immagine"
                        Button2.Text = "Ricaricare l'assistenza"
                        Button3.Text = "Abilitare i permessi di scrittura"
                        Button4.Text = "Aprire la directory di montaggio"
                        Button5.Text = "Rimuovere le immagini del volume..."
                        Button6.Text = "Carica nel progetto"
                End Select
            Case 1
                Text = "Mounted image manager"
                Label1.Text = "Here is an overview of the images that have been mounted on this system. You can look up information about them, and perform some basic tasks. To fully perform image actions with this program though, you need to load the mount directory into a project:"
                ListView1.Columns(0).Text = "Image file"
                ListView1.Columns(1).Text = "Index"
                ListView1.Columns(2).Text = "Mount directory"
                ListView1.Columns(3).Text = "Status"
                ListView1.Columns(4).Text = "Read/write permissions?"
                Button1.Text = "Unmount image"
                Button2.Text = "Reload servicing"
                Button3.Text = "Enable write permissions"
                Button4.Text = "Open mount directory"
                Button5.Text = "Remove volume images..."
                Button6.Text = "Load into project"
            Case 2
                Text = "Administrador de imágenes montadas"
                Label1.Text = "Este es un resumen de las imágenes que se han montado en este sistema. Puede consultar información sobre ellas, y realizar algunas tareas básicas. En cambio, si desea realizar todas las operaciones posibles con este programa, necesita cargar el directorio de montaje en un proyecto:"
                ListView1.Columns(0).Text = "Archivo de imagen"
                ListView1.Columns(1).Text = "Índice"
                ListView1.Columns(2).Text = "Directorio de montaje"
                ListView1.Columns(4).Text = "¿Permisos de lectura y escritura?"
                ListView1.Columns(5).Text = "Versión"
                Button1.Text = "Desmontar imagen"
                Button2.Text = "Recargar servicio"
                Button3.Text = "Habilitar escritura"
                Button4.Text = "Abrir directorio de montaje"
                Button5.Text = "Eliminar imágenes de volumen..."
                Button6.Text = "Cargar en proyecto"
            Case 3
                Text = "Gestionnaire des images montées"
                Label1.Text = "Voici une vue d'ensemble des images qui ont été montées sur ce système. Vous pouvez rechercher des informations à leur sujet et effectuer quelques tâches de base. Cependant, pour effectuer des actions sur les images avec ce programme, vous devez charger le répertoire de montage dans un projet :"
                ListView1.Columns(0).Text = "Fichier image"
                ListView1.Columns(1).Text = "Index"
                ListView1.Columns(2).Text = "Répertoire de montage"
                ListView1.Columns(3).Text = "État"
                ListView1.Columns(4).Text = "Droits de lecture/écriture ?"
                Button1.Text = "Démonter l'image"
                Button2.Text = "Recharger le service"
                Button3.Text = "Activer les droits d'écriture"
                Button4.Text = "Ouvrir le répertoire de montage"
                Button5.Text = "Supprimer les images de volume..."
                Button6.Text = "Charger dans le projet"
            Case 4
                Text = "Gestor de imagens montadas"
                Label1.Text = "Aqui está uma visão geral das imagens que foram montadas neste sistema. Pode procurar informação sobre elas e executar algumas tarefas básicas. No entanto, para executar totalmente as acções de imagem com este programa, é necessário carregar o diretório de montagem para um projeto:"
                ListView1.Columns(0).Text = "Ficheiro de imagem"
                ListView1.Columns(1).Text = "Índice"
                ListView1.Columns(2).Text = "Diretório de montagem"
                ListView1.Columns(3).Text = "Estado"
                ListView1.Columns(4).Text = "Permissões de leitura/escrita?"
                Button1.Text = "Desmontar imagem"
                Button2.Text = "Recarregar a manutenção"
                Button3.Text = "Ativar permissões de escrita"
                Button4.Text = "Abrir diretório de montagem"
                Button5.Text = "Remover imagens de volume..."
                Button6.Text = "Carregar no projeto"
            Case 5
                Text = "Gestione di immagini montate"
                Label1.Text = "Questa è una panoramica delle immagini che sono state montate su questo sistema. È possibile cercare informazioni su di esse ed eseguire alcune operazioni elementari. Per eseguire completamente le azioni sulle immagini con questo programma, tuttavia, è necessario caricare la directory di montaggio in un progetto:"
                ListView1.Columns(0).Text = "File immagine"
                ListView1.Columns(1).Text = "Indice"
                ListView1.Columns(2).Text = "Directory di montaggio"
                ListView1.Columns(3).Text = "Stato"
                ListView1.Columns(4).Text = "Permessi di lettura/scrittura?"
                Button1.Text = "Smontare l'immagine"
                Button2.Text = "Ricaricare l'assistenza"
                Button3.Text = "Abilitare i permessi di scrittura"
                Button4.Text = "Aprire la directory di montaggio"
                Button5.Text = "Rimuovere le immagini del volume..."
                Button6.Text = "Carica nel progetto"
        End Select
        CheckForIllegalCrossThreadCalls = False
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)

        ' Subscribe to MainForm event to get updates
        AddHandler MainForm.MountedImagesUpdated, AddressOf OnMountedImagesUpdated
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            ' Enable buttons according to the image conditions
            If ListView1.SelectedItems.Count > 0 Then
                Button1.Enabled = True
                Dim markedImage As WindowsImage = MainForm.MountedImageList.ElementAtOrDefault(ListView1.FocusedItem.Index)
                If markedImage Is Nothing Then Exit Sub
                If markedImage.ImageMountStatus <> DismMountStatus.Ok Then
                    Button2.Enabled = True
                    Select Case markedImage.ImageMountStatus
                        Case DismMountStatus.NeedsRemount
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            Button2.Text = "Reload servicing"
                                        Case "ESN"
                                            Button2.Text = "Recargar servicio"
                                        Case "FRA"
                                            Button2.Text = "Recharger le service"
                                        Case "PTB", "PTG"
                                            Button2.Text = "Recarregar o serviço"
                                        Case "ITA"
                                            Button2.Text = "Ricarica servizio"
                                    End Select
                                Case 1
                                    Button2.Text = "Reload servicing"
                                Case 2
                                    Button2.Text = "Recargar servicio"
                                Case 3
                                    Button2.Text = "Recharger le service"
                                Case 4
                                    Button2.Text = "Recarregar o serviço"
                                Case 5
                                    Button2.Text = "Ricarica servizio"
                            End Select
                        Case DismMountStatus.Invalid
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            Button2.Text = "Repair component store"
                                        Case "ESN"
                                            Button2.Text = "Reparar almacén de componentes"
                                        Case "FRA"
                                            Button2.Text = "Réparer le stock de composants"
                                        Case "PTB", "PTG"
                                            Button2.Text = "Reparação do armazém de componentes"
                                        Case "ITA"
                                            Button2.Text = "Ripara il magazzino dei componenti"
                                    End Select
                                Case 1
                                    Button2.Text = "Repair component store"
                                Case 2
                                    Button2.Text = "Reparar almacén de componentes"
                                Case 3
                                    Button2.Text = "Réparer le stock de composants"
                                Case 4
                                    Button2.Text = "Reparação do armazém de componentes"
                                Case 5
                                    Button2.Text = "Ripara il magazzino dei componenti"
                            End Select
                    End Select
                Else
                    Button2.Enabled = False
                End If
                Button3.Enabled = (markedImage.ImageMountMode = DismMountMode.ReadOnly)
                Button4.Enabled = True
                Button5.Enabled = True
                If MainForm.isProjectLoaded And MainForm.MountDir = "N/A" Or Not Directory.Exists(MainForm.MountDir & "\Windows") Then
                    Button6.Enabled = True
                Else
                    Button6.Enabled = False
                End If
                Button7.Enabled = True
            Else
                Button1.Enabled = False
                Button2.Enabled = False
                Button3.Enabled = False
                Button4.Enabled = False
                Button5.Enabled = False
                Button6.Enabled = False
                Button7.Enabled = False
            End If
        Catch ex As Exception
            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
            Button7.Enabled = False
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Directory.Exists(ListView1.FocusedItem.SubItems(2).Text) Then
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", ListView1.FocusedItem.SubItems(2).Text)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        DynaLog.LogMessage("Preparing to load the selected image in loaded project...")
        Dim useAlternateMethod As Boolean = False
        If MainForm.isProjectLoaded Then
            For x = 0 To ListView1.Columns.Count - 1
                If ListView1.FocusedItem.SubItems(x).Text = "" Or ListView1.FocusedItem.SubItems(x).Text = Nothing Then
                    useAlternateMethod = True
                    Exit For
                End If
            Next
        End If
        If useAlternateMethod Then
            Dim ImageToLoad As WindowsImage = MainForm.MountedImageList.FirstOrDefault(Function(image) image.ImageMountDirectory = ListView1.FocusedItem.SubItems(2).Text)
            If ImageToLoad IsNot Nothing Then
                MainForm.MountDir = ImageToLoad.ImageMountDirectory
                MainForm.ImgIndex = ImageToLoad.ImageIndex
                MainForm.SourceImg = ImageToLoad.ImageFile
                MainForm.isReadOnly = (ImageToLoad.ImageMountMode = DismMountMode.ReadOnly)
            End If
            MainForm.UpdateProjProperties(True, If(MainForm.isReadOnly, True, False))
            MainForm.SaveDTProj()
        Else
            MainForm.MountDir = ListView1.FocusedItem.SubItems(2).Text
            MainForm.ImgIndex = ListView1.FocusedItem.SubItems(1).Text
            MainForm.SourceImg = ListView1.FocusedItem.SubItems(0).Text
            IIf(ListView1.FocusedItem.SubItems(4).Text = "Yes", MainForm.isReadOnly = False, MainForm.isReadOnly = True)
            MainForm.UpdateProjProperties(True, If(MainForm.isReadOnly, True, False))
            MainForm.SaveDTProj()
        End If
        Button6.Enabled = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Checking status of the selected mount image...")
        Dim SelectedImage As WindowsImage = MainForm.MountedImageList.ElementAtOrDefault(ListView1.FocusedItem.Index)
        If SelectedImage IsNot Nothing Then
            Select Case SelectedImage.ImageMountStatus
                Case DismMountStatus.NeedsRemount
                    DynaLog.LogMessage("The selected image needs to be remounted.")
                    ProgressPanel.MountDir = ListView1.FocusedItem.SubItems(2).Text
                    ProgressPanel.OperationNum = 18
                    ProgressPanel.ShowDialog(Me)
                    Button2.Enabled = False
                Case DismMountStatus.Invalid
                    DynaLog.LogMessage("The selected image needs to be repaired.")
                    Visible = False
                    ImgCleanup.ComboBox1.SelectedIndex = 6
                    ImgCleanup.ShowDialog(MainForm)
                    Visible = True
            End Select
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DynaLog.LogMessage("Determining if changes can be written to the selected Windows image...")
        Select Case MainForm.MountedImageList(ListView1.FocusedItem.Index).ImageMountMode
            Case DismMountMode.ReadWrite
                DynaLog.LogMessage("The image has been mounted with read-write permissions.")
                MainForm.ImgUMountPopupCMS.Show(sender, New Point(24, Button1.Height * 0.75))
            Case DismMountMode.ReadOnly
                DynaLog.LogMessage("The image has been mounted with read-only permissions. No tasks other than unmounting whilst discarding changes can be made.")
                ' Unmount the image discarding changes
                If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
                ProgressPanel.OperationNum = 21
                ProgressPanel.UMountLocalDir = False
                ProgressPanel.RandomMountDir = ListView1.FocusedItem.SubItems(2).Text   ' Hope there isn't anything to set here
                ProgressPanel.UMountImgIndex = ListView1.FocusedItem.SubItems(1).Text
                ProgressPanel.MountDir = ""
                ProgressPanel.UMountOp = 1
                ProgressPanel.ShowDialog(Me)
        End Select
    End Sub

    Private Sub MountedImgMgr_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        RemoveHandler MainForm.MountedImagesUpdated, AddressOf OnMountedImagesUpdated
    End Sub

    Private Sub OnMountedImagesUpdated(sender As Object, e As EventArgs)
        Try
            ' Force a refresh of the ListView on the UI thread
            If InvokeRequired Then
                BeginInvoke(New MethodInvoker(AddressOf RefreshMountedList))
            Else
                RefreshMountedList()
            End If
        Catch ex As Exception
            DynaLog.LogMessage("OnMountedImagesUpdated error: " & ex.Message)
        End Try
    End Sub

    Private Sub RefreshMountedList()
        If ListView1.Items.Count <> MainForm.MountedImageList.Count Then
            DynaLog.LogMessage("There is a different amount of images mounted now. Forcing refresh of lists...")
            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
            Button7.Enabled = False
            Try
                ListView1.Items.Clear()
                For Each MountedImage In MainForm.MountedImageList
                    ListView1.Items.Add(New ListViewItem(New String() {MountedImage.ImageFile, MountedImage.ImageIndex, MountedImage.ImageMountDirectory, MountedImage.MountStatusToString(MainForm.Language), MountedImage.MountModeToString(MainForm.Language)}))
                Next
                ignoreRepeats = True
            Catch ex As Exception
                DynaLog.LogMessage("RefreshMountedList error: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        DynaLog.LogMessage("Preparing to remove volume images from selected image file...")
        DynaLog.LogMessage("Mounted image detector might be busy. Stopping it if it is...")
        MainForm.StopMountedImageDetector()
        ImgIndexDelete.TextBox1.Text = ListView1.FocusedItem.SubItems(0).Text
        ImgIndexDelete.ShowDialog(Me)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If MainForm.MountedImageList.Select(Function(image) image.ImageMountDirectory).Count > 0 Then
            DynaLog.LogMessage("Enabling write permissions on the selected image...")
            MainForm.EnableWritePermissions(ListView1.FocusedItem.SubItems(0).Text, CInt(ListView1.FocusedItem.SubItems(1).Text), ListView1.FocusedItem.SubItems(2).Text)
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        DynaLog.LogMessage("Showing special tasks...")
        MainForm.ImgSpecialToolsCMS.Show(sender, New Point(8, Button7.Height * 0.75))
    End Sub
End Class