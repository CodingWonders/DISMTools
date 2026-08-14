Namespace Classes

    ''' <summary>
    ''' The status for a wipe operation of a BitLocker volume.
    ''' </summary>
    Public Enum VolumeWipingStatus As Integer
        ''' <summary>
        ''' Unknown wiping status.
        ''' </summary>
        Unknown = -1
        ''' <summary>
        ''' Free space is not wiped.
        ''' </summary>
        FreeSpaceNotWiped = 0
        ''' <summary>
        ''' Free space is wiped.
        ''' </summary>
        FreeSpaceWiped = 1
        ''' <summary>
        ''' Free space is being wiped.
        ''' </summary>
        FreeSpaceWipingInProgress = 2
        ''' <summary>
        ''' A wipe operation has been paused.
        ''' </summary>
        FreeSpaceWipingPaused = 3
    End Enum

End Namespace
