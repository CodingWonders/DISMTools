param(
    [Parameter(Mandatory = $true)]
    [string]$InstallerPath,

    [Parameter(Mandatory = $true)]
    [string]$PortableSourcePath,

    [Parameter(Mandatory = $true)]
    [string]$BranchName,

    [Parameter(Mandatory = $true)]
    [string]$RunNumber,

    [Parameter(Mandatory = $true)]
    [string]$RunAttempt,

    [Parameter(Mandatory = $true)]
    [string]$Sha,

    [Parameter(Mandatory = $false)]
    [ValidateSet("stable", "preview")]
    [string]$ReleaseChannel = "stable",

    [Parameter(Mandatory = $false)]
    [string]$AssetOutputDirectory = "",

    [Parameter(Mandatory = $false)]
    [switch]$Prerelease,

    [Parameter(Mandatory = $false)]
    [switch]$Draft
)

$ErrorActionPreference = "Stop"

function Fail($Message) {
    throw $Message
}

function Get-RepositoryRoot {
    $root = (& git rev-parse --show-toplevel 2>$null)
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($root)) {
        return (Get-Location).Path
    }
    return $root.Trim()
}

function Get-AssemblyVersionInfo($RepositoryRoot) {
    $assemblyInfoPath = Join-Path $RepositoryRoot "My Project\AssemblyInfo.vb"
    if (-not (Test-Path -LiteralPath $assemblyInfoPath)) {
        Fail "AssemblyInfo.vb was not found: $assemblyInfoPath"
    }

    $assemblyInfo = Get-Content -LiteralPath $assemblyInfoPath -Raw
    $versionMatch = [regex]::Match($assemblyInfo, 'AssemblyFileVersion\("(?<version>[^"\)]+)"\)')
    if (-not $versionMatch.Success) {
        $versionMatch = [regex]::Match($assemblyInfo, 'AssemblyVersion\("(?<version>[^"\)]+)"\)')
    }
    if (-not $versionMatch.Success) {
        Fail "Could not find AssemblyFileVersion or AssemblyVersion in AssemblyInfo.vb."
    }

    $fullVersion = $versionMatch.Groups["version"].Value.Trim()
    $parts = $fullVersion.Split('.')
    if ($parts.Count -ge 2) {
        $publicVersion = "$($parts[0]).$($parts[1])"
    } else {
        $publicVersion = $fullVersion
    }

    return [pscustomobject]@{
        FullVersion = $fullVersion
        PublicVersion = $publicVersion
    }
}

function Get-ReleaseDateStamp {
    $utcNow = (Get-Date).ToUniversalTime()
    try {
        $timeZone = [System.TimeZoneInfo]::FindSystemTimeZoneById("FLE Standard Time")
        $releaseDate = [System.TimeZoneInfo]::ConvertTimeFromUtc($utcNow, $timeZone)
    } catch {
        $releaseDate = $utcNow
    }

    return [pscustomobject]@{
        DateStamp = $releaseDate.ToString("yyMMdd")
        LongDate = $releaseDate.ToString("yyyy-MM-dd HH:mm")
        ZoneLabel = if ($releaseDate -eq $utcNow) { "UTC" } else { "FLE" }
    }
}

function New-CleanName($Value) {
    $clean = $Value -replace '[^A-Za-z0-9._-]', '-'
    $clean = $clean.Trim('-')
    if ([string]::IsNullOrWhiteSpace($clean)) {
        return "release"
    }
    return $clean
}

function New-PortableArchive($SourcePath, $DestinationPath) {
    if (Test-Path -LiteralPath $DestinationPath) {
        Remove-Item -LiteralPath $DestinationPath -Force
    }

    $stagingRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("dismtools-portable-" + [guid]::NewGuid().ToString("N"))
    New-Item -ItemType Directory -Path $stagingRoot -Force | Out-Null

    try {
        foreach ($item in Get-ChildItem -LiteralPath $SourcePath -Force) {
            if ($item.Name -ieq "settings.ini") {
                Write-Host "Skipping default settings.ini for portable first run."
                continue
            }

            Copy-Item -LiteralPath $item.FullName -Destination $stagingRoot -Recurse -Force
        }

        $portableMarkerPath = Join-Path $stagingRoot "portable"
        if (-not (Test-Path -LiteralPath $portableMarkerPath)) {
            New-Item -ItemType File -Path $portableMarkerPath -Force | Out-Null
        }
        Set-ItemProperty -LiteralPath $portableMarkerPath -Name Attributes -Value ([System.IO.FileAttributes]::Normal)

        $settingsPath = Join-Path $stagingRoot "settings.ini"
        if (Test-Path -LiteralPath $settingsPath) {
            Remove-Item -LiteralPath $settingsPath -Force
        }

        $sourceItems = Join-Path $stagingRoot "*"
        Compress-Archive -Path $sourceItems -DestinationPath $DestinationPath -CompressionLevel Optimal -Force
    } finally {
        if (Test-Path -LiteralPath $stagingRoot) {
            Remove-Item -LiteralPath $stagingRoot -Recurse -Force
        }
    }
}

