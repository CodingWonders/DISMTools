Public Class ImageTaskHeader

    Private isScaled As Boolean

    Sub SetColors()
        BackColor = CurrentTheme.BackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
    End Sub

    Sub HideWindowTitle(WindowHandle As IntPtr)
        Try
            Dim refForm As Form = CType(Control.FromHandle(WindowHandle), Form)
            If refForm IsNot Nothing Then refForm.Text = ""
        Catch ex As Exception

        End Try
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
        If Not isScaled Then
            ItemTitle.Size = WindowHelper.ScaleSizeLogical(ItemTitle.Width, ItemTitle.Height)
            ItemPictureBox.Location = WindowHelper.ScalePositionLogical(ItemPictureBox.Left, ItemPictureBox.Top)
            isScaled = True
        End If
    End Sub
End Class
