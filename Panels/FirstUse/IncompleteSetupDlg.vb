Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class IncompleteSetupDlg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub IncompleteSetupDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENG"
                Label1.Text = "Setup is not complete yet, and your custom settings will not be saved. Proceeding will make the program use default settings." & CrLf & CrLf & "Do you want to proceed?"
                OK_Button.Text = "Yes"
                Cancel_Button.Text = "No"
            Case "ESN"
                Label1.Text = "No ha terminado de configurar el programa, y sus preferencias no serán guardadas. Si continúa, el programa utilizará configuraciones predeterminadas." & CrLf & CrLf & "¿Desea continuar?"
                OK_Button.Text = "Sí"
                Cancel_Button.Text = "No"
        End Select
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(Handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        Beep()
    End Sub
End Class
