using System.Windows.Controls;
using RevitPlague.ViewModels;

namespace RevitPlague.Views;

public partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
    }
}