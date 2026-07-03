Imports System.Windows.Forms
Imports StarterScriptEditor.Classes.ColorUtilities

Public Class ApiKeyGenerationStepsWizard

    Private CurrentColorMode As ColorThemeMode

    Private Const OVERALL_STEP_TOKEN_MGMT As Integer = 0, _
                  OVERALL_STEP_PAT_CREATE As Integer = 1

    ' Minimum and maximum page boundaries
    Private Const MIN_BOUNDARY As Integer = 1, _
                  TOKEN_MGMT_STEP_MAX_BOUNDARY As Integer = 3, _
                  CLASSIC_PAT_MAX_BOUNDARY As Integer = 2, _
                  FINEGRAINED_PAT_MAX_BOUNDARY As Integer = 3

    ' Page constants
    Private Const TOKEN_MGMT_ACC_SETTINGS_PAGE As Integer = 1, _
                  TOKEN_MGMT_DEV_APPS_PAGE As Integer = 2, _
                  TOKEN_MGMT_PAT_DASHBOARD_PAGE As Integer = 3
    Private Const CLASSIC_PAT_DETAILS_PAGE As Integer = 1, _
                  CLASSIC_PAT_KEY_PAGE As Integer = 2
    Private Const FINEGRAINED_PAT_DETAILS_PAGE As Integer = 1, _
                  FINEGRAINED_PAT_DETAILS_PAGE_CONT As Integer = 2, _
                  FINEGRAINED_PAT_KEY_PAGE As Integer = 3

    Private Enum PatMode As Integer
        Classic = 0
        FineGrained = 1
    End Enum

    Private SelectedPatMode As Integer

    Private CurrentTokenMgmtStep As Integer = TOKEN_MGMT_ACC_SETTINGS_PAGE, _
            CurrentClassicPatStep As Integer = CLASSIC_PAT_DETAILS_PAGE, _
            CurrentFineGrainedPatStep As Integer = FINEGRAINED_PAT_DETAILS_PAGE


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ApiKeyGenerationStepsWizard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CurrentColorMode = MainForm.CurrentColorMode
        SetColorMode()
    End Sub

    Private Sub SetColorMode()
        Select Case CurrentColorMode
            Case ColorThemeMode.Light
                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                StepsSidePanel.BackColor = Color.FromArgb(212, 212, 216)
                ButtonContainerPanel.BackColor = Color.FromArgb(200, 200, 204)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                StepsSidePanel.BackColor = Color.FromArgb(28, 28, 28)
                ButtonContainerPanel.BackColor = Color.FromArgb(14, 14, 14)
                ForeColor = Color.White
        End Select

        TabPage1.BackColor = BackColor
        TabPage2.BackColor = BackColor
        TabPage1.ForeColor = ForeColor
        TabPage2.ForeColor = ForeColor
    End Sub

    Private Sub ChangeStep(ByVal OverallStep As Integer, ByVal SpecificStep As Integer)
        TokenMgmtPanel.Visible = OverallStep = OVERALL_STEP_TOKEN_MGMT
        PatContainerPanel.Visible = OverallStep = OVERALL_STEP_PAT_CREATE

        Select Case OverallStep
            Case OVERALL_STEP_TOKEN_MGMT
                AccountMenuPanel.Visible = SpecificStep = TOKEN_MGMT_ACC_SETTINGS_PAGE
                AccountDevSettingsPanel.Visible = SpecificStep = TOKEN_MGMT_DEV_APPS_PAGE
                NewPATPanel.Visible = SpecificStep = TOKEN_MGMT_PAT_DASHBOARD_PAGE

                TokenMgmtPrevStepBtn.Enabled = Not SpecificStep <= MIN_BOUNDARY
                TokenMgmtNextStepBtn.Enabled = Not SpecificStep >= TOKEN_MGMT_STEP_MAX_BOUNDARY
                CurrentTokenMgmtStep = SpecificStep
            Case OVERALL_STEP_PAT_CREATE
                Select Case SelectedPatMode
                    Case PatMode.Classic
                        ClassicPatDetailsPanel.Visible = SpecificStep = CLASSIC_PAT_DETAILS_PAGE
                        ClassicPatKeyPanel.Visible = SpecificStep = CLASSIC_PAT_KEY_PAGE

                        PatCreationPrevStepBtn.Enabled = Not SpecificStep <= MIN_BOUNDARY
                        PatCreationNextStepBtn.Enabled = Not SpecificStep >= CLASSIC_PAT_MAX_BOUNDARY
                        CurrentClassicPatStep = SpecificStep
                    Case PatMode.FineGrained
                        FineGrainedPatDetailsPanel.Visible = SpecificStep = FINEGRAINED_PAT_DETAILS_PAGE
                        FineGrainedPatContDetailsPanel.Visible = SpecificStep = FINEGRAINED_PAT_DETAILS_PAGE_CONT
                        FineGrainedPatKeyPanel.Visible = SpecificStep = FINEGRAINED_PAT_KEY_PAGE

                        PatCreationPrevStepBtn.Enabled = Not SpecificStep <= MIN_BOUNDARY
                        PatCreationNextStepBtn.Enabled = Not SpecificStep >= FINEGRAINED_PAT_MAX_BOUNDARY
                        CurrentFineGrainedPatStep = SpecificStep
                End Select
        End Select
    End Sub

    Private Sub TokenMgmtPrevStepBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TokenMgmtPrevStepBtn.Click
        ChangeStep(OVERALL_STEP_TOKEN_MGMT, CurrentTokenMgmtStep - 1)
    End Sub

    Private Sub TokenMgmtNextStepBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TokenMgmtNextStepBtn.Click
        ChangeStep(OVERALL_STEP_TOKEN_MGMT, CurrentTokenMgmtStep + 1)
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        ChangeStep(OVERALL_STEP_TOKEN_MGMT, 1)

        LinkLabel1.Font = New Font("Tahoma", 8.25F, FontStyle.Bold)
        LinkLabel2.Font = New Font("Tahoma", 8.25F, FontStyle.Regular)
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        ChangeStep(OVERALL_STEP_PAT_CREATE, 1)

        LinkLabel1.Font = New Font("Tahoma", 8.25F, FontStyle.Regular)
        LinkLabel2.Font = New Font("Tahoma", 8.25F, FontStyle.Bold)
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        SelectedPatMode = TabControl1.SelectedIndex

        Select Case SelectedPatMode
            Case PatMode.Classic
                PatCreationPrevStepBtn.Enabled = Not CurrentClassicPatStep <= MIN_BOUNDARY
                PatCreationNextStepBtn.Enabled = Not CurrentClassicPatStep >= CLASSIC_PAT_MAX_BOUNDARY
            Case PatMode.FineGrained
                PatCreationPrevStepBtn.Enabled = Not CurrentFineGrainedPatStep <= MIN_BOUNDARY
                PatCreationNextStepBtn.Enabled = Not CurrentFineGrainedPatStep >= FINEGRAINED_PAT_MAX_BOUNDARY
        End Select
    End Sub

    Private Sub PatCreationPrevStepBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatCreationPrevStepBtn.Click
        ChangeStep(OVERALL_STEP_PAT_CREATE, IIf(SelectedPatMode = PatMode.Classic, CurrentClassicPatStep - 1, CurrentFineGrainedPatStep - 1))
    End Sub

    Private Sub PatCreationNextStepBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatCreationNextStepBtn.Click
        ChangeStep(OVERALL_STEP_PAT_CREATE, IIf(SelectedPatMode = PatMode.Classic, CurrentClassicPatStep + 1, CurrentFineGrainedPatStep + 1))
    End Sub
End Class
