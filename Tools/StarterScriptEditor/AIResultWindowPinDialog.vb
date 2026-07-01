Imports System.Windows.Forms
Imports StarterScriptEditor.Classes.ColorUtilities

Public Class AIResultWindowPinDialog

    Private PMBounds As Rectangle = Screen.PrimaryScreen.Bounds

    Private CurrentColorMode As ColorThemeMode

    Public PinMode As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AIResultWindowPinDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CurrentColorMode = MainForm.CurrentColorMode
        SetColorMode()
        WindowHelper.DisableCloseCapability(Handle)

        PMDetailLabel.Text = String.Format("Primary Screen:{0}{1}x{2} pixels", Environment.NewLine, PMBounds.Width, PMBounds.Height)
    End Sub

    Private Sub SetColorMode()
        Select Case CurrentColorMode
            Case ColorThemeMode.Light
                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                ForeColor = Color.White
        End Select
    End Sub

    Private Sub ClearWindowOffsetDetails(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TopLeftBtn.MouseLeave, TopRightBtn.MouseLeave, BottomLeftBtn.MouseLeave, BottomRightBtn.MouseLeave
        OffsetDetailLabel.Text = ""
    End Sub

    Private Sub ShowWindowOffsetDetails(ByVal pinMode As Integer, ByVal x As Integer, ByVal y As Integer)
        Dim pinCornerDetails As String = ""
        Select Case pinMode
            Case 0 : pinCornerDetails = "top-left"
            Case 1 : pinCornerDetails = "top-right"
            Case 2 : pinCornerDetails = "bottom-left"
            Case 3 : pinCornerDetails = "bottom-right"
        End Select
        OffsetDetailLabel.Text = String.Format("Move window to the {0}{1}({2}x{3})", pinCornerDetails, Environment.NewLine, x, y)
    End Sub

    Private Sub TopLeftBtn_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TopLeftBtn.MouseEnter
        ShowWindowOffsetDetails(0, 16, 16)
    End Sub

    Private Sub TopRightBtn_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TopRightBtn.MouseEnter
        ShowWindowOffsetDetails(1, PMBounds.Width - AIResults.Width - 16, 16)
    End Sub

    Private Sub BottomLeftBtn_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BottomLeftBtn.MouseEnter
        ShowWindowOffsetDetails(2, 16, PMBounds.Height - AIResults.Height - 48)
    End Sub

    Private Sub BottomRightBtn_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BottomRightBtn.MouseEnter
        ShowWindowOffsetDetails(3, PMBounds.Width - AIResults.Width - 16, PMBounds.Height - AIResults.Height - 48)
    End Sub

    Private Sub CloseWindow()
        DialogResult = Windows.Forms.DialogResult.OK
        ClearWindowOffsetDetails(Nothing, Nothing)
        Close()
    End Sub

    Private Sub TopLeftBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TopLeftBtn.Click
        PinMode = 0
        CloseWindow()
    End Sub

    Private Sub TopRightBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TopRightBtn.Click
        PinMode = 1
        CloseWindow()
    End Sub

    Private Sub BottomLeftBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BottomLeftBtn.Click
        PinMode = 2
        CloseWindow()
    End Sub

    Private Sub BottomRightBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BottomRightBtn.Click
        PinMode = 3
        CloseWindow()
    End Sub
End Class
