Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Dism
Imports System.Threading
Imports DISMTools.Utilities
Imports Microsoft.Win32
Imports System.Threading.Tasks

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
    ' - 10, to save service information from the default control set
    Public SaveTask As Integer

    Public ImageToGetInfoFrom As WindowsImage

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

    Const CodeBlockChar As String = " ` "       ' It is " ` " to prevent Markdig problem "Markdown elements in the input are too deeply nested - depth limit exceeded. Input is most likely not sensible or is a very large table."

    Dim OSVer As Version

    Private Sub ReportChanges(Message As String, ProgressPercentage As Double)
        Label2.Text = Message
        ProgressBar1.Value = ProgressPercentage
        TaskbarHelper.SetIndicatorState(ProgressPercentage, Windows.Shell.TaskbarItemProgressState.Normal, MainForm.Handle)
    End Sub

    Private Sub WriteExceptionInfo(ex As Exception)
        Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Get.Message")) & CrLf &
            GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Exception.Label") & ex.ToString(),
                                       LocalizationService.ForSection("ImageInfoSave.Report")("ExceptionMessage") & ex.Message,
                                       LocalizationService.ForSection("ImageInfoSave.Report")("ErrorCode.Label") & Hex(ex.HResult) & CrLf & CrLf}.
                                   ToList())
    End Sub

    Private Sub GetImageInformation()
        Dim ImageInfoCollection As DismImageInfoCollection = Nothing
        Dim ImageInfoList As New List(Of DismImageInfo)
        If ImageInfoList.Count <> 0 Then ImageInfoList.Clear()
        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("ImageInfo.Label"), HeaderSize.Header2) & CrLf
        If OnlineMode Then
            Dim revisionNumber As Integer
            Try
                Dim ubrRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
                revisionNumber = ubrRk.GetValue("UBR")
                ubrRk.Close()
            Catch ex As Exception
                revisionNumber = FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe").ProductPrivatePart
            End Try

            Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("Active.Install.Label"), HeaderSize.Header3) & CrLf &
                GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Name.Label") & My.Computer.Info.OSFullName,
                                           LocalizationService.ForSection("ImageInfoSave.Report")("Boot.Point.Mount.Label") & Environment.GetEnvironmentVariable("SYSTEMDRIVE"),
                                           LocalizationService.ForSection("ImageInfoSave.Report")("Version.Label") & Environment.OSVersion.Version.Major & "." & Environment.OSVersion.Version.Minor & "." & Environment.OSVersion.Version.Build & "." & revisionNumber}.
                                       ToList()) & CrLf
            Exit Sub
        ElseIf OfflineMode Then
            Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("Offline.Install.Label"), HeaderSize.Header3) & CrLf &
                GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Boot.Point.Mount.Label") & ImgMountDir,
                                           LocalizationService.ForSection("ImageInfoSave.Report")("OfflineVersion.Label") & FileVersionInfo.GetVersionInfo(ImgMountDir & "\Windows\system32\ntoskrnl.exe").ProductVersion.ToString()}.
                                       ToList()) & CrLf
            Exit Sub
        End If
        Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ImageFile.Get.Label") & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, "")}.ToList())
        Debug.WriteLine("[GetImageInformation] Starting task...")
        Try
            Debug.WriteLine("[GetImageInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetImageInformation] Populating info collection...")
            ImageInfoCollection = DismApi.GetImageInfo(SourceImage)
            Debug.WriteLine("[GetImageInformation] Information processes completed for the image. Obtained images: " & ImageInfoCollection.Count)
            Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("InfoSummary.Label") & ImageInfoCollection.Count & LocalizationService.ForSection("ImageInfoSave.Report")("ImageS.Label"), ParagraphStyle.Bold) & CrLf &
                GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Version.Column"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("ImageName.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("ImageDescription"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("ImageSize.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("Architecture.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("HAL.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("ServicePackBuild.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("ServicePackLevel.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("InstallationType.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("Edition.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("ProductType.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("ProductSuite.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("System.Root.Dir.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("Languages.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("DateCreation.Label"),
                                             LocalizationService.ForSection("ImageInfoSave.Report")("DateModification.Label")}.ToList())
            Debug.WriteLine("[GetImageInformation] Exporting information to contents...")
            For Each ImageInfo As DismImageInfo In ImageInfoCollection
                Dim msg As String = ""
                msg = LocalizationService.ForSection("ImageInfoSave.Image").Format("Getting.Image.Message", ImageInfoCollection.IndexOf(ImageInfo) + 1, ImageInfoCollection.Count)
                Dim languages As String = "<ul>"
                For Each language In ImageInfo.Languages
                    languages &= "<li>" & language.DisplayName & If(ImageInfo.DefaultLanguage.Name = language.Name, LocalizationService.ForSection("ImageInfoSave.Report")("Default.Label"), "") & "</li>"
                Next
                languages &= "</ul>"
                ReportChanges(msg, (ImageInfoCollection.IndexOf(ImageInfo) / ImageInfoCollection.Count) * 100)
                Contents &= GetTableRow(New String() {ImageInfo.ProductVersion.ToString(),
                                                      ImageInfo.ImageName,
                                                      ImageInfo.ImageDescription,
                                                      ImageInfo.ImageSize.ToString("N0") & LocalizationService.ForSection("ImageInfoSave.Report")("Bytes.Label") & Converters.BytesToReadableSize(ImageInfo.ImageSize) & ")",
                                                      Casters.CastDismArchitecture(ImageInfo.Architecture),
                                                      If(ImageInfo.Hal <> "", ImageInfo.Hal, LocalizationService.ForSection("ImageInfoSave.Report")("UndefinedImage.Label")),
                                                      ImageInfo.ProductVersion.Revision,
                                                      ImageInfo.SpLevel,
                                                      ImageInfo.InstallationType,
                                                      ImageInfo.EditionId,
                                                      ImageInfo.ProductType,
                                                      ImageInfo.ProductSuite,
                                                      ImageInfo.SystemRoot,
                                                      languages,
                                                      ImageInfo.CustomizedInfo.CreatedTime,
                                                      ImageInfo.CustomizedInfo.ModifiedTime}.
                                                  ToList())
            Next
        Catch ex As Exception
            Debug.WriteLine("[GetImageInformation] An error occurred while getting image information: " & ex.ToString() & " - " & ex.Message)
            WriteExceptionInfo(ex)
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Private Sub GetPackageInformation(GetEverything As Boolean)
        Dim InstalledPkgInfo As DismPackageCollection = Nothing
        Dim msg As String() = New String(2) {"", "", ""}
        msg(0) = LocalizationService.ForSection("ImgInfo.Packages")("Preparing.Package.Message")
        msg(1) = LocalizationService.ForSection("ImageInfoSave.Packages")("Basic.Ready.Message") & LocalizationService.ForSection("ImageInfoSave.Packages")("May.Take.Long.Message") & CrLf & CrLf & LocalizationService.ForSection("ImageInfoSave.Packages")("Prompt.Label")
        msg(2) = LocalizationService.ForSection("ImageInfoSave.Packages")("PackageInfo.Message")
        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("PackageInfo.Label"), HeaderSize.Header2) & CrLf &
                    GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ImageFile.Get.Label") & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, LocalizationService.ForSection("ImageInfoSave.Report")("Active.Install.Label.Label"))}.ToList()) & CrLf
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
                Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("InfoSummary.Label") & InstalledPkgInfo.Count & LocalizationService.ForSection("ImageInfoSave.Report")("PackageS.Label"), ParagraphStyle.Bold) & CrLf
                msg(0) = LocalizationService.ForSection("ImageInfoSave.Packages")("PackagesObtained.Message")
                ReportChanges(msg(0), 10)
                Dim pkgCustomPropsList As String = "<ul>"
                Dim pkgFeaturesList As String = "<ul>"
                If GetEverything Then
                    Contents &= CrLf & GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("PackageName.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Applicable.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Copyright.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Company.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("CreationTime.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Description"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("InstallClient.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Install.Package.Name.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("InstallTime.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Last.Update.Time.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("DisplayName.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("ProductName.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("ProductVersion.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("ReleaseType.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("RestartRequired.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("SupportInfo.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("PackageState.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Boot.Up.Required.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Capability.Identity.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("CustomProps.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Features.Label")}.
                                                                ToList())
                    Debug.WriteLine("[GetPackageInformation] Getting complete package information...")
                    For Each installedPackage As DismPackage In InstalledPkgInfo
                        msg(0) = LocalizationService.ForSection("ImgInfo.Packages").Format("Loading.Package.Message", InstalledPkgInfo.IndexOf(installedPackage) + 1, InstalledPkgInfo.Count)
                        ReportChanges(msg(0), (InstalledPkgInfo.IndexOf(installedPackage) / InstalledPkgInfo.Count) * 100)
                        Dim pkgInfoEx As DismPackageInfoEx = Nothing
                        Dim pkgInfo As DismPackageInfo = Nothing
                        Dim cProps As DismCustomPropertyCollection = Nothing

                        ' Determine Windows version, as capability identity information can't be obtained in Windows versions older than 10
                        If OSVer.Major >= 10 Then
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
                                pkgCustomPropsList = LocalizationService.ForSection("ImageInfoSave.Report")("None.Label")
                            End If
                            If pkgInfoEx.Features.Count > 0 Then
                                Dim pkgFeats As DismFeatureCollection = pkgInfoEx.Features
                                For Each pkgFeat As DismFeature In pkgFeats
                                    pkgFeaturesList &= "<li>" & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & "</li>"
                                Next
                                pkgFeaturesList &= "</ul>"
                            Else
                                pkgFeaturesList = LocalizationService.ForSection("ImageInfoSave.Report")("None.Label")
                            End If
                            Contents &= GetTableRow(New String() {CodeBlockChar & pkgInfoEx.PackageName & CodeBlockChar,
                                                                  Casters.Applicability(pkgInfoEx.Applicable),
                                                                  pkgInfoEx.Copyright,
                                                                  pkgInfoEx.Company,
                                                                  pkgInfoEx.CreationTime & If(pkgInfoEx.CreationTime.Year < 1900, LocalizationService.ForSection("ImageInfoSave.Report")("Preposterous.Time.Date.Label"), ""),
                                                                  pkgInfoEx.Description,
                                                                  pkgInfoEx.InstallClient,
                                                                  CodeBlockChar & pkgInfoEx.InstallPackageName & CodeBlockChar,
                                                                  pkgInfoEx.InstallTime,
                                                                  pkgInfoEx.LastUpdateTime & If(pkgInfoEx.LastUpdateTime.Year < 1900, LocalizationService.ForSection("ImageInfoSave.Report")("Preposterous.Time.Date.Label"), ""),
                                                                  pkgInfoEx.DisplayName,
                                                                  pkgInfoEx.ProductName,
                                                                  pkgInfoEx.ProductVersion.ToString(),
                                                                  Casters.CastDismReleaseType(pkgInfoEx.ReleaseType),
                                                                  Casters.CastDismRestartType(pkgInfoEx.RestartRequired),
                                                                  pkgInfoEx.SupportInformation,
                                                                  Casters.CastDismPackageState(pkgInfoEx.PackageState),
                                                                  Casters.OfflineInstallType(pkgInfoEx.FullyOffline),
                                                                  CodeBlockChar & pkgInfoEx.CapabilityId & CodeBlockChar,
                                                                  pkgCustomPropsList,
                                                                  pkgFeaturesList}.
                                                              ToList())
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
                                pkgCustomPropsList = LocalizationService.ForSection("ImageInfoSave.Report")("None.Label")
                            End If
                            If pkgInfo.Features.Count > 0 Then
                                Dim pkgFeats As DismFeatureCollection = pkgInfo.Features
                                For Each pkgFeat As DismFeature In pkgFeats
                                    pkgFeaturesList &= "<li>" & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & "</li>"
                                Next
                                pkgFeaturesList &= "</ul>"
                            Else
                                pkgFeaturesList = LocalizationService.ForSection("ImageInfoSave.Report")("None.Label")
                            End If
                            Contents &= GetTableRow(New String() {CodeBlockChar & pkgInfo.PackageName & CodeBlockChar,
                                                                  Casters.Applicability(pkgInfo.Applicable),
                                                                  pkgInfo.Copyright,
                                                                  pkgInfo.Company,
                                                                  pkgInfo.CreationTime & If(pkgInfo.CreationTime.Year < 1900, LocalizationService.ForSection("ImageInfoSave.Report")("Preposterous.Time.Date.Label"), ""),
                                                                  pkgInfo.Description,
                                                                  pkgInfo.InstallClient,
                                                                  CodeBlockChar & pkgInfo.InstallPackageName & CodeBlockChar,
                                                                  pkgInfo.InstallTime,
                                                                  pkgInfo.LastUpdateTime & If(pkgInfo.LastUpdateTime.Year < 1900, LocalizationService.ForSection("ImageInfoSave.Report")("Preposterous.Time.Date.Label"), ""),
                                                                  pkgInfo.DisplayName,
                                                                  pkgInfo.ProductName,
                                                                  pkgInfo.ProductVersion.ToString(),
                                                                  Casters.CastDismReleaseType(pkgInfo.ReleaseType),
                                                                  Casters.CastDismRestartType(pkgInfo.RestartRequired),
                                                                  pkgInfo.SupportInformation,
                                                                  Casters.CastDismPackageState(pkgInfo.PackageState),
                                                                  Casters.OfflineInstallType(pkgInfo.FullyOffline),
                                                                  LocalizationService.ForSection("ImageInfoSave.Report")("None.Label"),
                                                                  pkgCustomPropsList,
                                                                  pkgFeaturesList}.
                                                              ToList())
                        End If
                    Next
                    Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("PackageInfo.Ready.Label")) & CrLf
                Else
                    msg(0) = LocalizationService.ForSection("ImageInfoSave.Packages")("SavePackages.Message")
                    ReportChanges(msg(0), 50)
                    Contents &= GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("PackageName.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("PackageState.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("Package.Release.Type.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("Package.Install.Time.Label")}.
                                                         ToList())
                    For Each installedPackage As DismPackage In InstalledPkgInfo
                        Contents &= GetTableRow(New String() {CodeBlockChar & installedPackage.PackageName & CodeBlockChar,
                                                              Casters.CastDismPackageState(installedPackage.PackageState),
                                                              Casters.CastDismReleaseType(installedPackage.ReleaseType),
                                                              installedPackage.InstallTime}.
                                                          ToList())
                    Next
                    Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("PackageInfo.Missing.Label")) & CrLf
                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetPackageInformation] An error occurred while getting package information: " & ex.ToString() & " - " & ex.Message)
            WriteExceptionInfo(ex)
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Private Sub GetPackageFileInformation()
        Dim msg As String = ""
        msg = LocalizationService.ForSection("ImgInfo.PkgFiles")("Preparing.Package.Message")
        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("Package.File.Label"), HeaderSize.Header2) & CrLf &
                    GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ImageFile.Get.Label") & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, LocalizationService.ForSection("ImageInfoSave.Report")("Active.Install.Label.Label"))}.ToList()) & CrLf
        Debug.WriteLine("[GetPackageFileInformation] Starting task...")
        Try
            Debug.WriteLine("[GetPackageFileInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetPackageFileInformation] Creating image session...")
            ReportChanges(msg, 0)
            Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Amount.Package.Files.Label") & PackageFiles.Count, ParagraphStyle.Bold)
            Contents &= CrLf & GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("PackageName.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("Applicable.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("Copyright.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("Company.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("CreationTime.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("Description"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("InstallClient.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("Install.Package.Name.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("InstallTime.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("Last.Update.Time.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("DisplayName.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("ProductName.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("ProductVersion.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("ReleaseType.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("RestartRequired.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("SupportInfo.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("PackageState.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("Boot.Up.Required.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("Capability.Identity.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("CustomProps.Label"),
                                                            LocalizationService.ForSection("ImageInfoSave.Report")("Features.Label")}.
                                                        ToList())
            Dim pkgCustomPropsList As String = "<ul>"
            Dim pkgFeaturesList As String = "<ul>"
            Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                For Each pkgFile In PackageFiles
                    Try
                        msg = LocalizationService.ForSection("ImgInfo.PackageFiles").Format("Loading.Package.Message", PackageFiles.IndexOf(pkgFile) + 1, PackageFiles.Count)
                        ReportChanges(msg, (PackageFiles.IndexOf(pkgFile) / PackageFiles.Count) * 100)
                        If File.Exists(pkgFile) Then
                            Dim pkgInfoEx As DismPackageInfoEx = Nothing
                            Dim pkgInfo As DismPackageInfo = Nothing
                            Dim cProps As DismCustomPropertyCollection = Nothing

                            ' Determine Windows version
                            If OSVer.Major >= 10 Then
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
                                    pkgCustomPropsList = LocalizationService.ForSection("ImageInfoSave.Report")("None.Label")
                                End If
                                If pkgInfoEx.Features.Count > 0 Then
                                    Dim pkgFeats As DismFeatureCollection = pkgInfoEx.Features
                                    For Each pkgFeat As DismFeature In pkgFeats
                                        pkgFeaturesList &= "<li>" & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & "</li>"
                                    Next
                                    pkgFeaturesList &= "</ul>"
                                Else
                                    pkgFeaturesList = LocalizationService.ForSection("ImageInfoSave.Report")("None.Label")
                                End If
                                Contents &= GetTableRow(New String() {CodeBlockChar & pkgInfoEx.PackageName & CodeBlockChar,
                                                                      Casters.Applicability(pkgInfoEx.Applicable),
                                                                      pkgInfoEx.Copyright,
                                                                      pkgInfoEx.Company,
                                                                      pkgInfoEx.CreationTime,
                                                                      pkgInfoEx.Description,
                                                                      If(pkgInfoEx.InstallClient = "", LocalizationService.ForSection("ImageInfoSave.Report")("None.Label"), pkgInfoEx.InstallClient),
                                                                      If(pkgInfoEx.InstallPackageName = "", LocalizationService.ForSection("ImageInfoSave.Report")("None.Label"), CodeBlockChar & pkgInfoEx.InstallPackageName & CodeBlockChar),
                                                                      pkgInfoEx.InstallTime,
                                                                      pkgInfoEx.LastUpdateTime,
                                                                      pkgInfoEx.DisplayName,
                                                                      pkgInfoEx.ProductName,
                                                                      pkgInfoEx.ProductVersion.ToString(),
                                                                      Casters.CastDismReleaseType(pkgInfoEx.ReleaseType),
                                                                      Casters.CastDismRestartType(pkgInfoEx.RestartRequired),
                                                                      pkgInfoEx.SupportInformation,
                                                                      Casters.CastDismPackageState(pkgInfoEx.PackageState),
                                                                      Casters.OfflineInstallType(pkgInfoEx.FullyOffline),
                                                                      If(pkgInfoEx.CapabilityId = "", LocalizationService.ForSection("ImageInfoSave.Report")("None.Label"), CodeBlockChar & pkgInfoEx.CapabilityId & CodeBlockChar),
                                                                      pkgCustomPropsList,
                                                                      pkgFeaturesList}.ToList())
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
                                    pkgCustomPropsList = LocalizationService.ForSection("ImageInfoSave.Report")("None.Label")
                                End If
                                If pkgInfo.Features.Count > 0 Then
                                    Dim pkgFeats As DismFeatureCollection = pkgInfo.Features
                                    For Each pkgFeat As DismFeature In pkgFeats
                                        pkgFeaturesList &= "<li>" & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State) & ")" & "</li>"
                                    Next
                                    pkgFeaturesList &= "</ul>"
                                Else
                                    pkgFeaturesList = LocalizationService.ForSection("ImageInfoSave.Report")("None.Label")
                                End If
                                Contents &= GetTableRow(New String() {CodeBlockChar & pkgInfo.PackageName & CodeBlockChar,
                                                                      Casters.Applicability(pkgInfo.Applicable),
                                                                      pkgInfo.Copyright,
                                                                      pkgInfo.Company,
                                                                      pkgInfo.CreationTime,
                                                                      pkgInfo.Description,
                                                                      If(pkgInfo.InstallClient = "", LocalizationService.ForSection("ImageInfoSave.Report")("None.Label"), pkgInfo.InstallClient),
                                                                      If(pkgInfo.InstallPackageName = "", LocalizationService.ForSection("ImageInfoSave.Report")("None.Label"), CodeBlockChar & pkgInfo.InstallPackageName & CodeBlockChar),
                                                                      pkgInfo.InstallTime,
                                                                      pkgInfo.LastUpdateTime,
                                                                      pkgInfo.DisplayName,
                                                                      pkgInfo.ProductName,
                                                                      pkgInfo.ProductVersion.ToString(),
                                                                      Casters.CastDismReleaseType(pkgInfo.ReleaseType),
                                                                      Casters.CastDismRestartType(pkgInfo.RestartRequired),
                                                                      pkgInfo.SupportInformation,
                                                                      Casters.CastDismPackageState(pkgInfo.PackageState),
                                                                      Casters.OfflineInstallType(pkgInfo.FullyOffline),
                                                                      LocalizationService.ForSection("ImageInfoSave.Report")("None.Label"),
                                                                      pkgCustomPropsList,
                                                                      pkgFeaturesList}.ToList())
                            End If
                        End If
                    Catch PkgInfoEx As DismException
                        Debug.WriteLine("[GetPackageFileInformation] An error occurred while getting package information: " & PkgInfoEx.ToString() & " - " & PkgInfoEx.Message)
                    End Try
                Next
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetPackageFileInformation] An error occurred while getting package information: " & ex.ToString() & " - " & ex.Message)
            WriteExceptionInfo(ex)
        Finally
            DismApi.Shutdown()
        End Try

    End Sub

    Private Sub GetFeatureInformation(GetEverything As Boolean)
        Dim InstalledFeatInfo As DismFeatureCollection = Nothing
        Dim msg As String() = New String(2) {"", "", ""}
        msg(0) = LocalizationService.ForSection("ImgInfo.Features")("Preparing.Feature.Message")
        msg(1) = LocalizationService.ForSection("ImageInfoSave.Features")("Basic.Ready.Message") & LocalizationService.ForSection("ImageInfoSave.Features")("May.Take.Long.Message") & CrLf & CrLf & LocalizationService.ForSection("ImageInfoSave.Features")("Prompt.Label")
        msg(2) = LocalizationService.ForSection("ImageInfoSave.Features")("FeatureInfo.Message")
        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("FeatureInfo.Label"), HeaderSize.Header2) & CrLf &
                    GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ImageFile.Get.Label") & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, LocalizationService.ForSection("ImageInfoSave.Report")("Active.Install.Label.Label"))}.ToList()) & CrLf
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
                Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("InfoSummary.Label") & InstalledFeatInfo.Count & LocalizationService.ForSection("ImageInfoSave.Report")("FeatureCount.Suffix"), ParagraphStyle.Bold) & CrLf
                msg(0) = LocalizationService.ForSection("ImageInfoSave.Features")("FeaturesObtained.Message")
                ReportChanges(msg(0), 10)
                Dim featCustomPropsList As String = "<ul>"
                If GetEverything Then
                    Contents &= CrLf & GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("FeatureName.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("DisplayName.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Description"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("RestartRequired.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("FeatureState.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("CustomProps.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Web.Label")}.ToList())
                    Debug.WriteLine("[GetFeatureInformation] Getting complete feature information...")
                    For Each feature As DismFeature In InstalledFeatInfo
                        featCustomPropsList = "<ul>"
                        msg(0) = LocalizationService.ForSection("ImgInfo.Features").Format("Loading.Feature.Message", InstalledFeatInfo.IndexOf(feature) + 1, InstalledFeatInfo.Count)
                        ReportChanges(msg(0), (InstalledFeatInfo.IndexOf(feature) / InstalledFeatInfo.Count) * 100)
                        Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, feature.FeatureName)
                        Dim cProps As DismCustomPropertyCollection = featInfo.CustomProperties
                        If cProps.Count > 0 Then
                            For Each cProp As DismCustomProperty In cProps
                                featCustomPropsList &= "<li>" & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value.Replace(CrLf, " ").Replace(Lf, " ").Replace(Cr, " ").Trim() & "</li>"
                            Next
                            featCustomPropsList &= "</ul>"
                        Else
                            featCustomPropsList = LocalizationService.ForSection("ImageInfoSave.Report")("None.Label")
                        End If
                        Contents &= GetTableRow(New String() {featInfo.FeatureName,
                                                              featInfo.DisplayName,
                                                              featInfo.Description,
                                                              Casters.CastDismRestartType(featInfo.RestartRequired),
                                                              Casters.CastDismFeatureState(featInfo.FeatureState),
                                                              featCustomPropsList,
                                                              MarkdownHelper.GetLink(SearchEngineHelper.GetSearchQueryUri(String.Format("microsoft windows {0}{1}{0}", Quote, featInfo.FeatureName)), LocalizationService.ForSection("ImageInfoSave.Report")("Look.Item.Online.Label"))}.ToList())
                    Next
                    Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("FeatureInfo.Ready.Label")) & CrLf
                Else
                    msg(0) = LocalizationService.ForSection("ImageInfoSave.Features")("SaveFeatures.Message")
                    ReportChanges(msg(0), 50)
                    Contents &= GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("FeatureName.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("FeatureState.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("Web.Label")}.ToList())
                    For Each installedFeature As DismFeature In InstalledFeatInfo
                        Contents &= GetTableRow(New String() {installedFeature.FeatureName,
                                                              Casters.CastDismFeatureState(installedFeature.State),
                                                              MarkdownHelper.GetLink(SearchEngineHelper.GetSearchQueryUri(String.Format("microsoft windows {0}{1}{0}", Quote, installedFeature.FeatureName)), LocalizationService.ForSection("ImageInfoSave.Report")("Look.Item.Online.Label"))}.ToList()) & CrLf
                    Next
                    Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("FeatureInfo.Missing.Label")) & CrLf
                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetFeatureInformation] An error occurred while getting feature information: " & ex.ToString() & " - " & ex.Message)
            WriteExceptionInfo(ex)
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Private Sub GetAppxInformation(GetEverything As Boolean)
        Dim InstalledAppxPackageInfo As DismAppxPackageCollection = Nothing
        Dim msg As String() = New String(2) {"", "", ""}
        msg(0) = LocalizationService.ForSection("ImageInfoSave.AppxInfo")("Preparing.Package.Message")
        msg(1) = LocalizationService.ForSection("ImageInfoSave.AppxInfo")("Basic.Ready.Message") & LocalizationService.ForSection("ImageInfoSave.AppxInfo")("May.Take.Long.Message") & CrLf & CrLf & LocalizationService.ForSection("ImageInfoSave.AppxInfo")("Prompt.Label")
        msg(2) = LocalizationService.ForSection("ImageInfoSave.AppxInfo")("Package.Message")
        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("AppX.Package.Label"), HeaderSize.Header2) & CrLf &
                    GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ImageFile.Get.Label") & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, LocalizationService.ForSection("ImageInfoSave.Report")("Active.Install.Label.Label"))}.ToList()) & CrLf
        If ImageToGetInfoFrom.ImageEditionId Is Nothing Then
            ImageToGetInfoFrom.ImageEditionId = " "
        End If
        ' Detect if the image is Windows 8 or later. If not, skip this task
        If (Not OnlineMode And (Not MainForm.IsWindows8OrHigher(ImgMountDir & "\Windows\system32\ntoskrnl.exe") Or ImageToGetInfoFrom.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase))) Or (OnlineMode And Not MainForm.IsWindows8OrHigher(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe")) Then
            Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Task.Supported.Win.Message"), ParagraphStyle.Bold) & CrLf
            Exit Sub
        Else
            Debug.WriteLine("[GetAppxInformation] Starting task...")
            ' Do note that, when using the MainForm arrays, an empty entry appears at the end, so don't take it into account
            Try
                ' Windows 8 can't get this information with the API. Use the MainForm arrays
                If Environment.OSVersion.Version.Major < 10 Then
                    Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("InfoSummary.Label") & ImageToGetInfoFrom.ImageAppxPackages_Backup.Count() & LocalizationService.ForSection("ImageInfoSave.Report")("AppXPackages.Label"), ParagraphStyle.Bold) & CrLf &
                        GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("PackageName.Label"),
                                                     LocalizationService.ForSection("ImageInfoSave.Report")("App.Display.Name.Label"),
                                                     LocalizationService.ForSection("ImageInfoSave.Report")("Architecture.Label"),
                                                     LocalizationService.ForSection("ImageInfoSave.Report")("ResourceID.Label"),
                                                     LocalizationService.ForSection("ImageInfoSave.Report")("Version.Column"),
                                                     LocalizationService.ForSection("ImageInfoSave.Report")("RegisteredUser.Label"),
                                                     LocalizationService.ForSection("ImageInfoSave.Report")("Install.Location.Label"),
                                                     LocalizationService.ForSection("ImageInfoSave.Report")("Package.Manifest.Label"),
                                                     LocalizationService.ForSection("ImageInfoSave.Report")("StoreLogo.Asset.Dir.Label"),
                                                     LocalizationService.ForSection("ImageInfoSave.Report")("Main.StoreLogo.Asset.Label")}.
                                                 ToList())
                    Dim idx As Integer = 0
                    For Each AppxPackage In ImageToGetInfoFrom.ImageAppxPackages_Backup
                        msg(0) = LocalizationService.ForSection("ImageInfoSave.AppxInfo").Format("Getting.Message", idx + 1, ImageToGetInfoFrom.ImageAppxPackages_Backup.Count)
                        ReportChanges(msg(0), ((idx + 1) / ImageToGetInfoFrom.ImageAppxPackages_Backup.Count) * 100)
                        Dim registrationStatus As String = ""                         ' Use to pass final result to Markdown report
                        ' Detect if *.pckgdep files are present in the AppRepository folder, as that's how this program gets the registration status of an AppX package
                        If Directory.Exists(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & AppxPackage.PackageFullName,
                                               ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & AppxPackage.PackageFullName)) Then
                            ' Get the number of pckgdep files
                            If My.Computer.FileSystem.GetFiles(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & AppxPackage.PackageFullName,
                                                                  ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & AppxPackage.PackageFullName), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count > 0 Then
                                registrationStatus = LocalizationService.ForSection("ImageInfoSave.Report")("Yes.Button")
                            Else
                                registrationStatus = LocalizationService.ForSection("ImageInfoSave.Report")("No.Button")
                            End If
                        Else
                            registrationStatus = LocalizationService.ForSection("ImageInfoSave.Report")("No.Button")
                        End If
                        Dim installationLocation As String = (If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & AppxPackage.PackageFullName).Replace("\\", "\").Trim()
                        Dim pkgDirs() As String = Directory.GetDirectories(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps", AppxPackage.PackageFullName & "*", SearchOption.TopDirectoryOnly)
                        Dim instDir As String = ""
                        For Each folder In pkgDirs
                            If Not folder.Contains("neutral") Then
                                instDir = (folder & "\AppxManifest.xml").Replace("\\", "\").Trim()
                            End If
                        Next
                        Try
                            If pkgDirs.Count <= 1 And Not instDir.Contains(AppxPackage.PackageFullName) Then
                                If File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml") Then
                                    instDir = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml"
                                ElseIf File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml") Then
                                    instDir = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml"
                                Else
                                    instDir = LocalizationService.ForSection("ImageInfoSave.Report")("Unknown.Label")
                                End If
                            End If
                        Catch ex As Exception
                            instDir = LocalizationService.ForSection("ImageInfoSave.Report")("Unknown.Label")
                        End Try
                        ' Get store logo asset directory
                        Dim logoAssetDir As String = ""                         ' Use to pass final result to Markdown report
                        Dim assetDir As String = ""
                        Try
                            assetDir = MainForm.GetSuitablePackageFolder(AppxPackage.PackageName)
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
                            If File.Exists(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & AppxPackage.PackageFullName & "\AppxManifest.xml") Then
                                Dim ManFile As New RichTextBox() With {
                                    .Text = File.ReadAllText(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & AppxPackage.PackageFullName & "\AppxManifest.xml")
                                }
                                For Each line In ManFile.Lines
                                    If line.Contains("<Logo>") Then
                                        Dim SplitPaths As New List(Of String)
                                        SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                                        SplitPaths.RemoveAt(SplitPaths.Count - 1)
                                        Dim newPath As String = String.Join("\", SplitPaths)
                                        logoAssetDir = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & AppxPackage.PackageFullName & "\" & newPath).Replace("\\", "\").Trim()
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                        ' Since store logo assets can't be saved on plain text files, output their locations
                        Dim mainLogo As String = ""                         ' Use to pass final result to Markdown report
                        Dim mainAsset As String = MainForm.GetStoreAppMainLogo(AppxPackage.PackageFullName)
                        If mainAsset <> "" And File.Exists(mainAsset) Then
                            mainLogo = mainAsset.Replace("\\", "\").Trim()
                        Else
                            mainLogo = LocalizationService.ForSection("ImageInfoSave.Report")("Unknown.Label")
                        End If
                        Contents &= GetTableRow(New String() {AppxPackage.PackageFullName,
                                                              AppxPackage.PackageName,
                                                              Casters.CastDismArchitecture(AppxPackage.PackageArchitecture),
                                                              AppxPackage.PackageResourceId,
                                                              AppxPackage.PackageVersion.ToString(),
                                                              registrationStatus,
                                                              installationLocation,
                                                              instDir,
                                                              logoAssetDir.TrimEnd("\"),
                                                              mainLogo.TrimEnd(Quote)}.ToList())
                        idx += 1
                    Next
                    Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Notemain.StoreLogo.Message") & Quote & LocalizationService.ForSection("ImageInfoSave.Report")("StoreLogo.Asset.Label") & Quote & LocalizationService.ForSection("ImageInfoSave.Report")("Template.Provide.Message"), ParagraphStyle.Italic) & CrLf
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
                        pkgNames.AddRange(InstalledAppxPackageInfo.Select(Function(appx) appx.PackageName))
                        Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("InfoSummary.Label") & If(ImageToGetInfoFrom.ImageAppxPackages_Backup.Count() > pkgNames.Count,
                                                                                        ImageToGetInfoFrom.ImageAppxPackages_Backup.Count(), pkgNames.Count) & LocalizationService.ForSection("ImageInfoSave.Report")("AppXPackages.Label"), ParagraphStyle.Bold) & CrLf &
                            GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("PackageName.Label"),
                                                         LocalizationService.ForSection("ImageInfoSave.Report")("App.Display.Name.Label"),
                                                         LocalizationService.ForSection("ImageInfoSave.Report")("Architecture.Label"),
                                                         LocalizationService.ForSection("ImageInfoSave.Report")("ResourceID.Label"),
                                                         LocalizationService.ForSection("ImageInfoSave.Report")("Version.Column"),
                                                         LocalizationService.ForSection("ImageInfoSave.Report")("RegisteredUser.Label"),
                                                         LocalizationService.ForSection("ImageInfoSave.Report")("Install.Location.Label"),
                                                         LocalizationService.ForSection("ImageInfoSave.Report")("Package.Manifest.Label"),
                                                         LocalizationService.ForSection("ImageInfoSave.Report")("StoreLogo.Asset.Dir.Label"),
                                                         LocalizationService.ForSection("ImageInfoSave.Report")("Main.StoreLogo.Asset.Label")}.
                                                     ToList())
                        msg(0) = LocalizationService.ForSection("ImageInfoSave.AppxInfo")("Packages.Obtained.Message")
                        ReportChanges(msg(0), 10)
                        If GetEverything Then
                            Debug.WriteLine("[GetAppxInformation] Getting complete AppX package information...")
                            If Not ForceAppxApi AndAlso ImageToGetInfoFrom.ImageAppxPackages_Backup.Count - 1 > pkgNames.Count Then
                                Dim idx As Integer = 0
                                For Each AppxPackage In ImageToGetInfoFrom.ImageAppxPackages_Backup
                                    msg(0) = LocalizationService.ForSection("ImageInfoSave.AppxInfo").Format("Getting.Message", idx + 1, ImageToGetInfoFrom.ImageAppxPackages_Backup.Count)
                                    ReportChanges(msg(0), ((idx + 1) / ImageToGetInfoFrom.ImageAppxPackages_Backup.Count) * 100)
                                    Dim registrationStatus As String = ""                         ' Use to pass final result to Markdown report
                                    ' Detect if *.pckgdep files are present in the AppRepository folder, as that's how this program gets the registration status of an AppX package
                                    If Directory.Exists(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & AppxPackage.PackageFullName,
                                                           ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & AppxPackage.PackageFullName)) Then
                                        ' Get the number of pckgdep files
                                        If My.Computer.FileSystem.GetFiles(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & AppxPackage.PackageFullName,
                                                                              ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & AppxPackage.PackageFullName), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count > 0 Then
                                            registrationStatus = LocalizationService.ForSection("ImageInfoSave.Report")("Yes.Button")
                                        Else
                                            registrationStatus = LocalizationService.ForSection("ImageInfoSave.Report")("No.Button")
                                        End If
                                    Else
                                        registrationStatus = LocalizationService.ForSection("ImageInfoSave.Report")("No.Button")
                                    End If
                                    Dim installationLocation As String = (If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & AppxPackage.PackageFullName).Replace("\\", "\").Trim()
                                    Dim pkgDirs() As String = Directory.GetDirectories(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps", AppxPackage.PackageFullName & "*", SearchOption.TopDirectoryOnly)
                                    Dim instDir As String = ""
                                    For Each folder In pkgDirs
                                        If Not folder.Contains("neutral") Then
                                            instDir = (folder & "\AppxManifest.xml").Replace("\\", "\").Trim()
                                        End If
                                    Next
                                    Try
                                        If pkgDirs.Count <= 1 And Not instDir.Contains(AppxPackage.PackageFullName) Then
                                            If File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml") Then
                                                instDir = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml"
                                            ElseIf File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml") Then
                                                instDir = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml"
                                            Else
                                                instDir = LocalizationService.ForSection("ImageInfoSave.Report")("Unknown.Label")
                                            End If
                                        End If
                                    Catch ex As Exception
                                        instDir = LocalizationService.ForSection("ImageInfoSave.Report")("Unknown.Label")
                                    End Try
                                    ' Get store logo asset directory
                                    Dim logoAssetDir As String = ""                         ' Use to pass final result to Markdown report
                                    Dim assetDir As String = ""
                                    Try
                                        assetDir = MainForm.GetSuitablePackageFolder(AppxPackage.PackageName)
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
                                        If File.Exists(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & AppxPackage.PackageFullName & "\AppxManifest.xml") Then
                                            Dim ManFile As New RichTextBox() With {
                                                .Text = File.ReadAllText(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & AppxPackage.PackageFullName & "\AppxManifest.xml")
                                            }
                                            For Each line In ManFile.Lines
                                                If line.Contains("<Logo>") Then
                                                    Dim SplitPaths As New List(Of String)
                                                    SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                                                    SplitPaths.RemoveAt(SplitPaths.Count - 1)
                                                    Dim newPath As String = String.Join("\", SplitPaths)
                                                    logoAssetDir = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & AppxPackage.PackageFullName & "\" & newPath).Replace("\\", "\").Trim()
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    End If
                                    ' Since store logo assets can't be saved on plain text files, output their locations
                                    Dim mainLogo As String = ""                         ' Use to pass final result to Markdown report
                                    Dim mainAsset As String = MainForm.GetStoreAppMainLogo(AppxPackage.PackageFullName)
                                    If mainAsset <> "" And File.Exists(mainAsset) Then
                                        mainLogo = mainAsset.Replace("\\", "\").Trim()
                                    Else
                                        mainLogo = LocalizationService.ForSection("ImageInfoSave.Report")("Unknown.Label")
                                    End If
                                    Contents &= GetTableRow(New String() {AppxPackage.PackageFullName,
                                                                          AppxPackage.PackageName,
                                                                          Casters.CastDismArchitecture(AppxPackage.PackageArchitecture),
                                                                          AppxPackage.PackageResourceId,
                                                                          AppxPackage.PackageVersion.ToString(),
                                                                          registrationStatus,
                                                                          installationLocation,
                                                                          instDir,
                                                                          logoAssetDir.TrimEnd("\"),
                                                                          mainLogo.TrimEnd(Quote)}.ToList())
                                    idx += 1
                                Next
                                Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Notemain.StoreLogo.Message") & Quote & LocalizationService.ForSection("ImageInfoSave.Report")("StoreLogo.Asset.Label") & Quote & LocalizationService.ForSection("ImageInfoSave.Report")("Template.Provide.Message"), ParagraphStyle.Italic) & CrLf
                            Else
                                For Each appxPkg As DismAppxPackage In InstalledAppxPackageInfo
                                    msg(0) = LocalizationService.ForSection("ImageInfoSave.AppxInfo").Format("Getting.Message", InstalledAppxPackageInfo.IndexOf(appxPkg) + 1, InstalledAppxPackageInfo.Count)
                                    ReportChanges(msg(0), (InstalledAppxPackageInfo.IndexOf(appxPkg) / InstalledAppxPackageInfo.Count) * 100)
                                    Dim registrationStatus As String = ""                         ' Use to pass final result to Markdown report
                                    ' Detect if *.pckgdep files are present in the AppRepository folder, as that's how this program gets the registration status of an AppX package
                                    If Directory.Exists(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName,
                                                           ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName)) Then
                                        ' Get the number of pckgdep files
                                        If My.Computer.FileSystem.GetFiles(If(OnlineMode, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName,
                                                                              ImgMountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxPkg.PackageName), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count > 0 Then
                                            registrationStatus = LocalizationService.ForSection("ImageInfoSave.Report")("Yes.Button")
                                        Else
                                            registrationStatus = LocalizationService.ForSection("ImageInfoSave.Report")("No.Button")
                                        End If
                                    Else
                                        registrationStatus = LocalizationService.ForSection("ImageInfoSave.Report")("No.Button")
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
                                        pkgManifestLocation = LocalizationService.ForSection("ImageInfoSave.Report")("Unknown.Label")
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
                                        mainLogo = LocalizationService.ForSection("ImageInfoSave.Report")("Unknown.Label")
                                    End If
                                    Contents &= GetTableRow(New String() {appxPkg.PackageName,
                                                                          appxPkg.DisplayName,
                                                                          Casters.CastDismArchitecture(appxPkg.Architecture),
                                                                          appxPkg.ResourceId,
                                                                          appxPkg.Version.ToString(),
                                                                          registrationStatus,
                                                                          installationLocation,
                                                                          pkgManifestLocation,
                                                                          logoAssetDir.TrimEnd("\"),
                                                                          mainLogo}.ToList())
                                Next
                                Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Notemain.StoreLogo.Message") & Quote & LocalizationService.ForSection("ImageInfoSave.Report")("StoreLogo.Asset.Label") & Quote & LocalizationService.ForSection("ImageInfoSave.Report")("Template.Provide.Message"), ParagraphStyle.Italic) & CrLf
                            End If
                            Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Complete.AppX.Label")) & CrLf
                        Else
                            msg(0) = LocalizationService.ForSection("ImageInfoSave.AppxInfo")("Saving.Installed.Message")
                            ReportChanges(msg(0), 50)
                            Contents &= GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("PackageName.Label"),
                                                                     LocalizationService.ForSection("ImageInfoSave.Report")("App.Display.Name.Label"),
                                                                     LocalizationService.ForSection("ImageInfoSave.Report")("Architecture.Label"),
                                                                     LocalizationService.ForSection("ImageInfoSave.Report")("ResourceID.Label"),
                                                                     LocalizationService.ForSection("ImageInfoSave.Report")("Version.Column")}.ToList())
                            For Each installedAppxPkg As DismAppxPackage In InstalledAppxPackageInfo
                                Contents &= GetTableRow(New String() {installedAppxPkg.PackageName,
                                                                      installedAppxPkg.DisplayName,
                                                                      Casters.CastDismArchitecture(installedAppxPkg.Architecture),
                                                                      installedAppxPkg.ResourceId,
                                                                      installedAppxPkg.Version.ToString()}.ToList())
                            Next
                            Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("AppxInfo.Ready2.Label")) & CrLf
                        End If
                    End Using
                End If
            Catch ex As Exception
                Debug.WriteLine("[GetAppxInformation] An error occurred while getting AppX package information: " & ex.ToString() & " - " & ex.Message)
                WriteExceptionInfo(ex)
            Finally
                DismApi.Shutdown()
            End Try
        End If
    End Sub

    Private Sub GetCapabilityInformation(GetEverything As Boolean)
        Dim InstalledCapInfo As DismCapabilityCollection = Nothing
        Dim msg As String() = New String(2) {"", "", ""}
        msg(0) = LocalizationService.ForSection("ImgInfo.Capabilities")("Preparing.Message")
        msg(1) = LocalizationService.ForSection("ImgInfo.Capabilities")("Basic.Ready.Message") & LocalizationService.ForSection("ImgInfo.Capabilities")("May.Take.Long.Message") & CrLf & CrLf & LocalizationService.ForSection("ImgInfo.Capabilities")("Save.Prompt.Label")
        msg(2) = LocalizationService.ForSection("ImgInfo.Capabilities")("CapabilityInfo.Message")
        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("CapabilityInfo.Label"), HeaderSize.Header2) & CrLf &
                    GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ImageFile.Get.Label") & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, LocalizationService.ForSection("ImageInfoSave.Report")("Active.Install.Label.Label"))}.ToList()) & CrLf
        If ImageToGetInfoFrom.ImageEditionId Is Nothing Then
            ImageToGetInfoFrom.ImageEditionId = " "
        End If
        If (Not OnlineMode And (Not MainForm.IsWindows10OrHigher(ImgMountDir & "\Windows\system32\ntoskrnl.exe") Or ImageToGetInfoFrom.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase))) Or (OnlineMode And Not MainForm.IsWindows10OrHigher(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe")) Then
            Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Task.Supported.Message"), ParagraphStyle.Bold) & CrLf
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
                    Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("InfoSummary.Label") & InstalledCapInfo.Count & LocalizationService.ForSection("ImageInfoSave.Report")("CapabilityIes.Label"), ParagraphStyle.Bold) & CrLf
                    msg(0) = LocalizationService.ForSection("ImgInfo.Capabilities")("Loaded.Message")
                    ReportChanges(msg(0), 10)
                    If GetEverything Then
                        Contents &= CrLf & GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Capability.Identity.Label"),
                                                                        LocalizationService.ForSection("ImageInfoSave.Report")("CapabilityName.Label"),
                                                                        LocalizationService.ForSection("ImageInfoSave.Report")("CapabilityState.Label"),
                                                                        LocalizationService.ForSection("ImageInfoSave.Report")("DisplayName.Label"),
                                                                        LocalizationService.ForSection("ImageInfoSave.Report")("DownloadSize.Label"),
                                                                        LocalizationService.ForSection("ImageInfoSave.Report")("InstallationSize.Label"),
                                                                        LocalizationService.ForSection("ImageInfoSave.Report")("Web.Label")}.ToList())
                        Debug.WriteLine("[GetCapabilityInformation] Getting complete capability information...")
                        For Each capability As DismCapability In InstalledCapInfo
                            msg(0) = LocalizationService.ForSection("ImgInfo.Capabilities").Format("Loading.Capability.Message", InstalledCapInfo.IndexOf(capability) + 1, InstalledCapInfo.Count)
                            ReportChanges(msg(0), (InstalledCapInfo.IndexOf(capability) / InstalledCapInfo.Count) * 100)
                            Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, capability.Name)
                            Contents &= GetTableRow(New String() {CodeBlockChar & capInfo.Name & CodeBlockChar,
                                                                  CodeBlockChar & capInfo.Name.Remove(InStr(capInfo.Name, "~") - 1) & CodeBlockChar,
                                                                  Casters.CastDismPackageState(capInfo.State),
                                                                  capInfo.Description,
                                                                  capInfo.DownloadSize & LocalizationService.ForSection("ImageInfoSave.Report")("BytesSuffix.Label") & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")", ""),
                                                                  capInfo.InstallSize & LocalizationService.ForSection("ImageInfoSave.Report")("BytesSuffix.Label") & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")", ""),
                                                                  MarkdownHelper.GetLink(SearchEngineHelper.GetSearchQueryUri(String.Format("microsoft windows {0}{1}{0}", Quote, capInfo.Name)), LocalizationService.ForSection("ImageInfoSave.Report")("Look.Item.Online.Label"))}.ToList())
                        Next
                        Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("CapabilityInfo.Ready.Label")) & CrLf
                    Else
                        msg(0) = LocalizationService.ForSection("ImgInfo.Capabilities")("Saving.Message")
                        ReportChanges(msg(0), 50)
                        Contents &= GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Capability.Identity.Label"),
                                                                 LocalizationService.ForSection("ImageInfoSave.Report")("CapabilityState.Label"),
                                                                 LocalizationService.ForSection("ImageInfoSave.Report")("Web.Label")}.ToList())
                        For Each installedCapability As DismCapability In InstalledCapInfo
                            Contents &= GetTableRow(New String() {CodeBlockChar & installedCapability.Name & CodeBlockChar,
                                                                  Casters.CastDismPackageState(installedCapability.State),
                                                                  MarkdownHelper.GetLink(SearchEngineHelper.GetSearchQueryUri(String.Format("microsoft windows {0}{1}{0}", Quote, installedCapability.Name)), LocalizationService.ForSection("ImageInfoSave.Report")("Look.Item.Online.Label"))}.ToList())
                        Next
                        Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("CapabilityInfo.Missing.Label")) & CrLf
                    End If
                End Using
            Catch ex As Exception
                Debug.WriteLine("[GetCapabilityInformation] An error occurred while getting capability information: " & ex.ToString() & " - " & ex.Message)
                WriteExceptionInfo(ex)
            Finally
                DismApi.Shutdown()
            End Try
        End If
    End Sub

    Private Sub GetDriverInformation(GetEverything As Boolean, GetInboxDrivers As Boolean)
        Dim InstalledDrvInfo As DismDriverPackageCollection = Nothing
        Dim msg As String() = New String(3) {"", "", "", ""}
          msg(0) = LocalizationService.ForSection("ImageInfoSave.Drivers")("Preparing.Message")
          msg(1) = LocalizationService.ForSection("ImageInfoSave.Drivers")("Basic.Ready.Message") & LocalizationService.ForSection("ImageInfoSave.Drivers")("May.Take.Long.Message") & CrLf & CrLf & LocalizationService.ForSection("ImageInfoSave.Drivers")("Prompt.Label")
          msg(2) = LocalizationService.ForSection("ImageInfoSave.Drivers")("DriverInfo.Message")
          msg(3) = LocalizationService.ForSection("ImageInfoSave.GetDriverInfo")("BgProcessDetect.Message") & LocalizationService.ForSection("ImageInfoSave.Drivers")("Setting.Applied.Task.Message") & CrLf & CrLf & LocalizationService.ForSection("ImageInfoSave.Drivers")("Get.Message")
        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("DriverInfo.Label"), HeaderSize.Header2) & CrLf &
                    GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ImageFile.Get.Label") & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, LocalizationService.ForSection("ImageInfoSave.Report")("Active.Install.Label.Label")),
                                               LocalizationService.ForSection("ImageInfoSave.Report")("Box.Driver.Label") & If(AllDrivers, LocalizationService.ForSection("ImageInfoSave.Report")("WasSaved.Label"), LocalizationService.ForSection("ImageInfoSave.Report")("Saved.Label"))}.ToList()) & CrLf
        Debug.WriteLine("[GetDriverInformation] Starting task...")
        Try
            Debug.WriteLine("[GetDriverInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetDriverInformation] Creating image session...")
            Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                Debug.WriteLine("[GetDriverInformation] Getting basic driver information...")
                ReportChanges(msg(0), 5)
                InstalledDrvInfo = DismApi.GetDrivers(imgSession, GetInboxDrivers)
                Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("InfoSummary.Label") & InstalledDrvInfo.Count & LocalizationService.ForSection("ImageInfoSave.Report")("DriverS.Label"), ParagraphStyle.Bold) & CrLf
                msg(0) = LocalizationService.ForSection("ImageInfoSave.Drivers")("DriversObtained.Message")
                ReportChanges(msg(0), 10)
                If GetEverything Then
                    Contents &= CrLf & GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("PublishedName.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Original.File.Name.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("ProviderName.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("ClassName.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("ClassDescription"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("ClassGUID.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Catalog.File.Path.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Part.Windows.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Critical.Boot.Process.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Version.Column"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("Date.Label"),
                                                                    LocalizationService.ForSection("ImageInfoSave.Report")("SignatureStatus.Label")}.ToList())
                    Debug.WriteLine("[GetDriverInformation] Getting complete driver information...")
                    For Each driver As DismDriverPackage In InstalledDrvInfo
                        msg(0) = LocalizationService.ForSection("ImageInfoSave.Drivers").Format("Get.Driver.Message", InstalledDrvInfo.IndexOf(driver) + 1, InstalledDrvInfo.Count)
                        ReportChanges(msg(0), (InstalledDrvInfo.IndexOf(driver) / InstalledDrvInfo.Count) * 100)
                        Dim signer As String = DriverSignerViewer.GetSignerInfo(driver.OriginalFileName)
                        Contents &= GetTableRow(New String() {CodeBlockChar & driver.PublishedName & CodeBlockChar,
                                                              Path.GetFileName(driver.OriginalFileName) & " (" & Path.GetDirectoryName(driver.OriginalFileName) & ")",
                                                              driver.ProviderName,
                                                              driver.ClassName,
                                                              driver.ClassDescription,
                                                              driver.ClassGuid,
                                                              driver.CatalogFile,
                                                              If(driver.InBox, LocalizationService.ForSection("ImageInfoSave.Report")("Yes.Button"), LocalizationService.ForSection("ImageInfoSave.Report")("No.Button")),
                                                              If(driver.BootCritical, LocalizationService.ForSection("ImageInfoSave.Report")("Yes.Button"), LocalizationService.ForSection("ImageInfoSave.Report")("No.Button")),
                                                              driver.Version.ToString(),
                                                              driver.Date,
                                                              Casters.SignatureStatus(driver.DriverSignature) & If(Not (signer Is Nothing OrElse signer = ""), " by " & signer, "")}.ToList())
                    Next
                    Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("DriverInfo.Ready.Label")) & CrLf
                Else
                    msg(0) = LocalizationService.ForSection("ImageInfoSave.Drivers")("SaveDrivers.Message")
                    ReportChanges(msg(0), 50)
                    Contents &= GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("PublishedName.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("Original.File.Name.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("Part.Windows.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("ClassName.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("ProviderName.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("Date.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("Version.Column")}.ToList())
                    For Each installedDriver As DismDriverPackage In InstalledDrvInfo
                        Contents &= GetTableRow(New String() {CodeBlockChar & installedDriver.PublishedName & CodeBlockChar,
                                                              Path.GetFileName(installedDriver.OriginalFileName) & " (" & Path.GetDirectoryName(installedDriver.OriginalFileName) & ")",
                                                              If(installedDriver.InBox, LocalizationService.ForSection("ImageInfoSave.Report")("Yes.Button"), LocalizationService.ForSection("ImageInfoSave.Report")("No.Button")),
                                                              installedDriver.ClassName,
                                                              installedDriver.ProviderName,
                                                              installedDriver.Date,
                                                              installedDriver.Version.ToString()}.ToList())
                    Next
                    Contents &= CrLf & GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("DriverInfo.Missing.Label")) & CrLf
                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetDriverInformation] An error occurred while getting driver information: " & ex.ToString() & " - " & ex.Message)
            WriteExceptionInfo(ex)
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Private Sub GetDriverFileInformation()
        Dim msg As String = ""
        msg = LocalizationService.ForSection("ImgInfo.DriverFiles")("Preparing.Message")
        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("DriverPackage.Label"), HeaderSize.Header2) & CrLf & CrLf &
                    GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ImageFile.Get.Label") & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, LocalizationService.ForSection("ImageInfoSave.Report")("Active.Install.Label.Label"))}.ToList()) & CrLf
        Debug.WriteLine("[GetDriverFileInformation] Starting task...")
        Try
            Debug.WriteLine("[GetDriverFileInformation] Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            Debug.WriteLine("[GetDriverFileInformation] Creating image session...")
            ReportChanges(msg, 0)
            Using imgSession As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(ImgMountDir))
                Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("InfoSummary.Label") & DriverPkgs.Count & LocalizationService.ForSection("ImageInfoSave.Report")("DriverPackageS.Label"), ParagraphStyle.Bold) & CrLf
                For Each drvPkg In DriverPkgs
                    msg = LocalizationService.ForSection("ImgInfo.DriverFiles").Format("Loading.Driver.Message", DriverPkgs.IndexOf(drvPkg) + 1, DriverPkgs.Count)
                    ReportChanges(msg, (DriverPkgs.IndexOf(drvPkg) / DriverPkgs.Count) * 100)
                    If File.Exists(drvPkg) Then
                        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("DriverPackage.Driver.Label") & DriverPkgs.IndexOf(drvPkg) + 1 & LocalizationService.ForSection("ImageInfoSave.Report")("Value.Label") & DriverPkgs.Count & "", HeaderSize.Header3) & CrLf
                        Dim drvInfoCollection As DismDriverCollection = DismApi.GetDriverInfo(imgSession, drvPkg)
                        If drvInfoCollection.Count > 0 Then
                            Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("InfoSummary.Label") & drvInfoCollection.Count & LocalizationService.ForSection("ImageInfoSave.Report")("HardwareTargets.Label"), ParagraphStyle.Bold) & CrLf &
                                GetTableHeader(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Hardware.Description"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("HardwareID.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("CompatibleIds.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("ExcludeIds.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("Hardware.Manufacturer.Label"),
                                                             LocalizationService.ForSection("ImageInfoSave.Report")("Architecture.Label")}.ToList())
                            For Each hwTarget As DismDriver In drvInfoCollection
                                Contents &= GetTableRow(New String() {hwTarget.HardwareDescription,
                                                                      hwTarget.HardwareId,
                                                                      If(hwTarget.CompatibleIds = "", LocalizationService.ForSection("ImageInfoSave.Report")("None.Declared.Label"), hwTarget.CompatibleIds),
                                                                      If(hwTarget.ExcludeIds = "", LocalizationService.ForSection("ImageInfoSave.Report")("None.Declared.Label"), hwTarget.ExcludeIds),
                                                                      hwTarget.ManufacturerName,
                                                                      Casters.CastDismArchitecture(hwTarget.Architecture)}.ToList())
                            Next
                            Contents &= CrLf
                        Else
                            Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("File.Contains.Hardware.Label"), ParagraphStyle.Bold) & CrLf
                        End If
                    End If
                Next
            End Using
        Catch ex As Exception
            Debug.WriteLine("[GetDriverFileInformation] An error occurred while getting driver information: " & ex.ToString() & " - " & ex.Message)
            WriteExceptionInfo(ex)
        Finally
            DismApi.Shutdown()
        End Try
    End Sub

    Private Sub GetWinPEConfiguration()
        Dim msg As String = ""
        msg = LocalizationService.ForSection("ImageInfoSave.WinPE")("Prepare.Message")
        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("Windows.Label"), HeaderSize.Header2) & CrLf & CrLf
        If Not ImageToGetInfoFrom.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Then
            Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("UnsupportedWin.Message"), ParagraphStyle.Bold) & CrLf
            Exit Sub
        Else
            Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ImageFile.Get.Label") & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, LocalizationService.ForSection("ImageInfoSave.Report")("Active.Install.Label.Label"))}.ToList()) & CrLf
            Debug.WriteLine("[GetWinPEConfiguration] Starting task...")
            Debug.WriteLine("[GetWinPEConfiguration] Detecting target path...")
            ReportChanges(msg, 0)
            Dim regExitCode As Integer = RegistryHelper.LoadRegistryHive(Path.Combine(ImgMountDir, "Windows", "system32", "config", "SOFTWARE"), "HKLM\PE_SOFT")
            If regExitCode <> 0 Then
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Target.Path.Get.Label")}.ToList()) & CrLf
            End If
            regExitCode = RegistryHelper.LoadRegistryHive(Path.Combine(ImgMountDir, "Windows", "system32", "config", "SYSTEM"), "HKLM\PE_SYS")
            If regExitCode <> 0 Then
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ScratchSpace.Get.Value.Label")}.ToList()) & CrLf & CrLf
                Exit Sub
            End If
            Try
                msg = LocalizationService.ForSection("ImageInfoSave.WinPE")("Get.Target.Message")
                ReportChanges(msg, 50)
                ' Get target path first
                Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("PE_SOFT\Microsoft\Windows NT\CurrentVersion\WinPE", False)
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("TargetPath.Label") & regKey.GetValue("InstRoot", LocalizationService.ForSection("ImageInfoSave.Report")("GetValue.Label")).ToString()}.ToList()) & CrLf
                regKey.Close()
                msg = LocalizationService.ForSection("ImageInfoSave.WinPE")("Get.Scratch.Message")
                ReportChanges(msg, 75)
                regKey = Registry.LocalMachine.OpenSubKey("PE_SYS\ControlSet001\Services\FBWF", False)
                Dim scSize As String = regKey.GetValue("WinPECacheThreshold", "").ToString()
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ScratchSpace.Label") & If(Not scSize = "", scSize & " MB", LocalizationService.ForSection("ImageInfoSave.Report")("GetValue.Label"))}.ToList()) & CrLf & CrLf
                regKey.Close()
            Catch ex As Exception

            End Try
            ' Unload registry hives
            RegistryHelper.UnloadRegistryHive("HKLM\PE_SOFT")
            RegistryHelper.UnloadRegistryHive("HKLM\PE_SYS")
        End If
    End Sub

    Private Sub GetDefaultCSServiceInformation()
        Contents &= GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("ServiceInfo.Label"), HeaderSize.Header2) & CrLf &
                    GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ImageFile.Get.Label") & If(SourceImage <> "" And Not OnlineMode, Quote & SourceImage & Quote, LocalizationService.ForSection("ImageInfoSave.Report")("Active.Install.Label.Label"))}.ToList()) & CrLf
        ReportChanges(LocalizationService.ForSection("ImageInfoSave.Report")("Getting.Service.Label"), 0.0)
        Dim serviceList As List(Of WindowsService) = WindowsServiceHelper.GetServiceList(ImgMountDir, OnlineMode)
        If serviceList.Any() Then
            Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("InfoSummary.Label") & serviceList.Count & LocalizationService.ForSection("ImageInfoSave.Report")("Service.Default.Label"), ParagraphStyle.Bold) & CrLf &
                GetTableHeader({LocalizationService.ForSection("ImageInfoSave.Report")("ServiceName.Label"), LocalizationService.ForSection("ImageInfoSave.Report")("DisplayName.Column"), LocalizationService.ForSection("ImageInfoSave.Report")("Description"), LocalizationService.ForSection("ImageInfoSave.Report")("StartType.Label"), LocalizationService.ForSection("ImageInfoSave.Report")("ServiceType.Label"), LocalizationService.ForSection("ImageInfoSave.Report")("Web.Label")}.ToList())
            ' Do the service listing overview first; then do a loop again for each service.
            For Each service In serviceList
                ReportChanges(String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("Overview.Svc.Label"), serviceList.IndexOf(service) + 1, serviceList.Count),
                              (serviceList.IndexOf(service) / serviceList.Count) * 100)
                Contents &= GetTableRow({service.Name, service.DisplayName, service.Description, service.StartTypeToString(), service.TypeToString(),
                                         MarkdownHelper.GetLink(SearchEngineHelper.GetSearchQueryUri(String.Format("microsoft windows {0}{1}{0}", Quote, service.Name)),
                                                                LocalizationService.ForSection("ImageInfoSave.Report")("Look.Item.Online.Label"))}.ToList())
            Next
            Contents &= CrLf
            For Each service In serviceList
                ReportChanges(String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("Detailed.Svc.Label"), serviceList.IndexOf(service) + 1, serviceList.Count),
                              (serviceList.IndexOf(service) / serviceList.Count) * 100)

                Dim peruserServiceStatus As String = ""
                If {80, 96}.Contains(service.Type) Then
                    If service.UserServiceFlags = Integer.MinValue Then
                        peruserServiceStatus = LocalizationService.ForSection("ImageInfoSave.Report")("Undefined.Label")
                    Else
                        peruserServiceStatus = service.UserServiceFlags
                    End If
                Else
                    peruserServiceStatus = LocalizationService.ForSection("ImageInfoSave.Report")("Per.User.Service.Label")
                End If

                Contents &= GetHeader(String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("InfoService.Label"), service.Name), HeaderSize.Header3) & CrLf &
                    GetListItems({String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("Service.Display.Name.Label"), service.DisplayName),
                                  String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("Service.Description"), service.Description),
                                  String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("ImagePath.Label"), service.ImagePath),
                                  String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("ObjectName.Label"), service.ObjectName),
                                  String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("StartType.Option0.Label"), service.StartTypeToString()),
                                  String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("DelayedStart.Label"), If(service.StartType = WindowsService.ServiceStartType.Automatic AndAlso service.DelayedStart, LocalizationService.ForSection("ImageInfoSave.Report")("Yes.Button"), LocalizationService.ForSection("ImageInfoSave.Report")("No.Button"))),
                                  String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("ServiceType.Option0.Label"), service.TypeToString()),
                                  String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("Per.User.Flags.Label"), peruserServiceStatus),
                                  String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("Group.Label"), service.Group)}.ToList()) & CrLf &
                          GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("WindowsNt.Label"), ParagraphStyle.Bold) & CrLf &
                          GetTableHeader({LocalizationService.ForSection("ImageInfoSave.Report")("PrivilegeName.Label"), LocalizationService.ForSection("ImageInfoSave.Report")("Privilege.Display.Name.Label"), LocalizationService.ForSection("ImageInfoSave.Report")("Privilege.Description")}.ToList()) &
                          String.Join("", service.RequiredPrivileges.Select(Function(privilege) GetTableRow({privilege.ConstantNameText, privilege.ConstantUserRight, privilege.ConstantDescription}.ToList()))) & CrLf &
                          GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("ErrorControl.Label"), ParagraphStyle.Bold) & CrLf &
                          GetListItems({String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("ServiceError.Label"), service.ErrorControlToString()),
                                        String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("Failure.Action.First.Label"), service.FailureActionToString(service.FailureActions.FirstFailure)),
                                        String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("Failure.Action.Second.Label"), service.FailureActionToString(service.FailureActions.SecondFailure)),
                                        String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("Failure.Errors.Label"), service.FailureActionToString(service.FailureActions.SubsequentFailure)),
                                        String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("ResetErrorCount.Label"), service.FailureActions.ResetDelayInSeconds / 60),
                                        String.Format(LocalizationService.ForSection("ImageInfoSave.Report")("Restart.Service.Message"),
                                                      Math.Round((service.FailureActions.FirstDelayInMillis / 60000), 2),
                                                      Math.Round((service.FailureActions.FirstDelayInMillis / 1000), 2),
                                                      Math.Round((service.FailureActions.SecondDelayInMillis / 60000), 2),
                                                      Math.Round((service.FailureActions.SecondDelayInMillis / 1000), 2),
                                                      Math.Round((service.FailureActions.SubsequentDelaysInMillis / 60000), 2),
                                                      Math.Round((service.FailureActions.SubsequentDelaysInMillis / 1000), 2))}.ToList()) & CrLf &
                          GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Dependencies.Label"), ParagraphStyle.Bold) & CrLf &
                          GetTableHeader({LocalizationService.ForSection("ImageInfoSave.Report")("Name.Column"), LocalizationService.ForSection("ImageInfoSave.Report")("DisplayName.Column"), LocalizationService.ForSection("ImageInfoSave.Report")("Type.Label")}.ToList()) &
                          String.Join("", serviceList.Where(Function(srv) service.Dependencies.Contains(srv.Name)).OrderBy(Function(srv) srv.DisplayName).Select(Function(srv) GetTableRow({srv.Name, srv.DisplayName, srv.TypeToString()}.ToList()))) & CrLf &
                          GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Dependents.Label"), ParagraphStyle.Bold) & CrLf &
                          GetTableHeader({LocalizationService.ForSection("ImageInfoSave.Report")("Name.Column"), LocalizationService.ForSection("ImageInfoSave.Report")("DisplayName.Column"), LocalizationService.ForSection("ImageInfoSave.Report")("Type.Label")}.ToList()) &
                          String.Join("", serviceList.Where(Function(srv) srv.Dependencies.Contains(service.Name)).OrderBy(Function(srv) srv.DisplayName).Select(Function(srv) GetTableRow({srv.Name, srv.DisplayName, srv.TypeToString()}.ToList()))) & CrLf
            Next
        Else
            Contents &= GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("ServicesFound.Label"), ParagraphStyle.Bold) & CrLf
        End If
    End Sub

    Private Async Sub ImgInfoSaveDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not InfoSaveResults.IsDisposed Then
            InfoSaveResults.Close()
            InfoSaveResults.Dispose()
        End If
        OSVer = Environment.OSVersion.Version
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Height = WindowHelper.ScaleLogical(200)     ' tweak the height manually because Windows ain't doin' it!
        ProgressBar1.Width = WindowHelper.ScaleLogical(637)
        Visible = True
        Text = LocalizationService.ForSection("ImageInfoSave")("Saving.Image.Button")
        Label1.Text = LocalizationService.ForSection("ImageInfoSave")("Wait.Message")
        Label2.Text = LocalizationService.ForSection("ImageInfoSave")("Wait.Label")
        If MainForm.ImgBW.IsBusy Then
            Dim msg As String = ""
            msg = LocalizationService.ForSection("ImageInfoSave")("Wait.Background.Message")
            MsgBox(msg, vbOKOnly + vbInformation, Text)
            Label2.Text = LocalizationService.ForSection("ImageInfoSave")("Waiting.Bg.Procs.Item")
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.Indeterminate, MainForm.Handle)
            While MainForm.ImgBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(500)
            End While
        End If

        ' Stop the mounted image detector, as it makes the program crash when performing DISM API operations
        MainForm.StopMountedImageDetector()

        ' Close the image registry control panel before continuing. Operations with the DISM API open the image registry hives, something
        ' the control panel already loads. This causes the program to freeze for around a minute and then create a report with an
        ' exception thrown
        If RegistryControlPanel.Visible Then
            RegistryControlPanel.Close()
            If RegistryControlPanel.Visible Then
                Close()
                Exit Sub
            End If
        End If

        ' Create the target if it doesn't exist
        If Not File.Exists(SaveTarget) Then
            Try
                File.WriteAllText(SaveTarget, String.Empty)
            Catch ex As Exception
                Dim msg As String() = New String(1) {"", ""}
                msg(0) = LocalizationService.ForSection("ImageInfoSave")("Create.Target.Reason.Message")
                msg(1) = LocalizationService.ForSection("ImageInfoSave")("OperationFailed.Message")
                MsgBox(msg(0) & ex.ToString() & ": " & ex.Message, vbOKOnly + vbCritical, msg(1))
                Exit Sub
            End Try
        End If

        ' Set the beginning of the contents
        Contents = GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("DISM.Tools.Image.Title"), HeaderSize.Header1) &
                   GetParagraph(LocalizationService.ForSection("ImageInfoSave.Report")("Automatically.Message") & CrLf & CrLf &
                                LocalizationService.ForSection("ImageInfoSave.Report")("Report.Contains.Message") & CrLf & CrLf &
                                LocalizationService.ForSection("ImageInfoSave.Report")("Process.Primarily.Message") & Quote & Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\logs\DISM\DISM.log" & Quote & CrLf, ParagraphStyle.Normal) & CrLf &
                   GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("TaskDetails.Label"), HeaderSize.Header2) & CrLf &
                   GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("ProcessesStarted.Label") & Date.Now, LocalizationService.ForSection("ImageInfoSave.Report")("Report.File.Target.Label") & Quote & SaveTarget & Quote}.ToList())

        If OfflineMode Then SourceImage = ImgMountDir

        ' Disable logger to avoid degraded performance
        DynaLog.DisableLogging()

        Dim TaskMessages As New List(Of String),
            TaskTitles As New List(Of String)

        TaskTitles.AddRange({LocalizationService.ForSection("ImageInfoSave")("PackageInfo.Label"), LocalizationService.ForSection("ImageInfoSave")("FeatureInfo.Label"), LocalizationService.ForSection("ImageInfoSave")("AppX.Package.Label"), LocalizationService.ForSection("ImageInfoSave")("CapabilityInfo.Label"), LocalizationService.ForSection("ImageInfoSave")("DriverInfo.Label")})
        TaskMessages.AddRange({LocalizationService.ForSection("ImageInfoSave")("Desea.Obtener.Message"),
                               LocalizationService.ForSection("ImageInfoSave")("Statement3.Message"),
                               LocalizationService.ForSection("ImageInfoSave")("Statement4.Message"),
                               LocalizationService.ForSection("ImageInfoSave")("Statement5.Message"),
                               LocalizationService.ForSection("ImageInfoSave")("Statement6.Message")})

        Dim GetEveryPackage As Boolean = True,
            GetEveryFeature As Boolean = True,
            GetEveryAppxPackage As Boolean = True,
            GetEveryCapability As Boolean = True,
            GetEveryDriver As Boolean = True
        Select Case SaveTask
            Case 0
                If Not SkipQuestions Or Not AutoCompleteInfo(0) Then
                    GetEveryPackage = MessageBox.Show(TaskMessages(0), TaskTitles(0), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                End If
                If Not SkipQuestions Or Not AutoCompleteInfo(1) Then
                    GetEveryFeature = MessageBox.Show(TaskMessages(1), TaskTitles(1), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                End If
                If Environment.OSVersion.Version.Major = 10 AndAlso (Not SkipQuestions Or Not AutoCompleteInfo(2)) Then
                    GetEveryAppxPackage = MessageBox.Show(TaskMessages(2), TaskTitles(2), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                End If
                If Not SkipQuestions Or Not AutoCompleteInfo(3) Then
                    GetEveryCapability = MessageBox.Show(TaskMessages(3), TaskTitles(3), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                End If
                If Not SkipQuestions Or Not AutoCompleteInfo(4) Then
                    GetEveryDriver = MessageBox.Show(TaskMessages(4), TaskTitles(4), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                End If
            Case 2
                If Not SkipQuestions Or Not AutoCompleteInfo(0) Then
                    GetEveryPackage = MessageBox.Show(TaskMessages(0), TaskTitles(0), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                End If
            Case 4
                If Not SkipQuestions Or Not AutoCompleteInfo(1) Then
                    GetEveryFeature = MessageBox.Show(TaskMessages(1), TaskTitles(1), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                End If
            Case 5
                If Environment.OSVersion.Version.Major = 10 AndAlso (Not SkipQuestions Or Not AutoCompleteInfo(2)) Then
                    GetEveryAppxPackage = MessageBox.Show(TaskMessages(2), TaskTitles(2), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                End If
            Case 6
                If Not SkipQuestions Or Not AutoCompleteInfo(3) Then
                    GetEveryCapability = MessageBox.Show(TaskMessages(3), TaskTitles(3), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                End If
            Case 7
                If Not SkipQuestions Or Not AutoCompleteInfo(4) Then
                    GetEveryDriver = MessageBox.Show(TaskMessages(4), TaskTitles(4), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
                End If
        End Select

        ' Begin performing operations
        Select Case SaveTask
            Case 0
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Tasks.Get.Complete.Label")}.ToList()) & CrLf & CrLf

                Await Task.Run(Sub()
                                   GetImageInformation()
                                   GetPackageInformation((SkipQuestions And AutoCompleteInfo(0)) OrElse ((Not SkipQuestions Or Not AutoCompleteInfo(0)) And GetEveryPackage))
                                   GetFeatureInformation((SkipQuestions And AutoCompleteInfo(1)) OrElse ((Not SkipQuestions Or Not AutoCompleteInfo(1)) And GetEveryFeature))
                                   GetAppxInformation((SkipQuestions And AutoCompleteInfo(2)) OrElse ((Not SkipQuestions Or Not AutoCompleteInfo(2)) And GetEveryAppxPackage))
                                   GetCapabilityInformation((SkipQuestions And AutoCompleteInfo(3)) OrElse ((Not SkipQuestions Or Not AutoCompleteInfo(3)) And GetEveryCapability))
                                   GetDriverInformation((SkipQuestions And AutoCompleteInfo(4)) OrElse ((Not SkipQuestions Or Not AutoCompleteInfo(4)) And GetEveryDriver), False)
                                   GetWinPEConfiguration()
                                   GetDefaultCSServiceInformation()
                               End Sub)
            Case 1
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Tasks.Get.Image.Label")}.ToList()) & CrLf & CrLf
                Await Task.Run(Sub()
                                   GetImageInformation()
                               End Sub)
            Case 2
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Tasks.Get.Installed.Label")}.ToList()) & CrLf & CrLf
                Await Task.Run(Sub()
                                   GetPackageInformation((SkipQuestions And AutoCompleteInfo(0)) OrElse ((Not SkipQuestions Or Not AutoCompleteInfo(0)) And GetEveryPackage))
                               End Sub)
            Case 3
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Tasks.Get.Package.Label")}.ToList()) & CrLf & CrLf
                Await Task.Run(Sub()
                                   GetPackageFileInformation()
                               End Sub)
            Case 4
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Tasks.Get.Feature.Label")}.ToList()) & CrLf & CrLf
                Await Task.Run(Sub()
                                   GetFeatureInformation((SkipQuestions And AutoCompleteInfo(1)) OrElse ((Not SkipQuestions Or Not AutoCompleteInfo(1)) And GetEveryFeature))
                               End Sub)
            Case 5
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("TaskInstalled.Label")}.ToList()) & CrLf & CrLf
                Await Task.Run(Sub()
                                   GetAppxInformation((SkipQuestions And AutoCompleteInfo(2)) OrElse ((Not SkipQuestions Or Not AutoCompleteInfo(2)) And GetEveryAppxPackage))
                               End Sub)
            Case 6
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Tasks.Get.Capability.Label")}.ToList()) & CrLf & CrLf
                Await Task.Run(Sub()
                                   GetCapabilityInformation((SkipQuestions And AutoCompleteInfo(3)) OrElse ((Not SkipQuestions Or Not AutoCompleteInfo(3)) And GetEveryCapability))
                               End Sub)
            Case 7
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Tasks.Get.Driver.Label")}.ToList()) & CrLf & CrLf

                Dim InboxDriverMessage As String = ""
                InboxDriverMessage = LocalizationService.ForSection("ImageInfoSave")("BgProcessDetect.Message") & LocalizationService.ForSection("ImageInfoSave")("Setting.Applied.Task.Message") & CrLf & CrLf & LocalizationService.ForSection("ImageInfoSave")("Get.Message")

                Dim GetInboxDrivers As Boolean = True
                If Not AllDrivers Then GetInboxDrivers = MessageBox.Show(InboxDriverMessage, TaskTitles(4), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes

                Await Task.Run(Sub()
                                   GetDriverInformation((SkipQuestions And AutoCompleteInfo(4)) OrElse ((Not SkipQuestions Or Not AutoCompleteInfo(4)) And GetEveryDriver), GetInboxDrivers)
                               End Sub)
            Case 8
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Tasks.Get.Label.Label")}.ToList()) & CrLf & CrLf
                Await Task.Run(Sub()
                                   GetDriverFileInformation()
                               End Sub)
            Case 9
                Contents &= GetListItems(New String() {LocalizationService.ForSection("ImageInfoSave.Report")("Tasks.Get.Windows.Label")}.ToList()) & CrLf & CrLf
                Await Task.Run(Sub()
                                   GetWinPEConfiguration()
                               End Sub)
            Case 10
                Contents &= GetListItems({LocalizationService.ForSection("ImageInfoSave.Report")("Tasks.Get.Services.Label")}.ToList()) & CrLf & CrLf
                Await Task.Run(Sub()
                                   GetDefaultCSServiceInformation()
                               End Sub)
        End Select

        ' Put an ending to the contents
        Contents &= CrLf & CrLf & GetHeader(LocalizationService.ForSection("ImageInfoSave.Report")("WeEnded.Label") & Date.Now & LocalizationService.ForSection("ImageInfoSave.Report")("NiceDay.Label"), HeaderSize.Header2)

        ' Inform user that we are saving the file
        Dim saveMsg As String = ""
        saveMsg = LocalizationService.ForSection("ImageInfoSave")("SavingContents.Message")
        ReportChanges(saveMsg, ProgressBar1.Maximum)
        TaskbarHelper.SetIndicatorState(ProgressBar1.Maximum, Windows.Shell.TaskbarItemProgressState.None, MainForm.Handle)

        ' Enable the logger again
        DynaLog.EnableLogging()

        ' Save the file
        If Contents <> "" And File.Exists(SaveTarget) Then File.WriteAllText(SaveTarget, Contents, UTF8)
        If Debugger.IsAttached Then Process.Start(SaveTarget)
        InfoSaveResults.FilePath = SaveTarget
        MainForm.StartMountedImageDetector()
        Close()
    End Sub

End Class
