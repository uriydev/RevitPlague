using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitPlague.ViewModels;
using RevitPlague.Views.Pages;
using Wpf.Ui;

namespace RevitPlague.Views;

public partial class RevitPlagueView
{
    public ICommand NavigateToHomeCommand { get; }
    public ICommand NavigateToSettingsCommand { get; }

    public RevitPlagueView(ActionEventHandler actionEventHandler, INavigationService navigationService)
    {
        InitializeComponent();
        
        ActionEventHandler = actionEventHandler;    //  DI
        navigationService.SetNavigationControl(RootNavigation);

        NavigateToHomeCommand = new RelayCommand(NavigateToHome);
        NavigateToSettingsCommand = new RelayCommand(NavigateToSettings);
    }

    public ActionEventHandler ActionEventHandler { get; }

    private void NavigateToHome()
    {
        ActionEventHandler.Raise(application =>
        {
            RootNavigation.Navigate(typeof(HomePage));
        });
    }

    private void NavigateToSettings()
    {
        ActionEventHandler.Raise(application =>
        {
            var settingsViewModel = new SettingsViewModel(ActionEventHandler);
            RootNavigation.Navigate(typeof(SettingsPage), settingsViewModel);
        });
    }
}