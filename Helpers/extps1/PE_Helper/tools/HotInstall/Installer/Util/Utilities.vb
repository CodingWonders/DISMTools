Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.IO

Public Class Utilities

    Friend NotInheritable Class NativeMethods

        Private Sub New()
        End Sub

        <DllImport("dwmapi.dll")>
        Shared Function DwmSetWindowAttribute(hwnd As IntPtr, attr As Integer, ByRef attrValue As Integer, attrSize As Integer) As Integer
        End Function

    End Class

    Const DWMWA_USE_IMMERSIVE_DARK_MODE As Integer = 20
    Const WS_EX_COMPOSITED As Integer = &H2000000
    Const GWL_EXSTYLE As Integer = -20

    Shared Sub EnableDarkTitleBar(hwnd As IntPtr, isDarkMode As Boolean)
        Dim attribute As Integer = If(isDarkMode, 1, 0)
        Dim result As Integer = NativeMethods.DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, attribute, 4)
    End Sub

    Function GetWindowHandle(ctrl As Control) As IntPtr
        If ctrl Is Nothing Then Return IntPtr.Zero
        Return ctrl.Handle
    End Function

    Shared Function IsWindowsVersionOrGreater(majorVersion As Integer, minorVersion As Integer, buildNumber As Integer) As Boolean
        Dim version = Environment.OSVersion.Version
        Return version.Major > majorVersion OrElse (version.Major = majorVersion AndAlso version.Minor > minorVersion) OrElse (version.Major = majorVersion AndAlso version.Minor = minorVersion AndAlso version.Build >= buildNumber)
    End Function

    Public Shared Function IsSystemInDarkMode() As Boolean
        Try
            Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize")
            Dim ColorMode As Integer = ColorModeRk.GetValue("AppsUseLightTheme")
            ColorModeRk.Close()
            Return (ColorMode = 0)
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

End Class
