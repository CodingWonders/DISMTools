Imports System.Windows.Forms
Imports DISMTools.Elements

Public Class SetLayeredDriverDialog

    Dim CurrentKeyboardDriver As KeyboardDrivers.LayeredKeyboardDriver

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.currentKeybLayeredDriverType = CurrentKeyboardDriver
        ProgressPanel.KeyboardLayeredDriverType = (ComboBox1.SelectedIndex + 1)
        ProgressPanel.OperationNum = 60
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.Show(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SetLayeredDriver_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set to default value
        CurrentKeyboardDriver = KeyboardDrivers.LayeredKeyboardDriver.Unknown
        ' Color modes/language stuff go here
        Text = LocalizationService.ForSection("LayeredDriver.Set")("Title")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("LayeredDriver.Set").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("LayeredDriver.Set")("Intro.Message")
        Label3.Text = LocalizationService.ForSection("LayeredDriver.Set")("CurrentDriver.Label")
        Label5.Text = LocalizationService.ForSection("LayeredDriver.Set")("NewDriver.Label")
        Label6.Text = LocalizationService.ForSection("LayeredDriver.Set")("Driver.Already.Label")
        OK_Button.Text = LocalizationService.ForSection("LayeredDriver.Set")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("LayeredDriver.Set")("Cancel.Button")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        DynaLog.LogMessage("Getting currently installed keyboard layered driver in the Windows image...")

        ' Get keyboard driver
        CurrentKeyboardDriver = KeyboardDrivers.GetKeyboardDriver(MainForm.MountDir, MainForm.OnlineManagement)
        Select Case CurrentKeyboardDriver
            Case KeyboardDrivers.LayeredKeyboardDriver.Unknown
                Label4.Text = LocalizationService.ForSection("SetLayeredDriver")("UnknownInstalled.Label")
                ComboBox1.SelectedIndex = 0
            Case KeyboardDrivers.LayeredKeyboardDriver.PCATKey
                Label4.Text = LocalizationService.ForSection("SetLayeredDriver")("PC.Enhanced.Label")
                ComboBox1.SelectedIndex = 1
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT1
                Label4.Text = LocalizationService.ForSection("SetLayeredDriver.KoreanPC")("Keyboard101.Type1.Label")
                ComboBox1.SelectedIndex = 0
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT2
                Label4.Text = LocalizationService.ForSection("SetLayeredDriver")("DriverKorean.Label")
                ComboBox1.SelectedIndex = 0
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT3
                Label4.Text = LocalizationService.ForSection("SetLayeredDriver.KoreanPC")("Keyboard101.Type3.Label")
                ComboBox1.SelectedIndex = 0
            Case KeyboardDrivers.LayeredKeyboardDriver.K_103106Key
                Label4.Text = LocalizationService.ForSection("SetLayeredDriver")("Korean.Keyboard.Key.Item")
                ComboBox1.SelectedIndex = 0
            Case KeyboardDrivers.LayeredKeyboardDriver.J_106109Key
                Label4.Text = LocalizationService.ForSection("SetLayeredDriver")("Japanese.Keyboard.Key.Item")
                ComboBox1.SelectedIndex = 0
        End Select
        ' Do checks at startup
        If (CurrentKeyboardDriver - 1) = ComboBox1.SelectedIndex Then
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If (CurrentKeyboardDriver - 1) = ComboBox1.SelectedIndex Then
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
    End Sub
End Class
