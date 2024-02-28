Imports System.Windows.Forms

Public Class OSNoRollbackErrorDlg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OSNoRollbackErrorDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = "You can't roll back to an older version"
                        Label2.Text = "No old versions were detected, because its files were not found. You may have had this version for longer than the uninstall window lets you have, or you may have deleted the files of the old version (to save space). You don't need to do anything."
                        OK_Button.Text = "OK"
                    Case "ESN"
                        Label1.Text = "No puede revertir a una versión anterior"
                        Label2.Text = "No se detectaron versiones anteriores porque sus archivos no se encontraron. Podría haber tenido esta versión por más tiempo de lo que le permite el margen de desinstalación, o podría haber eliminado los archivos de la versión anterior (para liberar espacio). No tiene que hacer nada."
                        OK_Button.Text = "Aceptar"
                    Case "FRA"
                        Label1.Text = "Vous ne pouvez pas revenir à une version antérieure"
                        Label2.Text = "Aucune ancienne version n'a été détectée, car ses fichiers n'ont pas été trouvés. Il se peut que vous possédiez cette version depuis plus longtemps que la fenêtre de désinstallation ne vous le permet, ou que vous ayez supprimé les fichiers de l'ancienne version (pour économiser de l'espace). Vous n'avez rien à faire."
                        OK_Button.Text = "OK"
                    Case "PTB", "PTG"
                        Label1.Text = "Não é possível retroceder para uma versão anterior"
                        Label2.Text = "Não foram detectadas versões antigas, porque os seus ficheiros não foram encontrados. Poderá ter esta versão há mais tempo do que a janela de desinstalação lhe permite, ou poderá ter eliminado os ficheiros da versão antiga (para poupar espaço). Não precisa de fazer nada"
                        OK_Button.Text = "OK"
                End Select
            Case 1
                Label1.Text = "You can't roll back to an older version"
                Label2.Text = "No old versions were detected, because its files were not found. You may have had this version for longer than the uninstall window lets you have, or you may have deleted the files of the old version (to save space). You don't need to do anything."
                OK_Button.Text = "OK"
            Case 2
                Label1.Text = "No puede revertir a una versión anterior"
                Label2.Text = "No se detectaron versiones anteriores porque sus archivos no se encontraron. Podría haber tenido esta versión por más tiempo de lo que le permite el margen de desinstalación, o podría haber eliminado los archivos de la versión anterior (para liberar espacio). No tiene que hacer nada."
                OK_Button.Text = "Aceptar"
            Case 3
                Label1.Text = "Vous ne pouvez pas revenir à une version antérieure"
                Label2.Text = "Aucune ancienne version n'a été détectée, car ses fichiers n'ont pas été trouvés. Il se peut que vous possédiez cette version depuis plus longtemps que la fenêtre de désinstallation ne vous le permet, ou que vous ayez supprimé les fichiers de l'ancienne version (pour économiser de l'espace). Vous n'avez rien à faire."
                OK_Button.Text = "OK"
            Case 4
                Label1.Text = "Não é possível retroceder para uma versão anterior"
                Label2.Text = "Não foram detectadas versões antigas, porque os seus ficheiros não foram encontrados. Poderá ter esta versão há mais tempo do que a janela de desinstalação lhe permite, ou poderá ter eliminado os ficheiros da versão antiga (para poupar espaço). Não precisa de fazer nada"
                OK_Button.Text = "OK"
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
        Beep()
    End Sub
End Class
