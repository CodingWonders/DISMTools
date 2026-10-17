Imports System.Windows.Forms

Public Class ServiceNavigatorDialog

    Public NamesOfServiceDependencies As New List(Of String)
    Public NamesOfServiceDependents As New List(Of String)
    Public NameOfCurrentService As String
    Public NameOfFinalService As String

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ServiceNavigatorDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        lvServiceDependencies.BackColor = BackColor
        lvServiceDependencies.ForeColor = ForeColor
        lvServiceDependents.BackColor = BackColor
        lvServiceDependents.ForeColor = ForeColor

        lblNavigatorHeader.Text = String.Format("Navigate through the hierarchy of {0} by double-clicking an entry:", NameOfCurrentService)

        ColumnHeader1.Width = WindowHelper.ScaleLogical(512)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(512)

        lvServiceDependencies.Items.Clear()
        lvServiceDependents.Items.Clear()
        lvServiceDependencies.Items.AddRange(NamesOfServiceDependencies.OrderBy(Function(service) service).Select(Function(service) New ListViewItem(New String() {service})).ToArray())
        lvServiceDependents.Items.AddRange(NamesOfServiceDependents.OrderBy(Function(service) service).Select(Function(service) New ListViewItem(New String() {service})).ToArray())
    End Sub

    Private Sub ServiceListView_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lvServiceDependents.MouseDoubleClick, lvServiceDependencies.MouseDoubleClick
        If TypeOf sender Is ListView Then
            If CType(sender, ListView).SelectedItems.Count = 1 Then
                NameOfFinalService = CType(sender, ListView).FocusedItem.Text

                DialogResult = Windows.Forms.DialogResult.OK
                Close()
            End If
        End If
    End Sub

    Private Sub ServiceNavigatorDialog_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        ControlPaint.DrawBorder(e.Graphics, ClientRectangle, CurrentTheme.AccentColors(1), ButtonBorderStyle.Solid)
    End Sub
End Class
