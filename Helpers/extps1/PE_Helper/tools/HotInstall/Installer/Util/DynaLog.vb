Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Globalization

''' <summary>
''' Dynamic Logging (DynaLog) Logger Class
''' </summary>
''' <remarks></remarks>
Public Class DynaLog

    ''' <summary>
    ''' Determines whether the logger is temporarily enabled or disabled
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>This can be called by any function/method</remarks>
    Public Shared Property LoggerEnabled As Boolean = True

    Public Shared Sub BeginLogging()
        LogMessage("DynaLog Logger has begun logging program operations...", False)
        LogMessage("--- Time Stamps are shown in UTC Time!!! ---", False)
    End Sub

    Public Shared Sub EndLogging()
        LogMessage("DynaLog Logger has stopped logging program operations...", False)
        LogMessage("========================================================", False)
    End Sub

    Public Shared Sub DisableLogging()
        LogMessage("Logger has been temporarily disabled by caller " & New StackFrame(1).GetMethod().Name, False)
        LoggerEnabled = False
    End Sub

    Public Shared Sub EnableLogging()
        LoggerEnabled = True
        LogMessage("Logger has been enabled again by caller " & New StackFrame(1).GetMethod().Name, False)
    End Sub

    ''' <summary>
    ''' Logs a message with DynaLog to the log file
    ''' </summary>
    ''' <param name="message">The message to log. It cannot be empty</param>
    ''' <param name="GetParentCaller">Determines whether or not to get the name of the caller that called a specific method that called DynaLog logging</param>
    ''' <remarks></remarks>
    Public Shared Sub LogMessage(message As String, Optional GetParentCaller As Boolean = True)
        If Not LoggerEnabled OrElse message = "" Then
            Debug.WriteLine("Logger Enabled? " & If(LoggerEnabled, "Yes", "No"))
            Debug.WriteLine("Message: " & Quote & message & Quote)
            Debug.WriteLine("Either the logger is not enabled or the message is empty. Message cannot be logged.")
            Exit Sub
        End If
        Debug.WriteLine(message)
        Try
            Dim FileLength As Long = 0
            If File.Exists(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\HI_DynaLog.log") Then
                FileLength = New FileInfo(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\HI_DynaLog.log").Length
            End If
            Dim MessagePrefix As String = "[" & Date.UtcNow.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture) & "] [PID " & Process.GetCurrentProcess().Id & "] [" & New StackFrame(1).GetMethod().Name & If(GetParentCaller, " (" & New StackFrame(2).GetMethod().Name & ")", "") & "] "
            Dim MessageLine As String = MessagePrefix & message.Replace(CrLf, CrLf & MessagePrefix).Trim()
            File.AppendAllText(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\HI_DynaLog.log", If(FileLength > 0, CrLf, "") & MessageLine)
        Catch ex As Exception
            Debug.WriteLine("DynaLog logging could not log this operation. Error:" & CrLf & CrLf & ex.ToString())
        End Try
    End Sub

End Class
