Imports System.Windows.Forms
Imports DISMTools.Elements
Imports Microsoft.Dism

Public Class SetImageKey
    Implements IImageTaskDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Preparing to validate the product key syntax...")
        Dim key As ProductKey = ProductKeyValidator.ValidateProductKey(TextBox1.Text)
        If Not key.Valid Then
            DynaLog.LogMessage("Syntactically, the product key is bad.")
            MsgBox(LocalizationService.ForSection("SetImageKey.Messages")("ProductKey.Has.Label"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.OperationNum = 72
        ProgressPanel.pkSetNewProductKey = TextBox1.Text
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
        DynaLog.LogMessage("Preparing to validate the product key...")
        DynaLog.LogMessage("Stage 1: Product Key Syntax Check...")
        Dim key As ProductKey = ProductKeyValidator.ValidateProductKey(TextBox1.Text)
        If Not key.Valid Then
            DynaLog.LogMessage("Syntactically, the product key is bad.")
            MsgBox(LocalizationService.ForSection("SetImageKey.Messages")("ProductKey.Has.Label"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        DynaLog.LogMessage("Syntactically, the product key is good. Passing to stage 2...")
        DynaLog.LogMessage("Stage 2: Product Key Validation Check...")
        Dim validKey As Boolean
        Try
            DynaLog.LogMessage("Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            DynaLog.LogMessage("Creating session and validating key...")
            Using imgSession As DismSession = DismApi.OpenOfflineSession(MainForm.MountDir)
                validKey = DismApi.ValidateProductKey(imgSession, TextBox1.Text)
            End Using
            If validKey Then
                DynaLog.LogMessage("The product key can be applied to this Windows image.")
                MsgBox(LocalizationService.ForSection("SetImageKey.Messages")("ProductKey.Windows.Label"), vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
            Else
                DynaLog.LogMessage("The product key cannot be applied to this Windows image.")
                MsgBox(LocalizationService.ForSection("SetImageKey.Messages")("ProductKey.Valid.Message"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not validate product key. Error message: " & ex.Message)
            MsgBox(LocalizationService.ForSection("SetImageKey.Validation")("ProductKey.Valid.Message"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
        Finally
            Try
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        Dim msg As String = ""
        If MainForm.CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("Image edition is WindowsPE. This is a Windows PE image.")
            msg = LocalizationService.ForSection("SetImageKey.Initialize")("Windows.Message")
            MsgBox(msg, vbOKOnly + vbInformation, Text)
            Return False
        End If
        Return True
    End Function

    Private Sub SetImageKey_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("SetImageKey")("SetProductKey.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("SetImageKey").Format("Image.Task.Header.Label", Text)
        Label1.Text = LocalizationService.ForSection("SetImageKey")("Type.ProductKey.Label")
        Label2.Text = LocalizationService.ForSection("SetImageKey")("Check.ProductKey.Message")
        Button1.Text = LocalizationService.ForSection("SetImageKey")("ValidateKey.Button")
        OK_Button.Text = LocalizationService.ForSection("SetImageKey")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("SetImageKey")("Cancel.Button")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = CurrentTheme.ForegroundColor
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(Handle)
    End Sub
End Class
