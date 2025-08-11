#requires -version 5.0
#requires -runasadministrator
#                                              ....
#                                         .'^""""""^.
#      '^`'.                            '^"""""""^.
#     .^"""""`'                       .^"""""""^.                ---------------------------------------------------------
#      .^""""""`                      ^"""""""`                  | DISMTools 0.7.1                                       |
#       ."""""""^.                   `""""""""'           `,`    | The connected place for Windows system administration |
#         '`""""""`.                 """""""""^         `,,,"    ---------------------------------------------------------
#            '^"""""`.               ^""""""""""'.   .`,,,,,^    | PE Helper - FOG Helper Web-based API for Servers      |
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
#   - /api/installimages --> Gets the install images in the FOG store
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
#             image_name = "<File name of image in FOG>"
#             image_group = "<FOG image group>"
#         } | ConvertTo-Json
#
#         This must then be sent as part of the body. Then, mount a network share that will be created to the WinPE
#
#   - /api/clearfiles    --> Clears all the files created during deployment preparation
#   - /api/exit          --> Gracefully close the program
#
#   Settings for the server are declared in the Server Options section.


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


Write-Host "Thank you for your enthusiasm but, unfortunately, you'll have to wait. Expect this to be available in DISMTools 0.7.1." -ForegroundColor DarkYellow
Read-Host | Out-Null
exit 1