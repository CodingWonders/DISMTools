Imports Microsoft.Win32
Imports System.Runtime.InteropServices

Module HelpDocsModule

    ''' <summary>
    ''' Displays a DISMTools Help Documentation page
    ''' </summary>
    ''' <param name="DocsPath"></param>
    ''' <param name="SectionName"></param>
    ''' <remarks></remarks>
    Public Sub DisplayHelpDocumentation(DocsPath As String, Optional SectionName As String = "")
        Try
            Dim filePath As String = Path.Combine(Application.StartupPath, DocsPath)
            Dim uriBuilder As New UriBuilder(New Uri(filePath))

            If SectionName <> "" Then uriBuilder.Fragment = Uri.EscapeDataString(SectionName)

            Dim DefaultBrowserCommandline As String = GetDefaultBrowserCmdline()

            ' Separate each part of the command-line to get the application path
            Dim CmdlineParts As String() = DefaultBrowserCommandline.Replace(Quote, "").Split(" ")
            Dim DefaultBrowserExePath As String = ""
            For i = 0 To CmdlineParts.Length - 1
                DefaultBrowserExePath &= " " & CmdlineParts(i)
                If File.Exists(DefaultBrowserExePath) Then Exit For
            Next

            If DefaultBrowserExePath <> "" AndAlso File.Exists(DefaultBrowserExePath) Then
                Process.Start(DefaultBrowserExePath.TrimStart(" ").TrimEnd(" "), uriBuilder.Uri.AbsoluteUri)
            Else
                ' If we couldn't get the path to the default browser, we just guess with a standard API call. In that
                ' case, URI **fragments** will not work.
                Process.Start(uriBuilder.Uri.AbsoluteUri)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function GetDefaultBrowserCmdline() As String
        ' Based on user choice for HTTP we get its ProgId and then go to HKCR. That's how I believe
        ' browsers set themselves as defaults these days.
        Dim DefaultBrowserPath As String = "",
            DefaultBrowserCmdline As String = ""

        Dim HtmlUCRk As RegistryKey = Nothing,
            ClassesRk As RegistryKey = Nothing
        Try
            HtmlUCRk = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", False)
            DefaultBrowserPath = HtmlUCRk.GetValue("ProgId", "")

            ClassesRk = Registry.ClassesRoot.OpenSubKey(String.Format("{0}\shell\open\command", DefaultBrowserPath), False)
            DefaultBrowserCmdline = ClassesRk.GetValue(Nothing, "")
        Catch ex As Exception

        Finally
            If HtmlUCRk IsNot Nothing Then HtmlUCRk.Close()
            If ClassesRk IsNot Nothing Then ClassesRk.Close()
        End Try

        Return DefaultBrowserCmdline
    End Function

End Module
