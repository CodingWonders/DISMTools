Imports System.Drawing.Drawing2D
Public Class SplashScreen

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        Dim brush As LinearGradientBrush = New LinearGradientBrush(Panel1.ClientRectangle, Color.Transparent, Color.Black, LinearGradientMode.Horizontal)
        e.Graphics.FillRectangle(brush, Panel1.ClientRectangle)
    End Sub

    Private Sub SplashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Refresh()
    End Sub
End Class