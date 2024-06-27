Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports System.Threading

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
                    Case "ENU", "ENG"
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
                    Case "FRA"
                        Label1.Text = "Cette image semble n'avoir qu'un seul index"
                        Label2.Text = "Vous ne pouvez pas passer à d'autres index. Si vous souhaitez sauvegarder les modifications apportées à l'image, vous pouvez le faire en utilisant un nouvel index distinct."
                        LinkLabel1.Text = "Pour en savoir plus sur les index d'une image, ou sur certaines de ses propriétés spécifiques, allez dans " & Quote & "Commandes > Gestion des images > Obtenir des informations sur l'image" & Quote & ", ou cliquez ici"
                        LinkLabel1.LinkArea = New LinkArea(214, 3)
                        OK_Button.Text = "OK"
                    Case "PTB", "PTG"
                        Label1.Text = "Esta imagem parece ter apenas um índice"
                        Label2.Text = "Não é possível mudar para outros índices. Se quiser guardar as alterações da imagem, pode fazê-lo utilizando um índice novo e separado."
                        LinkLabel1.Text = "Para saber mais sobre os índices de uma imagem, ou sobre algumas das suas propriedades específicas, aceda a " & Quote & "Comandos > Gestão de imagens > Obter informações sobre a imagem" & Quote & ", ou clique aqui"
                        LinkLabel1.LinkArea = New LinkArea(185, 4)
                        OK_Button.Text = "OK"
                    Case "ITA"
                        Label1.Text = "Questa immagine sembra avere un solo indice"
                        Label2.Text = "Non è possibile passare ad altri indici. Se si desidera salvare le modifiche all'immagine, è possibile farlo utilizzando un nuovo indice separato"
                        LinkLabel1.Text = "Per saperne di più sugli indici di un'immagine o su alcune sue proprietà specifiche, andare su " & Quote & "Comandi > Gestione immagini > Ottieni informazioni sull'immagine" & Quote & ", oppure fare clic qui"
                        LinkLabel1.LinkArea = New LinkArea(176, 3)
                        OK_Button.Text = "OK"
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
            Case 3
                Label1.Text = "Cette image semble n'avoir qu'un seul index"
                Label2.Text = "Vous ne pouvez pas passer à d'autres index. Si vous souhaitez sauvegarder les modifications apportées à l'image, vous pouvez le faire en utilisant un nouvel index distinct."
                LinkLabel1.Text = "Pour en savoir plus sur les index d'une image, ou sur certaines de ses propriétés spécifiques, allez dans " & Quote & "Commandes > Gestion des images > Obtenir des informations sur l'image" & Quote & ", ou cliquez ici"
                LinkLabel1.LinkArea = New LinkArea(214, 3)
                OK_Button.Text = "OK"
            Case 4
                Label1.Text = "Esta imagem parece ter apenas um índice"
                Label2.Text = "Não é possível mudar para outros índices. Se quiser guardar as alterações da imagem, pode fazê-lo utilizando um índice novo e separado."
                LinkLabel1.Text = "Para saber mais sobre os índices de uma imagem, ou sobre algumas das suas propriedades específicas, aceda a " & Quote & "Comandos > Gestão de imagens > Obter informações sobre a imagem" & Quote & ", ou clique aqui"
                LinkLabel1.LinkArea = New LinkArea(185, 4)
                OK_Button.Text = "OK"
            Case 5
                Label1.Text = "Questa immagine sembra avere un solo indice"
                Label2.Text = "Non è possibile passare ad altri indici. Se si desidera salvare le modifiche all'immagine, è possibile farlo utilizzando un nuovo indice separato"
                LinkLabel1.Text = "Per saperne di più sugli indici di un'immagine o su alcune sue proprietà specifiche, andare su " & Quote & "Comandi > Gestione immagini > Ottieni informazioni sull'immagine" & Quote & ", oppure fare clic qui"
                LinkLabel1.LinkArea = New LinkArea(176, 3)
                OK_Button.Text = "OK"
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
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Beep()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Visible = False
        GetImgInfoDlg.RadioButton1.Checked = True
        GetImgInfoDlg.RadioButton2.Checked = False
        GetImgInfoDlg.ShowDialog(MainForm)
    End Sub
End Class
