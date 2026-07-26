Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms

Public Class LocalizationLanguageInfo

    Public Property Code As String
    Public Property Name As String
    Public Property Author As String
    Public Property FilePath As String

    Public Overrides Function ToString() As String
        If String.IsNullOrWhiteSpace(Name) Then Return Code
        If String.IsNullOrWhiteSpace(Code) Then Return Name
        Return Name
    End Function

End Class

Public Class MissingLocalizationItem

    Public Property CultureCode As String
    Public Property ItemKey As String

End Class

Public Class MissingLocalizationException
    Inherits InvalidOperationException

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub
End Class

Friend Class LocalizationLanguageData

    Public Property CultureCode As String
    Public Property LanguageName As String
    Public Property LanguageAuthor As String
    Public Property FilePath As String
    Public ReadOnly Property Sections As Dictionary(Of String, Dictionary(Of String, String))

    Public Sub New()
        Sections = New Dictionary(Of String, Dictionary(Of String, String))(StringComparer.OrdinalIgnoreCase)
    End Sub

End Class

Public Class SectionLocalizer

    Private ReadOnly SectionNameValue As String

    Friend Sub New(sectionName As String)
        If String.IsNullOrWhiteSpace(sectionName) Then Throw New ArgumentException("Localization section name cannot be empty.", NameOf(sectionName))
        SectionNameValue = sectionName.Trim()
    End Sub

    Public ReadOnly Property SectionName As String
        Get
            Return SectionNameValue
        End Get
    End Property

    Default Public ReadOnly Property Item(itemKey As String) As String
        Get
            Return LocalizationService.TForSection(SectionNameValue, itemKey, New Object() {})
        End Get
    End Property

    Public Function Format(itemKey As String, ParamArray args As Object()) As String
        Return LocalizationService.TForSection(SectionNameValue, itemKey, args)
    End Function

    Public Function Upper(itemKey As String, useUpperCase As Boolean, ParamArray args As Object()) As String
        Dim value As String = LocalizationService.TForSection(SectionNameValue, itemKey, args)
        If useUpperCase Then Return value.ToUpper(CultureInfo.CurrentCulture)
        Return value
    End Function

End Class

