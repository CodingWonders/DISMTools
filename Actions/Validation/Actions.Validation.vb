Public Class ValidationForm

    Private Sub ValidationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Height = Screen.GetWorkingArea(Me).Height
    End Sub

    Private Sub ValidationForm_Move(sender As Object, e As EventArgs) Handles MyBase.Move
        Height = Screen.GetWorkingArea(Me).Height
    End Sub
End Class