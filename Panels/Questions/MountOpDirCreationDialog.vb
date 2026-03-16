Imports System.Windows.Forms

Public Class MountOpDirCreationDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.No
        Me.Close()
    End Sub

    Private Sub MountOpDirCreationDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = ImgMount.ImageTaskHeader1.ItemText
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = "Do you want to create the mount directory?"
                        OK_Button.Text = "Yes"
                        Cancel_Button.Text = "No"
                    Case "ESN"
                        Label1.Text = "¿Desea crear el directorio de montaje?"
                        OK_Button.Text = "Sí"
                        Cancel_Button.Text = "No"
                    Case "FRA"
                        Label1.Text = "Voulez-vous créer le répertoire de montage ?"
                        OK_Button.Text = "Oui"
                        Cancel_Button.Text = "Non"
                    Case "PTB", "PTG"
                        Label1.Text = "Deseja criar o diretório de montagem?"
                        OK_Button.Text = "Sim"
                        Cancel_Button.Text = "Não"
                    Case "ITA"
                        Label1.Text = "Vuoi creare la directory di montaggio?"
                        OK_Button.Text = "Sì"
                        Cancel_Button.Text = "No"
                End Select
            Case 1
                Label1.Text = "Do you want to create the mount directory?"
                OK_Button.Text = "Yes"
                Cancel_Button.Text = "No"
            Case 2
                Label1.Text = "¿Desea crear el directorio de montaje?"
                OK_Button.Text = "Sí"
                Cancel_Button.Text = "No"
            Case 3
                Label1.Text = "Voulez-vous créer le répertoire de montage ?"
                OK_Button.Text = "Oui"
                Cancel_Button.Text = "Non"
            Case 4
                Label1.Text = "Deseja criar o diretório de montagem?"
                OK_Button.Text = "Sim"
                Cancel_Button.Text = "Não"
            Case 5
                Label1.Text = "Vuoi creare la directory di montaggio?"
                OK_Button.Text = "Sì"
                Cancel_Button.Text = "No"
        End Select
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Panel1.BackColor = CurrentTheme.SectionBackgroundColor
        Label1.ForeColor = Color.FromArgb(0, 122, 204)
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
    End Sub
End Class
