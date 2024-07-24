Namespace Elements

    Public Class UnattendedWizardPage

        Public Enum Page As Integer
            DisclaimerPage = 0
            RegionalPage = 1
            SysConfigPage = 2
        End Enum

        Public Property WizardPage As Page

        Public Const PageCount As Integer = 3
    End Class

End Namespace
