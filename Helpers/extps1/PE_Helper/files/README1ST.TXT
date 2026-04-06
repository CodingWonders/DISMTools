DISMTools Preinstallation Environment (PE) Helper - Read Me First for Disc Image Files
--------------------------------------------------------------------------------------

---- Applicable DISMTools versions: 0.7.1, 0.7.2, 0.7.3, 0.8+
---- Last updated: 6 April 2026

This installation disc image file (ISO file) contains the necessary files to install
your custom Windows image using the DISMTools Preinstallation Environment Helper.

=== Usage of this disc image file ===

This disc image file can perform a clean installation of the operating system by either
booting to its Preinstallation Environment (from your machine's BIOS/UEFI boot menu, or from
advanced startup options within Windows), or by running "setup.exe" in the disc root to
install the Preinstallation Environment image to your machine (HotInstall). This disc image
file can also prepare a computer for image capture with the System Preparation tool (Sysprep)
if it has booted to audit mode. Network-based installations can be started from this disc image
file too using the PXE Helpers.

=== Preinstallation Environment Helper Notes ===

1. About Third-Party Drivers

By booting to the DISMTools Preinstallation Environment, you are performing a cold-boot
installation. In this state, the Preinstallation Environment will not have third-party drivers
for SCSI adapters and storage controllers pre-installed. In the event that you cannot proceed
with installation because of missing disks in the disk listings, you must add these drivers
manually using the Driver Installation Module (DIM).

To obtain these drivers, head over to the manufacturer of the system (in the case of pre-built
machines), the manufacturer of your motherboard (in the case of a custom-built PC), or the
vendor of the SCSI adapter or storage controller; provided you know its hardware ID.

In many cases, the reason why disks are missing from the disk listings is due to computers
having certain storage controller features enabled. Head over to your computer's firmware and
look for features such as Rapid Storage Technology (RST). Disable them if possible and try
again.

On server equipment, the reason why disks are missing from the disk listings is due to the use
of third-party RAID controllers. If you are using the Preinstallation Environment Helper on a
server, you will need to use the Driver Installation Module to add your server's RAID
controller. In such cases, it's best that you search for controllers using hardware IDs. For
instance, hardware ID:

    PCI\VEN_103C&DEV_193F&SUBSYS_00E41590

...corresponds to a Microsemi RAID controller for a Hewlett-Packard Enterprise ProLiant ML30 
Gen10 server. That is the device that you will need to add.

When drivers are added using the Driver Installation Module, they are also scheduled for
installation on your target Windows image. In this case, any storage controller drivers added
to the environment will also be added to the target image. Otherwise, the target image will
fail to boot.

You can use the Driver Installation Module to install various other drivers, not just storage
controllers. Those will also be scheduled for installation on the target Windows image. However,
it is best to pre-install those drivers using the driver addition tasks from within DISMTools to
your target image if they are not "important". For instance, you may add an audio driver, but it
will not make any sense to add it to the Preinstallation Environment because it is not going to
play sounds. Conversely, adding storage media support is considered an important action because
not only will it benefit target images, but it will also benefit the Preinstallation Environment.
Consider what is important to add and what is not before installing drivers using the Driver
Installation Module.

At any time, to view hardware IDs of missing devices, run the following command from a command
prompt window:

    pnputil /enum-devices /problem

2. Windows UEFI CA 2023 support

On Unified Extensible Firmware Interface (UEFI) systems with Secure Boot enabled, you may see
a prompt asking you for the version of boot binaries to use. This is in response to the imminent
expiration of Microsoft Windows Production PCA 2011 boot certificates, which will happen in June
2026. After expiration, devices that have not yet received Secure Boot updates will start to
become less secure.

The solution for the expired certificates comes from updated boot certificates, mainly Windows
UEFI CA 2023. These must be supported:

a) At the Secure Boot level, with entries installed in a system's allowed items database (DB), and
b) At the boot manager/loader level, with a system using boot binaries signed with such certificates

Machines manufactured in 2024 and newer already contain Windows UEFI CA 2023 certificates in DB.
Older UEFI systems are starting to receive Secure Boot updates during monthly cumulative update
rollouts. If you wonder whether your machine has these updates installed, you can perform checks
in one of 2 ways:

