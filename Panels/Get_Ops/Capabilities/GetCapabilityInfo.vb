﻿Imports System.Windows.Forms
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
        ListView1.ForeColor = ForeColor
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
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
                                Label2.Text = "Preparing to get capability information..."
                            Case "ESN"
                                Label2.Text = "Preparándonos para obtener información de la funcionalidad..."
                        End Select
                    Case 1
                        Label2.Text = "Preparing to get capability information..."
                    Case 2
                        Label2.Text = "Preparándonos para obtener información de la funcionalidad..."
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
                        Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, ListView1.FocusedItem.SubItems(0).Text)
                        Label23.Text = capInfo.Name
                        Label25.Text = capInfo.Name.Remove(InStr(capInfo.Name, "~") - 1)
                        Label35.Text = Casters.CastDismPackageState(capInfo.State, True)
                        Label32.Text = capInfo.DisplayName
                        Label40.Text = capInfo.Description
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Label42.Text = "Download size: " & capInfo.DownloadSize & " bytes (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")" & CrLf & _
                                            "Install size: " & capInfo.InstallSize & " bytes (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")"
                                    Case "ESN"
                                        Label42.Text = "Tamaño de descarga: " & capInfo.DownloadSize & " bytes (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")" & CrLf & _
                                            "Tamaño de instalación: " & capInfo.InstallSize & " bytes (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")"
                                End Select
                            Case 1
                                Label42.Text = "Download size: " & capInfo.DownloadSize & " bytes (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")" & CrLf & _
                                    "Install size: " & capInfo.InstallSize & " bytes (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")"
                            Case 2
                                Label42.Text = "Tamaño de descarga: " & capInfo.DownloadSize & " bytes (~" & Converters.BytesToReadableSize(capInfo.DownloadSize) & ")" & CrLf & _
                                    "Tamaño de instalación: " & capInfo.InstallSize & " bytes (~" & Converters.BytesToReadableSize(capInfo.InstallSize) & ")"
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
                                Case "ENG"
                                    msg = "Could not get capability information. Reason: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                                Case "ESN"
                                    msg = "No pudimos obtener información de la funcionalidad. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                            End Select
                        Case 1
                            msg = "Could not get capability information. Reason: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case 2
                            msg = "No pudimos obtener información de la funcionalidad. Motivo: " & CrLf & CrLf & ex.ToString() & ": " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
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

    Private Sub GetCapabilityInfoDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub
End Class