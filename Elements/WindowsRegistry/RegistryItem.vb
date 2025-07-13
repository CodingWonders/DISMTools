''' <summary>
''' Class type for Windows Registry items
''' </summary>
''' <remarks></remarks>
Public Class RegistryItem

    ''' <summary>
    ''' The possible types a value can be that are interpreted by both REG and REGEDIT
    ''' </summary>
    ''' <remarks></remarks>
    Enum ValueType
        ''' <summary>
        ''' Undefined value type
        ''' </summary>
        ''' <remarks>Binary data stored by this key will be shown as hex in REGEDIT</remarks>
        RegNone = 0
        ''' <summary>
        ''' Single-line, null-terminated string value type
        ''' </summary>
        ''' <remarks>Default value</remarks>
        RegSz = 1
        ''' <summary>
        ''' Single-line, null-terminated string value type that contains unexpanded system environment variables
        ''' </summary>
        ''' <remarks>
        ''' For example, "%WINDIR%\explorer.exe" will be interpreted by the target system as "%SYSTEMDRIVE%\WINDOWS\explorer.exe",
        ''' therefore expanding the WINDIR environment variable to "%SYSTEMDRIVE%\WINDOWS" when getting the value data. In this example,
        ''' the %SYSTEMDRIVE% environment variable is used instead of "C:" because the system volume could have been assigned a drive letter
        ''' other than "C"
        ''' </remarks>
        RegExpandSz = 2
        ''' <summary>
        ''' Multi-line, null-terminated string value type
        ''' </summary>
        ''' <remarks></remarks>
        RegMultiSz = 3
        ''' <summary>
        ''' Binary value type
        ''' </summary>
        ''' <remarks>Binary data stored by this key will be shown as hex in REGEDIT</remarks>
        RegBinary = 4
        ''' <summary>
        ''' 32-bit integer value type
        ''' </summary>
        ''' <remarks></remarks>
        RegDword = 5
        ''' <summary>
        ''' 64-bit integer value type
        ''' </summary>
        ''' <remarks></remarks>
        RegQword = 6
    End Enum

    ''' <summary>
    ''' The path to the registry subkey in which the value will be added
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RegistryKeyLocation As String
    ''' <summary>
    ''' The name of the registry value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>If it's an empty string, the Registry Helper interprets it as the default value and calls REG with the "/ve" flag when supported</remarks>
    Public Property RegistryValueName As String
    ''' <summary>
    ''' The type of the registry value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Look at the ValueType enum values for more information</remarks>
    Public Property RegistryValueType As ValueType
    ''' <summary>
    ''' The data stored in the registry value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RegistryValueData As String

    ''' <summary>
    ''' Initializes a registry item object using the default single-line string value type
    ''' </summary>
    ''' <param name="loc">Where to store the value</param>
    ''' <param name="name">The name of the value</param>
    ''' <param name="data">The data stored in the value</param>
    ''' <remarks></remarks>
    Public Sub New(loc As String, name As String, data As String)
        Me.RegistryKeyLocation = loc
        Me.RegistryValueName = name
        Me.RegistryValueType = ValueType.RegSz
        Me.RegistryValueData = data
    End Sub

    ''' <summary>
    ''' Initializes a registry item object
    ''' </summary>
    ''' <param name="loc">Where to store the value</param>
    ''' <param name="name">The name of the value</param>
    ''' <param name="type">The type of the registry value</param>
    ''' <param name="data">The data stored in the value</param>
    ''' <remarks></remarks>
    Public Sub New(loc As String, name As String, type As ValueType, data As String)
        Me.RegistryKeyLocation = loc
        Me.RegistryValueName = name
        Me.RegistryValueType = type
        Me.RegistryValueData = data
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format("{0}!{1}; type {2}; data {3}", Me.RegistryKeyLocation, Me.RegistryValueName, Me.RegistryValueType, Me.RegistryValueData)
    End Function

End Class
