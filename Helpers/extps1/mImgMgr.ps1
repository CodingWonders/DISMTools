#                                              ....              
#                                         .'^""""""^.            
#      '^`'.                            '^"""""""^.              
#     .^"""""`'                       .^"""""""^.                ------------------------------------------------------
#      .^""""""`                      ^"""""""`                  | DISMTools 0.3                                      |
#       ."""""""^.                   `""""""""'           `,`    | Open-source Windows image management, evolved      |
#         '`""""""`.                 """""""""^         `,,,"    ------------------------------------------------------
#            '^"""""`.               ^""""""""""'.   .`,,,,,^    | Mounted image manager (CLI version)                |
#              .^"""""`.            ."""""""",,,,,,,,,,,,,,,.    ------------------------------------------------------
#                .^"""""^.        .`",,"""",,,,,,,,,,,,,,,,'     | (C) 2023 CodingWonders Software                    |
#                  .^"""""^.    '`^^"",:,,,,,,,,,,,,,,,,,".      ------------------------------------------------------
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
                                                           

# Detect PS version first, as some parameters require PowerShell 5 or newer
if ($PSVersionTable.PSVersion.Major -lt 5)
{
    Write-Host "You need to run this script in PowerShell 5 or newer. Press ENTER to continue..."
    Read-Host | Out-Null
    $host.UI.RawUI.WindowTitle = "DISMTools Command Console"
    exit 1
}

# Import the DISM module
Import-Module Dism

$global:mImage = Get-WindowsImage -Mounted
# Backup variable
$global:mImages = Get-WindowsImage -Mounted
$global:selImage = 0
$global:imgInfo = ''
$newImg = 0
$selImgPath = ''
$ver = '0.3'
$global:img_removalIndexes = New-Object System.Collections.ArrayList
$global:img_remIndexesBck = New-Object System.Collections.ArrayList

# Unmount setting variables
$global:checkIntegrity = $false
$global:appendIndex = $false

# Set window title
$host.UI.RawUI.WindowTitle = "Mounted image manager"

