Imports System.Runtime.InteropServices
Imports System.Windows.Forms.Control

Public Class WindowHelper

    Friend NotInheritable Class NativeMethods

        Public Sub New()
        End Sub

        <DllImport("dwmapi.dll")> _
        Public Shared Function DwmSetWindowAttribute(ByVal hWnd As IntPtr, ByVal attr As Integer, ByRef attrValue As Integer, ByVal attrSize As Integer) As Integer
        End Function
    End Class

    Private Const DWMWA_USE_IMMERSIVE_DARK_MODE As Integer = 20
    Private Const WS_EX_COMPOSITED As Integer = &H2000000
    Private Const GWL_EXSTYLE As Integer = -20

    Private Const DARKMODE_MINMAJOR As Integer = 10
    Private Const DARKMODE_MINMINOR As Integer = 0
    Private Const DARKMODE_MINBUILD As Integer = 18362

    Public Shared Sub ToggleDarkTitleBar(ByVal hwnd As IntPtr, ByVal darkMode As Boolean)
        Dim attribute As Integer = 0
        If darkMode Then attribute = 1
        If Not IsWindowsVersionOrGreater(DARKMODE_MINMAJOR, DARKMODE_MINMINOR, DARKMODE_MINBUILD) Then Exit Sub
        Dim result As Integer = NativeMethods.DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, attribute, 4)
    End Sub

    Private Shared Function IsWindowsVersionOrGreater(ByVal majorVersion As Integer, ByVal minorVersion As Integer, ByVal buildNumber As Integer) As Boolean
        Dim version As Version = Environment.OSVersion.Version
        Return version.Major > majorVersion OrElse _
               (version.Major = majorVersion AndAlso version.Minor > minorVersion) OrElse _
               (version.Major = majorVersion AndAlso version.Minor = minorVersion AndAlso version.Build >= buildNumber)
    End Function

    Public Shared Function ScaleLogical(ByVal px As Integer) As Integer
        Dim dx As Single
        Dim ctrl As New Control()
        Dim g As Graphics = ctrl.CreateGraphics()

        Try
            dx = g.DpiX
        Finally
            g.Dispose()
        End Try

        Return CInt(px * (dx / 96.0))
    End Function

    Public Shared Function ScalePositionLogical(ByVal posX As Integer, ByVal posY As Integer) As Point
        Return New Point(ScaleLogical(posX), ScaleLogical(posY))
    End Function

    Public Shared Function ScaleSizeLogical(ByVal width As Integer, ByVal height As Integer) As Size
        Return New Size(ScaleLogical(width), ScaleLogical(height))
    End Function

    Public Shared Function GetSystemDpi() As Single
        Return CSng(ScaleLogical(100))
    End Function

    Public Shared Sub DisplayToolTip(ByVal tooltipSender As Object, ByVal toolTipMessage As String)
        If TypeOf (tooltipSender) Is Control Then
            Dim displayedToolTip As New ToolTip()
            displayedToolTip.SetToolTip(tooltipSender, toolTipMessage)
        End If
    End Sub

End Class
