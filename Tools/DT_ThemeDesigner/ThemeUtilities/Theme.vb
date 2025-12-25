Imports Microsoft.VisualBasic.ControlChars

Public Class Theme
#If VBC_VER < 10.0 Then
    Private themeFileName As String
#End If
    ''' <summary>
    ''' The file name of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The theme file name</returns>
    ''' <remarks></remarks>
    Public Property FileName() As String
#If VBC_VER < 10.0 Then
        Get
            Return themeFileName
        End Get
        Set(ByVal value As String)
            themeFileName = value
        End Set
    End Property
#End If

#If VBC_VER < 10.0 Then
    Private themeName As String
#End If

    ''' <summary>
    ''' The name of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The theme name</returns>
    ''' <remarks>Not to be confused with <see>FileName</see></remarks>
    Public Property Name() As String
#If VBC_VER < 10.0 Then
        Get
            Return themeName
        End Get
        Set(ByVal value As String)
            themeName = value
        End Set
    End Property
#End If

#If VBC_VER < 10.0 Then
    Private themeIsDark As Boolean
#End If

    ''' <summary>
    ''' Determines whether a theme uses dark glyphs
    ''' </summary>
    ''' <value>True = the theme is expected to use dark glyphs; False = the theme is expected to use light glyphs</value>
    ''' <returns>Whether a theme uses dark glyphs</returns>
    ''' <remarks></remarks>
    Public Property IsDark() As Boolean
#If VBC_VER < 10.0 Then
        Get
            Return themeIsDark
        End Get
        Set(ByVal value As Boolean)
            themeIsDark = value
        End Set
    End Property
#End If

#If VBC_VER < 10.0 Then
    Private themeBackgroundColor As Color
#End If

    ''' <summary>
    ''' The main background color of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The private attribute</returns>
    ''' <remarks></remarks>
    Public Property BackgroundColor() As Color
#If VBC_VER < 10.0 Then
        Get
            Return themeBackgroundColor
        End Get
        Set(ByVal value As Color)
            themeBackgroundColor = value
        End Set
    End Property
#End If

#If VBC_VER < 10.0 Then
    Private themeSectionBackgroundColor As Color
#End If

    ''' <summary>
    ''' The background color of the theme for inner sections
    ''' </summary>
    ''' <value></value>
    ''' <returns>The section background color</returns>
    ''' <remarks></remarks>
    Public Property SectionBackgroundColor() As Color
#If VBC_VER < 10.0 Then
        Get
            Return themeSectionBackgroundColor
        End Get
        Set(ByVal value As Color)
            themeSectionBackgroundColor = value
        End Set
    End Property
#End If

#If VBC_VER < 10.0 Then
    Private themeForegroundColor As Color
#End If
    ''' <summary>
    ''' The main foreground color of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The main foreground color</returns>
    ''' <remarks></remarks>
    Public Property ForegroundColor() As Color
#If VBC_VER < 10.0 Then
        Get
            Return themeForegroundColor
        End Get
        Set(ByVal value As Color)
            themeForegroundColor = value
        End Set
    End Property
#End If

#If VBC_VER < 10.0 Then
    Private themeDisabledForegroundColor As Color
#End If
    ''' <summary>
    ''' The foreground color for disabled items (for example, inactive headers in a set of pages)
    ''' </summary>
    ''' <value></value>
    ''' <returns>The disabled foreground color</returns>
    ''' <remarks>This attribute is calculated automatically by <see>ThemeHelper.LoadThemes</see></remarks>
    Public Property DisabledForegroundColor() As Color
#If VBC_VER < 10.0 Then
        Get
            Return themeDisabledForegroundColor
        End Get
        Set(ByVal value As Color)
            themeDisabledForegroundColor = value
        End Set
    End Property
#End If

#If VBC_VER < 10.0 Then
    Private themeAccentColors As Color()
#End If
    ''' <summary>
    ''' A list of accent colors
    ''' </summary>
    ''' <value></value>
    ''' <returns>The accent color</returns>
    ''' <remarks>The implementation in 0.7 uses 4 accent colors</remarks>
    Public Property AccentColors() As Color()
#If VBC_VER < 10.0 Then
        Get
            Return themeAccentColors
        End Get
        Set(ByVal value As Color())
            themeAccentColors = value
        End Set
    End Property
#End If

    Public Sub New()
        Me.FileName = ""
        Me.Name = ""
        Me.IsDark = False
        Me.BackgroundColor = Color.White
        Me.SectionBackgroundColor = Color.LightGray
        Me.ForegroundColor = Color.Black
        Me.AccentColors = New Color() {Color.DarkGray, Color.SlateGray, Color.Gray, Color.LightGray}
    End Sub

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
    Public Sub New(ByVal fileName As String, ByVal name As String, ByVal isDark As Boolean, ByVal backgroundColor As Color, ByVal sectionBackgroundColor As Color, ByVal foregroundColor As Color, ByVal accentColors As Color())
        Me.FileName = fileName
        Me.Name = name
        Me.IsDark = isDark
        Me.BackgroundColor = backgroundColor
        Me.SectionBackgroundColor = sectionBackgroundColor
        Me.ForegroundColor = foregroundColor
        Me.AccentColors = accentColors
    End Sub

    Public Overrides Function ToString() As String
        Return "Theme:" & CrLf & _
               "- File Name: " & FileName & CrLf & _
               "- Name: " & Name & CrLf & _
               "Check additional properties in debugger."
    End Function
End Class
