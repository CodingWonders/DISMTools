Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' Dynamic Logging (DynaLog) Logger Class
''' </summary>
''' <remarks>Primitive form in 0.6. Will be fully used in future versions</remarks>
Public Class DynaLog

    Public Shared Sub BeginLogging()
        LogMessage("DynaLog Logger has begun logging program operations...", False)
        LogMessage("--- Time Stamps are shown in UTC Time!!! ---", False)
    End Sub

    Public Shared Sub EndLogging()
        LogMessage("DynaLog Logger has stopped logging program operations...", False)
    End Sub

    Public Shared Sub LogMessage(message As String, Optional GetParentCaller As Boolean = True)
        Debug.WriteLine(message)
        Try
            ' DynaLog will NOT display logs for log file/folder creation - ONLY in debugger.
            If Not Directory.Exists(Application.StartupPath & "\logs") Then
                Directory.CreateDirectory(Application.StartupPath & "\logs")
            End If
            Dim Contents As String = ""
            If File.Exists(Application.StartupPath & "\logs\DT_DynaLog.log") Then
                Contents = File.ReadAllText(Application.StartupPath & "\logs\DT_DynaLog.log")
            End If
            Dim MessageLine As String = "[" & Date.UtcNow.ToString("MM/dd/yyyy HH:mm:ss") & "] " & "[" & New StackFrame(1).GetMethod().Name & If(GetParentCaller, " (" & New StackFrame(2).GetMethod().Name & ")", "") & "] " & message
            Contents &= If(Contents <> "", CrLf, "") & MessageLine
            File.WriteAllText(Application.StartupPath & "\logs\DT_DynaLog.log", Contents)
        Catch ex As Exception
            Debug.WriteLine("DynaLog logging could not log this operation. Error:" & CrLf & CrLf & ex.ToString())
        End Try
    End Sub

End Class
