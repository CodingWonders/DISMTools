Imports Microsoft.Dism
Imports System.IO

Namespace Utilities

    ''' <summary>
    ''' All caster functions belong here
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Casters

        ''' <summary>
        ''' Casts the processor architecture enumerators from the DISM API into readable text
        ''' </summary>
        ''' <param name="Arch">The DISM processor architecture</param>
        ''' <param name="Translate">Whether the readable text should be translated to the appropriate language</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function CastDismArchitecture(Arch As DismProcessorArchitecture, Optional Translate As Boolean = False) As String
            Select Case Arch
                Case DismProcessorArchitecture.None
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.DISMArchitecture")("Unknown.Label")
                    Else
                        Return "Unknown"
                    End If
                Case DismProcessorArchitecture.Neutral
                    Return LocalizationService.ForSection("Casters.DISMArchitecture")("Neutral.Label")
                Case DismProcessorArchitecture.Intel
                    Return "x86"
                Case DismProcessorArchitecture.IA64
                    Return "Itanium"
                Case DismProcessorArchitecture.ARM64
                    Return "ARM64"
                Case DismProcessorArchitecture.ARM
                    Return "ARM"
                Case DismProcessorArchitecture.AMD64
                    Return "AMD64"
            End Select
            Return Nothing
        End Function

        Shared Function CastDismArchitectureString(ArchitectureString As String) As DismProcessorArchitecture
            Select Case ArchitectureString.ToLower()
                Case "x86"
                    Return DismProcessorArchitecture.Intel
                Case "x64"
                    Return DismProcessorArchitecture.AMD64
                Case "arm"
                    Return DismProcessorArchitecture.ARM
                Case "arm64"
                    Return DismProcessorArchitecture.ARM64
                Case "neutral"
                    Return DismProcessorArchitecture.Neutral
            End Select
            Return DismProcessorArchitecture.None
        End Function

        Shared Function SignatureStatus(Signature As DismDriverSignature, Optional Translate As Boolean = False) As String
            Select Case Signature
                Case DismDriverSignature.Unknown
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.SignatureStatus")("Unknown.Label")
                    Else
                        Return "Unknown"
                    End If
                Case DismDriverSignature.Unsigned
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.SignatureStatus")("UnsignedValidity.Message")
                    Else
                        Return "Unsigned. Please check the validity and expiration date of the signing certificate"
                    End If
                Case DismDriverSignature.Signed
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.SignatureStatus")("Signed.Label")
                    Else
                        Return "Signed"
                    End If
            End Select
            Return Nothing
        End Function

        Shared Function CastDismPackageState(State As DismPackageFeatureState, Optional Translate As Boolean = False) As String
            Select Case State
                Case DismPackageFeatureState.NotPresent
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Present.Label")
                    Else
                        Return "Not present"
                    End If
                Case DismPackageFeatureState.UninstallPending
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("UninstallPending.Label")
                    Else
                        Return "Uninstall Pending"
                    End If
                Case DismPackageFeatureState.Staged
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Uninstalled.Label")
                    Else
                        Return "Uninstalled"
                    End If
                Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Removed.Label")
                    Else
                        Return "Removed"
                    End If
                Case DismPackageFeatureState.Installed
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Installed.Label")
                    Else
                        Return "Installed"
                    End If
                Case DismPackageFeatureState.InstallPending
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("InstallPending.Label")
                    Else
                        Return "Install Pending"
                    End If
                Case DismPackageFeatureState.Superseded
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Superseded.Label")
                    Else
                        Return "Superseded"
                    End If
                Case DismPackageFeatureState.PartiallyInstalled
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Partially.Installed.Label")
                    Else
                        Return "Partially Installed"
                    End If
            End Select
            Return Nothing
        End Function

        Shared Function CastDismPackageStateString(StateString As String) As DismPackageFeatureState
            Dim obtainedState As DismPackageFeatureState = DismPackageFeatureState.NotPresent
            If StateString = "Not Present" Then
                obtainedState = DismPackageFeatureState.NotPresent
            ElseIf StateString = "Uninstall Pending" Then
                obtainedState = DismPackageFeatureState.UninstallPending
            ElseIf StateString = "Staged" Then
                obtainedState = DismPackageFeatureState.Staged
            ElseIf StateString = "Removed" Then
                obtainedState = DismPackageFeatureState.Removed
            ElseIf StateString = "Installed" Then
                obtainedState = DismPackageFeatureState.Installed
            ElseIf StateString = "Install Pending" Then
                obtainedState = DismPackageFeatureState.InstallPending
            ElseIf StateString = "Superseded" Then
                obtainedState = DismPackageFeatureState.Superseded
            ElseIf StateString = "Partially Installed" Then
                obtainedState = DismPackageFeatureState.PartiallyInstalled
            End If
            Return obtainedState
        End Function

        Shared Function CastDismFeatureState(State As DismPackageFeatureState, Optional Translate As Boolean = False) As String
            Select Case State
                Case DismPackageFeatureState.NotPresent
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Present.Label")
                    Else
                        Return "Not present"
                    End If
                Case DismPackageFeatureState.UninstallPending
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("DisablePending.Label")
                    Else
                        Return "Disable Pending"
                    End If
                Case DismPackageFeatureState.Staged
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Staged.Label")
                    Else
                        Return "Disabled"
                    End If
                Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Removed.Label")
                    Else
                        Return "Removed"
                    End If
                Case DismPackageFeatureState.Installed
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Installed.Label")
                    Else
                        Return "Enabled"
                    End If
                Case DismPackageFeatureState.InstallPending
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("EnablePending.Label")
                    Else
                        Return "Enable Pending"
                    End If
                Case DismPackageFeatureState.Superseded
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Superseded.Label")
                    Else
                        Return "Superseded"
                    End If
                Case DismPackageFeatureState.PartiallyInstalled
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Partially.Installed.Label")
                    Else
                        Return "Partially Installed"
                    End If
            End Select
            Return Nothing
        End Function

        Shared Function CastDismRestartType(RestartType As DismRestartType, Optional Translate As Boolean = False) As String
            Select Case RestartType
                Case DismRestartType.No
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("NotRequired.Label")
                    Else
                        Return "A restart is not required"
                    End If
                Case DismRestartType.Possible
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("May.Required.Label")
                    Else
                        Return "A restart may be required"
                    End If
                Case DismRestartType.Required
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Required.Label")
                    Else
                        Return "A restart is required"
                    End If
            End Select
            Return Nothing
        End Function

        Shared Function Applicability(AppState As Boolean, Optional Translate As Boolean = False) As String
            Select Case AppState
                Case True
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Applicability")("Yes.Button")
                    Else
                        Return "Yes"
                    End If
                Case False
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Applicability")("No.Button")
                    Else
                        Return "No"
                    End If
            End Select
            Return Nothing
        End Function

        Shared Function CastDismReleaseType(RelType As DismReleaseType, Optional Translate As Boolean = False) As String
            Select Case RelType
                Case DismReleaseType.CriticalUpdate
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("CriticalUpdate.Label")
                    Else
                        Return "Critical update"
                    End If
                Case DismReleaseType.Driver
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Driver.Label")
                    Else
                        Return "Driver"
                    End If
                Case DismReleaseType.FeaturePack
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("FeaturePack.Label")
                    Else
                        Return "Feature Pack"
                    End If
                Case DismReleaseType.Foundation
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Foundation.Package.Label")
                    Else
                        Return "Foundation package"
                    End If
                Case DismReleaseType.Hotfix
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Hotfix.Label")
                    Else
                        Return "Hotfix"
                    End If
                Case DismReleaseType.LanguagePack
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("LanguagePack.Label")
                    Else
                        Return "Language pack"
                    End If
                Case DismReleaseType.LocalPack
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("LocalPack.Label")
                    Else
                        Return "Local pack"
                    End If
                Case DismReleaseType.OnDemandPack
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("DemandPack.Label")
                    Else
                        Return "On Demand pack"
                    End If
                Case DismReleaseType.Other
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Other.Label")
                    Else
                        Return "Other"
                    End If
                Case DismReleaseType.Product
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Product.Label")
                    Else
                        Return "Product"
                    End If
                Case DismReleaseType.SecurityUpdate
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("SecurityUpdate.Label")
                    Else
                        Return "Security update"
                    End If
                Case DismReleaseType.ServicePack
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("ServicePack.Label")
                    Else
                        Return "Service Pack"
                    End If
                Case DismReleaseType.SoftwareUpdate
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("SoftwareUpdate.Label")
                    Else
                        Return "Software update"
                    End If
                Case DismReleaseType.Update
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("Update.Label")
                    Else
                        Return "Update"
                    End If
                Case DismReleaseType.UpdateRollup
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.Cast.DISM")("UpdateRollup.Label")
                    Else
                        Return "Update rollup"
                    End If
            End Select
            Return Nothing
        End Function

        Shared Function CastDismReleaseTypeString(ReleaseTypeString As String) As DismReleaseType
            Dim obtainedReleaseType As DismReleaseType = DismReleaseType.Other
            If ReleaseTypeString = "Critical Update" Then
                obtainedReleaseType = DismReleaseType.CriticalUpdate
            ElseIf ReleaseTypeString = "Driver" Then
                obtainedReleaseType = DismReleaseType.Driver
            ElseIf ReleaseTypeString = "Feature Pack" Then
                obtainedReleaseType = DismReleaseType.FeaturePack
            ElseIf ReleaseTypeString = "Hotfix" Then
                obtainedReleaseType = DismReleaseType.Hotfix
            ElseIf ReleaseTypeString = "Security Update" Then
                obtainedReleaseType = DismReleaseType.SecurityUpdate
            ElseIf ReleaseTypeString = "Software Update" Then
                obtainedReleaseType = DismReleaseType.SoftwareUpdate
            ElseIf ReleaseTypeString = "Update" Then
                obtainedReleaseType = DismReleaseType.Update
            ElseIf ReleaseTypeString = "Update Rollup" Then
                obtainedReleaseType = DismReleaseType.UpdateRollup
            ElseIf ReleaseTypeString = "Language Pack" Then
                obtainedReleaseType = DismReleaseType.LanguagePack
            ElseIf ReleaseTypeString = "Foundation" Then
                obtainedReleaseType = DismReleaseType.Foundation
            ElseIf ReleaseTypeString = "Service Pack" Then
                obtainedReleaseType = DismReleaseType.ServicePack
            ElseIf ReleaseTypeString = "Product" Then
                obtainedReleaseType = DismReleaseType.Product
            ElseIf ReleaseTypeString = "Local Pack" Then
                obtainedReleaseType = DismReleaseType.LocalPack
            ElseIf ReleaseTypeString = "Other" Then
                obtainedReleaseType = DismReleaseType.Other
            ElseIf ReleaseTypeString = "OnDemand Pack" Then
                obtainedReleaseType = DismReleaseType.OnDemandPack
            End If
            Return obtainedReleaseType
        End Function

        Shared Function OfflineInstallType(foiType As DismFullyOfflineInstallableType, Optional Translate As Boolean = False) As String
            Select Case foiType
                Case DismFullyOfflineInstallableType.FullyOfflineNotInstallable
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.OfflineInstall.Boot")("Required.Message")
                    Else
                        Return "A boot up to the target image is required to fully install this package"
                    End If
                Case DismFullyOfflineInstallableType.FullyOfflineInstallableUndetermined
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.OfflineInstall")("FullyOffline.Message")
                    Else
                        Return "A boot up to the target image may be required to fully install this package"
                    End If
                Case DismFullyOfflineInstallableType.FullyOfflineInstallable
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.OfflineInstall.Boot")("NotRequired.Message")
                    Else
                        Return "A boot up to the target image is not required to fully install this package"
                    End If
            End Select
            Return Nothing
        End Function

        Shared Function CastDriveType(DrType As DriveType, Optional Translate As Boolean = False) As String
            Select Case DrType
                Case DriveType.CDRom
                    Return "CD-ROM"
                Case DriveType.Fixed
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.CastDriveType")("Fixed.Label")
                    Else
                        Return "Fixed"
                    End If
                Case DriveType.Network
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.CastDriveType")("Network.Label")
                    Else
                        Return "Network"
                    End If
                Case DriveType.NoRootDirectory
                    Return "No root directory present"
                Case DriveType.Ram
                    Return "RAM"
                Case DriveType.Removable
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.CastDriveType")("Removable.Label")
                    Else
                        Return "Removable"
                    End If
                Case DriveType.Unknown
                    If Translate Then
                        Return LocalizationService.ForSection("Casters.CastDriveType")("Unknown.Label")
                    Else
                        Return "Unknown"
                    End If
            End Select
            Return Nothing
        End Function

    End Class

    Public Class Converters

        ''' <summary>
        ''' Using math procedures, converts the amount of bytes into a more readable format
        ''' </summary>
        ''' <param name="ByteSize">The amount of bytes, passed as a Long type for integers over (2 ^ 31) - 1</param>
        ''' <param name="UseCountryRepresentation">Uses a special representation of kB, MB, and GB based on the language. For example, France uses the "octet" representation, for ko, Mo, Go. This assumes the program is run on an OS in french</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function BytesToReadableSize(ByteSize As Long, Optional UseCountryRepresentation As Boolean = False) As String
            Select Case ByteSize
                Case 1024 To 1048575
                    ' Use kilobyte (kB) format
                    If UseCountryRepresentation Then
                        Return Math.Round(ByteSize / 1024, 2) & " Ko"
                    Else
                        Return Math.Round(ByteSize / 1024, 2) & " kB"
                    End If
                Case 1048576 To 1073741823
                    ' Use megabyte (MB) format
                    If UseCountryRepresentation Then
                        Return Math.Round(ByteSize / 1024 ^ 2, 2) & " Mo"
                    Else
                        Return Math.Round(ByteSize / 1024 ^ 2, 2) & " MB"
                    End If
                Case 1073741824 To 1099511627775
                    ' Use gigabyte (GB) format
                    If UseCountryRepresentation Then
                        Return Math.Round(ByteSize / 1024 ^ 3, 2) & " Go"
                    Else
                        Return Math.Round(ByteSize / 1024 ^ 3, 2) & " GB"
                    End If
                Case 1099511627776 To 1125899906842623
                    ' Use terabyte (TB) format
                    If UseCountryRepresentation Then
                        Return Math.Round(ByteSize / 1024 ^ 4, 2) & " To"
                    Else
                        Return Math.Round(ByteSize / 1024 ^ 4, 2) & " TB"
                    End If
                Case Is >= 1125899906842624
                    ' In a hypothetical world where drives that are petabytes big become mainstream, use petabyte (PB) format
                    If UseCountryRepresentation Then
                        Return Math.Round(ByteSize / 1024 ^ 5, 2) & " Po"
                    Else
                        Return Math.Round(ByteSize / 1024 ^ 5, 2) & " PB"
                    End If
            End Select
            Return Nothing
        End Function

    End Class
End Namespace

