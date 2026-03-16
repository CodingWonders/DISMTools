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

    Private Sub ImageTaskHeader_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ItemTitle.Size = WindowHelper.ScaleSizeLogical(ItemTitle.Width, ItemTitle.Height)
        ItemPictureBox.Location = WindowHelper.ScalePositionLogical(ItemPictureBox.Left, ItemPictureBox.Top)
    End Sub
End Class
