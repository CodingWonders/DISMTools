Partial Public Class ActionProperties
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    Public Sub ActionProperties()
        InitializeComponent()
    End Sub

    Private Sub ActionProperties_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        BackColor = Color.FromArgb(37, 37, 38)
        ForeColor = Actions_MainForm.ForeColor
    End Sub
End Class