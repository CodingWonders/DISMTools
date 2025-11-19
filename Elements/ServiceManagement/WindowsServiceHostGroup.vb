Public Class WindowsServiceHostGroup

    Public Property Name As String
    Public Property Services As List(Of WindowsService)

    Public Sub New(name As String, services As List(Of WindowsService))
        Me.Name = name
        Me.Services = services
    End Sub

End Class
