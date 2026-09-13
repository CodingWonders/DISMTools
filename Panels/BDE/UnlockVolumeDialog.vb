Imports System.Windows.Forms
Imports BDELib.BDELib
Imports BDELib.Classes

Public Class UnlockVolumeDialog

    Public DriveLetter As String

    Private PersistentVolumeID As String

    Private Function GetPersistentVolumeIdFromDriveLetter() As String
        Dim PersistentVolumeID As String = ""

        Dim EncryptedVolumeMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT PersistentVolumeID FROM Win32_EncryptableVolume WHERE DriveLetter = {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(DriveLetter).TrimEnd("\")), "root\cimv2\Security\MicrosoftVolumeEncryption")
        If EncryptedVolumeMOC Is Nothing Then Return PersistentVolumeID

        PersistentVolumeID = WMIHelper.GetObjectValue(EncryptedVolumeMOC(0), "PersistentVolumeID")
        Return PersistentVolumeID
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ' Form the recovery password
        Dim NumericalPassword As String = String.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}", RPS1.Text, RPS2.Text, RPS3.Text, RPS4.Text, RPS5.Text, RPS6.Text, RPS7.Text, RPS8.Text)
        Dim UnlockResult As UInteger = UnlockVolumeWithNumericalPassword(PersistentVolumeID, NumericalPassword)
        Select Case UnlockResult
            Case Constants.S_OK
                MessageBox.Show(LocalizationService.ForSection("BDE.UnlockVolume.Messages")("Success.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case Constants.FVE_E_NOT_ACTIVATED
                MessageBox.Show(LocalizationService.ForSection("BDE.UnlockVolume.Messages")("NotActivated.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Case Constants.FVE_E_PROTECTOR_NOT_FOUND
                MessageBox.Show(LocalizationService.ForSection("BDE.UnlockVolume.Messages")("ProtectorNotFound.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Case Constants.FVE_E_FAILED_AUTHENTICATION
                MessageBox.Show(LocalizationService.ForSection("BDE.UnlockVolume.Messages")("AuthenticationFailed.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Case Constants.FVE_E_INVALID_PASSWORD_FORMAT
                MessageBox.Show(LocalizationService.ForSection("BDE.UnlockVolume.Messages")("InvalidPassword.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Case Else
                MessageBox.Show(LocalizationService.ForSection("BDE.UnlockVolume.Messages").Format("UnknownError.Message", UnlockResult), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
        End Select
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub UnlockVolumeDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        RPS1.BackColor = BackColor
        RPS1.ForeColor = ForeColor
        RPS2.BackColor = BackColor
        RPS2.ForeColor = ForeColor
        RPS3.BackColor = BackColor
        RPS3.ForeColor = ForeColor
        RPS4.BackColor = BackColor
        RPS4.ForeColor = ForeColor
        RPS5.BackColor = BackColor
        RPS5.ForeColor = ForeColor
        RPS6.BackColor = BackColor
        RPS6.ForeColor = ForeColor
        RPS7.BackColor = BackColor
        RPS7.ForeColor = ForeColor
        RPS8.BackColor = BackColor
        RPS8.ForeColor = ForeColor
        RPS1.SelectAll()
        RPS1.Focus()

        ' Reset fields
        RPS1.Text = ""
        RPS2.Text = ""
        RPS3.Text = ""
        RPS4.Text = ""
        RPS5.Text = ""
        RPS6.Text = ""
        RPS7.Text = ""
        RPS8.Text = ""

        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        PersistentVolumeID = GetPersistentVolumeIdFromDriveLetter()

        ' Get the ID of the key protector
        Dim ProtectorIds As List(Of KeyProtector) = GetKeyProtectors(PersistentVolumeID)

        ' We're only interested in the key protectors for numerical passwords
        If Not ProtectorIds.Any(Function(protector) protector.ProtectorType = KeyProtectorType.NumericalPassword) Then
            MessageBox.Show(LocalizationService.ForSection("BDE.UnlockVolume.Messages")("NumericalProtectorMissing.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Cancel_Button.PerformClick()
            Exit Sub
        End If

        KeyProtectorIdLabel.Text = ProtectorIds.First(Function(protector) protector.ProtectorType = KeyProtectorType.NumericalPassword).ProtectorID.Replace("{", "").Replace("}", "")
    End Sub

    Private Sub RPS1_TextChanged(sender As Object, e As EventArgs) Handles RPS1.TextChanged
        If RPS1.Text.Length >= 6 Then
            ' Switch to the next segment
            RPS2.SelectAll()
            RPS2.Focus()
        End If
    End Sub

    Private Sub RPS2_TextChanged(sender As Object, e As EventArgs) Handles RPS2.TextChanged
        If RPS2.Text.Length >= 6 Then
            ' Switch to the next segment
            RPS3.SelectAll()
            RPS3.Focus()
        End If
    End Sub

    Private Sub RPS3_TextChanged(sender As Object, e As EventArgs) Handles RPS3.TextChanged
        If RPS3.Text.Length >= 6 Then
            ' Switch to the next segment
            RPS4.SelectAll()
            RPS4.Focus()
        End If
    End Sub

    Private Sub RPS4_TextChanged(sender As Object, e As EventArgs) Handles RPS4.TextChanged
        If RPS4.Text.Length >= 6 Then
            ' Switch to the next segment
            RPS5.SelectAll()
            RPS5.Focus()
        End If
    End Sub

    Private Sub RPS5_TextChanged(sender As Object, e As EventArgs) Handles RPS5.TextChanged
        If RPS5.Text.Length >= 6 Then
            ' Switch to the next segment
            RPS6.SelectAll()
            RPS6.Focus()
        End If
    End Sub

    Private Sub RPS6_TextChanged(sender As Object, e As EventArgs) Handles RPS6.TextChanged
        If RPS6.Text.Length >= 6 Then
            ' Switch to the next segment
            RPS7.SelectAll()
            RPS7.Focus()
        End If
    End Sub

    Private Sub RPS7_TextChanged(sender As Object, e As EventArgs) Handles RPS7.TextChanged
        If RPS7.Text.Length >= 6 Then
            ' Switch to the next segment
            RPS8.SelectAll()
            RPS8.Focus()
        End If
    End Sub

    Private Sub RPS8_TextChanged(sender As Object, e As EventArgs) Handles RPS8.TextChanged
        OK_Button.Enabled = RPS1.Text <> "" AndAlso RPS2.Text <> "" AndAlso RPS3.Text <> "" AndAlso RPS4.Text <> "" AndAlso RPS5.Text <> "" AndAlso RPS6.Text <> "" AndAlso RPS7.Text <> "" AndAlso RPS8.Text <> ""
    End Sub
End Class
