Imports System.Runtime.InteropServices

Namespace Utilities.NativeInterop.Network.Structures

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Structure SHARE_INFO_502

        <MarshalAs(UnmanagedType.LPWStr)>
        Public shi502_netname As String
        Public shi502_type As UInteger
        <MarshalAs(UnmanagedType.LPWStr)>
        Public shi502_remark As String
        Public shi502_permissions As UInteger
        Public shi502_max_uses As UInteger
        Public shi502_current_uses As UInteger
        <MarshalAs(UnmanagedType.LPWStr)>
        Public shi502_path As String
        <MarshalAs(UnmanagedType.LPWStr)>
        Public shi502_passwd As String
        Public shi502_reserved As UInteger
        Public shi502_security_descriptor As IntPtr

    End Structure

    Public Structure SHARE_INFO_1501
        Public shi1501_reserved As UInteger
        Public shi1501_security_descriptor As IntPtr
    End Structure

End Namespace