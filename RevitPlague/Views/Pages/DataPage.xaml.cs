using RevitPlague.ViewModels;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class DataPage : INavigableView<DataViewModel>
{
    public DataPage(DataViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
        
        ApplicationThemeManager.Changed += ApplicationThemeManager_Changed;
        ApplicationThemeManager.Apply(this);
    }
    
    public DataViewModel ViewModel { get; }
    
    private void ApplicationThemeManager_Changed(ApplicationTheme currentApplicationTheme, System.Windows.Media.Color systemAccent)
    {
        ApplicationThemeManager.Apply(this);
    }
}