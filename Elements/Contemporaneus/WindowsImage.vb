Imports System.Globalization
Imports System.IO
Imports Microsoft.Dism
Imports Microsoft.Win32
Imports DISMTools.Utilities

Namespace Elements.Contemporaneus

    ''' <summary>
    ''' The base class for Windows images
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WindowsImage

        ''' <summary>
        ''' The Globally Unique Identifier (GUID) of the mount path of the image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageMountGuid As Guid

#Region "Mounted Image Information (in Mounted Image Manager)"

        ''' <summary>
        ''' The path to the image file in question.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageFile As String = ""

        ''' <summary>
        ''' The index of the mounted image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageIndex As Integer

        ''' <summary>
        ''' The path to the folder the image has been mounted to.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageMountDirectory As String = ""

        ''' <summary>
        ''' The status of the mounted image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageMountStatus As DismMountStatus

        ''' <summary>
        ''' The mode of the mounted image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageMountMode As DismMountMode

#End Region

#Region "Complete Image Information (Basic + Advanced Image Info Getter for Background Processes)"

        ''' <summary>
        ''' The name of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageName As String = ""

        ''' <summary>
        ''' The description of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageDescription As String = ""

        ''' <summary>
        ''' The size, in bytes, of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageSize As ULong

        ''' <summary>
        ''' The compatibility status of Windows Image File Boot (WIMBoot) on the mounted
        ''' Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' WIMBoot can only be enabled on Windows 8.1 when capturing an image 
        ''' with the /wimboot option.
        ''' </remarks>
        Public Property ImageWimBootCompatible As Boolean

        ''' <summary>
        ''' The processor architecture of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageArchitecture As DismProcessorArchitecture

        ''' <summary>
        ''' The Hardware Abstraction Layer (HAL) of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' The image may not have a default HAL. This does not happen for captured Windows images
        ''' in some cases.
        ''' </remarks>
        Public Property ImageHal As String = ""

        ''' <summary>
        ''' The version of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageVersion As Version

        ''' <summary>
        ''' The Service Pack build part of the version of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageSpBuild As Integer

        ''' <summary>
        ''' The Service Pack level of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageSpLevel As Integer

        ''' <summary>
        ''' The edition identifier (ID) of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageEditionId As String = ""

        ''' <summary>
        ''' Determines whether a certain Windows image is a Windows PE image in disguise.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Certain Windows PE images, like the Sergei Strelec WinPE, have set their EditionIDs
        ''' to things other than WindowsPE.
        ''' </remarks>
        Public Property WinPeInDisguise As Boolean

        ''' <summary>
        ''' The installation type of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageInstallationType As String = ""

        ''' <summary>
        ''' The product type of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageProductType As String = ""

        ''' <summary>
        ''' The product suite of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageProductSuite As String = ""

        ''' <summary>
        ''' The system root directory of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageSystemRoot As String = ""

        ''' <summary>
        ''' The number of directories in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageDirectoryCount As Integer

        ''' <summary>
        ''' The number of files in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageFileCount As Integer

        ''' <summary>
        ''' The creation date of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageCreationDate As Date

        ''' <summary>
        ''' The modification date of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageModificationDate As Date

        ''' <summary>
        ''' The installed languages in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageLanguages As IEnumerable(Of CultureInfo)

        ''' <summary>
        ''' The default language of the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageDefaultLanguage As CultureInfo

#End Region

#Region "Items obtained by remaining background processes"

        ''' <summary>
        ''' The packages available in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImagePackages As DismPackageCollection

        ''' <summary>
        ''' The packages available in the mounted Windows image
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>This collection is filled when the program can't get the info with the DISM API</remarks>
        Public Property ImagePackages_Backup As List(Of ImagePackage)

        ''' <summary>
        ''' The features available in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageFeatures As DismFeatureCollection

        ''' <summary>
        ''' The features available in the mounted Windows image
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>This collection is filled when the program can't get the info with the DISM API</remarks>
        Public Property ImageFeatures_Backup As List(Of ImageFeature)

        ''' <summary>
        ''' The Microsoft Store packages (AppX packages) available in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>This collection is filled on hosts running Windows 10 and later.</remarks>
        Public Property ImageAppxPackages As DismAppxPackageCollection

        ''' <summary>
        ''' The Microsoft Store packages (AppX packages) available in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>This collection is filled on hosts running Windows 8.</remarks>
        Public Property ImageAppxPackages_Backup As List(Of ImageAppxPackage)

        ''' <summary>
        ''' The capabilities/Features on Demand available in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageCapabilities As DismCapabilityCollection

        ''' <summary>
        ''' The capabilities/Features on Demand available in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>This collection is filled when the program can't get the info with the DISM API</remarks>
        Public Property ImageCapabilities_Backup As List(Of ImageCapability)

        ''' <summary>
        ''' The device drivers installed in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImageDrivers As DismDriverPackageCollection

        ''' <summary>
        ''' The device drivers installed in the mounted Windows image.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>This collection is filled on Windows 7 images.</remarks>
        Public Property ImageDrivers_Backup As List(Of ImageDriver)

#End Region

#Region "Full Flash Utility only"

        Public Property FFUInfo As FullFlashUtilityImage

#End Region

        ''' <summary>
        ''' Creates a <see cref="WindowsImage"/> object instance with default values.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            ImageVersion = New Version(0, 0, 0, 0)
            ImagePackages_Backup = New List(Of ImagePackage)
            ImageFeatures_Backup = New List(Of ImageFeature)
            ImageAppxPackages_Backup = New List(Of ImageAppxPackage)
            ImageCapabilities_Backup = New List(Of ImageCapability)
            ImageDrivers_Backup = New List(Of ImageDriver)
            FFUInfo = New FullFlashUtilityImage()
        End Sub

        ''' <summary>
        ''' Creates a <see cref="WindowsImage"/> object instance with file path, index, mount directory, status, 
        ''' and mode parameters; and default values for everything else.
        ''' </summary>
        ''' <param name="filePath">The path to the Windows image file</param>
        ''' <param name="index">The index of the mounted Windows image</param>
        ''' <param name="mountDir">The directory the Windows image is mounted to</param>
        ''' <param name="mountStatus">The mount status of the mounted Windows image</param>
        ''' <param name="mountMode">The mount mode of the mounted Windows image</param>
        Public Sub New(filePath As String, index As Integer, mountDir As String, mountStatus As DismMountStatus, mountMode As DismMountMode)
            ImageFile = filePath
            ImageIndex = index
            ImageMountDirectory = mountDir
            ImageMountStatus = mountStatus
            ImageMountMode = mountMode

            ImageVersion = New Version(0, 0, 0, 0)
            ImagePackages_Backup = New List(Of ImagePackage)
            ImageFeatures_Backup = New List(Of ImageFeature)
            ImageAppxPackages_Backup = New List(Of ImageAppxPackage)
            ImageCapabilities_Backup = New List(Of ImageCapability)
            ImageDrivers_Backup = New List(Of ImageDriver)

            FFUInfo = New FullFlashUtilityImage()
        End Sub

        ''' <summary>
        ''' Gets the Globally Unique Identifier (GUID) of the mounted Windows image.
        ''' </summary>
        ''' <returns>The GUID assigned to the mounted Windows image in the Registry.</returns>
        Public Function GetImageMountGuid() As Guid
            Dim mountGuid As Guid
            Dim found As Boolean = False

            Try
                Dim wimmountRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\WIMMount\Mounted Images", False)
                For Each subkeyName In wimmountRk.GetSubKeyNames()
                    Try
                        Dim subkeyRk As RegistryKey = wimmountRk.OpenSubKey(subkeyName, False)
                        If subkeyRk.GetValue("Mount Path", "").Equals(ImageMountDirectory) Then
                            mountGuid = New Guid(subkeyName)
                        End If
                        subkeyRk.Close()

                        If found Then Exit For
                    Catch ex As Exception

                    End Try
                Next
                wimmountRk.Close()
            Catch ex As Exception

            End Try
            Return mountGuid
        End Function

        ''' <summary>
        ''' Gets the version of the mounted Windows image.
        ''' </summary>
        ''' <returns>The version of the mounted Windows image</returns>
        Private Function GetImageVersion() As Version
            Dim osVersion As Version = New Version(0, 0, 0, 0)

            Try
                DismApi.Initialize(DismLogLevel.LogErrors)
                osVersion = DismApi.GetImageInfo(ImageFile).ElementAt(ImageIndex - 1).ProductVersion
            Catch ex As Exception
                Dim kernelPath As String = Path.Combine(ImageMountDirectory, "Windows", "system32", "ntoskrnl.exe")
                Try
                    If File.Exists(kernelPath) Then
                        Return New Version(FileVersionInfo.GetVersionInfo(kernelPath).ProductVersion)
                    Else
                        Return New Version(0, 0, 0, 0)
                    End If
                Catch ex2 As Exception
                    Return New Version(0, 0, 0, 0)
                End Try
            Finally
                Try
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try

            Return osVersion
        End Function

        ''' <summary>
        ''' Gets a localized string displaying mount status
        ''' </summary>
        ''' <returns>The localized string</returns>
        Public Function MountStatusToString() As String
            Select Case ImageMountStatus
                Case DismMountStatus.Ok
                    Return LocalizationService.ForSection("WindowsImage.MountStatus")("Ok.Button")
                Case DismMountStatus.NeedsRemount
                    Return LocalizationService.ForSection("WindowsImage.MountStatus")("NeedsRemount.Label")
                Case DismMountStatus.Invalid
                    Return LocalizationService.ForSection("WindowsImage.MountStatus")("Invalid.Label")
            End Select

            Return ""
        End Function

        ''' <summary>
        ''' Gets a localized string displaying mount mode
        ''' </summary>
        ''' <returns>The localized string</returns>
        Public Function MountModeToString() As String
            Select Case ImageMountMode
                Case DismMountMode.ReadWrite
                    Return LocalizationService.ForSection("WindowsImage.MountMode")("Yes.Button")
                Case DismMountMode.ReadOnly
                    Return LocalizationService.ForSection("WindowsImage.MountMode")("No.Button")
            End Select

            Return ""
        End Function

        ''' <summary>
        ''' Gets all obtained information of a <see cref="WindowsImage"/> object.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return String.Format("Image information:{0}" &
                                 "- Image version: {1}{0}" &
                                 "- Image name: {2}{0}" &
                                 "- Image description: {3}{0}" &
                                 "- Image HAL: {4}{0}" &
                                 "- Image architecture: {5}{0}" &
                                 "- Image Service Pack build: {6}{0}" &
                                 "- Image Service Pack level: {7}{0}" &
                                 "- Image Edition ID: {8}{0}" &
                                 "- Image product type: {9}{0}" &
                                 "- Image product suite: {10}{0}" &
                                 "- Image system root: {11}{0}" &
                                 "- Image languages: {12}{0}" &
                                 "- Image format: {13}{0}" &
                                 "- Does image have read/write permissions? {14}{0}" &
                                 "- Image directory count: {15}{0}" &
                                 "- Image file count: {16}{0}" &
                                 "- Image creation time: {17}{0}" &
                                 "- Image modification time: {18}{0}" &
                                 "- Image installation type: {19}{0}" &
                                 "-------- Mount Point GUID: {20}{0}" &
                                 "-------- FFU Information: {0}{21}",
                                 Environment.NewLine,
                                 ImageVersion.ToString(),
                                 ImageName,
                                 ImageDescription,
                                 ImageHal,
                                 Casters.CastDismArchitecture(ImageArchitecture),
                                 ImageSpBuild,
                                 ImageSpLevel,
                                 String.Format("{0}{1}", ImageEditionId, If(WinPeInDisguise, " (in reality a heavily modified WinPE image)", "")),
                                 ImageProductType,
                                 ImageProductSuite,
                                 ImageSystemRoot,
                                 String.Join(", ", ImageLanguages.Select(Function(language) language.EnglishName).ToArray()),
                                 Path.GetExtension(ImageFile).ToUpper().TrimStart("."),
                                 If(ImageMountMode = DismMountMode.ReadWrite, "Yes", "No"),
                                 ImageDirectoryCount,
                                 ImageFileCount,
                                 ImageCreationDate,
                                 ImageModificationDate,
                                 ImageInstallationType,
                                 ImageMountGuid,
                                 If(Path.GetExtension(ImageFile).EndsWith("ffu", StringComparison.OrdinalIgnoreCase), FFUInfo.ToString(), ""))
        End Function
    End Class

End Namespace
