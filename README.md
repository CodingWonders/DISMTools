![Product image](https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/product_overview.png)

<!-- Tags (powered by Shields.io) -->

<p align="center">
	<img src="https://img.shields.io/github/downloads/CodingWonders/DISMTools/total" />
	<!-- For those who are new to GitHub and have lots to say - kudos to the SMELLY NERDS! (anyway, https://www.reddit.com/r/github/comments/1at9br4/i_am_new_to_github_and_i_have_lots_to_say/) -->
	<a href="https://github.com/CodingWonders/DISMTools/releases/latest"><img src="https://img.shields.io/github/v/release/CodingWonders/DISMTools?display_name=release&label=Latest%20Version" /></a>
	<a href="https://forums.mydigitallife.net/threads/dismtools.87263"><img src="https://img.shields.io/badge/MDL_Forums-blue" /></a>
	<a href="https://reddit.com/r/DISMTools"><img src="https://img.shields.io/badge/Subreddit-orange?logo=reddit&logoColor=white" /></a>
	<a href="https://matrix.to/#/#dismtools:gitter.im"><img src="https://img.shields.io/gitter/room/CodingWonders/DISMTools" /></a>
</p>
<hr>

DISMTools is a front-end for DISM that lets you manage your Windows Imaging (WIM) files and a whole lot more.

## About this project

This project was created simply because of the lack of free, open-source, and updated graphical interfaces for DISM. Before this tool was started, these popular UIs were available:

- [NTLite](https://www.ntlite.com/), which is the most popular offering. This software is free, but puts certain features (like downloading OS and program updates) behind a paywall, so it may not be for you if you are looking for a free, fully-featured program
- [MSMG Toolkit](https://forums.mydigitallife.net/threads/msmg-toolkit.50572/), which is one popular alternative. This is free and open-source (given that it is a script), but it may not be easily accessible for download, and may not be intuitive given its command-line user interface (TUI)
- [DISM GUI](https://github.com/mikecel79/DISMGUI), the first popular, .NET powered GUI that lets you perform a couple of actions with your Windows images. While it is open-source (with its source code available on GitHub), it has not been updated since 2017
- [DISM++](https://github.com/Chuyu-Team/Dism-Multi-language), another fully-featured, CBS-powered GUI. However, it has not been updated since 2023 and it is not open-source (even though there is a GitHub repository, it is only meant for the website and language translations)

There are also more GUIs for DISM, but they are way less known, so they are not mentioned here. Given this situation, this project was created to be a viable alternative to NTLite that was free, open-source, easily accessible, and constantly updated.

## Key features

### Working with projects

Inspired by Visual Studio, DISMTools is the first project-based GUI. Projects store the mounted image, unattended answer files you want to apply, and a scratch directory for temporary operations.

DISMTools projects are also tiny when you create them, and contain a structure that is easy to navigate.

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/project_sizes.png" />
</p>

The program also supports setting and removing file associations for projects with the click of a button (only on portable installations), so you can load your projects instantly by double-clicking them. You can also **copy your installed deployment tools** to your projects, allowing you to use those anywhere you take them.

### Manage your active installation, or installations on any drive

With the **online** and **offline installation management modes**, you can easily manage any installation of a modern Windows version.

### Compatibility and performance go hand in hand

Unlike other user interfaces for DISM that use either the DISM API or the DISM executable, DISMTools uses both, providing great performance to get the information you want from your images and installations quickly, and compatibility, allowing you to use any version of the DISM program, ranging from the Windows 7 version all the way up to the latest versions in Windows 10 and 11, so that your existing command-line workflows are not affected when you move to the graphical interface.

### An advanced front-end

DISMTools isn't just a front-end for DISM, but an advanced one. As you perform tasks with your images and installations, you're presented with rich information and functionality. Here are some examples:

- **Rich information during AppX package addition.** When adding an AppX package, you'll see rich information and, in most cases, the main Store logo asset. You can also add encrypted application packages to active installations, and configure stub package preferences, for full AppX package management:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/rich_appx_info.gif" />
</p>

- **Download content from App Installer packages automatically.** Have an App Installer package? No problem. Add it, and DISMTools will download the main package and use it automatically:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/app_installer_downloader.png" />
</p>

- **Automatic detection of sources from Group Policy.** If you want to enable a feature, repair the component store of a Windows image, or add a capability, with a source defined in the Group Policy; you can easily use it:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/group_policy_caps.gif" />
</p>

- **Easily create configuration lists.** With the *DISM Configuration List Editor* you can quickly create your configuration list to exclude certain items during operations like capturing an image:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/config_list_editor.png" />
</p>

- **Quickly manage all your mounted images in one interface.** The mounted image manager lets you perform basic image management tasks with your mounted images:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/mounted_image_manager.png" />
</p>

You can also get and save image file information using the manager:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/img_info_quick_action.png" />
</p>

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/img_info_saver_quick_action.png" />
</p>

- **Generate and print image information easily.** With image information reports, you can save the information of one area or all areas of the Windows image you're servicing for future reference as a Markdown file. You can also see your Markdown report in HTML form thanks to Markdig:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/info_report_viewer.png" />
</p>

- **Generate unattended answer files with ease.** Using the unattended answer file creation wizard, powered by the answer file generator from Christoph Schneegans, you can create your files by simply following the pages:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/autounattend_wizard.png" />
</p>

- **Quickly test your Windows images.** With the ISO creation wizard, you can test your Windows image quickly and easily. You can also use your unattended answer files to test them, or to speed up the installation process by removing tedious steps from the out-of-box experience:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/iso_creator.png" />
</p>
<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/assets/101426328/af2a2655-02de-4618-9423-30882303b424" />
</p>
<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/blob/dt_preview/res/iso_creator_results.png" />
</p>

> [!NOTE]
> The Windows ADK and its Windows PE plugin are required for this feature to work

- **View who signed an installed driver.** When getting installed driver information, you can view the signer information of a driver if it is signed:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/assets/101426328/9c84b0c3-6571-4b28-8ee4-44c4e6511e89" />
</p>

## Supported actions

The following actions are supported by DISMTools:

  > This program is **in beta stages**, so not every possible action is implemented. Check the "Unsupported actions" section for more details

- Image management
  - WIM/SWM/ESD file application
  - Image capture
  - Image commits
  - Volume image removal (removal of unnecessary Windows editions)
  - Image mounting and unmounting
  - Image servicing session reloads
  - Image index switches
  - WIM -> ESD and viceversa conversion
  - SWM file merger
  - Component cleanup
  - Image splitting
  - Appending changes to Windows images
  - Exporting Windows images to new image files
- OS packages and features
  - Package addition and removal
  - Feature enablement and disablement
- AppX package servicing
  - Application addition and removal
- Capabilities
  - Capability addition and removal
- Drivers
  - Driver addition and removal
- Provisioning packages
  - Add provisioning packages to an image
- Languages and regional servicing
  - Setting keyboard layered drivers
- Unattended answer files
  - Creating and managing unattended answer files
  - Applying unattended answer files
- Other
  - Get complete information of an image
  - Using the project's or program's scratch directory
  - Get information of packages, features, AppX packages, capabilities, and drivers
  - Configure Windows PE settings
  - Basic automation
  
## Unsupported actions

- Regional settings
- and more, it's in beta stages

These actions will be supported in future releases. They aren't implemented yet because it takes time to create working implementations that don't conflict with the rest of the program

## System requirements

DISMTools is compatible with the following operating systems:

- **Client:** Windows 8.1 and later (excluding Windows 10 versions 1507 and 1511)
- **Server:** Windows Server 2012 and later (excluding Server Core variants)

> [!NOTE]
> DISMTools is not compatible with Windows 7/Server 2008 R2 (versions 0.2.1 onwards), [Wine](https://www.winehq.org/), or [ReactOS](https://github.com/reactos/reactos)

## Downloading

You can download DISMTools from the [Releases](https://github.com/CodingWonders/DISMTools/releases) section (recommended), from [Softpedia](https://www.softpedia.com/get/Tweak/System-Tweak/DISMTools.shtml), or from WinGet:

- Stable version: `winget install CodingWondersSoftware.DISMTools.Stable` (moniker: `DISMTools`)
- Preview version: `winget install CodingWondersSoftware.DISMTools.Preview` (moniker: `DISMTools-pre`)

This program is also 100% Free.

<p align="center">
	<img src="https://www.softpedia.com/_img/softpedia_100_free.png" />
	<p align="center"><i>Last updated: August 3, 2024 (Roberto Zamfir)</i></p>
</p>

The [SourceForge project](https://sourceforge.net/projects/dismtools/) also keeps track of new releases in this repository, so you can download the latest releases from there as well.

## Notable mentions

DISMTools has been featured in news sites. Check them out if you're interested:

- [DeskModder](https://www.deskmodder.de/blog/2024/06/24/dismtools-iso-oder-image-bearbeiten-in-neuer-stable-version-erschienen/)
- [Computer BILD](https://www.computerbild.de/artikel/cb-Tipps-Windows-Windows-Media-Player-deinstallieren-31424181.html)
- [PC World](https://www.pcworld.com/article/2430467/operating-command-line-tools-with-the-mouse-the-best-guis.html)

## Support this project

If you find this project useful, consider giving it a star to encourage further development.

## Building

If you want to grab a copy straight from the source code, follow these instructions:

- **Requirements**:
  - [7-Zip](https://7-zip.org)
  - [.NET Framework 4.8 Developer Pack](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net48-offline-installer)
  - PowerShell 5 (part of [Windows Management Framework 5](https://www.microsoft.com/en-us/download/confirmation.aspx?id=54616)), or [newer](https://github.com/powershell/powershell), for script debugging

1. Begin by either cloning the project or downloading a ZIP of the source code. Go to "Code", and select an option from there
2. Prepare the NuGet packages by running `nugetpkgprep.bat` in the location you cloned the repository to
3. Open the solution in Visual Studio 2012 or later
4. Finally, go to "Build > Build solution", or press CTRL-Shift-B

### Additional startup flags

To speed up testing, you can perform these steps before running the program from within Visual Studio:

1. In the Solution Explorer, double-click `My Project`
2. Go to the Debug tab
3. Under the Startup options, type the following in the command line arguments text box: `/nomig /noupd`

- `/nomig` skips setting migration
- `/noupd` disables update check functionality

You should have this setting configured like this:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/assets/101426328/4c9c9384-71bd-45fc-9e17-460f107a50f9" />
</p>

### JetBrains users

If you use an IDE from JetBrains, you can also work on DISMTools. However, you can only modify the source code of forms, so there will not be any designer.

## Contributions

If you want to contribute to this project, you can do so in many ways:

- Code changes: changes that WILL make it to the next release. If you want to do these, please read [the contribution guidelines](https://github.com/CodingWonders/DISMTools/blob/stable/CONTRIBUTING.md)
- Documentation and/or artwork: if you like the visual side of things more, we recommend contributing to the help system! Check out the last section for instructions.

## Testing the latest

We continue the development of the next version in the Preview branch. To go to it, select "dt_preview" from the branch list. Commits are done every day, and new builds are released every 2 weeks.

However, if you want to download the latest release AS SOON AS the project is built with new changes, you can download the [nightly installer](https://github.com/CodingWonders/DISMTools/raw/dt_pre_24102/Installer/Nightly/dt_setup.exe).

<!-- **NOTE:** this branch contains release candidate builds of DISMTools 0.5.1, and will be deleted once this version gets published as a stable release -->

## Stay in touch

Be sure to [follow our official subreddit](https://reddit.com/r/DISMTools) for release announcements and other cool stuff. Also, check out the [My Digital Life discussion](https://forums.mydigitallife.net/threads/discussion-dismtools.87263/) to know about features being worked on.

## Contribute to the help system

We want your help to build a great help system for DISMTools. If you want to contribute to it, you can read more [here](https://github.com/CodingWonders/dt_help).
