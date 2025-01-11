Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

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

    ''' <summary>
    ''' Determines whether the logger is permanently enabled or disabled
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>This can only be called by specific methods used by DISMTools</remarks>
    Public Shared Property UltimatelyEnabled As Boolean = True

    ''' <summary>
    ''' Checks the age of the active DynaLog log file
    ''' </summary>
    ''' <remarks>If a DynaLog log file is older than 2 weeks, it will be archived</remarks>
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
                        File.SetCreationTimeUtc(Application.StartupPath & "\logs\DT_DynaLog.log", Date.UtcNow)
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
        LogMessage("========================================================", False)
    End Sub

    Public Shared Sub DisableLogging()
        LogMessage("Logger has been temporarily disabled by caller " & New StackFrame(1).GetMethod().Name)
        LoggerEnabled = False
    End Sub

    Public Shared Sub EnableLogging(Optional UltimateEnable As Boolean = False)
        If UltimatelyEnabled Then
            LoggerEnabled = True
            LogMessage("Logger has been enabled again by caller " & New StackFrame(1).GetMethod().Name)
        Else
            If UltimateEnable Then
                UltimatelyEnabled = True
                EnableLogging()
                Exit Sub
            End If
            Debug.WriteLine("The logger has been ultimately disabled and cannot be enabled with this method. Use the Ultimate switch to bypass.")
        End If
    End Sub

    ''' <summary>
    ''' Logs a message with DynaLog to the log file
    ''' </summary>
    ''' <param name="message">The message to log. It cannot be empty</param>
    ''' <param name="GetParentCaller">Determines whether or not to get the name of the caller that called a specific method that called DynaLog logging</param>
    ''' <remarks></remarks>
    Public Shared Sub LogMessage(message As String, Optional GetParentCaller As Boolean = True)
        If Not LoggerEnabled Then Exit Sub
        If message = "" Then Exit Sub
        Debug.WriteLine(message)
        Try
            ' DynaLog will NOT display logs for log file/folder creation - ONLY in debugger.
            If Not Directory.Exists(Application.StartupPath & "\logs") Then
                Debug.WriteLine("Creating log directory...")
                Directory.CreateDirectory(Application.StartupPath & "\logs")
            End If
            Dim FileLength As Integer = 0
            If File.Exists(Application.StartupPath & "\logs\DT_DynaLog.log") Then
                FileLength = New FileInfo(Application.StartupPath & "\logs\DT_DynaLog.log").Length
            End If
            Dim MessagePrefix As String = "[" & Date.UtcNow.ToString("MM/dd/yyyy HH:mm:ss") & "] " & "[" & New StackFrame(1).GetMethod().Name & If(GetParentCaller, " (" & New StackFrame(2).GetMethod().Name & ")", "") & "] "
            Dim MessageLine As String = MessagePrefix & message.Replace(CrLf, CrLf & MessagePrefix).Trim()
            File.AppendAllText(Application.StartupPath & "\logs\DT_DynaLog.log", If(FileLength > 0, CrLf, "") & MessageLine)
        Catch ex As Exception
            Debug.WriteLine("DynaLog logging could not log this operation. Error:" & CrLf & CrLf & ex.ToString())
        End Try
    End Sub

End Class
