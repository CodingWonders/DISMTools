Imports System.Windows.Forms
Imports System.IO

Public Class AddPackageDlg

    Public CheckedCount As Integer
    Public pkgCount As Integer
    Public pkgs(65535) As String        ' This is hard-coded. If you have more than 65535 selected packages, the program will throw an exception

    Public Language As Integer

    Dim Addition_MUMFile As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        ProgressPanel.pkgSource = TextBox1.Text
        pkgCount = CheckedListBox1.CheckedItems.Count
        If RadioButton1.Checked AndAlso (Addition_MUMFile Is Nothing OrElse Addition_MUMFile = "") Then
            ProgressPanel.pkgAdditionOp = 0
        Else
            If Addition_MUMFile <> "" Then
                ProgressPanel.pkgAdditionOp = 2
                ProgressPanel.pkgCount = 1
            Else
                ProgressPanel.pkgAdditionOp = 1
                ProgressPanel.pkgCount = pkgCount
            End If
        End If
        If CheckBox1.Checked Then
            ProgressPanel.pkgIgnoreApplicabilityChecks = True
        Else
            ProgressPanel.pkgIgnoreApplicabilityChecks = False
        End If
        If CheckBox2.Checked Then
            ProgressPanel.pkgPreventIfPendingOnline = True
        Else
            ProgressPanel.pkgPreventIfPendingOnline = False
        End If
        If CheckBox3.Checked And Not MainForm.OnlineManagement Then
            ProgressPanel.imgCommitAfterOps = True
        Else
            ProgressPanel.imgCommitAfterOps = False
        End If
        If ProgressPanel.pkgAdditionOp = 1 Then
            If CheckedListBox1.CheckedItems.Count <= 0 Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MessageBox.Show(MainForm, "Please select packages to add, and try again. You can also continue with letting DISM scan applicable packages", "No packages selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Case "ESN"
                                MessageBox.Show(MainForm, "Seleccione paquetes a añadir, e inténtelo de nuevo. También puede continuar dejando que DISM escanee paquetes aplicables", "No hay paquetes seleccionados", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Case "FRA"
                                MessageBox.Show(MainForm, "Veuillez sélectionner les paquets à ajouter et réessayer. Vous pouvez également continuer à laisser DISM analyser les paquets applicables", "Aucun paquet sélectionné", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Case "PTB", "PTG"
                                MessageBox.Show(MainForm, "Por favor, seleccione os pacotes a adicionar e tente novamente. Também pode continuar a deixar o DISM verificar os pacotes aplicáveis", "Nenhum pacote selecionado", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Case "ITA"
                                MessageBox.Show(MainForm, "Selezionare i pacchetti da aggiungere e riprovare. È anche possibile continuare a lasciare che DISM esegua la scansione dei pacchetti applicabili", "Nessun pacchetto selezionato", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Select
                    Case 1
                        MessageBox.Show(MainForm, "Please select packages to add, and try again. You can also continue with letting DISM scan applicable packages", "No packages selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Case 2
                        MessageBox.Show(MainForm, "Seleccione paquetes a añadir, e inténtelo de nuevo. También puede continuar dejando que DISM escanee paquetes aplicables", "No hay paquetes seleccionados", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Case 3
                        MessageBox.Show(MainForm, "Veuillez sélectionner les paquets à ajouter et réessayer. Vous pouvez également continuer à laisser DISM analyser les paquets applicables", "Aucun paquet sélectionné", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Case 4
                        MessageBox.Show(MainForm, "Por favor, seleccione os pacotes a adicionar e tente novamente. Também pode continuar a deixar o DISM verificar os pacotes aplicáveis", "Nenhum pacote selecionado", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Case 5
                        MessageBox.Show(MainForm, "Selezionare i pacchetti da aggiungere e riprovare. È anche possibile continuare a lasciare che DISM esegua la scansione dei pacchetti applicabili", "Nessun pacchetto selezionato", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Select
            Else
                If pkgCount > 65535 Then
                    MessageBox.Show(MainForm, "Right now, due to program limitations, you can select 65535 packages or less.", "Current program limitation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    Try
                        For x As Integer = 0 To pkgCount - 1
                            pkgs(x) = CheckedListBox1.CheckedItems(x).ToString()
                        Next
                        For x = 0 To pkgs.Length
                            ProgressPanel.pkgs(x) = pkgs(x)
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If ProgressPanel.pkgAdditionOp = 1 Then
                    ProgressPanel.pkgLastCheckedPackageName = CheckedListBox1.CheckedItems(pkgCount - 1).ToString()
                End If
                ProgressPanel.OperationNum = 26
                Visible = False
                ProgressPanel.ShowDialog(MainForm)
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        ElseIf ProgressPanel.pkgAdditionOp = 0 Then
            ProgressPanel.OperationNum = 26
            Visible = False
            ProgressPanel.ShowDialog(MainForm)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        ElseIf ProgressPanel.pkgAdditionOp = 2 Then
            pkgs(0) = Addition_MUMFile
            ProgressPanel.pkgs(0) = pkgs(0)
            ProgressPanel.OperationNum = 26
            Visible = False
            ProgressPanel.ShowDialog(MainForm)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
            ScanBW.RunWorkerAsync()
        End If
    End Sub

    Sub GatherPackages(FolderToScan As String)
        CheckedListBox1.Items.Clear()
        Cursor = Cursors.WaitCursor
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label4.Text = "Scanning directory for packages. Please wait..."
                    Case "ESN"
                        Label4.Text = "Escaneando directorio por paquetes. Espere..."
                    Case "FRA"
                        Label4.Text = "Recherche des paquets dans le répertoire en cours. Veuillez patienter..."
                    Case "PTB", "PTG"
                        Label4.Text = "A analisar o diretório em busca de pacotes. Aguarde..."
                    Case "ITA"
                        Label4.Text = "Scansione della directory per i pacchetti. Attendere..."
                End Select
            Case 1
                Label4.Text = "Scanning directory for packages. Please wait..."
            Case 2
                Label4.Text = "Escaneando directorio por paquetes. Espere..."
            Case 3
                Label4.Text = "Recherche des paquets dans le répertoire en cours. Veuillez patienter..."
            Case 4
                Label4.Text = "A analisar o diretório em busca de pacotes. Aguarde..."
            Case 5
                Label4.Text = "Scansione della directory per i pacchetti. Attendere..."
        End Select
        Refresh()
        ' TODO: show CheckedListBox items without full path
        Try
            For Each CabPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchAllSubDirectories, "*.cab")
                If CabPkg.Contains("MsuExtract") Then
                    ' CAB files stored in MsuExtract are skipped, as they come from MSU files. Skip these items and continue loop
                    Continue For
                End If
                CheckedListBox1.Items.Add(CabPkg)
            Next
            For Each MsuPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchAllSubDirectories, "*.msu")
                CheckedListBox1.Items.Add(MsuPkg)
            Next
        Catch ex As Exception
            For Each CabPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchTopLevelOnly, "*.cab")
                If CabPkg.Contains("MsuExtract") Then
                    ' CAB files stored in MsuExtract are skipped, as they come from MSU files. Skip these items and continue loop
                    Continue For
                End If
                CheckedListBox1.Items.Add(CabPkg)
            Next
            For Each MsuPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchTopLevelOnly, "*.msu")
                CheckedListBox1.Items.Add(MsuPkg)
            Next
        End Try
        CountItems()
        Cursor = Cursors.Arrow
    End Sub

    Sub CountItems()
        If CheckedCount > CheckedListBox1.CheckedItems.Count Then
            Do Until CheckedCount = CheckedListBox1.CheckedItems.Count
                CheckedCount -= 1
            Loop
        ElseIf CheckedCount < 0 Then
            Do Until CheckedCount = 0
                CheckedCount += 1
            Loop
        End If
        If CheckedListBox1.Items.Count = 0 Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label4.Text = "This folder does not contain any packages. Please use a different source and try again"
                        Case "ESN"
                            Label4.Text = "Esta carpeta no contiene ningún paquete. Utilice un origen diferente e inténtelo de nuevo"
                        Case "FRA"
                            Label4.Text = "Ce répertoire ne contient aucun paquet. Veuillez utiliser une autre source et réessayer"
                        Case "PTB", "PTG"
                            Label4.Text = "Esta pasta não contém quaisquer pacotes. Utilize uma origem diferente e tente novamente"
                        Case "ITA"
                            Label4.Text = "Questa cartella non contiene pacchetti. Utilizzare un'altra origine e riprovare"
                    End Select
                Case 1
                    Label4.Text = "This folder does not contain any packages. Please use a different source and try again"
                Case 2
                    Label4.Text = "Esta carpeta no contiene ningún paquete. Utilice un origen diferente e inténtelo de nuevo"
                Case 3
                    Label4.Text = "Ce répertoire ne contient aucun paquet. Veuillez utiliser une autre source et réessayer"
                Case 4
                    Label4.Text = "Esta pasta não contém quaisquer pacotes. Utilize uma origem diferente e tente novamente"
                Case 5
                    Label4.Text = "Questa cartella non contiene pacchetti. Utilizzare un'altra origine e riprovare"
            End Select
            Beep()
        Else
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label4.Text = "This folder contains " & CheckedListBox1.Items.Count & " package" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                        Case "ESN"
                            Label4.Text = "Esta carpeta contiene " & CheckedListBox1.Items.Count & " paquete" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                        Case "FRA"
                            Label4.Text = "Ce répertoire contient " & CheckedListBox1.Items.Count & " paquet" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                        Case "PTB", "PTG"
                            Label4.Text = "Esta pasta contém " & CheckedListBox1.Items.Count & " pacote" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                        Case "ITA"
                            Label4.Text = "Questa cartella contiene " & CheckedListBox1.Items.Count & " pacchett" & If(CheckedListBox1.Items.Count = 1, "o.", "i.")
                    End Select
                Case 1
                    Label4.Text = "This folder contains " & CheckedListBox1.Items.Count & " package" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                Case 2
                    Label4.Text = "Esta carpeta contiene " & CheckedListBox1.Items.Count & " paquete" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                Case 3
                    Label4.Text = "Ce répertoire contient " & CheckedListBox1.Items.Count & " paquet" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                Case 4
                    Label4.Text = "Esta pasta contém " & CheckedListBox1.Items.Count & " pacote" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                Case 5
                    Label4.Text = "Questa cartella contiene " & CheckedListBox1.Items.Count & " pacchett" & If(CheckedListBox1.Items.Count = 1, "o.", "i.")
            End Select
        End If
    End Sub

    Private Sub AddPackageDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Add packages"
                        Label1.Text = Text
                        Label2.Text = "Package source:"
                        Label3.Text = "Package operation:"
                        Button1.Text = "Browse..."
                        Button2.Text = "Select all"
                        Button3.Text = "Select none"
                        Button4.Text = "Add update manifest..."
                        Cancel_Button.Text = "Cancel"
                        OK_Button.Text = "OK"
                        RadioButton1.Text = "Scan folder recursively for packages"
                        RadioButton2.Text = "Choose which packages to add:"
                        CheckBox1.Text = "Ignore applicability checks (not recommended)"
                        CheckBox2.Text = "Skip package installation if online operations are pending"
                        CheckBox3.Text = "Commit image after adding packages"
                        FolderBrowserDialog1.Description = "Specify the folder containing CAB or MSU packages:"
                        GroupBox1.Text = "Packages"
                        GroupBox2.Text = "Options"
                    Case "ESN"
                        Text = "Añadir paquetes"
                        Label1.Text = Text
                        Label2.Text = "Origen:"
                        Label3.Text = "Operación de paquetes:"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Todos los paquetes"
                        Button3.Text = "Ningún paquete"
                        Button4.Text = "Añadir manifiesto de actualización..."
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "Aceptar"
                        RadioButton1.Text = "Escanear carpeta de forma recursiva por paquetes"
                        RadioButton2.Text = "Elegir qué paquetes añadir:"
                        CheckBox1.Text = "Ignorar comprobaciones de aplicabilidad (no recomendado)"
                        CheckBox2.Text = "Omitir instalación de paquetes si operaciones en línea deben realizarse"
                        CheckBox3.Text = "Guardar imagen tras añadir paquetes"
                        FolderBrowserDialog1.Description = "Especifique la carpeta que contenga paquetes CAB o MSU:"
                        GroupBox1.Text = "Paquetes"
                        GroupBox2.Text = "Opciones"
                    Case "FRA"
                        Text = "Ajouter des paquets"
                        Label1.Text = Text
                        Label2.Text = "Source des paquets :"
                        Label3.Text = "Opération d'ajout des paquets :"
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Sélectionner tout"
                        Button3.Text = "Sélectionner aucun"
                        Button4.Text = "Ajouter un manifeste de mise à jour..."
                        Cancel_Button.Text = "Annuler"
                        OK_Button.Text = "OK"
                        RadioButton1.Text = "Analyse récursive du dossier à la recherche des paquets à ajouter"
                        RadioButton2.Text = "Choisissez les paquets à ajouter :"
                        CheckBox1.Text = "Ignorer les contrôles d'applicabilité (non recommandé)"
                        CheckBox2.Text = "Sauter l'installation des paquets si les opérations en ligne sont en cours"
                        CheckBox3.Text = "Sauvegarder l'image après l'ajout des paquets"
                        FolderBrowserDialog1.Description = "Indiquez le répertoire qui contient les paquets CAB ou MSU :"
                        GroupBox1.Text = "Paquets"
                        GroupBox2.Text = "Paramètres"
                    Case "PTB", "PTG"
                        Text = "Adicionar pacotes"
                        Label1.Text = Text
                        Label2.Text = "Origem do pacote:"
                        Label3.Text = "Operação do pacote:"
                        Button1.Text = "Navegar..."
                        Button2.Text = "Selecionar tudo"
                        Button3.Text = "Não selecionar nenhum"
                        Button4.Text = "Adicionar manifesto de atualização..."
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "OK"
                        RadioButton1.Text = "Procurar pacotes na pasta de forma recursiva"
                        RadioButton2.Text = "Escolher quais os pacotes a adicionar:"
                        CheckBox1.Text = "Ignorar verificações de aplicabilidade (não recomendado)"
                        CheckBox2.Text = "Ignorar a instalação de pacotes se estiverem pendentes operações online"
                        CheckBox3.Text = "Confirmar imagem depois de adicionar pacotes"
                        FolderBrowserDialog1.Description = "Especificar a pasta que contém os pacotes CAB ou MSU:"
                        GroupBox1.Text = "Pacotes"
                        GroupBox2.Text = "Opções"
                    Case "ITA"
                        Text = "Aggiungi pacchetti"
                        Label1.Text = Text
                        Label2.Text = "Origine pacchetto:"
                        Label3.Text = "Operazione pacchetto:"
                        Button1.Text = "Sfoglia..."
                        Button2.Text = "Seleziona tutto"
                        Button3.Text = "Seleziona nessuno"
                        Button4.Text = "Aggiungere il manifesto di aggiornamento..."
                        Cancel_Button.Text = "Annullare"
                        OK_Button.Text = "OK"
                        RadioButton1.Text = "Scansiona la cartella in modo ricorsivo per i pacchetti"
                        RadioButton2.Text = "Scegliere quali pacchetti aggiungere:"
                        CheckBox1.Text = "Ignorare i controlli di applicabilità (non consigliato)"
                        CheckBox2.Text = "Salta l'installazione del pacchetto se sono in corso operazioni online"
                        CheckBox3.Text = "Impegna l'immagine dopo l'aggiunta dei pacchetti"
                        FolderBrowserDialog1.Description = "Specificare la cartella contenente i pacchetti CAB o MSU:"
                        GroupBox1.Text = "Pacchetti"
                        GroupBox2.Text = "Opzioni"
                End Select
            Case 1
                Text = "Add packages"
                Label1.Text = Text
                Label2.Text = "Package source:"
                Label3.Text = "Package operation:"
                Button1.Text = "Browse..."
                Button2.Text = "Select all"
                Button3.Text = "Select none"
                Button4.Text = "Add update manifest..."
                Cancel_Button.Text = "Cancel"
                OK_Button.Text = "OK"
                RadioButton1.Text = "Scan folder recursively for packages"
                RadioButton2.Text = "Choose which packages to add:"
                CheckBox1.Text = "Ignore applicability checks (not recommended)"
                CheckBox2.Text = "Skip package installation if online operations are pending"
                CheckBox3.Text = "Commit image after adding packages"
                FolderBrowserDialog1.Description = "Specify the folder containing CAB or MSU packages:"
                GroupBox1.Text = "Packages"
                GroupBox2.Text = "Options"
            Case 2
                Text = "Añadir paquetes"
                Label1.Text = Text
                Label2.Text = "Origen:"
                Label3.Text = "Operación de paquetes:"
                Button1.Text = "Examinar..."
                Button2.Text = "Todos los paquetes"
                Button3.Text = "Ningún paquete"
                Button4.Text = "Añadir manifiesto de actualización..."
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "Aceptar"
                RadioButton1.Text = "Escanear carpeta de forma recursiva por paquetes"
                RadioButton2.Text = "Elegir qué paquetes añadir:"
                CheckBox1.Text = "Ignorar comprobaciones de aplicabilidad (no recomendado)"
                CheckBox2.Text = "Omitir instalación de paquetes si operaciones en línea deben realizarse"
                CheckBox3.Text = "Guardar imagen tras añadir paquetes"
                FolderBrowserDialog1.Description = "Especifique la carpeta que contenga paquetes CAB o MSU:"
                GroupBox1.Text = "Paquetes"
                GroupBox2.Text = "Opciones"
            Case 3
                Text = "Ajouter des paquets"
                Label1.Text = Text
                Label2.Text = "Source des paquets :"
                Label3.Text = "Opération d'ajout des paquets :"
                Button1.Text = "Parcourir..."
                Button2.Text = "Sélectionner tout"
                Button3.Text = "Sélectionner aucun"
                Button4.Text = "Ajouter un manifeste de mise à jour..."
                Cancel_Button.Text = "Annuler"
                OK_Button.Text = "OK"
                RadioButton1.Text = "Analyse récursive du dossier à la recherche des paquets à ajouter"
                RadioButton2.Text = "Choisissez les paquets à ajouter :"
                CheckBox1.Text = "Ignorer les contrôles d'applicabilité (non recommandé)"
                CheckBox2.Text = "Sauter l'installation des paquets si les opérations en ligne sont en cours"
                CheckBox3.Text = "Sauvegarder l'image après l'ajout des paquets"
                FolderBrowserDialog1.Description = "Indiquez le répertoire qui contient les paquets CAB ou MSU :"
                GroupBox1.Text = "Paquets"
                GroupBox2.Text = "Paramètres"
            Case 4
                Text = "Adicionar pacotes"
                Label1.Text = Text
                Label2.Text = "Origem do pacote:"
                Label3.Text = "Operação do pacote:"
                Button1.Text = "Navegar..."
                Button2.Text = "Selecionar tudo"
                Button3.Text = "Não selecionar nenhum"
                Button4.Text = "Adicionar manifesto de atualização..."
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "OK"
                RadioButton1.Text = "Procurar pacotes na pasta de forma recursiva"
                RadioButton2.Text = "Escolher quais os pacotes a adicionar:"
                CheckBox1.Text = "Ignorar verificações de aplicabilidade (não recomendado)"
                CheckBox2.Text = "Ignorar a instalação de pacotes se estiverem pendentes operações online"
                CheckBox3.Text = "Confirmar imagem depois de adicionar pacotes"
                FolderBrowserDialog1.Description = "Especificar a pasta que contém os pacotes CAB ou MSU:"
                GroupBox1.Text = "Pacotes"
                GroupBox2.Text = "Opções"
            Case 5
                Text = "Aggiungi pacchetti"
                Label1.Text = Text
                Label2.Text = "Origine pacchetto:"
                Label3.Text = "Operazione pacchetto:"
                Button1.Text = "Sfoglia..."
                Button2.Text = "Seleziona tutto"
                Button3.Text = "Seleziona nessuno"
                Button4.Text = "Aggiungere il manifesto di aggiornamento..."
                Cancel_Button.Text = "Annullare"
                OK_Button.Text = "OK"
                RadioButton1.Text = "Scansiona la cartella in modo ricorsivo per i pacchetti"
                RadioButton2.Text = "Scegliere quali pacchetti aggiungere:"
                CheckBox1.Text = "Ignorare i controlli di applicabilità (non consigliato)"
                CheckBox2.Text = "Salta l'installazione del pacchetto se sono in corso operazioni online"
                CheckBox3.Text = "Impegna l'immagine dopo l'aggiunta dei pacchetti"
                FolderBrowserDialog1.Description = "Specificare la cartella contenente i pacchetti CAB o MSU:"
                GroupBox1.Text = "Pacchetti"
                GroupBox2.Text = "Opzioni"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            CheckedListBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            CheckedListBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        CheckedListBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Control.CheckForIllegalCrossThreadCalls = False
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If CheckedListBox1.Items.Count = 0 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label4.Text = "Please specify a directory where CAB or MSU files are located."
                        Case "ESN"
                            Label4.Text = "Especifique el directorio donde se encuentran archivos CAB o MSU."
                        Case "FRA"
                            Label4.Text = "Veuillez indiquer un répertoire où se trouvent les fichiers CAB ou MSU."
                        Case "PTB", "PTG"
                            Label4.Text = "Especifique um diretório onde estão localizados os ficheiros CAB ou MSU."
                        Case "ITA"
                            Label4.Text = "Specificare una directory in cui si trovano i file CAB o MSU"
                    End Select
                Case 1
                    Label4.Text = "Please specify a directory where CAB or MSU files are located."
                Case 2
                    Label4.Text = "Especifique el directorio donde se encuentran archivos CAB o MSU."
                Case 3
                    Label4.Text = "Veuillez indiquer un répertoire où se trouvent les fichiers CAB ou MSU."
                Case 4
                    Label4.Text = "Especifique um diretório onde estão localizados os ficheiros CAB ou MSU."
                Case 5
                    Label4.Text = "Specificare una directory in cui si trovano i file CAB o MSU"
            End Select
        End If
        Language = MainForm.Language
        CheckBox3.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Addition_MUMFile = ""
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label4.Enabled = False
            CheckedListBox1.Enabled = False
            TableLayoutPanel2.Enabled = False
        Else
            Label4.Enabled = True
            CheckedListBox1.Enabled = True
            TableLayoutPanel2.Enabled = True
        End If
        If ProgressPanel.OperationNum = 26 Then
            pkgCount = CheckedCount
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, True)
            CheckedCount += 1
            CountItems()
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, False)
            CheckedCount -= 1
            CountItems()
        Next
        DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub ScanBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ScanBW.DoWork
        GatherPackages(TextBox1.Text)
    End Sub

    Private Sub CheckedListBox1_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck
        CheckedCount = CheckedListBox1.CheckedItems.Count
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If MUMAdditionDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Addition_MUMFile = MUMAdditionDialog.MUMFile
            If File.Exists(Addition_MUMFile) Then
                OK_Button.PerformClick()
            End If
        End If
    End Sub
End Class
