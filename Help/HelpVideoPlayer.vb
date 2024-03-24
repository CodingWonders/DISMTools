Public Class HelpVideoPlayer

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
                End Select
            Case 1
                Text = "DISMTools Video Player"
            Case 2
                Text = "Reproductor de vídeo de DISMTools"
            Case 3
                Text = "Lecteur vidéo DISMTools"
            Case 4
                Text = "Reprodutor de vídeo DISMTools"
        End Select
    End Sub

    Private Sub HelpVideoPlayer_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Visible Then
            Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
            If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        End If
    End Sub
End Class