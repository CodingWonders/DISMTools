Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Linq
Imports System.Windows.Forms

Public Class ServiceDependencyDiagram
    Inherits Control

#Region "Nested Types"

    Private Class DiagramNode
        Public Property Service As WindowsService
        Public Property X As Single
        Public Property Y As Single
        Public Property Width As Single
        Public Property Height As Single
        Public Property Level As Integer
        Public Property IsMain As Boolean

        Public ReadOnly Property Bounds As RectangleF
            Get
                Return New RectangleF(X, Y, Width, Height)
            End Get
        End Property

        Public ReadOnly Property Center As PointF
            Get
                Return New PointF(X + Width / 2.0F, Y + Height / 2.0F)
            End Get
        End Property
    End Class

    Private Class DiagramConnection
        Public Property Source As DiagramNode
        Public Property Target As DiagramNode
    End Class

#End Region

#Region "Fields"

    Private _mainService As WindowsService
    Private _services As New List(Of WindowsService)()

    Private ReadOnly _nodes As New List(Of DiagramNode)()
    Private ReadOnly _connections As New List(Of DiagramConnection)()

    Private _zoom As Single = 1.0F
    Private _pan As PointF = PointF.Empty

    Private _isPanning As Boolean
    Private _lastMousePosition As Point

    Private ReadOnly NodeWidth As Single = WindowHelper.ScaleLogical(250)
    Private ReadOnly NodeHeight As Single = WindowHelper.ScaleLogical(126)

    Private ReadOnly HorizontalSpacing As Single = WindowHelper.ScaleLogical(100)
    Private ReadOnly VerticalSpacing As Single = WindowHelper.ScaleLogical(28)

    Private ReadOnly OuterPadding As Single = WindowHelper.ScaleLogical(80)

    Private Const MinimumZoom As Single = 0.2F
    Private Const MaximumZoom As Single = 3.0F

#End Region

