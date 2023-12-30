# DISMTools 0.4.1 - Extended AppX package getter for online installations

Import-Module Appx

(New-Item -Path "bin\extps1\out" -ItemType Directory) | Out-Null

# Detect additional AppX packages installed on this system
(Get-AppxPackage | Select-Object -ExpandProperty Name) | Out-File "bin\extps1\out\appxpkgnames"
(Get-AppxPackage | Select-Object -ExpandProperty PackageFullName) | Out-File "bin\extps1\out\appxpkgfullnames"
(Get-AppxPackage | Select-Object -ExpandProperty Architecture) | Out-File "bin\extps1\out\appxarch"
(Get-AppxPackage | Select-Object -ExpandProperty ResourceID) | Out-File "bin\extps1\out\appxresid"
(Get-AppxPackage | Select-Object -ExpandProperty Version) | Out-File "bin\extps1\out\appxver"
if ([System.Environment]::OSVersion.Version.Major -ge 10) { (Get-AppxPackage | Select-Object -ExpandProperty NonRemovable) | Out-File "bin\extps1\out\appxnonrempolicy" } else { Write-Output "Cannot get non-removable policy" }
if ([System.Environment]::OSVersion.Version.Major -ge 10) { (Get-AppxPackage | Select-Object -ExpandProperty IsFramework) | Out-File "bin\extps1\out\appxframework" } else { Write-Output "Cannot get framework property" }