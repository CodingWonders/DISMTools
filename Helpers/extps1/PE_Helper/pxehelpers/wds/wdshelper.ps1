#requires -version 5.0
#requires -runasadministrator
#                                              ....
#                                         .'^""""""^.
#      '^`'.                            '^"""""""^.
#     .^"""""`'                       .^"""""""^.                ---------------------------------------------------------
#      .^""""""`                      ^"""""""`                  | DISMTools 0.8                                         |
#       ."""""""^.                   `""""""""'           `,`    | The connected place for Windows system administration |
#         '`""""""`.                 """""""""^         `,,,"    ---------------------------------------------------------
#            '^"""""`.               ^""""""""""'.   .`,,,,,^    | PE Helper - Windows Deployment Services Helper        |
#              .^"""""`.            ."""""""",,,,,,,,,,,,,,,.    ---------------------------------------------------------
#                .^"""""^.        .`",,"""",,,,,,,,,,,,,,,,'     | (C) 2025-2026 CodingWonders Software                  |
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

. "$PSScriptRoot\..\Common\PXEHelpers.Common.ps1"
. "$env:SYSTEMDRIVE\DTPE.PolicyHelper.ps1"

class ServerAuthentication {
    [string]$serverIP
    [string]$serverPort
    [string]$serverUser
    [string]$serverPassword

    ServerAuthentication($ip, $port, $user, $password) {
        $this.serverIP = $ip
        $this.serverPort = $port
        $this.serverUser = $user
        $this.serverPassword = $password

        if ((Test-IPAddressSyntax -ipAddr $this.serverIP) -eq [IPAddress]::IPv6) {
            $this.serverIP = "[$($this.serverIP)]"
        }
    }
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

class DiskLayout {
    [string]$espVolume
    [string]$bootVolume
    [string]$recoveryVolume

