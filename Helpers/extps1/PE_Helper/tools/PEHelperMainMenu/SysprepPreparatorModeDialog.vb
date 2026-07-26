Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class SysprepPreparatorModeDialog

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MainForm.AutoCapture = CheckBox1.Checked
        MainForm.CopyProfile = CheckBox2.Checked
        DialogResult = System.Windows.Forms.DialogResult.Yes
        Close()
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        MainForm.AutoCapture = CheckBox1.Checked
        MainForm.CopyProfile = CheckBox2.Checked
        DialogResult = System.Windows.Forms.DialogResult.No
        Close()
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Private Sub SysprepPreparatorModeDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = MainForm.LinkLabel4.Text

        Label1.Text = LocalizationService.ForSection("PEHelper.Sysprep")("Responsibility.Message")
        LinkLabel1.Text = LocalizationService.ForSection("PEHelper.Sysprep")("AutomaticMode.Link")
        LinkLabel2.Text = LocalizationService.ForSection("PEHelper.Sysprep")("ManualMode.Link")
        LinkLabel3.Text = LocalizationService.ForSection("PEHelper.Sysprep")("Cancel.Link")
        CheckBox1.Text = LocalizationService.ForSection("PEHelper.Sysprep")("CaptureImage.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("PEHelper.Sysprep")("CopyRegistry.CheckBox")
    End Sub
End Class
