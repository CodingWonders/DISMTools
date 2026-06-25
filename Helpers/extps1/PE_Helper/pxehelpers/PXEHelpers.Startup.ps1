#requires -version 5.0
#requires -runasadministrator

using namespace System.Collections.Generic

. "$PSScriptRoot\Common\PXEHelpers.Common.ps1"
. "$env:SYSTEMDRIVE\DTPE.PolicyHelper.ps1"

$networkAdapters = $null

function Get-SystemArchitecture
{
    # Detect CPU architecture and compare with list
    switch (((Get-CimInstance -Class Win32_Processor | Where-Object { $_.DeviceID -eq "CPU0" }).Architecture).ToString())
    {
        "0"{
            return "i386"
        }
        "1"{
            return "mips"
        }
        "2"{
            return "alpha"
        }
        "3"{
            return "powerpc"
        }
        "5"{
            return "arm"
        }
        "6"{
            return "ia64"
        }
        "9"{
            return "amd64"
        }
        "12" {
            return "aarch64"
        }
        default {
            return ""
        }
    }
    return ""
}

function Show-InstallNetAdapterScreen {
    param(
        [switch]$firstStartup = $false
    )

    Show-SectionMessage -sectionTitle "Install a network adapter" -sectionDescription "Either no network adapters were detected on your computer or you decided to install a new network adapter."

    Write-Host "  The following network adapters were detected on your computer, excluding kernel debuggers:`n"

    foreach ($networkAdapter in $networkAdapters) {
        Write-Host "    - Device ID $($networkAdapter.DeviceID): $($networkAdapter.Name). Type: $($networkAdapter.AdapterType). Service Name: $($networkAdapter.ServiceName)"
    }

    Write-Host ""
    Write-Host "  To launch the Driver Installation Module to install a new network adapter, type `"DIM`" and press ENTER."

    if ($firstStartup) {
        Write-Host "  To cancel network installation and restart your computer, type `"R`" and press ENTER."
    } else {
        Write-Host "  To go back to the previous screen, type `"X`" and press ENTER."
    }

    Write-Host ""
    $option = Read-Host -Prompt "Choose an option described above and press ENTER"

    if ($option -eq "DIM") {
        # Get CPU architecture and launch Driver Installation Module
        $supportedArchitectures = [List[string]]::new()
        $supportedArchitectures.Add("i386")
        $supportedArchitectures.Add("amd64")
        $supportedArchitectures.Add("aarch64")
        $systemArchitecture = Get-SystemArchitecture

        if ($supportedArchitectures.Contains($systemArchitecture))
        {
            if (Test-Path -Path "$env:SYSTEMDRIVE\Tools\DIM\$systemArchitecture\DT-DIM.exe")
            {
                if ((Get-PolicyValue -PolicyName "DTDimShowPnputilOut" -DefaultPolicyValue 1 -ValidOptions @(0,1)) -eq 1) {
                    $compSys = Get-CimInstance -Query "SELECT Manufacturer, Model FROM Win32_ComputerSystem"
                    $baseBrd = Get-CimInstance -Query "SELECT Product FROM Win32_BaseBoard"

                    $manufacturer = $compSys.Manufacturer
                    $model = $compSys.Model
                    $boardModel = $baseBrd.Product

                    @"
These are the device IDs of the hardware devices that could not be detected. Please
install device drivers based on hardware IDs. After installation, please close this window.

To find the drivers for this specific device, please check the following information:
- Manufacturer/Model: $manufacturer $model
- Motherboard model : $boardModel

"@ | Out-File -FilePath "$env:SYSTEMDRIVE\unknowndevs.txt" -Force
                    pnputil /enum-devices /problem | Out-File -FilePath "$env:SYSTEMDRIVE\unknowndevs.txt" -Force -Append
                    notepad "$env:SYSTEMDRIVE\unknowndevs.txt"
                }
                Clear-Host
                Show-CenteredTextBox -Text "Starting the Driver Installation Module . . ." -MaxWidth 70 -CenterOfAll
                Start-Process -FilePath "$env:SYSTEMDRIVE\Tools\DIM\$systemArchitecture\DT-DIM.exe" -Wait
                Show-CenteredTextBox -Text "Getting Installed Network Adapters . . ." -MaxWidth 70 -CenterOfAll
            }
        }
    } else {
        if ($firstStartup) {
            wpeutil reboot
        }
    }
}

$host.UI.RawUI.WindowTitle = "Preboot eXecution Environment Helpers"
$global:product = "Preboot eXecution Environment Helpers"

Show-CenteredTextBox -Text "Preboot eXecution Environment Helpers. (c) 2025-2026 CodingWonders Software" -MaxWidth 70 -CenterOfAll
Write-Host "`n    Portions (c) CT Tech Group LLC; (c) JJ Fullmer"

Write-Progress -Activity "PXE Helpers starting up..." -Status "Getting network adapters in the system..." -PercentComplete 0
$networkAdapters = Get-CimInstance Win32_NetworkAdapter | Where-Object { $_.ServiceName -ne "kdnic" }    # there's no reason to add a kernel debugger to the network adapters list

if (($networkAdapters | Select-Object -ExpandProperty DeviceID).Count -lt 1) {
    Show-InstallNetAdapterScreen -firstStartup
}

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

Show-CenteredTextBox -Text "Preboot eXecution Environment Helpers. (c) 2025-2026 CodingWonders Software" -MaxWidth 70 -CenterOfAll
Write-Host "`n    Portions (c) CT Tech Group LLC; (c) JJ Fullmer"
Write-Progress -Activity "PXE Helpers starting up..." -Status "Loading PXE Helper providers..." -PercentComplete 50

$providerList = [List[PxeHelperProvider]]::new()
$providerList.Add([PxeHelperProvider]::new("Windows Deployment Services Helper", "Select this provider to deploy a Windows image using a WDS server.", "0.7+", "wds\wdshelper.ps1", $true, ""))
$providerList.Add([PxeHelperProvider]::new("FOG Helper", "Select this provider to deploy a Windows image using a FOG server.", "0.7.1+", "fog\foghelper.ps1", $true, "This provider is divided into 2 stages: a Windows stage and a Linux stage."))

function Invoke-PxeProvider {
    param (
        [Parameter(Mandatory = $true, Position = 0)] [int]$index
    )

    if (($index -lt 0) -or ($index -gt $($providerList.Count - 1))) {
        Write-Host "Please write appropriate data !"
        return
    }

    try {
        if (-not (Test-Path "$($providerList[$index].ProviderPath)" -PathType Leaf)) { throw }
        Invoke-Expression "$($providerList[$index].ProviderPath)"
    } catch {
        Write-Host "Could not launch the PXE utility. $_"
    }
}

Start-Sleep -Milliseconds 500
Write-Progress -Activity "PXE Helpers starting up..." -Status "Initialization Complete" -PercentComplete 100

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
            Write-Host "$nameString" -BackgroundColor DarkGreen -ForegroundColor Black
        }
        Write-Host "     $($provider.ProviderDescription)" -ForegroundColor DarkGray
        Write-Host "     This provider is supported on DISMTools versions $($provider.ProviderVersionCompatibility)"
        Write-Host "     " -NoNewline
        $csReqMessage = ""
        if ($provider.ProviderRequiresClientServer) {
            $csReqMessage = "This provider requires a server component to be launched. You should find the server component of the matching provider in this install disc. Otherwise, do not use this provider."
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

    # Show additional options
    Write-Host "  N. Install a Network Adapter"
    Write-Host "     Choose this option to install a new network adapter on this environment. You will go back to this screen afterwards"
    Write-Host ""
}

Write-Progress -Activity "PXE Helpers starting up..." -Completed
Show-SectionMessage -sectionTitle "Choose your provider" -sectionDescription "Choose the helper for the PXE provider you use."

if ($providerList.Count -gt 0) {
    Show-PxeProviders
    $validated = $false
    $util = -1
    do {
        $utilStr = Read-Host -Prompt "Choose a utility from the list above and press ENTER"
        
        if ($utilStr -eq "") {
            continue
        }

        if ($utilStr -eq "N") {
            Show-InstallNetAdapterScreen

            Show-SectionMessage -sectionTitle "Choose your provider" -sectionDescription "Choose the helper for the PXE provider you use."
            Show-PxeProviders
            $validated = $false
            $util = -1

            continue
        }

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
