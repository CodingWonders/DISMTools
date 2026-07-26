Imports System

' Runtime localization is intentionally kept outside Windows Forms designer files.
' English design-time text remains available to the Visual Studio form designer.

Partial Class EventProperties

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("Num.Events.Label")
        Me.Label2.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("EventTimestamp.Label")
        Me.GroupBox1.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("MethodCallers.Group")
        Me.LinkLabel1.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("Field.Empty.Link")
        Me.Label4.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("Method.Function.Label")
        Me.Label3.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("Logged.Method.Function.Label")
        Me.Label6.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("PID.Label")
        Me.Label5.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("EventMessage.Label")
        Me.btnNextEvent.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("NextEvent.Label")
        Me.btnPreviousEvent.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("PreviousEvent.Label")
        Me.Text = LocalizationService.ForSection("DynaViewer.Designer.EventProps")("EventProps.Label")
    End Sub

End Class

Partial Class MainForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Dyna.Log.File.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("DynaViewer.Designer.Main")("LogFiles.Filter")
        Me.GroupBox1.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Dyna.Log.Event")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("EventTimestamp.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("ProcessID.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("EventCaller.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Message.Column")
        Me.Button1.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Browse.Button")
        Me.Button2.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Refresh.Button")
        Me.Label2.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Processed.Entries.Label")
        Me.Button3.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("About.ActionButton")
        Me.LightCM_TSMI.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("LightCM.Label")
        Me.DarkCM_TSMI.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("DarkCM.Label")
        Me.SystemCM_TSMI.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("SystemCM.Label")
        Me.Button4.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("ColorMode.Button")
        Me.Label4.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("PID.Label")
        Me.Label5.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("EventCaller.Label")
        Me.Label6.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Options.Heading.Label")
        Me.RegexCB.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("RegexCB.Label")
        Me.RegexFailureBtn.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Regex.Failure.Btn.Label")
        Me.CaseSensitiveCB.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Aa.Label")
        Me.Label3.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Message.Label")
        Me.Text = LocalizationService.ForSection("DynaViewer.Designer.Main")("Dyna.Log.Viewer.Label")
    End Sub

End Class

Partial Class RegexCheatsheet

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("DynaViewer.Designer.Regex")("CheatsheetHelp.Label")
        Me.TextBox1.Text = LocalizationService.ForSection("DynaViewer.Designer.Regex")("CharacterClasses.Message")
        Me.CheckBox1.Text = LocalizationService.ForSection("DynaViewer.Designer.Regex")("PinTop.CheckBox")
        Me.Text = LocalizationService.ForSection("DynaViewer.Designer.Regex")("RegexCheatsheet.Label")
    End Sub

End Class
