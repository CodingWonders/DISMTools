Imports Microsoft.Dism

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

    End Class

End Namespace
