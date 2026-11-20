Public Class DarkModeRenderer
    Inherits ToolStripProfessionalRenderer

    Public Sub New()
        MyBase.New(New DarkModeColorTable())
    End Sub

    Protected Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
        e.ArrowColor = CurrentTheme.ForegroundColor
        MyBase.OnRenderArrow(e)
    End Sub
End Class