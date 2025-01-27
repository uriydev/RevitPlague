using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Media.Imaging;
using RevitPlague.Services;
using RevitPlague.ViewModels;
using FamilyType = Autodesk.Revit.DB.FamilyType;

namespace RevitPlague;

public class EntryApplication : IExternalApplication
{
    private readonly string _modelJsonPath = @"C:\_bim\FamilyUpdaterTest\FamilyParameters_Model01.json";
    private readonly string _libraryJsonPath = @"C:\_bim\FamilyUpdaterTest\Library_FamilyParameters.json";
    
    public Result OnStartup(UIControlledApplication application)
    {
        Host.Start();
        #region Ribbon Panel
        
        RibbonPanel ribbonPanel = application.CreateRibbonPanel("Revit Plague");
        string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
        PushButtonData buttonData = new PushButtonData("cmdHelloWorld",
            "RevitPlague", thisAssemblyPath, "RevitPlague.Commands.PlagueViewCommand");
        PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
        pushButton.ToolTip = "Say hello to the entire world.";
        Uri uriImage = new Uri("pack://application:,,,/RevitPlague;component/Resources/icons/RibbonIcon32.png");
        BitmapImage largeImage = new BitmapImage(uriImage);
        pushButton.LargeImage = largeImage;
        
        #endregion
        application.ControlledApplication.DocumentChanged += OnDocumentChanged;
        return Result.Succeeded;
    }
    
    public Result OnShutdown(UIControlledApplication application)
    {
        application.ControlledApplication.DocumentChanged -= OnDocumentChanged;
        return Result.Succeeded;
    }
    
    private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
    {
        var doc = e.GetDocument();

        // Получаем все добавленные элементы
        foreach (var elementId in e.GetAddedElementIds())
        {
            var element = doc.GetElement(elementId);

            // Проверяем, является ли элемент семейством (Family)
            if (element is Family family)
            {
                // Проверяем, что семейство принадлежит категории Mechanical Equipment
                if (IsMechanicalEquipmentFamily(family))
                {
                    var typeNames = GetFamilyTypes(doc, family).Select(t => t.Name).ToList();

                    if (typeNames.Count > 0)
                    {
                        SaveParameterToJson(family.Name, typeNames);
                    }
                }
            }
        }
    }

    private bool IsMechanicalEquipmentFamily(Family family)
    {
        // Получаем категорию семейства
        var categoryId = family.FamilyCategory?.Id.IntegerValue;

        // Проверяем, что категория соответствует Mechanical Equipment
        return categoryId == (int)BuiltInCategory.OST_MechanicalEquipment;
    }
    
    private List<ElementType> GetFamilyTypes(Document doc, Family family)
    {
        return family.GetFamilySymbolIds()
            .Select(id => doc.GetElement(id) as ElementType)
            .Where(type => type != null)
            .ToList();
    }

    private void SaveParameterToJson(string familyName, List<string> typeNames)
    {
        try
        {
            if (!File.Exists(_libraryJsonPath))
            {
                TaskDialog.Show("Ошибка", $"Файл библиотеки '{_libraryJsonPath}' не найден.");
                return;
            }

            var libraryData = ReadJsonFile(_libraryJsonPath);
            var familyData = libraryData.FirstOrDefault(f => f.FamilyName == familyName);

            if (familyData == null)
            {
                TaskDialog.Show("Ошибка", $"Семейство '{familyName}' отсутствует в библиотеке.");
                return;
            }

            var existingData = ReadJsonFile(_modelJsonPath);
            existingData.Add(new FamilyData
            {
                FUID = familyData.FUID,
                FamilyName = familyName,
                Types = typeNames.Select(t => new FamilyType { TypeName = t }).ToList(),
                IsActual = true // Галочка актуальности
            });

            // Настройки сериализации с поддержкой кириллицы
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            File.WriteAllText(_modelJsonPath, JsonSerializer.Serialize(existingData, options));
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Ошибка", $"Не удалось сохранить данные: {ex.Message}");
        }
    }

    private List<FamilyData> ReadJsonFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                // Создаем пустой файл, если он отсутствует
                File.WriteAllText(filePath, "[]");
                return new List<FamilyData>();
            }

            var jsonContent = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<FamilyData>>(jsonContent) ?? new List<FamilyData>();
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Ошибка", $"Не удалось прочитать файл {filePath}: {ex.Message}");
            return new List<FamilyData>();
        }
    }

    private class FamilyData
    {
        public string FUID { get; set; }
        public string FamilyName { get; set; }
        public List<FamilyType> Types { get; set; } = new();
        public bool IsActual { get; set; } // Галочка актуальности
    }

    private class FamilyType
    {
        public string TypeName { get; set; }
    }
}