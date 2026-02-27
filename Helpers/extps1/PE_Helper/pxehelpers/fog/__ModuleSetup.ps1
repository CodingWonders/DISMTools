$version = "0.8"

Write-Host "DISMTools $version - FOG Module Preparation"
Write-Host "(c) 2025-2026. CodingWonders Software"
Write-Host "-----------------------------------------------------------"

Write-Host "Grabbing latest release from GitHub..."
$moduleRequest = Invoke-WebRequest -UseBasicParsing "https://api.github.com/repos/darksidemilk/FogApi/releases/latest"

if ($moduleRequest.StatusCode -ne 200) {
    Write-Host "Could not grab data from GitHub API. (Error Code: $($moduleRequest.StatusCode): $($moduleRequest.StatusDescription))"
    return
}

$tempDir = [IO.Path]::GetTempPath().TrimEnd("\")

# Status Code 200 == OK
$assetData = $moduleRequest.Content | ConvertFrom-Json
Write-Host "Downloading release $($assetData.name) (tag $($assetData.tag_name))..."
$downloadUri = ($assetData.assets | Where-Object { $_.browser_download_url.Contains("BuiltModule") }).browser_download_url
if ($downloadUri -eq $null) {
    Write-Host "Could not determine download address"
    return
}
Invoke-WebRequest "$downloadUri" -OutFile "$tempDir\fogapi.zip"
if ($? -eq $false) {
    Write-Host "Could not download release file."
    return
}

if (Test-Path "$tempDir\fogapi.zip" -PathType Leaf) {
    Write-Host "Release has been downloaded. Extracting it..."
    Expand-Archive -Path "$tempDir\fogapi.zip" -Destination "$PSScriptRoot" -Force
    if ($?) {
        New-Item -Path "fogready" -Force | Out-Null
        $assetData.tag_name | Out-File "fogready" -Force
    }
    Remove-Item "$tempDir\fogapi.zip" -Force
}
