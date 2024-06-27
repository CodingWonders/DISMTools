Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports Microsoft.VisualBasic.ControlChars

Public Class DriverFileInfoDlg

    Dim drvPkg As DismDriverPackage

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Copy_Button_Click(sender As Object, e As EventArgs) Handles Copy_Button.Click
        Dim clipStr As String = "Driver file information of " & GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex) & ":" & CrLf & CrLf & _
                                "Published name: " & drvPkg.PublishedName & CrLf & _
                                "Original file name: " & drvPkg.OriginalFileName & CrLf & _
                                "Is critical to the boot process? " & If(drvPkg.BootCritical, "Yes", "No") & CrLf & _
                                "Is part of the Windows distribution? " & If(drvPkg.InBox, "Yes", "No") & CrLf & _
                                "Version: " & drvPkg.Version.ToString() & CrLf & _
                                "Class name: " & drvPkg.ClassName & CrLf & _
                                "Class description: " & drvPkg.ClassDescription & CrLf & _
                                "Class GUID: " & drvPkg.ClassGuid & CrLf & _
                                "Provider name: " & drvPkg.ProviderName & CrLf & _
                                "Date: " & drvPkg.Date & CrLf & _
                                "Signature status: " & Casters.CastDismSignatureStatus(drvPkg.DriverSignature) & CrLf & _
                                "Catalog file: " & drvPkg.CatalogFile
        Dim data As New DataObject()
        data.SetText(clipStr, TextDataFormat.Text)
        Clipboard.SetDataObject(data, True)
        DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub DriverFileInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Driver file information"
                        Label1.Text = "Information of driver file: " & Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex))
                        ListView1.Columns(0).Text = "Property"
                        ListView1.Columns(1).Text = "Value"
                        OK_Button.Text = "OK"
                        Copy_Button.Text = "Copy"
                    Case "ESN"
                        Text = "Información del archivo de controlador"
                        Label1.Text = "Información del archivo de controlador: " & Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex))
                        ListView1.Columns(0).Text = "Propiedad"
                        ListView1.Columns(1).Text = "Valor"
                        OK_Button.Text = "Aceptar"
                        Copy_Button.Text = "Copiar"
                    Case "FRA"
                        Text = "Informations sur le fichier du pilote"
                        Label1.Text = "Informations sur le fichier du pilote : " & Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex))
                        ListView1.Columns(0).Text = "Propriété"
                        ListView1.Columns(1).Text = "Valeur"
                        OK_Button.Text = "OK"
                        Copy_Button.Text = "Copier"
                    Case "PTB", "PTG"
                        Text = "Informações sobre o ficheiro do controlador"
                        Label1.Text = "Informações do ficheiro do controlador: " & Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex))
                        ListView1.Columns(0).Text = "Propriedade"
                        ListView1.Columns(1).Text = "Valor"
                        OK_Button.Text = "OK"
                        Copy_Button.Text = "Copiar"
                    Case "ITA"
                        Text = "Informazioni sul file del driver"
                        Label1.Text = "Informazioni sul file del driver: " & Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex))
                        ListView1.Columns(0).Text = "Proprietà"
                        ListView1.Columns(1).Text = "Valore"
                        OK_Button.Text = "OK"
                        Copy_Button.Text = "Copia"
                End Select
            Case 1
                Text = "Driver file information"
                Label1.Text = "Information of driver file: " & Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex))
                ListView1.Columns(0).Text = "Property"
                ListView1.Columns(1).Text = "Value"
                OK_Button.Text = "OK"
                Copy_Button.Text = "Copy"
            Case 2
                Text = "Información del archivo de controlador"
                Label1.Text = "Información del archivo de controlador: " & Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex))
                ListView1.Columns(0).Text = "Propiedad"
                ListView1.Columns(1).Text = "Valor"
                OK_Button.Text = "Aceptar"
                Copy_Button.Text = "Copiar"
            Case 3
                Text = "Informations sur le fichier du pilote"
                Label1.Text = "Informations sur le fichier du pilote : " & Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex))
                ListView1.Columns(0).Text = "Propriété"
                ListView1.Columns(1).Text = "Valeur"
                OK_Button.Text = "OK"
                Copy_Button.Text = "Copier"
            Case 4
                Text = "Informações sobre o ficheiro do controlador"
                Label1.Text = "Informações do ficheiro do controlador: " & Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex))
                ListView1.Columns(0).Text = "Propriedade"
                ListView1.Columns(1).Text = "Valor"
                OK_Button.Text = "OK"
                Copy_Button.Text = "Copiar"
            Case 5
                Text = "Informazioni sul file del driver"
                Label1.Text = "Informazioni sul file del driver: " & Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex))
                ListView1.Columns(0).Text = "Proprietà"
                ListView1.Columns(1).Text = "Valore"
                OK_Button.Text = "OK"
                Copy_Button.Text = "Copia"
        End Select
        ListView1.Items.Clear()
        drvPkg = Nothing
        Try
            DismApi.Initialize(DismLogLevel.LogErrors)
            Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                Dim drvInfo As DismDriverCollection = DismApi.GetDriverInfo(imgSession, GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex), drvPkg)
                If drvPkg IsNot Nothing Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    ListView1.Items.Add(New ListViewItem(New String() {"Published name", drvPkg.PublishedName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Original file name", drvPkg.OriginalFileName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Is critical to the boot process?", If(drvPkg.BootCritical, "Yes", "No")}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Is part of the Windows distribution?", If(drvPkg.InBox, "Yes", "No")}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Version", drvPkg.Version.ToString()}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Class name", drvPkg.ClassName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Class description", drvPkg.ClassDescription}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Class GUID", drvPkg.ClassGuid}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Provider name", drvPkg.ProviderName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Date", drvPkg.Date}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Signature status", Casters.CastDismSignatureStatus(drvPkg.DriverSignature, True)}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Catalog file", drvPkg.CatalogFile}))
                                Case "ESN"
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nombre publicado", drvPkg.PublishedName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nombre original del archivo", drvPkg.OriginalFileName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"¿Es crítico para el arranque?", If(drvPkg.BootCritical, "Sí", "No")}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"¿Es parte de la distribución de Windows?", If(drvPkg.InBox, "Sí", "No")}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Versión", drvPkg.Version.ToString()}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nombre de clase", drvPkg.ClassName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Descripción de clase", drvPkg.ClassDescription}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"GUID de clase", drvPkg.ClassGuid}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nombre del proveedor", drvPkg.ProviderName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Fecha", drvPkg.Date}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Estado de firma del controlador", Casters.CastDismSignatureStatus(drvPkg.DriverSignature, True)}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Archivo de catálogo", drvPkg.CatalogFile}))
                                Case "FRA"
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nom publiè", drvPkg.PublishedName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nom du fichier original", drvPkg.OriginalFileName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Est-il essentiel au processus de démarrage ?", If(drvPkg.BootCritical, "Oui", "Non")}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Est-il partie de la distribution Windows ?", If(drvPkg.InBox, "Oui", "Non")}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Version", drvPkg.Version.ToString()}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nom de classe", drvPkg.ClassName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Description de classe", drvPkg.ClassDescription}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"GUID de classe", drvPkg.ClassGuid}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nom du prestataire", drvPkg.ProviderName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Date", drvPkg.Date}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"État de la signature du pilote", Casters.CastDismSignatureStatus(drvPkg.DriverSignature, True)}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Chemin d'accès au fichier de catalogue", drvPkg.CatalogFile}))
                                Case "PTB", "PTG"
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nome publicado", drvPkg.PublishedName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nome do ficheiro original", drvPkg.OriginalFileName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"É fundamental para o processo de arranque?", If(drvPkg.BootCritical, "Sim", "Não")}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Faz parte da distribuição do Windows?", If(drvPkg.InBox, "Sim", "Não")}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Versão", drvPkg.Version.ToString()}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nome da classe", drvPkg.ClassName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Descrição da classe", drvPkg.ClassDescription}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"GUID da classe", drvPkg.ClassGuid}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nome do provedor", drvPkg.ProviderName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Data", drvPkg.Date}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Estado da assinatura", Casters.CastDismSignatureStatus(drvPkg.DriverSignature, True)}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Ficheiro de catálogo", drvPkg.CatalogFile}))
                                Case "ITA"
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nome pubblicato", drvPkg.PublishedName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nome del file originale", drvPkg.OriginalFileName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"È critico per il processo di avvio?", If(drvPkg.BootCritical, "Sì", "No")}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Fa parte della distribuzione di Windows?", If(drvPkg.InBox, "Sì", "No")}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Version", drvPkg.Version.ToString()}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nome classe", drvPkg.ClassName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Class description", drvPkg.ClassDescription}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Class GUID", drvPkg.ClassGuid}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Nome del provider", drvPkg.ProviderName}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Date", drvPkg.Date}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"Stato della firma", Casters.CastDismSignatureStatus(drvPkg.DriverSignature, True)}))
                                    ListView1.Items.Add(New ListViewItem(New String() {"File catalogo", drvPkg.CatalogFile}))
                            End Select
                        Case 1
                            ListView1.Items.Add(New ListViewItem(New String() {"Published name", drvPkg.PublishedName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Original file name", drvPkg.OriginalFileName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Is critical to the boot process?", If(drvPkg.BootCritical, "Yes", "No")}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Is part of the Windows distribution?", If(drvPkg.InBox, "Yes", "No")}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Version", drvPkg.Version.ToString()}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Class name", drvPkg.ClassName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Class description", drvPkg.ClassDescription}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Class GUID", drvPkg.ClassGuid}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Provider name", drvPkg.ProviderName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Date", drvPkg.Date}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Signature status", Casters.CastDismSignatureStatus(drvPkg.DriverSignature, True)}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Catalog file", drvPkg.CatalogFile}))
                        Case 2
                            ListView1.Items.Add(New ListViewItem(New String() {"Nombre publicado", drvPkg.PublishedName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nombre original del archivo", drvPkg.OriginalFileName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"¿Es crítico para el arranque?", If(drvPkg.BootCritical, "Sí", "No")}))
                            ListView1.Items.Add(New ListViewItem(New String() {"¿Es parte de la distribución de Windows?", If(drvPkg.InBox, "Sí", "No")}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Versión", drvPkg.Version.ToString()}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nombre de clase", drvPkg.ClassName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Descripción de clase", drvPkg.ClassDescription}))
                            ListView1.Items.Add(New ListViewItem(New String() {"GUID de clase", drvPkg.ClassGuid}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nombre del proveedor", drvPkg.ProviderName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Fecha", drvPkg.Date}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Estado de firma del controlador", Casters.CastDismSignatureStatus(drvPkg.DriverSignature, True)}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Archivo de catálogo", drvPkg.CatalogFile}))
                        Case 3
                            ListView1.Items.Add(New ListViewItem(New String() {"Nom publiè", drvPkg.PublishedName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nom du fichier original", drvPkg.OriginalFileName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Est-il essentiel au processus de démarrage ?", If(drvPkg.BootCritical, "Oui", "Non")}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Est-il partie de la distribution Windows ?", If(drvPkg.InBox, "Oui", "Non")}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Version", drvPkg.Version.ToString()}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nom de classe", drvPkg.ClassName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Description de classe", drvPkg.ClassDescription}))
                            ListView1.Items.Add(New ListViewItem(New String() {"GUID de classe", drvPkg.ClassGuid}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nom du prestataire", drvPkg.ProviderName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Date", drvPkg.Date}))
                            ListView1.Items.Add(New ListViewItem(New String() {"État de la signature du pilote", Casters.CastDismSignatureStatus(drvPkg.DriverSignature, True)}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Chemin d'accès au fichier de catalogue", drvPkg.CatalogFile}))
                        Case 4
                            ListView1.Items.Add(New ListViewItem(New String() {"Nome publicado", drvPkg.PublishedName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nome do ficheiro original", drvPkg.OriginalFileName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"É fundamental para o processo de arranque?", If(drvPkg.BootCritical, "Sim", "Não")}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Faz parte da distribuição do Windows?", If(drvPkg.InBox, "Sim", "Não")}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Versão", drvPkg.Version.ToString()}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nome da classe", drvPkg.ClassName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Descrição da classe", drvPkg.ClassDescription}))
                            ListView1.Items.Add(New ListViewItem(New String() {"GUID da classe", drvPkg.ClassGuid}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nome do provedor", drvPkg.ProviderName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Data", drvPkg.Date}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Estado da assinatura", Casters.CastDismSignatureStatus(drvPkg.DriverSignature, True)}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Ficheiro de catálogo", drvPkg.CatalogFile}))
                        Case 5
                            ListView1.Items.Add(New ListViewItem(New String() {"Nome pubblicato", drvPkg.PublishedName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nome del file originale", drvPkg.OriginalFileName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"È critico per il processo di avvio?", If(drvPkg.BootCritical, "Sì", "No")}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Fa parte della distribuzione di Windows?", If(drvPkg.InBox, "Sì", "No")}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Version", drvPkg.Version.ToString()}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nome classe", drvPkg.ClassName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Class description", drvPkg.ClassDescription}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Class GUID", drvPkg.ClassGuid}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Nome del provider", drvPkg.ProviderName}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Date", drvPkg.Date}))
                            ListView1.Items.Add(New ListViewItem(New String() {"Stato della firma", Casters.CastDismSignatureStatus(drvPkg.DriverSignature, True)}))
                            ListView1.Items.Add(New ListViewItem(New String() {"File catalogo", drvPkg.CatalogFile}))
                    End Select
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message & " (HRESULT: " & ex.HResult & ")", vbOKOnly + vbCritical, Text)
        Finally
            DismApi.Shutdown()
        End Try
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
