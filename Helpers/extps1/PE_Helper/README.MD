<p align="center">
  <img src="./logo.png" />
</p>

The Preinstallation Environment Helper is a set of scripts and applications designed to help during test deployments of your Windows images. It's used to create the ISO file and install your Windows image, whether it's a local deployment or a network-based one.

To learn more about the PE Helper in detail, refer to the DISMTools Help documentation.

## Folder Contents

| Script | Description |
|:--|:--:|
| `PE_Helper.ps1` | The main event. The script generates the ISO file and installs the operating system |
| `backgrounds` | The folder containing the background images used in the DISMTools Preinstallation Environment |
| `files\dimstart.bat` | A script that manually starts the Driver Installation Module |
| `files\startup\StartInstall.ps1` | A script that manually starts operating system installation from within the Extensibility Suite |
| `files\startup\startnet.cmd` | **Required for Windows PE**. A script that starts the preinstallation environment |
| `files\DISMTools-PE.zip` | A ZIP file containing Visual Studio solution files for the Extensibility Suite |
| `files\HotInstall.zip` | DISMTools HotInstall, which lets you install the operating system from a full Windows installation |
| `pxehelpers` | The DISMTools PXE Helpers, currently for Windows Deployment Services (WDS) |
| `pxehelpers\wds` | The DISMTools PXE Helpers for Windows Deployment Services (WDS) |
| `tools\DIM` | The folder containing the source code for the Driver Installation Module |

## Script disclaimer

The scripts present in this repository are provided **AS IS, WITHOUT ANY WARRANTY**. We AREN'T RESPONSIBLE for any damage done to your device.

Just like any script, you should read the source code of these files if you're a programmer who knows Batch and PowerShell. Otherwise, run these with caution, or ask a programmer friend to examine the source code and provide a more detailed description of what a script does for you.

## Credits/Copyright information

- **Preinstallation Environment Helper, Extensibility Suite, Driver Installation Module:** (c) 2024-2026 CodingWonders Software
- **HotInstall:** (c) 2025-2026 CodingWonders Software
- **PXE Helpers:** (c) 2025-2026 CodingWonders Software
- **Compilation Scripts:** (c) 2022 CT Tech Group LLC. Licensed under MIT