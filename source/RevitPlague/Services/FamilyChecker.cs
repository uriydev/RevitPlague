using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlague.ViewModels;
using FamilyType = RevitPlague.ViewModels.FamilyType;

namespace RevitPlague.Services;

public class FamilyChecker
{
    private readonly string _modelJsonPath = @"C:\_bim\FamilyUpdaterTest\FamilyParameters_Model01.json";
    private readonly string _libraryJsonPath = @"C:\_bim\FamilyUpdaterTest\Library_FamilyParameters.json";
    
    public List<FamilyData> CheckFamilyParameters(Document document)
{
    try
    {
        // Создаем файл _modelJsonPath, если он отсутствует
        if (!File.Exists(_modelJsonPath))
        {
            File.WriteAllText(_modelJsonPath, "[]");
        }

        // Читаем данные из библиотеки
        var libraryData = ReadJsonFile(_libraryJsonPath);

        if (libraryData == null)
        {
            TaskDialog.Show("Ошибка", "Не удалось прочитать данные из библиотеки.");
            return new List<FamilyData>();
        }

        // Получаем все семейства в документе
        var families = new FilteredElementCollector(document)
            .OfClass(typeof(Family))
            .Cast<Family>()
            .ToList();

        var modelData = new List<FamilyData>();

        // Проверяем каждое семейство
        foreach (var family in families)
        {
            var familyTypes = GetFamilyTypes(document, family);

            if (familyTypes.Count == 0)
            {
                continue; // Пропускаем семейства без типоразмеров
            }

            // Получаем FUID из первого типоразмера (он одинаковый для всех типоразмеров)
            var firstType = familyTypes.First();
            var fuidParameter = firstType.LookupParameter("FUID");

            if (fuidParameter == null || fuidParameter.StorageType != StorageType.String)
            {
                continue; // Пропускаем семейства без параметра FUID
            }

            string fuid = fuidParameter.AsString();

            // Проверяем, есть ли FUID в библиотеке
            var libraryFamily = libraryData.FirstOrDefault(f => f.FUID == fuid);
            bool isActual = libraryFamily != null;

            // Добавляем данные в список
            modelData.Add(new FamilyData
            {
                FUID = fuid,
                FamilyName = family.Name,
                Types = familyTypes.Select(t => new FamilyType { TypeName = t.Name }).ToList(),
                IsActual = isActual
            });
        }

        // Сохраняем обновленные данные
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        File.WriteAllText(_modelJsonPath, JsonSerializer.Serialize(modelData, options));

        return modelData;
    }
    catch (Exception ex)
    {
        TaskDialog.Show("Ошибка", $"Ошибка при проверке актуальности семейств: {ex.Message}");
        return new List<FamilyData>();
    }
}
    
    private List<FamilyData> ReadJsonFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
                return new List<FamilyData>();
            }

            var jsonContent = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<FamilyData>>(jsonContent) ?? new List<FamilyData>();
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Ошибка", $"Не удалось прочитать файл {filePath}: {ex.Message}");
            return null;
        }
    }
    
    private List<ElementType> GetFamilyTypes(Document doc, Family family)
    {
        return family.GetFamilySymbolIds()
            .Select(id => doc.GetElement(id) as ElementType)
            .Where(type => type != null)
            .ToList();
    }
}