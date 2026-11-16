Imports Excel = Microsoft.Office.Interop.Excel

Namespace Utilities.OfficeUtilities

    ''' <summary>
    ''' Specifies the appearance and configuration options for an Excel chart.
    ''' Use this class to control chart type, axis labels, legend visibility and position, and data series orientation.
    ''' </summary>
    Public Class ChartOptions

        ''' <summary>
        ''' Gets or sets the type of chart to create (e.g., column, pie, line). Defaults to column chart.
        ''' </summary>
        Public Property ChartType As Excel.XlChartType = Excel.XlChartType.xlColumnClustered
        ''' <summary>
        ''' Gets or sets the main title displayed above the chart. Leave this value set to Nothing if you
        ''' don't want to show a title in your chart.
        ''' </summary>
        Public Property Title As String = Nothing
        ''' <summary>
        ''' Gets or sets the label for the category axis (X-axis). This is ignored for pie and doughnut charts.
        ''' </summary>
        Public Property CategoryAxisTitle As String = Nothing
        ''' <summary>
        ''' Gets or sets the label for the value axis (Y-axis). This is ignored for pie and doughnut charts.
        ''' </summary>
        Public Property ValueAxisTitle As String = Nothing
        ''' <summary>
        ''' Gets or sets whether to display a legend in the chart.
        ''' </summary>
        Public Property ShowLegend As Boolean = True
        ''' <summary>
        ''' Gets or sets the position where the legend appears (e.g., bottom, top, left, right).
        ''' </summary>
        Public Property LegendPosition As Excel.XlLegendPosition = Excel.XlLegendPosition.xlLegendPositionBottom
        ''' <summary>
        ''' Gets or sets whether data series are arranged in columns or rows. This affects how the data is interpreted.
        ''' </summary>
        Public Property PlotBy As Excel.XlRowCol = Excel.XlRowCol.xlColumns
        ''' <summary>
        ''' Gets or sets the name assigned to the chart object in Excel. Set to Nothing to allow Excel to assign a default name.
        ''' </summary>
        Public Property ChartName As String = Nothing

    End Class

End Namespace
