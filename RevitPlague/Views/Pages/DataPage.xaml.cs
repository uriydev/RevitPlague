using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class DataPage : INavigableView<ViewModels.DataViewModel>
{
    public DataPage(ViewModels.DataViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = this;
    }
    
    public ViewModels.DataViewModel ViewModel { get; }
}