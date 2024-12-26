using Microsoft.Extensions.DependencyInjection;
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
        
        services.AddSingleton<IPageService, PageService>();
        
        // Theme manipulation
        services.AddSingleton<IThemeService, ThemeService>();
        
        // TaskBar manipulation
        services.AddSingleton<ITaskBarService, TaskBarService>();
        
        // Service containing navigation, same as INavigationWindow... but without window
        services.AddTransient<INavigationService, NavigationService>();
        
        // Main window with navigation
        services.AddTransient<RevitPlagueView>();
        services.AddTransient<MainWindowViewModel>();
        
        // Views and ViewModels
        services.AddTransient<DashboardPage>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<DataPage>();
        services.AddTransient<DataViewModel>();
        services.AddTransient<SettingsPage>();
        services.AddTransient<SettingsViewModel>();
        
        //Startup view
        services.AddTransient<IPlagueService, PlagueService>();
        
        _serviceProvider = services.BuildServiceProvider();
    }
    
    public static T GetService<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}