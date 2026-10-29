Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism

Public Class PopupMountedImagePicker

    Private Shared mountedImages As New List(Of WindowsImage)
    Private Shared focusedIndex As Integer

    Public Shared Function PickImage(ParamArray requiredExtensions() As String) As WindowsImage
        Dim pmipForm As Form = New Form With {
            .Size = WindowHelper.ScaleSizeLogical(800, 376),
            .FormBorderStyle = FormBorderStyle.None,
            .StartPosition = FormStartPosition.CenterScreen,
            .ControlBox = False,
            .Font = New Font("Tahoma", 8.25F),
            .KeyPreview = True,
            .BackColor = CurrentTheme.SectionBackgroundColor,
            .ForeColor = CurrentTheme.ForegroundColor,
            .Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
            .AutoScaleMode = AutoScaleMode.Dpi
        }
        Dim pmipInstructionLabel As Label = New Label With {
            .Location = WindowHelper.ScalePositionLogical(10, 10),
            .AutoSize = True
        }
        Dim pmipMountedImageList As ListView = New ListView With {
            .Location = WindowHelper.ScalePositionLogical(12, 32),
            .Size = WindowHelper.ScaleSizeLogical(760, 260),
            .BackColor = pmipForm.BackColor,
            .ForeColor = pmipForm.ForeColor,
            .Anchor = CType((AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles),
            .FullRowSelect = True,
            .MultiSelect = False,
            .View = View.Details
        }
        Dim pmipOkButton As Button = New Button With {
            .Location = WindowHelper.ScalePositionLogical(616, 300),
            .Size = WindowHelper.ScaleSizeLogical(75, 23),
            .Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles),
            .FlatStyle = FlatStyle.System,
            .Enabled = False
        }
        Dim pmipCancelButton As Button = New Button With {
            .Location = WindowHelper.ScalePositionLogical(698, 300),
            .Size = WindowHelper.ScaleSizeLogical(75, 23),
            .Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles),
            .FlatStyle = FlatStyle.System
        }
        pmipMountedImageList.Columns.AddRange(New ColumnHeader() {
                                              New ColumnHeader With {
                                                  .Width = WindowHelper.ScaleLogical(434)
                                              },
                                              New ColumnHeader With {
                                                  .Width = WindowHelper.ScaleLogical(64)
                                              },
                                              New ColumnHeader With {
                                                  .Width = WindowHelper.ScaleLogical(374)
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
                                       ControlPaint.DrawBorder(e.Graphics, pmipForm.ClientRectangle, CurrentTheme.AccentColors(1), ButtonBorderStyle.Solid)
                                   End Sub
        AddHandler pmipOkButton.Click, Sub(sender, e)
                                           pmipForm.DialogResult = DialogResult.OK
                                           pmipForm.Close()
                                       End Sub
        AddHandler pmipMountedImageList.MouseDoubleClick, Sub(sender, e)
                                                              If pmipMountedImageList.SelectedItems.Count <> 1 Then Exit Sub
                                                              pmipForm.DialogResult = DialogResult.OK
                                                              pmipForm.Close()
                                                          End Sub
        AddHandler pmipCancelButton.Click, Sub(sender, e)
                                               pmipForm.DialogResult = DialogResult.Cancel
                                               pmipForm.Close()
                                           End Sub

        ' Subscribe to MainForm mounted-images updates so the popup list refreshes in real time
        Dim mountedUpdatedHandler As EventHandler = Sub(s, ev)
                                                        Try
                                                            If pmipForm.IsDisposed Then Exit Sub
                                                            If pmipForm.InvokeRequired Then
                                                                pmipForm.BeginInvoke(New MethodInvoker(Sub()
                                                                                                           Try
                                                                                                               If mountedImages.Count = pmipMountedImageList.Items.Count Then Exit Sub
                                                                                                               pmipOkButton.Enabled = False
                                                                                                               pmipMountedImageList.Items.Clear()
                                                                                                               If mountedImages IsNot Nothing Then
                                                                                                                   If requiredExtensions.Any() Then mountedImages = mountedImages.Where(Function(image) requiredExtensions.Contains(Path.GetExtension(image.ImageFile))).ToList()

                                                                                                                   pmipMountedImageList.Items.AddRange(mountedImages.Select(Function(mountedImage) New ListViewItem(New String() {mountedImage.ImageFile,
                                                                                                                                                                                                                                  mountedImage.ImageIndex,
                                                                                                                                                                                                                                  mountedImage.ImageMountDirectory})).ToArray())
                                                                                                               End If
                                                                                                           Catch ex As Exception
                                                                                                               DynaLog.LogMessage("PMIP Update error: " & ex.Message)
                                                                                                           End Try
                                                                                                       End Sub))
                                                            Else
                                                                Try
                                                                    If mountedImages.Count = pmipMountedImageList.Items.Count Then Exit Sub
                                                                    pmipOkButton.Enabled = False
                                                                    pmipMountedImageList.Items.Clear()
                                                                    If mountedImages IsNot Nothing Then
                                                                        If requiredExtensions.Any() Then mountedImages = mountedImages.Where(Function(image) requiredExtensions.Contains(Path.GetExtension(image.ImageFile))).ToList()

                                                                        pmipMountedImageList.Items.AddRange(mountedImages.Select(Function(mountedImage) New ListViewItem(New String() {mountedImage.ImageFile,
                                                                                                                                                                                       mountedImage.ImageIndex,
                                                                                                                                                                                       mountedImage.ImageMountDirectory})).ToArray())
                                                                    End If
                                                                Catch ex As Exception
                                                                    DynaLog.LogMessage("PMIP update error: " & ex.Message)
                                                                End Try
                                                            End If
                                                        Catch ex As Exception
                                                            DynaLog.LogMessage("PMIP handler error: " & ex.Message)
                                                        End Try
                                                    End Sub

        AddHandler MainForm.MountedImagesUpdated, mountedUpdatedHandler

        ' Translate
        pmipForm.Text = LocalizationService.ForSection("MountedImagePicker.Pick")("Title.Label")
        pmipOkButton.Text = LocalizationService.ForSection("MountedImagePicker")("Ok.Button")
        pmipCancelButton.Text = LocalizationService.ForSection("MountedImagePicker")("Cancel.Button")
        pmipInstructionLabel.Text = LocalizationService.ForSection("MountedImagePicker.Pick")("Image.List.Label")
        pmipMountedImageList.Columns(0).Text = LocalizationService.ForSection("MountedImagePicker.Pick")("ImageFile.Column")
        pmipMountedImageList.Columns(1).Text = LocalizationService.ForSection("MountedImagePicker.Pick")("Index.Column")
        pmipMountedImageList.Columns(2).Text = LocalizationService.ForSection("MountedImagePicker.Pick")("MountDirectory.Column")

        ' Initial population and show the dialog
        GetMountedImages()
        If mountedImages IsNot Nothing Then
            If requiredExtensions.Any() Then mountedImages = mountedImages.Where(Function(image) requiredExtensions.Contains(Path.GetExtension(image.ImageFile))).ToList()

            pmipMountedImageList.Items.AddRange(mountedImages.Select(Function(mountedImage) New ListViewItem(New String() {mountedImage.ImageFile,
                                                                                                                           mountedImage.ImageIndex,
                                                                                                                           mountedImage.ImageMountDirectory})).ToArray())
        End If

        Dim dlgResult = pmipForm.ShowDialog()

        ' Unsubscribe the handler after dialog closes
        Try
            RemoveHandler MainForm.MountedImagesUpdated, mountedUpdatedHandler
        Catch
            ' ignore
        End Try

        Return If(dlgResult = DialogResult.OK, mountedImages.ElementAtOrDefault(focusedIndex), Nothing)
    End Function

    Private Shared Sub GetMountedImages()
        Try
            DynaLog.LogMessage("Preparing to get mounted images...")
            DynaLog.LogMessage("Getting mounted images...")
            mountedImages = MainForm.MountedImageList
        Catch ex As Exception
            DynaLog.LogMessage("Could not get mounted images. Error message: " & ex.Message)
            MsgBox(ex.Message, vbOKOnly + vbCritical, "")
        End Try
    End Sub

End Class
