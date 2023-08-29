Imports System.Xml.Serialization

Namespace Elements

    ''' <summary>
    ''' Class for AppX package files
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable(), XmlRoot("Identity")>
    Public Class AppxPackage
        ''' <summary>
        ''' The package file, in APPX, MSIX, APPXBUNDLE, or MSIXBUNDLE format
        ''' </summary>
        ''' <remarks></remarks>
        Public Property PackageFile As String

        ''' <summary>
        ''' The name of the AppX package
        ''' </summary>
        ''' <remarks></remarks>
        <XmlAttribute("Name")>
        Public Property PackageName As String

        ''' <summary>
        ''' The publisher of the application, usually in "CN=..., O=..., L=..., S=..., C=..." or in "CN=..." forms
        ''' </summary>
        ''' <remarks></remarks>
        <XmlAttribute("Publisher")>
        Public Property PackagePublisher As String

        ''' <summary>
        ''' The version string of the application
        ''' </summary>
        ''' <remarks></remarks>
        <XmlAttribute("Version")>
        Public Property PackageVersion As String

        <XmlAttribute("ProcessorArchitecture")>
        Public Property PackageArchitecture As String

        ''' <summary>
        ''' The dependencies required by the application and specified in its manifest file
        ''' </summary>
        ''' <remarks></remarks>
        Public Property PackageRequiredDependencies As New List(Of AppxDependency)

        ''' <summary>
        ''' The dependency packages specified by the user
        ''' </summary>
        ''' <remarks></remarks>
        Public Property PackageSpecifiedDependencies As New List(Of AppxDependency)

        Public Property PackageLicenseFile As String

        Public Property PackageCustomDataFile As String

        Public Property PackageRegions As String
    End Class

    ''' <summary>
    ''' Class for AppX package dependency files
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable(), XmlRoot("Dependencies")>
    Public Class AppxDependency

        ''' <summary>
        ''' The dependency file specified by the user
        ''' </summary>
        ''' <remarks></remarks>
        Public Property DependencyFile As String

        ''' <summary>
        ''' The name of the AppX dependency
        ''' </summary>
        ''' <remarks></remarks>
        Public Property DependencyName As String

        ''' <summary>
        ''' The version of the AppX dependency
        ''' </summary>
        ''' <remarks></remarks>
        Public Property DependencyVersion As String
    End Class
End Namespace

