Imports Microsoft.VisualBasic.ControlChars
Imports System.IO
Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.Text

Module WindowsServiceHelper

    NotInheritable Class NativeMethods

        Private Sub New()
        End Sub

        <DllImport("shlwapi.dll", BestFitMapping:=False, CharSet:=CharSet.Unicode, ExactSpelling:=True, SetLastError:=False, ThrowOnUnmappableChar:=True)>
        Shared Function SHLoadIndirectString(pszSource As String, pszOutBuf As StringBuilder, cchOutBuf As Integer, ppvReserved As IntPtr) As Integer
        End Function

    End Class

    Private PrivilegeConstantDictionary As New Dictionary(Of String, NTSecurityPrivilegeConstant)
    Private PrivilegeMappingDictionary As New Dictionary(Of String, String)

    Sub FillInConstants()
        PrivilegeConstantDictionary.Clear()
        PrivilegeMappingDictionary.Clear()
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

        For Each key As String In PrivilegeConstantDictionary.Keys
            Dim privilegeConstant As NTSecurityPrivilegeConstant = PrivilegeConstantDictionary(key)
            PrivilegeMappingDictionary.Add(privilegeConstant.ConstantNameText, key)
        Next
    End Sub

    Private Function ResolveIndirectString(source As String) As String
        Dim buffer As New StringBuilder(260)
        Dim hr As Integer = NativeMethods.SHLoadIndirectString(source, buffer, buffer.Capacity, IntPtr.Zero)
        If hr = 0 Then
            Return buffer.ToString()
        Else
            Return source
        End If
    End Function

    Function GetServiceList(MountPath As String) As List(Of WindowsService)
        ' For the required privileges a service may have, we have to fill in the constants first so that we don't have things like
        ' "SeUndockPrivilege", "SeShutdownPrivilege"; but rather "Remove computer from docking station", and so on... we want the
        ' friendly things.
        FillInConstants()
        Dim serviceList As New List(Of WindowsService)

        ' Time to load up a registry hive
        If RegistryHelper.LoadRegistryHive(Path.Combine(MountPath, "Windows", "system32", "config", "SYSTEM"), "HKLM\zSYS") = 0 Then
            Try
                ' First we need to grab the default control set of the target image
                Dim DefaultControlSet As Integer = RegistryHelper.GetDefaultControlSet("zSYS")
                If DefaultControlSet = -1 Then
                    Throw New Exception("Registry control set could not be obtained")
                End If
                ' We only document a maximum of 999 control sets. CurrentControlSet is not a thing in an offline system, as the registry
                ' subsystems guess the control set to use based on values in HKLM\SYSTEM\Select.
                Dim ServiceRk As RegistryKey = Registry.LocalMachine.OpenSubKey(String.Format("zSYS\ControlSet{0}\Services", DefaultControlSet.ToString().PadLeft(3, "0")), False)
                ' For some stupid reason, .NET keys are stored in HKLM\SYSTEM\ControlSet<nnn>\Services. GUID keys are also not allowed
                Dim ServiceNames() As String = ServiceRk.GetSubKeyNames().Where(Function(serviceName) Not serviceName.StartsWith(".NET", StringComparison.OrdinalIgnoreCase) AndAlso Not serviceName.StartsWith("{")).ToArray()
                ServiceRk.Close()

                ' Now we have to grab as much information as we can
                For Each ServiceName In ServiceNames
                    Dim serviceImagePath As String = "",
                        serviceEntryName As String = "",
                        serviceDisplayName As String = "",
                        serviceDescription As String = "",
                        serviceObjectName As String = "",
                        serviceStartType As WindowsService.ServiceStartType = WindowsService.ServiceStartType.Unknown,
                        serviceDelayedStart As Boolean = False,
                        serviceType As WindowsService.ServiceType = WindowsService.ServiceType.Unknown,
                        serviceErrorControl As WindowsService.ServiceErrorControl = WindowsService.ServiceErrorControl.Unknown,
                        serviceRequiredPrivilegesString() As String = New String() {}
                    Using ServiceInfoRk As RegistryKey = Registry.LocalMachine.OpenSubKey(String.Format("zSYS\ControlSet{0}\Services\{1}", DefaultControlSet.ToString().PadLeft(3, "0"), ServiceName), False)
                        ' We explicitly tell that we want to grab the raw data without env var expansion because REG_EXPAND_SZ values
                        ' are still string values, but with unexpanded environment variables. If the variable exists in the target system,
                        ' it will show that value.
                        serviceImagePath = ServiceInfoRk.GetValue("ImagePath", "", RegistryValueOptions.DoNotExpandEnvironmentNames)
                        If serviceImagePath = "" Then
                            ' This "service" is bogus
                            Continue For
                        End If
                        ' TODO: for devices, tweak the indirect string resolver to like INF files
                        ' TODO: failure/recovery actions need to be implemented, which will require us to understand binary data
                        ' TODO: relationships with services a service depends on or services that depend on a service need to be implemented

                        serviceEntryName = ServiceName
                        serviceDisplayName = ServiceInfoRk.GetValue("DisplayName", "")
                        If serviceDisplayName.StartsWith("@") Then
                            serviceDisplayName = ResolveIndirectString(serviceDisplayName)
                        End If
                        serviceDescription = ServiceInfoRk.GetValue("Description", "")
                        If serviceDescription.StartsWith("@") Then
                            serviceDescription = ResolveIndirectString(serviceDescription)
                        End If
                        serviceObjectName = ServiceInfoRk.GetValue("ObjectName", "")
                        serviceStartType = ServiceInfoRk.GetValue("Start", -1)
                        serviceDelayedStart = (ServiceInfoRk.GetValue("DelayedAutoStart", 0) = 1)
                        serviceType = ServiceInfoRk.GetValue("Type", -1)
                        serviceErrorControl = ServiceInfoRk.GetValue("ErrorControl", -1)
                        ' The required privileges property is a multi-value registry value, so we need an array
                        serviceRequiredPrivilegesString = ServiceInfoRk.GetValue("RequiredPrivileges", New String() {})

                        Dim serviceRequiredPrivilegeList As New List(Of NTSecurityPrivilegeConstant)

                        If serviceRequiredPrivilegesString.Count > 0 Then
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

                        serviceList.Add(New WindowsService(serviceEntryName,
                                                           serviceDisplayName,
                                                           serviceDescription,
                                                           serviceObjectName,
                                                           serviceImagePath,
                                                           serviceStartType,
                                                           serviceDelayedStart,
                                                           serviceType,
                                                           serviceErrorControl,
                                                           serviceRequiredPrivilegeList))
                    End Using
                Next
            Catch ex As Exception

            End Try

            ' Now we unload that hive
            RegistryHelper.UnloadRegistryHive("HKLM\zSYS")
        End If

        Return serviceList
    End Function

End Module