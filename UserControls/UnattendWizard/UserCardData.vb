Imports System.Xml.Serialization

Public Class UserCardData

    Private _id As Integer
    Private _displayName As String
    Private _accountName As String
    Private _base64Password As String
    Private _isAdministrator As Boolean

    <XmlAttribute("Id")>
    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    <XmlAttribute("DisplayName")>
    Public Property DisplayName As String
        Get
            Return _displayName
        End Get
        Set(value As String)
            _displayName = value
        End Set
    End Property

    <XmlAttribute("Name")>
    Public Property AccountName As String
        Get
            Return _accountName
        End Get
        Set(value As String)
            _accountName = value
        End Set
    End Property

    <XmlAttribute("Password")>
    Public Property Base64Password As String
        Get
            Return _base64Password
        End Get
        Set(value As String)
            _base64Password = value
        End Set
    End Property

    <XmlAttribute("IsAdmin")>
    Public Property IsAdministrator As Boolean
        Get
            Return _isAdministrator
        End Get
        Set(value As Boolean)
            _isAdministrator = value
        End Set
    End Property

End Class