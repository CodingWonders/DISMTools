Imports IniParser
Imports IniParser.Model
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports Microsoft.VisualBasic.ControlChars

Module ThemeHelper

    Private ReadOnly ThemePath As String = Path.Combine(Application.StartupPath, "bin", "themes")
    Private ThemeData As New List(Of IniData)

    Public Themes As New List(Of Theme)

    Sub LoadThemes()
        If Not Directory.Exists(ThemePath) Then
            Throw New Exception("No theme directory exists")
        End If
        For Each ThemeFile In Directory.GetFiles(ThemePath, "*.ini", SearchOption.TopDirectoryOnly)
            Try
                Dim parser = New FileIniDataParser()
                Using reader As New StreamReader(ThemeFile, Encoding.UTF8)
                    ThemeData.Add(parser.ReadData(reader))
                End Using
            Catch ex As Exception
                DynaLog.LogMessage("Could not parse this file. Error message: " & ex.Message)
            End Try
        Next
        If ThemeData.Count > 0 Then
            For Each DataFile As IniData In ThemeData
                Try
                    Dim name, isDark, bgColor, sectionBgColor, fgColor, ac1, ac2, ac3, ac4 As String
                    name = DataFile("Theme Information")("Name").Replace(Quote, "")
                    isDark = DataFile("Theme Colors")("IsDark")
                    bgColor = DataFile("Theme Colors")("BackgroundColor").Replace(Quote, "")
                    sectionBgColor = DataFile("Theme Colors")("SectionBackgroundColor").Replace(Quote, "")
                    fgColor = DataFile("Theme Colors")("ForegroundColor").Replace(Quote, "")
                    ac1 = DataFile("Theme Colors")("AccentColor1").Replace(Quote, "")
                    ac2 = DataFile("Theme Colors")("AccentColor2").Replace(Quote, "")
                    ac3 = DataFile("Theme Colors")("AccentColor3").Replace(Quote, "")
                    ac4 = DataFile("Theme Colors")("AccentColor4").Replace(Quote, "")

                    Themes.Add(New Theme(name,
                                         CInt(isDark) = 1,
                                         ColorTranslator.FromHtml(bgColor),
                                         ColorTranslator.FromHtml(sectionBgColor),
                                         ColorTranslator.FromHtml(fgColor),
                                         New Color(3) {
                                             ColorTranslator.FromHtml(ac1),
                                             ColorTranslator.FromHtml(ac2),
                                             ColorTranslator.FromHtml(ac3),
                                             ColorTranslator.FromHtml(ac4)}
                                         )
                                     )
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

End Module
