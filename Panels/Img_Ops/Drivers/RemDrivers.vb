Imports System.Windows.Forms

Public Class RemDrivers

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub RemDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Remove drivers"
                        Label1.Text = Text
                        Label2.Text = "Specify the driver packages you wish to remove and click OK:"
                        CheckBox1.Text = "Hide boot-critical drivers"
                        CheckBox2.Text = "Hide drivers part of the Windows distribution"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        ListView1.Columns(0).Text = "Published name"
                        ListView1.Columns(1).Text = "Original file name"
                        ListView1.Columns(2).Text = "Provider name"
                        ListView1.Columns(3).Text = "Class name"
                        ListView1.Columns(4).Text = "Part of the Windows distribution?"
                        ListView1.Columns(5).Text = "Is boot-critical?"
                        ListView1.Columns(6).Text = "Version"
                        ListView1.Columns(7).Text = "Date"
                    Case "ESN"
                        Text = "Eliminar controladores"
                        Label1.Text = Text
                        Label2.Text = "Especifique los paquetes de controladores que desea eliminar y haga clic en Aceptar:"
                        CheckBox1.Text = "Ocultar controladores críticos para el arranque"
                        CheckBox2.Text = "Ocultar controladores que son parte de la distribución de Windows"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Nombre publicado"
                        ListView1.Columns(1).Text = "Nombre original del archivo"
                        ListView1.Columns(2).Text = "Nombre del proveedor"
                        ListView1.Columns(3).Text = "Nombre de clase"
                        ListView1.Columns(4).Text = "¿Parte de la distribución de Windows?"
                        ListView1.Columns(5).Text = "¿Es crítico para el arranque?"
                        ListView1.Columns(6).Text = "Versión"
                        ListView1.Columns(7).Text = "Fecha"
                End Select
            Case 1
                Text = "Remove drivers"
                Label1.Text = Text
                Label2.Text = "Specify the driver packages you wish to remove and click OK:"
                CheckBox1.Text = "Hide boot-critical drivers"
                CheckBox2.Text = "Hide drivers part of the Windows distribution"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                ListView1.Columns(0).Text = "Published name"
                ListView1.Columns(1).Text = "Original file name"
                ListView1.Columns(2).Text = "Provider name"
                ListView1.Columns(3).Text = "Class name"
                ListView1.Columns(4).Text = "Part of the Windows distribution?"
                ListView1.Columns(5).Text = "Is boot-critical?"
                ListView1.Columns(6).Text = "Version"
                ListView1.Columns(7).Text = "Date"
            Case 2
                Text = "Eliminar controladores"
                Label1.Text = Text
                Label2.Text = "Especifique los paquetes de controladores que desea eliminar y haga clic en Aceptar:"
                CheckBox1.Text = "Ocultar controladores críticos para el arranque"
                CheckBox2.Text = "Ocultar controladores que son parte de la distribución de Windows"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Nombre publicado"
                ListView1.Columns(1).Text = "Nombre original del archivo"
                ListView1.Columns(2).Text = "Nombre del proveedor"
                ListView1.Columns(3).Text = "Nombre de clase"
                ListView1.Columns(4).Text = "¿Parte de la distribución de Windows?"
                ListView1.Columns(5).Text = "¿Es crítico para el arranque?"
                ListView1.Columns(6).Text = "Versión"
                ListView1.Columns(7).Text = "Fecha"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged, CheckBox2.CheckedChanged
        ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
        End Select
        If Not MainForm.areBackgroundProcessesDone Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(MainForm.imgDrvPublishedNames, MainForm.imgDrvPublishedNames.Last)
                If CheckBox1.Checked Then
                    If MainForm.imgDrvBootCriticalStatus(x) Then Continue For
                End If
                If CheckBox2.Checked Then
                    If CBool(MainForm.imgDrvInbox(x)) Then Continue For
                End If
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                ListView1.Items.Add(New ListViewItem(New String() {MainForm.imgDrvPublishedNames(x), MainForm.imgDrvOGFileNames(x), MainForm.imgDrvProviderNames(x), MainForm.imgDrvClassNames(x), If(CBool(MainForm.imgDrvInbox(x)), "Yes", "No"), If(MainForm.imgDrvBootCriticalStatus(x), "Yes", "No"), MainForm.imgDrvVersions(x), MainForm.imgDrvDates(x)}))
                            Case "ESN"
                                ListView1.Items.Add(New ListViewItem(New String() {MainForm.imgDrvPublishedNames(x), MainForm.imgDrvOGFileNames(x), MainForm.imgDrvProviderNames(x), MainForm.imgDrvClassNames(x), If(CBool(MainForm.imgDrvInbox(x)), "Sí", "No"), If(MainForm.imgDrvBootCriticalStatus(x), "Sí", "No"), MainForm.imgDrvVersions(x), MainForm.imgDrvDates(x)}))
                        End Select
                    Case 1
                        ListView1.Items.Add(New ListViewItem(New String() {MainForm.imgDrvPublishedNames(x), MainForm.imgDrvOGFileNames(x), MainForm.imgDrvProviderNames(x), MainForm.imgDrvClassNames(x), If(CBool(MainForm.imgDrvInbox(x)), "Yes", "No"), If(MainForm.imgDrvBootCriticalStatus(x), "Yes", "No"), MainForm.imgDrvVersions(x), MainForm.imgDrvDates(x)}))
                    Case 2
                        ListView1.Items.Add(New ListViewItem(New String() {MainForm.imgDrvPublishedNames(x), MainForm.imgDrvOGFileNames(x), MainForm.imgDrvProviderNames(x), MainForm.imgDrvClassNames(x), If(CBool(MainForm.imgDrvInbox(x)), "Sí", "No"), If(MainForm.imgDrvBootCriticalStatus(x), "Sí", "No"), MainForm.imgDrvVersions(x), MainForm.imgDrvDates(x)}))
                End Select
            Next
        Catch ex As Exception
            Exit Try
        End Try
    End Sub
End Class
