Imports System.Windows.Forms
Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Public Class GetPkgInfoDlg

    Dim PackageInfoList As New List(Of DismPackageInfoEx)
    Public InstalledPkgInfo As DismPackageCollection

    Function CastDismApplicabilityStatus(Applicability As Boolean) As String
        Select Case Applicability
            Case True
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
            Case False
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
        End Select
        Return Nothing
    End Function

    Function CastDismReleaseType(RelType As DismReleaseType) As String
        Select Case RelType
            Case DismReleaseType.CriticalUpdate
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
            Case DismReleaseType.Driver
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
            Case DismReleaseType.FeaturePack
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
            Case DismReleaseType.Foundation
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
            Case DismReleaseType.Hotfix
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
            Case DismReleaseType.LanguagePack
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
            Case DismReleaseType.LocalPack
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
            Case DismReleaseType.OnDemandPack
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
            Case DismReleaseType.Other
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
            Case DismReleaseType.Product
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
            Case DismReleaseType.SecurityUpdate
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
            Case DismReleaseType.ServicePack
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
            Case DismReleaseType.SoftwareUpdate
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
            Case DismReleaseType.Update
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
            Case DismReleaseType.UpdateRollup
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
        End Select
        Return Nothing
    End Function

    Function CastDismRestartType(rType As DismRestartType) As String
        Select Case rType
            Case DismRestartType.No
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
            Case DismRestartType.Possible
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
            Case DismRestartType.Required
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
        End Select
        Return Nothing
    End Function

    Function CastDismPackageState(state As DismPackageFeatureState) As String
        Select Case state
            Case DismPackageFeatureState.NotPresent
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
            Case DismPackageFeatureState.UninstallPending
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
            Case DismPackageFeatureState.Staged
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
            Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
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
            Case DismPackageFeatureState.Installed
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
            Case DismPackageFeatureState.InstallPending
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
            Case DismPackageFeatureState.Superseded
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
            Case DismPackageFeatureState.PartiallyInstalled
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
        End Select
        Return Nothing
    End Function

    Function CastDismFullyOfflineInstallationType(foiType As DismFullyOfflineInstallableType) As String
        Select Case foiType
            Case DismFullyOfflineInstallableType.FullyOfflineNotInstallable
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
            Case DismFullyOfflineInstallableType.FullyOfflineInstallableUndetermined
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
            Case DismFullyOfflineInstallableType.FullyOfflineInstallable
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
        End Select
        Return Nothing
    End Function

    Private Sub GetPkgInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
            ListBox2.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListBox1.BackColor = Color.FromArgb(238, 238, 242)
            ListBox2.BackColor = Color.FromArgb(238, 238, 242)
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Get package information"
                        Label1.Text = Text
                        Label2.Text = "What do you want to get information about?"
                        Label3.Text = "Click here to get information about packages that you've installed or that came with the Windows image you're servicing"
                        Label4.Text = "Click here to get information about packages that you want to add to the Windows image you're servicing before proceeding with the package addition process"
                        Label5.Text = "Ready"
                        Label6.Text = "Add or select a package file to view its information here"
                        Label7.Text = "Package information"
                        Label8.Text = "Package name:"
                        Label10.Text = "Is package applicable?"
                        Label12.Text = "Copyright:"
                        Label14.Text = "Product version:"
                        Label16.Text = "Release type:"
                        Label18.Text = "Company:"
                        Label20.Text = "Creation time:"
                        Label22.Text = "Package name:"
                        Label24.Text = "Is package applicable?"
                        Label26.Text = "Copyright:"
                        Label28.Text = "Install time:"
                        Label30.Text = "Last update time:"
                        Label31.Text = "Company:"
                        Label33.Text = "Install package name:"
                        Label36.Text = "Package information"
                        Label37.Text = "Select an installed package to view its information here"
                        Label39.Text = "Display name:"
                        Label41.Text = "Creation time:"
                        Label43.Text = "Description:"
                        Label45.Text = "Product name:"
                        Label47.Text = "Install client:"
                        Label48.Text = "Is a restart required?"
                        Label50.Text = "Support information:"
                        Label52.Text = "State:"
                        Label54.Text = "Is a boot up required for full installation?"
                        Label58.Text = "Custom properties:"
                        Label60.Text = "Features:"
                        Label61.Text = "Capability identity:"
                        Label63.Text = "Description:"
                        Label65.Text = "Install client:"
                        Label67.Text = "Install package name:"
                        Label69.Text = "Install time:"
                        Label71.Text = "Last update time:"
                        Label73.Text = "Display name:"
                        Label75.Text = "Product name:"
                        Label77.Text = "Product version:"
                        Label79.Text = "Release type:"
                        Label81.Text = "Is a restart required?"
                        Label83.Text = "Support information:"
                        Label85.Text = "State:"
                        Label87.Text = "Is a boot up required for full installation?"
                        Label89.Text = "Capability identity:"
                        Label91.Text = "Custom properties:"
                        Label93.Text = "Features:"
                        LinkLabel1.Text = "<- Go back"
                        Button1.Text = "Add package..."
                        Button2.Text = "Remove selected"
                        Button3.Text = "Remove all"
                        InstalledPackageLink.Text = "I want to get information about installed packages in the image"
                        PackageFileLink.Text = "I want to get information about package files"
                        OpenFileDialog1.Title = "Locate package files"
                    Case "ESN"
                        Text = "Obtener información de paquetes"
                        Label1.Text = Text
                        Label2.Text = "¿Acerca de qué le gustaría obtener información?"
                        Label3.Text = "Haga clic aquí para obtener información de paquetes que ha instalado o que vengan con la imagen de Windows a la que está dando servicio"
                        Label4.Text = "Haga clic aquí para obtener información de paquetes que le gustaría añadir a la imagen de Windows a la que está dando servicio antes de proceder con el proceso de adición de paquetes"
                        Label5.Text = "Listo"
                        Label6.Text = "Añada o seleccione un archivo de paquete para ver su información aquí"
                        Label7.Text = "Información de paquete"
                        Label8.Text = "Nombre de paquete:"
                        Label10.Text = "¿El paquete es aplicable?"
                        Label12.Text = "Copyright:"
                        Label14.Text = "Versión de producto:"
                        Label16.Text = "Tipo de paquete:"
                        Label18.Text = "Compañía:"
                        Label20.Text = "Tiempo de creación:"
                        Label22.Text = "Nombre de paquete:"
                        Label24.Text = "¿El paquete es aplicable?"
                        Label26.Text = "Copyright:"
                        Label28.Text = "Tiempo de instalación:"
                        Label30.Text = "Último tiempo de actualización:"
                        Label31.Text = "Compañía:"
                        Label33.Text = "Nombre del paquete de instalación:"
                        Label36.Text = "Información de paquete"
                        Label37.Text = "Seleccione un paquete instalado para ver su información aquí"
                        Label39.Text = "Nombre a mostrar:"
                        Label41.Text = "Tiempo de creación:"
                        Label43.Text = "Descripción:"
                        Label45.Text = "Nombre de producto:"
                        Label47.Text = "Cliente de instalación:"
                        Label48.Text = "¿Se requiere un reinicio?"
                        Label50.Text = "Información de soporte:"
                        Label52.Text = "Estado:"
                        Label54.Text = "¿Se requiere un arranque para una instalación completa?"
                        Label58.Text = "Propiedades personalizadas:"
                        Label60.Text = "Características:"
                        Label61.Text = "Identidad de funcionalidad:"
                        Label63.Text = "Descripción:"
                        Label65.Text = "Cliente de instalación:"
                        Label67.Text = "Nombre del paquete de instalación:"
                        Label69.Text = "Tiempo de instalación:"
                        Label71.Text = "Último tiempo de actualización:"
                        Label73.Text = "Nombre a mostrar:"
                        Label75.Text = "Nombre de producto:"
                        Label77.Text = "Versión de producto:"
                        Label79.Text = "Tipo de paquete:"
                        Label81.Text = "¿Se requiere un reinicio?"
                        Label83.Text = "Información de soporte:"
                        Label85.Text = "Estado:"
                        Label87.Text = "¿Se requiere un arranque para una instalación completa?"
                        Label89.Text = "Identidad de funcionalidad:"
                        Label91.Text = "Propiedades personalizadas:"
                        Label93.Text = "Características:"
                        LinkLabel1.Text = "<- Atrás"
                        Button1.Text = "Añadir paquete..."
                        Button2.Text = "Eliminar selección"
                        Button3.Text = "Eliminar todo"
                        InstalledPackageLink.Text = "Deseo obtener información acerca de paquetes instalados en la imagen"
                        PackageFileLink.Text = "Deseo obtener información acerca de archivos de paquetes"
                        OpenFileDialog1.Title = "Ubique los archivos de paquetes"
                End Select
            Case 1
                Text = "Get package information"
                Label1.Text = Text
                Label2.Text = "What do you want to get information about?"
                Label3.Text = "Click here to get information about packages that you've installed or that came with the Windows image you're servicing"
                Label4.Text = "Click here to get information about packages that you want to add to the Windows image you're servicing before proceeding with the package addition process"
                Label5.Text = "Ready"
                Label6.Text = "Add or select a package file to view its information here"
                Label7.Text = "Package information"
                Label8.Text = "Package name:"
                Label10.Text = "Is package applicable?"
                Label12.Text = "Copyright:"
                Label14.Text = "Product version:"
                Label16.Text = "Release type:"
                Label18.Text = "Company:"
                Label20.Text = "Creation time:"
                Label22.Text = "Package name:"
                Label24.Text = "Is package applicable?"
                Label26.Text = "Copyright:"
                Label28.Text = "Install time:"
                Label30.Text = "Last update time:"
                Label31.Text = "Company:"
                Label33.Text = "Install package name:"
                Label36.Text = "Package information"
                Label37.Text = "Select an installed package to view its information here"
                Label39.Text = "Display name:"
                Label41.Text = "Creation time:"
                Label43.Text = "Description:"
                Label45.Text = "Product name:"
                Label47.Text = "Install client:"
                Label48.Text = "Is a restart required?"
                Label50.Text = "Support information:"
                Label52.Text = "State:"
                Label54.Text = "Is a boot up required for full installation?"
                Label58.Text = "Custom properties:"
                Label60.Text = "Features:"
                Label61.Text = "Capability identity:"
                Label63.Text = "Description:"
                Label65.Text = "Install client:"
                Label67.Text = "Install package name:"
                Label69.Text = "Install time:"
                Label71.Text = "Last update time:"
                Label73.Text = "Display name:"
                Label75.Text = "Product name:"
                Label77.Text = "Product version:"
                Label79.Text = "Release type:"
                Label81.Text = "Is a restart required?"
                Label83.Text = "Support information:"
                Label85.Text = "State:"
                Label87.Text = "Is a boot up required for full installation?"
                Label89.Text = "Capability identity:"
                Label91.Text = "Custom properties:"
                Label93.Text = "Features:"
                LinkLabel1.Text = "<- Go back"
                Button1.Text = "Add package..."
                Button2.Text = "Remove selected"
                Button3.Text = "Remove all"
                InstalledPackageLink.Text = "I want to get information about installed packages in the image"
                PackageFileLink.Text = "I want to get information about package files"
                OpenFileDialog1.Title = "Locate package files"
            Case 2
                Text = "Obtener información de paquetes"
                Label1.Text = Text
                Label2.Text = "¿Acerca de qué le gustaría obtener información?"
                Label3.Text = "Haga clic aquí para obtener información de paquetes que ha instalado o que vengan con la imagen de Windows a la que está dando servicio"
                Label4.Text = "Haga clic aquí para obtener información de paquetes que le gustaría añadir a la imagen de Windows a la que está dando servicio antes de proceder con el proceso de adición de paquetes"
                Label5.Text = "Listo"
                Label6.Text = "Añada o seleccione un archivo de paquete para ver su información aquí"
                Label7.Text = "Información de paquete"
                Label8.Text = "Nombre de paquete:"
                Label10.Text = "¿El paquete es aplicable?"
                Label12.Text = "Copyright:"
                Label14.Text = "Versión de producto:"
                Label16.Text = "Tipo de paquete:"
                Label18.Text = "Compañía:"
                Label20.Text = "Tiempo de creación:"
                Label22.Text = "Nombre de paquete:"
                Label24.Text = "¿El paquete es aplicable?"
                Label26.Text = "Copyright:"
                Label28.Text = "Tiempo de instalación:"
                Label30.Text = "Último tiempo de actualización:"
                Label31.Text = "Compañía:"
                Label33.Text = "Nombre del paquete de instalación:"
                Label36.Text = "Información de paquete"
                Label37.Text = "Seleccione un paquete instalado para ver su información aquí"
                Label39.Text = "Nombre a mostrar:"
                Label41.Text = "Tiempo de creación:"
                Label43.Text = "Descripción:"
                Label45.Text = "Nombre de producto:"
                Label47.Text = "Cliente de instalación:"
                Label48.Text = "¿Se requiere un reinicio?"
                Label50.Text = "Información de soporte:"
                Label52.Text = "Estado:"
                Label54.Text = "¿Se requiere un arranque para una instalación completa?"
                Label58.Text = "Propiedades personalizadas:"
                Label60.Text = "Características:"
                Label61.Text = "Identidad de funcionalidad:"
                Label63.Text = "Descripción:"
                Label65.Text = "Cliente de instalación:"
                Label67.Text = "Nombre del paquete de instalación:"
                Label69.Text = "Tiempo de instalación:"
                Label71.Text = "Último tiempo de actualización:"
                Label73.Text = "Nombre a mostrar:"
                Label75.Text = "Nombre de producto:"
                Label77.Text = "Versión de producto:"
                Label79.Text = "Tipo de paquete:"
                Label81.Text = "¿Se requiere un reinicio?"
                Label83.Text = "Información de soporte:"
                Label85.Text = "Estado:"
                Label87.Text = "¿Se requiere un arranque para una instalación completa?"
                Label89.Text = "Identidad de funcionalidad:"
                Label91.Text = "Propiedades personalizadas:"
                Label93.Text = "Características:"
                LinkLabel1.Text = "<- Atrás"
                Button1.Text = "Añadir paquete..."
                Button2.Text = "Eliminar selección"
                Button3.Text = "Eliminar todo"
                InstalledPackageLink.Text = "Deseo obtener información acerca de paquetes instalados en la imagen"
                PackageFileLink.Text = "Deseo obtener información acerca de archivos de paquetes"
                OpenFileDialog1.Title = "Ubique los archivos de paquetes"
        End Select
        ListBox1.ForeColor = ForeColor
        ListBox2.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))

        ' Populate installed package listing
        ListBox2.Items.Clear()
        For Each InstalledPackage As DismPackage In InstalledPkgInfo
            ListBox2.Items.Add(InstalledPackage.PackageName)
        Next

        NoPkgPanel.Visible = True
        PackageFileInfoPanel.Visible = False
        Panel4.Visible = False
        Panel7.Visible = True
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MenuPanel.Visible = True
        PackageInfoPanel.Visible = False
    End Sub

    Private Sub InstalledPackageLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles InstalledPackageLink.LinkClicked
        MenuPanel.Visible = False
        PackageInfoPanel.Visible = True
        InfoFromInstalledPkgsPanel.Visible = True
        InfoFromPackageFilesPanel.Visible = False
    End Sub

    Private Sub PackageFileLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles PackageFileLink.LinkClicked
        MenuPanel.Visible = False
        PackageInfoPanel.Visible = True
        InfoFromInstalledPkgsPanel.Visible = False
        InfoFromPackageFilesPanel.Visible = True
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        Try
            If ListBox2.SelectedItems.Count = 1 Then
                ' Background processes need to have completed before showing information
                If MainForm.ImgBW.IsBusy Then
                    Dim msg As String = ""
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    msg = "Background processes need to have completed before showing package information. We'll wait until they have completed"
                                Case "ESN"
                                    msg = "Los procesos en segundo plano deben haber completado antes de obtener información del paquete. Esperaremos hasta que hayan completado"
                            End Select
                        Case 1
                            msg = "Background processes need to have completed before showing package information. We'll wait until they have completed"
                        Case 2
                            msg = "Los procesos en segundo plano deben haber completado antes de obtener información del paquete. Esperaremos hasta que hayan completado"
                    End Select
                    MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label5.Text = "Waiting for background processes to finish..."
                                Case "ESN"
                                    Label5.Text = "Esperando a que terminen los procesos en segundo plano..."
                            End Select
                        Case 1
                            Label5.Text = "Waiting for background processes to finish..."
                        Case 2
                            Label5.Text = "Esperando a que terminen los procesos en segundo plano..."
                    End Select
                    While MainForm.ImgBW.IsBusy
                        Application.DoEvents()
                        Thread.Sleep(500)
                    End While
                End If
                If MainForm.MountedImageDetectorBW.IsBusy Then
                    MainForm.MountedImageDetectorBW.CancelAsync()
                    While MainForm.MountedImageDetectorBW.IsBusy
                        Application.DoEvents()
                        Thread.Sleep(500)
                    End While
                End If
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Label5.Text = "Preparing to get package information..."
                            Case "ESN"
                                Label5.Text = "Preparándonos para obtener información del paquete..."
                        End Select
                    Case 1
                        Label5.Text = "Preparing to get package information..."
                    Case 2
                        Label5.Text = "Preparándonos para obtener información del paquete..."
                End Select
                Application.DoEvents()
                Try
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Label5.Text = "Getting information from " & Quote & ListBox2.SelectedItem & Quote & "..."
                                    Case "ESN"
                                        Label5.Text = "Obteniendo información de " & Quote & ListBox2.SelectedItem & Quote & "..."
                                End Select
                            Case 1
                                Label5.Text = "Getting information from " & Quote & ListBox2.SelectedItem & Quote & "..."
                            Case 2
                                Label5.Text = "Obteniendo información de " & Quote & ListBox2.SelectedItem & Quote & "..."
                        End Select
                        ' Use the extended version, as DISM gets extended package information
                        Dim PkgInfo As DismPackageInfoEx = DismApi.GetPackageInfoExByName(imgSession, ListBox2.SelectedItem)
                        Label23.Text = PkgInfo.PackageName
                        Label25.Text = CastDismApplicabilityStatus(PkgInfo.Applicable)
                        Label35.Text = PkgInfo.Copyright
                        Label32.Text = PkgInfo.Company
                        Label40.Text = PkgInfo.CreationTime
                        Label42.Text = PkgInfo.Description
                        Label46.Text = PkgInfo.InstallClient
                        Label34.Text = PkgInfo.InstallPackageName
                        Label27.Text = PkgInfo.InstallTime
                        Label29.Text = PkgInfo.LastUpdateTime
                        Label38.Text = PkgInfo.DisplayName
                        Label44.Text = PkgInfo.ProductName
                        Label15.Text = PkgInfo.ProductVersion.ToString()
                        Label21.Text = CastDismReleaseType(PkgInfo.ReleaseType)
                        Label13.Text = CastDismRestartType(PkgInfo.RestartRequired)
                        Label49.Text = PkgInfo.SupportInformation
                        Label51.Text = CastDismPackageState(PkgInfo.PackageState)
                        Label53.Text = CastDismFullyOfflineInstallationType(PkgInfo.FullyOffline)
                        Label56.Text = PkgInfo.CapabilityId
                        Label57.Text = ""
                        Dim cProps As DismCustomPropertyCollection = PkgInfo.CustomProperties
                        If cProps.Count > 0 Then
                            For Each cProp As DismCustomProperty In cProps
                                Label57.Text &= "- " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
                            Next
                        Else
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENG"
                                            Label57.Text = "None"
                                        Case "ESN"
                                            Label57.Text = "Ninguna"
                                    End Select
                                Case 1
                                    Label57.Text = "None"
                                Case 2
                                    Label57.Text = "Ninguna"
                            End Select
                        End If
                        Label59.Text = ""
                        Dim pkgFeats As DismFeatureCollection = PkgInfo.Features
                        If pkgFeats.Count > 0 Then
                            ' Output all features
                            For Each pkgFeat As DismFeature In pkgFeats
                                Label59.Text &= "- " & pkgFeat.FeatureName & " (" & pkgFeat.State & ")" & CrLf
                            Next
                        Else
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENG"
                                            Label59.Text = "None"
                                        Case "ESN"
                                            Label59.Text = "Ninguna"
                                    End Select
                                Case 1
                                    Label59.Text = "None"
                                Case 2
                                    Label59.Text = "Ninguna"
                            End Select
                        End If
                    End Using
                    Panel4.Visible = True
                    Panel7.Visible = False
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label5.Text = "Ready"
                                Case "ESN"
                                    Label5.Text = "Listo"
                            End Select
                        Case 1
                            Label5.Text = "Ready"
                        Case 2
                            Label5.Text = "Listo"
                    End Select
                Finally
                    DismApi.Shutdown()
                End Try
            Else
                Panel4.Visible = False
                Panel7.Visible = True
            End If
        Catch ex As Exception
            Panel4.Visible = False
            Panel7.Visible = True
        End Try
    End Sub

    Sub GetPackageFileInformation()
        PackageInfoList.Clear()
        Try
            ' Background processes need to have completed before showing information
            If MainForm.ImgBW.IsBusy Then
                Dim msg As String = ""
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                msg = "Background processes need to have completed before showing package information. We'll wait until they have completed"
                            Case "ESN"
                                msg = "Los procesos en segundo plano deben haber completado antes de obtener información del paquete. Esperaremos hasta que hayan completado"
                        End Select
                    Case 1
                        msg = "Background processes need to have completed before showing package information. We'll wait until they have completed"
                    Case 2
                        msg = "Los procesos en segundo plano deben haber completado antes de obtener información del paquete. Esperaremos hasta que hayan completado"
                End Select
                MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Label5.Text = "Waiting for background processes to finish..."
                            Case "ESN"
                                Label5.Text = "Esperando a que terminen los procesos en segundo plano..."
                        End Select
                    Case 1
                        Label5.Text = "Waiting for background processes to finish..."
                    Case 2
                        Label5.Text = "Esperando a que terminen los procesos en segundo plano..."
                End Select
                While MainForm.ImgBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(500)
                End While
            End If
            If MainForm.MountedImageDetectorBW.IsBusy Then
                MainForm.MountedImageDetectorBW.CancelAsync()
                While MainForm.MountedImageDetectorBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(500)
                End While
            End If
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label5.Text = "Preparing package information processes..."
                        Case "ESN"
                            Label5.Text = "Preparando procesos de información de paquetes..."
                    End Select
                Case 1
                    Label5.Text = "Preparing package information processes..."
                Case 2
                    Label5.Text = "Preparando procesos de información de paquetes..."
            End Select
            Application.DoEvents()
            Try
                DismApi.Initialize(DismLogLevel.LogErrors)
                Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                    For Each pkgFile In ListBox1.Items
                        If File.Exists(pkgFile) Then
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENG"
                                            Label5.Text = "Getting information from package file " & Quote & Path.GetFileName(pkgFile) & Quote & "..." & CrLf & "This may take some time and the program may temporarily freeze"
                                        Case "ESN"
                                            Label5.Text = "Obteniendo información del archivo de paquete " & Quote & Path.GetFileName(pkgFile) & Quote & "..." & CrLf & "Esto puede llevar algo de tiempo y el programa podría congelarse temporalmente"
                                    End Select
                                Case 1
                                    Label5.Text = "Getting information from package file " & Quote & Path.GetFileName(pkgFile) & Quote & "..." & CrLf & "This may take some time and the program may temporarily freeze"
                                Case 2
                                    Label5.Text = "Obteniendo información del archivo de paquete " & Quote & Path.GetFileName(pkgFile) & Quote & "..." & CrLf & "Esto puede llevar algo de tiempo y el programa podría congelarse temporalmente"
                            End Select
                            Application.DoEvents()
                            Dim pkgInfo As DismPackageInfoEx = DismApi.GetPackageInfoExByPath(imgSession, pkgFile)
                            If pkgInfo IsNot Nothing Then PackageInfoList.Add(pkgInfo)
                        End If
                    Next
                End Using
            Catch DISMEx As DismException
                MsgBox(DISMEx.Message & " (HRESULT " & Hex(DISMEx.HResult) & ")", vbOKOnly + vbCritical, Label1.Text)
            Finally
                DismApi.Shutdown()
            End Try
        Catch ex As Exception
            ' Cancel it
        End Try
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label5.Text = "Ready"
                    Case "ESN"
                        Label5.Text = "Listo"
                End Select
            Case 1
                Label5.Text = "Ready"
            Case 2
                Label5.Text = "Listo"
        End Select
    End Sub

    Sub DisplayPackageFileInformation(PkgFile As Integer)
        Label9.Text = PackageInfoList(PkgFile).PackageName
        Label11.Text = CastDismApplicabilityStatus(PackageInfoList(PkgFile).Applicable)
        Label17.Text = PackageInfoList(PkgFile).Copyright
        Label19.Text = PackageInfoList(PkgFile).Company
        Label62.Text = PackageInfoList(PkgFile).CreationTime
        Label64.Text = PackageInfoList(PkgFile).Description
        Label66.Text = PackageInfoList(PkgFile).InstallClient
        Label68.Text = PackageInfoList(PkgFile).InstallPackageName
        Label70.Text = PackageInfoList(PkgFile).InstallTime
        Label72.Text = PackageInfoList(PkgFile).LastUpdateTime
        Label74.Text = PackageInfoList(PkgFile).DisplayName
        Label76.Text = PackageInfoList(PkgFile).ProductName
        Label78.Text = PackageInfoList(PkgFile).ProductVersion.ToString()
        Label80.Text = CastDismReleaseType(PackageInfoList(PkgFile).ReleaseType)
        Label82.Text = CastDismRestartType(PackageInfoList(PkgFile).RestartRequired)
        Label84.Text = PackageInfoList(PkgFile).SupportInformation
        Label86.Text = CastDismPackageState(PackageInfoList(PkgFile).PackageState)
        Label88.Text = CastDismFullyOfflineInstallationType(PackageInfoList(PkgFile).FullyOffline)
        Label90.Text = PackageInfoList(PkgFile).CapabilityId
        Label92.Text = ""
        Dim cProps As DismCustomPropertyCollection = PackageInfoList(PkgFile).CustomProperties
        If cProps.Count > 0 Then
            For Each cProp As DismCustomProperty In cProps
                Label92.Text &= "- " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
            Next
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label92.Text = "None"
                        Case "ESN"
                            Label92.Text = "Ninguna"
                    End Select
                Case 1
                    Label92.Text = "None"
                Case 2
                    Label92.Text = "Ninguna"
            End Select
        End If
        Label94.Text = ""
        Dim pkgFeats As DismFeatureCollection = PackageInfoList(PkgFile).Features
        If pkgFeats.Count > 0 Then
            ' Output all features
            For Each pkgFeat As DismFeature In pkgFeats
                Label94.Text &= "- " & pkgFeat.FeatureName & " (" & pkgFeat.State & ")" & CrLf
            Next
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label94.Text = "None"
                        Case "ESN"
                            Label94.Text = "Ninguna"
                    End Select
                Case 1
                    Label94.Text = "None"
                Case 2
                    Label94.Text = "Ninguna"
            End Select
        End If
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each PackageFile In PackageFiles
            If Path.GetExtension(PackageFile).EndsWith("cab", StringComparison.OrdinalIgnoreCase) Then
                ListBox1.Items.Add(PackageFile)
            End If
        Next
        Button3.Enabled = True
        GetPackageFileInformation()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Try
            If ListBox1.SelectedItems.Count = 1 Then
                NoPkgPanel.Visible = False
                PackageFileInfoPanel.Visible = True
                Button2.Enabled = True
                If PackageInfoList.Count > 0 Then DisplayPackageFileInformation(ListBox1.SelectedIndex)
            Else
                NoPkgPanel.Visible = True
                PackageFileInfoPanel.Visible = False
                Button2.Enabled = False
            End If
        Catch ex As Exception
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            NoPkgPanel.Visible = True
            PackageFileInfoPanel.Visible = False
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PackageInfoList.RemoveAt(ListBox1.SelectedIndex)
        ListBox1.Items.Remove(ListBox1.SelectedItem)
        If ListBox1.Items.Count > 1 Then Button2.Enabled = False Else Button2.Enabled = True
        NoPkgPanel.Visible = True
        PackageFileInfoPanel.Visible = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        PackageInfoList.Clear()
        ListBox1.Items.Clear()
        Button2.Enabled = False
        Button3.Enabled = False
        NoPkgPanel.Visible = True
        PackageFileInfoPanel.Visible = False
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        ListBox1.Items.Add(OpenFileDialog1.FileName)
        Button3.Enabled = True
        GetPackageFileInformation()
    End Sub

    Private Sub GetPkgInfoDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub
End Class
