Module SearchEngineHelper

    Private SearchEngines As New List(Of SearchEngine) From {
        New SearchEngine("Google Search", "Google LLC", "https://google.com/search?q={0}&udm=14"),
        New SearchEngine("Bing", "Microsoft", "https://bing.com/search?q={0}"),
        New SearchEngine("DuckDuckGo", "", "https://duckduckgo.com/?q={0}&ia=web"),
        New SearchEngine("Startpage", "", "https://startpage.com/sp/search?q={0}"),
        New SearchEngine("Brave Search", "", "https://search.brave.com/search?q={0}&source=web")
    }

    Public Function GetAllSearchEngines() As List(Of SearchEngine)
        Return SearchEngines
    End Function

    Public Sub InvokeSearchQuery(SearchEngineName As String, SearchQuery As String)
        Dim selectedEngine As SearchEngine = SearchEngines.FirstOrDefault(Function(engine) engine.Name.ToLowerInvariant().Contains(SearchEngineName.ToLowerInvariant()))

        If selectedEngine IsNot Nothing Then
            Process.Start(String.Format(selectedEngine.SearchURI, SearchQuery))
        End If
    End Sub

End Module
