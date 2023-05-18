Public Class MigrationForm

    Dim msg As String

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Threading.Thread.Sleep(2000)
        msg = "Loading old settings file..."
        BackgroundWorker1.ReportProgress(33.3)
        MainForm.LoadDTSettings(1)
        Threading.Thread.Sleep(250)
        msg = "Saving new settings file..."
        BackgroundWorker1.ReportProgress(66.6)
        MainForm.SaveDTSettings()
        Threading.Thread.Sleep(250)
        msg = "Done"
        BackgroundWorker1.ReportProgress(100)
        Threading.Thread.Sleep(1000)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Style = ProgressBarStyle.Blocks
        Label2.Text = msg
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub MigrationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Refresh()
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Close()
    End Sub
End Class