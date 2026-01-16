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
        ''' Gets a localized string displaying mount mode
        ''' </summary>
        ''' <param name="LangCode">The language code. 0 to automatically detect from system languages; 1-5 for independent languages</param>
        ''' <returns>The localized string</returns>
        Public Function DriverInboxToString(LangCode As Integer) As String
            Dim driverInboxString As String = ""

            If DriverInbox Then
                Select Case LangCode
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                driverInboxString = "Yes"
                            Case "ESN"
                                driverInboxString = "Sí"
                            Case "FRA"
                                driverInboxString = "Oui"
                            Case "PTB", "PTG"
                                driverInboxString = "Sim"
                            Case "ITA"
                                driverInboxString = "Sì"
                        End Select
                    Case 1
                        driverInboxString = "Yes"
                    Case 2
                        driverInboxString = "Sí"
                    Case 3
                        driverInboxString = "Oui"
                    Case 4
                        driverInboxString = "Sim"
                    Case 5
                        driverInboxString = "Sì"
                End Select
            Else
                Select Case LangCode
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                driverInboxString = "No"
                            Case "ESN"
                                driverInboxString = "No"
                            Case "FRA"
                                driverInboxString = "Non"
                            Case "PTB", "PTG"
                                driverInboxString = "Não"
                            Case "ITA"
                                driverInboxString = "No"
                        End Select
                    Case 1
                        driverInboxString = "No"
                    Case 2
                        driverInboxString = "No"
                    Case 3
                        driverInboxString = "Non"
                    Case 4
                        driverInboxString = "Não"
                    Case 5
                        driverInboxString = "No"
                End Select
            End If

            Return driverInboxString
        End Function

    End Class

End Namespace
