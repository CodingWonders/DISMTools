Imports System.IO

Module UserDataManagerModule

    ''' <summary>
    ''' The path to the user data folder
    ''' </summary>
    ''' <remarks></remarks>
    Private ReadOnly UserDataPath As String = Path.Combine(Application.StartupPath, "userdata")

    ''' <summary>
    ''' A mapping between folders in the user data folder and additional folders in the structure
    ''' </summary>
    ''' <remarks></remarks>
    Private UserDataMapping As New Dictionary(Of String, String) From {
        {"dtpe_backgrounds", "bin\extps1\PE_Helper\backgrounds"},
        {"themes", "bin\themes"},
        {"starter_scripts", "AutoUnattend\StarterScripts\UserScripts"}
    }

    ''' <summary>
    ''' Creates the structure of the user data folder if it does not exist.
    ''' </summary>
    ''' <returns>Whether the structure was created successfully.</returns>
    ''' <remarks></remarks>
    Private Function CreateUserDataContents() As Boolean
        Try
            If Not Directory.Exists(UserDataPath) Then
                DynaLog.LogMessage("User Data Folder does not exist. Creating user data folder...")
                Directory.CreateDirectory(UserDataPath)
            Else
                DynaLog.LogMessage("User Data Folder exists. Stopping...")
                Return True
            End If

            DynaLog.LogMessage("Creating subdirectories in user data folder...")
            For Each UserDataFolderEntry In UserDataMapping.Keys
                DynaLog.LogMessage("Proceeding to create subdirectory " & UserDataFolderEntry & "...")
                If Not Directory.Exists(Path.Combine(UserDataPath, UserDataFolderEntry)) Then
                    Directory.CreateDirectory(Path.Combine(UserDataPath, UserDataFolderEntry))
                Else
                    DynaLog.LogMessage("This subdirectory exists. Skipping...")
                End If
            Next

            DynaLog.LogMessage("User data content preparation succeeded.")
            Return True
        Catch ex As Exception
            DynaLog.LogMessage("User data content preparation failed. Error message: " & ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Copies files in user data subdirectories to program files
    ''' </summary>
    ''' <returns>Whether the copy operation succeeded.</returns>
    ''' <remarks></remarks>
    Public Function CopyUserDataToProgramFiles() As Boolean
        Dim success As Boolean = False
        DynaLog.LogMessage("Checking if user data folder exists...")
        If Not Directory.Exists(UserDataPath) AndAlso Not CreateUserDataContents() Then
            DynaLog.LogMessage("User data folder does not exist and creation has failed. Stopping...")
            Return False
        End If

        Try
            DynaLog.LogMessage("Proceeding to copy all user data files to program files...")
            For Each UserDataFolder In UserDataMapping.Keys
                Dim UserDataDestinationPath As String = Path.Combine(Application.StartupPath, UserDataMapping(UserDataFolder))

                DynaLog.LogMessage("Checking if " & UserDataDestinationPath & " exists...")
                If Not Directory.Exists(UserDataDestinationPath) Then
                    DynaLog.LogMessage("Content destination folder does not exist. Creating...")
                    Try
                        Directory.CreateDirectory(UserDataDestinationPath)
                    Catch ex As Exception
                        DynaLog.LogMessage("Content destination folder creation failed. Skipping user data file collection...")
                        Continue For
                    End Try
                End If

                DynaLog.LogMessage("Copying all user data files to destination...")
                Dim filesInUserDataSource As String() = Directory.GetFiles(Path.Combine(UserDataPath, UserDataFolder))
                For Each fileInUserDataSource In filesInUserDataSource
                    Try
                        DynaLog.LogMessage("Copying " & ControlChars.Quote & fileInUserDataSource & ControlChars.Quote & " to destination folder...")
                        File.Copy(fileInUserDataSource, Path.Combine(UserDataDestinationPath, Path.GetFileName(fileInUserDataSource)), True)
                    Catch ex As Exception
                        DynaLog.LogMessage("Copy operation for this file has failed. Error message: " & ex.Message)
                        Continue For
                    End Try
                Next
            Next

            success = True
        Catch ex As Exception
            DynaLog.LogMessage("The copy operation has failed. Error message: " & ex.Message)
        End Try

        Return success
    End Function

    Public Function CopyUserDataToProgramFiles(UserDataItem As String) As Boolean
        Dim success As Boolean = False
        DynaLog.LogMessage("Checking if user data folder exists...")
        If Not Directory.Exists(UserDataPath) AndAlso Not CreateUserDataContents() Then
            DynaLog.LogMessage("User data folder does not exist and creation has failed. Stopping...")
            Return False
        End If

        If Not UserDataMapping.ContainsKey(UserDataItem) Then
            Return False
        End If

        Try
            DynaLog.LogMessage("Proceeding to copy user data files to program files...")
            Dim UserDataDestinationPath As String = Path.Combine(Application.StartupPath, UserDataMapping(UserDataItem))

            DynaLog.LogMessage("Checking if " & UserDataDestinationPath & " exists...")
            If Not Directory.Exists(UserDataDestinationPath) Then
                DynaLog.LogMessage("Content destination folder does not exist. Creating...")
                Try
                    Directory.CreateDirectory(UserDataDestinationPath)
                Catch ex As Exception
                    DynaLog.LogMessage("Content destination folder creation failed. Skipping user data file collection...")
                    Return False
                End Try
            End If

            DynaLog.LogMessage("Copying all user data files to destination...")
            Dim filesInUserDataSource As String() = Directory.GetFiles(Path.Combine(UserDataPath, UserDataItem))
            For Each fileInUserDataSource In filesInUserDataSource
                Try
                    DynaLog.LogMessage("Copying " & ControlChars.Quote & fileInUserDataSource & ControlChars.Quote & " to destination folder...")
                    File.Copy(fileInUserDataSource, Path.Combine(UserDataDestinationPath, Path.GetFileName(fileInUserDataSource)), True)
                Catch ex As Exception
                    DynaLog.LogMessage("Copy operation for this file has failed. Error message: " & ex.Message)
                    Continue For
                End Try
            Next

            success = True
        Catch ex As Exception
            DynaLog.LogMessage("The copy operation has failed. Error message: " & ex.Message)
        End Try

        Return success
    End Function

End Module
