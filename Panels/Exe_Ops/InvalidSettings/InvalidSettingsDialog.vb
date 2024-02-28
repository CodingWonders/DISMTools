Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class InvalidSettingsDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub InvalidSettingsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Invalid settings have been detected"
                        Label1.Text = "The program has detected invalid settings"
                        Label2.Text = "The invalid settings have been reset to default values. Check the fields below for more information:"
                        Button1.Text = "OK"
                    Case "ESN"
                        Text = "Se han detectado configuraciones inválidas"
                        Label1.Text = "El programa ha detectado configuraciones inválidas"
                        Label2.Text = "Las configuraciones inválidas han sido restablecidas a sus valores predeterminados. Compruebe los campos de abajo para más información:"
                        Button1.Text = "Aceptar"
                    Case "FRA"
                        Text = "Des paramètres non valides ont été détectés"
                        Label1.Text = "Le programme a détecté des paramètres non valides"
                        Label2.Text = "Les paramètres non valides ont été réinitialisés aux valeurs par défaut. Vérifiez les champs ci-dessous pour plus d'informations :"
                        Button1.Text = "OK"
                    Case "PTB", "PTG"
                        Text = "Foram detectadas definições inválidas"
                        Label1.Text = "O programa detectou definições inválidas"
                        Label2.Text = "As definições inválidas foram repostas para os valores predefinidos. Verifique os campos abaixo para obter mais informações:"
                        Button1.Text = "OK"
                End Select
            Case 1
                Text = "Invalid settings have been detected"
                Label1.Text = "The program has detected invalid settings"
                Label2.Text = "The invalid settings have been reset to default values. Check the fields below for more information:"
                Button1.Text = "OK"
            Case 2
                Text = "Se han detectado configuraciones inválidas"
                Label1.Text = "El programa ha detectado configuraciones inválidas"
                Label2.Text = "Las configuraciones inválidas han sido restablecidas a sus valores predeterminados. Compruebe los campos de abajo para más información:"
                Button1.Text = "Aceptar"
            Case 3
                Text = "Des paramètres non valides ont été détectés"
                Label1.Text = "Le programme a détecté des paramètres non valides"
                Label2.Text = "Les paramètres non valides ont été réinitialisés aux valeurs par défaut. Vérifiez les champs ci-dessous pour plus d'informations :"
                Button1.Text = "OK"
            Case 4
                Text = "Foram detectadas definições inválidas"
                Label1.Text = "O programa detectou definições inválidas"
                Label2.Text = "As definições inválidas foram repostas para os valores predefinidos. Verifique os campos abaixo para obter mais informações:"
                Button1.Text = "OK"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            Label1.ForeColor = Color.FromArgb(0, 122, 204)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            Label1.ForeColor = Color.FromArgb(0, 51, 153)
        End If
        If MainForm.isExeProblematic Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label3.Text = "The specified DISM executable does not exist: " & CrLf & Quote & MainForm.ProblematicStrings(0) & Quote
                        Case "ESN"
                            Label3.Text = "El ejecutable de DISM especificado no existe: " & CrLf & Quote & MainForm.ProblematicStrings(0) & Quote
                        Case "FRA"
                            Label3.Text = "L'exécutable DISM spécifié n'existe pas : " & CrLf & Quote & MainForm.ProblematicStrings(0) & Quote
                        Case "PTB", "PTG"
                            Label3.Text = "O executável DISM especificado não existe: " & CrLf & Quote & MainForm.ProblematicStrings(0) & Quote
                    End Select
                Case 1
                    Label3.Text = "The specified DISM executable does not exist: " & CrLf & Quote & MainForm.ProblematicStrings(0) & Quote
                Case 2
                    Label3.Text = "El ejecutable de DISM especificado no existe: " & CrLf & Quote & MainForm.ProblematicStrings(0) & Quote
                Case 3
                    Label3.Text = "L'exécutable DISM spécifié n'existe pas : " & CrLf & Quote & MainForm.ProblematicStrings(0) & Quote
                Case 4
                    Label3.Text = "O executável DISM especificado não existe: " & CrLf & Quote & MainForm.ProblematicStrings(0) & Quote
            End Select
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label3.Text = "The DISM executable setting seems to be in order"
                        Case "ESN"
                            Label3.Text = "La configuración del ejecutable de DISM parece estar bien"
                        Case "FRA"
                            Label3.Text = "Le paramétrage de l'exécutable DISM semble être en ordre"
                        Case "PTB", "PTG"
                            Label3.Text = "A configuração do executável DISM parece estar em ordem"
                    End Select
                Case 1
                    Label3.Text = "The DISM executable setting seems to be in order"
                Case 2
                    Label3.Text = "La configuración del ejecutable de DISM parece estar bien"
                Case 3
                    Label3.Text = "Le paramétrage de l'exécutable DISM semble être en ordre"
                Case 4
                    Label3.Text = "A configuração do executável DISM parece estar em ordem"
            End Select
        End If
        If MainForm.isLogFontProblematic Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label4.Text = "The specified log font does not exist in this system: " & CrLf & Quote & MainForm.ProblematicStrings(1) & Quote
                        Case "ESN"
                            Label4.Text = "La fuente del registro especificada no existe en este sistema: " & CrLf & Quote & MainForm.ProblematicStrings(1) & Quote
                        Case "FRA"
                            Label4.Text = "La fonte spécifiée n'existe pas dans ce système : " & CrLf & Quote & MainForm.ProblematicStrings(1) & Quote
                        Case "PTB", "PTG"
                            Label4.Text = "A fonte de registo especificada não existe neste sistema: " & CrLf & Quote & MainForm.ProblematicStrings(1) & Quote
                    End Select
                Case 1
                    Label4.Text = "The specified log font does not exist in this system: " & CrLf & Quote & MainForm.ProblematicStrings(1) & Quote
                Case 2
                    Label4.Text = "La fuente del registro especificada no existe en este sistema: " & CrLf & Quote & MainForm.ProblematicStrings(1) & Quote
                Case 3
                    Label4.Text = "La fonte spécifiée n'existe pas dans ce système : " & CrLf & Quote & MainForm.ProblematicStrings(1) & Quote
                Case 4
                    Label4.Text = "A fonte de registo especificada não existe neste sistema: " & CrLf & Quote & MainForm.ProblematicStrings(1) & Quote
            End Select
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label4.Text = "The log font setting seems to be in order"
                        Case "ESN"
                            Label4.Text = "La configuración de la fuente de registro parece estar bien"
                        Case "FRA"
                            Label4.Text = "Le paramètre de la fonte du journal semble être dans l'ordre"
                        Case "PTB", "PTG"
                            Label4.Text = "A configuração da fonte de registo parece estar em ordem"
                    End Select
                Case 1
                    Label4.Text = "The log font setting seems to be in order"
                Case 2
                    Label4.Text = "La configuración de la fuente de registro parece estar bien"
                Case 3
                    Label4.Text = "Le paramètre de la fonte du journal semble être dans l'ordre"
                Case 4
                    Label4.Text = "A configuração da fonte de registo parece estar em ordem"
            End Select
        End If
        If MainForm.isLogFileProblematic Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label5.Text = "The specified log file does not exist: " & CrLf & Quote & MainForm.ProblematicStrings(2) & Quote
                        Case "ESN"
                            Label5.Text = "El archivo de registro especificado no existe: " & CrLf & Quote & MainForm.ProblematicStrings(2) & Quote
                        Case "FRA"
                            Label5.Text = "Le fichier journal spécifié n'existe pas : " & CrLf & Quote & MainForm.ProblematicStrings(2) & Quote
                        Case "PTB", "PTG"
                            Label5.Text = "O ficheiro de registo especificado não existe: " & CrLf & Quote & MainForm.ProblematicStrings(2) & Quote
                    End Select
                Case 1
                    Label5.Text = "The specified log file does not exist: " & CrLf & Quote & MainForm.ProblematicStrings(2) & Quote
                Case 2
                    Label5.Text = "El archivo de registro especificado no existe: " & CrLf & Quote & MainForm.ProblematicStrings(2) & Quote
                Case 3
                    Label5.Text = "Le fichier journal spécifié n'existe pas : " & CrLf & Quote & MainForm.ProblematicStrings(2) & Quote
                Case 4
                    Label5.Text = "O ficheiro de registo especificado não existe: " & CrLf & Quote & MainForm.ProblematicStrings(2) & Quote
            End Select
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label5.Text = "The log file setting seems to be in order"
                        Case "ESN"
                            Label5.Text = "La configuración del archivo de registro parece estar bien"
                        Case "FRA"
                            Label5.Text = "Le paramètre du fichier journal semble être dans l'ordre"
                        Case "PTB", "PTG"
                            Label5.Text = "A configuração do ficheiro de registo parece estar em ordem"
                    End Select
                Case 1
                    Label5.Text = "The log file setting seems to be in order"
                Case 2
                    Label5.Text = "La configuración del archivo de registro parece estar bien"
                Case 3
                    Label5.Text = "Le paramètre du fichier journal semble être dans l'ordre"
                Case 4
                    Label5.Text = "A configuração do ficheiro de registo parece estar em ordem"
            End Select
        End If
        If MainForm.isScratchDirProblematic Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label6.Text = "The specified scratch directory does not exist: " & CrLf & Quote & MainForm.ProblematicStrings(3) & Quote
                        Case "ESN"
                            Label6.Text = "El directorio temporal especificado no existe: " & CrLf & Quote & MainForm.ProblematicStrings(3) & Quote
                        Case "FRA"
                            Label6.Text = "Le répertoire temporaire spécifié n'existe pas : " & CrLf & Quote & MainForm.ProblematicStrings(3) & Quote
                        Case "PTB", "PTG"
                            Label6.Text = "O diretório temporário especificado não existe: " & CrLf & Quote & MainForm.ProblematicStrings(3) & Quote
                    End Select
                Case 1
                    Label6.Text = "The specified scratch directory does not exist: " & CrLf & Quote & MainForm.ProblematicStrings(3) & Quote
                Case 2
                    Label6.Text = "El directorio temporal especificado no existe: " & CrLf & Quote & MainForm.ProblematicStrings(3) & Quote
                Case 3
                    Label6.Text = "Le répertoire temporaire spécifié n'existe pas : " & CrLf & Quote & MainForm.ProblematicStrings(3) & Quote
                Case 4
                    Label6.Text = "O diretório temporário especificado não existe: " & CrLf & Quote & MainForm.ProblematicStrings(3) & Quote
            End Select
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label6.Text = "The scratch directory setting seems to be in order"
                        Case "ESN"
                            Label6.Text = "La configuración del directorio temporal parece estar bien"
                        Case "FRA"
                            Label6.Text = "Le paramètre du répertoire temporaire semble être dans l'ordre"
                        Case "PTB", "PTG"
                            Label6.Text = "A configuração do diretório temporário parece estar em ordem"
                    End Select
                Case 1
                    Label6.Text = "The scratch directory setting seems to be in order"
                Case 2
                    Label6.Text = "La configuración del directorio temporal parece estar bien"
                Case 3
                    Label6.Text = "Le paramètre du répertoire temporaire semble être dans l'ordre"
                Case 4
                    Label6.Text = "A configuração do diretório temporário parece estar em ordem"
            End Select
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
