Public Class WindowsService

    ''' <summary>
    ''' The start type of the service
    ''' </summary>
    ''' <remarks></remarks>
    Enum ServiceStartType As Integer
        ''' <summary>
        ''' Unknown or undefined start type
        ''' </summary>
        ''' <remarks></remarks>
        Unknown = -1
        ''' <summary>
        ''' The service is loaded during the boot loader stage
        ''' </summary>
        ''' <remarks></remarks>
        BootLoader = 0
        ''' <summary>
        ''' The service is loaded during the I/O System stage
        ''' </summary>
        ''' <remarks></remarks>
        IOSystem = 1
        ''' <summary>
        ''' The service is loaded automatically
        ''' </summary>
        ''' <remarks></remarks>
        Automatic = 2
        ''' <summary>
        ''' The service is loaded manually
        ''' </summary>
        ''' <remarks></remarks>
        Manual = 3
        ''' <summary>
        ''' The service is not loaded
        ''' </summary>
        ''' <remarks></remarks>
        Disabled = 4
    End Enum

    ''' <summary>
    ''' The service type
    ''' </summary>
    ''' <remarks></remarks>
    Enum ServiceType As Integer
        ''' <summary>
        ''' Unknown or undefined service type
        ''' </summary>
        ''' <remarks></remarks>
        Unknown = -1
        ''' <summary>
        ''' The service is a kernel device driver
        ''' </summary>
        ''' <remarks></remarks>
        KernelDeviceDriver = 1
        ''' <summary>
        ''' The service is a file system driver
        ''' </summary>
        ''' <remarks></remarks>
        FileSystemDriver = 2
        ''' <summary>
        ''' The service is an adapter
        ''' </summary>
        ''' <remarks></remarks>
        Adapter = 4
        ''' <summary>
        ''' The service is a Windows application
        ''' </summary>
        ''' <remarks></remarks>
        WindowsApplication = 16
        ''' <summary>
        ''' The service is a Windows service
        ''' </summary>
        ''' <remarks></remarks>
        WindowsService = 32
    End Enum

    ''' <summary>
    ''' The error control for a service
    ''' </summary>
    ''' <remarks></remarks>
    Enum ServiceErrorControl As Integer
        ''' <summary>
        ''' Unknown or undefined error control
        ''' </summary>
        ''' <remarks></remarks>
        Unknown = -1
        ''' <summary>
        ''' The startup program ignores the error and continues the startup operation
        ''' </summary>
        ''' <remarks></remarks>
        Ignore = 0
        ''' <summary>
        ''' The startup program logs the error in the event log but continues the startup operation
        ''' </summary>
        ''' <remarks></remarks>
        Normal = 1
        ''' <summary>
        ''' The startup program logs the error in the event log. If the last-known-good configuration is being started, the startup operation continues.
        ''' Otherwise, the system is restarted with the last-known-good configuration.
        ''' </summary>
        ''' <remarks></remarks>
        Severe = 2
        ''' <summary>
        ''' The startup program logs the error in the event log, if possible. If the last-known-good configuration is being started, the startup operation fails.
        ''' Otherwise, the system is restarted with the last-known good configuration.
        ''' </summary>
        ''' <remarks></remarks>
        Critical = 3
    End Enum

    ''' <summary>
    ''' The failure actions of a service
    ''' </summary>
    ''' <remarks></remarks>
    Class ServiceFailureActions
        ''' <summary>
        ''' The action when a service fails for a first time
        ''' </summary>
        ''' <remarks></remarks>
        Public Property FirstFailure As ServiceFailureAction
        ''' <summary>
        ''' The delay in milliseconds to wait after the service fails for a first time
        ''' </summary>
        ''' <remarks></remarks>
        Public Property FirstDelayInMillis As Long
        ''' <summary>
        ''' The action when a service fails for a second time
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SecondFailure As ServiceFailureAction
        ''' <summary>
        ''' The delay in milliseconds to wait after the service fails for a second time
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SecondDelayInMillis As Long
        ''' <summary>
        ''' The action when a service fails from there on
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SubsequentFailure As ServiceFailureAction
        ''' <summary>
        ''' The delay in milliseconds to wait after the service fails from there on
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SubsequentDelaysInMillis As Long
        ''' <summary>
        ''' The delay in seconds to wait until failure counts are reset
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ResetDelayInSeconds As Integer

        ''' <summary>
        ''' Initializes an object of this class with default values
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            FirstFailure = ServiceFailureAction.NoAction
            FirstDelayInMillis = 0
            SecondFailure = ServiceFailureAction.NoAction
            SecondDelayInMillis = 0
            SubsequentFailure = ServiceFailureAction.NoAction
            SubsequentDelaysInMillis = 0
            ResetDelayInSeconds = 0
        End Sub

        ''' <summary>
        ''' Initializes an object of this class with specified values
        ''' </summary>
        ''' <param name="firstFail">The action when a service fails for a first time</param>
        ''' <param name="firstDelay">The delay in milliseconds to wait after the service fails for a first time</param>
        ''' <param name="secondFail">The action when a service fails for a second time</param>
        ''' <param name="secondDelay">The delay in milliseconds to wait after the service fails for a second time</param>
        ''' <param name="subsequentFail">The action when a service fails from there on</param>
        ''' <param name="subsequentDelays">The delay in milliseconds to wait after the service fails from there on</param>
        ''' <param name="resetDelays">The delay in seconds to wait until failure counts are reset</param>
        ''' <remarks></remarks>
        Public Sub New(firstFail As ServiceFailureAction, firstDelay As Long, secondFail As ServiceFailureAction, secondDelay As Long, subsequentFail As ServiceFailureAction, subsequentDelays As Long, resetDelays As Long)
            FirstFailure = firstFail
            FirstDelayInMillis = If(firstDelay >= 0, firstDelay, 0)
            SecondFailure = secondFail
            SecondDelayInMillis = If(secondDelay >= 0, secondDelay, 0)
            SubsequentFailure = subsequentFail
            SubsequentDelaysInMillis = If(subsequentDelays >= 0, subsequentDelays, 0)
            ResetDelayInSeconds = If(resetDelays >= 0, resetDelays, 0)
        End Sub
    End Class

    ''' <summary>
    ''' The action to take when a service fails
    ''' </summary>
    ''' <remarks></remarks>
    Enum ServiceFailureAction As Integer
        ''' <summary>
        ''' Unknown or undefined failure action
        ''' </summary>
        ''' <remarks></remarks>
        Unknown = -1
        ''' <summary>
        ''' Take no action
        ''' </summary>
        ''' <remarks></remarks>
        NoAction = 0
        ''' <summary>
        ''' Restart Service
        ''' </summary>
        ''' <remarks></remarks>
        RestartService = 1
        ''' <summary>
        ''' Restart Computer
        ''' </summary>
        ''' <remarks></remarks>
        RestartComputer = 2
        ''' <summary>
        ''' Run a program
        ''' </summary>
        ''' <remarks></remarks>
        RunProgram = 3
    End Enum

    ''' <summary>
    ''' The name of the Windows service
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Name As String

    ''' <summary>
    ''' The name of the Windows service to display to the user
    ''' </summary>
    ''' <remarks></remarks>
    Public Property DisplayName As String

    ''' <summary>
    ''' The description of the Windows service
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Description As String

    ''' <summary>
    ''' The object name of the Windows service
    ''' </summary>
    ''' <remarks></remarks>
    Public Property ObjectName As String

    ''' <summary>
    ''' The path of the application to run when the service starts
    ''' </summary>
    ''' <remarks></remarks>
    Public Property ImagePath As String

    ''' <summary>
    ''' The group the service is in
    ''' </summary>
    ''' <remarks>Groups are used by the service host (svchost) to launch a batch of services given their group</remarks>
    Public Property Group As String

    ''' <summary>
    ''' The start type of a service
    ''' </summary>
    ''' <remarks></remarks>
    Public Property StartType As ServiceStartType

    ''' <summary>
    ''' Whether a service has a delayed startup type
    ''' </summary>
    ''' <remarks>
    ''' This is true IF AND ONLY IF the service is started automatically and the respective option is
    ''' checked in the Services window.
    ''' 
    ''' In other cases, this is false.
    ''' </remarks>
    Public Property DelayedStart As Boolean

    ''' <summary>
    ''' The type of the Windows service
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Type As ServiceType

    ''' <summary>
    ''' The error control of the Windows service
    ''' </summary>
    ''' <remarks></remarks>
    Public Property ErrorControl As ServiceErrorControl

    ''' <summary>
    ''' The privileges associated to the Windows service
    ''' </summary>
    ''' <remarks></remarks>
    Public Property RequiredPrivileges As New List(Of NTSecurityPrivilegeConstant)

    ''' <summary>
    ''' The names of the services this Windows service depends on
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Dependencies As String()

    ''' <summary>
    ''' The failure actions of the Windows service
    ''' </summary>
    ''' <remarks></remarks>
    Public Property FailureActions As ServiceFailureActions

    ''' <summary>
    ''' The service flags of a per-user service
    ''' </summary>
    ''' <remarks>Undocumented values for now</remarks>
    Public Property UserServiceFlags As Integer

    ''' <summary>
    ''' Whether the service will be scheduled for deletion
    ''' </summary>
    ''' <remarks></remarks>
    Public Property MarkedForDeletion As Boolean

    ''' <summary>
    ''' Initializes an object of the Windows Service class with specified values
    ''' </summary>
    ''' <param name="name">The name of the Windows service</param>
    ''' <param name="displayName">The name of the Windows service to display to the user</param>
    ''' <param name="description">The description of the Windows service</param>
    ''' <param name="objectName">The object name of the Windows service</param>
    ''' <param name="group">The group the service is in</param>
    ''' <param name="imagePath">The path of the application to run when the service starts</param>
    ''' <param name="startType">The start type of a service</param>
    ''' <param name="delayedStart">Whether a service has a delayed startup type</param>
    ''' <param name="type">The type of the Windows service</param>
    ''' <param name="errorControl">The error control of the Windows service</param>
    ''' <param name="ntPrivileges">The privileges associated to the Windows service</param>
    ''' <param name="deps">The names of the services this Windows service depends on</param>
    ''' <param name="failureActions">The failure actions of the Windows service</param>
    ''' <param name="userServiceFlags">The service flags of a per-user service</param>
    ''' <remarks></remarks>
    Public Sub New(name As String, displayName As String, description As String, objectName As String, imagePath As String, group As String, startType As ServiceStartType, delayedStart As Boolean, type As ServiceType, errorControl As ServiceErrorControl, ntPrivileges As List(Of NTSecurityPrivilegeConstant), deps As String(), failureActions As ServiceFailureActions, userServiceFlags As Integer)
        Me.Name = name
        Me.DisplayName = displayName
        Me.Description = description
        Me.ObjectName = objectName
        Me.ImagePath = imagePath
        Me.Group = group
        Me.StartType = startType
        Me.DelayedStart = delayedStart
        Me.Type = type
        Me.ErrorControl = errorControl
        Me.RequiredPrivileges = ntPrivileges
        Me.Dependencies = deps
        Me.FailureActions = failureActions
        Me.UserServiceFlags = userServiceFlags
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If Not TypeOf obj Is WindowsService Then Return False
        Return Name.Equals(CType(obj, WindowsService).Name, StringComparison.OrdinalIgnoreCase)
    End Function

    ''' <summary>
    ''' Parses a start type enum value to a string
    ''' </summary>
    ''' <returns>The string representation</returns>
    ''' <remarks></remarks>
    Public Function StartTypeToString() As String
        Select Case StartType
            Case WindowsService.ServiceStartType.BootLoader
                Return "Boot Loader"
            Case WindowsService.ServiceStartType.IOSystem
                Return "I/O System"
            Case WindowsService.ServiceStartType.Automatic
                Return "Automatic" & If(DelayedStart, " (Delayed Start)", "")
            Case WindowsService.ServiceStartType.Manual
                Return "Manual"
            Case WindowsService.ServiceStartType.Disabled
                Return "Disabled"
            Case Else
                Return String.Format("Unknown (Type {0})", StartType)
        End Select
    End Function

    ''' <summary>
    ''' Parses a type enum value to a string
    ''' </summary>
    ''' <returns>The string representation</returns>
    ''' <remarks></remarks>
    Public Function TypeToString() As String
        Select Case Type
            Case WindowsService.ServiceType.KernelDeviceDriver
                Return "Kernel Device Driver"
            Case WindowsService.ServiceType.FileSystemDriver
                Return "File System Driver"
            Case WindowsService.ServiceType.Adapter
                Return "Adapter"
            Case WindowsService.ServiceType.WindowsApplication
                Return "Windows Application"
            Case WindowsService.ServiceType.WindowsService
                Return "Windows Service"
            Case 80, 96
                ' https://woshub.com/manage-per-user-services-windows/
                Return "Per-user Service"
            Case Else
                Return String.Format("Unknown (Type {0})", Type)
        End Select
    End Function

    ''' <summary>
    ''' Parses an error control enum value to a string
    ''' </summary>
    ''' <returns>The string representation</returns>
    ''' <remarks></remarks>
    Public Function ErrorControlToString() As String
        Select Case ErrorControl
            Case ServiceErrorControl.Ignore
                Return "The startup program ignores the error and continues the startup operation."
            Case ServiceErrorControl.Normal
                Return "The startup program logs the error in the event log but continues the startup operation."
            Case ServiceErrorControl.Severe
                Return "The startup program logs the error in the event log. If the last-known-good configuration is being started, the startup operation continues. Otherwise, the system is restarted with the last-known-good configuration."
            Case ServiceErrorControl.Critical
                Return "The startup program logs the error in the event log, if possible. If the last-known-good configuration is being started, the startup operation fails. Otherwise, the system is restarted with the last-known good configuration."
            Case Else
                Return String.Format("Unknown (Type {0})", ErrorControl)
        End Select
    End Function

    ''' <summary>
    ''' Parses a failure action enum value to a string
    ''' </summary>
    ''' <returns>The string representation</returns>
    ''' <remarks></remarks>
    Public Function FailureActionToString(FailureAction As ServiceFailureAction) As String
        Select Case FailureAction
            Case ServiceFailureAction.NoAction
                Return "Take no action"
            Case ServiceFailureAction.RestartService
                Return "Restart Service"
            Case ServiceFailureAction.RestartComputer
                Return "Restart Computer"
            Case ServiceFailureAction.RunProgram
                Return "Run a program"
        End Select
        Return ""
    End Function

End Class
