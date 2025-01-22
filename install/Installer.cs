using System;
using System.Collections.Generic;
using Installer;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

const string outputName = "RevitPlague";
const string projectName = "RevitPlague";
const string filesToCopy = @"C:\_code\Repos\RevitPlague\source\RevitPlague\bin\Debug R24\publish\Revit 2024 Debug R24 addin";

var guidMap = new Dictionary<int, string>
{
    { 2024, "51e0bf0e-1905-4a8d-8a21-fca3834e7738" },
};

var project = new Project
{
    OutDir = "output",
    Name = projectName,
    GUID = new Guid(guidMap[2024]),
    Platform = Platform.x64,
    UI = WUI.WixUI_InstallDir,
    // Version = versions.InstallerVersion,
    MajorUpgrade = MajorUpgrade.Default,
    // BackgroundImage = @"install\Resources\Icons\BackgroundImage.png",
    // BannerImage = @"install\Resources\Icons\BannerImage.png",
    ControlPanelInfo =
    {
        Manufacturer = "uriydev",
        HelpLink = "https://github.com/jeremytammik/RevitLookup/issues",
        // ProductIcon = @"install\Resources\Icons\ShellIcon.ico"
    }
};

// var wixEntities = Generator.GenerateWixEntities(args, versions.AssemblyVersion);
var wixEntities = Generator.GenerateWixEntities(new [] {filesToCopy});

project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);

//3. executing the build script
BuildSingleUserMsi();
// BuildMultiUserUserMsi();

void BuildSingleUserMsi()
{
    project.InstallScope = InstallScope.perUser;
    // project.OutFileName = $"{outputName}-{versions.AssemblyVersion}-SingleUser";
    project.OutFileName = $"{outputName}-SingleUser";
    project.Dirs =
    [
        // new InstallDir($@"%AppDataFolder%\Autodesk\Revit\Addins\{RevitVersion}", wixEntities)
        new InstallDir($@"%CommonAppDataFolder%\Autodesk\Revit\Addins\2024", wixEntities)
    ];
    project.BuildMsi();
}

void BuildMultiUserUserMsi()
{
    project.InstallScope = InstallScope.perMachine;
    // project.OutFileName = $"{outputName}-{versions.AssemblyVersion}-MultiUser";
    project.OutFileName = $"{outputName}-MultiUser";
    project.Dirs =
    [
        // new InstallDir($@"%CommonAppDataFolder%\Autodesk\Revit\Addins\{versions.RevitVersion}", wixEntities)
        new InstallDir($@"%CommonAppDataFolder%\Autodesk\Revit\Addins\2024)", wixEntities)
    ];
    project.BuildMsi();
}