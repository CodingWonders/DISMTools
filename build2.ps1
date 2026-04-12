# DISMTools Build2 Script - Tek EXE (ILRepack ile 3rd party DLL'leri embed eder)
# Gereksinim: nuget.exe (proje klasöründe mevcut)

$MSBuild    = "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
$Project    = "$PSScriptRoot\DISMTools.vbproj"
$Config     = "Release"
$Platform   = "AnyCPU"
$OutDir     = "$PSScriptRoot\bin\Release"
$SingleExe  = "$PSScriptRoot\bin\SingleExe\DISMTools.exe"
$ILRepack   = "$PSScriptRoot\packages\ILRepack.2.0.18\tools\ILRepack.exe"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  DISMTools Build2 - Tek EXE" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# --- Ön kontroller ---
if (-not (Test-Path $MSBuild)) {
    Write-Host "HATA: MSBuild bulunamadi: $MSBuild" -ForegroundColor Red; exit 1
}
if (-not (Test-Path $Project)) {
    Write-Host "HATA: Proje dosyasi bulunamadi: $Project" -ForegroundColor Red; exit 1
}

# --- ILRepack indir (yoksa) ---
if (-not (Test-Path $ILRepack)) {
    Write-Host "[0/3] ILRepack indiriliyor..." -ForegroundColor Yellow
    & "$PSScriptRoot\nuget.exe" install ILRepack -Version 2.0.18 -OutputDirectory "$PSScriptRoot\packages" -NonInteractive
    if (-not (Test-Path $ILRepack)) {
        Write-Host "HATA: ILRepack indirilemedi." -ForegroundColor Red; exit 1
    }
    Write-Host "ILRepack hazir." -ForegroundColor Green
}

# --- NuGet restore ---
Write-Host "[1/3] NuGet paketleri yukleniyor..." -ForegroundColor Yellow
& "$PSScriptRoot\nuget.exe" restore $Project -PackagesDirectory "$PSScriptRoot\packages"
Write-Host ""

# --- Derleme ---
Write-Host "[2/3] Derleniyor..." -ForegroundColor Yellow
Write-Host ""

& $MSBuild $Project `
    /p:Configuration=$Config `
    /p:Platform=$Platform `
    /p:ISPREVIEW=No `
    /p:GEN_INSTALLER=No `
    /p:COPY_DOCS=No `
    /p:CREATE_SAMPLE_USERDATA=No `
    /p:PostBuildEvent="" `
    /p:PreBuildEvent="" `
    /verbosity:minimal `
    /maxcpucount

$exitCode = $LASTEXITCODE
if ($exitCode -ne 0) {
    Write-Host "DERLEME BASARISIZ! (Exit: $exitCode)" -ForegroundColor Red; exit $exitCode
}

# settings.ini Language=6
$settingsPath = "$OutDir\settings.ini"
if (Test-Path $settingsPath) {
    (Get-Content $settingsPath -Raw) -replace 'Language=\d+', 'Language=6' | Set-Content $settingsPath -NoNewline
}

# docs kopyala
$docsSource = "$PSScriptRoot\docs"
$docsDest   = "$OutDir\docs"
if (Test-Path $docsSource) {
    if (Test-Path $docsDest) { Remove-Item $docsDest -Recurse -Force }
    Copy-Item $docsSource $docsDest -Recurse -Force
}

Write-Host ""
Write-Host "[3/3] Tek EXE olusturuluyor (ILRepack)..." -ForegroundColor Yellow

# Cikti klasoru
$singleDir = Split-Path $SingleExe
if (-not (Test-Path $singleDir)) { New-Item -ItemType Directory -Path $singleDir | Out-Null }

# Embed edilecek 3rd party DLL'ler (GAC/sistem DLL'leri haric)
$thirdPartyDlls = @(
    "DarkUI.dll",
    "INIFileParser.dll",
    "Markdig.dll",
    "Microsoft.Dism.dll",
    "Microsoft.WindowsAPICodePack.Core.dll",
    "Microsoft.WindowsAPICodePack.dll",
    "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.dll",
    "Microsoft.WindowsAPICodePack.Sensors.dll",
    "Microsoft.WindowsAPICodePack.Shell.dll",
    "Microsoft.WindowsAPICodePack.ShellExtensions.dll",
    "Scintilla.NET.dll",
    "System.Buffers.dll",
    "System.Memory.dll",
    "System.Numerics.Vectors.dll",
    "System.Runtime.CompilerServices.Unsafe.dll"
)

$mainExe = "$OutDir\DISMTools.exe"
$dllArgs = @()
foreach ($dll in $thirdPartyDlls) {
    $dllPath = Join-Path $OutDir $dll
    if (Test-Path $dllPath) {
        $dllArgs += $dllPath
    } else {
        Write-Host "  Uyari: $dll bulunamadi, atlanıyor." -ForegroundColor Yellow
    }
}

Write-Host "  Ana EXE: $mainExe" -ForegroundColor Gray
Write-Host "  Embed edilecek DLL sayisi: $($dllArgs.Count)" -ForegroundColor Gray
Write-Host "  Cikti: $SingleExe" -ForegroundColor Gray
Write-Host ""

# ILRepack calistir
$ilArgs = @(
    "/out:$SingleExe",
    "/target:winexe",
    "/targetplatform:v4,C:\Windows\Microsoft.NET\Framework64\v4.0.30319",
    "/wildcards",
    "/xmldocs",
    $mainExe
) + $dllArgs

& $ILRepack @ilArgs

$ilExit = $LASTEXITCODE

Write-Host ""
if ($ilExit -eq 0 -and (Test-Path $SingleExe)) {
    $sizeMB = [math]::Round((Get-Item $SingleExe).Length / 1MB, 2)
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "  TEK EXE OLUSTURULDU!" -ForegroundColor Green
    Write-Host "  Cikti : bin\SingleExe\DISMTools.exe" -ForegroundColor Green
    Write-Host "  Boyut : $sizeMB MB" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green

    # Gerekli dosyalari SingleExe klasorune kopyala (docs, helpers, settings vb.)
    $copyItems = @("docs", "Helpers", "Resources", "settings.ini", "portable", "LICENSE", "DISMTools.exe.config", "runtimes")
    foreach ($item in $copyItems) {
        $src = Join-Path $OutDir $item
        $dst = Join-Path $singleDir $item
        if (Test-Path $src) {
            if ((Get-Item $src).PSIsContainer) {
                if (Test-Path $dst) { Remove-Item $dst -Recurse -Force }
                Copy-Item $src $dst -Recurse -Force
            } else {
                Copy-Item $src $dst -Force
            }
            Write-Host "  Kopyalandi: $item" -ForegroundColor Gray
        }
    }
    Write-Host ""
    Write-Host "  Klasor: bin\SingleExe\" -ForegroundColor Cyan
} else {
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "  TEK EXE OLUSTURULAMADI! (Exit: $ilExit)" -ForegroundColor Red
    Write-Host "  Normal derleme basarili, bin\Release\ kullanilabilir." -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Red
}

exit $ilExit
