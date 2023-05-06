Imports System.IO
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

Public Class MainForm

    Dim btnToolTip As New ToolTip()
    Private isMouseDown As Boolean = False
    Private mouseOffset As Point
    Dim pageInt As Integer = 0

    Private Sub minBox_MouseEnter(sender As Object, e As EventArgs) Handles minBox.MouseEnter
        minBox.Image = My.Resources.minBox_focus
    End Sub

    Private Sub minBox_MouseLeave(sender As Object, e As EventArgs) Handles minBox.MouseLeave
        minBox.Image = My.Resources.minBox
    End Sub

    Private Sub minBox_MouseDown(sender As Object, e As MouseEventArgs) Handles minBox.MouseDown
        minBox.Image = My.Resources.minBox_down
    End Sub

    Private Sub minBox_MouseUp(sender As Object, e As MouseEventArgs) Handles minBox.MouseUp
        minBox.Image = My.Resources.minBox_focus
    End Sub

    Private Sub minBox_MouseHover(sender As Object, e As EventArgs) Handles minBox.MouseHover
        btnToolTip.SetToolTip(sender, "Minimize")
    End Sub

    Private Sub minBox_Click(sender As Object, e As EventArgs) Handles minBox.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub closeBox_MouseEnter(sender As Object, e As EventArgs) Handles closeBox.MouseEnter
        closeBox.Image = My.Resources.closebox_focus
    End Sub

    Private Sub closeBox_MouseLeave(sender As Object, e As EventArgs) Handles closeBox.MouseLeave
        closeBox.Image = My.Resources.closebox
    End Sub

    Private Sub closeBox_MouseDown(sender As Object, e As MouseEventArgs) Handles closeBox.MouseDown
        closeBox.Image = My.Resources.closebox_down
    End Sub

    Private Sub closeBox_MouseUp(sender As Object, e As MouseEventArgs) Handles closeBox.MouseUp
        closeBox.Image = My.Resources.closebox_focus
    End Sub

    Private Sub closeBox_MouseHover(sender As Object, e As EventArgs) Handles closeBox.MouseHover
        btnToolTip.SetToolTip(sender, "Close")
    End Sub

    Private Sub closeBox_Click(sender As Object, e As EventArgs) Handles closeBox.Click
        Close()
    End Sub

    Private Sub wndControlPanel_MouseDown(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Get the new position
            mouseOffset = New Point(-e.X, -e.Y)
            ' Set that left button is pressed
            isMouseDown = True
        End If
    End Sub

    Private Sub wndControlPanel_MouseMove(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            ' Get the new form position
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Location = mousePos
        End If
    End Sub

    Private Sub wndControlPanel_MouseUp(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "DISMTools Update Check System - Version " & Application.ProductVersion
    End Sub

    Private Sub ReleaseFetcherBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ReleaseFetcherBW.DoWork

    End Sub
End Class
