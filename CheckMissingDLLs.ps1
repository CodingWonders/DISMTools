$ErrorActionPreference = "Stop"

function Get-NormalizedDirectoryPath {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    $fullPath = [System.IO.Path]::GetFullPath($Path)
    if (-not $fullPath.EndsWith([System.IO.Path]::DirectorySeparatorChar)) {
        $fullPath += [System.IO.Path]::DirectorySeparatorChar
    }
    return $fullPath
}

if ($ghAction -ne "yes") {
    $SolutionDir = "$((Get-Location).Path)\..\.."
    $TargetDir = "$((Get-Location).Path)"
}

if ([string]::IsNullOrWhiteSpace($SolutionDir)) {
    $SolutionDir = (Get-Location).Path
}

if ([string]::IsNullOrWhiteSpace($TargetDir)) {
    $TargetDir = Join-Path $SolutionDir "bin\Debug"
}

$SolutionDir = Get-NormalizedDirectoryPath -Path $SolutionDir
$TargetDir = [System.IO.Path]::GetFullPath($TargetDir)
$projectFile = Join-Path $SolutionDir "DISMTools.vbproj"

if (-not (Test-Path -LiteralPath $projectFile)) {
    throw "DISMTools.vbproj was not found: $projectFile"
}

New-Item -Path $TargetDir -ItemType Directory -Force | Out-Null

[xml]$projectXml = Get-Content -LiteralPath $projectFile
$namespaceManager = New-Object System.Xml.XmlNamespaceManager($projectXml.NameTable)
$namespaceManager.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003")

$hintPathNodes = $projectXml.SelectNodes("//msb:Reference/msb:HintPath", $namespaceManager)
$missingFiles = New-Object System.Collections.Generic.List[string]
$copiedFiles = New-Object System.Collections.Generic.List[string]

foreach ($hintPathNode in $hintPathNodes) {
    $relativeHintPath = $hintPathNode.InnerText.Trim()
    if ([string]::IsNullOrWhiteSpace($relativeHintPath)) {
        continue
    }

    if (-not $relativeHintPath.EndsWith(".dll", [System.StringComparison]::OrdinalIgnoreCase)) {
        continue
    }

    $sourcePath = Join-Path $SolutionDir $relativeHintPath
    if (-not (Test-Path -LiteralPath $sourcePath -PathType Leaf)) {
        $missingFiles.Add($relativeHintPath) | Out-Null
        continue
    }

    $targetPath = Join-Path $TargetDir ([System.IO.Path]::GetFileName($sourcePath))
    if (-not (Test-Path -LiteralPath $targetPath -PathType Leaf)) {
        Copy-Item -LiteralPath $sourcePath -Destination $targetPath -Force
        $copiedFiles.Add([System.IO.Path]::GetFileName($sourcePath)) | Out-Null
    }
}

if ($missingFiles.Count -gt 0) {
    $message = "The following referenced DLL files are missing after NuGet restore:" + [Environment]::NewLine + ($missingFiles -join [Environment]::NewLine)
    throw $message
}

Write-Host "Checked referenced NuGet DLL files. Copied $($copiedFiles.Count) file(s) to $TargetDir"
