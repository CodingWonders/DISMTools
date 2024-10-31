Imports System.Windows.Shell
Imports Microsoft.WindowsAPICodePack.Taskbar

Public Class TaskbarHelper

    Public Shared Sub SetIndicatorState(value As Integer, state As TaskbarItemProgressState, handle As IntPtr)
        Try
            TaskbarManager.Instance.SetProgressValue(value, 100, handle)
            TaskbarManager.Instance.SetProgressState(state, handle)
        Catch ex As Exception
            Debug.WriteLine("Could not set TBI. " & ex.Message)
        End Try
    End Sub

End Class
