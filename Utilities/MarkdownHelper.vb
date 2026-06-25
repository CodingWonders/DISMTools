Imports Microsoft.VisualBasic.ControlChars

Module MarkdownHelper

    ''' <summary>
    ''' The size of a header
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum HeaderSize
        Header1 = 1
        Header2 = 2
        Header3 = 3
        Header4 = 4
        Header5 = 5
        Header6 = 6
    End Enum

    ''' <summary>
    ''' The style of a paragraph
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ParagraphStyle
        ''' <summary>
        ''' No styles are applied
        ''' </summary>
        ''' <remarks></remarks>
        Normal
        ''' <summary>
        ''' A bold style is applied
        ''' </summary>
        ''' <remarks></remarks>
        Bold
        ''' <summary>
        ''' An italic style is applied
        ''' </summary>
        ''' <remarks></remarks>
        Italic
        ''' <summary>
        ''' An underline style is applied
        ''' </summary>
        ''' <remarks></remarks>
        Underline
    End Enum

    ''' <summary>
    ''' Gets a header for Markdown files
    ''' </summary>
    ''' <param name="Contents">The contents of a header</param>
    ''' <param name="Size">The size of the header. If it is not specified, the biggest header size will be used</param>
    ''' <returns>A Markdown header</returns>
    ''' <remarks></remarks>
    Public Function GetHeader(Contents As String, Optional Size As HeaderSize = HeaderSize.Header1) As String
        If Contents = "" Then
            Return ""
        End If
        Return String.Format("{0} {1}{2}", New String("#", Size), Contents, CrLf)
    End Function

    ''' <summary>
    ''' Gets a paragraph for Markdown files
    ''' </summary>
    ''' <param name="Contents">The contents of a paragraph</param>
    ''' <param name="Style">The style of the whole paragraph. This does not apply to specific parts of a paragraph, which might have different styles. If it is not specified, no styles will be used</param>
    ''' <returns>A Markdown paragraph</returns>
    ''' <remarks></remarks>
    Public Function GetParagraph(Contents As String, Optional Style As ParagraphStyle = ParagraphStyle.Normal) As String
        If Contents = "" Then
            Return ""
        End If
        Select Case Style
            Case ParagraphStyle.Normal
                Return String.Format("{0}{1}{2}", CrLf, Contents, CrLf)
            Case ParagraphStyle.Bold
                Return String.Format("{0}**{1}**{2}", CrLf, Contents, CrLf)
            Case ParagraphStyle.Italic
                Return String.Format("{0}*{1}*{2}", CrLf, Contents, CrLf)
            Case ParagraphStyle.Underline
                Return String.Format("{0}<u>{1}</u>{2}", CrLf, Contents, CrLf)
        End Select
        Return Contents
    End Function

    ''' <summary>
    ''' Gets a table header for Markdown files
    ''' </summary>
    ''' <param name="HeaderColumns">The columns of the header. These will be delimited</param>
    ''' <returns>A Markdown table header</returns>
    ''' <remarks>A row is to be added later on</remarks>
    Public Function GetTableHeader(HeaderColumns As List(Of String)) As String
        Dim fullTableHeader As String = CrLf    ' We begin with a newline in order to keep consitency with rest of functions
        If HeaderColumns.Count > 0 Then
            ' First we write the table headers
            For Each HeaderColumn In HeaderColumns
                fullTableHeader &= String.Format("| {0} ", HeaderColumn)
            Next
            ' Then we close the table headers and indicate the alignment
            fullTableHeader &= " |" & CrLf & String.Concat(Enumerable.Repeat("|:--", HeaderColumns.Count)) & "|" & CrLf
            Return fullTableHeader
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Gets a table row for Markdown files
    ''' </summary>
    ''' <param name="RowColumns">The columns of the row. These will be delimited</param>
    ''' <returns>A Markdown table row</returns>
    ''' <remarks></remarks>
    Public Function GetTableRow(RowColumns As List(Of String)) As String
        ' Same thing here, but for rows
        Dim fullTableRow As String = ""
        If RowColumns.Count > 0 Then
            For Each RowColumn In RowColumns
                fullTableRow &= String.Format("| {0} ", RowColumn)
            Next
            fullTableRow &= " |" & CrLf
            Return fullTableRow
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Gets a collection of list items for Markdown files
    ''' </summary>
    ''' <param name="ListItems">The items in a list</param>
    ''' <param name="Ordered">Determines whether or not to order the items. If it is not specified, items will be unordered</param>
    ''' <returns>A Markdown list item</returns>
    ''' <remarks></remarks>
    Public Function GetListItems(ListItems As List(Of String), Optional Ordered As Boolean = False) As String
        Dim fullListItem As String = ""
        Dim startingNumber As Integer = 1
        If ListItems.Count > 0 Then
            For Each ListItem In ListItems
                If Ordered Then
                    fullListItem &= String.Format("{0}. {1}{2}", startingNumber, ListItem, CrLf)
                    startingNumber += 1
                Else
                    fullListItem &= String.Format("- {0}{1}", ListItem, CrLf)
                End If
            Next
            Return fullListItem
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Gets a URL link string for Markdown files
    ''' </summary>
    ''' <param name="Uri">The site to go to when clicking the link</param>
    ''' <param name="DisplayString">The name to show to the user of the link</param>
    ''' <returns>A Markdown link string</returns>
    ''' <remarks></remarks>
    Public Function GetLink(Uri As String, DisplayString As String) As String
        Return String.Format("[{0}]({1})", DisplayString, Uri)
    End Function

End Module
