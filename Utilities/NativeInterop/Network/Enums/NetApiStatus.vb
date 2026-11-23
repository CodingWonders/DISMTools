Namespace Utilities.NativeInterop.Network.Enums

    ''' <summary>
    ''' Status code for Windows Network API functions
    ''' </summary>
    ''' <remarks>References: lmerr.h; winerror.h</remarks>
    Public Enum NetApiStatus
        ''' <summary>
        ''' The operation completed successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Success = 0
        ''' <summary>
        ''' Access is denied.
        ''' </summary>
        ''' <remarks>ERROR_ACCESS_DENIED</remarks>
        AccessDenied = 5
        ''' <summary>
        ''' Not enough memory resources are available to process this command.
        ''' </summary>
        ''' <remarks>ERROR_NOT_ENOUGH_MEMORY</remarks>
        NotEnoughMemory = 8
        ''' <summary>
        ''' The parameter is incorrect.
        ''' </summary>
        ''' <remarks>ERROR_INVALID_PARAMETER</remarks>
        InvalidParameter = 87
        ''' <summary>
        ''' The filename, directory name, or volume label syntax is incorrect.
        ''' </summary>
        ''' <remarks>ERROR_INVALID_NAME</remarks>
        InvalidName = 123
        ''' <summary>
        ''' The system call level is not correct.
        ''' </summary>
        ''' <remarks>ERROR_INVALID_LEVEL</remarks>
        InvalidLevel = 124
        ''' <summary>
        ''' The device or directory does not exist.
        ''' </summary>
        ''' <remarks>NERR_UnknownDevDir</remarks>
        UnknownDevDir = 2116
        ''' <summary>
        ''' The share name is already in use on this server.
        ''' </summary>
        ''' <remarks>NERR_DuplicateShare</remarks>
        DuplicateShare = 2118
        ''' <summary>
        ''' The share name does not exist.
        ''' </summary>
        ''' <remarks>NERR_NetNameNotFound</remarks>
        NetNameNotFound = 2310
        ''' <summary>
        ''' Could not find domain controller for this domain.
        ''' </summary>
        ''' <remarks></remarks>
        DCNotFound = 2453
    End Enum

End Namespace
