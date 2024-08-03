Namespace Elements

    Public Class VirtualMachineSettings

        Public Property Provider As VMProvider

    End Class

    Public Enum VMProvider As Integer

        ''' <summary>
        ''' Oracle VM VirtualBox
        ''' </summary>
        ''' <remarks></remarks>
        VirtualBox_GAs = 0

        ''' <summary>
        ''' VMware Workstation/Fusion/etc.
        ''' </summary>
        ''' <remarks></remarks>
        VMware_Tools = 1

        ''' <summary>
        ''' QEMU/Proxmox VE/etc.
        ''' </summary>
        ''' <remarks></remarks>
        VirtIO_Guest_Tools = 2

    End Enum

End Namespace
