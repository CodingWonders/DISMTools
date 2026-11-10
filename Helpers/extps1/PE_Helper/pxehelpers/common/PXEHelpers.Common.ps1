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

# Holds the layout and the last drawn values of the progress bar. It is $null whenever the bar is not on screen.
$global:setupProgressState = $null

function Write-SetupProgressText {
    <#
        .SYNOPSIS
            Writes text at a specific position of the console window without moving the rest of the screen
        .PARAMETER X
            The zero-based column, relative to the visible window
        .PARAMETER Y
            The zero-based row, relative to the visible window
        .PARAMETER Text
            The text to write
        .PARAMETER ForegroundColor
            The text color. If omitted, the color stored by Show-SetupProgressBar is used (white if there is none)
        .NOTES
            This is a helper of Show-SetupProgressBar. Because it positions the cursor first, only the cells that changed
            have to be redrawn, which avoids the flicker you would get by clearing the screen on every progress update
    #>
    param (
        [Parameter(Mandatory, Position = 0)] $X,
        [Parameter(Mandatory, Position = 1)] $Y,
        [Parameter(Position = 2)] $Text = "",
        $ForegroundColor = $null
    )

    if ($null -eq $ForegroundColor) {
        $ForegroundColor = "White"
        if ($null -ne $global:setupProgressState) { $ForegroundColor = $global:setupProgressState.ForegroundColor }
    }

    $rawUI = $host.UI.RawUI
    # Cursor coordinates are buffer-based, so we add the window origin in case the window is scrolled
    $rawUI.CursorPosition = New-Object System.Management.Automation.Host.Coordinates -ArgumentList ($X + $rawUI.WindowPosition.X), ($Y + $rawUI.WindowPosition.Y)
    Write-Host $Text -NoNewline -ForegroundColor $ForegroundColor
}

