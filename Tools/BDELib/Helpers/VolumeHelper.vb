Imports BDELib.Classes
Imports System.Text.RegularExpressions

Namespace Helpers

    ''' <summary>
    ''' The <see cref="VolumeHelper"/> class allows the library to perform operations
    ''' with BitLocker volumes.
    ''' </summary>
    Public Class VolumeHelper

        ''' <summary>
        ''' Gets a list of encrypted BitLocker volumes.
        ''' </summary>
        ''' <returns>A list of encrypted BitLocker volumes</returns>
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

        ''' <summary>
        ''' Gets a WMI class instance of an encrypted volume using a volume's persistent volume
        ''' identifier.
        ''' </summary>
        ''' <param name="PersistentVolumeId">The persistent volume identifier associated to the encrypted volume</param>
        ''' <returns>A management object</returns>
        Private Shared Function GetEncryptedVolumeManagementInstance(PersistentVolumeId As String) As ManagementObject
            Dim managementResults As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT * FROM Win32_EncryptableVolume WHERE PersistentVolumeId = {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(PersistentVolumeId)), "root\cimv2\Security\MicrosoftVolumeEncryption")
            Return If(managementResults Is Nothing, Nothing, managementResults(0))
        End Function

        ''' <summary>
        ''' Gets a list of available key protectors in an encrypted BitLocker volume.
        ''' </summary>
        ''' <param name="PersistentVolumeId">The persistent volume identifier associated to the encrypted volume</param>
        ''' <param name="ProtectorType">The type of key protector to get</param>
        ''' <returns>A list of available key protectors in an encrypted BitLocker volume</returns>
        ''' <remarks>
        ''' In this case, an unknown protector type passed to this method equates to getting
        ''' all types of key protectors of a volume.
        ''' </remarks>
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

        ''' <summary>
        ''' Gets the lock status of an encrypted BitLocker volume.
        ''' </summary>
        ''' <param name="PersistentVolumeId">The persistent volume identifier associated to the encrypted volume</param>
        ''' <returns>The lock status of an encrypted BitLocker volume</returns>
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

        ''' <summary>
        ''' Locks an unlocked BitLocker encrypted volume.
        ''' </summary>
        ''' <param name="PersistentVolumeId">The persistent volume identifier associated to the encrypted volume</param>
        ''' <returns>The exit code of the operation</returns>
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

        ''' <summary>
        ''' Determines whether a 48-digit numerical password has been written correctly.
        ''' </summary>
        ''' <param name="NumericalPassword">The numerical password</param>
        ''' <returns>Whether the password passes the format checks</returns>
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

        ''' <summary>
        ''' Unlocks a locked BitLocker encrypted volume to allow a user to access the data
        ''' inside it.
        ''' </summary>
        ''' <param name="PersistentVolumeId">The persistent volume identifier associated to the encrypted volume</param>
        ''' <param name="NumericalPassword">The numerical password</param>
        ''' <returns>The exit code of the operation</returns>
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

        ''' <summary>
        ''' Gets the conversion status of an encrypted BitLocker volume.
        ''' </summary>
        ''' <param name="PersistentVolumeId">The persistent volume identifier associated to the encrypted volume</param>
        ''' <param name="PrecisionFactor">A precision factor for encryption and wipe percentages</param>
        ''' <returns>The conversion status of an encrypted volume</returns>
        ''' <remarks>
        ''' If not specified, <paramref name="PrecisionFactor"/> will default to the highest possible value, 4. The
        ''' higher the precision factor, the more zeros a divider will need to be able to display percentage values in a
        ''' 0-100 range.
        ''' <list type="bullet">
        '''     <item>
        '''         <term>For a precision factor of 1</term>
        '''         <description>Use 10 as the divider</description>
        '''     </item>
        '''     <item>
        '''         <term>For a precision factor of 2</term>
        '''         <description>Use 100 as the divider</description>
        '''     </item>
        '''     <item>
        '''         <term>For a precision factor of 3</term>
        '''         <description>Use 1000 as the divider</description>
        '''     </item>
        '''     <item>
        '''         <term>For a precision factor of 4</term>
        '''         <description>Use 10000 as the divider</description>
        '''     </item>
        ''' </list>
        ''' Values lower than 1 will be reset to 1, while values higher than 4 will be reset to 4.
        ''' </remarks>
        Public Shared Function GetVolumeConversionStatus(PersistentVolumeId As String, Optional PrecisionFactor As Integer = 4) As ConversionStatus
            Dim obtainedConversionStatus As ConversionStatus = Nothing

            If PrecisionFactor < 1 Then PrecisionFactor = 1
            If PrecisionFactor > 4 Then PrecisionFactor = 4

            Try
                Dim encryptedVolumeInstance As ManagementObject = GetEncryptedVolumeManagementInstance(PersistentVolumeId)
                If encryptedVolumeInstance Is Nothing Then Throw New Exception()
                Dim getterObject As ManagementBaseObject = encryptedVolumeInstance.GetMethodParameters("GetConversionStatus")
                getterObject("PrecisionFactor") = PrecisionFactor
                Dim getterResults As ManagementBaseObject = encryptedVolumeInstance.InvokeMethod("GetConversionStatus", getterObject, Nothing)
                If getterResults Is Nothing OrElse getterResults("ReturnValue") <> Constants.S_OK Then Throw New Exception()

                obtainedConversionStatus = New ConversionStatus(getterResults("ConversionStatus"), getterResults("EncryptionPercentage"), getterResults("EncryptionFlags"), getterResults("WipingStatus"), getterResults("WipingPercentage"))
            Catch ex As Exception
                Return Nothing
            End Try

            Return obtainedConversionStatus
        End Function

        ''' <summary>
        ''' Determines whether a volume contains auto-unlock keys.
        ''' </summary>
        ''' <param name="PersistentVolumeId">The persistent volume identifier associated to the encrypted volume</param>
        ''' <returns>Whether the volume contains auto-unlock keys</returns>
        Private Shared Function IsVolumeAutoUnlockable(PersistentVolumeId As String) As Boolean
            Try
                Dim encryptedVolumeInstance As ManagementObject = GetEncryptedVolumeManagementInstance(PersistentVolumeId)
                If encryptedVolumeInstance Is Nothing Then Throw New Exception()

                Dim autoUnlockResults As ManagementBaseObject = encryptedVolumeInstance.InvokeMethod("IsAutoUnlockEnabled", encryptedVolumeInstance.GetMethodParameters("IsAutoUnlockEnabled"), Nothing)
                If autoUnlockResults Is Nothing OrElse autoUnlockResults("ReturnValue") <> Constants.S_OK Then Throw New Exception()

                Return autoUnlockResults("IsAutoUnlockEnabled")
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' On a data volume, disables auto-unlock capabilities. On a system volume, clears
        ''' all auto-unlock keys.
        ''' </summary>
        ''' <param name="PersistentVolumeId">The persistent volume identifier associated to the encrypted volume</param>
        ''' <returns>The exit code of the operation</returns>
        Private Shared Function ClearVolumeAutoUnlockKeys(PersistentVolumeId As String) As UInteger
            Try
                Dim encryptedVolumeInstance As ManagementObject = GetEncryptedVolumeManagementInstance(PersistentVolumeId)
                If encryptedVolumeInstance Is Nothing Then Throw New Exception()

                ' If we're decrypting the OS volume, we're going to clear all keys; otherwise,
                ' we just disable autounlock
                Dim methodName As String = ""
                Dim encryptedVolume As EncryptableVolume = GetEncryptedVolumes().FirstOrDefault(Function(encVol) encVol.PersistentVolumeID.Equals(PersistentVolumeId))
                If encryptedVolume Is Nothing Then Throw New Exception()

                methodName = If(encryptedVolume.DriveLetter = Environment.GetEnvironmentVariable("SYSTEMDRIVE"), "ClearAllAutoUnlockKeys", "DisableAutoUnlock")

                Dim clearResults As ManagementBaseObject = encryptedVolumeInstance.InvokeMethod(methodName, encryptedVolumeInstance.GetMethodParameters(methodName), Nothing)
                If clearResults Is Nothing Then Throw New Exception()

                Return clearResults("ReturnValue")
            Catch ex As Exception
                Return Constants.E_FAIL
            End Try
        End Function

        ''' <summary>
        ''' Initiates decryption for an encrypted volume.
        ''' </summary>
        ''' <param name="PersistentVolumeId">The persistent volume identifier associated to the encrypted volume</param>
        ''' <param name="DecryptionProgressReporter">A progress reporter callback for graphical user interface threads</param>
        ''' <returns>The exit code of the operation</returns>
        Public Shared Async Function StartVolumeDecryption(PersistentVolumeId As String, Optional DecryptionProgressReporter As Action(Of ConversionStatus) = Nothing) As Task(Of UInteger)
            Try
                ' If auto-unlock is enabled on the volume, decryption will fail. Clear all keys
                ' before proceeding.
                If IsVolumeAutoUnlockable(PersistentVolumeId) Then
                    If ClearVolumeAutoUnlockKeys(PersistentVolumeId) <> Constants.S_OK Then Throw New Exception()
                End If

                Dim encryptedVolumeInstance As ManagementObject = GetEncryptedVolumeManagementInstance(PersistentVolumeId)
                If encryptedVolumeInstance Is Nothing Then Throw New Exception()
                Dim decryptionResults As ManagementBaseObject = encryptedVolumeInstance.InvokeMethod("Decrypt", encryptedVolumeInstance.GetMethodParameters("Decrypt"), Nothing)
                If decryptionResults Is Nothing Then Throw New Exception()
                If decryptionResults IsNot Nothing AndAlso decryptionResults("ReturnValue") <> Constants.S_OK Then Return decryptionResults("ReturnValue")

                ' Now we report if we have a reporter.
                If DecryptionProgressReporter IsNot Nothing Then
                    Do
                        Dim convStatus As ConversionStatus = GetVolumeConversionStatus(PersistentVolumeId)
                        DecryptionProgressReporter.Invoke(convStatus)
                        If convStatus Is Nothing OrElse convStatus.ConversionStatus = VolumeConversionStatus.FullyDecrypted Then Exit Do
                        Await Task.Delay(50)
                    Loop
                End If

                Return Constants.S_OK
            Catch ex As Exception
                Return Constants.E_FAIL
            End Try
        End Function

    End Class

End Namespace