Imports Microsoft.Dism

Namespace Elements.Contemporaneus

    Public Class ImagePackage

        Public Property PackageName As String
        Public Property PackageState As DismPackageFeatureState
        Public Property PackageInstallTime As DateTime
        Public Property PackageReleaseType As DismReleaseType

        Public Sub New(name As String, state As DismPackageFeatureState, installTime As DateTime, releaseType As DismReleaseType)
            PackageName = name
            PackageState = state
            PackageInstallTime = installTime
            PackageReleaseType = releaseType
        End Sub

    End Class

End Namespace