a) Within Windows:
    
    1. Open PowerShell as an administrator
    2. Verify the Secure Boot status with Confirm-SecureBootUEFI
    3. Verify the presence of Windows UEFI CA 2023:

    [System.Text.Encoding]::ASCII.GetString((Get-SecureBootUEFI DB).Bytes) -match 'Windows UEFI CA 2023'

    Both Secure Boot presence confirmation and DB query will return either True or False.

b) Within your computer's firmware (UEFI):

    Check the state of Secure Boot in your firmware. Depending on the manufacturer and model of
    computer, you may find this option in different locations under different names. If Secure Boot
    is enabled, check the keys present.

    Normally, you should see these keys grouped under the various key databases (DB for allowed items,
    DBX for blocked items, and so on). Check whether Windows UEFI CA 2023 is in DB

The Preinstallation Environment Helper will perform these checks and tell you the following if they
succeed:

    You may be able to use the UEFI CA 2023 binaries on this system.

        If UEFI CA 2023 certificates are in DB

    -- or --

    You may not be able to use the UEFI CA 2023 binaries on this system.

        If UEFI CA 2023 certificates are not in DB

For this to happen, the Preinstallation Environment must also contain a copy of BCDBoot with support
for copying specific versions of the boot binary based on its flags (specifically a /bootex flag). You
may see this prompt suppressed when configuring the environment with policies (versions 0.8 and later;
more info later). When it is configured to automatically use the UEFI CA 2023 boot binaries, it is up
to the Preinstallation Environment Helper to detect whether the target image supports such binaries.
If they are not present, the Preinstallation Environment Helper will fall back to the old boot binaries.

Now, a modern system will continue to boot systems using Microsoft Windows Production PCA 2011
certificates if it too is in DB. However, this will not happen if it is in DBX.

As for systems that have received updated boot binaries but do not have Secure Boot enabled, it is
likely that events will appear related to the certificates in the event log. It is assumed, however, that
this should not cause any issues after June 2026.

3. Policies

DISMTools 0.8 introduced policies as a means of letting end-users customize the environment with certain
preferences. These preferences range from partition table overrides to use by default, to behavior
when loading the Driver Installation Module, and more. Policies used in a session are saved to the
Preinstallation Environment as registry values found in:

    HKEY_LOCAL_MACHINE\SOFTWARE\DISMTools\Preinstallation Environment\Policies

These are referred to as custom policies. DISMTools also has a set of policies used by default when
creating ISO files. These are known as default policies, and can be changed by the end-user too
by tweaking application settings (either in settings.ini or the registry). You can tweak the
default policies if you want policies to persist between ISO creation sessions.

For more information on policies, refer to the Help documentation of the software (versions 0.8 and
later).

=== Preboot eXecution Environment (PXE) Helpers Notes ===

1. Common Notes

The PXE Helpers operate using a client-server architecture. From the server you must start the server
component for a specific solution. This component is an API listener.

The client component is included in the DISMTools Preinstallation Environment and can be accessed
using the PXE Helpers provider menu (access that by pressing option 2 from the main menu).

2. Windows Deployment Services Helper

You may store the boot image in this disc image file in a Windows Deployment Services (WDS) server
for use on network-based deployments. When this is done, the following is assumed:

a) You have correctly set up the infrastructure. The server must contain, at least, the Dynamic
   Host Configuration Protocol (DHCP) role. Scopes (whether IPv4 or IPv6) must have been created
   and must have been activated. The server must have been configured to transfer Network
   Bootstrap Programs (NBPs) to clients
b) A server exists in your network that contains the WDS role (it can be the same server as the
   DHCP server, or a different server) and has been configured to allow requests from unknown
   devices

In previous versions of DISMTools, a preparation script was included. With version 0.8, the preparation
script has been removed in favor of the WDS Helper because newer boot images no longer work correctly
with WDS.

Alongside that, there are some limitations of the WDS Helper:

a) The client will not honor any filters set in installation images

=== Warranty disclaimers ===

We are not responsible for the changes that you make to your computer's operating system
or its disk configuration. Be sure to follow every step in the installation process
very carefully. It is recommended to use virtual machine (VM) software, or spare
computers, to test your custom images. That way, you can perform testing without
modifying your computer.

Only after testing your Windows image and verifying that it works successfully can
you proceed with a larger deployment of the individual image.

Thanks for understanding this.

                                                              - CodingWonders Software