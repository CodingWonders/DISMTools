param (
    [switch]$Debug,
    [switch]$Run,
    [switch]$SkipPreprocessing,
    [string]$Arguments
)

$workingdir = $PSScriptRoot

Push-Location
Set-Location $workingdir

# Variable to sync between runspaces
$sync = [Hashtable]::Synchronized(@{})
$sync.PSScriptRoot = $workingdir
$sync.configs = @{}

function Update-Progress {
    param (
        [Parameter(Mandatory, position=0)]
        [string]$StatusMessage,

        [Parameter(Mandatory, position=1)]
        [ValidateRange(0,100)]
        [int]$Percent,

        [Parameter(position=2)]
        [string]$Activity = "Compiling"
    )

    Write-Progress -Activity $Activity -Status $StatusMessage -PercentComplete $Percent
}

Update-Progress "Pre-req: Running Preprocessor..." 0

# Dot source the 'Invoke-Preprocessing' Function from 'tools/Invoke-Preprocessing.ps1' Script
$preprocessingFilePath = ".\tools\Invoke-Preprocessing.ps1"
. $preprocessingFilePath

$excludedFiles = @("$preprocessingFilePath", '*.png', '*.exe')
$msg = "Pre-req: Code Formatting"
Invoke-Preprocessing -WorkingDir "$workingdir" -ExcludedFiles $excludedFiles -ProgressStatusMessage $msg -ThrowExceptionOnEmptyFilesList -SkipExcludedFilesValidation
