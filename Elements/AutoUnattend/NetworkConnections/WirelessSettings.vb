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

    Public Class WirelessValidator

        Public Shared Function ValidateWiFi(connection As WirelessSettings) As Boolean
            Dim valid As Boolean = True
            If connection.SSID = "" OrElse String.IsNullOrWhiteSpace(connection.SSID) Then
                valid = False
            End If
            Return valid
        End Function

    End Class

End Namespace
