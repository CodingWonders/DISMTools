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

        Public Sub CatchEmAll(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles MyBase.UnhandledException
            MsgBox("An internal error has occurred: " & CrLf & CrLf & e.Exception.ToString() & CrLf & CrLf & "Report this issue to the developers.", vbOKOnly + vbCritical, "Unhandled Error")
        End Sub

    End Class

End Namespace

