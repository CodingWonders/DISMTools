Imports System
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Class SearchBox
    Inherits TextBox

    Private Const EM_SETCUEBANNER As Integer = &H1501
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As String) As Int32
    End Function

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        If Not String.IsNullOrEmpty(cueBanner) Then UpdateCueBanner()
    End Sub

    Private m_CueBanner As String
    Public Property cueBanner As String
        Get
            Return m_CueBanner
        End Get
        Set(value As String)
            m_CueBanner = value
            UpdateCueBanner()
        End Set
    End Property

    Private Sub UpdateCueBanner()
        SendMessage(Handle, EM_SETCUEBANNER, 0, cueBanner)
    End Sub

End Class
