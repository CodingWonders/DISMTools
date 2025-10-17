using namespace System.Collections.Generic

$global:product = ""
$global:description = ""

enum IPAddress {
    IPv4 = 0
    IPv6 = 1
    Unknown = 2
}

function Show-CenteredTextBox {
    param (
        [string]$Text,
        [int]$MaxWidth = 40,
        [switch]$CenterOfAll = $false,
        [ConsoleColor]$ForegroundColor = "White"
    )

    # Get console width safely (fallback to $MaxWidth if it fails)
    try {
        $consoleWidth = $host.UI.RawUI.WindowSize.Width
    }
    catch {
        $consoleWidth = $MaxWidth
    }

    if ($CenterOfAll) {
        Clear-Host
        # We print as many newlines as possible to center the box vertically
        "`n" * (($host.UI.RawUI.WindowSize.Height / 2) - 4)
    }

    $boxWidth = [math]::Min($MaxWidth, $consoleWidth - 4)  # Keep box within screen width

    # Word-wrap the text
    $words = $Text -split ' '
    $wrappedLines = @()
    $currentLine = ""

    foreach ($word in $words) {
        if (($currentLine.Length + $word.Length + 1) -le $boxWidth) {
            if ($currentLine -eq "") {
                $currentLine += "$word"
            }
            else {
                $currentLine += " $word"
            }
        }
        else {
            $wrappedLines += $currentLine
            $currentLine = $word
        }
    }
    if ($currentLine -ne "") { $wrappedLines += $currentLine }

    # Find the longest line
    $contentWidth = ($wrappedLines | ForEach-Object { $_.Length } | Measure-Object -Maximum).Maximum
    $boxWidth = $contentWidth + 4  # Add padding for borders

    # Generate the box borders
    $border = "+" + ("-" * ($boxWidth - 2)) + "+"

    # Calculate left padding for centering
    $leftPadding = " " * [math]::Max(0, ($consoleWidth - $boxWidth) / 2)

    # Print the box
    Write-Host "$leftPadding$border" -ForegroundColor $ForegroundColor
    foreach ($line in $wrappedLines) {
        $spacePadding = " " * ($boxWidth - $line.Length - 4)
        Write-Host "$leftPadding| " -NoNewline -ForegroundColor $ForegroundColor
        Write-Host $line -NoNewline -ForegroundColor $ForegroundColor
        Write-Host " $spacePadding|" -ForegroundColor $ForegroundColor
    }
    Write-Host "$leftPadding$border" -ForegroundColor $ForegroundColor
}

function Show-SectionMessage {
    param (
        [string]$sectionTitle = "",
        [string]$sectionDescription = ""
    )

    Clear-Host
    Write-Host "`n $global:product`n$([String]::new("=", $global:product.Length))==`n"

    if ($sectionTitle -ne "") {
        Write-Host "  $sectionTitle" -ForegroundColor White
    }
    if ($sectionDescription -ne "") {
        Write-Host "`n  $sectionDescription" -ForegroundColor DarkGray
    }

    Write-Host "`n`n"
}

function Enable-Networking {
    if (-not (Test-Path "$env:SYSTEMDRIVE\net_initiated" -PathType Leaf)) {
        $successfulNetworkInitOperations = 0
        Write-Host "`n`nInitializing network..."
        wpeutil initializenetwork | Out-Null
        if ($?) {
            $successfulNetworkInitOperations++
        }
        wpeutil enablefirewall | Out-Null
        if ($?) {
            $successfulNetworkInitOperations++
        }

        # Prepare Flags
        if ($successfulNetworkInitOperations -eq 2) {
            New-Item -Path "$env:SYSTEMDRIVE\net_initiated" | Out-Null
        }
    } else {
        Write-Host "The network has already been initialized."
    }
}

function Disable-Networking {
    if (Test-Path "$env:SYSTEMDRIVE\net_initiated" -PathType Leaf) {
        Remove-Item -Path "$env:SYSTEMDRIVE\net_initiated" -Force
    }
}

function Test-IPAddressSyntax {
    param (
        [Parameter(Mandatory, Position = 0)] [string]$ipAddr
    )

    # For some stupid reason, the regex method that works in both a non-WinPE PWSH session and in a .NET application does
    # NOT work on WinPE. GPT suggested using the IPAddress.AddressFamily property, and it produces viable results under a debugger environment. So
    # we'll use these here.

    # We'll first check IPv4, then IPv6
    $parsed = $null
    if ([System.Net.IPAddress]::TryParse($ipAddr.Trim(), [ref]$parsed)) {
        if ($parsed.AddressFamily -eq [System.Net.Sockets.AddressFamily]::InterNetwork) {
            return [IPAddress]::IPv4
        } elseif ($parsed.AddressFamily -eq [System.Net.Sockets.AddressFamily]::InterNetworkV6) {
            return [IPAddress]::IPv6
        }
    }

    return [IPAddress]::Unknown
}
