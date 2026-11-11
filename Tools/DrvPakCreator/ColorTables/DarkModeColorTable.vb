''' <summary>
''' Set colors on any surface with the "Professional" RenderMode in dark mode
''' </summary>
''' <remarks></remarks>
Public Class DarkModeColorTable
    Inherits ProfessionalColorTable

    Public Overrides ReadOnly Property ToolStripBorder As Color
        Get
            Return CurrentTheme.BackgroundColor
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
        Get
            Return CurrentTheme.BackgroundColor
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientBegin As Color
        Get
            Return CurrentTheme.BackgroundColor
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientMiddle As Color
        Get
            Return CurrentTheme.BackgroundColor
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientEnd As Color
        Get
            Return CurrentTheme.BackgroundColor
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelected As Color
        Get
            Return CurrentTheme.BackgroundColor
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemBorder As Color
        Get
            Return CurrentTheme.BackgroundColor
        End Get
    End Property

    Public Overrides ReadOnly Property MenuBorder As Color
        Get
            Return CurrentTheme.BackgroundColor
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
        Get
            Return Color.FromArgb(62, 62, 64)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
        Get
            Return Color.FromArgb(62, 62, 64)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
        Get
            Return Color.FromArgb(27, 27, 28)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
        Get
            Return Color.FromArgb(27, 27, 28)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
        Get
            Return Color.FromArgb(27, 27, 28)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin As Color
        Get
            Return Color.FromArgb(27, 27, 28)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd As Color
        Get
            Return Color.FromArgb(27, 27, 28)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
        Get
            Return Color.FromArgb(27, 27, 28)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
        Get
            Return Color.FromArgb(27, 27, 28)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
        Get
            Return Color.FromArgb(27, 27, 28)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripPanelGradientBegin As Color
        Get
            Return Color.FromArgb(48, 48, 48)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonSelectedGradientBegin As Color
        Get
            Return Color.FromArgb(62, 62, 64)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonSelectedGradientMiddle As Color
        Get
            Return Color.FromArgb(62, 62, 64)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonSelectedGradientEnd As Color
        Get
            Return Color.FromArgb(62, 62, 64)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonSelectedBorder As Color
        Get
            Return Color.FromArgb(62, 62, 64)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonPressedGradientBegin As Color
        Get
            Return Color.FromArgb(0, 122, 204)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonPressedGradientMiddle As Color
        Get
            Return Color.FromArgb(0, 122, 204)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonPressedGradientEnd As Color
        Get
            Return Color.FromArgb(0, 122, 204)
        End Get
    End Property
End Class