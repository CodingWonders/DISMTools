#requires -version 5.0
#                                              ....              
#                                         .'^""""""^.            
#      '^`'.                            '^"""""""^.              
#     .^"""""`'                       .^"""""""^.                ---------------------------------------------------------
#      .^""""""`                      ^"""""""`                  | DISMTools 0.5                                         |
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
    [Parameter(Mandatory = $true, Position = 0)] [ValidateSet('StartPEGen', 'StartApply')] [string] $cmd,
    [Parameter(ParameterSetName = 'StartPEGen', Mandatory = $true, Position = 1)] [string] $arch,
    [Parameter(ParameterSetName = 'StartPEGen', Mandatory = $true, Position = 2)] [string] $imgFile,
    [Parameter(ParameterSetName = 'StartPEGen', Mandatory = $true, Position = 3)] [string] $isoPath
)

enum PE_Arch {
    x86 = 0
    amd64 = 1
    arm = 2
    arm64 = 3
}

if (([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator) -eq $false)
{
    Write-Host "You need to run this script as an administrator"
    exit 1
}

function Start-PEGeneration
{
    $architecture = [PE_Arch]::($arch)
    $version = "0.5"
    Write-Host "DISMTools $version - Preinstallation Environment Helper"
    Write-Host "(c) 2024. CodingWonders Software"
    Write-Host "-----------------------------------------------------------"
    # Start PE generation
    Write-Host "Starting PE generation..."
    # Detect if the Windows ADK is present
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
            Write-Host "Mounting Windows image. Please wait..."
            if ((Start-DismCommand -Verb Mount -ImagePath "$((Get-Location).Path)\ISOTEMP\media\sources\boot.wim" -ImageIndex 1 -MountPath "$((Get-Location).Path)\ISOTEMP\mount") -eq $false)
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
                    Start-DismCommand -Verb Add-Package -ImagePath "$((Get-Location).Path)\ISOTEMP\mount" -PackagePath $pkg | Out-Null
                }
            }
            Write-Host "Saving changes..."
            Start-DismCommand -Verb Commit -ImagePath "$((Get-Location).Path)\ISOTEMP\mount" | Out-Null
            # Perform customization tasks later
            Write-Host "Beginning customizations..."
            if ((Start-PECustomization -ImagePath "$((Get-Location).Path)\ISOTEMP\mount" -arch $architecture) -eq $false)
            {
                Write-Host "Preinstallation Environment creation has failed in the PE customization phase. Discarding changes..."
                Start-DismCommand -Verb Unmount -ImagePath "$((Get-Location).Path)\ISOTEMP\mount" -Commit $false | Out-Null
                Write-Host "`nPress ENTER to exit"
                Read-Host | Out-Null
                exit 1
            }
            Write-Host "Unmounting image..."
            Start-DismCommand -Verb Unmount -ImagePath "$((Get-Location).Path)\ISOTEMP\mount" -Commit $true | Out-Null
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

function Copy-PEFiles
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $peToolsPath,
        [Parameter(Mandatory = $true, Position = 1)] [PE_Arch] $architecture,
        [Parameter(Mandatory = $true, Position = 2)] [string] $targetDir
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
        Start-Process "$peToolsPath\copype.cmd" -ArgumentList "$architecture `"$targetDir`"" -Wait | Out-Host
        if ($?)
        {
            Write-Host "PE files copied successfully."
        }
        else
        {
            Write-Host "Failed to copy PE files."
        }
        Set-Location $og_Location
        return $?
    }
    catch
    {
        Write-Host "Failed to copy PE files."
        return $false
    }
}

function Copy-PEComponents
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $peToolsPath,
        [Parameter(Mandatory = $true, Position = 1)] [PE_Arch] $architecture,
        [Parameter(Mandatory = $true, Position = 2)] [string] $targetDir
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
            Copy-Item -Path "$peToolsPath\$($architecture.ToString())\WinPE_OCs\$file" -Destination "$targetDir\OCs" -Force -Container -PassThru -Verbose | ForEach-Object {
                $copied = ($_.BytesTransferred / $totalSize) * 100
                Write-Debug $copied
            }
        }
        foreach ($file in $loc_OCs)
        {
            Copy-Item -Path "$peToolsPath\$($architecture.ToString())\WinPE_OCs\en-US\$file" -Destination "$targetDir\OCs\en-US" -Force -Container -PassThru -Verbose | ForEach-Object {
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
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $imagePath,
        [Parameter(Mandatory = $true, Position = 1)] [PE_Arch] $arch
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
        try
        {
            Write-Host "CUSTOMIZATION STEP - Change Startup Commands" -BackgroundColor DarkGreen
            Write-Host "Changing startup commands..."
            Copy-Item -Path "$((Get-Location).Path)\files\startup\startnet.cmd" -Destination "$imagePath\Windows\system32\startnet.cmd" -Force
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
    param (
        [Parameter(Mandatory = $true, Position = 0)] [bool] $admins
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
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $regFile,
        [Parameter(Mandatory = $true, Position = 1)] [string] $regName,
        [Parameter(Mandatory = $true, Position = 2)] [bool] $regLoad
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
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $peToolsPath,
        [Parameter(Mandatory = $true, Position = 1)] [string] $isoLocation
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
        Start-Process "$env:NewPath\oscdimg.exe" -ArgumentList "-bootdata:$bootData -u2 -udfver102 `"$((Get-Location).Path)\ISOTEMP\media`" `"$isoLocation`"" -Wait | Out-Null
        if ($?)
        {
            Write-Host "ISO generation has completed successfully."            
        }
        else
        {
            Write-Host "Failed to generate an ISO file." 
        }
        return $?
    }
    catch
    {
        Write-Host "Failed to generate an ISO file."
        return $false
    }
}

