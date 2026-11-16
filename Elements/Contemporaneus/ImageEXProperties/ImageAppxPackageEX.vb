Imports DISMTools.Elements.Contemporaneus
Imports Microsoft.Dism

Namespace Elements.Contemporaneus.ImageEXProperties

    Public Class ImageAppxPackageEX
        Inherits ImageAppxPackage

        Public Property PackageDisplayName As String
        Public Property PackageInstallLocation As String
        Public Property PackageRegistrationStatus As String
        Public Property PackageManifestLocation As String
        Public Property PackageStoreLogoAssetDirectory As String
        Public Property PackageMainStoreLogoAsset As String

        Public Sub New(name As String, fullName As String, architecture As DismProcessorArchitecture, resourceId As String, version As Version)
            MyBase.New(name, fullName, architecture, resourceId, version)
        End Sub

    End Class

End Namespace
