Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class SingleImageIndexError

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SingleImageIndexError_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label1.Text = "This image seems to have only one index"
                        Label2.Text = "You cannot switch to other indexes. If you want to save the image changes, you can do so using a new, separate index."
                        LinkLabel1.Text = "To know more about the indexes of an image, or some of its specific properties, go to " & Quote & "Commands > Image management > Get image information" & Quote & ", or click here"
                        LinkLabel1.LinkArea = New LinkArea(150, 4)
                        OK_Button.Text = "OK"
                    Case "ESN"
                        Label1.Text = "Esta imagen parece tener solo un índice"
                        Label2.Text = "No puede cambiar a otros índices. Si desea guardar los cambios de la imagen, puede hacerlo usando un índice nuevo."
                        LinkLabel1.Text = "Para saber más acerca de los índices de una imagen, o algunas de sus propiedades específicas, ve a " & Quote & "Comandos > Administración de la imagen > Obtener información de imagen" & Quote & ", o haga clic aquí"
                        LinkLabel1.LinkArea = New LinkArea(185, 4)
                        OK_Button.Text = "Aceptar"
                End Select
            Case 1
                Label1.Text = "This image seems to have only one index"
                Label2.Text = "You cannot switch to other indexes. If you want to save the image changes, you can do so using a new, separate index."
                LinkLabel1.Text = "To know more about the indexes of an image, or some of its specific properties, go to " & Quote & "Commands > Image management > Get image information" & Quote & ", or click here"
                LinkLabel1.LinkArea = New LinkArea(150, 4)
                OK_Button.Text = "OK"
            Case 2
                Label1.Text = "Esta imagen parece tener solo un índice"
                Label2.Text = "No puede cambiar a otros índices. Si desea guardar los cambios de la imagen, puede hacerlo usando un índice nuevo."
                LinkLabel1.Text = "Para saber más acerca de los índices de una imagen, o algunas de sus propiedades específicas, ve a " & Quote & "Comandos > Administración de la imagen > Obtener información de imagen" & Quote & ", o haga clic aquí"
                LinkLabel1.LinkArea = New LinkArea(185, 4)
                OK_Button.Text = "Aceptar"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            Panel1.BackColor = Color.FromArgb(31, 31, 31)
            Panel2.BackColor = Color.FromArgb(31, 31, 31)
            Label1.ForeColor = Color.FromArgb(0, 122, 204)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            Panel1.BackColor = Color.FromArgb(238, 238, 242)
            Panel2.BackColor = Color.FromArgb(238, 238, 242)
            Label1.ForeColor = Color.FromArgb(0, 51, 153)
        End If
        Beep()
    End Sub
End Class
