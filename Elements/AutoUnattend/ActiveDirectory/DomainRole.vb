''' <summary>
''' Role of a computer in an assigned domain workgroup
''' </summary>
Public Enum DomainRole As Integer
    ''' <summary>
    ''' Unknown domain role definition
    ''' </summary>
    Unknown = -1
    ''' <summary>
    ''' Standalone Workstation
    ''' </summary>
    StandaloneWorkstation = 0
    ''' <summary>
    ''' Member Workstation
    ''' </summary>
    MemberWorkstation = 1
    ''' <summary>
    ''' Standalone Server
    ''' </summary>
    StandaloneServer = 2
    ''' <summary>
    ''' Member Server
    ''' </summary>
    MemberServer = 3
    ''' <summary>
    ''' Backup Domain Controller
    ''' </summary>
    BackupDomainController = 4
    ''' <summary>
    ''' Primary Domain Controller
    ''' </summary>
    PrimaryDomainController = 5
End Enum