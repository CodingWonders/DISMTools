Imports System.Runtime.InteropServices
Imports Excel = Microsoft.Office.Interop.Excel

Namespace Utilities.OfficeUtilities

    ''' <summary>
    ''' Provides utility methods for working with Excel via Component Object Model (COM) interop, 
    ''' including cell styling, value assignment, formulas, merged cells, and chart creation. 
    ''' </summary>
    ''' <remarks>
    ''' COM interoperability requires that all COM objects returned by this module be released by the caller to prevent
    ''' memory leaks. You can use the <see cref="ReleaseCom"/> procedure to safely release the objects.
    ''' </remarks>
    Public Module ExcelHelper

        ''' <summary>
        ''' Releases a COM object and prevents memory leaks.
        ''' </summary>
        ''' <param name="comObject">The COM object to release.</param>
        ''' <remarks>
        ''' </remarks>
        Public Sub ReleaseCom(comObject As Object)
            If comObject IsNot Nothing AndAlso Marshal.IsComObject(comObject) Then
                Marshal.ReleaseComObject(comObject)
            End If
        End Sub

        ''' <summary>
        ''' Checks whether Excel is installed and available on the system.
        ''' </summary>
        ''' <returns>True if Excel can be instantiated; otherwise, False.</returns>
        ''' <remarks>
        ''' This only checks whether the <c>Excel.Application</c> ProgID is registered. It does not start Excel, so a True result
        ''' does not guarantee that automation will succeed (for example, if the installation is broken).
        ''' </remarks>
        Public Function IsExcelAvailable() As Boolean
            Return Type.GetTypeFromProgID("Excel.Application") IsNot Nothing
        End Function

        ''' <summary>
        ''' Converts a 1-based column number to Excel letter notation (1 = A, 26 = Z, 27 = AA).
        ''' </summary>
        ''' <param name="columnNumber">The column number, starting at 1.</param>
        ''' <returns>The column letters (e.g., "A", "AA", "XFD").</returns>
        ''' <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="columnNumber"/> is less than 1.</exception>
        ''' <remarks>
        ''' This method does not check the upper limit of the worksheet (16384, or "XFD", in modern Excel versions); it only
        ''' performs the base-26 conversion. Excel will reject an out-of-range address when it is used.
        ''' </remarks>
        Public Function ColumnNumberToLetters(columnNumber As Integer) As String
            If columnNumber < 1 Then Throw New ArgumentOutOfRangeException("columnNumber", "Column numbers start at 1.")

            Dim result As String = String.Empty
            Dim remaining As Integer = columnNumber

            While remaining > 0
                Dim modulo As Integer = (remaining - 1) Mod 26
                result = ChrW(65 + modulo).ToString() & result
                remaining = (remaining - modulo - 1) \ 26
            End While

            Return result
        End Function

        ''' <summary>
        ''' Converts 1-based row and column numbers to an A1-style cell address.
        ''' </summary>
        ''' <param name="row">The row number, starting at 1.</param>
        ''' <param name="column">The column number, starting at 1.</param>
        ''' <returns>The A1-style address (e.g., "A1", "Z10", "AA100").</returns>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown if <paramref name="row"/> is less than 1, or if <paramref name="column"/> is less than 1 (the latter is
        ''' thrown by <see cref="ColumnNumberToLetters"/>).
        ''' </exception>
        Public Function ToAddress(row As Integer, column As Integer) As String
            If row < 1 Then Throw New ArgumentOutOfRangeException("row", "Row numbers start at 1.")
            Return ColumnNumberToLetters(column) & row.ToString()
        End Function

        ''' <summary>
        ''' Applies a cell style to a range, overriding the specified properties while leaving others unchanged.
        ''' If <paramref name="style"/> is Nothing, only the font is set to Arial.
        ''' </summary>
        ''' <param name="target">The range to format.</param>
        ''' <param name="style">The cell style to apply, or Nothing to apply default formatting.</param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="target"/> is Nothing.</exception>
        ''' <remarks>
        ''' Only the properties of <paramref name="style"/> that have a value (nullable properties with a value, non-empty strings,
        ''' or numbers greater than zero) are applied. Colors are converted from <see cref="System.Drawing.Color"/> to the
        ''' BGR "OLE" format that Excel expects. All intermediate COM objects (font, interior, borders, columns) are released
        ''' internally. The caller keeps ownership of <paramref name="target"/>.
        ''' </remarks>
        Public Sub ApplyStyle(target As Excel.Range, style As CellStyle)
            If target Is Nothing Then Throw New ArgumentNullException("target")

            Dim excelFont As Excel.Font = Nothing
            Dim interior As Excel.Interior = Nothing
            Dim borders As Excel.Borders = Nothing
            Dim columnsRange As Excel.Range = Nothing

            Try
                ' Font
                excelFont = target.Font

                If style Is Nothing Then excelFont.Name = "Arial" : Exit Sub

                excelFont.Name = If(String.IsNullOrEmpty(style.FontName), "Arial", style.FontName)

                If style.FontSize > 0 Then excelFont.Size = style.FontSize
                If style.Bold.HasValue Then excelFont.Bold = style.Bold.Value
                If style.Italic.HasValue Then excelFont.Italic = style.Italic.Value
                If style.Underline.HasValue Then excelFont.Underline = If(style.Underline.Value, Excel.XlUnderlineStyle.xlUnderlineStyleSingle, Excel.XlUnderlineStyle.xlUnderlineStyleNone)

                If style.FontColor.HasValue Then
                    ' Excel colors are BGR "OLE" values, not ARGB, so they must be converted.
                    excelFont.Color = ColorTranslator.ToOle(style.FontColor.Value)
                End If

                ' Fill
                If style.BackColor.HasValue Then
                    interior = target.Interior
                    interior.Color = ColorTranslator.ToOle(style.BackColor.Value)
                End If

                ' Alignment
                If style.HorizontalAlignment.HasValue Then target.HorizontalAlignment = style.HorizontalAlignment.Value
                If style.VerticalAlignment.HasValue Then target.VerticalAlignment = style.VerticalAlignment.Value
                If style.WrapText.HasValue Then target.WrapText = style.WrapText.Value

                ' Number formatting
                If Not String.IsNullOrEmpty(style.NumberFormat) Then target.NumberFormat = style.NumberFormat

                ' Borders
                If style.BorderStyle.HasValue Then
                    borders = target.Borders
                    borders.LineStyle = style.BorderStyle.Value
                    borders.Weight = style.BorderWeight

                    If style.BorderColor.HasValue Then
                        borders.Color = ColorTranslator.ToOle(style.BorderColor.Value)
                    End If
                End If

                ' Sizing
                If style.ColumnWidth > 0 Then target.ColumnWidth = style.ColumnWidth
                If style.RowHeight > 0 Then target.RowHeight = style.RowHeight

                If style.AutoFitColumns Then
                    columnsRange = target.Columns
                    columnsRange.AutoFit()
                End If
            Finally
                ReleaseCom(columnsRange)
                ReleaseCom(borders)
                ReleaseCom(interior)
                ReleaseCom(excelFont)
            End Try
        End Sub

        ''' <summary>
        ''' Applies a cell style to a cell in a given address, such as "A1", or an address range; such as "A1:D10".
        ''' </summary>
        ''' <param name="sheet">The sheet the cell or cells are in</param>
        ''' <param name="address">The address of the cell or cell range</param>
        ''' <param name="style">The cell style to apply to the cell or cell range</param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> is Nothing.</exception>
        ''' <remarks>
        ''' This is a convenience wrapper around <see cref="ApplyStyle"/> that resolves the range from an address and releases
        ''' it afterwards. An invalid <paramref name="address"/> causes Excel to raise a <see cref="COMException"/>.
        ''' </remarks>
        Public Sub StyleRange(sheet As Excel.Worksheet, address As String, style As CellStyle)
            If sheet Is Nothing Then Throw New ArgumentNullException("sheet")

            Dim target As Excel.Range = Nothing

            Try
                target = sheet.Range(address)
                ApplyStyle(target, style)
            Finally
                ReleaseCom(target)
            End Try
        End Sub

        ''' <summary>
        ''' Sets one cell (or a range, in which case every cell gets the same value) by address.
        ''' </summary>
        ''' <param name="sheet">The worksheet that contains the cell or range.</param>
        ''' <param name="address">The A1-style address of a cell (e.g., "B2") or a range (e.g., "B2:D4").</param>
        ''' <param name="value">
        ''' The value to write. <see cref="DateTime"/> values are converted to OLE Automation dates and given the
        ''' "yyyy-mm-dd" number format unless the style specifies another one.
        ''' </param>
        ''' <param name="style">An optional cell style to apply. If Nothing, only the default font (Arial) is applied.</param>
        ''' <param name="wrapText">
        ''' An optional flag that overrides the wrap setting of <paramref name="style"/>. Nothing keeps whatever the style or cell says.
        ''' </param>
        ''' <param name="horizontalAlignment">An optional horizontal alignment that overrides the one in <paramref name="style"/>.</param>
        ''' <param name="verticalAlignment">An optional vertical alignment that overrides the one in <paramref name="style"/>.</param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> is Nothing.</exception>
        ''' <remarks>
        ''' If the resulting text is wrapped, the row height is auto-fitted, unless the style defines an explicit row height or
        ''' the cell is merged. See <see cref="ApplyLayoutOverrides"/> for details.
        ''' </remarks>
        Public Sub SetCellValue(sheet As Excel.Worksheet, address As String, value As Object, Optional style As CellStyle = Nothing, Optional wrapText As Boolean? = Nothing,
                                Optional horizontalAlignment As Excel.XlHAlign? = Nothing, Optional verticalAlignment As Excel.XlVAlign? = Nothing)
            If sheet Is Nothing Then Throw New ArgumentNullException("sheet")

            Dim target As Excel.Range = Nothing

            Try
                target = sheet.Range(address)
                WriteValue(target, value, style)
                ApplyLayoutOverrides(target, style, wrapText, horizontalAlignment, verticalAlignment)
            Finally
                ReleaseCom(target)
            End Try
        End Sub

        ''' <summary>
        ''' Sets one cell by 1-based row and column numbers.
        ''' </summary>
        ''' <param name="sheet">The worksheet that contains the cell.</param>
        ''' <param name="row">The row number, starting at 1.</param>
        ''' <param name="column">The column number, starting at 1.</param>
        ''' <param name="value">The value to write. See the address-based overload for how dates are handled.</param>
        ''' <param name="style">An optional cell style to apply.</param>
        ''' <param name="wrapText">An optional flag that overrides the wrap setting of <paramref name="style"/>.</param>
        ''' <param name="horizontalAlignment">An optional horizontal alignment that overrides the one in <paramref name="style"/>.</param>
        ''' <param name="verticalAlignment">An optional vertical alignment that overrides the one in <paramref name="style"/>.</param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> is Nothing.</exception>
        ''' <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="row"/> or <paramref name="column"/> is less than 1.</exception>
        ''' <remarks>
        ''' This overload converts the row and column to an A1-style address with <see cref="ToAddress"/> and forwards the call
        ''' to the address-based overload.
        ''' </remarks>
        Public Sub SetCellValue(sheet As Excel.Worksheet, row As Integer, column As Integer, value As Object,
                                Optional style As CellStyle = Nothing, Optional wrapText As Boolean? = Nothing, Optional horizontalAlignment As Excel.XlHAlign? = Nothing,
                                Optional verticalAlignment As Excel.XlVAlign? = Nothing)
            SetCellValue(sheet, ToAddress(row, column), value, style, wrapText, horizontalAlignment, verticalAlignment)
        End Sub

        ''' <summary>
        ''' Writes a whole 2D array in one COM call, starting at topLeftAddress.
        ''' This is dramatically faster than setting cells one at a time.
        ''' </summary>
        ''' <param name="sheet">The worksheet to write to.</param>
        ''' <param name="topLeftAddress">The address of the top-left cell where the array will be written (e.g., "A1").</param>
        ''' <param name="values">
        ''' The two-dimensional array of values. The first dimension is the row and the second dimension is the column.
        ''' </param>
        ''' <param name="style">An optional cell style applied to the whole written area.</param>
        ''' <param name="wrapText">An optional flag that overrides the wrap setting of <paramref name="style"/>.</param>
        ''' <param name="horizontalAlignment">An optional horizontal alignment that overrides the one in <paramref name="style"/>.</param>
        ''' <param name="verticalAlignment">An optional vertical alignment that overrides the one in <paramref name="style"/>.</param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> or <paramref name="values"/> is Nothing.</exception>
        ''' <remarks>
        ''' If the array has no rows or no columns, the method returns without doing anything. The target range is sized
        ''' from the dimensions of <paramref name="values"/>. Unlike <see cref="SetCellValue"/>, dates are not converted for you, and
        ''' the number format is applied after the values are written, so pass numbers, text or OLE dates already prepared.
        ''' </remarks>
        Public Sub SetRangeValues(sheet As Excel.Worksheet, topLeftAddress As String, values As Object(,), Optional style As CellStyle = Nothing, Optional wrapText As Boolean? = Nothing,
                                  Optional horizontalAlignment As Excel.XlHAlign? = Nothing, Optional verticalAlignment As Excel.XlVAlign? = Nothing)
            If sheet Is Nothing Then Throw New ArgumentNullException("sheet")
            If values Is Nothing Then Throw New ArgumentNullException("values")

            Dim rowCount As Integer = values.GetLength(0)
            Dim columnCount As Integer = values.GetLength(1)

            If rowCount = 0 OrElse columnCount = 0 Then Exit Sub

            Dim anchor As Excel.Range = Nothing
            Dim target As Excel.Range = Nothing

            Try
                anchor = sheet.Range(topLeftAddress)

                Dim startRow As Integer = anchor.Row
                Dim startColumn As Integer = anchor.Column
                Dim startAddress As String = ToAddress(startRow, startColumn)
                Dim endAddress As String = ToAddress(startRow + rowCount - 1, startColumn + columnCount - 1)

                target = sheet.Range(startAddress, endAddress)
                target.Value2 = values
                ApplyStyle(target, style)
                ApplyLayoutOverrides(target, style, wrapText, horizontalAlignment, verticalAlignment)
            Finally
                ReleaseCom(target)
                ReleaseCom(anchor)
            End Try
        End Sub

        ''' <summary>
        ''' Applies the explicit wrap/alignment flags passed to the writing functions.
        ''' A flag that has a value wins over the same setting in the CellStyle;
        ''' Nothing means "use whatever the style (or the cell) already says".
        ''' When text ends up wrapped, the row height is auto-fitted so the text is visible,
        ''' unless the style sets an explicit RowHeight or the cell is part of a merged area
        ''' (Excel cannot auto-fit merged cells, so set RowHeight in the style for those).
        ''' </summary>
        ''' <param name="target">The range on which the overrides are applied.</param>
        ''' <param name="style">
        ''' The style that was used to write the range. It is only read to determine whether text is wrapped and whether an
        ''' explicit row height was requested. It can be Nothing.
        ''' </param>
        ''' <param name="wrapText">The explicit wrap flag, or Nothing to defer to <paramref name="style"/>.</param>
        ''' <param name="horizontalAlignment">The explicit horizontal alignment, or Nothing to leave it unchanged.</param>
        ''' <param name="verticalAlignment">The explicit vertical alignment, or Nothing to leave it unchanged.</param>
        ''' <remarks>
        ''' The auto-fit step only happens when no cell in the range is merged. When the merge state is mixed, Excel returns
        ''' DBNull, and this is treated as "may contain merged cells", so no auto-fit is done.
        ''' </remarks>
        Private Sub ApplyLayoutOverrides(target As Excel.Range, style As CellStyle, wrapText As Boolean?, horizontalAlignment As Excel.XlHAlign?, verticalAlignment As Excel.XlVAlign?)
            If horizontalAlignment.HasValue Then target.HorizontalAlignment = horizontalAlignment.Value
            If verticalAlignment.HasValue Then target.VerticalAlignment = verticalAlignment.Value
            If wrapText.HasValue Then target.WrapText = wrapText.Value

            ' Work out whether the text is wrapped after both sources have been considered.
            Dim effectiveWrap As Boolean = False

            If wrapText.HasValue Then
                effectiveWrap = wrapText.Value
            ElseIf style IsNot Nothing AndAlso style.WrapText.HasValue Then
                effectiveWrap = style.WrapText.Value
            End If

            Dim hasExplicitRowHeight As Boolean = (style IsNot Nothing) AndAlso (style.RowHeight > 0)

            If Not effectiveWrap OrElse hasExplicitRowHeight Then Exit Sub

            ' MergeCells returns True, False, or DBNull when the range is partly merged.
            Dim mergedState As Object = target.MergeCells
            Dim hasNoMergedCells As Boolean = (TypeOf mergedState Is Boolean) AndAlso Not DirectCast(mergedState, Boolean)

            If hasNoMergedCells Then
                Dim rowsRange As Excel.Range = Nothing

                Try
                    rowsRange = target.Rows
                    rowsRange.AutoFit()
                Finally
                    ReleaseCom(rowsRange)
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Writes a value to a range and applies the given style, taking care of number formats and dates.
        ''' </summary>
        ''' <param name="target">The range to write to. If it spans several cells, every cell receives the same value.</param>
        ''' <param name="value">The value to write. <see cref="DateTime"/> values are converted to OLE Automation dates.</param>
        ''' <param name="style">The style to apply, or Nothing for the default font only.</param>
        ''' <remarks>
        ''' The number format is set BEFORE the value so that text formats ("@") are respected: a value such as "00123"
        ''' written into a General cell would already have been converted to the number 123. Dates are written as serial numbers
        ''' through <c>Value2</c>, and receive the "yyyy-mm-dd" format when the style does not specify one.
        ''' </remarks>
        Private Sub WriteValue(target As Excel.Range, value As Object, style As CellStyle)
            Dim hasNumberFormat As Boolean = (style IsNot Nothing) AndAlso Not String.IsNullOrEmpty(style.NumberFormat)

            ' The number format goes in BEFORE the value. This matters for text ("@"),
            ' because "00123" written into a General cell is already converted to 123.
            If hasNumberFormat Then target.NumberFormat = style.NumberFormat

            Dim converted As Object = value

            If TypeOf value Is DateTime Then
                ' Value2 works with raw serial numbers, so convert dates explicitly,
                ' and give them a readable format if the caller did not choose one.
                converted = DirectCast(value, DateTime).ToOADate()

                If Not hasNumberFormat Then target.NumberFormat = "yyyy-mm-dd"
            End If

            target.Value2 = converted
            ApplyStyle(target, style)
        End Sub

        ''' <summary>
        ''' Merges an address such as "A1:D1". With mergeEachRowSeparately = True, every row of
        ''' the range becomes its own merged cell instead of one big block.
        ''' Only the top-left value survives a merge.
        ''' </summary>
        ''' <param name="sheet">The worksheet that contains the range.</param>
        ''' <param name="address">The A1-style address of the range to merge (e.g., "A1:D1").</param>
        ''' <param name="style">An optional style applied to the merged area after merging, so fills and borders cover it fully.</param>
        ''' <param name="mergeEachRowSeparately">
        ''' If True, each row in the range is merged on its own; if False (the default), the whole range becomes a single merged cell.
        ''' </param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> is Nothing.</exception>
        ''' <remarks>
        ''' Excel alerts are suppressed during the merge, so no confirmation prompt appears for non-empty cells.
        ''' </remarks>
        Public Sub MergeCells(sheet As Excel.Worksheet, address As String, Optional style As CellStyle = Nothing, Optional mergeEachRowSeparately As Boolean = False)
            If sheet Is Nothing Then Throw New ArgumentNullException("sheet")

            Dim target As Excel.Range = Nothing

            Try
                target = sheet.Range(address)
                MergeRange(target, mergeEachRowSeparately)
                ApplyStyle(target, style)
            Finally
                ReleaseCom(target)
            End Try
        End Sub

        ''' <summary>
        ''' Unmerges every merged area inside an address.
        ''' </summary>
        ''' <param name="sheet">The worksheet that contains the range.</param>
        ''' <param name="address">The A1-style address of the range to unmerge (e.g., "A1:D1").</param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> is Nothing.</exception>
        ''' <remarks>
        ''' After unmerging, the value that survived the merge stays in the top-left cell, and the remaining cells are empty.
        ''' </remarks>
        Public Sub UnmergeCells(sheet As Excel.Worksheet, address As String)
            If sheet Is Nothing Then Throw New ArgumentNullException("sheet")

            Dim target As Excel.Range = Nothing

            Try
                target = sheet.Range(address)
                target.UnMerge()
            Finally
                ReleaseCom(target)
            End Try
        End Sub

        ''' <summary>
        ''' Writes a value into a cell that spans several rows and/or columns, like HTML rowspan/colspan.
        ''' Example: SetSpannedCell(sheet, 1, 1, 1, 4, "Title") merges A1:D1.
        ''' </summary>
        ''' <param name="sheet">The worksheet to write to.</param>
        ''' <param name="startRow">The row of the top-left cell, starting at 1.</param>
        ''' <param name="startColumn">The column of the top-left cell, starting at 1.</param>
        ''' <param name="rowSpan">The number of rows the cell spans. Must be at least 1.</param>
        ''' <param name="columnSpan">The number of columns the cell spans. Must be at least 1.</param>
        ''' <param name="value">The value to write into the top-left cell.</param>
        ''' <param name="style">An optional style applied to the whole merged area.</param>
        ''' <param name="wrapText">An optional flag that overrides the wrap setting of <paramref name="style"/>.</param>
        ''' <param name="horizontalAlignment">An optional horizontal alignment that overrides the one in <paramref name="style"/>.</param>
        ''' <param name="verticalAlignment">An optional vertical alignment that overrides the one in <paramref name="style"/>.</param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> is Nothing.</exception>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown if <paramref name="rowSpan"/> or <paramref name="columnSpan"/> is less than 1, or if the start position is invalid.
        ''' </exception>
        ''' <remarks>
        ''' When both spans are 1, no merge is performed and the method behaves like <see cref="SetCellValue"/>.
        ''' Excel cannot auto-fit the row height of merged cells, so set <c>RowHeight</c> in the style when text is wrapped.
        ''' </remarks>
        Public Sub SetSpannedCell(sheet As Excel.Worksheet, startRow As Integer, startColumn As Integer, rowSpan As Integer, columnSpan As Integer, value As Object, 
                                  Optional style As CellStyle = Nothing, Optional wrapText As Boolean? = Nothing, Optional horizontalAlignment As Excel.XlHAlign? = Nothing,
                                  Optional verticalAlignment As Excel.XlVAlign? = Nothing)
            If sheet Is Nothing Then Throw New ArgumentNullException("sheet")
            If rowSpan < 1 Then Throw New ArgumentOutOfRangeException("rowSpan", "A span must cover at least one row.")
            If columnSpan < 1 Then Throw New ArgumentOutOfRangeException("columnSpan", "A span must cover at least one column.")

            Dim topLeftAddress As String = ToAddress(startRow, startColumn)
            Dim fullAddress As String = topLeftAddress & ":" & ToAddress(startRow + rowSpan - 1, startColumn + columnSpan - 1)

            Dim topLeft As Excel.Range = Nothing
            Dim fullRange As Excel.Range = Nothing

            Try
                topLeft = sheet.Range(topLeftAddress)
                fullRange = sheet.Range(fullAddress)

                ' Write the value into the top-left cell first, then merge, then style the
                ' merged area so that fill and borders cover the whole span.
                WriteValue(topLeft, value, style)

                If rowSpan > 1 OrElse columnSpan > 1 Then MergeRange(fullRange, False)

                ApplyStyle(fullRange, style)

                ' Applied after the merge so that the alignment covers the whole span.
                ApplyLayoutOverrides(fullRange, style, wrapText, horizontalAlignment, verticalAlignment)
            Finally
                ReleaseCom(fullRange)
                ReleaseCom(topLeft)
            End Try
        End Sub

        ''' <summary>
        ''' Merges a range without showing Excel's confirmation prompt.
        ''' </summary>
        ''' <param name="target">The range to merge.</param>
        ''' <param name="mergeEachRowSeparately">If True, each row is merged on its own; otherwise the range becomes one merged cell.</param>
        ''' <remarks>
        ''' Merging non-empty cells normally triggers a modal "only the upper-left value will be kept" prompt, which would freeze
        ''' an invisible Excel instance. To avoid this, <c>DisplayAlerts</c> is turned off during the merge and its previous
        ''' value is restored afterwards, even if the merge fails.
        ''' </remarks>
        Private Sub MergeRange(target As Excel.Range, mergeEachRowSeparately As Boolean)
            Dim app As Excel.Application = Nothing

            Try
                app = target.Application

                ' Merging non-empty cells normally triggers a modal "only the upper-left value
                ' will be kept" prompt, which would freeze an invisible Excel instance.
                Dim previousAlerts As Boolean = app.DisplayAlerts
                app.DisplayAlerts = False

                Try
                    target.Merge(mergeEachRowSeparately)
                Finally
                    app.DisplayAlerts = previousAlerts
                End Try
            Finally
                ReleaseCom(app)
            End Try
        End Sub

        ''' <summary>
        ''' Sets a formula on a cell or a range. Uses the Formula property: English function names
        ''' and comma separators, whatever the language of the installed Excel.
        ''' When the address is a range, relative references adjust per cell, like filling down:
        ''' SetFormula(sheet, "D2:D10", "=B2*C2") gives D3 = B3*C3, and so on.
        ''' Set useLocalizedSyntax = True to use the Excel UI language instead
        ''' (for example SUMA and semicolons in a Spanish installation).
        ''' </summary>
        ''' <param name="sheet">The worksheet to write to.</param>
        ''' <param name="address">The A1-style address of a cell or range (e.g., "D2" or "D2:D10").</param>
        ''' <param name="formula">The formula to set. The leading equals sign is optional and is added if missing.</param>
        ''' <param name="style">An optional cell style to apply. Its number format is set before the formula is written.</param>
        ''' <param name="useLocalizedSyntax">
        ''' If True, the formula is written with <c>FormulaLocal</c> (UI language, local separators); if False (the default),
        ''' it is written with <c>Formula</c> (English names, commas).
        ''' </param>
        ''' <param name="wrapText">An optional flag that overrides the wrap setting of <paramref name="style"/>.</param>
        ''' <param name="horizontalAlignment">An optional horizontal alignment that overrides the one in <paramref name="style"/>.</param>
        ''' <param name="verticalAlignment">An optional vertical alignment that overrides the one in <paramref name="style"/>.</param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> is Nothing.</exception>
        ''' <exception cref="ArgumentException">Thrown if <paramref name="formula"/> is Nothing, empty or only white space.</exception>
        ''' <remarks>
        ''' If Excel cannot parse the formula, it raises a <see cref="COMException"/>.
        ''' </remarks>
        Public Sub SetFormula(sheet As Excel.Worksheet, address As String, formula As String, Optional style As CellStyle = Nothing, Optional useLocalizedSyntax As Boolean = False,
                              Optional wrapText As Boolean? = Nothing, Optional horizontalAlignment As Excel.XlHAlign? = Nothing, Optional verticalAlignment As Excel.XlVAlign? = Nothing)
            If sheet Is Nothing Then Throw New ArgumentNullException("sheet")
            If String.IsNullOrWhiteSpace(formula) Then Throw New ArgumentException("The formula cannot be empty.", "formula")

            Dim normalized As String = NormalizeFormula(formula)
            Dim target As Excel.Range = Nothing

            Try
                target = sheet.Range(address)

                If style IsNot Nothing AndAlso Not String.IsNullOrEmpty(style.NumberFormat) Then target.NumberFormat = style.NumberFormat

                If useLocalizedSyntax Then
                    target.FormulaLocal = normalized
                Else
                    target.Formula = normalized
                End If

                ApplyStyle(target, style)
                ApplyLayoutOverrides(target, style, wrapText, horizontalAlignment, verticalAlignment)
            Finally
                ReleaseCom(target)
            End Try
        End Sub

        ''' <summary>
        ''' Sets a formula using invariant English function names and comma separators.
        ''' </summary>
        ''' <param name="sheet">The worksheet to write to.</param>
        ''' <param name="address">The A1-style address of a cell or range.</param>
        ''' <param name="formula">The formula to set, with English function names and comma separators.</param>
        ''' <param name="style">An optional cell style to apply.</param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> is Nothing.</exception>
        ''' <exception cref="ArgumentException">Thrown if <paramref name="formula"/> is empty.</exception>
        ''' <remarks>
        ''' This is a shortcut for <see cref="SetFormula"/> with <c>useLocalizedSyntax</c> set to False. When adding
        ''' an Excel formula, make sure it has broad version compatibility. The minimum version target you should
        ''' consider is Office 2010. Refer to the documentation to learn more.
        ''' </remarks>
        Public Sub SetNonLocalizedFormula(sheet As Excel.Worksheet, address As String, formula As String, Optional style As CellStyle = Nothing)
            SetFormula(sheet, address, formula, style, useLocalizedSyntax:=False)
        End Sub

        ''' <summary>
        ''' Sets a dynamic-array formula (FILTER, SORT, UNIQUE, SEQUENCE...) that spills into
        ''' neighboring cells. This needs Excel 2019/365 and a recent Interop assembly (Formula2).
        ''' </summary>
        ''' <param name="sheet">The worksheet to write to.</param>
        ''' <param name="address">The A1-style address of the cell where the formula (and its spill range) starts.</param>
        ''' <param name="formula">The dynamic-array formula, with English function names and comma separators.</param>
        ''' <param name="style">An optional cell style applied to the target cell.</param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> is Nothing.</exception>
        ''' <exception cref="ArgumentException">Thrown if <paramref name="formula"/> is empty.</exception>
        ''' <remarks>
        ''' The cells that the result spills into must be empty, otherwise Excel shows a #SPILL! error. The style is applied to
        ''' the anchor cell only, not to the spill range. Earlier Excel versions do not support <c>Formula2</c>.
        ''' </remarks>
        Public Sub SetDynamicArrayFormula(sheet As Excel.Worksheet, address As String, formula As String, Optional style As CellStyle = Nothing)
            If sheet Is Nothing Then Throw New ArgumentNullException("sheet")
            If String.IsNullOrWhiteSpace(formula) Then Throw New ArgumentException("The formula cannot be empty.", "formula")

            Dim normalized As String = NormalizeFormula(formula)
            Dim target As Excel.Range = Nothing

            Try
                target = sheet.Range(address)
                target.Formula2 = normalized
                ApplyStyle(target, style)
            Finally
                ReleaseCom(target)
            End Try
        End Sub

        ''' <summary>
        ''' Makes sure a formula starts with an equals sign.
        ''' </summary>
        ''' <param name="formula">The formula text, with or without a leading equals sign.</param>
        ''' <returns>The trimmed formula, always starting with "=".</returns>
        Private Function NormalizeFormula(formula As String) As String
            Dim trimmed As String = formula.Trim()

            If trimmed.StartsWith("=") Then Return trimmed
            Return "=" & trimmed
        End Function

        ''' <summary>
        ''' Creates a chart from sourceAddress (include the header row/column so series get names)
        ''' and places it over placementAddress, for example "F2:M18".
        ''' The caller owns the returned ChartObject and must release it with ReleaseCom.
        ''' </summary>
        ''' <param name="sheet">The worksheet that contains the data and will host the chart.</param>
        ''' <param name="sourceAddress">The address of the data range (e.g., "A1:C10"), preferably including headers.</param>
        ''' <param name="placementAddress">
        ''' The address of the range the chart will cover (e.g., "F2:M18"). The chart takes the position and size of this range.
        ''' </param>
        ''' <param name="options">
        ''' Optional chart settings (type, title, axis titles, legend, and so on). If Nothing, a new <see cref="ChartOptions"/> with
        ''' default values is used.
        ''' </param>
        ''' <returns>The created <see cref="Excel.ChartObject"/>. The caller must release it with <see cref="ReleaseCom"/>.</returns>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> is Nothing.</exception>
        ''' <remarks>
        ''' Axis titles are only set for chart types that have axes (pie and doughnut charts do not). All intermediate COM
        ''' objects are released internally, except for the returned chart object.
        ''' </remarks>
        Public Function AddChart(sheet As Excel.Worksheet, sourceAddress As String, placementAddress As String, Optional options As ChartOptions = Nothing) As Excel.ChartObject
            If sheet Is Nothing Then Throw New ArgumentNullException("sheet")

            Dim settings As ChartOptions = options

            If settings Is Nothing Then settings = New ChartOptions()

            Dim sourceRange As Excel.Range = Nothing
            Dim placementRange As Excel.Range = Nothing
            Dim chartObjects As Excel.ChartObjects = Nothing
            Dim chartObject As Excel.ChartObject = Nothing
            Dim chart As Excel.Chart = Nothing
            Dim titleObject As Excel.ChartTitle = Nothing
            Dim legend As Excel.Legend = Nothing

            Try
                sourceRange = sheet.Range(sourceAddress)
                placementRange = sheet.Range(placementAddress)

                ' Range.Left/Top/Width/Height are exposed as Object, hence the conversion.
                Dim leftPosition As Double = Convert.ToDouble(placementRange.Left)
                Dim topPosition As Double = Convert.ToDouble(placementRange.Top)
                Dim chartWidth As Double = Convert.ToDouble(placementRange.Width)
                Dim chartHeight As Double = Convert.ToDouble(placementRange.Height)

                chartObjects = DirectCast(sheet.ChartObjects(Type.Missing), Excel.ChartObjects)
                chartObject = chartObjects.Add(leftPosition, topPosition, chartWidth, chartHeight)
                chart = chartObject.Chart

                chart.ChartType = settings.ChartType
                chart.SetSourceData(sourceRange, settings.PlotBy)

                If Not String.IsNullOrEmpty(settings.ChartName) Then
                    chartObject.Name = settings.ChartName
                End If

                ' Title
                If Not String.IsNullOrEmpty(settings.Title) Then
                    chart.HasTitle = True
                    titleObject = chart.ChartTitle
                    titleObject.Text = settings.Title
                Else
                    chart.HasTitle = False
                End If

                ' Axis titles (pie and doughnut charts have no axes)
                If ChartTypeHasAxes(settings.ChartType) Then
                    SetAxisTitle(chart, Excel.XlAxisType.xlCategory, settings.CategoryAxisTitle)
                    SetAxisTitle(chart, Excel.XlAxisType.xlValue, settings.ValueAxisTitle)
                End If

                ' Legend
                chart.HasLegend = settings.ShowLegend

                If settings.ShowLegend Then
                    legend = chart.Legend
                    legend.Position = settings.LegendPosition
                End If

                Return chartObject
            Finally
                ReleaseCom(legend)
                ReleaseCom(titleObject)
                ReleaseCom(chart)
                ReleaseCom(chartObjects)
                ReleaseCom(placementRange)
                ReleaseCom(sourceRange)
                ' chartObject is intentionally NOT released: it is returned to the caller.
            End Try
        End Function

        ''' <summary>Adds one more series to an existing chart from explicit ranges.</summary>
        ''' <param name="chartObject">The chart object to add the series to, usually returned by <see cref="AddChart"/>.</param>
        ''' <param name="sheet">The worksheet that contains the data ranges.</param>
        ''' <param name="seriesName">The name of the new series, shown in the legend.</param>
        ''' <param name="valuesAddress">The address of the range with the values of the series (e.g., "D2:D10").</param>
        ''' <param name="categoriesAddress">
        ''' The optional address of the range with the category (X axis) labels. If Nothing or empty, the chart keeps its current categories.
        ''' </param>
        ''' <exception cref="ArgumentNullException">Thrown if <paramref name="chartObject"/> or <paramref name="sheet"/> is Nothing.</exception>
        ''' <remarks>
        ''' The caller keeps ownership of <paramref name="chartObject"/>. All COM objects created in this method are released internally.
        ''' </remarks>
        Public Sub AddSeriesToChart(chartObject As Excel.ChartObject, sheet As Excel.Worksheet, seriesName As String, valuesAddress As String, Optional categoriesAddress As String = Nothing)
            If chartObject Is Nothing Then Throw New ArgumentNullException("chartObject")
            If sheet Is Nothing Then Throw New ArgumentNullException("sheet")

            Dim chart As Excel.Chart = Nothing
            Dim seriesCollection As Excel.SeriesCollection = Nothing
            Dim newSeries As Excel.Series = Nothing
            Dim valuesRange As Excel.Range = Nothing
            Dim categoriesRange As Excel.Range = Nothing

            Try
                chart = chartObject.Chart
                seriesCollection = DirectCast(chart.SeriesCollection(Type.Missing), Excel.SeriesCollection)
                newSeries = seriesCollection.NewSeries()

                valuesRange = sheet.Range(valuesAddress)
                newSeries.Values = valuesRange
                newSeries.Name = seriesName

                If Not String.IsNullOrEmpty(categoriesAddress) Then
                    categoriesRange = sheet.Range(categoriesAddress)
                    newSeries.XValues = categoriesRange
                End If
            Finally
                ReleaseCom(categoriesRange)
                ReleaseCom(valuesRange)
                ReleaseCom(newSeries)
                ReleaseCom(seriesCollection)
                ReleaseCom(chart)
            End Try
        End Sub

        ''' <summary>
        ''' Sets the axis title of a chart object
        ''' </summary>
        ''' <param name="chart">The chart on which the title should be set</param>
        ''' <param name="axisType">The type of axis</param>
        ''' <param name="title">The title to set to the chart</param>
        ''' <remarks>
        ''' If <paramref name="title"/> is Nothing or empty, the method does nothing and the axis keeps its current state.
        ''' This method must only be called for chart types that have axes; see <see cref="ChartTypeHasAxes"/>.
        ''' </remarks>
        Private Sub SetAxisTitle(chart As Excel.Chart, axisType As Excel.XlAxisType, title As String)
            If String.IsNullOrEmpty(title) Then Exit Sub

            Dim axis As Excel.Axis = Nothing
            Dim axisTitle As Excel.AxisTitle = Nothing

            Try
                axis = DirectCast(chart.Axes(axisType), Excel.Axis)
                axis.HasTitle = True
                axisTitle = axis.AxisTitle
                axisTitle.Text = title
            Finally
                ReleaseCom(axisTitle)
                ReleaseCom(axis)
            End Try
        End Sub

        ''' <summary>
        ''' Determines whether a given chart type can support axes.
        ''' </summary>
        ''' <param name="chartType">The given chart type</param>
        ''' <returns>True if the chart type supports axes, False otherwise.</returns>
        ''' <remarks>
        ''' Pie, pie-of-pie, bar-of-pie, 3-D pie and doughnut charts (including their exploded variants) have no axes, so
        ''' trying to access them would raise a COM exception. All other chart types are considered to have axes.
        ''' </remarks>
        Private Function ChartTypeHasAxes(chartType As Excel.XlChartType) As Boolean
            Dim noAxesInChartTypes() As Integer = New Integer() {
                Excel.XlChartType.xlPie,
                Excel.XlChartType.xlPieExploded,
                Excel.XlChartType.xlPieOfPie,
                Excel.XlChartType.xlBarOfPie,
                Excel.XlChartType.xl3DPie,
                Excel.XlChartType.xl3DPieExploded,
                Excel.XlChartType.xlDoughnut,
                Excel.XlChartType.xlDoughnutExploded
            }

            Return Not noAxesInChartTypes.Contains(chartType)
        End Function

    End Module


End Namespace