    DiskLayout([string]$esp, [string]$boot, [string]$recovery) {
        $this.espVolume = $esp
        $this.bootVolume = $boot
        $this.recoveryVolume = $recovery
    }
}

enum PartitionTableOverride {
    NoOverride = 0
    AlwaysMBR = 1
    AlwaysGPT = 2
}

$global:override = [PartitionTableOverride]::NoOverride

$pxeReplySettings = $null

$global:destShareLetter = ""

function Invoke-ServerAuthentication {
    if ($pxeReplySettings -ne $null) {
        Write-Host "    We have detected the IP address of the WDS server you started this environment from ($($pxeReplySettings['serverAddr'])). If you started the WDS Helper Server on this server, you can use this address here."
        Write-Host "    The IP address assigned to this device is $($pxeReplySettings['clientAddr']).`n"
    }
    
    $validAddress = $false
    $validPort = $false
    
    $ip = ""
    $port = 0
    $user = ""
    
    $ipMessage = "Please enter the IP address of the server"
    
    if ($pxeReplySettings -ne $null) {
        $ipMessage += ", or press ENTER to use this IP address [$($pxeReplySettings['serverAddr'])]"
    }
    
    do {
        $ip = Read-Host -Prompt "$ipMessage"
        
        if (($ip -eq "") -and ($pxeReplySettings -ne $null)) {
            Write-Host "Using $($pxeReplySettings['serverAddr']) as the server..."
            $ip = $pxeReplySettings["serverAddr"]
        }
        
        if ((Test-IPAddressSyntax -ipAddr $ip) -ne [IPAddress]::Unknown) {
            $validAddress = $true
        }
    } until ($validAddress -eq $true)
    
    $defaultPort = Get-PolicyValue -PolicyName "PXEServerPort" -DefaultPolicyValue 8080 -ValidOptions @(80..65535)
    
    do {
        $portStr = Read-Host -Prompt "Please enter the port to which the WDS Helper Server is listening, or press ENTER to use the default port [$($defaultPort)]"
        
        # if we haven't input anything here we'll make it 8080, if we don't do anything it will default to 0
        if ($portStr -eq "") {
            Write-Host "Using default port..."
            $portStr = [string]$defaultPort
        }
        
        try {
            $port = [int]$portStr
            $validPort = $true
        } catch {
            Write-Host "The port needs to be numeric. Try again."
            $validPort = $false
        }
    } until ($validPort -eq $true)
    
    do {
        $user = Read-Host -Prompt "Please enter the user name for server authentication"
        
        if ($user -eq "") {
            Write-Host "Please enter a user."
        }
    } until ($user -ne "")
    
    $password = Read-Host -Prompt "Please enter the password for server authentication (WILL BE SHOWN!)"
    return [ServerAuthentication]::new($ip, $port, $user, $password)
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
            return "aarch64"
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
    Show-SectionMessage -sectionTitle "Perform disk configuration" -sectionDescription "Select the disk on which you want to perform the installation."

    # Show disk list with diskpart
    if (Test-Path "$env:SYSTEMDRIVE\files\diskpart\dp_listdisk.dp" -PathType Leaf)
    {
        diskpart /s "$env:SYSTEMDRIVE\files\diskpart\dp_listdisk.dp" | Out-Host
    }
    else
    {
        Write-Host "DISKPART scripts not found."
        return "ERROR"
    }

    # Show additional tools
    Write-Host "- To load drivers, type `"DIM`" and press ENTER"
    Write-Host "- To reload results, press R"
    Write-Host ""

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
                        Write-Host "Starting the Driver Installation Module...`n`nYou will go back to the disk selection screen after closing the program."
                        Start-Process -FilePath "$env:SYSTEMDRIVE\Tools\DIM\$systemArchitecture\DT-DIM.exe" -Wait
                    }
                }
                Get-Disks
            }
            "R" {
                # Refresh results
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
        .PARAMETER override
            The partition table override mode
        .EXAMPLE
            Get-Partitions 0
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [int]$driveNum,
        [Parameter(Position = 1)] [PartitionTableOverride]$override = [PartitionTableOverride]::NoOverride
    )
    
    Show-SectionMessage -sectionTitle "Perform disk configuration" -sectionDescription "These are the partitions in disk $($driveNum)."

    $partLister = @'
    sel dis <REPLACEME>
    lis par
    exit
'@
    $partLister = $partLister.Replace("<REPLACEME>", $driveNum).Trim()
    $partLister | Out-File -FilePath "$env:SYSTEMDRIVE\files\diskpart\dp_listpart.dp" -Force -Encoding utf8
    $part = -1
    diskpart /s "$env:SYSTEMDRIVE\files\diskpart\dp_listpart.dp" | Out-Host
    Write-Host ""
    Write-Host "- If the selected disk contains no partitions, press ENTER. Otherwise, type a partition number."
    Write-Host "- To reload results, press R"
    switch ($override) {
        NoOverride {
            Write-Host "`n    No partition table overrides have been set. The selected disk will use MBR on BIOS systems and GPT on UEFI systems.`n"
        }
        AlwaysMBR {
            Write-Host "`n    On a clean drive, a MBR partition table will be used regardless of the platform.`n"
        }
        AlwaysGPT {
            Write-Host "`n    On a clean drive, a GPT partition table will be used regardless of the platform.`n"
        }
    }
    if ($override -eq [PartitionTableOverride]::NoOverride) {
        Write-Host "- To specify a partition table override, press O"
    } else {
        Write-Host "- To specify or change a partition table override, press O"
    }
    Write-Host "- If you have selected the wrong disk, type `"B`" now and press ENTER`n"
    $part = Read-Host -Prompt "Please choose the partition to apply the image to"
    if ($part -eq -1)
    {
        return @{"partitionNumber" = $part; "selectedOverride" = $override}
    }
    elseif ($part -eq "B")
    {
        return @{"partitionNumber" = $part; "selectedOverride" = $override}
    }
    elseif ($part -eq "O")
    {
        Show-SectionMessage -sectionTitle "Specify a partition table override setting" -sectionDescription "Configure a partition table override to use with the selected disk."
        Write-Host ""
        Write-Host "Partition table overrides let you configure a disk using a specific partition table regardless of your current"
        Write-Host "platform. This is useful if you are deploying an operating system to another computer using this computer. Do"
        Write-Host "not use partition table overrides if you want to install an operating system to a disk on this computer.`n"
        Write-Host "Partition table overrides will not be used when you specify a partition number on the partition screen. In"
        Write-Host "that case, the existing partition table will be kept."
        Write-Host ""
        Write-Host "Currently," -NoNewline
        switch ($override) {
            NoOverride {
                if ($env:FIRMWARE_TYPE -eq "UEFI") {
                    Write-Host " a GPT partition table scheme will be used because this computer uses UEFI."
                } else {
                    Write-Host " a MBR partition table scheme will be used because this computer uses BIOS."
                }
            }
            AlwaysMBR {
                Write-Host " a MBR partition table scheme will be used because of an override."
            }
            AlwaysGPT {
                Write-Host " a GPT partition table scheme will be used because of an override."
            }
        }
        Write-Host ""
        if ($override -ne [PartitionTableOverride]::NoOverride) { Write-Host "- Press C to clear the overrides" }
        if ($override -ne [PartitionTableOverride]::AlwaysMBR) { Write-Host "- Press M to set a MBR partition table override" }
        if ($override -ne [PartitionTableOverride]::AlwaysGPT) { Write-Host "- Press G to set a GPT partition table override" }
        Write-Host "- Press B to go back"
        $overrideOption = Read-Host -Prompt "Select the option that you want to use and press ENTER"
        switch ($overrideOption) {
            "C" { $override = [PartitionTableOverride]::NoOverride }
            "M" {
                $override = [PartitionTableOverride]::AlwaysMBR
                if ($env:FIRMWARE_TYPE -eq "Legacy") {
                    Write-Host "You have chosen a MBR partition table override on a computer whose firmware type already"
                    Write-Host "supports MBR. While you can keep using this override, it becomes redundant and, thus, we"
                    Write-Host "recommend that you clear this partition table override."
                    $option = Read-Host -Prompt "Do you want to clear the override? (Y/n)"
                    if ($option -ne "N") { $override = [PartitionTableOverride]::NoOverride }
                }
            }
            "G" {
                $override = [PartitionTableOverride]::AlwaysGPT
                if ($env:FIRMWARE_TYPE -eq "UEFI") {
                    Write-Host "You have chosen a GPT partition table override on a computer whose firmware type already"
                    Write-Host "supports GPT. While you can keep using this override, it becomes redundant and, thus, we"
                    Write-Host "recommend that you clear this partition table override."
                    $option = Read-Host -Prompt "Do you want to clear the override? (Y/n)"
                    if ($option -ne "N") { $override = [PartitionTableOverride]::NoOverride }
                }
            }
        }
        Get-Partitions $driveNum $override
    }
    elseif ($part -eq "R")
    {
        Get-Partitions $driveNum $override
    }
    else
    {
        try
        {
            $partition = [int]$part
            return @{"partitionNumber" = $partition; "selectedOverride" = $override}
        }
        catch
        {
            Write-Host "Please specify a number and try again.`n"
            Get-Partitions $driveNum $override
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
        .PARAMETER override
            The partition table override mode
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
        [Parameter(Mandatory = $true, Position = 2)] [int]$partId,
        [Parameter(Mandatory = $true, Position = 3)] [PartitionTableOverride]$override
    )
    
    Show-SectionMessage -sectionTitle "Performing disk configuration..." -sectionDescription "Please wait while changes made to disk configuration are saved to disk $($diskId)."

    Write-Host "Writing disk configuration. Please wait..."
    if ($cleanDrive)
    {
        $preFormatter = @"
        sel dis #DISKID#
        cle
        exit
"@
        $preFormatter = $preFormatter.Replace("#DISKID#", $diskId).Trim()
        $preFormatter | Out-File "$env:SYSTEMDRIVE\files\diskpart\dp_preformat.dp" -Force -Encoding utf8
        $dpProc = Start-Process -FilePath "$env:SYSTEMROOT\system32\diskpart.exe" -ArgumentList "/s `"$env:SYSTEMDRIVE\files\diskpart\dp_preformat.dp`"" -Wait -PassThru -NoNewWindow

        $espLetter = "W"
        $bootLetter = "C"
        $recoveryLetter = "R"
        $usedLetters = 0
        $espUsed = $false
        $bootUsed = $false
        $recoveryUsed = $false

        Write-Host "Checking letters of mounted drives for conflicts..."

        # One of the three letters mentioned above may be already in use. Check these before assuming they're our targets.
        # This is more the case when you boot the ISO with Ventoy
        if ((Get-Volume | Where-Object { $_.DriveLetter -eq $espLetter } | Select-Object -ExpandProperty DriveLetter).Count -gt 0) {
            Write-Host "The default letter for the EFI System Partition is already in use."
            $usedLetters++
            $espUsed = $true
        }

        if ((Get-Volume | Where-Object { $_.DriveLetter -eq $bootLetter } | Select-Object -ExpandProperty DriveLetter).Count -gt 0) {
            Write-Host "The default letter for the boot partition is already in use."
            $usedLetters++
            $bootUsed = $true
        }

        if ((Get-Volume | Where-Object { $_.DriveLetter -eq $recoveryLetter } | Select-Object -ExpandProperty DriveLetter).Count -gt 0) {
            Write-Host "The default letter for the Windows Recovery Environment partition is already in use."
            $usedLetters++
            $recoveryUsed = $true
        }

        if ($usedLetters -gt 0) {
            Write-Host "After clearing the partitions of disk $diskId, some of the drive letters are still in use by, possibly, external disks. This may cause undesired behavior."
            Write-Host "You will now be shown a list of disks, and you will be given the opportunity to reassign disk letters."
            Write-Host "These settings only apply to the disk changes in the Preinstallation Environment."

            Get-Volume | Out-Host        # let's make sure we are outputting this info

            # Ask for all the letters that are producing conflicts

            if ($espUsed) {
                $newEspLetter = Read-Host -Prompt "Provide a volume letter for the EFI System Partition, or press ENTER to use the default letter [$($espLetter)]"
                if ($newEspLetter -ne "") {
                    $espLetter = $newEspLetter
                }
            }

            if ($bootUsed) {
                $newBootLetter = Read-Host -Prompt "Provide a volume letter for the boot partition, or press ENTER to use the default letter [$($bootLetter)]"
                if ($newBootLetter -ne "") {
                    $bootLetter = $newBootLetter
                }
            }

            if ($recoveryUsed) {
                $newRecoveryLetter = Read-Host -Prompt "Provide a volume letter for the Windows Recovery Environment partition, or press ENTER to use the default letter [$($recoveryLetter)]"
                if ($newRecoveryLetter -ne "") {
                    $recoveryLetter = $newRecoveryLetter
                }
            }

        } else {
            Write-Host "No conflicts were detected after clearing the partitions of disk $diskId. Continuing with disk configuration..."
        }

        $formatter = @'
        sel dis #DISKID#
        #GPTPART#
        #MBRPART#
        exit
'@
        $formatter_gpt = @"
        conv gpt
        cre par efi size=512
        for fs=fat32 quick label="System"
        ass letter $espLetter
        cre par msr size=16
        cre par pri
        REM Prevent updates from failing to update WinRE
        shrink minimum=1024
        for quick label="Windows"
        ass letter $bootLetter
        cre par pri
        for quick label="Recovery"
        ass letter $recoveryLetter
        set id="de94bba4-06d1-4d40-a16a-bfd50179d6ac"
        gpt attributes=0x8000000000000001
"@
        $formatter_mbr = @"
        cre par pri size=100
        for quick label="System"
        ass letter $espLetter
        REM Important for MBR configurations
        active
        cre par pri
        REM Prevent updates from failing to update WinRE
        shrink minimum=1024
        for quick label="Windows"
        ass letter $bootLetter
        cre par pri
        for quick label="Recovery"
        ass letter $recoveryLetter
        set id=27
"@
        $uefiMode = ($env:firmware_type -eq "UEFI")
        $formatter = $formatter.Replace("#DISKID#", $diskId).Trim()
        switch ($override) {
            NoOverride {
                $uefiMode = ($env:firmware_type -eq "UEFI")
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
            }
            AlwaysMBR {
                $formatter = $formatter.Replace("#MBRPART#", $formatter_mbr).Trim()
                $formatter = $formatter.Replace("#GPTPART#", "REM Unused Partition Block").Trim()
            }
            AlwaysGPT {
                $formatter = $formatter.Replace("#MBRPART#", "REM Unused Partition Block").Trim()
                $formatter = $formatter.Replace("#GPTPART#", $formatter_gpt).Trim()
            }
        }
        $formatter | Out-File "$env:SYSTEMDRIVE\files\diskpart\dp_format.dp" -Force -Encoding utf8
        $dpProc = Start-Process -FilePath "$env:SYSTEMROOT\system32\diskpart.exe" -ArgumentList "/s `"$env:SYSTEMDRIVE\files\diskpart\dp_format.dp`"" -Wait -PassThru -NoNewWindow
        $finalLayout = [DiskLayout]::new($espLetter, $bootLetter, $recoveryLetter)
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
        $formatter | Out-File "$env:SYSTEMDRIVE\files\diskpart\dp_format.dp" -Force -Encoding utf8
        $dpProc = Start-Process -FilePath "$env:SYSTEMROOT\system32\diskpart.exe" -ArgumentList "/s `"$env:SYSTEMDRIVE\files\diskpart\dp_format.dp`"" -Wait -PassThru -NoNewWindow
    }
    Write-Host "Disk configuration has been written successfully."
    if ($finalLayout -ne $null) {
        return $finalLayout
    }
}

function Get-WimIndexes
{
    <#
        .SYNOPSIS
            Gets the image indexes of the Windows Imaging (WIM) file
    #>
    Import-Module Dism
    $wimPath = ""
    $wimPath = "$($driveLetter):\NetInstall\install.wim"
    (Get-WindowsImage -ImagePath "$wimPath" | Format-Table ImageIndex, ImageName) | Out-Host
    Write-Host "To get more complete information about the Windows image, type `"INFO`"`n"
    $idx = Read-Host -Prompt "Specify the image index to apply"
    try
    {
        $index = [int]$idx
        $imageCount = (Get-WindowsImage -ImagePath "$wimPath").Count
        # return $index
        if (($index -lt 1) -or ($index -gt $imageCount)) {
            Write-Host "An invalid index has been specified."
            throw
        }
        $wimFile = [TargetImage]::new($index, $wimPath)
        return $wimFile
    }
    catch
    {
        if ($idx -eq "INFO") {
            # Get the information, save it to a text file, and go back to the choices
            # We could have used a more visual way, but I fear that it won't be supported by the WinPE .NET Framework
            try
            {
                Write-Progress -Activity "Getting image information..." -Status "Preparing to get image information..." -PercentComplete 0
                $images = [List[Microsoft.Dism.Commands.WimImageInfoObject]]::new()
                $imageCount = (Get-WindowsImage -ImagePath "$wimPath").Count
                if ($imageCount -gt 0)
                {
                    for ($i = 0; $i -lt $imageCount; $i++)
                    {
                        Write-Progress -Activity "Getting image information..." -Status "Getting information about index $($i + 1) of $($imageCount)..." -PercentComplete (($i / $imageCount) * 100)
                        try
                        {
                            $images.Add($(Get-WindowsImage -ImagePath "$wimPath" -Index $($i + 1)))
                        }
                        catch
                        {
                            Write-Host "Could not get information about index $($i + 1) of the selected Windows image. Information may be incomplete."
                        }
                    }
                    Write-Progress -Activity "Getting image information..." -Completed
                    # We'll avoid showing the image path over and over again. The user has gotten it once, they don't need to get what the image is
                    # every time
                    $images | Select-Object -ExcludeProperty ImagePath | Format-List | Out-File "$env:SYSTEMDRIVE\imageinfo.txt" -Force -Encoding UTF8
                }
                if (Test-Path "$env:SYSTEMDRIVE\imageinfo.txt" -PathType Leaf)
                {
                    notepad "$env:SYSTEMDRIVE\imageinfo.txt"
                }
                Get-WimIndexes
            }
            catch
            {
                Write-Host "Could not get additional information."
                Get-WimIndexes
            }
        } else {
            Write-Host "Please specify an index and try again.`n"
            Get-WimIndexes
        }
    }
}

