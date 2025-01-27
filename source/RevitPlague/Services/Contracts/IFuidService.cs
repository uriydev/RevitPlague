using RevitPlague.Models;

namespace RevitPlague.Services.Contracts;

public interface IFuidService
{
    string GenerateFuid();
    void SaveOrUpdateFuid(string familyName, string fuid, IEnumerable<LibraryFamilyTypeData> types);
}