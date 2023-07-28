Imports System.Windows.Forms

Public Class DisableFeat

    Public featDisablementCount As Integer
    Public featDisablementNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        featDisablementCount = ListView1.CheckedItems.Count
        ProgressPanel.featDisablementCount = featDisablementCount
        If ListView1.CheckedItems.Count <= 0 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            MessageBox.Show(MainForm, "Please select features to disable, and try again.", "No features selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "ESN"
                            MessageBox.Show(MainForm, "Seleccione las características a deshabilitar, e inténtelo de nuevo", "No hay características seleccionadas", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Select
                Case 1
                    MessageBox.Show(MainForm, "Please select features to disable, and try again.", "No features selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 2
                    MessageBox.Show(MainForm, "Seleccione las características a deshabilitar, e inténtelo de nuevo", "No hay características seleccionadas", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select
            Exit Sub
        Else
            Try
                For x = 0 To featDisablementCount - 1
                    featDisablementNames(x) = ListView1.CheckedItems(x).ToString()
                Next
                For x = 0 To featDisablementNames.Length
                    ProgressPanel.featDisablementNames(x) = featDisablementNames(x)
                Next
            Catch ex As Exception

            End Try
            ProgressPanel.featDisablementLastName = ListView1.CheckedItems(featDisablementCount - 1).ToString()
            If CheckBox1.Checked Then
                ProgressPanel.featDisablementParentPkgUsed = True
                ProgressPanel.featDisablementParentPkg = TextBox1.Text
            Else
                ProgressPanel.featDisablementParentPkgUsed = False
                ProgressPanel.featDisablementParentPkg = ""
            End If
            If CheckBox2.Checked Then
                ProgressPanel.featRemoveManifest = False
            Else
                ProgressPanel.featRemoveManifest = True
            End If
        End If
        ProgressPanel.OperationNum = 31
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DisableFeat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Disable features"
                        Label1.Text = Text
                        Label3.Text = "Package name:"
                        GroupBox1.Text = "Features"
                        GroupBox2.Text = "Options"
                        Button1.Text = "Lookup..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        ListView1.Columns(0).Text = "Feature name"
                        ListView1.Columns(1).Text = "State"
                        CheckBox1.Text = "Specify parent package name for features"
                        CheckBox2.Text = "Remove feature without removing manifest"
                    Case "ESN"
                        Text = "Deshabilitar características"
                        Label1.Text = Text
                        Label3.Text = "Paquete:"
                        GroupBox1.Text = "Características"
                        GroupBox2.Text = "Opciones"
                        Button1.Text = "Consultar"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Nombre de característica"
                        ListView1.Columns(1).Text = "Estado"
                        CheckBox1.Text = "Especificar nombre de paquete principal para las características"
                        CheckBox2.Text = "Eliminar característica sin eliminar manifiesto"
                End Select
            Case 1
                Text = "Disable features"
                Label1.Text = Text
                Label3.Text = "Package name:"
                GroupBox1.Text = "Features"
                GroupBox2.Text = "Options"
                Button1.Text = "Lookup..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                ListView1.Columns(0).Text = "Feature name"
                ListView1.Columns(1).Text = "State"
                CheckBox1.Text = "Specify parent package name for features"
                CheckBox2.Text = "Remove feature without removing manifest"
            Case 2
                Text = "Deshabilitar características"
                Label1.Text = Text
                Label3.Text = "Paquete:"
                GroupBox1.Text = "Características"
                GroupBox2.Text = "Opciones"
                Button1.Text = "Consultar"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Nombre de característica"
                ListView1.Columns(1).Text = "Estado"
                CheckBox1.Text = "Especificar nombre de paquete principal para las características"
                CheckBox2.Text = "Eliminar característica sin eliminar manifiesto"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label2.Text &= " Only enabled features (" & ListView1.Items.Count & ") are shown"
                    Case "ESN"
                        Label2.Text &= " Solo las características habilitadas (" & ListView1.Items.Count & ") son mostradas"
                End Select
            Case 1
                Label2.Text &= " Only enabled features (" & ListView1.Items.Count & ") are shown"
            Case 2
                Label2.Text &= " Solo las características habilitadas (" & ListView1.Items.Count & ") son mostradas"
        End Select
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label3.Enabled = True
            Button1.Enabled = True
        Else
            Label3.Enabled = False
            Button1.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PkgParentNameLookupDlg.pkgSource = MainForm.MountDir
        PkgParentNameLookupDlg.ShowDialog(Me)
    End Sub
End Class
