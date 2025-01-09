using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using RevitPlague.Core.Services;
using RevitPlague.ViewModels;
using RevitPlague.Views.Pages;
using Wpf.Ui;

namespace RevitPlague.Views;

public partial class RevitPlagueView
{
    public ICommand NavigateToHomeCommand { get; }
    public ICommand NavigateToSettingsCommand { get; }

    // public RevitPlagueView(ActionEventHandler actionEventHandler, INavigationService navigationService)
    public RevitPlagueView(RevitApiTaskHandler actionEventHandler, INavigationService navigationService)
    {
        InitializeComponent();
        
        ActionEventHandler = actionEventHandler;    //  DI
        navigationService.SetNavigationControl(RootNavigation);

        NavigateToHomeCommand = new RelayCommand(NavigateToHome);
        NavigateToSettingsCommand = new RelayCommand(NavigateToSettings);
    }

    // public ActionEventHandler ActionEventHandler { get; }
    public RevitApiTaskHandler ActionEventHandler { get; }

    private void NavigateToHome()
    {
        // ActionEventHandler.Raise(application =>
        ActionEventHandler.Run(application =>
        {
            RootNavigation.Navigate(typeof(HomePage));
        });
    }

    private void NavigateToSettings()
    {
        // ActionEventHandler.Raise(application =>
        ActionEventHandler.Run(application =>
        {
            RootNavigation.Navigate(typeof(SettingsPage));
        });
    }
}