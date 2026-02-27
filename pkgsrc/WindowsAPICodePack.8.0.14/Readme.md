# WindowsAPICodePack - Complete Package

This is the unified package containing all Windows API Code Pack components:

## Included Components

- **Core** - Essential Windows API functionality, Task Dialogs, Power Management, Network awareness
- **Shell** - Shell integration, Common File Dialogs, Explorer Browser, Taskbar, Jump Lists
- **Sensors** - Windows Sensor Platform integration
- **ExtendedLinguisticServices** - Language detection and text services
- **ShellExtensions** - Preview handlers and thumbnail providers

## Installation

```bash
# Install via Package Manager Console
Install-Package WindowsAPICodePack

# Or via .NET CLI
dotnet add package WindowsAPICodePack
```

## Usage

All components are available through their respective namespaces:

```csharp
using Microsoft.WindowsAPICodePack.Core;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Sensors;
using Microsoft.WindowsAPICodePack.ExtendedLinguisticServices;
using Microsoft.WindowsAPICodePack.ShellExtensions;
```

## Supported Frameworks

- .NET Framework 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1
- .NET 8.0 (Windows)
- .NET 9.0 (Windows)

## Individual Packages

If you prefer to install components individually:

- `WindowsAPICodePackCore`
- `WindowsAPICodePackShell`
- `WindowsAPICodePackSensors`
- `WindowsAPICodePackExtendedLinguisticServices`
- `WindowsAPICodePackShellExtensions`

## License

See [License.md](https://github.com/Wagnerp/Windows-API-CodePack-NET/blob/main/LICENSE)

## More Information

Visit [GitHub Repository](https://github.com/Wagnerp/Windows-API-CodePack-NET)

