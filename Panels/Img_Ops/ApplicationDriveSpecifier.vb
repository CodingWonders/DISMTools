Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports System.Threading
Imports System.Management
Imports DISMTools.Utilities

Public Class ApplicationDriveSpecifier

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ImgApply.TextBox3.Text = TextBox1.Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Sub ListDisks()
        ListView1.Items.Clear()
        Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher("SELECT DeviceID, Model, Partitions, Size FROM Win32_DiskDrive")
        Dim dskResults As ManagementObjectCollection = searcher.Get()
        For Each result As ManagementObject In dskResults
            ListView1.Items.Add(New ListViewItem(New String() {result("DeviceID"), result("Model"), result("Partitions"), result("Size") & " (~" & Converters.BytesToReadableSize(result("Size")) & ")"}))
        Next
    End Sub

    Private Sub ApplicationDriveSpecifier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Specify target disk..."
                        Label2.Text = "Destination disk ID (\\.\PHYSICALDRIVE(n)):"
                        Button2.Text = "Refresh"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        ListView1.Columns(0).Text = "Device ID"
                        ListView1.Columns(1).Text = "Model"
                        ListView1.Columns(2).Text = "Partitions"
                        ListView1.Columns(3).Text = "Size"
                    Case "ESN"
                        Text = "Especificar disco de destino..."
                        Label2.Text = "ID de disco (\\.\PHYSICALDRIVE(n)):"
                        Button2.Text = "Actualizar"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "ID de dispositivo"
                        ListView1.Columns(1).Text = "Modelo"
                        ListView1.Columns(2).Text = "Particiones"
                        ListView1.Columns(3).Text = "Tamaño"
                    Case "FRA"
                        Text = "Spécifier le disque cible..."
                        Label2.Text = "ID de disque de destination (\\.\PHYSICALDRIVE(n)):"
                        Button2.Text = "Rafraîchir"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        ListView1.Columns(0).Text = "ID de l'appareil"
                        ListView1.Columns(1).Text = "Modèle"
                        ListView1.Columns(2).Text = "Partitions"
                        ListView1.Columns(3).Text = "Taille"
                End Select
            Case 1
                Text = "Specify target disk..."
                Label2.Text = "Destination disk ID (\\.\PHYSICALDRIVE(n)):"
                Button2.Text = "Refresh"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                ListView1.Columns(0).Text = "Device ID"
                ListView1.Columns(1).Text = "Model"
                ListView1.Columns(2).Text = "Partitions"
                ListView1.Columns(3).Text = "Size"
            Case 2
                Text = "Especificar disco de destino..."
                Label2.Text = "ID de disco (\\.\PHYSICALDRIVE(n)):"
                Button2.Text = "Actualizar"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "ID de dispositivo"
                ListView1.Columns(1).Text = "Modelo"
                ListView1.Columns(2).Text = "Particiones"
                ListView1.Columns(3).Text = "Tamaño"
            Case 3
                Text = "Spécifier le disque cible..."
                Label2.Text = "ID de disque de destination (\\.\PHYSICALDRIVE(n)):"
                Button2.Text = "Rafraîchir"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                ListView1.Columns(0).Text = "ID de l'appareil"
                ListView1.Columns(1).Text = "Modèle"
                ListView1.Columns(2).Text = "Partitions"
                ListView1.Columns(3).Text = "Taille"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            RichTextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            RichTextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        ListDisks()
        BringToFront()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListDisks()
    End Sub

    Private Sub RichTextBox1_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox1.LinkClicked
        TextBox1.Text = e.LinkText
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        TextBox1.Text = ListView1.FocusedItem.SubItems(0).Text
    End Sub
End Class
