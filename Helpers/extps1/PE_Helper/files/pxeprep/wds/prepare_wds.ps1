#requires -version 5.0
#                                              ....
#                                         .'^""""""^.
#      '^`'.                            '^"""""""^.
#     .^"""""`'                       .^"""""""^.                ---------------------------------------------------------
#      .^""""""`                      ^"""""""`                  | DISMTools 0.7                                         |
#       ."""""""^.                   `""""""""'           `,`    | The connected place for Windows system administration |
#         '`""""""`.                 """""""""^         `,,,"    ---------------------------------------------------------
#            '^"""""`.               ^""""""""""'.   .`,,,,,^    | PE Helper - Windows Deployment Services Preparation   |
#              .^"""""`.            ."""""""",,,,,,,,,,,,,,,.    ---------------------------------------------------------
#                .^"""""^.        .`",,"""",,,,,,,,,,,,,,,,'     | (C) 2025 CodingWonders Software                       |
#                  .^"""""^.    '`^^"",:,,,,,,,,,,,,,,,,,".      ---------------------------------------------------------
#                    .^"""""^.`+]>,^^"",,:,,,,,,,,,,,,,`.
#                      .^""";_]]]?)}:^^""",,,`'````'..
#                        .;-]]]?(xxxx}:^^^^'
#                       `+]]]?(xxxxxxxr},'
#                     .`:+]?)xxxxxxxxxxxr<.
#                   .`^^^^:(xxxxxxxxxxxxxxr>.
#                 .`^^^^^^^^I(xxxxxxxxxxxxxxr<.
#               .`^^^^^^^^^^^^I(xxxxxxxxxxxxxxr<.
#             .`^^^^^^^^^^^^^^^'`[xxxxxxxxxxxxxxr<.
#           .`^^^^^^^^^^^^^^^'    `}xxxxxxxxxxxxxxr<.
#          `^^":ll:"^^^^^^^'        `}xxxxxxxxxxxxxxr,
#         '^^^I-??]l^^^^^'            `[xxxxxxxxxxxxxx.          This script is provided AS IS, without any warranty. It shouldn't
#         '^^^,<??~,^^^'                `{xxxxxxxxxxxx.          do any damage to your computer, but you still need to be careful over
#          `^^^^^^^^^'                    `{xxxxxxxxr,           what you do with it.
#           .'`^^^`'                        `i1jrt[:.
using namespace System.Collections.Generic

<#
    .PARAMETER bootImagePath
        The path to the boot image
#>
param (
    [Parameter(Mandatory = $true, Position = 0)] [string] $bootImagePath
)

$version = "0.7"

if (([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator) -eq $false)
{
    Write-Host "You need to run this script as an administrator"
    return $false
}

if ([Environment]::Is64BitOperatingSystem) {
    $progFilesPath = "Program Files (x86)"
} else {
    $progFilesPath = "Program Files"
}

$adkToolsPath = "$env:SystemDrive\$progFilesPath\Windows Kits\10\Assessment and Deployment Kit"
$winpeToolsPath = "$adkToolsPath\Windows Preinstallation Environment"

if (-not (Test-Path "$winpeToolsPath")) {
    Write-Host "Either the ADK is not installed, or the Windows PE addon is not installed. Please install them from the Internet."
    return $false
}

function Update-WinPEForWds {
    <#
        .SYNOPSIS
            Prepares a Windows Preinstallation Environment image for the Windows Deployment Services
        .PARAMETER bootImage
            The path to the boot image
        .NOTES
            This will replace the PE Helper as the operating system installer and will reinstate Windows Setup
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $bootImage
    )
    if (-not (Get-PxeOptionStatus)) {
        Write-Host "PXE option checkers have failed."
        return $false
    }
    Write-Host "Creating temporary mount directory..."
    try
    {
        $mountDirectory = "$env:TEMP\DISMTools_PE_Scratch_$((Get-Date).ToString("MM-dd-yyyy_HH-mm-ss"))_$(Get-Random -Maximum 10000)"
        New-Item "$mountDirectory" -ItemType Directory | Out-Null
    }
    catch
    {
        Write-Host "Could not create temporary mount directory."
        return $false
    }
    Write-Host "Mounting Windows image. Please wait..."
    if (-not (Start-DismCommand -Verb Mount -ImagePath "$bootImage" -ImageIndex 1 -MountPath "$mountDirectory")) {
        Write-Host "The image could not be mounted."
        return $false
    }
    Write-Host "Image mounted. Performing customizations for WDS..."
    try {
        Set-Content -Path "$mountDirectory\Windows\system32\startnet.cmd" -Value "wpeinit" -Force -Encoding UTF8
    } catch {
        Write-Host "Could not modify startup script..."
        Start-DismCommand -Verb Unmount -ImagePath "$mountDirectory" -Commit $false -ErrorAction SilentlyContinue
    }
    Write-Host "Adding packages..."
    if (-not (Add-WindowsSetupPackages -mountPath "$mountDirectory")) {
        Start-DismCommand -Verb Unmount -ImagePath "$mountDirectory" -Commit $false -ErrorAction SilentlyContinue
    }
    Write-Host "Saving changes..."
    if (Start-DismCommand -Verb Unmount -ImagePath "$mountDirectory" -Commit $true) {
        Remove-Item -Path "$mountDirectory" -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "The image has been prepared successfully."
        return $true
    } else {
        Write-Host "The image could not be saved."
        return $false
    }
}