#Region "Appearance"

    Private _nodeBackColor As Color = Color.FromArgb(48, 48, 48)
    Private _mainNodeBackColor As Color = Color.FromArgb(64, 86, 120)
    Private _nodeBorderColor As Color = Color.FromArgb(120, 120, 120)
    Private _mainNodeBorderColor As Color = Color.FromArgb(100, 170, 255)
    Private _nodeTextColor As Color = Color.White
    Private _descriptionTextColor As Color = Color.FromArgb(220, 220, 220)
    Private _arrowColor As Color = Color.FromArgb(150, 150, 150)
    Private _backgroundColor As Color = Color.FromArgb(32, 32, 32)

    ''' <summary>
    ''' Gets or sets the background color of the diagram.
    ''' </summary>
    Public Property DiagramBackColor As Color
        Get
            Return _backgroundColor
        End Get
        Set(value As Color)
            _backgroundColor = value
            Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the background color of ordinary nodes.
    ''' </summary>
    Public Property NodeBackColor As Color
        Get
            Return _nodeBackColor
        End Get
        Set(value As Color)
            _nodeBackColor = value
            Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the background color of the main node.
    ''' </summary>
    Public Property MainNodeBackColor As Color
        Get
            Return _mainNodeBackColor
        End Get
        Set(value As Color)
            _mainNodeBackColor = value
            Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the border color of ordinary nodes.
    ''' </summary>
    Public Property NodeBorderColor As Color
        Get
            Return _nodeBorderColor
        End Get
        Set(value As Color)
            _nodeBorderColor = value
            Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the border color of the main node.
    ''' </summary>
    Public Property MainNodeBorderColor As Color
        Get
            Return _mainNodeBorderColor
        End Get
        Set(value As Color)
            _mainNodeBorderColor = value
            Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the text color of nodes.
    ''' </summary>
    Public Property NodeTextColor As Color
        Get
            Return _nodeTextColor
        End Get
        Set(value As Color)
            _nodeTextColor = value
            Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the description text color.
    ''' </summary>
    Public Property DescriptionTextColor As Color
        Get
            Return _descriptionTextColor
        End Get
        Set(value As Color)
            _descriptionTextColor = value
            Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the arrow color.
    ''' </summary>
    Public Property ArrowColor As Color
        Get
            Return _arrowColor
        End Get
        Set(value As Color)
            _arrowColor = value
            Invalidate()
        End Set
    End Property

    <DefaultValue(8)>
    Public Property NodeCornerRadius As Integer = 8

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the service displayed at the center of the diagram.
    ''' </summary>
    Public ReadOnly Property MainService As WindowsService
        Get
            Return _mainService
        End Get
    End Property

    ''' <summary>
    ''' Gets the services currently used to resolve relationships.
    ''' </summary>
    Public ReadOnly Property Services As IList(Of WindowsService)
        Get
            Return _services.AsReadOnly()
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the diagram zoom factor.
    ''' </summary>
    Public Property Zoom As Single
        Get
            Return _zoom
        End Get
        Set(value As Single)
            _zoom = Math.Max(MinimumZoom, Math.Min(MaximumZoom, value))
            Invalidate()
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        BackColor = _backgroundColor
        ForeColor = Color.White
        DoubleBuffered = True
        TabStop = True
    End Sub

#End Region

#Region "Graph Setup"

    ''' <summary>
    ''' Sets the main service and the collection of services used to resolve
    ''' dependencies and dependents.
    ''' </summary>
    ''' <param name="mainService">The service to display in the center.</param>
    ''' <param name="services">
    ''' The complete collection of available services. This collection should
    ''' include the main service and its related services.
    ''' </param>
    Public Sub SetGraph(mainService As WindowsService, services As IEnumerable(Of WindowsService))
        _mainService = mainService
        _services = If(services Is Nothing, New List(Of WindowsService)(), services.Where(Function(service) service IsNot Nothing).ToList())

        RebuildGraph()
        ResetView()
    End Sub

    ''' <summary>
    ''' Rebuilds the nodes and connections from the current service collection.
    ''' </summary>
    Public Sub RebuildGraph()
        _nodes.Clear()
        _connections.Clear()

        If _mainService Is Nothing Then
            Invalidate()
            Exit Sub
        End If

        Dim nodesByName As New Dictionary(Of String, DiagramNode)(StringComparer.OrdinalIgnoreCase)

        ' The main node.
        Dim mainNode As New DiagramNode With {
            .Service = _mainService,
            .Level = 0,
            .IsMain = True,
            .Width = NodeWidth,
            .Height = NodeHeight
        }

        _nodes.Add(mainNode)
        nodesByName(_mainService.Name) = mainNode

        ' Build the dependency tree to the left.
        Dim visitedDependencies As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
            _mainService.Name
        }

        BuildDependencyTree(_mainService, mainNode, 1, nodesByName, visitedDependencies)

        ' Build the direct dependent list to the right.
        '
        ' A dependent is a service whose Dependencies array contains
        ' the main service name.
        Dim dependentServices As New List(Of WindowsService)()

        For Each service As WindowsService In _services
            If service Is Nothing OrElse String.Equals(service.Name, _mainService.Name, StringComparison.OrdinalIgnoreCase) Then Continue For

            If ServiceDependsOn(service, _mainService.Name) Then dependentServices.Add(service)
        Next

        ' If the main service was not present in the supplied collection,
        ' the direct dependent list can still be resolved from the collection.
        For Each service As WindowsService In dependentServices
            If nodesByName.ContainsKey(service.Name) Then Continue For

            Dim dependentNode As New DiagramNode With {
                .Service = service,
                .Level = 1,
                .IsMain = False,
                .Width = NodeWidth,
                .Height = NodeHeight
            }

            _nodes.Add(dependentNode)
            nodesByName(service.Name) = dependentNode

            _connections.Add(New DiagramConnection With {
                .Source = mainNode,
                .Target = dependentNode
            })
        Next

        LayoutGraph()
        Invalidate()
    End Sub

    Private Sub BuildDependencyTree(
        service As WindowsService,
        parentNode As DiagramNode,
        level As Integer,
        nodesByName As Dictionary(Of String, DiagramNode),
        visited As HashSet(Of String))

        If service Is Nothing OrElse service.Dependencies Is Nothing Then Exit Sub

        For Each dependencyName As String In service.Dependencies
            If String.IsNullOrWhiteSpace(dependencyName) Then Continue For

            ' Avoid cycles.
            If visited.Contains(dependencyName) Then Continue For

            Dim dependencyService As WindowsService =
                FindServiceByName(dependencyName)

            If dependencyService Is Nothing Then
                ' A dependency may exist in the registry but not be
                ' represented in the supplied collection.
                Continue For
            End If

            Dim dependencyNode As DiagramNode = Nothing

            If nodesByName.ContainsKey(dependencyService.Name) Then
                dependencyNode = nodesByName(dependencyService.Name)
            Else
                dependencyNode = New DiagramNode With {
                    .Service = dependencyService,
                    .Level = -level,
                    .IsMain = False,
                    .Width = NodeWidth,
                    .Height = NodeHeight
                }

                _nodes.Add(dependencyNode)
                nodesByName(dependencyService.Name) = dependencyNode
            End If

            ' Dependencies point toward the service that requires them.
            _connections.Add(New DiagramConnection With {
                .Source = dependencyNode,
                .Target = parentNode
            })

            visited.Add(dependencyName)

            BuildDependencyTree(dependencyService, dependencyNode, level + 1, nodesByName, visited)
        Next
    End Sub

    Private Function FindServiceByName(serviceName As String) As WindowsService
        If String.IsNullOrWhiteSpace(serviceName) Then Return Nothing

        If _mainService IsNot Nothing AndAlso String.Equals(_mainService.Name, serviceName, StringComparison.OrdinalIgnoreCase) Then Return _mainService

        Return _services.FirstOrDefault(Function(service)
                                            Return service IsNot Nothing AndAlso String.Equals(service.Name, serviceName, StringComparison.OrdinalIgnoreCase)
                                        End Function)
    End Function

    Private Shared Function ServiceDependsOn(service As WindowsService, dependencyName As String) As Boolean
        If service Is Nothing OrElse service.Dependencies Is Nothing Then Return False

        For Each name As String In service.Dependencies
            If String.Equals(name, dependencyName, StringComparison.OrdinalIgnoreCase) Then Return True
        Next

        Return False
    End Function

#End Region

#Region "Layout"

    ''' <summary>
    ''' Arranges the graph into left, center, and right columns.
    ''' Dependencies are placed recursively in columns to the left.
    ''' Direct dependents are placed in the column to the right.
    ''' </summary>
    Private Sub LayoutGraph()
        If _mainService Is Nothing OrElse _nodes.Count = 0 Then Exit Sub

        Dim mainNode As DiagramNode = _nodes.First(Function(node) node.IsMain)

        Dim leftNodes As New List(Of DiagramNode)(),
            rightNodes As New List(Of DiagramNode)()

        For Each node As DiagramNode In _nodes
            If node.IsMain Then Continue For

            If node.Level < 0 Then
                leftNodes.Add(node)
            Else
                rightNodes.Add(node)
            End If
        Next

        Dim minimumX As Single = 0
        Dim maximumX As Single = 0

        ' Main node is positioned at the origin first.
        mainNode.X = 0
        mainNode.Y = 0

        ' Organize left-side nodes by their dependency depth.
        Dim leftGroups As New Dictionary(Of Integer, List(Of DiagramNode))()

        For Each node As DiagramNode In leftNodes
            Dim depth As Integer = Math.Abs(node.Level)

            If Not leftGroups.ContainsKey(depth) Then leftGroups(depth) = New List(Of DiagramNode)()

            leftGroups(depth).Add(node)
        Next

        ' Position each dependency depth farther to the left.
        For Each pair As KeyValuePair(Of Integer, List(Of DiagramNode)) In leftGroups.OrderBy(Function(item) item.Key)
            Dim depth As Integer = pair.Key
            Dim group As List(Of DiagramNode) = pair.Value

            Dim x As Single = -(NodeWidth + HorizontalSpacing) * depth

            PositionVerticalGroup(group, x)
            minimumX = Math.Min(minimumX, x)
        Next

        ' Position direct dependents in one column to the right.
        If rightNodes.Count > 0 Then
            Dim x As Single = NodeWidth + HorizontalSpacing
            PositionVerticalGroup(rightNodes, x)
            maximumX = x
        End If

        ' Center the main node vertically relative to the other nodes.
        Dim allNonMainNodes As List(Of DiagramNode) = _nodes.Where(Function(node) Not node.IsMain).ToList()

        If allNonMainNodes.Count > 0 Then
            Dim minimumY As Single = allNonMainNodes.Min(Function(node) node.Y),
                maximumY As Single = allNonMainNodes.Max(Function(node) node.Y + node.Height)

            Dim groupCenter As Single = (minimumY + maximumY) / 2.0F
            mainNode.Y = groupCenter - mainNode.Height / 2.0F
        End If

        ' Ensure that the graph has a reasonable origin.
        Dim graphBounds As RectangleF = GetLogicalGraphBounds()

        Dim offsetX As Single = OuterPadding - graphBounds.Left,
            offsetY As Single = OuterPadding - graphBounds.Top

        For Each node As DiagramNode In _nodes
            node.X += offsetX
            node.Y += offsetY
        Next
    End Sub

    Private Sub PositionVerticalGroup(nodes As List(Of DiagramNode), baseX As Single)
        If nodes Is Nothing OrElse nodes.Count = 0 Then Exit Sub

        Const MaxPerColumn As Integer = 5

        ' Determine how many columns are needed
        Dim columnCount As Integer = CInt(Math.Ceiling(nodes.Count / MaxPerColumn))

        ' Determine direction: left side (negative X) or right side (positive X)
        Dim direction As Integer = If(baseX < 0, -1, 1)

        ' Horizontal offset between columns
        Dim columnSpacing As Single = NodeWidth + HorizontalSpacing

        ' Sort nodes alphabetically for consistency
        Dim sortedNodes = nodes.OrderBy(Function(n) n.Service.DisplayName).ToList()

        For col As Integer = 0 To columnCount - 1
            Dim columnNodes = sortedNodes.Skip(col * MaxPerColumn).Take(MaxPerColumn).ToList()

            Dim columnX As Single = baseX + (col * columnSpacing * direction)

            ' Vertical positioning inside each column
            Dim totalHeight As Single = columnNodes.Count * NodeHeight + (columnNodes.Count - 1) * VerticalSpacing
            Dim currentY As Single = -totalHeight / 2.0F

            For Each node As DiagramNode In columnNodes
                node.X = columnX
                node.Y = currentY
                currentY += NodeHeight + VerticalSpacing
            Next
        Next
    End Sub


    Private Function GetLogicalGraphBounds() As RectangleF
        If _nodes.Count = 0 Then Return RectangleF.Empty

        Dim left As Single = _nodes.Min(Function(node) node.X)
        Dim top As Single = _nodes.Min(Function(node) node.Y)
        Dim right As Single = _nodes.Max(Function(node) node.X + node.Width)
        Dim bottom As Single = _nodes.Max(Function(node) node.Y + node.Height)

        Return RectangleF.FromLTRB(left, top, right, bottom)
    End Function

