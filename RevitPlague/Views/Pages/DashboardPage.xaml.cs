using RevitPlague.ViewModels;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class DashboardPage : INavigableView<DashboardViewModel>
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
        
        ApplicationThemeManager.Apply(this);
        ApplicationThemeManager.Changed += ApplicationThemeManager_Changed;
    }
    
    public DashboardViewModel ViewModel { get; }
    
    private void ApplicationThemeManager_Changed(
        ApplicationTheme currentApplicationTheme, 
        System.Windows.Media.Color systemAccent)
    {
        ApplicationThemeManager.Apply(this);
    }
}