#requires -version 5.0
#requires -runasadministrator
#                                              ....
#                                         .'^""""""^.
#      '^`'.                            '^"""""""^.
#     .^"""""`'                       .^"""""""^.                ---------------------------------------------------------
#      .^""""""`                      ^"""""""`                  | DISMTools 0.8                                         |
#       ."""""""^.                   `""""""""'           `,`    | The connected place for Windows system administration |
#         '`""""""`.                 """""""""^         `,,,"    ---------------------------------------------------------
#            '^"""""`.               ^""""""""""'.   .`,,,,,^    | PE Helper - FOG Helper Web-based API for Servers      |
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
#   - /api/foghome          --> The Control Panel
#   - /api/fogsetup         --> Configures the FOG PowerShell module to communicate with the FOG server
#   - /api/hosts            --> Gets all the registered hosts in the FOG server
#   - /api/getImagesForHost --> Gets all the images that are associated to a given host
#
#         A client must send data to /api/getImagesForHost like this (example in PowerShell):
#
#         $json = @{
#             hostId = "<Host ID in FOG server>"
#         } | ConvertTo-Json
#
#   - /api/getAllImages     --> Gets all the registered images in the FOG server
#   - /api/deploy           --> Initiates a deployment task on the FOG server
#
#         A client must send data to /api/deploy like this (example in PowerShell):
#
#         $json = @{
#             hostId = "<Host ID in FOG server>"
#             imageId = "<Image ID in FOG server>"
#         } | ConvertTo-Json
#
#   - /api/setDhcp          --> (Windows Server ONLY) Sets the IP address in the DHCP server to initiate the second stage of image deployment (Linux-based)
#
#         A client must send data to /api/setDhcp like this (example in PowerShell):
#
#         $json = @{
#             fogIp = "<IP of underlying FOG server>"
#         } | ConvertTo-Json
#
#         This is only valid when starting the WINDOWS version of this script.
#
#   - /api/viewlogs         --> Views the logs recorded by the script (ONLY ACCESSIBLE BY SERVERS. DO NOT ACCESS ON CLIENTS!)#
#   - /api/exit             --> Gracefully close the program
#
#   Settings for the server are declared in the Server Options section.

param (
    [int] $sPort = 8080
)


# ----------------------- Server Options -----------------------
$webHost = "*"
$port = $sPort
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

function Invoke-FogModuleAvailabilityPreparation {
    # First, we'll check if the module is already imported. If not,
    # we'll check available modules. If not there, we'll install it.
    # If we couldn't install it, determine if we have a local copy from __ModuleSetup
    Write-LogMessage -message "Determining FOG Module availability..."
    if ((Get-Module -Name FogApi).Count -lt 1) {
        Write-LogMessage -message "No FOG modules are available. Attempting to import them..."
        Import-Module FogApi -Force -ErrorAction SilentlyContinue
        if ($?) { return $true }
    } else {
        Write-LogMessage -message "A FOG module is imported."
        return $true
    }

    if ((Get-Module -Name FogApi -ListAvailable).Count -lt 1) {
        Write-LogMessage -message "No FOG modules are available on the system. Attempting to install and import them..."
        Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force
        Set-PSRepository -Name PSGallery -InstallationPolicy Trusted
        Install-Module FogApi -Force
        if ($?) {
            Write-LogMessage -message "Installation succeeded. Attempting to import them..."
            Import-Module FogApi -Force -ErrorAction SilentlyContinue
            if ($?) { return $true }
        }
    }

    if ((Test-Path -Path "$PSScriptRoot\fogready" -PathType Leaf) -and (Test-Path -Path "$PSScriptRoot\fogapi")) {
        Write-LogMessage -message "A portable FOG module is available. Attempting to import it..."
        Import-Module -Name "$PSScriptRoot\FogApi\FogApi.psd1"
        if ($?) { return $true }
    }

    return $false
}

function Get-FogHosts {
    return Get-FogObject -type object -coreObject host
}

[Console]::TreatControlCAsInput = $true

$version = "0.8"

Clear-Host

