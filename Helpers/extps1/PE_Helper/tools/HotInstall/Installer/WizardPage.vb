Public Class WizardPage

    ''' <summary>
    ''' The installer pages
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Page As Integer
        ''' <summary>
        ''' Disclaimer page
        ''' </summary>
        ''' <remarks></remarks>
        DisclaimerPage = 0
        ''' <summary>
        ''' Boot and installation image file info page
        ''' </summary>
        ''' <remarks></remarks>
        ImageInfoPage = 1
        ''' <summary>
        ''' Process explanation page
        ''' </summary>
        ''' <remarks></remarks>
        ExplanationPage = 2
        ''' <summary>
        ''' The progress page
        ''' </summary>
        ''' <remarks></remarks>
        InstallationPage = 3
        ''' <summary>
        ''' The restart page
        ''' </summary>
        ''' <remarks></remarks>
        FinishPage = 4
        ''' <summary>
        ''' The failure page
        ''' </summary>
        ''' <remarks></remarks>
        FailurePage = 5
    End Enum

    Public Property InstallerWizardPage As Page
    Public Const PageCount As Integer = 6

End Class
