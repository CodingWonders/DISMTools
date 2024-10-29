#requires -version 5.0
#                                              ....
#                                         .'^""""""^.
#      '^`'.                            '^"""""""^.
#     .^"""""`'                       .^"""""""^.                ---------------------------------------------------------
#      .^""""""`                      ^"""""""`                  | DISMTools 0.6                                         |
#       ."""""""^.                   `""""""""'           `,`    | The connected place for Windows system administration |
#         '`""""""`.                 """""""""^         `,,,"    ---------------------------------------------------------
#            '^"""""`.               ^""""""""""'.   .`,,,,,^    | Preinstallation Environment (PE) helper               |
#              .^"""""`.            ."""""""",,,,,,,,,,,,,,,.    ---------------------------------------------------------
#                .^"""""^.        .`",,"""",,,,,,,,,,,,,,,,'     | (C) 2024 CodingWonders Software                       |
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

[CmdletBinding(DefaultParameterSetName='Default')]
param (
    [Parameter(Mandatory = $true, Position = 0)] [ValidateSet('StartPEGen', 'StartApply', 'StartDevelopment', 'Help')] [string]$cmd,
    [Parameter(ParameterSetName = 'StartPEGen', Mandatory = $true, Position = 1)] [string]$arch,
    [Parameter(ParameterSetName = 'StartPEGen', Mandatory = $true, Position = 2)] [string]$imgFile,
    [Parameter(ParameterSetName = 'StartPEGen', Mandatory = $true, Position = 3)] [string]$isoPath,
    [Parameter(ParameterSetName = 'StartPEGen', Position = 4)] [string]$unattendFile,
    [Parameter(ParameterSetName = 'StartDevelopment', Mandatory = $true, Position = 1)] [string]$testArch,
    [Parameter(ParameterSetName = 'StartDevelopment', Mandatory = $true, Position = 2)] [string]$targetPath
)

enum PE_Arch {
    x86 = 0
    amd64 = 1
    arm = 2
    arm64 = 3
}

class TargetImage {
    [int]$index
    [string]$wimPath
    TargetImage() { $this.Init(@{} )}
    # Create constructor
    TargetImage([int]$index, [string]$wimPath) {
        $this.index = $index
        $this.wimPath = $wimPath
    }
}

