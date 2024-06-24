Imports System.Windows.Forms
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities

Public Class GetCapabilityInfoDlg

    Public InstalledCapabilityInfo As DismCapabilityCollection

    Private Sub GetCapabilityInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        SearchBox1.BackColor = BackColor
        SearchBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        SearchPic.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.search_dark, My.Resources.search_light)
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Get capability information"
                        Label1.Text = Text
                        Label2.Text = "Ready"
                        Label22.Text = "Capability identity:"
                        Label24.Text = "Capability name:"
                        Label26.Text = "Capability state:"
                        Label31.Text = "Display name:"
                        Label36.Text = "Capability information"
                        Label37.Text = "Select an installed capability on the left to view its information here"
                        Label41.Text = "Capability description:"
                        Label43.Text = "Sizes:"
                        ListView1.Columns(0).Text = "Capability identity"
                        ListView1.Columns(1).Text = "State"
                        Button2.Text = "Save..."
                        SearchBox1.cueBanner = "Type here to search for a capability..."
                    Case "ESN"
                        Text = "Obtener información de funcionalidades"
                        Label1.Text = Text
                        Label2.Text = "Listo"
                        Label22.Text = "Identidad de la funcionalidad:"
                        Label24.Text = "Nombre de la funcionalidad:"
                        Label26.Text = "Estado de la funcionalidad:"
                        Label31.Text = "Nombre para mostrar"
                        Label36.Text = "Información de la funcionalidad"
                        Label37.Text = "Seleccione una funcionalidad instalada en la izquierda para ver su información aquí"
                        Label41.Text = "Descripción de la funcionalidad"
                        Label43.Text = "Tamaños:"
                        ListView1.Columns(0).Text = "Identidad de funcionalidad"
                        ListView1.Columns(1).Text = "Estado"
                        Button2.Text = "Guardar..."
                        SearchBox1.cueBanner = "Escriba aquí para buscar una funcionalidad..."
                    Case "FRA"
                        Text = "Obtenir des informations sur les capacités"
                        Label1.Text = Text
                        Label2.Text = "Prêt"
                        Label22.Text = "Identité de la capacité :"
                        Label24.Text = "Nom de la capacité :"
                        Label26.Text = "État de la capacité :"
                        Label31.Text = "Nom d'affichage :"
                        Label36.Text = "Informations sur la capacité"
                        Label37.Text = "Sélectionnez une capacité installée sur la gauche pour afficher les informations correspondantes ici."
                        Label41.Text = "Description de la capacité :"
                        Label43.Text = "Tailles :"
                        ListView1.Columns(0).Text = "Identité de la capacité"
                        ListView1.Columns(1).Text = "État"
                        Button2.Text = "Sauvegarder..."
                        SearchBox1.cueBanner = "Tapez ici pour rechercher une capacité..."
                    Case "PTB", "PTG"
                        Text = "Obter informações sobre as capacidades"
                        Label1.Text = Text
                        Label2.Text = "Pronto"
                        Label22.Text = "Identidade da capacidade:"
                        Label24.Text = "Nome da capacidade:"
                        Label26.Text = "Estado da capacidade:"
                        Label31.Text = "Nome de apresentação:"
                        Label36.Text = "Informação sobre a capacidade"
                        Label37.Text = "Seleccione uma capacidade instalada à esquerda para ver a sua informação aqui"
                        Label41.Text = "Descrição da capacidade:"
                        Label43.Text = "Tamanhos:"
                        ListView1.Columns(0).Text = "Identidade da capacidade"
                        ListView1.Columns(1).Text = "Estado"
                        Button2.Text = "Guardar..."
                        SearchBox1.cueBanner = "Digite aqui para pesquisar uma capacidade..."
                End Select
            Case 1
                Text = "Get capability information"
                Label1.Text = Text
                Label2.Text = "Ready"
                Label22.Text = "Capability identity:"
                Label24.Text = "Capability name:"
                Label26.Text = "Capability state:"
                Label31.Text = "Display name:"
                Label36.Text = "Capability information"
                Label37.Text = "Select an installed capability on the left to view its information here"
                Label41.Text = "Capability description:"
                Label43.Text = "Sizes:"
                ListView1.Columns(0).Text = "Capability identity"
                ListView1.Columns(1).Text = "State"
                Button2.Text = "Save..."
                SearchBox1.cueBanner = "Type here to search for a capability..."
            Case 2
                Text = "Obtener información de funcionalidades"
                Label1.Text = Text
                Label2.Text = "Listo"
                Label22.Text = "Identidad de la funcionalidad:"
                Label24.Text = "Nombre de la funcionalidad:"
                Label26.Text = "Estado de la funcionalidad:"
                Label31.Text = "Nombre para mostrar"
                Label36.Text = "Información de la funcionalidad"
                Label37.Text = "Seleccione una funcionalidad instalada en la izquierda para ver su información aquí"
                Label41.Text = "Descripción de la funcionalidad"
                Label43.Text = "Tamaños:"
                ListView1.Columns(0).Text = "Identidad de funcionalidad"
                ListView1.Columns(1).Text = "Estado"
                Button2.Text = "Guardar..."
                SearchBox1.cueBanner = "Escriba aquí para buscar una funcionalidad..."
            Case 3
                Text = "Obtenir des informations sur les capacités"
                Label1.Text = Text
                Label2.Text = "Prêt"
                Label22.Text = "Identité de la capacité :"
                Label24.Text = "Nom de la capacité :"
                Label26.Text = "État de la capacité :"
                Label31.Text = "Nom d'affichage :"
                Label36.Text = "Informations sur la capacité"
                Label37.Text = "Sélectionnez une capacité installée sur la gauche pour afficher les informations correspondantes ici."
                Label41.Text = "Description de la capacité :"
                Label43.Text = "Tailles :"
                ListView1.Columns(0).Text = "Identité de la capacité"
                ListView1.Columns(1).Text = "État"
                Button2.Text = "Sauvegarder..."
                SearchBox1.cueBanner = "Tapez ici pour rechercher une capacité..."
            Case 4
                Text = "Obter informações sobre as capacidades"
                Label1.Text = Text
                Label2.Text = "Pronto"
                Label22.Text = "Identidade da capacidade:"
                Label24.Text = "Nome da capacidade:"
                Label26.Text = "Estado da capacidade:"
                Label31.Text = "Nome de apresentação:"
                Label36.Text = "Informação sobre a capacidade"
                Label37.Text = "Seleccione uma capacidade instalada à esquerda para ver a sua informação aqui"
                Label41.Text = "Descrição da capacidade:"
                Label43.Text = "Tamanhos:"
                ListView1.Columns(0).Text = "Identidade da capacidade"
                ListView1.Columns(1).Text = "Estado"
                Button2.Text = "Guardar..."
                SearchBox1.cueBanner = "Digite aqui para pesquisar uma capacidade..."
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
        For Each InstalledCapability As DismCapability In InstalledCapabilityInfo
            ListView1.Items.Add(New ListViewItem(New String() {InstalledCapability.Name, Casters.CastDismPackageState(InstalledCapability.State, True)}))
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
                            End Select
                        Case 1
                            msg = "Background processes need to have completed before showing feature information. We'll wait until they have completed"
                        Case 2
                            msg = "Los procesos en segundo plano deben haber completado antes de obtener información de la característica. Esperaremos hasta que hayan completado"
                        Case 3
                            msg = "Les processus en plan doivent être terminés avant d'afficher les caractéristiques. Nous attendrons qu'ils soient terminés"
                        Case 4
                            msg = "Os processos em segundo plano têm de estar concluídos antes de mostrar informações sobre as características. Vamos esperar até que estejam concluídos"
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
                            End Select
                        Case 1
                            Label2.Text = "Waiting for background processes to finish..."
                        Case 2
                            Label2.Text = "Esperando a que terminen los procesos en segundo plano..."
                        Case 3
                            Label2.Text = "Attente de la fin des processus en arrière plan..."
                        Case 4
                            Label2.Text = "À espera que os processos em segundo plano terminem..."
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
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label2.Text = "Preparing to get capability information..."
                            Case "ESN"
                                Label2.Text = "Preparándonos para obtener información de la funcionalidad..."
                            Case "FRA"
                                Label2.Text = "Préparation de l'obtention des informations de la capacité en cours..."
                            Case "PTB", "PTG"
                                Label2.Text = "Preparar-se para obter informações sobre a capacidade..."
                        End Select
                    Case 1
                        Label2.Text = "Preparing to get capability information..."
                    Case 2
                        Label2.Text = "Preparándonos para obtener información de la funcionalidad..."
                    Case 3
                        Label2.Text = "Préparation de l'obtention des informations de la capacité en cours..."
                    Case 4
                        Label2.Text = "Preparar-se para obter informações sobre a capacidade..."
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
                                End Select
                            Case 1
                                Label2.Text = "Getting information from " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                            Case 2
                                Label2.Text = "Obteniendo información de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                            Case 3
                                Label2.Text = "Obtention des informations de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & " en cours..."
                            Case 4
                                Label2.Text = "Obter informações de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                        End Select
                        Application.DoEvents()
                        Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, ListView1.FocusedItem.SubItems(0).Text)
                        Label23.Text = capInfo.Name
                        Label25.Text = capInfo.Name.Remove(InStr(capInfo.Name, "~") - 1)
                        Label35.Text = Casters.CastDismPackageState(capInfo.State, True)
                        Label32.Text = capInfo.DisplayName
                        Label40.Text = capInfo.Description
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Label42.Text = "Download size: " & capInfo.DownloadSize & " bytes" & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")", "") & CrLf & _
                                            "Install size: " & capInfo.InstallSize & " bytes" & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")", "")
                                    Case "ESN"
                                        Label42.Text = "Tamaño de descarga: " & capInfo.DownloadSize & " bytes" & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")", "") & CrLf & _
                                            "Tamaño de instalación: " & capInfo.InstallSize & " bytes" & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")", "")
                                    Case "FRA"
                                        Label42.Text = "Taille du téléchargement : " & capInfo.DownloadSize & " octets" & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize, True) & ")", "") & CrLf & _
                                            "Taille d'installation : " & capInfo.InstallSize & " octets" & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize, True) & ")", "")
                                    Case "PTB", "PTG"
                                        Label42.Text = "Tamanho do descarregamento: " & capInfo.DownloadSize & " bytes" & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")", "") & CrLf & _
                                            "Tamanho da instalação: " & capInfo.InstallSize & " bytes" & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")", "")
                                End Select
                            Case 1
                                Label42.Text = "Download size: " & capInfo.DownloadSize & " bytes" & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")", "") & CrLf & _
                                    "Install size: " & capInfo.InstallSize & " bytes" & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")", "")
                            Case 2
                                Label42.Text = "Tamaño de descarga: " & capInfo.DownloadSize & " bytes" & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")", "") & CrLf & _
                                    "Tamaño de instalación: " & capInfo.InstallSize & " bytes" & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")", "")
                            Case 3
                                Label42.Text = "Taille du téléchargement : " & capInfo.DownloadSize & " octets" & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize, True) & ")", "") & CrLf & _
                                    "Taille d'installation : " & capInfo.InstallSize & " octets" & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize, True) & ")", "")
                            Case 4
                                Label42.Text = "Tamanho do descarregamento: " & capInfo.DownloadSize & " bytes" & If(capInfo.DownloadSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")", "") & CrLf & _
                                    "Tamanho da instalação: " & capInfo.InstallSize & " bytes" & If(capInfo.InstallSize >= 1024, " (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")", "")
                        End Select
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
                                    msg = "Could not get capability information. Reason: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                                Case "ESN"
                                    msg = "No pudimos obtener información de la funcionalidad. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                                Case "FRA"
                                    msg = "Impossible d'obtenir des informations sur les capacités. Raison : " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                                Case "PTB", "PTG"
                                    msg = "Não foi possível obter informações sobre a capacidade. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                            End Select
                        Case 1
                            msg = "Could not get capability information. Reason: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 2
                            msg = "No pudimos obtener información de la funcionalidad. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 3
                            msg = "Impossible d'obtenir des informations sur les capacités. Raison : " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 4
                            msg = "Não foi possível obter informações sobre a capacidade. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
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
                        End Select
                    Case 1
                        Label2.Text = "Ready"
                    Case 2
                        Label2.Text = "Listo"
                    Case 3
                        Label2.Text = "Prêt"
                    Case 4
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

    Private Sub GetCapabilityInfoDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
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
            ImgInfoSaveDlg.SaveTask = 6
            ImgInfoSaveDlg.ShowDialog()
            InfoSaveResults.Show()
        End If
    End Sub

    Sub SearchCapabilities(sQuery As String)
        If InstalledCapabilityInfo.Count > 0 Then
            For Each InstalledCapability As DismCapability In InstalledCapabilityInfo
                If InstalledCapability.Name.ToLower().Contains(sQuery.ToLower()) Then
                    ListView1.Items.Add(New ListViewItem(New String() {InstalledCapability.Name, Casters.CastDismFeatureState(InstalledCapability.State, True)}))
                End If
            Next
        End If
    End Sub

    Private Sub SearchBox1_TextChanged(sender As Object, e As EventArgs) Handles SearchBox1.TextChanged
        ListView1.Items.Clear()
        If SearchBox1.Text <> "" Then
            SearchCapabilities(SearchBox1.Text)
        Else
            For Each InstalledCapability As DismCapability In InstalledCapabilityInfo
                ListView1.Items.Add(New ListViewItem(New String() {InstalledCapability.Name, Casters.CastDismFeatureState(InstalledCapability.State, True)}))
            Next
        End If
    End Sub
End Class
