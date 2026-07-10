Imports BDELib.Classes
Imports System.Text.RegularExpressions

Namespace Helpers

    Public Class VolumeHelper

        Public Shared Function GetEncryptedVolumes() As List(Of EncryptableVolume)
            Dim encryptedVolumes As New List(Of EncryptableVolume)

            Dim encryptedDrives As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT * FROM Win32_EncryptableVolume WHERE ProtectionStatus > 0", "root\cimv2\Security\MicrosoftVolumeEncryption")
            If encryptedDrives Is Nothing Then Return encryptedVolumes

            For Each encryptedDrive As ManagementObject In encryptedDrives
                Dim encryptedVolumeProperties As Dictionary(Of String, Object) = WMIHelper.GetObjectValues(encryptedDrive, "DeviceID", "DriveLetter", "PersistentVolumeID")
                encryptedVolumes.Add(New EncryptableVolume(encryptedVolumeProperties("DeviceID"), encryptedVolumeProperties("DriveLetter"), encryptedVolumeProperties("PersistentVolumeID")))
            Next

            Return encryptedVolumes
        End Function

        Private Shared Function GetEncryptedVolumeManagementInstance(PersistentVolumeId As String) As ManagementObject
            Dim managementResults As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT * FROM Win32_EncryptableVolume WHERE PersistentVolumeId = {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(PersistentVolumeId)), "root\cimv2\Security\MicrosoftVolumeEncryption")
            Return If(managementResults Is Nothing, Nothing, managementResults(0))
        End Function

        Public Shared Function GetVolumeKeyProtectors(PersistentVolumeId As String, Optional ProtectorType As KeyProtectorType = KeyProtectorType.Unknown) As List(Of KeyProtector)
            Dim keyProtectors As New List(Of KeyProtector)

            Try
                Dim encryptedVolumeInstance As ManagementObject = GetEncryptedVolumeManagementInstance(PersistentVolumeId)
                If encryptedVolumeInstance Is Nothing Then Return keyProtectors

                Dim encryptedVolumeBaseObject As ManagementBaseObject = encryptedVolumeInstance.GetMethodParameters("GetKeyProtectors")
                encryptedVolumeBaseObject("KeyProtectorType") = ProtectorType
                Dim protectorResults As ManagementBaseObject = encryptedVolumeInstance.InvokeMethod("GetKeyProtectors", encryptedVolumeBaseObject, Nothing)
                If protectorResults Is Nothing Then Return keyProtectors

                Dim KeyProtectorIds As String() = protectorResults("VolumeKeyProtectorID")
                For Each KeyProtectorId In KeyProtectorIds
                    Dim volumeKeyProtector As KeyProtector = Nothing

                    Try
                        Dim keyProtectorBaseObject As ManagementBaseObject = encryptedVolumeInstance.GetMethodParameters("GetKeyProtectorType")
                        keyProtectorBaseObject("VolumeKeyProtectorID") = KeyProtectorId
                        Dim keyProtectorResults As ManagementBaseObject = encryptedVolumeInstance.InvokeMethod("GetKeyProtectorType", keyProtectorBaseObject, Nothing)
                        If keyProtectorResults Is Nothing Then Throw New Exception()

                        Dim volumeKeyProtectorType As KeyProtectorType = keyProtectorResults("KeyProtectorType")
                        keyProtectors.Add(New KeyProtector(KeyProtectorId, volumeKeyProtectorType))
                    Catch ex As Exception
                        Continue For
                    End Try

                    keyProtectors.Add(volumeKeyProtector)
                Next
            Catch ex As Exception
                Return keyProtectors
            End Try

            Return keyProtectors
        End Function

        Public Shared Function GetVolumeLockStatus(PersistentVolumeId As String) As LockStatus
            Try
                Dim encryptedVolumeInstance As ManagementObject = GetEncryptedVolumeManagementInstance(PersistentVolumeId)
                If encryptedVolumeInstance Is Nothing Then Return LockStatus.Unknown

                Dim lockStatusBaseObject As ManagementBaseObject = encryptedVolumeInstance.GetMethodParameters("GetLockStatus"),
                    lockStatusResults As ManagementBaseObject = encryptedVolumeInstance.InvokeMethod("GetLockStatus", lockStatusBaseObject, Nothing)

                If lockStatusResults Is Nothing OrElse lockStatusResults("ReturnValue") <> Constants.S_OK Then Throw New Exception()

                Return lockStatusResults("LockStatus")
            Catch ex As Exception
                Return LockStatus.Unknown
            End Try
        End Function

        Public Shared Function LockVolume(PersistentVolumeId As String) As UInteger
            Try
                Dim encryptedVolumeInstance As ManagementObject = GetEncryptedVolumeManagementInstance(PersistentVolumeId)
                If encryptedVolumeInstance Is Nothing Then Throw New Exception()

                Dim LockResults As ManagementBaseObject = encryptedVolumeInstance.InvokeMethod("Lock", encryptedVolumeInstance.GetMethodParameters("Lock"), Nothing)
                If LockResults Is Nothing Then Throw New Exception()

                Return LockResults("ReturnValue")
            Catch ex As Exception
                Return Constants.E_FAIL
            End Try
        End Function

        Private Shared Function ValidateNumericalPasswordFormat(NumericalPassword As String) As Boolean
            ' Test length
            If NumericalPassword.Length <> 55 Then Return False

            ' Test whether the password is fully numeric
            Dim noDashPassword As String = NumericalPassword.Replace("-", "")
            If Not Regex.IsMatch(noDashPassword, "^\d+$") Then Return False

            ' Test whether the modulo of 11 for the first 5 characters in each group returns the 6th character.
            Dim passwordParts As String() = NumericalPassword.Split("-")
            For Each passwordPart In passwordParts
                Try
                    Dim numericPasswordPart As Integer = CInt(passwordPart.Substring(0, 5)),
                        checksum As Integer = CInt(passwordPart(5).ToString()),
                        remainder As Integer = numericPasswordPart Mod 11
                    If remainder <> checksum Then Return False
                Catch ex As Exception
                    Return False
                End Try
            Next

            Return True
        End Function

        Public Shared Function UnlockEncryptedVolumeWithNumericalPassword(PersistentVolumeId As String, NumericalPassword As String) As UInteger
            If Not ValidateNumericalPasswordFormat(NumericalPassword) Then Return Constants.FVE_E_INVALID_PASSWORD_FORMAT

            Try
                Dim encryptedVolumeInstance As ManagementObject = GetEncryptedVolumeManagementInstance(PersistentVolumeId)
                If encryptedVolumeInstance Is Nothing Then Throw New Exception()

                Dim unlockBaseObject As ManagementBaseObject = encryptedVolumeInstance.GetMethodParameters("UnlockWithNumericalPassword")
                unlockBaseObject("NumericalPassword") = NumericalPassword

                Dim unlockResults As ManagementBaseObject = encryptedVolumeInstance.InvokeMethod("UnlockWithNumericalPassword", unlockBaseObject, Nothing)
                If unlockResults Is Nothing Then Throw New Exception()

                Return unlockResults("ReturnValue")
            Catch ex As Exception
                Return Constants.E_FAIL
            End Try
        End Function

    End Class

End Namespace