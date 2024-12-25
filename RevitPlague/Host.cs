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
        services.AddScoped<INavigationService, NavigationService>();
        
        // Main window with navigation
        // services.AddScoped<IWindow, MainWindow>();
        services.AddScoped<MainView>();
        services.AddScoped<MainWindowViewModel>();
        // services.AddScoped<MainWindow>();
        
        // Views and ViewModels
        services.AddTransient<DashboardPage>();
        services.AddTransient<DashboardViewModel>();
        services.AddScoped<DataPage>();
        services.AddScoped<DataViewModel>();
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