Namespace Elements

    Public Class UnattendedWizardPage

        Public Enum Page As Integer
            DisclaimerPage = 0
            RegionalPage = 1
            SysConfigPage = 2
            TimeZonePage = 3
        End Enum

        Public Property WizardPage As Page

        Public Const PageCount As Integer = 4
    End Class

End Namespace
