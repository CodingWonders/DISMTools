Imports Microsoft.VisualBasic.ControlChars

Public Class Theme
    ''' <summary>
    ''' Backing field for <see>FileName</see>
    ''' </summary>
    Private m_FileName As String

    ''' <summary>
    ''' The file name of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The theme file name</returns>
    ''' <remarks></remarks>
    Public Property FileName() As String
        Get
            Return m_FileName
        End Get
        Set(ByVal value As String)
            m_FileName = value
        End Set
    End Property

    Private m_Name As String

    ''' <summary>
    ''' The name of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The theme name</returns>
    ''' <remarks>Not to be confused with <see>FileName</see></remarks>
    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            m_Name = value
        End Set
    End Property

    Private m_IsDark As Boolean

    ''' <summary>
    ''' Determines whether a theme uses dark glyphs
    ''' </summary>
    ''' <value>True = the theme is expected to use dark glyphs; False = the theme is expected to use light glyphs</value>
    ''' <returns>Whether a theme uses dark glyphs</returns>
    ''' <remarks></remarks>
    Public Property IsDark() As Boolean
        Get
            Return m_IsDark
        End Get
        Set(ByVal value As Boolean)
            m_IsDark = value
        End Set
    End Property

    Private m_BackgroundColor As Color

    ''' <summary>
    ''' The main background color of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The private attribute</returns>
    ''' <remarks></remarks>
    Public Property BackgroundColor() As Color
        Get
            Return m_BackgroundColor
        End Get
        Set(ByVal value As Color)
            m_BackgroundColor = value
        End Set
    End Property

    Private m_SectionBackgroundColor As Color

    ''' <summary>
    ''' The background color of the theme for inner sections
    ''' </summary>
    ''' <value></value>
    ''' <returns>The section background color</returns>
    ''' <remarks></remarks>
    Public Property SectionBackgroundColor() As Color
        Get
            Return m_SectionBackgroundColor
        End Get
        Set(ByVal value As Color)
            m_SectionBackgroundColor = value
        End Set
    End Property

    Private m_ForegroundColor As Color

    ''' <summary>
    ''' The main foreground color of the theme
    ''' </summary>
    ''' <value></value>
    ''' <returns>The main foreground color</returns>
    ''' <remarks></remarks>
    Public Property ForegroundColor() As Color
        Get
            Return m_ForegroundColor
        End Get
        Set(ByVal value As Color)
            m_ForegroundColor = value
        End Set
    End Property

    Private m_DisabledForegroundColor As Color

    ''' <summary>
    ''' The foreground color for disabled items (for example, inactive headers in a set of pages)
    ''' </summary>
    ''' <value></value>
    ''' <returns>The disabled foreground color</returns>
    ''' <remarks>This attribute is calculated automatically by <see>ThemeHelper.LoadThemes</see></remarks>
    Public Property DisabledForegroundColor() As Color
        Get
            Return m_DisabledForegroundColor
        End Get
        Set(ByVal value As Color)
            m_DisabledForegroundColor = value
        End Set
    End Property

    Private m_AccentColors As Color()

    ''' <summary>
    ''' A list of accent colors
    ''' </summary>
    ''' <value></value>
    ''' <returns>The accent color</returns>
    ''' <remarks>The implementation in 0.7 uses 4 accent colors</remarks>
    Public Property AccentColors() As Color()
        Get
            Return m_AccentColors
        End Get
        Set(ByVal value As Color())
            m_AccentColors = value
        End Set
    End Property

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