function Start-OSApplication
{
    # Detect if it's run on Windows PE
    if ((Get-ItemPropertyValue -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -Name 'EditionID') -ne "WindowsPE")
    {
        Write-Host "This procedure must be run on Windows PE only."
        return
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
    if ($partition -eq 0)
    {
        # Proceed with default disk configuration
        Write-DiskConfiguration $drive $true
    }
    else
    {
        # Proceed with custom disk configuration
        Write-DiskConfiguration $drive $false
    }
    $index = Get-WimIndexes
    Write-Host "Applying Windows image. This can take some time..."
    Start-DismCommand -Verb Apply -ImagePath "C:\" -WimFile "$((Get-Location).Path)sources\install.wim" -WimIndex $index
    Set-Serviceability -ImagePath "C:\"
    New-BootFiles
    # Show message before rebooting system
    Write-Host "The first stage of Setup has completed, and your system will reboot automatically.`n`nIf there are any bootable devices, remove those.`n`nWhen your computer restarts, Setup will continue.`n"
    Show-Timeout -Seconds 15
    Write-Host "Restarting your system..."
    wpeutil reboot
}

function Get-Disks
{
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
    $destDisk = Read-Host -Prompt "Please choose the disk to apply the image to"    
    $destDrive = -1
    try
    {
        $destDrive = [int]$destDisk
        return $destDrive
    }
    catch 
    {
        Write-Host "Please specify a number and try again.`n"
        Get-Disks
    }
}

function Get-Partitions
{
    param (
        [Parameter(Mandatory = $true)] [int] $driveNum
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
    $part = Read-Host -Prompt "Please choose the partition to apply the image to. If the disk contains no partitions, leave it empty"
    if ($part -eq -1)
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
    param (
        [Parameter(Mandatory = $true, Position = 0)] [int] $diskid,
        [Parameter(Mandatory = $true, Position = 1)] [bool] $cleanDrive
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
        diskpart /s "X:\files\diskpart\dp_format.dp" | Out-Host
    }
    Write-Host "Disk configuration has been written successfully."
}

function Get-WimIndexes
{
    Import-Module Dism
    # Replace hard-coded value with dynamic one
    (Get-WindowsImage -ImagePath "$((Get-Location).Path)sources\install.wim" | Format-Table ImageIndex, ImageName) | Out-Host
    $idx = Read-Host -Prompt "Specify the image index to apply"
    try
    {
        $index = [int]$idx
        return $index
    }
    catch
    {
        Write-Host "Please specify an index and try again.`n"
        Get-WimIndexes
    }    
}

function Start-DismCommand
{
    [CmdletBinding(DefaultParameterSetName='Default')]
    param (
        [Parameter(Mandatory = $true, Position=0)] [ValidateSet('Mount', 'Commit', 'Unmount', 'Apply', 'Add-Package', 'Remove-Package', 'Enable-Feature', 'Disable-Feature', 'Add-Appx', 'Remove-Appx', 'Add-Capability', 'Remove-Capability', 'Add-Driver')] [string] $Verb,
        [Parameter(Mandatory = $true, Position=1)] [string] $ImagePath,
        # Parameters for mount command
        [Parameter(ParameterSetName='Mount', Mandatory = $true, Position = 2)] [int] $ImageIndex,
        [Parameter(ParameterSetName='Mount', Mandatory = $true, Position = 3)] [string] $MountPath,
        # Parameters for unmount command
        [Parameter(ParameterSetName='Unmount', Mandatory = $true, Position = 2)] [bool] $Commit,
        # Parameters for application command
        [Parameter(ParameterSetName='Apply', Mandatory = $true, Position=2)] [string] $WimFile,
        [Parameter(ParameterSetName='Apply', Mandatory = $true, Position=3)] [int] $WimIndex,
        # Parameters for package addition
        [Parameter(ParameterSetName='Add-Package', Mandatory = $true, Position=2)] [string] $PackagePath,
        # Parameters for package removal
        [Parameter(ParameterSetName='Remove-Package', Mandatory = $true, Position=2)] [string] $PackageName,
        # Parameters for feature enablement
        [Parameter(ParameterSetName='Enable-Feature', Mandatory = $true, Position=2)] [string] $FeatureEnablementName,
        [Parameter(ParameterSetName='Enable-Feature', Mandatory = $true, Position=3)] [string] $FeatureEnablementSource,
        # Parameters for feature disablement
        [Parameter(ParameterSetName='Disable-Feature', Mandatory = $true, Position=2)] [string] $FeatureDisablementName,
        [Parameter(ParameterSetName='Disable-Feature', Mandatory = $true, Position=3)] [bool] $FeatureDisablementRemove,
        # Parameters for AppX package addition
        [Parameter(ParameterSetName='Add-Appx', Mandatory = $true, Position=2)] [string] $AppxPackageFile,
        [Parameter(ParameterSetName='Add-Appx', Mandatory = $true, Position=3)] [string] $AppxLicenseFile,
        [Parameter(ParameterSetName='Add-Appx', Mandatory = $true, Position=4)] [string] $AppxCustomDataFile,
        [Parameter(ParameterSetName='Add-Appx', Mandatory = $true, Position=5)] [string] $AppxRegions,
        # Parameters for AppX package removal
        [Parameter(ParameterSetName='Remove-Appx', Mandatory = $true, Position=2)] [string] $AppxPackageName,
        # Parameters for capability addition
        [Parameter(ParameterSetName='Add-Capability', Mandatory = $true, Position=2)] [string] $CapabilityAdditionName,
        [Parameter(ParameterSetName='Add-Capability', Mandatory = $true, Position=3)] [string] $CapabilityAdditionSource,
        # Parameters for capability removal
        [Parameter(ParameterSetName='Remove-Capability', Mandatory = $true, Position=2)] [string] $CapabilityRemovalName,
        # Parameters for driver addition
        [Parameter(ParameterSetName='Add-Driver', Mandatory = $true, Position=2)] [string] $DriverAdditionFile,
        [Parameter(ParameterSetName='Add-Driver', Mandatory = $true, Position=3)] [bool] $DriverAdditionRecurse
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
                dism /apply-image /imagefile="$WimFile" /index=$WimIndex /applydir="$ImagePath" | Out-Host
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
                if ($DriverAdditionRecurse)
                {
                    Add-WindowsDriver -Path "$ImagePath" -Driver "$DriverAdditionFile" -Recurse -NoRestart | Out-Null
                }
                else
                {
                    Add-WindowsDriver -Path "$ImagePath" -Driver "$DriverAdditionFile" -NoRestart | Out-Null
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
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $ImagePath
    )
    Write-Host "Starting serviceability tests..."
    # Bit of a mouthful, but good for PowerShell verbs
    dism /image=$ImagePath /is-serviceable
    if ($?)
    {
        Write-Host "Serviceability tests have succeeded. The image is valid."
    }
    else
    {
        Write-Host "Serviceability tests have failed. The image is not valid."        
    }
}

function New-BootFiles
{
    if ($env:firmware_type -eq "UEFI")
    {
        # Make boot files for both BIOS and UEFI firmwares
        bcdboot "C:\Windows" /s "W:" /f ALL
    }
    else
    {
        # Install boot sector and make boot files for BIOS
        bootsect /nt60 W:
        bootsect /nt60 W: /mbr
        bcdboot "C:\Windows" /s "W:" /f BIOS        
    }
}

function Show-Timeout {
     param (
         [Parameter(Mandatory = $true, Position = 0)] [int] $seconds
     )
     for ($i = 0; $i -lt $seconds; $i++)
     {
         Write-Progress -Activity "Restarting system..." -Status "Your system will restart in $($seconds - $i) seconds" -PercentComplete (($i / $seconds) * 100)
         Start-Sleep -Seconds 1
     }
 }

if ($cmd -eq "StartApply")
{
    Start-OSApplication
}
elseif ($cmd -eq "StartPEGen")
{
    Start-PEGeneration
}
else
{
    Write-Host "Invalid command. Available commands: StartApply (begins OS application), StartPEGen (begins custom PE generation)"
    exit 1
}