using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.MSBuild;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Solution] readonly Solution Solution;

    string PluginName => Solution.Name;

    Target Compile => _ => _
        .Executes(() =>
        {
            var project = Solution.GetProject(PluginName);
            if (project == null)
                throw new FileNotFoundException("Project not found!");

            var buildConfigurations = new List<string>();
            foreach (var (_, configuration) in project.Configurations)
            {
                var configName = configuration.Split("|")[0];
                if (configName.Contains("Debug") || buildConfigurations.Contains(configName))
                    continue;

                Serilog.Log.Debug($"Configuration: {configName}");
                buildConfigurations.Add(configName);

                MSBuild(_ => _
                    .SetProjectFile(project.Path)
                    .SetConfiguration(configName)
                    .SetTargets("Restore"));

                MSBuild(_ => _
                    .SetProjectFile(project.Path)
                    .SetConfiguration(configName)
                    .SetTargets("Rebuild"));
            }
        });

Target BuildInstaller => _ => _
    // .DependsOn(Compile)
    .Executes(() =>
    {
        var baseDirectory = RootDirectory / "source" / "RevitPlague" / "bin";
        Serilog.Log.Information($"Base directory: {baseDirectory}");

        var allDirectories = Directory.GetDirectories(baseDirectory, "*", SearchOption.AllDirectories);
        var releaseDirectories = allDirectories
            .Where(dir => dir.Contains("Release") && dir.Contains("publish") && dir.Contains("Revit"))
            .ToList();

        if (releaseDirectories.Count == 0)
        {
            Serilog.Log.Error("No valid release directories found.");
            throw new DirectoryNotFoundException("No valid release directories found.");
        }

        foreach (var dir in releaseDirectories)
        {
            Serilog.Log.Information($"Valid release directory: {dir}");
        }

        var args = string.Join(" ", releaseDirectories.Select(x => $"\"{x}\""));
        var installerPath = RootDirectory / "install" / "bin" / "Installer.exe"; // Обновлённый путь
        Serilog.Log.Information($"Installer path: {installerPath}");

        if (!File.Exists(installerPath))
        {
            Serilog.Log.Error($"Installer.exe not found at {installerPath}");
            throw new FileNotFoundException($"Installer.exe not found at {installerPath}");
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = installerPath,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        var process = Process.Start(startInfo)!;
        process.OutputDataReceived += (sender, e) => Serilog.Log.Information(e.Data);
        process.ErrorDataReceived += (sender, e) => Serilog.Log.Error(e.Data);

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Installer failed with exit code {process.ExitCode}");
        }
    });
}