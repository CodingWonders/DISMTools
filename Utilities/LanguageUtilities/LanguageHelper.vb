Imports IniParser
Imports IniParser.Model
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text

Module LanguageHelper

    Private ReadOnly LanguagePath As String = Path.Combine(Application.StartupPath, "bin", "languages")

    Dim LanguageDatas As New List(Of IniData)

    Sub LoadLanguageFiles()
        LanguageDatas.Clear()
        If Not Directory.Exists(LanguagePath) Then
            Throw New Exception("Language files could not be found")
        End If
        For Each LanguageFile In Directory.GetFiles(LanguagePath, "*.ini", SearchOption.TopDirectoryOnly)
            Try
                Dim parser = New FileIniDataParser()
                Using reader As New StreamReader(LanguageFile, Encoding.UTF8)
                    LanguageDatas.Add(parser.ReadData(reader))
                End Using
            Catch ex As Exception
                DynaLog.LogMessage("Could not parse this file. Error message: " & ex.Message)
            End Try
        Next
    End Sub

    Function GetValueFromLanguageData(SpecifiedLanguage As Integer, ItemKey As String) As String
        If LanguageDatas IsNot Nothing Then
            If SpecifiedLanguage = 0 Then
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        SpecifiedLanguage = 1
                    Case "ESN"
                        SpecifiedLanguage = 2
                    Case "FRA"
                        SpecifiedLanguage = 3
                    Case "PTB", "PTG"
                        SpecifiedLanguage = 4
                    Case "ITA"
                        SpecifiedLanguage = 5
                End Select
            End If
            Try
                Dim KeySections() As String = ItemKey.Split(".")
                Return LanguageDatas(SpecifiedLanguage - 1)(KeySections(0))(KeySections(1)).Replace(Quote, "").Replace("{quot;}", Quote).Replace("{crlf;}", CrLf)
            Catch ex As Exception
                Return ItemKey
            End Try
        Else
            Return ItemKey
        End If
        Return Nothing
    End Function

End Module
