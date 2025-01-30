using System.IO;
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
            // Проверяем существование файла _modelJsonPath, создаем его, если отсутствует
            if (!File.Exists(_modelJsonPath))
            {
                File.WriteAllText(_modelJsonPath, "[]");
            }

            // Читаем данные из библиотеки и модели
            var libraryData = ReadJsonFile(_libraryJsonPath);
            var modelData = ReadJsonFile(_modelJsonPath);

            if (libraryData == null || modelData == null)
            {
                // TaskDialog.Show("Ошибка", "Не удалось прочитать данные из библиотеки или модели.");
                return new List<FamilyData>();
            }

            // Получаем все семейства в документе
            var families = new FilteredElementCollector(document)
                .OfClass(typeof(Family))
                .Cast<Family>()
                .ToList();

            var resultData = new List<FamilyData>();

            // Проверяем каждое семейство
            foreach (var family in families)
            {
                var familyName = family.Name;

                // Ищем семейство в библиотеке
                var libraryFamily = libraryData.FirstOrDefault(f => f.FamilyName == familyName);

                if (libraryFamily == null)
                {
                    // TaskDialog.Show("Предупреждение", $"Семейство '{familyName}' отсутствует в библиотеке.");
                    resultData.Add(new FamilyData
                    {
                        FamilyName = familyName,
                        IsActual = false,
                        FUID = null // FUID = null, если семейство отсутствует в библиотеке
                    });
                    continue;
                }

                // Ищем семейство в данных модели
                var modelFamily = modelData.FirstOrDefault(f => f.FamilyName == familyName);

                if (modelFamily == null)
                {
                    // TaskDialog.Show("Предупреждение", $"Семейство '{familyName}' отсутствует в модели.");
                    resultData.Add(new FamilyData
                    {
                        FamilyName = familyName,
                        IsActual = false,
                        FUID = null // FUID = null, если семейство отсутствует в модели
                    });
                    continue;
                }

                // Сравниваем FUID из библиотеки и модели
                bool isActual = libraryFamily.FUID == modelFamily.FUID;

                // Добавляем данные в список
                resultData.Add(new FamilyData
                {
                    FUID = isActual ? libraryFamily.FUID : null, // FUID = null, если семейство неактуальное
                    FamilyName = familyName,
                    Types = GetFamilyTypes(document, family).Select(t => new FamilyType { TypeName = t.Name }).ToList(),
                    IsActual = isActual
                });
            }

            // Сохраняем обновленные данные
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            File.WriteAllText(_modelJsonPath, JsonSerializer.Serialize(resultData, options));

            return resultData;
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