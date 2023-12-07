Imports System.IO

Public Class HelpBrowserForm

    Dim TitleMsg As String = ""

    Private Sub HelpBrowserForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        TitleMsg = "DISMTools Help Topics"
                    Case "ESN"
                        TitleMsg = "Contenidos de ayuda de DISMTools"
                    Case "FRA"
                        TitleMsg = "Aide de DISMTools"
                End Select
            Case 1
                TitleMsg = "DISMTools Help Topics"
            Case 2
                TitleMsg = "Contenidos de ayuda de DISMTools"
            Case 3
                TitleMsg = "Aide de DISMTools"
        End Select
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Text = TitleMsg
    End Sub

    Private Sub WebBrowser1_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        If File.Exists(e.Url.AbsoluteUri.Replace("file:///", "").Trim().Replace("/", "\").Trim().Replace("%20", " ").Trim() & "\index.html") Then
            WebBrowser1.Navigate(e.Url.AbsoluteUri & "\index.html")
        End If
        Text = WebBrowser1.DocumentTitle & " - " & TitleMsg
    End Sub
End Class