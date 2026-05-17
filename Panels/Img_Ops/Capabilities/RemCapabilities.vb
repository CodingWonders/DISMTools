Imports System.Windows.Forms
Imports Microsoft.Dism
Imports DISMTools.Utilities

Public Class RemCapabilities
    Implements IImageTaskDialog

    Dim capCount As Integer
    Dim capIds(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        Dim capIdList As New List(Of String)
        capCount = ListView1.CheckedItems.Count
        ProgressPanel.MountDir = MainForm.MountDir
        DynaLog.LogMessage("Detecting capabilities to remove...")
        If ListView1.CheckedItems.Count >= 1 Then
            For x = 0 To capCount - 1
                capIdList.Add(ListView1.CheckedItems(x).SubItems(0).Text)
            Next
            capIds = capIdList.ToArray()
            For x = 0 To capIds.Length - 1
                ProgressPanel.capRemovalIds(x) = capIds(x)
            Next
            ProgressPanel.capRemovalLastId = ListView1.CheckedItems(capCount - 1).SubItems(0).Text
        Else
            DynaLog.LogMessage("No items have been added to the queue.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("There aren't any selected capabilities to remove. Please select some capabilities and try again.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "ESN"
                            MsgBox("No hay funcionalidades seleccionadas para eliminar. Seleccione algunas de ellas e inténtelo de nuevo.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "FRA"
                            MsgBox("Il n'y a pas de capacités sélectionnées à supprimer. Veuillez sélectionner des capacités et réessayer.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "PTB", "PTG"
                            MsgBox("Não existem quaisquer capacidades seleccionadas para remover. Por favor, seleccione algumas capacidades e tente novamente.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "ITA"
                            MsgBox("Non ci sono capacità selezionate da rimuovere. Selezionare alcune funzionalità e riprovare.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    End Select
                Case 1
                    MsgBox("There aren't any selected capabilities to remove. Please select some capabilities and try again.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 2
                    MsgBox("No hay funcionalidades seleccionadas para eliminar. Seleccione algunas de ellas e inténtelo de nuevo.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 3
                    MsgBox("Il n'y a pas de capacités sélectionnées à supprimer. Veuillez sélectionner des capacités et réessayer.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 4
                    MsgBox("Não existem quaisquer capacidades seleccionadas para remover. Por favor, seleccione algumas capacidades e tente novamente.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 5
                    MsgBox("Non ci sono capacità selezionate da rimuovere. Selezionare alcune funzionalità e riprovare.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            End Select
            Exit Sub
        End If
        ProgressPanel.capRemovalCount = capCount
        ProgressPanel.OperationNum = 68
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Checking edition and version information for any unmet requirements...")
        If MainForm.CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not MainForm.IsWindows10OrHigher(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe") Then
            DynaLog.LogMessage("The image is not supported")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                        Case "PTB", "PTG"
                            MsgBox("Esta ação não é suportada nesta imagem", vbOKOnly + vbCritical, Text)
                        Case "ITA"
                            MsgBox("Questa azione non è supportata su questa immagine", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                Case 4
                    MsgBox("Esta ação não é suportada nesta imagem", vbOKOnly + vbCritical, Text)
                Case 5
                    MsgBox("Questa azione non è supportata su questa immagine", vbOKOnly + vbCritical, Text)
            End Select
            Return False
        End If
        DynaLog.LogMessage("All requirements are met. Continuing with the task...")
        ListView1.Items.Clear()
        If Not MainForm.CompletedTasks(3) Then
            DynaLog.LogMessage("Capability background processes haven't completed.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        DynaLog.LogMessage("Adding capabilities to arrays...")
        If MainForm.CurrentImage.ImageCapabilities IsNot Nothing AndAlso MainForm.CurrentImage.ImageCapabilities.Count > 0 Then
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageCapabilities.Where(Function(capability) New DismPackageFeatureState() {DismPackageFeatureState.Installed, DismPackageFeatureState.InstallPending}.Contains(capability.State)).Select(Function(capability) New ListViewItem(New String() {capability.Name, Casters.CastDismFeatureState(capability.State, True)})).ToArray())
        Else
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageCapabilities_Backup.Where(Function(capability) New DismPackageFeatureState() {DismPackageFeatureState.Installed, DismPackageFeatureState.InstallPending}.Contains(capability.CapabilityState)).Select(Function(capability) New ListViewItem(New String() {capability.CapabilityName, Casters.CastDismFeatureState(capability.CapabilityState, True)})).ToArray())
        End If
        Return True
    End Function

    Private Sub RemCapabilities_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Remove capabilities"
                        ImageTaskHeader1.ItemText = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        ListView1.Columns(0).Text = "Capability"
                        ListView1.Columns(1).Text = "State"
                    Case "ESN"
                        Text = "Eliminar funcionalidades"
                        ImageTaskHeader1.ItemText = Text
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Funcionalidad"
                        ListView1.Columns(1).Text = "Estado"
                    Case "FRA"
                        Text = "Supprimer les capacités"
                        ImageTaskHeader1.ItemText = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        ListView1.Columns(0).Text = "Capacité"
                        ListView1.Columns(1).Text = "État"
                    Case "PTB", "PTG"
                        Text = "Remover capacidades"
                        ImageTaskHeader1.ItemText = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Capacidade"
                        ListView1.Columns(1).Text = "Estado"
                    Case "ITA"
                        Text = "Rimuovi capacità"
                        ImageTaskHeader1.ItemText = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        ListView1.Columns(0).Text = "Capacità"
                        ListView1.Columns(1).Text = "Stato"
                End Select
            Case 1
                Text = "Remove capabilities"
                ImageTaskHeader1.ItemText = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                ListView1.Columns(0).Text = "Capability"
                ListView1.Columns(1).Text = "State"
            Case 2
                Text = "Eliminar funcionalidades"
                ImageTaskHeader1.ItemText = Text
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Funcionalidad"
                ListView1.Columns(1).Text = "Estado"
            Case 3
                Text = "Supprimer les capacités"
                ImageTaskHeader1.ItemText = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                ListView1.Columns(0).Text = "Capacité"
                ListView1.Columns(1).Text = "État"
            Case 4
                Text = "Remover capacidades"
                ImageTaskHeader1.ItemText = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Capacidade"
                ListView1.Columns(1).Text = "Estado"
            Case 5
                Text = "Rimuovi capacità"
                ImageTaskHeader1.ItemText = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                ListView1.Columns(0).Text = "Capacità"
                ListView1.Columns(1).Text = "Stato"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(1))
        ColumnHeader1.Width = WindowHelper.ScaleLogical(524)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(199)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub
End Class
