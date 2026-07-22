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

        Public Function GetLocalizedRegistrationStatus(MountDirectory As String) As String
            If IsPackageRegistered(MountDirectory) Then
                Return LocalizationService.ForSection("ImageAppxPackage.RegStatus")("Yes.Button")
            End If

            Return LocalizationService.ForSection("ImageAppxPackage.RegStatus")("No.Button")
        End Function

    End Class

End Namespace
