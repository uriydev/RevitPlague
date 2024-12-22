using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class SettingsPage : INavigableView<ViewModels.SettingsViewModel>
{
    public SettingsPage(ViewModels.SettingsViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
    }
    
    public ViewModels.SettingsViewModel ViewModel { get; }
}