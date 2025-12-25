#requires -version 5.0
#requires -runasadministrator

# FOG Helper

. "$PSScriptRoot\..\Common\PXEHelpers.Common.ps1"

class ServerAuthentication {
    [string]$serverIP
    [string]$serverPort
    [string]$serverUnderlyingFogServerIP

    ServerAuthentication($ip, $port, $underlyingIp) {
        $this.serverIP = $ip
        $this.serverPort = $port
        $this.serverUnderlyingFogServerIP = $underlyingIp

        if ((Test-IPAddressSyntax -ipAddr $this.serverIP) -eq [IPAddress]::IPv6) {
            $this.serverIP = "[$($this.serverIP)]"
        }
    }
}

function Invoke-ServerAuthentication {
    $ip = Read-Host -Prompt "Please enter the IP address of the server"
    $port = Read-Host -Prompt "Please enter the port to which the FOG Helper Web API is listening"
    $underlyingIp = Read-Host -Prompt "Please enter the IP address of the server running FOG (different from the first IP)"
    return [ServerAuthentication]::new($ip, $port, $underlyingIp)
}

function Select-InstallImage {
    param (
        [Parameter(Mandatory, Position = 0)] [System.Object]$images
    )

    Show-SectionMessage -sectionTitle "Choose an installation image" -sectionDescription "Either no images are associated to this host or you decided to pick another image. Select the image you want to deploy to this system:"

    # We'll show a table instead of a list so we save up screen space
    $images.images | Select-Object @{Name='#'; Expression='id'}, @{Name='Name'; Expression='name'}, @{Name='Description'; Expression='description'}, @{Name='Creation Time'; Expression='createdTime'}, @{Name='Created By'; Expression='createdBy'}, @{Name='Enabled'; Expression='isEnabled'} | Format-Table -AutoSize | Out-Host

    $validated = $false
    $idx = 0

    do {
        try {
            $idxStr = Read-Host -Prompt "Specify an image by its index property (`"#`") and press ENTER"
            $idx = [int]$idxStr
            if ($idx -lt 1) { throw }   # The user has entered bogus data

            # Let's check if the image is enabled in FOG
            if ((($images.images | Where-Object { $_.id -eq $idx }).isEnabled) -ne 1) {
                # This FOG API module is sure as hell basic. Interpreting reals as booleans instead of proper booleans
                # 1 = True
                # 0 = False
                Write-Host "The selected image is not enabled."
                throw
            }

            $validated = $true
        } catch {
            Write-Host "Could not validate option. Try again"
            $validated = $false
        }
    } until ($validated -eq $true)

    return $idx
}

$global:product = "FOG Helper"
$global:description = "This script will guide you through the process of deploying an operating system via a FOG server."

