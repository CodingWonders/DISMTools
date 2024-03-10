Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars

Namespace My
    ' Los siguientes eventos están disponibles para MyApplication:
    ' 
    ' Inicio: se desencadena cuando se inicia la aplicación, antes de que se cree el formulario de inicio.
    ' Apagado: generado después de cerrar todos los formularios de la aplicación. Este evento no se genera si la aplicación termina de forma anómala.
    ' UnhandledException: generado si la aplicación detecta una excepción no controlada.
    ' StartupNextInstance: se desencadena cuando se inicia una aplicación de instancia única y la aplicación ya está activa. 
    ' NetworkAvailabilityChanged: se desencadena cuando la conexión de red está conectada o desconectada.
    Partial Friend Class MyApplication

        Private Sub Start(sender As Object, e As EventArgs) Handles Me.Startup
            AddHandler Microsoft.Win32.SystemEvents.UserPreferenceChanged, AddressOf SysEvts_UserPreferenceChanged
            AddHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanging, AddressOf SysEvts_DisplaySettingsChanging
            AddHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, AddressOf SysEvts_DisplaySettingsChanged
        End Sub

        Private Sub CatchEmAll(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            ExceptionForm.ErrorText.Text = e.Exception.ToString() & CrLf & CrLf &
                                           "Error Message: " & e.Exception.Message & CrLf & CrLf &
                                           "Error Code (HRESULT): " & e.Exception.HResult
            ExceptionForm.ShowDialog()
            If ExceptionForm.DialogResult = DialogResult.OK Then
                e.ExitApplication = False
            ElseIf ExceptionForm.DialogResult = DialogResult.Cancel Then
                e.ExitApplication = True
            End If
        End Sub

        Private Sub SysEvts_UserPreferenceChanged(sender As Object, e As Microsoft.Win32.UserPreferenceChangedEventArgs)
            ' Do nothing
        End Sub

        Private Sub SysEvts_DisplaySettingsChanged(sender As Object, e As EventArgs)
            ' Do nothing
        End Sub

        Private Sub SysEvts_DisplaySettingsChanging(sender As Object, e As EventArgs)
            ' Do nothing
        End Sub

    End Class


End Namespace

