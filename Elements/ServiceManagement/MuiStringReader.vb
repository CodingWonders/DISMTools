Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Text

Public Class MuiStringReader

    Private Const LOAD_LIBRARY_AS_DATAFILE As UInteger = 2
    Private Const LOAD_LIBRARY_AS_IMAGE_RESOURCE As UInteger = 32

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function LoadLibraryEx(lpFileName As String, hFile As IntPtr, dwFlags As UInteger) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function FreeLibrary(hModule As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function LoadString(hInstance As IntPtr, uID As UInteger, lpBuffer As StringBuilder, cchBufferMax As Integer) As Integer
    End Function

    Public Shared Function GetString(muiPath As String, stringId As UInteger) As String
        Dim mdl As IntPtr = LoadLibraryEx(muiPath, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE Or LOAD_LIBRARY_AS_IMAGE_RESOURCE)
        If mdl = IntPtr.Zero Then Throw New Win32Exception(Marshal.GetLastWin32Error())

        Try
            Dim buffer As New StringBuilder(1024)
            Dim copied As Integer = LoadString(mdl, stringId, buffer, buffer.Capacity)

            If copied = 0 Then
                ' Possible RESOURCE_NOT_FOUND error
                Throw New Win32Exception(Marshal.GetLastWin32Error())
            End If

            Return buffer.ToString(0, copied)
        Finally
            FreeLibrary(mdl)
        End Try
    End Function

End Class
