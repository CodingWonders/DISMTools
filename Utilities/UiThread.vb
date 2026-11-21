Imports System.Threading

''' <summary>
''' Helpers to run code on the thread that owns a control (normally the UI thread).
''' </summary>
''' <remarks>
''' Background threads must never create forms or controls, touch VB default form instances
''' (for example "MainForm.Something"), or change control properties directly. Doing so creates
''' windows on threads that do not pump messages, and the program freezes the next time Windows
''' broadcasts a user preference change (theme change, workstation lock...) through SystemEvents.
''' </remarks>
Public Module UiThread

    Private uiThreadId As Integer = -1

    ''' <summary>
    ''' Records the current thread as the UI thread. Call it from the UI thread at startup
    ''' </summary>
    Public Sub RegisterUiThread()
        uiThreadId = Thread.CurrentThread.ManagedThreadId
    End Sub

    ''' <summary>
    ''' Determines whether the calling thread is the UI thread recorded by RegisterUiThread
    ''' </summary>
    Public ReadOnly Property IsUiThread As Boolean
        Get
            Return uiThreadId = -1 OrElse Thread.CurrentThread.ManagedThreadId = uiThreadId
        End Get
    End Property

    ''' <summary>
    ''' The real (UI thread) instance of the main form. Use it instead of the MainForm default instance from background threads
    ''' </summary>
    Public ReadOnly Property MainUi As MainForm
        Get
            Return MainForm.Current
        End Get
    End Property

    Private Function CanMarshal(ctl As Control) As Boolean
        Return ctl IsNot Nothing AndAlso Not ctl.IsDisposed AndAlso Not ctl.Disposing
    End Function

    ''' <summary>
    ''' Runs an action on the thread that owns the control and waits for it to finish
    ''' </summary>
    Public Sub RunOnUi(ctl As Control, action As MethodInvoker)
        If action Is Nothing OrElse Not CanMarshal(ctl) Then Exit Sub
        If ctl.InvokeRequired Then
            Try
                ctl.Invoke(action)
            Catch ex As ObjectDisposedException
                ' The control went away while we were waiting
            Catch ex As InvalidOperationException
                ' The control's handle was destroyed while we were waiting
                If ctl.IsHandleCreated Then Throw
            End Try
        Else
            action()
        End If
    End Sub

    ''' <summary>
    ''' Runs a function on the thread that owns the control, waits for it to finish, and returns its result
    ''' </summary>
    Public Function RunOnUi(Of T)(ctl As Control, func As Func(Of T)) As T
        If func Is Nothing OrElse Not CanMarshal(ctl) Then Return Nothing
        If ctl.InvokeRequired Then
            Try
                Return DirectCast(ctl.Invoke(func), T)
            Catch ex As ObjectDisposedException
                Return Nothing
            Catch ex As InvalidOperationException
                If ctl.IsHandleCreated Then Throw
                Return Nothing
            End Try
        Else
            Return func()
        End If
    End Function

    ''' <summary>
    ''' Queues an action on the thread that owns the control without waiting for it. If the caller already is that thread, the action runs immediately
    ''' </summary>
    Public Sub PostToUi(ctl As Control, action As MethodInvoker)
        If action Is Nothing OrElse Not CanMarshal(ctl) Then Exit Sub
        If ctl.InvokeRequired Then
            Try
                ctl.BeginInvoke(action)
            Catch ex As ObjectDisposedException
            Catch ex As InvalidOperationException
            End Try
        Else
            action()
        End If
    End Sub

    ''' <summary>
    ''' Shows a message box on the UI thread, owned by the specified form when possible
    ''' </summary>
    Public Function MsgBoxOnUi(owner As Control, prompt As String, Optional buttons As MsgBoxStyle = MsgBoxStyle.OkOnly, Optional title As String = Nothing) As MsgBoxResult
        If Not CanMarshal(owner) Then owner = MainUi
        If Not CanMarshal(owner) Then Return MsgBox(prompt, buttons, title)
        Return RunOnUi(Of MsgBoxResult)(owner, Function() MsgBox(prompt, buttons, title))
    End Function

End Module
