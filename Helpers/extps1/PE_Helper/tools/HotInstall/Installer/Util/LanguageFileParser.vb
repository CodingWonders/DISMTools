Imports IniParser
Imports IniParser.Model
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text

Module LanguageFileParser

    Dim LanguageData As IniData

    Sub LoadLanguageFile(LanguageFile As String)
        If LanguageFile = "" OrElse Not File.Exists(LanguageFile) Then
            Throw New Exception("Either no language file has been specified or it does not exist")
        End If
        Try
            Dim parser = New FileIniDataParser()
            Using Reader As New StreamReader(LanguageFile, Encoding.UTF8)
                LanguageData = parser.ReadData(Reader)
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Function GetValueFromLanguageData(ItemKey As String) As String
        If LanguageData IsNot Nothing Then
            Try
                Dim KeySections() As String = ItemKey.Split(".")
                Return LanguageData(KeySections(0))(KeySections(1)).Replace(Quote, "").Replace("{quot;}", Quote).Replace("{crlf;}", CrLf)
            Catch ex As Exception
                Return ItemKey
            End Try
        Else
            Return ItemKey
        End If
        Return Nothing
    End Function

End Module
