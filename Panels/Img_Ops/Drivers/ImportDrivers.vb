Imports System.Windows.Forms
Imports System.IO
Imports DISMTools.Utilities
Imports Microsoft.Dism

Public Class ImportDrivers
    Implements IImageTaskDialog

    Dim DIList As New List(Of DriveInfo)
    Dim ImportSourceInt As Integer = -1
    Dim ImportSources() As String = New String(2) {"Windows image", "Online installation", "Offline installation"}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If ImportSourceInt < 0 Then Exit Sub
        Dim msg As String = ""
        If ComboBox1.SelectedItem = "" Then
            DynaLog.LogMessage("No source has been selected.")
            msg = "Choose an action and try again"
            MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
            Exit Sub
        Else
            DynaLog.LogMessage("A source has been selected. Verifying inputs...")
            If ListView1.SelectedItems.Count = 1 Then
                If DIList(ListView1.FocusedItem.Index).Name = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) Then ImportSourceInt = 1
            End If
            DynaLog.LogMessage("Import source flag: " & ImportSourceInt)
            Select Case ImportSourceInt
                Case 0
                    DynaLog.LogMessage("Validating import source...")
                    If TextBox1.Text <> "" Then
                        DynaLog.LogMessage("An import source has been specified.")
                        DynaLog.LogMessage("Source: " & TextBox1.Text)
                        If TextBox1.Text = MainForm.MountDir Then
                            DynaLog.LogMessage("The import source is the same as the import target.")
                            msg = "The import target can't be specified as the import source. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                    Else
                        DynaLog.LogMessage("No import source has been specified.")
                        msg = "No import source has been specified. Specify a source and try again"
                        MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                        Exit Sub
                    End If
                Case 2
                    DynaLog.LogMessage("Validating import source...")
                    DynaLog.LogMessage("Source: " & TextBox2.Text)
                    If TextBox2.Text <> "" Then
                        DynaLog.LogMessage("An import source has been specified.")
                        DynaLog.LogMessage("Checking drive letter...")
                        If TextBox2.Text = DIList(ListView1.FocusedItem.Index).Name Then
                            DynaLog.LogMessage("The import source is the same as the import target.")
                            msg = "The import target can't be specified as the import source. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                        DynaLog.LogMessage("Checking drive format...")
                        If DIList(ListView1.FocusedItem.Index).DriveFormat <> "NTFS" Then
                            DynaLog.LogMessage("The source is not formatted with NTFS.")
                            msg = "The import source needs to be a drive formatted with NTFS. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                        DynaLog.LogMessage("Checking Windows installation in the drive...")
                        If Not File.Exists(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe") Then
                            DynaLog.LogMessage("The source drive does not contain ntoskrnl. There is either an utterly broken Windows installation or no installation at all.")
                            msg = "The import source doesn't contain a Windows installation. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        Else
                            DynaLog.LogMessage("The source drive contains ntoskrnl. Checking version...")
                            ' Don't support Windows Vista (incl. betas) or anything older than Vista
                            Dim sysVer As FileVersionInfo = FileVersionInfo.GetVersionInfo(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe")
                            If sysVer.ProductMajorPart < 6 Or _
                               (sysVer.ProductMajorPart = 6 And sysVer.ProductMinorPart = 0) Then
                                DynaLog.LogMessage("The import source contains Windows Vista or an earlier version of Windows.")
                                msg = "The import source has an installation of Windows Vista or an earlier version of Windows. Choose a different source and try again"
                                MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                                Exit Sub
                            End If
                        End If
                    Else
                        DynaLog.LogMessage("No import source has been specified.")
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

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        If Not MainForm.OnlineManagement Then
            DynaLog.LogMessage("The active installation is not being managed right now. Continuing...")
        Else
            DynaLog.LogMessage("This image is not supported.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on online installations", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
                        Case "PTB", "PTG"
                            MsgBox("Esta ação não é suportada em instalações em linha", vbOKOnly + vbCritical, Text)
                        Case "ITA"
                            MsgBox("Questa azione non è supportata dalle installazioni attive", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on online installations", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
                Case 4
                    MsgBox("Esta ação não é suportada em instalações em linha", vbOKOnly + vbCritical, Text)
                Case 5
                    MsgBox("Questa azione non è supportata dalle installazioni attive", vbOKOnly + vbCritical, Text)
            End Select
        End If
        Return Not MainForm.OnlineManagement
    End Function

    Private Sub ImportDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
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
                        Label9.Text = "Image file:"
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
                        Label9.Text = "Archivo de imagen:"
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
                        Label9.Text = "Fichier de l'image :"
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
                    Case "PTB", "PTG"
                        Text = "Importar controladores"
                        Label1.Text = Text
                        Label2.Text = "Este processo irá importar todos os controladores de terceiros da fonte que especificar para esta imagem ou instalação. Isto assegura que a imagem de destino terá a mesma compatibilidade de hardware da imagem de origem"
                        Label3.Text = "Importar fonte:"
                        Label4.Text = If(ImportSourceInt = 1, "Esta fonte não tem quaisquer configurações adicionais disponíveis.", "Escolha uma fonte listada acima para configurar as suas definições.")
                        Label5.Text = "Imagem do Windows a partir da qual importar controladores:"
                        Label6.Text = "Não é possível utilizar o destino de importação como fonte de importação"
                        Label7.Text = "Instalação offline para importar controladores de:"
                        Label8.Text = "Não é possível utilizar o destino de importação como fonte de importação"
                        Label9.Text = "Ficheiro de imagem:"
                        Button1.Text = "Escolher..."
                        Button2.Text = "Atualizar"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Letra da unidade"
                        ListView1.Columns(1).Text = "Label da unidade"
                        ListView1.Columns(2).Text = "Tipo de unidade"
                        ListView1.Columns(3).Text = "Tamanho total"
                        ListView1.Columns(4).Text = "Espaço livre disponível"
                        ListView1.Columns(5).Text = "Formato da unidade"
                        ListView1.Columns(6).Text = "Contém Windows?"
                        ListView1.Columns(7).Text = "Versão do Windows"
                        ImportSources(0) = "Imagem do Windows"
                        ImportSources(1) = "Instalação online"
                        ImportSources(2) = "Instalação offline"
                    Case "ITA"
                        Text = "Importare i driver"
                        Label1.Text = Text
                        Label2.Text = "Questo processo importerà tutti i driver di terze parti dell'origine specificata in questa immagine o installazione. Questo assicura che l'immagine di destinazione abbia la stessa compatibilità hardware dell'immagine di origine"
                        Label3.Text = "Importazione dell'origine:"
                        Label4.Text = If(ImportSourceInt = 1, "Questa sorgente non ha impostazioni aggiuntive disponibili.", "Scegliere una sorgente elencata sopra per configurarne le impostazioni.")
                        Label5.Text = "Immagine di Windows da cui importare i driver:"
                        Label6.Text = "Non è possibile utilizzare la destinazione di importazione come origine di importazione"
                        Label7.Text = "Installazione offline da cui importare i driver:"
                        Label8.Text = "Non è possibile utilizzare il target di importazione come sorgente di importazione"
                        Label9.Text = "File immagine:"
                        Button1.Text = "Scegliere..."
                        Button2.Text = "Aggiorna"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        ListView1.Columns(0).Text = "Lettera unità"
                        ListView1.Columns(1).Text = "Etichetta dell'unità"
                        ListView1.Columns(2).Text = "Tipo di unità"
                        ListView1.Columns(3).Text = "Dimensione totale"
                        ListView1.Columns(4).Text = "Spazio libero disponibile"
                        ListView1.Columns(5).Text = "Formato unità"
                        ListView1.Columns(6).Text = "Contiene Windows?"
                        ListView1.Columns(7).Text = "Versione di Windows"
                        ImportSources(0) = "Immagine di Windows"
                        ImportSources(1) = "Installazione attiva"
                        ImportSources(2) = "Installazione offline"
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
                Label9.Text = "Image file:"
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
                Label9.Text = "Archivo de imagen:"
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
                Label9.Text = "Fichier de l'image :"
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
            Case 4
                Text = "Importar controladores"
                Label1.Text = Text
                Label2.Text = "Este processo irá importar todos os controladores de terceiros da fonte que especificar para esta imagem ou instalação. Isto assegura que a imagem de destino terá a mesma compatibilidade de hardware da imagem de origem"
                Label3.Text = "Importar fonte:"
                Label4.Text = If(ImportSourceInt = 1, "Esta fonte não tem quaisquer configurações adicionais disponíveis.", "Escolha uma fonte listada acima para configurar as suas definições.")
                Label5.Text = "Imagem do Windows a partir da qual importar controladores:"
                Label6.Text = "Não é possível utilizar o destino de importação como fonte de importação"
                Label7.Text = "Instalação offline para importar controladores de:"
                Label8.Text = "Não é possível utilizar o destino de importação como fonte de importação"
                Label9.Text = "Ficheiro de imagem:"
                Button1.Text = "Escolher..."
                Button2.Text = "Atualizar"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Letra da unidade"
                ListView1.Columns(1).Text = "Label da unidade"
                ListView1.Columns(2).Text = "Tipo de unidade"
                ListView1.Columns(3).Text = "Tamanho total"
                ListView1.Columns(4).Text = "Espaço livre disponível"
                ListView1.Columns(5).Text = "Formato da unidade"
                ListView1.Columns(6).Text = "Contém Windows?"
                ListView1.Columns(7).Text = "Versão do Windows"
                ImportSources(0) = "Imagem do Windows"
                ImportSources(1) = "Instalação online"
                ImportSources(2) = "Instalação offline"
            Case 5
                Text = "Importare i driver"
                Label1.Text = Text
                Label2.Text = "Questo processo importerà tutti i driver di terze parti dell'origine specificata in questa immagine o installazione. Questo assicura che l'immagine di destinazione abbia la stessa compatibilità hardware dell'immagine di origine"
                Label3.Text = "Importazione dell'origine:"
                Label4.Text = If(ImportSourceInt = 1, "Questa sorgente non ha impostazioni aggiuntive disponibili.", "Scegliere una sorgente elencata sopra per configurarne le impostazioni.")
                Label5.Text = "Immagine di Windows da cui importare i driver:"
                Label6.Text = "Non è possibile utilizzare la destinazione di importazione come origine di importazione"
                Label7.Text = "Installazione offline da cui importare i driver:"
                Label8.Text = "Non è possibile utilizzare il target di importazione come sorgente di importazione"
                Label9.Text = "File immagine:"
                Button1.Text = "Scegliere..."
                Button2.Text = "Aggiorna"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                ListView1.Columns(0).Text = "Lettera unità"
                ListView1.Columns(1).Text = "Etichetta dell'unità"
                ListView1.Columns(2).Text = "Tipo di unità"
                ListView1.Columns(3).Text = "Dimensione totale"
                ListView1.Columns(4).Text = "Spazio libero disponibile"
                ListView1.Columns(5).Text = "Formato unità"
                ListView1.Columns(6).Text = "Contiene Windows?"
                ListView1.Columns(7).Text = "Versione di Windows"
                ImportSources(0) = "Immagine di Windows"
                ImportSources(1) = "Installazione attiva"
                ImportSources(2) = "Installazione offline"
        End Select
        ComboBox1.Items.AddRange(ImportSources)
        If ImportSourceInt >= 0 Then ComboBox1.SelectedIndex = ImportSourceInt
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Win10Title.BackColor = CurrentTheme.BackgroundColor
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.BackColor = BackColor
        TextBox2.ForeColor = ForeColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        DynaLog.LogMessage("Getting disks...")
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().Where(Function(disk) disk.IsReady).ToList()
        For Each DI As DriveInfo In DIList
            ListView1.Items.Add(New ListViewItem(New String() {DI.Name, DI.VolumeLabel, Casters.CastDriveType(DI.DriveType, True), Converters.BytesToReadableSize(DI.TotalSize), Converters.BytesToReadableSize(DI.AvailableFreeSpace), DI.DriveFormat, If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"), If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")}))
        Next
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, CurrentTheme.IsDark)
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
                    Case "PTB", "PTG"
                        Label4.Text = If(ImportSourceInt = 1, "Esta origem não tem quaisquer configurações adicionais disponíveis.", "Escolha uma origem listada acima para configurar as suas configurações.")
                    Case "ITA"
                        Label4.Text = If(ImportSourceInt = 1, "Questa sorgente non ha impostazioni aggiuntive disponibili", "Scegliere una sorgente elencata sopra per configurarne le impostazioni")
                End Select
            Case 1
                Label4.Text = If(ImportSourceInt = 1, "This source doesn't have any additional settings available.", "Choose a source listed above to configure its settings.")
            Case 2
                Label4.Text = If(ImportSourceInt = 1, "Este origen no tiene opciones adicionales disponibles.", "Escoja un origen mostrado arriba para configurar sus opciones.")
            Case 3
                Label4.Text = If(ImportSourceInt = 1, "Cette source ne dispose pas de paramètres supplémentaires.", "Choisissez une source dans la liste ci-dessus pour configurer ses paramètres.")
            Case 4
                Label4.Text = If(ImportSourceInt = 1, "Esta origem não tem quaisquer configurações adicionais disponíveis.", "Escolha uma origem listada acima para configurar as suas configurações.")
            Case 5
                Label4.Text = If(ImportSourceInt = 1, "Questa sorgente non ha impostazioni aggiuntive disponibili", "Scegliere una sorgente elencata sopra per configurarne le impostazioni")
        End Select
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim selectedImage As DismMountedImageInfo = PopupMountedImagePicker.PickImage(Button1.PointToScreen(Point.Empty))
        If selectedImage IsNot Nothing Then
            DynaLog.LogMessage("Information will be obtained from the popup mounted image manager...")
            TextBox1.Text = selectedImage.MountPath
            Label6.Visible = (TextBox1.Text = MainForm.MountDir)
            Label10.Text = selectedImage.ImageFilePath
            Label10.Visible = (TextBox1.Text <> "" And Directory.Exists(TextBox1.Text))
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DynaLog.LogMessage("Refreshing disk listings...")
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().Where(Function(disk) disk.IsReady).ToList()
        For Each DI As DriveInfo In DIList
            ListView1.Items.Add(New ListViewItem(New String() {DI.Name, DI.VolumeLabel, Casters.CastDriveType(DI.DriveType, True), Converters.BytesToReadableSize(DI.TotalSize), Converters.BytesToReadableSize(DI.AvailableFreeSpace), DI.DriveFormat, If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"), If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")}))
        Next
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            DynaLog.LogMessage("Checking selected item...")
            For x = 0 To DIList.Count - 1
                If DIList(x).Name = ListView1.FocusedItem.SubItems(0).Text Then
                    TextBox2.Text = DIList(x).Name
                    Label8.Visible = (DIList(x).Name = MainForm.MountDir)
                    If DIList(x).Name = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) Then ComboBox1.SelectedIndex = 1
                End If
            Next
        End If
    End Sub
End Class
