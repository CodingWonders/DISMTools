Imports System.IO

Public Class HelpBrowserForm

    Dim TitleMsg As String = ""
    Dim CurrentSite As String = ""
    Dim DocTitle As String = ""

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
                    Case "PTB", "PTG"
                        TitleMsg = "Tópicos de ajuda do DISMTools"
                    Case "ITA"
                        TitleMsg = "Argomenti della guida di DISMTools"
                End Select
            Case 1
                TitleMsg = "DISMTools Help Topics"
            Case 2
                TitleMsg = "Contenidos de ayuda de DISMTools"
            Case 3
                TitleMsg = "Aide de DISMTools"
            Case 4
                TitleMsg = "Tópicos de ajuda do DISMTools"
            Case 5
                TitleMsg = "Argomenti della guida di DISMTools"
        End Select
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Text = TitleMsg
    End Sub

    Private Sub WebBrowser1_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        If File.Exists(e.Url.AbsoluteUri.Replace("file:///", "").Trim().Replace("/", "\").Trim().Replace("%20", " ").Trim() & "\index.html") Then
            WebBrowser1.Navigate(e.Url.AbsoluteUri & "\index.html")
        ElseIf e.Url.AbsoluteUri.StartsWith("http", StringComparison.OrdinalIgnoreCase) Or e.Url.AbsoluteUri.StartsWith("ftp", StringComparison.OrdinalIgnoreCase) Then
            Process.Start(e.Url.AbsoluteUri)
            WebBrowser1.Navigate(CurrentSite)
        End If
        If WebBrowser1.DocumentTitle = "" Then
            Text = DocTitle & " - " & TitleMsg
        Else
            Text = WebBrowser1.DocumentTitle & " - " & TitleMsg
            If e.Url.AbsoluteUri.StartsWith("file:///") Then DocTitle = WebBrowser1.DocumentTitle
        End If
        CurrentSite = e.Url.AbsoluteUri
    End Sub

    Private Sub HelpBrowserForm_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Visible Then
            Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
            If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        End If
    End Sub
End Class