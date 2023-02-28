# DISMTools
 
![Product image](https://user-images.githubusercontent.com/101426328/219872612-e7a8a169-b699-4df0-8656-3f5dc87f95ca.png)

DISMTools is a front-end for DISM which lets you manage your Windows Imaging (WIM) files and a whole lot more.

## Key features

- DISMTools is the first project-based GUI. Projects store the mounted image, unattended answer files you want to apply (using the command line at this time), and provide a scratch directory for temporary operations (also for the command line)
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

- Getting any kind of information from packages or features (on demand)
- Driver management
- Capability (Features on Demand) management
- Regional settings
- Applying unattended answer files
- Using the project's scratch directory
- and more, it's still in Alpha stages

These actions will be supported on future releases. They aren't implemented yet because it takes time to create working implementations that don't conflict with the rest of the program

## Building

If you want to grab a copy straight from the source code, follow these instructions:

1. You begin by either cloning the project or downloading a ZIP of the source code. Go to "Code", and select an option from there
2. Open the solution in Visual Studio 2012 or later

    > You will need to install the .NET Framework 4.8 Developer Pack
    
3. Finally, go to "Build > Build solution", or press CTRL-Shift-B

## Testing the latest

We continue development of the next version in the Preview branch. To go to it, select "dt_preview" from the branch list. Commits are done every day, and new builds are released every Sunday.
