Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO

Namespace Elements

    <Serializable(), XmlRoot("UserLocale")>
    Public Class UserLocale

        ''' <summary>
        ''' The ID of the user locale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("Id")>
        Public Property Id As String

        ''' <summary>
        ''' The display name of the user locale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("DisplayName")>
        Public Property DisplayName As String

        ''' <summary>
        ''' The language code of the user locale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("LCID")>
        Public Property LCID As String

        ''' <summary>
        ''' The keyboard identifier of the user locale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("KeyboardLayout")>
        Public Property KeybId As String

        ''' <summary>
        ''' The GeoID of the user locale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("GeoLocation")>
        Public Property GeoLoc As String

        Public Shared Function LoadItems(filePath As String) As List(Of UserLocale)
            Dim localeList As New List(Of UserLocale)
            Try
                Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                    Dim xs As New XmlReaderSettings()
                    xs.IgnoreWhitespace = True
                    Using reader As XmlReader = XmlReader.Create(fs, xs)
                        While reader.Read()
                            If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "UserLocale" Then
                                Dim locale As New UserLocale()
                                locale.Id = reader.GetAttribute("Id")
                                locale.DisplayName = reader.GetAttribute("DisplayName")
                                locale.LCID = reader.GetAttribute("LCID")
                                locale.KeybId = reader.GetAttribute("KeyboardLayout")
                                locale.GeoLoc = reader.GetAttribute("GeoLocation")
                                localeList.Add(locale)
                            End If
                        End While
                    End Using
                End Using
                Return localeList
            Catch ex As Exception
                If Debugger.IsAttached Then Debugger.Break()
                Return Nothing
            End Try
            Return Nothing
        End Function
    End Class

End Namespace
