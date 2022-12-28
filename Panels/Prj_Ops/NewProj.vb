Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class NewProj

    Dim IsReqField1Valid As Boolean
    Dim IsReqField2Valid As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.projName = TextBox1.Text
        ProgressPanel.projPath = TextBox2.Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub NewProj_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
        End If
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
            IsReqField2Valid = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            If TextBox1.Text.Equals("con", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("CON", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("aux", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("AUX", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("prn", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("PRN", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("nul", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("NUL", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Contains("<") Or _
                TextBox1.Text.Contains(">") Or _
                TextBox1.Text.Contains(":") Or _
                TextBox1.Text.Contains(Quote) Or _
                TextBox1.Text.Contains("/") Or _
                TextBox1.Text.Contains("\") Or _
                TextBox1.Text.Contains("|") Or _
                TextBox1.Text.Contains("?") Or _
                TextBox1.Text.Contains("*") Then
                IsReqField1Valid = False
            Else
                IsReqField1Valid = True
            End If
        Else
            IsReqField1Valid = False
        End If
        CheckReqFields()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If Directory.Exists(TextBox2.Text) Then
            IsReqField2Valid = True
        Else
            IsReqField2Valid = False
        End If
        CheckReqFields()
    End Sub

    Sub CheckReqFields()
        If IsReqField1Valid And IsReqField2Valid Then
            OK_Button.Enabled = True
        Else
            OK_Button.Enabled = False
        End If
    End Sub
End Class
