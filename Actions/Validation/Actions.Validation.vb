Public Class ValidationForm

    Private Sub ValidationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = MainForm.BackColor
        ForeColor = MainForm.ForeColor
        Height = Screen.GetWorkingArea(Me).Height
    End Sub

    Private Sub ValidationForm_Move(sender As Object, e As EventArgs) Handles MyBase.Move
        Height = Screen.GetWorkingArea(Me).Height
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        ContextMenuStrip1.Show(sender, New Point(PictureBox1.Width / 2, PictureBox1.Height / 2))
    End Sub
End Class