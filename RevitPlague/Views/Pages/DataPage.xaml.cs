using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class DataPage : INavigableView<ViewModels.DataViewModel>
{
    public DataPage(ViewModels.DataViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        ApplicationThemeManager.Changed += ApplicationThemeManager_Changed;
        ApplicationThemeManager.Apply(this);
        
        // Устанавливаем DataContext в ViewModel
        DataContext = ViewModel;
    }
    
    public ViewModels.DataViewModel ViewModel { get; }
    
    private void ApplicationThemeManager_Changed(ApplicationTheme currentApplicationTheme, System.Windows.Media.Color systemAccent)
    {
        // Применение темы для страницы
        ApplicationThemeManager.Apply(this);
    }
}