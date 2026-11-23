Imports System.Runtime.InteropServices

Namespace Utilities.NativeInterop.AdvancedAPI

    Public Class AdvApi

        ''' <summary>
        ''' The SDDL revision to pass to ConvertStringSecurityDescriptorToSecurityDescriptor
        ''' </summary>
        ''' <remarks>SDDL_REVISION_1. Reference: sddl.h</remarks>
        Public Const SDDL_REVISION_1 As UInteger = 1

        ''' <summary>
        ''' Converts a security descriptor string in SDDL format (for example, "D:(A;;0x1200a9;;;WD)(A;;FA;;;CO)") to a self-relative security descriptor
        ''' </summary>
        ''' <param name="StringSecurityDescriptor">The SDDL string to convert</param>
        ''' <param name="StringSDRevision">The SDDL revision. Use SDDL_REVISION_1</param>
        ''' <param name="SecurityDescriptor">The security descriptor. It must be freed with LocalFree</param>
        ''' <param name="SecurityDescriptorSize">The size of the security descriptor, in bytes</param>
        ''' <returns>Whether the function succeeded. Call Marshal.GetLastWin32Error for the error if it did not</returns>
        ''' <remarks>Reference: sddl.h</remarks>
        <DllImport("advapi32.dll", EntryPoint:="ConvertStringSecurityDescriptorToSecurityDescriptorW", CharSet:=CharSet.Unicode, ExactSpelling:=True, SetLastError:=True)>
        Public Shared Function ConvertStringSecurityDescriptorToSecurityDescriptor(StringSecurityDescriptor As String, StringSDRevision As UInteger, ByRef SecurityDescriptor As IntPtr, ByRef SecurityDescriptorSize As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        ''' <summary>
        ''' Determines whether the components of a security descriptor are valid
        ''' </summary>
        ''' <param name="pSecurityDescriptor">The security descriptor to check</param>
        ''' <returns>Whether the security descriptor is valid</returns>
        ''' <remarks>Reference: securitybaseapi.h</remarks>
        <DllImport("advapi32.dll", SetLastError:=True)>
        Public Shared Function IsValidSecurityDescriptor(pSecurityDescriptor As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

    End Class

End Namespace
