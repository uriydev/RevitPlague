using Autodesk.Revit.DB;
using RevitPlague.Services.Contracts;

namespace RevitPlague.Services;

public class FamilyParameterServiceFactory : IFamilyParameterServiceFactory
{
    public IFamilyParameterService Create(Document document)
    {
        return new FamilyParameterService(document); // Передаем Document в конструктор
    }
}