Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism

Public Class PopupMountedImagePicker

    Private Shared mountedImages As DismMountedImageInfoCollection
    Private Shared focusedIndex As Integer

    Public Shared Function PickImage(position As Point, Optional showUpwards As Boolean = False) As DismMountedImageInfo
        Dim pmipForm As Form = New Form With {
            .Location = position,
            .Size = New Size(800, 376),
            .FormBorderStyle = FormBorderStyle.None,
            .StartPosition = FormStartPosition.Manual,
            .ControlBox = False,
            .Font = New Font("Tahoma", 8.25F),
            .KeyPreview = True,
            .BackColor = CurrentTheme.SectionBackgroundColor,
            .ForeColor = CurrentTheme.ForegroundColor,
            .Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location)
        }
        If showUpwards Then pmipForm.Top -= pmipForm.Height
        Dim pmipInstructionLabel As Label = New Label With {
            .Location = New Point(10, 10),
            .AutoSize = True
        }
        Dim pmipMountedImageList As ListView = New ListView With {
            .Location = New Point(12, 32),
            .Size = New Size(760, 260),
            .BackColor = pmipForm.BackColor,
            .ForeColor = pmipForm.ForeColor,
            .Anchor = CType((AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles),
            .FullRowSelect = True,
            .MultiSelect = False,
            .View = View.Details
        }
        Dim pmipOkButton As Button = New Button With {
            .Location = New Point(616, 300),
            .Size = New Size(75, 23),
            .Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles),
            .FlatStyle = FlatStyle.System,
            .Enabled = False
        }
        Dim pmipCancelButton As Button = New Button With {
            .Location = New Point(698, 300),
            .Size = New Size(75, 23),
            .Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles),
            .FlatStyle = FlatStyle.System
        }
        pmipMountedImageList.Columns.AddRange(New ColumnHeader() {
                                              New ColumnHeader With {
                                                  .Width = 434
                                              },
                                              New ColumnHeader With {
                                                  .Width = 72
                                              },
                                              New ColumnHeader With {
                                                  .Width = 374
                                              }
                                             })
        pmipForm.AcceptButton = pmipOkButton
        ' Add controls to form
        pmipForm.Controls.AddRange(New Control() {pmipInstructionLabel, pmipMountedImageList, pmipOkButton, pmipCancelButton})
        pmipOkButton.BringToFront()
        pmipCancelButton.BringToFront()

        ' Event Handlers
        AddHandler pmipMountedImageList.SelectedIndexChanged, Sub(sender, e)
                                                                  pmipOkButton.Enabled = (pmipMountedImageList.SelectedItems.Count = 1)
                                                                  If pmipMountedImageList.SelectedItems.Count = 1 Then
                                                                      focusedIndex = pmipMountedImageList.FocusedItem.Index
                                                                  End If
                                                              End Sub
        AddHandler pmipForm.KeyDown, Sub(sender, e)
                                         If e.KeyCode = Keys.Escape Then
                                             pmipCancelButton.PerformClick()
                                         ElseIf e.KeyCode = Keys.Enter Then
                                             If pmipMountedImageList.SelectedItems.Count <> 1 Then
                                                 e.SuppressKeyPress = True
                                             End If
                                         End If
                                     End Sub
        AddHandler pmipForm.Paint, Sub(sender, e)
                                       ControlPaint.DrawBorder(e.Graphics, pmipForm.ClientRectangle, Color.FromArgb(53, 153, 41), ButtonBorderStyle.Solid)
                                   End Sub
        AddHandler pmipOkButton.Click, Sub(sender, e)
                                           pmipForm.DialogResult = DialogResult.OK
                                           pmipForm.Close()
                                       End Sub
        AddHandler pmipCancelButton.Click, Sub(sender, e)
                                               pmipForm.DialogResult = DialogResult.Cancel
                                               pmipForm.Close()
                                           End Sub

        ' Translate
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        pmipForm.Text = "Pick image"
                        pmipOkButton.Text = "OK"
                        pmipCancelButton.Text = "Cancel"
                        pmipInstructionLabel.Text = "Pick an image from the list below:"
                        pmipMountedImageList.Columns(0).Text = "Image file"
                        pmipMountedImageList.Columns(1).Text = "Index"
                        pmipMountedImageList.Columns(2).Text = "Mount directory"
                    Case "ESN"
                        pmipForm.Text = "Escoger imagen"
                        pmipOkButton.Text = "Aceptar"
                        pmipCancelButton.Text = "Cancelar"
                        pmipInstructionLabel.Text = "Escoja una imagen de la lista de abajo:"
                        pmipMountedImageList.Columns(0).Text = "Archivo de imagen"
                        pmipMountedImageList.Columns(1).Text = "Índice"
                        pmipMountedImageList.Columns(2).Text = "Directorio de montaje"
                    Case "FRA"
                        pmipForm.Text = "Choisir l'image"
                        pmipOkButton.Text = "OK"
                        pmipCancelButton.Text = "Annuler"
                        pmipInstructionLabel.Text = "Choisissez une image dans la liste ci-dessous :"
                        pmipMountedImageList.Columns(0).Text = "Fichier de l'image"
                        pmipMountedImageList.Columns(1).Text = "Index"
                        pmipMountedImageList.Columns(2).Text = "Répertoire de montage"
                    Case "PTB", "PTG"
                        pmipForm.Text = "Escolher imagem"
                        pmipOkButton.Text = "OK"
                        pmipCancelButton.Text = "Cancelar"
                        pmipInstructionLabel.Text = "Escolher uma imagem da lista abaixo:"
                        pmipMountedImageList.Columns(0).Text = "Ficheiro de imagem"
                        pmipMountedImageList.Columns(1).Text = "Índice"
                        pmipMountedImageList.Columns(2).Text = "Diretório de montagem"
                    Case "ITA"
                        pmipForm.Text = "Scegli immagine"
                        pmipOkButton.Text = "OK"
                        pmipCancelButton.Text = "Annulla"
                        pmipInstructionLabel.Text = "Scegli un'immagine dall'elenco sottostante:"
                        pmipMountedImageList.Columns(0).Text = "File immagine"
                        pmipMountedImageList.Columns(1).Text = "Indice"
                        pmipMountedImageList.Columns(2).Text = "Directory di montaggio"
                End Select
            Case 1
                pmipForm.Text = "Pick image"
                pmipOkButton.Text = "OK"
                pmipCancelButton.Text = "Cancel"
                pmipInstructionLabel.Text = "Pick an image from the list below:"
                pmipMountedImageList.Columns(0).Text = "Image file"
                pmipMountedImageList.Columns(1).Text = "Index"
                pmipMountedImageList.Columns(2).Text = "Mount directory"
            Case 2
                pmipForm.Text = "Escoger imagen"
                pmipOkButton.Text = "Aceptar"
                pmipCancelButton.Text = "Cancelar"
                pmipInstructionLabel.Text = "Escoja una imagen de la lista de abajo:"
                pmipMountedImageList.Columns(0).Text = "Archivo de imagen"
                pmipMountedImageList.Columns(1).Text = "Índice"
                pmipMountedImageList.Columns(2).Text = "Directorio de montaje"
            Case 3
                pmipForm.Text = "Choisir l'image"
                pmipOkButton.Text = "OK"
                pmipCancelButton.Text = "Annuler"
                pmipInstructionLabel.Text = "Choisissez une image dans la liste ci-dessous :"
                pmipMountedImageList.Columns(0).Text = "Fichier de l'image"
                pmipMountedImageList.Columns(1).Text = "Index"
                pmipMountedImageList.Columns(2).Text = "Répertoire de montage"
            Case 4
                pmipForm.Text = "Escolher imagem"
                pmipOkButton.Text = "OK"
                pmipCancelButton.Text = "Cancelar"
                pmipInstructionLabel.Text = "Escolher uma imagem da lista abaixo:"
                pmipMountedImageList.Columns(0).Text = "Ficheiro de imagem"
                pmipMountedImageList.Columns(1).Text = "Índice"
                pmipMountedImageList.Columns(2).Text = "Diretório de montagem"
            Case 5
                pmipForm.Text = "Scegli immagine"
                pmipOkButton.Text = "OK"
                pmipCancelButton.Text = "Annulla"
                pmipInstructionLabel.Text = "Scegli un'immagine dall'elenco sottostante:"
                pmipMountedImageList.Columns(0).Text = "File immagine"
                pmipMountedImageList.Columns(1).Text = "Indice"
                pmipMountedImageList.Columns(2).Text = "Directory di montaggio"
        End Select

        GetMountedImages()
        If mountedImages IsNot Nothing Then
            For Each mountedImage As DismMountedImageInfo In mountedImages
                pmipMountedImageList.Items.Add(New ListViewItem(New String() {mountedImage.ImageFilePath,
                                                                              mountedImage.ImageIndex,
                                                                              mountedImage.MountPath}))
            Next
        End If

        Return If(pmipForm.ShowDialog() = DialogResult.OK, mountedImages(focusedIndex), Nothing)
    End Function

    Private Shared Sub GetMountedImages()
        Try
            DynaLog.LogMessage("Preparing to get mounted images...")
            MainForm.StopMountedImageDetector()
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            DynaLog.LogMessage("Getting mounted images...")
            mountedImages = DismApi.GetMountedImages()
        Catch ex As Exception
            DynaLog.LogMessage("Could not get mounted images. Error message: " & ex.Message)
            MsgBox(ex.Message, vbOKOnly + vbCritical, "")
        Finally
            Try
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As Exception
                ' Do nothing
            End Try
        End Try
        MainForm.StartMountedImageDetector()
    End Sub

End Class
