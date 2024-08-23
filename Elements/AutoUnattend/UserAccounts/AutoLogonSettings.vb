Namespace Elements

    Public Class AutoLogonSettings

        ''' <summary>
        ''' Determines whether to enable or disable auto-logon
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EnableAutoLogon As Boolean

        ''' <summary>
        ''' The auto-logon mode of the system
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>See enum for more information</remarks>
        Public Property LogonMode As AutoLogonMode

        ''' <summary>
        ''' The logon password for the first administrator if said option is selected
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LogonPassword As String

    End Class

    Public Enum AutoLogonMode As Integer

        ''' <summary>
        ''' First admin in user list
        ''' </summary>
        ''' <remarks></remarks>
        FirstAdmin = 0

        ''' <summary>
        ''' Windows built-in admin
        ''' </summary>
        ''' <remarks></remarks>
        WindowsAdmin = 1
    End Enum

End Namespace
