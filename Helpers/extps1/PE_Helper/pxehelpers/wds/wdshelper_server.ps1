#requires -version 5.0
#requires -runasadministrator
#                                              ....
#                                         .'^""""""^.
#      '^`'.                            '^"""""""^.
#     .^"""""`'                       .^"""""""^.                ---------------------------------------------------------
#      .^""""""`                      ^"""""""`                  | DISMTools 0.8                                         |
#       ."""""""^.                   `""""""""'           `,`    | The connected place for Windows system administration |
#         '`""""""`.                 """""""""^         `,,,"    ---------------------------------------------------------
#            '^"""""`.               ^""""""""""'.   .`,,,,,^    | PE Helper - Windows Deployment Services Helper Server |
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

param (
    [int] $sPort = 8080
)

# ----------------------- Server Options -----------------------
$webHost = "*"
$port = $sPort
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

$version = "0.8"

Clear-Host

$tempDir = [IO.Path]::GetTempPath().TrimEnd("\")

# Start logging stuff
Start-Transcript -Path "$tempDir\DT_WDSHS_Log.log" -Append -NoClobber | Out-Null

Write-Host "DISMTools $version - Windows Deployment Services Helper Server"
Write-Host "(c) 2025-2026. CodingWonders Software"
Write-Host "-----------------------------------------------------------"

Write-LogMessage -message "Checking operating environment..."

if ([Environment]::OSVersion.Platform -ne "Win32NT") {
    Write-Host "This script cannot be run on non-Windows NT platforms. Press ENTER to exit..."
    Read-Host | Out-Null
    return $false
}

$compInfo = Get-ComputerInfo
if ($compInfo.WindowsInstallationType -notlike "Server*") {
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
$wdsShareLocation = Get-ItemPropertyValue -Path "HKLM:\SYSTEM\CurrentControlSet\Services\WDSServer\Providers\WDSTFTP" -Name "RootFolder" -ErrorAction SilentlyContinue

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
Write-LogMessage -message "To shut down, click the `"Stop Server`" button in the Control Panel, which you can access at any time while the server is running at http://localhost:$port/api/wdshome."

# Function to get the list of WDS install images using native WDS cmdlets
function Get-WdsInstallImages {
    try {
        Write-LogMessage -message "Getting images from image groups in WDS store..."
        $imageGroups = Get-WdsInstallImageGroup
        $images = @()
        foreach ($group in $imageGroups) {
            $groupImages = Get-WdsInstallImage -ImageGroup $group.Name | Select-Object FileName, Name, Description, ImageGroup, Size, @{Name='LastModifyUtc'; Expression='LastModificationTime'}, Version, @{Name='Priority'; Expression='DisplayOrder'} | Sort-Object -Property Priority
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
        $allowedDeviceRequests = (Get-WdsClient -PendingClientStatus Approved -ErrorAction Stop)
        $blockedDeviceRequests = (Get-WdsClient -PendingClientStatus Denied -ErrorAction Stop)
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
        if ($Error[0].FullyQualifiedErrorId.Split(",")[0] -eq "0xC1050100") {
            # The Pending Devices policy is not set up. Immediately return true
            return [WdsConnectionInfo]::new($true, "", [System.Guid]::NewGuid().Guid)
        } else {
            throw $_
        }
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
    try {
        $conv = [Guid]$shareGuid
    } catch {
        throw "Invalid format for GUID."
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
        $wdsUtilProc = Start-Process "wdsutil" -ArgumentList " /verbose /progress /export-image /image:`"$($installImage.Name)`" /server:$($env:COMPUTERNAME) /imagetype:Install /imagegroup:`"$ImageGroup`" /filename:`"$ImageName`" /destinationimage /filepath:`"$tmpImageFolderPath\$shareGuid\install.wim`" /name:`"$($installImage.Name)`" /overwrite:yes" -NoNewWindow -Wait -PassThru
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

function Remove-SharedFolderByGuid {
    param (
        [Parameter(Mandatory)] [string]$guid
    )
    if ($guid -eq "") {
        return $false
    }
    try {
        $conv = [Guid]$guid
    } catch {
        return $false
    }
    Remove-Item -Path "$tmpImageFolderPath\$guid" -Recurse -Force -Verbose -ErrorAction SilentlyContinue
    return $true
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

Start-Process "http://localhost:$port/api/wdshome"

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

        $response.Headers.Add("Access-Control-Allow-Origin", "*")
        $response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS")
        $response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization")

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
            "/api/wdshome" {
                $teamingStatus = ""
                $nicStatus = ""
                $netAdapter = Get-NetIPConfiguration
                if (($netAdapter -ne $null) -and (($netAdapter | Select-Object -ExpandProperty InterfaceAlias).Count -gt 0)) {
                    $nicStatus += "$(($netAdapter | Select-Object -ExpandProperty InterfaceAlias).Count) network adapters were detected:`n<ul>`n"
                    foreach ($nic in $netAdapter) {
                        $nicStatus += @"
                        <li>$($nic.InterfaceAlias -join "; "): $($nic.InterfaceDescription -join "; "):
                            <ul>
                                <li><abbr title="Internet Protocol, version 4">IPv4</abbr> Address: $($nic.IPv4Address.IPAddress -join "; "). Default Gateway: $($nic.IPv4DefaultGateway.NextHop -join "; ")</li>
                                <li><abbr title="Media Access Control">MAC</abbr> Address: $($nic.NetAdapter.LinkLayerAddress -join "; ")</li>
                                <li><abbr title="Internet Protocol, version 6">IPv6</abbr> Link-Local: $($nic.IPv6LinkLocalAddress.IPAddress -join "; "). Default Gateway: $($nic.IPv6DefaultGateway.NextHop -join "; ")</li>
                                <li><abbr title="Domain Name System">DNS</abbr> Servers:
                                    <ul>
                                        $($nic.DNSServer | Where-Object { $_.ServerAddresses.Count -gt 0 } | Foreach-Object {
                                            "<li>$($_.InterfaceAlias -join "; "): $($_.ServerAddresses -join "; ")</li>"
                                        })
                                    </ul>
                                </li>
                                <li>Adapter Speed: $($nic.NetAdapter.LinkSpeed -join "; ")</li>
                            </ul>
                        </li>
"@
                    }
                    $nicStatus += "</ul>"
                } else {
                    $nicStatus += "No network adapters were detected."
                }
                $lbfoTeams = Get-NetLbfoTeam
                if (($lbfoTeams -ne $null) -and (($lbfoTeams | Select-Object -ExpandProperty Name).Count -gt 0)) {
                    $teamingStatus += "$(($lbfoTeams | Select-Object -ExpandProperty Name).Count) NIC teams were set up:`n<ul>`n"
                    foreach ($lbfoTeam in $lbfoTeams) {
                        $teamingStatus += @"
                        <li>$($lbfoTeam.Name)
                            <ul>
                                <li>Members: $($lbfoTeam.Members -join ", ")</li>
                                <li>Teaming Mode: $($lbfoTeam.TeamingMode)</li>
                                <li>Load Balancing Algorithm: $($lbfoTeam.LoadBalancingAlgorithm)</li>
                                <li>Status: $($lbfoTeam.Status)</li>
                            </ul>
                        </li>
"@
                    }
                    $teamingStatus += "</ul>"
                } else {
                    $teamingStatus += "No NIC teams were set up."
                }
                $wsm_html = @"
                <!DOCTYPE html>
                <html>
                    <head>
                        <title>Server Control Panel</title>
                        <meta charset="utf8">
                        <style>
                            * {
                                margin: 0;
                            }
                            body {
                                font-size: 1em;
                                font-family: "Trebuchet MS", "Arial", "Helvetica", sans-serif;
                                display: grid;
                                grid-template-areas:
                                    "header"
                                    "osinfo"
                                    "actions";
                                grid-template-rows: 96px auto 72px;
                                height: 100vh;
                            }
                            button {
                                font-size: 1.125em;
                                font-family: "Trebuchet MS", "Arial", "Helvetica", sans-serif;
                            }
                            button:disabled {
                                background-color: darkgray;
                                color: white;
                            }
                            .important_tab {
                                font-weight: bold;
                            }
                            .important_important_tab {
                                font-weight: bold;
                                text-decoration: underline;
                            }
                            .super_duper_important_tab {
                                font-weight: bold;
                                text-decoration: underline;
                                background-color: gold;
                                color: black;
                            }
                            .header {
                                grid-area: header;
                                padding: 8px;
                                background-color: black;
                                color: white;
                            }
                            .osinfo {
                                grid-area: osinfo;
                                overflow-y: auto;
                                padding: 4px;
                                background-color: #333;
                                color: white;
                            }
                            .actions {
                                grid-area: actions;
                                padding: 8px;
                                background-color: black;
                                color: white;
                            }
                            abbr {
                                cursor: help;
                            }
                            a {
                                text-decoration: none;
                                color: white;
                                transition: 0.25s ease-in-out;
                            }
                            a:hover {
                                text-decoration: underline;
                                color: lightblue;
                            }
                        </style>
                    </head>
                    <body>
                        <div class="header">
                            <h1>Windows Deployment Services Helper Server Control Panel</h1>
                            <p>WDSH Server component version: $version; included with DISMTools $($version).</p>
                        </div>
                        <div class="osinfo">
                            <fieldset>
                                <legend>Information about the REST API Server</legend>
                                <table border="0" cellspacing="2" cellpadding="4">
                                    <tr>
                                        <td class="important_tab">API Host:</td>
                                        <td>$webHost (use $(($netAdapter | Where-Object { $_.NetIPv4Interface.Dhcp -eq 'Disabled' }).IPv4Address.IPAddress -join ", or ") when connecting to it from other clients). <a onclick="displayChooserMessage()" href="#">Which do I choose if I see multiple addresses?</a></td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Port to listen to:</td>
                                        <td>$port</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">User that started the server:</td>
                                        <td>$env:USERNAME</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="super_duper_important_tab">IMPORTANT! You will need to provide the aforementioned information when connecting from clients</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Temporary folder for network shares:</td>
                                        <td>$tmpImageFolderPath</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Share Name:</td>
                                        <td>$shareName</td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Information about the Operating Environment</legend>

                                <table border="0" cellspacing="2" cellpadding="4">
                                    <tr>
                                        <td colspan="2" class="important_important_tab">System Hardware</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">System:</td>
                                        <td>$($compInfo.CsManufacturer) $($compInfo.CsModel)</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Processor:</td>
                                        <td>$($compInfo.CsProcessors[0].Name)</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Memory:</td>
                                        <td>$(((Get-CimInstance Win32_PhysicalMemory) | Measure-Object -Property Capacity -Sum).Sum/1MB) MB</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">NTFS Volumes:</td>
                                        <td>
                                            <ul>
                                            $(Get-Volume | Where-Object { $_.DriveType -eq "Fixed" -and $_.FileSystemType -eq "NTFS" -and $_.DriveLetter -ne $null } | Foreach-Object {
                                                "<li>Drive $($_.DriveLetter) (`"$($_.FileSystemLabel)`"). Size: $([Math]::Round($_.Size / 1073741824, 2)) GB (percentage remaining: $([Math]::Round(($_.SizeRemaining / $_.Size) * 100, 2))%)</li>"
                                            })
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Current Network Adapters (NICs):</td>
                                        <td>$nicStatus</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">NIC Teaming:</td>
                                        <td>$teamingStatus</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="important_important_tab">System Software</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Operating system:</td>
                                        <td>$($compInfo.OsName) (NT Version: $($compInfo.OsVersion). Extended Build String: $($compInfo.WindowsBuildLabEx))</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Build Type:</td>
                                        <td>$($compInfo.OsBuildType)</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Installed HotFixes:</td>
                                        <td>
                                            <ul>
                                                $($compInfo.OsHotFixes | ForEach-Object {
                                                    "<li>$($_.HotFixID): $($_.Description). Installed on: $($_.InstalledOn)</li>"
                                                })
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">OS Suites:</td>
                                        <td>$($compInfo.OsSuites -join ", ")</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Logon Server:</td>
                                        <td>$($compInfo.LogonServer)</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Available Environment Variables:</td>
                                        <td style="word-wrap: break-word; word-break: break-all; white-space: normal">
                                            <ul>
                                                $(Get-ChildItem "ENV:" | ForEach-Object {
                                                    "<li><b>$($_.Name)</b>: $($_.Value)</li>"
                                                })
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Service Pack Version:</td>
                                        <td>$($compInfo.OsServicePackMajorVersion).$($compInfo.OsServicePackMinorVersion)</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Server Level:</td>
                                        <td>$($compInfo.OsServerLevel)</td>
                                    </tr>
                                    <tr>
                                        <td class="important_tab">Architecture:</td>
                                        <td>$($compInfo.OsArchitecture)</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <div class="actions">
                            Actions:
                            <button onclick="invokeCleanup()">Clear Temporary Files</button>
                            <button onclick="invokeExit()">Stop the Server</button>
                            <button onclick="invokeLogViewer()">View Server Logs</button>
                            <button onclick="window.location.reload();">Refresh Page</button>
                            Other actions exposed by the API can only be called by clients.
                            <p align="right"><i>&copy; 2025-2026. <a href="https://github.com/CodingWonders" target="_blank">CodingWonders Software</a></i></p>
                        </div>

                        <script>
                            function invokeCleanup() {
                                fetch('/api/clearfiles', { method: "GET" });
                            }

                            function invokeExit() {
                                fetch('/api/exit', { method: "GET" });
                                alert("The server has stopped. Close this tab now.");
                                let buttons = document.getElementsByTagName("button");
                                for (let i = 0; i < buttons.length; i++) {
                                    buttons[i].disabled = true;
                                }
                            }

                            function invokeLogViewer() {
                                fetch('/api/viewlogs', { method: "GET" });
                            }

                            function displayChooserMessage() {
                                alert("Note the IP address that the client displays when booting to a boot image. Typically, these aren't affected by any DHCP scopes.");
                            }
                        </script>
                    </body>
                </html>
"@
                $buffer = [System.Text.Encoding]::UTF8.GetBytes($wsm_html)
                $response.ContentType = "text/html"
                $response.ContentLength64 = $buffer.Length
                $response.OutputStream.Write($buffer, 0, $buffer.Length)
                $response.OutputStream.Close()
            }
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
            "/api/clearbyguid" {
                if ($request.HttpMethod -eq "POST") {
                    try {
                        $reader = New-Object IO.StreamReader $request.InputStream
                        $body = $reader.ReadToEnd() | ConvertFrom-Json
                        $guid = $body.shareGuid

                        $output = Remove-SharedFolderByGuid -guid $guid
                        $sendJson.Invoke(@{ success = $true })
                    } catch {
                        Write-LogMessage -message "Exception caught: $_"
                        $sendJson.Invoke(@{ success = $false; error = $_.Exception.Message }, 500)
                    }
                } else {
                    $sendJson.Invoke(@{ error = "Method not allowed" }, 405)
                }
            }
            "/api/viewlogs" {
                notepad "$tempDir\DT_WDSHS_Log.log"
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
Stop-Transcript
