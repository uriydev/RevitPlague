using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class DashboardPage : INavigableView<ViewModels.DashboardViewModel>
{
    public DashboardPage(ViewModels.DashboardViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
    }
    
    public ViewModels.DashboardViewModel ViewModel { get; }
}