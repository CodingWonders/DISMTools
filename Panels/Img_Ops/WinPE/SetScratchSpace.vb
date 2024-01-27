Imports System.Windows.Forms
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars

Public Class SetPEScratchSpace

    Sub GetScratchSpace()
        Using reg As New Process
            reg.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
            reg.StartInfo.Arguments = "load HKLM\PE_SYS " & Quote & MainForm.MountDir & "\Windows\system32\config\SYSTEM" & Quote
            reg.StartInfo.CreateNoWindow = True
            reg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            reg.Start()
            reg.WaitForExit()
            Try
                Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("PE_SYS\ControlSet001\Services\FBWF", False)
                If regKey.GetValue("WinPECacheThreshold", "").ToString() <> "" Then
                    If Not ComboBox1.Items.Contains(regKey.GetValue("WinPECacheThreshold", "").ToString()) Then
                        Label5.Visible = True
                    End If
                End If
                ComboBox1.SelectedText = regKey.GetValue("WinPECacheThreshold", "").ToString()
                regKey.Close()
            Catch ex As Exception

            End Try
            ' Unload registry hives
            reg.StartInfo.Arguments = "unload HKLM\PE_SYS"
            reg.Start()
            reg.WaitForExit()
        End Using
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
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

    Private Sub SetPEScratchSpace_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Set Windows PE scratch space"
                        Label1.Text = Text
                        Label2.Text = "The scratch space is the amount of writable space available on the Windows PE system volume when its contents are copied to memory. Please specify a scratch space amount and click OK."
                        Label3.Text = "Scratch space:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Establecer espacio temporal de Windows PE"
                        Label1.Text = Text
                        Label2.Text = "El espacio temporal es la cantidad de espacio disponible que se puede escribir en el volumen del sistema de Windows PE cuando sus contenidos son copiados a la memoria. Especifique una cantidad de espacio temporal y haga clic en Aceptar."
                        Label3.Text = "Espacio temporal:"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Configurer l'espace temporaire de Windows PE"
                        Label1.Text = Text
                        Label2.Text = "L'espace temporaire est la quantité d'espace accessible en écriture disponible sur le volume du système Windows PE lorsque son contenu est copié dans la mémoire. Veuillez spécifier une quantité d'espace temporaire et cliquez sur OK."
                        Label3.Text = "Espace temporaire :"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                End Select
            Case 1
                Text = "Set Windows PE scratch space"
                Label1.Text = Text
                Label2.Text = "The scratch space is the amount of writable space available on the Windows PE system volume when its contents are copied to memory. Please specify a scratch space amount and click OK."
                Label3.Text = "Scratch space:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Establecer espacio temporal de Windows PE"
                Label1.Text = Text
                Label2.Text = "El espacio temporal es la cantidad de espacio disponible que se puede escribir en el volumen del sistema de Windows PE cuando sus contenidos son copiados a la memoria. Especifique una cantidad de espacio temporal y haga clic en Aceptar."
                Label3.Text = "Espacio temporal:"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Configurer l'espace temporaire de Windows PE"
                Label1.Text = Text
                Label2.Text = "L'espace temporaire est la quantité d'espace accessible en écriture disponible sur le volume du système Windows PE lorsque son contenu est copié dans la mémoire. Veuillez spécifier une quantité d'espace temporaire et cliquez sur OK."
                Label3.Text = "Espace temporaire :"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ComboBox1.ForeColor = ForeColor
        Label5.Visible = False
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        GetScratchSpace()
    End Sub
End Class
