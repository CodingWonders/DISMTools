Microsoft Windows API Codepack
=========================

README
======

## Current Version



License
-------

The library is not developed anymore by Microsoft and seems to have been left as 'free to use'. A clarification or update about the licence terms from Microsoft is welcome, however.
 
Release notes
-------------

This release has the latest bug fixes applied, including the fix for 64-bit exceptions.

Bugs
----

When you submit a bug:

 - provide a short example code showing the bug
 - describe the expected behavior/result
 - if a pull request is applicable, please reference it

Pull Requests
-------------

I'll be glad to accept pull requests if they fix a bug or add a worthwhile feature.


Usage notes
-----------

**TaskDialog**

If you get the following exception when you instantiate a `TaskDialog`:

```
An unhandled exception of type 'System.NotSupportedException' occurred in Microsoft.WindowsAPICodePack.dll

Additional information: TaskDialog feature needs to load version 6 of comctl32.dll but a different version is current loaded in memory.
```

To fix it, create an application manifest and un-comment the following block section:

```
  <!-- Enable themes for Windows common controls and dialogs (Windows XP and later) -->
  <!-- <dependency>
    <dependentAssembly>
      <assemblyIdentity
          type="win32"
          name="Microsoft.Windows.Common-Controls"
          version="6.0.0.0"
          processorArchitecture="*"
          publicKeyToken="6595b64144ccf1df"
          language="*"
        />
    </dependentAssembly>
  </dependency>-->
```

Note: you might have to restart Visual Studio as the DLLs seems to be cached in memory and rebuilding your project doesn't seem to be enough in some cases.

**DirectX**

The DirectX package will work under x86 and x64 configuration platforms but not for AnyCPU platform (because there is no such platform for C++/CLI projects). Consequently, the package will purposefully fail the build and tell you why it did.

Note: package is here for historical reasons, it is highly recommended to use [SharpDX](http://sharpdx.org/) instead.