function Start-OSApplication {
    $diskGetterOpScript = @'
    lis dis
    exit
'@
    New-Item -Path "$env:SYSTEMDRIVE\files\diskpart" -ItemType Directory -Force | Out-Null
    $overrideStr = Get-PolicyValue -PolicyName "PartTableOverridePreference" -DefaultPolicyValue "NoOverride" -ValidOptions @("NoOverride", "AlwaysMBR", "AlwaysGPT")
    switch ($overrideStr) {
        "NoOverride" { $global:override = [PartitionTableOverride]::NoOverride }
        "AlwaysMBR" { $global:override = [PartitionTableOverride]::AlwaysMBR }
        "AlwaysGPT" { $global:override = [PartitionTableOverride]::AlwaysGPT }
    }
    $diskGetterOpScript | Out-File "$env:SYSTEMDRIVE\files\diskpart\dp_listdisk.dp" -Force -Encoding utf8
    $drive = Get-Disks
    if ($drive -eq "ERROR")
    {
        Write-Host "Script has failed."
        return
    }
    Write-Host "Selected disk: disk $($drive)"
    $partitionOptions = Get-Partitions $drive $override
    if ($partitionOptions["partitionNumber"] -eq "B")
    {
        do {
            $drive = Get-Disks
            if ($drive -eq "ERROR")
            {
                Write-Host "Script has failed."
                return
            }
            Write-Host "Selected disk: disk $($drive)"
            $partitionOptions = Get-Partitions $drive $override
        } until ($partitionOptions["partitionNumber"] -ne "B")
    }
    if ($partitionOptions["partitionNumber"] -eq 0)
    {
        $msg = "This will perform disk configuration changes on disk $drive. THIS WILL DELETE ALL PARTITIONS IN IT. IF YOU ARE NOT WILLING TO LOSE DATA, DO NOT CONTINUE."
    }
    else
    {
        $msg = "This will perform disk configuration changes on partition $($partitionOptions["partitionNumber"]). THIS WILL FORMAT IT. IF YOU ARE NOT WILLING TO LOSE DATA, DO NOT CONTINUE."
    }
    if (Test-Path "$env:SYSTEMDRIVE\HotInstall") {
        $msg = "$msg`n`nIf you reboot your computer right after disk configuration is written, you will need to boot to installation media in order to install an operating system."
    }
    Write-Host $msg -BackgroundColor Black -ForegroundColor Yellow
    $choice = Read-Host "Are you sure you want to continue (Y/N)"
    if ($choice -ne "Y")
    {
        do
        {
            $partitionOptions = Get-Partitions $drive $override
            if ($partitionOptions["partitionNumber"] -eq "B")
            {
                do {
                    $drive = Get-Disks
                    if ($drive -eq "ERROR")
                    {
                        Write-Host "Script has failed."
                        return
                    }
                    Write-Host "Selected disk: disk $($drive)"
                    $partitionOptions = Get-Partitions $drive $override
                } until ($partitionOptions["partitionNumber"] -ne "B")
            }
            if ($partitionOptions["partitionNumber"] -eq 0)
            {
                $msg = "This will perform disk configuration changes on disk $drive. THIS WILL DELETE ALL PARTITIONS IN IT. IF YOU ARE NOT WILLING TO LOSE DATA, DO NOT CONTINUE.`n"
            }
            else
            {
                $msg = "This will perform disk configuration changes on partition $partition. THIS WILL FORMAT IT. IF YOU ARE NOT WILLING TO LOSE DATA, DO NOT CONTINUE.`n"
            }
            if (Test-Path "$env:SYSTEMDRIVE\HotInstall") {
                $msg = "$msg`n`nIf you reboot your computer right after disk configuration is written, you will need to boot to installation media in order to install an operating system."
            }
            Write-Host $msg -BackgroundColor Black -ForegroundColor Yellow
            $choice = Read-Host "Are you sure you want to continue (Y/N)"
        } until ($choice -eq "Y")
    }
    $driveLetter = ""
    $bootLetter = ""

    # Get-Partitions returns a hashtable. First item is the number, second item is the override.
    $partNumber = $partitionOptions["partitionNumber"]
    $global:override = $partitionOptions["selectedOverride"]
    
    if ($partNumber -eq 0)
    {
        # Proceed with default disk configuration
        $diskLayout = Write-DiskConfiguration $drive $true $partNumber $global:override
        if ($diskLayout -ne $null) {
            # Get the volume letter that was stored in the function
            $driveLetter = $diskLayout.bootVolume
            $bootLetter = $diskLayout.espVolume
        } else {
            # Assume boot drive is C and ESP is W
            $driveLetter = "C"
            $bootLetter = "W"
        }
    }
    else
    {
        # Proceed with custom disk configuration
        Write-DiskConfiguration $drive $false $partNumber [PartitionTableOverride]::NoOverride
        $volLister = @'
        lis vol
        exit
'@
        $volLister | Out-File "$env:SYSTEMDRIVE\files\diskpart\dp_vols.dp" -Force -Encoding utf8
        diskpart /s "$env:SYSTEMDRIVE\files\diskpart\dp_vols.dp" | Out-Host
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

    Show-CenteredTextBox -Text "Downloading installation image to target drive. Please wait. This can take some time, depending on the speed of the network connection..." -MaxWidth 100 -CenterOfAll

    wpeutil createpagefile /path="$($driveLetter):\WinPEpge.sys" /size=256 | Out-Null

    New-Item -Path "$($driveLetter):\NetInstall" -ItemType Directory | Out-Null

    if ((Copy-Item -Path "$($global:destShareLetter)\$($connectionResult.output.shareFolderGuid)\install.wim" -Destination "$($driveLetter):\NetInstall\install.wim" -Force) -eq $false) {
        Show-CenteredTextBox -Text "Could not prepare the deployment of this image file." -MaxWidth 100 -CenterOfAll -ForegroundColor DarkRed
        Start-Sleep -Seconds 5
        wpeutil reboot
    }

    if (Test-Path -Path "$($global:destShareLetter)\$($connectionResult.output.shareFolderGuid)\unattend.xml" -PathType Leaf) {
        Show-CenteredTextBox -Text "Downloading unattended answer file. This can take some time, depending on the speed of the network connection..." -MaxWidth 100 -CenterOfAll
        if ((Copy-Item -Path "$($global:destShareLetter)\$($connectionResult.output.shareFolderGuid)\unattend.xml" -Destination "$($driveLetter):\NetInstall\unattend.xml" -Force) -eq $false) {
            Show-CenteredTextBox -Text "An unattended answer file was detected, but could not be downloaded. The target installation will not be unattended." -MaxWidth 75 -CenterOfAll -ForegroundColor DarkYellow
            Write-Host "`n`nPress ENTER to continue..."
            Read-Host | Out-Null
        }
    }

    if ((Get-WindowsDriver -Online).Count -gt 0) {
        Show-CenteredTextBox -Text "Drivers were detected in this boot image and are being exported in order to be applied to the target device. Please wait, this can take some time..." -MaxWidth 100 -CenterOfAll
        New-Item -Path "$($driveLetter):\NetInstall\drivers" -ItemType Directory | Out-Null
        Export-WindowsDriver -Online -Destination "$($driveLetter):\NetInstall\drivers"
        if ($?) {
            New-Item -Path "$env:SYSTEMDRIVE\DT_InstDrvs.txt" | Out-Null
            foreach ($infFile in $(Get-ChildItem -Path "$($driveLetter):\NetInstall\drivers\*.inf" -Recurse -ErrorAction SilentlyContinue)) {
                "$infFile" | Out-File -FilePath "$env:SYSTEMDRIVE\DT_InstDrvs.txt" -Append -Encoding UTF8
            }
        } else {
            Show-CenteredTextBox -Text "Drivers could not be exported. The target installation may not work correctly if there are drivers necessary for the client device." -MaxWidth 75 -CenterOfAll -ForegroundColor DarkYellow
            Write-Host "`n`nPress ENTER to continue..."
            Read-Host | Out-Null
        }
    }

    Show-SectionMessage -sectionTitle "Select the Windows image to install"
    $wimFile = Get-WimIndexes
    $serviceableArchitecture = (((Get-CimInstance -Class Win32_Processor | Where-Object { $_.DeviceID -eq "CPU0" }).Architecture) -eq (Get-WindowsImage -ImagePath "$($wimFile.wimPath)" -Index $wimFile.index).Architecture)
    $bootexStr = Get-PolicyValue -PolicyName "UEFICA23Preference" -DefaultPolicyValue "AskUser" -ValidOptions @("AskUser", "UseNever", "UseAlways")
    $usebootex = $false
    $bootexPolicyUsed = $false
    if ($bootexStr -ne "AskUser") { $bootexPolicyUsed = $true }
    switch ($bootexStr) {
        "UseNever" { $usebootex = $false }
        "UseAlways" { $usebootex = $true }
    }
    $usebootex = $false
    if (($bootexPolicyUsed -ne $true) -and ($global:override -eq [PartitionTableOverride]::NoOverride) -and ((Get-Command Confirm-SecureBootUEFI -ErrorAction SilentlyContinue) -ne $null) -and ($env:FIRMWARE_TYPE -eq "UEFI") -and (Confirm-SecureBootUEFI) -and ((bcdboot /? | Select-String "/bootex") -ne $null)) {
        Show-SectionMessage -sectionTitle "Select UEFI boot binary" -sectionDescription "Setup has detected that UEFI and Secure Boot are enabled on your computer. You can pick from 2 versions of the EFI boot binary that will later be used when creating boot files:"
        # Quick run-down: we only ask for EFI boot binary when we find Secure Boot on the system, AND
        # if the provided bcdboot supports bootex.
        Write-Host " - Boot binaries signed with the Microsoft Windows Production PCA 2011 certificate allow for broader"
        Write-Host "   compatibility with UEFI systems that have not yet received the latest Secure Boot DB and DBX updates. These"
        Write-Host "   will expire in June 2026."
        Write-Host " - Boot binaries signed with the Windows UEFI CA 2023 certificate allow for compatibility with modern systems"
        Write-Host "   that have already received the latest Secure Boot DB and DBX updates. Systems that have not yet received these"
        Write-Host "   updates will not work using these boot binaries.`n"
        if (([System.Text.Encoding]::ASCII.GetString((Get-SecureBootUEFI DB).Bytes) -match 'Windows UEFI CA 2023') -eq $true) {
            Write-Host "You may be able to use the UEFI CA 2023 binaries on this system."
        } else {
            Write-Host "You may not be able to use the UEFI CA 2023 binaries on this system."
        }
        Write-Host "`nYou need to make sure that the target image contains the required boot files if you decide to use"
        Write-Host "the new version of such files. Failure to do so can cause boot file creation issues. These usually occur"
        Write-Host "if you are deploying an image that has not yet received updated UEFI CA 2023 binaries."
        $bootOptn = Read-Host -Prompt "Do you want to use the updated UEFI CA 2023 binaries? (Y/n)"
        if ($bootOptn -eq "") { $bootOptn = "Y" }
        $usebootex = ($bootOptn -eq "Y")
    }

    Show-SectionMessage -sectionTitle "Collecting information and copying files needed for Setup" -sectionDescription "Please wait while Setup applies the Windows image. This can take some time, depending on the speed of your computer's disks."
    if ((Start-DismCommand -Verb Apply -ImagePath "$($driveLetter):\" -WimFile "$($wimFile.wimPath)" -WimIndex $wimFile.index) -eq $true)
    {
        Write-Host "The Windows image has been applied successfully."
    }
    else
    {
        Write-Host "Failed to apply the Windows image."
        return
    }
    Show-SectionMessage -sectionTitle "Initializing the Windows image" -sectionDescription "Please wait while Setup initializes your installation configuration."
    if ($serviceableArchitecture) { Set-Serviceability -ImagePath "$($driveLetter):\" } else { Write-Host "Serviceability tests will not be run: the image architecture and the PE architecture are different." }
    if (Test-Path "$($driveLetter):\NetInstall\unattend.xml" -PathType Leaf)
    {
        Write-Host "A possible unattended answer file has been detected, applying it...        " -NoNewline
        if ((Start-DismCommand -Verb UnattendApply -ImagePath "$($driveLetter):" -unattendPath "$($driveLetter):\NetInstall\unattend.xml") -eq $true)
        {
            Write-Host "SUCCESS" -ForegroundColor White -BackgroundColor DarkGreen
        }
        else
        {
            Write-Host "FAILURE" -ForegroundColor Black -BackgroundColor DarkRed
        }
    }
    $driverPath = "$env:SYSTEMDRIVE\DT_InstDrvs.txt"
    if ((Test-Path "$($driveLetter):\`$DISMTOOLS.~LS") -and ($serviceableArchitecture) -and (Test-Path -Path $driverPath -PathType Leaf))
    {
        Write-Host "Adding drivers to the target image..."
        # Add drivers that were previously added to the Windows PE using the DIM
        $drivers = (Get-Content -Path $driverPath | Where-Object { $_.Trim() -ne "" } | Select-Object -Unique)
        $drvCount = $drivers.Count
        $successfulInstallations = 0
        $failedInstallations = 0
        $failedDrivers = [List[string]]::new()
        foreach ($driver in $drivers)
        {
            $curDrvIndex = $drivers.IndexOf($driver)
            if (Test-Path -Path "$driver" -PathType Leaf)
            {
                Write-Progress -Activity "Adding drivers..." -Status "Adding driver $($curDrvIndex + 1) of $($drvCount): `"$([IO.Path]::GetFileName($driver))`"..." -PercentComplete (($curDrvIndex / $drvCount) * 100)
                if ((Start-DismCommand -Verb Add-Driver -ImagePath "$($driveLetter):\" -DriverAdditionFile "$driver" -DriverAdditionRecurse $false) -eq $true)
                {
                    $successfulInstallations++
                }
                else
                {
                    $failedInstallations++
                    # Add the driver to the failed list, so we can display it later
                    $failedDrivers.Add("$driver")
                }
            }
        }
        Write-Progress -Activity "Adding drivers..." -Completed
        # Show results
        Write-Host "==================================================================="
        Write-Host "Driver installation summary:"
        Write-Host "- Successful driver installations: $successfulInstallations"
        Write-Host "- Failed driver installations: $failedInstallations"
        Write-Host "==================================================================="
        if ($failedDrivers.Count -gt 0)
        {
            Write-Host "  Drivers that could not be installed:"
            foreach ($failedDriver in $failedDrivers)
            {
                Write-Host "  - `"$failedDriver`""
            }
        }
        Write-Host "The installer will attempt to perform serviceability tests one more time. Hold on for a bit, this will not take long..."
        # Perform serviceability tests one more time
        if ($serviceableArchitecture) { Set-Serviceability -ImagePath "$($driveLetter):\" } else { Write-Host "Serviceability tests will not be run: the image architecture and the PE architecture are different." }
    }
    if (Test-Path "$($driveLetter):\`$DISMTOOLS.~LS")
    {
        Remove-Item -Path "$($driveLetter):\`$DISMTOOLS.~LS" -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
    }

    # NetInstall apparently isn't removed after this function ends, so we move it here.
    if (Test-Path "$($driveLetter):\NetInstall") {
        Remove-Item "$($driveLetter):\NetInstall" -Recurse -Force -ErrorAction SilentlyContinue
    }

    New-BootFiles -drLetter $driveLetter -bootPart "auto" -diskId $drive -cleanDrive $($partNumber -eq 0) -espLetter $bootLetter -bootEx $usebootex -override $global:override
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
                    if ((Get-PolicyValue -PolicyName "AutoUnattendCopytoSysprep" -DefaultPolicyValue 0 -ValidOptions @(0,1)) -eq 1) {
                        New-Item -ItemType Directory -Force -Path "$ImagePath\Windows\System32\Sysprep"
                        Copy-Item -Path "$unattendPath" -Destination "$ImagePath\Windows\System32\Sysprep\unattend.xml" -Force
                    }
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
        New-Item -Path "$folderPath\Windows\Logs\DISMTools" -ItemType Directory -Force -ErrorAction SilentlyContinue | Out-Null
        # Bit of a mouthful, but good for PowerShell verbs (+ scratch dir support)
        if (Test-Path -Path "$folderPath\Windows\Logs\DISMTools")
        {
            dism /image=$ImagePath /logpath="$folderPath\Windows\Logs\DISMTools\serviceability.log" /scratchdir="$scratchDir" /is-serviceable
        }
        else
        {
            dism /image=$ImagePath /scratchdir="$scratchDir" /is-serviceable
        }
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
        .PARAMETER espLetter
            The letter of the EFI System Partition volume. By default, it's W if not specified
        .PARAMETER override
            The partition table override mode
        .PARAMETER bootEx
            Determine whether to use the Windows UEFI CA 2023 or the Microsoft Windows Production PCA 2011 boot binaries
        .EXAMPLE
            New-BootFiles -drLetter "C:" -bootPart "auto" -diskId 0 -cleanDrive $false
        .EXAMPLE
            New-BootFiles -drLetter "C:" -bootPart "auto" -diskId 0 -cleanDrive $false -espLetter "V"
    #>
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string]$drLetter,
        [Parameter(Mandatory = $true, Position = 1)] [string]$bootPart,
        [Parameter(Mandatory = $true, Position = 2)] [int]$diskId,
        [Parameter(Mandatory = $true, Position = 3)] [bool]$cleanDrive,
        [Parameter(Position = 4)] [string]$espLetter = "W",
        [Parameter(Position = 5)] [PartitionTableOverride]$override = [PartitionTableOverride]::NoOverride,
        [Parameter(Position = 6)] [bool]$bootEx = $false
    )
    
    # Old Windows images don't come with the required UEFI CA 2023 binaries, causing bcdboot
    # to fail. The files in question are in \WINDOWS\Boot\EFI_EX. So, if we can't find the _EX
    # variants of the boot files we disable UEFI CA 2023 support and inform.
    if (($bootEx -eq $true) -and (-not (Test-Path -Path "$($drLetter):\WINDOWS\Boot\EFI_EX"))) {
        Write-Warning "UEFI CA 2023 boot binaries not found on the target installation. Falling back to Microsoft Windows Production PCA 2011..."
        $bootEx = $false
    }
    
    switch ($override) {
        NoOverride {
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
                                $MSRAssign = @"
                                sel dis #DISKID#
                                sel par #VOLNUM#
                                ass letter $espLetter
                                exit
"@
                                $MSRAssign = $MSRAssign.Replace("#DISKID#", $diskId).Trim()
                                $MSRAssign = $MSRAssign.Replace("#VOLNUM#", $($disk.Index + 1)).Trim()
                                $MSRAssign | Out-File "$env:SYSTEMDRIVE\files\diskpart\dp_bootassign.dp" -Force -Encoding utf8
                                diskpart /s "$env:SYSTEMDRIVE\files\diskpart\dp_bootassign.dp" | Out-Host
                            }
                        }

                        if (Test-Path -Path "$env:SYSTEMDRIVE\HotInstall\BcdEntry" -PathType Leaf) {
                            Write-Host "Deleting BCD entry..."
                            $entryGuid = Get-Content -Path "$env:SYSTEMDRIVE\HotInstall\BcdEntry"
                            if ($entryGuid -ne "") {
                                bcdedit /delete $entryGuid | Out-Host
                            }
                        }
                    }
                    if ($bootEx) {
                        bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f ALL /bootex
                    } else {
                        # Depending on the version of BCDBOOT there may be a /offline option. If so, use it
                        # as not using it causes bcdboot to keep using the UEFI CA 2023 binary, even though
                        # we said we didn't want to.
                        if ((bcdboot /? | Select-String "/offline") -eq $null) {
                            bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f ALL
                        } else {
                            bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f ALL /offline
                        }
                    }
                }
                else
                {
                    if ($bootEx) {
                        bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f ALL /bootex
                    } else {
                        # Depending on the version of BCDBOOT there may be a /offline option. If so, use it
                        # as not using it causes bcdboot to keep using the UEFI CA 2023 binary, even though
                        # we said we didn't want to.
                        if ((bcdboot /? | Select-String "/offline") -eq $null) {
                            bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f ALL
                        } else {
                            bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f ALL /offline
                        }
                    }
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
                                $MSRAssign = @"
                                sel dis #DISKID#
                                sel par #VOLNUM#
                                ass letter $espLetter
                                exit
"@
                                $MSRAssign = $MSRAssign.Replace("#DISKID#", $diskId).Trim()
                                $MSRAssign = $MSRAssign.Replace("#VOLNUM#", $($disk.Index + 1)).Trim()
                                $MSRAssign | Out-File "$env:SYSTEMDRIVE\files\diskpart\dp_bootassign.dp" -Force -Encoding utf8
                                diskpart /s "$env:SYSTEMDRIVE\files\diskpart\dp_bootassign.dp" | Out-Host
                            }
                        }

                        if (Test-Path -Path "$env:SYSTEMDRIVE\HotInstall\BcdEntry" -PathType Leaf) {
                            Write-Host "Deleting BCD entry..."
                            $entryGuid = Get-Content -Path "$env:SYSTEMDRIVE\HotInstall\BcdEntry"
                            if ($entryGuid -ne "") {
                                bcdedit /delete $entryGuid | Out-Host
                            }
                        }
                    }
                    # We have to do this stupid thing to coax bootsect to work for BIOS
                    bootsect /nt60 "$espLetter`:"
                    bootsect /nt60 "$espLetter`:" /mbr
                    bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f BIOS
                }
                else
                {
                    bootsect /nt60 "$espLetter`:"
                    bootsect /nt60 "$espLetter`:" /mbr
                    bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f BIOS
                }
            }
        }
        AlwaysMBR {
            # Install boot sector and make boot files for BIOS
            if ($bootpart -eq "auto")
            {
                if (-not $cleanDrive)
                {
                    foreach ($disk in $(Get-CimInstance -ClassName Win32_DiskPartition))
                    {
                        if (($disk.DiskIndex -eq $diskId) -and ($disk.BootPartition))
                        {
                            $MSRAssign = @"
                            sel dis #DISKID#
                            sel par #VOLNUM#
                            ass letter $espLetter
                            exit
"@
                            $MSRAssign = $MSRAssign.Replace("#DISKID#", $diskId).Trim()
                            $MSRAssign = $MSRAssign.Replace("#VOLNUM#", $($disk.Index + 1)).Trim()
                            $MSRAssign | Out-File "$env:SYSTEMDRIVE\files\diskpart\dp_bootassign.dp" -Force -Encoding utf8
                            diskpart /s "$env:SYSTEMDRIVE\files\diskpart\dp_bootassign.dp" | Out-Host
                        }
                    }

                    if (Test-Path -Path "$env:SYSTEMDRIVE\HotInstall\BcdEntry" -PathType Leaf) {
                        Write-Host "Deleting BCD entry..."
                        $entryGuid = Get-Content -Path "$env:SYSTEMDRIVE\HotInstall\BcdEntry"
                        if ($entryGuid -ne "") {
                            bcdedit /delete $entryGuid | Out-Host
                        }
                    }
                }
                # We have to do this stupid thing to coax bootsect to work for BIOS
                bootsect /nt60 "$espLetter`:"
                bootsect /nt60 "$espLetter`:" /mbr
                bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f BIOS
            }
            else
            {
                bootsect /nt60 "$espLetter`:"
                bootsect /nt60 "$espLetter`:" /mbr
                bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f BIOS
            }
        }
        AlwaysGPT {
            # Make boot files for both BIOS and UEFI firmwares
            if ($bootpart -eq "auto")
            {
                if (-not $cleanDrive)
                {
                    foreach ($disk in $(Get-CimInstance -ClassName Win32_DiskPartition))
                    {
                        if (($disk.DiskIndex -eq $diskId) -and ($disk.BootPartition))
                        {
                            $MSRAssign = @"
                            sel dis #DISKID#
                            sel par #VOLNUM#
                            ass letter $espLetter
                            exit
"@
                            $MSRAssign = $MSRAssign.Replace("#DISKID#", $diskId).Trim()
                            $MSRAssign = $MSRAssign.Replace("#VOLNUM#", $($disk.Index + 1)).Trim()
                            $MSRAssign | Out-File "$env:SYSTEMDRIVE\files\diskpart\dp_bootassign.dp" -Force -Encoding utf8
                            diskpart /s "$env:SYSTEMDRIVE\files\diskpart\dp_bootassign.dp" | Out-Host
                        }
                    }

                    if (Test-Path -Path "$env:SYSTEMDRIVE\HotInstall\BcdEntry" -PathType Leaf) {
                        Write-Host "Deleting BCD entry..."
                        $entryGuid = Get-Content -Path "$env:SYSTEMDRIVE\HotInstall\BcdEntry"
                        if ($entryGuid -ne "") {
                            bcdedit /delete $entryGuid | Out-Host
                        }
                    }
                }
                bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f ALL
            }
            else
            {
                bcdboot "$($drLetter):\Windows" /s "$($espLetter):" /f ALL
            }
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
        [Parameter(Mandatory = $true, Position = 0)] [int]$seconds,
        [Parameter(Position = 1)] [PartitionTableOverride]$override = [PartitionTableOverride]::NoOverride
    )
    for ($i = 0; $i -lt $seconds; $i++)
    {
        if ($override -eq [PartitionTableOverride]::NoOverride) {
            Write-Progress -Activity "Restarting system..." -Status "Your system will restart in $($seconds - $i) seconds" -PercentComplete (($i / $seconds) * 100)
        } else {
            Write-Progress -Activity "Shutting down system..." -Status "Your system will shut down in $($seconds - $i) seconds" -PercentComplete (($i / $seconds) * 100)
        }
        Start-Sleep -Seconds 1
    }
    if ($override -eq [PartitionTableOverride]::NoOverride) {
        Write-Progress -Activity "Restarting system..." -Status "Restarting your system..." -PercentComplete 100
    } else {
        Write-Progress -Activity "Shutting down system..." -Status "Shutting down your system..." -PercentComplete 100
    }
}

