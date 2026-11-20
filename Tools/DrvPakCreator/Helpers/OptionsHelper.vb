Imports DrvPakCreator.Classes
Imports IniParser
Imports IniParser.Model
Imports System.IO
Imports System.Text

Namespace Helpers

    Public Class OptionsHelper

        Private Shared ReadOnly SettingsPath As String = Path.Combine(Application.StartupPath, "settings.ini")

        ''' <summary>
        ''' Gets a setting value from a INI data collection
        ''' </summary>
        ''' <param name="IniEngineData">The INI data collection</param>
        ''' <param name="IniSection">The section of the INI data to parse</param>
        ''' <param name="IniKey">The key of the INI data to parse</param>
        ''' <param name="DefaultValue">The default value to return if the INI data could not be parsed</param>
        ''' <returns>The specified setting value, or <paramref name="DefaultValue"/> if it could not be obtained.</returns>
        ''' <remarks></remarks>
        Private Shared Function GetIniSettingValue(ByVal IniEngineData As IniData, ByVal IniSection As String, ByVal IniKey As String, ByVal DefaultValue As String) As String
            Try
                If IniEngineData(IniSection).ContainsKey(IniKey) Then
                    Return IniEngineData(IniSection)(IniKey)
                Else
                    Throw New Exception()
                End If
            Catch ex As Exception
                Return DefaultValue
            End Try
        End Function

        Public Shared Function LoadSettings() As Boolean
            If Not File.Exists(SettingsPath) Then
                If Not GenerateSettings() Then Return False
            End If

            Dim SettingsData As IniData

            Try
                Dim Parser As New FileIniDataParser()
                Using Reader As New StreamReader(SettingsPath, Encoding.UTF8)
                    SettingsData = Parser.ReadData(Reader)
                End Using

                ' begin loading settings
                Dim ColorMode As Integer = 2, _
                    UseLZMA As Boolean = False, _
                    ShowCreatedDriverPackInExplorer As Boolean = True
                Integer.TryParse(GetIniSettingValue(SettingsData, "General", "ColorMode", "2"), ColorMode)
                Boolean.TryParse(GetIniSettingValue(SettingsData, "General", "UseLZMA", "0"), UseLZMA)
                Boolean.TryParse(GetIniSettingValue(SettingsData, "General", "ShowCreatedDriverPackInExplorer", "1"), ShowCreatedDriverPackInExplorer)
                Options.ColorMode = ColorMode
                Options.UseLZMA = UseLZMA
                Options.ShowCreatedDriverPackInExplorer = ShowCreatedDriverPackInExplorer

                Return True
            Catch ex As Exception
                Return GenerateSettings()
            End Try
        End Function

        Public Shared Function SaveSettings() As Boolean
            Try
                Dim Parser As New FileIniDataParser()
                Dim SettingsData As New IniData()

                Try
                    SettingsData.Sections.AddSection("General")
                    SettingsData("General").AddKey("ColorMode", CInt(Options.ColorMode))
                    SettingsData("General").AddKey("UseLZMA", IIf(Options.UseLZMA, 1, 0))
                    SettingsData("General").AddKey("ShowCreatedDriverPackInExplorer", IIf(Options.ShowCreatedDriverPackInExplorer, 1, 0))
                    Parser.WriteFile(SettingsPath, SettingsData, Encoding.UTF8)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            Catch ex As Exception

            End Try
        End Function

        Public Shared Function GenerateSettings() As Boolean
            Try
                Dim Parser As New FileIniDataParser()
                Dim SettingsData As New IniData()

                Try
                    SettingsData.Sections.AddSection("General")
                    SettingsData("General").AddKey("ColorMode", 2)
                    SettingsData("General").AddKey("UseLZMA", 0)
                    SettingsData("General").AddKey("ShowCreatedDriverPackInExplorer", 1)
                    Parser.WriteFile(SettingsPath, SettingsData, Encoding.UTF8)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            Catch ex As Exception

            End Try
        End Function

    End Class

End Namespace