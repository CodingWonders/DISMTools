#If VBC_VER >= 11.0 Then
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Threading.Tasks
Imports System.Text.Json
Imports System.Text.Json.Nodes
#End If

Public Class GitHubClient

#If VBC_VER >= 11.0 Then
    Private ReadOnly _client As HttpClient

    Public Sub New(ByVal Token As String)
        _client = New HttpClient()
        _client.DefaultRequestHeaders.UserAgent.ParseAdd("StarterScriptEditor-GitHub-Uploader-Client-CWS")
        _client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", Token)
        _client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/vnd.github+json"))
    End Sub

    Private Async Function SendAsync(ByVal Method As HttpMethod, ByVal Url As String, Optional ByVal Body As JsonObject = Nothing) As Task(Of String)
        Dim Request As New HttpRequestMessage(Method, Url)
        If Body IsNot Nothing Then
            Request.Content = New StringContent(Body.ToString(), Encoding.UTF8, "application/json")
        End If

        Dim Response As HttpResponseMessage = Await _client.SendAsync(Request)
        Dim ResponseBody As String = Await Response.Content.ReadAsStringAsync()
        Debug.WriteLine(Url)
        Response.EnsureSuccessStatusCode()
        Return ResponseBody
    End Function

    Public Async Function GetAuthenticatedUserNameAsync() As Task(Of String)
        Dim JsonBody As String = Await SendAsync(HttpMethod.Get, "https://api.github.com/user")
        Dim JsonObj As JsonObject = JsonObject.Parse(JsonBody)
        Return JsonObj("login")
    End Function

    ''' <summary>
    ''' Checks whether the given (or authenticated) user already has a fork of Owner/Repo.
    ''' Looks up {Username}/{Repo} directly and confirms it's actually a fork of Owner/Repo
    ''' (rather than just an unrelated repo the user happens to have with the same name).
    ''' </summary>
    Public Async Function ForkExistsAsync(ByVal Owner As String, ByVal Repo As String, Optional ByVal Username As String = Nothing) As Task(Of Boolean)
        If Username Is Nothing Then Username = Await GetAuthenticatedUserNameAsync()

        Try
            Dim Url As String = String.Format("https://api.github.com/repos/{0}/{1}", Username, Repo)
            Dim Json As String = Await SendAsync(HttpMethod.Get, Url)
            Dim Obj As JsonNode = JsonNode.Parse(Json)

            If Obj("fork") Is Nothing OrElse Not CBool(Obj("fork").ToString()) Then Return False

            Dim ParentFullName As String = Obj("parent")("full_name").ToString()
            Return String.Equals(ParentFullName, Owner & "/" & Repo, StringComparison.OrdinalIgnoreCase)
        Catch ex As HttpRequestException
            ' 404 (or other failure) means no repo at Username/Repo, so no fork exists.
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Forks Owner/Repo if the authenticated user doesn't already have a fork of it.
    ''' Returns True if a new fork was created, False if one already existed (no-op).
    ''' </summary>
    Public Async Function ForkRepositoryAsync(ByVal Owner As String, ByVal Repo As String) As Task(Of Boolean)
        If Await ForkExistsAsync(Owner, Repo) Then
            Return False
        End If

        Dim Url As String = String.Format("https://api.github.com/repos/{0}/{1}/forks", Owner, Repo)
        Await SendAsync(HttpMethod.Post, Url)
        Return True
    End Function

    Public Async Function GetBranchShaAsync(ByVal Owner As String, ByVal Repo As String, ByVal Branch As String) As Task(Of String)
        Dim Url As String = String.Format("https://api.github.com/repos/{0}/{1}/git/ref/heads/{2}", Owner, Repo, Branch)
        Dim Json As String = Await SendAsync(HttpMethod.Get, Url)
        Dim Obj As JsonObject = JsonObject.Parse(Json)
        Return Obj("object")("sha")
    End Function

    Public Async Function CreateBranchAsync(ByVal Owner As String, ByVal Repo As String, ByVal BranchName As String, ByVal BaseSha As String) As Task
        Dim Url As String = String.Format("https://api.github.com/repos/{0}/{1}/git/refs", Owner, Repo)
        Dim Body As New JsonObject()

        Body.Add("ref", "refs/heads/" & BranchName)
        Body.Add("sha", BaseSha)

        Await SendAsync(HttpMethod.Post, Url, Body)

    End Function

    Public Async Function GetFileShaAsync(ByVal Owner As String, ByVal Repo As String, ByVal Path As String, ByVal Branch As String) As Task(Of String)
        Try
            Dim Url As String = String.Format("https://api.github.com/repos/{0}/{1}/contents/{2}?ref={3}", Owner, Repo, Path, Branch)
            Dim Json As String = Await SendAsync(HttpMethod.Get, Url)
            Dim Obj As JsonValue = JsonValue.Parse(Json)
            Return Obj("sha")
        Catch ex As HttpRequestException
            Return Nothing
        End Try
    End Function

    Public Async Function CreateOrUpdateFileAsync(ByVal Owner As String, ByVal Repo As String, ByVal FilePath As String, ByVal FileContent As String, ByVal CommitMessage As String, ByVal Branch As String) As Task
        Dim ExistingSha As String = Await GetFileShaAsync(Owner, Repo, FilePath, Branch)

        Dim Url As String = String.Format("https://api.github.com/repos/{0}/{1}/contents/{2}", Owner, Repo, FilePath)

        Dim Base64Content As String = Convert.ToBase64String(Encoding.UTF8.GetBytes(FileContent))

        Dim Body As New JsonObject()

        Body.Add("message", CommitMessage)
        Body.Add("content", Base64Content)
        Body.Add("branch", Branch)

        If ExistingSha IsNot Nothing Then
            Body.Add("sha", ExistingSha)
        End If

        Await SendAsync(HttpMethod.Put, Url, Body)
    End Function

    Public Async Function CreatePullRequestAsync(ByVal Owner As String, ByVal Repo As String, ByVal Title As String, ByVal Head As String, ByVal BaseBranch As String, ByVal Description As String) As Task(Of String)
        Dim Url As String = String.Format("https://api.github.com/repos/{0}/{1}/pulls", Owner, Repo)
        Dim Body As New JsonObject()

        Body.Add("title", Title)
        Body.Add("head", Head)
        Body.Add("base", BaseBranch)
        Body.Add("body", Description)

        Dim Json As String = Await SendAsync(HttpMethod.Post, Url, Body)
        Dim Obj As JsonObject = JsonObject.Parse(Json)

        Return Obj("html_url")
    End Function

#End If

End Class