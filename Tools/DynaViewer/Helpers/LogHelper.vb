Imports DynaViewer.Classes
Imports System.Text.RegularExpressions

Public Module LogHelper

    Public Function ParseEventLine(ByVal Line As String) As DynaLogEvent
        Dim timestamp As String = ""
        Dim caller As String = ""
        Dim message As String = ""

        Try
            Dim match As Match = Regex.Match(Line, "\[(.*?)\] \[(.*?)\] (.+)")
            If match.Success Then
                timestamp = match.Groups(1).Value
                caller = match.Groups(2).Value
                message = match.Groups(3).Value
            End If
        Catch ex As Exception

        End Try

        Return New DynaLogEvent(timestamp, caller, message)
    End Function

End Module
