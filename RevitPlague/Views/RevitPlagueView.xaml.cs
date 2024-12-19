using Wpf.Ui.Appearance;

namespace RevitPlague.Views;

public partial class RevitPlagueView
{
    public RevitPlagueView()
    {
        InitializeComponent();
        ApplicationThemeManager.Apply(this);
    }
}