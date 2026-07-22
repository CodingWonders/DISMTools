$ErrorActionPreference = "Stop"

$root = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
$rootPackagesDir = Join-Path $root "packages"
$hotInstallRoot = Join-Path $root "Helpers\extps1\PE_Helper\tools\HotInstall"
$hotInstallPackagesDir = Join-Path $hotInstallRoot "packages"
$themeDesignerPackagesDir = Join-Path $root "Tools\DT_ThemeDesigner\packages"
$primaryPackageArchive = Join-Path $root "tools\build\pkgsrc.bundle"
$packageArchive = $null

function Test-ZipHeader {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        return $false
    }

    $stream = [System.IO.File]::OpenRead($Path)
    try {
        if ($stream.Length -lt 4) {
            return $false
        }

        $buffer = New-Object byte[] 4
        [void]$stream.Read($buffer, 0, 4)
        return ($buffer[0] -eq 0x50 -and $buffer[1] -eq 0x4B)
    }
    finally {
        $stream.Dispose()
    }
}

function Test-GitLfsPointer {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        return $false
    }

    $stream = [System.IO.File]::OpenRead($Path)
    try {
        $length = [Math]::Min([int]$stream.Length, 256)
        if ($length -le 0) {
            return $false
        }

        $buffer = New-Object byte[] $length
        [void]$stream.Read($buffer, 0, $length)
        $header = [System.Text.Encoding]::ASCII.GetString($buffer)
        return $header.StartsWith("version https://git-lfs.github.com/spec/")
    }
    finally {
        $stream.Dispose()
    }
}

function Assert-RequiredPackageFile {
    param(
        [Parameter(Mandatory = $true)]
        [string]$RelativePath
    )

    $path = Join-Path $root $RelativePath
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        throw "Required package file is missing after preparation: $RelativePath"
    }
}

function Assert-PackageFolder {
    param(
        [Parameter(Mandatory = $true)]
        [string]$PackageFolder
    )

    $path = Join-Path $rootPackagesDir $PackageFolder
    if (-not (Test-Path -LiteralPath $path -PathType Container)) {
        throw "Required package folder is missing after extracting the package archive: packages\$PackageFolder"
    }
}

function Copy-PackageFolder {
    param(
        [Parameter(Mandatory = $true)]
        [string]$PackageFolder,

        [Parameter(Mandatory = $true)]
        [string]$DestinationPackagesDir
    )

    $source = Join-Path $rootPackagesDir $PackageFolder
    if (-not (Test-Path -LiteralPath $source -PathType Container)) {
        throw "Cannot copy nested package. Source package folder is missing: $source"
    }

    New-Item -ItemType Directory -Path $DestinationPackagesDir -Force | Out-Null
    $destination = Join-Path $DestinationPackagesDir $PackageFolder

    if (Test-Path -LiteralPath $destination) {
        Remove-Item -LiteralPath $destination -Recurse -Force
    }

    Copy-Item -LiteralPath $source -Destination $destination -Recurse -Force
}

function Assert-PackagesConfigFolders {
    param(
        [Parameter(Mandatory = $true)]
        [string]$PackagesConfig,

        [Parameter(Mandatory = $true)]
        [string]$PackagesDirectory
    )

    if (-not (Test-Path -LiteralPath $PackagesConfig -PathType Leaf)) {
        throw "packages.config was not found: $PackagesConfig"
    }

    [xml]$packagesXml = Get-Content -LiteralPath $PackagesConfig
    foreach ($package in $packagesXml.packages.package) {
        $folderName = "$($package.id).$($package.version)"
        $folderPath = Join-Path $PackagesDirectory $folderName
        if (-not (Test-Path -LiteralPath $folderPath -PathType Container)) {
            throw "Package folder from packages.config is missing: $folderPath"
        }
    }
}

function Assert-ProjectHintPathFiles {
    param(
        [Parameter(Mandatory = $true)]
        [string]$ProjectFile,

        [Parameter(Mandatory = $true)]
        [string]$ProjectRoot
    )

    if (-not (Test-Path -LiteralPath $ProjectFile -PathType Leaf)) {
        throw "Project file was not found: $ProjectFile"
    }

    [xml]$projectXml = Get-Content -LiteralPath $ProjectFile
    $namespaceManager = New-Object System.Xml.XmlNamespaceManager($projectXml.NameTable)
    $namespaceManager.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003")

    $hintPathNodes = $projectXml.SelectNodes("//msb:Reference/msb:HintPath", $namespaceManager)
    $missingFiles = New-Object System.Collections.Generic.List[string]

    foreach ($hintPathNode in $hintPathNodes) {
        $relativeHintPath = $hintPathNode.InnerText.Trim()
        if ([string]::IsNullOrWhiteSpace($relativeHintPath)) {
            continue
        }

        if (-not $relativeHintPath.EndsWith(".dll", [System.StringComparison]::OrdinalIgnoreCase)) {
            continue
        }

        $path = Join-Path $ProjectRoot $relativeHintPath
        if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
            $missingFiles.Add($relativeHintPath) | Out-Null
        }
    }

    if ($missingFiles.Count -gt 0) {
        $message = "Some project HintPath DLL files are still missing:" + [Environment]::NewLine + ($missingFiles -join [Environment]::NewLine)
        throw $message
    }
}

