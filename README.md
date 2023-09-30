<p align="center">
  <img src="https://github.com/CodingWonders/DISMTools/assets/101426328/4c753c9a-1440-44cb-a742-04e71d077dff">
  <h3 align="center">A free and open-source GUI for DISM operations</h3>
</p>
<hr>

![Product image](https://user-images.githubusercontent.com/101426328/233696501-a27a1e20-f489-4010-8f3c-f060fb808fd2.png)

DISMTools is a front-end for DISM that lets you manage your Windows Imaging (WIM) files and a whole lot more.

## Key features

- DISMTools is the first project-based GUI. Projects store the mounted image and unattended answer files you want to apply (using the command line at this time), while also providing a scratch directory for temporary operations
- DISMTools is also smart, checking first if an action can be performed before doing it

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

**NOTE:** in the event that the program detects an update and lets you download an older version, we suggest that you should disregard its advice (it's not like it will affect the binary being compiled, as it will be recompiled when you build the project, but it will make you waste time)

## Contributions

If you want to contribute to this project, you can do so in many ways:

- Code changes: changes that WILL make it to the next release. If you want to do these, do the following:

  1. Create your separate branch, based on the **latest** `dt_pre_****` one (you don't want to work on your changes based on an outdated source tree). This will make sure your change can arrive in the next preview
  2. Clone the newly created branch or, if you already cloned the repository, fetch the origin (`git fetch origin`) and switch to your branch
  3. Work on your changes **and test them**. We want to make sure your changes work as expected, and there aren't any [regressions](https://en.wikipedia.org/wiki/Regression_testing) because of them
  4. Commit your changes and create a pull request. If not set automatically, select the **latest** `dt_pre_****` branch to merge the contents to it. We'll review your changes and, if they're ready, we'll merge them.

- Documentation and/or artwork: if you like the visual side of things more, ~~you can contribute to the Wiki. For more information, read [the following issue](https://github.com/CodingWonders/DISMTools/issues/2)~~ A better help system will be introduced

## Testing the latest

We continue the development of the next version in the Preview branch. To go to it, select "dt_preview" from the branch list. Commits are done every day, and new builds are released every Sunday.

However, if you want to download the latest release AS SOON AS the project is built with new changes, you can download the [nightly installer](https://github.com/CodingWonders/DISMTools/raw/dt_pre_23101/Installer/Output/dt_setup.exe).

## Stay in touch

Be sure to [follow our official subreddit](https://reddit.com/r/DISMTools) for release announcements and other cool stuff. Also, check out the [My Digital Life discussion](https://forums.mydigitallife.net/threads/discussion-dismtools.87263/) to know about features being worked on.
