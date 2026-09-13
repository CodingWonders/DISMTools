Imports Microsoft.Win32
Public Class MigrationForm

    Dim msg As String

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        DynaLog.LogMessage("Beginning migration...")
        DynaLog.LogMessage("Loading old settings file...")
        msg = LocalizationService.ForSection("Migration.Background")("Loading.Old.Settings.Message")
        BackgroundWorker1.ReportProgress(33.299999999999997)
        MainForm.LoadDTSettings(1)
        Threading.Thread.Sleep(72)
        DynaLog.LogMessage("Saving new settings...")
        MainForm.Width = WindowHelper.ScaleLogical(1280)
        MainForm.Height = WindowHelper.ScaleLogical(720)
        msg = LocalizationService.ForSection("Migration.Background")("Saving.New.Settings.Message")
        BackgroundWorker1.ReportProgress(66.599999999999994)
        MainForm.SaveDTSettings()
        Threading.Thread.Sleep(72)
        msg = LocalizationService.ForSection("Migration.Background")("Done.Message")
        BackgroundWorker1.ReportProgress(100)
        Threading.Thread.Sleep(250)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Style = ProgressBarStyle.Blocks
        Label2.Text = msg
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub MigrationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Label1.Text = LocalizationService.ForSection("Migration")("Wait.Message")
        Label2.Text = LocalizationService.ForSection("Migration")("Wait.Label")
        Refresh()
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Close()
    End Sub

    Private Sub MigrationForm_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.FromArgb(53, 153, 41), ButtonBorderStyle.Solid)
    End Sub
End Class
