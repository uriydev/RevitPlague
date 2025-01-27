namespace RevitPlague.Services;

public interface IFileService
{
    void SaveToFile(string filePath, object data);
    T LoadFromFile<T>(string filePath);
}