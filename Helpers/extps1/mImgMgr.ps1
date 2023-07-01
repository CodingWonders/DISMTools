# DISMTools 0.3 - Mounted Image Manager CLI version

# Import the DISM module
Import-Module Dism

$global:mImage = Get-WindowsImage -Mounted
$global:selImage = 0
$newImg = 0
$selImgPath = ''
$ver = '0.3'

function Mark-Image {
    # Refresh the mounted image variable
    $global:mImage = Get-WindowsImage -Mounted
    if ($global:mImage.Count -ge 1)
    {
        Write-Host -NoNewline "Mark image number [1 - $($global:mImage.Count)] or press [B] to go back: "
    }
    else
    {
        Write-Host -NoNewline "Mark image number [1] or press [B] to go back: "        
    }

    $option = Read-Host
    if ((-not [System.String]::IsNullOrWhitespace($option)) -and $option -le $global:mImage.Count)
    {
        $global:selImage = $option
        $selImgPath = $global:mImage[$global:selImage - 1].ImagePath
        MainMenu
    }
    elseif ($option -eq "B")
    {
        MainMenu
    }
    elseif ([System.String]::IsNullOrWhiteSpace($option))
    {
        Write-Host "You have marked a non-existent mounted image"
        Mark-Image
    }
    else
    {
        Write-Host "You have marked a non-existent mounted image"
        Mark-Image
    }
}

function Unmount-Image {
    Clear-Host
    Write-Host "Image about to be unmounted: $($global:mImage[$global:selImage - 1].ImagePath)"`n
    if ($global:mImage[$global:selImage - 1].MountMode -eq 1)
    {
        Write-Host "This image is mounted with read-only permissions. You can't commit changes to it."`n
    }
    Write-Host "[C]: Commit changes and unmount"
    Write-Host "[D]: Discard changes and unmount"
    Write-Host "[S]: Unmount settings..."
    Write-Host "[B]: Back"
    $option = Read-Host `n"Choose an option and press ENTER"
    switch ($option)
    {
        "C" {
            Clear-Host
            if ($global:mImage[$global:selImage - 1].MountMode -eq 1)
            {
                Write-Host "This image is mounted with read-only permissions. Changes cannot be committed to this image."`n`n"If you want to make changes to this image, you must enable write permissions by pressing the [E] key in the main menu."`n -ForegroundColor White -BackgroundColor DarkRed
                Write-Host "Press ENTER to continue..."
                Read-Host | Out-Null
                Unmount-Image
            }
            Write-Host "Unmounting Windows image:"`n"- Image file and index: $($global:mImage[$global:selImage - 1].ImagePath) (index $($global:mImage[$global:selImage - 1].ImageIndex))"`n"- Mount directory: $($global:mImage[$global:selImage - 1].MountPath)"`n"- Operation: Commit"`n"- Additional opperations: don't check integrity, don't append to new index"
            Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Save
            Write-Host "The image should be unmounted. If you want to continue managing your mounted images, you must mark another one. Press ENTER to continue..."
            $global:selImage = 0
            Read-Host | Out-Null
            MainMenu
        }
        "D" {
            Clear-Host
            Write-Host "Unmounting Windows image:"`n"- Image file and index: $($global:mImage[$global:selImage - 1].ImagePath) (index $($global:mImage[$global:selImage - 1].ImageIndex))"`n"- Mount directory: $($global:mImage[$global:selImage - 1].MountPath)"`n"- Operation: Discard"
            Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Discard
            Write-Host "The image should be unmounted. If you want to continue managing your mounted images, you must mark another one. Press ENTER to continue..."
            $global:selImage = 0
            Read-Host | Out-Null
            MainMenu
        }
        "S" {
            Unmount-Settings $false, $false
        }
        "B" {
            MainMenu
        }
        default {
            Unmount-Image
        }
    }
}

function Unmount-Settings($checkIntegrity, $appendIndex)
{
    Clear-Host
    if ($global:mImage[$global:selImage - 1].MountMode -eq 1)
    {
        Write-Host "Unmount settings apply to the commit operation. This image was mounted with read-only permissions. Changes cannot be committed to this image."`n`n"If you want to make changes to this image, you must enable write permissions by pressing the [E] key in the main menu."`n -ForegroundColor White -BackgroundColor DarkRed
        Write-Host "Press ENTER to continue..."
        Read-Host | Out-Null
        Unmount-Image
    }
    Write-Host "Unmount settings for image: $($global:mImage[$global:selImage - 1].ImagePath)"`n
    if ($checkIntegrity)
    {
        Write-Host " [X] Check image integrity (press C to modify this setting)"
    }
    else
    {
        Write-Host " [ ] Check image integrity (press C to modify this setting)"
    }
    if ($appendIndex)
    {
        Write-Host " [X] Append changes to new image index (press A to modify this setting)"
    }
    else
    {
        Write-Host " [ ] Append changes to new image index (press A to modify this setting)"
    }
    Write-Host `n"Press the [P] key and then ENTER to commit the image with the specified settings"`n"Press the [B] key to go back"`n
    $option = Read-Host -Prompt "Choose an option and press ENTER"
    switch ($option)
    {
        "C" {
            $checkIntegrity = -not $checkIntegrity
            Unmount-Settings $checkIntegrity, $appendIndex
        }
        "A" {
            $appendIndex = -not $appendIndex
            Unmount-Settings $checkIntegrity, $appendIndex
        }
        "P" {
            $cmd = 'Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Save $(if ($checkIntegrity) { -CheckIntegrity }) $(if ($appendIndex) { -Append })'
        }
    }
}

