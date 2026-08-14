Namespace Classes

    ''' <summary>
    ''' The <see cref="ConversionStatus"/> class holds information about the conversion status of 
    ''' an encrypted BitLocker volume.
    ''' </summary>
    Public Class ConversionStatus

        ''' <summary>
        ''' The conversion status of an encrypted volume.
        ''' </summary>
        ''' <returns></returns>
        Public Property ConversionStatus As VolumeConversionStatus
        ''' <summary>
        ''' The percentage of the volume that is encrypted.
        ''' </summary>
        ''' <returns></returns>
        Public Property EncryptionPercentage As UInteger
        ''' <summary>
        ''' Encryption flags that were used during volume encryption
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Refer to Microsoft documentation for information on encryption flags.
        ''' </remarks>
        Public Property EncryptionFlags As UInteger
        ''' <summary>
        ''' The wiping status of an encrypted volume.
        ''' </summary>
        ''' <returns></returns>
        Public Property WipingStatus As VolumeWipingStatus
        ''' <summary>
        ''' The percentage of the volume that has been wiped.
        ''' </summary>
        ''' <returns></returns>
        Public Property WipingPercentage As UInteger

        ''' <summary>
        ''' Initializes a <see cref="ConversionStatus"/> object with default values.
        ''' </summary>
        Public Sub New()
            ConversionStatus = VolumeConversionStatus.Unknown
            EncryptionPercentage = 0
            EncryptionFlags = 0
            WipingStatus = VolumeWipingStatus.Unknown
            WipingPercentage = 0
        End Sub

        ''' <summary>
        ''' Initializes a <see cref="ConversionStatus"/> object with a provided conversion status.
        ''' </summary>
        ''' <param name="conversionStatus">The conversion status of a volume</param>
        Public Sub New(conversionStatus As VolumeConversionStatus)
            Me.ConversionStatus = conversionStatus
            EncryptionPercentage = 0
            EncryptionFlags = 0
            WipingStatus = VolumeWipingStatus.Unknown
            WipingPercentage = 0
        End Sub

        ''' <summary>
        ''' Initializes a <see cref="ConversionStatus"/> object with a provided conversion status,
        ''' encryption percentage, and encryption flags.
        ''' </summary>
        ''' <param name="conversionStatus">The conversion status of a volume</param>
        ''' <param name="encryptionPercentage">The encryption percentage of a volume</param>
        ''' <param name="encryptionFlags">The encryption flags of a volume</param>
        Public Sub New(conversionStatus As VolumeConversionStatus, encryptionPercentage As UInteger, encryptionFlags As UInteger)
            Me.ConversionStatus = conversionStatus
            Me.EncryptionPercentage = encryptionPercentage
            Me.EncryptionFlags = encryptionFlags
            WipingStatus = VolumeWipingStatus.Unknown
            WipingPercentage = 0
        End Sub

        ''' <summary>
        ''' Initializes a <see cref="ConversionStatus"/> object with a provided conversion status,
        ''' encryption percentage, encryption flags, wiping status, and wiping percentage.
        ''' </summary>
        ''' <param name="conversionStatus">The conversion status of a volume</param>
        ''' <param name="encryptionPercentage">The encryption percentage of a volume</param>
        ''' <param name="encryptionFlags">The encryption flags of a volume</param>
        ''' <param name="wipingStatus">The wiping status of a volume</param>
        ''' <param name="wipingPercentage">The wiping percentage of a volume</param>
        Public Sub New(conversionStatus As VolumeConversionStatus, encryptionPercentage As UInteger, encryptionFlags As UInteger, wipingStatus As VolumeWipingStatus, wipingPercentage As UInteger)
            Me.ConversionStatus = conversionStatus
            Me.EncryptionPercentage = encryptionPercentage
            Me.EncryptionFlags = encryptionFlags
            Me.WipingStatus = wipingStatus
            Me.WipingPercentage = wipingPercentage
        End Sub

    End Class

End Namespace
