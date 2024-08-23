Namespace Elements

    Public Class WirelessSettings

        ''' <summary>
        ''' The wireless network name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SSID As String

        ''' <summary>
        ''' Determines whether to connect without broadcasting the wireless network
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConnectWithoutBroadcast As Boolean

        ''' <summary>
        ''' The authentication mode for the network connection
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Possible values: Open, WPA2-PSK, WPA3-SAE (Simultaneous Authentication of Equals)</remarks>
        Public Property Authentication As WiFiAuthenticationMode

        ''' <summary>
        ''' The password of the wireless network
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Password As String

    End Class

    Public Enum WiFiAuthenticationMode As Integer
        Open = 0
        WPA2_PSK = 1
        WPA3_SAE = 2
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
