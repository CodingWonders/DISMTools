Public Class WindowsService

    Enum ServiceStartType
        Unknown = -1
        BootLoader = 0
        IOSystem = 1
        Automatic = 2
        Manual = 3
        Disabled = 4
    End Enum

    Enum ServiceType
        Unknown = -1
        KernelDeviceDriver = 1
        FileSystemDriver = 2
        Adapter = 4
        WindowsApplication = 16
        WindowsService = 32
    End Enum

    Enum ServiceErrorControl
        Unknown = -1
        Ignore = 0
        Normal = 1
        Severe = 2
        Critical = 3
    End Enum

    Public Property Name As String
    Public Property Description As String
    Public Property ObjectName As String
    Public Property ImagePath As String
    Public Property StartType As ServiceStartType
    Public Property Type As ServiceType
    Public Property ErrorControl As ServiceErrorControl
    Public Property RequiredPrivileges As New List(Of NTSecurityPrivilegeConstant)

    Public Sub New(name As String, description As String, objectName As String, imagePath As String, startType As ServiceStartType, type As ServiceType, errorControl As ServiceErrorControl, ntPrivileges As List(Of NTSecurityPrivilegeConstant))
        Me.Name = name
        Me.Description = description
        Me.ObjectName = objectName
        Me.ImagePath = imagePath
        Me.StartType = startType
        Me.Type = type
        Me.ErrorControl = errorControl
        Me.RequiredPrivileges = ntPrivileges
    End Sub

End Class
