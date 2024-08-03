Namespace Elements

    Public Class DiskConfiguration

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

        Public Property InstallRecEnv As Boolean

        Public Property RecEnvPartition As RecoveryEnvironmentLocation

        Public Property RecEnvSize As Integer

        ' CONFIGURATION FOR DISKPART SCRIPTS

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

        Public Property ScriptContents As String

        Public Property AutomaticInstall As Boolean

        Public Property TargetDisk As New Disk()

    End Class

    Public Class Disk

        Public Property DiskNum As Integer

        Public Property PartNum As Integer

    End Class

End Namespace
