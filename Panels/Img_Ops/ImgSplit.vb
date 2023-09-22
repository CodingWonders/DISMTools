Imports System.Windows.Forms
Imports System.IO

Public Class ImgSplit

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            ProgressPanel.SWMSplitSourceFile = TextBox1.Text
            ProgressPanel.SWMSplitFileSize = NumericUpDown1.Value
            If TextBox2.Text <> "" And Directory.Exists(Path.GetDirectoryName(TextBox2.Text)) Then
                ProgressPanel.SWMSplitTargetFile = TextBox2.Text
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MsgBox("Please specify a name and path for the target SWM file and try again. Also, make sure that the target path exists.", vbOKOnly + vbCritical, Label1.Text)
                            Case "ESN"
                                MsgBox("Especifique un nombre y un directorio para el archivo SWM de destino e inténtelo de nuevo. Asegúrese también de que el directorio de destino exista.", vbOKOnly + vbCritical, Label1.Text)
                        End Select
                    Case 1
                        MsgBox("Please specify a name and path for the target SWM file and try again. Also, make sure that the target path exists.", vbOKOnly + vbCritical, Label1.Text)
                    Case 2
                        MsgBox("Especifique un nombre y un directorio para el archivo SWM de destino e inténtelo de nuevo. Asegúrese también de que el directorio de destino exista.", vbOKOnly + vbCritical, Label1.Text)
                End Select
                Exit Sub
            End If
            ProgressPanel.SWMSplitCheckIntegrity = CheckBox1.Checked
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("Please specify a source WIM file and try again. Also, make sure that it exists.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ESN"
                            MsgBox("Especifique un archivo WIM de origen e inténtelo de nuevo. Asegúrese también de que el archivo exista.", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                Case 1
                    MsgBox("Please specify a source WIM file and try again. Also, make sure that it exists.", vbOKOnly + vbCritical, Label1.Text)
                Case 2
                    MsgBox("Especifique un archivo WIM de origen e inténtelo de nuevo. Asegúrese también de que el archivo exista.", vbOKOnly + vbCritical, Label1.Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.OperationNum = 20
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgSplit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Split images"
                        Label1.Text = Text
                        Label2.Text = "Source image to split:"
                        Label3.Text = "Name and path of the destination split image:"
                        Label4.Text = "Maximum size of split images (in MB):"
                        Label5.Text = "Do note that, to accommodate a large file in the image, a split image file may be larger than the specified value"
                        Button1.Text = "Browse..."
                        Button2.Text = "Browse..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        CheckBox1.Text = "Check image integrity"
                        OpenFileDialog1.Title = "Specify the source WIM file to split:"
                        SaveFileDialog1.Title = "Specify the target location of the split images:"
                    Case "ESN"
                        Text = "Dividir imágenes"
                        Label1.Text = Text
                        Label2.Text = "Imagen de origen a dividir:"
                        Label3.Text = "Nombre y ruta de la imagen dividida de destino:"
                        Label4.Text = "Tamaño máximo de imágenes divididas (en MB):"
                        Label5.Text = "Para acomodar un archivo grande de la imagen, una imagen dividida puede ocupar más tamaño del especificado"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Examinar..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        CheckBox1.Text = "Comprobar integridad de la imagen"
                        OpenFileDialog1.Title = "Especifique el archivo WIM de origen a dividir:"
                        SaveFileDialog1.Title = "Especifique la ubicación de destino de las imágenes divididas:"
                End Select
            Case 1
                Text = "Split images"
                Label1.Text = Text
                Label2.Text = "Source image to split:"
                Label3.Text = "Name and path of the destination split image:"
                Label4.Text = "Maximum size of split images (in MB):"
                Label5.Text = "Do note that, to accommodate a large file in the image, a split image file may be larger than the specified value"
                Button1.Text = "Browse..."
                Button2.Text = "Browse..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                CheckBox1.Text = "Check image integrity"
                OpenFileDialog1.Title = "Specify the source WIM file to split:"
                SaveFileDialog1.Title = "Specify the target location of the split images:"
            Case 2
                Text = "Dividir imágenes"
                Label1.Text = Text
                Label2.Text = "Imagen de origen a dividir:"
                Label3.Text = "Nombre y ruta de la imagen dividida de destino:"
                Label4.Text = "Tamaño máximo de imágenes divididas (en MB):"
                Label5.Text = "Para acomodar un archivo grande de la imagen, una imagen dividida puede ocupar más tamaño del especificado"
                Button1.Text = "Examinar..."
                Button2.Text = "Examinar..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                CheckBox1.Text = "Comprobar integridad de la imagen"
                OpenFileDialog1.Title = "Especifique el archivo WIM de origen a dividir:"
                SaveFileDialog1.Title = "Especifique la ubicación de destino de las imágenes divididas:"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            NumericUpDown1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            NumericUpDown1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(TextBox1.Text) & "_"
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub
End Class
