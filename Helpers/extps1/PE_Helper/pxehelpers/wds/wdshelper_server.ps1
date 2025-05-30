#requires -version 5.0
#requires -runasadministrator
#                                              ....
#                                         .'^""""""^.
#      '^`'.                            '^"""""""^.
#     .^"""""`'                       .^"""""""^.                ---------------------------------------------------------
#      .^""""""`                      ^"""""""`                  | DISMTools 0.7                                         |
#       ."""""""^.                   `""""""""'           `,`    | The connected place for Windows system administration |
#         '`""""""`.                 """""""""^         `,,,"    ---------------------------------------------------------
#            '^"""""`.               ^""""""""""'.   .`,,,,,^    | PE Helper - WDS Helper Web-based API for Servers      |
#              .^"""""`.            ."""""""",,,,,,,,,,,,,,,.    ---------------------------------------------------------
#                .^"""""^.        .`",,"""",,,,,,,,,,,,,,,,'     | (C) 2025 CodingWonders Software                       |
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
#
# Exposed APIs:
#
#   - /api/installimages --> Gets the install images in the WDS store
#   - /api/connect       --> Connects a client to a server
#
#         A client must send data to /api/connect like this (example in PowerShell):
#
#         $json = @{
#             deviceId = "<Device ID>"
#         } | ConvertTo-Json
#
#   - /api/deploy        --> Prepares a server for image deployment to a client
#
#         A client must send data to /api/deploy like this (example in PowerShell):
#
#         $json = @{
#             shareGuid = "<GUID for share, obtained with /api/connect>"
#             image_name = "<File name of image in WDS>"
#             image_group = "<WDS image group>"
#         } | ConvertTo-Json
#
#         This must then be sent as part of the body. Then, mount a network share that will be created to the WinPE
#
#   - /api/clearfiles    --> Clears all the files created during deployment preparation
#   - /api/exit          --> Gracefully close the program
#
#   Settings for the server are declared in the Server Options section.



# ----------------------- Server Options -----------------------
$webHost = "*"
$port = 8080
$tmpImageFolderPath = "$env:SystemDrive\NetInstallWDSTemp"
$shareName = "NetInstallTemp"
# --------------------------------------------------------------

function Write-LogMessage {
    param(
        [string]$message
    )
    Write-Host "[$(Get-Date)] $message"
}

