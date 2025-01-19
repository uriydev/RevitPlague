using Autodesk.Revit.DB;
using RevitPlague.Services.Contracts;

namespace RevitPlague.Services;

public class FamilyLoaderService : IFamilyLoaderService
{
    private readonly Document _document;

    public FamilyLoaderService(Document document)
    {
        _document = document;
    }

    public void LoadFamily(string familyPath)
    {
        using var transaction = new Transaction(_document, "Loading family");
        transaction.Start();
        _document.LoadFamily(familyPath, out _);
        transaction.Commit();
    }
}