using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class SettingsPage : INavigableView<ViewModels.SettingsViewModel>
{
    public SettingsPage(ViewModels.SettingsViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;

        // Подписка на событие изменения темы
        ApplicationThemeManager.Changed += ApplicationThemeManager_Changed;
    }

    public ViewModels.SettingsViewModel ViewModel { get; }

    private void ApplicationThemeManager_Changed(ApplicationTheme currentApplicationTheme,
        System.Windows.Media.Color systemAccent)
    {
        // Применение темы для страницы
        ApplicationThemeManager.Apply(this);
    }

    // Обработчик для отписки от события при закрытии страницы
    // protected override void OnClosed(EventArgs e)
    // {
    //     // Отписываемся от события при закрытии страницы
    //     ApplicationThemeManager.Changed -= ApplicationThemeManager_Changed;
    //     base.OnClosed(e);
    // }
}