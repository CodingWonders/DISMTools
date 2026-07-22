$ErrorActionPreference = "Stop"

function Fail([string]$Message) {
    throw $Message
}

$root = Resolve-Path (Join-Path $PSScriptRoot "..\..")
$sourceLanguageDir = Join-Path $root "language"
$buildLanguageDir = Join-Path $root "bin\Debug\language"
$installerPayloadLanguageDir = Join-Path $root "Installer\files\language"
$installerLanguageDir = Join-Path $root "Installer\Languages"
$installerScriptPath = Join-Path $root "Installer\dt.iss"
$installerRootDefaultMessagesPath = Join-Path $root "Installer\Default.isl"
$compilerDefaultMessagesPath = Join-Path $root "Installer\Compiler\Default.isl"
$installerOutputPath = Join-Path $root "Installer\Output\dt_setup.exe"

$requiredMainFiles = @("en-US.ini", "de-DE.ini", "es-ES.ini", "fr-FR.ini", "pt-PT.ini", "it-IT.ini")
$requiredMainMarkers = @(
    "[Designer.Main]",
    "[Designer.ExceptionForm]",
    "[Main.News]",
    "[Main.News.Load]",
    "[Main.News.Error]",
    "[Main.ExternalTools]",
    "[Main.ReportManager]",
    "[Main.SaveProjectAs]",
    "[MountedImgMgr]",
    "NewProject.Button=",
    "Retry.Button=",
    "NoDetails.Message="
)

foreach ($dir in @($buildLanguageDir, $installerPayloadLanguageDir)) {
    if (-not (Test-Path -LiteralPath $dir)) {
        Fail "Localization directory is missing: $dir"
    }

    foreach ($fileName in $requiredMainFiles) {
        $filePath = Join-Path $dir $fileName
        if (-not (Test-Path -LiteralPath $filePath)) {
            Fail "Localization file is missing: $filePath"
        }
    }

    $englishFile = Join-Path $dir "en-US.ini"
    $englishText = Get-Content -LiteralPath $englishFile -Raw -Encoding UTF8
    foreach ($marker in $requiredMainMarkers) {
        if (-not $englishText.Contains($marker)) {
            Fail "Required localization marker '$marker' was not found in $englishFile"
        }
    }

    Write-Host "Main localization payload verified: $dir"
}

foreach ($fileName in $requiredMainFiles) {
    $sourcePath = Join-Path $sourceLanguageDir $fileName
    $buildPath = Join-Path $buildLanguageDir $fileName
    $payloadPath = Join-Path $installerPayloadLanguageDir $fileName

    foreach ($path in @($sourcePath, $buildPath, $payloadPath)) {
        if (-not (Test-Path -LiteralPath $path)) {
            Fail "Localization file is missing: $path"
        }
    }

    $sourceHash = (Get-FileHash -LiteralPath $sourcePath -Algorithm SHA256).Hash
    $buildHash = (Get-FileHash -LiteralPath $buildPath -Algorithm SHA256).Hash
    $payloadHash = (Get-FileHash -LiteralPath $payloadPath -Algorithm SHA256).Hash
    if ($sourceHash -ne $buildHash -or $sourceHash -ne $payloadHash) {
        Fail "Localization file '$fileName' differs between source, build output, and installer payload."
    }
}

$mainExecutableName = "DISMTools.exe"
$buildExecutablePath = Join-Path (Split-Path $buildLanguageDir -Parent) $mainExecutableName
$payloadExecutablePath = Join-Path (Split-Path $installerPayloadLanguageDir -Parent) $mainExecutableName
foreach ($path in @($buildExecutablePath, $payloadExecutablePath)) {
    if (-not (Test-Path -LiteralPath $path)) {
        Fail "Main executable is missing: $path"
    }
}

$buildExecutableHash = (Get-FileHash -LiteralPath $buildExecutablePath -Algorithm SHA256).Hash
$payloadExecutableHash = (Get-FileHash -LiteralPath $payloadExecutablePath -Algorithm SHA256).Hash
if ($buildExecutableHash -ne $payloadExecutableHash) {
    Fail "DISMTools.exe differs between build output and installer payload."
}

