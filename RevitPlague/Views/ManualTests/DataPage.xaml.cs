using Wpf.Ui.Appearance;

namespace RevitPlague.Views.ManualTests;

public partial class DataPage
{
    public DataPage()
    {
        InitializeComponent();
        ApplicationThemeManager.Apply(this);
    }
}