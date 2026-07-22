Imports System.Windows.Forms

Module ProgressReporter

    Private progressForm As Form
    Private progressLabel As Label
    Private progressBarContainer As Panel
    Private progressBarCompletedProgress As Panel

    Private ProgressMessage As String = ""

    Private Sub InitializeForm()
        progressForm = New Form With {
            .StartPosition = FormStartPosition.CenterScreen,
            .Text = LocalizationService.ForSection("ProgressReporter")("Progress.Label"),
            .Size = WindowHelper.ScaleSizeLogical(384, 72),
            .FormBorderStyle = FormBorderStyle.None,
            .MinimizeBox = False,
            .MaximizeBox = False,
            .BackColor = CurrentTheme.BackgroundColor,
            .ForeColor = CurrentTheme.ForegroundColor,
            .ShowIcon = False,
            .ShowInTaskbar = False,
            .Cursor = Cursors.WaitCursor,
            .AutoScaleMode = AutoScaleMode.Dpi
        }
        progressLabel = New Label With {
            .Location = WindowHelper.ScalePositionLogical(4, 4),
            .Size = WindowHelper.ScaleSizeLogical(374, 56),
            .TextAlign = ContentAlignment.MiddleCenter,
            .AutoEllipsis = True,
            .AutoSize = False,
            .Font = New Font("Segoe UI", 11.25)
        }
        progressBarContainer = New Panel With {
            .Location = WindowHelper.ScalePositionLogical(8, 62),
            .Size = WindowHelper.ScaleSizeLogical(368, 2),
            .BackColor = CurrentTheme.DisabledForegroundColor
        }
        progressBarCompletedProgress = New Panel With {
            .Location = WindowHelper.ScalePositionLogical(8, 62),
            .Size = WindowHelper.ScaleSizeLogical(0, 2),
            .BackColor = CurrentTheme.ForegroundColor
        }
        progressForm.Controls.AddRange(New Control() {progressLabel, progressBarContainer, progressBarCompletedProgress})
        progressBarCompletedProgress.BringToFront()

        AddHandler progressForm.Paint, Sub(sender, e)
                                           ControlPaint.DrawBorder(e.Graphics, progressForm.ClientRectangle, CurrentTheme.AccentColors(1), ButtonBorderStyle.Solid)
                                       End Sub

        ' Enable double buffering (https://stackoverflow.com/questions/76993/how-to-double-buffer-net-controls-on-a-form)
        If Not SystemInformation.TerminalServerSession Then
            Dim aProp As Reflection.PropertyInfo = GetType(Control).GetProperty("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
            aProp.SetValue(progressForm, True, Nothing)
        End If
    End Sub

    Public Sub Show(sender As Object)
        If progressForm IsNot Nothing AndAlso
            progressLabel IsNot Nothing AndAlso
            progressBarContainer IsNot Nothing AndAlso
            progressBarCompletedProgress IsNot Nothing Then
            ' Dispose them
            progressForm.Dispose()
            progressLabel.Dispose()
            progressBarContainer.Dispose()
            progressBarCompletedProgress.Dispose()
        End If
        InitializeForm()
        progressForm.ShowDialog(sender)
    End Sub

    Public Sub Hide()
        ProgressMessage = ""
        progressForm.Close()
    End Sub

    Public Sub ReportProgress(sender As Object, Optional Percentage As Double = -1)
        If progressForm Is Nothing OrElse Not progressForm.Visible Then
            Show(sender)
        End If
        progressLabel.Text = ProgressMessage
        If Percentage >= 0 AndAlso (Not Percentage > 100) Then
            progressBarCompletedProgress.Width = progressBarContainer.Width * (Percentage / 100)
        End If
        progressForm.Refresh()
    End Sub

    Public Sub SetMessage(message As String)
        ProgressMessage = message
    End Sub

End Module
