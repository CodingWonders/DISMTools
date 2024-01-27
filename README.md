<!--
<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/assets/101426328/4c753c9a-1440-44cb-a742-04e71d077dff">
</p>
-->

![Product image](https://github.com/CodingWonders/DISMTools/assets/101426328/7f55a099-7e51-498e-9a31-c433fab7d8b9)

<!-- Tags (powered by Shields.io) -->

<p align="center">
	<img src="https://img.shields.io/github/downloads/CodingWonders/DISMTools/total" />
	<a href="https://github.com/CodingWonders/DISMTools/releases/latest"><img src="https://img.shields.io/github/v/release/CodingWonders/DISMTools" /></a>
	<a href="https://forums.mydigitallife.net/threads/dismtools.87263"><img src="https://img.shields.io/badge/MDL_Forums-blue" /></a>
	<a href="https://reddit.com/r/DISMTools"><img src="https://img.shields.io/badge/Subreddit-orange?logo=reddit&logoColor=white" /></a>
	<a href="https://matrix.to/#/#dismtools:gitter.im"><img src="https://img.shields.io/gitter/room/CodingWonders/DISMTools" /></a>
</p>
<hr>

DISMTools is a front-end for DISM that lets you manage your Windows Imaging (WIM) files and a whole lot more.

## Key features

### Working with projects

DISMTools is the first project-based GUI. Projects store the mounted image and unattended answer files you want to apply (using the command line at this time), while also providing a scratch directory for temporary operations.

### Manage your active installation, or installations on any drive

With the **online** and **offline installation management modes**, you can easily manage any installation of a modern Windows version.

### An advanced front-end

DISMTools isn't just a front-end for DISM, but an advanced one. As you perform tasks with your images and installations, you're presented with rich information and functionality. Here are some examples:

- **Rich information during AppX package addition.** When adding an AppX package, you'll see rich information and, in most cases, the main Store logo asset:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/assets/101426328/a45c5b56-e5a6-40c5-a2f3-37677ce80405" />
</p>

- **Download content from App Installer packages automatically.** Have an App Installer package? No problem. Add it, and DISMTools will download the main package and use it automatically:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/assets/101426328/ad39f3ba-b29d-4874-a219-f2595bf8073a" />
</p>

- **Automatic detection of sources from Group Policy.** If you want to enable a feature, repair the component store of a Windows image, or add a capability, with a source defined in the Group Policy; you can easily use it:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/assets/101426328/8b2cbb41-dc82-4841-9cda-307a40619d64" />
</p>

<p align="center">
	<img src="https://user-images.githubusercontent.com/101426328/230734474-358bbac8-2c2e-4a70-b382-9cc3283c0db8.gif" />
</p>

- **Easily create configuration lists.** With the *DISM Configuration List Editor* you can quickly create your configuration list to exclude certain items during operations like capturing an image:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/assets/101426328/5b472424-4595-4082-9574-c147babba64b" />
</p>

- **Quickly manage all your mounted images in one interface.** The mounted image manager lets you perform basic image management tasks with your mounted images:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/assets/101426328/a2e35a8b-7f7e-4b52-9366-c71de33ef9e2" />
</p>

- **Generate and print image information easily.** With image information reports, you can save the information of one area or all areas of the Windows image you're servicing for future reference:

<p align="center">
	<img src="https://github.com/CodingWonders/DISMTools/assets/101426328/d668f94e-0d39-49bb-b98f-ec045fed725b" />
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
- Other
  - Get complete information of an image
  - Using the project's or program's scratch directory
  - Get information of packages, features, AppX packages, capabilities, and drivers
  - Configure Windows PE settings
  - Basic automation
  
## Unsupported actions

- Regional settings
- Applying unattended answer files
- and more, it's in beta stages

These actions will be supported in future releases. They aren't implemented yet because it takes time to create working implementations that don't conflict with the rest of the program

## System requirements

DISMTools is compatible with the following operating systems:

- **Client:** Windows 8.1 and later (excluding Windows 10 versions 1507 and 1511)
- **Server:** Windows Server 2012 and later (excluding Server Core variants)

> [!NOTE]
> DISMTools is not compatible with Windows 7/Server 2008 R2 (versions 0.2.1 onwards), [Wine](https://www.winehq.org/), or [ReactOS](https://github.com/reactos/reactos)

## Downloading

You can download DISMTools from the [Releases](https://github.com/CodingWonders/DISMTools/releases) section (recommended), from [Softpedia](https://www.softpedia.com/get/Tweak/System-Tweak/DISMTools.shtml), or from WinGet (`winget install CodingWondersSoftware.DISMTools.Stable`). This program is also 100% Free.

<p align="center">
	<img src="https://www.softpedia.com/_img/softpedia_100_free.png" />
	<p align="center"><i>Last updated: December 2, 2023 (Alexandra Sava)</i></p>
</p>

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

## Contributions

If you want to contribute to this project, you can do so in many ways:

- Code changes: changes that WILL make it to the next release. If you want to do these, do the following:

  1. Create your separate branch, based on the **latest** `dt_pre_****` one (you don't want to work on your changes based on an outdated source tree). This will make sure your change can arrive in the next preview
  2. Clone the newly created branch or, if you already cloned the repository, fetch the origin (`git fetch origin`) and switch to your branch
  3. Work on your changes **and test them**. We want to make sure your changes work as expected, and there aren't any [regressions](https://en.wikipedia.org/wiki/Regression_testing) because of them
  4. Commit your changes and create a pull request. If not set automatically, select the **latest** `dt_pre_****` branch to merge the contents to it. We'll review your changes and, if they're ready, we'll merge them.

- Documentation and/or artwork: if you like the visual side of things more, we recommend contributing to the help system! Check out the last section for instructions.

## Testing the latest

We continue the development of the next version in the Preview branch. To go to it, select "dt_preview" from the branch list. Commits are done every day, and new builds are released every Sunday.

However, if you want to download the latest release AS SOON AS the project is built with new changes, you can download the [nightly installer](https://github.com/CodingWonders/DISMTools/raw/dt_pre_2413_relcndid/Installer/Output/dt_setup.exe).

**NOTE:** this branch contains release candidate builds of DISMTools 0.4.1, and will be deleted once this version gets published as a stable release

## Stay in touch

Be sure to [follow our official subreddit](https://reddit.com/r/DISMTools) for release announcements and other cool stuff. Also, check out the [My Digital Life discussion](https://forums.mydigitallife.net/threads/discussion-dismtools.87263/) to know about features being worked on.

## Contribute to the help system

We want your help to build a great help system for DISMTools. If you want to contribute to it, you can read more [here](https://github.com/CodingWonders/dt_help).
