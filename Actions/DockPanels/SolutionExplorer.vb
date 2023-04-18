Public Class SolutionExplorer
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    Private Sub SolutionExplorer_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        BackColor = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), Color.FromArgb(37, 37, 38), Color.FromArgb(246, 246, 246))
        ForeColor = Actions_MainForm.ForeColor
        ToolStrip1.Renderer = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), New ToolStripProfessionalRenderer(New MainForm.DarkModeColorTable()), New ToolStripProfessionalRenderer(New MainForm.LightModeColorTable()))
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TreeView1.BackColor = BackColor
        TreeView1.ForeColor = ForeColor
    End Sub
End Class
