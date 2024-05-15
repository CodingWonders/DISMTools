#requires -version 5.0
#                                              ....              
#                                         .'^""""""^.            
#      '^`'.                            '^"""""""^.              
#     .^"""""`'                       .^"""""""^.                ---------------------------------------------------------
#      .^""""""`                      ^"""""""`                  | DISMTools 0.5                                         |
#       ."""""""^.                   `""""""""'           `,`    | The connected place for Windows system administration |
#         '`""""""`.                 """""""""^         `,,,"    ---------------------------------------------------------
#            '^"""""`.               ^""""""""""'.   .`,,,,,^    | Mounted image manager (CLI version)                   |
#              .^"""""`.            ."""""""",,,,,,,,,,,,,,,.    ---------------------------------------------------------
#                .^"""""^.        .`",,"""",,,,,,,,,,,,,,,,'     | (C) 2023-2024 CodingWonders Software                  |
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

# Detect if admin privileges are present, and stop the script from running any further if they aren't present
if (([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator) -eq $false)
{
    Write-Host "This script must be run with administrative privileges. Press ENTER to exit..."
    Read-Host | Out-Null
    $host.UI.RawUI.WindowTitle = "DISMTools Command Console"
    exit 1    
}

Import-Module Dism

$ver = "0.5"

# Set window title
$host.UI.RawUI.WindowTitle = "Mounted image manager"

function Get-MountedImages
{
    return Get-WindowsImage -Mounted
}

function Get-MenuItems
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [int] $selImage,
        [Parameter(Mandatory = $true, Position = 1)] [Object[]] $mountedImages
    )
    try
    {
        if ($mountedImages.Count -ge 1)
        {
            Write-Host "[M] Mark image for management"
        }
        if ($selImage -ne 0)
        {
            Write-Host "[U] Unmount image"
            switch ($mountedImages[$selImage - 1].MountStatus)
            {
                "NeedsRemount" {
                    Write-Host "[R] Reload servicing"
                }
                "Invalid" {
                    Write-Host "[R] Repair component store"
                }
                default {
                    # Don't write anything
                }
            }
            if ($mountedImages[$selImage - 1].MountMode -eq "ReadOnly")
            {
                Write-Host "[E] Enable write permissions"
            }
            Write-Host "[A] Access mount directory"
            if ((Get-WindowsImage -ImagePath "$($mountedImages[$selImage - 1].ImagePath)" | Select-Object -ExpandProperty ImageIndex).Count -gt 1)
            {
                Write-Host "[V] Remove volume images..."
                Write-Host "[S] Switch image indexes..."
            }
        }
        Write-Host "[L] Update mounted image listing"
        Write-Host "[X] Exit"
        return $true
    }
    catch
    {
        Write-Host "An error occurred while getting menu items:`n`n$($_)"
        return $false
    }
}

function Get-UserOption
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string[]] $validOptions
    )
    $option = Read-Host -Prompt "Select an option and press ENTER"
    if ($validOptions.Contains($option.ToUpper()))
    {
        return $option
    }
    else
    {
        do {
            Write-Host "This option is not valid"
            $option = Read-Host -Prompt "Select an option and press ENTER"
        } until ($validOptions.Contains($option.ToUpper()))
        return $option
    }
}

