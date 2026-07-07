Imports StarterScriptEditor.Classes.AutoInspection
Imports System.IO
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions

Public Class AIHelper

    Private Shared ReadOnly RuleXmlPath As String = Path.Combine(Application.StartupPath, "AutoInspectRules.xml"), _
                            CustomRuleXmlPath As String = Path.Combine(Application.StartupPath, "CustomRules.xml"), _
                            CustomRuleXmlBackupPath As String = Path.Combine(Application.StartupPath, "CustomRules.xml.old")

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

    Public Shared Function GetScriptInspectionCustomRules(Optional ByVal ReturnEmptyIfThrown As Boolean = False) As List(Of AutoInspectionRule)
        If Not File.Exists(CustomRuleXmlPath) Then
            If ReturnEmptyIfThrown Then Return New List(Of AutoInspectionRule)
            Throw New FileNotFoundException("Inspection Rule File Not Found!", CustomRuleXmlPath)
        End If

        Dim serializer As New XmlSerializer(GetType(AutoInspectionRules))
        Dim rules As AutoInspectionRules

        Try
            Using fs As New FileStream(CustomRuleXmlPath, FileMode.Open)
                rules = CType(serializer.Deserialize(fs), AutoInspectionRules)
            End Using
            Return rules.Rules
        Catch ex As Exception
            If ReturnEmptyIfThrown Then Return New List(Of AutoInspectionRule)
            Throw
        End Try
    End Function

    Public Shared Function SaveCustomRules(ByVal CustomRules As List(Of AutoInspectionRule), Optional ByVal UserDataPath As String = "") As Boolean
        ' Back up the previous custom rule file if it exists.
        Try
            If File.Exists(CustomRuleXmlPath) Then
                File.Copy(CustomRuleXmlPath, CustomRuleXmlBackupPath, True)
            End If
        Catch ex As Exception

        End Try

        ' Begin saving the rules.
        Try
            Dim ruleCollection As New AutoInspectionRules(CustomRules)
            Dim serializer As New XmlSerializer(GetType(AutoInspectionRules))

            Using fs As New FileStream(CustomRuleXmlPath, FileMode.Create)
                serializer.Serialize(fs, ruleCollection)
            End Using
            If UserDataPath <> "" AndAlso Directory.Exists(UserDataPath) Then
                Try
                    File.Copy(CustomRuleXmlPath, Path.Combine(UserDataPath, Path.GetFileName(CustomRuleXmlPath)), True)
                Catch ex As Exception

                End Try
            End If
            Try
                File.Delete(CustomRuleXmlBackupPath)
            Catch ex As Exception

            End Try
            Return True
        Catch ex As Exception
            If File.Exists(CustomRuleXmlBackupPath) Then
                Try
                    File.Copy(CustomRuleXmlBackupPath, CustomRuleXmlPath, True)
                Catch restoreEx As Exception
                    ' ignore backup restore
                End Try
            End If
            Return False
        End Try
    End Function

    Private Shared ExpressionChecker As Regex

    Public Shared Function GetScriptCodeSecurityViolations(ByVal ScriptCode As String, ByVal ProgressReporter As Action(Of AutoInspectionProgressReport)) As List(Of AutoInspectionResult)
        Dim Violations As New List(Of AutoInspectionResult), _
            Rules As List(Of AutoInspectionRule) = GetScriptInspectionRules(), _
            CustomRules As List(Of AutoInspectionRule) = GetScriptInspectionCustomRules(True)

        ' Begin regex'ing the crap out of the script code
        Dim idx As Integer = 0
        For Each Rule As AutoInspectionRule In Rules
            Try
                ProgressReporter.Invoke(New AutoInspectionProgressReport(((idx / (Rules.Count + CustomRules.Count)) * 100), Rule.RuleName))

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

        ' Use custom rules now
        For Each CustomRule As AutoInspectionRule In CustomRules
            Try
                ProgressReporter.Invoke(New AutoInspectionProgressReport(((idx / (Rules.Count + CustomRules.Count)) * 100), CustomRule.RuleName))

                ExpressionChecker = New Regex(CustomRule.RuleExpression, RegexOptions.Compiled Or RegexOptions.IgnoreCase)
                Dim ViolationMatches As MatchCollection = ExpressionChecker.Matches(ScriptCode)
                If ViolationMatches.Count > 0 Then
                    For Each ViolationMatch As Match In ViolationMatches
                        Violations.Add(New AutoInspectionResult(CustomRule, ViolationMatch.Index, ViolationMatch.Length))
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