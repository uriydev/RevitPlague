using Microsoft.Extensions.DependencyInjection;
using RevitPlague.Services;
using RevitPlague.Services.Contracts;
using RevitPlague.ViewModels;
using RevitPlague.Views.Pages;
using Wpf.Ui;
using MainWindow = RevitPlague.Views.MainWindow;

namespace RevitPlague;

/// <summary>
///     Provides a host for the application's services and manages their lifetimes
/// </summary>
public static class Host
{
    private static IServiceProvider _serviceProvider;
    
    /// <summary>
    ///     Starts the host and configures the application's services
    /// </summary>
    public static void Start()
    {
        var services = new ServiceCollection();
        
        // Theme manipulation
        services.AddSingleton<IThemeService, ThemeService>();
        
        // TaskBar manipulation
        services.AddSingleton<ITaskBarService, TaskBarService>();
        
        // Service containing navigation, same as INavigationWindow... but without window
        services.AddScoped<INavigationService, NavigationService>();
        
        // Main window with navigation
        services.AddScoped<IWindow, MainWindow>();
        
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

    /// <summary>
    ///     Get service of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of service object to get</typeparam>
    /// <exception cref="System.InvalidOperationException">There is no service of type <typeparamref name="T"/></exception>
    public static T GetService<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}