# Scintilla.NET
[![.Build validate](https://github.com/desjarlais/Scintilla.NET/actions/workflows/dotnet-desktop-build.yml/badge.svg)](https://github.com/desjarlais/Scintilla.NET/actions/workflows/dotnet-desktop-build.yml) [![.NET NuGet Release](https://github.com/desjarlais/Scintilla.NET/actions/workflows/dotnet-nuget-release.yml/badge.svg)](https://github.com/desjarlais/Scintilla.NET/actions/workflows/dotnet-nuget-release.yml) [![NuGet](https://img.shields.io/nuget/v/Scintilla5.NET)](https://www.nuget.org/packages/Scintilla5.NET)

Scintilla.NET is a Windows Forms control, wrapper, and bindings for the versatile [Scintilla](http://www.scintilla.org) source code editing component.

> [!NOTE]
> I updated the Nuget package name to [Scintilla5.NET](https://www.nuget.org/packages/Scintilla5.NET) for those searching the gallery.

> "As well as features found in standard text editing components, Scintilla includes features especially useful when editing and debugging source code. These include support for syntax styling, error indicators, code completion and call tips. The selection margin can contain markers like those used in debuggers to indicate breakpoints and the current line. Styling choices are more open than with many editors, allowing the use of proportional fonts, bold and italics, multiple foreground and background colours and multiple fonts." -- [scintilla.org](http://www.scintilla.org)

Scintilla.NET can also be used with WPF using the <a href="https://msdn.microsoft.com/en-us/library/system.windows.forms.integration.windowsformshost(v=vs.110).aspx">WindowsFormsHost</a>.
For Linux, see the [Scintilla.NET.Gtk](https://github.com/VPKSoft/Scintilla.NET.Gtk).

## Utility assemblies
* [ScintillaLexers](https://github.com/desjarlais/ScintillaLexers) - A class library containing lexer definitions for the Scintilla.NET
* [VPKSoft.ScintillaUrlDetect](https://github.com/desjarlais/VPKSoft.ScintillaUrlDetect) - A library to detect URLs with the ScintillaNET control
* [VPKSoft.ScintillaSpellCheck](https://github.com/desjarlais/VPKSoft.ScintillaSpellCheck) - A spell checking library for the Scintilla.NET
* [ScintillaDiff](https://github.com/desjarlais/ScintillaDiff) - A class library for comparing two text files with the Scintilla.NET control
* [ScintillaTabbedTextControl](https://github.com/VPKSoft/ScintillaTabbedTextControl) - A tabbed control for Scintilla.NET to display multiple documents
* [ScintillaNetPrinting](https://github.com/desjarlais/ScintillaNetPrinting) - Add print functionallity to Scintilla.NET
* [AutoCompleteMenu-ScintillaNET](https://github.com/Ahmad45123/AutoCompleteMenu-ScintillaNET) - Adds an autocomplete menu to ScintillaNET that's more flexible.
* [ScintillaNET-FindReplaceDialog](https://github.com/desjarlais/ScintillaNET-FindReplaceDialog) project for adding a Find/Replace dialog to ScintillaNET (thanks to @Stumpii)

### Project Status
This project is continuing from the abandoned [ScintillaNET](https://github.com/jacobslusser/ScintillaNET) by Jacob Slusser and transferred from [VPKSoft](https://github.com/VPKSoft) who has done an incredible job of updating and extending the original project.

Scintilla.NET is in active development. If you find any issues or just have a question feel free to use the [Issues](https://github.com/desjarlais/Scintilla.NET/issues) feature at our GitHub page. **NOTE:** I don't read the issues posted to the main fork - so if your issue is about this project, post it here.

Compiled versions which are production ready can be downloaded from [NuGet](https://www.nuget.org/packages/Scintilla5.NET) or the [Releases](https://github.com/desjarlais/Scintilla.NET/releases) page.

For the latest and greatest you can build the [Master](https://github.com/desjarlais/Scintilla.NET/archive/master.zip) branch from source using Visual Studio 2022.

### Technical notes
**Versions before v.5.3.1.1:**
[Microsoft Visual C++ Redistributable for Visual Studio 2015, 2017, 2019, and 2022](https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist?view=msvc-170) is required for the component to work, see [#16](https://github.com/desjarlais/Scintilla.NET/issues/16).

Scintilla library version = 5.5.5

Lexilla library version = 5.4.3

## Background
For more information about the history and background information, look at the wiki [here](https://github.com/desjarlais/Scintilla.NET/wiki/Scintilla.NET-History).

### First Class Characters

One of the issues that ScintillaNET has historically suffered from is the fact that the native Scintilla control operates on bytes, not characters. Prior versions of ScintillaNET did not account for this, and when you're dealing with Unicode, [one byte doesn't always equal one character](http://www.joelonsoftware.com/articles/Unicode.html). The result was an API that sometimes expected byte offsets and at other times expected character offsets. Sometimes things would work as expected and other times random failures and out-of-range exceptions would occur.

No more. **One of the major focuses of this rewrite was to give ScintillaNET an understanding of Unicode from the ground up.** Every API now consistently works with character-based offsets and ranges just like .NET developers expect. Internally we maintain a mapping of character to byte offsets (and vice versa) and do all the translation for you so you never need to worry about it. No more out-of-range exceptions. No more confusion. No more pain. It just works.

### Keeping it Consistent

Another goal of the rewrite was to accept the original Scintilla API for what it is and not try to coerce it into a .NET-style API when it should not or could not be. A good example of this is how ScintillaNET uses indexers to access lines, but not treat them as a .NET collection. Lines in a Scintilla control are not items in a collection. There is no API to Add, Insert, or Remove a line in Scintilla and thus we don't try to create one in ScintillaNET. These deviations from .NET convention are rare, but are done to keep any native Scintilla documentation relevant to the managed wrapper and to avoid situations where trying to force the original API into a more familiar one is more detrimental than helpful.

*NOTE: This is not to say that ScintillaNET cannot add, insert, or remove lines. Those operations, however, are handled as text changes, not line changes.*

## Documentation

Complete API documentation is included with all of our packages. In addition there is extensive documentation at the project [Wiki](https://github.com/desjarlais/Scintilla.NET/wiki) which has recipes for common tasks and questions. If you're new to ScintillaNET, the Wiki is a good place to get started.

As previously noted in the project charter, great effort has been made to keep the ScintillaNET API consist with the native Scintilla API. As such, the [native Scintilla documentation](http://www.scintilla.org/ScintillaDoc.html) continues to be a valuable resource for learning some of the deeper features.

### Conventions

Generally speaking, their API will map to ours in the following ways:

+ A call that has an associated 'get' and 'set' such as `SCI_GETTEXT` and `SCI_SETTEXT(value)`, will map to a similarly named property such as `Text`.
+ A call that requires a number argument to access an item in a 'collection' such as `SCI_INDICSETFORE(indicatorNumber, ...)` or `SCI_STYLEGETSIZE(styleNumber, ...)`, will be accessed through an indexer such as `Indicators[0].ForeColor` or `Styles[0].Size`.

The native Scintilla control has a habit of clamping input values to within acceptable ranges rather than throwing exceptions and so we've kept that behavior in ScintillaNET. For example, the `GotoPosition` method requires a character `position` argument. If that value is less than zero or past the end of the document it will be clamped to either `0` or the `TextLength` rather than throw an `OutOfRangeException`. This tends to result in less exceptions, but the same desired outcome.
