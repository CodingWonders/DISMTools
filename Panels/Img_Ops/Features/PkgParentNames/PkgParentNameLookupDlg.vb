Imports System.Windows.Forms
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars

Public Class PkgParentNameLookupDlg

    Public pkgSource As String
    Public pkgArgs As String

    Public OriginatedFrom As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Name of selected parent package: " & Quote & TextBox1.Text & Quote)
        If TextBox1.Text = "" Then
            DynaLog.LogMessage("No package has been specified.")
            MsgBox(LocalizationService.ForSection("PkgNameLookup.Validation")("Package.Required.Message"), MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, LocalizationService.ForSection("PkgNameLookup.Validation")("Installed.Package.Title"))
            Exit Sub
        ElseIf Not ListBox1.Items.Contains(TextBox1.Text) Then
            DynaLog.LogMessage("A bogus package has been specified.")
            MsgBox(LocalizationService.ForSection("PkgNameLookup.Validation")("Package.Seem.Message"), MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, LocalizationService.ForSection("PkgNameLookup.Validation")("Installed.Package.Title"))
            Exit Sub
        Else
            Select Case OriginatedFrom
                Case "enablement"
                    EnableFeat.TextBox1.Text = TextBox1.Text
                Case "disablement"
                    DisableFeat.TextBox1.Text = TextBox1.Text
            End Select
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        TextBox1.Text = ListBox1.SelectedItem
    End Sub

    Private Sub PkgParentNameLookupDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("Package.Parent")("Installed.Package.Label")
        Label1.Text = LocalizationService.ForSection("Package.Parent")("Installed.Package.Names")
        Label2.Text = LocalizationService.ForSection("Package.Parent")("Name.ParentPackage.Label")
        Label3.Text = LocalizationService.ForSection("Package.Parent")("Get.Package.Names.Label")
        OK_Button.Text = LocalizationService.ForSection("Package.Parent")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("Package.Parent")("Cancel.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Control.CheckForIllegalCrossThreadCalls = False
        Label3.Visible = True
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
        ListBox1.Items.Clear()
        DynaLog.LogMessage("Grabbing packages obtained via the background processes...")
        ListBox1.Items.AddRange(MainForm.CurrentImage.ImagePackages.Select(Function(package) package.PackageName).ToArray())
        Label3.Visible = False
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
    End Sub

    Private Sub PackageListerBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles PackageListerBW.DoWork
        File.WriteAllText(Application.StartupPath & "\temp.bat",
                  "@echo off" & CrLf &
                  "dism /English /image=" & pkgSource & " /get-packages | findstr /c:" & Quote & "Package Identity : " & Quote & " > .\pkgnames")
        pkgProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        pkgArgs = "/c " & Application.StartupPath & "\temp.bat"
        pkgProc.StartInfo.Arguments = pkgArgs
        pkgProc.StartInfo.CreateNoWindow = True
        pkgProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        pkgProc.Start()
        pkgProc.WaitForExit()
        If Decimal.ToInt32(pkgProc.ExitCode) = 0 Then
            RemPackage.CheckedListBox1.Items.Clear()
            RemPackage.CheckedListBox2.Items.Clear()
            Debug.WriteLine("[INFO] Package names were successfully gathered. The program should return to normal state")
            Debug.WriteLine("Listing package names:" & CrLf & My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\pkgnames"))
            Dim pkgNames As New RichTextBox
            pkgNames.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\pkgnames")
            For x = 0 To pkgNames.Lines.Count - 1
                If pkgNames.Lines(x) = "" Then
                    Continue For
                End If
                ListBox1.Items.Add(pkgNames.Lines(x).Replace("Package Identity : ", "").Trim())
            Next
            File.Delete(Application.StartupPath & "\temp.bat")
            File.Delete(Application.StartupPath & "\pkgnames")
        Else
            Debug.WriteLine("[FAIL] Package names were not gathered. Please verify everything's working")
        End If
        Label3.Visible = False
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
    End Sub
End Class
