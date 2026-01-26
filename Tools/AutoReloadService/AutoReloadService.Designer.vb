Imports System.ServiceProcess

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AutoReloadService
    Inherits System.ServiceProcess.ServiceBase

    'UserService reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    ' Punto de entrada principal del proceso
    <MTAThread()> _
    <System.Diagnostics.DebuggerNonUserCode()> _
    Shared Sub Main()
        Dim ServicesToRun() As System.ServiceProcess.ServiceBase

        ' Puede que más de un servicio de NT se ejecute con el mismo proceso. Para agregar
        ' otro servicio a este proceso, cambie la siguiente línea para
        ' crear un segundo objeto de servicio. Por ejemplo,
        '
        '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1, New MySecondUserService}
        '
        ServicesToRun = New System.ServiceProcess.ServiceBase() {New AutoReloadService}

        System.ServiceProcess.ServiceBase.Run(ServicesToRun)
    End Sub

    'Requerido por el Diseñador de componentes
    Private components As System.ComponentModel.IContainer

    ' NOTA: el Diseñador de componentes requiere el siguiente procedimiento
    ' Se puede modificar utilizando el Diseñador de componentes.  
    ' No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.AutoReloadEL = New System.Diagnostics.EventLog()
        CType(Me.AutoReloadEL, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'AutoReloadEL
        '
        Me.AutoReloadEL.Log = "Application"
        Me.AutoReloadEL.Source = "DT-AutoReload"
        '
        'AutoReloadService
        '
        Me.CanShutdown = True
        Me.ServiceName = "DT_AutoReload"
        CType(Me.AutoReloadEL, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents AutoReloadEL As System.Diagnostics.EventLog

End Class
