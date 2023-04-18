Partial Public Class ActionProperties
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    Public Sub ActionProperties()
        InitializeComponent()
    End Sub

    Private Sub ActionProperties_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        BackColor = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), Color.FromArgb(37, 37, 38), Color.FromArgb(246, 246, 246))
        ForeColor = Actions_MainForm.ForeColor
    End Sub
End Class