function Show-MountedImages
{
    Clear-Host
    # Write version information
    Write-Host "DISMTools $ver - Mounted image manager"
    # Output the mounted images
    ($mountedImages | Format-Table) | Out-Host
    Write-Host `n`n`n
}
function Show-Menu
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [int] $selImage
    )
    $mountedImages = Get-MountedImages
    Show-MountedImages
    if (($selImage -eq 0) -and ($mountedImages.Count -ge 1))
    {
        Write-Host "No image has been marked for management. Press the [M] key to mark a mounted image..."
    }
    elseif (($selImage -eq 0) -and ($mountedImages.Count -lt 1))
    {
        Write-Host "No images have been mounted on this system. Mount an image and update the listing to be able to mark mounted images"
    }
    else
    {
        Write-Host "Selected image: image $selImage (`"$($mountedImages[$selImage - 1].ImagePath)`")"
    }
    Write-Host `n
    if ((Get-MenuItems -selImage $selImage -mountedImages $mountedImages) -eq $false)
    {
        Write-Host "An error occurred while getting menu items. Please report this issue to the developers."
        return $false
    }
    Write-Host `n
    $option = Get-UserOption -validOptions @("M", "U", "R", "E", "A", "V", "S", "L", "X")
    switch ($option)
    {
        "M" {
            $selImage = Set-Image -mountedImages $mountedImages -selImage $selImage
            Show-Menu -selImage $selImage
        }
        "U" {
            if ($selImage -lt 1) { Show-Menu -selImage $selImage }
            switch ($mountedImages[$selImage - 1].MountMode)
            {
                "ReadWrite" {
                    Dismount-Image -mountDir "$($mountedImages[$selImage - 1].MountPath)" -action "ask"
                }
                "ReadOnly" {
                    Dismount-Image -mountDir "$($mountedImages[$selImage - 1].MountPath)" -action "discard"
                }
                default {
                    Show-Menu -selImage $selImage
                }
            }
        }
        "R" {
            if ($selImage -lt 1) { Show-Menu -selImage $selImage }
            switch ($mountedImages[$selImage - 1].MountStatus)
            {
                "NeedsRemount" {
                    Set-ImageStatus -action "remount" -mountDir "$($mountedImages[$selImage - 1].MountPath)"
                }
                "Invalid" {
                    Set-ImageStatus -action "repair" -mountDir "$($mountedImages[$selImage - 1].MountPath)"
                }
                default {
                    Show-Menu -selImage $selImage
                }
            }
        }
        "E" {
            if ($selImage -lt 1) { Show-Menu -selImage $selImage }
            Enable-WritePermissions -mountDir "$($mountedImages[$selImage - 1].MountPath)"
        }
        "A" {
            if ($selImage -lt 1) { Show-Menu -selImage $selImage }
            if (($selImage -gt 0) -and (Test-Path -Path "$($mountedImages[$selImage - 1].MountPath)"))
            {
                Start-Process -FilePath $(([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Windows)) + '\explorer.exe') -ArgumentList "$($mountedImages[$selImage - 1].MountPath)"
            }
            Show-Menu -selImage $selImage
        }
        "V" {
            if ($selImage -lt 1) { Show-Menu -selImage $selImage }
            Remove-VolumeImages -imagePath "$($mountedImages[$selImage - 1].ImagePath)" -mountDir "$($mountedImages[$selImage - 1].MountPath)" -removalIndexes @(0)
        }
        "S" {
            if ($selImage -lt 1) { Show-Menu -selImage $selImage }
            Switch-Indexes -imagePath "$($mountedImages[$selImage - 1].ImagePath)" -mountDir "$($mountedImages[$selImage - 1].Path)" -destIndex 0 -suMountOp 0
        }
        "L" {
            if ($selImage -gt 0) { $mountDir = $mountedImages[$selImage - 1].Path }
            Write-Host "Updating mounted image listing..."
            $mountedImgs = (Get-WindowsImage -Mounted).MountPath
            $idx = 0
            $unmounted = $false
            foreach ($image in $mountedImgs)
            {
                if ($image -eq $mountDir)
                {
                    $idx = $mountedImgs.IndexOf($image)
                    $unmounted = $false
                    break
                }
                $unmounted = $true
            }
            if (($selImage -ne 0) -and ($unmounted))
            {
                $idx = -1
                Write-Host `n
                Write-Host "The selected image has been unmounted. You will need to mark another image for management." -BackgroundColor DarkYellow -ForegroundColor Black
                Start-Sleep -Seconds 2
            }
            if ($selImage -eq 0) { $idx = -1 }
            Show-Menu -selImage $($idx + 1)
        }
        "X" {
            $host.UI.RawUI.WindowTitle = "DISMTools Command Console"
            exit 0
        }
        default {
            Show-Menu -selImage $selImage
        }
    }
}

function Set-Image
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [Object[]] $mountedImages,
        [Parameter(Mandatory = $true, Position = 0)] [int] $selImage
    )
    $mountedImages = Get-WindowsImage -Mounted
    Clear-Host
    ($mountedImages | Format-Table) | Out-Host
    if ($selImage -ne 0)
    {
        Write-Host "`nCurrently selected image: image $selImage"
    }
    Write-Host `n`n`n
    if ($mountedImages.Count -gt 1)
    {
        Write-Host -NoNewline "Mark image number [1 - $($mountedImages.Count)] or press [B] to go back: "
    }
    elseif ($mountedImages.Count -eq 1)
    {
        Write-Host -NoNewline "Mark image number [1] or press [B] to go back: "        
    }
    else
    {
        Write-Host "No images have been mounted on this system. You need to mount an image before marking it here."`n`n"Press ENTER to continue..."
        Read-Host | Out-Null
        return 0
    }
    $optn = Read-Host
    # Convert the string to an integer
    try
    {
        $option = [int]$optn
        if (($option -gt $mountedImages.Count) -or ($option -eq 0))
        {
            do {
                $option = Set-Image -mountedImages $mountedImages -selImage $selImage
            } until (($option -ne 0) -and ($option -le $mountedImages.Count))
        }
        return $option
    }
    catch
    {
        if ($optn -eq "B")
        {
            if ($selImage -eq 0)
            {
                return 0                
            }
            return $selImage
        }
        else
        {
            Write-Host "You have marked a non-existent mounted image"
            do {
                $option = Set-Image -mountedImages $mountedImages -selImage $selImage
            } until ($null -ne $option)
            return $option
        }
    }
}

function Set-ImageStatus
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $action,
        [Parameter(Mandatory = $true, Position = 1)] [string] $mountDir
    )
    switch ($action)
    {
        "remount" {
            Write-Host "Reloading the servicing session of the mounted image..."
            Mount-WindowsImage -Path $mountDir -Remount
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
            $mountedImgs = (Get-WindowsImage -Mounted).MountPath
            foreach ($image in $mountedImgs)
            {
                if ($image -eq $mountDir)
                {
                    Show-Menu -selImage $($mountedImgs.IndexOf($image) + 1)
                }
            }            
        }
        "repair" {
            $src = Read-Host "Please specify the source to use for the component store repair process or [B] to go back"
            if (($src -eq "") -or (Test-Path $src -eq $false))
            {
                do {
                    $src = Read-Host "Please specify the source to use for the component store repair process"
                } until (($src -ne "") -and (Test-Path $src))
            }
            if ($src -eq "B") 
            {
                $mountedImgs = (Get-WindowsImage -Mounted).MountPath
                foreach ($image in $mountedImgs)
                {
                    if ($image -eq $mountDir)
                    {
                        Show-Menu -selImage $($mountedImgs.IndexOf($image) + 1)
                    }
                }
            }
            Repair-WindowsImage -Path $mountDir -RestoreHealth -Source "$src" -LimitAccess
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
            $mountedImgs = (Get-WindowsImage -Mounted).MountPath
            foreach ($image in $mountedImgs)
            {
                if ($image -eq $mountDir)
                {
                    Show-Menu -selImage $($mountedImgs.IndexOf($image) + 1)
                }
            } 
        }
        default {

        }
    }
}

function Dismount-Image
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $mountDir,
        [Parameter(Mandatory = $true, Position = 1)] [string] $action
    )
    Clear-Host
    $mountedImgs = (Get-WindowsImage -Mounted).MountPath
    $path = ""
    $index = 0
    foreach ($image in $mountedImgs)
    {
        if ($image -eq $mountDir)
        {
            $idx = $mountedImgs.IndexOf($image)
            $imageInfo = (Get-WindowsImage -Mounted)[$idx]
            $path = $imageInfo.ImagePath
            $index = $imageInfo.ImageIndex
            $mode = $imageInfo.MountMode
        }
    }
    switch ($action)
    {
        "commit" {
            if ($mode -eq "ReadOnly")
            {
                Write-Host "This image is mounted with read-only permissions. Changes cannot be committed to this image."`n`n"If you want to make changes to this image, you must enable write permissions by pressing the [E] key in the main menu."`n -ForegroundColor Black -BackgroundColor DarkRed
                Write-Host "Press ENTER to continue..."
                Read-Host | Out-Null
                Dismount-Image -mountDir $mountDir -action "ask"
            }
            Write-Host "Unmounting Windows image...`n"
            Write-Host "- Image file and index: `"$path`" (index $index)"
            Write-Host "- Mount directory: `"$mountDir`""
            Write-Host "- Operation: commit"
            Dismount-WindowsImage -Path "$mountDir" -Save
            $success = $?
            if ($success)
            {
                Write-Host "This image has been unmounted successfully. " -NoNewline
            }
            else
            {
                Write-Host "We could not unmount this image. Refer to the log file for more information. " -NoNewline                
            }
            Write-Host "Press ENTER to continue..."
            Read-Host | Out-Null
            if ($success)
            {
                Show-Menu -selImage 0 
            }
            else
            {
                Show-Menu -selImage $($idx + 1)
            }
        }
        "discard" {
            Write-Host "Unmounting Windows image...`n"
            Write-Host "- Image file and index: `"$path`" (index $index)"
            Write-Host "- Mount directory: `"$mountDir`""
            Write-Host "- Operation: discard"
            Dismount-WindowsImage -Path "$mountDir" -Discard
            $success = $?
            if ($success)
            {
                Write-Host "This image has been unmounted successfully. " -NoNewline
            }
            else
            {
                Write-Host "We could not unmount this image. Refer to the log file for more information. " -NoNewline                
            }
            Write-Host "Press ENTER to continue..."
            Read-Host | Out-Null
            if ($success)
            {
                Show-Menu -selImage 0 
            }
            else
            {
                Show-Menu -selImage $($idx + 1)
            }            
        }
        default {
            Clear-Host
            Write-Host "Image about to be unmounted: `"$path`""`n
            if ($mode -eq "ReadOnly")
            {
                Write-Host "This image is mounted with read-only permissions. You can't commit changes to it."`n
            }
            Write-Host "[C] Commit changes and unmount"
            Write-Host "[D] Discard changes and unmount"
            Write-Host "[S] Unmount settings..."
            Write-Host "[B] Back"`n
            $option = Get-UserOption -validOptions @("C", "D", "S", "B")
            switch ($option)
            {
                "C" {
                    Dismount-Image -mountDir $mountDir -action "commit"
                }
                "D" {
                    Dismount-Image -mountDir $mountDir -action "discard"
                }
                "S" {
                    Dismount-Settings -checkIntegrity $false -appendIndex $false -mountDir "$mountDir"
                }
                "B" {
                    Show-Menu -selImage $($idx + 1)                    
                }
            }
        }
    }
}

