Public Class HelpVideoPlayer

    Private VideoServer As New DTHttpServer(Path.Combine(Application.StartupPath, "videos"), 2026)

    Private Sub HelpVideoPlayer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "DISMTools Video Player"
                    Case "ESN"
                        Text = "Reproductor de vídeo de DISMTools"
                    Case "FRA"
                        Text = "Lecteur vidéo DISMTools"
                    Case "PTB", "PTG"
                        Text = "Reprodutor de vídeo DISMTools"
                    Case "ITA"
                        Text = "Lettore video DISMTools"
                End Select
            Case 1
                Text = "DISMTools Video Player"
            Case 2
                Text = "Reproductor de vídeo de DISMTools"
            Case 3
                Text = "Lecteur vidéo DISMTools"
            Case 4
                Text = "Reprodutor de vídeo DISMTools"
            Case 5
                Text = "Lettore video DISMTools"
        End Select

        VideoServer.StartServer()
        If VideoServer.IsListenerAlive() Then
            WebBrowser1.Navigate("http://localhost:2026/videoplay.html")
        End If
    End Sub

    Private Sub HelpVideoPlayer_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Visible Then
            Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
            WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        End If
    End Sub

    Private Sub HelpVideoPlayer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        VideoServer.StopServer()
    End Sub
End Class