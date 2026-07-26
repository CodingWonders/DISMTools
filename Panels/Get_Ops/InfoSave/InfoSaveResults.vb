Imports System.IO
Imports System.Drawing.Printing
Imports Markdig
Imports Microsoft.VisualBasic.ControlChars
Imports System.Threading.Tasks

Public Class InfoSaveResults

    Dim document As PrintDocument = New PrintDocument()
    Dim stringToPrint As String = ""

    Public FilePath As String = ""

    Private LogFont As String
    Private LogFontSize As Single


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Private Async Sub InfoSaveResults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("InfoSaveResults")("Image.Report.Label")
        Label1.Text = LocalizationService.ForSection("InfoSaveResults")("ReportSaved.Message")
        Button1.Text = LocalizationService.ForSection("InfoSaveResults.Actions")("Ok.Button")
        Button2.Text = LocalizationService.ForSection("InfoSaveResults")("SaveReport.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        LogFont = MainForm.LogFont
        LogFontSize = MainForm.LogFontSize
        Await Task.Run(Sub()
                           TextBox1.Clear()
                           DynaLog.LogMessage("Checking if the report exists...")
                           If File.Exists(FilePath) Then
                               DynaLog.LogMessage("The report exists. Reading and parsing to HTML...")
                               TextBox1.Text = File.ReadAllText(FilePath)
                               TextBox1.Font = New Font(LogFont, LogFontSize, FontStyle.Regular)

                               ' Convert Markdown report to HTML and add style tags to make the HTML report prettier.
                               Dim prettyHTML As String = "<html>" & CrLf &
                                                          "    <head>" & CrLf &
                                                          "        <meta charset=" & Quote & "utf-8" & Quote & ">" & CrLf &
                                                          "        <title>DISMTools Image Information Report</title>" & CrLf &
                                                          "        <style>" & CrLf &
                                                          "            body {" & CrLf &
                                                          "                font-family: " & Quote & "Segoe UI" & Quote & ", Arial, Verdana, sans-serif;" & CrLf &
                                                          "                display: flex;" & CrLf &
                                                          "                margin: 0;" & CrLf &
                                                          "                height: 100vh;" & CrLf &
                                                          "                overflow: hidden;" & CrLf &
                                                          "                background-color: #FCFBFF;" & CrLf &
                                                          "                color: black;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            table {" & CrLf &
                                                          "                border-collapse: collapse;" & CrLf &
                                                          "                margin-bottom: 20px;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            table th {" & CrLf &
                                                          "                padding: 8px;" & CrLf &
                                                          "                border-bottom: 1px solid #222" & CrLf &
                                                          "            }" & CrLf &
                                                          "            table td {" & CrLf &
                                                          "                padding: 8px;" & CrLf &
                                                          "                border-bottom: 1px solid #222" & CrLf &
                                                          "            }" & CrLf &
                                                          "            code {" & CrLf &
                                                          "                font-family: Inconsolata, " & Quote & "Cascadia Code" & Quote & ", Consolas, " & Quote & "Courier New" & Quote & CrLf &
                                                          "            }" & CrLf &
                                                          "            #sidebar {" & CrLf &
                                                          "                width: 200px;" & CrLf &
                                                          "                border-right: 1px solid #222;" & CrLf &
                                                          "                position: fixed;" & CrLf &
                                                          "                height: 100%;" & CrLf &
                                                          "                overflow-y: auto;" & CrLf &
                                                          "                overflow-x: hidden;" & CrLf &
                                                          "                background-color: white;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #sidebar.shrink {" & CrLf &
                                                          "                width: 50px;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #sidebar.shrink a {" & CrLf &
                                                          "                display: none;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #content {" & CrLf &
                                                          "                margin-left: 200px;" & CrLf &
                                                          "                padding-left: 24px;" & CrLf &
                                                          "                padding-right: 24px;" & CrLf &
                                                          "                overflow: auto;" & CrLf &
                                                          "                width: calc(100% - 50px);" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #content.shrink {" & CrLf &
                                                          "                margin-left: 50px;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #sidebar a {" & CrLf &
                                                          "                text-decoration: none;" & CrLf &
                                                          "                color: black;" & CrLf &
                                                          "                display: block;" & CrLf &
                                                          "                padding: 5px 16px;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #sidebar a:hover {" & CrLf &
                                                          "                background-color: #f0f0f0;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #menu-toggle {" & CrLf &
                                                          "                cursor: pointer;" & CrLf &
                                                          "                padding: 5px 16px;" & CrLf &
                                                          "                background-color: #222;" & CrLf &
                                                          "                color: white;" & CrLf &
                                                          "                margin-bottom: 10px;" & CrLf &
                                                          "                text-align: center;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #menu-toggle:hover {" & CrLf &
                                                          "                background-color: #333;" & CrLf &
                                                          "            }" & CrLf & CrLf &
                                                          "            @media (prefers-color-sheme: dark) {" & CrLf &
                                                          "                body {" & CrLf &
                                                          "                    background-color: #1F1F1F;" & CrLf &
                                                          "                    color: white;" & CrLf &
                                                          "                }" & CrLf & CrLf &
                                                          "                table th {" & CrLf &
                                                          "                    border-bottom: 1px solid #EEE" & CrLf &
                                                          "                }" & CrLf & CrLf &
                                                          "                table td {" & CrLf &
                                                          "                    border-bottom: 1px solid #EEE" & CrLf &
                                                          "                }" & CrLf & CrLf &
                                                          "                #sidebar {" & CrLf &
                                                          "                    border-right: 1px solid #EEE" & CrLf &
                                                          "                }" & CrLf & CrLf &
                                                          "                #sidebar a {" & CrLf &
                                                          "                    color: white;" & CrLf &
                                                          "                }" & CrLf & CrLf &
                                                          "                #sidebar a:hover {" & CrLf &
                                                          "                    background-color: #282828;" & CrLf &
                                                          "                }" & CrLf &
                                                          "            }" & CrLf &
                                                          "        </style>" & CrLf &
                                                          "    </head>" & CrLf &
                                                          "    <body>" & CrLf &
                                                          "        <div id=" & Quote & "sidebar" & Quote & ">" & CrLf &
                                                          "            <div id=" & Quote & "menu-toggle" & Quote & ">☰</div>" & CrLf &
                                                          "        </div>" & CrLf &
                                                          "        <div id=" & Quote & "content" & Quote & ">" & CrLf &
                                                          "            <!-- Content Goes Here!!! -->" & CrLf &
                                                          "        </div>" & CrLf &
                                                          "        <script>" & CrLf &
                                                          "            document.addEventListener(" & Quote & "DOMContentLoaded" & Quote & ", function() {" & CrLf &
                                                          "                var sidebar = document.getElementById(" & Quote & "sidebar" & Quote & ");" & CrLf &
                                                          "                var content = document.getElementById(" & Quote & "content" & Quote & ");" & CrLf &
                                                          "                var menuToggle = document.getElementById(" & Quote & "menu-toggle" & Quote & ");" & CrLf & CrLf &
                                                          "                menuToggle.addEventListener(" & Quote & "click" & Quote & ", function() {" & CrLf &
                                                          "                    sidebar.classList.toggle(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                    content.classList.toggle(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                });" & CrLf & CrLf &
                                                          "                var headings = content.querySelectorAll(" & Quote & "h2, h4, h5, h6" & Quote & ");" & CrLf & CrLf &
                                                          "                for (var i = 0; i < headings.length; i++) {" & CrLf &
                                                          "                    var heading = headings[i];" & CrLf &
                                                          "                    var link = document.createElement(" & Quote & "a" & Quote & ");" & CrLf &
                                                          "                    link.href = " & Quote & "#" & Quote & " + heading.id;" & CrLf &
                                                          "                    link.textContent = heading.textContent;" & CrLf &
                                                          "                    if (link.textContent.indexOf(" & Quote & "We have ended" & Quote & ") === 0) {" & CrLf &
                                                          "                        return;" & CrLf &
                                                          "                    }" & CrLf &
                                                          "                    sidebar.appendChild(link);" & CrLf &
                                                          "                }" & CrLf &
                                                          "            });" & CrLf & CrLf &
                                                          "            window.addEventListener(" & Quote & "resize" & Quote & ", function() {" & CrLf &
                                                          "                var sidebar = document.getElementById(" & Quote & "sidebar" & Quote & ");" & CrLf &
                                                          "                var content = document.getElementById(" & Quote & "content" & Quote & ");" & CrLf & CrLf &
                                                          "                if (window.innerWidth < 680) {" & CrLf &
                                                          "                    sidebar.classList.add(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                    content.classList.add(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                } else {" & CrLf &
                                                          "                    sidebar.classList.remove(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                    content.classList.remove(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                }" & CrLf &
                                                          "            });" & CrLf & CrLf &
                                                          "        </script>" & CrLf & CrLf &
                                                          "    </body>" & CrLf &
                                                          "</html>" & CrLf
                               Try
                                   DynaLog.LogMessage("Parsing to HTML...")
                                   Dim pipeline = New MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
                                   Dim result As String = Markdown.ToHtml(TextBox1.Text, pipeline)
                                   DynaLog.LogMessage("Saving to prettier HTML report...")
                                   File.WriteAllText(Application.StartupPath & "\report.html", prettyHTML.Replace("<!-- Content Goes Here!!! -->", result))
                                   If File.Exists(Application.StartupPath & "\report.html") Then
                                       WebBrowser1.Navigate("file:///" & Application.StartupPath.Replace("\", "/").Trim() & "/report.html")
                                   End If
                                   BringToFront()
                               Catch ex As Exception
                                   DynaLog.LogMessage("Could not convert to HTML. Error message: " & ex.Message)
                                   DynaLog.LogMessage("This could be an issue with Markdig.")
                                   If MsgBox(LocalizationService.ForSection("InfoSave.Results.Messages").Format("HtmlFailed.Message", ex.Message), vbYesNo + vbCritical, LocalizationService.ForSection("InfoSave.Results.Messages")("ConversionError.Label")) = MsgBoxResult.Yes Then
                                       Process.Start(FilePath)
                                   End If
                                   Close()
                               End Try
                           Else
                               Close()
                           End If
                       End Sub)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        WebBrowser1.Visible = CheckBox1.Checked
    End Sub

    Private Sub InfoSaveResults_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If File.Exists(Application.StartupPath & "\report.html") Then
            DynaLog.LogMessage("Attempting to delete temporary report...")
            Try
                File.Delete(Application.StartupPath & "\report.html")
            Catch ex As Exception
                ' Let something else delete it
            End Try
        End If
    End Sub

    Private Sub WebBrowser1_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        ' TODO When using google to look up items online what we get is a cookie consent prompt that gets rid of our udm=14 thing, therefore showing google results with AI overview crap. Either find a way to pass in udm=14 or stop using google search. This CANNOT be replicated with saved reports, only here.

        If e.Url.AbsoluteUri.StartsWith("http", StringComparison.OrdinalIgnoreCase) Or e.Url.AbsoluteUri.StartsWith("ftp", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("An external link has been clicked. Opening it in the default browser...")
            Process.Start(e.Url.AbsoluteUri)
            WebBrowser1.Navigate("file:///" & Application.StartupPath.Replace("\", "/").Trim() & "/report.html")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        If File.Exists(Application.StartupPath & "\report.html") Then
            DynaLog.LogMessage("Attempting to save report to destination...")
            Try
                File.Copy(Path.Combine(Application.StartupPath, "report.html"), SaveFileDialog1.FileName, True)
            Catch ex As Exception

            End Try
        End If
    End Sub
End Class