function Test-PxeBoot {
    # First we check if we have PXE keys
    if ((Test-Path -Path "HKLM:\SYSTEM\CurrentControlSet\Control\PXE") -eq $false) {
        return $false
    }
    
    # The user may have added that key manually. We'll see if we have a reply. Get-ItemPropertyValue
    # does not respect error actions, so we'll have to intervene in a different way.
    try {
        $reply = Get-ItemPropertyValue -Path "HKLM:\SYSTEM\CurrentControlSet\Control\PXE" -Name "BootServerReply" -ErrorAction Stop
    } catch {
        return $false
    }
    
    # Next we'll check if we ACTUALLY have a reply.
    if ($reply -eq $null) {
        return $false
    }
    
    # All good.
    return $true
}

function Get-PxeIpAddresses {
    <#
        .SYNOPSIS
            Gets the client and DHCP server addresses from PXE boot server reply information.
        .OUTPUTS
            A hashtable containing both client and server addresses, a null object if PXE info could not be obtained or if the system is not in a PXE environment.
        .NOTES
            Hashtable values are in IPv4 form. IPv6 is not yet accounted for.
    #>
    
    # if we haven't booted via PXE, then don't grab anything
    if ((Test-PxeBoot) -eq $false) {
        return $null
    }
    
    $clientAddress = ""
    $serverAddress = ""
    
    try {
        $reply = Get-ItemPropertyValue -Path "HKLM:\SYSTEM\CurrentControlSet\Control\PXE" -Name "BootServerReply"
        
        # Bytes 13 through 16 contain our client address, while bytes 21 through 24 contain the server address
        $clientAddress = $reply[12..15] -join "."
        $serverAddress = $reply[20..23] -join "."
    } catch {
        return $null
    }
    
    return @{"clientAddr" = $clientAddress; "serverAddr" = $serverAddress}
}

