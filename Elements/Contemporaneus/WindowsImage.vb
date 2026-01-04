Imports Microsoft.Dism
Imports System.Globalization
Imports Microsoft.Win32
Imports System.IO

Namespace Elements.Contemporaneus

    Public Class WindowsImage

        Public Property ImageMountGuid As Guid

        Public Property ImageFile As String
        Public Property ImageIndex As Integer
        Public Property ImageMountDirectory As String
        Public Property ImageMountStatus As DismMountStatus
        Public Property ImageMountMode As DismMountMode

        Public Property ImageName As String
        Public Property ImageDescription As String
        Public Property ImageSize As ULong
        Public Property ImageWimBootCompatible As Boolean
        Public Property ImageArchitecture As DismProcessorArchitecture
        Public Property ImageHal As String
        Public Property ImageVersion As Version
        Public Property ImageSpBuild As Integer
        Public Property ImageSpLevel As Integer
        Public Property ImageEditionId As String
        Public Property ImageInstallationType As String
        Public Property ImageProductType As String
        Public Property ImageProductSuite As String
        Public Property ImageSystemRoot As String
        Public Property ImageDirectoryCount As Integer
        Public Property ImageFileCount As Integer
        Public Property ImageCreationDate As Date
        Public Property ImageModificationDate As Date
        Public Property ImageLanguages As IEnumerable(Of CultureInfo)

        Public Property ImagePackages As DismPackageCollection
        Public Property ImageFeatures As DismFeatureCollection
        Public Property ImageAppxPackages As DismAppxPackageCollection
        Public Property ImageAppxPackages_Win8 As List(Of ImageAppxPackage)
        Public Property ImageCapabilities As DismCapabilityCollection
        Public Property ImageDrivers As DismDriverPackageCollection

        Public Sub New(imageFile As String, imageIndex As Integer, imageMountDir As String, imageMountStatus As DismMountStatus, imageMountMode As DismMountMode)
            Me.ImageFile = imageFile
            Me.ImageIndex = imageIndex
            Me.ImageMountDirectory = imageMountDir
            Me.ImageMountStatus = imageMountStatus
            Me.ImageMountMode = imageMountMode

            Me.ImageVersion = New Version(0, 0, 0, 0)
        End Sub

        Private Function GetImageMountGuid() As Guid
            Dim mountGuid As Guid
            Dim found As Boolean = False

            Try
                Dim wimmountRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\WIMMount\Mounted Images", False)
                For Each subkeyName In wimmountRk.GetSubKeyNames()
                    Try
                        Dim subkeyRk As RegistryKey = wimmountRk.OpenSubKey(subkeyName, False)
                        If subkeyRk.GetValue("Mount Path", "").Equals(Me.ImageMountDirectory) Then
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

        Private Function GetImageVersion() As Version
            Dim osVersion As Version = New Version(0, 0, 0, 0)

            Try
                DismApi.Initialize(DismLogLevel.LogErrors)
                osVersion = DismApi.GetImageInfo(Me.ImageFile).ElementAt(Me.ImageIndex - 1).ProductVersion
            Catch ex As Exception
                Dim kernelPath As String = Path.Combine(Me.ImageMountDirectory, "Windows", "system32", "ntoskrnl.exe")
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

        Public Function MountStatusToString(LangCode As Integer) As String
            Dim mountStatusString As String = ""

            Select Case ImageMountStatus
                Case DismMountStatus.Ok
                    Select Case LangCode
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    mountStatusString = "OK"
                                Case "ESN"
                                    mountStatusString = "Correcto"
                                Case "FRA"
                                    mountStatusString = "OK"
                                Case "PTB", "PTG"
                                    mountStatusString = "OK"
                                Case "ITA"
                                    mountStatusString = "OK"
                            End Select
                        Case 1
                            mountStatusString = "OK"
                        Case 2
                            mountStatusString = "Correcto"
                        Case 3
                            mountStatusString = "OK"
                        Case 4
                            mountStatusString = "OK"
                        Case 5
                            mountStatusString = "OK"
                    End Select
                Case DismMountStatus.NeedsRemount
                    Select Case LangCode
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    mountStatusString = "Needs Remount"
                                Case "ESN"
                                    mountStatusString = "Necesita recarga"
                                Case "FRA"
                                    mountStatusString = "Nécessite un remontage"
                                Case "PTB", "PTG"
                                    mountStatusString = "Necessita de remontagem"
                                Case "ITA"
                                    mountStatusString = "Necessità di rimontaggio"
                            End Select
                        Case 1
                            mountStatusString = "Needs Remount"
                        Case 2
                            mountStatusString = "Necesita recarga"
                        Case 3
                            mountStatusString = "Nécessite un remontage"
                        Case 4
                            mountStatusString = "Necessita de remontagem"
                        Case 5
                            mountStatusString = "Necessità di rimontaggio"
                    End Select
                Case DismMountStatus.Invalid
                    Select Case LangCode
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    mountStatusString = "Invalid"
                                Case "ESN"
                                    mountStatusString = "Inválido"
                                Case "FRA"
                                    mountStatusString = "Invalide"
                                Case "PTB", "PTG"
                                    mountStatusString = "Inválido"
                                Case "ITA"
                                    mountStatusString = "Non valido"
                            End Select
                        Case 1
                            mountStatusString = "Invalid"
                        Case 2
                            mountStatusString = "Inválido"
                        Case 3
                            mountStatusString = "Invalide"
                        Case 4
                            mountStatusString = "Inválido"
                        Case 5
                            mountStatusString = "Non valido"
                    End Select
            End Select

            Return mountStatusString
        End Function

        Public Function MountModeToString(LangCode As Integer) As String
            Dim mountModeString As String = ""

            Select Case ImageMountMode
                Case DismMountMode.ReadWrite
                    Select Case LangCode
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    mountModeString = "Yes"
                                Case "ESN"
                                    mountModeString = "Sí"
                                Case "FRA"
                                    mountModeString = "Oui"
                                Case "PTB", "PTG"
                                    mountModeString = "Sim"
                                Case "ITA"
                                    mountModeString = "Sì"
                            End Select
                        Case 1
                            mountModeString = "Yes"
                        Case 2
                            mountModeString = "Sí"
                        Case 3
                            mountModeString = "Oui"
                        Case 4
                            mountModeString = "Sim"
                        Case 5
                            mountModeString = "Sì"
                    End Select
                Case DismMountMode.ReadOnly
                    Select Case LangCode
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    mountModeString = "No"
                                Case "ESN"
                                    mountModeString = "No"
                                Case "FRA"
                                    mountModeString = "Non"
                                Case "PTB", "PTG"
                                    mountModeString = "Não"
                                Case "ITA"
                                    mountModeString = "No"
                            End Select
                        Case 1
                            mountModeString = "No"
                        Case 2
                            mountModeString = "No"
                        Case 3
                            mountModeString = "Non"
                        Case 4
                            mountModeString = "Não"
                        Case 5
                            mountModeString = "No"
                    End Select
            End Select

            Return mountModeString
        End Function
    End Class

End Namespace