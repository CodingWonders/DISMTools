Public Class ServiceDependencyGraphViewer

    Public ServicesToDisplay As IEnumerable(Of WindowsService),
           MainServiceName As String

    Public AllSystemServices As List(Of WindowsService)

    Private Sub ServiceDependencyGraphViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        Dim mainServiceToDisplay As WindowsService = ServicesToDisplay.FirstOrDefault(Function(service) service.Name.Equals(MainServiceName, StringComparison.OrdinalIgnoreCase))

        ' Configure colors
        depDiagram.ArrowColor = CurrentTheme.ForegroundColor
        depDiagram.BackColor = CurrentTheme.BackgroundColor
        depDiagram.DiagramBackColor = CurrentTheme.BackgroundColor
        depDiagram.DescriptionTextColor = CurrentTheme.ForegroundColor
        depDiagram.NodeTextColor = CurrentTheme.ForegroundColor
        depDiagram.ForeColor = CurrentTheme.ForegroundColor
        depDiagram.MainNodeBackColor = CurrentTheme.AccentColors(0)
        depDiagram.MainNodeBorderColor = CurrentTheme.AccentColors(1)
        depDiagram.NodeBackColor = CurrentTheme.SectionBackgroundColor
        depDiagram.NodeBorderColor = CurrentTheme.ForegroundColor

        btnZoomIn.BackColor = CurrentTheme.SectionBackgroundColor
        btnZoomOut.BackColor = CurrentTheme.SectionBackgroundColor
        btnResetZoom.BackColor = CurrentTheme.SectionBackgroundColor
        btnSaveImage.BackColor = CurrentTheme.SectionBackgroundColor
        btnSaveToMermaid.BackColor = CurrentTheme.SectionBackgroundColor
        btnNavigateToService.BackColor = CurrentTheme.SectionBackgroundColor
        btnZoomIn.FlatAppearance.BorderColor = CurrentTheme.ForegroundColor
        btnZoomOut.FlatAppearance.BorderColor = CurrentTheme.ForegroundColor
        btnResetZoom.FlatAppearance.BorderColor = CurrentTheme.ForegroundColor
        btnSaveImage.FlatAppearance.BorderColor = CurrentTheme.ForegroundColor
        btnSaveToMermaid.FlatAppearance.BorderColor = CurrentTheme.ForegroundColor
        btnNavigateToService.FlatAppearance.BorderColor = CurrentTheme.ForegroundColor

        btnZoomIn.Image = GetGlyphResource("diagram_zoom_in")
        btnZoomOut.Image = GetGlyphResource("diagram_zoom_out")
        btnResetZoom.Image = GetGlyphResource("diagram_zoom_reset")
        btnSaveImage.Image = GetGlyphResource("diagram_save_image")
        btnSaveToMermaid.Image = GetGlyphResource("diagram_save_mermaid")
        btnNavigateToService.Image = GetGlyphResource("diagram_navigate_to_svc")

        depDiagram.SetGraph(mainServiceToDisplay, ServicesToDisplay)
    End Sub

    Public Sub RedisplayServices()
        Dim mainServiceToDisplay As WindowsService = ServicesToDisplay.FirstOrDefault(Function(service) service.Name.Equals(MainServiceName, StringComparison.OrdinalIgnoreCase))
        depDiagram.SetGraph(mainServiceToDisplay, ServicesToDisplay)
    End Sub

#Region "Zoom Controls"

    Private Sub ZoomIn()
        depDiagram.Zoom *= 1.10000002F
    End Sub

    Private Sub ZoomOut()
        depDiagram.Zoom /= 1.10000002F
    End Sub

    Private Sub ResetZoom()
        depDiagram.ResetView()
    End Sub

