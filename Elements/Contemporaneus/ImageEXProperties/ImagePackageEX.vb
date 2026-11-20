Imports DISMTools.Elements.Contemporaneus

Namespace Elements.Contemporaneus.ImageEXProperties

    Public Class ImagePackageEX
        Inherits ImagePackage

        Public Property PackageApplicable As Boolean
        Public Property PackageCopyright As String
        Public Property PackageCompany As String
        Public Property PackageCreationTime As DateTime
        Public Property PackageDescription As String
        Public Property PackageInstallClient As String
        Public Property PackageInstallPackageName As String
        Public Property PackageLastUpdateTime As DateTime
        Public Property PackageDisplayName As String
        Public Property PackageProductName As String
        Public Property PackageProductVersion As String
        Public Property PackageRestartRequired As Microsoft.Dism.DismRestartType
        Public Property PackageSupportInformation As String
        Public Property PackageFullyOffline As Microsoft.Dism.DismFullyOfflineInstallableType
        Public Property PackageCapabilityId As String
        Public Property PackageCustomProperties As Microsoft.Dism.DismCustomPropertyCollection
        Public Property PackageFeatures As Microsoft.Dism.DismFeatureCollection

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(name As String, state As Microsoft.Dism.DismPackageFeatureState, installTime As DateTime, releaseType As Microsoft.Dism.DismReleaseType)
            MyBase.New(name, state, installTime, releaseType)
        End Sub

    End Class

End Namespace