if (([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator) -eq $false)
{
    Write-Host "You need to run this script as an administrator"
    exit 1
}

function Start-PEGeneration
{
    <#
        .SYNOPSIS
            Generates a Preinstallation Environment (PE) that contains the Windows image specified in the GUI or via the command line
    #>
    $mountDirectory = ""
    $architecture = [PE_Arch]::($arch)
    $version = "0.6"
    Write-Host "DISMTools $version - Preinstallation Environment Helper"
    Write-Host "(c) 2024. CodingWonders Software"
    Write-Host "-----------------------------------------------------------"
    # Start PE generation
    Write-Host "Starting PE generation..."
    # Detect if the Windows ADK is present
    try
    {
        if ((Get-ItemPropertyValue -Path 'HKLM:\SOFTWARE\Microsoft\WIMMount' -Name 'AdkInstallation') -eq 1)
        {
            # An ADK may be installed, but it may not be Windows 10 ADK
            $progFiles = ""
            $peToolsPath = ""
            if ([Environment]::Is64BitOperatingSystem)
            {
                $progFiles = "$([IO.Path]::GetPathRoot([Environment]::GetFolderPath([Environment+SpecialFolder]::Windows)))Program Files (x86)"
            }
            else
            {
                $progFiles = "$([IO.Path]::GetPathRoot([Environment]::GetFolderPath([Environment+SpecialFolder]::Windows)))Program Files"
            }
            if (Test-Path "$progFiles\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment")
            {
                $peToolsPath = "$progFiles\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment"
                Write-Host "Creating working directory and copying Preinstallation Environment (PE) files..."
                if ((Copy-PEFiles -peToolsPath "$progFiles\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment" -architecture $architecture -targetDir "$((Get-Location).Path)\ISOTEMP") -eq $false)
                {
                    Write-Host "Preinstallation Environment creation has failed in the PE file copy phase."
                    Write-Host "`nPress ENTER to exit"
                    Read-Host | Out-Null
                    exit 1
                }
                Write-Host "Creating temporary mount directory..."
                try
                {
                    $mountDirectory = "$env:TEMP\DISMTools_PE_Scratch_$((Get-Date).ToString("MM-dd-yyyy_HH-mm-ss"))_$(Get-Random -Maximum 10000)"
                    New-Item "$mountDirectory" -ItemType Directory | Out-Null
                }
                catch
                {
                    Write-Host "Could not create temporary mount directory. Using default folder..."
                    $mountDirectory = "$((Get-Location).Path)\ISOTEMP\mount"
                }
                Write-Host "Mounting Windows image. Please wait..."
                if ((Start-DismCommand -Verb Mount -ImagePath "$((Get-Location).Path)\ISOTEMP\media\sources\boot.wim" -ImageIndex 1 -MountPath "$mountDirectory") -eq $false)
                {
                    Write-Host "Preinstallation Environment creation has failed in the PE image mount phase."
                    Write-Host "`nPress ENTER to exit"
                    Read-Host | Out-Null
                    exit 1
                }
                Write-Host "Copying Windows PE optional components. Please wait..."
                if ((Copy-PEComponents -peToolsPath "$progFiles\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment" -architecture $architecture -targetDir "$((Get-Location).Path)\ISOTEMP") -eq $false)
                {
                    Write-Host "Preinstallation Environment creation has failed in the PE optional component copy phase."
                    Write-Host "`nPress ENTER to exit"
                    Read-Host | Out-Null
                    exit 1
                }
                Write-Host "Adding OS packages..."
                $pkgs = [List[string]]::new()
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\WinPE-NetFx.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\en-US\WinPE-NetFx_en-us.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\WinPE-WMI.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\en-US\WinPE-WMI_en-us.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\WinPE-PowerShell.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\en-US\WinPE-PowerShell_en-us.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\WinPE-DismCmdlets.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\en-US\WinPE-DismCmdlets_en-us.cab")
                foreach ($pkg in $pkgs)
                {
                    if (Test-Path $pkg -PathType Leaf)
                    {
                        Write-Host "Adding OS package $([IO.Path]::GetFileNameWithoutExtension($pkg))..."
                        Start-DismCommand -Verb Add-Package -ImagePath "$mountDirectory" -PackagePath $pkg | Out-Null
                    }
                }
                Write-Host "Saving changes..."
                Start-DismCommand -Verb Commit -ImagePath "$mountDirectory" | Out-Null
                # Perform customization tasks later
                Write-Host "Beginning customizations..."
                if ((Start-PECustomization -ImagePath "$mountDirectory" -arch $architecture -testStartNet $false) -eq $false)
                {
                    Write-Host "Preinstallation Environment creation has failed in the PE customization phase. Discarding changes..."
                    Start-DismCommand -Verb Unmount -ImagePath "$mountDirectory" -Commit $false | Out-Null
                    Write-Host "`nPress ENTER to exit"
                    Read-Host | Out-Null
                    exit 1
                }
                Write-Host "Unmounting image..."
                Start-DismCommand -Verb Unmount -ImagePath "$mountDirectory" -Commit $true | Out-Null
                Write-Host "PE generated successfully"
                # Continue ISO customization
                Write-Host "Copying image file. This can take some time..."
                $totalTime = 0
                if (Test-Path "$imgFile" -PathType Leaf)
                {
                    $totalTime = Measure-Command { Copy-Item -Path "$imgFile" -Destination "$((Get-Location).Path)\ISOTEMP\media\sources\install.wim" -Verbose -Force -Recurse -Container }
                }
                if ($?)
                {
                    Write-Host "The image file has been copied successfully. Time taken: $($totalTime.Minutes) minutes, $($totalTime.Seconds) seconds"
                }
                else
                {
                    Write-Host "The image file has not been copied successfully."
                    Write-Host "`nPress ENTER to exit"
                    Read-Host | Out-Null
                    exit 1
                }
                Write-Host "Copying setup tools..."
                Copy-Item -Path "$((Get-Location).Path)\PE_Helper.ps1" -Destination "$((Get-Location).Path)\ISOTEMP\media" -Verbose -Force -Recurse -Container -ErrorAction SilentlyContinue
                New-Item -Path "$((Get-Location).Path)\ISOTEMP\media\files\diskpart" -ItemType Directory | Out-Null
                Copy-Item -Path "$((Get-Location).Path)\files\diskpart\*.dp" -Destination "$((Get-Location).Path)\ISOTEMP\media\files\diskpart" -Verbose -Force -Recurse -Container -ErrorAction SilentlyContinue
                Copy-Item -Path "$((Get-Location).Path)\files\README1ST.TXT" -Destination "$((Get-Location).Path)\ISOTEMP\media\README.TXT" -Verbose -Force -Recurse -Container -ErrorAction SilentlyContinue
                New-Item -Path "$((Get-Location).Path)\ISOTEMP\media\Tools\DIM" -ItemType Directory | Out-Null
                Copy-Item -Path "$((Get-Location).Path)\tools\DIM\*" -Destination "$((Get-Location).Path)\ISOTEMP\media\Tools\DIM" -Verbose -Force -Recurse -Container -ErrorAction SilentlyContinue
                New-Item -Path "$((Get-Location).Path)\ISOTEMP\media\Tools\RestartDialog" -ItemType Directory | Out-Null
                Copy-Item -Path "$((Get-Location).Path)\tools\RestartDialog\*" -Destination "$((Get-Location).Path)\ISOTEMP\media\Tools\RestartDialog" -Verbose -Force -Recurse -Container -ErrorAction SilentlyContinue
                if (($unattendFile -ne "") -and (Test-Path "$unattendFile" -PathType Leaf))
                {
                    Write-Host "Unattended answer file has been detected. Copying to ISO file..."
                    Copy-Item -Path "$unattendFile" -Destination "$((Get-Location).Path)\ISOTEMP\media\unattend.xml" -Verbose -Force -Recurse -Container -ErrorAction SilentlyContinue
                }
                Write-Host "Deleting temporary files..."
                Remove-Item -Path "$((Get-Location).Path)\ISOTEMP\OCs" -Recurse -Force -ErrorAction SilentlyContinue
                if ($?)
                {
                    Write-Host "Temporary files have been deleted successfully"
                }
                else
                {
                    Write-Host "Temporary files haven't been deleted successfully"
                }
                Write-Host "The ISO file structure has been successfully created. DISMTools will continue creating the ISO file automatically after 5 seconds."
                Start-Sleep -Seconds 5
                Write-Host "Creating ISO file..."
                if ((New-WinPEIso -peToolsPath $peToolsPath -isoLocation $isoPath) -eq $false)
                {
                    Write-Host "The ISO file has not been created successfully."
                    Write-Host "Deleting temporary files..."
                    Remove-Item -Path "$((Get-Location).Path)\ISOTEMP" -Recurse -Force -ErrorAction SilentlyContinue
                    Write-Host "`nPress ENTER to exit"
                    Read-Host | Out-Null
                    exit 1
                }
                Write-Host "Deleting temporary files..."
                Remove-Item -Path "$((Get-Location).Path)\ISOTEMP" -Recurse -Force -ErrorAction SilentlyContinue
                if ($mountDirectory.StartsWith("$env:TEMP"))
                {
                    Remove-Item -Path "$mountDirectory" -Recurse -Force -ErrorAction SilentlyContinue
                }
                Write-Host "The ISO file has been successfully created on the location you specified"
                Start-Sleep -Seconds 5
                exit 0
            }
            else
            {
                Write-Host "A Windows Assessment and Deployment Kit (ADK) could not be found on your system. Please install the Windows ADK for Windows 10 (or Windows 11), and its Windows PE plugin, and try again."
                Write-Host "`nPress ENTER to exit"
                Read-Host | Out-Null
                exit 1
            }
        }
        else
        {
            Write-Host "A Windows Assessment and Deployment Kit (ADK) could not be found on your system. Please install the Windows ADK for Windows 10 (or Windows 11), and its Windows PE plugin, and try again."
            Write-Host "`nPress ENTER to exit"
            Read-Host | Out-Null
            exit 1
        }
    }
    catch
    {
        Write-Host "This process is unsuccessful as the following error occurred: $_"
        Write-Host "`nPress ENTER to exit"
        Read-Host | Out-Null
        exit 1
    }

}

function Copy-PEFiles
{
    <#
        .SYNOPSIS
            Copies the Preinstallation Environment (PE) files to a temporary folder in the working directory
        .PARAMETER peToolsPath
            The path of the Preinstallation Environment (PE) tools. By default, this is "Program Files\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools"
        .PARAMETER architecture
            The architecture of the target Preinstallation Environment (PE). Valid options: x86, amd64, arm, arm64
        .PARAMETER targetDir
            The target directory to copy the Preinstallation Environment (PE) files to
        .EXAMPLE
            Copy-PEFiles -peToolsPath "C:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools" -architecture amd64 -targetDir "ISOTEMP"
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string]$peToolsPath,
        [Parameter(Mandatory = $true, Position = 1)] [PE_Arch]$architecture,
        [Parameter(Mandatory = $true, Position = 2)] [string]$targetDir
    )
    try
    {
        $og_Location = (Get-Location).Path
        Set-Location $peToolsPath
        # Set required environment variables
        Set-Item -Path "env:WinPERoot" -Value "$peToolsPath"
        if ([Environment]::Is64BitOperatingSystem)
        {
            Set-Item -Path "env:OSCDImgRoot" -Value "$peToolsPath\..\Deployment Tools\amd64\Oscdimg"
        }
        else
        {
            Set-Item -Path "env:OSCDImgRoot" -Value "$peToolsPath\..\Deployment Tools\x86\Oscdimg"
        }
        $copype = Start-Process -FilePath "$peToolsPath\copype.cmd" -ArgumentList "$architecture `"$targetDir`"" -Wait -PassThru -NoNewWindow
        if ($copype.ExitCode -eq 0)
        {
            Write-Host "PE files copied successfully."
        }
        else
        {
            Write-Host "Failed to copy PE files."
        }
        Set-Location $og_Location
        return $($copype.ExitCode -eq 0)
    }
    catch
    {
        Write-Host "Failed to copy PE files."
        return $false
    }
}

function Copy-PEComponents
{
    <#
        .SYNOPSIS
            Copies the Preinstallation Environment (PE) component files to a temporary folder in the working directory
        .PARAMETER peToolsPath
            The path of the Preinstallation Environment (PE) tools. By default, this is "Program Files\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools"
        .PARAMETER architecture
            The architecture of the target Preinstallation Environment (PE). Valid options: x86, amd64, arm, arm64
        .PARAMETER targetDir
            The target directory to copy the Preinstallation Environment (PE) component files to
        .EXAMPLE
            Copy-PEComponents -peToolsPath "C:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools" -architecture amd64 -targetDir "ISOTEMP"
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string]$peToolsPath,
        [Parameter(Mandatory = $true, Position = 1)] [PE_Arch]$architecture,
        [Parameter(Mandatory = $true, Position = 2)] [string]$targetDir
    )
    try
    {
        New-Item -ItemType Directory -Path "$targetDir\OCs"
        New-Item -ItemType Directory -Path "$targetDir\OCs\en-US"
        $general_OCs = Get-ChildItem -Path "$peToolsPath\$($architecture.ToString())\WinPE_OCs" -File
        $loc_OCs = Get-ChildItem -Path "$peToolsPath\$($architecture.ToString())\WinPE_OCs\en-US" -File
        $copied = 0
        $totalSize = 1
        foreach ($file in $general_OCs)
        {
            Copy-Item -Path "$peToolsPath\$($architecture.ToString())\WinPE_OCs\$($file.Name)" -Destination "$targetDir\OCs" -Force -Container -PassThru -Verbose | ForEach-Object {
                $copied = ($_.BytesTransferred / $totalSize) * 100
                Write-Debug $copied
            }
        }
        foreach ($file in $loc_OCs)
        {
            Copy-Item -Path "$peToolsPath\$($architecture.ToString())\WinPE_OCs\en-US\$($file.Name)" -Destination "$targetDir\OCs\en-US" -Force -Container -PassThru -Verbose | ForEach-Object {
                $copied = ($_.BytesTransferred / $totalSize) * 100
                Write-Debug $copied
            }
        }
        Write-Host "PE components have been copied successfully."
        return $true
    }
    catch
    {
        Write-Host "Failed to copy PE optional components."
        return $false
    }
}

function Start-PECustomization
{
    <#
        .SYNOPSIS
            Starts the customization process of the Windows Preinstallation Environment (PE). This is a process required for the installer to work
        .PARAMETER imagePath
            The path of the mounted Windows PE image
        .PARAMETER arch
            The architecture of the target Windows PE image, which is used to customize the wallpaper
        .PARAMETE testStartNet
            Customizes the "startnet.cmd" file for WinPE testing
        .EXAMPLE
            Start-PECustomization -imagePath "<Mount Directory>" -arch "amd64" -testStartNet $false
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string]$imagePath,
        [Parameter(Mandatory = $true, Position = 1)] [PE_Arch]$arch,
        [Parameter(Mandatory = $true, Position = 2)] [bool]$testStartNet
    )
    try
    {
        if (Test-Path "$imagePath\Windows\system32\winpe.jpg" -PathType Leaf)
        {
            try
            {
                Write-Host "CUSTOMIZATION STEP - Change Wallpaper" -BackgroundColor DarkGreen
                Write-Host "Taking ownership of wallpaper..."
                takeown /F "$imagePath\Windows\system32\winpe.jpg" /A
                Write-Host "Setting Access Control Lists (ACLs) for wallpaper using icacls..."
                icacls "$imagePath\Windows\system32\winpe.jpg" /grant "$(Get-LocalizedUsers -admins $true):(M)" | Out-Host
                icacls "$imagePath\Windows\system32\winpe.jpg" /grant "$(Get-LocalizedUsers -admins $false):(M)" | Out-Host
                Write-Host "Changing wallpaper..."
                switch ($arch)
                {
                    x86 {
                        Copy-Item -Path "$((Get-Location).Path)\backgrounds\winpe_x86.jpg" -Destination "$imagePath\Windows\system32\winpe.jpg" -Force
                    }
                    amd64 {
                        Copy-Item -Path "$((Get-Location).Path)\backgrounds\winpe_amd64.jpg" -Destination "$imagePath\Windows\system32\winpe.jpg" -Force
                    }
                    arm {
                        Copy-Item -Path "$((Get-Location).Path)\backgrounds\winpe_arm.jpg" -Destination "$imagePath\Windows\system32\winpe.jpg" -Force
                    }
                    arm64 {
                        Copy-Item -Path "$((Get-Location).Path)\backgrounds\winpe_arm64.jpg" -Destination "$imagePath\Windows\system32\winpe.jpg" -Force
                    }
                    default {
                        Copy-Item -Path "$((Get-Location).Path)\backgrounds\winpe_amd64.jpg" -Destination "$imagePath\Windows\system32\winpe.jpg" -Force
                    }
                }
                Write-Host "Wallpaper changed"
            }
            catch
            {
                Write-Host "Could not change wallpaper..."
            }
        }
        try
        {
            Write-Host "CUSTOMIZATION STEP - Change Terminal Settings" -BackgroundColor DarkGreen
            Write-Host "Opening registry..."
            if (Open-PERegistry -regFile "$imagePath\Windows\system32\config\DEFAULT" -regName "PE_DefUser" -regLoad $true)
            {
                Write-Host "Setting window position..."
                Set-ItemProperty -Path "HKLM:\PE_DefUser\Console" -Name "WindowPosition" -Value 6291480
                Write-Host "Closing registry..."
                Open-PERegistry -regFile "$imagePath\Windows\system32\config\DEFAULT" -regName "PE_DefUser" -regLoad $false
            }
            else
            {
                Write-Host "Could not modify terminal settings"
            }
        }
        catch
        {
            Write-Host "Could not modify terminal settings"
        }
        if (($arch.ToString() -eq "x86") -or ($arch.ToString() -eq "amd64"))
        {
            try
            {
                Write-Host "CUSTOMIZATION STEP - Prepare System for Graphical Applications" -BackgroundColor DarkGreen
                Write-Host "Opening registry..."
                if (Open-PERegistry -regFile "$imagePath\Windows\system32\config\SOFTWARE" -regName "WINPESOFT" -regLoad $true)
                {
                    Write-Host "Setting CLSID keys..."
                    $clsidKey = "HKLM\WINPESOFT\Classes\CLSID\{AE054212-3535-4430-83ED-D501AA6680E6}"
                    reg add "$clsidKey" /f
                    reg add "$clsidKey" /f /ve /t REG_SZ /d "Shell Name Space ListView"
                    reg add "$clsidKey\InprocServer32" /f
                    reg add "$clsidKey\InprocServer32" /f /ve /t REG_EXPAND_SZ /d "%SystemRoot%\system32\explorerframe.dll"
                    reg add "$clsidKey\InprocServer32" /f /v "ThreadingModel" /t REG_SZ /d "Apartment"
                    Write-Host "Closing registry..."
                    reg unload "HKLM\WINPESOFT"
                    if (-not $?)
                    {
                        $attempts = 0
                        do
                        {
                            $attempts += 1
                            Start-Sleep -Milliseconds 500
                            reg unload "HKLM\WINPESOFT"
                        } until ($?)
                        Write-Host "Registry closed successfully after $($attempts + 1) attempt(s)"
                    }
                }
                else
                {
                    Write-Host "Could not prepare the system for graphical applications"
                }
                Write-Host "Copying DLL files..."
                switch ($arch)
                {
                    x86 {
                        Copy-Item -Path "\Windows\system32\ExplorerFrame.dll" -Destination "$imagePath\Windows\system32" -Force -Verbose
                    }
                    amd64 {
                        Copy-Item -Path "\Windows\system32\ExplorerFrame.dll" -Destination "$imagePath\Windows\system32" -Force -Verbose
                        Copy-Item -Path "\Windows\SysWOW64\ExplorerFrame.dll" -Destination "$imagePath\Windows\SysWOW64" -Force -Verbose
                    }
                }
                Write-Host "Creating folders..."
                New-Item -Path "$imagePath\Windows\system32\config\systemprofile\Desktop" -ItemType Directory -Force
                Write-Host "The target system is now ready for graphical applications"
            }
            catch
            {
                Write-Host "Could not prepare the system for graphical applications"
            }
        }
        try
        {
            Write-Host "CUSTOMIZATION STEP - Change Startup Commands" -BackgroundColor DarkGreen
            Write-Host "Changing startup commands..."
            Copy-Item -Path "$((Get-Location).Path)\files\startup\startnet.cmd" -Destination "$imagePath\Windows\system32\startnet.cmd" -Force
            if ($testStartNet)
            {
                $contents = Get-Content -Path "$imagePath\Windows\system32\startnet.cmd"
                $contents[5] = "set debug=2"
                Set-Content -Path "$imagePath\Windows\system32\startnet.cmd" -Value $contents -Force
            }
            Copy-Item -Path "$((Get-Location).Path)\files\startup\StartInstall.ps1" -Destination "$imagePath\StartInstall.ps1" -Force
            Write-Host "Startup commands changed"
        }
        catch
        {
            Write-Host "Could not change startup commands"
        }
        Write-Host "CUSTOMIZATION STEP - Set Scratch Size" -BackgroundColor DarkGreen
        Write-Host "Setting scratch size..."
        dism /English /image="$imagePath" /set-scratchspace=512 | Out-Host
        if ($?)
        {
            Write-Host "Scratch size set."
        }
        else
        {
            Write-Host "Scratch size could not be set."
        }
        return $true
    }
    catch
    {
        return $false
    }
}

function Get-LocalizedUsers
{
    <#
        .SYNOPSIS
            Gets a localized user group representation for ICACLS commands
        .PARAMETER admins
            Determines whether to get a localized user group representation for the Administrators user group
        .OUTPUTS
            A string containing the localized user group
        .EXAMPLE
            Get-LocalizedUsers -admins $true
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [bool]$admins
    )
    if ($admins)
    {
        return (Get-LocalGroup | Where-Object { $_.SID.Value -like "S-1-5-32-544" }).Name
    }
    else
    {
        return (Get-LocalGroup | Where-Object { $_.SID.Value -like "S-1-5-32-545" }).Name
    }
}

function Open-PERegistry
{
    <#
        .SYNOPSIS
            Performs actions with the registry hives of the Windows Preinstallation Environment (PE)
        .PARAMETER regFile
            The file of the registry hive to load
        .PARAMETER regName
            The name to use when loading a registry hive
        .PARAMETER regLoad
            Determine whether to load or unload a registry hive
        .EXAMPLE
            Open-PERegistry -regFile "<Mount Directory>\Windows\system32\config\SOFTWARE" -regName "PESoft" -regLoad $true
        .EXAMPLE
            Open-PERegistry -regFile "" -regName "PESoft" -regLoad $false
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string]$regFile,
        [Parameter(Mandatory = $true, Position = 1)] [string]$regName,
        [Parameter(Mandatory = $true, Position = 2)] [bool]$regLoad
    )
    try
    {
        if ($regLoad)
        {
            reg load "HKLM\$regName" "$regFile"
        }
        else
        {
            reg unload "HKLM\$regName"
        }
        Write-Host "Registry action performed successfully"
        return $true
    }
    catch
    {
        return $false
    }
}

function New-WinPEIso
{
    <#
        .SYNOPSIS
            Creates the target ISO file defined either in the GUI or via the command line
        .PARAMETER peToolsPath
            The path of the Preinstallation Environment (PE) tools. By default, this is "Program Files\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools"
        .PARAMETER isoLocation
            The path of the target ISO file
        .EXAMPLE
            New-WinPEIso -peToolsPath "C:\Program Files\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools" -isoLocation "C:\PreInstEnv.iso"
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string]$peToolsPath,
        [Parameter(Mandatory = $true, Position = 1)] [string]$isoLocation
    )
    try
    {
        if (Test-Path "$isoLocation" -PathType Leaf)
        {
            # Check if the ISO file exists
            Remove-Item -Path "$isoLocation" -Force
        }
        if ([Environment]::Is64BitOperatingSystem)
        {
            Set-Item -Path "env:NewPath" -Value "$peToolsPath\..\Deployment Tools\amd64\Oscdimg"
        }
        else
        {
            Set-Item -Path "env:NewPath" -Value "$peToolsPath\..\Deployment Tools\x86\Oscdimg"
        }
        if (Test-Path "$((Get-Location).Path)\ISOTEMP\fwfiles\etfsboot.com" -PathType Leaf)
        {
            Write-Host "Generating ISO file with BIOS and UEFI compatibility..."
            $bootData = "2#p0,e,b`"$((Get-Location).Path)\ISOTEMP\fwfiles\etfsboot.com`"#pEF,e,b`"$((Get-Location).Path)\ISOTEMP\fwfiles\efisys.bin`""
        }
        else
        {
            Write-Host "Generating ISO file with UEFI compatibility..."
            $bootData = "1#pEF,e,b`"$((Get-Location).Path)\ISOTEMP\fwfiles\efisys.bin`""
        }
        $oscdimgProc = Start-Process "$env:NewPath\oscdimg.exe" -ArgumentList "-lDISMTools_PE -bootdata:$bootData -u2 -udfver102 `"$((Get-Location).Path)\ISOTEMP\media`" `"$isoLocation`"" -Wait -PassThru -NoNewWindow
        if ($oscdimgProc.ExitCode -eq 0)
        {
            Write-Host "ISO generation has completed successfully."
        }
        else
        {
            Write-Host "Failed to generate an ISO file."
        }
        return $($oscdimgProc.ExitCode -eq 0)
    }
    catch
    {
        Write-Host "Failed to generate an ISO file."
        return $false
    }
}

