using RevitPlague.Models;
using RevitPlague.Services.Contracts;

namespace RevitPlague.Services;

public class FuidService : IFuidService
{
    private readonly IFileService _fileService;
    private readonly AppSettings _appSettings;

    public FuidService(IFileService fileService, AppSettings appSettings)
    {
        _fileService = fileService;
        _appSettings = appSettings;
    }

    public string GenerateFuid()
    {
        return Guid.NewGuid().ToString();
    }

    public void SaveOrUpdateFuid(string familyName, string fuid, IEnumerable<LibraryFamilyTypeData> types)
    {
        var filePath = _appSettings.FamilyParametersFilePath;
        var existingData = _fileService.LoadFromFile<List<LibraryFamilyData>>(filePath) ?? new List<LibraryFamilyData>();

        var familyData = existingData.FirstOrDefault(f => f.FamilyName == familyName);
        if (familyData == null)
        {
            familyData = new LibraryFamilyData
            {
                FUID = fuid,
                FamilyName = familyName,
                Types = types.ToList()
            };
            existingData.Add(familyData);
        }
        else
        {
            familyData.FUID = fuid;
            familyData.Types = types.ToList();
        }

        _fileService.SaveToFile(filePath, existingData);
    }
}