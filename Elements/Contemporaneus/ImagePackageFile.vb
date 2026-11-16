Imports Microsoft.Dism

Namespace Elements.Contemporaneus

    Public Class ImagePackageFile

        Public Property FilePath As String
        Public Property PackageName As String
        Public Property PackageApplicable As Boolean
        Public Property PackageCopyright As String
        Public Property PackageCompany As String
        Public Property PackageCreationTime As DateTime
        Public Property PackageDescription As String
        Public Property PackageInstallClient As String
        Public Property PackageInstallPackageName As String
        Public Property PackageInstallTime As DateTime
        Public Property PackageLastUpdateTime As DateTime
        Public Property PackageDisplayName As String
        Public Property PackageProductName As String
        Public Property PackageProductVersion As String
        Public Property PackageReleaseType As DismReleaseType
        Public Property PackageRestartRequired As DismRestartType
        Public Property PackageSupportInformation As String
        Public Property PackageState As DismPackageFeatureState
        Public Property PackageFullyOffline As DismFullyOfflineInstallableType
        Public Property PackageCapabilityId As String
        Public Property PackageCustomProperties As DismCustomPropertyCollection
        Public Property PackageFeatures As DismFeatureCollection

        Public Sub New()
        End Sub

        Public Sub New(filePath As String)
            Me.FilePath = filePath
        End Sub

    End Class

End Namespace
