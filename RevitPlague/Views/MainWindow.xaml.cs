using RevitPlague.Services;
using RevitPlague.ViewModels;

namespace RevitPlague.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        var navigationService = new NavigationService(MainFrame);
        DataContext = new MainViewModel(navigationService);
    }
}