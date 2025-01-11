using System.Windows.Input;
using System.Windows.Interop;
using CommunityToolkit.Mvvm.Input;
using RevitPlague.Core.Services;
using RevitPlague.Views.Pages;
using Wpf.Ui;

namespace RevitPlague.Views;

public partial class RevitPlagueView
{
    public ICommand NavigateToHomeCommand { get; }
    public ICommand NavigateToSettingsCommand { get; }

    public RevitPlagueView(RevitApiTaskHandler revitApiTaskHandler, INavigationService navigationService)
    {
        InitializeComponent();
        
        RevitApiTaskHandler = revitApiTaskHandler;
        navigationService.SetNavigationControl(RootNavigation);

        NavigateToHomeCommand = new RelayCommand(NavigateToHome);
        NavigateToSettingsCommand = new RelayCommand(NavigateToSettings);
        
        Loaded += (_, _) => RootNavigation.Navigate(typeof(HomePage));
    }

    public RevitApiTaskHandler RevitApiTaskHandler { get; }

    private void NavigateToHome()
    {
        RevitApiTaskHandler.Run(application =>
        {
            RootNavigation.Navigate(typeof(HomePage));
        });
    }

    private void NavigateToSettings()
    {
        RevitApiTaskHandler.Run(application =>
        {
            RootNavigation.Navigate(typeof(SettingsPage));
        });
    }
    
    // Перегруженный метод Show
    public void Show(IntPtr ownerHandle)
    {
        // Привязываем окно WPF к окну Revit
        new WindowInteropHelper(this).Owner = ownerHandle;

        // Показываем окно
        base.Show();
    }
}