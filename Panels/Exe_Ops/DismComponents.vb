Imports System.Windows.Forms
Imports System.IO

Public Class DismComponents

    Dim fv As FileVersionInfo

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub DismComponents_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "DISM Components"
                        ListView1.Columns(0).Text = "Component"
                        ListView1.Columns(1).Text = "Version"
                        OK_Button.Text = "OK"
                    Case "ESN"
                        Text = "Componentes de DISM"
                        ListView1.Columns(0).Text = "Componente"
                        ListView1.Columns(1).Text = "Versión"
                        OK_Button.Text = "Aceptar"
                End Select
            Case 1
                Text = "DISM Components"
                ListView1.Columns(0).Text = "Component"
                ListView1.Columns(1).Text = "Version"
                OK_Button.Text = "OK"
            Case 2
                Text = "Componentes de DISM"
                ListView1.Columns(0).Text = "Componente"
                ListView1.Columns(1).Text = "Versión"
                OK_Button.Text = "Aceptar"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Visible = True
        For Each DismComponent In My.Computer.FileSystem.GetFiles(Path.GetDirectoryName(Options.TextBox1.Text) & "\dism", FileIO.SearchOption.SearchTopLevelOnly)
            fv = FileVersionInfo.GetVersionInfo(DismComponent)
            ListView1.Items.Add(Path.GetFileName(DismComponent)).SubItems.Add(fv.ProductVersion.ToString())
        Next
    End Sub
End Class
