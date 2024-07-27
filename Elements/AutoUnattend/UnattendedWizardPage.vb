Namespace Elements

    Public Class UnattendedWizardPage

        Public Enum Page As Integer
            DisclaimerPage = 0
            RegionalPage = 1
            SysConfigPage = 2
            TimeZonePage = 3
            DiskConfigPage = 4
            ProductKeyPage = 5
            UserAccountsPage = 6
        End Enum

        Public Property WizardPage As Page

        Public Const PageCount As Integer = 7
    End Class

End Namespace
