Imports System.Windows.Forms
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Public Class GetFeatureInfoDlg

    Public InstalledFeatureInfo As DismFeatureCollection

    Function CastDismFeatureState(state As DismPackageFeatureState) As String
        Select Case state
            Case DismPackageFeatureState.NotPresent
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "Not present"
                            Case "ESN"
                                Return "No presente"
                        End Select
                    Case 1
                        Return "Not present"
                    Case 2
                        Return "No presente"
                End Select
            Case DismPackageFeatureState.UninstallPending
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "Disable Pending"
                            Case "ESN"
                                Return "Deshabilitación pendiente"
                        End Select
                    Case 1
                        Return "Disable Pending"
                    Case 2
                        Return "Deshabilitación pendiente"
                End Select
            Case DismPackageFeatureState.Staged
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "Disabled"
                            Case "ESN"
                                Return "Deshabilitado"
                        End Select
                    Case 1
                        Return "Disabled"
                    Case 2
                        Return "Deshabilitado"
                End Select
            Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "Removed"
                            Case "ESN"
                                Return "Eliminado"
                        End Select
                    Case 1
                        Return "Removed"
                    Case 2
                        Return "Eliminado"
                End Select
            Case DismPackageFeatureState.Installed
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "Enabled"
                            Case "ESN"
                                Return "Habilitado"
                        End Select
                    Case 1
                        Return "Enabled"
                    Case 2
                        Return "Habilitado"
                End Select
            Case DismPackageFeatureState.InstallPending
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "Enable Pending"
                            Case "ESN"
                                Return "Habilitación pendiente"
                        End Select
                    Case 1
                        Return "Enable Pending"
                    Case 2
                        Return "Habilitación pendiente"
                End Select
            Case DismPackageFeatureState.Superseded
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "Superseded"
                            Case "ESN"
                                Return "Sustituido"
                        End Select
                    Case 1
                        Return "Superseded"
                    Case 2
                        Return "Sustituido"
                End Select
            Case DismPackageFeatureState.PartiallyInstalled
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "Partially Installed"
                            Case "ESN"
                                Return "Instalado parcialmente"
                        End Select
                    Case 1
                        Return "Partially Installed"
                    Case 2
                        Return "Instalado parcialmente"
                End Select
        End Select
        Return Nothing
    End Function

    Function CastDismRestartType(rType As DismRestartType) As String
        Select Case rType
            Case DismRestartType.No
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "A restart is not required"
                            Case "ESN"
                                Return "No se requiere un reinicio"
                        End Select
                    Case 1
                        Return "A restart is not required"
                    Case 2
                        Return "No se requiere un reinicio"
                End Select
            Case DismRestartType.Possible
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "A restart may be required"
                            Case "ESN"
                                Return "Puede requerirse un reinicio"
                        End Select
                    Case 1
                        Return "A restart may be required"
                    Case 2
                        Return "Puede requerirse un reinicio"
                End Select
            Case DismRestartType.Required
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Return "A restart is required"
                            Case "ESN"
                                Return "Se requiere un reinicio"
                        End Select
                    Case 1
                        Return "A restart is required"
                    Case 2
                        Return "Se requiere un reinicio"
                End Select
        End Select
        Return Nothing
    End Function

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
                    Case "ENG"
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
            ListView1.Items.Add(New ListViewItem(New String() {InstalledFeature.FeatureName, CastDismFeatureState(InstalledFeature.State)}))
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
                                Case "ENG"
                                    msg = "Background processes need to have completed before showing feature information. We'll wait until they have completed"
                                Case "ESN"
                                    msg = "Los procesos en segundo plano deben haber completado antes de obtener información de la característica. Esperaremos hasta que hayan completado"
                            End Select
                        Case 1
                            msg = "Background processes need to have completed before showing feature information. We'll wait until they have completed"
                        Case 2
                            msg = "Los procesos en segundo plano deben haber completado antes de obtener información de la característica. Esperaremos hasta que hayan completado"
                    End Select
                    MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label2.Text = "Waiting for background processes to finish..."
                                Case "ESN"
                                    Label2.Text = "Esperando a que terminen los procesos en segundo plano..."
                            End Select
                        Case 1
                            Label2.Text = "Waiting for background processes to finish..."
                        Case 2
                            Label2.Text = "Esperando a que terminen los procesos en segundo plano..."
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
                            Case "ENG"
                                Label2.Text = "Preparing to get feature information..."
                            Case "ESN"
                                Label2.Text = "Preparándonos para obtener información de la característica..."
                        End Select
                    Case 1
                        Label2.Text = "Preparing to get feature information..."
                    Case 2
                        Label2.Text = "Preparándonos para obtener información de la característica..."
                End Select
                Application.DoEvents()
                Try
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Label2.Text = "Getting information from " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                                    Case "ESN"
                                        Label2.Text = "Obteniendo información de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                                End Select
                            Case 1
                                Label2.Text = "Getting information from " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                            Case 2
                                Label2.Text = "Obteniendo información de " & Quote & ListView1.FocusedItem.SubItems(0).Text & Quote & "..."
                        End Select
                        Application.DoEvents()
                        Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, ListView1.FocusedItem.SubItems(0).Text)
                        Label23.Text = featInfo.FeatureName
                        Label25.Text = featInfo.DisplayName
                        Label35.Text = featInfo.Description
                        Label32.Text = CastDismRestartType(featInfo.RestartRequired)
                        Label40.Text = CastDismFeatureState(featInfo.FeatureState)
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
                                        Case "ENG"
                                            Label42.Text = "None"
                                        Case "ESN"
                                            Label42.Text = "Ninguna"
                                    End Select
                                Case 1
                                    Label42.Text = "None"
                                Case 2
                                    Label42.Text = "Ninguna"
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
                                Case "ENG"
                                    msg = "Could not get feature information. Reason: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                                Case "ESN"
                                    msg = "No pudimos obtener información de la característica. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                            End Select
                        Case 1
                            msg = "Could not get feature information. Reason: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 2
                            msg = "No pudimos obtener información de la característica. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                    End Select
                    MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                Finally
                    DismApi.Shutdown()
                End Try
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Label2.Text = "Ready"
                            Case "ESN"
                                Label2.Text = "Listo"
                        End Select
                    Case 1
                        Label2.Text = "Ready"
                    Case 2
                        Label2.Text = "Listo"
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
End Class
