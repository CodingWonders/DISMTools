Imports DynaViewer.Classes
Imports System.Text.RegularExpressions

Public Module LogHelper

    Public Function ParseEventLine(ByVal Line As String) As DynaLogEvent
        Dim timestamp As String = ""
        Dim pid As String = ""
        Dim caller As String = ""
        Dim message As String = ""

        Dim Matches() As String = New String(2) {"\[(.*?)\] \[(.*?)\] \[(.*?)\] (.+)", "\[(.*?)\] \[(.*?)\] \[(.*?)\]", "\[(.*?)\] \[(.*?)\] (.+)"}
        Dim MatchArrayIndex As Integer = 0

        Try
            Dim match As Match = Nothing
            For Each RegexMatch As String In Matches
                match = Regex.Match(Line, RegexMatch)
                If match.Success Then Exit For
                MatchArrayIndex += 1
            Next

            Select Case MatchArrayIndex
                Case 0
                    ' We go here when everything was grabbed (timestamp, pid, callers, message)
                    timestamp = match.Groups(1).Value
                    pid = match.Groups(2).Value
                    caller = match.Groups(3).Value
                    message = match.Groups(4).Value
                Case 1
                    ' We go here when the message was not grabbed (timestamp, pid, callers)
                    timestamp = match.Groups(1).Value
                    pid = match.Groups(2).Value
                    caller = match.Groups(3).Value
                    message = "====== NO MESSAGE RECORDED IN THIS EVENT. Please ignore ======"
                Case 2
                    ' We go here in case we encountered code made by legacy DynaLog (timestamp, callers, message)
                    timestamp = match.Groups(1).Value
                    pid = "NOT OBTAINED"
                    caller = match.Groups(2).Value
                    message = match.Groups(3).Value
            End Select

            'If match.Success Then
            '    timestamp = match.Groups(1).Value
            '    pid = match.Groups(2).Value
            '    caller = match.Groups(3).Value
            '    message = match.Groups(4).Value
            'Else
            '    match = Regex.Match(Line, "\[(.*?)\] \[(.*?)\] (.+)")
            '    If match.Success Then
            '        timestamp = match.Groups(1).Value
            '        pid = "NOT OBTAINED"
            '        caller = match.Groups(2).Value
            '        message = match.Groups(3).Value
            '    End If
            'End If
        Catch ex As Exception

        End Try

        Return New DynaLogEvent(timestamp, pid.Replace("PID ", ""), caller, message)
    End Function

End Module
