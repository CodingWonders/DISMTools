Namespace Classes.ModelCatalogs.Dell

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellHashCollection

        Private _hashes As List(Of DellHash)

        <XmlElement("Hash")> _
        Public Property Hashes() As List(Of DellHash)
            Get
                Return _hashes
            End Get
            Set(ByVal value As List(Of DellHash))
                _hashes = value
            End Set
        End Property

        Public ReadOnly Property MD5() As String
            Get
                Return GetHash("MD5")
            End Get
        End Property

        Public ReadOnly Property SHA1() As String
            Get
                Return GetHash("SHA1")
            End Get
        End Property

        Public ReadOnly Property SHA256() As String
            Get
                Return GetHash("SHA256")
            End Get
        End Property

        Public Sub New()
            Hashes = New List(Of DellHash)
        End Sub

        Public Function GetHash(ByVal algorithm As String) As String
            For Each currentHash As DellHash In Hashes
                If String.Equals(currentHash.Algorithm, algorithm, StringComparison.OrdinalIgnoreCase) Then
                    Return currentHash.Value
                End If
            Next
            Return String.Empty
        End Function

    End Class

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellHash

        Private _algorithm As String
        Private _value As String

        <XmlAttribute("algorithm")> _
        Public Property Algorithm() As String
            Get
                Return _algorithm
            End Get
            Set(ByVal value As String)
                _algorithm = value
            End Set
        End Property

        <XmlText()> _
        Public Property Value() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("{0} -- {1}", Algorithm, Value)
        End Function

    End Class

End Namespace