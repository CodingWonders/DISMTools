Imports System.Windows.Forms

Public Class SaveProjectQuestionDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Yes_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub No_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles No_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.No
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SaveProjectQuestionDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = "Do you want to save the changes of this project?"
                        Label2.Text = "If you shut down or restart your system without unmounting the images, you will need to reload the servicing session."
                        CheckBox1.Text = "Save changes and unmount image"
                        Yes_Button.Text = "Yes"
                        No_Button.Text = "No"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Label1.Text = "¿Desea guardar los cambios de este proyecto?"
                        Label2.Text = "Si apaga o reinicia su sistema sin desmontar las imágenes, necesitará recargar la sesión de servicio."
                        CheckBox1.Text = "Guardar cambios y desmontar imagen"
                        Yes_Button.Text = "Sí"
                        No_Button.Text = "No"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Label1.Text = "Souhaitez-vous sauvegarder les modifications apportées à ce projet ?"
                        Label2.Text = "Si vous arrêtez ou redémarrez votre système sans démonter les images, vous devrez recharger la session de maintenance."
                        CheckBox1.Text = "Sauvegarder les modifications et démonter l'image"
                        Yes_Button.Text = "Oui"
                        No_Button.Text = "Non"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Label1.Text = "Pretende guardar as alterações deste projeto?"
                        Label2.Text = "Se desligar ou reiniciar o sistema sem desmontar as imagens, terá de recarregar a sessão de manutenção."
                        CheckBox1.Text = "Guardar as alterações e desmontar a imagem"
                        Yes_Button.Text = "Sim"
                        No_Button.Text = "Não"
                        Cancel_Button.Text = "Cancelar"
                    Case "ITA"
                        Label1.Text = "Volete salvare le modifiche di questo progetto?"
                        Label2.Text = "Se si spegne o si riavvia il sistema senza smontare le immagini, sarà necessario ricaricare la sessione di assistenza"
                        CheckBox1.Text = "Salva le modifiche e smonta l'immagine"
                        Yes_Button.Text = "Sì"
                        No_Button.Text = "No"
                        Cancel_Button.Text = "Annulla"
                End Select
            Case 1
                Label1.Text = "Do you want to save the changes of this project?"
                Label2.Text = "If you shut down or restart your system without unmounting the images, you will need to reload the servicing session."
                CheckBox1.Text = "Save changes and unmount image"
                Yes_Button.Text = "Yes"
                No_Button.Text = "No"
                Cancel_Button.Text = "Cancel"
            Case 2
                Label1.Text = "¿Desea guardar los cambios de este proyecto?"
                Label2.Text = "Si apaga o reinicia su sistema sin desmontar las imágenes, necesitará recargar la sesión de servicio."
                CheckBox1.Text = "Guardar cambios y desmontar imagen"
                Yes_Button.Text = "Sí"
                No_Button.Text = "No"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Label1.Text = "Souhaitez-vous sauvegarder les modifications apportées à ce projet ?"
                Label2.Text = "Si vous arrêtez ou redémarrez votre système sans démonter les images, vous devrez recharger la session de maintenance."
                CheckBox1.Text = "Sauvegarder les modifications et démonter l'image"
                Yes_Button.Text = "Oui"
                No_Button.Text = "Non"
                Cancel_Button.Text = "Annuler"
            Case 4
                Label1.Text = "Pretende guardar as alterações deste projeto?"
                Label2.Text = "Se desligar ou reiniciar o sistema sem desmontar as imagens, terá de recarregar a sessão de manutenção."
                CheckBox1.Text = "Guardar as alterações e desmontar a imagem"
                Yes_Button.Text = "Sim"
                No_Button.Text = "Não"
                Cancel_Button.Text = "Cancelar"
            Case 5
                Label1.Text = "Volete salvare le modifiche di questo progetto?"
                Label2.Text = "Se si spegne o si riavvia il sistema senza smontare le immagini, sarà necessario ricaricare la sessione di assistenza"
                CheckBox1.Text = "Salva le modifiche e smonta l'immagine"
                Yes_Button.Text = "Sì"
                No_Button.Text = "No"
                Cancel_Button.Text = "Annulla"
        End Select
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Panel1.BackColor = CurrentTheme.SectionBackgroundColor
        Label1.ForeColor = Color.FromArgb(0, 122, 204)
        If MainForm.IsImageMounted Then
            CheckBox1.Enabled = True
            Label2.Visible = True
        Else
            CheckBox1.Enabled = False
            CheckBox1.Checked = False
            Label2.Visible = False
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
    End Sub
End Class
