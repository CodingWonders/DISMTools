Imports System.IO

Public Class HelpBrowserForm

    Private Sub HelpBrowserForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        WebBrowser1.Navigate(Application.StartupPath & "\docs\index.html")
    End Sub

    Private Sub WebBrowser1_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        If File.Exists(e.Url.AbsoluteUri.Replace("file:///", "").Trim().Replace("/", "\").Trim() & "\index.html") Then
            WebBrowser1.Navigate(e.Url.AbsoluteUri & "\index.html")
        End If
        Text = WebBrowser1.DocumentTitle & " - DISMTools Help Topics"
    End Sub
End Class