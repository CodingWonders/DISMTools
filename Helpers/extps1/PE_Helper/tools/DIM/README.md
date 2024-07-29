# DISMTools Driver Installation Module (DIM)

The Driver Installation Module (DIM) lets you add drivers to an active Windows Preinstallation Environment (PE) image to give it additional hardware compatibility. The drivers that you add with this program will then be added to the target Windows image by the operating system installer of the Preinstallation Environment (PE) Helper.

## Features

The Driver Installation Module is **fast**, **light-weight** and **compatible**.

- It is fast because it makes direct calls to the Win32 APIs, without the use of the Microsoft Foundation Classes (MFC), the Active Template Library (ATL), or the Common Language Runtime (CLR)
- It is light-weight, being ~2 MB in size. With this, you can easily put it in a 2.88 MB floppy disk if you want to, and [if you have one](https://dfarq.homeip.net/what-happened-to-2-88-mb-floppies/)
- It is compatible with full Windows environments and Preinstallation Environments (most functionality)

> [!NOTE]
> If the Driver Installation Module is run on a full Windows environment, you will not be able to install drivers. Preinstallation Environments require 2 modifications for full functionality

You can use the pre-built versions, or you can build it yourself.

## Building

To build this program, you need Visual Studio 2012. Building the DIM may also work in newer versions of Visual Studio, but you may need to change the build toolkit used in the project properties. The solution also provides architecture-specific versions of the program for x86 and amd64, which you can build by doing a Batch Build:

1. Right-click the project
2. Click Build and wait for a successful compilation

The project will perform post-build events to compile a 64-bit version.

## Manual Setup

To add the Driver Installation Module (DIM) to your Preinstallation Environments, follow these steps:

**Requirements:** [Windows Assessment and Deployment Kit](https://learn.microsoft.com/en-us/windows-hardware/get-started/adk-install) and the Preinstallation Environment add-on

> [!NOTE]
> If, during this process, you specify a path that contains spaces, make sure to surround it with quotation marks (`"`)

### Creating your Preinstallation Environment

You need to follow these steps if you haven't created a Preinstallation Environment yet:

1. After the installation of the requirements, open the Deployment Tools Command Prompt **as an administrator** and accept any UAC pop-ups
2. Copy the files of the Preinstallation Environment to any location by typing `copype <architecture> <location>` (where `<architecture>` is the architecture of the Windows PE and `<location>` is the target location to which you want to copy the PE files)
3. Mount the Windows PE image by typing `dism /mount-image /imagefile="<Windows PE target location>\media\sources\boot.wim" /index=1 /mountdir="<Windows PE target location>\mount"`

### Adding the DIM

1. Copy the Driver Installation Module executable **designed for your image architecture** to any location in the image
2. Copy the required DLL files from your computer to the Windows image:

    | DLL file | Source location | Target location | Notes |
    |:--:|:--:|:--:|:--:|
    | `ExplorerFrame.dll` | `<Boot drive>\Windows\system32` | `<Image Root>\Windows\system32` | - |
    | `ExplorerFrame.dll` | `<Boot drive>\Windows\SysWOW64` | `<Image Root>\Windows\SysWOW64` | Only copy this file if you are working with a 64-bit image |

3. Load the `SOFTWARE` registry hive of the Windows PE image to your computer:

    1. Open the Registry Editor (`regedit`) and select HKEY_LOCAL_MACHINE
    2. Go to "File > Load hive..." and select the `SOFTWARE` hive in `<Image Root>\Windows\system32\config`
    3. Give the loaded hive the name of `WINPESOFT`

4. Merge the registry file `listview.reg` stored in the files for the DIM project and accept any questions
5. Unload the `WINPESOFT` hive from HKEY_LOCAL_MACHINE:

    1. Select the WINPESOFT hive
    2. Go to "File > Unload hive..." and click Yes

6. Save the changes to your Windows image by typing `dism /commit-image /mountdir="<Image Root>"`

### Creating the ISO file

Back in the Deployment Tools Command Prompt, type `makewinpemedia /ISO <Windows PE target location> <Path to ISO file>` (where `<Windows PE target location>` is the folder to which you've copied the PE files and `<Path to ISO file>` is the path of the target ISO file). Finally, you can test your new Preinstallation Environment in a virtual machine or on any computer.