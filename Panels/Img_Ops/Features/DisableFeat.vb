Imports System.Windows.Forms
Imports Microsoft.Dism
Imports DISMTools.Utilities

Public Class DisableFeat
    Implements IImageTaskDialog

    Public featDisablementCount As Integer
    Public featDisablementNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        featDisablementCount = ListView1.CheckedItems.Count
        ProgressPanel.featDisablementCount = featDisablementCount
        DynaLog.LogMessage("Detecting features to disable...")
        If ListView1.CheckedItems.Count <= 0 Then
            DynaLog.LogMessage("No items have been added to the queue.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MessageBox.Show(MainForm, "Please select features to disable, and try again.", "No features selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "ESN"
                            MessageBox.Show(MainForm, "Seleccione las características a deshabilitar, e inténtelo de nuevo", "No hay características seleccionadas", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "FRA"
                            MessageBox.Show(MainForm, "Veuillez sélectionner les caractéristiques à désactiver et réessayer.", "Aucune caractéristique sélectionée", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "PTB", "PTG"
                            MessageBox.Show(MainForm, "Por favor, seleccione as características a desativar e tente novamente.", "Nenhuma caraterística selecionada", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "ITA"
                            MessageBox.Show(MainForm, "Selezionare le caratteristiche da disabilitare e riprovare", "Nessuna caratteristica selezionata", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Select
                Case 1
                    MessageBox.Show(MainForm, "Please select features to disable, and try again.", "No features selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 2
                    MessageBox.Show(MainForm, "Seleccione las características a deshabilitar, e inténtelo de nuevo", "No hay características seleccionadas", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 3
                    MessageBox.Show(MainForm, "Veuillez sélectionner les caractéristiques à désactiver et réessayer.", "Aucune caractéristique sélectionée", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 4
                    MessageBox.Show(MainForm, "Por favor, seleccione as características a desativar e tente novamente.", "Nenhuma caraterística selecionada", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 5
                    MessageBox.Show(MainForm, "Selezionare le caratteristiche da disabilitare e riprovare", "Nessuna caratteristica selezionata", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select
            Exit Sub
        Else
            Try
                For x = 0 To featDisablementCount - 1
                    featDisablementNames(x) = ListView1.CheckedItems(x).ToString()
                Next
                For x = 0 To featDisablementNames.Length
                    ProgressPanel.featDisablementNames(x) = featDisablementNames(x)
                Next
            Catch ex As Exception

            End Try
            ProgressPanel.featDisablementLastName = ListView1.CheckedItems(featDisablementCount - 1).ToString()
            If CheckBox1.Checked Then
                ProgressPanel.featDisablementParentPkgUsed = True
                ProgressPanel.featDisablementParentPkg = TextBox1.Text
            Else
                ProgressPanel.featDisablementParentPkgUsed = False
                ProgressPanel.featDisablementParentPkg = ""
            End If
            If CheckBox2.Checked Then
                ProgressPanel.featDisablementRemoveManifest = False
            Else
                ProgressPanel.featDisablementRemoveManifest = True
            End If
        End If
        ProgressPanel.OperationNum = 31
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
        DynaLog.LogMessage("Opening feature disablement dialog...")
        ListView1.Items.Clear()
        If Not MainForm.CompletedTasks(1) Then
            DynaLog.LogMessage("Feature background processes haven't completed.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        DynaLog.LogMessage("Adding features to arrays...")
        If MainForm.CurrentImage.ImageFeatures IsNot Nothing AndAlso MainForm.CurrentImage.ImageFeatures.Count > MainForm.CurrentImage.ImageFeatures_Backup.Count Then
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageFeatures.Where(Function(feature) Not New DismPackageFeatureState() {DismPackageFeatureState.NotPresent, DismPackageFeatureState.UninstallPending, DismPackageFeatureState.Staged, DismPackageFeatureState.Removed}.Contains(feature.State)).Select(Function(feature) New ListViewItem(New String() {feature.FeatureName, Casters.CastDismFeatureState(feature.State, True)})).ToArray())
        Else
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageFeatures_Backup.Where(Function(feature) Not New DismPackageFeatureState() {DismPackageFeatureState.NotPresent, DismPackageFeatureState.UninstallPending, DismPackageFeatureState.Staged, DismPackageFeatureState.Removed}.Contains(feature.FeatureState)).Select(Function(feature) New ListViewItem(New String() {feature.FeatureName, Casters.CastDismFeatureState(feature.FeatureState, True)})).ToArray())
        End If
        Return True
    End Function

    Private Sub DisableFeat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Disable features"
                        ImageTaskHeader1.ItemText = Text
                        Label3.Text = "Package name:"
                        GroupBox1.Text = "Features"
                        GroupBox2.Text = "Options"
                        Button1.Text = "Lookup..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        ListView1.Columns(0).Text = "Feature name"
                        ListView1.Columns(1).Text = "State"
                        CheckBox1.Text = "Specify parent package name for features"
                        CheckBox2.Text = "Remove feature without removing manifest"
                    Case "ESN"
                        Text = "Deshabilitar características"
                        ImageTaskHeader1.ItemText = Text
                        Label3.Text = "Paquete:"
                        GroupBox1.Text = "Características"
                        GroupBox2.Text = "Opciones"
                        Button1.Text = "Consultar"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Nombre de característica"
                        ListView1.Columns(1).Text = "Estado"
                        CheckBox1.Text = "Especificar nombre de paquete principal para las características"
                        CheckBox2.Text = "Eliminar característica sin eliminar manifiesto"
                    Case "FRA"
                        Text = "Désactiver des caractéristiques"
                        ImageTaskHeader1.ItemText = Text
                        Label3.Text = "Nom du paquet :"
                        GroupBox1.Text = "Caractéristiques"
                        GroupBox2.Text = "Paramètres"
                        Button1.Text = "Rechercher..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        ListView1.Columns(0).Text = "Nom de la caractéristique"
                        ListView1.Columns(1).Text = "État"
                        CheckBox1.Text = "Spécifier le nom du paquet parent pour les caractéristiques"
                        CheckBox2.Text = "Supprimer une caractéristique sans supprimer le manifeste"
                    Case "PTB", "PTG"
                        Text = "Desativar características"
                        ImageTaskHeader1.ItemText = Text
                        Label3.Text = "Nome do pacote:"
                        GroupBox1.Text = "Características"
                        GroupBox2.Text = "Opções"
                        Button1.Text = "Navegar..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Nome da caraterística"
                        ListView1.Columns(1).Text = "Estado"
                        CheckBox1.Text = "Especificar o nome do pacote principal para as características"
                        CheckBox2.Text = "Remover caraterística sem remover manifesto"
                    Case "ITA"
                        Text = "Disabilita caratteristiche"
                        ImageTaskHeader1.ItemText = Text
                        Label3.Text = "Nome pacchetto:"
                        GroupBox1.Text = "Caratteristiche"
                        GroupBox2.Text = "Opzioni"
                        Button1.Text = "Ricerca..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        ListView1.Columns(0).Text = "Nome caratteristica"
                        ListView1.Columns(1).Text = "Stato"
                        CheckBox1.Text = "Specificare il nome del pacchetto padre per le caratteristiche"
                        CheckBox2.Text = "Rimuovi la caratteristica senza rimuovere il manifesto"
                End Select
            Case 1
                Text = "Disable features"
                ImageTaskHeader1.ItemText = Text
                Label3.Text = "Package name:"
                GroupBox1.Text = "Features"
                GroupBox2.Text = "Options"
                Button1.Text = "Lookup..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                ListView1.Columns(0).Text = "Feature name"
                ListView1.Columns(1).Text = "State"
                CheckBox1.Text = "Specify parent package name for features"
                CheckBox2.Text = "Remove feature without removing manifest"
            Case 2
                Text = "Deshabilitar características"
                ImageTaskHeader1.ItemText = Text
                Label3.Text = "Paquete:"
                GroupBox1.Text = "Características"
                GroupBox2.Text = "Opciones"
                Button1.Text = "Consultar"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Nombre de característica"
                ListView1.Columns(1).Text = "Estado"
                CheckBox1.Text = "Especificar nombre de paquete principal para las características"
                CheckBox2.Text = "Eliminar característica sin eliminar manifiesto"
            Case 3
                Text = "Désactiver des caractéristiques"
                ImageTaskHeader1.ItemText = Text
                Label3.Text = "Nom du paquet :"
                GroupBox1.Text = "Caractéristiques"
                GroupBox2.Text = "Paramètres"
                Button1.Text = "Rechercher..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                ListView1.Columns(0).Text = "Nom de la caractéristique"
                ListView1.Columns(1).Text = "État"
                CheckBox1.Text = "Spécifier le nom du paquet parent pour les caractéristiques"
                CheckBox2.Text = "Supprimer une caractéristique sans supprimer le manifeste"
            Case 4
                Text = "Desativar características"
                ImageTaskHeader1.ItemText = Text
                Label3.Text = "Nome do pacote:"
                GroupBox1.Text = "Características"
                GroupBox2.Text = "Opções"
                Button1.Text = "Navegar..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Nome da caraterística"
                ListView1.Columns(1).Text = "Estado"
                CheckBox1.Text = "Especificar o nome do pacote principal para as características"
                CheckBox2.Text = "Remover caraterística sem remover manifesto"
            Case 5
                Text = "Disabilita caratteristiche"
                ImageTaskHeader1.ItemText = Text
                Label3.Text = "Nome pacchetto:"
                GroupBox1.Text = "Caratteristiche"
                GroupBox2.Text = "Opzioni"
                Button1.Text = "Ricerca..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                ListView1.Columns(0).Text = "Nome caratteristica"
                ListView1.Columns(1).Text = "Stato"
                CheckBox1.Text = "Specificare il nome del pacchetto padre per le caratteristiche"
                CheckBox2.Text = "Rimuovi la caratteristica senza rimuovere il manifesto"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)

        ColumnHeader1.Width = WindowHelper.ScaleLogical(372)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(339)
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PkgParentNameLookupDlg.pkgSource = MainForm.MountDir
        PkgParentNameLookupDlg.OriginatedFrom = "disablement"
        PkgParentNameLookupDlg.ShowDialog(Me)
    End Sub
End Class