function Test-PortableArchive($ArchivePath) {
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zip = [System.IO.Compression.ZipFile]::OpenRead($ArchivePath)
    try {
        $entryNames = @($zip.Entries | ForEach-Object { $_.FullName -replace '\\', '/' })
        if ($entryNames -contains "settings.ini") {
            Fail "Portable archive contains settings.ini. First run setup would be skipped."
        }
        if ($entryNames -notcontains "portable") {
            Fail "Portable archive does not contain the portable marker file."
        }
    } finally {
        $zip.Dispose()
    }
}

function Get-ShortSha($CommitSha) {
    if ([string]::IsNullOrWhiteSpace($CommitSha)) {
        return "unknown"
    }
    if ($CommitSha.Length -le 7) {
        return $CommitSha
    }
    return $CommitSha.Substring(0, 7)
}

function Get-ChangeList($CommitSha) {
    $changes = @()
    $previousTag = (& git describe --tags --abbrev=0 "$CommitSha^" 2>$null)
    if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($previousTag)) {
        $range = "$($previousTag.Trim())..$CommitSha"
        $changes = (& git log --pretty=format:"* %s in %h" $range 2>$null)
    }

    if ($changes.Count -eq 0) {
        $changes = (& git log --pretty=format:"* %s in %h" -n 20 $CommitSha 2>$null)
    }

    if ($LASTEXITCODE -ne 0 -or $changes.Count -eq 0) {
        return "* No commit list was generated for this build."
    }

    return ($changes -join [Environment]::NewLine)
}

function New-ReleaseNotes($Context) {
    $hashRows = @()
    foreach ($asset in $Context.Assets) {
        $hashRows += "| $($asset.Kind) | $($asset.Name) | $($asset.Hash) |"
    }

    $changes = Get-ChangeList $Context.CommitSha
    $shortSha = Get-ShortSha $Context.CommitSha
    $preReleaseText = if ($Context.IsPrerelease) { "Yes" } else { "No" }
    $draftText = if ($Context.IsDraft) { "Yes" } else { "No" }

    $notes = @"
DISMTools $($Context.PublicVersion) is now available as a $($Context.ReleaseChannel) build, with new artifacts attached to this GitHub Release.

## File hashes

| File | Name | Hash SHA256 |
| --- | --- | --- |
$($hashRows -join [Environment]::NewLine)

> [!IMPORTANT]
> If Windows Defender or SmartScreen reports this build as suspicious, check the SHA256 hashes above and download only from this GitHub Release. Unsigned Windows desktop tools can be flagged more often because code signing certificates are expensive.

## Overall changes

This release was generated by the GitHub Actions release pipeline. It creates a new GitHub Release for every run instead of updating an old tag or replacing old assets.

## Update details

| Setting | New status | Reason |
| --- | --- | --- |
| Product version | $($Context.FullVersion) | Version read from AssemblyInfo.vb |
| Release channel | $($Context.ReleaseChannel) | Selected by workflow input or branch name |
| Release tag | $($Context.TagName) | Unique tag generated for this run |
| Target commit | $shortSha | Source commit used for the build |
| Workflow run | $($Context.RunNumber) | Keeps releases unique on the same day |
| Workflow attempt | $($Context.RunAttempt) | Added to the tag when the run is retried |
| Release date | $($Context.ReleaseDate) | Date stamp used in the release tag |
| Prerelease | $preReleaseText | Controlled by channel or workflow input |
| Draft | $draftText | Controlled by workflow input |

## Assets

* Installer: `dt_setup.exe`
* Portable package: `DISMTools.zip`

## What's changed

$changes
"@

    return $notes.Trim()
}
if ([string]::IsNullOrWhiteSpace($env:GH_TOKEN) -and -not [string]::IsNullOrWhiteSpace($env:GITHUB_TOKEN)) {
    $env:GH_TOKEN = $env:GITHUB_TOKEN
}

if ([string]::IsNullOrWhiteSpace($env:GH_TOKEN)) {
    Fail "GH_TOKEN is empty. The workflow must pass github.token to this script."
}

if ([string]::IsNullOrWhiteSpace($env:GITHUB_REPOSITORY)) {
    Fail "GITHUB_REPOSITORY is empty. This script must run inside GitHub Actions."
}

if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Fail "GitHub CLI was not found on PATH."
}

if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Fail "Git was not found on PATH."
}

if (-not (Test-Path -LiteralPath $InstallerPath)) {
    Fail "Installer file was not found: $InstallerPath"
}

if (-not (Test-Path -LiteralPath $PortableSourcePath)) {
    Fail "Portable source directory was not found: $PortableSourcePath"
}

$repositoryRoot = Get-RepositoryRoot
$versionInfo = Get-AssemblyVersionInfo $repositoryRoot
$releaseDateInfo = Get-ReleaseDateStamp
$cleanRunNumber = New-CleanName $RunNumber
$cleanRunAttempt = New-CleanName $RunAttempt
$attemptSuffix = ""
if ($cleanRunAttempt -ne "1") {
    $attemptSuffix = ".$cleanRunAttempt"
}

