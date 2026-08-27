Imports System.Text.RegularExpressions
Imports StarterScriptEditor.Classes.ColorUtilities
Imports StarterScriptEditor.EditorEX.EditorDialogs.FunctionSignatures
Imports System.ServiceProcess

Public Class DocumentOutlineViewer

    Public EditorControl As Control, _
           MyParent As Form

    ' This is invisible and will only serve us for repositioning the caret and determining the line and column
    ' based on that.
    Private InternalTB As New TextBox

    Private CurrentColorMode As ColorThemeMode

    Private ReferenceExpression As Regex, _
            FunctionDeclarationMatches As MatchCollection, _
            SelectedMatchIndex As Integer = 0

    Private Const DO_LM_BATCH As Integer = 0, _
                  DO_LM_POWERSHELL As Integer = 1, _
                  DO_LM_VBSCRIPT As Integer = 2, _
                  DO_LM_JSCRIPT As Integer = 3

    Private Sub cbPin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPin.CheckedChanged
        TopMost = cbPin.Checked
        ShowInTaskbar = Not cbPin.Checked
        MinimizeBox = Not cbPin.Checked
    End Sub

    Private Sub GetAndDisplayDocumentOutline(ByVal LanguageMode As Integer)
        Dim refEx As String = ""
        lblLine.Visible = False
        lvFunctionParameters.Items.Clear()

        InternalTB.Text = EditorControl.Text

        Select Case LanguageMode
            Case DO_LM_BATCH : refEx = "^\s*:(?![:])(?<Name>[A-Za-z_][\w-]*)\b"
            Case DO_LM_POWERSHELL : refEx = "\b(?<Kind>function|filter)\s+(?<Scope>global:|local:|script:|private:)?(?<Name>[A-Za-z_][\w-]*)\s*(\{|\()"
            Case DO_LM_VBSCRIPT : refEx = "\b(?<Modifier>Public\s+Default\s+|Private\s+Default\s+|Public\s+|Private\s+|Default\s+)?(?<Kind>Sub|Function)\s+(?<Name>[A-Za-z_][\w]*)\s*\((?<Params>[^)]*)\)"
            Case DO_LM_JSCRIPT : refEx = "(?<Declaration>\bfunction\s+(?<DeclName>[A-Za-z_$][\w$]*)\s*\((?<DeclParams>[^)]*)\))|(?<Assignment>\b(?:var|let|const)?\s*(?<AssignName>[A-Za-z_$][\w$.]*)\s*=\s*function\b\s*\((?<AssignParams>[^)]*\))"
        End Select

        Try
            ReferenceExpression = New Regex(refEx, RegexOptions.IgnoreCase Or RegexOptions.Compiled Or RegexOptions.Multiline)
            FunctionDeclarationMatches = ReferenceExpression.Matches(EditorControl.Text)

            comboFunctionList.Items.Clear()
            Dim Matches(FunctionDeclarationMatches.Count - 1) As String, _
                idx As Integer = 0
            For Each FunctionDeclarationMatch As Match In FunctionDeclarationMatches
                Dim MatchName As String = FunctionDeclarationMatch.Value.TrimStart(":").TrimEnd("{")
                If LanguageMode = DO_LM_BATCH Then MatchName = Regex.Replace(MatchName, "\r\n", "", RegexOptions.IgnoreCase)
                If LanguageMode = DO_LM_VBSCRIPT Then MatchName = Regex.Replace(MatchName, "\b(?<Modifier>Public\s+Default\s+|Private\s+Default\s+|Public\s+|Private\s+|Default\s+)?(?<Kind>Sub|Function)\s+", "", RegexOptions.IgnoreCase)
                If LanguageMode = DO_LM_POWERSHELL Then MatchName = Regex.Replace(MatchName, "\b(?:function|filter)\s+(?:global:|local:|script:|private:)?", "", RegexOptions.IgnoreCase Or RegexOptions.Multiline).Trim(ControlChars.Cr, ControlChars.Lf, ControlChars.CrLf)
                Matches(idx) = MatchName
                idx += 1
            Next
            comboFunctionList.Items.AddRange(Matches)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DocumentOutlineViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetAndDisplayDocumentOutline(comboLangMode.SelectedIndex)
        With InternalTB
            .MaxLength = Integer.MaxValue
            .WordWrap = False
            .Multiline = True
        End With
        CurrentColorMode = MainForm.CurrentColorMode
        SetColorMode()
        AddHandler EditorControl.TextChanged, AddressOf UnderlyingEditorControlContentChanged
        ColumnHeader1.Width = WindowHelper.ScaleLogical(160)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(128)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(96)
    End Sub

    Private Sub UnderlyingEditorControlContentChanged(ByVal sender As Object, ByVal e As EventArgs)
        If EditorControl Is Nothing Then Exit Sub
        GetAndDisplayDocumentOutline(comboLangMode.SelectedIndex)
    End Sub

    Private Sub comboLangMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboLangMode.SelectedIndexChanged
        If EditorControl Is Nothing Then Exit Sub
        GetAndDisplayDocumentOutline(comboLangMode.SelectedIndex)
    End Sub

    Private Sub comboFunctionList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboFunctionList.SelectedIndexChanged
        Try
            ' Reposition the "caret" to get line
            InternalTB.SelectionStart = FunctionDeclarationMatches(comboFunctionList.SelectedIndex).Index
            InternalTB.SelectionLength = 0
            InternalTB.Select()

            Dim caretPosition As Integer = InternalTB.SelectionStart, _
                line As Integer = InternalTB.GetLineFromCharIndex(caretPosition)

            lblLine.Text = String.Format("Line {0}", line + 1)
            lblLine.Visible = True
            SelectedMatchIndex = comboFunctionList.SelectedIndex

            ' If it is VBScript, then we can try to show signature information
            If comboLangMode.SelectedIndex = DO_LM_VBSCRIPT Then
                lvFunctionParameters.Items.Clear()
                Dim fnSignature As String = comboFunctionList.SelectedItem
                Dim fnSignatureParamBegin As String = InStr(fnSignature, "("), _
                    fnSignatureBeginning As String = fnSignature.Substring(0, fnSignatureParamBegin)
                fnSignature = fnSignature.Replace(fnSignatureBeginning, "").TrimStart("(").TrimEnd(")")

                Dim fnSignatureParams() As String = fnSignature.Split(",")
                Dim params As New List(Of VisualBasicFunctionSignatureParam)
                For Each fnSignatureParam As String In fnSignatureParams
                    ' Any occurrence of byref already tells us we're passing the
                    ' parameter by reference; otherwise, we pass it by value. Also,
                    ' any occurrence of Optional tells us we don't need it when calling it.
                    Dim paramName As String = Regex.Replace(fnSignatureParam, "Optional ", "", RegexOptions.IgnoreCase)
                    paramName = Regex.Replace(paramName, "ByRef ", "", RegexOptions.IgnoreCase)
                    paramName = Regex.Replace(paramName, "ByVal ", "", RegexOptions.IgnoreCase)
                    If String.IsNullOrEmpty(paramName.Trim()) Then Continue For

                    Dim param As New VisualBasicFunctionSignatureParam(paramName.Trim())
                    param.ByReference = Regex.IsMatch(fnSignatureParam, "ByRef ", RegexOptions.IgnoreCase)
                    param.IsOptional = Regex.IsMatch(fnSignatureParam, "Optional ", RegexOptions.IgnoreCase)

                    params.Add(param)
                Next

                Dim paramItems(params.Count - 1) As ListViewItem
                For i As Integer = 0 To params.Count - 1
                    paramItems(i) = New ListViewItem(New String() {params(i).ParameterName, IIf(params(i).ByReference, "By Reference", "By Value"), IIf(params(i).IsOptional, "Optional", "Required")})
                Next
                lvFunctionParameters.Items.AddRange(paramItems)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnNavigate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNavigate.Click
        Try
            With CType(EditorControl, TextBox)
                .SelectionStart = FunctionDeclarationMatches(SelectedMatchIndex).Index
                .SelectionLength = FunctionDeclarationMatches(SelectedMatchIndex).Length
                .Select()
                .ScrollToCaret()
            End With
            MyParent.Focus()
            If MyParent.Name = "MainForm" Then MainForm.UpdateCaretPosition() ' we know for certain our parent is the mainform
        Catch ex As Exception

        End Try
    End Sub

    Private Function IsThemesSvcRunning() As Boolean
        Try
            Dim themesService As New ServiceController("Themes")
            Return themesService.Status = ServiceControllerStatus.Running
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub SetColorMode()
        Select Case CurrentColorMode
            Case ColorThemeMode.Light
                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                ForeColor = Color.White
        End Select

        ' If the themes service is running, we don't theme the image
        If Not IsThemesSvcRunning() Then cbPin.Image = IIf(CurrentColorMode = ColorThemeMode.Light, My.Resources.pin, My.Resources.pin_dark)
        cbPin.ImageAlign = IIf(WindowHelper.GetSystemDpi() > 96.0F, ContentAlignment.MiddleCenter, ContentAlignment.BottomRight)

        comboLangMode.BackColor = BackColor
        comboLangMode.ForeColor = ForeColor
        comboFunctionList.BackColor = BackColor
        comboFunctionList.ForeColor = ForeColor
        gbSignatureDetails.ForeColor = ForeColor
        lvFunctionParameters.BackColor = BackColor
        lvFunctionParameters.ForeColor = ForeColor
    End Sub
End Class