# Managed DismApi Wrapper
[![Build Status](https://dev.azure.com/jeffkl/Public/_apis/build/status/ManagedDism?branchName=main)](https://dev.azure.com/jeffkl/Public/_build/latest?definitionId=15&branchName=main)
[![Microsoft.Dism](https://img.shields.io/nuget/v/Microsoft.Dism.svg?maxAge=2592000)](https://www.nuget.org/packages/Microsoft.Dism)
[![NuGet](https://img.shields.io/nuget/dt/Microsoft.Dism.svg)](https://www.nuget.org/packages/Microsoft.Dism)


This is a managed wrapper for the native [Deployment Image Servicing and Management (DISM) API]([url](https://learn.microsoft.com/windows-hardware/manufacture/desktop/dism/deployment-image-servicing-and-management--dism--api?view=windows-11)). 

This assembly allows .NET developers to call directly into the DismApi without having to shell out to Dism.exe. This allows for better integration and richer errors. 

## [Runtime Requirements](http://msdn.microsoft.com/en-us/library/windows/desktop/hh825837.aspx)

The DISM API files are available in all Windows® 8 operating systems. You can also run DISM API applications on any Windows operating system that is supported by the Windows ADK. You must install the Windows ADK in order to run DISM API applications on these operating systems. For more information about supported operating systems, see the [Windows® Assessment and Deployment Kit (Windows ADK) Technical Reference](http://go.microsoft.com/fwlink/?LinkId=206587).

## Getting Started

Install the [NuGet package](https://www.nuget.org/packages/Microsoft.Dism/):

```
<ItemGroup>
  <PackageReference Include="Microsoft.Dism" Version="<version>" />
</ItemGroup>
```

The wrapper is as close to the native DismApi as possible. Most methods in the DismApi class should mirror the functions in the DismApi. Managed classes are used in place of the native structs and I tried to keep the property names the same. I also included XML doc comments on every class, method, and property which should help with development.

## Initializing DISMAPI

You'll need to initialize the DismApi first (don't forget to call Shutdown() at the end of your program)

``` C#
DismApi.Initialize(DismLogLevel.LogErrors);

DismApi.Shutdown();
```

## List Images

An example on how to list the images contained in a WIM file:

``` C#
// Path to the WIM file
string imagePath = @"C:\image.wim";

// Initialize the DismApi
DismApi.Initialize(DismLogLevel.LogErrors);

try
{
	// Get the images in the WIM
	DismImageInfoCollection imageInfos = DismApi.GetImageInfo(imagePath);

	// Print the image path and image count
	Console.WriteLine("Image {0} contains {1} image(s)", imagePath, imageInfos.Count);

	// Loop through each image
	foreach(DismImageInfo imageInfo in imageInfos)
	{
		// Print the image index and name
		Console.WriteLine("Image Index: {0}", imageInfo.ImageIndex);
		Console.WriteLine("Image Name: {0}", imageInfo.ImageName);
		Console.WriteLine("------------------------");
	}
}
finally
{
	// Shut down the DismApi
	DismApi.Shutdown();
}
```

## Mounting Images

An example on how to mount an image, list the features, and unmount it:

``` C#
string imagePath = @"C:\image.wim";
string mountPath = @"C:\temp\mount";
int imageIndex = 1;

// Initialize the DismApi
DismApi.Initialize(DismLogLevel.LogErrors);

try
{
	// Create the mount dir if it doesn't exit
	if(Directory.Exists(mountPath) == false)
	{
		Directory.Create(mountPath);
	}

	// Mount the image
	DismApi.MountImage(imagePath, mountPath, imageIndex);

	// Open a session to the mounted image
	using(DismSession session = DismApi.OpenOfflineSession(mountPath))
	{
		// Get the features of the image
		DismFeatureCollection features = DismApi.GetFeatures(session);

		// Loop through the features
		foreach(DismFeature feature in features)
		{
			// Print the feature name
			Console.WriteLine("Feature: {0}", feature.FeatureName);
		}
	}

	// Unmount the image and discard changes
	DismApi.UnmountImage(mountPath, false);
}
finally
{
	// Shut down the DismApi
	DismApi.Shutdown();
}
```

## Using DismProgressCallback
[DismProgressCallback](https://github.com/josemesona/ManagedDism/blob/master/src/Microsoft.Dism/DismProgressCallback.cs#L9) is a delegate that receives progress updates during certain operations.
``` C#
static void Main(string[] args)
{
    DismApi.Initialize(DismLogLevel.LogErrors);
    try
    {
        using (DismSession session = DismApi.OpenOnlineSession())
        {
            List<string> sourcePaths = new List<string>();

            DismApi.AddCapability(
                session,
                capabilityName: "Capability",
                limitAccess: false,
                sourcePaths: new List<string>(),
                progressCallback: HandleProgress,
                userData: null);
        }
    }
    finally
    {
        DismApi.Shutdown();
    }
            
}

private static void HandleProgress(DismProgress progress)
{
    // Print the current progress
    //
    Console.WriteLine("Current: {0}", progress.Current);
    Console.WriteLine("Total: {0}", progress.Total);

    if (progress.Current == 50)
    {
        // Cancel the operation at 50%
        //
        progress.Cancel = true;
    }
}
```

## Contributing
Please read the [Contributing doc](CONTRIBUTING.md) for information on building the code and contributing to the project.
