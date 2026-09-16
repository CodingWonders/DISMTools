Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Public Class DisplayNameFormatterDialog

    Public FormattedAccountName As String,
           SourceDisplayName As String

    Private wordParts() As String

    ' Initial Templates
    Private OneWordFormat As String = "",
            TwoWordFormat As String = "",
            ThreeWordFormat As String = "",
            FourWordFormat As String = "",
            FiveWordFormat As String = ""

    Private NameFormatter As New Regex("")

    Private Sub btnOneWord_Click(sender As Object, e As EventArgs) Handles btnOneWord.Click
        FormattedAccountName = OneWordFormat
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnTwoWords_Click(sender As Object, e As EventArgs) Handles btnTwoWords.Click
        FormattedAccountName = TwoWordFormat
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnThreeWords_Click(sender As Object, e As EventArgs) Handles btnThreeWords.Click
        FormattedAccountName = ThreeWordFormat
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnFourWords_Click(sender As Object, e As EventArgs) Handles btnFourWords.Click
        FormattedAccountName = FourWordFormat
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnFiveWords_Click(sender As Object, e As EventArgs) Handles btnFiveWords.Click
        FormattedAccountName = FiveWordFormat
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub DisplayNameFormatterDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = ForeColor
        tbCustomUserFormat.BackColor = BackColor
        tbCustomUserFormat.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Depending on how many words the source display name has, we have to enable and disable buttons
        wordParts = SourceDisplayName.Split(" ")

        btnTwoWords.Enabled = wordParts.Count >= 2
        btnThreeWords.Enabled = wordParts.Count >= 3
        btnFourWords.Enabled = wordParts.Count >= 4
        btnFiveWords.Enabled = wordParts.Count >= 5

        OneWordFormat = wordParts(0).ToLower().Substring(0, Math.Min(5, wordParts(0).Length))
        If wordParts.Count >= 2 Then TwoWordFormat = String.Format("{0}{1}", wordParts(0).ToLower().Substring(0, Math.Min(5, wordParts(0).Length)), wordParts(1).ToLower().Substring(0, 1))
        If wordParts.Count >= 3 Then ThreeWordFormat = String.Format("{0}{1}{2}", wordParts(0).ToLower().Substring(0, 1), wordParts(1).ToLower().Substring(0, 1), wordParts(2).ToLower().Substring(0, Math.Min(5, wordParts(2).Length)))
        If wordParts.Count >= 4 Then FourWordFormat = String.Format("{0}{1}{2}{3}", wordParts(0).ToLower().Substring(0, 1), wordParts(1).ToLower().Substring(0, 1), wordParts(2).ToLower().Substring(0, 1), wordParts(3).ToLower().Substring(0, Math.Min(5, wordParts(3).Length)))
        If wordParts.Count >= 5 Then FiveWordFormat = String.Format("{0}{1}{2}{3}{4}", wordParts(0).ToLower().Substring(0, 1), wordParts(1).ToLower().Substring(0, 1), wordParts(2).ToLower().Substring(0, 1), wordParts(3).ToLower().Substring(0, 1), wordParts(4).ToLower().Substring(0, Math.Min(5, wordParts(4).Length)))

        btnOneWord.Text = String.Format("Continue with {0}{1}{0}", Quote, OneWordFormat)
        btnTwoWords.Text = If(wordParts.Count >= 2, String.Format("Continue with {0}{1}{0}", Quote, TwoWordFormat), "A minimum of 2 words is required to use this option")
        btnThreeWords.Text = If(wordParts.Count >= 3, String.Format("Continue with {0}{1}{0}", Quote, ThreeWordFormat), "A minimum of 3 words is required to use this option")
        btnFourWords.Text = If(wordParts.Count >= 4, String.Format("Continue with {0}{1}{0}", Quote, FourWordFormat), "A minimum of 4 words is required to use this option")
        btnFiveWords.Text = If(wordParts.Count >= 5, String.Format("Continue with {0}{1}{0}", Quote, FiveWordFormat), "A minimum of 5 words is required to use this option")

        lblSourceDispName.Text = SourceDisplayName

        If tbCustomUserFormat.Text <> "" Then
            FormattedAccountName = tbCustomUserFormat.Text
            ParseAccountTokens()
        End If
    End Sub

    Private Sub tbCustomUserFormat_TextChanged(sender As Object, e As EventArgs) Handles tbCustomUserFormat.TextChanged
        FormattedAccountName = tbCustomUserFormat.Text
        ParseAccountTokens()
    End Sub

    Private Sub SetPattern(Pattern As String)
        NameFormatter = New Regex(Pattern)
    End Sub

    Private Sub ParseAccountTokens()
        Dim expMatches As MatchCollection

        Try
            ' {rnd} keywords
            SetPattern("\{rnd\}")
            FormattedAccountName = NameFormatter.Replace(FormattedAccountName, New Random().Next(UShort.MaxValue).ToString().PadLeft(5, "0"))
            ' {rnd} keywords with custom padding chars
            SetPattern("\{rnd:(?<PaddingChar>.{1})\}")
            expMatches = NameFormatter.Matches(FormattedAccountName)
            If expMatches IsNot Nothing AndAlso expMatches.Cast(Of Match)().Any() Then
                Dim padCharacter As String = expMatches(0).Groups("PaddingChar").Value
                FormattedAccountName = NameFormatter.Replace(FormattedAccountName, New Random().Next(UShort.MaxValue).ToString().PadLeft(5, padCharacter))
            End If
            ' {rnd} keywords with no padding chars
            SetPattern("\{rnd:\}")
            FormattedAccountName = NameFormatter.Replace(FormattedAccountName, New Random().Next(UShort.MaxValue))
            ' standalone {1}-{5}
            SetPattern("\{[1-5]{1}\}")
            expMatches = NameFormatter.Matches(FormattedAccountName)
            If expMatches IsNot Nothing AndAlso expMatches.Cast(Of Match)().Any() Then
                For Each expMatch As Match In expMatches
                    Select Case expMatch.Value
                        Case "{1}" : If wordParts.Length >= 1 Then FormattedAccountName = FormattedAccountName.Replace(expMatch.Value, wordParts(0).ToLower())
                        Case "{2}" : If wordParts.Length >= 2 Then FormattedAccountName = FormattedAccountName.Replace(expMatch.Value, wordParts(1).ToLower())
                        Case "{3}" : If wordParts.Length >= 3 Then FormattedAccountName = FormattedAccountName.Replace(expMatch.Value, wordParts(2).ToLower())
                        Case "{4}" : If wordParts.Length >= 4 Then FormattedAccountName = FormattedAccountName.Replace(expMatch.Value, wordParts(3).ToLower())
                        Case "{5}" : If wordParts.Length >= 5 Then FormattedAccountName = FormattedAccountName.Replace(expMatch.Value, wordParts(4).ToLower())
                    End Select
                Next
            End If
            ' {1}-{5} with first N letters
            SetPattern("\{[1-5]{1}:\d*\}")
            expMatches = NameFormatter.Matches(FormattedAccountName)
            If expMatches IsNot Nothing AndAlso expMatches.Cast(Of Match)().Any() Then
                For Each expMatch As Match In expMatches
                    Dim targetWord As String = "",
                        expressionParts() As String = expMatch.Value.Trim("{", "}").Split(":"),
                        wordIndex As Integer = CInt(expressionParts(0)),
                        totalWordLength As Integer = CInt(expressionParts(1))

                    Select Case wordIndex
                        Case 1 : If wordParts.Length >= 1 Then targetWord = wordParts(0)
                        Case 2 : If wordParts.Length >= 2 Then targetWord = wordParts(1)
                        Case 3 : If wordParts.Length >= 3 Then targetWord = wordParts(2)
                        Case 4 : If wordParts.Length >= 4 Then targetWord = wordParts(3)
                        Case 5 : If wordParts.Length >= 5 Then targetWord = wordParts(4)
                    End Select

                    If totalWordLength > 0 Then targetWord = targetWord.Substring(0, Math.Min(targetWord.Length, totalWordLength))

                    FormattedAccountName = FormattedAccountName.Replace(expMatch.Value, targetWord)
                Next
            End If
            ' {1}-{5} with N letters with a starting index
            SetPattern("\{[1-5]{1}:\d*-\d*\}")
            expMatches = NameFormatter.Matches(FormattedAccountName)
            If expMatches IsNot Nothing AndAlso expMatches.Cast(Of Match)().Any() Then
                For Each expMatch As Match In expMatches
                    Dim targetWord As String = "",
                        expressionParts() As String = expMatch.Value.Trim("{", "}").Split(":"),
                        wordIndex As Integer = CInt(expressionParts(0)),
                        wordLengthParts() As String = expressionParts(1).Split("-"),
                        beginningIndex As Integer = CInt(wordLengthParts(0)),
                        characterLength As Integer = CInt(wordLengthParts(1))

                    Select Case wordIndex
                        Case 1 : If wordParts.Length >= 1 Then targetWord = wordParts(0)
                        Case 2 : If wordParts.Length >= 2 Then targetWord = wordParts(1)
                        Case 3 : If wordParts.Length >= 3 Then targetWord = wordParts(2)
                        Case 4 : If wordParts.Length >= 4 Then targetWord = wordParts(3)
                        Case 5 : If wordParts.Length >= 5 Then targetWord = wordParts(4)
                    End Select

                    If beginningIndex > 0 AndAlso characterLength > 0 Then targetWord = targetWord.Substring(beginningIndex - 1, Math.Min(targetWord.Length - beginningIndex, characterLength))

                    FormattedAccountName = FormattedAccountName.Replace(expMatch.Value, targetWord)
                Next
            End If

            lblTargetFormattedName.Text = FormattedAccountName
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnCustomFormat_Click(sender As Object, e As EventArgs) Handles btnCustomFormat.Click
        If FormattedAccountName.Length > 20 Then
            Dim msg As String = "The specified account name contains more than 20 characters. If you continue with this name, it will be trimmed to the first 20 characters. Do you want to continue with this name?"
            If MessageBox.Show(Me, msg, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub

            FormattedAccountName = FormattedAccountName.Substring(0, 20)
        End If

        Dim invalidChars As Char() = {"/", "\", "[", "]", ":", ";", "|", "=", ",", "+", "*", "?", "<", ">", Quote, "%"}
        FormattedAccountName = New String(FormattedAccountName.Where(Function(c) Not invalidChars.Contains(c)).ToArray()).TrimEnd(".")

        DialogResult = DialogResult.OK
        Close()
    End Sub
End Class
