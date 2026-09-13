Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class AddProvisioningPkg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Selected provisioning package for addition: " & Quote & TextBox1.Text & Quote)
        If TextBox1.Text <> "" Then
            DynaLog.LogMessage("A provisioning package has been selected.")
            If File.Exists(TextBox1.Text) Then
                DynaLog.LogMessage("The provisioning package specified exists in the file system.")
                ProgressPanel.ppkgAdditionPackagePath = TextBox1.Text
            Else
                DynaLog.LogMessage("The provisioning package specified does not exist in the file system.")
                MsgBox(LocalizationService.ForSection("ProvPackage.Validation")("PackageNotFound.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Exit Sub
            End If
            DynaLog.LogMessage("Checking catalog path...")
            If TextBox2.Text <> "" And File.Exists(TextBox2.Text) Then
                DynaLog.LogMessage("A catalog path has been selected and exists in the file system.")
                ProgressPanel.ppkgAdditionCatalogPath = TextBox2.Text
            ElseIf TextBox2.Text <> "" And Not File.Exists(TextBox2.Text) Then
                DynaLog.LogMessage("Either no catalog path has been selected or it does not exist in the file system.")
                Dim msg As String = ""
                msg = LocalizationService.ForSection("ProvPackage.Validation")("CatalogNotFound.Message")
                If MsgBox(msg, vbYesNo + vbExclamation, ImageTaskHeader1.ItemText) = MsgBoxResult.No Then
                    Exit Sub
                End If
            Else
                ProgressPanel.ppkgAdditionCatalogPath = ""
            End If
        Else
            MsgBox(LocalizationService.ForSection("ProvPackage.Validation")("PackageRequired.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.ppkgAdditionCommit = If(CheckBox1.Checked, True, False)
        ProgressPanel.OperationNum = 33
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        OpenFileDialog2.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub OpenFileDialog2_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk
        TextBox2.Text = OpenFileDialog2.FileName
    End Sub

    Private Sub TextBox1_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub TextBox1_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox1.DragDrop
        TextBox1.Text = e.Data.GetData(DataFormats.FileDrop)
    End Sub

    Private Sub TextBox2_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox2.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub TextBox2_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox2.DragDrop
        TextBox2.Text = e.Data.GetData(DataFormats.FileDrop)
    End Sub

    Private Sub AddProvisioningPkg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ProvPackage")("Add.Packages.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("ProvPackage").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("ProvPackage")("PackagePath.Label")
        Label3.Text = LocalizationService.ForSection("ProvPackage")("Action.Treverted.Add.Message")
        Label4.Text = LocalizationService.ForSection("ProvPackage")("CatalogPath.Label")
        OK_Button.Text = LocalizationService.ForSection("ProvPackage")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ProvPackage")("Cancel.Button")
        Button1.Text = LocalizationService.ForSection("ProvPackage")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("ProvPackage")("Browse.Button")
        CheckBox1.Text = LocalizationService.ForSection("ProvPackage")("CommitImage.CheckBox")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub
End Class
