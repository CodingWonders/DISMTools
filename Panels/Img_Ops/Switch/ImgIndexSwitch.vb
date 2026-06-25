Imports System.Windows.Forms

Public Class ImgIndexSwitch
    Implements IImageTaskDialog

    Public indexNames(1024) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        DynaLog.LogMessage("Preparing to switch image indexes...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.SwitchSourceImg = MainForm.SourceImg
        ProgressPanel.SwitchTarget = MainForm.MountDir
        ProgressPanel.SwitchSourceIndex = MainForm.ImgIndex
        ProgressPanel.SwitchTargetIndex = NumericUpDown1.Value
        ProgressPanel.SwitchTargetIndexName = Label5.Text
        If RadioButton1.Checked Then
            ProgressPanel.SwitchCommitSourceIndex = True
        Else
            ProgressPanel.SwitchCommitSourceIndex = False
        End If
        If MainForm.isReadOnly Then
            ProgressPanel.SwitchMountAsReadOnly = True
        Else
            ProgressPanel.SwitchMountAsReadOnly = False
        End If
        ProgressPanel.OperationNum = 996
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Opening image index switch dialog...")
        DynaLog.LogMessage("Stopping mounted image detector...")
        MainForm.StopMountedImageDetector()
        DynaLog.LogMessage("Getting image indexes...")
        ProgressPanel.OperationNum = 995
        PleaseWaitDialog.indexesSourceImg = MainForm.SourceImg
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting image indexes..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo índices de la imagen..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des index de l'image en cours..."
                    Case "PTB", "PTG"
                        PleaseWaitDialog.Label2.Text = "Obter índices de imagem..."
                    Case "ITA"
                        PleaseWaitDialog.Label2.Text = "Ottenere gli indici delle immagini..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting image indexes..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo índices de la imagen..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des index de l'image en cours..."
            Case 4
                PleaseWaitDialog.Label2.Text = "Obter índices de imagem..."
            Case 5
                PleaseWaitDialog.Label2.Text = "Ottenere gli indici delle immagini..."
        End Select
        PleaseWaitDialog.ShowDialog(Me)
        MainForm.StartMountedImageDetector()
        Return (PleaseWaitDialog.imgIndexes > 1)
    End Function

    Private Sub ImgIndexSwitch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Switch image indexes"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Image:"
                        Label3.Text = "When unmounting source index, what to do?"
                        Label4.Text = "Destination index to mount:"
                        Label6.Text = "This index has already been mounted"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        GroupBox1.Text = "Indexes"
                        RadioButton1.Text = "Save changes to index"
                        RadioButton2.Text = "Unmount discarding changes"
                    Case "ESN"
                        Text = "Cambiar índices de imagen"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Imagen:"
                        Label3.Text = "Al desmontar índice de origen, ¿qué hacer?"
                        Label4.Text = "Índice de destino a montar:"
                        Label6.Text = "Este índice ya está montado"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox1.Text = "Índice"
                        RadioButton1.Text = "Guardar cambios en el índice"
                        RadioButton2.Text = "Desmontar descartando cambios"
                    Case "FRA"
                        Text = "Changer d'index de l'image"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Image :"
                        Label3.Text = "Que faire lors du démonter l'index original ?"
                        Label4.Text = "Index de destination à monter :"
                        Label6.Text = "Cet index a déjà été monté"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        GroupBox1.Text = "Index"
                        RadioButton1.Text = "Sauvegarder les modifications dans l'index"
                        RadioButton2.Text = "Annuler les modifications et démonter"
                    Case "PTB", "PTG"
                        Text = "Mudar os índices de imagem"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Imagem:"
                        Label3.Text = "Quando desmontar o índice de origem, o que fazer?"
                        Label4.Text = "Índice de destino para montar:"
                        Label6.Text = "Este índice já foi montado"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox1.Text = "Índices"
                        RadioButton1.Text = "Guardar alterações no índice"
                        RadioButton2.Text = "Desmontar descartando as alterações"
                    Case "ITA"
                        Text = "Cambia gli indici delle immagini"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Immagine:"
                        Label3.Text = "Quando si smonta l'indice sorgente, cosa fare?"
                        Label4.Text = "Indice di destinazione da montare:"
                        Label6.Text = "Questo indice è già stato montato"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        GroupBox1.Text = "Indici"
                        RadioButton1.Text = "Salva le modifiche all'indice"
                        RadioButton2.Text = "Smonta scartando le modifiche"
                End Select
            Case 1
                Text = "Switch image indexes"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Image:"
                Label3.Text = "When unmounting source index, what to do?"
                Label4.Text = "Destination index to mount:"
                Label6.Text = "This index has already been mounted"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                GroupBox1.Text = "Indexes"
                RadioButton1.Text = "Save changes to index"
                RadioButton2.Text = "Unmount discarding changes"
            Case 2
                Text = "Cambiar índices de imagen"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Imagen:"
                Label3.Text = "Al desmontar índice de origen, ¿qué hacer?"
                Label4.Text = "Índice de destino a montar:"
                Label6.Text = "Este índice ya está montado"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                GroupBox1.Text = "Índice"
                RadioButton1.Text = "Guardar cambios en el índice"
                RadioButton2.Text = "Desmontar descartando cambios"
            Case 3
                Text = "Changer d'index de l'image"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Image :"
                Label3.Text = "Que faire lors du démonter l'index original ?"
                Label4.Text = "Index de destination à monter :"
                Label6.Text = "Cet index a déjà été monté"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                GroupBox1.Text = "Index"
                RadioButton1.Text = "Sauvegarder les modifications dans l'index"
                RadioButton2.Text = "Annuler les modifications et démonter"
            Case 4
                Text = "Mudar os índices de imagem"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Imagem:"
                Label3.Text = "Quando desmontar o índice de origem, o que fazer?"
                Label4.Text = "Índice de destino para montar:"
                Label6.Text = "Este índice já foi montado"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                GroupBox1.Text = "Índices"
                RadioButton1.Text = "Guardar alterações no índice"
                RadioButton2.Text = "Desmontar descartando as alterações"
            Case 5
                Text = "Cambia gli indici delle immagini"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Immagine:"
                Label3.Text = "Quando si smonta l'indice sorgente, cosa fare?"
                Label4.Text = "Indice di destinazione da montare:"
                Label6.Text = "Questo indice è già stato montato"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                GroupBox1.Text = "Indici"
                RadioButton1.Text = "Salva le modifiche all'indice"
                RadioButton2.Text = "Smonta scartando le modifiche"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Label5.Text = indexNames(NumericUpDown1.Value - 1)
        If Label5.Text = MainForm.CurrentImage.ImageName Then
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        Label5.Text = indexNames(NumericUpDown1.Value - 1)
        If Label5.Text = MainForm.CurrentImage.ImageName Then
            DynaLog.LogMessage("The index target is already mounted.")
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
    End Sub
End Class
