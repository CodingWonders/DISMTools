Imports System.Runtime.InteropServices

Public Class NativeMethods

    Public Sub New()

    End Sub

    Public Const SC_CLOSE As Integer = &HF060
    Public Const MF_BYCOMMAND As Long = &H0L
    Public Const MF_GRAYED As Long = 1
    Public Const MF_ENABLED As Long = 0
    Public Const MF_DISABLED As Long = 2

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Shared Function GetSystemMenu(hWnd As IntPtr, bRevert As Boolean) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Shared Function EnableMenuItem(hMenu As IntPtr, uIDEnableItem As UInteger, uEnable As UInteger) As Boolean
    End Function

End Class
