Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class DriverManualFilePicker

    Public DriverDir As String = ""
    Dim Language As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If CheckedListBox1.CheckedItems.Count <= 0 Then Exit Sub
        Dim SelectedDrivers As New List(Of String)
        For Each DrvItem As ListViewItem In AddDrivers.ListView1.Items
            SelectedDrivers.Add(DrvItem.SubItems(0).Text)
        Next
        If CheckedListBox1.Items.Count > 0 Then
            For Each Item In CheckedListBox1.CheckedItems
                If SelectedDrivers.Contains(Item) Then Continue For
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, "File"}))
                            Case "ESN"
                                AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, "Archivo"}))
                            Case "FRA"
                                AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, "Fichier"}))
                            Case "PTB", "PTG"
                                AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, "Ficheiro"}))
                            Case "ITA"
                                AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, "File"}))
                        End Select
                    Case 1
                        AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, "File"}))
                    Case 2
                        AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, "Archivo"}))
                    Case 3
                        AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, "Fichier"}))
                    Case 4
                        AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, "Ficheiro"}))
                    Case 5
                        AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, "File"}))
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
        Control.CheckForIllegalCrossThreadCalls = False
        CheckedListBox1.Items.Clear()
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
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            CheckedListBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            CheckedListBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        CheckedListBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        If DriverDir <> "" And Directory.Exists(DriverDir) Then ScanBW.RunWorkerAsync()
    End Sub

    Private Sub ScanBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ScanBW.DoWork
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label2.Text = "Scanning directory..." & CrLf & _
                                      "Driver files found thus far: " & CheckedListBox1.Items.Count
                    Case "ESN"
                        Label2.Text = "Escaneando directorio..." & CrLf & _
                                      "Archivos de controladores encontrados por ahora: " & CheckedListBox1.Items.Count
                    Case "FRA"
                        Label2.Text = "Scannage du répertoire en cours..." & CrLf & _
                                      "Fichiers de pilotes trouvés jusqu'à présent : " & CheckedListBox1.Items.Count
                    Case "PTB", "PTG"
                        Label2.Text = "Pesquisar diretório..." & CrLf & _
                                      "Ficheiros de controladores encontrados até agora: " & CheckedListBox1.Items.Count
                    Case "ITA"
                        Label2.Text = "Scansione della cartella..." & CrLf & _
                                      "File di driver trovati finora: " & CheckedListBox1.Items.Count
                End Select
            Case 1
                Label2.Text = "Scanning directory..." & CrLf & _
                              "Driver files found thus far: " & CheckedListBox1.Items.Count
            Case 2
                Label2.Text = "Escaneando directorio..." & CrLf & _
                              "Archivos de controladores encontrados por ahora: " & CheckedListBox1.Items.Count
            Case 3
                Label2.Text = "Scannage du répertoire en cours..." & CrLf & _
                              "Fichiers de pilotes trouvés jusqu'à présent : " & CheckedListBox1.Items.Count
            Case 4
                Label2.Text = "Pesquisar diretório..." & CrLf & _
                              "Ficheiros de controladores encontrados até agora: " & CheckedListBox1.Items.Count
            Case 5
                Label2.Text = "Scansione della cartella..." & CrLf & _
                              "File di driver trovati finora: " & CheckedListBox1.Items.Count
        End Select
        For Each DrvFile In Directory.GetFiles(DriverDir, "*.inf", SearchOption.AllDirectories)
            CheckedListBox1.Items.Add(DrvFile)
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label2.Text = "Scanning directory..." & CrLf & _
                                          "Driver files found thus far: " & CheckedListBox1.Items.Count
                        Case "ESN"
                            Label2.Text = "Escaneando directorio..." & CrLf & _
                                          "Archivos de controladores encontrados por ahora: " & CheckedListBox1.Items.Count
                        Case "FRA"
                            Label2.Text = "Scannage du répertoire en cours..." & CrLf & _
                                          "Fichiers de pilotes trouvés jusqu'à présent : " & CheckedListBox1.Items.Count
                        Case "PTB", "PTG"
                            Label2.Text = "Pesquisar diretório..." & CrLf & _
                                          "Ficheiros de controladores encontrados até agora: " & CheckedListBox1.Items.Count
                        Case "ITA"
                            Label2.Text = "Scansione della cartella..." & CrLf & _
                                          "File di driver trovati finora: " & CheckedListBox1.Items.Count
                    End Select
                Case 1
                    Label2.Text = "Scanning directory..." & CrLf & _
                                  "Driver files found thus far: " & CheckedListBox1.Items.Count
                Case 2
                    Label2.Text = "Escaneando directorio..." & CrLf & _
                                  "Archivos de controladores encontrados por ahora: " & CheckedListBox1.Items.Count
                Case 3
                    Label2.Text = "Scannage du répertoire en cours..." & CrLf & _
                                  "Fichiers de pilotes trouvés jusqu'à présent : " & CheckedListBox1.Items.Count
                Case 4
                    Label2.Text = "Pesquisar diretório..." & CrLf & _
                                  "Ficheiros de controladores encontrados até agora: " & CheckedListBox1.Items.Count
                Case 5
                    Label2.Text = "Scansione della cartella..." & CrLf & _
                                  "File di driver trovati finora: " & CheckedListBox1.Items.Count
            End Select
        Next
    End Sub

    Private Sub ScanBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ScanBW.RunWorkerCompleted
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label2.Text = "Directory scan complete." & CrLf & _
                                      "Driver files found: " & CheckedListBox1.Items.Count
                    Case "ESN"
                        Label2.Text = "Escaneo del directorio completado." & CrLf & _
                                      "Archivos de controladores encontrados: " & CheckedListBox1.Items.Count
                    Case "FRA"
                        Label2.Text = "Scannage du répertoire terminé." & CrLf & _
                                      "Fichiers de pilotes trouvés : " & CheckedListBox1.Items.Count
                    Case "PTB", "PTG"
                        Label2.Text = "Pesquisa de diretório concluída." & CrLf & _
                                      "Ficheiros de controladores encontrados: " & CheckedListBox1.Items.Count
                    Case "ITA"
                        Label2.Text = "Scansione della directory completata." & CrLf & _
                                      "File driver trovati: " & CheckedListBox1.Items.Count
                End Select
            Case 1
                Label2.Text = "Directory scan complete." & CrLf & _
                              "Driver files found: " & CheckedListBox1.Items.Count
            Case 2
                Label2.Text = "Escaneo del directorio completado." & CrLf & _
                              "Archivos de controladores encontrados: " & CheckedListBox1.Items.Count
            Case 3
                Label2.Text = "Scannage du répertoire terminé." & CrLf & _
                              "Fichiers de pilotes trouvés : " & CheckedListBox1.Items.Count
            Case 4
                Label2.Text = "Pesquisa de diretório concluída." & CrLf & _
                              "Ficheiros de controladores encontrados: " & CheckedListBox1.Items.Count
            Case 5
                Label2.Text = "Scansione della directory completata." & CrLf & _
                              "File driver trovati: " & CheckedListBox1.Items.Count
        End Select
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CheckedListBox1.Items.Clear()
        If DriverDir <> "" And Directory.Exists(DriverDir) Then ScanBW.RunWorkerAsync()
    End Sub
End Class
