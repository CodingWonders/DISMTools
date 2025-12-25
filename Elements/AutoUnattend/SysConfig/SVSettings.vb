Namespace Elements

    ''' <summary>
    ''' Windows 11 Settings
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SVSettings

        ''' <summary>
        ''' Determines whether to bypass system requirements in Windows Setup
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>This does not make a difference if the installation image is deployed with third-party PEs, such as the DISMTools Preinstallation Environment</remarks>
        Public Property LabConfig_BypassRequirements As Boolean

        ''' <summary>
        ''' Determines whether to bypass mandatory network connection setup in the Windows 11 Out-of-Box Experience (OOBE)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Using unattended files still works at defeating this requirement. However, Shift+F10 no longer works on Windows 11 24H2+</remarks>
        Public Property OOBE_BypassNRO As Boolean
    End Class

End Namespace