function Dismount-Settings
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [bool] $checkIntegrity,
        [Parameter(Mandatory = $true, Position = 1)] [bool] $appendIndex,
        [Parameter(Mandatory = $true, Position = 2)] [string] $mountDir
    )
    Clear-Host
    $mountedImgs = (Get-WindowsImage -Mounted).MountPath
    $path = ""
    foreach ($image in $mountedImgs)
    {
        if ($image -eq $mountDir)
        {
            $idx = $mountedImgs.IndexOf($image)
            $imageInfo = (Get-WindowsImage -Mounted)[$idx]
            $path = $imageInfo.ImagePath
            $mode = $imageInfo.MountMode
        }
    }
    if ($mode -eq "ReadOnly")
    {
        Write-Host "Unmount settings apply to the commit operation. This image was mounted with read-only permissions. Changes cannot be committed to this image."`n`n"If you want to make changes to this image, you must enable write permissions by pressing the [E] key in the main menu."`n -ForegroundColor Black -BackgroundColor DarkRed
        Write-Host "Press ENTER to continue..."
        Read-Host | Out-Null
        Dismount-Image
    }
    Write-Host "Unmount settings for image: `"$path`""`n
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
    $option = Get-UserOption -validOptions @("C", "A", "P", "B")
    switch ($option)
    {
        "C" {
            Dismount-Settings -checkIntegrity $(-not $checkIntegrity) -appendIndex $appendIndex -mountDir $mountDir
        }
        "A" {
            Dismount-Settings -checkIntegrity $checkIntegrity -appendIndex $(-not $appendIndex) -mountDir $mountDir
        }
        "P" {
            Write-Host `n"Unmounting image with specified settings..."`n
            if (($appendIndex) -and ($checkIntegrity))
            {
                Dismount-WindowsImage -Path $mountDir -Save -CheckIntegrity -Append
            }
            elseif ($appendIndex)
            {
                Dismount-WindowsImage -Path $mountDir -Save -Append
            }
            elseif ($checkIntegrity)
            {
                Dismount-WindowsImage -Path $mountDir -Save -CheckIntegrity
            }
            else
            {
                Dismount-WindowsImage -Path $mountDir -Save
            }
            $success = $?
            if ($success)
            {
                Write-Host "This image has been unmounted successfully. " -NoNewline
            }
            else
            {
                Write-Host "We could not unmount this image. Refer to the log file for more information. " -NoNewline                
            }
            Write-Host "Press ENTER to continue..."
            Read-Host | Out-Null
            if ($success)
            {
                Show-Menu -selImage 0 
            }
            else
            {
                Show-Menu -selImage $($idx + 1)
            }            
        }
        "B" {
            Show-Menu -selImage $($idx + 1)
        }
        default {
            Dismount-Settings -checkIntegrity $checkIntegrity -appendIndex $appendIndex -mountDir $mountDir
        }
    }        
}

