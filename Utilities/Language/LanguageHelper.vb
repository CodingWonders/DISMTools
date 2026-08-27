Module LanguageHelper

    Sub LoadLanguageFiles()
        LocalizationService.LoadLanguageFiles()
    End Sub

    Function GetValueFromLanguageData(ItemKey As String) As String
        Return LocalizationService.T(ItemKey)
    End Function

End Module
