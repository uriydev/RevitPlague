using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPlague.Models;

public class GetElementByIdService
{
    private UIDocument _uiDocument;

    public GetElementByIdService(
        UIDocument uiDocument)
    {
        _uiDocument = uiDocument;
    }

    public Element? GetElement(string id)
    {
        var elementId = new ElementId(
            int.Parse(id));

        var element = _uiDocument
            .Document
            .GetElement(elementId);

        return element;
    }
}