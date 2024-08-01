Namespace Elements

    Public Class PasswordExpirationSettings

        Public Property Mode As PasswordExpirationMode

        Public Property WindowsDefault As Boolean = True

        Public Property Days As Integer

    End Class

    Public Enum PasswordExpirationMode As Integer

        ''' <summary>
        ''' Passwords should never expire
        ''' </summary>
        ''' <remarks>Recommended by NIST</remarks>
        NIST_Unlimited = 0

        ''' <summary>
        ''' Passwords should expire after a certain amount of days
        ''' </summary>
        ''' <remarks></remarks>
        NIST_Limited = 1

    End Enum

End Namespace