function Start-OSApplication
{
    <#
        .SYNOPSIS
            Starts the OS installation stage
    #>
    # Detect if it's run on Windows PE
    if ((Get-ItemPropertyValue -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -Name 'EditionID') -ne "WindowsPE")
    {
        Write-Host "This procedure must be run on Windows PE only."
        return
    }
    if ((Get-ChildItem -Path "$((Get-Location).Path)sources\*.wim" -Exclude "boot.wim").Count -lt 1)
    {
        Write-Host "No Windows image has been found on this drive. An installation image is required. Exiting..."
        exit 1
    }
    New-Item -Path "X:\files\diskpart" -ItemType Directory -Force | Out-Null
    $drive = Get-Disks
    if ($drive -eq "ERROR")
    {
        Write-Host "Script has failed."
        return
    }
    Write-Host "Selected disk: disk $($drive)"
    $partition = Get-Partitions $drive
    if ($partition -eq "B")
    {
        do {
            $drive = Get-Disks
            if ($drive -eq "ERROR")
            {
                Write-Host "Script has failed."
                return
            }
            Write-Host "Selected disk: disk $($drive)"
            $partition = Get-Partitions $drive
        } until ($partition -ne "B")
    }
    if ($partition -eq 0)
    {
        $msg = "This will perform disk configuration changes on disk $drive. THIS WILL DELETE ALL PARTITIONS IN IT. IF YOU ARE NOT WILLING TO LOSE DATA, DO NOT CONTINUE."
    }
    else
    {
        $msg = "This will perform disk configuration changes on partition $partition. THIS WILL FORMAT IT IT. IF YOU ARE NOT WILLING TO LOSE DATA, DO NOT CONTINUE."
    }
    Write-Host $msg -BackgroundColor Black -ForegroundColor Yellow
    $choice = Read-Host "Are you sure you want to continue (Y/N)"
    if ($choice -ne "Y")
    {
        do
        {
            $partition = Get-Partitions $drive
            if ($partition -eq "B")
            {
                do {
                    $drive = Get-Disks
                    if ($drive -eq "ERROR")
                    {
                        Write-Host "Script has failed."
                        return
                    }
                    Write-Host "Selected disk: disk $($drive)"
                    $partition = Get-Partitions $drive
                } until ($partition -ne "B")
            }
            if ($partition -eq 0)
            {
                $msg = "This will perform disk configuration changes on disk $drive. THIS WILL DELETE ALL PARTITIONS IN IT. IF YOU ARE NOT WILLING TO LOSE DATA, DO NOT CONTINUE.`n"
            }
            else
            {
                $msg = "This will perform disk configuration changes on partition $partition. THIS WILL FORMAT IT. IF YOU ARE NOT WILLING TO LOSE DATA, DO NOT CONTINUE.`n"
            }
            Write-Host $msg -BackgroundColor Black -ForegroundColor Yellow
            $choice = Read-Host "Are you sure you want to continue (Y/N)"
        } until ($choice -eq "Y")
    }
    $driveLetter = ""
    if ($partition -eq 0)
    {
        $driveLetter = "C"
        # Proceed with default disk configuration
        Write-DiskConfiguration $drive $true $partition
    }
    else
    {
        # Proceed with custom disk configuration
        Write-DiskConfiguration $drive $false $partition
        $volLister = @'
        lis vol
        exit
'@
        $volLister | Out-File "X:\files\diskpart\dp_vols.dp" -Force -Encoding utf8
        diskpart /s "X:\files\diskpart\dp_vols.dp" | Out-Host
        $driveLetter = Read-Host "Specify a drive letter"
        if ($driveLetter -eq "")
        {
            do
            {
                Write-Host "No drive letter has been specified."
                $driveLetter = Read-Host "Specify a drive letter"
            } until ($driveLetter -ne "")
        }
    }
    Write-Host "Creating page file for Windows PE..."
    wpeutil createpagefile /path="$($driveLetter):\WinPEpge.sys" /size=256
    $wimFile = Get-WimIndexes
    $serviceableArchitecture = (((Get-CimInstance -Class Win32_Processor | Where-Object { $_.DeviceID -eq "CPU0" }).Architecture) -eq (Get-WindowsImage -ImagePath "$($wimFile.wimPath)" -Index $wimFile.index).Architecture)
    Write-Host "Applying Windows image. This can take some time..."
    if ((Start-DismCommand -Verb Apply -ImagePath "$($driveLetter):\" -WimFile "$($wimFile.wimPath)" -WimIndex $wimFile.index) -eq $true)
    {
        Write-Host "The Windows image has been applied successfully."
    }
    else
    {
        Write-Host "Failed to apply the Windows image."
    }
    if ($serviceableArchitecture) { Set-Serviceability -ImagePath "$($driveLetter):\" } else { Write-Host "Serviceability tests will not be run: the image architecture and the PE architecture are different." }
    if (Test-Path "$((Get-Location).Path)\unattend.xml" -PathType Leaf)
    {
        Write-Host "A possible unattended answer file has been detected, applying it...        " -NoNewline
        if ((Start-DismCommand -Verb UnattendApply -ImagePath "$($driveLetter):" -unattendPath "$((Get-Location).Path)\unattend.xml") -eq $true)
        {
            Write-Host "SUCCESS" -ForegroundColor White -BackgroundColor DarkGreen
        }
        else
        {
            Write-Host "FAILURE" -ForegroundColor Black -BackgroundColor DarkRed
        }
    }
    $driverPath = "$([IO.Path]::GetPathRoot([Environment]::GetFolderPath([Environment+SpecialFolder]::Windows)))DT_InstDrvs.txt"
    if ((Test-Path "$($driveLetter):\`$DISMTOOLS.~LS") -and ($serviceableArchitecture) -and (Test-Path -Path $driverPath -PathType Leaf))
    {
        Write-Host "Adding drivers to the target image..."
        # Add drivers that were previously added to the Windows PE using the DIM
        $drivers = (Get-Content -Path $driverPath | Where-Object { $_.Trim() -ne "" })
        foreach ($driver in $drivers)
        {
            if (Test-Path -Path "$driver" -PathType Leaf)
            {
                Write-Host "Adding driver `"$driver`"...        " -NoNewline
                if ((Start-DismCommand -Verb Add-Driver -ImagePath "$($driveLetter):\" -DriverAdditionFile "$driver" -DriverAdditionRecurse $false) -eq $true)
                {
                    Write-Host "SUCCESS" -ForegroundColor White -BackgroundColor DarkGreen
                }
                else
                {
                    Write-Host "FAILURE" -ForegroundColor Black -BackgroundColor DarkRed
                }
            }
        }
        # Perform serviceability tests one more time
        if ($serviceableArchitecture) { Set-Serviceability -ImagePath "$($driveLetter):\" } else { Write-Host "Serviceability tests will not be run: the image architecture and the PE architecture are different." }
    }
    if (Test-Path "$($driveLetter):\`$DISMTOOLS.~LS")
    {
        Remove-Item -Path "$($driveLetter):\`$DISMTOOLS.~LS" -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
    }
    New-BootFiles -drLetter $driveLetter -bootPart "auto" -diskId $drive -cleanDrive $($partition -eq 0)
    Start-Sleep -Milliseconds 250
    try
    {
        # Get CPU architecture and launch Driver Installation Module
        $supportedArchitectures = [List[string]]::new()
        $supportedArchitectures.Add("i386")
        $supportedArchitectures.Add("amd64")
        $systemArchitecture = Get-SystemArchitecture

        if ($supportedArchitectures.Contains($systemArchitecture))
        {
            if (Test-Path -Path "$([IO.Path]::GetPathRoot([Environment]::GetFolderPath([Environment+SpecialFolder]::Windows)))Tools\RestartDialog\$systemArchitecture\DTPE-RestartDialog.exe")
            {
                Start-Process -FilePath "$([IO.Path]::GetPathRoot([Environment]::GetFolderPath([Environment+SpecialFolder]::Windows)))Tools\RestartDialog\$systemArchitecture\DTPE-RestartDialog.exe" -Wait
            }
        }

        Start-Sleep -Milliseconds 250
        Write-Host "Restarting your system..."
    }
    catch
    {
        # Show message before rebooting system
        Write-Host "The first stage of Setup has completed, and your system will reboot automatically."
        Write-Host "If there are any bootable devices, remove those before proceeding, as your system may boot to this environment again."
        Write-Host "When your computer restarts, Setup will continue."
        Show-Timeout -Seconds 10
    }
    wpeutil reboot
}

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
            return "arm64"
        }
        default {
            return ""
        }
    }
    return ""
}

function Get-Disks
{
    <#
        .SYNOPSIS
            Gets the available disks with DiskPart
    #>

    # Show disk list with diskpart
    if (Test-Path .\files\diskpart\dp_listdisk.dp -PathType Leaf)
    {
        diskpart /s ".\files\diskpart\dp_listdisk.dp" | Out-Host
    }
    else
    {
        Write-Host "DISKPART scripts not found."
        return "ERROR"
    }

    # Show additional tools
    Write-Host "- To load drivers, type `"DIM`" and press ENTER`n"

    $destDisk = Read-Host -Prompt "Please choose the disk to apply the image to"
    $destDrive = -1
    try
    {
        $destDrive = [int]$destDisk
        return $destDrive
    }
    catch
    {
        switch ($destDisk)
        {
            "DIM" {
                # Get CPU architecture and launch Driver Installation Module
                $supportedArchitectures = [List[string]]::new()
                $supportedArchitectures.Add("i386")
                $supportedArchitectures.Add("amd64")
                $systemArchitecture = Get-SystemArchitecture

                if ($supportedArchitectures.Contains($systemArchitecture))
                {
                    if (Test-Path -Path "$([IO.Path]::GetPathRoot([Environment]::GetFolderPath([Environment+SpecialFolder]::Windows)))Tools\DIM\$systemArchitecture\DT-DIM.exe")
                    {
                        Clear-Host
                        Write-Host "Starting the Driver Installation Module...`n`nYou will go back to the disk selection screen after closing the program."
                        Start-Process -FilePath "$([IO.Path]::GetPathRoot([Environment]::GetFolderPath([Environment+SpecialFolder]::Windows)))Tools\DIM\$systemArchitecture\DT-DIM.exe" -Wait
                    }
                }
                Get-Disks
            }
            default {
                Write-Host "Please specify a number and try again.`n"
                Get-Disks
            }
        }
    }
}

function Get-Partitions
{
    <#
        .SYNOPSIS
            Gets the partitions of a drive using DiskPart
        .PARAMETER driveNum
            The drive number
        .EXAMPLE
            Get-Partitions 0
    #>
    param (
        [Parameter(Mandatory = $true)] [int]$driveNum
    )

    $partLister = @'
    sel dis <REPLACEME>
    lis par
    exit
'@
    $partLister = $partLister.Replace("<REPLACEME>", $driveNum).Trim()
    $partLister | Out-File -FilePath "X:\files\diskpart\dp_listpart.dp" -Force -Encoding utf8
    $part = -1
    diskpart /s "X:\files\diskpart\dp_listpart.dp" | Out-Host
    Write-Host ""
    Write-Host "- If the selected disk contains no partitions, press ENTER. Otherwise, type a partition number."
    Write-Host "- If you have selected the wrong disk, type `"B`" now and press ENTER`n"
    $part = Read-Host -Prompt "Please choose the partition to apply the image to"
    if ($part -eq -1)
    {
        return $part
    }
    elseif ($part -eq "B")
    {
        return $part
    }
    else
    {
        try
        {
            $partition = [int]$part
            return $partition
        }
        catch
        {
            Write-Host "Please specify a number and try again.`n"
            Get-Partitions $driveNum
        }
    }
}

function Write-DiskConfiguration
{
    <#
        .SYNOPSIS
            Writes disk configuration using DiskPart
        .PARAMETER diskid
            The index number of the disk
        .PARAMETER cleanDrive
            Determine whether to clean the entire drive. Useful for single-boot scenarios
        .PARAMETER partId
            The partition number
        .NOTES
            The partition ID is 0 if the user decides to clean a drive
        .EXAMPLE
            Write-DiskConfiguration -diskid 0 -cleanDrive $true -partId 0
        .EXAMPLE
            Write-DiskConfiguration -diskid 0 -cleanDrive $false -partId 2
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [int]$diskid,
        [Parameter(Mandatory = $true, Position = 1)] [bool]$cleanDrive,
        [Parameter(Mandatory = $true, Position = 2)] [int]$partId
    )

    Write-Host "Writing disk configuration. Please wait..."
    if ($cleanDrive)
    {
        $formatter = @'
        sel dis #DISKID#
        cle
        #GPTPART#
        #MBRPART#
        exit
'@
        $formatter_gpt = @'
        conv gpt
        cre par efi size=512
        for fs=fat32 quick label="System"
        ass letter W
        cre par msr size=16
        cre par pri
        REM Prevent updates from failing to update WinRE
        shrink minimum=1024
        for quick label="Windows"
        ass letter C
        cre par pri
        for quick label="Recovery"
        ass letter R
        set id="de94bba4-06d1-4d40-a16a-bfd50179d6ac"
        gpt attributes=0x8000000000000001
'@
        $formatter_mbr = @'
        cre par pri size=100
        for quick label="System"
        ass letter W
        REM Important for MBR configurations
        active
        cre par pri
        REM Prevent updates from failing to update WinRE
        shrink minimum=1024
        for quick label="Windows"
        ass letter C
        cre par pri
        for quick label="Recovery"
        ass letter R
        set id=27
'@
        $uefiMode = ($env:firmware_type -eq "UEFI")
        $formatter = $formatter.Replace("#DISKID#", $diskId).Trim()
        if ($uefiMode)
        {
            $formatter = $formatter.Replace("#MBRPART#", "REM Unused Partition Block").Trim()
            $formatter = $formatter.Replace("#GPTPART#", $formatter_gpt).Trim()
        }
        else
        {
            $formatter = $formatter.Replace("#MBRPART#", $formatter_mbr).Trim()
            $formatter = $formatter.Replace("#GPTPART#", "REM Unused Partition Block").Trim()
        }
        $formatter | Out-File "X:\files\diskpart\dp_format.dp" -Force -Encoding utf8
        $dpProc = Start-Process -FilePath "$env:SYSTEMROOT\system32\diskpart.exe" -ArgumentList "/s `"X:\files\diskpart\dp_format.dp`"" -Wait -PassThru -NoNewWindow
    }
    else
    {
        $formatter = @'
        sel dis #DISKID#
        sel par #PARTID#
        for quick label="Windows"
        exit
'@
        $formatter = $formatter.Replace("#DISKID#", $diskId).Trim()
        $formatter = $formatter.Replace("#PARTID#", $partId).Trim()
        $formatter | Out-File "X:\files\diskpart\dp_format.dp" -Force -Encoding utf8
        $dpProc = Start-Process -FilePath "$env:SYSTEMROOT\system32\diskpart.exe" -ArgumentList "/s `"X:\files\diskpart\dp_format.dp`"" -Wait -PassThru -NoNewWindow
    }
    Write-Host "Disk configuration has been written successfully."
}

function Get-WimIndexes
{
    <#
        .SYNOPSIS
            Gets the image indexes of the Windows Imaging (WIM) file
    #>
    Import-Module Dism
    $wimPath = ""
    if ((Get-ChildItem -Path "$((Get-Location).Path)sources\*.wim" -Exclude "boot.wim").Count -gt 1)
    {
        Write-Host "`nMultiple installation images have been found in this installation medium. Please select an image file from the list and press ENTER."
        Write-Host "`nDo note that, after the selection of an image, you may not be able to go back."
        (Get-ChildItem -Path "$((Get-Location).Path)sources\*.wim" -Exclude "boot.wim") | Out-Host
        $wimPath = Read-Host "Choose the image file to apply"
        $wimPath = "$((Get-Location).Path)sources\$wimPath"
        if (($wimPath -eq "") -or (-not (Test-Path "$wimPath" -PathType Leaf)))
        {
            do {
                $wimPath = Read-Host "Choose the image file to apply"
                $wimPath = "$((Get-Location).Path)sources\$wimPath"
            } until (($wimPath -ne "") -and (Test-Path "$wimPath" -PathType Leaf))
        }
    }
    elseif ((Get-ChildItem -Path "$((Get-Location).Path)sources\*.wim" -Exclude "boot.wim").Count -eq 1)
    {
        $wimPath = "$((Get-Location).Path)sources\install.wim"
    }
    (Get-WindowsImage -ImagePath "$wimPath" | Format-Table ImageIndex, ImageName) | Out-Host
    $idx = Read-Host -Prompt "Specify the image index to apply"
    try
    {
        $index = [int]$idx
        # return $index
        $wimFile = [TargetImage]::new($index, $wimPath)
        return $wimFile
    }
    catch
    {
        Write-Host "Please specify an index and try again.`n"
        Get-WimIndexes
    }
}

function Start-DismCommand
{
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

function Set-Serviceability
{
    <#
        .SYNOPSIS
            Runs the serviceability tests to make sure the Windows image is valid for installation
        .PARAMETER ImagePath
            The path of the deployed image to test
        .NOTES
            Passing the serviceability tests is required for a successful installation. This test may fail due to these reasons:
            - The component store of the image is corrupted. Run "dism /image=<image> /cleanup-image /restorehealth /source=<source> /limitaccess" to attempt repairs
            - The architectures of the Preinstallation Environment (PE) and the target image are different
            If the serviceability tests fail due to the former, the second stage of Windows Setup (windeploy stage) will fail and, because of how the Setup system works in Windows Vista and later, you will not be able to install your image
        .EXAMPLE
            Set-Serviceability -ImagePath "C:"
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string]$ImagePath
    )
    Write-Host "Starting serviceability tests..."
    # Follow Panther engine steps (https://github.com/CodingWonders/Panther-Diagram)
    Write-Host "Creating temporary directory for serviceability operations..."
    $scratchDir = ""
    $driveLetter = ""
    try
    {
        $folderPath = $ImagePath.Replace("\", "").Trim()
        $driveLetter = $folderPath
        if (-not (Test-Path "$folderPath\`$DISMTOOLS.~LS")) { New-Item -Path "$folderPath\`$DISMTOOLS.~LS" -ItemType Directory | Out-Null }
        $guidStr = [System.Guid]::NewGuid().Guid
        New-Item -Path "$folderPath\`$DISMTOOLS.~LS\PackageTemp\$guidStr" -ItemType Directory | Out-Null
        Write-Host "Successfully created the scratch directory."
        $scratchDir = "$folderPath\`$DISMTOOLS.~LS\PackageTemp\$guidStr"
        # Bit of a mouthful, but good for PowerShell verbs (+ scratch dir support)
        dism /image=$ImagePath /scratchdir="$scratchDir" /is-serviceable
    }
    catch
    {
        Write-Host "Could not create temporary directory. Continuing without one. Do note that the serviceability tests might fail."
        # Bit of a mouthful, but good for PowerShell verbs
        dism /image=$ImagePath /is-serviceable
    }
    if ($?)
    {
        Write-Host "Serviceability tests have succeeded. The image is valid."
    }
    else
    {
        Write-Host "Serviceability tests have failed. The image is not valid."
        if (($scratchDir -ne "") -and (Test-Path -Path "$scratchDir"))
        {
            Write-Host "Removing temporary directory..."
            Remove-Item -Path "$scratchDir" -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
            Remove-Item -Path "$driveLetter\`$DISMTOOLS.~LS" -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
        }
    }
}

function New-BootFiles
{
    <#
        .SYNOPSIS
            Creates boot files compatible with BIOS/UEFI systems
        .PARAMETER drLetter
            The drive letter containing the Windows installation
        .PARAMETER bootPart
            The letter of the boot (System Reserved) partition. A value of "auto" can be passed to let the script determine the boot partition
        .PARAMETER diskId
            The index of a disk
        .PARAMETER cleanDrive
            Determine whether to run detections for specific boot scenarios
        .EXAMPLE
            New-BootFiles -drLetter "C:" -bootPart "auto" -diskId 0 -cleanDrive $false
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string]$drLetter,
        [Parameter(Mandatory = $true, Position = 1)] [string]$bootPart,
        [Parameter(Mandatory = $true, Position = 2)] [int]$diskId,
        [Parameter(Mandatory = $true, Position = 3)] [bool]$cleanDrive
    )
    if ($env:firmware_type -eq "UEFI")
    {
        # Make boot files for both BIOS and UEFI firmwares
        if ($bootpart -eq "auto")
        {
            if (-not $cleanDrive)
            {
                foreach ($disk in $(Get-CimInstance -ClassName Win32_DiskPartition))
                {
                    if (($disk.DiskIndex -eq $diskId) -and ($disk.BootPartition))
                    {
                        $MSRAssign = @'
                        sel dis #DISKID#
                        sel par #VOLNUM#
                        ass letter w
                        exit
'@
                        $MSRAssign = $MSRAssign.Replace("#DISKID#", $diskId).Trim()
                        $MSRAssign = $MSRAssign.Replace("#VOLNUM#", $($disk.Index + 1)).Trim()
                        $MSRAssign | Out-File "X:\files\diskpart\dp_bootassign.dp" -Force -Encoding utf8
                        diskpart /s "X:\files\diskpart\dp_bootassign.dp" | Out-Host
                    }
                }
            }
            bcdboot "$($drLetter):\Windows" /s "W:" /f ALL
        }
        else
        {
            bcdboot "$($drLetter):\Windows" /s "W:" /f ALL
        }
    }
    else
    {
        # Install boot sector and make boot files for BIOS
        if ($bootpart -eq "auto")
        {
            if (-not $cleanDrive)
            {
                foreach ($disk in $(Get-CimInstance -ClassName Win32_DiskPartition))
                {
                    if (($disk.DiskIndex -eq $diskId) -and ($disk.BootPartition))
                    {
                        $MSRAssign = @'
                        sel dis #DISKID#
                        sel par #VOLNUM#
                        ass letter w
                        exit
'@
                        $MSRAssign = $MSRAssign.Replace("#DISKID#", $diskId).Trim()
                        $MSRAssign = $MSRAssign.Replace("#VOLNUM#", $($disk.Index + 1)).Trim()
                        $MSRAssign | Out-File "X:\files\diskpart\dp_bootassign.dp" -Force -Encoding utf8
                        diskpart /s "X:\files\diskpart\dp_bootassign.dp" | Out-Host
                    }
                }
            }
            bootsect /nt60 W:
            bootsect /nt60 W: /mbr
            bcdboot "$($drLetter):\Windows" /s "W:" /f BIOS
        }
        else
        {
            bootsect /nt60 W:
            bootsect /nt60 W: /mbr
            bcdboot "$($drLetter):\Windows" /s "W:" /f BIOS
        }
    }
}

