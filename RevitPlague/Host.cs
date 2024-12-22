using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    private static IHost _host;

    /// <summary>
    ///     Starts the host and configures the application's services
    /// </summary>
    public static void Start()
    {
        var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            ContentRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly()!.Location),
            DisableDefaults = true
        });
        
        // builder.Services.AddScoped<IPageService, PageService>();
        
        // Theme manipulation
        builder.Services.AddScoped<IThemeService, ThemeService>();
        
        // TaskBar manipulation
        builder.Services.AddScoped<ITaskBarService, TaskBarService>();
        
        // Service containing navigation, same as INavigationWindow... but without window
        builder.Services.AddScoped<INavigationService, NavigationService>();
        
        // Main window with navigation
        // builder.Services.AddScoped<INavigationWindow, MainWindow>();
        // builder.Services.AddTransient<MainWindow>();
        builder.Services.AddScoped<IWindow, MainWindow>();
        builder.Services.AddScoped<MainWindowViewModel>();
        
        // Views and ViewModels
        builder.Services.AddScoped<DashboardPage>();
        builder.Services.AddScoped<DashboardViewModel>();
        builder.Services.AddScoped<DataPage>();
        builder.Services.AddScoped<DataViewModel>();
        builder.Services.AddScoped<SettingsPage>();
        builder.Services.AddScoped<SettingsViewModel>();
        
        //Startup view
        builder.Services.AddScoped<IPlagueService, PlagueService>();
        
        _host = builder.Build();
        _host.Start();
    }

    /// <summary>
    ///     Starts the host proxy and configures the application's services
    /// </summary>
    public static void StartProxy(IHost host)
    {
        _host = host;
        host.Start();
    }

    /// <summary>
    ///     Stops the host and handle <see cref="IHostedService"/> services
    /// </summary>
    public static void Stop()
    {
        _host.StopAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    ///     Get service of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of service object to get</typeparam>
    /// <exception cref="System.InvalidOperationException">There is no service of type <typeparamref name="T"/></exception>
    public static T GetService<T>() where T : class
    {
        return _host.Services.GetRequiredService<T>();
    }
}