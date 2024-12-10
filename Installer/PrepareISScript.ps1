param (
	[Parameter(Mandatory = $true, Position = 0)] [string] $ReleaseID
)

try {
	if ($ReleaseID -ne "") {
		# Get the contents of the ISS and modify them
		$issInstaller = Get-Content -Path "$((Get-Location).Path)\dt.iss"
		
		switch ($ReleaseID) {
			"stable" {
				$issInstaller = $issInstaller.Replace("DISMTools\Preview", "DISMTools\Stable").Trim()
				$issInstaller = $issInstaller.Replace("AppId={{AB033696-A4AC-4DF2-B802-9D8BB8B0EEB5}}", "AppId={{BC1A3BB3-3B0A-4D21-B778-0B21C136C6E0}}").Trim()
				$issInstaller = $issInstaller.Replace("#define scName             `"DISMTools Preview`"", "#define scName             `"DISMTools`"").Trim()
				$issInstaller = $issInstaller.Replace(" Preview`"", "`"").Trim()
			}
			"preview" {
				$issInstaller = $issInstaller.Replace("DISMTools\Stable", "DISMTools\Preview").Trim()
				$issInstaller = $issInstaller.Replace("AppId={{BC1A3BB3-3B0A-4D21-B778-0B21C136C6E0}}", "AppId={{AB033696-A4AC-4DF2-B802-9D8BB8B0EEB5}}").Trim()
				$issInstaller = $issInstaller.Replace("#define scName             `"DISMTools`"", "#define scName             `"DISMTools Preview`"").Trim()
				Write-Host "Please update verConst in Inno Setup script to include `"Preview`"" -BackgroundColor DarkGreen
			}
			default {
				Write-Host "Unknown release ID `"$ReleaseID`". Available IDs: preview, stable"
				return
			}
		}
		
		$issInstaller | Out-File "$((Get-Location).Path)\dt.iss" -Encoding utf8
		
	} else {
		Write-Host "No release ID has been specified. Available IDs: preview, stable"
	}
} catch {
	Write-Host "Could not modify file. Reason: $_"
}