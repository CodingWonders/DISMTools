Imports System.Drawing.Drawing2D
Public Class SplashScreen

    Dim opacityFade As Single

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        Dim brush As LinearGradientBrush = New LinearGradientBrush(Panel1.ClientRectangle, Color.Transparent, Color.Black, LinearGradientMode.Horizontal)
        e.Graphics.FillRectangle(brush, Panel1.ClientRectangle)
    End Sub

    Private Sub SplashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        Refresh()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For Me.opacityFade = 0 To 1 Step 0.05
            Opacity = opacityFade
            Refresh()
        Next opacityFade
        Timer1.Enabled = False
        Timer1.Stop()
    End Sub
End Class