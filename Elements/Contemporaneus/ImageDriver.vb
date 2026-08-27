Namespace Elements.Contemporaneus

    Public Class ImageDriver

        Public Property DriverPublishedName As String
        Public Property DriverOriginalFileName As String
        Public Property DriverInbox As Boolean
        Public Property DriverClassName As String
        Public Property DriverProviderName As String
        Public Property DriverDate As String
        Public Property DriverVersion As Version

        Public Sub New(publishedName As String, originalFileName As String, inbox As Boolean, className As String, providerName As String, publishedDate As String, version As Version)
            DriverPublishedName = publishedName
            DriverOriginalFileName = originalFileName
            DriverInbox = inbox
            DriverClassName = className
            DriverProviderName = providerName
            DriverDate = publishedDate
            DriverVersion = version
        End Sub

        ''' <summary>
        ''' Gets the driver inbox state in the current application language.
        ''' </summary>
        Public Function DriverInboxToString() As String
            If DriverInbox Then
                Return LocalizationService.ForSection("ImageDriver.DriverInbox")("Yes.Button")
            End If

            Return LocalizationService.ForSection("ImageDriver.DriverInbox")("No.Button")
        End Function

    End Class

End Namespace
