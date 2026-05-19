using namespace System.Collections.Generic

$Global:SelectedLayoutCode = ""

$MainForm_Load = {
	$keybLayouts = (Get-ChildItem -Path "HKLM:\SYSTEM\CurrentControlSet\Control\Keyboard Layouts").Name
	$keybLayoutCodes = [List[string]]::new()
	
	foreach ($keybLayout in $keybLayouts) {
		# we have to replace "HKEY_LOCAL_MACHINE" with "HKLM:" for it to be friendly with PWSH
		$regPath = $keybLayout.Replace("HKEY_LOCAL_MACHINE", "HKLM:")
		# GetFileName really works with registry keys and gets the name of the keys out of their paths... unbelievable
		$keybLayoutCode = [IO.Path]::GetFileName("$keybLayout")
		$keybLayoutCodes.Add($keybLayoutCode)
	
		$ListView1.Items.Add([System.Windows.Forms.ListViewItem]::new([string[]]@("$keybLayoutCode", "$(Get-ItemPropertyValue -Path "$regPath" -Name "Layout Text" -ErrorAction Ignore)")))
	}
	
	# Try to get the current layout
	try {
		$preloadValue = Get-ItemPropertyValue -Path "HKCU:\Keyboard Layout\Preload" -Name "1" -ErrorAction Stop
		$currentLayoutCode = Get-ItemPropertyValue -Path "HKCU:\Keyboard Layout\Substitutes" -Name $preloadValue -ErrorAction Stop
		
		$currentLayoutIdx = $keybLayoutCodes.IndexOf($currentLayoutCode)
		if ($currentLayoutIdx -ge 0) {
			$ListView1.Items[$currentLayoutIdx].Selected = $true
			$ListView1.Select()
		} else {
			throw
		}
	} catch {
		try {
			# Try with International key
			$currentLayoutCode = Get-ItemPropertyValue -Path "HKCU:\Control Panel\International" -Name "Locale" -ErrorAction Stop
			$currentLayoutIdx = $keybLayoutCodes.IndexOf($currentLayoutCode)
			if ($currentLayoutIdx -ge 0) {
				$ListView1.Items[$currentLayoutIdx].Selected = $true
				$ListView1.Select()
			}
		} catch {
			# ignore
		}
	}
}

$ListView1_SelectedIndexChanged = {
	$OK_Button.Enabled = $ListView1.SelectedItems.Count -eq 1
	if ($ListView1.SelectedItems.Count -eq 1) {
		$Global:SelectedLayoutCode = $ListView1.FocusedItem.Text
	}
}

$OK_Button_Click = {
	$MainForm.DialogResult = 'OK'
	$MainForm.Close()
}

Add-Type -AssemblyName System.Windows.Forms
. (Join-Path $PSScriptRoot 'changekeyboardlayout.designer.ps1')
if ($MainForm.ShowDialog() -eq 'OK') {
	if (Test-Path -Path "$env:SYSTEMROOT\system32\wpeutil.exe" -PathType Leaf) {
		$newLayout = $Global:SelectedLayoutCode
		
		wpeutil setkeyboardlayout 0409:$newLayout
		
		if ($?) {
			# Reload startnet in a new window
			$setupScript = Get-Content -Path "$env:SYSTEMDRIVE\Windows\system32\startnet.cmd"
			$setupScript[5] = "set debug=0"
			Set-Content -Path "$env:SYSTEMDRIVE\Windows\system32\startnet.cmd" -Value $setupScript -Force
			
			# Let's lie to startnet and claim we have already configured the keyboard layout, since it likes
			# to reconfigure it.
			reg add "HKLM\SOFTWARE\DISMTools\Preinstallation Environment\Policies" /f /v KeyboardLayoutCode /t REG_SZ /d "$newLayout"
			
			Write-Host "Reloading startup sequence..."
			Remove-Item -Path "$env:SYSTEMDRIVE\changekeyb" -Force -ErrorAction SilentlyContinue
		} else {
			Write-Host "Keyboard layout could not be changed..."
		}
	}
}
Start-Process -FilePath "$env:SYSTEMROOT\system32\cmd.exe" -ArgumentList "/K $env:SYSTEMROOT\system32\startnet.cmd" -Wait