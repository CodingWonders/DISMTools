Imports System.Windows.Forms

Public Class RemCapabilities

    Dim capCount As Integer
    Dim capIds(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        Dim capIdList As New List(Of String)
        capCount = ListView1.CheckedItems.Count
        ProgressPanel.MountDir = MainForm.MountDir
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
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("There aren't any selected capabilities to remove. Please select some capabilities and try again.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ESN"
                            MsgBox("No hay funcionalidades seleccionadas para eliminar. Seleccione algunas de ellas e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                        Case "FRA"
                            MsgBox("Il n'y a pas de capacités sélectionnées à supprimer. Veuillez sélectionner des capacités et réessayer.", vbOKOnly + vbCritical, Label1.Text)
                        Case "PTB", "PTG"
                            MsgBox("Não existem quaisquer capacidades seleccionadas para remover. Por favor, seleccione algumas capacidades e tente novamente.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ITA"
                            MsgBox("Non ci sono capacità selezionate da rimuovere. Selezionare alcune funzionalità e riprovare.", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                Case 1
                    MsgBox("There aren't any selected capabilities to remove. Please select some capabilities and try again.", vbOKOnly + vbCritical, Label1.Text)
                Case 2
                    MsgBox("No hay funcionalidades seleccionadas para eliminar. Seleccione algunas de ellas e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                Case 3
                    MsgBox("Il n'y a pas de capacités sélectionnées à supprimer. Veuillez sélectionner des capacités et réessayer.", vbOKOnly + vbCritical, Label1.Text)
                Case 4
                    MsgBox("Não existem quaisquer capacidades seleccionadas para remover. Por favor, seleccione algumas capacidades e tente novamente.", vbOKOnly + vbCritical, Label1.Text)
                Case 5
                    MsgBox("Non ci sono capacità selezionate da rimuovere. Selezionare alcune funzionalità e riprovare.", vbOKOnly + vbCritical, Label1.Text)
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

    Private Sub RemCapabilities_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Remove capabilities"
                        Label1.Text = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        ListView1.Columns(0).Text = "Capability"
                        ListView1.Columns(1).Text = "State"
                    Case "ESN"
                        Text = "Eliminar funcionalidades"
                        Label1.Text = Text
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Funcionalidad"
                        ListView1.Columns(1).Text = "Estado"
                    Case "FRA"
                        Text = "Supprimer les capacités"
                        Label1.Text = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        ListView1.Columns(0).Text = "Capacité"
                        ListView1.Columns(1).Text = "État"
                    Case "PTB", "PTG"
                        Text = "Remover capacidades"
                        Label1.Text = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Capacidade"
                        ListView1.Columns(1).Text = "Estado"
                    Case "ITA"
                        Text = "Rimuovi capacità"
                        Label1.Text = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        ListView1.Columns(0).Text = "Capacità"
                        ListView1.Columns(1).Text = "Stato"
                End Select
            Case 1
                Text = "Remove capabilities"
                Label1.Text = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                ListView1.Columns(0).Text = "Capability"
                ListView1.Columns(1).Text = "State"
            Case 2
                Text = "Eliminar funcionalidades"
                Label1.Text = Text
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Funcionalidad"
                ListView1.Columns(1).Text = "Estado"
            Case 3
                Text = "Supprimer les capacités"
                Label1.Text = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                ListView1.Columns(0).Text = "Capacité"
                ListView1.Columns(1).Text = "État"
            Case 4
                Text = "Remover capacidades"
                Label1.Text = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Capacidade"
                ListView1.Columns(1).Text = "Estado"
            Case 5
                Text = "Rimuovi capacità"
                Label1.Text = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                ListView1.Columns(0).Text = "Capacità"
                ListView1.Columns(1).Text = "Stato"
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label2.Text &= " Only installed capabilities (" & ListView1.Items.Count & ") are shown"
                    Case "ESN"
                        Label2.Text &= " Solo las funcionalidades instaladas (" & ListView1.Items.Count & ") son mostradas"
                    Case "FRA"
                        Label2.Text &= " Seules les capacités installées (" & ListView1.Items.Count & ") sont représentées"
                    Case "PTB", "PTG"
                        Label2.Text &= " Só são mostradas as capacidades instaladas (" & ListView1.Items.Count & ")"
                    Case "ITA"
                        Label2.Text &= " Sono mostrate solo le capacità installate (" & ListView1.Items.Count & ")"
                End Select
            Case 1
                Label2.Text &= " Only installed capabilities (" & ListView1.Items.Count & ") are shown"
            Case 2
                Label2.Text &= " Solo las funcionalidades instaladas (" & ListView1.Items.Count & ") son mostradas"
            Case 3
                Label2.Text &= " Seules les capacités installées (" & ListView1.Items.Count & ") sont représentées"
            Case 4
                Label2.Text &= " Só são mostradas as capacidades instaladas (" & ListView1.Items.Count & ")"
            Case 5
                Label2.Text &= " Sono mostrate solo le capacità installate (" & ListView1.Items.Count & ")"
        End Select
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
