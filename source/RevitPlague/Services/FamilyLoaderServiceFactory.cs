using Autodesk.Revit.DB;
using RevitPlague.Services.Contracts;

namespace RevitPlague.Services;

public class FamilyLoaderServiceFactory : IFamilyLoaderServiceFactory
{
    public IFamilyLoaderService Create(Document document)
    {
        return new FamilyLoaderService(document);
    }
}