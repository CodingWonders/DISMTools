Imports System.Runtime.InteropServices

Namespace Utilities.NativeInterop.Kernel

    Public Class Kernel32

        ''' <summary>
        ''' Frees a buffer allocated by ConvertStringSecurityDescriptorToSecurityDescriptor
        ''' </summary>
        ''' <param name="hMem">The buffer to free</param>
        ''' <returns>IntPtr.Zero if the function succeeded; the handle passed in if it did not</returns>
        ''' <remarks>Exported by kernel32, but kept here since it frees AdvApi buffers. Reference: winbase.h</remarks>
        <DllImport("kernel32.dll", SetLastError:=True)>
        Public Shared Function LocalFree(hMem As IntPtr) As IntPtr
        End Function

    End Class

End Namespace