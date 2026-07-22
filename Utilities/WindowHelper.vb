Imports System.Runtime.InteropServices
Imports System.Windows.Forms.Control

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

        <DllImport("user32.dll")>
        Public Shared Function SetForegroundWindow(hWnd As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll")>
        Public Shared Function GetForegroundWindow() As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Public Shared Function SetWindowPos(hWnd As IntPtr,
                                            hWndInsertAfter As IntPtr,
                                            x As Integer,
                                            y As Integer,
                                            cx As Integer,
                                            cy As Integer,
                                            flags As UInteger) As Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Public Shared Function GetWindowLong(hWnd As IntPtr, index As Integer) As Integer
        End Function

        <DllImport("dwmapi.dll")>
        Public Shared Function DwmSetWindowAttribute(hwnd As IntPtr, attr As Integer, ByRef attrValue As Integer, attrSize As Integer) As Integer
        End Function

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
        Public Structure LVITEM
            Public mask As Integer
            Public iItem As Integer
            Public iSubItem As Integer
            Public state As Integer
            Public stateMask As Integer
            <MarshalAs(UnmanagedType.LPTStr)>
            Public pszText As String
            Public cchTextMax As Integer
            Public iImage As Integer
            Public lParam As IntPtr
            Public iIndent As Integer
            Public iGroupId As Integer
            Public cColumns As Integer
            Public puColumns As IntPtr
        End Structure

        <DllImport("user32.dll", EntryPoint:="SendMessage", CharSet:=CharSet.Auto)>
        Public Shared Function SendMessageLVItem(hWnd As IntPtr, msg As Integer, wParam As Integer, ByRef lvi As LVITEM) As IntPtr
        End Function
    End Class

    ' USER32 Constants
    Const SC_CLOSE As Integer = &HF060
    Const MF_BYCOMMAND As Long = &H0L
    Const MF_ENABLED As Long = 0
    Const MF_GRAYED As Long = 1
    Const MF_DISABLED As Long = 2

    ' DWMAPI Constants
    Const DWMWA_USE_IMMERSIVE_DARK_MODE As Integer = 20
    Const WS_EX_COMPOSITED As Integer = &H2000000
    Const GWL_EXSTYLE As Integer = -20

    Private Shared ReadOnly HWND_TOPMOST As New IntPtr(-1)
    Private Shared ReadOnly HWND_NOTOPMOST As New IntPtr(-2)
    Private Const SWP_NOSIZE As UInteger = &H1UI
    Private Const SWP_NOMOVE As UInteger = &H2UI
    Private Const SWP_SHOWWINDOW As UInteger = &H40UI
    Private Const WS_EX_TOPMOST As Integer = &H8

    Private Const LVM_FIRST As Integer = &H1000
    Private Const LVM_SETITEMSTATE As Integer = LVM_FIRST + 43

    Private Const LVIS_UNCHECKED As Integer = &H1000
    Private Const LVIS_CHECKED As Integer = &H2000
    Private Const LVIS_STATEIMAGEMASK As Integer = &H3000

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

    Public Shared Function GetWindowHandle(ctrl As Control) As IntPtr
        Return ctrl.Handle
    End Function

    Public Shared Function RequestForegroundWindow(wndHandle As IntPtr) As Boolean
        If wndHandle.Equals(IntPtr.Zero) Then Return False
        Return NativeMethods.SetForegroundWindow(wndHandle)
    End Function

    Public Shared Function RaiseWindowAndRestoreNormalZOrder(wndHandle As IntPtr,
                                                              ByRef normalZOrderRestored As Boolean) As Boolean
        normalZOrderRestored = False
        If wndHandle.Equals(IntPtr.Zero) Then Return False

        Dim flags As UInteger = SWP_NOMOVE Or SWP_NOSIZE Or SWP_SHOWWINDOW
        Dim raisedAboveOtherWindows As Boolean = NativeMethods.SetWindowPos(wndHandle,
                                                                            HWND_TOPMOST,
                                                                            0,
                                                                            0,
                                                                            0,
                                                                            0,
                                                                            flags)
        normalZOrderRestored = NativeMethods.SetWindowPos(wndHandle,
                                                           HWND_NOTOPMOST,
                                                           0,
                                                           0,
                                                           0,
                                                           0,
                                                           flags)
        Return raisedAboveOtherWindows
    End Function

    Public Shared Function IsForegroundWindow(wndHandle As IntPtr) As Boolean
        Return Not wndHandle.Equals(IntPtr.Zero) AndAlso NativeMethods.GetForegroundWindow().Equals(wndHandle)
    End Function

    Public Shared Function IsTopMostWindow(wndHandle As IntPtr) As Boolean
        If wndHandle.Equals(IntPtr.Zero) Then Return False
        Return (NativeMethods.GetWindowLong(wndHandle, GWL_EXSTYLE) And WS_EX_TOPMOST) = WS_EX_TOPMOST
    End Function

    Const DARKMODE_MINMAJOR As Integer = 10
    Const DARKMODE_MINMINOR As Integer = 0
    Const DARKMODE_MINBUILD As Integer = 18362

    Public Shared Sub ToggleDarkTitleBar(hwnd As IntPtr, darkMode As Boolean)
        Dim attribute As Integer = If(darkMode, 1, 0)
        If Not IsWindowsVersionOrGreater(DARKMODE_MINMAJOR, DARKMODE_MINMINOR, DARKMODE_MINBUILD) Then Exit Sub
        Dim result As Integer = NativeMethods.DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, attribute, 4)
    End Sub

    Private Shared Function IsWindowsVersionOrGreater(majorVersion As Integer, minorVersion As Integer, buildNumber As Integer) As Boolean
        Dim version = Environment.OSVersion.Version
        Return version.Major > majorVersion OrElse (version.Major = majorVersion AndAlso version.Minor > minorVersion) OrElse (version.Major = majorVersion AndAlso version.Minor = minorVersion AndAlso version.Build >= buildNumber)
    End Function

    Public Shared Function ScaleLogical(px As Integer) As Integer
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

    Public Shared Function ScalePositionLogical(posX As Integer, posY As Integer) As Point
        Return New Point(ScaleLogical(posX), ScaleLogical(posY))
    End Function

    Public Shared Function ScaleSizeLogical(width As Integer, height As Integer) As Size
        Return New Size(ScaleLogical(width), ScaleLogical(height))
    End Function

    Public Shared Function GetSystemDpi() As Single
        Return CSng(ScaleLogical(100))
    End Function

    Public Shared Sub DisplayToolTip(tooltipSender As Object, toolTipMessage As String)
        If TypeOf (tooltipSender) Is Control Then
            Dim displayedToolTip As New ToolTip()
            displayedToolTip.SetToolTip(tooltipSender, toolTipMessage)
        End If
    End Sub

    Private Shared notificationBalloon As NotifyIcon

    Public Shared Sub DisplayNotificationBalloon(balloonIcon As ToolTipIcon, balloonCaption As String, balloonMessage As String)
        notificationBalloon = New NotifyIcon() With {
            .BalloonTipIcon = balloonIcon,
            .Icon = CType(New System.ComponentModel.ComponentResourceManager(GetType(MainForm)).GetObject("$this.Icon"), System.Drawing.Icon),
            .Text = "DISMTools",
            .BalloonTipTitle = balloonCaption,
            .BalloonTipText = balloonMessage,
            .Visible = True
        }
        notificationBalloon.ShowBalloonTip(5000)
        notificationBalloon.Visible = False
    End Sub

    Public Shared Sub CheckAllItems(lv As ListView)
        SetLVItemState(lv, -1, LVIS_STATEIMAGEMASK, LVIS_CHECKED)
    End Sub

    Public Shared Sub UncheckAllItems(lv As ListView)
        SetLVItemState(lv, -1, LVIS_STATEIMAGEMASK, LVIS_UNCHECKED)
    End Sub

    Private Shared Sub SetLVItemState(lv As ListView, itemIndex As Integer, mask As Integer, value As Integer)
        Dim lvi As New NativeMethods.LVITEM()
        lvi.stateMask = mask
        lvi.state = value
        NativeMethods.SendMessageLVItem(lv.Handle, LVM_SETITEMSTATE, itemIndex, lvi)
    End Sub

End Class