function Show-Timeout {
    <#
        .SYNOPSIS
            Displays a timeout for the amount of seconds given
        .PARAMETER seconds
            The number of seconds of the timeout. This must be a non-zero, positive number
        .EXAMPLE
            Show-Timeout -seconds 15
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [int]$seconds
    )
    for ($i = 0; $i -lt $seconds; $i++)
    {
        Write-Progress -Activity "Restarting system..." -Status "Your system will restart in $($seconds - $i) seconds" -PercentComplete (($i / $seconds) * 100)
        Start-Sleep -Seconds 1
    }
    Write-Progress -Activity "Restarting system..." -Status "Restarting your system" -PercentComplete 100
}

function Start-ProjectDevelopment {
    $mountDirectory = ""
    $architecture = [PE_Arch]::($testArch)
    $version = "0.6"
    $ESVer = "0.6"
    Write-Host "DISMTools $version - Preinstallation Environment Helper"
    Write-Host "(c) 2024. CodingWonders Software"
    Write-Host "-----------------------------------------------------------"
    # Start PE generation
    Write-Host "Starting project creation... (Extensibility Suite version $ESVer)"
    # Detect if the Windows ADK is present
    try
    {
        if ((Get-ItemPropertyValue -Path 'HKLM:\SOFTWARE\Microsoft\WIMMount' -Name 'AdkInstallation') -eq 1)
        {
            # An ADK may be installed, but it may not be Windows 10 ADK
            $progFiles = ""
            $peToolsPath = ""
            if ([Environment]::Is64BitOperatingSystem)
            {
                $progFiles = "$([IO.Path]::GetPathRoot([Environment]::GetFolderPath([Environment+SpecialFolder]::Windows)))Program Files (x86)"
            }
            else
            {
                $progFiles = "$([IO.Path]::GetPathRoot([Environment]::GetFolderPath([Environment+SpecialFolder]::Windows)))Program Files"
            }
            if (Test-Path "$progFiles\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment")
            {
                $peToolsPath = "$progFiles\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment"
                if (-not (Test-Path "$targetPath"))
                {
                    New-Item -Path "$targetPath" -ItemType Directory | Out-Null
                }
                Write-Host "Creating working directory and copying Preinstallation Environment (PE) files..."
                if ((Copy-PEFiles -peToolsPath "$progFiles\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment" -architecture $architecture -targetDir "$((Get-Location).Path)\ISOTEMP") -eq $false)
                {
                    Write-Host "Preinstallation Environment creation has failed in the PE file copy phase."
                    Write-Host "`nPress ENTER to exit"
                    Read-Host | Out-Null
                    exit 1
                }
                Write-Host "Creating temporary mount directory..."
                try
                {
                    $mountDirectory = "$env:TEMP\DISMTools_PE_Scratch_$((Get-Date).ToString("MM-dd-yyyy_HH-mm-ss"))_$(Get-Random -Maximum 10000)"
                    New-Item "$mountDirectory" -ItemType Directory | Out-Null
                }
                catch
                {
                    Write-Host "Could not create temporary mount directory. Using default folder..."
                    $mountDirectory = "$((Get-Location).Path)\ISOTEMP\mount"
                }
                Write-Host "Mounting Windows image. Please wait..."
                if ((Start-DismCommand -Verb Mount -ImagePath "$((Get-Location).Path)\ISOTEMP\media\sources\boot.wim" -ImageIndex 1 -MountPath "$mountDirectory") -eq $false)
                {
                    Write-Host "Preinstallation Environment creation has failed in the PE image mount phase."
                    Write-Host "`nPress ENTER to exit"
                    Read-Host | Out-Null
                    exit 1
                }
                Write-Host "Copying Windows PE optional components. Please wait..."
                if ((Copy-PEComponents -peToolsPath "$progFiles\Windows Kits\10\Assessment and Deployment Kit\Windows Preinstallation Environment" -architecture $architecture -targetDir "$((Get-Location).Path)\ISOTEMP") -eq $false)
                {
                    Write-Host "Preinstallation Environment creation has failed in the PE optional component copy phase."
                    Write-Host "`nPress ENTER to exit"
                    Read-Host | Out-Null
                    exit 1
                }
                Write-Host "Adding OS packages..."
                $pkgs = [List[string]]::new()
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\WinPE-NetFx.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\en-US\WinPE-NetFx_en-us.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\WinPE-WMI.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\en-US\WinPE-WMI_en-us.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\WinPE-PowerShell.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\en-US\WinPE-PowerShell_en-us.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\WinPE-DismCmdlets.cab")
                $pkgs.Add("$((Get-Location).Path)\ISOTEMP\OCs\en-US\WinPE-DismCmdlets_en-us.cab")
                foreach ($pkg in $pkgs)
                {
                    if (Test-Path $pkg -PathType Leaf)
                    {
                        Write-Host "Adding OS package $([IO.Path]::GetFileNameWithoutExtension($pkg))..."
                        Start-DismCommand -Verb Add-Package -ImagePath "$mountDirectory" -PackagePath $pkg | Out-Null
                    }
                }
                Write-Host "Saving changes..."
                Start-DismCommand -Verb Commit -ImagePath "$mountDirectory" | Out-Null
                # Perform customization tasks later
                Write-Host "Beginning customizations..."
                if ((Start-PECustomization -ImagePath "$mountDirectory" -arch $architecture -testStartNet $true) -eq $false)
                {
                    Write-Host "Preinstallation Environment creation has failed in the PE customization phase. Discarding changes..."
                    Start-DismCommand -Verb Unmount -ImagePath "$mountDirectory" -Commit $false | Out-Null
                    Write-Host "`nPress ENTER to exit"
                    Read-Host | Out-Null
                    exit 1
                }
                Write-Host "Unmounting image..."
                Start-DismCommand -Verb Unmount -ImagePath "$mountDirectory" -Commit $true | Out-Null
                Write-Host "PE generated successfully"
                Write-Host "Copying project files..."
                # Copy project files
                Expand-Archive -Path "$((Get-Location).Path)\files\DISMTools-PE.zip" -Destination "$targetPath" -Force
                Write-Host "Project files have been copied."
                if ([Environment]::Is64BitOperatingSystem)
                {
                    Copy-Item -Path "$peToolsPath\..\Deployment Tools\amd64\Oscdimg\oscdimg.exe" -Destination "$targetPath\ISORoot\oscdimg.exe" -Force -Verbose
                }
                else
                {
                    Copy-Item -Path "$peToolsPath\..\Deployment Tools\x86\Oscdimg\oscdimg.exe" -Destination "$targetPath\ISORoot\oscdimg.exe" -Force -Verbose
                }
                Write-Host "Copying setup tools..."
                Copy-Item -Path "$((Get-Location).Path)\PE_Helper.ps1" -Destination "$((Get-Location).Path)\ISOTEMP\media" -Verbose -Force -Recurse -Container -ErrorAction SilentlyContinue
                New-Item -Path "$((Get-Location).Path)\ISOTEMP\media\files\diskpart" -ItemType Directory | Out-Null
                Copy-Item -Path "$((Get-Location).Path)\files\diskpart\*.dp" -Destination "$((Get-Location).Path)\ISOTEMP\media\files\diskpart" -Verbose -Force -Recurse -Container -ErrorAction SilentlyContinue
                New-Item -Path "$((Get-Location).Path)\ISOTEMP\media\Tools\DIM" -ItemType Directory | Out-Null
                Copy-Item -Path "$((Get-Location).Path)\tools\DIM\*" -Destination "$((Get-Location).Path)\ISOTEMP\media\Tools\DIM" -Verbose -Force -Recurse -Container -ErrorAction SilentlyContinue
                Write-Host "Deleting temporary files..."
                Remove-Item -Path "$((Get-Location).Path)\ISOTEMP\OCs" -Recurse -Force -ErrorAction SilentlyContinue
                if ($mountDirectory.StartsWith("$env:TEMP"))
                {
                    Remove-Item -Path "$mountDirectory" -Recurse -Force -ErrorAction SilentlyContinue
                }
                if ($?)
                {
                    Write-Host "Temporary files have been deleted successfully"
                }
                else
                {
                    Write-Host "Temporary files haven't been deleted successfully"
                }
                Write-Host "The ISO file structure has been successfully created. DISMTools will finish preparing the project after 5 seconds."
                Start-Sleep -Seconds 5
                Copy-Item -Path "$((Get-Location).Path)\ISOTEMP\*" -Destination "$targetPath\ISORoot" -Recurse -Force -Verbose -ErrorAction SilentlyContinue
                if ($?)
                {
                    Write-Host "Deleting temporary files..."
                    Remove-Item -Path "$((Get-Location).Path)\ISOTEMP" -Recurse -Force -ErrorAction SilentlyContinue
                    # Delete local DIM src directory - not needed
                    if (Test-Path "$targetPath\ISORoot\media\Tools\DIM\src") { Remove-Item -Path "$targetPath\ISORoot\media\Tools\DIM\src" -Recurse -Force -ErrorAction SilentlyContinue | Out-Null }
                    Write-Host "The project has been successfully created"
                    try
                    {
                        Write-Host "Mounting Windows PE image..."
                        Mount-WindowsImage -ImagePath "$targetPath\ISORoot\media\sources\boot.wim" -Index 1 -Path "$targetPath\mount"
                        Write-Host "Updating the project configuration..."
                        $dtProjConfig = Get-Content -Path "$targetPath\settings\project.ini"
                        # Only update image file, index, and mount point configs. Let DISMTools configure the rest.
                        $dtProjConfig[6] = "ImageFile=`"$targetPath\ISORoot\media\sources\boot.wim`""
                        $dtprojConfig[7] = "ImageIndex=1"
                        $dtProjConfig[8] = "ImageMountPoint=`"$targetPath\mount`""
                        Set-Content -Path "$targetPath\settings\project.ini" -Value $dtprojConfig -Force
                        Write-Host "`nThe generation process is complete! You can start testing your applications on Windows PEs."
                    }
                    catch
                    {
                        Write-Host "Could not mount the target Windows PE image. You will have to do this manually."
                    }
                }
                else
                {
                    Write-Host "Could not finish preparing the project."
                }
                Start-Sleep -Seconds 5
                exit 0
            }
            else
            {
                Write-Host "A Windows Assessment and Deployment Kit (ADK) could not be found on your system. Please install the Windows ADK for Windows 10 (or Windows 11), and its Windows PE plugin, and try again."
                Write-Host "`nPress ENTER to exit"
                Read-Host | Out-Null
                exit 1
            }
        }
        else
        {
            Write-Host "A Windows Assessment and Deployment Kit (ADK) could not be found on your system. Please install the Windows ADK for Windows 10 (or Windows 11), and its Windows PE plugin, and try again."
            Write-Host "`nPress ENTER to exit"
            Read-Host | Out-Null
            exit 1
        }
    }
    catch
    {
        Write-Host "This process is unsuccessful as the following error occurred: $_"
        Write-Host "`nPress ENTER to exit"
        Read-Host | Out-Null
        exit 1
    }
}

