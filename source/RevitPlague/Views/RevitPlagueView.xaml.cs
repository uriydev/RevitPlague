using System.Windows.Interop;
using RevitPlague.Views.Pages;
using Wpf.Ui;

namespace RevitPlague.Views;

public partial class RevitPlagueView
{
    public RevitPlagueView(INavigationService navigationService)
    {
        InitializeComponent();
        navigationService.SetNavigationControl(RootNavigation);
        Loaded += (_, _) => RootNavigation.Navigate(typeof(HomePage));
    }
    
    // Перегруженный метод Show
    public void Show(IntPtr ownerHandle)
    {
        // Привязываем окно WPF к окну Revit
        new WindowInteropHelper(this).Owner = ownerHandle;

        // Показываем окно
        base.Show();
    }
}