function Add-WindowsSetupPackages {
    <#
        .SYNOPSIS
            Adds mandatory Windows Setup packages
        .PARAMETER mountPath
            The path to the mounted Windows PE image
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $mountPath
    )
    $pkgs = [List[string]]::new()
    $pkgs.Add("$winpeToolsPath\amd64\WinPE_OCs\WinPE-Setup.cab")
    $pkgs.Add("$winpeToolsPath\amd64\WinPE_OCs\en-US\WinPE-Setup_en-us.cab")
    $pkgs.Add("$winpeToolsPath\amd64\WinPE_OCs\WinPE-Setup-Client.cab")
    $pkgs.Add("$winpeToolsPath\amd64\WinPE_OCs\en-US\WinPE-Setup-Client_en-us.cab")

    $pkgCount = $pkgs.Count
    $curPkgIndex = 0

    try {
        foreach ($pkg in $pkgs) {
            $curPkgIndex = $pkgs.IndexOf($pkg)
            Write-Progress -Activity "Adding OS packages..." -Status "Adding OS package $($curPkgIndex + 1) of $($pkgCount): `"$([IO.Path]::GetFileNameWithoutExtension("$pkg"))`"..." -PercentComplete (($curPkgIndex / $pkgCount) * 100)
            if (Test-Path -Path "$pkg" -PathType Leaf) {
                Start-DismCommand -Verb Add-Package -ImagePath "$mountPath" -PackagePath "$pkg" | Out-Null
            }
        }
        Write-Progress -Activity "Adding OS packages..." -Completed
        return $true
    } catch {
        return $false
    }
}

function Get-PxeOptionStatus {
    <#
        .SYNOPSIS
            Determines whether the system has the necessary components for PXE (Preboot eXecution Environment)
        .NOTES
            Required components:
            - Windows Server
            - DHCP
    #>

    if ((Get-ComputerInfo).WindowsInstallationType -ne "Server") {
        Write-Host "This computer is not running Windows Server."
        return $false
    }

    # First, we check if we're running this on Windows Server; we need the ServerManager module for this
    Write-Host "Checking server roles..."
    if ((Get-Module -Name "ServerManager" -ListAvailable | Select-Object -ExpandProperty Name).Count -gt 0) {
        # Next, we check if DHCP server role is enabled on the server
        if ((Get-WindowsFeature -Name "DHCP").InstallState -eq "Installed") {
            # Then, we see if there are scopes
            Write-Host "Getting DHCPv4 scopes..."
            if ((Get-DhcpServerv4Scope | Select-Object -ExpandProperty Name).Count -gt 0) {
                return (Get-DhcpScopeOptionsForPxe -dhcpVersion ([DhcpVersion]::DHCPv4))
            }
            # No DHCPv4 scopes were set. Maybe they are DHCPv6?
            Write-Host "Getting DHCPv6 scopes..."
            if ((Get-DhcpServerv6Scope | Select-Object -ExpandProperty Name).Count -gt 0) {
                return (Get-DhcpScopeOptionsForPxe -dhcpVersion ([DhcpVersion]::DHCPv6))
            }
            # No scopes were set
            Write-Host "There are no DHCPv4 and DHCPv6 scopes defined in the server. Please add scopes to allow communication from clients to this server."
            return $false
        } else {
            Write-Host "The DHCP server role is not installed. Install it via Server Manager."
            return $false
        }
    } else {
        Write-Host "No roles are available. Maybe you're trying to run this script on a platform other than Windows Server."
        return $false
    }
}

enum DhcpVersion {
    <#
        .SYNOPSIS
            Protocol versions of the Dynamic Host Configuration Protocol (DHCP)
        .DESCRIPTION
            Based on the stack that is used for IP addresses, a specific version suits best
    #>
    DHCPv4    # DHCPv4 is designed for IPv4 addresses
    DHCPv6    # DHCPv6 is designed for IPv6 addresses
}

function Get-DhcpScopeOptionsForPxe {
    <#
        .SYNOPSIS
            Checks if the DHCP scopes support PXE (DHCP Option 60)
        .PARAMETER dhcpVersion
            The version of the DHCP protocol
        .OUTPUTS
            Whether there is Option 60 for a given subset of the DHCP server
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [DhcpVersion] $dhcpVersion
    )
    $hasOption60 = $false
    switch ($dhcpVersion) {
        "$([DhcpVersion]::DHCPv4)" {
            Write-Host "Checking server options for IPv4/DHCPv4..."
            # Start with scopes and finish with global server options if not set on scopes
            $v4Scopes = Get-DhcpServerv4Scope
            foreach ($scope in $v4Scopes) {
                $hasOption60 = ((Get-DhcpServerv4OptionValue -ScopeId $scope.ScopeId -All | Where-Object { $_.OptionId -eq 60 } | Select-Object -ExpandProperty Name).Count -gt 0)
                if ($hasOption60) {
                    # Stop
                    break
                }
            }
            if ($hasOption60 -eq $false) {
                $hasOption60 = ((Get-DhcpServerv4OptionValue | Where-Object { $_.OptionId -eq 60 } | Select-Object -ExpandProperty Name).Count -gt 0)
            }
            break
        }
        "$([DhcpVersion]::DHCPv6)" {
            Write-Host "Checking server options for IPv6/DHCPv6..."
            # Start with scopes and finish with gloval server options if not set on scopes
            $v6Scopes = Get-DhcpServerv6Scope
            foreach ($scope in $v6Scopes) {
                $hasOption60 = ((Get-DhcpServerv6OptionValue -Prefix $scope.Prefix -All | Where-Object { $_.OptionId -eq 60 } | Select-Object -ExpandProperty Name).Count -gt 0)
                if ($hasOption60) {
                    # Stop
                    break
                }
            }
            if ($hasOption60 -eq $false) {
                $hasOption60 = ((Get-DhcpServerv6OptionValue | Where-Object { $_.OptionId -eq 60 } | Select-Object -ExpandProperty Name).Count -gt 0)
            }
            break
        }
    }
    if ($hasOption60) {
        Write-Host "DHCP Option 60 (PXEClient) found on server."
    } else {
        Write-Host "DHCP Option 60 (PXEClient) not found on server."
    }
    return $hasOption60
}

function Start-DismCommand {
    <#
        .SYNOPSIS
            Starts a DISM command/cmdlet
        .PARAMETER Verb
            The DISM action to perform
        .PARAMETER ImagePath
            The target image to perform changes to/WIM file to mount
        .PARAMETER ImageIndex
            The image index to mount
        .PARAMETER MountPath
            The directory to mount the Windows image to
        .PARAMETER Commit
            Determine whether to commit (save) the changes made to a Windows image
        .PARAMETER WimFile
            The source WIM file to apply
        .PARAMETER WimIndex
            The image index to apply
        .PARAMETER PackagePath
            The source package file to add to the Windows image
        .PARAMETER PackageName
            The package to remove from the Windows image
        .PARAMETER FeatureEnablementName
            The feature to enable on the Windows image
        .PARAMETER FeatureEnablementSource
            The source to use for feature enablement
        .PARAMETER FeatureDisablementName
            The feature to disable on the Windows image
        .PARAMETER FeatureDisablementRemove
            Determine whether to remove the manifest of a feature
        .PARAMETER AppxPackageFile
            The application (AppX) package to add to the Windows image
        .PARAMETER AppxLicenseFile
            The license file to add in order to install an application
        .PARAMETER AppxCustomDataFile
            The custom data file for an application
        .PARAMETER AppxRegions
            The regions to make an application available on
        .PARAMETER AppxPackageName
            The name of the application (AppX) package to remove
        .PARAMETER CapabilityAdditionName
            The name of the capability to add
        .PARAMETER CapabilityAdditionSource
            The source to use for capability addition
        .PARAMETER CapabilityRemovalName
            The name of the capability to remove
        .PARAMETER DriverAdditionFile
            The driver package to add to the Windows image
        .PARAMETER DriverAdditionRecurse
            Determine whether to scan a driver folder recursively for additional packages
    #>
    [CmdletBinding(DefaultParameterSetName='Default')]
    param (
        [Parameter(Mandatory = $true, Position=0)] [ValidateSet('Mount', 'Commit', 'Unmount', 'Apply', 'Add-Package', 'Remove-Package', 'Enable-Feature', 'Disable-Feature', 'Add-Appx', 'Remove-Appx', 'Add-Capability', 'Remove-Capability', 'Add-Driver', 'UnattendApply')] [string]$Verb,
        [Parameter(Mandatory = $true, Position=1)] [string]$ImagePath,
        # Parameters for mount command
        [Parameter(ParameterSetName='Mount', Mandatory = $true, Position = 2)] [int]$ImageIndex,
        [Parameter(ParameterSetName='Mount', Mandatory = $true, Position = 3)] [string]$MountPath,
        # Parameters for unmount command
        [Parameter(ParameterSetName='Unmount', Mandatory = $true, Position = 2)] [bool]$Commit,
        # Parameters for application command
        [Parameter(ParameterSetName='Apply', Mandatory = $true, Position=2)] [string]$WimFile,
        [Parameter(ParameterSetName='Apply', Mandatory = $true, Position=3)] [int]$WimIndex,
        # Parameters for package addition
        [Parameter(ParameterSetName='Add-Package', Mandatory = $true, Position=2)] [string]$PackagePath,
        # Parameters for package removal
        [Parameter(ParameterSetName='Remove-Package', Mandatory = $true, Position=2)] [string]$PackageName,
        # Parameters for feature enablement
        [Parameter(ParameterSetName='Enable-Feature', Mandatory = $true, Position=2)] [string]$FeatureEnablementName,
        [Parameter(ParameterSetName='Enable-Feature', Mandatory = $true, Position=3)] [string]$FeatureEnablementSource,
        # Parameters for feature disablement
        [Parameter(ParameterSetName='Disable-Feature', Mandatory = $true, Position=2)] [string]$FeatureDisablementName,
        [Parameter(ParameterSetName='Disable-Feature', Mandatory = $true, Position=3)] [bool]$FeatureDisablementRemove,
        # Parameters for AppX package addition
        [Parameter(ParameterSetName='Add-Appx', Mandatory = $true, Position=2)] [string]$AppxPackageFile,
        [Parameter(ParameterSetName='Add-Appx', Mandatory = $true, Position=3)] [string]$AppxLicenseFile,
        [Parameter(ParameterSetName='Add-Appx', Mandatory = $true, Position=4)] [string]$AppxCustomDataFile,
        [Parameter(ParameterSetName='Add-Appx', Mandatory = $true, Position=5)] [string]$AppxRegions,
        # Parameters for AppX package removal
        [Parameter(ParameterSetName='Remove-Appx', Mandatory = $true, Position=2)] [string]$AppxPackageName,
        # Parameters for capability addition
        [Parameter(ParameterSetName='Add-Capability', Mandatory = $true, Position=2)] [string]$CapabilityAdditionName,
        [Parameter(ParameterSetName='Add-Capability', Mandatory = $true, Position=3)] [string]$CapabilityAdditionSource,
        # Parameters for capability removal
        [Parameter(ParameterSetName='Remove-Capability', Mandatory = $true, Position=2)] [string]$CapabilityRemovalName,
        # Parameters for driver addition
        [Parameter(ParameterSetName='Add-Driver', Mandatory = $true, Position=2)] [string]$DriverAdditionFile,
        [Parameter(ParameterSetName='Add-Driver', Mandatory = $true, Position=3)] [bool]$DriverAdditionRecurse,
        # Parameters for unattended answer file application
        [Parameter(ParameterSetName='UnattendApply', Mandatory = $true, Position=2)] [string]$unattendPath
    )
    try
    {
        switch ($Verb)
        {
            "Mount" {
                Mount-WindowsImage -ImagePath $ImagePath -Index $ImageIndex -Path $MountPath | Out-Null
            }
            "Commit" {
                Save-WindowsImage -Path $ImagePath | Out-Null
            }
            "Unmount" {
                if ($Commit)
                {
                    Dismount-WindowsImage -Path $ImagePath -Save | Out-Null
                }
                else
                {
                    Dismount-WindowsImage -Path $ImagePath -Discard | Out-Null
                }
            }
            "Apply" {
                $dismProc = Start-Process -FilePath "$env:SYSTEMROOT\system32\dism.exe" -ArgumentList "/apply-image /imagefile=`"$WimFile`" /index=$WimIndex /applydir=$ImagePath" -Wait -PassThru -NoNewWindow
                return ($($dismProc.ExitCode) -eq 0)
            }
            "Add-Package" {
                Add-WindowsPackage -Path "$ImagePath" -PackagePath "$PackagePath" -NoRestart | Out-Null
            }
            "Remove-Package" {
                Remove-WindowsPackage -Path "$ImagePath" -PackageName $PackageName -NoRestart | Out-Null
            }
            "Enable-Feature" {
                Enable-WindowsOptionalFeature -Path "$ImagePath" -FeatureName $FeatureEnablementName -LimitAccess -Source "$FeatureEnablementSource" -NoRestart | Out-Null
            }
            "Disable-Feature" {
                if ($FeatureDisablementRemove)
                {
                    Disable-WindowsOptionalFeature -Path "$ImagePath" -FeatureName $FeatureDisablementName -NoRestart -Remove | Out-Null
                }
                else
                {
                    Disable-WindowsOptionalFeature -Path "$ImagePath" -FeatureName $FeatureDisablementName -NoRestart | Out-Null
                }
            }
            "Add-Appx" {
                if ($AppxRegions -eq "all")
                {
                    Add-AppxProvisionedPackage -Path "$ImagePath" -PackagePath "$AppxPackageFile" -LicensePath "$AppxLicenseFile" -CustomDataPath "$AppxCustomDataFile" | Out-Null
                }
                else
                {
                    Add-AppxProvisionedPackage -Path "$ImagePath" -PackagePath "$AppxPackageFile" -LicensePath "$AppxLicenseFile" -CustomDataPath "$AppxCustomDataFile" -Regions "$AppxRegions" | Out-Null
                }
            }
            "Remove-Appx" {
                Remove-AppxProvisionedPackage -Path "$ImagePath" -PackageName $AppxPackageName | Out-Null
            }
            "Add-Capability" {
                Add-WindowsCapability -Path "$ImagePath" -Name $CapabilityAdditionName -LimitAccess -Source "$CapabilityAdditionSource" -NoRestart | Out-Null
            }
            "Remove-Capability" {
                Remove-WindowsCapability -Path "$ImagePath" -Name $CapabilityRemovalName -NoRestart | Out-Null
            }
            "Add-Driver" {
                $scratchDir = ""
                if ((Test-Path -Path "$($ImagePath)`$DISMTOOLS.~LS") -and ((Get-ChildItem "$($ImagePath)`$DISMTOOLS.~LS\PackageTemp" -Directory).Count -eq 1))
                {
                    foreach ($dir in (Get-ChildItem "$($ImagePath)`$DISMTOOLS.~LS\PackageTemp" -Directory))
                    {
                        $scratchDir = $dir.FullName
                    }
                }
                if ($DriverAdditionRecurse)
                {
                    if ($scratchDir -ne "")
                    {
                        Add-WindowsDriver -Path "$ImagePath" -Driver "$DriverAdditionFile" -ScratchDirectory "$scratchDir" -Recurse | Out-Null
                    }
                    else
                    {
                        Add-WindowsDriver -Path "$ImagePath" -Driver "$DriverAdditionFile" -Recurse | Out-Null
                    }
                }
                else
                {
                    if ($scratchDir -ne "")
                    {
                        Add-WindowsDriver -Path "$ImagePath" -Driver "$DriverAdditionFile" -ScratchDirectory "$scratchDir" | Out-Null
                    }
                    else
                    {
                        Add-WindowsDriver -Path "$ImagePath" -Driver "$DriverAdditionFile" | Out-Null
                    }
                }
            }
            "UnattendApply" {
                try
                {
                    # Copy unattended answer file to target image
                    New-Item -ItemType Directory -Force -Path "$ImagePath\Windows\Panther"
                    Copy-Item -Path "$unattendPath" -Destination "$ImagePath\Windows\Panther\unattend.xml" -Force
                    New-Item -ItemType Directory -Force -Path "$ImagePath\Windows\System32\Sysprep"
                    Copy-Item -Path "$unattendPath" -Destination "$ImagePath\Windows\System32\Sysprep\unattend.xml" -Force
                }
                catch
                {
                    Apply-WindowsUnattend -Path "$ImagePath\" -UnattendPath "$unattendPath" -NoRestart
                }
            }
            default {

            }
        }
        return $?
    }
    catch
    {
        Write-Host "Could not run command successfully."
        return $false
    }
}

