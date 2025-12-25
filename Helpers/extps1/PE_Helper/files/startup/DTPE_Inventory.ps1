# Perform System Inventory

Write-Host "Getting computer information... This can take some time."
Write-Progress -Activity "Collecting inventory..." -Status "Getting System information..."
$compInfo = Get-ComputerInfo
Write-Progress -Activity "Collecting inventory..." -Status "Getting DT PE information..."
$dtpe_MinVersion = Get-ItemPropertyValue -Path "HKLM:\Software\DISMTools\Preinstallation Environment" -Name "MinBuild"
$dtpe_FullVersion = Get-ItemPropertyValue -Path "HKLM:\Software\DISMTools\Preinstallation Environment" -Name "FullBuild"
Write-Progress -Activity "Collecting inventory..." -Completed

# Show this information
Write-Host "Device Inventory" -ForegroundColor White -BackgroundColor DarkGreen
Write-Output "---------------------------------------------------------"
Write-Output "|          DEVICE INVENTORY COLLECTION RESULTS          |"
Write-Output "---------------------------------------------------------`n"
Write-Output "  DISMTools $dtpe_MinVersion Preinstallation Environment ($($env:FIRMWARE_TYPE))"
Write-Output "  Version $dtpe_FullVersion"
Write-Output "  WinPE Build $($compInfo.WindowsBuildLabEx) -- $($compInfo.WindowsSystemRoot)`n"
Write-Output "  --- System Hardware:`n"
Write-Output "  System: $($compInfo.CsManufacturer)"
Write-Output "  Processor: $($compInfo.CsProcessors[0].Name)"
Write-Output "  Memory: $(((Get-CimInstance Win32_PhysicalMemory) | Measure-Object -Property Capacity -Sum).Sum / 1MB) MB"
Write-Output "  NTFS Volumes:"
Get-Volume | Where-Object { $_.DriveType -eq "Fixed" -and $_.FileSystemType -eq "NTFS" -and $_.DriveLetter -ne $null } | Foreach-Object {
	Write-Output "    - Drive $($_.DriveLetter) (`"$($_.FileSystemLabel)`"). Size: $([Math]::Round($_.Size / 1073748124, 2)) GB (percentage remaining: $([Math]::Round(($_.SizeRemaining / $_.Size) * 100, 2))%)"
}
Write-Output "  Network Adapters:"
$networkAdapters = Get-CimInstance Win32_NetworkAdapter | Where-Object { $_.ServiceName -ne "kdnic" }
foreach ($networkAdapter in $networkAdapters) {
	Write-Output "    - Device ID $($networkAdapter.DeviceID): $($networkAdapter.Name). Type: $($networkAdapter.AdapterType). Service Name: $($networkAdapter.ServiceName)"
}
Write-Output ""
Write-Output "  --- System Software:`n"
Write-Output "  Operating system: $($compInfo.OsName) (NT Version: $($compInfo.OsVersion). Extended Build String: $($compInfo.WindowsBuildLabEx))"
Write-Output "  Build Type: $($compInfo.OsBuildType)"
Write-Output "  Installed HotFixes:"
$compInfo.OsHotFixes | Foreach-Object {
	Write-Output "    - $($_.HotFixID): $($_.Description). Installed on: $($_.InstalledOn)"
}
Write-Output "  OS Suites: $($compInfo.OsSuites -join ', ')"
Write-Output "  Logon Server: $($compInfo.LogonServer)"
Write-Output "  Environment Variables:"
Get-ChildItem "ENV:" | Foreach-Object {
	Write-Output "    - $($_.Name): $($_.Value)"
}
Write-Output "  Service Pack Version: $($compInfo.OsServicePackMajorVersion).$($compInfo.OsServicePackMinorVersion)"
Write-Output "  Architecture: $($compInfo.OsArchitecture)`n"
Write-Host "This information can be saved to a file if you redirect the output of this script to a file."