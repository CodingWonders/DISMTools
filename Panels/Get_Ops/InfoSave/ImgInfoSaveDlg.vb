Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Dism
Imports System.Threading
Imports DISMTools.Utilities
Imports Microsoft.Win32

Public Class ImgInfoSaveDlg

    ' Like ProgressPanel, this dialog is task-based. This integer represents the task that will be run. It can be:
    ' - 0, to save every information possible (image, packages, features, and so on)
    ' - 1, to save image information (only in offline image mode)
    ' - 2, to save installed package information
    ' - 3, to save information of the package files specified
    ' - 4, to save feature information
    ' - 5, to save installed AppX package information
    ' - 6, to save capability information
    ' - 7, to save installed driver information
    '   Do note that, if background processes have been configured to not detect all drivers, this dialog will ask you
    ' - 8, to save information of the driver files specified
    ' - 9, to save Windows PE configuration (only for WinPE images)
    Public SaveTask As Integer

    ' The source image to get the information from
    Public SourceImage As String

    Public ImgMountDir As String

    Public OnlineMode As Boolean
    Public OfflineMode As Boolean

    Public AllDrivers As Boolean

    ' The file to save the information to
    Public SaveTarget As String

    ' The contents the target file will have
    Public Contents As String

    ' List of package files
    Public PackageFiles As New List(Of String)

    ' List of driver packages
    Public DriverPkgs As New List(Of String)

    Public SkipQuestions As Boolean
    Public AutoCompleteInfo(4) As Boolean

    Public ForceAppxApi As Boolean

    Const TableSeparator As String = "|"
    Const CodeBlockChar As String = " ` "       ' It is " ` " to prevent Markdig problem "Markdown elements in the input are too deeply nested - depth limit exceeded. Input is most likely not sensible or is a very large table."

    ' Table Headers
    Const ImageFileTableHeader As String = "| Version | Image name | Image description | Image size | Architecture | HAL | Service Pack build | Service Pack level | Installation type | Edition | Product type | Product suite | System root directory | Languages | Date of creation | Date of modification |" & CrLf & _
                                           "|:--------|:-----------|:------------------|:-----------|:-------------|:----|:-------------------|:-------------------|:------------------|:--------|:-------------|:--------------|:----------------------|:----------|:-----------------|:---------------------|" & CrLf
    Const FullPackageTableHeader As String = "| Package name | Applicable? | Copyright | Company | Creation time | Description | Install client | Install package name | Install time | Last update time | Display name | Product name | Product version | Release type | Restart required? | Support information | Package state | Boot up required? | Capability identity | Custom properties | Features |" & CrLf & _
                                             "|:-------------|:------------|:----------|:--------|:--------------|:------------|:---------------|:---------------------|:-------------|:-----------------|:-------------|:-------------|:----------------|:-------------|:------------------|:--------------------|:--------------|:------------------|:--------------------|:------------------|:---------|" & CrLf
    Const BasicPackageTableHeader As String = "| Package name | Package state | Package release type | Package install time |" & CrLf & _
                                              "|:-------------|:--------------|:---------------------|:---------------------|" & CrLf
    Const FullFeatureTableHeader As String = "| Feature name | Display name | Description | Restart required? | Feature state | Custom properties |" & CrLf & _
                                             "|:-------------|:-------------|:------------|:------------------|:--------------|:------------------|" & CrLf
    Const BasicFeatureTableHeader As String = "| Feature name | Feature state |" & CrLf & _
                                              "|:-------------|:--------------|" & CrLf
    Const FullAppxTableHeader As String = "| Package name | Application display name | Architecture | Resource ID | Version | Registered to a user? | Installation location | Package manifest location | Store logo asset directory | Main store logo asset |" & CrLf & _
                                          "|:-------------|:-------------------------|:-------------|:------------|:--------|:----------------------|:----------------------|:--------------------------|:---------------------------|:----------------------|" & CrLf
    Const BasicAppxTableHeader As String = "| Package name | Application display name | Architecture | Resource ID | Version |" & CrLf & _
                                           "|:-------------|:-------------------------|:-------------|:------------|:--------|" & CrLf
    Const FullCapabilityTableHeader As String = "| Capability identity | Capability name | Capability state | Display name | Download size | Installation size |" & CrLf & _
                                                "|:--------------------|:----------------|:-----------------|:-------------|:--------------|:------------------|" & CrLf
    Const BasicCapabilityTableHeader As String = "| Capability name | Capability state |" & CrLf & _
                                                 "|:----------------|:-----------------|" & CrLf
    Const FullDriverTableHeader As String = "| Published name | Original file name | Provider name | Class name | Class description | Class GUID | Catalog file path | Part of the Windows distribution? | Critical to the boot process? | Version | Date | Signature status |" & CrLf & _
                                            "|:---------------|:-------------------|:--------------|:-----------|:------------------|:-----------|:------------------|:----------------------------------|:------------------------------|:--------|:-----|:-----------------|" & CrLf
    Const BasicDriverTableHeader As String = "| Published name | Original file name | Part of the Windows distribution? | Class name | Provider name | Date | Version |" & CrLf & _
                                             "|:---------------|:-------------------|:----------------------------------|:-----------|:--------------|:-----|:--------|" & CrLf
    Const DriverFileTableHeader As String = "| Hardware description | Hardware IDs | Compatible IDs | Exclude IDs | Hardware manufacturer | Architecture |" & CrLf & _
                                            "|:---------------------|:-------------|:---------------|:------------|:----------------------|:-------------|" & CrLf

    Sub ReportChanges(Message As String, ProgressPercentage As Double)
        Label2.Text = Message
        ProgressBar1.Value = ProgressPercentage
        TaskbarHelper.SetIndicatorState(ProgressPercentage, Windows.Shell.TaskbarItemProgressState.Normal, MainForm.Handle)
        Application.DoEvents()
    End Sub

    Sub GetImageInformation()
        Dim ImageInfoCollection As DismImageInfoCollection = Nothing
        Dim ImageInfoList As New List(Of DismImageInfo)
        If ImageInfoList.Count <> 0 Then ImageInfoList.Clear()
        Contents &= "## Image information" & CrLf & CrLf
        If OnlineMode Then
            Contents &= "### Active installation information:" & CrLf & CrLf & _
                        "- Name: " & My.Computer.Info.OSFullName & CrLf & _
                        "- Boot point (mount point): " & Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & CrLf & _
                        "- Version: " & Environment.OSVersion.Version.Major & "." & Environment.OSVersion.Version.Minor & "." & Environment.OSVersion.Version.Build & "." & FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe").ProductPrivatePart & CrLf & CrLf
            Exit Sub
        ElseIf OfflineMode Then
            Contents &= "### Offline installation information:" & CrLf & CrLf & _
                        "- Boot point (mount point): " & ImgMountDir & CrLf & _
                        "- Version: " & FileVersionInfo.GetVersionInfo(ImgMountDir & "\Windows\system32\ntoskrnl.exe").ProductVersion.ToString() & CrLf & CrLf
            Exit Sub
        End If
        Contents &= "- Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "")
        Debug.WriteLine("[GetImageInformation] Starting task...")
        Try
            Debug.WriteLine("[GetImageInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetImageInformation] Populating info collection...")
            ImageInfoCollection = DismApi.GetImageInfo(SourceImage)
            Debug.WriteLine("[GetImageInformation] Information processes completed for the image. Obtained images: " & ImageInfoCollection.Count)
            Contents &= CrLf & CrLf & _
                        "**Information summary for " & ImageInfoCollection.Count & " image(s):**" & CrLf & CrLf & ImageFileTableHeader
            Debug.WriteLine("[GetImageInformation] Exporting information to contents...")
            For Each ImageInfo As DismImageInfo In ImageInfoCollection
                Dim msg As String = ""
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg = "Getting image information... (image " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " of " & ImageInfoCollection.Count & ")"
                            Case "ESN"
                                msg = "Obteniendo información de la imagen... (imagen " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " de " & ImageInfoCollection.Count & ")"
                            Case "FRA"
                                msg = "Obtention des informations sur l'image en cours... (image " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " de " & ImageInfoCollection.Count & ")"
                            Case "PTB", "PTG"
                                msg = "Obter informações sobre a imagem... (imagem " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " de " & ImageInfoCollection.Count & ")"
                            Case "ITA"
                                msg = "Ottenere informazioni sull'immagine... (immagine " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " di " & ImageInfoCollection.Count & ")"
                        End Select
                    Case 1
                        msg = "Getting image information... (image " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " of " & ImageInfoCollection.Count & ")"
                    Case 2
                        msg = "Obteniendo información de la imagen... (imagen " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " de " & ImageInfoCollection.Count & ")"
                    Case 3
                        msg = "Obtention des informations sur l'image en cours... (image " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " de " & ImageInfoCollection.Count & ")"
                    Case 4
                        msg = "Obter informações sobre a imagem... (imagem " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " de " & ImageInfoCollection.Count & ")"
                    Case 5
                        msg = "Ottenere informazioni sull'immagine... (immagine " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " di " & ImageInfoCollection.Count & ")"
                End Select
                Dim languages As String = "<ul>"
                For Each language In ImageInfo.Languages
                    languages &= "<li>" & language.DisplayName & If(ImageInfo.DefaultLanguage.Name = language.Name, " (default)", "") & "</li>"
                Next
                languages &= "</ul>"
                ReportChanges(msg, (ImageInfoCollection.IndexOf(ImageInfo) / ImageInfoCollection.Count) * 100)
                Contents &= TableSeparator & ImageInfo.ProductVersion.ToString() & TableSeparator & ImageInfo.ImageName & TableSeparator & ImageInfo.ImageDescription & TableSeparator & ImageInfo.ImageSize.ToString("N0") & " bytes (~" & Converters.BytesToReadableSize(ImageInfo.ImageSize) & ")" & TableSeparator & Casters.CastDismArchitecture(ImageInfo.Architecture) & TableSeparator & If(ImageInfo.Hal <> "", ImageInfo.Hal, "Undefined by the image") & TableSeparator & ImageInfo.ProductVersion.Revision & TableSeparator & ImageInfo.SpLevel & TableSeparator & ImageInfo.InstallationType & TableSeparator & ImageInfo.EditionId & TableSeparator & ImageInfo.ProductType & TableSeparator & ImageInfo.ProductSuite & TableSeparator & ImageInfo.SystemRoot & TableSeparator & languages & TableSeparator & ImageInfo.CustomizedInfo.CreatedTime & TableSeparator & ImageInfo.CustomizedInfo.ModifiedTime & TableSeparator & CrLf
            Next
        Catch ex As Exception
            Debug.WriteLine("[GetImageInformation] An error occurred while getting image information: " & ex.ToString() & " - " & ex.Message)
            Contents &= CrLf & "The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                        "- Exception: " & ex.ToString() & CrLf & _
                        "- Exception message: " & ex.Message & CrLf & _
                        "- Error code: " & Hex(ex.HResult) & CrLf & CrLf
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Sub GetPackageInformation()
        Dim InstalledPkgInfo As DismPackageCollection = Nothing
        Dim msg As String() = New String(2) {"", "", ""}
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg(0) = "Preparing package information processes..."
                        msg(1) = "The program has obtained basic information of the installed packages of this image. You can also get complete information of such packages and save it in the report." & CrLf & CrLf & _
                          "Do note that this will take longer depending on the number of installed packages." & CrLf & CrLf & _
                          "Do you want to get this information and save it in the report?"
                        msg(2) = "Package information"
                    Case "ESN"
                        msg(0) = "Preparando procesos de información de paquetes..."
                        msg(1) = "El programa ha obtenido información básica de los paquetes instalados en esta imagen. También puede obtener información completa de dichos paquetes y guardarla en el informe." & CrLf & CrLf & _
                          "Dese cuenta de que esto tardará más, dependiendo del número de paquetes instalados." & CrLf & CrLf & _
                          "¿Desea obtener esta información y guardarla en el informe?"
                        msg(2) = "Información de paquetes"
                    Case "FRA"
                        msg(0) = "Préparation des processus d'information sur les paquets en cours..."
                        msg(1) = "Le programme a obtenu des informations basiques sur les paquets installés sur cette image. Vous pouvez également obtenir des informations complètes sur ces paquets et les enregistrer dans le rapport." & CrLf & CrLf & _
                          "Notez que cette opération peut prendre plus de temps en fonction du nombre de paquets installés." & CrLf & CrLf & _
                          "Souhaitez-vous obtenir ces informations et les enregistrer dans le rapport ?"
                        msg(2) = "Informations sur les paquets"
                    Case "PTB", "PTG"
                        msg(0) = "A preparar processos de informação de pacotes..."
                        msg(1) = "O programa obteve informações básicas sobre os pacotes instalados nesta imagem. Também pode obter informações completas sobre esses pacotes e guardá-las no relatório." & CrLf & CrLf & _
                          "Tem em atenção que isto pode demorar mais tempo, dependendo do número de pacotes instalados." & CrLf & CrLf & _
                          "Deseja obter esta informação e guardá-la no relatório?"
                        msg(2) = "Informações do pacote"
                    Case "ITA"
                        msg(0) = "Preparazione dei processi di informazione sui pacchetti..."
                        msg(1) = "Il programma ha ottenuto informazioni elementari sui pacchetti installati in questa immagine. È anche possibile ottenere informazioni complete su tali pacchetti e salvarle nel rapporto." & CrLf & CrLf & _
                          "Si noti che questa operazione richiederà più tempo a seconda del numero di pacchetti installati." & CrLf & CrLf & _
                          "Volete ottenere queste informazioni e salvarle nel rapporto?"
                        msg(2) = "Informazioni sul pacchetto"
                End Select
            Case 1
                msg(0) = "Preparing package information processes..."
                msg(1) = "The program has obtained basic information of the installed packages of this image. You can also get complete information of such packages and save it in the report." & CrLf & CrLf & _
                  "Do note that this will take longer depending on the number of installed packages." & CrLf & CrLf & _
                  "Do you want to get this information and save it in the report?"
                msg(2) = "Package information"
            Case 2
                msg(0) = "Preparando procesos de información de paquetes..."
                msg(1) = "El programa ha obtenido información básica de los paquetes instalados en esta imagen. También puede obtener información completa de dichos paquetes y guardarla en el informe." & CrLf & CrLf & _
                  "Dese cuenta de que esto tardará más, dependiendo del número de paquetes instalados." & CrLf & CrLf & _
                  "¿Desea obtener esta información y guardarla en el informe?"
                msg(2) = "Información de paquetes"
            Case 3
                msg(0) = "Préparation des processus d'information sur les paquets en cours..."
                msg(1) = "Le programme a obtenu des informations basiques sur les paquets installés sur cette image. Vous pouvez également obtenir des informations complètes sur ces paquets et les enregistrer dans le rapport." & CrLf & CrLf & _
                  "Notez que cette opération peut prendre plus de temps en fonction du nombre de paquets installés." & CrLf & CrLf & _
                  "Souhaitez-vous obtenir ces informations et les enregistrer dans le rapport ?"
                msg(2) = "Informations sur les paquets"
            Case 4
                msg(0) = "A preparar processos de informação de pacotes..."
                msg(1) = "O programa obteve informações básicas sobre os pacotes instalados nesta imagem. Também pode obter informações completas sobre esses pacotes e guardá-las no relatório." & CrLf & CrLf & _
                  "Tem em atenção que isto pode demorar mais tempo, dependendo do número de pacotes instalados." & CrLf & CrLf & _
                  "Deseja obter esta informação e guardá-la no relatório?"
                msg(2) = "Informações do pacote"
            Case 5
                msg(0) = "Preparazione dei processi di informazione sui pacchetti..."
                msg(1) = "Il programma ha ottenuto informazioni elementari sui pacchetti installati in questa immagine. È anche possibile ottenere informazioni complete su tali pacchetti e salvarle nel rapporto." & CrLf & CrLf & _
                  "Si noti che questa operazione richiederà più tempo a seconda del numero di pacchetti installati." & CrLf & CrLf & _
                  "Volete ottenere queste informazioni e salvarle nel rapporto?"
                msg(2) = "Informazioni sul pacchetto"
        End Select
        Contents &= "## Package information" & CrLf & CrLf & _
                    "- Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf & CrLf
        Debug.WriteLine("[GetPackageInformation] Starting task...")
        Try
            Debug.WriteLine("[GetPackageInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetPackageInformation] Creating image session...")
            ReportChanges(msg(0), 0)
            Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                Debug.WriteLine("[GetPackageInformation] Getting basic package information...")
                ReportChanges(msg(0), 5)
                InstalledPkgInfo = DismApi.GetPackages(imgSession)
                Contents &= "**Information summary for " & InstalledPkgInfo.Count & " package(s):**" & CrLf & CrLf
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg(0) = "Packages have been obtained"
                            Case "ESN"
                                msg(0) = "Los paquetes han sido obtenidos"
                            Case "FRA"
                                msg(0) = "Des paquets ont été obtenus"
                            Case "PTB", "PTG"
                                msg(0) = "Os pacotes foram obtidos"
                            Case "ITA"
                                msg(0) = "I pacchetti sono stati acquisiti"
                        End Select
                    Case 1
                        msg(0) = "Packages have been obtained"
                    Case 2
                        msg(0) = "Los paquetes han sido obtenidos"
                    Case 3
                        msg(0) = "Des paquets ont été obtenus"
                    Case 4
                        msg(0) = "Os pacotes foram obtidos"
                    Case 5
                        msg(0) = "I pacchetti sono stati acquisiti"
                End Select
                ReportChanges(msg(0), 10)
                Dim pkgCustomPropsList As String = "<ul>"
                Dim pkgFeaturesList As String = "<ul>"
                If SkipQuestions And AutoCompleteInfo(0) Then
                    Contents &= CrLf & CrLf & FullPackageTableHeader
                    Debug.WriteLine("[GetPackageInformation] Getting complete package information...")
                    For Each installedPackage As DismPackage In InstalledPkgInfo
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        msg(0) = "Getting information of packages... (package " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " of " & InstalledPkgInfo.Count & ")"
                                    Case "ESN"
                                        msg(0) = "Obteniendo información de paquetes... (paquete " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                                    Case "FRA"
                                        msg(0) = "Obtention des informations sur les paquets en cours... (paquet " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                                    Case "PTB", "PTG"
                                        msg(0) = "Obter informações sobre os pacotes... (pacote " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                                    Case "ITA"
                                        msg(0) = "Ottenere informazioni sui pacchetti... (pacchetto " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " di " & InstalledPkgInfo.Count & ")"
                                End Select
                            Case 1
                                msg(0) = "Getting information of packages... (package " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " of " & InstalledPkgInfo.Count & ")"
                            Case 2
                                msg(0) = "Obteniendo información de paquetes... (paquete " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                            Case 3
                                msg(0) = "Obtention des informations sur les paquets en cours... (paquet " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                            Case 4
                                msg(0) = "Obter informações sobre os pacotes... (pacote " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                            Case 5
                                msg(0) = "Ottenere informazioni sui pacchetti... (pacchetto " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " di " & InstalledPkgInfo.Count & ")"
                        End Select
                        ReportChanges(msg(0), (InstalledPkgInfo.IndexOf(installedPackage) / InstalledPkgInfo.Count) * 100)
                        Dim pkgInfoEx As DismPackageInfoEx = Nothing
                        Dim pkgInfo As DismPackageInfo = Nothing
                        Dim cProps As DismCustomPropertyCollection = Nothing

                        ' Determine Windows version, as capability identity information can't be obtained in Windows versions older than 10
                        If Environment.OSVersion.Version.Major >= 10 Then
                            pkgInfoEx = DismApi.GetPackageInfoExByName(imgSession, installedPackage.PackageName)
                        Else
                            pkgInfo = DismApi.GetPackageInfoByName(imgSession, installedPackage.PackageName)
                        End If
                        If pkgInfoEx IsNot Nothing Then
                            pkgCustomPropsList = "<ul>"
                            pkgFeaturesList = "<ul>"
                            cProps = pkgInfoEx.CustomProperties
                            If cProps.Count > 0 Then
                                For Each cProp As DismCustomProperty In cProps
                                    pkgCustomPropsList &= "<li>" & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value.Replace(CrLf, " ").Replace(Lf, " ").Replace(Cr, " ").Trim() & "</li>"
                                Next
                                pkgCustomPropsList &= "</ul>"
                            Else
                                pkgCustomPropsList = "None"
                            End If
                            If pkgInfoEx.Features.Count > 0 Then
                                Dim pkgFeats As DismFeatureCollection = pkgInfoEx.Features
                                For Each pkgFeat As DismFeature In pkgFeats
                                    pkgFeaturesList &= "<li>" & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & "</li>"
                                Next
                                pkgFeaturesList &= "</ul>"
                            Else
                                pkgFeaturesList = "None"
                            End If
                            Contents &= TableSeparator & CodeBlockChar & pkgInfoEx.PackageName & CodeBlockChar & TableSeparator & Casters.CastDismApplicabilityStatus(pkgInfoEx.Applicable) & TableSeparator & pkgInfoEx.Copyright & TableSeparator & pkgInfoEx.Company & TableSeparator & pkgInfoEx.CreationTime & If(pkgInfoEx.CreationTime.Year < 1900, " - **Preposterous time and date**", "") & TableSeparator & pkgInfoEx.Description & TableSeparator & pkgInfoEx.InstallClient & TableSeparator & CodeBlockChar & pkgInfoEx.InstallPackageName & CodeBlockChar & TableSeparator & pkgInfoEx.InstallTime & TableSeparator & pkgInfoEx.LastUpdateTime & If(pkgInfoEx.LastUpdateTime.Year < 1900, " - **Preposterous time and date**", "") & TableSeparator & pkgInfoEx.DisplayName & TableSeparator & pkgInfoEx.ProductName & TableSeparator & pkgInfoEx.ProductVersion.ToString() & TableSeparator & Casters.CastDismReleaseType(pkgInfoEx.ReleaseType) & TableSeparator & Casters.CastDismRestartType(pkgInfoEx.RestartRequired) & TableSeparator & pkgInfoEx.SupportInformation & TableSeparator & Casters.CastDismPackageState(pkgInfoEx.PackageState) & TableSeparator & Casters.CastDismFullyOfflineInstallationType(pkgInfoEx.FullyOffline) & TableSeparator & CodeBlockChar & pkgInfoEx.CapabilityId & CodeBlockChar & TableSeparator & pkgCustomPropsList & TableSeparator & pkgFeaturesList & TableSeparator & CrLf
                        ElseIf pkgInfo IsNot Nothing Then
                            pkgCustomPropsList = "<ul>"
                            pkgFeaturesList = "<ul>"
                            cProps = pkgInfo.CustomProperties
                            If cProps.Count > 0 Then
                                For Each cProp As DismCustomProperty In cProps
                                    pkgCustomPropsList &= "<li>" & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value.Replace(CrLf, " ").Replace(Lf, " ").Replace(Cr, " ").Trim() & "</li>"
                                Next
                                pkgCustomPropsList &= "</ul>"
                            Else
                                pkgCustomPropsList = "None"
                            End If
                            If pkgInfo.Features.Count > 0 Then
                                Dim pkgFeats As DismFeatureCollection = pkgInfo.Features
                                For Each pkgFeat As DismFeature In pkgFeats
                                    pkgFeaturesList &= "<li>" & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & "</li>"
                                Next
                                pkgFeaturesList &= "</ul>"
                            Else
                                pkgFeaturesList = "None"
                            End If
                            Contents &= TableSeparator & CodeBlockChar & pkgInfo.PackageName & CodeBlockChar & TableSeparator & Casters.CastDismApplicabilityStatus(pkgInfo.Applicable) & TableSeparator & pkgInfo.Copyright & TableSeparator & pkgInfo.Company & TableSeparator & pkgInfo.CreationTime & If(pkgInfo.CreationTime.Year < 1900, " - **Preposterous time and date**", "") & TableSeparator & pkgInfo.Description & TableSeparator & pkgInfo.InstallClient & TableSeparator & CodeBlockChar & pkgInfo.InstallPackageName & CodeBlockChar & TableSeparator & pkgInfo.InstallTime & TableSeparator & pkgInfo.LastUpdateTime & If(pkgInfo.LastUpdateTime.Year < 1900, " - **Preposterous time and date**", "") & TableSeparator & pkgInfo.DisplayName & TableSeparator & pkgInfo.ProductName & TableSeparator & pkgInfo.ProductVersion.ToString() & TableSeparator & Casters.CastDismReleaseType(pkgInfo.ReleaseType) & TableSeparator & Casters.CastDismRestartType(pkgInfo.RestartRequired) & TableSeparator & pkgInfo.SupportInformation & TableSeparator & Casters.CastDismPackageState(pkgInfo.PackageState) & TableSeparator & Casters.CastDismFullyOfflineInstallationType(pkgInfo.FullyOffline) & TableSeparator & "None" & TableSeparator & pkgCustomPropsList & TableSeparator & pkgFeaturesList & TableSeparator & CrLf
                        End If
                    Next
                    Contents &= CrLf & "- Complete package information has been gathered" & CrLf
                ElseIf (Not SkipQuestions Or Not AutoCompleteInfo(0)) And MsgBox(msg(1), vbYesNo + vbQuestion, msg(2)) = MsgBoxResult.Yes Then
                    Contents &= CrLf & CrLf & FullPackageTableHeader
                    Debug.WriteLine("[GetPackageInformation] Getting complete package information...")
                    For Each installedPackage As DismPackage In InstalledPkgInfo
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        msg(0) = "Getting information of packages... (package " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " of " & InstalledPkgInfo.Count & ")"
                                    Case "ESN"
                                        msg(0) = "Obteniendo información de paquetes... (paquete " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                                    Case "FRA"
                                        msg(0) = "Obtention des informations sur les paquets en cours... (paquet " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                                    Case "PTB", "PTG"
                                        msg(0) = "Obter informações sobre os pacotes... (pacote " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                                    Case "ITA"
                                        msg(0) = "Ottenere informazioni sui pacchetti... (pacchetto " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " di " & InstalledPkgInfo.Count & ")"
                                End Select
                            Case 1
                                msg(0) = "Getting information of packages... (package " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " of " & InstalledPkgInfo.Count & ")"
                            Case 2
                                msg(0) = "Obteniendo información de paquetes... (paquete " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                            Case 3
                                msg(0) = "Obtention des informations sur les paquets en cours... (paquet " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                            Case 4
                                msg(0) = "Obter informações sobre os pacotes... (pacote " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " de " & InstalledPkgInfo.Count & ")"
                            Case 5
                                msg(0) = "Ottenere informazioni sui pacchetti... (pacchetto " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " di " & InstalledPkgInfo.Count & ")"
                        End Select
                        ReportChanges(msg(0), (InstalledPkgInfo.IndexOf(installedPackage) / InstalledPkgInfo.Count) * 100)
                        Dim pkgInfoEx As DismPackageInfoEx = Nothing
                        Dim pkgInfo As DismPackageInfo = Nothing
                        Dim cProps As DismCustomPropertyCollection = Nothing

                        ' Determine Windows version, as capability identity information can't be obtained in Windows versions older than 10
                        If Environment.OSVersion.Version.Major >= 10 Then
                            pkgInfoEx = DismApi.GetPackageInfoExByName(imgSession, installedPackage.PackageName)
                        Else
                            pkgInfo = DismApi.GetPackageInfoByName(imgSession, installedPackage.PackageName)
                        End If
                        If pkgInfoEx IsNot Nothing Then
                            pkgCustomPropsList = "<ul>"
                            pkgFeaturesList = "<ul>"
                            cProps = pkgInfoEx.CustomProperties
                            If cProps.Count > 0 Then
                                For Each cProp As DismCustomProperty In cProps
                                    pkgCustomPropsList &= "<li>" & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value.Replace(CrLf, " ").Replace(Lf, " ").Replace(Cr, " ").Trim() & "</li>"
                                Next
                                pkgCustomPropsList &= "</ul>"
                            Else
                                pkgCustomPropsList = "None"
                            End If
                            If pkgInfoEx.Features.Count > 0 Then
                                Dim pkgFeats As DismFeatureCollection = pkgInfoEx.Features
                                For Each pkgFeat As DismFeature In pkgFeats
                                    pkgFeaturesList &= "<li>" & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & "</li>"
                                Next
                                pkgFeaturesList &= "</ul>"
                            Else
                                pkgFeaturesList = "None"
                            End If
                            Contents &= TableSeparator & CodeBlockChar & pkgInfoEx.PackageName & CodeBlockChar & TableSeparator & Casters.CastDismApplicabilityStatus(pkgInfoEx.Applicable) & TableSeparator & pkgInfoEx.Copyright & TableSeparator & pkgInfoEx.Company & TableSeparator & pkgInfoEx.CreationTime & If(pkgInfoEx.CreationTime.Year < 1900, " - **Preposterous time and date**", "") & TableSeparator & pkgInfoEx.Description & TableSeparator & pkgInfoEx.InstallClient & TableSeparator & CodeBlockChar & pkgInfoEx.InstallPackageName & CodeBlockChar & TableSeparator & pkgInfoEx.InstallTime & TableSeparator & pkgInfoEx.LastUpdateTime & If(pkgInfoEx.LastUpdateTime.Year < 1900, " - **Preposterous time and date**", "") & TableSeparator & pkgInfoEx.DisplayName & TableSeparator & pkgInfoEx.ProductName & TableSeparator & pkgInfoEx.ProductVersion.ToString() & TableSeparator & Casters.CastDismReleaseType(pkgInfoEx.ReleaseType) & TableSeparator & Casters.CastDismRestartType(pkgInfoEx.RestartRequired) & TableSeparator & pkgInfoEx.SupportInformation & TableSeparator & Casters.CastDismPackageState(pkgInfoEx.PackageState) & TableSeparator & Casters.CastDismFullyOfflineInstallationType(pkgInfoEx.FullyOffline) & TableSeparator & CodeBlockChar & pkgInfoEx.CapabilityId & CodeBlockChar & TableSeparator & pkgCustomPropsList & TableSeparator & pkgFeaturesList & TableSeparator & CrLf
                        ElseIf pkgInfo IsNot Nothing Then
                            pkgCustomPropsList = "<ul>"
                            pkgFeaturesList = "<ul>"
                            cProps = pkgInfo.CustomProperties
                            If cProps.Count > 0 Then
                                For Each cProp As DismCustomProperty In cProps
                                    pkgCustomPropsList &= "<li>" & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value.Replace(CrLf, " ").Replace(Lf, " ").Replace(Cr, " ").Trim() & "</li>"
                                Next
                                pkgCustomPropsList &= "</ul>"
                            Else
                                pkgCustomPropsList = "None"
                            End If
                            If pkgInfo.Features.Count > 0 Then
                                Dim pkgFeats As DismFeatureCollection = pkgInfo.Features
                                For Each pkgFeat As DismFeature In pkgFeats
                                    pkgFeaturesList &= "<li>" & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & "</li>"
                                Next
                                pkgFeaturesList &= "</ul>"
                            Else
                                pkgFeaturesList = "None"
                            End If
                            Contents &= TableSeparator & CodeBlockChar & pkgInfo.PackageName & CodeBlockChar & TableSeparator & Casters.CastDismApplicabilityStatus(pkgInfo.Applicable) & TableSeparator & pkgInfo.Copyright & TableSeparator & pkgInfo.Company & TableSeparator & pkgInfo.CreationTime & If(pkgInfo.CreationTime.Year < 1900, " - **Preposterous time and date**", "") & TableSeparator & pkgInfo.Description & TableSeparator & pkgInfo.InstallClient & TableSeparator & CodeBlockChar & pkgInfo.InstallPackageName & CodeBlockChar & TableSeparator & pkgInfo.InstallTime & TableSeparator & pkgInfo.LastUpdateTime & If(pkgInfo.LastUpdateTime.Year < 1900, " - **Preposterous time and date**", "") & TableSeparator & pkgInfo.DisplayName & TableSeparator & pkgInfo.ProductName & TableSeparator & pkgInfo.ProductVersion.ToString() & TableSeparator & Casters.CastDismReleaseType(pkgInfo.ReleaseType) & TableSeparator & Casters.CastDismRestartType(pkgInfo.RestartRequired) & TableSeparator & pkgInfo.SupportInformation & TableSeparator & Casters.CastDismPackageState(pkgInfo.PackageState) & TableSeparator & Casters.CastDismFullyOfflineInstallationType(pkgInfo.FullyOffline) & TableSeparator & "None" & TableSeparator & pkgCustomPropsList & TableSeparator & pkgFeaturesList & TableSeparator & CrLf
                        End If
                    Next
                    Contents &= CrLf & "- Complete package information has been gathered" & CrLf
                Else
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg(0) = "Saving installed packages..."
                                Case "ESN"
                                    msg(0) = "Guardando paquetes instalados..."
                                Case "FRA"
                                    msg(0) = "Sauvegarde des paquets installés en cours..."
                                Case "PTB", "PTG"
                                    msg(0) = "Guardar os pacotes instalados..."
                                Case "ITA"
                                    msg(0) = "Salvataggio dei pacchetti installati..."
                            End Select
                        Case 1
                            msg(0) = "Saving installed packages..."
                        Case 2
                            msg(0) = "Guardando paquetes instalados..."
                        Case 3
                            msg(0) = "Sauvegarde des paquets installés en cours..."
                        Case 4
                            msg(0) = "Guardar os pacotes instalados..."
                        Case 5
                            msg(0) = "Salvataggio dei pacchetti installati..."
                    End Select
                    ReportChanges(msg(0), 50)
                    Contents &= BasicPackageTableHeader
                    For Each installedPackage As DismPackage In InstalledPkgInfo
                        Contents &= TableSeparator & CodeBlockChar & installedPackage.PackageName & CodeBlockChar & TableSeparator & Casters.CastDismPackageState(installedPackage.PackageState) & TableSeparator & Casters.CastDismReleaseType(installedPackage.ReleaseType) & TableSeparator & installedPackage.InstallTime & TableSeparator & CrLf
                    Next
                    Contents &= CrLf & "- Complete package information has not been gathered" & CrLf
                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetPackageInformation] An error occurred while getting package information: " & ex.ToString() & " - " & ex.Message)
            Contents &= "The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                        "- Exception: " & ex.ToString() & CrLf & _
                        "- Exception message: " & ex.Message & CrLf & _
                        "- Error code: " & Hex(ex.HResult) & CrLf & CrLf
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Sub GetPackageFileInformation()
        Dim msg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg = "Preparing package information processes..."
                    Case "ESN"
                        msg = "Preparando procesos de información de paquetes..."
                    Case "FRA"
                        msg = "Préparation des processus d'information des paquets en cours..."
                    Case "PTB", "PTG"
                        msg = "A preparar processos de informação sobre pacotes..."
                    Case "ITA"
                        msg = "Preparazione dei processi di informazione sui pacchetti..."
                End Select
            Case 1
                msg = "Preparing package information processes..."
            Case 2
                msg = "Preparando procesos de información de paquetes..."
            Case 3
                msg = "Préparation des processus d'information des paquets en cours..."
            Case 4
                msg = "A preparar processos de informação sobre pacotes..."
            Case 5
                msg = "Preparazione dei processi di informazione sui pacchetti..."
        End Select
        Contents &= "## Package file information" & CrLf & CrLf & _
                    "- Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf & CrLf
        Debug.WriteLine("[GetPackageFileInformation] Starting task...")
        Try
            Debug.WriteLine("[GetPackageFileInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetPackageFileInformation] Creating image session...")
            ReportChanges(msg, 0)
            Contents &= "**Amount of package files to get information about: " & PackageFiles.Count & "**"
            Contents &= CrLf & CrLf & FullPackageTableHeader
            Dim pkgCustomPropsList As String = "<ul>"
            Dim pkgFeaturesList As String = "<ul>"
            Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                For Each pkgFile In PackageFiles
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "Getting information from package files... (package file " & PackageFiles.IndexOf(pkgFile) + 1 & " of " & PackageFiles.Count & ")"
                                Case "ESN"
                                    msg = "Obteniendo información de archivos de paquetes... (archivo de paquete " & PackageFiles.IndexOf(pkgFile) + 1 & " de " & PackageFiles.Count & ")"
                                Case "FRA"
                                    msg = "Obtention des informations des fichiers paquets en cours... (fichier paquet " & PackageFiles.IndexOf(pkgFile) + 1 & " de " & PackageFiles.Count & ")"
                                Case "PTB", "PTG"
                                    msg = "Obter informações dos ficheiros do pacote... (ficheiro do pacote " & PackageFiles.IndexOf(pkgFile) + 1 & " de " & PackageFiles.Count & ")"
                                Case "ITA"
                                    msg = "Ottenere informazioni dai file di pacchetto... (file di pacchetto " & PackageFiles.IndexOf(pkgFile) + 1 & " di " & PackageFiles.Count & ")"
                            End Select
                        Case 1
                            msg = "Getting information from package files... (package file " & PackageFiles.IndexOf(pkgFile) + 1 & " of " & PackageFiles.Count & ")"
                        Case 2
                            msg = "Obteniendo información de archivos de paquetes... (archivo de paquete " & PackageFiles.IndexOf(pkgFile) + 1 & " de " & PackageFiles.Count & ")"
                        Case 3
                            msg = "Obtention des informations des fichiers paquets en cours... (fichier paquet " & PackageFiles.IndexOf(pkgFile) + 1 & " de " & PackageFiles.Count & ")"
                        Case 4
                            msg = "Obter informações dos ficheiros do pacote... (ficheiro do pacote " & PackageFiles.IndexOf(pkgFile) + 1 & " de " & PackageFiles.Count & ")"
                        Case 5
                            msg = "Ottenere informazioni dai file di pacchetto... (file di pacchetto " & PackageFiles.IndexOf(pkgFile) + 1 & " di " & PackageFiles.Count & ")"
                    End Select
                    ReportChanges(msg, (PackageFiles.IndexOf(pkgFile) / PackageFiles.Count) * 100)
                    If File.Exists(pkgFile) Then
                        Dim pkgInfoEx As DismPackageInfoEx = Nothing
                        Dim pkgInfo As DismPackageInfo = Nothing
                        Dim cProps As DismCustomPropertyCollection = Nothing

                        ' Determine Windows version
                        If Environment.OSVersion.Version.Major >= 10 Then
                            pkgInfoEx = DismApi.GetPackageInfoExByPath(imgSession, pkgFile)
                        Else
                            pkgInfo = DismApi.GetPackageInfoByPath(imgSession, pkgFile)
                        End If
                        If pkgInfoEx IsNot Nothing Then
                            pkgCustomPropsList = "<ul>"
                            pkgFeaturesList = "<ul>"
                            cProps = pkgInfoEx.CustomProperties
                            If cProps.Count > 0 Then
                                For Each cProp As DismCustomProperty In cProps
                                    pkgCustomPropsList &= "<li>" & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value.Replace(CrLf, " ").Replace(Lf, " ").Replace(Cr, " ").Trim() & "</li>"
                                Next
                                pkgCustomPropsList &= "</ul>"
                            Else
                                pkgCustomPropsList = "None"
                            End If
                            If pkgInfoEx.Features.Count > 0 Then
                                Dim pkgFeats As DismFeatureCollection = pkgInfoEx.Features
                                For Each pkgFeat As DismFeature In pkgFeats
                                    pkgFeaturesList &= "<li>" & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & "</li>"
                                Next
                                pkgFeaturesList &= "</ul>"
                            Else
                                pkgFeaturesList = "None"
                            End If
                            Contents &= TableSeparator & CodeBlockChar & pkgInfoEx.PackageName & CodeBlockChar & TableSeparator & Casters.CastDismApplicabilityStatus(pkgInfoEx.Applicable) & TableSeparator & pkgInfoEx.Copyright & TableSeparator & pkgInfoEx.Company & TableSeparator & pkgInfoEx.CreationTime & TableSeparator & pkgInfoEx.Description & TableSeparator & If(pkgInfoEx.InstallClient = "", "None", pkgInfoEx.InstallClient) & TableSeparator & If(pkgInfoEx.InstallPackageName = "", "None", CodeBlockChar & pkgInfoEx.InstallPackageName & CodeBlockChar) & TableSeparator & pkgInfoEx.InstallTime & TableSeparator & pkgInfoEx.LastUpdateTime & TableSeparator & pkgInfoEx.DisplayName & TableSeparator & pkgInfoEx.ProductName & TableSeparator & pkgInfoEx.ProductVersion.ToString() & TableSeparator & Casters.CastDismReleaseType(pkgInfoEx.ReleaseType) & TableSeparator & Casters.CastDismRestartType(pkgInfoEx.RestartRequired) & TableSeparator & pkgInfoEx.SupportInformation & TableSeparator & Casters.CastDismPackageState(pkgInfoEx.PackageState) & TableSeparator & Casters.CastDismFullyOfflineInstallationType(pkgInfoEx.FullyOffline) & TableSeparator & If(pkgInfoEx.CapabilityId = "", "None", CodeBlockChar & pkgInfoEx.CapabilityId & CodeBlockChar) & TableSeparator & pkgCustomPropsList & TableSeparator & pkgFeaturesList & TableSeparator & CrLf
                        ElseIf pkgInfo IsNot Nothing Then
                            pkgCustomPropsList = "<ul>"
                            pkgFeaturesList = "<ul>"
                            cProps = pkgInfo.CustomProperties
                            If cProps.Count > 0 Then
                                For Each cProp As DismCustomProperty In cProps
                                    pkgCustomPropsList &= "<li>" & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value.Replace(CrLf, " ").Replace(Lf, " ").Replace(Cr, " ").Trim() & "</li>"
                                Next
                                pkgCustomPropsList &= "</ul>"
                            Else
                                pkgCustomPropsList = "None"
                            End If
                            If pkgInfo.Features.Count > 0 Then
                                Dim pkgFeats As DismFeatureCollection = pkgInfo.Features
                                For Each pkgFeat As DismFeature In pkgFeats
                                    pkgFeaturesList &= "<li>" & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & "</li>"
                                Next
                                pkgFeaturesList &= "</ul>"
                            Else
                                pkgFeaturesList = "None"
                            End If
                            Contents &= TableSeparator & CodeBlockChar & pkgInfo.PackageName & CodeBlockChar & TableSeparator & Casters.CastDismApplicabilityStatus(pkgInfo.Applicable) & TableSeparator & pkgInfo.Copyright & TableSeparator & pkgInfo.Company & TableSeparator & pkgInfo.CreationTime & TableSeparator & pkgInfo.Description & TableSeparator & If(pkgInfo.InstallClient = "", "None", pkgInfo.InstallClient) & TableSeparator & If(pkgInfo.InstallPackageName = "", "None", CodeBlockChar & pkgInfo.InstallPackageName & CodeBlockChar) & TableSeparator & pkgInfo.InstallTime & TableSeparator & pkgInfo.LastUpdateTime & TableSeparator & pkgInfo.DisplayName & TableSeparator & pkgInfo.ProductName & TableSeparator & pkgInfo.ProductVersion.ToString() & TableSeparator & Casters.CastDismReleaseType(pkgInfo.ReleaseType) & TableSeparator & Casters.CastDismRestartType(pkgInfo.RestartRequired) & TableSeparator & pkgInfo.SupportInformation & TableSeparator & Casters.CastDismPackageState(pkgInfo.PackageState) & TableSeparator & Casters.CastDismFullyOfflineInstallationType(pkgInfo.FullyOffline) & TableSeparator & "None" & TableSeparator & pkgCustomPropsList & TableSeparator & pkgFeaturesList & TableSeparator & CrLf
                        End If
                    End If
                Next
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetPackageFileInformation] An error occurred while getting package information: " & ex.ToString() & " - " & ex.Message)
            Contents &= "The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                        "- Exception: " & ex.ToString() & CrLf & _
                        "- Exception message: " & ex.Message & CrLf & _
                        "- Error code: " & Hex(ex.HResult) & CrLf & CrLf
        Finally
            DismApi.Shutdown()
        End Try

    End Sub

    Sub GetFeatureInformation()
        Dim InstalledFeatInfo As DismFeatureCollection = Nothing
        Dim msg As String() = New String(2) {"", "", ""}
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg(0) = "Preparing feature information processes..."
                        msg(1) = "The program has obtained basic information of the installed features of this image. You can also get complete information of such features and save it in the report." & CrLf & CrLf & _
                          "Do note that this will take longer depending on the number of installed features." & CrLf & CrLf & _
                          "Do you want to get this information and save it in the report?"
                        msg(2) = "Feature information"
                    Case "ESN"
                        msg(0) = "Preparando procesos de información de características..."
                        msg(1) = "El programa ha obtenido información básica de las características instaladas en esta imagen. También puede obtener información completa de dichas características y guardarla en el informe." & CrLf & CrLf & _
                          "Dese cuenta de que esto tardará más, dependiendo del número de características instaladas." & CrLf & CrLf & _
                          "¿Desea obtener esta información y guardarla en el informe?"
                        msg(2) = "Información de características"
                    Case "FRA"
                        msg(0) = "Préparation des processus d'information sur les caractéristiques en cours..."
                        msg(1) = "Le programme a obtenu des informations basiques sur les caractéristiques installées sur cette image. Vous pouvez également obtenir des informations complètes sur ces caractéristiques et les enregistrer dans le rapport." & CrLf & CrLf & _
                          "Notez que cette opération peut prendre plus de temps en fonction du nombre de caractéristiques installées." & CrLf & CrLf & _
                          "Souhaitez-vous obtenir ces informations et les enregistrer dans le rapport ?"
                        msg(2) = "Informations sur les caractéristiques"
                    Case "PTB", "PTG"
                        msg(0) = "A preparar processos de informação de características..."
                        msg(1) = "O programa obteve informações básicas sobre as características instaladas desta imagem. Também pode obter informações completas sobre essas características e guardá-las no relatório." & CrLf & CrLf & _
                          "Tenha em atenção que isto pode demorar mais tempo, dependendo do número de características instaladas." & CrLf & CrLf & _
                          "Pretende obter esta informação e guardá-la no relatório?"
                        msg(2) = "Informação sobre as características"
                    Case "ITA"
                        msg(0) = "Preparazione dei processi di informazione sulle caratteristiche..."
                        msg(1) = "Il programma ha ottenuto informazioni elementari sulle caratteristiche installate su questa immagine. È inoltre possibile ottenere informazioni complete su tali funzioni e salvarle nel rapporto." & CrLf & CrLf & _
                          "Tenere presente che questa operazione richiederà più tempo a seconda del numero di funzioni installate." & CrLf & CrLf & _
                          "Volete ottenere queste informazioni e salvarle nel rapporto?"
                        msg(2) = "Informazioni sulle caratteristiche"
                End Select
            Case 1
                msg(0) = "Preparing feature information processes..."
                msg(1) = "The program has obtained basic information of the installed features of this image. You can also get complete information of such features and save it in the report." & CrLf & CrLf & _
                  "Do note that this will take longer depending on the number of installed features." & CrLf & CrLf & _
                  "Do you want to get this information and save it in the report?"
                msg(2) = "Feature information"
            Case 2
                msg(0) = "Preparando procesos de información de características..."
                msg(1) = "El programa ha obtenido información básica de las características instaladas en esta imagen. También puede obtener información completa de dichos características y guardarla en el informe." & CrLf & CrLf & _
                  "Dese cuenta de que esto tardará más, dependiendo del número de características instalados." & CrLf & CrLf & _
                  "¿Desea obtener esta información y guardarla en el informe?"
                msg(2) = "Información de características"
            Case 3
                msg(0) = "Préparation des processus d'information sur les caractéristiques en cours..."
                msg(1) = "Le programme a obtenu des informations basiques sur les caractéristiques installées sur cette image. Vous pouvez également obtenir des informations complètes sur ces caractéristiques et les enregistrer dans le rapport." & CrLf & CrLf & _
                  "Notez que cette opération peut prendre plus de temps en fonction du nombre de caractéristiques installées." & CrLf & CrLf & _
                  "Souhaitez-vous obtenir ces informations et les enregistrer dans le rapport ?"
                msg(2) = "Informations sur les caractéristiques"
            Case 4
                msg(0) = "A preparar processos de informação de características..."
                msg(1) = "O programa obteve informações básicas sobre as características instaladas desta imagem. Também pode obter informações completas sobre essas características e guardá-las no relatório." & CrLf & CrLf & _
                  "Tenha em atenção que isto pode demorar mais tempo, dependendo do número de características instaladas." & CrLf & CrLf & _
                  "Pretende obter esta informação e guardá-la no relatório?"
                msg(2) = "Informação sobre as características"
            Case 5
                msg(0) = "Preparazione dei processi di informazione sulle caratteristiche..."
                msg(1) = "Il programma ha ottenuto informazioni elementari sulle caratteristiche installate su questa immagine. È inoltre possibile ottenere informazioni complete su tali funzioni e salvarle nel rapporto." & CrLf & CrLf & _
                  "Tenere presente che questa operazione richiederà più tempo a seconda del numero di funzioni installate." & CrLf & CrLf & _
                  "Volete ottenere queste informazioni e salvarle nel rapporto?"
                msg(2) = "Informazioni sulle caratteristiche"
        End Select
        Contents &= "## Feature information" & CrLf & CrLf & _
                    "- Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf & CrLf
        Debug.WriteLine("[GetFeatureInformation] Starting task...")
        Try
            Debug.WriteLine("[GetFeatureInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetFeatureInformation] Creating image session...")
            ReportChanges(msg(0), 0)
            Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                Debug.WriteLine("[GetFeatureInformation] Getting basic feature information...")
                ReportChanges(msg(0), 5)
                InstalledFeatInfo = DismApi.GetFeatures(imgSession)
                Contents &= "**Information summary for " & InstalledFeatInfo.Count & " feature(s):**" & CrLf & CrLf
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg(0) = "Features have been obtained"
                            Case "ESN"
                                msg(0) = "Las características han sido obtenidas"
                            Case "FRA"
                                msg(0) = "Des caractéristiques ont été obtenues"
                            Case "PTB", "PTG"
                                msg(0) = "As características foram obtidas"
                            Case "ITA"
                                msg(0) = "Le caratteristiche sono state ottenute"
                        End Select
                    Case 1
                        msg(0) = "Features have been obtained"
                    Case 2
                        msg(0) = "Las características han sido obtenidas"
                    Case 3
                        msg(0) = "Des caractéristiques ont été obtenues"
                    Case 4
                        msg(0) = "As características foram obtidas"
                    Case 5
                        msg(0) = "Le caratteristiche sono state ottenute"
                End Select
                ReportChanges(msg(0), 10)
                Dim featCustomPropsList As String = "<ul>"
                If SkipQuestions And AutoCompleteInfo(1) Then
                    Contents &= CrLf & CrLf & FullFeatureTableHeader
                    Debug.WriteLine("[GetFeatureInformation] Getting complete feature information...")
                    For Each feature As DismFeature In InstalledFeatInfo
                        featCustomPropsList = "<ul>"
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        msg(0) = "Getting information of features... (feature " & InstalledFeatInfo.IndexOf(feature) + 1 & " of " & InstalledFeatInfo.Count & ")"
                                    Case "ESN"
                                        msg(0) = "Obteniendo información de características... (característica " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                                    Case "FRA"
                                        msg(0) = "Obtention des informations sur les caractéristiques en cours... (caractéristique " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                                    Case "PTB", "PTG"
                                        msg(0) = "Obter informações sobre as características... (caraterística " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                                    Case "ITA"
                                        msg(0) = "Ottenere informazioni sulle caratteristiche... (caratteristica " & InstalledFeatInfo.IndexOf(feature) + 1 & " di " & InstalledFeatInfo.Count & ")"
                                End Select
                            Case 1
                                msg(0) = "Getting information of features... (feature " & InstalledFeatInfo.IndexOf(feature) + 1 & " of " & InstalledFeatInfo.Count & ")"
                            Case 2
                                msg(0) = "Obteniendo información de características... (característica " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                            Case 3
                                msg(0) = "Obtention des informations sur les caractéristiques en cours... (caractéristique " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                            Case 4
                                msg(0) = "Obter informações sobre as características... (caraterística " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                            Case 5
                                msg(0) = "Ottenere informazioni sulle caratteristiche... (caratteristica " & InstalledFeatInfo.IndexOf(feature) + 1 & " di " & InstalledFeatInfo.Count & ")"
                        End Select
                        ReportChanges(msg(0), (InstalledFeatInfo.IndexOf(feature) / InstalledFeatInfo.Count) * 100)
                        Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, feature.FeatureName)
                        Dim cProps As DismCustomPropertyCollection = featInfo.CustomProperties
                        If cProps.Count > 0 Then
                            For Each cProp As DismCustomProperty In cProps
                                featCustomPropsList &= "<li>" & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value.Replace(CrLf, " ").Replace(Lf, " ").Replace(Cr, " ").Trim() & "</li>"
                            Next
                            featCustomPropsList &= "</ul>"
                        Else
                            featCustomPropsList = "None"
                        End If
                        Contents &= TableSeparator & featInfo.FeatureName & TableSeparator & featInfo.DisplayName & TableSeparator & featInfo.Description & TableSeparator & Casters.CastDismRestartType(featInfo.RestartRequired) & TableSeparator & Casters.CastDismFeatureState(featInfo.FeatureState) & TableSeparator & featCustomPropsList & TableSeparator & CrLf
                    Next
                    Contents &= CrLf & "- Complete feature information has been gathered" & CrLf
                ElseIf (Not SkipQuestions Or Not AutoCompleteInfo(1)) And MsgBox(msg(1), vbYesNo + vbQuestion, msg(2)) = MsgBoxResult.Yes Then
                    Contents &= CrLf & CrLf & FullFeatureTableHeader
                    Debug.WriteLine("[GetFeatureInformation] Getting complete feature information...")
                    For Each feature As DismFeature In InstalledFeatInfo
                        featCustomPropsList = "<ul>"
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        msg(0) = "Getting information of features... (feature " & InstalledFeatInfo.IndexOf(feature) + 1 & " of " & InstalledFeatInfo.Count & ")"
                                    Case "ESN"
                                        msg(0) = "Obteniendo información de características... (característica " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                                    Case "FRA"
                                        msg(0) = "Obtention des informations sur les caractéristiques en cours... (caractéristique " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                                    Case "PTB", "PTG"
                                        msg(0) = "Obter informações sobre as características... (caraterística " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                                    Case "ITA"
                                        msg(0) = "Ottenere informazioni sulle caratteristiche... (caratteristica " & InstalledFeatInfo.IndexOf(feature) + 1 & " di " & InstalledFeatInfo.Count & ")"
                                End Select
                            Case 1
                                msg(0) = "Getting information of features... (feature " & InstalledFeatInfo.IndexOf(feature) + 1 & " of " & InstalledFeatInfo.Count & ")"
                            Case 2
                                msg(0) = "Obteniendo información de características... (característica " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                            Case 3
                                msg(0) = "Obtention des informations sur les caractéristiques en cours... (caractéristique " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                            Case 4
                                msg(0) = "Obter informações sobre as características... (caraterística " & InstalledFeatInfo.IndexOf(feature) + 1 & " de " & InstalledFeatInfo.Count & ")"
                            Case 5
                                msg(0) = "Ottenere informazioni sulle caratteristiche... (caratteristica " & InstalledFeatInfo.IndexOf(feature) + 1 & " di " & InstalledFeatInfo.Count & ")"
                        End Select
                        ReportChanges(msg(0), (InstalledFeatInfo.IndexOf(feature) / InstalledFeatInfo.Count) * 100)
                        Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, feature.FeatureName)
                        Dim cProps As DismCustomPropertyCollection = featInfo.CustomProperties
                        If cProps.Count > 0 Then
                            For Each cProp As DismCustomProperty In cProps
                                featCustomPropsList &= "<li>" & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value.Replace(CrLf, " ").Replace(Lf, " ").Replace(Cr, " ").Trim() & "</li>"
                            Next
                            featCustomPropsList &= "</ul>"
                        Else
                            featCustomPropsList = "None"
                        End If
                        Contents &= TableSeparator & featInfo.FeatureName & TableSeparator & featInfo.DisplayName & TableSeparator & featInfo.Description & TableSeparator & Casters.CastDismRestartType(featInfo.RestartRequired) & TableSeparator & Casters.CastDismFeatureState(featInfo.FeatureState) & TableSeparator & featCustomPropsList & TableSeparator & CrLf
                    Next
                    Contents &= CrLf & "- Complete feature information has been gathered" & CrLf
                Else
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg(0) = "Saving installed features..."
                                Case "ESN"
                                    msg(0) = "Guardando características instaladas..."
                                Case "FRA"
                                    msg(0) = "Sauvegarde des caractéristiques installés en cours..."
                                Case "PTB", "PTG"
                                    msg(0) = "Guardar as características instaladas..."
                                Case "ITA"
                                    msg(0) = "Salvataggio delle caratteristiche installate..."
                            End Select
                        Case 1
                            msg(0) = "Saving installed features..."
                        Case 2
                            msg(0) = "Guardando características instaladas..."
                        Case 3
                            msg(0) = "Sauvegarde des caractéristiques installés en cours..."
                        Case 4
                            msg(0) = "Guardar as características instaladas..."
                        Case 5
                            msg(0) = "Salvataggio delle caratteristiche installate..."
                    End Select
                    ReportChanges(msg(0), 50)
                    Contents &= BasicFeatureTableHeader
                    For Each installedFeature As DismFeature In InstalledFeatInfo
                        Contents &= TableSeparator & installedFeature.FeatureName & TableSeparator & Casters.CastDismFeatureState(installedFeature.State) & TableSeparator & CrLf
                    Next
                    Contents &= CrLf & "- Complete feature information has not been gathered" & CrLf
                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetFeatureInformation] An error occurred while getting feature information: " & ex.ToString() & " - " & ex.Message)
            Contents &= "The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                        "- Exception: " & ex.ToString() & CrLf & _
                        "- Exception message: " & ex.Message & CrLf & _
                        "- Error code: " & Hex(ex.HResult) & CrLf & CrLf
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Sub GetAppxInformation()
        Dim InstalledAppxPackageInfo As DismAppxPackageCollection = Nothing
        Dim msg As String() = New String(2) {"", "", ""}
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg(0) = "Preparing AppX package information processes..."
                        msg(1) = "The program has obtained basic information of the installed AppX packages of this image. You can also get complete information of such AppX packages and save it in the report." & CrLf & CrLf & _
                          "Do note that this will take longer depending on the number of installed AppX packages." & CrLf & CrLf & _
                          "Do you want to get this information and save it in the report?"
                        msg(2) = "AppX package information"
                    Case "ESN"
                        msg(0) = "Preparando procesos de información de paquetes AppX..."
                        msg(1) = "El programa ha obtenido información básica de los paquetes AppX instalados en esta imagen. También puede obtener información completa de dichos paquetes AppX y guardarla en el informe." & CrLf & CrLf & _
                          "Dese cuenta de que esto tardará más, dependiendo del número de paquetes AppX instalados." & CrLf & CrLf & _
                          "¿Desea obtener esta información y guardarla en el informe?"
                        msg(2) = "Información de paquetes AppX"
                    Case "FRA"
                        msg(0) = "Préparation des processus d'information sur les paquets AppX en cours..."
                        msg(1) = "Le programme a obtenu des informations basiques sur les paquets AppX installés sur cette image. Vous pouvez également obtenir des informations complètes sur ces paquets AppX et les enregistrer dans le rapport." & CrLf & CrLf & _
                          "Notez que cette opération peut prendre plus de temps en fonction du nombre de paquets AppX installés." & CrLf & CrLf & _
                          "Souhaitez-vous obtenir ces informations et les enregistrer dans le rapport ?"
                        msg(2) = "Informations sur les paquets AppX"
                    Case "PTB", "PTG"
                        msg(0) = "A preparar processos de informação dos pacotes AppX..."
                        msg(1) = "O programa obteve informações básicas sobre os pacotes AppX instalados nesta imagem. Também pode obter informações completas sobre esses pacotes AppX e guardá-las no relatório." & CrLf & CrLf & _
                          "Tem em atenção que isto demorará mais tempo, dependendo do número de pacotes AppX instalados." & CrLf & CrLf & _
                          "Deseja obter esta informação e guardá-la no relatório?"
                        msg(2) = "Informação dos pacotes AppX"
                    Case "ITA"
                        msg(0) = "Preparazione dei processi di informazione sui pacchetti AppX..."
                        msg(1) = "Il programma ha ottenuto informazioni elementari sui pacchetti AppX installati in questa immagine. È inoltre possibile ottenere informazioni complete su tali pacchetti AppX e salvarle nel rapporto." & CrLf & CrLf & _
                          "Si noti che questa operazione richiederà più tempo a seconda del numero di pacchetti AppX installati." & CrLf & CrLf & _
                          "Volete ottenere queste informazioni e salvarle nel rapporto?"
                        msg(2) = "Informazioni sui pacchetti AppX"
                End Select
            Case 1
                msg(0) = "Preparing AppX package information processes..."
                msg(1) = "The program has obtained basic information of the installed AppX packages of this image. You can also get complete information of such AppX packages and save it in the report." & CrLf & CrLf & _
                  "Do note that this will take longer depending on the number of installed AppX packages." & CrLf & CrLf & _
                  "Do you want to get this information and save it in the report?"
                msg(2) = "AppX package information"
            Case 2
                msg(0) = "Preparando procesos de información de paquetes AppX..."
                msg(1) = "El programa ha obtenido información básica de los paquetes AppX instalados en esta imagen. También puede obtener información completa de dichos paquetes AppX y guardarla en el informe." & CrLf & CrLf & _
                  "Dese cuenta de que esto tardará más, dependiendo del número de paquetes AppX instalados." & CrLf & CrLf & _
                  "¿Desea obtener esta información y guardarla en el informe?"
                msg(2) = "Información de paquetes AppX"
            Case 3
                msg(0) = "Préparation des processus d'information sur les paquets AppX en cours..."
                msg(1) = "Le programme a obtenu des informations basiques sur les paquets AppX installés sur cette image. Vous pouvez également obtenir des informations complètes sur ces paquets AppX et les enregistrer dans le rapport." & CrLf & CrLf & _
                  "Notez que cette opération peut prendre plus de temps en fonction du nombre de paquets AppX installés." & CrLf & CrLf & _
                  "Souhaitez-vous obtenir ces informations et les enregistrer dans le rapport ?"
                msg(2) = "Informations sur les paquets AppX"
            Case 4
                msg(0) = "A preparar processos de informação dos pacotes AppX..."
                msg(1) = "O programa obteve informações básicas sobre os pacotes AppX instalados nesta imagem. Também pode obter informações completas sobre esses pacotes AppX e guardá-las no relatório." & CrLf & CrLf & _
                  "Tem em atenção que isto demorará mais tempo, dependendo do número de pacotes AppX instalados." & CrLf & CrLf & _
                  "Deseja obter esta informação e guardá-la no relatório?"
                msg(2) = "Informação dos pacotes AppX"
            Case 5
                msg(0) = "Preparazione dei processi di informazione sui pacchetti AppX..."
                msg(1) = "Il programma ha ottenuto informazioni elementari sui pacchetti AppX installati in questa immagine. È inoltre possibile ottenere informazioni complete su tali pacchetti AppX e salvarle nel rapporto." & CrLf & CrLf & _
                  "Si noti che questa operazione richiederà più tempo a seconda del numero di pacchetti AppX installati." & CrLf & CrLf & _
                  "Volete ottenere queste informazioni e salvarle nel rapporto?"
                msg(2) = "Informazioni sui pacchetti AppX"
        End Select
        Contents &= "## AppX package information" & CrLf & CrLf & _
                    "- Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf
        If MainForm.imgEdition Is Nothing Then
            MainForm.imgEdition = " "
        End If
        ' Detect if the image is Windows 8 or later. If not, skip this task
        If (Not OnlineMode And (Not MainForm.IsWindows8OrHigher(ImgMountDir & "\Windows\system32\ntoskrnl.exe") Or MainForm.imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase))) Or (OnlineMode And Not MainForm.IsWindows8OrHigher(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe")) Then
            Contents &= "**This task is not supported on the specified Windows image. Check that it contains Windows 8 or a later Windows version, and that it isn't a Windows PE image.** Skipping task..." & CrLf & CrLf
            Exit Sub
        Else
            Debug.WriteLine("[GetAppxInformation] Starting task...")
            ' Do note that, when using the MainForm arrays, an empty entry appears at the end, so don't take it into account
            Try
                ' Windows 8 can't get this information with the API. Use the MainForm arrays
                If Environment.OSVersion.Version.Major < 10 Then
                    Contents &= CrLf & "**Information summary for " & MainForm.imgAppxPackageNames.Count - 1 & " AppX package(s):**" & CrLf & CrLf & FullAppxTableHeader
                    For x = 0 To Array.LastIndexOf(MainForm.imgAppxPackageNames, MainForm.imgAppxPackageNames.Last)
                        If x = MainForm.imgAppxPackageNames.Count - 1 Or MainForm.imgAppxPackageNames(x) Is Nothing Then Continue For
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        msg(0) = "Getting information of AppX packages... (AppX package " & x + 1 & " of " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                    Case "ESN"
                                        msg(0) = "Obteniendo información de paquetes AppX... (paquete AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                    Case "FRA"
                                        msg(0) = "Obtention des informations sur les paquets AppX en cours... (paquet AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                    Case "PTB", "PTG"
                                        msg(0) = "Obter informações sobre os pacotes AppX... (pacote AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                    Case "ITA"
                                        msg(0) = "Ottenere informazioni sui pacchetti AppX... (pacchetto AppX " & x + 1 & " di " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                End Select
                            Case 1
                                msg(0) = "Getting information of AppX packages... (AppX package " & x + 1 & " of " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                            Case 2
                                msg(0) = "Obteniendo información de paquetes AppX... (paquete AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                            Case 3
                                msg(0) = "Obtention des informations sur les paquets AppX en cours... (paquet AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                            Case 4
                                msg(0) = "Obter informações sobre os pacotes AppX... (pacote AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                            Case 5
                                msg(0) = "Ottenere informazioni sui pacchetti AppX... (pacchetto AppX " & x + 1 & " di " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                        End Select
                        ReportChanges(msg(0), ((x + 1) / MainForm.imgAppxPackageNames.Count) * 100)
                        Contents &= TableSeparator & MainForm.imgAppxPackageNames(x) & TableSeparator & MainForm.imgAppxDisplayNames(x) & TableSeparator & MainForm.imgAppxArchitectures(x) & TableSeparator & MainForm.imgAppxResourceIds(x) & TableSeparator & MainForm.imgAppxVersions(x) & TableSeparator & "{APPXPKG.REGISTERED}" & TableSeparator & "{APPXPKG.INSTALLLOCATION}" & TableSeparator & "{APPXPKG.MANLOCATION}" & TableSeparator & "{APPXPKG.LOGOASSETDIR}" & TableSeparator & "{APPXPKG.MAINLOGO}" & TableSeparator & CrLf
                        Dim registrationStatus As String = ""                         ' Use to pass final result to Markdown report
                        ' Detect if *.pckgdep files are present in the AppRepository folder, as that's how this program gets the registration status of an AppX package
                        If Directory.Exists(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x), _
                                               ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x))) Then
                            ' Get the number of pckgdep files
                            If My.Computer.FileSystem.GetFiles(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x), _
                                                                  ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x)), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count > 0 Then
                                registrationStatus = "Yes"
                            Else
                                registrationStatus = "No"
                            End If
                        Else
                            registrationStatus = "No"
                        End If
                        Dim installationLocation As String = (If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x)).Replace("\\", "\").Trim()
                        Dim pkgDirs() As String = Directory.GetDirectories(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps", MainForm.imgAppxPackageNames(x) & "*", SearchOption.TopDirectoryOnly)
                        Dim instDir As String = ""
                        For Each folder In pkgDirs
                            If Not folder.Contains("neutral") Then
                                instDir = (folder & "\AppxManifest.xml").Replace("\\", "\").Trim()
                            End If
                        Next
                        Try
                            If pkgDirs.Count <= 1 And Not instDir.Contains(MainForm.imgAppxPackageNames(x)) Then
                                If File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml") Then
                                    instDir = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml"
                                ElseIf File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml") Then
                                    instDir = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml"
                                Else
                                    instDir = "Unknown"
                                End If
                            End If
                        Catch ex As Exception
                            instDir = "Unknown"
                        End Try
                        ' Get store logo asset directory
                        Dim logoAssetDir As String = ""                         ' Use to pass final result to Markdown report
                        Dim assetDir As String = ""
                        Try
                            assetDir = MainForm.GetSuitablePackageFolder(MainForm.imgAppxDisplayNames(x))
                        Catch ex As Exception
                            ' Continue
                        End Try
                        If assetDir <> "" Then
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
                                        logoAssetDir = (assetDir & "\" & newPath).Replace("\\", "\").Trim()
                                        Exit For
                                    End If
                                Next
                            End If
                        Else
                            If File.Exists(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x) & "\AppxManifest.xml") Then
                                Dim ManFile As New RichTextBox() With {
                                    .Text = File.ReadAllText(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x) & "\AppxManifest.xml")
                                }
                                For Each line In ManFile.Lines
                                    If line.Contains("<Logo>") Then
                                        Dim SplitPaths As New List(Of String)
                                        SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                                        SplitPaths.RemoveAt(SplitPaths.Count - 1)
                                        Dim newPath As String = String.Join("\", SplitPaths)
                                        logoAssetDir = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x) & "\" & newPath).Replace("\\", "\").Trim()
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                        ' Since store logo assets can't be saved on plain text files, output their locations
                        Dim mainLogo As String = ""                         ' Use to pass final result to Markdown report
                        Dim mainAsset As String = MainForm.GetStoreAppMainLogo(MainForm.imgAppxPackageNames(x))
                        If mainAsset <> "" And File.Exists(mainAsset) Then
                            mainLogo = mainAsset.Replace("\\", "\").Trim()
                        Else
                            mainLogo = "Unknown"
                        End If
                        Contents = Contents.Replace("{APPXPKG.REGISTERED}", registrationStatus).Replace("{APPXPKG.INSTALLLOCATION}", installationLocation).Replace("{APPXPKG.MANLOCATION}", instDir).Replace("{APPXPKG.LOGOASSETDIR}", logoAssetDir.TrimEnd("\")).Replace("{APPXPKG.MAINLOGO}", mainLogo.TrimEnd(Quote))
                    Next
                    Contents &= CrLf & "NOTE: main store logo asset locations are a guess, and may not be the assets you're looking for. If that happens, report an issue on the GitHub repo using the " & Quote & "Store logo asset preview issue" & Quote & " template. Then, provide the package name, the expected asset and the obtained asset." & CrLf & CrLf
                Else
                    Debug.WriteLine("[GetAppxInformation] Starting API...")
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Debug.WriteLine("[GetAppxInformation] Creating image session...")
                    Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                        Debug.WriteLine("[GetAppxInformation] Getting basic AppX package information...")
                        ReportChanges(msg(0), 5)
                        InstalledAppxPackageInfo = DismApi.GetProvisionedAppxPackages(imgSession)
                        ' Determine if MainForm arrays contain more stuff
                        Dim pkgNames As New List(Of String)
                        For Each pkg As DismAppxPackage In InstalledAppxPackageInfo
                            pkgNames.Add(pkg.PackageName)
                        Next
                        Contents &= CrLf & "**Information summary for " & If(MainForm.imgAppxPackageNames.Count - 1 > pkgNames.Count, MainForm.imgAppxPackageNames.Count - 1, pkgNames.Count) & " AppX package(s):**" & CrLf & CrLf & FullAppxTableHeader
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        msg(0) = "AppX packages have been obtained"
                                    Case "ESN"
                                        msg(0) = "Los paquetes AppX han sido obtenidos"
                                    Case "FRA"
                                        msg(0) = "Des paquets AppX ont été obtenus"
                                    Case "PTB", "PTG"
                                        msg(0) = "Os pacotes AppX foram obtidos"
                                    Case "ITA"
                                        msg(0) = "I pacchetti AppX sono stati ottenuti"
                                End Select
                            Case 1
                                msg(0) = "AppX packages have been obtained"
                            Case 2
                                msg(0) = "Los paquetes AppX han sido obtenidos"
                            Case 3
                                msg(0) = "Des paquets AppX ont été obtenus"
                            Case 4
                                msg(0) = "Os pacotes AppX foram obtidos"
                            Case 5
                                msg(0) = "I pacchetti AppX sono stati ottenuti"
                        End Select
                        ReportChanges(msg(0), 10)
                        If SkipQuestions And AutoCompleteInfo(2) Then
                            Debug.WriteLine("[GetAppxInformation] Getting complete AppX package information...")
                            If Not ForceAppxApi AndAlso MainForm.imgAppxPackageNames.Count - 1 > pkgNames.Count Then
                                For x = 0 To Array.LastIndexOf(MainForm.imgAppxPackageNames, MainForm.imgAppxPackageNames.Last)
                                    If x = MainForm.imgAppxPackageNames.Count - 1 Then Continue For
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENU", "ENG"
                                                    msg(0) = "Getting information of AppX packages... (AppX package " & x + 1 & " of " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                                Case "ESN"
                                                    msg(0) = "Obteniendo información de paquetes AppX... (paquete AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                                Case "FRA"
                                                    msg(0) = "Obtention des informations sur les paquets AppX en cours... (paquet AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                                Case "PTB", "PTG"
                                                    msg(0) = "Obter informações sobre os pacotes AppX... (pacote AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                                Case "ITA"
                                                    msg(0) = "Ottenere informazioni sui pacchetti AppX... (pacchetto AppX " & x + 1 & " di " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                            End Select
                                        Case 1
                                            msg(0) = "Getting information of AppX packages... (AppX package " & x + 1 & " of " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                        Case 2
                                            msg(0) = "Obteniendo información de paquetes AppX... (paquete AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                        Case 3
                                            msg(0) = "Obtention des informations sur les paquets AppX en cours... (paquet AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                        Case 4
                                            msg(0) = "Obter informações sobre os pacotes AppX... (pacote AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                        Case 5
                                            msg(0) = "Ottenere informazioni sui pacchetti AppX... (pacchetto AppX " & x + 1 & " di " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                    End Select
                                    ReportChanges(msg(0), ((x + 1) / MainForm.imgAppxPackageNames.Count) * 100)
                                    Contents &= TableSeparator & MainForm.imgAppxPackageNames(x) & TableSeparator & MainForm.imgAppxDisplayNames(x) & TableSeparator & MainForm.imgAppxArchitectures(x) & TableSeparator & MainForm.imgAppxResourceIds(x) & TableSeparator & MainForm.imgAppxVersions(x) & TableSeparator & "{APPXPKG.REGISTERED}" & TableSeparator & "{APPXPKG.INSTALLLOCATION}" & TableSeparator & "{APPXPKG.MANLOCATION}" & TableSeparator & "{APPXPKG.LOGOASSETDIR}" & TableSeparator & "{APPXPKG.MAINLOGO}" & TableSeparator & CrLf
                                    Dim registrationStatus As String = ""                         ' Use to pass final result to Markdown report
                                    ' Detect if *.pckgdep files are present in the AppRepository folder, as that's how this program gets the registration status of an AppX package
                                    If Directory.Exists(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x), _
                                                           ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x))) Then
                                        ' Get the number of pckgdep files
                                        If My.Computer.FileSystem.GetFiles(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x), _
                                                                              ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x)), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count > 0 Then
                                            registrationStatus = "Yes"
                                        Else
                                            registrationStatus = "No"
                                        End If
                                    Else
                                        registrationStatus = "No"
                                    End If
                                    Dim installationLocation As String = (If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x)).Replace("\\", "\").Trim()
                                    Dim pkgDirs() As String = Directory.GetDirectories(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps", MainForm.imgAppxPackageNames(x) & "*", SearchOption.TopDirectoryOnly)
                                    Dim instDir As String = ""
                                    For Each folder In pkgDirs
                                        If Not folder.Contains("neutral") Then
                                            instDir = (folder & "\AppxManifest.xml").Replace("\\", "\").Trim()
                                        End If
                                    Next
                                    Try
                                        If pkgDirs.Count <= 1 And Not instDir.Contains(MainForm.imgAppxPackageNames(x)) Then
                                            If File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml") Then
                                                instDir = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml"
                                            ElseIf File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml") Then
                                                instDir = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml"
                                            Else
                                                instDir = "Unknown"
                                            End If
                                        End If
                                    Catch ex As Exception
                                        instDir = "Unknown"
                                    End Try
                                    ' Get store logo asset directory
                                    Dim logoAssetDir As String = ""                         ' Use to pass final result to Markdown report
                                    Dim assetDir As String = ""
                                    Try
                                        assetDir = MainForm.GetSuitablePackageFolder(MainForm.imgAppxDisplayNames(x))
                                    Catch ex As Exception
                                        ' Continue
                                    End Try
                                    If assetDir <> "" Then
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
                                                    logoAssetDir = (assetDir & "\" & newPath).Replace("\\", "\").Trim()
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    Else
                                        If File.Exists(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x) & "\AppxManifest.xml") Then
                                            Dim ManFile As New RichTextBox() With {
                                                .Text = File.ReadAllText(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x) & "\AppxManifest.xml")
                                            }
                                            For Each line In ManFile.Lines
                                                If line.Contains("<Logo>") Then
                                                    Dim SplitPaths As New List(Of String)
                                                    SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                                                    SplitPaths.RemoveAt(SplitPaths.Count - 1)
                                                    Dim newPath As String = String.Join("\", SplitPaths)
                                                    logoAssetDir = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x) & "\" & newPath).Replace("\\", "\").Trim()
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    End If
                                    ' Since store logo assets can't be saved on plain text files, output their locations
                                    Dim mainLogo As String = ""                         ' Use to pass final result to Markdown report
                                    Dim mainAsset As String = MainForm.GetStoreAppMainLogo(MainForm.imgAppxPackageNames(x))
                                    If mainAsset <> "" And File.Exists(mainAsset) Then
                                        mainLogo = mainAsset.Replace("\\", "\").Trim()
                                    Else
                                        mainLogo = "Unknown"
                                    End If
                                    Contents = Contents.Replace("{APPXPKG.REGISTERED}", registrationStatus).Replace("{APPXPKG.INSTALLLOCATION}", installationLocation).Replace("{APPXPKG.MANLOCATION}", instDir).Replace("{APPXPKG.LOGOASSETDIR}", logoAssetDir.TrimEnd("\")).Replace("{APPXPKG.MAINLOGO}", mainLogo)
                                Next
                                Contents &= CrLf & "NOTE: main store logo asset locations are a guess, and may not be the assets you're looking for. If that happens, report an issue on the GitHub repo using the " & Quote & "Store logo asset preview issue" & Quote & " template. Then, provide the package name, the expected asset and the obtained asset." & CrLf & CrLf
                            Else
                                For Each appxPkg As DismAppxPackage In InstalledAppxPackageInfo
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENU", "ENG"
                                                    msg(0) = "Getting information of AppX packages... (AppX package " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " of " & InstalledAppxPackageInfo.Count & ")"
                                                Case "ESN"
                                                    msg(0) = "Obteniendo información de paquetes AppX ... (paquete AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                                Case "FRA"
                                                    msg(0) = "Obtention des informations sur les paquets AppX en cours... (paquet AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                                Case "PTB", "PTG"
                                                    msg(0) = "Obter informações sobre os pacotes AppX... (pacote AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                                Case "ITA"
                                                    msg(0) = "Ottenere informazioni sui pacchetti AppX... (pacchetto AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " di " & InstalledAppxPackageInfo.Count & ")"
                                            End Select
                                        Case 1
                                            msg(0) = "Getting information of AppX packages... (AppX package " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " of " & InstalledAppxPackageInfo.Count & ")"
                                        Case 2
                                            msg(0) = "Obteniendo información de paquetes AppX ... (paquete AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                        Case 3
                                            msg(0) = "Obtention des informations sur les paquets AppX en cours... (paquet AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                        Case 4
                                            msg(0) = "Obter informações sobre os pacotes AppX... (pacote AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                        Case 5
                                            msg(0) = "Ottenere informazioni sui pacchetti AppX... (pacchetto AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " di " & InstalledAppxPackageInfo.Count & ")"
                                    End Select
                                    ReportChanges(msg(0), (InstalledAppxPackageInfo.IndexOf(appxPkg) / InstalledAppxPackageInfo.Count) * 100)
                                    Contents &= TableSeparator & appxPkg.PackageName & TableSeparator & appxPkg.DisplayName & TableSeparator & Casters.CastDismArchitecture(appxPkg.Architecture) & TableSeparator & appxPkg.ResourceId & TableSeparator & appxPkg.Version.ToString() & TableSeparator & "{APPXPKG.REGISTERED}" & TableSeparator & "{APPXPKG.INSTALLLOCATION}" & TableSeparator & "{APPXPKG.MANLOCATION}" & TableSeparator & "{APPXPKG.LOGOASSETDIR}" & TableSeparator & "{APPXPKG.MAINLOGO}" & TableSeparator & CrLf
                                    Dim registrationStatus As String = ""                         ' Use to pass final result to Markdown report
                                    ' Detect if *.pckgdep files are present in the AppRepository folder, as that's how this program gets the registration status of an AppX package
                                    If Directory.Exists(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName, _
                                                           ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName)) Then
                                        ' Get the number of pckgdep files
                                        If My.Computer.FileSystem.GetFiles(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName, _
                                                                              ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count > 0 Then
                                            registrationStatus = "Yes"
                                        Else
                                            registrationStatus = "No"
                                        End If
                                    Else
                                        registrationStatus = "No"
                                    End If
                                    ' Use the InstallLocation property of the AppxPackage class.
                                    ' TODO: if this works, implement InstallLocation on all other cases
                                    Dim installationLocation As String = appxPkg.InstallLocation.Replace("%SYSTEMDRIVE%", Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)).Replace("\", "").Trim()).Trim().Replace("\" & Path.GetFileName(appxPkg.InstallLocation), "").Trim()
                                    Dim pkgManifestLocation As String = ""                         ' Use to pass final result to Markdown report
                                    ' Detect if the source is an appx or appxbundle package by the manifest file
                                    If File.Exists(installationLocation & "\AppxManifest.xml") Then
                                        ' APPX/MSIX file
                                        pkgManifestLocation = installationLocation & "\AppxManifest.xml"
                                    ElseIf File.Exists(installationLocation & "\AppxBundleManifest.xml") Then
                                        ' APPXBUNDLE/MSIXBUNDLE file
                                        pkgManifestLocation = installationLocation & "\AppxBundleManifest.xml"
                                    Else
                                        ' Unrecognized type of file
                                        pkgManifestLocation = "Unknown"
                                    End If
                                    ' Get store logo asset directory
                                    Dim logoAssetDir As String = ""                         ' Use to pass final result to Markdown report
                                    Dim assetDir As String = ""
                                    Try
                                        assetDir = MainForm.GetSuitablePackageFolder(appxPkg.DisplayName)
                                    Catch ex As Exception
                                        ' Continue
                                    End Try
                                    If assetDir <> "" Then
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
                                                    logoAssetDir = (assetDir & "\" & newPath).Replace("\\", "\").Trim()
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    Else
                                        If File.Exists(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & appxPkg.PackageName & "\AppxManifest.xml") Then
                                            Dim ManFile As New RichTextBox() With {
                                                .Text = File.ReadAllText(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & appxPkg.PackageName & "\AppxManifest.xml")
                                            }
                                            For Each line In ManFile.Lines
                                                If line.Contains("<Logo>") Then
                                                    Dim SplitPaths As New List(Of String)
                                                    SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                                                    SplitPaths.RemoveAt(SplitPaths.Count - 1)
                                                    Dim newPath As String = String.Join("\", SplitPaths)
                                                    logoAssetDir = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & appxPkg.PackageName & "\" & newPath).Replace("\\", "\").Trim()
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    End If
                                    ' Since store logo assets can't be saved on plain text files, output their locations
                                    Dim mainLogo As String = ""                         ' Use to pass final result to Markdown report
                                    Dim mainAsset As String = MainForm.GetStoreAppMainLogo(appxPkg.PackageName)
                                    If mainAsset <> "" And File.Exists(mainAsset) Then
                                        mainLogo = mainAsset.Replace("\\", "\").Trim()
                                    Else
                                        mainLogo = "Unknown"
                                    End If
                                    Contents = Contents.Replace("{APPXPKG.REGISTERED}", registrationStatus).Replace("{APPXPKG.INSTALLLOCATION}", installationLocation).Replace("{APPXPKG.MANLOCATION}", pkgManifestLocation).Replace("{APPXPKG.LOGOASSETDIR}", logoAssetDir.TrimEnd("\")).Replace("{APPXPKG.MAINLOGO}", mainLogo)
                                Next
                                Contents &= CrLf & "NOTE: main store logo asset locations are a guess, and may not be the assets you're looking for. If that happens, report an issue on the GitHub repo using the " & Quote & "Store logo asset preview issue" & Quote & " template. Then, provide the package name, the expected asset and the obtained asset." & CrLf & CrLf
                            End If
                            Contents &= CrLf & "- Complete AppX package information has been gathered" & CrLf
                        ElseIf (Not SkipQuestions Or Not AutoCompleteInfo(2)) And MsgBox(msg(1), vbYesNo + vbQuestion, msg(2)) = MsgBoxResult.Yes Then
                            Debug.WriteLine("[GetAppxInformation] Getting complete AppX package information...")
                            If MainForm.imgAppxPackageNames.Count - 1 > pkgNames.Count Then
                                For x = 0 To Array.LastIndexOf(MainForm.imgAppxPackageNames, MainForm.imgAppxPackageNames.Last)
                                    If x = MainForm.imgAppxPackageNames.Count - 1 Then Continue For
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENU", "ENG"
                                                    msg(0) = "Getting information of AppX packages... (AppX package " & x + 1 & " of " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                                Case "ESN"
                                                    msg(0) = "Obteniendo información de paquetes AppX... (paquete AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                                Case "FRA"
                                                    msg(0) = "Obtention des informations sur les paquets AppX en cours... (paquet AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                                Case "PTB", "PTG"
                                                    msg(0) = "Obter informações sobre os pacotes AppX... (pacote AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                                Case "ITA"
                                                    msg(0) = "Ottenere informazioni sui pacchetti AppX... (pacchetto AppX " & x + 1 & " di " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                            End Select
                                        Case 1
                                            msg(0) = "Getting information of AppX packages... (AppX package " & x + 1 & " of " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                        Case 2
                                            msg(0) = "Obteniendo información de paquetes AppX... (paquete AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                        Case 3
                                            msg(0) = "Obtention des informations sur les paquets AppX en cours... (paquet AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                        Case 4
                                            msg(0) = "Obter informações sobre os pacotes AppX... (pacote AppX " & x + 1 & " de " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                        Case 5
                                            msg(0) = "Ottenere informazioni sui pacchetti AppX... (pacchetto AppX " & x + 1 & " di " & MainForm.imgAppxPackageNames.Count - 1 & ")"
                                    End Select
                                    ReportChanges(msg(0), ((x + 1) / MainForm.imgAppxPackageNames.Count) * 100)
                                    Contents &= TableSeparator & MainForm.imgAppxPackageNames(x) & TableSeparator & MainForm.imgAppxDisplayNames(x) & TableSeparator & MainForm.imgAppxArchitectures(x) & TableSeparator & MainForm.imgAppxResourceIds(x) & TableSeparator & MainForm.imgAppxVersions(x) & TableSeparator & "{APPXPKG.REGISTERED}" & TableSeparator & "{APPXPKG.INSTALLLOCATION}" & TableSeparator & "{APPXPKG.MANLOCATION}" & TableSeparator & "{APPXPKG.LOGOASSETDIR}" & TableSeparator & "{APPXPKG.MAINLOGO}" & TableSeparator & CrLf
                                    Dim registrationStatus As String = ""                         ' Use to pass final result to Markdown report
                                    ' Detect if *.pckgdep files are present in the AppRepository folder, as that's how this program gets the registration status of an AppX package
                                    If Directory.Exists(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x), _
                                                           ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x))) Then
                                        ' Get the number of pckgdep files
                                        If My.Computer.FileSystem.GetFiles(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x), _
                                                                              ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & MainForm.imgAppxPackageNames(x)), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count > 0 Then
                                            registrationStatus = "Yes"
                                        Else
                                            registrationStatus = "No"
                                        End If
                                    Else
                                        registrationStatus = "No"
                                    End If
                                    Dim installationLocation As String = (If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x)).Replace("\\", "\").Trim()
                                    Dim pkgDirs() As String = Directory.GetDirectories(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps", MainForm.imgAppxPackageNames(x) & "*", SearchOption.TopDirectoryOnly)
                                    Dim instDir As String = ""
                                    For Each folder In pkgDirs
                                        If Not folder.Contains("neutral") Then
                                            instDir = (folder & "\AppxManifest.xml").Replace("\\", "\").Trim()
                                        End If
                                    Next
                                    Try
                                        If pkgDirs.Count <= 1 And Not instDir.Contains(MainForm.imgAppxPackageNames(x)) Then
                                            If File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml") Then
                                                instDir = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml"
                                            ElseIf File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml") Then
                                                instDir = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml"
                                            Else
                                                instDir = "Unknown"
                                            End If
                                        End If
                                    Catch ex As Exception
                                        instDir = "Unknown"
                                    End Try
                                    ' Get store logo asset directory
                                    Dim logoAssetDir As String = ""                         ' Use to pass final result to Markdown report
                                    Dim assetDir As String = ""
                                    Try
                                        assetDir = MainForm.GetSuitablePackageFolder(MainForm.imgAppxDisplayNames(x))
                                    Catch ex As Exception
                                        ' Continue
                                    End Try
                                    If assetDir <> "" Then
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
                                                    logoAssetDir = (assetDir & "\" & newPath).Replace("\\", "\").Trim()
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    Else
                                        If File.Exists(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x) & "\AppxManifest.xml") Then
                                            Dim ManFile As New RichTextBox() With {
                                                .Text = File.ReadAllText(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x) & "\AppxManifest.xml")
                                            }
                                            For Each line In ManFile.Lines
                                                If line.Contains("<Logo>") Then
                                                    Dim SplitPaths As New List(Of String)
                                                    SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                                                    SplitPaths.RemoveAt(SplitPaths.Count - 1)
                                                    Dim newPath As String = String.Join("\", SplitPaths)
                                                    logoAssetDir = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & MainForm.imgAppxPackageNames(x) & "\" & newPath).Replace("\\", "\").Trim()
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    End If
                                    ' Since store logo assets can't be saved on plain text files, output their locations
                                    Dim mainLogo As String = ""                         ' Use to pass final result to Markdown report
                                    Dim mainAsset As String = MainForm.GetStoreAppMainLogo(MainForm.imgAppxPackageNames(x))
                                    If mainAsset <> "" And File.Exists(mainAsset) Then
                                        mainLogo = mainAsset.Replace("\\", "\").Trim()
                                    Else
                                        mainLogo = "Unknown"
                                    End If
                                    Contents = Contents.Replace("{APPXPKG.REGISTERED}", registrationStatus).Replace("{APPXPKG.INSTALLLOCATION}", installationLocation).Replace("{APPXPKG.MANLOCATION}", instDir).Replace("{APPXPKG.LOGOASSETDIR}", logoAssetDir.TrimEnd("\")).Replace("{APPXPKG.MAINLOGO}", mainLogo)
                                Next
                                Contents &= CrLf & "NOTE: main store logo asset locations are a guess, and may not be the assets you're looking for. If that happens, report an issue on the GitHub repo using the " & Quote & "Store logo asset preview issue" & Quote & " template. Then, provide the package name, the expected asset and the obtained asset." & CrLf & CrLf
                            Else
                                For Each appxPkg As DismAppxPackage In InstalledAppxPackageInfo
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENU", "ENG"
                                                    msg(0) = "Getting information of AppX packages... (AppX package " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " of " & InstalledAppxPackageInfo.Count & ")"
                                                Case "ESN"
                                                    msg(0) = "Obteniendo información de paquetes AppX ... (paquete AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                                Case "FRA"
                                                    msg(0) = "Obtention des informations sur les paquets AppX en cours... (paquet AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                                Case "PTB", "PTG"
                                                    msg(0) = "Obter informações sobre os pacotes AppX... (pacote AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                                Case "ITA"
                                                    msg(0) = "Ottenere informazioni sui pacchetti AppX... (pacchetto AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " di " & InstalledAppxPackageInfo.Count & ")"
                                            End Select
                                        Case 1
                                            msg(0) = "Getting information of AppX packages... (AppX package " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " of " & InstalledAppxPackageInfo.Count & ")"
                                        Case 2
                                            msg(0) = "Obteniendo información de paquetes AppX ... (paquete AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                        Case 3
                                            msg(0) = "Obtention des informations sur les paquets AppX en cours... (paquet AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                        Case 4
                                            msg(0) = "Obter informações sobre os pacotes AppX... (pacote AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " de " & InstalledAppxPackageInfo.Count & ")"
                                        Case 5
                                            msg(0) = "Ottenere informazioni sui pacchetti AppX... (pacchetto AppX " & InstalledAppxPackageInfo.IndexOf(appxPkg) + 1 & " di " & InstalledAppxPackageInfo.Count & ")"
                                    End Select
                                    ReportChanges(msg(0), (InstalledAppxPackageInfo.IndexOf(appxPkg) / InstalledAppxPackageInfo.Count) * 100)
                                    Contents &= TableSeparator & appxPkg.PackageName & TableSeparator & appxPkg.DisplayName & TableSeparator & Casters.CastDismArchitecture(appxPkg.Architecture) & TableSeparator & appxPkg.ResourceId & TableSeparator & appxPkg.Version.ToString() & TableSeparator & "{APPXPKG.REGISTERED}" & TableSeparator & "{APPXPKG.INSTALLLOCATION}" & TableSeparator & "{APPXPKG.MANLOCATION}" & TableSeparator & "{APPXPKG.LOGOASSETDIR}" & TableSeparator & "{APPXPKG.MAINLOGO}" & TableSeparator & CrLf
                                    Dim registrationStatus As String = ""                         ' Use to pass final result to Markdown report
                                    ' Detect if *.pckgdep files are present in the AppRepository folder, as that's how this program gets the registration status of an AppX package
                                    If Directory.Exists(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName, _
                                                           ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName)) Then
                                        ' Get the number of pckgdep files
                                        If My.Computer.FileSystem.GetFiles(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName, _
                                                                              ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count > 0 Then
                                            registrationStatus = "Yes"
                                        Else
                                            registrationStatus = "No"
                                        End If
                                    Else
                                        registrationStatus = "No"
                                    End If
                                    ' Use the InstallLocation property of the AppxPackage class.
                                    ' TODO: if this works, implement InstallLocation on all other cases
                                    Dim installationLocation As String = ""
                                    installationLocation = appxPkg.InstallLocation.Replace("%SYSTEMDRIVE%", Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)).Replace("\", "").Trim()).Trim().Replace("\" & Path.GetFileName(appxPkg.InstallLocation), "").Trim()
                                    Dim pkgManifestLocation As String = ""                         ' Use to pass final result to Markdown report
                                    ' Detect if the source is an appx or appxbundle package by the manifest file
                                    If File.Exists(installationLocation & "\AppxManifest.xml") Then
                                        ' APPX/MSIX file
                                        pkgManifestLocation = installationLocation & "\AppxManifest.xml"
                                    ElseIf File.Exists(installationLocation & "\AppxBundleManifest.xml") Then
                                        ' APPXBUNDLE/MSIXBUNDLE file
                                        pkgManifestLocation = installationLocation & "\AppxBundleManifest.xml"
                                    Else
                                        ' Unrecognized type of file
                                        pkgManifestLocation = "Unknown"
                                    End If
                                    ' Get store logo asset directory
                                    Dim logoAssetDir As String = ""                         ' Use to pass final result to Markdown report
                                    Dim assetDir As String = ""
                                    Try
                                        assetDir = MainForm.GetSuitablePackageFolder(appxPkg.DisplayName)
                                    Catch ex As Exception
                                        ' Continue
                                    End Try
                                    If assetDir <> "" Then
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
                                                    logoAssetDir = (assetDir & "\" & newPath).Replace("\\", "\").Trim()
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    Else
                                        If File.Exists(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & appxPkg.PackageName & "\AppxManifest.xml") Then
                                            Dim ManFile As New RichTextBox() With {
                                                .Text = File.ReadAllText(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & appxPkg.PackageName & "\AppxManifest.xml")
                                            }
                                            For Each line In ManFile.Lines
                                                If line.Contains("<Logo>") Then
                                                    Dim SplitPaths As New List(Of String)
                                                    SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                                                    SplitPaths.RemoveAt(SplitPaths.Count - 1)
                                                    Dim newPath As String = String.Join("\", SplitPaths)
                                                    logoAssetDir = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & appxPkg.PackageName & "\" & newPath).Replace("\\", "\").Trim()
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    End If
                                    ' Since store logo assets can't be saved on plain text files, output their locations
                                    Dim mainLogo As String = ""                         ' Use to pass final result to Markdown report
                                    Dim mainAsset As String = MainForm.GetStoreAppMainLogo(appxPkg.PackageName)
                                    If mainAsset <> "" And File.Exists(mainAsset) Then
                                        mainLogo = mainAsset.Replace("\\", "\").Trim() & Quote
                                    Else
                                        mainLogo = "Unknown"
                                    End If
                                    Contents = Contents.Replace("{APPXPKG.REGISTERED}", registrationStatus).Replace("{APPXPKG.INSTALLLOCATION}", installationLocation).Replace("{APPXPKG.MANLOCATION}", pkgManifestLocation).Replace("{APPXPKG.LOGOASSETDIR}", logoAssetDir.TrimEnd("\")).Replace("{APPXPKG.MAINLOGO}", mainLogo)
                                Next
                            End If
                            Contents &= CrLf & "- Complete AppX package information has been gathered" & CrLf
                        Else
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            msg(0) = "Saving installed AppX packages..."
                                        Case "ESN"
                                            msg(0) = "Guardando paquetes AppX instalados..."
                                        Case "FRA"
                                            msg(0) = "Sauvegarde des paquets AppX installés en cours..."
                                        Case "PTB", "PTG"
                                            msg(0) = "Guardar os pacotes AppX instalados..."
                                        Case "ITA"
                                            msg(0) = "Salvataggio dei pacchetti AppX installati..."
                                    End Select
                                Case 1
                                    msg(0) = "Saving installed AppX packages..."
                                Case 2
                                    msg(0) = "Guardando paquetes AppX instalados..."
                                Case 3
                                    msg(0) = "Sauvegarde des paquets AppX installés en cours..."
                                Case 4
                                    msg(0) = "Guardar os pacotes AppX instalados..."
                                Case 5
                                    msg(0) = "Salvataggio dei pacchetti AppX installati..."
                            End Select
                            ReportChanges(msg(0), 50)
                            Contents &= BasicAppxTableHeader
                            For Each installedAppxPkg As DismAppxPackage In InstalledAppxPackageInfo
                                Contents &= TableSeparator & installedAppxPkg.PackageName & TableSeparator & installedAppxPkg.DisplayName & TableSeparator & Casters.CastDismArchitecture(installedAppxPkg.Architecture) & TableSeparator & installedAppxPkg.ResourceId & TableSeparator & installedAppxPkg.Version.ToString() & TableSeparator & CrLf
                            Next
                            Contents &= CrLf & "- Complete AppX package information has not been gathered" & CrLf
                        End If
                    End Using
                End If
            Catch ex As Exception
                Debug.WriteLine("[GetAppxInformation] An error occurred while getting AppX package information: " & ex.ToString() & " - " & ex.Message)
                Contents &= "The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                            "- Exception: " & ex.ToString() & CrLf & _
                            "- Exception message: " & ex.Message & CrLf & _
                            "- Error code: " & Hex(ex.HResult) & CrLf & CrLf
            Finally
                DismApi.Shutdown()
            End Try
        End If
    End Sub

    Sub GetCapabilityInformation()
        Dim InstalledCapInfo As DismCapabilityCollection = Nothing
        Dim msg As String() = New String(2) {"", "", ""}
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg(0) = "Preparing capability information processes..."
                        msg(1) = "The program has obtained basic information of the installed capabilities of this image. You can also get complete information of such capabilities and save it in the report." & CrLf & CrLf & _
                          "Do note that this will take longer depending on the number of installed capabilities." & CrLf & CrLf & _
                          "Do you want to get this information and save it in the report?"
                        msg(2) = "Capability information"
                    Case "ESN"
                        msg(0) = "Preparando procesos de información de funcionalidades..."
                        msg(1) = "El programa ha obtenido información básica de las funcionalidades instaladas en esta imagen. También puede obtener información completa de dichas funcionalidades y guardarla en el informe." & CrLf & CrLf & _
                          "Dese cuenta de que esto tardará más, dependiendo del número de funcionalidades instaladas." & CrLf & CrLf & _
                          "¿Desea obtener esta información y guardarla en el informe?"
                        msg(2) = "Información de funcionalidades"
                    Case "FRA"
                        msg(0) = "Préparation des processus d'information sur les capacités en cours..."
                        msg(1) = "Le programme a obtenu des informations basiques sur les capacités installés sur cette image. Vous pouvez également obtenir des informations complètes sur ces capacités et les enregistrer dans le rapport." & CrLf & CrLf & _
                          "Notez que cette opération peut prendre plus de temps en fonction du nombre de capacités installées." & CrLf & CrLf & _
                          "Souhaitez-vous obtenir ces informations et les enregistrer dans le rapport ?"
                        msg(2) = "Informations sur les capacités"
                    Case "PTB", "PTG"
                        msg(0) = "A preparar processos de informação de capacidades..."
                        msg(1) = "O programa obteve informações básicas sobre as capacidades instaladas desta imagem. Também pode obter informações completas sobre essas capacidades e guardá-las no relatório." & CrLf & CrLf & _
                          "Tenha em atenção que isto pode demorar mais tempo, dependendo do número de capacidades instaladas." & CrLf & CrLf & _
                          "Deseja obter esta informação e guardá-la no relatório?"
                        msg(2) = "Informações sobre as capacidades"
                    Case "ITA"
                        msg(0) = "Preparazione dei processi di informazione sulle capacità..."
                        msg(1) = "Il programma ha ottenuto informazioni elementari sulle capacità installate di questa immagine. È inoltre possibile ottenere informazioni complete su tali funzionalità e salvarle nel rapporto." & CrLf & CrLf & _
                          "Si noti che questa operazione richiederà più tempo a seconda del numero di funzionalità installate." & CrLf & CrLf & _
                          "Volete ottenere queste informazioni e salvarle nel rapporto?"
                        msg(2) = "Informazioni sulle capacità"
                End Select
            Case 1
                msg(0) = "Preparing capability information processes..."
                msg(1) = "The program has obtained basic information of the installed capabilities of this image. You can also get complete information of such capabilities and save it in the report." & CrLf & CrLf & _
                  "Do note that this will take longer depending on the number of installed capabilities." & CrLf & CrLf & _
                  "Do you want to get this information and save it in the report?"
                msg(2) = "Capability information"
            Case 2
                msg(0) = "Preparando procesos de información de funcionalidades..."
                msg(1) = "El programa ha obtenido información básica de las funcionalidades instaladas en esta imagen. También puede obtener información completa de dichas funcionalidades y guardarla en el informe." & CrLf & CrLf & _
                  "Dese cuenta de que esto tardará más, dependiendo del número de funcionalidades instaladas." & CrLf & CrLf & _
                  "¿Desea obtener esta información y guardarla en el informe?"
                msg(2) = "Información de funcionalidades"
            Case 3
                msg(0) = "Préparation des processus d'information sur les capacités en cours..."
                msg(1) = "Le programme a obtenu des informations basiques sur les capacités installés sur cette image. Vous pouvez également obtenir des informations complètes sur ces capacités et les enregistrer dans le rapport." & CrLf & CrLf & _
                  "Notez que cette opération peut prendre plus de temps en fonction du nombre de capacités installées." & CrLf & CrLf & _
                  "Souhaitez-vous obtenir ces informations et les enregistrer dans le rapport ?"
                msg(2) = "Informations sur les capacités"
            Case 4
                msg(0) = "A preparar processos de informação de capacidades..."
                msg(1) = "O programa obteve informações básicas sobre as capacidades instaladas desta imagem. Também pode obter informações completas sobre essas capacidades e guardá-las no relatório." & CrLf & CrLf & _
                  "Tenha em atenção que isto pode demorar mais tempo, dependendo do número de capacidades instaladas." & CrLf & CrLf & _
                  "Deseja obter esta informação e guardá-la no relatório?"
                msg(2) = "Informações sobre as capacidades"
            Case 5
                msg(0) = "Preparazione dei processi di informazione sulle capacità..."
                msg(1) = "Il programma ha ottenuto informazioni elementari sulle capacità installate di questa immagine. È inoltre possibile ottenere informazioni complete su tali funzionalità e salvarle nel rapporto." & CrLf & CrLf & _
                  "Si noti che questa operazione richiederà più tempo a seconda del numero di funzionalità installate." & CrLf & CrLf & _
                  "Volete ottenere queste informazioni e salvarle nel rapporto?"
                msg(2) = "Informazioni sulle capacità"
        End Select
        Contents &= "## Capability information" & CrLf & CrLf & _
                    "- Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf & CrLf
        If MainForm.imgEdition Is Nothing Then
            MainForm.imgEdition = " "
        End If
        If (Not OnlineMode And (Not MainForm.IsWindows10OrHigher(ImgMountDir & "\Windows\system32\ntoskrnl.exe") Or MainForm.imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase))) Or (OnlineMode And Not MainForm.IsWindows10OrHigher(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe")) Then
            Contents &= "**This task is not supported on the specified Windows image. Check that it contains Windows 10 or a later Windows version, and that it isn't a Windows PE image.** Skipping task..." & CrLf & CrLf
            Exit Sub
        Else
            Debug.WriteLine("[GetCapabilityInformation] Starting task...")
            Try
                Debug.WriteLine("[GetCapabilityInformation] Starting API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                Debug.WriteLine("[GetCapabilityInformation] Creating image session...")
                Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                    Debug.WriteLine("[GetCapabilityInformation] Getting basic capability information...")
                    ReportChanges(msg(0), 5)
                    InstalledCapInfo = DismApi.GetCapabilities(imgSession)
                    Contents &= "**Information summary for " & InstalledCapInfo.Count & " capability/ies:**" & CrLf & CrLf
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg(0) = "Capabilities have been obtained"
                                Case "ESN"
                                    msg(0) = "Las funcionalidades han sido obtenidas"
                                Case "FRA"
                                    msg(0) = "Des capacités ont été obtenues"
                                Case "PTB", "PTG"
                                    msg(0) = "As capacidades foram obtidas"
                                Case "ITA"
                                    msg(0) = "Le capacità sono state ottenute"
                            End Select
                        Case 1
                            msg(0) = "Capabilities have been obtained"
                        Case 2
                            msg(0) = "Las funcionalidades han sido obtenidas"
                        Case 3
                            msg(0) = "Des capacités ont été obtenues"
                        Case 4
                            msg(0) = "As capacidades foram obtidas"
                        Case 5
                            msg(0) = "Le capacità sono state ottenute"
                    End Select
                    ReportChanges(msg(0), 10)
                    If SkipQuestions And AutoCompleteInfo(3) Then
                        Contents &= CrLf & CrLf & FullCapabilityTableHeader
                        Debug.WriteLine("[GetCapabilityInformation] Getting complete capability information...")
                        For Each capability As DismCapability In InstalledCapInfo
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            msg(0) = "Getting information of capabilities... (capability " & InstalledCapInfo.IndexOf(capability) + 1 & " of " & InstalledCapInfo.Count & ")"
                                        Case "ESN"
                                            msg(0) = "Obteniendo información de funcionalidades... (funcionalidad " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                        Case "FRA"
                                            msg(0) = "Obtention des informations sur les capacités en cours... (capacité " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                        Case "PTB", "PTG"
                                            msg(0) = "Obter informações sobre as capacidades... (capacidade " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                        Case "ITA"
                                            msg(0) = "Ottenere informazioni sulle capacità... (capacità " & InstalledCapInfo.IndexOf(capability) + 1 & " di " & InstalledCapInfo.Count & ")"
                                    End Select
                                Case 1
                                    msg(0) = "Getting information of capabilities... (capability " & InstalledCapInfo.IndexOf(capability) + 1 & " of " & InstalledCapInfo.Count & ")"
                                Case 2
                                    msg(0) = "Obteniendo información de funcionalidades... (funcionalidad " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                Case 3
                                    msg(0) = "Obtention des informations sur les capacités en cours... (capacité " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                Case 4
                                    msg(0) = "Obter informações sobre as capacidades... (capacidade " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                Case 5
                                    msg(0) = "Ottenere informazioni sulle capacità... (capacità " & InstalledCapInfo.IndexOf(capability) + 1 & " di " & InstalledCapInfo.Count & ")"
                            End Select
                            ReportChanges(msg(0), (InstalledCapInfo.IndexOf(capability) / InstalledCapInfo.Count) * 100)
                            Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, capability.Name)
                            Contents &= TableSeparator & CodeBlockChar & capInfo.Name & CodeBlockChar & TableSeparator & CodeBlockChar & capInfo.Name.Remove(InStr(capInfo.Name, "~") - 1) & CodeBlockChar & TableSeparator & Casters.CastDismPackageState(capInfo.State) & TableSeparator & capInfo.Description & TableSeparator & capInfo.DownloadSize & " bytes" & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")", "") & TableSeparator & capInfo.InstallSize & " bytes" & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")", "") & TableSeparator & CrLf
                        Next
                        Contents &= CrLf & "- Complete capability information has been gathered" & CrLf
                    ElseIf (Not SkipQuestions Or Not AutoCompleteInfo(3)) And MsgBox(msg(1), vbYesNo + vbQuestion, msg(2)) = MsgBoxResult.Yes Then
                        Contents &= CrLf & CrLf & FullCapabilityTableHeader
                        Debug.WriteLine("[GetCapabilityInformation] Getting complete capability information...")
                        For Each capability As DismCapability In InstalledCapInfo
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            msg(0) = "Getting information of capabilities... (capability " & InstalledCapInfo.IndexOf(capability) + 1 & " of " & InstalledCapInfo.Count & ")"
                                        Case "ESN"
                                            msg(0) = "Obteniendo información de funcionalidades... (funcionalidad " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                        Case "FRA"
                                            msg(0) = "Obtention des informations sur les capacités en cours... (capacité " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                        Case "PTB", "PTG"
                                            msg(0) = "Obter informações sobre as capacidades... (capacidade " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                        Case "ITA"
                                            msg(0) = "Ottenere informazioni sulle capacità... (capacità " & InstalledCapInfo.IndexOf(capability) + 1 & " di " & InstalledCapInfo.Count & ")"
                                    End Select
                                Case 1
                                    msg(0) = "Getting information of capabilities... (capability " & InstalledCapInfo.IndexOf(capability) + 1 & " of " & InstalledCapInfo.Count & ")"
                                Case 2
                                    msg(0) = "Obteniendo información de funcionalidades... (funcionalidad " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                Case 3
                                    msg(0) = "Obtention des informations sur les capacités en cours... (capacité " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                Case 4
                                    msg(0) = "Obter informações sobre as capacidades... (capacidade " & InstalledCapInfo.IndexOf(capability) + 1 & " de " & InstalledCapInfo.Count & ")"
                                Case 5
                                    msg(0) = "Ottenere informazioni sulle capacità... (capacità " & InstalledCapInfo.IndexOf(capability) + 1 & " di " & InstalledCapInfo.Count & ")"
                            End Select
                            ReportChanges(msg(0), (InstalledCapInfo.IndexOf(capability) / InstalledCapInfo.Count) * 100)
                            Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, capability.Name)
                            Contents &= TableSeparator & CodeBlockChar & capInfo.Name & CodeBlockChar & TableSeparator & CodeBlockChar & capInfo.Name.Remove(InStr(capInfo.Name, "~") - 1) & CodeBlockChar & TableSeparator & Casters.CastDismPackageState(capInfo.State) & TableSeparator & capInfo.Description & TableSeparator & capInfo.DownloadSize & " bytes" & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")", "") & TableSeparator & capInfo.InstallSize & " bytes" & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")", "") & TableSeparator & CrLf
                        Next
                        Contents &= CrLf & "- Complete capability information has been gathered" & CrLf
                    Else
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        msg(0) = "Saving installed capabilities..."
                                    Case "ESN"
                                        msg(0) = "Guardando funcionalidades instaladas..."
                                    Case "FRA"
                                        msg(0) = "Sauvegarde des caractéristiques installées en cours..."
                                    Case "PTB", "PTG"
                                        msg(0) = "Guardar as capacidades instaladas..."
                                    Case "ITA"
                                        msg(0) = "Salvataggio delle capacità installate..."
                                End Select
                            Case 1
                                msg(0) = "Saving installed capabilities..."
                            Case 2
                                msg(0) = "Guardando funcionalidades instaladas..."
                            Case 3
                                msg(0) = "Sauvegarde des caractéristiques installées en cours..."
                            Case 4
                                msg(0) = "Guardar as capacidades instaladas..."
                            Case 5
                                msg(0) = "Salvataggio delle capacità installate..."
                        End Select
                        ReportChanges(msg(0), 50)
                        Contents &= BasicCapabilityTableHeader
                        For Each installedCapability As DismCapability In InstalledCapInfo
                            Contents &= TableSeparator & CodeBlockChar & installedCapability.Name & CodeBlockChar & TableSeparator & Casters.CastDismPackageState(installedCapability.State) & TableSeparator & CrLf
                        Next
                        Contents &= CrLf & "- Complete capability information has not been gathered" & CrLf
                    End If
                End Using
            Catch ex As Exception
                Debug.WriteLine("[GetCapabilityInformation] An error occurred while getting capability information: " & ex.ToString() & " - " & ex.Message)
                Contents &= "The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                            "- Exception: " & ex.ToString() & CrLf & _
                            "- Exception message: " & ex.Message & CrLf & _
                            "- Error code: " & Hex(ex.HResult) & CrLf & CrLf
            Finally
                DismApi.Shutdown()
            End Try
        End If
    End Sub

    Sub GetDriverInformation()
        Dim InstalledDrvInfo As DismDriverPackageCollection = Nothing
        Dim msg As String() = New String(3) {"", "", "", ""}
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg(0) = "Preparing driver information processes..."
                        msg(1) = "The program has obtained basic information of the installed drivers of this image. You can also get complete information of such drivers and save it in the report." & CrLf & CrLf & _
                          "Do note that this will take longer depending on the number of installed drivers." & CrLf & CrLf & _
                          "Do you want to get this information and save it in the report?"
                        msg(2) = "Driver information"
                        msg(3) = "You have configured background processes to not detect all drivers, which includes drivers part of the Windows distribution, so you may not see the driver you're interested in." & CrLf & CrLf & _
                      "This setting is also applied to this task, but you can get the information of all drivers now. Do note that this can take a long time, depending on the amount of first-party drivers." & CrLf & CrLf & _
                      "Do you want to get the information of all drivers, including drivers part of the Windows distribution?"
                    Case "ESN"
                        msg(0) = "Preparando procesos de información de controladores..."
                        msg(1) = "El programa ha obtenido información básica de los controladores instalados en esta imagen. También puede obtener información completa de dichos controladores y guardarla en el informe." & CrLf & CrLf & _
                          "Dese cuenta de que esto tardará más, dependiendo del número de controladores instalados." & CrLf & CrLf & _
                          "¿Desea obtener esta información y guardarla en el informe?"
                        msg(2) = "Información de controladores"
                        msg(3) = "Ha configurado los procesos en segundo plano para no detectar todos los controladores, lo que incluye controladores parte de la distribución de Windows, por lo que podría no ver el controlador que le interesa." & CrLf & CrLf & _
                      "Esta configuración también se aplica a esta tarea, pero puede obtener la información de todos los controladores ahora. Dese cuenta de que esto puede llevar mucho tiempo, dependiendo del número de controladores de serie." & CrLf & CrLf & _
                      "¿Desea obtener la información de todos los controladores, incluyendo los controladores que son parte de la distribución de Windows?"
                    Case "FRA"
                        msg(0) = "Préparation des processus d'information sur les pilotes en cours..."
                        msg(1) = "Le programme a obtenu des informations basiques sur les pilotes installés sur cette image. Vous pouvez également obtenir des informations complètes sur ces pilotes et les enregistrer dans le rapport." & CrLf & CrLf & _
                          "Notez que cette opération peut prendre plus de temps en fonction du nombre de pilotes installés." & CrLf & CrLf & _
                          "Souhaitez-vous obtenir ces informations et les enregistrer dans le rapport ?"
                        msg(2) = "Informations sur les pilotes"
                        msg(3) = "Vous avez configuré les processus d'arrière-plan pour qu'ils ne détectent pas tous les pilotes, ce qui inclut les pilotes faisant partie de la distribution Windows, il se peut donc que vous ne voyiez pas le pilote qui vous intéresse." & CrLf & CrLf & _
                      "Ce paramètre est également appliqué à cette tâche, mais vous pouvez obtenir les informations de tous les pilotes maintenant. Notez que cela peut prendre beaucoup de temps, en fonction du nombre de pilotes de première partie." & CrLf & CrLf & _
                      "Voulez-vous obtenir les informations de tous les pilotes, y compris les pilotes faisant partie de la distribution Windows ?"
                    Case "PTB", "PTG"
                        msg(0) = "A preparar processos de informação sobre controladores..."
                        msg(1) = "O programa obteve informações básicas sobre os controladores instalados nesta imagem. Também pode obter informações completas sobre esses controladores e guardá-las no relatório." & CrLf & CrLf & _
                          "Tenha em atenção que isto pode demorar mais tempo dependendo do número de controladores instalados." & CrLf & CrLf & _
                          "Pretende obter esta informação e guardá-la no relatório?"
                        msg(2) = "Informações do controlador"
                        msg(3) = "Configurou os processos em segundo plano para não detectarem todos os controladores, o que inclui controladores que fazem parte da distribuição do Windows, pelo que poderá não ver o controlador em que está interessado." & CrLf & CrLf & _
                      "Esta configuração também é aplicada a esta tarefa, mas pode obter as informações de todos os controladores agora. Tenha em atenção que isto pode demorar muito tempo, dependendo da quantidade de controladores originais." & CrLf & CrLf & _
                      "Pretende obter as informações de todos os controladores, incluindo os controladores que fazem parte da distribuição do Windows?"
                    Case "ITA"
                        msg(0) = "Preparazione dei processi di informazione sui driver..."
                        msg(1) = "Il programma ha ottenuto informazioni elementari sui driver installati su questa immagine. È inoltre possibile ottenere informazioni complete su tali driver e salvarle nel rapporto." & CrLf & CrLf & _
                          "Si noti che questa operazione richiederà più tempo a seconda del numero di driver installati." & CrLf & CrLf & _
                          "Volete ottenere queste informazioni e salvarle nel rapporto?"
                        msg(2) = "Informazioni sul driver"
                        msg(3) = "Avete configurato i processi in background in modo che non rilevino tutti i driver, compresi quelli che fanno parte della distribuzione di Windows, quindi potreste non vedere il driver che vi interessa." & CrLf & CrLf & _
                      "Questa impostazione viene applicata anche a questa attività, ma ora è possibile ottenere le informazioni su tutti i driver. Tenere presente che questa operazione può richiedere molto tempo, a seconda della quantità di driver di prima parte." & CrLf & CrLf & _
                      "Volete ottenere le informazioni su tutti i driver, compresi quelli che fanno parte della distribuzione di Windows?"
                End Select
            Case 1
                msg(0) = "Preparing driver information processes..."
                msg(1) = "The program has obtained basic information of the installed drivers of this image. You can also get complete information of such drivers and save it in the report." & CrLf & CrLf & _
                  "Do note that this will take longer depending on the number of installed drivers." & CrLf & CrLf & _
                  "Do you want to get this information and save it in the report?"
                msg(2) = "Driver information"
                msg(3) = "You have configured background processes to not detect all drivers, which includes drivers part of the Windows distribution, so you may not see the driver you're interested in." & CrLf & CrLf & _
              "This setting is also applied to this task, but you can get the information of all drivers now. Do note that this can take a long time, depending on the amount of first-party drivers." & CrLf & CrLf & _
              "Do you want to get the information of all drivers, including drivers part of the Windows distribution?"
            Case 2
                msg(0) = "Preparando procesos de información de controladores..."
                msg(1) = "El programa ha obtenido información básica de los controladores instalados en esta imagen. También puede obtener información completa de dichos controladores y guardarla en el informe." & CrLf & CrLf & _
                  "Dese cuenta de que esto tardará más, dependiendo del número de controladores instalados." & CrLf & CrLf & _
                  "¿Desea obtener esta información y guardarla en el informe?"
                msg(2) = "Información de controladores"
                msg(3) = "Ha configurado los procesos en segundo plano para no detectar todos los controladores, lo que incluye controladores parte de la distribución de Windows, por lo que podría no ver el controlador que le interesa." & CrLf & CrLf & _
              "Esta configuración también se aplica a esta tarea, pero puede obtener la información de todos los controladores ahora. Dese cuenta de que esto puede llevar mucho tiempo, dependiendo del número de controladores de serie." & CrLf & CrLf & _
              "¿Desea obtener la información de todos los controladores, incluyendo los controladores que son parte de la distribución de Windows?"
            Case 3
                msg(0) = "Préparation des processus d'information sur les pilotes en cours..."
                msg(1) = "Le programme a obtenu des informations basiques sur les pilotes installés sur cette image. Vous pouvez également obtenir des informations complètes sur ces pilotes et les enregistrer dans le rapport." & CrLf & CrLf & _
                  "Notez que cette opération peut prendre plus de temps en fonction du nombre de pilotes installés." & CrLf & CrLf & _
                  "Souhaitez-vous obtenir ces informations et les enregistrer dans le rapport ?"
                msg(2) = "Informations sur les pilotes"
                msg(3) = "Vous avez configuré les processus d'arrière-plan pour qu'ils ne détectent pas tous les pilotes, ce qui inclut les pilotes faisant partie de la distribution Windows, il se peut donc que vous ne voyiez pas le pilote qui vous intéresse." & CrLf & CrLf & _
              "Ce paramètre est également appliqué à cette tâche, mais vous pouvez obtenir les informations de tous les pilotes maintenant. Notez que cela peut prendre beaucoup de temps, en fonction du nombre de pilotes de première partie." & CrLf & CrLf & _
              "Voulez-vous obtenir les informations de tous les pilotes, y compris les pilotes faisant partie de la distribution Windows ?"
            Case 4
                msg(0) = "A preparar processos de informação sobre controladores..."
                msg(1) = "O programa obteve informações básicas sobre os controladores instalados nesta imagem. Também pode obter informações completas sobre esses controladores e guardá-las no relatório." & CrLf & CrLf & _
                  "Tenha em atenção que isto pode demorar mais tempo dependendo do número de controladores instalados." & CrLf & CrLf & _
                  "Pretende obter esta informação e guardá-la no relatório?"
                msg(2) = "Informações do controlador"
                msg(3) = "Configurou os processos em segundo plano para não detectarem todos os controladores, o que inclui controladores que fazem parte da distribuição do Windows, pelo que poderá não ver o controlador em que está interessado." & CrLf & CrLf & _
              "Esta configuração também é aplicada a esta tarefa, mas pode obter as informações de todos os controladores agora. Tenha em atenção que isto pode demorar muito tempo, dependendo da quantidade de controladores originais." & CrLf & CrLf & _
              "Pretende obter as informações de todos os controladores, incluindo os controladores que fazem parte da distribuição do Windows?"
            Case 5
                msg(0) = "Preparazione dei processi di informazione sui driver..."
                msg(1) = "Il programma ha ottenuto informazioni elementari sui driver installati su questa immagine. È inoltre possibile ottenere informazioni complete su tali driver e salvarle nel rapporto." & CrLf & CrLf & _
                  "Si noti che questa operazione richiederà più tempo a seconda del numero di driver installati." & CrLf & CrLf & _
                  "Volete ottenere queste informazioni e salvarle nel rapporto?"
                msg(2) = "Informazioni sul driver"
                msg(3) = "Avete configurato i processi in background in modo che non rilevino tutti i driver, compresi quelli che fanno parte della distribuzione di Windows, quindi potreste non vedere il driver che vi interessa." & CrLf & CrLf & _
              "Questa impostazione viene applicata anche a questa attività, ma ora è possibile ottenere le informazioni su tutti i driver. Tenere presente che questa operazione può richiedere molto tempo, a seconda della quantità di driver di prima parte." & CrLf & CrLf & _
              "Volete ottenere le informazioni su tutti i driver, compresi quelli che fanno parte della distribuzione di Windows?"
        End Select
        If SaveTask = 7 And Not AllDrivers Then
            If MsgBox(msg(3), vbYesNo + vbQuestion, msg(2)) = MsgBoxResult.Yes Then
                AllDrivers = True
            End If
        End If
        Contents &= "## Driver information" & CrLf & CrLf & _
                    "- Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf & _
                    "- In-box driver information " & If(AllDrivers, "was saved", "was not saved") & CrLf & CrLf
        Debug.WriteLine("[GetDriverInformation] Starting task...")
        Try
            Debug.WriteLine("[GetDriverInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetDriverInformation] Creating image session...")
            Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                Debug.WriteLine("[GetDriverInformation] Getting basic driver information...")
                ReportChanges(msg(0), 5)
                InstalledDrvInfo = DismApi.GetDrivers(imgSession, AllDrivers)
                Contents &= "**Information summary for " & InstalledDrvInfo.Count & " driver(s):**" & CrLf & CrLf
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg(0) = "Drivers have been obtained"
                            Case "ESN"
                                msg(0) = "Los controladores han sido obtenidos"
                            Case "FRA"
                                msg(0) = "Des pilotes ont été obtenus"
                            Case "PTB", "PTG"
                                msg(0) = "Os controladores foram obtidos"
                            Case "ITA"
                                msg(0) = "I driver del dispositivo sono stati ottenuti"
                        End Select
                    Case 1
                        msg(0) = "Drivers have been obtained"
                    Case 2
                        msg(0) = "Los controladores han sido obtenidos"
                    Case 3
                        msg(0) = "Des pilotes ont été obtenus"
                    Case 4
                        msg(0) = "Os controladores foram obtidos"
                    Case 5
                        msg(0) = "I driver del dispositivo sono stati ottenuti"
                End Select
                ReportChanges(msg(0), 10)
                If SkipQuestions And AutoCompleteInfo(4) Then
                    Contents &= CrLf & CrLf & FullDriverTableHeader
                    Debug.WriteLine("[GetDriverInformation] Getting complete driver information...")
                    For Each driver As DismDriverPackage In InstalledDrvInfo
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        msg(0) = "Getting information of drivers... (driver " & InstalledDrvInfo.IndexOf(driver) + 1 & " of " & InstalledDrvInfo.Count & ")"
                                    Case "ESN"
                                        msg(0) = "Obteniendo información de controladores... (controlador " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                                    Case "FRA"
                                        msg(0) = "Obtention des informations sur les pilotes en cours... (pilote " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                                    Case "PTB", "PTG"
                                        msg(0) = "Obter informações sobre os controladores... (controlador " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                                    Case "ITA"
                                        msg(0) = "Ottenere informazioni sui driver... (driver " & InstalledDrvInfo.IndexOf(driver) + 1 & " di " & InstalledDrvInfo.Count & ")"
                                End Select
                            Case 1
                                msg(0) = "Getting information of drivers... (driver " & InstalledDrvInfo.IndexOf(driver) + 1 & " of " & InstalledDrvInfo.Count & ")"
                            Case 2
                                msg(0) = "Obteniendo información de controladores... (controlador " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                            Case 3
                                msg(0) = "Obtention des informations sur les pilotes en cours... (pilote " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                            Case 4
                                msg(0) = "Obter informações sobre os controladores... (controlador " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                            Case 5
                                msg(0) = "Ottenere informazioni sui driver... (driver " & InstalledDrvInfo.IndexOf(driver) + 1 & " di " & InstalledDrvInfo.Count & ")"
                        End Select
                        ReportChanges(msg(0), (InstalledDrvInfo.IndexOf(driver) / InstalledDrvInfo.Count) * 100)
                        Dim signer As String = DriverSignerViewer.GetSignerInfo(driver.OriginalFileName)
                        Contents &= TableSeparator & CodeBlockChar & driver.PublishedName & CodeBlockChar & TableSeparator & Path.GetFileName(driver.OriginalFileName) & " (" & Path.GetDirectoryName(driver.OriginalFileName) & ")" & TableSeparator & driver.ProviderName & TableSeparator & driver.ClassName & TableSeparator & driver.ClassDescription & TableSeparator & driver.ClassGuid & TableSeparator & driver.CatalogFile & TableSeparator & If(driver.InBox, "Yes", "No") & TableSeparator & If(driver.BootCritical, "Yes", "No") & TableSeparator & driver.Version.ToString() & TableSeparator & driver.Date & TableSeparator & Casters.CastDismSignatureStatus(driver.DriverSignature) & If(Not (signer Is Nothing OrElse signer = ""), " by " & signer, "") & TableSeparator & CrLf
                    Next
                    Contents &= CrLf & "- Complete driver information has been gathered" & CrLf
                ElseIf (Not SkipQuestions Or Not AutoCompleteInfo(4)) And MsgBox(msg(1), vbYesNo + vbQuestion, msg(2)) = MsgBoxResult.Yes Then
                    Contents &= CrLf & CrLf & FullDriverTableHeader
                    Debug.WriteLine("[GetDriverInformation] Getting complete driver information...")
                    For Each driver As DismDriverPackage In InstalledDrvInfo
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        msg(0) = "Getting information of drivers... (driver " & InstalledDrvInfo.IndexOf(driver) + 1 & " of " & InstalledDrvInfo.Count & ")"
                                    Case "ESN"
                                        msg(0) = "Obteniendo información de controladores... (controlador " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                                    Case "FRA"
                                        msg(0) = "Obtention des informations sur les pilotes en cours... (pilote " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                                    Case "PTB", "PTG"
                                        msg(0) = "Obter informações sobre os controladores... (controlador " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                                    Case "ITA"
                                        msg(0) = "Ottenere informazioni sui driver... (driver " & InstalledDrvInfo.IndexOf(driver) + 1 & " di " & InstalledDrvInfo.Count & ")"
                                End Select
                            Case 1
                                msg(0) = "Getting information of drivers... (driver " & InstalledDrvInfo.IndexOf(driver) + 1 & " of " & InstalledDrvInfo.Count & ")"
                            Case 2
                                msg(0) = "Obteniendo información de controladores... (controlador " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                            Case 3
                                msg(0) = "Obtention des informations sur les pilotes en cours... (pilote " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                            Case 4
                                msg(0) = "Obter informações sobre os controladores... (controlador " & InstalledDrvInfo.IndexOf(driver) + 1 & " de " & InstalledDrvInfo.Count & ")"
                            Case 5
                                msg(0) = "Ottenere informazioni sui driver... (driver " & InstalledDrvInfo.IndexOf(driver) + 1 & " di " & InstalledDrvInfo.Count & ")"
                        End Select
                        ReportChanges(msg(0), (InstalledDrvInfo.IndexOf(driver) / InstalledDrvInfo.Count) * 100)
                        Dim signer As String = DriverSignerViewer.GetSignerInfo(driver.OriginalFileName)
                        Contents &= TableSeparator & CodeBlockChar & driver.PublishedName & CodeBlockChar & TableSeparator & Path.GetFileName(driver.OriginalFileName) & " (" & Path.GetDirectoryName(driver.OriginalFileName) & ")" & TableSeparator & driver.ProviderName & TableSeparator & driver.ClassName & TableSeparator & driver.ClassDescription & TableSeparator & driver.ClassGuid & TableSeparator & driver.CatalogFile & TableSeparator & If(driver.InBox, "Yes", "No") & TableSeparator & If(driver.BootCritical, "Yes", "No") & TableSeparator & driver.Version.ToString() & TableSeparator & driver.Date & TableSeparator & Casters.CastDismSignatureStatus(driver.DriverSignature) & If(Not (signer Is Nothing OrElse signer = ""), " by " & signer, "") & TableSeparator & CrLf
                    Next
                    Contents &= CrLf & "- Complete driver information has been gathered" & CrLf
                Else
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg(0) = "Saving installed drivers..."
                                Case "ESN"
                                    msg(0) = "Guardando controladores instalados..."
                                Case "FRA"
                                    msg(0) = "Sauvegarde des pilotes installés en cours..."
                                Case "PTB", "PTG"
                                    msg(0) = "Guardar os controladores instalados..."
                                Case "ITA"
                                    msg(0) = "Salvataggio dei driver installati..."
                            End Select
                        Case 1
                            msg(0) = "Saving installed drivers..."
                        Case 2
                            msg(0) = "Guardando controladores instalados..."
                        Case 3
                            msg(0) = "Sauvegarde des pilotes installés en cours..."
                        Case 4
                            msg(0) = "Guardar os controladores instalados..."
                        Case 5
                            msg(0) = "Salvataggio dei driver installati..."
                    End Select
                    ReportChanges(msg(0), 50)
                    Contents &= BasicDriverTableHeader
                    For Each installedDriver As DismDriverPackage In InstalledDrvInfo
                        Contents &= TableSeparator & CodeBlockChar & installedDriver.PublishedName & CodeBlockChar & TableSeparator & Path.GetFileName(installedDriver.OriginalFileName) & " (" & Path.GetDirectoryName(installedDriver.OriginalFileName) & ")" & TableSeparator & If(installedDriver.InBox, "Yes", "No") & TableSeparator & installedDriver.ClassName & TableSeparator & installedDriver.ProviderName & TableSeparator & installedDriver.Date & TableSeparator & installedDriver.Version.ToString() & TableSeparator & CrLf
                    Next
                    Contents &= CrLf & "- Complete driver information has not been gathered" & CrLf
                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetDriverInformation] An error occurred while getting driver information: " & ex.ToString() & " - " & ex.Message)
            Contents &= "**The program could not get information about this task. See below for reasons why:**" & CrLf & CrLf & _
                        "- Exception: " & ex.ToString() & CrLf & _
                        "- Exception message: " & ex.Message & CrLf & _
                        "- Error code: " & Hex(ex.HResult) & CrLf & CrLf
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Sub GetDriverFileInformation()
        Dim msg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg = "Preparing driver information processes..."
                    Case "ESN"
                        msg = "Preparando procesos de información de controladores..."
                    Case "FRA"
                        msg = "Préparation des processus d'information des pilotes en cours..."
                    Case "PTB", "PTG"
                        msg = "Preparar os processos de informação dos controladores..."
                    Case "ITA"
                        msg = "Preparazione dei processi di informazione del driver..."
                End Select
            Case 1
                msg = "Preparing driver information processes..."
            Case 2
                msg = "Preparando procesos de información de controladores..."
            Case 3
                msg = "Préparation des processus d'information des pilotes en cours..."
            Case 4
                msg = "Preparar os processos de informação dos controladores..."
            Case 5
                msg = "Preparazione dei processi di informazione del driver..."
        End Select
        Contents &= "## Driver package information" & CrLf & CrLf & _
                    "- Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf & CrLf
        Debug.WriteLine("[GetDriverFileInformation] Starting task...")
        Try
            Debug.WriteLine("[GetDriverFileInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetDriverFileInformation] Creating image session...")
            ReportChanges(msg, 0)
            Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                Contents &= "**Information summary for " & DriverPkgs.Count & " driver package(s):**" & CrLf & CrLf
                For Each drvPkg In DriverPkgs
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "Getting information from driver files... (driver file " & DriverPkgs.IndexOf(drvPkg) + 1 & " of " & DriverPkgs.Count & ")"
                                Case "ESN"
                                    msg = "Obteniendo información de archivos de controladores... (archivo de controlador " & DriverPkgs.IndexOf(drvPkg) + 1 & " de " & DriverPkgs.Count & ")"
                                Case "FRA"
                                    msg = "Obtention des informations des fichiers pilotes en cours... (fichier pilote " & DriverPkgs.IndexOf(drvPkg) + 1 & " de " & DriverPkgs.Count & ")"
                                Case "PTB", "PTG"
                                    msg = "Obter informações dos ficheiros de controladores... (ficheiro de controlador " & DriverPkgs.IndexOf(drvPkg) + 1 & " de " & DriverPkgs.Count & ")"
                                Case "ITA"
                                    msg = "Ottenere informazioni dai file dei driver... (file dei driver " & DriverPkgs.IndexOf(drvPkg) + 1 & " di " & DriverPkgs.Count & ")"
                            End Select
                        Case 1
                            msg = "Getting information from driver files... (driver file " & DriverPkgs.IndexOf(drvPkg) + 1 & " of " & DriverPkgs.Count & ")"
                        Case 2
                            msg = "Obteniendo información de archivos de controladores... (archivo de controlador " & DriverPkgs.IndexOf(drvPkg) + 1 & " de " & DriverPkgs.Count & ")"
                        Case 3
                            msg = "Obtention des informations des fichiers pilotes en cours... (fichier pilote " & DriverPkgs.IndexOf(drvPkg) + 1 & " de " & DriverPkgs.Count & ")"
                        Case 4
                            msg = "Obter informações dos ficheiros de controladores... (ficheiro de controlador " & DriverPkgs.IndexOf(drvPkg) + 1 & " de " & DriverPkgs.Count & ")"
                        Case 5
                            msg = "Ottenere informazioni dai file dei driver... (file dei driver " & DriverPkgs.IndexOf(drvPkg) + 1 & " di " & DriverPkgs.Count & ")"
                    End Select
                    ReportChanges(msg, (DriverPkgs.IndexOf(drvPkg) / DriverPkgs.Count) * 100)
                    If File.Exists(drvPkg) Then
                        Contents &= "### Driver package " & DriverPkgs.IndexOf(drvPkg) + 1 & " of " & DriverPkgs.Count & "" & CrLf & CrLf
                        Dim drvInfoCollection As DismDriverCollection = DismApi.GetDriverInfo(imgSession, drvPkg)
                        If drvInfoCollection.Count > 0 Then
                            Contents &= "**Information summary for " & drvInfoCollection.Count & " hardware target(s):**" & CrLf & CrLf & DriverFileTableHeader
                            For Each hwTarget As DismDriver In drvInfoCollection
                                Select Case MainForm.Language
                                    Case 0
                                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                            Case "ENU", "ENG"
                                                msg = "Getting information from hardware targets... (target " & drvInfoCollection.IndexOf(hwTarget) + 1 & " of " & drvInfoCollection.Count & ")"
                                            Case "ESN"
                                                msg = "Obteniendo información de destinos de hardware... (destino " & drvInfoCollection.IndexOf(hwTarget) + 1 & " de " & drvInfoCollection.Count & ")"
                                            Case "FRA"
                                                msg = "Obtention des informations des matériels cibles en cours... (cible " & drvInfoCollection.IndexOf(hwTarget) + 1 & " de " & drvInfoCollection.Count & ")"
                                            Case "PTB", "PTG"
                                                msg = "Obtendo informações de alvos de hardware... (alvo " & drvInfoCollection.IndexOf(hwTarget) + 1 & " de " & drvInfoCollection.Count & ")"
                                            Case "ITA"
                                                msg = "Ottenere informazioni dai target hardware... (target " & drvInfoCollection.IndexOf(hwTarget) + 1 & " of " & drvInfoCollection.Count & ")"
                                        End Select
                                    Case 1
                                        msg = "Getting information from hardware targets... (target " & drvInfoCollection.IndexOf(hwTarget) + 1 & " of " & drvInfoCollection.Count & ")"
                                    Case 2
                                        msg = "Obteniendo información de destinos de hardware... (destino " & drvInfoCollection.IndexOf(hwTarget) + 1 & " de " & drvInfoCollection.Count & ")"
                                    Case 3
                                        msg = "Obtention des informations des matériels cibles en cours... (cible " & drvInfoCollection.IndexOf(hwTarget) + 1 & " de " & drvInfoCollection.Count & ")"
                                    Case 4
                                        msg = "Obtendo informações de alvos de hardware... (alvo " & drvInfoCollection.IndexOf(hwTarget) + 1 & " de " & drvInfoCollection.Count & ")"
                                    Case 5
                                        msg = "Ottenere informazioni dai target hardware... (target " & drvInfoCollection.IndexOf(hwTarget) + 1 & " of " & drvInfoCollection.Count & ")"
                                End Select
                                ReportChanges(msg, (DriverPkgs.IndexOf(drvPkg) / DriverPkgs.Count) * 100 + (drvInfoCollection.IndexOf(hwTarget) + 1) / drvInfoCollection.Count * 100 / DriverPkgs.Count)
                                Contents &= TableSeparator & hwTarget.HardwareDescription & TableSeparator & hwTarget.HardwareId & TableSeparator & If(hwTarget.CompatibleIds = "", "None declared by the manufacturer", hwTarget.CompatibleIds) & TableSeparator & If(hwTarget.ExcludeIds = "", "None declared by the manufacturer", hwTarget.ExcludeIds) & TableSeparator & hwTarget.ManufacturerName & TableSeparator & Casters.CastDismArchitecture(hwTarget.Architecture) & TableSeparator & CrLf
                            Next
                            Contents &= CrLf
                        Else
                            Contents &= "**This file contains no hardware targets. It could be invalid.**" & CrLf & CrLf
                        End If
                    End If
                Next
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetDriverFileInformation] An error occurred while getting driver information: " & ex.ToString() & " - " & ex.Message)
            Contents &= "The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                        "- Exception: " & ex.ToString() & CrLf & _
                        "- Exception message: " & ex.Message & CrLf & _
                        "- Error code: " & Hex(ex.HResult) & CrLf & CrLf
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Sub GetWinPEConfiguration()
        Dim msg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg = "Preparing to get Windows PE configuration..."
                    Case "ESN"
                        msg = "Preparándonos para obtener la configuración de Windows PE..."
                    Case "FRA"
                        msg = "Préparation de l'obtention de la configuration de Windows PE en cours..."
                    Case "PTB", "PTG"
                        msg = "A preparar para obter a configuração do Windows PE..."
                    Case "ITA"
                        msg = "Preparazione per ottenere la configurazione di Windows PE..."
                End Select
            Case 1
                msg = "Preparing to get Windows PE configuration..."
            Case 2
                msg = "Preparándonos para obtener la configuración de Windows PE..."
            Case 3
                msg = "Préparation de l'obtention de la configuration de Windows PE en cours..."
            Case 4
                msg = "A preparar para obter a configuração do Windows PE..."
            Case 5
                msg = "Preparazione per ottenere la configurazione di Windows PE..."
        End Select
        Contents &= "## Windows PE configuration" & CrLf & CrLf
        If Not MainForm.imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Then
            Contents &= "**This task is not supported on the specified Windows image. Check that it is a Windows PE image.** Skipping task..." & CrLf & CrLf
            Exit Sub
        Else
            Contents &= " - Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf & CrLf
            Debug.WriteLine("[GetWinPEConfiguration] Starting task...")
            Using reg As New Process
                Debug.WriteLine("[GetWinPEConfiguration] Detecting target path...")
                ReportChanges(msg, 0)
                reg.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
                reg.StartInfo.Arguments = "load HKLM\PE_SOFT " & Quote & MainForm.MountDir & "\Windows\system32\config\SOFTWARE" & Quote
                reg.StartInfo.CreateNoWindow = True
                reg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                reg.Start()
                reg.WaitForExit()
                If reg.ExitCode <> 0 Then
                    Contents &= "- Target path: could not get value" & CrLf
                End If
                reg.StartInfo.Arguments = "load HKLM\PE_SYS " & Quote & MainForm.MountDir & "\Windows\system32\config\SYSTEM" & Quote
                reg.Start()
                reg.WaitForExit()
                If reg.ExitCode <> 0 Then
                    Contents &= "- Scratch space: could not get value" & CrLf & CrLf
                    Exit Sub
                End If
                Try
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "Getting Windows PE target path..."
                                Case "ESN"
                                    msg = "Obteniendo la ruta de destino de Windows PE..."
                                Case "FRA"
                                    msg = "Obtention du chemin d'accès cible de Windows PE en cours..."
                                Case "PTB", "PTG"
                                    msg = "Obter a localização do objetivo do Windows PE..."
                                Case "ITA"
                                    msg = "Ottenere il percorso di destinazione di Windows PE..."
                            End Select
                        Case 1
                            msg = "Getting Windows PE target path..."
                        Case 2
                            msg = "Obteniendo la ruta de destino de Windows PE..."
                        Case 3
                            msg = "Obtention du chemin d'accès cible de Windows PE en cours..."
                        Case 4
                            msg = "Obter a localização do objetivo do Windows PE..."
                        Case 5
                            msg = "Ottenere il percorso di destinazione di Windows PE..."
                    End Select
                    ReportChanges(msg, 50)
                    ' Get target path first
                    Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("PE_SOFT\Microsoft\Windows NT\CurrentVersion\WinPE", False)
                    Contents &= "- Target path: " & regKey.GetValue("InstRoot", "could not get value").ToString() & CrLf
                    regKey.Close()
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "Getting Windows PE scratch space..."
                                Case "ESN"
                                    msg = "Obteniendo espacio temporal de Windows PE..."
                                Case "FRA"
                                    msg = "Obtention de l'espace temporaire de Windows PE en cours..."
                                Case "PTB", "PTG"
                                    msg = "A obter espaço temporário do Windows PE..."
                                Case "ITA"
                                    msg = "Ottenere lo spazio temporaneo di Windows PE..."
                            End Select
                        Case 1
                            msg = "Getting Windows PE scratch space..."
                        Case 2
                            msg = "Obteniendo espacio temporal de Windows PE..."
                        Case 3
                            msg = "Obtention de l'espace temporaire de Windows PE en cours..."
                        Case 4
                            msg = "A obter espaço temporário do Windows PE..."
                        Case 5
                            msg = "Ottenere lo spazio temporaneo di Windows PE..."
                    End Select
                    ReportChanges(msg, 75)
                    regKey = Registry.LocalMachine.OpenSubKey("PE_SYS\ControlSet001\Services\FBWF", False)
                    Dim scSize As String = regKey.GetValue("WinPECacheThreshold", "").ToString()
                    Contents &= "- Scratch space: " & If(Not scSize = "", scSize & " MB", "could not get value") & CrLf & CrLf
                    regKey.Close()
                Catch ex As Exception

                End Try
                ' Unload registry hives
                reg.StartInfo.Arguments = "unload HKLM\PE_SOFT"
                reg.Start()
                reg.WaitForExit()
                reg.StartInfo.Arguments = "unload HKLM\PE_SYS"
                reg.Start()
                reg.WaitForExit()
            End Using
        End If
    End Sub

    Private Sub ImgInfoSaveDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not InfoSaveResults.IsDisposed Then
            InfoSaveResults.Close()
            InfoSaveResults.Dispose()
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Visible = True
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Saving image information..."
                        Label1.Text = "Please wait while DISMTools saves the image information to a file. This can take some time, depending on the tasks that are run."
                        Label2.Text = "Please wait..."
                    Case "ESN"
                        Text = "Guardando información de la imagen..."
                        Label1.Text = "Espere mientras DISMTools guarda la información de la imagen en un archivo. Esto puede llevar algo de tiempo, dependiendo de las tareas que son ejecutadas."
                        Label2.Text = "Espere..."
                    Case "FRA"
                        Text = "Sauvegarde des informations sur l'image en cours..."
                        Label1.Text = "Veuillez patienter pendant que DISMTools enregistre l'information sur l'image dans un fichier. Cette opération peut prendre un certain temps, en fonction des tâches exécutées."
                        Label2.Text = "Veuillez patienter..."
                    Case "PTB", "PTG"
                        Text = "Salvando informações da imagem..."
                        Label1.Text = "Aguarde enquanto o DISMTools salva as informações da imagem em um arquivo. Isso pode levar algum tempo, dependendo das tarefas que estão sendo executadas."
                        Label2.Text = "Aguarde..."
                    Case "ITA"
                        Text = "Salvataggio delle informazioni sull'immagine..."
                        Label1.Text = "Attendere che DISMTools salvi le informazioni sull'immagine in un file. Questa operazione può richiedere un certo tempo, a seconda delle attività eseguite."
                        Label2.Text = "Attendere..."
                End Select
            Case 1
                Text = "Saving image information..."
                Label1.Text = "Please wait while DISMTools saves the image information to a file. This can take some time, depending on the tasks that are run."
                Label2.Text = "Please wait..."
            Case 2
                Text = "Guardando información de la imagen..."
                Label1.Text = "Espere mientras DISMTools guarda la información de la imagen en un archivo. Esto puede llevar algo de tiempo, dependiendo de las tareas que son ejecutadas."
                Label2.Text = "Espere..."
            Case 3
                Text = "Sauvegarde des informations sur l'image en cours..."
                Label1.Text = "Veuillez patienter pendant que DISMTools enregistre l'information sur l'image dans un fichier. Cette opération peut prendre un certain temps, en fonction des tâches exécutées."
                Label2.Text = "Veuillez patienter..."
            Case 4
                Text = "Salvando informações da imagem..."
                Label1.Text = "Aguarde enquanto o DISMTools salva as informações da imagem em um arquivo. Isso pode levar algum tempo, dependendo das tarefas que estão sendo executadas."
                Label2.Text = "Aguarde..."
            Case 5
                Text = "Salvataggio delle informazioni sull'immagine..."
                Label1.Text = "Attendere che DISMTools salvi le informazioni sull'immagine in un file. Questa operazione può richiedere un certo tempo, a seconda delle attività eseguite."
                Label2.Text = "Attendere..."
        End Select
        If MainForm.ImgBW.IsBusy Then
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Background processes need to have completed before getting information. We'll wait until they have completed"
                        Case "ESN"
                            msg = "Los procesos en segundo plano deben haber completado antes de obtener información. Esperaremos hasta que hayan completado"
                        Case "FRA"
                            msg = "Les processus en plan doivent être terminés avant d'afficher l'information. Nous attendrons qu'ils soient terminés"
                        Case "PTB", "PTG"
                            msg = "Os processos em segundo plano têm de estar concluídos antes de obter informações. Vamos esperar até que estejam concluídos"
                        Case "ITA"
                            msg = "I processi in background devono essere completati prima di ottenere informazioni. Aspetteremo che siano completati"
                    End Select
                Case 1
                    msg = "Background processes need to have completed before getting information. We'll wait until they have completed"
                Case 2
                    msg = "Los procesos en segundo plano deben haber completado antes de obtener información. Esperaremos hasta que hayan completado"
                Case 3
                    msg = "Les processus en plan doivent être terminés avant d'afficher l'information. Nous attendrons qu'ils soient terminés"
                Case 4
                    msg = "Os processos em segundo plano têm de estar concluídos antes de obter informações. Vamos esperar até que estejam concluídos"
                Case 5
                    msg = "I processi in background devono essere completati prima di ottenere informazioni. Aspetteremo che siano completati"
            End Select
            MsgBox(msg, vbOKOnly + vbInformation, Text)
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label2.Text = "Waiting for background processes to finish..."
                        Case "ESN"
                            Label2.Text = "Esperando a que terminen los procesos en segundo plano..."
                        Case "FRA"
                            Label2.Text = "Attente de la fin des processus en arrière plan..."
                        Case "PTB", "PTG"
                            Label2.Text = "A aguardar que os processos em segundo plano terminem..."
                        Case "ITA"
                            Label2.Text = "In attesa che i processi in background finiscano..."
                    End Select
                Case 1
                    Label2.Text = "Waiting for background processes to finish..."
                Case 2
                    Label2.Text = "Esperando a que terminen los procesos en segundo plano..."
                Case 3
                    Label2.Text = "Attente de la fin des processus en arrière plan..."
                Case 4
                    Label2.Text = "A aguardar que os processos em segundo plano terminem..."
                Case 5
                    Label2.Text = "In attesa che i processi in background finiscano..."
            End Select
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.Indeterminate, MainForm.Handle)
            While MainForm.ImgBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(500)
            End While
        End If

        ' Stop the mounted image detector, as it makes the program crash when performing DISM API operations
        If MainForm.MountedImageDetectorBW.IsBusy Then
            MainForm.MountedImageDetectorBW.CancelAsync()
            While MainForm.MountedImageDetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(500)
            End While
        End If
        MainForm.WatcherTimer.Enabled = False
        If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
        While MainForm.WatcherBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While

        ' Create the target if it doesn't exist
        If Not File.Exists(SaveTarget) Then
            Try
                File.WriteAllText(SaveTarget, String.Empty)
            Catch ex As Exception
                Dim msg As String() = New String(1) {"", ""}
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg(0) = "Could not create the save target. Reason: "
                                msg(1) = "The operation has failed"
                            Case "ESN"
                                msg(0) = "No se pudo crear el informe de destino. Razón: "
                                msg(1) = "La operación ha fallado"
                            Case "FRA"
                                msg(0) = "Impossible de créer le fichier cible. Raison : "
                                msg(1) = "L'opération a échoué"
                            Case "PTB", "PTG"
                                msg(0) = "Não foi possível criar o destino de gravação. Motivo: "
                                msg(1) = "A operação falhou"
                            Case "ITA"
                                msg(0) = "Impossibile creare la destinazione di salvataggio. Motivo: "
                                msg(1) = "L'operazione non è riuscita"
                        End Select
                    Case 1
                        msg(0) = "Could not create the save target. Reason: "
                        msg(1) = "The operation has failed"
                    Case 2
                        msg(0) = "No se pudo crear el informe de destino. Razón: "
                        msg(1) = "La operación ha fallado"
                    Case 3
                        msg(0) = "Impossible de créer le fichier cible. Raison : "
                        msg(1) = "L'opération a échoué"
                    Case 4
                        msg(0) = "Não foi possível criar o destino de gravação. Motivo: "
                        msg(1) = "A operação falhou"
                    Case 5
                        msg(0) = "Impossibile creare la destinazione di salvataggio. Motivo: "
                        msg(1) = "L'operazione non è riuscita"
                End Select
                MsgBox(msg(0) & ex.ToString() & ": " & ex.Message, vbOKOnly + vbCritical, msg(1))
                Exit Sub
            End Try
        End If

        ' Set the beginning of the contents
        Contents = "# DISMTools Image Information Report" & CrLf & CrLf & _
                   "This is an automatically generated report created by DISMTools. It can be viewed at any time to check image information." & CrLf & CrLf & _
                   "This report contains information about the tasks that you wanted to get information about, which are reflected below this message." & CrLf & CrLf & _
                   "This process primarily uses the DISM API to get information. If you want to get information of the API operations, this file does not include it. However, you can get that information from the log file stored in the standard location of: " & Quote & Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\logs\DISM\DISM.log" & Quote & CrLf & CrLf & _
                   "## Task details" & CrLf & _
                   "- Processes started at: " & Date.Now & CrLf & _
                   "- Report file target: " & Quote & SaveTarget & Quote & CrLf & CrLf

        If OfflineMode Then SourceImage = ImgMountDir

        ' Begin performing operations
        Select Case SaveTask
            Case 0
                Contents &= "- Information tasks: get complete image information" & CrLf & CrLf
                GetImageInformation()
                GetPackageInformation()
                GetFeatureInformation()
                GetAppxInformation()
                GetCapabilityInformation()
                GetDriverInformation()
                GetWinPEConfiguration()
            Case 1
                Contents &= "- Information tasks: get image file information" & CrLf & CrLf
                GetImageInformation()
            Case 2
                Contents &= "- Information tasks: get installed package information" & CrLf & CrLf
                GetPackageInformation()
            Case 3
                Contents &= "- Information tasks: get package file information" & CrLf & CrLf
                GetPackageFileInformation()
            Case 4
                Contents &= "- Information tasks: get feature information" & CrLf & CrLf
                GetFeatureInformation()
            Case 5
                Contents &= "- Information tasks: get installed AppX package information" & CrLf & CrLf
                GetAppxInformation()
            Case 6
                Contents &= "- Information tasks: get capability information" & CrLf & CrLf
                GetCapabilityInformation()
            Case 7
                Contents &= "- Information tasks: get installed driver information" & CrLf & CrLf
                GetDriverInformation()
            Case 8
                Contents &= "- Information tasks: get driver package information" & CrLf & CrLf
                GetDriverFileInformation()
            Case 9
                Contents &= "- Information tasks: get Windows PE configuration" & CrLf & CrLf
                GetWinPEConfiguration()
        End Select

        ' Put an ending to the contents
        Contents &= CrLf & CrLf & "## We have ended at " & Date.Now & ". Have a nice day!"

        ' Inform user that we are saving the file
        Dim saveMsg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        saveMsg = "Saving contents..."
                    Case "ESN"
                        saveMsg = "Guardando contenidos..."
                    Case "FRA"
                        saveMsg = "Sauvegarde des contenus en cours..."
                    Case "PTB", "PTG"
                        saveMsg = "A guardar o conteúdo..."
                    Case "ITA"
                        saveMsg = "Salvataggio dei contenuti..."
                End Select
            Case 1
                saveMsg = "Saving contents..."
            Case 2
                saveMsg = "Guardando contenidos..."
            Case 3
                saveMsg = "Sauvegarde des contenus en cours..."
            Case 4
                saveMsg = "A guardar o conteúdo..."
            Case 5
                saveMsg = "Salvataggio dei contenuti..."
        End Select
        ReportChanges(saveMsg, ProgressBar1.Maximum)
        TaskbarHelper.SetIndicatorState(ProgressBar1.Maximum, Windows.Shell.TaskbarItemProgressState.None, MainForm.Handle)

        ' Save the file
        If Contents <> "" And File.Exists(SaveTarget) Then File.WriteAllText(SaveTarget, Contents, UTF8)
        If Debugger.IsAttached Then Process.Start(SaveTarget)
        InfoSaveResults.FilePath = SaveTarget
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
        MainForm.WatcherTimer.Enabled = True
        Close()
    End Sub
End Class
