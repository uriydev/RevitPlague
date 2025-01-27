using Autodesk.Revit.DB;
using RevitPlague.Models;

namespace RevitPlague.Services.Contracts;

public interface IFamilyParameterService
{
    IEnumerable<LibraryFamilyTypeData> GetFamilyTypes();
}