using Autodesk.Revit.DB;
using RevitPlague.Models;
using RevitPlague.Services.Contracts;

namespace RevitPlague.Services;

public class FamilyParameterService : IFamilyParameterService
{
    private readonly Document _document;

    // Конструктор принимает Document
    public FamilyParameterService(Document document)
    {
        _document = document;
    }

    public IEnumerable<LibraryFamilyTypeData> GetFamilyTypes()
    {
        var familyManager = _document.FamilyManager;
        var types = new List<LibraryFamilyTypeData>();

        foreach (FamilyType type in familyManager.Types)
        {
            if (!string.IsNullOrEmpty(type.Name)) // Игнорируем пустые типы
            {
                types.Add(new LibraryFamilyTypeData
                {
                    TypeName = type.Name
                });
            }
        }

        return types;
    }
}