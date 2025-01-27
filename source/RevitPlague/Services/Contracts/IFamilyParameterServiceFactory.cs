using Autodesk.Revit.DB;

namespace RevitPlague.Services.Contracts;

public interface IFamilyParameterServiceFactory
{
    IFamilyParameterService Create(Document document);
}