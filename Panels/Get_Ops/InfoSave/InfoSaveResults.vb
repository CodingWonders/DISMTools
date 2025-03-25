Imports System.IO
Imports System.Drawing.Printing
Imports Markdig
Imports Microsoft.VisualBasic.ControlChars

Public Class InfoSaveResults

    Dim document As PrintDocument = New PrintDocument()
    Dim stringToPrint As String = ""

    Public FilePath As String = ""

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Private Sub InfoSaveResults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Image information report results"
                        Label1.Text = "The report has been saved to the location you had specified, and its contents will be shown below."
                        Button1.Text = "OK"
                    Case "ESN"
                        Text = "Resultados del informe de información de la imagen"
                        Label1.Text = "El informe ha sido guardado en la ubicación que especificó, y sus contenidos serán mostrados abajo."
                        Button1.Text = "Aceptar"
                    Case "FRA"
                        Text = "Résultats du rapport d'information de l'image"
                        Label1.Text = "Le rapport a été sauvegardé à l'emplacement que vous aviez indiqué et son contenu s'affiche ci-dessous."
                        Button1.Text = "OK"
                    Case "PTB", "PTG"
                        Text = "Resultados do relatório de informações sobre imagens"
                        Label1.Text = "O relatório foi guardado na localização que especificou e o seu conteúdo será apresentado abaixo."
                        Button1.Text = "OK"
                    Case "ITA"
                        Text = "Risultati del rapporto sulle informazioni sull'immagine"
                        Label1.Text = "Il rapporto è stato salvato nella posizione specificata e il suo contenuto viene visualizzato sottostante."
                        Button1.Text = "OK"
                End Select
            Case 1
                Text = "Image information report results"
                Label1.Text = "The report has been saved to the location you had specified, and its contents will be shown below."
                Button1.Text = "OK"
            Case 2
                Text = "Resultados del informe de información de la imagen"
                Label1.Text = "El informe ha sido guardado en la ubicación que especificó, y sus contenidos serán mostrados abajo."
                Button1.Text = "Aceptar"
            Case 3
                Text = "Résultats du rapport d'information de l'image"
                Label1.Text = "Le rapport a été sauvegardé à l'emplacement que vous aviez indiqué et son contenu s'affiche ci-dessous."
                Button1.Text = "OK"
            Case 4
                Text = "Resultados do relatório de informações sobre imagens"
                Label1.Text = "O relatório foi guardado na localização que especificou e o seu conteúdo será apresentado abaixo."
                Button1.Text = "OK"
            Case 5
                Text = "Risultati del rapporto sulle informazioni sull'immagine"
                Label1.Text = "Il rapporto è stato salvato nella posizione specificata e il suo contenuto viene visualizzato sottostante."
                Button1.Text = "OK"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        TextBox1.Clear()
        DynaLog.LogMessage("Checking if the report exists...")
        If File.Exists(ImgInfoSaveDlg.SaveTarget) Then
            DynaLog.LogMessage("The report exists. Reading and parsing to HTML...")
            TextBox1.Text = File.ReadAllText(ImgInfoSaveDlg.SaveTarget)
            TextBox1.Font = New Font(MainForm.LogFont, MainForm.LogFontSize, FontStyle.Regular)

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
                                       "                font-family: Inconsolata, " & Quote & "Cascadia Code" & Quote & ", Consolas, " & Quote & "Courier New" & Quote & ";" & CrLf &
                                       "                font-size: 16px" & CrLf &
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
                If MsgBox("Conversion to HTML has failed due to the following error: " & ex.Message & CrLf & CrLf & "Do you want to open this file in a text editor?", vbYesNo + vbCritical, "Conversion error") = MsgBoxResult.Yes Then
                    Process.Start(FilePath)
                End If
                Close()
            End Try
        Else
            Close()
        End If
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
        If e.Url.AbsoluteUri.StartsWith("http", StringComparison.OrdinalIgnoreCase) Or e.Url.AbsoluteUri.StartsWith("ftp", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("An external link has been clicked. Opening it in the default browser...")
            Process.Start(e.Url.AbsoluteUri)
            WebBrowser1.Navigate("file:///" & Application.StartupPath.Replace("\", "/").Trim() & "/report.html")
        End If
    End Sub
End Class