$mountedManagerSourcePath = Join-Path $root "Panels\Img_Ops\MountedImgMgr.vb"
$mainFormSourcePath = Join-Path $root "MainForm.vb"
$windowHelperSourcePath = Join-Path $root "Utilities\WindowHelper.vb"
$fileAssociationHelperSourcePath = Join-Path $root "Utilities\FileAssociations\FileAssociationHelper.vb"
$newProjectSourcePath = Join-Path $root "Panels\Prj_Ops\NewProj.vb"
$optionsSourcePath = Join-Path $root "Panels\Exe_Ops\Options.vb"
$mountedManagerSource = Get-Content -LiteralPath $mountedManagerSourcePath -Raw -Encoding UTF8
$mainFormSource = Get-Content -LiteralPath $mainFormSourcePath -Raw -Encoding UTF8
$windowHelperSource = Get-Content -LiteralPath $windowHelperSourcePath -Raw -Encoding UTF8
$fileAssociationHelperSource = Get-Content -LiteralPath $fileAssociationHelperSourcePath -Raw -Encoding UTF8
$newProjectSource = Get-Content -LiteralPath $newProjectSourcePath -Raw -Encoding UTF8
$optionsSource = Get-Content -LiteralPath $optionsSourcePath -Raw -Encoding UTF8

if ($mountedManagerSource.Contains("ListView1.Columns(5).Text")) {
    Fail "Mounted Image Manager still accesses the non-existent column at index 5."
}
if (-not $mainFormSource.Contains("Handles ReportManagerToolStripMenuItem.Click, ManageReportsToolStripMenuItem.Click")) {
    Fail "Report Manager click handlers are missing."
}
if (-not $mainFormSource.Contains("Handles LinkLabel14.LinkClicked")) {
    Fail "The project-side image mount link click handler is missing."
}
if (-not $mainFormSource.Contains("Handles SaveProjectasToolStripMenuItem.Click")) {
    Fail "The Save Project As click handler is missing."
}
if (-not $mainFormSource.Contains("Save Project As menu command received.")) {
    Fail "The Save Project As click handler is not logged."
}
if (-not $mainFormSource.Contains("Handles MyBase.Shown")) {
    Fail "The main window does not request foreground activation from its Shown event."
}
if (-not $mainFormSource.Contains("WindowHelper.RequestForegroundWindow(Handle)")) {
    Fail "The main window does not request Win32 foreground activation after it is shown."
}
if (-not $mainFormSource.Contains("WindowHelper.RaiseWindowAndRestoreNormalZOrder(Handle, normalZOrderRestored)")) {
    Fail "The main window does not include a one-time foreground fallback."
}
if (-not $mainFormSource.Contains("main window is foreground:") -or
    -not $mainFormSource.Contains("main window remains topmost:")) {
    Fail "The foreground activation result is not fully logged."
}
if (-not $windowHelperSource.Contains("SetForegroundWindow") -or
    -not $windowHelperSource.Contains("GetForegroundWindow") -or
    -not $windowHelperSource.Contains("SetWindowPos") -or
    -not $windowHelperSource.Contains("HWND_NOTOPMOST")) {
    Fail "The foreground-window Win32 helper is missing."
}
if ($mainFormSource -match 'TopMost\s*=\s*True') {
    Fail "The main window must not be made permanently topmost."
}
if (-not $newProjectSource.Contains('LocalizationService.ForSection("Main.SaveProjectAs")("Save.Button")')) {
    Fail "The Save Project As dialog does not use its dedicated localized Save button text."
}
if (-not $mainFormSource.Contains("Handles RefreshViewTSB.Click")) {
    Fail "The project tree refresh button click handler is missing."
}
if (-not $mainFormSource.Contains("SwitchImageIndexesToolStripMenuItem1.Click, SwitchImageIndexesToolStripMenuItem.Click")) {
    Fail "One or more Switch Image Indexes menu items are not connected."
}
if ($optionsSource.Contains('{0}{1}{0} {0}/load={0}%1{0}{0}')) {
    Fail "Options still registers a malformed .dtproj open command."
}
if (-not $optionsSource.Contains('{0}{1}{0} /load={0}%1{0}')) {
    Fail "Options does not register the expected .dtproj open command."
}
if ($optionsSource.Contains('"StarterScript.exe"')) {
    Fail "Options still registers the obsolete StarterScript.exe path."
}
if (-not $optionsSource.Contains("LoadFileAssociationState()")) {
    Fail "Options does not restore the current file association and icon state."
}
if (-not $optionsSource.Contains("CheckBox11.Checked = DTProjAssocCB.Checked")) {
    Fail "Enabling the project association does not enable its icon by default."
}
if (-not $optionsSource.Contains("Executable path registered for association:")) {
    Fail "Options does not validate the executable path parsed from an association command."
}
if (-not $fileAssociationHelperSource.Contains("Registry.CurrentUser.CreateSubKey(UserClassesRegistryPath)")) {
    Fail "Portable file associations are not registered per user."
}
if (-not $fileAssociationHelperSource.Contains('CreateSubKey("DefaultIcon")')) {
    Fail "The project icon registry key is not created when required."
}
if ($fileAssociationHelperSource.Contains('OpenSubKey("DefaultIcon", True)')) {
    Fail "The file association helper still assumes that the icon registry key already exists."
}