function Enable-WritePermissions
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $mountDir
    )
    Clear-Host
    $mountedImgs = (Get-WindowsImage -Mounted).MountPath
    $path = ""
    $index = 0
    foreach ($image in $mountedImgs)
    {
        if ($image -eq $mountDir)
        {
            $idx = $mountedImgs.IndexOf($image)
            $imageInfo = (Get-WindowsImage -Mounted)[$idx]
            $path = $imageInfo.ImagePath
            $index = $imageInfo.ImageIndex
            $mode = $imageInfo.MountMode
        }
    }
    if ($mode -ne "ReadOnly")
    {
        Write-Host "This image has already been mounted with write permissions."`n -ForegroundColor Black -BackgroundColor DarkGreen
        Write-Host "Press ENTER to continue..."
        Read-Host | Out-Null
        Show-Menu -selImage $($idx + 1)
    }
    Write-Host "Image to enable write permissions to: `"$path`"`n`n"
    Write-Host "This option will unmount the image specified and (try to) remount it with write permissions."`n
    Write-Host "PLEASE BE SURE THAT THE SPECIFIED IMAGE STILL EXISTS IN ITS DIRECTORY. If it is located in an external drive or network share, make sure it is either plugged in or online at all times."`n`n"Do you want to proceed?"`n
    Write-Host "[Y] Proceed"
    Write-Host "[N] Don't proceed"`n
    $option = Get-UserOption -validOptions @("Y", "N")
    switch ($option)
    {
        "Y" {
            Write-Host "0  % - Unmounting specified image... (step 1 of 2)"
            Dismount-WindowsImage -Path "$mountDir" -Discard
            if ($? -eq $false)
            {
                Write-Host "The unmount operation has failed. Read the log file for more information."`n`n"Press ENTER to continue..."
                Read-Host | Out-Null
                Show-Menu -selImage $($idx + 1)
            }
            Write-Host "50 % - Remounting specified image with write permissions... (step 2 of 2)"
            Mount-WindowsImage -ImagePath "$path" -Index $index -Path "$mountDir"
            if ($? -eq $false)
            {
                Write-Host "The mount operation has failed. Read the log file for more information."`n`n"Press ENTER to continue..."
                Read-Host | Out-Null
                Show-Menu -selImage 0
            }
            Write-Host "100% - This operation has completed and the image should have been mounted with write permissions."`n`n"Press ENTER to continue..."
            Read-Host | Out-Null     
            Show-Menu -selImage $($idx + 1)       
        }
        "N" {
            Show-Menu -selImage $($idx + 1)
        }
    }
}

function Remove-VolumeImages
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $imagePath,
        [Parameter(Mandatory = $true, Position = 1)] [string] $mountDir,
        [Parameter(Mandatory = $true, Position = 2)] [int[]] $removalIndexes
    )
    $idx = 0
    $mountedImgs = (Get-WindowsImage -Mounted).MountPath
    foreach ($image in $mountedImgs)
    {
        if ($image -eq $mountDir)
        {
            $idx = $mountedImgs.IndexOf($image)
        }
    }
    Clear-Host
    $imgInfo = (Get-WindowsImage -ImagePath "$imagePath")
    if (($imgInfo | Select-Object -ExpandProperty ImageIndex).Count -le 1)
    {
        Write-Host "This image only contains one index, so you can't remove volume images from this image file."`n`n -ForegroundColor Black -BackgroundColor DarkYellow
        Write-Host "Press ENTER to continue..."
        Read-Host | Out-Null
        Show-Menu -selImage $($idx + 1)
    }
    Write-Host "List of volume images in image file: `"$imagePath`""
    ($imgInfo | Format-Table ImageIndex, ImageName) | Out-Host
    Write-Host `n"Marked indexes to remove: " -NoNewline
    if ($removalIndexes[0] -eq 0)
    {
        Write-Host "none"
    }
    else
    {
        Write-Host `n
        for ($i = 0; $i -lt $removalIndexes.Count; $i++)
        {
            if ($removalIndexes[$i] -lt 1) { continue }
            Write-Host " - Index $($removalIndexes[$i]) ($($imgInfo[$removalIndexes[$i] - 1].ImageName))"
        }
    }
    Write-Host `n
    # Add menu items
    Write-Host "[M] Mark indexes to remove"
    Write-Host "[P] Proceed with volume image removal"`n
    Write-Host "[B] Go back"
    Write-Host `n
    $option = Get-UserOption -validOptions @("M", "P", "B")
    switch ($option)
    {
        "M" {
            Write-Host `n"Mark volume images to remove based on their indexes."`n"For this image, you must choose anything between 1 and $(($imgInfo | Select-Object -ExpandProperty ImageIndex).Count) and separate multiple entries with commas (,). Then, press ENTER."`n"You can also type 'B' and press ENTER to go back."`n
            $images = Read-Host -Prompt "Volume images to remove"
            if ($images.Equals("B", [System.StringComparison]::OrdinalIgnoreCase))
            {
                Remove-VolumeImages -imagePath "$imagePath" -mountDir "$mountDir" -removalIndexes $removalIndexes
            }
            else
            {
                $indexCount = ($imgInfo | Select-Object -ExpandProperty ImageIndex).Count
                $removalIndexList = $images.Split(",")
                if ($removalIndexList.Count -gt 0)
                {
                    for ($i = 0; $i -lt $removalIndexList.Count; $i++)
                    {
                        try
                        {
                            $index = [int] $removalIndexList[$i]
                            if (($index -lt 1) -or ($index -gt $indexCount))
                            {
                                $removalIndexList[$i] = 0
                            }
                        }
                        catch
                        {
                            Write-Host "WARNING: the following option ($($removalIndexList[$i])) is invalid. Removing item..."
                            $removalIndexList[$i] = 0
                            continue
                        }
                    }
                    Remove-VolumeImages -imagePath "$imagePath" -mountDir "$mountDir" -removalIndexes $removalIndexList
                }
                elseif ($removalIndexList.Count -ge ($imgInfo | Select-Object -ExpandProperty ImageIndex).Count)
                {
                    for ($i = 0; $i -lt $removalIndexList.Count; $i++)
                    {
                        $removalIndexList[$i] = 0
                    }
                    Write-Host `n"You have specified all indexes in this image. The resulting image must have at least 1 index available. Press ENTER to go back..." -BackgroundColor DarkYellow -ForegroundColor Black
                    Read-Host | Out-Null
                    Remove-VolumeImages -imagePath "$imagePath" -mountDir "$mountDir" -removalIndexes $removalIndexList
                }
            }
        }
        "P" {
            if ($removalIndexes.Count -ge 1)
            {            
                Write-Host `n"The marked image will be unmounted discarding changes. Make sure you commit your changes to it before removing its volume images. Do you want to proceed?"`n
                Write-Host "[Y] Proceed"
                Write-Host "[N] Don't proceed"`n
                $option = Get-UserOption -validOptions @("Y", "N")
                switch ($option)
                {
                    "Y" {
                        Write-Host "Unmounting image discarding changes..."
                        Dismount-WindowsImage -Path "$mountDir" -Discard
                        if ($?)
                        {
                            Write-Host "Removing volume images..."`n
                            for ($i = 0; $i -lt $removalIndexes.Count; $i++)
                            {
                                if ($removalIndexes[$i] -lt 1) { continue }
                                Write-Host "$([System.Math]::Round(($i / $removalIndexes.Count) * 100))% - Removing volume image... (index: $($removalIndexes[$i]), name: $($imgInfo[$removalIndexes[$i] - 1].ImageName))    " -NoNewline
                                Remove-WindowsImage -ImagePath "$imagePath" -Name "$($imgInfo[$removalIndexes[$i] - 1].ImageName)" | Out-Null
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
                            $idx = -1
                            Show-Menu -selImage $($idx + 1)                
                        }
                        else
                        {
                            Write-Host "The unmount operation has failed. Refer to the error shown above for more information."`n`n"Read the error, then press ENTER to continue"
                            Read-Host | Out-Null
                            Remove-VolumeImages -imagePath "$imagePath" -mountDir "$mountDir" -removalIndexes $removalIndexes
                        }
                    }
                    "N" {
                        Remove-VolumeImages -imagePath "$imagePath" -mountDir "$mountDir" -removalIndexes $removalIndexes
                    }
                    default {
                        Remove-VolumeImages -imagePath "$imagePath" -mountDir "$mountDir" -removalIndexes $removalIndexes
                    }
                }
            }
            else
            {
                Write-Host "Please mark the volume images to remove from this image, and try again." -ForegroundColor Black -BackgroundColor DarkYellow
                Read-Host | Out-Null
                Remove-VolumeImages -imagePath "$imagePath" -mountDir "$mountDir" -removalIndexes $removalIndexes
            }
        }
        "B" {
            Show-Menu -selImage $($idx + 1)
        }
    }
}

