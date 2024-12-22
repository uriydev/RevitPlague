using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using RevitPlague.Services.Contracts;
using RevitPlague.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitPlague.Views;

// public partial class MainWindow : INavigationWindow
// public partial class MainWindow
public partial class MainWindow : FluentWindow, IWindow
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow(
        MainWindowViewModel viewModel,
        // IPageService pageService,
        INavigationService navigationService
    )
    {
        ViewModel = viewModel;
        DataContext = this;
        // Appearance.SystemThemeWatcher.Watch(this);
        InitializeComponent();
        // SetPageService(pageService);
        navigationService.SetNavigationControl(RootNavigation);
    }
    
    // public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);
    
    protected override void OnActivated(EventArgs args)
    {
        base.OnActivated(args);
        // Wpf.Ui.Application.MainWindow = this; // N3P
        UiApplication.Current.MainWindow = this;
    }
    
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        // Application.Current.Shutdown();
        UiApplication.Current.Shutdown();
        // Wpf.Ui.Application.Windows.Remove(this); // N3P
    }

    /* PAST
    public INavigationView GetNavigation() => RootNavigation;
    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);
    public void SetPageService(IPageService pageService)
    {
        throw new NotImplementedException();
    }
    
    INavigationView INavigationWindow.GetNavigation()
    {
        throw new NotImplementedException();
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
    */
}