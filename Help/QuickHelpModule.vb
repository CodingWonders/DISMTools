Module QuickHelpModule

    Public Sub ShowQuickHelp(QuickHelpMessage As String)
        MessageBox.Show(QuickHelpMessage, LocalizationService.ForSection("Help.QuickHelp")("QuickHelp.Message"), MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Module
