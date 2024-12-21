using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class DataPage : INavigableView<ViewModels.DataViewModel>
{
    public ViewModels.DataViewModel ViewModel { get; }

    public DataPage(ViewModels.DataViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}