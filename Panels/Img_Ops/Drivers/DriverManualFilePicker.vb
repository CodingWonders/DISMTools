Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class DriverManualFilePicker

    Public DriverDir As String = ""
    Dim Language As Integer

    Private InfFiles As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Items checked for addition: " & ListView1.CheckedItems.Count)
        If ListView1.CheckedItems.Count <= 0 Then Exit Sub
        DynaLog.LogMessage("Adding selected items...")
        Dim SelectedDrivers As New List(Of String)
        For Each DrvItem As ListViewItem In AddDrivers.ListView1.Items
            SelectedDrivers.Add(DrvItem.SubItems(0).Text)
        Next
        If ListView1.Items.Count > 0 Then
            For Each Item As ListViewItem In ListView1.CheckedItems
                If SelectedDrivers.Contains(Item.Text) Then Continue For
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG" : AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item.Text, "File"}))
                            Case "ESN" : AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item.Text, "Archivo"}))
                            Case "FRA" : AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item.Text, "Fichier"}))
                            Case "PTB", "PTG" : AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item.Text, "Ficheiro"}))
                            Case "ITA" : AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item.Text, "File"}))
                        End Select
                    Case 1 : AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item.Text, "File"}))
                    Case 2 : AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item.Text, "Archivo"}))
                    Case 3 : AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item.Text, "Fichier"}))
                    Case 4 : AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item.Text, "Ficheiro"}))
                    Case 5 : AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item.Text, "File"}))
                End Select
            Next
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DriverManualFilePicker_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Language = MainForm.Language
        InfFiles.Clear()
        ListView1.Items.Clear()
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Choose driver files in directory"
                        Label1.Text = "Below is a recursive listing of all drivers in the directory you are specifying. From this list, pick the drivers you want to add and click OK."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        Button1.Text = "Refresh"
                    Case "ESN"
                        Text = "Escoja archivos de controladores en directorio"
                        Label1.Text = "Debajo se muestra un listado recursivo de todos los controladores en el directorio que está especificando. Escoja los controladores que quiera añadir de esta lista y haga clic en Aceptar."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Actualizar"
                    Case "FRA"
                        Text = "Choisir les fichiers du pilote dans le répertoire"
                        Label1.Text = "Vous trouverez ci-dessous une liste récursive de tous les pilotes dans le répertoire que vous avez spécifié. Dans cette liste, sélectionnez les pilotes que vous souhaitez ajouter et cliquez sur OK."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        Button1.Text = "Rafraîchir"
                    Case "PTB", "PTG"
                        Text = "Escolher ficheiros de controladores no diretório"
                        Label1.Text = "Abaixo está uma lista recursiva de todos os controladores no diretório que está a especificar. A partir desta lista, escolha os controladores que pretende adicionar e clique em OK."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Atualizar"
                    Case "ITA"
                        Text = "Scegliere i file dei driver nella cartella"
                        Label1.Text = "Di seguito è riportato un elenco ricorsivo di tutti i driver presenti nella cartella specificata. Da questo elenco, scegliere i driver che si desidera aggiungere e fare clic su OK."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        Button1.Text = "Aggiorna"
                End Select
            Case 1
                Text = "Choose driver files in directory"
                Label1.Text = "Below is a recursive listing of all drivers in the directory you are specifying. From this list, pick the drivers you want to add and click OK."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                Button1.Text = "Refresh"
            Case 2
                Text = "Escoja archivos de controladores en directorio"
                Label1.Text = "Debajo se muestra un listado recursivo de todos los controladores en el directorio que está especificando. Escoja los controladores que quiera añadir de esta lista y haga clic en Aceptar."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Actualizar"
            Case 3
                Text = "Choisir les fichiers du pilote dans le répertoire"
                Label1.Text = "Vous trouverez ci-dessous une liste récursive de tous les pilotes dans le répertoire que vous avez spécifié. Dans cette liste, sélectionnez les pilotes que vous souhaitez ajouter et cliquez sur OK."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                Button1.Text = "Rafraîchir"
            Case 4
                Text = "Escolher ficheiros de controladores no diretório"
                Label1.Text = "Abaixo está uma lista recursiva de todos os controladores no diretório que está a especificar. A partir desta lista, escolha os controladores que pretende adicionar e clique em OK."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Atualizar"
            Case 5
                Text = "Scegliere i file dei driver nella cartella"
                Label1.Text = "Di seguito è riportato un elenco ricorsivo di tutti i driver presenti nella cartella specificata. Da questo elenco, scegliere i driver che si desidera aggiungere e fare clic su OK."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                Button1.Text = "Aggiorna"
        End Select
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        ColumnHeader1.Width = WindowHelper.ScaleLogical(574)
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        If DriverDir <> "" And Directory.Exists(DriverDir) Then ScanBW.RunWorkerAsync()
    End Sub

    Private Sub ScanBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ScanBW.DoWork
        DynaLog.LogMessage("Scanning directory " & Quote & DriverDir & Quote & "...")
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG" : Label2.Text = "Scanning directory..."
                    Case "ESN" : Label2.Text = "Escaneando directorio..."
                    Case "FRA" : Label2.Text = "Scannage du répertoire en cours..."
                    Case "PTB", "PTG" : Label2.Text = "Pesquisar diretório..."
                    Case "ITA" : Label2.Text = "Scansione della cartella..."
                End Select
            Case 1 : Label2.Text = "Scanning directory..."
            Case 2 : Label2.Text = "Escaneando directorio..."
            Case 3 : Label2.Text = "Scannage du répertoire en cours..."
            Case 4 : Label2.Text = "Pesquisar diretório..."
            Case 5 : Label2.Text = "Scansione della cartella..." 
        End Select
        For Each DrvFile In Directory.GetFiles(DriverDir, "*.inf", SearchOption.AllDirectories)
            InfFiles.Add(DrvFile)
        Next
        DynaLog.LogMessage("Items detected in directory: " & ListView1.Items.Count)
    End Sub

    Private Sub ScanBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ScanBW.RunWorkerCompleted
        ListView1.Items.AddRange(InfFiles.Select(Function(infFile) New ListViewItem(New String() {infFile})).ToArray())
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label2.Text = "Directory scan complete." & CrLf & _
                                      "Driver files found: " & ListView1.Items.Count
                    Case "ESN"
                        Label2.Text = "Escaneo del directorio completado." & CrLf & _
                                      "Archivos de controladores encontrados: " & ListView1.Items.Count
                    Case "FRA"
                        Label2.Text = "Scannage du répertoire terminé." & CrLf & _
                                      "Fichiers de pilotes trouvés : " & ListView1.Items.Count
                    Case "PTB", "PTG"
                        Label2.Text = "Pesquisa de diretório concluída." & CrLf & _
                                      "Ficheiros de controladores encontrados: " & ListView1.Items.Count
                    Case "ITA"
                        Label2.Text = "Scansione della directory completata." & CrLf & _
                                      "File driver trovati: " & ListView1.Items.Count
                End Select
            Case 1
                Label2.Text = "Directory scan complete." & CrLf & _
                              "Driver files found: " & ListView1.Items.Count
            Case 2
                Label2.Text = "Escaneo del directorio completado." & CrLf & _
                              "Archivos de controladores encontrados: " & ListView1.Items.Count
            Case 3
                Label2.Text = "Scannage du répertoire terminé." & CrLf & _
                              "Fichiers de pilotes trouvés : " & ListView1.Items.Count
            Case 4
                Label2.Text = "Pesquisa de diretório concluída." & CrLf & _
                              "Ficheiros de controladores encontrados: " & ListView1.Items.Count
            Case 5
                Label2.Text = "Scansione della directory completata." & CrLf & _
                              "File driver trovati: " & ListView1.Items.Count
        End Select
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DynaLog.LogMessage("Preparing to refresh results...")
        InfFiles.Clear()
        ListView1.Items.Clear()
        If DriverDir <> "" And Directory.Exists(DriverDir) Then ScanBW.RunWorkerAsync()
    End Sub
End Class
