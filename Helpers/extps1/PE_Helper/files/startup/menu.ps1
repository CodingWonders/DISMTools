Write-Host "Choose an option:`n"
Write-Host "  1 - Perform a Local Installation"
Write-Host "      Choose this method if you started the Preinstallation Environment using local media, such as"
Write-Host "      DVD or USB drives. This is recommended for newcomers."
Write-Host "  2 - Perform a Network Installation"
Write-Host "      Choose this method if you started the Preinstallation Environment using a network-based"
Write-Host "      deployment solution. This is recommended for system administrators that want to deploy a system"
Write-Host "      image to multiple computers at once."
Write-Host "      Note that WDS is only supported at the moment." -ForegroundColor White
Write-Host "  C - Enter a command line"
Write-Host "  S - Shut down my computer"
Write-Host "  R - Restart my computer`n"
Write-Host "You will not be able to go back to choose another option after making your decision. You must reboot your"
Write-Host "computer and select the correct option. You can also restart your computer by closing this window.`n"
$option = Read-Host -Prompt "Choose an installation method by typing the option and pressing ENTER"
switch ($option) {
	"2" {
		Clear-Host
		Write-Host "Preview Release Notice:`n"
		Write-Host "Thanks for trying out network-based deployment with Windows Deployment Services and the DISMTools Preinstallation"
		Write-Host "Environment.`n"
		Write-Host "The WDS Helper follows a client-server architecture. You are about to launch the client component, and it's required"
		Write-Host "that you start the server component beforehand. You can find it in the `"pxehelpers\wds`" folder of"
		Write-Host "this DVD/USB drive, which you can eject now. The required installation components are already loaded into memory.`n"
		Write-Host "Expect to find issues, so don't hesitate to report feedback on this technology to make it better in"
		Write-Host "future releases of DISMTools. If you didn't want to select this option, restart your computer.`n`n"
		Write-Host "                                  - CodingWonders Software`n`n"
		Write-Host "Press ENTER to start the WDS Helper..."
		Read-Host | Out-Null
		New-Item -Path "$env:SYSTEMDRIVE\netinstall" -ErrorAction SilentlyContinue | Out-Null
	}
	"C" {
		New-Item -Path "$env:SYSTEMDRIVE\cmdcons" -ErrorAction SilentlyContinue | Out-Null
	}
	"S" {
		Start-Process -FilePath "$env:WINDIR\system32\wpeutil.exe" -ArgumentList "shutdown" -NoNewWindow -Wait
	}
	"R" {
		Start-Process -FilePath "$env:WINDIR\system32\wpeutil.exe" -ArgumentList "reboot" -NoNewWindow -Wait
	}
}