# Reset variable
$setupScript = Get-Content -Path "$env:SYSTEMDRIVE\Windows\system32\startnet.cmd"
$setupScript[5] = "set debug=0"
Set-Content -Path "$env:SYSTEMDRIVE\Windows\system32\startnet.cmd" -Value $setupScript -Force
Clear-Host
Set-Location "$env:SYSTEMDRIVE\"
iex "$env:SYSTEMDRIVE\Windows\system32\startnet.cmd"