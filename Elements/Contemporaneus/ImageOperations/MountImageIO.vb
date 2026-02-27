Imports System.IO

Namespace Elements.Contemporaneus.ImageOperations

    Public Class MountImageIO
        Inherits ImageOperation

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(executor As Func(Of String, String, Integer))
            MyBase.New(executor)
        End Sub

        Public Overrides Function RunOperation() As Integer
            DynaLog.LogMessage("Preparing to mount the Windows image...")
            DynaLog.LogMessage("- Image file to mount: " & Quote & GetProperty("SourceImg") & Quote)
            DynaLog.LogMessage("- Image index to mount: " & GetProperty("ImgIndex"))
            DynaLog.LogMessage("- Location to mount image to: " & Quote & GetProperty("MountDir") & Quote)
            DynaLog.LogMessage("- Mount with read-only permissions? " & If(GetProperty("IsReadOnly"), "Yes", "No"))
            DynaLog.LogMessage("- Optimize mount times? " & If(GetProperty("IsOptimized"), "Yes", "No"))
            DynaLog.LogMessage("- Check image integrity? " & If(GetProperty("IsIntegrityTested"), "Yes", "No"))
            ReportLogAllTasks("Mounting image...")
            ReportLogCurrTask("Mounting specified image...")
            ReportLogActivity(CrLf & "Mounting image..." & CrLf & "Options:" & CrLf &
                                     "- Image file: " & GetProperty("SourceImg") & CrLf &
                                     "- Image index: " & GetProperty("ImgIndex") & CrLf &
                                     "- Mount point: " & GetProperty("MountDir"))
            Try
                If Not GetProperty("IsReadOnly") AndAlso (File.GetAttributes(GetProperty("SourceImg")) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                    DynaLog.LogMessage("Source image contains read-only flag. Attempting to remove it...")
                    ' Remove readonly flag
                    File.SetAttributes(GetProperty("SourceImg"), (File.GetAttributes(GetProperty("SourceImg")) And Not FileAttributes.ReadOnly))
                    DynaLog.LogMessage("Flags were removed successfully.")
                End If
            Catch ex As Exception
                DynaLog.LogMessage("Could not remove or get flags. Error message: " & ex.Message)
            End Try
            Select Case GetProperty("DismVersionChecker").ProductMajorPart
                Case 6
                    Select Case GetProperty("DismVersionChecker").ProductMinorPart
                        Case 1
                            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /mount-wim /wimfile=" & Quote & GetProperty("SourceImg") & Quote & " /index=" & GetProperty("ImgIndex") & " /MountDir=" & Quote & GetProperty("MountDir") & Quote
                        Case Is >= 2
                            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /mount-image /imagefile=" & Quote & GetProperty("SourceImg") & Quote & " /index=" & GetProperty("ImgIndex") & " /MountDir=" & Quote & GetProperty("MountDir") & Quote
                    End Select
                Case 10
                    CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /mount-image /imagefile=" & Quote & GetProperty("SourceImg") & Quote & " /index=" & GetProperty("ImgIndex") & " /MountDir=" & Quote & GetProperty("MountDir") & Quote
            End Select
            If GetProperty("IsReadOnly") Then
                ReportLogActivity(CrLf & "- Mount image with read-only permissions? Yes")
                CommandArgs &= " /readonly"
            Else
                ReportLogActivity(CrLf & "- Mount image with read-only permissions? No")
            End If
            If GetProperty("IsOptimized") Then
                ReportLogActivity(CrLf & "- Optimize mount time? Yes")
                CommandArgs &= " /optimize"
            Else
                ReportLogActivity(CrLf & "- Optimize mount time? No")
            End If
            If GetProperty("IsIntegrityTested") Then
                ReportLogActivity(CrLf & "- Check image integrity? Yes")
                CommandArgs &= " /checkintegrity"
            Else
                ReportLogActivity(CrLf & "- Check image integrity? No")
            End If
            Dim errCode As Integer = RunProcess(GetProperty("DismProgram"), CommandArgs)
            ReportLogCurrTask("Gathering error level...")
            ReportLogActivity(CrLf & "Gathering error level...")
            ReportLogActivity(CrLf & CrLf & "    Error level : 0x" & errCode)

            Return errCode
        End Function
    End Class

End Namespace