Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgWinVistaIncompatibilityDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgWinVistaIncompatibilityDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label1.Text = "The program can't service Windows Vista images"
                        Label2.Text = "Neither this program nor DISM support servicing Windows Vista images. DISM is meant to service Windows 7 or newer images. You can still mount Windows Vista images, but all options will be disabled." & CrLf & CrLf & "If you still want to service a Windows Vista image, refer to the " & Quote & "Compatibility with older Windows versions" & Quote & " topic in the Help documentation." & CrLf & CrLf & "Do you want to continue?"
                        OK_Button.Text = "Yes"
                        Cancel_Button.Text = "No"
                    Case "ESN"
                        Label1.Text = "El programa no puede ofrecer servicio a imágenes de Windows Vista"
                        Label2.Text = "Ni este programa ni DISM soportan ofrecer servicio a imágenes de Windows Vista. DISM puede ofrecer servicio a imágenes de Windows 7 o posterior. Aún puede montar imágenes de Windows Vista, pero todas las opciones serán deshabilitadas." & CrLf & CrLf & "Si todavía desea ofrecer servicio a una imagen de Windows Vista, véase el tópico " & Quote & "Compatibilidad con versiones antiguas de Windows" & Quote & " en la documentación de ayuda." & CrLf & CrLf & "¿Desea continuar?"
                        OK_Button.Text = "Sí"
                        Cancel_Button.Text = "No"
                End Select
            Case 1
                Label1.Text = "The program can't service Windows Vista images"
                Label2.Text = "Neither this program nor DISM support servicing Windows Vista images. DISM is meant to service Windows 7 or newer images. You can still mount Windows Vista images, but all options will be disabled." & CrLf & CrLf & "If you still want to service a Windows Vista image, refer to the " & Quote & "Compatibility with older Windows versions" & Quote & " topic in the Help documentation." & CrLf & CrLf & "Do you want to continue?"
                OK_Button.Text = "Yes"
                Cancel_Button.Text = "No"
            Case 2
                Label1.Text = "El programa no puede ofrecer servicio a imágenes de Windows Vista"
                Label2.Text = "Ni este programa ni DISM soportan ofrecer servicio a imágenes de Windows Vista. DISM puede ofrecer servicio a imágenes de Windows 7 o posterior. Aún puede montar imágenes de Windows Vista, pero todas las opciones serán deshabilitadas." & CrLf & CrLf & "Si todavía desea ofrecer servicio a una imagen de Windows Vista, véase el tópico " & Quote & "Compatibilidad con versiones antiguas de Windows" & Quote & " en la documentación de ayuda." & CrLf & CrLf & "¿Desea continuar?"
                OK_Button.Text = "Sí"
                Cancel_Button.Text = "No"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            Panel1.BackColor = Color.FromArgb(31, 31, 31)
            Label1.ForeColor = Color.FromArgb(0, 122, 204)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            Panel1.BackColor = Color.FromArgb(238, 238, 242)
            Label1.ForeColor = Color.FromArgb(0, 51, 153)
        End If
    End Sub
End Class
