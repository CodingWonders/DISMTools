Write-Host "Choose your preferred installation method:`n"
Write-Host "  1 - Local Installation"
Write-Host "      Choose this method if you started the Preinstallation Environment using local media, such as"
Write-Host "      DVD or USB drives. This is recommended for newcomers."
Write-Host "  2 - Network Installation"
Write-Host "      Choose this method if you started the Preinstallation Environment using a network-based"
Write-Host "      deployment solution. This is recommended for system administrators that want to deploy a system"
Write-Host "      image to multiple computers at once."
Write-Host "      Note that WDS is only supported at the moment." -ForegroundColor White
Write-Host "  C - Command-Line"
Write-Host "  S - Shut down my computer"
Write-Host "  R - Restart my computer`n"
Write-Host "You will not be able to go back to choose another option after making your decision. You must reboot your"
Write-Host "computer and select the correct option. You can also restart your computer by closing this window.`n"
$option = Read-Host -Prompt "Choose an installation method by typing the option and pressing ENTER"
switch ($option) {
	"2" {
		Clear-Host
		Write-Host "Welcome to the Windows Deployment Services Technology Preview.`n"
		Write-Host "The WDS Technology Preview 1 allows you to perform basic operating system deployment using a Windows Deployment"
		Write-Host "Services server. The WDS Helper takes care of the installation process.`n"
		Write-Host "The WDS Helper follows a client-server architecture. You are about to launch the client component, and it's required"
		Write-Host "that you start the server component beforehand. You can find it in the `"pxehelpers\wds`" folder of"
		Write-Host "this DVD/USB drive, which you can eject now. The required installation components are already loaded into memory.`n"
		Write-Host "Since this is a technology preview, only basic testing has been made in order to make sure that you have a smooth"
		Write-Host "experience. Expect to find issues, so don't hesitate to report feedback on this technology to make it better in"
		Write-Host "future releases of DISMTools. If you didn't want to select this option, restart your computer.`n"
		Write-Host "Once again, thanks for trying the WDS Technology Preview.`n`n"
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