Imports IniParser
Imports IniParser.Model
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text
Imports System.Windows.Forms
Imports System.Collections.Generic

Module LanguageFileParser

    Private Const DefaultLanguageCode As String = "en-US"

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
            Throw
        End Try
    End Sub

    Public Function ResolveInstallerLanguageCode() As String
        Dim availableLanguages As Dictionary(Of String, String) = GetAvailableInstallerLanguages()
        Dim explicitLanguage As String = ResolveLanguageFromCommandLine(availableLanguages)
        If explicitLanguage <> "" Then Return explicitLanguage

        If availableLanguages.ContainsKey(DefaultLanguageCode) Then Return DefaultLanguageCode
        For Each languageCode As String In availableLanguages.Keys
            Return languageCode
        Next

        Return DefaultLanguageCode
    End Function

    Public Function GetInstallerLanguageFilePath(LanguageCode As String) As String
        Dim availableLanguages As Dictionary(Of String, String) = GetAvailableInstallerLanguages()
        Dim normalizedCode As String = NormalizeLanguageCode(LanguageCode, availableLanguages)

        If normalizedCode = "" AndAlso availableLanguages.ContainsKey(DefaultLanguageCode) Then
            normalizedCode = DefaultLanguageCode
        End If

        If normalizedCode <> "" AndAlso availableLanguages.ContainsKey(normalizedCode) Then
            Return availableLanguages(normalizedCode)
        End If

        Return ""
    End Function

    Private Function GetAvailableInstallerLanguages() As Dictionary(Of String, String)
        Dim availableLanguages As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        Dim languageDirectory As String = Path.Combine(Application.StartupPath, "Languages")
        If Not Directory.Exists(languageDirectory) Then Return availableLanguages

        Dim languageFiles As String() = Directory.GetFiles(languageDirectory, "*.ini", SearchOption.TopDirectoryOnly)
        Array.Sort(languageFiles, StringComparer.OrdinalIgnoreCase)

        For Each languageFile As String In languageFiles
            Try
                Dim parser = New FileIniDataParser()
                Dim languageFileData As IniData
                Using reader As New StreamReader(languageFile, Encoding.UTF8)
                    languageFileData = parser.ReadData(reader)
                End Using

                Dim languageCode As String = ""
                Try
                    languageCode = languageFileData("LanguageFileInformation")("LanguageCode").Replace(Quote, "").Trim()
                Catch
                End Try

                If languageCode <> "" AndAlso Not availableLanguages.ContainsKey(languageCode) Then
                    availableLanguages.Add(languageCode, languageFile)
                End If
            Catch
            End Try
        Next

        Return availableLanguages
    End Function

    Private Function ResolveLanguageFromCommandLine(availableLanguages As Dictionary(Of String, String)) As String
        Dim args() As String = Environment.GetCommandLineArgs()
        For index As Integer = 0 To args.Length - 1
            Dim argument As String = args(index)
            If String.IsNullOrWhiteSpace(argument) Then Continue For

            Dim value As String = ""
            If argument.StartsWith("/lang:", StringComparison.OrdinalIgnoreCase) OrElse argument.StartsWith("/lang=", StringComparison.OrdinalIgnoreCase) Then
                value = argument.Substring(6)
            ElseIf argument.StartsWith("--lang=", StringComparison.OrdinalIgnoreCase) Then
                value = argument.Substring(7)
            ElseIf argument.StartsWith("--language=", StringComparison.OrdinalIgnoreCase) Then
                value = argument.Substring(11)
            ElseIf (argument.Equals("/lang", StringComparison.OrdinalIgnoreCase) OrElse argument.Equals("--lang", StringComparison.OrdinalIgnoreCase) OrElse argument.Equals("--language", StringComparison.OrdinalIgnoreCase)) AndAlso index + 1 < args.Length Then
                value = args(index + 1)
            End If

            Dim normalized As String = NormalizeLanguageCode(value, availableLanguages)
            If normalized <> "" Then Return normalized
        Next

        Return ""
    End Function

    Private Function NormalizeLanguageCode(value As String, availableLanguages As Dictionary(Of String, String)) As String
        If String.IsNullOrWhiteSpace(value) Then Return ""

        Dim cleaned As String = value.Trim().Trim(ChrW(34)).Replace("_", "-")
        For Each languageCode As String In availableLanguages.Keys
            If languageCode.Equals(cleaned, StringComparison.OrdinalIgnoreCase) Then Return languageCode
        Next

        Dim neutralLanguage As String = cleaned.Split("-"c)(0)
        For Each languageCode As String In availableLanguages.Keys
            Dim availableNeutralLanguage As String = languageCode.Split("-"c)(0)
            If availableNeutralLanguage.Equals(neutralLanguage, StringComparison.OrdinalIgnoreCase) Then Return languageCode
        Next

        Return ""
    End Function

    Function GetValueFromLanguageData(ItemKey As String) As String
        If LanguageData Is Nothing Then
            Throw New InvalidOperationException("HotInstall language data has not been loaded.")
        End If

        If String.IsNullOrWhiteSpace(ItemKey) OrElse Not ItemKey.Contains("."c) Then
            Throw New InvalidOperationException("HotInstall localization key is invalid: " & If(ItemKey, "<empty>"))
        End If

        Dim separatorIndex As Integer = ItemKey.LastIndexOf("."c)
        Dim sectionName As String = ItemKey.Substring(0, separatorIndex)
        Dim valueName As String = ItemKey.Substring(separatorIndex + 1)

        Try
            Dim value As String = LanguageData(sectionName)(valueName)
            If value Is Nothing Then Throw New KeyNotFoundException()
            Return value.Replace(Quote, "").Replace("{quot;}", Quote).Replace("{crlf;}", CrLf).Replace("{space;}", " ").Replace("{tab;}", vbTab)
        Catch ex As Exception
            Throw New InvalidOperationException("HotInstall localization key was not found." & CrLf & CrLf &
                                                "Section: " & sectionName & CrLf &
                                                "Key: " & valueName & CrLf &
                                                "Full key: " & ItemKey, ex)
        End Try
    End Function

End Module
