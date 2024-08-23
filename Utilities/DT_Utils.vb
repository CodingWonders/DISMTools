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
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Unknown"
                                    Case "ESN"
                                        Return "Desconocida"
                                    Case "FRA"
                                        Return "Inconnu"
                                    Case "PTB", "PTG"
                                        Return "Desconhecido"
                                    Case "ITA"
                                        Return "Sconosciuta"
                                End Select
                            Case 1
                                Return "Unknown"
                            Case 2
                                Return "Desconocida"
                            Case 3
                                Return "Inconnu"
                            Case 4
                                Return "Desconhecido"
                            Case 5
                                Return "Sconosciuta"
                        End Select
                    Else
                        Return "Unknown"
                    End If
                Case DismProcessorArchitecture.Neutral
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Return "Neutral"
                                Case "ESN"
                                    Return "Neutral"
                                Case "FRA"
                                    Return "Neutre"
                                Case "PTB", "PTG"
                                    Return "Neutro"
                                Case "ITA"
                                    Return "Neutro"
                            End Select
                        Case 1
                            Return "Neutral"
                        Case 2
                            Return "Neutral"
                        Case 3
                            Return "Neutre"
                        Case 4
                            Return "Neutro"
                        Case 5
                            Return "Neutro"
                    End Select
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
                                    Case "ENU", "ENG"
                                        Return "Unknown"
                                    Case "ESN"
                                        Return "Desconocido"
                                    Case "FRA"
                                        Return "Inconnu"
                                    Case "PTB", "PTG"
                                        Return "Desconhecido"
                                    Case "ITA"
                                        Return "Sconosciuto"
                                End Select
                            Case 1
                                Return "Unknown"
                            Case 2
                                Return "Desconocido"
                            Case 3
                                Return "Inconnu"
                            Case 4
                                Return "Desconhecido"
                            Case 5
                                Return "Sconosciuto"
                        End Select
                    Else
                        Return "Unknown"
                    End If
                Case DismDriverSignature.Unsigned
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Unsigned. Please check the validity and expiration date of the signing certificate"
                                    Case "ESN"
                                        Return "No firmado. Compruebe la validez y la fecha de expiración del certificado del controlador"
                                    Case "FRA"
                                        Return "Non signé. Veuillez vérifier la validité et la date d'expiration du certificat de signature."
                                    Case "PTB", "PTG"
                                        Return "Não assinado. Verifique a validade e a data de expiração do certificado de assinatura"
                                    Case "ITA"
                                        Return "Non firmato. Verificare la validità e la data di scadenza del certificato di firma"
                                End Select
                            Case 1
                                Return "Unsigned. Please check the validity and expiration date of the signing certificate"
                            Case 2
                                Return "No firmado. Compruebe la validez y la fecha de expiración del certificado del controlador"
                            Case 3
                                Return "Non signé. Veuillez vérifier la validité et la date d'expiration du certificat de signature."
                            Case 4
                                Return "Não assinado. Verifique a validade e a data de expiração do certificado de assinatura"
                            Case 5
                                Return "Non firmato. Verificare la validità e la data di scadenza del certificato di firma"
                        End Select
                    Else
                        Return "Unsigned. Please check the validity and expiration date of the signing certificate"
                    End If
                Case DismDriverSignature.Signed
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Signed"
                                    Case "ESN"
                                        Return "Firmado"
                                    Case "FRA"
                                        Return "Signé"
                                    Case "PTB", "PTG"
                                        Return "Assinado"
                                    Case "ITA"
                                        Return "Firmato"
                                End Select
                            Case 1
                                Return "Signed"
                            Case 2
                                Return "Firmado"
                            Case 3
                                Return "Signé"
                            Case 4
                                Return "Assinado"
                            Case 5
                                Return "Firmato"
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
                                    Case "ENU", "ENG"
                                        Return "Not present"
                                    Case "ESN"
                                        Return "No presente"
                                    Case "FRA"
                                        Return "Absent"
                                    Case "PTB", "PTG"
                                        Return "Não presente"
                                    Case "ITA"
                                        Return "Non presente"
                                End Select
                            Case 1
                                Return "Not present"
                            Case 2
                                Return "No presente"
                            Case 3
                                Return "Absent"
                            Case 4
                                Return "Não presente"
                            Case 5
                                Return "Non presente"
                        End Select
                    Else
                        Return "Not present"
                    End If
                Case DismPackageFeatureState.UninstallPending
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Uninstall Pending"
                                    Case "ESN"
                                        Return "Desinstalación pendiente"
                                    Case "FRA"
                                        Return "Désinstallation en cours"
                                    Case "PTB", "PTG"
                                        Return "Desinstalação pendente"
                                    Case "ITA"
                                        Return "Disinstallazione in corso"
                                End Select
                            Case 1
                                Return "Uninstall Pending"
                            Case 2
                                Return "Desinstalación pendiente"
                            Case 3
                                Return "Désinstallation en cours"
                            Case 4
                                Return "Desinstalação pendente"
                            Case 5
                                Return "Disinstallazione in corso"
                        End Select
                    Else
                        Return "Uninstall Pending"
                    End If
                Case DismPackageFeatureState.Staged
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Uninstalled"
                                    Case "ESN"
                                        Return "Desinstalado"
                                    Case "FRA"
                                        Return "Désinstallé"
                                    Case "PTB", "PTG"
                                        Return "Desinstalado"
                                    Case "ITA"
                                        Return "Disinstallato"
                                End Select
                            Case 1
                                Return "Uninstalled"
                            Case 2
                                Return "Desinstalado"
                            Case 3
                                Return "Désinstallé"
                            Case 4
                                Return "Desinstalado"
                            Case 5
                                Return "Disinstallato"
                        End Select
                    Else
                        Return "Uninstalled"
                    End If
                Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Removed"
                                    Case "ESN"
                                        Return "Eliminado"
                                    Case "FRA"
                                        Return "Supprimé"
                                    Case "PTB", "PTG"
                                        Return "Removido"
                                    Case "ITA"
                                        Return "Rimosso"
                                End Select
                            Case 1
                                Return "Removed"
                            Case 2
                                Return "Eliminado"
                            Case 3
                                Return "Supprimé"
                            Case 4
                                Return "Removido"
                            Case 5
                                Return "Rimosso"
                        End Select
                    Else
                        Return "Removed"
                    End If
                Case DismPackageFeatureState.Installed
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Installed"
                                    Case "ESN"
                                        Return "Instalado"
                                    Case "FRA"
                                        Return "Installé"
                                    Case "PTB", "PTG"
                                        Return "Instalado"
                                    Case "ITA"
                                        Return "Installato"
                                End Select
                            Case 1
                                Return "Installed"
                            Case 2
                                Return "Instalado"
                            Case 3
                                Return "Installé"
                            Case 4
                                Return "Instalado"
                            Case 5
                                Return "Installato"
                        End Select
                    Else
                        Return "Installed"
                    End If
                Case DismPackageFeatureState.InstallPending
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Install Pending"
                                    Case "ESN"
                                        Return "Instalación pendiente"
                                    Case "FRA"
                                        Return "Installation en cours"
                                    Case "PTB", "PTG"
                                        Return "Instalação pendente"
                                    Case "ITA"
                                        Return "In attesa di installazione"
                                End Select
                            Case 1
                                Return "Install Pending"
                            Case 2
                                Return "Instalación pendiente"
                            Case 3
                                Return "Installation en cours"
                            Case 4
                                Return "Instalação pendente"
                            Case 5
                                Return "In attesa di installazione"
                        End Select
                    Else
                        Return "Install Pending"
                    End If
                Case DismPackageFeatureState.Superseded
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Superseded"
                                    Case "ESN"
                                        Return "Sustituido"
                                    Case "FRA"
                                        Return "Remplacé"
                                    Case "PTB", "PTG"
                                        Return "Substituído"
                                    Case "ITA"
                                        Return "Sostituito"
                                End Select
                            Case 1
                                Return "Superseded"
                            Case 2
                                Return "Sustituido"
                            Case 3
                                Return "Remplacé"
                            Case 4
                                Return "Substituído"
                            Case 5
                                Return "Sostituito"
                        End Select
                    Else
                        Return "Superseded"
                    End If
                Case DismPackageFeatureState.PartiallyInstalled
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Partially Installed"
                                    Case "ESN"
                                        Return "Instalado parcialmente"
                                    Case "FRA"
                                        Return "Partiellement installé"
                                    Case "PTB", "PTG"
                                        Return "Parcialmente instalado"
                                    Case "ITA"
                                        Return "Parzialmente installato"
                                End Select
                            Case 1
                                Return "Partially Installed"
                            Case 2
                                Return "Instalado parcialmente"
                            Case 3
                                Return "Partiellement installé"
                            Case 4
                                Return "Parcialmente instalado"
                            Case 5
                                Return "Parzialmente installato"
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
                                    Case "ENU", "ENG"
                                        Return "Not present"
                                    Case "ESN"
                                        Return "No presente"
                                    Case "FRA"
                                        Return "Absente"
                                    Case "PTB", "PTG"
                                        Return "Não presente"
                                    Case "ITA"
                                        Return "Non presente"
                                End Select
                            Case 1
                                Return "Not present"
                            Case 2
                                Return "No presente"
                            Case 3
                                Return "Absente"
                            Case 4
                                Return "Não presente"
                            Case 5
                                Return "Non presente"
                        End Select
                    Else
                        Return "Not present"
                    End If
                Case DismPackageFeatureState.UninstallPending
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Disable Pending"
                                    Case "ESN"
                                        Return "Deshabilitación pendiente"
                                    Case "FRA"
                                        Return "Invalidité en cours"
                                    Case "PTB", "PTG"
                                        Return "Desativação pendente"
                                    Case "ITA"
                                        Return "In attesa di invalidità"
                                End Select
                            Case 1
                                Return "Disable Pending"
                            Case 2
                                Return "Deshabilitación pendiente"
                            Case 3
                                Return "Invalidité en cours"
                            Case 4
                                Return "Desativação pendente"
                            Case 5
                                Return "In attesa di invalidità"
                        End Select
                    Else
                        Return "Disable Pending"
                    End If
                Case DismPackageFeatureState.Staged
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Disabled"
                                    Case "ESN"
                                        Return "Deshabilitado"
                                    Case "FRA"
                                        Return "Désactivée"
                                    Case "PTB", "PTG"
                                        Return "Desativado"
                                    Case "ITA"
                                        Return "Disabili"
                                End Select
                            Case 1
                                Return "Disabled"
                            Case 2
                                Return "Deshabilitado"
                            Case 3
                                Return "Désactivée"
                            Case 4
                                Return "Desativado"
                            Case 5
                                Return "Disabili"
                        End Select
                    Else
                        Return "Disabled"
                    End If
                Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Removed"
                                    Case "ESN"
                                        Return "Eliminado"
                                    Case "FRA"
                                        Return "Supprimée"
                                    Case "PTB", "PTG"
                                        Return "Removido"
                                    Case "ITA"
                                        Return "Rimosso"
                                End Select
                            Case 1
                                Return "Removed"
                            Case 2
                                Return "Eliminado"
                            Case 3
                                Return "Supprimée"
                            Case 4
                                Return "Removido"
                            Case 5
                                Return "Rimosso"
                        End Select
                    Else
                        Return "Removed"
                    End If
                Case DismPackageFeatureState.Installed
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Enabled"
                                    Case "ESN"
                                        Return "Habilitado"
                                    Case "FRA"
                                        Return "Activée"
                                    Case "PTB", "PTG"
                                        Return "Ativado"
                                    Case "ITA"
                                        Return "Abilitato"
                                End Select
                            Case 1
                                Return "Enabled"
                            Case 2
                                Return "Habilitado"
                            Case 3
                                Return "Activée"
                            Case 4
                                Return "Ativado"
                            Case 5
                                Return "Abilitato"
                        End Select
                    Else
                        Return "Enabled"
                    End If
                Case DismPackageFeatureState.InstallPending
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Enable Pending"
                                    Case "ESN"
                                        Return "Habilitación pendiente"
                                    Case "FRA"
                                        Return "Activation en cours"
                                    Case "PTB", "PTG"
                                        Return "Ativação pendente"
                                    Case "ITA"
                                        Return "In attesa di abilitazione"
                                End Select
                            Case 1
                                Return "Enable Pending"
                            Case 2
                                Return "Habilitación pendiente"
                            Case 3
                                Return "Activation en cours"
                            Case 4
                                Return "Ativação pendente"
                            Case 5
                                Return "In attesa di abilitazione"
                        End Select
                    Else
                        Return "Enable Pending"
                    End If
                Case DismPackageFeatureState.Superseded
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Superseded"
                                    Case "ESN"
                                        Return "Sustituido"
                                    Case "FRA"
                                        Return "Remplacée"
                                    Case "PTB", "PTG"
                                        Return "Substituído"
                                    Case "ITA"
                                        Return "Sostituito"
                                End Select
                            Case 1
                                Return "Superseded"
                            Case 2
                                Return "Sustituido"
                            Case 3
                                Return "Remplacée"
                            Case 4
                                Return "Substituído"
                            Case 5
                                Return "Sostituito"
                        End Select
                    Else
                        Return "Superseded"
                    End If
                Case DismPackageFeatureState.PartiallyInstalled
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Partially Installed"
                                    Case "ESN"
                                        Return "Instalado parcialmente"
                                    Case "FRA"
                                        Return "Partiellement installée"
                                    Case "PTB", "PTG"
                                        Return "Parcialmente instalado"
                                    Case "ITA"
                                        Return "Parzialmente installato"
                                End Select
                            Case 1
                                Return "Partially Installed"
                            Case 2
                                Return "Instalado parcialmente"
                            Case 3
                                Return "Partiellement installée"
                            Case 4
                                Return "Parcialmente instalado"
                            Case 5
                                Return "Parzialmente installato"
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
                                    Case "ENU", "ENG"
                                        Return "A restart is not required"
                                    Case "ESN"
                                        Return "No se requiere un reinicio"
                                    Case "FRA"
                                        Return "Un redémarrage n'est pas nécessaire"
                                    Case "PTB", "PTG"
                                        Return "Não é necessário reiniciar"
                                    Case "ITA"
                                        Return "Non è necessario un riavvio"
                                End Select
                            Case 1
                                Return "A restart is not required"
                            Case 2
                                Return "No se requiere un reinicio"
                            Case 3
                                Return "Un redémarrage n'est pas nécessaire"
                            Case 4
                                Return "Não é necessário reiniciar"
                            Case 5
                                Return "Non è necessario un riavvio"
                        End Select
                    Else
                        Return "A restart is not required"
                    End If
                Case DismRestartType.Possible
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "A restart may be required"
                                    Case "ESN"
                                        Return "Puede requerirse un reinicio"
                                    Case "FRA"
                                        Return "Un redémarrage peut être nécessaire"
                                    Case "PTB", "PTG"
                                        Return "Poderá ser necessário um reinício"
                                    Case "ITA"
                                        Return "Potrebbe essere necessario un riavvio"
                                End Select
                            Case 1
                                Return "A restart may be required"
                            Case 2
                                Return "Puede requerirse un reinicio"
                            Case 3
                                Return "Un redémarrage peut être nécessaire"
                            Case 4
                                Return "Poderá ser necessário um reinício"
                            Case 5
                                Return "Potrebbe essere necessario un riavvio"
                        End Select
                    Else
                        Return "A restart may be required"
                    End If
                Case DismRestartType.Required
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "A restart is required"
                                    Case "ESN"
                                        Return "Se requiere un reinicio"
                                    Case "FRA"
                                        Return "Un redémarrage est nécessaire"
                                    Case "PTB", "PTG"
                                        Return "É necessário um reinício"
                                    Case "ITA"
                                        Return "È necessario un riavvio"
                                End Select
                            Case 1
                                Return "A restart is required"
                            Case 2
                                Return "Se requiere un reinicio"
                            Case 3
                                Return "Un redémarrage est nécessaire"
                            Case 4
                                Return "É necessário um reinício"
                            Case 5
                                Return "È necessario un riavvio"
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
                                    Case "ENU", "ENG"
                                        Return "Yes"
                                    Case "ESN"
                                        Return "Sí"
                                    Case "FRA"
                                        Return "Oui"
                                    Case "PTB", "PTG"
                                        Return "Sim"
                                    Case "ITA"
                                        Return "Sì"
                                End Select
                            Case 1
                                Return "Yes"
                            Case 2
                                Return "Sí"
                            Case 3
                                Return "Oui"
                            Case 4
                                Return "Sim"
                            Case 5
                                Return "Sì"
                        End Select
                    Else
                        Return "Yes"
                    End If
                Case False
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "No"
                                    Case "ESN"
                                        Return "No"
                                    Case "FRA"
                                        Return "Non"
                                    Case "PTB", "PTG"
                                        Return "Não"
                                    Case "ITA"
                                        Return "No"
                                End Select
                            Case 1
                                Return "No"
                            Case 2
                                Return "No"
                            Case 3
                                Return "Non"
                            Case 4
                                Return "Não"
                            Case 5
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
                                    Case "ENU", "ENG"
                                        Return "Critical update"
                                    Case "ESN"
                                        Return "Actualización crítica"
                                    Case "FRA"
                                        Return "Mise à jour critique"
                                    Case "PTB", "PTG"
                                        Return "Atualização crítica"
                                    Case "ITA"
                                        Return "Aggiornamento critico"
                                End Select
                            Case 1
                                Return "Critical update"
                            Case 2
                                Return "Actualización crítica"
                            Case 3
                                Return "Mise à jour critique"
                            Case 4
                                Return "Atualização crítica"
                            Case 5
                                Return "Aggiornamento critico"
                        End Select
                    Else
                        Return "Critical update"
                    End If
                Case DismReleaseType.Driver
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Driver"
                                    Case "ESN"
                                        Return "Controlador"
                                    Case "FRA"
                                        Return "Pilote"
                                    Case "PTB", "PTG"
                                        Return "Controlador"
                                    Case "ITA"
                                        Return "Driver"
                                End Select
                            Case 1
                                Return "Driver"
                            Case 2
                                Return "Controlador"
                            Case 3
                                Return "Pilote"
                            Case 4
                                Return "Controlador"
                            Case 5
                                Return "Driver"
                        End Select
                    Else
                        Return "Driver"
                    End If
                Case DismReleaseType.FeaturePack
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Feature Pack"
                                    Case "ESN"
                                        Return "Paquete de características"
                                    Case "FRA"
                                        Return "Pack de caractéristiques"
                                    Case "PTB", "PTG"
                                        Return "Pacote de características"
                                    Case "ITA"
                                        Return "Pacchetto di caratteristiche"
                                End Select
                            Case 1
                                Return "Feature Pack"
                            Case 2
                                Return "Paquete de características"
                            Case 3
                                Return "Pack de caractéristiques"
                            Case 4
                                Return "Pacote de características"
                            Case 5
                                Return "Pacchetto di caratteristiche"
                        End Select
                    Else
                        Return "Feature Pack"
                    End If
                Case DismReleaseType.Foundation
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Foundation package"
                                    Case "ESN"
                                        Return "Paquete de fundación"
                                    Case "FRA"
                                        Return "Paquet de base"
                                    Case "PTB", "PTG"
                                        Return "Pacote de fundação"
                                    Case "ITA"
                                        Return "Pacchetto di fondazione"
                                End Select
                            Case 1
                                Return "Foundation package"
                            Case 2
                                Return "Paquete de fundación"
                            Case 3
                                Return "Paquet de base"
                            Case 4
                                Return "Pacote de fundação"
                            Case 5
                                Return "Pacchetto di fondazione"
                        End Select
                    Else
                        Return "Foundation package"
                    End If
                Case DismReleaseType.Hotfix
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Hotfix"
                                    Case "ESN"
                                        Return "Corrección de fallos"
                                    Case "FRA"
                                        Return "Correctif"
                                    Case "PTB", "PTG"
                                        Return "Correção"
                                    Case "ITA"
                                        Return "Correzione di bug"
                                End Select
                            Case 1
                                Return "Hotfix"
                            Case 2
                                Return "Corrección de fallos"
                            Case 3
                                Return "Correctif"
                            Case 4
                                Return "Correção"
                            Case 5
                                Return "Correzione di bug"
                        End Select
                    Else
                        Return "Hotfix"
                    End If
                Case DismReleaseType.LanguagePack
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Language pack"
                                    Case "ESN"
                                        Return "Paquete de idiomas"
                                    Case "FRA"
                                        Return "Pack linguistique"
                                    Case "PTB", "PTG"
                                        Return "Pacote de idiomas"
                                    Case "ITA"
                                        Return "Pacchetto lingua"
                                End Select
                            Case 1
                                Return "Language pack"
                            Case 2
                                Return "Paquete de idiomas"
                            Case 3
                                Return "Pack linguistique"
                            Case 4
                                Return "Pacote de idiomas"
                            Case 5
                                Return "Pacchetto lingua"
                        End Select
                    Else
                        Return "Language pack"
                    End If
                Case DismReleaseType.LocalPack
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Local pack"
                                    Case "ESN"
                                        Return "Paquete local"
                                    Case "FRA"
                                        Return "Paquet local"
                                    Case "PTB", "PTG"
                                        Return "Pacote local"
                                    Case "ITA"
                                        Return "Pacchetto locale"
                                End Select
                            Case 1
                                Return "Local pack"
                            Case 2
                                Return "Paquete local"
                            Case 3
                                Return "Paquet local"
                            Case 4
                                Return "Pacote local"
                            Case 5
                                Return "Pacchetto locale"
                        End Select
                    Else
                        Return "Local pack"
                    End If
                Case DismReleaseType.OnDemandPack
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "On Demand pack"
                                    Case "ESN"
                                        Return "Paquete de funcionalidad"
                                    Case "FRA"
                                        Return "Paquet de capacités"
                                    Case "PTB", "PTG"
                                        Return "Pacote de capacidades"
                                    Case "ITA"
                                        Return "Pacchetto di capacità"
                                End Select
                            Case 1
                                Return "On Demand pack"
                            Case 2
                                Return "Paquete de funcionalidad"
                            Case 3
                                Return "Paquet de capacités"
                            Case 4
                                Return "Pacote de capacidades"
                            Case 5
                                Return "Pacchetto di capacità"
                        End Select
                    Else
                        Return "On Demand pack"
                    End If
                Case DismReleaseType.Other
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Other"
                                    Case "ESN"
                                        Return "Otros"
                                    Case "FRA"
                                        Return "Autres"
                                    Case "PTB", "PTG"
                                        Return "Outros"
                                    Case "ITA"
                                        Return "Altro"
                                End Select
                            Case 1
                                Return "Other"
                            Case 2
                                Return "Otros"
                            Case 3
                                Return "Autres"
                            Case 4
                                Return "Outros"
                            Case 5
                                Return "Altro"
                        End Select
                    Else
                        Return "Other"
                    End If
                Case DismReleaseType.Product
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Product"
                                    Case "ESN"
                                        Return "Producto"
                                    Case "FRA"
                                        Return "Produit"
                                    Case "PTB", "PTG"
                                        Return "Produto"
                                    Case "ITA"
                                        Return "Prodotto"
                                End Select
                            Case 1
                                Return "Product"
                            Case 2
                                Return "Producto"
                            Case 3
                                Return "Produit"
                            Case 4
                                Return "Produto"
                            Case 5
                                Return "Prodotto"
                        End Select
                    Else
                        Return "Product"
                    End If
                Case DismReleaseType.SecurityUpdate
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Security update"
                                    Case "ESN"
                                        Return "Actualización de seguridad"
                                    Case "FRA"
                                        Return "Mise à jour de la sécurité"
                                    Case "PTB", "PTG"
                                        Return "Atualização de segurança"
                                    Case "ITA"
                                        Return "Aggiornamento della sicurezza"
                                End Select
                            Case 1
                                Return "Security update"
                            Case 2
                                Return "Actualización de seguridad"
                            Case 3
                                Return "Mise à jour de la sécurité"
                            Case 4
                                Return "Atualização de segurança"
                            Case 5
                                Return "Aggiornamento della sicurezza"
                        End Select
                    Else
                        Return "Security update"
                    End If
                Case DismReleaseType.ServicePack
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Service Pack"
                                    Case "ESN"
                                        Return "Service Pack"
                                    Case "FRA"
                                        Return "Service Pack"
                                    Case "PTB", "PTG"
                                        Return "Service Pack"
                                    Case "ITA"
                                        Return "Service Pack"
                                End Select
                            Case 1
                                Return "Service Pack"
                            Case 2
                                Return "Service Pack"
                            Case 3
                                Return "Service Pack"
                            Case 4
                                Return "Service Pack"
                            Case 5
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
                                    Case "ENU", "ENG"
                                        Return "Software update"
                                    Case "ESN"
                                        Return "Actualización de software"
                                    Case "FRA"
                                        Return "Mise à jour du logiciel"
                                    Case "PTB", "PTG"
                                        Return "Atualização do software"
                                    Case "ITA"
                                        Return "Aggiornamento software"
                                End Select
                            Case 1
                                Return "Software update"
                            Case 2
                                Return "Actualización de software"
                            Case 3
                                Return "Mise à jour du logiciel"
                            Case 4
                                Return "Atualização do software"
                            Case 5
                                Return "Aggiornamento software"
                        End Select
                    Else
                        Return "Software update"
                    End If
                Case DismReleaseType.Update
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Update"
                                    Case "ESN"
                                        Return "Actualización"
                                    Case "FRA"
                                        Return "Mise à jour"
                                    Case "PTB", "PTG"
                                        Return "Atualização"
                                    Case "ITA"
                                        Return "Aggiornamento"
                                End Select
                            Case 1
                                Return "Update"
                            Case 2
                                Return "Actualización"
                            Case 3
                                Return "Mise à jour"
                            Case 4
                                Return "Atualização"
                            Case 5
                                Return "Aggiornamento"
                        End Select
                    Else
                        Return "Update"
                    End If
                Case DismReleaseType.UpdateRollup
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Update rollup"
                                    Case "ESN"
                                        Return "Actualización acumulativa"
                                    Case "FRA"
                                        Return "Mise à jour cumulative"
                                    Case "PTB", "PTG"
                                        Return "Atualização cumulativa"
                                    Case "ITA"
                                        Return "Aggiornamento cumulativo"
                                End Select
                            Case 1
                                Return "Update rollup"
                            Case 2
                                Return "Actualización acumulativa"
                            Case 3
                                Return "Mise à jour cumulative"
                            Case 4
                                Return "Atualização cumulativa"
                            Case 5
                                Return "Aggiornamento cumulativo"
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
                                    Case "ENU", "ENG"
                                        Return "A boot up to the target image is required to fully install this package"
                                    Case "ESN"
                                        Return "Se requiere un arranque a la imagen de destino para instalar este paquete por completo"
                                    Case "FRA"
                                        Return "Un démarrage sur l'image cible est nécessaire pour installer complètement ce paquet."
                                    Case "PTB", "PTG"
                                        Return "É necessário um arranque para a imagem de destino para instalar completamente este pacote"
                                    Case "ITA"
                                        Return "Per installare completamente questo pacchetto è necessario un avvio dell'immagine di destinazione."
                                End Select
                            Case 1
                                Return "A boot up to the target image is required to fully install this package"
                            Case 2
                                Return "Se requiere un arranque a la imagen de destino para instalar este paquete por completo"
                            Case 3
                                Return "Un démarrage sur l'image cible est nécessaire pour installer complètement ce paquet."
                            Case 4
                                Return "É necessário um arranque para a imagem de destino para instalar completamente este pacote"
                            Case 5
                                Return "Per installare completamente questo pacchetto è necessario un avvio dell'immagine di destinazione."
                        End Select
                    Else
                        Return "A boot up to the target image is required to fully install this package"
                    End If
                Case DismFullyOfflineInstallableType.FullyOfflineInstallableUndetermined
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "A boot up to the target image may be required to fully install this package"
                                    Case "ESN"
                                        Return "Se podría requerir un arranque a la imagen de destino para instalar este paquete por completo"
                                    Case "FRA"
                                        Return "Un démarrage sur l'image cible peut être nécessaire pour installer complètement ce paquet."
                                    Case "PTB", "PTG"
                                        Return "Poderá ser necessário um arranque para a imagem de destino para instalar completamente este pacote"
                                    Case "ITA"
                                        Return "Per installare completamente questo pacchetto potrebbe essere necessario un avvio dell'immagine di destinazione."
                                End Select
                            Case 1
                                Return "A boot up to the target image may be required to fully install this package"
                            Case 2
                                Return "Se podría requerir un arranque a la imagen de destino para instalar este paquete por completo"
                            Case 3
                                Return "Un démarrage sur l'image cible peut être nécessaire pour installer complètement ce paquet."
                            Case 4
                                Return "Poderá ser necessário um arranque para a imagem de destino para instalar completamente este pacote"
                            Case 5
                                Return "Per installare completamente questo pacchetto potrebbe essere necessario un avvio dell'immagine di destinazione."
                        End Select
                    Else
                        Return "A boot up to the target image may be required to fully install this package"
                    End If
                Case DismFullyOfflineInstallableType.FullyOfflineInstallable
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "A boot up to the target image is not required to fully install this package"
                                    Case "ESN"
                                        Return "No se requiere un arranque a la imagen de destino para instalar este paquete por completo"
                                    Case "FRA"
                                        Return "Il n'est pas nécessaire de démarrer sur l'image cible pour installer complètement ce paquet."
                                    Case "PTB", "PTG"
                                        Return "Não é necessário arrancar com a imagem de destino para instalar completamente este pacote"
                                    Case "ITA"
                                        Return "Non è necessario un avvio dell'immagine di destinazione per installare completamente questo pacchetto."
                                End Select
                            Case 1
                                Return "A boot up to the target image is not required to fully install this package"
                            Case 2
                                Return "No se requiere un arranque a la imagen de destino para instalar este paquete por completo"
                            Case 3
                                Return "Il n'est pas nécessaire de démarrer sur l'image cible pour installer complètement ce paquet."
                            Case 4
                                Return "Não é necessário arrancar com a imagem de destino para instalar completamente este pacote"
                            Case 5
                                Return "Non è necessario un avvio dell'immagine di destinazione per installare completamente questo pacchetto."
                        End Select
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
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Fixed"
                                    Case "ESN"
                                        Return "Fijo"
                                    Case "FRA"
                                        Return "Fixe"
                                    Case "PTB", "PTG"
                                        Return "Fixo"
                                    Case "ITA"
                                        Return "Fisso"
                                End Select
                            Case 1
                                Return "Fixed"
                            Case 2
                                Return "Fijo"
                            Case 3
                                Return "Fixe"
                            Case 4
                                Return "Fixo"
                            Case 5
                                Return "Fisso"
                        End Select
                    Else
                        Return "Fixed"
                    End If
                Case DriveType.Network
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Network"
                                    Case "ESN"
                                        Return "Red"
                                    Case "FRA"
                                        Return "Réseau"
                                    Case "PTB", "PTG"
                                        Return "Rede"
                                    Case "ITA"
                                        Return "Rete"
                                End Select
                            Case 1
                                Return "Network"
                            Case 2
                                Return "Red"
                            Case 3
                                Return "Réseau"
                            Case 4
                                Return "Rede"
                            Case 5
                                Return "Rete"
                        End Select
                    Else
                        Return "Network"
                    End If
                Case DriveType.NoRootDirectory
                    Return "No root directory present"
                Case DriveType.Ram
                    Return "RAM"
                Case DriveType.Removable
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Removable"
                                    Case "ESN"
                                        Return "Removible"
                                    Case "FRA"
                                        Return "Amovible"
                                    Case "PTB", "PTG"
                                        Return "Removível"
                                    Case "ITA"
                                        Return "Rimovibile"
                                End Select
                            Case 1
                                Return "Removable"
                            Case 2
                                Return "Removible"
                            Case 3
                                Return "Amovible"
                            Case 4
                                Return "Removível"
                            Case 5
                                Return "Rimovibile"
                        End Select
                    Else
                        Return "Removable"
                    End If
                Case DriveType.Unknown
                    If Translate Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Return "Unknown"
                                    Case "ESN"
                                        Return "Desconocido"
                                    Case "FRA"
                                        Return "Inconnu"
                                    Case "PTB", "PTG"
                                        Return "Desconhecido"
                                    Case "ITA"
                                        Return "Sconosciuto"
                                End Select
                            Case 1
                                Return "Unknown"
                            Case 2
                                Return "Desconocido"
                            Case 3
                                Return "Inconnu"
                            Case 4
                                Return "Desconhecido"
                            Case 5
                                Return "Sconosciuto"
                        End Select
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

