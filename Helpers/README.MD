# Script disclaimer

The scripts present in this repository are provided **AS IS, WITHOUT ANY WARRANTY**. We AREN'T RESPONSIBLE for any damage done to your device.

Just like any script, you should read the source code of these files if you're a programmer who knows Batch and PowerShell. Otherwise, run these with caution, or ask a programmer friend to examine the source code and provide a more detailed description of what a script does for you.

## Content description

We can provide a short description of all scripts in this directory:

| Script | Description |
|:--:|:--:|
| `dthelper.bat` | The DISMTools Helper script, which helps the program get drive information and start the Command Console |
| `extbatch\cmdhelp.bat` | Command Help. This lets you learn about the syntax of DISM commands |
| `extbatch\options.bat` | (Functionality _to be added_) Options script that configures the Command Console |
| `extps1\extappx.ps1` | The Extended AppX package getter script, which is run when managing online installations |
| `extps1\mImgMgr.ps1` | The CLI version of the mounted image manager, which lets you manage your mounted Windows images |
| `extps1\PE_Helper\PE_Helper.ps1` | The DISMTools Preinstallation Environment (PE) Helper |

This list of scripts will be updated as we add new scripts in this directory.

## Can I run these?

Apart from the `cmdhelp.bat` script, which you can run from the Command Console, these scripts aren't meant to be run manually, and it's recommended to let DISMTools run those. However, you can still run them (**as an administrator**)

## Why do I need admin privileges?

You need administrative privileges because most of the scripts, apart from the aforementioned `cmdhelp.bat` script, will fail if they're run as a normal user. If the admin account in your system is password-protected, you should let the system administrator run these scripts instead of you.
