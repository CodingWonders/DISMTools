Namespace Classes

    Public Class ConversionStatus

        Public Property ConversionStatus As VolumeConversionStatus
        Public Property EncryptionPercentage As UInteger
        Public Property EncryptionFlags As UInteger
        Public Property WipingStatus As VolumeWipingStatus
        Public Property WipingPercentage As UInteger

        Public Sub New()
            ConversionStatus = VolumeConversionStatus.Unknown
            EncryptionPercentage = 0
            EncryptionFlags = 0
            WipingStatus = VolumeWipingStatus.Unknown
            WipingPercentage = 0
        End Sub

        Public Sub New(conversionStatus As VolumeConversionStatus)
            Me.ConversionStatus = conversionStatus
            EncryptionPercentage = 0
            EncryptionFlags = 0
            WipingStatus = VolumeWipingStatus.Unknown
            WipingPercentage = 0
        End Sub

        Public Sub New(conversionStatus As VolumeConversionStatus, encryptionPercentage As UInteger, encryptionFlags As UInteger)
            Me.ConversionStatus = conversionStatus
            Me.EncryptionPercentage = encryptionPercentage
            Me.EncryptionFlags = encryptionFlags
            WipingStatus = VolumeWipingStatus.Unknown
            WipingPercentage = 0
        End Sub

        Public Sub New(conversionStatus As VolumeConversionStatus, encryptionPercentage As UInteger, encryptionFlags As UInteger, wipingStatus As VolumeWipingStatus, wipingPercentage As UInteger)
            Me.ConversionStatus = conversionStatus
            Me.EncryptionPercentage = encryptionPercentage
            Me.EncryptionFlags = encryptionFlags
            Me.WipingStatus = wipingStatus
            Me.WipingPercentage = wipingPercentage
        End Sub

    End Class

End Namespace
