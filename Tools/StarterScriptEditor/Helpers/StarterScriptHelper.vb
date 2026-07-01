Imports StarterScriptEditor.Classes
Imports System.IO
Imports System.Text.Encoding
#If VBC_VER >= 10.0 Then
Imports System.Linq
#End If

Module StarterScriptHelper

    Public Function LoadScriptFile(ByVal ScriptFile As String, Optional ByVal CodeBlockStartingIndex As Integer = 4) As StarterScript
        Dim scriptFileContents As String() = File.ReadAllLines(ScriptFile)

        ' Script Format:
        ' <Language>
        ' <Name>
        ' <Description>
        ' <Customizable> (0.8+)
        ' <code>
        Dim scriptLang As String = scriptFileContents(0).Replace("Language: ", "")
        Dim scriptName As String = scriptFileContents(1).Replace("Name: ", "")
        Dim scriptDescription As String = scriptFileContents(2).Replace("Description: ", "")
        Dim scriptOptionsCustomizable As Boolean = scriptFileContents(3).Equals("Customizable: Yes", StringComparison.OrdinalIgnoreCase)
#If VBC_VER >= 9.0 Then
        Return New StarterScript(scriptName, scriptDescription, scriptLang, String.Join(ControlChars.CrLf, New List(Of String)(scriptFileContents).Skip(CodeBlockStartingIndex).ToArray()), scriptOptionsCustomizable)
#Else
        ' NDPv2 and earlier do not support LINQ statements.
        Dim ScriptCodeLines As New List(Of String)
        For x As Integer = CodeBlockStartingIndex To scriptFileContents.Length - 1
            ScriptCodeLines.Add(scriptFileContents(x))
        Next
        Return New StarterScript(scriptName, scriptDescription, scriptLang, String.Join(ControlChars.CrLf, ScriptCodeLines.ToArray()), scriptOptionsCustomizable)
#End If
    End Function

    Public Function SaveStarterScript(ByVal ScriptFile As String, ByVal Script As StarterScript, ByVal ScriptVer As ScriptVersion) As Boolean
        If Script Is Nothing Then Throw New ArgumentNullException()

        Dim customizableStr As String
        If Script.OptionsCustomizable Then
            customizableStr = "Yes"
        Else
            customizableStr = "No"
        End If

        Try
            Select Case ScriptVer
                Case ScriptVersion.Seven
                    File.WriteAllText(ScriptFile, String.Format("Language: {0}{1}" & _
                                      "Name: {2}{1}" & _
                                      "Description: {3}{1}" & _
                                      "{4}", Script.Language, Environment.NewLine, Script.Name, Script.Description, Script.Code), UTF8)
                Case ScriptVersion.Infinity
                    File.WriteAllText(ScriptFile, String.Format("Language: {0}{1}" & _
                                      "Name: {2}{1}" & _
                                      "Description: {3}{1}" & _
                                      "Customizable: {4}{1}" & _
                                      "{5}", Script.Language, Environment.NewLine, Script.Name, Script.Description, customizableStr, Script.Code), UTF8)
            End Select
            Return True
        Catch ex As Exception
#If DEBUG Then
            MessageBox.Show(ex.Message, "Save Failure", MessageBoxButtons.OK, MessageBoxIcon.Error)
#End If
            Return False
        End Try
    End Function

End Module
