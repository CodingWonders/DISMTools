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
        If MainForm.IsImageMounted Then
            CheckBox1.Enabled = True
            Label2.Visible = True
        Else
            CheckBox1.Enabled = False
            CheckBox1.Checked = False
            Label2.Visible = False
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
