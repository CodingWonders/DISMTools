Imports System.Runtime.InteropServices

Public Class WindowHelper

    Friend NotInheritable Class NativeMethods

        Public Sub New()

        End Sub

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Shared Function GetSystemMenu(hWnd As IntPtr, bRevert As Boolean) As IntPtr
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Shared Function EnableMenuItem(hMenu As IntPtr, uIDEnableItem As UInteger, uEnable As UInteger) As Boolean
        End Function
    End Class

    Const SC_CLOSE As Integer = &HF060
    Const MF_BYCOMMAND As Long = &H0L
    Const MF_ENABLED As Long = 0
    Const MF_GRAYED As Long = 1
    Const MF_DISABLED As Long = 2

    Public Shared Sub DisableCloseCapability(wndHandle As IntPtr)
        If Not wndHandle.Equals(IntPtr.Zero) Then
            Dim menu As IntPtr = NativeMethods.GetSystemMenu(wndHandle, False)
            If Not menu.Equals(IntPtr.Zero) Then
                NativeMethods.EnableMenuItem(menu, SC_CLOSE, MF_BYCOMMAND Or MF_GRAYED Or MF_DISABLED)
            End If
        End If
    End Sub

    Public Shared Sub EnableCloseCapability(wndHandle As IntPtr)
        If Not wndHandle.Equals(IntPtr.Zero) Then
            Dim menu As IntPtr = NativeMethods.GetSystemMenu(wndHandle, False)
            If Not menu.Equals(IntPtr.Zero) Then
                NativeMethods.EnableMenuItem(menu, SC_CLOSE, MF_BYCOMMAND Or MF_ENABLED)
            End If
        End If
    End Sub

End Class
