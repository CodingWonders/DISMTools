Imports System.IO
Imports System.Drawing.Printing

Public Class InfoSaveResults

    Dim document As PrintDocument = New PrintDocument()

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PrintDialog1.Document = document
        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            document.Print()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Sub doc_PrintPage(sender As Object, e As PrintPageEventArgs)
        e.Graphics.DrawString(TextBox1.Text, New Font(MainForm.LogFont, MainForm.LogFontSize, FontStyle.Regular), Brushes.Black, 20, 20)
    End Sub

    Private Sub InfoSaveResults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RemoveHandler document.PrintPage, AddressOf doc_PrintPage
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
        TextBox1.Font = New Font(MainForm.LogFont, MainForm.LogFontSize, If(MainForm.LogFontIsBold, FontStyle.Bold, FontStyle.Regular))
        AddHandler document.PrintPage, AddressOf doc_PrintPage
    End Sub
End Class