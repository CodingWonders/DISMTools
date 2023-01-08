Imports System.Threading

Public Class BGProcNotify

    Dim opacityFade As Single

    Private Sub BGProcNotify_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Opacity = 100
        If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
            Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - (7 + MainForm.StatusStrip.Height))
        ElseIf Environment.OSVersion.Version.Major = 6 Then
            If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - (7 + MainForm.StatusStrip.Height))
            Else
                Location = New Point(MainForm.Left, MainForm.Top + MainForm.StatusStrip.Top - MainForm.StatusStrip.Height)
            End If
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(37, 37, 38)
            ForeColor = Color.White
            PictureBox1.Image = New Bitmap(My.Resources.close_glyph_dark)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.White
            ForeColor = Color.Black
            PictureBox1.Image = New Bitmap(My.Resources.close_glyph)
        End If
        Timer1.Enabled = True
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For Me.opacityFade = 1 To 0 Step -0.01
            Opacity = opacityFade
            Refresh()
            Thread.Sleep(10)
        Next opacityFade
        Timer1.Enabled = False
        Timer1.Stop()
        Dispose()
        Close()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        ControlPaint.DrawBorder(e.Graphics, Panel1.ClientRectangle, Color.FromArgb(0, 122, 204), ButtonBorderStyle.Solid)
    End Sub
End Class