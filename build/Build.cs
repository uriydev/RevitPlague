using System.Collections.Generic;
using System.IO;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.MSBuild;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    [Solution] readonly Solution Solution;

    string PluginName => Solution.Name;

    //  TODO: ADD CLEAN BUILD FOLDER TARGET?
    Target Compile => _ => _
        .Executes(() =>
        {
            var project = Solution.GetProject(PluginName);
            if (project == null)
                throw new FileNotFoundException("Not found!");

            var build = new List<string>();
            foreach (var (_, c) in project.Configurations)
            {
                var configuration = c.Split("|")[0];

                if (configuration.Contains("Debug") || build.Contains(configuration))
                    continue;

                Serilog.Log.Debug($"Configuration: {configuration}");

                build.Add(configuration);

                MSBuild(_ => _
                    .SetProjectFile(project.Path)
                    .SetConfiguration(configuration)
                    .SetTargets("Restore"));
                MSBuild(_ => _
                    .SetProjectFile(project.Path)
                    .SetConfiguration(configuration)
                    .SetTargets("Rebuild"));
            }
        });
}