using Autodesk.Revit.DB;

namespace RevitPlague.Services.Contracts;

public interface IFamilyLoaderServiceFactory
{
    IFamilyLoaderService Create(Document document);
}