Imports System.IO
Public Class MountedImgMgr

    Public ignoreRepeats As Boolean = False

    Private Sub MountedImgMgr_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Mounted image manager"
                        Label1.Text = "Here is an overview of the images that have been mounted on this system. You can look up information about them, and perform some basic tasks. To fully perform image actions with this program though, you need to load the mount directory into a project:"
                        ListView1.Columns(0).Text = "Image file"
                        ListView1.Columns(1).Text = "Index"
                        ListView1.Columns(2).Text = "Mount directory"
                        ListView1.Columns(3).Text = "Status"
                        ListView1.Columns(4).Text = "Read/write permissions?"
                        ListView1.Columns(5).Text = "Version"
                        Button1.Text = "Unmount image"
                        Button2.Text = "Reload servicing"
                        Button3.Text = "Enable write permissions"
                        Button4.Text = "Open mount directory"
                        Button5.Text = "Remove volume images..."
                        Button6.Text = "Load into project"
                        LinkLabel1.Text = "This view is being updated in real time, which may cause a higher CPU usage. Change image detection settings..."
                        LinkLabel1.LinkArea = New LinkArea(77, 34)
                    Case "ESN"
                        Text = "Administrador de imágenes montadas"
                        Label1.Text = "Este es un resumen de las imágenes que se han montado en este sistema. Puede consultar información sobre ellas, y realizar algunas tareas básicas. En cambio, si desea realizar todas las operaciones posibles con este programa, necesita cargar el directorio de montaje en un proyecto:"
                        ListView1.Columns(0).Text = "Archivo de imagen"
                        ListView1.Columns(1).Text = "Índice"
                        ListView1.Columns(2).Text = "Directorio de montaje"
                        ListView1.Columns(3).Text = "Estado"
                        ListView1.Columns(4).Text = "¿Permisos de lectura y escritura?"
                        ListView1.Columns(5).Text = "Versión"
                        Button1.Text = "Desmontar imagen"
                        Button2.Text = "Recargar servicio"
                        Button3.Text = "Habilitar escritura"
                        Button4.Text = "Abrir directorio de montaje"
                        Button5.Text = "Eliminar imágenes de volumen..."
                        Button6.Text = "Cargar en proyecto"
                        LinkLabel1.Text = "Esta vista está siendo actualizada en tiempo real, lo que podría significar un aumento en el uso de CPU. Cambiar configuraciones de detección de imágenes..."
                        LinkLabel1.LinkArea = New LinkArea(105, 51)
                End Select
            Case 1
                Text = "Mounted image manager"
                Label1.Text = "Here is an overview of the images that have been mounted on this system. You can look up information about them, and perform some basic tasks. To fully perform image actions with this program though, you need to load the mount directory into a project:"
                ListView1.Columns(0).Text = "Image file"
                ListView1.Columns(1).Text = "Index"
                ListView1.Columns(2).Text = "Mount directory"
                ListView1.Columns(3).Text = "Status"
                ListView1.Columns(4).Text = "Read/write permissions?"
                ListView1.Columns(5).Text = "Version"
                Button1.Text = "Unmount image"
                Button2.Text = "Reload servicing"
                Button3.Text = "Enable write permissions"
                Button4.Text = "Open mount directory"
                Button5.Text = "Remove volume images..."
                Button6.Text = "Load into project"
                LinkLabel1.Text = "This view is being updated in real time, which may cause a higher CPU usage. Change image detection settings..."
                LinkLabel1.LinkArea = New LinkArea(77, 34)
            Case 2
                Text = "Administrador de imágenes montadas"
                Label1.Text = "Este es un resumen de las imágenes que se han montado en este sistema. Puede consultar información sobre ellas, y realizar algunas tareas básicas. En cambio, si desea realizar todas las operaciones posibles con este programa, necesita cargar el directorio de montaje en un proyecto:"
                ListView1.Columns(0).Text = "Archivo de imagen"
                ListView1.Columns(1).Text = "Índice"
                ListView1.Columns(2).Text = "Directorio de montaje"
                ListView1.Columns(3).Text = "Estado"
                ListView1.Columns(4).Text = "¿Permisos de lectura y escritura?"
                ListView1.Columns(5).Text = "Versión"
                Button1.Text = "Desmontar imagen"
                Button2.Text = "Recargar servicio"
                Button3.Text = "Habilitar escritura"
                Button4.Text = "Abrir directorio de montaje"
                Button5.Text = "Eliminar imágenes de volumen..."
                Button6.Text = "Cargar en proyecto"
                LinkLabel1.Text = "Esta vista está siendo actualizada en tiempo real, lo que podría significar un aumento en el uso de CPU. Cambiar configuraciones de detección de imágenes..."
                LinkLabel1.LinkArea = New LinkArea(105, 51)
        End Select
        Control.CheckForIllegalCrossThreadCalls = False
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        DetectorBW.RunWorkerAsync()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Options.TabControl1.SelectedIndex = 8
        If MainForm.WindowState = FormWindowState.Minimized Then MainForm.WindowState = FormWindowState.Normal
        Options.ShowDialog(MainForm)
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        ' Enable buttons according to the image conditions
        If ListView1.SelectedItems.Count > 0 Then
            Button1.Enabled = True
            If MainForm.MountedImageImgStatuses(ListView1.FocusedItem.Index) > 0 Then
                Button2.Enabled = True
                Select Case MainForm.MountedImageImgStatuses(ListView1.FocusedItem.Index)
                    Case 1
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Button2.Text = "Reload servicing"
                                    Case "ESN"
                                        Button2.Text = "Recargar servicio"
                                End Select
                            Case 1
                                Button2.Text = "Reload servicing"
                            Case 2
                                Button2.Text = "Recargar servicio"
                        End Select
                    Case 2
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Button2.Text = "Repair component store"
                                    Case "ESN"
                                        Button2.Text = "Reparar almacén de componentes"
                                End Select
                            Case 1
                                Button2.Text = "Repair component store"
                            Case 2
                                Button2.Text = "Reparar almacén de componentes"
                        End Select
                End Select
            Else
                Button2.Enabled = False
            End If
            IIf(MainForm.MountedImageMountedReWr(ListView1.FocusedItem.Index) = 1, Button3.Enabled = True, Button3.Enabled = False)
            Button4.Enabled = True
            Button5.Enabled = True
            If MainForm.isProjectLoaded And MainForm.MountDir = "N/A" Or Not Directory.Exists(MainForm.MountDir & "\Windows") Then
                Button6.Enabled = True
            Else
                Button6.Enabled = False
            End If
        Else
            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Directory.Exists(ListView1.FocusedItem.SubItems(2).Text) Then
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", ListView1.FocusedItem.SubItems(2).Text)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
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
            Try
                For x = 0 To Array.LastIndexOf(MainForm.MountedImageMountDirs, MainForm.MountedImageMountDirs.Last)
                    If MainForm.MountedImageMountDirs(x) = ListView1.FocusedItem.SubItems(2).Text Then
                        MainForm.MountDir = MainForm.MountedImageMountDirs(x)
                        MainForm.ImgIndex = MainForm.MountedImageImgIndexes(x)
                        MainForm.SourceImg = MainForm.MountedImageImgFiles(x)
                        IIf(MainForm.MountedImageMountedReWr(x) = "Yes", MainForm.isReadOnly = True, MainForm.isReadOnly = False)
                    End If
                Next
            Catch ex As Exception
                Exit Try
            End Try
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
        ProgressPanel.MountDir = ListView1.FocusedItem.SubItems(2).Text
        ProgressPanel.OperationNum = 18
        ProgressPanel.ShowDialog()
        Button2.Enabled = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MainForm.ImgUMountPopupCMS.Show(sender, New Point(24, Button1.Height * 0.75))
    End Sub

    Private Sub DetectorBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles DetectorBW.DoWork
        Do
            If DetectorBW.CancellationPending Then Exit Do
            DetectorBW.ReportProgress(0)
            Application.DoEvents()
            Threading.Thread.Sleep(500)
        Loop
    End Sub

    Private Sub MountedImgMgr_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        DetectorBW.CancelAsync()
    End Sub

    Private Sub DetectorBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles DetectorBW.ProgressChanged
        If DetectorBW.CancellationPending Then Exit Sub
        Try
            For x = 0 To Array.LastIndexOf(MainForm.MountedImageImgFiles, MainForm.MountedImageImgFiles.Last)
                If ignoreRepeats Then
                    If ListView1.Items.Count <> MainForm.MountedImageImgFiles.Length Then
                        ListView1.Items.Clear()
                        PopupImageManager.ListView1.Items.Clear()
                        ignoreRepeats = False
                        Exit Sub
                    End If
                    ' Thanks ChatGPT for providing a little help on this
                    For Each item As ListViewItem In ListView1.Items
                        If Not MainForm.MountedImageImgFiles.Contains(item.Text) Then
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENG"
                                            ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "OK", If(MainForm.MountedImageImgStatuses(x) = 1, "Needs Remount", "Invalid")), If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No"), MainForm.MountedImageImgVersions(x)}))
                                            PopupImageManager.ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "OK", If(MainForm.MountedImageImgStatuses(x) = 1, "Needs Remount", "Invalid")), If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No"), MainForm.MountedImageImgVersions(x)}))
                                        Case "ESN"
                                            ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "Correcto", If(MainForm.MountedImageImgStatuses(x) = 1, "Necesita recarga", "Inválido")), If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No"), MainForm.MountedImageImgVersions(x)}))
                                            PopupImageManager.ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "Correcto", If(MainForm.MountedImageImgStatuses(x) = 1, "Necesita recarga", "Inválido")), If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No"), MainForm.MountedImageImgVersions(x)}))
                                    End Select
                                Case 1
                                    ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "OK", If(MainForm.MountedImageImgStatuses(x) = 1, "Needs Remount", "Invalid")), If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No"), MainForm.MountedImageImgVersions(x)}))
                                    PopupImageManager.ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "OK", If(MainForm.MountedImageImgStatuses(x) = 1, "Needs Remount", "Invalid")), If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No"), MainForm.MountedImageImgVersions(x)}))
                                Case 2
                                    ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "Correcto", If(MainForm.MountedImageImgStatuses(x) = 1, "Necesita recarga", "Inválido")), If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No"), MainForm.MountedImageImgVersions(x)}))
                                    PopupImageManager.ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "Correcto", If(MainForm.MountedImageImgStatuses(x) = 1, "Necesita recarga", "Inválido")), If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No"), MainForm.MountedImageImgVersions(x)}))
                            End Select
                        End If
                    Next
                End If
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                If Not ignoreRepeats Then ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "OK", If(MainForm.MountedImageImgStatuses(x) = 1, "Needs Remount", "Invalid")), If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No"), MainForm.MountedImageImgVersions(x)}))
                                If Not ignoreRepeats Then PopupImageManager.ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "OK", If(MainForm.MountedImageImgStatuses(x) = 1, "Needs Remount", "Invalid")), If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No"), MainForm.MountedImageImgVersions(x)}))
                            Case "ESN"
                                If Not ignoreRepeats Then ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "Correcto", If(MainForm.MountedImageImgStatuses(x) = 1, "Necesita recarga", "Inválido")), If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No"), MainForm.MountedImageImgVersions(x)}))
                                If Not ignoreRepeats Then PopupImageManager.ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "Correcto", If(MainForm.MountedImageImgStatuses(x) = 1, "Necesita recarga", "Inválido")), If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No"), MainForm.MountedImageImgVersions(x)}))
                        End Select
                    Case 1
                        If Not ignoreRepeats Then ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "OK", If(MainForm.MountedImageImgStatuses(x) = 1, "Needs Remount", "Invalid")), If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No"), MainForm.MountedImageImgVersions(x)}))
                        If Not ignoreRepeats Then PopupImageManager.ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "OK", If(MainForm.MountedImageImgStatuses(x) = 1, "Needs Remount", "Invalid")), If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No"), MainForm.MountedImageImgVersions(x)}))
                    Case 2
                        If Not ignoreRepeats Then ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "Correcto", If(MainForm.MountedImageImgStatuses(x) = 1, "Necesita recarga", "Inválido")), If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No"), MainForm.MountedImageImgVersions(x)}))
                        If Not ignoreRepeats Then PopupImageManager.ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "Correcto", If(MainForm.MountedImageImgStatuses(x) = 1, "Necesita recarga", "Inválido")), If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No"), MainForm.MountedImageImgVersions(x)}))
                End Select
            Next
            ignoreRepeats = True
        Catch ex As Exception
            Exit Try
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If MainForm.MountedImageDetectorBW.IsBusy Then MainForm.MountedImageDetectorBW.CancelAsync()
        While MainForm.MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Threading.Thread.Sleep(100)
        End While
        ImgIndexDelete.TextBox1.Text = ListView1.FocusedItem.SubItems(0).Text
        ImgIndexDelete.ShowDialog()
    End Sub
End Class