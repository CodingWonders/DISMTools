# DISMTools
 
![Product image](https://user-images.githubusercontent.com/101426328/219872612-e7a8a169-b699-4df0-8656-3f5dc87f95ca.png)


DISMTools is a front-end for DISM that lets you manage your Windows Imaging (WIM) files and a whole lot more.

## Key features

- DISMTools is the first project-based GUI. Projects store the mounted image and unattended answer files you want to apply (using the command line at this time), while also providing a scratch directory for temporary operations
- DISMTools is also smart, checking first if an action can be performed before doing it

## Supported actions

The following actions are supported by DISMTools:

  > This program is **still in Alpha stages**, so not every possible action is implemented. Check the "Unsupported actions" for more details

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
- OS packages and features
  - Package addition and removal
  - Feature enablement and disablement
- AppX package servicing
  - Application addition and removal
- Other
  - Get complete information of an image
  
## Unsupported actions

- Getting any information from packages or features (on demand)
- Regional settings
- Applying unattended answer files
- Using the project's scratch directory
- and more, it's still in Alpha stages

These actions will be supported in future releases. They aren't implemented yet because it takes time to create working implementations that don't conflict with the rest of the program

## Building

If you want to grab a copy straight from the source code, follow these instructions:

1. You begin by either cloning the project or downloading a ZIP of the source code. Go to "Code", and select an option from there
2. Open the solution in Visual Studio 2012 or later

    > You will need to install the .NET Framework 4.8 Developer Pack
    
3. Finally, go to "Build > Build solution", or press CTRL-Shift-B

## Contributions

If you want to contribute to this project, you can do so in many ways:

- Code changes: changes that WILL make it to the next release. If you want to do these, do the following:

  1. Create your separate branch, based on the **latest** `dt_pre_****` one (you don't want to work on your changes based on an outdated source tree). This will make sure your change can arrive in the next preview
  2. Clone the newly created branch or, if you already cloned the repository, fetch the origin (`git fetch origin`) and switch to your branch
  3. Work on your changes **and test them**. We want to make sure your changes work as expected, and there aren't any [regressions](https://en.wikipedia.org/wiki/Regression_testing) because of them
  4. Commit your changes and create a pull request. If not set automatically, select the **latest** `dt_pre_****` branch to merge the contents to it. We'll review your changes and, if they're ready, we'll merge them.

- Documentation and/or artwork: if you like the visual side of things more, you can contribute to the Wiki. For more information, read [the following issue](https://github.com/CodingWonders/DISMTools/issues/2)

## Testing the latest

We continue the development of the next version in the Preview branch. To go to it, select "dt_preview" from the branch list. Commits are done every day, and new builds are released every Sunday.
