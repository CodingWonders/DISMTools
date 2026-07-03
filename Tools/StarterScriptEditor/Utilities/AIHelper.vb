Imports StarterScriptEditor.Classes.AutoInspection
Imports System.IO
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions

Public Class AIHelper

    Private Shared ReadOnly RuleXmlPath As String = Path.Combine(Application.StartupPath, "AutoInspectRules.xml")

    Private Shared Function GetScriptInspectionRules() As List(Of AutoInspectionRule)
        If Not File.Exists(RuleXmlPath) Then Return New List(Of AutoInspectionRule)

        Dim serializer As New XmlSerializer(GetType(AutoInspectionRules))
        Dim rules As AutoInspectionRules

        Try
            Using fs As New FileStream(RuleXmlPath, FileMode.Open)
                rules = CType(serializer.Deserialize(fs), AutoInspectionRules)
            End Using
            Return rules.Rules
        Catch ex As Exception
            Return New List(Of AutoInspectionRule)
        End Try
    End Function

    Private Shared ExpressionChecker As Regex

    Public Shared Function GetScriptCodeSecurityViolations(ByVal ScriptCode As String, ByVal ProgressReporter As Action(Of AutoInspectionProgressReport)) As List(Of AutoInspectionResult)
        Dim Violations As New List(Of AutoInspectionResult), _
            Rules As List(Of AutoInspectionRule) = GetScriptInspectionRules()

        ' Begin regex'ing the crap out of the script code
        Dim idx As Integer = 0
        For Each Rule As AutoInspectionRule In Rules
            Try
                ProgressReporter.Invoke(New AutoInspectionProgressReport(((idx / Rules.Count) * 100), Rule.RuleName))

                ExpressionChecker = New Regex(Rule.RuleExpression, RegexOptions.Compiled Or RegexOptions.IgnoreCase)
                Dim ViolationMatches As MatchCollection = ExpressionChecker.Matches(ScriptCode)
                If ViolationMatches.Count > 0 Then
                    For Each ViolationMatch As Match In ViolationMatches
                        Violations.Add(New AutoInspectionResult(Rule, ViolationMatch.Index, ViolationMatch.Length))
                    Next
                End If

                idx += 1
            Catch ex As Exception
                ' continue iterating
            End Try
        Next

        Return Violations
    End Function

End Class