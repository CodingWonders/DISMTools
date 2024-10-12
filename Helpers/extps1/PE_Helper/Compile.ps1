# This is a heavily modified compilation script for the Preinstallation Environment Helper, originally from the Windows Utility (WinUtil)
#
# Original license (affects both this script and the pre-processor):
# 
# MIT License
# 
# Copyright (c) 2022 CT Tech Group LLC
# 
# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:
# 
# The above copyright notice and this permission notice shall be included in all
# copies or substantial portions of the Software.
# 
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
# SOFTWARE.

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

$excludedFiles = @('.\tools\DIM\', "$preprocessingFilePath", '*.png', '*.exe')
$msg = "Pre-req: Code Formatting"
Invoke-Preprocessing -WorkingDir "$workingdir" -ExcludedFiles $excludedFiles -ProgressStatusMessage $msg -ThrowExceptionOnEmptyFilesList