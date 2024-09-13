Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports DISMTools.Elements
Imports System.Xml
Imports System.Xml.Serialization

Public Class AddProvAppxPackage

    ' Variables used by the AppX scanner component
    Dim AppxNameList As New List(Of String)
    Dim AppxPublisherList As New List(Of String)
    Dim AppxVersionList As New List(Of String)
    Public AppxNames(65535) As String
    Public AppxPublishers(65535) As String
    Public AppxVersion(65535) As String

    ' Variables passed to ProgressPanel
    Public AppxPackages(65535) As String
    Public AppxDependencies(65535) As String

    ' Internal variables helpful to pass information
    Public AppxAdditionCount As Integer
    Public AppxDependencyCount As Integer

    Dim LogoAssetPopupForm As New Form()
    Dim LogoAssetPreview As New PictureBox()
    Dim previewer As New ToolTip()

    Dim Packages As New List(Of AppxPackage)

    Dim StubPreferences() As String = New String(2) {"Do not configure stub preference", "Install application as a stub package", "Install application as a full package"}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        AppxAdditionCount = ListView1.Items.Count
        AppxDependencyCount = ListBox1.Items.Count
        ProgressPanel.appxAdditionCount = AppxAdditionCount
        If ListView1.Items.Count = 0 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("Please specify packed or unpacked AppX packages and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                        Case "ESN"
                            MsgBox("Especifique archivos AppX empaquetados o desempaquetados e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                        Case "FRA"
                            MsgBox("Veuillez spécifier les paquets AppX comprimés ou non et réessayez.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                        Case "PTB", "PTG"
                            MsgBox("Especifique pacotes AppX embalados ou não embalados e tente novamente.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                        Case "ITA"
                            MsgBox("Specificare i pacchetti AppX imballati o non imballati e riprovare.", vbOKOnly + vbCritical, "Aggiungere i pacchetti AppX approvvigionati")
                    End Select
                Case 1
                    MsgBox("Please specify packed or unpacked AppX packages and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                Case 2
                    MsgBox("Especifique archivos AppX empaquetados o desempaquetados e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                Case 3
                    MsgBox("Veuillez spécifier les paquets AppX comprimés ou non et réessayez.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                Case 4
                    MsgBox("Especifique pacotes AppX embalados ou não embalados e tente novamente.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                Case 5
                    MsgBox("Specificare i pacchetti AppX imballati o non imballati e riprovare.", vbOKOnly + vbCritical, "Aggiungere i pacchetti AppX approvvigionati")
            End Select
            Exit Sub
        Else
            If AppxAdditionCount > 65535 Then
                MsgBox("Right now, you can only specify less than 65535 AppX packages. This is a program limitation that will be gone in a future update.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                Exit Sub
            Else
                For x = 0 To AppxAdditionCount - 1
                    AppxPackages(x) = ListView1.Items(x).Text
                Next
                For x = 0 To AppxDependencyCount - 1
                    AppxDependencies(x) = ListBox1.Items(x).ToString()
                Next
                ' Fill in remote arrays, even empty slots
                For x = 0 To AppxPackages.Length - 1
                    ProgressPanel.appxAdditionPackages(x) = AppxPackages(x)
                Next
                For x = 0 To AppxDependencies.Length - 1
                    ProgressPanel.appxAdditionDependencies(x) = AppxDependencies(x)
                Next
                ProgressPanel.appxAdditionLastPackage = ListView1.Items(AppxAdditionCount - 1).ToString().Replace("ListViewItem: {", "").Trim().Replace("}", "").Trim()
                If AppxDependencyCount > 0 Then
                    ProgressPanel.appxAdditionLastDependency = ListBox1.Items(AppxDependencyCount - 1).ToString()
                Else
                    ProgressPanel.appxAdditionLastDependency = ""
                End If
                If CheckBox3.Checked Then
                    If TextBox1.Text = "" Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        MsgBox("Please specify a license file and try again. You can also continue without one, but this may compromise the image.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                                    Case "ESN"
                                        MsgBox("Especifique un archivo de licencia e inténtelo de nuevo. También puede continuar sin uno, pero esta acción podría comprometer la imagen.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                                    Case "FRA"
                                        MsgBox("Veuillez indiquer un fichier de licence et réessayer. Vous pouvez également continuer sans licence, mais cela risque de compromettre l'image.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                                    Case "PTB", "PTG"
                                        MsgBox("Por favor, especifique um ficheiro de licença e tente novamente. Também pode continuar sem um, mas isso pode comprometer a imagem.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                                    Case "ITA"
                                        MsgBox("Specificare un file di licenza e riprovare. È possibile continuare anche senza, ma ciò potrebbe compromettere l'immagine", vbOKOnly + vbCritical, "Aggiungere i pacchetti AppX approvvigionati")
                                End Select
                            Case 1
                                MsgBox("Please specify a license file and try again. You can also continue without one, but this may compromise the image.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                            Case 2
                                MsgBox("Especifique un archivo de licencia e inténtelo de nuevo. También puede continuar sin uno, pero esta acción podría comprometer la imagen.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                            Case 3
                                MsgBox("Veuillez indiquer un fichier de licence et réessayer. Vous pouvez également continuer sans licence, mais cela risque de compromettre l'image.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                            Case 4
                                MsgBox("Por favor, especifique um ficheiro de licença e tente novamente. Também pode continuar sem um, mas isso pode comprometer a imagem.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                            Case 5
                                MsgBox("Specificare un file di licenza e riprovare. È possibile continuare anche senza, ma ciò potrebbe compromettere l'immagine", vbOKOnly + vbCritical, "Aggiungere i pacchetti AppX approvvigionati")
                        End Select
                        Exit Sub
                    ElseIf Not File.Exists(TextBox1.Text) Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        MsgBox("The license file specified was not found. Make sure it exists on the specified location and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                                    Case "ESN"
                                        MsgBox("El archivo de licencia especificado no se ha encontrado. Asegúrese de que exista en la ubicación especificada e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                                    Case "FRA"
                                        MsgBox("Le fichier de licence spécifié n'a pas été trouvé. Assurez-vous qu'il existe à l'emplacement spécifié et réessayez.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                                    Case "PTB", "PTG"
                                        MsgBox("O ficheiro de licença especificado não foi encontrado. Certifique-se de que existe no local especificado e tente novamente.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                                    Case "ITA"
                                        MsgBox("Il file di licenza specificato non è stato trovato. Assicuratevi che esista nella posizione specificata e riprovate", vbOKOnly + vbCritical, "Aggiungi pacchetti AppX approvvigionati")
                                End Select
                            Case 1
                                MsgBox("The license file specified was not found. Make sure it exists on the specified location and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                            Case 2
                                MsgBox("El archivo de licencia especificado no se ha encontrado. Asegúrese de que exista en la ubicación especificada e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                            Case 3
                                MsgBox("Le fichier de licence spécifié n'a pas été trouvé. Assurez-vous qu'il existe à l'emplacement spécifié et réessayez.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                            Case 4
                                MsgBox("O ficheiro de licença especificado não foi encontrado. Certifique-se de que existe no local especificado e tente novamente.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                            Case 5
                                MsgBox("Il file di licenza specificato non è stato trovato. Assicuratevi che esista nella posizione specificata e riprovate", vbOKOnly + vbCritical, "Aggiungi pacchetti AppX approvvigionati")
                        End Select
                        Exit Sub
                    Else
                        ProgressPanel.appxAdditionUseLicenseFile = True
                        ProgressPanel.appxAdditionLicenseFile = TextBox1.Text
                    End If
                Else
                    ProgressPanel.appxAdditionUseLicenseFile = False
                    ProgressPanel.appxAdditionLicenseFile = ""
                End If
                If CheckBox1.Checked Then
                    If TextBox2.Text = "" Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        MsgBox("Please specify a custom data file and try again. You can also continue without one.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                                    Case "ESN"
                                        MsgBox("Especifique un archivo de datos personalizados e inténtelo de nuevo. También puede continuar sin uno", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                                    Case "FRA"
                                        MsgBox("Veuillez spécifier un fichier de données personnalisé et réessayer. Vous pouvez également continuer sans fichier.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                                    Case "PTB", "PTG"
                                        MsgBox("Especifique um ficheiro de dados personalizado e tente novamente. Também pode continuar sem um.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                                    Case "ITA"
                                        MsgBox("Specificare un file di dati personalizzato e riprovare. È possibile continuare anche senza", vbOKOnly + vbCritical, "Aggiungere pacchetti AppX approvvigionati")
                                End Select
                            Case 1
                                MsgBox("Please specify a custom data file and try again. You can also continue without one.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                            Case 2
                                MsgBox("Especifique un archivo de datos personalizados e inténtelo de nuevo. También puede continuar sin uno", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                            Case 3
                                MsgBox("Veuillez spécifier un fichier de données personnalisé et réessayer. Vous pouvez également continuer sans fichier.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                            Case 4
                                MsgBox("Especifique um ficheiro de dados personalizado e tente novamente. Também pode continuar sem um.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                            Case 5
                                MsgBox("Specificare un file di dati personalizzato e riprovare. È possibile continuare anche senza", vbOKOnly + vbCritical, "Aggiungere pacchetti AppX approvvigionati")
                        End Select
                        Exit Sub
                    ElseIf Not File.Exists(TextBox2.Text) Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        MsgBox("The custom data file specified was not found. Make sure it exists on the specified location and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                                    Case "ESN"
                                        MsgBox("El archivo de datos personalizados especificado no se ha encontrado. Asegúrese de que exista en la ubicación especificada e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                                    Case "FRA"
                                        MsgBox("Le fichier de données personnalisées spécifié n'a pas été trouvé. Assurez-vous qu'il existe à l'emplacement spécifié et réessayez.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                                    Case "PTB", "PTG"
                                        MsgBox("O ficheiro de dados personalizado especificado não foi encontrado. Certifique-se de que existe na localização especificada e tente novamente.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                                    Case "ITA"
                                        MsgBox("Il file di dati personalizzati specificato non è stato trovato. Assicurarsi che esista nella posizione specificata e riprovare", vbOKOnly + vbCritical, "Aggiungere pacchetti AppX approvvigionati")
                                End Select
                            Case 1
                                MsgBox("The custom data file specified was not found. Make sure it exists on the specified location and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                            Case 2
                                MsgBox("El archivo de datos personalizados especificado no se ha encontrado. Asegúrese de que exista en la ubicación especificada e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                            Case 3
                                MsgBox("Le fichier de données personnalisées spécifié n'a pas été trouvé. Assurez-vous qu'il existe à l'emplacement spécifié et réessayez.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                            Case 4
                                MsgBox("O ficheiro de dados personalizado especificado não foi encontrado. Certifique-se de que existe na localização especificada e tente novamente.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                            Case 5
                                MsgBox("Il file di dati personalizzati specificato non è stato trovato. Assicurarsi che esista nella posizione specificata e riprovare", vbOKOnly + vbCritical, "Aggiungere pacchetti AppX approvvigionati")
                        End Select
                        Exit Sub
                    Else
                        ProgressPanel.appxAdditionUseCustomDataFile = True
                        ProgressPanel.appxAdditionCustomDataFile = TextBox2.Text
                    End If
                Else
                    ProgressPanel.appxAdditionUseCustomDataFile = False
                    ProgressPanel.appxAdditionCustomDataFile = ""
                End If
                If CheckBox4.Checked Then
                    ProgressPanel.appxAdditionUseAllRegions = True
                    ProgressPanel.appxAdditionRegions = "all"
                Else
                    ProgressPanel.appxAdditionUseAllRegions = False
                    ProgressPanel.appxAdditionRegions = TextBox3.Text
                End If
                If CheckBox2.Checked And Not MainForm.OnlineManagement Then
                    ProgressPanel.appxAdditionCommit = True
                Else
                    ProgressPanel.appxAdditionCommit = False
                End If
            End If
            ProgressPanel.appxAdditionPackageList = Packages
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 37
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AddProvAppxPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Add provisioned AppX packages"
                        Label1.Text = Text
                        Label2.Text = "Please add packed or unpacked AppX packages by using the buttons below, or by dropping them to the list view below:"
                        Label3.Text = "An AppX package may need some dependencies for it to be installed correctly. If so, you can specify a list of dependencies now:"
                        Label4.Text = "Stub preference:"
                        Label5.Text = "To specify multiple app regions, separate them with a semicolon (;)"
                        Label6.Text = "Select an entry in the list view to show the details of an app and to configure addition settings"
                        Button1.Text = "Add file"
                        Button2.Text = "Add folder"
                        Button3.Text = "Remove all entries"
                        Button4.Text = "Remove all dependencies"
                        Button5.Text = "Remove dependency"
                        Button6.Text = "Add dependency..."
                        Button7.Text = "Browse..."
                        Button8.Text = "Browse..."
                        Button9.Text = "Remove selected entry"
                        Cancel_Button.Text = "Cancel"
                        OK_Button.Text = "OK"
                        CheckBox1.Text = "Custom data file:"
                        CheckBox2.Text = "Commit image after adding AppX packages"
                        CustomDataFileOFD.Title = "Specify a custom data file"
                        GroupBox2.Text = "AppX dependencies"
                        GroupBox3.Text = "AppX regions"
                        LicenseFileOFD.Title = "Specify a license file"
                        LinkLabel1.Text = "App regions need to be in the form of ISO 3166-1 Alpha 2 or Alpha-3 codes. To learn more about these codes, click here"
                        LinkLabel1.LinkArea = New LinkArea(108, 10)
                        ListView1.Columns(0).Text = "File/Folder"
                        ListView1.Columns(1).Text = "Type"
                        ListView1.Columns(2).Text = "Application name"
                        ListView1.Columns(3).Text = "Application publisher"
                        ListView1.Columns(4).Text = "Application version"
                        CheckBox3.Text = "License file:"
                        CheckBox4.Text = "Make app available for all regions"
                        UnpackedAppxFolderFBD.Description = "Please specify a folder containing unpacked AppX files:"
                    Case "ESN"
                        Text = "Añadir paquetes aprovisionados AppX"
                        Label1.Text = Text
                        Label2.Text = "Añada archivos AppX empaquetados o desempaquetados usando los botones de abajo, o soltándolos en la lista de abajo:"
                        Label3.Text = "Un paquete AppX podría necesitar algunas dependencias para que sea instalado correctamente. Si es así, puede especificarlas ahora:"
                        Label4.Text = "Preferencia de talón:"
                        Label5.Text = "Para especificar regiones de aplicación múltiples, sepáralos con un punto y coma (;)"
                        Label6.Text = "Seleccione una entrada en la lista para mostrar los detalles de una aplicación y para configurar opciones de adición"
                        Button1.Text = "Añadir archivo"
                        Button2.Text = "Añadir carpeta"
                        Button3.Text = "Eliminar todas las entradas"
                        Button4.Text = "Eliminar todas las dependencias"
                        Button5.Text = "Eliminar dependencia"
                        Button6.Text = "Añadir dependencia..."
                        Button7.Text = "Examinar..."
                        Button8.Text = "Examinar..."
                        Button9.Text = "Eliminar entrada seleccionada"
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "Aceptar"
                        CheckBox1.Text = "Archivo de datos:"
                        CheckBox2.Text = "Guardar imagen tras añadir paquetes AppX"
                        CustomDataFileOFD.Title = "Especificar un archivo de datos personalizados"
                        GroupBox2.Text = "Dependencias de aplicaciones"
                        GroupBox3.Text = "Regiones de aplicaciones"
                        LicenseFileOFD.Title = "Especificar un archivo de licencia"
                        LinkLabel1.Text = "Las regiones de aplicaciones deben estar en el formato de códigos ISO 3166-1 Alpha 2 o Alpha 3. Saber más acerca de estos códigos"
                        LinkLabel1.LinkArea = New LinkArea(96, 33)
                        ListView1.Columns(0).Text = "Archivo/Carpeta"
                        ListView1.Columns(1).Text = "Tipo"
                        ListView1.Columns(2).Text = "Nombre de aplicación"
                        ListView1.Columns(3).Text = "Publicador de aplicación"
                        ListView1.Columns(4).Text = "Versión de aplicación"
                        CheckBox3.Text = "Archivo de licencia:"
                        CheckBox4.Text = "Hacer aplicación disponible para todas las regiones"
                        UnpackedAppxFolderFBD.Description = "Especifique un directorio contenedor de archivos de una aplicación AppX:"
                    Case "FRA"
                        Text = "Ajouter des paquets AppX provisionnés"
                        Label1.Text = Text
                        Label2.Text = "Veuillez ajouter des paquets AppX emballés ou non emballés en utilisant les boutons ci-dessous, ou en les déposant dans la liste ci-dessous :"
                        Label3.Text = "Un paquet AppX peut avoir besoin de certaines dépendances pour être installé correctement. Si c'est le cas, vous pouvez spécifier une liste de dépendances maintenant :"
                        Label4.Text = "Préférence pour le paquet de stub :"
                        Label5.Text = "Pour spécifier plusieurs régions d'application, séparez-les par un point-virgule ( ;)"
                        Label6.Text = "Sélectionnez une entrée dans la liste pour afficher les détails d'une application et pour configurer les paramètres d'ajout."
                        Button1.Text = "Ajouter un fichier"
                        Button2.Text = "Ajouter un répertoire"
                        Button3.Text = "Supprimer toutes les entrées"
                        Button4.Text = "Supprimer toutes les dépendances"
                        Button5.Text = "Supprimer la dépendance"
                        Button6.Text = "Ajouter une dépendance..."
                        Button7.Text = "Parcourir..."
                        Button8.Text = "Parcourir..."
                        Button9.Text = "Supprimer l'entrée sélectionnée"
                        Cancel_Button.Text = "Annuler"
                        OK_Button.Text = "OK"
                        CheckBox1.Text = "Fichier de données personnalisé :"
                        CheckBox2.Text = "Sauvegarder l'image après l'ajout de paquets AppX"
                        CustomDataFileOFD.Title = "Spécifier un fichier de données personnalisé"
                        GroupBox2.Text = "Dépendances AppX"
                        GroupBox3.Text = "Régions AppX"
                        LicenseFileOFD.Title = "Spécifier un fichier de licence"
                        LinkLabel1.Text = "Les régions d'application doivent être présentées sous la forme de codes ISO 3166-1 Alpha 2 ou Alpha 3. Pour en savoir plus sur ces codes, cliquez ici"
                        LinkLabel1.LinkArea = New LinkArea(139, 11)
                        ListView1.Columns(0).Text = "Fichier/Répertoire"
                        ListView1.Columns(1).Text = "Type"
                        ListView1.Columns(2).Text = "Nom de l'application"
                        ListView1.Columns(3).Text = "Éditeur de l'application"
                        ListView1.Columns(4).Text = "Version de l'application"
                        CheckBox3.Text = "Fichier de licence :"
                        CheckBox4.Text = "Mettre l'application à la disposition de toutes les régions"
                        UnpackedAppxFolderFBD.Description = "Veuillez spécifier un répertoire contenant les fichiers AppX décompressés :"
                    Case "PTB", "PTG"
                        Text = "Adicionar pacotes AppX provisionados"
                        Label1.Text = Text
                        Label2.Text = "Adicione pacotes AppX embalados ou descompactados utilizando os botões abaixo, ou largando-os na vista de lista abaixo:"
                        Label3.Text = "Um pacote AppX pode precisar de algumas dependências para ser instalado corretamente. Se assim for, pode especificar uma lista de dependências agora:"
                        Label4.Text = "Preferência de pacote de stub:"
                        Label5.Text = "Para especificar várias regiões de aplicação, separe-as com um ponto e vírgula (;)"
                        Label6.Text = "Seleccione uma entrada na vista de lista para mostrar os detalhes de uma aplicação e para configurar definições adicionais"
                        Button1.Text = "Adicionar ficheiro"
                        Button2.Text = "Adicionar pasta"
                        Button3.Text = "Remover todas as entradas"
                        Button4.Text = "Remover todas as dependências"
                        Button5.Text = "Remover dependência"
                        Button6.Text = "Adicionar dependência..."
                        Button7.Text = "Navegar..."
                        Button8.Text = "Navegar..."
                        Button9.Text = "Remover entrada selecionada"
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "OK"
                        CheckBox1.Text = "Ficheiro de dados personalizado:"
                        CheckBox2.Text = "Confirmar imagem depois de adicionar pacotes AppX"
                        CustomDataFileOFD.Title = "Especificar um ficheiro de dados personalizado"
                        GroupBox2.Text = "Dependências AppX"
                        GroupBox3.Text = "Regiões da AppX"
                        LicenseFileOFD.Title = "Especificar um ficheiro de licença"
                        LinkLabel1.Text = "As regiões da aplicação têm de estar na forma de códigos ISO 3166-1 Alpha 2 ou Alpha-3. Para saber mais sobre estes códigos, clique aqui"
                        LinkLabel1.LinkArea = New LinkArea(125, 11)
                        ListView1.Columns(0).Text = "Ficheiro/Pasta"
                        ListView1.Columns(1).Text = "Tipo"
                        ListView1.Columns(2).Text = "Nome da aplicação"
                        ListView1.Columns(3).Text = "Editor da aplicação"
                        ListView1.Columns(4).Text = "Versão da aplicação"
                        CheckBox3.Text = "Ficheiro de licença:"
                        CheckBox4.Text = "Tornar a aplicação disponível para todas as regiões"
                        UnpackedAppxFolderFBD.Description = "Especifique uma pasta que contenha ficheiros AppX descompactados:"
                    Case "ITA"
                        Text = "Aggiungi pacchetti AppX approvvigionati"
                        Label1.Text = Text
                        Label2.Text = "Aggiungere pacchetti AppX imballati o non imballati utilizzando i pulsanti sottostanti o rilasciandoli nella vista elenco sottostante:"
                        Label3.Text = "Un pacchetto AppX può richiedere alcune dipendenze per essere installato correttamente. In tal caso, è possibile specificare un elenco di dipendenze:"
                        Label4.Text = "Preferenza pacchetto stub:"
                        Label5.Text = "Per specificare più regioni di app, separarle con un punto e virgola (;)"
                        Label6.Text = "Selezionate un elemento nella vista elenco per visualizzare i dettagli di un'applicazione e per configurare le impostazioni aggiuntive"
                        Button1.Text = "Aggiungi file"
                        Button2.Text = "Aggiungi cartella"
                        Button3.Text = "Rimuovi tutte le voci"
                        Button4.Text = "Rimuovi tutte le dipendenze"
                        Button5.Text = "Rimuovi dipendenza"
                        Button6.Text = "Aggiungi dipendenza..."
                        Button7.Text = "Sfogliare..."
                        Button8.Text = "Sfoglia..."
                        Button9.Text = "Rimuovi voce selezionata"
                        Cancel_Button.Text = "Annullare"
                        OK_Button.Text = "OK"
                        CheckBox1.Text = "File dati personalizzato:"
                        CheckBox2.Text = "Impegnare l'immagine dopo l'aggiunta dei pacchetti AppX"
                        CustomDataFileOFD.Title = "Specifica un file di dati personalizzato"
                        GroupBox2.Text = "Dipendenze AppX"
                        GroupBox3.Text = "Regioni di AppX"
                        LicenseFileOFD.Title = "Specificare un file di licenza"
                        LinkLabel1.Text = "Le regioni dell'app devono essere sotto forma di codici ISO 3166-1 Alpha 2 o Alpha-3. Per saperne di più su questi codici, fare clic qui"
                        LinkLabel1.LinkArea = New LinkArea(123, 13)
                        ListView1.Columns(0).Text = "File/Cartella"
                        ListView1.Columns(1).Text = "Tipo"
                        ListView1.Columns(2).Text = "Nome applicazione"
                        ListView1.Columns(3).Text = "Editore dell'applicazione"
                        ListView1.Columns(4).Text = "Versione dell'applicazione"
                        CheckBox3.Text = "File di licenza:"
                        CheckBox4.Text = "Rendi l'applicazione disponibile per tutte le regioni"
                        UnpackedAppxFolderFBD.Description = "Specificare una cartella contenente i file AppX scompattati:"
                End Select
            Case 1
                Text = "Add provisioned AppX packages"
                Label1.Text = Text
                Label2.Text = "Please add packed or unpacked AppX packages by using the buttons below, or by dropping them to the list view below:"
                Label3.Text = "An AppX package may need some dependencies for it to be installed correctly. If so, you can specify a list of dependencies now:"
                Label4.Text = "Stub preference:"
                Label5.Text = "To specify multiple app regions, separate them with a semicolon (;)"
                Label6.Text = "Select an entry in the list view to show the details of an app and to configure addition settings"
                Button1.Text = "Add file"
                Button2.Text = "Add folder"
                Button3.Text = "Remove all entries"
                Button4.Text = "Remove all dependencies"
                Button5.Text = "Remove dependency"
                Button6.Text = "Add dependency..."
                Button7.Text = "Browse..."
                Button8.Text = "Browse..."
                Button9.Text = "Remove selected entry"
                Cancel_Button.Text = "Cancel"
                OK_Button.Text = "OK"
                CheckBox1.Text = "Custom data file:"
                CheckBox2.Text = "Commit image after adding AppX packages"
                CustomDataFileOFD.Title = "Specify a custom data file"
                GroupBox2.Text = "AppX dependencies"
                GroupBox3.Text = "AppX regions"
                LicenseFileOFD.Title = "Specify a license file"
                LinkLabel1.Text = "App regions need to be in the form of ISO 3166-1 Alpha 2 or Alpha-3 codes. To learn more about these codes, click here"
                LinkLabel1.LinkArea = New LinkArea(108, 10)
                ListView1.Columns(0).Text = "File/Folder"
                ListView1.Columns(1).Text = "Type"
                ListView1.Columns(2).Text = "Application name"
                ListView1.Columns(3).Text = "Application publisher"
                ListView1.Columns(4).Text = "Application version"
                CheckBox3.Text = "License file:"
                CheckBox4.Text = "Make app available for all regions"
                UnpackedAppxFolderFBD.Description = "Please specify a folder containing unpacked AppX files:"
            Case 2
                Text = "Añadir paquetes aprovisionados AppX"
                Label1.Text = Text
                Label2.Text = "Añada archivos AppX empaquetados o desempaquetados usando los botones de abajo, o soltándolos en la lista de abajo:"
                Label3.Text = "Un paquete AppX podría necesitar algunas dependencias para que sea instalado correctamente. Si es así, puede especificarlas ahora:"
                Label4.Text = "Preferencia de talón:"
                Label5.Text = "Para especificar regiones de aplicación múltiples, sepáralos con un punto y coma (;)"
                Label6.Text = "Seleccione una entrada en la lista para mostrar los detalles de una aplicación y para configurar opciones de adición"
                Button1.Text = "Añadir archivo"
                Button2.Text = "Añadir carpeta"
                Button3.Text = "Eliminar todas las entradas"
                Button4.Text = "Eliminar todas las dependencias"
                Button5.Text = "Eliminar dependencia"
                Button6.Text = "Añadir dependencia..."
                Button7.Text = "Examinar..."
                Button8.Text = "Examinar..."
                Button9.Text = "Eliminar entrada seleccionada"
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "Aceptar"
                CheckBox1.Text = "Archivo de datos:"
                CheckBox2.Text = "Guardar imagen tras añadir paquetes AppX"
                CustomDataFileOFD.Title = "Especificar un archivo de datos personalizados"
                GroupBox2.Text = "Dependencias de aplicaciones"
                GroupBox3.Text = "Regiones de aplicaciones"
                LicenseFileOFD.Title = "Especificar un archivo de licencia"
                LinkLabel1.Text = "Las regiones de aplicaciones deben estar en el formato de códigos ISO 3166-1 Alpha 2 o Alpha 3. Saber más acerca de estos códigos"
                LinkLabel1.LinkArea = New LinkArea(96, 33)
                ListView1.Columns(0).Text = "Archivo/Carpeta"
                ListView1.Columns(1).Text = "Tipo"
                ListView1.Columns(2).Text = "Nombre de aplicación"
                ListView1.Columns(3).Text = "Publicador de aplicación"
                ListView1.Columns(4).Text = "Versión de aplicación"
                CheckBox3.Text = "Archivo de licencia:"
                CheckBox4.Text = "Hacer aplicación disponible para todas las regiones"
                UnpackedAppxFolderFBD.Description = "Especifique un directorio contenedor de archivos de una aplicación AppX:"
            Case 3
                Text = "Ajouter des paquets AppX provisionnés"
                Label1.Text = Text
                Label2.Text = "Veuillez ajouter des paquets AppX emballés ou non emballés en utilisant les boutons ci-dessous, ou en les déposant dans la liste ci-dessous :"
                Label3.Text = "Un paquet AppX peut avoir besoin de certaines dépendances pour être installé correctement. Si c'est le cas, vous pouvez spécifier une liste de dépendances maintenant :"
                Label4.Text = "Préférence pour le paquet de stub :"
                Label5.Text = "Pour spécifier plusieurs régions d'application, séparez-les par un point-virgule ( ;)"
                Label6.Text = "Sélectionnez une entrée dans la liste pour afficher les détails d'une application et pour configurer les paramètres d'ajout."
                Button1.Text = "Ajouter un fichier"
                Button2.Text = "Ajouter un répertoire"
                Button3.Text = "Supprimer toutes les entrées"
                Button4.Text = "Supprimer toutes les dépendances"
                Button5.Text = "Supprimer la dépendance"
                Button6.Text = "Ajouter une dépendance..."
                Button7.Text = "Parcourir..."
                Button8.Text = "Parcourir..."
                Button9.Text = "Supprimer l'entrée sélectionnée"
                Cancel_Button.Text = "Annuler"
                OK_Button.Text = "OK"
                CheckBox1.Text = "Fichier de données personnalisé :"
                CheckBox2.Text = "Sauvegarder l'image après l'ajout de paquets AppX"
                CustomDataFileOFD.Title = "Spécifier un fichier de données personnalisé"
                GroupBox2.Text = "Dépendances AppX"
                GroupBox3.Text = "Régions AppX"
                LicenseFileOFD.Title = "Spécifier un fichier de licence"
                LinkLabel1.Text = "Les régions d'application doivent être présentées sous la forme de codes ISO 3166-1 Alpha 2 ou Alpha 3. Pour en savoir plus sur ces codes, cliquez ici"
                LinkLabel1.LinkArea = New LinkArea(139, 11)
                ListView1.Columns(0).Text = "Fichier/Répertoire"
                ListView1.Columns(1).Text = "Type"
                ListView1.Columns(2).Text = "Nom de l'application"
                ListView1.Columns(3).Text = "Éditeur de l'application"
                ListView1.Columns(4).Text = "Version de l'application"
                CheckBox3.Text = "Fichier de licence :"
                CheckBox4.Text = "Mettre l'application à la disposition de toutes les régions"
                UnpackedAppxFolderFBD.Description = "Veuillez spécifier un répertoire contenant les fichiers AppX décompressés :"
            Case 4
                Text = "Adicionar pacotes AppX provisionados"
                Label1.Text = Text
                Label2.Text = "Adicione pacotes AppX embalados ou descompactados utilizando os botões abaixo, ou largando-os na vista de lista abaixo:"
                Label3.Text = "Um pacote AppX pode precisar de algumas dependências para ser instalado corretamente. Se assim for, pode especificar uma lista de dependências agora:"
                Label4.Text = "Preferência de pacote de stub:"
                Label5.Text = "Para especificar várias regiões de aplicação, separe-as com um ponto e vírgula (;)"
                Label6.Text = "Seleccione uma entrada na vista de lista para mostrar os detalhes de uma aplicação e para configurar definições adicionais"
                Button1.Text = "Adicionar ficheiro"
                Button2.Text = "Adicionar pasta"
                Button3.Text = "Remover todas as entradas"
                Button4.Text = "Remover todas as dependências"
                Button5.Text = "Remover dependência"
                Button6.Text = "Adicionar dependência..."
                Button7.Text = "Navegar..."
                Button8.Text = "Navegar..."
                Button9.Text = "Remover entrada selecionada"
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "OK"
                CheckBox1.Text = "Ficheiro de dados personalizado:"
                CheckBox2.Text = "Confirmar imagem depois de adicionar pacotes AppX"
                CustomDataFileOFD.Title = "Especificar um ficheiro de dados personalizado"
                GroupBox2.Text = "Dependências AppX"
                GroupBox3.Text = "Regiões da AppX"
                LicenseFileOFD.Title = "Especificar um ficheiro de licença"
                LinkLabel1.Text = "As regiões da aplicação têm de estar na forma de códigos ISO 3166-1 Alpha 2 ou Alpha-3. Para saber mais sobre estes códigos, clique aqui"
                LinkLabel1.LinkArea = New LinkArea(125, 11)
                ListView1.Columns(0).Text = "Ficheiro/Pasta"
                ListView1.Columns(1).Text = "Tipo"
                ListView1.Columns(2).Text = "Nome da aplicação"
                ListView1.Columns(3).Text = "Editor da aplicação"
                ListView1.Columns(4).Text = "Versão da aplicação"
                CheckBox3.Text = "Ficheiro de licença:"
                CheckBox4.Text = "Tornar a aplicação disponível para todas as regiões"
                UnpackedAppxFolderFBD.Description = "Especifique uma pasta que contenha ficheiros AppX descompactados:"
            Case 5
                Text = "Aggiungi pacchetti AppX approvvigionati"
                Label1.Text = Text
                Label2.Text = "Aggiungere pacchetti AppX imballati o non imballati utilizzando i pulsanti sottostanti o rilasciandoli nella vista elenco sottostante:"
                Label3.Text = "Un pacchetto AppX può richiedere alcune dipendenze per essere installato correttamente. In tal caso, è possibile specificare un elenco di dipendenze:"
                Label4.Text = "Preferenza pacchetto stub:"
                Label5.Text = "Per specificare più regioni di app, separarle con un punto e virgola (;)"
                Label6.Text = "Selezionate un elemento nella vista elenco per visualizzare i dettagli di un'applicazione e per configurare le impostazioni aggiuntive"
                Button1.Text = "Aggiungi file"
                Button2.Text = "Aggiungi cartella"
                Button3.Text = "Rimuovi tutte le voci"
                Button4.Text = "Rimuovi tutte le dipendenze"
                Button5.Text = "Rimuovi dipendenza"
                Button6.Text = "Aggiungi dipendenza..."
                Button7.Text = "Sfogliare..."
                Button8.Text = "Sfoglia..."
                Button9.Text = "Rimuovi voce selezionata"
                Cancel_Button.Text = "Annullare"
                OK_Button.Text = "OK"
                CheckBox1.Text = "File dati personalizzato:"
                CheckBox2.Text = "Impegnare l'immagine dopo l'aggiunta dei pacchetti AppX"
                CustomDataFileOFD.Title = "Specifica un file di dati personalizzato"
                GroupBox2.Text = "Dipendenze AppX"
                GroupBox3.Text = "Regioni di AppX"
                LicenseFileOFD.Title = "Specificare un file di licenza"
                LinkLabel1.Text = "Le regioni dell'app devono essere sotto forma di codici ISO 3166-1 Alpha 2 o Alpha-3. Per saperne di più su questi codici, fare clic qui"
                LinkLabel1.LinkArea = New LinkArea(123, 13)
                ListView1.Columns(0).Text = "File/Cartella"
                ListView1.Columns(1).Text = "Tipo"
                ListView1.Columns(2).Text = "Nome applicazione"
                ListView1.Columns(3).Text = "Editore dell'applicazione"
                ListView1.Columns(4).Text = "Versione dell'applicazione"
                CheckBox3.Text = "File di licenza:"
                CheckBox4.Text = "Rendi l'applicazione disponibile per tutte le regioni"
                UnpackedAppxFolderFBD.Description = "Specificare una cartella contenente i file AppX scompattati:"
        End Select
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        StubPreferences(0) = "Do not configure stub preference"
                        StubPreferences(1) = "Install application as a stub package"
                        StubPreferences(2) = "Install application as a full package"
                    Case "ESN"
                        StubPreferences(0) = "No configurar preferencia de talón"
                        StubPreferences(1) = "Instalar aplicación como un paquete talón"
                        StubPreferences(2) = "Instalar aplicación como un paquete completo"
                    Case "FRA"
                        StubPreferences(0) = "Ne pas configurer la préférence de stub"
                        StubPreferences(1) = "Installer l'application en tant que paquet partiel"
                        StubPreferences(2) = "Installer l'application en tant que paquet complet"
                    Case "PTB", "PTG"
                        StubPreferences(0) = "Não configurar preferência de stub"
                        StubPreferences(1) = "Instalar a aplicação como um pacote de stub"
                        StubPreferences(2) = "Instalar a aplicação como um pacote completo"
                    Case "ITA"
                        StubPreferences(0) = "Non configurare le preferenze di stub"
                        StubPreferences(1) = "Installa l'applicazione come pacchetto stub"
                        StubPreferences(2) = "Installa l'applicazione come pacchetto completo"
                End Select
            Case 1
                StubPreferences(0) = "Do not configure stub preference"
                StubPreferences(1) = "Install application as a stub package"
                StubPreferences(2) = "Install application as a full package"
            Case 2
                StubPreferences(0) = "No configurar preferencia de talón"
                StubPreferences(1) = "Instalar aplicación como un paquete talón"
                StubPreferences(2) = "Instalar aplicación como un paquete completo"
            Case 3
                StubPreferences(0) = "Ne pas configurer la préférence de stub"
                StubPreferences(1) = "Installer l'application en tant que paquet partiel"
                StubPreferences(2) = "Installer l'application en tant que paquet complet"
            Case 4
                StubPreferences(0) = "Não configurar preferência de stub"
                StubPreferences(1) = "Instalar a aplicação como um pacote de stub"
                StubPreferences(2) = "Instalar a aplicação como um pacote completo"
            Case 5
                StubPreferences(0) = "Non configurare le preferenze di stub"
                StubPreferences(1) = "Installa l'applicazione come pacchetto stub"
                StubPreferences(2) = "Installa l'applicazione come pacchetto completo"
        End Select
        ComboBox1.Items.AddRange(StubPreferences)
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            GroupBox3.ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            GroupBox3.ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            ListBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        CheckBox2.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        AppxDetailsPanel.Height = If(ListView1.SelectedItems.Count <= 0, 520, 83)
        If (FileVersionInfo.GetVersionInfo(MainForm.DismExe).ProductMajorPart >= 10 And FileVersionInfo.GetVersionInfo(MainForm.DismExe).ProductBuildPart >= 17134) And
            (MainForm.imgVersionInfo.Major >= 10 And MainForm.imgVersionInfo.Build >= 17134) Then
            GroupBox3.Enabled = True
        Else
            GroupBox3.Enabled = False
        End If
        If FileVersionInfo.GetVersionInfo(MainForm.DismExe).ProductMajorPart >= 10 And MainForm.imgVersionInfo.Major >= 10 Then
            Panel2.Enabled = True
        Else
            Panel2.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AppxFileOFD.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If UnpackedAppxFolderFBD.ShowDialog = Windows.Forms.DialogResult.OK And UnpackedAppxFolderFBD.SelectedPath <> "" Then
            ScanAppxPackage(True, UnpackedAppxFolderFBD.SelectedPath)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Packages.Clear()
        ListView1.Items.Clear()
        Button3.Enabled = False
        Button9.Enabled = False
        NoAppxFilePanel.Visible = True
        AppxFilePanel.Visible = False
        AppxDetailsPanel.Height = 520
        FlowLayoutPanel1.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ListBox1.Items.Clear()
        Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Clear()
        Button4.Enabled = False
        Button5.Enabled = False
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If Not ListBox1.SelectedItem = "" Then
            'Dim dep As New AppxDependency()
            'dep.DependencyFile.Add(ListBox1.SelectedItem)
            Dim deps As New List(Of AppxDependency)
            deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
            deps.RemoveAt(ListBox1.SelectedIndex)
            Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
            ListBox1.Items.Remove(ListBox1.SelectedItem)
        End If
        If ListBox1.SelectedItem = "" Then
            Button5.Enabled = False
        Else
            Button5.Enabled = True
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        AppxDependencyOFD.ShowDialog()
    End Sub

    Private Sub AppxFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AppxFileOFD.FileOk
        If AppxFileOFD.FileNames.Count > 0 Then
            For Each AppxFile In AppxFileOFD.FileNames
                If Path.GetExtension(AppxFile).Equals(".appinstaller", StringComparison.OrdinalIgnoreCase) Then
                    If Not AppInstallerDownloader.IsDisposed Then AppInstallerDownloader.Dispose()
                    AppInstallerDownloader.AppInstallerFile = AppxFile
                    If Not File.Exists(AppxFile.Replace(".appinstaller", ".appxbundle").Trim()) Then AppInstallerDownloader.ShowDialog(Me)
                    If File.Exists(AppxFile.Replace(".appinstaller", ".appxbundle").Trim()) Then ScanAppxPackage(False, AppxFile.Replace(".appinstaller", ".appxbundle").Trim())
                    Continue For
                End If
                ScanAppxPackage(False, AppxFile)
            Next
        End If
    End Sub

    Private Sub AppxDependencyOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AppxDependencyOFD.FileOk
        If ListView1.SelectedItems.Count = 1 Then
            Dim dep As New AppxDependency()
            dep.DependencyFile = AppxDependencyOFD.FileName
            If Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count > 0 And Not Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Contains(dep) Then
                Dim deps As New List(Of AppxDependency)
                deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                deps.Add(dep)
                Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
            ElseIf Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count = 0 Then
                Dim deps As New List(Of AppxDependency)
                deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                deps.Add(dep)
                Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
            End If
            ListBox1.Items.Add(AppxDependencyOFD.FileName)
            If ListBox1.Items.Count > 0 Then
                Button4.Enabled = True
            End If
        End If
    End Sub

    Private Sub LicenseFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles LicenseFileOFD.FileOk
        If ListView1.SelectedItems.Count = 1 Then
            TextBox1.Text = LicenseFileOFD.FileName
        End If
    End Sub

    Private Sub CustomDataFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CustomDataFileOFD.FileOk
        If ListView1.SelectedItems.Count = 1 Then
            TextBox2.Text = CustomDataFileOFD.FileName
        End If
    End Sub

    ''' <summary>
    ''' DISMTools AppX header scanner component: version 0.5.1
    ''' </summary>
    ''' <param name="IsFolder">Determines whether the given value for "Package" is a folder</param>
    ''' <param name="Package">The name of the packed or unpacked AppX file. It may be a file containing the full structure, or a folder containing all AppX files</param>
    ''' <remarks>Scans the header of AppX packages to gather application name, publisher, and version information</remarks>
    Sub ScanAppxPackage(IsFolder As Boolean, Package As String)
        ' Detect if the package specified is encrypted
        If Path.GetExtension(Package).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) AndAlso MainForm.OnlineManagement Then
            If Not Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase) Then
                Dim uEAppProc As New Process()
                uEAppProc.StartInfo.FileName = Application.StartupPath & "\Tools\UnpEax\UnpEax.exe"
                uEAppProc.StartInfo.Arguments = Quote & Package & Quote
                If Not Debugger.IsAttached Then
                    uEAppProc.StartInfo.CreateNoWindow = True
                    uEAppProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                End If
                uEAppProc.Start()
                uEAppProc.WaitForExit()
                If uEAppProc.ExitCode = 0 Then
                    If Directory.Exists(Application.StartupPath & "\appxscan") Then Directory.Delete(Application.StartupPath & "\appxscan", True)
                    Directory.CreateDirectory(Application.StartupPath & "\appxscan")
                    Directory.CreateDirectory(Application.StartupPath & "\appxscan\Assets")
                    For Each asset In Directory.GetFiles(Path.Combine(Application.StartupPath, Package.Replace(Path.GetExtension(Package), "").Trim(), "Assets"))
                        File.Copy(asset, Path.Combine(Application.StartupPath, "appxscan\Assets", Path.GetFileName(asset)))
                    Next
                    File.Copy(Path.Combine(Application.StartupPath, Package.Replace(Path.GetExtension(Package), "").Trim(), "AppxManifest.xml"), Application.StartupPath & "\appxscan\AppxManifest.xml")
                    If Directory.Exists(Path.Combine(Application.StartupPath, Package.Replace(Path.GetExtension(Package), "").Trim())) Then
                        Directory.Delete(Path.Combine(Application.StartupPath, Package.Replace(Path.GetExtension(Package), "").Trim()), True)
                    End If
                    Dim EScannerRTB As New RichTextBox()
                    EScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\appxscan\AppxManifest.xml")
                    Dim EIdScanner As String
                    Dim EcurrentAppxName As String = ""
                    Dim EcurrentAppxPublisher As String = ""
                    Dim EcurrentAppxVersion As String = ""
                    Dim EcurrentAppxArchitecture As String = ""
                    For x = 0 To EScannerRTB.Lines.Count - 1
                        If EScannerRTB.Lines(x).Contains("<Identity") Then
                            EIdScanner = EScannerRTB.Lines(x)
                            Dim serializer As New XmlSerializer(GetType(AppxPackage))
                            Using tReader As TextReader = New StringReader(EIdScanner)
                                Using reader As XmlReader = XmlReader.Create(tReader)
                                    Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                    EcurrentAppxName = id.PackageName
                                    EcurrentAppxPublisher = id.PackagePublisher
                                    EcurrentAppxVersion = id.PackageVersion
                                    EcurrentAppxArchitecture = id.PackageArchitecture
                                End Using
                            End Using
                            AppxNameList.Add(EcurrentAppxName)
                            AppxPublisherList.Add(EcurrentAppxPublisher)
                            AppxVersionList.Add(EcurrentAppxVersion)
                            AppxNames = AppxNameList.ToArray()
                            AppxPublishers = AppxPublisherList.ToArray()
                            AppxVersion = AppxVersionList.ToArray()
                            ' Add the package right away
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            If IsFolder Then
                                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Unpacked (Encrypted)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                            Else
                                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Packed (Encrypted)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                            End If
                                        Case "ESN"
                                            If IsFolder Then
                                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Desempaquetado (Encriptado)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                            Else
                                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Empaquetado (Encriptado)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                            End If
                                        Case "FRA"
                                            If IsFolder Then
                                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Décompacté (Crypté)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                            Else
                                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Compacté (Crypté)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                            End If
                                        Case "PTB", "PTG"
                                            If IsFolder Then
                                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Desembalado (Encriptado)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                            Else
                                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Embalado (Encriptado)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                            End If
                                        Case "ITA"
                                            If IsFolder Then
                                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Disimballato (criptato)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                            Else
                                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Imballato (criptato)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                            End If
                                    End Select
                                Case 1
                                    If IsFolder Then
                                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Unpacked (Encrypted)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                    Else
                                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Packed (Encrypted)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                    End If
                                Case 2
                                    If IsFolder Then
                                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Desempaquetado (Encriptado)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                    Else
                                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Empaquetado (Encriptado)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                    End If
                                Case 3
                                    If IsFolder Then
                                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Décompacté (Crypté)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                    Else
                                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Compacté (Crypté)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                    End If
                                Case 4
                                    If IsFolder Then
                                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Desembalado (Encriptado)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                    Else
                                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Embalado (Encriptado)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                    End If
                                Case 5
                                    If IsFolder Then
                                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Disimballato (criptato)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                    Else
                                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Imballato (criptato)", EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                                    End If
                            End Select
                            Exit For
                        End If
                    Next
                    Dim extPackage As New AppxPackage()
                    extPackage.PackageFile = Package
                    extPackage.PackageName = EcurrentAppxName
                    extPackage.PackagePublisher = EcurrentAppxPublisher
                    extPackage.PackageVersion = EcurrentAppxVersion
                    extPackage.PackageArchitecture = EcurrentAppxArchitecture
                    extPackage.StubPackageOption = StubPreference.NoPreference
                    If Not Packages.Contains(extPackage) Then Packages.Add(extPackage)
                    Button3.Enabled = True
                    GetApplicationStoreLogoAssets("", False, False, Package, EcurrentAppxName)
                    Exit Sub
                End If
            End If
            ' Add the package right away
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            If IsFolder Then
                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Unpacked (Encrypted)", "Encrypted application", "Encrypted application", "Encrypted application"}))
                            Else
                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Packed (Encrypted)", "Encrypted application", "Encrypted application", "Encrypted application"}))
                            End If
                        Case "ESN"
                            If IsFolder Then
                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Desempaquetado (Encriptado)", "Aplicación encriptada", "Aplicación encriptada", "Aplicación encriptada"}))
                            Else
                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Empaquetado (Encriptado)", "Aplicación encriptada", "Aplicación encriptada", "Aplicación encriptada"}))
                            End If
                        Case "FRA"
                            If IsFolder Then
                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Décompacté (Crypté)", "Application cryptée", "Application cryptée", "Application cryptée"}))
                            Else
                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Compacté (Crypté)", "Application cryptée", "Application cryptée", "Application cryptée"}))
                            End If
                        Case "PTB", "PTG"
                            If IsFolder Then
                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Desembalado (Encriptado)", "Aplicação encriptada", "Aplicação encriptada", "Aplicação encriptada"}))
                            Else
                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Embalado (Encriptado)", "Aplicação encriptada", "Aplicação encriptada", "Aplicação encriptada"}))
                            End If
                        Case "ITA"
                            If IsFolder Then
                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Disimballato (criptato)", "Applicazione criptata", "Applicazione criptata", "Applicazione criptata"}))
                            Else
                                ListView1.Items.Add(New ListViewItem(New String() {Package, "Imballato (criptato)", "Applicazione criptata", "Applicazione criptata", "Applicazione criptata"}))
                            End If
                    End Select
                Case 1
                    If IsFolder Then
                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Unpacked (Encrypted)", "Encrypted application", "Encrypted application", "Encrypted application"}))
                    Else
                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Packed (Encrypted)", "Encrypted application", "Encrypted application", "Encrypted application"}))
                    End If
                Case 2
                    If IsFolder Then
                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Desempaquetado (Encriptado)", "Aplicación encriptada", "Aplicación encriptada", "Aplicación encriptada"}))
                    Else
                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Empaquetado (Encriptado)", "Aplicación encriptada", "Aplicación encriptada", "Aplicación encriptada"}))
                    End If
                Case 3
                    If IsFolder Then
                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Décompacté (Crypté)", "Application cryptée", "Application cryptée", "Application cryptée"}))
                    Else
                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Compacté (Crypté)", "Application cryptée", "Application cryptée", "Application cryptée"}))
                    End If
                Case 4
                    If IsFolder Then
                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Desembalado (Encriptado)", "Aplicação encriptada", "Aplicação encriptada", "Aplicação encriptada"}))
                    Else
                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Embalado (Encriptado)", "Aplicação encriptada", "Aplicação encriptada", "Aplicação encriptada"}))
                    End If
                Case 5
                    If IsFolder Then
                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Disimballato (criptato)", "Applicazione criptata", "Applicazione criptata", "Applicazione criptata"}))
                    Else
                        ListView1.Items.Add(New ListViewItem(New String() {Package, "Imballato (criptato)", "Applicazione criptata", "Applicazione criptata", "Applicazione criptata"}))
                    End If
            End Select
            Dim encPackage As New AppxPackage()
            encPackage.PackageFile = Package
            encPackage.PackageName = "<Encrypted>"
            encPackage.PackagePublisher = "<Encrypted>"
            encPackage.PackageVersion = "<Encrypted>"
            encPackage.PackageArchitecture = "<Encrypted>"
            encPackage.StubPackageOption = StubPreference.NoPreference
            If Not Packages.Contains(encPackage) Then Packages.Add(encPackage)
            Button3.Enabled = True
            Exit Sub
        ElseIf Path.GetExtension(Package).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) AndAlso Not MainForm.OnlineManagement Then
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "The package:" & CrLf & CrLf & Package & CrLf & CrLf & "is an encrypted application package. Neither DISMTools nor DISM support adding these application types. If you'd like to add it, you can do so, after the image is applied and booted to."
                        Case "ESN"
                            msg = "El paquete:" & CrLf & CrLf & Package & CrLf & CrLf & "es un paquete de aplicación encriptado. Ni DISMTools ni DISM soportan añadir estos tipos de aplicaciones. Si le gustaría añadirlo, puede hacerlo, después de que la imagen se haya aplicado e iniciado."
                        Case "FRA"
                            msg = "Le paquet :" & CrLf & CrLf & Package & CrLf & CrLf & "est un paquet d'applications cryptées. Ni DISMTools ni DISM ne supportent l'ajout de ces types d'applications. Si vous souhaitez l'ajouter, vous pouvez le faire après l'application de l'image et le démarrage."
                        Case "PTB", "PTG"
                            msg = "O pacote:" & CrLf & CrLf & Package & CrLf & CrLf & "é um pacote de aplicações encriptadas. Nem o DISMTools nem o DISM suportam a adição destes tipos de aplicações. Se quiser adicioná-lo, pode fazê-lo depois de a imagem ser aplicada e inicializada."
                        Case "ITA"
                            msg = "Il pacchetto:" & CrLf & CrLf & Package & CrLf & CrLf & "è un pacchetto di applicazioni criptate. Né DISMTools né DISM supportano l'aggiunta di questi tipi di applicazioni. Se si desidera aggiungerlo, è possibile farlo dopo che l'immagine è stata applicata e avviata."
                    End Select
                Case 1
                    msg = "The package:" & CrLf & CrLf & Package & CrLf & CrLf & "is an encrypted application package. Neither DISMTools nor DISM support adding these application types. If you'd like to add it, you can do so, after the image is applied and booted to."
                Case 2
                    msg = "El paquete:" & CrLf & CrLf & Package & CrLf & CrLf & "es un paquete de aplicación encriptado. Ni DISMTools ni DISM soportan añadir estos tipos de aplicaciones. Si le gustaría añadirlo, puede hacerlo, después de que la imagen se haya aplicado e iniciado."
                Case 3
                    msg = "Le paquet :" & CrLf & CrLf & Package & CrLf & CrLf & "est un paquet d'applications cryptées. Ni DISMTools ni DISM ne supportent l'ajout de ces types d'applications. Si vous souhaitez l'ajouter, vous pouvez le faire après l'application de l'image et le démarrage."
                Case 4
                    msg = "O pacote:" & CrLf & CrLf & Package & CrLf & CrLf & "é um pacote de aplicações encriptadas. Nem o DISMTools nem o DISM suportam a adição destes tipos de aplicações. Se quiser adicioná-lo, pode fazê-lo depois de a imagem ser aplicada e inicializada."
                Case 5
                    msg = "Il pacchetto:" & CrLf & CrLf & Package & CrLf & CrLf & "è un pacchetto di applicazioni criptate. Né DISMTools né DISM supportano l'aggiunta di questi tipi di applicazioni. Se si desidera aggiungerlo, è possibile farlo dopo che l'immagine è stata applicata e avviata."
            End Select
            MsgBox(msg, vbOKOnly + vbExclamation, Label1.Text)
            Exit Sub
        End If
        Dim Stepper As Integer = 2
        Dim QuoteCount As Integer = 0
        Dim ScannerRTB As New RichTextBox()
        Dim currentAppxName As String = ""
        Dim currentAppxPublisher As String = ""
        Dim currentAppxVersion As String = ""
        Dim currentAppxArchitecture As String = ""
        Dim pkgName As String = ""
        Dim IdScanner As String
        Dim StubSupported As Boolean = False
        If IsFolder Then
            If File.Exists(Package & "\AppxMetadata\AppxBundleManifest.xml") Then
                ' AppXBundle file
                ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Package & "\AppxMetadata\AppxBundleManifest.xml")
                StubSupported = ScannerRTB.Text.Contains("IsStub=" & Quote & "true" & Quote)
                IdScanner = ScannerRTB.Lines(If(ScannerRTB.Lines(2).EndsWith("<!--"), 10, 4))
                Dim CharIndex As Integer = 0
                Dim CharNext As Integer
                For Each Character As Char In ScannerRTB.Lines(If(ScannerRTB.Lines(2).EndsWith("<!--"), 10, 4))
                    CharNext = CharIndex + 1
                    If Not IdScanner(CharIndex) = Quote Then
                        CharIndex += 1
                        Continue For
                    ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                        CharIndex += 1
                        Continue For
                    Else
                        Character = IdScanner(CharIndex + 1)
                        If Not IdScanner(CharIndex + Stepper) = " " Then
                            If QuoteCount = 3 Then
                                QuoteCount += 1
                                Do
                                    If Character = Quote Then
                                        CharIndex += Stepper - 1
                                        Character = IdScanner(CharIndex - 1)
                                        QuoteCount += 1
                                        Stepper = 2
                                        Exit For
                                    Else
                                        pkgName &= Character.ToString()
                                        Character = IdScanner(CharIndex + Stepper)
                                        Stepper += 1
                                    End If
                                Loop
                            Else
                                QuoteCount += 1
                                CharIndex += Stepper - 1
                                Character = IdScanner(CharIndex + Stepper)
                            End If
                        End If
                    End If
                Next
                pkgName = pkgName.Replace(" ", "%20").Trim()
                QuoteCount = 0
                Stepper = 2
                For x = 0 To ScannerRTB.Lines.Count - 1
                    If ScannerRTB.Lines(x).Contains("<Identity") Then
                        IdScanner = ScannerRTB.Lines(x)
                        Dim serializer As New XmlSerializer(GetType(AppxPackage))
                        Using tReader As TextReader = New StringReader(IdScanner)
                            Using reader As XmlReader = XmlReader.Create(tReader)
                                Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                currentAppxName = id.PackageName
                                currentAppxPublisher = id.PackagePublisher
                                currentAppxVersion = id.PackageVersion
                                currentAppxArchitecture = id.PackageArchitecture
                            End Using
                        End Using
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                        Exit For
                    End If
                Next
            ElseIf File.Exists(Package & "\AppxManifest.xml") Then
                ' AppX file
                ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Package & "\AppxManifest.xml")
                For x = 0 To ScannerRTB.Lines.Count - 1
                    If ScannerRTB.Lines(x).Contains("<Identity") Then
                        IdScanner = ScannerRTB.Lines(x)
                        Dim serializer As New XmlSerializer(GetType(AppxPackage))
                        Using tReader As TextReader = New StringReader(IdScanner)
                            Using reader As XmlReader = XmlReader.Create(tReader)
                                Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                currentAppxName = id.PackageName
                                currentAppxPublisher = id.PackagePublisher
                                currentAppxVersion = id.PackageVersion
                                currentAppxArchitecture = id.PackageArchitecture
                            End Using
                        End Using
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                        Exit For
                    End If
                Next
            Else
                ' Unrecognized type
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MsgBox("This folder doesn't seem to contain an AppX package structure. It will not be added to the list", vbOKOnly + vbExclamation, "Add provisioned AppX packages")
                            Case "ESN"
                                MsgBox("Esta carpeta no parece contener una estructura de un paquete AppX. No será añadida a la lista", vbOKOnly + vbExclamation, "Añadir paquetes aprovisionados AppX")
                            Case "FRA"
                                MsgBox("Ce répertoire ne semble pas contenir de structure de paquetage AppX. Il ne sera pas ajouté à la liste", vbOKOnly + vbExclamation, "Ajouter des paquets AppX provisionnés")
                            Case "PTB", "PTG"
                                MsgBox("Esta pasta não parece conter uma estrutura de pacotes AppX. Não será adicionada à lista", vbOKOnly + vbExclamation, "Adicionar pacotes AppX provisionados")
                            Case "ITA"
                                MsgBox("Questa cartella non sembra contenere una struttura di pacchetti AppX. Non verrà aggiunta all'elenco", vbOKOnly + vbExclamation, "Aggiungi pacchetti AppX approvvigionati")
                        End Select
                    Case 1
                        MsgBox("This folder doesn't seem to contain an AppX package structure. It will not be added to the list", vbOKOnly + vbExclamation, "Add provisioned AppX packages")
                    Case 2
                        MsgBox("Esta carpeta no parece contener una estructura de un paquete AppX. No será añadida a la lista", vbOKOnly + vbExclamation, "Añadir paquetes aprovisionados AppX")
                    Case 3
                        MsgBox("Ce répertoire ne semble pas contenir de structure de paquetage AppX. Il ne sera pas ajouté à la liste", vbOKOnly + vbExclamation, "Ajouter des paquets AppX provisionnés")
                    Case 4
                        MsgBox("Esta pasta não parece conter uma estrutura de pacotes AppX. Não será adicionada à lista", vbOKOnly + vbExclamation, "Adicionar pacotes AppX provisionados")
                    Case 5
                        MsgBox("Questa cartella non sembra contenere una struttura di pacchetti AppX. Non verrà aggiunta all'elenco", vbOKOnly + vbExclamation, "Aggiungi pacchetti AppX approvvigionati")
                End Select
                Exit Sub
            End If
            GetApplicationStoreLogoAssets(pkgName, True, False, Package, currentAppxName)
        Else
            If Directory.Exists(Application.StartupPath & "\appxscan") Then Directory.Delete(Application.StartupPath & "\appxscan", True)
            Directory.CreateDirectory(Application.StartupPath & "\appxscan")
            AppxScanner.StartInfo.FileName = Application.StartupPath & "\bin\utils\" & If(Environment.Is64BitOperatingSystem, "x64", "x86") & "\7z.exe"
            AppxScanner.StartInfo.Arguments = "e " & Quote & Package & Quote & " " & Quote & If(Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase), "appxmetadata\appxbundlemanifest.xml", "appxmanifest.xml") & Quote & " -o" & Quote & Application.StartupPath & "\appxscan" & Quote
            AppxScanner.StartInfo.CreateNoWindow = True
            AppxScanner.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            AppxScanner.Start()
            AppxScanner.WaitForExit()
            If AppxScanner.ExitCode = 0 Then
                If Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase) Then
                    ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\appxscan\AppxBundleManifest.xml")
                    StubSupported = ScannerRTB.Text.Contains("IsStub=" & Quote & "true" & Quote)
                    IdScanner = ScannerRTB.Lines(If(ScannerRTB.Lines(2).EndsWith("<!--"), 10, 4))
                    Dim CharIndex As Integer = 0
                    Dim CharNext As Integer
                    For Each Character As Char In ScannerRTB.Lines(If(ScannerRTB.Lines(2).EndsWith("<!--"), 10, 4))
                        CharNext = CharIndex + 1
                        If Not IdScanner(CharIndex) = Quote Then
                            CharIndex += 1
                            Continue For
                        ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                            CharIndex += 1
                            Continue For
                        Else
                            Character = IdScanner(CharIndex + 1)
                            If Not IdScanner(CharIndex + Stepper) = " " Then
                                If QuoteCount = 3 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit For
                                        Else
                                            pkgName &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                Else
                                    QuoteCount += 1
                                    CharIndex += Stepper - 1
                                    Character = IdScanner(CharIndex + Stepper)
                                End If
                            End If
                        End If
                    Next
                    pkgName = pkgName.Replace(" ", "%20").Trim()
                    QuoteCount = 0
                    Stepper = 2
                    For x = 0 To ScannerRTB.Lines.Count - 1
                        If ScannerRTB.Lines(x).Contains("<Identity") Then
                            IdScanner = ScannerRTB.Lines(x)
                            Dim serializer As New XmlSerializer(GetType(AppxPackage))
                            Using tReader As TextReader = New StringReader(IdScanner)
                                Using reader As XmlReader = XmlReader.Create(tReader)
                                    Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                    currentAppxName = id.PackageName
                                    currentAppxPublisher = id.PackagePublisher
                                    currentAppxVersion = id.PackageVersion
                                    currentAppxArchitecture = id.PackageArchitecture
                                End Using
                            End Using
                            AppxNameList.Add(currentAppxName)
                            AppxPublisherList.Add(currentAppxPublisher)
                            AppxVersionList.Add(currentAppxVersion)
                            AppxNames = AppxNameList.ToArray()
                            AppxPublishers = AppxPublisherList.ToArray()
                            AppxVersion = AppxVersionList.ToArray()
                            Exit For
                        End If
                    Next
                Else
                    ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\appxscan\AppxManifest.xml")
                    For x = 0 To ScannerRTB.Lines.Count - 1
                        If ScannerRTB.Lines(x).Contains("<Identity") Then
                            IdScanner = ScannerRTB.Lines(x)
                            Dim serializer As New XmlSerializer(GetType(AppxPackage))
                            Using tReader As TextReader = New StringReader(IdScanner)
                                Using reader As XmlReader = XmlReader.Create(tReader)
                                    Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                    currentAppxName = id.PackageName
                                    currentAppxPublisher = id.PackagePublisher
                                    currentAppxVersion = id.PackageVersion
                                    currentAppxArchitecture = id.PackageArchitecture
                                End Using
                            End Using
                            AppxNameList.Add(currentAppxName)
                            AppxPublisherList.Add(currentAppxPublisher)
                            AppxVersionList.Add(currentAppxVersion)
                            AppxNames = AppxNameList.ToArray()
                            AppxPublishers = AppxPublisherList.ToArray()
                            AppxVersion = AppxVersionList.ToArray()
                            Exit For
                        End If
                    Next
                End If
                GetApplicationStoreLogoAssets(pkgName, False, If(Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase), True, False), Package, currentAppxName)
            Else

            End If
        End If
        ' Detect ListView items
        If ListView1.Items.Count > 0 Then
            ' Iterate through the ListView items until we can find an entry with properties similar to those currently obtained
            For Each Item As ListViewItem In ListView1.Items
                If Item.SubItems(2).Text = currentAppxName And Item.SubItems(3).Text = currentAppxPublisher And Item.SubItems(4).Text = currentAppxVersion And Packages(Item.Index).PackageArchitecture = currentAppxArchitecture Then
                    ' Cancel everything
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    MsgBox("The package you want to add is already added to the list, and all its properties match with the properties of the package specified. We won't add the specified package", vbOKOnly + vbCritical, Label1.Text)
                                Case "ESN"
                                    MsgBox("El paquete que desea añadir ya está añadido a la lista, y todas sus propiedades coinciden con las propiedades del paquete especificado. No añadiremos el paquete especificado", vbOKOnly + vbCritical, Label1.Text)
                                Case "FRA"
                                    MsgBox("Le paquet que vous souhaitez ajouter est déjà ajouté à la liste et toutes ses propriétés correspondent à celles du paquet spécifié. Nous n'ajouterons pas le paquet spécifié", vbOKOnly + vbCritical, Label1.Text)
                                Case "PTB", "PTG"
                                    MsgBox("O pacote que pretende adicionar já foi adicionado à lista e todas as suas propriedades coincidem com as propriedades do pacote especificado. Não vamos adicionar o pacote especificado", vbOKOnly + vbCritical, Label1.Text)
                                Case "ITA"
                                    MsgBox("Il pacchetto che si desidera aggiungere è già stato aggiunto all'elenco e tutte le sue proprietà corrispondono a quelle del pacchetto specificato. Non aggiungeremo il pacchetto specificato", vbOKOnly + vbCritical, Label1.Text)
                            End Select
                        Case 1
                            MsgBox("The package you want to add is already added to the list, and all its properties match with the properties of the package specified. We won't add the specified package", vbOKOnly + vbCritical, Label1.Text)
                        Case 2
                            MsgBox("El paquete que desea añadir ya está añadido a la lista, y todas sus propiedades coinciden con las propiedades del paquete especificado. No añadiremos el paquete especificado", vbOKOnly + vbCritical, Label1.Text)
                        Case 3
                            MsgBox("Le paquet que vous souhaitez ajouter est déjà ajouté à la liste et toutes ses propriétés correspondent à celles du paquet spécifié. Nous n'ajouterons pas le paquet spécifié", vbOKOnly + vbCritical, Label1.Text)
                        Case 4
                            MsgBox("O pacote que pretende adicionar já foi adicionado à lista e todas as suas propriedades coincidem com as propriedades do pacote especificado. Não vamos adicionar o pacote especificado", vbOKOnly + vbCritical, Label1.Text)
                        Case 5
                            MsgBox("Il pacchetto che si desidera aggiungere è già stato aggiunto all'elenco e tutte le sue proprietà corrispondono a quelle del pacchetto specificato. Non aggiungeremo il pacchetto specificato", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                    If Directory.Exists(Application.StartupPath & "\appxscan") Then
                        Directory.Delete(Application.StartupPath & "\appxscan", True)
                    End If
                    Exit Sub
                ElseIf Item.SubItems(2).Text = currentAppxName And Not Item.SubItems(3).Text = currentAppxPublisher Then
                    Dim msg As String = ""
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "The package you want to add is already added to the list, but it comes from a different developer or publisher." & CrLf & CrLf & "Do note that applications redistributed by third-party publishers or developers can cause damage to the Windows image." & CrLf & CrLf & "Do you want to replace the entry in the list with the package specified?"
                                Case "ESN"
                                    msg = "El paquete que desea añadir ya está añadido a la lista, pero proviene de un desarrollador o publicador distinto." & CrLf & CrLf & "Dese cuenta de que las aplicaciones redistribuidas por publicadores o desarrolladores de terceros pueden dañar la imagen de Windows." & CrLf & CrLf & "¿Desea reemplazar la entrada en la lista por el paquete especificado?"
                                Case "FRA"
                                    msg = "Le paquet que vous souhaitez ajouter a déjà été ajouté à la liste, mais il provient d'un développeur ou d'un éditeur différent." & CrLf & CrLf & "Notez que les applications redistribuées par des éditeurs ou des développeurs tiers peuvent endommager l'image Windows." & CrLf & CrLf & "Voulez-vous remplacer l'entrée de la liste par le paquet spécifié ?"
                                Case "PTB", "PTG"
                                    msg = "O pacote que pretende adicionar já foi adicionado à lista, mas vem de um programador ou editor diferente." & CrLf & CrLf & "Tenha em atenção que as aplicações redistribuídas por programadores ou editores terceiros podem causar danos na imagem do Windows." & CrLf & CrLf & "Pretende substituir a entrada na lista pelo pacote especificado?"
                                Case "ITA"
                                    msg = "Il pacchetto che si desidera aggiungere è già stato aggiunto all'elenco, ma proviene da uno sviluppatore o da un editore diverso." & CrLf & CrLf & "Si noti che le applicazioni ridistribuite da editori o sviluppatori di terze parti possono causare danni all'immagine di Windows." & CrLf & CrLf & "Si desidera sostituire la voce nell'elenco con il pacchetto specificato?"
                            End Select
                        Case 1
                            msg = "The package you want to add is already added to the list, but it comes from a different developer or publisher." & CrLf & CrLf & "Do note that applications redistributed by third-party publishers or developers can cause damage to the Windows image." & CrLf & CrLf & "Do you want to replace the entry in the list with the package specified?"
                        Case 2
                            msg = "El paquete que desea añadir ya está añadido a la lista, pero proviene de un desarrollador o publicador distinto." & CrLf & CrLf & "Dese cuenta de que las aplicaciones redistribuidas por publicadores o desarrolladores de terceros pueden dañar la imagen de Windows." & CrLf & CrLf & "¿Desea reemplazar la entrada en la lista por el paquete especificado?"
                        Case 3
                            msg = "Le paquet que vous souhaitez ajouter a déjà été ajouté à la liste, mais il provient d'un développeur ou d'un éditeur différent." & CrLf & CrLf & "Notez que les applications redistribuées par des éditeurs ou des développeurs tiers peuvent endommager l'image Windows." & CrLf & CrLf & "Voulez-vous remplacer l'entrée de la liste par le paquet spécifié ?"
                        Case 4
                            msg = "O pacote que pretende adicionar já foi adicionado à lista, mas vem de um programador ou editor diferente." & CrLf & CrLf & "Tenha em atenção que as aplicações redistribuídas por programadores ou editores terceiros podem causar danos na imagem do Windows." & CrLf & CrLf & "Pretende substituir a entrada na lista pelo pacote especificado?"
                        Case 5
                            msg = "Il pacchetto che si desidera aggiungere è già stato aggiunto all'elenco, ma proviene da uno sviluppatore o da un editore diverso." & CrLf & CrLf & "Si noti che le applicazioni ridistribuite da editori o sviluppatori di terze parti possono causare danni all'immagine di Windows." & CrLf & CrLf & "Si desidera sostituire la voce nell'elenco con il pacchetto specificato?"
                    End Select
                    If MsgBox(msg, vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.Yes Then
                        ' Set properties
                        Item.SubItems(0).Text = Package
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Item.SubItems(1).Text = If(IsFolder, "Unpacked", "Packed")
                                    Case "ESN"
                                        Item.SubItems(1).Text = If(IsFolder, "Desempaquetado", "Empaquetado")
                                    Case "FRA"
                                        Item.SubItems(1).Text = If(IsFolder, "Décompacté", "Compacté")
                                    Case "PTB", "PTG"
                                        Item.SubItems(1).Text = If(IsFolder, "Desembalado", "Embalado")
                                    Case "ITA"
                                        Item.SubItems(1).Text = If(IsFolder, "Disimballato", "Imballato")
                                End Select
                            Case 1
                                Item.SubItems(1).Text = If(IsFolder, "Unpacked", "Packed")
                            Case 2
                                Item.SubItems(1).Text = If(IsFolder, "Desempaquetado", "Empaquetado")
                            Case 3
                                Item.SubItems(1).Text = If(IsFolder, "Décompacté", "Compacté")
                            Case 4
                                Item.SubItems(1).Text = If(IsFolder, "Desembalado", "Embalado")
                            Case 5
                                Item.SubItems(1).Text = If(IsFolder, "Disimballato", "Imballato")
                        End Select
                        Item.SubItems(2).Text = currentAppxName
                        Item.SubItems(3).Text = currentAppxPublisher
                        Item.SubItems(4).Text = currentAppxVersion

                        ' Configure Element list
                        Packages(Item.Index).PackageFile = Package
                        Packages(Item.Index).PackageName = currentAppxName
                        Packages(Item.Index).PackagePublisher = currentAppxPublisher
                        Packages(Item.Index).PackageVersion = currentAppxVersion
                    Else
                        If Directory.Exists(Application.StartupPath & "\appxscan") Then
                            Directory.Delete(Application.StartupPath & "\appxscan", True)
                        End If
                    End If
                    Exit Sub
                ElseIf Item.SubItems(2).Text = currentAppxName And Not Item.SubItems(4).Text = currentAppxVersion Then
                    ' This is a rudimentary check which will run even if specifying an older version. It will be improved, so expect the following enhancements:
                    ' - Cast the version strings to version objects
                    ' - Compare the version objects part by part
                    Dim msg As String = ""
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "The package you want to add is already added to the list, but it contains a newer version." & CrLf & CrLf & "Do you want to replace the entry in the list with the updated package specified?"
                                Case "ESN"
                                    msg = "El paquete que desea añadir ya está añadido a la lista, pero contiene una nueva versión." & CrLf & CrLf & "¿Desea reemplazar la entrada en la lista por el paquete actualizado especificado?"
                                Case "FRA"
                                    msg = "Le paquet que vous souhaitez ajouter est déjà ajouté à la liste, mais il contient une version plus récente." & CrLf & CrLf & "Voulez-vous remplacer l'entrée de la liste par le paquet mis à jour spécifié ?"
                                Case "PTB", "PTG"
                                    msg = "O pacote que pretende adicionar já foi adicionado à lista, mas contém uma versão mais recente." & CrLf & CrLf & "Pretende substituir a entrada na lista pelo pacote atualizado especificado?"
                                Case "ITA"
                                    msg = "Il pacchetto che si desidera aggiungere è già stato aggiunto all'elenco, ma contiene una versione più recente." & CrLf & CrLf & "Si desidera sostituire la voce nell'elenco con il pacchetto aggiornato specificato?"
                            End Select
                        Case 1
                            msg = "The package you want to add is already added to the list, but it contains a newer version." & CrLf & CrLf & "Do you want to replace the entry in the list with the updated package specified?"
                        Case 2
                            msg = "El paquete que desea añadir ya está añadido a la lista, pero contiene una nueva versión." & CrLf & CrLf & "¿Desea reemplazar la entrada en la lista por el paquete actualizado especificado?"
                        Case 3
                            msg = "Le paquet que vous souhaitez ajouter est déjà ajouté à la liste, mais il contient une version plus récente." & CrLf & CrLf & "Voulez-vous remplacer l'entrée de la liste par le paquet mis à jour spécifié ?"
                        Case 4
                            msg = "O pacote que pretende adicionar já foi adicionado à lista, mas contém uma versão mais recente." & CrLf & CrLf & "Pretende substituir a entrada na lista pelo pacote atualizado especificado?"
                        Case 5
                            msg = "Il pacchetto che si desidera aggiungere è già stato aggiunto all'elenco, ma contiene una versione più recente." & CrLf & CrLf & "Si desidera sostituire la voce nell'elenco con il pacchetto aggiornato specificato?"
                    End Select
                    If MsgBox(msg, vbYesNo + vbQuestion, Label1.Text) = MsgBoxResult.Yes Then
                        ' Set properties
                        Item.SubItems(0).Text = Package
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Item.SubItems(1).Text = If(IsFolder, "Unpacked", "Packed")
                                    Case "ESN"
                                        Item.SubItems(1).Text = If(IsFolder, "Desempaquetado", "Empaquetado")
                                    Case "FRA"
                                        Item.SubItems(1).Text = If(IsFolder, "Décompacté", "Compacté")
                                    Case "PTB", "PTG"
                                        Item.SubItems(1).Text = If(IsFolder, "Desembalado", "Embalado")
                                    Case "ITA"
                                        Item.SubItems(1).Text = If(IsFolder, "Disimballato", "Imballato")
                                End Select
                            Case 1
                                Item.SubItems(1).Text = If(IsFolder, "Unpacked", "Packed")
                            Case 2
                                Item.SubItems(1).Text = If(IsFolder, "Desempaquetado", "Empaquetado")
                            Case 3
                                Item.SubItems(1).Text = If(IsFolder, "Décompacté", "Compacté")
                            Case 4
                                Item.SubItems(1).Text = If(IsFolder, "Desembalado", "Embalado")
                            Case 5
                                Item.SubItems(1).Text = If(IsFolder, "Disimballato", "Imballato")
                        End Select
                        Item.SubItems(2).Text = currentAppxName
                        Item.SubItems(3).Text = currentAppxPublisher
                        Item.SubItems(4).Text = currentAppxVersion

                        ' Configure Element list
                        Packages(Item.Index).PackageFile = Package
                        Packages(Item.Index).PackageName = currentAppxName
                        Packages(Item.Index).PackagePublisher = currentAppxPublisher
                        Packages(Item.Index).PackageVersion = currentAppxVersion
                    Else
                        If Directory.Exists(Application.StartupPath & "\appxscan") Then
                            Directory.Delete(Application.StartupPath & "\appxscan", True)
                        End If
                    End If
                    Exit Sub
                End If
            Next
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        If IsFolder Then
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Unpacked", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        Else
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Packed", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        End If
                    Case "ESN"
                        If IsFolder Then
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Desempaquetado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        Else
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Empaquetado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        End If
                    Case "FRA"
                        If IsFolder Then
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Décompacté", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        Else
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Compacté", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        End If
                    Case "PTB", "PTG"
                        If IsFolder Then
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Desembalado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        Else
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Embalado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        End If
                    Case "ITA"
                        If IsFolder Then
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Disimballato", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        Else
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Imballato", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        End If
                End Select
            Case 1
                If IsFolder Then
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Unpacked", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                Else
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Packed", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                End If
            Case 2
                If IsFolder Then
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Desempaquetado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                Else
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Empaquetado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                End If
            Case 3
                If IsFolder Then
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Décompacté", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                Else
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Compacté", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                End If
            Case 4
                If IsFolder Then
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Desembalado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                Else
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Embalado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                End If
            Case 5
                If IsFolder Then
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Disimballato", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                Else
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Imballato", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                End If
        End Select
        Dim currentPackage As New AppxPackage()
        currentPackage.PackageFile = Package
        currentPackage.PackageName = currentAppxName
        currentPackage.PackagePublisher = currentAppxPublisher
        currentPackage.PackageVersion = currentAppxVersion
        currentPackage.PackageArchitecture = currentAppxArchitecture
        currentPackage.SupportsStub = StubSupported
        currentPackage.StubPackageOption = StubPreference.NoPreference
        If Not Packages.Contains(currentPackage) Then Packages.Add(currentPackage)
        Button3.Enabled = True
        If Directory.Exists(Application.StartupPath & "\appxscan") Then
            Directory.Delete(Application.StartupPath & "\appxscan", True)
        End If
    End Sub

    ''' <summary>
    ''' Gets the application store logo assets from APPX or APPXBUNDLE packages (also from MSIX and MSIXBUNDLE packages)
    ''' </summary>
    ''' <param name="PackageName">The name of the package. Packages with names containing spaces will replace those with &quot;%20&quot;</param>
    ''' <param name="IsDirectory">Determines if the package given is an unpacked APPX/MSIX/APPXBUNDLE/MSIXBUNDLE file</param>
    ''' <param name="IsBundlePackage">Determines if the package given is an APPXBUNDLE or MSIXBUNDLE package</param>
    ''' <param name="SourcePackage">The path of the source package</param>
    ''' <param name="AppxPackageName">The name of the AppX package, used for storing logo assets in an organized way</param>
    ''' <remarks>If the package processed is an APPXBUNDLE or MSIXBUNDLE package, this procedure will extract the asset contents from the package with the given name. Otherwise, it will directly extract them from the &quot;Assets&quot; folder</remarks>
    Sub GetApplicationStoreLogoAssets(PackageName As String, IsDirectory As Boolean, IsBundlePackage As Boolean, SourcePackage As String, AppxPackageName As String)
        ' The assets from the main package are enough for us. The current AppX XML schema also puts these in the Assets folder, so
        ' getting them should be a breeze
        Try
            If IsDirectory Then
                If File.Exists(SourcePackage & "\AppxMetadata\AppxBundleManifest.xml") Then
                    ' APPXBUNDLE/MSIXBUNDLE
                    AppxScanner.StartInfo.Arguments = "x " & Quote & SourcePackage & "\" & PackageName & Quote & " -o" & Quote & Application.StartupPath & "\appxscan" & Quote
                    AppxScanner.Start()
                    AppxScanner.WaitForExit()
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    If AppxScanner.ExitCode = 0 Then
                        Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets\" & AppxPackageName)
                        If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                            For Each AssetFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\appxscan\Assets", FileIO.SearchOption.SearchTopLevelOnly)
                                If Path.GetFileNameWithoutExtension(AssetFile).StartsWith("small", StringComparison.OrdinalIgnoreCase) Then
                                    File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                                ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("store", StringComparison.OrdinalIgnoreCase) Then
                                    File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                                ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("large", StringComparison.OrdinalIgnoreCase) Then
                                    File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                                End If
                            Next
                        End If
                    End If
                    Directory.Delete(Application.StartupPath & "\appxscan", True)
                ElseIf File.Exists(SourcePackage & "\AppxManifest.xml") Then
                    ' APPX/MSIX
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                        For Each AssetFile In My.Computer.FileSystem.GetFiles(SourcePackage & "\Assets", FileIO.SearchOption.SearchTopLevelOnly)
                            If Path.GetFileNameWithoutExtension(AssetFile).StartsWith("small", StringComparison.OrdinalIgnoreCase) Then
                                File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                            ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("store", StringComparison.OrdinalIgnoreCase) Then
                                File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                            ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("large", StringComparison.OrdinalIgnoreCase) Then
                                File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                            End If
                        Next
                    End If
                Else
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    MsgBox("Could not get application store logo assets from this package - cannot read from manifest", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                                Case "ESN"
                                    MsgBox("No se pudo obtener recursos de logotipos de este paquete - no se puede leer el manifiesto", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                                Case "FRA"
                                    MsgBox("Impossible d'obtenir les ressources du logo de la boutique d'applications à partir de ce paquet - impossible de lire le manifeste.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                                Case "PTB", "PTG"
                                    MsgBox("Não foi possível obter os activos do logótipo da loja de aplicações deste pacote - não é possível ler do manifesto", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                                Case "ITA"
                                    MsgBox("Impossibile ottenere le risorse del logo dell'application store da questo pacchetto - non è possibile leggere dal manifest", vbOKOnly + vbCritical, "Aggiungere pacchetti AppX approvvigionati")
                            End Select
                        Case 1
                            MsgBox("Could not get application store logo assets from this package - cannot read from manifest", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                        Case 2
                            MsgBox("No se pudo obtener recursos de logotipos de este paquete - no se puede leer el manifiesto", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                        Case 3
                            MsgBox("Impossible d'obtenir les ressources du logo de la boutique d'applications à partir de ce paquet - impossible de lire le manifeste.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                        Case 4
                            MsgBox("Não foi possível obter os activos do logótipo da loja de aplicações deste pacote - não é possível ler do manifesto", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                        Case 5
                            MsgBox("Impossibile ottenere le risorse del logo dell'application store da questo pacchetto - non è possibile leggere dal manifest", vbOKOnly + vbCritical, "Aggiungere pacchetti AppX approvvigionati")
                    End Select
                End If
            Else
                If IsBundlePackage Then
                    AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & PackageName & Quote & " -o" & Quote & Application.StartupPath & "\appxscan" & Quote
                    AppxScanner.Start()
                    AppxScanner.WaitForExit()
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    If AppxScanner.ExitCode = 0 Then
                        Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets\" & AppxPackageName)
                        If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                            ' Try extracting small, store and large assets
                            AppxScanner.StartInfo.Arguments = "e " & Quote & Application.StartupPath & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\small*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                            AppxScanner.StartInfo.Arguments = "e " & Quote & Application.StartupPath & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\store*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                            AppxScanner.StartInfo.Arguments = "e " & Quote & Application.StartupPath & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\large*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                        End If
                    End If
                Else
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets\" & AppxPackageName)
                    If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                        If Path.GetExtension(SourcePackage).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) Then
                            For Each asset In Directory.GetFiles(Application.StartupPath & "\appxscan\Assets")
                                File.Copy(asset, Application.StartupPath & "\temp\storeassets\" & AppxPackageName & "\" & Path.GetFileName(asset))
                            Next
                        Else
                            ' Try extracting small, store and large assets
                            AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & "Assets\small*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                            AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & "Assets\store*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                            AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & "Assets\large*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Could not get store logo assets. Reason: " & ex.ToString())
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If ListView1.FocusedItem.Text <> "" Then
            Packages.RemoveAt(ListView1.FocusedItem.Index)
            ListView1.Items.Remove(ListView1.FocusedItem)
            NoAppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, True, False)
            AppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
            AppxDetailsPanel.Height = If(ListView1.SelectedItems.Count <= 0, 520, 83)
            FlowLayoutPanel1.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://en.wikipedia.org/wiki/ISO_3166-1#Current_codes")
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked Then
            TextBox1.Enabled = True
            Button7.Enabled = True
        Else
            TextBox1.Enabled = False
            Button7.Enabled = False
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            TextBox2.Enabled = True
            Button8.Enabled = True
        Else
            TextBox2.Enabled = False
            Button8.Enabled = False
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            TextBox3.Enabled = False
            Label5.Enabled = False
            LinkLabel1.Enabled = False
        Else
            TextBox3.Enabled = True
            Label5.Enabled = True
            LinkLabel1.Enabled = True
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            If ListView1.SelectedItems.Count <> 1 Then
                Button9.Enabled = False
            Else
                Button9.Enabled = True
            End If
            If ListView1.SelectedItems.Count > 1 Then
                DetectMultiSelectionCommonProperties()
            Else
                Label9.Visible = True
                PictureBox2.Visible = True
                TableLayoutPanel3.Enabled = True
                ListBox1.Enabled = True
                CheckBox3.Enabled = True
                TextBox1.Enabled = True
                CheckBox1.Enabled = True
                TextBox2.Enabled = True
                CheckBox4.Enabled = True
                TextBox3.Enabled = True
            End If
        Catch ex As NullReferenceException
            Button9.Enabled = True
        End Try
        NoAppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, True, False)
        AppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        AppxDetailsPanel.Height = If(ListView1.SelectedItems.Count <= 0, 520, 83)
        FlowLayoutPanel1.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        If ListView1.SelectedItems.Count = 1 Then
            Try
                Label7.Text = ListView1.FocusedItem.SubItems(2).Text
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label8.Text = "Publisher: " & ListView1.FocusedItem.SubItems(3).Text
                                Label9.Text = "Version: " & ListView1.FocusedItem.SubItems(4).Text
                            Case "ESN"
                                Label8.Text = "Publicador: " & ListView1.FocusedItem.SubItems(3).Text
                                Label9.Text = "Versión: " & ListView1.FocusedItem.SubItems(4).Text
                            Case "FRA"
                                Label8.Text = "Éditeur : " & ListView1.FocusedItem.SubItems(3).Text
                                Label9.Text = "Version : " & ListView1.FocusedItem.SubItems(4).Text
                            Case "PTB", "PTG"
                                Label8.Text = "Editora: " & ListView1.FocusedItem.SubItems(3).Text
                                Label9.Text = "Versão: " & ListView1.FocusedItem.SubItems(4).Text
                            Case "ITA"
                                Label8.Text = "Editore: " & ListView1.FocusedItem.SubItems(3).Text
                                Label9.Text = "Versione: " & ListView1.FocusedItem.SubItems(4).Text
                        End Select
                    Case 1
                        Label8.Text = "Publisher: " & ListView1.FocusedItem.SubItems(3).Text
                        Label9.Text = "Version: " & ListView1.FocusedItem.SubItems(4).Text
                    Case 2
                        Label8.Text = "Publicador: " & ListView1.FocusedItem.SubItems(3).Text
                        Label9.Text = "Versión: " & ListView1.FocusedItem.SubItems(4).Text
                    Case 3
                        Label8.Text = "Éditeur : " & ListView1.FocusedItem.SubItems(3).Text
                        Label9.Text = "Version : " & ListView1.FocusedItem.SubItems(4).Text
                    Case 4
                        Label8.Text = "Editora: " & ListView1.FocusedItem.SubItems(3).Text
                        Label9.Text = "Versão: " & ListView1.FocusedItem.SubItems(4).Text
                    Case 5
                        Label8.Text = "Editore: " & ListView1.FocusedItem.SubItems(3).Text
                        Label9.Text = "Versione: " & ListView1.FocusedItem.SubItems(4).Text
                End Select
            Catch ex As NullReferenceException

            End Try
        End If
        Try
            If Directory.Exists(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text) And My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count > 0 Then
                PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
                Dim asset As Image = Nothing
                For Each StoreAsset In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text)
                    If Path.GetExtension(StoreAsset).EndsWith("png") Then
                        asset = Image.FromFile(StoreAsset)
                        If asset.Width / asset.Height = 1 Then      ' Determine if the image's aspect ratio is 1:1
                            If asset.Width <= 100 And asset.Height <= 100 Then      ' Determine if it is a "small" or "store" asset
                                PictureBox2.Image = asset
                            End If
                        End If
                    End If
                Next
            Else
                PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
                PictureBox2.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.preview_unavail_dark, My.Resources.preview_unavail_light)
            End If
        Catch ex As Exception
            PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
            PictureBox2.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.preview_unavail_dark, My.Resources.preview_unavail_light)
        End Try

        If ListView1.FocusedItem IsNot Nothing Then
            FlowLayoutPanel1.Enabled = Not Path.GetExtension(ListView1.FocusedItem.SubItems(0).Text).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase)
        End If

        ' Detect properties obtained by the AppxPackage Element
        Try
            If ListView1.SelectedItems.Count = 1 Then
                ListBox1.Items.Clear()
                If Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count > 0 Then
                    For Each Dependency As AppxDependency In Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                        ListBox1.Items.Add(Dependency.DependencyFile)
                    Next
                End If
                TextBox1.Text = Packages(ListView1.FocusedItem.Index).PackageLicenseFile
                If Packages(ListView1.FocusedItem.Index).PackageLicenseFile <> "" And File.Exists(Packages(ListView1.FocusedItem.Index).PackageLicenseFile) Then
                    CheckBox3.Checked = True
                Else
                    CheckBox3.Checked = False
                End If
                TextBox2.Text = Packages(ListView1.FocusedItem.Index).PackageCustomDataFile
                If Packages(ListView1.FocusedItem.Index).PackageCustomDataFile <> "" And File.Exists(Packages(ListView1.FocusedItem.Index).PackageCustomDataFile) Then
                    CheckBox1.Checked = True
                Else
                    CheckBox1.Checked = False
                End If
                TextBox3.Text = Packages(ListView1.FocusedItem.Index).PackageRegions
                If TextBox3.Text = "" Then
                    CheckBox4.Checked = True
                Else
                    CheckBox4.Checked = False
                End If
                If (FileVersionInfo.GetVersionInfo(MainForm.DismExe).ProductMajorPart >= 10 And MainForm.imgVersionInfo.Major >= 10) And
                    Packages(ListView1.FocusedItem.Index).SupportsStub Then
                    Panel2.Enabled = True
                    Select Case Packages(ListView1.FocusedItem.Index).StubPackageOption
                        Case StubPreference.NoPreference
                            ComboBox1.SelectedIndex = 0
                        Case StubPreference.StubOnly
                            ComboBox1.SelectedIndex = 1
                        Case StubPreference.FullPackage
                            ComboBox1.SelectedIndex = 2
                    End Select
                Else
                    Panel2.Enabled = False
                    ComboBox1.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            NoAppxFilePanel.Visible = True
            AppxFilePanel.Visible = False
            AppxDetailsPanel.Height = 520
            FlowLayoutPanel1.Visible = False
        End Try
    End Sub

    Sub DetectMultiSelectionCommonProperties()
        NoAppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, True, False)
        AppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        AppxDetailsPanel.Height = If(ListView1.SelectedItems.Count <= 0, 520, 83)
        FlowLayoutPanel1.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label7.Text = "Multiple selection"
                        Label8.Text = "View the common properties of all selected applications"
                    Case "ESN"
                        Label7.Text = "Selección múltiple"
                        Label8.Text = "Vea las propiedades comunes de todas las aplicaciones seleccionadas"
                    Case "FRA"
                        Label7.Text = "Sélection multiple"
                        Label8.Text = "Voir les propriétés communes de toutes les applications sélectionnées"
                    Case "PTB", "PTG"
                        Label7.Text = "Seleção múltipla"
                        Label8.Text = "Ver as propriedades comuns de todas as aplicações seleccionadas"
                    Case "ITA"
                        Label7.Text = "Selezione multiple"
                        Label8.Text = "Visualizza le proprietà comuni di tutte le applicazioni selezionate"
                End Select
            Case 1
                Label7.Text = "Multiple selection"
                Label8.Text = "View the common properties of all selected applications"
            Case 2
                Label7.Text = "Selección múltiple"
                Label8.Text = "Vea las propiedades comunes de todas las aplicaciones seleccionadas"
            Case 3
                Label7.Text = "Sélection multiple"
                Label8.Text = "Voir les propriétés communes de toutes les applications sélectionnées"
            Case 4
                Label7.Text = "Seleção múltipla"
                Label8.Text = "Ver as propriedades comuns de todas as aplicações seleccionadas"
            Case 5
                Label7.Text = "Selezione multiple"
                Label8.Text = "Visualizza le proprietà comuni di tutte le applicazioni selezionate"
        End Select
        Label9.Visible = False
        PictureBox2.Visible = False
        ListBox1.Items.Clear()
        TextBox1.Text = ""
        ' Detect common properties. Use the Elements for that
        Dim depLists As New List(Of AppxPackage)
        Dim commonDeps As New List(Of String)
        Dim liFileLists As New List(Of AppxPackage)
        Dim commonLicense As String = ""
        Dim cdFileLists As New List(Of AppxPackage)
        Dim commonCustomDataFile As String = ""
        Dim regionLists As New List(Of AppxPackage)
        Dim commonRegions As String = ""
        For Each SelectedItem As ListViewItem In ListView1.SelectedItems
            ' Detect dependencies
            depLists.Add(Packages(ListView1.Items.IndexOf(SelectedItem)))
            ' Detect license files
            liFileLists.Add(Packages(ListView1.Items.IndexOf(SelectedItem)))
            ' Detect custom data files
            cdFileLists.Add(Packages(ListView1.Items.IndexOf(SelectedItem)))
            ' Detect regions
            regionLists.Add(Packages(ListView1.Items.IndexOf(SelectedItem)))
        Next
        If depLists.Count > 0 Then
            For Each dependency As AppxPackage In depLists
                Dim depList As New List(Of AppxDependency)
                depList = dependency.PackageSpecifiedDependencies
                Dim depFileList As New List(Of String)
                For Each dep As AppxDependency In depList
                    depFileList.Add(dep.DependencyFile)
                Next
                If commonDeps.Count = 0 Then commonDeps = depFileList
                commonDeps = commonDeps.Intersect(depFileList).ToList()
            Next
        End If
        If commonDeps.Count > 0 Then
            For Each commonDep In commonDeps
                ListBox1.Items.Add(commonDep)
            Next
        End If
        If liFileLists.Count > 0 Then
            Dim liFileList As New List(Of String)
            For Each LicenseFileInPkg As AppxPackage In liFileLists
                liFileList.Add(LicenseFileInPkg.PackageLicenseFile)
            Next
            If liFileList.Count > 0 Then
                commonLicense = liFileList(0)
                Dim singleLiFile As String = liFileList(0)
                ' If a license file is repeated every time, it's our common one
                For Each licenseFile In liFileList
                    If Not licenseFile = singleLiFile Then
                        singleLiFile = licenseFile
                        commonLicense = ""
                    End If
                Next
            End If
        End If
        If commonLicense <> "" And File.Exists(commonLicense) Then
            CheckBox3.Checked = True
            TextBox1.Text = commonLicense
        End If
        If cdFileLists.Count > 0 Then
            Dim cdFileList As New List(Of String)
            For Each CustomDataFile As AppxPackage In cdFileLists
                cdFileList.Add(CustomDataFile.PackageCustomDataFile)
            Next
            If cdFileList.Count > 0 Then
                commonCustomDataFile = cdFileList(0)
                Dim singleCdFile As String = cdFileList(0)
                ' If a custom data file is repeated every time, it's our common one
                For Each customDataFile In cdFileList
                    If Not customDataFile = singleCdFile Then
                        singleCdFile = customDataFile
                        commonCustomDataFile = ""
                    End If
                Next
            End If
        End If
        If commonCustomDataFile <> "" And File.Exists(commonCustomDataFile) Then
            CheckBox1.Checked = True
            TextBox2.Text = commonCustomDataFile
        End If
        If regionLists.Count > 0 Then
            Dim regionList As New List(Of String)
            For Each regionString As AppxPackage In regionLists
                regionList.Add(regionString.PackageRegions)
            Next
            If regionList.Count > 0 Then
                commonRegions = regionList(0)
                Dim singleRegion As String = regionList(0)
                ' If a custom data file is repeated every time, it's our common one
                For Each regionStr In regionList
                    If Not regionStr = singleRegion Then
                        singleRegion = regionStr
                        commonRegions = ""
                    End If
                Next
            End If
        End If
        If commonRegions <> "" Then
            CheckBox4.Checked = True
            TextBox3.Text = commonRegions
        End If

        ' Disable manipulation controls, as editing is not implemented yet
        TableLayoutPanel3.Enabled = False
        ListBox1.Enabled = False
        CheckBox3.Enabled = False
        TextBox1.Enabled = False
        CheckBox1.Enabled = False
        TextBox2.Enabled = False
        CheckBox4.Enabled = False
        TextBox3.Enabled = False
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedItem = "" Then
            Button5.Enabled = False
        Else
            Button5.Enabled = True
        End If
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0 Then Exit Sub
        HidePopupForm()
        With LogoAssetPopupForm
            .BackColor = BackColor
            .ForeColor = ForeColor
            .ShowIcon = False
            .ShowInTaskbar = False
            .ControlBox = False
            .FormBorderStyle = Windows.Forms.FormBorderStyle.None
            .Size = New Size(152, 152)
            Dim ctrlLoc As Point = PictureBox2.PointToScreen(Point.Empty)
            .StartPosition = FormStartPosition.Manual
            .Location = ctrlLoc
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            .Text = "Preview"
                        Case "ESN"
                            .Text = "Vista previa"
                        Case "FRA"
                            .Text = "Aperçu"
                        Case "PTB", "PTG"
                            .Text = "Pré-visualização"
                        Case "ITA"
                            .Text = "Anteprima"
                    End Select
                Case 1
                    .Text = "Preview"
                Case 2
                    .Text = "Vista previa"
                Case 3
                    .Text = "Aperçu"
                Case 4
                    .Text = "Pré-visualização"
                Case 5
                    .Text = "Anteprima"
            End Select
            With LogoAssetPreview
                .Parent = LogoAssetPopupForm
                .Dock = DockStyle.Fill
                .SizeMode = PictureBoxSizeMode.Zoom
                Try
                    If Directory.Exists(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text) And My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count > 0 Then
                        Dim asset As Image = Nothing
                        For Each StoreAsset In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text)
                            If Path.GetExtension(StoreAsset).EndsWith("png") Then
                                asset = Image.FromFile(StoreAsset)
                                If asset.Width / asset.Height = 1 Then      ' Determine if the image's aspect ratio is 1:1
                                    If asset.Width > 100 And asset.Width <= 200 And asset.Height > 100 And asset.Height <= 200 Then      ' Determine if it is a "large" asset
                                        .Image = asset
                                        Exit For
                                    Else
                                        .SizeMode = PictureBoxSizeMode.CenterImage
                                        .Image = PictureBox2.Image
                                    End If
                                End If
                            End If
                        Next
                    End If
                Catch ex As Exception

                End Try
            End With
            .Controls.Add(LogoAssetPreview)
            AddHandler LogoAssetPreview.Click, AddressOf HidePopupForm
            .Show()
            .BringToFront()
        End With
    End Sub

    Sub HidePopupForm() Handles MyBase.FormClosing, MyBase.VisibleChanged
        LogoAssetPopupForm.Hide()
    End Sub

    Private Sub PictureBox2_MouseHover(sender As Object, e As EventArgs) Handles PictureBox2.MouseHover
        Try
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "The logo assets for this file could not be detected", "Click here to enlarge the view"))
                        Case "ESN"
                            previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "Los recursos de este archivo no pudieron ser detectados", "Haga clic para agrandar la vista"))
                        Case "FRA"
                            previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "Le logo de ce fichier n'a pas pu être détecté.", "Cliquez ici pour agrandir la vue"))
                        Case "PTB", "PTG"
                            previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "Não foi possível detetar os activos do logótipo para este ficheiro", "Clique aqui para ampliar a vista"))
                        Case "ITA"
                            previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "Non è stato possibile rilevare le risorse del logo per questo file", "Fare clic qui per ingrandire la visualizzazione"))
                    End Select
                Case 1
                    previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "The logo assets for this file could not be detected", "Click here to enlarge the view"))
                Case 2
                    previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "Los recursos de este archivo no pudieron ser detectados", "Haga clic para agrandar la vista"))
                Case 3
                    previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "Le logo de ce fichier n'a pas pu être détecté.", "Cliquez ici pour agrandir la vue"))
                Case 4
                    previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "Não foi possível detetar os activos do logótipo para este ficheiro", "Clique aqui para ampliar a vista"))
                Case 5
                    previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "Non è stato possibile rilevare le risorse del logo per questo file", "Fare clic qui per ingrandire la visualizzazione"))
            End Select
        Catch ex As Exception
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            previewer.SetToolTip(sender, "The logo assets for this file could not be detected")
                        Case "ESN"
                            previewer.SetToolTip(sender, "Los recursos de este archivo no pudieron ser detectados")
                        Case "FRA"
                            previewer.SetToolTip(sender, "Le logo de ce fichier n'a pas pu être détecté.")
                        Case "PTB", "PTG"
                            previewer.SetToolTip(sender, "Não foi possível detetar os activos do logótipo para este ficheiro")
                        Case "ITA"
                            previewer.SetToolTip(sender, "Non è stato possibile rilevare le risorse del logo per questo file")
                    End Select
                Case 1
                    previewer.SetToolTip(sender, "The logo assets for this file could not be detected")
                Case 2
                    previewer.SetToolTip(sender, "Los recursos de este archivo no pudieron ser detectados")
                Case 3
                    previewer.SetToolTip(sender, "Le logo de ce fichier n'a pas pu être détecté.")
                Case 4
                    previewer.SetToolTip(sender, "Não foi possível detetar os activos do logótipo para este ficheiro")
                Case 5
                    previewer.SetToolTip(sender, "Non è stato possibile rilevare le risorse del logo per questo file")
            End Select
        End Try
    End Sub

    Private Sub ListView1_DragEnter(sender As Object, e As DragEventArgs) Handles ListView1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Function GetDownloadedPackageExtensionFromAppInstaller(PkgFile As String) As String
        Dim appExtensions() As String = New String(7) {".appx", ".appxbundle", ".msix", ".msixbundle", ".eappx", ".eappxbundle", ".emsix", ".emsixbundle"}
        Try
            For Each appExtension In appExtensions
                Dim targetFile As String = PkgFile.Replace(Path.GetExtension(PkgFile), appExtension)
                If File.Exists(targetFile) Then Return appExtension
            Next
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    Private Sub ListView1_DragDrop(sender As Object, e As DragEventArgs) Handles ListView1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        Cursor = Cursors.WaitCursor
        For Each PackageFile In PackageFiles
            If Path.GetExtension(PackageFile).Equals(".appx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".msix", StringComparison.OrdinalIgnoreCase) Or
                Path.GetExtension(PackageFile).Equals(".appxbundle", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".msixbundle", StringComparison.OrdinalIgnoreCase) Or
                Path.GetExtension(PackageFile).Equals(".eappx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".emsix", StringComparison.OrdinalIgnoreCase) Or
                Path.GetExtension(PackageFile).Equals(".eappxbundle", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".emsixbundle", StringComparison.OrdinalIgnoreCase) Then
                ScanAppxPackage(False, PackageFile)
            ElseIf Path.GetExtension(PackageFile).Equals(".appinstaller", StringComparison.OrdinalIgnoreCase) Then
                If Not AppInstallerDownloader.IsDisposed Then AppInstallerDownloader.Dispose()
                AppInstallerDownloader.AppInstallerFile = PackageFile
                If Not File.Exists(PackageFile.Replace(".appinstaller", GetDownloadedPackageExtensionFromAppInstaller(PackageFile))) Then AppInstallerDownloader.ShowDialog(Me)
                If File.Exists(PackageFile.Replace(".appinstaller", GetDownloadedPackageExtensionFromAppInstaller(PackageFile)).Trim()) Then ScanAppxPackage(False, PackageFile.Replace(".appinstaller", GetDownloadedPackageExtensionFromAppInstaller(PackageFile)).Trim())
            ElseIf File.GetAttributes(PackageFile) = (FileAttributes.Directory Or FileAttributes.Archive) Then
                Dim msg As String = ""
                ' Temporary support for directories
                If File.Exists(PackageFile & "\AppxSignature.p7x") And File.Exists(PackageFile & "\AppxMetadata\AppxBundleManifest.xml") Or File.Exists(PackageFile & "\AppxManifest.xml") Then
                    ScanAppxPackage(True, PackageFile)
                ElseIf My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.appx").Count > 0 Or My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.msix").Count > 0 Or
                    My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.appxbundle").Count > 0 Or My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.msixbundle").Count > 0 Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "The following directory:" & CrLf & Quote & PackageFile & Quote & CrLf & "contains application packages. Do you want to process them as well?" & CrLf & CrLf & "NOTE: this will scan this directory recursively, so it may take longer for this operation to complete"
                                Case "ESN"
                                    msg = "El siguiente directorio:" & CrLf & Quote & PackageFile & Quote & CrLf & "contiene paquetes de aplicación. ¿Desea procesarlos también?" & CrLf & CrLf & "NOTA: esto escaneará este directorio de una forma recursiva, así que esta operación podría tardar más tiempo en completar"
                                Case "FRA"
                                    msg = "Le répertoire suivant :" & CrLf & Quote & PackageFile & Quote & CrLf & "contient des paquets d'application. Voulez-vous les traiter également ?" & CrLf & CrLf & "REMARQUE : l'analyse de ce répertoire se fera de manière récursive, ce qui peut prolonger la durée de l'opération."
                                Case "PTB", "PTG"
                                    msg = "O seguinte diretório:" & CrLf & Quote & PackageFile & Quote & CrLf & "contém pacotes de aplicações. Deseja processá-los também?" & CrLf & CrLf & "NOTA: esta operação irá analisar este diretório recursivamente, pelo que poderá demorar mais tempo a ser concluída"
                                Case "ITA"
                                    msg = "La seguente cartella:" & CrLf & Quote & PackageFile & Quote & CrLf & "contiene pacchetti di applicazioni. Si desidera elaborare anche questi?" & CrLf & CrLf & "NOTA: la scansione di questa cartella avverrà in modo ricorsivo, pertanto il completamento dell'operazione potrebbe richiedere più tempo"
                            End Select
                        Case 1
                            msg = "The following directory:" & CrLf & Quote & PackageFile & Quote & CrLf & "contains application packages. Do you want to process them as well?" & CrLf & CrLf & "NOTE: this will scan this directory recursively, so it may take longer for this operation to complete"
                        Case 2
                            msg = "El siguiente directorio:" & CrLf & Quote & PackageFile & Quote & CrLf & "contiene paquetes de aplicación. ¿Desea procesarlos también?" & CrLf & CrLf & "NOTA: esto escaneará este directorio de una forma recursiva, así que esta operación podría tardar más tiempo en completar"
                        Case 3
                            msg = "Le répertoire suivant :" & CrLf & Quote & PackageFile & Quote & CrLf & "contient des paquets d'application. Voulez-vous les traiter également ?" & CrLf & CrLf & "REMARQUE : l'analyse de ce répertoire se fera de manière récursive, ce qui peut prolonger la durée de l'opération."
                        Case 4
                            msg = "O seguinte diretório:" & CrLf & Quote & PackageFile & Quote & CrLf & "contém pacotes de aplicações. Deseja processá-los também?" & CrLf & CrLf & "NOTA: esta operação irá analisar este diretório recursivamente, pelo que poderá demorar mais tempo a ser concluída"
                        Case 5
                            msg = "La seguente cartella:" & CrLf & Quote & PackageFile & Quote & CrLf & "contiene pacchetti di applicazioni. Si desidera elaborare anche questi?" & CrLf & CrLf & "NOTA: la scansione di questa cartella avverrà in modo ricorsivo, pertanto il completamento dell'operazione potrebbe richiedere più tempo"
                    End Select
                    If MsgBox(msg, vbYesNo + vbQuestion, Label1.Text) = MsgBoxResult.Yes Then
                        For Each AppPkg In My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchAllSubDirectories)
                            If Path.GetExtension(AppPkg).Equals(".appx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".appxbundle", StringComparison.OrdinalIgnoreCase) Or
                                Path.GetExtension(AppPkg).Equals(".msix", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".msixbundle", StringComparison.OrdinalIgnoreCase) Then
                                ScanAppxPackage(False, AppPkg)
                            ElseIf Path.GetExtension(AppPkg).Equals(".appinstaller", StringComparison.OrdinalIgnoreCase) Then
                                If Not AppInstallerDownloader.IsDisposed Then AppInstallerDownloader.Dispose()
                                AppInstallerDownloader.AppInstallerFile = AppPkg
                                If Not File.Exists(AppPkg.Replace(".appinstaller", GetDownloadedPackageExtensionFromAppInstaller(AppPkg))) Then AppInstallerDownloader.ShowDialog(Me)
                                If File.Exists(AppPkg.Replace(".appinstaller", GetDownloadedPackageExtensionFromAppInstaller(AppPkg))) Then ScanAppxPackage(False, AppPkg.Replace(".appinstaller", GetDownloadedPackageExtensionFromAppInstaller(AppPkg)))
                            Else
                                Continue For
                            End If
                        Next
                    Else
                        Continue For
                    End If
                End If
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MsgBox("The file that has been dropped here isn't an application package.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                            Case "ESN"
                                MsgBox("El archivo que se ha soltado aquí no es un paquete de aplicación.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                            Case "FRA"
                                MsgBox("Le fichier qui a été déposé ici n'est pas un paquet d'application.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                            Case "PTB", "PTG"
                                MsgBox("O ficheiro que foi deixado aqui não é um pacote de aplicações.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                            Case "ITA"
                                MsgBox("Il file che è stato scaricato qui non è un pacchetto dell'applicazione", vbOKOnly + vbCritical, "Aggiungere i pacchetti AppX approvvigionati")
                        End Select
                    Case 1
                        MsgBox("The file that has been dropped here isn't an application package.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                    Case 2
                        MsgBox("El archivo que se ha soltado aquí no es un paquete de aplicación.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                    Case 3
                        MsgBox("Le fichier qui a été déposé ici n'est pas un paquet d'application.", vbOKOnly + vbCritical, "Ajouter des paquets AppX provisionnés")
                    Case 4
                        MsgBox("O ficheiro que foi deixado aqui não é um pacote de aplicações.", vbOKOnly + vbCritical, "Adicionar pacotes AppX provisionados")
                    Case 5
                        MsgBox("Il file che è stato scaricato qui non è un pacchetto dell'applicazione", vbOKOnly + vbCritical, "Aggiungere i pacchetti AppX approvvigionati")
                End Select
            End If
        Next
        Cursor = Cursors.Arrow
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        LicenseFileOFD.ShowDialog()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        CustomDataFileOFD.ShowDialog()
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop
        If ListView1.SelectedItems.Count < 1 Then Exit Sub
        Dim DependencyFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each Dependency In DependencyFiles
            If Not ListBox1.Items.Contains(Dependency) And (Path.GetExtension(Dependency).EndsWith("appx", StringComparison.OrdinalIgnoreCase) Or _
                                                            Path.GetExtension(Dependency).EndsWith("msix", StringComparison.OrdinalIgnoreCase) Or _
                                                            Path.GetExtension(Dependency).EndsWith("appxbundle", StringComparison.OrdinalIgnoreCase) Or _
                                                            Path.GetExtension(Dependency).EndsWith("msixbundle", StringComparison.OrdinalIgnoreCase)) Then
                Dim dep As New AppxDependency()
                dep.DependencyFile = Dependency
                If Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count > 0 And Not Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Contains(dep) Then
                    Dim deps As New List(Of AppxDependency)
                    deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                    deps.Add(dep)
                    Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
                ElseIf Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count = 0 Then
                    Dim deps As New List(Of AppxDependency)
                    deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                    deps.Add(dep)
                    Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
                End If
                ListBox1.Items.Add(Dependency)
            End If
        Next
        If ListBox1.Items.Count > 0 Then Button4.Enabled = True
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If ListView1.SelectedItems.Count = 1 Then Packages(ListView1.FocusedItem.Index).PackageLicenseFile = TextBox1.Text
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If ListView1.SelectedItems.Count = 1 Then Packages(ListView1.FocusedItem.Index).PackageCustomDataFile = TextBox2.Text
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        MainForm.AppxRelatedLinksCMS.Show(sender, New Point(8, 8))
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 0 Then
            If ListView1.SelectedItems.Count = 1 Then Packages(ListView1.FocusedItem.Index).StubPackageOption = StubPreference.NoPreference
        ElseIf ComboBox1.SelectedIndex = 1 Then
            If ListView1.SelectedItems.Count = 1 Then Packages(ListView1.FocusedItem.Index).StubPackageOption = StubPreference.StubOnly
        ElseIf ComboBox1.SelectedIndex = 2 Then
            If ListView1.SelectedItems.Count = 1 Then Packages(ListView1.FocusedItem.Index).StubPackageOption = StubPreference.FullPackage
        End If
    End Sub
End Class
