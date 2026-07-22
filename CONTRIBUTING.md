# Contributing to the DISMTools repository

If you ever want to make a change to DISMTools (whether it is a bugfix or a new feature), please be sure to follow these rules:

## Prerequisites

You'll need the following to make changes:

- A GitHub account
- A fork of the repo

We'll go over each in greater detail.

### GitHub account

This is a simple step. If you don't have a GitHub account, create it. Otherwise, ignore this step.

### Forking the repository

This is a required process to make changes. I won't be accepting modifications made directly to this repo besides mine. To fork this repository, do the following:

1. Go to the [repository page](https://github.com/CodingWonders/DISMTools) and click the Fork button
2. You should copy every branch instead of just the default one (`stable`). That way, you know what branch you have to make changes to more easily. If you use GitHub Desktop, you can skip this step, as it will pick up branches from `upstream`
3. Clone your fork by using `git clone`

> [!NOTE]
> After cloning your fork and opening GitHub Desktop, the program will ask you how you want to use it. If you want to contribute to this repo, select "To contribute to the parent repository". Otherwise, choose "For my own purposes"
> 
> If you want to learn more about forks and GitHub Desktop, check out [this help page](https://docs.github.com/en/desktop/adding-and-cloning-repositories/cloning-and-forking-repositories-from-github-desktop) for more information.
>
> If you are not using GitHub Desktop to manage forks, follow [this guide](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/working-with-forks/fork-a-repo) for more information.

You're now good to start. Follow these next instructions to achieve good contributions.

## Making changes

You should know where to make changes. Any changes made in the wrong branch will cause me to change the base branch once you submit them.

### The right branch

All changes aren't performed in the `stable` branch but in a branch based on the `dt_preview` branch. This branch uses the following naming pattern:

`dt_pre_<year><month><week>`

- `<year>`: 2-digit year (eg. 24 (2024))
- `<month>`: current month (eg. 3 (March))
- `<week>`: release number of the current month (eg. 2 (second release of `<month>`))

Examples:

- `dt_pre_2441` (first release of April 2024)
- `dt_pre_24102` (second release of October 2024)

With this in mind, choose the latest `dt_pre` branch from `upstream` to copy it to your fork.

### Creating your pull request

All changes are submitted via pull requests (PRs). After making your changes, go to GitHub Desktop, select your repo, and click "Preview Pull Request":

<p align="center">
    <img src="https://github.com/CodingWonders/DISMTools/assets/101426328/724d4ea8-ccc3-4efc-aa04-1341829a6cb2" />
</p>

Then, choose the correct base branch (the latest `dt_pre` branch from `upstream`), make sure there are no conflicts present, and click "Create pull request":

<p align="center">
    <img src="https://github.com/CodingWonders/DISMTools/assets/101426328/8b3dede3-f798-4df8-9f56-112de13e1230" />
</p>

> [!NOTE]
> Don't worry about any conflicts caused by `dt_setup.exe`. This is the nightly installer that is generated every time you build the program

Next, provide a pull request title and description and, finally, click "Create pull request":

<p align="center">
    <img src="https://github.com/CodingWonders/DISMTools/assets/101426328/674279a3-c081-41dd-b58b-9f6c80df0283" />
</p>

> [!NOTE]
> When writing a description, make sure it reflects what you've done as best as you can.
>
> You can also include any form of media (like pictures or video) if they help explain the pull request better. A picture is worth a thousand words

Then, just **wait**. I'll look into your pull request more carefully and provide any feedback. Meanwhile, you can make other changes to include in your PR, change the description, or change the title.

If I approve the PR, you will see it mentioned in the release notes for the next version.

## What can I work on?

You can work on the following:

- Source code of the main program
- Source code of the update system
- Command-line (CMD & PowerShell) toolkit (found on the `Helpers` directory)

## Guidelines

- Make sure to make your pull request easy to understand by putting a descriptive title and description. I will comment on the lack of understanding of the change and suggest you improve those things
- Make sure that your pull request **works**. There are some cases in which pull requests feature broken functionality, so make sure you test it thoroughly
  
> [!NOTE]
> I will test your changes as well, and comment on any broken functionality. This will put the PR on hold until the change fully works
  
- Make sure that your pull request does not break existing functionality
- Make sure that your pull request does not impose a **security risk** or **vulnerability**. Nobody wants a repeat of the [xz incident](https://boehs.org/node/everything-i-know-about-the-xz-backdoor)
  
> [!IMPORTANT]
> I will look at the code changes, and if something isn't right security-wise, I'll comment on that. If it isn't fixed within 2 days, I'll close the PR without merging it. Everyone wants this program to always be safe to download and use

## Conclusion

If you follow all these steps and rules, you'll achieve good pull requests, so go on and make your changes!

## Installer language files

Edit installer translations in `Installer\Languages`. Do not edit `Installer\Compiler\Default.isl` for DISMTools text. That file belongs to the bundled Inno Setup compiler runtime.

## Application language files

Application translations are edited in the root `language` folder. The application discovers available languages by reading all `*.ini` files in that folder.

When adding a new language, copy an existing INI file and update `[LanguageFileInformation]` with `LanguageCode` and `LanguageName`. The file name can be chosen freely because the application identifies the language from the metadata inside the file. The application and every utility must keep using the shared `Utilities\Language\LocalizationService.vb` source file. Do not create project specific copies of the localization service.
Keep utility interface sections scoped to the utility, for example `DynaViewer.Designer.Main` or `Updater.Designer.Main`. Do not place unrelated utility windows in the main application section `Designer.Main`.

Keep section names and key names in English. Translate only the values after `=`.

PE Helper localization is exported into a generated Windows installation ISO as one minimal INI for the selected `LanguageCode`. Keep `Helpers\extps1\PE_Helper\PE_Helper.ps1` in sync with the actual localization calls in `PEHelperMainMenu`. The localization validator checks this automatically. Do not copy a complete application language file into the ISO. HotInstall uses its own language files and must not reuse the application INI files.
