Imports Microsoft.VisualBasic.ControlChars

Public Class Theme
    ''' <summary>
    ''' The file name of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The theme file name</returns>
    ''' <remarks></remarks>
    Public Property FileName As String

    ''' <summary>
    ''' The name of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The theme name</returns>
    ''' <remarks>Not to be confused with <see>FileName</see></remarks>
    Public Property Name As String

    ''' <summary>
    ''' Determines whether a theme uses dark glyphs
    ''' </summary>
    ''' <value>True = the theme is expected to use dark glyphs; False = the theme is expected to use light glyphs</value>
    ''' <returns>Whether a theme uses dark glyphs</returns>
    ''' <remarks></remarks>
    Public Property IsDark As Boolean

    ''' <summary>
    ''' The main background color of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The private attribute</returns>
    ''' <remarks></remarks>
    Public Property BackgroundColor As Color

    ''' <summary>
    ''' The background color of the theme for inner sections
    ''' </summary>
    ''' <value></value>
    ''' <returns>The section background color</returns>
    ''' <remarks></remarks>
    Public Property SectionBackgroundColor As Color

    ''' <summary>
    ''' The main foreground color of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The main foreground color</returns>
    ''' <remarks></remarks>
    Public Property ForegroundColor As Color

    ''' <summary>
    ''' The foreground color for disabled items (for example, inactive headers in a set of pages)
    ''' </summary>
    ''' <value></value>
    ''' <returns>The disabled foreground color</returns>
    ''' <remarks>This attribute is calculated automatically by <see>ThemeHelper.LoadThemes</see></remarks>
    Public Property DisabledForegroundColor As Color

    ''' <summary>
    ''' A list of accent colors
    ''' </summary>
    ''' <value></value>
    ''' <returns>The accent color</returns>
    ''' <remarks>The implementation in 0.7 uses 4 accent colors</remarks>
    Public Property AccentColors As Color()

    ''' <summary>
    ''' Initializes a new theme object
    ''' </summary>
    ''' <param name="fileName">The name of the theme file</param>
    ''' <param name="name">The name of the theme</param>
    ''' <param name="isDark">Determines whether to set light/dark glyphs</param>
    ''' <param name="backgroundColor">The main background color</param>
    ''' <param name="sectionBackgroundColor">The background color for inner sections</param>
    ''' <param name="foregroundColor">The main foreground color</param>
    ''' <param name="accentColors">A list of accent colors</param>
    ''' <remarks></remarks>
    Public Sub New(fileName As String, name As String, isDark As Boolean, backgroundColor As Color, sectionBackgroundColor As Color, foregroundColor As Color, accentColors As Color())
        Me.FileName = fileName
        Me.Name = name
        Me.IsDark = isDark
        Me.BackgroundColor = backgroundColor
        Me.SectionBackgroundColor = sectionBackgroundColor
        Me.ForegroundColor = foregroundColor
        Me.AccentColors = accentColors
    End Sub

    Public Overrides Function ToString() As String
        Return "Theme:" & CrLf &
               "- File Name: " & FileName & CrLf &
               "- Name: " & Name & CrLf &
               "Check additional properties in debugger."
    End Function
End Class
