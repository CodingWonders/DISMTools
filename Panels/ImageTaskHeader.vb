Public Class ImageTaskHeader

    Sub SetColors()
        BackColor = CurrentTheme.BackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
    End Sub

    Public Property ItemText As String
        Get
            Return ItemTitle.Text
        End Get
        Set(value As String)
            ItemTitle.Text = value
        End Set
    End Property

    Public Property ItemPicture As Image
        Get
            Return ItemPictureBox.Image
        End Get
        Set(value As Image)
            ItemPictureBox.Image = value
        End Set
    End Property
End Class
