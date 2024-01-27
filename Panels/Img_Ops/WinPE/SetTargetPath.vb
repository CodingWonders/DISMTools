Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32

Public Class SetPETargetPath

    Sub GetTargetPath()
        Using reg As New Process
            reg.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
            reg.StartInfo.Arguments = "load HKLM\PE_SOFT " & Quote & MainForm.MountDir & "\Windows\system32\config\SOFTWARE" & Quote
            reg.StartInfo.CreateNoWindow = True
            reg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            reg.Start()
            reg.WaitForExit()
            Try
                Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("PE_SOFT\Microsoft\Windows NT\CurrentVersion\WinPE", False)
                TextBox1.Text = regKey.GetValue("InstRoot", "").ToString()
                regKey.Close()
            Catch ex As Exception

            End Try
            ' Unload registry hives
            reg.StartInfo.Arguments = "unload HKLM\PE_SOFT"
            reg.Start()
            reg.WaitForExit()
        End Using
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ' Detect the following requirements before proceeding:
        ' - Right length (3-32 chars)
        ' - Starts with any drive letter other than A or B
        ' - Is absolute
        ' - Drive letter is followed by :
        ' - Doesn't contain spaces or quotation marks
        Dim msg As String = ""
        If TextBox1.TextLength < 3 Or TextBox1.TextLength > 32 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "The target path must be at least 3 characters and no longer than 32 characters"
                        Case "ESN"
                            msg = "La ruta de destino debe tener al menos 3 caracteres y no más de 32"
                        Case "FRA"
                            msg = "Le chemin cible doit être composé d'au moins 3 caractères et d'au plus 32 caractères."
                    End Select
                Case 1
                    msg = "The target path must be at least 3 characters and no longer than 32 characters"
                Case 2
                    msg = "La ruta de destino debe tener al menos 3 caracteres y no más de 32"
                Case 3
                    msg = "Le chemin cible doit être composé d'au moins 3 caractères et d'au plus 32 caractères."
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        If TextBox1.Text.StartsWith("A", StringComparison.OrdinalIgnoreCase) Or TextBox1.Text.StartsWith("B", StringComparison.OrdinalIgnoreCase) Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "The target path must start with any letter other than A or B"
                        Case "ESN"
                            msg = "La ruta de destino debe empezar con cualquier letra que no sea A o B"
                        Case "FRA"
                            msg = "Le chemin cible doit commencer par une lettre autre que A ou B."
                    End Select
                Case 1
                    msg = "The target path must start with any letter other than A or B"
                Case 2
                    msg = "La ruta de destino debe empezar con cualquier letra que no sea A o B"
                Case 3
                    msg = "Le chemin cible doit commencer par une lettre autre que A ou B."
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        If Not TextBox1.Text.Chars(1).Equals(":"c) Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "A drive letter must be followed by :"
                        Case "ESN"
                            msg = "Una letra de disco debe estar seguida por :"
                        Case "FRA"
                            msg = "Une lettre de disque doit être suivie de :"
                    End Select
                Case 1
                    msg = "A drive letter must be followed by :"
                Case 2
                    msg = "Una letra de disco debe estar seguida por :"
                Case 3
                    msg = "Une lettre de disque doit être suivie de :"
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        If TextBox1.Text.Contains(".\") Or TextBox1.Text.Contains("..\") Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "The target path must be absolute, and must not contain relative elements"
                        Case "ESN"
                            msg = "La ruta de destino debe ser absoluta, y no debe contener elementos relativos"
                        Case "FRA"
                            msg = "Le chemin cible doit être absolu et ne doit pas contenir d'éléments relatifs."
                    End Select
                Case 1
                    msg = "The target path must be absolute, and must not contain relative elements"
                Case 2
                    msg = "La ruta de destino debe ser absoluta, y no debe contener elementos relativos"
                Case 3
                    msg = "Le chemin cible doit être absolu et ne doit pas contenir d'éléments relatifs."
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        If TextBox1.Text.Contains(" ") Or TextBox1.Text.Contains(Quote) Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "The target path must not contain spaces or quotation marks"
                        Case "ESN"
                            msg = "La ruta de destino no debe contener espacios o comillas"
                        Case "FRA"
                            msg = "Le chemin cible ne doit pas contenir d'espaces ou de guillemets."
                    End Select
                Case 1
                    msg = "The target path must not contain spaces or quotation marks"
                Case 2
                    msg = "La ruta de destino no debe contener espacios o comillas"
                Case 3
                    msg = "Le chemin cible ne doit pas contenir d'espaces ou de guillemets."
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        ProgressPanel.peNewTargetPath = TextBox1.Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 84
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SetTargetPath_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Set Windows PE target path"
                        Label1.Text = Text
                        Label2.Text = "The target path is a directory where the Windows PE files will be copied to in order to boot to the environment. Please specify a target path and click OK."
                        Label3.Text = "Target path:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Establecer ruta de destino de Windows PE"
                        Label1.Text = Text
                        Label2.Text = "La ruta de destino es una carpeta donde los archivos de Windows PE serán copiados para iniciar el entorno. Especifique una ruta de destino y haga clic en Aceptar."
                        Label3.Text = "Ruta de destino:"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Configurer le chemin cible de Windows PE"
                        Label1.Text = Text
                        Label2.Text = "Le chemin cible est un répertoire dans lequel les fichiers Windows PE seront copiés afin de démarrer dans l'environnement. Veuillez indiquer un chemin cible et cliquer sur OK."
                        Label3.Text = "Chemin cible :"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                End Select
            Case 1
                Text = "Set Windows PE target path"
                Label1.Text = Text
                Label2.Text = "The target path is a directory where the Windows PE files will be copied to in order to boot to the environment. Please specify a target path and click OK."
                Label3.Text = "Target path:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Establecer ruta de destino de Windows PE"
                Label1.Text = Text
                Label2.Text = "La ruta de destino es una carpeta donde los archivos de Windows PE serán copiados para iniciar el entorno. Especifique una ruta de destino y haga clic en Aceptar."
                Label3.Text = "Ruta de destino:"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Configurer le chemin cible de Windows PE"
                Label1.Text = Text
                Label2.Text = "Le chemin cible est un répertoire dans lequel les fichiers Windows PE seront copiés afin de démarrer dans l'environnement. Veuillez indiquer un chemin cible et cliquer sur OK."
                Label3.Text = "Chemin cible :"
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
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor

        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        GetTargetPath()
    End Sub
End Class
