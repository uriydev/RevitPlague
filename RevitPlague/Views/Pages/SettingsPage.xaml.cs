using RevitPlague.ViewModels;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class SettingsPage : INavigableView<SettingsViewModel>
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = ViewModel;

        ApplicationThemeManager.Apply(this);
    }

    public SettingsViewModel ViewModel { get; }
}