$global:product = "Windows Deployment Services Helper"
$global:description = "This script will guide you through the process of deploying an operating system via a Windows Deployment Services server."

if ((Get-ItemPropertyValue -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion" -Name "EditionID") -ne "WindowsPE") {
    Show-CenteredTextBox -Text "This script is intended to be run in Windows PE. Please restart your device and boot into Windows PE to continue. Press ENTER to exit . . ." -MaxWidth 100 -CenterOfAll -ForegroundColor DarkRed
    Read-Host | Out-Null
    exit 1
}

$host.UI.RawUI.WindowTitle = "Preboot eXecution Environment Helpers: $($global:product)"

Show-SectionMessage -sectionTitle "Welcome to Setup." -sectionDescription "$global:description Make sure that your device meets the requirements for a network-based deployment. Afterwards, networking will be initialized."

Show-CenteredTextBox -Text "Please plug in your Ethernet cable. Make sure the device can detect the WDS server based on the scope settings you had configured when setting up the DHCP server, and that you have configured the WDS server to accept requests from unknown devices." -MaxWidth 100 -ForegroundColor DarkYellow
Write-Host "`n"
Show-CenteredTextBox -Text "Press ENTER to initialize the network stack and start the installation process . . ." -MaxWidth 100
Read-Host | Out-Null

Enable-Networking

Write-Host "Preparing to connect to the WDS server..."

# Detect if we booted to WinPE using PXE
if ((Test-PxeBoot) -eq $false) {
    Clear-Host
    Show-CenteredTextBox -Text "We have detected that you started the WDS Helper without booting the Preinstallation Environment via the Preboot Execution Environment (PXE). You may experience issues if you continue." -MaxWidth 100 -CenterOfAll -ForegroundColor DarkYellow
    Write-Host "`n"
    Write-Host "    Press Y to continue with operating system installation (not recommended)"
    Write-Host "    Press N to restart your computer. You can restart the installation process once you boot via PXE`n"
    $option = Read-Host -Prompt "Do you want to continue? (y/N)"
    if ($option -ne "y") {
        exit 1
    }
}

$pxeReplySettings = Get-PxeIpAddresses

Show-SectionMessage -sectionTitle "Connect to the server" -sectionDescription "Please start the WDS Helper Server on your WDS server. You can invoke it from either DISMTools or the autorun menu of the install disc. After startup, provide server authentication information that will be used to communicate with the server and the API."

$authInfo = Invoke-ServerAuthentication

$connectionBody = @{
    deviceID = (Get-WmiObject Win32_ComputerSystemProduct).UUID
} | ConvertTo-Json

$connectionResult = $null

$maxAttempts = Get-PolicyValue -PolicyName "WDSHCConnAttempts" -DefaultPolicyValue 5 -ValidOptions @(2..16)
$attempts = 0

do {
    try {
        Show-CenteredTextBox -Text "Connecting to the WDS server . . . (Attempt $($attempts + 1) of $maxAttempts)" -MaxWidth 100 -CenterOfAll
        $connectionResult = Invoke-RestMethod -Method Post -Body $connectionBody -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/connect"
    } catch {
        # Could not connect to the server. Try again.
    }
    $attempts++
    if ($attempts -ge $maxAttempts) {
        break
    }
} until ($null -ne $connectionResult)

if (($null -eq $connectionResult) -or ($connectionResult.output.successful -eq $false)) {
    if ($null -ne $connectionResult) {
        Show-CenteredTextBox -Text "Could not connect to the server. Reason: $($connectionResult.output.failureReason). The server has imposed a block of 2 minutes for this device. Wait 2 minutes, then try again." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
    } else {
        Show-CenteredTextBox -Text "Could not connect to the server. The server has imposed a block of 2 minutes for this device. Wait 2 minutes, then try again." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
    }
    Start-Sleep -Seconds 5
    wpeutil reboot
}

Show-CenteredTextBox -Text "Getting images from install groups in the WDS server . . ." -MaxWidth 100 -CenterOfAll
$installImages = Invoke-RestMethod -Method Get -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/installimages"

if (($installImages -eq $null) -or ($installImages.success -eq $false) -or (($installImages.images | Select-Object -ExpandProperty FileName).Count -le 0)) {
    Show-CenteredTextBox -Text "Could not get installation images. The server may have imposed a block of 2 minutes for this device. Wait 2 minutes, then try again." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
    Start-Sleep -Seconds 5
    wpeutil reboot
}


$installationImageToDeploy = ""
$installationImageGroup = ""

$imageFileValidated = $false
$imageGroupValidated = $false

$graphoView = Get-PolicyValue -PolicyName "WDSHCGraphoView" -DefaultPolicyValue 1 -ValidOptions @(0, 1)

if (-not (Test-Path -Path "$PSScriptRoot\GraphoView\wdshcGraphoView.ps1" -PathType Leaf)) {
    $graphoView = $false
}

if ($graphoView) {
    # We don't really have to show anything because we do it in a separate window
    Show-SectionMessage -sectionTitle ""
    do {
        $installImages.images | ConvertTo-Json | Out-File "$PSScriptRoot\GraphoView\images.json" -Encoding UTF8 -Force

        Push-Location -Path "$PSScriptRoot\GraphoView"
        & ".\wdshcGraphoView.ps1"
        Pop-Location

        if (Test-Path -Path "$PSScriptRoot\GraphoView\rescan") {
            Show-CenteredTextBox -Text "Getting images from install groups in the WDS server . . ." -MaxWidth 100 -CenterOfAll
            $installImages = Invoke-RestMethod -Method Get -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/installimages"

            if (($null -eq $installImages) -or ($installImages.success -eq $false) -or (($installImages.images | Select-Object -ExpandProperty FileName).Count -le 0)) {
                Show-CenteredTextBox -Text "Could not get installation images. The server may have imposed a block of 2 minutes for this device. Wait 2 minutes, then try again." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
                Start-Sleep -Seconds 5
                wpeutil reboot
            }

            Show-SectionMessage -sectionTitle ""
            continue
        }

        if (Test-Path -Path "$PSScriptRoot\GraphoView\selected.json" -PathType Leaf) {
            $selectedImage = Get-Content -Path "$PSScriptRoot\GraphoView\selected.json" | ConvertFrom-Json

            $installationImageToDeploy = $selectedImage.image
            $installationImageGroup = $selectedImage.group

            # Perform the validation to make sure the selected image file and group values exist in the server.
            $imageFiles = $installImages.images | Select-Object -ExpandProperty FileName
            $imageGroups = $installImages.images | Select-Object -ExpandProperty ImageGroup
            
            # Check if the image file exists in the overall list of images.
            if (-not ($imageFiles.Contains("$installationImageToDeploy"))) {
                Write-Host "The installation image does not exist in the server. Press ENTER to specify the file and group again."
                Read-Host | Out-Null
                continue
            }
            
            # Check if the image group exist in the overall list of groups.
            if (-not ($imageGroups.Contains("$installationImageGroup"))) {
                Write-Host "The specified installation image group does not exist in the server. Press ENTER to specify the file and group again."
                Read-Host | Out-Null
                continue
            }
            
            # Check if the selected image belongs to the selected group.
            $image = $installImages.images | Where-Object { $_.FileName -eq "$installationImageToDeploy" -and $_.ImageGroup -eq "$installationImageGroup" }
            if ($image -eq $null) {
                Write-Host "The specified installation image, `"$installationImageToDeploy`", does not appear to be in the specified installation image group, `"$installationImageGroup`"."
                Write-Host "Press ENTER to specify the file and group again."
                Read-Host | Out-Null
                continue
            }
            
            $imageFileValidated = $true
            $imageGroupValidated = $true
        }
    } until (($imageFileValidated -eq $true) -and ($imageGroupValidated -eq $true))
} else {
    Show-SectionMessage -sectionTitle "Choose an installation image" -sectionDescription "Please choose an installation image to apply to this device. Type its file name and press ENTER"
    $installImages | Select-Object -ExpandProperty images | Group-Object -Property ImageGroup | Select-Object -ExpandProperty Group | Out-Host
    do {
        $installationImageToDeploy = Read-Host -Prompt "Please type the file name of the installation image and press ENTER. Press R to refresh"
        
        if ($installationImageToDeploy -eq "R") {
            Show-CenteredTextBox -Text "Getting images from install groups in the WDS server . . ." -MaxWidth 100 -CenterOfAll
            $installImages = Invoke-RestMethod -Method Get -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/installimages"

            if (($installImages -eq $null) -or ($installImages.success -eq $false) -or (($installImages.images | Select-Object -ExpandProperty FileName).Count -le 0)) {
                Show-CenteredTextBox -Text "Could not get installation images. The server may have imposed a block of 2 minutes for this device. Wait 2 minutes, then try again." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
                Start-Sleep -Seconds 5
                wpeutil reboot
            }

            Show-SectionMessage -sectionTitle "Choose an installation image" -sectionDescription "Please choose an installation image to apply to this device. Type its file name and press ENTER"
            $installImages | Select-Object -ExpandProperty images | Group-Object -Property ImageGroup | Select-Object -ExpandProperty Group | Out-Host
            continue
        }
        
        $installationImageGroup = Read-Host -Prompt "Please type the group the desired image is in. Type `"--refresh`" to refresh the list"
        
        if ($installationImageGroup -eq "--refresh") {
            Show-CenteredTextBox -Text "Getting images from install groups in the WDS server . . ." -MaxWidth 100 -CenterOfAll
            $installImages = Invoke-RestMethod -Method Get -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/installimages"

            if (($installImages -eq $null) -or ($installImages.success -eq $false) -or (($installImages.images | Select-Object -ExpandProperty FileName).Count -le 0)) {
                Show-CenteredTextBox -Text "Could not get installation images. The server may have imposed a block of 2 minutes for this device. Wait 2 minutes, then try again." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
                Start-Sleep -Seconds 5
                wpeutil reboot
            }

            Show-SectionMessage -sectionTitle "Choose an installation image" -sectionDescription "Please choose an installation image to apply to this device. Type its file name and press ENTER"
            $installImages | Select-Object -ExpandProperty images | Group-Object -Property ImageGroup | Select-Object -ExpandProperty Group | Out-Host
            continue
        }
        
        # Perform the validation to make sure the selected image file and group values exist in the server.
        $imageFiles = $installImages.images | Select-Object -ExpandProperty FileName
        $imageGroups = $installImages.images | Select-Object -ExpandProperty ImageGroup
        
        # Check if the image file exists in the overall list of images.
        if (-not ($imageFiles.Contains("$installationImageToDeploy"))) {
            Write-Host "The installation image does not exist in the server. Press ENTER to specify the file and group again."
            Read-Host | Out-Null
            continue
        }
        
        # Check if the image group exist in the overall list of groups.
        if (-not ($imageGroups.Contains("$installationImageGroup"))) {
            Write-Host "The specified installation image group does not exist in the server. Press ENTER to specify the file and group again."
            Read-Host | Out-Null
            continue
        }
        
        # Check if the selected image belongs to the selected group.
        $image = $installImages.images | Where-Object { $_.FileName -eq "$installationImageToDeploy" -and $_.ImageGroup -eq "$installationImageGroup" }
        if ($image -eq $null) {
            Write-Host "The specified installation image, `"$installationImageToDeploy`", does not appear to be in the specified installation image group, `"$installationImageGroup`"."
            Write-Host "Press ENTER to specify the file and group again."
            Read-Host | Out-Null
            continue
        }
        
        $imageFileValidated = $true
        $imageGroupValidated = $true
    } until (($imageFileValidated -eq $true) -and ($imageGroupValidated -eq $true))
}

