﻿Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports System.Threading

Public Class ApplicationDriveSpecifier

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If RichTextBox1.Text.Contains(TextBox1.Text) Then
            ImgApply.TextBox3.Text = TextBox1.Text
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MsgBox("The specified Drive ID does not exist. Please specify an existing Drive ID and try again. You can also refresh the list if you've just plugged or unplugged external drives", MsgBoxStyle.Critical, "Destination drive")
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ApplicationDriveSpecifier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            RichTextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            RichTextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        Dim WmicProc As Process = Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe", "/c .\bin\dthelper.bat /drinfo")
        Do Until WmicProc.HasExited
            If WmicProc.HasExited Then
                Exit Do
            End If
        Loop
        Try
            RichTextBox1.Text = File.ReadAllText(".\wmic")
            File.Delete(".\wmic")
        Catch ex As Exception

        End Try
        BringToFront()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim WmicProc As Process = Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe", "/c .\bin\dthelper.bat /drinfo")
        Do Until WmicProc.HasExited
            If WmicProc.HasExited Then
                Exit Do
            End If
        Loop
        Try
            RichTextBox1.Text = File.ReadAllText(".\wmic")
            File.Delete(".\wmic")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RichTextBox1_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox1.LinkClicked
        TextBox1.Text = e.LinkText
    End Sub
End Class
