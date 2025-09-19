Public Class WindowsService

    Enum ServiceStartType As Integer
        Unknown = -1
        BootLoader = 0
        IOSystem = 1
        Automatic = 2
        Manual = 3
        Disabled = 4
    End Enum

    Enum ServiceType As Integer
        Unknown = -1
        KernelDeviceDriver = 1
        FileSystemDriver = 2
        Adapter = 4
        WindowsApplication = 16
        WindowsService = 32
    End Enum

    Enum ServiceErrorControl As Integer
        Unknown = -1
        Ignore = 0
        Normal = 1
        Severe = 2
        Critical = 3
    End Enum

    Class ServiceFailureActions
        Public Property FirstFailure As ServiceFailureAction
        Public Property FirstDelayInMillis As Long
        Public Property SecondFailure As ServiceFailureAction
        Public Property SecondDelayInMillis As Long
        Public Property SubsequentFailure As ServiceFailureAction
        Public Property SubsequentDelaysInMillis As Long
        Public Property ResetDelayInSeconds As Integer

        Public Sub New()
            FirstFailure = ServiceFailureAction.NoAction
            FirstDelayInMillis = 0
            SecondFailure = ServiceFailureAction.NoAction
            SecondDelayInMillis = 0
            SubsequentFailure = ServiceFailureAction.NoAction
            SubsequentDelaysInMillis = 0
            ResetDelayInSeconds = 0
        End Sub

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

    Enum ServiceFailureAction As Integer
        Unknown = -1
        NoAction = 0
        RestartService = 1
        RestartComputer = 2
        RunProgram = 3
    End Enum

    Public Property Name As String
    Public Property DisplayName As String
    Public Property Description As String
    Public Property ObjectName As String
    Public Property ImagePath As String
    Public Property StartType As ServiceStartType
    Public Property DelayedStart As Boolean
    Public Property Type As ServiceType
    Public Property ErrorControl As ServiceErrorControl
    Public Property RequiredPrivileges As New List(Of NTSecurityPrivilegeConstant)
    Public Property Dependencies As String()
    Public Property FailureActions As ServiceFailureActions

    Public Sub New(name As String, displayName As String, description As String, objectName As String, imagePath As String, startType As ServiceStartType, delayedStart As Boolean, type As ServiceType, errorControl As ServiceErrorControl, ntPrivileges As List(Of NTSecurityPrivilegeConstant), deps As String(), failureActions As ServiceFailureActions)
        Me.Name = name
        Me.DisplayName = displayName
        Me.Description = description
        Me.ObjectName = objectName
        Me.ImagePath = imagePath
        Me.StartType = startType
        Me.DelayedStart = delayedStart
        Me.Type = type
        Me.ErrorControl = errorControl
        Me.RequiredPrivileges = ntPrivileges
        Me.Dependencies = deps
        Me.FailureActions = failureActions
    End Sub

    Public Function StartTypeToString() As String
        Select Case StartType
            Case WindowsService.ServiceStartType.BootLoader
                Return "Boot Loader"
            Case WindowsService.ServiceStartType.IOSystem
                Return "I/O System"
            Case WindowsService.ServiceStartType.Automatic
                Return "Automatic"
            Case WindowsService.ServiceStartType.Manual
                Return "Manual"
            Case WindowsService.ServiceStartType.Disabled
                Return "Disabled"
            Case Else
                Return String.Format("Unknown (Type {0})", StartType)
        End Select
    End Function

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
            Case Else
                Return String.Format("Unknown (Type {0})", Type)
        End Select
    End Function

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