Module LocalizationService

    Public Const DefaultCultureCode As String = "en-US"

    Private ReadOnly LanguageFiles As New Dictionary(Of String, LocalizationLanguageData)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly MissingItems As New List(Of MissingLocalizationItem)()
    Private ReadOnly MissingItemsLock As New Object()
    Private CurrentCultureCodeValue As String = DefaultCultureCode
    Private FilesLoaded As Boolean = False

    Public ReadOnly Property CurrentCultureCode As String
        Get
            Return CurrentCultureCodeValue
        End Get
    End Property

    Public ReadOnly Property HasLanguageData As Boolean
        Get
            Return LanguageFiles.Count > 0
        End Get
    End Property

    Public ReadOnly Property LoadedCultureCodes As String
        Get
            If LanguageFiles.Count = 0 Then Return "<none>"
            Dim cultureCodes As New List(Of String)(LanguageFiles.Keys)
            cultureCodes.Sort(StringComparer.OrdinalIgnoreCase)
            Return String.Join(", ", cultureCodes.ToArray())
        End Get
    End Property

    Public Sub Initialize(Optional cultureCode As String = Nothing)
        LoadLanguageFiles()

        Dim requestedCultureCode As String = cultureCode
        If String.IsNullOrWhiteSpace(requestedCultureCode) Then requestedCultureCode = GetCommandLineCultureCode()
        If String.IsNullOrWhiteSpace(requestedCultureCode) Then requestedCultureCode = GetSingleLocalLanguageCode()
        If String.IsNullOrWhiteSpace(requestedCultureCode) Then requestedCultureCode = GetPersistedCultureCode()
        If String.IsNullOrWhiteSpace(requestedCultureCode) Then requestedCultureCode = DefaultCultureCode

        SetLanguageByCultureCode(requestedCultureCode)
    End Sub

    Public Sub LoadLanguageFiles()
        LanguageFiles.Clear()
        SyncLock MissingItemsLock
            MissingItems.Clear()
        End SyncLock

        For Each languageDirectory As String In CandidateLanguageDirectories()
            Dim languageFilePaths As String()
            Try
                languageFilePaths = Directory.GetFiles(languageDirectory, "*.ini", SearchOption.TopDirectoryOnly)
                Array.Sort(languageFilePaths, StringComparer.OrdinalIgnoreCase)
            Catch ex As Exception
                Trace.WriteLine("Could not enumerate localization directory " & languageDirectory & ". " & ex.Message)
                Continue For
            End Try

            For Each languageFile As String In languageFilePaths
                Try
                    Dim languageData As LocalizationLanguageData = ParseLanguageFile(languageFile)
                    If languageData Is Nothing OrElse String.IsNullOrWhiteSpace(languageData.CultureCode) Then Continue For

                    If Not LanguageFiles.ContainsKey(languageData.CultureCode) Then
                        LanguageFiles.Add(languageData.CultureCode, languageData)
                    End If
                Catch ex As Exception
                    Trace.WriteLine("Could not parse localization file " & languageFile & ". " & ex.Message)
                End Try
            Next
        Next

        FilesLoaded = True
        Trace.WriteLine("Loaded localization cultures: " & LoadedCultureCodes)
    End Sub

    Public Sub SetLanguageByCultureCode(cultureCode As String)
        EnsureLoaded()
        CurrentCultureCodeValue = NormalizeCultureCode(cultureCode)
        ApplyCurrentCulture(CurrentCultureCodeValue)
    End Sub

    Public Function ResolveCultureCode(settingValue As Object) As String
        If settingValue Is Nothing Then Return DefaultCultureCode

        Dim rawValue As String = settingValue.ToString().Replace(Quote, "").Trim()
        If String.IsNullOrWhiteSpace(rawValue) Then Return DefaultCultureCode

        Return NormalizeCultureCode(rawValue)
    End Function

    Public Function ResolveStartupCultureCode(settingValue As Object) As String
        Dim commandLineCultureCode As String = GetCommandLineCultureCode()
        If Not String.IsNullOrWhiteSpace(commandLineCultureCode) Then Return NormalizeCultureCode(commandLineCultureCode)

        Return ResolveCultureCode(settingValue)
    End Function

    Public Function NormalizeCultureCode(cultureCode As String) As String
        EnsureLoaded()
        If String.IsNullOrWhiteSpace(cultureCode) Then Return DefaultCultureCode

        Dim requestedCultureCode As String = cultureCode.Trim().Trim(ChrW(34))
        For Each loadedCultureCode As String In LanguageFiles.Keys
            If loadedCultureCode.Equals(requestedCultureCode, StringComparison.OrdinalIgnoreCase) Then Return loadedCultureCode
        Next

        Return DefaultCultureCode
    End Function

    Public Function GetMicrosoftLearnCultureCode() As String
        Try
            Dim culture As New CultureInfo(CurrentCultureCodeValue)
            Return culture.Name.ToLowerInvariant()
        Catch
            Return DefaultCultureCode.ToLowerInvariant()
        End Try
    End Function

    Public Function GetLinkArea(fullText As String, linkText As String) As LinkArea
        If fullText Is Nothing Then fullText = ""
        If linkText Is Nothing Then linkText = ""

        Dim startIndex As Integer = fullText.IndexOf(linkText, StringComparison.CurrentCulture)
        If startIndex < 0 Then Return New LinkArea(0, fullText.Length)

        Return New LinkArea(startIndex, linkText.Length)
    End Function

    Public Function GetDocumentationLanguageCode() As String
        Dim cultureCode As String = CurrentCultureCodeValue
        If String.IsNullOrWhiteSpace(cultureCode) Then Return "en"

        Dim neutralCode As String = cultureCode.Split("-"c)(0).ToLowerInvariant()
        Select Case neutralCode
            Case "es", "fr", "pt", "it"
                Return neutralCode
            Case Else
                Return "en"
        End Select
    End Function

    Public Function ForSection(sectionName As String) As SectionLocalizer
        Return New SectionLocalizer(sectionName)
    End Function

    Public Function GetLanguageCommandLineArgument() As String
        Return "/language=" & Quote & CurrentCultureCodeValue & Quote
    End Function

    Public Function TUpper(itemKey As String, useUpperCase As Boolean, ParamArray args As Object()) As String
        Dim value As String = TForCulture(CurrentCultureCodeValue, itemKey, args)
        If useUpperCase Then Return value.ToUpper(CultureInfo.CurrentCulture)
        Return value
    End Function

    Public Function TUpper(sectionName As String, itemName As String, useUpperCase As Boolean, ParamArray args As Object()) As String
        Dim value As String = TForSection(sectionName, itemName, args)
        If useUpperCase Then Return value.ToUpper(CultureInfo.CurrentCulture)
        Return value
    End Function

    Public Function T(itemKey As String, ParamArray args As Object()) As String
        Return TForCulture(CurrentCultureCodeValue, itemKey, args)
    End Function

    Public Function T(sectionName As String, itemName As String, ParamArray args As Object()) As String
        Return TForSection(sectionName, itemName, args)
    End Function

    Friend Function TForSection(sectionName As String, itemName As String, args As Object()) As String
        EnsureLoaded()

        Dim value As String = FindExactValue(CurrentCultureCodeValue, sectionName, itemName)
        If value Is Nothing Then
            Dim itemKey As String = CombineKey(sectionName, itemName)
            RegisterMissingItem(CurrentCultureCodeValue, itemKey)
            Throw New MissingLocalizationException(BuildMissingLocalizationMessage(CurrentCultureCodeValue, itemKey, sectionName, itemName))
        End If

        Return FormatValue(CurrentCultureCodeValue, CombineKey(sectionName, itemName), value, args)
    End Function

    Public Function GetMissingItems() As List(Of MissingLocalizationItem)
        SyncLock MissingItemsLock
            Return New List(Of MissingLocalizationItem)(MissingItems)
        End SyncLock
    End Function

    Public Function GetLanguageFilePath(cultureCode As String) As String
        EnsureLoaded()
        Dim normalizedCultureCode As String = NormalizeCultureCode(cultureCode)
        If LanguageFiles.ContainsKey(normalizedCultureCode) Then Return LanguageFiles(normalizedCultureCode).FilePath
        Return ""
    End Function

    Public Function GetAvailableLanguages() As List(Of LocalizationLanguageInfo)
        EnsureLoaded()

        Dim cultureCodes As New List(Of String)(LanguageFiles.Keys)
        cultureCodes.Sort(StringComparer.OrdinalIgnoreCase)

        Dim languages As New List(Of LocalizationLanguageInfo)()
        For Each cultureCode As String In cultureCodes
            Dim data As LocalizationLanguageData = LanguageFiles(cultureCode)
            languages.Add(New LocalizationLanguageInfo With {
                .Code = cultureCode,
                .Name = If(String.IsNullOrWhiteSpace(data.LanguageName), cultureCode, data.LanguageName),
                .Author = data.LanguageAuthor,
                .FilePath = data.FilePath
            })
        Next

        Return languages
    End Function

    Public Function ValidateLanguage(cultureCode As String, ByRef userMessage As String) As Boolean
        EnsureLoaded()
        userMessage = ""

        Dim requestedCultureCode As String = If(cultureCode, "").Trim().Trim(ChrW(34))
        If String.IsNullOrWhiteSpace(requestedCultureCode) OrElse Not LanguageFiles.ContainsKey(requestedCultureCode) Then
            userMessage = "The requested language is not available." & Environment.NewLine & Environment.NewLine &
                          "Language code: " & If(String.IsNullOrWhiteSpace(requestedCultureCode), "<empty>", requestedCultureCode) & Environment.NewLine &
                          "Loaded languages: " & LoadedCultureCodes & Environment.NewLine & Environment.NewLine &
                          "Make sure the file has the .ini extension and contains [LanguageFileInformation] with a unique LanguageCode value."
            Return False
        End If

        If requestedCultureCode.Equals(DefaultCultureCode, StringComparison.OrdinalIgnoreCase) Then Return True

        If Not LanguageFiles.ContainsKey(DefaultCultureCode) Then
            userMessage = "The reference language file could not be loaded." & Environment.NewLine & Environment.NewLine &
                          "Required language: " & DefaultCultureCode & Environment.NewLine &
                          "Expected file: language\" & DefaultCultureCode & ".ini" & Environment.NewLine & Environment.NewLine &
                          "Restore the original reference language file and try again."
            Return False
        End If

        Dim languageData As LocalizationLanguageData = LanguageFiles(requestedCultureCode)
        Dim referenceData As LocalizationLanguageData = LanguageFiles(DefaultCultureCode)
        Dim errors As New List(Of String)()
        Dim warnings As New List(Of String)()
        Dim missingCount As Integer = 0
        Dim unknownCount As Integer = 0

        AddLanguageSyntaxIssues(languageData.FilePath, errors, warnings)

        For Each referenceSection In referenceData.Sections
            For Each referenceItem In referenceSection.Value
                Dim translatedValue As String = FindExactValue(languageData, referenceSection.Key, referenceItem.Key)
                If translatedValue Is Nothing Then
                    missingCount += 1
                    errors.Add("Missing required entry [" & referenceSection.Key & "] " & referenceItem.Key & ".")
                    Continue For
                End If

                Dim referencePlaceholders As String = GetFormatPlaceholderSignature(referenceItem.Value)
                Dim translatedPlaceholders As String = GetFormatPlaceholderSignature(translatedValue)
                If Not referencePlaceholders.Equals(translatedPlaceholders, StringComparison.Ordinal) Then
                    errors.Add("Numbered placeholder mismatch in [" & referenceSection.Key & "] " & referenceItem.Key &
                               ". Expected {" & referencePlaceholders & "}; found {" & translatedPlaceholders & "}.")
                End If

                Dim unknownControlTokens As String = GetUnknownControlTokens(translatedValue)
                If unknownControlTokens <> "" Then
                    errors.Add("Unknown control token(s) in [" & referenceSection.Key & "] " & referenceItem.Key &
                               ": " & unknownControlTokens & ". Control tokens must not be translated.")
                End If
            Next
        Next

        For Each translatedSection In languageData.Sections
            For Each translatedItem In translatedSection.Value
                If FindExactValue(referenceData, translatedSection.Key, translatedItem.Key) Is Nothing Then
                    unknownCount += 1
                    warnings.Add("Unknown entry [" & translatedSection.Key & "] " & translatedItem.Key & ". Key and section names must not be translated.")
                End If
            Next
        Next

        If errors.Count = 0 Then Return True

        Dim reportPath As String = WriteLanguageValidationReport(languageData, referenceData, errors, warnings)
        Dim message As New StringBuilder()
        message.AppendLine("The language file is incompatible with this DISMTools build or contains invalid entries. It was not applied, and DISMTools will keep using the previous language.")
        message.AppendLine()
        message.AppendLine("Language: " & requestedCultureCode)
        message.AppendLine("File: " & languageData.FilePath)
        message.AppendLine("Reference: " & referenceData.FilePath)
        message.AppendLine("Errors: " & errors.Count.ToString(CultureInfo.InvariantCulture) &
                           " (missing required entries: " & missingCount.ToString(CultureInfo.InvariantCulture) & ")")
        message.AppendLine("Unrecognized entries: " & unknownCount.ToString(CultureInfo.InvariantCulture))
        message.AppendLine()
        message.AppendLine("What is wrong:")

        For index As Integer = 0 To Math.Min(errors.Count, 8) - 1
            message.AppendLine("- " & errors(index))
        Next
        If errors.Count > 8 Then message.AppendLine("- ... and " & (errors.Count - 8).ToString(CultureInfo.InvariantCulture) & " more error(s).")

        message.AppendLine()
        message.AppendLine("How to fix it:")
        If missingCount > 0 AndAlso unknownCount > 0 Then
            message.AppendLine("This file probably belongs to a different DISMTools version. Update DISMTools or use the language file included with this build.")
        End If
        message.AppendLine("Keep every section name and key exactly as written in en-US.ini. Translate only the text after the first '=' character. Do not remove or rename placeholders such as {0} or {1}.")
        If reportPath <> "" Then
            message.AppendLine()
            message.AppendLine("Full validation report: " & reportPath)
        End If

        userMessage = message.ToString().TrimEnd()
        Return False
    End Function

    Private Sub AddLanguageSyntaxIssues(filePath As String, errors As List(Of String), warnings As List(Of String))
        Dim currentSection As String = ""
        Dim knownEntries As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Dim lineNumber As Integer = 0

        For Each rawLine As String In File.ReadAllLines(filePath, Encoding.UTF8)
            lineNumber += 1
            Dim trimmedLine As String = rawLine.Trim()
            If trimmedLine = "" OrElse trimmedLine.StartsWith(";") OrElse trimmedLine.StartsWith("#") Then Continue For

            If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim()
                If currentSection = "" Then errors.Add("Line " & lineNumber.ToString(CultureInfo.InvariantCulture) & ": empty section name.")
                Continue For
            End If

            Dim separatorIndex As Integer = rawLine.IndexOf("="c)
            If currentSection = "" OrElse separatorIndex <= 0 Then
                errors.Add("Line " & lineNumber.ToString(CultureInfo.InvariantCulture) & ": malformed entry: " & trimmedLine)
                Continue For
            End If

            Dim itemName As String = rawLine.Substring(0, separatorIndex).Trim()
            Dim entryIdentity As String = currentSection & ChrW(0) & itemName
            If knownEntries.Contains(entryIdentity) Then
                warnings.Add("Line " & lineNumber.ToString(CultureInfo.InvariantCulture) & ": duplicate entry [" & currentSection & "] " & itemName & ". The last value will be used.")
            Else
                knownEntries.Add(entryIdentity)
            End If
        Next
    End Sub

    Private Function GetFormatPlaceholderSignature(value As String) As String
        If value Is Nothing Then Return ""

        Dim placeholders As New List(Of String)()
        For Each placeholder As Match In Regex.Matches(value, "\{(\d+)(?:[^{}]*)?\}")
            placeholders.Add(placeholder.Groups(1).Value)
        Next
        placeholders.Sort(StringComparer.Ordinal)
        Return String.Join(",", placeholders.ToArray())
    End Function

    Private Function GetUnknownControlTokens(value As String) As String
        If value Is Nothing Then Return ""

        Dim unknownTokens As New List(Of String)()
        For Each tokenMatch As Match In Regex.Matches(value, "\{[^{}\r\n]+\}")
            Dim token As String = tokenMatch.Value
            If Regex.IsMatch(token, "^\{\d+(?:,[^}:]+)?(?::[^{}]+)?\}$") Then Continue For

            Select Case token.ToLowerInvariant()
                Case "{quot;}", "{lbrace;}", "{rbrace;}", "{crlf;}", "{space;}", "{tab;}",
                     "{count}", "{current}", "{currenttcont}", "{taskcount}"
                    Continue For
            End Select

            If Not unknownTokens.Contains(token) Then unknownTokens.Add(token)
        Next

        unknownTokens.Sort(StringComparer.Ordinal)
        Return String.Join(", ", unknownTokens.ToArray())
    End Function

    Private Function WriteLanguageValidationReport(languageData As LocalizationLanguageData,
                                                     referenceData As LocalizationLanguageData,
                                                     errors As List(Of String),
                                                     warnings As List(Of String)) As String
        Try
            Dim reportDirectory As String = Path.Combine(Application.StartupPath, "logs", "localization")
            Directory.CreateDirectory(reportDirectory)
            Dim reportPath As String = Path.Combine(reportDirectory, "language-validation-" & languageData.CultureCode & ".log")
            Dim report As New StringBuilder()
            report.AppendLine("DISMTools language file validation report")
            report.AppendLine("Generated: " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))
            report.AppendLine("Language: " & languageData.CultureCode)
            report.AppendLine("File: " & languageData.FilePath)
            report.AppendLine("Reference: " & referenceData.FilePath)
            report.AppendLine()
            report.AppendLine("ERRORS (" & errors.Count.ToString(CultureInfo.InvariantCulture) & ")")
            For Each validationError As String In errors
                report.AppendLine("ERROR: " & validationError)
            Next
            report.AppendLine()
            report.AppendLine("UNRECOGNIZED ENTRIES (" & warnings.Count.ToString(CultureInfo.InvariantCulture) & ")")
            For Each validationWarning As String In warnings
                report.AppendLine("WARNING: " & validationWarning)
            Next
            report.AppendLine()
            report.AppendLine("Fix: keep section names, key names, and placeholders identical to en-US.ini; translate only values after '='.")
            File.WriteAllText(reportPath, report.ToString(), Encoding.UTF8)
            Return reportPath
        Catch ex As Exception
            Trace.WriteLine("Could not write language validation report. " & ex.Message)
            Return ""
        End Try
    End Function

    Private Function TForCulture(cultureCode As String, itemKey As String, args As Object()) As String
        EnsureLoaded()

        Dim matchedSectionName As String = ""
        Dim matchedValueName As String = ""
        Dim value As String = FindValue(cultureCode, itemKey, matchedSectionName, matchedValueName)
        If value Is Nothing Then
            RegisterMissingItem(cultureCode, itemKey)
            Throw New MissingLocalizationException(BuildMissingLocalizationMessage(cultureCode, itemKey, matchedSectionName, matchedValueName))
        End If

        Return FormatValue(cultureCode, itemKey, value, args)
    End Function

    Private Function FormatValue(cultureCode As String, itemKey As String, rawValue As String, args As Object()) As String
        Dim value As String = DecodeValue(rawValue)

        If args IsNot Nothing AndAlso args.Length > 0 Then
            Try
                value = String.Format(CultureInfo.CurrentCulture, value, args)
            Catch ex As FormatException
                Throw New MissingLocalizationException(BuildInvalidFormatMessage(cultureCode, itemKey, value, ex))
            End Try
        End If

        Return value
    End Function

    Private Sub RegisterMissingItem(cultureCode As String, itemKey As String)
        If String.IsNullOrWhiteSpace(itemKey) Then Return

        SyncLock MissingItemsLock
            For Each item As MissingLocalizationItem In MissingItems
                If item.CultureCode.Equals(cultureCode, StringComparison.OrdinalIgnoreCase) AndAlso item.ItemKey.Equals(itemKey, StringComparison.OrdinalIgnoreCase) Then Return
            Next

            MissingItems.Add(New MissingLocalizationItem With {
                .CultureCode = cultureCode,
                .ItemKey = itemKey
            })
        End SyncLock

        Trace.WriteLine("Missing localization item " & itemKey & " for " & cultureCode)
    End Sub

    Private Sub EnsureLoaded()
        If Not FilesLoaded Then LoadLanguageFiles()
    End Sub

    Private Function CandidateLanguageDirectories() As List(Of String)
        Dim result As New List(Of String)()
        AddCandidateLanguageDirectories(result, AppDomain.CurrentDomain.BaseDirectory)
        AddCandidateLanguageDirectories(result, Application.StartupPath)
        AddCandidateLanguageDirectories(result, Environment.CurrentDirectory)
        Return result
    End Function

    Private Sub AddCandidateLanguageDirectories(result As List(Of String), startDirectory As String)
        If String.IsNullOrWhiteSpace(startDirectory) Then Return

        Dim currentDirectory As DirectoryInfo
        Try
            currentDirectory = New DirectoryInfo(startDirectory)
        Catch
            Return
        End Try

        For depth As Integer = 0 To 8
            If currentDirectory Is Nothing Then Exit For
            Dim candidate As String = Path.Combine(currentDirectory.FullName, "language")
            If Directory.Exists(candidate) Then AddUniquePath(result, candidate)
            currentDirectory = currentDirectory.Parent
        Next
    End Sub

    Private Function CandidateSettingsFiles() As List(Of String)
        Dim result As New List(Of String)()
        AddCandidateSettingsFiles(result, AppDomain.CurrentDomain.BaseDirectory)
        AddCandidateSettingsFiles(result, Application.StartupPath)
        AddCandidateSettingsFiles(result, Environment.CurrentDirectory)
        Return result
    End Function

    Private Sub AddCandidateSettingsFiles(result As List(Of String), startDirectory As String)
        If String.IsNullOrWhiteSpace(startDirectory) Then Return

        Dim currentDirectory As DirectoryInfo
        Try
            currentDirectory = New DirectoryInfo(startDirectory)
        Catch
            Return
        End Try

        For depth As Integer = 0 To 8
            If currentDirectory Is Nothing Then Exit For
            Dim candidate As String = Path.Combine(currentDirectory.FullName, "settings.ini")
            If File.Exists(candidate) Then AddUniquePath(result, candidate)
            currentDirectory = currentDirectory.Parent
        Next
    End Sub

    Private Sub AddUniquePath(paths As List(Of String), candidate As String)
        For Each existingPath As String In paths
            If existingPath.Equals(candidate, StringComparison.OrdinalIgnoreCase) Then Return
        Next
        paths.Add(candidate)
    End Sub

    Private Function GetCommandLineCultureCode() As String
        For Each argument As String In Environment.GetCommandLineArgs()
            Dim prefixes As String() = {"/language=", "/languagecode=", "--language="}
            For Each prefix As String In prefixes
                If argument.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) Then
                    Return argument.Substring(prefix.Length).Trim().Trim(ChrW(34))
                End If
            Next
        Next
        Return ""
    End Function

    Private Function GetSingleLocalLanguageCode() As String
        For Each languageDirectory As String In CandidateLanguageDirectories()
            Dim files As String()
            Try
                files = Directory.GetFiles(languageDirectory, "*.ini", SearchOption.TopDirectoryOnly)
            Catch
                Continue For
            End Try

            If files.Length = 0 Then Continue For

            Dim cultureCodes As New List(Of String)()
            For Each languageFile As String In files
                Dim cultureCode As String = ReadIniValue(languageFile, "LanguageFileInformation", "LanguageCode").Trim().Trim(ChrW(34))
                If Not String.IsNullOrWhiteSpace(cultureCode) Then cultureCodes.Add(cultureCode)
            Next

            If cultureCodes.Count = 1 Then Return cultureCodes(0)
            Return ""
        Next

        Return ""
    End Function

    Private Function GetPersistedCultureCode() As String
        For Each settingsPath As String In CandidateSettingsFiles()
            Dim cultureCode As String = ReadIniValue(settingsPath, "Personalization", "LanguageCode")
            If Not String.IsNullOrWhiteSpace(cultureCode) Then Return cultureCode.Trim().Trim(ChrW(34))
        Next

        Dim registryPaths As String() = {
            "Software\DISMTools\Stable\Personalization",
            "Software\DISMTools\Preview\Personalization"
        }

        For Each registryPath As String In registryPaths
            Try
                Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(registryPath, False)
                    If key Is Nothing Then Continue For
                    Dim value As Object = key.GetValue("LanguageCode", Nothing)
                    If value IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(value.ToString()) Then Return value.ToString()
                End Using
            Catch
            End Try
        Next

        Return ""
    End Function

    Private Function ParseLanguageFile(languageFile As String) As LocalizationLanguageData
        Dim data As New LocalizationLanguageData With {.FilePath = languageFile}
        Dim currentSection As String = ""

        For Each rawLine As String In File.ReadAllLines(languageFile, Encoding.UTF8)
            Dim trimmedLine As String = rawLine.Trim()
            If trimmedLine = "" OrElse trimmedLine.StartsWith(";") OrElse trimmedLine.StartsWith("#") Then Continue For

            If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim()
                If currentSection <> "" AndAlso Not data.Sections.ContainsKey(currentSection) Then
                    data.Sections.Add(currentSection, New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase))
                End If
                Continue For
            End If

            If currentSection = "" Then Continue For
            Dim separatorIndex As Integer = rawLine.IndexOf("="c)
            If separatorIndex <= 0 Then Continue For

            Dim itemName As String = rawLine.Substring(0, separatorIndex).Trim()
            Dim itemValue As String = rawLine.Substring(separatorIndex + 1).Trim()
            If itemName = "" Then Continue For

            data.Sections(currentSection)(itemName) = itemValue
        Next

        data.CultureCode = DecodeValue(FindExactValue(data, "LanguageFileInformation", "LanguageCode"))
        data.LanguageName = DecodeValue(FindExactValue(data, "LanguageFileInformation", "LanguageName"))
        data.LanguageAuthor = DecodeValue(FindExactValue(data, "LanguageFileInformation", "LanguageAuthor"))

        If data.CultureCode IsNot Nothing Then data.CultureCode = data.CultureCode.Trim()
        If data.LanguageName IsNot Nothing Then data.LanguageName = data.LanguageName.Trim()
        If data.LanguageAuthor IsNot Nothing Then data.LanguageAuthor = data.LanguageAuthor.Trim()

        Return data
    End Function

    Private Function ReadIniValue(filePath As String, sectionName As String, itemName As String) As String
        If String.IsNullOrWhiteSpace(filePath) OrElse Not File.Exists(filePath) Then Return ""

        Dim currentSection As String = ""
        For Each rawLine As String In File.ReadAllLines(filePath, Encoding.UTF8)
            Dim trimmedLine As String = rawLine.Trim()
            If trimmedLine = "" OrElse trimmedLine.StartsWith(";") OrElse trimmedLine.StartsWith("#") Then Continue For

            If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim()
                Continue For
            End If

            If Not currentSection.Equals(sectionName, StringComparison.OrdinalIgnoreCase) Then Continue For
            Dim separatorIndex As Integer = rawLine.IndexOf("="c)
            If separatorIndex <= 0 Then Continue For

            Dim candidateName As String = rawLine.Substring(0, separatorIndex).Trim()
            If candidateName.Equals(itemName, StringComparison.OrdinalIgnoreCase) Then Return rawLine.Substring(separatorIndex + 1).Trim()
        Next

        Return ""
    End Function

    Private Function FindValue(cultureCode As String, itemKey As String, ByRef matchedSectionName As String, ByRef matchedValueName As String) As String
        matchedSectionName = ""
        matchedValueName = ""

        If String.IsNullOrWhiteSpace(cultureCode) OrElse String.IsNullOrWhiteSpace(itemKey) Then Return Nothing
        If Not LanguageFiles.ContainsKey(cultureCode) Then Return Nothing

        Dim separatorIndexes As New List(Of Integer)()
        For index As Integer = 0 To itemKey.Length - 1
            If itemKey(index) = "."c Then separatorIndexes.Add(index)
        Next

        For separatorIndex As Integer = separatorIndexes.Count - 1 To 0 Step -1
            Dim dotIndex As Integer = separatorIndexes(separatorIndex)
            If dotIndex <= 0 OrElse dotIndex >= itemKey.Length - 1 Then Continue For

            Dim sectionName As String = itemKey.Substring(0, dotIndex)
            Dim valueName As String = itemKey.Substring(dotIndex + 1)
            Dim value As String = FindExactValue(LanguageFiles(cultureCode), sectionName, valueName)
            If value Is Nothing Then Continue For

            matchedSectionName = sectionName
            matchedValueName = valueName
            Return value
        Next

        Return Nothing
    End Function

    Private Function FindExactValue(cultureCode As String, sectionName As String, valueName As String) As String
        If String.IsNullOrWhiteSpace(cultureCode) OrElse Not LanguageFiles.ContainsKey(cultureCode) Then Return Nothing
        Return FindExactValue(LanguageFiles(cultureCode), sectionName, valueName)
    End Function

    Private Function FindExactValue(data As LocalizationLanguageData, sectionName As String, valueName As String) As String
        If data Is Nothing OrElse String.IsNullOrWhiteSpace(sectionName) OrElse String.IsNullOrWhiteSpace(valueName) Then Return Nothing
        If Not data.Sections.ContainsKey(sectionName) Then Return Nothing
        If Not data.Sections(sectionName).ContainsKey(valueName) Then Return Nothing
        Return data.Sections(sectionName)(valueName)
    End Function

    Private Sub ApplyCurrentCulture(cultureCode As String)
        Try
            Dim culture As New CultureInfo(cultureCode)
            Thread.CurrentThread.CurrentCulture = culture
            Thread.CurrentThread.CurrentUICulture = culture
            CultureInfo.DefaultThreadCurrentCulture = culture
            CultureInfo.DefaultThreadCurrentUICulture = culture
        Catch
            Dim fallbackCulture As New CultureInfo(DefaultCultureCode)
            Thread.CurrentThread.CurrentCulture = fallbackCulture
            Thread.CurrentThread.CurrentUICulture = fallbackCulture
            CultureInfo.DefaultThreadCurrentCulture = fallbackCulture
            CultureInfo.DefaultThreadCurrentUICulture = fallbackCulture
        End Try
    End Sub

    Private Function CombineKey(sectionName As String, itemName As String) As String
        If String.IsNullOrWhiteSpace(sectionName) Then Throw New ArgumentException("Localization section name cannot be empty.", NameOf(sectionName))
        If String.IsNullOrWhiteSpace(itemName) Then Throw New ArgumentException("Localization item name cannot be empty.", NameOf(itemName))
        Return sectionName.Trim().TrimEnd("."c) & "." & itemName.Trim().TrimStart("."c)
    End Function

    Private Function BuildMissingLocalizationMessage(cultureCode As String, itemKey As String, Optional sectionName As String = "", Optional valueName As String = "") As String
        If String.IsNullOrWhiteSpace(sectionName) OrElse String.IsNullOrWhiteSpace(valueName) Then SplitPreferredKey(itemKey, sectionName, valueName)

        Dim languageFile As String = "<not loaded>"
        If LanguageFiles.ContainsKey(cultureCode) Then languageFile = LanguageFiles(cultureCode).FilePath

        Dim message As New StringBuilder()
        message.AppendLine("Localization key was not found.")
        message.AppendLine()
        message.AppendLine("Language: " & cultureCode)
        message.AppendLine("Language file: " & languageFile)
        message.AppendLine("Section: " & If(String.IsNullOrWhiteSpace(sectionName), "<unknown>", sectionName))
        message.AppendLine("Key: " & If(String.IsNullOrWhiteSpace(valueName), itemKey, valueName))
        message.AppendLine("Full key: " & itemKey)

        Dim sourceLocation As String = GetSourceLocation()
        If sourceLocation <> "" Then message.AppendLine("Source: " & sourceLocation)

        message.AppendLine("Loaded cultures: " & LoadedCultureCodes)
        message.AppendLine()
        message.AppendLine("Fix:")
        message.AppendLine("Open the language INI file and add this entry:")
        message.AppendLine()
        message.AppendLine("[" & If(String.IsNullOrWhiteSpace(sectionName), "Section.Name", sectionName) & "]")
        message.AppendLine(If(String.IsNullOrWhiteSpace(valueName), itemKey, valueName) & "=")
        Return message.ToString().TrimEnd()
    End Function

    Private Function BuildInvalidFormatMessage(cultureCode As String, itemKey As String, value As String, ex As FormatException) As String
        Dim sectionName As String = ""
        Dim valueName As String = ""
        SplitPreferredKey(itemKey, sectionName, valueName)

        Return "Localization value format is invalid." & Environment.NewLine & Environment.NewLine &
               "Language: " & cultureCode & Environment.NewLine &
               "Section: " & sectionName & Environment.NewLine &
               "Key: " & valueName & Environment.NewLine &
               "Full key: " & itemKey & Environment.NewLine &
               "Value: " & value & Environment.NewLine &
               "Error: " & ex.Message
    End Function

    Private Sub SplitPreferredKey(itemKey As String, ByRef sectionName As String, ByRef valueName As String)
        sectionName = ""
        valueName = ""
        If String.IsNullOrWhiteSpace(itemKey) Then Return

        Dim dotIndex As Integer = itemKey.LastIndexOf("."c)
        If dotIndex <= 0 OrElse dotIndex >= itemKey.Length - 1 Then Return

        sectionName = itemKey.Substring(0, dotIndex)
        valueName = itemKey.Substring(dotIndex + 1)
    End Sub

    Private Function GetSourceLocation() As String
        Try
            Dim trace As New StackTrace(True)
            For Each frame As StackFrame In trace.GetFrames()
                Dim method = frame.GetMethod()
                If method Is Nothing OrElse method.DeclaringType Is Nothing Then Continue For
                If method.DeclaringType.Name = NameOf(LocalizationService) OrElse method.DeclaringType.Name = NameOf(SectionLocalizer) Then Continue For

                Dim fileName As String = frame.GetFileName()
                Dim lineNumber As Integer = frame.GetFileLineNumber()
                If Not String.IsNullOrWhiteSpace(fileName) AndAlso lineNumber > 0 Then Return fileName & ":" & lineNumber.ToString(CultureInfo.InvariantCulture)
                Return method.DeclaringType.FullName & "." & method.Name
            Next
        Catch
        End Try

        Return ""
    End Function

    Private Function DecodeValue(value As String) As String
        If value Is Nothing Then Return Nothing
        Return value.Replace(Quote, "").Replace("{quot;}", Quote).Replace("{lbrace;}", "{").Replace("{rbrace;}", "}").Replace("{crlf;}", vbCrLf).Replace("{space;}", " ").Replace("{tab;}", vbTab)
    End Function

End Module
