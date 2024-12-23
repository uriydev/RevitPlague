using RevitPlague.Services.Contracts;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views;

public partial class MainWindow : FluentWindow, IWindow
{
    public MainWindow(
        INavigationService navigationService
    )
    {
        DataContext = this;
        
        InitializeComponent();
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