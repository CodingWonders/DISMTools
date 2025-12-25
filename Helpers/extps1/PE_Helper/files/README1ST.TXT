DISMTools Preinstallation Environment (PE) Helper - Read Me First for Disc Image Files
--------------------------------------------------------------------------------------

This installation disc image file (ISO file) contains the necessary files to install
your custom Windows image using the DISMTools Preinstallation Environment Helper.

This disc image file can perform a clean installation of the operating system by
booting to its Preinstallation Environment, or by running "setup.exe" in the disc root.
For network-based deployments based on the Windows Deployment Services, perform the
following steps:

1. Copy the WIM files to your server. These are in the sources folder
2. In an administrative PowerShell session pointing to the disc image, run the
   following:

   PS D:\> .\pxeprep\prepare_wds.ps1 -bootImagePath "<path-to-boot-image-on-local-server>"

=== Notes regarding PXE/WDS preparation ===

- This will replace the PE Helper as the operating system installer and will reinstate Windows
  Setup
- The script can only be run on a device running Windows Server. It will immediately fail
  if run on a Windows client. The server must have the Windows ADK and the Preinstallation
  Environment addon that match the version of the boot image
- The server must contain the DHCP and Windows Deployment Services roles. For DHCP, the
  server must have at least one scope with defined ranges. Clients must be able to be given
  IP addresses within the scopes you defined and must be able to start boot programs via
  the network
- You have the possibility to add the prepared boot image to WDS automatically. Install
  images, however, must be added manually to image groups

=== Implementation notices for the WDS Preparation script ===

The WDS preparation script is still in development, and there are some things to consider
as a system administrator:

- The script will guess whether to grab settings from DHCPv4 or from DHCPv6. You can't force
  a specific version of DHCP
- The PXE/WDS server must also be the DHCP server. No testing has been made yet on servers
  that act as PXE/WDS servers but don't as DHCP servers
- WDS is the only supported option right now. Other solutions are not yet supported

For more info on WDS:
https://learn.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-r2-and-2012/jj648426(v=ws.11)

We are not responsible for the changes that you make to your computer's operating system
or its disk configuration. Be sure to follow every step in the installation process
very carefully. It is recommended to use virtual machine (VM) software, or spare
computers, to test your custom images. That way, you can perform testing without
modifying your computer.

Only after testing your Windows image and verifying that it works successfully, you
can proceed with a larger deployment of the individual image.

Thanks for understanding this.

                                                              - CodingWonders Software