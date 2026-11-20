Namespace Classes.ModelCatalogs.Dell

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellLocalizedText

        Private _display As DellDisplay

        <XmlElement("Display")> _
        Public Property Display() As DellDisplay
            Get
                Return _display
            End Get
            Set(ByVal value As DellDisplay)
                _display = value
            End Set
        End Property

        Public ReadOnly Property Text() As String
            Get
                If Display Is Nothing Then
                    Return String.Empty
                End If
                Return Display.ToString()
            End Get
        End Property

    End Class

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellDisplay

        Private _language As String
        Private _text As String

        <XmlAttribute("lang")> _
        Public Property Language() As String
            Get
                Return _language
            End Get
            Set(ByVal value As String)
                _language = value
            End Set
        End Property

        <XmlText()> _
        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal value As String)
                _text = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            If Text Is Nothing Then
                Return String.Empty
            End If
            Return Text.Trim()
        End Function

    End Class

End Namespace