function Add-BootImageToWds {
    <#
        .SYNOPSIS
            Adds the boot image to the Windows Deployment Services server
        .PARAMETER bootImage
            The image to add to the server
    #>

    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $bootImage
    )

    $imageName = Read-Host -Prompt "Name of the Windows image to put in WDS"
    if ($imageName -eq "auto") {
        try {
            $imageName = (Get-WindowsImage -ImagePath "$bootImage")[0].ImageName
        } catch {
            $imageName = "Boot Image"
        }
    }
    $imageDescription = Read-Host -Prompt "Description of the Windows image to put in WDS (you can continue without it)"
    $imagePriority = 0
    $imagePriorityStr = Read-Host -Prompt "Priority of the Windows image to put in WDS (lower values have more priority)"
    try {
        if ($imagePriorityStr -eq "") { throw }
        $imagePriority = [int]$imagePriorityStr
    } catch {
        Write-Host "Defaulting to 500000. You can set a different priority in WDS."
        $imagePriority = 500000
    }
    $imageMulticast = Read-Host -Prompt "Choose transmission type: [U]nicast; [M]ulticast"
    $imageTrName = ""
    if ($imageMulticast -eq "M") {
        $imageTrName = Read-Host -Prompt "Since you specified a multicast transmission, you have to specify a name. Please type the name of the transmission:"
        if ($imageTrName -eq "") {
            Write-Host "No transmission name was specified. Reverting to unicast..."
            $imageMulticast = "U"
        }
    } else {
        Write-Host "Using unicast transmissions..."
    }

    try {
        Write-Host "Adding image to WDS..."
        if ($imageMulticast -eq "M") {
            Import-WdsBootImage -Path "$bootImage" -NewImageName "$imageName" -NewDescription "$imageDescription" -DisplayOrder $imagePriority -Multicast $true -TransmissionName "$imageTrName"
        } else {
            Import-WdsBootImage -Path "$bootImage" -NewImageName "$imageName" -NewDescription "$imageDescription" -DisplayOrder $imagePriority
        }
        Write-Host "Image added to WDS successfully. Close and re-open WDS to see the changes in the `"Boot Images`" part of the server."
    } catch {
        Write-Host "Could not add image to WDS..."
    }
}

Clear-Host

Write-Host "DISMTools $version - PE Helper WDS Preparation Scripts"
Write-Host "(c) 2025. CodingWonders Software"
Write-Host "-----------------------------------------------------------"

if (Update-WinPEForWds -bootImage "$bootImagePath") {
    if ((Read-Host -Prompt "Do you want to add this image to WDS? (Y/N)") -eq "Y") {
        Add-BootImageToWds -bootImage "$bootImagePath"
    }
}