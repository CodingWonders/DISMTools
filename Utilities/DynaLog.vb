Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' Dynamic Logging (DynaLog) Logger Class
''' </summary>
''' <remarks>Primitive form in 0.6. Will be fully used in future versions</remarks>
Public Class DynaLog

    Public Shared Sub CheckLogAge()
        LogMessage("Checking existing logs...", False)
        If File.Exists(Application.StartupPath & "\logs\DT_DynaLog.log") Then
            LogMessage("Log File Found. Checking log file creation date...", False)
            Try
                Dim CreationDate As DateTime = File.GetCreationTimeUtc(Application.StartupPath & "\logs\DT_DynaLog.log")
                If CreationDate < DateTime.UtcNow.AddDays(-14) Then
                    LogMessage("Current log file is more than 2 weeks old. Archiving...", False)
                    Dim ArchivedFileName As String = "DT_DynaLog_" & DateTime.UtcNow.ToString("yyMMdd-HHmm") & ".old"
                    Try
                        File.Move(Application.StartupPath & "\logs\DT_DynaLog.log", Application.StartupPath & "\logs\" & ArchivedFileName)
                        LogMessage("The old log file has been archived. New messages will be shown in a new log file", False)       ' A blank sheet of... logs?
                    Catch ex As Exception
                        LogMessage("Could not archive log. Error info:" & CrLf & CrLf & ex.ToString(), False)
                    End Try
                Else
                    ' Don't archive
                End If
            Catch ex As Exception
                LogMessage("Could not check log file age. Error info:" & CrLf & CrLf & ex.ToString(), False)
            End Try
        Else
            ' Don't do anything
        End If
    End Sub

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
