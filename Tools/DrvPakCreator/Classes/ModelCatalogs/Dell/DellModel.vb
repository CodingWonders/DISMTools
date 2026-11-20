Namespace Classes.ModelCatalogs.Dell

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellModel

        Private _systemId As String
        Private _name As String
        Private _releaseDate As String
        Private _generation As String
        Private _display As DellDisplay

        <XmlAttribute("systemID")> _
        Public Property SystemId() As String
            Get
                Return _systemId
            End Get
            Set(ByVal value As String)
                _systemId = value
            End Set
        End Property

        <XmlAttribute("name")> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        <XmlAttribute("rtsDate")> _
        Public Property ReleaseDate() As String
            Get
                Return _releaseDate
            End Get
            Set(ByVal value As String)
                _releaseDate = value
            End Set
        End Property

        Public ReadOnly Property DateReleased() As Date
            Get
                Dim parsedDate As Date
                If Date.TryParse(ReleaseDate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, parsedDate) Then
                    Return parsedDate
                End If
                Return New Date(1970, 1, 1, 0, 0, 0)
            End Get
        End Property

        <XmlAttribute("generation")> _
        Public Property Generation() As String
            Get
                Return _generation
            End Get
            Set(ByVal value As String)
                _generation = value
            End Set
        End Property

        <XmlElement("Display")> _
        Public Property Display() As DellDisplay
            Get
                Return _display
            End Get
            Set(ByVal value As DellDisplay)
                _display = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("ID {0} -- {1}", SystemId, Name)
        End Function

    End Class

End Namespace