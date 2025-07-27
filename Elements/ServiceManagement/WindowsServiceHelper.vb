Imports Microsoft.VisualBasic.ControlChars

Module WindowsServiceHelper

    Private PrivilegeConstantDictionary As New Dictionary(Of String, NTSecurityPrivilegeConstant)
    Private PrivilegeMappingDictionary As New Dictionary(Of String, String)

    Sub FillInConstants()
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
                                            "Required to perform backup operations. This privilege causes the system to grant all read access control to any file, regardless of the\nRegSaveKeyEx functions. The following access rights are granted if this privilege is held:\n\n - READ_CONTROL\n - ACCESS_SYSTEM_SECURITY\n - FILE_GENERIC_READ\n - FILE_TRAVERSE"))
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
                                            "Required to create a primary token.\nYou cannot add this privilege to a user account with the " & Quote & "Create a token object" & Quote & " policy. Additionally, you cannot add this privilege to an owned process using Windows APIs. Windows Server 2003 and Windows XP with SP1 and earlier: Windows APIs can add this privilege to an owned process."))
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
                                            "Required to perform restore operations. This privilege causes the system to grant all write access control to any file, regardless of the ACL specified for the file. Any access request other than write is still evaluated with the ACL. Additionally, this privilege enables you to set any valid user or group SID as the owner of a file. This privilege is required by the\nRegLoadKey function. The following access rights are granted if this privilege is held:\n\n - WRITE_DAC\n - WRITE_OWNER\n - ACCESS_SYSTEM_SECURITY\n - FILE_GENERIC_WRITE\n - FILE_ADD_FILE\n - FILE_ADD_SUBDIRECTORY\n - DELETE"))
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

End Module