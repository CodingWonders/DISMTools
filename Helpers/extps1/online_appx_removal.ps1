param (
    [Parameter(Mandatory = $true, Position = 0)] [string] $appxFullNames
)

$appxFullNamesArray = $appxFullNames.Split(";")
Write-Host "Proceeding with the removal of $($appxFullNamesArray.Count) package(s)..."
Get-AppxPackage -AllUsers | Where-Object { $appxFullNamesArray.Contains($_.PackageFullName) } | Remove-AppxPackage -AllUsers
Write-Host "Log off and log on again for applications to be fully deprovisioned."
Start-Sleep -Seconds 5