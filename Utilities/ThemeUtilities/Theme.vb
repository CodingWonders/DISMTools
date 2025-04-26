Public Class Theme

    Private _themeName As String
    Private _themeIsDark As Boolean
    Private _themeBackgroundColor, _themeSectionBackgroundColor, _themeForegroundColor As Color
    Private _themeAccentColors() As Color

    Public Property Name As String
        Get
            Return _themeName
        End Get
        Set(value As String)
            _themeName = value
        End Set
    End Property

    Public Property IsDark As Boolean
        Get
            Return _themeIsDark
        End Get
        Set(value As Boolean)
            _themeIsDark = value
        End Set
    End Property

    Public Property BackgroundColor As Color
        Get
            Return _themeBackgroundColor
        End Get
        Set(value As Color)

        End Set
    End Property

    Public Property SectionBackgroundColor As Color
        Get
            Return _themeSectionBackgroundColor
        End Get
        Set(value As Color)
            _themeSectionBackgroundColor = value
        End Set
    End Property

    Public Property ForegroundColor As Color
        Get
            Return _themeForegroundColor
        End Get
        Set(value As Color)
            _themeForegroundColor = value
        End Set
    End Property

    Public Property AccentColors As Color()
        Get
            Return _themeAccentColors
        End Get
        Set(value As Color())
            _themeAccentColors = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(name As String, isDark As Boolean, backgroundColor As Color, sectionBackgroundColor As Color, foregroundColor As Color, accentColors As Color())
        Me._themeName = name
        Me._themeIsDark = isDark
        Me._themeBackgroundColor = backgroundColor
        Me._themeSectionBackgroundColor = sectionBackgroundColor
        Me._themeForegroundColor = foregroundColor
        Me._themeAccentColors = accentColors
    End Sub

End Class
