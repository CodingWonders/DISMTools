Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class SetOSUninstWindow

    Dim uninstWindow As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If NumericUpDown1.Value = uninstWindow Then Exit Sub
        ProgressPanel.osUninstDayCount = NumericUpDown1.Value
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 88
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SetOSUninstWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Set operating system uninstall window"
                        Label1.Text = Text
                        Label2.Text = "By default, and after an OS update, you have 10 days to roll back to the previous Windows version. However, you can change this setting if you want to revert to the old OS version at a later date." & CrLf & CrLf & _
                                      "Please use the numeric slider to increase or decrease the amount of days you have to revert to the old Windows version. It must be between 2 and 60."
                        Label3.Text = "Amount of days you have to revert to the old Windows version:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Establecer margen de desinstalación del sistema operativo"
                        Label1.Text = Text
                        Label2.Text = "Por defecto, y tras una actualización del sistema operativo, tiene 10 días para revertir a la versión anterior de Windows. Sin embargo, puede cambiar esta configuración si desea revertir al SO anterior más tarde." & CrLf & CrLf & _
                                      "Utilice el deslizador numérico para aumentar o reducir el número de días que tiene para revertir a la versión anterior de Windows. Debe estar entre 2 y 60."
                        Label3.Text = "Número de días que tiene para revertir a la versión anterior de Windows:"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Définir la créneau de désinstallation du système d'exploitation"
                        Label1.Text = Text
                        Label2.Text = "Par défaut, et après une mise à jour du système d'exploitation, vous disposez de 10 jours pour revenir à la version précédente de Windows. Toutefois, vous pouvez modifier ce paramètre si vous souhaitez revenir à l'ancienne version du système d'exploitation à une date ultérieure." & CrLf & CrLf & _
                                      "Utilisez le curseur numérique pour augmenter ou diminuer le nombre de jours dont vous disposez pour revenir à l'ancienne version de Windows. Ce nombre doit être compris entre 2 et 60."
                        Label3.Text = "Nombre de jours nécessaires pour revenir à l'ancienne version de Windows :"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Text = "Configurar janela de desinstalação do sistema operativo"
                        Label1.Text = Text
                        Label2.Text = "Por predefinição, e após uma atualização do SO, tem 10 dias para reverter para a versão anterior do Windows. No entanto, pode alterar esta configuração se pretender reverter para a versão antiga do SO numa data posterior." & CrLf & CrLf & _
                                      "Utilize o cursor numérico para aumentar ou diminuir o número de dias que tem para reverter para a versão antiga do Windows. Tem de estar entre 2 e 60."
                        Label3.Text = "Quantidade de dias que tem para reverter para a versão antiga do Windows:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                    Case "ITA"
                        Text = "Impostare la finestra di disinstallazione del sistema operativo"
                        Label1.Text = Text
                        Label2.Text = "Per impostazione predefinita e dopo un aggiornamento del sistema operativo, si hanno 10 giorni per tornare alla versione precedente di Windows. Tuttavia, è possibile modificare questa impostazione se si desidera tornare alla vecchia versione del sistema operativo in un secondo momento." & CrLf & CrLf & _
                                      "Utilizzare il cursore numerico per aumentare o diminuire il numero di giorni a disposizione per tornare alla vecchia versione di Windows. Deve essere compreso tra 2 e 60."
                        Label3.Text = "Numero di giorni necessari per tornare alla vecchia versione di Windows:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                End Select
            Case 1
                Text = "Set operating system uninstall window"
                Label1.Text = Text
                Label2.Text = "By default, and after an OS update, you have 10 days to roll back to the previous Windows version. However, you can change this setting if you want to revert to the old OS version at a later date." & CrLf & CrLf & _
                              "Please use the numeric slider to increase or decrease the amount of days you have to revert to the old Windows version. It must be between 2 and 60."
                Label3.Text = "Amount of days you have to revert to the old Windows version:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Establecer margen de desinstalación del sistema operativo"
                Label1.Text = Text
                Label2.Text = "Por defecto, y tras una actualización del sistema operativo, tiene 10 días para revertir a la versión anterior de Windows. Sin embargo, puede cambiar esta configuración si desea revertir al SO anterior más tarde." & CrLf & CrLf & _
                              "Utilice el deslizador numérico para aumentar o reducir el número de días que tiene para revertir a la versión anterior de Windows. Debe estar entre 2 y 60."
                Label3.Text = "Número de días que tiene para revertir a la versión anterior de Windows:"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Définir la créneau de désinstallation du système d'exploitation"
                Label1.Text = Text
                Label2.Text = "Par défaut, et après une mise à jour du système d'exploitation, vous disposez de 10 jours pour revenir à la version précédente de Windows. Toutefois, vous pouvez modifier ce paramètre si vous souhaitez revenir à l'ancienne version du système d'exploitation à une date ultérieure." & CrLf & CrLf & _
                              "Utilisez le curseur numérique pour augmenter ou diminuer le nombre de jours dont vous disposez pour revenir à l'ancienne version de Windows. Ce nombre doit être compris entre 2 et 60."
                Label3.Text = "Nombre de jours nécessaires pour revenir à l'ancienne version de Windows :"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case 4
                Text = "Configurar janela de desinstalação do sistema operativo"
                Label1.Text = Text
                Label2.Text = "Por predefinição, e após uma atualização do SO, tem 10 dias para reverter para a versão anterior do Windows. No entanto, pode alterar esta configuração se pretender reverter para a versão antiga do SO numa data posterior." & CrLf & CrLf & _
                              "Utilize o cursor numérico para aumentar ou diminuir o número de dias que tem para reverter para a versão antiga do Windows. Tem de estar entre 2 e 60."
                Label3.Text = "Quantidade de dias que tem para reverter para a versão antiga do Windows:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
            Case 5
                Text = "Impostare la finestra di disinstallazione del sistema operativo"
                Label1.Text = Text
                Label2.Text = "Per impostazione predefinita e dopo un aggiornamento del sistema operativo, si hanno 10 giorni per tornare alla versione precedente di Windows. Tuttavia, è possibile modificare questa impostazione se si desidera tornare alla vecchia versione del sistema operativo in un secondo momento." & CrLf & CrLf & _
                              "Utilizzare il cursore numerico per aumentare o diminuire il numero di giorni a disposizione per tornare alla vecchia versione di Windows. Deve essere compreso tra 2 e 60."
                Label3.Text = "Numero di giorni necessari per tornare alla vecchia versione di Windows:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
        End Select
        ' Get the uninstall window from the registry first
        Try
            Dim osUninstReg As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\Setup")
            uninstWindow = CInt(osUninstReg.GetValue("UninstallWindow").ToString())
            osUninstReg.Close()
        Catch ex As Exception
            MsgBox(ex.ToString() & " - " & ex.Message & "(HRESULT " & ex.HResult & ")", vbOKOnly + vbCritical, Label1.Text)
            Close()
        End Try
        If (uninstWindow >= 2 And uninstWindow <= 60) Then NumericUpDown1.Value = uninstWindow
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
