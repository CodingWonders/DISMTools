param (
    [Parameter(Mandatory = $true, Position = 0)] [string] $imageGroup,
    [Parameter(Mandatory = $true, Position = 1)] [string] $installImagePath,
    [Parameter(Position = 2)] [int] $installImageIndex = 0
)

function Write-ErrorMessage {
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $message
    )
    Write-Error -Message $message
    Write-Host "Press ENTER to exit..."
    Read-Host | Out-Null
}

function New-WdsInstallImage {
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $imageGroup,
        [Parameter(Mandatory = $true, Position = 1)] [string] $installImagePath,
        [Parameter(Position = 2)] [int] $installImageIndex = 0
    )

    if (($imageGroup -eq "") -or ($installImagePath -eq "") -or ($installImageIndex -lt 0)) { return }

    Write-Host "Preparing to add installation image to group `"$imageGroup`"..."

    if (-not (Test-Path -Path "$installImagePath" -PathType Leaf)) {
        Write-ErrorMessage -message "The installation image does not exist in the file system."
        return
    }

    # Check if the group exists and create it if it doesn't
    try {
        Get-WdsInstallImageGroup -Name "$imageGroup" -ErrorAction Stop
    } catch {
        Write-Host "Group does not exist. Creating it..."
        New-WdsInstallImageGroup -Name "$imageGroup"
    }

    # If no index is provided we need to add all of the indexes of the source image; otherwise
    # we grab the index name from the Windows image; wdsutil doesn't support /singleimage with
    # index numbers. To provide some better differentiation so we don't have "install-(2)", we
    # will also try to grab the EditionID, though this WON'T prevent unique numbers to be appended
    # if there happens to be an image with that same name.
    $providedIndexName = ""
    $providedIndexEditionId = ""
    if ($installImageIndex -ge 1) {
        Write-Host "Single image mode detected. Grabbing image name and edition ID..."
        try {
            $installImageInfo = Get-WindowsImage -ImagePath "$installImagePath" -Index $installImageIndex
            if ($null -ne $installImageInfo) {
                $providedIndexName = $installImageInfo.ImageName
                $providedIndexEditionId = $installImageInfo.EditionId

                Write-Host "Obtained index name: $providedIndexName"
                Write-Host "Obtained index edition ID: $providedIndexEditionId"
            }
        } catch {
            # Fall back to adding all of them
            Write-Host "Could not grab image information. Reverting to adding all install images..."
            $installImageIndex = 0
        }
    }

    $argList = "/verbose /progress /add-image /imagefile:`"$installImagePath`" /imagetype:install /imagegroup:`"$imageGroup`""
    if ($installImageIndex -gt 0) {
        $destImageFileName = "$([IO.Path]::GetFileNameWithoutExtension("$installImagePath"))_$providedIndexEditionId$([IO.Path]::GetExtension("$installImagePath"))"
        $argList += " /singleimage:`"$providedIndexName`" /filename:`"$destImageFileName`""
    }

    $wdsUtilProc = Start-Process -FilePath "wdsutil" -ArgumentList "$argList" -NoNewWindow -Wait -PassThru

    return ($wdsUtilProc.ExitCode -eq 0)
}

if (New-WdsInstallImage -imageGroup $imageGroup -installImagePath $installImagePath -installImageIndex $installImageIndex) {
    Write-Host "Image addition succeeded."
} else {
    Write-Host "Image addition failed."
    Read-Host | Out-Null
}