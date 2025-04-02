Imports System.Drawing.Drawing2D
Imports Microsoft.Win32

Public Class SplashScreen

    Dim opacityFade As Single

    Private Sub SplashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        VersionLabel.Text = String.Format("Version {0}.{1}_{2}.{3}",
                                          My.Application.Info.Version.ToString(),
                                          MainForm.dtBranch,
                                          MainForm.dt_codeName.ToLower(),
                                          RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm"))
        If MainForm.dtBranch.Contains("preview") Then
            PreviewFlag.Visible = True
            VersionLabel.Visible = True
        End If
        Try
            Dim wmReg As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\Desktop\WindowMetrics")
            If wmReg.GetValue("MinAnimate") = 1 Then
                Timer1.Enabled = True
            Else
                Opacity = 1
            End If
            wmReg.Close()
        Catch ex As Exception
            Opacity = 1
        End Try
        Refresh()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For Me.opacityFade = 0 To 1 Step 0.05
            Opacity = opacityFade
            Refresh()
        Next opacityFade
        Opacity = 1
        Refresh()
        Timer1.Enabled = False
        Timer1.Stop()
    End Sub

    Private Sub SplashScreen_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If WindowState = FormWindowState.Maximized Then
            WindowState = FormWindowState.Normal
        End If
    End Sub
End Class