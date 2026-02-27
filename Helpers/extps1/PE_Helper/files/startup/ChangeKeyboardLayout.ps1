using namespace System.Collections.Generic

Write-Host "Change the Preinstallation Environment keyboard layout`n"

$keybLayouts = (Get-ChildItem -Path "HKLM:\SYSTEM\CurrentControlSet\Control\Keyboard Layouts").Name
$keybLayoutCodes = [List[string]]::new()

foreach ($keybLayout in $keybLayouts) {
	# we have to replace "HKEY_LOCAL_MACHINE" with "HKLM:" for it to be friendly with PWSH
	$regPath = $keybLayout.Replace("HKEY_LOCAL_MACHINE", "HKLM:")
	# GetFileName really works with registry keys and gets the name of the keys out of their paths... unbelievable
	$keybLayoutCode = [IO.Path]::GetFileName("$keybLayout")
	
	Write-Host "Keyboard: $(Get-ItemPropertyValue -Path "$regPath" -Name "Layout Text" -ErrorAction Ignore). Code: $keybLayoutCode"
	$keybLayoutCodes.Add($keybLayoutCode)
}

$validatedLayout = $false

do {
	$newLayout = Read-Host -Prompt "Please enter the code of the new keyboard layout to use and press ENTER"
	
	if ($keybLayoutCodes.Contains($newLayout)) {
		$validatedLayout = $true
	} else {
		Write-Host "Invalid keyboard code. Try again..."
	}
} until ($validatedLayout -eq $true)

Write-Host "Setting keyboard layout with code 0409:$newLayout..."
wpeutil setkeyboardlayout 0409:$newLayout

if ($?) {
	# Reload startnet in a new window
	$setupScript = Get-Content -Path "$env:SYSTEMDRIVE\Windows\system32\startnet.cmd"
	$setupScript[5] = "set debug=0"
	Set-Content -Path "$env:SYSTEMDRIVE\Windows\system32\startnet.cmd" -Value $setupScript -Force
	
	Write-Host "Reloading startup sequence..."
	Remove-Item -Path "$env:SYSTEMDRIVE\changekeyb" -Force -ErrorAction SilentlyContinue
	Start-Process -FilePath "$env:SYSTEMROOT\system32\cmd.exe" -ArgumentList "/K $env:SYSTEMROOT\system32\startnet.cmd" -Wait
} else {
	Write-Host "Keyboard layout could not be changed..."
}