function Switch-Indexes
{
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $imagePath,
        [Parameter(Mandatory = $true, Position = 1)] [string] $mountDir,
        [Parameter(Mandatory = $true, Position = 2)] [int] $destIndex,
        [Parameter(Mandatory = $true, Position = 3)] [int] $suMountOp
    )
    Clear-Host
    $mountedImgs = (Get-WindowsImage -Mounted).MountPath
    $imageInfo = ""
    $idx = 0
    foreach ($image in $mountedImgs)
    {
        if ($image -eq $mountDir)
        {
            $idx = $mountedImgs.IndexOf($image)
            $imageInfo = (Get-WindowsImage -Mounted)[$idx]
            $imgInfo = (Get-WindowsImage -ImagePath "$($imageInfo.ImagePath)")
        }
    }
    Write-Host "Getting image indexes. This may take some time..."`n
    if ((Get-WindowsImage -ImagePath "$imagePath").Count -le 1)
    {
        Write-Host "This image only contains 1 index, so you can't switch to other indexes."`n"Press ENTER to go back to the main menu..."
        Read-Host | Out-Null
        Show-Menu -selImage $($idx + 1)
    }
    # Since we have done the necessary prep work, clear the screen
    Clear-Host

    # Output image info
    Write-Host "- Image: `"$($imageInfo.ImagePath)`""
    Write-Host "- Currently mounted index: index $($imageInfo.ImageIndex) ($($imgInfo[$imageInfo.ImageIndex - 1].ImageName))"`n
    if (($destIndex -le 0) -or ($destIndex -gt $imgInfo.Count))
    {
        Write-Host "- Destination index to mount: not specified. Press S to specify..."
    }
    else
    {
        Write-Host "- Destination index to mount: $($destIndex) ($($imgInfo[$destIndex - 1].ImageName))"
    }
    switch ($suMountOp)
    {
        0 {
            Write-Host "- Unmount operation when switching indexes: not specified. Press C to change this setting..."
        }
        1 {
            Write-Host "- Unmount operation when switching indexes: commit"
        }
        2 {
            Write-Host "- Unmount operation when switching indexes: discard"
        }
    }

    # Display menu items
    Write-Host `n
    Write-Host "[S] Specify target index"`n -NoNewline
    if (($suMountOp -le 0) -or ($suMountOp -gt 2))
    {
        Write-Host "[C] Specify unmount operation"
    }
    else
    {
        Write-Host "[C] Change unmount operation"
    }
    Write-Host "[P] Proceed"`n`n"[B] Go back"`n
    $option = Get-UserOption -validOptions @("S", "C", "P", "B")
    switch ($option)
    {
        "S" {
            ($imgInfo | Format-Table ImageIndex, ImageName) | Out-Host
            Write-Host `n`n
            $option = Read-Host -Prompt "Choose an index from the list above and press ENTER, or press [B] to go back"
            switch ($option)
            {
                "B" {
                    Switch-Indexes -imagePath "$imagePath" -mountDir "$mountDir" -destIndex $destIndex -suMountOp $suMountOp
                }
                default {
                    try
                    {
                        $destIndex = [int]$option
                        # The user could have picked a non-existent image. Detect that
                        if (($destIndex -le 0) -or ($destIndex -gt $imgInfo.Count))
                        {
                            $destIndex = 0
                            Write-Host "The index selected does not exist in the image file. Press ENTER to go back, and try again."
                            Read-Host | Out-Null
                        }
                        Switch-Indexes -imagePath "$imagePath" -mountDir "$mountDir" -destIndex $destIndex -suMountOp $suMountOp
                    }
                    catch
                    {
                        Write-Host "The index selected does not exist in the image file. Press ENTER to go back, and try again."
                        Read-Host | Out-Null
                        Switch-Indexes -imagePath "$imagePath" -mountDir "$mountDir" -destIndex $destIndex -suMountOp $suMountOp
                    }
                }
            }
        }
        "C" {
            $option = Read-Host -Prompt "Press [C] to unmount the source index whilst saving changes, [U] to discard the changes and unmount, or [B] to go back"
            switch ($option)
            {
                "C" {
                    $suMountOp = 1
                }
                "U" {
                    $suMountOp = 2
                }
                "B" {
                    # Go back
                }
                default {
                    Write-Host "An invalid option has been specified. Press ENTER to go back, and try again"
                    Read-Host | Out-Null
                }
            }
            Switch-Indexes -imagePath "$imagePath" -mountDir "$mountDir" -destIndex $destIndex -suMountOp $suMountOp          
        }
        "P" {
            # Detect if necessary parameters weren't set up, and stop if that's the case
            if (($destIndex -le 0) -or ($destIndex -gt $imgInfo.Count))
            {
                Write-Host "The destination index hasn't been specified. Please specify one, and try again." -ForegroundColor Black -BackgroundColor DarkYellow
                Read-Host | Out-Null
                Switch-Indexes -imagePath "$imagePath" -mountDir "$mountDir" -destIndex $destIndex -suMountOp $suMountOp 
            }            
            if (($suMountOp -le 0) -or ($suMountOp -gt 2))
            {
                Write-Host "The unmount operation hasn't been specified. Please specify one, and try again." -ForegroundColor Black -BackgroundColor DarkYellow
                Read-Host | Out-Null
                Switch-Indexes -imagePath "$imagePath" -mountDir "$mountDir" -destIndex $destIndex -suMountOp $suMountOp 
            }
            # Stop this process if the user chose to switch to the same index
            if ($destIndex -eq $imageInfo.ImageIndex)
            {
                Write-Host "The destination index you have specified is the same as the source index. Make sure you have selected the destination index you wanted, and try again." -ForegroundColor Black -BackgroundColor DarkYellow
                Read-Host | Out-Null
                Switch-Indexes -imagePath "$imagePath" -mountDir "$mountDir" -destIndex $destIndex -suMountOp $suMountOp 
            }
            Write-Host "Beginning switch operation..."
            Write-Host "0  % - Unmounting source index... (step 1 of 2)"
            switch ($suMountOp)
            {
                1 {
                    Dismount-WindowsImage -Path "$mountDir" -Save
                }
                2 {
                    Dismount-WindowsImage -Path "$mountDir" -Discard
                }
            }
            if ($? -eq $false)
            {
                Write-Host "The unmount operation has failed. Read the log file for more information."`n`n"Press ENTER to continue..."
                Read-Host | Out-Null
                Show-Menu -selImage $($idx + 1)
            }
            Write-Host "50 % - Mounting destination index... (step 2 of 2)"
            Mount-WindowsImage -ImagePath "$imagePath" -Index $destIndex -Path "$mountDir"
            if ($? -eq $false)
            {
                Write-Host "The mount operation has failed. Read the log file for more information."`n`n"Press ENTER to continue..."
                Read-Host | Out-Null
                Show-Menu -selImage $($idx + 1)
            }
            Write-Host "100% - This operation has completed and the destination image should have been mounted."`n`n"Press ENTER to continue..."
            Read-Host | Out-Null
            Show-Menu -selImage $($idx + 1)           
        }
        "B" {
            Show-Menu -selImage $($idx + 1)
        }
    }
}

if ((Show-Menu -selImage 0) -eq $false)
{
    Write-Host "Script execution failed."
    exit 1
}