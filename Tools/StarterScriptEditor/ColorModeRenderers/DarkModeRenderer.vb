Public Class DarkModeRenderer
    Inherits ToolStripProfessionalRenderer

    Public Sub New()
        MyBase.New(New DarkModeColorTable())
    End Sub

    Protected Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
        e.ArrowColor = Color.White
        MyBase.OnRenderArrow(e)
    End Sub

    Protected Overrides Sub OnRenderMenuItemBackground(ByVal e As System.Windows.Forms.ToolStripItemRenderEventArgs)
        Using b As New SolidBrush(Color.FromArgb(32, 32, 32))
            e.Graphics.FillRectangle(b, e.Item.Bounds)
        End Using
    End Sub

    Protected Overrides Sub OnRenderItemText(ByVal e As System.Windows.Forms.ToolStripItemTextRenderEventArgs)
        e.TextColor = Color.White
        MyBase.OnRenderItemText(e)
    End Sub
End Class