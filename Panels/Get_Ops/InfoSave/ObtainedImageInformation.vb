Imports DISMTools.Elements.Contemporaneus
Imports DISMTools.Elements.Contemporaneus.ImageEXProperties
Imports Microsoft.Dism

Public Class ObtainedImageInformation

    Public Property Mode As ImageInformationMode = ImageInformationMode.Unknown
    Public Property InformationSourcePath As String
    Public Property InformationSnapshotTime As DateTime
    Public Property InformationEndTime As DateTime

#Region "Image Information"

    Public Property ImageList As New List(Of DismImageInfo)
    Public Property PackageList As New List(Of ImagePackageEX)
    Public Property EveryPackageObtained As Boolean
    Public Property PackageFileList As New List(Of ImagePackageFile)
    Public Property FeatureList As New List(Of ImageFeatureEX)
    Public Property EveryFeatureObtained As Boolean
    Public Property AppxList As New List(Of ImageAppxPackageEX)
    Public Property EveryAppxPackageObtained As Boolean
    Public Property CapabilityList As New List(Of ImageCapabilityEX)
    Public Property EveryCapabilityObtained As Boolean
    Public Property DriverList As New List(Of ImageDriverEX)
    Public Property EveryDriverObtained As Boolean
    Public Property DriverFileList As New List(Of ImageDriverFile)
    Public Property ServiceList As New List(Of WindowsService)

#End Region

    Public Sub New()
        Mode = ImageInformationMode.Unknown
        ImageList = New List(Of DismImageInfo)()
        PackageList = New List(Of ImagePackageEX)()
        PackageFileList = New List(Of ImagePackageFile)()
        FeatureList = New List(Of ImageFeatureEX)()
        AppxList = New List(Of ImageAppxPackageEX)()
        CapabilityList = New List(Of ImageCapabilityEX)()
        DriverList = New List(Of ImageDriverEX)()
        DriverFileList = New List(Of ImageDriverFile)()
        ServiceList = New List(Of WindowsService)()
    End Sub

End Class

Public Enum ImageInformationMode As Integer
    Unknown = -1
    WindowsImage = 0
    OnlineInstallation = 1
    OfflineInstallation = 2
End Enum
