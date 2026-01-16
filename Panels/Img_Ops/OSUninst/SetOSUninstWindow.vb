Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class SetOSUninstWindow
    Implements IImageTaskDialog

    Dim uninstWindow As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
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

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        If MainForm.OnlineManagement Then
            DynaLog.LogMessage("The active installation is being managed right now. Checking if it can uninstall an OS...")
            If Not MainForm.CheckOSUninstallCapability() Then
                DynaLog.LogMessage("No rollbacks/uninstallations can be performed.")
                OSNoRollbackErrorDlg.ShowDialog(MainForm)
                Return False
            End If
        Else
            DynaLog.LogMessage("The active installation is not being managed right now.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is only supported on online installations", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción solo está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action est seulement prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
                        Case "PTB", "PTG"
                            MsgBox("Esta ação só é suportada em instalações online", vbOKOnly + vbCritical, Text)
                        Case "ITA"
                            MsgBox("Questa azione è supportata solo su installazioni attive", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is only supported on online installations", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción solo está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action est seulement prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
                Case 4
                    MsgBox("Esta ação só é suportada em instalações online", vbOKOnly + vbCritical, Text)
                Case 5
                    MsgBox("Questa azione è supportata solo su installazioni attive", vbOKOnly + vbCritical, Text)
            End Select
            Return False
        End If
        Return True
    End Function

    Private Sub SetOSUninstWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
            Exit Sub
        End If
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
            DynaLog.LogMessage("Getting OS uninstall window...")
            Dim osUninstReg As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\Setup")
            uninstWindow = CInt(osUninstReg.GetValue("UninstallWindow").ToString())
            osUninstReg.Close()
        Catch ex As Exception
            MsgBox(ex.ToString() & " - " & ex.Message & "(HRESULT " & ex.HResult & ")", vbOKOnly + vbCritical, Label1.Text)
            Close()
        End Try
        DynaLog.LogMessage("Uninstall window: " & uninstWindow)
        DynaLog.LogMessage("Checking value...")
        If (uninstWindow >= 2 And uninstWindow <= 60) Then NumericUpDown1.Value = uninstWindow
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Win10Title.BackColor = CurrentTheme.BackgroundColor
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
    End Sub
End Class
