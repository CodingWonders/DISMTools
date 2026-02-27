Namespace Elements.Contemporaneus.ImageOperations

    Public MustInherit Class ImageOperation

        Protected Friend OperationOptions As New Dictionary(Of String, Object)
        Protected Friend ProcessExecutor As Func(Of String, String, Integer)

        Protected Friend Overridable Property CommandArgs As String = ""

        Protected Friend dateStr As String = "DISMTools-"

        Public Sub New()
            OperationOptions = New Dictionary(Of String, Object)
        End Sub

        Public Sub New(executor As Func(Of String, String, Integer))
            ProcessExecutor = executor
        End Sub

        Public MustOverride Function RunOperation() As Integer

        Protected Function GetProperty(PropertyName As String) As Object
            If Not OperationOptions.ContainsKey(PropertyName) Then Return Nothing
            Return OperationOptions(PropertyName)
        End Function

        ''' <summary>
        ''' Sets the name of the log file using the current date and time
        ''' </summary>
        ''' <param name="CurrentDate">The date to add. It is always "Now"</param>
        ''' <returns>This function returns a file name that can be used in log files, file-system friendly on both Unix and Windows</returns>
        ''' <remarks></remarks>
        Function GetCurrentDateAndTime(CurrentDate As Date) As String
            DynaLog.LogMessage("Getting a suitable name for log files with current date...")
            DynaLog.LogMessage("Current date: " & CurrentDate.ToString())
            dateStr = "DISMTools-" & CurrentDate.ToString()
            ' Make sure the file with the name is file-system friendly
            If dateStr.Contains("/") Or dateStr.Contains(":") Then
                dateStr = dateStr.Replace("/", "-").Trim().Replace(":", "-").Trim()
            End If
            dateStr &= ".log"
            Return dateStr
        End Function

#Region "Action Reporters"
        Protected Friend LogActivityReporter As Action(Of String) = Nothing
        Protected Friend LogAllTasksReporter As Action(Of String) = Nothing
        Protected Friend LogCurrTaskReporter As Action(Of String) = Nothing
#End Region

        Public Sub ReportLogActivity(LogMessage As String)
            If LogActivityReporter IsNot Nothing Then LogActivityReporter.Invoke(LogMessage)
        End Sub

        Public Sub ReportLogAllTasks(AllTasksMessage As String)
            If LogAllTasksReporter IsNot Nothing Then LogAllTasksReporter.Invoke(AllTasksMessage)
        End Sub

        Public Sub ReportLogCurrTask(CurrTaskMessage As String)
            If LogCurrTaskReporter IsNot Nothing Then LogCurrTaskReporter.Invoke(CurrTaskMessage)
        End Sub

        ''' <summary>
        ''' Runs the specified process and returns an exit code
        ''' </summary>
        ''' <param name="FilePath">The path of the file to run</param>
        ''' <param name="CommandArguments">The command-line arguments to pass to the file to run</param>
        ''' <param name="WorkingDirectory">The directory the file is in. This is optional and can be set to fix issues with the file to open</param>
        ''' <param name="DoNotRedirect">Determines whether to redirect output to console text area</param>
        ''' <remarks>Any logging is done with DynaLog</remarks>
        Public Function RunProcess(FilePath As String, CommandArguments As String, Optional WorkingDirectory As String = "", Optional DoNotRedirect As Boolean = False) As Integer
            If ProcessExecutor IsNot Nothing Then
                Return ProcessExecutor(FilePath, CommandArguments)
            End If
            Return -1
        End Function

    End Class

End Namespace