foreach ($directory in @($rootPackagesDir, $hotInstallPackagesDir, $themeDesignerPackagesDir)) {
    if (Test-Path -LiteralPath $directory) {
        Remove-Item -LiteralPath $directory -Recurse -Force
    }
}

if (Test-Path -LiteralPath $primaryPackageArchive -PathType Leaf) {
    $packageArchive = $primaryPackageArchive
    Write-Host "Using bundled package archive: tools\build\pkgsrc.bundle"
}
else {
    throw "Bundled package archive was not found. Add tools\build\pkgsrc.bundle before building."
}

if ($null -ne $packageArchive) {
    $packageArchiveLabel = Resolve-Path -LiteralPath $packageArchive

    if (Test-GitLfsPointer -Path $packageArchive) {
        throw "$packageArchiveLabel is a Git LFS pointer, not a real ZIP archive. Commit the real archive file. The preferred path is tools\build\pkgsrc.bundle because it is not matched by ZIP LFS rules."
    }

    if (-not (Test-ZipHeader -Path $packageArchive)) {
        $size = (Get-Item -LiteralPath $packageArchive).Length
        throw "$packageArchiveLabel is not a valid ZIP archive. Current file size is $size bytes."
    }

    Write-Host "$packageArchiveLabel is a valid ZIP archive. Extracting bundled packages."
    New-Item -ItemType Directory -Path $rootPackagesDir -Force | Out-Null
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::ExtractToDirectory($packageArchive, $rootPackagesDir)
    Write-Host "Bundled packages were extracted to $rootPackagesDir"

    Copy-PackageFolder -PackageFolder "ini-parser.2.5.2" -DestinationPackagesDir $hotInstallPackagesDir
    Copy-PackageFolder -PackageFolder "Microsoft.Dism.6.0.0" -DestinationPackagesDir $hotInstallPackagesDir
    Copy-PackageFolder -PackageFolder "ini-parser.2.5.2" -DestinationPackagesDir $themeDesignerPackagesDir
}

Assert-PackagesConfigFolders -PackagesConfig (Join-Path $root "packages.config") -PackagesDirectory $rootPackagesDir
Assert-PackagesConfigFolders -PackagesConfig (Join-Path $root "Tools\AutoReloadService\packages.config") -PackagesDirectory $rootPackagesDir
Assert-PackagesConfigFolders -PackagesConfig (Join-Path $hotInstallRoot "Installer\packages.config") -PackagesDirectory $hotInstallPackagesDir

Assert-RequiredPackageFile "packages\Microsoft.Dism.6.0.0\lib\net472\Microsoft.Dism.dll"
Assert-RequiredPackageFile "packages\ini-parser.2.5.2\lib\net20\INIFileParser.dll"
Assert-RequiredPackageFile "packages\Scintilla5.NET.6.1.2\build\scintilla5.net.targets"
Assert-RequiredPackageFile "Helpers\extps1\PE_Helper\tools\HotInstall\packages\ini-parser.2.5.2\lib\net20\INIFileParser.dll"
Assert-RequiredPackageFile "Helpers\extps1\PE_Helper\tools\HotInstall\packages\Microsoft.Dism.6.0.0\lib\net472\Microsoft.Dism.dll"
Assert-RequiredPackageFile "Tools\DT_ThemeDesigner\packages\ini-parser.2.5.2\lib\net20\INIFileParser.dll"

foreach ($folder in @(
    "DarkUI.2.0.2",
    "ini-parser.2.5.2",
    "Markdig.1.3.1",
    "Microsoft.Dism.6.0.0",
    "Scintilla5.NET.6.1.2",
    "System.Runtime.4.3.1",
    "System.Security.Cryptography.Algorithms.4.3.1",
    "System.Security.Cryptography.X509Certificates.4.3.2",
    "Tulpep.ActiveDirectoryObjectPicker.3.0.11",
    "WindowsAPICodePack.8.0.15.2"
)) {
    Assert-PackageFolder -PackageFolder $folder
}

Assert-ProjectHintPathFiles -ProjectFile (Join-Path $root "DISMTools.vbproj") -ProjectRoot $root
Assert-ProjectHintPathFiles -ProjectFile (Join-Path $hotInstallRoot "Installer\HotInstall.vbproj") -ProjectRoot (Join-Path $hotInstallRoot "Installer")
Assert-ProjectHintPathFiles -ProjectFile (Join-Path $root "Tools\DT_ThemeDesigner\DT_ThemeDesigner.vbproj") -ProjectRoot (Join-Path $root "Tools\DT_ThemeDesigner")

Write-Host "NuGet package preparation completed successfully from the bundled package archive."