if (($installationImageToDeploy -ne "") -and ($installationImageGroup -ne "")) {
    Show-CenteredTextBox -Text "Preparing the deployment of the selected image file . . ." -MaxWidth 100 -CenterOfAll
    $shareBody = @{
        shareGuid = $($connectionResult.output.shareFolderGuid)
        image_group = "$installationImageGroup"
        image_name = "$installationImageToDeploy"
    } | ConvertTo-Json
    $shareResults = Invoke-RestMethod -Method Post -Body $shareBody -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/deploy"
    if ($shareResults.success) {
        Show-CenteredTextBox -Text "Mounting network share to this system . . ." -MaxWidth 100 -CenterOfAll
        net use * $($shareResults.output.mountPath) $($authInfo.serverPassword) /user:$($shareResults.output.username) /P:Yes
        # Start querying mapped network share information to get the drive letter, otherwise
        # default to Z:
        try {
            $shareSet = $false
            Get-ChildItem -Path "HKCU:\Network" | Foreach-Object {
                $shareProps = Get-ItemProperty -Path $_.PSPath
                if (($shareSet -eq $false) -and ($shareProps.RemotePath -eq "$($shareResults.output.mountPath)")) {
                    # Get actual drive letter and not the full path, like HKEY_CURRENT_USER\Network\
                    $global:destShareLetter = "$($_.Name.Replace('HKEY_CURRENT_USER\Network\', '')):"
                    $shareSet = $true
                }
            }
            if (-not $shareSet) {
                $global:destShareLetter = "Z:"
            }
        } catch {
            $global:destShareLetter = "Z:"
        }
    } else {
        Show-CenteredTextBox -Text "Could not prepare the deployment of this image file. The server may have imposed a block of 2 minutes for this device. Wait 2 minutes, then try again." -MaxWidth 100 -CenterOfAll -ForegroundColor DarkRed
        Start-Sleep -Seconds 5
        wpeutil reboot
    }
} else {
    Start-Sleep -Seconds 5
    wpeutil reboot
}