$tempDir = [IO.Path]::GetTempPath().TrimEnd("\")

# Start logging stuff
Start-Transcript -Path "$tempDir\DT_FOGHS_Log.log" -Append -NoClobber | Out-Null

Write-Host "DISMTools $version - FOG Helper Server"
Write-Host "(c) 2025-2026. CodingWonders Software"
Write-Host "-----------------------------------------------------------"

Write-LogMessage -message "Checking operating environment..."
$compInfo = Get-ComputerInfo
if ($compInfo.WindowsInstallationType -notlike "Server*") {
    Write-LogMessage -message "This computer is not running Windows Server."
    return $false
}

Write-LogMessage -message "Checking roles..."
if ((Get-WindowsRole -RoleName "DHCP") -eq $false) {
    Write-LogMessage -message "A required role is missing on this server. Make sure DHCP is installed."
    return $false
}

if ((Invoke-FogModuleAvailabilityPreparation) -eq $false) {
    Write-LogMessage -message "Could not prepare FOG API module. Exiting..."
    return $false
}

Write-LogMessage -message "Storing FOG API version..."
$fogVersion = (Get-Module -Name FogApi).Version.ToString()

Write-LogMessage -message "Checking share locations..."

Write-LogMessage -message "Starting FOG Helper Web API..."
Write-LogMessage -message "Server Options:"
Write-LogMessage -message " - Web API Host: $webHost"
Write-LogMessage -message " - Web API Port to listen to: $port"
Write-LogMessage -message "Creating firewall rules..."
$fwRule = $null
try {
    New-NetFirewallRule -DisplayName "Allow FOG listener on port $port" -Name "AllowListener" -Protocol TCP -LocalPort $port -Action Allow -ErrorAction Stop | Out-Null
    Write-LogMessage -message "Firewall rule creation succeeded. Continuing startup..."
} catch {
    Write-LogMessage -message "$_"
    Write-LogMessage -message "Could not add rule. Port $port may already be allowlisted. Check firewall settings before proceeding. The script, however, will continue"
}

if (-not (Test-Path "$([IO.Path]::GetDirectoryName((Get-Module -Name FogApi).Path))\lib\settings.json" -PathType Leaf)) {
    Write-Host "FOG API settings are not configured yet. Invoking configuration..."
    Set-FogServerSettings -interactive
}

$fwRule = Get-NetFirewallRule -Name "AllowListener"

$listener = [System.Net.HttpListener]::new()
$listener.Prefixes.Add("http://$($webHost):$port/api/")
$listener.Start()
Write-LogMessage -message "FOG REST API Listener running on http://$($webHost):$port/api/"
Write-LogMessage -message "To shut down, click the `"Stop Server`" button in the Control Panel, which you can access at any time while the server is running at http://localhost:$port/api/foghome."

class FOGShareAuthenticationInfo {
    [string]$server
    [string]$username
    [string]$mountPath

    FOGShareAuthenticationInfo() {
        $this.server = ""
        $this.username = ""
        $this.mountPath = ""
    }

    FOGShareAuthenticationInfo($srv, $usr, $mnt) {
        $this.server = $srv
        $this.username = $usr
        $this.mountPath = $mnt
    }
}

$shutdownRequested = $false
$shutdownEvent = New-Object System.Threading.ManualResetEvent $false

$ctrlC_EH = [ConsoleCancelEventHandler]{
    param($sender, $args)

    $shutdownRequested = $true
    throw
}

Start-Process "http://localhost:$port/api/foghome"

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
            "/api/foghome" {
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
                            <h1>FOG Helper Server Control Panel</h1>
                            <p>FOGH Server component version: $version; included with DISMTools $($version).</p>
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
                            <button onclick="invokeFogApiSetup()">Set up the FOG API</button>
                            <button onclick="invokeExit()">Stop the Server</button>
                            <button onclick="invokeLogViewer()">View Server Logs</button>
                            <button onclick="window.location.reload();">Refresh Page</button>
                            Other actions exposed by the API can only be called by clients.
                            <p align="right"><i>&copy; 2025-2026. <a href="https://github.com/CodingWonders" target="_blank">CodingWonders Software</a></i></p>
                        </div>

                        <script>
                            function invokeFogApiSetup() {
                                alert("You will be asked a series of questions to set up communication with the API and the server. Close this popup to continue...");
                                let fogServer = prompt("Enter the FOG server name or IP address. When entering an IP address, verify that it is either a static address in the DHCP server or a reservation.");
                                let fogApiToken = prompt("Enter the FOG API token:");
                                let fogUserToken = prompt("Enter the FOG User token:");

                                const response = fetch('/api/fogsetup', {
                                    method: "POST",
                                    body: JSON.stringify({
                                        server: fogServer,
                                        apiToken: fogApiToken,
                                        userToken: fogUserToken
                                    })
                                });

                                fogServer = "";
                                fogApiToken = "";
                                fogUserToken = "";
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
            "/api/getostype" {
                if ($request.HttpMethod -eq "GET") {
                    $osType = [Environment]::OSVersion.Platform
                    $sendJson.Invoke(@{ success = $true; platform = $osType })
                } else {
                    $sendJson.Invoke(@{ error = "Method not allowed" }, 405)
                }
            }
            "/api/fogsetup" {
                if (Test-Path "$([IO.Path]::GetDirectoryName((Get-Module -Name FogApi).Path))\lib\settings.json" -PathType Leaf) {
                    Write-Host "FOG API settings are already configured. If you continue, settings will be reset."
                    if ((Read-Host -Prompt "Do you want to reset these settings? (Y/N)") -eq "n") {
                        $sendJson.Invoke(@{ success = $true; reason = "The operation was cancelled." })
                        continue
                    }
                }

                if ($request.HttpMethod -eq "POST") {
                    $reader = New-Object IO.StreamReader $request.InputStream
                    $body = $reader.ReadToEnd() | ConvertFrom-Json
                    $server = $body.server
                    $apiToken = $body.apiToken
                    $userToken = $body.userToken

                    Set-FogServerSettings -fogapitoken "$apiToken" -fogusertoken "$userToken" -fogserver "$server" | Out-Null
                } else {
                    $sendJson.Invoke(@{ error = "Method not allowed" }, 405)
                }
            }
            "/api/hosts" {
                if ($request.HttpMethod -eq "GET") {
                    try {
                        $hosts = Get-FogObject -type object -coreObject host
                        $sendJson.Invoke(@{ success = $true; hosts = $hosts.data })
                    } catch {
                        Write-LogMessage -message "Exception caught: $_"
                        $sendJson.Invoke(@{ success = $false; error = $_.Exception.Message }, 500)
                    }
                } else {
                    $sendJson.Invoke(@{ error = "Method not allowed" }, 405)
                }
            }
            "/api/getImagesForHost" {
                if ($request.HttpMethod -eq "POST") {
                    try {
                        $reader = New-Object IO.StreamReader $request.InputStream
                        $body = $reader.ReadToEnd() | ConvertFrom-Json
                        $hostId = $body.hostId

                        $result = (Get-FogHost -hostID $hostId).image
                        if ($result -ne $null) {
                            $sendJson.Invoke(@{ success = $true; output = $result })
                        }
                    } catch {
                        Write-LogMessage -message "Exception caught: $_"
                        $sendJson.Invoke(@{ success = $false; error = $_.Exception.Message }, 500)
                    }
                } else {
                    $sendJson.Invoke(@{ error = "Method not allowed" }, 405)
                }
            }
            "/api/getAllImages" {
                if ($request.HttpMethod -eq "GET") {
                    try {
                        $images = (Get-FogObject -type object -coreObject image).data
                        $sendJson.Invoke(@{ success = $true; images = $images })
                    } catch {
                        Write-LogMessage -message "Exception caught: $_"
                        $sendJson.Invoke(@{ success = $false; error = $_.Exception.Message }, 500)
                    }
                } else {
                    $sendJson.Invoke(@{ error = "Method not allowed" }, 405)
                }
            }
            "/api/deploy" {
                if ($request.HttpMethod -eq "POST") {
                    try {
                        $reader = New-Object IO.StreamReader $request.InputStream
                        $body = $reader.ReadToEnd() | ConvertFrom-Json
                        $hostId = $body.hostId
                        $imageId = $body.imageId


                        $imageName = ((Get-FogObject -type object -coreObject image).data | Where-Object { $_.id -eq "$imageId " }).name

                        # For some reason, invoking this API causes an error condition at first. On a normal (non-scripted) PowerShell
                        # session, after throwing the error, it continues with this task. So we have to ignore this error
                        # and continue onwards.
                        #
                        # Some utterly broken nonsense that darksidemilk has to fix.
                        # $output = Send-FogImage -hostId "$hostId" -imageName "$imageName" 2>$null

                        & "$env:WINDIR\system32\WindowsPowerShell\v1.0\powershell.exe" `
                          -command "Import-Module FogApi ; Send-FogImage -hostId \""$hostId\"" -imageName \""$imageName\"" -ErrorAction SilentlyContinue"
                        $sendJson.Invoke(@{ success = $? })
                    } catch {
                        Write-LogMessage -message "Exception caught: $_"
                        $sendJson.Invoke(@{ success = $false; error = $_.Exception.Message }, 500)
                    }
                } else {
                    $sendJson.Invoke(@{ error = "Method not allowed" }, 405)
                }
            }
            "/api/setDhcp" {
                if ($request.HttpMethod -eq "POST") {
                    try {
                        $reader = New-Object IO.StreamReader $request.InputStream
                        $body = $reader.ReadToEnd() | ConvertFrom-Json
                        $fogIp = $body.fogIp

                        Set-DhcpServerv4OptionValue -OptionId "066" -Value "$fogIp"
                        Set-DhcpServerv4OptionValue -OptionId "067" -Value "ipxe.efi"

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
                notepad "$tempDir\DT_FOGHS_Log.log"
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
