Namespace Elements

    ''' <summary>
    ''' Class for AppX package files
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AppxPackage
        ''' <summary>
        ''' The package file, in APPX, MSIX, APPXBUNDLE, or MSIXBUNDLE format
        ''' </summary>
        ''' <remarks></remarks>
        Public PackageFile As String

        ''' <summary>
        ''' The name of the AppX package
        ''' </summary>
        ''' <remarks></remarks>
        Public PackageName As String

        ''' <summary>
        ''' The publisher of the application, usually in "CN=..., O=..., L=..., S=..., C=..." or in "CN=..." forms
        ''' </summary>
        ''' <remarks></remarks>
        Public PackagePublisher As String

        ''' <summary>
        ''' The version string of the application
        ''' </summary>
        ''' <remarks></remarks>
        Public PackageVersion As String

        ''' <summary>
        ''' The dependencies required by the application and specified in its manifest file
        ''' </summary>
        ''' <remarks></remarks>
        Public PackageRequiredDependencies As New List(Of AppxDependency)

        ''' <summary>
        ''' The dependency packages specified by the user
        ''' </summary>
        ''' <remarks></remarks>
        Public PackageSpecifiedDependencies As New List(Of AppxDependency)

        Public PackageCustomDataFile As String

        Public PackageRegions As String
    End Class

    ''' <summary>
    ''' Class for AppX package dependency files
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AppxDependency

        ''' <summary>
        ''' The dependency file specified by the user
        ''' </summary>
        ''' <remarks></remarks>
        Public DependencyFile As New List(Of String)

        ''' <summary>
        ''' The name of the AppX dependency
        ''' </summary>
        ''' <remarks></remarks>
        Public DependencyName As String

        ''' <summary>
        ''' The version of the AppX dependency
        ''' </summary>
        ''' <remarks></remarks>
        Public DependencyVersion As String
    End Class
End Namespace

