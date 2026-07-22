param (
    [Parameter(Mandatory = $true, Position = 0)] [string] $ReleaseID
)

$ErrorActionPreference = "Stop"

try {
    if ([string]::IsNullOrWhiteSpace($ReleaseID)) {
        Write-Host "No release ID has been specified. Available IDs: preview, stable"
        return
    }

    $scriptPath = Join-Path (Get-Location).Path "dt.iss"
    $issInstaller = Get-Content -LiteralPath $scriptPath -Raw -Encoding UTF8

    switch ($ReleaseID) {
        "stable" {
            $issInstaller = $issInstaller -replace '#define pfDir\s+"\{commonpf\}\\DISMTools\\Preview"', '#define pfDir              "{commonpf}\DISMTools"'
            $issInstaller = $issInstaller.Replace("AppId={{AB033696-A4AC-4DF2-B802-9D8BB8B0EEB5}}", "AppId={{BC1A3BB3-3B0A-4D21-B778-0B21C136C6E0}}")
            $issInstaller = $issInstaller.Replace("#define scName             `"DISMTools Preview`"", "#define scName             `"DISMTools`"")
            $issInstaller = $issInstaller.Replace(" Preview`"", "`"")
        }
        "preview" {
            $issInstaller = $issInstaller -replace '#define pfDir\s+"\{commonpf\}\\DISMTools"', '#define pfDir              "{commonpf}\DISMTools\Preview"'
            $issInstaller = $issInstaller.Replace("AppId={{BC1A3BB3-3B0A-4D21-B778-0B21C136C6E0}}", "AppId={{AB033696-A4AC-4DF2-B802-9D8BB8B0EEB5}}")
            $issInstaller = $issInstaller.Replace("#define scName             `"DISMTools`"", "#define scName             `"DISMTools Preview`"")
            if ($issInstaller -notmatch 'Preview') {
                Write-Host "Please update verConst in Inno Setup script to include `"Preview`"" -BackgroundColor DarkGreen
            }
        }
        default {
            Write-Host "Unknown release ID `"$ReleaseID`". Available IDs: preview, stable"
            return
        }
    }

    $issInstaller | Out-File -LiteralPath $scriptPath -Encoding utf8
} catch {
    Write-Host "Could not modify file. Reason: $_"
    throw
}
