Imports System.IO
Imports System.Drawing.Printing
Imports Markdig
Imports Microsoft.VisualBasic.ControlChars

Public Class InfoSaveResults

    Dim document As PrintDocument = New PrintDocument()
    Dim stringToPrint As String = ""

    Public FilePath As String = ""

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Old implementation - 0.4-0.5.1
        'PrintDialog1.Document = document
        'If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '    stringToPrint = TextBox1.Text
        '    document.Print()
        'End If
        WebBrowser1.ShowPrintDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Sub doc_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim charsOnPage As Integer = 0
        Dim linesPerPage As Integer = 0
        Dim strFormat As New StringFormat()
        strFormat.Alignment = StringAlignment.Near
        Dim margin As Single = 20
        Dim printArea As New RectangleF(e.MarginBounds.Left - margin, e.MarginBounds.Top - margin, e.MarginBounds.Width + 2 * margin, e.MarginBounds.Height + 2 * margin)
        e.Graphics.MeasureString(stringToPrint, TextBox1.Font, e.MarginBounds.Size, StringFormat.GenericTypographic, charsOnPage, linesPerPage)
        e.Graphics.DrawString(stringToPrint, TextBox1.Font, Brushes.Black, e.MarginBounds, StringFormat.GenericTypographic)
        stringToPrint = stringToPrint.Substring(charsOnPage)
        e.HasMorePages = stringToPrint.Length > 0

        'e.Graphics.DrawString(TextBox1.Text, New Font(MainForm.LogFont, MainForm.LogFontSize, FontStyle.Regular), Brushes.Black, 20, 20)
    End Sub

    Private Sub InfoSaveResults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RemoveHandler document.PrintPage, AddressOf doc_PrintPage
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Image information report results"
                        Label1.Text = "The report has been saved to the location you had specified, and its contents will be shown below."
                        Button1.Text = "OK"
                        Button2.Text = "Print..."
                    Case "ESN"
                        Text = "Resultados del informe de información de la imagen"
                        Label1.Text = "El informe ha sido guardado en la ubicación que especificó, y sus contenidos serán mostrados abajo."
                        Button1.Text = "Aceptar"
                        Button2.Text = "Imprimir..."
                    Case "FRA"
                        Text = "Résultats du rapport d'information de l'image"
                        Label1.Text = "Le rapport a été sauvegardé à l'emplacement que vous aviez indiqué et son contenu s'affiche ci-dessous."
                        Button1.Text = "OK"
                        Button2.Text = "Imprimer..."
                    Case "PTB", "PTG"
                        Text = "Resultados do relatório de informações sobre imagens"
                        Label1.Text = "O relatório foi guardado na localização que especificou e o seu conteúdo será apresentado abaixo."
                        Button1.Text = "OK"
                        Button2.Text = "Imprimir..."
                    Case "ITA"
                        Text = "Risultati del rapporto sulle informazioni sull'immagine"
                        Label1.Text = "Il rapporto è stato salvato nella posizione specificata e il suo contenuto viene visualizzato sottostante."
                        Button1.Text = "OK"
                        Button2.Text = "Stampa..."
                End Select
            Case 1
                Text = "Image information report results"
                Label1.Text = "The report has been saved to the location you had specified, and its contents will be shown below."
                Button1.Text = "OK"
                Button2.Text = "Print..."
            Case 2
                Text = "Resultados del informe de información de la imagen"
                Label1.Text = "El informe ha sido guardado en la ubicación que especificó, y sus contenidos serán mostrados abajo."
                Button1.Text = "Aceptar"
                Button2.Text = "Imprimir..."
            Case 3
                Text = "Résultats du rapport d'information de l'image"
                Label1.Text = "Le rapport a été sauvegardé à l'emplacement que vous aviez indiqué et son contenu s'affiche ci-dessous."
                Button1.Text = "OK"
                Button2.Text = "Imprimer..."
            Case 4
                Text = "Resultados do relatório de informações sobre imagens"
                Label1.Text = "O relatório foi guardado na localização que especificou e o seu conteúdo será apresentado abaixo."
                Button1.Text = "OK"
                Button2.Text = "Imprimir..."
            Case 5
                Text = "Risultati del rapporto sulle informazioni sull'immagine"
                Label1.Text = "Il rapporto è stato salvato nella posizione specificata e il suo contenuto viene visualizzato sottostante."
                Button1.Text = "OK"
                Button2.Text = "Stampa..."
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
        TextBox1.Text = File.ReadAllText(ImgInfoSaveDlg.SaveTarget)
        TextBox1.Font = New Font(MainForm.LogFont, MainForm.LogFontSize, FontStyle.Regular)

        ' Convert Markdown report to HTML and add style tags to make the HTML report prettier. None of the contents need to be in <body> - all web browsers handle HTML content separately
        Dim prettyHTML As String = "<html>" & CrLf &
                                   "    <head>" & CrLf &
                                   "        <meta charset=" & Quote & "utf-8" & Quote & ">" & CrLf &
                                   "        <title>DISMTools Image Information Report</title>" & CrLf &
                                   "        <style>" & CrLf &
                                   "            body {" & CrLf &
                                   "                font-family: " & Quote & "Segoe UI" & Quote & ", Arial, Verdana, sans-serif" & CrLf &
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
                                   "        </style>" & CrLf &
                                   "    </head>" & CrLf &
                                   "</html>" & CrLf
        Try
            Dim pipeline = New MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
            Dim result As String = Markdown.ToHtml(TextBox1.Text, pipeline)
            File.WriteAllText(Application.StartupPath & "\report.html", prettyHTML & result)
            If File.Exists(Application.StartupPath & "\report.html") Then
                WebBrowser1.Navigate("file:///" & Application.StartupPath.Replace("\", "/").Trim() & "/report.html")
            End If
        Catch ex As Exception
            If MsgBox("Conversion to HTML has failed due to the following error: " & ex.Message & CrLf & CrLf & "Do you want to open this file in a text editor?", vbYesNo + vbCritical, "Conversion error") = MsgBoxResult.Yes Then
                Process.Start(FilePath)
                Close()
            End If
        End Try

        AddHandler document.PrintPage, AddressOf doc_PrintPage
        BringToFront()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        WebBrowser1.Visible = CheckBox1.Checked
        Button2.Visible = CheckBox1.Checked
        Label2.Visible = CheckBox1.Checked
    End Sub

    Private Sub InfoSaveResults_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If File.Exists(Application.StartupPath & "\report.html") Then
            Try
                File.Delete(Application.StartupPath & "\report.html")
            Catch ex As Exception
                ' Let something else delete it
            End Try
        End If
    End Sub

    Private Sub WebBrowser1_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        If e.Url.AbsoluteUri.StartsWith("http", StringComparison.OrdinalIgnoreCase) Or e.Url.AbsoluteUri.StartsWith("ftp", StringComparison.OrdinalIgnoreCase) Then
            Process.Start(e.Url.AbsoluteUri)
            WebBrowser1.Navigate("file:///" & Application.StartupPath.Replace("\", "/").Trim() & "/report.html")
        End If
    End Sub
End Class