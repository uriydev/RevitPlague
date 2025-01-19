using System.Windows;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RevitPlague.ViewModels;

public partial class FamilyUpdaterViewModel : ObservableObject
{
    public FamilyUpdaterViewModel()
    {
    }
    
    [ObservableProperty]
    private Visibility _openedFolderPathVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private string _openedFolderPath = string.Empty;
    
    [RelayCommand]
    private void OnOpenFolder()
    {
        using var folderDialog = new FolderBrowserDialog();
        folderDialog.Description = "Choose a folder";
        DialogResult result = folderDialog.ShowDialog();
        if (result != DialogResult.OK || string.IsNullOrWhiteSpace(folderDialog.SelectedPath)) return;
        OpenedFolderPath = folderDialog.SelectedPath;
        
        OpenedFolderPathVisibility = Visibility.Visible;
    }
}