using Microsoft.Extensions.DependencyInjection;
using RevitPlague.Core.Services;
using RevitPlague.Services;
using RevitPlague.Services.Contracts;
using RevitPlague.ViewModels;
using RevitPlague.Views;
using RevitPlague.Views.Pages;
using Wpf.Ui;

namespace RevitPlague;

public static class Host
{
    private static IServiceProvider _serviceProvider;
    
    public static void Start()
    {
        var services = new ServiceCollection();
        
        services.AddTransient<INavigationService, NavigationService>();
        
        services.AddTransient<SettingsPage>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<HomePage>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<RevitPlagueView>();
        services.AddSingleton<RevitApiTaskExecutor>();
        
        //
        services.AddTransient<FamilyUpdaterViewModel>();
        services.AddTransient<FamilyUpdater>();
        //
        services.AddTransient<IFamilyLoaderServiceFactory, FamilyLoaderServiceFactory>();
        services.AddTransient<FamilyChecker>();
        //
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IFuidService, FuidService>();
        // services.AddTransient<IFamilyParameterService, FamilyParameterService>();
        services.AddTransient<IFamilyParameterServiceFactory, FamilyParameterServiceFactory>();
        services.AddTransient<LibraryFamilyUpdater>();
        
        var appSettings = new AppSettings
        {
            FamilyParametersFilePath = @"C:\_bim\FamilyUpdaterTest\Library_FamilyParameters.json"
        };
        services.AddSingleton(appSettings);
        
        _serviceProvider = services.BuildServiceProvider();
    }
    
    public static T GetService<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}