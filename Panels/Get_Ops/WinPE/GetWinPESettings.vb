﻿Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32

Public Class GetWinPESettings

    Sub GetPESettings()
        ' Mount the SOFTWARE and SYSTEM keys
        Using reg As New Process
            reg.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
            reg.StartInfo.Arguments = "load HKLM\PE_SOFT " & Quote & MainForm.MountDir & "\Windows\system32\config\SOFTWARE" & Quote
            reg.StartInfo.CreateNoWindow = True
            reg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            reg.Start()
            reg.WaitForExit()
            If reg.ExitCode <> 0 Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Label5.Text = "Could not get value"
                            Case "ESN"
                                Label5.Text = "No se pudo obtener el valor"
                        End Select
                    Case 1
                        Label5.Text = "Could not get value"
                    Case 2
                        Label5.Text = "No se pudo obtener el valor"
                End Select
                Button1.Visible = False
            End If
            reg.StartInfo.Arguments = "load HKLM\PE_SYS " & Quote & MainForm.MountDir & "\Windows\system32\config\SYSTEM" & Quote
            reg.Start()
            reg.WaitForExit()
            If reg.ExitCode <> 0 Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Label6.Text = "Could not get value"
                            Case "ESN"
                                Label6.Text = "No se pudo obtener el valor"
                        End Select
                    Case 1
                        Label6.Text = "Could not get value"
                    Case 2
                        Label6.Text = "No se pudo obtener el valor"
                End Select
                Button2.Visible = False
            End If
            Try
                Dim msg As String = ""
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                msg = "Could not get value"
                            Case "ESN"
                                msg = "No se pudo obtener el valor"
                        End Select
                    Case 1
                        msg = "Could not get value"
                    Case 2
                        msg = "No se pudo obtener el valor"
                End Select
                ' Get target path first
                Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("PE_SOFT\Microsoft\Windows NT\CurrentVersion\WinPE", False)
                Label5.Text = regKey.GetValue("InstRoot", msg).ToString()
                regKey.Close()
                regKey = Registry.LocalMachine.OpenSubKey("PE_SYS\ControlSet001\Services\FBWF", False)
                Label6.Text = regKey.GetValue("WinPECacheThreshold", msg).ToString() & " MB"
                regKey.Close()
            Catch ex As Exception

            End Try
            ' Unload registry hives
            reg.StartInfo.Arguments = "unload HKLM\PE_SOFT"
            reg.Start()
            reg.WaitForExit()
            reg.StartInfo.Arguments = "unload HKLM\PE_SYS"
            reg.Start()
            reg.WaitForExit()
        End Using
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub GetWinPESettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Get Windows PE settings"
                        Label1.Text = Text
                        Label2.Text = "These are the Windows PE settings for this image:"
                        Label3.Text = "Target path:"
                        Label4.Text = "Scratch space:"
                        Button1.Text = "Change..."
                        Button2.Text = "Change..."
                        OK_Button.Text = "OK"
                    Case "ESN"
                        Text = "Obtener configuraciones de Windows PE"
                        Label1.Text = Text
                        Label2.Text = "Estas son las configuraciones de Windows PE para esta imagen:"
                        Label3.Text = "Carpeta de destino:"
                        Label4.Text = "Espacio temporal:"
                        Button1.Text = "Cambiar..."
                        Button2.Text = "Cambiar..."
                        OK_Button.Text = "Aceptar"
                End Select
            Case 1
                Text = "Get Windows PE settings"
                Label1.Text = Text
                Label2.Text = "These are the Windows PE settings for this image:"
                Label3.Text = "Target path:"
                Label4.Text = "Scratch space:"
                Button1.Text = "Change..."
                Button2.Text = "Change..."
                OK_Button.Text = "OK"
            Case 2
                Text = "Obtener configuraciones de Windows PE"
                Label1.Text = Text
                Label2.Text = "Estas son las configuraciones de Windows PE para esta imagen:"
                Label3.Text = "Carpeta de destino:"
                Label4.Text = "Espacio temporal:"
                Button1.Text = "Cambiar..."
                Button2.Text = "Cambiar..."
                OK_Button.Text = "Aceptar"
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If

        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))

        Button1.Visible = True
        Button2.Visible = True

        ' Get Windows PE settings
        GetPESettings()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Visible = False
        If SetPETargetPath.ShowDialog(MainForm) = Windows.Forms.DialogResult.OK Then GetPESettings()
        Visible = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Visible = False
        If SetPEScratchSpace.ShowDialog(MainForm) = Windows.Forms.DialogResult.OK Then GetPESettings()
        Visible = True
    End Sub
End Class