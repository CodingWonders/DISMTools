Namespace Elements

    Public Class AutoLogonSettings

        Public Property EnableAutoLogon As Boolean

        Public Property LogonMode As AutoLogonMode

        Public Property LogonPassword As String

    End Class

    Public Enum AutoLogonMode As Integer
        FirstAdmin = 0
        WindowsAdmin = 1
    End Enum

End Namespace