if ((Get-ItemPropertyValue -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion" -Name "EditionID") -ne "WindowsPE") {
    Show-CenteredTextBox -Text "This script is intended to be run in Windows PE. Please restart your device and boot into Windows PE to continue. Press ENTER to exit . . ." -MaxWidth 100 -CenterOfAll -ForegroundColor DarkRed
    Read-Host | Out-Null
    exit 1
}

$host.UI.RawUI.WindowTitle = "Preboot eXecution Environment Helpers: $($global:product)"

Show-SectionMessage -sectionTitle "Welcome to Setup." -sectionDescription "$global:description Make sure that your device meets the requirements for a network-based deployment. Afterwards, networking will be initialized."

Show-CenteredTextBox -Text "Please plug in your Ethernet cable." -MaxWidth 100 -ForegroundColor DarkYellow
Write-Host "`n"
Show-CenteredTextBox -Text "Press ENTER to initialize the network stack and start the installation process . . ." -MaxWidth 100
Read-Host | Out-Null

Enable-Networking

Write-Host "Preparing to connect to the FOG server..."

Show-SectionMessage -sectionTitle "Connect to the server" -sectionDescription "Please start the FOG Helper Web API on your Windows server. You can find it in the `"pxehelpers\fog`" folder on the install disc. After startup, provide server authentication information that will be used to communicate with the server and the API."

$authInfo = Invoke-ServerAuthentication

$computerUUID = (Get-WmiObject Win32_ComputerSystemProduct).UUID.ToLower()

$hostResult = $null
$attempts = 0
do {
    try {
        Show-CenteredTextBox -Text "Connecting to the server . . . (Attempt $($attempts + 1) of 5)" -MaxWidth 100 -CenterOfAll
        $hostResult = Invoke-RestMethod -Method Get -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/hosts"
    } catch {
        # Could not connect to the server. Try again.
    }
    $attempts++
    if ($attempts -ge 5) {
        break
    }
} until ($hostResult -ne $null)

if (($hostResult -eq $null) -or ($hostResult.success -eq $false)) {
    Show-CenteredTextBox -Text "Could not connect to the server. The server has imposed a block of 2 minutes for this device. Wait 2 minutes, then try again." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
    Start-Sleep -Seconds 5
    wpeutil reboot
}

# We'll set this variable to false to begin with. Then we'll set it to the actual value.
# That way we prevent having to deal with null variables. This variable determines whether the
# server is a Windows or a UNIX system.
$isWindows = $false
$isWindows = (Invoke-RestMethod -Method Get -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/getostype").platform -eq 2       # 2 = System.PlatformID.Win32NT

$hosts = $hostResult.hosts

$hostId = ($hosts | ForEach-Object { $_.inventory | Where-Object { $_.sysuuid -eq "$computerUUID" } }).hostId

if ($hostId -eq $null) {
    Show-CenteredTextBox -Text "This device doesn't appear to be registered in the FOG server. Either register the device using the management control panel or perform a capture task. More information can be found in the FOG Project documentation." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
    Start-Sleep -Seconds 5
    wpeutil reboot
}

Show-CenteredTextBox -Text "Getting images from install groups in the FOG server . . ." -MaxWidth 100 -CenterOfAll
$installImages = Invoke-RestMethod -Method Get -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/getAllImages"

if (($installImages -eq $null) -or ($installImages.success -eq $false)) {
    Show-CenteredTextBox -Text "Could not get installation images. The server may have imposed a block of 2 minutes for this device. Wait 2 minutes, then try again." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
    Start-Sleep -Seconds 5
    wpeutil reboot
} elseif ($installImages.images.Count -lt 1) {
    Show-CenteredTextBox -Text "The FOG server does not appear to have any images to deploy" -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
    Start-Sleep -Seconds 5
    wpeutil reboot
}

Show-CenteredTextBox -Text "Determining if this host has install images linked to it..."
$imageLinkBody = @{ hostId = $hostId } | ConvertTo-Json

$imageLinkResults = Invoke-RestMethod -Method Post -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/getImagesForHost" -Body $imageLinkBody

if (($imageLinkResults -eq $null) -or ($imageLinkResults.success -eq $false)) {
    Show-CenteredTextBox -Text "We could not grab any install images linked to the host -- perhaps there aren't any, or there is a problem with the API. Contact the developers about this issue if it's the latter." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
    Start-Sleep -Seconds 5
    wpeutil reboot
}

# If the ID of the associated image is null, then we know there's no image associated to this host. Otherwise,
# we let the user either roll with that or choose one of the available images.
if (($imageLinkResults.output.id) -ne $null) {
    Show-SectionMessage -sectionTitle "Choose an installation image" -sectionDescription "Based on how you configured your installation images, we have determined this image to be ideal for this host. You can pick this one to deploy it, or you can choose from the complete list of images."

    $imageLinkResults.output | Select-Object @{Name='#'; Expression='id'}, @{Name='Name'; Expression='name'}, @{Name='Description'; Expression='description'}, @{Name='Creation Time'; Expression='createdTime'}, @{Name='Created By'; Expression='createdBy'} | Format-List | Out-Host

    Write-Host "`n"
    Write-Host "    Press [Y] to accept this image"
    Write-Host "    Press [N] to choose another image"

    if ((Read-Host -Prompt "Do you want to choose this image? [Y/N]") -eq "y") {
        $imageId = $imageLinkResults.output.id
    } else {
        $imageId = Select-InstallImage -images $installImages
    }
} else {
    $imageId = Select-InstallImage -images $installImages
}

