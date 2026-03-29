Public Class LightModeRenderer
    Inherits ToolStripProfessionalRenderer

    Public Sub New()
        MyBase.New(New LightModeColorTable())
    End Sub

    Protected Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
        e.ArrowColor = Color.Black
        MyBase.OnRenderArrow(e)
    End Sub
End Class
