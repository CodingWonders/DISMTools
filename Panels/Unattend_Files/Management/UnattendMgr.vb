Public Class UnattendMgr

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub LinkLabel1_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel1.MouseEnter
        StatusLbl.Text = "Create a new unattended answer file tailored to the selected image"
    End Sub

    Private Sub LinkLabel2_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel2.MouseEnter
        StatusLbl.Text = "Copy an already existing unattended answer file to the project location for later use"
    End Sub

    Private Sub LinkLabel3_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel3.MouseEnter
        StatusLbl.Text = "Apply an image to a destination whilst setting up everything using the unattended answer file"
    End Sub

    Private Sub LinkLabel4_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel4.MouseEnter
        StatusLbl.Text = "Recycle or delete the selected unattended answer file"
    End Sub

    Private Sub LinkLabelMouseLeave(sender As Object, e As EventArgs) Handles LinkLabel1.MouseLeave, LinkLabel2.MouseLeave, LinkLabel3.MouseLeave, LinkLabel4.MouseLeave
        StatusLbl.Text = "Hover over an action to see its description"
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        NewUnattendWiz.Show()
    End Sub
End Class