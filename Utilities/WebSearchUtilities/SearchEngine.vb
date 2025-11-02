Public Class SearchEngine

    ''' <summary>
    ''' The name of the search engine
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Name As String
    ''' <summary>
    ''' The company that develops the search engine
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Company As String
    ''' <summary>
    ''' The URL to use when performing searches using this engine
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SearchURI As String

    Public Sub New(name As String, company As String, searchURI As String)
        Me.Name = name
        Me.Company = company
        Me.SearchURI = searchURI
    End Sub

End Class
