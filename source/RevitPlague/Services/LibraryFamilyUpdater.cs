using Autodesk.Revit.DB;
using RevitPlague.Services.Contracts;

namespace RevitPlague.Services;

public class LibraryFamilyUpdater
{
    private readonly IFuidService _fuidService;
    private readonly IFamilyParameterServiceFactory _parameterServiceFactory;

    public LibraryFamilyUpdater(
        IFuidService fuidService, 
        IFamilyParameterServiceFactory parameterServiceFactory)
    {
        _fuidService = fuidService;
        _parameterServiceFactory = parameterServiceFactory;
    }

    public void OnFamilySaved(Document document)
    {
        var parameterService = _parameterServiceFactory.Create(document);
        var familyName = document.Title;
        var types = parameterService.GetFamilyTypes();

        if (types.Any()) // Проверяем, есть ли типы
        {
            var fuid = _fuidService.GenerateFuid();
            _fuidService.SaveOrUpdateFuid(familyName, fuid, types);
        }
    }
}