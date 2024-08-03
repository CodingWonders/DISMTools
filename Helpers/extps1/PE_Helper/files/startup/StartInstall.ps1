# Reset variable
$setupScript = Get-Content -Path "\Windows\system32\startnet.cmd"
$setupScript[5] = "set debug=0"
Set-Content -Path "\Windows\system32\startnet.cmd" -Value $setupScript -Force
Clear-Host
\Windows\system32\startnet.cmd