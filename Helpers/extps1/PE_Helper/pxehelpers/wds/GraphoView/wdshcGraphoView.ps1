$global:images = $null

if (-not (Test-Path -Path "$PSScriptRoot\images.json" -PathType Leaf)) {
    return
}

if (Test-Path -Path "$PSScriptRoot\rescan" -PathType Leaf) {
    Remove-Item -Path "$PSScriptRoot\rescan" -Force
}

$global:imageGroupArray = @()
$global:imageFilesArray = @()

$global:selectedImageGroup = ""
$selectedImageName = ""
$global:selectedImageFileName = ""

$ImgGroupCombo_SelectedIndexChanged = {
    if ($null -ne $global:images) {
        $global:selectedImageGroup = $global:imageGroupArray[$ImgGroupCombo.SelectedIndex]
        $global:imageFilesArray = ($global:images | Where-Object { $_.ImageGroup -eq "$global:selectedImageGroup" } | Group-Object -Property FileName).Name

        $ImgNameCombo.Items.Clear()
        $ImgNameCombo.Items.AddRange(($global:images | Where-Object { $_.ImageGroup -eq "$global:selectedImageGroup" } | Select-Object -ExpandProperty Name))
        $ImgNameCombo.SelectedIndex = 0
    }
}

$ImgNameCombo_SelectedIndexChanged = {
    if ($null -ne $global:images) {
        # When there's one image in a group the array becomes a string; we have to intervene here...
        $global:selectedImageFileName = ([string[]]$global:imageFilesArray)[$ImgNameCombo.SelectedIndex]

        $selectedImage = $global:images | Where-Object { $_.ImageGroup -eq "$global:selectedImageGroup" -and $_.FileName -eq "$global:selectedImageFileName" }

        $ImageDetailsTB.Text = @"
File Name: $($selectedImage.FileName)
Image Name: $($selectedImage.Name)
Image Description: $($selectedImage.Description)
Image Group: $($selectedImage.ImageGroup)
Size: $([Math]::Round($selectedImage.Size / 1GB, 2)) GB
Last Modification Time (UTC): $($selectedImage.LastModifyUtc)
Version: $($selectedImage.Version)
Priority in WDS server: $($selectedImage.Priority)
"@
    }
}

$Select_Button_Click = {
    $WDSHCImagePicker.DialogResult = 'OK'
    $WDSHCImagePicker.Close()
}

$Refresh_Button_Click = {
    New-Item -Path "$PSScriptRoot\rescan" -Force
    $WDSHCImagePicker.DialogResult = 'Cancel'
    $WDSHCImagePicker.Close()
}

Add-Type -AssemblyName System.Windows.Forms
. (Join-Path $PSScriptRoot 'wdshcGraphoView.designer.ps1') | Out-Null

$response = Get-Content -Path "$PSScriptRoot\images.json" | ConvertFrom-Json
if ($null -ne $response) {
    $global:images = $response
    $global:imageGroupArray = ($global:images | Group-Object -Property ImageGroup).Name
}

$ImgGroupCombo.Items.Clear()
$ImgGroupCombo.Items.AddRange($global:imageGroupArray)

# The combobox may start showing duplicate items. Remove them until we have enough
if ($ImgGroupCombo.Items.Count -gt $global:imageGroupArray.Count) {
    do {
        $ImgGroupCombo.Items.RemoveAt($global:imageGroupArray.Count)
    } until ($ImgGroupCombo.Items.Count -gt $global:imageGroupArray.Count)
}

$WDSHCImagePicker.ShowDialog() | Out-Null

if ($WDSHCImagePicker.DialogResult -eq 'OK') {
    # Output the selected information to a file
    @{"image" = $global:selectedImageFileName; "group" = $global:selectedImageGroup} | ConvertTo-Json | Out-File "$PSScriptRoot\selected.json" -Encoding UTF8 -Force
}