$deployTaskBody = @{
    hostId = $hostId
    imageId = $imageId
} | ConvertTo-Json

Show-CenteredTextBox -Text "Initiating deployment task..." -CenterOfAll

$deployTaskResults = Invoke-RestMethod -Method Post -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/deploy" -Body $deployTaskBody

if (($deployTaskResults -eq $null) -or ($deployTaskResults.success -eq $false)) {
    Show-CenteredTextBox -Text "Could not initiate the deployment task. Check the FOG Helper Server logs for more information." -MaxWidth 70 -CenterOfAll -ForegroundColor DarkRed
    Start-Sleep -Seconds 5
    wpeutil reboot
}

Show-SectionMessage -sectionTitle "The first portion is complete" -sectionDescription "The deployment task has been successfully initiated. Now, follow these steps:"

Show-CenteredTextBox -Text "You have completed the first portion of the FOG Helper. To continue the deployment of this image, the computer needs to boot into a Linux-based environment to restore all the partitions from the image into your machine. Follow these steps to initiate the second stage. (You can take a picture of this screen in case you'll forget these steps)" -MaxWidth 100 -ForegroundColor DarkGreen

Write-Host "1. Power off the machine. It will do so when you press ENTER"
Write-Host "2. Configure DHCP settings in your server to boot to the Linux environment. The FOG Helper can attempt to do this for you based on the information you provided (WINDOWS SYSTEMS ONLY)"
Write-Host "   NOTE: this will configure global DHCP server settings that may be overridden by scopes you may have created. Revise your configuration" -ForegroundColor DarkYellow
Write-Host "3. Configure the boot procedure of the target machine to boot via the network, if you haven't already done so."
Write-Host "   NOTE: you may need to turn off Secure Boot on the target machine for the Linux environment to work" -ForegroundColor DarkYellow

Show-CenteredTextBox -Text "Afterwards, deployment will become an automated process that may take some time, depending on network connection and computer speeds." -MaxWidth 100 -ForegroundColor DarkGreen

Write-Host "Press ENTER to configure global DHCP server settings and shut down your computer . . ."
Read-Host | Out-Null

if ($isWindows) {
    Show-CenteredTextBox -Text "Setting DHCP global options 66 and 67 . . ." -MaxWidth 75 -CenterOfAll

    $dhcpSetterBody = @{ fogIp = "$($authInfo.serverUnderlyingFogServerIP)" } | ConvertTo-Json
    $dhcpSetterResults = Invoke-RestMethod -Method Post -Uri "http://$($authInfo.serverIP):$($authInfo.serverPort)/api/setDhcp" -Body $dhcpSetterBody

    if (($dhcpSetterResults -eq $null) -or ($dhcpSetterResults.success -eq $false)) {
        Show-CenteredTextBox -Text "DHCP settings could not be set. You will have to set them manually." -MaxWidth 75 -CenterOfAll -ForegroundColor DarkYellow
        Write-Host "Use these options:"
        Write-Host "- DHCP Option 66 (Boot Server Host Name): `"$($authInfo.serverUnderlyingFogServerIP)`""
        Write-Host "- DHCP Option 67 (Bootfile Name): `"ipxe.efi`" (note that you may need to use a different value based on your setup, since this assumes all target machines use UEFI)"

        Write-Host "`nJot down these steps, and press ENTER to shut down the machine."
        Read-Host | Out-Null
    }
}

Show-CenteredTextBox -Text "Shutting down . . ." -MaxWidth 75 -CenterOfAll
wpeutil shutdown
