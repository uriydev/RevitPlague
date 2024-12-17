using RevitPlague.Services.Contracts;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views;

public partial class RevitPlagueView : FluentWindow, IWindow
{
    public RevitPlagueView()
    {
        InitializeComponent();
        ApplicationThemeManager.Apply(this);
    }
        
    public void Close() => base.Close();

    public void Show() => base.Show();

    public bool? ShowDialog() => base.ShowDialog();
}