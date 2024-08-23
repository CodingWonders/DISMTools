Imports System.Windows.Forms
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities

Public Class GetFeatureInfoDlg

    Public InstalledFeatureInfo As DismFeatureCollection

    Private Sub GetFeatureInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            cPropPathView.BackColor = Color.FromArgb(31, 31, 31)
            cPropValue.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            cPropPathView.BackColor = Color.FromArgb(238, 238, 242)
            cPropValue.BackColor = Color.FromArgb(238, 238, 242)
        End If
        SearchBox1.BackColor = BackColor
        SearchBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        cPropPathView.ForeColor = ForeColor
        cPropValue.ForeColor = ForeColor
        SearchPic.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.search_dark, My.Resources.search_light)
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Get feature information"
                        Label1.Text = Text
                        Label2.Text = "Ready"
                        Label22.Text = "Feature name:"
                        Label24.Text = "Display name:"
                        Label26.Text = "Feature description:"
                        Label31.Text = "Is a restart required?"
                        Label36.Text = "Feature information"
                        Label37.Text = "Select an installed feature on the left to view its information here"
                        Label41.Text = "Feature state:"
                        Label43.Text = "Custom properties:"
                        ListView1.Columns(0).Text = "Feature name"
                        ListView1.Columns(1).Text = "Feature state"
                        Button2.Text = "Save..."
                        SearchBox1.cueBanner = "Type here to search for a feature..."
                    Case "ESN"
                        Text = "Obtener información de características"
                        Label1.Text = Text
                        Label2.Text = "Listo"
                        Label22.Text = "Nombre de característica:"
                        Label24.Text = "Nombre para mostrar:"
                        Label26.Text = "Descripción de la característica:"
                        Label31.Text = "¿Se requiere un reinicio?"
                        Label36.Text = "Información de la característica"
                        Label37.Text = "Seleccione una característica instalada en la izquierda para ver su información aquí"
                        Label41.Text = "Estado de la característica"
                        Label43.Text = "Propiedades personalizadas:"
                        ListView1.Columns(0).Text = "Nombre de característica"
                        ListView1.Columns(1).Text = "Estado"
                        Button2.Text = "Guardar..."
                        SearchBox1.cueBanner = "Escriba aquí para buscar una característica..."
                    Case "FRA"
                        Text = "Obtenir des informations sur les caractéristiques"
                        Label1.Text = Text
                        Label2.Text = "Prêt"
                        Label22.Text = "Nom de la caractéristique :"
                        Label24.Text = "Nom d'affichage :"
                        Label26.Text = "Description de la caractéristique :"
                        Label31.Text = "Un redémarrage est-il nécessaire ?"
                        Label36.Text = "Information sur la caractéristique"
                        Label37.Text = "Sélectionnez une caractéristique installée sur la gauche pour afficher ses informations ici"
                        Label41.Text = "État de la caractéristique :"
                        Label43.Text = "Propriétés personnalisées :"
                        ListView1.Columns(0).Text = "Nom de la caractéristique"
                        ListView1.Columns(1).Text = "État de la caractéristique"
                        Button2.Text = "Sauvegarder..."
                        SearchBox1.cueBanner = "Tapez ici pour rechercher une caractéristique..."
                    Case "PTB", "PTG"
                        Text = "Obter informações sobre a caraterística"
                        Label1.Text = Text
                        Label2.Text = "Pronto"
                        Label22.Text = "Nome da caraterística:"
                        Label24.Text = "Nome do ecrã:"
                        Label26.Text = "Descrição da caraterística:"
                        Label31.Text = "É necessário reiniciar?"
                        Label36.Text = "Informação sobre a caraterística"
                        Label37.Text = "Seleccione uma caraterística instalada à esquerda para ver as suas informações aqui"
                        Label41.Text = "Estado da funcionalidade:"
                        Label43.Text = "Propriedades personalizadas:"
                        ListView1.Columns(0).Text = "Nome da caraterística"
                        ListView1.Columns(1).Text = "Estado da caraterística"
                        Button2.Text = "Guardar..."
                        SearchBox1.cueBanner = "Digite aqui para pesquisar uma caraterística..."
                    Case "ITA"
                        Text = "Ottieni informazioni sulle caratteristiche"
                        Label1.Text = Text
                        Label2.Text = "Pronto"
                        Label22.Text = "Nome caratteristica:"
                        Label24.Text = "Nome visualizzato:"
                        Label26.Text = "Descrizione caratteristica:"
                        Label31.Text = "È necessario un riavvio?"
                        Label36.Text = "Informazioni sulla caratteristica"
                        Label37.Text = "Selezionare una funzione installata a sinistra per visualizzarne le informazioni qui"
                        Label41.Text = "Stato della funzione:"
                        Label43.Text = "Proprietà personalizzate:"
                        ListView1.Columns(0).Text = "Nome della funzione"
                        ListView1.Columns(1).Text = "Stato della caratteristica"
                        Button2.Text = "Salva..."
                        SearchBox1.cueBanner = "Digitare qui per cercare una caratteristica..."
                End Select
            Case 1
                Text = "Get feature information"
                Label1.Text = Text
                Label2.Text = "Ready"
                Label22.Text = "Feature name:"
                Label24.Text = "Display name:"
                Label26.Text = "Feature description:"
                Label31.Text = "Is a restart required?"
                Label36.Text = "Feature information"
                Label37.Text = "Select an installed feature on the left to view its information here"
                Label41.Text = "Feature state:"
                Label43.Text = "Custom properties:"
                ListView1.Columns(0).Text = "Feature name"
                ListView1.Columns(1).Text = "Feature state"
                Button2.Text = "Save..."
                SearchBox1.cueBanner = "Type here to search for a feature..."
            Case 2
                Text = "Obtener información de características"
                Label1.Text = Text
                Label2.Text = "Listo"
                Label22.Text = "Nombre de característica:"
                Label24.Text = "Nombre para mostrar:"
                Label26.Text = "Descripción de la característica:"
                Label31.Text = "¿Se requiere un reinicio?"
                Label36.Text = "Información de la característica"
                Label37.Text = "Seleccione una característica instalada en la izquierda para ver su información aquí"
                Label41.Text = "Estado de la característica"
                Label43.Text = "Propiedades personalizadas:"
                ListView1.Columns(0).Text = "Nombre de característica"
                ListView1.Columns(1).Text = "Estado"
                Button2.Text = "Guardar..."
                SearchBox1.cueBanner = "Escriba aquí para buscar una característica..."
            Case 3
                Text = "Obtenir des informations sur les caractéristiques"
                Label1.Text = Text
                Label2.Text = "Prêt"
                Label22.Text = "Nom de la caractéristique :"
                Label24.Text = "Nom d'affichage :"
                Label26.Text = "Description de la caractéristique :"
                Label31.Text = "Un redémarrage est-il nécessaire ?"
                Label36.Text = "Information sur la caractéristique"
                Label37.Text = "Sélectionnez une caractéristique installée sur la gauche pour afficher ses informations ici"
                Label41.Text = "État de la caractéristique :"
                Label43.Text = "Propriétés personnalisées :"
                ListView1.Columns(0).Text = "Nom de la caractéristique"
                ListView1.Columns(1).Text = "État de la caractéristique"
                Button2.Text = "Sauvegarder..."
                SearchBox1.cueBanner = "Tapez ici pour rechercher une caractéristique..."
            Case 4
                Text = "Obter informações sobre a caraterística"
                Label1.Text = Text
                Label2.Text = "Pronto"
                Label22.Text = "Nome da caraterística:"
                Label24.Text = "Nome do ecrã:"
                Label26.Text = "Descrição da caraterística:"
                Label31.Text = "É necessário reiniciar?"
                Label36.Text = "Informação sobre a caraterística"
                Label37.Text = "Seleccione uma caraterística instalada à esquerda para ver as suas informações aqui"
                Label41.Text = "Estado da funcionalidade:"
                Label43.Text = "Propriedades personalizadas:"
                ListView1.Columns(0).Text = "Nome da caraterística"
                ListView1.Columns(1).Text = "Estado da caraterística"
                Button2.Text = "Guardar..."
                SearchBox1.cueBanner = "Digite aqui para pesquisar uma caraterística..."
            Case 5
                Text = "Ottieni informazioni sulle caratteristiche"
                Label1.Text = Text
                Label2.Text = "Pronto"
                Label22.Text = "Nome caratteristica:"
                Label24.Text = "Nome visualizzato:"
                Label26.Text = "Descrizione caratteristica:"
                Label31.Text = "È necessario un riavvio?"
                Label36.Text = "Informazioni sulla caratteristica"
                Label37.Text = "Selezionare una funzione installata a sinistra per visualizzarne le informazioni qui"
                Label41.Text = "Stato della funzione:"
                Label43.Text = "Proprietà personalizzate:"
                ListView1.Columns(0).Text = "Nome della funzione"
                ListView1.Columns(1).Text = "Stato della caratteristica"
                Button2.Text = "Salva..."
                SearchBox1.cueBanner = "Digitare qui per cercare una caratteristica..."
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        ' Populate feature information list
        Panel4.Visible = False
        Panel7.Visible = True
        ListView1.Items.Clear()
        For Each InstalledFeature As DismFeature In InstalledFeatureInfo
            ListView1.Items.Add(New ListViewItem(New String() {InstalledFeature.FeatureName, Casters.CastDismFeatureState(InstalledFeature.State, True)}))
        Next
        SearchBox1.Text = ""
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            If ListView1.SelectedItems.Count = 1 Then
                ' Background processes need to have completed before showing information
                If MainForm.ImgBW.IsBusy Then
                    Dim msg As String = ""
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "Background processes need to have completed before showing feature information. We'll wait until they have completed"
                                Case "ESN"
                                    msg = "Los procesos en segundo plano deben haber completado antes de obtener información de la característica. Esperaremos hasta que hayan completado"
                                Case "FRA"
                                    msg = "Les processus en plan doivent être terminés avant d'afficher les caractéristiques. Nous attendrons qu'ils soient terminés"
                                Case "PTB", "PTG"
                                    msg = "Os processos em segundo plano têm de estar concluídos antes de mostrar informações sobre as características. Vamos esperar até que estejam concluídos"
                                Case "ITA"
                                    msg = "I processi in background devono essere completati prima di mostrare le informazioni sulle caratteristiche. Aspetteremo che siano completati"
                            End Select
                        Case 1
                            msg = "Background processes need to have completed before showing feature information. We'll wait until they have completed"
                        Case 2
                            msg = "Los procesos en segundo plano deben haber completado antes de obtener información de la característica. Esperaremos hasta que hayan completado"
                        Case 3
                            msg = "Les processus en plan doivent être terminés avant d'afficher les caractéristiques. Nous attendrons qu'ils soient terminés"
                        Case 4
                            msg = "Os processos em segundo plano têm de estar concluídos antes de mostrar informações sobre as características. Vamos esperar até que estejam concluídos"
                        Case 5
                            msg = "I processi in background devono essere completati prima di mostrare le informazioni sulle caratteristiche. Aspetteremo che siano completati"
                    End Select
                    MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label2.Text = "Waiting for background processes to finish..."
                                Case "ESN"
                                    Label2.Text = "Esperando a que terminen los procesos en segundo plano..."
                                Case "FRA"
                                    Label2.Text = "Attente de la fin des processus en arrière plan..."
                                Case "PTB", "PTG"
                                    Label2.Text = "À espera que os processos em segundo plano terminem..."
                                Case "ITA"
                                    Label2.Text = "In attesa che i processi in secondo piano finiscano..."
                            End Select
                        Case 1
                            Label2.Text = "Waiting for background processes to finish..."
                        Case 2
                            Label2.Text = "Esperando a que terminen los procesos en segundo plano..."
                        Case 3
                            Label2.Text = "Attente de la fin des processus en arrière plan..."
                        Case 4
                            Label2.Text = "À espera que os processos em segundo plano terminem..."
                        Case 5
                            Label2.Text = "In attesa che i processi in secondo piano finiscano..."
                    End Select
                    While MainForm.ImgBW.IsBusy
                        Application.DoEvents()
                        Thread.Sleep(500)
                    End While
                End If
                If MainForm.MountedImageDetectorBW.IsBusy Then
                    MainForm.MountedImageDetectorBW.CancelAsync()
                    While MainForm.MountedImageDetectorBW.IsBusy
                        Application.DoEvents()
                        Thread.Sleep(500)
                    End While
                End If
                MainForm.WatcherTimer.Enabled = False
                If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
                While MainForm.WatcherBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(100)
                End While
                cPropPathView.Nodes.Clear()
                cPropName.Text = ""
                cPropValue.Text = ""
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label2.Text = "Preparing to get feature information..."
                            Case "ESN"
                                Label2.Text = "Preparándonos para obtener información de la característica..."
                            Case "FRA"
                                Label2.Text = "Préparation de l'obtention des informations de la caractéristique en cours..."
                            Case "PTB", "PTG"
                                Label2.Text = "Preparar-se para obter informações sobre a característica..."
                            Case "ITA"
                                Label2.Text = "Preparazione per ottenere informazioni sulle caratteristiche..."
                        End Select
                    Case 1
                        Label2.Text = "Preparing to get feature information..."
                    Case 2
                        Label2.Text = "Preparándonos para obtener información de la característica..."
                    Case 3
                        Label2.Text = "Préparation de l'obtention des informations de la caractéristique en cours..."
                    Case 4
                        Label2.Text = "Preparar-se para obter informações sobre a característica..."
                    Case 5
                        Label2.Text = "Preparazione per ottenere informazioni sulle caratteristiche..."
                End Select
                Application.DoEvents()
                Try
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Label2.Text = "Getting information from " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                                    Case "ESN"
                                        Label2.Text = "Obteniendo información de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                                    Case "FRA"
                                        Label2.Text = "Obtention des informations de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & " en cours..."
                                    Case "PTB", "PTG"
                                        Label2.Text = "Obter informações de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                                    Case "ITA"
                                        Label2.Text = "Ottenere informazioni da " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                                End Select
                            Case 1
                                Label2.Text = "Getting information from " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                            Case 2
                                Label2.Text = "Obteniendo información de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                            Case 3
                                Label2.Text = "Obtention des informations de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & " en cours..."
                            Case 4
                                Label2.Text = "Obter informações de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                            Case 5
                                Label2.Text = "Ottenere informazioni da " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                        End Select
                        Application.DoEvents()
                        Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, ListView1.FocusedItem.SubItems(0).Text)
                        Label23.Text = featInfo.FeatureName
                        Label25.Text = featInfo.DisplayName
                        Label35.Text = featInfo.Description
                        Label32.Text = Casters.CastDismRestartType(featInfo.RestartRequired, True)
                        Label40.Text = Casters.CastDismFeatureState(featInfo.FeatureState, True)
                        Dim cProps As DismCustomPropertyCollection = featInfo.CustomProperties
                        If cProps.Count > 0 Then
                            Label42.Visible = False
                            CPropViewer.Visible = True
                            Dim cPropContents As String = ""
                            For Each cProp As DismCustomProperty In cProps
                                cPropContents &= "- " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
                            Next
                            PopulateTreeView(cPropPathView, cPropContents.Replace("- ", "").Trim())
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            cPropValue.Text = "Please select or expand an entry."
                                        Case "ESN"
                                            cPropValue.Text = "Por favor, seleccione o expanda una entrada."
                                        Case "FRA"
                                            cPropValue.Text = "Veuillez sélectionner ou étendre une entrée."
                                        Case "PTB", "PTG"
                                            cPropValue.Text = "Por favor, seleccione ou expanda uma entrada."
                                        Case "ITA"
                                            cPropValue.Text = "Selezionare o espandere un elemento."
                                    End Select
                                Case 1
                                    cPropValue.Text = "Please select or expand an entry."
                                Case 2
                                    cPropValue.Text = "Por favor, seleccione o expanda una entrada."
                                Case 3
                                    cPropValue.Text = "Veuillez sélectionner ou étendre une entrée."
                                Case 4
                                    cPropValue.Text = "Por favor, seleccione ou expanda uma entrada."
                                Case 5
                                    cPropValue.Text = "Selezionare o espandere un elemento."
                            End Select
                        Else
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            Label42.Text = "None"
                                        Case "ESN"
                                            Label42.Text = "Ninguna"
                                        Case "FRA"
                                            Label42.Text = "Aucune"
                                        Case "PTB", "PTG"
                                            Label42.Text = "Nenhum"
                                        Case "ITA"
                                            Label42.Text = "Nessuno"
                                    End Select
                                Case 1
                                    Label42.Text = "None"
                                Case 2
                                    Label42.Text = "Ninguna"
                                Case 3
                                    Label42.Text = "Aucune"
                                Case 4
                                    Label42.Text = "Nenhum"
                                Case 5
                                    Label42.Text = "Nessuno"
                            End Select
                            Label42.Visible = True
                            CPropViewer.Visible = False
                        End If
                    End Using
                Catch NRE As NullReferenceException
                    Panel4.Visible = False
                    Panel7.Visible = True
                Catch ex As Exception
                    Dim msg As String = ""
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "Could not get feature information. Reason: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                                Case "ESN"
                                    msg = "No pudimos obtener información de la característica. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                                Case "FRA"
                                    msg = "Impossible d'obtenir des informations sur les caractéristiques. Raison : " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                                Case "PTB", "PTG"
                                    msg = "Não foi possível obter informações sobre a característica. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                                Case "ITA"
                                    msg = "Impossibile ottenere informazioni sulle caratteristiche. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                            End Select
                        Case 1
                            msg = "Could not get feature information. Reason: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 2
                            msg = "No pudimos obtener información de la característica. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 3
                            msg = "Impossible d'obtenir des informations sur les caractéristiques. Raison : " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 4
                            msg = "Não foi possível obter informações sobre a característica. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 5
                            msg = "Impossibile ottenere informazioni sulle caratteristiche. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                    End Select
                    MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                Finally
                    DismApi.Shutdown()
                End Try
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label2.Text = "Ready"
                            Case "ESN"
                                Label2.Text = "Listo"
                            Case "FRA"
                                Label2.Text = "Prêt"
                            Case "PTB", "PTG"
                                Label2.Text = "Pronto"
                            Case "ITA"
                                Label2.Text = "Pronto"
                        End Select
                    Case 1
                        Label2.Text = "Ready"
                    Case 2
                        Label2.Text = "Listo"
                    Case 3
                        Label2.Text = "Prêt"
                    Case 4
                        Label2.Text = "Pronto"
                    Case 5
                        Label2.Text = "Pronto"
                End Select
                Panel4.Visible = True
                Panel7.Visible = False
            Else
                Panel4.Visible = False
                Panel7.Visible = True
            End If
        Catch ex As Exception
            Panel4.Visible = False
            Panel7.Visible = True
        End Try
    End Sub

    Private Sub PopulateTreeView(treeView As TreeView, input As String)
        Dim lines As String() = input.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
        For Each line As String In lines
            ' Split the line at the last colon to get the path and value
            Dim colonIndex As Integer = line.LastIndexOf(": ")
            If colonIndex = -1 Then Continue For ' Skip lines without a colon
            Dim path As String = line.Substring(0, colonIndex).Trim()
            Dim value As String = line.Substring(colonIndex + 1).Trim()
            Dim pathParts As String() = path.Split("\"c)
            AddNodeRecursive(treeView.Nodes, pathParts, 0, value)
        Next
    End Sub

    Private Sub AddNodeRecursive(parentNodes As TreeNodeCollection, parts As String(), startIndex As Integer, value As String)
        If startIndex >= parts.Length Then
            Return
        End If
        Dim nodeName As String = parts(startIndex).Trim()
        Dim existingNode As TreeNode = parentNodes.Cast(Of TreeNode)().FirstOrDefault(Function(n) n.Text = nodeName)
        If existingNode Is Nothing Then
            existingNode = New TreeNode(nodeName)
            parentNodes.Add(existingNode)
        Else
            If startIndex = parts.Length - 1 Then
                ' If the node already exists and we are at the last part, add a new node
                Dim newNode As New TreeNode(nodeName)
                newNode.Tag = value
                parentNodes.Add(newNode)
                Return
            End If
        End If
        If startIndex = parts.Length - 1 Then
            existingNode.Tag = value
        Else
            AddNodeRecursive(existingNode.Nodes, parts, startIndex + 1, value)
        End If
    End Sub

    Private Sub cPropPathView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles cPropPathView.AfterSelect
        cPropName.Text = cPropPathView.SelectedNode.Text
        Dim selectedNode As TreeNode = cPropPathView.SelectedNode
        If selectedNode IsNot Nothing AndAlso selectedNode.Tag IsNot Nothing Then
            cPropValue.Text = selectedNode.Tag.ToString()
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            cPropValue.Text = "No value has been defined. If the selected item has subitems, expand it."
                        Case "ESN"
                            cPropValue.Text = "No se ha definido un valor. Si el elemento seleccionado tiene elementos secundarios, expándalo."
                        Case "FRA"
                            cPropValue.Text = "Aucune valeur n'a été définie. Si l'élément sélectionné a des sous-éléments, développez-le."
                        Case "PTB", "PTG"
                            cPropValue.Text = "Nenhum valor foi definido. Se o item selecionado tiver subitens, expanda-o."
                        Case "ITA"
                            cPropValue.Text = "Non è stato definito alcun valore. Se l'elemento selezionato ha delle sottovoci, espandetelo."
                    End Select
                Case 1
                    cPropValue.Text = "No value has been defined. If the selected item has subitems, expand it."
                Case 2
                    cPropValue.Text = "No se ha definido un valor. Si el elemento seleccionado tiene elementos secundarios, expándalo."
                Case 3
                    cPropValue.Text = "Aucune valeur n'a été définie. Si l'élément sélectionné a des sous-éléments, développez-le."
                Case 4
                    cPropValue.Text = "Nenhum valor foi definido. Se o item selecionado tiver subitens, expanda-o."
                Case 5
                    cPropValue.Text = "Non è stato definito alcun valore. Se l'elemento selezionato ha delle sottovoci, espandetelo."
            End Select
        End If
    End Sub

    Private Sub GetFeatureInfoDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
        MainForm.WatcherTimer.Enabled = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MainForm.ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = MainForm.SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.OfflineMode = MainForm.OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = MainForm.SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = MainForm.AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 4
            ImgInfoSaveDlg.ShowDialog()
            InfoSaveResults.Show()
        End If
    End Sub

    Sub SearchFeatures(sQuery As String)
        If InstalledFeatureInfo.Count > 0 Then
            For Each InstalledFeature As DismFeature In InstalledFeatureInfo
                If InstalledFeature.FeatureName.ToLower().Contains(sQuery.ToLower()) Then
                    ListView1.Items.Add(New ListViewItem(New String() {InstalledFeature.FeatureName, Casters.CastDismFeatureState(InstalledFeature.State, True)}))
                End If
            Next
        End If
    End Sub

    Private Sub SearchBox1_TextChanged(sender As Object, e As EventArgs) Handles SearchBox1.TextChanged
        ListView1.Items.Clear()
        If SearchBox1.Text <> "" Then
            SearchFeatures(SearchBox1.Text)
        Else
            For Each InstalledFeature As DismFeature In InstalledFeatureInfo
                ListView1.Items.Add(New ListViewItem(New String() {InstalledFeature.FeatureName, Casters.CastDismFeatureState(InstalledFeature.State, True)}))
            Next
        End If
    End Sub
End Class
