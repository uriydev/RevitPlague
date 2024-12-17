using RevitPlague.Services.Contracts;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.ManualTests;

public partial class DataPage : FluentWindow, IWindow
{
    public DataPage()
    {
        InitializeComponent();
        ApplicationThemeManager.Apply(this);
    }
    
    public void Close() => base.Close();

    public void Show() => base.Show();

    public bool? ShowDialog() => base.ShowDialog();
}