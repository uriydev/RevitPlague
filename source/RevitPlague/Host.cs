using Microsoft.Extensions.DependencyInjection;
using RevitPlague.Core.Services;
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
        services.AddTransient<FamilyUpdaterViewModel>();
        services.AddTransient<FamilyUpdater>();
        
        services.AddSingleton<RevitApiTaskExecutor>();
        
        _serviceProvider = services.BuildServiceProvider();
    }
    
    public static T GetService<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}