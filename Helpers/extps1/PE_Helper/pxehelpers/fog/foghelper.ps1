#requires -version 5.0
#requires -runasadministrator

# FOG Helper

. "$PSScriptRoot\..\Common\PXEHelpers.Common.ps1"

$global:product = "FOG Helper"
$global:description = "This script will guide you through the process of deploying an operating system via a FOG server."

if ((Get-ItemPropertyValue -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion" -Name "EditionID") -ne "WindowsPE") {
    Show-CenteredTextBox -Text "This script is intended to be run in Windows PE. Please restart your device and boot into Windows PE to continue. Press ENTER to exit . . ." -MaxWidth 100 -CenterOfAll -ForegroundColor DarkRed
    Read-Host | Out-Null
    exit 1
}

$host.UI.RawUI.WindowTitle = "Preboot eXecution Environment Helpers: $($global:product)"

Clear-Host

Write-Host "                                                                                                      "
Write-Host "                                                                                                      "
Write-Host "     OOOOOOOOO                                                                                        "
Write-Host "   OO:::::::::OO                                                                                      "
Write-Host " OO:::::::::::::OO                                                                                    "
Write-Host "O:::::::OOO:::::::O                                                                                   "
Write-Host "O::::::O   O::::::O   ooooooooooo   ppppp   ppppppppp       ssssssssss                                "
Write-Host "O:::::O     O:::::O oo:::::::::::oo p::::ppp:::::::::p    ss::::::::::s                               "
Write-Host "O:::::O     O:::::Oo:::::::::::::::op:::::::::::::::::p ss:::::::::::::s                              "
Write-Host "O:::::O     O:::::Oo:::::ooooo:::::opp::::::ppppp::::::ps::::::ssss:::::s                             "
Write-Host "O:::::O     O:::::Oo::::o     o::::o p:::::p     p:::::p s:::::s  ssssss                              "
Write-Host "O:::::O     O:::::Oo::::o     o::::o p:::::p     p:::::p   s::::::s                                   "
Write-Host "O:::::O     O:::::Oo::::o     o::::o p:::::p     p:::::p      s::::::s                                "
Write-Host "O::::::O   O::::::Oo::::o     o::::o p:::::p    p::::::pssssss   s:::::s                              "
Write-Host "O:::::::OOO:::::::Oo:::::ooooo:::::o p:::::ppppp:::::::ps:::::ssss::::::s                             "
Write-Host " OO:::::::::::::OO o:::::::::::::::o p::::::::::::::::p s::::::::::::::s       ......  ......  ...... "
Write-Host "   OO:::::::::OO    oo:::::::::::oo  p::::::::::::::pp   s:::::::::::ss        .::::.  .::::.  .::::. "
Write-Host "     OOOOOOOOO        ooooooooooo    p::::::pppppppp      sssssssssss          ......  ......  ...... "
Write-Host "                                     p:::::p                                                          "
Write-Host "                                     p:::::p                                                          "
Write-Host "                                    p:::::::p                                                         "
Write-Host "                                    p:::::::p                                                         "
Write-Host "                                    p:::::::p                                                         "
Write-Host "                                    ppppppppp                                                         "
Write-Host "                                                                                                      "
Write-Host ""


Show-CenteredTextBox -Text "Thank you for your enthusiasm but, unfortunately, you'll have to wait. Expect this to be available in DISMTools 0.7.1." -MaxWidth 100 -ForegroundColor DarkYellow
Read-Host | Out-Null
exit 1