#End Region

#Region "Painting"

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.Clear(_backgroundColor)
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

        If _mainService Is Nothing OrElse _nodes.Count = 0 Then
            DrawEmptyState(e.Graphics)
            Exit Sub
        End If

        e.Graphics.TranslateTransform(_pan.X, _pan.Y)
        e.Graphics.ScaleTransform(_zoom, _zoom)

        DrawConnections(e.Graphics)

        For Each node As DiagramNode In _nodes
            DrawNode(e.Graphics, node)
        Next

        e.Graphics.ResetTransform()

        DrawZoomIndicator(e.Graphics)
    End Sub

    Private Sub DrawEmptyState(g As Graphics)
        Using brush As New SolidBrush(Color.FromArgb(160, 160, 160))
            Using format As New StringFormat With {
                .Alignment = StringAlignment.Center,
                .LineAlignment = StringAlignment.Center
            }
                g.DrawString("No service selected", Font, brush, ClientRectangle, format)
            End Using
        End Using
    End Sub

    Private Sub DrawConnections(g As Graphics)
        Using pen As New Pen(_arrowColor, 2.0F / _zoom)
            pen.EndCap = LineCap.ArrowAnchor

            For Each connection As DiagramConnection In _connections
                DrawConnection(g, pen, connection)
            Next
        End Using
    End Sub

    Private Sub DrawConnection(g As Graphics, pen As Pen, connection As DiagramConnection)
        Dim source As DiagramNode = connection.Source
        Dim target As DiagramNode = connection.Target

        Dim sourceCenter As PointF = source.Center
        Dim targetCenter As PointF = target.Center

        Dim startPoint As PointF
        Dim endPoint As PointF

        If target.X > source.X Then
            startPoint = New PointF(source.X + source.Width, sourceCenter.Y)
            endPoint = New PointF(target.X, targetCenter.Y)
        Else
            startPoint = New PointF(source.X, sourceCenter.Y)
            endPoint = New PointF(target.X + target.Width, targetCenter.Y)
        End If

        ' Use a horizontal cubic Bézier curve. This keeps arrows readable
        ' even when multiple nodes share the same column.
        Dim horizontalDistance As Single = Math.Abs(endPoint.X - startPoint.X)
        Dim curveOffset As Single = Math.Max(30.0F, horizontalDistance * 0.35F)

        Dim controlPoint1 As PointF
        Dim controlPoint2 As PointF

        If endPoint.X > startPoint.X Then
            controlPoint1 = New PointF(startPoint.X + curveOffset, startPoint.Y)
            controlPoint2 = New PointF(endPoint.X - curveOffset, endPoint.Y)
        Else
            controlPoint1 = New PointF(startPoint.X - curveOffset, startPoint.Y)
            controlPoint2 = New PointF(endPoint.X + curveOffset, endPoint.Y)
        End If

        g.DrawBezier(pen, startPoint, controlPoint1, controlPoint2, endPoint)
    End Sub

    Private Function ClampRadius(radius As Integer, bounds As RectangleF) As Single
        Dim maximum As Single = Math.Min(bounds.Width, bounds.Height) / 2.0F
        Return Math.Max(0.0F, Math.Min(radius, CInt(maximum)))
    End Function

    Private Sub DrawNode(g As Graphics, node As DiagramNode)
        Dim bounds As RectangleF = node.Bounds

        Dim fillColor As Color = If(node.IsMain, _mainNodeBackColor, _nodeBackColor)

        Dim borderColor As Color = If(node.IsMain, _mainNodeBorderColor, _nodeBorderColor)

        Using path As GraphicsPath = CreateRectangle(bounds, ClampRadius(NodeCornerRadius, bounds))
            Using fillBrush As New SolidBrush(fillColor)
                g.FillPath(fillBrush, path)
            End Using

            Using borderPen As New Pen(borderColor, 1.5F / _zoom)
                g.DrawPath(borderPen, path)
            End Using
        End Using

        Dim padding As Single = 10.0F

        Dim titleRectangle As New RectangleF(bounds.X + padding, bounds.Y + 8.0F, bounds.Width - padding * 2.0F, 24.0F)

        Dim bodyRectangle As New RectangleF(bounds.X + padding, bounds.Y + 36.0F, bounds.Width - padding * 2.0F, bounds.Height - 44.0F)

        Using titleFormat As New StringFormat With {
            .Alignment = StringAlignment.Center,
            .LineAlignment = StringAlignment.Center,
            .Trimming = StringTrimming.EllipsisCharacter,
            .FormatFlags = StringFormatFlags.NoWrap
        }
            Using titleBrush As New SolidBrush(_nodeTextColor)
                Using titleFont As New Font(Font.FontFamily, Font.Size + 1.0F, FontStyle.Bold)
                    ' DisplayName (Name)
                    Dim title As String = node.Service.Name
                    g.DrawString(title, titleFont, titleBrush, titleRectangle, titleFormat)
                End Using
            End Using
        End Using

        Using bodyFormat As New StringFormat With {
            .Alignment = StringAlignment.Near,
            .LineAlignment = StringAlignment.Near,
            .Trimming = StringTrimming.EllipsisCharacter
        }
            Using bodyBrush As New SolidBrush(_descriptionTextColor)
                Using bodyFont As New Font(Font.FontFamily, Font.Size, FontStyle.Regular)
                    Dim description As String = If(String.IsNullOrWhiteSpace(node.Service.Description), "No description", node.Service.Description)
                    Dim startMode As String = node.Service.StartTypeToString()
                    Dim serviceType As String = node.Service.TypeToString()

                    Dim bodyText As String = String.Format("{1}{0}Start mode: {2}{0}Type: {3}", Environment.NewLine, description, node.Service.StartTypeToString(), node.Service.TypeToString())

                    g.DrawString(bodyText, bodyFont, bodyBrush, bodyRectangle, bodyFormat)
                End Using
            End Using
        End Using
    End Sub

    Private Shared Function CreateRectangle(rectangle As RectangleF, radius As Single) As GraphicsPath
        Dim path As New GraphicsPath()

        If radius = 0 Then
            path.AddRectangle(rectangle)
        Else
            Dim diameter As Single = radius * 2.0F
            Dim arcRectangle As New RectangleF(rectangle.X, rectangle.Y, diameter, diameter)

            path.AddArc(arcRectangle, 180.0F, 90.0F)

            arcRectangle.X = rectangle.Right - diameter
            path.AddArc(arcRectangle, 270.0F, 90.0F)

            arcRectangle.Y = rectangle.Bottom - diameter
            path.AddArc(arcRectangle, 0.0F, 90.0F)

            arcRectangle.X = rectangle.X
            path.AddArc(arcRectangle, 90.0F, 90.0F)
        End If

        path.CloseFigure()

        Return path
    End Function

    Private Sub DrawZoomIndicator(g As Graphics)
        Dim text As String = String.Format("Zoom: {0}%", CInt(_zoom * 100.0F))

        Using brush As New SolidBrush(Color.FromArgb(190, 190, 190))
            g.DrawString(text, Font, brush, New PointF(8.0F, 8.0F))
        End Using
    End Sub