function Mark-Image {
    # Refresh the mounted image variable
    $global:mImage = Get-WindowsImage -Mounted
    if ($global:mImage.Count -ne $global:mImages.Count)
    {
        # Show mounted image list once again, as indexes may have changed

        $global:mImages = Get-WindowsImage -Mounted
        Clear-Host
        # Print version info
        Write-Output "DISMTools $ver - Mounted image manager"
        # List mounted Windows images
        Get-WindowsImage -Mounted | Format-Table
        Write-Host `n`n`n
    }
    if ($global:mImage.Count -gt 1)
    {
        Write-Host -NoNewline "Mark image number [1 - $($global:mImage.Count)] or press [B] to go back: "
    }
    elseif ($global:mImage.Count -le 0)
    {
        Write-Host "No images have been mounted on this system. You need to mount an image before marking it here."`n`n"Press ENTER to continue..."
        Read-Host | Out-Null
        MainMenu
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
                Write-Host "This image is mounted with read-only permissions. Changes cannot be committed to this image."`n`n"If you want to make changes to this image, you must enable write permissions by pressing the [E] key in the main menu."`n -ForegroundColor Black -BackgroundColor DarkRed
                Write-Host "Press ENTER to continue..."
                Read-Host | Out-Null
                Unmount-Image
            }
            Write-Host "Unmounting Windows image:"`n"- Image file and index: $($global:mImage[$global:selImage - 1].ImagePath) (index $($global:mImage[$global:selImage - 1].ImageIndex))"`n"- Mount directory: $($global:mImage[$global:selImage - 1].MountPath)"`n"- Operation: Commit"`n"- Additional opperations: don't check integrity, don't append to new index"
            Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Save
            if ($?)
            {
                Write-Host "This image has been unmounted successfully. " -NoNewline
            }
            else
            {
                Write-Host "We could not unmount this image. Refer to the log file for more information. " -NoNewline                
            }
            Write-Host "Press ENTER to continue..."
            if ($?) { $global:selImage = 0 }
            Read-Host | Out-Null
            MainMenu
        }
        "D" {
            Clear-Host
            Write-Host "Unmounting Windows image:"`n"- Image file and index: $($global:mImage[$global:selImage - 1].ImagePath) (index $($global:mImage[$global:selImage - 1].ImageIndex))"`n"- Mount directory: $($global:mImage[$global:selImage - 1].MountPath)"`n"- Operation: Discard"
            Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Discard
            if ($?)
            {
                Write-Host "This image has been unmounted successfully. " -NoNewline
            }
            else
            {
                Write-Host "We could not unmount this image. Refer to the log file for more information. " -NoNewline
            }
            Write-Host "Press ENTER to continue..."
            if ($?) { $global:selImage = 0 }
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

function Unmount-Settings {
    Clear-Host
    if ($global:mImage[$global:selImage - 1].MountMode -eq 1)
    {
        Write-Host "Unmount settings apply to the commit operation. This image was mounted with read-only permissions. Changes cannot be committed to this image."`n`n"If you want to make changes to this image, you must enable write permissions by pressing the [E] key in the main menu."`n -ForegroundColor Black -BackgroundColor DarkRed
        Write-Host "Press ENTER to continue..."
        Read-Host | Out-Null
        Unmount-Image
    }
    Write-Host "Unmount settings for image: $($global:mImage[$global:selImage - 1].ImagePath)"`n
    if ($global:checkIntegrity)
    {
        Write-Host " [X] Check image integrity (press C to modify this setting)"
    }
    else
    {
        Write-Host " [ ] Check image integrity (press C to modify this setting)"
    }
    if ($global:appendIndex)
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
            $global:checkIntegrity = -not $global:checkIntegrity
            Unmount-Settings
        }
        "A" {
            $global:appendIndex = -not $global:appendIndex
            Unmount-Settings
        }
        "P" {
            Write-Host `n"Unmounting image with specified settings..."`n
            if (($global:appendIndex) -and ($global:checkIntegrity))
            {
                Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Save -CheckIntegrity -Append
            }
            elseif ($global:appendIndex)
            {
                Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Save -Append
            }
            elseif ($global:checkIntegrity)
            {
                Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Save -CheckIntegrity
            }
            else
            {
                Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Save
            }
            if ($?)
            {
                Write-Host "This image has been unmounted successfully. " -NoNewline
            }
            else
            {
                Write-Host "The operation has failed"
            }
            Write-Host `n"Press ENTER to continue..."
            Read-Host | Out-Null
            MainMenu
        }
        "B" {
            MainMenu
        }
        default {
            Unmount-Settings
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
            if ($? -eq $false)
            {
                Write-Host "The unmount operation has failed. Read the log file for more information."`n`n"Press ENTER to continue..."
                Read-Host | Out-Null
                MainMenu
            }
            Write-Host "50 % - Remounting specified image with write permissions... (step 2 of 2)"
            Mount-WindowsImage -ImagePath $global:mImage[$global:selImage - 1].ImagePath -Index $global:mImage[$global:selImage - 1].ImageIndex -Path $global:mImage[$global:selImage - 1].MountPath
            if ($? -eq $false)
            {
                Write-Host "The mount operation has failed. Read the log file for more information."`n`n"Press ENTER to continue..."
                Read-Host | Out-Null
                MainMenu
            }
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

function Remove-VolumeImages {
    Clear-Host
    Write-Host "List of volume images in image file:"
    $global:imgInfo = Get-WindowsImage -ImagePath $global:mImage[$global:selImage - 1].ImagePath
    if (($global:imgInfo | Select-Object -ExpandProperty ImageIndex).Count -le 1)
    {
        Write-Host "This image only contains one index, so you can't remove volume images from this image file."`n`n -ForegroundColor Black -BackgroundColor DarkYellow
        Write-Host "Press ENTER to continue..."
        Read-Host | Out-Null
        MainMenu
    }
    $global:imgInfo | Format-Table ImageIndex, ImageName
    Write-Host `n"Marked indexes to remove: " -NoNewline
    if ($global:img_removalIndexes.Count -lt 1)
    {
        Write-Host "none"
    }
    else
    {
        Write-Host `n
        for ($i = 0; $i -lt $global:img_removalIndexes.Count; $i++)
        {
            Write-Host " - Index $($global:img_removalIndexes[$i]) ($($global:imgInfo[$global:img_removalIndexes[$i] - 1].ImageName))"
        }
    }
    Write-Host `n
    # Add menu items
    Write-Host "[M]: Mark indexes to remove"
    Write-Host "[P]: Proceed with volume image removal"`n
    Write-Host "[B]: Go back"
    Write-Host `n
    $option = Read-Host -Prompt "Select an option and press ENTER"
    switch ($option)
    {
        "M" {
            Write-Host `n"Mark volume images to remove based on their indexes."`n"For this image, you must choose anything between 1 and $(($global:imgInfo | Select-Object -ExpandProperty ImageIndex).Count) and separate multiple entries with commas (,). Then, press ENTER."`n"You can also type 'B' and press ENTER to go back."`n
            $images = Read-Host -Prompt "Volume images to remove"
            if ($images.Equals("B", [System.StringComparison]::OrdinalIgnoreCase))
            {
                Remove-VolumeImages
            }
            else
            {
                $global:img_removalIndexes = $images.Split(",")
                $global:img_remIndexesBck = $images.Split(",")
                if ($global:img_removalIndexes.Count -ge $(($global:imgInfo | Select-Object -ExpandProperty ImageIndex).Count))
                {
                    # Clear lists, as the user has typed more entries than image indexes available,
                    # or has typed all indexes (leaving the image with no indexes).
                    $global:img_removalIndexes = $global:img_removalIndexes.Clear()
                    $global:img_remIndexesBck = $global:img_remIndexesBck.Clear()
                    # Go back
                    Remove-VolumeImages
                }
                # The user might have added an index higher than the index count, or might have done some trickery.
                # Go through each answer and delete invalid entries.
                for ($i = 0; $i -lt $global:img_remIndexesBck.Count; $i++)
                {
                    if ($global:img_remIndexesBck[$i] -gt $(($global:imgInfo | Select-Object -ExpandProperty ImageIndex).Count))
                    {
                        for ($j = 0; $j -lt $global:img_removalIndexes.Count; $j++)
                        {
                            if ($global:img_remIndexesBck[$i] -eq $global:img_removalIndexes[$j])
                            {
                                $global:img_removalIndexes = $global:img_removalIndexes.RemoveAt($global:img_removalIndexes[$j])
                            }
                        }
                    }
                }
                Remove-VolumeImages
            }
        }
        "P" {
            if ($global:img_removalIndexes.Count -ge 1)
            {            
                Write-Host `n"The marked image will be unmounted discarding changes. Make sure you commit your changes to it before removing its volume images."
                $option = Read-Host -Prompt "Do you want to proceed? (Y/N)"
                switch ($option)
                {
                    "Y" {
                        Write-Host "Unmounting image discarding changes..."
                        Dismount-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -Discard
                        if ($?)
                        {
                            Write-Host "Removing volume images..."`n
                            for ($i = 0; $i -lt $global:img_removalIndexes.Count; $i++)
                            {
                                Write-Host "$([System.Math]::Round(($i / $global:img_removalIndexes.Count) * 100))% - Removing volume image... (index: $($global:img_removalIndexes[$i]), name: $($global:imgInfo[$global:img_removalIndexes[$i] - 1].ImageName))    " -NoNewline
                                Remove-WindowsImage -ImagePath $global:mImage[$global:selImage - 1].ImagePath -Name $global:imgInfo[$global:img_removalIndexes[$i] - 1].ImageName | Out-Null
                                if ($?)
                                {
                                    Write-Host "SUCCESS" -ForegroundColor White -BackgroundColor DarkGreen
                                }
                                else
                                {
                                    Write-Host "FAILURE" -ForegroundColor Black -BackgroundColor DarkRed
                                }
                            }
                            # Et voilà !
                            Write-Host "100% - Complete"`n
                            Write-Host "Press ENTER to continue..."
                            Read-Host | Out-Null
                            MainMenu
                        }
                        else
                        {
                            Write-Host "The unmount operation has failed. Refer to the error shown above for more information."`n`n"Read the error, then press ENTER to continue"
                            Read-Host | Out-Null
                            Remove-VolumeImages
                        }
                    }
                    "N" {
                        Remove-VolumeImages
                    }
                    default {
                        Remove-VolumeImages
                    }
                }
            }
            else
            {
                Write-Host "Please mark the volume images to remove from this image, and try again." -ForegroundColor Black -BackgroundColor DarkYellow
                Read-Host | Out-Null
                Remove-VolumeImages
            }
        }
        "B" {
            if ($global:img_removalIndexes.Count -ge 1)
            {
                Write-Host `n"The volume images you have marked for removal will be unmarked."`n
                $option = Read-Host -Prompt "Do you want to continue? (Y/N)"
                switch ($option)
                {
                    "Y" {
                        $global:img_removalIndexes = $global:img_removalIndexes.Clear()
                        $global:img_remIndexesBck = $global:img_remIndexesBck.Clear()
                        MainMenu
                    }
                    "N" {
                        Remove-VolumeImages
                    }
                    default {
                        # Go back
                        Remove-VolumeImages
                    }
                }
            }
            MainMenu
        }
        default {
            Remove-VolumeImages
        }
    }
}

