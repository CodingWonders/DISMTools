Imports System.IO
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars

Public Class RegistryControlPanel

    Dim PreviouslyLoadedKey As String       ' The previous key regedit had opened
    Dim RegHiveLocation As String

    Dim LoadButtonText As String = "Load"
    Dim UnloadButtonText As String = "Unload"
    Dim OpenButtonText As String = "Open"

    Dim LoadedHives As Integer = 0

    Dim CustomHiveLoaded As Boolean

    Private Sub RegistryControlPanel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Image registry hives"
                        LoadButtonText = "Load"
                        UnloadButtonText = "Unload"
                        OpenButtonText = "Open"
                        Label1.Text = "This tool lets you load the image registry hives you specify here to the local system. This lets you perform modifications to configuration stored in the Windows image. Once you have finished customizing a key from a hive, you can also unload it here:"
                        ' Labels 2-4 don't change
                        Label5.Text = "NTUSER.DAT (Default User)"
                        Label6.Text = "If you want to load a different registry hive, specify its path and click Load:"
                        Label7.Text = "Hive location:"
                        Label8.Text = "Path in the registry:"
                        Button5.Text = "Browse..."
                        GroupBox1.Text = "Load Custom Hive"
                    Case "ESN"
                        Text = "Subárboles del registro de la imagen"
                        LoadButtonText = "Cargar"
                        UnloadButtonText = "Descargar"
                        OpenButtonText = "Abrir"
                        Label1.Text = "Esta herramienta le permite cargar los subárboles del registro de la imagen que especifique aquí a su sistema local. Esto le permite modificar la configuración de la imagen de Windows. Cuando haya terminado de modificar un subárbol, lo puede descargar aquí:"
                        ' Labels 2-4 don't change
                        Label5.Text = "NTUSER.DAT (Usuario predeterminado)"
                        Label6.Text = "Si desea cargar un subárbol del registro distinto, especifique su ubicación y haga clic en Cargar:"
                        Label7.Text = "Ubicación del subárbol:"
                        Label8.Text = "Ubicación en el registro:"
                        Button5.Text = "Examinar..."
                        GroupBox1.Text = "Cargar subárbol personalizado"
                    Case "FRA"
                        Text = "Ruches du registre des images"
                        LoadButtonText = "Charger"
                        UnloadButtonText = "Décharger"
                        OpenButtonText = "Ouvrir"
                        Label1.Text = "Cet outil vous permet de charger les ruches de registre de l'image que vous spécifiez ici sur le système local. Vous pouvez ainsi modifier la configuration stockée dans l'image Windows. Une fois que vous avez fini de personnaliser une clé d'une ruche, vous pouvez également la décharger ici :"
                        ' Labels 2-4 don't change
                        Label5.Text = "NTUSER.DAT (Utilisateur par défaut)"
                        Label6.Text = "Si vous souhaitez charger un ruche de registres d'images différent, indiquez son chemin d'accès et cliquez sur Charger :"
                        Label7.Text = "Emplacement du répertoire de stockage :"
                        Label8.Text = "Chemin d'accès dans le registre :"
                        Button5.Text = "Parcourir..."
                        GroupBox1.Text = "Charger le ruche personnalisé"
                    Case "PTB", "PTG"
                        Text = "Colmeias do registo de imagens"
                        LoadButtonText = "Carregar"
                        UnloadButtonText = "Descarregar"
                        OpenButtonText = "Abrir"
                        Label1.Text = "Esta ferramenta permite-lhe carregar as colmeias do registo de imagem que especificar aqui para o sistema local. Isto permite-lhe efetuar modificações na configuração armazenada na imagem do Windows. Quando terminar de personalizar uma chave de uma colmeia, pode também descarregá-la aqui:"
                        ' Labels 2-4 don't change
                        Label5.Text = "NTUSER.DAT (Utilizador predefinido)"
                        Label6.Text = "Se pretender carregar uma colmeia de registo de imagem diferente, especifique o respetivo caminho e clique em Carregar:"
                        Label7.Text = "Localização da colmeia:"
                        Label8.Text = " Localização no registo:"
                        Button5.Text = "Procurar..."
                        GroupBox1.Text = "Carregar colmeia personalizada"
                    Case "ITA"
                        Text = "Alveari del registro delle immagini"
                        LoadButtonText = "Caricare"
                        UnloadButtonText = "Scarico"
                        OpenButtonText = "Apri"
                        Label1.Text = "Questo strumento consente di caricare sul sistema locale gli alveari del registro dell'immagine specificati qui. In questo modo è possibile modificare la configurazione memorizzata nell'immagine di Windows. Una volta terminata la personalizzazione di una chiave da un hive, è anche possibile scaricarla qui:"
                        ' Labels 2-4 don't change
                        Label5.Text = "NTUSER.DAT (Utente predefinito)"
                        Label6.Text = "Se si desidera caricare un altro hive del registro immagini, specificarne il percorso e fare clic su Caricare:"
                        Label7.Text = "Posizione dell'hive:"
                        Label8.Text = "Percorso nel registro di sistema:"
                        Button5.Text = "Sfoglia..."
                        GroupBox1.Text = "Carica l'alveare personalizzato"
                End Select
            Case 1
                Text = "Image registry hives"
                LoadButtonText = "Load"
                UnloadButtonText = "Unload"
                OpenButtonText = "Open"
                Label1.Text = "This tool lets you load the image registry hives you specify here to the local system. This lets you perform modifications to configuration stored in the Windows image. Once you have finished customizing a key from a hive, you can also unload it here:"
                ' Labels 2-4 don't change
                Label5.Text = "NTUSER.DAT (Default User)"
                Label6.Text = "If you want to load a different registry hive, specify its path and click Load:"
                Label7.Text = "Hive location:"
                Label8.Text = "Path in the registry:"
                Button5.Text = "Browse..."
                GroupBox1.Text = "Load Custom Hive"
            Case 2
                Text = "Subárboles del registro de la imagen"
                LoadButtonText = "Cargar"
                UnloadButtonText = "Descargar"
                OpenButtonText = "Abrir"
                Label1.Text = "Esta herramienta le permite cargar los subárboles del registro de la imagen que especifique aquí a su sistema local. Esto le permite modificar la configuración de la imagen de Windows. Cuando haya terminado de modificar un subárbol, lo puede descargar aquí:"
                ' Labels 2-4 don't change
                Label5.Text = "NTUSER.DAT (Usuario predeterminado)"
                Label6.Text = "Si desea cargar un subárbol del registro distinto, especifique su ubicación y haga clic en Cargar:"
                Label7.Text = "Ubicación del subárbol:"
                Label8.Text = "Ubicación en el registro:"
                Button5.Text = "Examinar..."
                GroupBox1.Text = "Cargar subárbol personalizado"
            Case 3
                Text = "Ruches du registre des images"
                LoadButtonText = "Charger"
                UnloadButtonText = "Décharger"
                OpenButtonText = "Ouvrir"
                Label1.Text = "Cet outil vous permet de charger les ruches de registre de l'image que vous spécifiez ici sur le système local. Vous pouvez ainsi modifier la configuration stockée dans l'image Windows. Une fois que vous avez fini de personnaliser une clé d'une ruche, vous pouvez également la décharger ici :"
                ' Labels 2-4 don't change
                Label5.Text = "NTUSER.DAT (Utilisateur par défaut)"
                Label6.Text = "Si vous souhaitez charger un ruche de registres d'images différent, indiquez son chemin d'accès et cliquez sur Charger :"
                Label7.Text = "Emplacement du répertoire de stockage :"
                Label8.Text = "Chemin d'accès dans le registre :"
                Button5.Text = "Parcourir..."
                GroupBox1.Text = "Charger le ruche personnalisé"
            Case 4
                Text = "Colmeias do registo de imagens"
                LoadButtonText = "Carregar"
                UnloadButtonText = "Descarregar"
                OpenButtonText = "Abrir"
                Label1.Text = "Esta ferramenta permite-lhe carregar as colmeias do registo de imagem que especificar aqui para o sistema local. Isto permite-lhe efetuar modificações na configuração armazenada na imagem do Windows. Quando terminar de personalizar uma chave de uma colmeia, pode também descarregá-la aqui:"
                ' Labels 2-4 don't change
                Label5.Text = "NTUSER.DAT (Utilizador predefinido)"
                Label6.Text = "Se pretender carregar uma colmeia de registo de imagem diferente, especifique o respetivo caminho e clique em Carregar:"
                Label7.Text = "Localização da colmeia:"
                Label8.Text = " Localização no registo:"
                Button5.Text = "Procurar..."
                GroupBox1.Text = "Carregar colmeia personalizada"
            Case 5
                Text = "Alveari del registro delle immagini"
                LoadButtonText = "Caricare"
                UnloadButtonText = "Scarico"
                OpenButtonText = "Apri"
                Label1.Text = "Questo strumento consente di caricare sul sistema locale gli alveari del registro dell'immagine specificati qui. In questo modo è possibile modificare la configurazione memorizzata nell'immagine di Windows. Una volta terminata la personalizzazione di una chiave da un hive, è anche possibile scaricarla qui:"
                ' Labels 2-4 don't change
                Label5.Text = "NTUSER.DAT (Utente predefinito)"
                Label6.Text = "Se si desidera caricare un altro hive del registro immagini, specificarne il percorso e fare clic su Caricare:"
                Label7.Text = "Posizione dell'hive:"
                Label8.Text = "Percorso nel registro di sistema:"
                Button5.Text = "Sfoglia..."
                GroupBox1.Text = "Carica l'alveare personalizzato"
        End Select
        Button1.Text = OpenButtonText
        Button2.Text = OpenButtonText
        Button3.Text = OpenButtonText
        Button4.Text = OpenButtonText
        Button6.Text = UnloadButtonText
        Button8.Text = LoadButtonText
        Button9.Text = LoadButtonText
        Button10.Text = LoadButtonText
        Button11.Text = LoadButtonText
        Button12.Text = LoadButtonText
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        TextBox1.BackColor = BackColor
        TextBox2.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Try
            Dim LastKeyReg As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Applets\Regedit")
            PreviouslyLoadedKey = LastKeyReg.GetValue("LastKey")
            LastKeyReg.Close()
        Catch ex As Exception
            ' Could not grab Last Key
        End Try
        RegHiveLocation = Path.Combine(MainForm.MountDir, "Windows\system32\config")
        If RegHiveLocation <> "" Then
            OpenFileDialog1.InitialDirectory = RegHiveLocation
        End If
    End Sub

    Function ModifyRegistryHives(Load As Boolean, HiveLocation As String) As Boolean
        Dim regLoaderProc As New Process()
        regLoaderProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
        Dim regName As String = "z" & Path.GetFileNameWithoutExtension(HiveLocation)
        If Load Then
            regLoaderProc.StartInfo.Arguments = "load HKLM\" & regName & " " & Quote & HiveLocation & Quote
        Else
            regLoaderProc.StartInfo.Arguments = "unload HKLM\" & regName
        End If
        regLoaderProc.StartInfo.CreateNoWindow = True
        regLoaderProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        regLoaderProc.Start()
        regLoaderProc.WaitForExit()
        If regLoaderProc.ExitCode = 0 Then
            Return True
        Else
            Debug.WriteLine("The registry process failed with exit code " & Hex(regLoaderProc.ExitCode))
            Return False
        End If
    End Function

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If ModifyRegistryHives(Button9.Text = LoadButtonText, RegHiveLocation & "\SOFTWARE") Then
            If Button9.Text = LoadButtonText Then
                Button9.Text = UnloadButtonText
                LoadedHives += 1
            Else
                Button9.Text = LoadButtonText
                LoadedHives -= 1
            End If
            Button1.Enabled = (Button9.Text <> LoadButtonText)
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If ModifyRegistryHives(Button10.Text = LoadButtonText, RegHiveLocation & "\SYSTEM") Then
            If Button10.Text = LoadButtonText Then
                Button10.Text = UnloadButtonText
                LoadedHives += 1
            Else
                Button10.Text = LoadButtonText
                LoadedHives -= 1
            End If
            Button2.Enabled = (Button10.Text <> LoadButtonText)
        End If
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If ModifyRegistryHives(Button11.Text = LoadButtonText, RegHiveLocation & "\DEFAULT") Then
            If Button11.Text = LoadButtonText Then
                Button11.Text = UnloadButtonText
                LoadedHives += 1
            Else
                Button11.Text = LoadButtonText
                LoadedHives -= 1
            End If
            Button3.Enabled = (Button11.Text <> LoadButtonText)
        End If
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If ModifyRegistryHives(Button12.Text = LoadButtonText, MainForm.MountDir & "\Users\Default\NTUSER.DAT") Then
            If Button12.Text = LoadButtonText Then
                Button12.Text = UnloadButtonText
                LoadedHives += 1
            Else
                Button12.Text = LoadButtonText
                LoadedHives -= 1
            End If
            Button4.Enabled = (Button12.Text <> LoadButtonText)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & "HKEY_LOCAL_MACHINE\zSOFTWARE" & Quote & " /f").WaitForExit()
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & "HKEY_LOCAL_MACHINE\zSYSTEM" & Quote & " /f").WaitForExit()
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & "HKEY_LOCAL_MACHINE\zDEFAULT" & Quote & " /f").WaitForExit()
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & "HKEY_LOCAL_MACHINE\zNTUSER" & Quote & " /f").WaitForExit()
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub RegistryControlPanel_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim msg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg = "The registry hives need to be unloaded to close this window. Do you want to unload them now?"
                    Case "ESN"
                        msg = "Los subárboles del registro deben ser descargados para cerrar esta ventana. ¿Desea descargarlos ahora?"
                    Case "FRA"
                        msg = "Les ruches de registre doivent être déchargées pour fermer cette fenêtre. Voulez-vous les décharger maintenant ?"
                    Case "PTB", "PTG"
                        msg = "As colmeias do registo têm de ser descarregadas para fechar esta janela. Deseja descarregá-las agora?"
                    Case "ITA"
                        msg = "Gli alveari del registro devono essere scaricati per chiudere questa finestra. Volete scaricarli ora?"
                End Select
            Case 1
                msg = "The registry hives need to be unloaded to close this window. Do you want to unload them now?"
            Case 2
                msg = "Los subárboles del registro deben ser descargados para cerrar esta ventana. ¿Desea descargarlos ahora?"
            Case 3
                msg = "Les ruches de registre doivent être déchargées pour fermer cette fenêtre. Voulez-vous les décharger maintenant ?"
            Case 4
                msg = "As colmeias do registo têm de ser descarregadas para fechar esta janela. Deseja descarregá-las agora?"
            Case 5
                msg = "Gli alveari del registro devono essere scaricati per chiudere questa finestra. Volete scaricarli ora?"
        End Select
        If LoadedHives > 0 Or CustomHiveLoaded Then
            If MsgBox(msg, vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                If Button9.Text <> LoadButtonText Then Button9.PerformClick()
                If Button10.Text <> LoadButtonText Then Button10.PerformClick()
                If Button11.Text <> LoadButtonText Then Button11.PerformClick()
                If Button12.Text <> LoadButtonText Then Button12.PerformClick()
                If CustomHiveLoaded Then Button6.PerformClick()
            Else
                e.Cancel = True
                Exit Sub
            End If
            If LoadedHives > 0 Or CustomHiveLoaded Then
                ' Some hives could not be unloaded
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg = "Some hives could not be unloaded. Please unload them before closing this window."
                            Case "ESN"
                                msg = "Algunos subárboles no pudieron ser descargados. Descárguelos antes de cerrar esta ventana."
                            Case "FRA"
                                msg = "Certaines ruches n'ont pas pu être déchargées. Veuillez les décharger avant de fermer cette fenêtre."
                            Case "PTB", "PTG"
                                msg = "Algumas colmeias não puderam ser descarregadas. Por favor, descarregue-as antes de fechar esta janela."
                            Case "ITA"
                                msg = "Non è stato possibile scaricare alcune arnie. Si prega di scaricarle prima di chiudere questa finestra."
                        End Select
                    Case 1
                        msg = "Some hives could not be unloaded. Please unload them before closing this window."
                    Case 2
                        msg = "Algunos subárboles no pudieron ser descargados. Descárguelos antes de cerrar esta ventana."
                    Case 3
                        msg = "Certaines ruches n'ont pas pu être déchargées. Veuillez les décharger avant de fermer cette fenêtre."
                    Case 4
                        msg = "Algumas colmeias não puderam ser descarregadas. Por favor, descarregue-as antes de fechar esta janela."
                    Case 5
                        msg = "Non è stato possibile scaricare alcune arnie. Si prega di scaricarle prima di chiudere questa finestra."
                End Select
                MsgBox(msg, vbOKOnly + vbCritical, Text)
                e.Cancel = True
                Exit Sub
            End If
        End If

        ' Set last key back
        If PreviouslyLoadedKey <> "" Then
            Try
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & PreviouslyLoadedKey & Quote & " /f")
            Catch ex As Exception
                ' Could not grab Last Key
            End Try
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
        TextBox2.Text = "HKEY_LOCAL_MACHINE\z" & Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If TextBox1.Text = "" Then Exit Sub
        ' Load Custom Hive
        If ModifyRegistryHives(True, TextBox1.Text) Then
            TextBox1.Enabled = False
            Button8.Enabled = False
            Button7.Enabled = True
            Button6.Enabled = True
            Button5.Enabled = False
            CustomHiveLoaded = True
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        ' Launch regedit on the loaded Hive
        Try
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & TextBox2.Text & Quote & " /f").WaitForExit()
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ' Unload Custom Hive
        If ModifyRegistryHives(False, TextBox1.Text) Then
            TextBox1.Enabled = True
            Button8.Enabled = True
            Button7.Enabled = False
            Button6.Enabled = False
            Button5.Enabled = True
            CustomHiveLoaded = False
        End If
    End Sub
End Class