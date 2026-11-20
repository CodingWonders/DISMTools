Imports System.Drawing
Imports Excel = Microsoft.Office.Interop.Excel

Namespace Utilities.OfficeUtilities

    ''' <summary>
    ''' Encapsulates formatting options that can be applied to Excel cells and ranges.
    ''' Properties set to their default values (Nothing, 0, or empty strings) are ignored during application,
    ''' allowing for selective formatting without affecting unspecified attributes.
    ''' </summary>
    Public Class CellStyle

        ''' <summary>Font name. Defaults to Arial.</summary>
        Public Property FontName As String = "Arial"
        ''' <summary>Font size in points. A value of 0 leaves the size unchanged.</summary>
        Public Property FontSize As Double = 0
        ''' <summary>Whether text should be bold.</summary>
        Public Property Bold As Boolean? = Nothing
        ''' <summary>Whether text should be italicized.</summary>
        Public Property Italic As Boolean? = Nothing
        ''' <summary>Whether text should be underlined.</summary>
        Public Property Underline As Boolean? = Nothing
        ''' <summary>Font color.</summary>
        Public Property FontColor As Color? = Nothing

        ''' <summary>Cell background color.</summary>
        Public Property BackColor As Color? = Nothing

        ''' <summary>Horizontal alignment within the cell.</summary>
        Public Property HorizontalAlignment As Excel.XlHAlign? = Nothing
        ''' <summary>Vertical alignment within the cell.</summary>
        Public Property VerticalAlignment As Excel.XlVAlign? = Nothing
        ''' <summary>Whether text should wrap within the cell.</summary>
        Public Property WrapText As Boolean? = Nothing

        ''' <summary>Number format string (e.g., "#,##0.00" for currency, "0%" for percentage, "yyyy-mm-dd" for dates, "@" for text).</summary>
        Public Property NumberFormat As String = Nothing

        ''' <summary>Border line style applied to all edges of the cell or range.</summary>
        Public Property BorderStyle As Excel.XlLineStyle? = Nothing
        ''' <summary>Border thickness.</summary>
        Public Property BorderWeight As Excel.XlBorderWeight = Excel.XlBorderWeight.xlThin
        ''' <summary>Border color.</summary>
        Public Property BorderColor As Color? = Nothing

        ''' <summary>Column width in characters. A value of 0 leaves the width unchanged.</summary>
        Public Property ColumnWidth As Double = 0
        ''' <summary>Row height in points. A value of 0 leaves the height unchanged.</summary>
        Public Property RowHeight As Double = 0
        ''' <summary>Whether to automatically adjust column widths to fit content. This has no effect on merged cells.</summary>
        Public Property AutoFitColumns As Boolean = False

        ''' <summary>
        ''' Creates a header style for a cell to use. This style uses bold text with black background and white foreground, centered both horizontally and vertically, with continuous borders.
        ''' </summary>
        Public Shared Function Header() As CellStyle
            Dim result As New CellStyle()
            result.Bold = True
            result.BackColor = Color.Black
            result.FontColor = Color.White
            result.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            result.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            result.BorderStyle = Excel.XlLineStyle.xlContinuous
            Return result
        End Function

        ''' <summary>
        ''' Creates a title style for a cell to use. This style uses a bold, large font (16 pt) with black background
        ''' and white foreground, centered both horizontally and vertically.
        ''' </summary>
        Public Shared Function Title() As CellStyle
            Dim result As New CellStyle()
            result.Bold = True
            result.FontSize = 16
            result.BackColor = Color.Black
            result.FontColor = Color.White
            result.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            result.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            Return result
        End Function

        ''' <summary>
        ''' Creates a bordered style for a cell to use, with continuous line borders on all edges.
        ''' </summary>
        Public Shared Function Bordered() As CellStyle
            Dim result As New CellStyle()
            result.BorderStyle = Excel.XlLineStyle.xlContinuous
            Return result
        End Function
    End Class

End Namespace