Module SearchEngineHelper

    ''' <summary>
    ''' List of registered search engines
    ''' </summary>
    ''' <remarks>
    ''' Brave wanted to be silly and snuck the AI summaries on me. TURN THESE OFF!!!! Also, if you're stupid enough 
    ''' to trust AI results, here you go. Learn how to search and not how to prompt! Trust me... don't use AI search
    ''' engines.
    ''' </remarks>
    Private SearchEngines As New List(Of SearchEngine) From {
        New SearchEngine("Google Search (No AI)", "Google LLC", "https://google.com/search?q={0}&udm=14", SearchEngine.AIPermissionType.Mixed),
        New SearchEngine("Google Search (AI Mode)", "Google LLC", "https://google.com/search?q={0}&udm=50", SearchEngine.AIPermissionType.Enabled),
        New SearchEngine("Bing", "Microsoft", "https://bing.com/search?q={0}", SearchEngine.AIPermissionType.Enabled),
        New SearchEngine("DuckDuckGo", "", "https://duckduckgo.com/?q={0}&ia=web", SearchEngine.AIPermissionType.Mixed),
        New SearchEngine("DuckDuckGo (No AI)", "", "https://noai.duckduckgo.com/?q={0}&ia=web", SearchEngine.AIPermissionType.Disabled),
        New SearchEngine("DuckDuckGo (with AI)", "", "https://yesai.duckduckgo.com/?q={0}&ia=web", SearchEngine.AIPermissionType.Enabled),
        New SearchEngine("Startpage", "", "https://startpage.com/sp/search?q={0}", SearchEngine.AIPermissionType.Disabled),
        New SearchEngine("Brave Search", "", "https://search.brave.com/search?q={0}&source=web&summary=0", SearchEngine.AIPermissionType.Mixed),
        New SearchEngine("Brave Search (with summaries)", "", "https://search.brave.com/search?q={0}&source=web", SearchEngine.AIPermissionType.Mixed)
    }

    Public Function GetAllSearchEngines() As List(Of SearchEngine)
        Return SearchEngines
    End Function

    Public Sub InvokeSearchQuery(SearchEngineName As String, SearchQuery As String)
        Dim selectedEngine As SearchEngine = SearchEngines.FirstOrDefault(Function(engine) engine.Name.ToLowerInvariant().Contains(SearchEngineName.ToLowerInvariant()))

        If selectedEngine IsNot Nothing Then
            ' Exact queries can only be sent to Google Search and Brave Search. If we had picked either, then we replace those quotes
            ' with actual quotes passed with the search query. Other search engines, like DuckDuckGo, don't seem to cope
            ' with THE exact terms quite well, unless we don't pass the quotes.
            If selectedEngine.Equals(SearchEngines.First(Function(engine) engine.Name.ToLowerInvariant().Contains("google"))) OrElse
                selectedEngine.Equals(SearchEngines.First(Function(engine) engine.Name.ToLowerInvariant().Contains("brave"))) Then
                SearchQuery = SearchQuery.Replace(ControlChars.Quote, "%22")
            End If

            Process.Start(String.Format(selectedEngine.SearchURI, SearchQuery.Replace(" ", "%20")))
        End If
    End Sub

    Public Function GetSearchQueryUri(SearchQuery As String) As String
        Dim selectedEngine As SearchEngine = SearchEngines.FirstOrDefault(Function(engine) engine.AIPermission = SearchEngine.AIPermissionType.Mixed AndAlso engine.Name.IndexOf("Google Search", StringComparison.OrdinalIgnoreCase) > -1)
        If selectedEngine Is Nothing Then Return ""

        Return String.Format(selectedEngine.SearchURI, SearchQuery.Replace(Quote, "%22").Replace(" ", "%20"))
    End Function

End Module
