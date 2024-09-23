Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class UnattendMgr

    Private Sub OK_Button_Click(sender As Object, e As EventArgs)
        DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If FolderBrowserDialog1.SelectedPath.Contains("unattend_xml") Then
                TextBox1.Text = FolderBrowserDialog1.SelectedPath
            Else
                TextBox1.Text = Path.Combine(FolderBrowserDialog1.SelectedPath, "unattend_xml")
            End If
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" AndAlso Directory.Exists(TextBox1.Text) Then
            ScanForUnattendFiles(TextBox1.Text)
        End If
    End Sub

    Sub ScanForUnattendFiles(folderPath As String)
        Dim UnattendFiles() As String
        Dim errorMsg As String = ""
        ListView1.Items.Clear()
        Try
            If Directory.Exists(folderPath) Then
                If folderPath.Contains("unattend_xml") Then
                    UnattendFiles = Directory.GetFiles(folderPath, "*.xml", SearchOption.AllDirectories)
                Else
                    If Directory.Exists(Path.Combine(folderPath, "unattend_xml")) Then
                        UnattendFiles = Directory.GetFiles(Path.Combine(folderPath, "unattend_xml"), "*.xml", SearchOption.AllDirectories)
                    Else
                        UnattendFiles = Directory.GetFiles(folderPath, "*.xml", SearchOption.AllDirectories)
                    End If
                End If
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                errorMsg = "The folder path does not exist"
                            Case "ESN"
                                errorMsg = "La carpeta especificada no existe"
                            Case "FRA"
                                errorMsg = "Le chemin d'accès au dossier n'existe pas"
                            Case "PTB", "PTG"
                                errorMsg = "A localização da pasta não existe"
                            Case "ITA"
                                errorMsg = "Il percorso della cartella non esiste"
                        End Select
                    Case 1
                        errorMsg = "The folder path does not exist"
                    Case 2
                        errorMsg = "La carpeta especificada no existe"
                    Case 3
                        errorMsg = "Le chemin d'accès au dossier n'existe pas"
                    Case 4
                        errorMsg = "A localização da pasta não existe"
                    Case 5
                        errorMsg = "Il percorso della cartella non esiste"
                End Select
                Throw New Exception(errorMsg)
            End If
            If UnattendFiles.Length > 0 Then
                For Each xmlFile In UnattendFiles
                    ListView1.Items.Add(New ListViewItem(New String() {Path.GetFileName(xmlFile), File.GetCreationTime(xmlFile), File.GetLastWriteTime(xmlFile), File.GetLastAccessTime(xmlFile)}))
                Next
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                errorMsg = "Search concluded with no elements found"
                            Case "ESN"
                                errorMsg = "La búsqueda ha finalizado. No se han encontrado elementos"
                            Case "FRA"
                                errorMsg = "La recherche s'est terminée sans qu'aucun élément n'ait été trouvé"
                            Case "PTB", "PTG"
                                errorMsg = "Pesquisa concluída sem elementos encontrados"
                            Case "ITA"
                                errorMsg = "La ricerca si è conclusa con nessun elemento trovato"
                        End Select
                    Case 1
                        errorMsg = "Search concluded with no elements found"
                    Case 2
                        errorMsg = "La búsqueda ha finalizado. No se han encontrado elementos"
                    Case 3
                        errorMsg = "La recherche s'est terminée sans qu'aucun élément n'ait été trouvé"
                    Case 4
                        errorMsg = "Pesquisa concluída sem elementos encontrados"
                    Case 5
                        errorMsg = "La ricerca si è conclusa con nessun elemento trovato"
                End Select
                Throw New Exception(errorMsg)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbCritical, Text)
        End Try
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        ActionsTLP.Enabled = (ListView1.SelectedItems.Count = 1)
        Button4.Enabled = (MainForm.isProjectLoaded And Not (MainForm.OnlineManagement Or MainForm.OfflineManagement))
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Process.Start(Path.Combine(TextBox1.Text, ListView1.FocusedItem.SubItems(0).Text))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", "/select," & Quote & Path.Combine(TextBox1.Text, ListView1.FocusedItem.SubItems(0).Text) & Quote)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ApplyUnattendFile.TextBox1.Text = Path.Combine(TextBox1.Text, ListView1.FocusedItem.SubItems(0).Text)
        WindowState = FormWindowState.Minimized
        ApplyUnattendFile.ShowDialog(MainForm)
        WindowState = FormWindowState.Normal
    End Sub

    Private Sub UnattendMgr_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Unattended answer file manager"
                        Label1.Text = "Project path:"
                        Button1.Text = "Browse..."
                        Button2.Text = "Open file"
                        Button3.Text = "Open file location"
                        Button4.Text = "Apply to image..."
                        ListView1.Columns(0).Text = "File name"
                        ListView1.Columns(1).Text = "Created"
                        ListView1.Columns(2).Text = "Last modified"
                        ListView1.Columns(3).Text = "Last accessed"
                    Case "ESN"
                        Text = "Administrador de archivos de respuesta desatendida"
                        Label1.Text = "Ubicación de proyecto:"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Abrir archivo"
                        Button3.Text = "Abrir ubicación del archivo"
                        Button4.Text = "Aplicar a una imagen..."
                        ListView1.Columns(0).Text = "Nombre de archivo"
                        ListView1.Columns(1).Text = "Creado"
                        ListView1.Columns(2).Text = "Última modificación"
                        ListView1.Columns(3).Text = "Último acceso"
                    Case "FRA"
                        Text = "Gestionnaire de fichiers de réponse sans surveillance"
                        Label1.Text = "Chemin du projet :"
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Ouvrir le fichier"
                        Button3.Text = "Ouvrir l'emplacement du fichier"
                        Button4.Text = "Appliquer à l'image..."
                        ListView1.Columns(0).Text = "Nom du fichier"
                        ListView1.Columns(1).Text = "Créé"
                        ListView1.Columns(2).Text = "Dernière modification"
                        ListView1.Columns(3).Text = "Dernier accès"
                    Case "PTB", "PTG"
                        Text = "Gestor de ficheiros de resposta não assistida"
                        Label1.Text = "Localização do projeto:"
                        Button1.Text = "Procurar..."
                        Button2.Text = "Abrir ficheiro"
                        Button3.Text = "Abrir localização do ficheiro"
                        Button4.Text = "Aplicar à imagem..."
                        ListView1.Columns(0).Text = "Nome do ficheiro"
                        ListView1.Columns(1).Text = "Criado"
                        ListView1.Columns(2).Text = "Última modificação"
                        ListView1.Columns(3).Text = "Último acesso"
                    Case "ITA"
                        Text = "Gestore file di risposta non presidiato"
                        Label1.Text = "Posizione del progetto:"
                        Button1.Text = "Sfoglia..."
                        Button2.Text = "Apri file"
                        Button3.Text = "Apri il percorso del file"
                        Button4.Text = "Applica all'immagine..."
                        ListView1.Columns(0).Text = "Nome del file"
                        ListView1.Columns(1).Text = "Creato"
                        ListView1.Columns(2).Text = "Ultima modifica"
                        ListView1.Columns(3).Text = "Ultimo accesso"
                End Select
            Case 1
                Text = "Unattended answer file manager"
                Label1.Text = "Project path:"
                Button1.Text = "Browse..."
                Button2.Text = "Open file"
                Button3.Text = "Open file location"
                Button4.Text = "Apply to image..."
                ListView1.Columns(0).Text = "File name"
                ListView1.Columns(1).Text = "Created"
                ListView1.Columns(2).Text = "Last modified"
                ListView1.Columns(3).Text = "Last accessed"
            Case 2
                Text = "Administrador de archivos de respuesta desatendida"
                Label1.Text = "Ubicación de proyecto:"
                Button1.Text = "Examinar..."
                Button2.Text = "Abrir archivo"
                Button3.Text = "Abrir ubicación del archivo"
                Button4.Text = "Aplicar a una imagen..."
                ListView1.Columns(0).Text = "Nombre de archivo"
                ListView1.Columns(1).Text = "Creado"
                ListView1.Columns(2).Text = "Última modificación"
                ListView1.Columns(3).Text = "Último acceso"
            Case 3
                Text = "Gestionnaire de fichiers de réponse sans surveillance"
                Label1.Text = "Chemin du projet :"
                Button1.Text = "Parcourir..."
                Button2.Text = "Ouvrir le fichier"
                Button3.Text = "Ouvrir l'emplacement du fichier"
                Button4.Text = "Appliquer à l'image..."
                ListView1.Columns(0).Text = "Nom du fichier"
                ListView1.Columns(1).Text = "Créé"
                ListView1.Columns(2).Text = "Dernière modification"
                ListView1.Columns(3).Text = "Dernier accès"
            Case 4
                Text = "Gestor de ficheiros de resposta não assistida"
                Label1.Text = "Localização do projeto:"
                Button1.Text = "Procurar..."
                Button2.Text = "Abrir ficheiro"
                Button3.Text = "Abrir localização do ficheiro"
                Button4.Text = "Aplicar à imagem..."
                ListView1.Columns(0).Text = "Nome do ficheiro"
                ListView1.Columns(1).Text = "Criado"
                ListView1.Columns(2).Text = "Última modificação"
                ListView1.Columns(3).Text = "Último acesso"
            Case 5
                Text = "Gestore file di risposta non presidiato"
                Label1.Text = "Posizione del progetto:"
                Button1.Text = "Sfoglia..."
                Button2.Text = "Apri file"
                Button3.Text = "Apri il percorso del file"
                Button4.Text = "Applica all'immagine..."
                ListView1.Columns(0).Text = "Nome del file"
                ListView1.Columns(1).Text = "Creato"
                ListView1.Columns(2).Text = "Ultima modifica"
                ListView1.Columns(3).Text = "Ultimo accesso"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class