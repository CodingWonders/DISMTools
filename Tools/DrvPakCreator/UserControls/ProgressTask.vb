Public Class ProgressTask

    Private _taskStatusPicture As Image
    Private _taskName As String = ""
    Private _taskStatus As ProgressTaskStatus = ProgressTaskStatus.NotStarted
    Private _taskPercentage As Double = 0
    Private Const _taskPercentageRounding As UShort = 0

    Public Property TaskName() As String
        Get
            Return _taskName
        End Get
        Set(ByVal value As String)
            _taskName = value

            Dim fullLabel As String = IIf(_taskName <> "", _taskName, "Task Name")
            If _taskPercentage >= 0 Then
                fullLabel &= String.Format(" ({0}%)", Math.Round(_taskPercentage, _taskPercentageRounding))
            End If

            lblTaskName.Text = fullLabel
        End Set
    End Property

    Public Property TaskStatus() As ProgressTaskStatus
        Get
            Return _taskStatus
        End Get
        Set(ByVal value As ProgressTaskStatus)
            Try
                Select Case value
                    Case ProgressTaskStatus.NotStarted : pbTaskStatus.Image = Nothing
                    Case ProgressTaskStatus.InProgress : pbTaskStatus.Image = GetGlyphResource("inprogress")
                    Case ProgressTaskStatus.Succeeded : pbTaskStatus.Image = GetGlyphResource("succeeded")
                    Case ProgressTaskStatus.Failed : pbTaskStatus.Image = GetGlyphResource("failed")
                    Case ProgressTaskStatus.Skipped : pbTaskStatus.Image = GetGlyphResource("skipped")
                End Select
            Catch ex As Exception
                pbTaskStatus.Image = Nothing
            End Try
            _taskStatus = value

            If value = ProgressTaskStatus.InProgress Then
                SetActive()
            Else
                SetInactive()
            End If
        End Set
    End Property

    Public Property TaskPercentage() As Double
        Get
            Return _taskPercentage
        End Get
        Set(ByVal value As Double)
            _taskPercentage = value

            Dim fullLabel As String = IIf(_taskName <> "", _taskName, "Task Name")
            If _taskPercentage >= 0 Then
                fullLabel &= String.Format(" ({0}%)", Math.Round(_taskPercentage, _taskPercentageRounding))
            End If

            lblTaskName.Text = fullLabel
        End Set
    End Property

    Public Sub SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
    End Sub

    Private Sub SetActive()
        lblTaskName.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
        If CurrentTheme IsNot Nothing Then
            lblTaskName.ForeColor = CurrentTheme.ForegroundColor
        Else
            lblTaskName.ForeColor = Color.Black
        End If
    End Sub

    Private Sub SetInactive()
        lblTaskName.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Regular)
        If CurrentTheme IsNot Nothing Then
            lblTaskName.ForeColor = CurrentTheme.DisabledForegroundColor
        Else
            lblTaskName.ForeColor = Color.Gray
        End If
    End Sub

End Class

Public Enum ProgressTaskStatus As Integer
    NotStarted = 0
    InProgress = 1
    Succeeded = 2
    Failed = 3
    Skipped = 4
End Enum