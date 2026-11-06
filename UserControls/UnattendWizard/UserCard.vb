Imports System.Xml.Serialization
Imports DISMTools.Elements
Imports System.Text

Public Class UserCard

    Private _id As Integer
    Private _userDisplayName As String
    Private _userAccountName As String
    Private _userAccountPassword As String
    Private _userAccountIsAdministrator As Boolean

    ''' <summary>
    ''' The identifier of the user card
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    ''' <summary>
    ''' The name of the account
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Account names must not exceed 20 characters</remarks>
    Public Property DisplayName As String
        Get
            Return _userDisplayName
        End Get
        Set(value As String)
            RemoveHandler tbUserDisplayName.TextChanged, AddressOf tbUserDisplayName_TextChanged
            _userDisplayName = value
            tbUserDisplayName.Text = value
            AddHandler tbUserDisplayName.TextChanged, AddressOf tbUserDisplayName_TextChanged
        End Set
    End Property

    ''' <summary>
    ''' The display name of the account
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Account display names must not exceed 256 characters</remarks>
    Public Property AccountName As String
        Get
            Return _userAccountName
        End Get
        Set(value As String)
            RemoveHandler tbUserAccountName.TextChanged, AddressOf tbUserAccountName_TextChanged
            _userAccountName = value
            tbUserAccountName.Text = value
            AddHandler tbUserAccountName.TextChanged, AddressOf tbUserAccountName_TextChanged
        End Set
    End Property

    ''' <summary>
    ''' The password of the account
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Password As String
        Get
            Return _userAccountPassword
        End Get
        Set(value As String)
            RemoveHandler tbUserPassword.TextChanged, AddressOf tbUserPassword_TextChanged
            _userAccountPassword = value
            tbUserPassword.Text = value
            AddHandler tbUserPassword.TextChanged, AddressOf tbUserPassword_TextChanged
        End Set
    End Property

    Public Property Base64Password As String
        Get
            Try
                Return Convert.ToBase64String(Encoding.UTF8.GetBytes(Password))
            Catch ex As Exception
                Return ""
            End Try
        End Get
        Set(value As String)
            Try
                Password = Encoding.UTF8.GetString(Convert.FromBase64String(value))
            Catch ex As Exception
                Password = ""
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Whether the user is part of the Administrators group (S-1-5-32-544) in addition to the Users group (S-1-5-32-545)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsAdministrator As Boolean
        Get
            Return _userAccountIsAdministrator
        End Get
        Set(value As Boolean)
            RemoveHandler comboUserGroup.SelectedIndexChanged, AddressOf comboUserGroup_SelectedIndexChanged
            _userAccountIsAdministrator = value
            comboUserGroup.SelectedIndex = If(value, 0, 1)
            AddHandler comboUserGroup.SelectedIndexChanged, AddressOf comboUserGroup_SelectedIndexChanged
        End Set
    End Property

    Public WriteOnly Property CanDeleteUser As Boolean
        Set(value As Boolean)
            btnRemoveUser.Enabled = value
        End Set
    End Property

    Private Sub comboUserGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles comboUserGroup.SelectedIndexChanged
        IsAdministrator = comboUserGroup.SelectedIndex = 0
    End Sub

    Private Sub cbShowPassword_CheckedChanged(sender As Object, e As EventArgs) Handles cbShowPassword.CheckedChanged
        tbUserPassword.PasswordChar = If(cbShowPassword.Checked, Nothing, "●")
    End Sub

    Public Sub SetColors()
        If CurrentTheme IsNot Nothing Then
            BackColor = CurrentTheme.SectionBackgroundColor
            ForeColor = CurrentTheme.ForegroundColor

            tbUserAccountName.BackColor = BackColor
            tbUserAccountName.ForeColor = ForeColor
            tbUserDisplayName.BackColor = BackColor
            tbUserDisplayName.ForeColor = ForeColor
            tbUserPassword.BackColor = BackColor
            tbUserPassword.ForeColor = ForeColor
            comboUserGroup.BackColor = BackColor
            comboUserGroup.ForeColor = ForeColor
        End If
    End Sub

    Private Sub UserCard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetColors()
    End Sub

    Private Sub tbUserAccountName_TextChanged(sender As Object, e As EventArgs) Handles tbUserAccountName.TextChanged
        AccountName = tbUserAccountName.Text
    End Sub

    Private Sub tbUserPassword_TextChanged(sender As Object, e As EventArgs) Handles tbUserPassword.TextChanged
        Password = tbUserPassword.Text
    End Sub

    Private Sub tbUserDisplayName_TextChanged(sender As Object, e As EventArgs) Handles tbUserDisplayName.TextChanged
        DisplayName = tbUserDisplayName.Text
    End Sub

    Private Sub tlpCardBody_Paint(sender As Object, e As PaintEventArgs) Handles tlpCardBody.Paint
        Dim finalColor As Color = If(CurrentTheme IsNot Nothing, CurrentTheme.AccentColors(0), Color.FromKnownColor(KnownColor.Control))
        ControlPaint.DrawBorder(e.Graphics, tlpCardBody.ClientRectangle, finalColor, ButtonBorderStyle.Solid)
    End Sub

    Public Delegate Sub AccountCardDeletionEventHandler(sender As Object, e As AccountCardDeletionEventArgs)
    Public Event AccountDeletionEvent As AccountCardDeletionEventHandler

    Private Sub btnRemoveUser_Click(sender As Object, e As EventArgs) Handles btnRemoveUser.Click
        Dim acdea As New AccountCardDeletionEventArgs(Id)
        RaiseEvent AccountDeletionEvent(Me, acdea)
    End Sub

    Private Sub tbUserDisplayName_Leave(sender As Object, e As EventArgs) Handles tbUserDisplayName.Leave
        ' If we haven't put anything in the system account field we suggest a name
        If AccountName = "" Then AccountName = UserNameSuggester.SuggestUserName(tbUserDisplayName.Text)
    End Sub

    Public Delegate Sub AccountCardNameGenerationEventHandler(sender As Object, e As AccountCardNameGenerationEventArgs)
    Public Event AccountNameGenerationEvent As AccountCardNameGenerationEventHandler

    Private Sub btnGenerateUserName_Click(sender As Object, e As EventArgs) Handles btnGenerateUserName.Click
        Dim acngea As New AccountCardNameGenerationEventArgs(Id, DisplayName)
        RaiseEvent AccountNameGenerationEvent(Me, acngea)
    End Sub

    Public Function ToData() As UserCardData
        Return New UserCardData() With {
            .Id = Id,
            .DisplayName = DisplayName,
            .AccountName = AccountName,
            .Base64Password = Base64Password,
            .IsAdministrator = IsAdministrator
        }
    End Function

    Public Sub FromData(data As UserCardData)
        If data Is Nothing Then Exit Sub

        Id = data.Id
        DisplayName = data.DisplayName
        AccountName = data.AccountName
        Base64Password = data.Base64Password
        IsAdministrator = data.IsAdministrator
    End Sub
End Class

Public Class AccountCardNameGenerationEventArgs
    Inherits EventArgs

    Private _id As Integer
    Private _name As String

    Public Sub New(Id As Integer, Name As String)
        _id = Id
        _name = Name
    End Sub

    ''' <summary>
    ''' The identifier of the user card
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Id As Integer
        Get
            Return _id
        End Get
    End Property

    ''' <summary>
    ''' The name of the account
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Account names must not exceed 20 characters</remarks>
    Public ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

End Class

Public Class AccountCardDeletionEventArgs
    Inherits EventArgs

    Private _id As Integer

    Public Sub New(Id As Integer)
        _id = Id
    End Sub

    ''' <summary>
    ''' The identifier of the user card
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Id As Integer
        Get
            Return _id
        End Get
    End Property

End Class

<XmlRoot("AccountCards")>
Public Class AccountCardCollection

    Private _cards As New List(Of UserCardData)

    <XmlElement("AccountCard")>
    Public Property Cards As List(Of UserCardData)
        Get
            Return _cards
        End Get
        Set(value As List(Of UserCardData))
            _cards = value
        End Set
    End Property

    Public Sub New()
        Cards = New List(Of UserCardData)
    End Sub

End Class
