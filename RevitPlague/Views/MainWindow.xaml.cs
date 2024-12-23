using RevitPlague.Services.Contracts;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitPlague.Views;

public partial class MainWindow : FluentWindow, IWindow
{
    public MainWindow(
        INavigationService navigationService
    )
    {
        DataContext = this;
        // Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);
        InitializeComponent();
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