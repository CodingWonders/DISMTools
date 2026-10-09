Public Class WizardStep

    Private _wizardStepName As String
    Private _wizardStepIsActive As Boolean

    Public Property WizardStepName As String
        Get
            Return _wizardStepName
        End Get
        Set(value As String)
            _wizardStepName = value
            lblWizardStepName.Text = _wizardStepName
        End Set
    End Property

    Public Property WizardStepIsActive As Boolean
        Get
            Return _wizardStepIsActive
        End Get
        Set(value As Boolean)
            _wizardStepIsActive = value
            lblWizardStepName.Font = New Font(Font, If(_wizardStepIsActive, FontStyle.Bold, FontStyle.Regular))
        End Set
    End Property

    Private Sub WizardStep_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If CurrentTheme IsNot Nothing Then BackColor = CurrentTheme.SectionBackgroundColor
    End Sub
End Class
