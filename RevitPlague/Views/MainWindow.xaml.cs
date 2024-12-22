using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using RevitPlague.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitPlague.Views;

// public partial class MainWindow : INavigationWindow
public partial class MainWindow
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
        //
        // SetPageService(pageService);
        navigationService.SetNavigationControl(RootNavigation);
        //
    }
    
    // public INavigationView GetNavigation() => RootNavigation;

    // public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);
    

    // public void SetPageService(IPageService pageService)
    // {
    //     throw new NotImplementedException();
    // }
    public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();

    /// <summary>
    /// Raises the closed event.
    /// </summary>
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        // Make sure that closing this window will begin the process of closing the application.
        Application.Current.Shutdown();
    }

    // INavigationView INavigationWindow.GetNavigation()
    // {
    //     throw new NotImplementedException();
    // }

    // public void SetServiceProvider(IServiceProvider serviceProvider)
    // {
    //     throw new NotImplementedException();
    // }
}
