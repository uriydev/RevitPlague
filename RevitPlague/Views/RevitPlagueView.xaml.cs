using RevitPlague.ViewModels;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using System.Windows;
using RevitPlague.Services.Contracts;

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