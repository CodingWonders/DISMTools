Imports System.IO
Imports System.Drawing.Printing

Public Class InfoSaveResults

    Dim document As PrintDocument = New PrintDocument()
    Dim stringToPrint As String = ""

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PrintDialog1.Document = document
        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            stringToPrint = TextBox1.Text
            document.Print()
        End If
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
                        Label1.Text = "The report has been saved to the location you had specified, and its contents will be shown in the text box below."
                        Button1.Text = "OK"
                        Button2.Text = "Print..."
                    Case "ESN"
                        Text = "Resultados del informe de información de la imagen"
                        Label1.Text = "El informe ha sido guardado en la ubicación que especificó, y sus contenidos serán mostrados en la caja de texto de abajo."
                        Button1.Text = "Aceptar"
                        Button2.Text = "Imprimir..."
                    Case "FRA"
                        Text = "Résultats du rapport d'information de l'image"
                        Label1.Text = "Le rapport a été sauvegardé à l'emplacement que vous aviez indiqué et son contenu s'affiche dans la zone de texte ci-dessous."
                        Button1.Text = "OK"
                        Button2.Text = "Imprimer..."
                    Case "PTB", "PTG"
                        Text = "Resultados do relatório de informações sobre imagens"
                        Label1.Text = "O relatório foi guardado na localização que especificou e o seu conteúdo será apresentado na caixa de texto abaixo."
                        Button1.Text = "OK"
                        Button2.Text = "Imprimir..."
                    Case "ITA"
                        Text = "Risultati del rapporto sulle informazioni sull'immagine"
                        Label1.Text = "Il rapporto è stato salvato nella posizione specificata e il suo contenuto viene visualizzato nella casella di testo sottostante."
                        Button1.Text = "OK"
                        Button2.Text = "Stampa..."
                End Select
            Case 1
                Text = "Image information report results"
                Label1.Text = "The report has been saved to the location you had specified, and its contents will be shown in the text box below."
                Button1.Text = "OK"
                Button2.Text = "Print..."
            Case 2
                Text = "Resultados del informe de información de la imagen"
                Label1.Text = "El informe ha sido guardado en la ubicación que especificó, y sus contenidos serán mostrados en la caja de texto de abajo."
                Button1.Text = "Aceptar"
                Button2.Text = "Imprimir..."
            Case 3
                Text = "Résultats du rapport d'information de l'image"
                Label1.Text = "Le rapport a été sauvegardé à l'emplacement que vous aviez indiqué et son contenu s'affiche dans la zone de texte ci-dessous."
                Button1.Text = "OK"
                Button2.Text = "Imprimer..."
            Case 4
                Text = "Resultados do relatório de informações sobre imagens"
                Label1.Text = "O relatório foi guardado na localização que especificou e o seu conteúdo será apresentado na caixa de texto abaixo."
                Button1.Text = "OK"
                Button2.Text = "Imprimir..."
            Case 5
                Text = "Risultati del rapporto sulle informazioni sull'immagine"
                Label1.Text = "Il rapporto è stato salvato nella posizione specificata e il suo contenuto viene visualizzato nella casella di testo sottostante."
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
        AddHandler document.PrintPage, AddressOf doc_PrintPage
        BringToFront()
    End Sub
End Class