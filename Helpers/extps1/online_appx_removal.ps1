param (
    [Parameter(Mandatory = $true, Position = 0)] [string[]] $appxFullNames
)

Get-AppxPackage -AllUsers | Where-Object { $appxFullNames.Contains($_.PackageFullName) } | Remove-AppxPackage -AllUsers
Write-Host "Log off and log on again for applications to be fully deprovisioned."
Start-Sleep -Seconds 5