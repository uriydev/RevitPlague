using System.Windows;
using RevitPlague.Services.Contracts;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace RevitPlague.Views;

public sealed partial class MainWindow : IWindow
{
    public MainWindow(
        INavigationService navigationService
    )
    {
        InitializeComponent();
        DataContext = this;
        
        SystemThemeWatcher.Watch(this);
        ApplicationThemeManager.Apply(this);
        
        navigationService.SetNavigationControl(RootNavigation);
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
}