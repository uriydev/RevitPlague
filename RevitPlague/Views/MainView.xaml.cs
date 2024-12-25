using RevitPlague.ViewModels;
using RevitPlague.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace RevitPlague.Views;

public partial class MainView
{
    public MainView(IPageService pageService, MainWindowViewModel model)
    {
        DataContext = model;
        InitializeComponent();

        // Применение темы при создании окна
        ApplicationThemeManager.Apply(this);
        ApplicationThemeManager.Changed += ApplicationThemeManager_Changed;

        Unloaded += (s, e) =>
        {
            ApplicationThemeManager.Changed -= ApplicationThemeManager_Changed;
        };

        // Навигация и установка PageService
        Loaded += (sender, args) =>
        {
            // Проверка RootNavigation
            if (RootNavigation == null)
                throw new InvalidOperationException("RootNavigation is not initialized.");
            
            RootNavigation.SetPageService(pageService);
            RootNavigation.Navigate(typeof(DataPage));

            SystemThemeWatcher.Watch(this);
        };
    }

    private void ApplicationThemeManager_Changed(ApplicationTheme currentApplicationTheme, System.Windows.Media.Color systemAccent)
    {
        // Применение темы при изменении
        ApplicationThemeManager.Apply(this);
    }
}
