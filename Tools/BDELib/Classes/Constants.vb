Namespace Classes

    Public Class Constants

        ''' <summary>
        ''' The operation completed successfully.
        ''' </summary>
        Public Const S_OK As Integer = 0
        ''' <summary>
        ''' Unspecified error.
        ''' </summary>
        Public Const E_FAIL As UInteger = 2147500037
        ''' <summary>
        ''' Access is denied.
        ''' </summary>
        Public Const E_ACCESS_DENIED As UInteger = 2147942405
        ''' <summary>
        ''' Invalid argument.
        ''' </summary>
        Public Const E_INVALIDARG As UInteger = 2147942487
        ''' <summary>
        ''' This drive is locked by BitLocker Drive Encryption. You must unlock this
        ''' drive from Control Panel.
        ''' </summary>
        Public Const FVE_E_LOCKED_VOLUME As UInteger = 2150694912
        ''' <summary>
        ''' This drive is not encrypted.
        ''' </summary>
        Public Const FVE_E_NOT_ENCRYPTED As UInteger = 2150694913
        ''' <summary>
        ''' BitLocker Drive Encryption is not enabled on this drive. Turn on BitLocker.
        ''' </summary>
        Public Const FVE_E_NOT_ACTIVATED As UInteger = 2150694920
        ''' <summary>
        ''' Automatic unlocking on the volume is disabled.
        ''' </summary>
        Public Const FVE_E_VOLUME_NOT_BOUND As UInteger = 2150694935
        ''' <summary>
        ''' The operation attempted cannot be performed on an operating system volume.
        ''' </summary>
        Public Const FVE_E_NOT_DATA_VOLUME As UInteger = 2150694937
        ''' <summary>
        ''' Cluster configurations are not supported by BitLocker Drive Encryption.
        ''' </summary>
        Public Const FVE_E_CLUSTERING_NOT_SUPPORTED As UInteger = 2150694942
        ''' <summary>
        ''' 
        ''' </summary>
        Public Const FVE_E_PROTECTION_DISABLED As UInteger = 2150694945
        ''' <summary>
        ''' The drive you are attempting to lock does not have any key protectors available for
        ''' encryption because BitLocker protection is currently suspended. Re-enable BitLocker to
        ''' lock this drive.
        ''' </summary>
        Public Const FVE_E_RECOVERY_KEY_REQUIRED As UInteger = 2150694946
        ''' <summary>
        ''' The drive cannot be unlocked with the key provided. Confirm that you have
        ''' provided the correct key and try again.
        ''' </summary>
        Public Const FVE_E_FAILED_AUTHENTICATION As UInteger = 2150694951
        ''' <summary>
        ''' The drive specified is not the operating system drive.
        ''' </summary>
        Public Const FVE_E_NOT_OS_VOLUME As UInteger = 2150694952
        ''' <summary>
        ''' BitLocker Drive Encryption cannot be turned off on the operating system drive until
        ''' the auto unlock feature has been disabled for the fixed data drives and removable data
        ''' drives associated with this computer.
        ''' </summary>
        Public Const FVE_E_AUTOUNLOCK_ENABLED As UInteger = 2150694953
        ''' <summary>
        ''' Group Policy settings require that a recovery password be specified before
        ''' encrypting the drive
        ''' </summary>
        Public Const FVE_E_POLICY_PASSWORD_REQUIRED As UInteger = 2150694956
        ''' <summary>
        ''' The drive encryption algorithm and key cannot be set on a previously encrypted drive.
        ''' To encrypt this drive with BitLocker Drive Encryption, remove the previous encryption
        ''' and then turn on BitLocker.
        ''' </summary>
        Public Const FVE_E_CANNOT_SET_FVEK_ENCRYPTED As UInteger = 2150694957
        ''' <summary>
        ''' BitLocker Drive Encryption cannot encrypt the specified drive because an
        ''' encryption key is not available. Add a key protector to encrypt this drive.
        ''' </summary>
        Public Const FVE_E_CANNOT_ENCRYPT_NO_KEY As UInteger = 2150694958
        ''' <summary>
        ''' Same as <see cref="FVE_E_CANNOT_ENCRYPT_NO_KEY"/> on Windows Vista systems.
        ''' </summary>
        Public Const ERROR_INVALID_OPERATION As UInteger = 4317
        ''' <summary>
        ''' The specified key protector was not found on the drive. Try another key protector.
        ''' </summary>
        Public Const FVE_E_PROTECTOR_NOT_FOUND As UInteger = 2150694963
        ''' <summary>
        ''' The format of the recovery password provided is invalid. BitLocker recovery passwords
        ''' are 48 digits. Verify that the recovery password is in the correct format and then
        ''' try again.
        ''' </summary>
        ''' <remarks>
        ''' Valid recovery keys have their groups succeed in calculating the following formula,
        ''' assuming that the digits in a group are x1, x2, x3, x4, x5, x6: x1x2x3x4x5 mod 11 == x6.
        ''' A group must also not surpass 720896.
        ''' Source: https://learn.microsoft.com/en-us/windows/win32/secprov/isnumericalpasswordvalid-win32-encryptablevolume
        ''' </remarks>
        Public Const FVE_E_INVALID_PASSWORD_FORMAT As UInteger = 2150694965


    End Class

End Namespace
