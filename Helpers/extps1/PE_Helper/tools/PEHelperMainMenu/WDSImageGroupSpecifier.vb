Imports System.Windows.Forms
Imports System.Xml.Serialization
Imports System.IO

Public Class WDSImageGroupSpecifier

    Public SpecifiedImageGroup As String
    Private WdsImageGroups As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ' If we chose to create the image group we'll do that first, then we select it
        If RadioButton2.Checked AndAlso Not CreateWdsImageGroup(TextBox1.Text) Then
            MessageBox.Show("The specified WDS image group could not be created.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        SpecifiedImageGroup = If(RadioButton1.Checked, ComboBox1.SelectedItem, TextBox1.Text)
        TextBox1.Text = ""
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub GetWdsGroups()
        Try
            Dim PSWdsGroupGetterOutput As String = GetGroupGetterOutput()
            WdsImageGroups.Clear()
            If PSWdsGroupGetterOutput <> "" Then
                Dim deserializer As New XmlSerializer(GetType(PSInterop.PsObjects))
                Dim objectsCollection As New PSInterop.PsObjects()
                Using reader As New StringReader(PSWdsGroupGetterOutput)
                    objectsCollection = CType(deserializer.Deserialize(reader), PSInterop.PsObjects)
                End Using
                If objectsCollection.Items.Count > 0 Then
                    For Each item In objectsCollection.Items
                        ComboBox1.Items.AddRange(item.Properties.Select(Function(prop) prop.Value).ToArray())
                        WdsImageGroups.AddRange(item.Properties.Select(Function(prop) prop.Value).ToArray())
                    Next
                End If
            End If
        Catch ex As Exception
            MsgBox("Could not get image groups.", vbOKOnly + vbCritical, Text)
        End Try
    End Sub

    Private Function CreateWdsImageGroup(WdsGroupName As String) As Boolean
        Dim exitCode As Integer

        Using PSWdsGroupCreatorProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                .Arguments = String.Format("-command New-WdsInstallImageGroup -Name '{0}'", WdsGroupName),
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }
        }
            PSWdsGroupCreatorProc.Start()
            PSWdsGroupCreatorProc.WaitForExit()
            exitCode = PSWdsGroupCreatorProc.ExitCode
        End Using

        Return exitCode = 0
    End Function

    Private Function GetGroupGetterOutput() As String
        Dim output As String = ""
        Using PSWdsGroupProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe",
                .WorkingDirectory = Application.StartupPath,
                .Arguments = "-command Get-WdsInstallImageGroup | Select-Object Name | ConvertTo-Xml -As String",
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .UseShellExecute = False,
                .RedirectStandardOutput = True
            }
        }
            Try
                ' Set the stdout encoding
                PSWdsGroupProc.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding(Globalization.CultureInfo.CurrentCulture.TextInfo.OEMCodePage)
            Catch ex As Exception
                PSWdsGroupProc.StartInfo.StandardOutputEncoding = Nothing
            End Try
            PSWdsGroupProc.Start()
            output = PSWdsGroupProc.StandardOutput.ReadToEnd()
            PSWdsGroupProc.WaitForExit()
        End Using
        Return output
    End Function

    Private Sub WDSImageGroupSpecifier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                Text = "Specify a group in your WDS server..."
                Label1.Text = "Choose an action:"
                Label2.Text = "This group already exists."
                RadioButton1.Text = "Upload this image to the following WDS image group:"
                RadioButton2.Text = "Create the following WDS image group for me and upload this image there:"
                Refresh_Button.Text = "Refresh"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case "ESN"
                Text = "Especificar un grupo en su servidor WDS..."
                Label1.Text = "Escoja una acción:"
                Label2.Text = "Este grupo ya existe."
                RadioButton1.Text = "Subir esta imagen al siguiente grupo de WDS:"
                RadioButton2.Text = "Crear el siguiente grupo de WDS por mí y subir esta imagen ahí:"
                Refresh_Button.Text = "Actuaizar"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case "FRA"
                Text = "Spécifiez un groupe sur votre serveur WDS..."
                Label1.Text = "Choisissez une action :"
                Label2.Text = "Ce groupe existe déjà."
                RadioButton1.Text = "Télécharger cette image dans le groupe d'images WDS suivant :"
                RadioButton2.Text = "Créer le groupe d'images WDS suivant pour moi et y télécharger cette image :"
                Refresh_Button.Text = "Actualiser"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case "PTB", "PTG"
                Text = "Especifique um grupo no seu servidor WDS..."
                Label1.Text = "Escolha uma ação:"
                Label2.Text = "Este grupo já existe."
                RadioButton1.Text = "Carregar esta imagem para o seguinte grupo de imagens WDS:"
                RadioButton2.Text = "Criar o seguinte grupo de imagens WDS para mim e carregar esta imagem para lá:"
                Refresh_Button.Text = "Atualizar"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
            Case "ITA"
                Text = "Specificare un gruppo nel proprio server WDS..."
                Label1.Text = "Scegliere un'azione:"
                Label2.Text = "Questo gruppo esiste già."
                RadioButton1.Text = "Carica questa immagine nel seguente gruppo di immagini WDS:"
                RadioButton2.Text = "Crea per me il seguente gruppo di immagini WDS e carica questa immagine lì:"
                Refresh_Button.Text = "Aggiorna"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annulla"
        End Select
        ComboBox1.Items.Clear()
        GetWdsGroups()
        Try
            ComboBox1.SelectedIndex = 0
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Refresh_Button_Click(sender As Object, e As EventArgs) Handles Refresh_Button.Click
        ComboBox1.Items.Clear()
        GetWdsGroups()
        Try
            ComboBox1.SelectedIndex = 0
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        ComboBox1.Enabled = RadioButton1.Checked
        TextBox1.Enabled = Not RadioButton1.Checked
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Label2.Visible = WdsImageGroups.Contains(TextBox1.Text)
    End Sub
End Class
