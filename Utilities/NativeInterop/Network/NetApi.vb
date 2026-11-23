Imports System.Runtime.InteropServices
Imports DISMTools.Utilities.NativeInterop.Network.Enums
Imports DISMTools.Utilities.NativeInterop.Network.Structures
Imports DISMTools.Utilities.NativeInterop.AdvancedAPI

Namespace Utilities.NativeInterop.Network

    Public Class NetApi

        ''' <summary>
        ''' Shares a server resource. With level 502, the share's permissions can be set through shi502_security_descriptor
        ''' </summary>
        ''' <param name="servername">The server to create the share on. Nothing for the local computer</param>
        ''' <param name="level">The information level of buf. Use 502 for SHARE_INFO_502</param>
        ''' <param name="buf">The share information</param>
        ''' <param name="parm_err">If the function returns InvalidParameter, the index of the first member that caused the error</param>
        ''' <returns>The status of the operation</returns>
        ''' <remarks>Reference: lmshare.h</remarks>
        <DllImport("netapi32.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
        Public Shared Function NetShareAdd(servername As String, level As UInteger, ByRef buf As SHARE_INFO_502, ByRef parm_err As UInteger) As NetApiStatus
        End Function

        ''' <summary>
        ''' Sets the parameters of an existing share. With level 1501, only the share's security descriptor is changed
        ''' </summary>
        ''' <param name="servername">The server the share is on. Nothing for the local computer</param>
        ''' <param name="netname">The name of the share</param>
        ''' <param name="level">The information level of buf. Use 1501 for SHARE_INFO_1501</param>
        ''' <param name="buf">The share information</param>
        ''' <param name="parm_err">If the function returns InvalidParameter, the index of the first member that caused the error</param>
        ''' <returns>The status of the operation</returns>
        ''' <remarks>Reference: lmshare.h</remarks>
        <DllImport("netapi32.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
        Public Shared Function NetShareSetInfo(servername As String, netname As String, level As UInteger, ByRef buf As SHARE_INFO_1501, ByRef parm_err As UInteger) As NetApiStatus
        End Function

        ''' <summary>
        ''' Gets information about a share
        ''' </summary>
        ''' <param name="servername">The server the share is on. Nothing for the local computer</param>
        ''' <param name="netname">The name of the share</param>
        ''' <param name="level">The information level of the data returned in bufptr (for example, 502 or 1501)</param>
        ''' <param name="bufptr">A buffer with the share information. It must be freed with NetApiBufferFree</param>
        ''' <returns>The status of the operation</returns>
        ''' <remarks>Reference: lmshare.h</remarks>
        <DllImport("netapi32.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
        Public Shared Function NetShareGetInfo(servername As String, netname As String, level As UInteger, ByRef bufptr As IntPtr) As NetApiStatus
        End Function

        ''' <summary>
        ''' Deletes a share. Open connections to it are closed
        ''' </summary>
        ''' <param name="servername">The server the share is on. Nothing for the local computer</param>
        ''' <param name="netname">The name of the share</param>
        ''' <param name="reserved">Reserved. Must be 0</param>
        ''' <returns>The status of the operation</returns>
        ''' <remarks>Reference: lmshare.h</remarks>
        <DllImport("netapi32.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
        Public Shared Function NetShareDel(servername As String, netname As String, reserved As UInteger) As NetApiStatus
        End Function

        ''' <summary>
        ''' Frees a buffer allocated by a Network API function, such as NetShareGetInfo
        ''' </summary>
        ''' <param name="Buffer">The buffer to free</param>
        ''' <returns>The status of the operation</returns>
        ''' <remarks>Reference: lmapibuf.h</remarks>
        <DllImport("netapi32.dll")>
        Public Shared Function NetApiBufferFree(Buffer As IntPtr) As NetApiStatus
        End Function

    End Class

End Namespace
