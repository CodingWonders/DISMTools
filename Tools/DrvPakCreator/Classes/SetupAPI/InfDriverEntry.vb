Namespace Classes.SetupAPI

    Public Class InfDriverEntry

        Private m_Manufacturer As String
        Private m_Description As String
        Private m_TargetPlatform As String
        Private m_InstallSection As String
        Private m_ServiceName As String
        Private m_HardwareIDs As List(Of String)

        Public Sub New()
            m_Manufacturer = ""
            m_Description = ""
            m_TargetPlatform = ""
            m_InstallSection = ""
            m_ServiceName = ""
            m_HardwareIDs = New List(Of String)
        End Sub

        Public Property Manufacturer() As String
            Get
                Return m_Manufacturer
            End Get
            Set(ByVal value As String)
                m_Manufacturer = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                m_Description = value
            End Set
        End Property

        Public Property TargetPlatform() As String
            Get
                Return m_TargetPlatform
            End Get
            Set(ByVal value As String)
                m_TargetPlatform = value
            End Set
        End Property

        Public Property InstallSection() As String
            Get
                Return m_InstallSection
            End Get
            Set(ByVal value As String)
                m_InstallSection = value
            End Set
        End Property

        Public Property ServiceName() As String
            Get
                Return m_ServiceName
            End Get
            Set(ByVal value As String)
                m_ServiceName = value
            End Set
        End Property

        Public ReadOnly Property HardwareIDs() As List(Of String)
            Get
                Return m_HardwareIDs
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("{0} - {1} [{2}]", m_Manufacturer, m_Description, m_TargetPlatform)
        End Function

    End Class

End Namespace
