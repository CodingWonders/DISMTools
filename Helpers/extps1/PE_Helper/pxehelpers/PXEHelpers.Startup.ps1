#requires -version 5.0
#requires -runasadministrator

using namespace System.Collections.Generic

. "$PSScriptRoot\Common\PXEHelpers.Common.ps1"

$host.UI.RawUI.WindowTitle = "Preboot eXecution Environment Helpers"

class PxeHelperProvider {
    [string]$ProviderName
    [string]$ProviderDescription
    [string]$ProviderVersionCompatibility
    [string]$ProviderPath
    [bool]$ProviderRequiresClientServer
    [string]$ProviderNotes

    PxeHelperProvider($name, $description, $vercompat, $path, $reqsCs, $notes) {
        $this.ProviderName = $name
        $this.ProviderDescription = $description
        $this.ProviderVersionCompatibility = $vercompat
        $this.ProviderPath = "$PSScriptRoot\$path"
        $this.ProviderRequiresClientServer = $reqsCs
        $this.ProviderNotes = $notes
    }
}

$providerList = [List[PxeHelperProvider]]::new()
$providerList.Add([PxeHelperProvider]::new("Windows Deployment Services Helper", "Select this provider to deploy a Windows image using a WDS server.", "0.7+", "wds\wdshelper.ps1", $true, ""))
#$providerList.Add([PxeHelperProvider]::new("FOG Helper", "Select this provider to deploy a Windows image using a FOG server.", "0.7.1+", "fog\foghelper.ps1", $true, "This provider is divided into 2 stages: a Windows stage and a Linux stage."))

function Invoke-PxeProvider {
    param (
        [Parameter(Mandatory = $true, Position = 0)] [int]$index
    )

    if (($index -lt 0) -or ($index -gt $($providerList.Count - 1))) {
        Write-Host "Please write appropriate data !"
    }

    try {
        if (-not (Test-Path "$($providerList[$index].ProviderPath)" -PathType Leaf)) { throw }
        Invoke-Expression "$($providerList[$index].ProviderPath)"
    } catch {
        Write-Host "Could not launch the PXE utility. $_"
    }
}

function Show-PxeProviders {
    $idx = 1
    foreach ($provider in $providerList) {
        $unavail = $false
        $nameString = "$idx. $($provider.ProviderName)"
        if (-not (Test-Path "$($provider.ProviderPath)" -PathType Leaf)) {
            $unavail = $true
            $nameString += " (unavailable)"
        }

        Write-Host "  " -NoNewline
        if ($unavail) {
            Write-Host "$nameString" -BackgroundColor DarkYellow -ForegroundColor Black
        } else {
            Write-Host "$nameString" -BackgroundColor DarkGreen -ForegroundColor White
        }
        Write-Host "     $($provider.ProviderDescription)" -ForegroundColor DarkGray
        Write-Host ""
        Write-Host "     This provider is supported on DISMTools versions $($provider.ProviderVersionCompatibility)"
        Write-Host "     " -NoNewline
        $csReqMessage = ""
        if ($provider.ProviderRequiresClientServer) {
            $csReqMessage = "This provider requires a server component to be launched. You should find the server component of the matching provider. Otherwise, do not use this provider."
        } else {
            $csReqMessage = "This provider does not require a server component to be launched."
        }
        Write-Host "$csReqMessage" -BackgroundColor DarkBlue -ForegroundColor White
        $noteMessage = ""
        if ($provider.ProviderNotes -eq "") {
            $noteMessage = "none"
        } else {
            $noteMessage = $provider.ProviderNotes
        }
        Write-Host "     Notes: $noteMessage -- Make sure that the device can contact the server hosting the deployment solution."
        $idx++
        Write-Host ""
    }
    Write-Host ""
}

$global:product = "Preboot eXecution Environment Helpers"

Show-SectionMessage -sectionTitle "Choose your provider" -sectionDescription "Choose the helper for the PXE provider you use."

if ($providerList.Count -gt 0) {
    Show-PxeProviders
    $validated = $false
    $util = -1
    do {
        $utilStr = Read-Host -Prompt "Choose an utility from the list above and press ENTER"

        try {
            $util = [int]$utilStr
            $validated = $true
        } catch {
            Write-Host "Could not validate your option."
            $validated = $false
        }
    } until ($validated -eq $true)

    Invoke-PxeProvider -index $($util - 1)
} else {
    Write-Host "Could not get a list of providers. The system will reboot when you press ENTER . . ."
    Read-Host | Out-Null
}
wpeutil reboot