function Get-WindowsRole {
    param(
        [Parameter(Mandatory = $true)] [string]$RoleName
    )
    Write-LogMessage -message "Detecting server role `"$RoleName`"..."
    return (Get-WindowsFeature | Where-Object { $_.Name -match "$RoleName" }).InstallState -eq "Installed"
}

[Console]::TreatControlCAsInput = $true

$version = "0.7"

Clear-Host

Write-Host "DISMTools $version - Windows Deployment Services Helper API"
Write-Host "(c) 2025. CodingWonders Software"
Write-Host "-----------------------------------------------------------"

Write-LogMessage -message "Checking operating environment..."
if ((Get-ComputerInfo).WindowsInstallationType -ne "Server") {
    Write-LogMessage -message "This computer is not running Windows Server."
    return $false
}

Write-LogMessage -message "Checking roles..."
if (((Get-WindowsRole -RoleName "WDS") -eq $false) -or ((Get-WindowsRole -RoleName "DHCP") -eq $false)) {
    Write-LogMessage -message "Some required roles are missing on this server. Make sure DHCP and WDS are installed."
    return $false
}

Write-LogMessage -message "Checking share locations..."
$wdsShareLocation = ""
$wdsShareLocation = (Get-ItemPropertyValue -Path "HKLM:\SYSTEM\CurrentControlSet\Services\LanmanServer\Shares" -Name "REMINST" -ErrorAction SilentlyContinue)[3].Replace("Path=", "")

Write-LogMessage -message "Starting Windows Deployment Services Web API..."
Write-LogMessage -message "Server Options:"
Write-LogMessage -message " - Web API Host: $webHost"
Write-LogMessage -message " - Web API Port to listen to: $port"
Write-LogMessage -message " - Temporary directory for deployment operations: $tmpImageFolderPath"
Write-LogMessage -message " - Name for SMB network share: $shareName"
Write-LogMessage -message "Creating firewall rules..."
$fwRule = $null
try {
    New-NetFirewallRule -DisplayName "Allow WDS listener on port $port" -Name "AllowListener" -Protocol TCP -LocalPort $port -Action Allow -ErrorAction Stop | Out-Null
    Write-LogMessage -message "Firewall rule creation succeeded. Continuing startup..."
} catch {
    Write-LogMessage -message "$_"
    Write-LogMessage -message "Could not add rule. Port $port may already be allowlisted. Check firewall settings before proceeding. The script, however, will continue"
}

$fwRule = Get-NetFirewallRule -Name "AllowListener"

$listener = [System.Net.HttpListener]::new()
$listener.Prefixes.Add("http://$($webHost):$port/api/")
$listener.Start()
Write-LogMessage -message "WDS REST API Listener running on http://$($webHost):$port/api/"
Write-LogMessage -message "To shut down, press CTRL + C and perform an API call. Alternatively, close the window"

# Function to get the list of WDS install images using native WDS cmdlets
function Get-WdsInstallImages {
    try {
        Write-LogMessage -message "Getting images from image groups in WDS store..."
        $imageGroups = Get-WdsInstallImageGroup
        $images = @()
        foreach ($group in $imageGroups) {
            $groupImages = Get-WdsInstallImage -ImageGroup $group.Name | Select-Object FileName, Name, Description, ImageGroup, Size, @{Name='Last Modification Time (UTC)'; Expression='LastModificationTime'}, Version, @{Name='Priority'; Expression='DisplayOrder'} | Sort-Object -Property Priority
            $images += $groupImages
        }
        Write-LogMessage -message "Returning $($images.Count) image(s)..."
        return $images
    } catch {
        throw $_
    }
}

class WdsConnectionInfo {
    [bool]$successful
    [string]$failureReason
    [string]$shareFolderGuid

    WdsConnectionInfo() {
        $this.successful = $false
        $this.failureReason = ""
        $this.shareFolderGuid = ""
    }

    WdsConnectionInfo($success, $failReason, $guid) {
        $this.successful = $success
        $this.failureReason = $failReason
        $this.shareFolderGuid = $guid
    }
}

class WdsShareAuthenticationInfo {
    [string]$server
    [string]$username
    [string]$mountPath

    WdsShareAuthenticationInfo() {
        $this.server = ""
        $this.username = ""
        $this.mountPath = ""
    }

    WdsShareAuthenticationInfo($srv, $usr, $mnt) {
        $this.server = $srv
        $this.username = $usr
        $this.mountPath = $mnt
    }
}

function Start-ServerConnection {
    param (
        [Parameter(Mandatory)] [string]$deviceId
    )
    try {
        Write-LogMessage -message "Checking if device is approved..."
        $allowedDeviceRequests = (Get-WdsClient -PendingClientStatus Approved)
        $blockedDeviceRequests = (Get-WdsClient -PendingClientStatus Denied)
        # Start with blocked devices
        if ((($blockedDeviceRequests | Where-Object { $_.DeviceID.Contains($deviceId) }) | Select-Object -ExpandProperty DeviceID).Count -ge 1) {
            Write-LogMessage -message "This device is blocked."
            return [WdsConnectionInfo]::new($false, "This device cannot connect to this server because its request has been denied in the WDS server", "")
        }
        # Continue with allowed devices. If it's not there, it's still pending or its status could not be obtained
        if ((($allowedDeviceRequests | Where-Object { $_.DeviceID.Contains($deviceId) }) | Select-Object -ExpandProperty DeviceId).Count -lt 1) {
            Write-LogMessage -message "This device is neither approved nor blocked."
            return [WdsConnectionInfo]::new($false, "This device cannot connect to this server because its approval is either pending or unknown", "")
        }
        return [WdsConnectionInfo]::new($true, "", [System.Guid]::NewGuid().Guid)
    } catch {
        throw $_
    }
}

# Function to deploy a WIM image to the target drive using native WDS cmdlets
function Deploy-WimImage {
    param(
        [string]$shareGuid,
        [string]$ImageName,
        [string]$ImageGroup
    )
    if ($shareGuid -eq "") {
        throw "The Share GUID cannot be empty."
    }
    Write-Progress -Activity "WDS Deployment Preparation Work" -Status "Please wait..." -PercentComplete 0
    Write-LogMessage -message "Preparing the deployment of a WIM file..."
    try {
        Write-Progress -Activity "WDS Deployment Preparation Work" -Status "Preparing NetInstall dir..." -PercentComplete 15
        Write-LogMessage -message "Preparing temporary NetInstall directory..."
        if (-not (Test-Path -Path "$tmpImageFolderPath")) {
            New-Item -Path "$tmpImageFolderPath" -ItemType Directory | Out-Null
        }
        if ((Get-SmbShare -Name "$shareName" -ErrorAction SilentlyContinue) -eq $null) {
            Remove-Item -Path "$tmpImageFolderPath\*.wim" -Recurse -Force -Verbose -ErrorAction SilentlyContinue
            Write-Progress -Activity "WDS Deployment Preparation Work" -Status "Creating SMB network share..." -PercentComplete 30
            Write-LogMessage -message "Setting network share..."
            # Create the SMB share if it doesn't exist
            if (((Get-SmbShare -Name "$shareName" -ErrorAction Ignore) | Select-Object -ExpandProperty Name).Count -le 0) {
                New-SmbShare -Path "$tmpImageFolderPath" -Name "$shareName" -ReadAccess 'EVERYONE' | Out-Null
            }
        }
        New-Item -Path "$tmpImageFolderPath\$shareGuid" -ItemType Directory | Out-Null
        Write-LogMessage -message "Beginning image export..."
        Write-Progress -Activity "WDS Deployment Preparation Work" -Status "Getting complete information about specified image..." -PercentComplete 45
        $installImage = (Get-WdsInstallImage -ImageGroup "$ImageGroup" -FileName "$ImageName")
        if ($installImage -eq $null) {
            throw "Image information could not be found"
        }
        Write-Progress -Activity "WDS Deployment Preparation Work" -Status "Exporting image to share..." -PercentComplete 60
        $wdsUtilProc = Start-Process "wdsutil" -ArgumentList " /verbose /progress /export-image /image:`"$($installImage.Name)`" /server:$($env:COMPUTERNAME) /imagetype:Install /imagegroup:`"$ImageGroup`" /filename:`"$ImageName`" /destinationimage /filepath:`"$tmpImageFolderPath\$shareGuid\$ImageName`" /name:`"$($installImage.Name)`" /overwrite:yes" -NoNewWindow -Wait -PassThru
        if ($wdsUtilProc.ExitCode -ne 0) {
            throw "WDSUtil Exited with Code $($wdsUtilProc.ExitCode)"
        }
        if (Test-Path -Path "$wdsShareLocation\Images\$($ImageGroup)\$([IO.Path]::GetFileNameWithoutExtension("$ImageName"))\Unattend\ImageUnattend.xml" -PathType Leaf) {
            Write-Progress -Activity "WDS Deployment Preparation Work" -Status "Copying answer file..." -PercentComplete 80
            try {
                Copy-Item -Path "$wdsShareLocation\Images\$($ImageGroup)\$([IO.Path]::GetFileNameWithoutExtension("$ImageName"))\Unattend\ImageUnattend.xml" -Destination "$tmpImageFolderPath\$shareGuid\unattend.xml"
            } catch {
                Write-LogMessage "Could not copy unattended answer file. The target installation will not be unattended"
            }
        }
        Write-Progress -Activity "WDS Deployment Preparation Work" -Status "Finishing up..." -PercentComplete 90
        $authInfo = [WdsShareAuthenticationInfo]::new("$env:COMPUTERNAME", "$env:USERNAME", "\\$env:COMPUTERNAME\$shareName")
        Write-Progress -Activity "WDS Deployment Preparation Work" -Completed
        return $authInfo
    } catch {
        Write-Progress -Activity "WDS Deployment Preparation Work" -Completed
        throw $_
    }
}