if (-not (Test-Path -LiteralPath $installerScriptPath)) {
    Fail "Installer script was not found: $installerScriptPath"
}

$installerScriptText = Get-Content -LiteralPath $installerScriptPath -Raw -Encoding UTF8
if ($installerScriptText -match '#define\s+pfDir\s+"\{commonpf\}\\DISMTools\\Stable"') {
    Fail "Stable installer must install to C:\Program Files\DISMTools, not C:\Program Files\DISMTools\Stable."
}

$installerUsesPreviewName = $installerScriptText -match '#define\s+scName\s+"DISMTools Preview"'
if ($installerUsesPreviewName) {
    if ($installerScriptText -notmatch '#define\s+pfDir\s+"\{commonpf\}\\DISMTools\\Preview"') {
        Fail "Preview installer must install to C:\Program Files\DISMTools\Preview."
    }
} else {
    if ($installerScriptText -notmatch '#define\s+pfDir\s+"\{commonpf\}\\DISMTools"') {
        Fail "Stable installer must install to C:\Program Files\DISMTools."
    }
}

if ($installerScriptText -match "compiler:Default\.isl") {
    Fail "Installer script must not use compiler:Default.isl. English must be loaded from Installer\Languages\English.isl."
}

if ($installerScriptText -match "(?m)^\[Messages\]\s*$") {
    Fail "Installer\dt.iss must not contain a [Messages] section. Installer messages must come from the selected .isl file."
}

if (Test-Path -LiteralPath $installerRootDefaultMessagesPath) {
    Fail "Installer\Default.isl must not exist in the installer root. The compiler runtime Default.isl belongs in Installer\Compiler."
}

if (-not (Test-Path -LiteralPath $compilerDefaultMessagesPath)) {
    Fail "Inno Setup compiler runtime Default.isl is missing: $compilerDefaultMessagesPath"
}

$compilerDefaultText = Get-Content -LiteralPath $compilerDefaultMessagesPath -Raw -Encoding UTF8
if (-not $compilerDefaultText.Contains("[Messages]")) {
    Fail "Compiler runtime Default.isl does not contain [Messages]: $compilerDefaultMessagesPath"
}

if ($compilerDefaultText.Contains("[CustomMessages]")) {
    Fail "Compiler runtime Default.isl must not contain project CustomMessages. Edit project translations in Installer\Languages instead."
}

$requiredInstallerFiles = @(
    "English.isl",
    "Spanish.isl",
    "French.isl",
    "German.isl",
    "Italian.isl",
    "Portuguese.isl"
)

$requiredInstallerSections = @("[LangOptions]", "[Messages]", "[CustomMessages]")
$requiredInstallerMessageKeys = @(
    "ButtonNext=",
    "ButtonBack=",
    "ButtonCancel=",
    "ButtonInstall=",
    "ButtonBrowse=",
    "WelcomeLabel1=",
    "SelectDirDesc=",
    "ReadyLabel1=",
    "FinishedHeadingLabel="
)

$customMessageKeys = [regex]::Matches($installerScriptText, "\{cm:([^},]+)") | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique

foreach ($fileName in $requiredInstallerFiles) {
    $filePath = Join-Path $installerLanguageDir $fileName
    if (-not (Test-Path -LiteralPath $filePath)) {
        Fail "Installer language file is missing: $filePath"
    }

    $text = Get-Content -LiteralPath $filePath -Raw -Encoding UTF8

    foreach ($section in $requiredInstallerSections) {
        if (-not $text.Contains($section)) {
            Fail "Installer language file '$filePath' does not contain required section $section"
        }
    }

    foreach ($key in $requiredInstallerMessageKeys) {
        if (-not $text.Contains($key)) {
            Fail "Installer language file '$filePath' does not contain required Inno Setup message key $key"
        }
    }

    foreach ($key in $customMessageKeys) {
        if (-not $text.Contains("$key=")) {
            Fail "Installer language file '$filePath' does not contain required CustomMessages key $key"
        }
    }

    Write-Host "Installer language file verified: $filePath"
}

if (-not (Test-Path -LiteralPath $installerOutputPath)) {
    Fail "Installer output file was not found after build: $installerOutputPath"
}

Get-Item -LiteralPath $installerOutputPath | Select-Object FullName, Length, LastWriteTime | Format-List | Out-String | Write-Host
Write-Host "Installer localization and output verified successfully."
