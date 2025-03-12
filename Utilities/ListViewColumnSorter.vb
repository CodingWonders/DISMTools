Public Class ListViewColumnSorter
    Implements IComparer

    Private ColumnToSort As Integer
    Private OrderOfSort As SortOrder
    Private ObjectCompare As CaseInsensitiveComparer

    Public Sub New()
        ColumnToSort = 0
        OrderOfSort = SortOrder.None
        ObjectCompare = New CaseInsensitiveComparer()
    End Sub

    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Dim compareResult As Integer
        Dim listviewX, listviewY As ListViewItem

        listviewX = CType(x, ListViewItem)
        listviewY = CType(y, ListViewItem)

        compareResult = ObjectCompare.Compare(listviewX.SubItems(ColumnToSort).Text, listviewY.SubItems(ColumnToSort).Text)

        Select Case OrderOfSort
            Case SortOrder.Ascending
                Return compareResult
            Case SortOrder.Descending
                Return -compareResult
            Case Else
                Return 0
        End Select
    End Function

    Public Property SortColumn As Integer
        Get
            Return ColumnToSort
        End Get
        Set(value As Integer)
            ColumnToSort = value
        End Set
    End Property

    Public Property Order As SortOrder
        Get
            Return OrderOfSort
        End Get
        Set(value As SortOrder)
            OrderOfSort = value
        End Set
    End Property
End Class
