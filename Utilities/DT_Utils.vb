Imports Microsoft.Dism

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
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Unknown"
                                    Case "ESN"
                                        Return "Desconocida"
                                End Select
                            Case 1
                                Return "Unknown"
                            Case 2
                                Return "Desconocida"
                        End Select
                    Else
                        Return "Unknown"
                    End If
                Case DismProcessorArchitecture.Neutral
                    Return "Neutral"
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

        Shared Function CastDismSignatureStatus(Signature As DismDriverSignature, Optional Translate As Boolean = False) As String
            Select Case Signature
                Case DismDriverSignature.Unknown
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Unknown"
                                    Case "ESN"
                                        Return "Desconocido"
                                End Select
                            Case 1
                                Return "Unknown"
                            Case 2
                                Return "Desconocido"
                        End Select
                    Else
                        Return "Unknown"
                    End If
                Case DismDriverSignature.Unsigned
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Unsigned. Please check the validity and expiration date of the signing certificate"
                                    Case "ESN"
                                        Return "No firmado. Compruebe la validez y la fecha de expiración del certificado del controlador"
                                End Select
                            Case 1
                                Return "Unsigned. Please check the validity and expiration date of the signing certificate"
                            Case 2
                                Return "No firmado. Compruebe la validez y la fecha de expiración del certificado del controlador"
                        End Select
                    Else
                        Return "Unsigned. Please check the validity and expiration date of the signing certificate"
                    End If
                Case DismDriverSignature.Signed
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Signed"
                                    Case "ESN"
                                        Return "Firmado"
                                End Select
                            Case 1
                                Return "Signed"
                            Case 2
                                Return "Firmado"
                        End Select
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
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Not present"
                                    Case "ESN"
                                        Return "No presente"
                                End Select
                            Case 1
                                Return "Not present"
                            Case 2
                                Return "No presente"
                        End Select
                    Else
                        Return "Not present"
                    End If
                Case DismPackageFeatureState.UninstallPending
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Uninstall Pending"
                                    Case "ESN"
                                        Return "Desinstalación pendiente"
                                End Select
                            Case 1
                                Return "Uninstall Pending"
                            Case 2
                                Return "Desinstalación pendiente"
                        End Select
                    Else
                        Return "Uninstall Pending"
                    End If
                Case DismPackageFeatureState.Staged
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Uninstalled"
                                    Case "ESN"
                                        Return "Desinstalado"
                                End Select
                            Case 1
                                Return "Uninstalled"
                            Case 2
                                Return "Desinstalado"
                        End Select
                    Else
                        Return "Uninstalled"
                    End If
                Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Removed"
                                    Case "ESN"
                                        Return "Eliminado"
                                End Select
                            Case 1
                                Return "Removed"
                            Case 2
                                Return "Eliminado"
                        End Select
                    Else
                        Return "Removed"
                    End If
                Case DismPackageFeatureState.Installed
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Installed"
                                    Case "ESN"
                                        Return "Instalado"
                                End Select
                            Case 1
                                Return "Installed"
                            Case 2
                                Return "Instalado"
                        End Select
                    Else
                        Return "Installed"
                    End If
                Case DismPackageFeatureState.InstallPending
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Install Pending"
                                    Case "ESN"
                                        Return "Instalación pendiente"
                                End Select
                            Case 1
                                Return "Install Pending"
                            Case 2
                                Return "Instalación pendiente"
                        End Select
                    Else
                        Return "Install Pending"
                    End If
                Case DismPackageFeatureState.Superseded
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Superseded"
                                    Case "ESN"
                                        Return "Sustituido"
                                End Select
                            Case 1
                                Return "Superseded"
                            Case 2
                                Return "Sustituido"
                        End Select
                    Else
                        Return "Superseded"
                    End If
                Case DismPackageFeatureState.PartiallyInstalled
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Partially Installed"
                                    Case "ESN"
                                        Return "Instalado parcialmente"
                                End Select
                            Case 1
                                Return "Partially Installed"
                            Case 2
                                Return "Instalado parcialmente"
                        End Select
                    Else
                        Return "Partially Installed"
                    End If
            End Select
            Return Nothing
        End Function

        Shared Function CastDismFeatureState(State As DismPackageFeatureState, Optional Translate As Boolean = False) As String
            Select Case State
                Case DismPackageFeatureState.NotPresent
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Not present"
                                    Case "ESN"
                                        Return "No presente"
                                End Select
                            Case 1
                                Return "Not present"
                            Case 2
                                Return "No presente"
                        End Select
                    Else
                        Return "Not present"
                    End If
                Case DismPackageFeatureState.UninstallPending
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Disable Pending"
                                    Case "ESN"
                                        Return "Deshabilitación pendiente"
                                End Select
                            Case 1
                                Return "Disable Pending"
                            Case 2
                                Return "Deshabilitación pendiente"
                        End Select
                    Else
                        Return "Disable Pending"
                    End If
                Case DismPackageFeatureState.Staged
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Disabled"
                                    Case "ESN"
                                        Return "Deshabilitado"
                                End Select
                            Case 1
                                Return "Disabled"
                            Case 2
                                Return "Deshabilitado"
                        End Select
                    Else
                        Return "Disabled"
                    End If
                Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Removed"
                                    Case "ESN"
                                        Return "Eliminado"
                                End Select
                            Case 1
                                Return "Removed"
                            Case 2
                                Return "Eliminado"
                        End Select
                    Else
                        Return "Removed"
                    End If
                Case DismPackageFeatureState.Installed
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Enabled"
                                    Case "ESN"
                                        Return "Habilitado"
                                End Select
                            Case 1
                                Return "Enabled"
                            Case 2
                                Return "Habilitado"
                        End Select
                    Else
                        Return "Enabled"
                    End If
                Case DismPackageFeatureState.InstallPending
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Enable Pending"
                                    Case "ESN"
                                        Return "Habilitación pendiente"
                                End Select
                            Case 1
                                Return "Enable Pending"
                            Case 2
                                Return "Habilitación pendiente"
                        End Select
                    Else
                        Return "Enable Pending"
                    End If
                Case DismPackageFeatureState.Superseded
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Superseded"
                                    Case "ESN"
                                        Return "Sustituido"
                                End Select
                            Case 1
                                Return "Superseded"
                            Case 2
                                Return "Sustituido"
                        End Select
                    Else
                        Return "Superseded"
                    End If
                Case DismPackageFeatureState.PartiallyInstalled
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Partially Installed"
                                    Case "ESN"
                                        Return "Instalado parcialmente"
                                End Select
                            Case 1
                                Return "Partially Installed"
                            Case 2
                                Return "Instalado parcialmente"
                        End Select
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
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "A restart is not required"
                                    Case "ESN"
                                        Return "No se requiere un reinicio"
                                End Select
                            Case 1
                                Return "A restart is not required"
                            Case 2
                                Return "No se requiere un reinicio"
                        End Select
                    Else
                        Return "A restart is not required"
                    End If
                Case DismRestartType.Possible
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "A restart may be required"
                                    Case "ESN"
                                        Return "Puede requerirse un reinicio"
                                End Select
                            Case 1
                                Return "A restart may be required"
                            Case 2
                                Return "Puede requerirse un reinicio"
                        End Select
                    Else
                        Return "A restart may be required"
                    End If
                Case DismRestartType.Required
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "A restart is required"
                                    Case "ESN"
                                        Return "Se requiere un reinicio"
                                End Select
                            Case 1
                                Return "A restart is required"
                            Case 2
                                Return "Se requiere un reinicio"
                        End Select
                    Else
                        Return "A restart is required"
                    End If
            End Select
            Return Nothing
        End Function

        Shared Function CastDismApplicabilityStatus(AppState As Boolean, Optional Translate As Boolean = False) As String
            Select Case AppState
                Case True
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Yes"
                                    Case "ESN"
                                        Return "Sí"
                                End Select
                            Case 1
                                Return "Yes"
                            Case 2
                                Return "Sí"
                        End Select
                    Else
                        Return "Yes"
                    End If
                Case False
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "No"
                                    Case "ESN"
                                        Return "No"
                                End Select
                            Case 1
                                Return "No"
                            Case 2
                                Return "No"
                        End Select
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
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Critical update"
                                    Case "ESN"
                                        Return "Actualización crítica"
                                End Select
                            Case 1
                                Return "Critical update"
                            Case 2
                                Return "Actualización crítica"
                        End Select
                    Else
                        Return "Critical update"
                    End If
                Case DismReleaseType.Driver
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Driver"
                                    Case "ESN"
                                        Return "Controlador"
                                End Select
                            Case 1
                                Return "Driver"
                            Case 2
                                Return "Controlador"
                        End Select
                    Else
                        Return "Driver"
                    End If
                Case DismReleaseType.FeaturePack
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Feature Pack"
                                    Case "ESN"
                                        Return "Paquete de características"
                                End Select
                            Case 1
                                Return "Feature Pack"
                            Case 2
                                Return "Paquete de características"
                        End Select
                    Else
                        Return "Feature Pack"
                    End If
                Case DismReleaseType.Foundation
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Foundation package"
                                    Case "ESN"
                                        Return "Paquete de fundación"
                                End Select
                            Case 1
                                Return "Foundation package"
                            Case 2
                                Return "Paquete de fundación"
                        End Select
                    Else
                        Return "Foundation package"
                    End If
                Case DismReleaseType.Hotfix
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Hotfix"
                                    Case "ESN"
                                        Return "Corrección de fallos"
                                End Select
                            Case 1
                                Return "Hotfix"
                            Case 2
                                Return "Corrección de fallos"
                        End Select
                    Else
                        Return "Hotfix"
                    End If
                Case DismReleaseType.LanguagePack
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Language pack"
                                    Case "ESN"
                                        Return "Paquete de idiomas"
                                End Select
                            Case 1
                                Return "Language pack"
                            Case 2
                                Return "Paquete de idiomas"
                        End Select
                    Else
                        Return "Language pack"
                    End If
                Case DismReleaseType.LocalPack
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Local pack"
                                    Case "ESN"
                                        Return "Paquete local"
                                End Select
                            Case 1
                                Return "Local pack"
                            Case 2
                                Return "Paquete local"
                        End Select
                    Else
                        Return "Local pack"
                    End If
                Case DismReleaseType.OnDemandPack
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "On Demand pack"
                                    Case "ESN"
                                        Return "Paquete de funcionalidad"
                                End Select
                            Case 1
                                Return "On Demand pack"
                            Case 2
                                Return "Paquete de funcionalidad"
                        End Select
                    Else
                        Return "On Demand pack"
                    End If
                Case DismReleaseType.Other
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Other"
                                    Case "ESN"
                                        Return "Otros"
                                End Select
                            Case 1
                                Return "Other"
                            Case 2
                                Return "Otros"
                        End Select
                    Else
                        Return "Other"
                    End If
                Case DismReleaseType.Product
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Product"
                                    Case "ESN"
                                        Return "Producto"
                                End Select
                            Case 1
                                Return "Product"
                            Case 2
                                Return "Producto"
                        End Select
                    Else
                        Return "Product"
                    End If
                Case DismReleaseType.SecurityUpdate
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Security update"
                                    Case "ESN"
                                        Return "Actualización de seguridad"
                                End Select
                            Case 1
                                Return "Security update"
                            Case 2
                                Return "Actualización de seguridad"
                        End Select
                    Else
                        Return "Security update"
                    End If
                Case DismReleaseType.ServicePack
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Service Pack"
                                    Case "ESN"
                                        Return "Service Pack"
                                End Select
                            Case 1
                                Return "Service Pack"
                            Case 2
                                Return "Service Pack"
                        End Select
                    Else
                        Return "Service Pack"
                    End If
                Case DismReleaseType.SoftwareUpdate
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Software update"
                                    Case "ESN"
                                        Return "Actualización de software"
                                End Select
                            Case 1
                                Return "Software update"
                            Case 2
                                Return "Actualización de software"
                        End Select
                    Else
                        Return "Software update"
                    End If
                Case DismReleaseType.Update
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Update"
                                    Case "ESN"
                                        Return "Actualización"
                                End Select
                            Case 1
                                Return "Update"
                            Case 2
                                Return "Actualización"
                        End Select
                    Else
                        Return "Update"
                    End If
                Case DismReleaseType.UpdateRollup
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "Update rollup"
                                    Case "ESN"
                                        Return "Actualización acumulativa"
                                End Select
                            Case 1
                                Return "Update rollup"
                            Case 2
                                Return "Actualización acumulativa"
                        End Select
                    Else
                        Return "Update rollup"
                    End If
            End Select
            Return Nothing
        End Function

        Shared Function CastDismFullyOfflineInstallationType(foiType As DismFullyOfflineInstallableType, Optional Translate As Boolean = False) As String
            Select Case foiType
                Case DismFullyOfflineInstallableType.FullyOfflineNotInstallable
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "A boot up to the target image is required to fully install this package"
                                    Case "ESN"
                                        Return "Se requiere un arranque a la imagen de destino para instalar este paquete por completo"
                                End Select
                            Case 1
                                Return "A boot up to the target image is required to fully install this package"
                            Case 2
                                Return "Se requiere un arranque a la imagen de destino para instalar este paquete por completo"
                        End Select
                    Else
                        Return "A boot up to the target image is required to fully install this package"
                    End If
                Case DismFullyOfflineInstallableType.FullyOfflineInstallableUndetermined
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "A boot up to the target image may be required to fully install this package"
                                    Case "ESN"
                                        Return "Se podría requerir un arranque a la imagen de destino para instalar este paquete por completo"
                                End Select
                            Case 1
                                Return "A boot up to the target image may be required to fully install this package"
                            Case 2
                                Return "Se podría requerir un arranque a la imagen de destino para instalar este paquete por completo"
                        End Select
                    Else
                        Return "A boot up to the target image may be required to fully install this package"
                    End If
                Case DismFullyOfflineInstallableType.FullyOfflineInstallable
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Return "A boot up to the target image is not required to fully install this package"
                                    Case "ESN"
                                        Return "No se requiere un arranque a la imagen de destino para instalar este paquete por completo"
                                End Select
                            Case 1
                                Return "A boot up to the target image is not required to fully install this package"
                            Case 2
                                Return "No se requiere un arranque a la imagen de destino para instalar este paquete por completo"
                        End Select
                    Else
                        Return "A boot up to the target image is not required to fully install this package"
                    End If
            End Select
            Return Nothing
        End Function

    End Class
End Namespace