$host.UI.RawUI.WindowTitle = "DISMTools - Preinstallation Environment Helper"

if ($cmd -eq "StartApply")
{
    Start-OSApplication
}
elseif ($cmd -eq "StartPEGen")
{
    Start-PEGeneration
}
elseif ($cmd -eq "StartDevelopment")
{
    Start-ProjectDevelopment
}
elseif ($cmd -eq "Help")
{
    # Show help documentation
    Write-Host "DISMTools - Preinstallation Environment Helper"
    Write-Host "(c) 2024. CodingWonders Software"
    Write-Host "-----------------------------------------------------------`n"

    Write-Host "Usage: PE_Helper.ps1 {-cmd} [StartPEGen -arch <arch> -imgFile <imgFile> -isoPath <isoPath>] [StartApply] [StartDevelopment -testArch <arch> -targetPath <targetPath>] [Help]`n"
    Write-Host " -cmd: Specifies the command to run. Typing this is optional. Valid options: StartPEGen, StartApply, Help`n"
    Write-Host "    StartPEGen: starts the Preinstallation Environment (PE) generation process. Parameters:"
    Write-Host "      -arch: (Mandatory) Specifies the architecture of the target Preinstallation Environment (PE). Valid options:"
    Write-Host "             x86, amd64, arm, arm64"
    Write-Host "      -imgFile: (Mandatory) Specifies the WIM file to copy to the target Preinstallation Environment (PE)"
    Write-Host "      -isoPath: (Mandatory) Specifies the target path of the ISO file"
    Write-Host "      You need the Windows ADK and the PE plugin, which you can download here:"
    Write-Host "        https://learn.microsoft.com/en-us/windows-hardware/get-started/adk-install"
    Write-Host "    StartApply: starts the Windows image application process from the Preinstallation Environment (PE). Parameters: none"
    Write-Host "      This can only be run on Windows PE. Starting this action on other environments will fail."
    Write-Host "    StartDevelopment: starts the PE project creation phase. Parameters:"
    Write-Host "      -testArch: (Mandatory) Specifies the architecture of the target Preinstallation Environment (PE). Valid options:"
    Write-Host "                 x86, amd64, arm, arm64"
    Write-Host "      -targetPath: (Mandatory) Specifies the target path for the PE project"
    Write-Host "    Help: shows this help documentation`n"

    Write-Host "Examples:`n"
    Write-Host "    PE_Helper.ps1 [-cmd] StartPEGen -arch amd64 -imgFile `"C:\Whatever.wim`" -isoPath `"C:\dt_pe.iso`""
    Write-Host "    PE_Helper.ps1 [-cmd] StartApply"
    Write-Host "    PE_Helper.ps1 [-cmd] StartDevelopment -testArch amd64 -targetPath `"C:\FooBar`""
    Write-Host "    PE_Helper.ps1 [-cmd] Help"
}
else
{
    Write-Host "Invalid command. Available commands: StartApply (begins OS application), StartPEGen (begins custom PE generation), StartDevelopment, Help"
    exit 1
}
