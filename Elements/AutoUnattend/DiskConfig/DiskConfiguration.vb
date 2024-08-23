Namespace Elements

    Public Class DiskConfiguration

        ''' <summary>
        ''' The disk configuration mode
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Possible values: look at enum declaration</remarks>
        Public Property DiskConfigMode As DiskConfigurationMode

        ' CONFIGURATION FOR DISK 0

        ''' <summary>
        ''' Partition style
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartStyle As PartitionStyle

        ''' <summary>
        ''' The size of the EFI System Partition (in MB)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ESPSize As Integer

        ''' <summary>
        ''' Determines whether to install a Recovery Environment
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InstallRecEnv As Boolean

        ''' <summary>
        ''' The location of the recovery partition
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Possible values: look at enum declaration</remarks>
        Public Property RecEnvPartition As RecoveryEnvironmentLocation

        ''' <summary>
        ''' The size of the recovery partition if chosen
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RecEnvSize As Integer

        ' CONFIGURATION FOR DISKPART SCRIPTS

        ''' <summary>
        ''' DiskPart Script configuration
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Look at class declaration for more details</remarks>
        Public Property DiskPartScriptConfig As New DPConfig()

    End Class

    Public Enum DiskConfigurationMode As Integer
        AutoDisk0 = 0
        DiskPart = 1
    End Enum

    Public Enum PartitionStyle As Integer
        MBR = 0
        GPT = 1
    End Enum

    Public Enum RecoveryEnvironmentLocation As Integer
        WinREPartition = 0
        WindowsPartition = 1
    End Enum

    Public Class DPConfig

        ''' <summary>
        ''' The contents of a DiskPart script file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScriptContents As String

        ''' <summary>
        ''' Determines whether to automatically install to the first available partition with enough space and no installations
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AutomaticInstall As Boolean

        ''' <summary>
        ''' Manual disk configuration
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Look at class declaration for more details</remarks>
        Public Property TargetDisk As New Disk()

    End Class

    Public Class Disk

        Public Property DiskNum As Integer

        Public Property PartNum As Integer

    End Class

End Namespace
