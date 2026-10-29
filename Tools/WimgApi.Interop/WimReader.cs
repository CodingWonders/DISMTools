using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Globalization;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;

namespace WimgApi.Interop
{
    // ------------------------------------------------------------------
    // Native layer: P/Invoke declarations for wimgapi.dll
    // ------------------------------------------------------------------
    internal static class NativeMethods
    {
        private const string WimgApi = "wimgapi.dll";

        // Access / disposition / compression constants (from wimgapi.h)
        internal const uint WIM_GENERIC_READ = 0x80000000;
        internal const uint WIM_OPEN_EXISTING = 3;

        internal const uint WIM_COMPRESS_NONE = 0;
        internal const uint WIM_COMPRESS_XPRESS = 1;
        internal const uint WIM_COMPRESS_LZX = 2;
        internal const uint WIM_COMPRESS_LZMS = 3;

        internal const int MAX_PATH = 260;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WIM_INFO
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

        [DllImport(WimgApi, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr WIMCreateFile(
            string pszWimPath,
            uint dwDesiredAccess,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            uint dwCompressionType,
            out uint pdwCreationResult);

        [DllImport(WimgApi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WIMCloseHandle(IntPtr hObject);

        [DllImport(WimgApi, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WIMSetTemporaryPath(IntPtr hWim, string pszPath);

        [DllImport(WimgApi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WIMGetAttributes(
            IntPtr hWim,
            ref WIM_INFO pWimInfo,
            uint cbWimInfo);

        [DllImport(WimgApi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WIMGetImageInformation(
            IntPtr hImage,
            out IntPtr ppvImageInfo,
            out uint pcbImageInfo);

        // The buffer returned by WIMGetImageInformation must be released with LocalFree
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LocalFree(IntPtr hMem);
    }

    // ------------------------------------------------------------------
    // Managed model
    // ------------------------------------------------------------------
    [Flags]
    public enum WimAttributes : uint
    {
        Normal = 0x00000000,
        ResourceOnly = 0x00000001,
        MetadataOnly = 0x00000002,
        VerifyData = 0x00000004,
        RpFix = 0x00000008,
        Spanned = 0x00000010,
        WriteInProgress = 0x00000020
    }

    public sealed class WimImageInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EditionId { get; set; }
        public string InstallationType { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string ProductSuite { get; set; }
        public string Architecture { get; set; }
        public string Version { get; set; }
        public string SystemRoot { get; set; }
        public string DefaultLanguage { get; set; }
        public List<string> Languages { get; set; }
        public long DirectoryCount { get; set; }
        public long FileCount { get; set; }
        public long TotalBytes { get; set; }
        public DateTime? CreationTimeUtc { get; set; }
        public DateTime? LastModificationTimeUtc { get; set; }

        public WimImageInfo()
        {
            Languages = new List<string>();
        }
    }

    public sealed class WimFileInfo
    {
        public string Path { get; set; }
        public Guid Guid { get; set; }
        public int ImageCount { get; set; }
        public string Compression { get; set; }
        public int PartNumber { get; set; }
        public int TotalParts { get; set; }
        public int BootIndex { get; set; }
        public WimAttributes Attributes { get; set; }
        public long TotalBytes { get; set; }
        public List<WimImageInfo> Images { get; set; }

        public WimFileInfo()
        {
            Images = new List<WimImageInfo>();
        }
    }

    // ------------------------------------------------------------------
    // Reader: wraps the WIMGAPI calls and parses the XML metadata
    // ------------------------------------------------------------------
    public static class WimReader
    {
        public static WimFileInfo GetWimInfo(string wimPath)
        {
            if (string.IsNullOrEmpty(wimPath))
            {
                throw new ArgumentNullException("wimPath");
            }
            if (!File.Exists(wimPath))
            {
                throw new FileNotFoundException("The WIM file was not found.", wimPath);
            }

            uint creationResult = 0;
            IntPtr hWim = NativeMethods.WIMCreateFile(
                wimPath,
                NativeMethods.WIM_GENERIC_READ,
                NativeMethods.WIM_OPEN_EXISTING,
                0,
                NativeMethods.WIM_COMPRESS_NONE, // ignored when opening an existing file
                out creationResult);

            if (hWim == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            try
            {
                // WIMGAPI wants a temp path for most operations; harmless to set it.
                NativeMethods.WIMSetTemporaryPath(hWim, Path.GetTempPath());

                WimFileInfo result = new WimFileInfo();
                result.Path = wimPath;

                // 1) Basic attributes via WIMGetAttributes
                NativeMethods.WIM_INFO nativeInfo = new NativeMethods.WIM_INFO();
                uint structSize = (uint)Marshal.SizeOf(typeof(NativeMethods.WIM_INFO));
                if (!NativeMethods.WIMGetAttributes(hWim, ref nativeInfo, structSize))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                result.Guid = nativeInfo.Guid;
                result.ImageCount = (int)nativeInfo.ImageCount;
                result.Compression = CompressionToString(nativeInfo.CompressionType);
                result.PartNumber = nativeInfo.PartNumber;
                result.TotalParts = nativeInfo.TotalParts;
                result.BootIndex = (int)nativeInfo.BootIndex;
                result.Attributes = (WimAttributes)nativeInfo.WimAttributes;

                // 2) Full XML metadata (per-image details) via WIMGetImageInformation
                string xml = GetXmlInformation(hWim);
                ParseXml(xml, result);

                return result;
            }
            finally
            {
                NativeMethods.WIMCloseHandle(hWim);
            }
        }

        private static string GetXmlInformation(IntPtr hWim)
        {
            IntPtr buffer = IntPtr.Zero;
            uint byteCount = 0;

            if (!NativeMethods.WIMGetImageInformation(hWim, out buffer, out byteCount))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            try
            {
                // The buffer is UTF-16 (little endian) text that begins with a BOM.
                string text = Marshal.PtrToStringUni(buffer, (int)(byteCount / 2));
                return text.TrimStart('\uFEFF');
            }
            finally
            {
                NativeMethods.LocalFree(buffer);
            }
        }

        private static void ParseXml(string xml, WimFileInfo target)
        {
            XDocument document = XDocument.Parse(xml);
            XElement root = document.Root;
            if (root == null)
            {
                return;
            }

            target.TotalBytes = ParseLong(GetValue(root, "TOTALBYTES"));

            foreach (XElement imageElement in root.Elements("IMAGE"))
            {
                WimImageInfo image = new WimImageInfo();

                int index = 0;
                int.TryParse((string)imageElement.Attribute("INDEX"), NumberStyles.Integer, CultureInfo.InvariantCulture, out index);
                image.Index = index;

                image.Name = GetValue(imageElement, "NAME");
                image.Description = GetValue(imageElement, "DESCRIPTION");
                image.DirectoryCount = ParseLong(GetValue(imageElement, "DIRCOUNT"));
                image.FileCount = ParseLong(GetValue(imageElement, "FILECOUNT"));
                image.TotalBytes = ParseLong(GetValue(imageElement, "TOTALBYTES"));
                image.CreationTimeUtc = ParseFileTime(imageElement.Element("CREATIONTIME"));
                image.LastModificationTimeUtc = ParseFileTime(imageElement.Element("LASTMODIFICATIONTIME"));

                // The WINDOWS node only exists for Windows images
                XElement windows = imageElement.Element("WINDOWS");
                if (windows != null)
                {
                    image.EditionId = GetValue(windows, "EDITIONID");
                    image.InstallationType = GetValue(windows, "INSTALLATIONTYPE");
                    image.ProductName = GetValue(windows, "PRODUCTNAME");
                    image.ProductType = GetValue(windows, "PRODUCTTYPE");
                    image.ProductSuite = GetValue(windows, "PRODUCTSUITE");
                    image.SystemRoot = GetValue(windows, "SYSTEMROOT");
                    image.Architecture = ArchitectureToString(GetValue(windows, "ARCH"));

                    XElement version = windows.Element("VERSION");
                    if (version != null)
                    {
                        image.Version = string.Format(
                            CultureInfo.InvariantCulture,
                            "{0}.{1}.{2}.{3}",
                            GetValue(version, "MAJOR"),
                            GetValue(version, "MINOR"),
                            GetValue(version, "BUILD"),
                            GetValue(version, "SPBUILD"));
                    }

                    XElement languages = windows.Element("LANGUAGES");
                    if (languages != null)
                    {
                        foreach (XElement language in languages.Elements("LANGUAGE"))
                        {
                            image.Languages.Add(language.Value);
                        }
                        image.DefaultLanguage = GetValue(languages, "DEFAULT");
                    }
                }

                target.Images.Add(image);
            }
        }

        // ---------------- helpers ----------------

        private static string GetValue(XElement parent, string childName)
        {
            XElement child = parent.Element(childName);
            return child == null ? string.Empty : child.Value;
        }

        private static long ParseLong(string text)
        {
            long value = 0;
            long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
            return value;
        }

        // The XML stores FILETIMEs as two hex strings: <HIGHPART>0x01D...</HIGHPART><LOWPART>0x...</LOWPART>
        private static DateTime? ParseFileTime(XElement element)
        {
            if (element == null)
            {
                return null;
            }

            uint high = 0;
            uint low = 0;
            if (!TryParseHex(GetValue(element, "HIGHPART"), out high) ||
                !TryParseHex(GetValue(element, "LOWPART"), out low))
            {
                return null;
            }

            long fileTime = ((long)high << 32) | low;
            if (fileTime <= 0)
            {
                return null;
            }

            return DateTime.FromFileTimeUtc(fileTime);
        }

        private static bool TryParseHex(string text, out uint value)
        {
            value = 0;
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            string trimmed = text.Trim();
            if (trimmed.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                trimmed = trimmed.Substring(2);
            }
            return uint.TryParse(trimmed, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }

        private static string CompressionToString(uint compression)
        {
            switch (compression)
            {
                case NativeMethods.WIM_COMPRESS_NONE: return "None";
                case NativeMethods.WIM_COMPRESS_XPRESS: return "XPRESS";
                case NativeMethods.WIM_COMPRESS_LZX: return "LZX";
                case NativeMethods.WIM_COMPRESS_LZMS: return "LZMS";
                default: return "Unknown (" + compression.ToString(CultureInfo.InvariantCulture) + ")";
            }
        }

        // PROCESSOR_ARCHITECTURE_* values
        private static string ArchitectureToString(string arch)
        {
            switch (arch)
            {
                case "0": return "x86";
                case "5": return "ARM";
                case "6": return "IA64";
                case "9": return "x64";
                case "12": return "ARM64";
                default: return string.IsNullOrEmpty(arch) ? "Unknown" : "Unknown (" + arch + ")";
            }
        }
    }
}
