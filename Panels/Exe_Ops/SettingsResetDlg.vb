Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class SettingsResetDlg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SettingsResetDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Reset preferences"
                        Label1.Text = "If you proceed, the settings will be reset to their default values. Once this process is complete, you'll return to the main program window." & CrLf & CrLf & "Do you want to proceed?"
                        OK_Button.Text = "Yes"
                        Cancel_Button.Text = "No"
                    Case "ESN"
                        Text = "Restablecer preferencias"
                        Label1.Text = "Si continúa, las configuraciones serán restablecidas a sus valores predeterminados. Cuando este proceso haya completado, regresará a la ventana principal." & CrLf & CrLf & "¿Desea continuar?"
                        OK_Button.Text = "Sí"
                        Cancel_Button.Text = "No"
                    Case "FRA"
                        Text = "Réinitialiser les préférences"
                        Label1.Text = "Si vous continuez, les paramètres seront réinitialisés à leurs valeurs par défaut. Une fois ce processus terminé, vous reviendrez à la fenêtre principale du programme." & CrLf & CrLf & "Voulez-vous continuer ?"
                        OK_Button.Text = "Oui"
                        Cancel_Button.Text = "Non"
                    Case "PTB", "PTG"
                        Text = "Repor preferências"
                        Label1.Text = "Se prosseguir, as configurações serão repostas para os valores predefinidos. Quando este processo estiver concluído, regressará à janela principal do programa." & CrLf & CrLf & "Deseja continuar?"
                        OK_Button.Text = "Sim"
                        Cancel_Button.Text = "Não"
                End Select
            Case 1
                Text = "Reset preferences"
                Label1.Text = "If you proceed, the settings will be reset to their default values. Once this process is complete, you'll return to the main program window." & CrLf & CrLf & "Do you want to proceed?"
                OK_Button.Text = "Yes"
                Cancel_Button.Text = "No"
            Case 2
                Text = "Restablecer preferencias"
                Label1.Text = "Si continúa, las configuraciones serán restablecidas a sus valores predeterminados. Cuando este proceso haya completado, regresará a la ventana principal." & CrLf & CrLf & "¿Desea continuar?"
                OK_Button.Text = "Sí"
                Cancel_Button.Text = "No"
            Case 3
                Text = "Réinitialiser les préférences"
                Label1.Text = "Si vous continuez, les paramètres seront réinitialisés à leurs valeurs par défaut. Une fois ce processus terminé, vous reviendrez à la fenêtre principale du programme." & CrLf & CrLf & "Voulez-vous continuer ?"
                OK_Button.Text = "Oui"
                Cancel_Button.Text = "Non"
            Case 4
                Text = "Repor preferências"
                Label1.Text = "Se prosseguir, as configurações serão repostas para os valores predefinidos. Quando este processo estiver concluído, regressará à janela principal do programa." & CrLf & CrLf & "Deseja continuar?"
                OK_Button.Text = "Sim"
                Cancel_Button.Text = "Não"
        End Select
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(Handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Beep()
    End Sub
End Class
