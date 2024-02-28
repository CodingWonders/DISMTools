Imports System.Windows.Forms
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports System.Threading

Public Class ImgIndexDelete

    Public IndexPositions(65535) As String
    Public IndexNames(65535) As String

    Public IndexRemovalNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        Array.Clear(IndexRemovalNames, 0, IndexRemovalNames.Last)
        Dim imageCount As Integer = ListView1.CheckedItems.Count
        ' Detect whether volume indexes have been marked for removal
        If ListView1.CheckedItems.Count <= 0 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("Please select volume images to remove from this image, and try again.", vbOKOnly + vbCritical, "Remove a volume image")
                        Case "ESN"
                            MsgBox("Seleccione imágenes de volumen para eliminar de esta imagen, e inténtelo de nuevo.", vbOKOnly + vbCritical, "Eliminar una imagen de volumen")
                        Case "FRA"
                            MsgBox("Veuillez sélectionner les images de volume à supprimer de cette image et réessayer.", vbOKOnly + vbCritical, "Supprimer une image de volume")
                        Case "PTB", "PTG"
                            MsgBox("Seleccione as imagens de volume a remover desta imagem e tente novamente.", vbOKOnly + vbCritical, "Remover uma imagem de volume")
                    End Select
                Case 1
                    MsgBox("Please select volume images to remove from this image, and try again.", vbOKOnly + vbCritical, "Remove a volume image")
                Case 2
                    MsgBox("Seleccione imágenes de volumen para eliminar de esta imagen, e inténtelo de nuevo.", vbOKOnly + vbCritical, "Eliminar una imagen de volumen")
                Case 3
                    MsgBox("Veuillez sélectionner les images de volume à supprimer de cette image et réessayer.", vbOKOnly + vbCritical, "Supprimer une image de volume")
                Case 4
                    MsgBox("Seleccione as imagens de volume a remover desta imagem e tente novamente.", vbOKOnly + vbCritical, "Remover uma imagem de volume")
            End Select
            Exit Sub
        End If
        ProgressPanel.imgIndexDeletionSourceImg = TextBox1.Text
        ' Detect whether image is mounted
        ProgressPanel.imgIndexDeletionUnmount = False
        If MainForm.MountedImageImgFiles.Contains(TextBox1.Text) Then
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "The program has detected that this image is mounted. In order to remove volume images from a file, it needs to be unmounted. You can remount it later, if you want." & CrLf & CrLf & "Do note that this will unmount the image without saving changes. Make sure all your changes have been saved before proceeding." & CrLf & CrLf & "Do you want to unmount this image?"
                        Case "ESN"
                            msg = "El programa ha detectado que esta imagen está montada. Para eliminar imágenes de volumen de este archivo, debe ser desmontado. Puede remontarlo más tarde, si lo desea." & CrLf & CrLf & "Dese cuenta de que esto desmontará la imagen descartando los cambios. Asegúrese de que todos sus cambios hayan sido guardados antes de proceder." & CrLf & CrLf & "¿Desea desmontar esta imagen?"
                        Case "FRA"
                            msg = "Le programme a détecté que cette image est montée. Pour supprimer les images de volume d'un fichier, celui-ci doit être démonté. Vous pourrez la remonter plus tard, si vous le souhaitez." & CrLf & CrLf & "Notez que cette opération démontera l'image sans enregistrer les modifications. Assurez-vous que toutes vos modifications ont été enregistrées avant de continuer." & CrLf & CrLf & "Voulez-vous démonter cette image ?"
                        Case "PTB", "PTG"
                            msg = "O programa detectou que esta imagem está montada. Para remover imagens de volume de um ficheiro, este tem de ser desmontado. Pode voltar a montá-la mais tarde, se quiser." & CrLf & CrLf & "Tenha em atenção que isto irá desmontar a imagem sem guardar as alterações. Certifique-se de que todas as suas alterações foram guardadas antes de continuar." & CrLf & CrLf & "Deseja desmontar esta imagem?"
                    End Select
                Case 1
                    msg = "The program has detected that this image is mounted. In order to remove volume images from a file, it needs to be unmounted. You can remount it later, if you want." & CrLf & CrLf & "Do note that this will unmount the image without saving changes. Make sure all your changes have been saved before proceeding." & CrLf & CrLf & "Do you want to unmount this image?"
                Case 2
                    msg = "El programa ha detectado que esta imagen está montada. Para eliminar imágenes de volumen de este archivo, debe ser desmontado. Puede remontarlo más tarde, si lo desea." & CrLf & CrLf & "Dese cuenta de que esto desmontará la imagen descartando los cambios. Asegúrese de que todos sus cambios hayan sido guardados antes de proceder." & CrLf & CrLf & "¿Desea desmontar esta imagen?"
                Case 3
                    msg = "Le programme a détecté que cette image est montée. Pour supprimer les images de volume d'un fichier, celui-ci doit être démonté. Vous pourrez la remonter plus tard, si vous le souhaitez." & CrLf & CrLf & "Notez que cette opération démontera l'image sans enregistrer les modifications. Assurez-vous que toutes vos modifications ont été enregistrées avant de continuer." & CrLf & CrLf & "Voulez-vous démonter cette image ?"
                Case 4
                    msg = "O programa detectou que esta imagem está montada. Para remover imagens de volume de um ficheiro, este tem de ser desmontado. Pode voltar a montá-la mais tarde, se quiser." & CrLf & CrLf & "Tenha em atenção que isto irá desmontar a imagem sem guardar as alterações. Certifique-se de que todas as suas alterações foram guardadas antes de continuar." & CrLf & CrLf & "Deseja desmontar esta imagem?"
            End Select
            If MsgBox(msg, vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.Yes Then
                Try
                    For x = 0 To Array.LastIndexOf(MainForm.MountedImageImgFiles, MainForm.MountedImageImgFiles.Last)
                        If MainForm.MountedImageImgFiles(x) = TextBox1.Text Then
                            ProgressPanel.imgIndexDeletionUnmount = True
                            ProgressPanel.UMountImgIndex = MainForm.MountedImageImgIndexes(x)
                            If MainForm.MountedImageMountDirs(x) = MainForm.MountDir Then ProgressPanel.UMountLocalDir = True Else ProgressPanel.UMountLocalDir = False
                            ProgressPanel.MountDir = MainForm.MountedImageMountDirs(x)
                            ProgressPanel.UMountOp = 1
                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
            Else
                Exit Sub
            End If
        End If
        For x = 0 To ListView1.CheckedItems.Count - 1
            IndexRemovalNames(x) = ListView1.CheckedItems(x).SubItems(1).Text
        Next
        For x = 0 To IndexRemovalNames.Length - 1
            ProgressPanel.imgIndexDeletionNames(x) = IndexRemovalNames(x)
        Next
        ProgressPanel.imgIndexDeletionLastName = ListView1.CheckedItems(imageCount - 1).SubItems(1).Text.Replace("{ListViewSubItem: {", "").Trim().Replace("}}", "").Trim()
        imageCount = ListView1.CheckedItems.Count
        ProgressPanel.imgIndexDeletionCount = imageCount
        If CheckBox1.Checked Then
            ProgressPanel.imgIndexDeletionIntCheck = True
        Else
            ProgressPanel.imgIndexDeletionIntCheck = False
        End If
        ProgressPanel.OperationNum = 9
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgIndexDelete_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Remove a volume image"
                        Label1.Text = Text
                        Label2.Text = "Source image:"
                        Label3.Text = "Please mark the volume images to delete on the left. The image will then have the indexes shown on the right"
                        Label4.Text = "Getting indexes from the image. Please wait..."
                        Button1.Text = "Browse..."
                        Button2.Text = "Use mounted image"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        CheckBox1.Text = "Check image integrity"
                        ListView1.Columns(0).Text = "Index"
                        ListView1.Columns(1).Text = "Image name"
                        ListView2.Columns(0).Text = "Index"
                        ListView2.Columns(1).Text = "Image name"
                        GroupBox1.Text = "Volume images"
                    Case "ESN"
                        Text = "Eliminar una imagen de volumen"
                        Label1.Text = Text
                        Label2.Text = "Imagen:"
                        Label3.Text = "Marque las imágenes de volumen a eliminar en la parte izquierda. La imagen tendrá luego los índices mostrados en la parte derecha"
                        Label4.Text = "Obteniendo índices de la imagen. Espere..."
                        Button1.Text = "Examinar..."
                        Button2.Text = "Usar imagen montada"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        CheckBox1.Text = "Comprobar integridad de imagen"
                        ListView1.Columns(0).Text = "Índice"
                        ListView1.Columns(1).Text = "Nombre de imagen"
                        ListView2.Columns(0).Text = "Índice"
                        ListView2.Columns(1).Text = "Nombre de imagen"
                        GroupBox1.Text = "Imágenes de volumen"
                    Case "FRA"
                        Text = "Supprimer une image de volume"
                        Label1.Text = Text
                        Label2.Text = "Image source :"
                        Label3.Text = "Veuillez marquer les images de volume à supprimer sur la gauche. L'image aura alors les index affichés à droite."
                        Label4.Text = "Obtention des index de l'image en cours. Veuillez patienter..."
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Utiliser l'image montée"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        CheckBox1.Text = "Vérifier l'intégrité de l'image"
                        ListView1.Columns(0).Text = "Index"
                        ListView1.Columns(1).Text = "Nom de l'image"
                        ListView2.Columns(0).Text = "Index de l'image"
                        ListView2.Columns(1).Text = "Nom de l'image"
                        GroupBox1.Text = "Images de volume"
                    Case "PTB", "PTG"
                        Text = "Remover uma imagem de volume"
                        Label1.Text = Text
                        Label2.Text = "Imagem de origem:"
                        Label3.Text = "Marque as imagens de volume a eliminar à esquerda. A imagem ficará então com os índices apresentados à direita"
                        Label4.Text = "A obter os índices da imagem. Aguarde..."
                        Button1.Text = "Navegar..."
                        Button2.Text = "Utilizar imagem montada"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        CheckBox1.Text = "Verificar a integridade da imagem"
                        ListView1.Columns(0).Text = "Índice"
                        ListView1.Columns(1).Text = "Nome da imagem"
                        ListView2.Columns(0).Text = "Índice"
                        ListView2.Columns(1).Text = "Nome da imagem"
                        GroupBox1.Text = "Imagens de volume"
                End Select
            Case 1
                Text = "Remove a volume image"
                Label1.Text = Text
                Label2.Text = "Source image:"
                Label3.Text = "Please mark the volume images to delete on the left. The image will then have the indexes shown on the right"
                Label4.Text = "Getting indexes from the image. Please wait..."
                Button1.Text = "Browse..."
                Button2.Text = "Use mounted image"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                CheckBox1.Text = "Check image integrity"
                ListView1.Columns(0).Text = "Index"
                ListView1.Columns(1).Text = "Image name"
                ListView2.Columns(0).Text = "Index"
                ListView2.Columns(1).Text = "Image name"
                GroupBox1.Text = "Volume images"
            Case 2
                Text = "Eliminar una imagen de volumen"
                Label1.Text = Text
                Label2.Text = "Imagen:"
                Label3.Text = "Marque las imágenes de volumen a eliminar en la parte izquierda. La imagen tendrá luego los índices mostrados en la parte derecha"
                Label4.Text = "Obteniendo índices de la imagen. Espere..."
                Button1.Text = "Examinar..."
                Button2.Text = "Usar imagen montada"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                CheckBox1.Text = "Comprobar integridad de imagen"
                ListView1.Columns(0).Text = "Índice"
                ListView1.Columns(1).Text = "Nombre de imagen"
                ListView2.Columns(0).Text = "Índice"
                ListView2.Columns(1).Text = "Nombre de imagen"
                GroupBox1.Text = "Imágenes de volumen"
            Case 3
                Text = "Supprimer une image de volume"
                Label1.Text = Text
                Label2.Text = "Image source :"
                Label3.Text = "Veuillez marquer les images de volume à supprimer sur la gauche. L'image aura alors les index affichés à droite."
                Label4.Text = "Obtention des index de l'image en cours. Veuillez patienter..."
                Button1.Text = "Parcourir..."
                Button2.Text = "Utiliser l'image montée"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                CheckBox1.Text = "Vérifier l'intégrité de l'image"
                ListView1.Columns(0).Text = "Index"
                ListView1.Columns(1).Text = "Nom de l'image"
                ListView2.Columns(0).Text = "Index de l'image"
                ListView2.Columns(1).Text = "Nom de l'image"
                GroupBox1.Text = "Images de volume"
            Case 4
                Text = "Remover uma imagem de volume"
                Label1.Text = Text
                Label2.Text = "Imagem de origem:"
                Label3.Text = "Marque as imagens de volume a eliminar à esquerda. A imagem ficará então com os índices apresentados à direita"
                Label4.Text = "A obter os índices da imagem. Aguarde..."
                Button1.Text = "Navegar..."
                Button2.Text = "Utilizar imagem montada"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                CheckBox1.Text = "Verificar a integridade da imagem"
                ListView1.Columns(0).Text = "Índice"
                ListView1.Columns(1).Text = "Nome da imagem"
                ListView2.Columns(0).Text = "Índice"
                ListView2.Columns(1).Text = "Nome da imagem"
                GroupBox1.Text = "Imagens de volume"
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.SourceImg = "N/A" Or Not File.Exists(MainForm.SourceImg) Or MainForm.OnlineManagement Or MainForm.OfflineManagement Then Button2.Enabled = False Else Button2.Enabled = True
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            ListView2.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            ListView2.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        ListView2.ForeColor = ForeColor

        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))

        ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
        Dim bm As New Bitmap(ListView2.ClientSize.Width, ListView2.ClientSize.Height)
        Graphics.FromImage(bm).Clear(ListView2.BackColor)
        ListView2.BackgroundImage = bm
    End Sub

    Sub GetImageIndexInfo(SourceImage As String)
        If MainForm.MountedImageDetectorBW.IsBusy Then MainForm.MountedImageDetectorBW.CancelAsync()
        While MainForm.MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Threading.Thread.Sleep(100)
        End While
        MainForm.WatcherTimer.Enabled = False
        If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
        While MainForm.WatcherBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        RemoveHandler ListView1.ItemChecked, AddressOf ListView1_ItemChecked
        ' Clear arrays
        Array.Clear(IndexNames, 0, IndexNames.Length)
        Array.Clear(IndexPositions, 0, IndexPositions.Length)
        ListView1.Items.Clear()
        ListView2.Items.Clear()
        Label4.Visible = True
        Dim infoCollection As DismImageInfoCollection = Nothing
        Try
            infoCollection = DismApi.GetImageInfo(SourceImage)
        Catch ex As DismNotInitializedException
            DismApi.Initialize(DismLogLevel.LogErrors)
            infoCollection = DismApi.GetImageInfo(SourceImage)
        End Try
        If infoCollection.Count <= 1 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This image only contains 1 index. You can't remove volume images from this file", vbOKOnly + vbExclamation, "Remove a volume image")
                        Case "ESN"
                            MsgBox("Esta imagen solo contiene 1 índice. No puede eliminar imágenes de volumen de este archivo", vbOKOnly + vbExclamation, "Eliminar una imagen de volumen")
                        Case "FRA"
                            MsgBox("Cette image ne contient qu'un seul index. Vous ne pouvez pas supprimer les images de volume de ce fichier.", vbOKOnly + vbExclamation, "Supprimer une image de volume")
                        Case "PTB", "PTG"
                            MsgBox("Esta imagem contém apenas 1 índice. Não é possível remover imagens de volume deste ficheiro", vbOKOnly + vbExclamation, "Remover uma imagem de volume")
                    End Select
                Case 1
                    MsgBox("This image only contains 1 index. You can't remove volume images from this file", vbOKOnly + vbExclamation, "Remove a volume image")
                Case 2
                    MsgBox("Esta imagen solo contiene 1 índice. No puede eliminar imágenes de volumen de este archivo", vbOKOnly + vbExclamation, "Eliminar una imagen de volumen")
                Case 3
                    MsgBox("Cette image ne contient qu'un seul index. Vous ne pouvez pas supprimer les images de volume de ce fichier.", vbOKOnly + vbExclamation, "Supprimer une image de volume")
                Case 4
                    MsgBox("Esta imagem contém apenas 1 índice. Não é possível remover imagens de volume deste ficheiro", vbOKOnly + vbExclamation, "Remover uma imagem de volume")
            End Select
            Label4.Visible = False
            OK_Button.Enabled = False
            Exit Sub
        Else
            For Each indexInfo As DismImageInfo In infoCollection
                ListView1.Items.Add(New ListViewItem(New String() {indexInfo.ImageIndex, indexInfo.ImageName}))
                ListView2.Items.Add(New ListViewItem(New String() {indexInfo.ImageIndex, indexInfo.ImageName}))
            Next
        End If
        OK_Button.Enabled = True
        DismApi.Shutdown()
        Label4.Visible = False
        AddHandler ListView1.ItemChecked, AddressOf ListView1_ItemChecked
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
        MainForm.WatcherTimer.Enabled = True
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If File.Exists(TextBox1.Text) Then
            If Path.GetExtension(TextBox1.Text).Equals(".wim", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(TextBox1.Text).Equals(".esd", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(TextBox1.Text).Equals(".vhd", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(TextBox1.Text).Equals(".vhdx", StringComparison.OrdinalIgnoreCase) Then
                GetImageIndexInfo(TextBox1.Text)
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub ListView1_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView1.ItemChecked
        ListView2.Items.Clear()
        Try
            For x = 0 To ListView1.Items.Count - 1
                If ListView1.Items(x).Checked Then
                    Continue For
                Else
                    If CInt(ListView1.Items(x).Text) - 1 < 0 Then
                        ListView2.Items.Add(New ListViewItem(New String() {CInt(ListView1.Items(x).Text) + 1, ListView1.Items(x).SubItems(1).Text}))
                    Else
                        ListView2.Items.Add(New ListViewItem(New String() {ListView1.Items(x).Text, ListView1.Items(x).SubItems(1).Text}))
                    End If
                End If
            Next
        Catch ex As Exception
            Exit Sub
        End Try
        If ListView2.Items.Count < 1 Then
            OK_Button.Enabled = False
        Else
            OK_Button.Enabled = True
        End If
    End Sub
End Class
