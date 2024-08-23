Namespace Elements

    Public Class PasswordExpirationSettings

        ''' <summary>
        ''' The password expiration mode
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>See enum for more information</remarks>
        Public Property Mode As PasswordExpirationMode

        ''' <summary>
        ''' Determines whether or not to use default Windows values for password expiration (42 days)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property WindowsDefault As Boolean = True

        ''' <summary>
        ''' The number of days that need to pass before the password becomes invalid
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
        ''' <remarks>Not recommended by NIST</remarks>
        NIST_Limited = 1

    End Enum

End Namespace
