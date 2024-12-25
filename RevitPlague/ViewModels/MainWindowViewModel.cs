using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RevitPlague.Views.Pages;
using Wpf.Ui.Controls;

namespace RevitPlague.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<NavigationViewItem> _navigationItems;

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter;

    public MainWindowViewModel()
    {
        InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        NavigationItems = new ObservableCollection<NavigationViewItem>
        {
            new NavigationViewItem
            {
                Content = "Home|DashboardPage",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(DashboardPage)
            },
            new NavigationViewItem
            {
                Content = "Data",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(DataPage)
            }
        };

        NavigationFooter = new ObservableCollection<object>
        {
            new NavigationViewItem
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(SettingsPage)
            }
        };
    }
}