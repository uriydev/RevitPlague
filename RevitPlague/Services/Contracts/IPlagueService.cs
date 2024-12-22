using System.Windows.Controls;

namespace RevitPlague.Services.Contracts;

public interface IPlagueService : IPlagueServiceDependsStage, IPlagueServiceShowStage, IPlagueServiceExecuteStage
{
    new IPlagueServiceShowStage DependsOn(IServiceProvider provider);
    new IPlagueServiceExecuteStage Show<T>() where T : Page;
}

public interface IPlagueServiceDependsStage
{
    IPlagueServiceShowStage DependsOn(IServiceProvider provider);
    IPlagueServiceExecuteStage Show<T>() where T : Page;
}

public interface IPlagueServiceShowStage
{
    IPlagueServiceExecuteStage Show<T>() where T : Page;
}

public interface IPlagueServiceExecuteStage
{
    void Execute<T>(Action<T> handler) where T : class;
}