function Clear-Files {
    $smbShare = Get-SmbShare -Name "$shareName" -ErrorAction SilentlyContinue
    $smbShare | Remove-SmbShare -Force -ErrorAction SilentlyContinue
    Remove-Item -Path "$tmpImageFolderPath" -Recurse -Force -Verbose -ErrorAction SilentlyContinue
}

$shutdownRequested = $false
$shutdownEvent = New-Object System.Threading.ManualResetEvent $false

$ctrlC_EH = [ConsoleCancelEventHandler]{
    param($sender, $args)

    $shutdownRequested = $true
    throw
}

try {
    while (-not $shutdownRequested) {
        if ($host.UI.RawUI.KeyAvailable -and (3 -eq [int]$host.UI.RawUI.ReadKey("AllowCtrlC,IncludeKeyUp,NoEcho").Character)) {
            Write-LogMessage -message "CTRL + C key pressed"
            $shutdownRequested = $true
            throw
        }
        Write-LogMessage -message "Ready to listen..."
        $context = $listener.GetContext()
        $request = $context.Request
        $response = $context.Response

        $sendJson = {
            param($data, $status = 200)
            $response.StatusCode = $status
            $response.ContentType = "application/json"
            $json = $data | ConvertTo-Json -Depth 4
            $buffer = [System.Text.Encoding]::UTF8.GetBytes($json)
            $response.ContentLength64 = $buffer.Length
            $response.OutputStream.Write($buffer, 0, $buffer.Length)
            $response.OutputStream.Close()
        }

        Write-LogMessage -message "Requested API path: $($request.Url.AbsolutePath)"
        Write-LogMessage -message "API method: $($request.HttpMethod)"

        switch -Wildcard ($request.Url.AbsolutePath) {
            "/api/installimages" {
                if ($request.HttpMethod -eq "GET") {
                    try {
                        $images = Get-WdsInstallImages
                        $sendJson.Invoke(@{ success = $true; images = $images })
                    } catch {
                        Write-LogMessage -message "Exception caught: $_"
                        $sendJson.Invoke(@{ success = $false; error = $_.Exception.Message }, 500)
                    }
                } else {
                    $sendJson.Invoke(@{ error = "Method not allowed" }, 405)
                }
            }
            "/api/connect" {
                if ($request.HttpMethod -eq "POST") {
                    try {
                        $reader = New-Object IO.StreamReader $request.InputStream
                        $body = $reader.ReadToEnd() | ConvertFrom-Json
                        $deviceId = $body.deviceId

                        $result = Start-ServerConnection -deviceId $deviceId
                        if ($result -ne $null) {
                            $sendJson.Invoke(@{ success = $result.successful; output = $result })
                        }
                    } catch {
                        Write-LogMessage -message "Exception caught: $_"
                        $sendJson.Invoke(@{ success = $false; error = $_.Exception.Message }, 500)
                    }
                }
            }
            "/api/deploy" {
                if ($request.HttpMethod -eq "POST") {
                    try {
                        $reader = New-Object IO.StreamReader $request.InputStream
                        $body = $reader.ReadToEnd() | ConvertFrom-Json
                        $guid = $body.shareGuid
                        $imageName = $body.image_name
                        $imageGroup = if ($body.image_group) { $body.image_group } else { "ImageGroup1" }

                        $output = Deploy-WimImage -shareGuid $guid -ImageName $imageName -ImageGroup $imageGroup
                        $sendJson.Invoke(@{ success = $true; output = $output })
                    } catch {
                        Write-LogMessage -message "Exception caught: $_"
                        $sendJson.Invoke(@{ success = $false; error = $_.Exception.Message }, 500)
                    }
                } else {
                    $sendJson.Invoke(@{ error = "Method not allowed" }, 405)
                }
            }
            "/api/clearfiles" {
                if ($request.HttpMethod -eq "GET") {
                    try {
                        $output = Clear-Files
                        $sendJson.Invoke(@{ success = $true; output = $output })
                    } catch {
                        Write-LogMessage -message "Exception caught: $_"
                        $sendJson.Invoke(@{ success = $false; error = $_.Exception.Message }, 500)
                    }
                } else {
                    $sendJson.Invoke(@{ error = "Method not allowed" }, 405)
                }
            }
            "/api/exit" {
                $sendJson.Invoke(@{ success = $true })
                throw
            }
            default {
                $sendJson.Invoke(@{ error = "Not found" }, 404)
            }
        }
    }
} catch {
    # Do nothing
} finally {
    Write-LogMessage -message "Shutting down..."
    Clear-Files | Out-Null
    $listener.Stop()
    if ($fwRule -ne $null) {
        Get-NetFirewallRule -Name $($fwRule.Name) | Remove-NetFirewallRule
    }
}

# Clean up
Write-LogMessage -message "Stopping listener..."
$listener.Stop()

Write-LogMessage -message "Shutdown complete."
