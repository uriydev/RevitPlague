using Microsoft.Extensions.DependencyInjection;
using RevitPlague.ViewModels;
using RevitPlague.Views;

namespace RevitPlague;

public static class Host
{
    private static IServiceProvider _serviceProvider;
    
    public static void Start()
    {
        var services = new ServiceCollection();
        
        // var view = new MainWindow().ShowDialog();
        
        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();
        
        services.AddTransient<SettingsPage>();
        services.AddTransient<SettingsViewModel>();
        
        _serviceProvider = services.BuildServiceProvider();
    }
    
    public static T GetService<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}