Imports WeifenLuo.WinFormsUI.Docking

Public Class Actions_MainForm

    Private Sub Actions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Specify the DockPanel theme using the VS2012 themes according to the program theme
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            DockPanel1.Theme = VS2012DarkTheme1
            MenuStrip1.Renderer = New ToolStripProfessionalRenderer(New MainForm.DarkModeColorTable())
            ToolStrip1.Renderer = New ToolStripProfessionalRenderer(New MainForm.DarkModeColorTable())
            MenuStrip1.BackColor = Color.FromArgb(48, 48, 48)
            MenuStrip1.ForeColor = Color.White
            ToolStrip1.BackColor = Color.FromArgb(48, 48, 48)
            ToolStrip1.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            DockPanel1.Theme = VS2012LightTheme1
            MenuStrip1.Renderer = New ToolStripProfessionalRenderer(New MainForm.LightModeColorTable())
            ToolStrip1.Renderer = New ToolStripProfessionalRenderer(New MainForm.LightModeColorTable())
            MenuStrip1.BackColor = Color.FromArgb(239, 239, 242)
            MenuStrip1.ForeColor = Color.Black
            ToolStrip1.BackColor = Color.FromArgb(239, 239, 242)
            ToolStrip1.ForeColor = Color.Black
        End If
        BackColor = MainForm.BackColor
        ForeColor = MainForm.ForeColor
        StatusStrip.BackColor = Color.FromArgb(0, 122, 204)
        StatusStrip.ForeColor = Color.White
        ActionProperties.Show(DockPanel1, DockAreas.DockTop)
    End Sub
End Class