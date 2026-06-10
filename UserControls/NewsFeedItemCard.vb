Imports System.Globalization

Public Class NewsFeedItemCard

    Sub SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
    End Sub

    Public Property FeedItemText As String
        Get
            Return FeedItemLinkLabel.Text
        End Get
        Set(value As String)
            FeedItemLinkLabel.Text = value
        End Set
    End Property

    Private _date As Date
    Private _link As String
    Private _contents As String

    Public Property FeedItemDate As Date
        Get
            Return _date
        End Get
        Set(value As Date)
            Dim currentOSCulture As CultureInfo = CultureInfo.CurrentCulture
            _date = value
            FeedItemDateLabel.Text = String.Format("{0}, {1}", _date.ToString(currentOSCulture.DateTimeFormat.LongDatePattern, currentOSCulture),
                                                               _date.ToString(currentOSCulture.DateTimeFormat.LongTimePattern, currentOSCulture))
        End Set
    End Property

    Public Property FeedItemLink As String
        Get
            Return _link
        End Get
        Set(value As String)
            _link = value
        End Set
    End Property

    Public Property FeedItemContents As String
        Get
            Return _contents
        End Get
        Set(value As String)
            _contents = value
        End Set
    End Property

    Public Delegate Sub NewsFeedItemCardLinkClickedEventHandler(sender As Object, e As NewsFeedItemCardLinkClickedEventArgs)
    Public Event LinkContentsEvent As NewsFeedItemCardLinkClickedEventHandler

    Private Sub FeedItemLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles FeedItemLinkLabel.LinkClicked
        Dim lcea As New NewsFeedItemCardLinkClickedEventArgs(FeedItemText, FeedItemContents, FeedItemDate)
        RaiseEvent LinkContentsEvent(Me, lcea)
    End Sub
End Class

Public Class NewsFeedItemCardLinkClickedEventArgs
    Inherits EventArgs

    Private _title As String
    Private _contents As String
    Private _publishDate As Date

    Public Sub New(Title As String, Contents As String, PublishDate As Date)
        _title = Title
        _contents = Contents
        _publishDate = PublishDate
    End Sub

    Public ReadOnly Property Contents As String
        Get
            Return _contents
        End Get
    End Property

    Public ReadOnly Property Title As String
        Get
            Return _title
        End Get
    End Property

    Public ReadOnly Property PublishDate As Date
        Get
            Return _publishDate
        End Get
    End Property

End Class
