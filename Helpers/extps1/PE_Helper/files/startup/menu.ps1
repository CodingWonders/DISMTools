Write-Host "Choose an option:`n"
Write-Host "  1 - Perform a Local Installation"
Write-Host "      Choose this method if you started the Preinstallation Environment using local media, such as"
Write-Host "      DVD or USB drives. This is recommended for newcomers."
Write-Host "  C - Enter a command line"
Write-Host "  S - Shut down my computer"
Write-Host "  R - Restart my computer`n"
Write-Host "You will not be able to go back to choose another option after making your decision. You must reboot your"
Write-Host "computer and select the correct option. You can also restart your computer by closing this window.`n"
$option = Read-Host -Prompt "Choose an installation method by typing the option and pressing ENTER"
switch ($option) {
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