$channelSuffix = ""
if ($ReleaseChannel -eq "preview") {
    $channelSuffix = "_preview"
}

$tagName = "v$($versionInfo.PublicVersion)$($channelSuffix)_$($releaseDateInfo.DateStamp).$cleanRunNumber$attemptSuffix"
$releaseTitle = $tagName

if ([string]::IsNullOrWhiteSpace($AssetOutputDirectory)) {
    if (-not [string]::IsNullOrWhiteSpace($env:RUNNER_TEMP)) {
        $AssetOutputDirectory = Join-Path $env:RUNNER_TEMP "dismtools-release-assets"
    } else {
        $AssetOutputDirectory = Join-Path $repositoryRoot "artifacts\release"
    }
}

if (Test-Path -LiteralPath $AssetOutputDirectory) {
    Remove-Item -LiteralPath $AssetOutputDirectory -Recurse -Force
}
New-Item -ItemType Directory -Path $AssetOutputDirectory -Force | Out-Null

$installerAssetPath = Join-Path $AssetOutputDirectory "dt_setup.exe"
$portableAssetPath = Join-Path $AssetOutputDirectory "DISMTools.zip"
$hashFilePath = Join-Path $AssetOutputDirectory "SHA256SUMS.txt"
$notesFilePath = Join-Path $AssetOutputDirectory "RELEASE_NOTES.md"

Copy-Item -LiteralPath $InstallerPath -Destination $installerAssetPath -Force
New-PortableArchive -SourcePath $PortableSourcePath -DestinationPath $portableAssetPath
Test-PortableArchive -ArchivePath $portableAssetPath

$assetList = @(
    [pscustomobject]@{ Kind = "Installer"; Name = "dt_setup.exe"; Path = $installerAssetPath },
    [pscustomobject]@{ Kind = "Portable"; Name = "DISMTools.zip"; Path = $portableAssetPath }
)

$hashLines = @()
$assetsWithHashes = @()
foreach ($asset in $assetList) {
    $hash = (Get-FileHash -LiteralPath $asset.Path -Algorithm SHA256).Hash.ToUpperInvariant()
    $hashLines += "$hash  $($asset.Name)"
    $assetsWithHashes += [pscustomobject]@{
        Kind = $asset.Kind
        Name = $asset.Name
        Path = $asset.Path
        Hash = $hash
    }
}
$hashLines | Out-File -LiteralPath $hashFilePath -Encoding utf8

$releaseContext = [pscustomobject]@{
    PublicVersion = $versionInfo.PublicVersion
    FullVersion = $versionInfo.FullVersion
    ReleaseChannel = $ReleaseChannel
    TagName = $tagName
    BranchName = $BranchName
    RunNumber = $RunNumber
    RunAttempt = $RunAttempt
    CommitSha = $Sha
    ReleaseDate = "$($releaseDateInfo.LongDate) $($releaseDateInfo.ZoneLabel)"
    IsPrerelease = [bool]$Prerelease
    IsDraft = [bool]$Draft
    Assets = $assetsWithHashes
}

New-ReleaseNotes $releaseContext | Out-File -LiteralPath $notesFilePath -Encoding utf8

Write-Host "Preparing a new GitHub Release"
Write-Host "Repository: $env:GITHUB_REPOSITORY"
Write-Host "Release tag: $tagName"
Write-Host "Release title: $releaseTitle"
Write-Host "Asset directory: $AssetOutputDirectory"

& git config user.name "github-actions[bot]"
& git config user.email "41898282+github-actions[bot]@users.noreply.github.com"
& git fetch --tags origin

$existingTag = (& git tag -l $tagName)
if (-not [string]::IsNullOrWhiteSpace($existingTag)) {
    Fail "Tag $tagName already exists. Refusing to overwrite an existing release tag."
}

& gh release view $tagName --repo $env:GITHUB_REPOSITORY *> $null
if ($LASTEXITCODE -eq 0) {
    Fail "Release $tagName already exists. Refusing to overwrite an existing GitHub Release."
}

$releaseArguments = @(
    "release",
    "create",
    $tagName,
    $installerAssetPath,
    $portableAssetPath,
    "--repo",
    $env:GITHUB_REPOSITORY,
    "--target",
    $Sha,
    "--title",
    $releaseTitle,
    "--notes-file",
    $notesFilePath
)

if ($Prerelease) {
    $releaseArguments += "--prerelease"
}

if ($Draft) {
    $releaseArguments += "--draft"
}

& gh @releaseArguments
if ($LASTEXITCODE -ne 0) {
    Fail "Failed to create GitHub Release $tagName"
}

Write-Host "GitHub Release has been created: $tagName"
Write-Host "Release assets:"
foreach ($asset in $assetsWithHashes) {
    Write-Host "  $($asset.Name)  $($asset.Hash)"
}
Write-Host "  SHA256SUMS.txt  saved to workflow artifact only"
