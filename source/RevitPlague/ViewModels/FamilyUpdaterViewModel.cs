using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RevitPlague.Core.Services;
using RevitPlague.Services;
using RevitPlague.Services.Contracts;
using Visibility = System.Windows.Visibility;

namespace RevitPlague.ViewModels;

public partial class FamilyUpdaterViewModel : ObservableObject
{
    private readonly RevitApiTaskExecutor _revitApiTaskExecutor;
    private readonly IFamilyLoaderServiceFactory _familyLoaderServiceFactory;
    private readonly FamilyChecker _familyChecker;
    
    public FamilyUpdaterViewModel(
        RevitApiTaskExecutor revitApiTaskExecutor, 
        IFamilyLoaderServiceFactory familyLoaderServiceFactory)
    {
        _revitApiTaskExecutor = revitApiTaskExecutor;
        _familyLoaderServiceFactory = familyLoaderServiceFactory;
        _familyChecker = new FamilyChecker();
    }
    
    [ObservableProperty]
    private Visibility _openedFolderPathVisibility = Visibility.Collapsed;
    
    [ObservableProperty]
    private Visibility _openedFilePathVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private string _openedFolderPath = string.Empty;
    
    [ObservableProperty]
    private string _openedFilePath = string.Empty;
    
    [RelayCommand]
    private void OpenFolder()
    {
        using var folderDialog = new FolderBrowserDialog();
        folderDialog.Description = "Choose a folder";
        DialogResult result = folderDialog.ShowDialog();
        if (result != DialogResult.OK || string.IsNullOrWhiteSpace(folderDialog.SelectedPath)) return;
        OpenedFolderPath = folderDialog.SelectedPath;
        
        OpenedFolderPathVisibility = Visibility.Visible;
    }
    
    [RelayCommand]
    private void OpenFile()
    {
        using var fileDialog = new OpenFileDialog();
        fileDialog.Title = "Choose a file";
        fileDialog.Filter = "All Files (*.*)|*.*";
    
        DialogResult result = fileDialog.ShowDialog();
        if (result != DialogResult.OK || string.IsNullOrWhiteSpace(fileDialog.FileName)) return;
    
        OpenedFilePath = fileDialog.FileName;
        OpenedFilePathVisibility = Visibility.Visible;
    
        LoadFamily(OpenedFilePath);
    }
    
    [RelayCommand]
    private void LoadFamily(string filePath)
    {
        _revitApiTaskExecutor.ExecuteAsync(app =>
        {
            UIDocument uiDocument = app.ActiveUIDocument;
            Document document = uiDocument.Document;

            // Загружаем семейство
            IFamilyLoaderService familyLoaderService = _familyLoaderServiceFactory.Create(document);
            familyLoaderService.LoadFamily(filePath);

            // Проверка актуальности семейства
            FamilyChecker familyChecker = new FamilyChecker();
            familyChecker.CheckFamilyParameters();

            // Используем TaskDialog для вывода результата
            TaskDialog.Show("Проверка семейств", "Проверка завершена. Результаты сохранены в лог.");
        });
    }
}