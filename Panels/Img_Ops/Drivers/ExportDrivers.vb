Imports System.Windows.Forms
Imports System.IO

Public Class ExportDrivers

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextBox1.Text <> "" And Directory.Exists(TextBox1.Text) Then
            ProgressPanel.drvExportTarget = TextBox1.Text
        Else
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Please specify a target to export the drivers to and make sure that the specified target exists."
                        Case "ESN"
                            msg = "Especifique un destino al que exportar los controladores y asegúrese de que el destino especificado existe."
                        Case "FRA"
                            msg = "Veuillez spécifier une cible vers laquelle exporter les pilotes et assurez-vous que la cible spécifiée existe."
                        Case "PTB", "PTG"
                            msg = "Especifique um destino para o qual exportar os controladores e certifique-se de que o destino especificado existe."
                        Case "ITA"
                            msg = "Specificare una destinazione in cui esportare i driver e assicurarsi che la destinazione specificata esista"
                    End Select
                Case 1
                    msg = "Please specify a target to export the drivers to and make sure that the specified target exists."
                Case 2
                    msg = "Especifique un destino al que exportar los controladores y asegúrese de que el destino especificado existe."
                Case 3
                    msg = "Veuillez spécifier une cible vers laquelle exporter les pilotes et assurez-vous que la cible spécifiée existe."
                Case 4
                    msg = "Especifique um destino para o qual exportar os controladores e certifique-se de que o destino especificado existe."
                Case 5
                    msg = "Specificare una destinazione in cui esportare i driver e assicurarsi che la destinazione specificata esista"
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        ProgressPanel.OperationNum = 77
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ExportDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Export drivers"
                        Label1.Text = Text
                        Label2.Text = "Export target:"
                        Button1.Text = "Browse..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        FolderBrowserDialog1.Description = "Please specify the path where the drivers will be exported to:"
                    Case "ESN"
                        Text = "Exportar controladores"
                        Label1.Text = Text
                        Label2.Text = "Destino de exportación:"
                        Button1.Text = "Examinar..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        FolderBrowserDialog1.Description = "Especifique la ruta a la que los controladores serán exportados:"
                    Case "FRA"
                        Text = "Exporter les pilotes"
                        Label1.Text = Text
                        Label2.Text = "Cible d'exportation :"
                        Button1.Text = "Parcourir..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        FolderBrowserDialog1.Description = "Veuillez indiquer le chemin vers lequel les pilotes seront exportés :"
                    Case "PTB", "PTG"
                        Text = "Controladores de exportação"
                        Label1.Text = Text
                        Label2.Text = "Exportar destino:"
                        Button1.Text = "Navegar..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        FolderBrowserDialog1.Description = "Especifique o caminho para onde os controladores serão exportados:"
                    Case "ITA"
                        Text = "Esportazione di driver"
                        Label1.Text = Text
                        Label2.Text = "Destinazione di esportazione:"
                        Button1.Text = "Sfoglia..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        FolderBrowserDialog1.Description = "Specificare il percorso in cui verranno esportati i driver:"
                End Select
            Case 1
                Text = "Export drivers"
                Label1.Text = Text
                Label2.Text = "Export target:"
                Button1.Text = "Browse..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                FolderBrowserDialog1.Description = "Please specify the path where the drivers will be exported to:"
            Case 2
                Text = "Exportar controladores"
                Label1.Text = Text
                Label2.Text = "Destino de exportación:"
                Button1.Text = "Examinar..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                FolderBrowserDialog1.Description = "Especifique la ruta a la que los controladores serán exportados:"
            Case 3
                Text = "Exporter les pilotes"
                Label1.Text = Text
                Label2.Text = "Cible d'exportation :"
                Button1.Text = "Parcourir..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                FolderBrowserDialog1.Description = "Veuillez indiquer le chemin vers lequel les pilotes seront exportés :"
            Case 4
                Text = "Controladores de exportação"
                Label1.Text = Text
                Label2.Text = "Exportar destino:"
                Button1.Text = "Navegar..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                FolderBrowserDialog1.Description = "Especifique o caminho para onde os controladores serão exportados:"
            Case 5
                Text = "Esportazione di driver"
                Label1.Text = Text
                Label2.Text = "Destinazione di esportazione:"
                Button1.Text = "Sfoglia..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                FolderBrowserDialog1.Description = "Specificare il percorso in cui verranno esportati i driver:"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
End Class
