Namespace My
    ' Los siguientes eventos están disponibles para MyApplication:
    ' 
    ' Inicio: se desencadena cuando se inicia la aplicación, antes de que se cree el formulario de inicio.
    ' Apagado: generado después de cerrar todos los formularios de la aplicación. Este evento no se genera si la aplicación termina de forma anómala.
    ' UnhandledException: generado si la aplicación detecta una excepción no controlada.
    ' StartupNextInstance: se desencadena cuando se inicia una aplicación de instancia única y la aplicación ya está activa. 
    ' NetworkAvailabilityChanged: se desencadena cuando la conexión de red está conectada o desconectada.
    Partial Friend Class MyApplication

        Public Sub CatchEmAll(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            DynaLog.LogMessage("Unhandled exception occurred. Gotta catch'em all !")
            DynaLog.LogMessage("Exception information:")
            DynaLog.LogMessage("- Message: " & e.Exception.Message)
            DynaLog.LogMessage("- Code (HRESULT): " & Hex(e.Exception.HResult))
            ' Gotta catch 'em all !
            Installer.MainForm.LogErrorMessage(e.Exception, InstallationStage.InstallerStage.Miscellaneous)
            Installer.MainForm.ChangePage(WizardPage.Page.FailurePage, True)
            e.ExitApplication = False
        End Sub

    End Class


End Namespace

