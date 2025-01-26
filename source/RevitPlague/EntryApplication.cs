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

namespace RevitPlague;

public class EntryApplication : IExternalApplication
{
    private readonly string jsonFilePath = @"C:\_bim\FamilyUpdaterTest\FamilyParameters_Model01.json";
    
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
        Document doc = e.GetDocument();

        // Получаем все элементы, добавленные в документ
        var addedElements = e.GetAddedElementIds();

        foreach (var elementId in addedElements)
        {
            Element element = doc.GetElement(elementId);

            // Проверяем, является ли элемент семейством
            if (element is Family family)
            {
                // Извлекаем имя семейства
                string familyName = family.Name;

                // Получаем типы этого семейства
                var familyTypes = GetFamilyTypes(doc, family);

                foreach (var familyType in familyTypes)
                {
                    // Ищем параметр customGUID в типе семейства
                    string customGUID = familyType.LookupParameter("customGUID")?.AsString();

                    if (!string.IsNullOrEmpty(customGUID))
                    {
                        SaveParameterToJson(familyName, familyType.Name, customGUID, doc.Title);
                    }
                }
            }
        }
    }

    private List<ElementType> GetFamilyTypes(Document doc, Family family)
    {
        // Собираем все типы семейства
        List<ElementType> familyTypes = new List<ElementType>();

        foreach (ElementId id in family.GetFamilySymbolIds())
        {
            ElementType type = doc.GetElement(id) as ElementType;
            if (type != null)
            {
                familyTypes.Add(type);
            }
        }

        return familyTypes;
    }

    private void SaveParameterToJson(string familyName, string typeName, string customGUID, string modelName)
    {
        // Формируем данные для записи
        var parameterData = new
        {
            FamilyName = familyName,
            TypeName = typeName,
            CustomGUID = customGUID,
            ModelName = modelName
        };

        List<object> existingData;

        // Если файл существует, читаем текущие данные
        if (File.Exists(jsonFilePath))
        {
            string existingJson = File.ReadAllText(jsonFilePath);
            existingData = JsonSerializer.Deserialize<List<object>>(existingJson) ?? new List<object>();
        }
        else
        {
            existingData = new List<object>();
        }

        // Добавляем новую запись
        existingData.Add(parameterData);

        // Настройки сериализации для читаемого текста
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        
        // Записываем данные обратно в файл JSON
        // string updatedJson = JsonSerializer.Serialize(existingData, new JsonSerializerOptions { WriteIndented = true });
        string updatedJson = JsonSerializer.Serialize(existingData, options);
        File.WriteAllText(jsonFilePath, updatedJson);
    }
}