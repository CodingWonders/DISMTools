Imports IniParser
Imports IniParser.Model
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports Microsoft.VisualBasic.ControlChars

Module ThemeHelper

    ''' <summary>
    ''' Gets the foreground color for inactive items
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetDisabledForegroundColor(ByVal theme As Theme) As Color
        Dim multiplicationFactor As Decimal = IIf(IsLightColor(theme.ForegroundColor), 0.7, 1.3)
        Dim colorValue As Integer
        If multiplicationFactor = 1.3 Then
            colorValue = Math.Min(theme.ForegroundColor.R + 133, 255)
        Else
            colorValue = CInt(theme.ForegroundColor.R * multiplicationFactor)
        End If
        Dim disabledFgColor As Color = Color.FromArgb( _
            theme.ForegroundColor.A, _
            colorValue, _
            colorValue, _
            colorValue _
            )
        Return disabledFgColor
    End Function

    Private Function IsLightColor(ByVal color As Color) As Boolean
        Dim brightness As Double = (0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B)
        Return brightness >= 128
    End Function

    ''' <summary>
    ''' Gets a resource for a glyph based on a name
    ''' </summary>
    ''' <param name="resourceName">The name of the resource</param>
    ''' <param name="checkForDarkVariant">Determines whether to check for dark variants of a glyph.</param>
    ''' <returns>A glyph bitmap</returns>
    ''' <remarks>Setting <paramref>CheckForDarkVariant</paramref> to false can be useful for glyphs that don't adapt to color schemes</remarks>
    Function GetGlyphResource(ByVal resourceName As String, Optional ByVal checkForDarkVariant As Boolean = True, Optional ByVal ThemeProperties As Theme = Nothing) As Bitmap
        If ThemeProperties IsNot Nothing Then
            If ThemeProperties.IsDark AndAlso checkForDarkVariant Then resourceName &= "_dark"
        End If
        Dim obj As Object
        obj = My.Resources.ResourceManager.GetObject(resourceName)
        If obj Is Nothing Then
            ' Try with _light
            obj = My.Resources.ResourceManager.GetObject(resourceName & "_light")
        End If
        Return CType(obj, Bitmap)
    End Function

#Region "I/O"

    Public Function LoadThemeFile(ByVal ThemePath As String) As Theme
        Dim finalTheme As Theme = New Theme()
        Dim fileData As IniData = Nothing
        If File.Exists(ThemePath) Then
            Try
                Dim parser = New FileIniDataParser()
                Using reader As New StreamReader(ThemePath, Encoding.UTF8)
                    fileData = parser.ReadData(reader)
                End Using
            Catch ex As Exception

            End Try
            If fileData IsNot Nothing Then
                Dim name, isDark, bgColor, sectionBgColor, fgColor, ac1, ac2, ac3, ac4 As String
                name = fileData("Theme Information")("Name").Replace(Quote, "")
                isDark = fileData("Theme Colors")("IsDark")
                bgColor = fileData("Theme Colors")("BackgroundColor").Replace(Quote, "")
                sectionBgColor = fileData("Theme Colors")("SectionBackgroundColor").Replace(Quote, "")
                fgColor = fileData("Theme Colors")("ForegroundColor").Replace(Quote, "")
                ac1 = fileData("Theme Colors")("AccentColor1").Replace(Quote, "")
                ac2 = fileData("Theme Colors")("AccentColor2").Replace(Quote, "")
                ac3 = fileData("Theme Colors")("AccentColor3").Replace(Quote, "")
                ac4 = fileData("Theme Colors")("AccentColor4").Replace(Quote, "")

                finalTheme = New Theme(Path.GetFileName(ThemePath), _
                                     name, _
                                     CInt(isDark) = 1, _
                                     ColorTranslator.FromHtml(bgColor), _
                                     ColorTranslator.FromHtml(sectionBgColor), _
                                     ColorTranslator.FromHtml(fgColor), _
                                     New Color() { _
                                         ColorTranslator.FromHtml(ac1), _
                                         ColorTranslator.FromHtml(ac2), _
                                         ColorTranslator.FromHtml(ac3), _
                                         ColorTranslator.FromHtml(ac4)} _
                                     )
            End If
        End If
        Return finalTheme
    End Function

    Public Function SaveTheme(ByVal ThemeProperties As Theme, ByVal TargetFile As String) As Boolean
        Try
            Dim parser = New FileIniDataParser()
            Dim data As IniData = New IniData()
            data.Sections.AddSection("Theme Information")
            data("Theme Information").AddKey("Name", Quote & ThemeProperties.Name & Quote)
            data.Sections.AddSection("Theme Colors")
            data("Theme Colors").AddKey("IsDark", IIf(ThemeProperties.IsDark, 1, 0))
            data("Theme Colors").AddKey("BackgroundColor", Quote & String.Format("#{0}{1}{2}", _
                ThemeProperties.BackgroundColor.R.ToString("X2"), _
                ThemeProperties.BackgroundColor.G.ToString("X2"), _
                ThemeProperties.BackgroundColor.B.ToString("X2")) & _
            Quote)
            data("Theme Colors").AddKey("SectionBackgroundColor", Quote & String.Format("#{0}{1}{2}", _
                ThemeProperties.SectionBackgroundColor.R.ToString("X2"), _
                ThemeProperties.SectionBackgroundColor.G.ToString("X2"), _
                ThemeProperties.SectionBackgroundColor.B.ToString("X2")) & _
            Quote)
            data("Theme Colors").AddKey("ForegroundColor", Quote & String.Format("#{0}{1}{2}", _
                ThemeProperties.ForegroundColor.R.ToString("X2"), _
                ThemeProperties.ForegroundColor.G.ToString("X2"), _
                ThemeProperties.ForegroundColor.B.ToString("X2")) & _
            Quote)
            data("Theme Colors").AddKey("AccentColor1", Quote & String.Format("#{0}{1}{2}", _
                ThemeProperties.AccentColors(0).R.ToString("X2"), _
                ThemeProperties.AccentColors(0).G.ToString("X2"), _
                ThemeProperties.AccentColors(0).B.ToString("X2")) & _
            Quote)
            data("Theme Colors").AddKey("AccentColor2", Quote & String.Format("#{0}{1}{2}", _
                ThemeProperties.AccentColors(1).R.ToString("X2"), _
                ThemeProperties.AccentColors(1).G.ToString("X2"), _
                ThemeProperties.AccentColors(1).B.ToString("X2")) & _
            Quote)
            data("Theme Colors").AddKey("AccentColor3", Quote & String.Format("#{0}{1}{2}", _
                ThemeProperties.AccentColors(2).R.ToString("X2"), _
                ThemeProperties.AccentColors(2).G.ToString("X2"), _
                ThemeProperties.AccentColors(2).B.ToString("X2")) & _
            Quote)
            data("Theme Colors").AddKey("AccentColor4", Quote & String.Format("#{0}{1}{2}", _
                ThemeProperties.AccentColors(3).R.ToString("X2"), _
                ThemeProperties.AccentColors(3).G.ToString("X2"), _
                ThemeProperties.AccentColors(3).B.ToString("X2")) & _
            Quote)
            parser.WriteFile(TargetFile, data)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetNewTheme() As Theme
        Return New Theme()
    End Function

#End Region

End Module
