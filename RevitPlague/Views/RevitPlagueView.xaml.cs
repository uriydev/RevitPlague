using Wpf.Ui.Appearance;

namespace RevitPlague.Views;

public partial class RevitPlagueView
{
    public RevitPlagueView()
    {
        InitializeComponent();
        DataContext = this; // Устанавливаем DataContext на View

        ApplicationThemeManager.Apply(this); // Применяем тему
    }
}