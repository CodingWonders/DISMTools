Imports System.IO

Module BuildGetter

    Dim AssemblyPath As String = Path.Combine(My.Application.Info.DirectoryPath, My.Application.Info.AssemblyName & ".exe")

    Public Function RetrieveLinkerTimestamp() As DateTime
        Const PeHeaderOffset As Integer = 60
        Const LinkerTimestampOffset As Integer = 8
        Dim b(2047) As Byte
        Dim s As Stream = Nothing
        Try
            s = New FileStream(AssemblyPath, FileMode.Open, FileAccess.Read)
            s.Read(b, 0, 2048)
        Finally
            If Not s Is Nothing Then s.Close()
        End Try
        Dim i As Integer = BitConverter.ToInt32(b, PeHeaderOffset)
        Dim SecondsSince1970 As Integer = BitConverter.ToInt32(b, i + LinkerTimestampOffset)
        Dim dt As New DateTime(1970, 1, 1, 0, 0, 0)
        dt = dt.AddSeconds(SecondsSince1970)
        Dim tz As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")
        dt = TimeZoneInfo.ConvertTimeFromUtc(dt, tz)
        Return dt
    End Function

End Module
