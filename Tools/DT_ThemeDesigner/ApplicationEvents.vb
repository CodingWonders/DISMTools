Imports Microsoft.VisualBasic.ControlChars

Namespace My

    ' Los siguientes eventos estn disponibles para MyApplication:
    ' 
    ' Inicio: se desencadena cuando se inicia la aplicacin, antes de que se cree el formulario de inicio.
    ' Apagado: generado despus de cerrar todos los formularios de la aplicacin. Este evento no se genera si la aplicacin termina de forma anmala.
    ' UnhandledException: generado si la aplicacin detecta una excepcin no controlada.
    ' StartupNextInstance: se desencadena cuando se inicia una aplicacin de instancia nica y la aplicacin ya est activa. 
    ' NetworkAvailabilityChanged: se desencadena cuando la conexin de red est conectada o desconectada.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            LocalizationService.Initialize()
        End Sub

        Public Sub CatchEmAll(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles MyBase.UnhandledException
            MsgBox(LocalizationService.ForSection("ThemeDesigner.Errors").Format("InternalError.Message", e.Exception.ToString()), vbOKOnly + vbCritical, LocalizationService.ForSection("ThemeDesigner.Errors")("UnhandledError.Message"))
        End Sub

    End Class

End Namespace

