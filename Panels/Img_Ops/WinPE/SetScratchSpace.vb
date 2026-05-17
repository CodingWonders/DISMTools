Imports System.Windows.Forms
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO

Public Class SetPEScratchSpace
    Implements IImageTaskDialog

    Sub GetScratchSpace()
        DynaLog.LogMessage("Preparing to get Windows PE settings...")
        DynaLog.LogMessage("Loading SYSTEM hive of WinPE image...")
        Dim regExitCode As Integer = RegistryHelper.LoadRegistryHive(Path.Combine(MainForm.MountDir, "Windows", "system32", "config", "SYSTEM"), "HKLM\PE_SYS")
        DynaLog.LogMessage("REG hive exit code: " & Hex(regExitCode))
        Try
            DynaLog.LogMessage("Getting scratch space...")
            Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("PE_SYS\ControlSet001\Services\FBWF", False)
            DynaLog.LogMessage("Scratch space: " & regKey.GetValue("WinPECacheThreshold", 0) & " MB")
            If regKey.GetValue("WinPECacheThreshold", "").ToString() <> "" Then
                If Not ComboBox1.Items.Contains(regKey.GetValue("WinPECacheThreshold", "").ToString()) Then
                    Label5.Visible = True
                End If
            End If
            ComboBox1.SelectedText = regKey.GetValue("WinPECacheThreshold", "").ToString()
            regKey.Close()
        Catch ex As Exception

        End Try
        DynaLog.LogMessage("Unloading hives...")
        ' Unload registry hives
        RegistryHelper.UnloadRegistryHive("HKLM\PE_SYS")
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.peNewScratchSpace = ComboBox1.SelectedItem
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 83
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Opening scratch space configuration dialog...")
        If MainForm.ImgBW.IsBusy Then
            DynaLog.LogMessage("Background processes are still busy.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        Return True
    End Function

    Private Sub SetPEScratchSpace_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Set Windows PE scratch space"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "The scratch space is the amount of writable space available on the Windows PE system volume when its contents are copied to memory. Please specify a scratch space amount and click OK."
                        Label3.Text = "Scratch space:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Establecer espacio temporal de Windows PE"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "El espacio temporal es la cantidad de espacio disponible que se puede escribir en el volumen del sistema de Windows PE cuando sus contenidos son copiados a la memoria. Especifique una cantidad de espacio temporal y haga clic en Aceptar."
                        Label3.Text = "Espacio temporal:"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Configurer l'espace temporaire de Windows PE"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "L'espace temporaire est la quantité d'espace accessible en écriture disponible sur le volume du système Windows PE lorsque son contenu est copié dans la mémoire. Veuillez spécifier une quantité d'espace temporaire et cliquez sur OK."
                        Label3.Text = "Espace temporaire :"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Text = "Configurar o espaço temporário do Windows PE"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "O espaço de rascunho é a quantidade de espaço gravável disponível no volume do sistema Windows PE quando o seu conteúdo é copiado para a memória. Especifique uma quantidade de espaço de rascunho e clique em OK."
                        Label3.Text = "Espaço temporário:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                    Case "ITA"
                        Text = "Imposta spazio temporaneo Windows PE"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Lo spazio temporaneo è la quantità di spazio scrivibile disponibile sul volume di sistema Windows PE quando il suo contenuto viene copiato in memoria. Specificare la quantità di spazio temporaneo e fare clic su OK"
                        Label3.Text = "Spazio temporaneo:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                End Select
            Case 1
                Text = "Set Windows PE scratch space"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "The scratch space is the amount of writable space available on the Windows PE system volume when its contents are copied to memory. Please specify a scratch space amount and click OK."
                Label3.Text = "Scratch space:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Establecer espacio temporal de Windows PE"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "El espacio temporal es la cantidad de espacio disponible que se puede escribir en el volumen del sistema de Windows PE cuando sus contenidos son copiados a la memoria. Especifique una cantidad de espacio temporal y haga clic en Aceptar."
                Label3.Text = "Espacio temporal:"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Configurer l'espace temporaire de Windows PE"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "L'espace temporaire est la quantité d'espace accessible en écriture disponible sur le volume du système Windows PE lorsque son contenu est copié dans la mémoire. Veuillez spécifier une quantité d'espace temporaire et cliquez sur OK."
                Label3.Text = "Espace temporaire :"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case 4
                Text = "Configurar o espaço temporário do Windows PE"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "O espaço de rascunho é a quantidade de espaço gravável disponível no volume do sistema Windows PE quando o seu conteúdo é copiado para a memória. Especifique uma quantidade de espaço de rascunho e clique em OK."
                Label3.Text = "Espaço temporário:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
            Case 5
                Text = "Imposta spazio temporaneo Windows PE"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Lo spazio temporaneo è la quantità di spazio scrivibile disponibile sul volume di sistema Windows PE quando il suo contenuto viene copiato in memoria. Specificare la quantità di spazio temporaneo e fare clic su OK"
                Label3.Text = "Spazio temporaneo:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = ForeColor
        Label5.Visible = False
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        GetScratchSpace()
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub
End Class
