Namespace Elements

    Public Class UnattendedWizardPage

        Public Enum Page As Integer
            WelcomePage = 0
            RegionalPage = 1
            SysConfigPage = 2
            TimeZonePage = 3
            DiskConfigPage = 4
            ProductKeyPage = 5
            UserAccountsPage = 6
            PWExpirationPage = 7
            AccountLockdownPage = 8
            VirtualMachinePage = 9
            NetworkConnectionsPage = 10
            SystemTelemetryPage = 11
            PostInstallPage = 12
            ComponentPage = 13
            ReviewPage = 14
            ProgressPage = 15
            FinishPage = 16
        End Enum

        ''' <summary>
        ''' The page of the unattended answer file wizard
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property WizardPage As Page

        ''' <summary>
        ''' Page count
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PageCount As Integer = 17
    End Class

End Namespace
