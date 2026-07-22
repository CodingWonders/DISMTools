Imports Microsoft.VisualBasic.ControlChars
Imports System.IO
Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.ServiceProcess

Module WindowsServiceHelper

    NotInheritable Class NativeMethods

        Private Sub New()
        End Sub

        <DllImport("shlwapi.dll", BestFitMapping:=False, CharSet:=CharSet.Unicode, ExactSpelling:=True, SetLastError:=False, ThrowOnUnmappableChar:=True)>
        Shared Function SHLoadIndirectString(pszSource As String, pszOutBuf As StringBuilder, cchOutBuf As Integer, ppvReserved As IntPtr) As Integer
        End Function

    End Class

    ''' <summary>
    ''' The dictionary containing privilege constants.
    ''' </summary>
    ''' <remarks>
    ''' Keys are the security privilege constants defined by the Windows NT headers.
    ''' For example, "SE_ASSIGNPRIMARYTOKEN_NAME" or "SE_SHUTDOWN_NAME". Values are objects of the
    ''' Windows NT security privilege constants containing the representations of the constants
    ''' as defined in the Windows Registry, a description of said privilege constant, and user rights
    ''' of said privilege constants.
    ''' </remarks>
    Private PrivilegeConstantDictionary As New Dictionary(Of String, NTSecurityPrivilegeConstant)

    ''' <summary>
    ''' The dictionary containing mapping objects between the user right of a privilege constant and the constants
    ''' defined by the Windows NT headers.
    ''' </summary>
    ''' <remarks>
    ''' Keys are the user rights of a privilege constant. Values are the constants defined by the Windows NT headers.
    ''' </remarks>
    Private PrivilegeMappingDictionary As New Dictionary(Of String, String)

    ''' <summary>
    ''' Clears the constant dictionaries if already filled, then fills them with constant data.
    ''' </summary>
    ''' <remarks>
    ''' Constant data is defined in Microsoft documentation:
    ''' https://learn.microsoft.com/en-us/windows/win32/secauthz/privilege-constants
    ''' </remarks>
    Private Sub FillInConstants()
        DynaLog.LogMessage("Clearing dictionaries...")
        PrivilegeConstantDictionary.Clear()
        PrivilegeMappingDictionary.Clear()
        DynaLog.LogMessage("Filling privilege constant dictionary...")
        PrivilegeConstantDictionary.Add("SE_ASSIGNPRIMARYTOKEN_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeAssignPrimaryTokenPrivilege",
                                            "Replace a process-level token",
                                            "Required to assign the primary token of a process."))
        PrivilegeConstantDictionary.Add("SE_AUDIT_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeAuditPrivilege",
                                            "Generate security audits",
                                            "Required to generate audit-log entries. Give this privilege to secure servers."))
        PrivilegeConstantDictionary.Add("SE_BACKUP_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeBackupPrivilege",
                                            "Back up files and directories",
                                            "Required to perform backup operations. This privilege causes the system to grant all read access control to any file, regardless of the RegSaveKeyEx functions. The following access rights are granted if this privilege is held: READ_CONTROL, ACCESS_SYSTEM_SECURITY, FILE_GENERIC_READ, FILE_TRAVERSE"))
        PrivilegeConstantDictionary.Add("SE_CHANGE_NOTIFY_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeChangeNotifyPrivilege",
                                            "Bypass traverse checking",
                                            "Required to receive notifications of changes to files or directories. This privilege also causes the system to skip all traversal access checks. It is enabled by default for all users."))
        PrivilegeConstantDictionary.Add("SE_CREATE_GLOBAL_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeCreateGlobalPrivilege",
                                            "Create global objects",
                                            "Required to create named file mapping objects in the global namespace during Terminal Services sessions. This privilege is enabled by default for administrators, services, and the local system account."))
        PrivilegeConstantDictionary.Add("SE_CREATE_PAGEFILE_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeCreatePagefilePrivilege",
                                            "Create a pagefile",
                                            "Required to create a paging file."))
        PrivilegeConstantDictionary.Add("SE_CREATE_PERMANENT_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeCreatePermanentPrivilege",
                                            "Create permanent shared objects",
                                            "Required to create a permanent object."))
        PrivilegeConstantDictionary.Add("SE_CREATE_SYMBOLIC_LINK_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeCreateSymbolicLinkPrivilege",
                                            "Create symbolic links",
                                            "Required to create a symbolic link."))
        PrivilegeConstantDictionary.Add("SE_CREATE_TOKEN_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeCreateTokenPrivilege",
                                            "Create a token object",
                                            "Required to create a primary token. You cannot add this privilege to a user account with the " & Quote & "Create a token object" & Quote & " policy. Additionally, you cannot add this privilege to an owned process using Windows APIs. Windows Server 2003 and Windows XP with SP1 and earlier: Windows APIs can add this privilege to an owned process."))
        PrivilegeConstantDictionary.Add("SE_DEBUG_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeDebugPrivilege",
                                            "Debug programs",
                                            "Debug and adjust the memory of any process, ignoring the DACL for the process."))
        PrivilegeConstantDictionary.Add("SE_DELEGATE_SESSION_USER_IMPERSONATE_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeDelegateSessionUserImpersonatePrivilege",
                                            "Impersonate other users",
                                            "Required to obtain an impersonation token for another user in the same session."))
        PrivilegeConstantDictionary.Add("SE_ENABLE_DELEGATION_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeEnableDelegationPrivilege",
                                            "Enable computer and user accounts to be trusted for delegation",
                                            "Required to mark user and computer accounts as trusted for delegation."))
        PrivilegeConstantDictionary.Add("SE_IMPERSONATE_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeImpersonatePrivilege",
                                            "Impersonate a client after authentication",
                                            "Required to impersonate."))
        PrivilegeConstantDictionary.Add("SE_INC_BASE_PRIORITY_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeIncreaseBasePriorityPrivilege",
                                            "Increase scheduling priority",
                                            "Required to increase the base priority of a process."))
        PrivilegeConstantDictionary.Add("SE_INCREASE_QUOTA_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeIncreaseQuotaPrivilege",
                                            "Adjust memory quotas for a process",
                                            "Required to increase the quota assigned to a process."))
        PrivilegeConstantDictionary.Add("SE_INC_WORKING_SET_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeIncreaseWorkingSetPrivilege",
                                            "Increase a process working set",
                                            "Required to allocate more memory for applications that run in the context of users."))
        PrivilegeConstantDictionary.Add("SE_LOAD_DRIVER_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeLoadDriverPrivilege",
                                            "Load and unload device drivers",
                                            "Required to load or unload a device driver."))
        PrivilegeConstantDictionary.Add("SE_LOCK_MEMORY_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeLockMemoryPrivilege",
                                            "Lock pages in memory",
                                            "Required to lock physical pages in memory."))
        PrivilegeConstantDictionary.Add("SE_MACHINE_ACCOUNT_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeMachineAccountPrivilege",
                                            "Add workstations to domain",
                                            "Required to create a computer account."))
        PrivilegeConstantDictionary.Add("SE_MANAGE_VOLUME_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeManageVolumePrivilege",
                                            "Perform volume maintenance tasks",
                                            "Required to enable volume management privileges."))
        PrivilegeConstantDictionary.Add("SE_PROF_SINGLE_PROCESS_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeProfileSingleProcessPrivilege",
                                            "Profile single process",
                                            "Required to gather profiling information for a single process."))
        PrivilegeConstantDictionary.Add("SE_RELABEL_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeRelabelPrivilege",
                                            "Modify an object label",
                                            "Required to modify the mandatory integrity level of an object."))
        PrivilegeConstantDictionary.Add("SE_REMOTE_SHUTDOWN_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeRemoteShutdownPrivilege",
                                            "Force shutdown from a remote system",
                                            "Required to shut down a system using a network request."))
        PrivilegeConstantDictionary.Add("SE_RESTORE_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeRestorePrivilege",
                                            "Restore files and directories",
                                            "Required to perform restore operations. This privilege causes the system to grant all write access control to any file, regardless of the ACL specified for the file. Any access request other than write is still evaluated with the ACL. Additionally, this privilege enables you to set any valid user or group SID as the owner of a file. This privilege is required by the RegLoadKey function. The following access rights are granted if this privilege is held: WRITE_DAC, WRITE_OWNER, ACCESS_SYSTEM_SECURITY, FILE_GENERIC_WRITE, FILE_ADD_FILE, FILE_ADD_SUBDIRECTORY, DELETE"))
        PrivilegeConstantDictionary.Add("SE_SECURITY_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeSecurityPrivilege",
                                            "Manage auditing and security log",
                                            "Required to perform a number of security-related functions, such as controlling and viewing audit messages. This privilege identifies its holder as a security operator."))
        PrivilegeConstantDictionary.Add("SE_SHUTDOWN_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeShutdownPrivilege",
                                            "Shut down the system",
                                            "Required to shut down a local system."))
        PrivilegeConstantDictionary.Add("SE_SYNC_AGENT_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeSyncAgentPrivilege",
                                            "Synchronize directory service data",
                                            "Required for a domain controller to use the Lightweight Directory Access Protocol directory synchronization services. This privilege enables the holder to read all objects and properties in the directory, regardless of the protection on the objects and properties. By default, it is assigned to the Administrator and LocalSystem accounts on domain controllers."))
        PrivilegeConstantDictionary.Add("SE_SYSTEM_ENVIRONMENT_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeSystemEnvironmentPrivilege",
                                            "Modify firmware environment values",
                                            "Required to modify the nonvolatile RAM of systems that use this type of memory to store configuration information."))
        PrivilegeConstantDictionary.Add("SE_SYSTEM_PROFILE_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeSystemProfilePrivilege",
                                            "Profile system performance",
                                            "Required to gather profiling information for the entire system."))
        PrivilegeConstantDictionary.Add("SE_SYSTEMTIME_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeSystemtimePrivilege",
                                            "Change the system time",
                                            "Required to modify the system time."))
        PrivilegeConstantDictionary.Add("SE_TAKE_OWNERSHIP_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeTakeOwnershipPrivilege",
                                            "Take ownership of files or other objects",
                                            "Required to take ownership of an object without being granted discretionary access. This privilege allows the owner value to be set only to those values that the holder may legitimately assign as the owner of an object."))
        PrivilegeConstantDictionary.Add("SE_TCB_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeTcbPrivilege",
                                            "Act as part of the operating system",
                                            "This privilege identifies its holder as part of the trusted computer base. Some trusted protected subsystems are granted this privilege."))
        PrivilegeConstantDictionary.Add("SE_TIME_ZONE_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeTimeZonePrivilege",
                                            "Change the time zone",
                                            "Required to adjust the time zone associated with the computer's internal clock."))
        PrivilegeConstantDictionary.Add("SE_TRUSTED_CREDMAN_ACCESS_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeTrustedCredManAccessPrivilege",
                                            "Access Credential Manager as a trusted caller",
                                            "Required to access Credential Manager as a trusted caller."))
        PrivilegeConstantDictionary.Add("SE_UNDOCK_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeUndockPrivilege",
                                            "Remove computer from docking station",
                                            "Required to undock a laptop."))
        PrivilegeConstantDictionary.Add("SE_UNSOLICITED_INPUT_NAME",
                                        New NTSecurityPrivilegeConstant(
                                            "SeUnsolicitedInputPrivilege",
                                            "Not applicable",
                                            "Required to read unsolicited input from a terminal device."))

        DynaLog.LogMessage("Filling privilege mapping dictionary...")
        For Each key As String In PrivilegeConstantDictionary.Keys
            Dim privilegeConstant As NTSecurityPrivilegeConstant = PrivilegeConstantDictionary(key)
            PrivilegeMappingDictionary.Add(privilegeConstant.ConstantNameText, key)
        Next
    End Sub

    ''' <summary>
    ''' Resolves an indirect string
    ''' </summary>
    ''' <param name="source">The indirect string to resolve</param>
    ''' <returns>The resolved string</returns>
    ''' <remarks></remarks>
    Private Function ResolveIndirectString(source As String) As String
        DynaLog.LogMessage("Resolving indirect string " & source & "...")
        Dim buffer As New StringBuilder(260)
        Dim hr As Integer = NativeMethods.SHLoadIndirectString(source, buffer, buffer.Capacity, IntPtr.Zero)
        DynaLog.LogMessage("Resolver Result: " & hr)
        If hr = 0 Then
            Return buffer.ToString()
        Else
            Return source
        End If
    End Function

    ''' <summary>
    ''' Parses a line pointing to a INF file
    ''' </summary>
    ''' <param name="line">The line to parse</param>
    ''' <returns>A tuple containing the path to the INF file and the token to look for</returns>
    ''' <remarks></remarks>
    Private Function ParseInfLine(line As String) As Tuple(Of String, String)
        DynaLog.LogMessage("Parsing provided INF file line...")
        DynaLog.LogMessage("- Line: " & line)
        Dim noComment = line.Split(";"c)(0).Trim()
        Dim parts = noComment.Split(","c)

        DynaLog.LogMessage("Line Parts Length: " & parts.Length)
        If parts.Length < 2 Then
            DynaLog.LogMessage("Line Parts Length below minimum threshold.")
            Return Nothing
        End If

        Dim infFile = parts(0).Trim().TrimStart("@"c)
        Dim token = parts(1).Trim()

        Return Tuple.Create(infFile, token)
    End Function

    ''' <summary>
    ''' Resolves a INF token
    ''' </summary>
    ''' <param name="infPath">The path to the INF file</param>
    ''' <param name="token">The token to look for</param>
    ''' <returns>The value of the token in the INF file</returns>
    ''' <remarks></remarks>
    Private Function ResolveInfToken(infPath As String, token As String) As String
        DynaLog.LogMessage("Resolving INF File Tokens...")
        DynaLog.LogMessage("- INF File Path: " & infPath)
        DynaLog.LogMessage("- Token: " & token)
        Try
            Dim key As String = token.Trim("%"c)

            DynaLog.LogMessage("Opening file for read access...")
            Dim lines As String() = File.ReadAllLines(infPath)
            Dim inStrings As Boolean = False
            For Each line In lines
                Dim lineTrim As String = line.Trim()
                If lineTrim.StartsWith("[Strings]", StringComparison.OrdinalIgnoreCase) Then
                    inStrings = True
                    Continue For
                End If

                If inStrings Then
                    If lineTrim.StartsWith("[") Then Exit For

                    Dim match = Regex.Match(lineTrim, "^(?<k>[^\s=]+)\s*=\s*""?(?<v>[^""]+)""?$")
                    If match.Success AndAlso match.Groups("k").Value.Equals(key, StringComparison.OrdinalIgnoreCase) Then
                        DynaLog.LogMessage("Found a suitable match! Returning...")
                        Return match.Groups("v").Value
                    End If
                End If
            Next
        Catch ex As Exception
            DynaLog.LogMessage("Could not resolve token. Error message: " & ex.Message)
        End Try
        Return ""
    End Function

    Private Function GetOnlineServiceList() As List(Of WindowsService)
        Dim serviceList As New List(Of WindowsService)

        Try
            ' We only document a maximum of 999 control sets. CurrentControlSet is not a thing in an offline system, as the registry
            ' subsystems guess the control set to use based on values in HKLM\SYSTEM\Select.
            DynaLog.LogMessage("Opening mount path services key for read access...")
            Dim ServiceRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services", False)
            ' For some stupid reason, .NET keys are stored in HKLM\SYSTEM\ControlSet<nnn>\Services. GUID keys are also not allowed
            DynaLog.LogMessage("Getting service names...")
            Dim ServiceNames() As String = ServiceRk.GetSubKeyNames().Where(Function(serviceName) Not serviceName.StartsWith(".NET", StringComparison.OrdinalIgnoreCase) AndAlso Not serviceName.StartsWith("{")).ToArray()
            ServiceRk.Close()

            ' Now we have to grab as much information as we can
            For Each ServiceName In ServiceNames
                Dim serviceImagePath As String = "",
                    serviceEntryName As String = "",
                    serviceDisplayName As String = "",
                    serviceDescription As String = "",
                    serviceObjectName As String = "",
                    serviceGroupName As String = "",
                    serviceStartType As WindowsService.ServiceStartType = WindowsService.ServiceStartType.Unknown,
                    serviceDelayedStart As Boolean = False,
                    serviceType As WindowsService.ServiceType = WindowsService.ServiceType.Unknown,
                    serviceErrorControl As WindowsService.ServiceErrorControl = WindowsService.ServiceErrorControl.Unknown,
                    serviceRequiredPrivilegesString() As String = New String() {},
                    serviceDependencies() As String = New String() {},
                    serviceFailActionByteArr() As Byte = New Byte() {},
                    serviceUserServFlags As Integer = Integer.MinValue
                Using ServiceInfoRk As RegistryKey = Registry.LocalMachine.OpenSubKey(String.Format("SYSTEM\CurrentControlSet\Services\{0}", ServiceName), False)
                    ' We explicitly tell that we want to grab the raw data without env var expansion because REG_EXPAND_SZ values
                    ' are still string values, but with unexpanded environment variables. If the variable exists in the target system,
                    ' it will show that value.
                    serviceImagePath = ServiceInfoRk.GetValue("ImagePath", "", RegistryValueOptions.DoNotExpandEnvironmentNames)
                    If serviceImagePath = "" Then
                        DynaLog.LogMessage("The service image path cannot be obtained.")
                        ' This "service" is bogus
                        Continue For
                    End If

                    serviceEntryName = ServiceName
                    serviceDisplayName = ServiceInfoRk.GetValue("DisplayName", "")
                    DynaLog.LogMessage("Raw service display name: " & serviceDisplayName)
                    If serviceDisplayName.StartsWith("@") AndAlso serviceDisplayName.ToLowerInvariant().Contains(".inf") Then
                        DynaLog.LogMessage("Raw display name points to a device driver. Parsing...")
                        Dim parsedInf As Tuple(Of String, String) = ParseInfLine(serviceDisplayName)

                        If parsedInf IsNot Nothing Then
                            DynaLog.LogMessage("We have grabbed the path and the token. Continuing...")
                            Dim resolvedString As String = ResolveInfToken(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "INF", parsedInf.Item1), parsedInf.Item2)
                            DynaLog.LogMessage("- Resolved string (using current system): " & resolvedString)
                            If Not String.IsNullOrEmpty(resolvedString) Then
                                DynaLog.LogMessage("We grabbed the resolved string. Using that...")
                                serviceDisplayName = resolvedString
                            End If
                        End If
                    ElseIf serviceDisplayName.StartsWith("@") Then
                        DynaLog.LogMessage("Raw display name indicates an indirect string. Parsing...")
                        serviceDisplayName = ResolveIndirectString(serviceDisplayName)
                    End If
                    serviceDescription = ServiceInfoRk.GetValue("Description", "")
                    DynaLog.LogMessage("Raw service description: " & serviceDescription)
                    If serviceDescription.StartsWith("@") Then
                        DynaLog.LogMessage("Raw description indicates an indirect string. Parsing...")
                        serviceDescription = ResolveIndirectString(serviceDescription)
                    End If
                    serviceObjectName = ServiceInfoRk.GetValue("ObjectName", "")
                    serviceGroupName = ServiceInfoRk.GetValue("Group", "")
                    serviceStartType = ServiceInfoRk.GetValue("Start", -1)
                    serviceDelayedStart = (ServiceInfoRk.GetValue("DelayedAutoStart", 0) = 1)
                    serviceType = ServiceInfoRk.GetValue("Type", -1)
                    serviceErrorControl = ServiceInfoRk.GetValue("ErrorControl", -1)
                    ' The required privileges property is a multi-value registry value, so we need an array
                    serviceRequiredPrivilegesString = ServiceInfoRk.GetValue("RequiredPrivileges", New String() {})
                    ' Same goes for dependencies
                    serviceDependencies = ServiceInfoRk.GetValue("DependOnService", New String() {})
                    serviceFailActionByteArr = ServiceInfoRk.GetValue("FailureActions", New Byte() {})
                    serviceUserServFlags = ServiceInfoRk.GetValue("UserServiceFlags", Integer.MinValue)

                    Dim serviceRequiredPrivilegeList As New List(Of NTSecurityPrivilegeConstant)
                    DynaLog.LogMessage("Privilege items defined by the service: " & serviceRequiredPrivilegesString.Count)

                    If serviceRequiredPrivilegesString.Count > 0 Then
                        DynaLog.LogMessage("This service defines privileges. Getting privilege constant representations...")
                        ' Parse the items themselves to keys that are available in the dictionary we filled
                        ' stuff in
                        For Each serviceRequiredPrivilegeString In serviceRequiredPrivilegesString
                            If PrivilegeMappingDictionary.Keys.Contains(serviceRequiredPrivilegeString) Then
                                ' Then add it
                                Dim constantInHeader As String = PrivilegeMappingDictionary(serviceRequiredPrivilegeString)
                                serviceRequiredPrivilegeList.Add(PrivilegeConstantDictionary(constantInHeader))
                            End If
                        Next
                    End If

                    DynaLog.LogMessage("Adding service " & serviceEntryName & " to service list...")
                    serviceList.Add(New WindowsService(serviceEntryName,
                                                       serviceDisplayName,
                                                       serviceDescription,
                                                       serviceObjectName,
                                                       serviceImagePath,
                                                       serviceGroupName,
                                                       serviceStartType,
                                                       serviceDelayedStart,
                                                       serviceType,
                                                       serviceErrorControl,
                                                       serviceRequiredPrivilegeList,
                                                       serviceDependencies,
                                                       ParseFailureActionByteArray(serviceFailActionByteArr),
                                                       serviceUserServFlags))
                End Using
            Next
        Catch ex As Exception

        End Try

        Return serviceList
    End Function

    ''' <summary>
    ''' Gets a list of system services/devices from the mounted image
    ''' </summary>
    ''' <param name="MountPath">The path to the mounted image</param>
    ''' <returns>The service list</returns>
    ''' <remarks></remarks>
    Function GetServiceList(MountPath As String, Optional OnlineManagement As Boolean = False) As List(Of WindowsService)
        ' For the required privileges a service may have, we have to fill in the constants first so that we don't have things like
        ' "SeUndockPrivilege", "SeShutdownPrivilege"; but rather "Remove computer from docking station", and so on... we want the
        ' friendly things.
        DynaLog.LogMessage("Preparing to get all services in this image...")
        DynaLog.LogMessage("- Mount Path: " & MountPath)
        If OnlineManagement Then Return GetOnlineServiceList()

        DynaLog.LogMessage("Filling dictionaries...")
        FillInConstants()
        Dim serviceList As New List(Of WindowsService)

        ' Time to load up a registry hive
        DynaLog.LogMessage("Loading mount path SYSTEM hive...")
        If RegistryHelper.LoadRegistryHive(Path.Combine(MountPath, "Windows", "system32", "config", "SYSTEM"), "HKLM\zSYSTEM") = 0 Then
            DynaLog.LogMessage("Load operation succeeded. Continuing...")
            Try
                ' First we need to grab the default control set of the target image
                DynaLog.LogMessage("Determining default control set...")
                Dim DefaultControlSet As Integer = RegistryHelper.GetDefaultControlSet("zSYSTEM")
                DynaLog.LogMessage("Determined control set: " & DefaultControlSet)
                If DefaultControlSet = -1 Then
                    DynaLog.LogMessage("Control set is wrong.")
                    Throw New Exception("Registry control set could not be obtained")
                End If
                ' We only document a maximum of 999 control sets. CurrentControlSet is not a thing in an offline system, as the registry
                ' subsystems guess the control set to use based on values in HKLM\SYSTEM\Select.
                DynaLog.LogMessage("Opening mount path services key for read access...")
                Dim ServiceRk As RegistryKey = Registry.LocalMachine.OpenSubKey(String.Format("zSYSTEM\ControlSet{0}\Services", DefaultControlSet.ToString().PadLeft(3, "0")), False)
                ' For some stupid reason, .NET keys are stored in HKLM\SYSTEM\ControlSet<nnn>\Services. GUID keys are also not allowed
                DynaLog.LogMessage("Getting service names...")
                Dim ServiceNames() As String = ServiceRk.GetSubKeyNames().Where(Function(serviceName) Not serviceName.StartsWith(".NET", StringComparison.OrdinalIgnoreCase) AndAlso Not serviceName.StartsWith("{")).ToArray()
                ServiceRk.Close()

                ' Now we have to grab as much information as we can
                For Each ServiceName In ServiceNames
                    Dim serviceImagePath As String = "",
                        serviceEntryName As String = "",
                        serviceDisplayName As String = "",
                        serviceDescription As String = "",
                        serviceObjectName As String = "",
                        serviceGroupName As String = "",
                        serviceStartType As WindowsService.ServiceStartType = WindowsService.ServiceStartType.Unknown,
                        serviceDelayedStart As Boolean = False,
                        serviceType As WindowsService.ServiceType = WindowsService.ServiceType.Unknown,
                        serviceErrorControl As WindowsService.ServiceErrorControl = WindowsService.ServiceErrorControl.Unknown,
                        serviceRequiredPrivilegesString() As String = New String() {},
                        serviceDependencies() As String = New String() {},
                        serviceFailActionByteArr() As Byte = New Byte() {},
                        serviceUserServFlags As Integer = Integer.MinValue
                    Using ServiceInfoRk As RegistryKey = Registry.LocalMachine.OpenSubKey(String.Format("zSYSTEM\ControlSet{0}\Services\{1}", DefaultControlSet.ToString().PadLeft(3, "0"), ServiceName), False)
                        ' We explicitly tell that we want to grab the raw data without env var expansion because REG_EXPAND_SZ values
                        ' are still string values, but with unexpanded environment variables. If the variable exists in the target system,
                        ' it will show that value.
                        serviceImagePath = ServiceInfoRk.GetValue("ImagePath", "", RegistryValueOptions.DoNotExpandEnvironmentNames)
                        If serviceImagePath = "" Then
                            DynaLog.LogMessage("The service image path cannot be obtained.")
                            ' This "service" is bogus
                            Continue For
                        End If

                        serviceEntryName = ServiceName
                        serviceDisplayName = ServiceInfoRk.GetValue("DisplayName", "")
                        DynaLog.LogMessage("Raw service display name: " & serviceDisplayName)
                        If serviceDisplayName.StartsWith("@") AndAlso serviceDisplayName.ToLowerInvariant().Contains(".inf") Then
                            DynaLog.LogMessage("Raw display name points to a device driver. Parsing...")
                            Dim parsedInf As Tuple(Of String, String) = ParseInfLine(serviceDisplayName)

                            If parsedInf IsNot Nothing Then
                                DynaLog.LogMessage("We have grabbed the path and the token. Continuing...")
                                Dim resolvedString As String = ResolveInfToken(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "INF", parsedInf.Item1), parsedInf.Item2)
                                DynaLog.LogMessage("- Resolved string (using current system): " & resolvedString)
                                If Not String.IsNullOrEmpty(resolvedString) Then
                                    DynaLog.LogMessage("We grabbed the resolved string. Using that...")
                                    serviceDisplayName = resolvedString
                                End If
                            End If
                        ElseIf serviceDisplayName.StartsWith("@") Then
                            DynaLog.LogMessage("Raw display name indicates an indirect string. Parsing...")
                            serviceDisplayName = ResolveIndirectString(serviceDisplayName)
                        End If
                        serviceDescription = ServiceInfoRk.GetValue("Description", "")
                        DynaLog.LogMessage("Raw service description: " & serviceDescription)
                        If serviceDescription.StartsWith("@") Then
                            DynaLog.LogMessage("Raw description indicates an indirect string. Parsing...")
                            serviceDescription = ResolveIndirectString(serviceDescription)
                        End If
                        serviceObjectName = ServiceInfoRk.GetValue("ObjectName", "")
                        serviceGroupName = ServiceInfoRk.GetValue("Group", "")
                        serviceStartType = ServiceInfoRk.GetValue("Start", -1)
                        serviceDelayedStart = (ServiceInfoRk.GetValue("DelayedAutoStart", 0) = 1)
                        serviceType = ServiceInfoRk.GetValue("Type", -1)
                        serviceErrorControl = ServiceInfoRk.GetValue("ErrorControl", -1)
                        ' The required privileges property is a multi-value registry value, so we need an array
                        serviceRequiredPrivilegesString = ServiceInfoRk.GetValue("RequiredPrivileges", New String() {})
                        ' Same goes for dependencies
                        serviceDependencies = ServiceInfoRk.GetValue("DependOnService", New String() {})
                        serviceFailActionByteArr = ServiceInfoRk.GetValue("FailureActions", New Byte() {})
                        serviceUserServFlags = ServiceInfoRk.GetValue("UserServiceFlags", Integer.MinValue)

                        Dim serviceRequiredPrivilegeList As New List(Of NTSecurityPrivilegeConstant)
                        DynaLog.LogMessage("Privilege items defined by the service: " & serviceRequiredPrivilegesString.Count)

                        If serviceRequiredPrivilegesString.Count > 0 Then
                            DynaLog.LogMessage("This service defines privileges. Getting privilege constant representations...")
                            ' Parse the items themselves to keys that are available in the dictionary we filled
                            ' stuff in
                            For Each serviceRequiredPrivilegeString In serviceRequiredPrivilegesString
                                If PrivilegeMappingDictionary.Keys.Contains(serviceRequiredPrivilegeString) Then
                                    ' Then add it
                                    Dim constantInHeader As String = PrivilegeMappingDictionary(serviceRequiredPrivilegeString)
                                    serviceRequiredPrivilegeList.Add(PrivilegeConstantDictionary(constantInHeader))
                                End If
                            Next
                        End If

                        DynaLog.LogMessage("Adding service " & serviceEntryName & " to service list...")
                        serviceList.Add(New WindowsService(serviceEntryName,
                                                           serviceDisplayName,
                                                           serviceDescription,
                                                           serviceObjectName,
                                                           serviceImagePath,
                                                           serviceGroupName,
                                                           serviceStartType,
                                                           serviceDelayedStart,
                                                           serviceType,
                                                           serviceErrorControl,
                                                           serviceRequiredPrivilegeList,
                                                           serviceDependencies,
                                                           ParseFailureActionByteArray(serviceFailActionByteArr),
                                                           serviceUserServFlags))
                    End Using
                Next
            Catch ex As Exception

            End Try

            ' Now we unload that hive
            RegistryHelper.UnloadRegistryHive("HKLM\zSYSTEM")
        End If

        Return serviceList
    End Function

    ''' <summary>
    ''' Parses a failure action byte array into a service failure actions object
    ''' </summary>
    ''' <param name="FailureActions">The byte array to parse</param>
    ''' <returns>The set of failure actions</returns>
    ''' <remarks></remarks>
    Private Function ParseFailureActionByteArray(FailureActions As Byte()) As WindowsService.ServiceFailureActions
        Dim scFailure As WindowsService.ServiceFailureActions = New WindowsService.ServiceFailureActions()
        Dim firstFail As WindowsService.ServiceFailureAction = WindowsService.ServiceFailureAction.Unknown,
            secondFail As WindowsService.ServiceFailureAction = WindowsService.ServiceFailureAction.Unknown,
            subsequentFails As WindowsService.ServiceFailureAction = WindowsService.ServiceFailureAction.Unknown
        Dim firstDelay As Long = 0,
            secondDelay As Long = 0,
            subsequentDelay As Long = 0
        Dim resetDelay As Integer = 0
        DynaLog.LogMessage("Parsing specified failure action byte array...")
        DynaLog.LogMessage("Byte array length: " & FailureActions.Count)

        If FailureActions.Count >= 1 Then
            DynaLog.LogMessage("Some failure actions have been defined.")
            Try
                DynaLog.LogMessage("Getting service reset delay...")
                resetDelay = GetDelay(FailureActions.Skip(0).Take(4).ToArray())

                ' We have to get the number of byte elements twice because undefined failure measures
                ' cause our byte array to be smaller than expected, therefore causing indexes out of bounds.
                DynaLog.LogMessage("Getting 1st failure action and delay (in ms)...")
                firstFail = FailureActions(20)
                firstDelay = GetDelay(FailureActions.Skip(24).Take(4).ToArray())
                If FailureActions.Count > 28 Then
                    DynaLog.LogMessage("Byte array is long enough for second failure measures. Getting 2nd failure action and delay (in ms)...")
                    ' We have defined second failure measures
                    secondFail = FailureActions(28)
                    secondDelay = GetDelay(FailureActions.Skip(32).Take(4).ToArray())
                End If
                If FailureActions.Count > 36 Then
                    DynaLog.LogMessage("Byte array is long enough for subsequent failure measures. Getting subsequent failure actions and delays (in ms)...")
                    ' We have defined subsequent failure measures
                    subsequentFails = FailureActions(36)
                    subsequentDelay = GetDelay(FailureActions.Skip(40).Take(4).ToArray())
                End If
            Catch ex As Exception

            End Try

            scFailure = New WindowsService.ServiceFailureActions(firstFail, firstDelay, secondFail, secondDelay, subsequentFails, subsequentDelay, resetDelay)
        End If
        Return scFailure
    End Function

    Private Function ExportCurrentServiceInformation() As Boolean
        Dim defaultControlSet As Integer = GetDefaultControlSet("zSYSTEM")

        If defaultControlSet = -1 Then
            Return False
        End If

        Return RegistryHelper.ExportRegistryToFile(String.Format("HKLM\zSYSTEM\ControlSet{0}\Services", defaultControlSet.ToString().PadLeft(3, "0"c)),
                                                   Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                                                                String.Format("CurrentServiceInformation_{0}.reg", Date.UtcNow.ToString("yyyyMMdd-HHmmss")))) = 0
    End Function

    Public Function SaveServiceInformation(MountPath As String, ServiceList As List(Of WindowsService), Optional reportProgress As Action(Of Integer, Integer) = Nothing) As Boolean
        If RegistryHelper.LoadRegistryHive(Path.Combine(MountPath, "Windows", "system32", "config", "SYSTEM"), "HKLM\zSYSTEM") = 0 Then

            ' Export current key to at least have a backup, in case either we or the user
            ' screws up with service information.
            If Not ExportCurrentServiceInformation() Then
                ' Current service information could not be backed up. We'll ask the user
                ' if we can continue or not given the backup.
                If MsgBox(LocalizationService.ForSection("WindowsServices.Helper")("Service.Backed.Message"), vbYesNo + vbExclamation, LocalizationService.ForSection("WindowsServices.Helper")("Service.Backed.Up.Title")) = MsgBoxResult.No Then
                    Return False
                End If
            End If

            ' zSYSTEM to denote differences. DO NOT CHANGE FROM zSYSTEM TO SYSTEM. YOU WILL BREAK THE SYSTEM!!!!!
            Dim defaultControlSet As Integer = GetDefaultControlSet("zSYSTEM")

            If defaultControlSet = -1 Then
                Return False
            End If

            Dim failedSets As Integer = 0
            Dim currentService As Integer = 0,
                serviceCount As Integer = ServiceList.Count

            ' Now, we can save the properties. Only the start type for now
            DynaLog.DisableLogging()
            For Each Service As WindowsService In ServiceList
                currentService += 1
                Dim registryPath As String = String.Format("HKLM\zSYSTEM\ControlSet{0}\Services\{1}", defaultControlSet.ToString().PadLeft(3, "0"c), Service.Name)
                ' For those wondering: why not .NET APIs? Well, they throw access denied exceptions. Handling the exceptions tells us that
                ' none of the information will be saved. For example, if we have 672 service entries, all of them will fail if we use .NET APIs.
                ' However, if we use the APIs provided by the Registry Helper (which runs the reg program under the hood), we bring the failed
                ' set operations down to 9, and we **do** set this information using this method. We'll roll with it, though we'll disable
                ' DynaLog logging so it doesn't take about a minute.
                If Service.MarkedForDeletion Then
                    If RegistryHelper.RemoveRegistryItem(registryPath, "/va /f") <> 0 Then
                        failedSets += 1
                        Continue For
                    End If
                Else
                    If RegistryHelper.AddRegistryItem(New RegistryItem(registryPath, "Start", RegistryItem.ValueType.RegDword, Service.StartType)) <> 0 Then
                        failedSets += 1
                        Continue For
                    End If
                    If Service.StartType = WindowsService.ServiceStartType.Automatic Then
                        ' Currently, we assume that setting DelayedAutoStart to 0 does the same thing as removing this value altogether: turning off
                        ' delayed start. If this is not the case, we'll just get rid of it.
                        RegistryHelper.AddRegistryItem(New RegistryItem(registryPath, "DelayedAutoStart", RegistryItem.ValueType.RegDword, If(Service.DelayedStart, 1, 0)))
                    End If
                End If
                If reportProgress IsNot Nothing Then reportProgress.Invoke(currentService, serviceCount)
            Next
            DynaLog.EnableLogging()

            Debug.WriteLine("Service Count: " & ServiceList.Count)
            Debug.WriteLine("Failed Sets: " & failedSets)

            RegistryHelper.UnloadRegistryHive("HKLM\zSYSTEM")
        Else
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Gets a numeric delay from a byte array representing a delay for a specific fail
    ''' </summary>
    ''' <param name="ByteArray">The byte array to parse</param>
    ''' <returns>The numeric delay</returns>
    ''' <remarks></remarks>
    Private Function GetDelay(ByteArray As Byte()) As Long
        DynaLog.LogMessage("Getting numeric delay from our byte array...")
        Dim binary As String = ""

        For x = ByteArray.Length - 1 To 0 Step -1
            binary &= Convert.ToString(ByteArray(x), 2).PadLeft(8, "0"c)
        Next

        Return Convert.ToInt32(binary, 2)
    End Function

    Public Function GetSvchostGroups(MountDir As String, ServiceList As List(Of WindowsService)) As List(Of WindowsServiceHostGroup)
        Dim svchostGroups As New List(Of WindowsServiceHostGroup)

        Dim svchostGroupValues As String() = New String() {}
        Dim svchostGroupMappingDictionary As New Dictionary(Of String, String())

        ' First we get the registered service groups in svchost (HKLM\Software\Microsoft\Windows NT\CurrentVersion\Svchost)
        If RegistryHelper.LoadRegistryHive(Path.Combine(MountDir, "Windows", "system32", "config", "SOFTWARE"), "HKLM\zSOFTWARE") = 0 Then
            Dim svchostGroupsRk As RegistryKey = Registry.LocalMachine.OpenSubKey("zSOFTWARE\Microsoft\Windows NT\CurrentVersion\Svchost", False)
            svchostGroupValues = svchostGroupsRk.GetValueNames()

            For Each svchostGroupValue In svchostGroupValues
                ' we map them so we can iterate through the multiline values later
                svchostGroupMappingDictionary.Add(svchostGroupValue, svchostGroupsRk.GetValue(svchostGroupValue))
            Next

            svchostGroupsRk.Close()
            RegistryHelper.UnloadRegistryHive("HKLM\zSOFTWARE")
        End If

        ' We iterate through all the keys and filter the service list accordingly
        For Each key In svchostGroupMappingDictionary.Keys
            Dim filteredServices As New List(Of WindowsService)

            For Each serviceValue In svchostGroupMappingDictionary(key)
                filteredServices.Add(ServiceList.FirstOrDefault(Function(service) service.Name.Equals(serviceValue, StringComparison.InvariantCultureIgnoreCase)))
            Next

            svchostGroups.Add(New WindowsServiceHostGroup(key, filteredServices.Where(Function(service) service IsNot Nothing).ToList()))
        Next

        Return svchostGroups
    End Function

    ''' <summary>
    ''' Gets information about an installed Windows service from the current system using a provided service name.
    ''' </summary>
    ''' <param name="ServiceName">The name of the Windows service to get information about</param>
    ''' <returns>A <see cref="WindowsService" /> object containing information about the Windows service, or null if it can't find a service with that name.</returns>
    ''' <remarks></remarks>
    Public Function GetOnlineSystemServiceInformationByName(ServiceName As String) As WindowsService
        Dim detectedService As WindowsService = Nothing

        Dim serviceImagePath As String = "",
            serviceEntryName As String = "",
            serviceDisplayName As String = "",
            serviceDescription As String = "",
            serviceObjectName As String = "",
            serviceGroupName As String = "",
            serviceStartType As WindowsService.ServiceStartType = WindowsService.ServiceStartType.Unknown,
            serviceDelayedStart As Boolean = False,
            serviceType As WindowsService.ServiceType = WindowsService.ServiceType.Unknown,
            serviceErrorControl As WindowsService.ServiceErrorControl = WindowsService.ServiceErrorControl.Unknown,
            serviceRequiredPrivilegesString() As String = New String() {},
            serviceDependencies() As String = New String() {},
            serviceFailActionByteArr() As Byte = New Byte() {},
            serviceUserServFlags As Integer = Integer.MinValue

        ' Because we are now targeting the active system, we can pull info from the current control set.
        Try
            Using ServiceInfoRk As RegistryKey = Registry.LocalMachine.OpenSubKey(String.Format("SYSTEM\CurrentControlSet\Services\{0}", ServiceName), False)
                serviceImagePath = ServiceInfoRk.GetValue("ImagePath", "", RegistryValueOptions.DoNotExpandEnvironmentNames)
                If serviceImagePath = "" Then
                    DynaLog.LogMessage("The service image path cannot be obtained.")
                    ' This "service" is bogus
                    Return Nothing
                End If

                serviceEntryName = ServiceName
                serviceDisplayName = ServiceInfoRk.GetValue("DisplayName", "")
                DynaLog.LogMessage("Raw service display name: " & serviceDisplayName)
                If serviceDisplayName.StartsWith("@") AndAlso serviceDisplayName.ToLowerInvariant().Contains(".inf") Then
                    DynaLog.LogMessage("Raw display name points to a device driver. Parsing...")
                    Dim parsedInf As Tuple(Of String, String) = ParseInfLine(serviceDisplayName)

                    If parsedInf IsNot Nothing Then
                        DynaLog.LogMessage("We have grabbed the path and the token. Continuing...")
                        Dim resolvedString As String = ResolveInfToken(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "INF", parsedInf.Item1), parsedInf.Item2)
                        DynaLog.LogMessage("- Resolved string (using current system): " & resolvedString)
                        If Not String.IsNullOrEmpty(resolvedString) Then
                            DynaLog.LogMessage("We grabbed the resolved string. Using that...")
                            serviceDisplayName = resolvedString
                        End If
                    End If
                ElseIf serviceDisplayName.StartsWith("@") Then
                    DynaLog.LogMessage("Raw display name indicates an indirect string. Parsing...")
                    serviceDisplayName = ResolveIndirectString(serviceDisplayName)
                End If
                serviceDescription = ServiceInfoRk.GetValue("Description", "")
                DynaLog.LogMessage("Raw service description: " & serviceDescription)
                If serviceDescription.StartsWith("@") Then
                    DynaLog.LogMessage("Raw description indicates an indirect string. Parsing...")
                    serviceDescription = ResolveIndirectString(serviceDescription)
                End If
                serviceObjectName = ServiceInfoRk.GetValue("ObjectName", "")
                serviceGroupName = ServiceInfoRk.GetValue("Group", "")
                serviceStartType = ServiceInfoRk.GetValue("Start", -1)
                serviceDelayedStart = (ServiceInfoRk.GetValue("DelayedAutoStart", 0) = 1)
                serviceType = ServiceInfoRk.GetValue("Type", -1)
                serviceErrorControl = ServiceInfoRk.GetValue("ErrorControl", -1)
                ' The required privileges property is a multi-value registry value, so we need an array
                serviceRequiredPrivilegesString = ServiceInfoRk.GetValue("RequiredPrivileges", New String() {})
                ' Same goes for dependencies
                serviceDependencies = ServiceInfoRk.GetValue("DependOnService", New String() {})
                serviceFailActionByteArr = ServiceInfoRk.GetValue("FailureActions", New Byte() {})
                serviceUserServFlags = ServiceInfoRk.GetValue("UserServiceFlags", Integer.MinValue)

                Dim serviceRequiredPrivilegeList As New List(Of NTSecurityPrivilegeConstant)
                DynaLog.LogMessage("Privilege items defined by the service: " & serviceRequiredPrivilegesString.Count)

                If serviceRequiredPrivilegesString.Count > 0 Then
                    DynaLog.LogMessage("This service defines privileges. Getting privilege constant representations...")
                    ' Parse the items themselves to keys that are available in the dictionary we filled
                    ' stuff in
                    For Each serviceRequiredPrivilegeString In serviceRequiredPrivilegesString
                        If PrivilegeMappingDictionary.Keys.Contains(serviceRequiredPrivilegeString) Then
                            ' Then add it
                            Dim constantInHeader As String = PrivilegeMappingDictionary(serviceRequiredPrivilegeString)
                            serviceRequiredPrivilegeList.Add(PrivilegeConstantDictionary(constantInHeader))
                        End If
                    Next
                End If

                DynaLog.LogMessage("Adding service " & serviceEntryName & " to service list...")
                Return New WindowsService(serviceEntryName,
                                          serviceDisplayName,
                                          serviceDescription,
                                          serviceObjectName,
                                          serviceImagePath,
                                          serviceGroupName,
                                          serviceStartType,
                                          serviceDelayedStart,
                                          serviceType,
                                          serviceErrorControl,
                                          serviceRequiredPrivilegeList,
                                          serviceDependencies,
                                          ParseFailureActionByteArray(serviceFailActionByteArr),
                                          serviceUserServFlags)
            End Using
        Catch ex As Exception
            Return Nothing
        End Try

        Return detectedService
    End Function

    ''' <summary>
    ''' Installs a system service to the current Windows installation.
    ''' </summary>
    ''' <param name="NewService">A <see cref="WindowsService"/> object containing information about the service to install</param>
    ''' <returns>Whether the service was correctly installed.</returns>
    ''' <remarks></remarks>
    Public Function InstallService(NewService As WindowsService) As Boolean
        If NewService Is Nothing Then Throw New ArgumentNullException()
        If String.IsNullOrEmpty(NewService.Name) Then Throw New ArgumentNullException(NewService.Name)
        If String.IsNullOrEmpty(NewService.ImagePath) OrElse Not File.Exists(NewService.ImagePath) Then Throw New FileNotFoundException(String.Format("Service Path not found: {0}", NewService.ImagePath))

        ' Essential stuff out of the way
        Dim args As String = String.Format("create {1} binPath= {0}{2}{0}", Quote, NewService.Name, NewService.ImagePath)

        If {WindowsService.ServiceStartType.Automatic,
            WindowsService.ServiceStartType.Manual,
            WindowsService.ServiceStartType.Disabled}.Contains(NewService.StartType) Then
            Dim svcStartArgs As String = "start= "

            Select Case NewService.StartType
                Case WindowsService.ServiceStartType.Automatic
                    If NewService.DelayedStart Then
                        svcStartArgs &= "delayed-auto"
                    Else
                        svcStartArgs &= "auto"
                    End If
                Case WindowsService.ServiceStartType.Manual
                    svcStartArgs &= "demand"
                Case WindowsService.ServiceStartType.Disabled
                    svcStartArgs &= "disabled"
            End Select
            args &= String.Format(" {0}", svcStartArgs)
        End If

        If Not String.IsNullOrEmpty(NewService.DisplayName) Then args &= String.Format(" DisplayName= {0}{1}{0}", Quote, NewService.DisplayName)
        If NewService.Dependencies.Any() Then args &= String.Format(" depend= {0}", String.Join("/", NewService.Dependencies))

        Dim scProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "sc.exe"),
                .Arguments = args,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }
        }

        scProc.Start()
        scProc.WaitForExit()

        Return scProc.ExitCode = 0
    End Function

    ''' <summary>
    ''' Deletes a system service from the current Windows installation.
    ''' </summary>
    ''' <param name="ServiceName">The name of the service to delete</param>
    ''' <returns>Whether the service was correctly deleted.</returns>
    ''' <remarks></remarks>
    Public Function DeleteService(ServiceName As String) As Boolean
        If GetOnlineSystemServiceInformationByName(ServiceName) Is Nothing Then Return False

        Dim scProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "sc.exe"),
                .Arguments = String.Format("delete {0}", ServiceName),
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }
        }

        scProc.Start()
        scProc.WaitForExit()

        Return scProc.ExitCode = 0
    End Function

    ''' <summary>
    ''' Sets the description of an installed system service in the current Windows installation.
    ''' </summary>
    ''' <param name="ServiceName">The name of the service to which to set the description</param>
    ''' <param name="ServiceDescription">The description to set to the service</param>
    ''' <returns>Whether the operation succeeded.</returns>
    ''' <remarks></remarks>
    Public Function SetOnlineServiceDescription(ServiceName As String, ServiceDescription As String) As Boolean
        If GetOnlineSystemServiceInformationByName(ServiceName) Is Nothing Then Return False

        Dim scProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "sc.exe"),
                .Arguments = String.Format("description {1} {0}{2}{0}", Quote, ServiceName, ServiceDescription),
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }
        }

        scProc.Start()
        scProc.WaitForExit()

        Return scProc.ExitCode = 0
    End Function

    ''' <summary>
    ''' Enables a system service in the current Windows installation.
    ''' </summary>
    ''' <param name="ServiceName">The name of the system service to enable</param>
    ''' <param name="StartType">A custom start type for the service to enable</param>
    ''' <returns>Whether the operation succeeded.</returns>
    ''' <remarks></remarks>
    Public Function EnableOnlineService(ServiceName As String, Optional StartType As WindowsService.ServiceStartType = WindowsService.ServiceStartType.Automatic) As Boolean
        If GetOnlineSystemServiceInformationByName(ServiceName) Is Nothing Then Return False

        Dim args As String = String.Format("config {0}", ServiceName)

        If {WindowsService.ServiceStartType.Automatic,
            WindowsService.ServiceStartType.Manual,
            WindowsService.ServiceStartType.Disabled}.Contains(StartType) Then
            Dim svcStartArgs As String = "start= "

            Select Case StartType
                Case WindowsService.ServiceStartType.Automatic : svcStartArgs &= "auto"
                Case WindowsService.ServiceStartType.Manual : svcStartArgs &= "demand"
            End Select

            args &= String.Format(" {0}", svcStartArgs)
        Else
            args &= " start= demand"
        End If

        Dim scProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "sc.exe"),
                .Arguments = args,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }
        }

        scProc.Start()
        scProc.WaitForExit()

        Return scProc.ExitCode = 0
    End Function

    ''' <summary>
    ''' Disables a system service in the current Windows installation.
    ''' </summary>
    ''' <param name="ServiceName">The name of the system service to disable</param>
    ''' <returns>Whether the operation succeeded.</returns>
    ''' <remarks></remarks>
    Public Function DisableOnlineService(ServiceName As String) As Boolean
        If GetOnlineSystemServiceInformationByName(ServiceName) Is Nothing Then Return False

        Dim scProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "sc.exe"),
                .Arguments = String.Format("config {0} start= disabled", ServiceName),
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }
        }

        scProc.Start()
        scProc.WaitForExit()

        Return scProc.ExitCode = 0
    End Function

    ''' <summary>
    ''' Gets the current status of a system service in the current Windows installation.
    ''' </summary>
    ''' <param name="ServiceName">The name of the system service to query</param>
    ''' <returns>A <see cref="ServiceControllerStatus"/> object containing the current status of the queried service.</returns>
    ''' <remarks></remarks>
    Public Function GetOnlineServiceStartStatus(ServiceName As String) As ServiceControllerStatus
        If GetOnlineSystemServiceInformationByName(ServiceName) Is Nothing Then Return ServiceControllerStatus.Stopped
        Return New ServiceController(ServiceName).Status
    End Function

    ''' <summary>
    ''' Starts a system service in the current Windows installation.
    ''' </summary>
    ''' <param name="ServiceName">The name of the system service to start</param>
    ''' <returns>Whether the operation succeeded.</returns>
    ''' <remarks>
    ''' This function returns False when either no service going by the provided <paramref name="ServiceName"/> value exists in the current installation,
    ''' or if the service did not report a successful start operation after 15 seconds. Conversely, this function will return True if the system service
    ''' is already started.
    ''' </remarks>
    Public Function StartOnlineService(ServiceName As String) As Boolean
        DynaLog.LogMessage("Starting system service: " & ServiceName)
        If GetOnlineSystemServiceInformationByName(ServiceName) Is Nothing Then Return False

        DynaLog.LogMessage("Getting current service status...")
        Dim serviceStatus As ServiceControllerStatus = GetOnlineServiceStartStatus(ServiceName)
        If {ServiceControllerStatus.Running, ServiceControllerStatus.ContinuePending, ServiceControllerStatus.StartPending}.Contains(serviceStatus) Then Return True

        DynaLog.LogMessage("Starting service...")
        Using OnlineServiceController As New ServiceController(ServiceName)
            OnlineServiceController.Start()
            Try
                OnlineServiceController.WaitForStatus(ServiceControllerStatus.Running, New TimeSpan(0, 0, 0, 15, 0))
                Return True
            Catch serviceTimeoutEx As TimeoutException
                DynaLog.LogMessage("Service could not be started within 15 seconds.")
                Return False
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Starts a set of system services in the current Windows installation.
    ''' </summary>
    ''' <param name="ServiceNames">The names of the system services to start</param>
    ''' <returns>Whether there are more services that successfully started than those that did not start.</returns>
    ''' <remarks></remarks>
    Public Function StartOnlineService(ParamArray ServiceNames As String()) As Boolean
        Dim successfulServiceStarts As Integer = 0,
            failedServiceStarts As Integer = 0

        DynaLog.LogMessage("Starting " & ServiceNames.Count & " service(s)...")
        For Each ServiceName In ServiceNames
            If StartOnlineService(ServiceName) Then
                successfulServiceStarts += 1
            Else
                failedServiceStarts += 1
            End If
        Next

        Return successfulServiceStarts >= failedServiceStarts
    End Function

    ''' <summary>
    ''' Stops a system service in the current Windows installation.
    ''' </summary>
    ''' <param name="ServiceName">The name of the system service to stop</param>
    ''' <returns>Whether the operation succeeded.</returns>
    ''' <remarks>
    ''' This function returns False when either no service going by the provided <paramref name="ServiceName"/> value exists in the current installation,
    ''' or if the service did not report a successful stop operation after 15 seconds. Conversely, this function will return True if the system service
    ''' is already stopped.
    ''' </remarks>
    Public Function StopOnlineService(ServiceName As String) As Boolean
        DynaLog.LogMessage("Stopping system service: " & ServiceName)
        If GetOnlineSystemServiceInformationByName(ServiceName) Is Nothing Then Return False

        DynaLog.LogMessage("Getting current service status...")
        Dim serviceStatus As ServiceControllerStatus = GetOnlineServiceStartStatus(ServiceName)
        If Not {ServiceControllerStatus.Running, ServiceControllerStatus.ContinuePending, ServiceControllerStatus.StartPending}.Contains(serviceStatus) Then Return True

        DynaLog.LogMessage("Stopping service...")
        Using OnlineServiceController As New ServiceController(ServiceName)
            OnlineServiceController.Stop()
            Try
                OnlineServiceController.WaitForStatus(ServiceControllerStatus.Stopped, New TimeSpan(0, 0, 0, 15, 0))
                Return True
            Catch serviceTimeoutEx As TimeoutException
                DynaLog.LogMessage("Service could not be stopped within 15 seconds.")
                Return False
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Stops a set of system services in the current Windows installation.
    ''' </summary>
    ''' <param name="ServiceNames">The names of the system services to stop</param>
    ''' <returns>Whether there are more services that successfully stopped than those that did not stop.</returns>
    ''' <remarks></remarks>
    Public Function StopOnlineService(ParamArray ServiceNames As String()) As Boolean
        Dim successfulServiceStops As Integer = 0,
            failedServiceStops As Integer = 0

        DynaLog.LogMessage("Stopping " & ServiceNames.Count & " service(s)...")
        For Each ServiceName In ServiceNames
            If StopOnlineService(ServiceName) Then
                successfulServiceStops += 1
            Else
                failedServiceStops += 1
            End If
        Next

        Return successfulServiceStops >= failedServiceStops
    End Function

End Module