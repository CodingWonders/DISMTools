# DISMTools Build Script
# Visual Studio 2022 Professional - MSBuild

$MSBuild = "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
$Project  = "$PSScriptRoot\DISMTools.vbproj"
$Config   = "Release"
$Platform = "AnyCPU"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  DISMTools Build Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if (-not (Test-Path $MSBuild)) {
    Write-Host "ERROR: MSBuild bulunamadi: $MSBuild" -ForegroundColor Red
    exit 1
}
if (-not (Test-Path $Project)) {
    Write-Host "ERROR: Proje dosyasi bulunamadi: $Project" -ForegroundColor Red
    exit 1
}

Write-Host "MSBuild : $MSBuild" -ForegroundColor Gray
Write-Host "Project : $Project" -ForegroundColor Gray
Write-Host "Config  : $Config | $Platform" -ForegroundColor Gray
Write-Host ""

# NuGet restore (sadece ana proje)
Write-Host "[1/2] NuGet paketleri yukleniyor..." -ForegroundColor Yellow
& "$PSScriptRoot\nuget.exe" restore $Project -PackagesDirectory "$PSScriptRoot\packages"
Write-Host ""

# Derle (sadece DISMTools.vbproj)
Write-Host "[2/2] Derleniyor..." -ForegroundColor Yellow
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
    /verbosity:normal `
    /maxcpucount

$exitCode = $LASTEXITCODE

Write-Host ""
if ($exitCode -eq 0) {
    # exe obj\Release'den bin\Release'e kopyala
    $objExe = "$PSScriptRoot\obj\Release\DISMTools.exe"
    $outExe = "$PSScriptRoot\bin\Release\DISMTools.exe"
    if (Test-Path $objExe) {
        Copy-Item $objExe $outExe -Force
        Write-Host "DISMTools.exe -> bin\Release\" -ForegroundColor Gray
    }
    # settings.ini Language=6 yap
    $settingsPath = "$PSScriptRoot\bin\Release\settings.ini"
    if (Test-Path $settingsPath) {
        (Get-Content $settingsPath -Raw) -replace 'Language=\d+', 'Language=6' | Set-Content $settingsPath -NoNewline
    }
    # docs klasörünü bin\Release'e kopyala (yardım linkleri için)
    $docsSource = "$PSScriptRoot\docs"
    $docsDest   = "$PSScriptRoot\bin\Release\docs"
    if (Test-Path $docsSource) {
        if (Test-Path $docsDest) { Remove-Item $docsDest -Recurse -Force }
        Copy-Item $docsSource $docsDest -Recurse -Force
        Write-Host "docs\ -> bin\Release\docs\" -ForegroundColor Gray
    }
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "  DERLEME BASARILI!" -ForegroundColor Green
    Write-Host "  Cikti: bin\Release\DISMTools.exe" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
} else {
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "  DERLEME BASARISIZ! (Exit: $exitCode)" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
}

exit $exitCode
