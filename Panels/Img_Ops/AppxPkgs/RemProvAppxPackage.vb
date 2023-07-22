Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class RemProvAppxPackage

    Public AppxRemovalPackages(65535) As String
    Public AppxRemovalFriendlyNames(65535) As String
    Public AppxRemovalCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        AppxRemovalCount = ListView1.CheckedItems.Count
        ProgressPanel.appxRemovalCount = AppxRemovalCount
        If ListView1.CheckedItems.Count = 0 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            MsgBox("Please specify AppX packages to remove and try again.", vbOKOnly + vbCritical, "Remove provisioned AppX packages")
                        Case "ESN"
                            MsgBox("Especifique paquetes AppX a eliminar e inténtelo de nuevo.", vbOKOnly + vbCritical, "Eliminar paquetes aprovisionados AppX")
                    End Select
                Case 1
                    MsgBox("Please specify AppX packages to remove and try again.", vbOKOnly + vbCritical, "Remove provisioned AppX packages")
                Case 2
                    MsgBox("Especifique paquetes AppX a eliminar e inténtelo de nuevo.", vbOKOnly + vbCritical, "Eliminar paquetes aprovisionados AppX")
            End Select
            Exit Sub
        Else
            If AppxRemovalCount > 65535 Then
                MsgBox("Right now, you can only specify less than 65535 AppX packages. This is a program limitation that will be gone in a future update.", vbOKOnly + vbCritical, "Remove provisioned AppX packages")
                Exit Sub
            Else
                For x = 0 To AppxRemovalCount - 1
                    AppxRemovalPackages(x) = ListView1.CheckedItems(x).Text
                Next
                For x = 0 To AppxRemovalCount - 1
                    AppxRemovalFriendlyNames(x) = ListView1.CheckedItems(x).SubItems(1).Text
                Next
                For x = 0 To AppxRemovalPackages.Length - 1
                    ProgressPanel.appxRemovalPackages(x) = AppxRemovalPackages(x)
                Next
                For x = 0 To AppxRemovalFriendlyNames.Length - 1
                    ProgressPanel.appxRemovalPkgNames(x) = AppxRemovalFriendlyNames(x)
                Next
                ProgressPanel.appxRemovalLastPackage = ListView1.CheckedItems(AppxRemovalCount - 1).ToString().Replace("ListViewItem: {", "").Trim().Replace("}", "").Trim()
            End If
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 38
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub RemProvAppxPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Remove provisioned AppX packages"
                        Label1.Text = Text
                        Label3.Text = "If an application is registered to a user, you will need to run this PowerShell command in order to completely remove it:"
                        Label4.Text = "Remove-AppxPackage -Package <package name>"
                        Label5.Text = "Otherwise, the application will be completely removed. Check the " & Quote & "Registered to any user?" & Quote & " column for more details"
                        LinkLabel1.Text = "How does the program determine whether an application is registered to a user?"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        ListView1.Columns(0).Text = "Package name"
                        ListView1.Columns(1).Text = "Application display name"
                        ListView1.Columns(2).Text = "Architecture"
                        ListView1.Columns(3).Text = "Resource ID"
                        ListView1.Columns(4).Text = "Version"
                        ListView1.Columns(5).Text = "Registered to any user?"
                    Case "ESN"
                        Text = "Eliminar paquetes aprovisionados AppX"
                        Label1.Text = Text
                        Label3.Text = "Si una aplicación está registrada a un usuario, debe ejecutar este comando de PowerShell para eliminarla completamente:"
                        Label4.Text = "Remove-AppxPackage -Package <nombre de paquete>"
                        Label5.Text = "Si no, la aplicación será eliminada completamente. Compruebe la columna " & Quote & "¿Registrada a un usuario?" & Quote & " para más información"
                        LinkLabel1.Text = "¿Cómo determina el programa si una aplicación está registrada a un usuario?"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Nombre de paquete"
                        ListView1.Columns(1).Text = "Nombre de aplicación"
                        ListView1.Columns(2).Text = "Arquitectura"
                        ListView1.Columns(3).Text = "ID de recursos"
                        ListView1.Columns(4).Text = "Versión"
                        ListView1.Columns(5).Text = "¿Registrada a un usuario?"
                End Select
            Case 1
                Text = "Remove provisioned AppX packages"
                Label1.Text = Text
                Label3.Text = "If an application is registered to a user, you will need to run this PowerShell command in order to completely remove it:"
                Label4.Text = "Remove-AppxPackage -Package <package name>"
                Label5.Text = "Otherwise, the application will be completely removed. Check the " & Quote & "Registered to any user?" & Quote & " column for more details"
                LinkLabel1.Text = "How does the program determine whether an application is registered to a user?"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                ListView1.Columns(0).Text = "Package name"
                ListView1.Columns(1).Text = "Application display name"
                ListView1.Columns(2).Text = "Architecture"
                ListView1.Columns(3).Text = "Resource ID"
                ListView1.Columns(4).Text = "Version"
                ListView1.Columns(5).Text = "Registered to any user?"
            Case 2
                Text = "Eliminar paquetes aprovisionados AppX"
                Label1.Text = Text
                Label3.Text = "Si una aplicación está registrada a un usuario, debe ejecutar este comando de PowerShell para eliminarla completamente:"
                Label4.Text = "Remove-AppxPackage -Package <nombre de paquete>"
                Label5.Text = "Si no, la aplicación será eliminada completamente. Compruebe la columna " & Quote & "¿Registrada a un usuario?" & Quote & " para más información"
                LinkLabel1.Text = "¿Cómo determina el programa si una aplicación está registrada a un usuario?"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Nombre de paquete"
                ListView1.Columns(1).Text = "Nombre de aplicación"
                ListView1.Columns(2).Text = "Arquitectura"
                ListView1.Columns(3).Text = "ID de recursos"
                ListView1.Columns(4).Text = "Versión"
                ListView1.Columns(5).Text = "¿Registrada a un usuario?"
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
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
        MainForm.ViewPackageDirectoryToolStripMenuItem.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.openfile_dark, My.Resources.openfile)
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub ListView1_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim item As ListViewItem = ListView1.GetItemAt(e.X, e.Y)
            If item IsNot Nothing Then
                MainForm.AppxPackagePopupCMS.Show(sender, e.Location)
            End If
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            MainForm.ResViewTSMI.Visible = True
            Try
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MainForm.ResViewTSMI.Text = "View resources of " & If(MainForm.GetPackageDisplayName(ListView1.FocusedItem.SubItems(0).Text, ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim()).ToString().StartsWith("ms-resource:", StringComparison.OrdinalIgnoreCase), ListView1.FocusedItem.SubItems(1).Text, MainForm.GetPackageDisplayName(ListView1.FocusedItem.SubItems(0).Text, ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim()))
                            Case "ESN"
                                MainForm.ResViewTSMI.Text = "Ver recursos de " & If(MainForm.GetPackageDisplayName(ListView1.FocusedItem.SubItems(0).Text, ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim()).ToString().StartsWith("ms-resource:", StringComparison.OrdinalIgnoreCase), ListView1.FocusedItem.SubItems(1).Text, MainForm.GetPackageDisplayName(ListView1.FocusedItem.SubItems(0).Text, ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim()))
                        End Select
                    Case 1
                        MainForm.ResViewTSMI.Text = "View resources of " & If(MainForm.GetPackageDisplayName(ListView1.FocusedItem.SubItems(0).Text, ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim()).ToString().StartsWith("ms-resource:", StringComparison.OrdinalIgnoreCase), ListView1.FocusedItem.SubItems(1).Text, MainForm.GetPackageDisplayName(ListView1.FocusedItem.SubItems(0).Text, ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim()))
                    Case 2
                        MainForm.ResViewTSMI.Text = "Ver recursos de " & If(MainForm.GetPackageDisplayName(ListView1.FocusedItem.SubItems(0).Text, ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim()).ToString().StartsWith("ms-resource:", StringComparison.OrdinalIgnoreCase), ListView1.FocusedItem.SubItems(1).Text, MainForm.GetPackageDisplayName(ListView1.FocusedItem.SubItems(0).Text, ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim()))
                End Select
            Catch ex As Exception
                MainForm.ResViewTSMI.Text = ""
                MainForm.ResViewTSMI.Visible = False
            End Try
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Help_RegisteredAppxPkgsDlg.ShowDialog(Me)
    End Sub
End Class
