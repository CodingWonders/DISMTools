Public Class WimFileSourceControl

    Sub SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        PictureBox1.Image = GetGlyphResource("image_glyph")
    End Sub

    Public Property ImageFile As String
        Get
            Return Label1.Text
        End Get
        Set(value As String)
            Label1.Text = value
        End Set
    End Property

    Public Property ImageIndex As Integer
        Get
            Dim IndexInt As Integer
            If Integer.TryParse(Label2.Text, IndexInt) Then Return IndexInt
            Return 1
        End Get
        Set(value As Integer)
            Label2.Text = value
        End Set
    End Property

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Visible = False
    End Sub
End Class
