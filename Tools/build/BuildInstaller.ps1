param(
    [string]$BuildOutputPath = "",
    [string]$InstallerDirectory = "",
    [switch]$CleanFilesDirectory
)

$ErrorActionPreference = "Stop"

function Fail([string]$Message) {
    throw $Message
}

$repositoryRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..")

if ([string]::IsNullOrWhiteSpace($InstallerDirectory)) {
    $InstallerDirectory = Join-Path $repositoryRoot "Installer"
}

if ([string]::IsNullOrWhiteSpace($BuildOutputPath)) {
    $BuildOutputPath = Join-Path $repositoryRoot "bin\Debug"
}

$installerDirectoryResolved = Resolve-Path -LiteralPath $InstallerDirectory
$installerDirectory = $installerDirectoryResolved.Path

if (-not (Test-Path -LiteralPath $BuildOutputPath)) {
    Fail "Build output directory was not found: $BuildOutputPath"
}

$scriptPath = Join-Path $installerDirectory "dt.iss"
if (-not (Test-Path -LiteralPath $scriptPath)) {
    Fail "Inno Setup script was not found: $scriptPath"
}

$compilerDirectory = Join-Path $installerDirectory "Compiler"
$compilerCandidates = @(
    (Join-Path $compilerDirectory "ISCC.exe"),
    (Join-Path $compilerDirectory "iscc.exe")
)

$compilerPath = $compilerCandidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
if ([string]::IsNullOrWhiteSpace($compilerPath)) {
    Fail "Inno Setup compiler was not found in: $compilerDirectory"
}

$compilerDefaultMessagesPath = Join-Path $compilerDirectory "Default.isl"
if (-not (Test-Path -LiteralPath $compilerDefaultMessagesPath)) {
    Fail "Inno Setup compiler runtime message file was not found: $compilerDefaultMessagesPath"
}

$languageDirectory = Join-Path $installerDirectory "Languages"
if (-not (Test-Path -LiteralPath $languageDirectory)) {
    Fail "Installer language directory was not found: $languageDirectory"
}

$englishLanguagePath = Join-Path $languageDirectory "English.isl"

$requiredInstallerLanguages = @(
    "English.isl",
    "Spanish.isl",
    "French.isl",
    "German.isl",
    "Italian.isl",
    "Portuguese.isl"
)

foreach ($languageFile in $requiredInstallerLanguages) {
    $languagePath = Join-Path $languageDirectory $languageFile
    if (-not (Test-Path -LiteralPath $languagePath)) {
        Fail "Installer language file was not found: $languagePath"
    }
}

if (-not (Test-Path -LiteralPath $englishLanguagePath)) {
    Fail "Installer English language file was not found: $englishLanguagePath"
}

$filesDirectory = Join-Path $installerDirectory "files"
if ($CleanFilesDirectory -and (Test-Path -LiteralPath $filesDirectory)) {
    Remove-Item -LiteralPath $filesDirectory -Recurse -Force
}

if (-not (Test-Path -LiteralPath $filesDirectory)) {
    New-Item -ItemType Directory -Path $filesDirectory -Force | Out-Null
}

Write-Host "Copying program files to installer payload..."
Copy-Item -Path (Join-Path $BuildOutputPath "*") -Destination $filesDirectory -Recurse -Force

Get-ChildItem -LiteralPath $filesDirectory -Filter "VS*.tmp" -Force -ErrorAction SilentlyContinue | Remove-Item -Force -ErrorAction SilentlyContinue

$outputDirectory = Join-Path $installerDirectory "Output"
$outputPath = Join-Path $outputDirectory "dt_setup.exe"

if (Test-Path -LiteralPath $outputPath) {
    Remove-Item -LiteralPath $outputPath -Force
}

Write-Host "Building installer with Inno Setup..."
Write-Host "Compiler: $compilerPath"
Write-Host "Compiler default messages: $compilerDefaultMessagesPath"
Write-Host "Script:   $scriptPath"
Write-Host "Output:   $outputPath"

Push-Location $installerDirectory
try {
    & $compilerPath "dt.iss"
    $exitCode = $LASTEXITCODE
} finally {
    Pop-Location
}

if ($exitCode -ne 0) {
    Fail "Inno Setup compiler failed with exit code $exitCode."
}

if (-not (Test-Path -LiteralPath $outputPath)) {
    Write-Host "Installer output was not found. Current Installer\Output contents:"
    if (Test-Path -LiteralPath $outputDirectory) {
        Get-ChildItem -LiteralPath $outputDirectory -Force | Format-Table FullName, Length, LastWriteTime -AutoSize | Out-String | Write-Host
    } else {
        Write-Host "Output directory does not exist: $outputDirectory"
    }
    Fail "Installer output file was not created: $outputPath"
}

Get-Item -LiteralPath $outputPath | Select-Object FullName, Length, LastWriteTime | Format-List | Out-String | Write-Host
Write-Host "Installer build completed successfully."
