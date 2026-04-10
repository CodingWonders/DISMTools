param (
    [Parameter(Mandatory = $true, Position = 0)] [string] $appxFullNames
)

$appxFullNamesArray = $appxFullNames.Split(";")
Write-Output "The following AppX packages ($($appxFullNamesArray.Count)) have been scheduled for removal:"
$appxFullNamesArray
Get-AppxPackage -AllUsers | Where-Object { $appxFullNamesArray.Contains($_.PackageFullName) } | Remove-AppxPackage -AllUsers
Write-Output "Log off and log on again for applications to be fully deprovisioned."