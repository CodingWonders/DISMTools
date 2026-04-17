Imports System.Diagnostics
Imports System.IO
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars

Module RegistryHelper

    ' The error constant values don't have any meaning. This goes to you, SPEEDY. Only me and you know it.
    Private Const DTERR_RegNotFound As Integer = &H24D80191
    Private Const DTERR_RegItemObjectNull As Integer = &H24D80192

    Private Const ERROR_SUCCESS As Integer = 0

    ''' <summary>
    ''' Runs a REG process
    ''' </summary>
    ''' <param name="CommandLine">The command line arguments to pass to the REG program</param>
    ''' <returns>The exit code of REG process</returns>
    ''' <remarks></remarks>
    Private Function RunRegProcess(CommandLine As String) As Integer
        DynaLog.LogMessage("Running REG process...")
        DynaLog.LogMessage("- Command-line Arguments: " & CommandLine)
        DynaLog.LogMessage("Checking presence of REG program...")
        If Not File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "reg.exe")) Then
            DynaLog.LogMessage("REG is not found. Aborting this procedure!")
            Return DTERR_RegNotFound
        End If
        DynaLog.LogMessage("REG found. Running...")
        Dim regProc As New Process With {
            .StartInfo = New ProcessStartInfo With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "reg.exe"),
                .Arguments = CommandLine,
                .CreateNoWindow = Not Debugger.IsAttached,
                .WindowStyle = If(Debugger.IsAttached, ProcessWindowStyle.Normal, ProcessWindowStyle.Hidden)
            }
        }
        regProc.Start()
        regProc.WaitForExit()
        Return regProc.ExitCode
    End Function

    Public Function RegistryKeyExists(KeyPath As String) As Boolean
        Return RunRegProcess(String.Format("query {0}{1}{0}", Quote, KeyPath.Replace(Quote, ""))) = ERROR_SUCCESS
    End Function

    Public Function RegistryValueExists(KeyPath As String, ValueName As String) As Boolean
        Return RunRegProcess(String.Format("query {0}{1}{0} {2}", Quote, KeyPath.Replace(Quote, ""),
                                           If(ValueName = "", "/ve",
                                              String.Format("/v {0}{1}{0}", Quote, ValueName.Replace(Quote, ""))))) = ERROR_SUCCESS
    End Function

    ''' <summary>
    ''' Gets an appropriate representation of registry value types for REG commands
    ''' </summary>
    ''' <param name="ValueType">The registry value type</param>
    ''' <returns>The representation for REG commands</returns>
    ''' <remarks></remarks>
    Private Function GetRegValueTypeFromEnum(ValueType As RegistryItem.ValueType) As String
        Select Case ValueType
            Case RegistryItem.ValueType.RegNone
                Return "REG_NONE"
            Case RegistryItem.ValueType.RegSz
                Return "REG_SZ"
            Case RegistryItem.ValueType.RegExpandSz
                Return "REG_EXPAND_SZ"
            Case RegistryItem.ValueType.RegMultiSz
                Return "REG_MULTI_SZ"
            Case RegistryItem.ValueType.RegBinary
                Return "REG_BINARY"
            Case RegistryItem.ValueType.RegDword
                Return "REG_DWORD"
            Case RegistryItem.ValueType.RegQword
                Return "REG_QWORD"
        End Select
        Return ""
    End Function

    ''' <summary>
    ''' Adds a registry item to the system
    ''' </summary>
    ''' <param name="regItem">The new registry item</param>
    ''' <returns>The exit code of the underlying REG process call</returns>
    ''' <remarks></remarks>
    Function AddRegistryItem(regItem As RegistryItem) As Integer
        If regItem Is Nothing Then Return DTERR_RegItemObjectNull

        DynaLog.LogMessage("Adding registry item " & regItem.ToString())

        Return RunRegProcess(String.Format("add {0} /f {1} /t {2} /d {3}",
                                           Quote & regItem.RegistryKeyLocation & Quote,
                                           If(String.IsNullOrEmpty(regItem.RegistryValueName),
                                              "/ve",
                                              String.Format("/v {0}",
                                                            regItem.RegistryValueName)),
                                           GetRegValueTypeFromEnum(regItem.RegistryValueType),
                                           Quote & regItem.RegistryValueData & Quote))
    End Function

    ''' <summary>
    ''' Removes a registry item from the system
    ''' </summary>
    ''' <param name="regPath">The absolute path to the item (key or value)</param>
    ''' <param name="deletionArgs">Deletion arguments to pass to REG</param>
    ''' <returns>The exit code of the underlying REG process call</returns>
    ''' <remarks></remarks>
    Function RemoveRegistryItem(regPath As String, deletionArgs As String) As Integer
        If String.IsNullOrEmpty(regPath) Then Return DTERR_RegItemObjectNull
        If String.IsNullOrEmpty(deletionArgs) Then Return DTERR_RegItemObjectNull

        DynaLog.LogMessage(String.Format("Removing registry item {0} with provided REG argument {1}", regPath, deletionArgs))

        Return RunRegProcess(String.Format("delete {0} {1}",
                                           ControlChars.Quote & regPath & ControlChars.Quote,
                                           deletionArgs))
    End Function

    ''' <summary>
    ''' Loads a registry hive to the system
    ''' </summary>
    ''' <param name="regHivePath">The path of the registry hive</param>
    ''' <param name="regMountedPath">The path to mount the registry hive to</param>
    ''' <returns>The exit code of the underlying REG process call</returns>
    ''' <remarks></remarks>
    Function LoadRegistryHive(regHivePath As String, regMountedPath As String) As Integer
        If String.IsNullOrEmpty(regHivePath) Then Return DTERR_RegItemObjectNull
        If String.IsNullOrEmpty(regMountedPath) Then Return DTERR_RegItemObjectNull

        DynaLog.LogMessage(String.Format("Loading registry hive {0} to path {1}", regHivePath, regMountedPath))

        Return RunRegProcess(String.Format("load {0} {1}",
                                           regMountedPath,
                                           ControlChars.Quote & regHivePath & ControlChars.Quote))
    End Function

    ''' <summary>
    ''' Unloads a registry hive from the system
    ''' </summary>
    ''' <param name="regMountedPath">The path of the mounted hive to unload</param>
    ''' <returns>The exit code of the underlying REG process call</returns>
    ''' <remarks></remarks>
    Function UnloadRegistryHive(regMountedPath As String) As Integer
        If String.IsNullOrEmpty(regMountedPath) Then Return DTERR_RegItemObjectNull

        DynaLog.LogMessage(String.Format("Unloading registry hive {0}", regMountedPath))

        Return RunRegProcess(String.Format("unload {0}",
                                           regMountedPath))
    End Function

    ''' <summary>
    ''' Gets the default control set of a system's SYSTEM hive
    ''' </summary>
    ''' <param name="RegistryPath">The path to the loaded hive (DO NOT ADD HKLM)</param>
    ''' <returns>The default control set value if obtained, -1 otherwise</returns>
    ''' <remarks></remarks>
    Function GetDefaultControlSet(RegistryPath As String) As Integer
        If String.IsNullOrEmpty(RegistryPath) Then Return DTERR_RegItemObjectNull

        Try
            Dim ControlSetRk As RegistryKey = Registry.LocalMachine.OpenSubKey(String.Format("{0}\Select", RegistryPath), False)
            Dim ControlSet As Integer = ControlSetRk.GetValue("Default", -1)
            ControlSetRk.Close()

            Return ControlSet
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Function ExportRegistryToFile(RegistryPath As String, TargetRegFile As String) As Integer
        If String.IsNullOrEmpty(RegistryPath) Then Return DTERR_RegItemObjectNull
        If String.IsNullOrEmpty(TargetRegFile) Then Return DTERR_RegItemObjectNull

        Return RunRegProcess(String.Format("export " & ControlChars.Quote & "{0}" & ControlChars.Quote & " " & ControlChars.Quote & "{1}" & ControlChars.Quote & " /y",
                                           RegistryPath, TargetRegFile))
    End Function

End Module
