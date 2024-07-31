Namespace Elements

    Public Class WirelessSettings

        Public Property SSID As String

        Public Property ConnectWithoutBroadcast As Boolean

        Public Property Authentication As WiFiAuthenticationMode

        Public Property Password As String

    End Class

    Public Enum WiFiAuthenticationMode As Integer
        Open = 0
        WPA2_PSK = 1
        WPA3_PSK = 2
    End Enum

End Namespace
