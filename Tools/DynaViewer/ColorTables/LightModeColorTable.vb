''' <summary>
''' Set colors on any surface with the "Professional" RenderMode in light mode
''' </summary>
''' <remarks></remarks>
Public Class LightModeColorTable
    Inherits ProfessionalColorTable

    Public Overrides ReadOnly Property ToolStripBorder As Color
        Get
            Return Color.FromArgb(239, 239, 242)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
        Get
            Return Color.FromArgb(239, 239, 242)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientBegin As Color
        Get
            Return Color.FromArgb(239, 239, 242)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientMiddle As Color
        Get
            Return Color.FromArgb(239, 239, 242)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripGradientEnd As Color
        Get
            Return Color.FromArgb(239, 239, 242)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelected As Color
        Get
            Return Color.FromArgb(254, 254, 254)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemBorder As Color
        Get
            Return Color.FromArgb(239, 239, 239)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuBorder As Color
        Get
            Return Color.FromArgb(239, 239, 239)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
        Get
            Return Color.FromArgb(254, 254, 254)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
        Get
            Return Color.FromArgb(254, 254, 254)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
        Get
            Return Color.FromArgb(231, 232, 236)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
        Get
            Return Color.FromArgb(231, 232, 236)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
        Get
            Return Color.FromArgb(231, 232, 236)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin As Color
        Get
            Return Color.FromArgb(231, 232, 236)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd As Color
        Get
            Return Color.FromArgb(231, 232, 236)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
        Get
            Return Color.FromArgb(231, 232, 236)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
        Get
            Return Color.FromArgb(231, 232, 236)
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
        Get
            Return Color.FromArgb(231, 232, 236)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonSelectedGradientBegin As Color
        Get
            Return Color.FromArgb(254, 254, 254)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonSelectedGradientMiddle As Color
        Get
            Return Color.FromArgb(254, 254, 254)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonSelectedGradientEnd As Color
        Get
            Return Color.FromArgb(254, 254, 254)
        End Get
    End Property

    Public Overrides ReadOnly Property ButtonSelectedBorder As Color
        Get
            Return Color.FromArgb(254, 254, 254)
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