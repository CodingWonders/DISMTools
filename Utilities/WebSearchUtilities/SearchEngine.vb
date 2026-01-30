Public Class SearchEngine

    ''' <summary>
    ''' The type of AI permission for search engines
    ''' </summary>
    ''' <remarks></remarks>
    Enum AIPermissionType As Integer
        ''' <summary>
        ''' AI is not permitted on a search engine
        ''' </summary>
        ''' <remarks></remarks>
        Disabled = 0
        ''' <summary>
        ''' AI is allowed on a search engine, but can be turned off with engine settings 
        ''' should the user not need it (eg, service settings or HTTP GET parameters)
        ''' </summary>
        ''' <remarks></remarks>
        Mixed = 1
        ''' <summary>
        ''' AI is allowed on a search engine and/or there are no available settings to 
        ''' turn it off.
        ''' </summary>
        ''' <remarks></remarks>
        Enabled = 2
    End Enum

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
    ''' <summary>
    ''' The type of permission for Artificial Intelligence (AI) results and/or overviews
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AIPermission As AIPermissionType

    Public Sub New(name As String, company As String, searchURI As String, aiPermissionMode As AIPermissionType)
        Me.Name = name
        Me.Company = company
        Me.SearchURI = searchURI
        Me.AIPermission = aiPermissionMode
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim engineToCompare As SearchEngine = CType(obj, SearchEngine)
        Return Name.Equals(engineToCompare.Name, StringComparison.InvariantCultureIgnoreCase) AndAlso
            Company.Equals(engineToCompare.Company, StringComparison.InvariantCultureIgnoreCase) AndAlso
            SearchURI.Equals(engineToCompare.SearchURI, StringComparison.InvariantCultureIgnoreCase) AndAlso
            AIPermission.Equals(engineToCompare.AIPermission)
    End Function

End Class
