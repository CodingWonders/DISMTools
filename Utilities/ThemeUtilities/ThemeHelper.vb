Imports IniParser
Imports IniParser.Model
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports Microsoft.VisualBasic.ControlChars

Module ThemeHelper

    ''' <summary>
    ''' The path to the theme directory
    ''' </summary>
    ''' <remarks></remarks>
    Private ReadOnly ThemePath As String = Path.Combine(Application.StartupPath, "bin", "themes")

    ''' <summary>
    ''' A list of INI data for the theme
    ''' </summary>
    ''' <remarks></remarks>
    Private ReadOnly ThemeData As New List(Of IniData)

    ''' <summary>
    ''' The list of file names for the themes
    ''' </summary>
    ''' <remarks></remarks>
    Private ReadOnly FileNames As New List(Of String)

    ''' <summary>
    ''' The list of loaded themes
    ''' </summary>
    ''' <remarks></remarks>
    Private _themes As New List(Of Theme)

    ''' <summary>
    ''' The currently specified theme
    ''' </summary>
    ''' <remarks></remarks>
    Public CurrentTheme As Theme

    ''' <summary>
    ''' The list of fallback themes
    ''' </summary>
    ''' <remarks>These act as default themes in case the themes folder is not present</remarks>
    Private ReadOnly FallbackThemes As New List(Of Theme)

#Region "DPI Handling"

    Private Const DPI_100 As Double = 96.0F
    Private Const DPI_125 As Double = 120.0F
    Private Const DPI_125_ASSET As String = "_125"
    Private Const DPI_150 As Double = 144.0F
    Private Const DPI_150_ASSET As String = "_150"
    Private Const DPI_175 As Double = 168.0F
    Private Const DPI_175_ASSET As String = "_175"
    Private Const DPI_200 As Double = 192.0F
    Private Const DPI_200_ASSET As String = "_200"
    Private Const DPI_300 As Double = 288.0F
    Private Const DPI_300_ASSET As String = "_300"

