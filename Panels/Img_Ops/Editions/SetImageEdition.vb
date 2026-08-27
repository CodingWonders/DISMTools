Imports System.Windows.Forms
Imports DISMTools.Elements
Imports System.IO
Imports Microsoft.Dism

Public Class SetImageEdition
    Implements IImageTaskDialog

    Public TargetEditions As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.imgEditionNewEdition = ComboBox1.SelectedItem
        If MainForm.CurrentImage.ImageInstallationType.ToLower().Contains("server") AndAlso MainForm.OnlineManagement Then
            ProgressPanel.imgEditionCopyEula = RadioButton1.Checked
            ProgressPanel.imgEditionAcceptEula = RadioButton2.Checked
            If RadioButton1.Checked Then
                If (TextBox1.Text = "" Or Not Directory.Exists(TextBox1.Text)) Then
                    MsgBox(LocalizationService.ForSection("ImageOps.Editions.Set")("DirectoryMissing.Message"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
                    Exit Sub
                End If
                ProgressPanel.imgEditionEulaDestination = TextBox1.Text
            Else
                Dim productKey As ProductKey = ProductKeyValidator.ValidateProductKey(TextBox2.Text)
                If Not productKey.Valid Then
                    MsgBox(LocalizationService.ForSection("ImageOps.Editions.Set")("ProductKey.Message"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
                    Exit Sub
                End If
                ProgressPanel.imgEditionEditionKey = productKey.Key
            End If
        Else
            ProgressPanel.imgEditionCopyEula = False
            ProgressPanel.imgEditionAcceptEula = False
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 71
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Preparing to get target editions...")
        TargetEditions.Clear()
        DynaLog.LogMessage("Getting target editions...")
        Dim msg As String = ""
        Try
            DynaLog.LogMessage("Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            DynaLog.LogMessage("Creating session...")
            Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                Dim imageTargetEditions As DismEditionCollection = DismApi.GetTargetEditions(imgSession)
                DynaLog.LogMessage("Amount of target editions: " & imageTargetEditions.Count)
                If imageTargetEditions.Count > 0 Then
                    ' This image hasn't been upgraded to its highest edition
                    DynaLog.LogMessage("There are target editions. This image can give a little more")
                    For Each targetEdition In imageTargetEditions
                        TargetEditions.Add(targetEdition)
                    Next
                Else
                    ' This image has been upgraded to its highest edition
                    DynaLog.LogMessage("There are no target editions. This image is already rocking the best edition")
                    msg = LocalizationService.ForSection("ImageEdition.Initialize")("Image.Cannot.Message")
                    MsgBox(msg, vbOKOnly + vbInformation, Text)
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not grab edition targets. Error message: " & ex.Message)
            If MainForm.CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Then
                DynaLog.LogMessage("Image edition is WindowsPE. This is a Windows PE image.")
                msg = LocalizationService.ForSection("ImageEdition.Initialize")("Windows.Message")
            Else
                msg = ex.ToString()
            End If
            MsgBox(msg, vbOKOnly + vbExclamation, Text)
        Finally
            Try
                DismApi.Shutdown()
            Catch ex As Exception
                ' Don't do anything
            End Try
        End Try
        If TargetEditions.Count < 1 Then
            Return False
        Else
            ComboBox1.Items.Clear()
            ComboBox1.Items.AddRange(TargetEditions.ToArray())
            If ComboBox1.Items.Count > 0 Then
                ComboBox1.SelectedIndex = 0
            End If
        End If
        Return True
    End Function

    Private Sub SetImageEdition_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("ImageEdition")("Title.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("ImageEdition").Format("Image.Task.Header.Label", Text)
        Label1.Text = LocalizationService.ForSection("ImageEdition")("Target.Upgrade.Label")
        GroupBox1.Text = LocalizationService.ForSection("ImageEdition")("ServerOptions.Group")
        RadioButton1.Text = LocalizationService.ForSection("ImageEdition")("Copy.EndUser.RadioButton")
        RadioButton2.Text = LocalizationService.ForSection("ImageEdition")("AcceptEULA.RadioButton")
        Button1.Text = LocalizationService.ForSection("ImageEdition")("Browse.Button")
        OK_Button.Text = LocalizationService.ForSection("ImageEdition")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImageEdition")("Cancel.Button")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = BackColor
        TextBox1.BackColor = BackColor
        TextBox2.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        DynaLog.LogMessage("Determining EULA option compatibility...")
        DynaLog.LogMessage("- Image Installation Type: " & MainForm.CurrentImage.ImageProductType)
        DynaLog.LogMessage("- Managing Active Installation? " & If(MainForm.OnlineManagement, "Yes", "No"))
        ' Disable group box if not managing an active server installation
        If MainForm.CurrentImage.ImageInstallationType.ToLower().Contains("server") AndAlso MainForm.OnlineManagement Then
            DynaLog.LogMessage("All requirements are met. We are managing a Windows Server installation")
            GroupBox1.Enabled = True
        Else
            DynaLog.LogMessage("Either one or none of the two requirements described above is met. The image we are managing is not an active installation, or a Windows Server installation")
            GroupBox1.Enabled = False
        End If
        ImageTaskHeader1.HideWindowTitle(Handle)
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        EulaPanel.Enabled = RadioButton1.Checked
        TextBox2.Enabled = RadioButton2.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Selected path: " & FolderBrowserDialog1.SelectedPath)
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
End Class
