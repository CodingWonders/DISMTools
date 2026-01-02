Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities

Public Class EnableFeat
    Implements IImageTaskDialog

    Public featEnablementCount As Integer
    Public featEnablementNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        featEnablementCount = ListView1.CheckedItems.Count
        ProgressPanel.featEnablementCount = featEnablementCount
        DynaLog.LogMessage("Detecting features to enable...")
        If ListView1.CheckedItems.Count <= 0 Then
            DynaLog.LogMessage("No items have been added to the queue.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MessageBox.Show(MainForm, "Please select features to enable, and try again.", "No features selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "ESN"
                            MessageBox.Show(MainForm, "Seleccione las características a habilitar, e inténtelo de nuevo.", "No se ha seleccionado ninguna característica", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "FRA"
                            MessageBox.Show(MainForm, "Veuillez sélectionner les caractéristiques à activer et réessayer.", "Aucune caractéristique sélectionnée", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "PTB", "PTG"
                            MessageBox.Show(MainForm, "Por favor, seleccione as características a ativar e tente novamente.", "Nenhuma caraterística selecionada", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "ITA"
                            MessageBox.Show(MainForm, "Selezionare le caratteristiche da abilitare e riprovare", "Nessuna funzione selezionata", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Select
                Case 1
                    MessageBox.Show(MainForm, "Please select features to enable, and try again.", "No features selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 2
                    MessageBox.Show(MainForm, "Seleccione las características a habilitar, e inténtelo de nuevo.", "No se ha seleccionado ninguna característica", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 3
                    MessageBox.Show(MainForm, "Veuillez sélectionner les caractéristiques à activer et réessayer.", "Aucune caractéristique sélectionnée", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 4
                    MessageBox.Show(MainForm, "Por favor, seleccione as características a ativar e tente novamente.", "Nenhuma caraterística selecionada", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 5
                    MessageBox.Show(MainForm, "Selezionare le caratteristiche da abilitare e riprovare", "Nessuna funzione selezionata", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select
            Exit Sub
        Else
            Try
                For x = 0 To featEnablementCount - 1
                    featEnablementNames(x) = ListView1.CheckedItems(x).ToString()
                Next
                For x = 0 To featEnablementNames.Length
                    ProgressPanel.featEnablementNames(x) = featEnablementNames(x)
                Next
            Catch ex As Exception

            End Try
            DynaLog.LogMessage("Getting states of features for any missing sources...")
            For x = 0 To featEnablementCount - 1
                If MainForm.OnlineManagement And CheckBox4.Checked Then Exit For
                If ListView1.CheckedItems(x).SubItems(1).Text = "Removed" Or ListView1.CheckedItems(x).SubItems(1).Text = "Eliminado" Or ListView1.CheckedItems(x).SubItems(1).Text = "Supprimée" Or ListView1.CheckedItems(x).SubItems(1).Text = "Removido" Or ListView1.CheckedItems(x).SubItems(1).Text = "Rimosso" Then
                    If RichTextBox1.Text = "" Or Not Directory.Exists(RichTextBox1.Text) Then
                        DynaLog.LogMessage("No source has been specified or it does not exist.")
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        If MsgBox("Some features in this image require specifying a source for them to be enabled. The specified source is not valid for this operation." & CrLf & CrLf & If(RichTextBox1.Text = "", "Please specify a valid source and try again.", "Please make sure the source exists in the file system and try again."), vbOKOnly + vbCritical, "Enable features") = MsgBoxResult.Ok Then
                                            CheckBox2.Checked = True
                                            Button2.PerformClick()
                                        End If
                                    Case "ESN"
                                        If MsgBox("Algunas características en esta imagen requieren especificar un origen para ser habilitadas. El origen especificado no es válido para esta operación" & CrLf & CrLf & If(RichTextBox1.Text = "", "Especifique un origen válido e inténtelo de nuevo.", "Asegúrese de que el origen exista en el sistema de archivos e inténtelo de nuevo."), vbOKOnly + vbCritical, "Habilitar características") = MsgBoxResult.Ok Then
                                            CheckBox2.Checked = True
                                            Button2.PerformClick()
                                        End If
                                    Case "FRA"
                                        If MsgBox("Certaines caractéristiques de cette image nécessitent la spécification d'une source pour être activées. La source spécifiée n'est pas valide pour cette opération." & CrLf & CrLf & If(RichTextBox1.Text = "", "Veuillez indiquer une source valide et réessayer.", "Assurez-vous que la source existe dans le système de fichiers et réessayez."), vbOKOnly + vbCritical, "Activer les caractéristiques") = MsgBoxResult.Ok Then
                                            CheckBox2.Checked = True
                                            Button2.PerformClick()
                                        End If
                                    Case "PTB", "PTG"
                                        If MsgBox("Algumas características desta imagem requerem a especificação de uma origem para serem activadas. A origem especificada não é válida para esta operação." & CrLf & CrLf & If(RichTextBox1.Text = "", "Especifique uma origem válida e tente novamente.", "Certifique-se de que a origem existe no sistema de ficheiros e tente novamente."), vbOKOnly + vbCritical, "Ativar características") = MsgBoxResult.Ok Then
                                            CheckBox2.Checked = True
                                            Button2.PerformClick()
                                        End If
                                    Case "ITA"
                                        If MsgBox("Alcune caratteristiche di questa immagine richiedono l'indicazione di un'origine per essere abilitate. L'origine specificata non è valida per questa operazione." & CrLf & CrLf & If(RichTextBox1.Text = "", "Specificare un'origine valida e riprovare.", "Assicurarsi che l'origine esista nel file system e riprovare"), vbOKOnly + vbCritical, "Abilitare le caratteristiche") = MsgBoxResult.Ok Then
                                            CheckBox2.Checked = True
                                            Button2.PerformClick()
                                        End If
                                End Select
                            Case 1
                                If MsgBox("Some features in this image require specifying a source for them to be enabled. The specified source is not valid for this operation." & CrLf & CrLf & If(RichTextBox1.Text = "", "Please specify a valid source and try again.", "Please make sure the source exists in the file system and try again."), vbOKOnly + vbCritical, "Enable features") = MsgBoxResult.Ok Then
                                    CheckBox2.Checked = True
                                    Button2.PerformClick()
                                End If
                            Case 2
                                If MsgBox("Algunas características en esta imagen requieren especificar un origen para ser habilitadas. El origen especificado no es válido para esta operación" & CrLf & CrLf & If(RichTextBox1.Text = "", "Especifique un origen válido e inténtelo de nuevo.", "Asegúrese de que el origen exista en el sistema de archivos e inténtelo de nuevo."), vbOKOnly + vbCritical, "Habilitar características") = MsgBoxResult.Ok Then
                                    CheckBox2.Checked = True
                                    Button2.PerformClick()
                                End If
                            Case 3
                                If MsgBox("Certaines caractéristiques de cette image nécessitent la spécification d'une source pour être activées. La source spécifiée n'est pas valide pour cette opération." & CrLf & CrLf & If(RichTextBox1.Text = "", "Veuillez indiquer une source valide et réessayer.", "Assurez-vous que la source existe dans le système de fichiers et réessayez."), vbOKOnly + vbCritical, "Activer les caractéristiques") = MsgBoxResult.Ok Then
                                    CheckBox2.Checked = True
                                    Button2.PerformClick()
                                End If
                            Case 4
                                If MsgBox("Algumas características desta imagem requerem a especificação de uma origem para serem activadas. A origem especificada não é válida para esta operação." & CrLf & CrLf & If(RichTextBox1.Text = "", "Especifique uma origem válida e tente novamente.", "Certifique-se de que a origem existe no sistema de ficheiros e tente novamente."), vbOKOnly + vbCritical, "Ativar características") = MsgBoxResult.Ok Then
                                    CheckBox2.Checked = True
                                    Button2.PerformClick()
                                End If
                            Case 5
                                If MsgBox("Alcune caratteristiche di questa immagine richiedono l'indicazione di un'origine per essere abilitate. L'origine specificata non è valida per questa operazione." & CrLf & CrLf & If(RichTextBox1.Text = "", "Specificare un'origine valida e riprovare.", "Assicurarsi che l'origine esista nel file system e riprovare"), vbOKOnly + vbCritical, "Abilitare le caratteristiche") = MsgBoxResult.Ok Then
                                    CheckBox2.Checked = True
                                    Button2.PerformClick()
                                End If
                        End Select
                    Else

                    End If
                    Exit For
                End If
            Next
            ProgressPanel.featEnablementLastName = ListView1.CheckedItems(featEnablementCount - 1).ToString()
            If CheckBox1.Checked Then
                ProgressPanel.featisParentPkgNameUsed = True
                ProgressPanel.featParentPkgName = TextBox1.Text
            Else
                ProgressPanel.featisParentPkgNameUsed = False
                ProgressPanel.featParentPkgName = ""
            End If
            If CheckBox2.Checked Then
                ProgressPanel.featisSourceSpecified = True
                If RichTextBox1.Text = "" Or Not Directory.Exists(RichTextBox1.Text) Then
                    DynaLog.LogMessage("No source has been specified or it does not exist.")
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    MsgBox("The specified source is not valid. Please specify a valid source and try again", vbOKOnly + vbCritical, "Enable features")
                                Case "ESN"
                                    MsgBox("El origen especificado no es válido. Especifique uno válido e inténtelo de nuevo", vbOKOnly + vbCritical, "Habilitar características")
                                Case "FRA"
                                    MsgBox("La source spécifiée n'est pas valide. Veuillez indiquer une source valide et réessayer", vbOKOnly + vbCritical, "Activer les caractéristiques")
                                Case "PTB", "PTG"
                                    MsgBox("A origem especificada não é válida. Por favor, especifique uma origem válida e tente novamente", vbOKOnly + vbCritical, "Ativar características")
                                Case "ITA"
                                    MsgBox("La fonte specificata non è valida. Specificare un'origine valida e riprovare", vbOKOnly + vbCritical, "Abilita funzioni")
                            End Select
                        Case 1
                            MsgBox("The specified source is not valid. Please specify a valid source and try again", vbOKOnly + vbCritical, "Enable features")
                        Case 2
                            MsgBox("El origen especificado no es válido. Especifique uno válido e inténtelo de nuevo", vbOKOnly + vbCritical, "Habilitar características")
                        Case 3
                            MsgBox("La source spécifiée n'est pas valide. Veuillez indiquer une source valide et réessayer", vbOKOnly + vbCritical, "Activer les caractéristiques")
                        Case 4
                            MsgBox("A origem especificada não é válida. Por favor, especifique uma origem válida e tente novamente", vbOKOnly + vbCritical, "Ativar características")
                        Case 5
                            MsgBox("La fonte specificata non è valida. Specificare un'origine valida e riprovare", vbOKOnly + vbCritical, "Abilita funzioni")
                    End Select
                    Exit Sub
                Else
                    ProgressPanel.featSource = RichTextBox1.Text
                End If
            Else
                ProgressPanel.featisSourceSpecified = True
                ProgressPanel.featSource = ""
            End If
            If CheckBox3.Checked Then
                ProgressPanel.featParentIsEnabled = True
            Else
                ProgressPanel.featParentIsEnabled = False
            End If
            If CheckBox4.Checked Then
                ProgressPanel.featContactWindowsUpdate = True
            ElseIf CheckBox4.Checked = False And CheckBox4.Enabled Then
                ProgressPanel.featContactWindowsUpdate = False
            ElseIf CheckBox4.Enabled = False Then
                ' Tell program to contact Windows Update, as the parameter "/LimitAccess" doesn't apply to offline images
                ProgressPanel.featContactWindowsUpdate = True
            End If
            If CheckBox5.Checked And Not MainForm.OnlineManagement Then
                ProgressPanel.featEnablementCommit = True
            Else
                ProgressPanel.featEnablementCommit = False
            End If
        End If
        ProgressPanel.OperationNum = 30
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Opening feature enablement dialog...")
        ListView1.Items.Clear()
        DisableFeat.ListView1.Items.Clear()
        If Not MainForm.CompletedTasks(1) Then
            DynaLog.LogMessage("Feature background processes haven't completed.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        DynaLog.LogMessage("Adding features to arrays...")
        If MainForm.imgFeatures.Count > 0 Then
            For Each imgFeature In MainForm.imgFeatures.Where(Function(feature) Not New DismPackageFeatureState() {DismPackageFeatureState.Installed, DismPackageFeatureState.InstallPending}.Contains(feature.State)).ToList()
                ListView1.Items.Add(New ListViewItem(New String() {imgFeature.FeatureName, Casters.CastDismFeatureState(imgFeature.State, True)}))
            Next
        Else
            Try
                For x = 0 To Array.LastIndexOf(MainForm.imgFeatureNames, MainForm.imgFeatureNames.Last)
                    If MainForm.imgFeatureState(x).Contains("Enable") Or MainForm.imgFeatureState(x) = "" Or MainForm.imgFeatureState(x) = "Nothing" Then
                        Continue For
                    End If
                    ListView1.Items.Add(MainForm.imgFeatureNames(x)).SubItems.Add(MainForm.imgFeatureState(x))
                Next
            Catch ex As Exception
                ' We should have enough with the entries already added.
                Exit Try
            End Try
        End If
        Return True
    End Function

    Private Sub EnableFeature_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Enable features"
                        Label1.Text = Text
                        Label3.Text = "Package name:"
                        Label4.Text = "Feature source:"
                        Button1.Text = "Lookup..."
                        Button2.Text = "Browse..."
                        Button3.Text = "Detect from group policy"
                        Cancel_Button.Text = "Cancel"
                        OK_Button.Text = "OK"
                        GroupBox1.Text = "Features"
                        GroupBox2.Text = "Options"
                        CheckBox1.Text = "Specify parent package name for features"
                        CheckBox2.Text = "Specify feature source"
                        CheckBox3.Text = "Enable all parent features"
                        CheckBox4.Text = "Contact Windows Update for online images"
                        CheckBox5.Text = "Commit image after enabling features"
                        ListView1.Columns(0).Text = "Feature name"
                        ListView1.Columns(1).Text = "State"
                        FolderBrowserDialog1.Description = "Specify a folder which will act as the feature source:"
                    Case "ESN"
                        Text = "Habilitar característica"
                        Label1.Text = Text
                        Label3.Text = "Paquete:"
                        Label4.Text = "Origen:"
                        Button1.Text = "Consultar"
                        Button2.Text = "Examinar..."
                        Button3.Text = "Detectar políticas de grupo"
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "Aceptar"
                        GroupBox1.Text = "Características"
                        GroupBox2.Text = "Opciones"
                        CheckBox1.Text = "Especificar nombre de paquete principal para características"
                        CheckBox2.Text = "Especificar origen de características"
                        CheckBox3.Text = "Habilitar todas las características principales"
                        CheckBox4.Text = "Contactar Windows Update para instalaciones activas"
                        CheckBox5.Text = "Guardar imagen tras habilitar características"
                        ListView1.Columns(0).Text = "Nombre de característica"
                        ListView1.Columns(1).Text = "Estado"
                        FolderBrowserDialog1.Description = "Especifique una carpeta que actuará como origen de las características:"
                    Case "FRA"
                        Text = "Activer les caractéristiques"
                        Label1.Text = Text
                        Label3.Text = "Nom du paquet :"
                        Label4.Text = "Source de la caractéristique :"
                        Button1.Text = "Rechercher..."
                        Button2.Text = "Parcourir..."
                        Button3.Text = "Détecter à partir des politiques de groupe"
                        Cancel_Button.Text = "Annuler"
                        OK_Button.Text = "OK"
                        GroupBox1.Text = "Caractéristiques"
                        GroupBox2.Text = "Paramètres"
                        CheckBox1.Text = "Spécifier le nom du paquet parent pour les caractéristiques"
                        CheckBox2.Text = "Spécifier la source des caractéristiques"
                        CheckBox3.Text = "Activer toutes les caractéristiques des parents"
                        CheckBox4.Text = "Contacter Windows Update sur les images en ligne"
                        CheckBox5.Text = "Sauvegarder l'image après l'activation des caractéristiques"
                        ListView1.Columns(0).Text = "Nom de la caractéristique"
                        ListView1.Columns(1).Text = "État"
                        FolderBrowserDialog1.Description = "Spécifiez un répertoire qui servira de source des caractéristiques :"
                    Case "PTB", "PTG"
                        Text = "Ativar características"
                        Label1.Text = Text
                        Label3.Text = "Nome do pacote:"
                        Label4.Text = "Fonte da caraterística:"
                        Button1.Text = "Navegar..."
                        Button2.Text = "Navegar..."
                        Button3.Text = "Detetar a partir da política de grupo"
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "OK"
                        GroupBox1.Text = "Características"
                        GroupBox2.Text = "Opções"
                        CheckBox1.Text = "Especificar o nome do pacote principal para as características"
                        CheckBox2.Text = "Especificar a origem da caraterística"
                        CheckBox3.Text = "Ativar todas as características principais"
                        CheckBox4.Text = "Contactar o Windows Update para obter imagens online"
                        CheckBox5.Text = "Confirmar a imagem depois de ativar as funcionalidades"
                        ListView1.Columns(0).Text = "Nome da caraterística"
                        ListView1.Columns(1).Text = "Estado"
                        FolderBrowserDialog1.Description = "Especificar uma pasta que actuará como fonte da caraterística:"
                    Case "ITA"
                        Text = "Abilita funzionalità"
                        Label1.Text = Text
                        Label3.Text = "Nome pacchetto:"
                        Label4.Text = "Origine caratteristiche:"
                        Button1.Text = "Cerca..."
                        Button2.Text = "Sfoglia..."
                        Button3.Text = "Rileva da criteri di gruppo"
                        Cancel_Button.Text = "Annulla"
                        OK_Button.Text = "OK"
                        GroupBox1.Text = "Caratteristiche"
                        GroupBox2.Text = "Opzioni"
                        CheckBox1.Text = "Specifica il nome del pacchetto padre per le funzioni"
                        CheckBox2.Text = "Specificare l'origine delle caratteristiche"
                        CheckBox3.Text = "Abilita tutte le funzioni genitore"
                        CheckBox4.Text = "Contatta Windows Update per le immagini online"
                        CheckBox5.Text = "Applica l'immagine dopo aver abilitato le funzioni"
                        ListView1.Columns(0).Text = "Nome della funzione"
                        ListView1.Columns(1).Text = "Stato"
                        FolderBrowserDialog1.Description = "Specificare una cartella che fungerà da origine delle caratteristiche:"
                End Select
            Case 1
                Text = "Enable features"
                Label1.Text = Text
                Label3.Text = "Package name:"
                Label4.Text = "Feature source:"
                Button1.Text = "Lookup..."
                Button2.Text = "Browse..."
                Button3.Text = "Detect from group policy"
                Cancel_Button.Text = "Cancel"
                OK_Button.Text = "OK"
                GroupBox1.Text = "Features"
                GroupBox2.Text = "Options"
                CheckBox1.Text = "Specify parent package name for features"
                CheckBox2.Text = "Specify feature source"
                CheckBox3.Text = "Enable all parent features"
                CheckBox4.Text = "Contact Windows Update for online images"
                CheckBox5.Text = "Commit image after enabling features"
                ListView1.Columns(0).Text = "Feature name"
                ListView1.Columns(1).Text = "State"
                FolderBrowserDialog1.Description = "Specify a folder which will act as the feature source:"
            Case 2
                Text = "Habilitar característica"
                Label1.Text = Text
                Label3.Text = "Paquete:"
                Label4.Text = "Origen:"
                Button1.Text = "Consultar"
                Button2.Text = "Examinar..."
                Button3.Text = "Detectar políticas de grupo"
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "Aceptar"
                GroupBox1.Text = "Características"
                GroupBox2.Text = "Opciones"
                CheckBox1.Text = "Especificar nombre de paquete principal para características"
                CheckBox2.Text = "Especificar origen de características"
                CheckBox3.Text = "Habilitar todas las características principales"
                CheckBox4.Text = "Contactar Windows Update para instalaciones activas"
                CheckBox5.Text = "Guardar imagen tras habilitar características"
                ListView1.Columns(0).Text = "Nombre de característica"
                ListView1.Columns(1).Text = "Estado"
                FolderBrowserDialog1.Description = "Especifique una carpeta que actuará como origen de las características:"
            Case 3
                Text = "Activer les caractéristiques"
                Label1.Text = Text
                Label3.Text = "Nom du paquet :"
                Label4.Text = "Source de la caractéristique :"
                Button1.Text = "Rechercher..."
                Button2.Text = "Parcourir..."
                Button3.Text = "Détecter à partir des politiques de groupe"
                Cancel_Button.Text = "Annuler"
                OK_Button.Text = "OK"
                GroupBox1.Text = "Caractéristiques"
                GroupBox2.Text = "Paramètres"
                CheckBox1.Text = "Spécifier le nom du paquet parent pour les caractéristiques"
                CheckBox2.Text = "Spécifier la source des caractéristiques"
                CheckBox3.Text = "Activer toutes les caractéristiques des parents"
                CheckBox4.Text = "Contacter Windows Update sur les images en ligne"
                CheckBox5.Text = "Sauvegarder l'image après l'activation des caractéristiques"
                ListView1.Columns(0).Text = "Nom de la caractéristique"
                ListView1.Columns(1).Text = "État"
                FolderBrowserDialog1.Description = "Spécifiez un répertoire qui servira de source des caractéristiques :"
            Case 4
                Text = "Ativar características"
                Label1.Text = Text
                Label3.Text = "Nome do pacote:"
                Label4.Text = "Fonte da caraterística:"
                Button1.Text = "Navegar..."
                Button2.Text = "Navegar..."
                Button3.Text = "Detetar a partir da política de grupo"
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "OK"
                GroupBox1.Text = "Características"
                GroupBox2.Text = "Opções"
                CheckBox1.Text = "Especificar o nome do pacote principal para as características"
                CheckBox2.Text = "Especificar a origem da caraterística"
                CheckBox3.Text = "Ativar todas as características principais"
                CheckBox4.Text = "Contactar o Windows Update para obter imagens online"
                CheckBox5.Text = "Confirmar a imagem depois de ativar as funcionalidades"
                ListView1.Columns(0).Text = "Nome da caraterística"
                ListView1.Columns(1).Text = "Estado"
                FolderBrowserDialog1.Description = "Especificar uma pasta que actuará como fonte da caraterística:"
            Case 5
                Text = "Abilita funzionalità"
                Label1.Text = Text
                Label3.Text = "Nome pacchetto:"
                Label4.Text = "Origine caratteristiche:"
                Button1.Text = "Cerca..."
                Button2.Text = "Sfoglia..."
                Button3.Text = "Rileva da criteri di gruppo"
                Cancel_Button.Text = "Annulla"
                OK_Button.Text = "OK"
                GroupBox1.Text = "Caratteristiche"
                GroupBox2.Text = "Opzioni"
                CheckBox1.Text = "Specifica il nome del pacchetto padre per le funzioni"
                CheckBox2.Text = "Specificare l'origine delle caratteristiche"
                CheckBox3.Text = "Abilita tutte le funzioni genitore"
                CheckBox4.Text = "Contatta Windows Update per le immagini online"
                CheckBox5.Text = "Applica l'immagine dopo aver abilitato le funzioni"
                ListView1.Columns(0).Text = "Nome della funzione"
                ListView1.Columns(1).Text = "Stato"
                FolderBrowserDialog1.Description = "Specificare una cartella che fungerà da origine delle caratteristiche:"
        End Select
        Win10Title.BackColor = CurrentTheme.BackgroundColor
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        RichTextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        PictureBox2.Image = GetGlyphResource("image_glyph")
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        CheckBox5.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        DynaLog.LogMessage("Detecting ability to contact Windows Update (in the case of active installation management)...")
        DynaLog.LogMessage("Boot Mode of Host System: " & SystemInformation.BootMode.ToString())
        If MainForm.OnlineManagement And (SystemInformation.BootMode = BootMode.Normal Or SystemInformation.BootMode = BootMode.FailSafeWithNetwork) Then
            DynaLog.LogMessage("Host system is booted to either normal mode or Safe Mode with networking.")
            CheckBox4.Enabled = True
        Else
            If MainForm.OnlineManagement Then
                DynaLog.LogMessage("Host system is booted to Safe Mode. This mode does not have networking capabilities.")
            Else
                DynaLog.LogMessage("The active installation is not being managed. No online capabilities are supported, regardless of the mode the host system is in.")
            End If
            CheckBox4.Checked = False
            CheckBox4.Enabled = False
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label3.Enabled = True
            Button1.Enabled = True
        Else
            Label3.Enabled = False
            Button1.Enabled = False
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        Label4.Enabled = CheckBox2.Checked = True
        Button2.Enabled = CheckBox2.Checked = True
        RichTextBox1.Enabled = CheckBox2.Checked = True
        Button3.Enabled = CheckBox2.Checked = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PkgParentNameLookupDlg.pkgSource = MainForm.MountDir
        PkgParentNameLookupDlg.OriginatedFrom = "enablement"
        PkgParentNameLookupDlg.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            RichTextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DynaLog.LogMessage("Getting source established in the group policy...")
        RichTextBox1.Text = MainForm.GetSrcFromGPO()
        If RichTextBox1.Text.StartsWith("wim:\", StringComparison.OrdinalIgnoreCase) Then
            TextBoxSourcePanel.Visible = False
            WimFileSourcePanel.Visible = True
            Dim parts() As String = RichTextBox1.Text.Split(":")
            Label6.Text = parts(parts.Length - 1)
            Label5.Text = parts(1).Replace("\", "").Trim() & ":" & parts(2)
            If Label5.Text.EndsWith(":" & parts(parts.Length - 1)) Then Label5.Text = Label5.Text.Replace(":" & parts(parts.Length - 1), "").Trim()
        Else
            TextBoxSourcePanel.Visible = True
            WimFileSourcePanel.Visible = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        TextBoxSourcePanel.Visible = True
        WimFileSourcePanel.Visible = False
    End Sub
End Class
