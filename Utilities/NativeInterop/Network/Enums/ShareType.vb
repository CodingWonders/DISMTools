Namespace Utilities.NativeInterop.Network.Enums

    ''' <summary>
    ''' The type of a shared resource, used in the shi*_type members of SHARE_INFO structures
    ''' </summary>
    ''' <remarks>Reference: lmshare.h</remarks>
    Public Enum ShareType As UInteger
        ''' <summary>
        ''' Disk drive or folder
        ''' </summary>
        ''' <remarks>STYPE_DISKTREE</remarks>
        DiskTree = 0
        ''' <summary>
        ''' Print queue
        ''' </summary>
        ''' <remarks>STYPE_PRINTQ</remarks>
        PrintQueue = 1
        ''' <summary>
        ''' Communication device
        ''' </summary>
        ''' <remarks>STYPE_DEVICE</remarks>
        Device = 2
        ''' <summary>
        ''' Interprocess communication (IPC)
        ''' </summary>
        ''' <remarks>STYPE_IPC</remarks>
        IPC = 3
        ''' <summary>
        ''' Special share reserved for interprocess communication (IPC$) or remote administration (ADMIN$, C$...). Combined with the other values
        ''' </summary>
        ''' <remarks>STYPE_SPECIAL</remarks>
        Special = &H80000000UI
        ''' <summary>
        ''' Temporary share. Combined with the other values
        ''' </summary>
        ''' <remarks>STYPE_TEMPORARY</remarks>
        Temporary = &H40000000UI
    End Enum

End Namespace
