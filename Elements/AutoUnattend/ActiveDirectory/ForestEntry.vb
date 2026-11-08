Public Class ForestEntry

    Public DnsPath As String
    Public NetBIOSName As String

    Public Sub New(dns As String, netbios As String)
        DnsPath = dns
        NetBIOSName = netbios
    End Sub

End Class