function Update-Listing {
    $global:mImage = Get-WindowsImage -Mounted
    # MainMenu
}

function Get-MenuItems {
    if ($global:mImage.Count -ge 1)
    {
        Write-Host "[M]: Mark image for management"
    }
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
    if ($global:selImage -eq 0) { return }
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
            Write-Host "The image you have marked has been unmounted by an external program. You can't manage this image until you mount it again, and you must mark another image now."`n -ForegroundColor White -BackgroundColor Red
            return
        }
        # Iterate through each mounted image so that we can switch to the appropriate one
        for ($i = 0; $i -lt $global:mImage.Count; $i++)
        {
            if ($global:mImage[$i].ImagePath -eq $selImgPath)
            {
                $global:selImage = $i + 1
                Write-Host "The image you have marked has moved indexes, so the mounted image manager switched to the index the image is now in."`n"You can continue to do your management tasks."`n -ForegroundColor White -BackgroundColor DarkBlue
                return
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
    Detect-MountedImageIndexChanges
    if (($global:selImage -eq 0) -and $global:mImage.Count -ge 1)
    {
        Write-Host "No image has been marked for management. Press the [M] key to mark a mounted image..."
    }
    elseif (($global:selImage -eq 0) -and $global:mImage.Count -le 1)
    {
        Write-Host "No images have been mounted on this system. Mount an image and update the listing to be able to mark mounted images"
    }
    else
    {
        # Detect whether the previously marked image is 0 and refresh the menu
        #if ($global:selImage -eq 0) { MainMenu }
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
                if ($?)
                {
                    Write-Host `n"The servicing session for this image has been reloaded successfully."`n
                }
                else
                {
                    Write-Host `n"The servicing session for this image could not be reloaded. Refer to the log file for more information"`n                    
                }
                Write-Host "Press ENTER to continue..."
                Read-Host | Out-Null
                MainMenu
            }
            elseif ($global:mImage[$global:selImage - 1].MountStatus -eq 2)
            {
                Write-Host `n"Repairing the component store of this image..."
                Repair-WindowsImage -Path $global:mImage[$global:selImage - 1].MountPath -RestoreHealth
                if ($?)
                {
                    Write-Host `n"The component store of this image has been repaired successfully."`n
                }
                else
                {
                    Write-Host `n"The component store of this image could not be repaired. Refer to the log file for more information."`n
                }
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
            if (($global:selImage -gt 0) -and (Test-Path -Path $($global:mImage[$global:selImage - 1].MountPath)))
            {
                Start-Process -FilePath $(([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Windows)) + '\explorer.exe') -ArgumentList $($global:mImage[$global:selImage - 1].MountPath)
            }
            MainMenu
        }
        "V" {
            Remove-VolumeImages
        }
        "L" {
            Write-Host `n"Updating mounted image listing..." 
            Update-Listing
            MainMenu
        }
        "X" {
            $host.UI.RawUI.WindowTitle = "DISMTools Command Console"
            exit 0
        }
        default { MainMenu }
    }
}

Update-Listing
MainMenu