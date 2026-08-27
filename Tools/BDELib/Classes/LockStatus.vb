Namespace Classes

    ''' <summary>
    ''' The lock status of a BitLocker encrypted volume.
    ''' </summary>
    Public Enum LockStatus As Integer
        ''' <summary>
        ''' Unknown lock status.
        ''' </summary>
        Unknown = -1
        ''' <summary>
        ''' The volume is unlocked, either with an auto-unlock key, or with a manually
        ''' provided key.
        ''' </summary>
        Unlocked = 0
        ''' <summary>
        ''' The volume is locked and must be unlocked to access the data in it.
        ''' </summary>
        Locked = 1
    End Enum

End Namespace
