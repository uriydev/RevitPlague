using RevitPlague.ViewModels;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class HomePage : INavigableView<HomeViewModel>
{
    public HomePage(HomeViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = ViewModel;
        
        ApplicationThemeManager.Apply(ApplicationTheme.Dark);
    }
    
    public HomeViewModel ViewModel { get; }
}