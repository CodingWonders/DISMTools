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
                            End Select
                        Case 1
                            msg = "Background processes need to have completed before showing feature information. We'll wait until they have completed"
                        Case 2
                            msg = "Los procesos en segundo plano deben haber completado antes de obtener información de la característica. Esperaremos hasta que hayan completado"
                        Case 3
                            msg = "Les processus en plan doivent être terminés avant d'afficher les caractéristiques. Nous attendrons qu'ils soient terminés"
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
                            End Select
                        Case 1
                            Label2.Text = "Waiting for background processes to finish..."
                        Case 2
                            Label2.Text = "Esperando a que terminen los procesos en segundo plano..."
                        Case 3
                            Label2.Text = "Attente de la fin des processus en arrière plan..."
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
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label2.Text = "Preparing to get feature information..."
                            Case "ESN"
                                Label2.Text = "Preparándonos para obtener información de la característica..."
                            Case "FRA"
                                Label2.Text = "Préparation de l'obtention des informations de la caractéristique en cours..."
                        End Select
                    Case 1
                        Label2.Text = "Preparing to get feature information..."
                    Case 2
                        Label2.Text = "Preparándonos para obtener información de la característica..."
                    Case 3
                        Label2.Text = "Préparation de l'obtention des informations de la caractéristique en cours..."
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
                                End Select
                            Case 1
                                Label2.Text = "Getting information from " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                            Case 2
                                Label2.Text = "Obteniendo información de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                            Case 3
                                Label2.Text = "Obtention des informations de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & " en cours..."
                        End Select
                        Application.DoEvents()
                        Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, ListView1.FocusedItem.SubItems(0).Text)
                        Label23.Text = featInfo.FeatureName
                        Label25.Text = featInfo.DisplayName
                        Label35.Text = featInfo.Description
                        Label32.Text = Casters.CastDismRestartType(featInfo.RestartRequired, True)
                        Label40.Text = Casters.CastDismFeatureState(featInfo.FeatureState, True)
                        Label42.Text = ""
                        Dim cProps As DismCustomPropertyCollection = featInfo.CustomProperties
                        If cProps.Count > 0 Then
                            For Each cProp As DismCustomProperty In cProps
                                Label42.Text &= "- " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
                            Next
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
                                    End Select
                                Case 1
                                    Label42.Text = "None"
                                Case 2
                                    Label42.Text = "Ninguna"
                                Case 3
                                    Label42.Text = "Aucune"
                            End Select
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
                            End Select
                        Case 1
                            msg = "Could not get feature information. Reason: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 2
                            msg = "No pudimos obtener información de la característica. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 3
                            msg = "Impossible d'obtenir des informations sur les caractéristiques. Raison : " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
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
                        End Select
                    Case 1
                        Label2.Text = "Ready"
                    Case 2
                        Label2.Text = "Listo"
                    Case 3
                        Label2.Text = "Prêt"
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

    Private Sub GetFeatureInfoDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MainForm.ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = MainForm.SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.OfflineMode = MainForm.OfflineManagement
            ImgInfoSaveDlg.SaveTask = 4
            ImgInfoSaveDlg.ShowDialog()
        End If
    End Sub
End Class
