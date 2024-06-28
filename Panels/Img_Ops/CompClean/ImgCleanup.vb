Imports System.Windows.Forms
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO

Public Class ImgCleanup

    Dim Tasks() As String = New String(6) {"Revert pending actions", "Clean up Service Pack backup files", "Clean up component store", "Analyze component store", "Check component store", "Scan component store for corruption", "Repair component store"}

    Dim SelTask As Integer = -1

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.CleanupTask = ComboBox1.SelectedIndex
        Select Case ComboBox1.SelectedIndex
            Case 1
                ProgressPanel.CleanupHideSP = CheckBox1.Checked = True
            Case 2
                ProgressPanel.ResetCompBase = CheckBox2.Checked = True
                ProgressPanel.DeferCleanupOps = If(CheckBox2.Checked And CheckBox3.Checked, True, False)
            Case 6
                ProgressPanel.UseCompRepairSource = CheckBox4.Checked = True
                If CheckBox4.Checked And RichTextBox1.Text = "" Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    MsgBox("No valid source has been provided for component store repair." & CrLf & CrLf & If(RichTextBox1.Text = "", "Please provide a source and try again.", "Please make sure the specified source exists in the file system and try again."), vbOKOnly + vbCritical, "Image cleanup")
                                Case "ESN"
                                    MsgBox("No se ha proporcionado un origen válido para la reparación del almacén de componentes." & CrLf & CrLf & If(RichTextBox1.Text = "", "Proporcione un origen e inténtelo de nuevo", "Asegúrese de que el origen especificado exista en el sistema de archivos e inténtelo de nuevo."), vbOKOnly + vbCritical, "Limpieza de imagen")
                                Case "FRA"
                                    MsgBox("Aucune source valide n'a été fournie pour la réparation du stock de composants." & CrLf & CrLf & If(RichTextBox1.Text = "", "Veuillez indiquer une source et réessayer.", "Assurez-vous que la source spécifiée existe dans le système de fichiers et réessayez."), vbOKOnly + vbCritical, "Nettoyage de l'image")
                                Case "PTB", "PTG"
                                    MsgBox("Não foi fornecida nenhuma fonte válida para a reparação do armazenamento de componentes." & CrLf & CrLf & If(RichTextBox1.Text = "", "Forneça uma fonte e tente novamente.", "Certifique-se de que a fonte especificada existe no sistema de ficheiros e tente novamente."), vbOKOnly + vbCritical, "Limpeza da imagem")
                                Case "ITA"
                                    MsgBox("Non è stata fornita un'origine valida per la riparazione dell'archivio componenti." & CrLf & CrLf & If(RichTextBox1.Text = "", "Fornire un'origine e riprovare.", "Assicurarsi che l'origine specificata esista nel file system e riprovare."), vbOKOnly + vbCritical, "Pulizia immagine")
                            End Select
                        Case 1
                            MsgBox("No valid source has been provided for component store repair." & CrLf & CrLf & If(RichTextBox1.Text = "", "Please provide a source and try again.", "Please make sure the specified source exists in the file system and try again."), vbOKOnly + vbCritical, "Image cleanup")
                        Case 2
                            MsgBox("No se ha proporcionado un origen válido para la reparación del almacén de componentes." & CrLf & CrLf & If(RichTextBox1.Text = "", "Proporcione un origen e inténtelo de nuevo", "Asegúrese de que el origen especificado exista en el sistema de archivos e inténtelo de nuevo."), vbOKOnly + vbCritical, "Limpieza de imagen")
                        Case 3
                            MsgBox("Aucune source valide n'a été fournie pour la réparation du stock de composants." & CrLf & CrLf & If(RichTextBox1.Text = "", "Veuillez indiquer une source et réessayer.", "Assurez-vous que la source spécifiée existe dans le système de fichiers et réessayez."), vbOKOnly + vbCritical, "Nettoyage de l'image")
                        Case 4
                            MsgBox("Não foi fornecida nenhuma fonte válida para a reparação do armazenamento de componentes." & CrLf & CrLf & If(RichTextBox1.Text = "", "Forneça uma fonte e tente novamente.", "Certifique-se de que a fonte especificada existe no sistema de ficheiros e tente novamente."), vbOKOnly + vbCritical, "Limpeza da imagem")
                        Case 5
                            MsgBox("Non è stata fornita un'origine valida per la riparazione dell'archivio componenti." & CrLf & CrLf & If(RichTextBox1.Text = "", "Fornire un'origine e riprovare.", "Assicurarsi che l'origine specificata esista nel file system e riprovare."), vbOKOnly + vbCritical, "Pulizia immagine")
                    End Select
                    Exit Sub
                End If
                ProgressPanel.ComponentRepairSource = RichTextBox1.Text
                ProgressPanel.LimitWUAccess = CheckBox5.Checked = True
        End Select
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 32
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgCleanup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Image cleanup"
                        Label1.Text = Text
                        Label2.Text = "Choose a task:"
                        If ComboBox1.SelectedItem = "" Then Label3.Text = "Choose a task to see its description"
                        Label4.Text = "There are no configurable options for this task. However, you should only run this task to try to recover a Windows image that fails to boot."
                        Label5.Text = "The superseded components base reset was last run on:"
                        Label7.Text = "You should only check this option if the base reset takes more than 30 minutes to complete"
                        Label8.Text = "There are no configurable options for this task."
                        Label9.Text = "There are no configurable options for this task."
                        Label10.Text = "There are no configurable options for this task."
                        Label11.Text = "Source:"
                        Label12.Text = "Select a task listed above to configure its options."
                        GroupBox1.Text = "Task options"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        Tasks(0) = "Revert pending actions"
                        Tasks(1) = "Clean up Service Pack backup files"
                        Tasks(2) = "Clean up component store"
                        Tasks(3) = "Analyze component store"
                        Tasks(4) = "Check component store"
                        Tasks(5) = "Scan component store for corruption"
                        Tasks(6) = "Repair component store"
                        ComboBox1.Items.AddRange(Tasks)
                        HealthRestoreSourceOFD.Title = "Specify the source from which we will restore the component store health"
                        Button1.Text = "Browse..."
                        Button2.Text = "Detect from group policy"
                        CheckBox1.Text = "Hide service pack from the Installed Updates list"
                        CheckBox2.Text = "Reset base of superseded components"
                        CheckBox3.Text = "Defer long-running cleanup operations"
                        CheckBox4.Text = "Use different source for component repair"
                        CheckBox5.Text = "Limit access to Windows Update"
                    Case "ESN"
                        Text = "Limpieza de imagen"
                        Label1.Text = Text
                        Label2.Text = "Elija una tarea:"
                        If ComboBox1.SelectedItem = "" Then Label3.Text = "Elija una tarea para ver su descripción"
                        Label4.Text = "No hay opciones configurables para esta tarea. Sin embargo, debería ejecutar esta tarea para intentar recuperar una imagen de Windows que no inicia."
                        Label5.Text = "El restablecimiento de la base de componentes sustituidos fue ejecutado por última vez en:"
                        Label7.Text = "Debería marcar esta opción solo si el restablecimiento de la base tarda más de 30 minutos en completar"
                        Label8.Text = "No hay opciones configurables para esta tarea."
                        Label9.Text = "No hay opciones configurables para esta tarea."
                        Label10.Text = "No hay opciones configurables para esta tarea."
                        Label11.Text = "Origen:"
                        Label12.Text = "Seleccione una tarea en la lista de arriba para configurar sus opciones."
                        GroupBox1.Text = "Opciones de tarea"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        Tasks(0) = "Revertir acciones pendientes"
                        Tasks(1) = "Limpiar archivos de copia de seguridad de Service Pack"
                        Tasks(2) = "Limpiar almacén de componentes"
                        Tasks(3) = "Analizar almacén de componentes"
                        Tasks(4) = "Comprobar almacén de componentes"
                        Tasks(5) = "Escanear almacén de componentes para detectar corrupción"
                        Tasks(6) = "Reparar almacén de componentes"
                        ComboBox1.Items.AddRange(Tasks)
                        HealthRestoreSourceOFD.Title = "Especifique el origen desde donde restauraremos la salud del almacén de componentes"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Detectar políticas de grupo"
                        CheckBox1.Text = "Ocultar Service Pack del listado de Actualizaciones instaladas"
                        CheckBox2.Text = "Restablecer la base de componentes sustituidos"
                        CheckBox3.Text = "Diferir operaciones largas de limpieza"
                        CheckBox4.Text = "Usar origen diferente para la reparación de componentes"
                        CheckBox5.Text = "Limitar acceso a Windows Update"
                    Case "FRA"
                        Text = "Nettoyage de l'image"
                        Label1.Text = Text
                        Label2.Text = "Choisissez une tâche :"
                        If ComboBox1.SelectedItem = "" Then Label3.Text = "Choisissez une tâche pour voir sa description"
                        Label4.Text = "Il n'y a pas d'options configurables pour cette tâche. Cependant, vous ne devez exécuter cette tâche que pour essayer de récupérer une image Windows qui ne démarre pas."
                        Label5.Text = "La réinitialisation de la base des composants remplacés a été exécutée pour la dernière fois :"
                        Label7.Text = "Ne cochez cette option que si la réinitialisation de la base prend plus de 30 minutes."
                        Label8.Text = "Il n'y a pas d'options configurables pour cette tâche."
                        Label9.Text = "Il n'y a pas d'options configurables pour cette tâche."
                        Label10.Text = "Il n'y a pas d'options configurables pour cette tâche."
                        Label11.Text = "Source :"
                        Label12.Text = "Sélectionnez une tâche dans la liste ci-dessus pour configurer ses paramètres."
                        GroupBox1.Text = "Paramètres de la tâche"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        Tasks(0) = "Annuler les actions en cours"
                        Tasks(1) = "Nettoyer les fichiers de sauvegarde du Service Pack"
                        Tasks(2) = "Nettoyer le stock de composants"
                        Tasks(3) = "Analyser le stock de composants"
                        Tasks(4) = "Vérifier le stock de composants"
                        Tasks(5) = "Rechercher la corruption du stock de composants"
                        Tasks(6) = "Réparer le stock de composants"
                        ComboBox1.Items.AddRange(Tasks)
                        HealthRestoreSourceOFD.Title = "Indiquez la source à partir de laquelle nous restaurerons l'état du stock de composants."
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Détecter à partir des politiques de groupe"
                        CheckBox1.Text = "Cacher le Service Pack de la liste des mises à jour installées"
                        CheckBox2.Text = "Réinitialiser la base des composants remplacés"
                        CheckBox3.Text = "Différer les opérations de nettoyage de longue durée"
                        CheckBox4.Text = "Utiliser une autre source pour la réparation des composants"
                        CheckBox5.Text = "Limiter l'accès à Windows Update"
                    Case "PTB", "PTG"
                        Text = "Limpeza de imagem"
                        Label1.Text = Text
                        Label2.Text = "Escolha uma tarefa:"
                        If ComboBox1.SelectedItem = "" Then Label3.Text = "Escolha uma tarefa para ver a sua descrição"
                        Label4.Text = "Não existem opções configuráveis para esta tarefa. No entanto, só deve executar esta tarefa para tentar recuperar uma imagem do Windows que não arranque."
                        Label5.Text = "A reinicialização da base de componentes substituídos foi executada pela última vez em:"
                        Label7.Text = "Só deve marcar esta opção se a reposição de base demorar mais de 30 minutos a concluir"
                        Label8.Text = "Não existem opções configuráveis para esta tarefa."
                        Label9.Text = "Não existem opções configuráveis para esta tarefa."
                        Label10.Text = "Não existem opções configuráveis para esta tarefa."
                        Label11.Text = "Fonte:"
                        Label12.Text = "Seleccione uma tarefa listada acima para configurar as suas opções."
                        GroupBox1.Text = "Opções da tarefa"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        Tasks(0) = "Reverter acções pendentes"
                        Tasks(1) = "Limpar ficheiros de cópia de segurança do Service Pack"
                        Tasks(2) = "Limpar o armazenamento de componentes"
                        Tasks(3) = "Analisar o armazenamento de componentes"
                        Tasks(4) = "Verificar o arquivo de componentes"
                        Tasks(5) = "Verificar se o arquivo de componentes está corrompido"
                        Tasks(6) = "Reparar o arquivo de componentes"
                        ComboBox1.Items.AddRange(Tasks)
                        HealthRestoreSourceOFD.Title = "Especificar a fonte a partir da qual iremos restaurar o estado do armazenamento de componentes"
                        Button1.Text = "Navegar..."
                        Button2.Text = "Detetar a partir da política de grupo"
                        CheckBox1.Text = "Ocultar o service pack da lista de actualizações instaladas"
                        CheckBox2.Text = "Repor a base de componentes substituídos"
                        CheckBox3.Text = "Adiar operações de limpeza de longa duração"
                        CheckBox4.Text = "Utilizar outra fonte para reparação de componentes"
                        CheckBox5.Text = "Limitar o acesso ao Windows Update"
                    Case "ITA"
                        Text = "Pulizia immagine"
                        Label1.Text = Text
                        Label2.Text = "Scegliere un'attività:"
                        If ComboBox1.SelectedItem = "" Then Label3.Text = "Scegliere un'attività per vederne la descrizione"
                        Label4.Text = "Non ci sono opzioni configurabili per questa attività. Tuttavia, si dovrebbe eseguire questa attività solo per tentare di ripristinare un'immagine di Windows che non riesce ad avviarsi"
                        Label5.Text = "Il ripristino della base dei componenti sostituiti è stato eseguito l'ultima volta in data:"
                        Label7.Text = "Selezionare questa opzione solo se il ripristino della base richiede più di 30 minuti per essere completato"
                        Label8.Text = "Non ci sono opzioni configurabili per questa attività"
                        Label9.Text = "Non ci sono opzioni configurabili per questa attività"
                        Label10.Text = "Non ci sono opzioni configurabili per questa attività"
                        Label11.Text = "Fonte:"
                        Label12.Text = "Selezionare un'attività elencata sopra per configurarne le opzioni"
                        GroupBox1.Text = "Opzioni dell'attività"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        Tasks(0) = "Annullamento delle azioni in sospeso"
                        Tasks(1) = "Pulisci i file di backup del Service Pack"
                        Tasks(2) = "Ripulire l'archivio dei componenti"
                        Tasks(3) = "Analizzare l'archivio dei componenti"
                        Tasks(4) = "Controllare l'archivio dei componenti"
                        Tasks(5) = "Scansiona l'archivio dei componenti per individuare eventuali corruzioni"
                        Tasks(6) = "Ripara l'archivio dei componenti"
                        ComboBox1.Items.AddRange(Tasks)
                        HealthRestoreSourceOFD.Title = "Specificare l'origine da cui ripristinare lo stato di salute dell'archivio componenti"
                        Button1.Text = "Sfogliare..."
                        Button2.Text = "Rileva dai criteri di gruppo"
                        CheckBox1.Text = "Nascondi il service pack dall'elenco degli aggiornamenti installati"
                        CheckBox2.Text = "Ripristina la base dei componenti sostituiti"
                        CheckBox3.Text = "Rimanda le operazioni di pulizia di lunga durata"
                        CheckBox4.Text = "Usa un'altra fonte per la riparazione dei componenti"
                        CheckBox5.Text = "Limita l'accesso a Windows Update"
                End Select
            Case 1
                Text = "Image cleanup"
                Label1.Text = Text
                Label2.Text = "Choose a task:"
                If ComboBox1.SelectedItem = "" Then Label3.Text = "Choose a task to see its description"
                Label4.Text = "There are no configurable options for this task. However, you should only run this task to try to recover a Windows image that fails to boot."
                Label5.Text = "The superseded components base reset was last run on:"
                Label7.Text = "You should only check this option if the base reset takes more than 30 minutes to complete"
                Label8.Text = "There are no configurable options for this task."
                Label9.Text = "There are no configurable options for this task."
                Label10.Text = "There are no configurable options for this task."
                Label11.Text = "Source:"
                Label12.Text = "Select a task listed above to configure its options."
                GroupBox1.Text = "Task options"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                Tasks(0) = "Revert pending actions"
                Tasks(1) = "Clean up Service Pack backup files"
                Tasks(2) = "Clean up component store"
                Tasks(3) = "Analyze component store"
                Tasks(4) = "Check component store"
                Tasks(5) = "Scan component store for corruption"
                Tasks(6) = "Repair component store"
                ComboBox1.Items.AddRange(Tasks)
                HealthRestoreSourceOFD.Title = "Specify the source from which we will restore the component store health"
                Button1.Text = "Browse..."
                Button2.Text = "Detect from group policy"
                CheckBox1.Text = "Hide service pack from the Installed Updates list"
                CheckBox2.Text = "Reset base of superseded components"
                CheckBox3.Text = "Defer long-running cleanup operations"
                CheckBox4.Text = "Use different source for component repair"
                CheckBox5.Text = "Limit access to Windows Update"
            Case 2
                Text = "Limpieza de imagen"
                Label1.Text = Text
                Label2.Text = "Elija una tarea:"
                If ComboBox1.SelectedItem = "" Then Label3.Text = "Elija una tarea para ver su descripción"
                Label4.Text = "No hay opciones configurables para esta tarea. Sin embargo, debería ejecutar esta tarea para intentar recuperar una imagen de Windows que no inicia."
                Label5.Text = "El restablecimiento de la base de componentes sustituidos fue ejecutado por última vez en:"
                Label7.Text = "Debería marcar esta opción solo si el restablecimiento de la base tarda más de 30 minutos en completar"
                Label8.Text = "No hay opciones configurables para esta tarea."
                Label9.Text = "No hay opciones configurables para esta tarea."
                Label10.Text = "No hay opciones configurables para esta tarea."
                Label11.Text = "Origen:"
                Label12.Text = "Seleccione una tarea en la lista de arriba para configurar sus opciones."
                GroupBox1.Text = "Opciones de tarea"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                Tasks(0) = "Revertir acciones pendientes"
                Tasks(1) = "Limpiar archivos de copia de seguridad de Service Pack"
                Tasks(2) = "Limpiar almacén de componentes"
                Tasks(3) = "Analizar almacén de componentes"
                Tasks(4) = "Comprobar almacén de componentes"
                Tasks(5) = "Escanear almacén de componentes para detectar corrupción"
                Tasks(6) = "Reparar almacén de componentes"
                ComboBox1.Items.AddRange(Tasks)
                HealthRestoreSourceOFD.Title = "Especifique el origen desde donde restauraremos la salud del almacén de componentes"
                Button1.Text = "Examinar..."
                Button2.Text = "Detectar políticas de grupo"
                CheckBox1.Text = "Ocultar Service Pack del listado de Actualizaciones instaladas"
                CheckBox2.Text = "Restablecer la base de componentes sustituidos"
                CheckBox3.Text = "Diferir operaciones largas de limpieza"
                CheckBox4.Text = "Usar origen diferente para la reparación de componentes"
                CheckBox5.Text = "Limitar acceso a Windows Update"
            Case 3
                Text = "Nettoyage de l'image"
                Label1.Text = Text
                Label2.Text = "Choisissez une tâche :"
                If ComboBox1.SelectedItem = "" Then Label3.Text = "Choisissez une tâche pour voir sa description"
                Label4.Text = "Il n'y a pas d'options configurables pour cette tâche. Cependant, vous ne devez exécuter cette tâche que pour essayer de récupérer une image Windows qui ne démarre pas."
                Label5.Text = "La réinitialisation de la base des composants remplacés a été exécutée pour la dernière fois :"
                Label7.Text = "Ne cochez cette option que si la réinitialisation de la base prend plus de 30 minutes."
                Label8.Text = "Il n'y a pas d'options configurables pour cette tâche."
                Label9.Text = "Il n'y a pas d'options configurables pour cette tâche."
                Label10.Text = "Il n'y a pas d'options configurables pour cette tâche."
                Label11.Text = "Source :"
                Label12.Text = "Sélectionnez une tâche dans la liste ci-dessus pour configurer ses paramètres."
                GroupBox1.Text = "Paramètres de la tâche"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                Tasks(0) = "Annuler les actions en cours"
                Tasks(1) = "Nettoyer les fichiers de sauvegarde du Service Pack"
                Tasks(2) = "Nettoyer le stock de composants"
                Tasks(3) = "Analyser le stock de composants"
                Tasks(4) = "Vérifier le stock de composants"
                Tasks(5) = "Rechercher la corruption du stock de composants"
                Tasks(6) = "Réparer le stock de composants"
                ComboBox1.Items.AddRange(Tasks)
                HealthRestoreSourceOFD.Title = "Indiquez la source à partir de laquelle nous restaurerons l'état du stock de composants."
                Button1.Text = "Parcourir..."
                Button2.Text = "Détecter à partir des politiques de groupe"
                CheckBox1.Text = "Cacher le Service Pack de la liste des mises à jour installées"
                CheckBox2.Text = "Réinitialiser la base des composants remplacés"
                CheckBox3.Text = "Différer les opérations de nettoyage de longue durée"
                CheckBox4.Text = "Utiliser une autre source pour la réparation des composants"
                CheckBox5.Text = "Limiter l'accès à Windows Update"
            Case 4
                Text = "Limpeza de imagem"
                Label1.Text = Text
                Label2.Text = "Escolha uma tarefa:"
                If ComboBox1.SelectedItem = "" Then Label3.Text = "Escolha uma tarefa para ver a sua descrição"
                Label4.Text = "Não existem opções configuráveis para esta tarefa. No entanto, só deve executar esta tarefa para tentar recuperar uma imagem do Windows que não arranque."
                Label5.Text = "A reinicialização da base de componentes substituídos foi executada pela última vez em:"
                Label7.Text = "Só deve marcar esta opção se a reposição de base demorar mais de 30 minutos a concluir"
                Label8.Text = "Não existem opções configuráveis para esta tarefa."
                Label9.Text = "Não existem opções configuráveis para esta tarefa."
                Label10.Text = "Não existem opções configuráveis para esta tarefa."
                Label11.Text = "Fonte:"
                Label12.Text = "Seleccione uma tarefa listada acima para configurar as suas opções."
                GroupBox1.Text = "Opções da tarefa"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                Tasks(0) = "Reverter acções pendentes"
                Tasks(1) = "Limpar ficheiros de cópia de segurança do Service Pack"
                Tasks(2) = "Limpar o armazenamento de componentes"
                Tasks(3) = "Analisar o armazenamento de componentes"
                Tasks(4) = "Verificar o arquivo de componentes"
                Tasks(5) = "Verificar se o arquivo de componentes está corrompido"
                Tasks(6) = "Reparar o arquivo de componentes"
                ComboBox1.Items.AddRange(Tasks)
                HealthRestoreSourceOFD.Title = "Especificar a fonte a partir da qual iremos restaurar o estado do armazenamento de componentes"
                Button1.Text = "Navegar..."
                Button2.Text = "Detetar a partir da política de grupo"
                CheckBox1.Text = "Ocultar o service pack da lista de actualizações instaladas"
                CheckBox2.Text = "Repor a base de componentes substituídos"
                CheckBox3.Text = "Adiar operações de limpeza de longa duração"
                CheckBox4.Text = "Utilizar outra fonte para reparação de componentes"
                CheckBox5.Text = "Limitar o acesso ao Windows Update"
            Case 5
                Text = "Pulizia immagine"
                Label1.Text = Text
                Label2.Text = "Scegliere un'attività:"
                If ComboBox1.SelectedItem = "" Then Label3.Text = "Scegliere un'attività per vederne la descrizione"
                Label4.Text = "Non ci sono opzioni configurabili per questa attività. Tuttavia, si dovrebbe eseguire questa attività solo per tentare di ripristinare un'immagine di Windows che non riesce ad avviarsi"
                Label5.Text = "Il ripristino della base dei componenti sostituiti è stato eseguito l'ultima volta in data:"
                Label7.Text = "Selezionare questa opzione solo se il ripristino della base richiede più di 30 minuti per essere completato"
                Label8.Text = "Non ci sono opzioni configurabili per questa attività"
                Label9.Text = "Non ci sono opzioni configurabili per questa attività"
                Label10.Text = "Non ci sono opzioni configurabili per questa attività"
                Label11.Text = "Fonte:"
                Label12.Text = "Selezionare un'attività elencata sopra per configurarne le opzioni"
                GroupBox1.Text = "Opzioni dell'attività"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                Tasks(0) = "Annullamento delle azioni in sospeso"
                Tasks(1) = "Pulisci i file di backup del Service Pack"
                Tasks(2) = "Ripulire l'archivio dei componenti"
                Tasks(3) = "Analizzare l'archivio dei componenti"
                Tasks(4) = "Controllare l'archivio dei componenti"
                Tasks(5) = "Scansiona l'archivio dei componenti per individuare eventuali corruzioni"
                Tasks(6) = "Ripara l'archivio dei componenti"
                ComboBox1.Items.AddRange(Tasks)
                HealthRestoreSourceOFD.Title = "Specificare l'origine da cui ripristinare lo stato di salute dell'archivio componenti"
                Button1.Text = "Sfogliare..."
                Button2.Text = "Rileva dai criteri di gruppo"
                CheckBox1.Text = "Nascondi il service pack dall'elenco degli aggiornamenti installati"
                CheckBox2.Text = "Ripristina la base dei componenti sostituiti"
                CheckBox3.Text = "Rimanda le operazioni di pulizia di lunga durata"
                CheckBox4.Text = "Usa un'altra fonte per la riparazione dei componenti"
                CheckBox5.Text = "Limita l'accesso a Windows Update"
        End Select
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
        ComboBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        RichTextBox1.BackColor = BackColor
        RichTextBox1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        PictureBox2.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.image_dark, My.Resources.image_light)
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        ' Determine when the last base reset was run
        If MainForm.OnlineManagement Then
            Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing", False)
            Dim LastResetBase_UTC As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Could not get last base reset date. It is possible that no base resets were made").ToString()
                        Case "ESN"
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "No se pudo obtener la fecha de restablecimiento de base. Es posible que no se haya hecho ningún restablecimiento").ToString()
                        Case "FRA"
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Impossible d'obtenir la date de la dernière remise à zéro de la base. Il est possible qu'aucune réinitialisation de la base n'ait été effectuée.").ToString()
                        Case "PTB", "PTG"
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Não foi possível obter a data da última reposição de base. É possível que não tenham sido efectuadas reinicializações de base").ToString()
                        Case "ITA"
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Impossibile ottenere la data dell'ultimo ripristino della base. È possibile che non sia stato effettuato alcun azzeramento della base").ToString()
                    End Select
                Case 1
                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Could not get last base reset date. It is possible that no base resets were made").ToString()
                Case 2
                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "No se pudo obtener la fecha de restablecimiento de base. Es posible que no se haya hecho ningún restablecimiento").ToString()
                Case 3
                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Impossible d'obtenir la date de la dernière remise à zéro de la base. Il est possible qu'aucune réinitialisation de la base n'ait été effectuée.").ToString()
                Case 4
                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Não foi possível obter a data da última reposição de base. É possível que não tenham sido efectuadas reinicializações de base").ToString()
                Case 5
                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Impossibile ottenere la data dell'ultimo ripristino della base. È possibile che non sia stato effettuato alcun azzeramento della base").ToString()
            End Select
            regKey.Close()
            Dim charArray() As Char = LastResetBase_UTC.ToCharArray()
            If LastResetBase_UTC.Contains("/") Then charArray(10) = " "
            LastResetBase_UTC = New String(charArray)
            Label6.Text = LastResetBase_UTC
        Else
            Using reg As New Process
                reg.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
                reg.StartInfo.Arguments = "load HKLM\MountedSoft " & Quote & MainForm.MountDir & "\Windows\system32\config\software" & Quote
                reg.StartInfo.CreateNoWindow = True
                reg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                reg.Start()
                reg.WaitForExit()
                If reg.ExitCode = 0 Then
                    Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("MountedSoft\Microsoft\Windows\CurrentVersion\Component Based Servicing", False)
                    Dim LastResetBase_UTC As String = ""
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Could not get last base reset date. It is possible that no base resets were made").ToString()
                                Case "ESN"
                                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "No se pudo obtener la fecha de restablecimiento de base. Es posible que no se haya hecho ningún restablecimiento").ToString()
                                Case "FRA"
                                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Impossible d'obtenir la date de la dernière remise à zéro de la base. Il est possible qu'aucune réinitialisation de la base n'ait été effectuée.").ToString()
                                Case "PTB", "PTG"
                                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Não foi possível obter a data da última reposição de base. É possível que não tenham sido efectuadas reinicializações de base").ToString()
                                Case "ITA"
                                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Impossibile ottenere la data dell'ultimo ripristino della base. È possibile che non sia stato effettuato alcun azzeramento della base").ToString()
                            End Select
                        Case 1
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Could not get last base reset date. It is possible that no base resets were made").ToString()
                        Case 2
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "No se pudo obtener la fecha de restablecimiento de base. Es posible que no se haya hecho ningún restablecimiento").ToString()
                        Case 3
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Impossible d'obtenir la date de la dernière remise à zéro de la base. Il est possible qu'aucune réinitialisation de la base n'ait été effectuée.").ToString()
                        Case 4
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Não foi possível obter a data da última reposição de base. É possível que não tenham sido efectuadas reinicializações de base").ToString()
                        Case 5
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Impossibile ottenere la data dell'ultimo ripristino della base. È possibile che non sia stato effettuato alcun azzeramento della base").ToString()
                    End Select
                    regKey.Close()
                    Dim charArray() As Char = LastResetBase_UTC.ToCharArray()
                    If LastResetBase_UTC.Contains("/") Then charArray(10) = " "
                    LastResetBase_UTC = New String(charArray)
                    Label6.Text = LastResetBase_UTC
                    reg.StartInfo.Arguments = "unload HKLM\MountedSoft"
                    reg.Start()
                    reg.WaitForExit()
                Else
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label6.Text = "Could not get last base reset date"
                                Case "ESN"
                                    Label6.Text = "No se pudo obtener la fecha del último restablecimiento de base"
                                Case "FRA"
                                    Label6.Text = "Impossible d'obtenir la date de la dernière réinitialisation de la base"
                                Case "PTB", "PTG"
                                    Label6.Text = "Não foi possível obter a data da última reposição de base."
                                Case "ITA"
                                    Label6.Text = "Impossibile ottenere la data dell'ultimo ripristino della base."
                            End Select
                        Case 1
                            Label6.Text = "Could not get last base reset date"
                        Case 2
                            Label6.Text = "No se pudo obtener la fecha del último restablecimiento de base"
                        Case 3
                            Label6.Text = "Impossible d'obtenir la date de la dernière réinitialisation de la base"
                        Case 4
                            Label6.Text = "Não foi possível obter a data da última reposição de base."
                        Case 5
                            Label6.Text = "Impossibile ottenere la data dell'ultimo ripristino della base."
                    End Select
                End If
            End Using
        End If

        If MainForm.OnlineManagement And (SystemInformation.BootMode = BootMode.Normal Or SystemInformation.BootMode = BootMode.FailSafeWithNetwork) Then
            CheckBox5.Enabled = True
        Else
            CheckBox5.Checked = False
            CheckBox5.Enabled = False
        End If

        If SelTask >= 0 And SelTask < ComboBox1.Items.Count Then ComboBox1.SelectedIndex = SelTask
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "" Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label3.Text = "Choose a task to see its description"
                        Case "ESN"
                            Label3.Text = "Elija una tarea para ver su descripción"
                        Case "FRA"
                            Label3.Text = "Choisissez une tâche pour voir sa description"
                        Case "PTB", "PTG"
                            Label3.Text = "Escolha uma tarefa para ver a sua descrição"
                        Case "ITA"
                            Label3.Text = "Scegliere un'attività per vederne la descrizione"
                    End Select
                Case 1
                    Label3.Text = "Choose a task to see its description"
                Case 2
                    Label3.Text = "Elija una tarea para ver su descripción"
                Case 3
                    Label3.Text = "Choisissez une tâche pour voir sa description"
                Case 4
                    Label3.Text = "Escolha uma tarefa para ver a sua descrição"
                Case 5
                    Label3.Text = "Scegliere un'attività per vederne la descrizione"
            End Select
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
        Else
            Select Case ComboBox1.SelectedIndex
                Case 0
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label3.Text = "If you experience a boot failure, this option can try to recover the system by reverting all pending actions from previous servicing operations"
                                Case "ESN"
                                    Label3.Text = "Si experimenta un error en el arranque, esta opción puede intentar recuperar el sistema revirtiendo todas las acciones pendientes de operaciones de servicio previas"
                                Case "FRA"
                                    Label3.Text = "En cas d'échec du démarrage, cette option permet d'essayer de récupérer le système en annulant toutes les actions en cours des opérations de maintenance précédentes."
                                Case "PTB", "PTG"
                                    Label3.Text = "Se ocorrer uma falha de arranque, esta opção pode tentar recuperar o sistema revertendo todas as acções pendentes de operações de manutenção anteriores"
                                Case "ITA"
                                    Label3.Text = "Se si verifica un errore di avvio, questa opzione può tentare di ripristinare il sistema annullando tutte le azioni in sospeso dalle precedenti operazioni di assistenza."
                            End Select
                        Case 1
                            Label3.Text = "If you experience a boot failure, this option can try to recover the system by reverting all pending actions from previous servicing operations"
                        Case 2
                            Label3.Text = "Si experimenta un error en el arranque, esta opción puede intentar recuperar el sistema revirtiendo todas las acciones pendientes de operaciones de servicio previas"
                        Case 3
                            Label3.Text = "En cas d'échec du démarrage, cette option permet d'essayer de récupérer le système en annulant toutes les actions en cours des opérations de maintenance précédentes."
                        Case 4
                            Label3.Text = "Se ocorrer uma falha de arranque, esta opção pode tentar recuperar o sistema revertendo todas as acções pendentes de operações de manutenção anteriores"
                        Case 5
                            Label3.Text = "Se si verifica un errore di avvio, questa opzione può tentare di ripristinare il sistema annullando tutte le azioni in sospeso dalle precedenti operazioni di assistenza."
                    End Select
                    Panel2.Visible = True
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 1
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label3.Text = "Removes backup files created during the installation of a service pack"
                                Case "ESN"
                                    Label3.Text = "Elimina archivos de copia de seguridad creados durante la instalación de un Service Pack"
                                Case "FRA"
                                    Label3.Text = "Supprime les fichiers de sauvegarde créés lors de l'installation d'un service pack"
                                Case "PTB", "PTG"
                                    Label3.Text = "Remove os ficheiros de cópia de segurança criados durante a instalação de um service pack"
                                Case "ITA"
                                    Label3.Text = "Rimuove i file di backup creati durante l'installazione di un service pack"
                            End Select
                        Case 1
                            Label3.Text = "Removes backup files created during the installation of a service pack"
                        Case 2
                            Label3.Text = "Elimina archivos de copia de seguridad creados durante la instalación de un Service Pack"
                        Case 3
                            Label3.Text = "Supprime les fichiers de sauvegarde créés lors de l'installation d'un service pack"
                        Case 4
                            Label3.Text = "Remove os ficheiros de cópia de segurança criados durante a instalação de um service pack"
                        Case 5
                            Label3.Text = "Rimuove i file di backup creati durante l'installazione di un service pack"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = True
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 2
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label3.Text = "Cleans up the superseded components and reduces the size of the component store"
                                Case "ESN"
                                    Label3.Text = "Limpia los componentes sustituidos y reduce el tamaño del almacén de componentes"
                                Case "FRA"
                                    Label3.Text = "Nettoie les composants remplacés et réduit la taille du stock de composants."
                                Case "PTB", "PTG"
                                    Label3.Text = "Limpa os componentes substituídos e reduz o tamanho do armazenamento de componentes"
                                Case "ITA"
                                    Label3.Text = "Pulisce i componenti sostituiti e riduce le dimensioni dell'archivio dei componenti"
                            End Select
                        Case 1
                            Label3.Text = "Cleans up the superseded components and reduces the size of the component store"
                        Case 2
                            Label3.Text = "Limpia los componentes sustituidos y reduce el tamaño del almacén de componentes"
                        Case 3
                            Label3.Text = "Nettoie les composants remplacés et réduit la taille du stock de composants."
                        Case 4
                            Label3.Text = "Limpa os componentes substituídos e reduz o tamanho do armazenamento de componentes"
                        Case 5
                            Label3.Text = "Pulisce i componenti sostituiti e riduce le dimensioni dell'archivio dei componenti"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = True
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 3
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label3.Text = "Creates a report of the component store, including its size"
                                Case "ESN"
                                    Label3.Text = "Crea un informe del almacén de componentes, incluyendo su tamaño"
                                Case "FRA"
                                    Label3.Text = "Crée un rapport sur le stock de composants, y compris sa taille."
                                Case "PTB", "PTG"
                                    Label3.Text = "Cria um relatório do armazenamento de componentes, incluindo o seu tamanho"
                                Case "ITA"
                                    Label3.Text = "Crea un rapporto sul magazzino dei componenti, comprese le dimensioni"
                            End Select
                        Case 1
                            Label3.Text = "Creates a report of the component store, including its size"
                        Case 2
                            Label3.Text = "Crea un informe del almacén de componentes, incluyendo su tamaño"
                        Case 3
                            Label3.Text = "Crée un rapport sur le stock de composants, y compris sa taille."
                        Case 4
                            Label3.Text = "Cria um relatório do armazenamento de componentes, incluindo o seu tamanho"
                        Case 5
                            Label3.Text = "Crea un rapporto sul magazzino dei componenti, comprese le dimensioni"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = True
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 4
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label3.Text = "Checks whether the image has been flagged as corrupted by a failed process and whether the corruption can be repaired"
                                Case "ESN"
                                    Label3.Text = "Comprueba si la imagen ha sido reportada como corrupta por un proceso fallido y si la corrupción puede ser reparada"
                                Case "FRA"
                                    Label3.Text = "Vérifie si l'image a été signalée comme corrompue par un processus ayant échoué et si la corruption peut être réparée."
                                Case "PTB", "PTG"
                                    Label3.Text = "Verifica se a imagem foi assinalada como corrompida por um processo falhado e se a corrupção pode ser reparada"
                                Case "ITA"
                                    Label3.Text = "Controlla se l'immagine è stata contrassegnata come corrotta da un processo non riuscito e se la corruzione può essere riparata"
                            End Select
                        Case 1
                            Label3.Text = "Checks whether the image has been flagged as corrupted by a failed process and whether the corruption can be repaired"
                        Case 2
                            Label3.Text = "Comprueba si la imagen ha sido reportada como corrupta por un proceso fallido y si la corrupción puede ser reparada"
                        Case 3
                            Label3.Text = "Vérifie si l'image a été signalée comme corrompue par un processus ayant échoué et si la corruption peut être réparée."
                        Case 4
                            Label3.Text = "Verifica se a imagem foi assinalada como corrompida por um processo falhado e se a corrupção pode ser reparada"
                        Case 5
                            Label3.Text = "Controlla se l'immagine è stata contrassegnata come corrotta da un processo non riuscito e se la corruzione può essere riparata"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = True
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 5
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label3.Text = "Scans the image for component store corruption, but does not perform repair options automatically"
                                Case "ESN"
                                    Label3.Text = "Escanea la imagen para comprobar corrupción en el almacén de componentes, pero no realiza operaciones de reparación automáticamente"
                                Case "FRA"
                                    Label3.Text = "Analyse l'image pour détecter une corruption du stock de composants, mais n'exécute pas automatiquement les options de réparation."
                                Case "PTB", "PTG"
                                    Label3.Text = "Analisa a imagem em busca de corrupção no armazenamento de componentes, mas não executa as opções de reparação automaticamente"
                                Case "ITA"
                                    Label3.Text = "Esegue la scansione dell'immagine per individuare eventuali danni all'archivio componenti, ma non esegue automaticamente le opzioni di riparazione"
                            End Select
                        Case 1
                            Label3.Text = "Scans the image for component store corruption, but does not perform repair options automatically"
                        Case 2
                            Label3.Text = "Escanea la imagen para comprobar corrupción en el almacén de componentes, pero no realiza operaciones de reparación automáticamente"
                        Case 3
                            Label3.Text = "Analyse l'image pour détecter une corruption du stock de composants, mais n'exécute pas automatiquement les options de réparation."
                        Case 4
                            Label3.Text = "Analisa a imagem em busca de corrupção no armazenamento de componentes, mas não executa as opções de reparação automaticamente"
                        Case 5
                            Label3.Text = "Esegue la scansione dell'immagine per individuare eventuali danni all'archivio componenti, ma non esegue automaticamente le opzioni di riparazione"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = True
                    Panel8.Visible = False
                Case 6
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label3.Text = "Scans the image for component store corruption and performs repair operations automatically"
                                Case "ESN"
                                    Label3.Text = "Escanea la imagen para comprobar corrupción en el almacén de componentes y realiza operaciones de reparación automáticamente"
                                Case "FRA"
                                    Label3.Text = "Analyse l'image pour détecter une corruption du stock de composants et effectue les opérations de réparation automatiquement."
                                Case "PTB", "PTG"
                                    Label3.Text = "Analisa a imagem em busca de corrupção no armazenamento de componentes e efectua operações de reparação automaticamente"
                                Case "ITA"
                                    Label3.Text = "Esegue la scansione dell'immagine per individuare eventuali danni all'archivio componenti ed esegue automaticamente le operazioni di riparazione"
                            End Select
                        Case 1
                            Label3.Text = "Scans the image for component store corruption and performs repair operations automatically"
                        Case 2
                            Label3.Text = "Escanea la imagen para comprobar corrupción en el almacén de componentes y realiza operaciones de reparación automáticamente"
                        Case 3
                            Label3.Text = "Analyse l'image pour détecter une corruption du stock de composants et effectue les opérations de réparation automatiquement."
                        Case 4
                            Label3.Text = "Analisa a imagem em busca de corrupção no armazenamento de componentes e efectua operações de reparação automaticamente"
                        Case 5
                            Label3.Text = "Esegue la scansione dell'immagine per individuare eventuali danni all'archivio componenti ed esegue automaticamente le operazioni di riparazione"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = True
            End Select
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        Label11.Enabled = CheckBox4.Checked = True
        RichTextBox1.Enabled = CheckBox4.Checked = True
        Button1.Enabled = CheckBox4.Checked = True
        Button2.Enabled = CheckBox4.Checked = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        HealthRestoreSourceOFD.ShowDialog()
    End Sub

    Private Sub HealthRestoreSourceOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles HealthRestoreSourceOFD.FileOk
        RichTextBox1.Text = HealthRestoreSourceOFD.FileName
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        RichTextBox1.Text = MainForm.GetSrcFromGPO()
        If RichTextBox1.Text.StartsWith("wim:\", StringComparison.OrdinalIgnoreCase) Then
            TextBoxSourcePanel.Visible = False
            WimFileSourcePanel.Visible = True
            Dim parts() As String = RichTextBox1.Text.Split(":")
            Label14.Text = parts(parts.Length - 1)
            Label13.Text = parts(1).Replace("\", "").Trim() & ":" & parts(2)
            If Label13.Text.EndsWith(":" & parts(parts.Length - 1)) Then Label13.Text = Label13.Text.Replace(":" & parts(parts.Length - 1), "").Trim()
        Else
            TextBoxSourcePanel.Visible = True
            WimFileSourcePanel.Visible = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        TextBoxSourcePanel.Visible = True
        WimFileSourcePanel.Visible = False
    End Sub

    Private Sub ImgCleanup_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SelTask = ComboBox1.SelectedIndex
    End Sub
End Class
