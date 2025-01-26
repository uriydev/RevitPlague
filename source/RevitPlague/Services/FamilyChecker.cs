using System.IO;
using System.Text.Json;

namespace RevitPlague.Services;

public class FamilyChecker
{
    private readonly string modelJsonPath = @"C:\_bim\FamilyUpdaterTest\FamilyParameters_Model01.json";
    private readonly string libraryJsonPath = @"C:\_bim\FamilyUpdaterTest\Library_FamilyParameters.json";

    public void CheckFamilyParameters()
    {
        // Путь к файлу лога
        string logDirectory = @"C:\_bim\FamilyUpdaterTest\_logs";
        string logFilePath = Path.Combine(logDirectory, "log.txt");

        // Проверяем, существует ли директория для логов, если нет — создаём
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        // Создаём или открываем лог файл для записи
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            // Считываем данные из файлов
            var modelData = ReadJsonFile(modelJsonPath);
            var libraryData = ReadJsonFile(libraryJsonPath);

            // Сравнение данных
            var discrepancies = CompareFamilyParameters(modelData, libraryData);

            // Записываем результат в лог
            writer.WriteLine($"Проверка актуальности семейств ({DateTime.Now})");

            // Вывод результатов
            if (discrepancies.Any())
            {
                writer.WriteLine("Найдены несоответствия:");
                foreach (var discrepancy in discrepancies)
                {
                    writer.WriteLine($"Семейство: {discrepancy.FamilyName}, Тип: {discrepancy.TypeName}");
                    writer.WriteLine($"GUID в модели: {discrepancy.ModelGUID}");
                    writer.WriteLine($"Актуальный GUID: {discrepancy.LibraryGUID}");
                    writer.WriteLine("------------------------");
                }
            }
            else
            {
                writer.WriteLine("Все семейства актуальны.");
            }

            writer.WriteLine("============================================");
        }
    }
    
    private List<FamilyParameter> ReadJsonFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл {filePath} не найден.");

        string jsonContent = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<FamilyParameter>>(jsonContent) ?? new List<FamilyParameter>();
    }

    private List<Discrepancy> CompareFamilyParameters(List<FamilyParameter> modelData, List<FamilyParameter> libraryData)
    {
        var discrepancies = new List<Discrepancy>();

        foreach (var modelParam in modelData)
        {
            // Ищем соответствующий элемент в библиотеке
            var libraryParam = libraryData.FirstOrDefault(l =>
                l.FamilyName == modelParam.FamilyName &&
                l.TypeName == modelParam.TypeName);

            // Если элемент найден, проверяем GUID
            if (libraryParam != null && libraryParam.CustomGUID != modelParam.CustomGUID)
            {
                discrepancies.Add(new Discrepancy
                {
                    FamilyName = modelParam.FamilyName,
                    TypeName = modelParam.TypeName,
                    ModelGUID = modelParam.CustomGUID,
                    LibraryGUID = libraryParam.CustomGUID
                });
            }
        }

        return discrepancies;
    }

    // Класс для параметров семейства
    private class FamilyParameter
    {
        public string FamilyName { get; set; }
        public string TypeName { get; set; }
        public string CustomGUID { get; set; }
    }

    // Класс для несоответствий
    private class Discrepancy
    {
        public string FamilyName { get; set; }
        public string TypeName { get; set; }
        public string ModelGUID { get; set; }
        public string LibraryGUID { get; set; }
    }
}