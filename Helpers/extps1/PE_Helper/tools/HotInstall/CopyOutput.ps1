if (Test-Path "$((Get-Location).Path)\out") {
	Remove-Item -Path "$((Get-Location).Path)\out" -Recurse -Force
}

New-Item -Path "$((Get-Location).Path)\out" -ItemType Directory -Force | Out-Null
Copy-Item -Path "$((Get-Location).Path)\Installer\bin\Debug\*" -Destination "$((Get-Location).Path)\out" -Recurse -Force -Verbose

if ($?) {
	Copy-Item -Path "$((Get-Location).Path)\Installer\logo.ico" -Destination "$((Get-Location).Path)\out\autorun.ico" -Recurse -Force -Verbose
	Move-Item -Path "$((Get-Location).Path)\out\Installer.exe" -Destination "$((Get-Location).Path)\out\setup.exe"
	Move-Item -Path "$((Get-Location).Path)\out\Installer.exe.config" -Destination "$((Get-Location).Path)\out\setup.exe.config"
	Move-Item -Path "$((Get-Location).Path)\out\Installer.xml" -Destination "$((Get-Location).Path)\out\setup.xml"
	Remove-Item -Path "$((Get-Location).Path)\out\*.pdb" -Recurse -Force -Verbose
	Remove-Item -Path "$((Get-Location).Path)\out\*.vshost.exe*" -Recurse -Force -Verbose
	Remove-Item -Path "$((Get-Location).Path)\out\sources" -Recurse -Force -Verbose
	
	$infContents = @'

[autorun]
open=setup.exe
icon=autorun.ico
'@

	$infContents | Out-File -FilePath "$((Get-Location).Path)\out\autorun.inf" -Force -Encoding utf8
}