param (
    [Parameter(Mandatory = $true, Position = 0)] [string] $imageGroup,
    [Parameter(Mandatory = $true, Position = 1)] [string] $installImagePath,
    [Parameter(Position = 2)] [int] $installImageIndex = 0
)

# ----------------------------------------------------------------------
# Native layer. The P/Invoke surface lives in a small C# helper so that
# out/ref parameters, structs and buffer freeing are handled with real
# types on the C# side, which keeps the PowerShell code untyped.
# ----------------------------------------------------------------------
if (-not ('WimNative.WimApi' -as [type])) {
    Add-Type -TypeDefinition @'
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
 
namespace WimNative
{
    public class WimBasicInfo
    {
        public Guid Guid { get; set; }
        public uint ImageCount { get; set; }
        public string Compression { get; set; }
        public int PartNumber { get; set; }
        public int TotalParts { get; set; }
        public uint BootIndex { get; set; }
        public string Attributes { get; set; }
    }
 
    public static class WimApi
    {
        private const string Dll = "wimgapi.dll";
        private const uint WIM_GENERIC_READ = 0x80000000;
        private const uint WIM_OPEN_EXISTING = 3;
        private const int MAX_PATH = 260;
 
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct WIM_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string WimPath;
            public Guid Guid;
            public uint ImageCount;
            public uint CompressionType;
            public ushort PartNumber;
            public ushort TotalParts;
            public uint BootIndex;
            public uint WimAttributes;
            public uint WimFlagsAndAttr;
        }
 
        [DllImport(Dll, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr WIMCreateFile(string path, uint access, uint disposition,
            uint flags, uint compression, out uint creationResult);
 
        [DllImport(Dll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WIMCloseHandle(IntPtr handle);
 
        [DllImport(Dll, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WIMSetTemporaryPath(IntPtr hWim, string path);
 
        [DllImport(Dll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WIMGetAttributes(IntPtr hWim, ref WIM_INFO info, uint size);
 
        [DllImport(Dll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WIMGetImageInformation(IntPtr handle, out IntPtr buffer, out uint size);
 
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LocalFree(IntPtr mem);
 
        public static IntPtr Open(string path)
        {
            uint creationResult;
            IntPtr handle = WIMCreateFile(path, WIM_GENERIC_READ, WIM_OPEN_EXISTING, 0, 0, out creationResult);
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            // Not needed just to read info, but required if you later load/mount images
            WIMSetTemporaryPath(handle, System.IO.Path.GetTempPath());
            return handle;
        }
 
        public static void Close(IntPtr handle)
        {
            if (handle != IntPtr.Zero)
            {
                WIMCloseHandle(handle);
            }
        }
 
        public static WimBasicInfo GetBasicInfo(IntPtr hWim)
        {
            WIM_INFO info = new WIM_INFO();
            uint size = (uint)Marshal.SizeOf(typeof(WIM_INFO));
            if (!WIMGetAttributes(hWim, ref info, size))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
 
            WimBasicInfo result = new WimBasicInfo();
            result.Guid = info.Guid;
            result.ImageCount = info.ImageCount;
            result.PartNumber = info.PartNumber;
            result.TotalParts = info.TotalParts;
            result.BootIndex = info.BootIndex;
            result.Compression = CompressionName(info.CompressionType);
            result.Attributes = AttributeNames(info.WimAttributes);
            return result;
        }
 
        // Returns the XML metadata for the whole WIM (all images)
        public static string GetXml(IntPtr hWim)
        {
            IntPtr buffer;
            uint byteCount;
            if (!WIMGetImageInformation(hWim, out buffer, out byteCount))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            try
            {
                // UTF-16 text with a leading BOM
                string text = Marshal.PtrToStringUni(buffer, (int)(byteCount / 2));
                return text.TrimStart('\uFEFF');
            }
            finally
            {
                LocalFree(buffer);
            }
        }
 
        private static string CompressionName(uint value)
        {
            switch (value)
            {
                case 0: return "None";
                case 1: return "XPRESS";
                case 2: return "LZX";
                case 3: return "LZMS";
                default: return "Unknown (" + value + ")";
            }
        }
 
        private static string AttributeNames(uint flags)
        {
            if (flags == 0) { return "Normal"; }
            List<string> names = new List<string>();
            if ((flags & 0x01) != 0) { names.Add("ResourceOnly"); }
            if ((flags & 0x02) != 0) { names.Add("MetadataOnly"); }
            if ((flags & 0x04) != 0) { names.Add("VerifyData"); }
            if ((flags & 0x08) != 0) { names.Add("RpFix"); }
            if ((flags & 0x10) != 0) { names.Add("Spanned"); }
            if ((flags & 0x20) != 0) { names.Add("WriteInProgress"); }
            return string.Join(", ", names.ToArray());
        }
    }
}
'@
}
 
# ----------------------------------------------------------------------
# Small XML / conversion helpers
# ----------------------------------------------------------------------
function Get-NodeText {
    param($Node, $XPath)
    if ($null -eq $Node) { return '' }
    $child = $Node.SelectSingleNode($XPath)
    if ($null -eq $child) { return '' }
    return $child.InnerText
}
 
function ConvertTo-LongSafe {
    param($Text)
    $value = 0L
    [void][long]::TryParse($Text, [ref]$value)
    return $value
}
 
function ConvertFrom-HexText {
    param($Text)
    if ([string]::IsNullOrEmpty($Text)) { return $null }
    $clean = $Text.Trim() -replace '^0[xX]', ''
    try { return [Convert]::ToUInt32($clean, 16) } catch { return $null }
}
 
# The XML stores FILETIMEs as <HIGHPART>0x...</HIGHPART><LOWPART>0x...</LOWPART>
function Get-FileTimeUtc {
    param($Node, $XPath)
    $timeNode = $Node.SelectSingleNode($XPath)
    if ($null -eq $timeNode) { return $null }
 
    $high = ConvertFrom-HexText (Get-NodeText $timeNode 'HIGHPART')
    $low = ConvertFrom-HexText (Get-NodeText $timeNode 'LOWPART')
    if ($null -eq $high -or $null -eq $low) { return $null }
 
    $ticks = ($high * 4294967296) + $low
    if ($ticks -le 0) { return $null }
    return [DateTime]::FromFileTimeUtc($ticks)
}
 
# PROCESSOR_ARCHITECTURE_* values
function ConvertTo-ArchName {
    param($Value)
    switch ($Value) {
        '0'  { return 'x86' }
        '5'  { return 'ARM' }
        '6'  { return 'IA64' }
        '9'  { return 'x64' }
        '12' { return 'ARM64' }
        ''   { return 'Unknown' }
        default { return "Unknown ($Value)" }
    }
}
 
# ----------------------------------------------------------------------
# Main function: WIMGAPI attributes + per-image XML + header version
# ----------------------------------------------------------------------
function Get-WimInfo {
    param($Path)
 
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "The WIM file was not found: $Path"
    }
    $fullPath = (Resolve-Path -LiteralPath $Path).ProviderPath
 
    $handle = [WimNative.WimApi]::Open($fullPath)
    try {
        $basic = [WimNative.WimApi]::GetBasicInfo($handle)
        $xmlText = [WimNative.WimApi]::GetXml($handle)
    }
    finally {
        [WimNative.WimApi]::Close($handle)
    }
 
    $doc = New-Object System.Xml.XmlDocument
    $doc.LoadXml($xmlText)
 
    $images = @()
    foreach ($imageNode in $doc.SelectNodes('/WIM/IMAGE')) {
        $windows = $imageNode.SelectSingleNode('WINDOWS')   # absent for non-Windows images
 
        $languages = @()
        $defaultLanguage = ''
        $version = ''
        $edition = ''
        $installType = ''
        $productName = ''
        $productType = ''
        $productSuite = ''
        $systemRoot = ''
        $arch = 'Unknown'
 
        if ($null -ne $windows) {
            $edition = Get-NodeText $windows 'EDITIONID'
            $installType = Get-NodeText $windows 'INSTALLATIONTYPE'
            $productName = Get-NodeText $windows 'PRODUCTNAME'
            $productType = Get-NodeText $windows 'PRODUCTTYPE'
            $productSuite = Get-NodeText $windows 'PRODUCTSUITE'
            $systemRoot = Get-NodeText $windows 'SYSTEMROOT'
            $arch = ConvertTo-ArchName (Get-NodeText $windows 'ARCH')
 
            $versionNode = $windows.SelectSingleNode('VERSION')
            if ($null -ne $versionNode) {
                $version = '{0}.{1}.{2}.{3}' -f (Get-NodeText $versionNode 'MAJOR'),
                                                (Get-NodeText $versionNode 'MINOR'),
                                                (Get-NodeText $versionNode 'BUILD'),
                                                (Get-NodeText $versionNode 'SPBUILD')
            }
 
            foreach ($languageNode in $windows.SelectNodes('LANGUAGES/LANGUAGE')) {
                $languages += $languageNode.InnerText
            }
            $defaultLanguage = Get-NodeText $windows 'LANGUAGES/DEFAULT'
        }
 
        $images += [pscustomobject]@{
            Index               = $imageNode.GetAttribute('INDEX')
            Name                = Get-NodeText $imageNode 'NAME'
            Description         = Get-NodeText $imageNode 'DESCRIPTION'
            EditionId           = $edition
            InstallationType    = $installType
            ProductName         = $productName
            ProductType         = $productType
            ProductSuite        = $productSuite
            Architecture        = $arch
            Version             = $version
            SystemRoot          = $systemRoot
            Languages           = ($languages -join ', ')
            DefaultLanguage     = $defaultLanguage
            DirectoryCount      = ConvertTo-LongSafe (Get-NodeText $imageNode 'DIRCOUNT')
            FileCount           = ConvertTo-LongSafe (Get-NodeText $imageNode 'FILECOUNT')
            TotalBytes          = ConvertTo-LongSafe (Get-NodeText $imageNode 'TOTALBYTES')
            CreationTimeUtc     = Get-FileTimeUtc $imageNode 'CREATIONTIME'
            LastModifiedTimeUtc = Get-FileTimeUtc $imageNode 'LASTMODIFICATIONTIME'
        }
    }
 
    return [pscustomobject]@{
        Path          = $fullPath
        Guid          = $basic.Guid
        FormatVersion = $formatVersion.Display
        Compression   = $basic.Compression
        PartNumber    = $basic.PartNumber
        TotalParts    = $basic.TotalParts
        BootIndex     = $basic.BootIndex
        Attributes    = $basic.Attributes
        ImageCount    = $basic.ImageCount
        TotalBytes    = ConvertTo-LongSafe (Get-NodeText $doc.DocumentElement 'TOTALBYTES')
        Images        = $images
    }
}

function Write-ErrorMessage {
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $message
    )
    Write-Error -Message $message
    Write-Host "Press ENTER to exit..."
    Read-Host | Out-Null
}

function New-WdsInstallImage {
    param (
        [Parameter(Mandatory = $true, Position = 0)] [string] $imageGroup,
        [Parameter(Mandatory = $true, Position = 1)] [string] $installImagePath,
        [Parameter(Position = 2)] [int] $installImageIndex = 0
    )

    if (($imageGroup -eq "") -or ($installImagePath -eq "") -or ($installImageIndex -lt 0)) { return }

    Write-Host "Preparing to add installation image to group `"$imageGroup`"..."

    if (-not (Test-Path -Path "$installImagePath" -PathType Leaf)) {
        Write-ErrorMessage -message "The installation image does not exist in the file system."
        return
    }

    # Check if the group exists and create it if it doesn't
    try {
        Get-WdsInstallImageGroup -Name "$imageGroup" -ErrorAction Stop
    } catch {
        Write-Host "Group does not exist. Creating it..."
        New-WdsInstallImageGroup -Name "$imageGroup"
    }

    # If no index is provided we need to add all of the indexes of the source image; otherwise
    # we grab the index name from the Windows image; wdsutil doesn't support /singleimage with
    # index numbers. To provide some better differentiation so we don't have "install-(2)", we
    # will also try to grab the EditionID, though this WON'T prevent unique numbers to be appended
    # if there happens to be an image with that same name.

    # WDS does NOT use the DISM API, but the older WIMGAPI. That means we need to update the calls...
    $providedIndexName = ""
    $providedIndexEditionId = ""
    if ($installImageIndex -ge 1) {
        Write-Host "Single image mode detected. Grabbing image name and edition ID..."
        try {
            $installImageInfo = Get-WimInfo -Path "$installImagePath"
            if ($null -ne $installImageInfo) {
                $imageInfo = $installImageInfo.Images[$installImageIndex - 1]

                $providedIndexName = $imageInfo.Name
                $providedIndexEditionId = $imageInfo.EditionId

                Write-Host "Obtained index name: $providedIndexName"
                Write-Host "Obtained index edition ID: $providedIndexEditionId"
            }
        } catch {
            # Fall back to adding all of them
            Write-Host "Could not grab image information. Reverting to adding all install images..."
            $installImageIndex = 0
        }
    }

    $argList = "/verbose /progress /add-image /imagefile:`"$installImagePath`" /imagetype:install /imagegroup:`"$imageGroup`""
    if ($installImageIndex -gt 0) {
        $destImageFileName = "$([IO.Path]::GetFileNameWithoutExtension("$installImagePath"))_$providedIndexEditionId_$([System.Random]::new().Next())$([IO.Path]::GetExtension("$installImagePath"))"
        $argList += " /singleimage:`"$providedIndexName`" /filename:`"$destImageFileName`""
    }

    $wdsUtilProc = Start-Process -FilePath "wdsutil" -ArgumentList "$argList" -NoNewWindow -Wait -PassThru

    return $?
}

if (New-WdsInstallImage -imageGroup $imageGroup -installImagePath $installImagePath -installImageIndex $installImageIndex) {
    Write-Host "Image addition succeeded."
} else {
    Write-Host "Image addition failed."
    Read-Host | Out-Null
}