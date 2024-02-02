Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class ReloadProjectQuestionDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ReloadProjectQuestionDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = "This image is no longer available"
                        Label2.Text = "The image that was loaded in this project is no longer available. This can happen if it was unmounted by an external program. Because of this, the project needs to be reloaded. Click " & Quote & "OK" & Quote & " to reload this project." & CrLf & CrLf & "NOTE: if you click " & Quote & "Cancel" & Quote & ", the project will be unloaded"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Label1.Text = "Esta imagen ya no está disponible"
                        Label2.Text = "La imagen que fue cargada en este proyecto ya no está disponible. Esto puede ocurrir si dicha imagen fue desmontada por un programa externo. Debido a esto, el proyecto debe ser recargado. Haga clic en " & Quote & "Aceptar" & Quote & " para recargar este proyecto." & CrLf & CrLf & "NOTA: si hace clic en " & Quote & "Cancelar" & Quote & ", el proyecto será descargado"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Label1.Text = "Cette image n'est plus disponible"
                        Label2.Text = "L'image qui a été chargée dans ce projet n'est plus disponible. Cela peut se produire si elle a été démontée par un programme externe. Pour cette raison, le projet doit être rechargé. Cliquez sur " & Quote & "OK" & Quote & " pour recharger ce projet." & CrLf & CrLf & "NOTE: si vous cliquez sur " & Quote & "Annuler" & Quote & ", le projet sera déchargé."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                End Select
            Case 1
                Label1.Text = "This image is no longer available"
                Label2.Text = "The image that was loaded in this project is no longer available. This can happen if it was unmounted by an external program. Because of this, the project needs to be reloaded. Click " & Quote & "OK" & Quote & " to reload this project." & CrLf & CrLf & "NOTE: if you click " & Quote & "Cancel" & Quote & ", the project will be unloaded"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Label1.Text = "Esta imagen ya no está disponible"
                Label2.Text = "La imagen que fue cargada en este proyecto ya no está disponible. Esto puede ocurrir si dicha imagen fue desmontada por un programa externo. Debido a esto, el proyecto debe ser recargado. Haga clic en " & Quote & "Aceptar" & Quote & " para recargar este proyecto." & CrLf & CrLf & "NOTA: si hace clic en " & Quote & "Cancelar" & Quote & ", el proyecto será descargado"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Label1.Text = "Cette image n'est plus disponible"
                Label2.Text = "L'image qui a été chargée dans ce projet n'est plus disponible. Cela peut se produire si elle a été démontée par un programme externe. Pour cette raison, le projet doit être rechargé. Cliquez sur " & Quote & "OK" & Quote & " pour recharger ce projet." & CrLf & CrLf & "NOTE: si vous cliquez sur " & Quote & "Annuler" & Quote & ", le projet sera déchargé."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
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
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
