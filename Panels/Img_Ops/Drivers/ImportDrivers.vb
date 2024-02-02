Imports System.Windows.Forms
Imports System.IO
Imports DISMTools.Utilities

Public Class ImportDrivers

    Dim DIList As New List(Of DriveInfo)
    Dim ImportSourceInt As Integer = -1
    Dim ImportSources() As String = New String(2) {"Windows image", "Online installation", "Offline installation"}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If ImportSourceInt < 0 Then Exit Sub
        Dim msg As String = ""
        If ComboBox1.SelectedItem = "" Then
            msg = "Choose an action and try again"
            MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
            Exit Sub
        Else
            Select Case ImportSourceInt
                Case 0
                    If TextBox1.Text <> "" Then
                        If TextBox1.Text = MainForm.MountDir Then
                            msg = "The import target can't be specified as the import source. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                    Else
                        msg = "No import source has been specified. Specify a source and try again"
                        MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                        Exit Sub
                    End If
                Case 2
                    If TextBox2.Text <> "" Then
                        If TextBox2.Text = DIList(ListView1.FocusedItem.Index).Name Then
                            msg = "The import target can't be specified as the import source. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                        If DIList(ListView1.FocusedItem.Index).DriveFormat <> "NTFS" Then
                            msg = "The import source needs to be a drive formatted with NTFS. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                        If Casters.CastDriveType(DIList(ListView1.FocusedItem.Index).DriveType) <> "Fixed" Then
                            msg = "The import source needs to be a fixed drive. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                        If Not File.Exists(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe") Then
                            msg = "The import source doesn't contain a Windows installation. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        Else
                            ' Don't support Windows Vista (incl. betas) or anything older than Vista
                            Dim sysVer As FileVersionInfo = FileVersionInfo.GetVersionInfo(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe")
                            If sysVer.ProductMajorPart < 6 Or _
                               (sysVer.ProductMajorPart = 6 And sysVer.ProductMinorPart = 0) Then
                                msg = "The import source has an installation of Windows Vista or an earlier version of Windows. Choose a different source and try again"
                                MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                                Exit Sub
                            End If
                        End If
                    Else
                        msg = "No import source has been specified. Specify a source and try again"
                        MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                        Exit Sub
                    End If
            End Select
        End If
        ProgressPanel.ImportSourceInt = ImportSourceInt
        ProgressPanel.DrvImport_SourceImage = TextBox1.Text
        ProgressPanel.DrvImport_SourceDisk = TextBox2.Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 78
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImportDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.SelectedText = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Import drivers"
                        Label1.Text = Text
                        Label2.Text = "This process will import all third-party drivers of the source you specify to this image or installation. This ensures that the target image will have the same hardware compatibility of the source image"
                        Label3.Text = "Import source:"
                        Label4.Text = If(ImportSourceInt = 1, "This source doesn't have any additional settings available.", "Choose a source listed above to configure its settings.")
                        Label5.Text = "Windows image to import drivers from:"
                        Label6.Text = "You can't use the import target as the import source"
                        Label7.Text = "Offline installation to import drivers from:"
                        Label8.Text = "You can't use the import target as the import source"
                        Button1.Text = "Pick..."
                        Button2.Text = "Refresh"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        ListView1.Columns(0).Text = "Drive letter"
                        ListView1.Columns(1).Text = "Drive label"
                        ListView1.Columns(2).Text = "Drive type"
                        ListView1.Columns(3).Text = "Total size"
                        ListView1.Columns(4).Text = "Available free space"
                        ListView1.Columns(5).Text = "Drive format"
                        ListView1.Columns(6).Text = "Contains Windows?"
                        ListView1.Columns(7).Text = "Windows version"
                        ImportSources(0) = "Windows image"
                        ImportSources(1) = "Online installation"
                        ImportSources(2) = "Offline installation"
                    Case "ESN"
                        Text = "Importar controladores"
                        Label1.Text = Text
                        Label2.Text = "Este proceso importará todos los controladores de terceros del origen que especifique a esta imagen o instalación. Esto asegura que la imagen de destino tenga la misma compatibilidad de hardware de la imagen de origen"
                        Label3.Text = "Origen de importación:"
                        Label4.Text = If(ImportSourceInt = 1, "Este origen no tiene opciones adicionales disponibles.", "Escoja un origen mostrado arriba para configurar sus opciones.")
                        Label5.Text = "Imagen de Windows de la que importar controladores:"
                        Label6.Text = "No puede utilizar el destino de importación como el origen de importación"
                        Label7.Text = "Instalación fuera de línea de la que importar controladores:"
                        Label8.Text = "No puede utilizar el destino de importación como el origen de importación"
                        Button1.Text = "Escoger..."
                        Button2.Text = "Actualizar"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Letra de disco"
                        ListView1.Columns(1).Text = "Etiqueta de disco"
                        ListView1.Columns(2).Text = "Tipo de disco"
                        ListView1.Columns(3).Text = "Tamaño total"
                        ListView1.Columns(4).Text = "Espacio libre"
                        ListView1.Columns(5).Text = "Formato del disco"
                        ListView1.Columns(6).Text = "¿Contiene Windows?"
                        ListView1.Columns(7).Text = "Versión de Windows"
                        ImportSources(0) = "Imagen de Windows"
                        ImportSources(1) = "Instalación en línea"
                        ImportSources(2) = "Instalación fuera de línea"
                    Case "FRA"
                        Text = "Importer des pilotes"
                        Label1.Text = Text
                        Label2.Text = "Ce processus importera tous les pilotes tiers de la source que vous spécifiez dans cette image ou installation. Cela garantit que l'image cible aura la même compatibilité matérielle que l'image source."
                        Label3.Text = "Source d'importation :"
                        Label4.Text = If(ImportSourceInt = 1, "Cette source ne dispose pas de paramètres supplémentaires.", "Choisissez une source dans la liste ci-dessus pour configurer ses paramètres.")
                        Label5.Text = "Image Windows à partir de laquelle les pilotes sont importés :"
                        Label6.Text = "Vous ne pouvez pas utiliser la cible d'importation comme source d'importation."
                        Label7.Text = "Installation hors ligne à partir de laquelle les pilotes sont importés :"
                        Label8.Text = "Vous ne pouvez pas utiliser la cible d'importation comme source d'importation."
                        Button1.Text = "Choisir..."
                        Button2.Text = "Actualiser"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        ListView1.Columns(0).Text = "Lettre de disque"
                        ListView1.Columns(1).Text = "Étiquette de disque"
                        ListView1.Columns(2).Text = "Type de disque"
                        ListView1.Columns(3).Text = "Taille totale"
                        ListView1.Columns(4).Text = "Espace libre disponible"
                        ListView1.Columns(5).Text = "Format de disque"
                        ListView1.Columns(6).Text = "Contient Windows ?"
                        ListView1.Columns(7).Text = "Version Windows"
                        ImportSources(0) = "Image de Windows"
                        ImportSources(1) = "Installation en ligne"
                        ImportSources(2) = "Installation hors ligne"
                End Select
            Case 1
                Text = "Import drivers"
                Label1.Text = Text
                Label2.Text = "This process will import all third-party drivers of the source you specify to this image or installation. This ensures that the target image will have the same hardware compatibility of the source image"
                Label3.Text = "Import source:"
                Label4.Text = If(ImportSourceInt = 1, "This source doesn't have any additional settings available.", "Choose a source listed above to configure its settings.")
                Label5.Text = "Windows image to import drivers from:"
                Label6.Text = "You can't use the import target as the import source"
                Label7.Text = "Offline installation to import drivers from:"
                Label8.Text = "You can't use the import target as the import source"
                Button1.Text = "Pick..."
                Button2.Text = "Refresh"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                ListView1.Columns(0).Text = "Drive letter"
                ListView1.Columns(1).Text = "Drive label"
                ListView1.Columns(2).Text = "Drive type"
                ListView1.Columns(3).Text = "Total size"
                ListView1.Columns(4).Text = "Available free space"
                ListView1.Columns(5).Text = "Drive format"
                ListView1.Columns(6).Text = "Contains Windows?"
                ListView1.Columns(7).Text = "Windows version"
                ImportSources(0) = "Windows image"
                ImportSources(1) = "Online installation"
                ImportSources(2) = "Offline installation"
            Case 2
                Text = "Importar controladores"
                Label1.Text = Text
                Label2.Text = "Este proceso importará todos los controladores de terceros del origen que especifique a esta imagen o instalación. Esto asegura que la imagen de destino tenga la misma compatibilidad de hardware de la imagen de origen"
                Label3.Text = "Origen de importación:"
                Label4.Text = If(ImportSourceInt = 1, "Este origen no tiene opciones adicionales disponibles.", "Escoja un origen mostrado arriba para configurar sus opciones.")
                Label5.Text = "Imagen de Windows de la que importar controladores:"
                Label6.Text = "No puede utilizar el destino de importación como el origen de importación"
                Label7.Text = "Instalación fuera de línea de la que importar controladores:"
                Label8.Text = "No puede utilizar el destino de importación como el origen de importación"
                Button1.Text = "Escoger..."
                Button2.Text = "Actualizar"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Letra de disco"
                ListView1.Columns(1).Text = "Etiqueta de disco"
                ListView1.Columns(2).Text = "Tipo de disco"
                ListView1.Columns(3).Text = "Tamaño total"
                ListView1.Columns(4).Text = "Espacio libre"
                ListView1.Columns(5).Text = "Formato del disco"
                ListView1.Columns(6).Text = "¿Contiene Windows?"
                ListView1.Columns(7).Text = "Versión de Windows"
                ImportSources(0) = "Imagen de Windows"
                ImportSources(1) = "Instalación en línea"
                ImportSources(2) = "Instalación fuera de línea"
            Case 3
                Text = "Importer des pilotes"
                Label1.Text = Text
                Label2.Text = "Ce processus importera tous les pilotes tiers de la source que vous spécifiez dans cette image ou installation. Cela garantit que l'image cible aura la même compatibilité matérielle que l'image source."
                Label3.Text = "Source d'importation :"
                Label4.Text = If(ImportSourceInt = 1, "Cette source ne dispose pas de paramètres supplémentaires.", "Choisissez une source dans la liste ci-dessus pour configurer ses paramètres.")
                Label5.Text = "Image Windows à partir de laquelle les pilotes sont importés :"
                Label6.Text = "Vous ne pouvez pas utiliser la cible d'importation comme source d'importation."
                Label7.Text = "Installation hors ligne à partir de laquelle les pilotes sont importés :"
                Label8.Text = "Vous ne pouvez pas utiliser la cible d'importation comme source d'importation."
                Button1.Text = "Choisir..."
                Button2.Text = "Actualiser"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                ListView1.Columns(0).Text = "Lettre de disque"
                ListView1.Columns(1).Text = "Étiquette de disque"
                ListView1.Columns(2).Text = "Type de disque"
                ListView1.Columns(3).Text = "Taille totale"
                ListView1.Columns(4).Text = "Espace libre disponible"
                ListView1.Columns(5).Text = "Format de disque"
                ListView1.Columns(6).Text = "Contient Windows ?"
                ListView1.Columns(7).Text = "Version Windows"
                ImportSources(0) = "Image de Windows"
                ImportSources(1) = "Installation en ligne"
                ImportSources(2) = "Installation hors ligne"
        End Select
        ComboBox1.Items.AddRange(ImportSources)
        If ImportSourceInt >= 0 Then ComboBox1.SelectedIndex = ImportSourceInt
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.ForeColor = Color.Black
        End If
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.BackColor = BackColor
        TextBox2.ForeColor = ForeColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().ToList()
        For Each DI As DriveInfo In DIList
            If DI.IsReady Then
                ListView1.Items.Add(New ListViewItem(New String() {DI.Name, DI.VolumeLabel, Casters.CastDriveType(DI.DriveType, True), Converters.BytesToReadableSize(DI.TotalSize), Converters.BytesToReadableSize(DI.AvailableFreeSpace), DI.DriveFormat, If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"), If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")}))
            End If
        Next
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "" Then
            DefaultPanel.Visible = True
            WinImagePanel.Visible = False
            OfflineInstPanel.Visible = False
            ImportSourceInt = -1
        Else
            Select Case ComboBox1.SelectedIndex
                Case 0
                    DefaultPanel.Visible = False
                    WinImagePanel.Visible = True
                    OfflineInstPanel.Visible = False
                Case 1
                    DefaultPanel.Visible = True
                    WinImagePanel.Visible = False
                    OfflineInstPanel.Visible = False
                Case 2
                    DefaultPanel.Visible = False
                    WinImagePanel.Visible = False
                    OfflineInstPanel.Visible = True
            End Select
            ImportSourceInt = ComboBox1.SelectedIndex
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label4.Text = If(ImportSourceInt = 1, "This source doesn't have any additional settings available.", "Choose a source listed above to configure its settings.")
                    Case "ESN"
                        Label4.Text = If(ImportSourceInt = 1, "Este origen no tiene opciones adicionales disponibles.", "Escoja un origen mostrado arriba para configurar sus opciones.")
                    Case "FRA"
                        Label4.Text = If(ImportSourceInt = 1, "Cette source ne dispose pas de paramètres supplémentaires.", "Choisissez une source dans la liste ci-dessus pour configurer ses paramètres.")
                End Select
            Case 1
                Label4.Text = If(ImportSourceInt = 1, "This source doesn't have any additional settings available.", "Choose a source listed above to configure its settings.")
            Case 2
                Label4.Text = If(ImportSourceInt = 1, "Este origen no tiene opciones adicionales disponibles.", "Escoja un origen mostrado arriba para configurar sus opciones.")
            Case 3
                Label4.Text = If(ImportSourceInt = 1, "Cette source ne dispose pas de paramètres supplémentaires.", "Choisissez une source dans la liste ci-dessus pour configurer ses paramètres.")
        End Select
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PopupImageManager.Location = Button1.PointToScreen(Point.Empty)
        If PopupImageManager.ShowDialog() = DialogResult.OK Then
            TextBox1.Text = PopupImageManager.selectedMntDir
            Label6.Visible = (TextBox1.Text = MainForm.MountDir)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().ToList()
        For Each DI As DriveInfo In DIList
            If DI.IsReady Then
                ListView1.Items.Add(New ListViewItem(New String() {DI.Name, DI.VolumeLabel, Casters.CastDriveType(DI.DriveType, True), Converters.BytesToReadableSize(DI.TotalSize), Converters.BytesToReadableSize(DI.AvailableFreeSpace), DI.DriveFormat, If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"), If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")}))
            End If
        Next
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            For x = 0 To DIList.Count - 1
                If DIList(x).Name = ListView1.FocusedItem.SubItems(0).Text Then
                    TextBox2.Text = DIList(x).Name
                    Label8.Visible = (DIList(x).Name = MainForm.MountDir)
                End If
            Next
        End If
    End Sub
End Class
