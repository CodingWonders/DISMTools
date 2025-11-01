Imports System.IO
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

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
                        TitleMsg = "Argomenti guida DISMTools"
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
        MainForm.EnableDarkTitleBar(handle, CurrentTheme.IsDark)
        Text = TitleMsg
    End Sub

    Private Sub WebBrowser1_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        DynaLog.LogMessage("Navigating to page " & Quote & e.Url.AbsoluteUri & Quote & "...")
        If e.Url.AbsoluteUri.Equals("https://dismtools.com/tour", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("Tour imaginary site is present. Attempting to launch the tour...")
            If Directory.Exists(Path.Combine(Application.StartupPath, "docs", "tour")) Then
                DynaLog.LogMessage("Tour directory exists. Starting the tour!")

                Dim languageCode As String = "en"

                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                languageCode = "en"
                            Case "ESN"
                                languageCode = "es"
                            Case "FRA"
                                languageCode = "fr"
                            Case "PTB", "PTG"
                                languageCode = "pt"
                            Case "ITA"
                                languageCode = "it"
                        End Select
                    Case 1
                        languageCode = "en"
                    Case 2
                        languageCode = "es"
                    Case 3
                        languageCode = "fr"
                    Case 4
                        languageCode = "pt"
                    Case 5
                        languageCode = "it"
                End Select

                Process.Start(Path.Combine(Application.StartupPath, "docs", "tour", languageCode, "tour-start.html"))
            End If
            WebBrowser1.Navigate(CurrentSite)
            Exit Sub
        End If
        If File.Exists(e.Url.AbsoluteUri.Replace("file:///", "").Trim().Replace("/", "\").Trim().Replace("%20", " ").Trim() & "\index.html") Then
            DynaLog.LogMessage("HTML exists in Absolute URI path. Navigating...")
            WebBrowser1.Navigate(e.Url.AbsoluteUri & "\index.html")
        ElseIf e.Url.AbsoluteUri.StartsWith("http", StringComparison.OrdinalIgnoreCase) Or e.Url.AbsoluteUri.StartsWith("ftp", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("Absolute URI points to an external website. Opening in default browser...")
            Process.Start(e.Url.AbsoluteUri)
            WebBrowser1.Navigate(CurrentSite)
        End If
        DynaLog.LogMessage("Document title: " & WebBrowser1.DocumentTitle)
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
            If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, CurrentTheme.IsDark)
        End If
    End Sub
End Class