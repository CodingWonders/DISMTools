Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Dism
Imports System.Threading
Imports DISMTools.Utilities

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

    ' The file to save the information to
    Public SaveTarget As String

    ' The contents the target file will have
    Public Contents As String

    Sub ReportChanges(Message As String, ProgressPercentage As Double)
        Label2.Text = Message
        ProgressBar1.Value = ProgressPercentage
        Application.DoEvents()
    End Sub

    Sub GetImageInformation()
        Dim ImageInfoCollection As DismImageInfoCollection = Nothing
        Dim ImageInfoList As New List(Of DismImageInfo)
        If ImageInfoList.Count <> 0 Then ImageInfoList.Clear()
        Contents &= "----> Image information" & CrLf & CrLf
        If OnlineMode Then
            Contents &= "  Active installation information:" & CrLf & _
                        "    - Name: " & My.Computer.Info.OSFullName & CrLf & _
                        "    - Boot point (mount point): " & Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & CrLf & _
                        "    - Version: " & Environment.OSVersion.Version.Major & "." & Environment.OSVersion.Version.Minor & "." & Environment.OSVersion.Version.Build & "." & FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe").ProductPrivatePart & CrLf & CrLf
            Exit Sub
        End If
        Contents &= " - Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "")
        Debug.WriteLine("[GetImageInformation] Starting task...")
        Try
            Debug.WriteLine("[GetImageInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetImageInformation] Populating info collection...")
            ImageInfoCollection = DismApi.GetImageInfo(SourceImage)
            Debug.WriteLine("[GetImageInformation] Information processes completed for the image. Obtained images: " & ImageInfoCollection.Count)
            Contents &= CrLf & CrLf & _
                        "  Getting information of " & ImageInfoCollection.Count & " images..." & CrLf & CrLf
            Debug.WriteLine("[GetImageInformation] Exporting information to contents...")
            For Each ImageInfo As DismImageInfo In ImageInfoCollection
                ReportChanges("Getting image information... (image " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " of " & ImageInfoCollection.Count & ")", (ImageInfoCollection.IndexOf(ImageInfo) / ImageInfoCollection.Count) * 100)
                Contents &= "  Image " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & ":" & CrLf & _
                            "    - Version: " & ImageInfo.ProductVersion.ToString() & CrLf & _
                            "    - Image name: " & Quote & ImageInfo.ImageName & Quote & CrLf & _
                            "    - Image description: " & Quote & ImageInfo.ImageDescription & Quote & CrLf & _
                            "    - Image size: " & ImageInfo.ImageSize.ToString("N0") & " bytes (~" & Converters.BytesToReadableSize(ImageInfo.ImageSize) & ")" & CrLf & _
                            "    - Architecture: " & Casters.CastDismArchitecture(ImageInfo.Architecture) & CrLf & _
                            "    - HAL: " & If(ImageInfo.Hal <> "", ImageInfo.Hal, "undefined by the image") & CrLf & _
                            "    - Service Pack build: " & ImageInfo.ProductVersion.Revision & CrLf & _
                            "    - Service Pack level: " & ImageInfo.SpLevel & CrLf & _
                            "    - Installation type: " & ImageInfo.InstallationType & CrLf & _
                            "    - Edition: " & ImageInfo.EditionId & CrLf & _
                            "    - Product type: " & ImageInfo.ProductType & CrLf & _
                            "    - Product suite: " & ImageInfo.ProductSuite & CrLf & _
                            "    - System root directory: " & ImageInfo.SystemRoot & CrLf & _
                            "    - Languages:" & CrLf
                For Each language In ImageInfo.Languages
                    Contents &= "      - " & language.DisplayName & If(ImageInfo.DefaultLanguage.Name = language.Name, " (default)", "") & CrLf
                Next
                Contents &= "    - Dates:" & CrLf & _
                            "      - Created: " & ImageInfo.CustomizedInfo.CreatedTime & CrLf & _
                            "      - Modified: " & ImageInfo.CustomizedInfo.ModifiedTime & CrLf & CrLf
            Next
        Catch ex As Exception
            Debug.WriteLine("[GetImageInformation] An error occurred while getting image information: " & ex.ToString() & " - " & ex.Message)
            Contents &= "  The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                        "  - Exception: " & ex.ToString() & CrLf & _
                        "  - Exception message: " & ex.Message & CrLf & _
                        "  - Error code: " & Hex(ex.HResult) & CrLf & CrLf
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Sub GetPackageInformation()
        'Dim PackageInfoExList As New List(Of DismPackageInfoEx)
        'Dim PackageInfoList As New List(Of DismPackageInfo)
        Dim InstalledPkgInfo As DismPackageCollection = Nothing
        Contents &= "----> Package information" & CrLf & CrLf & _
                    " - Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf & CrLf
        Debug.WriteLine("[GetPackageInformation] Starting task...")
        Try
            Debug.WriteLine("[GetPackageInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetPackageInformation] Creating image session...")
            ReportChanges("Preparing package information processes...", 0)
            Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                Debug.WriteLine("[GetPackageInformation] Getting basic package information...")
                ReportChanges("Getting installed packages...", 5)
                InstalledPkgInfo = DismApi.GetPackages(imgSession)
                Contents &= "  Installed packages in this image: " & InstalledPkgInfo.Count & CrLf & CrLf
                ReportChanges("Packages have been obtained", 10)
                If SaveTask = 0 Then
                    If MsgBox("The program has obtained basic information of the installed packages of this image. You can also get complete information of such packages and save it in the report." & CrLf & CrLf & _
                              "Do note that this will take longer depending on the number of installed packages." & CrLf & CrLf & _
                              "Do you want to get this information and save it in the report?", vbYesNo + vbQuestion, "Package information") = MsgBoxResult.Yes Then
                        Debug.WriteLine("[GetPackageInformation] Getting complete package information...")
                        For Each installedPackage As DismPackage In InstalledPkgInfo
                            ReportChanges("Getting information of package... (package " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " of " & InstalledPkgInfo.Count & ")", (InstalledPkgInfo.IndexOf(installedPackage) / InstalledPkgInfo.Count) * 100)
                            Dim pkgInfoEx As DismPackageInfoEx = Nothing
                            Dim pkgInfo As DismPackageInfo = Nothing
                            Dim cProps As DismCustomPropertyCollection = Nothing

                            ' Determine Windows version, as capability identity information can't be obtained in Windows versions older than 10
                            If Environment.OSVersion.Version.Major >= 10 Then
                                pkgInfoEx = DismApi.GetPackageInfoExByName(imgSession, installedPackage.PackageName)
                            Else
                                pkgInfo = DismApi.GetPackageInfoByName(imgSession, installedPackage.PackageName)
                            End If
                            Contents &= "  Package " & InstalledPkgInfo.IndexOf(installedPackage) + 1 & " of " & InstalledPkgInfo.Count & ":" & CrLf
                            If pkgInfoEx IsNot Nothing Then
                                cProps = pkgInfoEx.CustomProperties
                                Contents &= "    - Package name: " & pkgInfoEx.PackageName & CrLf & _
                                            "    - Is applicable? " & Casters.CastDismApplicabilityStatus(pkgInfoEx.Applicable) & CrLf & _
                                            "    - Copyright: " & pkgInfoEx.Copyright & CrLf & _
                                            "    - Company: " & pkgInfoEx.Company & CrLf & _
                                            "    - Creation time: " & pkgInfoEx.CreationTime & CrLf & _
                                            "    - Description: " & pkgInfoEx.Description & CrLf & _
                                            "    - Install client: " & pkgInfoEx.InstallClient & CrLf & _
                                            "    - Install package name: " & pkgInfoEx.InstallPackageName & CrLf & _
                                            "    - Install time: " & pkgInfoEx.InstallTime & CrLf & _
                                            "    - Last update time: " & pkgInfoEx.LastUpdateTime & CrLf & _
                                            "    - Display name: " & pkgInfoEx.DisplayName & CrLf & _
                                            "    - Product name: " & pkgInfoEx.ProductName & CrLf & _
                                            "    - Product version: " & pkgInfoEx.ProductVersion.ToString() & CrLf & _
                                            "    - Release type: " & Casters.CastDismReleaseType(pkgInfoEx.ReleaseType) & CrLf & _
                                            "    - Is a restart required? " & Casters.CastDismRestartType(pkgInfoEx.RestartRequired) & CrLf & _
                                            "    - Support information: " & pkgInfoEx.SupportInformation & CrLf & _
                                            "    - Package state: " & Casters.CastDismPackageState(pkgInfoEx.PackageState) & CrLf & _
                                            "    - Is a boot up required for full installation? " & Casters.CastDismFullyOfflineInstallationType(pkgInfoEx.FullyOffline) & CrLf & _
                                            "    - Capability identity: " & pkgInfoEx.CapabilityId & CrLf & _
                                            "    - Custom properties: " & If(cProps.Count <= 0, "none", "") & CrLf
                                If cProps.Count > 0 Then
                                    For Each cProp As DismCustomProperty In cProps
                                        Contents &= "      - " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
                                    Next
                                End If
                                Contents &= "    - Features: " & If(pkgInfoEx.Features.Count <= 0, "none", "") & CrLf
                                If pkgInfoEx.Features.Count > 0 Then
                                    Dim pkgFeats As DismFeatureCollection = pkgInfoEx.Features
                                    For Each pkgFeat As DismFeature In pkgFeats
                                        Contents &= "      - " & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & CrLf
                                    Next
                                End If
                                Contents &= CrLf & CrLf
                            ElseIf pkgInfo IsNot Nothing Then
                                cProps = pkgInfo.CustomProperties
                                Contents &= "    - Package name: " & pkgInfo.PackageName & CrLf & _
                                            "    - Is applicable? " & Casters.CastDismApplicabilityStatus(pkgInfo.Applicable) & CrLf & _
                                            "    - Copyright: " & pkgInfo.Copyright & CrLf & _
                                            "    - Company: " & pkgInfo.Company & CrLf & _
                                            "    - Creation time: " & pkgInfo.CreationTime & CrLf & _
                                            "    - Description: " & pkgInfo.Description & CrLf & _
                                            "    - Install client: " & pkgInfo.InstallClient & CrLf & _
                                            "    - Install package name: " & pkgInfo.InstallPackageName & CrLf & _
                                            "    - Install time: " & pkgInfo.InstallTime & CrLf & _
                                            "    - Last update time: " & pkgInfo.LastUpdateTime & CrLf & _
                                            "    - Display name: " & pkgInfo.DisplayName & CrLf & _
                                            "    - Product name: " & pkgInfo.ProductName & CrLf & _
                                            "    - Product version: " & pkgInfo.ProductVersion.ToString() & CrLf & _
                                            "    - Release type: " & Casters.CastDismReleaseType(pkgInfo.ReleaseType) & CrLf & _
                                            "    - Is a restart required? " & Casters.CastDismRestartType(pkgInfo.RestartRequired) & CrLf & _
                                            "    - Support information: " & pkgInfo.SupportInformation & CrLf & _
                                            "    - Package state: " & Casters.CastDismPackageState(pkgInfo.PackageState) & CrLf & _
                                            "    - Is a boot up required for full installation? " & Casters.CastDismFullyOfflineInstallationType(pkgInfo.FullyOffline) & CrLf & _
                                            "    - Capability identity: not applicable (the installation this information was obtained with can't get capability information)" & CrLf & _
                                            "    - Custom properties: " & If(cProps.Count <= 0, "none", "") & CrLf
                                If cProps.Count > 0 Then
                                    For Each cProp As DismCustomProperty In cProps
                                        Contents &= "      - " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
                                    Next
                                End If
                                Contents &= "    - Features: " & If(pkgInfo.Features.Count <= 0, "none", "") & CrLf
                                If pkgInfo.Features.Count > 0 Then
                                    Dim pkgFeats As DismFeatureCollection = pkgInfo.Features
                                    For Each pkgFeat As DismFeature In pkgFeats
                                        Contents &= "      - " & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & CrLf
                                    Next
                                End If
                                Contents &= CrLf & CrLf
                            End If
                        Next
                        Contents &= "  - Complete package information has been gathered" & CrLf & CrLf
                    Else
                        ReportChanges("Saving installed packages...", 50)
                        Contents &= "  - Complete package information has not been gathered" & CrLf & CrLf & _
                                    "  Installed packages in this image:" & CrLf
                        For Each installedPackage As DismPackage In InstalledPkgInfo
                            Contents &= "  - Package name: " & installedPackage.PackageName & CrLf & _
                                        "  - Package state: " & Casters.CastDismPackageState(installedPackage.PackageState) & CrLf & _
                                        "  - Release type: " & Casters.CastDismReleaseType(installedPackage.ReleaseType) & CrLf & _
                                        "  - Install time: " & installedPackage.InstallTime & CrLf & CrLf
                        Next
                    End If
                ElseIf SaveTask <> 3 Then

                Else

                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetPackageInformation] An error occurred while getting package information: " & ex.ToString() & " - " & ex.Message)
            Contents &= "  The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                        "  - Exception: " & ex.ToString() & CrLf & _
                        "  - Exception message: " & ex.Message & CrLf & _
                        "  - Error code: " & Hex(ex.HResult) & CrLf & CrLf
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Sub GetFeatureInformation()
        Dim InstalledFeatInfo As DismFeatureCollection = Nothing
        Contents &= "----> Feature information" & CrLf & CrLf & _
                    " - Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf & CrLf
        Debug.WriteLine("[GetFeatureInformation] Starting task...")
        Try
            Debug.WriteLine("[GetFeatureInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetFeatureInformation] Creating image session...")
            ReportChanges("Preparing feature information processes...", 0)
            Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                Debug.WriteLine("[GetFeatureInformation] Getting basic feature information...")
                ReportChanges("Getting installed features...", 5)
                InstalledFeatInfo = DismApi.GetFeatures(imgSession)
                Contents &= "  Installed features in this image: " & InstalledFeatInfo.Count & CrLf & CrLf
                ReportChanges("Features have been obtained", 10)
                If SaveTask = 0 Then
                    If MsgBox("The program has obtained basic information of the installed features of this image. You can also get complete information of such features and save it in the report." & CrLf & CrLf & _
                              "Do note that this will take longer depending on the number of installed features." & CrLf & CrLf & _
                              "Do you want to get this information and save it in the report?", vbYesNo + vbQuestion, "Feature information") = MsgBoxResult.Yes Then
                        Debug.WriteLine("[GetFeatureInformation] Getting complete feature information...")
                        For Each feature As DismFeature In InstalledFeatInfo
                            ReportChanges("Getting information of feature... (feature " & InstalledFeatInfo.IndexOf(feature) + 1 & " of " & InstalledFeatInfo.Count & ")", (InstalledFeatInfo.IndexOf(feature) / InstalledFeatInfo.Count) * 100)
                            Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, feature.FeatureName)
                            Contents &= "  Feature " & InstalledFeatInfo.IndexOf(feature) + 1 & " of " & InstalledFeatInfo.Count & ":" & CrLf
                            Dim cProps As DismCustomPropertyCollection = featInfo.CustomProperties
                            Contents &= "    - Feature name: " & featInfo.FeatureName & CrLf & _
                                        "    - Display name: " & featInfo.DisplayName & CrLf & _
                                        "    - Description: " & featInfo.Description & CrLf & _
                                        "    - Is a restart required? " & Casters.CastDismRestartType(featInfo.RestartRequired) & CrLf & _
                                        "    - State: " & Casters.CastDismFeatureState(featInfo.FeatureState) & CrLf & _
                                            "    - Custom properties: " & If(cProps.Count <= 0, "none", "") & CrLf
                            If cProps.Count > 0 Then
                                For Each cProp As DismCustomProperty In cProps
                                    Contents &= "      - " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
                                Next
                            End If
                            Contents &= CrLf & CrLf
                        Next
                        Contents &= "  - Complete feature information has been gathered" & CrLf & CrLf
                    Else
                        ReportChanges("Saving installed features...", 50)
                        Contents &= "  - Complete feature information has not been gathered" & CrLf & CrLf & _
                                    "  Installed features in this image:" & CrLf
                        For Each installedFeature As DismFeature In InstalledFeatInfo
                            Contents &= "  - Feature name: " & installedFeature.FeatureName & CrLf & _
                                        "  - Feature state: " & Casters.CastDismPackageState(installedFeature.State) & CrLf & CrLf
                        Next
                    End If
                Else

                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetFeatureInformation] An error occurred while getting feature information: " & ex.ToString() & " - " & ex.Message)
            Contents &= "  The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                        "  - Exception: " & ex.ToString() & CrLf & _
                        "  - Exception message: " & ex.Message & CrLf & _
                        "  - Error code: " & Hex(ex.HResult) & CrLf & CrLf
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Sub GetAppxInformation()
        ' This is a work in progress. Do not call this procedure until this message has been removed
        Dim InstalledAppxPackageInfo As DismAppxPackageCollection = Nothing
        Contents &= "----> AppX package information" & CrLf & CrLf & _
                    " - Image file to get information from: " & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "active installation") & CrLf & CrLf
        ' Detect if the image is Windows 8 or later. If not, skip this task
        If (Not OnlineMode And (Not MainForm.IsWindows8OrHigher(ImgMountDir & "\Windows\system32\ntoskrnl.exe") Or MainForm.imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase))) Or (OnlineMode And Not MainForm.IsWindows8OrHigher(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe")) Then
            Contents &= "    This task is not supported on the specified Windows image. Check that it contains Windows 8 or a later Windows version, and that it isn't a Windows PE image. Skipping task..." & CrLf & CrLf
            Exit Sub
        Else
            Debug.WriteLine("[GetAppxInformation] Starting task...")
            Try
                ' Windows 8 can't get this information with the API. Use the MainForm arrays
                If Environment.OSVersion.Version.Major < 10 Then

                Else
                    Debug.WriteLine("[GetAppxInformation] Starting API...")
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Debug.WriteLine("[GetAppxInformation] Creating image session...")
                    ReportChanges("Preparing AppX package information processes...", 0)
                    Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                        Debug.WriteLine("[GetAppxInformation] Getting basic AppX package information...")
                        ReportChanges("Getting installed AppX packages...", 5)
                        InstalledAppxPackageInfo = DismApi.GetProvisionedAppxPackages(imgSession)
                        Contents &= "  Installed AppX packages in this image: " & InstalledAppxPackageInfo.Count & CrLf & CrLf
                        ReportChanges("AppX packages have been obtained", 10)
                        If SaveTask = 0 Then
                            If MsgBox("The program has obtained basic information of the installed packages of this image. You can also get complete information of such packages and save it in the report." & CrLf & CrLf & _
                              "Do note that this will take longer depending on the number of installed packages." & CrLf & CrLf & _
                              "Do you want to get this information and save it in the report?", vbYesNo + vbQuestion, "Package information") = MsgBoxResult.Yes Then

                            Else

                            End If
                        End If
                    End Using
                End If
            Catch ex As Exception
                Debug.WriteLine("[GetAppxInformation] An error occurred while getting AppX package information: " & ex.ToString() & " - " & ex.Message)
                Contents &= "  The program could not get information about this task. See below for reasons why:" & CrLf & CrLf & _
                            "  - Exception: " & ex.ToString() & CrLf & _
                            "  - Exception message: " & ex.Message & CrLf & _
                            "  - Error code: " & Hex(ex.HResult) & CrLf & CrLf
            Finally
                DismApi.Shutdown()
            End Try
        End If
    End Sub

    Private Sub ImgInfoSaveDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
                        'Text = "Downloading application package..."
                        'Label1.Text = "Please wait while DISMTools downloads the application package to add it to this image. This can take some time, depending on your network connection speed."
                        Label2.Text = "Please wait..."
                    Case "ESN"
                        'Text = "Descargando paquete de aplicación..."
                        'Label1.Text = "Espere mientras DISMTools descarga el paquete de aplicación para añadirlo a esta imagen. Esto puede llevar algo de tiempo, dependiendo de la velocidad de su conexión de red."
                        Label2.Text = "Espere..."
                    Case "FRA"
                        'Text = "Téléchargement du paquet de l'application en cours..."
                        'Label1.Text = "Veuillez patienter pendant que DISMTools télécharge le paquet d'application pour l'ajouter à cette image. Cela peut prendre un certain temps, en fonction de la vitesse de votre connexion réseau."
                        Label2.Text = "Veuillez patienter..."
                End Select
            Case 1
                'Text = "Downloading application package..."
                'Label1.Text = "Please wait while DISMTools downloads the application package to add it to this image. This can take some time, depending on your network connection speed."
                Label2.Text = "Please wait..."
            Case 2
                'Text = "Descargando paquete de aplicación..."
                'Label1.Text = "Espere mientras DISMTools descarga el paquete de aplicación para añadirlo a esta imagen. Esto puede llevar algo de tiempo, dependiendo de la velocidad de su conexión de red."
                Label2.Text = "Espere..."
            Case 3
                'Text = "Téléchargement du paquet de l'application en cours..."
                'Label1.Text = "Veuillez patienter pendant que DISMTools télécharge le paquet d'application pour l'ajouter à cette image. Cela peut prendre un certain temps, en fonction de la vitesse de votre connexion réseau."
                Label2.Text = "Veuillez patienter..."
        End Select

        ' Stop the mounted image detector, as it makes the program crash when performing DISM API operations
        If MainForm.MountedImageDetectorBW.IsBusy Then
            MainForm.MountedImageDetectorBW.CancelAsync()
            While MainForm.MountedImageDetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(500)
            End While
        End If

        ' Create the target if it doesn't exist
        If Not File.Exists(SaveTarget) Then
            Try
                File.WriteAllText(SaveTarget, String.Empty)
            Catch ex As Exception
                MsgBox("Could not create the save target. Reason: " & ex.ToString() & ": " & ex.Message, vbOKOnly + vbCritical, "The operation has failed")
                Exit Sub
            End Try
        End If

        ' Set the beginning of the contents
        Contents = "================ DISMTools Image Information Report ================" & CrLf & _
                   "This is an automatically generated report created by DISMTools. It" & CrLf & _
                   "can be viewed at any time to check image information." & CrLf & CrLf & _
                   "This report contains information about the tasks that you wanted to" & CrLf & _
                   "get information about, which are reflected below this message." & CrLf & CrLf & _
                   "This process primarily uses the DISM API to get information. If you" & CrLf & _
                   "want to get information of the API operations, this file does not" & CrLf & _
                   "include it. However, you can get that information from the log file" & CrLf & _
                   "stored in the standard location of:" & CrLf & _
                   "    " & Quote & Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\logs\DISM\DISM.log" & Quote & "    " & CrLf & _
                   "====================================================================" & CrLf & CrLf & _
                   " - Processes started at: " & Date.Now & CrLf & _
                   " - Report file target: " & Quote & SaveTarget & Quote & CrLf

        ' Begin performing operations
        Select Case SaveTask
            Case 0
                Contents &= " - Information tasks: get complete image information" & CrLf & CrLf
                GetImageInformation()
                GetPackageInformation()
                GetFeatureInformation()
                'GetAppxInformation()
        End Select

        ' Save the file
        If Contents <> "" And File.Exists(SaveTarget) Then File.WriteAllText(SaveTarget, Contents, UTF8)
        If Debugger.IsAttached Then Process.Start(SaveTarget)
        Close()
    End Sub
End Class
