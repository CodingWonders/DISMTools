Imports System.Runtime.InteropServices
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

    End Class

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

End Module
