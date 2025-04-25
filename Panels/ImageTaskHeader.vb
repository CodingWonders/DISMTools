Public Class ImageTaskHeader

    Public Enum ColorMode
        Dark
        Light
    End Enum

    Public Property ItemText As String
        Get
            Return ItemTitle.Text
        End Get
        Set(value As String)
            ItemTitle.Text = Value
        End Set
    End Property

    Public Property ItemPicture As Image
        Get
            Return ItemPictureBox.Image
        End Get
        Set(value As Image)
            ItemPictureBox.Image = Value
        End Set
    End Property

    Public Property ItemColor As ColorMode
        Get
            If BackColor = Color.FromArgb(48, 48, 48) Then
                Return ColorMode.Dark
            Else
                Return ColorMode.Light
            End If
        End Get
        Set(value As ColorMode)
            If value = ColorMode.Dark Then
                BackColor = Color.FromArgb(48, 48, 48)
                ForeColor = Color.White
            Else
                BackColor = Color.White
                ForeColor = Color.Black
            End If
        End Set
    End Property
End Class
