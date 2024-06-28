Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class OrphanedMountedImgDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OrphanedMountedImgDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = "This image needs a servicing session reload"
                        Label2.Text = "The project that has been loaded contains an orphaned image (an image which needs to be remounted)" & CrLf & "The image will be remounted when you click " & Quote & "OK" & Quote & ". This should not affect your modifications to the image, and should also not take a long time." & CrLf & CrLf & "NOTE: if you click " & Quote & "Cancel" & Quote & ", the project will be unloaded"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Label1.Text = "Esta imagen necesita una recarga de la sesión de servicio"
                        Label2.Text = "El proyecto que ha sido cargado contiene una imagen huérfana (una imagen que debe ser remontada)" & CrLf & "La imagen será remontada al hacer clic en " & Quote & "Aceptar" & Quote & ". Esto no debería afectar las modificaciones a la imagen, y no debería tardar mucho tiempo." & CrLf & CrLf & "NOTA: si hace clic en " & Quote & "Cancelar" & Quote & ", el proyecto será descargado"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Label1.Text = "Cette image nécessite un rechargement de la session de maintenance"
                        Label2.Text = "Le projet qui a été chargé contient une image orpheline (une image qui doit être remontée)" & CrLf & "L'image sera remontée lorsque vous cliquerez sur " & Quote & "OK" & Quote & ". Cela ne devrait pas affecter vos modifications de l'image et ne devrait pas prendre beaucoup de temps." & CrLf & CrLf & "NOTE: si vous cliquez sur " & Quote & "Annuler" & Quote & ", le projet sera déchargé."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Label1.Text = "Esta imagem precisa de ser recarregada numa sessão de manutenção"
                        Label2.Text = "O projeto que foi carregado contém uma imagem órfã (uma imagem que precisa de ser montada novamente)" & CrLf & "A imagem será montada novamente quando clicar em " & Quote & "OK" & Quote & ". Isto não deve afetar as suas modificações na imagem e também não deve demorar muito tempo." & CrLf & CrLf & "NOTA: se clicar em " & Quote & "Cancel" & Quote & ", o projeto será descarregado"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                    Case "ITA"
                        Label1.Text = "Questa immagine necessita di una sessione di assistenza per essere ricaricata"
                        Label2.Text = "Il progetto che è stato caricato contiene un'immagine orfana (un'immagine che deve essere rimontata)" & CrLf & "L'immagine verrà rimontata quando si fa clic su " & Quote & "OK" & Quote & ". Questa operazione non dovrebbe influire sulle modifiche apportate all'immagine e non dovrebbe richiedere molto tempo." & CrLf & CrLf & "NOTA: se si fa clic su " & Quote & "Cancel" & Quote & ", il progetto verrà scaricato"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                End Select
            Case 1
                Label1.Text = "This image needs a servicing session reload"
                Label2.Text = "The project that has been loaded contains an orphaned image (an image which needs to be remounted)" & CrLf & "The image will be remounted when you click " & Quote & "OK" & Quote & ". This should not affect your modifications to the image, and should also not take a long time." & CrLf & CrLf & "NOTE: if you click " & Quote & "Cancel" & Quote & ", the project will be unloaded"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Label1.Text = "Esta imagen necesita una recarga de la sesión de servicio"
                Label2.Text = "El proyecto que ha sido cargado contiene una imagen huérfana (una imagen que debe ser remontada)" & CrLf & "La imagen será remontada al hacer clic en " & Quote & "Aceptar" & Quote & ". Esto no debería afectar las modificaciones a la imagen, y no debería tardar mucho tiempo." & CrLf & CrLf & "NOTA: si hace clic en " & Quote & "Cancelar" & Quote & ", el proyecto será descargado"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Label1.Text = "Cette image nécessite un rechargement de la session de maintenance"
                Label2.Text = "Le projet qui a été chargé contient une image orpheline (une image qui doit être remontée)" & CrLf & "L'image sera remontée lorsque vous cliquerez sur " & Quote & "OK" & Quote & ". Cela ne devrait pas affecter vos modifications de l'image et ne devrait pas prendre beaucoup de temps." & CrLf & CrLf & "NOTE: si vous cliquez sur " & Quote & "Annuler" & Quote & ", le projet sera déchargé."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case 4
                Label1.Text = "Esta imagem precisa de ser recarregada numa sessão de manutenção"
                Label2.Text = "O projeto que foi carregado contém uma imagem órfã (uma imagem que precisa de ser montada novamente)" & CrLf & "A imagem será montada novamente quando clicar em " & Quote & "OK" & Quote & ". Isto não deve afetar as suas modificações na imagem e também não deve demorar muito tempo." & CrLf & CrLf & "NOTA: se clicar em " & Quote & "Cancel" & Quote & ", o projeto será descarregado"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
            Case 5
                Label1.Text = "Questa immagine necessita di una sessione di assistenza per essere ricaricata"
                Label2.Text = "Il progetto che è stato caricato contiene un'immagine orfana (un'immagine che deve essere rimontata)" & CrLf & "L'immagine verrà rimontata quando si fa clic su " & Quote & "OK" & Quote & ". Questa operazione non dovrebbe influire sulle modifiche apportate all'immagine e non dovrebbe richiedere molto tempo." & CrLf & CrLf & "NOTA: se si fa clic su " & Quote & "Cancel" & Quote & ", il progetto verrà scaricato"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            Panel1.BackColor = Color.FromArgb(31, 31, 31)
            Label1.ForeColor = Color.FromArgb(0, 122, 204)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            Panel1.BackColor = Color.FromArgb(238, 238, 242)
            Label1.ForeColor = Color.FromArgb(0, 51, 153)
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
