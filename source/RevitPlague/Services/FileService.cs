using System.IO;
using System.Text.Json;

namespace RevitPlague.Services;

public class FileService : IFileService
{
    public void SaveToFile(string filePath, object data)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory); // Создаем директорию, если её нет
        }
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        
        var json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, json);
    }

    public T LoadFromFile<T>(string filePath)
    {
        if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
        {
            // Если файл не существует или пуст, возвращаем значение по умолчанию
            return default;
        }
    
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }
}
