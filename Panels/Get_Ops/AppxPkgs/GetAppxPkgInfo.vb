Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports Microsoft.VisualBasic.ControlChars

Public Class GetAppxPkgInfoDlg

    Public InstalledAppxPkgInfo As DismAppxPackageCollection
    Dim mainAsset As String = ""
    Dim assetDir As String = ""

    Public displayName As String = ""

    Private Sub GetAppxPkgInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Get AppX package information"
                        Label1.Text = Text
                        Label36.Text = "AppX package information"
                        Label37.Text = "Select an installed AppX package on the left to view its information here"
                        Label22.Text = "Package name:"
                        Label24.Text = "Application display name:"
                        Label26.Text = "Architecture:"
                        Label31.Text = "Resource ID:"
                        Label41.Text = "Version:"
                        Label43.Text = "Is registered to any user?"
                        Label4.Text = "Installation directory:"
                        Label6.Text = "Package manifest location:"
                        Label8.Text = "Store logo asset directory:"
                        Label9.Text = "Main store logo asset:"
                        Label10.Text = "This asset has been guessed by DISMTools based on its size, which can lead to an incorrect result. If that happens, please report an issue on the GitHub repository"
                        LinkLabel1.Text = "This asset is not the one I'm looking for"
                        Button2.Text = "Save..."
                        SearchBox1.cueBanner = "Type here to search for an application..."
                    Case "ESN"
                        Text = "Obtener información de paquetes AppX"
                        Label1.Text = Text
                        Label36.Text = "Información de paquete AppX"
                        Label37.Text = "Seleccione un paquete AppX instalado en la izquierda para ver su información aquí"
                        Label22.Text = "Nombre de paquete:"
                        Label24.Text = "Nombre de aplicación a mostrar:"
                        Label26.Text = "Arquitectura:"
                        Label31.Text = "ID de recurso:"
                        Label41.Text = "Versión:"
                        Label43.Text = "¿Está registrado a algún usuario?"
                        Label4.Text = "Directorio de instalación:"
                        Label6.Text = "Ubicación del manifiesto del paquete:"
                        Label8.Text = "Directorio de recursos de logotipos de Tienda:"
                        Label9.Text = "Recurso de logotipos de Tienda principal:"
                        Label10.Text = "Este recurso ha sido averiguado por DISMTools por su tamaño, lo que puede llevar a un resultado incorrecto. Si eso ocurre, informe de un problema en el repositorio de GitHub"
                        LinkLabel1.Text = "Este recurso no es el que estaba buscando"
                        Button2.Text = "Guardar..."
                        SearchBox1.cueBanner = "Escriba aquí para buscar una aplicación..."
                    Case "FRA"
                        Text = "Obtenir des informations sur les paquets AppX"
                        Label1.Text = Text
                        Label36.Text = "Informations sur le paquet AppX"
                        Label37.Text = "Sélectionnez un paquet AppX installé sur la gauche pour afficher son information ici."
                        Label22.Text = "Nom du paquet :"
                        Label24.Text = "Nom d'affichage de l'application :"
                        Label26.Text = "Architecture :"
                        Label31.Text = "ID de la ressource :"
                        Label41.Text = "Version :"
                        Label43.Text = "Est-il enregistré au nom d'un utilisateur ?"
                        Label4.Text = "Répertoire d'installation :"
                        Label6.Text = "Emplacement du manifeste du paquet :"
                        Label8.Text = "Répertoire du logo du magasin :"
                        Label9.Text = "Logo du magasin principal :"
                        Label10.Text = "Ce bien a été deviné par DISMTools sur la base de sa taille, ce qui peut conduire à un résultat incorrect. Si cela se produit, veuillez signaler un problème sur le dépôt GitHub."
                        LinkLabel1.Text = "Cette ressource n'est pas celle que je recherche"
                        Button2.Text = "Sauvegarder..."
                        SearchBox1.cueBanner = "Tapez ici pour rechercher une application..."
                    Case "PTB", "PTG"
                        Text = "Obter informações do pacote AppX"
                        Label1.Text = Text
                        Label36.Text = "Informações do pacote AppX"
                        Label37.Text = "Seleccione um pacote AppX instalado à esquerda para ver as suas informações aqui"
                        Label22.Text = "Nome do pacote:"
                        Label24.Text = "Nome de apresentação da aplicação:"
                        Label26.Text = "Arquitetura:"
                        Label31.Text = "ID do recurso:"
                        Label41.Text = "Versão:"
                        Label43.Text = "Está registada para algum utilizador?"
                        Label4.Text = "Diretório de instalação:"
                        Label6.Text = "Localização do manifesto do pacote:"
                        Label8.Text = "Diretório de activos do logótipo da loja:"
                        Label9.Text = "Ativo do logótipo principal da loja:"
                        Label10.Text = "Este ativo foi adivinhado pelo DISMTools com base no seu tamanho, o que pode conduzir a um resultado incorreto. Se isso acontecer, comunique um problema no repositório do GitHub"
                        LinkLabel1.Text = "Este recurso não é o que estou à procura"
                        Button2.Text = "Guardar..."
                        SearchBox1.cueBanner = "Digite aqui para pesquisar uma aplicação..."
                    Case "ITA"
                        Text = "Verifica informazioni pacchetto AppX"
                        Label1.Text = Text
                        Label36.Text = "Informazioni pacchetti AppX"
                        Label37.Text = "Seleziona un pacchetto AppX installato a sinistra per visualizzarne qui le informazioni"
                        Label22.Text = "Nome pacchetto:"
                        Label24.Text = "Nome applicazione visualizzato:"
                        Label26.Text = "Architettura:"
                        Label31.Text = "ID risorsa:"
                        Label41.Text = "Versione:"
                        Label43.Text = "È registrato a qualche utente?"
                        Label4.Text = "Cartella installazione:"
                        Label6.Text = "Percorso manifesto pacchetto:"
                        Label8.Text = "Cartella risorse logo negozio:"
                        Label9.Text = "Asset principale logo negozio:"
                        Label10.Text = "Questa risorsa è stata rilevata da DISMTools in base alle sue dimensioni, il che può portare ad un risultato errato. Se ciò accade, segnala il problema nel repository GitHub"
                        LinkLabel1.Text = "Questa risorsa non è quella cercata"
                        Button2.Text = "Salva..."
                        SearchBox1.cueBanner = "Digita qui per cercare un'applicazione..."
                End Select
            Case 1
                Text = "Get AppX package information"
                Label1.Text = Text
                Label36.Text = "AppX package information"
                Label37.Text = "Select an installed AppX package on the left to view its information here"
                Label22.Text = "Package name:"
                Label24.Text = "Application display name:"
                Label26.Text = "Architecture:"
                Label31.Text = "Resource ID:"
                Label41.Text = "Version:"
                Label43.Text = "Is registered to any user?"
                Label4.Text = "Installation directory:"
                Label6.Text = "Package manifest location:"
                Label8.Text = "Store logo asset directory:"
                Label9.Text = "Main store logo asset:"
                Label10.Text = "This asset has been guessed by DISMTools based on its size, which can lead to an incorrect result. If that happens, please report an issue on the GitHub repository"
                LinkLabel1.Text = "This asset is not the one I'm looking for"
                Button2.Text = "Save..."
                SearchBox1.cueBanner = "Type here to search for an application..."
            Case 2
                Text = "Obtener información de paquetes AppX"
                Label1.Text = Text
                Label36.Text = "Información de paquete AppX"
                Label37.Text = "Seleccione un paquete AppX instalado en la izquierda para ver su información aquí"
                Label22.Text = "Nombre de paquete:"
                Label24.Text = "Nombre de aplicación a mostrar:"
                Label26.Text = "Arquitectura:"
                Label31.Text = "ID de recurso:"
                Label41.Text = "Versión:"
                Label43.Text = "¿Está registrado a algún usuario?"
                Label4.Text = "Directorio de instalación:"
                Label6.Text = "Ubicación del manifiesto del paquete:"
                Label8.Text = "Directorio de recursos de logotipos de Tienda:"
                Label9.Text = "Recurso de logotipos de Tienda principal:"
                Label10.Text = "Este recurso ha sido averiguado por DISMTools por su tamaño, lo que puede llevar a un resultado incorrecto. Si eso ocurre, informe de un problema en el repositorio de GitHub"
                LinkLabel1.Text = "Este recurso no es el que estaba buscando"
                Button2.Text = "Guardar..."
                SearchBox1.cueBanner = "Escriba aquí para buscar una aplicación..."
            Case 3
                Text = "Obtenir des informations sur les paquets AppX"
                Label1.Text = Text
                Label36.Text = "Informations sur le paquet AppX"
                Label37.Text = "Sélectionnez un paquet AppX installé sur la gauche pour afficher son information ici."
                Label22.Text = "Nom du paquet :"
                Label24.Text = "Nom d'affichage de l'application :"
                Label26.Text = "Architecture :"
                Label31.Text = "ID de la ressource :"
                Label41.Text = "Version :"
                Label43.Text = "Est-il enregistré au nom d'un utilisateur ?"
                Label4.Text = "Répertoire d'installation :"
                Label6.Text = "Emplacement du manifeste du paquet :"
                Label8.Text = "Répertoire du logo du magasin :"
                Label9.Text = "Logo du magasin principal :"
                Label10.Text = "Ce bien a été deviné par DISMTools sur la base de sa taille, ce qui peut conduire à un résultat incorrect. Si cela se produit, veuillez signaler un problème sur le dépôt GitHub."
                LinkLabel1.Text = "Cette ressource n'est pas celle que je recherche"
                Button2.Text = "Sauvegarder..."
                SearchBox1.cueBanner = "Tapez ici pour rechercher une application..."
            Case 4
                Text = "Obter informações do pacote AppX"
                Label1.Text = Text
                Label36.Text = "Informações do pacote AppX"
                Label37.Text = "Seleccione um pacote AppX instalado à esquerda para ver as suas informações aqui"
                Label22.Text = "Nome do pacote:"
                Label24.Text = "Nome de apresentação da aplicação:"
                Label26.Text = "Arquitetura:"
                Label31.Text = "ID do recurso:"
                Label41.Text = "Versão:"
                Label43.Text = "Está registada para algum utilizador?"
                Label4.Text = "Diretório de instalação:"
                Label6.Text = "Localização do manifesto do pacote:"
                Label8.Text = "Diretório de activos do logótipo da loja:"
                Label9.Text = "Ativo do logótipo principal da loja:"
                Label10.Text = "Este ativo foi adivinhado pelo DISMTools com base no seu tamanho, o que pode conduzir a um resultado incorreto. Se isso acontecer, comunique um problema no repositório do GitHub"
                LinkLabel1.Text = "Este recurso não é o que estou à procura"
                Button2.Text = "Guardar..."
                SearchBox1.cueBanner = "Digite aqui para pesquisar uma aplicação..."
            Case 5
                Text = "Verifica informazioni sul pacchetto AppX"
                Label1.Text = Text
                Label36.Text = "Informazioni pacchetti AppX"
                Label37.Text = "Seleziona un pacchetto AppX installato a sinistra per visualizzarne qui le informazioni"
                Label22.Text = "Nome pacchetto:"
                Label24.Text = "Nome applicazione visualizzato:"
                Label26.Text = "Architettura:"
                Label31.Text = "ID risorsa:"
                Label41.Text = "Versione:"
                Label43.Text = "È registrato a qualche utente?"
                Label4.Text = "Cartella installazione:"
                Label6.Text = "Percorso manifesto pacchetto:"
                Label8.Text = "Cartella risorse logo negozio:"
                Label9.Text = "Asset principale logo negozio:"
                Label10.Text = "Questa risorsa è stata rilevata da DISMTools in base alle sue dimensioni, il che può portare ad un risultato errato. Se ciò accade, segnala il problema nel repository GitHub"
                LinkLabel1.Text = "Questa risorsa non è quella cercata"
                Button2.Text = "Salva..."
                SearchBox1.cueBanner = "Digita qui per cercare un'applicazione..."
        End Select
        Win10Title.BackColor = CurrentTheme.BackgroundColor
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListBox1.ForeColor = ForeColor
        SearchBox1.BackColor = BackColor
        SearchBox1.ForeColor = ForeColor
        SearchPic.Image = GetGlyphResource("search")
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ' Populate feature information list
        Panel4.Visible = False
        Panel7.Visible = True
        ' Populate package listing
        DynaLog.LogMessage("Updating items in list box...")
        ListBox1.Items.Clear()
        ' The PowerShell helper may have added stuff to the MainForm arrays. Check that
        DynaLog.LogMessage("Detecting conditions imposed by host system...")
        If InstalledAppxPkgInfo IsNot Nothing Then
            DynaLog.LogMessage("Host system is running Windows 10 or 11. Using the technology provided by the DISM API...")
            DynaLog.LogMessage("Detecting if the extended AppX package getter script has been run...")
            If MainForm.CurrentImage.ImageAppxPackages_Backup.Count > InstalledAppxPkgInfo.Count Then
                DynaLog.LogMessage("Array has more items than AppX package collection. The script has been run.")
                DynaLog.LogMessage("Getting AppX packages from arrays...")
                ListBox1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages_Backup.Select(Function(appxPackage) appxPackage.PackageFullName).ToArray())
                SearchPanel.Visible = False
            Else
                DynaLog.LogMessage("Array has the same items as the AppX package collection. The script has not been run.")
                DynaLog.LogMessage("Getting AppX packages...")
                For Each InstalledAppxPkg As DismAppxPackage In InstalledAppxPkgInfo
                    ListBox1.Items.Add(InstalledAppxPkg.PackageName)
                Next
                SearchPanel.Visible = True
            End If
        Else
            DynaLog.LogMessage("Host system is running Windows 8. Getting AppX packages from arrays...")
            ' This condition is met on Windows 8 hosts, as they can't get AppX package information with the DISM API.
            ListBox1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages_Backup.Select(Function(appxPackage) appxPackage.PackageFullName).ToArray())
            SearchPanel.Visible = False
        End If
        SearchBox1.Text = ""
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Label10.Visible = True
        LinkLabel1.Visible = True
        mainAsset = ""
        assetDir = ""
        ' Clear the values of Label7, Label5, and Label3; as the program can't update their text properties on some packages
        Label7.Text = ""
        Label5.Text = ""
        Label3.Text = ""

        Dim trueIndex As Integer = 0

        DynaLog.LogMessage("Selected items: " & ListBox1.SelectedItems.Count)

        If ListBox1.SelectedItems.Count = 1 Then
            DynaLog.LogMessage("An item is selected.")
            DynaLog.LogMessage("Detecting conditions imposed by host system...")
            If InstalledAppxPkgInfo IsNot Nothing Then
                DynaLog.LogMessage("Host system is running Windows 10 or 11. Using the technology provided by the DISM API...")
                DynaLog.LogMessage("Detecting if the extended AppX package getter script has been run...")
                If MainForm.CurrentImage.ImageAppxPackages_Backup.Count > InstalledAppxPkgInfo.Count Then
                    DynaLog.LogMessage("Array has more items than AppX package collection. The script has been run.")
                    DynaLog.LogMessage("Getting AppX packages from arrays...")
                    Label23.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageFullName
                    Label25.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageName
                    Label35.Text = Casters.CastDismArchitecture(MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageArchitecture, True)
                    Label32.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageResourceId
                    Label40.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageVersion.ToString()
                Else
                    DynaLog.LogMessage("Array has the same items as the AppX package collection. The script has not been run.")
                    DynaLog.LogMessage("Getting AppX packages...")
                    DynaLog.LogMessage("Search function may have been used. Grabbing true index of selected AppX package...")
                    For Each InstalledAppx As DismAppxPackage In InstalledAppxPkgInfo
                        If InstalledAppx.PackageName.ToLower().Contains(SearchBox1.Text.ToLower()) And InstalledAppx.PackageName = ListBox1.Items(ListBox1.SelectedIndex) Then
                            trueIndex = InstalledAppxPkgInfo.IndexOf(InstalledAppx)
                        End If
                    Next
                    DynaLog.LogMessage("True index: " & trueIndex)
                    If SearchBox1.Text = "" Then
                        DynaLog.LogMessage("No search query has been typed.")
                        Label23.Text = InstalledAppxPkgInfo(ListBox1.SelectedIndex).PackageName
                        Label25.Text = InstalledAppxPkgInfo(ListBox1.SelectedIndex).DisplayName
                        Label35.Text = Casters.CastDismArchitecture(InstalledAppxPkgInfo(ListBox1.SelectedIndex).Architecture, True)
                        Label32.Text = InstalledAppxPkgInfo(ListBox1.SelectedIndex).ResourceId
                        Label40.Text = InstalledAppxPkgInfo(ListBox1.SelectedIndex).Version.ToString()
                    Else
                        DynaLog.LogMessage("A search query has been typed. Using true index...")
                        Label23.Text = InstalledAppxPkgInfo(trueIndex).PackageName
                        Label25.Text = InstalledAppxPkgInfo(trueIndex).DisplayName
                        Label35.Text = Casters.CastDismArchitecture(InstalledAppxPkgInfo(trueIndex).Architecture, True)
                        Label32.Text = InstalledAppxPkgInfo(trueIndex).ResourceId
                        Label40.Text = InstalledAppxPkgInfo(trueIndex).Version.ToString()
                    End If
                End If
            Else
                DynaLog.LogMessage("Host system is running Windows 8. Getting AppX packages from arrays...")
                Label23.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageFullName
                Label25.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageName
                Label35.Text = Casters.CastDismArchitecture(MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageArchitecture, True)
                Label32.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageResourceId
                Label40.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageVersion.ToString()
            End If

            displayName = Label25.Text

            DynaLog.LogMessage("Getting AppX display name (the one you would see if the app is installed)...")
            DynaLog.LogMessage("- Package name: " & Label23.Text)
            DynaLog.LogMessage("- Package display name: " & Label25.Text)
            Dim packageDispName As String = MainForm.GetPackageDisplayName(Label23.Text, Label25.Text)

            Dim appDisplayName As String = ""

            If packageDispName IsNot Nothing Then
                DynaLog.LogMessage("Package display name: " & packageDispName)
                DynaLog.LogMessage("Checking if display name relies on a PRI...")
                appDisplayName = If(Not packageDispName.StartsWith("ms-resource:"), packageDispName, "")
                If InstalledAppxPkgInfo IsNot Nothing And packageDispName.StartsWith("ms-resource:") Then
                    DynaLog.LogMessage("Display name starts with " & Quote & "ms-resource:" & Quote & ". Using PRI reader...")
                    If MainForm.CurrentImage.ImageAppxPackages_Backup.Count > InstalledAppxPkgInfo.Count Then
                        DynaLog.LogMessage("Array has more items than AppX package collection. The script has been run.")
                        Dim PriName As String = PriReader.ReadFromPri((If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & Label23.Text).Replace("\\", "\").Trim(), _
                                                                      Label25.Text, _
                                                                      packageDispName)
                        DynaLog.LogMessage("Name obtained from " & Quote & "resources.pri" & Quote & ": " & PriName)
                        If PriName <> "" And Not PriName = Label25.Text Then appDisplayName = PriName
                    Else
                        DynaLog.LogMessage("Array has the same items as the AppX package collection. The script has not been run.")
                        If SearchBox1.Text = "" Then
                            Dim PriName As String = PriReader.ReadFromPri(InstalledAppxPkgInfo(ListBox1.SelectedIndex).InstallLocation, _
                                                                          InstalledAppxPkgInfo(ListBox1.SelectedIndex).DisplayName, _
                                                                          packageDispName)
                            DynaLog.LogMessage("Name obtained from " & Quote & "resources.pri" & Quote & ": " & PriName)
                            If PriName <> "" And Not PriName = InstalledAppxPkgInfo(ListBox1.SelectedIndex).DisplayName Then appDisplayName = PriName
                        Else
                            Dim PriName As String = PriReader.ReadFromPri(InstalledAppxPkgInfo(trueIndex).InstallLocation, _
                                                                          InstalledAppxPkgInfo(trueIndex).DisplayName, _
                                                                          packageDispName)
                            DynaLog.LogMessage("Name obtained from " & Quote & "resources.pri" & Quote & ": " & PriName)
                            If PriName <> "" And Not PriName = InstalledAppxPkgInfo(trueIndex).DisplayName Then appDisplayName = PriName
                        End If
                    End If
                End If
            End If

            If appDisplayName <> "" Then Label25.Text &= " (" & appDisplayName & ")"

            DynaLog.LogMessage("Getting registration status of AppX package...")
            ' Get exclusive things that can't be obtained with the DISM API
            If Directory.Exists(MainForm.MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & Label23.Text) Then
                If My.Computer.FileSystem.GetFiles(MainForm.MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & Label23.Text, FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count = 0 Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label42.Text = "No"
                                Case "ESN"
                                    Label42.Text = "No"
                                Case "FRA"
                                    Label42.Text = "Non"
                                Case "PTB", "PTG"
                                    Label42.Text = "Não"
                                Case "ITA"
                                    Label42.Text = "No"
                            End Select
                        Case 1
                            Label42.Text = "No"
                        Case 2
                            Label42.Text = "No"
                        Case 3
                            Label42.Text = "Non"
                        Case 4
                            Label42.Text = "Não"
                        Case 5
                            Label42.Text = "No"
                    End Select
                Else
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label42.Text = "Yes"
                                Case "ESN"
                                    Label42.Text = "Sí"
                                Case "FRA"
                                    Label42.Text = "Oui"
                                Case "PTB", "PTG"
                                    Label42.Text = "Sim"
                                Case "ITA"
                                    Label42.Text = "Sì"
                            End Select
                        Case 1
                            Label42.Text = "Yes"
                        Case 2
                            Label42.Text = "Sí"
                        Case 3
                            Label42.Text = "Oui"
                        Case 4
                            Label42.Text = "Sim"
                        Case 5
                            Label42.Text = "Sì"
                    End Select
                    If MainForm.OnlineManagement AndAlso Not MainForm.NoNTSamMappings Then
                        DynaLog.LogMessage("Online installation management mode has been detected and we're expected to map SAM information. Proceeding...")
                        Try
                            Dim profileFiles As New List(Of String)
                            profileFiles = My.Computer.FileSystem.GetFiles(MainForm.MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & Label23.Text, FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").ToList()
                            If profileFiles.Count > 0 Then
                                Label42.Text &= ":" & CrLf & SamHelper.MapPckgdepsToSamProfiles(profileFiles)
                            End If
                        Catch ex As Exception

                        End Try
                    End If
                End If
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label42.Text = "No"
                            Case "ESN"
                                Label42.Text = "No"
                            Case "FRA"
                                Label42.Text = "Non"
                            Case "PTB", "PTG"
                                Label42.Text = "Não"
                            Case "ITA"
                                Label42.Text = "No"
                        End Select
                    Case 1
                        Label42.Text = "No"
                    Case 2
                        Label42.Text = "No"
                    Case 3
                        Label42.Text = "Non"
                    Case 4
                        Label42.Text = "Não"
                    Case 5
                        Label42.Text = "No"
                End Select
            End If
            DynaLog.LogMessage("Getting AppX main Store logo asset...")
            mainAsset = MainForm.GetStoreAppMainLogo(Label23.Text)
            DynaLog.LogMessage("Main asset location: " & Quote & mainAsset & Quote)
            If mainAsset <> "" And File.Exists(mainAsset) Then
                Try
                    DynaLog.LogMessage("Attempting to open picture...")
                    Dim asset As Image = Image.FromFile(mainAsset)
                    If (asset.Width > PictureBox2.Width) Or (asset.Height > PictureBox2.Height) Then
                        PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
                    Else
                        PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
                    End If
                Catch ex As Exception
                    DynaLog.LogMessage("Could not open picture. This can happen if the file is corrupted. Error message: " & ex.Message)
                End Try
            Else
                Label10.Visible = False
                LinkLabel1.Visible = False
                PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
            End If
            Try
                If mainAsset <> "" And File.Exists(mainAsset) Then PictureBox2.Image = Image.FromFile(mainAsset) Else PictureBox2.Image = GetGlyphResource("preview_unavail")
            Catch ex As Exception
                PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
                PictureBox2.Image = GetGlyphResource("preview_unavail")
            End Try
            Try
                assetDir = MainForm.GetSuitablePackageFolder(Label25.Text)
            Catch ex As Exception
                ' Continue
            End Try
            DynaLog.LogMessage("Part of asset directory: " & Quote & assetDir & Quote)
            If assetDir <> "" Then
                DynaLog.LogMessage("Getting full asset directory...")
                If File.Exists(assetDir & "\AppxManifest.xml") Then
                    Dim ManFile As New RichTextBox() With {
                        .Text = File.ReadAllText(assetDir & "\AppxManifest.xml")
                    }
                    For Each line In ManFile.Lines
                        If line.Contains("<Logo>") Then
                            Dim SplitPaths As New List(Of String)
                            SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                            SplitPaths.RemoveAt(SplitPaths.Count - 1)
                            Dim newPath As String = String.Join("\", SplitPaths)
                            Label7.Text = (assetDir & "\" & newPath).Replace("\\", "\").Trim()
                            Exit For
                        End If
                    Next
                End If
            Else
                DynaLog.LogMessage("Getting full asset directory...")
                If File.Exists(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & Label23.Text & "\AppxManifest.xml") Then
                    Dim ManFile As New RichTextBox() With {
                        .Text = File.ReadAllText(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & Label23.Text & "\AppxManifest.xml")
                    }
                    For Each line In ManFile.Lines
                        If line.Contains("<Logo>") Then
                            Dim SplitPaths As New List(Of String)
                            SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                            SplitPaths.RemoveAt(SplitPaths.Count - 1)
                            Dim newPath As String = String.Join("\", SplitPaths)
                            Label7.Text = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & Label23.Text & "\" & newPath).Replace("\\", "\").Trim()
                            Exit For
                        End If
                    Next
                End If
            End If
            DynaLog.LogMessage("Getting location of AppX manifest...")
            Label3.Text = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & Label23.Text).Replace("\\", "\").Trim()
            Dim pkgDirs() As String = Directory.GetDirectories(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps", Label23.Text & "*", SearchOption.TopDirectoryOnly)
            For Each folder In pkgDirs
                If Not folder.Contains("neutral") Then
                    Label5.Text = (folder & "\AppxManifest.xml").Replace("\\", "\").Trim()
                End If
            Next
            Try
                If pkgDirs.Count <= 1 And Not Label5.Text.Contains(Label23.Text) Then
                    If File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml") Then
                        DynaLog.LogMessage("The installed application originated from a bundle package.")
                        Label5.Text = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml"
                    ElseIf File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml") Then
                        DynaLog.LogMessage("The installed application originated from a regular AppX package.")
                        Label5.Text = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml"
                    Else
                        DynaLog.LogMessage("The installed application originated from an unknown source.")
                        Label5.Text = ""
                    End If
                End If
            Catch ex As Exception
                DynaLog.LogMessage("Could not get some information about this application. Error message: " & ex.Message)
                MsgBox("Could not get some information about this application.", vbOKOnly + vbCritical, Label1.Text)
            End Try
            Panel4.Visible = True
            Panel7.Visible = False
        Else
            Panel4.Visible = False
            Panel7.Visible = True
        End If
    End Sub

    Private Sub PictureBox2_MouseClick(sender As Object, e As MouseEventArgs) Handles PictureBox2.MouseClick
        If mainAsset <> "" And File.Exists(mainAsset) Then
            If e.Button = Windows.Forms.MouseButtons.Right Then
                MainForm.AppxResCMS.Show(sender, e.Location)
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MainForm.ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving installed AppX package information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = MainForm.SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.OfflineMode = MainForm.OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = MainForm.SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = MainForm.AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 5
            ImgInfoSaveDlg.ImageToGetInfoFrom = MainForm.CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://github.com/CodingWonders/DISMTools/issues/new?assignees=CodingWonders&labels=bug&projects=&template=store-logo-asset-preview-issue.md&title=")
    End Sub

    Sub SearchPackages(sQuery As String)
        DynaLog.LogMessage("Search query: " & sQuery)
        If InstalledAppxPkgInfo.Count > 0 Then
            Dim FilteredAppxPackages = InstalledAppxPkgInfo.Where(Function(AppxPackage) AppxPackage.PackageName.ToLower().Contains(sQuery.ToLower()))
            For Each FilteredAppx As DismAppxPackage In FilteredAppxPackages
                ListBox1.Items.Add(FilteredAppx.PackageName)
            Next
        End If
    End Sub

    Private Sub SearchBox1_TextChanged(sender As Object, e As EventArgs) Handles SearchBox1.TextChanged
        ListBox1.Items.Clear()
        If SearchBox1.Text <> "" Then
            SearchPackages(SearchBox1.Text)
        Else
            DynaLog.LogMessage("No search query has been specified. Showing all items...")
            For Each InstalledAppx As DismAppxPackage In InstalledAppxPkgInfo
                ListBox1.Items.Add(InstalledAppx.PackageName)
            Next
        End If
    End Sub

    Private Sub SearchBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles SearchBox1.KeyDown
        If e.KeyCode = Keys.Back And e.Control Then
            Dim text As String = SearchBox1.Text
            Dim lastSpaceIndex As Integer = text.LastIndexOf(" "c)
            If lastSpaceIndex > 0 Then
                SearchBox1.Text = text.Substring(0, lastSpaceIndex).TrimEnd()
            Else
                SearchBox1.Text = ""
            End If
            e.SuppressKeyPress = True
            SearchBox1.SelectionStart = SearchBox1.TextLength
        End If
    End Sub
End Class
