using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class DashboardPage : INavigableView<ViewModels.DashboardViewModel>
{
    public DashboardPage(ViewModels.DashboardViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
        ApplicationThemeManager.Apply(this);
        ApplicationThemeManager.Changed += ApplicationThemeManager_Changed;

    }
    
    private void ApplicationThemeManager_Changed(ApplicationTheme currentApplicationTheme,
        System.Windows.Media.Color systemAccent)
    {
        // Применение темы для страницы
        ApplicationThemeManager.Apply(this);
    }
    
    public ViewModels.DashboardViewModel ViewModel { get; }
}