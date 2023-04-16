Imports DarkUI.Docking
Public Class Actions_MainForm

    Dim _toolWindows As New List(Of DarkDockContent)
    Private _ActionProperties As ActionProperties = New ActionProperties()

    Private Sub Actions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Application.AddMessageFilter(DarkDockPanel1.DockContentDragFilter)
        Application.AddMessageFilter(DarkDockPanel1.DockResizeFilter)
    End Sub
End Class