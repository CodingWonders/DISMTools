Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports System.Threading
Imports DISMTools.Utilities

Public Class GetDriverInfo

    Dim DriverInfoList As New List(Of DismDriverCollection)
    Public InstalledDriverInfo As DismDriverPackageCollection
    Dim InstalledDriverList As New List(Of DismDriverPackage)

    Dim CurrentHWTarget As Integer
    Dim CurrentHWFile As Integer = -1        ' This variable gets updated every time an element is selected in the driver packages list box
    Dim JumpTo As Integer = -1               ' This variable gets updated every time a target is specified in the Jump To panel

    Dim ButtonTT As New ToolTip()

    Private Sub GetDriverInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Get driver information"
                        Label1.Text = Text
                        Label2.Text = "What do you want to get information about?"
                        Label3.Text = "Click here to get information about drivers that you've installed or that came with the Windows image you're servicing"
                        Label4.Text = "Click here to get information about drivers that you want to add to the Windows image you're servicing before proceeding with the driver addition process"
                        Label5.Text = "Ready"
                        Label6.Text = "Add or select a driver package to view its information here"
                        Label7.Text = "Hardware targets"
                        Label8.Text = "Hardware description:"
                        Label10.Text = "Hardware ID:"
                        Label12.Text = "Additional IDs:"
                        Label13.Text = "Compatible IDs:"
                        Label16.Text = "Exclude IDs:"
                        Label17.Text = "Hardware manufacturer:"
                        Label20.Text = "Architecture:"
                        Label21.Text = "Jump to target:"
                        Label22.Text = "Published name:"
                        Label24.Text = "Original file name:"
                        Label26.Text = "Provider name:"
                        Label28.Text = "Is critical to the boot process?"
                        Label30.Text = "Version:"
                        Label31.Text = "Class name:"
                        Label33.Text = "Part of the Windows distribution?"
                        Label36.Text = "Driver information"
                        Label37.Text = "Select an installed driver to view its information here"
                        Label39.Text = "Date:"
                        Label41.Text = "Class description:"
                        Label43.Text = "Class GUID:"
                        Label45.Text = "Driver signature status:"
                        Label47.Text = "Catalog file path:"
                        Label48.Text = "You have configured the background processes to not show all drivers present in this image, which includes drivers part of the Windows distribution, so you may not see the driver you're interested in."
                        Button1.Text = "Add driver..."
                        Button2.Text = "Remove selected"
                        Button3.Text = "Remove all"
                        Button7.Text = "Change"
                        LinkLabel1.Text = "<- Go back"
                        InstalledDriverLink.Text = "I want to get information about installed drivers in the image"
                        DriverFileLink.Text = "I want to get information about driver files"
                        ListView1.Columns(0).Text = "Published name"
                        ListView1.Columns(1).Text = "Original file name"
                        OpenFileDialog1.Title = "Locate driver files"
                    Case "ESN"
                        Text = "Obtener información de controladores"
                        Label1.Text = Text
                        Label2.Text = "¿Acerca de qué le gustaría obtener información?"
                        Label3.Text = "Haga clic aquí para obtener información de controladores que ha instalado o que vengan con la imagen de Windows a la que está dando servicio"
                        Label4.Text = "Haga clic aquí para obtener información de controladores que le gustaría añadir a la imagen de Windows a la que está dando servicio antes de proceder con el proceso de adición de controladores"
                        Label5.Text = "Listo"
                        Label6.Text = "Añada o seleccione un paquete de controlador para ver su información aquí"
                        Label7.Text = "Hardware de destino"
                        Label8.Text = "Descripción de hardware:"
                        Label10.Text = "ID de hardware:"
                        Label12.Text = "Identificadores adicionales:"
                        Label13.Text = "Identificadores compatibles:"
                        Label16.Text = "Identificadores excluidos:"
                        Label17.Text = "Fabricante de hardware:"
                        Label20.Text = "Arquitectura:"
                        Label21.Text = "Saltar a hardware:"
                        Label22.Text = "Nombre publicado:"
                        Label24.Text = "Nombre de archivo original:"
                        Label26.Text = "Nombre de proveedor:"
                        Label28.Text = "¿Es crítico para el proceso de arranque?"
                        Label30.Text = "Versión:"
                        Label31.Text = "Nombre de clase:"
                        Label33.Text = "¿Es parte de la distribución de Windows?"
                        Label36.Text = "Información del controlador"
                        Label37.Text = "Seleccione un controlador instalado para obtener su información aquí"
                        Label39.Text = "Fecha:"
                        Label41.Text = "Descripción de clase:"
                        Label43.Text = "Identificador GUID de clase:"
                        Label45.Text = "Estado de firma del controlador:"
                        Label47.Text = "Ruta del archivo de catálogo:"
                        Label48.Text = "Ha configurado los procesos en segundo plano de manera que no se muestren todos los controladores de esta imagen, que incluye controladores parte de la distribución de Windows, por lo que podría no ver el controlador que le interesa."
                        Button1.Text = "Añadir controlador..."
                        Button2.Text = "Eliminar selección"
                        Button3.Text = "Eliminar todos"
                        Button7.Text = "Cambiar"
                        LinkLabel1.Text = "<- Atrás"
                        InstalledDriverLink.Text = "Deseo obtener información acerca de controladores instalados en la imagen"
                        DriverFileLink.Text = "Deseo obtener información acerca de archivos de controladores"
                        ListView1.Columns(0).Text = "Nombre publicado"
                        ListView1.Columns(1).Text = "Nombre de archivo original"
                        OpenFileDialog1.Title = "Ubique los archivos de controladores"
                End Select
            Case 1
                Text = "Get driver information"
                Label1.Text = Text
                Label2.Text = "What do you want to get information about?"
                Label3.Text = "Click here to get information about drivers that you've installed or that came with the Windows image you're servicing"
                Label4.Text = "Click here to get information about drivers that you want to add to the Windows image you're servicing before proceeding with the driver addition process"
                Label5.Text = "Ready"
                Label6.Text = "Add or select a driver package to view its information here"
                Label7.Text = "Hardware targets"
                Label8.Text = "Hardware description:"
                Label10.Text = "Hardware ID:"
                Label12.Text = "Additional IDs:"
                Label13.Text = "Compatible IDs:"
                Label16.Text = "Exclude IDs:"
                Label17.Text = "Hardware manufacturer:"
                Label20.Text = "Architecture:"
                Label21.Text = "Jump to target:"
                Label22.Text = "Published name:"
                Label24.Text = "Original file name:"
                Label26.Text = "Provider name:"
                Label28.Text = "Is critical to the boot process?"
                Label30.Text = "Version:"
                Label31.Text = "Class name:"
                Label33.Text = "Part of the Windows distribution?"
                Label36.Text = "Driver information"
                Label37.Text = "Select an installed driver to view its information here"
                Label39.Text = "Date:"
                Label41.Text = "Class description:"
                Label43.Text = "Class GUID:"
                Label45.Text = "Driver signature status:"
                Label47.Text = "Catalog file path:"
                Label48.Text = "You have configured the background processes to not show all drivers present in this image, which includes drivers part of the Windows distribution, so you may not see the driver you're interested in."
                Button1.Text = "Add driver..."
                Button2.Text = "Remove selected"
                Button3.Text = "Remove all"
                Button7.Text = "Change"
                LinkLabel1.Text = "<- Go back"
                InstalledDriverLink.Text = "I want to get information about installed drivers in the image"
                DriverFileLink.Text = "I want to get information about driver files"
                ListView1.Columns(0).Text = "Published name"
                ListView1.Columns(1).Text = "Original file name"
                OpenFileDialog1.Title = "Locate driver files"
            Case 2
                Text = "Obtener información de controladores"
                Label1.Text = Text
                Label2.Text = "¿Acerca de qué le gustaría obtener información?"
                Label3.Text = "Haga clic aquí para obtener información de controladores que ha instalado o que vengan con la imagen de Windows a la que está dando servicio"
                Label4.Text = "Haga clic aquí para obtener información de controladores que le gustaría añadir a la imagen de Windows a la que está dando servicio antes de proceder con el proceso de adición de controladores"
                Label5.Text = "Listo"
                Label6.Text = "Añada o seleccione un paquete de controlador para ver su información aquí"
                Label7.Text = "Hardware de destino"
                Label8.Text = "Descripción de hardware:"
                Label10.Text = "ID de hardware:"
                Label12.Text = "Identificadores adicionales:"
                Label13.Text = "Identificadores compatibles:"
                Label16.Text = "Identificadores excluidos:"
                Label17.Text = "Fabricante de hardware:"
                Label20.Text = "Arquitectura:"
                Label21.Text = "Saltar a hardware:"
                Label22.Text = "Nombre publicado:"
                Label24.Text = "Nombre de archivo original:"
                Label26.Text = "Nombre de proveedor:"
                Label28.Text = "¿Es crítico para el proceso de arranque?"
                Label30.Text = "Versión:"
                Label31.Text = "Nombre de clase:"
                Label33.Text = "¿Es parte de la distribución de Windows?"
                Label36.Text = "Información del controlador"
                Label37.Text = "Seleccione un controlador instalado para obtener su información aquí"
                Label39.Text = "Fecha:"
                Label41.Text = "Descripción de clase:"
                Label43.Text = "Identificador GUID de clase:"
                Label45.Text = "Estado de firma del controlador:"
                Label47.Text = "Ruta del archivo de catálogo:"
                Label48.Text = "Ha configurado los procesos en segundo plano de manera que no se muestren todos los controladores de esta imagen, que incluye controladores parte de la distribución de Windows, por lo que podría no ver el controlador que le interesa."
                Button1.Text = "Añadir controlador..."
                Button2.Text = "Eliminar selección"
                Button3.Text = "Eliminar todos"
                Button7.Text = "Cambiar"
                LinkLabel1.Text = "<- Atrás"
                InstalledDriverLink.Text = "Deseo obtener información acerca de controladores instalados en la imagen"
                DriverFileLink.Text = "Deseo obtener información acerca de archivos de controladores"
                ListView1.Columns(0).Text = "Nombre publicado"
                ListView1.Columns(1).Text = "Nombre de archivo original"
                OpenFileDialog1.Title = "Ubique los archivos de controladores"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListBox1.BackColor = Color.FromArgb(238, 238, 242)
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        InstalledDriverList.Clear()
        ListView1.Items.Clear()
        If InstalledDriverList.Count >= 0 Then
            For Each DriverPackage As DismDriverPackage In InstalledDriverInfo
                InstalledDriverList.Add(DriverPackage)
                ListView1.Items.Add(New ListViewItem(New String() {DriverPackage.PublishedName, Path.GetFileName(DriverPackage.OriginalFileName)}))
            Next
        End If

        ' Detect if the "Detect all drivers" option is checked and act accordingly
        Panel6.Visible = MainForm.AllDrivers = False

        ' Switch to the selection panels
        Panel4.Visible = False
        Panel7.Visible = True
        DrvPackageInfoPanel.Visible = False
        NoDrvPanel.Visible = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        ListBox1.Items.Add(OpenFileDialog1.FileName)
        Button3.Enabled = True
        GetDriverInformation()
    End Sub

    Private Sub InstalledDriverLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles InstalledDriverLink.LinkClicked
        MenuPanel.Visible = False
        DriverInfoPanel.Visible = True
        InfoFromInstalledDrvsPanel.Visible = True
        InfoFromDrvPackagesPanel.Visible = False

        ' Detect if the "Detect all drivers" option is checked and act accordingly
        Panel6.Visible = MainForm.AllDrivers = False

        Label5.Visible = False
    End Sub

    Private Sub DriverFileLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles DriverFileLink.LinkClicked
        MenuPanel.Visible = False
        DriverInfoPanel.Visible = True
        InfoFromInstalledDrvsPanel.Visible = False
        InfoFromDrvPackagesPanel.Visible = True
        Panel6.Visible = False
        Label5.Visible = True
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MenuPanel.Visible = True
        DriverInfoPanel.Visible = False
    End Sub

    Sub GetDriverInformation()
        DriverInfoList.Clear()
        Try
            ' Background processes need to have completed before showing information
            If MainForm.ImgBW.IsBusy Then
                Dim msg As String = ""
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                msg = "Background processes need to have completed before showing package information. We'll wait until they have completed"
                            Case "ESN"
                                msg = "Los procesos en segundo plano deben haber completado antes de obtener información del paquete. Esperaremos hasta que hayan completado"
                        End Select
                    Case 1
                        msg = "Background processes need to have completed before showing package information. We'll wait until they have completed"
                    Case 2
                        msg = "Los procesos en segundo plano deben haber completado antes de obtener información del paquete. Esperaremos hasta que hayan completado"
                End Select
                MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Label5.Text = "Waiting for background processes to finish..."
                            Case "ESN"
                                Label5.Text = "Esperando a que terminen los procesos en segundo plano..."
                        End Select
                    Case 1
                        Label5.Text = "Waiting for background processes to finish..."
                    Case 2
                        Label5.Text = "Esperando a que terminen los procesos en segundo plano..."
                End Select
                While MainForm.ImgBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(500)
                End While
            End If
            If MainForm.MountedImageDetectorBW.IsBusy Then
                MainForm.MountedImageDetectorBW.CancelAsync()
                While MainForm.MountedImageDetectorBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(500)
                End While
            End If
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label5.Text = "Preparing driver information processes..."
                        Case "ESN"
                            Label5.Text = "Preparando procesos de información de controladores..."
                    End Select
                Case 1
                    Label5.Text = "Preparing driver information processes..."
                Case 2
                    Label5.Text = "Preparando procesos de información de controladores..."
            End Select
            Application.DoEvents()
            DismApi.Initialize(DismLogLevel.LogErrors)
            Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                For Each drvFile In ListBox1.Items
                    If File.Exists(drvFile) Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Label5.Text = "Getting information from driver file " & Quote & Path.GetFileName(drvFile) & Quote & "..." & CrLf & "This may take some time and the program may temporarily freeze"
                                    Case "ESN"
                                        Label5.Text = "Obteniendo información del archivo de controlador " & Quote & Path.GetFileName(drvFile) & Quote & "..." & CrLf & "Esto puede llevar algo de tiempo y el programa podría congelarse temporalmente"
                                End Select
                            Case 1
                                Label5.Text = "Getting information from driver file " & Quote & Path.GetFileName(drvFile) & Quote & "..." & CrLf & "This may take some time and the program may temporarily freeze"
                            Case 2
                                Label5.Text = "Obteniendo información del archivo de controlador " & Quote & Path.GetFileName(drvFile) & Quote & "..." & CrLf & "Esto puede llevar algo de tiempo y el programa podría congelarse temporalmente"
                        End Select
                        Application.DoEvents()
                        Dim drvInfoCollection As DismDriverCollection = DismApi.GetDriverInfo(imgSession, drvFile)
                        If drvInfoCollection.Count > 0 Then DriverInfoList.Add(drvInfoCollection)
                    End If
                Next
            End Using
        Catch ex As Exception
            ' Cancel it
        Finally
            DismApi.Shutdown()
        End Try
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label5.Text = "Ready"
                    Case "ESN"
                        Label5.Text = "Listo"
                End Select
            Case 1
                Label5.Text = "Ready"
            Case 2
                Label5.Text = "Listo"
        End Select
    End Sub

    Sub DisplayDriverInformation(HWTarget As Integer)
        Dim CurrentDriverCollection As DismDriverCollection = DriverInfoList(ListBox1.SelectedIndex)
        For Each DriverPackageInfo As DismDriver In CurrentDriverCollection
            If CurrentDriverCollection.IndexOf(DriverPackageInfo) = HWTarget - 1 Then
                Label9.Text = DriverPackageInfo.HardwareDescription
                Label11.Text = DriverPackageInfo.HardwareId
                Label14.Text = DriverPackageInfo.CompatibleIds
                Label15.Text = DriverPackageInfo.ExcludeIds
                Label18.Text = DriverPackageInfo.ManufacturerName
                Label19.Text = Casters.CastDismArchitecture(DriverPackageInfo.Architecture, True)
                If Label14.Text = "" Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label14.Text = "None declared by the hardware manufacturer"
                                Case "ESN"
                                    Label14.Text = "Ninguno declarado por el fabricante del hardware"
                            End Select
                        Case 1
                            Label14.Text = "None declared by the hardware manufacturer"
                        Case 2
                            Label14.Text = "Ninguno declarado por el fabricante del hardware"
                    End Select
                End If
                If Label15.Text = "" Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label15.Text = "None declared by the hardware manufacturer"
                                Case "ESN"
                                    Label15.Text = "Ninguno declarado por el fabricante del hardware"
                            End Select
                        Case 1
                            Label15.Text = "None declared by the hardware manufacturer"
                        Case 2
                            Label15.Text = "Ninguno declarado por el fabricante del hardware"
                    End Select
                End If
                Exit For
            End If
        Next
    End Sub

    Sub DisplayHardwareTargetOverview()
        ' This function is called when the user clicks on the "Jump to target" button
        If ListBox1.SelectedItems.Count <> 1 Then
            ' Don't continue
            Exit Sub
        Else
            JumpTo = -1
            ComboBox1.Text = ""
            Dim CurrentDriverCollection As DismDriverCollection = DriverInfoList(ListBox1.SelectedIndex)
            For Each DriverPackageInfo As DismDriver In CurrentDriverCollection
                ComboBox1.Items.Add(CurrentDriverCollection.IndexOf(DriverPackageInfo) + 1 & " - " & DriverPackageInfo.HardwareDescription & " (" & DriverPackageInfo.HardwareId & ")")
            Next
        End If
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each PackageFile In PackageFiles
            If Path.GetExtension(PackageFile).EndsWith("inf", StringComparison.OrdinalIgnoreCase) Then
                ListBox1.Items.Add(PackageFile)
            End If
        Next
        Button3.Enabled = True
        GetDriverInformation()
    End Sub

    Private Sub GetDriverInfo_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Try
            If ListBox1.SelectedItems.Count = 1 Then
                JumpToPanel.Visible = False
                NoDrvPanel.Visible = False
                DrvPackageInfoPanel.Visible = True
                Button2.Enabled = True
                If Not CurrentHWFile = ListBox1.SelectedIndex Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label7.Text = "Hardware target 1 of " & DriverInfoList(ListBox1.SelectedIndex).Count
                                Case "ESN"
                                    Label7.Text = "Hardware de destino 1 de " & DriverInfoList(ListBox1.SelectedIndex).Count
                            End Select
                        Case 1
                            Label7.Text = "Hardware target 1 of " & DriverInfoList(ListBox1.SelectedIndex).Count
                        Case 2
                            Label7.Text = "Hardware de destino 1 de " & DriverInfoList(ListBox1.SelectedIndex).Count
                    End Select
                End If
                If Not CurrentHWFile = ListBox1.SelectedIndex Then CurrentHWTarget = 1
                Button4.Enabled = False
                Button5.Enabled = True
                If Not CurrentHWFile = ListBox1.SelectedIndex Then DisplayDriverInformation(1)
                CurrentHWFile = ListBox1.SelectedIndex
            Else
                NoDrvPanel.Visible = True
                DrvPackageInfoPanel.Visible = False
                Button2.Enabled = False
            End If
        Catch ex As Exception
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            NoDrvPanel.Visible = True
            DrvPackageInfoPanel.Visible = False
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DriverInfoList.RemoveAt(ListBox1.SelectedIndex)
        ListBox1.Items.Remove(ListBox1.SelectedItem)
        If ListBox1.Items.Count > 1 Then Button2.Enabled = False Else Button2.Enabled = True
        NoDrvPanel.Visible = True
        DrvPackageInfoPanel.Visible = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DriverInfoList.Clear()
        ListBox1.Items.Clear()
        Button2.Enabled = False
        Button3.Enabled = False
        NoDrvPanel.Visible = True
        DrvPackageInfoPanel.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If CurrentHWTarget > 1 Then
            DisplayDriverInformation(CurrentHWTarget - 1)
            CurrentHWTarget -= 1
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label7.Text = "Hardware target " & CurrentHWTarget & " of " & DriverInfoList(ListBox1.SelectedIndex).Count
                        Case "ESN"
                            Label7.Text = "Hardware de destino " & CurrentHWTarget & " de " & DriverInfoList(ListBox1.SelectedIndex).Count
                    End Select
                Case 1
                    Label7.Text = "Hardware target " & CurrentHWTarget & " of " & DriverInfoList(ListBox1.SelectedIndex).Count
                Case 2
                    Label7.Text = "Hardware de destino " & CurrentHWTarget & " de " & DriverInfoList(ListBox1.SelectedIndex).Count
            End Select
            Button5.Enabled = True
            If CurrentHWTarget = 1 Then Button4.Enabled = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If CurrentHWTarget < DriverInfoList(ListBox1.SelectedIndex).Count Then
            DisplayDriverInformation(CurrentHWTarget + 1)
            CurrentHWTarget += 1
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label7.Text = "Hardware target " & CurrentHWTarget & " of " & DriverInfoList(ListBox1.SelectedIndex).Count
                        Case "ESN"
                            Label7.Text = "Hardware de destino " & CurrentHWTarget & " de " & DriverInfoList(ListBox1.SelectedIndex).Count
                    End Select
                Case 1
                    Label7.Text = "Hardware target " & CurrentHWTarget & " of " & DriverInfoList(ListBox1.SelectedIndex).Count
                Case 2
                    Label7.Text = "Hardware de destino " & CurrentHWTarget & " de " & DriverInfoList(ListBox1.SelectedIndex).Count
            End Select
            Button4.Enabled = True
            If CurrentHWTarget = DriverInfoList(ListBox1.SelectedIndex).Count Then Button5.Enabled = False
        End If
    End Sub

    Private Sub Button4_MouseHover(sender As Object, e As EventArgs) Handles Button4.MouseHover
        Dim msg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        msg = "Previous hardware target"
                    Case "ESN"
                        msg = "Anterior hardware de destino"
                End Select
            Case 1
                msg = "Previous hardware target"
            Case 2
                msg = "Anterior hardware de destino"
        End Select
        ButtonTT.SetToolTip(sender, msg)
    End Sub

    Private Sub Button5_MouseHover(sender As Object, e As EventArgs) Handles Button5.MouseHover
        Dim msg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        msg = "Next hardware target"
                    Case "ESN"
                        msg = "Siguiente hardware de destino"
                End Select
            Case 1
                msg = "Next hardware target"
            Case 2
                msg = "Siguiente hardware de destino"
        End Select
        ButtonTT.SetToolTip(sender, msg)
    End Sub

    Private Sub Button6_MouseHover(sender As Object, e As EventArgs) Handles Button6.MouseHover
        Dim msg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        msg = "Jump to specific hardware target"
                    Case "ESN"
                        msg = "Saltar a hardware de destino específico"
                End Select
            Case 1
                msg = "Jump to specific hardware target"
            Case 2
                msg = "Saltar a hardware de destino específico"
        End Select
        ButtonTT.SetToolTip(sender, msg)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        JumpTo = ComboBox1.SelectedIndex + 1
        If JumpTo < 1 Then Exit Sub
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label7.Text = "Hardware target " & JumpTo & " of " & DriverInfoList(ListBox1.SelectedIndex).Count
                    Case "ESN"
                        Label7.Text = "Hardware de destino " & JumpTo & " de " & DriverInfoList(ListBox1.SelectedIndex).Count
                End Select
            Case 1
                Label7.Text = "Hardware target " & JumpTo & " of " & DriverInfoList(ListBox1.SelectedIndex).Count
            Case 2
                Label7.Text = "Hardware de destino " & JumpTo & " de " & DriverInfoList(ListBox1.SelectedIndex).Count
        End Select
        CurrentHWTarget = JumpTo
        DisplayDriverInformation(JumpTo)
        JumpToPanel.Visible = False
        If CurrentHWTarget = DriverInfoList(ListBox1.SelectedIndex).Count Then Button5.Enabled = False
        If CurrentHWTarget = 1 Then Button4.Enabled = False
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        JumpToPanel.Visible = True
        Button4.Enabled = True
        Button5.Enabled = True
        ComboBox1.Items.Clear()
        DisplayHardwareTargetOverview()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            If ListView1.SelectedItems.Count = 1 Then
                Panel4.Visible = True
                Panel7.Visible = False
                Label23.Text = InstalledDriverList(ListView1.FocusedItem.Index).PublishedName
                Label25.Text = Path.GetFileName(InstalledDriverList(ListView1.FocusedItem.Index).OriginalFileName)
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Label27.Text = If(InstalledDriverList(ListView1.FocusedItem.Index).BootCritical, "Yes", "No")
                                Label34.Text = If(InstalledDriverList(ListView1.FocusedItem.Index).InBox, "Yes", "No")
                            Case "ESN"
                                Label27.Text = If(InstalledDriverList(ListView1.FocusedItem.Index).BootCritical, "Sí", "No")
                                Label34.Text = If(InstalledDriverList(ListView1.FocusedItem.Index).InBox, "Sí", "No")
                        End Select
                    Case 1
                        Label27.Text = If(InstalledDriverList(ListView1.FocusedItem.Index).BootCritical, "Yes", "No")
                        Label34.Text = If(InstalledDriverList(ListView1.FocusedItem.Index).InBox, "Yes", "No")
                    Case 2
                        Label27.Text = If(InstalledDriverList(ListView1.FocusedItem.Index).BootCritical, "Sí", "No")
                        Label34.Text = If(InstalledDriverList(ListView1.FocusedItem.Index).InBox, "Sí", "No")
                End Select
                Label29.Text = InstalledDriverList(ListView1.FocusedItem.Index).Version.ToString()
                Label32.Text = InstalledDriverList(ListView1.FocusedItem.Index).ClassName
                Label35.Text = InstalledDriverList(ListView1.FocusedItem.Index).ProviderName
                Label38.Text = InstalledDriverList(ListView1.FocusedItem.Index).Date
                Label40.Text = InstalledDriverList(ListView1.FocusedItem.Index).ClassDescription
                Label42.Text = InstalledDriverList(ListView1.FocusedItem.Index).ClassGuid
                Label44.Text = Casters.CastDismSignatureStatus(InstalledDriverList(ListView1.FocusedItem.Index).DriverSignature, True)
                Label46.Text = InstalledDriverList(ListView1.FocusedItem.Index).CatalogFile
            Else
                Panel4.Visible = False
                Panel7.Visible = True
            End If
        Catch ex As Exception
            Panel4.Visible = False
            Panel7.Visible = True
        End Try
    End Sub

    Private Sub Label25_MouseHover(sender As Object, e As EventArgs) Handles Label25.MouseHover
        ButtonTT.SetToolTip(sender, InstalledDriverList(ListView1.FocusedItem.Index).OriginalFileName)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Visible = False
        BGProcsAdvSettings.ShowDialog(MainForm)
        If BGProcsAdvSettings.DialogResult = Windows.Forms.DialogResult.OK And BGProcsAdvSettings.NeedsDriverChecks Then Close() Else Visible = True
    End Sub
End Class
