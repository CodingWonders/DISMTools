Namespace Elements

    Public Class AccountLockdownSettings

        Public Property Enabled As Boolean = True

        Public Property DefaultPolicy As Boolean = True

        Public Property TimedLockdownSettings As New TimedLockdown()

    End Class

    Public Class TimedLockdown

        Public Property FailedAttempts As Integer

        Public Property Timeframe As Integer

        Public Property AutoUnlockTime As Integer

    End Class

End Namespace
