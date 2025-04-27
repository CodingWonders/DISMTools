Imports IniParser
Imports IniParser.Model
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports Microsoft.VisualBasic.ControlChars

Module ThemeHelper

    Private ReadOnly ThemePath As String = Path.Combine(Application.StartupPath, "bin", "themes")
    Private ThemeData As New List(Of IniData)
    Private FileNames As New List(Of String)

    Private Themes As New List(Of Theme)
    Public CurrentTheme As Theme

    Private FallbackThemes As New List(Of Theme)

    Private resourceMan As Resources.ResourceManager
    Private resourceCulture As Globalization.CultureInfo

    Sub LoadThemes(Optional FallbackOnly As Boolean = False)
        ThemeData.Clear()
        FileNames.Clear()
        Themes.Clear()
        FallbackThemes.AddRange(New Theme() {New Theme("",
                                                       "DISMTools 0.7 Color Scheme (Dark)",
                                                       True,
                                                       ColorTranslator.FromHtml("#1F1F1F"),
                                                       ColorTranslator.FromHtml("#121212"),
                                                       Color.White,
                                                       New Color(3) {
                                                           ColorTranslator.FromHtml("#143A10"),
                                                           ColorTranslator.FromHtml("#246B1C"),
                                                           ColorTranslator.FromHtml("#057F1A"),
                                                           ColorTranslator.FromHtml("#085522")
                                                       }
                                            ),
                                             New Theme("",
                                                       "DISMTools 0.7 Color Scheme (Light)",
                                                       False,
                                                       ColorTranslator.FromHtml("#EEEEF2"),
                                                       ColorTranslator.FromHtml("#FCFBFF"),
                                                       Color.Black,
                                                       New Color(3) {
                                                           ColorTranslator.FromHtml("#C4E5C0"),
                                                           ColorTranslator.FromHtml("#6FCF97"),
                                                           ColorTranslator.FromHtml("#81E6A8"),
                                                           ColorTranslator.FromHtml("#A3F7C5")
                                                       }
                                            )
                                            }
                                        )
        If FallbackOnly Then
            Themes = FallbackThemes
            ChangeCurrentTheme(0, True)
            Exit Sub
        End If
        Try
            If Not Directory.Exists(ThemePath) Then
                Throw New Exception("No theme directory exists")
            End If
            For Each ThemeFile In Directory.GetFiles(ThemePath, "*.ini", SearchOption.TopDirectoryOnly)
                Try
                    Dim parser = New FileIniDataParser()
                    Using reader As New StreamReader(ThemeFile, Encoding.UTF8)
                        ThemeData.Add(parser.ReadData(reader))
                        FileNames.Add(Path.GetFileName(ThemeFile))
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

                        Themes.Add(New Theme(FileNames(ThemeData.IndexOf(DataFile)),
                                             name,
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
        Catch ex As Exception
            DynaLog.LogMessage("Could not load themes. Falling back...")
            Themes = FallbackThemes
        End Try
        For Each LoadedTheme As Theme In Themes
            Dim MultiplicationFactor As Decimal = If(LoadedTheme.IsDark, 0.7, 1.3)
            Dim ColorValue As Integer
            If MultiplicationFactor = 1.3 Then
                ColorValue = Math.Min(LoadedTheme.ForegroundColor.R + 133, 255)
            Else
                ColorValue = CInt(LoadedTheme.ForegroundColor.R * MultiplicationFactor)
            End If
            Dim disabledFgColor As Color = Color.FromArgb(
                LoadedTheme.ForegroundColor.A,
                ColorValue,
                ColorValue,
                ColorValue
                )
            LoadedTheme.DisabledForegroundColor = disabledFgColor
        Next
    End Sub

    Sub ChangeCurrentTheme(ThemeIndex As Integer, Optional ForceDarkTheme As Boolean = False)
        Try
            CurrentTheme = Themes(ThemeIndex)
        Catch ex As Exception
            Try
                CurrentTheme = Themes(If(ForceDarkTheme,
                                         0,
                                         1))
            Catch ex2 As Exception
                If ForceDarkTheme Then
                    CurrentTheme = FallbackThemes(0)
                Else
                    CurrentTheme = FallbackThemes(1)
                End If
            End Try
        End Try
    End Sub

    Function GetGlyphResource(ResourceName As String, Optional CheckForDarkVariant As Boolean = True) As Bitmap
        If CurrentTheme.IsDark AndAlso CheckForDarkVariant Then ResourceName &= "_dark"
        Dim obj As Object
        obj = My.Resources.ResourceManager.GetObject(ResourceName)
        If obj Is Nothing Then
            ' Try with _light
            obj = My.Resources.ResourceManager.GetObject(ResourceName & "_light")
        End If
        Return CType(obj, Bitmap)
    End Function

    Function GetProfessionalRenderer() As ToolStripProfessionalRenderer
        If CurrentTheme.IsDark Then
            Return New DarkModeRenderer()
        Else
            Return New LightModeRenderer()
        End If
    End Function

End Module
