using RevitPlague.ViewModels;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class SettingsPage : INavigableView<SettingsViewModel>, IDisposable
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
        
        ApplicationThemeManager.Apply(this);
        ApplicationThemeManager.Changed += ApplicationThemeManager_Changed;
    }

    public SettingsViewModel ViewModel { get; }

    private void ApplicationThemeManager_Changed(ApplicationTheme currentApplicationTheme,
        System.Windows.Media.Color systemAccent)
    {
        ApplicationThemeManager.Apply(this);
    }

    public void Dispose()
    {
        ApplicationThemeManager.Changed -= ApplicationThemeManager_Changed;
    }
}