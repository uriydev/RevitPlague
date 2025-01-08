using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitPlague.Services;
using RevitPlague.Views;

namespace RevitPlague.ViewModels;

using System.Windows.Input;

public class MainViewModel : INotifyPropertyChanged
{
    public ICommand NavigateToHomeCommand { get; }
    public ICommand NavigateToSettingsCommand { get; }

    private readonly NavigationService _navigationService;
    
    public MainViewModel(NavigationService navigationService)
    {
        ActionEventHandler = new ActionEventHandler();
        
        _navigationService = navigationService;
        NavigateToHomeCommand = new RelayCommand(NavigateToHome);
        NavigateToSettingsCommand = new RelayCommand(NavigateToSettings);
    }
    
    public ActionEventHandler ActionEventHandler { get; }
    
    private void NavigateToHome()
    {
        ActionEventHandler.Raise(application =>
        {
            _navigationService.Navigate(new HomePage()); 
        });
    }
    
    private void NavigateToSettings()
    {
        ActionEventHandler.Raise(application =>
        {
            _navigationService.Navigate(new SettingsPage());
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;
}