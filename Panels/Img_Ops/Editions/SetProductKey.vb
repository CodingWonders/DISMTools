Imports System.Windows.Forms
Imports DISMTools.Elements
Imports Microsoft.Dism

Public Class SetImageKey

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Preparing to validate the product key syntax...")
        Dim key As ProductKey = ProductKeyValidator.ValidateProductKey(TextBox1.Text)
        If Not key.Valid Then
            DynaLog.LogMessage("Syntactically, the product key is bad.")
            MsgBox("The product key has not been typed correctly.", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
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
            MsgBox("The product key has not been typed correctly.", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
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
                MsgBox("The product key is valid for this Windows image.", vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
            Else
                DynaLog.LogMessage("The product key cannot be applied to this Windows image.")
                MsgBox("The product key has been typed correctly, but is not valid for this Windows image.", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not validate product key. Error message: " & ex.Message)
            MsgBox("The product key has been typed correctly, but we could not check if it's valid for this Windows image.", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
        Finally
            Try
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
    End Sub

    Private Sub SetImageKey_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            ImageTaskHeader1.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            ImageTaskHeader1.ItemColor = ImageTaskHeader.ColorMode.Dark
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            ImageTaskHeader1.ItemColor = ImageTaskHeader.ColorMode.Light
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.ForeColor = Color.Black
        End If
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(Handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
