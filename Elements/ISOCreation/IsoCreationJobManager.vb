Imports System.Collections.Concurrent
Imports System.Threading.Tasks

Namespace Elements.ISOCreation

    Public Class IsoCreationJobManager

        Private ReadOnly _jobQueue As New ConcurrentQueue(Of KeyValuePair(Of Integer, IsoCreationTask))

        Public ReadOnly Property JobQueue As List(Of KeyValuePair(Of Integer, IsoCreationTask))
            Get
                Return _jobQueue.ToList()
            End Get
        End Property

        Private ReadOnly _activeTasks As New Dictionary(Of Integer, IsoCreationTask)

        Public ReadOnly Property ActiveTasks As List(Of KeyValuePair(Of Integer, IsoCreationTask))
            Get
                SyncLock _syncLock
                    Return _activeTasks.ToList()
                End SyncLock
            End Get
        End Property

        Private ReadOnly _jobStatuses As New ConcurrentDictionary(Of Integer, JobStatus)
        Private ReadOnly _jobMetadata As New ConcurrentDictionary(Of Integer, JobMetadata)
        Private ReadOnly _maxConcurrentTasks As Integer
        Private _nextJobId As Integer = 0
        Private _isProcessing As Boolean = False
        Private ReadOnly _syncLock As New Object()

        Public Event JobStatusChanged(jobId As Integer, status As JobStatus)
        Public Event JobProgressChanged(jobId As Integer, isRunning As Boolean)

        Public Sub New(Optional maxConcurrentTasks As Integer = 2)
            _maxConcurrentTasks = If(maxConcurrentTasks > 0, maxConcurrentTasks, 2)
        End Sub

        ''' <summary>
        ''' Queues an ISO creation task for execution.
        ''' </summary>
        Public Function QueueJob(sourceImage As String, destinationIso As String, architecture As IsoArchitecture,
                                unattendedFile As String, copyToVentoy As Boolean, useUEFICA2023 As Boolean,
                                includeSystemDrivers As Boolean) As Integer

            Dim task As New IsoCreationTask(sourceImage, destinationIso, architecture, unattendedFile,
                                            copyToVentoy, useUEFICA2023, includeSystemDrivers)

            Dim jobId = System.Threading.Interlocked.Increment(_nextJobId)

            ' Queue the task with its jobId
            _jobQueue.Enqueue(New KeyValuePair(Of Integer, IsoCreationTask)(jobId, task))

            _jobStatuses.TryAdd(jobId, JobStatus.Queued)

            ' Store metadata for later retrieval
            Dim metadata As New JobMetadata With {
                .DestinationIsoFile = destinationIso,
                .SourceImageFile = sourceImage,
                .Architecture = architecture
            }
            _jobMetadata.TryAdd(jobId, metadata)

            RaiseEvent JobStatusChanged(jobId, JobStatus.Queued)

            ProcessQueue()

            Return jobId
        End Function

        ''' <summary>
        ''' Gets the status of a specific job.
        ''' </summary>
        Public Function GetJobStatus(jobId As Integer) As JobStatus
            Dim status As JobStatus
            If _jobStatuses.TryGetValue(jobId, status) Then
                Return status
            End If
            Return JobStatus.Unknown
        End Function

        ''' <summary>
        ''' Gets metadata for a specific job.
        ''' </summary>
        Public Function GetJobMetadata(jobId As Integer) As JobMetadata
            Dim metadata As JobMetadata = Nothing
            If _jobMetadata.TryGetValue(jobId, metadata) Then
                Return metadata
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Gets the number of active tasks currently running.
        ''' </summary>
        Public Function GetActiveTaskCount() As Integer
            SyncLock _syncLock
                Return _activeTasks.Count
            End SyncLock
        End Function

        ''' <summary>
        ''' Gets the number of queued tasks waiting to run.
        ''' </summary>
        Public Function GetQueuedTaskCount() As Integer
            Return _jobQueue.Count
        End Function

        ''' <summary>
        ''' Processes the job queue by executing tasks up to the concurrent limit.
        ''' </summary>
        Private Async Sub ProcessQueue()
            If _isProcessing Then
                Exit Sub
            End If

            _isProcessing = True

            Try
                Await ProcessQueueAsync()
            Finally
                _isProcessing = False
            End Try
        End Sub

        ''' <summary>
        ''' Asynchronously processes the job queue.
        ''' </summary>
        Private Async Function ProcessQueueAsync() As Task
            Try
                While _jobQueue.Count > 0 OrElse GetActiveTaskCount() > 0
                    ' Start new jobs if under the concurrent limit
                    Dim jobItem As KeyValuePair(Of Integer, IsoCreationTask) = Nothing
                    While GetActiveTaskCount() < _maxConcurrentTasks AndAlso _jobQueue.TryDequeue(jobItem)
                        Dim jobId = jobItem.Key
                        Dim creationTask = jobItem.Value

                        SyncLock _syncLock
                            _activeTasks.Add(jobId, creationTask)
                        End SyncLock

                        _jobStatuses(jobId) = JobStatus.Running
                        RaiseEvent JobStatusChanged(jobId, JobStatus.Running)
                        RaiseEvent JobProgressChanged(jobId, True)

                        Dim unused = Task.Run(Function() ExecuteJobAsync(jobId, creationTask))
                    End While

                    ' Wait a bit before checking again
                    Await Task.Delay(100)
                End While
            Catch ex As Exception
                DynaLog.LogMessage("Error in ProcessQueueAsync: " & ex.Message)
            End Try
        End Function

        ''' <summary>
        ''' Executes a single ISO creation task.
        ''' </summary>
        Private Async Function ExecuteJobAsync(jobId As Integer, task As IsoCreationTask) As Task
            Try
                Dim result = Await task.StartTaskAsync()
                _jobStatuses(jobId) = If(result, JobStatus.Completed, JobStatus.Failed)
            Catch ex As Exception
                _jobStatuses(jobId) = JobStatus.Failed
            Finally
                SyncLock _syncLock
                    _activeTasks.Remove(jobId)
                End SyncLock

                RaiseEvent JobStatusChanged(jobId, _jobStatuses(jobId))
                RaiseEvent JobProgressChanged(jobId, False)

                ' Continue processing if there are more jobs
                If _jobQueue.Count > 0 OrElse GetActiveTaskCount() > 0 Then
                    ProcessQueue()
                End If
            End Try
        End Function

    End Class

    ''' <summary>
    ''' Enumeration of possible job statuses.
    ''' </summary>
    Public Enum JobStatus
        Unknown = 0
        Queued = 1
        Running = 2
        Completed = 3
        Failed = 4
    End Enum

    ''' <summary>
    ''' Stores metadata about an ISO creation job.
    ''' </summary>
    Public Class JobMetadata
        Public Property DestinationIsoFile As String
        Public Property SourceImageFile As String
        Public Property Architecture As IsoArchitecture
    End Class

End Namespace