#End Region

    ''' <summary>
    ''' Loads the themes in <paramref>ThemePath</paramref>
    ''' </summary>
    ''' <param name="fallbackOnly">Determines whether fallback themes will only be loaded. The default value is false</param>
    ''' <remarks></remarks>
    Sub LoadThemes(Optional fallbackOnly As Boolean = False)
        DynaLog.LogMessage("Preparing to load themes...")
        DynaLog.LogMessage("- Are fallback themes only loaded? " & If(fallbackOnly, "Yes", "No"))
        DynaLog.LogMessage("Clearing previously loaded themes...")
        ThemeData.Clear()
        FileNames.Clear()
        _themes.Clear()
        DynaLog.LogMessage("Adding fallback themes...")
        FallbackThemes.AddRange(New Theme() {New Theme("",
                                                       "DISMTools 0.7 Color Scheme (Dark)",
                                                       True,
                                                       ColorTranslator.FromHtml("#1F1F1F"),
                                                       ColorTranslator.FromHtml("#121212"),
                                                       Color.White,
                                                       New Color() {
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
                                                       New Color() {
                                                           ColorTranslator.FromHtml("#C4E5C0"),
                                                           ColorTranslator.FromHtml("#6FCF97"),
                                                           ColorTranslator.FromHtml("#81E6A8"),
                                                           ColorTranslator.FromHtml("#A3F7C5")
                                                       }
                                            )
                                            }
                                        )
        If fallbackOnly Then
            DynaLog.LogMessage("Fallback themes will only be loaded. Setting them as final themes and setting the current theme...")
            _themes = FallbackThemes
            ChangeCurrentTheme(0, True)
            Exit Sub
        End If
        Try
            DynaLog.LogMessage("Checking if themes directory exists...")
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
                _themes.Clear()
                For Each dataFile As IniData In ThemeData
                    Try
                        Dim name, isDark, bgColor, sectionBgColor, fgColor, ac1, ac2, ac3, ac4 As String
                        name = dataFile("Theme Information")("Name").Replace(Quote, "")
                        isDark = dataFile("Theme Colors")("IsDark")
                        bgColor = dataFile("Theme Colors")("BackgroundColor").Replace(Quote, "")
                        sectionBgColor = dataFile("Theme Colors")("SectionBackgroundColor").Replace(Quote, "")
                        fgColor = dataFile("Theme Colors")("ForegroundColor").Replace(Quote, "")
                        ac1 = dataFile("Theme Colors")("AccentColor1").Replace(Quote, "")
                        ac2 = dataFile("Theme Colors")("AccentColor2").Replace(Quote, "")
                        ac3 = dataFile("Theme Colors")("AccentColor3").Replace(Quote, "")
                        ac4 = dataFile("Theme Colors")("AccentColor4").Replace(Quote, "")

                        _themes.Add(New Theme(FileNames(ThemeData.IndexOf(dataFile)),
                                             name,
                                             CInt(isDark) = 1,
                                             ColorTranslator.FromHtml(bgColor),
                                             ColorTranslator.FromHtml(sectionBgColor),
                                             ColorTranslator.FromHtml(fgColor),
                                             New Color() {
                                                 ColorTranslator.FromHtml(ac1),
                                                 ColorTranslator.FromHtml(ac2),
                                                 ColorTranslator.FromHtml(ac3),
                                                 ColorTranslator.FromHtml(ac4)}
                                             )
                                         )
                        DynaLog.LogMessage(_themes.Last().ToString())
                    Catch ex As Exception

                    End Try
                Next
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not load themes. Error message: " & ex.Message)
            DynaLog.LogMessage("Falling back...")
            _themes = FallbackThemes
        End Try
        SetDisabledForegroundColors()
    End Sub

    ''' <summary>
    ''' Sets the foreground color for inactive items
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDisabledForegroundColors()
        For Each loadedTheme As Theme In _themes
            Dim multiplicationFactor As Decimal = If(IsLightColor(loadedTheme.ForegroundColor), 0.7, 1.3)
            Dim colorValue As Integer
            If multiplicationFactor = 1.3 Then
                colorValue = Math.Min(loadedTheme.ForegroundColor.R + 133, 255)
            Else
                colorValue = CInt(loadedTheme.ForegroundColor.R * multiplicationFactor)
            End If
            Dim disabledFgColor As Color = Color.FromArgb(
                loadedTheme.ForegroundColor.A,
                colorValue,
                colorValue,
                colorValue
                )
            loadedTheme.DisabledForegroundColor = disabledFgColor
        Next
    End Sub

    Private Function IsLightColor(color As Color) As Boolean
        Dim brightness As Double = (0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B)
        Return brightness >= 128
    End Function

    ''' <summary>
    ''' Changes the current theme to one of the currently loaded ones
    ''' </summary>
    ''' <param name="themeIndex">The index of the theme to load</param>
    ''' <param name="forceDarkTheme">Determines whether to consider a dark theme, for fallback purposes</param>
    ''' <remarks>If there's an out-of-bounds exception when changing the theme, it starts taking into account <paramref>ForceDarkTheme</paramref> to
    ''' get one of the first loaded themes. If that throws an exception as well, it loads from the fallback set, whilst still taking into account <paramref>ForceDarkTheme</paramref>
    ''' </remarks>
    Sub ChangeCurrentTheme(themeIndex As Integer, Optional forceDarkTheme As Boolean = False)
        Try
            CurrentTheme = _themes(themeIndex)
        Catch ex As Exception
            Try
                CurrentTheme = _themes(If(forceDarkTheme,
                                         0,
                                         1))
            Catch ex2 As Exception
                If forceDarkTheme Then
                    CurrentTheme = FallbackThemes(0)
                Else
                    CurrentTheme = FallbackThemes(1)
                End If
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Gets a resource for a glyph based on a name
    ''' </summary>
    ''' <param name="resourceName">The name of the resource</param>
    ''' <param name="checkForDarkVariant">Determines whether to check for dark variants of a glyph.</param>
    ''' <returns>A glyph bitmap</returns>
    ''' <remarks>Setting <paramref>CheckForDarkVariant</paramref> to false can be useful for glyphs that don't adapt to color schemes</remarks>
    Function GetGlyphResource(resourceName As String, Optional checkForDarkVariant As Boolean = True) As Bitmap
        If CurrentTheme.IsDark AndAlso checkForDarkVariant Then resourceName &= "_dark"
        Dim obj As Object
        obj = My.Resources.ResourceManager.GetObject(resourceName)
        If obj Is Nothing Then
            ' Try with _light
            obj = My.Resources.ResourceManager.GetObject(resourceName & "_light")
        End If
        Return CType(obj, Bitmap)
    End Function

    ''' <summary>
    ''' Gets a professional renderer for toolstrips and menustrips based on the properties of the current theme
    ''' </summary>
    ''' <returns>A professional renderer</returns>
    ''' <remarks></remarks>
    Function GetProfessionalRenderer() As ToolStripProfessionalRenderer
        If CurrentTheme.IsDark Then
            Return New DarkModeRenderer()
        Else
            Return New LightModeRenderer()
        End If
    End Function

    ''' <summary>
    ''' Gets the list of loaded themes
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetThemes() As List(Of Theme)
        Return _themes
    End Function

    Public Sub UpdateLinkLabelColors(parentControl As Control, linkColor As Color, activeLinkColor As Color)
        For Each ctrl As Control In parentControl.Controls
            If TypeOf ctrl Is LinkLabel Then
                Dim linkLbl As LinkLabel = DirectCast(ctrl, LinkLabel)
                linkLbl.LinkColor = linkColor
                linkLbl.ActiveLinkColor = activeLinkColor
            End If

            If ctrl.HasChildren Then
                UpdateLinkLabelColors(ctrl, linkColor, activeLinkColor)
            End If
        Next
    End Sub

End Module
