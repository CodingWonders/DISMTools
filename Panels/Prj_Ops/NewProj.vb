Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.RegularExpressions

Public Class NewProj

    Public SaveAsMode As Boolean

    Dim IsReqField1Valid As Boolean
    Dim IsReqField2Valid As Boolean

    Private InvalidFieldRegex As New Regex("(^(?:CON|AUX|PRN|LPT[1-9]|COM[1-9]|NUL))|(<|>|:|\" & Quote & "|/|\\|\||\?|\*)", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Creating project...")
        DynaLog.LogMessage("- Project name: " & TextBox1.Text)
        DynaLog.LogMessage("- Project path: " & Quote & TextBox2.Text & Quote)
        If Not Directory.Exists(TextBox2.Text) Then
            DynaLog.LogMessage("The project path does not exist. Asking user whether or not to create it...")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("NewProj.Validation").Format("Dir.Exist.Create.Message", TextBox2.Text)
            If MsgBox(msg, vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.Yes Then
                DynaLog.LogMessage("The user has decided to create the folder. Attempting to create it...")
                Try
                    Directory.CreateDirectory(TextBox2.Text)
                Catch ex As Exception
                    DynaLog.LogMessage("Could not create the folder. Error message: " & ex.Message)
                    msg = LocalizationService.ForSection("NewProj.Validation").Format("Create.Project.Dir.Message", ex.ToString(), ex.Message)
                    MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Exit Sub
                End Try
            Else
                Exit Sub
            End If
        End If
        If SaveAsMode Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Hide()
            Exit Sub
        End If
        ProgressPanel.OperationNum = 0
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        If MainForm.isProjectLoaded Then
            DynaLog.LogMessage("Unloading any loaded projects...")
            If MainForm.OnlineManagement Then
                MainForm.EndOnlineManagement()
            ElseIf MainForm.OfflineManagement Then
                MainForm.EndOfflineManagement()
            Else
                MainForm.UnloadDTProj(False, True)
            End If
            If MainForm.ImgBW.IsBusy Then Exit Sub
        End If
        ProgressPanel.projName = TextBox1.Text
        ProgressPanel.projPath = TextBox2.Text
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub NewProj_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If SaveAsMode Then
            Text = LocalizationService.ForSection("Main.Interface")("SaveProjectas.Button").Replace("&", "").TrimEnd("."c)
            ImageTaskHeader1.ItemText = Text
        Else
            Text = LocalizationService.ForSection("NewProj")("Create.Project.Label")
            ImageTaskHeader1.ItemText = LocalizationService.ForSection("NewProj").Format("Image.Task.Header.Label", Text)
        End If
        Label2.Text = LocalizationService.ForSection("NewProj")("Options.Required.Label")
        Label3.Text = LocalizationService.ForSection("NewProj")("Name.Label")
        Label4.Text = LocalizationService.ForSection("NewProj")("Location.Label")
        Label5.Text = LocalizationService.ForSection("NewProj")("Fields.End.Required.Label")
        Button1.Text = LocalizationService.ForSection("NewProj")("Browse.Button")
        OK_Button.Text = If(SaveAsMode, LocalizationService.ForSection("Main.SaveProjectAs")("Save.Button"), LocalizationService.ForSection("NewProj")("Ok.Button"))
        Cancel_Button.Text = LocalizationService.ForSection("NewProj")("Cancel.Button")
        GroupBox1.Text = LocalizationService.ForSection("NewProj")("Project.Group")
        FolderBrowserDialog1.Description = LocalizationService.ForSection("NewProj")("Folder.Store.Description")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog(Me)
        If DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
            IsReqField2Valid = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        DynaLog.LogMessage("Specified project name: " & Quote & TextBox1.Text & Quote)
        If TextBox1.Text <> "" Then
            DynaLog.LogMessage("Verifying name...")
            If InvalidFieldRegex.IsMatch(TextBox1.Text) Then
                DynaLog.LogMessage("Project name contains invalid text.")
                IsReqField1Valid = False
            Else
                DynaLog.LogMessage("Project name is valid.")
                IsReqField1Valid = True
            End If
        Else
            DynaLog.LogMessage("No project name has been specified.")
            IsReqField1Valid = False
        End If
        CheckReqFields()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        DynaLog.LogMessage("Specified project path: " & Quote & TextBox2.Text & Quote)
        If TextBox2.Text <> "" Then
            DynaLog.LogMessage("Verifying path...")
            If InvalidFieldRegex.IsMatch(TextBox1.Text) Then
                DynaLog.LogMessage("Project path contains invalid text.")
                IsReqField2Valid = False
            Else
                DynaLog.LogMessage("Project path is valid.")
                IsReqField2Valid = True
            End If
        Else
            DynaLog.LogMessage("No project path has been specified.")
            IsReqField2Valid = False
        End If
        CheckReqFields()
    End Sub

    Sub CheckReqFields()
        DynaLog.LogMessage("Checking fields...")
        If IsReqField1Valid And IsReqField2Valid Then
            DynaLog.LogMessage("All fields are valid.")
            OK_Button.Enabled = True
        Else
            DynaLog.LogMessage("None or not all fields are valid.")
            OK_Button.Enabled = False
        End If
    End Sub
End Class