#End Region

    Private Sub btnResetZoom_Click(sender As Object, e As EventArgs) Handles btnResetZoom.Click
        ResetZoom()
    End Sub

    Private Sub btnZoomOut_Click(sender As Object, e As EventArgs) Handles btnZoomOut.Click
        ZoomOut()
    End Sub

    Private Sub btnZoomIn_Click(sender As Object, e As EventArgs) Handles btnZoomIn.Click
        ZoomIn()
    End Sub

    Private Sub btnSaveImage_Click(sender As Object, e As EventArgs) Handles btnSaveImage.Click
        If sfdDiagram.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            depDiagram.SaveAsImage(sfdDiagram.FileName)
        End If
    End Sub

    Private Sub btnSaveToMermaid_Click(sender As Object, e As EventArgs) Handles btnSaveToMermaid.Click
        If sfdMermaidDiagram.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim mermaidDiagram As String = depDiagram.SaveAsMermaid(),
                mdContents As String = ""

            mdContents = GetHeader(String.Format("Dependency diagram for service {0}", MainServiceName)) & CrLf & String.Format("```mermaid{0}{0}{1}{0}{0}```", Environment.NewLine, mermaidDiagram)
            File.WriteAllText(sfdMermaidDiagram.FileName, mdContents)
        End If
    End Sub

    Private Sub btnZoomIn_MouseHover(sender As Object, e As EventArgs) Handles btnZoomIn.MouseHover
        WindowHelper.DisplayToolTip(sender, "Zoom in")
    End Sub

    Private Sub btnZoomOut_MouseHover(sender As Object, e As EventArgs) Handles btnZoomOut.MouseHover
        WindowHelper.DisplayToolTip(sender, "Zoom out")
    End Sub

    Private Sub btnResetZoom_MouseHover(sender As Object, e As EventArgs) Handles btnResetZoom.MouseHover
        WindowHelper.DisplayToolTip(sender, "Zoom to fit on screen")
    End Sub

    Private Sub btnSaveImage_MouseHover(sender As Object, e As EventArgs) Handles btnSaveImage.MouseHover
        WindowHelper.DisplayToolTip(sender, "Save diagram as image...")
    End Sub

    Private Sub btnSaveToMermaid_MouseHover(sender As Object, e As EventArgs) Handles btnSaveToMermaid.MouseHover
        WindowHelper.DisplayToolTip(sender, "Save diagram as a Mermaid diagram for Markdown files...")
    End Sub

    Private Sub btnNavigateToService_MouseHover(sender As Object, e As EventArgs) Handles btnNavigateToService.MouseHover
        WindowHelper.DisplayToolTip(sender, "Navigate through service hierarchy...")
    End Sub

    Private Sub btnNavigateToService_Click(sender As Object, e As EventArgs) Handles btnNavigateToService.Click
        ServiceNavigatorDialog.NamesOfServiceDependencies = depDiagram.MainService.Dependencies.ToList()
        ServiceNavigatorDialog.NamesOfServiceDependents = depDiagram.Services.Where(Function(service) service.Dependencies.Select(Function(svc) svc.ToLowerInvariant()).Contains(MainServiceName.ToLowerInvariant())).Select(Function(service) service.Name).ToList()
        ServiceNavigatorDialog.NameOfCurrentService = MainServiceName
        If ServiceNavigatorDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim selectedService As WindowsService = AllSystemServices.FirstOrDefault(Function(service) service.Name.Equals(ServiceNavigatorDialog.NameOfFinalService, StringComparison.OrdinalIgnoreCase))
            If selectedService Is Nothing Then Exit Sub

            Dim dependencies As IEnumerable(Of WindowsService) = AllSystemServices.Where(Function(service) selectedService.Dependencies.Select(Function(svc) svc.ToLowerInvariant()).Contains(service.Name.ToLowerInvariant())).OrderBy(Function(service) service.DisplayName)
            Dim dependents As IEnumerable(Of WindowsService) = AllSystemServices.Where(Function(service) service.Dependencies.Select(Function(svc) svc.ToLowerInvariant()).Contains(selectedService.Name.ToLowerInvariant())).OrderBy(Function(service) service.DisplayName)

            Dim servicesToShow As New List(Of WindowsService)
            servicesToShow.Add(selectedService)
            servicesToShow.AddRange(dependencies)
            servicesToShow.AddRange(dependents)

            ServicesToDisplay = servicesToShow.AsEnumerable()
            MainServiceName = selectedService.Name

            RedisplayServices()
        End If
    End Sub

    Private Sub ServiceDependencyGraphViewer_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control Then
            Select Case e.KeyCode
                Case Keys.Oemplus, Keys.Add : ZoomIn()
                Case Keys.OemMinus, Keys.Subtract : ZoomOut()
                Case Keys.D0, Keys.NumPad0 : ResetZoom()
            End Select
        End If
    End Sub
End Class