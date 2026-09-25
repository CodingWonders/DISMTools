Imports System.Drawing.Drawing2D
Imports Microsoft.Win32

Public Class SplashScreen

    Dim opacityFade As Single

    Private Sub SplashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        VersionLabel.Text = String.Format(LocalizationService.ForSection("SplashScreen")("Version.Label"),
                                          My.Application.Info.Version.ToString(),
                                          MainForm.dtBranch,
                                          RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm"))
        Try
            ResizeImage()
        Catch ex As Exception

        End Try
        If MainForm.dtBranch.Contains("pre") Then
            LogoPic.Image = My.Resources.dt_branding_preview
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

    Private Sub ResizeImage()
        If BackgroundImage Is Nothing Then Exit Sub

        Dim scale As Double = WindowHelper.ScaleLogical(100) / 96.0F

        ' we don't need to resize with 100% display scaling
        If scale = 96.0F Then Exit Sub

        Dim newWidth As Integer = BackgroundImage.Width * scale,
            newHeight As Integer = BackgroundImage.Height * scale

        Dim scaled As New Bitmap(BackgroundImage, New Size(newWidth, newHeight))

        BackgroundImage = scaled
    End Sub
End Class