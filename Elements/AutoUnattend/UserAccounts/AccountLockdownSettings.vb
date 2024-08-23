Namespace Elements

    Public Class AccountLockdownSettings

        ''' <summary>
        ''' Determines whether to enable account lockdown
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Disabling these policies will make a system more vulnerable to brute-force attacks</remarks>
        Public Property Enabled As Boolean = True

        ''' <summary>
        ''' Determines whether to use default Windows lockdown policies
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DefaultPolicy As Boolean = True

        ''' <summary>
        ''' Timed lockdown settings in case the default policy is not used
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>See class declaration for more information</remarks>
        Public Property TimedLockdownSettings As New TimedLockdown()

    End Class

    Public Class TimedLockdown

        ''' <summary>
        ''' The number of failed attempts at signing in before the account is locked down
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Default value: 10 attempts</remarks>
        Public Property FailedAttempts As Integer

        ''' <summary>
        ''' The timeframe (in minutes) in which the number of failed attempts can be exceeded to enable lockdown on an account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Default value: 10 minutes</remarks>
        Public Property Timeframe As Integer

        ''' <summary>
        ''' The amount of time (in minutes) that the user needs to wait in order for the account to become available
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Default value: 10 minutes</remarks>
        Public Property AutoUnlockTime As Integer

    End Class

End Namespace
