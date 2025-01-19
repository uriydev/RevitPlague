using RevitPlague.ViewModels;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitPlague.Views.Pages;

public partial class FamilyUpdater : INavigableView<FamilyUpdaterViewModel>
{
    public FamilyUpdater(FamilyUpdaterViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
        DataContext = ViewModel;
        
        ApplicationThemeManager.Apply(this);
    }

    public FamilyUpdaterViewModel ViewModel { get; }
}