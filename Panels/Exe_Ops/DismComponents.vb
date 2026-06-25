Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

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
                    Case "ENU", "ENG"
                        Text = "DISM Components"
                        ListView1.Columns(0).Text = "Component"
                        ListView1.Columns(1).Text = "Version"
                        OK_Button.Text = "OK"
                    Case "ESN"
                        Text = "Componentes de DISM"
                        ListView1.Columns(0).Text = "Componente"
                        ListView1.Columns(1).Text = "Versión"
                        OK_Button.Text = "Aceptar"
                    Case "FRA"
                        Text = "Composants du DISM"
                        ListView1.Columns(0).Text = "Composant"
                        ListView1.Columns(1).Text = "Version"
                        OK_Button.Text = "OK"
                    Case "PTB", "PTG"
                        Text = "Componentes DISM"
                        ListView1.Columns(0).Text = " Componente"
                        ListView1.Columns(1).Text = "Versão"
                        OK_Button.Text = "OK"
                    Case "ITA"
                        Text = "Componenti DISM"
                        ListView1.Columns(0).Text = "Componente"
                        ListView1.Columns(1).Text = "Versione"
                        OK_Button.Text = "OK"
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
            Case 3
                Text = "Composants du DISM"
                ListView1.Columns(0).Text = "Composant"
                ListView1.Columns(1).Text = "Version"
                OK_Button.Text = "OK"
            Case 4
                Text = "Componentes DISM"
                ListView1.Columns(0).Text = " Componente"
                ListView1.Columns(1).Text = "Versão"
                OK_Button.Text = "OK"
            Case 5
                Text = "Componenti DISM"
                ListView1.Columns(0).Text = "Componente"
                ListView1.Columns(1).Text = "Versione"
                OK_Button.Text = "OK"
        End Select
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Visible = True
        DynaLog.LogMessage("Getting DISM components...")
        For Each DismComponent In My.Computer.FileSystem.GetFiles(Path.GetDirectoryName(Options.TextBox1.Text) & "\dism", FileIO.SearchOption.SearchTopLevelOnly)
            Try
                fv = FileVersionInfo.GetVersionInfo(DismComponent)
                DynaLog.LogMessage("Version of component " & Quote & Path.GetFileName(DismComponent) & Quote & ": " & fv.ProductVersion)
                ListView1.Items.Add(Path.GetFileName(DismComponent)).SubItems.Add(fv.ProductVersion)
            Catch ex As Exception
                Continue For
            End Try
        Next

        ColumnHeader1.Width = WindowHelper.ScaleLogical(250)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(238)
    End Sub
End Class
