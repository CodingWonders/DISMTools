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
    $diskGetterDpScript = @'
    lis dis
    exit
'@
    New-Item -Path "$env:SYSTEMDRIVE\files\diskpart" -ItemType Directory -Force | Out-Null
    $diskGetterDpScript | Out-File "$env:SYSTEMDRIVE\files\diskpart\dp_listdisk.dp" -Force -Encoding utf8
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
    if (Test-Path "$env:SYSTEMDRIVE\HotInstall") {
        $msg = "$msg`n`nIf you reboot your computer right after disk configuration is written, you will need to boot to installation media in order to install an operating system."
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
            if (Test-Path "$env:SYSTEMDRIVE\HotInstall") {
                $msg = "$msg`n`nIf you reboot your computer right after disk configuration is written, you will need to boot to installation media in order to install an operating system."
            }
            Write-Host $msg -BackgroundColor Black -ForegroundColor Yellow
            $choice = Read-Host "Are you sure you want to continue (Y/N)"
        } until ($choice -eq "Y")
    }
    $driveLetter = ""
    $bootLetter = ""
    if ($partition -eq 0)
    {
        # Proceed with default disk configuration
        $diskLayout = Write-DiskConfiguration $drive $true $partition
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
        Write-DiskConfiguration $drive $false $partition
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
        $bootLetter = "W"
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
    $driverPath = "$env:SYSTEMDRIVE\DT_InstDrvs.txt"
    if ((Test-Path "$($driveLetter):\`$DISMTOOLS.~LS") -and ($serviceableArchitecture) -and (Test-Path -Path $driverPath -PathType Leaf))
    {
        Write-Host "Adding drivers to the target image..."
        # Add drivers that were previously added to the Windows PE using the DIM
        $drivers = (Get-Content -Path $driverPath | Where-Object { $_.Trim() -ne "" })
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
    New-BootFiles -drLetter $driveLetter -bootPart "auto" -diskId $drive -cleanDrive $($partition -eq 0) -espLetter $bootLetter
    Start-Sleep -Milliseconds 250
    Clear-Host
    Write-Host "`n`n`n`n`n`n`n`n`n`n"
    # Write-Host "Choose an option:`n"
    #Write-Host "  1 - FOG"
    #Write-Host "      Choose this method if you started the Preinstallation Environment using local media, such as"
    #Write-Host "      DVD or USB drives. This is recommended for newcomers."
    #Write-Host "  2 - WDS"
    #Write-Host "      Choose this method if you started the Preinstallation Environment using a network-based"
    #Write-Host "      deployment solution. This is recommended for system administrators that want to deploy a system"
    #Write-Host "      image to multiple computers at once."
    #Write-Host "You will not be able to go back to choose another option after making your decision. You must reboot your"
    #Write-Host "computer and select the correct option. You can also restart your computer by closing this window.`n"
    #$option = Read-Host -Prompt "Choose an capture method by typing the option and pressing ENTER"
    #switch ($option) {
    #    "1" {
    #        New-Item -Path "./scripts/imagecapture.ps1" -ErrorAction SilentlyContinue | Out-Null
    #    }
    #    "2" {
    #        New-Item -Path "./scripts/installwds.ps1" -ErrorAction SilentlyContinue | Out-Null
    #    }
    # }
    
    New-Item -Path "./scripts/imagecapture.ps1" -ErrorAction SilentlyContinue | Out-Null
