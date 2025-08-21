Imports System.Runtime.InteropServices
Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement
Imports System.DirectoryServices.ActiveDirectory.Domain

Module DomainServicesModule

    ''' <summary>
    ''' Status code for Windows Network API functions
    ''' </summary>
    ''' <remarks>References: lmerr.h; winerror.h</remarks>
    Private Enum NetApiStatus
        ''' <summary>
        ''' The operation completed successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Success = 0
        ''' <summary>
        ''' Could not find domain controller for this domain.
        ''' </summary>
        ''' <remarks></remarks>
        DCNotFound = 2453
    End Enum

    Private NotInheritable Class NativeMethods

        <DllImport("netapi32.dll")>
        Public Shared Function NetApiBufferFree(Buffer As IntPtr) As NetApiStatus
        End Function

        <DllImport("netapi32.dll", CharSet:=CharSet.Unicode)>
        Public Shared Function NetGetDCName(serverName As String, domainName As String, ByRef buffer As IntPtr) As NetApiStatus
        End Function

        <DllImport("netapi32.dll", CharSet:=CharSet.Unicode)>
        Public Shared Function NetWkstaGetInfo(<MarshalAs(UnmanagedType.LPWStr)> serverName As String, level As Integer, ByRef bufPtr As IntPtr) As Integer
        End Function

    End Class

    ''' <summary>
    ''' Structure for workstation environment information obtained by NetWkstaGetInfo
    ''' </summary>
    ''' <remarks>Headers: lmwksta.h</remarks>
    <StructLayout(LayoutKind.Sequential)>
    Private Structure WKSTA_INFO_100
        ''' <summary>
        ''' Information level for platform-specific info
        ''' </summary>
        ''' <remarks>
        ''' 300: MS-DOS;
        ''' 400: OS/2;
        ''' 500: Windows NT;
        ''' 600: OSF/1;
        ''' 700: VMS
        ''' </remarks>
        Public wki100_platform_id As Integer
        ''' <summary>
        ''' The name of the local computer
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.LPWStr)>
        Public wki100_computer_name As String
        ''' <summary>
        ''' The name of the domain to which the PC belongs
        ''' </summary>
        ''' <remarks></remarks>
        <MarshalAs(UnmanagedType.LPWStr)>
        Public wki100_langroup As String
        ''' <summary>
        ''' The major part of the version of running OS
        ''' </summary>
        ''' <remarks></remarks>
        Public wki100_ver_major As Integer
        ''' <summary>
        ''' The minor part of the version of running OS
        ''' </summary>
        ''' <remarks></remarks>
        Public wki100_ver_minor As Integer
    End Structure

    ''' <summary>
    ''' Gets the information about the name of the domain the system has joined
    ''' </summary>
    ''' <returns>The domain name as a pointer</returns>
    ''' <remarks>If the device is not joined to a domain, the domain name pointer will be IntPtr.Zero</remarks>
    Private Function GetDomainNameInformation() As IntPtr
        Dim domainInfo As IntPtr = IntPtr.Zero
        Dim domain As IntPtr = IntPtr.Zero

        DynaLog.LogMessage("Getting information from Win32 network APIs...")

        Try
            Dim result As NetApiStatus = NativeMethods.NetGetDCName(Nothing, Nothing, domainInfo)
            DynaLog.LogMessage("Result after calling API: " & result)
            If result = NetApiStatus.Success Then
                domain = domainInfo
            End If
        Finally
            DynaLog.LogMessage("Freeing local resources...")
            NativeMethods.NetApiBufferFree(domainInfo)
        End Try

        DynaLog.LogMessage("Domain Info Pointer value: " & domain.ToInt64())
        Return domain

    End Function

    ''' <summary>
    ''' Determines whether a device is joined to a domain powered by Active Directory Domain Services
    ''' </summary>
    ''' <returns>True if the device has joined a domain; false otherwise</returns>
    ''' <remarks>The function checks if resulting IntPtr from GetDomainNameInformation is not IntPtr.Zero</remarks>
    Public Function DSIsInDomain() As Boolean
        DynaLog.LogMessage("Getting domain information...")
        Dim domain As IntPtr = GetDomainNameInformation()
        DynaLog.LogMessage("Is part of a domain? " & If(domain <> IntPtr.Zero, "Yes", "No"))
        Return (domain <> IntPtr.Zero)
    End Function

    ''' <summary>
    ''' Gets the name of the domain
    ''' </summary>
    ''' <returns>The name of the domain</returns>
    ''' <remarks>The function checks if the device is part of a domain. If it isn't, it will return an empty string</remarks>
    Public Function DSGetDomainName() As String
        DynaLog.LogMessage("Preparing to get the name of the domain...")
        Dim domainName As String = ""
        DynaLog.LogMessage("Checking if device is part of a domain...")
        If DSIsInDomain() Then
            DynaLog.LogMessage("This device is part of a domain. Grabbing name...")
            domainName = GetComputerDomain().Name       ' Get it from AD DS .NET API
        End If
        DynaLog.LogMessage("Domain name: " & ControlChars.Quote & domainName & ControlChars.Quote & ". If it's empty, it could be because the device is not part of a domain.")
        Return domainName
    End Function

    ''' <summary>
    ''' Gets the NetBIOS name of the workstation/Primary Domain Controller (PDC)
    ''' </summary>
    ''' <returns>The NetBIOS name</returns>
    ''' <remarks></remarks>
    Public Function DSGetDomainControllerNetBIOSName() As String
        Dim netbiosName As String = ""
        DynaLog.LogMessage("Preparing to get the NetBIOS name of the DC...")
        Dim pBuffer As IntPtr = IntPtr.Zero
        Dim result As Integer = NativeMethods.NetWkstaGetInfo(Nothing, 100, pBuffer)
        DynaLog.LogMessage("NETAPI::NetWkstaGetInfo exit code: " & result)
        If result = 0 Then
            DynaLog.LogMessage("NETAPI::NetWkstaGetInfo succeeded! Parsing pointer to struct...")
            Try
                netbiosName = Marshal.PtrToStructure(Of WKSTA_INFO_100)(pBuffer).wki100_langroup
            Catch ex As Exception
                DynaLog.LogMessage("Pointer could not be parsed to struct: " & ex.Message)
            End Try
            NativeMethods.NetApiBufferFree(pBuffer)
        Else
            DynaLog.LogMessage("NETAPI::NetWkstaGetInfo failed.")
        End If
        Return netbiosName
    End Function

    ''' <summary>
    ''' Gets a mapping between organizational units in a domain and the users in each OU
    ''' </summary>
    ''' <param name="dsDomainDnsName">The name of the domain in DNS (eg: dismtools.local)</param>
    ''' <param name="dsDomainControllerNetBios">The NetBIOS name of the domain controller (eg: DISMTOOLS-DC)</param>
    ''' <returns>The mapping between organizational units and users in each OU</returns>
    ''' <remarks>
    ''' Unimplemented in 0.7, to be implemented in a future release of DISMTools. Keys are the organizational unit names,
    ''' and values are all the users in each of the OUs. To access the value, use the organizational unit name as the key.
    ''' Do note that this ONLY works with organizational units in a domain, and NOT sites.
    ''' </remarks>
    Public Function DSMapOrganizationalUnitsAndUsers(dsDomainDnsName As String, dsDomainControllerNetBios As String) As Dictionary(Of String, List(Of Principal))
        Dim principalMappings As New Dictionary(Of String, List(Of Principal))
        Dim orgUnitPaths As New List(Of String), orgUnits As New List(Of String)

        Dim ldapPath As String = GetLdapPathFromDnsName(dsDomainDnsName)

        Dim startingPoint As DirectoryEntry = New DirectoryEntry(String.Format("LDAP://{0}", ldapPath))
        Dim searcher As DirectorySearcher = New DirectorySearcher(startingPoint)
        searcher.Filter = "(objectCategory=organizationalUnit)"

        For Each result As SearchResult In searcher.FindAll()
            orgUnitPaths.Add(result.Path)
            orgUnits.Add(result.GetDirectoryEntry().Properties("ou").Value.ToString())
        Next

        searcher.Dispose()      ' We need to call this due to some implementation quirks in the directory searcher

        For i = 0 To orgUnits.Count
            Dim allUsers As New List(Of String)
            Dim ctx As PrincipalContext = New PrincipalContext(ContextType.Domain, dsDomainControllerNetBios, orgUnitPaths(i).Replace("LDAP://", ""))
            Dim qbeUser As UserPrincipal = New UserPrincipal(ctx)

            Dim srch As PrincipalSearcher = New PrincipalSearcher(qbeUser)
            Dim principals As New List(Of Principal)
            principals = srch.FindAll().ToList()

            If principals.Count > 0 Then
                principalMappings.Add(orgUnits(i), principals)
            End If
        Next

        Return principalMappings
    End Function

    ''' <summary>
    ''' Gets a Lightweight Directory Access Protocol (LDAP) representation of the domain name in DNS records.
    ''' </summary>
    ''' <param name="dnsName">The name of the domain in DNS records</param>
    ''' <returns>A LDAP representation</returns>
    ''' <remarks>LDAP representation of, for example, dismtools.local: DC=dismtools, DC=local</remarks>
    Private Function GetLdapPathFromDnsName(dnsName As String) As String
        Dim pathParts() As String = dnsName.Split(".")

        For i = 0 To pathParts.Length
            pathParts(i) = String.Format("DC={0}", pathParts(i))
        Next

        Return String.Join(",", pathParts)
    End Function

End Module
