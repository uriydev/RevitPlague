using System.Windows;
using RevitPlague.ViewModels;

namespace RevitPlague.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel mainViewModel)
    {
        DataContext = mainViewModel;
        InitializeComponent();
    }
}