#End Region

#Region "Panning and Zooming"

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)

        If e.Button = MouseButtons.Left Then
            _isPanning = True
            _lastMousePosition = e.Location
            Cursor = Cursors.SizeAll
            Focus()
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        If Not _isPanning Then Exit Sub

        Dim deltaX As Integer = e.X - _lastMousePosition.X
        Dim deltaY As Integer = e.Y - _lastMousePosition.Y

        _pan = New PointF(_pan.X + deltaX, _pan.Y + deltaY)
        _lastMousePosition = e.Location

        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If e.Button = MouseButtons.Left Then
            _isPanning = False
            Cursor = Cursors.Default
        End If
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        MyBase.OnMouseWheel(e)

        If _mainService Is Nothing Then Exit Sub

        Dim oldZoom As Single = _zoom

        If e.Delta > 0 Then
            _zoom *= 1.1F
        ElseIf e.Delta < 0 Then
            _zoom /= 1.1F
        End If

        _zoom = Math.Max(MinimumZoom, Math.Min(MaximumZoom, _zoom))

        If Math.Abs(oldZoom - _zoom) < 0.001F Then Exit Sub

        ' Keep the logical point beneath the mouse cursor stationary
        ' while zooming.
        Dim mouseX As Single = e.X
        Dim mouseY As Single = e.Y

        Dim logicalX As Single = (mouseX - _pan.X) / oldZoom
        Dim logicalY As Single = (mouseY - _pan.Y) / oldZoom

        _pan = New PointF(mouseX - logicalX * _zoom, mouseY - logicalY * _zoom)
        Invalidate()
    End Sub

    Protected Overrides Sub OnDoubleClick(e As EventArgs)
        MyBase.OnDoubleClick(e)
        ResetView()
    End Sub

    ''' <summary>
    ''' Resets the zoom and centers the diagram in the control.
    ''' </summary>
    Public Sub ResetView()
        _zoom = 1.0F

        If _nodes.Count = 0 Then
            _pan = PointF.Empty
            Invalidate()
            Exit Sub
        End If

        Dim bounds As RectangleF = GetLogicalGraphBounds()

        Dim availableWidth As Single = Math.Max(1.0F, ClientSize.Width)
        Dim availableHeight As Single = Math.Max(1.0F, ClientSize.Height)

        Dim graphWidth As Single = Math.Max(1.0F, bounds.Width)
        Dim graphHeight As Single = Math.Max(1.0F, bounds.Height)

        ' Fit the graph if it is larger than the viewport.
        Dim fitZoomX As Single = availableWidth / (graphWidth + 40.0F)
        Dim fitZoomY As Single = availableHeight / (graphHeight + 40.0F)

        Dim fitZoom As Single = Math.Min(fitZoomX, fitZoomY)

        If fitZoom < 1.0F Then _zoom = Math.Max(MinimumZoom, Math.Min(1.0F, fitZoom))

        _pan = New PointF((availableWidth - graphWidth * _zoom) / 2.0F - bounds.Left * _zoom, (availableHeight - graphHeight * _zoom) / 2.0F - bounds.Top * _zoom)
        Invalidate()
    End Sub

#End Region

#Region "Image Export"

    ''' <summary>
    ''' Saves the complete logical diagram as a PNG image.
    ''' The exported image includes all nodes and arrows, even those
    ''' outside the current viewport.
    ''' </summary>
    ''' <param name="fileName">The destination image file.</param>
    Public Sub SaveAsImage(fileName As String)
        If String.IsNullOrWhiteSpace(fileName) Then Throw New ArgumentException("A valid file name must be specified.", fileName)

        If Not _nodes.Any() Then Throw New InvalidOperationException("There is no diagram to export.")

        Dim bounds As RectangleF = GetLogicalGraphBounds()

        Dim exportPadding As Single = OuterPadding
        Dim imageWidth As Integer = CInt(Math.Ceiling(bounds.Width + exportPadding * 2.0F)),
            imageHeight As Integer = CInt(Math.Ceiling(bounds.Height + exportPadding * 2.0F))

        imageWidth = Math.Max(1, imageWidth)
        imageHeight = Math.Max(1, imageHeight)

        Using bitmap As New Bitmap(imageWidth, imageHeight, PixelFormat.Format32bppPArgb)
            Using g As Graphics = Graphics.FromImage(bitmap)
                g.Clear(_backgroundColor)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

                g.TranslateTransform(exportPadding - bounds.Left, exportPadding - bounds.Top)

                ' Add a header for the diagram
                Dim headerRectangle As New RectangleF(WindowHelper.ScaleLogical(24), WindowHelper.ScaleLogical(24),
                                                      WindowHelper.ScaleLogical(bounds.Width), WindowHelper.ScaleLogical(48))

                Using headerFormat As New StringFormat With {
                    .Alignment = StringAlignment.Near,
                    .LineAlignment = StringAlignment.Near,
                    .Trimming = StringTrimming.EllipsisCharacter,
                    .FormatFlags = StringFormatFlags.NoWrap
                }
                    Using headerBrush As New SolidBrush(_nodeTextColor)
                        Using headerFont As New Font(Font.FontFamily, 24, FontStyle.Regular)
                            g.DrawString(String.Format("Dependency diagram for service {0}", _mainService.Name), headerFont, headerBrush, headerRectangle, headerFormat)
                        End Using
                    End Using
                End Using

                ' Export at logical zoom 1.0, regardless of the
                ' current viewport zoom.
                DrawConnectionsForExport(g)

                For Each node As DiagramNode In _nodes
                    DrawNodeForExport(g, node)
                Next
            End Using

            bitmap.Save(fileName, ImageFormat.Png)
        End Using
    End Sub

    Private Sub DrawConnectionsForExport(g As Graphics)
        Using pen As New Pen(_arrowColor, 2.0F)
            pen.EndCap = LineCap.ArrowAnchor

            For Each connection As DiagramConnection In _connections
                DrawConnection(g, pen, connection)
            Next
        End Using
    End Sub

    Private Sub DrawNodeForExport(g As Graphics, node As DiagramNode)
        ' DrawNode uses the current zoom for pen and text scaling.
        ' The export must always use logical zoom 1.0.
        DrawNodeAtScale(g, node)
    End Sub

    Private Sub DrawNodeAtScale(g As Graphics, node As DiagramNode)
        Dim bounds As RectangleF = node.Bounds

        Dim fillColor As Color = If(node.IsMain, _mainNodeBackColor, _nodeBackColor)
        Dim borderColor As Color = If(node.IsMain, _mainNodeBorderColor, _nodeBorderColor)

        Using path As GraphicsPath = CreateRectangle(bounds, ClampRadius(NodeCornerRadius, bounds))
            Using fillBrush As New SolidBrush(fillColor)
                g.FillPath(fillBrush, path)
            End Using

            Using borderPen As New Pen(borderColor, 1.5F)
                g.DrawPath(borderPen, path)
            End Using
        End Using

        Dim padding As Single = 10.0F

        Dim titleRectangle As New RectangleF(bounds.X + padding, bounds.Y + 8.0F, bounds.Width - padding * 2.0F, 24.0F),
            bodyRectangle As New RectangleF(bounds.X + padding, bounds.Y + 36.0F, bounds.Width - padding * 2.0F, bounds.Height - 44.0F)

        Using titleFormat As New StringFormat With {
            .Alignment = StringAlignment.Center,
            .LineAlignment = StringAlignment.Center,
            .Trimming = StringTrimming.EllipsisCharacter,
            .FormatFlags = StringFormatFlags.NoWrap
        }
            Using titleBrush As New SolidBrush(_nodeTextColor)
                Using titleFont As New Font(Font.FontFamily, Font.Size + 1.0F, FontStyle.Bold)
                    g.DrawString(node.Service.Name, titleFont, titleBrush, titleRectangle, titleFormat)
                End Using
            End Using
        End Using

        Using bodyFormat As New StringFormat With {
            .Alignment = StringAlignment.Near,
            .LineAlignment = StringAlignment.Near,
            .Trimming = StringTrimming.EllipsisCharacter
        }
            Using bodyBrush As New SolidBrush(_descriptionTextColor)
                Using bodyFont As New Font(Font.FontFamily, Font.Size, FontStyle.Regular)
                    Dim description As String = If(String.IsNullOrWhiteSpace(node.Service.Description), "No description", node.Service.Description)
                    Dim bodyText As String = String.Format("{1}{0}Start mode: {2}{0}Type: {3}", Environment.NewLine, description, node.Service.StartTypeToString(), node.Service.TypeToString())
                    g.DrawString(bodyText, bodyFont, bodyBrush, bodyRectangle, bodyFormat)
                End Using
            End Using
        End Using
    End Sub

#End Region

End Class