function Enable-WritePerms {
    Clear-Host
    Write-Host "Image to enable write permissions to: $($global:mImage[$global:selImage - 1].ImagePath)"`n`n"This option will unmount the image specified and (try to) remount it with write permissions."`n`n"PLEASE BE SURE THAT THE SPECIFIED IMAGE STILL EXISTS IN ITS DIRECTORY. If it is located in an external drive or network share, make sure it is either plugged in or online at all times."`n
    $option = Read-Host "Proceed (Y/N)?"
    switch ($option)
    {
        "Y" {
            Write-Host "0  % - Unmounting specified image... (step 1 of 2)"
            Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Discard
            Write-Host "50 % - Remounting specified image with write permissions... (step 2 of 2)"
            Mount-WindowsImage -ImagePath $global:mImage[$global:selImage - 1].ImagePath -Index $global:mImage[$global:selImage - 1].ImageIndex -Path $global:mImage[$global:selImage - 1].MountPath
            Write-Host "100% - This operation has completed and the image should have been mounted with write permissions."`n`n"Press ENTER to continue..."
            Read-Host | Out-Null
            MainMenu
        }
        "N" {
            MainMenu
        }
        default {
            Enable-WritePerms
        }
    }
}

function Update-Listing {
    $global:mImage = Get-WindowsImage -Mounted
    # MainMenu
}

function Get-MenuItems {
    Write-Host "[M]: Mark image for management"
    if ($global:selImage -ne 0)
    {
        # Begin doing the actual stuff
        Write-Host "[U]: Unmount image"
        if ($global:mImage[$global:selImage - 1].MountStatus -eq 1)
        {
            Write-Host "[R]: Reload servicing"
        }
        elseif ($global:mImage[$global:selImage - 1].MountStatus -eq 2)
        {
            Write-Host "[R]: Repair component store"
        }
        if ($global:mImage[$global:selImage - 1].MountMode -eq 1)
        {
            Write-Host "[E]: Enable write permissions"
        }
        Write-Host "[A]: Access mount directory"
        if ((Get-WindowsImage -ImagePath $global:mImage[$global:selImage - 1].ImagePath | Select-Object -ExpandProperty ImageIndex).Count -gt 1)
        {
            Write-Host "[V]: Remove volume images..."
        }
    }
    Write-Host "[L]: Update mounted image listing"
    Write-Host "[X]: Exit"
}

function Detect-MountedImageIndexChanges {
    $global:mImage = Get-WindowsImage -Mounted
    if ($global:mImage[$global:selImage - 1].ImagePath -eq $selImgPath)
    {
        # All good
        return
    }
    else
    {
        if ($($global:mImage | Where-Object { $_.ImagePath -eq $selImgPath } | Select-Object -ExpandProperty ImagePath) -ne $selImgPath)
        {
            $global:selImage = 0
            Write-Host "The image you have marked has been unmounted by an external program. You can't manage this image until you mount it again, and you must mark another image now." -ForegroundColor White -BackgroundColor Red
            return
        }
        # Iterate through each mounted image so that we can switch to the appropriate one
        for ($i = 0; $i -lt $global:mImage.Count; $i++)
        {
            if ($global:mImage[$i].ImagePath -eq $selImgPath)
            {
                $global:selImage = $i + 1
                Write-Host "The image you have marked has moved indexes, so the mounted image manager switched to the index the image is now in."`n"You can continue to do your management tasks." -ForegroundColor White -BackgroundColor DarkBlue
            }
        }
    }
}

function MainMenu {
    Update-Listing
    Clear-Host
    # Print version info
    Write-Output "DISMTools $ver - Mounted image manager"
    # List mounted Windows images
    Get-WindowsImage -Mounted | Format-Table
    Write-Host `n`n`n
    if ($global:selImage -eq 0)
    {
        Write-Host "No image has been marked for management. Press the [M] key to mark a mounted image..."
    }
    else
    {
        Detect-MountedImageIndexChanges
        # Detect whether the previously marked image is 0 and refresh the menu
        if ($global:selImage -eq 0) { MainMenu }
        Write-Host "Selected image: image $global:selImage ($($global:mImage[$global:selImage - 1].ImagePath))"
    }
    Write-Host `n
    Get-MenuItems
    Write-Host `n
    $option = Read-Host -Prompt "Select an option and press ENTER"
    switch ($option)
    {
        "M" { Mark-Image }
        "U" { Unmount-Image }
        "R" {
            if ($global:mImage[$global:selImage - 1].MountStatus -eq 1)
            {
                Write-Host `n"Reloading servicing for this image..."
                Mount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Remount
                Write-Host `n"The servicing session for this image should have been reloaded."`n
                Write-Host "Press ENTER to continue..."
                Read-Host | Out-Null
                MainMenu
            }
            elseif ($global:mImage[$global:selImage - 1].MountStatus -eq 2)
            {
                Write-Host `n"Repairing the component store of this image..."
                Repair-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -RestoreHealth
                Write-Host `n"The component store of this image should have been repaired."`n
                Write-Host "Press ENTER to continue..."
                Read-Host | Out-Null
                MainMenu
            }
            else
            {
                MainMenu
            }
        }
        "E" {
            if ($global:mImage[$global:selImage - 1].MountMode -eq 0)
            {
                MainMenu
            }
            Enable-WritePerms
        }
        "A" { 
            Start-Process -FilePath $(([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Windows)) + '\explorer.exe') -ArgumentList $($global:mImage[$global:selImage - 1].MountPath)
            MainMenu
        }
        "L" {
            Write-Host `n"Updating mounted image listing..." 
            Update-Listing
            MainMenu
        }
        "X" { exit 0 }
        default { MainMenu }
    }
}

Update-Listing
MainMenu