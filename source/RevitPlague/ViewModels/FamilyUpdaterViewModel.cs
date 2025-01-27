using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
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
    private const string ModelJsonPath = @"C:\_bim\FamilyUpdaterTest\FamilyParameters_Model01.json";

    private readonly RevitApiTaskExecutor _revitApiTaskExecutor;
    private readonly IFamilyLoaderServiceFactory _familyLoaderServiceFactory;
    private readonly FamilyChecker _familyChecker;

    public FamilyUpdaterViewModel(
        RevitApiTaskExecutor revitApiTaskExecutor,
        IFamilyLoaderServiceFactory familyLoaderServiceFactory,
        FamilyChecker familyChecker)
    {
        _revitApiTaskExecutor = revitApiTaskExecutor;
        _familyLoaderServiceFactory = familyLoaderServiceFactory;
        _familyChecker = familyChecker;
    }

    [ObservableProperty]
    private Visibility _openedFilePathVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private string _openedFilePath = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Product> _productsCollection = new();

    [RelayCommand]
    private void LoadFamily(string filePath)
    {
        _revitApiTaskExecutor.ExecuteAsync(app =>
        {
            var document = app.ActiveUIDocument.Document;

            // Загружаем семейство
            var familyLoaderService = _familyLoaderServiceFactory.Create(document);
            familyLoaderService.LoadFamily(filePath);
        });
    }

    [RelayCommand]
    private void CheckFamilyLibrary()
    {
        _revitApiTaskExecutor.ExecuteAsync(app =>
        {
            Document document = app.ActiveUIDocument.Document;
            var familyDataList = _familyChecker.CheckFamilyParameters(document);

            if (familyDataList == null)
            {
                TaskDialog.Show("Ошибка", "Не удалось проверить актуальность семейств.");
                return;
            }

            UpdateProductsCollection(familyDataList);
            TaskDialog.Show("Проверка семейств", "Проверка завершена.");
        });
    }

    [RelayCommand]
    private void LoadDataFromJson()
    {
        try
        {
            if (!File.Exists(ModelJsonPath))
            {
                TaskDialog.Show("Ошибка", "Файл не найден.");
                return;
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var familyDataList = JsonSerializer.Deserialize<List<FamilyData>>(File.ReadAllText(ModelJsonPath), options);

            if (familyDataList == null)
            {
                TaskDialog.Show("Ошибка", "Не удалось загрузить данные из файла.");
                return;
            }

            UpdateProductsCollection(familyDataList);
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Ошибка", $"Не удалось загрузить данные: {ex.Message}");
        }
    }

    private void UpdateProductsCollection(List<FamilyData> familyDataList)
    {
        ProductsCollection = new ObservableCollection<Product>(
            familyDataList.Select(f => new Product
            {
                FamilyName = f.FamilyName,
                FUID = f.FUID,
                Types = string.Join(", ", f.Types.Select(t => t.TypeName)),
                IsActual = f.IsActual
            }).ToList()
        );
    }
}

public class Product
{
    public string FamilyName { get; set; }
    public string FUID { get; set; }
    public string Types { get; set; } // Типы семейства (объединены в строку)
    public bool IsActual { get; set; } // Галочка актуальности
}

public class FamilyData
{
    public string FUID { get; set; }
    public string FamilyName { get; set; }
    public List<FamilyType> Types { get; set; } = new();
    public bool IsActual { get; set; } // Галочка актуальности
}

public class FamilyType
{
    public string TypeName { get; set; }
}