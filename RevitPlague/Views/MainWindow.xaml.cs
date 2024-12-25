using System.Windows;
using RevitPlague.Services.Contracts;
using RevitPlague.ViewModels;
using RevitPlague.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace RevitPlague.Views;

public sealed partial class MainWindow : IWindow
{
    public MainWindow(IPageService pageService, MainWindowViewModel model)
    {
        InitializeComponent();
        DataContext = this;
        
        SystemThemeWatcher.Watch(this);
        ApplicationThemeManager.Apply(this);
        
        // Навигация и установка PageService
        Loaded += (_, _) =>
        {
            if (RootNavigation == null)
                throw new InvalidOperationException("RootNavigation is not initialized.");
            
            RootNavigation.SetPageService(pageService);
            RootNavigation.Navigate(typeof(DataPage));
        };
    }
    
    protected override void OnActivated(EventArgs args)
    {
        base.OnActivated(args);
        
        UiApplication.Current.MainWindow = this;
    }
    
    protected override void OnClosed(EventArgs args)
    {
        base.OnClosed(args);
        
        UiApplication.Current.Shutdown();
    }
    
    private void ApplicationThemeManager_Changed(ApplicationTheme currentApplicationTheme, System.Windows.Media.Color systemAccent)
    {
        // Применение темы
        ApplicationThemeManager.Apply(this);
    }
}