Start-OSApplication

Show-CenteredTextBox -Text "Removing temporary files and unmounting network shares..." -MaxWidth 100 -CenterOfAll
$cleanupBody = @{
    shareGuid = $($connectionResult.output.shareFolderGuid)
} | ConvertTo-Json

if (Test-Path "$($driveLetter):\NetInstall") {
    Remove-Item "$($driveLetter):\NetInstall" -Recurse -Force -ErrorAction SilentlyContinue
}
net use * /d /y | Out-Null

Invoke-RestMethod -Method Post -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/clearbyguid" -Body $cleanupBody | Out-Null

Start-Sleep -Milliseconds 250
Clear-Host
Write-Host "`n`n`n`n`n`n`n`n`n`n"
if ($global:override -eq [PartitionTableOverride]::NoOverride) {
    Write-Host "The first stage of Setup has completed, and your system will reboot automatically."
} else {
    Write-Host "The first stage of Setup has completed, and your system will shut down automatically."
}
Write-Host "If there are any bootable devices, remove those before proceeding, as your system may boot to this environment again."
if ($global:override -eq [PartitionTableOverride]::NoOverride) { Write-Host "When your computer restarts, Setup will continue." }
Show-Timeout -Seconds 10 -override $global:override
if ($global:override -eq [PartitionTableOverride]::NoOverride) {
    wpeutil reboot
} else {
    wpeutil shutdown
}
