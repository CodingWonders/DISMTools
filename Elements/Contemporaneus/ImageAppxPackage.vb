Imports Microsoft.Dism
Imports System.IO

Namespace Elements.Contemporaneus

    Public Class ImageAppxPackage

        Public Property PackageName As String
        Public Property PackageFullName As String
        Public Property PackageArchitecture As DismProcessorArchitecture
        Public Property PackageResourceId As String
        Public Property PackageVersion As Version

        Public Sub New(name As String, fullName As String, architecture As DismProcessorArchitecture, resourceId As String, version As Version)
            PackageName = name
            PackageFullName = fullName
            PackageArchitecture = architecture
            PackageResourceId = resourceId
            PackageVersion = version
        End Sub

        Private Function IsPackageRegistered(MountDirectory As String) As Boolean
            Dim isRegistered As Boolean = False
            Try
                If Directory.Exists(MountDirectory & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & PackageFullName) Then
                    isRegistered = My.Computer.FileSystem.GetFiles(MountDirectory & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & PackageFullName, FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count > 0
                End If
            Catch ex As Exception
                ' Ignore
            End Try
            Return isRegistered
        End Function

        Public Function GetLocalizedRegistrationStatus(MountDirectory As String, LangCode As Integer) As String
            Dim registrationString As String = ""

            If IsPackageRegistered(MountDirectory) Then
                Select Case LangCode
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                registrationString = "Yes"
                            Case "ESN"
                                registrationString = "Sí"
                            Case "FRA"
                                registrationString = "Oui"
                            Case "PTB", "PTG"
                                registrationString = "Sim"
                            Case "ITA"
                                registrationString = "Sì"
                        End Select
                    Case 1
                        registrationString = "Yes"
                    Case 2
                        registrationString = "Sí"
                    Case 3
                        registrationString = "Oui"
                    Case 4
                        registrationString = "Sim"
                    Case 5
                        registrationString = "Sì"
                End Select
            Else
                Select Case LangCode
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                registrationString = "No"
                            Case "ESN"
                                registrationString = "No"
                            Case "FRA"
                                registrationString = "Non"
                            Case "PTB", "PTG"
                                registrationString = "Não"
                            Case "ITA"
                                registrationString = "No"
                        End Select
                    Case 1
                        registrationString = "No"
                    Case 2
                        registrationString = "No"
                    Case 3
                        registrationString = "Non"
                    Case 4
                        registrationString = "Não"
                    Case 5
                        registrationString = "No"
                End Select
            End If
            Return registrationString
        End Function

    End Class

End Namespace
