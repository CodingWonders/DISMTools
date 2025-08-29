Public Class InstallationStage

    ''' <summary>
    ''' The stage the installer is at
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum InstallerStage
        ''' <summary>
        ''' The installer is checking disk space
        ''' </summary>
        ''' <remarks></remarks>
        DiskSpaceChecker
        ''' <summary>
        ''' Files are being copied
        ''' </summary>
        ''' <remarks></remarks>
        FileCopy
        ''' <summary>
        ''' The BCD is being modified with the addition of new entries
        ''' </summary>
        ''' <remarks></remarks>
        BootEntryCreation
        ''' <summary>
        ''' The Windows PE image is being mounted
        ''' </summary>
        ''' <remarks></remarks>
        WIMMount
        ''' <summary>
        ''' The Windows PE image is being customized
        ''' </summary>
        ''' <remarks></remarks>
        WIMCustomize
        ''' <summary>
        ''' The Windows PE image is being unmounted
        ''' </summary>
        ''' <remarks></remarks>
        WIMUnmount
        ''' <summary>
        ''' A different thing is being done
        ''' </summary>
        ''' <remarks></remarks>
        Miscellaneous
    End Enum

End Class
