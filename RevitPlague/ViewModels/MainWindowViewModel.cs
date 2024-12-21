using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RevitPlague.Views.Pages;
using Wpf.Ui.Controls;

namespace RevitPlague.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<NavigationViewItem> _navigationItems;
    
    public MainWindowViewModel()
    {
        NavigationItems = new ObservableCollection<NavigationViewItem>
        {
            new NavigationViewItem
            {
                Content = "Data",
                TargetPageType = typeof(DataPage)
            },
            new NavigationViewItem
            {
                Content = "Settings",
                TargetPageType = typeof(SettingsPage)
            }
        };
    }
}