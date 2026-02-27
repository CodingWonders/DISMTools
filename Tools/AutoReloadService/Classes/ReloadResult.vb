Namespace Classes

    Public Enum ReloadResultType As Integer
        Unknown = -1
        Succeeded = 0
        Failed = 1
        Skipped = 2
    End Enum

    Public Class ReloadResult
        Public Property ResultType As ReloadResultType
        Public Property ResultCode As Integer

        Public Sub New(type As ReloadResultType, code As Integer)
            ResultType = type
            ResultCode = code
        End Sub
    End Class

End Namespace
