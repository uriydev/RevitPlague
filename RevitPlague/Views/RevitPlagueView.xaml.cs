using RevitPlague.Services.Contracts;
using RevitPlague.ViewModels;
using RevitPlague.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace RevitPlague.Views;

public partial class RevitPlagueView : IWindow, IDisposable
{
    public RevitPlagueView(MainWindowViewModel viewModel, IPageService pageService)
    {
        // ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
        
        RootNavigation.SetPageService(pageService);
        ApplyTheme();
        ApplicationThemeManager.Changed += ApplicationThemeManager_Changed;
    }
    
    public MainWindowViewModel ViewModel { get; }

    private void ApplyTheme()
    {
        ApplicationThemeManager.Apply(this);
    }
    
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