function Show-SetupProgressBar {
    <#
        .SYNOPSIS
            Draws (or updates) a progress bar inside a box
        .DESCRIPTION
            Draws a double-line box that contains the activity text, the percentage and a framed progress bar. Nothing
            else on the screen is cleared or touched, so titles, messages and any other text must be rendered by other
            functions. The box is centered horizontally and, like the box of Show-CenteredTextBox, is at most MaxWidth
            characters wide, shrinking to fit narrow consoles. It starts at the row where the cursor is when it is first
            drawn (it is moved up if it would not fit above the bottom of the window). The progress bar takes up most of the box width. The console colors are never
            changed, and no background colors are used.
            The first call draws the box, the activity text and the frame of the bar. Further calls only repaint the
            percentage and the bar, and only if the percentage changed. Call Hide-SetupProgressBar when you are done.
        .PARAMETER Percent
            The completion percentage, from 0 to 100. Values outside of that range are clamped
        .PARAMETER Activity
            The text shown inside the box, above the bar. It is drawn together with the box, not on later updates
        .PARAMETER MaxWidth
            The maximum width of the box. The default is 100, and the box is always at least 4 characters narrower than
            the console
        .PARAMETER ForegroundColor
            The color of everything this function draws. It is applied when the box is drawn, not on later updates
        .PARAMETER NoPercentage
            Do not show the percentage above the bar
        .PARAMETER Redraw
            Force the box to be drawn again (useful if something else wrote over it, or to change the activity text)
        .EXAMPLE
            Show-SetupProgressBar -Percent 0 -Activity "Setup is copying files..."
            Show-SetupProgressBar -Percent 42.5
            Hide-SetupProgressBar
    #>
    param (
        [Parameter(Mandatory, Position = 0)] $Percent,
        $Activity = "",
        $MaxWidth = 100,
        $ForegroundColor = "White",
        [switch]$NoPercentage,
        [switch]$Redraw
    )

    $rawUI = $host.UI.RawUI

    $pct = [Math]::Max(0, [Math]::Min(100, $Percent))
    $pctWhole = [Math]::Floor($pct)

    $state = $global:setupProgressState

    # Static part: the box, the activity text and the frame of the bar. It is only drawn on the first call or when a
    # redraw is requested
    if (($null -eq $state) -or $Redraw) {
        # The color is chosen when the box is drawn and stored in the state, so later updates keep using it
        $color = $ForegroundColor
        # Remember where the cursor was only once, so that Hide-SetupProgressBar can put it back
        $originalCursor = $rawUI.CursorPosition
        if ($null -ne $state) {
            $color = $state.ForegroundColor
            $originalCursor = $state.OriginalCursor
        }

        $width = $rawUI.WindowSize.Width
        $height = $rawUI.WindowSize.Height

        # Same sizing rule as Show-CenteredTextBox: up to MaxWidth characters, but never wider than the console minus 4.
        # The box is 7 rows tall. It starts at the cursor row (like Show-CenteredTextBox without CenterOfAll), but is moved
        # up if it would run past the bottom of the window. A redraw keeps the row of the first draw
        $boxWidth = [Math]::Min($MaxWidth, $width - 4)
        $boxLeft = [Math]::Floor(($width - $boxWidth) / 2)
        $boxTop = [Math]::Max(0, [Math]::Min($originalCursor.Y - $rawUI.WindowPosition.Y, $height - 7))
        if ($null -ne $state) { $boxTop = $state.Top }

        $state = @{
            Left = $boxLeft
            Top = $boxTop
            Width = $boxWidth
            InnerLeft = $boxLeft + 4
            InnerWidth = $boxWidth - 8
            ShowPercentage = (-not $NoPercentage)
            LastPercent = -1
            ForegroundColor = $color
            OriginalCursor = $originalCursor
        }
        $global:setupProgressState = $state

        try { [Console]::CursorVisible = $false } catch { }

        # Box-drawing characters are built from code points so the script does not depend on its file encoding
        $dh = [string][char]0x2550   # double horizontal
        $dv = [string][char]0x2551   # double vertical
        $dtl = [string][char]0x2554  # double top-left
        $dtr = [string][char]0x2557  # double top-right
        $dbl = [string][char]0x255A  # double bottom-left
        $dbr = [string][char]0x255D  # double bottom-right
        $sh = [string][char]0x2500   # single horizontal
        $sv = [string][char]0x2502   # single vertical
        $stl = [string][char]0x250C  # single top-left
        $str = [string][char]0x2510  # single top-right
        $sbl = [string][char]0x2514  # single bottom-left
        $sbr = [string][char]0x2518  # single bottom-right

        # Outer double-line box: top border, 5 interior rows, bottom border
        $outerLine = $dh * ($boxWidth - 2)
        Write-SetupProgressText $boxLeft $boxTop ($dtl + $outerLine + $dtr)
        for ($row = 1; $row -le 5; $row++) {
            # Writing the whole interior row also erases whatever was there before
            Write-SetupProgressText $boxLeft ($boxTop + $row) ($dv + (" " * ($boxWidth - 2)) + $dv)
        }
        Write-SetupProgressText $boxLeft ($boxTop + 6) ($dbl + $outerLine + $dbr)

        # Activity text, truncated if it does not fit in the box
        $activityText = "$Activity"
        if ($activityText.Length -gt ($boxWidth - 4)) { $activityText = $activityText.Substring(0, $boxWidth - 4) }
        Write-SetupProgressText ($boxLeft + 2) ($boxTop + 1) $activityText

        # Inner single-line frame of the progress bar
        $innerLine = $sh * ($state.InnerWidth - 2)
        Write-SetupProgressText $state.InnerLeft ($boxTop + 3) ($stl + $innerLine + $str)
        Write-SetupProgressText $state.InnerLeft ($boxTop + 4) $sv
        Write-SetupProgressText ($state.InnerLeft + $state.InnerWidth - 1) ($boxTop + 4) $sv
        Write-SetupProgressText $state.InnerLeft ($boxTop + 5) ($sbl + $innerLine + $sbr)
    }

    # Dynamic part. Skip all the drawing if nothing visible has changed since the last call
    if ($state.LastPercent -eq $pctWhole) {
        return
    }

    # Percentage, centered inside the box. Padding to the full interior width erases the previous value
    if ($state.ShowPercentage) {
        $interiorWidth = $state.Width - 2
        $percentText = "$pctWhole%"
        $percentLine = ((" " * [Math]::Floor(($interiorWidth - $percentText.Length) / 2)) + $percentText).PadRight($interiorWidth)
        Write-SetupProgressText ($state.Left + 1) ($state.Top + 2) $percentLine
    }

    # Filled section (full blocks) followed by the empty remainder (light shade). Only characters are used, no background
    # colors. The bar fills the inside of its frame
    $fillWidth = $state.InnerWidth - 2
    $filled = [Math]::Floor($fillWidth * $pct / 100)
    $barText = (([string][char]0x2588) * $filled) + (" " * ($fillWidth - $filled))
    Write-SetupProgressText ($state.InnerLeft + 1) ($state.Top + 4) $barText

    $state.LastPercent = $pctWhole
}

function Hide-SetupProgressBar {
    <#
        .SYNOPSIS
            Erases the progress bar box, puts the cursor back where it was and makes it visible again
        .NOTES
            Only the area of the box is erased. Does nothing if the bar is not on screen, so it is safe to call from a
            "finally" block
    #>
    $state = $global:setupProgressState
    if ($null -eq $state) {
        return
    }

    $blank = " " * $state.Width
    for ($row = 0; $row -lt 7; $row++) {
        Write-SetupProgressText $state.Left ($state.Top + $row) $blank
    }

    $host.UI.RawUI.CursorPosition = $state.OriginalCursor
    try { [Console]::CursorVisible = $true } catch { }

    $global:setupProgressState = $null
}