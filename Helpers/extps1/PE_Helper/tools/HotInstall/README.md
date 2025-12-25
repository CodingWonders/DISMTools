# HotInstall

**HotInstall** is an operating system installer that prepares your computer to boot to the DISMTools Preinstallation Environment from within full Windows environments.

## How does it work?

1. HotInstall copies all files of the disc image, minus the installation image (`install.wim`), to your local disk
2. HotInstall configures your computer's boot entries to allow Preinstallation Environments to boot to RAM (*if you want to know more about this task, check out [this guide](https://learn.microsoft.com/en-us/answers/questions/563319/setup-bcd-to-boot-pe-from-ram)*)
3. HotInstall customizes the boot image to give it the identifier of the new boot entry

> [!NOTE]
> This is required, if you format a partition instead of a disk, to delete the boot entry after the OS is installed

After all of this is done, you can continue installing the OS as you would with the PE Helper. Make sure the ISO file is always available to your computer.

## How do I use it?

HotInstall is included with the ISO files you create with DISMTools. To start the installer, simply double-click `setup.exe` in the root of the mounted ISO, or take advantage of AutoRun:

<p align="center">
  <img src="https://github.com/CodingWonders/DT-HotInstall/blob/main/res/HotInstall_AutoRun.png" />
</p>

Then, follow the steps of the wizard.

> [!NOTE]
> Make sure that you created your ISO file with the correct Windows image to test. You will be able to see some information about this image.
>
> If you have used the wrong installation image, you will have to recreate your ISO file if you want to see expected results.

### Notes for Ventoy drives

If the ISO file from which you started HotInstall is in a Ventoy drive, you will not be able to install the operating system. This is because of the way Ventoy works. The ISO file is never mounted or inserted to your computer -- only the Ventoy drive is. It will be able to prepare your computer for installation, but you will be stuck, as the first phase of Setup will fail at finding the installer scripts.

## System requirements

HotInstall requires [.NET Framework 4.8](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net48-offline-installer) to function. Modern Windows 10 versions and Windows 11 already have this installed, but you will have to install it if you want to run it on an older version of Windows. Based on this requirement, the following supported operating systems are:

- **Client:** Windows 8.1 and later, excluding Windows 10 versions 1507 and 1511
- **Server:** Windows Server 2012 and later, excluding Server Core editions

**Windows 7 and Windows Server 2008 R2 are not supported as they do not support the DISM API.**

## Contributing

### Reporting feedback

We appreciate your feedback. If you have any issues with HotInstall, or want to suggest a new feature, please report them in the Issues section.

### Contributing code

**Requirements:**

- Visual Studio 2012 or later
- [.NET Framework 4.8 Developer Pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net48-developer-pack-offline-installer)

To contribute to HotInstall:

1. Fork this repository
2. Clone your fork to your computer
3. Open the solution file in Visual Studio and make sure you have the required NuGet packages (run `nuget restore` if you don't)
4. Make your changes
5. Test your changes
6. Commit your changes and make a pull request

When testing your changes, copy a Windows PE image and an installation image to the `sources` folder of the output directory of the Installer project. Also, experiment with the command-line flags you can pass to the **Installer** project:

- If you pass `/test`, the installer will not create BCD entries, but will still copy files and prepare the boot image
- Otherwise, if you don't pass any flags or pass `/bcdtest`, the installer will create BCD entries

If you pass either `/test` or `/bcdtest`, HotInstall will copy files from the startup directory instead of the path root. Make sure to use either when not planning to include the installer in the ISO file.

#### Preparing HotInstall for ISO files

If you want to include HotInstall in the ISO file, run `CopyOutput.ps1` at the root of the repository. This will copy all files from the output, except for PDB files, Visual Studio host files and WIM files, to the `out` folder at the root of the repository. Then, zip the contents of the `out` folder to a file called `HotInstall.zip` with 7-Zip.

After the ZIP file has been created, copy it to the following location:

```
<Location where you cloned DISMTools>\Helpers\extps1\PE_Helper\files\HotInstall.zip
```

Then, create the ISO file with DISMTools as you would normally do.