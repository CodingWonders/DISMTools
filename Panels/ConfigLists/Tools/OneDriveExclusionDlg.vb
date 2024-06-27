Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class OneDriveExclusionDlg

    Public ExcludedFolders As New List(Of String)
    Dim successfulExclusion As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ExcludeFolders(TextBox1.Text)
        If Not successfulExclusion Then Exit Sub
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label3.Text = "User OneDrive folders have been excluded and will be added to the configuration list."
                    Case "ESN"
                        Label3.Text = "Las carpetas de OneDrive del usuario han sido excluidas y serán añadidas a la lista de configuración."
                    Case "FRA"
                        Label3.Text = "Les répertoires OneDrive de l'utilisateur ont été exclus et seront ajoutés à la liste de configuration."
                    Case "PTB", "PTG"
                        Label3.Text = "As pastas do OneDrive dos utilizadores foram excluídas e serão adicionadas à lista de configuração."
                    Case "ITA"
                        Label3.Text = "Le cartelle OneDrive dell'utente sono state escluse e saranno aggiunte all'elenco di configurazione"
                End Select
            Case 1
                Label3.Text = "User OneDrive folders have been excluded and will be added to the configuration list."
            Case 2
                Label3.Text = "Las carpetas de OneDrive del usuario han sido excluidas y serán añadidas a la lista de configuración."
            Case 3
                Label3.Text = "Les répertoires OneDrive de l'utilisateur ont été exclus et seront ajoutés à la liste de configuration."
            Case 4
                Label3.Text = "As pastas do OneDrive dos utilizadores foram excluídas e serão adicionadas à lista de configuração."
            Case 5
                Label3.Text = "Le cartelle OneDrive dell'utente sono state escluse e saranno aggiunte all'elenco di configurazione"
        End Select
        Refresh()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Sub ExcludeFolders(ImagePath As String)
        If ImagePath = "" Or Not Directory.Exists(ImagePath) Then
            successfulExclusion = False
            Exit Sub
        End If
        If Directory.Exists(ImagePath & "\Users") Then
            Try
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label3.Text = "Excluding user OneDrive folders..."
                            Case "ESN"
                                Label3.Text = "Excluyendo carpetas de OneDrive del usuario..."
                            Case "FRA"
                                Label3.Text = "Exclusion des répertoires OneDrive de l'utilisateur en cours..."
                            Case "PTB", "PTG"
                                Label3.Text = "Excluir pastas do OneDrive dos utilizadores..."
                            Case "ITA"
                                Label3.Text = "Esclusione delle cartelle OneDrive dell'utente..."
                        End Select
                    Case 1
                        Label3.Text = "Excluding user OneDrive folders..."
                    Case 2
                        Label3.Text = "Excluyendo carpetas de OneDrive del usuario..."
                    Case 3
                        Label3.Text = "Exclusion des répertoires OneDrive de l'utilisateur en cours..."
                    Case 4
                        Label3.Text = "Excluir pastas do OneDrive dos utilizadores..."
                    Case 5
                        Label3.Text = "Esclusione delle cartelle OneDrive dell'utente..."
                End Select
                Refresh()
                ' Go through all User folders and exclude all OneDrive folders
                For Each UserDir In Directory.GetDirectories(ImagePath & "\Users", "*", SearchOption.TopDirectoryOnly)
                    If Directory.Exists(UserDir & "\OneDrive") Then
                        Dim excludedPath As String = ""
                        excludedPath = UserDir.Replace(ImagePath & "\", "\").Trim() & "\OneDrive"
                        ExcludedFolders.Add(excludedPath)
                    ElseIf Directory.Exists(UserDir & "\SkyDrive") Then
                        Dim excludedPath As String = ""
                        excludedPath = UserDir.Replace(ImagePath & "\", "\").Trim() & "\SkyDrive"
                        ExcludedFolders.Add(excludedPath)
                    End If
                Next
                successfulExclusion = True
            Catch ex As Exception
                successfulExclusion = False
            End Try
        Else
            successfulExclusion = False
        End If
    End Sub

    Private Sub OneDriveExclusionDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Exclude user OneDrive folders"
                        Label1.Text = "This tool will help you exclude user OneDrive folders in the configuration list you're working on. Simply specify the path to which you want to apply the configuration list file, and click Exclude." & CrLf & CrLf & _
                                      "NOTE: once you've run this tool and excluded user OneDrive folders, you shouldn't use the configuration list on an image other than the one you specify here. If you want to use the configuration list on other images, remove the user OneDrive folders in the configuration list and re-run this tool."
                        Label2.Text = "Path to exclude OneDrive folders from:"
                        Label3.Text = "When you're ready, click Exclude."
                        Button1.Text = "Browse..."
                        OK_Button.Text = "Exclude"
                        Cancel_Button.Text = "Cancel"
                        FolderBrowserDialog1.Description = "Choose a path that contains user folders:"
                    Case "ESN"
                        Text = "Excluir carpetas de OneDrive del usuario"
                        Label1.Text = "Esta herramienta le ayudará a excluir carpetas de OneDrive del usuario en la lista de configuración en la que esté trabajando. Especifique la ruta a la que desea aplicar el archivo de lista de configuración y haga clic en Excluir." & CrLf & CrLf & _
                                      "NOTA: una vez ejecutada esta herramienta y excluidas las carpetas de OneDrive del usuario, no debería utilizar la lista de configuración en una imagen distinta a la que especifique aquí. Si desea utilizar la lista en otras imágenes, elimine las carpetas de OneDrive en la lista de configuración y vuelva a ejecutar esta herramienta."
                        Label2.Text = "Ruta donde excluir las carpetas de OneDrive del usuario:"
                        Label3.Text = "Cuando esté listo, haga clic en Excluir."
                        Button1.Text = "Examinar..."
                        OK_Button.Text = "Excluir"
                        Cancel_Button.Text = "Cancelar"
                        FolderBrowserDialog1.Description = "Escoja una ruta que contenga carpetas de usuario:"
                    Case "FRA"
                        Text = "Exclure les répertoires OneDrive de l'utilisateur"
                        Label1.Text = "Cet outil vous aidera à exclure les répertoires OneDrive de l'utilisateur dans la liste de configuration sur laquelle vous travaillez. Indiquez simplement le chemin d'accès auquel vous souhaitez appliquer le fichier de la liste de configuration, puis cliquez sur Exclure." & CrLf & CrLf & _
                                      "REMARQUE : une fois que vous avez exécuté cet outil et exclu les répertoires OneDrive de l'utilisateur, vous ne devez pas utiliser la liste de configuration sur une image autre que celle que vous avez spécifiée ici. Si vous souhaitez utiliser la liste de configuration sur d'autres images, supprimez les répertoires OneDrive de l'utilisateur dans la liste de configuration et exécutez à nouveau cet outil."
                        Label2.Text = "Chemin d'accès à partir duquel exclure les répertoires OneDrive :"
                        Label3.Text = "Lorsque vous êtes prêt, cliquez sur Exclure."
                        Button1.Text = "Parcourir..."
                        OK_Button.Text = "Exclure"
                        Cancel_Button.Text = "Annuler"
                        FolderBrowserDialog1.Description = "Choisissez un chemin qui contient des répertoires d'utilisateurs :"
                    Case "PTB", "PTG"
                        Text = "Excluir pastas do OneDrive do utilizador"
                        Label1.Text = "Esta ferramenta irá ajudá-lo a excluir pastas do OneDrive de utilizadores na lista de configuração em que está a trabalhar. Basta especificar o caminho ao qual pretende aplicar o ficheiro da lista de configuração e clicar em Excluir." & CrLf & CrLf & _
                                      "NOTA: depois de executar esta ferramenta e excluir as pastas do OneDrive dos utilizadores, não deve utilizar a lista de configuração numa imagem que não seja a que especificou aqui. Se quiser usar a lista de configuração em outras imagens, remova as pastas do OneDrive do usuário na lista de configuração e execute esta ferramenta novamente."
                        Label2.Text = "Caminho para excluir as pastas do OneDrive de:"
                        Label3.Text = "Quando estiver pronto, clique em Excluir."
                        Button1.Text = "Navegar..."
                        OK_Button.Text = "Excluir"
                        Cancel_Button.Text = "Cancelar"
                        FolderBrowserDialog1.Description = "Escolha um caminho que contenha pastas dos utilizadores:"
                    Case "ITA"
                        Text = "Escludere le cartelle OneDrive dell'utente"
                        Label1.Text = "Questo strumento consente di escludere le cartelle OneDrive dell'utente dall'elenco di configurazione su cui si sta lavorando. È sufficiente specificare il percorso a cui si desidera applicare il file dell'elenco di configurazione e fare clic su Escludi." & CrLf & CrLf & _
                                      "NOTA: una volta eseguito questo strumento ed escluse le cartelle OneDrive dell'utente, non si dovrebbe usare l'elenco di configurazione su un'immagine diversa da quella specificata qui. Se si desidera utilizzare l'elenco di configurazione su altre immagini, rimuovere le cartelle OneDrive dell'utente nell'elenco di configurazione ed eseguire nuovamente questo strumento."
                        Label2.Text = "Percorso da cui escludere le cartelle OneDrive:"
                        Label3.Text = "Quando si è pronti, fare clic su Escludi"
                        Button1.Text = "Sfoglia..."
                        OK_Button.Text = "Escludere"
                        Cancel_Button.Text = "Annullare"
                        FolderBrowserDialog1.Description = "Scegliere un percorso che contenga le cartelle dell'utente:"
                End Select
            Case 1
                Text = "Exclude user OneDrive folders"
                Label1.Text = "This tool will help you exclude user OneDrive folders in the configuration list you're working on. Simply specify the path to which you want to apply the configuration list file, and click Exclude." & CrLf & CrLf & _
                              "NOTE: once you've run this tool and excluded user OneDrive folders, you shouldn't use the configuration list on an image other than the one you specify here. If you want to use the configuration list on other images, remove the user OneDrive folders in the configuration list and re-run this tool."
                Label2.Text = "Path to exclude OneDrive folders from:"
                Label3.Text = "When you're ready, click Exclude."
                Button1.Text = "Browse..."
                OK_Button.Text = "Exclude"
                Cancel_Button.Text = "Cancel"
                FolderBrowserDialog1.Description = "Choose a path that contains user folders:"
            Case 2
                Text = "Excluir carpetas de OneDrive del usuario"
                Label1.Text = "Esta herramienta le ayudará a excluir carpetas de OneDrive del usuario en la lista de configuración en la que esté trabajando. Especifique la ruta a la que desea aplicar el archivo de lista de configuración y haga clic en Excluir." & CrLf & CrLf & _
                              "NOTA: una vez ejecutada esta herramienta y excluidas las carpetas de OneDrive del usuario, no debería utilizar la lista de configuración en una imagen distinta a la que especifique aquí. Si desea utilizar la lista en otras imágenes, elimine las carpetas de OneDrive en la lista de configuración y vuelva a ejecutar esta herramienta."
                Label2.Text = "Ruta donde excluir las carpetas de OneDrive del usuario:"
                Label3.Text = "Cuando esté listo, haga clic en Excluir."
                Button1.Text = "Examinar..."
                OK_Button.Text = "Excluir"
                Cancel_Button.Text = "Cancelar"
                FolderBrowserDialog1.Description = "Escoja una ruta que contenga carpetas de usuario:"
            Case 3
                Text = "Exclure les répertoires OneDrive de l'utilisateur"
                Label1.Text = "Cet outil vous aidera à exclure les répertoires OneDrive de l'utilisateur dans la liste de configuration sur laquelle vous travaillez. Indiquez simplement le chemin d'accès auquel vous souhaitez appliquer le fichier de la liste de configuration, puis cliquez sur Exclure." & CrLf & CrLf & _
                              "REMARQUE : une fois que vous avez exécuté cet outil et exclu les répertoires OneDrive de l'utilisateur, vous ne devez pas utiliser la liste de configuration sur une image autre que celle que vous avez spécifiée ici. Si vous souhaitez utiliser la liste de configuration sur d'autres images, supprimez les répertoires OneDrive de l'utilisateur dans la liste de configuration et exécutez à nouveau cet outil."
                Label2.Text = "Chemin d'accès à partir duquel exclure les répertoires OneDrive :"
                Label3.Text = "Lorsque vous êtes prêt, cliquez sur Exclure."
                Button1.Text = "Parcourir..."
                OK_Button.Text = "Exclure"
                Cancel_Button.Text = "Annuler"
                FolderBrowserDialog1.Description = "Choisissez un chemin qui contient des répertoires d'utilisateurs :"
            Case 4
                Text = "Excluir pastas do OneDrive do utilizador"
                Label1.Text = "Esta ferramenta irá ajudá-lo a excluir pastas do OneDrive de utilizadores na lista de configuração em que está a trabalhar. Basta especificar o caminho ao qual pretende aplicar o ficheiro da lista de configuração e clicar em Excluir." & CrLf & CrLf & _
                              "NOTA: depois de executar esta ferramenta e excluir as pastas do OneDrive dos utilizadores, não deve utilizar a lista de configuração numa imagem que não seja a que especificou aqui. Se quiser usar a lista de configuração em outras imagens, remova as pastas do OneDrive do usuário na lista de configuração e execute esta ferramenta novamente."
                Label2.Text = "Caminho para excluir as pastas do OneDrive de:"
                Label3.Text = "Quando estiver pronto, clique em Excluir."
                Button1.Text = "Navegar..."
                OK_Button.Text = "Excluir"
                Cancel_Button.Text = "Cancelar"
                FolderBrowserDialog1.Description = "Escolha um caminho que contenha pastas dos utilizadores:"
            Case 5
                Text = "Escludere le cartelle OneDrive dell'utente"
                Label1.Text = "Questo strumento consente di escludere le cartelle OneDrive dell'utente dall'elenco di configurazione su cui si sta lavorando. È sufficiente specificare il percorso a cui si desidera applicare il file dell'elenco di configurazione e fare clic su Escludi." & CrLf & CrLf & _
                              "NOTA: una volta eseguito questo strumento ed escluse le cartelle OneDrive dell'utente, non si dovrebbe usare l'elenco di configurazione su un'immagine diversa da quella specificata qui. Se si desidera utilizzare l'elenco di configurazione su altre immagini, rimuovere le cartelle OneDrive dell'utente nell'elenco di configurazione ed eseguire nuovamente questo strumento."
                Label2.Text = "Percorso da cui escludere le cartelle OneDrive:"
                Label3.Text = "Quando si è pronti, fare clic su Escludi"
                Button1.Text = "Sfoglia..."
                OK_Button.Text = "Escludere"
                Cancel_Button.Text = "Annullare"
                FolderBrowserDialog1.Description = "Scegliere un percorso che contenga le cartelle dell'utente:"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        ExcludedFolders.Clear()
        successfulExclusion = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And Directory.Exists(TextBox1.Text) Then OK_Button.Enabled = True Else OK_Button.Enabled = False
    End Sub
End Class
