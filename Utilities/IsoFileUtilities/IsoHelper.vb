Imports System.Management
Imports System.Threading
Imports System.IO

''' <summary>
''' The IsoHelper module provides functions to work with ISO files.
''' </summary>
''' <remarks>
''' This module uses WMI methods and queries, and thus, imposes a requirement of a working WMI setup. If
''' a system with a broken WMI setup invokes the methods in this module, they may fail. In that case,
''' run wmidiag.
''' </remarks>
Module IsoHelper

    Private Const WmiScope As String = "root\Microsoft\Windows\Storage"

    ''' <summary>
    ''' Mounts an ISO file to the system and retrieves the mapped volume letter
    ''' </summary>
    ''' <param name="IsoPath">The path of the ISO file to mount</param>
    ''' <returns>The retrieved mapped volume letter</returns>
    ''' <remarks></remarks>
    Public Function MountIso(IsoPath As String) As Char
        Try
            ' Immediately return the null terminator if it can't find the ISO file.
            If Not File.Exists(IsoPath) Then Return Chr(0)

            Dim isoObjectPath As String = BuildIsoObjectPath(IsoPath)
            Using isoObject As New ManagementObject(WmiScope, isoObjectPath, Nothing)
                Using inParams As ManagementBaseObject = isoObject.GetMethodParameters("Mount")
                    isoObject.InvokeMethod("Mount", inParams, Nothing)
                End Using
            End Using

            ' Now we have to get the volume letter
            Dim volumeQuery As String = String.Format("ASSOCIATORS OF {{{0}}} WHERE ASSOCCLASS = MSFT_DiskImageToVolume RESULTCLASS = MSFT_Volume", isoObjectPath)
            Dim mountLetter As Char = Chr(0)

            Using query As New ManagementObjectSearcher(WmiScope, volumeQuery)
                While mountLetter = Chr(0)
                    Thread.Sleep(50)
                    Using queryCollection As ManagementObjectCollection = query.Get()
                        For Each item As ManagementBaseObject In queryCollection
                            mountLetter = item("DriveLetter").ToString()(0)
                        Next
                    End Using
                End While
            End Using

            Return mountLetter
        Catch ex As Exception
            DynaLog.LogMessage("Could not mount ISO. Error message: " & ex.Message)
            Return Chr(0)
        End Try
    End Function

    ''' <summary>
    ''' Unmounts a given ISO file.
    ''' </summary>
    ''' <param name="IsoPath">The ISO file to unmount</param>
    ''' <remarks></remarks>
    Public Sub DismountIso(IsoPath As String)
        Try
            If Not File.Exists(IsoPath) Then Exit Sub

            Using isoObject As New ManagementObject(WmiScope, BuildIsoObjectPath(IsoPath), Nothing)
                Using inParams As ManagementBaseObject = isoObject.GetMethodParameters("Dismount")
                    isoObject.InvokeMethod("Dismount", inParams, Nothing)
                End Using
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not unmount ISO. Error message: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Creates a WMI object path for the ISO file.
    ''' </summary>
    ''' <param name="IsoPath">The ISO file for which to create the object path</param>
    ''' <returns>The WMI object path</returns>
    ''' <remarks></remarks>
    Private Function BuildIsoObjectPath(IsoPath As String) As String
        Return String.Format("MSFT_DiskImage.ImagePath={0},StorageType=1", ControlChars.Quote & IsoPath.Replace("\", "\\") & ControlChars.Quote)
    End Function

End Module
