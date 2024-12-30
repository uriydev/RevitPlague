using RevitPlague.ViewModels;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class DataPage : INavigableView<DataViewModel>
{
    public DataPage(DataViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
        
        ApplicationThemeManager.Apply(this);
    }
    
    public DataViewModel ViewModel { get; }
}