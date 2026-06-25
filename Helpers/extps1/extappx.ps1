# DISMTools 0.8 - Extended AppX package getter for online installations

param (
	[Parameter(Position = 0)] [string] $noNonRemovable = "false",
	[Parameter(Position = 1)] [string] $noFramework = "false"
)

Import-Module Appx

$appxPackages = Get-AppxPackage -AllUsers

if ([System.Environment]::OSVersion.Version.Major -ge 10) {
	if ($noNonRemovable -eq "true") { $appxPackages = $appxPackages | Where-Object { $_.NonRemovable -eq $false } }
	if ($noFramework -eq "true") { $appxPackages = $appxPackages | Where-Object { $_.IsFramework -eq $false } }
}

$appxPackages | Select-Object Name,PackageFullName,Architecture,ResourceID,Version | ConvertTo-Xml -As String | Out-Host