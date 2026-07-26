Imports System.IO
Imports System.Net
Imports System.Threading
Imports System.Threading.Tasks

''' <summary>
''' The tour server class provides means to start a tiny HTTP server for pages hosted by DISMTools.
''' </summary>
''' <remarks>
''' A HTTP server like this one (or IIS, or Apache) is needed because YouTube videos no longer work
''' in DISMTools-hosted pages when their respective pages are invoked directly (you can distinguish by
''' looking at "file:///" in the browser's address bar). This fixes any YouTube video player and
''' CORS issues that may arise.
''' </remarks>
Public Class DTHttpServer

    Private ReadOnly _rootDir As String
    Private ReadOnly _tcpPort As Integer
    Private _listener As HttpListener = New HttpListener()
    Private _cts As CancellationTokenSource

    Public Sub New(RootDirectory As String, TCPPort As Integer)
        _rootDir = RootDirectory
        _tcpPort = TCPPort
    End Sub

    Public Sub StartServer()
        ' Exit if the listener is already alive
        If IsListenerAlive() Then Exit Sub

        If Not Directory.Exists(_rootDir) Then
            MessageBox.Show(LocalizationService.ForSection("HttpServer.Messages").Format("Root.Dir.Exist.Message", _rootDir), LocalizationService.ForSection("HttpServer.Messages")("TourServer.Label"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try
            ' Reinitialize the listener so we can re-add the prefixes
            _listener = New HttpListener()
            _listener.Prefixes.Add(String.Format("http://+:{0}/", _tcpPort))
            _listener.IgnoreWriteExceptions = True
            _listener.Start()

            _cts = New CancellationTokenSource()
            Dim unused = ListenLoop(_cts.Token)
        Catch ex As Exception
            ' ignore exceptions
        End Try
    End Sub

    Public Function IsListenerAlive() As Boolean
        Return _listener.IsListening
    End Function

    Public Function GetTcpPort() As Integer
        Return _tcpPort
    End Function

    Public Sub StopServer()
        ' Exit if the listener is dead
        If Not IsListenerAlive() Then Exit Sub

        Try
            _cts.Cancel()
            _listener.Stop()
            _listener.Close()
        Catch ex As Exception
            ' ignore exceptions
        End Try
    End Sub

    Private Async Function ListenLoop(token As CancellationToken) As Task
        While Not token.IsCancellationRequested
            Dim context As HttpListenerContext

            Try
                context = Await _listener.GetContextAsync()
            Catch ex As Exception
                Exit While
            End Try

            Await Task.Run(Sub()
                               HandleRequest(context)
                           End Sub)
        End While
    End Function

    Private Sub HandleRequest(ctx As HttpListenerContext)
        Dim localPath As String = ctx.Request.Url.LocalPath.TrimStart("/")
        Dim fullPath As String = Path.Combine(_rootDir, localPath).Replace("\", "/")

        If Directory.Exists(fullPath) Then fullPath = Path.Combine(fullPath, "index.html")

        If Not File.Exists(fullPath) Then
            ctx.Response.StatusCode = 404
            Using writer As StreamWriter = New StreamWriter(ctx.Response.OutputStream)
                writer.Write("404 Not Found")
                Exit Sub
            End Using
        End If

        Dim buffer As Byte() = File.ReadAllBytes(fullPath)
        ctx.Response.StatusCode = 200
        Dim mimetype As String = GetMime(Path.GetExtension(fullPath))
        ctx.Response.ContentType = mimetype

        Debug.WriteLine(String.Format("MIME Type for {0} : {1}", fullPath, mimetype))

        Try
            ctx.Response.OutputStream.Write(buffer, 0, buffer.Length)
        Catch ex As Exception
            ' ignore potential errors
        End Try
        ctx.Response.OutputStream.Close()
    End Sub

    Private Function GetMime(extension As String)
        Select Case extension
            Case ".html"
                Return "text/html"
            Case ".css"
                Return "text/css"
            Case ".js"
                Return "text/javascript"
            Case ".png"
                Return "image/png"
            Case ".jpg"
                Return "image/jpeg"
            Case ".gif"
                Return "image/gif"
            Case ".svg"
                Return "image/svg+html"
            Case ".woff"
                Return "font/woff"
            Case ".ico"
                Return "image/x-icon"
            Case Else
                Return "application/octet-stream"
        End Select
    End Function

End Class
