Imports IniParser
Imports IniParser.Model
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports Microsoft.VisualBasic.ControlChars

Module ThemeHelper

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
    Sub LoadThemes(Optional ByVal fallbackOnly As Boolean = False)
        ThemeData.Clear()
        FileNames.Clear()
        _themes.Clear()
        FallbackThemes.AddRange(New Theme() {New Theme("", "Dark Mode", True, ColorTranslator.FromHtml("#0C0C0C"), ColorTranslator.FromHtml("#000000"), Color.White, New Color() {ColorTranslator.FromHtml("#6C0000"), ColorTranslator.FromHtml("#4A0000"), ColorTranslator.FromHtml("#280000"), ColorTranslator.FromHtml("#220000")}), _
                                             New Theme("", "Light Mode", False, ColorTranslator.FromHtml("#FFFFFF"), ColorTranslator.FromHtml("#EAEAEA"), Color.Black, New Color() {ColorTranslator.FromHtml("#6C0000"), ColorTranslator.FromHtml("#4A0000"), ColorTranslator.FromHtml("#280000"), ColorTranslator.FromHtml("#220000")})})
        _themes = FallbackThemes
        SetDisabledForegroundColors()
    End Sub

    ''' <summary>
    ''' Sets the foreground color for inactive items
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDisabledForegroundColors()
        For Each loadedTheme As Theme In _themes
            Dim multiplicationFactor As Decimal = IIf(IsLightColor(loadedTheme.ForegroundColor), 0.7, 1.3)
            Dim colorValue As Integer
            If multiplicationFactor = 1.3 Then
                colorValue = Math.Min(loadedTheme.ForegroundColor.R + 133, 255)
            Else
                colorValue = CInt(loadedTheme.ForegroundColor.R * multiplicationFactor)
            End If
            Dim disabledFgColor As Color = Color.FromArgb(loadedTheme.ForegroundColor.A, colorValue, colorValue, colorValue)
            loadedTheme.DisabledForegroundColor = disabledFgColor
        Next
    End Sub

    Private Function IsLightColor(ByVal color As Color) As Boolean
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
    Sub ChangeCurrentTheme(ByVal themeIndex As Integer, Optional ByVal forceDarkTheme As Boolean = False)
        Try
            CurrentTheme = _themes(themeIndex)
        Catch ex As Exception
            Try
                CurrentTheme = _themes(IIf(forceDarkTheme, 0, 1))
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
    Function GetGlyphResource(ByVal resourceName As String, Optional ByVal checkForDarkVariant As Boolean = True) As Bitmap
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

    Public Sub UpdateLinkLabelColors(ByVal parentControl As Control, ByVal linkColor As Color, ByVal activeLinkColor As Color)
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
