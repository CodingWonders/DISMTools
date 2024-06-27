Imports System.Windows.Forms
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars

Public Class PkgParentNameLookupDlg

    Public pkgSource As String
    Public pkgArgs As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextBox1.Text = "" Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("Please specify a package name, and try again.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Installed package names")
                        Case "ESN"
                            MsgBox("Especifique un nombre de paquete, e inténtelo de nuevo.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nombres de paquetes instalados")
                        Case "FRA"
                            MsgBox("Veuillez spécifier un nom de paquet et réessayer.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Noms des paquets installés")
                        Case "PTB", "PTG"
                            MsgBox("Especifique um nome de pacote e tente novamente.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nomes dos pacotes instalados")
                        Case "ITA"
                            MsgBox("Specificare il nome di un pacchetto e riprovare", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nomi dei pacchetti installati")
                    End Select
                Case 1
                    MsgBox("Please specify a package name, and try again.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Installed package names")
                Case 2
                    MsgBox("Especifique un nombre de paquete, e inténtelo de nuevo.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nombres de paquetes instalados")
                Case 3
                    MsgBox("Veuillez spécifier un nom de paquet et réessayer.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Noms des paquets installés")
                Case 4
                    MsgBox("Especifique um nome de pacote e tente novamente.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nomes dos pacotes instalados")
                Case 5
                    MsgBox("Specificare il nome di un pacchetto e riprovare", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nomi dei pacchetti installati")
            End Select
            Exit Sub
        ElseIf Not ListBox1.Items.Contains(TextBox1.Text) Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("The specified package name does not seem to be in the image. Please specify an available entry, and try again", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Installed package names")
                        Case "ESN"
                            MsgBox("El paquete especificado no parece estar en la imagen. Especifique una entrada disponible, e inténtelo de nuevo", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nombres de paquetes instalados")
                        Case "FRA"
                            MsgBox("Le nom du paquet spécifié ne semble pas figurer dans l'image. Veuillez spécifier une entrée disponible et réessayer", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Noms des paquets installés")
                        Case "PTB", "PTG"
                            MsgBox("O nome do pacote especificado não parece estar na imagem. Especifique uma entrada disponível e tente novamente", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nomes dos pacotes instalados")
                        Case "ITA"
                            MsgBox("Il nome del pacchetto specificato non sembra essere presente nell'immagine. Si prega di specificare una voce disponibile e di riprovare", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nomi di pacchetti installati")
                    End Select
                Case 1
                    MsgBox("The specified package name does not seem to be in the image. Please specify an available entry, and try again", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Installed package names")
                Case 2
                    MsgBox("El paquete especificado no parece estar en la imagen. Especifique una entrada disponible, e inténtelo de nuevo", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nombres de paquetes instalados")
                Case 3
                    MsgBox("Le nom du paquet spécifié ne semble pas figurer dans l'image. Veuillez spécifier une entrée disponible et réessayer", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Noms des paquets installés")
                Case 4
                    MsgBox("O nome do pacote especificado não parece estar na imagem. Especifique uma entrada disponível e tente novamente", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nomes dos pacotes instalados")
                Case 5
                    MsgBox("Il nome del pacchetto specificato non sembra essere presente nell'immagine. Si prega di specificare una voce disponibile e di riprovare", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Nomi di pacchetti installati")
            End Select
            Exit Sub
        Else
            EnableFeat.TextBox1.Text = TextBox1.Text
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        TextBox1.Text = ListBox1.SelectedItem
    End Sub

    Private Sub PkgParentNameLookupDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Installed package names"
                        Label1.Text = "Names of installed packages in the mounted image:"
                        Label2.Text = "Name of parent package:"
                        Label3.Text = "Getting package names. Please wait..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Nombres de paquetes instalados"
                        Label1.Text = "Nombres de paquetes instalados en la imagen montada:"
                        Label2.Text = "Paquete principal:"
                        Label3.Text = "Obteniendo nombres de paquetes. Espere..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Noms des paquets installés"
                        Label1.Text = "Noms des paquets installés dans l'image montée :"
                        Label2.Text = "Nom du paquet parent :"
                        Label3.Text = "Obtention des noms des paquets en cours. Veuillez patienter..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Text = "Nomes dos pacotes instalados"
                        Label1.Text = "Nomes dos pacotes instalados na imagem montada:"
                        Label2.Text = "Nome do pacote pai:"
                        Label3.Text = "A obter nomes de pacotes. Aguarde..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                    Case "ITA"
                        Text = "Nomi dei pacchetti installati"
                        Label1.Text = "Nomi dei pacchetti installati nell'immagine montata:"
                        Label2.Text = "Nome del pacchetto padre:"
                        Label3.Text = "Ottenere i nomi dei pacchetti. Attendere prego..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                End Select
            Case 1
                Text = "Installed package names"
                Label1.Text = "Names of installed packages in the mounted image:"
                Label2.Text = "Name of parent package:"
                Label3.Text = "Getting package names. Please wait..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Nombres de paquetes instalados"
                Label1.Text = "Nombres de paquetes instalados en la imagen montada:"
                Label2.Text = "Paquete principal:"
                Label3.Text = "Obteniendo nombres de paquetes. Espere..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Noms des paquets installés"
                Label1.Text = "Noms des paquets installés dans l'image montée :"
                Label2.Text = "Nom du paquet parent :"
                Label3.Text = "Obtention des noms des paquets en cours. Veuillez patienter..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case 4
                Text = "Nomes dos pacotes instalados"
                Label1.Text = "Nomes dos pacotes instalados na imagem montada:"
                Label2.Text = "Nome do pacote pai:"
                Label3.Text = "A obter nomes de pacotes. Aguarde..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
            Case 5
                Text = "Nomi dei pacchetti installati"
                Label1.Text = "Nomi dei pacchetti installati nell'immagine montata:"
                Label2.Text = "Nome del pacchetto padre:"
                Label3.Text = "Ottenere i nomi dei pacchetti. Attendere prego..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Control.CheckForIllegalCrossThreadCalls = False
        Label3.Visible = True
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
        ListBox1.Items.Clear()
        For x = 0 To MainForm.imgPackageNames.Length - 1
            If MainForm.imgPackageNames(x) = "" Then
                Continue For
            ElseIf MainForm.imgPackageNames(x) = Nothing Then
                Exit For
            Else
                ListBox1.Items.Add(MainForm.imgPackageNames(x))
            End If
        Next
        Label3.Visible = False
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub PackageListerBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles PackageListerBW.DoWork
        File.WriteAllText(Application.StartupPath & "\temp.bat",
                  "@echo off" & CrLf &
                  "dism /English /image=" & pkgSource & " /get-packages | findstr /c:" & Quote & "Package Identity : " & Quote & " > .\pkgnames")
        pkgProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        pkgArgs = "/c " & Application.StartupPath & "\temp.bat"
        pkgProc.StartInfo.Arguments = pkgArgs
        pkgProc.StartInfo.CreateNoWindow = True
        pkgProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        pkgProc.Start()
        pkgProc.WaitForExit()
        If Decimal.ToInt32(pkgProc.ExitCode) = 0 Then
            RemPackage.CheckedListBox1.Items.Clear()
            RemPackage.CheckedListBox2.Items.Clear()
            Debug.WriteLine("[INFO] Package names were successfully gathered. The program should return to normal state")
            Debug.WriteLine("Listing package names:" & CrLf & My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\pkgnames"))
            Dim pkgNames As New RichTextBox
            pkgNames.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\pkgnames")
            For x = 0 To pkgNames.Lines.Count - 1
                If pkgNames.Lines(x) = "" Then
                    Continue For
                End If
                ListBox1.Items.Add(pkgNames.Lines(x).Replace("Package Identity : ", "").Trim())
            Next
            File.Delete(Application.StartupPath & "\temp.bat")
            File.Delete(Application.StartupPath & "\pkgnames")
        Else
            Debug.WriteLine("[FAIL] Package names were not gathered. Please verify everything's working")
        End If
        Label3.Visible = False
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
    End Sub
End Class
