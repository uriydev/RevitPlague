using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using WixSharp.CommonTasks;

namespace Installer;

[StructLayout(LayoutKind.Auto)]
public struct Versions
{
    public Version InstallerVersion { get; set; }
    public Version AssemblyVersion { get; set; }
    public int RevitVersion { get; set; }
}

public static class Tools
{
    public static Versions ComputeVersions(string[] args)
    {
        foreach (var directory in args)
        {
            var assemblies = Directory.GetFiles(
                directory, 
                "RevitPlague.dll", 
                SearchOption.AllDirectories);
            if (assemblies.Length == 0) continue;

            var projectAssembly = FileVersionInfo.GetVersionInfo(assemblies[0]);
            var version = new Version(projectAssembly.FileVersion).ClearRevision();
            return new Versions
            {
                AssemblyVersion = version,
                RevitVersion = version.Major,
                InstallerVersion = version.Major > 255 ? new Version(version.Major % 100, version.Minor, version.Build) : version
            };
        }

        throw new Exception("RevitPlague.dll file could not be found");
    }
}