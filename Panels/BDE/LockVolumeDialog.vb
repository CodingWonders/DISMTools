Imports System.Windows.Forms
Imports BDELib.BDELib
Imports BDELib.Classes

Public Class LockVolumeDialog

    Public DriveLetter As String

    Private PersistentVolumeID As String

    Private Function GetPersistentVolumeIdFromDriveLetter() As String
        Dim PersistentVolumeID As String = ""

        Dim EncryptedVolumeMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT PersistentVolumeID FROM Win32_EncryptableVolume WHERE DriveLetter = {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(DriveLetter).TrimEnd("\")), "root\cimv2\Security\MicrosoftVolumeEncryption")
        If EncryptedVolumeMOC Is Nothing Then Return PersistentVolumeID

        PersistentVolumeID = WMIHelper.GetObjectValue(EncryptedVolumeMOC(0), "PersistentVolumeID")
        Return PersistentVolumeID
    End Function

    Private Sub UnlockVolumeDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor

        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        PersistentVolumeID = GetPersistentVolumeIdFromDriveLetter()

        DrLetterLabel.Text = DriveLetter
        PersistentVolumeIdLabel.Text = PersistentVolumeID

        Visible = True

        Dim lockResult As UInteger = LockVolume(PersistentVolumeID)
        Select Case lockResult
            Case Constants.S_OK : ' Ignore
            Case Constants.E_ACCESS_DENIED : MessageBox.Show(LocalizationService.ForSection("BDE.LockVolume.Messages")("AccessDenied.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case Constants.E_ACCESS_DENIED : MessageBox.Show(LocalizationService.ForSection("BDE.LockVolume.Messages")("AccessDenied.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case Constants.FVE_E_LOCKED_VOLUME : MessageBox.Show(LocalizationService.ForSection("BDE.LockVolume.Messages")("AlreadyLocked.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case Constants.FVE_E_NOT_ENCRYPTED : MessageBox.Show(LocalizationService.ForSection("BDE.LockVolume.Messages")("NotEncrypted.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case Constants.FVE_E_PROTECTION_DISABLED : MessageBox.Show(LocalizationService.ForSection("BDE.LockVolume.Messages")("ProtectionDisabled.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case Constants.FVE_E_RECOVERY_KEY_REQUIRED : MessageBox.Show(LocalizationService.ForSection("BDE.LockVolume.Messages")("RecoveryKeyRequired.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case Else : MessageBox.Show(LocalizationService.ForSection("BDE.LockVolume.Messages").Format("UnknownError.Message", lockResult), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Select

        Close()
    End Sub
End Class
